using System;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class UserData
    {
        public List<Saving> savings = new List<Saving>();
        public CategoryData[] categories = new CategoryData[14];
        public List<YearlyTransaction> transactions = new List<YearlyTransaction>();
        public List<ArchivedTransaction> archivedTransactions = new List<ArchivedTransaction>();
        public MonobankData monobankData = new MonobankData();

        public UserData()
        {
            for (var i = 0; i < categories.Length; ++i)
            {
                categories[i] = new CategoryData();
                categories[i].NumberOfPlace = i;
            }
        }
    }
    
}