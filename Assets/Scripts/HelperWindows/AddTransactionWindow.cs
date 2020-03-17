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
        [SerializeField] private TMP_InputField inputField;

        public Category fromCategory;

        public void Init()
        {
            gameObject.SetActive(true);
            dropdown.options = PlayerData.Wallets.Where(wallet => wallet._currency == Currency.UAH)
                .Select(wallet => new TMP_Dropdown.OptionData
                {
                   text = wallet.name
                }).ToList();
            inputField.text = null;
        }


        public void OnOk()
        {
            var walletName = dropdown.options[dropdown.value].text;

            foreach (var wallet in PlayerData.Wallets)
            {
                if (walletName == wallet.name)
                {
                    wallet._count -= int.Parse(inputField.text);
                    if(!PlayerData.CurrentDayilyTrasactions.Any(transaction => transaction.Category == fromCategory))
                        PlayerData.CurrentDayilyTrasactions.Add(new CategoriesTransaction
                        {
                            Category = fromCategory
                        });

                    var categoriesTransaction =
                        PlayerData.CurrentDayilyTrasactions.First(transaction => transaction.Category == fromCategory);
                    categoriesTransaction.Transactions.Add(new Transaction
                    {
                        _count = int.Parse(inputField.text)
                    });
                    TabManager.UpdateTab();
                    break;
                }
            }
            gameObject.SetActive(false);
        }
    }
}