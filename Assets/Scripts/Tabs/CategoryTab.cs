using System;
using System.Collections.Generic;
using HelperWindows;
using Items;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
#pragma warning disable 0649
namespace Tabs
{
    [Serializable]
    class WholeAmount
    {
        public TextMeshProUGUI usdText;
        public TextMeshProUGUI uahText;
    }
    
    public class CategoryTab : Tab
    {
        [SerializeField] private TextMeshProUGUI Date;
        [SerializeField] private WholeAmount wholeAmount;

        [SerializeField] private List<CategoryItem> categoryItems = new List<CategoryItem>();
        
        public override void Init()
        {
            base.Init();
            Date.text = UserDataManager.SelectedDate.ToString("MMMM yyyy");
            for (var i = 0; i < categoryItems.Count; i++)
            {
                categoryItems[i].Init(UserDataManager.Categories[i]);
            }
            TransactionUtils.UpdateCurrencyRates(() =>
            {
                SetWholeAmount();
            });
            Inited = true;
        }

        private void SetWholeAmount()
        {
            var amountPerMonth = TransactionUtils.AmountPerMonth(true,false);
            wholeAmount.uahText.text = amountPerMonth.ToString();

            wholeAmount.usdText.text =
                ((int) Math.Round(amountPerMonth / UserDataManager.DollarRate, MidpointRounding.AwayFromZero))
                .ToString();
        }
        
        
        [Button]
        private void UpdateCategoryItemsFromTemplate()
        {
            var categoryItemTemplate = transform.Find("Template").Find("CategoryItem").gameObject;

            for (var i = 0; i < categoryItems.Count; ++i)
            {
                var categoryItem = categoryItems[i];
                var newCategoryItem = Instantiate(categoryItemTemplate, categoryItem.transform.parent);
                newCategoryItem.name = categoryItemTemplate.name;
                DestroyImmediate(categoryItem.gameObject);
                categoryItems[i] = newCategoryItem.GetComponent<CategoryItem>();
            }
            
            Debug.Log("Success!");
        }
    }
}