using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class RobotArmController : MonoBehaviour
{
    [Serializable]
    public class Joint
    {
        public Transform jointTransform;
        public float angle;
        public Vector3 rotationAxis = Vector3.up;
    }

    public Joint[] joints;

    private void OnValidate()
    {
        UpdateJointRotations();
    }

    public void UpdateJointRotations()
    {
        if(joints != null)
        {
            foreach(Joint joint in joints)
            {
                if(joint.jointTransform != null)
                {
                    joint.jointTransform.localRotation = Quaternion.AngleAxis(joint.angle, joint.rotationAxis);
                }
            }
        }
    }
}
