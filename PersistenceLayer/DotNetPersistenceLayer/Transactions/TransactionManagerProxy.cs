//using System.Runtime.Remoting.Contexts;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Channels.Tcp;
//using System.Runtime.Remoting.Proxies;
//using System.Runtime.Remoting.Messaging;

namespace OOAdvantech.Transactions
{
    #if PocketPC
    using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
    using LockCookie = OOAdvantech.Synchronization.LockCookie;
    using Collections = System.Collections;

    
    #else
        using LockCookie = System.Threading.LockCookie; 
        
        using ReaderWriterLock = System.Threading.ReaderWriterLock;
#endif

    /// <MetaDataID>{9B3CACB7-01C5-40EC-9324-31B3FAF5ACB0}</MetaDataID>
    internal class TransactionManagerRunTime : TransactionManager
    {

        /// <MetaDataID>{59E32D31-0230-4CDD-B388-483221E34B67}</MetaDataID>
        public override string Marshal(Transaction transaction)
        {
            if (MonoStateTransactionManagerProxy != this)
                return MonoStateTransactionManagerProxy.Marshal(transaction);
            if (transaction == null)
                return null;
            return (transaction as TransactionRunTime).GlobalTransactionUri;
        }
        /// <MetaDataID>{F98282F1-0C6D-4A84-BF5C-4D4B66119C23}</MetaDataID>
        public override Transaction Unmarshal(string globalTransactionUri)
        {
            if (MonoStateTransactionManagerProxy != this)
                return MonoStateTransactionManagerProxy.Unmarshal(globalTransactionUri);
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                TransactionRunTime transaction = Transactions[globalTransactionUri];
                if (transaction != null)
                    return transaction;
                foreach (Transaction endedTransaction in transactionHistory)
                {
                    if ((endedTransaction as TransactionRunTime).GlobalTransactionUri == globalTransactionUri)
                        throw new System.Exception("Transction with uri '" + globalTransactionUri + "' " + endedTransaction.Status.ToString() + ".");
                }
                transaction = new TransactionRunTime(globalTransactionUri);
                Transactions[transaction.LocalTransactionUri] = transaction;
                Transactions[transaction.GlobalTransactionUri] = transaction;
                //transaction.TransactionManager = this;
                return transaction;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
 
        }
        /// <MetaDataID>{408FD9C1-CBEC-477D-8387-354840E99A71}</MetaDataID>
        public void Abort(Transaction transaction, System.Exception exception)
        {
            if (MonoStateTransactionManagerProxy != this)
            {
                MonoStateTransactionManagerProxy.Abort(transaction, exception);
                return;
            }
            try
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Transaction abort");
                    ServerTransactionManager.Abort((transaction as TransactionRunTime).LocalTransactionUri, exception);
                }
                catch (System.Exception Error)
                {
                    AttachToServerTransactionManager();
                    ServerTransactionManager.Abort((transaction as TransactionRunTime).LocalTransactionUri, exception);
                }
            }
            finally
            {
                if (ReaderWriterLock.IsReaderLockHeld)
                    ReaderWriterLock.ReleaseReaderLock();
                if (ReaderWriterLock.IsWriterLockHeld)
                    ReaderWriterLock.ReleaseWriterLock();
            }


        }
        /// <MetaDataID>{A8B60991-3AB9-4A88-BA10-F53D668DA097}</MetaDataID>
        public void Commit(Transaction transaction)
        {
            if (MonoStateTransactionManagerProxy != this)
            {
                MonoStateTransactionManagerProxy.Commit(transaction);
                return;
            }

            TransactionRunTime transactionRunTime = TransactionStateContollers[transaction];
            transactionRunTime.Commit();
            return;

            try
            {
              // ServerTransactionManager.Commit((transaction as TransactionRunTime).GlobalTransactionUri);
            }
            catch (System.Exception Error)
            {
                //Error Prone απο πλευράς remmoting 
                //θα πρέπει να εξασφαλιστή ότι άν έχω πρόβλημα επικοινωνίας θα πέρνω 
                //error επικοινωνίας καί όχι άλλα ντάλα
                //απο πλευράς 
                string DeclaringType = " ";
#if DISTRIBUTED_TRANSACTIONS
                if (Error.TargetSite != null)
                    DeclaringType = Error.TargetSite.DeclaringType.FullName;
#endif
                if (DeclaringType.IndexOf("OOAdvantech.Transactions") == -1)
                {
                    AttachToServerTransactionManager();
                  //  ServerTransactionManager.Commit((transaction as TransactionRunTime).GlobalTransactionUri);
                }
                else
                    throw Error;
            }
        }
        /// <MetaDataID>{788E4E83-568B-48A1-8E67-4CC204EA900E}</MetaDataID>
        public void EnlistObjectOnActiveTransaction(object TransactionedObject)
        {
            Transaction transaction = OOAdvantech.Transactions.SystemStateTransition.Transaction;
            #region Preconditions Chechk
            if (TransactionedObject == null)
                return;
            if (transaction == null)
                throw new System.Exception("There isn't assigned transaction to th scoop");
            #endregion

            #region Mono state class
            if (MonoStateTransactionManagerProxy != this)
            {
                MonoStateTransactionManagerProxy.EnlistObjectOnActiveTransaction(TransactionedObject);
                return;
            }
            #endregion

            #region Enlist object to transaction context of process which run the object. For remote object only
            if (TransactionedObject is System.MarshalByRefObject)
            {
                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(TransactionedObject as System.MarshalByRefObject))
                {
                    string ChannelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(TransactionedObject as System.MarshalByRefObject);
                    TransactionManagerRunTime transactionManagerProxy = Remoting.RemotingServices.CreateInstance(ChannelUri, typeof(TransactionManagerRunTime).ToString()) as TransactionManagerRunTime;
                    transactionManagerProxy.EnlistObjectOnActiveTransaction(TransactionedObject);
                    return;
                }
            }
            #endregion

            #region Checks if the object is transactional. If it isn't abort transaction and throw exception
            if (!ExtensionProperties.IsTransactional(TransactionedObject.GetType()))
            {
                try
                {
                    transaction.Abort(null);
                }
                finally
                {
                    throw new System.Exception("The Transaction attribute isn't declared in type '" + TransactionedObject.GetType().FullName + "'.");
                }
            }
            #endregion

            TransactionContext transactionContext = null;

            #region Get transaction context for the current transaction if exist.

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                transactionContext = TransactionContexts[transaction] as TransactionContext;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            #endregion

            if (transactionContext != null)
            {
                transactionContext.EnlistObject(TransactionedObject);
            }
            else
            {
                #region Creates and initialize a new transaction context


                LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    transactionContext = new TransactionContext(transaction);
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
                #endregion

                #region Enlist resource objects state manager to transaction control
                if (transaction.GlobalTransactionUri == null)
                {
                    TransactionStateContollers[transaction].EnlistmentsController.EnlistObjectsStateManager(transactionContext);
                }
                else
                    throw new System.NotSupportedException();
                #endregion

                transactionContext.EnlistObject(TransactionedObject);
            }
        }


        /// <MetaDataID>{D985FFBA-8890-4FB0-9982-524524FA8E81}</MetaDataID>
        private System.Collections.Generic.Dictionary<string, TransactionRunTime> Transactions;

        /// <MetaDataID>{3F9D39BC-5A28-4AD4-8CF6-DE01E2F77D57}</MetaDataID>
        private System.Collections.Generic.Dictionary<Transaction, TransactionRunTime> TransactionStateContollers;



        /// <MetaDataID>{647347CD-8828-463A-99CD-839261F08454}</MetaDataID>
        internal void OnTransactionComplete(string transactionUri, TransactionStatus transactionState)
		{

            LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                Transaction transaction = Transactions[transactionUri];
                if (TransactionContexts.ContainsKey(transaction))
                {
                    TransactionContext transactionContext = TransactionContexts[transaction] ;
                    //(transactionContext.Transaction as TransactionRunTime)._State = transactionState;
                    transactionHistory.Enqueue(transactionContext.Transaction);
                    if (transactionHistory.Count > 90)
                        transactionHistory.Dequeue();

                }
                TransactionContexts.Remove(transaction);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

		}


        /// <MetaDataID>{CF0B7AEE-0AE5-423B-91BB-F50307804142}</MetaDataID>
		System.Collections.ArrayList GetTransactionContexts( Transactions.Transaction transaction,TransactionCoordinator serverTransactionManager)
		{
			Collections.ArrayList TransactionContexts=new Collections.ArrayList(); 
			System.Collections.ArrayList  objectsStateManagers=null;
			try
			{
                //if((transaction as TransactionRunTime).TransactionOwner!=null)
                //    objectsStateManagers=(transaction as TransactionRunTime).TransactionOwner.GetEnlistedObjectsStateManagers((transaction as TransactionRunTime).LocalTransactionUri);
                //else
                //    objectsStateManagers = serverTransactionManager.GetEnlistedObjectsStateManagers((transaction as TransactionRunTime).LocalTransactionUri);
			}
			catch(System.Exception Error)
			{
				AttachToServerTransactionManager();
                objectsStateManagers = serverTransactionManager.GetEnlistedObjectsStateManagers((transaction as TransactionRunTime).LocalTransactionUri);
			}
			foreach(ObjectsStateManager objectsStateManager in objectsStateManagers)
			{
				if(objectsStateManager is EnlistmentsController)
				{
					foreach(ObjectsStateManager enlistedObjectsStateManager in 	(objectsStateManager as EnlistmentsController).GetEnlistedObjectsStateManagers())
					{
						if(enlistedObjectsStateManager is ITransactionContext)
							TransactionContexts.Add(enlistedObjectsStateManager);
						else
							throw new System.Exception("ObjectsStateManager as surrogate can represent only objectsStateManager as transaction context");

					}
				}
				if(objectsStateManager is ITransactionContext)
                    TransactionContexts.Add(objectsStateManager);
			}
			return TransactionContexts;
		}
        /// <MetaDataID>{C4C6F965-CAEE-41B0-BB8E-48AA1BF26EE7}</MetaDataID>
		public System.Collections.ArrayList GetTransactionContexts(Transactions.Transaction transaction)
		{
			if(MonoStateTransactionManagerProxy!=this)
				return MonoStateTransactionManagerProxy.GetTransactionContexts(transaction);
			return GetTransactionContexts(transaction, ServerTransactionManager);
		}

        public override ITransactionContext GetTransactionContext(Transaction transaction)
        {
            return (transaction as TransactionRunTime).TransactionContext;
        }



		/// <MetaDataID>{CAFEA32E-763B-4023-A6C8-ABC065424F06}</MetaDataID>
        private ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();
		
		/// <MetaDataID>{F026C770-4393-4C3B-BB0D-B0C0627A64F1}</MetaDataID>
        private TransactionManagerRunTime MonoStateTransactionManagerProxy;
        /// <MetaDataID>{4BF167E6-F378-4B17-A046-E3B1394C794F}</MetaDataID>
        public TransactionManagerRunTime()
		{
			if(MonoStateTransactionManagerProxy!=null)
				return;
			 if(MonoStateTransactionManagerProxy==null)
				 MonoStateTransactionManagerProxy=this;		
			
			AttachToServerTransactionManager();
            Transactions = new System.Collections.Generic.Dictionary<string, TransactionRunTime>();
            TransactionStateContollers=new System.Collections.Generic.Dictionary<Transaction,TransactionRunTime>();
		}

