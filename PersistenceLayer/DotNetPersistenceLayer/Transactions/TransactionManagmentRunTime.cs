namespace OOAdvantech.Transactions
{
    using ReaderWriterLock = OOAdvantech.Synchronization.ReaderWriterLock;
    using LockCookie = OOAdvantech.Synchronization.LockCookie;
    using System;
#if DeviceDotNet
    using Xamarin.Forms;
    using System;
    using System.Transactions;
#endif
    /// <MetaDataID>{1060F0D8-9E7B-47E7-8917-63DED1D71F9B}</MetaDataID>
    /// <summary>Transaction coordinator is the core of transaction system for the machine which hosts it. </summary>

    internal class TransactionCoordinator : Remoting.MonoStateClass,ITransactionCoordinator
    {
        /// <MetaDataID>{29b7e6ee-452b-4aa8-b0cd-ca676c832fd2}</MetaDataID>
        public void InitProcessConnection()
        {
        }

#if DeviceDotNet
        /// <MetaDataID>{1D33D746-613E-4574-B222-1E9F692B9051}</MetaDataID>
        private System.Threading.Timer Timer;
#else
        private System.Threading.Timer Timer;
#endif 

        /// <summary>The transaction coordinator is monostate class. This means that when create a new instance of transaction coordinator actually you create a surrogate of a static instance which created when you create the first surrogate instance. The surrogate instance propagates calls to the static instance. This constructor initializes the surrogate instance. </summary>
        /// <MetaDataID>{F207338E-71AB-47A2-A5A3-A8C903334754}</MetaDataID>
        public TransactionCoordinator()
        {


            //lock (typeof(TransactionCoordinator))
            //{
            //    if (MonoStateTransactionCoordinator == null)
            //    {
            //        MonoStateTransactionCoordinator = this;
            //    }
            //}
#if !DeviceDotNet
            ChannelUri = "tcp://" + System.Net.Dns.GetHostName() + ":9050";
            // ReaderWriterLock = new ReaderWriterLock(true);
            Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTimer), null, 4000, 4000);
#else
            //Timer = new System.Timers.Timer(new System.Timers.TimerCallback(OnTimer), null, TimeSpan.FromSeconds(4));
            //Timer.Start();
            Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTimer), null, 4000, 4000);
