using Data;
using Managers;
using UnityEngine;

namespace HelperWindows.ManageWalletWindow
{
    public class ManageWalletWindow : MonoBehaviour
    {
        [HideInInspector]public Wallet wallet;
        [SerializeField] private ConfirmWindow confirmWindow;

        public void Init(Wallet wallet)
        {
            gameObject.SetActive(true);
            this.wallet = wallet;
        }


        public void OnDeleteWalletWindow()
        {
            confirmWindow.Open(() =>
            {
                var deleted = PlayerData.Wallets.Remove(wallet);
                if (!deleted)
                    Debug.LogError("Can't delete wallet");
                
                TabManager.UpdateTab();
                gameObject.SetActive(false);
            });
        }
    }
}