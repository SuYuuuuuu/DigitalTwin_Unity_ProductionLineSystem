using Common;
using System.Collections.Generic;
using UnityEngine;

namespace LabProductLine.DataManagerModule
{


    public class ToolDataManager : SingletonBase<ToolDataManager>, IToolDataAccess
    {
        Dictionary<int, RobotData> robotDic;
        Dictionary<int, PositionSensorData> positionSensorDic;
        Dictionary<int, CylinderData> cylinderDic;
        Dictionary<int, ConveyorData> conveyorDic;


        public ToolDataManager()
        {
            robotDic = new Dictionary<int, RobotData>();
            positionSensorDic = new Dictionary<int, PositionSensorData>();
            cylinderDic = new Dictionary<int, CylinderData>();
            conveyorDic = new Dictionary<int, ConveyorData>();
        }

        /// <summary>
        /// 根据ID获取机械臂数据类型
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public RobotData GetRobotDataByID(int ID)
        {
            if (!robotDic.ContainsKey(ID))
                return null;
            return robotDic[ID];
        }

        public ConveyorData GetConveyorDataByID(int ID)
        {
            if (!conveyorDic.ContainsKey(ID))
                return null;
            return conveyorDic[ID];
        }

        public CylinderData GetCylinderDataByID(int ID)
        {
            if (!cylinderDic.ContainsKey(ID))
                return null;
            return cylinderDic[ID];
        }

        public PositionSensorData GetPositionSensorDataByID(int ID)
        {
            if (!positionSensorDic.ContainsKey(ID))
                return null;
            return positionSensorDic[ID];
        }

        /// <summary>
        /// 添加机械臂数据
        /// </summary>
        /// <param name="robotData"></param>
        public void AddRobotData(RobotData robotData)
        {
            if (!robotDic.ContainsKey(robotData.RobotID))
                robotDic.Add(robotData.RobotID, robotData);
            else
                Debug.LogWarning("Robot data with ID " + robotData.RobotID + " already exists.");
        }

        /// <summary>
        /// 移除机械臂数据
        /// </summary>
        /// <param name="data"></param>
        public void RemoveRobotData(int robotID)
        {
            if (robotDic.ContainsKey(robotID))
                robotDic.Remove(robotID);
            else
                Debug.LogError("Robot data with ID " + robotID + " does not exist.");
        }
        public void RemoveRobotData(RobotData robotData)
        {
            if (robotDic.ContainsKey(robotData.RobotID))
                robotDic.Remove(robotData.RobotID);
            else
                Debug.LogError("Robot data with ID " + robotData.RobotID + " does not exist.");
        }

        /// <summary>
        /// 根据ID更新机械臂数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="data"></param>
        public void UpdateRobotDataByID(int ID, RobotData data)
        {
            RobotData robotData = GetRobotDataByID(ID);
            if (robotData != null)
            {
                robotData = data;
            }
        }
        public void UpdateRobotDataByID(int ID, float[] jointAngle = default, float[] endEffectorPos = default, RobotOperationStatus status = default)
        {
            RobotData robotData = GetRobotDataByID(ID);
            if (robotData != null)
            {
                if (jointAngle != default)
                    robotData.JointAngles = jointAngle;
                if (endEffectorPos != default)
                    robotData.EndEffectorPos = endEffectorPos;
                if (status != default)
                    robotData.OperationStatus = status;
            }
        }



