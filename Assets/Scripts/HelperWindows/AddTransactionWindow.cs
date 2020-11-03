using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DefaultNamespace;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
namespace HelperWindows
{
    public class AddTransactionWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI categoryName;
        [SerializeField] private TMP_InputField countField;
        [SerializeField] private TMP_InputField commentField;

        [HideInInspector] public string fromCategory;

        public void Open()
        {
            gameObject.SetActive(true);
            countField.text = null;
            commentField.text = null;
            categoryName.text = fromCategory;
        }


        public void OnOk()
        {
            UserDataManager.SelectedDate = DateTime.Now;

            var count = int.Parse(countField.text);

            var transaction = new Transaction
            {
                _category = fromCategory,
                _count = count,
                _comment = commentField.text,
                Time = DateTime.Now
            };
            UserDataManager.CurrentDayilyTrasactions.Add(transaction);
            Events.OnUpdateTab?.Invoke();
            
            gameObject.SetActive(false);
        }
    }
}