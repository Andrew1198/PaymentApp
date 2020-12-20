using System;
#pragma warning disable 0649
namespace Data
{
    [Serializable]
    public class CashTransaction : TransactionBase
    {
        public override DateTime Time
        {
            get => DateTime.FromBinary(time);
            set => time = value.ToBinary();
        }
        public string category;

        public CashTransaction(CashTransactionInitData initData): base(initData)
        {
            category = initData.category;
        }

        public class CashTransactionInitData : TransactionBaseInitData
        {
            public string category;
        }
    }
    
}