using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class FireBaseBackup
    {
        public List<Saving> savings = new List<Saving>();
        public CategoryData[] categories = new CategoryData[14];
        public List<YearlyTransactions> _transactions = new List<YearlyTransactions>();
    }
}