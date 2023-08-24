using UGUI.Framework;
using UnityEngine.EventSystems;

namespace LabProductLine.UIModule
{

    public class ParamsPanel : UIWindow
    {
        protected override void Start()
        {
            base.Start();
            GetUIEventListener("CloseBtn").PointClickHandler += OnParamsPanelClosed;
        }

        private void OnParamsPanelClosed(PointerEventData eventData)
        {
            SetVisible(false);
        }
    }
}