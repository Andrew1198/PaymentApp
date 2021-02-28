using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DailyTransaction
    {
        public int day;
        public List<CashTransaction> cashTransactions = new List<CashTransaction>();
        public List<BankTransaction> bankTransactions = new List<BankTransaction>();

        public List<TransactionBase> GetALlTypeTransactions()
        {
            var result = new List<TransactionBase>();
            result.AddRange(cashTransactions);
            result.AddRange(bankTransactions);
            return result;
        }

        public bool RemoveTransaction(TransactionBase transaction)
        {
            var result = transaction.type == TransactionType.Bank
                ? bankTransactions.Remove((BankTransaction) transaction)
                : cashTransactions.Remove((CashTransaction) transaction);
            if(result)
                UserDataManager.Instance.UserData.deletedTransactions.Add(new DeletedTransaction(transaction));
            return result;
        }
    }

}