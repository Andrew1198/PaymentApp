using System;
using Windows.WindowsData.TabData;
using Managers;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649

namespace Windows.HUDs
{
    public class BottomHud : MonoBehaviour
    {
        [SerializeField] private Button accountButton;
        [SerializeField] private Button categoriesButton;
        [SerializeField] private Button transactionsButton;
        [SerializeField] private Button overviewButton;
        public void Awake()
        {
            accountButton.onClick.AddListener(TabManager.OpenTab<AccountTabData>);
            categoriesButton.onClick.AddListener(TabManager.OpenTab<CategoriesTabData>);
            transactionsButton.onClick.AddListener(TabManager.OpenTab<TransactionsTabData>);
            overviewButton.onClick.AddListener(TabManager.OpenTab<OverviewTabData>);
        }

        public void OnDestroy()
        {
            accountButton.onClick.RemoveAllListeners();
            categoriesButton.onClick.RemoveAllListeners();
            transactionsButton.onClick.RemoveAllListeners();
            overviewButton.onClick.RemoveAllListeners();
        }
    }
}