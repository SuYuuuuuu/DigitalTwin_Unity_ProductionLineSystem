using UnityEngine;


namespace Common
{
    /// <summary>
    /// 脚本单例类
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        public static T Instance //按需查找，只在第一次调用时创建实例，后续调用直接返回
        {
            get
            {
                if (instance == null)//若实例为空，则先在场景中查找
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)//若实例仍为空，则说明类没有挂载在对象上，创建一个对象挂载该类
                    {
                        //创建脚本对象（立即执行Awake）
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

        //初始化
        public virtual void Init()
        {

        }
        /*
         备注：
        1.适用性：场景中存在唯一的对象，即可让该对象继承当前类
        2.如何使用：
        --继承时必须传递子类类型
        --在任意脚本生命周期中，通过子类类型访问Instance属性
         */
    }
}
