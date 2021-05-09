
#if DeviceDotNet
using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
using LockCookie = OOAdvantech.Synchronization.LockCookie;
using OOAdvantech.Remoting;
#else
#if OOAdvantech
using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
using LockCookie = OOAdvantech.Synchronization.LockCookie;
using System;
#else
    using ReaderWriterLock = System.Threading.ReaderWriterLock;
    using LockCookie = System.Threading.LockCookie; 
#endif
#endif

namespace OOAdvantech.Transactions
{
    ///// <MetaDataID>{8162cba7-17fe-49e3-bcce-facf203894a2}</MetaDataID>
    //public enum ObjectsStateManagerStatus
    //{
    //    OnAction = 0,
    //    PrepareRequest,
    //    PrepareDone,
    //    //PrepareRequest,
    //    //PrepareDone,
    //    CommitRequest,
    //    CommitDone,
    //    AbortRequest,
    //    AbortDone
    //}

    internal delegate void ObjectsStateManagerStatusChanged(object sender, ObjectsStateManagerStatus newState);
    public delegate void TransactionCompletedEventHandler(Transaction transaction);
    public delegate void TransactionDistributedEventHandler(Transaction transaction);





    /// <MetaDataID>{185ED953-25E3-4889-9612-4131037A2DE0}</MetaDataID>
    /// <summary>This class defines a collection of objects state manager enlistments. 
    /// It has three states PrepareRequestDone, AbortRequestDone, CommitRequestDone. 
    /// Change state when all enlistments goes to the new state. 
    /// Always raise corresponding event for new state.</summary>
    internal class EnlistmentsController : MarshalByRefObject, IEnlistmentsController, ObjectsStateManager
    {
        internal delegate void TransactionCompletedEventHandler(TransactionStatus transactionState);

        ///// <MetaDataID>{BDEF2F8C-0C19-43D4-A015-A776D5D468E4}</MetaDataID>
        //public System.Collections.ArrayList GetEnlistedObjectsStateManagers()
        //{

        //    Collections.ArrayList objectsStateManagers = new Collections.ArrayList();
        //    OOAdvantech.Collections.Generic.List<TransactionEnlistment> enlistments = null;
        //    lock (this)
        //    {
        //        enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
        //    }

        //    foreach (TransactionEnlistment transactionEnlistment in enlistments)
        //    {
        //        if (transactionEnlistment.EnlistedObjectsStateManager is EnlistmentsController)
        //            objectsStateManagers.AddRange((transactionEnlistment.EnlistedObjectsStateManager as EnlistmentsController).GetEnlistedObjectsStateManagers());
        //        else
        //            objectsStateManagers.Add(transactionEnlistment.EnlistedObjectsStateManager);
        //    }
        //    return objectsStateManagers;
        //}
        /// <MetaDataID>{E66226E8-9D78-47D5-A5BA-DC659C2156CC}</MetaDataID>
        public void DetatchEnlistments()
        {
            OOAdvantech.Collections.Generic.List<TransactionEnlistment> enlistments = null;
            lock (this)
            {
#if !DeviceDotNet
                System.Transactions.Transaction nativeTransaction = NativeTransaction as System.Transactions.Transaction;
                if (nativeTransaction != null)
                    nativeTransaction.TransactionCompleted -= new System.Transactions.TransactionCompletedEventHandler(OnNativeTransactionCompleted);
#endif
                enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
                _Enlistments.Clear();
            }

            foreach (TransactionEnlistment transactionEnlistment in enlistments)
            {
                transactionEnlistment.ChangeState -= new ObjectsStateManagerStatusChanged(OnObjectsStateManagerChangeState);
                if (transactionEnlistment.EnlistedObjectsStateManager is NestedTransactionController)
                    (transactionEnlistment.EnlistedObjectsStateManager as NestedTransactionController).TransactionCompletted.Set();
                transactionEnlistment.EnlistedObjectsStateManager = null;
            }

        }

