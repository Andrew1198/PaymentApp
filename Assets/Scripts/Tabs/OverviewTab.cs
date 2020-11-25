using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using HelperWindows;
using Items;
using Managers;
using TMPro;
using UnityEngine;
#pragma warning disable 0649
namespace Tabs
{
    public class OverviewTab : Tab
    {
        [SerializeField] private TextMeshProUGUI Date;
        [SerializeField] private GameObject overviewPref;
        [SerializeField] private Transform mccContainer;
        [SerializeField] private Transform descriptionContainer;
        
        [SerializeField] private TextMeshProUGUI DayAvg;
        [SerializeField] private TextMeshProUGUI TodayAmountOrWeekAvg;
        [SerializeField] private TextMeshProUGUI WeekAmountOrMonth;
        [SerializeField] private TextMeshProUGUI Spent;

        [SerializeField] private Graph graphPrefab;

        private Graph _graph;
        
        public override void Init()
        {
            base.Init();
            Date.text = UserDataManager.SelectedDate.ToString("MMMM yyyy");
            TransactionUtils.UpdateAllBankTransactions(() =>
            {
                SetAmounts();
                SetOverviewItems(GetTransactionsPerMonthByMcc(),mccContainer);
                SetOverviewItems(GetBankTransactionsDescriptions(),descriptionContainer);
                if (_graph == null)
                    _graph = Instantiate(graphPrefab,transform.parent.parent);
                Inited = true;
            });
        }

        private void SetAmounts()
        {
            TransactionUtils.UpdateCurrencyRates(()=>
            {
                var amountPerMonth = TransactionUtils.AmountPerMonth(true, true);
                if (UserDataManager.SelectedDate.Month == DateTime.Now.Month)
                {
                    var dayAvgValue = Math.Round(amountPerMonth / (float) DateTime.Today.Day,
                        MidpointRounding.AwayFromZero);
                    var dayAvgValueUsd = Math.Round(dayAvgValue / UserDataManager.DollarRate, 1, MidpointRounding.AwayFromZero);
                    DayAvg.text = "DayAvg\n" +dayAvgValue +"("+ dayAvgValueUsd + ")" ;
                    var todayAmountUsd = Math.Round(UserDataManager.AmountPerDay / UserDataManager.DollarRate, 1,
                        MidpointRounding.AwayFromZero);
                    TodayAmountOrWeekAvg.text = "Today\n" + UserDataManager.AmountPerDay + "(" + todayAmountUsd + ")";
                    var weekAmountUsd = Math.Round(UserDataManager.AmountPerWeek / UserDataManager.DollarRate, 1,
                        MidpointRounding.AwayFromZero);
                    WeekAmountOrMonth.text = "Week\n" + UserDataManager.AmountPerWeek+ "(" + weekAmountUsd + ")";
                    var spentValue = TransactionUtils.AmountPerMonth(true, true);
                    var spentValueUsd = Math.Round(spentValue / UserDataManager.DollarRate, 1, MidpointRounding.AwayFromZero);
                    Spent.text = "Spent\n" + spentValue + "(" + spentValueUsd + ")";
                    Spent.gameObject.SetActive(true);
                }
                else
                {
                    var daysInMonth = DateTime.DaysInMonth(UserDataManager.SelectedDate.Year, UserDataManager.SelectedDate.Month);
                    var dayAvgValue = Math.Round(amountPerMonth / (float)daysInMonth,MidpointRounding.AwayFromZero);
                    var dailyAvgUsd = Math.Round(dayAvgValue / UserDataManager.DollarRate,1, MidpointRounding.AwayFromZero); 
                    DayAvg.text = "DayAvg \n" + dayAvgValue + "(" + dailyAvgUsd + ")";
                
                    const int dayCountPerWeek = 7;
                    const int weekCountPerMonth = 4;
                    const int fourWeeksDaysCount = 28;
                    var restDaysFrom4Weeks = daysInMonth - fourWeeksDaysCount;
                    var divider = weekCountPerMonth + restDaysFrom4Weeks * (float)restDaysFrom4Weeks/dayCountPerWeek;
                    var weekAwg = Math.Round(amountPerMonth / (float)divider,MidpointRounding.AwayFromZero);
                    var weekAvgUsd = Math.Round(weekAwg / UserDataManager.DollarRate, 1,MidpointRounding.AwayFromZero);
                    TodayAmountOrWeekAvg.text ="Week Avg \n" + weekAwg + "(" + weekAvgUsd + ")";

                    var weekAmountPerMonthUsd = Math.Round(amountPerMonth / UserDataManager.DollarRate, 1,MidpointRounding.AwayFromZero);
                    WeekAmountOrMonth.text = "Month\n"+amountPerMonth + "(" + weekAmountPerMonthUsd + ")";
                    Spent.gameObject.SetActive(false);
                }
            });
        }

