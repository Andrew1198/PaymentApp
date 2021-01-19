using System;
using System.Linq;
using DefaultNamespace;
using HelperWindows;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

#pragma warning disable 0649
namespace Items
{
    public class TransactionItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI category;
        [SerializeField] private TextMeshProUGUI comment;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private TextMeshProUGUI typeTransaction;
        [SerializeField] private ConfirmWindow confirmWindow;
        private float _downClickTime;

        private bool _pointerDown;
        private readonly float _requireHold = 1f;

        private TransactionItemData _transactionItemData;

        private void Reset()
        {
            _pointerDown = false;
        }

        private void Update()
        {
            if (_pointerDown)
                if (Time.time >= _downClickTime + _requireHold)
                {
                    confirmWindow.Open(() =>
                    {
                        var monthlyTransaction = UserDataManager.CurrentMonthlyTransaction;

                        var payment = monthlyTransaction._transactions
                            .SelectMany(dailyTransaction => dailyTransaction._transactions)
                            .First(transaction => transaction.Time == _transactionItemData.Time);

                        foreach (var dailyTransaction in monthlyTransaction._transactions)
                            if (dailyTransaction._transactions.Remove(payment))
                            {
                                UserDataManager.Instance.UserData.deletedTransactions.Add(payment);
                                break;
                            }

                        Events.OnUpdateTab?.Invoke();
                    });
                    Reset();
                }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown = true;
            _downClickTime = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerDown = false;
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