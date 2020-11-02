using System;
using DefaultNamespace;
using Tabs;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0649
namespace Managers
{
    public class TabManager : MonoBehaviour
    {
        
        [SerializeField] private Tab _currentTab;
        
        private void Awake()
        {
            Events.OnUpdateTab += UpdateTab;
        }

        public void OpenTab(Tab tab)
        {
            _currentTab?.gameObject.SetActive(false);
            _currentTab = tab;
            tab.Init();
        }

        private void UpdateTab()
        {
            _currentTab.Init();
        }

        private void OnDestroy()
        {
            Events.OnUpdateTab -= UpdateTab;
        }

        public void OnPreviousMonth()
        {
            UserDataManager.SelectedDate = UserDataManager.SelectedDate.AddMonths(-1);
            Events.OnUpdateTab?.Invoke();
        }

        public void OnNextMonth()
        {
            UserDataManager.SelectedDate = UserDataManager.SelectedDate.AddMonths(1);
            Events.OnUpdateTab?.Invoke();
        }
    }
}