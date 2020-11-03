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
        [SerializeField] private ConfirmWindow confirmWindow;

        private Transaction _transaction;
        
        public void Init(Transaction transaction)
        {
            category.text = transaction._category;
            comment.text = transaction._comment;
            count.text = transaction._count.ToString();
            _transaction = transaction;
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
                    confirmWindow.Open(() =>
                    {
                        var monthlyTransaction = UserDataManager.CurrentMonthlyTransaction;
                        
                        var payment = monthlyTransaction.SelectMany(dailyTransaction => dailyTransaction._transactions)
                            .First(transaction => transaction == _transaction);
                        
                        foreach (var dailyTransaction in monthlyTransaction)
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
    }
}