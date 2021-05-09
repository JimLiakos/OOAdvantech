using System;
namespace OOAdvantech.Transactions
{
    using System;
    using System.Linq;


    /// <MetaDataID>{8C82D64F-6EED-4710-B564-3C53B5E56A57}</MetaDataID>
    /// <summary>Logical Thread takes advantage of stack trace to know if in previous call in stack has begin a new transaction.</summary>
    internal class LogicalThread
    {
#if !DeviceDotNet
        /// <MetaDataID>{391b89d7-938e-496e-a560-b30b4383731c}</MetaDataID>
        static void RemoveLogicalThread(WeakReference logicalThreadWR)
        {
            lock (LogicalThreads)
            {
                LogicalThreads.Remove(logicalThreadWR);
            }
        }
#else
    static void RemoveLogicalThread(WeakReference logicalThreadWR)
    {
      lock (LogicalThreads)
      {
        foreach (System.Collections.Generic.KeyValuePair<string, WeakReference> entry in LogicalThreads)
        {
          if (entry.Value == logicalThreadWR)
          {
            LogicalThreads.Remove(entry.Key);
            break;
          }
        }
      }
    }
#endif

        /// <MetaDataID>{d0306cbb-b186-4f0f-982c-7db0e28746a8}</MetaDataID>
        static void AddLogicalThread(LogicalThread logicalThread)
        {
            lock (LogicalThreads)
            {
#if !DeviceDotNet
                LogicalThreads.Add(new WeakReference(logicalThread));
#else
        LogicalThreads[logicalThread.ThreadID] = new WeakReference(logicalThread);
#endif
            }
        }


#if !DeviceDotNet
        int ThreadID;
        /// <MetaDataID>{0c37e9c5-5a6e-4574-8425-57ffc1f1e93b}</MetaDataID>
        public LogicalThread()
        {
            ThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            AddLogicalThread(this);
        }
#else
    string ThreadID;



    public LogicalThread()
    {
      ThreadID = CurrentThreadID;

      AddLogicalThread(this);

    }
    ~LogicalThread()
    {

    }

#endif


        //TODO:Να ενημερωθεί το UML διάγραμμα 

        /// <MetaDataID>{B31BF985-2DE6-4BC9-A1FC-BBDED0126C73}</MetaDataID>
        /// <summary>State transition entry keeps information about the native offset of code that do the object state transition, the transaction which used for the state transition and some debug information when the code run in debug mode.</summary>
      internal  class StateTransitionEntry
        {
            /// <MetaDataID>{6F3E49CA-4B16-41DD-BE15-E7A299193566}</MetaDataID>
            public StateTransitionEntry(Transaction transaction, SystemStateTransition stateTransition)
            {
                Transaction = transaction;
                StateTransition = stateTransition;
#if DeviceDotNet
        ThreadId = LogicalThread.CurrentThreadID;
#else
                ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif


                if (StateTransition.ObjectEnlistmentTimeOut == TimeSpan.Zero)
                    StateTransition.ObjectEnlistmentTimeOut = LogicalThread.ObjectEnlistmentTimeOut;
                else
                {

                }

            }

#if DeviceDotNet
      public readonly string ThreadId;
#else
            public readonly int ThreadId;
#endif



            /// <MetaDataID>{870BBABE-3904-494C-9C4B-ACE2EEF25016}</MetaDataID>
            public readonly Transaction Transaction;
            /// <MetaDataID>{9CA6BADB-6620-48B2-AACA-84660456A0BD}</MetaDataID>
            public readonly SystemStateTransition StateTransition;

        }

#if !DeviceDotNet
        internal struct EntryCodeCookie
        {
            public EntryCodeCookie(short callMethodStackDinstance)
            {
                callMethodStackDinstance++;
                if (System.Diagnostics.Debugger.IsAttached)
                    st = new System.Diagnostics.StackTrace(callMethodStackDinstance, true);
                else
                    st = new System.Diagnostics.StackTrace(callMethodStackDinstance, false);
                CalledFunctionCaller = st.GetFrame(1);
                CalledFunction = st.GetFrame(0);
                if (CalledFunctionCaller == null)
                    EntryCodeOwnerID = "Main" + CalledFunction.GetNativeOffset().ToString();
                else
                    EntryCodeOwnerID = CalledFunction.GetMethod().GetHashCode().ToString() + CalledFunctionCaller.GetNativeOffset().ToString();
            }
            public string EntryCodeOwnerID;
            public readonly System.Diagnostics.StackTrace st;
            public readonly System.Diagnostics.StackFrame CalledFunctionCaller;
            public readonly System.Diagnostics.StackFrame CalledFunction;
        }
#endif



