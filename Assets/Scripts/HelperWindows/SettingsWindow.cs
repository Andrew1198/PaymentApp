using System;
using System.Globalization;
using System.Xml;
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
        [SerializeField] private DollarRate dollarRate;
        
        public void Init()
        {
            if (gameObject.activeInHierarchy)
            {
                OnClose();
                return;
            }
            gameObject.SetActive(true);
            dollarRate.count.text = UserDataManager.DollarRate.ToString(CultureInfo.InvariantCulture);
        }

        public void OnClose()
        {
            UserDataManager.DollarRate = float.Parse(dollarRate.count.text);
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