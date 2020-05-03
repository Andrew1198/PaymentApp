using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Data;
using HelperWindows;
using Items;
using UnityEngine;

namespace Managers
{
    public class PlayerData : Singleton<PlayerData>
    {
        public UserData userData;
        private DateTime _selectedDate;

        public static List<Wallet> Wallets => Instance.userData._wallets;

        public static DateTime SelectedDate
        {
            get => Instance._selectedDate;
            set => Instance._selectedDate = value;
        }

        public static float DollarRate
        {
            get => Instance.userData.dollarRate;
            set => Instance.userData.dollarRate = value;
        }
        public static YearlyTransactions CurrentYearlyTransactions
        {
            get
            {
                var tmp = Instance.userData._transactions.FirstOrDefault(transactions =>
                    transactions.year == Instance._selectedDate.Year);
                if (tmp == null)
                {
                    Instance.userData._transactions.Add(new YearlyTransactions
                    {
                        year = Instance._selectedDate.Year
                    });
                    tmp = Instance.userData._transactions.Last();
                    for (var i = 0; i < 12; ++i)
                        tmp._monthlyTransactions[i] = new MonthlyTransaction();
                }

                return tmp;
            }
        }

        public static List<DailyTransaction> CurrentMonthlyTransaction =>
            CurrentYearlyTransactions[Instance._selectedDate.Month - 1]._transactions;



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

        public static CategoryData[] GetCategories
        {
            get => Instance.userData.categories;
        }

        public static int AmountPerMonth => PlayerData.CurrentMonthlyTransaction
            .SelectMany(dailyTransaction => dailyTransaction._transactions).Sum(transaction => transaction._count);

        public static int AmountPerDay => PlayerData.CurrentDayilyTrasactions.Sum(transaction => transaction._count);

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
        
        private new void Awake()
        {
            base.Awake();
            _selectedDate = DateTime.Now;
            var path = Path.Combine(Application.persistentDataPath, SystemInfo.deviceUniqueIdentifier + ".json");
            if (!File.Exists(path))
                userData = new UserData();
            else
            {
                var json = File.ReadAllText(path);
                userData = JsonUtility.FromJson<UserData>(json);
            }
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
               Save();
                Debug.LogError("Save");
            }
            #endif
        }
        private void Save()
        {
            var path = Path.Combine(Application.persistentDataPath, SystemInfo.deviceUniqueIdentifier + ".json");
            var json = JsonUtility.ToJson(userData, true);
            File.WriteAllText(path, json);
        }
        
        #if !UNITY_EDITOR && UNITY_ANDROID
        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
                Save();
            else
            {
                _selectedDate = DateTime.Now;
                TabManager.UpdateTab();
            }
        }
        #endif
        

        public static void AddWallet(Wallet wallet)
        {
            Instance.userData._wallets.Add(wallet);
            TabManager.UpdateTab();
        }

        public static IEnumerable<string> GetCategoriesName()
        {
            return Instance.userData.categories.Where(data => data.IsEmpty == false).Select(data => data.Name);
        }

        public static int GetSumByCategory(string category)
        {
            var transactions = PlayerData.TransactionsPerMonth;

            return transactions.Where(transaction => transaction._category == category)
                .Sum(transaction => transaction._count);
        }
    }
}