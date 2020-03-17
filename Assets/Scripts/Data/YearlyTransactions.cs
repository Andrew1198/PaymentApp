using System;

namespace Data
{
    [Serializable]
    public class YearlyTransactions
    {
        public int year;
        public MonthlyTransaction[] _monthlyTransactions = new MonthlyTransaction[12];

        public MonthlyTransaction this[int index] => _monthlyTransactions[index];
    }
}