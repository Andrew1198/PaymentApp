using System.Linq;
using DefaultNamespace;
using Managers;
using TMPro;
using UnityEngine;
#pragma warning disable 0649
namespace HelperWindows.ManageWalletWindow
{
    public class TransferWindow : ManageWalletBase
    {
        [SerializeField] private TMP_Dropdown walletsDropdown;

        public override void Init()
        {
            base.Init();
            var walletsName = UserDataManager.Wallets.Where(wallet => wallet.name != SelectedWallet.name)
                .Select(wallet => new TMP_Dropdown.OptionData {text = wallet.name}).ToList();
            walletsDropdown.options = walletsName;
        }

        public override void OnOk()
        {
            var walletName = walletsDropdown.options[walletsDropdown.value].text;
            var toWallet = UserDataManager.Wallets.First(x => x.name == walletName);
            
            toWallet.AddCount(int.Parse(countField.text),SelectedWallet._currency);
            SelectedWallet.Subtract( int.Parse(countField.text),SelectedWallet._currency);
            Events.OnUpdateTab?.Invoke();
            OnClose();
        }
    }
}