namespace OOAdvantech.Transactions
{

    using System;
    using System.Linq;
    using LockCookie = OOAdvantech.Synchronization.LockCookie;
    using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
    using System.Security.Principal;
    using Remoting;
#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
    using MarshalByRefObject = System.MarshalByRefObject;
#endif

    /// <MetaDataID>{E120D6F6-2A52-4311-892B-9AD110F3EB3C}</MetaDataID>
    /// <summary>Transaction proxy is an in client process object which delegate with transaction system for commit or abort the transaction with identity the transaction uri. Implement the Transaction interface. </summary>
    [Serializable]
    public class TransactionRunTime : Transaction, ITransactionStatusNotification, System.Runtime.Serialization.ISerializable
    {



        /// <MetaDataID>{5465001c-eb77-46fd-8822-39f8921629b4}</MetaDataID>
        private System.Threading.ManualResetEvent TransactionCompletedEvent = new System.Threading.ManualResetEvent(false);

        /// <summary>
        /// Blocks the current thread until the transaction complete, using
        /// 32-bit signed integer to measure the time interval in milliseconds and specifying whether to
        /// exit the synchronization domain before the wait.
        /// </summary>
        /// <param name="timeout">
        /// A 32-bit signed integer that represents the number of milliseconds to wait, or
        /// a 32-bit signed integer with -1 to wait indefinitely.
        /// </param>
        /// <returns>
        /// true if the transaction complete; otherwise, false.
        /// </returns>
        /// <MetaDataID>{3049872a-676e-4917-b8c9-40575e832150}</MetaDataID>
        public override bool WaitToComplete(int millisecondsTimeout)
        {
            if (InternalTransaction != null)
                return (InternalTransaction as TransactionRunTime).WaitToComplete(millisecondsTimeout);

#if !DeviceDotNet
            return TransactionCompletedEvent.WaitOne(millisecondsTimeout, false);
#else
            return TransactionCompletedEvent.WaitOne(millisecondsTimeout);
#endif
        }
        /// <summary>
        /// Blocks the current thread until the transaction complete, using
        /// a System.TimeSpan to measure the time interval and specifying whether to
        /// exit the synchronization domain before the wait.
        /// </summary>
        /// <param name="timeout">
        /// A System.TimeSpan that represents the number of milliseconds to wait, or
        /// a System.TimeSpan that represents -1 milliseconds to wait indefinitely.
        /// </param>
        /// <returns>
        /// true if the transaction complete; otherwise, false.
        /// </returns>
        /// <MetaDataID>{fefd6a13-8b39-409e-b80e-448d93cf0f17}</MetaDataID>
        public override bool WaitToComplete(TimeSpan timeout)
        {
            if (InternalTransaction != null)
                return (InternalTransaction as TransactionRunTime).WaitToComplete(timeout);
#if !DeviceDotNet
            return TransactionCompletedEvent.WaitOne(timeout, false);
#else
            return TransactionCompletedEvent.WaitOne((int)timeout.TotalMilliseconds);
#endif
        }
        ///// <MetaDataID>{EC4C609A-FF33-4F8E-90F6-1FDA98C5DBAF}</MetaDataID>
        //OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7FD4B3FC-3364-40D9-896C-0E90AEF710BD}</MetaDataID>
        private TransactionContext _TransactionContext;
        /// <MetaDataID>{609B06B0-4523-46EA-8C14-723F077CAE81}</MetaDataID>
        internal TransactionContext TransactionContext
        {
            get
            {
                lock (this)
                {
                    if (InternalTransaction != null)
                        return (InternalTransaction as TransactionRunTime).TransactionContext;

                    if (_TransactionContext == null)
                        CreateTransactionContext();
                    return _TransactionContext;
                }
            }
            set
            {
                lock (this)
                {

                    _TransactionContext = value;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{278C277F-16CD-4FAA-BBFB-3237E98E524B}</MetaDataID>
        private System.Transactions.Transaction _NativeTransaction;
        /// <MetaDataID>{0D86CD6C-B8E4-425C-90CC-F7A406FD7A09}</MetaDataID>
        internal System.Transactions.Transaction NativeTransaction
        {


            get
            {

#if !DeviceDotNet
                lock (this)
                {
                    if (InternalTransaction != null)
                        return (InternalTransaction as TransactionRunTime).NativeTransaction;
                    if (_NativeTransaction == null && !string.IsNullOrEmpty(_GlobalTransactionUri))
                    {
                        if (_TransactionContext == null)
                            CreateTransactionContext();
                        if (_NativeTransaction == null)
                            _NativeTransaction = _TransactionContext.NativeTransaction as System.Transactions.Transaction;
                        if (_NativeTransaction == null)
                            throw new OOAdvantech.Transactions.TransactionException("Transaction system can't retrieve native transaction");
                    }
                }
#endif

                return _NativeTransaction;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3CE3BC80-DE6C-4660-A6F3-65562342BC9D}</MetaDataID>
        private static ITransactionCoordinator _LocalTransactionCoordinator;
        /// <MetaDataID>{EB4559A1-4CCF-4779-B968-A73A951C26B7}</MetaDataID>
        internal static ITransactionCoordinator LocalTransactionCoordinator
        {
            get
            {
#if !DeviceDotNet

                lock (DistributedTransactions)
                {
                    if (_LocalTransactionCoordinator == null)
                    {
                        try
                        {

                            //if (Remoting.RemotingServices.ProcessUserAnonymous)
                            //    _LocalTransactionCoordinator = Remoting.RemotingServices.CreateInstance("tcp://localhost:9051", typeof(TransactionCoordinator).FullName, "") as ITransactionCoordinator;
                            //else
                            _LocalTransactionCoordinator = Remoting.RemotingServices.CreateRemoteInstance("tcp://localhost:9050", typeof(TransactionCoordinator).FullName, "") as ITransactionCoordinator;
                        }
                        catch (System.Exception error)
                        {
                            throw;

                        }
                    }
                }
                //Remoting.RemotingServices.CheckIPCServerChannelsAutorityGroup();
                return _LocalTransactionCoordinator;
#else
                throw new OOAdvantech.Transactions.TransactionException("The property LocalTransactionCoordinator is not implemented for PockePC. ");
#endif
            }
        }


        /// <MetaDataID>{152F7125-6353-4197-A1B9-10E2B43A1262}</MetaDataID>
        public System.Collections.Generic.List<Transaction> _NestedTransactions;

        /// <MetaDataID>{e26ee158-30c0-4be6-86ae-84e0b1eed771}</MetaDataID>
        public override System.Collections.Generic.List<Transaction> NestedTransactions
        {
            get 
            {
                if (InternalTransaction != null)
                    return InternalTransaction.NestedTransactions;

                if (_NestedTransactions != null)
                    return new System.Collections.Generic.List<Transaction>(_NestedTransactions);
                else
                    return new System.Collections.Generic.List<Transaction>();
            }
        }
        /// <MetaDataID>{44ACF223-F6DB-4BFA-A8D1-55060614CA97}</MetaDataID>
        TransactionRunTime _OriginTransaction;

        /// <MetaDataID>{44a850b8-8582-4aa7-addc-60bbd26c4d91}</MetaDataID>
        public override Transaction OriginTransaction
        {
            get
            {
                if (InternalTransaction != null)
                    return InternalTransaction.OriginTransaction;

                lock (this)
                {
                    return _OriginTransaction;
                }
            }

        }
        /// <MetaDataID>{F036A2F2-A179-48F5-AED3-66930FA6FF79}</MetaDataID>
        internal OOAdvantech.Collections.Generic.List<System.Exception> AbortReasons;



        /// <MetaDataID>{5ABEBDC7-843B-48FF-9ABD-DC3C687BEB2F}</MetaDataID>
        internal Transaction LaunchNestedTransaction()
        {
            TransactionRunTime nestedTransaction = null;
            bool localTransaction = false;
            lock (this)
            {
                nestedTransaction = new TransactionRunTime(this);
                if (_NestedTransactions == null)
                    _NestedTransactions = new OOAdvantech.Collections.Generic.List<Transaction>();
                //NestedTransactions.Add(nestedTransaction);
                localTransaction = _GlobalTransactionUri == null;
            }
#if !DeviceDotNet
            if (localTransaction)
            {
                NestedTransactionController nestedTransactionController = new NestedTransactionController(nestedTransaction.EnlistmentsController);
                EnlistmentsController.EnlistObjectsStateManager(nestedTransactionController);
            }
            else
                nestedTransaction.Marshal();
#else
            NestedTransactionController nestedTransactionController = new NestedTransactionController(nestedTransaction.EnlistmentsController);
            EnlistmentsController.EnlistObjectsStateManager(nestedTransactionController);
#endif
            return nestedTransaction;
        }
#if !DeviceDotNet
        //TODO it isn't thread safe
        /// <MetaDataID>{AED6D533-677F-42B9-BE08-BFEC01156A7B}</MetaDataID>
        internal bool InMarshal = false;
        /// <MetaDataID>{b0866a0b-1de4-4739-8583-375996f71388}</MetaDataID>
        string MarshalingString;

        /// <MetaDataID>{E2E76903-55DF-4FC5-AB0F-4C39012A410C}</MetaDataID>
        public override string Marshal()
        {
            
            bool tcpClientChannelRegister = false;
            bool ipcChannelRegister = false;

            if (!OOAdvantech.Remoting.RemotingServices.IsSeverChannelRegiter )
            {
                OOAdvantech.Remoting.RemotingServices.RegisterSecureIpcChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), true);
            }
            


            if (InternalTransaction != null)
                return (InternalTransaction as TransactionRunTime).Marshal();


            lock (this)
            {

                if (!string.IsNullOrEmpty(MarshalingString))
                    return MarshalingString;

                if (InMarshal)
                    return null;
                InMarshal = true;
            }
            try
            {
                bool transactionDistributed = false;
                lock (this)
                {
                    string globalTransactionUri = GlobalTransactionUri;
                    string originTransactionGlobalUri = null;
                    if (_OriginTransaction != null)
                        originTransactionGlobalUri = _OriginTransaction.Marshal();
                    //TODO ναγραφτεί test case για την περίπτωση που η original transaction depended

                    if (!string.IsNullOrEmpty(globalTransactionUri))
                    {
                        if (originTransactionGlobalUri != null)
                            MarshalingString = globalTransactionUri + @"\" + originTransactionGlobalUri;
                        else
                            MarshalingString = globalTransactionUri;
                    }
                    else
                    {

                        EnlistmentsController.DetatchEnlistments();
                        EnlistmentsController.TransactionCompletted -= new TransactionEndedEventHandler((this as ITransactionStatusNotification).OnTransactionCompletted);
                        IEnlistmentsController enlistmentsController = null;
                        TransactionStatusNotification = new TransactionStatusNotifier(this);
                        if (_OriginTransaction == null)
                        {
                            LocalTransactionCoordinator.ExportTransaction(System.TimeSpan.FromMilliseconds(0),
                                                                        System.Diagnostics.Process.GetCurrentProcess().Id,
                                                                        TransactionContext,
                                                                        _NativeTransaction,
                                                                        TransactionStatusNotification,
                                                                        out globalTransactionUri,
                                                                        out enlistmentsController);
                        }
                        else
                        {

                            LocalTransactionCoordinator.ExportAsNestedTransaction(System.TimeSpan.FromMilliseconds(0),
                                                                        System.Diagnostics.Process.GetCurrentProcess().Id,
                                                                        originTransactionGlobalUri,
                                                                        TransactionContext,
                                                                        _NativeTransaction,
                                                                        TransactionStatusNotification,
                                                                        out globalTransactionUri,
                                                                        out enlistmentsController);
                        }
                        _GlobalTransactionUri = globalTransactionUri;
                        EnlistmentsController = enlistmentsController;
                        DistributedTransactions.Add(globalTransactionUri, this);
                        Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTimer), null, 4000, 4000);
                        if (originTransactionGlobalUri != null)
                            MarshalingString = globalTransactionUri + @"\" + originTransactionGlobalUri;
                        else
                            MarshalingString = globalTransactionUri;


                        transactionDistributed = true;
                    }
                    if (transactionDistributed)
                    {
                        try
                        {
                            if (_TransactionDistributed != null)
                                _TransactionDistributed(this);
                        }
                        catch (System.Exception error)
                        {

                        }
                    }
                    return MarshalingString;
                }
            }
            finally
            {
                InMarshal = false;
            }
        }



