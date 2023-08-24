using UnityEngine;

namespace LabProductLine.ProductionLineSimulationModule
{

    public class ScrollViewCamera : MonoBehaviour
    {
        public float zoomSpeed = 10f; // �����ٶ�
        private float scrollInput;

        void Update()
        {
            // ��ȡ��������
            scrollInput = -Input.GetAxis("Mouse ScrollWheel");

            // ���ݹ��������������ķŴ���С
            Camera.main.fieldOfView += scrollInput * zoomSpeed;

            // ���������Ұ�ķ�Χ
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 60f);
        }

    }
}