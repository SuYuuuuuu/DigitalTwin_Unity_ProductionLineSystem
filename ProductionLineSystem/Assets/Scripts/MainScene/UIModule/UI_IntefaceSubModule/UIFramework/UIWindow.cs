using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGUI.Framework
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIWindow : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private Dictionary<string, UIEventListener> UIEventListenerDic;
        protected virtual void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            UIEventListenerDic = new Dictionary<string, UIEventListener>();
        }

        public void SetVisible(bool state, float delay = 0)//�����������
        {
            StartCoroutine(SetVisibleDelay(state, delay));
        }

        private IEnumerator SetVisibleDelay(bool state, float delay)
        {
            yield return new WaitForSeconds(delay);
            canvasGroup.alpha = state ? 1 : 0;
            canvasGroup.interactable = state ? true : false;
            canvasGroup.blocksRaycasts = state ? true : false;
        }

        /// <summary>
        /// 根据名字获取UI事件监听器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UIEventListener GetUIEventListener(string name)
        {
            if (!UIEventListenerDic.ContainsKey(name))
            {
                Transform tf = transform.GetChildByName(name);
                UIEventListener uIEventListener = UIEventListener.GetUIEventListener(tf);
                UIEventListenerDic.Add(name, uIEventListener);
                return UIEventListenerDic[name];
            }
            return UIEventListenerDic[name];
        }
    }
}
