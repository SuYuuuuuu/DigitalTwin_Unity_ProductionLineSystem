using Common;
using LabProductLine.DataManagerModule;
using System.Collections.Generic;
using TMPro;
using UGUI.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LabProductLine.UIModule
{

    public class EquipmentPagePanel : UIWindow
    {
        public RectTransform content;//这个为显示的内容区域
        public Transform[] Pages;//存储内容页
        public RobotInfo robotInfo;//自定义数据类型，包含大部分信息，可在面板手动配置
        private Dictionary<GameObject, int> robotDic; //存储每个内容对应的机械臂序号，方便查找信息
        private GameObject robotPagePrefab;
        private Button NextBtn;
        private Button LastBtn;
        private GameObject curRobotObj;
        private List<RobotData> registeredRobotDataList;
        private int curPageIndex = 0;//当前页码



        protected override void Start()
        {
            base.Start();
            robotDic = new Dictionary<GameObject, int>();
            registeredRobotDataList = new List<RobotData>();
            robotPagePrefab = ResourceManager.Load<GameObject>("RobotEx");//加载预制件
            for (int i = 0; i < ToolDataManager.Instance.GetRobotDataNumber(); i++)
            {
                GameObject go = Instantiate(robotPagePrefab);
                go.transform.SetParent(content);//实例化预制件并设置其父物体
                go.GetComponent<UIEventListener>().PointClickHandler += OnRobotInfoPagePanelOpened;//这里没有取消订阅
                robotDic.Add(go, i);//因为机械臂序号是从0开始按顺序排列，所以可以直接添加
                //这里应该要动态修改content的高度保证容纳所有子物体
            }
            GetUIEventListener("NextBtn").PointClickHandler += OnNextPageOpened;
            GetUIEventListener("LastBtn").PointClickHandler += OnLastPageOpened;
            GetUIEventListener("CloseBtn").PointClickHandler += OnEquipmentPagePanelClosed;

            NextBtn = GetUIEventListener("NextBtn").GetComponent<Button>();
            LastBtn = GetUIEventListener("LastBtn").GetComponent<Button>();
            UpdateBtnState();
        }


        //private void OnParamsPanelOpened(PointerEventData eventData)
        //{
        //    UIManager.Instance.GetWindow<ParamsPanel>().SetVisible(true);
        //}

        public void OnEquipmentPagePanelOpened()
        {
            if (UIManager.Instance.openingUIWindows.Contains(this)) return;
            foreach (GameObject key in robotDic.Keys)
            {
                RobotData data = ToolDataManager.Instance.GetRobotDataByID(robotDic[key]);
                if (data.OperationStatus == RobotOperationStatus.waiting) continue; //直接跳过未连接的机械臂
                key.transform.Find("Name").GetComponent<TMP_Text>().text = data.RobotName;
                key.transform.Find("WorkState").GetComponent<TMP_Text>().text = data.OperationStatus == RobotOperationStatus.working ? "工作中" : "已断开";
                key.transform.Find("WorkHour").GetComponent<TMP_Text>().text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(Time.time / 60f), Mathf.FloorToInt(Time.time % 60f));
            }
            // RegisterParamsBtn();//注册一波按钮事件
            UIManager.Instance.GetWindow<RobotInfoPanel>().SetVisible(false);//打开时要把信息面板隐藏
            UIManager.Instance.openingUIWindows.Add(this);//将当前面板加入到开启队列中
        }

        private void OnEquipmentPagePanelClosed(PointerEventData eventData)
        {
            SetVisible(false);
            for (int i = 0; i < registeredRobotDataList.Count; i++)
            {
                RobotData data = registeredRobotDataList[i];
                UnregisterRobotDataEvent(data);
            }
            registeredRobotDataList.Clear();//清空已订阅的机械臂数据
            UIManager.Instance.GetWindow<SideNavigationPanel>().PagePanelOpened -= OnEquipmentPagePanelOpened;//这里关闭负责取消订阅
            UIManager.Instance.openingUIWindows.Remove(this);//将当前面板加入到开启队列中
            //UnregisterParamsBtn();
        }


        private void OnRobotInfoPagePanelOpened(PointerEventData eventData)
        {
            GameObject obj = eventData.pointerClick;
            UIManager.Instance.GetWindow<RobotInfoPanel>().SetVisible(true);
            if (!robotDic.ContainsKey(obj) || curRobotObj == obj) return;
            RobotData data = ToolDataManager.Instance.GetRobotDataByID(robotDic[obj]);
            UpdateRobotInfo(data);//更新机械臂数据
            RegisterRobotDataEvent(data);
            registeredRobotDataList.Add(data);
            if (curRobotObj != null)
            {
                RobotData lastData = ToolDataManager.Instance.GetRobotDataByID(robotDic[curRobotObj]);
                UnregisterRobotDataEvent(lastData);//注销上一个机械臂事件
                registeredRobotDataList.Remove(lastData);//从已注册列表中移除
            }
            curRobotObj = obj;//赋值当前物体
        }

        //负责注册属性变化通知事件
        private void RegisterRobotDataEvent(RobotData data)
        {
            data.IdDataChanged += OnRobotIdDataChanged;
            data.NameDataChanged += OnRobotNameDataChanged;
            data.EndEffectorPosDataChanged += OnEndEffectorPosDataChanged;
            data.JointAnglesDataChanged += OnJointAnglesDataChanged;
            data.HomeParamsDataChanged += OnHomeParamsDataChanged;
            data.EndEffectorSuctionCupDataChanged += OnEndEffectorSuctionCupDataChanged;
            data.RobotOperationStatusDataChanged += OnRobotOperationStatusDataChanged;
            data.JogJointParamsDataChanged += OnJogJointParamsDataChanged;
            data.JogCoordinateDataChanged += OnJogCoordinateDataChanged;
            data.JogCommonDataChanged += OnJogCommonDataChanged;
            data.PTPJointParamsDataChanged += OnPTPJointParamsDataChanged;
            data.PTPCoordinateParamsDataChanged += OnPTPCoordinateParamsDataChanged;
            data.PTPCommonParamsDataChanged += OnPTPCommonParamsDataChanged;
        }

        //负责注销属性变化通知事件
        private void UnregisterRobotDataEvent(RobotData data)
        {
            data.IdDataChanged -= OnRobotIdDataChanged;
            data.NameDataChanged -= OnRobotNameDataChanged;
            data.EndEffectorPosDataChanged -= OnEndEffectorPosDataChanged;
            data.JointAnglesDataChanged -= OnJointAnglesDataChanged;
            data.HomeParamsDataChanged -= OnHomeParamsDataChanged;
            data.EndEffectorSuctionCupDataChanged -= OnEndEffectorSuctionCupDataChanged;
            data.RobotOperationStatusDataChanged -= OnRobotOperationStatusDataChanged;
            data.JogJointParamsDataChanged -= OnJogJointParamsDataChanged;
            data.JogCoordinateDataChanged -= OnJogCoordinateDataChanged;
            data.JogCommonDataChanged -= OnJogCommonDataChanged;
            data.PTPJointParamsDataChanged -= OnPTPJointParamsDataChanged;
            data.PTPCoordinateParamsDataChanged -= OnPTPCoordinateParamsDataChanged;
            data.PTPCommonParamsDataChanged -= OnPTPCommonParamsDataChanged;
        }

        //更新机械臂信息面板内容
        private void UpdateRobotInfo(RobotData data)
        {

            OnRobotIdDataChanged(-1, data.RobotID);
            OnRobotNameDataChanged(-1, data.RobotName);
            robotInfo.robotAlarmState.text = "无";
            OnJointAnglesDataChanged(-1, data.JointAngles);
            OnEndEffectorPosDataChanged(-1, data.EndEffectorPos);
            OnHomeParamsDataChanged(-1, data.HomeParams);
            OnEndEffectorSuctionCupDataChanged(-1, data.EndEffectorSuctionCup);
            OnRobotOperationStatusDataChanged(-1, data.OperationStatus);
            OnJogJointParamsDataChanged(-1, data.JogJointParams);
            OnJogCoordinateDataChanged(-1, data.JogCoordinateParams);
            OnJogCommonDataChanged(-1, data.JogCommonParams);
            OnPTPJointParamsDataChanged(-1, data.PTPJointParams);
            OnPTPCoordinateParamsDataChanged(-1, data.PTPCoordinateParams);
            OnPTPCommonParamsDataChanged(-1, data.PTPCommonParams);

        }


        private void OnLastPageOpened(PointerEventData eventData)
        {
            curPageIndex -= 1;
            UpdateBtnState();
            UpdatePage();
        }

        private void OnNextPageOpened(PointerEventData eventData)
        {
            curPageIndex += 1;
            UpdateBtnState();
            UpdatePage();
        }

        ///根据当前页码更新上下按钮的点击状态
        private void UpdateBtnState()
        {
            Image lastBtnImage = LastBtn.GetComponent<Image>();
            Image nextBtnImage = NextBtn.GetComponent<Image>();
            if (curPageIndex == 0) //说明在第一页
            {
                LastBtn.interactable = false;
                lastBtnImage.raycastTarget = false;
                NextBtn.interactable = true;
                nextBtnImage.raycastTarget = true;
            }
            else if (curPageIndex == Pages.Length - 1) //说明在最后一页
            {
                NextBtn.interactable = false;
                nextBtnImage.raycastTarget = false;
                LastBtn.interactable = true;
                lastBtnImage.raycastTarget = true;
            }
            else //说明在两者之间
            {
                NextBtn.interactable = true;
                nextBtnImage.raycastTarget = true;
                LastBtn.interactable = true;
                lastBtnImage.raycastTarget = true;
            }
        }

        ///更新当前页码内容
        private void UpdatePage()
        {
            for (int i = 0; i < Pages.Length; i++)
            {
                if (i == curPageIndex)
                    Pages[i].GetComponent<CanvasGroup>().alpha = 1;
                else
                    Pages[i].GetComponent<CanvasGroup>().alpha = 0;
            }
        }


        private void OnRobotIdDataChanged(int id, int value)
        {
            robotInfo.robotID.text = value.ToString();
        }

        private void OnRobotNameDataChanged(int id, string value)
        {
            robotInfo.robotName.text = value;
        }

        private void OnJointAnglesDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotPos_JointAngles.text = "xxx";
            else
                robotInfo.robotPos_JointAngles.text = string.Format("关节1角度:{0}\n关节2角度:{1}\n关节3角度:{2}\n关节4角度:{3}",
               value[0], value[1], value[2], value[3]);
        }

        private void OnEndEffectorPosDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotPos_EndPos.text = "xxx";
            else
                robotInfo.robotPos_EndPos.text = string.Format("x:{0},y:{1}\nz:{2},r:{3}",
                value[0], value[1], value[2], value[3]);
        }
        private void OnHomeParamsDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotHomeParams.text = "xxx";
            else
                robotInfo.robotHomeParams.text = string.Format("x:{0},y:{1}\nz:{2},r:{3}", value[0], value[1], value[2], value[3]);
        }

        private void OnEndEffectorSuctionCupDataChanged(int id, bool[] value)
        {
            if (value == null) robotInfo.robotSuctionCupState.text = "xxx";
            else
                robotInfo.robotSuctionCupState.text = value[1] ? "开启" : "关闭";
        }
        private void OnRobotOperationStatusDataChanged(int id, RobotOperationStatus value)
        {
            robotInfo.robotWorkState.text = value == RobotOperationStatus.working ? "工作中" : "等待连接";
        }

        private void OnJogJointParamsDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotJogJointParams.text = "xxx";
            else
                robotInfo.robotJogJointParams.text = string.Format("速度:\n轴1:{0}mm/s,轴2:{1}mm/s\n轴3:{2}mm/s,轴4:{3}mm/s\n加速度:\n轴1:{4}mm/s,轴2:{5}mm/s\n轴3:{6}mm/s,轴4:{7}mm/s",
                value[0], value[1], value[2], value[3], value[4], value[5], value[6], value[7]);
        }
        private void OnJogCoordinateDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotJogCooParams.text = "xxx";
            else
                robotInfo.robotJogCooParams.text = string.Format("速度:\n轴1:{0}mm/s,轴2:{1}mm/s\n轴3:{2}mm/s,轴4:{3}mm/s\n加速度:\n轴1:{4}mm/s,轴2:{5}mm/s\n轴3:{6}mm/s,轴4:{7}mm/s",
                value[0], value[1], value[2], value[3], value[4], value[5], value[6], value[7]);
        }
        private void OnJogCommonDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotJogCommonParams.text = "xxx";
            else
                robotInfo.robotJogCommonParams.text = string.Format("速度比例:{0}mm/s,加速度比例:{1}mm/s", value[0], value[1]);
        }

        private void OnPTPJointParamsDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotPtpJointParams.text = "xxx";
            else
                robotInfo.robotPtpJointParams.text = string.Format("速度:\n轴1:{0}mm/s,轴2:{1}mm/s\n轴3:{2}mm/s,轴4:{3}mm/s\n加速度:\n轴1:{4}mm/s,轴2:{5}mm/s\n轴3:{6}mm/s,轴4:{7}mm/s",
               value[0], value[1], value[2], value[3], value[4], value[5], value[6], value[7]);
        }
        private void OnPTPCoordinateParamsDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotPtpCooParams.text = "xxx";
            else
                robotInfo.robotPtpCooParams.text = string.Format("xyz轴坐标轴速度:{0}mm/s,末端速度:{1}mm/s\nxyz轴坐标轴加速度:{2}mm/s,末端加速度:{3}mm/s",
                value[0], value[1], value[2], value[3]);
        }
        private void OnPTPCommonParamsDataChanged(int id, float[] value)
        {
            if (value == null) robotInfo.robotPtpCommonParams.text = "xxx";
            else
                robotInfo.robotPtpCommonParams.text = string.Format("速度比例:{0}mm/s,加速度比例:{1}mm/s", value[0], value[1]);
        }
    }
}