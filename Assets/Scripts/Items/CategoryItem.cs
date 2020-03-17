using Data;
using HelperWindows;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
    public class CategoryItem : MonoBehaviour , IPointerDownHandler
    {
         public Category category;
         public int sum;

         [SerializeField] private AddTransactionWindow addTransactionWindow;
         public void OnPointerDown(PointerEventData eventData)
         {
             addTransactionWindow.fromCategory = category;
             addTransactionWindow.Init();
         }
    }
}