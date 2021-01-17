using System.Collections.Generic;
using System.Linq;
using Data;
using HelperWindows;
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


        private void Awake()
        {
            data.needToUpdateCurrencyRate = false;
        }
        
        public override void Init()
        {
            base.Init();
            InitTabByTabData(() =>
            {
                Date.text = UserDataManager.SelectedDate.ToString("MMMM yyyy");
                Draw();
                Inited = true;
            });
        }

        private void Draw()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);
          
                var monthlyTransaction = GetTransactions();
                foreach (var dailyTransaction in monthlyTransaction._transactions)
                {
                    if (dailyTransaction._transactions.Count == 0)
                        continue;
                    var daySeparator = Instantiate(daySeparatorPref, content);
                    var data = daySeparator.transform.Find("Data");
                    data.Find("Number").GetComponent<TextMeshProUGUI>().text = dailyTransaction.day.ToString();
                    var moneySpended = dailyTransaction._transactions.Sum(payment => payment.amount);
                    data.Find("MoneySpent").GetComponent<TextMeshProUGUI>().text = "<color=yellow>" + moneySpended;

                    var allTypeTransactions = new List<TransactionItem.TransactionItemData>();
                 foreach (var transaction in TransactionUtils.GetTransactionsByType<CashTransaction>(dailyTransaction._transactions))
                     allTypeTransactions.Add(new TransactionItem.TransactionItemData
                     {
                         Count   =  transaction.amount,
                         Category = transaction.category,
                         Comment = transaction.comment,
                         Time = transaction.Time,
                         IsBankTransaction = false
                     });
                 foreach (var transaction in TransactionUtils.GetTransactionsByType<BankTransaction>(dailyTransaction._transactions))
                 {
                     allTypeTransactions.Add(new TransactionItem.TransactionItemData
                     {
                         Count   =  transaction.amount,
                         Category = MonoBankManager.Instance.mccDataBase.GetDescriptionByMccCode(transaction.mcc),
                         Comment = transaction.comment,
                         Time = transaction.Time,
                         IsBankTransaction = true
                     });
                 }
                 allTypeTransactions = allTypeTransactions.OrderByDescending(u => u.Time).ToList();
                    foreach (var transaction in allTypeTransactions)
                        Instantiate(transactionItemPref, content).GetComponent<TransactionItem>().Init(transaction);
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(content as RectTransform);
            
        }

        private MonthlyTransaction GetTransactions()
        {
            var currentMonthlyTransactions = UserDataManager.CurrentMonthlyTransaction;
            currentMonthlyTransactions._transactions = currentMonthlyTransactions._transactions
                .OrderByDescending(transaction => transaction.day).ToList();

            return currentMonthlyTransactions;
        }
    }
}