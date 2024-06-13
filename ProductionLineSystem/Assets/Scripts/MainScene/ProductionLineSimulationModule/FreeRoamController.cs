using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LabProductLine.ProductionLineSimulationModule
{
    public class FreeRoamController : MonoBehaviour
    {
        public float moveSpeed = 5f; // 移动速度
        public float rotationSpeed = 100f; // 旋转速度
        private float horizontal;
        private float vertical;
        private float up;
        private float rotateX;
        private float rotateY;
        private float rotX;
        private float rotY;
        private Vector3 initialPosition; // 初始位置
        private Quaternion initialRotation; //初始旋转

        private RaycastHit hitInfo;

        private bool isCursorLocked = true;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = isCursorLocked ? CursorLockMode.None : CursorLockMode.Locked;
                isCursorLocked = !isCursorLocked;
            }

            // if (Input.GetMouseButtonDown(0))
            // {
            //     // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //     // if(Physics.Raycast(ray,out hitInfo))
            //     // {
            //     //     if(!hitInfo.transform.CompareTag("UI Button"))
            //     //         Cursor.lockState = CursorLockMode.Locked;
            //     // }
            // }

            // 获取键盘输入
            if (Cursor.lockState == CursorLockMode.None)
                return;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            up = Input.GetAxis("Jump");

            Vector3 moveDirection = new Vector3(horizontal, up, vertical);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            // 获取旋转输入
            rotateX = Input.GetAxis("Mouse X");
            rotateY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, rotateX * rotationSpeed * Time.deltaTime);
            transform.Rotate(Vector3.left, rotateY * rotationSpeed * Time.deltaTime);


            // 复位
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
            if (this.enabled == false) return;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

    }
}

