using UnityEngine;

namespace LabProductLine.ProductionLineSimulationModule
{
    public class FreeRoamController : MonoBehaviour
    {
        public float moveSpeed = 5f; // 相机移动速度
        public float rotationSpeed = 100f; // 相机旋转速度
        private float horizontal;
        private float vertical;
        private float up;
        private float rotateX;
        private float rotateY;
        private float rotX;
        private float rotY;
        private Vector3 initialPosition; // 相机初始位置

        private void Start()
        {
            initialPosition = transform.position;
            //Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // 相机移动
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            up = Input.GetAxis("Jump");

            Vector3 moveDirection = new Vector3(horizontal, up, vertical);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            // 相机旋转
            rotateX = Input.GetAxis("Mouse X");
            rotateY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, rotateX * rotationSpeed * Time.deltaTime);
            transform.Rotate(Vector3.left, rotateY * rotationSpeed * Time.deltaTime);


            // 重置相机位置和旋转
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetCamPos();
            }

        }

        private void LateUpdate()
        {
            if (transform.localEulerAngles.z != 0)
            {
                rotX = transform.localEulerAngles.x;
                rotY = transform.localEulerAngles.y;
                transform.localEulerAngles = new Vector3(rotX, rotY, 0);
            }
        }



        public void ResetCamPos()
        {
            transform.position = initialPosition;
            transform.rotation = Quaternion.identity;
        }
    }
}

