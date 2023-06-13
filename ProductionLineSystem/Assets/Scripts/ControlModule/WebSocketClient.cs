using BestHTTP.WebSocket;
using Common;
using LabProductLine.DataManagerModule;
using LabProductLine.Protobuf;
using LabProductLine.UIModule;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UGUI.Framework;
using UnityEngine;
using static DobotRealTimeData.Types;

namespace LabProductLine.ControlModule
{

    public class WebSocketClient : SingletonBase<WebSocketClient>
    {
        private WebSocket webSocket;
        private List<byte> bufferList = new List<byte>(1024);

        public void Init(string uri = null)
        {
            webSocket = new WebSocket(new Uri(uri));
            webSocket.OnOpen += OnWebSocketOpen;
            webSocket.OnMessage += OnMessageReceived;
            webSocket.OnClosed += OnWebSocketClose;
            webSocket.OnError += OnError;
            webSocket.OnBinary += OnBinaryReceived;
        }


        private void OnBinaryReceived(WebSocket webSocket, byte[] data)
        {
            if (data != null) bufferList.AddRange(data);
            ProcessProtobufData(webSocket);
        }

        private void antiInit()
        {
            webSocket.OnOpen = null;
            webSocket.OnMessage = null;
            webSocket.OnError = null;
            webSocket.OnClosed = null;
            webSocket = null;
        }

        public void Connect()
        {
            webSocket.Open();
        }

        public void Send(string msg)
        {
            webSocket.Send(msg);
        }

        public void Send(byte[] buf)
        {
            webSocket.Send(buf);
        }

        public void Close()
        {
            webSocket.Close();
        }




        private void OnMessageReceived(WebSocket webSocket, string message)
        {
            Debug.Log(message);
            GameObject go = GameObjectPool.Instance.CreateObject("MsgEx", ResourceManager.Load<GameObject>("MsgEx"), Vector3.zero, Quaternion.identity);
            Transform content = UIManager.Instance.GetWindow<NotificationPagePanel>().content;
            go.transform.GetComponent<TMP_Text>().text = message;
            go.transform.SetParent(content);
        }

        private void OnError(WebSocket webSocket, string reason)
        {
            Debug.LogError("Error: " + reason);
        }

        private void OnWebSocketClose(WebSocket webSocket, ushort code, string message)
        {
            Debug.Log("WebSocket is now Closed!");
            antiInit();
            Init();
        }

        private void OnWebSocketOpen(WebSocket webSocket)
        {
            Debug.Log("WebSocket is now Open!");
        }

        private void OnDestroy()
        {
            if (webSocket != null && webSocket.IsOpen)
            {
                webSocket.Close();
                antiInit();
            }
        }


        private void ProcessProtobufData(WebSocket webSocket)
        {
            if (webSocket.State == WebSocketStates.Closed)
                return;

            while (bufferList.Count > 0)
            {
                int dataType = bufferList[0];
                int packageLen = bufferList[1];
                if (bufferList.Count < packageLen + 2) break; //说明接收尚未完全，退出循环继续接受
                if (dataType == 0)//实时数据
                {
                    DobotRealTimeData data = DataHelper.DeserializeRealTimeData(bufferList.ToArray()[2..(2 + packageLen)]);
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
                bufferList.RemoveRange(0, 2 + packageLen);//移除已经处理过的数据
            }


        }
    }
}