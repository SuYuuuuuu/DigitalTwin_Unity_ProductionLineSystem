namespace Common
{

    public class SingletonBase<T> where T : SingletonBase<T>
    {
        private static T instance;

        // 获取单例实例的方法
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = System.Activator.CreateInstance(typeof(T), true) as T;
                }
                return instance;
            }
        }

        // 其他基类方法
        public virtual void BaseMethod()
        {
            // 实现逻辑
        }
    }

}