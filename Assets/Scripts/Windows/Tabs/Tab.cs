using System;
using System.Collections;
using System.Collections.Generic;
using Windows.HelperWindows;
using Windows.WindowsData;
using HelperScripts;
using HelperWindows;
using Managers;
using UnityEngine;

#pragma warning disable 0649
namespace Windows.Tabs
{
    public abstract class Tab : WindowBase
    {
        public override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            base.Open(DynamicWindowData);
            TransactionUtils.UpdateCurrencyRates();
            TransactionUtils.UpdateAllBankTransactions();
        }
        
    }
}