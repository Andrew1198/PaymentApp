using System;
using System.Linq;
using Data;
using DefaultNamespace;
using HelperWindows;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        
        private TransactionItemData _transactionItemData;
        
        public void Init(TransactionItemData transaction)
        {
            category.text = transaction.Category;
            comment.text = transaction.Comment;
            count.text = transaction.Count.ToString();
            _transactionItemData = transaction;
            typeTransaction.text = transaction.IsBankTransaction ? "Bank Transaction" : "Cash Transaction";
        }

        private bool _pointerDown;
        private float _downClickTime;
        private float _requireHold = 1f;
        private void Update()
        {
            if (_pointerDown)
            {
                if (Time.time >= _downClickTime + _requireHold)
                {
                    if(_transactionItemData.IsBankTransaction) // удалять можем только транзакции за наличные
                        return;
                    confirmWindow.Open(() =>
                    {
                        var monthlyTransaction = UserDataManager.CurrentMonthlyTransaction;
                        
                        var payment = monthlyTransaction._transactions.SelectMany(dailyTransaction => dailyTransaction._transactions)
                            .First(transaction => transaction.Time == _transactionItemData.Time);
                        
                        foreach (var dailyTransaction in monthlyTransaction._transactions)
                          if(dailyTransaction._transactions.Remove(payment))
                              break;
                        
                        Events.OnUpdateTab?.Invoke();
                    });
                    Reset();
                }
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

        private void Reset()
        {
            _pointerDown = false;
        }

        public class TransactionItemData
        {
            public long Count;
            public string Comment;
            public DateTime Time;
            public string Category;
            public bool IsBankTransaction;
        }
    }
}