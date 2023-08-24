using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    public Material[] Axis_Mat;

    public float[] Temp { set => temp = value; get => temp; }
    private float[] temp;


    public void SetTemperature()
    {
        for (int i = 0; i < temp.Length; i++)
        {
            Axis_Mat[i].SetFloat("_Temperature_Axis", temp[i]);
        }

    }

    private void Update()
    {
        if (temp != null)
            SetTemperature();
    }


}
