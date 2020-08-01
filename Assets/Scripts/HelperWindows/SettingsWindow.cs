using System;
using System.Globalization;
using System.Xml;
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
            dollarRate.count.text = PlayerData.DollarRate.ToString(CultureInfo.InvariantCulture);
        }

        public void OnClose()
        {
            PlayerData.DollarRate = float.Parse(dollarRate.count.text);
            gameObject.SetActive(false);
        }
    }
}