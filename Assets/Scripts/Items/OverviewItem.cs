using System;
using HelperWindows;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
namespace Items
{
    public class OverviewItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI categoryName;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI percentCount;
        [SerializeField] private TextMeshProUGUI count;
        public void Init(OverviewData data)
        {
            TransactionUtils.UpdateCurrencyRates(() =>
            {
                categoryName.text = data.CategoryName;
                slider.value = data.percentageOfAmount;
                percentCount.text = $"<color=red>{data.percentageOfAmount}%</color=red>";
                var countUsd = Math.Round(data.sum / UserDataManager.DollarRate,1, MidpointRounding.AwayFromZero);
                count.text = data.sum.ToString() + "(" + countUsd + ")";
            });
           
               
            
           
        }
        
        public class OverviewData
        {
            public string CategoryName;
            public long sum;
            public int percentageOfAmount;
        }
    }
}