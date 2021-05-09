using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Transactions.Bridgea
{
    internal class NativeTransactionOutcomeEvents
    {
        public event NativeTransactionEventRaise NativeTransactionEvent;

        internal System.Transactions.Transaction NativeTransaction;
        internal string TransactionUri;
        /// <MetaDataID>{72F3A9BA-B09C-452A-9547-0E69907A0349}</MetaDataID>
        public NativeTransactionOutcomeEvents(object nativeTransaction, string transactionUri)
        {
            NativeTransaction = nativeTransaction as System.Transactions.Transaction;
            TransactionUri = transactionUri;
            NativeTransaction.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(this.TransactionCompleted);
        }
        public void TransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        {
            if (NativeTransactionEvent != null)
            {
                switch (e.Transaction.TransactionInformation.Status)
                {
                    case System.Transactions.TransactionStatus.Active:
                        {
                            NativeTransactionEvent(this, NativeTransactionState.Active);
                            break;
                        }
                    case System.Transactions.TransactionStatus.Committed:
                        {
                            NativeTransactionEvent(this, NativeTransactionState.Commit);
                            break;
                        }
                    case System.Transactions.TransactionStatus.Aborted:
                        {
                            NativeTransactionEvent(this, NativeTransactionState.Abort);
                            break;
                        }
                    case System.Transactions.TransactionStatus.InDoubt:
                        {
                            NativeTransactionEvent(this, NativeTransactionState.Indoubt);
                            break;
                        }
                }
            }

        }


    }
}
