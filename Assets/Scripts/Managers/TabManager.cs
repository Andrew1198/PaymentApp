using Windows;
using DefaultNamespace;
using HelperWindows;
using UnityEngine;

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

        private void OnDestroy()
        {
            Events.OnUpdateTab -= UpdateTab;
        }

        public void OpenTab(Tab tab)
        {
            _currentTab?.gameObject.SetActive(false);
            _currentTab = tab;
            
        }

        private void UpdateTab()
        {
           
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

            Events.OnUpdateTab?.Invoke();
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

            Events.OnUpdateTab?.Invoke();
        }
    }
}