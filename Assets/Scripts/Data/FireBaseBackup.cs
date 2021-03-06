using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class FireBaseBackup
    {
        public List<Saving> savings = new List<Saving>();
        public CategoryData[] categories = new CategoryData[14];
        public List<YearlyTransaction> _transactions = new List<YearlyTransaction>();
    }
}