        /// <MetaDataID>{A6F3A0A7-D04C-4147-9A68-D3837CB66738}</MetaDataID>
        static private Collections.Generic.Dictionary<string, Transaction> DistributedTransactions = new OOAdvantech.Collections.Generic.Dictionary<string, Transaction>();
#else
        public override string Marshal()
        {
            return null;
        }

#endif



#if DeviceDotNet
        /// <MetaDataID>{B31BB057-C4B1-4CB2-98D8-B3F9BBD68961}</MetaDataID>
        private System.Timers.Timer Timer;
#else
        /// <MetaDataID>{B31BB057-C4B1-4CB2-98D8-B3F9BBD68961}</MetaDataID>
        private System.Threading.Timer Timer = null;
#endif

        /// <MetaDataID>{84E46DA4-4FD5-425C-A631-69FDD679AABE}</MetaDataID>
        void OnTimer(object state)
        {
            try
            {

                //Είναι απαραίτητο γιατί αν φύγει προς τα έξω exception στον timer τότε θα τερματιστεί το process.
                TransactionContext transactionContext = null;
                lock (this)
                {
                    transactionContext = _TransactionContext;
                }
                if (transactionContext != null)
                {
                    try
                    {
                        if (LocalTransactionCoordinator.CommunicationCheck(GlobalTransactionUri))
                            return;
                    }
                    catch (System.Exception error)
                    {
                    }
                    if (Status == TransactionStatus.Continue
                        && transactionContext.Status != ObjectsStateManagerStatus.AbortDone
                        && transactionContext.Status != ObjectsStateManagerStatus.CommitDone)
                    {
                        transactionContext.AbortRequest(null);
                        OnTransactionCompletted(TransactionStatus.Aborted);
                    }
                }
            }
            catch (System.Exception error)
            {
                try
                {
#if !DeviceDotNet
                    if (_NativeTransaction != null && _NativeTransaction.TransactionInformation.Status != System.Transactions.TransactionStatus.Active)
                        Timer.Dispose();
#else
                    Timer.Dispose();
#endif
                }
                catch (System.Exception innerError)
                {
                }

            }
        }
#if DISTRIBUTED_TRANSACTIONS
        /// <MetaDataID>{7D040CC7-C28C-4EAC-B27B-4243AAEAB086}</MetaDataID>
        static Transaction UnMarshal(string globalTransactionUri, TransactionRunTime originalTransaction)
        {
            lock (DistributedTransactions)
            {
                if (globalTransactionUri.IndexOf("Derived_") == -1)
                {
                    if (DistributedTransactions.ContainsKey(globalTransactionUri))
                        return DistributedTransactions[globalTransactionUri];
                    else
                    {
                        Transaction transaction = new TransactionRunTime(globalTransactionUri, originalTransaction);
                        DistributedTransactions.Add(globalTransactionUri, transaction);
                        return transaction;
                    }
                }
                else
                {
                    string distributedIdentifier = TransactionCoordinator.GetDistributedIdentifier(globalTransactionUri);
                    if (DistributedTransactions.ContainsKey(distributedIdentifier))
                    {
                        TransactionRunTime transaction = DistributedTransactions[distributedIdentifier] as TransactionRunTime;
                        if (transaction.GlobalTransactionUri != globalTransactionUri)
                        {
                            transaction.ReOrderDerivedTransaction(globalTransactionUri);
                            transaction.MainTransaction = false;
                        }
                        return transaction;
                    }
                    else
                    {
                        Transaction transaction = new TransactionRunTime(globalTransactionUri, originalTransaction);
                        DistributedTransactions.Add(distributedIdentifier, transaction);
                        return transaction;
                    }

                }
            }
        }

