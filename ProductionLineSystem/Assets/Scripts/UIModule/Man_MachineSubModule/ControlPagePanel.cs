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

        [HideInInspector]
        public List<UIEventListener> robotExes;//用于存储已有的机械臂面板的UI监听器
        public Transform content;
        public List<RobotInfo> robotInfos;
        public event Action<PointerEventData> onControlPanelOpen;
        private CanvasGroup backGroundGroup;
        protected override void Start()
        {
            base.Start();
            robotExes = new List<UIEventListener>();//初始化
            backGroundGroup = GetUIEventListener("CloseBtn").transform.GetChild(0).GetComponent<CanvasGroup>();
            GetUIEventListener("CloseBtn").PointEnterHandler += OnBackGroundShowed;
            GetUIEventListener("CloseBtn").PointClickHandler += OnControlPagePanelClosed;
            GetUIEventListener("CloseBtn").PointExitHandler += OnBackGroundHided;
            for (int i = 0; i < content.childCount; i++)
            {
                robotExes.Add(content.GetChild(i).GetComponent<UIEventListener>());
            }
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
            UnRegisterRobotPanelShowed();
        }



        //用于注册机械臂面板的委托，在当前面板被打开时调用
        public void RegisterControlPanelOpened()
        {
            for (int i = 0; i < robotExes.Count; i++)
            {
                robotExes[i].PointClickHandler += OnControlPanelOpened;
            }
        }

        //用于注销机械臂面板的委托，在当前面板被关闭时调用
        public void UnRegisterControlPanelOpened()
        {
            for (int i = 0; i < robotExes.Count; i++)
            {
                robotExes[i].PointClickHandler -= OnControlPanelOpened;
            }
        }

        //用于注册当前机械臂面板信息委托(只包含名字、id、连接状态),在当前面板被打开时调用
        public void RegisterRobotPanelShowed()
        {
            //int num = ToolDataManager.Instance.GetRobotDataNumber();         
            for (int i = 0; i < robotExes.Count; i++)
            {
                RobotControlEx ex = robotExes[i].GetComponent<RobotControlEx>();
                int id = ex.robotData.RobotID;
                ToolDataManager.Instance.GetRobotDataByID(id).NameDataChanged += ex.OnNameChanged;
                ToolDataManager.Instance.GetRobotDataByID(id).IdDataChanged += ex.OnIdChanged;
                ToolDataManager.Instance.GetRobotDataByID(id).RobotOperationStatusDataChanged += ex.OnStateChanged;
            }
        }

        //用于注销当前机械臂面板信息委托(只包含名字、id、连接状态)，在当前面板被关闭时调用
        public void UnRegisterRobotPanelShowed()
        {
            //int num = ToolDataManager.Instance.GetRobotDataNumber();
            for (int i = 0; i < robotExes.Count; i++)
            {
                RobotControlEx ex = robotExes[i].GetComponent<RobotControlEx>();
                int id = ex.robotData.RobotID;
                ToolDataManager.Instance.GetRobotDataByID(id).NameDataChanged -= ex.OnNameChanged;
                ToolDataManager.Instance.GetRobotDataByID(id).IdDataChanged -= ex.OnIdChanged;
                ToolDataManager.Instance.GetRobotDataByID(id).RobotOperationStatusDataChanged -= ex.OnStateChanged;
            }
        }


        
    }
}