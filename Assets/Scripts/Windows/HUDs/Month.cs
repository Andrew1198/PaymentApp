using HelperWindows;
using Managers;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649

namespace Windows.HUDs
{
    public class Month : MonoBehaviour
    {
        [SerializeField] private Button onPreviousMonthButton;
        [SerializeField] private Button onNextMonthButton;

        private void Awake()
        {
            onPreviousMonthButton.onClick.AddListener(OnPreviousMonth);
            onNextMonthButton.onClick.AddListener(OnNextMonth);
        }

        private void OnDestroy()
        {
            onPreviousMonthButton.onClick.RemoveAllListeners();
            onNextMonthButton.onClick.RemoveAllListeners();
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