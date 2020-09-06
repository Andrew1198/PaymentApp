using Data;
using DefaultNamespace;
using Managers;
using UnityEngine;
#pragma warning disable 0649
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
                var deleted = UserDataManager.Wallets.Remove(wallet);
                if (!deleted)
                    Debug.LogError("Can't delete wallet");
                
                Events.OnUpdateTab?.Invoke();
                gameObject.SetActive(false);
            });
        }
    }
}