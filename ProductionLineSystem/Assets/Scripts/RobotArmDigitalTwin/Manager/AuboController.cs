using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabProductLine.RobotArmDigitalTwin
{

    public class AuboController : MonoBehaviour
    {
        private List<Transform> jointTF = new List<Transform>();//存储关节tf引用
        private List<Quaternion> baseRotation = new List<Quaternion>();//存储关节角度
        private int length = 6;//关节轴数量
        private float[] JointArr;//用于存储上次的关节角度数组
        [Header("Robot Switch")]
        public bool isRotated;
        [Header("Basic Settings")]
        public float speed = 7f;//关节转动速度
        public float rotateTime = 2f; //到目标点的旋转时间

        private float[] robotJoint;
        public float[] RobotJoint{get => robotJoint;set => robotJoint = value;}//存储获取的关节角度数组

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
            if (isRotated)
                Move_Robot_Arm(robotJoint);
        }


        private IEnumerator Move_RobotArm(float[] joint_angles, float time = 2f)
        {
            float curTime = 0f;
            CalculateRotation(joint_angles);//计算角度
            Quaternion[] startRotation = new Quaternion[jointTF.Count];
            for (int i = 0; i < jointTF.Count; i++)
            {
                startRotation[i] = jointTF[i].localRotation;
            }
            while (curTime < time)
            {
                for (int i = 0; i < length; i++)
                {
                    jointTF[i].localRotation = Quaternion.Slerp(startRotation[i], baseRotation[i], curTime/time);
                }
                curTime +=Time.deltaTime;
                yield return null;
            }

        }
        public void MoveRobot(float[] joint_angles, float time = 2f)
        {
            StartCoroutine(Move_RobotArm(joint_angles,time));
        }




        private void Move_Robot_Arm(float[] joint_angles)
        {
            CalculateRotation(joint_angles);//计算角度
            for (int i = 0; i < length; i++)
            {
                jointTF[i].localRotation = Quaternion.Slerp(jointTF[i].localRotation, baseRotation[i], speed * Time.fixedDeltaTime);
            }
        }



        private void CalculateRotation(float[] robotJoint)
        {

            if (robotJoint == null)//若获取到的角度为空则返回
                return;
            else if (JointArr == null) JointArr = robotJoint;
            else if (JointArr.SequenceEqual(robotJoint)) return; //比较上次的数组和这次的数组，若相同则返回

            for (int i = 0; i < length; i++)//
            {
                if (i == 2 || i == 3)
                    baseRotation[i] = Quaternion.Euler(jointTF[i].localEulerAngles.x, jointTF[i].localEulerAngles.y, robotJoint[i]);
                else
                    baseRotation[i] = i == 4 ? Quaternion.Euler(jointTF[i].localEulerAngles.x, robotJoint[i], -jointTF[i].localEulerAngles.z)
                       : Quaternion.Euler(jointTF[i].localEulerAngles.x, -robotJoint[i], jointTF[i].localEulerAngles.z);

            }

            JointArr = (float[])robotJoint.Clone(); //赋值数组
        }
    }
}