        /// <MetaDataID>{2C09ECC2-CBC6-45DB-8F61-BE073A4AF69E}</MetaDataID>
        public bool StillActive
        {
            get
            {
#if !DeviceDotNet
                try
                {
                    if (InitiatorProcess.HasExited)
                    {
                        if (Status == ObjectsStateManagerStatus.AbortDone)
                            return false;
                        System.Transactions.Transaction transaction = NativeTransaction as System.Transactions.Transaction;
                        if (transaction != null && transaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Active)
                            (NativeTransaction as System.Transactions.Transaction).Rollback(new System.Exception("Transaction initiator process has exited."));
                        else
                            AbortRequest(new System.Exception("Transaction initiator process has exited."));
                        if (Status == ObjectsStateManagerStatus.AbortDone)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        try
                        {
                            OOAdvantech.Collections.Generic.List<TransactionEnlistment> enlistmenWithoutCommunication = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>();
                            OOAdvantech.Collections.Generic.List<TransactionEnlistment> enlistments = null;
                            lock (this)
                            {
                                enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
                            }

                            foreach (TransactionEnlistment transactionEnlistment in enlistments)
                            {
                                if (!transactionEnlistment.CommunicationActive)
                                    enlistmenWithoutCommunication.Add(transactionEnlistment);
                            }
                            lock (this)
                            {
                                foreach (TransactionEnlistment transactionEnlistment in enlistmenWithoutCommunication)
                                    _Enlistments.Remove(transactionEnlistment);
                            }
                            if (enlistmenWithoutCommunication.Count > 0)
                            {
                                AbortRequest(new System.Exception("Distributed transaction communication error"));
                                return false;
                            }
                        }
                        catch (System.Exception error)
                        {

                        }
                        return true;
                    }
                }
                catch (System.Exception error)
                {
                    return true;
                }
#else
            return false;
#endif
            }
        }



