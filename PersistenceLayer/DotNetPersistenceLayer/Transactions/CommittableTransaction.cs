using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Transactions
{

    ///<summary>
    ///Describes a committable transaction.
    ///</summary>
    /// <MetaDataID>{2DD76D4D-ECAB-4017-A9E7-E56816139BC4}</MetaDataID>
    public class CommittableTransaction : Transactions.Transaction
    {
        /// <MetaDataID>{D0576C61-370C-4AFC-98C1-61FE771FF411}</MetaDataID>
        public CommittableTransaction()
        {
            InternalTransaction = new TransactionRunTime();
        }

        //void OnTransactionDistributed(Transaction transaction)
        //{
        //    if (TransactionDistributed != null)
        //    {
        //        TransactionDistributed(transaction);
        //        InternalTransaction.TransactionDistributed -= new TransactionDistributedEventHandler(OnTransactionDistributed);
        //    }

        //}
        /// <MetaDataID>{666135dc-3cf1-4c8c-981f-9d39e7a333cc}</MetaDataID>
        public CommittableTransaction(Transaction transaction)
        {
            
            if (!(transaction is TransactionRunTime) && (transaction.InternalTransaction as TransactionRunTime) == null)
                throw new OOAdvantech.Transactions.TransactionException("System can't create nested transaction for transaction :" + transaction.LocalTransactionUri);

            if (transaction.InternalTransaction==null)
                InternalTransaction = new TransactionRunTime(transaction as TransactionRunTime);
            else
                InternalTransaction = new TransactionRunTime(transaction.InternalTransaction as TransactionRunTime);
            //InternalTransaction.TransactionCompletted += new TransactionCompletedEventHandler(OnTransactionCompletted);
            //InternalTransaction.TransactionDistributed += new TransactionDistributedEventHandler(OnTransactionDistributed);

        }
        public override string Marshal()
        {
            return InternalTransaction.Marshal();
        }

        ///// <MetaDataID>{6646F624-E1EB-463B-8593-1A383F3202F6}</MetaDataID>
        //void OnTransactionCompletted(Transaction transaction)
        //{
        //    if (TransactionCompletted != null)
        //    {
        //        TransactionCompletted(transaction);
        //        InternalTransaction.TransactionCompletted -= new TransactionCompletedEventHandler(OnTransactionCompletted);
        //    }
        //}


        public override bool HasChangesOnElistedObjects(bool checkOnlyPersistentClassInstances)
        {
            
                return (InternalTransaction as TransactionRunTime).HasChangesOnElistedObjects(checkOnlyPersistentClassInstances);
            }
        

        /// <MetaDataID>{FF81D5AD-C0C3-469E-9313-06D2192A5F10}</MetaDataID>
        public void Commit()
        {
            (InternalTransaction as TransactionRunTime).Commit();
        }
        /// <MetaDataID>{92AF548B-BC65-44D3-9AED-8B8098D46326}</MetaDataID>
        public Transaction Transaction
        {
            get
            {
                return InternalTransaction;
            }
        }
        /// <MetaDataID>{67ce2744-df44-4c2d-a687-86f7ab3686bb}</MetaDataID>
        public override bool WaitToComplete(int millisecondsTimeout)
        {
            return  Transaction.WaitToComplete(millisecondsTimeout);
        }

        /// <MetaDataID>{96748738-0a82-4a90-82b5-8f18efb04435}</MetaDataID>
        public override bool WaitToComplete(TimeSpan timeout)
        {
            return Transaction.WaitToComplete(timeout);
        }

        public override bool IsNestedTransaction(Transaction transaction)
        {
            return InternalTransaction.IsNestedTransaction(transaction);
        }

        /// <MetaDataID>{3bb25b3e-e018-4576-b26d-7b81edfe1d2c}</MetaDataID>
        public override List<Transaction> NestedTransactions
        {
            get { return InternalTransaction.NestedTransactions; }
        }
        public override event OOAdvantech.Transactions.TransactionCompletedEventHandler TransactionCompleted
        {
            add
            {
                InternalTransaction.TransactionCompleted += value;
            }
            remove
            {
                InternalTransaction.TransactionCompleted -= value;
            }
        }
        public override event TransactionDistributedEventHandler TransactionDistributed
        {
            add
            {
                InternalTransaction.TransactionDistributed += value;
            }
            remove
            {
                InternalTransaction.TransactionDistributed -= value;
            }
        }

        /// <MetaDataID>{81850e23-5308-4407-9a4c-5212267ca622}</MetaDataID>
        public override Transaction OriginTransaction
        {
            get
            {
                return (InternalTransaction as TransactionRunTime).OriginTransaction;
            }
        }
    }
}
