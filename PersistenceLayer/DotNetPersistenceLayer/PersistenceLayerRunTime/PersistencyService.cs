using System;
using System.Linq;

namespace OOAdvantech.PersistenceLayerRunTime
{

    using System;
    using System.Collections;
    using OOAdvantech.Transactions;
    using System.Reflection;
    using PersistenceLayer;

    /// <summary>
    /// </summary>
    /// <MetaDataID>{C5D008A5-2E9B-4A4C-86B0-54955B46FF90}</MetaDataID>
    public class PersistencyService : Remoting.MonoStateClass, OOAdvantech.PersistenceLayer.IPersistencyService, Remoting.IPersistentObjectLifeTimeController, ITransactionContextManager
    {

#if !DeviceDotNet
        /// <MetaDataID>{1f59303f-85d8-4987-88f9-054ed2f027e3}</MetaDataID>
        static PersistencyService GetPersistencyService(OOAdvantech.Transactions.Transaction transaction)
        {

            string hostAddress = Remoting.RemotingServices.GetHostAddress(Transactions.TransactionManager.GetTransactionChannelUri(transaction));
            //port 9060
            return Remoting.RemotingServices.CreateRemoteInstance("tcp://" + hostAddress + ":9060", typeof(PersistencyService).FullName, "") as PersistencyService;
        }
#endif


        /// <MetaDataID>{cd5811cb-8049-4b98-a8da-85dc0de53419}</MetaDataID>
        public TransactionContext GetMasterTransactionContext(Transactions.Transaction transaction)
        {

#if !DeviceDotNet
            foreach (Transactions.ITransactionContextExtender transactionContextExtender in Transactions.TransactionManager.GetTransactionContext(transaction).Extenders)
            {
                if (transactionContextExtender is ITransactionContext)
                {
                    OOAdvantech.PersistenceLayerRunTime.ITransactionContextManager transacionInitiatorPersistencyService = GetPersistencyService(transaction);
                    if (transacionInitiatorPersistencyService != this)
                        (transactionContextExtender as TransactionContext).Master = transacionInitiatorPersistencyService.GetMasterTransactionContext(transaction.Marshal());
                    return transactionContextExtender as TransactionContext;
                }
            }
#endif
            return null;

        }

        /// <MetaDataID>{cd5811cb-8049-4b98-a8da-85dc0de53419}</MetaDataID>
        public ITransactionContext GetMasterTransactionContext(string globalTransactionUri)
        {
#if !DeviceDotNet
            Transaction transaction = Transactions.TransactionManager.UnMarshal(globalTransactionUri);
            foreach (Transactions.ITransactionContextExtender transactionContextExtender in Transactions.TransactionManager.GetTransactionContext(transaction).Extenders)
            {
                if (transactionContextExtender is ITransactionContext)
                {
                    OOAdvantech.PersistenceLayerRunTime.ITransactionContextManager transacionInitiatorPersistencyService = GetPersistencyService(transaction);
                    if (transacionInitiatorPersistencyService != this)
                        (transactionContextExtender as TransactionContext).Master = transacionInitiatorPersistencyService.GetMasterTransactionContext(transaction.Marshal());
                    return transactionContextExtender as TransactionContext;
                }
            }
#endif

            return null;

        }

        /// <MetaDataID>{a9952863-47fa-4adc-99a9-7d1849072e89}</MetaDataID>
        public void GetStorageInstancePersistentObjectID(object _object, out OOAdvantech.PersistenceLayer.ObjectID objectID, out string storageIdentity)
        {
            StorageInstanceAgent storageInstanceAgent = GetStorageInsatnceAgent(_object);
            if (storageInstanceAgent == null)
            {
                objectID = OOAdvantech.PersistenceLayer.ObjectID.Empty;
                storageIdentity = "";
            }
            else
            {
                objectID = (OOAdvantech.PersistenceLayer.ObjectID)storageInstanceAgent.PersistentObjectID;
                storageIdentity = storageInstanceAgent.StorageIdentity;
            }
        }

        /// <MetaDataID>{2675d26c-b9fe-4e27-865f-98953e83c479}</MetaDataID>
        public void GetStorageInstanceObjectID(object _object, out OOAdvantech.PersistenceLayer.ObjectID objectID, out string storageIdentity)
        {
            StorageInstanceAgent storageInstanceAgent = GetStorageInsatnceAgent(_object);

            if (storageInstanceAgent == null)
            {
                objectID = OOAdvantech.PersistenceLayer.ObjectID.Empty;
                storageIdentity = "";
            }
            else
            {
                objectID = (OOAdvantech.PersistenceLayer.ObjectID)storageInstanceAgent.ObjectID;
                storageIdentity = storageInstanceAgent.StorageIdentity;
            }
        }


        /// <MetaDataID>{51DFFF68-F74D-47EF-A707-F8335B8A4216}</MetaDataID>
        public StorageInstanceAgent GetStorageInsatnceAgent(object _object)
        {
#if !DeviceDotNet
            if (_object is System.MarshalByRefObject && OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_object as System.MarshalByRefObject))
            {
                Remoting.RemotingServices remotingServices = Remoting.RemotingServices.GetRemotingServices(OOAdvantech.Remoting.RemotingServices.GetChannelUri(_object as System.MarshalByRefObject)) as Remoting.RemotingServices;
                PersistencyService persistencyService = (PersistencyService)remotingServices.CreateInstance(typeof(PersistencyService).ToString(), "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b");
                return persistencyService.GetStorageInsatnceAgent(_object);
            }
            else

