using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAngleUI : MonoBehaviour
{
    private Camera cam;
    public Transform UIPoint;
    private void OnEnable()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.position =UIPoint.position;
        transform.forward = cam.transform.forward;
    }
}
