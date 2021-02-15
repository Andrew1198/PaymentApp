#pragma warning disable 0649
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.HelperWindows;
using Windows.WindowsData;
using Data.WindowData;
using NaughtyAttributes;
using UnityEngine;
namespace Managers
{
    public class WindowsManager : Singleton<WindowsManager>
    {
        [SerializeField] private Transform tabContainer;
        [SerializeField] private Transform fullScreenWindowContainer;
        [SerializeField] private List<WindowBase> windows;
        private readonly Dictionary<Type, WindowBase> _pool = new Dictionary<Type, WindowBase>();

        public override void Awake()
        {
            base.Awake();
            PreloadWindows();
        }

        public static WindowBase OpenWindow<T>(Dictionary<string,object>dynamicData = null) where T: WindowData, new()
        {
            var window = GetOrCreateWindowBehaviour<T>();
            window.Open(dynamicData);
            return window;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static WindowBase GetOrCreateWindowBehaviour<T>() where T : WindowData, new()
        {
            WindowBase window;
            if (!Instance._pool.ContainsKey(typeof(T)))
            {
                var winData = new T();
                
                var dataTypeString = winData.behaviourNamespaceName + "." + typeof(T).Name;
                var monobehaviorType = Type.GetType(dataTypeString.Substring(0,dataTypeString.Length - 4));
                var windowForInstantiate = Instance.windows.FirstOrDefault(w => w.GetType() == monobehaviorType);
                if (windowForInstantiate == null)
                {
                    Debug.LogError("Window doesn't exist");
                    return null;
                }
                
                
                Transform parent;
                switch (winData.type)
                {
                    case WindowType.Tab:
                        parent = Instance.tabContainer;
                        break;
                    case WindowType.FullScreen:
                        parent = Instance.fullScreenWindowContainer;
                        break;
                    default:
                        parent = Instance.fullScreenWindowContainer;
                        break;
                }

                window = Instantiate(windowForInstantiate, parent);
                window.gameObject.name = windowForInstantiate.name; // remove (Clone) from name
                (window.transform as RectTransform).anchorMin = Vector2.zero;
                (window.transform as RectTransform).anchorMax = Vector2.one;
                (window.transform as RectTransform).offsetMax = Vector2.zero;
                (window.transform as RectTransform).offsetMin = Vector2.zero;
                window.SetWindowData(winData);
                Instance._pool[typeof(T)] = window;
            }
            else
                window = Instance._pool[typeof(T)];

            return window;
        }
        
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void CloseWindow<T>() where T: WindowData
        {
            if (!Instance._pool.ContainsKey(typeof(T)))
            {
                Debug.LogError("can't close window, the window never opened");
                return;
            }

            var window = Instance._pool[typeof(T)];
            if (window.state == WindowBase.State.Closed)
            {
                Debug.LogWarning("the window has been already closed");
                return;
            }
            window.Close();
        }

        public static void ConfirmWindow(Action onOk, Action onNo = null)
        {
            var dynamicWindowData = new Dictionary<string, object> {{"OnOkAction", onOk}};
            if (onNo != null)
                dynamicWindowData["OnNoAction"] = onNo;

            OpenWindow<ConfirmWindowData>(dynamicWindowData);
        }

        public static void EnableDisableLoadingWindow(bool value = true)
        {
            if (value)
                OpenWindow<LoadingScreenData>();
            else
             CloseWindow<LoadingScreenData>();
        }

        private void PreloadWindows()
        {
            GetOrCreateWindowBehaviour<LoadingScreenData>().gameObject.SetActive(false);
        }
        
    }
    
    public enum WindowType
    {
        Tab,
        FullScreen 
    }
    
}