using System;
using System.Linq;
using Windows;
using Windows.HelperWindows;
using DefaultNamespace;
using Managers;
using TMPro;
using UnityEngine;


#pragma warning disable 0649
namespace Items
{
    public class TransactionItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI category;
        [SerializeField] private TextMeshProUGUI comment;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private TextMeshProUGUI typeTransaction;
        [SerializeField] private ConfirmWindow confirmWindow;
        
        private TransactionItemData _transactionItemData;

        public void OnHoldActivate()
        {
            WindowsManager.ConfirmWindow(() =>
            {
                var monthlyTransaction = UserDataManager.CurrentMonthlyTransaction;

                var payment = monthlyTransaction.transactions
                    .SelectMany(dailyTransaction => dailyTransaction.GetALlTypeTransactions())
                    .First(transaction => transaction.time == _transactionItemData.Time);

                foreach (var dailyTransaction in monthlyTransaction.transactions)
                    if (dailyTransaction.RemoveTransaction(payment))
                        break;

                TabManager.UpdateOpenedTab();
            });
        }

        public void Init(TransactionItemData transaction)
        {
            category.text = transaction.Category;
            comment.text = transaction.Comment;
            count.text = transaction.Count.ToString();
            _transactionItemData = transaction;
            typeTransaction.text = transaction.IsBankTransaction ? "Bank Transaction" : "Cash Transaction";
        }

        public class TransactionItemData
        {
            public string Category;
            public string Comment;
            public long Count;
            public bool IsBankTransaction;
            public DateTime Time;
        }
    }
}