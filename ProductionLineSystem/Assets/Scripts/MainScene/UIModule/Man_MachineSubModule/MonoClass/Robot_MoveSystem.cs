using System.Net.Sockets;
using LabProductLine.ControlModule;
using UnityEngine;

namespace LabProductLine.UIModule
{


    public class Robot_MoveSystem : MonoBehaviour
    {
        private Socket socket;
        private int robotID;
        private bool isOpened = true;
        public int RobotID{get => robotID;set => robotID=value;}


        private void OnEnable() {
            socket = Client.Instance.Socket;
        }
        private void Update()
        {
            InputControl();
        }


        private void InputControl()
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                DataHelper.SendData(robotID,JogCmd.AP_DOWN, socket);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                DataHelper.SendData(robotID, JogCmd.IDLE, socket);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                DataHelper.SendData(robotID,JogCmd.AN_DOWN,socket);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                DataHelper.SendData(robotID, JogCmd.IDLE, socket);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                DataHelper.SendData(robotID,JogCmd.BP_DOWN,socket);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                DataHelper.SendData(robotID, JogCmd.IDLE, socket);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                DataHelper.SendData(robotID,JogCmd.BN_DOWN,socket);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                DataHelper.SendData(robotID, JogCmd.IDLE, socket);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DataHelper.SendData(robotID,JogCmd.DP_DOWN,socket);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                DataHelper.SendData(robotID, JogCmd.IDLE, socket);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                DataHelper.SendData(robotID,JogCmd.DN_DOWN,socket);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                DataHelper.SendData(robotID, JogCmd.IDLE, socket);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DataHelper.SendData(robotID,JogCmd.CP_DOWN,socket);
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                DataHelper.SendData(robotID, JogCmd.IDLE, socket);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                DataHelper.SendData(robotID,JogCmd.CN_DOWN,socket);
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                DataHelper.SendData(robotID, JogCmd.IDLE, socket);
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                DataHelper.SendData(robotID, isOpened,socket);
                isOpened = !isOpened;
            }

        }

    }



}