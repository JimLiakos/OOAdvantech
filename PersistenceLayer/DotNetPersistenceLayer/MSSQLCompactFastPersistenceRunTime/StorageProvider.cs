using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MSSQLCompactFastPersistenceRunTime
{
    /// <MetaDataID>{6c506b68-d1d7-4703-86ce-0f938e887b89}</MetaDataID>
    public class StorageProvider:PersistenceLayerRunTime.StorageProvider
    {
        static public void CreateSQLDatabase(string StorageName, string StorageLocation)
        {
            string connectionString = "Data Source=[Location]";
            connectionString=connectionString.Replace("[Location]", StorageLocation);
            System.Data.SqlServerCe.SqlCeEngine sqlCeEngine = new System.Data.SqlServerCe.SqlCeEngine(connectionString);

            sqlCeEngine.CreateDatabase();
        }
        /// <exclude>Excluded</exclude>
        private System.Guid _ProviderID = System.Guid.Empty;
        /// <summary>The Provider identity. Globally unique. </summary>
        public override System.Guid ProviderID
        {
            get
            {
                if (_ProviderID == System.Guid.Empty)
                    _ProviderID = new System.Guid("{1CFFA7BB-CFEF-4a78-AC2E-2177737FE08F}");
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
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, string StorageLocation)
        {
            try
            {
                return new ObjectStorage(StorageName, StorageLocation, false);
            }
            catch (System.Exception Error) 
            {
                int tt = 0;
                throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + StorageName + " at location " + StorageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist, Error);

            }
        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, string StorageLocation)
        {
            CreateSQLDatabase(StorageName, StorageLocation);

            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
#if !DeviceDotNet 
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {
                    ObjectStorage storage = new ObjectStorage(StorageName, StorageLocation, true);
                    transactionScope.Complete();
                    StateTransition.Consistent = true;
                    return storage;
                }
#else
                    string connectionString = "Data Source=[Location]";
                    connectionString = connectionString.Replace("[Location]", StorageLocation);
                    ObjectStorage storage = new ObjectStorage(StorageName, StorageLocation, true);
                    StateTransition.Consistent = true;
                    return storage;
#endif
            }
        }

        public override string GetInstanceName(string StorageName, string StorageLocation)
        {
            return "";
        }

        public override string GetHostComuterName(string StorageName, string StorageLocation)
        {
            return "";
        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, object rawStorageData)
        {
            throw new NotImplementedException();
        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            throw new NotImplementedException();
        }

        public override bool AllowEmbeddedStorage()
        {
            throw new NotImplementedException();
        }

        public override void DeleteStorage(string storageName, string storageLocation)
        {
            throw new NotImplementedException();
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