        /// <MetaDataID>{2BCFED8E-2116-4DB8-80E9-46FA6540EA43}</MetaDataID>
        static public Transaction UnMarshal(string globalTransactionUri)
        {
            TransactionRunTime originalTransaction = null;
            while (globalTransactionUri.IndexOf(@"\") != -1)
            {
                int nPos = globalTransactionUri.LastIndexOf(@"\");
                string transactionUri = globalTransactionUri.Substring(nPos + 1);
                originalTransaction = UnMarshal(transactionUri, originalTransaction) as TransactionRunTime;
                globalTransactionUri = globalTransactionUri.Substring(0, nPos);
            }
            return UnMarshal(globalTransactionUri, originalTransaction);
        }
        //System.Collections.Generic.List<string> EquivalentGlobalTransactionUri = new System.Collections.Generic.List<string>();
        /// <MetaDataID>{FDAFF88C-D3CA-46BD-BCBE-A7B244EA6F43}</MetaDataID>
        private void ReOrderDerivedTransaction(string globalTransactionUri)
        {
            lock (this)
            {
                LocalTransactionCoordinator.ReOrderDerivedTransactions(ref globalTransactionUri);
                _GlobalTransactionUri = globalTransactionUri;
                MarshalingString = null;
            }
        }
#endif




        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B4FEB2CE-5D77-42C8-AFE4-F18C29C9A51E}</MetaDataID>
        private OOAdvantech.Transactions.TransactionStatus _Status;
        /// <MetaDataID>{6000EA30-8A00-4E35-9C11-8F422DFB20B4}</MetaDataID>
        public override OOAdvantech.Transactions.TransactionStatus Status
        {

            get
            {
                lock (this)
                {
                    if (InternalTransaction != null)
                        return InternalTransaction.Status;
                    return _Status;
                }
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{45380E81-B381-4CC7-A59B-1C7F06C396A4}</MetaDataID>
        private string _LocalTransactionUri;
        /// <summary>Define the identity of transaction. </summary>
        /// <MetaDataID>{2BEE3B67-3438-40A2-ADB8-A29CB5EE0250}</MetaDataID>
        public override string LocalTransactionUri
        {

            get
            {
                lock (this)
                {
                    if (InternalTransaction != null)
                        return InternalTransaction.LocalTransactionUri;
                    return _LocalTransactionUri;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{63327761-0A31-4252-BF92-5714A4E25A81}</MetaDataID>
        private string _GlobalTransactionUri;
        /// <summary>Define the identity of transaction. </summary>
        /// <MetaDataID>{77662675-182C-4FA7-8A67-66B55CE68E08}</MetaDataID>
        public override string GlobalTransactionUri
        {
            get
            {
                lock (this)
                {
                    if (InternalTransaction != null)
                        return InternalTransaction.GlobalTransactionUri;
                    return _GlobalTransactionUri;
                }
            }
        }

        /// <MetaDataID>{7B454E70-3FA4-451A-837B-70FB1E46B469}</MetaDataID>
        ~TransactionRunTime()
        {
            int rett = 0;

        }

        /// <MetaDataID>{e82e1082-fb4f-4785-863c-07b17ed3d5e9}</MetaDataID>
        string TransactionOwnerUserID;

        /// <MetaDataID>{DAC18307-FE5D-4505-9B82-7DC96B70440A}</MetaDataID>
        private bool MainTransaction = false;
        /// <MetaDataID>{745CADC6-2CC6-4B4C-951B-41DA3DD0123A}</MetaDataID>
        void InitTransaction()
        {
            TransactionOwnerUserID=OOAdvantech.Security.SecurityService.CurrentUser;
            MainTransaction = true;
            EnlistmentsController = new EnlistmentsController(this);
            EnlistmentsController.TransactionCompletted += new TransactionEndedEventHandler((this as ITransactionStatusNotification).OnTransactionCompletted);
            CreateNativeTransaction();
        }

        /// <MetaDataID>{9280425B-8DA8-48AD-9A1F-CE8B806965C8}</MetaDataID>
        public TransactionRunTime()
        {
            _LocalTransactionUri = System.Guid.NewGuid().ToString();
            InitTransaction();

        }

        /// <MetaDataID>{8B66AFDE-751C-43B3-BBBA-BF082FBF90E9}</MetaDataID>
        public TransactionRunTime(TransactionRunTime originTransaction)
        {
            _OriginTransaction = originTransaction;
            _LocalTransactionUri = System.Guid.NewGuid().ToString();
            InitTransaction();

            if (_OriginTransaction != null)
            {
                _OriginTransaction.AddNested(this);
            }


        }

        /// <MetaDataID>{0c604d52-5e8b-4703-8166-6c8b534b0c2d}</MetaDataID>
        private void AddNested(TransactionRunTime nestedTransaction)
        {
            if (_NestedTransactions == null)
                _NestedTransactions = new OOAdvantech.Collections.Generic.List<Transaction>();
            _NestedTransactions.Add(nestedTransaction);
            nestedTransaction.TransactionCompleted += new TransactionCompletedEventHandler(OnNestedTransactionTransactionCompletted);


        }

        /// <MetaDataID>{a1158c94-cbc1-4a7d-8919-6b127c883926}</MetaDataID>
        void OnNestedTransactionTransactionCompletted(Transaction transaction)
        {
            _NestedTransactions.Remove(GetTransactionRunTime(transaction));

        }
        

        /// <MetaDataID>{FCB7F8D2-02A3-4648-9490-3F0058AF5590}</MetaDataID>
        public TransactionRunTime(string globalTransactionUri, TransactionRunTime originTransaction)
        {
            

            _OriginTransaction = originTransaction;
            _LocalTransactionUri = System.Guid.NewGuid().ToString();
            _GlobalTransactionUri = globalTransactionUri;
            TransactionStatusNotification = new TransactionStatusNotifier(this);
            try
            {
                LocalTransactionCoordinator.TransactionImport(globalTransactionUri, TransactionStatusNotification);
            }
            catch (Exception error)
            {
                throw;
            }
#if !DeviceDotNet
            Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTimer), null, 4000, 4000);
#else
            Timer = new System.Timers.Timer(new System.Timers.TimerCallback(OnTimer),null, TimeSpan.FromSeconds(4));
            Timer.Start();

#endif

            if (_OriginTransaction != null)
                _OriginTransaction.AddNested(this);
        }


        /// <MetaDataID>{B85F5E55-E66E-4EB9-BAE1-1709C5B29802}</MetaDataID>
        public TransactionRunTime(string globalTransactionUri)
        {
            _LocalTransactionUri = System.Guid.NewGuid().ToString();
            _GlobalTransactionUri = globalTransactionUri;
            TransactionStatusNotification = new TransactionStatusNotifier(this);
            LocalTransactionCoordinator.TransactionImport(globalTransactionUri, TransactionStatusNotification);
#if !DeviceDotNet
            Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTimer), null, 4000, 4000);
#else
            Timer = new System.Timers.Timer(new System.Timers.TimerCallback(OnTimer), null, TimeSpan.FromSeconds(4));
            Timer.Start();

#endif
        }

        #if !DeviceDotNet

        /// <MetaDataID>{D3BCA62F-072A-45A8-9D5C-DA2DC8750EF9}</MetaDataID>
        public TransactionRunTime(System.Transactions.Transaction nativeTransaction)
        {
            _LocalTransactionUri = nativeTransaction.TransactionInformation.LocalIdentifier;
            _NativeTransaction = nativeTransaction;
            Dependent = true;
            _GlobalTransactionUri = LocalTransactionCoordinator.AttachToNativeTransaction(nativeTransaction);
            DistributedTransactions.Add(nativeTransaction.TransactionInformation.DistributedIdentifier.ToString().ToLower(), this);
            MainTransaction = true;
        }

        #endif
            /// <MetaDataID>{178BEF18-8C2E-4034-9DE3-85963834757B}</MetaDataID>
        private TransactionStatusNotifier TransactionStatusNotification = null;
        /// <MetaDataID>{BE16C7C7-4BF5-4348-8359-5C260A9227C8}</MetaDataID>
        private bool Dependent = false;






        /// <MetaDataID>{2B812A6F-DCEB-49D7-8604-C542EBE027DB}</MetaDataID>
        private TransactionContext CreateTransactionContext()
        {

            lock (this)
            {
                if (_TransactionContext != null)
                    return _TransactionContext;
#region Creates and initialize a new transaction context
#if !DeviceDotNet
                if (_NativeTransaction != null && _NativeTransaction.TransactionInformation.Status != System.Transactions.TransactionStatus.Active)
                    throw new OOAdvantech.Transactions.TransactionException("Native transaction is in invalid state to create transaction condext");
#endif

                _TransactionContext = new TransactionContext(this);

#if !DeviceDotNet
                if (_NativeTransaction != null)
                    _TransactionContext.AttachToNativeTransaction(_NativeTransaction);
#endif
#endregion
            }
#region Enlist resource objects state manager to transaction control
            if (GlobalTransactionUri == null)
                EnlistmentsController.EnlistObjectsStateManager(_TransactionContext);
            else
            {
                string globalTransactionUri = GlobalTransactionUri;
                LocalTransactionCoordinator.Enlist(_TransactionContext, ref globalTransactionUri);
                if (globalTransactionUri != GlobalTransactionUri)
                    _GlobalTransactionUri = globalTransactionUri;

                lock (this)
                {
                    if (!MainTransaction && !Dependent)
                    {
                        _NativeTransaction = _TransactionContext.NativeTransaction as System.Transactions.Transaction;
#if !DeviceDotNet
                        _NativeTransaction.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(OnNativeTransactionCompleted);
#endif
                    }
                }
            }
#endregion
            return _TransactionContext;
        }


        /// <MetaDataID>{DFFFD19A-D92F-491A-BA1C-46A3E05318E9}</MetaDataID>
        ///<summary>
        ///Enlist object in transaction. If transactioned object is out of process then propagate the enlistment 
        ///to transaction manager which hosted in object living process 
        ///</summary>
        ///<param name="transactionedObject">
        ///This parameter defines the enlisted object.
        ///</param>
        ///<param name="memberInfo">
        ///Sometimes the object enlisted in transction partially
        ///This parameter defines the member which will control the transaction 
        ///</param>
        public void EnlistObject(object transactionedObject, System.Reflection.MemberInfo memberInfo)
        {
            if (InternalTransaction != null)
            {
                (InternalTransaction as TransactionRunTime).EnlistObject(transactionedObject, memberInfo);
                return;
            }



#region Preconditions Chechk
            if (transactionedObject == null)
                return;
#endregion
#if !DeviceDotNet
#region Enlist object to transaction context of process which run the object. For remote object only
            if (transactionedObject is System.MarshalByRefObject)
            {
                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(transactionedObject as System.MarshalByRefObject))
                {
                    string ChannelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(transactionedObject as System.MarshalByRefObject);
                    TransactionManager transactionManager = Remoting.RemotingServices.CreateRemoteInstance(ChannelUri, typeof(TransactionManager).ToString(), typeof(TransactionManager).Assembly.FullName) as TransactionManager;
                    transactionManager.EnlistObjectOnActiveTransaction(transactionedObject, memberInfo);
                    return;
                }
            }
#endregion
#endif

#region Checks if the object is transactional. If it isn't abort transaction and throw exception
            if (!ObjectStateTransition.IsTransactional(transactionedObject.GetType()))
            {
                try
                {
                    Abort(null);
                }
                finally
                {
                    throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute isn't declared in type '" + transactionedObject.GetType().FullName + "'.");
                }
            }
#endregion


            TransactionContext.EnlistObject(transactionedObject, memberInfo);

        }



        /// <MetaDataID>{8BB16693-7545-4E79-B5A6-CC201AE4FBCA}</MetaDataID>
        void UpdateStatus(TransactionStatus status)
        {
            _Status = status;
        }


        /// <MetaDataID>{31D380B9-EED3-4591-AC9B-9BA987148B9C}</MetaDataID>
        public IEnlistmentsController EnlistmentsController;




        /// <MetaDataID>{5B8C12B3-0548-4858-97E3-DAC8BCC41818}</MetaDataID>
        public override void Abort(System.Exception exception)
        {
            if (InternalTransaction != null)
            {
                (InternalTransaction as TransactionRunTime).Abort(exception);
                return;
            }

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                if (Status != TransactionStatus.Aborted)
                {
                    if (_NestedTransactions != null)
                    {
                        foreach (TransactionRunTime nestedTransaction in _NestedTransactions.ToList())
                            nestedTransaction.Abort(exception);
                    }
                    if (_NativeTransaction != null)
                    {
                        if (AbortReasons == null)
                            AbortReasons = new OOAdvantech.Collections.Generic.List<System.Exception>();
                        AbortReasons.Add(exception);
                        //TODO πώς πέρνω το exception
#if !DeviceDotNet
                        if (_NativeTransaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Aborted)
                            return;
#endif
                        _NativeTransaction.Rollback(exception);
                        while (Status == TransactionStatus.Continue)
                        {
#if !DeviceDotNet
                            System.Threading.Thread.Sleep(10);
#else
                            System.Threading.Tasks.Task.Delay(10).Wait();
#endif
                        }

                    }
                    else
                    {
                        if (MainTransaction)
                            EnlistmentsController.AbortRequest(exception);
                        else
                        {
                            if (GlobalTransactionUri != null && GlobalTransactionUri.Trim().Length > 0)
                                LocalTransactionCoordinator.Abort(GlobalTransactionUri, exception);
                            {
                                if (AbortReasons == null)
                                    AbortReasons = new OOAdvantech.Collections.Generic.List<System.Exception>();
                                AbortReasons.Add(exception);
                            }

                        }
                    }
                }
                else
                {
                    if (GlobalTransactionUri != null && GlobalTransactionUri.Trim().Length > 0)
                        LocalTransactionCoordinator.Abort(GlobalTransactionUri, exception);
                    else
                    {
                        if (AbortReasons == null)
                            AbortReasons = new OOAdvantech.Collections.Generic.List<System.Exception>();
                        if (!AbortReasons.Contains(exception))
                            AbortReasons.Add(exception);
                    }

                }
                stateTransition.Consistent = true;
            }
        }


        public override bool HasChangesOnElistedObjects(bool checkOnlyPersistentClassInstances)
        {
            
            {
                if (InternalTransaction != null)
                {
                    return (InternalTransaction as TransactionRunTime).HasChangesOnElistedObjects(checkOnlyPersistentClassInstances);
                    
                }

                if (Status == TransactionStatus.Aborted)
                {
                    return false;
                }
                return EnlistmentsController.HasChangesOnElistedObjects(checkOnlyPersistentClassInstances);

            }
        }

        /// <MetaDataID>{805C404B-BD3A-4A5D-B037-D0ECEB879FC7}</MetaDataID>
        public void Commit()
        {
            try
            {
                if (InternalTransaction != null)
                {
                    (InternalTransaction as TransactionRunTime).Commit();
                    return;
                }

                if (Status == TransactionStatus.Aborted)
                {
                    if (_NativeTransaction != null)
                        (_NativeTransaction as System.Transactions.CommittableTransaction).Commit();
                    throw new OOAdvantech.Transactions.TransactionException("Transaction Aborted");
                }

                if (_NativeTransaction == null)
                    CreateNativeTransaction();

                ObjectsStateManagerStatus newStatus;
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    //string chanellUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(EnlistmentsController as MarshalByRefObject);
                    newStatus = EnlistmentsController.PrepareRequest();
                    stateTransition.Consistent = true;
                }


                if (newStatus == ObjectsStateManagerStatus.PrepareDone)
                {
                    if (_NativeTransaction is System.Transactions.CommittableTransaction)
                    {
                        (_NativeTransaction as System.Transactions.CommittableTransaction).Commit();
                        return;
                    }
                    else
                    {
                        if (Dependent)
                            return;

                        //newStatus = EnlistmentsController.PrepareRequest();
                        //if (newStatus == ObjectsStateManagerStatus.PrepareDone)
                        newStatus = EnlistmentsController.CommitRequest();
                        if (newStatus != ObjectsStateManagerStatus.CommitDone)
                        {
                            while (Status == TransactionStatus.Continue)
                            {
#if !DeviceDotNet
                            System.Threading.Thread.Sleep(10);
#else
                                System.Threading.Tasks.Task.Delay(10).Wait();
#endif
                            }
                        }

                    }
                    if (_TransactionContext != null)
                        _TransactionContext.Transaction = null;
                    return;
                }

                UpdateStatus(TransactionStatus.Aborted);
#if !DeviceDotNet
                if (_NativeTransaction is System.Transactions.CommittableTransaction && _NativeTransaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Active)
                    (_NativeTransaction as System.Transactions.CommittableTransaction).Rollback();
#endif

                throw new AbortException(AbortReasons, "Transaction aborted");
            }
            catch (System.Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error.StackTrace);
                throw;
            }
        }

