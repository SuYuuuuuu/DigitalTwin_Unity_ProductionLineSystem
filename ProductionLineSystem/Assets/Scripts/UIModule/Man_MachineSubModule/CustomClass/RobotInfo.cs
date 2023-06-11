using TMPro;

namespace LabProductLine.UIModule
{
    [System.Serializable]
    public class RobotInfo
    {
        public TMP_Text robotName;
        public TMP_Text robotID;
        public TMP_Text robotWorkState;
        public TMP_Text robotAlarmState;
        public TMP_Text robotSuctionCupState;
        public TMP_Text robotPos_EndPos;
        public TMP_Text robotPos_JointAngles;
        public TMP_Text robotHomeParams;
        public TMP_Text robotJogJointParams;
        public TMP_Text robotJogCooParams;
        public TMP_Text robotJogCommonParams;
        public TMP_Text robotPtpJointParams;
        public TMP_Text robotPtpCooParams;
        public TMP_Text robotPtpCommonParams;


    }
}