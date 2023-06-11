using Common;
using UnityEngine;

namespace LabProductLine.ProductionLineSimulationModule
{

    public class BuildController : MonoSingleton<BuildController>
    {

        public LayerMask LayerMask;
        [HideInInspector]
        public GameObject placeObject;//需要实例化的物体预制体
        private bool isPlacingObject;//是否正在放置物体
        RaycastHit hitInfo;
        Ray ray;

        public override void Init()
        {
            base.Init();
        }


        private void Update()
        {
            if (isPlacingObject)
            {

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo, 50f, LayerMask))
                {
                    placeObject.transform.position = hitInfo.point;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    PlaceObject();
                }
            }

        }

        private void PlaceObject()
        {
            placeObject.transform.position = hitInfo.point;
            isPlacingObject = false;
        }

        //private bool IsInPlacementArea(GameObject obj)
        //{

        //}

        //开始放置物体
        public void StartPlacingObject(string key)
        {
            placeObject = GameObjectPool.Instance.CreateObject(key, ResourceManager.Load<GameObject>(key), Vector3.zero, Quaternion.identity);
            placeObject.SetActive(true);
            isPlacingObject = true;
        }

    }
}