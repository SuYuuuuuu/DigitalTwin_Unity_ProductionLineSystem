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

        public static InputSignal DeserializeInputSignal(byte[] data)
        {
            InputSignal inputData = InputSignal.Parser.ParseFrom(data);
            return inputData;
        }

        public static OutputSignal DeserializeOutputSignal(byte[] data)
        {
            OutputSignal outputData = OutputSignal.Parser.ParseFrom(data);
            return outputData;
        }

    }
}