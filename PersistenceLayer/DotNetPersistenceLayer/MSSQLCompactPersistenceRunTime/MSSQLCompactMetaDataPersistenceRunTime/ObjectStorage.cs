using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MSSQLCompactFastPersistenceRunTime
{
    /// <MetaDataID>{92c41d31-80e8-4af7-81d5-f1fdecf78b1f}</MetaDataID>
    public class ObjectStorage : RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage
    {

        public override System.Data.Common.DbConnection SchemaUpdateConnection
        {
            get
            {
                return Connection;
            }
        }
        protected override OOAdvantech.MetaDataRepository.StorageCellReference GetStorageCellReference(OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }


        /// <MetaDataID>{8828cf12-95d7-4bd5-9fa6-5821a43f815c}</MetaDataID>
        static OOAdvantech.RDBMSMetaDataPersistenceRunTime.TypeDictionary _TypeDictionary;
        /// <MetaDataID>{d49ac85c-b322-4738-95dd-4b972de0e208}</MetaDataID>
        public override OOAdvantech.RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get 
            {
                if (_TypeDictionary == null)
                    _TypeDictionary = new TypeDictionary();

                return _TypeDictionary;

            }
        }
        


        /// <MetaDataID>{bc835794-c957-4e31-b16c-fb08c585f763}</MetaDataID>
        public override int NextOID
        {
            get
            {
                //TODO:αν κάτι πάει στραβά και η transaction κάνει abort τότε μένου αντικείμενα στον life time controller την επόμενη φορά η βάση μου
                //δίνει τα ίδια ids και κοπανάει.
                if (AllocatedOIDs.Count == 0)
                {
                    PersistenceLayerRunTime.TransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext as PersistenceLayerRunTime.TransactionContext;
                    System.Collections.ArrayList commandCollection = transactionContext.PrioritizeEnlistedCommands[10] as System.Collections.ArrayList;
                    int Count = 100;
                    if (commandCollection != null && commandCollection.Count > Count)
                        Count = commandCollection.Count;


//                    string allocateOIDsCommandText = @"declare @NEXTID int 
//                                                set @NEXTID=0 
//                                                SELECT @NEXTID=MAX(ID)FROM ObjectBLOBS  
//                                                if @NEXTID is NULL begin set @NEXTID=0 end 
//                                                if @NEXTID=0  
//                                                begin 
//                                                insert into IdentityTable (NEXTID) values(1) 
//                                                end 
//                                                set @NEXTID=@NEXTID+1 
//                                                set @FirstID=@NEXTID 
//                                                set @LastID=@NEXTID+@NumOfIDS-1 
//                                                select @FirstID As FirstID ,@LastID As LastID 
//                                                UPDATE IdentityTable  SET NEXTID=NEXTID+@NumOfIDS ";


                    if (Connection.State != System.Data.ConnectionState.Open)
                        Connection.Open();


                    System.Data.Common.DbCommand command = Connection.CreateCommand();
                    command.CommandText = "SELECT MAX(ID) as NEXTID FROM ObjectBLOBS ";
                    System.Data.Common.DbDataReader dataReader = command.ExecuteReader(); 
                    dataReader.Read();
                    int nextId = 0;
                    if (dataReader["NEXTID"] is System.DBNull)
                    {
                        dataReader.Close();
                        command.CommandText = "insert into IdentityTable (NEXTID) values(1)";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        nextId = (int)dataReader["NEXTID"];
                        dataReader.Close();
                    }
                    nextId++;
                    int firstID = nextId;
                    int lastID = nextId+Count-1;
                    command.CommandText = "UPDATE IdentityTable  SET NEXTID="+((int)nextId+Count).ToString();
                    command.ExecuteNonQuery();
                    for (int i = firstID; i <= lastID; i++)
                        AllocatedOIDs.Add(i);



                    ////Connection.EnlistTransaction(System.Transactions.Transaction.Current);
                    ////Connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                    //System.Data.Common.DbCommand command = Connection.CreateCommand();//new System.Data.SqlClient.SqlCommand(allocateOIDsCommandText,Connection);
                    //System.Data.Common.DbParameter parameter = command.CreateParameter();
                    //parameter.ParameterName = "@NumOfIDS";
                    //parameter.DbType = DbType.Int32;
                    //parameter.Value = Count;
                    //command.Parameters.Add(parameter);
                    ////command.Parameters.Add("@NumOfIDS",System.Data.SqlDbType.Int).Value=Count;
                    //System.Data.Common.DbParameter firstID = command.CreateParameter();
                    //firstID.ParameterName = "@FirstID";
                    //firstID.DbType = DbType.Int32;
                    //command.Parameters.Add(firstID);//command.Parameters.Add("@FirstID", System.Data.SqlDbType.Int);

                    ////System.Data.SqlClient.SqlParameter  LastID=command.Parameters.Add("@LastID",System.Data.SqlDbType.Int);
                    //System.Data.Common.DbParameter lastID = command.CreateParameter();
                    //lastID.ParameterName = "@LastID";
                    //lastID.DbType = DbType.Int32;
                    //command.Parameters.Add(lastID);

                    //firstID.Direction = System.Data.ParameterDirection.InputOutput;
                    //lastID.Direction = System.Data.ParameterDirection.InputOutput;
                    //command.ExecuteNonQuery();
                    //for (int i = (int)firstID.Value; i <= (int)lastID.Value; i++)
                    //    AllocatedOIDs.Add(i);
                }
                int nextOID = (int)AllocatedOIDs[AllocatedOIDs.Count - 1];
                AllocatedOIDs.RemoveAt(AllocatedOIDs.Count - 1);
                return nextOID;
            }
        }

        /// <MetaDataID>{8195e112-cf95-44ab-8a00-957ea33f78d6}</MetaDataID>
        public static System.TimeSpan timsp;
        /// <MetaDataID>{073c09ef-7f0a-4a6f-a972-065d1e9909ee}</MetaDataID>
        public ObjectStorage(string storageName, string storageLocation, bool newStorage)
        {
            double tim = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
            //Data Source=C:\PocketPC\Storage.sdf
            SQLConnectionString = "Data Source=[Location]";//;Initial Catalog=[DatabaseName];Integrated Security=True";
            //SQLConnectionString = SQLConnectionString.Replace("[DatabaseName]", storageName);
            SQLConnectionString = SQLConnectionString.Replace("[Location]", storageLocation);//  @"localhost\SQLExpress");

            _StorageMetaData = new Storage(storageName, storageLocation, "OOAdvantech.MSSQLCompactFastPersistenceRunTime.StorageProvider", Connection, newStorage);
            System.DateTime start = System.DateTime.Now;
            if (!newStorage)
                LoadStorageObjects();
            timsp = System.DateTime.Now - start;
            
            
        }
        /// <MetaDataID>{9eed2c57-8b8c-46b2-b86f-cb3475d1a385}</MetaDataID>
        protected override void LoadStorageObjects()
        {

            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();
            System.Data.Common.DbCommand command = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS", Connection);
            command.CommandText = "SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS";
            System.Data.Common.DbDataReader dataReader = command.ExecuteReader();
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
                RDBMSMetaDataPersistenceRunTime.StorageInstanceRef storageInstanceRef = CreateStorageInstanceRef(NewObject,new ObjectID( OID)) as RDBMSMetaDataPersistenceRunTime.StorageInstanceRef;
                double tima = (System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds;
                int offset = 0;
                storageInstanceRef.LoadObjectState(byteStream, offset, out offset);
                //storageInstanceRef.SnapshotStorageInstance();
                storageInstanceRef.ObjectActived();
                if (NewObject is RDBMSMetaDataRepository.Component)
                {
#if!NETCompactFramework
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

            foreach (System.Collections.Generic.KeyValuePair<PersistenceLayer.ObjectID,RDBMSMetaDataPersistenceRunTime.StorageInstanceRef> entry in StorageObjects)
            {
                RDBMSMetaDataPersistenceRunTime.StorageInstanceRef storageInstanceRef = entry.Value as RDBMSMetaDataPersistenceRunTime.StorageInstanceRef;
                storageInstanceRef.ResolveRelationships();
            }

            Connection.Close();


        }


           /// <MetaDataID>{3b401a4a-b2c0-4511-9766-6d10ddfcbc51}</MetaDataID>
        public override void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection)
        {
            //base.CreateBuildContainedObjectIndiciesCommand(collection);
        }
        /// <MetaDataID>{8e726e62-dab9-412b-bf22-054c9b570f7c}</MetaDataID>
        public override System.Data.Common.DbConnection Connection
        {
            get
            {

                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    string transactionUri = "WithoutTransaction";
                    if (OOAdvantech.Transactions.Transaction.Current!= null)
                        transactionUri = OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri;

                    System.Data.Common.DbConnection _DBConnection = null;// DatabaseConections[transactionUri] as System.Data.Common.DbConnection;

                    if (!DatabaseConections.TryGetValue(transactionUri, out _DBConnection))
                    {
                        _DBConnection = new System.Data.SqlServerCe.SqlCeConnection(SQLConnectionString);
                        DatabaseConections[transactionUri] = _DBConnection;
                        if (transactionUri != "WithoutTransaction")
                            Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);

                        //_DBConnection.Open();
                    }
                    return _DBConnection;

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }

        }

        public override string GetSQLScriptForName(string name)
        {
            return "[" + name + "]";
        }
        public override string GetAdoNetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }


        public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader CreateDataLoader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata dataLoaderMetadata)
        {
            return new RDBMSMetaDataPersistenceRunTime.ObjectQueryLanguage.DataLoader(dataNode, dataLoaderMetadata);
        }


        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstanceAgent)
        {
            throw new NotImplementedException();
        }

        public override OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID, OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }
    }



}
