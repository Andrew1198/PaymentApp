using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class YearlyTransaction
    {
        public int year;
        public List<MonthlyTransaction> transactions = new List<MonthlyTransaction>();
    }
}