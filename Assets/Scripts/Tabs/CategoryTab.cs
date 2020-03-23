﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Items;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Tabs
{
    public class CategoryTab : Tab
    {
        [SerializeField] private TextMeshProUGUI Date;

        [SerializeField] private List<CategoryItem> categoryItems = new List<CategoryItem>();
        
        public override void Init()
        {
            base.Init();
            Date.text = PlayerData.SelectedDate.ToString("MMMM yyyy");
            foreach (var categoryItem in categoryItems)
                categoryItem.Init();
            
        }
        
        [Button]
        private void UpdateCategoryItemsFromTemplate()
        {
            var categoryItemTemplate = transform.root.Find("Templates").Find("CategoryItem").gameObject;

            for (var i = 0; i < categoryItems.Count; ++i)
            {
                var categoryItem = categoryItems[i];
                var newCategoryItem = Instantiate(categoryItemTemplate, categoryItem.transform.parent);
                newCategoryItem.name = categoryItemTemplate.name;
                DestroyImmediate(categoryItem.gameObject);
                categoryItems[i] = newCategoryItem.GetComponent<CategoryItem>();
            }
            
            for (var i = 0; i < categoryItems.Count; ++i)
                categoryItems[i].numberOfPlace = i;
            
            Debug.Log("Success");
        }
    }
}