using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ScriptObjectPool
{
    [MenuItem("ScriptableObject/CreateDobotAnimation")]
    public static void CreateDobotAnimation()
    {
        //创建数据资源文件
        DobotAnimation anim = ScriptableObject.CreateInstance<DobotAnimation>();

        //
        AssetDatabase.CreateAsset(anim,"Assets/Resources/ScriptObject/DobotAniamtion.asset");

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();


    }
}
