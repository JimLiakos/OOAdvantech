using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.PersistenceLayer;

namespace OOAdvantech.SQLitePersistenceRunTime
{
    /// <MetaDataID>{826231d1-b321-4a68-bb1e-43a9764230b7}</MetaDataID>
    public class StorageProvider : OOAdvantech.RDBMSPersistenceRunTime.StorageProvider
    {
        /// <MetaDataID>{e3f4b0b2-cd7b-40cb-904a-dd2e49e188ea}</MetaDataID>
        public override string GetNativeStorageID(string storageDataLocation)
        {
            throw new System.NotImplementedException();

            ////System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(storageDataLocation);

            //var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            //OOAdvantech.IDBConnaction DBConnaction = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDBConnaction)) as OOAdvantech.IDBConnaction;


            //string nativeStorageID= DBConnaction.GetNativeStorageID(storageDataLocation);

            //string machineName = null;
            //string instanceName = null;
            //string dataBaseName = builder.InitialCatalog;
            //if (builder.DataSource.IndexOf(@"\") == 1)
            //    machineName = builder.DataSource;
            //else
            //{
            //    machineName = builder.DataSource.Substring(0, builder.DataSource.IndexOf(@"\"));
            //    instanceName = builder.DataSource.Substring(builder.DataSource.IndexOf(@"\") + 1);
            //}
            //string machineSID = new SecurityIdentifier((byte[])new DirectoryEntry(string.Format("WinNT://{0},Computer", machineName)).Children.Cast<DirectoryEntry>().First().InvokeGet("objectSID"), 0).AccountDomainSid.ToString();

            //string nativeStorageID = null;
            //if (!string.IsNullOrEmpty(instanceName))
            //    nativeStorageID = machineSID + @"\" + instanceName + @"\" + dataBaseName;
            //else
            //    nativeStorageID = machineSID + @"\" + dataBaseName;

            //return nativeStorageID.ToLower();

        }

        public static void init()
        {
#if DeviceDotNet
            OOAdvantech.TypeLoader.SetAssemblyMetaData(typeof(StorageProvider));
            OOAdvantech.TypeLoader.SetAssemblyMetaData(typeof(OOAdvantech.PersistenceLayerRunTime.StorageProvider));
            OOAdvantech.TypeLoader.SetAssemblyMetaData(typeof(OOAdvantech.DotNetMetaDataRepository.Assembly));
            OOAdvantech.TypeLoader.SetAssemblyMetaData(typeof(OOAdvantech.MetaDataRepository.Storage));
#endif
        }

        /// <MetaDataID>{476573f8-2e0e-4d73-bc3e-667746babc6d}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.Storage AttachStorage(string storageName, string storageLocation, string nativeStorageConnectionString)
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{842584a9-4840-4f40-8f5c-d4e89f6dc6c5}</MetaDataID>
        public override void DeleteStorage(string storageName, string storageLocation)
        {
            throw new System.NotImplementedException();

        }
        /// <exclude>Excluded</exclude>
        private System.Guid _ProviderID = System.Guid.Empty;
        /// <summary>The Provider identity. Globally unique.</summary>
        /// <MetaDataID>{16321928-C7AA-4B23-845C-5DC6BA684038}</MetaDataID>
        public override System.Guid ProviderID
        {
            get
            {
                if (_ProviderID == System.Guid.Empty)
                    _ProviderID = new System.Guid("{FB525F3B-C26A-4820-A1BA-170C2B0691F9}");
                return _ProviderID;
            }
            set
            {
            }
        }

        /// <MetaDataID>{c5d7d39f-919e-4020-a257-ef5c82e4ba90}</MetaDataID>
        public override OOAdvantech.RDBMSDataObjects.DataBase GetDataBase(string connectionString)
        {
            throw new System.NotImplementedException();
            //System.Data.SqlClient.SqlConnectionStringBuilder connectionStringBuilde = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

            //string dataBaseName = connectionStringBuilde.InitialCatalog;

            //return new DataBase(dataBaseName, new RDBMSMetaDataPersistenceRunTime.DataBaseConnection(new System.Data.SqlClient.SqlConnection(connectionString)));
        }

