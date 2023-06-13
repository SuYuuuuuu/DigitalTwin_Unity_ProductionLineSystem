using LabProductLine.DataManagerModule;
using UnityEngine;

namespace LabProductLine.UIModule
{
    public class RobotControlEx:MonoBehaviour
    {
        public RobotInfo robotInfo;



        public void OnStateChanged(int id, RobotOperationStatus value)
        {
            //这样写的话就需要在Inspector面板按照顺序一一对应填入信息,并且要满足GetRobotDataNumber()的数量要求，最少五个
            robotInfo.robotWorkState.text = string.Format("机械臂连接状态: {0}",
                value == RobotOperationStatus.working ? "<color=#1EFF00>已连接</color>" : "<color=#F84545>未连接</color>");
        }

        public void OnIdChanged(int id, int value)
        {
            robotInfo.robotID.text = string.Format("机械臂Id: {0}", value);
        }

        public void OnNameChanged(int id, string value)
        {
            robotInfo.robotName.text = string.Format("机械臂名字: {0}", value);
        }
    }
}