        /// <MetaDataID>{18b631ab-49d0-4d53-8ba2-690defcb26f3}</MetaDataID>
        Transaction CurrentTransaction;
        /// <MetaDataID>{F7895E5E-70F9-426E-A224-561921D6616B}</MetaDataID>
        StateTransitionEntry CreateStateTransitionEntryAndPushInStack(Transaction transaction, SystemStateTransition stateTransition)
        {
            StateTransitionEntry transactionEntry = null;
            lock (this)
            {
                transactionEntry = new StateTransitionEntry(transaction, stateTransition);
                stateTransition.StateTransitionTransaction = transaction;
                transactionEntry.StateTransition.LogicalThread = this;
                OpenStateTransitionScoops.Push(transactionEntry);

                CurrentTransaction = transaction;

            }
            if (LogicalThread.HasOpenScoops(transaction))
            {
            }

            return transactionEntry;
        }


        /// <MetaDataID>{86889628-F89A-491E-9CFF-F91B2920BCDA}</MetaDataID>
        /// <summary>
        /// OpenTransitions stack keeps the state transition entries 
        /// for all open object state transition. 
        /// </summary>
        private System.Collections.Generic.Stack<StateTransitionEntry> OpenStateTransitionScoops = new System.Collections.Generic.Stack<StateTransitionEntry>();



        System.Collections.Generic.List<StateTransitionEntry> GetOpenStateTransitionScoops()
        {
            lock (this)
            {
                return OpenStateTransitionScoops.ToList();
            }
        }


        /// <MetaDataID>{B8B4828C-6802-4313-8983-911DEE15765B}</MetaDataID>
        StateTransitionEntry PeekLastStateTransitionEntryFromStack()
        {
            lock (this)
            {
                if (OpenStateTransitionScoops.Count > 0)
                    return (StateTransitionEntry)OpenStateTransitionScoops.Peek();
                else
                    return null;
            }

        }

        /// <summary>With this method retrieve the active transaction of logical thread. In logical thread we can start with a transaction. If in nested call initiate an object state transition with the transaction option RequiresNew then we start a new transaction the old transaction stay passive in stack. The last created transaction in logical thread is the active transaction.</summary>
        /// <MetaDataID>{F12FF72D-B28C-413D-9F87-08C72B2A9B9E}</MetaDataID>
        internal static Transaction GetActiveTransaction()
        {
            LogicalThread logicalThread = GetLogicalThreadForCurrentDotNetThread();
            return logicalThread.CurrentTransaction;

            //StateTransitionEntry stateTransitionEntry = logicalThread.PeekLastStateTransitionEntryFromStack();
            //if (stateTransitionEntry != null)
            //{
            //    if (stateTransitionEntry.Transaction is CommittableTransaction)
            //        return (stateTransitionEntry.Transaction as CommittableTransaction).Transaction;
            //    else
            //        return stateTransitionEntry.Transaction;
            //}
            //else
            //    return null;
        }

