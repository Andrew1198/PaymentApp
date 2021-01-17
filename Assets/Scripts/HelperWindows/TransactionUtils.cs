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
                foreach (var dailyTransaction in UserDataManager.CurrentMonthlyTransaction._transactions)
                    result.AddRange(dailyTransaction._transactions
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
                foreach (var dailyTransaction in UserDataManager.CurrentMonthlyTransaction._transactions)
                    result.AddRange(dailyTransaction._transactions
                        .Where(transaction => transaction.type == TransactionType.Bank)
                        .Select(transaction => (BankTransaction)transaction).ToList());
                
                return result;
            }
        }


        public static long AmountPerMonth(bool includeCashTransactions, bool includeBankTransactions)
        {
            long result = 0;
            if (includeCashTransactions)
                result += UserDataManager.CurrentMonthlyTransaction._transactions
                    .SelectMany(dailyTransaction => dailyTransaction._transactions.Where(transaction=>transaction.type == TransactionType.Cash))
                    .Sum(transaction => transaction.amount);
            if (includeBankTransactions)
                result += UserDataManager.CurrentMonthlyTransaction._transactions
                    .SelectMany(dailyTransaction => dailyTransaction._transactions.Where(transaction=>transaction.type == TransactionType.Bank))
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
            Events.EnableLoadingScreen.Invoke();
            MonoBankManager.GetTransactions(bankTransactions =>
            {
                if (bankTransactions == null)
                {
                    onFinish();
                    Events.DisableLoadingScreen.Invoke();
                    return;
                }
                
                foreach (var bankTransaction in bankTransactions)
                {
                        if (bankTransaction.amount >= 0 || UserDataManager.Instance.UserData.deletedTransactions.Any(
                            transaction =>
                                transaction.type == TransactionType.Bank && ((BankTransaction) transaction).id == bankTransaction.id
                        ))
                            continue;
                        
                        var time = bankTransaction.Time;

                        YearlyTransactions yearlyTransactions;
                        if (!UserDataManager.Instance.UserData._transactions.Any(item => item.year == time.Year))
                        {
                            UserDataManager.Instance.UserData._transactions.Add(new YearlyTransactions
                            {
                                year = time.Year
                            });
                            yearlyTransactions = UserDataManager.Instance.UserData._transactions.Last();
                        }
                        else
                            yearlyTransactions = UserDataManager.Instance.UserData._transactions.First(item => item.year == time.Year);
                        

                        if (!yearlyTrans.transactions.Any(item => item.month == time.Month))
                            yearlyTrans.transactions.Add(new MonthlyTransaction
                            {
                               month = time.Month
                            });

                        var monthlyTrans =
                            yearlyTrans.transactions.First(transaction => transaction.month == time.Month);

                        if (!monthlyTrans._transactions.Any(transaction => transaction.day == time.Day))
                            monthlyTrans._transactions.Add(new DailyTransaction
                            {
                                day = time.Day
                            });

                        var dayTrans = monthlyTrans._transactions.First(item => item.day == time.Day);
                        
                        var dayBankTransactions = GetTransactionsByType<BankTransaction>(dayTrans._transactions);
                        if (!dayBankTransactions.Any(item => item.id == bankTransaction.id))
                        {
                            bankTransaction.amount = (long) Math.Round((float) bankTransaction.amount / 100,
                                                         MidpointRounding.AwayFromZero) * -1;
                            dayTrans._transactions.Add(bankTransaction);
                        }
                    }
                
                Events.DisableLoadingScreen.Invoke();
                onFinish();
            });
        }


        public static void UpdateCurrencyRates(Action onFinish)
        {
            Events.EnableLoadingScreen.Invoke();
            MonoBankManager.GetExchangeRates(onSuccessful =>
            {
                UserDataManager.Instance.UserData.monobankData.currenciesRate = onSuccessful;
                onFinish();
                Events.DisableLoadingScreen.Invoke();
            }, () =>
            {
                onFinish();
                Events.DisableLoadingScreen.Invoke();
            });
        }

        public static bool IsThereTransactionInMonth()
        {
            return UserDataManager.CurrentMonthlyTransaction._transactions.Count != 0;
        }

        public static List<T> GetTransactionsByType<T>(List<TransactionBase> transactions)where T : TransactionBase
        {
            TransactionType type = TransactionType.Bank;
            if (typeof(T) == typeof(BankTransaction))
                type = TransactionType.Bank;
            else
            if (typeof(T) == typeof(CashTransaction))
                type = TransactionType.Cash;

            return transactions.Where(transaction => transaction.type == type).Select(transaction => (T) transaction).ToList();
        }
    }
}