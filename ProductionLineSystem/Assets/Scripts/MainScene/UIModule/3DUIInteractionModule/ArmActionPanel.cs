using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmActionPanel : MonoBehaviour
{
    public float completeTime = 0f;
    public float remainTime = 0f;

    public int curActionId = -1;
    public string curActionDescription;

    public int nextActionId = -1;
    public string nextActionDescription;

    public List<float> axisJoint;

    private void Start() {
        axisJoint = new List<float>(6);
    }

}
