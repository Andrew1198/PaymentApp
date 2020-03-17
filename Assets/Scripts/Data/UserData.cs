using System;
using System.Collections.Generic;

namespace Data
    {
        [Serializable]
        public class UserData
        {
            public List<Wallet> _wallets = new List<Wallet>();
            public List<YearlyTransactions> _transactions = new List<YearlyTransactions>();
        }
    }
