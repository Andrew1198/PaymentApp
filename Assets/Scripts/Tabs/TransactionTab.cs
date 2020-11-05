using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DefaultNamespace;
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
            Date.text = UserDataManager.SelectedDate.ToString("MMMM yyyy");
           Draw();
        }

        private void Draw()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);
            Events.EnableLoadingScreen.Invoke();
            UserDataManager.SetNewBankTransactionsInData(()=>
            {
                var transactions = GetTransactions();
                foreach (var dailyTransaction in transactions)
                {
                    if (dailyTransaction._transactions.Count == 0 && dailyTransaction.bankTransactions.Count == 0)
                        continue;
                    var daySeparator = Instantiate(daySeparatorPref, content);
                    var data = daySeparator.transform.Find("Data");
                    data.Find("Number").GetComponent<TextMeshProUGUI>().text = dailyTransaction.day.ToString();
                    data.Find("Month").GetComponent<TextMeshProUGUI>().text = dailyTransaction._transactions.First().Time.ToString("dddd");
                    data.Find("MoneySpent").GetComponent<TextMeshProUGUI>().text = dailyTransaction._transactions.Sum(payment=>payment._count).ToString();
                    
                    
                 var allTypeTransactions = new List<TransactionItem.TransactionItemData>();
                 foreach (var transaction in dailyTransaction._transactions)
                     allTypeTransactions.Add(new TransactionItem.TransactionItemData
                     {
                         Count   =  transaction._count,
                         Category = transaction._category,
                         Comment = transaction._comment,
                         Time = transaction.Time
                     });
                 /*foreach (var transaction in dailyTransaction.bankTransactions)
                 {
                     allTypeTransactions.Add(new TransactionItem.TransactionItemData
                     {
                         Count   =  transaction.amount,
                         Category = transaction._category,
                         Comment = transaction._comment,
                         Time = DateTimeOffset.FromUnixTimeSeconds(transaction.time).LocalDateTime
                     });
                 }*/
                 
                 
                        /*Instantiate(transactionItemPref, content).GetComponent<TransactionItem>().Init(new TransactionItem.TransactionItemData
                            {
                                Count   =  transaction._count,
                                Category = transaction._category,
                                Comment = transaction._comment,
                                Time = transaction.Time
                            }
                        );*/
                
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(content as RectTransform);
            });
            
        }
        
        private List<DailyTransaction> GetTransactions()
        {
            var currentMonthlyTransactions = UserDataManager.CurrentMonthlyTransaction;
            currentMonthlyTransactions = currentMonthlyTransactions.OrderByDescending(transaction => transaction.day).ToList();
            
            
            return currentMonthlyTransactions;
        }
    }
}