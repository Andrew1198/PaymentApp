using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class YearlyTransactions
    {
        public int year;
        public List<MonthlyTransaction> transactions = new List<MonthlyTransaction>();
    }
}