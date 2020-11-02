﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Managers;
using UnityEngine;

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

        public void AddCount(int count, Currency currency)
        {
            UserDataManager.GetDollarRate(dollarRate =>
            {
                if (currency == _currency)
                    _count += count;
                else if (_currency == Currency.UAH)
                    _count += (int)Math.Round(count * dollarRate, MidpointRounding.AwayFromZero);
                else
                    _count += (int)Math.Round(count / dollarRate, MidpointRounding.AwayFromZero);
            });
           
        }

        public void Subtract(int count, Currency currency)=> AddCount(count * -1,currency);
        
        
    }
    
}