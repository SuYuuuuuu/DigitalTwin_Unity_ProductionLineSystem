using LabProductLine.ControlModule;
using UGUI.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LabProductLine.UIModule
{

    public class NotificationPagePanel : UIWindow
    {
        public Transform content;
        protected override void Start()
        {
            base.Start();
            GetUIEventListener("CloseBtn").PointClickHandler += OnNotificationPagePanelClose;
            GetUIEventListener("OpenWebSocketBtn").PointClickHandler += OnWebSocketOpen;
            GetUIEventListener("CloseWebSocketBtn ").PointClickHandler += OnWebSocketClose;
        }

        private void OnWebSocketClose(PointerEventData eventData)
        {
            WebSocketClient.Instance.Close();
        }

        private void OnWebSocketOpen(PointerEventData eventData)
        {
            WebSocketClient.Instance.Init("ws://192.168.8.168:9000/");
            WebSocketClient.Instance.Connect();
            //设置按钮不可用直到断开连接
        }

        private void OnNotificationPagePanelClose(PointerEventData eventData)
        {
            UIManager.Instance.GetWindow<NotificationPagePanel>()?.SetVisible(false);
            UIManager.Instance.openingUIWindows.Remove(this);
        }
    }
}