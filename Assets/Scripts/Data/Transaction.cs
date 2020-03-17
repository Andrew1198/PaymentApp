using System;

namespace Data
{
    public enum Category
    {
        Restaurant,
        Groceries,
        Leisure,
        Transport,
        Lapka,
        Health,
        Shopping,
        Family
    }
    [Serializable]
    public class Transaction
    {
        public int _count;
        private string _comment;
        public DateTime _time;
        private WalletType _fromWallet;
        private Category _category;
    }
}