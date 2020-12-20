using System;

namespace Data
{
    [Serializable]
    public class BankTransaction : TransactionBase
    {
        public string id;
        public int mcc;
        
        public override DateTime Time
        {
            get => DateTimeOffset.FromUnixTimeSeconds(time).LocalDateTime;
            set {}
        }

        public BankTransaction(BankTransactionInitData initData) : base(initData)
        {
            mcc = initData.mcc;
            id = initData.id;
        }

        public class BankTransactionInitData : TransactionBaseInitData
        {
            public string id;
            public int mcc;
        }
    }
}