        private void SetOverviewItems(List<OverviewItem.OverviewData> overviewsData, Transform content)
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);
            
            foreach (var overviewData in overviewsData)
            {
                var overviewItem = Instantiate(overviewPref, content).GetComponent<OverviewItem>();
                overviewItem.Init(overviewData);
            }
        }

        private List<OverviewItem.OverviewData> GetTransactionsPerMonthByMcc()
        {
            var category = new Dictionary<string,long>();
            var cashTransactionsPerMonth = UserDataManager.CurrentMonthlyTransaction._transactions
                .SelectMany(dailyTransaction => dailyTransaction._transactions);
            foreach (var transaction in cashTransactionsPerMonth)
            {
                if (!category.ContainsKey(transaction._category))
                    category[transaction._category] = transaction._count;
                else
                    category[transaction._category] += transaction._count;
            }

            var bankTransactionsPerMonth = UserDataManager.CurrentMonthlyTransaction._transactions
                .SelectMany(dailyTransaction => dailyTransaction.bankTransactions);
            foreach (var transaction in bankTransactionsPerMonth)
            {
                if (!category.ContainsKey(MonoBankManager.Instance.mccDataBase.GetDescriptionByMccCode(transaction.mcc)))
                    category[MonoBankManager.Instance.mccDataBase.GetDescriptionByMccCode(transaction.mcc)] = transaction.amount;
                else
                    category[MonoBankManager.Instance.mccDataBase.GetDescriptionByMccCode(transaction.mcc)] += transaction.amount;
            }
            
            var result = new List<OverviewItem.OverviewData>();
            
            foreach (var item in category)
            {
                result.Add(new OverviewItem.OverviewData
                {
                    CategoryName = item.Key,
                    percentageOfAmount = TransactionUtils.GetPercentageOfAmount(item.Value,true,true),
                    sum = item.Value
                });
            }
            
            

            return result.OrderByDescending(item=>item.percentageOfAmount).ToList();
        }
        /// <summary>
        /// Возрощаем все банковские транзакции использую поле description
        /// </summary>
        /// <returns></returns>
        private List<OverviewItem.OverviewData> GetBankTransactionsDescriptions() 
        {
            var category = new Dictionary<string,long>();
            
            var bankTransactionsPerMonth = UserDataManager.CurrentMonthlyTransaction._transactions
                .SelectMany(dailyTransaction => dailyTransaction.bankTransactions);
            foreach (var transaction in bankTransactionsPerMonth)
            {
                if (!category.ContainsKey(transaction.description))
                    category[transaction.description] = transaction.amount;
                else
                    category[transaction.description] += transaction.amount;
            }
            
            var result = new List<OverviewItem.OverviewData>();
            
            foreach (var item in category)
            {
                result.Add(new OverviewItem.OverviewData
                {
                    CategoryName = item.Key,
                    percentageOfAmount = TransactionUtils.GetPercentageOfAmount(item.Value,false,true),
                    sum = item.Value
                });
            }
            return result.OrderByDescending(item=>item.percentageOfAmount).ToList();
        }

        public void OpenGraphic()
        {
            _graph.Init();
        }
        
    }
}