using Common;
using LabProductLine.DataManagerModule;
using LabProductLine.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static DobotRealTimeData.Types;
using Debug = UnityEngine.Debug;

namespace LabProductLine.ControlModule
{
    public class Client : SingletonBase<Client>
    {
        struct ReceiveState//用于传递信息，包括连接的socket对象和字节数组
        {
            public Socket cilentSocket;
            public byte[] buffer;
        }

        private byte[] buffers;
        private List<byte> bufferList = new List<byte>(1024);
        private Socket socket;
        public Socket Socket { get => socket; private set { } }
        //private List<Timer> timerList = new List<Timer>();
        public Client()
        {
            // for (int i = 0; i < 5; i++)
            // {
            //     Timer timer = new Timer(10000);
            //     timer.Elapsed += Timer_Elapsed;
            //     timerList.Add(timer);
            // }
        }

        private bool[] msgBool_Input;
        public bool[] MsgBool_Input { get => msgBool_Input; private set { } }//传输过来的IO信号存储
        private bool[] msgBool_Output;
        public bool[] MsgBool_Output { get => msgBool_Output; private set { } }//传输过来的IO信号存储

        public uint[] MsgUint { get; set; }//传输过来的Uint值，譬如装配体数量


        /*-----------------接收数据----------------------*/

