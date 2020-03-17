using Tabs;
using UnityEngine;

namespace Managers
{
    public class TabManager : Singleton<TabManager>
    {
        //[SerializeField] private List<GameObject> tabs;

        [SerializeField] private Tab _currentTab;

        public Tab CurrentTab => Instance._currentTab;
           
        
        
        public void OpenTab(Tab tab)
        {
            _currentTab?.gameObject.SetActive(false);
            _currentTab = tab;
            tab.Init();
        }

        public static void UpdateTab()
        {
            Instance._currentTab.Init();
        }
    }
}