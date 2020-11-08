using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Managers;

namespace HelperWindows
{
    public class TransactionUtils
    {

        public static List<Transaction>CashTransactionsPerMonth
        {
            get
            {
                var result = new List<Transaction>();
                foreach (var dailyTransaction in UserDataManager.CurrentMonthlyTransaction._transactions)
                    result.AddRange(dailyTransaction._transactions);

                return result;
            }
        }

        public static List<BankTransaction> BankTransactionsPerMonth
        {
            get
            {
                var result = new List<BankTransaction>();
                foreach (var dailyTransaction in UserDataManager.CurrentMonthlyTransaction._transactions)
                    result.AddRange(dailyTransaction.bankTransactions);

                return result;
            }
        }
        
        


        public static long AmountPerMonth(bool includeCashTransactions, bool includeBankTransactions)
        {
            long result = 0;
            if (includeCashTransactions)
                result += UserDataManager.CurrentMonthlyTransaction._transactions
                    .SelectMany(dailyTransaction => dailyTransaction._transactions)
                    .Sum(transaction => transaction._count);
            if(includeBankTransactions)
                result += UserDataManager.CurrentMonthlyTransaction._transactions
                    .SelectMany(dailyTransaction => dailyTransaction.bankTransactions)
                    .Sum(transaction => transaction.amount);

            return result;
        }
        
        public static int GetPercentageOfAmount(long sum, bool includeCashTransactions, bool includeBankTransactions)
        {
            var amountPerMonth = AmountPerMonth(includeCashTransactions,includeBankTransactions);
            
            var wholePart = Convert.ToInt32((float)sum / amountPerMonth*100);
            var fraction = (float)sum / amountPerMonth*100 - wholePart;

            if (fraction >= .5f)
                wholePart++;
            return wholePart;
        }
    }
}