using System;
using System.Linq;
using Windows;
using Windows.HelperWindows;
using Data;
using DefaultNamespace;
using HelperWindows;
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
        
        
        protected TransactionBase _transaction;

        public virtual void OnTapHold()
        {
            WindowsManager.ConfirmWindow(() =>
            {
                TransactionUtils.DeleteTransaction(_transaction);
                TabManager.UpdateOpenedTab();
            });
        }

        public virtual void Init(TransactionBase transaction)
        {
            category.text = transaction.category;
            comment.text = transaction.description;
            count.text = transaction.amount.ToString();
            _transaction = transaction;
            typeTransaction.text = transaction.type == TransactionType.Bank ? "Bank TransactionBase" : "Cash TransactionBase";
        }
    }
}