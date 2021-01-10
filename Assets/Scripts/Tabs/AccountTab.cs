using System;
using Data;
using HelperWindows;
using Items;
using Managers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649
namespace Tabs
{
    public class AccountTab : Tab
    {
        [SerializeField] private Transform savingContainer;
        [SerializeField] private GameObject savingPrefab;
        [SerializeField] private TextMeshProUGUI wholeAmount;

        public override void Init()
        {
            base.Init();
            foreach (Transform wallet in savingContainer) Destroy(wallet.gameObject);

            TransactionUtils.UpdateCurrencyRates(() =>
            {
                var sumSavingUsd = 0f;
                foreach (var saving in UserDataManager.Savings)
                {
                    SetSaving(saving);
                    if (saving.currency == Currency.UAH)
                        sumSavingUsd += saving.count * UserDataManager.DollarRate;
                    else
                        sumSavingUsd += saving.count;
                }

                wholeAmount.text = ((int) Math.Round(sumSavingUsd, MidpointRounding.AwayFromZero)).ToString();
            });
            Inited = true;
        }

        private void SetSaving(Saving saving)
        {
            var parent = savingContainer;
            var walletItem = Instantiate(savingPrefab, parent).GetComponent<SavingItem>();
            walletItem.Init(saving);
        }
    }
}