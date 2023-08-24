using LabProductLine.DataManagerModule;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LabProductLine.RobotArmModule
{
    public class DobotRotation : MonoBehaviour
    {
        [SerializeField]
        private int index;//这里的index从0开始
        public int Index { get => index; }
        [HideInInspector]
        public List<Transform> jointTF = new List<Transform>();//存储四个机械轴的transform引用
        List<Quaternion> baseRotation = new List<Quaternion>();//存储四个轴的旋量
        [HideInInspector]
        public Transform suctionHolder;//末端整体部分的transform
        private int length = 4;//轴的数量
        public float speed = 7f;//这个速度下机械臂转动渲染不会有明显卡帧
        private float[] robotJoint;//旋转关节角度

        private RobotData robotData;//存储机械臂数据引用
        public RobotOperationStatus ArmStatus
        {
            private set { }
            get
            {
                if (robotData == null)
                {
                    robotData = DataManager.Instance.GetDataById<RobotData>(index);
                    if (robotData == null)
                        return RobotOperationStatus.waiting;
                }
                return robotData.OperationStatus;
            }
        }

        private ControlMode _controlMode = ControlMode.Entity2Digital;//初始化控制模式为实控虚
        public ControlMode ControlMode { get => _controlMode; set => _controlMode = value; }


        private void Awake()
        {
            Transform tf = transform;
            for (int i = 0; i < length + 3; i++)//此处因为有其余三个节点，所以长度加三
            {
                tf = tf.GetChild(0);
                if (i == 4)
                {
                    suctionHolder = tf;//获取末端整体部分的Transform
                    continue;
                }
                if (i == 1 || i == 5)//跳过Fpoint和另外两个点
                    continue;
                jointTF.Add(tf);
                baseRotation.Add(tf.localRotation);
            }
            baseRotation.Add(suctionHolder.localRotation);//将该末端整体部分的旋量放在最后,即第五个部分
        }



        private void FixedUpdate()//每0.02s执行一次
        {
            if (ArmStatus == RobotOperationStatus.working && ControlMode == ControlMode.Entity2Digital)
            {
                SelectRobot();
                CalculateRotation(robotJoint);//计算旋量
                suctionHolder.localRotation = Quaternion.Slerp(suctionHolder.localRotation, baseRotation[4], speed * Time.fixedDeltaTime);//将末端对齐到水平线
                for (int i = 0; i < 4; i++)
                {
                    jointTF[i].localRotation = Quaternion.Slerp(jointTF[i].localRotation, baseRotation[i], speed * Time.fixedDeltaTime);
                }
            }
        }


        private void SelectRobot()//根据索引选择对应的机械臂
        {
            robotJoint = DataManager.Instance.GetDataById<RobotData>(index).JointAngles;
        }



        private float[] JointArr;
        /// <summary>
        /// 将float类型数据转换为Quaternion类型,注意：这里传入的只是四个关节角度
        /// </summary>
        private void CalculateRotation(float[] robotJoint)
        {

            if (robotJoint == null)//判断参数是否为空
                return;
            else if (JointArr == null) //第一次进入给赋值
                JointArr = robotJoint; //相当于存储着上一次的参数
            else if (JointArr.SequenceEqual(robotJoint)) return; //若相等，说明机械臂没有移动，则不需要计算

            for (int i = 0; i < 4; i++)//采用相对坐标计算旋量
            {
                baseRotation[i] = (i == 2) ? Quaternion.Euler(jointTF[i].localEulerAngles.x, -(robotJoint[i] - robotJoint[i - 1]), jointTF[i].localEulerAngles.z)
                    : Quaternion.Euler(jointTF[i].localEulerAngles.x, -robotJoint[i], jointTF[i].localEulerAngles.z);//将角度值转换为四元数,并存入数组中,这里小臂需要特殊计算，因为大臂运动会带动小臂运动，需要加上大臂的旋转度数
            }
            baseRotation[4] = Quaternion.Euler(suctionHolder.localEulerAngles.x, robotJoint[2], suctionHolder.localEulerAngles.z);

            JointArr = robotJoint; //最后将这次参数赋值给变量
        }
    }
}

