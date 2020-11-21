using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using HelperWindows;
using Managers;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DailyTransaction
    {
        public int day;
        public List<Transaction> _transactions = new List<Transaction>();
        public List<BankTransaction> bankTransactions = new List<BankTransaction>();
    }
}