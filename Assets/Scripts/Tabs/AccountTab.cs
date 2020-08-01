using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Items;
using Managers;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
namespace Tabs
{
    public class AccountTab : Tab
    {
        [SerializeField] private Transform walletsContainer;
        [SerializeField] private Transform savingContainer;
        [SerializeField] private GameObject walletPrefab;

        [SerializeField] private RectTransform[] needToRebuild;

        public override void Init()
        {
            base.Init();
            foreach (Transform wallet in walletsContainer)
            {
                Destroy(wallet.gameObject);
            }
            foreach (Transform wallet in savingContainer)
            {
                Destroy(wallet.gameObject);
            }
            
            foreach (var wallet in PlayerData.Wallets)
                SetWallet(wallet);
            StartCoroutine(RebuildElements());
        }
        
        private void SetWallet(Wallet wallet)
        {
            var parent = wallet._type == WalletType.Used ? walletsContainer : savingContainer;
            var walletItem = Instantiate(walletPrefab, parent).GetComponent<WalletItem>();
            walletItem.Init(wallet);
        }

        IEnumerator RebuildElements()
        {
            yield return null;
            foreach (var rectTransform in needToRebuild)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            
        }
    }
}