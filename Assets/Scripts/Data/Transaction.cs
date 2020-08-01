using System;
using System.Runtime.Serialization;
using UnityEngine;
#pragma warning disable 0649
namespace Data
{
    [Serializable]
    public class Transaction
    {
        public int _count;
        public string _comment;
        [SerializeField] private long _time;
        public DateTime Time
        {
            get => DateTime.FromBinary(_time);
            set => _time = value.ToBinary();
        }
        
        public string wallet;
        public string _category;
    }
}