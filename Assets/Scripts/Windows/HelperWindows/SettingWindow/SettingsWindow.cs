using System;
using Data;
using DefaultNamespace;
using HelperWindows;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Windows.HelperWindows.SettingWindow
{
    [Serializable]
    internal class DollarRate
    {
        public TMP_InputField count;
    }

    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI exchangeRates;
        [SerializeField] private TMP_InputField monobankTokenInputField;
        [SerializeField] private Button monobankTokenApproveBttn;

        private void Awake()
        {
            monobankTokenApproveBttn.onClick.AddListener(() =>
            {
                UserDataManager.Instance.UserData.monobankData.token = monobankTokenInputField.text;
                monobankTokenApproveBttn.interactable = false;
                TabManager.UpdateOpenedTab();
                
            });
        }

        private void OnDestroy()
        {
            monobankTokenApproveBttn.onClick.RemoveAllListeners();
        }

        public void Init()
        {
            if (gameObject.activeInHierarchy)
            {
                OnClose();
                return;
            }

            gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(UserDataManager.Instance.UserData.monobankData.token))
                monobankTokenInputField.text = UserDataManager.Instance.UserData.monobankData.token;
            monobankTokenApproveBttn.interactable = true;
            TransactionUtils.UpdateCurrencyRates(() =>
            {
                var currencyInfos = UserDataManager.Instance.UserData.monobankData.currenciesRate;
                Array.Sort(currencyInfos, delegate(CurrencyInfo user1, CurrencyInfo user2)
                {
                    if (user1.currencyCodeA == (int) MonoBankManager.CurrencyCode.USD)
                        return -1;
                    return 1;
                }); // доллар выставляем на пе
                var result = string.Empty;
                foreach (var item in currencyInfos)
                    result += MonoBankManager.GetNameByCurrencyCode(item.currencyCodeA) + " " +
                              Math.Round(item.rateBuy, 2, MidpointRounding.AwayFromZero).ToString("0.00") + " / " +
                              Math.Round(item.rateSell, 2, MidpointRounding.AwayFromZero).ToString("0.00") + "\n";
                exchangeRates.text = result;
            });
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}