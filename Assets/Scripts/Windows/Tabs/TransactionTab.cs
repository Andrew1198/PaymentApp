using System.Collections.Generic;
using System.Linq;
using Data;
using HelperScripts;
using Items;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Windows.Tabs
{
    public class TransactionTab : Tab
    {
        [SerializeField] private TextMeshProUGUI Date;
        [SerializeField] private GameObject transactionItemPref;
        [SerializeField] private GameObject daySeparatorPref;
        [SerializeField] private Transform content;

        public override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            base.Open(DynamicWindowData);
            Date.text = UserDataManager.SelectedDate.ToString("MMMM yyyy");
            Draw();
            Inited = true;
        }
        
        private void Draw()
        {
            content.DeleteChildren();
            
            var monthlyTransaction = GetTransactions();
                foreach (var dailyTransaction in monthlyTransaction.transactions)
                {
                    if (dailyTransaction.GetALlTypeTransactions().Count == 0)
                        continue;
                    var daySeparator = Instantiate(daySeparatorPref, content);
                    var data = daySeparator.transform.Find("Data");
                    data.Find("Number").GetComponent<TextMeshProUGUI>().text = dailyTransaction.day.ToString();
                    var moneySpended = dailyTransaction.GetALlTypeTransactions().Sum(payment => payment.amount);
                    data.Find("MoneySpent").GetComponent<TextMeshProUGUI>().text = "<color=yellow>" + moneySpended;

                    var transactionsItemDataList = new List<TransactionItem.TransactionItemData>();
                 foreach (var transaction in dailyTransaction.GetALlTypeTransactions())
                     transactionsItemDataList.Add(new TransactionItem.TransactionItemData
                     {
                         Count   =  transaction.amount,
                         Category = transaction.category,
                         Comment = transaction.description,
                         Time = transaction.time,
                         IsBankTransaction = transaction.type == TransactionType.Bank
                     });
                
                 transactionsItemDataList = transactionsItemDataList.OrderByDescending(u => u.Time).ToList();
                    foreach (var transaction in transactionsItemDataList)
                        Instantiate(transactionItemPref, content).GetComponent<TransactionItem>().Init(transaction);
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(content as RectTransform);
            
        }

        private MonthlyTransaction GetTransactions()
        {
            var currentMonthlyTransactions = UserDataManager.CurrentMonthlyTransaction;
            currentMonthlyTransactions.transactions = currentMonthlyTransactions.transactions
                .OrderByDescending(transaction => transaction.day).ToList();

            return currentMonthlyTransactions;
        }
    }
}