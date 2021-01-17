using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Data;
using DefaultNamespace;
using GoogleFireBase;
using UnityEngine;

namespace Managers
{
    public class UserDataManager
    {
        public static UserDataManager Instance;
        public static bool Inited;
        public readonly UserData UserData;
        private DateTime _selectedDate;

        private UserDataManager(UserData data)
        {
            UserData = data;
        }

        public static List<Saving> Savings => Instance.UserData.savings;

        public static DateTime SelectedDate
        {
            get => Instance._selectedDate;
            set => Instance._selectedDate = value;
        }

        public static float DollarRate
        {
            get
            {
                return (float) Math.Round(Instance.UserData.monobankData.currenciesRate.First(item =>
                        item.currencyCodeA == (int) MonoBankManager.CurrencyCode.USD).rateSell, 2,
                    MidpointRounding.AwayFromZero);
            }
        }

        public static List<YearlyTransactions> YearlyTransactions => Instance.UserData._transactions;

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

        public static MonthlyTransaction CurrentMonthlyTransaction
        {
            get
            {
                if (!CurrentYearlyTransactions.transactions.Any(item => item.month == Instance._selectedDate.Month))
                    CurrentYearlyTransactions.transactions.Add(new MonthlyTransaction
                    {
                        month = Instance._selectedDate.Month
                    });
                return CurrentYearlyTransactions.transactions.First(item => item.month == Instance._selectedDate.Month);
            }
        }


        public static DailyTransaction CurrentDailyTransactions
        {
            get
            {
                if (!CurrentMonthlyTransaction._transactions.Any(transaction =>
                    transaction.day == Instance._selectedDate.Day))
                    CurrentMonthlyTransaction._transactions.Add(new DailyTransaction
                    {
                        day = Instance._selectedDate.Day
                    });
                return CurrentMonthlyTransaction._transactions.First(transaction =>
                    transaction.day == Instance._selectedDate.Day);
            }
        }


        public static CategoryData[] Categories
        {
            get => Instance.UserData.categories;
        }
        
        public static long AmountPerDay=>CurrentDailyTransactions._transactions.Sum(transaction => transaction.amount);
       
        
        public static long AmountPerWeek
        {
            get
            {
                long result = 0;
                for (var i = 0; i < 7; i++)
                {
                    SelectedDate = SelectedDate.Subtract(TimeSpan.FromDays(i>0?1:0));
                    result += CurrentDailyTransactions._transactions.Sum(transaction => transaction.amount);
                }
                
                SelectedDate = SelectedDate.AddDays(6);
                return result;
            }
        }

        public static void Init(UserData data)
        {
            Instance = new UserDataManager(data);
            SelectedDate = DateTime.Now;
            Inited = true;
        }

        public static int GetWeekNumber(DateTime dt)
        {
            var curr = CultureInfo.CurrentCulture;
            var week = curr.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return week;
        }


        public static void Save()
        {

            CurrentMonthlyTransaction._transactions.RemoveAll(transaction => transaction._transactions.Count == 0);

            if (!Inited)
            {
                Debug.LogError("Coudn't save userData is null");
                return;
            }

            GoogleFireBaseManager.UpdateUserData();
            var path = Path.Combine(Application.persistentDataPath, SystemInfo.deviceUniqueIdentifier + ".json");
            var json = JsonUtility.ToJson(Instance.UserData);
            if (File.Exists(path))
            {
                var oldJson = File.ReadAllText(path);
                if (json == oldJson) return;
            }

            File.WriteAllText(path, json);
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