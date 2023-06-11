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
        private Vector3 pos;
        private ConveyorData conveyorData;
        public ConveyorData ConveyorData
        {
            get
            {
                if (conveyorData == null)
                    conveyorData = ToolDataManager.Instance.GetConveyorDataByID(0);
                return conveyorData;
            }
        }
        //����ɼ�һ��������ͼ�Ĵ���
        private void Start()
        {
            rdby = this.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (ConveyorData.operationStatus == ConveyorOperationStatus.close) return;
            pos = rdby.position;
            rdby.position += direction * speed * Time.fixedDeltaTime;
            rdby.MovePosition(pos);
        }

    }
}