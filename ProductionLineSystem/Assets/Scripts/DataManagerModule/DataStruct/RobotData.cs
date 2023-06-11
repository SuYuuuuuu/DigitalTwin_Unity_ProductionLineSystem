
using Common;
using System;
using System.Linq;
using UnityEngine;

namespace LabProductLine.DataManagerModule
{
    /// <summary>
    /// 机械臂数据
    /// </summary>
    [System.Serializable]
    public class RobotData : BaseData
    {

        public event Action<int, float[]> EndEffectorPosDataChanged;
        public event Action<int, float[]> JointAnglesDataChanged;
        public event Action<int, string> NameDataChanged;
        public event Action<int, int> IdDataChanged;
        public event Action<int, float> VibrationalFrequencyDataChanged;
        public event Action<int, float> TemperatureDataChanged;
        public event Action<int, float[]> HomeParamsDataChanged;
        public event Action<int, bool[]> EndEffectorSuctionCupDataChanged;
        public event Action<int, float[]> JogJointParamsDataChanged;
        public event Action<int, float[]> JogCoordinateDataChanged;
        public event Action<int, float[]> JogCommonDataChanged;
        public event Action<int, float[]> PTPJointParamsDataChanged;
        public event Action<int, float[]> PTPCoordinateParamsDataChanged;
        public event Action<int, float[]> PTPJumpParamsDataChanged;
        public event Action<int, float[]> PTPJump2ParamsDataChanged;
        public event Action<int, float[]> PTPCommonParamsDataChanged;
        public event Action<int, RobotOperationStatus> RobotOperationStatusDataChanged;


        public int robotID;//机械臂唯一ID ,这里修改了访问属性是为了在Inspector面板序列化出来，用于手动填写Id
        public int RobotID
        {
            get => robotID;
            set
            {
                if (value != robotID)
                {
                    robotID = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => IdDataChanged?.Invoke(robotID, value));
                    //JointAnglesDataChanged?.Invoke(robotID,value);
                }
            }
        }
        private string robotName;//机械臂名字
        public string RobotName
        {
            get => robotName;
            set
            {
                if (value != robotName)
                {
                    robotName = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => NameDataChanged?.Invoke(robotID, value));
                    //JointAnglesDataChanged?.Invoke(robotID,value);
                }
            }
        }
        public string robotDescription;//机械臂描述

        public string alarmState;

        private float[] homeParams;
        public float[] HomeParams
        {
            get => homeParams;
            set
            {
                if (homeParams == null || !value.SequenceEqual(homeParams))
                {
                    homeParams = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => HomeParamsDataChanged?.Invoke(robotID, value));
                    //JointAnglesDataChanged?.Invoke(robotID,value);
                }
            }
        }

        private bool[] endEffectorSuctionCup;
        public bool[] EndEffectorSuctionCup
        {
            get => endEffectorSuctionCup;
            set
            {
                if (endEffectorSuctionCup == null || !value.SequenceEqual(endEffectorSuctionCup))
                {
                    endEffectorSuctionCup = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => EndEffectorSuctionCupDataChanged?.Invoke(robotID, value));
                    //JointAnglesDataChanged?.Invoke(robotID,value);
                }
            }
        }

        private float[] jointAngles;//机械臂角度
        public float[] JointAngles
        {
            get { return jointAngles; }
            set
            {
                if (jointAngles == null || !value.SequenceEqual(jointAngles))
                {
                    jointAngles = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => JointAnglesDataChanged?.Invoke(robotID, value));
                    //JointAnglesDataChanged?.Invoke(robotID,value);
                }
            }
        }

        private float[] endEffectorPos;
        public float[] EndEffectorPos//机械臂末端位置
        {
            get { return endEffectorPos; }
            set
            {
                if (endEffectorPos == null || !value.SequenceEqual(endEffectorPos))
                {
                    endEffectorPos = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => EndEffectorPosDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float[] jogJointParams;
        public float[] JogJointParams//机械臂关节点动参数
        {
            get { return jogJointParams; }
            set
            {
                if (jogJointParams == null || !value.SequenceEqual(jogJointParams))
                {
                    jogJointParams = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => JogJointParamsDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float[] jogCoordinateParams;
        public float[] JogCoordinateParams//机械臂关节点动参数
        {
            get { return jogCoordinateParams; }
            set
            {
                if (jogCoordinateParams == null || !value.SequenceEqual(jogCoordinateParams))
                {
                    jogCoordinateParams = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => JogCoordinateDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float[] jogCommonParams;
        public float[] JogCommonParams//机械臂关节点动参数
        {
            get { return jogCommonParams; }
            set
            {
                if (jogCommonParams == null || !value.SequenceEqual(jogCommonParams))
                {
                    jogCommonParams = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => JogCommonDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float[] ptpJointParams;
        public float[] PTPJointParams//机械臂关节点动参数
        {
            get { return ptpJointParams; }
            set
            {
                if (ptpJointParams == null || !value.SequenceEqual(ptpJointParams))
                {
                    ptpJointParams = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => PTPJointParamsDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float[] ptpCoordinateParams;
        public float[] PTPCoordinateParams//机械臂关节ptp参数
        {
            get { return ptpCoordinateParams; }
            set
            {
                if (ptpCoordinateParams == null || !value.SequenceEqual(ptpCoordinateParams))
                {
                    ptpCoordinateParams = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => PTPCoordinateParamsDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float[] ptpJumpParams;
        public float[] PTPJumpParams//机械臂关节点动参数
        {
            get { return ptpJumpParams; }
            set
            {
                if (ptpJumpParams == null || !value.SequenceEqual(ptpJumpParams))
                {
                    ptpJumpParams = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => PTPJumpParamsDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float[] ptpJump2Params;
        public float[] PTPJump2Params//机械臂关节点动参数
        {
            get { return ptpJump2Params; }
            set
            {
                if (ptpJump2Params == null || !value.SequenceEqual(ptpJump2Params))
                {
                    ptpJump2Params = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => PTPJump2ParamsDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float[] ptpCommonParams;
        public float[] PTPCommonParams//机械臂关节点动参数
        {
            get { return ptpCommonParams; }
            set
            {
                if (ptpCommonParams == null || !value.SequenceEqual(ptpCommonParams))
                {
                    ptpCommonParams = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => PTPCommonParamsDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private RobotOperationStatus operationStatus = RobotOperationStatus.waiting;
        public RobotOperationStatus OperationStatus
        {
            get { return operationStatus; }
            set
            {
                if (value != operationStatus)
                {
                    operationStatus = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => RobotOperationStatusDataChanged?.Invoke(robotID, value));
                }
            }
        }


        private float temperature;
        public float Temperature//电机温度
        {
            get => temperature;
            set
            {
                if (value != temperature)
                {
                    temperature = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => TemperatureDataChanged?.Invoke(robotID, value));
                }
            }
        }

        private float vibrationalFrequency;
        public float VibrationalFrequency//振动频率
        {
            get => vibrationalFrequency;
            set
            {
                if (value != vibrationalFrequency)
                {
                    vibrationalFrequency = value;
                    UnityMainThreadDispatcher.Instance.Enqueue(() => VibrationalFrequencyDataChanged?.Invoke(robotID, value));
                }
            }
        }

        public Vector3 objectPos;//放置位置





    }
}