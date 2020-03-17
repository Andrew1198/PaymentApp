using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class MonthlyTransaction
    {
        public List<DailyTransaction> _transactions = new List<DailyTransaction>();
    }
}