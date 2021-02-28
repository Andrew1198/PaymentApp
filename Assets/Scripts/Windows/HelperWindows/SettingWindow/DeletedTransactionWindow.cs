#pragma warning disable 0649
using System.Collections.Generic;
using Data;
using HelperScripts;
using Items;
using Managers;
using UnityEngine;

namespace Windows.HelperWindows.SettingWindow
{
    public class DeletedTransactionWindow : WindowBase
    {
        [SerializeField] private DeletedTransactionItem deletedTransactionItemPrefab;
        [SerializeField] private Transform content;

        public override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            base.Open(DynamicWindowData);
            
            content.DeleteChildren();
            foreach (var deletedTransaction in UserDataManager.Instance.UserData.deletedTransactions)
            {
                var deletedTransactionItem = Instantiate(deletedTransactionItemPrefab, content);
                deletedTransactionItem.Init(deletedTransaction.Transaction);
            }
        }
    }
}