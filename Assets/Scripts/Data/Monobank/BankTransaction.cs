using System;

namespace Data
{
    [Serializable]
    public class BankTransaction
    {
        public string id;
        public long time;
        public string description;
        public int mcc;
        public long amount;
        public long commissionRate;
    }
}