        /// <MetaDataID>{4f48d34f-01c6-4b90-96a5-7e238fb170fb}</MetaDataID>
        internal static TimeSpan ObjectEnlistmentTimeOut
        {
            get
            {

                LogicalThread logicalThread = GetLogicalThreadForCurrentDotNetThread();
                StateTransitionEntry stateTransitionEntry = logicalThread.PeekLastStateTransitionEntryFromStack();
                if (stateTransitionEntry != null)
                    return stateTransitionEntry.StateTransition.ObjectEnlistmentTimeOut;
                else
                    return TransactionManager.ObjectEnlistmentTimeOut;
            }
        }
        // static Synchronization.ReaderWriterLock ReaderWriterLock = new Synchronization.ReaderWriterLock();
        static internal bool HasOpenScoopsInOtherThreads(Transactions.Transaction transaction)
        {
            lock (LogicalThreads)
            {

                System.Collections.Generic.List<WeakReference> removeReferences = null;
                bool hasOpenScoops = false;

#if !DeviceDotNet
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                foreach (WeakReference weakReference in LogicalThreads)
                {

                    LogicalThread logicalThread = weakReference.Target as LogicalThread;
                    if (logicalThread != null)
                    {
                        if (logicalThread.ThreadID != threadId)
                        {
                            foreach (StateTransitionEntry transactionEntry in logicalThread.GetOpenStateTransitionScoops())
                            {
                                if (transactionEntry.StateTransition.StateTransitionTransaction == transaction && transactionEntry.ThreadId != threadId)
                                    hasOpenScoops = true;
                            }
                        }
                    }
                    else
                    {
                        if (removeReferences == null)
                            removeReferences = new System.Collections.Generic.List<WeakReference>();
                        removeReferences.Add(weakReference);
                    }
                }
#else
        string threadId = LogicalThread.CurrentThreadID;// System.Threading.Thread.CurrentThread.ManagedThreadId;
        foreach (WeakReference weakReference in LogicalThreads.Values)
        {

          LogicalThread logicalThread = weakReference.Target as LogicalThread;
          if (logicalThread != null)
          {
            if (logicalThread.ThreadID != threadId)
            {
              foreach (StateTransitionEntry transactionEntry in logicalThread.GetOpenStateTransitionScoops())
              {
                if (transactionEntry.StateTransition.StateTransitionTransaction == transaction && transactionEntry.ThreadId != threadId)
                  hasOpenScoops = true;
              }
            }

            //foreach (StateTransitionEntry transactionEntry in logicalThread.OpenStateTransitionScoops)
            //{
            //    if (transactionEntry.StateTransition.StateTransitionTransaction == transaction)
            //        hasOpenScoops = true;
            //}
          }
          else
          {
            if (removeReferences == null)
              removeReferences = new System.Collections.Generic.List<WeakReference>();
            removeReferences.Add(weakReference);
          }
        }

#endif
                if (removeReferences != null)
                {
                    foreach (WeakReference weakReference in removeReferences)
                        RemoveLogicalThread(weakReference);
                }

                return hasOpenScoops;
            }
        }


        static internal bool HasOpenScoopsInCurrentThread(Transactions.Transaction transaction)
        {
            lock (LogicalThreads)
            {

                System.Collections.Generic.List<WeakReference> removeReferences = null;
                bool hasOpenScoops = false;

#if !DeviceDotNet
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                foreach (WeakReference weakReference in LogicalThreads)
                {

                    LogicalThread logicalThread = weakReference.Target as LogicalThread;
                    if (logicalThread != null)
                    {
                        if (logicalThread.ThreadID == threadId)
                        {
                            foreach (StateTransitionEntry transactionEntry in logicalThread.GetOpenStateTransitionScoops())
                            {
                                if (transactionEntry.StateTransition.StateTransitionTransaction == transaction && transactionEntry.ThreadId == threadId)
                                    hasOpenScoops = true;
                            }
                        }
                    }
                    else
                    {
                        if (removeReferences == null)
                            removeReferences = new System.Collections.Generic.List<WeakReference>();
                        removeReferences.Add(weakReference);
                    }
                }
#else
        string threadId = LogicalThread.CurrentThreadID;
        foreach (WeakReference weakReference in LogicalThreads.Values)
        {

          LogicalThread logicalThread = weakReference.Target as LogicalThread;
          if (logicalThread != null)
          {
            if (logicalThread.ThreadID == threadId)
            {
              foreach (StateTransitionEntry transactionEntry in logicalThread.OpenStateTransitionScoops)
              {
                if (transactionEntry.StateTransition.StateTransitionTransaction == transaction)
                  hasOpenScoops = true;
              }
            }
          }
          else
          {
            if (removeReferences == null)
              removeReferences = new System.Collections.Generic.List<WeakReference>();
            removeReferences.Add(weakReference);
          }
        }

#endif
                if (removeReferences != null)
                {
                    foreach (WeakReference weakReference in removeReferences)
                        RemoveLogicalThread(weakReference);
                }

                return hasOpenScoops;
            }
        }

