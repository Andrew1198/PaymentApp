using System;
using Data;
using Managers;
using TMPro;
using UnityEngine;

namespace HelperWindows.ManageWalletWindow
{
    public class RechargeWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI walletName;
        [SerializeField] private TMP_InputField countField;
        [SerializeField] private ManageWalletWindow manageWalletWindow;
        
        public void Init()
        {
            gameObject.SetActive(true);
            manageWalletWindow.wallet = manageWalletWindow.wallet;
            walletName.text = manageWalletWindow.wallet.name;
        }

        public void OnOk()
        {
            manageWalletWindow.wallet._count = int.Parse(countField.text);
            TabManager.UpdateTab();
            OnClose();
        }

        public void OnClose()
        {
            Reset();
            gameObject.SetActive(false);
            manageWalletWindow.gameObject.SetActive(false);
        }

        private void Reset()
        {
            countField.text = null;
        }
    }
}