using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUI.Framework
{
    public delegate void PointEventHandler(PointerEventData eventData);

    /// <summary>
    /// UI�¼������࣬�����UI���������ṩί�з���
    /// </summary>
    public class UIEventListener : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        public event PointEventHandler PointDownHandler;
        public event PointEventHandler PointUpHandler;
        public event PointEventHandler PointClickHandler;
        public event PointEventHandler PointEnterHandler;
        public event PointEventHandler PointExitHandler;
        public event PointEventHandler BeginDragHandler;
        public event PointEventHandler EndDragHandler;
        public event PointEventHandler DragHandler;
        public event PointEventHandler DropHandler;

        public static UIEventListener uIEventListener;
        /// <summary>
        ///���ݱ任�����ȡ�¼�������
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static UIEventListener GetUIEventListener(Transform tf)
        {
            uIEventListener = tf.GetComponent<UIEventListener>();
            if (uIEventListener == null)
            {
                uIEventListener = tf.gameObject.AddComponent<UIEventListener>();
            }
            return uIEventListener;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointClickHandler?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointDownHandler?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointUpHandler?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragHandler?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointEnterHandler?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointExitHandler?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragHandler?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragHandler?.Invoke(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            DropHandler?.Invoke(eventData);
        }
    }
}
