using LabProductLine.DataManagerModule;
using UnityEngine;



namespace LabProductLine.SensorModule
{
    /// <summary>
    ///��������ײ���
    /// <summary>
    public class Sensor : MonoBehaviour
    {
        [SerializeField]
        private int index;
        private Vector3 infraredDefaultRayStart;//射线默认起始位置
        private Vector3 infraredDefaultRayEnd;//射线默认末端位置
        private Vector3 infraredCurrentRayEnd;//射线当前末端位置
        private Vector3 direction;//射线方向
        private GameObject infraredRay;//射线实例
        public Material material;

        private PositionSensorData sensorData;
        public PositionSensorData SensorData
        {
            get
            {
                if (sensorData == null)
                {
                    sensorData = DataManager.Instance.GetDataById<PositionSensorData>(index);
                }
                return sensorData;
            }
        }

        private void Start()
        {
            infraredDefaultRayStart = transform.position;//当前位置
            //infraredDefaultRayEnd = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.forward.z * 1.1f);//�˴���z�������Ҫ����ԭ����λ�õ�z�ᣬ����᲻����
            infraredDefaultRayEnd = new Vector3(transform.position.x + transform.right.x * 1.3f, transform.position.y, transform.position.z);
            infraredCurrentRayEnd = infraredDefaultRayEnd;
            //direction = transform.TransformDirection(Vector3.forward);
            direction = transform.TransformDirection(Vector3.right);
            infraredRay = DrawLine(infraredDefaultRayStart, infraredDefaultRayEnd);
        }

        public GameObject DrawLine(Vector3 start, Vector3 end)//
        {
            GameObject line = new GameObject();

            line.transform.position = start;
            line.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.material = material;
            lineRenderer.name = "InfraredRay";
            //lineRenderer.material.color = new Color(255, 0, 0, 47);
            //

            //Set width
            lineRenderer.startWidth = 0.014f;
            lineRenderer.endWidth = 0.014f;

            //Set position
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            return line;
        }

        private void Update()
        {
            InfraredRay_Virtual();
        }

        /// <summary>
        ///
        /// </summary>
        private void InfraredRay_Virtual()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, 1.1f))
            {
                infraredCurrentRayEnd = infraredDefaultRayEnd;//��ֵ��ǰ��ά����
                //infraredCurrentRayEnd.z = hit.point.z;//�ı�z��λ�õ��ڸı�����λ��
                infraredCurrentRayEnd.x = hit.point.x;//�ı�z��λ�õ��ڸı�����λ��
                Destroy(infraredRay);//�ݻ�֮ǰ������
                infraredRay = DrawLine(infraredDefaultRayStart, infraredCurrentRayEnd);//�½�һ������
                //SetCylinderBySensor();
            }
            else
            {
                if (infraredCurrentRayEnd != infraredDefaultRayEnd)
                {
                    Destroy(infraredRay);
                    infraredRay = DrawLine(transform.position, infraredDefaultRayEnd);
                    infraredCurrentRayEnd = infraredDefaultRayEnd;//��������
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void InfraredRay_Entity()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, direction, out hitInfo, 0.6f))
            {
                infraredCurrentRayEnd = infraredDefaultRayEnd;//��ֵ��ǰ��ά����
                infraredCurrentRayEnd.z = hitInfo.point.z;//�ı�z��λ�õ��ڸı�����λ��
                Destroy(infraredRay);//�ݻ�֮ǰ������
                infraredRay = DrawLine(infraredDefaultRayStart, infraredCurrentRayEnd);//�½�һ������
            }
            else
            {
                if (infraredCurrentRayEnd != infraredDefaultRayEnd)
                {
                    Destroy(infraredRay);
                    infraredRay = DrawLine(infraredDefaultRayStart, infraredDefaultRayEnd);
                    infraredCurrentRayEnd = infraredDefaultRayEnd;//��������
                }
            }
        }
        private bool sign = true;//���ڿ��ƺ���ִ�д���Ϊһ��
        private void SetCylinderBySensor()//����sensor����ȥ�������׶���
        {
            if (sign)
            {
                switch (gameObject.name)
                {
                    case "ChargerSensorPoint":

                        break;
                    case "EarPhoneSensorPoint":

                        break;
                    case "TraySensorPoint":

                        break;
                    case "PhoneSensorPoint":

                        break;
                }
                sign = false;//�������Ҫ���䵱������ռ��л�е�������תץȡ���ź�ֵ��Ϊtrue���ӳټ��룩
            }

        }
    }
}














