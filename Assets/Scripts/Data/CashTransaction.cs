using System;
using UnityEngine;

#pragma warning disable 0649
namespace Data
{
    [Serializable]
    public class CashTransaction : TransactionBase
    {
        public override DateTime time
        {
            get => DateTime.FromBinary(_time);
            set => _time = value.ToBinary();
        }
    }
    
}