using Common;
using LabProductLine.ControlModule;
using LabProductLine.DataManagerModule;
using System;
using System.Collections.Generic;
using UGUI.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LabProductLine.UIModule
{

    public class ControlPagePanel : UIWindow
    {

        public Transform content;//存放内容的区域的TF
        public event Action<PointerEventData> onControlPanelOpen;//具体信息面板打开事件
        private Dictionary<GameObject,int> robotControlEx2Id_Dic;//存放储存的面板-机械臂Id键值对的字典
        private GameObject robotcontrolExPrefab;//面板预制件
        private List<GameObject> unusedObjs;//未使用的面板列表
        private List<int> existingRobot;//存放了已加入的机械臂ID列表，同样和robotControlEx2Id_Dic、usedObjs一起使用
        private CanvasGroup backGroundGroup;//背景图片的group

        protected override void Start()
        {
            base.Start();
            unusedObjs = new List<GameObject>();
            existingRobot = new List<int>();
            robotControlEx2Id_Dic = new Dictionary<GameObject, int>();
            robotcontrolExPrefab = ResourceManager.Load<GameObject>("RobotControlEx");
            backGroundGroup = GetUIEventListener("CloseBtn").transform.GetChild(0).GetComponent<CanvasGroup>();
            Client.Instance.RobotConnected += OnRobotConnected;//订阅机械臂连接通知事件
            GetUIEventListener("CloseBtn").PointEnterHandler += OnBackGroundShowed;
            GetUIEventListener("CloseBtn").PointClickHandler += OnControlPagePanelClosed;
            GetUIEventListener("CloseBtn").PointExitHandler += OnBackGroundHided;
            for (int i = 0; i < content.childCount; i++)
            {
                GameObject go = content.GetChild(i).gameObject;
                //robotExes.Add(go.GetComponent<RobotControlEx>());
                unusedObjs.Add(go);
            }
        }



        private void OnRobotConnected(int ID)
        {
            if(existingRobot.Contains(ID)) return;
            if(unusedObjs.Count==0)
            {
                GameObject go = Instantiate(robotcontrolExPrefab);
                go.transform.SetParent(content);//实例化预制件并设置其父物体
                go.GetComponent<UIEventListener>().PointClickHandler += OnControlPanelOpened;//这里没有取消订阅
                existingRobot.Add(ID);
                robotControlEx2Id_Dic.Add(go,ID);
                RegisterRobotPanelShowed(ID,go.GetComponent<RobotControlEx>());//订阅机械臂信息事件

            }
            else
            {
                existingRobot.Add(ID);
                robotControlEx2Id_Dic.Add(unusedObjs[0],ID);
                RegisterRobotPanelShowed(ID,unusedObjs[0].GetComponent<RobotControlEx>());//订阅
                unusedObjs.RemoveAt(0);//将第一个元素移除

            }
            UpdateRobotControlPanel();//更新面板内容
        }

        private void OnControlPanelOpened(PointerEventData obj)
        {
            UIManager.Instance.GetWindow<ControlPanel>().SetVisible(true);
            onControlPanelOpen.Invoke(obj);
        }

        private void OnBackGroundHided(PointerEventData eventData)
        {
            backGroundGroup.alpha = 0f;
        }

        private void OnBackGroundShowed(PointerEventData eventData)
        {
            backGroundGroup.alpha = 0.15f;
        }

        private void OnControlPagePanelClosed(PointerEventData eventData)
        {
            this.SetVisible(false);
            UnRegisterControlPanelOpened();
            //UnRegisterRobotPanelShowed();
        }


        ///更新面板内容
        public void UpdateRobotControlPanel()
        {
            foreach (GameObject item in robotControlEx2Id_Dic.Keys)
            {
                RobotControlEx ex = item.GetComponent<RobotControlEx>();
                RobotData data = DataManager.Instance.GetDataById<RobotData>(robotControlEx2Id_Dic[item]);
                ex.OnNameChanged(data.ID,data.RobotName);
                ex.OnIdChanged(data.ID,data.ID);
                ex.OnStateChanged(data.ID,data.OperationStatus);
            }

        }

        //用于注册机械臂面板的委托，在当前面板被打开时调用
        public void RegisterControlPanelOpened()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).GetComponent<UIEventListener>().PointClickHandler += OnControlPanelOpened;
            }
        }

        //用于注销机械臂面板的委托，在当前面板被关闭时调用
        public void UnRegisterControlPanelOpened()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).GetComponent<UIEventListener>().PointClickHandler -= OnControlPanelOpened;
            }
        }

        //用于注册当前已存在的机械臂面板信息委托(只包含名字、id、连接状态),在当前面板被打开时调用
        public void RegisterRobotPanelShowed(int ID,RobotControlEx ex)
        {
            DataManager.Instance.GetDataById<RobotData>(ID).NameDataChanged += ex.OnNameChanged;
            DataManager.Instance.GetDataById<RobotData>(ID).IdDataChanged += ex.OnIdChanged;
            DataManager.Instance.GetDataById<RobotData>(ID).RobotOperationStatusDataChanged += ex.OnStateChanged;
        }

        //用于注销当前已存在的机械臂面板信息委托(只包含名字、id、连接状态)，在当前面板被关闭时调用
        public void UnRegisterRobotPanelShowed(int ID,RobotControlEx ex)
        {
            DataManager.Instance.GetDataById<RobotData>(ID).NameDataChanged -= ex.OnNameChanged;
            DataManager.Instance.GetDataById<RobotData>(ID).IdDataChanged -= ex.OnIdChanged;
            DataManager.Instance.GetDataById<RobotData>(ID).RobotOperationStatusDataChanged -= ex.OnStateChanged;
        }



    }
}