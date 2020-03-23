using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Managers;
using TMPro;
using UnityEngine;

namespace HelperWindows.ManageWalletWindow
{
    public class TransferWindow : MonoBehaviour
    {
        [SerializeField] private ManageWalletWindow manageWalletWindow;
        [SerializeField] private TMP_Dropdown walletsDropdown;
        [SerializeField] private TMP_InputField countField;
        
        public void Init()
        {
            gameObject.SetActive(true);
            manageWalletWindow.wallet = manageWalletWindow.wallet;
            var walletsName = PlayerData.Wallets.Where(wallet => wallet.name != manageWalletWindow.wallet.name)
                .Select(wallet => new TMP_Dropdown.OptionData {text = wallet.name}).ToList();
            walletsDropdown.options = walletsName;
        }

        public void OnOk()
        {
            var walletName = walletsDropdown.options[walletsDropdown.value].text;
            var toWallet = PlayerData.Wallets.First(x => x.name == walletName);
            
            toWallet.AddCount(int.Parse(countField.text),manageWalletWindow.wallet._currency);
            manageWalletWindow.wallet.Subtract( int.Parse(countField.text),manageWalletWindow.wallet._currency);
            TabManager.UpdateTab();
            OnClose();
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
            manageWalletWindow.gameObject.SetActive(false);
            Reset();
        }

        private void Reset()
        {
            countField.text = null;
        }
    }
}