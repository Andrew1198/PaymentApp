using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class MonthlyTransaction
    {
        public int month;
        public List<DailyTransaction> _transactions = new List<DailyTransaction>();
    }
}