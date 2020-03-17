using System.Collections.Generic;
using System.Linq;
using Data;
using Items;
using Managers;
using TMPro;
using UnityEngine;

namespace Tabs
{
    public class CategoryTab : Tab
    {
        [SerializeField] private TextMeshProUGUI Date;
        [SerializeField] private List<CategoryItem> CategoryItems;
        public override void Init()
        {
            base.Init();
            Date.text = PlayerData.SelectedDate.ToString("MMMM yyyy");
            SetCategories();
        }

        private void SetCategories()
        {
            var transactions = PlayerData.TransactionsPerMonth;
            var categoriesTransactions = new Dictionary<Category,List<Transaction>>();
            foreach (var categoriesTransaction in transactions)
            {
                if (!categoriesTransactions.ContainsKey(categoriesTransaction.Category))
                {
                    categoriesTransactions[categoriesTransaction.Category] = new List<Transaction>();
                }
                categoriesTransactions[categoriesTransaction.Category].AddRange(categoriesTransaction.Transactions);
            }
            
            
            foreach (var categoryItem in CategoryItems)
            {
                if (!categoriesTransactions.ContainsKey(categoryItem.category))
                {
                    categoryItem.sum = 0;
                    continue;
                }

                var count = 0;
                foreach (var transaction in categoriesTransactions[categoryItem.category])
                    count += transaction._count;

                categoryItem.sum = count;
            }
        }
    }
}