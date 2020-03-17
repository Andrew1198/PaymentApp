using System.Collections.Generic;
using System.Linq;
using Data;
using Managers;
using UnityEngine;

namespace Tabs
{
    public class TransactionTab : Tab
    {




        private List<DailyTransaction> GetTransactions()
        {
            var currentMonthlyTransactions = PlayerData.CurrentMonthlyTransaction;
            currentMonthlyTransactions = currentMonthlyTransactions.OrderBy(transaction => transaction.day).ToList();
            
            foreach (var dayTransaction in currentMonthlyTransactions)
            {
                dayTransaction._transactions =
                    dayTransaction._transactions.OrderBy(transaction => transaction._time).ToList();
            }

            return currentMonthlyTransactions;
        }
    }
}