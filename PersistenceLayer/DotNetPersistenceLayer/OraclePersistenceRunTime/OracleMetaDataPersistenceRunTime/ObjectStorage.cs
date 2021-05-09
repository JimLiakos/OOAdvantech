using System;
using System.Data;
using OOAdvantech.RDBMSMetaDataPersistenceRunTime;
using OOAdvantech.Transactions;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;

namespace OOAdvantech.OracleMetaDataPersistenceRunTime
{


    /// <MetaDataID>{36502eca-058f-4f32-be4a-8d36fbd660d4}</MetaDataID>
    public class ObjectStorage : OOAdvantech.RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage
    {

        protected override OOAdvantech.MetaDataRepository.StorageCellReference GetStorageCellReference(OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }

        
        public override string GetAdoNetParameterName(string parameterName)
        {
            return ":" + parameterName.ToUpper();
        }
        public override string GetSQLScriptForName(string name)
        {
            return "\"" + name.ToUpper() + "\"";
        }
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstanceAgent)
        {
            throw new NotImplementedException();
        }
        public override OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID, OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }
        public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader CreateDataLoader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata dataLoaderMetadata)
        {
            return new RDBMSMetaDataPersistenceRunTime.ObjectQueryLanguage.DataLoader(dataNode, dataLoaderMetadata);
        }


        public override IDataBaseConnection SchemaUpdateConnection
        {
            get 
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    stateTransition.Consistent = true;
                    using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                    {
                        transactionScope.Complete();
                        return Connection;
                    }
                }
            }
        }
        protected override void LoadStorageObjects()
        {

            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();
            var command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS", Connection);
            command.CommandText = string.Format("SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS", StorageMetaData.StorageName);
            var dataReader = command.ExecuteReader();
            int lastOID = 0;
            int count = 0;
             
            while (dataReader.Read())
            {
                int OID =System.Convert.ToInt32(dataReader["ID"]);
                if (OID > lastOID)
                    lastOID = OID;
                int classBLOBSID = System.Convert.ToInt32(dataReader["ClassBLOBSID"]);
                byte[] byteStream = (byte[])dataReader["ObjectData"];
                //if (OID == 667)
                //{
                //   System.IO.FileStream stream= System.IO.File.Open(@"C:\Load.bin", System.IO.FileMode.CreateNew);
                //   stream.Write(byteStream, 0, byteStream.Length);
                //   stream.Close();

                //}
                System.Type memoryInstanceType = (_StorageMetaData as Storage).GetClassBLOB(classBLOBSID).Class.GetExtensionMetaObject(typeof(System.Type)) as System.Type;

                double tim1 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                object NewObject = AccessorBuilder.CreateInstance(memoryInstanceType);
                double tim2 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                StorageInstanceRef storageInstanceRef = CreateStorageInstanceRef(NewObject,new ObjectID( OID)) as StorageInstanceRef;
                double tima = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                int offset = 0;
                storageInstanceRef.LoadObjectState(byteStream, offset, out offset);
                //storageInstanceRef.SnapshotStorageInstance();
                storageInstanceRef.ObjectActived();
                if (NewObject is RDBMSMetaDataRepository.Component)
                {
#if!DeviceDotNet
                    if (!string.IsNullOrEmpty((NewObject as RDBMSMetaDataRepository.Component).AssemblyString))
                        System.Reflection.Assembly.Load((NewObject as RDBMSMetaDataRepository.Component).AssemblyString);
#else
                    if(!string.IsNullOrEmpty((NewObject as RDBMSMetaDataRepository.Component).AssemblyString))
                        ModulePublisher.ClassRepository.LoadedAssemblies.Add(System.Reflection.Assembly.Load((NewObject as RDBMSMetaDataRepository.Component).AssemblyString));
#endif

                }
                int asw = 0;
                count++;
            }
            dataReader.Close();
            //double tim3 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;

            foreach (System.Collections.Generic.KeyValuePair<PersistenceLayer.ObjectID, StorageInstanceRef> entry in StorageObjects)
            {
                StorageInstanceRef storageInstanceRef = entry.Value as StorageInstanceRef;
                storageInstanceRef.ResolveRelationships();

            }


        }
        //public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
        //{
        //    Commands.NewStorageInstanceCommand newStorageInstanceCommand = new Commands.NewStorageInstanceCommand(StorageInstance as StorageInstanceRef);
        //    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
        //    transactionContext.EnlistCommand(newStorageInstanceCommand);
        //}

        //public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        //{
        //    Commands.UpdateStorageInstanceCommand updateStorageInstanceCommand = new Commands.UpdateStorageInstanceCommand(storageInstanceRef);
        //    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
        //    transactionContext.EnlistCommand(updateStorageInstanceCommand);
        //}

        //public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, PersistenceLayer.DeleteOptions deleteOption)
        //{
        //    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(storageInstance))
        //    {
        //        Commands.DeleteStorageInstanceCommand Command = new Commands.DeleteStorageInstanceCommand(storageInstance, deleteOption);
        //        PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
        //        if (!transactionContext.ContainCommand(Command.Identity))
        //            transactionContext.EnlistCommand(Command);
        //        stateTransition.Consistent = true;
        //    }
        //}


        /// <MetaDataID>{272e2e99-7cc3-4e1d-82e3-5ee7cf8eace2}</MetaDataID>
        static OOAdvantech.RDBMSMetaDataPersistenceRunTime.TypeDictionary _TypeDictionary;
        public override OOAdvantech.RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get
            {
                if (_TypeDictionary == null)
                    _TypeDictionary = new TypeDictionary();
                return _TypeDictionary;
            }
        }


        /// <MetaDataID>{befaf3de-eac1-45f3-9d23-e7a045f07f75}</MetaDataID>
        ~ObjectStorage()
        {
            try
            {
                if (Connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    Connection.Close();
            }
            catch (System.Exception)
            {
            }
        }
        public override void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection)
        {
            //base.CreateBuildContainedObjectIndiciesCommand(collection);
        }
        //internal CacheEngine Engine; 

        /// <MetaDataID>{c43b37fe-a057-4938-a9d6-00018ae8765e}</MetaDataID>
        public ObjectStorage(string storageName, string storageLocation, bool newStorage, bool embedded)
        {

            //SQLConnectionString = "Data Source=[Location];User ID=[User];Password=astraxan;Unicode=True";// "Server=[Location];UserId=root;Password=astraxan";// "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";
            SQLConnectionString = "Data Source=[Location];User ID=[User];Password=astraxan";// "Server=[Location];UserId=root;Password=astraxan";// "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";

            //SQLConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=rocket)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=[Location])));User Id=[User];Password=astraxan;";
            if (!embedded)
            {
                if (OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.InStorageService)
                {
                    string sqlServerInstanceName = @"localhost";
                    System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                    storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
                    foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.OraclePersistenceRunTime.StorageProvider"))
                    {
                        if (xmlElement.HasAttribute("DefaultInsatnceName") && !string.IsNullOrEmpty(xmlElement.GetAttribute("DefaultInsatnceName")))
                            sqlServerInstanceName = xmlElement.GetAttribute("DefaultInsatnceName");
                        break;
                    }
                    SQLConnectionString = SQLConnectionString.Replace("[Location]", sqlServerInstanceName);
                }
                else
                    SQLConnectionString = SQLConnectionString.Replace("[Location]", @"localhost");
            }
            else
                SQLConnectionString = SQLConnectionString.Replace("[Location]", storageLocation).Replace("[User]", storageName);
            //OOAdvantech.MySQLMetaDataPersistenceRunTime.EmbeddedStorageProvider
            if (embedded)
                _StorageMetaData = new Storage(storageName, storageLocation, "OOAdvantech.OracleMetaDataPersistenceRunTime.EmbeddedStorageProvider", Connection, newStorage);
            else
                _StorageMetaData = new Storage(storageName, storageLocation, "OOAdvantech.OracleMetaDataPersistenceRunTime.EmbeddedStorageProvider", Connection, newStorage);
            if (!newStorage)
                LoadStorageObjects();
        }
        /// <MetaDataID>{04efcba1-79c0-4b55-8844-1d6ef2f8cc7f}</MetaDataID>
        protected ObjectStorage()
        {
        }
        public override int NextOID
        {
            get
            {
                //TODO:αν κάτι πάει στραβά και η transaction κάνει abort τότε μένου αντικείμενα στον life time controller την επόμενη φορά η βάση μου
                //δίνει τα ίδια ids και κοπανάει.
                if (AllocatedOIDs.Count == 0)
                {
                    PersistenceLayerRunTime.TransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext as PersistenceLayerRunTime.TransactionContext;
                    System.Collections.Generic.List<OOAdvantech.PersistenceLayerRunTime.Commands.Command> commandCollection = transactionContext.PrioritizeEnlistedCommands[10] ;
                    int Count = 100;
                    if (commandCollection != null && commandCollection.Count > Count)
                        Count = commandCollection.Count;
                    using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                    {
                        System.Data.Common.DbConnection connection = new Oracle.DataAccess.Client.OracleConnection(Connection.ConnectionString + ";Password = astraxan");
                        connection.Open();

                        System.Data.Common.DbCommand command = connection.CreateCommand();
                        command.CommandText = string.Format("SELECT NEXTID as NEXTID FROM IDENTITYTABLE ", StorageMetaData.StorageName);
                        System.Data.Common.DbDataReader dataReader = command.ExecuteReader();
                        ;
                        int nextId = 0;
                        if (!dataReader.Read() )
                        {
                            dataReader.Close();
                            command.CommandText = string.Format("insert into IdentityTable (NEXTID) values(1)", StorageMetaData.StorageName);
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            nextId = (int)(decimal)dataReader["NEXTID"];
                            dataReader.Close();
                        }
                        nextId++;
                        int firstID = nextId;
                        int lastID = nextId + Count - 1;
                        command.CommandText = string.Format("UPDATE IdentityTable  SET NEXTID=" + ((int)nextId + Count).ToString(), StorageMetaData.StorageName);
                        command.ExecuteNonQuery();
                        for (int i = firstID; i <= lastID; i++)
                            AllocatedOIDs.Add(i);
                        connection.Close();
                        transactionScope.Complete();
                    }

                }
                int nextOID = (int)AllocatedOIDs[0];
                AllocatedOIDs.RemoveAt(0);
                return nextOID;
            }
        }

        //        public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, PersistenceLayer.DeleteOptions deleteOption)
        //        {
        //            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(storageInstance))
        //            {
        //                Commands.DeleteStorageInstanceCommand Command = new Commands.DeleteStorageInstanceCommand(storageInstance, deleteOption);
        //                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
        //                if (!transactionContext.ContainCommand(Command.Identity))
        //                    transactionContext.EnlistCommand(Command);
        //                stateTransition.Consistent = true;
        //            }
        //        }

        //        /// <MetaDataID>{980E3A51-0F9C-4F23-94F1-BC3BAB4DE57C}</MetaDataID>
        //        protected string SQLConnectionString = null;
        //        /// <MetaDataID>{adfc9fc8-c56a-4e13-be19-6742c5fdcf2e}</MetaDataID>
        //        public OOAdvantech.Collections.Generic.Dictionary<string,System.Data.Common.DbConnection> DatabaseConections = new OOAdvantech.Collections.Generic.Dictionary<string,System.Data.Common.DbConnection>();

        public override IDataBaseConnection Connection
        {
            get
            {
                string transactionUri = "WithoutTransaction";
                Transactions.Transaction currentTransaction = Transactions.Transaction.Current;
                if (currentTransaction != null)
                    transactionUri = currentTransaction.LocalTransactionUri;


                IDataBaseConnection _DBConnection = null;
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (!DatabaseConections.TryGetValue(transactionUri, out _DBConnection))
                    {
                        OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                        try
                        {
                            //if (currentTransaction != null && System.Transactions.Transaction.Current == null)
                            //{
                            //    using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(currentTransaction) as System.Transactions.Transaction))
                            //    {
                            //        _DBConnection = new MySql.Data.MySqlClient.MySqlConnection(SQLConnectionString);// StorageProvider.GetMySqlConnection(SQLConnectionString);

                            //        DatabaseConections[transactionUri] = _DBConnection;
                            //        if (transactionUri != "WithoutTransaction")
                            //            Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);
                            //        transactionScope.Complete();
                            //    }
                            //}
                            //else
                            {
                                _DBConnection = new RDBMSMetaDataPersistenceRunTime.DataBaseConnection( new OracleConnection(SQLConnectionString));// StorageProvider.GetMySqlConnection(SQLConnectionString);

                                DatabaseConections[transactionUri] = _DBConnection;
                                if (transactionUri != "WithoutTransaction")
                                    Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);
                            }
                        }
                        finally
                        {
                            ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                        }
                        //_DBConnection.Open();
                    }

                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
                return _DBConnection;

            }
        }




        public override IDataBaseConnection GetConnectionFor(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            IDataBaseConnection _DBConnection = null;
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (DatabaseConections.TryGetValue(theTransaction.Transaction.LocalTransactionUri, out _DBConnection))
                    return _DBConnection;
                else
                    return null;
                //_DBConnection.Open();
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }
    }
}
