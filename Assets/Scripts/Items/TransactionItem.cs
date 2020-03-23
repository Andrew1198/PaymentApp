using System;
using System.Linq;
using Data;
using HelperWindows;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
    public class TransactionItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI wallet;
        [SerializeField] private TextMeshProUGUI category;
        [SerializeField] private TextMeshProUGUI comment;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private ConfirmWindow confirmWindow;

        private Transaction _transaction;
        
        public void Init(Transaction transaction)
        {
            wallet.text = transaction.wallet;
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
                        var monthlyTransaction = PlayerData.CurrentMonthlyTransaction;
                        var deleted = monthlyTransaction.Any(dailyTransaction => dailyTransaction._transactions.Remove(_transaction));
                        if (!deleted)
                            Debug.LogError("Can't delete transaction");
                        TabManager.UpdateTab();
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