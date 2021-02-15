using DefaultNamespace;
using Managers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649
namespace Windows.HelperWindows
{
    public class AddCategoryWindow : WindowBase
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
            var category = UserDataManager.CurrentCategories[numberOfPlace];


            category.IsEmpty = false;
            category.Name = categoryName.text;
            TabManager.UpdateOpenedTab();
            gameObject.SetActive(false);
            categoryName.text = null;
        }
    }
}