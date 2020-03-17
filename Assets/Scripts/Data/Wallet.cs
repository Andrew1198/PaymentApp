using System;
using System.Security.Cryptography;

namespace Data
{
    public enum WalletType
    {
        Used,
        Saving
    }

    public enum Currency
    {
        USD,
        UAH
    }
    [Serializable]
    public class Wallet
    {
        public string name;
        public int _count;
        public WalletType _type;
        public Currency _currency;
    }
    
}