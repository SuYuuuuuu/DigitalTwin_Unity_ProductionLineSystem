namespace LabProductLine.DataManagerModule
{
    public interface IToolDataAccess
    {
        ConveyorData GetConveyorDataByID(int ID);
        CylinderData GetCylinderDataByID(int ID);
        PositionSensorData GetPositionSensorDataByID(int ID);
        RobotData GetRobotDataByID(int ID);
    }
}
