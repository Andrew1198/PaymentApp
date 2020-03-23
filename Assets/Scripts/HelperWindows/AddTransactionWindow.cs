using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HelperWindows
{
    public class AddTransactionWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private TMP_InputField countField;
        [SerializeField] private TMP_InputField commentField;

        public string fromCategory;

        public void Open()
        {
            gameObject.SetActive(true);
            dropdown.options = PlayerData.Wallets
                .Select(wallet => new TMP_Dropdown.OptionData
                {
                   text = wallet.name
                }).ToList();
            countField.text = null;
        }


        public void OnOk()
        {
            var walletName = dropdown.options[dropdown.value].text;
            
            
            foreach (var wallet in PlayerData.Wallets)
            {
                if (walletName == wallet.name)
                {
                    wallet.Subtract( int.Parse(countField.text),wallet._currency);
                    var count = int.Parse(countField.text);
                    if (wallet._currency == Currency.USD)
                        count = (int)Math.Round(count * PlayerData.DollarRate, MidpointRounding.AwayFromZero);
                    
                    var transaction = new Transaction
                    {
                        _category = fromCategory,
                        _count = count,
                        _comment = commentField.text,
                        wallet =  walletName,
                        Time = DateTime.Now
                    };
                   PlayerData.CurrentDayilyTrasactions.Add(transaction);
                    TabManager.UpdateTab();
                    break;
                }
            }
            gameObject.SetActive(false);
        }
    }
}