            {
                if (ClassOfObjectIsPersistent(_object))
                {
                    StorageInstanceRef storageInstanceRef = null;
                    storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(_object) as StorageInstanceRef;
                    if (storageInstanceRef == null)
                        return null;
                    return new StorageInstanceAgent(storageInstanceRef);
                }
                else
                    return null;
            }
#else
            if (ClassOfObjectIsPersistent(_object))
            {
                StorageInstanceRef storageInstanceRef = null;
                storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(_object) as StorageInstanceRef;
                if (storageInstanceRef == null)
                    return null;
                return new StorageInstanceAgent(storageInstanceRef);
            }
            else
                return null;
#endif

        }
        /// <MetaDataID>{4FAF655B-159E-4C97-A30A-2CEE4F5ECDA1}</MetaDataID>
        public PersistenceLayer.ObjectStorage GetStorageOfObject(object _object)
        {
#if !DeviceDotNet

            if (_object is System.MarshalByRefObject && OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(_object as System.MarshalByRefObject))
            {
                Remoting.RemotingServices remotingServices = Remoting.RemotingServices.GetRemotingServices(OOAdvantech.Remoting.RemotingServices.GetChannelUri(_object as System.MarshalByRefObject)) as Remoting.RemotingServices;
                PersistencyService persistencyService = (PersistencyService)remotingServices.CreateInstance(typeof(PersistencyService).ToString(), "PersistenceLayerRunTime, Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b");
                PersistenceLayer.ObjectStorage serverSideStorageSession = persistencyService.GetStorageOfObject(_object);
                if (serverSideStorageSession != null)
                    return new ClientSide.ObjectStorageAgent(serverSideStorageSession);
                else
                    return null;
            }
            else
            {
                if (ClassOfObjectIsPersistent(_object))
                    return ObjectStorage.GetStorageOfObject(_object);
                else
                    throw new System.Exception("The class '" + _object.GetType().FullName + "' isn't persistent");
            }
#else
            if (ClassOfObjectIsPersistent(_object))
                return ObjectStorage.GetStorageOfObject(_object);
            else
                throw new System.Exception("The class '" + _object.GetType().FullName + "' isn't persistent");
#endif

        }

        /// <MetaDataID>{ac2a4da2-e42e-4902-ae1f-8fafa4dc0800}</MetaDataID>
        public bool IsPersistent(object memoryInstance)
        {
#if !DeviceDotNet

            if (memoryInstance is System.MarshalByRefObject && OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(memoryInstance as System.MarshalByRefObject))
            {
                Remoting.RemotingServices remotingServices = Remoting.RemotingServices.GetRemotingServices(OOAdvantech.Remoting.RemotingServices.GetChannelUri(memoryInstance as System.MarshalByRefObject)) as Remoting.RemotingServices;
                PersistencyService persistencyService = (PersistencyService)remotingServices.CreateInstance(typeof(PersistencyService).ToString(), "PersistenceLayerRunTime,  Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b");
                return persistencyService.IsPersistent(memoryInstance);
            }
            else
            {
                if (ClassOfObjectIsPersistent(memoryInstance))
                {
                    PersistenceLayer.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(memoryInstance);
                    if (storageInstanceRef == null)
                        return false;
                    else
                        return storageInstanceRef.IsPersistent;
                }
                else
                    return false;
            }
#else
            if (ClassOfObjectIsPersistent(memoryInstance))
            {
                PersistenceLayer.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(memoryInstance);
                if (storageInstanceRef == null)
                    return false;
                else
                    return storageInstanceRef.IsPersistent;
            }
            else
                return false;
#endif

        }

        /// <MetaDataID>{178E0B2B-5427-4E7A-A589-584D68CE76E5}</MetaDataID>
        public bool ClassOfObjectIsPersistent(object memoryInstance)
        {

            bool persistent = false;
            if (memoryInstance == null)
                return false;
            MetaDataRepository.Class Class = MetaDataRepository.Classifier.GetClassifier(memoryInstance.GetType()) as MetaDataRepository.Class;// DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(memoryInstance.GetType()) as MetaDataRepository.Class;
            if (Class == null)
                return memoryInstance.GetType().GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.Persistent), true).Length > 0;
            else
                return Class.Persistent;

            return persistent;
        }

        ///// <MetaDataID>{5981331F-8409-48E0-AEBF-44F90698E4A0}</MetaDataID>
        //static PersistencyService MonoStatePersistencyService; 
        /// <MetaDataID>{D44270B1-A429-442E-996F-7A2C0CBF626E}</MetaDataID>
        bool IsInServerInstance = false;
        /// <MetaDataID>{97D159C1-C1DE-406D-A9A4-E97ECCFEFAFE}</MetaDataID>
        public PersistencyService()
        {
            InitOodtc();
            InitPersistencyService(null);
        }

        static bool ServiceForInstanceExist(String instanceName)
        {
#if !DeviceDotNet
            if (instanceName == null || instanceName.Trim().Length == 0)
                return false;
            string serviceName;
            if (instanceName.Trim().ToLower() == "default")
                serviceName = "StorageServer";
            else
                serviceName = "StorageServer$" + instanceName;

            return System.ServiceProcess.ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == serviceName) != null;
