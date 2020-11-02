using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DefaultNamespace;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
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
            dropdown.options = UserDataManager.Wallets
                .Select(wallet => new TMP_Dropdown.OptionData
                {
                   text = wallet.name
                }).ToList();
            countField.text = null;
            commentField.text = null;
        }


        public void OnOk()
        {
            UserDataManager.SelectedDate = DateTime.Now;
            var walletName = dropdown.options[dropdown.value].text;
            UserDataManager.GetDollarRate(dollarRate =>
            {
                foreach (var wallet in UserDataManager.Wallets)
                {
                    if (walletName == wallet.name)
                    {
                        wallet.Subtract( int.Parse(countField.text),wallet._currency);
                        var count = int.Parse(countField.text);
                        if (wallet._currency == Currency.USD)
                            count = (int)Math.Round(count * dollarRate, MidpointRounding.AwayFromZero);
                    
                        var transaction = new Transaction
                        {
                            _category = fromCategory,
                            _count = count,
                            _comment = commentField.text,
                            wallet =  walletName,
                            Time = DateTime.Now
                        };
                        UserDataManager.CurrentDayilyTrasactions.Add(transaction);
                        Events.OnUpdateTab?.Invoke();
                        break;
                    }
                }
                gameObject.SetActive(false);
            });
            
            
            
        }
    }
}