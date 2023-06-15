using LabProductLine.ProductionLineSimulationModule;
using System;
using UGUI.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LabProductLine.UIModule
{

    public class SideNavigationPanel : UIWindow
    {
        public event Action PagePanelOpened;
        protected override void Start()
        {
            base.Start();
            GetUIEventListener("OverviewPageBtn").PointClickHandler += OnOverviewPageClick;
            GetUIEventListener("EquipmentPageBtn").PointClickHandler += OnEquipmentPageClick;
            GetUIEventListener("ControlPageBtn").PointClickHandler += OnControlPageClick;
            GetUIEventListener("FaultReportPageBtn").PointClickHandler += OnFaultReportPageClick;
            GetUIEventListener("RealTimeMonitorPageBtn").PointClickHandler += OnRealTimeMonitorPageClick;
            GetUIEventListener("StatisticalAnalysisPageBtn").PointClickHandler += OnStatisticalAnalysisPageClick;
            GetUIEventListener("ProductionLineSimulationPageBtn").PointClickHandler += OnProductionLineSimulationPageClick;
        }

        private void OnProductionLineSimulationPageClick(PointerEventData eventData)
        {
            SceneManager.LoadScene(2);
            for (int i = UIManager.instance.openingUIWindows.Count-1; i >=0; i--)
            {
                UIManager.instance.openingUIWindows[i].SetVisible(false);
                UIManager.instance.openingUIWindows.RemoveAt(i);
            }
        }

        private void OnStatisticalAnalysisPageClick(PointerEventData eventData)
        {

        }

        private void OnRealTimeMonitorPageClick(PointerEventData eventData)
        {

        }

        private void OnFaultReportPageClick(PointerEventData eventData)
        {

        }

        private void OnControlPageClick(PointerEventData eventData)
        {
            UIManager.Instance.GetWindow<ControlPagePanel>().SetVisible(true);
            UIManager.Instance.GetWindow<ControlPagePanel>().RegisterControlPanelOpened();//注册事件
            //UIManager.Instance.GetWindow<ControlPagePanel>().UpdateRobotControlPanel();//更新面板内容
        }

        private void OnEquipmentPageClick(PointerEventData eventData)
        {
            UIManager.Instance.GetWindow<EquipmentPagePanel>().SetVisible(true);
            PagePanelOpened += UIManager.Instance.GetWindow<EquipmentPagePanel>().OnEquipmentPagePanelOpened;
            PagePanelOpened?.Invoke();
        }

        private void OnOverviewPageClick(PointerEventData eventData)
        {
            UIManager.Instance.GetWindow<HomePagePanel>().GetComponent<Image>().enabled = false;
            //做动画
            Camera.main.GetComponent<FreeRoamController>().enabled = true;
        }
    }
}