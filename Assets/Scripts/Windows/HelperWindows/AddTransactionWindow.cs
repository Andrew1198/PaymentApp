using System;
using Data;
using DefaultNamespace;
using Managers;
using TMPro;
using UnityEngine;

#pragma warning disable 0649
namespace Windows.HelperWindows
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

            var transaction = new CashTransaction
            {
                amount = long.Parse(countField.text),
                category = fromCategory,
                time = DateTime.Now,
                description = commentField.text,
            };

            UserDataManager.CurrentDailyTransaction.cashTransactions.Add(transaction);
            Events.OnUpdateTab?.Invoke();

            gameObject.SetActive(false);
        }
    }
}