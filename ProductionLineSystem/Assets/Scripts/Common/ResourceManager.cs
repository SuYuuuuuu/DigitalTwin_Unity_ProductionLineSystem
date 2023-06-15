using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class ResourceManager
    {
        private static Dictionary<string, string> configDic;
        private static string configText;
        //
        static ResourceManager()
        {
            //1����ȡ�����ļ�
            string config = GetConfigFile("config.txt");
            //GetConfigFile("config.txt");
            //2�������ļ����ֵ���<string,string>
            BuildMap(config);
        }

        /// <summary>
        /// 从资产中获取配置文件
        /// </summary>
        /// <param name="fileName">�����ļ�����</param>
        /// <returns>�����ļ��ı�</returns>
        public static string GetConfigFile(string fileName)
        {
            string url;
#if UNITY_EDITOR || UNITY_STANDALONE
            url = "file://" + Application.dataPath + "/StreamingAssets/" + fileName;
#elif UNITY_IPHONE
        url = "file://" + Application.dataPath + "/Raw/"+ fileName;
#elif UNITY_ANDROID
        url = "file://" + Application.dataPath + "!/Assets/"+ fileName;
// #else
//         url = new System.Uri(Path.Combine(Application.streamingAssetsPath,string.Format("{0}",fileName))).AbsoluteUri;

#endif
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SendWebRequest();
            while (true)
            {
                if (www.downloadHandler.isDone)
                    return www.downloadHandler.text;
            }
        }

        private static IEnumerator GetConfigFileOfWebGL(string url, string fileName, Action callback)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.downloadHandler.isDone)
            {
                configText = www.downloadHandler.text;
                BuildMap(configText);
                callback?.Invoke();
            }
        }

        /// <summary>
        /// ������Դ���ձ�
        /// </summary>
        /// <param name="configContent">�����ļ��ı�</param>
        public static void BuildMap(string configContent)
        {
            configDic = new Dictionary<string, string>();
            string content;
            //��������
            //����=·��/r/n����=·��/r/n
            using (StringReader reader = new StringReader(configContent))//ʹ��using �����Բ���дreader.Dispose()
            {
                while (((content = reader.ReadLine()) != null))
                {
                    string[] keyValues = content.Split('=');
                    configDic.Add(keyValues[0], keyValues[1]);
                }
            }
        }

        /// <summary>
        /// ���ݶ������ּ��ض���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="prefabName">��������</param>
        /// <returns>����</returns>
        public static T Load<T>(string prefabName) where T : UnityEngine.Object
        {
            string prefabPath = configDic[prefabName];
            //���� ----->·��
            return Resources.Load<T>(prefabPath);
        }
    }
}
