using UnityEngine;

namespace LabProductLine.ProductionLineSimulationModule
{
    public class FreeRoamController : MonoBehaviour
    {
        public float moveSpeed = 5f; // ����ƶ��ٶ�
        public float rotationSpeed = 100f; // �����ת�ٶ�
        private float horizontal;
        private float vertical;
        private float up;
        private float rotateX;
        private float rotateY;
        private float rotX;
        private float rotY;
        private Vector3 initialPosition; // �����ʼλ��

        private void Start()
        {
            initialPosition = transform.position;
            //Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // ����ƶ�
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            up = Input.GetAxis("Jump");

            Vector3 moveDirection = new Vector3(horizontal, up, vertical);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            // �����ת
            rotateX = Input.GetAxis("Mouse X");
            rotateY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, rotateX * rotationSpeed * Time.deltaTime);
            transform.Rotate(Vector3.left, rotateY * rotationSpeed * Time.deltaTime);


            // �������λ�ú���ת
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

