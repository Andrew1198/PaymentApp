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
        private bool isUpdatedCurrencyRates;
        private bool isUpdatedAllBankTransactions;
        public sealed override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            base.Open(DynamicWindowData);
            TransactionUtils.UpdateCurrencyRates(() =>
            {
                isUpdatedCurrencyRates = true;
            });
            TransactionUtils.UpdateAllBankTransactions(() =>
            {
                isUpdatedAllBankTransactions = true;
            });
            StartCoroutine(WaitForUpdateData());
        }

        IEnumerator WaitForUpdateData()
        {
            yield return new WaitUntil(() => isUpdatedCurrencyRates && isUpdatedAllBankTransactions);
            OpenTab();
        }

        protected virtual void OpenTab()
        {
            
        }
        
    }
}