using UnityEngine;


namespace Common
{
    /// <summary>
    /// Unity单例模式
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        internal static T instance;
        public static T Instance //单例
        {
            get
            {
                if (instance == null)//如果实例为null则寻找场景中的实例
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)//如果场景中不存在实例，则手动创建一个物体并添加相应组件
                    {
                        //命名自定义
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


        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            Init();
        }

        //虚初始化方法
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
