using System.Collections.Generic;
using Data;
using HelperScripts;
using Items;
using Managers;
using UnityEngine;

namespace HelperWindows
{
    public class DeletedTransactionWindow : MonoBehaviour
    {
        [SerializeField] private TransactionItem transactionItemPrefab;
        [SerializeField] private Transform content;
        public void Init()
        {
            gameObject.SetActive(true);
            content.DeleteChildren();
            foreach (var transaction in UserDataManager.Instance.UserData.deletedTransactions)
            {
                var transactionItem = Instantiate(transactionItemPrefab, content);
                TransactionItem.TransactionItemData transactionItemData;
                if (transaction.type == TransactionType.Bank)
                {
                    transactionItemData = new TransactionItem.TransactionItemData
                    {
                        Count = transaction.amount,
                        Category = MonoBankManager.Instance.mccDataBase.GetDescriptionByMccCode(
                            ((BankTransaction) transaction).mcc),
                        Comment = transaction.comment,
                        Time = transaction.Time,
                        IsBankTransaction = true
                    };
                }
                else
                {
                    transactionItemData = new TransactionItem.TransactionItemData
                    {
                        Count = transaction.amount,
                        Category = ((CashTransaction)transaction).category,
                        Comment = transaction.comment,
                        Time = transaction.Time,
                        IsBankTransaction = false
                    };
                }
                transactionItem.Init(transactionItemData);
            }
        }
        
    }
}