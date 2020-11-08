using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DefaultNamespace;
using Items;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
            foreach (Transform wallet in savingContainer)
            {
                Destroy(wallet.gameObject);
            }
            
            UserDataManager.GetDollarRate(dollarRate =>
            {
                var sumSavingUsd = 0f;
                foreach (var saving in UserDataManager.Savings)
                {
                    SetSaving(saving);
                    if (saving.currency == Currency.UAH)
                        sumSavingUsd += saving.count * dollarRate;
                    else
                        sumSavingUsd += saving.count;
                }

                wholeAmount.text = ((int) Math.Round(sumSavingUsd, MidpointRounding.AwayFromZero)).ToString();
            });
            
        }

        private void SetSaving(Saving saving)
        {
            var parent = savingContainer;
            var walletItem = Instantiate(savingPrefab, parent).GetComponent<SavingItem>();
            walletItem.Init(saving);
        }
    }
}