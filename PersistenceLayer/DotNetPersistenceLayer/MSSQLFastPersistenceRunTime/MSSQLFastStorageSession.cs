using System;
using System.Data;
using OOAdvantech.RDBMSMetaDataPersistenceRunTime;

namespace OOAdvantech.MSSQLFastPersistenceRunTime
{

    /// <MetaDataID>{d82f6401-bb1c-4a67-abba-d360c42bda63}</MetaDataID>
    public class ObjectStorage : OOAdvantech.RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage
    {
      
        public override string GetSQLScriptForName(string name)
        {
            return "[" + name + "]";
        }
        public override string GetAdoNetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }

        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstanceAgent)
        {
            throw new NotImplementedException();
        }

        protected override OOAdvantech.MetaDataRepository.StorageCellReference GetStorageCellReference(OOAdvantech.MetaDataRepository.StorageCell storageCell)
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
                return Connection;
            }
        }
        public override OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID, OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }


        protected override void LoadStorageObjects()
        {

            if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                Connection.Open();
            var command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS", Connection);
            command.CommandText = "SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS";
            var dataReader = command.ExecuteReader();
            int lastOID = 0;
            int count = 0;

            while (dataReader.Read())
            {
                int OID = (int)dataReader["ID"];
                if (OID > lastOID)
                    lastOID = OID;
                int classBLOBSID = (int)dataReader["ClassBLOBSID"];
                byte[] byteStream = (byte[])dataReader["ObjectData"];
                System.Type memoryInstanceType = (_StorageMetaData as Storage).GetClassBLOB(classBLOBSID).Class.GetExtensionMetaObject(typeof(System.Type)) as System.Type;

                double tim1 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                object NewObject = AccessorBuilder.CreateInstance(memoryInstanceType);
                double tim2 = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                RDBMSMetaDataPersistenceRunTime.StorageInstanceRef storageInstanceRef = CreateStorageInstanceRef(NewObject, new ObjectID(OID)) as StorageInstanceRef;
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

        /// <MetaDataID>{0ea21f2b-33d3-44e7-af25-8bb37cdc15e6}</MetaDataID>
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


        public override void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection)
        {
            //base.CreateBuildContainedObjectIndiciesCommand(collection);
        }
        //internal CacheEngine Engine; 
        /// <MetaDataID>{3A61EDEE-9244-45E4-AAC3-356CD6348B17}</MetaDataID>
        public ObjectStorage(string storageName, string storageLocation, bool newStorage, bool embedded)
        {

            SQLConnectionString = "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";
            SQLConnectionString = SQLConnectionString.Replace("[DatabaseName]", storageName);

            if (!embedded)
            {
                if (OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.InStorageService)
                {
                    string sqlServerInstanceName = @"localhost\SQLExpress";
                    System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                    storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
                    foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.MSSQLPersistenceRunTime.StorageProvider"))
                    {
                        if (xmlElement.HasAttribute("DefaultInsatnceName") && !string.IsNullOrEmpty(xmlElement.GetAttribute("DefaultInsatnceName")))
                            sqlServerInstanceName = xmlElement.GetAttribute("DefaultInsatnceName");
                        break;
                    }
                    SQLConnectionString = SQLConnectionString.Replace("[Location]", sqlServerInstanceName);
                }
                else
                    SQLConnectionString = SQLConnectionString.Replace("[Location]", @"localhost\SQLExpress");
            }
            else
                SQLConnectionString = SQLConnectionString.Replace("[Location]", @"localhost\SQLExpress");
            //OOAdvantech.MSSQLFastPersistenceRunTime.EmbeddedStorageProvider
            if (embedded)
                _StorageMetaData = new Storage(storageName, storageLocation, "OOAdvantech.MSSQLFastPersistenceRunTime.EmbeddedStorageProvider", Connection, newStorage);
            else
                _StorageMetaData = new Storage(storageName, storageLocation, "OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider", Connection, newStorage);
            if (!newStorage)
                LoadStorageObjects();
        }
        public ObjectStorage(string storageName,string storageLocation,string sqlConnectionString,bool newStorage)
        {

            SQLConnectionString = sqlConnectionString;
            _StorageMetaData = new Storage(storageName, storageLocation, "OOAdvantech.MSSQLFastPersistenceRunTime.EmbeddedStorageProvider", Connection, newStorage);
            if (!newStorage)
                LoadStorageObjects();
        }
        /// <MetaDataID>{afb3d706-5bea-4175-862f-994699a84762}</MetaDataID>
        protected ObjectStorage()
        {
            System.AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
        }

        void CurrentDomain_ProcessExit(object sender, EventArgs e)
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

        //        /// <MetaDataID>{942E0EFA-0A58-4CC0-950D-DFB87D6B95FE}</MetaDataID>
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
        /// <MetaDataID>{4a78332a-024d-4d1f-9b90-5c9e9a3baab1}</MetaDataID>
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
                            _DBConnection =new DataBaseConnection( new System.Data.SqlClient.SqlConnection(SQLConnectionString));
                            DatabaseConections[transactionUri] = _DBConnection;
                            if (transactionUri != "WithoutTransaction")
                                Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);
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
            return null;
        }
    }
}
