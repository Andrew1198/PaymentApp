﻿using System;
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
        public static bool Inited;
        public readonly UserData UserData;
        private DateTime _selectedDate;
#if UNITY_EDITOR
        public UserData OldUserData;
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

        public static List<YearlyTransactions> YearlyTransactions => Instance.UserData.transactions;

        public static YearlyTransactions CurrentYearlyTransaction
        {
            get
            {
                var yearlyTransaction = Instance.UserData.transactions.FirstOrDefault(transactions =>
                    transactions.year == Instance._selectedDate.Year);
                if (yearlyTransaction == null)
                {
                    Instance.UserData.transactions.Add(new YearlyTransactions
                    {
                        year = Instance._selectedDate.Year
                    });
                    yearlyTransaction = Instance.UserData.transactions.Last();
                }

                return yearlyTransaction;
            }
        }

        public static MonthlyTransaction CurrentMonthlyTransaction
        {
            get
            {
                if (!CurrentYearlyTransaction.transactions.Any(item => item.month == Instance._selectedDate.Month))
                    CurrentYearlyTransaction.transactions.Add(new MonthlyTransaction
                    {
                        month = Instance._selectedDate.Month
                    });
                return CurrentYearlyTransaction.transactions.First(item => item.month == Instance._selectedDate.Month);
            }
        }


        public static DailyTransaction CurrentDailyTransaction
        {
            get
            {
                if (!CurrentMonthlyTransaction.transactions.Any(transaction =>
                    transaction.day == Instance._selectedDate.Day))
                    CurrentMonthlyTransaction.transactions.Add(new DailyTransaction
                    {
                        day = Instance._selectedDate.Day
                    });
                return CurrentMonthlyTransaction.transactions.First(transaction =>
                    transaction.day == Instance._selectedDate.Day);
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
            Inited = true;
        }
        
        public static void Save(bool fromCtrl_C = false)
        {
            CurrentMonthlyTransaction.transactions.RemoveAll(transaction => transaction.GetALlTypeTransactions().Count == 0);

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