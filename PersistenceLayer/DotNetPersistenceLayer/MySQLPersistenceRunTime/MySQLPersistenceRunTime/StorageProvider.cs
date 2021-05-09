using System;
namespace OOAdvantech.MySQLPersistenceRunTime
{
    //using OOAdvantech.RDBMSPersistenceRunTime;

    /// <MetaDataID>{e9823717-c87c-4f37-8861-e700d7391f03}</MetaDataID>
    public class StorageProvider : PersistenceLayerRunTime.StorageProvider
    {
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
                foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.MySQLPersistenceRunTime.StorageProvider"))
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


        
        protected DataObjects.DataBase GetDataBase(string databaseLocation, string databaseName)
        {


            string connectionString = "Server=[Location];UserId=root;Password=astraxan;";
            connectionString = connectionString.Replace("[Location]", databaseLocation);

            System.Data.Common.DbConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            try
            {
                connection.Open();
                System.Data.Common.DbCommand command = connection.CreateCommand();
                command.CommandText = "SELECT SCHEMA_NAME FROM information_schema.SCHEMATA";
                try
                {
                    System.Data.Common.DbDataReader dataReader = command.ExecuteReader();
                    try
                    {
                        while (dataReader.Read())
                        {
                            if (databaseName.ToLower() == dataReader["SCHEMA_NAME"].ToString().ToLower())
                                return new DataObjects.DataBase( databaseName);
                        }
                    }
                    finally { dataReader.Close(); }
                }
                finally { connection.Close(); }
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("The storage location " + databaseLocation + " can't be accessed.", Error);
            }
            return null;
        }

        static protected System.Collections.Generic.Dictionary<string, MySQLPersistenceRunTime.ObjectStorage> OpenStorages = new System.Collections.Generic.Dictionary<string, MySQLPersistenceRunTime.ObjectStorage>();
        static protected string OpenStorageLock = "OpenStorageLock";
        /// <summary>Create a storage access session.</summary>
        /// <param name="StorageName">The name of Object Storage</param>
        /// <param name="StorageLocation">This parameter contains the location of object storage.
        /// If it is null then the provider will look at Persistence Layer repository.
        /// </param>
        
        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, string StorageLocation)
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
                    foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.MySQLPersistenceRunTime.StorageProvider"))
                    {
                        if(xmlElement.HasAttribute("DefaultInsatnceName")&&!string.IsNullOrEmpty(xmlElement.GetAttribute("DefaultInsatnceName")))
                            sqlServerInstanceName=xmlElement.GetAttribute("DefaultInsatnceName");
                        break;
                    }




                    using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {
                        RDBMSPersistenceRunTime.Storage storage = null;
                        PersistenceLayer.ObjectStorage metaDataObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, "OOAdvantech.MySQLMetaDataPersistenceRunTime.StorageProvider");
                        Collections.StructureSet aStructureSet = metaDataObjectStorage.Execute("SELECT ObjectStorage FROM " + typeof(RDBMSPersistenceRunTime.Storage).FullName + " ObjectStorage ");
                        storage = null;
                        foreach (Collections.StructureSet Rowset in aStructureSet)
                        {
                            storage = (RDBMSPersistenceRunTime.Storage)Rowset.Members["ObjectStorage"].Value;
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
                            System.Data.Common.DbConnection dbConnection = ((ObjectStorage)objectStorage).Connection;
                            if (dbConnection.State != System.Data.ConnectionState.Open)
                                dbConnection.Open();
                            //if(System.EnterpriseServices.ContextUtil.IsInTransaction)
                            //    oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                            System.Data.Common.DbCommand command = dbConnection.CreateCommand();
                            command.CommandText = CommandText;
                            try
                            {
                                System.Data.Common.DbDataReader myReader = command.ExecuteReader();
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
                            System.Data.Common.DbConnection oleDbConnection = ((ObjectStorage)objectStorage).Connection;
                            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                                oleDbConnection.Open();
                            //if(System.EnterpriseServices.ContextUtil.IsInTransaction)
                            //    oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                            System.Data.Common.DbCommand oleDbCommand = oleDbConnection.CreateCommand();
                            oleDbCommand.CommandText = CommandText;
                            try
                            {
                                System.Data.Common.DbDataReader myReader = oleDbCommand.ExecuteReader();
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
        
        public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string StorageName, string StorageLocation)
        {
            System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
            storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
            string sqlServerInstanceName = @"localhost\SQLExpress";
            foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.MySQLPersistenceRunTime.StorageProvider"))
            {
                if (xmlElement.HasAttribute("DefaultInsatnceName") && !string.IsNullOrEmpty(xmlElement.GetAttribute("DefaultInsatnceName")))
                    sqlServerInstanceName = xmlElement.GetAttribute("DefaultInsatnceName");
                break;
            }

            MySQLMetaDataPersistenceRunTime.StorageProvider.CreateSQLDatabase(StorageName, sqlServerInstanceName);

            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {

                    try
                    {

                      

                        PersistenceLayer.ObjectStorage MetaDataStorageSession = ObjectStorage.NewStorage(StorageName,
                            StorageLocation,
                            "OOAdvantech.MySQLMetaDataPersistenceRunTime.StorageProvider", true);
                        string assemblyFullName = typeof(RDBMSPersistenceRunTime.Storage).Assembly.FullName;
                        MetaDataStorageSession.StorageMetaData.RegisterComponent(assemblyFullName);
                        // object tmp = PersistenceLayer.ObjectStorage.NewStorage(null, "UnitializedRawData", StorageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        //tmp MetaDataLoadingSystem.MetaDataStorageSession MetaDataStorageSession=(MetaDataLoadingSystem.MetaDataStorageSession)tmp;
                        RDBMSPersistenceRunTime.Storage mObjectStorage = new RDBMSPersistenceRunTime.Storage(StorageName, StorageLocation, "OOAdvantech.MySQLPersistenceRunTime.StorageProvider", GetDataBase(sqlServerInstanceName, StorageName));
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

        
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return false;
        }
        
        public override bool AllowEmbeddedStorage()
        {
            return false;
        }
        
        public override string GetHostComuterName(string StorageName, string StorageLocation)
        {
            int npos = StorageLocation.IndexOf('\\');
            if (npos == -1)
                return StorageLocation;
            else
                return StorageLocation.Substring(0, npos);

        }
        public override string GetInstanceName(string storageName, string storageLocation)
        {
            int npos = storageLocation.IndexOf('\\');
            if (npos == -1)
                return "default";
            else
                return storageLocation.Substring(npos + 1);
        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, object rawStorageData)
        {
            throw new System.Exception("OOAdvantech.MySQLPersistenceRunTime.StorageProvider doesn't suport raw storage data functionality.");
        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            throw new System.Exception("OOAdvantech.MySQLPersistenceRunTime.StorageProvider doesn't suport raw storage data functionality.");
        }
    }
}
