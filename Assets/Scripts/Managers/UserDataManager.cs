using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Data;
using DefaultNamespace;
using GoogleFireBase;
using NaughtyAttributes;
using UnityEngine;

namespace Managers
{
    public class UserDataManager
    {
        public static UserDataManager Instance;
        private DateTime _selectedDate;
        public readonly UserData UserData;
        public static bool Inited;

        public static List<Saving> Savings => Instance.UserData.savings;

        public static DateTime SelectedDate
        {
            get => Instance._selectedDate;
            set => Instance._selectedDate = value;
        }

        public static void Init(UserData data)
        {
            Instance = new UserDataManager(data);
            SelectedDate = DateTime.Now;
            Inited = true;
        }

        private UserDataManager(UserData data)
        {
            UserData = data;
        }

        public static void GetDollarRate(Action<float> result)
        {
            GetCurrenciesRate(infos =>
            {
                result((float) Math.Round(infos.First(item =>
                        item.currencyCodeA == (int) MonoBankManager.CurrencyCode.USD).rateSell, 2,
                    MidpointRounding.AwayFromZero));
            });
        }

        public static void GetCurrenciesRate(Action<CurrencyInfo[]> result)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() -
                MonoBankManager.Instance.updateInfo.LastUpdateCurrencyInfoTime >
                60 * 5)
            {
                MonoBankManager.GetExchangeRates(onSuccessful =>
                {
                    Instance.UserData.currenciesRate = onSuccessful;
                    MonoBankManager.Instance.updateInfo.LastUpdateCurrencyInfoTime =
                        DateTimeOffset.Now.ToUnixTimeSeconds();
                    result(onSuccessful);
                },onError:()=>result(Instance.UserData.currenciesRate));
            }
            else
                result(Instance.UserData.currenciesRate);
        }

        public static void SetNewBankTransactionsInData(Action onFinish)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() -
                MonoBankManager.Instance.updateInfo.LastUpdateCurrencyInfoTime >
                60)
            {
                MonoBankManager.GetTransactions(bankTransactions =>
                {
                    if (bankTransactions == null)
                    {
                        onFinish();
                        return;
                    }

                    foreach (var bankTransaction in bankTransactions)
                    {
                        if(bankTransaction.amount <= 0)
                            continue;
                        var time = DateTimeOffset.FromUnixTimeSeconds(bankTransaction.time).LocalDateTime;
                        if (!Instance.UserData._transactions.Any(item => item.year == time.Year))
                        {
                            Instance.UserData._transactions.Add(new YearlyTransactions
                            {
                                year = time.Year
                            });
                        }

                        var yearlyTrans = Instance.UserData._transactions.First(item => item.year == time.Year);
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
                            dayTrans.bankTransactions.Add(bankTransaction);
                        }
                    }

                    onFinish();
                });
            }
        }

        public static YearlyTransactions CurrentYearlyTransactions
        {
            get
            {
                var tmp = Instance.UserData._transactions.FirstOrDefault(transactions =>
                    transactions.year == Instance._selectedDate.Year);
                if (tmp == null)
                {
                    Instance.UserData._transactions.Add(new YearlyTransactions
                    {
                        year = Instance._selectedDate.Year
                    });
                    tmp = Instance.UserData._transactions.Last();
                }

                return tmp;
            }
        }

        public static List<DailyTransaction> CurrentMonthlyTransaction
        {
            get
            {
                if (!CurrentYearlyTransactions.transactions.Any(item => item.month == Instance._selectedDate.Month))
                {
                    CurrentYearlyTransactions.transactions.Add(new MonthlyTransaction
                    {
                        month =  Instance._selectedDate.Month
                    });
                }
                return CurrentYearlyTransactions.transactions.First(item => item.month == Instance._selectedDate.Month)._transactions;
            }
        }




        public static List<Transaction> CurrentDayilyTrasactions
        {
            get
            {
                if (!CurrentMonthlyTransaction.Any(transaction => transaction.day == Instance._selectedDate.Day))
                    CurrentMonthlyTransaction.Add(new DailyTransaction
                    {
                        day = Instance._selectedDate.Day
                    });
                return CurrentMonthlyTransaction.First(transaction => transaction.day == Instance._selectedDate.Day)
                    ._transactions;
            }
        }

        public static List<Transaction> TransactionsPerMonth
        {
            get
            {
                var result = new List<Transaction>();
                foreach (var dailyTransaction in CurrentMonthlyTransaction)
                    result.AddRange(dailyTransaction._transactions);

                return result;
            }
        }

        public static void AddTransactionWindow(Transaction transaction)
        {

        }

        public static CategoryData[] Categories
        {
            get => Instance.UserData.categories;
        }

        public static int AmountPerMonth => UserDataManager.CurrentMonthlyTransaction
            .SelectMany(dailyTransaction => dailyTransaction._transactions).Sum(transaction => transaction._count);

        public static int AmountPerDay => UserDataManager.CurrentDayilyTrasactions.Sum(transaction => transaction._count);

        public static int AmountPerWeek
        {
            get
            {
                var currentWeek = GetWeekNumber(DateTime.Now);

                var result = 0; 
                foreach (var dailyTransaction in CurrentMonthlyTransaction)
                foreach (var transaction in dailyTransaction._transactions)
                {
                    if (GetWeekNumber(transaction.Time) == currentWeek)
                        result += transaction._count;
                }

                return result;
            }
        }
        public static int GetWeekNumber(DateTime dt)
        {
            CultureInfo curr= CultureInfo.CurrentCulture;
            int week = curr.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return week;
        }
        
        
        public static void Save()
        {
            if (!Inited)
            {
                Debug.LogError("Coudn't save userData is null");
                return;
            }
            var path = Path.Combine(Application.persistentDataPath, SystemInfo.deviceUniqueIdentifier + ".json");
            var json = JsonUtility.ToJson(Instance.UserData, true);
            if (File.Exists(path))
            {
                var oldJson = File.ReadAllText(path);
                if (json == oldJson) return;
            }
            File.WriteAllText(path, json);
            GoogleFireBaseManager.UpdateUserData();
            Debug.Log("Save");
        }
        
        
        public static void AddSaving(Saving saving)
        {
            Instance.UserData.savings.Add(saving);
            Events.OnUpdateTab?.Invoke();
        }

        public static IEnumerable<string> GetCategoriesName()
        {
            return Instance.UserData.categories.Where(data => data.IsEmpty == false).Select(data => data.Name);
        }




       
    }
}