using System.Collections.Generic;
using System.Linq;
using Data;
using Items;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
namespace Tabs
{
    public class TransactionTab : Tab
    {
        [SerializeField] private TextMeshProUGUI Date;
        [SerializeField] private GameObject transactionItemPref;
        [SerializeField] private GameObject daySeparatorPref;
        [SerializeField] private Transform content;
        public override void Init()
        {
            base.Init();
            Date.text = PlayerData.SelectedDate.ToString("MMMM yyyy");
           Draw();
        }

        private void Draw()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);
            
            var transactions = GetTransactions();
            foreach (var dailyTransaction in transactions)
            {
                if (dailyTransaction._transactions.Count == 0)
                    continue;
                var daySeparator = Instantiate(daySeparatorPref, content);
                var data = daySeparator.transform.Find("Data");
                data.Find("Number").GetComponent<TextMeshProUGUI>().text = dailyTransaction.day.ToString();
                data.Find("Month").GetComponent<TextMeshProUGUI>().text = dailyTransaction._transactions.First().Time.ToString("dddd");
                data.Find("MoneySpent").GetComponent<TextMeshProUGUI>().text = dailyTransaction._transactions.Sum(payment=>payment._count).ToString();
                
                foreach (var transaction in dailyTransaction._transactions)
                    Instantiate(transactionItemPref, content).GetComponent<TransactionItem>().Init(transaction);
                
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(content as RectTransform);
        }
        
        private List<DailyTransaction> GetTransactions()
        {
            var currentMonthlyTransactions = PlayerData.CurrentMonthlyTransaction;
            currentMonthlyTransactions = currentMonthlyTransactions.OrderByDescending(transaction => transaction.day).ToList();
            
            foreach (var dayTransaction in currentMonthlyTransactions)
            {
                dayTransaction._transactions =
                    dayTransaction._transactions.OrderByDescending(transaction => transaction.Time).ToList();
            }

            return currentMonthlyTransactions;
        }
    }
}