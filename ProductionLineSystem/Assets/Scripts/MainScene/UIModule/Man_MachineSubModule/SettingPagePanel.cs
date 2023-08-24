using UGUI.Framework;
using UnityEngine.EventSystems;

namespace LabProductLine.UIModule
{

    public class SettingPagePanel : UIWindow
    {
        protected override void Start()
        {
            base.Start();
            GetUIEventListener("CloseBtn").PointClickHandler += OnSettingPagePanelClose;
        }

        private void OnSettingPagePanelClose(PointerEventData eventData)
        {
            UIManager.Instance.GetWindow<SettingPagePanel>()?.SetVisible(false, 0);
            UIManager.Instance.openingUIWindows.Remove(this);
        }
    }
}