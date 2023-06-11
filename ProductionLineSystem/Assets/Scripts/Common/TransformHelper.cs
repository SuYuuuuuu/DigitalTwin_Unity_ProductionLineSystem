using UnityEngine;

namespace Common
{
    public static class TransformHelper
    {

        /// <summary>
        /// 层级未知，组件查找类
        /// </summary>
        /// <param name="currentTF"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform GetChildByName(this Transform currentTF, string childName)
        {
            Transform childTF = currentTF.Find(childName);
            if (childTF != null) return childTF;
            for (int i = 0; i < currentTF.childCount; i++)
            {
                childTF = GetChildByName(currentTF.GetChild(i), childName);
                if (childTF != null) return childTF;
            }
            return null;
        }
    }
}
