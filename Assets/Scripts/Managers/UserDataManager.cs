using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Data;
using DefaultNamespace;
using GoogleFireBase;
using HelperScripts;
using UnityEngine;

namespace Managers
{
    public class UserDataManager
    {
        public static UserDataManager Instance;
        public readonly UserData UserData;
        private DateTime _selectedDate;
#if UNITY_EDITOR
        private readonly UserData OldUserData;
#endif

        private UserDataManager(UserData data)
        {
            UserData = data;
#if UNITY_EDITOR
            OldUserData = Utility.DeepCopy(data);
#endif
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

        public static List<YearlyTransaction> YearlyTransactions => Instance.UserData.transactions;

        public static YearlyTransaction CurrentYearlyTransaction
        {
            get
            {
                var yearlyTransaction = Instance.UserData.transactions.FirstOrDefault(transactions =>
                    transactions.year == Instance._selectedDate.Year);
                if (yearlyTransaction != null) return yearlyTransaction;
                Instance.UserData.transactions.Add(new YearlyTransaction
                {
                    year = Instance._selectedDate.Year
                });
                yearlyTransaction = Instance.UserData.transactions.Last();

                return yearlyTransaction;
            }
        }

        public static MonthlyTransaction CurrentMonthlyTransaction
        {
            get
            {
                var currentYearlyTransaction =
                    CurrentYearlyTransaction.transactions.FirstOrDefault(item =>
                        item.month == Instance._selectedDate.Month);
                if (currentYearlyTransaction != null) return currentYearlyTransaction;
                currentYearlyTransaction = new MonthlyTransaction
                {
                    month = Instance._selectedDate.Month
                };
                CurrentYearlyTransaction.transactions.Add(currentYearlyTransaction);

                return currentYearlyTransaction;
            }
        }
        

        public static DailyTransaction CurrentDailyTransaction
        {
            get
            {
                var currentMonthlyTransaction = CurrentMonthlyTransaction.transactions.FirstOrDefault(transaction =>
                    transaction.day == Instance._selectedDate.Day);
                if (currentMonthlyTransaction != null) return currentMonthlyTransaction;
                
                currentMonthlyTransaction = new DailyTransaction {
                    day = Instance._selectedDate.Day
                };

                CurrentMonthlyTransaction.transactions.Add(currentMonthlyTransaction);

                return currentMonthlyTransaction;
            }
        }


        public static CategoryData[] CurrentCategories => Instance.UserData.categories;

        public static long AmountPerDay=>CurrentDailyTransaction.GetALlTypeTransactions().Sum(transaction => transaction.amount);
       
        
        public static long AmountPerWeek
        {
            get
            {
                long result = 0;
                for (var i = 0; i < 7; i++)
                {
                    SelectedDate = SelectedDate.Subtract(TimeSpan.FromDays(i>0?1:0));
                    result += CurrentDailyTransaction.GetALlTypeTransactions().Sum(transaction => transaction.amount);
                }
                
                SelectedDate = SelectedDate.AddDays(6);
                return result;
            }
        }

        public static void Init(UserData data)
        {
            Instance = new UserDataManager(data);
            SelectedDate = DateTime.Now;
            DeleteExpiredArchivedTransactions();
        }

        private static void DeleteExpiredArchivedTransactions()
        {
            var needToDelete = Instance.UserData.archivedTransactions.Where(archivedTransaction =>
                (DateTime.Now - archivedTransaction.TransactionBase.time).TotalDays >= 40);
            foreach (var archivedTransaction in needToDelete)
                Instance.UserData.archivedTransactions.Remove(archivedTransaction);
        }
        
        public static void Save(bool fromCtrl_C = false)
        {
            CurrentMonthlyTransaction.transactions.RemoveAll(transaction => transaction.GetALlTypeTransactions().Count == 0);
            
            GoogleFireBaseManager.UpdateUserData();
            var path = Path.Combine(Application.persistentDataPath, SystemInfo.deviceUniqueIdentifier + ".json");
            var json = JsonUtility.ToJson(Instance.UserData);
            if (File.Exists(path))
            {
                var oldJson = File.ReadAllText(path);
                if (json == oldJson) return;
            }

#if UNITY_EDITOR
            if (!fromCtrl_C)
            {
                Instance.OldUserData.monobankData = Instance.UserData.monobankData;
                json = JsonUtility.ToJson(Instance.OldUserData);
                File.WriteAllText(path, json);
                Debug.Log("Light Save");
                return;
            }
#endif

            File.WriteAllText(path, json);
            Debug.Log("Save");
        }


        public static void AddSaving(Saving saving)
        {
            Instance.UserData.savings.Add(saving);
            TabManager.UpdateOpenedTab();        }
    }
}