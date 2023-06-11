using LabProductLine.ControlModule;
using UnityEngine;

namespace ConnectTest
{

    public class ConnectTest : MonoBehaviour
    {
        private void OnGUI()
        {
            if (GUILayout.Button("连接"))
            {
                Client.Instance.Open("192.168.8.168", 12888);
            }
            if (GUILayout.Button("断开连接"))
            {
                Client.Instance.Close();
            }
        }

    }
}