        internal static bool WaitToComplete(TransactionRunTime transaction)
        {
            LogicalThread logicalThread = GetLogicalThreadForCurrentDotNetThread();
            var stateTransitionEntry = logicalThread.PeekLastStateTransitionEntryFromStack();
            var lockTimeOut = stateTransitionEntry.StateTransition.ObjectEnlistmentTimeOut;

            var openScoops = GetOpenScoops(transaction);

          var task=  System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (openScoops.Count > 0)
                {
                    foreach (var openScoop in openScoops.ToList())
                    {
                        if (openScoop.StateTransition.Completed)
                            openScoops.Remove(openScoop);
                    }
                    System.Threading.Thread.Sleep(100);
                }
            });
            return task.Wait(lockTimeOut);
        }

        /// <MetaDataID>{76310dca-7f17-40cc-8358-1a824198e5a1}</MetaDataID>
        static internal bool HasOpenScoops(Transactions.Transaction transaction)
        {
            lock (LogicalThreads)
            {

                System.Collections.Generic.List<WeakReference> removeReferences = null;
                bool hasOpenScoops = false;
#if !DeviceDotNet
                foreach (WeakReference weakReference in LogicalThreads)
                {

                    LogicalThread logicalThread = weakReference.Target as LogicalThread;
                    if (logicalThread != null)
                    {
                        foreach (StateTransitionEntry transactionEntry in logicalThread.GetOpenStateTransitionScoops())
                        {
                            if (transactionEntry.StateTransition.StateTransitionTransaction == transaction)
                            {
                                if(System.Threading.Thread.CurrentThread.ManagedThreadId != transactionEntry.ThreadId)
                                {

                                }
                                hasOpenScoops = true;
                            }
                        }
                    }
                    else
                    {
                        if (removeReferences == null)
                            removeReferences = new System.Collections.Generic.List<WeakReference>();
                        removeReferences.Add(weakReference);
                    }
                }
#else
                foreach (WeakReference weakReference in LogicalThreads.Values)
                {

                    LogicalThread logicalThread = weakReference.Target as LogicalThread;
                    if (logicalThread != null)
                    {

                        foreach (StateTransitionEntry transactionEntry in logicalThread.GetOpenStateTransitionScoops())
                        {
                            if (transactionEntry.StateTransition.StateTransitionTransaction == transaction)
                                hasOpenScoops = true;
                        }
                    }
                    else
                    {
                        if (removeReferences == null)
                            removeReferences = new System.Collections.Generic.List<WeakReference>();
                        removeReferences.Add(weakReference);
                    }
                }

#endif
                if (removeReferences != null)
                {
                    foreach (WeakReference weakReference in removeReferences)
                        RemoveLogicalThread(weakReference);
                }

                return hasOpenScoops;
            }
        }

        static internal System.Collections.Generic.List<StateTransitionEntry> GetOpenScoops(Transactions.Transaction transaction)
        {
            lock (LogicalThreads)
            {
                System.Collections.Generic.List<StateTransitionEntry> openScoops = new System.Collections.Generic.List<StateTransitionEntry>();

                System.Collections.Generic.List<WeakReference> removeReferences = null;
                
#if !DeviceDotNet
                foreach (WeakReference weakReference in LogicalThreads)
                {

                    LogicalThread logicalThread = weakReference.Target as LogicalThread;
                    if (logicalThread != null)
                    {
                        foreach (StateTransitionEntry transactionEntry in logicalThread.GetOpenStateTransitionScoops())
                        {
                            if (transactionEntry.StateTransition.StateTransitionTransaction == transaction)
                            {
                                if (System.Threading.Thread.CurrentThread.ManagedThreadId != transactionEntry.ThreadId)
                                {
                                }
                                openScoops.Add(transactionEntry);
                            }
                        }
                    }
                    else
                    {
                        if (removeReferences == null)
                            removeReferences = new System.Collections.Generic.List<WeakReference>();
                        removeReferences.Add(weakReference);
                    }
                }
#else
                foreach (WeakReference weakReference in LogicalThreads.Values)
                {

                    LogicalThread logicalThread = weakReference.Target as LogicalThread;
                    if (logicalThread != null)
                    {

                        foreach (StateTransitionEntry transactionEntry in logicalThread.GetOpenStateTransitionScoops())
                        {
                            if (transactionEntry.StateTransition.StateTransitionTransaction == transaction)
                                openScoops.Add(transactionEntry);
                        }
                    }
                    else
                    {
                        if (removeReferences == null)
                            removeReferences = new System.Collections.Generic.List<WeakReference>();
                        removeReferences.Add(weakReference);
                    }
                }
#endif
                if (removeReferences != null)
                {
                    foreach (WeakReference weakReference in removeReferences)
                        RemoveLogicalThread(weakReference);
                }

                return openScoops;
            }
        }


