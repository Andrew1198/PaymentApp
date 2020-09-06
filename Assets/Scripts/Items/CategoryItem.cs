using System;
using System.Linq;
using Data;
using HelperWindows;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
#pragma warning disable 0649
namespace Items
{
    public class CategoryItem : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
    {
        [SerializeField] private AddTransactionWindow addTransactionWindow;
        [SerializeField] private AddCategoryWindow addCategoryWindow;

        [SerializeField] private Transform notFoundTransform;
        [SerializeField] private Transform existingTransform;
        [HideInInspector] public int numberOfPlace;
        
         public TextMeshProUGUI category;
         public TextMeshProUGUI sum;

         private bool _isEmpty;
         
         
         private bool _pointerDown;
         private float _downClickTime;
         private float _requireHold = 1f;
         
         public void Init(CategoryData data)
         {
             notFoundTransform.gameObject.SetActive(false);
             existingTransform.gameObject.SetActive(false);
             _isEmpty = data.IsEmpty;
             numberOfPlace = data.NumberOfPlace;
             if (!_isEmpty)
             {
                 category.text = data.Name;
                 sum.text = GetSumByCategory(category.text).ToString();
                 existingTransform.gameObject.SetActive(true);
             }
             else
                 notFoundTransform.gameObject.SetActive(true);
         }

         private void Update()
         {
             if (_pointerDown)
             {
                 if (Time.time >= _downClickTime + _requireHold)
                 {
                     OnLongTouch();
                     Reset();
                 }
             }
         }

         public void OnPointerDown(PointerEventData eventData)
         {
             _pointerDown = true;
             _downClickTime = Time.time;
         }

         public void OnPointerUp(PointerEventData eventData)
         {
             if (Time.time <= _downClickTime + .3f)
                 OnClick();
             _pointerDown = false;
         }
         private void Reset()
         {
             _pointerDown = false;
         }

         private void OnClick()
         {
             if (_isEmpty)
             {
                 addCategoryWindow.numberOfPlace = numberOfPlace;
                 addCategoryWindow.Open();
             }
             else
             {
                 if (DateTime.Now.Month != UserDataManager.SelectedDate.Month)
                     return;
                 
                 addTransactionWindow.fromCategory = category.text;
                 addTransactionWindow.Open();
             } 
         }

         private void OnLongTouch()
         {
             addCategoryWindow.numberOfPlace = numberOfPlace;
             addCategoryWindow.Open();
         }
         
         public static int GetSumByCategory(string category)
         {
             var transactions = UserDataManager.TransactionsPerMonth;

             return transactions.Where(transaction => transaction._category == category)
                 .Sum(transaction => transaction._count);
         }
    }
}