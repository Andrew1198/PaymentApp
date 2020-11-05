using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using Firebase.Firestore;
using HelperScripts;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Networking;
using Utility = HelperScripts.Utility;

namespace Managers
{
    public class MonoBankManager : Singleton<MonoBankManager>
    {
        private const string token = "uOpR4ZpvpBxHnnCcI0OXXjWD2-qwK_owBS6pC1UCdh7Q";
        private const string ApiEndPoint = "https://api.monobank.ua/";

        public UpdateMonobankTime updateInfo = new UpdateMonobankTime();
        
        public static void GetTransactions(Action<BankTransaction[]>onFinish)
        {
            var to = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var from = to - 60 * 60 * 24 * 31;
            var url = ApiEndPoint + $"/personal/statement/0/{from}/{to}";
            Instance.SendRequest(url, result =>
            {
                onFinish(JsonHelper.GetJsonArray<BankTransaction>(result));
            },header:new KeyValuePair<string, string>("X-Token", token),onError: () => { onFinish(null);
            });
        }
        
        /// <summary>
        /// Возращает курсы валют доллар, евро
        /// </summary>
        public static void GetExchangeRates(Action<CurrencyInfo[]> onSuccessful,Action onError = null)
        {
            var url = ApiEndPoint + "bank/currency";
            Instance.SendRequest(url, result=>
            {
                var data = JsonHelper.GetJsonArray<CurrencyInfo>(result).ToList();
                data.RemoveAll(item =>
                    !((item.currencyCodeA == (int) CurrencyCode.EUR || item.currencyCodeA == (int) CurrencyCode.USD) && // удаляем все ненужные валюта кроме USD/UAH EUR/UAH
                      item.currencyCodeB == (int) CurrencyCode.UAH)
                );
                onSuccessful(data.ToArray());
            });
        }

        [Button()]
        private void GetPersonalInfo()
        {
            var url = ApiEndPoint + "personal/client-info";
            SendRequest(url, result =>
            {
                using (StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/personalInfo.txt"))
                {
                    sw.Write(result);
                }
            }, header: new KeyValuePair<string, string>("X-Token", token));
        }

        void SendRequest(string uri, Action<string> onFinish,Action onError = null,KeyValuePair<string,string> header = default) => StartCoroutine(SendRequestCor(uri, onFinish,onError,header));
        IEnumerator SendRequestCor(string uri,Action<string> onFinish,Action onError = null,KeyValuePair<string,string> header = default)
        {
            using (var webRequest = UnityWebRequest.Get(uri))
            {
                if(header.Key != null)
                    webRequest.SetRequestHeader(header.Key,header.Value);
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                
                if (webRequest.isNetworkError)
                {
                    Debug.Log( "Error: " + webRequest.error);
                    onError();
                }
                else
                {
                    if (webRequest.downloadHandler.text.Contains("Too many requests"))
                    {
                        Utility.Invoke(this,()=>SendRequest(uri,onFinish),.5f);
                        Debug.Log("reapet");
                        yield break;
                    }
                    onFinish(webRequest.downloadHandler.text);
                }
            }
        }

        public static string GetNameByCurrencyCode(int code)
        {
            return Enum.GetName(typeof(CurrencyCode), code)??"";
        }
        
       

        public class UpdateMonobankTime
        {
            public long LastUpdateCurrencyInfoTime;
            public long LastUpdateBankTransactions;
        }

        public enum CurrencyCode
        {
            USD = 840,
            UAH = 980,
            EUR = 978
        }


       [Button()]
        private void TestTransactions()
        {
            UserDataManager.SetNewBankTransactionsInData(() =>
            {
                using (var sr = new StreamWriter(Application.persistentDataPath + "/transactions.txt"))
                {
                    var json = JsonUtility.ToJson(UserDataManager.CurrentYearlyTransactions,true);
                    sr.Write(json);
                    Debug.LogError("Success");
                }
            });
        }
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                UserDataManager.Save();
                Debug.LogError("Save");
            }
#endif
        }
        
    }
   
}