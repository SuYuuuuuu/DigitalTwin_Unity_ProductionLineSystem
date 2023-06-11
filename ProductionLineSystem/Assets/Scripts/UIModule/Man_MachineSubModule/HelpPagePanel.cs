using UGUI.Framework;
using UnityEngine.EventSystems;

namespace LabProductLine.UIModule
{

    public class HelpPagePanel : UIWindow
    {
        protected override void Start()
        {
            base.Start();
            GetUIEventListener("CloseBtn").PointClickHandler += OnHelpPagePanelClose;
        }

        private void OnHelpPagePanelClose(PointerEventData eventData)
        {
            SetVisible(false, 0);
            UIManager.Instance.openingUIWindows.Remove(this);
        }
    }
}