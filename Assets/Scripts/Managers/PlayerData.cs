using System;
using System.Collections.Generic;
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
                    for(var i =0 ;i < 12; ++i)
                        tmp._monthlyTransactions[i] = new MonthlyTransaction();
                }

                return tmp;
            }
        }

        public static List<DailyTransaction> CurrentMonthlyTransaction=>CurrentYearlyTransactions[Instance._selectedDate.Month - 1]._transactions;
            
        

        public static List<Transaction> CurrentDayilyTrasactions
        {
            get
            {
                if(!CurrentMonthlyTransaction.Any(transaction => transaction.day == Instance._selectedDate.Day))
                    CurrentMonthlyTransaction.Add(new DailyTransaction
                    {
                        day = Instance._selectedDate.Day
                    });
                return CurrentMonthlyTransaction.First(transaction => transaction.day == Instance._selectedDate.Day)._transactions;
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
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                var path = Path.Combine(Application.persistentDataPath, SystemInfo.deviceUniqueIdentifier + ".json");
                var json = JsonUtility.ToJson(userData,true);
                File.WriteAllText(path, json);
                Debug.LogError("Save");
            }
        }

        public static void AddWallet(Wallet wallet)
        {
            Instance.userData._wallets.Add(wallet);
            TabManager.UpdateTab();
        }

    }
}