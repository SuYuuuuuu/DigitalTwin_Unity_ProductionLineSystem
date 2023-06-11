using UGUI.Framework;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace LabProductLine.UIModule.ProductionLineSimluationModule
{

    public class MainPanel : UIWindow
    {
        protected override void Start()
        {
            base.Start();
            GetUIEventListener("ReturnBtn").PointClickHandler += OnMainSceneReturned;
        }

        private void OnMainSceneReturned(PointerEventData eventData)
        {
            SceneManager.LoadScene(1);
        }
    }
}