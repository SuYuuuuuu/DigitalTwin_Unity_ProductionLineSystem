using LabProductLine.DataManagerModule;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabProductLine.RobotArmModule
{

    public class AuboRotation : MonoBehaviour
    {
        [SerializeField]
        private int index = 4;
        private List<Transform> jointTF = new List<Transform>();//存储关节tf引用
        private List<Quaternion> baseRotation = new List<Quaternion>();//存储关节角度
        private int length = 6;//关节轴数量
        private float[] robotJoint;//存储获取的关节角度数组
        private float[] JointArr;//用于存储上次的关节角度数组
        public float speed = 7f;//关节转动速度

        private RobotData robotData;//机械臂数据
        public RobotOperationStatus ArmStatus //机械臂操作状态
        {
            get
            {
                if (robotData == null)
                {
                    robotData = DataManager.Instance.GetDataById<RobotData>(index);
                    if (robotData == null) return RobotOperationStatus.waiting;
                }
                return robotData.OperationStatus;
            }
        }

        private ControlMode _controlMode = ControlMode.Entity2Digital;//控制模式
        public ControlMode ControlMode { get => _controlMode; set => _controlMode = value; }

        private void Awake()
        {
            Transform tf = transform;
            for (int i = 0; i < length; i++)//获取关节组件引用
            {
                tf = tf.GetChild(0);
                jointTF.Add(tf);
                baseRotation.Add(tf.localRotation);
            }
        }

        private void FixedUpdate()
        {
            if (ArmStatus == RobotOperationStatus.working && ControlMode == ControlMode.Entity2Digital)
            {
                SelectRobot();
                CalculateRotation(robotJoint);//计算角度
                for (int i = 0; i < length; i++)
                {
                    jointTF[i].localRotation = Quaternion.Slerp(jointTF[i].localRotation, baseRotation[i], speed * Time.fixedDeltaTime);
                }
            }
        }


        private void SelectRobot()//选择对应机械臂的关节角度
        {
            robotJoint = DataManager.Instance.GetDataById<RobotData>(index).JointAngles;
        }


        private void CalculateRotation(float[] robotJoint)
        {

            if (robotJoint == null)//若获取到的角度为空则返回
                return;
            else if (JointArr == null) //第一次赋值另外一个角度数组
                JointArr = robotJoint;
            else if (JointArr.SequenceEqual(robotJoint)) return; //比较上次的数组和这次的数组，若相同则返回

            for (int i = 0; i < length; i++)//单独处理一下第二个关节
            {
                if (i == 2 || i == 3)
                    baseRotation[i] = Quaternion.Euler(jointTF[i].localEulerAngles.x, jointTF[i].localEulerAngles.y, robotJoint[i]);
                else
                    baseRotation[i] = i == 4 ? Quaternion.Euler(jointTF[i].localEulerAngles.x, robotJoint[i], -jointTF[i].localEulerAngles.z)
                       : Quaternion.Euler(jointTF[i].localEulerAngles.x, -robotJoint[i], jointTF[i].localEulerAngles.z);

            }

            JointArr = robotJoint; //赋值数组
        }
    }
}