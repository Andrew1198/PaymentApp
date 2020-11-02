using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using Data;
using DefaultNamespace;
using GoogleFireBase;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
namespace HelperWindows
{
    [Serializable]
    class DollarRate
    {
        public TMP_InputField count;
    }
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI exchangeRates;

        public void Init()
        {
            if (gameObject.activeInHierarchy)
            {
                OnClose();
                return;
            }

            gameObject.SetActive(true);
            UserDataManager.GetCurrenciesRate(currencyInfos =>
                {
                    Array.Sort(currencyInfos, delegate(CurrencyInfo user1, CurrencyInfo user2)
                    {
                        if (user1.currencyCodeA == (int) MonoBankManager.CurrencyCode.USD)
                            return -1;
                        return 1;
                    }); // доллар выставляем на пе
                    var result = string.Empty;
                    foreach (var item in currencyInfos)
                    {
                        result += MonoBankManager.GetNameByCurrencyCode(item.currencyCodeA) + " " +
                                  Math.Round(item.rateBuy, 2, MidpointRounding.AwayFromZero).ToString("0.00") + " / " +
                                  Math.Round(item.rateSell, 2, MidpointRounding.AwayFromZero).ToString("0.00") + "\n";
                    }
                    exchangeRates.text =result ;
                }
            );
            
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
        }

        public void LoadDataFromFirebase()
        {
            GoogleFireBaseManager.GetData( userData =>
            {
                if (userData == null) return;
                UserDataManager.Init(userData);
                UserDataManager.Save();
                Events.OnUpdateTab?.Invoke();
                Debug.Log("FirebaseData has been loaded");
            });
        }
    }
}