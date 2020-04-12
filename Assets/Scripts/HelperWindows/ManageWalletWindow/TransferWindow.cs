using System.Linq;
using Managers;
using TMPro;
using UnityEngine;

namespace HelperWindows.ManageWalletWindow
{
    public class TransferWindow : ManageWalletBase
    {
        [SerializeField] private TMP_Dropdown walletsDropdown;

        public override void Init()
        {
            base.Init();
            var walletsName = PlayerData.Wallets.Where(wallet => wallet.name != SelectedWallet.name)
                .Select(wallet => new TMP_Dropdown.OptionData {text = wallet.name}).ToList();
            walletsDropdown.options = walletsName;
        }

        public override void OnOk()
        {
            var walletName = walletsDropdown.options[walletsDropdown.value].text;
            var toWallet = PlayerData.Wallets.First(x => x.name == walletName);
            
            toWallet.AddCount(int.Parse(countField.text),SelectedWallet._currency);
            SelectedWallet.Subtract( int.Parse(countField.text),SelectedWallet._currency);
            TabManager.UpdateTab();
            OnClose();
        }
    }
}