#endif
            return true;
        }

        /// <MetaDataID>{bd396910-00ea-4721-b057-3070d0025e5e}</MetaDataID>
        static bool InitOodtc()
        {
#if !DeviceDotNet
            if (ServiceForInstanceExist("OOAdvantechTransactionCoordinator"))
            {
                System.ServiceProcess.ServiceController control = new System.ServiceProcess.ServiceController("OOAdvantechTransactionCoordinator");
                if (control.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                    control.Start();
                else if (control.Status == System.ServiceProcess.ServiceControllerStatus.Paused)
                    control.Continue();
                return true;
            }
            else
                return false;
#endif

            return true;
        }
        /// <MetaDataID>{f852947f-d358-4360-bb57-31857c251826}</MetaDataID>
        string ServerInstanceName;
        /// <MetaDataID>{2c1b1766-c730-4e88-9982-489f6ff9dce4}</MetaDataID>
        public void InitPersistencyService(string serverInstanceName)
        {
            //if (MonoStatePersistencyService != null)
            //    return;
            //MonoStatePersistencyService = this;
            try
            {
                #region Initialize for storage server instance
                if (serverInstanceName != null && serverInstanceName.Trim().Length > 0)
                {
                    IsInServerInstance = true;
                    ServerInstanceName = serverInstanceName.Trim().ToLower();
                }
                #endregion

#if !DeviceDotNet
                #region Initialize remoting system for the persistent object life time control
                System.Reflection.FieldInfo PersistentObjectLifeTimeControllerField =
                    typeof(Remoting.RemotingServices).GetField("PersistentObjectLifeTimeController",
                  BindingFlags.Static
                  | BindingFlags.NonPublic
                  | BindingFlags.FlattenHierarchy
                  | BindingFlags.Public);
                PersistentObjectLifeTimeControllerField.SetValue(null, this);
                #endregion

                #region Loads metadata for assemblies of current application domain
                using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition())
                {
                    foreach (System.Reflection.Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        try
                        {
                            //TODO ’‹Ò˜ÂÈ ÂÒﬂÙ˘ÛÁ Ì· ÛÁÍ˘ËÂﬂ ÏÈ· Assembly ‰˝Ô ˆÔÒ›Ú
                            DotNetMetaDataRepository.Assembly MetaAssembly = DotNetMetaDataRepository.Assembly.GetComponent(assembly) as DotNetMetaDataRepository.Assembly;
                            //MetaAssembly=(DotNetMetaDataRepository.Assembly)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(new OOAdvantech.MetaDataRepository.MetaObjectID(assembly.FullName), assembly);
                            //if (MetaAssembly == null)
                            //    MetaAssembly = new DotNetMetaDataRepository.Assembly(assembly);
                        }
                        catch (System.Exception mException)
                        {
                            System.Diagnostics.Debug.WriteLine(mException.StackTrace);
                            throw new System.Exception(mException.Message, mException);
                            int lo = 0;
                        }
                    }
                    stateTransition.Consistent = true;
                }
                #endregion
#endif
                Transactions.TransactionManager.RegisterTransactionContextProvider(new TransactioContextProvider());
            }
            catch (System.Exception Error)
            {
#if !DeviceDotNet
                #region Publish error computer on event viewer
                if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                    System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "PersistencySystem";
                if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                    myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);

                System.Diagnostics.Debug.WriteLine(
                    Error.Message + Error.StackTrace);
                myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                if (Error is Transactions.AbortException)
                {
                    Transactions.AbortException transError = Error as Transactions.AbortException;
                    if (transError.AbortReasons != null && transError.AbortReasons.Count > 0)
                    {
                        System.Exception exception = transError.AbortReasons[0] as System.Exception;
                        while (exception.InnerException != null)
                            exception = exception.InnerException;
                        //TODO Ì· ‰È·˜ÂÈÒﬂÊ˘ÏÂ Û˘ÛÙ‹ ÙÔ log file ÏÁ˘Ú Í·È Í‹ÌÂÈ overflow
                        myLog.WriteEntry(exception.Message + exception.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
                #endregion
#endif

                throw new System.Exception(Error.Message, Error);
            }
        }

        /// <MetaDataID>{83099592-F600-4A18-984D-8DD16DF81D8E}</MetaDataID>
        public PersistencyService(string serverInstanceName)
        {
            InitOodtc();
            InitPersistencyService(serverInstanceName);
        }


        /// <MetaDataID>{b75003d6-de13-49e8-b226-12427a9238ee}</MetaDataID>
        static bool IsTheLocalMachine(string computerName)
        {


#if !DeviceDotNet
            return Remoting.RemotingServices.IsTheLocalMachine(computerName);
#else
            return true;
#endif
        }

#if !DeviceDotNet
        /// <MetaDataID>{4473C20E-1661-422B-8F6F-F9B500EF465A}</MetaDataID>
        PersistenceLayer.IPersistencyService GetPersistencyServiceOnMachine(string computerName, string instanceName)
        {
            try
            {


                bool isTheLocalMachine = IsTheLocalMachine(computerName);
                if (isTheLocalMachine && instanceName.Trim().ToLower() == ServerInstanceName)
                    return this;
                else if (isTheLocalMachine)
                {
                    Remoting.RemotingServices theLocalRemotingServices = Remoting.RemotingServices.GetRemotingServices("tcp://localhost:9060") as Remoting.RemotingServices;

                    //object oob = theLocalRemotingServices.CreateInstance(typeof(OOAdvantech.ObjectIDTest).ToString(), typeof(OOAdvantech.ObjectIDTest).Assembly.GetName().Name);
                    //object mer = oob.GetObject();

                    PersistenceLayer.StorageServerInstanceLocator locator = theLocalRemotingServices.CreateInstance(typeof(PersistenceLayer.StorageServerInstanceLocator).ToString(), typeof(PersistenceLayer.StorageServerInstanceLocator).Assembly.GetName().Name) as PersistenceLayer.StorageServerInstanceLocator;

                    //PersistenceLayer.StorageServerInstanceLocator locator = OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.GetStorageServerInstanceLocator();
                    return locator.GetPersistencyService(instanceName);
                }
                else
                {
                    Remoting.RemotingServices theLocalRemotingServices = Remoting.RemotingServices.GetRemotingServices("tcp://" + computerName + ":9060") as Remoting.RemotingServices;
                    PersistenceLayer.StorageServerInstanceLocator locator = theLocalRemotingServices.CreateInstance(typeof(PersistenceLayer.StorageServerInstanceLocator).ToString(), typeof(PersistenceLayer.StorageServerInstanceLocator).Assembly.FullName) as PersistenceLayer.StorageServerInstanceLocator;
                    return locator.GetPersistencyService(instanceName);
                }
            }
            catch (System.Exception Error)
            {
                if (instanceName != null && instanceName.Length > 0)
                    throw new OOAdvantech.PersistenceLayer.StorageException(@"System can't connect with storage server '\\" + computerName + @"\" + instanceName + "' .",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.CanotConnectWithStorage, Error);
                else
                    throw new OOAdvantech.PersistenceLayer.StorageException(@"System can't connect with storage server '\\" + computerName + "' .",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.CanotConnectWithStorage, Error);

            }
        }
#endif



        /// <MetaDataID>{E5ED2860-59D1-4406-A36E-86B16D902151}</MetaDataID>
        public PersistenceLayer.ObjectStorage NewStorage(PersistenceLayer.Storage storageMetadata, string storageName, string storageLocation, string storageType, bool InProcess, string userName = "", string password = "")
        {
            //#region MonoStateClass
            //if(this!=MonoStatePersistencyService) 
            //    return MonoStatePersistencyService.NewStorage(storageMetadata,storageName,storageLocation,storageType,InProcess);
            //#endregion

            #region Gets storage provider for the type of storage
            StorageProvider storageProvider = null;
            try
            {
                storageProvider = GetStorageProvider(storageType);
                //AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }
            #endregion

            #region Gets some important informationís for the storage like host machine, server instance etc.
            string storageHostComputerName = storageProvider.GetHostComuterName(storageName, storageLocation).Trim().ToLower();
            string instanceName = storageProvider.GetInstanceName(storageName, storageLocation).Trim().ToLower();
            InProcess |= storageProvider.IsEmbeddedStorage(storageName, storageLocation);
            #endregion

            #region Creates storage and load in server instance other than this and return the new storage
            if (!InProcess)
            {
#if !DeviceDotNet
                PersistenceLayer.IPersistencyService persistencyService = GetPersistencyServiceOnMachine(storageHostComputerName, instanceName);
                if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                    return new ClientSide.ObjectStorageAgent(persistencyService.NewStorage(storageMetadata, storageName, storageLocation, storageType, InProcess, userName, password));
#endif

            }
            #endregion

            #region Creates storage and load in this server instance and return the new storage.

            lock (OpenStorages)
            {
                if (OpenStorages.ContainsKey(storageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower()))
                    throw new OOAdvantech.PersistenceLayer.StorageException(string.Format(@"Storage with name '{0}' at '{1}", storageName, storageLocation),
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageAlreadyExist);

                PersistenceLayer.ObjectStorage newStorage = storageProvider.NewStorage(storageMetadata, storageName, storageLocation, userName, password);
                OpenStorages[storageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower()] = newStorage;

                if (StorageServer != null && !InProcess && !storageProvider.IsEmbeddedStorage(storageName, storageLocation) && !StorageServer.ExisStorageReference(storageName, storageType, storageLocation))
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        MetaDataRepository.StorageReference storageReference = new OOAdvantech.MetaDataRepository.StorageReference();
                        storageReference.Name = storageName;
                        storageReference.StorageName = storageName;
                        storageReference.StorageType = storageType;
                        storageReference.StorageLocation = storageLocation;

                        PersistenceLayer.ObjectStorage.GetStorageOfObject(StorageServer).CommitTransientObjectState(storageReference);
                        StorageServer.AddStorage(storageReference);
                        stateTransition.Consistent = true;
                    }

                }

                return newStorage;
            }
            #endregion
        }
        public void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool InProcess, string userName = "", string password = "", bool overrideObjectStorage = false)
        {


            #region Gets storage provider for the type of storage
            StorageProvider storageProvider = null;
            try
            {
                storageProvider = GetStorageProvider(storageType);
                //AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }
            #endregion

            #region Gets some important informationís for the storage like host machine, server instance etc.
            string storageHostComputerName = storageProvider.GetHostComuterName(storageName, storageLocation).Trim().ToLower();
            string instanceName = storageProvider.GetInstanceName(storageName, storageLocation).Trim().ToLower();
            InProcess |= storageProvider.IsEmbeddedStorage(storageName, storageLocation);
            #endregion

            #region Creates storage and load in server instance other than this and return the new storage
            if (!InProcess)
            {
#if !DeviceDotNet
                PersistenceLayer.IPersistencyService persistencyService = GetPersistencyServiceOnMachine(storageHostComputerName, instanceName);
                if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                {
                    persistencyService.Restore(archive, storageName, storageLocation, storageType, InProcess, userName, password, overrideObjectStorage);
                    return;
                }
#endif

            }
            #endregion

            #region Creates storage and load in this server instance and return the new storage.

            storageProvider.Restore(archive, storageName, storageLocation, storageType, InProcess, userName, password, overrideObjectStorage);

            #endregion

        }

        public void Repair(string storageName, string storageLocation, string storageType, bool InProcess, string userName = "", string password = "")
        {


            #region Gets storage provider for the type of storage
            StorageProvider storageProvider = null;
            try
            {
                storageProvider = GetStorageProvider(storageType);
                //AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }
            #endregion

            #region Gets some important informationís for the storage like host machine, server instance etc.
            string storageHostComputerName = storageProvider.GetHostComuterName(storageName, storageLocation).Trim().ToLower();
            string instanceName = storageProvider.GetInstanceName(storageName, storageLocation).Trim().ToLower();
            InProcess |= storageProvider.IsEmbeddedStorage(storageName, storageLocation);
            #endregion

            #region Creates storage and load in server instance other than this and return the new storage
            if (!InProcess)
            {
#if !DeviceDotNet
                PersistenceLayer.IPersistencyService persistencyService = GetPersistencyServiceOnMachine(storageHostComputerName, instanceName);
                if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                {
                    persistencyService.Repair(storageName, storageLocation, storageType, InProcess, userName, password);
                    return;
                }
#endif

            }
            #endregion

            #region Creates storage and load in this server instance and return the new storage.

            storageProvider.Repair(storageName, storageLocation, storageType, InProcess, userName, password);

            #endregion

        }

        /// <MetaDataID>{8E2833E3-9956-407E-9052-5B398501253D}</MetaDataID>
        private System.Collections.Generic.Dictionary<object, PersistenceLayer.ObjectStorage> OpenStorages = new System.Collections.Generic.Dictionary<object, PersistenceLayer.ObjectStorage>();

        private System.Collections.Generic.Dictionary<string, IRawStorageData> RawStoragesData = new System.Collections.Generic.Dictionary<string, IRawStorageData>();





        /// <MetaDataID>{31518FEF-9480-4C61-8040-3FB09F19E68F}</MetaDataID>
        public MetaDataRepository.StorageServer GetStorageServer(string storageServerLocation)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                //#region MonoStateClass
                //if(this!=MonoStatePersistencyService) 
                //    return MonoStatePersistencyService.OpenStorage(storageName,storageLocation,storageType);
                //#endregion


                string instanceName = GetInstanceName(storageServerLocation);
                string storageHostComputerName = GetHostComuterName(storageServerLocation).Trim().ToLower();
#if !DeviceDotNet
                PersistenceLayer.IPersistencyService persistencyService = GetPersistencyServiceOnMachine(storageHostComputerName, instanceName);
                if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                    return persistencyService.GetStorageServer(storageServerLocation);
#endif

                return StorageServer;
            }
        }

        /// <MetaDataID>{0f8926a8-3380-4693-b743-5c9b45caa53d}</MetaDataID>
        public string GetInstanceName(string storageLocation)
        {
            int npos = storageLocation.IndexOf('\\');
            if (npos == -1)
                return "default";
            else
                return storageLocation.Substring(npos + 1);
        }
        /// <MetaDataID>{6275df8b-4487-47a4-b64a-71cd02de7a5b}</MetaDataID>
        public string GetHostComuterName(string StorageLocation)
        {
            int npos = StorageLocation.IndexOf('\\');
            if (npos == -1)
                return StorageLocation;
            else
                return StorageLocation.Substring(0, npos);
        }





        /// <MetaDataID>{31518FEF-9480-4C61-8040-3FB09F19E68F}</MetaDataID>
        public PersistenceLayer.ObjectStorage OpenStorage(string storageName, string storageLocation, string storageType, string userName = "", string password = "")
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                //#region MonoStateClass
                //if(this!=MonoStatePersistencyService) 
                //    return MonoStatePersistencyService.OpenStorage(storageName,storageLocation,storageType);
                //#endregion


                #region Gets storage provider for the type of storage
                StorageProvider storageProvider = GetStorageProvider(storageType);
                #endregion

                #region Gets some important informationís for the storage like host machine, server instance etc.
                bool embedded = storageProvider.IsEmbeddedStorage(storageName, storageLocation);

                string instanceName = storageProvider.GetInstanceName(storageName, storageLocation);
                #endregion

                #region Gets storage from machine and server instance which host the storage
                if (!embedded)
                {

#if !DeviceDotNet
                    string storageHostComputerName = storageProvider.GetHostComuterName(storageName, storageLocation).Trim().ToLower();
                    PersistenceLayer.IPersistencyService persistencyService = GetPersistencyServiceOnMachine(storageHostComputerName, instanceName);
                    if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                    {

                        return new ClientSide.ObjectStorageAgent(persistencyService.OpenStorage(storageName, storageLocation, storageType, userName, password));
                    }
#endif
                }
                #endregion

                #region Load storage locally if must be hosted in this process.
                lock (OpenStorages)
                {
                    if (OpenStorages.ContainsKey(storageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower()))
                    {
                        ObjectStorage objectStorage = (ObjectStorage)OpenStorages[storageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower()];
                        if (objectStorage.StorageMetaData != null && objectStorage.StorageMetaData.StorageType == storageType)
                            return objectStorage;
                    }

                    System.DateTime start = System.DateTime.UtcNow;

                    PersistenceLayer.ObjectStorage OpenedStorageSession = storageProvider.OpenStorage(storageName, storageLocation, userName, password);
                    
                    System.Diagnostics.Debug.WriteLine($"#@% {System.DateTime.Now.ToLongTimeString()} Open  Storage : {storageName} in :   {(System.DateTime.UtcNow-start).TotalMilliseconds}");

                    MetaDataRepository.StorageMetaData storageMetaData = new MetaDataRepository.StorageMetaData()
                    {
                        StorageIdentity = OpenedStorageSession.StorageMetaData.StorageIdentity,
                        StorageLocation = OpenedStorageSession.StorageMetaData.StorageLocation,
                        StorageType = OpenedStorageSession.StorageMetaData.StorageType,
                        StorageName = OpenedStorageSession.StorageMetaData.StorageName,
                        NativeStorageID = OpenedStorageSession.StorageMetaData.NativeStorageID,
                        MultipleObjectContext = true
                    };
#if !DeviceDotNet
                    PersistenceLayer.StorageServerInstanceLocator.Current.PublishStorageMetaData(storageMetaData);
#endif

                    OpenStorages[storageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower()] = OpenedStorageSession; // error prone;
                    return OpenedStorageSession;
                }
                #endregion

                stateTransition.Consistent = true;
            }

        }

        public void UpdateOperativeObjects(string storageIdentity)
        {

            lock (OpenStorages)
            {
                var objectStorage = (from openedObjectStorage in OpenStorages.Values
                                     where openedObjectStorage.StorageMetaData.StorageIdentity == storageIdentity
                                     select openedObjectStorage).OfType<ObjectStorage>().FirstOrDefault();
                if (objectStorage != null)
                    objectStorage.UpdateOperativeObjects();
            }

        }

        private static StorageProvider GetStorageProvider(string storageType)
        {
            StorageProvider storageProvider = null;
#if DeviceDotNet
            if (storageType == "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider")
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "MetaDataLoadingSystem, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null")) as StorageProvider;

            if (storageType == "OOAdvantech.SQLitePersistenceRunTime.StorageProvider")
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "SQLitePersistenceRunTime, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null")) as StorageProvider;

            if (storageType == "OOAdvantech.SQLiteMetaDataPersistenceRunTime.StorageProvider")
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "SQLitePersistenceRunTime, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null")) as StorageProvider;

            if (storageProvider == null)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                 OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            return storageProvider;
