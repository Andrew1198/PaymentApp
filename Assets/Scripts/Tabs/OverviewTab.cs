using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        [SerializeField] private Transform content;
        
        [SerializeField] private TextMeshProUGUI DayAvg;
        [SerializeField] private TextMeshProUGUI TodayAmountOrWeekAvg;
        [SerializeField] private TextMeshProUGUI WeekAmountOrMonth;
        public override void Init()
        {
            base.Init();
            Date.text = UserDataManager.SelectedDate.ToString("MMMM yyyy");
            SetAmounts();
            SetCategories();
        }

        private void SetAmounts()
        {
           
            if (UserDataManager.SelectedDate.Month == DateTime.Now.Month)
            {
                DayAvg.text = "DayAvg \n" + Math.Round(UserDataManager.AmountPerMonth / (float)DateTime.Today.Day,MidpointRounding.AwayFromZero);
                TodayAmountOrWeekAvg.text = "Today \n" + UserDataManager.AmountPerDay;
                WeekAmountOrMonth.text = "Week \n" + UserDataManager.AmountPerWeek;
            }
            else
            {
                var daysInMonth = DateTime.DaysInMonth(UserDataManager.SelectedDate.Year, UserDataManager.SelectedDate.Month);
                DayAvg.text = "DayAvg \n" +  Math.Round(UserDataManager.AmountPerMonth / (float)daysInMonth,MidpointRounding.AwayFromZero);
                
                const int dayCountPerWeek = 7;
                const int weekCountPerMonth = 4;
                const int fourWeeksDaysCount = 28;
                var restDaysFrom4Weeks = daysInMonth - fourWeeksDaysCount;
                var divider = weekCountPerMonth + restDaysFrom4Weeks * (float)restDaysFrom4Weeks/dayCountPerWeek;
                var weekAwg = Math.Round(UserDataManager.AmountPerMonth / (float)divider,MidpointRounding.AwayFromZero);
                TodayAmountOrWeekAvg.text ="Week Avg \n" + weekAwg;
                
                WeekAmountOrMonth.text = "Month\n"+UserDataManager.AmountPerMonth;
            }

        }

        private void SetCategories()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);
            
            var overviewDataList = new List<OverviewItem.OverviewData>();

            var categoriesNames = new List<string>();
            var transactionsPerMonth = UserDataManager.TransactionsPerMonth;
            foreach (var transaction in transactionsPerMonth)
            {
                if(categoriesNames.Contains(transaction._category))
                    continue;
                categoriesNames.Add(transaction._category);
            }
            foreach (var category in categoriesNames)
            {
                var sum = CategoryItem.GetSumByCategory(category);
                var percentageOfAmount = sum == 0 ? sum : GetPercentageOfAmount(sum);
                var overviewData = new OverviewItem.OverviewData {sum = sum, CategoryName = category, percentageOfAmount = percentageOfAmount};
                
                overviewDataList.Add(overviewData);
            }

            overviewDataList = overviewDataList.OrderByDescending(data => data.sum).ToList();
            
           
            foreach (var overviewData in overviewDataList)
            {
                var overviewItem = Instantiate(overviewPref, content).GetComponent<OverviewItem>();
                overviewItem.Init(overviewData);
            }
        }
        

        private int GetPercentageOfAmount(int sum)
        {
            var amountPerMonth = UserDataManager.AmountPerMonth;
            
            var wholePart = Convert.ToInt32((float)sum / amountPerMonth*100);
            var fraction = (float)sum / amountPerMonth*100 - wholePart;

            if (fraction >= .5f)
                wholePart++;
            return wholePart;
        }
    }
}