using LabProductLine.ProductionLineSimulationModule;
using UGUI.Framework;
using UnityEngine.EventSystems;

namespace LabProductLine.UIModule.ProductionLineSimluationModule
{

    public class BuildPanel : UIWindow
    {

        protected override void Start()
        {
            base.Start();
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<UIEventListener>().PointClickHandler += OnGameObjectBuilded;
            }

        }

        private void OnGameObjectBuilded(PointerEventData eventData)
        {
            BuildController.Instance.StartPlacingObject(eventData.pointerClick.name);
        }
    }
}