        public void Open(string ip, int port)
        {
            try
            {
                if (socket != null && socket.Connected) return;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.Connect(endPoint);
                if (socket.Connected)
                {
                    Debug.Log("客户端开启成功！");
                    //StartAllTimer();//开启定时器
                    ReceiveData(socket);   //接收数据
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        private void ReceiveData(Socket socket)
        {
            if (!socket.Connected) return;
            buffers = new byte[1024];
            socket.BeginReceive(buffers, 0, buffers.Length, SocketFlags.None, Receive_CallBack, new ReceiveState
            {
                cilentSocket = socket,
                buffer = buffers
            });
        }
        private void Receive_CallBack(IAsyncResult ar)
        {
            ReceiveState receiveState = (ReceiveState)ar.AsyncState;//接收从beginReceive中传递来的参数
            Socket client = receiveState.cilentSocket;
            byte[] buffer = receiveState.buffer;
            int receiveLen = client.EndReceive(ar);
            if (receiveLen > 0)
                bufferList.AddRange(buffer[0..receiveLen]);

            try
            {
                //ProcessData(client);
                ProcessProtobufData(client, receiveLen);
                ReceiveData(client);
            }
            catch (Exception ex)
            {
                Close();
                Debug.LogException(ex);
            }
        }
        public void Close()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;
            }
        }

        private void ProcessProtobufData(Socket client, int len)
        {
            if (client == null || !client.Connected)
                return;

            while (bufferList.Count > 0)
            {
                int dataType = bufferList[0];
                int packageLen = bufferList[1];
                if (bufferList.Count < packageLen + 2) break; //说明接收尚未完全，退出循环继续接受
                if (dataType == 0)//实时数据
                {
                    DobotRealTimeData data = DataHelper.DeserializeRealTimeData(bufferList.ToArray()[2..(2 + packageLen)]);
                    RobotData robotdata = ToolDataManager.Instance.GetRobotDataByID(data.Id);
                    robotdata.EndEffectorPos = data.Pose?.ToArray()[0..4];
                    robotdata.JointAngles = data.Pose?.ToArray()[4..8];
                    robotdata.OperationStatus = data.LiveState == DobotConnectState.Connected ? RobotOperationStatus.working : RobotOperationStatus.waiting;
                    robotdata.alarmState = data.AlarmState;
                    robotdata.EndEffectorSuctionCup = data.EndEffectorSuctionCup.ToArray();
                }
                else if (dataType == 1)//非实时数据
                {
                    DobotNonRealTimeData data = DataHelper.DeserializeNonRealTimeData(bufferList.ToArray()[2..(2 + packageLen)]);
                    RobotData robotdata = ToolDataManager.Instance.GetRobotDataByID(data.Id);
                    robotdata.RobotName = data.Name;
                    robotdata.HomeParams = data.HomeParams?.ToArray();
                    robotdata.JogJointParams = data.JogJointVelocityWithAcceleration?.ToArray();
                    robotdata.JogCoordinateParams = data.JogCoordinateVelocityWithAcceleration?.ToArray();
                    robotdata.JogCommonParams = data.JogCommonVelocityRatioWithAcceleration?.ToArray();
                    robotdata.PTPJointParams = data.PtpJointVelocityWithAcceleration?.ToArray();
                    robotdata.PTPCoordinateParams = data.PtpCoordinateVelocityWithAcceleration?.ToArray();
                    robotdata.PTPCommonParams = data.PtpCommonVelocityRatioWithAcceleration?.ToArray();
                }
                else if (dataType == 2) //输入信号
                {
                    InputSignal data = DataHelper.DeserializeInputSignal(bufferList.ToArray()[2..(2 + packageLen)]);

                    for (int i = 0; i < ToolDataManager.Instance.GetPositionSensorNumber(); i++)
                    {
                        ToolDataManager.Instance.GetPositionSensorDataByID(i).detectStatus = data.InputSignal_[i + 3]; //这里加3是因为PLC中的输入传感器位置信号是从第四个开始计算
                    }
                }
                else if (dataType == 3)
                {
                    OutputSignal data = DataHelper.DeserializeOutputSignal(bufferList.ToArray()[2..(2 + packageLen)]);
                    for (int i = 0; i < ToolDataManager.Instance.GetCylinderNumber(); i++)
                    {
                        //从第二位开始到第六位为气缸
                        ToolDataManager.Instance.GetCylinderDataByID(i).operationStatus = data.OutputSignal_[i + 1] ? CylinderOperationStatus.open : CylinderOperationStatus.close;
                    }
                    ToolDataManager.Instance.GetConveyorDataByID(0).operationStatus = data.OutputSignal_[8] ? ConveyorOperationStatus.open : ConveyorOperationStatus.close;
                    Debug.Log(ToolDataManager.Instance.GetConveyorDataByID(0).operationStatus);
                }

                // long clientDateTime = long.Parse(data.DateTime);
                // double timeSpan = (DateTime.Now.Ticks - clientDateTime) / (double)TimeSpan.TicksPerMillisecond;
                // Debug.Log("当前时间为：" +DateTime.Now.Ticks + "发送时间为："+ clientDateTime + "时间差为:" +   timeSpan + "ms");
                bufferList.RemoveRange(0, 2 + packageLen);//移除已经处理过的数据
            }


        }

        /*  ----------------之前根据自定义数据协议处理数据的函数
                private void ProcessData(Socket client)
                {
                    if (client == null || !client.Connected)
                        return;
                    byte[] receivedBytes;
                    while (bufferList.Count > 4)
                    {
                        if (bufferList[0] == 0xAA)//包头字节
                        {
                            int len = bufferList[2];
                            if (bufferList.Count < len + 4) //数据区尚未接收完整
                            {
                                break;
                            }
                            receivedBytes = new byte[len + 4];
                            //得到完整的数据，复制到ReceiveBytes中进行校验
                            bufferList.CopyTo(0, receivedBytes, 0, len + 4);
                            byte checkSum = GetByteOfPayloadCheckSum_Receive(receivedBytes[3..(receivedBytes.Length - 1)]);
                            if ((checkSum + receivedBytes[receivedBytes.Length - 1]) % 256 == 0)
                            {
                                DataProcess(receivedBytes);
                                bufferList.RemoveRange(0, len + 4);
                            }
                            else
                            {
                                bufferList.RemoveRange(0, len + 4);
                                Debug.Log("数据校验不正确");
                                continue;
                            }
                        }
                        else//帧头不正确
                        {
                            bufferList.RemoveAt(0);
                            //Debug.Log("帧头不正确");
                        }
                    }
                }
                private void DataProcess(byte[] receivedBytes)
                {
                    byte dataType = receivedBytes[1];//数据头，代表了哪种数据类型
                    int len = receivedBytes[2];//负载长度
                    if (len == 0) return;//负载中没有数据
                    switch (dataType)
                    {
                        case 0://byte[]-----string
                            string msgString = Encoding.GetEncoding("GBK").GetString(receivedBytes, 3, len);
                            Debug.Log(msgString);//测试
                            break;

                        case 1://byte[]-----uint[]
                            MsgUint = new uint[len / 4];
                            for (int i = 3, count = 0; i < len + 3; i += 4, count++)
                            {
                                MsgUint[count] = BitConverter.ToUInt32(receivedBytes[i..(i + 4)]);
                            }
                            break;

                        case 2://byte[]-----bool[]
                            byte[] res = new byte[len];
                            int signalType = receivedBytes[3];//信号类型
                            Buffer.BlockCopy(receivedBytes, 4, res, 0, len);
                            switch (signalType)
                            {
                                case 1:
                                    msgBool_Input = Array.ConvertAll(res, value => value == 1 ? true : false);
                                    UpdateInputStatus(msgBool_Input);//更新输入状态信息
                                    break;
                                case 2:
                                    msgBool_Output = Array.ConvertAll(res, value => value == 1 ? true : false);
                                    UpdateOutputStatus(msgBool_Output);//更新输出状态信息
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case 3://byte[]-----float[]
                            int dobotType = receivedBytes[3];
                            switch (dobotType)
                            {
                                case 1:
                                    ChangeByteToFloat(receivedBytes, len, 1);
                                    //重置计时器
                                    ChangeRobotRotationStatus(timerList[dobotType - 1]);
                                    break;
                                case 2:
                                    ChangeByteToFloat(receivedBytes, len, 2);
                                    ChangeRobotRotationStatus(timerList[dobotType - 1]);
                                    break;
                                case 3:
                                    ChangeByteToFloat(receivedBytes, len, 3);
                                    ChangeRobotRotationStatus(timerList[dobotType - 1]);
                                    break;
                                case 4:
                                    ChangeByteToFloat(receivedBytes, len, 4);
                                    ChangeRobotRotationStatus(timerList[dobotType - 1]);
                                    break;
                                case 5:
                                    ChangeByteToFloat(receivedBytes, len, 5);
                                    ChangeRobotRotationStatus(timerList[dobotType - 1]);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;

                    }


                }
                private void UpdateInputStatus(bool[] msgBool_Input)
                {
                    int posSensor_Count = 8;//位置传感器数量
                    for (int i = 0; i < posSensor_Count; i++)//有八个位置传感器的信号，都是按照顺序来的
                    {
                        ToolDataManager.Instance.UpdatePositionSensorDataByID(i, msgBool_Input[i + 3]);
                    }
                    //大机械臂更新运动状态
                    ToolDataManager.Instance.UpdateRobotDataByID(4, default, default, msgBool_Input[12] ? RobotOperationStatus.working : RobotOperationStatus.waiting);
                }
                private void UpdateOutputStatus(bool[] msgBool_Output)
                {
                    int cylinder_Count = 5;//气缸数量
                    for (int i = 0; i < cylinder_Count; i++)
                    {
                        //从接收的数组第二位开始到第六位才是气缸
                        ToolDataManager.Instance.UpdateCylinderDataByID(i, msgBool_Output[i + 1] ? CylinderOperationStatus.open : CylinderOperationStatus.close);
                    }
                    ToolDataManager.Instance.UpdateConveyorDataByID(0, msgBool_Output[8] ? ConveyorOperationStatus.open : ConveyorOperationStatus.close);
                    //这里接收的数组还包含了大机械臂的一些信息，后面再处理****************************************
                }



                private void ChangeByteToFloat(byte[] bytes, int len, int robotIndex)
                {
                    float[] floats = new float[(len - 1) / 4];
                    for (int i = 4, count = 0; i < 3 + len; i += 4, count++)
                    {
                        floats[count] = BitConverter.ToSingle(bytes[i..(i + 4)]);
                        // Debug.Log(count.ToString() + "---" + floats[count].ToString());//测试
                    }
                    //这里因为传输的数据不一样所以分开来
                    if (robotIndex == 5)
                        ToolDataManager.Instance.UpdateRobotDataByID(robotIndex - 1, floats, default, RobotOperationStatus.working);
                    else
                        ToolDataManager.Instance.UpdateRobotDataByID(robotIndex - 1, floats[4..8], floats[0..4], RobotOperationStatus.working);
                    //更新小机械臂关节角度数据,此处可以将末端位置数据传过来
                }
                private void ChangeRobotRotationStatus(Timer timer)//改变机械臂的旋转状态
                {
                    timer.Reset();//重置计时器
                }

                private void Timer_Elapsed(object sender, ElapsedEventArgs e)//检测是否没有服务端发送数据，则改变机械臂状态为静止
                {
                    Timer timer = sender as Timer;
                    int robotIndex = timerList.IndexOf(timer) + 1;//需要考虑索引的问题
                    ToolDataManager.Instance.UpdateRobotDataByID(robotIndex - 1, default, default, RobotOperationStatus.waiting);
                    timer.Stop();
                }
                private void StartAllTimer()//开启所有定时器
                {
                    for (int i = 0; i < timerList.Count; i++)
                    {
                        timerList[i].Start();
                    }
                }
                */

        /*----------------发送数据-------------------------*/
        public void SendData(string msg, Socket client)
        {
            if (msg == null || client == null) return;
            try
            {
                byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(msg);
                byte[] res = new byte[bytes.Length + 4];//为新建字节数组添加数据头
                res[0] = 0xAA;//包头
                res[1] = 0;//数据类型
                res[2] = (byte)bytes.Length;//负载帧长
                Buffer.BlockCopy(bytes, 0, res, 3, bytes.Length);
                res[res.Length - 1] = GetByteOfPayloadCheckSum_Send(bytes);
                client.BeginSend(res, 0, res.Length, SocketFlags.None, EndSendData, client);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

        }//发送数据（字符串类型）
        public void SendData(UInt32[] msg, Socket client)
        {
            if (msg == null || client == null) return;
            try
            {
                byte[] bytes = new byte[msg.Length * 4 + 4]; //包含包头、数据类型、负载帧长、负载、校验位
                bytes[0] = 0xAA;
                bytes[1] = 1;
                bytes[2] = (byte)bytes.Length;
                int count = 3;
                foreach (var item in msg)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(item), 0, bytes, count, 4);//将得到的每个uint转换的byte[]添加到原有的字节数组上
                    count += 4;
                }
                bytes[bytes.Length - 1] = GetByteOfPayloadCheckSum_Send(bytes[3..(bytes.Length - 1)]);
                client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, EndSendData, client);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

        }//发送数据（uint类型）
        public void SendData(bool[] msg, Socket client, int signalType)//signalType为发送信号类型，1为输入，2为输出，0为其他
        {
            if (msg == null || client == null) return;
            try
            {
                byte[] bytes = Array.ConvertAll(msg, value => value ? (byte)1 : (byte)0);//负载
                byte[] res = new byte[bytes.Length + 5];//为新建字节数组添加数据头
                res[0] = 0xAA;//包头
                res[1] = 2;//数据类型
                res[2] = (byte)(bytes.Length + 1);//负载帧长
                res[3] = (byte)signalType;//信号类型
                Buffer.BlockCopy(bytes, 0, res, 4, bytes.Length);
                res[res.Length - 1] = GetByteOfPayloadCheckSum_Send(res[3..(res.Length - 1)]);//校验位

                client.BeginSend(res, 0, res.Length, SocketFlags.None, EndSendData, client);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        public void SendData(float[] msg, Socket client, int robotType)//robotType为机械臂型号标识，共有1、2、3、4；若发送其他浮点数据该位字节为0即可
        {

            if (msg == null || client == null) return;
            try
            {
                byte[] bytes = new byte[msg.Length * 4 + 5];//负载帧第一位为机械臂型号，其余为浮点数;总长为包头(1)+负载帧长(1)+负载帧(1+msg.Length*4)+校验位(1)
                bytes[0] = 0xAA;
                bytes[1] = 3;//数据类型
                bytes[2] = (byte)((msg.Length * 4) + 1);
                bytes[3] = (byte)robotType;
                int count = 4;
                foreach (var item in msg)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(item), 0, bytes, count, 4);//将得到的每个float转换的byte[]添加到原有的字节数组上
                    count += 4;
                }
                bytes[bytes.Length - 1] = GetByteOfPayloadCheckSum_Send(bytes[3..(bytes.Length - 1)]);//最后一位为校验位
                client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, EndSendData, client);//(这里开始发送数据)
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

        }//发送数据（浮点数类型）

        private void EndSendData(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndSend(ar);
        }

        public static byte GetByteOfPayloadCheckSum_Receive(params byte[] bytes)
        {
            byte res = 0;
            if (bytes.Length > 0)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    res += bytes[i];
                }
            }
            return Convert.ToByte(res % 256);
        }
        public static byte GetByteOfPayloadCheckSum_Send(params byte[] bytes)
        {
            byte res = 0;
            if (bytes.Length > 0)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    res += bytes[i];
                }
            }
            return Convert.ToByte(256 - res % 256);

        }
    }
}
