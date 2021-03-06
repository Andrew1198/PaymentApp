using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class ArchivedTransaction
    {
        [SerializeField]private BankTransaction _bankTransaction;
        
        [SerializeField]private CashTransaction _cashTransaction;

        public TransactionType type;

        public TransactionBase TransactionBase => type == TransactionType.Bank
            ? (TransactionBase) _bankTransaction
            : (TransactionBase) _cashTransaction;

        public ArchivedTransaction(TransactionBase transaction)
        {
            switch (transaction.type)
            {
                case TransactionType.Bank:
                    _bankTransaction = (BankTransaction) transaction;
                    break;
                case TransactionType.Cash:
                    _cashTransaction = (CashTransaction) transaction;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            type = transaction.type;
        }
    }
}