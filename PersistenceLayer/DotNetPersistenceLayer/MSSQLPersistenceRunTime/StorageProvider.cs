using System;
using System.Security.Principal;
//using System.DirectoryServices;
using System.Linq;
using System.DirectoryServices;
using OOAdvantech.PersistenceLayer;

namespace OOAdvantech.MSSQLPersistenceRunTime
{
    //using OOAdvantech.RDBMSPersistenceRunTime;
    /// <MetaDataID>{FB051EBA-36FE-4FEB-A06A-D0B36D2BA27A}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{FB051EBA-36FE-4FEB-A06A-D0B36D2BA27A}")]
    public class StorageProvider : OOAdvantech.RDBMSPersistenceRunTime.StorageProvider
    {
        /// <MetaDataID>{e3f4b0b2-cd7b-40cb-904a-dd2e49e188ea}</MetaDataID>
        public override string GetNativeStorageID(string storageDataLocation)
        {

            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(storageDataLocation);
            string machineName = null;
            string instanceName = null;
            string dataBaseName = builder.InitialCatalog;
            if (builder.DataSource.IndexOf(@"\") == 1)
                machineName = builder.DataSource;
            else
            {
                machineName = builder.DataSource.Substring(0, builder.DataSource.IndexOf(@"\"));
                instanceName = builder.DataSource.Substring(builder.DataSource.IndexOf(@"\") + 1);
            }
            string machineSID = new SecurityIdentifier((byte[])new DirectoryEntry(string.Format("WinNT://{0},Computer", machineName)).Children.Cast<DirectoryEntry>().First().InvokeGet("objectSID"), 0).AccountDomainSid.ToString();

            string nativeStorageID = null;
            if(!string.IsNullOrEmpty(instanceName))
                nativeStorageID = machineSID + @"\" + instanceName + @"\" + dataBaseName;
            else
                nativeStorageID = machineSID + @"\" + dataBaseName;

            return nativeStorageID.ToLower();

        }
        /// <MetaDataID>{476573f8-2e0e-4d73-bc3e-667746babc6d}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.Storage AttachStorage(string storageName, string storageLocation, string nativeStorageConnectionString)
        {
            MSSQLFastPersistenceRunTime.StorageProvider metaDataStorageProvider = new OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider();

            
            var metaDataObjectStorage=  metaDataStorageProvider.OpenStorage(storageName, storageLocation, nativeStorageConnectionString);
            if (metaDataObjectStorage != null)
            {
                Collections.StructureSet aStructureSet = metaDataObjectStorage.Execute("SELECT ObjectStorage FROM " + typeof(RDBMSPersistenceRunTime.Storage).FullName + " ObjectStorage ");
                OOAdvantech.RDBMSPersistenceRunTime.Storage storage = null;
                foreach (Collections.StructureSet Rowset in aStructureSet)
                {
                    storage = (RDBMSPersistenceRunTime.Storage)Rowset["ObjectStorage"];
                    return storage;
                    break;
                }
            }
            else
            {
                using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                {
                    using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                    {

                        metaDataObjectStorage = metaDataStorageProvider.NewStorage(storageName, storageLocation, nativeStorageConnectionString);

                        string assemblyFullName = typeof(RDBMSPersistenceRunTime.Storage).Assembly.FullName;
                        metaDataObjectStorage.StorageMetaData.RegisterComponent(assemblyFullName);
                        RDBMSPersistenceRunTime.Storage mObjectStorage = new RDBMSPersistenceRunTime.Storage(storageName, "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider", storageLocation, GetNativeStorageID(storageLocation), GetDataBase(nativeStorageConnectionString));
                        metaDataObjectStorage.CommitTransientObjectState(mObjectStorage);
                        transactionScope.Complete();
                        StateTransition.Consistent = true;
                        return mObjectStorage;
                    }
                    
                }

//                NewStorage(
            }

           // metaDataStorageProvider.OpenStorage(

                //using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                //    {
                //        RDBMSPersistenceRunTime.Storage storage = null;
                //        PersistenceLayer.ObjectStorage metaDataObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageName, StorageLocation, "OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider");
                //        Collections.StructureSet aStructureSet = metaDataObjectStorage.Execute("SELECT ObjectStorage FROM " + typeof(RDBMSPersistenceRunTime.Storage).FullName + " ObjectStorage ");
                //        storage = null;
                //        foreach (Collections.StructureSet Rowset in aStructureSet)
                //        {
                //            storage = (RDBMSPersistenceRunTime.Storage)Rowset.Members["ObjectStorage"].Value;
                //            storage.StorageDataBase = GetDataBase(sqlServerInstanceName, storageName);
                //            break;
                //        }

                //        if (storage == null)
                //            throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + StorageName + " at location " + StorageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);

            throw new NotImplementedException();
        }        
        /// <MetaDataID>{842584a9-4840-4f40-8f5c-d4e89f6dc6c5}</MetaDataID>
        public override void DeleteStorage(string storageName, string storageLocation)
        {
            //TODO αν έστω και μία φορά έχει ανοιχτεί connection με τη βάση δεδομένων τότε δέν μπορεί να σβηστεί
            //γιατί δεν μπορείς να κάνεις logout παραμόνο αν κλείσεις το process. Αυτό μπορεί να αλλάξει και να σε αφησεί
            //εκθετω.
            string sqlServerInstanceName = @"localhost\SQLExpress";
            if (OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.InStorageService)
            {

                System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
                foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.MSSQLPersistenceRunTime.StorageProvider"))
                {
                    if (xmlElement.HasAttribute("DefaultInsatnceName") && !string.IsNullOrEmpty(xmlElement.GetAttribute("DefaultInsatnceName")))
                        sqlServerInstanceName = xmlElement.GetAttribute("DefaultInsatnceName");
                    break;
                }

            }

            string connectionString = "Integrated Security=True;Initial Catalog=master;Data Source=" + sqlServerInstanceName;
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
            connection.Open();
            try
            {
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("DROP DATABASE Family", connection);//objectStorage.StorageMetaData.StorageName
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
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
            System.Data.SqlClient.SqlConnectionStringBuilder connectionStringBuilde = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

            string dataBaseName = connectionStringBuilde.InitialCatalog;

            return new DataObjects.DataBase(dataBaseName, new RDBMSMetaDataPersistenceRunTime.DataBaseConnection( new System.Data.SqlClient.SqlConnection(connectionString)));
        }
         
        /// <MetaDataID>{51AC29F2-4AEF-446D-B18E-E01903E04E88}</MetaDataID>
        protected OOAdvantech.RDBMSDataObjects.DataBase GetDataBase(string DataBaseLocation, string DataBaseName)
        {
           
            string ConnectionString = @"Initial Catalog=master;Integrated Security=True;Data Source=" + DataBaseLocation;
            System.Data.SqlClient.SqlConnection Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
            try
            {
                Connection.Open();
                System.Data.SqlClient.SqlCommand Command = new System.Data.SqlClient.SqlCommand("exec sp_databases", Connection);
                try
                {
                    System.Data.SqlClient.SqlDataReader DataReader = Command.ExecuteReader();
                    try
                    {
                        foreach (System.Data.Common.DbDataRecord CurrRecord in DataReader)
                        {
                            if (DataBaseName.Equals(CurrRecord["DATABASE_NAME"].ToString()))
                            {
                                ConnectionString = string.Format(@"Initial Catalog={1};Integrated Security=True;Data Source = {0}", DataBaseLocation, DataBaseName);

                                return new DataObjects.DataBase(DataBaseName,new RDBMSMetaDataPersistenceRunTime.DataBaseConnection(new System.Data.SqlClient.SqlConnection(ConnectionString)));
                            }
                        }
                    }
                    finally { DataReader.Close(); }
                }
                finally { Connection.Close(); }
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("The storage location " + DataBaseLocation + " can't be accessed.", Error);
            }
            return null;
        }

        /// <MetaDataID>{40cfa7c0-920d-44c4-9e53-5ed6220bc752}</MetaDataID>
        static protected System.Collections.Generic.Dictionary<string, MSSQLPersistenceRunTime.ObjectStorage> OpenStorages = new System.Collections.Generic.Dictionary<string, MSSQLPersistenceRunTime.ObjectStorage>();
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
                    ObjectStorage objectStorage = null;
                    if (OpenStorages.TryGetValue(StorageName.ToLower(), out objectStorage))
                        return objectStorage;

                    string sqlServerInstanceName = @"localhost\SQLExpress";


                    System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                    storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
                    foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.MSSQLPersistenceRunTime.StorageProvider"))
                    {
                        if (xmlElement.HasAttribute("DefaultInsatnceName") && !string.IsNullOrEmpty(xmlElement.GetAttribute("DefaultInsatnceName")))
                            sqlServerInstanceName = xmlElement.GetAttribute("DefaultInsatnceName");
                        break;
                    }




                    using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {
                        RDBMSPersistenceRunTime.Storage storage = null;
                        PersistenceLayer.ObjectStorage metaDataObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, "OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider");
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
                        objectStorage.ActivateObjectsInPreoadedStorageCells();
                        //(objectStorage.StorageMetaData as MetaDataRepository.Storage).PutPropertyValue("StorageMetadata", "MSSQLInstancePath", System.Net.Dns.GetHostName() + @"\SQLExpress");
                        stateTransition.Consistent = true;
                        if (objectStorage != null)
                            OpenStorages[StorageName.ToLower()] = objectStorage;

                        storage.StorageLocation = StorageLocation;
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

        /// <MetaDataID>{b6ac3998-d031-4992-bb9e-3914bd979392}</MetaDataID>
        protected static void InitializeDataPartioningMetaData(ObjectStorage objectStorage, RDBMSPersistenceRunTime.Storage storage)
        {
            System.Collections.Hashtable Associations = new System.Collections.Hashtable();
            foreach (MetaDataRepository.Component component in storage.Components)
            {
                foreach (MetaDataRepository.MetaObject metaobject in component.Residents)
                {

                    if (metaobject is RDBMSMetaDataRepository.Class)
                    {
                        RDBMSMetaDataRepository.Class rdbmsMappingClass = (metaobject as RDBMSMetaDataRepository.Class);
                        if (rdbmsMappingClass.HistoryClass)
                        {
                            string CommandText = "SELECT     COUNT(*) AS ROW_COUNT	FROM   " + rdbmsMappingClass.ActiveStorageCell.MainTable.Name;
                            var oleDbConnection = ((ObjectStorage)objectStorage).Connection;
                            if (oleDbConnection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                                oleDbConnection.Open();
                            //if(System.EnterpriseServices.ContextUtil.IsInTransaction)
                            //    oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                            var oleDbCommand = oleDbConnection.CreateCommand();
                            oleDbCommand.CommandText = CommandText;
                            try
                            {
                                var myReader = oleDbCommand.ExecuteReader();
                                while (myReader.Read())
                                {
                                    rdbmsMappingClass.ActiveStorageCell.ObjectsCount = (int)myReader["ROW_COUNT"];

                                    break;
                                }
                                myReader.Close();
                            }
                            catch { }


                        }
                        foreach (MetaDataRepository.AssociationEnd CurrAssociationEnd in rdbmsMappingClass.GetAssociateRoles(false))
                        {
                            if (!Associations.Contains(CurrAssociationEnd.Association.GetHashCode()))
                                Associations.Add(CurrAssociationEnd.Association.GetHashCode(), CurrAssociationEnd.Association);
                        }
                    }
                }
            }
            foreach (System.Collections.DictionaryEntry CurrDictionaryEntry in Associations)
            {
                RDBMSMetaDataRepository.Association CurrAssociation = (RDBMSMetaDataRepository.Association)CurrDictionaryEntry.Value;
                if (CurrAssociation.LinkClass == null)
                {
                    foreach (RDBMSMetaDataRepository.StorageCellsLink CurrObjectLinksStorage in CurrAssociation.ObjectLinksStorages)
                    {
                        if (CurrObjectLinksStorage.ObjectLinksTable == null)
                            continue;
                        if (CurrObjectLinksStorage.IsFull)
                            continue;
                        if (CurrObjectLinksStorage.ObjectsLinksCount == -1)
                        {
                            string CommandText = "SELECT     COUNT(*) AS ROW_COUNT	FROM   " + CurrObjectLinksStorage.ObjectLinksTable.Name;
                            var oleDbConnection = ((ObjectStorage)objectStorage).Connection;
                            if (oleDbConnection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                                oleDbConnection.Open();
                            //if(System.EnterpriseServices.ContextUtil.IsInTransaction)
                            //    oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                            var oleDbCommand = oleDbConnection.CreateCommand();
                            oleDbCommand.CommandText = CommandText;
                            try
                            {
                                var myReader = oleDbCommand.ExecuteReader();
                                while (myReader.Read())
                                {
                                    CurrObjectLinksStorage.ObjectsLinksCount = (int)myReader["ROW_COUNT"];
                                    break;
                                }
                                myReader.Close();
                            }
                            catch { }
                        }
                    }
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
        public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string StorageName, string StorageLocation, string userName = "", string password = "")
        {
            System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
            storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
            string sqlServerInstanceName = @"localhost\SQLExpress";
            foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.MSSQLPersistenceRunTime.StorageProvider"))
            {
                if (xmlElement.HasAttribute("DefaultInsatnceName") && !string.IsNullOrEmpty(xmlElement.GetAttribute("DefaultInsatnceName")))
                    sqlServerInstanceName = xmlElement.GetAttribute("DefaultInsatnceName");
                break;
            }

            MSSQLFastPersistenceRunTime.StorageProvider.CreateSQLDatabase(StorageName, sqlServerInstanceName);

            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {

                    try
                    {
                        PersistenceLayer.ObjectStorage MetaDataStorageSession = ObjectStorage.NewStorage(StorageName,
                            StorageLocation,
                            "OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider", true);
                        string assemblyFullName = typeof(RDBMSPersistenceRunTime.Storage).Assembly.FullName;
                        MetaDataStorageSession.StorageMetaData.RegisterComponent(assemblyFullName);
                        // object tmp = PersistenceLayer.ObjectStorage.NewStorage(null, "UnitializedRawData", StorageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        //tmp MetaDataLoadingSystem.MetaDataStorageSession MetaDataStorageSession=(MetaDataLoadingSystem.MetaDataStorageSession)tmp;
                        var dataBase = GetDataBase(sqlServerInstanceName, StorageName);
                        
                        RDBMSPersistenceRunTime.Storage mObjectStorage = new RDBMSPersistenceRunTime.Storage(StorageName, StorageLocation, "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider", GetNativeStorageID(dataBase.Connection.ConnectionString), dataBase);
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


             /// <MetaDataID>{57C34B53-CF4C-4D2A-8E40-3215CF263E71}</MetaDataID>
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return false;
        }
        /// <MetaDataID>{FEA8B201-84F1-4444-827F-424C3D7B3651}</MetaDataID>
        public override bool AllowEmbeddedStorage()
        {
            return false;
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