        //������������Ҫ���Ӵ���
        /// <summary>
        /// ���Ӵ��ʹ�����
        /// </summary>
        /// <param name="conveyorData"></param>
        public void AddConveyorData(ConveyorData conveyorData)
        {
            if (!conveyorDic.ContainsKey(conveyorData.ID))
                conveyorDic.Add(conveyorData.ID, conveyorData);
            else
                Debug.LogWarning("Conveyor data with ID " + conveyorData.ID + " already exists.");
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="cylinderData"></param>
        public void AddCylinderData(CylinderData cylinderData)
        {
            if (!cylinderDic.ContainsKey(cylinderData.ID))
                cylinderDic.Add(cylinderData.ID, cylinderData);
            else
                Debug.LogWarning("cylinder data with ID " + cylinderData.ID + " already exists.");
        }

        /// <summary>
        /// ����λ�ô���������
        /// </summary>
        /// <param name="positionSensorData"></param>
        public void AddPositionSensorData(PositionSensorData positionSensorData)
        {
            if (!positionSensorDic.ContainsKey(positionSensorData.ID))
                positionSensorDic.Add(positionSensorData.ID, positionSensorData);
            else
                Debug.LogWarning("positionSensor data with ID " + positionSensorData.ID + " already exists.");
        }


        public void RemoveConveyorData(ConveyorData conveyorData)
        {
            if (conveyorDic.ContainsKey(conveyorData.ID))
                conveyorDic.Remove(conveyorData.ID);
            else
                Debug.LogError("conveyor data with ID " + conveyorData.ID + " does not exist.");
        }
        public void RemoveConveyorData(int ID)
        {
            if (conveyorDic.ContainsKey(ID))
                conveyorDic.Remove(ID);
            else
                Debug.LogError("conveyor data with ID " + ID + " does not exist.");
        }

        public void RemoveCylinderData(CylinderData cylinderData)
        {
            if (cylinderDic.ContainsKey(cylinderData.ID))
                cylinderDic.Remove(cylinderData.ID);
            else
                Debug.LogError("cylinder data with ID " + cylinderData.ID + " does not exist.");
        }
        public void RemoveCylinderData(int ID)
        {
            if (cylinderDic.ContainsKey(ID))
                cylinderDic.Remove(ID);
            else
                Debug.LogError("cylinder data with ID " + ID + " does not exist.");
        }

        public void RemovePositionSensorData(PositionSensorData positionSensorData)
        {
            if (positionSensorDic.ContainsKey(positionSensorData.ID))
                positionSensorDic.Remove(positionSensorData.ID);
            else
                Debug.LogError("positionSensor data with ID " + positionSensorData.ID + " does not exist.");
        }
        public void RemovePositionSensorData(int ID)
        {
            if (positionSensorDic.ContainsKey(ID))
                positionSensorDic.Remove(ID);
            else
                Debug.LogError("positionSensor data with ID " + ID + " does not exist.");
        }

        public void UpdateConveyorDataByID(int ID, ConveyorData conveyorData)
        {
            ConveyorData data = GetConveyorDataByID(ID);
            if (data != null)
            {
                data = conveyorData;
            }
        }
        public void UpdateConveyorDataByID(int ID, ConveyorOperationStatus status, float speed = 1f)
        {
            ConveyorData data = GetConveyorDataByID(ID);
            if (data != null)
            {
                data.operationStatus = status;
                data.speed = speed;
            }
        }

        public void UpdateCylinderDataByID(int ID, CylinderData cylinderData)
        {
            CylinderData data = GetCylinderDataByID(ID);
            if (data != null)
            {
                data = cylinderData;
            }
        }
        public void UpdateCylinderDataByID(int ID, CylinderOperationStatus status)
        {
            CylinderData data = GetCylinderDataByID(ID);
            if (data != null)
            {
                data.operationStatus = status;
            }
        }

        public void UpdatePositionSensorDataByID(int ID, PositionSensorData positionSensorData)
        {
            PositionSensorData data = GetPositionSensorDataByID(ID);
            if (data != null)
            {
                data = positionSensorData;
            }
        }
        public void UpdatePositionSensorDataByID(int ID, bool detectStatus, Vector3 pos = default)
        {
            PositionSensorData data = GetPositionSensorDataByID(ID);
            if (data != null)
            {
                data.detectStatus = detectStatus;
                if (pos != default)
                    data.position = pos;
            }
        }

        public int GetRobotDataNumber()
        {
            return robotDic.Keys.Count;
        }

        public int GetPositionSensorNumber()
        {
            return positionSensorDic.Keys.Count;
        }

        public int GetCylinderNumber()
        {
            return cylinderDic.Keys.Count;
        }

        public int GetConveyorNumber()
        {
            return conveyorDic.Keys.Count;
        }


    }
}