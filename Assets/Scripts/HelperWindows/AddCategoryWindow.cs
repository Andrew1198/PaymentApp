using DefaultNamespace;
using Managers;
using TMPro;
using UnityEngine;
#pragma warning disable 0649
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
            var category = UserDataManager.Categories[numberOfPlace];
            
          
            category.IsEmpty = false;
            category.Name = categoryName.text;
            Events.OnUpdateTab?.Invoke();
            gameObject.SetActive(false);
            categoryName.text = null;
        }
    }
}