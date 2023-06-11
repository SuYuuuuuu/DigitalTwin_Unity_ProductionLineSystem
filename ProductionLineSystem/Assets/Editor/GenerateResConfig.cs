using System.IO;
using UnityEditor;
using UnityEngine;


public class GenerateResConfig : Editor
{ 
    [MenuItem("Tools/Resources/Generate Config")]
      public static void GenerateConfig()
    {
        //1����ResourcesĿ¼������Ԥ��������·��(���ص���Unity�����ÿ�����嶨���GUID)
        string[] resFiles = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" });
        for (int i = 0; i < resFiles.Length; i++)//ת��GUIDΪ·����תΪ��������
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
            string key = Path.GetFileNameWithoutExtension(resFiles[i]);
            string value = resFiles[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);
            resFiles[i] = key + "=" + value;
        }
        //2�����ɶ�Ӧ��ϵ{����=·��}����Դ���ձ�

        //3�����������ļ�

        File.WriteAllLines("Assets/StreamingAssets/config.txt", resFiles);
        AssetDatabase.Refresh();
    }

}
