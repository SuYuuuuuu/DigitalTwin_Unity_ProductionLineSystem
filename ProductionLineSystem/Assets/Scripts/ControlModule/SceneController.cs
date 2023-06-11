using Common;
using LabProductLine.DataManagerModule;

namespace LabProductLine.ControlModule
{

    public class SceneController : MonoSingleton<SceneController>
    {
        public int robotData_Num = 5;
        public int earPhoneData_Num = 5;
        public int phoneBoxData_Num = 5;
        public int phoneData_Num = 5;
        public int plugData_Num = 5;
        public int trackData_Num = 5;
        public int cylinderData_Num = 5;
        public int positionSensorData_Num = 5;
        public int conveyorData_Num = 1;
        public int grapData_Num = 1;
        public override void Init()
        {
            base.Init();

            //�������ݹ�����������������-->���RobotData/���CylinderData
            ///һ��GrapData/һ��ConveyorData
            //����earPhoneData/����PhoneBoxData/����PhoneData/����PlugData/����TrackData
            //�˸�PositionSensorData
            for (int i = 0; i < 8; i++)
            {
                ToolDataManager.Instance.AddPositionSensorData(new PositionSensorData() { ID = i });
                if (i >= 2)
                {
                    ProductionDataManager.Instance.AddEarPhoneData(new EarPhoneData() { ID = i - 2 });
                    ProductionDataManager.Instance.AddPlugData(new PlugData() { ID = i - 2 });
                    ProductionDataManager.Instance.AddTrackData(new TrackData() { ID = i - 2 });
                    ProductionDataManager.Instance.AddPhoneData(new PhoneData() { ID = i - 2 });
                    ProductionDataManager.Instance.AddPhoneBoxData(new PhoneBoxData() { ID = i - 2 });
                }

                if (i >= 3)
                {
                    ToolDataManager.Instance.AddRobotData(new RobotData() { RobotID = i - 3 });
                    ToolDataManager.Instance.AddCylinderData(new CylinderData() { ID = i - 3 });
                }
            }
            ToolDataManager.Instance.AddConveyorData(new ConveyorData() { ID = 0 });
            ProductionDataManager.Instance.AddGrapData(new GrapData() { ID = 0 });


        }

        private void GenerateData<T>(T data,int num) where T : BaseData     
        {
            for (int i = 0; i < num; i++)
            {

            }
        }

    }



}