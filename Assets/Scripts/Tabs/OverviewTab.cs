using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Items;
using Managers;
using TMPro;
using UnityEngine;

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
            Date.text = PlayerData.SelectedDate.ToString("MMMM yyyy");
            SetAmounts();
            SetCategories();
        }

        private void SetAmounts()
        {
           
            if (PlayerData.SelectedDate.Month == DateTime.Now.Month)
            {
                DayAvg.text = "DayAvg \n" + Math.Round(PlayerData.AmountPerMonth / (float)DateTime.Today.Day,MidpointRounding.AwayFromZero);
                TodayAmountOrWeekAvg.text = "Today \n" + PlayerData.AmountPerDay;
                WeekAmountOrMonth.text = "Week \n" + PlayerData.AmountPerWeek;
            }
            else
            {
                var daysInMonth = DateTime.DaysInMonth(PlayerData.SelectedDate.Year, PlayerData.SelectedDate.Month);
                DayAvg.text = "DayAvg \n" +  Math.Round(PlayerData.AmountPerMonth / (float)daysInMonth,MidpointRounding.AwayFromZero);
                
                const int dayCountPerWeek = 7;
                const int weekCountPerMonth = 4;
                const int fourWeeksDaysCount = 28;
                var restDaysFrom4Weeks = daysInMonth - fourWeeksDaysCount;
                var divider = weekCountPerMonth + restDaysFrom4Weeks * (float)restDaysFrom4Weeks/dayCountPerWeek;
                var weekAwg = Math.Round(PlayerData.AmountPerMonth / (float)divider,MidpointRounding.AwayFromZero);
                TodayAmountOrWeekAvg.text ="Week Avg \n" + weekAwg;
                
                WeekAmountOrMonth.text = "Month\n"+PlayerData.AmountPerMonth;
            }

        }

        private void SetCategories()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);
            
            var overviewDataList = new List<OverviewItem.OverviewData>();

            var categoriesName = PlayerData.GetCategoriesName();

            foreach (var category in categoriesName)
            {
                var sum = PlayerData.GetSumByCategory(category);
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
            var amountPerMonth = PlayerData.AmountPerMonth;
            
            var wholePart = Convert.ToInt32((float)sum / amountPerMonth*100);
            var fraction = (float)sum / amountPerMonth*100 - wholePart;

            if (fraction >= .5f)
                wholePart++;
            return wholePart;
        }
    }
}