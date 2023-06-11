using UnityEngine;

namespace LabProductLine.ProductionLineSimulationModule
{

    public class ScrollViewCamera : MonoBehaviour
    {
        public float zoomSpeed = 10f; // 缩放速度
        private float scrollInput;

        void Update()
        {
            // 获取滚轮输入
            scrollInput = -Input.GetAxis("Mouse ScrollWheel");

            // 根据滚轮输入调整相机的放大缩小
            Camera.main.fieldOfView += scrollInput * zoomSpeed;

            // 限制相机视野的范围
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 60f);
        }

    }
}