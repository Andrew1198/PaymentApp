using Data;
using TMPro;
using UnityEngine;
#pragma warning disable 0649
namespace Items
{
    public class DeletedTransactionItem : TransactionItem
    {
        [SerializeField] private TextMeshProUGUI time;
        public override void Init(TransactionBase transaction)
        {
            base.Init(transaction);
            time.text = _transaction.time.ToString("MM/dd/yyyy HH:mm");
        }
    }
}