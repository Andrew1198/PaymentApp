using System;
using Data;
using Managers;
using TMPro;
using UnityEngine;
#pragma warning disable 0649
namespace HelperWindows
{
    public class AddWalletWindow : MonoBehaviour
    {
        
        [SerializeField] private TMP_InputField nameOfWallet;
        [SerializeField] private TMP_Dropdown typeOfWallet;
        [SerializeField] private TMP_Dropdown typeOfCurrency;
        
        public void OnClose()
        {
            gameObject.SetActive(false);
        }

        public void OnOk()
        {
            var walletName = nameOfWallet.text;
            var type = (WalletType)typeOfWallet.value;
            var currency = (Currency) typeOfCurrency.value;
            var wallet = new Wallet
            {
               name = walletName,
               _type = type,
               _currency = currency
            };
            
            PlayerData.AddWallet(wallet);
            gameObject.SetActive(false);
        }

        private void Undo()
        {
            nameOfWallet.text = null;
            typeOfWallet.value = 0;
            typeOfCurrency.value = 0;
        }

        private void OnDisable()
        {
            Undo();
        }
    }
}