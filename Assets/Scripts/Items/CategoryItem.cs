using System;
using System.Linq;
using Data;
using HelperWindows;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
    public class CategoryItem : MonoBehaviour , IPointerDownHandler
    {
        [SerializeField] private AddTransactionWindow addTransactionWindow;
        [SerializeField] private AddCategoryWindow addCategoryWindow;

        [SerializeField] private Transform notFoundTransform;
        [SerializeField] private Transform existingTransform;
         public int numberOfPlace;
        
         public TextMeshProUGUI category;
         public TextMeshProUGUI sum;

         private bool _isEmpty;
         
         public void Init()
         {
             notFoundTransform.gameObject.SetActive(false);
             existingTransform.gameObject.SetActive(false);
             var categoryData = PlayerData.GetCategories[numberOfPlace];
             _isEmpty = categoryData.IsEmpty;
             if (!_isEmpty)
             {
                 category.text = categoryData.Name;
                 sum.text = PlayerData.GetSumByCategory(category.text).ToString();
                 existingTransform.gameObject.SetActive(true);
             }
             else
                 notFoundTransform.gameObject.SetActive(true);
         }


         public void OnPointerDown(PointerEventData eventData)
         {
             if (_isEmpty)
             {
                 addCategoryWindow.numberOfPlace = numberOfPlace;
                 addCategoryWindow.Open();
             }
             else
             {
                 addTransactionWindow.fromCategory = category.text;
                 addTransactionWindow.Open();
             }
         }
    }
}