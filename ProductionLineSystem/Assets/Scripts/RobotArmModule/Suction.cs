using LabProductLine.DataManagerModule;
using UnityEngine;

namespace LabProductLine.RobotArmModule
{

    public class Suction : MonoBehaviour
    {

        Rigidbody myRigidbody;
        [HideInInspector]
        public FixedJoint fixedJoint;
        public float breakForce;
        public float breakTorque;
        private int index;


        private void Start()
        {
            index = GetComponentsInParent<DobotRotation>()[0].Index;
        }



        private void OnTriggerStay(Collider other)
        {
            if (ToolDataManager.Instance.GetRobotDataByID(index).EndEffectorSuctionCup[1] && other.gameObject.CompareTag("grappable"))
            {
                myRigidbody = other.attachedRigidbody;
                other.isTrigger = true;
                if (myRigidbody == null) return;

                if (fixedJoint == null)
                {
                    fixedJoint = gameObject.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = myRigidbody;
                    fixedJoint.enableCollision = true;
                    fixedJoint.breakForce = breakForce;
                    fixedJoint.breakTorque = breakTorque;
                }
            }
            else if (!ToolDataManager.Instance.GetRobotDataByID(index).EndEffectorSuctionCup[1] && other.gameObject.CompareTag("grappable"))
            {
                if (gameObject.TryGetComponent<FixedJoint>(out fixedJoint))
                    Destroy(fixedJoint);
                other.isTrigger = false;
            }
        }

    }
}