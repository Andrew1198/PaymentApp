using Windows.HelperWindows.SettingWindow;
using Data;
using HelperWindows;
using Managers;
using TMPro;
using UnityEngine;
#pragma warning disable 0649
namespace Items
{
    public class DeletedTransactionItem : TransactionItem
    {
        [SerializeField] private ArchivedTransactionsWindow archivedTransactionsWindow;
        [SerializeField] private TextMeshProUGUI time;
        
        public override void Init(TransactionBase transaction)
        {
            base.Init(transaction);
            time.text = _transaction.time.ToString("MM/dd/yyyy HH:mm");
        }

        public override void OnTapHold()
        {
           WindowsManager.ConfirmWindow(() =>
           {
               TransactionUtils.RestoreTransaction(_transaction);
               archivedTransactionsWindow.MakeDeletedTransactionItems();
           });
        }
    }
}