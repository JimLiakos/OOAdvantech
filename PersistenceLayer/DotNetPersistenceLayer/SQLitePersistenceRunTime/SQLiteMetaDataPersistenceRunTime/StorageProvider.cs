using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.PersistenceLayerRunTime;
//using SQLiteWrapper;

namespace OOAdvantech.SQLiteMetaDataPersistenceRunTime
{
    /// <MetaDataID>{1e99f146-f926-416d-b6dc-837ebbd52b2a}</MetaDataID>
    public class StorageProvider : PersistenceLayerRunTime.StorageProvider
    {
        static public void CreateSQLiteDatabase(string StorageName, string storageLocation)
        {
#if DeviceDotNet

            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection DBConnection = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection)) as OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection;
            DBConnection.ConnectionString = storageLocation;
            DBConnection.CreateDataBase();
#else
            IntPtr Db=IntPtr.Zero;
            using (SQLite.SQLiteConnection SQLiteConnection = new SQLite.SQLiteConnection(storageLocation))
            {

            }
#endif
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
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, string StorageLocation, string userName = "", string password = "")
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

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, string StorageLocation, string userName = "", string password = "")
        {
            CreateSQLiteDatabase(StorageName, StorageLocation);

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

   
        public override void Repair(string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password, bool overrideObjectStorage)
        {
            throw new NotImplementedException();
        }
    }
}
