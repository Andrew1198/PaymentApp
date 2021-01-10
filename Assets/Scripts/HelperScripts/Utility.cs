using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using DefaultNamespace;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace HelperScripts
{
    public class Utility : Singleton<Utility>
    {
        public static void Invoke(MonoBehaviour obj, Action method, float time)
        {
            obj.StartCoroutine(InvokeCor(method, time));
        }

        private static IEnumerator InvokeCor(Action method, float time)
        {
            yield return new WaitForSeconds(time);
            method();
        }

        private static IEnumerator InvokeCor(Action method, Func<bool> predicate)
        {
            yield return new WaitUntil(predicate);
            method();
        }

        public static void Invoke(Action method, float time)
        {
            Instance.StartCoroutine(InvokeCor(method, time));
        }

        public static void Invoke(Action method, Func<bool> predicate)
        {
            Instance.StartCoroutine(InvokeCor(method, predicate));
        }


        public static void StartCor(IEnumerator routine)
        {
            Instance.StartCoroutine(routine);
        }

        [Button]
        private void EnableLoadingScreen()
        {
            Events.EnableLoadingScreen.Invoke();
        }

        [Button]
        private void DisableLoadingScreen()
        {
            Events.DisableLoadingScreen.Invoke();
        }

        [Button]
        private void ChangeOldData()
        {
            var path = Application.persistentDataPath + "/OldData.json";
            string json;
            UserData userData;
            using (var sr1 =
                new StreamReader(Application.persistentDataPath + "/c4fe2be7891c4c31ad7f145dfff12949d5c1191b.json"))
            {
                json = sr1.ReadToEnd();
                userData = JsonUtility.FromJson<UserData>(json);
            }

            OldUserData oldUserData;
            using (var sr = new StreamReader(path))
            {
                json = sr.ReadToEnd();
                oldUserData = JsonUtility.FromJson<OldUserData>(json);
            }

            var dict = new Dictionary<string, List<Transaction>>();
            foreach (var yearlyTransactions in oldUserData._transactions)
            foreach (var monthlyTransaction in yearlyTransactions._monthlyTransactions)
            foreach (var dailyTransaction in monthlyTransaction._transactions)
            foreach (var transaction in dailyTransaction._transactions)
            {
                if (!dict.ContainsKey(transaction._category))
                    dict[transaction._category] = new List<Transaction>();
                dict[transaction._category].Add(transaction);
            }

            for (var i = 0; i < dict["Subway"].Count; i++)
                dict["Subway"][i]._category = "Transport";

            dict["Transport"].AddRange(dict["Subway"]);
            dict.Remove("Subway");

            foreach (var key in dict.Keys)
            foreach (var transaction in dict[key])
            {
                var year = transaction.Time.Year;
                var month = transaction.Time.Month;
                var day = transaction.Time.Day;
                if (!userData._transactions.Any(yearlyTransaction =>
                    yearlyTransaction.year == year))
                    userData._transactions.Add(new YearlyTransactions
                    {
                        year = year
                    });
                var yearTransaction =
                    userData._transactions.First(yearlyTransaction =>
                        yearlyTransaction.year == year);
                if (!yearTransaction.transactions.Any(monthTransaction => monthTransaction.month == month))
                    yearTransaction.transactions.Add(new MonthlyTransaction
                    {
                        month = month
                    });
                var monthlyTransaction =
                    yearTransaction.transactions.First(monthTransaction => monthTransaction.month == month);

                if (!monthlyTransaction._transactions.Any(dayTransa => dayTransa.day == day))
                    monthlyTransaction._transactions.Add(new DailyTransaction
                    {
                        day = day
                    });
                var dayTransaction =
                    monthlyTransaction._transactions.First(dayTrans => dayTrans.day == day);
                dayTransaction._transactions.Add(transaction);
            }

            using (var sr = new StreamWriter(Application.persistentDataPath + "/newData.json"))
            {
                sr.Write(JsonUtility.ToJson(userData));
            }
        }


        [Button]
        private void DeleteEmptyObjects()
        {
            UserData userData;
            using (var sr1 =
                new StreamReader(Application.persistentDataPath + "/old.json"))
            {
                var json = sr1.ReadToEnd();
                userData = JsonUtility.FromJson<UserData>(json);
            }

            userData._transactions.RemoveAll(transaction => transaction.transactions.Count == 0);
            foreach (var yearlyTransactionse in userData._transactions)
                yearlyTransactionse.transactions.RemoveAll(transaction => transaction._transactions.Count == 0);

            foreach (var yearlyTransaction in userData._transactions)
            foreach (var monthlyTransaction in yearlyTransaction.transactions)
                monthlyTransaction._transactions.RemoveAll(transaction =>
                    transaction._transactions.Count == 0 && transaction.bankTransactions.Count == 0);

            using (var sr1 = new StreamWriter(Application.persistentDataPath + "/new.json"))
            {
                sr1.Write(JsonUtility.ToJson(userData));
            }

            Debug.LogError("success");
        }

        [Serializable]
        private class OldUserData
        {
            public List<YearlyTransactions> _transactions = new List<YearlyTransactions>();

            [Serializable]
            public class YearlyTransactions
            {
                public int year;
                public List<MonthlyTransaction> _monthlyTransactions = new List<MonthlyTransaction>();

                [Serializable]
                public class MonthlyTransaction
                {
                    public List<DailyTransaction> _transactions = new List<DailyTransaction>();
                }
            }
        }
    }
}