using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public abstract class TransactionBase
    {
        public TransactionType type;
        [SerializeField] protected long time;
        public abstract DateTime Time { get; set; }
        public long amount;
        public string comment;

        protected TransactionBase(TransactionBaseInitData initData)
        {
            type = initData.type;
            time = initData.time;
            amount = initData.amount;
            comment = initData.comment;
        }
        
        
        public class TransactionBaseInitData
        {
            public TransactionType type;
            public long time;
            public long amount;
            public string comment;
        }
    }
    
    public enum TransactionType
    {
        Cash,
        Bank
    }
}