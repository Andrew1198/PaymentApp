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

        public override void Awake()
        {
            base.Awake();
            accountButton.onClick.AddListener(OpenTab<AccountTabData>);
            categoriesButton.onClick.AddListener(OpenTab<CategoriesTabData>);
            transactionsButton.onClick.AddListener(OpenTab<TransactionTabData>);
            overviewButton.onClick.AddListener(OpenTab<OverviewTabData>);
        }

        public void OnDestroy()
        {
            accountButton.onClick.RemoveAllListeners();
            categoriesButton.onClick.RemoveAllListeners();
            transactionsButton.onClick.RemoveAllListeners();
            overviewButton.onClick.RemoveAllListeners();
        }

        public void OpenTab<T>() where T : TabData, new()
        {
            _currentTab?.Close();
           _currentTab = WindowsManager.OpenWindow<T>() as Tab;
        }

        public static void UpdateOpenedTab()
        {
           Instance._currentTab?.Open();
        }

        public void OnPreviousMonth()
        {
            UserDataManager.SelectedDate = UserDataManager.SelectedDate.AddMonths(-1);
            if (!TransactionUtils.IsThereTransactionInMonth())
            {
                UserDataManager.SelectedDate = UserDataManager.SelectedDate.AddMonths(1);
                if (UserDataManager.SelectedDate.Month == 1)
                    UserDataManager.YearlyTransactions.RemoveAll(transactions =>
                        transactions.year == UserDataManager.SelectedDate.Year - 1);
                else
                    UserDataManager.CurrentYearlyTransaction.transactions.RemoveAll(transaction =>
                        transaction.month == UserDataManager.SelectedDate.Month - 1);

                return;
            }

            TabManager.UpdateOpenedTab();
        }

        public void OnNextMonth()
        {
            UserDataManager.SelectedDate = UserDataManager.SelectedDate.AddMonths(1);
            if (!TransactionUtils.IsThereTransactionInMonth())
            {
                UserDataManager.SelectedDate = UserDataManager.SelectedDate.AddMonths(-1);
                if (UserDataManager.SelectedDate.Month == 12)
                    UserDataManager.YearlyTransactions.RemoveAll(transactions =>
                        transactions.year == UserDataManager.SelectedDate.Year + 1);
                else
                    UserDataManager.CurrentYearlyTransaction.transactions.RemoveAll(transaction =>
                        transaction.month == UserDataManager.SelectedDate.Month + 1);

                return;
            }

            TabManager.UpdateOpenedTab();        }
    }
}