        /// <MetaDataID>{B45D56CB-7E20-4C6B-B02B-A3FEF8518807}</MetaDataID>
        private void CreateNativeTransaction()
        {
#if !DeviceDotNet
            _NativeTransaction = new System.Transactions.CommittableTransaction(System.TimeSpan.FromMilliseconds(0));
            _NativeTransaction.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(OnNativeTransactionCompleted);
            System.Transactions.TransactionManager.DistributedTransactionStarted += new System.Transactions.TransactionStartedEventHandler(TransactionManager_DistributedTransactionStarted);
            if (EnlistmentsController != null)
                EnlistmentsController.AttachToNativeTransaction(_NativeTransaction);
#endif
        }

#if !DeviceDotNet
        /// <MetaDataID>{BDFB8C87-E768-42F8-96B3-8D7930F88449}</MetaDataID>
        private void TransactionManager_DistributedTransactionStarted(object sender, System.Transactions.TransactionEventArgs e)
        {

        }
#endif

#region ITransactionStatusNotification Members

        /// <MetaDataID>{A6996A99-735E-4C41-A530-F743F6EBEF72}</MetaDataID>
        public void OnTransactionCompletted(TransactionStatus transactionState)
        {
            lock (this)
            {
                if (_Status == transactionState)
                    return;
                _Status = transactionState;
            }
            TransactionCompletedEvent.Set();

            _Status = transactionState;
            if (Timer != null)
            {
                Timer.Dispose();
                Timer = null;
            }

            if (!OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(EnlistmentsController as MarshalByRefObject) && EnlistmentsController != null)
                EnlistmentsController.TransactionCompletted -= new TransactionEndedEventHandler((this as ITransactionStatusNotification).OnTransactionCompletted);
            else if (TransactionStatusNotification != null)
                TransactionStatusNotification.StopNotification();


            EnlistmentsController = null;
#if !DeviceDotNet
            if (GlobalTransactionUri != null && DistributedTransactions.ContainsKey(GlobalTransactionUri))
                DistributedTransactions.Remove(GlobalTransactionUri);
#endif


            if (_TransactionCompletted != null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    _TransactionCompletted(this);
                    stateTransition.Consistent = true;
                }


            }


        }

        /// <MetaDataID>{00637E9E-B031-466F-876A-E8AE66D65FF4}</MetaDataID>
        public void AttachToNativeTransaction(object nativeTransacion)
        {
#if !DeviceDotNet
            _NativeTransaction = nativeTransacion as System.Transactions.Transaction;
            _NativeTransaction.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(OnNativeTransactionCompleted);
#endif
        }
        private object _eventLock = new object();
        event OOAdvantech.Transactions.TransactionCompletedEventHandler _TransactionCompletted;
        public override event OOAdvantech.Transactions.TransactionCompletedEventHandler TransactionCompleted
        {
            add
            {
                if (Status == TransactionStatus.Aborted || Status == TransactionStatus.Committed)
                    throw new ObjectDisposedException("Transaction");
                if (InternalTransaction != null)
                {


                    InternalTransaction.TransactionCompleted += value;

                }
                else
                {
                    lock (_eventLock)
                    {
                        _TransactionCompletted -= value;
                        _TransactionCompletted += value;
                    }
                }

            }
            remove
            {
                if (InternalTransaction != null)
                    InternalTransaction.TransactionCompleted -= value;
                else
                {
                    lock (_eventLock)
                    {
                        _TransactionCompletted -= value;
                    }
                }
            }
        }
        event OOAdvantech.Transactions.TransactionDistributedEventHandler _TransactionDistributed;
        public override event OOAdvantech.Transactions.TransactionDistributedEventHandler TransactionDistributed
        {
            add
            { 
                if (Status == TransactionStatus.Aborted || Status == TransactionStatus.Committed)
                    throw new ObjectDisposedException("Transaction");
                _TransactionDistributed += value;
            }
            remove
            {
                _TransactionDistributed -= value;
            }
        }
