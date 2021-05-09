using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MSSQLPersistenceRunTime
{
    /// <MetaDataID>{9524d3ce-f181-4366-b5a6-a36924d09e8e}</MetaDataID>
    public class EmbeddedStorageProvider:StorageProvider
    {
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, string StorageLocation, string userName = "", string password = "")
        {
            lock (OpenStorageLock)
            {
                try
                {
                    ObjectStorage objectStorage = null;
                    if (OpenStorages.TryGetValue(StorageName.ToLower(), out objectStorage))
                        return objectStorage;

                    string sqlServerInstanceName = StorageLocation;

                    using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {
                        
                        RDBMSPersistenceRunTime.Storage storage = null;
                        PersistenceLayer.ObjectStorage metaDataObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, "OOAdvantech.MSSQLFastPersistenceRunTime.EmbeddedStorageProvider");
                        Collections.StructureSet aStructureSet = metaDataObjectStorage.Execute("SELECT ObjectStorage FROM " + typeof(RDBMSPersistenceRunTime.Storage).FullName + " ObjectStorage ");
                        storage = null;
                        foreach (Collections.StructureSet Rowset in aStructureSet)
                        {
                            storage = (RDBMSPersistenceRunTime.Storage)Rowset["ObjectStorage"];
                            storage.StorageDataBase = GetDataBase(sqlServerInstanceName, StorageName);
                            break;
                        }

                        if (storage == null)
                            throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + StorageName + " at location " + StorageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);


                        objectStorage = new ObjectStorage(storage);
                        //objectStorage.SetObjectStorage(storage);
                        InitializeDataPartioningMetaData(objectStorage, storage);
                        //(objectStorage.StorageMetaData as MetaDataRepository.Storage).PutPropertyValue("StorageMetadata", "MSSQLInstancePath", System.Net.Dns.GetHostName() + @"\SQLExpress");
                        stateTransition.Consistent = true;
                        if (objectStorage != null)
                            OpenStorages[StorageName.ToLower()] = objectStorage;

                        storage.StorageLocation = StorageLocation;
                        //storage.StorageDataBase.Update();
                        return objectStorage;
                    }

                }
                catch (OOAdvantech.PersistenceLayer.StorageException Error)
                {
                    throw Error; 
                }
                catch (System.Exception Error)
                {
                    throw new OOAdvantech.PersistenceLayer.StorageException("can't open storage " + StorageName + " at location " + StorageLocation, OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.Unknown, Error);
                }
            }

        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage originalStorage, string storageName, string storageLocation, string userName = "", string password = "")
        {
            string sqlServerInstanceName = storageLocation;
          
            MSSQLFastPersistenceRunTime.StorageProvider.CreateSQLDatabase(storageName, sqlServerInstanceName);

            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {

                    try
                    {
                        PersistenceLayer.ObjectStorage MetaDataStorageSession = ObjectStorage.NewStorage(storageName,
                            storageLocation,
                            "OOAdvantech.MSSQLFastPersistenceRunTime.EmbeddedStorageProvider", true);
                        string assemblyFullName = typeof(RDBMSPersistenceRunTime.Storage).Assembly.FullName;
                        MetaDataStorageSession.StorageMetaData.RegisterComponent(assemblyFullName);
                        // object tmp = PersistenceLayer.ObjectStorage.NewStorage(null, "UnitializedRawData", StorageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        //tmp MetaDataLoadingSystem.MetaDataStorageSession MetaDataStorageSession=(MetaDataLoadingSystem.MetaDataStorageSession)tmp;

                        var dataBase=GetDataBase(sqlServerInstanceName, storageName);
                        RDBMSPersistenceRunTime.Storage mObjectStorage = new RDBMSPersistenceRunTime.Storage(storageName, storageLocation, "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider", GetNativeStorageID(dataBase.Connection.ConnectionString), dataBase);
                        MetaDataStorageSession.CommitTransientObjectState(mObjectStorage);
                        //mObjectStorage.UpdateSchema();
                        ObjectStorage objectStorage = new ObjectStorage(mObjectStorage);
                        //                        objectStorage.SetObjectStorage(mObjectStorage);
                        transactionScope.Complete();
                        //(objectStorage.StorageMetaData as MetaDataRepository.Storage).PutPropertyValue("StorageMetadata", "MSSQLInstancePath", System.Net.Dns.GetHostName() + @"\SQLExpress");
                        StateTransition.Consistent = true;
                        return objectStorage;
                    }
                    catch (System.Exception Error)
                    {
                        throw new PersistenceLayer.StorageException("Error in storage creation", PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
                    }

                    transactionScope.Complete();
                }

                StateTransition.Consistent = true;
            }

        }
        
    }
}
