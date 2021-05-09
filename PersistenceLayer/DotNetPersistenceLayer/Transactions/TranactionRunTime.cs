namespace OOAdvantech.Transactions
{
#if PocketPC
    using Collections = System.Collections;
    using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
#else

    using ReaderWriterLock = System.Threading.ReaderWriterLock;
#endif


    /// <MetaDataID>{0FAA01BA-03A8-4B10-8366-2DB534A3A764}</MetaDataID>
    /// <summary>This class defines the core instance of transaction, control the state of transaction keep all enlistment of object state managers through enlistmentsController member. </summary>
    internal class TransactionRunTime
	{
        /// <MetaDataID>{33A01E40-1A08-4F65-85EF-71E7152DD16A}</MetaDataID>
        /// <summary>Enlistment controller keeps all enlistment of object state managers in transaction and controls theirs state. </summary>

        public EnlistmentsController EnlistmentsController;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{96A6698A-3390-454E-A3A0-886DB4BC4E6B}</MetaDataID>
        private string _TransactionUri;
        /// <summary>Define the identity of transaction. </summary>
        /// <MetaDataID>{2DE5FFFA-6375-4CCA-BAC9-DB15BE273737}</MetaDataID>
        public string TransactionUri
		{
			get
			{
				return _TransactionUri;
			}
		}
        /// <MetaDataID>{A287A3E9-AA34-4363-9F4C-B2D2A807C567}</MetaDataID>
        private Collections.ArrayList AbortReasons = new Collections.ArrayList();

        /// <MetaDataID>{7BF90A18-CF2E-4343-8076-70376CE68941}</MetaDataID>
        /// <summary>This member used for the thread safety. </summary>

        private ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();
        /// <summary>Aborts the transaction. </summary>
        /// <MetaDataID>{71C3CE68-B60 3-4B43-8B0F-54BB7A978A8D}</MetaDataID>
        public void Abort(object reason)
		{
			if(reason!=null)
				AbortReasons.Add(reason);
			try
			{
				ReaderWriterLock.AcquireReaderLock(20000);
				
				if(NativeTransaction!=null)
				{
					try
					{
						//Υπάρχει περίπτωση ένας objects state manager να πάει κατευθίαν από Prepare request στην 
                        //κατάσταση aborte done όταν κάτι συμβεί στο prepare; 
#if DISTRIBUTED_TRANSACTIONS
                        NativeTransaction.Rollback();
#endif
					}
					catch(System.Exception Error) 
					{
						int hh=Error.GetHashCode();
						ReaderWriterLock.ReleaseReaderLock();
						if(EnlistmentsController.State==ObjectsStateManagerState.CommitRequest||
							EnlistmentsController.State==ObjectsStateManagerState.CommitDone)
						{

							// Error Prone πρέπει να γυρίσει δικόμου error και όχι του native transaction system
							throw Error;
						}

					}
					if(EnlistmentsController.State==ObjectsStateManagerState.AbortRequest||
						EnlistmentsController.State==ObjectsStateManagerState.AbortDone)
					{
						return;
					}

					EnlistmentsController.State=ObjectsStateManagerState.AbortRequest;
					ReaderWriterLock.ReleaseLock();
				}
				else
				{
					if(EnlistmentsController.State==ObjectsStateManagerState.OnAction)
					{
						EnlistmentsController.AbortRequest();
						ReaderWriterLock.ReleaseLock();
						while(EnlistmentsController.State!=ObjectsStateManagerState.AbortDone)
						{
							if(TimeExpired())
								break;
							System.Threading.Thread.Sleep(2);
						}

					}
					else
						EnlistmentsController.AbortRequest();
				}
			}
			finally
			{
				_TransactionEnd=true;
				ReaderWriterLock.ReleaseLock();
			}

		}
        /// <MetaDataID>{B0B89B23-5A5B-42CC-93BC-9A2952520675}</MetaDataID>
		public bool TimeExpired()
		{
			bool tt=false;
			return tt;
        }
#if DISTRIBUTED_TRANSACTIONS
        /// <MetaDataID>{18F81C8C-DFE5-457B-AD5A-622E806DCB83}</MetaDataID>
        /// <summary>Define the transaction of native transaction. 
        /// Transaction can work stand alone or as wrapper of a native transaction system. </summary>
        private System.Transactions.CommittableTransaction NativeTransaction;
        /// <MetaDataID>{9EDEB3DB-3FBF-42A6-AD6D-994D44817ED5}</MetaDataID>
		public void CreateNativeTransactionAndAttatch()
		{
			if(NativeTransaction!=null)
				return;
            System.Transactions.CommittableTransaction nativeTransaction = null;
			try
			{
				//Error prone είναι αυθαίρετα τα 120000 miliseconds
				//προσοχή CreateNativeTransactionAndAttatch καλείται στο commit άλλα και κατα την διάρκεια ενός transaction.

                System.Transactions.TransactionOptions transactionOptions = new System.Transactions.TransactionOptions();
                transactionOptions.Timeout = new System.TimeSpan (0, 0, 0, 0, 1120000);
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead;
                nativeTransaction= new System.Transactions.CommittableTransaction(transactionOptions);
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(nativeTransaction as System.Transactions.Transaction))
                {
                    EnlistmentsController.AttachToNativeTransaction();
                    transactionScope.Complete(); 
                }
			}
			catch(System.Exception Error)
			{
				Abort(null);
				try
				{
                    if (nativeTransaction != null)
                        nativeTransaction.Rollback(Error);
				}
				catch(System.Exception)
				{
				}
				throw new Exception(AbortReasons, "Transaction Aborted",Error);
			}
			ReaderWriterLock.AcquireWriterLock(10000);
			NativeTransaction=nativeTransaction;
			ReaderWriterLock.ReleaseLock();
        }
#endif
        /// <summary>Commits the transaction. </summary>
        /// <MetaDataID>{B81D1B33-EC77-4A4B-90D4-CAC14D00F6AE}</MetaDataID>
		public void Commit()
		{

			try
			{
				if(NativeTransaction==null)
					CreateNativeTransactionAndAttatch();

				//System.Threading.Thread.Sleep(100);
				//ReaderWriterLock.AcquireWriterLock(20000);
				//System.Threading.Thread.Sleep(20000);
				if(EnlistmentsController.State==ObjectsStateManagerState.OnAction)
				{
					
					EnlistmentsController.PrepareRequest();
                    

					//TransactionEnlistments.Enlistments

					while(EnlistmentsController.State==ObjectsStateManagerState.PrepareRequest)
					{
						System.Threading.Thread.Sleep(2);
						if(TimeExpired())
							EnlistmentsController.AbortRequest();
					}
					if(EnlistmentsController.State==ObjectsStateManagerState.PrepareDone)
					{
                        if (NativeTransaction != null)
                            NativeTransaction.Commit();
						while(EnlistmentsController.State!=ObjectsStateManagerState.CommitDone)
						{
							if(TimeExpired())
								break;
							System.Threading.Thread.Sleep(2);
						}
						return;
					}
					else
						ReaderWriterLock.ReleaseLock();

					int DelayCount=250;//delay 500 ms for abort reasons
					ReaderWriterLock.AcquireReaderLock(10000);
					int reasonsCount=AbortReasons.Count;
					ReaderWriterLock.ReleaseReaderLock();

					while(EnlistmentsController.State!=ObjectsStateManagerState.AbortDone||(reasonsCount<=0&&DelayCount>0))
					{
						ReaderWriterLock.AcquireReaderLock(10000);
						reasonsCount=AbortReasons.Count;
						ReaderWriterLock.ReleaseReaderLock();

						DelayCount--;
						System.Threading.Thread.Sleep(2);
						if(TimeExpired())
							break;
					}
					throw new Exception(AbortReasons,"Transaction Aborted");
				}
				else
				{
					ReaderWriterLock.ReleaseLock();
					while(EnlistmentsController.State!=ObjectsStateManagerState.AbortDone&&EnlistmentsController.State!=ObjectsStateManagerState.CommitDone)
					{
						System.Threading.Thread.Sleep(2);
						if(TimeExpired())
							break;
					}
					if(EnlistmentsController.State==ObjectsStateManagerState.AbortDone)
						throw new Exception(AbortReasons,"Transaction Aborted");

				}
			}
			finally
			{
				_TransactionEnd=true;
				ReaderWriterLock.ReleaseLock();

			}
	
		
		}
        /// <MetaDataID>{C06A4DFF-FFCD-4076-83F4-ADB131375B90}</MetaDataID>
        /// <summary>Define the process that start the transaction. </summary>
        public System.Diagnostics.Process InitiatorProcess = null;
        /// <summary>Initializes a new instance of the TransactionRunTime class. </summary>
        /// <param name="transactionUri">Define the identity of new transaction. </param>
        /// <param name="initiatorProcessID">Define the identity of process that begins the transaction. </param>
        /// <MetaDataID>{99FB31E9-B522-4D91-BCED-F2D324CB5B85}</MetaDataID>
        public TransactionRunTime(string transactionUri, int initiatorProcessID)
		{
			//Error Prone Can throw exception if it can't attach to the process;
			InitiatorProcess=System.Diagnostics.Process.GetProcessById(initiatorProcessID);
			_TransactionUri=transactionUri;
			// EnlistmentsController= new EnlistmentsController(_TransactionUri);
			
		}
        /// <MetaDataID>{76FEC97F-C89E-4C54-9524-3589ADD3C6BC}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private bool _TransactionEnd = false;
        /// <MetaDataID>{04D295F2-7278-4005-8AB4-312B43EB6492}</MetaDataID>
        public bool TransactionEnd
		{
			
			get
			{
				try
				{
					ReaderWriterLock.AcquireReaderLock(10000);
					return _TransactionEnd;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();

				}
			}
		}
        /// <summary>This method checks if the process that begins the transaction is stay live. If the process terminated then abort the transaction. Also this method called periodically from transaction coordinator. </summary>
        /// <MetaDataID>{F85B2797-2AB8-4D24-9903-1CC967D4B1F1}</MetaDataID>
		public bool AbortIfInitiatorProcessExit()
		{
			try
			{
				try
				{
					ReaderWriterLock.AcquireWriterLock(10000);
				}
				catch(System.Exception)
				{
					return false;
				}

				if(InitiatorProcess.HasExited)
				{
					Abort(null);
					if(EnlistmentsController.State==ObjectsStateManagerState.AbortDone)
						return true;
				}
				return false;
			}
			finally
			{
				ReaderWriterLock.ReleaseLock();
			}
		}

        /// <MetaDataID>{5E8FD657-84EA-41C5-8E56-806EA5DA0ED8}</MetaDataID>
		~ TransactionRunTime()
		{
			int gg=0;
		}
	}
}
