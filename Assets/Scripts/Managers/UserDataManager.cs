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
        private DateTime _selectedDate;
        public readonly UserData UserData;
        public static bool Inited;

        public static List<Wallet> Wallets => Instance.UserData._wallets;

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
        public static float DollarRate
        {
            get => Instance.UserData.dollarRate;
            set => Instance.UserData.dollarRate = value;
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
        
        
        public static void AddWallet(Wallet wallet)
        {
            Instance.UserData._wallets.Add(wallet);
            Events.OnUpdateTab?.Invoke();
        }

        public static IEnumerable<string> GetCategoriesName()
        {
            return Instance.UserData.categories.Where(data => data.IsEmpty == false).Select(data => data.Name);
        }

       
    }
}