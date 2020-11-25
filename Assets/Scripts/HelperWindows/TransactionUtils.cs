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

        public static List<Transaction> CashTransactionsPerMonth
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
            if (includeBankTransactions)
                result += UserDataManager.CurrentMonthlyTransaction._transactions
                    .SelectMany(dailyTransaction => dailyTransaction.bankTransactions)
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
                        if (bankTransaction.amount >= 0)
                            continue;
                        var time = DateTimeOffset.FromUnixTimeSeconds(bankTransaction.time).LocalDateTime;
                        if (!UserDataManager.Instance.UserData._transactions.Any(item => item.year == time.Year))
                        {
                            UserDataManager.Instance.UserData._transactions.Add(new YearlyTransactions
                            {
                                year = time.Year
                            });
                        }

                        var yearlyTrans =
                            UserDataManager.Instance.UserData._transactions.First(item => item.year == time.Year);
                        if (!yearlyTrans.transactions.Any(item => item.month == time.Month))
                        {
                            yearlyTrans.transactions.Add(new MonthlyTransaction
                            {
                                month = time.Month
                            });
                        }

                        var monthlyTrans = yearlyTrans.transactions.First(item => item.month == time.Month);
                        if (!monthlyTrans._transactions.Any(item => item.day == time.Day))
                        {
                            monthlyTrans._transactions.Add(new DailyTransaction
                            {
                                day = time.Day
                            });
                        }

                        var dayTrans = monthlyTrans._transactions.First(item => item.day == time.Day);
                        if (!dayTrans.bankTransactions.Any(item => item.id == bankTransaction.id))
                        {
                            bankTransaction.amount = ((long) Math.Round((float) bankTransaction.amount / 100,
                                                         MidpointRounding.AwayFromZero)) * -1;
                            dayTrans.bankTransactions.Add(bankTransaction);
                        }
                    }

                    Events.DisableLoadingScreen.Invoke();
                    onFinish();
                });
        }

       
        
        public static void UpdateCurrencyRates(Action onFinish)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() -
                UserDataManager.Instance.UserData.monobankData.updateInfo.LastUpdateCurrencyInfoTime >
                60 * 5)
            {
                UserDataManager.Instance.UserData.monobankData.updateInfo.LastUpdateCurrencyInfoTime =
                    DateTimeOffset.Now.ToUnixTimeSeconds();
                Events.EnableLoadingScreen.Invoke();
                MonoBankManager.GetExchangeRates(onSuccessful =>
                {
                    UserDataManager.Instance.UserData.monobankData.currenciesRate = onSuccessful;
                    onFinish();
                    Events.DisableLoadingScreen.Invoke();
                }, onError: () =>
                {
                    onFinish();
                    Events.DisableLoadingScreen.Invoke();
                });
            }
            else
                onFinish();
        }
    }
}