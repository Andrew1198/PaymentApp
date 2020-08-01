using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
namespace HelperWindows.ManageWalletWindow
{
    public abstract class ManageWalletBase : MonoBehaviour
    {
        [SerializeField] private ManageWalletWindow manageWalletWindow;
        [SerializeField] protected TextMeshProUGUI selectedWalletText;
        [SerializeField] protected TMP_InputField countField;

        protected Wallet SelectedWallet => manageWalletWindow.wallet;
        


        public virtual void Init()
        {
            selectedWalletText.text = SelectedWallet.name;
            gameObject.SetActive(true);
        }
        
        public virtual void OnOk()
        {
            
        }
        
        public virtual void OnClose()
        {
            gameObject.SetActive(false);
            manageWalletWindow.gameObject.SetActive(false);
            Reset();
        }
        
        protected virtual void Reset()
        {
            selectedWalletText.text = null;
            countField.text = null;
        }
    }
}