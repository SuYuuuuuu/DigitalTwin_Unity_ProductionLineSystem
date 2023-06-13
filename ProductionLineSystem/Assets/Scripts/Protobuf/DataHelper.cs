using Google.Protobuf;
using System.IO;

namespace LabProductLine.Protobuf
{

    public class DataHelper
    {
        public static byte[] Serialize(DobotNonRealTimeData data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }

        }

        public static byte[] Serialize(DobotRealTimeData data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }

        }

        public static byte[] Serialize(PositionSensor_Data data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }

        }

        public static byte[] Serialize(Conveyor_Data data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }

        }
        public static byte[] Serialize(Cylinder_Data data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }
        }

        public static byte[] Serialize(EarPhone_Data data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }
        }
        public static byte[] Serialize(Plug_Data data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }
        }

        public static byte[] Serialize(Track_Data data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }
        }

        public static byte[] Serialize(Phone_Data data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }
        }

        public static byte[] Serialize(PhoneBox_Data data)
        {
            using (MemoryStream output = new MemoryStream())
            {
                data.WriteTo(output);
                return output.ToArray();
            }
        }


        public static DobotNonRealTimeData DeserializeNonRealTimeData(byte[] data)
        {
            DobotNonRealTimeData dobotData = DobotNonRealTimeData.Parser.ParseFrom(data);
            return dobotData;
        }


        public static DobotRealTimeData DeserializeRealTimeData(byte[] data)
        {
            DobotRealTimeData dobotData = DobotRealTimeData.Parser.ParseFrom(data);
            return dobotData;
        }

        public static Conveyor_Data DeserializeConveyorData(byte[] data)
        {
            Conveyor_Data conveyorData = Conveyor_Data.Parser.ParseFrom(data);
            return conveyorData;
        }

        public static Cylinder_Data DeserializeCylinderData(byte[] data)
        {
            Cylinder_Data cylinderData = Cylinder_Data.Parser.ParseFrom(data);
            return cylinderData;
        }

        public static PositionSensor_Data DeserializePositionSensorData(byte[] data)
        {
            PositionSensor_Data positionSensorData = PositionSensor_Data.Parser.ParseFrom(data);
            return positionSensorData;
        }

        public static EarPhone_Data DeserializeEarPhoneData(byte[] data)
        {
            EarPhone_Data earPhoneData = EarPhone_Data.Parser.ParseFrom(data);
            return earPhoneData;
        }

        public static Plug_Data DeserializePlugData(byte[] data)
        {
            Plug_Data plugData = Plug_Data.Parser.ParseFrom(data);
            return plugData;
        }

        public static Track_Data DeserializeTrackData(byte[] data)
        {
            Track_Data trackData = Track_Data.Parser.ParseFrom(data);
            return trackData;
        }

        public static Phone_Data DeserializePhoneData(byte[] data)
        {
            Phone_Data phoneData = Phone_Data.Parser.ParseFrom(data);
            return phoneData;
        }

        public static PhoneBox_Data DeserializePhoneBoxData(byte[] data)
        {
            PhoneBox_Data phoneBoxData = PhoneBox_Data.Parser.ParseFrom(data);
            return phoneBoxData;
        }

    }
}