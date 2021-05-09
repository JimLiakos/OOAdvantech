using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using OOAdvantech.RDBMSPersistenceRunTime.Storage;


namespace OOAdvantech.MSSQLCompactPersistenceRunTime
{
    /// <MetaDataID>{2f661c2f-f75e-48ed-aff7-658c17170faf}</MetaDataID>
    public class StorageProvider : OOAdvantech.PersistenceLayerRunTime.StorageProvider	
    {
        static System.Collections.Generic.Dictionary<string, ObjectStorage> OpenStorages = new Dictionary<string, ObjectStorage>();
        static string OpenStorageLock = "OpenStorageLock";

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            throw new NotImplementedException();
        }
        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, string StorageLocation)
        {


            lock (OpenStorageLock)
            {
                try
                {
                    ObjectStorage mStorageSession = null;
                    if (OpenStorages.TryGetValue(StorageName.ToLower(), out mStorageSession))
                        return mStorageSession;
                    using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {
                        RDBMSPersistenceRunTime.Storage mObjectStorage = null;


                        PersistenceLayer.ObjectStorage MetaDataStorageSession = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, "OOAdvantech.MSSQLCompactFastPersistenceRunTime.StorageProvider",true);
                        Collections.StructureSet aStructureSet = MetaDataStorageSession.Execute("SELECT ObjectStorage FROM " + typeof(RDBMSMetaDataRepository.Storage).FullName + " ObjectStorage ");
                        mObjectStorage = null;


                        foreach (Collections.StructureSet Rowset in aStructureSet)
                        {
                            mObjectStorage = (RDBMSPersistenceRunTime.Storage)Rowset.Members["ObjectStorage"].Value;
                            mObjectStorage.StorageDataBase = GetDataBase(StorageName, StorageLocation);
                            break;
                        }

                        if (mObjectStorage == null)
                            throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + StorageName + " at location " + StorageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);

                        //if (!mObjectStorage.ServerContainsDatabase(StorageName))
                        //    throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + StorageName + " at location " + StorageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);


                        mStorageSession = new ObjectStorage(mObjectStorage);


                        stateTransition.Consistent = true;
                        if (mStorageSession != null)
                            OpenStorages[StorageName.ToLower()] = mStorageSession;

                        return mStorageSession;
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
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, object rawStorageData)
        {
            throw new NotImplementedException();
        }
        public override string GetInstanceName(string StorageName, string StorageLocation)
        {
            return "";
        }
        public override string GetHostComuterName(string StorageName, string StorageLocation)
        {
            return "";
        }
        public override void DeleteStorage(string storageName, string storageLocation)
        {
            throw new NotImplementedException();
        }
        public override bool AllowEmbeddedStorage()
        {
            throw new NotImplementedException();
        }
        /// <exclude>Excluded</exclude>
        private System.Guid _ProviderID = System.Guid.Empty;
        /// <summary>The Provider identity. Globally unique. </summary>
        public override System.Guid ProviderID
        {
            get
            {
                if (_ProviderID == System.Guid.Empty)
                    _ProviderID = new System.Guid("{11144643-1C40-4414-B113-A83829D2CA64}");
                return _ProviderID;

            }
            set
            {
            }
        }
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }

        DataObjects.DataBase GetDataBase(string dataBaseName, string dataBaseLocation)
        {

            if (System.IO.File.Exists(dataBaseLocation))
                return new DataObjects.DataBase(dataBaseName);
            else
                return null;

            string connectionString = @"Data Source=" + dataBaseLocation; // @"Initial Catalog=master;Integrated Security=True;Data Source=localhost\SQLExpress";
            System.Data.SqlServerCe.SqlCeConnection connection = new System.Data.SqlServerCe.SqlCeConnection(connectionString);
            try
            {

                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                    return new DataObjects.DataBase(dataBaseName);
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("The storage location " + dataBaseLocation + " can't be accessed.", Error);
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, string StorageLocation)
        {
            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
#if! DeviceDotNet 
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {
                    try
                    {
                        PersistenceLayer.ObjectStorage metaDataStorage =PersistenceLayer.ObjectStorage.NewStorage(StorageName,StorageLocation,
                            "OOAdvantech.MSSQLCompactFastPersistenceRunTime.StorageProvider", true);
                        string assemblyFullName = typeof(RDBMSPersistenceRunTime.Storage).Assembly.FullName;
                        metaDataStorage.StorageMetaData.RegisterComponent(assemblyFullName);
                        RDBMSPersistenceRunTime.Storage storage = new RDBMSPersistenceRunTime.Storage(StorageName, StorageLocation, "OOAdvantech.MSSQLCompactPersistenceRunTime.StorageProvider", GetDataBase(StorageName, StorageLocation));
                        metaDataStorage.CommitTransientObjectState(storage);
                        //storage.UpdateSchema();
                        ObjectStorage objectStorage = new ObjectStorage(storage);
                        transactionScope.Complete();
                       //(objectStorage.StorageMetaData as MetaDataRepository.Storage).PutPropertyValue("StorageMetadata", "MSSQLInstancePath", System.Net.Dns.GetHostName() + @"\SQLExpress");
                        StateTransition.Consistent = true;
                        return objectStorage;
                    }
                    catch (System.Exception Error)
                    {
                        throw new PersistenceLayer.StorageException("Error in storage creation", PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
                    }
                }
#else
                    try
                    {
                        PersistenceLayer.ObjectStorage metaDataStorage =PersistenceLayer.ObjectStorage.NewStorage(StorageName,StorageLocation,
                            "OOAdvantech.MSSQLCompactFastPersistenceRunTime.StorageProvider", true);
                        string assemblyFullName = typeof(RDBMSPersistenceRunTime.Storage).Assembly.FullName;
                        metaDataStorage.StorageMetaData.RegisterComponent(assemblyFullName);
                        RDBMSPersistenceRunTime.Storage storage = new RDBMSPersistenceRunTime.Storage(StorageName, StorageLocation, "OOAdvantech.MSSQLCompactPersistenceRunTime.StorageProvider", GetDataBase(StorageName, StorageLocation));
                        metaDataStorage.CommitTransientObjectState(storage);
                        //storage.UpdateSchema();
                        ObjectStorage objectStorage = new ObjectStorage(storage);
                       //(objectStorage.StorageMetaData as MetaDataRepository.Storage).PutPropertyValue("StorageMetadata", "MSSQLInstancePath", System.Net.Dns.GetHostName() + @"\SQLExpress");
                        StateTransition.Consistent = true;
                        return objectStorage;
                    }
                    catch (System.Exception Error)
                    {
                        throw new PersistenceLayer.StorageException("Error in storage creation", PersistenceLayer.StorageException.ExceptionReason.StorageProviderError, Error);
                    }
#endif
            }
        }

        public override OOAdvantech.PersistenceLayer.Storage AttachStorage(string storageName, string storageLocation, string nativeStorageConnectionString)
        {
            throw new NotImplementedException();
        }

        public override string GetNativeStorageID(string storageDataLocation)
        {
            throw new NotImplementedException();
        }

        public override OOAdvantech.PersistenceLayer.ObjectStorage CreateNewLogicalStorage(OOAdvantech.PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName)
        {
            throw new NotImplementedException();
        }
    }
}