#if DISTRIBUTED_TRANSACTIONS
        /// <MetaDataID>{1D614D1D-671B-4204-9DA5-A067A1B21C25}</MetaDataID>
		void AttachToServerTransactionManager()
		{
            //try
            //{
            //    ReaderWriterLock.AcquireWriterLock(10000);
            //    ServerTransactionManager=Remoting.RemotingServices.CreateInstance("tcp://"+System.Net.Dns.GetHostName()+":4000",typeof(TransactionCoordinator).ToString()) as TransactionCoordinator;
            //}
            //finally
            //{
            //    ReaderWriterLock.ReleaseWriterLock();
            //}
        }
#endif

        /// <MetaDataID>{7BD92389-F3EC-444A-90E4-A592D537578C}</MetaDataID>
        private TransactionCoordinator ServerTransactionManager;
        /// <MetaDataID>{E19758E2-3373-49AD-9681-231D0C89F8E7}</MetaDataID>
		public override Transaction BeginTransaction()
		{
			if(MonoStateTransactionManagerProxy!=this)
				return MonoStateTransactionManagerProxy.BeginTransaction();


            int processId = 0;
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            if (process == null)
                throw new System.Exception("System can't retrieve process data");
            processId = process.Id;
			TransactionRunTime  transaction=new TransactionRunTime();
            //TransactionRunTime transactionStateContoller = new TransactionRunTime(transaction.LocalTransactionUri, processId);
            //this.TransactionStateContollers.Add(transaction, transactionStateContoller);
            this.Transactions.Add(transaction.LocalTransactionUri, transaction);
			return transaction;
		}






        /// <MetaDataID>{D981FB75-C265-40BA-A45D-B334F88CD7CD}</MetaDataID>
        private System.Collections.Queue transactionHistory = new System.Collections.Queue(100);
		
		
		/// <MetaDataID>{A905EAF2-4E83-4297-BCAE-6E81E74649B3}</MetaDataID>
        private System.Collections.Generic.Dictionary<Transaction, TransactionContext> TransactionContexts=new System.Collections.Generic.Dictionary<Transaction,TransactionContext>();
	}
}

