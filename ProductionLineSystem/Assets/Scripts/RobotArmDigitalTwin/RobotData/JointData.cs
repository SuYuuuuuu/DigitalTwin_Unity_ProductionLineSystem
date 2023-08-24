
[System.Serializable]
public class JointData:JsonData
{
    public float[] joint;//六轴的角度(单位：弧度)
    public float[] pos;//末端的位置
    public float[] ori;//末端的姿态
}
