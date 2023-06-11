using System.IO;
using UnityEditor;
using UnityEngine;


public class GenerateResConfig : Editor
{ 
    [MenuItem("Tools/Resources/Generate Config")]
      public static void GenerateConfig()
    {
        //1、找Resources目录下所有预制体完整路径(返回的是Unity自身给每个物体定义的GUID)
        string[] resFiles = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" });
        for (int i = 0; i < resFiles.Length; i++)//转换GUID为路径再转为物体名字
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
            string key = Path.GetFileNameWithoutExtension(resFiles[i]);
            string value = resFiles[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
            resFiles[i] = key + "=" + value;
        }
        //2、生成对应关系{名称=路径}（资源对照表）

        //3、生成配置文件

        File.WriteAllLines("Assets/StreamingAssets/config.txt", resFiles);
        AssetDatabase.Refresh();
    }

}
