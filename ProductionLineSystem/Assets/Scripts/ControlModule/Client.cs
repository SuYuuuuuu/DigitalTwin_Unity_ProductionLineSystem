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
        private List<byte> bufferList;
        private Socket socket;
        public Socket Socket { get => socket; private set { } }

        public Dictionary<int,RobotOperationStatus> robot2LastState_Dic;
        public event Action<int> RobotConnected;


        private bool[] msgBool_Input;
        public bool[] MsgBool_Input { get => msgBool_Input; private set { } }//传输过来的IO信号存储
        private bool[] msgBool_Output;
        public bool[] MsgBool_Output { get => msgBool_Output; private set { } }//传输过来的IO信号存储

        public uint[] MsgUint { get; set; }//传输过来的Uint值，譬如装配体数量


        public Client()
        {
            bufferList = new List<byte>(1024);
            robot2LastState_Dic = new Dictionary<int, RobotOperationStatus>();
        }

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
                     if(!robot2LastState_Dic.ContainsKey(data.Id))//这里涉及到代码顺序问题，应该是先被触发后订阅UI更新事件，再进行赋值
                    {
                        robot2LastState_Dic.Add(data.Id,RobotOperationStatus.waiting);//默认为断开
                    }
                    //如果之前的状态为断开，当前的状态为连接，则触发事件
                    if(robot2LastState_Dic[data.Id]==RobotOperationStatus.waiting && data.LiveState == DobotConnectState.Connected)
                        UnityMainThreadDispatcher.Instance.Enqueue(() => RobotConnected?.Invoke(data.Id));
                    RobotData robotdata  = DataManager.Instance.GetDataById<RobotData>(data.Id);
                    if(robotdata==null) robotdata = DataManager.Instance.AddData<RobotData>(data.Id,new RobotData()); //若不存在则创建
                    robotdata.EndEffectorPos = data.Pose?.ToArray()[0..4];
                    robotdata.JointAngles = data.Pose?.ToArray()[4..8];
                    robotdata.OperationStatus = data.LiveState == DobotConnectState.Connected ? RobotOperationStatus.working : RobotOperationStatus.waiting;
                    robotdata.alarmState = data.AlarmState;
                    robotdata.EndEffectorSuctionCup = data.EndEffectorSuctionCup.ToArray();
                }
                else if (dataType == 1)//非实时数据
                {
                    DobotNonRealTimeData data = DataHelper.DeserializeNonRealTimeData(bufferList.ToArray()[2..(2 + packageLen)]);
                    RobotData robotdata  = DataManager.Instance.GetDataById<RobotData>(data.Id);
                    if(robotdata==null) robotdata = DataManager.Instance.AddData<RobotData>(data.Id,new RobotData()); //若不存在则创建
                    robotdata.RobotName = data.Name;
                    robotdata.HomeParams = data.HomeParams?.ToArray();
                    robotdata.JogJointParams = data.JogJointVelocityWithAcceleration?.ToArray();
                    robotdata.JogCoordinateParams = data.JogCoordinateVelocityWithAcceleration?.ToArray();
                    robotdata.JogCommonParams = data.JogCommonVelocityRatioWithAcceleration?.ToArray();
                    robotdata.PTPJointParams = data.PtpJointVelocityWithAcceleration?.ToArray();
                    robotdata.PTPCoordinateParams = data.PtpCoordinateVelocityWithAcceleration?.ToArray();
                    robotdata.PTPCommonParams = data.PtpCommonVelocityRatioWithAcceleration?.ToArray();
                }
                else if(dataType==2) //传送带数据
                {
                    Conveyor_Data data =  DataHelper.DeserializeConveyorData(bufferList.ToArray()[2..(2 + packageLen)]);
                    ConveyorData conveyorData  = DataManager.Instance.GetDataById<ConveyorData>(data.Id);
                    if(conveyorData==null) conveyorData = DataManager.Instance.AddData<ConveyorData>(data.Id,new ConveyorData()); //若不存在则创建
                    conveyorData.operationStatus = data.IsOpened? ConveyorOperationStatus.open:ConveyorOperationStatus.close;
                }
                else if(dataType==3)//气缸数据
                {
                    Cylinder_Data data =  DataHelper.DeserializeCylinderData(bufferList.ToArray()[2..(2 + packageLen)]);
                    CylinderData cylinderData  = DataManager.Instance.GetDataById<CylinderData>(data.Id);
                    if(cylinderData==null) cylinderData = DataManager.Instance.AddData<CylinderData>(data.Id,new CylinderData()); //若不存在则创建
                    cylinderData.operationStatus = data.IsOpened?CylinderOperationStatus.open:CylinderOperationStatus.close;
                }
                else if(dataType==4)//传感器数据--位置数据
                {
                    PositionSensor_Data data =  DataHelper.DeserializePositionSensorData(bufferList.ToArray()[2..(2 + packageLen)]);
                    PositionSensorData sensorData  = DataManager.Instance.GetDataById<PositionSensorData>(data.Id);
                    if(sensorData==null) sensorData = DataManager.Instance.AddData<PositionSensorData>(data.Id,new PositionSensorData()); //若不存在则创建
                    sensorData.detectStatus = data.IsActived;
                    Debug.Log("传感器ID为：" + sensorData.ID.ToString() + sensorData.detectStatus);
                }
                // long clientDateTime = long.Parse(data.DateTime);
                // double timeSpan = (DateTime.Now.Ticks - clientDateTime) / (double)TimeSpan.TicksPerMillisecond;
                // Debug.Log("当前时间为：" +DateTime.Now.Ticks + "发送时间为："+ clientDateTime + "时间差为:" +   timeSpan + "ms");
                bufferList.RemoveRange(0, 2 + packageLen);//移除已经处理过的数据
            }


        }


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
