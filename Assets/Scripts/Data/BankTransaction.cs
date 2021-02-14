using System;
using Managers;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class BankTransaction : TransactionBase
    {
        public string id;
        [SerializeField] private int mcc;
        public override DateTime time
        {
            get =>DateTimeOffset.FromUnixTimeSeconds(_time).LocalDateTime;
            set{}
        }
        public override string category
        {
            get => MonoBankManager.Instance.mccDataBase.GetDescriptionByMccCode(mcc);
            set {}
        }

        public override long amount => (long) Math.Round((float) _amount / 100,
            MidpointRounding.AwayFromZero) * -1;

        public BankTransaction(MonoBankManager.PaymentData data)
        {
            _amount = data.amount;
            description = data.description;
            id = data.id;
            _time = data.time;
            mcc = data.mcc;
            type = TransactionType.Bank;
        }
    }
}