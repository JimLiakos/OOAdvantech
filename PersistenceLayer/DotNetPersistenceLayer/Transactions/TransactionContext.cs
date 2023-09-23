namespace OOAdvantech.Transactions
{
#if DeviceDotNet
    using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
    using LockCookie = OOAdvantech.Synchronization.LockCookie;
    using Remoting;
#else
#if OOAdvantech
    using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
    using LockCookie = OOAdvantech.Synchronization.LockCookie;
    using System;
    using System.Threading;
    using System.Diagnostics;
#else

        using ReaderWriterLock = System.Threading.ReaderWriterLock;
        using LockCookie = System.Threading.LockCookie;
#endif
#endif


    internal delegate void ObjectLockEventHandler(object _object, System.Reflection.MemberInfo[] memberInfo, Transaction transaction);

    /// <MetaDataID>{B890CDE1-EB6F-4584-80CB-59B6027B18A2}</MetaDataID>
    /// <summary>Transaction context is the place which live the object where participate in transaction.
    /// Transaction context makes the work to commit the state of objects that live in. Also take the job of roll back the state of objects if transaction aborted. </summary>
    internal class TransactionContext : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ObjectsStateManager, ITransactionContext, System.Transactions.IEnlistmentNotification
    {
        static internal event ObjectLockEventHandler ObjectLocked;

        /// <MetaDataID>{5b575caa-2f27-4f34-9186-d3190f539bd6}</MetaDataID>
        static internal void ObjectIsLocked(object lockedObject, System.Reflection.MemberInfo[] memberInfo, TransactionRunTime transaction)
        {

            if (ObjectLocked != null)
            {
                try
                {
                    ObjectLocked(lockedObject, memberInfo, transaction);
                }
                catch (System.Exception error)
                {
                }
            }


        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{95364797-339E-448C-AB30-19FB4E03E4F9}</MetaDataID>
        private Collections.Generic.List<ITransactionContextExtender> _Extenders = new OOAdvantech.Collections.Generic.List<ITransactionContextExtender>();
        /// <MetaDataID>{BB480D20-A4A2-4CA2-B158-8310633D4D89}</MetaDataID>
        public Collections.Generic.List<ITransactionContextExtender> Extenders
        {
            get
            {
                return new Collections.Generic.List<ITransactionContextExtender>(_Extenders);
            }

        }

        // /// <MetaDataID>{CBC4EAAE-D378-404E-A43A-07ED8CF219E1}</MetaDataID>

        //// private System.Threading.ManualResetEvent TransactionCompletedEvent = new System.Threading.ManualResetEvent(false);
        // /// <MetaDataID>{7086F7E0-4951-419A-AD51-5E6F2491C545}</MetaDataID>
        // internal bool WaitToComplete(int millisecondsTimeout)
        // {
        //     return Transaction.WaitToComplete(millisecondsTimeout);
        // }

        /// <MetaDataID>{1580DF0B-39FA-4BF5-95F3-EECE5667E468}</MetaDataID>
        ~TransactionContext()
        {
            int gg = 0;
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{14727FA3-FD5B-460F-8061-944369AB7B52}</MetaDataID>
        private object _NativeTransaction;
        /// <summary>Define the transaction of native transaction system in this case a COM+ transaction; </summary>
        /// <MetaDataID>{435783C3-3DC3-4E97-891D-9C3FE47232CD}</MetaDataID>
        public object NativeTransaction
        {
            set
            {
            }
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _NativeTransaction;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }





        /// <MetaDataID>{52B5227C-B8DB-45EE-B616-516E532D28DD}</MetaDataID>
        public void AttachToNativeTransaction(System.Transactions.Transaction nativeTransaction)
        {
#if DISTRIBUTED_TRANSACTIONS
            LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (nativeTransaction == null)
                    throw new OOAdvantech.Transactions.TransactionException("There isn't native transaction to attach.");

                if (_NativeTransaction != null && nativeTransaction.TransactionInformation.LocalIdentifier != (NativeTransaction as System.Transactions.Transaction).TransactionInformation.LocalIdentifier)
                    throw new OOAdvantech.Transactions.TransactionException("Transaction context already  attached to native transaction.");
                if (_NativeTransaction != null)
                    return;
                _NativeTransaction = nativeTransaction;
                //nativeTransaction.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(OnNativeTransactionCompleted);
                nativeTransaction.EnlistVolatile(this, System.Transactions.EnlistmentOptions.EnlistDuringPrepareRequired);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
#endif
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{FEFC7A99-8FED-4D86-8C95-813D821D6193}</MetaDataID>
        private OOAdvantech.Transactions.ObjectsStateManagerStatus _Status;
        /// <MetaDataID>{A5D077E3-52A1-412D-B39D-FF6DDA2CB44A}</MetaDataID>
        public OOAdvantech.Transactions.ObjectsStateManagerStatus Status
        {
            set
            {
            }
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _Status;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{67ED0863-6630-4B77-A11B-A99782EC60A6}</MetaDataID>
        /// <summary>This member used for the thread safety. </summary>

        internal ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();


        /// <MetaDataID>{F7789439-99AE-419B-AED6-440253B37A3F}</MetaDataID>
        internal System.Collections.Generic.List<object> EnlistObjects = new System.Collections.Generic.List<object>(10);

        /// <summary>This operation enlists the object in transaction context 
        /// and mark the state of object for the case of roll back (abort). 
        /// If the object or some not shared members of them are enlisted in 
        /// transaction other than the transaction of transaction context 
        /// then blocked for predefined time to end the other transaction. 
        /// If the time expired then raise exception "Time out expired". 
        /// Object is locked from other transaction" </summary>
        ///<param name="transactionedObject">
        ///This parameter defines the enlisted object.
        ///</param>
        ///<param name="memberInfo">
        ///Sometimes the object enlisted in transction partially
        ///This parameter defines the member which will control the transaction 
        ///</param>
        /// <MetaDataID>{0A4F40AB-5109-4F02-B6F0-8E0BB47BF5C2}</MetaDataID>
        public void EnlistObject(object transactionedObject, System.Reflection.MemberInfo memberInfo)
        {
            if (_Status == ObjectsStateManagerStatus.CommitRequest
               || _Status == ObjectsStateManagerStatus.CommitDone
               || _Status == ObjectsStateManagerStatus.AbortRequest
               || _Status == ObjectsStateManagerStatus.AbortDone)
            {
                throw new TransactionException("Transaction System can't enlist object when its state is " + _Status.ToString());
            }
            LockedObjectEntry.EnlistObject(transactionedObject, this, memberInfo);
        }

        /// <MetaDataID>{5C975CB0-F3BB-4FA3-9029-A87DE3E74550}</MetaDataID>
        public TransactionContext(TransactionRunTime transaction)
        {
            if (transaction == null)
                throw new OOAdvantech.Transactions.TransactionException("There isn't transaction");

            Transaction = transaction;
            //TODO check την περίπτωση που το transaction.InternalTransaction !=null
            if (transaction.InternalTransaction == null)
                (transaction as TransactionRunTime).TransactionContext = this;


            object obj = OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService;
            foreach (ITransactionContextProvider transactionContextProvider in TransactionManager.TransactionedContextProviders)
                _Extenders.Add(transactionContextProvider.CreateTransactionContext(transaction));

            //AttachToNativeTransaction(System.Transactions.Transaction.Current);

        }
        /// <MetaDataID>{65E79C2D-A902-454C-B362-6F420B5DACD3}</MetaDataID>
        /// <summary>Define the transaction which belong the transaction context. </summary>
        public TransactionRunTime Transaction;




        /// <MetaDataID>{82434D44-97EB-4119-8B5B-5C9475737BB8}</MetaDataID>
        void UpdateStatus(ObjectsStateManagerStatus status)
        {

            LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                _Status = status;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }

        /// <MetaDataID>{A0BF0D7A-7C01-4F33-AD81-4B05E0F4E682}</MetaDataID>
        internal static void InformEventLog(System.Exception Error)
        {
#if !DeviceDotNet
            if (!System.Diagnostics.EventLog.SourceExists("TransactionSystem", "."))
            {
                System.Diagnostics.EventLog.CreateEventSource("TransactionSystem", "OOTransactionSystem");
            }

            //TODO γεμισει με message το log file τοτε παράγει exception
            System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
            myLog.Source = "TransactionSystem";
            if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);

            System.Diagnostics.Debug.WriteLine(
                Error.Message + Error.StackTrace);
            myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
            System.Exception InerError = Error.InnerException;
            while (InerError != null)
            {
                //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow

                System.Diagnostics.Debug.WriteLine(
                    InerError.Message + InerError.StackTrace);
                myLog.WriteEntry(InerError.Message + InerError.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                InerError = InerError.InnerException;
            }
#endif
        }






#if DISTRIBUTED_TRANSACTIONS
        /// <summary>This operation catch the event of native transaction (COM+ transaction). </summary>
        /// <MetaDataID>{07CE4946-8B1E-4021-97D3-24FE1E65F780}</MetaDataID>
        public void OnNativeTransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        {

            //if (e.Transaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Committed)
            //{
            //    if (Extender != null)
            //    {
            //        try
            //        {
            //            Extender.Commit();
            //        }
            //        catch (System.Exception Error)
            //        {
            //            int rr = 0;
            //        }
            //    }

            //    UpdateStatus(ObjectsStateManagerStatus.CommitDone);
            //    TransactionCompletedEvent.Set();
            //    TransactionEnlistment = null;

            //}
            //else
            //{
            //    ObjectsStateManagerStatus status = Status;

            //    if (status == ObjectsStateManagerStatus.AbortDone)
            //        return;
            //    if (status == ObjectsStateManagerStatus.OnAction
            //        || status == ObjectsStateManagerStatus.MakeDurableDone
            //        || status == ObjectsStateManagerStatus.PrepareDone)
            //    {
            //        UpdateStatus(ObjectsStateManagerStatus.AbortRequest);
            //    }
            //    else
            //    {
            //        if (status != ObjectsStateManagerStatus.CommitRequest && status != ObjectsStateManagerStatus.CommitDone)
            //            UpdateStatus(ObjectsStateManagerStatus.AbortRequest);
            //    }

            //}
        }
#endif
        #region ObjectsStateManager Members
        /// <summary>The transaction manager irequests from  objects state manager to prepare the transaction to commit. This is phase one of the two-phase commit protocol </summary>
        /// <MetaDataID>{746B4325-5617-4608-8BFE-73BE998D0B2C}</MetaDataID>
        public void PrepareRequest(ITransactionEnlistment transactionEnlistment)
        {
            if (LogicalThread.HasOpenScoops(Transaction))
            {
                if (Status == ObjectsStateManagerStatus.OnAction)
                {

                    if (!LogicalThread.WaitToComplete(Transaction))
                    {
                        Transaction.Abort(new System.Exception("There are open trnasaction scoops"));
                        return;
                    }
                    else
                    {

                    }
                }


            }
            if (Status == ObjectsStateManagerStatus.PrepareDone)
                transactionEnlistment.PrepareRequestDone();

            if (Status != ObjectsStateManagerStatus.OnAction)
            {
                if (NativeTransaction != null)
                    (NativeTransaction as System.Transactions.Transaction).Rollback();
                return;
            }
            UpdateStatus(ObjectsStateManagerStatus.PrepareRequest);
            System.Exception exception = null;
            using (SystemStateTransition StateTransition = new SystemStateTransition(Transaction))
            {
                try
                {

                    if (_Extenders.Count > 0)
                    {

                        if (NativeTransaction != null)
                        {
                            using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(NativeTransaction as System.Transactions.Transaction))
                            {
                                try
                                {
                                    foreach (ITransactionContextExtender extender in _Extenders)
                                        extender.Prepare();
                                    transactionScope.Complete();
                                }
                                catch (System.Exception error)
                                {
#if !DeviceDotNet
                                    if (System.Transactions.Transaction.Current != null)
                                        System.Transactions.Transaction.Current.Rollback(error);
#endif
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            foreach (ITransactionContextExtender extender in _Extenders)
                                extender.Prepare();
                        }
                    }
                }
                catch (System.Exception Error)
                {
                    //if (Transaction.AbortReasons == null)
                    //    Transaction.AbortReasons = new OOAdvantech.Collections.Generic.List<System.Exception>();

                    Transaction.Abort(Error);
                    InformEventLog(Error);
                    exception = Error;

                }
                StateTransition.Consistent = true;
            }
            if (exception == null)
            {
                if (Status == ObjectsStateManagerStatus.PrepareRequest)
                {
                    UpdateStatus(ObjectsStateManagerStatus.PrepareDone);
                    transactionEnlistment.PrepareRequestDone();
                }
            }
        }

        ///// <summary>The transaction manager irequests from  objects state manager to prepare the transaction to commit. This is phase one of the two-phase commit protocol</summary>
        ///// <MetaDataID>{345E0B76-F234-484F-AD46-5BC2E90A43CA}</MetaDataID>
        //public void PrepareRequest(ITransactionEnlistment transactionEnlistment)
        //{
        //    if (Status != ObjectsStateManagerStatus.MakeDurableDone)
        //    {
        //        if (NativeTransaction != null)
        //            (NativeTransaction as System.Transactions.Transaction).Rollback();
        //        return;
        //    }
        //    UpdateStatus(ObjectsStateManagerStatus.PrepareRequest);
        //    System.Exception exception = null;
        //    using (SystemStateTransition StateTransition = new SystemStateTransition(Transaction))
        //    {
        //        try
        //        {
        //            bool tt = false;
        //            if (tt)
        //                throw new OOAdvantech.Transactions.TransactionException("df");

        //            if (Extender != null)
        //            {
        //                if (NativeTransaction != null)
        //                {

        //                    using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(NativeTransaction as System.Transactions.Transaction))
        //                    {
        //                        Extender.Prepare();
        //                        transactionScope.Complete();
        //                    }
        //                }
        //                else
        //                    Extender.Prepare();

        //            }
        //        }
        //        catch (System.Exception Error)
        //        {
        //            InformEventLog(Error);
        //            exception = Error;
        //        }

        //        StateTransition.Consistent = true;
        //    }
        //    if (exception != null)
        //        Transaction.Abort(exception);
        //    else
        //    {
        //        if (Status == ObjectsStateManagerStatus.PrepareRequest)
        //        {
        //            UpdateStatus(ObjectsStateManagerStatus.PrepareDone);
        //            transactionEnlistment.PrepareRequestDone();
        //        }
        //    }

        //}

        /// <summary>Implements the CommitRequest from ObjectsSateManager.
        /// The transaction manager irequests from objects state manager to commit the transaction. This is phase two of the two-phase commit protocol. </summary>
        /// <MetaDataID>{C4971C98-37C9-44A1-8888-B5190369EAC1}</MetaDataID>
        public void CommitRequest(ITransactionEnlistment transactionEnlistment)
        {

            ObjectsStateManagerStatus status = Status;
            if (status == ObjectsStateManagerStatus.PrepareDone)
            {
                UpdateStatus(ObjectsStateManagerStatus.CommitRequest);
                foreach (ITransactionContextExtender extender in _Extenders)
                    extender.Commit();
                System.Collections.Generic.List<ITransactionNotification> transactionNotificationObjects = new System.Collections.Generic.List<ITransactionNotification>();
                ReleaseEnlistObjects(false, transactionNotificationObjects);
                UpdateStatus(ObjectsStateManagerStatus.CommitDone);
                //                TransactionCompletedEvent.Set();
                transactionEnlistment.CommitRequestDone();

                _Extenders = null;
                foreach (ITransactionNotification transactionNotificationObject in transactionNotificationObjects)
                {
                    try
                    {
                        transactionNotificationObject.OnTransactionCompletted(Transaction);
                    }
                    catch (System.Exception error)
                    {
                    }
                }
            }
            else
                throw new OOAdvantech.Transactions.TransactionException("The objects state manager isn't in the prepare done state");

        }

        /// <summary>Implements the AbortRequest from ObjectsSateManager.
        /// Transaction manager requests from objects state manager to abort the transaction. </summary>
        /// <MetaDataID>{C973C7A8-124A-46BD-A748-4D95F2B23DA5}</MetaDataID>
        public void AbortRequest(ITransactionEnlistment transactionEnlistment)
        {

            ObjectsStateManagerStatus status = Status;
            if (status == ObjectsStateManagerStatus.AbortRequest ||
                status == ObjectsStateManagerStatus.CommitRequest ||
                status == ObjectsStateManagerStatus.CommitDone ||
                status == ObjectsStateManagerStatus.AbortDone)
                return;
            try
            {
                foreach (ITransactionContextExtender extender in _Extenders)
                    extender.Abort();

            }
            catch (System.Exception AbortError)
            {
            }
            System.Collections.Generic.List<ITransactionNotification> transactionNotificationObjects = new System.Collections.Generic.List<ITransactionNotification>();
            ReleaseEnlistObjects(true, transactionNotificationObjects);
            UpdateStatus(ObjectsStateManagerStatus.AbortDone);
            //            TransactionCompletedEvent.Set();
            if (transactionEnlistment != null)
                transactionEnlistment.AbortRequestDone();
            _Extenders = null;
            foreach (ITransactionNotification transactionNotificationObject in transactionNotificationObjects)
            {
                try
                {
                    transactionNotificationObject.OnTransactionCompletted(Transaction);
                }
                catch (System.Exception error)
                {
                }
            }
        }

        #endregion
#if !PORTABLE
        #region IEnlistmentNotification Members

        /// <MetaDataID>{86762180-D8E0-4DCC-BEDA-128B11053094}</MetaDataID>
        public void Commit(System.Transactions.Enlistment enlistment)
        {
            try
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    UpdateStatus(ObjectsStateManagerStatus.CommitRequest);
                    try
                    {
                        foreach (ITransactionContextExtender extender in _Extenders)
                            extender.Commit();
                    }
                    catch (System.Exception error)
                    {

                    }
                    System.Collections.Generic.List<ITransactionNotification> transactionNotificationObjects = new System.Collections.Generic.List<ITransactionNotification>();
                    ReleaseEnlistObjects(false, transactionNotificationObjects);
                    UpdateStatus(ObjectsStateManagerStatus.CommitDone);
                    int def = 12;
                    //                    TransactionCompletedEvent.Set();
                    enlistment.Done();

                    Transaction.TransactionCompleted += delegate (Transaction transaction)
                     {
                         var ss = def;
                         using (SystemStateTransition suppressTransactioStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                         {
                             foreach (ITransactionNotification transactionNotificationObject in transactionNotificationObjects)
                             {
                                 try
                                 {
                                     transactionNotificationObject.OnTransactionCompletted(Transaction);
                                 }
                                 catch (System.Exception error)
                                 {
                                 }
                             }

                         }

                     };

                    stateTransition.Consistent = true;
                }
                _Extenders = null;
            }
            catch (System.Exception error)
            {
                //TODO Ενημέρωση Application event viewer
            }


        }

        /// <MetaDataID>{7E82F29C-4E69-4D21-A2A1-636439201457}</MetaDataID>
        public void InDoubt(System.Transactions.Enlistment enlistment)
        {
            foreach (ITransactionContextExtender extender in _Extenders)
                extender.Abort();

            System.Collections.Generic.List<ITransactionNotification> transactionNotificationObjects = new System.Collections.Generic.List<ITransactionNotification>();
            ReleaseEnlistObjects(true, transactionNotificationObjects);
            enlistment.Done();

        }

        /// <MetaDataID>{8B662CBD-824F-4D5F-A725-96A05B04FD0F}</MetaDataID>
        public void Prepare(System.Transactions.PreparingEnlistment preparingEnlistment)
        {
            if (Status == ObjectsStateManagerStatus.OnAction || Status == ObjectsStateManagerStatus.PrepareRequest)
                preparingEnlistment.ForceRollback(new System.Exception("OOAdvantech TransactionContext is in inconsistent state"));
            else
                preparingEnlistment.Prepared();

        }


        public bool HasChangesOnElistedObjects(bool checkOnlyPersistentClassInstances)
        {

            {
                System.Collections.Generic.List<object> enlistObjects = null;
                lock (this)
                {
                    enlistObjects = new System.Collections.Generic.List<object>(EnlistObjects);
                }
                foreach (object _object in enlistObjects)
                {
                    LockedObjectEntry transactionContextLockedObjectEntry = null;
                    lock (LockedObjectEntry.LockedObjects)
                    {
                        foreach (LockedObjectEntry lockedObjectEntry in LockedObjectEntry.LockedObjects[_object].Values)
                        {
                            if (lockedObjectEntry.TransactionContext.Transaction == Transaction || (Transaction.OriginTransaction != null && lockedObjectEntry.TransactionContext.Transaction.OriginTransaction == Transaction.OriginTransaction))
                            {
                                transactionContextLockedObjectEntry = lockedObjectEntry;
                                break;
                            }
                        }
                    }
                    if (transactionContextLockedObjectEntry.HasGhanged(this, checkOnlyPersistentClassInstances))
                        return true;
                }
                return false;
            }
        }

        /// <MetaDataID>{3F8AF49E-67F2-4742-93F5-1DF40EE979D2}</MetaDataID>
        public void Rollback(System.Transactions.Enlistment enlistment)
        {
            try
            {
                ObjectsStateManagerStatus status = Status;

                if (status == ObjectsStateManagerStatus.AbortRequest ||
                    status == ObjectsStateManagerStatus.CommitRequest ||
                    status == ObjectsStateManagerStatus.CommitDone ||
                    status == ObjectsStateManagerStatus.AbortDone)
                    return;
                try
                {
                    UpdateStatus(ObjectsStateManagerStatus.AbortRequest);
                    foreach (ITransactionContextExtender extender in _Extenders)
                        extender.Abort();

                }
                catch (System.Exception AbortError)
                {
                }
                System.Collections.Generic.List<ITransactionNotification> transactionNotificationObjects = new System.Collections.Generic.List<ITransactionNotification>();
                ReleaseEnlistObjects(true, transactionNotificationObjects);
                UpdateStatus(ObjectsStateManagerStatus.AbortDone);

                foreach (ITransactionNotification transactionNotificationObject in transactionNotificationObjects)
                {
                    try
                    {
                        transactionNotificationObject.OnTransactionCompletted(Transaction);
                    }
                    catch (System.Exception error)
                    {
                    }
                }
                //                TransactionCompletedEvent.Set();
                _Extenders = null;
            }
            finally
            {
                enlistment.Done();
            }
        }

        #endregion
#endif


        /// <MetaDataID>{5895291B-1F5D-4690-97BC-176B53358E1B}</MetaDataID>
        void ReleaseEnlistObjects(bool Rollback, System.Collections.Generic.List<ITransactionNotification> transactionNotificationObjects)
        {

            System.Collections.Generic.List<object> enlistObjects = null;
            lock (this)
            {
                enlistObjects = new System.Collections.Generic.List<object>(EnlistObjects);
                EnlistObjects.Clear();
            }
            foreach (object _object in enlistObjects)
            {
                LockedObjectEntry transactionContextLockedObjectEntry = null;
                lock (LockedObjectEntry.LockedObjects)
                {
                    foreach (LockedObjectEntry lockedObjectEntry in LockedObjectEntry.LockedObjects[_object].Values)
                    {
                        if (lockedObjectEntry.TransactionContext.Transaction == Transaction || (Transaction.OriginTransaction != null && lockedObjectEntry.TransactionContext.Transaction.OriginTransaction == Transaction.OriginTransaction))
                        {
                            transactionContextLockedObjectEntry = lockedObjectEntry;
                            break;
                        }
                    }
                }


                if (Rollback)
                    transactionContextLockedObjectEntry.RollBack(this, transactionNotificationObjects);
                else
                    transactionContextLockedObjectEntry.Commit(this, transactionNotificationObjects);


                lock (LockedObjectEntry.LockedObjects)
                {
                    if (transactionContextLockedObjectEntry.ObjectStateSnapshots.Count == 0)// && lockedObjectEntry.ObjectStateSnapshots.ContainsKey(this))
                    {
                        Transaction originTransaction = transactionContextLockedObjectEntry.TransactionContext.Transaction;
                        while (originTransaction.OriginTransaction != null)
                            originTransaction = originTransaction.OriginTransaction;


                        LockedObjectEntry.LockedObjects[transactionContextLockedObjectEntry.LockedObject].Remove(originTransaction.LocalTransactionUri);
                        if (LockedObjectEntry.LockedObjects[transactionContextLockedObjectEntry.LockedObject].Count == 0)
                            LockedObjectEntry.LockedObjects.Remove(transactionContextLockedObjectEntry.LockedObject);
                    }
                }
            }

        }
    }

    /// <MetaDataID>{5E94FE62-ED90-41E8-813A-0662EE492A26}</MetaDataID>
    /// <summary>
    /// LockedObjectEntry keeps all information which needed from transaction subsystem for full or partial participation in transaction.
    /// Also manage the transaction locks of object.There is on LockEntry per transaction hierarchy (transaction and nested transactions).
    /// </summary>
    internal class LockedObjectEntry
    {



        /// <MetaDataID>{06919da8-d00c-4df1-b1fa-bd528c919266}</MetaDataID>
        /// <summary>This dictionary keeps the object lockEntries one per transaction hierarchy for eachObject </summary>
        internal static System.Collections.Generic.Dictionary<object, System.Collections.Generic.Dictionary<string, LockedObjectEntry>> LockedObjects = new System.Collections.Generic.Dictionary<object, System.Collections.Generic.Dictionary<string, LockedObjectEntry>>(500);
        /// <MetaDataID>{426c06aa-c6c8-4f2d-b444-6649dcd3fd1d}</MetaDataID>
        /// <summary>Enlist an object to transaction with  exclusive control if permitted from system.
        /// In case where doesn't permitted system raise exception</summary>
        internal static void ExclusiveLock(object lockedObject, TransactionContext transactionContext)
        {
            LockedObjectEntry transactionContextLockObjectEntry = LockTransactionalObject(lockedObject, transactionContext, null, LockOptions.Exclusive);
            transactionContextLockObjectEntry.SnapShotFor(transactionContext);
        }

        /// <MetaDataID>{3d334c23-5224-4e78-b054-6372fe721983}</MetaDataID>
        /// <summary>
        /// Enlist an object to transaction with  shared control if permitted from system.
        /// This means that object can be enlist in two transaction partially 
        /// one number of transaction exclusive members in one transaction 
        /// and some others transaction exclusive members in other transaction.
        /// system can't enlist object partially in two transaction with the same transaction exclusive member
        /// System can enlist object partially in any number of transactions if the members has marked as sharing members 
        /// except the case where the object enlisted in other transaction with exclusive control
        /// </summary>
        internal static void EnlistObject(object lockedObject, TransactionContext transactionContext, System.Reflection.MemberInfo memberInfo)
        {

            try
            {
                TransactionalMemberAttribute transactionalMember = null;
                if (memberInfo != null)
                {
                    transactionalMember = TransactionalMemberAttribute.GetTransactionalMember(memberInfo, lockedObject.GetType());

                }
                LockedObjectEntry transactionContextLockObjectEntry = LockTransactionalObject(lockedObject, transactionContext, transactionalMember, LockOptions.Shared);

                if (memberInfo == null)
                    transactionContextLockObjectEntry.SnapShotFor(transactionContext);
                else
                    transactionContextLockObjectEntry.SnapShotFieldFor(transactionalMember, transactionContext);
            }
            catch (System.Exception error)
            {
                throw;
            }


        }

        /// <summary>
        /// "System calls this method to gain the control of object for the transaction of transaction context
        /// If the object is locked for other transaction system wait for lock timeout.
        /// If the object is locked in other transaction after time out sytem raise exception.
        /// If the system gains control for the transaction of transaction context return a lockObjectEntry. 
        /// </summary>
        /// <param name="lockedObject">
        /// The object where system wants to gain the transaction control
        /// </param>
        /// <param name="transactionContext">
        /// System wants to gain control of object for the transaction of transactionContext
        /// </param>
        /// <param name="transactionalMember">
        /// This parameter defines the member infos for partial transaction enlistment.
        /// Can be null when entire object enlisted in transaction
        /// </param>
        /// <param name="lockOption">
        /// Defines the lock option, if the value is 
        /// </param>
        /// <MetaDataID>{10681ce5-46f5-42ce-b654-d3635a13d57d}</MetaDataID>
        private static LockedObjectEntry LockTransactionalObject(object lockedObject, TransactionContext transactionContext, TransactionalMemberAttribute transactionalMember, LockOptions lockOption)
        {
            Transaction originTransaction = transactionContext.Transaction;
            while (originTransaction.OriginTransaction != null)
                originTransaction = originTransaction.OriginTransaction;
            LockType oldLockType;
            bool newMemberLocked = false;

            Start:

            //Defines the lockObjectEntry for transaction context
            //There is only one lockObjectEntry for transaction and nested transactions of this transaction
            LockedObjectEntry transactionContextLockObjectEntry = null;
            System.Collections.Generic.List<LockedObjectEntry> lockedObjectEntries = null;
            System.Collections.Generic.List<string> garbageLockedObjectEntries = null;
            lock (LockedObjects)
            {
                if (!LockedObjects.ContainsKey(lockedObject))
                {
                    System.Collections.Generic.Dictionary<string, LockedObjectEntry> newLockedObjectEntries = new System.Collections.Generic.Dictionary<string, LockedObjectEntry>();
                    transactionContextLockObjectEntry = new LockedObjectEntry(lockedObject, transactionContext, transactionalMember);

                    newLockedObjectEntries.Add(originTransaction.LocalTransactionUri, transactionContextLockObjectEntry);
                    LockedObjects.Add(lockedObject, newLockedObjectEntries);

                    if (transactionalMember != null)
                    {
                        if (transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive)
                        {
                            if (transactionContextLockObjectEntry.ExclusiveLockMembers == null)
                                transactionContextLockObjectEntry.ExclusiveLockMembers = new System.Collections.Generic.List<TransactionalMemberAttribute>();
                            newMemberLocked = true;
                            transactionContextLockObjectEntry.ExclusiveLockMembers.Add(transactionalMember);
                        }
                        else
                        {
                            if (transactionContextLockObjectEntry.SharedLockMembers == null)
                                transactionContextLockObjectEntry.SharedLockMembers = new System.Collections.Generic.List<TransactionalMemberAttribute>();
                            transactionContextLockObjectEntry.SharedLockMembers.Add(transactionalMember);
                        }
                    }
                }
                else
                {
                    #region Search for lock object entries where system must be wait on the lock object entry transaction to complete
                    foreach (LockedObjectEntry lockedObjectEntry in LockedObjects[lockedObject].Values)
                    {


                        if (lockedObjectEntry.TransactionContext == transactionContext ||
                            lockedObjectEntry.TransactionContext.Transaction.IsNestedTransaction(transactionContext.Transaction))
                        {
                            //There is only one lockObjectEntry for a transaction and its nested transactions 
                            transactionContextLockObjectEntry = lockedObjectEntry;
                        }
                        else if (lockOption == LockOptions.Exclusive && !lockedObjectEntry.TransactionContext.Transaction.WaitToComplete(0))
                        {
                            //if lockOption  is Exclusive lock then system wait all others transaction to complete

                            if (lockedObjectEntries == null)
                                lockedObjectEntries = new System.Collections.Generic.List<LockedObjectEntry>();
                            lockedObjectEntries.Add(lockedObjectEntry);

                        }
                        else if (lockedObjectEntry.LockType != LockType.SharedFieldsParticipation /*!lockedObjectEntry.SharedFields*/ && !lockedObjectEntry.TransactionContext.Transaction.WaitToComplete(0))
                        {
                            if (lockedObjectEntry.LockType == LockType.ExclusiveObjectLock)// (lockedObjectEntry.ExclusiveLocked)
                            {
                                if (lockedObjectEntries == null)
                                    lockedObjectEntries = new System.Collections.Generic.List<LockedObjectEntry>();
                                lockedObjectEntries.Add(lockedObjectEntry);
                            }
                            else if (transactionalMember == null || transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive)
                            {
                                //when new enlistment has full object participation or partial participation with member marked as exclusive lock

                                bool addLockedObjectEntry = false;
                                if (transactionalMember == null)
                                    addLockedObjectEntry = true;       //new enlistment has full object participation 
                                else if (transactionalMember != null &&
                                    transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive &&
                                    lockedObjectEntry.LockType == LockType.ObjectLock)//SnapshotAllFields)
                                    addLockedObjectEntry = true;       //new enlistment has partial participation with member marked as exclusive lock
                                //but the already exist lock entry has full object participation 
                                else if (transactionalMember.Member != null &&
                                    transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive &&
                                    lockedObjectEntry.LockEntryField.ContainsKey(transactionalMember.FieldInfo))
                                    addLockedObjectEntry = true;    //new enlistment has partial participation with member marked as exclusive lock
                                //but the already exist lock entry also has partial participation with the same member. 

                                if (addLockedObjectEntry)
                                {
                                    if (lockedObjectEntries == null)
                                        lockedObjectEntries = new System.Collections.Generic.List<LockedObjectEntry>();
                                    lockedObjectEntries.Add(lockedObjectEntry);
                                }
                            }
                        }
                        else if (lockedObjectEntry.TransactionContext.Transaction.WaitToComplete(0))
                        {
                            Transaction transaction = lockedObjectEntry.TransactionContext.Transaction;
                            while (transaction.OriginTransaction != null)
                            {
                                transaction = transaction.OriginTransaction;
                                if (!transaction.WaitToComplete(0))
                                    break;
                            }
                            if (transaction.WaitToComplete(0))
                            {
                                if (garbageLockedObjectEntries == null)
                                    garbageLockedObjectEntries = new System.Collections.Generic.List<string>();
                                garbageLockedObjectEntries.Add(transaction.LocalTransactionUri);
                            }


                        }
                    }
                    if (garbageLockedObjectEntries != null)
                    {
                        foreach (string localTransactionUri in garbageLockedObjectEntries)
                            LockedObjects[lockedObject].Remove(localTransactionUri);
                        if (LockedObjects[lockedObject].Count == 0)
                            LockedObjects.Remove(lockedObject);
                    }
                    #endregion
                }


                if (lockedObjectEntries != null && lockedObjectEntries.Count > 0)
                    goto WaitForTransactionComplete;

                if (transactionContextLockObjectEntry == null)
                {
                    #region System creates a new locked object entry



                    if (!LockedObjects.ContainsKey(lockedObject))
                    {
                        System.Collections.Generic.Dictionary<string, LockedObjectEntry> newLockedObjectEntries = new System.Collections.Generic.Dictionary<string, LockedObjectEntry>();
                        transactionContextLockObjectEntry = new LockedObjectEntry(lockedObject, transactionContext, transactionalMember);
                        newLockedObjectEntries.Add(originTransaction.LocalTransactionUri, transactionContextLockObjectEntry);
                        LockedObjects.Add(lockedObject, newLockedObjectEntries);
                    }
                    else
                    {
                        //There is only one lockObjectEntry for transaction and nested transactions of this transaction
                        if (!LockedObjects[lockedObject].TryGetValue(originTransaction.LocalTransactionUri, out transactionContextLockObjectEntry))
                        {
                            transactionContextLockObjectEntry = new LockedObjectEntry(lockedObject, transactionContext, transactionalMember);
                            LockedObjects[lockedObject].Add(originTransaction.LocalTransactionUri, transactionContextLockObjectEntry);
                            if (transactionalMember != null)
                            {
                                if (transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive)
                                {
                                    if (transactionContextLockObjectEntry.ExclusiveLockMembers == null)
                                        transactionContextLockObjectEntry.ExclusiveLockMembers = new System.Collections.Generic.List<TransactionalMemberAttribute>();
                                    newMemberLocked = true;
                                    transactionContextLockObjectEntry.ExclusiveLockMembers.Add(transactionalMember);
                                }
                                else
                                {
                                    if (transactionContextLockObjectEntry.SharedLockMembers == null)
                                        transactionContextLockObjectEntry.SharedLockMembers = new System.Collections.Generic.List<TransactionalMemberAttribute>();
                                    transactionContextLockObjectEntry.SharedLockMembers.Add(transactionalMember);
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                    #endregion
                }
                oldLockType = transactionContextLockObjectEntry.LockType;
                #region Sets lock entry status
                if (lockOption == LockOptions.Exclusive)
                {
                    transactionContextLockObjectEntry.LockType = LockType.ExclusiveObjectLock;
                    //transactionContextLockObjectEntry.SnapshotAllFields = true;
                    //transactionContextLockObjectEntry.SharedFields = false;
                    //transactionContextLockObjectEntry.ExclusiveLocked = true;
                }
                else if (transactionalMember == null && transactionContextLockObjectEntry.LockType != LockType.ExclusiveObjectLock)
                {
                    transactionContextLockObjectEntry.LockType = LockType.ObjectLock;
                    //transactionContextLockObjectEntry.SnapshotAllFields = true;
                    //transactionContextLockObjectEntry.SharedFields = false;
                }
                else if (transactionalMember != null &&
                    transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive &&
                    transactionContextLockObjectEntry.LockType == LockType.SharedFieldsParticipation)
                {
                    transactionContextLockObjectEntry.LockType = LockType.PartialObjectLock;
                    //transactionContextLockObjectEntry.SharedFields = false;
                }
                #endregion


                if (transactionalMember != null &&
                    (transactionContextLockObjectEntry.LockEntryField == null || !transactionContextLockObjectEntry.LockEntryField.ContainsKey(transactionalMember.FieldInfo)))
                {
                    if (transactionContextLockObjectEntry.LockEntryField == null)
                        transactionContextLockObjectEntry.LockEntryField = new System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, LockOptions>();
                    transactionContextLockObjectEntry.LockEntryField.Add(transactionalMember.FieldInfo, transactionalMember.MemberTransactionLockOption);
                    if (transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive)
                    {
                        if (transactionContextLockObjectEntry.ExclusiveLockMembers == null)
                            transactionContextLockObjectEntry.ExclusiveLockMembers = new System.Collections.Generic.List<TransactionalMemberAttribute>();
                        newMemberLocked = true;
                        transactionContextLockObjectEntry.ExclusiveLockMembers.Add(transactionalMember);
                    }
                    else
                    {
                        if (transactionContextLockObjectEntry.SharedLockMembers == null)
                            transactionContextLockObjectEntry.SharedLockMembers = new System.Collections.Generic.List<TransactionalMemberAttribute>();
                        transactionContextLockObjectEntry.SharedLockMembers.Add(transactionalMember);
                    }
                }

            }

            #region Raise event to transaction lock subscribers that the object lock state has changed
            try
            {
                if (!transactionContextLockObjectEntry.ObjectStateSnapshots.ContainsKey(transactionContext))
                {
                    // at the first time where object enlisted in transaction context
                    if (transactionContextLockObjectEntry.LockType != LockType.SharedFieldsParticipation)
                    {
                        if (transactionContextLockObjectEntry.LockType == LockType.PartialObjectLock)
                        {

                            System.Reflection.MemberInfo[] membersInfos = new System.Reflection.MemberInfo[transactionContextLockObjectEntry.ExclusiveLockMembers.Count];
                            int i = 0;
                            foreach (TransactionalMemberAttribute memberMetaData in transactionContextLockObjectEntry.ExclusiveLockMembers)
                                membersInfos[i++] = memberMetaData.Member;

                            TransactionContext.ObjectIsLocked(lockedObject, membersInfos, transactionContext.Transaction);
                        }
                        else
                        {
                            TransactionContext.ObjectIsLocked(lockedObject, null, transactionContext.Transaction);
                        }
                    }
                }
                else
                {
                    if (oldLockType == transactionContextLockObjectEntry.LockType && oldLockType == LockType.PartialObjectLock && newMemberLocked)
                    {
                        //the object already enlisted partially in transaction context but there is one more member which locked.
                        System.Reflection.MemberInfo[] membersInfos = new System.Reflection.MemberInfo[1] { transactionalMember.Member };
                        TransactionContext.ObjectIsLocked(lockedObject, membersInfos, transactionContext.Transaction);
                    }
                    else if (oldLockType != transactionContextLockObjectEntry.LockType)
                    {
                        if ((oldLockType == LockType.PartialObjectLock || oldLockType == LockType.SharedFieldsParticipation) && (transactionContextLockObjectEntry.LockType == LockType.ObjectLock || transactionContextLockObjectEntry.LockType == LockType.ExclusiveObjectLock))
                        {
                            //The lock state of object has changed from partial enlistement to full object enlistment 
                            TransactionContext.ObjectIsLocked(lockedObject, null, transactionContext.Transaction);
                        }

                        if (oldLockType == LockType.SharedFieldsParticipation && oldLockType == LockType.PartialObjectLock)
                        {
                            //The lock state of object has changed from partial enlistement with trsnsactions shared field to partial enlistement with trsnsaction exclusive fields  

                            System.Reflection.MemberInfo[] membersInfos = new System.Reflection.MemberInfo[1] { transactionalMember.Member };
                            TransactionContext.ObjectIsLocked(lockedObject, membersInfos, transactionContext.Transaction);
                        }
                    }
                }
            }
            catch (System.Exception error)
            {

            }
            #endregion

            return transactionContextLockObjectEntry;
            WaitForTransactionComplete:
            foreach (LockedObjectEntry lockedObjectEntry in lockedObjectEntries)
            {
                if (!lockedObjectEntry.TransactionContext.Transaction.WaitToComplete((int)5))
                {
                    //TODO ο τελικός χρόνος που θα κάνει μπορεί να είνα πάνω από 30 δευτερόλεπτα
                    if (!lockedObjectEntry.TransactionContext.Transaction.WaitToComplete((int)LogicalThread.ObjectEnlistmentTimeOut.TotalMilliseconds))
                    {
                        throw new OOAdvantech.Transactions.TransactionException("Time out expired. Object is locked from other transaction");
                    }
                }
            }
            goto Start;
        }
        /// <MetaDataID>{d430aa8c-921c-44f9-9b12-92891ebdb233}</MetaDataID>
        internal System.Collections.Generic.List<System.Reflection.FieldInfo> PartialObjectLockFields
        {
            get
            {
                System.Collections.Generic.List<System.Reflection.FieldInfo> partialObjectLockFields = new System.Collections.Generic.List<System.Reflection.FieldInfo>();
                if (SharedLockMembers != null)
                {
                    foreach (TransactionalMemberAttribute transactionalMember in SharedLockMembers)
                        partialObjectLockFields.Add(transactionalMember.FieldInfo);
                }
                if (ExclusiveLockMembers != null)
                {
                    foreach (TransactionalMemberAttribute transactionalMember in ExclusiveLockMembers)
                        partialObjectLockFields.Add(transactionalMember.FieldInfo);
                }
                return partialObjectLockFields;
            }
        }

        /// <MetaDataID>{ae2d5760-23c4-4d5d-a87a-97f68922c707}</MetaDataID>
        internal System.Collections.Generic.List<TransactionalMemberAttribute> ExclusiveLockMembers;
        /// <MetaDataID>{addfe3f8-3548-4447-a22e-c0d13802609c}</MetaDataID>
        internal System.Collections.Generic.List<TransactionalMemberAttribute> SharedLockMembers;

        /// <summary>
        /// Contains fieldInfos and lock option of fields in partial participation
        /// </summary>
        /// <MetaDataID>{15108b9a-a0d2-4d3d-a20d-31fd048464a1}</MetaDataID>
        System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, LockOptions> LockEntryField;

        /// <MetaDataID>{8d5f5c27-203d-4ce9-9149-520272fc651f}</MetaDataID>
        public LockType LockType;
        /////<summary>
        /////The SnapshotAllFields is true when the whole object participate in transaction not partially.
        /////The object isn’t locked when enlisted in other transaction partially with shared fields
        /////</summary>
        ///// <MetaDataID>{0a391b37-0b07-4715-b0fb-3f8970b35586}</MetaDataID>
        //public bool SnapshotAllFields = false;

        /////<summary>
        /////The SharedFields is true when the object participated in transaction partially only with shared fields. 
        /////The object is locked only when enlisted in other transaction exclusively.  
        /////</summary>
        ///// <MetaDataID>{7d3982b4-1690-4551-9e15-32d896effb45}</MetaDataID>
        //public bool SharedFields = true;
        /////<summary>
        /////The ExclusiveLocked is true when the whole object participate in transaction not partially exclusively. 
        /////The object is locked for any type of enlistment in other transaction.  
        /////</summary>
        ///// <MetaDataID>{6cce7d34-c013-4723-b459-124665cf5c49}</MetaDataID>
        //public bool ExclusiveLocked = false;
        ///<summary>
        ///LockedObjectEntry constructor
        ///</summary>
        ///<param name="lockedObject">
        ///This parameter defines the transactional object.
        ///</param>
        ///<param name="transactionContext">
        ///This parameter defines the transaction context where lock entry belongs.     
        ///</param>
        ///<param name="transactionalMember">
        ///This parameter defines the member infos for partial transaction enlistment.
        ///Can be null when entire object enlisted in transaction
        ///</param>
        /// <MetaDataID>{70BE9366-24DD-40C1-AB8C-E0CFE0978950}</MetaDataID>
        public LockedObjectEntry(object lockedObject, TransactionContext transactionContext, TransactionalMemberAttribute transactionalMember)
        {

            ThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

#if !DeviceDotNet
            NativeThreadID = AppDomain.GetCurrentThreadId();
#endif

            LockedObject = lockedObject;
            LockType = LockType.SharedFieldsParticipation;
            if (transactionalMember == null)
            {
                LockType = LockType.ObjectLock;
                //SnapshotAllFields = true;
                //SharedFields = false;
            }
            else
            {
                //TransactionalMemberAttribute transactionalMember = memberInfo.GetCustomAttributes(typeof(TransactionalMemberAttribute), true)[0] as TransactionalMemberAttribute;
                // transactionalMember.InitFor(memberInfo.DeclaringType, memberInfo);
                LockEntryField = new System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, LockOptions>();
                LockEntryField[transactionalMember.FieldInfo] = transactionalMember.MemberTransactionLockOption;
                if (transactionalMember.MemberTransactionLockOption != LockOptions.Shared)
                {
                    LockType = LockType.PartialObjectLock;
                }

                //                    SharedFields = false;

            }
            TransactionContext = transactionContext;

        }

        int ThreadID;

        public int NativeThreadID { get; } = 0;

        ///<summary>
        ///This filed defines the object which lock entry refered.
        ///</summary>
        /// <MetaDataID>{F62DA12A-0B0B-4920-9A91-03146A205F16}</MetaDataID>
        public readonly object LockedObject;

        ///<summary>
        ///This field defines the transaction context where lock entry belongs.     
        ///</summary>
        /// <MetaDataID>{43D27EB1-098A-4EE2-80E9-23ED6AEA42DA}</MetaDataID>
        public TransactionContext TransactionContext;
        /// <MetaDataID>{CA6FBE07-B8F1-45C1-A833-0BF34C3168F0}</MetaDataID>
        public OOAdvantech.Collections.Generic.Dictionary<TransactionContext, ObjectStateSnapshot> ObjectStateSnapshots = new OOAdvantech.Collections.Generic.Dictionary<TransactionContext, ObjectStateSnapshot>();

#region Takes Snapshots
        ///<summary>
        ///Takes a snapshot of object state for transaction context 
        ///</summary>
        ///<param name="transactionContext">
        ///Defines the transaction context.
        ///</param>
        /// <MetaDataID>{e3ee3a5b-0ed8-4876-9421-3bde45e340e0}</MetaDataID>
        internal void SnapShotFor(TransactionContext transactionContext)
        {
            ObjectStateSnapshot objectStateSnapshot = null;
            lock (this)
            {
                ObjectStateSnapshots.TryGetValue(transactionContext, out objectStateSnapshot);
            }
            if (objectStateSnapshot != null)
            {
                if (transactionContext != objectStateSnapshot.TransactionContext)
                    throw new OOAdvantech.Transactions.TransactionException("TransactionContext error");
                objectStateSnapshot.SnapshotMemoryInstance();
                return;
            }
            lock (this)
            {
                objectStateSnapshot = new ObjectStateSnapshot(LockedObject, transactionContext);
                ObjectStateSnapshots.Add(transactionContext, objectStateSnapshot);
                TransactionContext = transactionContext;
                transactionContext.EnlistObjects.Add(LockedObject);
            }
            TransactionContext = transactionContext;
            if (transactionContext != objectStateSnapshot.TransactionContext)
                throw new OOAdvantech.Transactions.TransactionException("TransactionContext error");
            objectStateSnapshot.SnapshotMemoryInstance();
            try
            {
                foreach (ITransactionContextExtender extender in transactionContext.Extenders)
                    extender.EnlistObject(LockedObject, null);
            }
            catch (System.Exception error)
            {
            }
            //try
            //{
            //    //Raise event is locked from transaction of transaction context
            //    //TransactionContext.ObjectIsLocked(LockedObject, null, transactionContext.Transaction);
            //}
            //catch (System.Exception error)
            //{
            //}


        }



        /// <summary>
        /// Takes a snapshot for field if there isn't. Used in partial enlistment. 
        /// </summary>
        /// <param name="fieldInfo"> 
        /// This parameter contains the field which system will take the snapshot 
        /// </param>
        /// <param name="transactionContext">
        /// This parameter defines the transaction context where the object enlisted partialy
        /// </param>
        /// <MetaDataID>{94e9efb6-3f53-44e7-97af-ded13b3807dc}</MetaDataID>
        internal void SnapShotFieldFor(TransactionalMemberAttribute transactionalMember, TransactionContext transactionContext)
        {

            ObjectStateSnapshot objectStateSnapshot = null;
            lock (this)
            {
                ObjectStateSnapshots.TryGetValue(transactionContext, out objectStateSnapshot);
            }
            if (objectStateSnapshot != null)
            {
                if (transactionContext != objectStateSnapshot.TransactionContext)
                    throw new OOAdvantech.Transactions.TransactionException("TransactionContext error");

                objectStateSnapshot.SnapshotMemoryInstance(transactionalMember.FieldMetadata);
                return;
            }
            lock (this)
            {
                objectStateSnapshot = new ObjectStateSnapshot(LockedObject, transactionContext);
                ObjectStateSnapshots.Add(transactionContext, objectStateSnapshot);
            }
            if (transactionContext != objectStateSnapshot.TransactionContext)
                throw new OOAdvantech.Transactions.TransactionException("TransactionContext error");
            objectStateSnapshot.SnapshotMemoryInstance(transactionalMember.FieldMetadata);
            bool justEnlisted = false;
            lock (this)
            {
                TransactionContext = transactionContext;
                if (!transactionContext.EnlistObjects.Contains(LockedObject))
                {
                    transactionContext.EnlistObjects.Add(LockedObject);
                    justEnlisted = true;
                }
            }
            if (justEnlisted)
            {
                try
                {
                    foreach (ITransactionContextExtender extender in transactionContext.Extenders)
                        extender.EnlistObject(LockedObject, null);
                }
                catch (System.Exception error)
                {
                }

                //try
                //{
                //    //TransactionContext.ObjectIsLocked(LockedObject, fieldInfo, transactionContext.Transaction);
                //}
                //catch (System.Exception error)
                //{


                //}
            }


        }


        /// <summary>
        /// Check for existence of snapshot for original transaction. 
        /// If there isn’t create one and transfer the origin values 
        /// of current transaction snapshot to the new snapshot. 
        /// If the transaction of transaction context isn’t nested the original transaction is null 
        /// and method raise inconsistent state exception.  
        /// </summary>
        /// <MetaDataID>{bf2cfa38-d2e7-4768-bc13-a02ab0421e65}</MetaDataID>
        internal void EnsureSnapShotForOriginTransaction(TransactionContext transactionContext)
        {
            if (transactionContext.Transaction.OriginTransaction == null)
                throw new System.SystemException("Used only for nested transactions");
            TransactionContext originTransactionContext = (transactionContext.Transaction.OriginTransaction as TransactionRunTime).TransactionContext;

            ObjectStateSnapshot objectStateSnapshot = null;
            lock (this)
            {
                ObjectStateSnapshots.TryGetValue(originTransactionContext, out objectStateSnapshot);
            }

            if (objectStateSnapshot == null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(originTransactionContext.Transaction))
                {
                    lock (this)
                    {
                        objectStateSnapshot = new ObjectStateSnapshot(ObjectStateSnapshots[transactionContext], originTransactionContext);
                        ObjectStateSnapshots.Add(originTransactionContext, objectStateSnapshot);
                        originTransactionContext.EnlistObjects.Add(LockedObject);
                    }
                    try
                    {
                        foreach (ITransactionContextExtender extender in originTransactionContext.Extenders)
                            extender.EnlistObject(LockedObject, null);
                    }
                    catch (System.Exception error)
                    {
                    }
                    try
                    {
                        TransactionContext.ObjectIsLocked(LockedObject, null, originTransactionContext.Transaction);
                    }
                    catch (System.Exception error)
                    {
                    }
                    stateTransition.Consistent = true;
                }
            }
        }

#endregion

        /// <MetaDataID>{fc5d6cbf-4b38-4786-a19f-9f0d3a91a74f}</MetaDataID>
        internal static bool IsLocked(object lockedObject, System.Reflection.MemberInfo memberInfo)
        {

            TransactionalMemberAttribute transactionalMember = null;
            if (memberInfo != null)
                transactionalMember = TransactionalMemberAttribute.GetTransactionalMember(memberInfo, lockedObject.GetType());

            lock (LockedObjects)
            {


                if (!LockedObjects.ContainsKey(lockedObject))
                {
                    return false;
                }
                else
                {

                    foreach (LockedObjectEntry lockedObjectEntry in LockedObjects[lockedObject].Values)
                    {
                        if (Transactions.Transaction.Current == null)
                        {
                            if (lockedObjectEntry.LockType == LockType.ExclusiveObjectLock)// lockedObjectEntry.ExclusiveLocked)
                                return true;

                            if (lockedObjectEntry.LockType == LockType.SharedFieldsParticipation)
                                continue;

                            //the lock entry is not for shared fields
                            if (transactionalMember == null || transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive)
                            {
#region Search for lock object entries where system must be wait on the lock object entry transactions to complete
                                if (transactionalMember == null && lockedObjectEntry.LockType != LockType.SharedFieldsParticipation)
                                    return true;
                                else if (lockedObjectEntry.LockType == LockType.ObjectLock &&  //(lockedObjectEntry.SnapshotAllFields &&
                                    transactionalMember != null &&
                                    transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive)
                                    return true;
                                else if (lockedObjectEntry.LockType == LockType.PartialObjectLock && //(!lockedObjectEntry.SnapshotAllFields &&
                                        transactionalMember != null &&
                                        transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive &&
                                        lockedObjectEntry.LockEntryField.ContainsKey(transactionalMember.FieldInfo))
                                    return true;

#endregion
                            }

                        }
                        else if (lockedObjectEntry.TransactionContext.Transaction != Transactions.Transaction.Current &&
                            !lockedObjectEntry.TransactionContext.Transaction.IsNestedTransaction(Transactions.Transaction.Current as TransactionRunTime)
                            && !lockedObjectEntry.TransactionContext.Transaction.WaitToComplete(0))
                        {
                            //must be not object enlistment in nested transaction
                            //and lockObjectTransaction transaction must be not completed

                            if (lockedObjectEntry.LockType == LockType.ExclusiveObjectLock)// lockedObjectEntry.ExclusiveLocked)
                                return true;

                            if (lockedObjectEntry.LockType == LockType.SharedFieldsParticipation)
                                continue;

                            //the lock entry is not for shared fields
                            if (transactionalMember == null || transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive)
                            {
#region Search for lock object entries where system must be wait on the lock object entry transactions to complete
                                if (transactionalMember == null && lockedObjectEntry.LockType != LockType.SharedFieldsParticipation)
                                    return true;
                                else if (lockedObjectEntry.LockType == LockType.ObjectLock &&  //(lockedObjectEntry.SnapshotAllFields &&
                                    transactionalMember != null &&
                                    transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive)
                                    return true;
                                else if (lockedObjectEntry.LockType == LockType.PartialObjectLock && //(!lockedObjectEntry.SnapshotAllFields &&
                                        transactionalMember != null &&
                                        transactionalMember.MemberTransactionLockOption == LockOptions.Exclusive &&
                                        lockedObjectEntry.LockEntryField.ContainsKey(transactionalMember.FieldInfo))
                                    return true;
#endregion
                            }

                        }
                    }
                    return false;

                }
            }
        }
        /// <MetaDataID>{5c0dca9e-9a6d-4133-b1c2-35f7c96ab8a9}</MetaDataID>
        object SynchroObject = new object();

        ///<summary>
        ///This method manipulate the object state through object snapshots when transaction of transaction context aborted
        ///</summary> 
        ///<param name="transactionContext">
        ///Defines the transaction context of transaction which aborted
        ///</param>
        /// <MetaDataID>{d1bfe9a9-8aee-45b9-9658-b60240f9b557}</MetaDataID>
        internal void RollBack(TransactionContext transactionContext, System.Collections.Generic.List<ITransactionNotification> transactionNotificationObjects)
        {
            lock (SynchroObject)
            {
                ObjectStateSnapshot objectStateSnapshot = null;
                if (transactionContext.Transaction.OriginTransaction != null)
                {
                    if (ObjectStateSnapshots.Count > 1)
                        EnsureSnapShotForOriginTransaction(transactionContext);
                }

                lock (this)
                {
                    if (ObjectStateSnapshots.TryGetValue(transactionContext, out objectStateSnapshot))
                        ObjectStateSnapshots.Remove(transactionContext);
                    if (transactionContext.Transaction.OriginTransaction != null)
                        if (ObjectStateSnapshots.Count != 0)
                            TransactionContext = (transactionContext.Transaction.OriginTransaction as TransactionRunTime).TransactionContext;
                }

                if (objectStateSnapshot != null)
                {
                    if (transactionContext != objectStateSnapshot.TransactionContext)
                        throw new OOAdvantech.Transactions.TransactionException("TransactionContext error");

                    objectStateSnapshot.Rollback();

                    if (objectStateSnapshot.MemoryInstance is ITransactionNotification)
                        transactionNotificationObjects.Add(objectStateSnapshot.MemoryInstance as ITransactionNotification);

                }
            }
        }

        internal bool HasGhanged(TransactionContext transactionContext, bool checkOnlyPersistentClassInstances)
        {
            lock (SynchroObject)
            {
                ObjectStateSnapshot objectStateSnapshot = null;
                //if (transactionContext.Transaction.OriginTransaction != null) persistent 
                //{
                //    if (ObjectStateSnapshots.Count > 1)
                //        EnsureSnapShotForOriginTransaction(transactionContext);
                //}

                lock (this)
                {
                    if (!ObjectStateSnapshots.TryGetValue(transactionContext, out objectStateSnapshot))
                        return false;
                }

                if (objectStateSnapshot != null)
                {
                    if (transactionContext != objectStateSnapshot.TransactionContext)
                        throw new OOAdvantech.Transactions.TransactionException("TransactionContext error");
                    if (checkOnlyPersistentClassInstances)
                    {
                        var _class = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectStateSnapshot.MemoryInstance.GetType()) as OOAdvantech.MetaDataRepository.Class;
                        if (_class != null && _class.Persistent)
                            return objectStateSnapshot.ObjectHasGhanged();
                        else
                            return false;

                    }
                    else
                        return objectStateSnapshot.ObjectHasGhanged();
                }
                else
                    return false;
            }

        }

        ///<summary>
        ///This method manipulate the object state through object snapshots when transaction of transaction context committed
        ///</summary> 
        ///<param name="transactionContext">
        ///Defines the transaction context of transaction which committed
        ///</param>
        /// <MetaDataID>{849468b5-af86-4edb-ac7d-eb933b967cd7}</MetaDataID>
        internal void Commit(TransactionContext transactionContext, System.Collections.Generic.List<ITransactionNotification> transactionNotificationObjects)
        {
            lock (SynchroObject)
            {
                if (transactionContext.Transaction.OriginTransaction != null)
                {
                    TransactionContext = (transactionContext.Transaction.OriginTransaction as TransactionRunTime).TransactionContext;
                    EnsureSnapShotForOriginTransaction(transactionContext);
                    ObjectStateSnapshots[(transactionContext.Transaction.OriginTransaction as TransactionRunTime).TransactionContext].Merge(transactionContext.Transaction.OriginTransaction, transactionContext.Transaction);
                }

                lock (this)
                {
                    if (transactionContext.Transaction.OriginTransaction == null)
                    {
                        ObjectStateSnapshot objectStateSnapshot = ObjectStateSnapshots[transactionContext];
                        if (transactionContext != objectStateSnapshot.TransactionContext)
                            throw new OOAdvantech.Transactions.TransactionException("TransactionContext error");

                        objectStateSnapshot.Commit();

                        if (objectStateSnapshot.MemoryInstance is ITransactionNotification)
                            transactionNotificationObjects.Add(objectStateSnapshot.MemoryInstance as ITransactionNotification);
                    }
                    ObjectStateSnapshots.Remove(transactionContext);

                }
            }

        }
    }

    /// <MetaDataID>{5413dcf5-4bb0-46df-90ea-7ae2a902a25c}</MetaDataID>
    public enum LockType
    {
        /// <summary>
        /// The object participated in transaction partially only with shared fields. 
        /// The object is locked only when enlisted in other transaction exclusively.  
        /// </summary>
        SharedFieldsParticipation,
        /// <summary>
        /// Whole object participate in transaction exclusively. 
        /// The object is locked for any type of enlistment in other transaction.  
        /// </summary>
        ExclusiveObjectLock,
        /// <summary>
        /// Whole object participate in transaction.
        /// The object doesn't locked when enlisted in other transaction partially with shared fields
        /// </summary>
        ObjectLock,
        /// <summary>
        /// The object participated in transaction partially with exclusive transaction fields. 
        /// The object doesn't locked only when enlisted in other transaction partially with shared fields 
        /// or with exclusive transaction fields other than exclusive transaction fields of this transaction.  
        /// </summary>
        PartialObjectLock,

        Unlocked
    }
}
