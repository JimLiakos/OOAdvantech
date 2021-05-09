
#if PocketPC
    using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
    using Collections = System.Collections;

    namespace OOAdvantech.Remoting
    {
        public interface IExtMarshalByRefObject
        {
        }
    }
#else
    using ReaderWriterLock = System.Threading.ReaderWriterLock;
#endif

namespace OOAdvantech.Transactions
{
	public enum ObjectsStateManagerStatus
	{
		OnAction=0,
        MakeDurableRequest,
        MakeDurableDone,
		PrepareRequest,
		PrepareDone,
		CommitRequest,
		CommitDone,
		AbortRequest,
		AbortDone
	}

	internal delegate void ObjectsStateManagerStatusChanged(object sender,ObjectsStateManagerStatus newState);
	

	/// <MetaDataID>{185ED953-25E3-4889-9612-4131037A2DE0}</MetaDataID>
	/// <summary>This class defines a collection of objects state manager enlistments. It has three states PrepareRequestDone, AbortRequestDone, CommitRequestDone. Change state when all enlistments go to new state. At this time raise corresponding event to new state.</summary>
	internal class EnlistmentsController :System.MarshalByRefObject,Remoting.IExtMarshalByRefObject,ObjectsStateManager
	{
       
		/// <MetaDataID>{BDEF2F8C-0C19-43D4-A015-A776D5D468E4}</MetaDataID>
		public System.Collections.ArrayList GetEnlistedObjectsStateManagers()
		{
			ReaderWriterLock.AcquireReaderLock(10000);
			try
			{
				
				Collections.ArrayList objectsStateManagers=new Collections.ArrayList();
				foreach(System.Collections.DictionaryEntry dictionaryEntry in Enlistments)
				{
					if(dictionaryEntry.Key is EnlistmentsController)
						objectsStateManagers.AddRange((dictionaryEntry.Key as EnlistmentsController).GetEnlistedObjectsStateManagers());
					else
                        objectsStateManagers.Add(dictionaryEntry.Key);
				}
				return objectsStateManagers;
			}
			finally
			{
				ReaderWriterLock.ReleaseLock();
			}

		}
        internal void DetatchEnlistments()
        {

            System.Threading.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                System.Transactions.Transaction nativeTransaction = NativeTransaction as System.Transactions.Transaction;
                if (nativeTransaction != null)
                    nativeTransaction.TransactionCompleted -= new System.Transactions.TransactionCompletedEventHandler(OnNativeTransactionEvent);
                foreach (System.Collections.DictionaryEntry entry in _Enlistments)
                {
                    TransactionEnlistment transactionEnlistment = entry.Value as TransactionEnlistment;
                    transactionEnlistment.ChangeState -= new ObjectsStateManagerStatusChanged(OnObjectsStateManagerChangeState);
                }
                _Enlistments.Clear();
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        public bool AbortIfInitiatorProcessExit()
        {
            try
            {
                try
                {
                    ReaderWriterLock.AcquireWriterLock(10000);
                }
                catch (System.Exception)
                {
                    return false;
                }
                if (InitiatorProcess.HasExited)
                {
                    AbortRequest();
                    if (Status == ObjectsStateManagerStatus.AbortDone)
                        return true;
                }
                return false;
            }
            finally
            {
                ReaderWriterLock.ReleaseLock();
            }
        }


#if DISTRIBUTED_TRANSACTIONS
		/// <MetaDataID>{75600996-4E40-42AA-AD68-91FB671F3215}</MetaDataID>
		void AttachToNativeTransaction()
		{
            System.Transactions.Transaction nativeTransaction = System.Transactions.Transaction.Current;
            nativeTransaction.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(OnNativeTransactionEvent);
            _NativeTransaction = nativeTransaction;
		}
#endif

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{0F0D7C13-706C-41DC-B6D9-CD4819CA8951}</MetaDataID>
		private object _NativeTransaction;
		/// <summary>Define the transaction of native transaction system in this case a COM+ transaction;</summary>
		/// <MetaDataID>{1A00F7E0-967D-485C-A4E3-F1B7D5E3157D}</MetaDataID>
		public object NativeTransaction
		{
			get
			{
				return _NativeTransaction;
			}
			set
			{
				_NativeTransaction=value;
			}
		}

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{EAE23DC6-9F76-464F-BD31-13153ED1921D}</MetaDataID>
		private System.Collections.Hashtable _Enlistments=new System.Collections.Hashtable(40);
		/// <MetaDataID>{C0F8E2E3-1051-4842-981B-509D5C32611F}</MetaDataID>
		public System.Collections.Hashtable Enlistments
		{
			get
			{
				
				lock(this)
				{
					return _Enlistments.Clone() as System.Collections.Hashtable;
				}
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{E2573762-E0AB-46B0-9B9A-1FC0A825E374}</MetaDataID>
        private string _GlobalTransactionUri;
		/// <MetaDataID>{C99A9D3B-FCE0-4848-9736-CD5D9C840EA9}</MetaDataID>
		/// <summary>Define the identity of transaction.</summary>
		public string GlobalTransactionUri
		{
			get
			{
				string tmpTransactionURI=null;
				lock(this)
				{
                    tmpTransactionURI = _GlobalTransactionUri;
				}
				return tmpTransactionURI;
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
				try
				{
					ReaderWriterLock.AcquireReaderLock(10000);
					return _Status;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}

			}
			set
			{
				try
				{
					ReaderWriterLock.AcquireWriterLock(10000);
					_Status=value;
				}
				finally
				{
					ReaderWriterLock.ReleaseWriterLock();
				}
			}
		}
	


		/// <MetaDataID>{A32EDC9A-DBE7-4D7C-9CEA-2F40231C76B5}</MetaDataID>
		protected ReaderWriterLock ReaderWriterLock=new ReaderWriterLock();
		/// <summary>Transaction manager is requesting that the objects state manager abort the transaction.</summary>
		/// <MetaDataID>{A1AFC2F5-A076-4156-B393-A6CBAC8DCD5E}</MetaDataID>
        public ObjectsStateManagerStatus AbortRequest()
		{
			try
			{
				ReaderWriterLock.AcquireWriterLock(10000);
				if(_Status==ObjectsStateManagerStatus.AbortRequest||_Status==ObjectsStateManagerStatus.AbortDone)
					return _Status;
				_Status=ObjectsStateManagerStatus.AbortRequest;
				//Zero can be in the case where a transaction started and there isn't enlistment.
				if(_Enlistments.Count==0)
				{
					ReaderWriterLock.ReleaseLock();
					OnObjectsStateManagerChangeState(this,ObjectsStateManagerStatus.AbortDone);
                    return _Status;
				}
				System.Collections.Hashtable enlistments=new System.Collections.Hashtable(_Enlistments.Count);
				foreach(System.Collections.DictionaryEntry CurrEntry in _Enlistments)
				{
					//We use try catch to ensure the call of stay live objects state managers. 
					//In transaction live some enlisted objects state managers actually remote objects state managers,
					//may be disconnect from transaction manager (for reasons of low quality network connections).
					try
					{
						enlistments.Add(CurrEntry.Key,CurrEntry.Value);
					}
					catch(System.Exception)
					{
					}
				}
				
				ReaderWriterLock.ReleaseLock();
				
				foreach(System.Collections.DictionaryEntry CurrEntry in enlistments)
				{
					//We use try catch to ensure the call of stay live objects state managers. 
					//In transaction live some enlisted objects state managers actually remote objects state managers,
					//may be disconnect from transaction manager (for reasons of low quality network connections).

					try
					{
						TransactionEnlistment transactionEnlistment=CurrEntry.Value as TransactionEnlistment;
						transactionEnlistment.AbortRequest();
					}
					catch(System.Exception)
					{
					}
				}
                while (Status == ObjectsStateManagerStatus.AbortRequest)
                {
                    System.Threading.Thread.Sleep(2);
                    if (TimeExpired())
                        break;
                }
			}
			finally
			{
				
				ReaderWriterLock.ReleaseLock();

			}
            return Status;

		}

        public ObjectsStateManagerStatus MakeDurableRequest()
        {
            try
			{
				ReaderWriterLock.AcquireWriterLock(10000);
				if(_Status!=ObjectsStateManagerStatus.OnAction)
					return _Status;
				_Status=ObjectsStateManagerStatus.MakeDurableRequest;

				//Zero can be in the case where a transaction started and there isn't enlistment.
				if(_Enlistments.Count==0)
				{
					ReaderWriterLock.ReleaseLock();
					OnObjectsStateManagerChangeState(this,ObjectsStateManagerStatus.MakeDurableDone);
                    return _Status;
				}
				System.Collections.Hashtable enlistments=new System.Collections.Hashtable(_Enlistments.Count);
				foreach(System.Collections.DictionaryEntry CurrEntry in _Enlistments)
				{
					//We use try catch to ensure the call of stay live objects state managers. 
					//In transaction live some enlisted objects state managers actually remote objects state managers,
					//may be disconnect from transaction manager (for reasons of low quality network connections).
					try
					{
						enlistments.Add(CurrEntry.Key,CurrEntry.Value);
					}
					catch(System.Exception)
					{
					}
				}
				ReaderWriterLock.ReleaseLock();
				foreach(System.Collections.DictionaryEntry CurrEntry in enlistments)
				{
					//We use try catch to ensure the call of stay live objects state managers. 
					//In transaction live some enlisted objects state managers actually remote objects state managers,
					//may be disconnect from transaction manager (for reasons of low quality network connections).
					try
					{
						TransactionEnlistment transactionEnlistment=CurrEntry.Value as TransactionEnlistment;
						transactionEnlistment.MakeDurableRequest();
					}
					catch(System.Exception errror)
					{
                        int rtt = 0;
					}
				}

                while (Status == ObjectsStateManagerStatus.MakeDurableRequest)
                {
                    System.Threading.Thread.Sleep(2);
                    if (TimeExpired())
                        AbortRequest();
                }
			}
			finally
			{
				ReaderWriterLock.ReleaseLock();
			}
            return Status;
        }
        public bool TimeExpired()
        {
            bool tt = false;
            return tt;
        }

		/// <summary>The transaction manager is requesting that the objects state manager prepare the transaction to commit. This is phase one of the two-phase commit protocol.</summary>
		/// <MetaDataID>{E7348574-A5AB-42CB-9014-01427CB71028}</MetaDataID>
        public ObjectsStateManagerStatus PrepareRequest()
		{
			try
			{
				ReaderWriterLock.AcquireWriterLock(10000);
				if(_Status!=ObjectsStateManagerStatus.OnAction)
					return _Status;
				_Status=ObjectsStateManagerStatus.PrepareRequest;

				//Zero can be in the case where a transaction started and there isn't enlistment.
				if(_Enlistments.Count==0)
				{
					ReaderWriterLock.ReleaseLock();
					OnObjectsStateManagerChangeState(this,ObjectsStateManagerStatus.PrepareDone);
                    return _Status;
				}
				System.Collections.Hashtable enlistments=new System.Collections.Hashtable(_Enlistments.Count);
				foreach(System.Collections.DictionaryEntry CurrEntry in _Enlistments)
				{
					//We use try catch to ensure the call of stay live objects state managers. 
					//In transaction live some enlisted objects state managers actually remote objects state managers,
					//may be disconnect from transaction manager (for reasons of low quality network connections).
					try
					{
						enlistments.Add(CurrEntry.Key,CurrEntry.Value);
					}
					catch(System.Exception)
					{
					}
				}
				ReaderWriterLock.ReleaseLock();
				foreach(System.Collections.DictionaryEntry CurrEntry in enlistments)
				{
					//We use try catch to ensure the call of stay live objects state managers. 
					//In transaction live some enlisted objects state managers actually remote objects state managers,
					//may be disconnect from transaction manager (for reasons of low quality network connections).
					try
					{
						TransactionEnlistment transactionEnlistment=CurrEntry.Value as TransactionEnlistment;
						transactionEnlistment.PrepareRequest();
					}
					catch(System.Exception errror)
					{
                        int rtt = 0;
					}
				}

                while (Status == ObjectsStateManagerStatus.PrepareRequest)
                {
                    System.Threading.Thread.Sleep(2);
                    if (TimeExpired())
                        AbortRequest();
                }

			}
			finally
			{
				ReaderWriterLock.ReleaseLock();
			}
            return Status;

		}
		/// <summary>The transaction manager is requesting that the objects state manager commit the transaction. This is phase two of the two-phase commit protocol.</summary>
		/// <MetaDataID>{7182AC35-D77B-4517-96FE-C3C24444E9C2}</MetaDataID>
        public ObjectsStateManagerStatus CommitRequest()
		{

			try
			{
				ReaderWriterLock.AcquireWriterLock(10000);
				_Status=ObjectsStateManagerStatus.CommitRequest;

				//Zero can be in the case where a transaction started and there isn't enlistment.
				if(_Enlistments.Count==0)
				{
					ReaderWriterLock.ReleaseLock();
					OnObjectsStateManagerChangeState(this,ObjectsStateManagerStatus.CommitDone);
					return _Status;
				}
				System.Collections.Hashtable enlistments=new System.Collections.Hashtable(_Enlistments.Count);
				foreach(System.Collections.DictionaryEntry CurrEntry in _Enlistments)
				{
					//We use try catch to ensure the call of stay live objects state managers. 
					//In transaction live some enlisted objects state managers actually remote objects state managers,
					//may be disconnect from transaction manager (for reasons of low quality network connections).
					try
					{
						enlistments.Add(CurrEntry.Key,CurrEntry.Value);
					}
					catch(System.Exception)
					{
					}
				}
				ReaderWriterLock.ReleaseLock();
				foreach(System.Collections.DictionaryEntry CurrEntry in enlistments)
				{
					//We use try catch to ensure the call of stay live objects state managers. 
					//In transaction live some enlisted objects state managers actually remote objects state managers,
					//may be disconnect from transaction manager (for reasons of low quality network connections).
					try
					{
						TransactionEnlistment transactionEnlistment=CurrEntry.Value as TransactionEnlistment;
						transactionEnlistment.CommitRequest();
					}
					catch(System.Exception)
					{
					}
				}

                while (Status == ObjectsStateManagerStatus.CommitRequest)
                {
                    System.Threading.Thread.Sleep(2);
                    if (TimeExpired())
                        break;
                }

			}
			finally
			{
				ReaderWriterLock.ReleaseLock();
			}
            return Status;
		}
		/// <MetaDataID>{7FE9E9C3-9342-45B9-B52F-26736BB55BE7}</MetaDataID>
		public event ObjectsStateManagerStatusChanged StatusChanged;
#if DISTRIBUTED_TRANSACTIONS
        /// <MetaDataID>{3230977D-5282-437B-B9A9-FB0586821513}</MetaDataID>
        public void OnNativeTransactionEvent(object sender, System.Transactions.TransactionEventArgs e)
        {
            
            try
            {
                System.Threading.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                ObjectsStateManagerStatus OldState = _Status;
                try
                {
                    #region Change state
                    if (e.Transaction.TransactionInformation.Status == System.Transactions.TransactionStatus.Committed)
                        _Status = ObjectsStateManagerStatus.CommitDone;
                    else
                        _Status = ObjectsStateManagerStatus.AbortDone;
                    #endregion

                    #region Release transaction enlistment
                    foreach (System.Collections.DictionaryEntry CurrEntry in _Enlistments)
                    {
                        TransactionEnlistment transactionEnlistment = CurrEntry.Value as TransactionEnlistment;
                        transactionEnlistment.ChangeState -= new ObjectsStateManagerStatusChanged(OnObjectsStateManagerChangeState);
                    }
                    _Enlistments.Clear();
                    MasterTransactionEnlistment = null;
                    #endregion
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

                #region Inform for new state
                if (StatusChanged != null && OldState != _Status)
                    StatusChanged(this, _Status);
                #endregion
            }
            catch (System.Exception Error)
            {
                int ll = Error.GetHashCode();
            }
        }
#endif
        /// <MetaDataID>{FDECACE5-4184-4BF5-AA10-2E53DD97DEBB}</MetaDataID>
		public void OnObjectsStateManagerChangeState(object sender,ObjectsStateManagerStatus newState)
		{
			try
			{
				ReaderWriterLock.AcquireWriterLock(10000);
				if(_Status==ObjectsStateManagerStatus.CommitDone||_Status==ObjectsStateManagerStatus.AbortDone)
					return;
				ObjectsStateManagerStatus OldState=_Status;
                if(newState==ObjectsStateManagerStatus.PrepareDone||newState==ObjectsStateManagerStatus.CommitDone||newState==ObjectsStateManagerStatus.AbortDone)
				{
					if((_Status==ObjectsStateManagerStatus.OnAction||_Status==ObjectsStateManagerStatus.PrepareRequest||
						_Status==ObjectsStateManagerStatus.PrepareDone)&&newState==ObjectsStateManagerStatus.AbortDone)
					{
						if(ReaderWriterLock.IsWriterLockHeld)
							ReaderWriterLock.ReleaseWriterLock();
						try
						{
                            TransactionManager.CurrentTransactionManager.Unmarshal(GlobalTransactionUri).Abort(null);
						}
						catch(System.Exception)
						{
						}
						return;
					}
					if(_Status==ObjectsStateManagerStatus.OnAction&&newState==ObjectsStateManagerStatus.PrepareDone)
					{
						if(ReaderWriterLock.IsWriterLockHeld)
							ReaderWriterLock.ReleaseWriterLock();
						try
						{
                            TransactionManager.CurrentTransactionManager.Unmarshal(GlobalTransactionUri).Abort(null);
						}
						catch(System.Exception)
						{
						}
						return;
					}

					foreach(System.Collections.DictionaryEntry CurrEntry in _Enlistments)
					{
						TransactionEnlistment transactionEnlistment=CurrEntry.Value as TransactionEnlistment;
						if(transactionEnlistment.State!=newState)
							return;
					}
					_Status=newState;
				}
				else
					return;

				if(ReaderWriterLock.IsWriterLockHeld)
					ReaderWriterLock.ReleaseWriterLock();

				if(StatusChanged!=null&&OldState!=_Status)
				{
					try
					{
						StatusChanged(this,_Status);
					}
					catch(System.Exception)
					{
					}
				}
				if(MasterTransactionEnlistment!=null)
				{
					try
					{
						switch(_Status)
						{
							case ObjectsStateManagerStatus.CommitDone:
							{
								MasterTransactionEnlistment.CommitRequestDone();
								MasterTransactionEnlistment=null;
								break;
							}

							case ObjectsStateManagerStatus.AbortDone:
							{
								MasterTransactionEnlistment.AbortRequestDone();
								MasterTransactionEnlistment=null;
								break;
							}

							case ObjectsStateManagerStatus.PrepareDone:
							{
								MasterTransactionEnlistment.PrepareRequestDone();
								break;
							}
						}
					}
					catch(System.Exception)
					{
					}
				}
				ReaderWriterLock.AcquireWriterLock(10000);
				if(_Status==ObjectsStateManagerStatus.CommitDone||_Status==ObjectsStateManagerStatus.AbortDone)
				{
					foreach(System.Collections.DictionaryEntry CurrEntry in _Enlistments)
					{
						TransactionEnlistment transactionEnlistment=CurrEntry.Value as TransactionEnlistment;
						transactionEnlistment.ChangeState-=new ObjectsStateManagerStatusChanged(OnObjectsStateManagerChangeState);
					}
					_Enlistments.Clear();
				}
				ReaderWriterLock.ReleaseWriterLock();

			}
			finally
			{
				if(ReaderWriterLock.IsWriterLockHeld)
					ReaderWriterLock.ReleaseWriterLock();
			}
 		}

		/// <param name="objectsStateManager">The objects state manager must be not null.</param>
		/// <MetaDataID>{D3758C67-B2EF-4D53-BAA3-FE894EB27C04}</MetaDataID>
		public ITransactionEnlistment EnlistObjectsStateManager(ObjectsStateManager objectsStateManager)
		{
			try
			{
				ReaderWriterLock.AcquireReaderLock(10000);

				if(objectsStateManager==null)
					throw new System.Exception("The objects state manager must be not null.");
				if(_Enlistments.Contains(objectsStateManager))
					return _Enlistments[objectsStateManager] as ITransactionEnlistment;

				TransactionEnlistment TransactionEnlistment=new TransactionEnlistment(objectsStateManager,GlobalTransactionUri);
				TransactionEnlistment.ChangeState+=new ObjectsStateManagerStatusChanged(this.OnObjectsStateManagerChangeState);
				if(ReaderWriterLock.IsReaderLockHeld)
					ReaderWriterLock.ReleaseReaderLock();

				if(Status==ObjectsStateManagerStatus.PrepareRequest)
					TransactionEnlistment.PrepareRequest();
				ReaderWriterLock.AcquireWriterLock(10000);
				_Enlistments.Add(objectsStateManager,TransactionEnlistment);
				return TransactionEnlistment;
			}
			finally
			{
				if(ReaderWriterLock.IsWriterLockHeld)
					ReaderWriterLock.ReleaseWriterLock();
				if(ReaderWriterLock.IsReaderLockHeld)
					ReaderWriterLock.ReleaseReaderLock();
			}
		}

		/// <summary>Initializes a new instance of the OOAdvantech.Transactions.EnlistmentsController</summary>
		/// <param name="transactionUri">Define the identity of the transaction.</param>
		/// <MetaDataID>{E218462B-34CB-4BB7-8252-74BB0EB65A60}</MetaDataID>
        public EnlistmentsController(Transaction transaction)
		{
            AttachToNativeTransaction();
		}
		/// <MetaDataID>{186EE79D-AD9E-44E3-8857-114144404AB9}</MetaDataID>
		private ITransactionEnlistment MasterTransactionEnlistment=null;
		/// <summary>This constructor initializes a new instance of the OOAdvantech.Transactions.EnlistmentsController as surrogate of all objects state managers that act on this machine. This class act as surrogate in case where the transaction has begins in other machine. When an transaction began in one machine and propagated to other then the EnlistmentsController act like objects state manager, a objects state manager that surrogate the objects state managers in the local machine to the machine that began the transaction.</summary>
		/// <MetaDataID>{D6F37C02-AC3B-48F2-9798-5532A7755E76}</MetaDataID>
        public EnlistmentsController(string globalTransactionUri, TransactionCoordinator transactionInitiator)
		{
            MasterTransactionEnlistment = transactionInitiator.Enlist(this, globalTransactionUri);
            _GlobalTransactionUri = globalTransactionUri;
		}
        protected System.Diagnostics.Process InitiatorProcess;

        public EnlistmentsController(string globalTransactionUri, int initiatorProcessID)
        {
            InitiatorProcess = System.Diagnostics.Process.GetProcessById(initiatorProcessID);
            _GlobalTransactionUri = globalTransactionUri;
        }

		/// <MetaDataID>{4FD5AF6E-68E2-4E30-B583-DF28607AAE5D}</MetaDataID>
		~EnlistmentsController()
		{
			int hh=0;
		}



        //#region ObjectsStateManager Members

        //void ObjectsStateManager.AbortRequest()
        //{
        //    AbortRequest();
        //}

        //void ObjectsStateManager.CommitRequest()
        //{
        //    CommitRequest();
        //}

        //void ObjectsStateManager.PrepareRequest()
        //{
        //    PrepareRequest();
        //}

        //void ObjectsStateManager.MakeDurableRequest()
        //{
        //    MakeDurableRequest();
        //}

        //#endregion


        #region ObjectsStateManager Members

        void ObjectsStateManager.AbortRequest(ITransactionEnlistment transactionEnlistment)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        void ObjectsStateManager.CommitRequest(ITransactionEnlistment transactionEnlistment)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        void ObjectsStateManager.PrepareRequest(ITransactionEnlistment transactionEnlistment)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        void ObjectsStateManager.MakeDurableRequest(ITransactionEnlistment transactionEnlistment)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