        /// <MetaDataID>{75600996-4E40-42AA-AD68-91FB671F3215}</MetaDataID>
        public void AttachToNativeTransaction(System.Transactions.Transaction nativeTransaction)
        {
#if DISTRIBUTED_TRANSACTIONS
            lock (this)
            {
                
                if (nativeTransaction == null)
                    throw new OOAdvantech.Transactions.TransactionException("There isn't native transaction to attach.");

                if (NativeTransaction != null && nativeTransaction.TransactionInformation.LocalIdentifier != (NativeTransaction as System.Transactions.Transaction).TransactionInformation.LocalIdentifier)
                    throw new OOAdvantech.Transactions.TransactionException("Transaction context already  attached to native transaction.");
                if (NativeTransaction != null)
                    return;

                nativeTransaction.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(OnNativeTransactionCompleted);
                _NativeTransaction = nativeTransaction;
            }
#endif
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{0F0D7C13-706C-41DC-B6D9-CD4819CA8951}</MetaDataID>
        private object _NativeTransaction;
        /// <summary>Define the transaction of native transaction system in this case a COM+ transaction;</summary>
        /// <MetaDataID>{1A00F7E0-967D-485C-A4E3-F1B7D5E3157D}</MetaDataID>
        public object NativeTransaction
        {
            get
            {

                lock (this)
                {
                    return _NativeTransaction;
                }

            }
            set
            {
                lock (this)
                {
                    _NativeTransaction = value;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{EAE23DC6-9F76-464F-BD31-13153ED1921D}</MetaDataID>
        private OOAdvantech.Collections.Generic.List<TransactionEnlistment> _Enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>();
        ///// <MetaDataID>{C0F8E2E3-1051-4842-981B-509D5C32611F}</MetaDataID>
        //public OOAdvantech.Collections.Generic.List<TransactionEnlistment> Enlistments
        //{
        //    get
        //    {

        //        lock(this)
        //        {
        //            return new OOAdvantech.Collections.Generic.List<TransactionEnlistment>( _Enlistments);
        //        }
        //    }
        //}
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E2573762-E0AB-46B0-9B9A-1FC0A825E374}</MetaDataID>
        private string _GlobalTransactionUri;
        /// <MetaDataID>{C99A9D3B-FCE0-4848-9736-CD5D9C840EA9}</MetaDataID>
        /// <summary>Define the identity of transaction.</summary>
        public string GlobalTransactionUri
        {
            get
            {
                string tmpTransactionURI = null;
                lock (this)
                {
                    tmpTransactionURI = _GlobalTransactionUri;
                }
                return tmpTransactionURI;
            }
            set
            {
                lock (this)
                {
                    _GlobalTransactionUri = value;
                }
            }
        }


        public System.DateTime _LastStatusChangeTimestamp;

        /// <MetaDataID>{386FA399-04EB-47E6-A52D-41BECA9BF425}</MetaDataID>
        public System.DateTime LastStatusChangeTimestamp
        {
            get
            {
                return _LastStatusChangeTimestamp;
            }
            set
            {
                _LastStatusChangeTimestamp = value;
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{20297CBF-5C22-4F60-8386-71ECB467F9BC}</MetaDataID>
        private ObjectsStateManagerStatus _Status;
        /// <MetaDataID>{2399A09D-04CD-4A1D-B6C1-71A75CFD889D}</MetaDataID>
        public ObjectsStateManagerStatus Status
        {
            get
            {

                lock (this)
                {
                    return _Status;
                }
            }
            set
            {

                bool stateChanged = false;
                lock (this)
                {
                    if (_Status != value)
                    {
                        stateChanged = true;
                        LastStatusChangeTimestamp = System.DateTime.Now;
                        _Status = value;

                    }
                }
                if (stateChanged)
                    ChangeStateEvent.Set();

            }
        }

        /// <MetaDataID>{874B1028-5703-4761-98FA-36C2A20FEE85}</MetaDataID>
        private System.Threading.ManualResetEvent ChangeStateEvent = new System.Threading.ManualResetEvent(false);





        /// <MetaDataID>{224D36E1-3060-44EA-96CE-0CD3102EFFCE}</MetaDataID>
        public bool TimeExpired()
        {
            bool tt = false;
            return tt;
        }

        public bool HasChangesOnElistedObjects(bool checkOnlyPersistentClassInstances)
        {
            
            {
                ObjectsStateManagerStatus status = Status;
                if (status != ObjectsStateManagerStatus.OnAction)
                {
                    if (Status == ObjectsStateManagerStatus.AbortDone && AbortReasons.Count > 0)
                        throw new AbortException(AbortReasons, "Transaction aborted");

                    return true;
                }

                Collections.Generic.List<TransactionEnlistment> enlistments = null;
                bool noEnlistments = false;
                lock (this)
                {
                    noEnlistments = _Enlistments.Count == 0;
                }
                if (noEnlistments)
                {
                    return false;
                }
                lock (this)
                {
                    enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
                }

                foreach (TransactionEnlistment subTransactionEnlistment in enlistments)
                {
                    if (subTransactionEnlistment.HasChangesOnElistedObjects(checkOnlyPersistentClassInstances))
                        return true;
                }
                return false;
            }
        }

        /// <MetaDataID>{23F07232-7F67-4585-89D1-1D19FE0CC051}</MetaDataID>
        public ObjectsStateManagerStatus PrepareRequest()
        {

            ObjectsStateManagerStatus status = Status;

            if (status != ObjectsStateManagerStatus.OnAction && status != ObjectsStateManagerStatus.PrepareDone)
            {
                if (Status == ObjectsStateManagerStatus.AbortDone && AbortReasons.Count > 0)
                    throw new AbortException(AbortReasons, "Transaction aborted");
                return status;
            }

            Status = ObjectsStateManagerStatus.PrepareRequest;

            ////EnlistmentsController can go to PrepareRequest state from OnAction or  PrepareDone state
            //Status = ObjectsStateManagerStatus.PrepareRequest;

            //Zero can be in the case where a transaction started and there isn't enlistment.
            Collections.Generic.List<TransactionEnlistment> enlistments = null;
            bool noEnlistments = false;
            lock (this)
            {
                noEnlistments = _Enlistments.Count == 0;
            }
            if (noEnlistments)
            {
                Status = ObjectsStateManagerStatus.PrepareDone;
                OnObjectsStateManagerChangeState(this, ObjectsStateManagerStatus.PrepareDone);
                return ObjectsStateManagerStatus.PrepareDone;
            }
            lock (this)
            {
                enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
            }

            foreach (TransactionEnlistment subTransactionEnlistment in enlistments)
            {
                //We use try catch to ensure the call of stay live objects state managers. 
                //In transaction live some enlisted objects state managers actually remote objects state managers,
                //may be disconnect from transaction manager (for reasons of low quality network connections).
                try
                {
                    subTransactionEnlistment.PrepareRequest();
                }
                catch (System.Exception errror)
                {
                    int rtt = 0;
                }
            }
            ChangeStateEvent.Reset();
            while (Status == ObjectsStateManagerStatus.PrepareRequest)
            {
#if !DeviceDotNet
                ChangeStateEvent.WaitOne(10, false);
#else
                ChangeStateEvent.WaitOne(10);
#endif
                if (TimeExpired())
                    AbortRequest();
            }
            ChangeStateEvent.Reset();
            while (Status == ObjectsStateManagerStatus.AbortRequest)
            {
#if !DeviceDotNet
                ChangeStateEvent.WaitOne(10, false);
#else
                ChangeStateEvent.WaitOne(10);
#endif

                if (TimeExpired())
                    AbortRequest();
            }
#region Delay for abort reasons
            int delayCount = 0;
            if (Status == ObjectsStateManagerStatus.AbortDone)
            {
                while (delayCount < 150 && AbortReasons.Count == 0)
                {

#if !DeviceDotNet
                    System.Threading.Thread.Sleep(10);
#else
                    System.Threading.Tasks.Task.Delay(10).Wait();
#endif
                    delayCount++;
                }
                if (AbortReasons.Count > 0)
                    throw new AbortException(AbortReasons, "Transaction aborted", AbortReasons[0]);
            }
#endregion
            return Status;
        }

        ///// <summary>The transaction manager is requesting that the objects state manager prepare the transaction to commit. This is phase one of the two-phase commit protocol.</summary>
        ///// <MetaDataID>{E7348574-A5AB-42CB-9014-01427CB71028}</MetaDataID>
        //public ObjectsStateManagerStatus PrepareRequest()
        //{
        //    ObjectsStateManagerStatus status = Status;
        //    if (status != ObjectsStateManagerStatus.MakeDurableDone)
        //    {
        //        if (Status == ObjectsStateManagerStatus.AbortDone && AbortReasons.Count > 0)
        //            throw new Exception(AbortReasons, "Transaction aborted", AbortReasons[0]);
        //         return status;
        //    }
        //    Status = ObjectsStateManagerStatus.PrepareRequest;

        //    Collections.Generic.List<TransactionEnlistment> enlistments = null;
        //    ReaderWriterLock.AcquireReaderLock(10000);
        //    try
        //    {
        //        if (_Enlistments.Count == 0)
        //        {
        //            OnObjectsStateManagerChangeState(this, ObjectsStateManagerStatus.PrepareDone);
        //            return ObjectsStateManagerStatus.PrepareDone;

        //        }
        //        enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments.Count);
        //        foreach (TransactionEnlistment transactionEnlistment in _Enlistments)
        //        {
        //            //We use try catch to ensure the call of stay live objects state managers. 
        //            //In transaction live some enlisted objects state managers actually remote objects state managers,
        //            //may be disconnect from transaction manager (for reasons of low quality network connections).
        //            try
        //            {
        //                enlistments.Add(transactionEnlistment);
        //            }
        //            catch (System.Exception)
        //            {
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        ReaderWriterLock.ReleaseReaderLock();
        //    }
        //    foreach (TransactionEnlistment subTransactionEnlistment in enlistments)
        //    {
        //        //We use try catch to ensure the call of stay live objects state managers. 
        //        //In transaction live some enlisted objects state managers actually remote objects state managers,
        //        //may be disconnect from transaction manager (for reasons of low quality network connections).
        //        try
        //        {
        //            subTransactionEnlistment.PrepareRequest();
        //        }
        //        catch (System.Exception errror)
        //        {
        //            int rtt = 0;
        //        }
        //    }

        //    while (Status == ObjectsStateManagerStatus.PrepareRequest)
        //    {
        //        ChangeStateEvent.WaitOne(50, false);
        //        if (TimeExpired())
        //            AbortRequest();
        //    }
        //    while (Status == ObjectsStateManagerStatus.AbortRequest)
        //    {
        //        ChangeStateEvent.WaitOne(50, false);
        //        if (TimeExpired())
        //            AbortRequest();
        //    }
        //    if (Status == ObjectsStateManagerStatus.AbortDone && AbortReasons.Count > 0)
        //        throw new Exception(AbortReasons, "Transaction aborted", AbortReasons[0]);

        //    return Status;

        //}
        /// <summary>The transaction manager is requesting that the objects state manager commit the transaction. This is phase two of the two-phase commit protocol.</summary>
        /// <MetaDataID>{7182AC35-D77B-4517-96FE-C3C24444E9C2}</MetaDataID>
        public ObjectsStateManagerStatus CommitRequest()
        {


            Status = ObjectsStateManagerStatus.CommitRequest;


            Collections.Generic.List<TransactionEnlistment> enlistments = null;
            bool noEnlistments = false;
            lock (this)
            {
                noEnlistments = _Enlistments.Count == 0;
            }
            if (noEnlistments)
            {
                Status = ObjectsStateManagerStatus.CommitDone;
                OnObjectsStateManagerChangeState(this, ObjectsStateManagerStatus.CommitDone);
                return ObjectsStateManagerStatus.CommitDone;
            }
            lock (this)
            {
                enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
            }


            foreach (TransactionEnlistment transactionEnlistment in enlistments)
            {
                //We use try catch to ensure the call of stay live objects state managers. 
                //In transaction live some enlisted objects state managers actually remote objects state managers,
                //may be disconnect from transaction manager (for reasons of low quality network connections).
                try
                {
                    transactionEnlistment.CommitRequest();
                }
                catch (System.Exception)
                {
                }
            }
            ChangeStateEvent.Reset();
            while (Status == ObjectsStateManagerStatus.CommitRequest)
            {
#if !DeviceDotNet
                ChangeStateEvent.WaitOne(10, false);
#else
                ChangeStateEvent.WaitOne(10);
#endif
      
                if (TimeExpired())
                    break;
            }


            return Status;
        }

        /// <MetaDataID>{5F802A94-DDEA-4E13-9E91-21EDE07A14E5}</MetaDataID>
        private OOAdvantech.Collections.Generic.List<System.Exception> AbortReasons = new OOAdvantech.Collections.Generic.List<System.Exception>();

        /// <MetaDataID>{E703A53B-D768-42C1-96BC-297F83911DCE}</MetaDataID>
        public ObjectsStateManagerStatus AbortRequest()
        {
            return AbortRequest(null);
        }
        /// <summary>Transaction manager is requesting that the objects state manager abort the transaction.</summary>
        /// <MetaDataID>{A1AFC2F5-A076-4156-B393-A6CBAC8DCD5E}</MetaDataID>
        public ObjectsStateManagerStatus AbortRequest(System.Exception exception)
        {
            if (exception != null)
                AbortReasons.Add(exception);

            ObjectsStateManagerStatus status = Status;
            if (status == ObjectsStateManagerStatus.AbortRequest || status == ObjectsStateManagerStatus.AbortDone)
                return status;
            Status = ObjectsStateManagerStatus.AbortRequest;
            if ((NativeTransaction as System.Transactions.Transaction) != null)
            {
                (NativeTransaction as System.Transactions.Transaction).Rollback();
                return Status;
            }

            //Zero can be in the case where a transaction started and there isn't enlistment.
            Collections.Generic.List<TransactionEnlistment> enlistments = null;

                        bool noEnlistments = false;
            lock (this)
            {
                noEnlistments = _Enlistments.Count == 0;
            }
            if (noEnlistments)
            {


                Status = ObjectsStateManagerStatus.AbortDone;
                OnObjectsStateManagerChangeState(this, ObjectsStateManagerStatus.AbortDone);
                return status;
            }
            lock (this)
            {
                enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
            }
  

            foreach (TransactionEnlistment transactionEnlistment in enlistments)
            {
                //We use try catch to ensure the call of stay live objects state managers. 
                //In transaction live some enlisted objects state managers actually remote objects state managers,
                //may be disconnect from transaction manager (for reasons of low quality network connections).

                try
                {
                    transactionEnlistment.AbortRequest();
                }
                catch (System.Exception)
                {
                }
            }
            ChangeStateEvent.Reset();
            while (Status == ObjectsStateManagerStatus.AbortRequest)
            {
#if !DeviceDotNet
                ChangeStateEvent.WaitOne(10, false);
#else
                ChangeStateEvent.WaitOne(10);
#endif
      
                if (TimeExpired())
                    break;
            }

            return Status;

        }

        /// <MetaDataID>{7FE9E9C3-9342-45B9-B52F-26736BB55BE7}</MetaDataID>
        public event ObjectsStateManagerStatusChanged StatusChanged;
        
        public ITransactionCoordinator _TransactionInitiator;
        /// <MetaDataID>{1D3D2F7A-3F24-4E64-961D-B45D9091700C}</MetaDataID>
        public ITransactionCoordinator TransactionInitiator
        {
            get
            {
                return _TransactionInitiator;
            }
            set
            {
                _TransactionInitiator = value;
            }
        }



        public event  TransactionEndedEventHandler TransactionCompletted;
#if DISTRIBUTED_TRANSACTIONS
        /// <MetaDataID>{3230977D-5282-437B-B9A9-FB0586821513}</MetaDataID>
        public void OnNativeTransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        {
            try
            {

                ObjectsStateManagerStatus OldState = _Status;
                lock(this)
                {
#region Change state
                    if (e.Transaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Committed)
                        Status = ObjectsStateManagerStatus.CommitDone;
                    else
                        Status = ObjectsStateManagerStatus.AbortDone;
#endregion

#region Release transaction enlistment
                    foreach (TransactionEnlistment transactionEnlistment in _Enlistments)
                    {
                        transactionEnlistment.ChangeState -= new ObjectsStateManagerStatusChanged(OnObjectsStateManagerChangeState);
                        transactionEnlistment.EnlistedObjectsStateManager = null;
                    }
                    _Enlistments.Clear();
#endregion
                }
           
#region Inform for new state
                if (StatusChanged != null && OldState != _Status)
                    StatusChanged(this, Status);
                if (TransactionCompletted != null)
                {
                    if (Status == ObjectsStateManagerStatus.CommitDone)
                        TransactionCompletted(TransactionStatus.Committed);
                    if (Status == ObjectsStateManagerStatus.AbortDone)
                        TransactionCompletted(TransactionStatus.Aborted);

                }

#endregion
            }
            catch (System.Exception Error)
            {
            }
        }
#endif
        /// <MetaDataID>{FDECACE5-4184-4BF5-AA10-2E53DD97DEBB}</MetaDataID>
        public void OnObjectsStateManagerChangeState(object sender, ObjectsStateManagerStatus newState)
        {

            ObjectsStateManagerStatus status = Status;
            if (status == ObjectsStateManagerStatus.CommitDone || status == ObjectsStateManagerStatus.AbortDone)
                return;
            ObjectsStateManagerStatus OldState = status;
            if (newState == ObjectsStateManagerStatus.CommitDone ||
                newState == ObjectsStateManagerStatus.AbortDone ||
                newState == ObjectsStateManagerStatus.PrepareDone)
            {
                status = Status;


                if ((status == ObjectsStateManagerStatus.OnAction ||
                    status == ObjectsStateManagerStatus.PrepareRequest ||
                    status == ObjectsStateManagerStatus.PrepareDone) && newState == ObjectsStateManagerStatus.AbortDone)
                {
                    try
                    {
#if !DeviceDotNet
                        if (GlobalTransactionUri != null && GlobalTransactionUri.Length > 0)
                            TransactionRunTime.UnMarshal(GlobalTransactionUri).Abort(null);
                        else
                            AbortRequest();
#else
                        AbortRequest();
#endif
                    }
                    catch (System.Exception)
                    {
                    }
                    foreach (TransactionEnlistment transactionEnlistment in _Enlistments)
                    {
                        if (transactionEnlistment.Status != newState)
                            return;
                    }
                    Status = newState;

                    return;
                }
                if (status == ObjectsStateManagerStatus.OnAction && newState == ObjectsStateManagerStatus.PrepareDone)
                {
                    try
                    {
#if !DeviceDotNet
                        if (GlobalTransactionUri != null && GlobalTransactionUri.Length > 0)
                            TransactionRunTime.UnMarshal(GlobalTransactionUri).Abort(null);
                        else
                            AbortRequest();
#else
                        AbortRequest();
#endif

                    }
                    catch (System.Exception)
                    {
                    }
                    foreach (TransactionEnlistment transactionEnlistment in _Enlistments)
                    {
                        if (transactionEnlistment.Status != newState)
                            return;
                    }
                    Status = newState;
                    return;
                }

                foreach (TransactionEnlistment transactionEnlistment in _Enlistments)
                {
                    if (transactionEnlistment.Status != newState)
                        return;
                }
                Status = newState;
            }
            else
                return;

            status = Status;

            if (StatusChanged != null && OldState != status)
            {
                try
                {
                    StatusChanged(this, status);
                }
                catch (System.Exception)
                {
                }

            }
            if (TransactionCompletted != null)
            {
                try
                {
                    switch (status)
                    {
                        case ObjectsStateManagerStatus.AbortDone:
                            {
                                TransactionCompletted(TransactionStatus.Aborted);
                                break;
                            }
                        case ObjectsStateManagerStatus.CommitDone:
                            {
                                TransactionCompletted(TransactionStatus.Committed);
                                break;
                            }
                    }
                }
                catch
                {
                }
            }
            status = Status;

            if (status == ObjectsStateManagerStatus.CommitDone || status == ObjectsStateManagerStatus.AbortDone)
            {

                lock(this)
                {
                    foreach (TransactionEnlistment transactionEnlistment in _Enlistments)
                    {
                        transactionEnlistment.ChangeState -= new ObjectsStateManagerStatusChanged(OnObjectsStateManagerChangeState);
                    }
                    _Enlistments.Clear();
                }
            }
        }

        /// <param name="objectsStateManager">The objects state manager must be not null.</param>
        /// <MetaDataID>{D3758C67-B2EF-4D53-BAA3-FE894EB27C04}</MetaDataID>
        public void EnlistObjectsStateManager(ObjectsStateManager objectsStateManager)
        {

            lock (this)
            {
                if (Status != ObjectsStateManagerStatus.OnAction && Status != ObjectsStateManagerStatus.PrepareRequest)
                {
                    if (Status == ObjectsStateManagerStatus.AbortDone)
                        throw new TransactionException( "Transction with uri '" + GlobalTransactionUri + "' Aborted");
                    if (Status == ObjectsStateManagerStatus.CommitDone)
                        throw new OOAdvantech.Transactions.TransactionException("Transction with uri '" + GlobalTransactionUri + "' Commited");
                    throw new OOAdvantech.Transactions.TransactionException("You can enlist objects state manager only when transaction is in OnAction or PrepareRequest state");
                }
            }


            TransactionEnlistment transactionEnlistment = null;

            if (objectsStateManager == null)
                throw new OOAdvantech.Transactions.TransactionException("The objects state manager must be not null.");
            object nativeTransaction = null;
            lock (this)
            {
                foreach (TransactionEnlistment currTransactionEnlistment in _Enlistments)
                {
                    if (currTransactionEnlistment.EnlistedObjectsStateManager == objectsStateManager)
                        return;
                }
                nativeTransaction = NativeTransaction;
            }
            if (nativeTransaction != null)
            {
                EnlistmentsController enlistmentsController = objectsStateManager as EnlistmentsController;
                TransactionContext transactionContext = objectsStateManager as TransactionContext;
                NestedTransactionController nestedTransactionController = objectsStateManager as NestedTransactionController;
                if (enlistmentsController != null)
                    enlistmentsController.AttachToNativeTransaction(NativeTransaction as System.Transactions.Transaction);
                if (transactionContext != null)
                    transactionContext.AttachToNativeTransaction(NativeTransaction as System.Transactions.Transaction);
                if (nestedTransactionController != null)
                    nestedTransactionController.AttachToNativeTransaction(NativeTransaction as System.Transactions.Transaction);
            }
            ObjectsStateManagerStatus status;
            lock (this)
            {

                transactionEnlistment = new TransactionEnlistment(objectsStateManager, GlobalTransactionUri,this);
                transactionEnlistment.ChangeState += new ObjectsStateManagerStatusChanged(this.OnObjectsStateManagerChangeState);
                _Enlistments.Add(transactionEnlistment);
                status = Status;
            }
            if (status == ObjectsStateManagerStatus.PrepareRequest)
                transactionEnlistment.PrepareRequest();



        }

        /// <summary>Initializes a new instance of the OOAdvantech.Transactions.EnlistmentsController</summary>
        /// <param name="transactionUri">Define the identity of the transaction.</param>
        /// <MetaDataID>{E218462B-34CB-4BB7-8252-74BB0EB65A60}</MetaDataID>
        public EnlistmentsController(Transaction transaction)
        {
            // AttachToNativeTransaction(System.Transactions.Transaction.Current);
        }
        /// <summary>This constructor initializes a new instance of the OOAdvantech.Transactions.EnlistmentsController as surrogate of all objects state managers that act on this machine. This class act as surrogate in case where the transaction has begins in other machine. When an transaction began in one machine and propagated to other then the EnlistmentsController act like objects state manager, a objects state manager that surrogate the objects state managers in the local machine to the machine that began the transaction.</summary>
        /// <MetaDataID>{D6F37C02-AC3B-48F2-9798-5532A7755E76}</MetaDataID>
        public EnlistmentsController(string globalTransactionUri, ITransactionCoordinator transactionInitiator)
        {
            transactionInitiator.Enlist(this, ref globalTransactionUri);
            _GlobalTransactionUri = globalTransactionUri;
            TransactionInitiator = transactionInitiator;
            //AttachToNativeTransaction();
        }
#if !DeviceDotNet
        /// <MetaDataID>{31268873-1062-45D5-A1C7-771BBFCE7BA3}</MetaDataID>
        protected System.Diagnostics.Process InitiatorProcess;
#endif
        /// <MetaDataID>{BD290273-8874-4F21-A09B-D411DDE5C881}</MetaDataID>
        public EnlistmentsController(string globalTransactionUri, int initiatorProcessID)
        {
#if !DeviceDotNet
            InitiatorProcess = System.Diagnostics.Process.GetProcessById(initiatorProcessID);
#endif
            _GlobalTransactionUri = globalTransactionUri;
            //AttachToNativeTransaction();
        }
        /// <MetaDataID>{2BEBDF11-6298-4B57-9A12-F69D8A3B13EC}</MetaDataID>
        public EnlistmentsController(string globalTransactionUri)
        {
            _GlobalTransactionUri = globalTransactionUri;
            //AttachToNativeTransaction();
        }

        /// <MetaDataID>{4FD5AF6E-68E2-4E30-B583-DF28607AAE5D}</MetaDataID>
        ~EnlistmentsController()
        {
            int hh = 0;
        }



#region ObjectsStateManager Members





        /// <MetaDataID>{45C9D237-700D-45CF-8AD4-E65A8C4D1F8A}</MetaDataID>
        void ObjectsStateManager.PrepareRequest(ITransactionEnlistment transactionEnlistment)
        {

            ObjectsStateManagerStatus status = PrepareRequest();
            switch (status)
            {
                case ObjectsStateManagerStatus.PrepareDone:
                    transactionEnlistment.PrepareRequestDone();
                    break;
                //case ObjectsStateManagerStatus.PrepareDone:
                //    transactionEnlistment.PrepareRequestDone();
                //    break;
                case ObjectsStateManagerStatus.CommitDone:
                    transactionEnlistment.CommitRequestDone();
                    break;
                case ObjectsStateManagerStatus.AbortDone:
                    transactionEnlistment.CommitRequestDone();
                    break;
            }
        }

        //void ObjectsStateManager.PrepareRequest(ITransactionEnlistment transactionEnlistment)
        //{
        //    ObjectsStateManagerStatus status = PrepareRequest();
        //    switch (status)
        //    {
        //        case ObjectsStateManagerStatus.PrepareDone:
        //            transactionEnlistment.PrepareRequestDone();
        //            break;
        //        case ObjectsStateManagerStatus.CommitDone:
        //            transactionEnlistment.CommitRequestDone();
        //            break;
        //        case ObjectsStateManagerStatus.AbortDone:
        //            transactionEnlistment.CommitRequestDone();
        //            break;
        //    }
        //}

        /// <MetaDataID>{9F62CCC4-6C21-4540-92A6-A4CFF6452A74}</MetaDataID>
        void ObjectsStateManager.CommitRequest(ITransactionEnlistment transactionEnlistment)
        {
            ObjectsStateManagerStatus status = CommitRequest();
            switch (status)
            {
                case ObjectsStateManagerStatus.CommitDone:
                    transactionEnlistment.CommitRequestDone();
                    break;
            }
        }

        /// <MetaDataID>{FBED2A1B-E331-4A90-8FBE-E7C494B2CC4E}</MetaDataID>
        void ObjectsStateManager.AbortRequest(ITransactionEnlistment transactionEnlistment)
        {

            ObjectsStateManagerStatus status = AbortRequest();
            switch (status)
            {
                case ObjectsStateManagerStatus.AbortDone:
                    transactionEnlistment.CommitRequestDone();
                    break;
            }

        }

#endregion

        /// <MetaDataID>{F09B6A4A-3168-4BF4-86F9-B7FEAD00B325}</MetaDataID>
        internal void ReviseObjectsStateManager(ObjectsStateManager objectsStateManager)
        {
            TransactionEnlistment transactionEnlistment = null;
            OOAdvantech.Collections.Generic.List<TransactionEnlistment> enlistments = null;
            lock (this)
            {
                enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
            }

            foreach (TransactionEnlistment currTransactionEnlistment in enlistments)
            {
                if (currTransactionEnlistment.EnlistedObjectsStateManager == objectsStateManager)
                {
                    transactionEnlistment = currTransactionEnlistment;
                    break;
                }
            }
            if (transactionEnlistment == null)
                throw new OOAdvantech.Transactions.TransactionException("There isn't enlistment for this object state manager");

            if (Status == ObjectsStateManagerStatus.CommitRequest
                || Status == ObjectsStateManagerStatus.CommitDone
                || Status == ObjectsStateManagerStatus.AbortRequest
                || Status == ObjectsStateManagerStatus.AbortDone)
            {
                throw new TransactionException("System can't revise the object state manager from the sate : " + Status.ToString() + ".");
            }
            if (Status == ObjectsStateManagerStatus.PrepareDone)
            {
                Status = ObjectsStateManagerStatus.OnAction;
                lock (this)
                {
                    enlistments = new OOAdvantech.Collections.Generic.List<TransactionEnlistment>(_Enlistments);
                }

                foreach (TransactionEnlistment currTransactionEnlistment in enlistments)
                    currTransactionEnlistment.OnAction();

            }
            else if (Status == ObjectsStateManagerStatus.PrepareRequest && transactionEnlistment.Status == ObjectsStateManagerStatus.PrepareDone)
            {
                transactionEnlistment.PrepareRequest();
            }
        }
    }
}