        /// <MetaDataID>{51AC29F2-4AEF-446D-B18E-E01903E04E88}</MetaDataID>
        protected OOAdvantech.RDBMSDataObjects.DataBase GetDataBase(string dataBaseLocation, string dataBaseName)
        {
#if DeviceDotNet

            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            if ((deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem).FileExists(dataBaseLocation))
            {
                try
                {
                    OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection DBConnection = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection)) as OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection;
                    DBConnection.ConnectionString = dataBaseLocation;
                    return new DataBase(dataBaseName, DBConnection);
                }
                catch (Exception error)
                {
                    throw new System.Exception("The storage location " + dataBaseLocation + " can't be accessed.", error);
                }
            }
            else
                throw new System.Exception("The storage location " + dataBaseLocation + " can't be accessed.");
#else
            if (System.IO.File.Exists(dataBaseLocation))
            {
                try
                {
                    var DBConnection = new SQLitePersistenceRunTime.SQLiteDataBaseConnection();
                    DBConnection.SQLiteFilePath = dataBaseLocation;
                    return new DataBase(dataBaseName, DBConnection);

                }
                catch (Exception error)
                {
                    throw new System.Exception("The storage location " + dataBaseLocation + " can't be accessed.",error);
                }
            }
            else
                throw new System.Exception("The storage location " + dataBaseLocation + " can't be accessed.");
#endif

        }

        /// <MetaDataID>{40cfa7c0-920d-44c4-9e53-5ed6220bc752}</MetaDataID>
        static protected System.Collections.Generic.Dictionary<string, SQLitePersistenceRunTime.ObjectStorage> OpenStorages = new System.Collections.Generic.Dictionary<string, SQLitePersistenceRunTime.ObjectStorage>();
        /// <MetaDataID>{4a8f9490-671e-48a5-869e-1bb98572e61d}</MetaDataID>
        static protected string OpenStorageLock = "OpenStorageLock";
        /// <summary>Create a storage access session.</summary>
        /// <param name="StorageName">The name of Object Storage</param>
        /// <param name="StorageLocation">This parameter contains the location of object storage.
        /// If it is null then the provider will look at Persistence Layer repository.
        /// </param>
        /// <MetaDataID>{E1A7DB91-C68B-412D-AEC7-341DD705F6F2}</MetaDataID>
        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, string StorageLocation, string userName = "", string password = "")
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


                        PersistenceLayer.ObjectStorage MetaDataStorageSession = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, "OOAdvantech.SQLiteMetaDataPersistenceRunTime.StorageProvider", true);
                        Collections.StructureSet aStructureSet = MetaDataStorageSession.Execute("SELECT ObjectStorage FROM " + typeof(RDBMSMetaDataRepository.Storage).FullName + " ObjectStorage ");
                        mObjectStorage = null;


                        foreach (Collections.StructureSet Rowset in aStructureSet)
                        {
                            mObjectStorage = (RDBMSPersistenceRunTime.Storage)Rowset["ObjectStorage"];
                            mObjectStorage.StorageDataBase = GetDataBase(StorageLocation, StorageName);
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


        /// <summary>Create a new Object Storage with schema like original storage and open a storage session with it.</summary>
        /// <param name="OriginalStorage">Cloned Metada (scema)</param>
        /// <param name="StorageName">The name of new Object Storage</param>
        /// <param name="StorageLocation">This parameter contains the location of object storage.
        /// If it is null then the provider will look at Persistence Layer repository.
        /// </param>
        /// <MetaDataID>{F5CF7C34-A93D-4A59-BFA8-D1A873E593F6}</MetaDataID>

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, string StorageLocation, string userName = "", string password = "")
        {
            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
#if !DeviceDotNet
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {
                    try
                    {
                        
                        PersistenceLayer.ObjectStorage metaDataStorage = PersistenceLayer.ObjectStorage.NewStorage(StorageName, StorageLocation,
                            "OOAdvantech.SQLiteMetaDataPersistenceRunTime.StorageProvider", true);
                        string assemblyFullName = typeof(RDBMSPersistenceRunTime.Storage).Assembly.FullName;
                        metaDataStorage.StorageMetaData.RegisterComponent(assemblyFullName);
                        var dataBase = GetDataBase(StorageLocation, StorageName);
                        RDBMSPersistenceRunTime.Storage storage = new RDBMSPersistenceRunTime.Storage(StorageName, StorageLocation, "OOAdvantech.SQLitePersistenceRunTime.StorageProvider", "", dataBase);
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

                    PersistenceLayer.ObjectStorage metaDataStorage = PersistenceLayer.ObjectStorage.NewStorage(StorageName, StorageLocation,
                        "OOAdvantech.SQLiteMetaDataPersistenceRunTime.StorageProvider", true);
                    string assemblyFullName = typeof(RDBMSPersistenceRunTime.Storage).GetMetaData().Assembly.FullName;
                    metaDataStorage.StorageMetaData.RegisterComponent(assemblyFullName);
                    var dataBase = GetDataBase(StorageLocation, StorageName);
                    RDBMSPersistenceRunTime.Storage storage = new RDBMSPersistenceRunTime.Storage(StorageName, StorageLocation, "OOAdvantech.SQLitePersistenceRunTime.StorageProvider", "", dataBase);
                    metaDataStorage.CommitTransientObjectState(storage);
                    //storage.UpdateSchema();
                    ObjectStorage objectStorage = new ObjectStorage(storage);
                    //transactionScope.Complete();
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


        /// <MetaDataID>{57C34B53-CF4C-4D2A-8E40-3215CF263E71}</MetaDataID>
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }
        /// <MetaDataID>{FEA8B201-84F1-4444-827F-424C3D7B3651}</MetaDataID>
        public override bool AllowEmbeddedStorage()
        {
            return true;
        }
        /// <MetaDataID>{1045F153-41B9-4548-A6F9-3860FE56131A}</MetaDataID>
        public override string GetHostComuterName(string StorageName, string StorageLocation)
        {
            int npos = StorageLocation.IndexOf('\\');
            if (npos == -1)
                return StorageLocation;
            else
                return StorageLocation.Substring(0, npos);

        }
        /// <MetaDataID>{cb22ea42-cccf-4553-b64c-0cce8dd4516e}</MetaDataID>
        public override string GetInstanceName(string storageName, string storageLocation)
        {
            int npos = storageLocation.IndexOf('\\');
            if (npos == -1)
                return "default";
            else
                return storageLocation.Substring(npos + 1);
        }

        /// <MetaDataID>{b8a055a3-1ec0-4916-bab4-bbd6946d9af6}</MetaDataID>
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, object rawStorageData)
        {
            throw new System.Exception("OOAdvantech.MSSQLPersistenceRunTime.StorageProvider doesn't suport raw storage data functionality.");
        }

        /// <MetaDataID>{e38c87a4-f5d1-4dae-b9d6-e46b07a12a02}</MetaDataID>
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            throw new System.Exception("OOAdvantech.MSSQLPersistenceRunTime.StorageProvider doesn't suport raw storage data functionality.");
        }

        public override OOAdvantech.PersistenceLayer.ObjectStorage CreateNewLogicalStorage(OOAdvantech.PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName)
        {
            throw new NotImplementedException();
        }

        public override void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override void Repair(string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
