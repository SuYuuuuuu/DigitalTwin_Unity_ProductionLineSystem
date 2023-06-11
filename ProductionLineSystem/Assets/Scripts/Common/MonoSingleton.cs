using UnityEngine;


namespace Common
{
    /// <summary>
    /// �ű�������
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        public static T Instance //������ң�ֻ�ڵ�һ�ε���ʱ����ʵ������������ֱ�ӷ���
        {
            get
            {
                if (instance == null)//��ʵ��Ϊ�գ������ڳ����в���
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)//��ʵ����Ϊ�գ���˵����û�й����ڶ����ϣ�����һ��������ظ���
                    {
                        //�����ű���������ִ��Awake��
                        instance = new GameObject("Singleton Of" + typeof(T)).AddComponent<T>();
                    }
                    else
                    {
                        instance.Init();
                    }
                }
                return instance;
            }
        }


        protected void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            Init();
        }

        //��ʼ��
        public virtual void Init()
        {

        }
        /*
         ��ע��
        1.�����ԣ������д���Ψһ�Ķ��󣬼����øö���̳е�ǰ��
        2.���ʹ�ã�
        --�̳�ʱ���봫����������
        --������ű����������У�ͨ���������ͷ���Instance����
         */
    }
}
