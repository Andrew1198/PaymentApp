using System;
using Windows;
using Windows.Tabs;
using Windows.WindowsData;
using Windows.WindowsData.TabData;
using Data.WindowData;
using DefaultNamespace;
using HelperWindows;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Managers
{
    public class TabManager : Singleton<TabManager>
    {
        [Header("HUD Buttons")]
        [SerializeField] private Button accountButton; 
        [SerializeField] private Button categoriesButton; 
        [SerializeField] private Button transactionsButton; 
        [SerializeField] private Button overviewButton; 
        private Tab _currentTab;
        
        public void Start()
        {
            OpenTab<TransactionsTabData>();
        }
        
        public static void OpenTab<T>() where T : TabData, new()
        {
            Instance._currentTab?.Close();
            Instance._currentTab = WindowsManager.OpenWindow<T>() as Tab;
        }

        public static void UpdateOpenedTab()
        {
           Instance._currentTab?.Open();
        }
    }
}