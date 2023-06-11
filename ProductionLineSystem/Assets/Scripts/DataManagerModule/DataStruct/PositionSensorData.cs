using UnityEngine;

namespace LabProductLine.DataManagerModule
{
    [System.Serializable]
    public class PositionSensorData : BaseData
    {
        public int ID;
        public Vector3 position;//放置位置

        public bool detectStatus;
    }
}