﻿using System;
using System.Collections.Generic;
using Managers;

namespace Data
    {
        [Serializable]
        public class UserData
        {
            public List<Wallet> _wallets = new List<Wallet>();
            public CategoryData[] categories = new CategoryData[14];
            public List<YearlyTransactions> _transactions = new List<YearlyTransactions>();
            public CurrencyInfo[] currenciesRate = {new CurrencyInfo()};

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