#if !DeviceDotNet

        /// <MetaDataID>{04735EAF-99BE-431F-A7EF-9865B3581451}</MetaDataID>
        public void OnNativeTransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        {
            try
            {
                if (e.Transaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Committed)
                    OnTransactionCompletted(TransactionStatus.Committed);
                else if (e.Transaction.TransactionInformation.Status == System.Transactions.TransactionStatus.InDoubt)
                    OnTransactionCompletted(TransactionStatus.InDoubt);
                else
                    OnTransactionCompletted(TransactionStatus.Aborted);
            }
            catch
            {
            }
        }

#endif
#endregion



        /// <MetaDataID>{185c76f2-90c4-4e8a-91af-42395249bdbb}</MetaDataID>
        internal static TransactionRunTime GetTransactionRunTime(Transaction transaction)
        {

            if (transaction.InternalTransaction != null)
                return GetTransactionRunTime(transaction.InternalTransaction);
            else
                return transaction as TransactionRunTime;


        }

        /// <MetaDataID>{da0efdb1-e89e-4aef-9928-43a67b5fa6a4}</MetaDataID>
        public override bool IsNestedTransaction(Transaction transaction)
        {

            if (transaction.OriginTransaction == this)
                return true;
            else
             if (transaction.OriginTransaction == null)
                return false;
            else
                return IsNestedTransaction(transaction.OriginTransaction as TransactionRunTime);
        }
        // /// <MetaDataID>{0f33dd55-6207-40d4-812b-d82b4cd75767}</MetaDataID>
        //internal bool IsNestedTransaction(TransactionRunTime transaction)
        //{

        //    if (transaction.OriginTransaction == this)
        //        return true;
        //    else
        //        if (transaction.OriginTransaction == null)
        //            return false;
        //        else
        //            return IsNestedTransaction(transaction.OriginTransaction as TransactionRunTime);

        //}
        #region Excluded for .Net CompactFramework
#if !DeviceDotNet
        /// <MetaDataID>{fa8e6338-16e0-4a0a-be6e-0a0f6cde33b0}</MetaDataID>
        public TransactionRunTime(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            string marsalingStream = (String)info.GetValue("MarsalingStream", typeof(string));
            InternalTransaction = UnMarshal(marsalingStream);
            //InternalTransaction.TransactionCompletted += new TransactionCompletedEventHandler(OnTransactionCompletted);
        }
    

#endif
#if !PORTABLE
        #region ISerializable Members

        /// <MetaDataID>{a8d13994-4ff2-46d4-a334-f8a14a25f0de}</MetaDataID>
        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("MarsalingStream", Marshal());

        }

        #endregion
#endif
        #endregion


    }
}
