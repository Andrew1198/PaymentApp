using Data;
using Managers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649
namespace HelperWindows
{
    public class AddSavingWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameOfWallet;
        [SerializeField] private TMP_Dropdown typeOfCurrency;

        private void OnDisable()
        {
            Undo();
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }

        public void OnOk()
        {
            var walletName = nameOfWallet.text;
            var currency = (Currency) typeOfCurrency.value;
            var wallet = new Saving
            {
                name = walletName,
                currency = currency
            };

            UserDataManager.AddSaving(wallet);
            gameObject.SetActive(false);
        }

        private void Undo()
        {
            nameOfWallet.text = null;
            typeOfCurrency.value = 0;
        }
    }
}