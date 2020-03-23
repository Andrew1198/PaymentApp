using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace HelperWindows.ManageWalletWindow
{
    public class TopUpWindow : MonoBehaviour
    {
        [SerializeField] private ManageWalletWindow manageWalletWindow;
        [SerializeField] private TextMeshProUGUI walletName;
        [SerializeField] private TMP_InputField countField;
        
        public void Init()
        {
            gameObject.SetActive(true);
            manageWalletWindow.wallet = manageWalletWindow.wallet;
            walletName.text = manageWalletWindow.wallet.name;
        }
        
        public void OnOk()
        {
            manageWalletWindow.wallet._count += int.Parse(countField.text);
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