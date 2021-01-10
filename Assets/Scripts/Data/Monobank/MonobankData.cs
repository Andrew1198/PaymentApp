using System;
using Managers;

namespace Data
{
    [Serializable]
    public class MonobankData
    {
        public CurrencyInfo[] currenciesRate =
        {
            new CurrencyInfo
            {
                currencyCodeA = (int) MonoBankManager.CurrencyCode.USD,
                currencyCodeB = (int) MonoBankManager.CurrencyCode.UAH,
                rateBuy = 25f,
                rateSell = 25f
            }
        };

        public MonoBankManager.UpdateMonobankTime updateInfo;
        public string token;
    }
}