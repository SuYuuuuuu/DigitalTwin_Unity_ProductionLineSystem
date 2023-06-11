using LabProductLine.DataManagerModule;
using UnityEngine;


namespace LabProductLine.CylinderModule
{
    public class Cylinder : MonoBehaviour
    {
        [SerializeField]
        private int index;
        private Animator animator;
        private CylinderData cylinderData;
        public CylinderData CylinderData
        {
            get
            {
                if (cylinderData == null)
                {
                    cylinderData = ToolDataManager.Instance.GetCylinderDataByID(index);
                }
                return cylinderData;
            }
        }

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


        private void Start()
        {
            animator = this.GetComponent<Animator>();
        }
        private void Update()
        {
            //if (ConveyorData.operationStatus == ConveyorOperationStatus.close) return;

            if (CylinderData.operationStatus == CylinderOperationStatus.open && !animator.GetBool("IsDetected"))
                animator.SetBool("IsDetected", true);
            else if (CylinderData.operationStatus == CylinderOperationStatus.close && animator.GetBool("IsDetected"))
                animator.SetBool("IsDetected", false);
        }



    }
}


