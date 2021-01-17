using System;
using Data;
using DefaultNamespace;
using Managers;
using TMPro;
using UnityEngine;

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

            var transaction = new CashTransaction(new CashTransaction.CashTransactionInitData
            {
                amount = count,
                category = fromCategory,
                comment = commentField.text,
                time = DateTime.Now.ToBinary(),
                type = TransactionType.Cash
            });
            UserDataManager.CurrentDailyTransactions._transactions.Add(transaction);
            Events.OnUpdateTab?.Invoke();

            gameObject.SetActive(false);
        }
    }
}