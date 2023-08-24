
[System.Serializable]
public class ArmStatus:JsonData
{
    public JointStateData joint1;
    public JointStateData joint2;
    public JointStateData joint3;
    public JointStateData joint4;
    public JointStateData joint5;
    public JointStateData joint6;

    public float[] jointArr_Temperature
    {
        get => new float[]{
        joint1.temperature,
        joint2.temperature,
        joint3.temperature,
        joint4.temperature,
        joint5.temperature,
        joint6.temperature
        };
    }
}
