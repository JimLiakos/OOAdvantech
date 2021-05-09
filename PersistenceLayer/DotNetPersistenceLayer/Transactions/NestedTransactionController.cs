namespace OOAdvantech.Transactions
{
    using OOAdvantech.Synchronization;
    using Remoting;
    using System;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
    using MarshalByRefObject = System.MarshalByRefObject;
#endif

    /// <MetaDataID>{0EB8FBB8-8F9F-44E5-A3C0-7B847C823009}</MetaDataID>
    internal class NestedTransactionController : MarshalByRefObject, Remoting.IExtMarshalByRefObject, System.Transactions.IEnlistmentNotification, ObjectsStateManager
    {
        /// <MetaDataID>{2d7cd0b8-731c-45dd-a629-719f1482ddb8}</MetaDataID>
        ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();

        /// <MetaDataID>{d677bbfb-f154-4f60-a99b-ddc2472653e5}</MetaDataID>
        internal NestedTransactionController(IEnlistmentsController nestedEnlistmentsController)
        {
            NestedEnlistmentsController = nestedEnlistmentsController;
            NestedEnlistmentsController.TransactionCompletted += new TransactionEndedEventHandler(NestedEnlistmentsController_TransactionCompletted);

        }
        /// <MetaDataID>{79ecf8d9-b9fe-4e56-bd91-58c45548fbef}</MetaDataID>
        internal System.Threading.ManualResetEvent TransactionCompletted = new System.Threading.ManualResetEvent(false);

        /// <MetaDataID>{34426ad6-dd76-4b21-abb4-2a2db41ba6dd}</MetaDataID>
        void NestedEnlistmentsController_TransactionCompletted(TransactionStatus transactionState)
        {
            TransactionCompletted.Set();

        }
        /// <MetaDataID>{ee653b8c-2981-45da-a24c-60262c45e3a7}</MetaDataID>
        private object NativeTransaction;
        /// <MetaDataID>{c23d0067-0ca9-4144-80ce-d29e885f237b}</MetaDataID>
        public void AttachToNativeTransaction(System.Transactions.Transaction nativeTransaction)
        {
#if DISTRIBUTED_TRANSACTIONS
            LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (nativeTransaction == null)
                    throw new OOAdvantech.Transactions.TransactionException("There isn't native transaction to attach.");

                if (NativeTransaction != null && nativeTransaction.TransactionInformation.LocalIdentifier != (NativeTransaction as System.Transactions.Transaction).TransactionInformation.LocalIdentifier)
                    throw new OOAdvantech.Transactions.TransactionException("Transaction context already  attached to native transaction.");
                if (NativeTransaction != null)
                    return;
                NativeTransaction = nativeTransaction;
                nativeTransaction.EnlistVolatile(this, System.Transactions.EnlistmentOptions.EnlistDuringPrepareRequired);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
#endif
        }

        /// <MetaDataID>{295201ED-6795-414B-AE39-3333664C9461}</MetaDataID>
        IEnlistmentsController NestedEnlistmentsController;

#if !PORTABLE
        #region IEnlistmentNotification Members

        /// <MetaDataID>{7ca8f54d-6070-4d88-a409-ca453712bdce}</MetaDataID>
        public void Commit(System.Transactions.Enlistment enlistment)
        {
            enlistment.Done();
        }

        /// <MetaDataID>{e0c18a7a-cb46-4648-85a2-5d9458a7ef1a}</MetaDataID>
        public void InDoubt(System.Transactions.Enlistment enlistment)
        {
            enlistment.Done();
        }

        /// <MetaDataID>{08905af1-2b5c-4a9e-86f5-f702078b7325}</MetaDataID>
        public void Prepare(System.Transactions.PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        /// <MetaDataID>{17f7ebaf-e1bb-4bf5-9aec-ce7d4db9995f}</MetaDataID>
        public void Rollback(System.Transactions.Enlistment enlistment)
        {
            enlistment.Done();
            if (!TransactionCompletted.WaitOne(System.TimeSpan.FromTicks(1), false))
            {
                if (NestedEnlistmentsController.NativeTransaction != null &&
                    (NestedEnlistmentsController.NativeTransaction as System.Transactions.Transaction).TransactionInformation.Status == System.Transactions.TransactionStatus.Active)
                {
                    (NestedEnlistmentsController.NativeTransaction as System.Transactions.Transaction).Rollback();
                }
                else
                {
                    if (NestedEnlistmentsController.Status != ObjectsStateManagerStatus.AbortDone &&
                        NestedEnlistmentsController.Status != ObjectsStateManagerStatus.CommitDone)
                    {
                        NestedEnlistmentsController.AbortRequest();
                    }
                }
            }
        }

        #endregion
#endif

        #region ObjectsStateManager Members

        /// <MetaDataID>{7dc2af86-df7f-40d5-8549-d677c0dc8382}</MetaDataID>
        public void AbortRequest(ITransactionEnlistment transactionEnlistment)
        {
            //TODO αύτος ο χρόνος θα πρέπει να ειναι συνάρτιση του χρόνου της transaction
#if !DeviceDotNet
            if (!TransactionCompletted.WaitOne(System.TimeSpan.FromMilliseconds(500), false))
            {
                if (NestedEnlistmentsController.NativeTransaction != null &&
                    (NestedEnlistmentsController.NativeTransaction as System.Transactions.Transaction).TransactionInformation.Status == System.Transactions.TransactionStatus.Active)
                {
                    (NestedEnlistmentsController.NativeTransaction as System.Transactions.Transaction).Rollback();
                }
                else
                {
                    if (NestedEnlistmentsController.Status != ObjectsStateManagerStatus.AbortDone &&
                        NestedEnlistmentsController.Status != ObjectsStateManagerStatus.CommitDone)
                    {
                        NestedEnlistmentsController.AbortRequest();
                    }
                }
            }
#else
            if (!TransactionCompletted.WaitOne(500))
            {
                if (NestedEnlistmentsController.Status != ObjectsStateManagerStatus.AbortDone &&
                    NestedEnlistmentsController.Status != ObjectsStateManagerStatus.CommitDone)
                {
                    NestedEnlistmentsController.AbortRequest();
                }

            }
#endif
            transactionEnlistment.AbortRequestDone();

        }

        /// <MetaDataID>{b0bd923b-a958-44f6-bf63-30e6f0455854}</MetaDataID>
        public void CommitRequest(ITransactionEnlistment transactionEnlistment)
        {
            //TODO αύτος ο χρόνος θα πρέπει να ειναι συνάρτιση του χρόνου της transaction
            //αν και εδώ δεν θα φτάσει γιατί θα κοπεί από το prepare αν δεν είναι completted
#if !DeviceDotNet
            if (!TransactionCompletted.WaitOne(System.TimeSpan.FromMilliseconds(500), false))
            {

                if (NestedEnlistmentsController.NativeTransaction != null &&
                    (NestedEnlistmentsController.NativeTransaction as System.Transactions.Transaction).TransactionInformation.Status == System.Transactions.TransactionStatus.Active)
                {
                    (NestedEnlistmentsController.NativeTransaction as System.Transactions.Transaction).Rollback();
                }
                else
                {
                    if (NestedEnlistmentsController.Status != ObjectsStateManagerStatus.AbortDone &&
                        NestedEnlistmentsController.Status != ObjectsStateManagerStatus.CommitDone)
                    {
                        NestedEnlistmentsController.AbortRequest();
                    }
                }
                if ((NativeTransaction as System.Transactions.Transaction) != null)
                    (NativeTransaction as System.Transactions.Transaction).Rollback();

                transactionEnlistment.AbortRequestDone();

            }
            else
                transactionEnlistment.CommitRequestDone();

#else
            if (!TransactionCompletted.WaitOne(500))
            {
                if (NestedEnlistmentsController.Status != ObjectsStateManagerStatus.AbortDone &&
                    NestedEnlistmentsController.Status != ObjectsStateManagerStatus.CommitDone)
                {
                    NestedEnlistmentsController.AbortRequest();
                }
                transactionEnlistment.AbortRequestDone();
            }
            else
                transactionEnlistment.CommitRequestDone();
#endif

        }
        public bool HasChangesOnElistedObjects(bool checkOnlyPersistentClassInstances)
        {
            
            {
                if (NestedEnlistmentsController.Status != ObjectsStateManagerStatus.AbortDone &&
                    NestedEnlistmentsController.Status != ObjectsStateManagerStatus.CommitDone)
                {
                    return NestedEnlistmentsController.HasChangesOnElistedObjects(checkOnlyPersistentClassInstances);
                }
                else
                    return false;

            }
        }

        /// <MetaDataID>{a31d50f2-c031-474e-afe4-32178cf1d726}</MetaDataID>
        public void PrepareRequest(ITransactionEnlistment transactionEnlistment)
        {
            //TODO αύτος ο χρόνος θα πρέπει να ειναι συνάρτιση του χρόνου της transaction
#if !DeviceDotNet
            if (!TransactionCompletted.WaitOne(System.TimeSpan.FromMilliseconds(500), false))
            {

                if (NestedEnlistmentsController.NativeTransaction != null &&
                    (NestedEnlistmentsController.NativeTransaction as System.Transactions.Transaction).TransactionInformation.Status == System.Transactions.TransactionStatus.Active)
                {
                    (NestedEnlistmentsController.NativeTransaction as System.Transactions.Transaction).Rollback();
                }
                else
                {
                    if (NestedEnlistmentsController.Status != ObjectsStateManagerStatus.AbortDone &&
                        NestedEnlistmentsController.Status != ObjectsStateManagerStatus.CommitDone)
                    {
                        NestedEnlistmentsController.AbortRequest();
                    }
                }
                if ((NativeTransaction as System.Transactions.Transaction) != null)
                    (NativeTransaction as System.Transactions.Transaction).Rollback(new System.Exception("Nested transaction doesn't completed"));

                transactionEnlistment.AbortRequestDone();

            }
            else
                transactionEnlistment.PrepareRequestDone();
#else
            if (!TransactionCompletted.WaitOne(500))
            {
                if (NestedEnlistmentsController.Status != ObjectsStateManagerStatus.AbortDone &&
                    NestedEnlistmentsController.Status != ObjectsStateManagerStatus.CommitDone)
                {
                    NestedEnlistmentsController.AbortRequest();
                }
                transactionEnlistment.AbortRequestDone();

            }
            else
                transactionEnlistment.PrepareRequestDone();
#endif


        }

        #endregion
    }
}