#else

            try
            {
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }

            return storageProvider;
#endif
        }

        /// <MetaDataID>{b90deb74-79e1-4b0c-a4d1-78826f73fcbb}</MetaDataID>
        static string _HostName;
        /// <MetaDataID>{b02f8609-7d5a-465b-b824-2f1828555afb}</MetaDataID>
        static string HostName
        {
            get
            {
#if !DeviceDotNet
                if (string.IsNullOrEmpty(_HostName))
                    _HostName = System.Net.Dns.GetHostName().Trim().ToLower();
#endif

                return _HostName;
            }
        }

        #region IPersistentObjectLifeTimeController Members

        /// <MetaDataID>{F4B32E85-B6B8-40FA-AF43-A80874857663}</MetaDataID>
        public object GetObject(string persistentObjectUri)
        {
            //#region MonoStateClass
            //if (this != MonoStatePersistencyService)
            //    return MonoStatePersistencyService.GetObject(persistentObjectUri);
            //#endregion

            if (persistentObjectUri == null || persistentObjectUri == "ClassNotPersistent")
                return null;

            foreach (var entry in OpenStorages)
            {
                ObjectStorage objectStorage = entry.Value as ObjectStorage;
                int nPos = persistentObjectUri.IndexOf("\\");
                if (nPos == -1)
                    throw new Exception("Uri syntax Error");

                string storageUri = persistentObjectUri.Substring(0, nPos);
                persistentObjectUri = persistentObjectUri.Substring(nPos + 1);

                //TODO ÙÈ „ﬂÌÂÙ·È ÏÂ Ù· storages Ôı ÙÒ›˜ÔıÌ in process
                if (objectStorage.StorageMetaData != null && storageUri == objectStorage.StorageMetaData.StorageIdentity)
                    return objectStorage.GetObject(persistentObjectUri);

            }
            return null;
        }

        /// <MetaDataID>{FFBEC954-1D5B-4A23-92BE-E5E01FB2DFE2}</MetaDataID>
        public string GetPersistentObjectUri(object obj)
        {
            //#region MonoStateClass
            //if (this != MonoStatePersistencyService)
            //    return MonoStatePersistencyService.GetPersistentObjectUri(obj);
            //#endregion
            //TODO ÙÈ „ﬂÌÂÙ·È ÏÂ Ù· storages Ôı ÙÒ›˜ÔıÌ in process

            if (obj == null)
                return null;
            if (!ClassOfObjectIsPersistent(obj))
                return "ClassNotPersistent";

            StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(obj) as StorageInstanceRef;
            if (storageInstanceRef == null)
                return null;
            else
            {
                string persistentObjectUri = (storageInstanceRef.ObjectStorage as Remoting.IPersistentObjectLifeTimeController).GetPersistentObjectUri(obj);
                if (persistentObjectUri != null && persistentObjectUri.Trim().Length > 0)
                    return HostName + "\\" + persistentObjectUri;
            }
            return null;
        }

        #endregion

        #region IPersistencyService Members

        /// <MetaDataID>{9baa4bd9-8dd5-4b5e-8efd-4ddf592c67a3}</MetaDataID>
        public void DeleteStorage(string storageName, string storageLocation, string storageType)
        {

            //#region MonoStateClass
            //if (this != MonoStatePersistencyService)
            //{
            //    MonoStatePersistencyService.DeleteStorage(storageName, storageLocation, storageType);
            //    return;
            //}
            //#endregion

            #region Gets storage provider for the type of storage
            StorageProvider storageProvider = null;
            try
            {
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }
            #endregion

            #region Gets some important informationís for the storage like host machine, server instance etc.
            bool embedded = storageProvider.IsEmbeddedStorage(storageName, storageLocation);
            string storageHostComputerName = storageProvider.GetHostComuterName(storageName, storageLocation).Trim().ToLower();
            string instanceName = storageProvider.GetInstanceName(storageName, storageLocation);
            #endregion

            #region Call server instance which host the storage to delete the storage if storage is out of process
            if (!embedded)
            {
#if !DeviceDotNet
                PersistenceLayer.IPersistencyService persistencyService = GetPersistencyServiceOnMachine(storageHostComputerName, instanceName);
                if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                {
                    persistencyService.DeleteStorage(storageName, storageLocation, storageType);
                    return;
                }
#endif
            }
            #endregion

            #region Deletes the storage.
            storageProvider.DeleteStorage(storageName, storageLocation);
            if (OpenStorages.ContainsKey(storageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower()))
                OpenStorages.Remove(storageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower());
            #endregion

        }



        /// <MetaDataID>{d7a317e1-091a-4214-b5cc-252e72f16f34}</MetaDataID>
        public OOAdvantech.PersistenceLayer.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage storageMetadata, string storageName, object rawStorageData, string storageType)
        {
            //#region MonoStateClass
            //if (this != MonoStatePersistencyService)
            //    return MonoStatePersistencyService.NewStorage(storageMetadata, storageName, rawStorageData, storageType);
            //#endregion

            #region Gets storage provider for the type of storage
            StorageProvider storageProvider = null;
            try
            {
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }
            #endregion



            #region Creates storage and load in this server instance and return the new storage.
            PersistenceLayer.ObjectStorage newStorage = storageProvider.NewStorage(storageMetadata, storageName, rawStorageData);
            if (string.IsNullOrWhiteSpace(newStorage.StorageMetaData.StorageLocation))
                newStorage.StorageMetaData.StorageLocation = newStorage.StorageMetaData.StorageIdentity;
            string openedStorageID = newStorage.StorageMetaData.StorageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower();
            RemovePreviousRawStorageData(openedStorageID);

            OpenStorages[openedStorageID] = newStorage;

            OpenStorages[rawStorageData] = newStorage;


            if (rawStorageData is IRawStorageData)
                RawStoragesData[openedStorageID] = rawStorageData as IRawStorageData;


            return newStorage;
            #endregion
        }

        /// <MetaDataID>{9a336dd9-6a4d-42d1-b5a7-e5bea7c8deed}</MetaDataID>
        public OOAdvantech.PersistenceLayer.ObjectStorage OpenStorage(string storageName, object rawStorageData, string storageType)
        {
            //#region MonoStateClass
            //if (this != MonoStatePersistencyService)
            //    return MonoStatePersistencyService.OpenStorage(storageName, rawStorageData, storageType);
            //#endregion

            #region Gets storage provider for the type of storage
            StorageProvider storageProvider = null;
            try
            {
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }
            #endregion


            #region Load storage locally if must be hosted in this process.
            lock (OpenStorages)
            {
                if (OpenStorages.ContainsKey(rawStorageData))
                {
                    ObjectStorage objectStorage = (ObjectStorage)OpenStorages[rawStorageData];
                    if (objectStorage.StorageMetaData != null && objectStorage.StorageMetaData.StorageType == storageType)
                        return objectStorage;
                }
                string openedStorageID = null;
                if (rawStorageData is IRawStorageData)
                {
                    openedStorageID = (rawStorageData as IRawStorageData).StorageLocation.ToLower() + "." + (rawStorageData as IRawStorageData).StorageName.ToLower() + ":" + storageType.ToLower();
                    if (OpenStorages.ContainsKey(openedStorageID))
                    {
                        ObjectStorage objectStorage = (ObjectStorage)OpenStorages[openedStorageID];
                        return objectStorage;
                    }
                }



                PersistenceLayer.ObjectStorage OpenedStorageSession = storageProvider.OpenStorage(storageName, rawStorageData);
                if (string.IsNullOrWhiteSpace(OpenedStorageSession.StorageMetaData.StorageLocation))
                    OpenedStorageSession.StorageMetaData.StorageLocation = OpenedStorageSession.StorageMetaData.StorageIdentity;

                openedStorageID = OpenedStorageSession.StorageMetaData.StorageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower();
                OpenStorages[openedStorageID] = OpenedStorageSession;

                MetaDataRepository.StorageMetaData storageMetaData = new MetaDataRepository.StorageMetaData()
                {
                    StorageIdentity = OpenedStorageSession.StorageMetaData.StorageIdentity,
                    StorageLocation = OpenedStorageSession.StorageMetaData.StorageLocation,
                    StorageType = OpenedStorageSession.StorageMetaData.StorageType,
                    StorageName = OpenedStorageSession.StorageMetaData.StorageName,
                    NativeStorageID = OpenedStorageSession.StorageMetaData.NativeStorageID,
                    MultipleObjectContext = true
                };

                PersistenceLayer.StorageServerInstanceLocator.Current.PublishStorageMetaData(storageMetaData);

                RemovePreviousRawStorageData(openedStorageID);

                OpenStorages[rawStorageData] = OpenedStorageSession;
                if (rawStorageData is IRawStorageData)
                    RawStoragesData[openedStorageID] = rawStorageData as IRawStorageData;

                return OpenedStorageSession;

            }
            #endregion


        }

        private void RemovePreviousRawStorageData(string openedStorageID)
        {

            IRawStorageData rawStorageData = null;
            if (RawStoragesData.TryGetValue(openedStorageID, out rawStorageData))
            {
                if (this.OpenStorages.ContainsKey(rawStorageData))
                    this.OpenStorages.Remove(rawStorageData);
            }
        }

        #endregion
        /// <MetaDataID>{0fedc386-3bad-41a2-bd5e-3b577688bae2}</MetaDataID>
        public OOAdvantech.MetaDataRepository.StorageServer StorageServer;

        public event ObjectStorageResolveHandler ObjectStorageResolve;
        public event ObjectStorageLoadeHandler ObjectStorageLoad;

        /// <MetaDataID>{f41b8e4e-d9b2-40d0-9c0e-599be61e3fb7}</MetaDataID>
        public OOAdvantech.PersistenceLayer.ObjectStorage NewLogicalStorage(OOAdvantech.PersistenceLayer.ObjectStorage hostingStorage, string logicalStorageName)
        {

            #region Gets storage provider for the type of storage
            StorageProvider storageProvider = null;
            string storageType = hostingStorage.StorageMetaData.StorageType;
            string storageName = hostingStorage.StorageMetaData.StorageName;
            string storageLocation = hostingStorage.StorageMetaData.StorageLocation;
            try
            {
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }
            #endregion

            if (hostingStorage is OOAdvantech.PersistenceLayerRunTime.ClientSide.ObjectStorageAgent)
                hostingStorage = (hostingStorage as OOAdvantech.PersistenceLayerRunTime.ClientSide.ObjectStorageAgent).ServerSideObjectStorage;
#if !DeviceDotNet
            if (Remoting.RemotingServices.IsOutOfProcess(hostingStorage))
            {

                PersistenceLayer.IPersistencyService persistencyService = Remoting.RemotingServices.CreateRemoteInstance(Remoting.RemotingServices.GetChannelUri(hostingStorage), typeof(PersistencyService).FullName, "") as PersistenceLayer.IPersistencyService;
                return persistencyService.NewLogicalStorage(hostingStorage, logicalStorageName);
            }
            else
                return CreateNewLogicalStorage(hostingStorage, logicalStorageName);
#else
            return CreateNewLogicalStorage(hostingStorage, logicalStorageName);
#endif

        }


        /// <MetaDataID>{bfeeef75-8489-4fd9-aa77-88b744d615f5}</MetaDataID>
        public OOAdvantech.PersistenceLayer.ObjectStorage NewLogicalStorage(string storageName, string storageLocation, string storageType, string logicalStorageName, bool InProcess)
        {
            PersistenceLayer.ObjectStorage hostingObjectStorage = null;
            PersistenceLayer.ObjectStorage newStorage = null;


            if (hostingObjectStorage != null)
                return CreateNewLogicalStorage(hostingObjectStorage, logicalStorageName);

            #region Gets storage provider for the type of storage
            StorageProvider storageProvider = null;
            try
            {
                storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageType, "")) as StorageProvider;
                if (storageProvider == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                        OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError);
            }
            catch (System.Exception Error)
            {
                throw new OOAdvantech.PersistenceLayer.StorageException("PersistencyService can't instantiate the " + storageType + " Provider",
                    OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
            }
            #endregion

            #region Gets some important informationís for the storage like host machine, server instance etc.
            string storageHostComputerName = storageProvider.GetHostComuterName(storageName, storageLocation).Trim().ToLower();
            string instanceName = storageProvider.GetInstanceName(storageName, storageLocation).Trim().ToLower();
            InProcess |= storageProvider.IsEmbeddedStorage(storageName, storageLocation);
            #endregion

            #region Creates storage and load in server instance other than this and return the new storage
            if (!InProcess)
            {
#if !DeviceDotNet
                PersistenceLayer.IPersistencyService persistencyService = GetPersistencyServiceOnMachine(storageHostComputerName, instanceName);
                if (Remoting.RemotingServices.IsOutOfProcess(persistencyService as MarshalByRefObject))
                    return persistencyService.NewLogicalStorage(storageName, storageLocation, storageType, logicalStorageName, InProcess);

#endif

            }
            #endregion

            #region Creates storage and load in this server instance and return the new storage.
            hostingObjectStorage = storageProvider.NewStorage(null, storageName, storageLocation);
            OpenStorages[storageLocation.ToLower() + "." + storageName.ToLower() + ":" + storageType.ToLower()] = newStorage;
            return CreateNewLogicalStorage(hostingObjectStorage, logicalStorageName);
            #endregion

        }

        /// <MetaDataID>{fde94566-a6ec-4ddd-aa73-a21728966279}</MetaDataID>
        private OOAdvantech.PersistenceLayer.ObjectStorage CreateNewLogicalStorage(OOAdvantech.PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName)
        {
            StorageProvider storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(hostingObjectStorage.StorageMetaData.StorageType, "")) as StorageProvider;
            return storageProvider.CreateNewLogicalStorage(hostingObjectStorage, storageName);
        }




    }


}
