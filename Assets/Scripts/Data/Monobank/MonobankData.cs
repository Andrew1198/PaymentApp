using System;
using Managers;

namespace Data
{
    [Serializable]
    public class MonobankData
    {
        public CurrencyInfo[] currenciesRate = {new CurrencyInfo()};
        public MonoBankManager.UpdateMonobankTime updateInfo = new MonoBankManager.UpdateMonobankTime();

    }
}