using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DefaultNamespace;
using Managers;

namespace HelperWindows
{
    public class TransactionUtils
    {
        public static List<CashTransaction> CashTransactionsPerMonth
        {
            get
            {
                var result = new List<CashTransaction>();
                foreach (var dailyTransaction in UserDataManager.CurrentMonthlyTransaction.transactions)
                    result.AddRange(dailyTransaction.cashTransactions
                        .Where(transaction => transaction.type == TransactionType.Cash)
                        .Select(transaction => (CashTransaction)transaction).ToList());
                
                return result;
            }
        }

        public static List<BankTransaction> BankTransactionsPerMonth
        {
            get
            {
                var result = new List<BankTransaction>();
                foreach (var dailyTransaction in UserDataManager.CurrentMonthlyTransaction.transactions)
                    result.AddRange(dailyTransaction.bankTransactions
                        .Where(transaction => transaction.type == TransactionType.Bank)
                        .Select(transaction => (BankTransaction)transaction).ToList());
                
                return result;
            }
        }


        public static long AmountPerMonth(bool includeCashTransactions, bool includeBankTransactions)
        {
            long result = 0;
            if (includeCashTransactions)
                result += UserDataManager.CurrentMonthlyTransaction.transactions
                    .SelectMany(dailyTransaction => dailyTransaction.cashTransactions.Where(transaction=>transaction.type == TransactionType.Cash))
                    .Sum(transaction => transaction.amount);
            if (includeBankTransactions)
                result += UserDataManager.CurrentMonthlyTransaction.transactions
                    .SelectMany(dailyTransaction => dailyTransaction.bankTransactions.Where(transaction=>transaction.type == TransactionType.Bank))
                    .Sum(transaction => transaction.amount);

            return result;
        }

        public static int GetPercentageOfAmount(long sum, bool includeCashTransactions, bool includeBankTransactions)
        {
            var amountPerMonth = AmountPerMonth(includeCashTransactions, includeBankTransactions);

            var wholePart = Convert.ToInt32((float) sum / amountPerMonth * 100);
            var fraction = (float) sum / amountPerMonth * 100 - wholePart;

            if (fraction >= .5f)
                wholePart++;
            return wholePart;
        }

        public static void UpdateAllBankTransactions(Action onFinish)
        {
            WindowsManager.EnableDisableLoadingWindow();
            MonoBankManager.GetTransactions(bankTransactions =>
            {
                if (bankTransactions == null)
                {
                    onFinish();
                    WindowsManager.EnableDisableLoadingWindow(false);
                    return;
                }

                foreach (var bankTransaction in bankTransactions)
                {
                    if (bankTransaction.amount <= 0 || UserDataManager.Instance.UserData.deletedTransactions.Any(
                        transaction =>
                            transaction.type == TransactionType.Bank &&
                            ((BankTransaction) transaction).id == bankTransaction.id
                    ))
                        continue;

                    var time = bankTransaction.time;

                    var yearlyTransaction =
                        UserDataManager.Instance.UserData.transactions.FirstOrDefault(item => item.year == time.Year);
                    if (yearlyTransaction == null)
                    {
                        UserDataManager.Instance.UserData.transactions.Add(new YearlyTransactions
                        {
                            year = time.Year
                        });
                        yearlyTransaction = UserDataManager.Instance.UserData.transactions.Last();
                    }

                    var monthlyTransaction =
                        yearlyTransaction.transactions.FirstOrDefault(item => item.month == time.Month);
                    if (monthlyTransaction == null)
                    {
                        yearlyTransaction.transactions.Add(new MonthlyTransaction
                        {
                            month = time.Month
                        });
                        monthlyTransaction = yearlyTransaction.transactions.Last();
                    }

                    var dailyTransaction =
                        monthlyTransaction.transactions.FirstOrDefault(transaction => transaction.day == time.Day);
                    if (dailyTransaction == null)
                    {
                        monthlyTransaction.transactions.Add(new DailyTransaction
                        {
                            day = time.Day
                        });
                        dailyTransaction = monthlyTransaction.transactions.Last();
                    }


                    var dayBankTransactions = dailyTransaction.bankTransactions;
                    if (!dayBankTransactions.Any(item => item.id == bankTransaction.id))
                        dailyTransaction.bankTransactions.Add(bankTransaction);
                }

                WindowsManager.EnableDisableLoadingWindow(false);
                onFinish();
            });
        }


        public static void UpdateCurrencyRates(Action onFinish)
        {
            WindowsManager.EnableDisableLoadingWindow();
            MonoBankManager.GetExchangeRates(onSuccessful =>
            {
                UserDataManager.Instance.UserData.monobankData.currenciesRate = onSuccessful;
                onFinish();
                WindowsManager.EnableDisableLoadingWindow(false);
            }, () =>
            {
                onFinish();
                WindowsManager.EnableDisableLoadingWindow(false);
            });
        }

        public static bool IsThereTransactionInMonth()
        {
            return UserDataManager.CurrentMonthlyTransaction.transactions.Count != 0;
        }
    }
}