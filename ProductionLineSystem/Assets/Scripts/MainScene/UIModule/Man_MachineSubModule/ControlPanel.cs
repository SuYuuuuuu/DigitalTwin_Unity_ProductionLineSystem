using System;
using LabProductLine.ControlModule;
using LabProductLine.DataManagerModule;
using TMPro;
using UGUI.Framework;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace LabProductLine.UIModule
{

    public class ControlPanel : UIWindow
    {
        public Slider[] sliders;
        public RobotInfo robotInfo;//存储Text的引用
        private RobotOperationStatus state; //机械臂操作状态
        private RobotData curRobotdata;//当前选中的机械臂data
        private Robot_MoveSystem moveSystem;
        protected override void Start()
        {
            base.Start();
            moveSystem = GetComponent<Robot_MoveSystem>();
            moveSystem.enabled = false;
            UIManager.Instance.GetWindow<ControlPagePanel>().onControlPanelOpen += ControlPanel_onControlPanelOpen;//订阅事件面板打开事件
            GetUIEventListener("CloseBtn").PointClickHandler += OnControlPanelClosed;
            foreach (Slider item in sliders)
            {
                TMP_Text text = item.transform.Find("Value").GetComponent<TMP_Text>();
                item.onValueChanged.AddListener((value) => OnSlidersValueChanged(value, item, text));
            }
        }

        private void ControlPanel_onControlPanelOpen(PointerEventData obj)
        {
            //实现对应信息显示
            RobotData data = obj.pointerClick.GetComponent<RobotControlEx>().robotData;
            if( data.ID==0 || data.OperationStatus==RobotOperationStatus.waiting) return; //id为0代表该面板为空,或者出于未连接状态
            UpdateRobotControlPanel(data.ID);
            data.JointAnglesDataChanged += OnJointAnglesDataChanged;
            data.EndEffectorPosDataChanged += OnEndEffectorPosDataChanged;
            curRobotdata = data;
            //启用机械臂键盘控制脚本,并传输当前机械臂ID
            moveSystem.enabled = true;
            moveSystem.RobotID = data.ID;
        }

        private void OnEndEffectorPosDataChanged(int Id, float[] value)
        {
            robotInfo.robotPos_EndPos.text = string.Format("X:{0}mm\nY:{1}mm\nZ:{2}mm\nR:{3}mm",value[0],value[1],value[2],value[3]);
        }

        private void OnJointAnglesDataChanged(int Id, float[] value)
        {
            robotInfo.robotPos_JointAngles.text = string.Format("J1:{0}°\nJ2:{1}°\nJ3:{2}°\nJ4:{3}°",value[0],value[1],value[2],value[3]);
        }

        private void OnSlidersValueChanged(float value, Slider item, TMP_Text text)
        {
            switch (item.name)
            {
                case "JogJointParams":
                case "JogCooParams":
                case "JogCommonParams":
                    text.text = string.Format("{0}°/s", value);
                    break;
                case "PTPJointParams":
                case "PTPCooParams":
                case "PTPCommonParams":
                    text.text = string.Format("{0}%", value);
                    break;
            }
        }

        private void OnControlPanelClosed(PointerEventData eventData)
        {
            this.SetVisible(false);
            if(curRobotdata==null) return;
            curRobotdata.JointAnglesDataChanged -= OnJointAnglesDataChanged;
            curRobotdata.EndEffectorPosDataChanged -= OnEndEffectorPosDataChanged;
            curRobotdata=null;//最后记得赋值为null
            moveSystem.enabled = false;//关闭机械臂移动脚本
        }

        private void UpdateRobotControlPanel(int id)
        {
            RobotData data = DataManager.Instance.GetDataById<RobotData>(id);
            OnJointAnglesDataChanged(id,data.JointAngles);
            OnEndEffectorPosDataChanged(id,data.EndEffectorPos);
            for (int i = 0; i < sliders.Length; i++)
            {
                switch(sliders[i].name)
                {
                    case "JogJointParams":
                        sliders[i].value = data.JogJointParams[0];//默认所有关节速度一致
                        break;
                    case "JogCooParams":
                        sliders[i].value = data.JogCoordinateParams[0];//默认所有关节速度一致
                        break;
                    case "JogCommonParams":
                        sliders[i].value = data.JogCommonParams[0];//默认所有关节速度一致
                        break;
                    case "PTPJointParams":
                        sliders[i].value = data.PTPJointParams[0];//默认所有关节速度一致
                        break;
                    case "PTPCooParams":
                        sliders[i].value = data.PTPCoordinateParams[0];//默认所有关节速度一致
                        break;
                    case "PTPCommonParams":
                        sliders[i].value = data.PTPCommonParams[0];//默认所有关节速度一致
                        break;
                }
            }
        }



        /// <summary>
        /// 状态改变通知服务端
        /// </summary>
        private void ChangeParamsToServer()
        {
            if (!Client.Instance.Socket.Connected || state == RobotOperationStatus.waiting) return;

        }
    }
}