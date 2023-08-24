using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using LabProductLine.RobotArmDigitalTwin;
using UnityEngine;
using System.Linq;


namespace LabProductLine.RobotArmDigitalTwin{
public class AuboClient : MonoBehaviour
{

    private const string SERVER_IP = "127.0.0.1"; // 服务器的IP地址，这里应该填写你Aubo机械臂服务器的IP
    private const int SERVER_PORT = 12345; // 服务器的端口，保持与服务器的端口一致

    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024];

    private AuboController auboController;//获取数据接口中的数组引用
    private TemperatureController temperatureController;//获取温度数据接口的数组引用
    private void Awake()
    {
        auboController = FindObjectOfType<AuboController>();
        temperatureController = FindObjectOfType<TemperatureController>();
    }
    private void Start()
    {
        try
        {
            // 创建TCP客户端，并连接到服务器
            client = new TcpClient(SERVER_IP, SERVER_PORT);
            stream = client.GetStream();

            // 开始异步接收服务器数据
            stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnDataReceived, null);
        }
        catch (Exception e)
        {
            Debug.LogError("连接到服务器失败: " + e.Message);
        }
    }

    private void OnDataReceived(IAsyncResult result)
    {
        try
        {
            int bytesRead = stream.EndRead(result);

            if (bytesRead > 0)
            {
                // 解析收到的数据并更新关节轴状态
                string data = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);
                UpdateJointData(data);

                // 继续异步接收数据
                stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnDataReceived, null);
            }
            else
            {
                // 服务器断开连接
                Debug.LogWarning("与服务器断开连接。");
                client.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("接收数据时发生错误: " + e.Message);
            client.Close();
        }
    }

    private void UpdateJointData(string data)
    {
        // 在这里解析从服务器接收的数据，并更新温度图或其他显示内容
        // 假设服务器发送的数据为逗号分隔的关节轴状态，按照你的数据格式来解析数据
        try
        {
            var json_Data = JsonUtility.FromJson<JsonData>(data);
            if (json_Data.type == "arm_state")
            {
                ArmStatus armStatus = JsonUtility.FromJson<ArmStatus>(data);
                temperatureController.Temp = (float[])armStatus.jointArr_Temperature.Clone();
            }
            else if (json_Data.type == "joint_data")
            {
                JointData joint_data = JsonUtility.FromJson<JointData>(data);
                auboController.RobotJoint = joint_data.joint.Select(num => num*=57.3f).ToArray();//转换为角度
            }

        }
        catch (Exception e)
        {
            Debug.LogError("解析JSON数据时发生错误: " + e.Message);
        }


        // 使用解析后的关节轴状态数据进行显示或其他操作
        // 例如，可以更新温度图、机械臂模型的姿态等
    }

    private void OnDestroy()
    {
        // 关闭TCP连接
        if (client != null && client.Connected)
        {
            client.Close();
        }
    }
}
}

