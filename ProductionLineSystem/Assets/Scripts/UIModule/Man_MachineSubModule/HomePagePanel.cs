using LabProductLine.ProductionLineSimulationModule;
using System.Collections.Generic;
using UGUI.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LabProductLine.UIModule
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HomePagePanel : UIWindow
    {
        protected override void Start()
        {
            base.Start();
            GetUIEventListener("HomePageBtn").PointClickHandler += OnHomePagePanelClick;
            GetUIEventListener("SidePageBtn").PointClickHandler += OnSidePagePanelClick;
            GetUIEventListener("NotificationPageBtn").PointClickHandler += OnNotificationPagePanelClick;
            GetUIEventListener("SettingPageBtn").PointClickHandler += OnSettingPagePanelClick;
            GetUIEventListener("HelpPageBtn").PointClickHandler += OnHelpPagePanelClick;
        }

        private void OnSettingPagePanelClick(PointerEventData eventData)
        {
            SettingPagePanel panel = UIManager.Instance.GetWindow<SettingPagePanel>();
            if (panel == null) return;
            bool curState = panel.GetComponent<CanvasGroup>().alpha == 1 ? true : false;
            panel.SetVisible(!curState, 0);
            panel.GetComponent<RectTransform>().SetAsLastSibling();
            if (!curState)//!curState相当于是当前的状态了
                UIManager.Instance.openingUIWindows.Add(panel);
            else
                UIManager.Instance.openingUIWindows.Remove(panel);
        }

        private void OnNotificationPagePanelClick(PointerEventData eventData)
        {
            NotificationPagePanel panel = UIManager.Instance.GetWindow<NotificationPagePanel>();
            if (panel == null) return;
            bool curState = panel.GetComponent<CanvasGroup>().alpha == 1 ? true : false;
            panel.SetVisible(!curState, 0);
            panel.GetComponent<RectTransform>().SetAsLastSibling();
            if (!curState)
                UIManager.Instance.openingUIWindows.Add(panel);
            else
                UIManager.Instance.openingUIWindows.Remove(panel);
        }

        private void OnSidePagePanelClick(PointerEventData eventData)
        {
            SideNavigationPanel panel = UIManager.Instance.GetWindow<SideNavigationPanel>();
            if (panel == null) return;
            bool curState = panel.GetComponent<CanvasGroup>().alpha == 1 ? true : false;
            //可添加一些动画效果
            panel.SetVisible(!curState, 0);
            panel.GetComponent<RectTransform>().SetAsLastSibling();
            if (!curState)
                UIManager.Instance.openingUIWindows.Add(panel);
            else
                UIManager.Instance.openingUIWindows.Remove(panel);
        }

        private void OnHomePagePanelClick(PointerEventData eventData)
        {
            //播放动画、
            //关闭其他开启的界面
            List<UIWindow> openingUIWindows = UIManager.Instance.openingUIWindows;
            for (int i = 0; i < openingUIWindows.Count; i++)
            {
                openingUIWindows[i].SetVisible(false);
            }
            GetComponent<Image>().enabled = true;
            Camera.main.GetComponent<FreeRoamController>().ResetCamPos();
            Camera.main.GetComponent<FreeRoamController>().enabled = false;

            //UIManager.Instance.openingUIWindow?.SetVisible(false, 0);
        }

        private void OnHelpPagePanelClick(PointerEventData eventData)
        {
            HelpPagePanel panel = UIManager.Instance.GetWindow<HelpPagePanel>();
            if (panel == null) return;
            bool curState = panel.GetComponent<CanvasGroup>().alpha == 1 ? true : false;
            panel.SetVisible(!curState, 0);
            panel.GetComponent<RectTransform>().SetAsLastSibling();
            if (!curState)
                UIManager.Instance.openingUIWindows.Add(panel);
            else
                UIManager.Instance.openingUIWindows.Remove(panel);
        }
    }
}