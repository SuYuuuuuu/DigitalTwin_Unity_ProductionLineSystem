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
                //byte head = bufferList[0];
                //if(head!=0xAA)
                //{
                //    bufferList.RemoveAt(0);
                //    continue;
                //}
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
                    robotdata.EndEffectorSuctionCup = data.EndEffectorSuctionCup?.ToArray();
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
    }
}