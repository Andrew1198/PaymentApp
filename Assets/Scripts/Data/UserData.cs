﻿using System;
using System.Collections.Generic;
using Managers;

namespace Data
    {
        [Serializable]
        public class UserData
        {
            public List<Saving> savings = new List<Saving>();
            public CategoryData[] categories = new CategoryData[14];
            public List<YearlyTransactions> _transactions = new List<YearlyTransactions>();
            public MonobankData monobankData = new MonobankData();
            public UserData()
            {
                for(int i=0;i<categories.Length;++i)
                {
                    categories[i] = new CategoryData();
                    categories[i].NumberOfPlace = i;
                }
            }
        }
    }
