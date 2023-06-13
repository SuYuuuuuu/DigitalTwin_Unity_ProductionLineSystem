using Common;
using LabProductLine.DataManagerModule;
using UnityEngine;

namespace LabProductLine.ControlModule
{

    public class SceneController : MonoSingleton<SceneController>
    {
        [Tooltip("机械臂数据数量")]
        public int robotData_Num = 5;
        [Tooltip("耳机数据数量")]
        public int earPhoneData_Num = 5;
        [Tooltip("手机盒数据数量")]
        public int phoneBoxData_Num = 5;
        [Tooltip("手机数据数量")]
        public int phoneData_Num = 5;
        [Tooltip("插头数据数量")]
        public int plugData_Num = 5;
        [Tooltip("手机板数据数量")]
        public int trackData_Num = 5;
        [Tooltip("气缸数据数量")]
        public int cylinderData_Num = 5;
        [Tooltip("红外传感器数据数量")]
        public int positionSensorData_Num = 5;
        [Tooltip("传送带数据数量")]
        public int conveyorData_Num = 1;
        [Tooltip("抓取数量数据数量")]
        public int grapData_Num = 1;
        public override void Init()
        {
            base.Init();
            // for (int i = 0; i < 8; i++)
            // {
            //     ToolDataManager.Instance.AddPositionSensorData(new PositionSensorData() { ID = i });
            //     if (i >= 2)
            //     {
            //         ProductionDataManager.Instance.AddEarPhoneData(new EarPhoneData() { ID = i - 2 });
            //         ProductionDataManager.Instance.AddPlugData(new PlugData() { ID = i - 2 });
            //         ProductionDataManager.Instance.AddTrackData(new TrackData() { ID = i - 2 });
            //         ProductionDataManager.Instance.AddPhoneData(new PhoneData() { ID = i - 2 });
            //         ProductionDataManager.Instance.AddPhoneBoxData(new PhoneBoxData() { ID = i - 2 });
            //     }

            //     if (i >= 3)
            //     {
            //         ToolDataManager.Instance.AddRobotData(new RobotData() { RobotID = i - 3 });
            //         ToolDataManager.Instance.AddCylinderData(new CylinderData() { ID = i - 3 });
            //     }
            // }
            // ToolDataManager.Instance.AddConveyorData(new ConveyorData() { ID = 0 });
            // ProductionDataManager.Instance.AddGrapData(new GrapData() { ID = 0 });
            // GenerateData<RobotData>(new RobotData(), robotData_Num);
            // GenerateData<CylinderData>(new CylinderData(), cylinderData_Num);
            // GenerateData<ConveyorData>(new ConveyorData(), conveyorData_Num);
            // GenerateData<PositionSensorData>(new PositionSensorData(), positionSensorData_Num);
            // GenerateData<EarPhoneData>(new EarPhoneData(), earPhoneData_Num);
            // GenerateData<PlugData>(new PlugData(), plugData_Num);
            // GenerateData<TrackData>(new TrackData(), trackData_Num);
            // GenerateData<PhoneData>(new PhoneData(), phoneData_Num);
            // GenerateData<PhoneBoxData>(new PhoneBoxData(), phoneBoxData_Num);

        }

        private void GenerateData<T>(T data, int num) where T : BaseData
        {
            for (int i = 0; i < num; i++)
            {
                DataManager.Instance.AddData<T>(data);
            }
        }


    }



}