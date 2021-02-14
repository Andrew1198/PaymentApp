using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class TransactionBase
    {
        [SerializeField] protected long _time;
        [SerializeField] protected long _amount;
        public string description;
        [SerializeField] protected string _category;
        public TransactionType type;

        public virtual long amount
        {
            get => _amount;
            set => _amount = value;
        }

        public virtual DateTime time
        {
            get;
            set;
        }
        public virtual string category
        {
            get => _category;
            set => _category = value;
        }
        
    }
    [Serializable]
    public enum TransactionType
    {
        Cash,
        Bank
    }
}