#if !DeviceDotNet
        /// <MetaDataID>{f73db85b-0ffa-4419-89a2-592d534b932a}</MetaDataID>
        static System.Collections.Generic.List<WeakReference> LogicalThreads = new System.Collections.Generic.List<WeakReference>();

        /// <MetaDataID>{4FCC54B3-765D-44FE-AADA-FF7E95EF3D66}</MetaDataID>
        [ThreadStatic]
        static LogicalThread m_LogicalThread;
        /// <summary>Logical thread is one per thread of .Net Framework so there is a catalogue with pair entries. This method returns logical thread from current .Net thread.</summary>
        /// <MetaDataID>{9FE966B5-6FBA-44CE-8C68-8AD051664539}</MetaDataID>
        private static LogicalThread GetLogicalThreadForCurrentDotNetThread()
        {
            if (m_LogicalThread == null)
                m_LogicalThread = new LogicalThread();
            return m_LogicalThread;
        }
#else
    static System.Collections.Generic.Dictionary<string, WeakReference> LogicalThreads = new System.Collections.Generic.Dictionary<string, WeakReference>();
    /// <summary>Logical thread is one per thread of .Net Framework so there is a catalogue with pair entries. This method returns logical thread from current .Net thread.</summary>
    /// <MetaDataID>{9FE966B5-6FBA-44CE-8C68-8AD051664539}</MetaDataID>
    private static LogicalThread GetLogicalThreadForCurrentDotNetThread()
    {
      WeakReference logicalThreadReference = null;
      if (LogicalThreads.TryGetValue(CurrentThreadID, out logicalThreadReference))
      {
        LogicalThread logicalThread = logicalThreadReference.Target as LogicalThread;
        if (logicalThread != null)
          return logicalThread;
      }

      return new LogicalThread();
    }

    internal static string CurrentThreadID
    {
      get
      {
        if (System.Threading.Tasks.Task.CurrentId == null)
          return "Main_Thread";

        else
          return "Thread_" + System.Threading.Tasks.Task.CurrentId.ToString();
      }
    }
