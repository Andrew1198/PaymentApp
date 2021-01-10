using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using DefaultNamespace;
using HelperScripts;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Networking;
using Utility = HelperScripts.Utility;

namespace Managers
{
    public class MonoBankManager : Singleton<MonoBankManager>
    {
        public enum CurrencyCode
        {
            USD = 840,
            UAH = 980,
            EUR = 978
        }

        private const string ApiEndPoint = "https://api.monobank.ua/";
        public MCC_DataBase mccDataBase;
        private RequestStatus currencyRequsetStatus = RequestStatus.Updated;

        private RequestStatus traansactionRequsetStatus = RequestStatus.Updated;

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

        public static void GetTransactions(Action<BankTransaction[]> onFinish)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() -
                UserDataManager.Instance.UserData.monobankData.updateInfo.LastUpdateBankTransactions <
                60)
            {
                onFinish(null);
                return;
            }

            if (Instance.traansactionRequsetStatus == RequestStatus.Updating)
            {
                Utility.Invoke(() => { onFinish(null); },
                    () => Instance.traansactionRequsetStatus == RequestStatus.Updated);
                return;
            }

            Instance.traansactionRequsetStatus = RequestStatus.Updating;
            var to = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var from = to - 60 * 60 * 24 * 31;
            var url = ApiEndPoint + $"/personal/statement/0/{from}/{to}";
            Instance.SendRequest(url, result =>
                {
                    UserDataManager.Instance.UserData.monobankData.updateInfo.LastUpdateBankTransactions =
                        DateTimeOffset.Now.ToUnixTimeSeconds();
                    onFinish(JsonHelper.GetJsonArray<BankTransaction>(result));
                    Instance.traansactionRequsetStatus = RequestStatus.Updated;
                },
                header: new KeyValuePair<string, string>("X-Token",
                    UserDataManager.Instance.UserData.monobankData.token),
                onError: () =>
                {
                    onFinish(null);
                    Instance.traansactionRequsetStatus = RequestStatus.Updated;
                });
        }

        /// <summary>
        ///     Возращает курсы валют доллар, евро
        /// </summary>
        public static void GetExchangeRates(Action<CurrencyInfo[]> onSuccessful, Action onError = null)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() -
                UserDataManager.Instance.UserData.monobankData.updateInfo.LastUpdateCurrencyInfoTime <
                60 * 5)
            {
                onError();
                return;
            }

            if (Instance.currencyRequsetStatus == RequestStatus.Updating)
            {
                Utility.Invoke(onError,
                    () => Instance.currencyRequsetStatus == RequestStatus.Updated);
                return;
            }

            Instance.currencyRequsetStatus = RequestStatus.Updating;
            var url = ApiEndPoint + "bank/currency";
            Instance.SendRequest(url, result =>
            {
                var data = JsonHelper.GetJsonArray<CurrencyInfo>(result).ToList();
                data.RemoveAll(item =>
                    !((item.currencyCodeA == (int) CurrencyCode.EUR ||
                       item.currencyCodeA == (int) CurrencyCode.USD
                      ) && // удаляем все ненужные валюта кроме USD/UAH EUR/UAH
                      item.currencyCodeB == (int) CurrencyCode.UAH)
                );
                UserDataManager.Instance.UserData.monobankData.updateInfo.LastUpdateCurrencyInfoTime =
                    DateTimeOffset.Now.ToUnixTimeSeconds();
                onSuccessful(data.ToArray());
                Instance.currencyRequsetStatus = RequestStatus.Updated;
            }, () =>
            {
                Instance.currencyRequsetStatus = RequestStatus.Updated;
                onError();
            });
        }

        [Button]
        private void GetPersonalInfo()
        {
            var url = ApiEndPoint + "personal/client-info";
            SendRequest(url, result =>
                {
                    using (var sw = new StreamWriter(Application.streamingAssetsPath + "/personalInfo.txt"))
                    {
                        sw.Write(result);
                    }
                },
                header: new KeyValuePair<string, string>("X-Token",
                    UserDataManager.Instance.UserData.monobankData.token));
        }

        private void SendRequest(string uri, Action<string> onFinish, Action onError = null,
            KeyValuePair<string, string> header = default)
        {
            StartCoroutine(SendRequestCor(uri, onFinish, onError, header));
        }

        private IEnumerator SendRequestCor(string uri, Action<string> onFinish, Action onError = null,
            KeyValuePair<string, string> header = default)
        {
            using (var webRequest = UnityWebRequest.Get(uri))
            {
                if (header.Key != null)
                {
                    if (string.IsNullOrEmpty(header.Value))
                    {
                        onError();
                        yield break;
                    }

                    webRequest.SetRequestHeader(header.Key, header.Value);
                }

                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.LogError("Error: " + webRequest.error + $"\n {uri}");
                    onError?.Invoke();
                }
                else
                {
                    onFinish(webRequest.downloadHandler.text);
                }
            }
        }

        public static string GetNameByCurrencyCode(int code)
        {
            return Enum.GetName(typeof(CurrencyCode), code) ?? "";
        }


        [Serializable]
        public struct UpdateMonobankTime
        {
            public long LastUpdateCurrencyInfoTime;
            public long LastUpdateBankTransactions;
        }

        private enum RequestStatus

        {
            Updated,
            Updating
        }
    }
}