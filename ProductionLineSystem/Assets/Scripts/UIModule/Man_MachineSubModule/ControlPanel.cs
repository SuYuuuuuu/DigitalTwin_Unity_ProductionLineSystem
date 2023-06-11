using LabProductLine.ControlModule;
using LabProductLine.DataManagerModule;
using TMPro;
using UGUI.Framework;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LabProductLine.UIModule
{

    public class ControlPanel : UIWindow
    {
        public Slider[] sliders;
        public RobotOperationStatus state; //��ǰ����е�۵�����״̬
        protected override void Start()
        {
            base.Start();
            UIManager.Instance.GetWindow<ControlPagePanel>().onControlPanelOpen += ControlPanel_onControlPanelOpen;//ע���¼������ڴ��ݵ�ǰ��е����Ϣ
            GetUIEventListener("CloseBtn").PointClickHandler += OnControlPanelClosed;
            foreach (Slider item in sliders)
            {
                TMP_Text text = item.transform.Find("Value").GetComponent<TMP_Text>();
                item.onValueChanged.AddListener((value) => OnSlidersValueChanged(value, item, text));
            }
        }

        private void ControlPanel_onControlPanelOpen(PointerEventData obj)
        {
            
        }

        private void OnSlidersValueChanged(float value, Slider item, TMP_Text text)
        {
            switch (item.name)
            {
                case "JogJointParams":
                    text.text = string.Format("{0}��/s", value);
                    break;
                case "JogCooParams":
                    text.text = string.Format("{0}��/s", value);
                    break;
                case "JogCommonParams":
                    text.text = string.Format("{0}��/s", value);
                    break;
                case "PTPJointParams":
                    text.text = string.Format("{0}%", value);
                    break;
                case "PTPCooParams":
                    text.text = string.Format("{0}%", value);
                    break;
                case "PTPCommonParams":
                    text.text = string.Format("{0}%", value);
                    break;
            }
        }

        private void OnControlPanelClosed(PointerEventData eventData)
        {
            this.SetVisible(false);
        }

        /// <summary>
        /// ������Ϣ����е����λ�������²���
        /// </summary>
        private void ChangeParamsToServer()
        {
            if (!Client.Instance.Socket.Connected || state == RobotOperationStatus.waiting) return;

        }
    }
}