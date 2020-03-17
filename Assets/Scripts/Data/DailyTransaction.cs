using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class DailyTransaction
    {
        public int day;
        public List<Transaction> _transactions = new List<Transaction>();
    }
}