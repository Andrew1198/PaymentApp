using System;

namespace Data
{
    [Serializable]
    public class CurrencyInfo
    {
        public int currencyCodeA;
        public int currencyCodeB;
        public float rateSell;
        public float rateBuy;
    }
}