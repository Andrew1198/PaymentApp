using System;
using Managers;

namespace Data
{
    public enum Currency
    {
        USD,
        UAH
    }

    [Serializable]
    public class Saving
    {
        public string name;
        public int count;
        public Currency currency;

        public void AddCount(int count, Currency currency)
        {
            var dollarRate = UserDataManager.DollarRate;
            if (currency == this.currency)
                this.count += count;
            else if (this.currency == Currency.UAH)
                this.count += (int) Math.Round(count * dollarRate, MidpointRounding.AwayFromZero);
            else
                this.count += (int) Math.Round(count / dollarRate, MidpointRounding.AwayFromZero);
        }

        public void SubtractCount(int count, Currency currency)
        {
            AddCount(count * -1, currency);
        }
    }
}