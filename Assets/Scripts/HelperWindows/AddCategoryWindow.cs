using Managers;
using TMPro;
using UnityEngine;

namespace HelperWindows
{
    public class AddCategoryWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField categoryName;

        public int numberOfPlace;
        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
            categoryName.text = null;
        }

        public void OnOk()
        {
            var category = PlayerData.GetCategories[numberOfPlace];
            
            //todo delete all old category transactions
            category.IsEmpty = false;
            category.Name = categoryName.text;
            TabManager.UpdateTab();
            gameObject.SetActive(false);
            categoryName.text = null;
        }
    }
}