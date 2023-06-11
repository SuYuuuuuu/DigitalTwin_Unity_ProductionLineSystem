using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /*
     ʹ�÷�ʽ��
    1.����Ƶ������/���ٵ����壬��ͨ������ش���/����
    2.��Ҫͨ������ش��������壬����ÿ�δ���ʱִ�У����ýű�ʵ��IResetable�ӿ�
     */

    /// <summary>
    /// �����ýӿ�
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
        /// ʹ�ö���
        /// </summary>
        /// <param name="go">��Ϸ����</param>
        /// /// <param name="pos">����λ��</param>
        /// /// <param name="rotate">������ת</param>
        private void UseObject(GameObject go, Vector3 pos, Quaternion rotate)
        {
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.SetActive(true);

            //ʵ������
            foreach (IResetable item in go.GetComponents<IResetable>())
            {
                item.OnReset();
            }
        }

        /// <summary>
        /// ������ͣ��������¶���
        /// </summary>
        /// <param name="key">��������</param>
        /// <param name="prefab">����Ԥ����</param>
        /// <returns>��Ϸ����</returns>
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
        /// ���Ҷ���
        /// </summary>
        /// <param name="key">��������</param>
        /// <returns></returns>
        private GameObject FindUsableObject(string key)
        {
            if (pool.ContainsKey(key))
                return pool[key].Find(go => go.activeInHierarchy == false);
            return null;
        }

        /// <summary>
        /// ���ն���
        /// </summary>
        /// <param name="go">���յĶ���</param>
        public void CollectionObject(GameObject go)
        {
            go.SetActive(false);
        }

        /// <summary>
        /// ���ĳ�����
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
        /// ����������
        /// </summary>
        public void ClearAll()
        {
            //foreachֻ��Ԫ��
            foreach (var key in new List<string>(pool.Keys))//���ֵ������м�����List������
            {
                Clear(key);
            }
        }
    }
}


