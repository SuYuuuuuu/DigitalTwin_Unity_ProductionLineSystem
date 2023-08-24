using LabProductLine.DataManagerModule;
using UnityEngine;


namespace LabProductLine.CylinderModule
{
    public class Cylinder : MonoBehaviour
    {
        [SerializeField]
        private int cylinder_Index;
        [SerializeField]
        private int conveyor_Index;
        private Animator animator;
        private CylinderData cylinderData;
        public CylinderData CylinderData
        {
            get
            {
                if (cylinderData == null)
                {
                    cylinderData = DataManager.Instance.GetDataById<CylinderData>(cylinder_Index);
                }
                return cylinderData;
            }
        }

        private ConveyorData conveyorData;
        public ConveyorOperationStatus ConveyorStatus
        {
            get
            {
                if (conveyorData == null)
                {
                    conveyorData = DataManager.Instance.GetDataById<ConveyorData>(conveyor_Index);
                    if(conveyorData==null) return ConveyorOperationStatus.close;
                }
                return conveyorData.operationStatus;
            }
        }


        private void Start()
        {
            animator = this.GetComponent<Animator>();
        }
        private void Update()
        {
            if (ConveyorStatus == ConveyorOperationStatus.close) return;
            if (CylinderData.operationStatus == CylinderOperationStatus.open && !animator.GetBool("IsDetected"))
                animator.SetBool("IsDetected", true);
            else if (CylinderData.operationStatus == CylinderOperationStatus.close && animator.GetBool("IsDetected"))
                animator.SetBool("IsDetected", false);
        }



    }
}


