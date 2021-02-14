#pragma warning disable 0649
using Data;
using HelperScripts;
using Items;
using Managers;
using UnityEngine;

namespace Windows.HelperWindows.SettingWindow
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
                var transactionItemData = new TransactionItem.TransactionItemData
                {
                    Category = transaction.category,
                    Comment = transaction.description,
                    Count = transaction.amount,
                    IsBankTransaction = transaction.type == TransactionType.Bank,
                    Time = transaction.time
                };
                transactionItem.Init(transactionItemData);
            }
        }
        
    }
}