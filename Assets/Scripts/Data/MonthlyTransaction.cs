using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class MonthlyTransaction
    {
        public int month;
        public List<DailyTransaction> transactions = new List<DailyTransaction>();
    }
}