#endif

            ExternalTransactionsEnlistmentsControllers = new OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController>();
            EnlistmentsControllers = new OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController>();

        }
        /// <MetaDataID>{09B82F24-8FFF-4E8D-8FC1-9D90B46EFD1A}</MetaDataID>
        public bool CommunicationCheck(string globalTransactionUri)
        {
            lock (EnlistmentsControllers)
            {
                if (!EnlistmentsControllers.ContainsKey(globalTransactionUri) && !ExternalTransactionsEnlistmentsControllers.ContainsKey(globalTransactionUri))
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// This method enlists an objects state manager in transaction 
        /// </summary>
        /// <param name="objectsStateManager">
        /// Define the objects state manager wants to enlist.
        /// </param>
        /// <param name="globalTransactionUri">
        /// Transaction identity in which the objects state manager wants to enlist.
        /// </param>
        /// <MetaDataID>{76F36543-C9E4-4F85-A89E-16F1E01A55C6}</MetaDataID>
        public void Enlist(ObjectsStateManager objectsStateManager, ref string globalTransactionUri)
        {



            IEnlistmentsController enlistmentsController = null;
            lock (EnlistmentsControllers)
            {
                EnlistmentsControllers.TryGetValue(globalTransactionUri, out enlistmentsController);
            }

            if (enlistmentsController == null)
            {
                //TODO  τα σενάρια των Derived transactions θα πρέπει να ελεχθούν γιατί έχων μέγαλο βαθμό 
                //πολυπλοκότητας και είναι δυσνόητα.

                //Derived transactions ("Derived_") είναι τα transaction που έχουν παραχθεί transaction system άλλο από το OOAdvantech DTC

                //Εάν το globalTransactionUri έχει παραχθεί από αυτόν τον DTC και δέν υπαρχει enlistments controller 
                //τοτε το σύστημα είναι σε μη συνεκτική κατασταση και εγείρει exception
                if (IsTheSame(GetTransactionInitiator(globalTransactionUri), this) && globalTransactionUri.IndexOf("Derived_") != 0)
                    throw new OOAdvantech.Transactions.TransactionException("Transaction with uri '" + globalTransactionUri + "' doesn't exist, commited or aborted");
                // TODO θα πρέπει να ξέρουμε τη απέγινε η transaction

                EnlistToExternalTransaction(objectsStateManager, ref globalTransactionUri);
            }
            else
                enlistmentsController.EnlistObjectsStateManager(objectsStateManager);




        }

        /// <MetaDataID>{b34caad0-0d44-40e2-a548-f713f1cb0549}</MetaDataID>
        public void ExportAsNestedTransaction(System.TimeSpan timeSpan, int initiatorProcessID, string originTransactionGlobalUri, ObjectsStateManager objectsStateManager, System.Transactions.Transaction nativeTransaction, ITransactionStatusNotification transactionStatusNotification, out string globalTransactionUri, out IEnlistmentsController enlistmentsController)
        {

            globalTransactionUri = System.Guid.NewGuid().ToString() + "_" + ChannelUri;


            enlistmentsController = new EnlistmentsController(globalTransactionUri, initiatorProcessID);
            enlistmentsController.TransactionInitiator = this;

            lock (EnlistmentsControllers)
            {
                EnlistmentsControllers[globalTransactionUri] = enlistmentsController;
            }
            if (nativeTransaction != null)
                enlistmentsController.AttachToNativeTransaction(nativeTransaction);
            (enlistmentsController as EnlistmentsController).StatusChanged += new ObjectsStateManagerStatusChanged(this.OnObjectsStateManagerChangeState);
            if (objectsStateManager != null)
                (enlistmentsController as EnlistmentsController).EnlistObjectsStateManager(objectsStateManager);
            if (transactionStatusNotification != null)
                enlistmentsController.TransactionCompletted += new TransactionEndedEventHandler(transactionStatusNotification.OnTransactionCompletted);

            NestedTransactionController nestedTransactionController = new NestedTransactionController(enlistmentsController);
            string lastGlobalTransactionUri = GetLastTransactionGlobalUriFromChain(originTransactionGlobalUri);
            Enlist(nestedTransactionController, ref lastGlobalTransactionUri);

        }

#if !DeviceDotNet
        /// <MetaDataID>{11CC48E0-45D8-45B6-9139-92AC5060054A}</MetaDataID>
        public void ExportTransaction(System.TimeSpan timeSpan, int initiatorProcessID, ObjectsStateManager objectsStateManager, System.Transactions.Transaction nativeTransaction, ITransactionStatusNotification transactionStatusNotification, out string globalTransactionUri, out IEnlistmentsController enlistmentsController)
        {
            globalTransactionUri = System.Guid.NewGuid().ToString() + "_" + ChannelUri;


            enlistmentsController = new EnlistmentsController(globalTransactionUri, initiatorProcessID);
            enlistmentsController.TransactionInitiator = this;
            lock (EnlistmentsControllers)
            {
                EnlistmentsControllers[globalTransactionUri] = enlistmentsController;
            }
            if (nativeTransaction != null)
            {
                enlistmentsController.AttachToNativeTransaction(nativeTransaction);
                nativeTransaction.EnlistVolatile(new NativeTransactionControl(), System.Transactions.EnlistmentOptions.EnlistDuringPrepareRequired);
            }
            (enlistmentsController as EnlistmentsController).StatusChanged += new ObjectsStateManagerStatusChanged(this.OnObjectsStateManagerChangeState);
            if (objectsStateManager != null)
                enlistmentsController.EnlistObjectsStateManager(objectsStateManager);
            if (transactionStatusNotification != null)
                enlistmentsController.TransactionCompletted += new TransactionEndedEventHandler(transactionStatusNotification.OnTransactionCompletted);

        }

        /// <MetaDataID>{27B95DF8-1E64-4376-89C5-F68EBA99A6E2}</MetaDataID>
        public void TransactionImport(string globalTransactionUri, ITransactionStatusNotification transactionStatusNotification)
        {
            //#region MonoStateClass
            //if (this != MonoStateTransactionCoordinator)
            //{
            //    MonoStateTransactionCoordinator.TransactionImport(globalTransactionUri, transactionStatusNotification);
            //    return;
            //}
            //#endregion
            //TODO  prone ReaderWriterLock
            IEnlistmentsController enlistmentsController = null;
            lock (EnlistmentsControllers)
            {
                EnlistmentsControllers.TryGetValue(globalTransactionUri, out enlistmentsController);
            }

            if (enlistmentsController == null)
            {

                ITransactionCoordinator transactionInitiator = GetTransactionInitiator(globalTransactionUri);

                if (transactionInitiator == this)
                    throw new OOAdvantech.Transactions.TransactionException("Transaction with uri '" + globalTransactionUri + "' doesn't exist, commited or aborted"); // Error Prone θα πρέπει να ξέρουμε τη απέγινε η transaction

                enlistmentsController = GetExternalTransactionEnlistmentsController(globalTransactionUri);

                if (enlistmentsController == null)
                {
                    ITransactionCoordinator TransactionCreator = GetTransactionInitiator(globalTransactionUri);
                    lock (this)
                    {
                        if (!ExternalTransactionsEnlistmentsControllers.TryGetValue(globalTransactionUri, out enlistmentsController))
                        {
                            enlistmentsController = new EnlistmentsController(globalTransactionUri, TransactionCreator);
                            ExternalTransactionsEnlistmentsControllers[globalTransactionUri] = enlistmentsController;
                            (enlistmentsController as EnlistmentsController).StatusChanged += new ObjectsStateManagerStatusChanged(this.OnObjectsStateManagerChangeState);
                            enlistmentsController.TransactionInitiator = TransactionCreator;
                            return;
                        }
                    }

                }
                if (enlistmentsController.NativeTransaction != null)
                    transactionStatusNotification.AttachToNativeTransaction(enlistmentsController.NativeTransaction);
                if (transactionStatusNotification != null)
                    enlistmentsController.TransactionCompletted += new TransactionEndedEventHandler(transactionStatusNotification.OnTransactionCompletted);

            }
            else
            {

                if (enlistmentsController.Status == ObjectsStateManagerStatus.AbortDone)
                    throw new OOAdvantech.Transactions.TransactionException("Transction with uri '" + globalTransactionUri + "' Aborted");
                if (enlistmentsController.Status == ObjectsStateManagerStatus.CommitDone)
                    throw new OOAdvantech.Transactions.TransactionException("Transction with uri '" + globalTransactionUri + "' Commited");

                if (enlistmentsController.NativeTransaction != null)
                    transactionStatusNotification.AttachToNativeTransaction(enlistmentsController.NativeTransaction);
                if (transactionStatusNotification != null)
                    enlistmentsController.TransactionCompletted += new TransactionEndedEventHandler(transactionStatusNotification.OnTransactionCompletted);

            }
        }

        /// <MetaDataID>{939A3436-A401-4B22-B6C6-8B7620C4F053}</MetaDataID>
        public string AttachToNativeTransaction(System.Transactions.Transaction nativeTransaction)
        {

            OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController> externalTransactionsEnlistmentsControllers = null;
            lock (this)
            {
                externalTransactionsEnlistmentsControllers = new OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController>(ExternalTransactionsEnlistmentsControllers);
            }
            foreach (System.Collections.Generic.KeyValuePair<string, IEnlistmentsController> enlistmentsControllerPair in externalTransactionsEnlistmentsControllers)
            {

                if ((enlistmentsControllerPair.Value.NativeTransaction as System.Transactions.Transaction) == nativeTransaction)
                    return enlistmentsControllerPair.Value.GlobalTransactionUri;
            }

            string globalTransactionUri = "Derived_" + nativeTransaction.TransactionInformation.DistributedIdentifier + "_" + ChannelUri;

            EnlistmentsController enlistmentsController = new EnlistmentsController(globalTransactionUri);
            enlistmentsController.TransactionInitiator = this;
            lock (this)
            {
                ExternalTransactionsEnlistmentsControllers[globalTransactionUri] = enlistmentsController;
            }
            enlistmentsController.StatusChanged += new ObjectsStateManagerStatusChanged(this.OnObjectsStateManagerChangeState);
            enlistmentsController.AttachToNativeTransaction(nativeTransaction);
            return globalTransactionUri;

        }

#endif

        /// <summary>The initiator of the transaction abort the transaction as any objects state manager enlisted on the transaction. </summary>
        /// <param name="globalTransactionUri">Define the identity of transaction.</param>
        /// <MetaDataID>{6278D5E4-6537-4113-AED8-720ED347A09C}</MetaDataID>
        public void Abort(string globalTransactionUri, System.Exception exception)
        {
            bool externalTransaction = false;
            lock (this)
            {
                externalTransaction = ExternalTransactionsEnlistmentsControllers.ContainsKey(globalTransactionUri);
            }
            if (externalTransaction)
            {
                try
                {
                    ITransactionCoordinator TransactionCreator = GetTransactionInitiator(globalTransactionUri);
                    TransactionCreator.Abort(globalTransactionUri, exception);
                }
                catch (System.Exception Error)
                {
                    int lo = 0;
                }
                return;
            }

            IEnlistmentsController enlistmentsController = null;
            lock (EnlistmentsControllers)
            {
                EnlistmentsControllers.TryGetValue(globalTransactionUri, out enlistmentsController);
            }
            if (enlistmentsController == null)
                throw new OOAdvantech.Transactions.TransactionException("Transaction with uri '" + globalTransactionUri + "' doesn't exist");
            enlistmentsController.AbortRequest(exception);
        }





        /// <summary>
        /// This method reorders the enlistments controller chain. 
        /// For example if the enlistments controller in machine (B) for transaction (A), 
        /// is subsidiary of enlistments controller in machine (A) 
        /// and client call the reorder method of transaction coordinator of machine (B) 
        /// to reorder chain to the machine (C), 
        /// the system will make the head of enlistment controller chain 
        /// which is the enlistment controller at machine (A), 
        /// subsidiary enlistment controller of head enlistment controller,
        /// of machine (C) enlistment controller. 
        /// </summary>
        /// <param name="globalTransactionUri"></param>
        /// <MetaDataID>{7785AEE3-9A21-46EE-A88C-EA1AD7B1871E}</MetaDataID>
        public void ReOrderDerivedTransactions(ref string globalTransactionUri)
        {
            IEnlistmentsController enlistmentsController = GetExternalTransactionEnlistmentsController(globalTransactionUri);
            if (enlistmentsController != null)
            {
                if (globalTransactionUri == enlistmentsController.GlobalTransactionUri)
                    return;

                if (IsTheSame(GetTransactionInitiator(globalTransactionUri), this))
                    globalTransactionUri = enlistmentsController.GlobalTransactionUri;


                if (!IsTheSame(enlistmentsController.TransactionInitiator, this))
                {
                    enlistmentsController.TransactionInitiator.ReOrderDerivedTransactions(ref globalTransactionUri);
                    if (enlistmentsController.GlobalTransactionUri != globalTransactionUri)
                    {
                        lock (this)
                        {
                            ExternalTransactionsEnlistmentsControllers.Remove(enlistmentsController.GlobalTransactionUri);
                            enlistmentsController.GlobalTransactionUri = globalTransactionUri;
                            ExternalTransactionsEnlistmentsControllers.Add(enlistmentsController.GlobalTransactionUri, enlistmentsController);
                        }
                    }
                }
                else
                {
                    TransactionCoordinator transactionCoordinator = GetTransactionInitiator(globalTransactionUri);
                    if (!IsTheSame(transactionCoordinator, this))
                    {
                        enlistmentsController.TransactionInitiator = transactionCoordinator.EnlistToRoot(enlistmentsController, ref globalTransactionUri);
                        if (enlistmentsController.GlobalTransactionUri != globalTransactionUri)
                        {
                            lock (this)
                            {
                                ExternalTransactionsEnlistmentsControllers.Remove(enlistmentsController.GlobalTransactionUri);
                                enlistmentsController.GlobalTransactionUri = globalTransactionUri;
                                ExternalTransactionsEnlistmentsControllers.Add(enlistmentsController.GlobalTransactionUri, enlistmentsController);
                            }
                        }
                        if (IsTheSame(transactionCoordinator, this))
                            enlistmentsController.TransactionInitiator = this;

                    }
                }
            }
        }

        /// <MetaDataID>{ECEF0497-DB4B-494B-B9B1-3F6DAE8188EB}</MetaDataID>
        public Transactions.ITransactionCoordinator EnlistToRoot(IEnlistmentsController remoteEnlistmentsController, ref string globalTransactionUri)
        {

            IEnlistmentsController enlistmentsController = GetExternalTransactionEnlistmentsController(globalTransactionUri);
            if (!IsTheSame(enlistmentsController.TransactionInitiator, this))
            {
                Transactions.ITransactionCoordinator transactionCoordinator =
                enlistmentsController.TransactionInitiator.EnlistToRoot(remoteEnlistmentsController, ref globalTransactionUri);

                if (enlistmentsController.GlobalTransactionUri != globalTransactionUri)
                {
                    lock (this)
                    {
                        ExternalTransactionsEnlistmentsControllers.Remove(enlistmentsController.GlobalTransactionUri);
                        enlistmentsController.GlobalTransactionUri = globalTransactionUri;
                        ExternalTransactionsEnlistmentsControllers.Add(enlistmentsController.GlobalTransactionUri, enlistmentsController);
                    }
                }
                return transactionCoordinator;
            }
            else
            {
                if (enlistmentsController != remoteEnlistmentsController)
                    enlistmentsController.EnlistObjectsStateManager(remoteEnlistmentsController as ObjectsStateManager);
                globalTransactionUri = enlistmentsController.GlobalTransactionUri;
                return this;
            }

        }





        ///// <summary>This operation returns all objects state managers that participate in transaction with ID the value of transactionUri parameter. </summary>
        ///// <param name="globalTransactionUri">Define the identity of transaction. </param>
        ///// <MetaDataID>{0B643886-7C90-45F4-BA04-309649F7123B}</MetaDataID>
        //public System.Collections.ArrayList GetEnlistedObjectsStateManagers(string globalTransactionUri)
        //{

        //    //#region MonoStateClass
        //    //if (this != MonoStateTransactionCoordinator)
        //    //    return MonoStateTransactionCoordinator.GetEnlistedObjectsStateManagers(globalTransactionUri);
        //    //#endregion


        //    //OOAdvantech.Collections.ArrayList resourceManagers=new OOAdvantech.Collections.ArrayList();
        //    ReaderWriterLock.AcquireReaderLock(10000);
        //    try
        //    {


        //        if (ExternalTransactionsEnlistmentsControllers.ContainsKey(globalTransactionUri))
        //            return GetTransactionInitiator(globalTransactionUri).GetEnlistedObjectsStateManagers(globalTransactionUri); // Error Prone εάν φάει exception
        //        if (EnlistmentsControllers.ContainsKey(globalTransactionUri))
        //            return EnlistmentsControllers[globalTransactionUri].GetEnlistedObjectsStateManagers();
        //    }
        //    finally
        //    {
        //        ReaderWriterLock.ReleaseReaderLock();
        //    }
        //    return new Collections.ArrayList();
        //}



        ///// <MetaDataID>{4041FA32-3415-4CDD-9312-7459D1626331}</MetaDataID>
        //public void ReviseObjectsStateManager(ObjectsStateManager objectsStateManager, string globalTransactionUri)
        //{

        //    EnlistmentsController enlistmentsController = null;
        //    lock (this)
        //    {
        //        EnlistmentsControllers.TryGetValue(globalTransactionUri, out enlistmentsController);
        //    }
        //    if (enlistmentsController != null)
        //        enlistmentsController.ReviseObjectsStateManager(objectsStateManager);
        //    else
        //    {
        //        enlistmentsController = GetExternalTransactionEnlistmentsController(globalTransactionUri);
        //        if (enlistmentsController != null)
        //            enlistmentsController.ReviseObjectsStateManager(objectsStateManager);
        //        else
        //            throw new OOAdvantech.Transactions.TransactionException("Transaction with uri '" + globalTransactionUri + "' doesn't exist, commited or aborted"); // TODO Prone θα πρέπει να ξέρουμε τη απέγινε η transaction
        //    }

        //}


        /// <MetaDataID>{26290AD9-F755-4EFC-BE49-07B46EB2AFA5}</MetaDataID>
        internal OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController> ExternalTransactionsEnlistmentsControllers;


        /// <MetaDataID>{898BC10F-BC15-4799-A569-2DDB283B4369}</MetaDataID>
        internal OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController> EnlistmentsControllers;

        /// <MetaDataID>{D4BACB5E-ED0B-415C-93E5-E404E5334AE8}</MetaDataID>
        IEnlistmentsController GetExternalTransactionEnlistmentsController(string globalTransactionUri)
        {
            OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController> externalTransactionsEnlistmentsControllers = null;
            lock (this)
            {
                if (ExternalTransactionsEnlistmentsControllers.Count == 0)
                    return null;
                externalTransactionsEnlistmentsControllers = new OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController>(ExternalTransactionsEnlistmentsControllers);
            }

            if (externalTransactionsEnlistmentsControllers.ContainsKey(globalTransactionUri))
                return externalTransactionsEnlistmentsControllers[globalTransactionUri];
            else
            {
                //Derived transactions ("Derived_") είναι τα transaction που έχουν παραχθεί transaction system άλλο από το OOAdvantech DTC
                if (globalTransactionUri.IndexOf("Derived_") != 0)
                    return null;

                foreach (System.Collections.Generic.KeyValuePair<string, IEnlistmentsController> entry in externalTransactionsEnlistmentsControllers)
                {

                    //Derived transactions ("Derived_") είναι τα transaction που έχουν παραχθεί transaction system άλλο από το OOAdvantech DTC
                    if (entry.Value.GlobalTransactionUri.IndexOf("Derived_") == 0
                        && GetDistributedIdentifier(entry.Value.GlobalTransactionUri) == GetDistributedIdentifier(globalTransactionUri))
                    {
                        return entry.Value;
                    }
                }
                return null;
            }

        }

        /// <MetaDataID>{C00F9805-D893-4A26-A1BC-8D25DD10198A}</MetaDataID>
        internal static string GetDistributedIdentifier(string globalTransactionUri)
        {

            //Derived transactions ("Derived_") είναι τα transaction που έχουν παραχθεί transaction system άλλο από το OOAdvantech DTC
            if (globalTransactionUri.IndexOf("Derived_") == 0)
            {
                globalTransactionUri = globalTransactionUri.Replace("Derived_", "");
                int index = globalTransactionUri.LastIndexOf('_');
                return globalTransactionUri.Substring(0, index);
            }
            else
                return null;
        }






        /// <MetaDataID>{987C5D7F-F0F6-4A64-BF86-686FFA145C05}</MetaDataID>
        private string ChannelUri;



        /// <summary>The OnObjectsStateManagerChangeState method is a ObjectsStateManagerChangeState event consumer. When this event happens and the state is one from transaction end state remove the transaction from the control of transaction coordinator. </summary>
        /// <MetaDataID>{39C6D9EB-A20F-4C01-B02C-933064C80A17}</MetaDataID>
        void OnObjectsStateManagerChangeState(object sender, ObjectsStateManagerStatus newState)
        {
            if (sender is EnlistmentsController)
            {
                EnlistmentsController enlistmentsController = sender as EnlistmentsController;
                if (newState == ObjectsStateManagerStatus.AbortDone || newState == ObjectsStateManagerStatus.CommitDone)
                {
                    ExternalTransactionsEnlistmentsControllers.Remove(enlistmentsController.GlobalTransactionUri);
                    enlistmentsController.StatusChanged -= new ObjectsStateManagerStatusChanged(this.OnObjectsStateManagerChangeState);
                }
            }
        }


        /// <summary>The OnTimer method is a timer event consumer. Periodically the transaction coordinator check if the processes that begin the transactions are still live, and if there is connection between the transaction manager end objects state managers. </summary>
        /// <MetaDataID>{CFF1054E-0F7C-477A-84B1-ADE7156170DF}</MetaDataID>
        void OnTimer(object state)
        {
            try
            {
                System.GC.Collect();
                OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController> enlistmentsControllers;
                lock (EnlistmentsControllers)
                {


                    try
                    {
                        enlistmentsControllers = new OOAdvantech.Collections.Generic.Dictionary<string, IEnlistmentsController>(EnlistmentsControllers);
                    }
                    catch (System.Exception)
                    {
                        return;
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, IEnlistmentsController> entry in enlistmentsControllers)
                {
                    IEnlistmentsController enlistmentsController = entry.Value;
                    if (!enlistmentsController.StillActive)
                    {

                        lock (EnlistmentsControllers)
                        {
                            EnlistmentsControllers.Remove(entry.Key);
                        }
                    }
                    else if (enlistmentsController.Status == ObjectsStateManagerStatus.CommitDone ||
                        enlistmentsController.Status == ObjectsStateManagerStatus.AbortDone && ((System.TimeSpan)(System.DateTime.Now - enlistmentsController.LastStatusChangeTimestamp)).Milliseconds > 10000)
                    {
                        lock (EnlistmentsControllers)
                        {
                            EnlistmentsControllers.Remove(entry.Key);
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                try
                {
                    InformEventLog(error);
                }
                catch (System.Exception err)
                {

                }


            }
        }
        /// <MetaDataID>{412DF177-D66D-4FA0-B5C4-3A55CB95E965}</MetaDataID>
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
            myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
            System.Exception InerError = Error.InnerException;
            //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
            while (InerError != null)
            {
                System.Diagnostics.Debug.WriteLine(
                    InerError.Message + InerError.StackTrace);
                myLog.WriteEntry(InerError.Message + InerError.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                InerError = InerError.InnerException;
            }
#endif
        }

        /// <MetaDataID>{3934A273-2B41-419D-94ED-CBFB7CFBF0D5}</MetaDataID>
        static bool IsTheSame(ITransactionCoordinator leftTransactionCoordinator, ITransactionCoordinator rightTransactionCoordinator)
        {
#if !DeviceDotNet
            if (leftTransactionCoordinator == rightTransactionCoordinator)
                return true;
            bool leftOutOfProcess = Remoting.RemotingServices.IsOutOfProcess(leftTransactionCoordinator as System.MarshalByRefObject);
            bool rightOutOfProcess = Remoting.RemotingServices.IsOutOfProcess(leftTransactionCoordinator as System.MarshalByRefObject);
            //the left and right are in the same process
            if (!leftOutOfProcess && !rightOutOfProcess)
                return true;
            //One from them is in process and the other isn't. 
            if (leftOutOfProcess != rightOutOfProcess)
                return false;
            if (Remoting.RemotingServices.GetChannelUri(leftTransactionCoordinator as System.MarshalByRefObject) == Remoting.RemotingServices.GetChannelUri(rightTransactionCoordinator as System.MarshalByRefObject))
                return true;
#endif
            return false;

        }

        /// <summary>This method takes charge to enlist the objects state manager in transaction which isn't started from this coordinator. </summary>
        /// <MetaDataID>{155C7AA3-9E81-4D30-B8C0-02E5C5E2D86C}</MetaDataID>
        void EnlistToExternalTransaction(ObjectsStateManager objectsStateManager, ref string globalTransactionUri)
        {
            try
            {
                IEnlistmentsController enlistmentsController = GetExternalTransactionEnlistmentsController(globalTransactionUri);
                if (enlistmentsController == null)
                {

                    TransactionCoordinator TransactionCreator = GetTransactionInitiator(globalTransactionUri);
                    lock (this)
                    {
                        if (ExternalTransactionsEnlistmentsControllers.TryGetValue(globalTransactionUri, out enlistmentsController))
                        {
                            enlistmentsController = new EnlistmentsController(globalTransactionUri, TransactionCreator);
                            ExternalTransactionsEnlistmentsControllers[globalTransactionUri] = enlistmentsController;
                            (enlistmentsController as EnlistmentsController).StatusChanged += new ObjectsStateManagerStatusChanged(this.OnObjectsStateManagerChangeState);
                            return;
                        }
                    }
                    enlistmentsController.EnlistObjectsStateManager(objectsStateManager);
                    return;
                }
                else
                {
                    if (!IsTheSame(enlistmentsController.TransactionInitiator, GetTransactionInitiator(globalTransactionUri)))
                    {
                        ReOrderDerivedTransactions(ref globalTransactionUri);
                        if (enlistmentsController.GlobalTransactionUri != globalTransactionUri)
                        {
                            lock (this)
                            {
                                ExternalTransactionsEnlistmentsControllers.Remove(enlistmentsController.GlobalTransactionUri);
                                enlistmentsController.GlobalTransactionUri = globalTransactionUri;
                                ExternalTransactionsEnlistmentsControllers.Add(enlistmentsController.GlobalTransactionUri, enlistmentsController);
                            }
                        }



                    }

                    if (!IsTheSame(enlistmentsController.TransactionInitiator, this) && objectsStateManager is EnlistmentsController)
                        enlistmentsController.TransactionInitiator.Enlist(objectsStateManager, ref globalTransactionUri);
                    else
                        enlistmentsController.EnlistObjectsStateManager(objectsStateManager);
                    return;
                }
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.Transactions.TransactionException(Error.Message, Error);
            }

        }

        /// <MetaDataID>{1b989a1d-a9dc-4fb3-bf61-d80f970f70a3}</MetaDataID>
        static string GetLastTransactionGlobalUriFromChain(string transactionGlobalUrisChain)
        {
            int nPos = transactionGlobalUrisChain.IndexOf(@"\");
            if (nPos == -1)
                return transactionGlobalUrisChain;
            else
                return transactionGlobalUrisChain.Substring(0, nPos);

        }




        /// <MetaDataID>{42e2b99e-ea07-48de-8958-b9a8d79b3719}</MetaDataID>
        internal static string GetChannelUri(string globalTransactionUri)
        {

            string TransactionChannelUri = globalTransactionUri.Substring(37, globalTransactionUri.Length - 37);
            TransactionChannelUri = TransactionChannelUri.ToLower();
            TransactionChannelUri = TransactionChannelUri.Trim();
            return TransactionChannelUri;

        }

        /// <summary>This operation returns the transaction coordinator that begins the transaction with identity of transactionUri parameter. </summary>
        /// <param name="globalTransactionUri">Define the identity of transaction. </param>
        /// <MetaDataID>{6774704A-6B14-451A-B5F6-D020C733AC71}</MetaDataID>
        private TransactionCoordinator GetTransactionInitiator(string globalTransactionUri)
        {

#if DISTRIBUTED_TRANSACTIONS


            lock (EnlistmentsControllers)
            {

                if (EnlistmentsControllers.ContainsKey(globalTransactionUri))
                    return this;
            }
            string TransactionChannelUri = GetChannelUri(globalTransactionUri);//.Substring(37, globalTransactionUri.Length - 37);
            TransactionChannelUri = TransactionChannelUri.ToLower();
            TransactionChannelUri = TransactionChannelUri.Trim();
            ChannelUri = ChannelUri.ToLower();
            ChannelUri.Trim();
            Remoting.RemotingServices theRemotingServices = Remoting.RemotingServices.GetRemotingServices(TransactionChannelUri);
            TransactionCoordinator TransactionCreator = theRemotingServices.CreateInstance(typeof(TransactionCoordinator).ToString(), typeof(TransactionCoordinator).Assembly.FullName) as TransactionCoordinator;
            //if (!OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(TransactionCreator))
            //    throw new OOAdvantech.Transactions.TransactionException("Transaction with URI '" + globalTransactionUri + "' isn't more active");
            return TransactionCreator;
#else
            return null;
#endif
        }
#if DeviceDotNet
        public void TransactionImport(string globalTransactionUri, ITransactionStatusNotification transactionStatusNotification)
        {
            throw new NotImplementedException();
        }

        public void ExportTransaction(TimeSpan timeSpan, int initiatorProcessID, ObjectsStateManager objectsStateManager, System.Transactions.Transaction nativeTransaction, ITransactionStatusNotification transactionStatusNotification, out string globalTransactionUri, out IEnlistmentsController enlistmentsController)
        {
            throw new NotImplementedException();
        }

        public string AttachToNativeTransaction(System.Transactions.Transaction nativeTransaction)
        {
            throw new NotImplementedException();
        }
#endif
    }

    
    
#region Excluded for .Net CompactFramework


#if !DeviceDotNet

    /// <MetaDataID>{5131a27e-b9af-4bfe-9098-6f2bec96ce31}</MetaDataID>
    class NativeTransactionControl : System.Transactions.IEnlistmentNotification
    {


        /// <MetaDataID>{369bf0c9-7e47-4b0f-a4f4-26d0ee05d3e2}</MetaDataID>
        public void Commit(System.Transactions.Enlistment enlistment)
        {
            enlistment.Done();

        }

        /// <MetaDataID>{bfff9d90-eea0-4b5b-93b9-21a13a0e9848}</MetaDataID>
        public void InDoubt(System.Transactions.Enlistment enlistment)
        {
            enlistment.Done();
        }

        /// <MetaDataID>{e456651d-8e9f-44f1-8bd2-c36b1de94fb9}</MetaDataID>
        public void Prepare(System.Transactions.PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        /// <MetaDataID>{9cd9a1ba-5ca1-4492-becc-d864db089ddc}</MetaDataID>
        public void Rollback(System.Transactions.Enlistment enlistment)
        {
            enlistment.Done();
        }


    }
#endif
#endregion

}
