using LabProductLine.DataManagerModule;
using UnityEngine;

namespace LabProductLine.ConveyorModule
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveSystem : MonoBehaviour
    {
        private Rigidbody rdby;
        public float speed;//速度
        public Vector3 direction;
        public int index;
        private Vector3 pos;
        private ConveyorData conveyorData;
        public ConveyorOperationStatus ConveyorDataStatus
        {
            get
            {
                if (conveyorData == null)
                    conveyorData = DataManager.Instance.GetDataById<ConveyorData>(index);
                    if(conveyorData == null)
                        return ConveyorOperationStatus.close;
                return conveyorData.operationStatus;
            }
        }
        //
        private void Start()
        {
            rdby = transform.Find("Belt").GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (ConveyorDataStatus == ConveyorOperationStatus.close) return;
            pos = rdby.position;
            rdby.position += direction * speed * Time.fixedDeltaTime;
            rdby.MovePosition(pos);
        }

    }
}