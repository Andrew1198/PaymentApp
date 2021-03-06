#pragma warning disable 0649
using System.Collections.Generic;
using Data;
using HelperScripts;
using Items;
using Managers;
using UnityEngine;

namespace Windows.HelperWindows.SettingWindow
{
    public class ArchivedTransactionsWindow : WindowBase
    {
        [SerializeField] private DeletedTransactionItem deletedTransactionItemPrefab;
        [SerializeField] private Transform content;

        public override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            base.Open(DynamicWindowData);
            MakeDeletedTransactionItems();
        }

        public void MakeDeletedTransactionItems()
        {
            content.DeleteChildren();
            foreach (var deletedTransaction in UserDataManager.Instance.UserData.archivedTransactions)
            {
                var deletedTransactionItem = Instantiate(deletedTransactionItemPrefab, content);
                deletedTransactionItem.Init(deletedTransaction.TransactionBase);
            }
        }
    }
}