#endif


        /// <summary>Initialize the object state transition object in proportion with transaction option and update the stack for this state transition.
        /// If in stack exists open object state transition object then aborted.</summary>
        /// <param name="ObjectStateTransitionType">Specifies the automatic transaction type requested by the state transition.</param>
        /// <param name="systemStateTransition">Specifies the object state transition will be initiated.</param>
        /// <MetaDataID>{11CCB3DE-97C8-4FE5-ABBE-4F72B3275FF6}</MetaDataID>
        internal static void InitSystemStateTransition(TransactionOption ObjectStateTransitionType, SystemStateTransition systemStateTransition)
        {

            LogicalThread logicalThread = GetLogicalThreadForCurrentDotNetThread();
            StateTransitionEntry stateTransitionEntry = logicalThread.PeekLastStateTransitionEntryFromStack();//(true,entryCodeCookie);

            Transaction transaction = null;
            if (stateTransitionEntry != null)
                transaction = stateTransitionEntry.Transaction;

            switch (ObjectStateTransitionType)
            {
                case TransactionOption.Required:
                    {
                        if (transaction == null)
                        {
                            transaction = new TransactionRunTime();
                            systemStateTransition.IsTransactionInitiator = true;
                        }
                        else
                        {
                            if (transaction.Status != TransactionStatus.Continue)
                                throw new OOAdvantech.Transactions.TransactionException("The transaction of scoop is " + transaction.Status.ToString());
                        }
                        logicalThread.CreateStateTransitionEntryAndPushInStack(transaction, systemStateTransition);
                        break;
                    }

                case TransactionOption.RequiresNew:
                    {
                        transaction = new TransactionRunTime();
                        systemStateTransition.IsTransactionInitiator = true;
                        logicalThread.CreateStateTransitionEntryAndPushInStack(transaction, systemStateTransition);
                        break;
                    }
                case TransactionOption.RequiredNested:
                    {
                        if (transaction != null)
                            transaction = TransactionRunTime.GetTransactionRunTime(transaction).LaunchNestedTransaction();

                        else
                            transaction = new TransactionRunTime();


                        systemStateTransition.IsTransactionInitiator = true;
                        logicalThread.CreateStateTransitionEntryAndPushInStack(transaction, systemStateTransition);
                        break;
                    }

                case TransactionOption.Supported:
                    {
                        logicalThread.CreateStateTransitionEntryAndPushInStack(transaction, systemStateTransition);//,callMethodStackDinstance,entryCodeCookie);
                        break;

                    }
                case TransactionOption.Suppress:
                    {

                        logicalThread.CreateStateTransitionEntryAndPushInStack(null, systemStateTransition);//,callMethodStackDinstance,entryCodeCookie);
                        break;

                    }
                default:
                    throw new OOAdvantech.Transactions.TransactionException("The " + ObjectStateTransitionType.ToString() + " doesn't supported.");
            }
        }




        /// <MetaDataID>{8D9F65F4-FEFA-4782-B3F4-281B7157798F}</MetaDataID>
        public static void InitSystemStateTransitionWithTransaction(Transaction transaction, SystemStateTransition objectStateTransition)
        {

            if (transaction.Status != TransactionStatus.Continue)
                throw new OOAdvantech.Transactions.TransactionException("The transaction of scoop is " + transaction.Status.ToString());

            LogicalThread logicalThread = GetLogicalThreadForCurrentDotNetThread();
            logicalThread.CreateStateTransitionEntryAndPushInStack(transaction, objectStateTransition);//,callMethodStackDinstance,entryCodeCookie);



        }


        //public static void InitObjectStateTransitionWithMarshaledTransaction(string transactionUri, SystemStateTransition objectStateTransition)
        //{
        //    LogicalThread logicalThread = GetLogicalThreadForCurrentDotNetThread();
        //    Transaction transaction = TransactionManager.CurrentTransactionManager.Unmarshal(transactionUri);
        //    //logicalThread.CreateStateTransitionEntryAndPushInStack(transaction,objectStateTransition,callMethodStackDinstance,entryCodeCookie);
        //    logicalThread.CreateStateTransitionEntryAndPushInStack(transaction, objectStateTransition);//,callMethodStackDinstance,entryCodeCookie);

        //}

        /// <summary>Inform the logical thread that the object state transition has finished its work.</summary>
        /// <param name="stateTransition">Specifies the object state transition has finished its work.</param>
        /// <MetaDataID>{0E26AD4F-5689-47FE-B520-0A4FE3DBFFB7}</MetaDataID>
        public static void CheckStateTransitionCompleted(SystemStateTransition stateTransition)
        {
            LogicalThread logicalThread = GetLogicalThreadForCurrentDotNetThread();
            logicalThread.InternalCheckStateTransitionCompleted(stateTransition);
        }

        private void InternalCheckStateTransitionCompleted(SystemStateTransition stateTransition)
        {
            lock (this)
            {
                if (OpenStateTransitionScoops.Count == 0 && stateTransition.TransactionOption == TransactionOption.Supported)
                    return;
                if (OpenStateTransitionScoops.Count == 0)
                    throw new OOAdvantech.Transactions.TransactionException("There aren't transactions under LogicalThread control");

                bool TransactionExist = false;


                StateTransitionEntry stateTransitionEntry = null;
                foreach (StateTransitionEntry CurrTransactionEntry in OpenStateTransitionScoops)
                {
                    if (CurrTransactionEntry.StateTransition == stateTransition)
                    {
                        stateTransitionEntry = CurrTransactionEntry;
                        TransactionExist = true;
                        break;
                    }
                }

                if (TransactionExist)
                {
                    string ErorMessage = null;
                    StateTransitionEntry CurrTransactionEntry = PeekLastStateTransitionEntryFromStack();// (StateTransitionEntry)OpenStateTransitionScoops.Peek();

                    while (OpenStateTransitionScoops.Count > 0)
                    {
                        if (stateTransition != CurrTransactionEntry.StateTransition)
                        {

                            try
                            {
                                if (CurrTransactionEntry.Transaction != stateTransitionEntry.Transaction && CurrTransactionEntry.StateTransition.TransactionOption != TransactionOption.Suppress)
                                    CurrTransactionEntry.Transaction.Abort(new System.Exception("Object state transition doesn't completed"));
                            }
                            catch { }


                            lock (this)
                            {
                                OpenStateTransitionScoops.Peek().StateTransition.StateTransitionCompleted();
                                OpenStateTransitionScoops.Pop();


                                ErorMessage = "Object state transition doesn't completed";

                                CurrTransactionEntry = (StateTransitionEntry)OpenStateTransitionScoops.Peek();
                            }
                        }
                        else
                            break;
                    }


                    StateTransitionEntry currentStateTransitionEntry = PeekLastStateTransitionEntryFromStack();
                    if (currentStateTransitionEntry != null)
                        CurrentTransaction = currentStateTransitionEntry.Transaction;

                    //if (OpenStateTransitionScoops.Count > 0)
                    //  CurrentTransaction = OpenStateTransitionScoops.Peek().Transaction;
                    //else
                    //  CurrentTransaction = null;

                    if (ErorMessage != null)
                    {
                        //TODO να γραφτεί test case για αυτή την περίπτωση
                        if (stateTransition.StateTransitionTransaction != null && stateTransition.StateTransitionTransaction.Status == TransactionStatus.Aborted)
                            return;
                        if (stateTransition.TransactionOption != TransactionOption.Supported && stateTransition.TransactionOption != TransactionOption.Suppress)
                            stateTransition.StateTransitionTransaction.Abort(null);
                        else if (stateTransition.StateTransitionTransaction != null)
                            stateTransition.StateTransitionTransaction.Abort(null);
                        throw new OOAdvantech.Transactions.TransactionException(ErorMessage);
                    }

                }
                else
                {
                    //TODO να γραφτεί test case για αυτή την περίπτωση
                    //stateTransition.StateTransitionTransaction.Abort(null);
                    if (stateTransition.StateTransitionTransaction != null && stateTransition.StateTransitionTransaction.Status == TransactionStatus.Aborted)
                        return;
                    if (stateTransition.TransactionOption != TransactionOption.Supported && stateTransition.TransactionOption != TransactionOption.Suppress)
                        stateTransition.StateTransitionTransaction.Abort(null);
                    else if (stateTransition.StateTransitionTransaction != null)
                        stateTransition.StateTransitionTransaction.Abort(null);

                    throw new OOAdvantech.Transactions.TransactionException("There isn't transaction under LogicalThread control");
                }
            }
        }

        /// <MetaDataID>{8b5408d1-017d-4a04-bbb9-7433a5731537}</MetaDataID>
        internal static void StateTransitionCompleted(SystemStateTransition systemStateTransition)
        {
            LogicalThread logicalThread = GetLogicalThreadForCurrentDotNetThread();

            logicalThread.InternalStateTransitionCompleted(systemStateTransition);
        }

        internal void InternalStateTransitionCompleted(SystemStateTransition systemStateTransition)
        {
            lock (this)
            {
                int openScoopsCount = OpenStateTransitionScoops.Count;

                if (openScoopsCount == 0 && systemStateTransition.TransactionOption == TransactionOption.Supported)
                    return;

                if (openScoopsCount == 0)
                    throw new OOAdvantech.Transactions.TransactionException("There aren't transactions under LogicalThread control");

                lock (this)
                {
                    if ((OpenStateTransitionScoops.Peek() as OOAdvantech.Transactions.LogicalThread.StateTransitionEntry).StateTransition != systemStateTransition)
                        throw new OOAdvantech.Transactions.TransactionException("Transaction scoops are in incosistent state");

                    OpenStateTransitionScoops.Peek().StateTransition.StateTransitionCompleted();
                    OpenStateTransitionScoops.Pop();
                    if (openScoopsCount > 1)
                        CurrentTransaction = OpenStateTransitionScoops.Peek().Transaction;
                    else
                        CurrentTransaction = null;
                }
            }
        }
    }

}
