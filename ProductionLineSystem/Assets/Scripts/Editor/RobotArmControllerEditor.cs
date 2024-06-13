using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/*
 * FileName:    RobotArmControllerEditor
 * Author:      #AUTHORNAME#
 * CreateTime:  #CREATETIME#
 * Description:  用于在Inspector面板调节初始机械臂的角度
 *
*/



namespace MyCommonNs
{
    [CustomEditor(typeof(RobotArmController))]
    public class RobotArmControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RobotArmController controller = (RobotArmController)target;

            if (controller.joints != null)
            {
                foreach (var joint in controller.joints)
                {
                    if (joint.jointTransform != null)
                    {
                        joint.angle = EditorGUILayout.Slider($"{joint.jointTransform.name} Angle", joint.angle, -180f, 180f);
                    }
                }
            }

            if (GUI.changed)
            {
                controller.UpdateJointRotations();
                EditorUtility.SetDirty(controller);
            }
        }
    }
}


