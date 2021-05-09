namespace OOAdvantech.PersistenceLayerRunTime
{

    using Remoting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif

#if DeviceDotNet
    /// <MetaDataID>{51c0ec0b-ad67-4bcb-80a5-6a5e1701aeed}</MetaDataID>
    public enum ContextState
    {
        Active,
        Prepare,
        Commit,
        Abort,
        InDoubt
    }

    /// <MetaDataID>{07787414-f45a-4728-8f93-66ee6b75e482}</MetaDataID>
    public interface ITransactionContext : Transactions.ITransactionContextExtender
    {
        /// <MetaDataID>{b34ebcdc-0f26-402b-ac1d-b429c3322a9d}</MetaDataID>
        System.Collections.Generic.List<object> EnlistObjects
        {
            get;
        }

        /// <MetaDataID>{4446bbfc-0631-44e9-bf6e-0fb6ac91ac30}</MetaDataID>
        void EnlistCommand(Commands.Command command);
        /// <MetaDataID>{0816f084-7d2d-4a0e-a96b-ee3ff5454f9e}</MetaDataID>
        bool ContainCommand(string Identity);
        /// <MetaDataID>{29711e46-ad91-4ab2-ade9-cbe03489965e}</MetaDataID>
        System.Collections.Generic.Dictionary<string, OOAdvantech.PersistenceLayerRunTime.Commands.Command> EnlistedCommands
        {
            get;
        }

        /// <MetaDataID>{582ca6f2-3a5c-433b-9cac-f290124f60b3}</MetaDataID>
        void AddSlaveTransactionContext(ITransactionContext transactionContext);
        /// <MetaDataID>{df57c910-e2ba-41d8-85cf-994c2df41419}</MetaDataID>
        int ProcessCommands(int executionOrder);
    }

    /// <MetaDataID>{f01b07db-d709-47e8-97f8-f2361a42bb61}</MetaDataID>
    public interface ITransactionContextManager
    {
        /// <MetaDataID>{6277f8a9-cfde-48b1-b0ff-d97f1e1dad8d}</MetaDataID>
        ITransactionContext GetMasterTransactionContext(string globalTransactionUri);
    }
#endif

    /// <summary>
    /// This class defines a transaction context extension. 
    /// When enlist an object in the transaction, the state of object in memory, 
    /// manipulated from the transaction context of transaction system. 
    /// This extension of transaction context manipulates the persistent state of objects. 
    /// </summary>
    /// <MetaDataID>{1C7EDB61-B6A2-411F-84EB-B9C70AFC606B}</MetaDataID>
    public class TransactionContext : MarshalByRefObject, Remoting.IExtMarshalByRefObject, ITransactionContext
    {

        /// <MetaDataID>{FBB2641D-1035-47E0-84EE-EFE5064CEECF}</MetaDataID>
        public ContextState State = ContextState.Active;
        

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7C4466CD-6918-4951-B603-F29A9DD6FFC8}</MetaDataID>
        private System.Collections.Generic.List<ITransactionContext> _Slaves = new System.Collections.Generic.List<ITransactionContext>();


        ///<summary>
        ///This Property defines a collection with slave transaction contexts. 
        ///Client code can enlist one or more objects in transaction,
        ///this objects can live in one or more processes 
        ///also in one or more machines.
        ///In preparation state transaction context execute commands. 
        ///The commands have execution order. 
        ///For this reason there is a master context which controls the execution order. 
        ///The slave contexts execute commands through ProcessCommands method.      
        ///</summary>
        /// <MetaDataID>{DCC4F145-B55C-420F-AC2B-DC0B1CA21BB5}</MetaDataID>
        public System.Collections.Generic.List<ITransactionContext>  Slaves
        {
            get
            {
                lock (this)
                {
                    return new System.Collections.Generic.List<ITransactionContext>(_Slaves);
                }
            }
        }

        /// <MetaDataID>{ab1482c6-003e-47ab-bc13-ed0551017ac6}</MetaDataID>
        public static ITransactionContext GetTransactionContext(Transactions.Transaction transaction)
        {
            if (transaction == null)
                return null;
            foreach (Transactions.ITransactionContextExtender extender in Transactions.TransactionManager.GetTransactionContext(transaction).Extenders)
            {
                if (extender is PersistenceLayerRunTime.ITransactionContext)
                    return extender as PersistenceLayerRunTime.ITransactionContext;
            }
            return null;

        }
        ///<summary>
        ///This property has the transaction context where run the client code.
        ///</summary>
        /// <MetaDataID>{330224EB-A6B5-4B01-BB10-68B2846923A6}</MetaDataID>
        public static ITransactionContext CurrentTransactionContext
        {
            get
            {


                Transactions.Transaction transaction = Transactions.Transaction.Current;
                if (transaction == null)
                    return null;
                foreach (Transactions.ITransactionContextExtender extender in Transactions.TransactionManager.GetTransactionContext(transaction).Extenders)
                {
                    if (extender is PersistenceLayerRunTime.ITransactionContext)
                        return extender as PersistenceLayerRunTime.ITransactionContext;
                }
                return null;
            }
        }






        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7EA2398B-52F1-492E-B89C-19FCFB37ADEE}</MetaDataID>
        private ITransactionContext _Master;
        ///<summary>
        ///This Property defines  the master transaction contexts. 
        ///Client code can enlist one or more objects in transaction,
        ///this objects can live in one or more processes 
        ///also in one or more machines.
        ///In preparation state transaction context execute commands. 
        ///The commands have execution order. 
        ///For this reason must be a master context which control the execution order. 
        ///The slave contexts execute commands through ProcessCommands method.      
        ///</summary>
        /// <MetaDataID>{F1A16279-82C4-40CB-9CD4-E5AF9593D6AA}</MetaDataID>
        public ITransactionContext Master
        {
            get
            {
                lock (this)
                {
                    if (_Master != null)
                        return _Master;
                    else
                        return this;
                }
            }
            set
            {
                lock (this)
                {
                    if (_Master == value)
                        return;
                    if (this == value)
                        return;
                    if (_Master != null && _Master != value)
                        throw new System.Exception("Master transaction context already set");
                    _Master = value;
                }
                _Master.AddSlaveTransactionContext(this);

            }
        }

        ///<summary>
        ///Add transaction context as slave. 
        ///</summary>
        /// <MetaDataID>{D8FC74AD-3E70-4E68-A97F-21A4E7227F0E}</MetaDataID>
        public void AddSlaveTransactionContext(ITransactionContext transactionContext)
        {

            if (transactionContext == null)
                throw new System.ArgumentNullException("transactionContext");
            lock (this)
            {
                _Slaves.Add(transactionContext);
            }

        }

        ///<summary>
        ///Remove a transaction context from slave collection. 
        ///</summary>
        ///<param name="transactionContext">
        ///This parameter define the slave transaction context
        ///</param>
        /// <MetaDataID>{1722D790-7607-4EC5-B174-5C3F33B3581F}</MetaDataID>
        public void RemoveSlaveTransactionContext(TransactionContext transactionContext)
        {
            if (transactionContext == null)
                return;
            lock (this)
            {
                if (_Slaves.Contains(transactionContext))
                    _Slaves.Remove(transactionContext);
            }
        }


        /// <summary>
        /// This field define the number of commands which will be executed.
        /// </summary>
        /// <MetaDataID>{6EE951A3-0E1E-4969-8DC4-1CDC811DEC48}</MetaDataID>
        int NumberOfCommands = 0;

        /// <MetaDataID>{18B6A91A-B115-489C-85F4-3959602A3979}</MetaDataID>
        /// <summary>
        /// This collection defines the enlisted objects in transaction, where the classes 
        /// of objects is persistent.
        /// </summary>
        internal System.Collections.Generic.List<object> _EnlistObjects = new System.Collections.Generic.List<object>();
        /// <MetaDataID>{4aa24efd-e090-4eff-ab1e-ebc05e7ff8b2}</MetaDataID>
        public System.Collections.Generic.List<object> EnlistObjects
        {
            get
            {
                return _EnlistObjects.ToList();
            }
        }


        #region ITransactionContextExtender Members
        ///<summary>
        ///This method realize the OOAdvantech.Transactions.ITransactionContextExtender.EnlistObject
        ///operation.
        ///</summary>
        /// <MetaDataID>{A0BB6ADC-792F-4B16-8B26-5DF91C36BC65}</MetaDataID>
        public void EnlistObject(object transactionedObject, System.Reflection.FieldInfo fieldInfo)
        {


            //δεν χριάζεται συνηρονισμό γιατί καλείται μόνο μία φορά όταν το object προσκολήσει στο τρανσαψτιον


            if (ObjectStorage.PersistencyService.ClassOfObjectIsPersistent(transactionedObject))
            {
#if !DeviceDotNet
                bool tryToAttachMaster = false;
                lock (this)
                {
                    tryToAttachMaster = !System.String.IsNullOrEmpty(Transaction.GlobalTransactionUri) && Master == this;
                }

                if (tryToAttachMaster)
                {
                    OOAdvantech.PersistenceLayerRunTime.ITransactionContextManager persistencyService = Remoting.RemotingServices.CreateRemoteInstance("tcp://localhost:9060", typeof(PersistencyService).FullName, "") as OOAdvantech.PersistenceLayerRunTime.ITransactionContextManager;
                    if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                        Master = persistencyService.GetMasterTransactionContext(Transaction.Marshal());
                }
#endif
                lock (this)
                {
                    _EnlistObjects.Add(transactionedObject);
                }
                if (State == ContextState.Prepare)
                    PrepareEnlistedObject(transactionedObject);
            }
            else if (transactionedObject is StorageInstanceRef)
            {
                bool tryToAttachMaster = false;
#if !DeviceDotNet
                lock (this)
                {
                    tryToAttachMaster = !System.String.IsNullOrEmpty(Transaction.GlobalTransactionUri) && Master == this;
                }
                if (tryToAttachMaster)
                {
                    OOAdvantech.PersistenceLayerRunTime.ITransactionContextManager persistencyService = Remoting.RemotingServices.CreateRemoteInstance("tcp://localhost:9060", typeof(PersistencyService).FullName, "") as OOAdvantech.PersistenceLayerRunTime.ITransactionContextManager;
                    if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                        Master = persistencyService.GetMasterTransactionContext(Transaction.Marshal());
                }
#endif

                #region Check if there is update storrage instance command. If exist then return.

                StorageInstanceRef storageInstanceRef = transactionedObject as StorageInstanceRef;

                //TODO  να γραφτεί test case  για αυτήν την περίπτωση.			
                if (ContainCommand(Commands.UpdateStorageInstanceCommand.GetIdentity(storageInstanceRef)) ||
                    ContainCommand(Commands.UpdateReferentialIntegrity.GetIdentity(storageInstanceRef)))
                {
                    return;
                }

                AddParticipatedStorageInstance(storageInstanceRef);

                #endregion

            }
        }
        ///<summary>
        ///This method realize the OOAdvantech.Transactions.ITransactionContextExtender.Prepare
        ///operation. Produce end execute all commands to update the storage instances which
        ///correspond to th enlisted object.
        ///</summary>
        /// <MetaDataID>{E6C5CB07-ABAB-4FA1-ACE3-6BC1D551E5EC}</MetaDataID>
        public void Prepare()
        {
            if (Master == this)
            {
                try
                {
                    
                    System.Collections.Generic.List<PersistenceLayer.ObjectStorage> objectStoragesUnderTransaction = null;
                    lock (this)
                    {
                        objectStoragesUnderTransaction = ObjectStoragesUnderTransaction.Values.ToList();
                    }

                    Exception exception = null;
                    foreach (ObjectStorage objectStorage in objectStoragesUnderTransaction)
                    {
                        try
                        {
                            objectStorage.PrepareForChanges(this);
                        }
                        catch (System.Exception error)
                        {

#if !DeviceDotNet
                            System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                            myLog.Source = "PersistencySystem";
                            if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                                myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);
                            System.Diagnostics.Debug.WriteLine(
                                error.Message + error.StackTrace);
                            //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                            myLog.WriteEntry(error.Message + error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
#endif
                            throw new System.Exception(error.Message, exception);
                        }
                    }


                    int CurrCommandOrder = 0;
                    do
                    {

                        int nextCommandOrder = ProcessCommands(CurrCommandOrder);
                        CurrCommandOrder = nextCommandOrder;

                    } while (CurrCommandOrder < LastCommandOrder + 1);

                    ProcessCommands(CurrCommandOrder);



                }
                catch (System.Exception Exception)
                {
                    System.Diagnostics.Debug.WriteLine(Exception.Message);
                    System.Diagnostics.Debug.WriteLine(Exception.StackTrace);
                    Abort();
                    throw;
                }
                finally
                {
                    lock (this)
                    {
                        if (_Slaves != null)
                            _Slaves.Clear();
                    }
                }
            }
        }


        ///<summary>
        ///This method realize the OOAdvantech.Transactions.ITransactionContextExtender.Commit
        ///operation. Snapshot storage instance state. and inform object stoarages of transaction commit; 
        ///</summary>
        /// <MetaDataID>{CD95CE57-79ED-436D-90B2-FEEF416A9AF3}</MetaDataID>
        public void Commit()
        {
            try
            {
                lock (this)
                {
                    if (_Slaves != null)
                    {
                        _Slaves.Clear();
                        _Slaves = null;
                    }
                    _Master = null;
                }




                //TODO Αυτά τα catch πνίγουν πολλά exception που στο τέλος ενδέχεται να κανουν  propagation error
                //που δέν το βρίσκεις εύκολα
                System.Collections.Generic.List<object> participatedStorageInstances = null;
                System.Collections.Generic.List<object> objectStoragesUnderTransaction = null;
                lock (this)
                {
                    participatedStorageInstances = ParticipatedStorageInstances.Values.OfType<object>().ToList();
                    ParticipatedStorageInstances.Clear();
                    objectStoragesUnderTransaction = ObjectStoragesUnderTransaction.Values.OfType<object>().ToList();
                    ObjectStoragesUnderTransaction.Clear();
                    PrioritizeEnlistedCommands.Clear();
                    _EnlistObjects.Clear();
                }
                System.Exception exception = null;
                foreach (StorageInstanceRef storageInstanceRef in participatedStorageInstances)
                {
                    try
                    {
                        // the system must be try to snapshot storage instance state for all objects
                        storageInstanceRef.SnapshotStorageInstance();
                    }
                    catch (System.Exception error)
                    {
                        exception = error;
#if !DeviceDotNet

                        System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                        myLog.Source = "PersistencySystem";
                        if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                            myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);
                        System.Diagnostics.Debug.WriteLine(
                            error.Message + error.StackTrace);
                        //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                        myLog.WriteEntry(error.Message + error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
#endif
                    }
                }

                foreach (ObjectStorage objectStorage in objectStoragesUnderTransaction)
                {
                    try
                    {
                        objectStorage.CommitChanges(this);
                    }
                    catch (System.Exception error)
                    {
                        exception = error;
#if !DeviceDotNet

                        System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                        myLog.Source = "PersistencySystem";
                        if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                            myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);
                        System.Diagnostics.Debug.WriteLine(
                            error.Message + error.StackTrace);
                        //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                        myLog.WriteEntry(error.Message + error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
#endif

                    }
                }
                if (exception != null)
                    throw new System.Exception(exception.Message, exception);


            }
            finally
            {
                State = ContextState.Commit;
            }

        }
        ///<summary>
        ///This method realize the OOAdvantech.Transactions.ITransactionContextExtender.Abort
        ///operation. Snapshot storage instance state. and inform object stoarages of transaction commit; 
        ///</summary>
        /// <MetaDataID>{841F08DF-C6D3-4CAA-8189-B83587889B11}</MetaDataID>
        public void Abort()
        {

            try
            {

                System.Collections.Generic.List<object> objectStoragesUnderTransaction = null;
                lock (this)
                {
                    objectStoragesUnderTransaction = new System.Collections.Generic.List<object>(ObjectStoragesUnderTransaction.Values);
                    ObjectStoragesUnderTransaction.Clear();
                    ParticipatedStorageInstances.Clear();
                    PrioritizeEnlistedCommands.Clear();
                    _EnlistObjects.Clear();
                }
                System.Exception exception = null;
                foreach (ObjectStorage objectStorage in objectStoragesUnderTransaction)
                {
                    try
                    {
                        objectStorage.AbortChanges(this);
                    }
                    catch (System.Exception error)
                    {
                        exception = error;
#if !DeviceDotNet

                        System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                        myLog.Source = "PersistencySystem";
                        if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                            myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);
                        System.Diagnostics.Debug.WriteLine(
                            error.Message + error.StackTrace);
                        //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                        myLog.WriteEntry(error.Message + error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
#endif
                    }
                }
                if (exception != null)
                    throw new System.Exception(exception.Message, exception);

            }
            finally
            {
                State = ContextState.Abort;
            }

        }
        #endregion



        /// <MetaDataID>{70F7B6F0-6C91-440C-B60E-AB4911530C4D}</MetaDataID>
        public void TransientToPersistent(object _object)
        {
            if (State == ContextState.Prepare)
                PrepareEnlistedObject(_object);
        }

        /// <MetaDataID>{A673C134-957A-460E-AF2C-74A959078CCD}</MetaDataID>
        internal void PrepareEnlistedObject(object memoryInstance)
        {
            StorageInstanceRef ObjStorageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(memoryInstance) as StorageInstanceRef;
            if (ObjStorageInstanceRef == null)
                return;
            AddParticipatedStorageInstance(ObjStorageInstanceRef);
            if (ObjStorageInstanceRef.PersistentObjectID == null)
            {
                ObjectStorage theObjectStorage = ObjStorageInstanceRef.ObjectStorage as ObjectStorage;
                theObjectStorage.CreateNewStorageInstanceCommand(ObjStorageInstanceRef);
            }
            else
            {
                PersistenceLayerRunTime.ObjectStorage theObjectStorage = ObjStorageInstanceRef.ObjectStorage as ObjectStorage;
                theObjectStorage.CreateUpdateStorageInstanceCommand(ObjStorageInstanceRef);
            }

        }

        ///<summary>
        ///Produce the initial command for the storage update.
        ///</summary>
        /// <MetaDataID>{9982B9DC-5834-4BA8-B99B-0CC90B557296}</MetaDataID>
        void PreparationStart()
        {
            if (State != ContextState.Active)
                return;
            System.Collections.Generic.List<object> enlistObjects = null;
            lock (_EnlistObjects)
            {
                enlistObjects = _EnlistObjects.ToList();
                State = ContextState.Prepare;
            }
            foreach (object memoryInstance in enlistObjects)
                PrepareEnlistedObject(memoryInstance);
        }


        /// <MetaDataID>{324EEED7-340F-4E06-B597-D981CBCCE7BE}</MetaDataID>
        private void ExecuteNextOrderCommands()
        {
            System.Collections.Generic.List<Commands.Command> commands = null;
            System.Collections.Generic.List<Commands.Command> cachCommands = null;
            lock (this)
            {
                PrioritizeEnlistedCommands.TryGetValue(NextCommandOrder, out commands);
                if (commands == null || commands.Count == 0)
                    return;
                cachCommands = commands.ToList();
            }
            foreach (Commands.Command command in cachCommands)
            {
                command.Execute();
                lock (this)
                {
                    commands.Remove(command);
                    _EnlistedCommands.Remove(command.Identity);
                    NumberOfCommands--;
                }
            }
        }





        /// <MetaDataID>{F8F1F8B4-E851-4D4B-B09B-91812906761A}</MetaDataID>
        public void AddParticipatedStorageInstance(PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef)
        {
            bool storageEnlistment = false;
            lock (this)
            {
                if (!ParticipatedStorageInstances.ContainsKey(StorageInstanceRef))
                    ParticipatedStorageInstances.Add(StorageInstanceRef, StorageInstanceRef);
                else
                    return;

                if (!ObjectStoragesUnderTransaction.ContainsKey(StorageInstanceRef.ObjectStorage))
                {
                    ObjectStoragesUnderTransaction.Add(StorageInstanceRef.ObjectStorage, StorageInstanceRef.ObjectStorage);
                    storageEnlistment = true;
                }
            }
            if (storageEnlistment)
                (StorageInstanceRef.ObjectStorage as PersistenceLayerRunTime.ObjectStorage).BeginChanges(this);
        }



        /// <MetaDataID>{5A65898D-7E21-4AEE-9A84-D0C470FEC147}</MetaDataID>
        private System.Collections.Generic.Dictionary<PersistenceLayerRunTime.StorageInstanceRef, PersistenceLayerRunTime.StorageInstanceRef> ParticipatedStorageInstances = new System.Collections.Generic.Dictionary<PersistenceLayerRunTime.StorageInstanceRef, PersistenceLayerRunTime.StorageInstanceRef>();

        //TODO PrioritizeEnlistedCommands θα πρέπει να είναι private
        /// <MetaDataID>{952BFDC6-ADF7-4E7C-B4A2-2FBEB1838305}</MetaDataID>
        public System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<Commands.Command>> PrioritizeEnlistedCommands = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<Commands.Command>>();

        /// <MetaDataID>{E564736D-8C9C-4C32-8A7A-AE1F8765864E}</MetaDataID>
        private System.Collections.Generic.Dictionary<OOAdvantech.PersistenceLayer.ObjectStorage, OOAdvantech.PersistenceLayer.ObjectStorage> ObjectStoragesUnderTransaction = new System.Collections.Generic.Dictionary<OOAdvantech.PersistenceLayer.ObjectStorage, OOAdvantech.PersistenceLayer.ObjectStorage>();


        //	/// <MetaDataID>{A846F52F-5CF4-4122-8401-2B0FAF45A724}</MetaDataID>
        //	private COMPlusTransaction TheMTSTransactionObject;


        /// <MetaDataID>{BA499B28-8A93-4BAF-A94F-CE9EA6B621A4}</MetaDataID>
        ~TransactionContext()
        {
        }
        /// <MetaDataID>{b34d168d-7f7d-4c03-9e32-41f4d9f6b4af}</MetaDataID>
        OOAdvantech.Transactions.ITransactionContext MainTransactionContext;
        /// <MetaDataID>{ca0a5343-6860-4f41-bf31-765e1771a132}</MetaDataID>
        public Transactions.Transaction Transaction;
        /// <MetaDataID>{A20B38E7-A1DB-42E0-9E47-1BCB5A1F17F4}</MetaDataID>
        public TransactionContext(Transactions.Transaction transaction)
        {
            try
            {
                Transaction = transaction;
                MainTransactionContext = OOAdvantech.Transactions.TransactionManager.GetTransactionContext(transaction);
#if !DeviceDotNet
                if (transaction.GlobalTransactionUri == null)
                    transaction.TransactionDistributed += new OOAdvantech.Transactions.TransactionDistributedEventHandler(OnTransactionDistributed);
#endif
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

#if !DeviceDotNet
        /// <MetaDataID>{84d82981-6654-43f6-880c-c257cef84c57}</MetaDataID>
        void OnTransactionDistributed(OOAdvantech.Transactions.Transaction transaction)
        {
            bool tryToAttachMaster = false;
            lock (this)
            {
                tryToAttachMaster = !System.String.IsNullOrEmpty(transaction.GlobalTransactionUri) && _EnlistObjects.Count > 0;
            }

            if (tryToAttachMaster)
            {
                OOAdvantech.PersistenceLayerRunTime.ITransactionContextManager persistencyService = Remoting.RemotingServices.CreateRemoteInstance("tcp://localhost:9060", typeof(PersistencyService).FullName, "") as OOAdvantech.PersistenceLayerRunTime.ITransactionContextManager;
                if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                    Master = persistencyService.GetMasterTransactionContext(transaction.Marshal());
            }

        }
#endif
        /// <MetaDataID>{C3CF6E4E-F415-4757-B015-73FAB8A12AB7}</MetaDataID>
        public bool ContainCommand(string Identity)
        {
            lock (this)
            {
                return _EnlistedCommands.ContainsKey(Identity);
            }

        }


        /// <MetaDataID>{4696E12A-60C1-492C-9656-1A5893CE027A}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, Commands.Command> EnlistedCommands
        {
            get
            {
                return _EnlistedCommands;
            }
        }



        /// <MetaDataID>{7e6d4739-cbed-422e-af3f-2c6713ebf82d}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, OOAdvantech.PersistenceLayerRunTime.Commands.Command> _EnlistedCommands = new System.Collections.Generic.Dictionary<string, Commands.Command>();

        /// <summary>All the change of the system in the frame of transaction produces transaction commands and enlists them in transaction through EnlistCommand method. </summary>
        /// <MetaDataID>{215EAEF2-21F0-4C3F-B863-7068BD146B98}</MetaDataID>
        public void EnlistCommand(Commands.Command command)
        {
            
            lock (this)
            {
                if (command.ExecutionOrder > LastCommandOrder)
                    throw new System.Exception("Command order out of range."); // message
                if (command.OwnerTransactiont != null)
                    throw new System.Exception("TransactionCommand has to belong to only one Transaction"); // message
                else
                    command.OwnerTransactiont = this;

                if (command.Identity == null || command.Identity.Trim().Length == 0)
                    throw new System.Exception("You can’t enlist command without identity.");

                if (_EnlistedCommands.ContainsKey(command.Identity))
                    throw new System.Exception("Command with equivalent functionality already exists.");
            }

            // TODO test case για την Διασφάλιση διπλοεγγραφής και ειδικά για delete storage instance command
            lock (this)
            {
                System.Collections.Generic.List<Commands.Command> commands = null;
                if (!PrioritizeEnlistedCommands.TryGetValue(command.ExecutionOrder, out commands))
                {
                    commands = new System.Collections.Generic.List<Commands.Command>();
                    PrioritizeEnlistedCommands[command.ExecutionOrder] = commands;
                }
                commands.Add(command);
                _EnlistedCommands.Add(command.Identity, command);
                NumberOfCommands++;
            }
        }
        /// <MetaDataID>{7A083FC0-FCE1-46F2-A51E-3374F7B6D812}</MetaDataID>
        public static readonly int LastCommandOrder = 100;
        /// <MetaDataID>{6F90F6F4-9D20-4F1C-8C35-2563666DF281}</MetaDataID>
        private void GetSubCommands(int currentOrder)
        {
            int numberOfNewCommands = 1;
            //Runs while no more produced commands
            do
            {
                int backUpNumberOfCommands = 0;
                int lastCommandOrder = 0;
                lock (this)
                {
                    backUpNumberOfCommands = NumberOfCommands;
                    lastCommandOrder = LastCommandOrder;
                }

                for (int i = 0; i <= lastCommandOrder; i++)
                {
                    System.Collections.Generic.List<Commands.Command> prioritizeEnlistedCommands = null;
                    lock (this)
                    {
                        PrioritizeEnlistedCommands.TryGetValue(i, out prioritizeEnlistedCommands);
                        if (prioritizeEnlistedCommands != null)
                            prioritizeEnlistedCommands = new System.Collections.Generic.List<Commands.Command>(prioritizeEnlistedCommands);
                    }
                    if (prioritizeEnlistedCommands != null)
                    {
                        foreach (Commands.Command command in prioritizeEnlistedCommands)
                            command.GetSubCommands(currentOrder);
                    }
                }
                lock (this)
                {
                    numberOfNewCommands = NumberOfCommands - backUpNumberOfCommands;
                }
            }
            while (numberOfNewCommands != 0);




        }
        /// <MetaDataID>{A00BCE52-DD2C-47C4-A7FC-0F60972DF438}</MetaDataID>
        private int NextCommandOrder
        {
            get
            {
                lock (this)
                {
                    for (int executionOrder = 0; executionOrder <= LastCommandOrder; executionOrder++)
                    {
                        System.Collections.Generic.List<Commands.Command> commands = null;
                        PrioritizeEnlistedCommands.TryGetValue(executionOrder, out commands);
                        if (commands != null && commands.Count > 0)
                            return executionOrder;
                    }
                    return LastCommandOrder + 1;
                }
            }

        }

        /// <MetaDataID>{D23C3008-3713-4FB0-8C6D-CEA1FD563787}</MetaDataID>
        public int ProcessCommands(int executionOrder)
        {
            if (MainTransactionContext.Status == OOAdvantech.Transactions.ObjectsStateManagerStatus.OnAction)
                return executionOrder;
#if !DeviceDotNet
            System.Security.Principal.WindowsIdentity windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (TransactioContextProvider.Impersonate)
                windowsIdentity = Transaction.TransactionInitiatorUser;
            using (windowsIdentity.Impersonate())
            {
#endif

            if (State == ContextState.Active)
                    PreparationStart();

                try
                {
                    int nextCommandOrder = 0;
#if !DeviceDotNet
                    #region Execute transaction context commands in current order
                    using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(OOAdvantech.Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                    {
                        GetSubCommands(executionOrder);
                        if (executionOrder < NextCommandOrder)
                        {
                            transactionScope.Complete();
                            nextCommandOrder = NextCommandOrder;
                        }
                        else
                        {
                            ExecuteNextOrderCommands();
                            bool throwExeption = false;
                            if (throwExeption)
                                throw new System.Exception("RollBack");
                            transactionScope.Complete();
                            nextCommandOrder = NextCommandOrder;
                        }
                    }
                    #endregion


                    #region Slaves transaction context, execute commands in current order
                    System.Collections.Generic.List<ITransactionContext>  slaves = null;
                    lock (this)
                    {
                        if (_Slaves != null)
                            slaves = new System.Collections.Generic.List<ITransactionContext> ( _Slaves);
                    }

                    if (slaves != null)
                    {
                        int lowestCommandOrderinCommandCollection = nextCommandOrder;
                        foreach (ITransactionContext transactionContext in slaves)
                        {
                            lowestCommandOrderinCommandCollection = transactionContext.ProcessCommands(executionOrder);
                            if (nextCommandOrder > lowestCommandOrderinCommandCollection)
                                nextCommandOrder = lowestCommandOrderinCommandCollection;
                        }
                    }
                    return nextCommandOrder;
                    #endregion

#else
                GetSubCommands(executionOrder);
                if (executionOrder < NextCommandOrder)
                {
                    return NextCommandOrder;
                }
                ExecuteNextOrderCommands();
                bool throwExeption = false;
                if (throwExeption)
                    throw new System.Exception("RollBack");
                return NextCommandOrder;
#endif
                }
                catch (System.Exception Error)
                {
                    string stackTrace = Error.StackTrace;
                    Transactions.Transaction.Current.Abort(Error);
                    throw;
                }
                finally
                {
                    if (executionOrder == LastCommandOrder + 1)
                    {
                        State = ContextState.InDoubt;
                        foreach (var dictionaryEntry in ObjectStoragesUnderTransaction)
                        {
                            ObjectStorage objectStorage = dictionaryEntry.Value as ObjectStorage;
                            objectStorage.MakeChangesDurable(this);
                        }
                    }
                }
            }

#if !DeviceDotNet
        }
#endif

    }
    /// <MetaDataID>{dc530f7f-5869-47c6-8d16-ed73af825f9b}</MetaDataID>
    public class NestedTransactionContext : MarshalByRefObject, Remoting.IExtMarshalByRefObject, ITransactionContext
    {
        /// <MetaDataID>{93b82dc9-e8c5-4391-b2d1-365ba4bcebdd}</MetaDataID>
        Transactions.Transaction Transaction;
        /// <MetaDataID>{70a2d6ec-d363-41d3-a2aa-e52b85252a28}</MetaDataID>
        public NestedTransactionContext(Transactions.Transaction transaction)
        {
            Transaction = transaction;
        }
#region ITransactionContextExtender Members

        /// <MetaDataID>{cc262e0e-f253-448d-83bd-8195356d5c53}</MetaDataID>
        internal System.Collections.Generic.List<object> _EnlistObjects = new System.Collections.Generic.List<object>();
        /// <MetaDataID>{77b34481-29ff-4acb-a9b8-b8d2974e88dd}</MetaDataID>
        public System.Collections.Generic.List<object> EnlistObjectsf
        {
            get
            {
                System.Collections.Generic.List<object> enlistObjects = _EnlistObjects.ToList();

                Transactions.Transaction transaction = Transaction.OriginTransaction;
                while (transaction != null)
                {
                    foreach (Transactions.ITransactionContextExtender extender in Transactions.TransactionManager.GetTransactionContext(transaction).Extenders)
                    {
                        if (extender is PersistenceLayerRunTime.ITransactionContext)
                        {
                            enlistObjects.AddRange((extender as PersistenceLayerRunTime.ITransactionContext).EnlistObjects);
                            break;
                        }
                    }
                    transaction = transaction.OriginTransaction;
                }

                return enlistObjects;
                
            }
        }
        /// <MetaDataID>{7b09face-2cca-40e5-9cbd-fb418d9fe535}</MetaDataID>
        public void EnlistObject(object transactionedObject, System.Reflection.FieldInfo fieldInfo)
        {
            if (ObjectStorage.PersistencyService.ClassOfObjectIsPersistent(transactionedObject))
                _EnlistObjects.Add(transactionedObject);
        }

        /// <MetaDataID>{3f0ac25a-4cdc-4fdb-87e1-702cc79543c9}</MetaDataID>
        public void Abort()
        {
            _EnlistedCommands.Clear();
            _EnlistObjects.Clear();
            Transaction = null;
        }

        /// <MetaDataID>{82b6889d-6987-4c6d-932c-dd3fdcb82928}</MetaDataID>
        public void Commit()
        {

            
            foreach (Transactions.ITransactionContextExtender extender in Transactions.TransactionManager.GetTransactionContext(Transaction.OriginTransaction).Extenders)
            {
                if (extender is PersistenceLayerRunTime.ITransactionContext)
                {
                    foreach (Commands.Command command in EnlistedCommands.Values)
                    {
                        command.OwnerTransactiont = null;
                        (extender as PersistenceLayerRunTime.ITransactionContext).EnlistCommand(command);
                    }
                    return;
                }
            }
            _EnlistObjects.Clear();
            Transaction = null;

        }

        /// <MetaDataID>{eb8e8a88-22ad-412a-b083-41cf30bbe3b9}</MetaDataID>
        public void Prepare()
        {
        }

#endregion

        /// <MetaDataID>{47967f6e-7975-43e5-85dc-87afdf16f004}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, Commands.Command> EnlistedCommands
        {
            get
            {
                return _EnlistedCommands;
            }
        }

        public List<object> EnlistObjects
        {
           
           get
            {
                    System.Collections.Generic.List<object> enlistObjects = _EnlistObjects.ToList();

                    Transactions.Transaction transaction = Transaction.OriginTransaction;
                    while (transaction != null)
                    {
                        foreach (Transactions.ITransactionContextExtender extender in Transactions.TransactionManager.GetTransactionContext(transaction).Extenders)
                        {
                            if (extender is PersistenceLayerRunTime.ITransactionContext)
                            {
                                enlistObjects.AddRange((extender as PersistenceLayerRunTime.ITransactionContext).EnlistObjects);
                                break;
                            }
                        }
                        transaction = transaction.OriginTransaction;
                    }

                    return enlistObjects;

                }
            }
        

        /// <MetaDataID>{11b6cc98-f9a8-4a06-8e5b-75cd0c27277d}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, Commands.Command> _EnlistedCommands = new System.Collections.Generic.Dictionary<string, OOAdvantech.PersistenceLayerRunTime.Commands.Command>();

#region ITransactionContext Members

        /// <MetaDataID>{20387acb-5260-4650-833b-88d155491c5d}</MetaDataID>
        public void EnlistCommand(OOAdvantech.PersistenceLayerRunTime.Commands.Command command)
        {
            lock (this)
            {
                if (command.OwnerTransactiont != null)
                    throw new System.Exception("TransactionCommand has to belong to only one Transaction"); // message
                else
                    command.OwnerTransactiont = this;

                if (command.Identity == null || command.Identity.Trim().Length == 0)
                    throw new System.Exception("You can’t enlist command without identity.");

                if (_EnlistedCommands.ContainsKey(command.Identity))
                    throw new System.Exception("Command with equivalent functionality already exists.");
                _EnlistedCommands.Add(command.Identity, command);
            }
        }
        /// <MetaDataID>{e5782f18-481c-438c-9e8e-209adc7dc80d}</MetaDataID>
        public bool ContainCommand(string Identity)
        {
            lock (this)
            {
                return _EnlistedCommands.ContainsKey(Identity);
            }
        }

        public void AddSlaveTransactionContext(ITransactionContext transactionContext)
        {
            throw new NotImplementedException();
        }
        public int ProcessCommands(int executionOrder)
        {
            throw new NotImplementedException();
        }

#endregion
    }
}
