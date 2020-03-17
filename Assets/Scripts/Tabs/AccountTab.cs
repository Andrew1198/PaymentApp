using System.Collections.Generic;
using Data;
using Items;
using Managers;
using UnityEngine;

namespace Tabs
{
    public class AccountTab : Tab
    {
        [SerializeField] private Transform walletsContainer;
        [SerializeField] private Transform savingContainer;
        [SerializeField] private GameObject walletPrefab;
        
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
                
            
        }

        private void SetWallet(Wallet wallet)
        {
            var parent = wallet._type == WalletType.Used ? walletsContainer : savingContainer;
            var walletItem = Instantiate(walletPrefab, parent).GetComponent<WalletItem>();
            walletItem.Init(wallet);
        }
    }
}