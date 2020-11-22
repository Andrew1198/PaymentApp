using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            obj.StartCoroutine(InvokeCor(method,time));
        }

        private static IEnumerator InvokeCor(Action method, float time)
        {
            yield return new WaitForSeconds(time);
            method();
        }
        public static void Invoke(Action method, float time)
        {
            Instance.StartCoroutine(InvokeCor(method,time));
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

        [Button()]
        private void TestJson()
        {
            var path = Application.persistentDataPath + "/OldData.json";
            using (var sr = new StreamReader(path))
            {
                var dict = new Dictionary<string, List<Transaction>>();
                var json = sr.ReadToEnd();
                var userData = JsonUtility.FromJson<OldUserData>(json);
                foreach (var yearlyTransactions in userData._transactions)
                foreach (var monthlyTransaction in yearlyTransactions._monthlyTransactions)
                foreach (var dailyTransaction in monthlyTransaction._transactions)
                foreach (var transaction in dailyTransaction._transactions)
                {
                    if(!dict.ContainsKey(transaction._category))
                        dict[transaction._category] = new List<Transaction>();
                    dict[transaction._category].Add(transaction);
                }
                foreach (var dictKey in dict.Keys)
                {
                    Debug.LogError(dictKey);
                }
                
            }
        }

        [Serializable]
        class OldUserData
        {
            public List<OldUserData.YearlyTransactions> _transactions = new List<OldUserData.YearlyTransactions>();
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