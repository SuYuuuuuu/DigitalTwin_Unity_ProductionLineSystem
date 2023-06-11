using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /*
     使用方式：
    1.素有频繁创建/销毁的物体，都通过对象池创建/回收
    2.需要通过对象池创建的物体，如需每次创建时执行，则让脚本实现IResetable接口
     */

    /// <summary>
    /// 可重置接口
    /// </summary>
    public interface IResetable
    {
        void OnReset();
    }
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        private Dictionary<string, List<GameObject>> pool;

        public override void Init()
        {
            base.Init();
            pool = new Dictionary<string, List<GameObject>>();
        }

        public GameObject CreateObject(string key, GameObject prefab, Vector3 position, Quaternion rotate)
        {
            GameObject go = FindUsableObject(key);

            if (go == null)
                go = AddObject(key, prefab);

            UseObject(go, position, rotate);
            return go;

        }

        /// <summary>
        /// 使用对象
        /// </summary>
        /// <param name="go">游戏对象</param>
        /// /// <param name="pos">生成位置</param>
        /// /// <param name="rotate">生成旋转</param>
        private void UseObject(GameObject go, Vector3 pos, Quaternion rotate)
        {
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.SetActive(true);

            //实现重置
            foreach (IResetable item in go.GetComponents<IResetable>())
            {
                item.OnReset();
            }
        }

        /// <summary>
        /// 添加类型，并创建新对象
        /// </summary>
        /// <param name="key">对象类型</param>
        /// <param name="prefab">对象预制体</param>
        /// <returns>游戏对象</returns>
        private GameObject AddObject(string key, GameObject prefab)
        {
            GameObject go = Instantiate(prefab);
            if (!pool.ContainsKey(key))
            {
                pool.Add(key, new List<GameObject>());
            }
            pool[key].Add(go);
            return go;
        }

        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="key">对象类型</param>
        /// <returns></returns>
        private GameObject FindUsableObject(string key)
        {
            if (pool.ContainsKey(key))
                return pool[key].Find(go => go.activeInHierarchy == false);
            return null;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go">回收的对象</param>
        public void CollectionObject(GameObject go)
        {
            go.SetActive(false);
        }

        /// <summary>
        /// 清空某个类别
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            for (int i = pool[key].Count - 1; i >= 0; i--)
            {
                Destroy(pool[key][i]);
            }
            pool.Remove(key);
        }

        /// <summary>
        /// 清除所有类别
        /// </summary>
        public void ClearAll()
        {
            //foreach只读元素
            foreach (var key in new List<string>(pool.Keys))//将字典中所有键存入List集合中
            {
                Clear(key);
            }
        }
    }
}


