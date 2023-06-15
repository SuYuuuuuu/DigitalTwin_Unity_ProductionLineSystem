using Google.Protobuf;
using LabProductLine.ControlModule;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace LabProductLine.ControlModule
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



        //-------------------------------------发送数据-------------------------//
        public static void SendData(int robotID,JogCmd jogCmd, Socket client)
        {
            try
            {
                byte[] res = ChangeCmdToByte(robotID,jogCmd);
                client.BeginSend(res, 0, res.Length, SocketFlags.None, EndSendData, client);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

        }


        public static void SendData(int robotID,PtpCmd ptpCmd, float[] pos, Socket client)
        {
            try
            {
                byte[] res = ChangeCmdToByte(robotID, ptpCmd, pos);
                client.BeginSend(res, 0, res.Length, SocketFlags.None, EndSendData, client);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

        }


        public static void SendData(int robotID, bool isOpened, Socket client)
        {
            try
            {
                byte[] res = ChangeCmdToByte(robotID, isOpened);
                client.BeginSend(res, 0, res.Length, SocketFlags.None, EndSendData, client);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

        }


       




        private static void EndSendData(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndSend(ar);
        }



        private static byte[] ChangeCmdToByte(int robotID, bool isOpened)
        {

            byte[] res = new byte[7];
            res[0] = 11;
            res[1] = 5;
            Buffer.BlockCopy(BitConverter.GetBytes(robotID), 0, res, 2, 4);
            res[res.Length - 1] = BitConverter.GetBytes(isOpened)[0];
            return res;

        }
        private static byte[] ChangeCmdToByte(int ID,JogCmd jogcmd)
        {
            //数据类型(1)+数据长度(1)+数据(ID(4) + jogCmd(1))=7字节  这里有个隐患，一个byte最多表示128个数据
            //9--Jog运动指令

            byte[] res = new byte[7];
            res[0] = 9;
            res[1] = 5;
            Buffer.BlockCopy(BitConverter.GetBytes(ID), 0,res,2,4);
            res[res.Length-1] = (byte)jogcmd;
            return res;
        }

        private static byte[] ChangeCmdToByte(int ID,PtpCmd ptpCmd, float[] pos)
        {
            //数据类型(1)+数据长度(1)+数据(4(机械臂ID)+1(ptp模式)+4*4)=23字节
            //10--Jog运动指令
            int count = 7;
            byte[] bytes = new byte[23];
            bytes[0] = 10;
            bytes[1] = 21;
            Buffer.BlockCopy(BitConverter.GetBytes(ID), 0, bytes, 2, 4);
            bytes[6] = (byte)ptpCmd;
            foreach (float item in pos)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(item), 0, bytes, count, 4);//将得到的每个float转换的byte[]添加到原有的字节数组上
                count += 4;
            }
            return bytes;
        }

    }
}