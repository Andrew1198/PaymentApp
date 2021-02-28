using System;
using System.Collections.Generic;
using System.Linq;
using Windows.WindowsData;
using HelperWindows;
using Items;
using Managers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649
namespace Windows.Tabs
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
        
        
        public override void Open(Dictionary<string, object> DynamicWindowData = null)
        {
            base.Open(DynamicWindowData);
            Date.text = UserDataManager.SelectedDate.ToString("MMMM yyyy");
            SetAmounts();
            SetOverviewItems(GetTransactionsPerMonthByMcc(), mccContainer);
            SetOverviewItems(GetBankTransactionsDescriptions(), descriptionContainer);
        }
        
        private void SetAmounts()
        {
            var amountPerMonth = TransactionUtils.AmountPerMonth(true, true);
                if (UserDataManager.SelectedDate.Month == DateTime.Now.Month)
                {
                    var dayAvgValue = Math.Round(amountPerMonth / (float) DateTime.Today.Day,
                        MidpointRounding.AwayFromZero);
                    var dayAvgValueUsd = Math.Round(dayAvgValue / UserDataManager.DollarRate, 1,
                        MidpointRounding.AwayFromZero);
                    DayAvg.text = "DayAvg\n<color=#FF5567>" + dayAvgValue + "(" + dayAvgValueUsd + ")</color>";
                    var todayAmountUsd = Math.Round(UserDataManager.AmountPerDay / UserDataManager.DollarRate, 1,
                        MidpointRounding.AwayFromZero);
                    TodayAmountOrWeekAvg.text = "Today\n<color=#FF5567>" + UserDataManager.AmountPerDay + "(" + todayAmountUsd + ")</color>";
                    var weekAmountUsd = Math.Round(UserDataManager.AmountPerWeek / UserDataManager.DollarRate, 1,
                        MidpointRounding.AwayFromZero);
                    WeekAmountOrMonth.text = "Week\n<color=#FF5567>" + UserDataManager.AmountPerWeek + "(" + weekAmountUsd + ")</color>";
                    var spentValue = TransactionUtils.AmountPerMonth(true, true);
                    var spentValueUsd = Math.Round(spentValue / UserDataManager.DollarRate, 1,
                        MidpointRounding.AwayFromZero);
                    Spent.text = "Spent\n<color=#FF5567>" + spentValue + "(" + spentValueUsd + ")</color>";
                    Spent.gameObject.SetActive(true);
                }
                else
                {
                    var daysInMonth = DateTime.DaysInMonth(UserDataManager.SelectedDate.Year,
                        UserDataManager.SelectedDate.Month);
                    var dayAvgValue = Math.Round(amountPerMonth / (float) daysInMonth, MidpointRounding.AwayFromZero);
                    var dailyAvgUsd = Math.Round(dayAvgValue / UserDataManager.DollarRate, 1,
                        MidpointRounding.AwayFromZero);
                    DayAvg.text = "DayAvg \n<color=#FF5567>" + dayAvgValue + "(" + dailyAvgUsd + ")</color>";

                    const int dayCountPerWeek = 7;
                    const int weekCountPerMonth = 4;
                    const int fourWeeksDaysCount = 28;
                    var restDaysFrom4Weeks = daysInMonth - fourWeeksDaysCount;
                    var divider = weekCountPerMonth + restDaysFrom4Weeks * (float) restDaysFrom4Weeks / dayCountPerWeek;
                    var weekAwg = Math.Round(amountPerMonth / divider, MidpointRounding.AwayFromZero);
                    var weekAvgUsd = Math.Round(weekAwg / UserDataManager.DollarRate, 1, MidpointRounding.AwayFromZero);
                    TodayAmountOrWeekAvg.text = "Week Avg \n<color=#FF5567>" + weekAwg + "(" + weekAvgUsd + ")</color>";

                    var weekAmountPerMonthUsd = Math.Round(amountPerMonth / UserDataManager.DollarRate, 1,
                        MidpointRounding.AwayFromZero);
                    WeekAmountOrMonth.text = "Month\n<color=#FF5567>" + amountPerMonth + "(" + weekAmountPerMonthUsd + ")</color>";
                    Spent.gameObject.SetActive(false);
                }
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
            var category = new Dictionary<string, long>();
            var cashTransactionsPerMonth = UserDataManager.CurrentMonthlyTransaction.transactions
                .SelectMany(dailyTransaction => dailyTransaction.cashTransactions);
            foreach (var transaction in cashTransactionsPerMonth)
                if (!category.ContainsKey(transaction.category))
                    category[transaction.category] = transaction.amount;
                else
                    category[transaction.category] += transaction.amount;

            var bankTransactionsPerMonth = UserDataManager.CurrentMonthlyTransaction.transactions
                .SelectMany(dailyTransaction => dailyTransaction.bankTransactions);
            foreach (var transaction in bankTransactionsPerMonth)
                if (!category.ContainsKey(transaction.category)
                )
                    category[transaction.category] =
                        transaction.amount;
                else
                    category[transaction.category] +=
                        transaction.amount;

            var result = new List<OverviewItem.OverviewData>();

            foreach (var item in category)
                result.Add(new OverviewItem.OverviewData
                {
                    CategoryName = item.Key,
                    percentageOfAmount = TransactionUtils.GetPercentageOfAmount(item.Value, true, true),
                    sum = item.Value
                });


            return result.OrderByDescending(item => item.percentageOfAmount).ToList();
        }

        /// <summary>
        ///     Возрощаем все банковские транзакции использую поле description
        /// </summary>
        /// <returns></returns>
        private List<OverviewItem.OverviewData> GetBankTransactionsDescriptions()
        {
            var category = new Dictionary<string, long>();

            var bankTransactionsPerMonth = UserDataManager.CurrentMonthlyTransaction.transactions
                .SelectMany(dailyTransaction => dailyTransaction.bankTransactions);
            foreach (var transaction in bankTransactionsPerMonth)
                if (!category.ContainsKey(transaction.description))
                    category[transaction.description] = transaction.amount;
                else
                    category[transaction.description] += transaction.amount;

            var result = new List<OverviewItem.OverviewData>();

            foreach (var item in category)
                result.Add(new OverviewItem.OverviewData
                {
                    CategoryName = item.Key,
                    percentageOfAmount = TransactionUtils.GetPercentageOfAmount(item.Value, false, true),
                    sum = item.Value
                });
            return result.OrderByDescending(item => item.percentageOfAmount).ToList();
        }

        public void OpenGraphic()
        {
            WindowsManager.OpenWindow<GraphWindowData>();
        }
    }
}