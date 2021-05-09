namespace OOAdvantech.Transactions
{

    using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
    using LockCookie = OOAdvantech.Synchronization.LockCookie;
    using Remoting;
    using System;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
    using MarshalByRefObject = System.MarshalByRefObject;
#endif    

    /// <MetaDataID>{F13748B7-A955-42F2-BDB9-F7C2AA0BAFAD}</MetaDataID>
    internal class TransactionEnlistment : MarshalByRefObject, ITransactionEnlistment, Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{F48C3324-A7DE-4802-90C8-1B9FA5B60067}</MetaDataID>
        ~TransactionEnlistment()
        {
            int hh = 0;
        }
        /// <MetaDataID>{fb473e61-111f-4e4c-b93c-2457f0b9cd79}</MetaDataID>
        System.DateTime LastCommunication = System.DateTime.Now;
        /// <MetaDataID>{586F14DF-9835-43EA-BD58-B42D18C61390}</MetaDataID>
        internal bool CommunicationActive
        {
            get
            {
#if !DeviceDotNet
                try
                {

                    if (Remoting.RemotingServices.IsOutOfProcess(EnlistedObjectsStateManager as System.MarshalByRefObject))
                    {
                        OOAdvantech.Remoting.RemotingServices.SetLeaseTime(EnlistedObjectsStateManager as System.MarshalByRefObject, System.TimeSpan.FromSeconds(20));
                        //System.Runtime.Remoting.Lifetime.ILease lease = (Remoting.RemotingServices.GetOrgObject(EnlistedObjectsStateManager as System.MarshalByRefObject) as System.MarshalByRefObject).GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                        //lease.Renew(System.TimeSpan.FromSeconds(20));
                        LastCommunication = System.DateTime.Now;
                        return true;
                    }
                    else
                    {
                        LastCommunication = System.DateTime.Now;
                        return true;
                    }
                }
                catch (System.Exception error)
                {
                    //InformEventLog(new System.Exception("Communication error ", error));
                    Microsoft.Win32.RegistryKey transactionCoordinatorKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"\SOFTWARE\HandySoft\TransactionCoordinator");
                    if (transactionCoordinatorKey != null)
                    {
                        object _value = transactionCoordinatorKey.GetValue("Debug");
                        if (_value != null && ((int)_value) == 1)
                            return true;
                    }
                    if (((System.TimeSpan)(LastCommunication - System.DateTime.Now)).TotalSeconds > 120)
                    {
                        InformEventLog(new System.Exception("Communication error ", error));
                        return false;
                    }
                    else
                        return true;
                }
#else
            return true;
#endif
            }

        }
        /// <MetaDataID>{C47F68A6-E8FA-4A71-BAAC-0D06EE5AF482}</MetaDataID>
        private static void InformEventLog(System.Exception Error)
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
            //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
            myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
            System.Exception InerError = Error.InnerException;
            while (InerError != null)
            {
                System.Diagnostics.Debug.WriteLine(
                    InerError.Message + InerError.StackTrace);
                myLog.WriteEntry(InerError.Message + InerError.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                InerError = InerError.InnerException;
            }
#endif
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{F1DA4E20-74C5-4D80-B104-D44F0B3B49E7}</MetaDataID>
        private ObjectsStateManagerStatus _Status;
        /// <MetaDataID>{0465FDDD-2829-4AEE-AB5C-82F6DD09F8F9}</MetaDataID>
        /// <summary>Define the state of transaction context of objects state manager.</summary>
        public ObjectsStateManagerStatus Status
        {
            get
            {
                lock (this)
                {
                    return _Status;
                }
            }
        }

        

        /// <MetaDataID>{D561C642-1B94-431C-A7A8-EA408C3665AF}</MetaDataID>
        /// <summary>Define the identity of transaction.</summary>
        public string TransactionUri;
        /// <summary>Propagate asynchronously the method call to objects state manager that enlisted in transaction. (Transaction manager inform that the transaction committed and ask from objects state manager to release objectss from transaction lock. This is phase two of the two-phase commit protocol.)</summary>
        /// <MetaDataID>{920DB1CF-74CC-4452-B814-2FEE471F235E}</MetaDataID>
        public void CommitRequest()
        {
            bool inPrepareDone = false;
            lock (this)
            {
                if (_Status == ObjectsStateManagerStatus.PrepareDone)
                {
                    _Status = ObjectsStateManagerStatus.CommitRequest;
                    inPrepareDone = true;
                }
            }
            if (inPrepareDone)
            {
#if !DeviceDotNet
                //Εάν ο object state manager είναι out of process τότε λόγο του [OneWay] attribute η method call
                //θα είναι ασύγχρονη αλλιώς σύγχρονη.
                if (Remoting.RemotingServices.IsOutOfProcess(EnlistedObjectsStateManager as System.MarshalByRefObject))
                {
                    Transactions.ActionRequest commitRequest = new Transactions.ActionRequest((Remoting.RemotingServices.GetOrgObject(EnlistedObjectsStateManager as System.MarshalByRefObject) as ObjectsStateManager).CommitRequest);
                    System.IAsyncResult asyncResult = commitRequest.BeginInvoke(this, null, null);
                }
                else
                    EnlistedObjectsStateManager.CommitRequest(this);
#else
                EnlistedObjectsStateManager.CommitRequest(this);
#endif

                if (ChangeState != null)
                    ChangeState(this, ObjectsStateManagerStatus.CommitRequest);
            }
            else if (_Status != ObjectsStateManagerStatus.CommitRequest && _Status != ObjectsStateManagerStatus.CommitDone)
                throw new OOAdvantech.Transactions.TransactionException("Only from PrepareDone state go to CommitRequest");
        }

        void AsyncPreparRequestCallBack(System.IAsyncResult asyncResult)
        {
            Transactions.ActionRequest prepareRequest = asyncResult.AsyncState as Transactions.ActionRequest;

            try
            {
                prepareRequest.EndInvoke(asyncResult);
            }
            catch (System.Exception error)
            {
                OwnerEnlistmentsController.AbortRequest(error);
            }

        }
        /// <summary>Propagate asynchronously the method call to objects state manager that enlisted in transaction.
        /// (Transaction manager ask from objects state manager to make all needed actions in order that the objectss to be in a consistent state and durable for the persistent objects. This is phase two of the two-phase commit protocol.)</summary>
        /// <MetaDataID>{D26E12A7-83C2-412E-A690-F4640A20B0D8}</MetaDataID>
        public void PrepareRequest()
        {


            bool inOnAction = false;
            lock (this)
            {
                if (_Status == ObjectsStateManagerStatus.OnAction)
                {
                    _Status = ObjectsStateManagerStatus.PrepareRequest;
                    inOnAction = true;
                }
            }
            if (inOnAction)
            {
#if !DeviceDotNet
                //Εάν ο object state manager είναι out of process τότε λόγο του [OneWay] attribute η method call
                //θα είναι ασύγχρονη αλλιώς σύγχρονη.
                if (Remoting.RemotingServices.IsOutOfProcess(EnlistedObjectsStateManager as System.MarshalByRefObject))
                {

                    Transactions.ActionRequest PrepareRequest = new Transactions.ActionRequest((Remoting.RemotingServices.GetOrgObject(EnlistedObjectsStateManager as System.MarshalByRefObject) as ObjectsStateManager).PrepareRequest);
                    System.IAsyncResult asyncResult = PrepareRequest.BeginInvoke(this, new System.AsyncCallback(AsyncPreparRequestCallBack), PrepareRequest);
                }
                else
                    EnlistedObjectsStateManager.PrepareRequest(this);
#else
                EnlistedObjectsStateManager.PrepareRequest(this);
#endif

                if (ChangeState != null)
                    ChangeState(this, ObjectsStateManagerStatus.PrepareRequest);
            }
            else if (_Status != ObjectsStateManagerStatus.PrepareRequest && _Status != ObjectsStateManagerStatus.PrepareDone)
                throw new OOAdvantech.Transactions.TransactionException("Only from OnAction state go to PrepareRequest");
        }
        public bool HasChangesOnElistedObjects(bool checkOnlyPersistentClassInstances)
        {
            
            {

                bool inOnAction = false;
                lock (this)
                {
                    if (_Status == ObjectsStateManagerStatus.OnAction)
                        inOnAction = true;
                }
                if (inOnAction)
                    return EnlistedObjectsStateManager.HasChangesOnElistedObjects(checkOnlyPersistentClassInstances);
                return true;

            }
        }



        //        /// <summary>Propagate asynchronously the method call to objects state manager that enlisted in transaction.
        //        /// (Transaction manager ask from objects state manager to make all needed actions in order that the objectss to be in a consistent state and durable for the persistent objects. This is phase two of the two-phase commit protocol.)</summary>
        //        /// <MetaDataID>{1D0E0C68-932C-4502-A5A7-27996E3AEF59}</MetaDataID>
        //        public void PrepareRequest()
        //        {

        //            if (Status == ObjectsStateManagerStatus.MakeDurableDone)
        //            {

        //                LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
        //                try
        //                {
        //                    _Status = ObjectsStateManagerStatus.PrepareRequest;
        //                }
        //                finally
        //                {
        //                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
        //                }
        //#if !DeviceDotNet 
        //                //Εάν ο object state manager είναι out of process τότε λόγο του [OneWay] attribute η method call
        //                //θα είναι ασύγχρονη αλλιώς σύγχρονη.
        //                if (Remoting.RemotingServices.IsOutOfProcess(EnlistedObjectsStateManager as System.MarshalByRefObject))
        //                {
        //                    Transactions.ActionRequest prepareRequest = new Transactions.ActionRequest((Remoting.RemotingServices.GetOrgObject(EnlistedObjectsStateManager as System.MarshalByRefObject) as ObjectsStateManager).PrepareRequest);
        //                    System.IAsyncResult asyncResult = prepareRequest.BeginInvoke(this, null, null);
        //                }
        //                else
        //                    EnlistedObjectsStateManager.PrepareRequest(this);
        //#else
        //                EnlistedObjectsStateManager.PrepareRequest(this);
        //#endif

        //                if (ChangeState != null)
        //                    ChangeState(this, ObjectsStateManagerStatus.PrepareRequest);
        //            }
        //            else
        //                throw new OOAdvantech.Transactions.TransactionException("Only from OnAction state go to MakeDurableDone");

        //        }
        ///// <MetaDataID>{1ACB918A-3BF9-4F2B-9851-F096BA48B3DA}</MetaDataID>
        //private ReaderWriterLock ReaderWriterLock=new ReaderWriterLock();
        /// <summary>Propagate asynchronously the method call to objects state manager that enlisted in transaction. (Transaction manager inform that the transaction aborted and ask from objects state manager to roll back the state of objects.)</summary>
        /// <MetaDataID>{2217FF49-B612-420D-9930-471BCEE6CC1E}</MetaDataID>
        public void AbortRequest()
        {
            bool changeSate = false;
            lock (this)
            {
                changeSate = _Status != ObjectsStateManagerStatus.AbortRequest;
                _Status = ObjectsStateManagerStatus.AbortRequest;
            }

            if (changeSate)
            {
#if !DeviceDotNet
                //Εάν ο object state manager είναι out of process τότε λόγο του [OneWay] attribute η method call
                //θα είναι ασύγχρονη αλλιώς σύγχρονη.
                if (Remoting.RemotingServices.IsOutOfProcess(EnlistedObjectsStateManager as System.MarshalByRefObject))
                {
                    Transactions.ActionRequest abortRequest = new Transactions.ActionRequest((Remoting.RemotingServices.GetOrgObject(EnlistedObjectsStateManager as System.MarshalByRefObject) as ObjectsStateManager).AbortRequest);
                    System.IAsyncResult asyncResult = abortRequest.BeginInvoke(this, null, null);
                }
                else
                    EnlistedObjectsStateManager.AbortRequest(this);
#else
            EnlistedObjectsStateManager.AbortRequest(this);
#endif

                if (ChangeState != null)
                    ChangeState(this, ObjectsStateManagerStatus.AbortRequest);
            }

        }
        ///// <MetaDataID>{A93CF471-8F00-439C-A1AC-26041C3A2318}</MetaDataID>
        //public ObjectsStateManagerState State=ObjectsStateManagerState.OnAction;
        /// <MetaDataID>{A5C50732-C4C9-4660-9FCD-00631F64D0B5}</MetaDataID>
        public event ObjectsStateManagerStatusChanged ChangeState;
        ///// <summary>Objects state manager inform that is ready to commit transaction. This is phase one of the two-phase commit protocol.</summary>
        ///// <MetaDataID>{34B4574D-CF93-4C6A-8613-1A4EAC754977}</MetaDataID>
        //public void PrepareRequestDone()
        //{

        //    LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
        //    try
        //    {
        //        _Status = ObjectsStateManagerStatus.PrepareDone;
        //    }
        //    finally
        //    {
        //        ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
        //    }
        //    if (ChangeState != null)
        //        ChangeState(this, ObjectsStateManagerStatus.PrepareDone);

        //}

        /// <summary>Objects state manager inform that is ready to commit transaction. This is phase one of the two-phase commit protocol.</summary>
        /// <MetaDataID>{64DD69BC-96A2-48FC-8DC3-C47934A95002}</MetaDataID>
        public void PrepareRequestDone()
        {
            bool changeStateToPrepareDone = false;
            lock (this)
            {
                changeStateToPrepareDone = _Status != ObjectsStateManagerStatus.PrepareDone;
                _Status = ObjectsStateManagerStatus.PrepareDone;
            }

            if (ChangeState != null && changeStateToPrepareDone)
                ChangeState(this, ObjectsStateManagerStatus.PrepareDone);
        }
        /// <summary>Objects state manager inform that commit the objects state that participated in transaction. This is phase two of the two-phase commit protocol.</summary>
        /// <MetaDataID>{F0287329-4B4A-4EA0-804F-71388F29789F}</MetaDataID>
        public void CommitRequestDone()
        {
            bool changeStateToCommitDone = false;
            lock (this)
            {
                changeStateToCommitDone = _Status != ObjectsStateManagerStatus.CommitDone;
                _Status = ObjectsStateManagerStatus.CommitDone;
            }

            if (ChangeState != null && changeStateToCommitDone)
                ChangeState(this, ObjectsStateManagerStatus.CommitDone);

        }
        /// <summary>Objects state manager inform that abort changes of the transaction.</summary>
        /// <MetaDataID>{99550B0F-DAE2-4870-8BCD-2FD2F011D225}</MetaDataID>
        public void AbortRequestDone()
        {
            bool changeStateToAbortDone = false;
            lock (this)
            {
                changeStateToAbortDone = _Status != ObjectsStateManagerStatus.AbortDone;
                _Status = ObjectsStateManagerStatus.AbortDone;
            }

            if (ChangeState != null && changeStateToAbortDone)
                ChangeState(this, ObjectsStateManagerStatus.AbortDone);
        }
        /// <MetaDataID>{BBD35FE5-AE80-4843-AF46-4EC438D1BB4D}</MetaDataID>
        public ObjectsStateManager EnlistedObjectsStateManager;

        IEnlistmentsController OwnerEnlistmentsController;

        /// <MetaDataID>{A1CB68D2-2BC4-4DA0-BEE1-07AB87FC6070}</MetaDataID>
        public TransactionEnlistment(ObjectsStateManager enlistedObjectsStateManager, string transactionUri,IEnlistmentsController ownerEnlistmentsController) 
        {
            OwnerEnlistmentsController = ownerEnlistmentsController;
            TransactionUri = transactionUri;
            EnlistedObjectsStateManager = enlistedObjectsStateManager;
        }

        /// <MetaDataID>{66397E7E-1515-4ED3-B70B-732D5066F448}</MetaDataID>
        public void OnAction()
        {


            lock (this)
            {
                _Status = ObjectsStateManagerStatus.OnAction;
            }
        }
    }
}
