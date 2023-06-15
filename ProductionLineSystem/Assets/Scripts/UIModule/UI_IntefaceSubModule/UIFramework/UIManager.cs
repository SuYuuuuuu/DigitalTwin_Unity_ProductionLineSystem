using Common;
using LabProductLine.UIModule;
using System.Collections.Generic;

namespace UGUI.Framework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        /*
         key -----窗口类名        value-----------窗口对象引用
         */
        public List<UIWindow> openingUIWindows;//正在开启的窗户

        private Dictionary<string, UIWindow> uiWindowsDic;



        public override void Init()
        {
            uiWindowsDic = new Dictionary<string, UIWindow>();
            UIWindow[] uiWindows = FindObjectsOfType<UIWindow>();
            for (int i = 0; i < uiWindows.Length; i++)
            {
                uiWindows[i].SetVisible(false);
                AddWindow(uiWindows[i]);
            }
            GetWindow<HomePagePanel>()?.SetVisible(true, 0);
            openingUIWindows = new List<UIWindow>();
        }

        public void AddWindow(UIWindow uiWindow)
        {
            uiWindowsDic.Add(uiWindow.GetType().Name, uiWindow);
        }

        public T GetWindow<T>() where T : class
        {
            if (!uiWindowsDic.ContainsKey(typeof(T).Name)) return null;
            return uiWindowsDic[typeof(T).Name] as T;
        }
    }
}
