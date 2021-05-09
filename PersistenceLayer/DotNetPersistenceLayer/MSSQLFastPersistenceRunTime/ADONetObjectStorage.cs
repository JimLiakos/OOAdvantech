using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
    /// <MetaDataID>{A1464C5A-F38C-4E61-A5E3-7634AC15E93C}</MetaDataID>
    public abstract class AdoNetObjectStorage : OOAdvantech.PersistenceLayerRunTime.ObjectStorage
    {

        /// <MetaDataID>{d744b964-e586-4880-855a-ced863c159a1}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery DistributeObjectQuery(OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> dataTrees,
            OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> selectListItems,
             OOAdvantech.Collections.Generic.Dictionary<Guid, MetaDataRepository.ObjectQueryLanguage.StorageDataSource.DataLoaderMetadata> storageCells)
        {
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in dataTrees)
                InitializeMetaData(dataNode);


            MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery distributedObjectQuery = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery(dataTrees, selectListItems);
            foreach (System.Collections.Generic.KeyValuePair<Guid, MetaDataRepository.ObjectQueryLanguage.StorageDataSource.DataLoaderMetadata> entry in storageCells)
            {
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataSource dataSource = distributedObjectQuery.GetDataSource(entry.Key);
                dataSource.DataLoaders.Add(StorageMetaData.StorageIdentity, new ObjectQueryLanguage.DataLoader(dataSource.DataNode, entry.Value));
            }

            OOAdvantech.Collections.Generic.Dictionary<Guid, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader> dataLoaders = distributedObjectQuery.DataLoaders;
            return distributedObjectQuery;
        }
        /// <MetaDataID>{54381c2a-1d25-4b99-bb8b-5ef7d380256d}</MetaDataID>
        void InitializeMetaData(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        {
            if (dataNode.AssignedMetaObject == null && dataNode.AssignedMetaObjectIdenty != null)
                dataNode.AssignedMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(dataNode.AssignedMetaObjectIdenty);
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in dataNode.SubDataNodes)
                InitializeMetaData(subDataNode);
        }




        /// <MetaDataID>{55c57860-2334-4852-88ff-b5ecfea728ba}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ValueTypePath valueTypePath, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCell)
        {
            return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
        }
        /// <MetaDataID>{adf799c4-67a9-432d-9ef7-6c5b39a3bb13}</MetaDataID>
        System.Collections.Generic.Dictionary<MetaDataRepository.Class, OOAdvantech.MetaDataRepository.StorageCell> StorageCells = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.Class, OOAdvantech.MetaDataRepository.StorageCell>();
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();

            if ((classifier is MetaDataRepository.Class) && (!(classifier as MetaDataRepository.Class).Abstract))
            {

                System.Type type = classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                PersistenceLayerRunTime.ClassMemoryInstanceCollection classObjects = OperativeObjectCollections[type] as PersistenceLayerRunTime.ClassMemoryInstanceCollection;

                if (StorageCells.ContainsKey(classifier as MetaDataRepository.Class))
                    storageCells.Add(StorageCells[classifier as MetaDataRepository.Class]);
                else
                {

                    StorageCell storageCell = new StorageCell(_StorageMetaData.StorageIdentity, classifier as MetaDataRepository.Class, _StorageMetaData as MetaDataRepository.Namespace, classObjects);

                    storageCells.Add(storageCell);
                    StorageCells[classifier as MetaDataRepository.Class] = storageCell;
                }

            }
            foreach (MetaDataRepository.Classifier subClassifier in classifier.GetAllSpecializeClasifiers())
            {
                if (!(subClassifier is MetaDataRepository.Class) || (subClassifier as MetaDataRepository.Class).Abstract)
                    continue;

                System.Type type = subClassifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                PersistenceLayerRunTime.ClassMemoryInstanceCollection classObjects = OperativeObjectCollections[type] as PersistenceLayerRunTime.ClassMemoryInstanceCollection;

                if (StorageCells.ContainsKey(subClassifier as MetaDataRepository.Class))
                    storageCells.Add(StorageCells[subClassifier as MetaDataRepository.Class]);
                else
                {

                    StorageCell storageCell = new StorageCell(_StorageMetaData.StorageIdentity, subClassifier as MetaDataRepository.Class, _StorageMetaData as MetaDataRepository.Namespace, classObjects);

                    storageCells.Add(storageCell);
                    StorageCells[subClassifier as MetaDataRepository.Class] = storageCell;
                }
            }

            return storageCells;


        }



        public override object GetObject(string persistentObjectUri)
        {
            int objectID = int.Parse(persistentObjectUri);
            StorageInstanceRef storageInstanceRef = StorageObjects[objectID] as StorageInstanceRef;
            if (storageInstanceRef == null)
                return null;
            else
                return storageInstanceRef.MemoryInstance;

        }


        public override string GetPersistentObjectUri(object obj)
        {
            if (obj is StorageInstanceRef)
            {
                StorageInstanceRef storageInstanceRef = obj as StorageInstanceRef;
                if (storageInstanceRef != null && storageInstanceRef.ObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.ObjectID.ToString();
            }
            else if (PersistencyService.ClassOfObjectIsPersistent(obj))
            {
                PersistenceLayer.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(obj);
                if (storageInstanceRef != null && storageInstanceRef.ObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.ObjectID.ToString();
            }
            return null;
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8C1D2C04-3D46-4D2D-9FC5-3FCDD548C1F9}</MetaDataID>
        protected OOAdvantech.PersistenceLayer.Storage _StorageMetaData;
        /// <MetaDataID>{EF6BDB63-E195-464C-B163-92A5E492DB57}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.Storage StorageMetaData
        {
            get
            {
                return _StorageMetaData;
            }
        }



        /// <MetaDataID>{461A8B2B-76A4-430A-9CBD-18FA88D59B58}</MetaDataID>
        ~AdoNetObjectStorage()
        {
            try
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                    Connection.Close();
            }
            catch (System.Exception)
            {
            }
        }
        //internal CacheEngine Engine; 
        ///// <MetaDataID>{3A61EDEE-9244-45E4-AAC3-356CD6348B17}</MetaDataID>
        //public AdoNetObjectStorage(string storageName, string storageLocation, bool newStorage)
        //{

        //    SQLConnectionString = "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";
        //    SQLConnectionString = SQLConnectionString.Replace("[DatabaseName]", storageName);
        //    SQLConnectionString = SQLConnectionString.Replace("[Location]", @"localhost\SQLExpress");
        //    _StorageMetaData = new Storage(storageName, storageLocation, Connection, newStorage);
        //    if (!newStorage)
        //        LoadStorageObjects();
        //}
        protected AdoNetObjectStorage()
        {
        }

        /// <MetaDataID>{942E0EFA-0A58-4CC0-950D-DFB87D6B95FE}</MetaDataID>
        public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, PersistenceLayer.DeleteOptions deleteOption)
        {
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(storageInstance))
            {
                Commands.DeleteStorageInstanceCommand Command = new Commands.DeleteStorageInstanceCommand(storageInstance, deleteOption);
                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                if (!transactionContext.ContainCommand(Command.Identity))
                    transactionContext.EnlistCommand(Command);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{980E3A51-0F9C-4F23-94F1-BC3BAB4DE57C}</MetaDataID>
        protected string SQLConnectionString = null;
        /// <MetaDataID>{adfc9fc8-c56a-4e13-be19-6742c5fdcf2e}</MetaDataID>
        public OOAdvantech.Collections.Generic.Dictionary<string, System.Data.Common.DbConnection> DatabaseConections = new OOAdvantech.Collections.Generic.Dictionary<string, System.Data.Common.DbConnection>();
        /// <MetaDataID>{4a78332a-024d-4d1f-9b90-5c9e9a3baab1}</MetaDataID>
        public abstract System.Data.Common.DbConnection Connection
        {
            get;
        }
        //{
        //    get
        //    {
        //        OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
        //        try
        //        {
        //            string transactionUri = "WithoutTransaction";
        //            if (System.Transactions.Transaction.Current != null)
        //                transactionUri = System.Transactions.Transaction.Current.TransactionInformation.LocalIdentifier;

        //            System.Data.SqlClient.SqlConnection _DBConnection = DatabaseConections[transactionUri] as System.Data.SqlClient.SqlConnection;
        //            if (_DBConnection == null)
        //            {
        //                _DBConnection = new System.Data.SqlClient.SqlConnection(SQLConnectionString);
        //                DatabaseConections[transactionUri] = _DBConnection;
        //                if (transactionUri != "WithoutTransaction")
        //                    Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);

        //                //_DBConnection.Open();
        //            }
        //            return _DBConnection;

        //        }
        //        finally
        //        {
        //            ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
        //        }
        //    }
        //}

        /// <MetaDataID>{d66f8311-9341-40d5-87eb-4ce232cd09cb}</MetaDataID>
        protected void OnTransactionCompletted(OOAdvantech.Transactions.Transaction transaction)
        {
            System.Data.Common.DbConnection _DBConnection = DatabaseConections[transaction.LocalTransactionUri];
            if (_DBConnection != null)
            {
                _DBConnection.Close();
                DatabaseConections.Remove(transaction.LocalTransactionUri);
            }
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompletted);



        }

        /// <MetaDataID>{FB9DB25A-6F86-44E3-8E39-897FF3EE32A6}</MetaDataID>
        protected System.Collections.ArrayList AllocatedOIDs = new System.Collections.ArrayList();
        /// <MetaDataID>{B666001C-B832-4E1C-9782-887CB1C6A404}</MetaDataID>
        public virtual int NextOID
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


                    string allocateOIDsCommandText = "declare @NEXTID int " +
                                                "set @NEXTID=0 " +
                                                "SELECT @NEXTID=MAX(ID)FROM ObjectBLOBS  " +
                                                "if @NEXTID is NULL begin set @NEXTID=0 end " +
                                                "if @NEXTID=0  " +
                                                "begin " +
                                                "insert into IdentityTable (NEXTID) values(1) " +
                                                "end " +
                                                "set @NEXTID=@NEXTID+1 " +
                                                "set @FirstID=@NEXTID " +
                                                "set @LastID=@NEXTID+@NumOfIDS-1 " +
                                                "select @FirstID As FirstID ,@LastID As LastID " +
                                                "UPDATE IdentityTable  SET NEXTID=NEXTID+@NumOfIDS ";


                    if (Connection.State != System.Data.ConnectionState.Open)
                        Connection.Open();
                    //Connection.EnlistTransaction(System.Transactions.Transaction.Current);
                    //Connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                    System.Data.Common.DbCommand command = Connection.CreateCommand();//new System.Data.SqlClient.SqlCommand(allocateOIDsCommandText,Connection);
                    command.CommandText = allocateOIDsCommandText;
                    System.Data.Common.DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = "@NumOfIDS";
                    parameter.DbType = DbType.Int32;
                    parameter.Value = Count;
                    command.Parameters.Add(parameter);
                    //command.Parameters.Add("@NumOfIDS",System.Data.SqlDbType.Int).Value=Count;
                    System.Data.Common.DbParameter firstID = command.CreateParameter();
                    firstID.ParameterName = "@FirstID";
                    firstID.DbType = DbType.Int32;
                    command.Parameters.Add(firstID);//command.Parameters.Add("@FirstID", System.Data.SqlDbType.Int);

                    //System.Data.SqlClient.SqlParameter  LastID=command.Parameters.Add("@LastID",System.Data.SqlDbType.Int);
                    System.Data.Common.DbParameter lastID = command.CreateParameter();
                    lastID.ParameterName = "@LastID";
                    lastID.DbType = DbType.Int32;
                    command.Parameters.Add(lastID);

                    firstID.Direction = System.Data.ParameterDirection.Output;
                    lastID.Direction = System.Data.ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    for (int i = (int)firstID.Value; i <= (int)lastID.Value; i++)
                        AllocatedOIDs.Add(i);
                }
                int nextOID = (int)AllocatedOIDs[AllocatedOIDs.Count - 1];
                AllocatedOIDs.RemoveAt(AllocatedOIDs.Count - 1);
                return nextOID;
            }
        }
        /// <MetaDataID>{8AB311AD-6251-4520-91CF-3F57FB30EB8F}</MetaDataID>
        public int Bytes = 0;

        /// <MetaDataID>{EA4FCDF2-A6F2-4CB2-AF6D-D48D779029D9}</MetaDataID>
        public override void BeginChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            //Net 'dbmslpcn';


            //string SQLConnectionString = "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";
            //SQLConnectionString=SQLConnectionString.Replace("[DatabaseName]",StorageMetaData.StorageName);
            //SQLConnectionString = SQLConnectionString.Replace("[Location]", StorageMetaData.StorageLocation + @"\SQLExpress");//"(local)");//;
            ////if(Connection !=null &&Connection.State==System.Data.ConnectionState.Open)
            //    Connection.Close();
            //Connection = new System.Data.SqlClient.SqlConnection(SQLConnectionString);
            Bytes = 0;

            /*Commands.BulkInsertCommand bulkInsertCommand=new OOAdvantech.MSSQLFastPersistenceRunTime.Commands.BulkInsertCommand(this);
            PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(bulkInsertCommand);*/


        }
        /// <MetaDataID>{EF21B8DB-EFD3-4532-83C9-D7C43EEA725F}</MetaDataID>
        public override void AbortChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            System.GC.Collect();
        }
        /// <MetaDataID>{A5E3EBD9-8AE6-4186-9D67-50EDAA11634F}</MetaDataID>
        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, object objectID)
        {
            return new StorageInstanceRef(memoryInstance, this, objectID);

        }
        /// <MetaDataID>{330574D7-D9C8-4EC2-A62D-4D854ACBDCD1}</MetaDataID>
        public override void CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.UnLinkObjectsCommand mLinkObjectsCommand = new Commands.UnLinkObjectsCommand(roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(mLinkObjectsCommand);
        }
        /*
        internal void bulktest()
        {
            /*
            DataTable ObjectBLOBS = new DataTable("ObjectBLOBS") ;
            ObjectBLOBS.Columns.Add("ID",typeof(System.Int32));
            ObjectBLOBS.Columns.Add("ClassBLOBSID",typeof(System.Int32));
            ObjectBLOBS.Columns.Add("ObjectData",typeof(byte[]));
            string selProc ="sp_SelectObjectBLOBS";
            string insProc ="";
            string delProc= "";
            string updProc = "";
            CacheEngine engine = new CacheEngine(ObjectBLOBS,selProc,insProc,delProc,updProc,Connection.ConnectionString,10000);

            int numIns =4000;
            long startTicks =DateTime.Now.Ticks ;
            object[] itemArr = new object[]{null,null,null};//,null,null,null};

            for (int i=12000;i<numIns+12000;i++)			
            {
                itemArr[0]=i;
                itemArr[1]=(int)4;
                itemArr[2]=System.Text.Encoding.ASCII.GetBytes("itemArr[1]=\"test\"+i.ToString();");
                engine.AddItem(itemArr) ;
            }
            engine.BulkInsertData();

            long endTicks=DateTime.Now.Ticks ;
            TimeSpan elapsed = new TimeSpan(endTicks-startTicks) ;
            double rows_Sec = numIns /elapsed.TotalSeconds;
            System.Diagnostics.Debug.WriteLine("inserted "+	rows_Sec.ToString() +" rows / second.");
            int erer=0;






        }*/
        /// <MetaDataID>{40623DD6-62AA-44BD-A27E-FF2D66E46D0F}</MetaDataID>
        public override void CommitChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();



        }
        /// <MetaDataID>{B26D33D2-5674-4878-87AA-4C06F0088C39}</MetaDataID>
        /// <summary>Executes the specified query in OQLStatement parameter. Return a StructureSet object that contain the result of object query statement. </summary>
        /// <param name="OQLStatement">A String value that contains the OQL statement </param>
        public override Collections.StructureSet Parse(string OQLStatement)
        {
            return null;

        }
        /// <MetaDataID>{2BFC4071-7C27-4745-BB21-58F964328A1F}</MetaDataID>
        public override Collections.StructureSet Execute(string OQLStatement)
        {
            MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
            mStructureSet.Open(OQLStatement, null);
#if !NETCompactFramework
            return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);
#else
            return mStructureSet;
#endif



            //StructureSet structureSet=new StructureSet();
            //structureSet.SourceStorageSession=this;
            //structureSet.Open(OQLStatement);
            //return structureSet;

        }
        /// <MetaDataID>{CF64AE2F-795A-4907-810D-81FEF2955AA3}</MetaDataID>
        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
        {
            MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
            mStructureSet.Open(OQLStatement, parameters);
#if !NETCompactFramework
            return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);
#else
            return mStructureSet;
#endif
        }






        /// <MetaDataID>{4A98936E-A4C8-4340-9B08-AA79505F84FB}</MetaDataID>
        public override void MakeChangesDurable(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }
        /// <MetaDataID>{346FFC65-03A4-45BD-845A-FCCAECB35316}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceRef deletedStorageInstanceRef, DotNetMetaDataRepository.AssociationEnd AssociationEnd)
        {
            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(deletedStorageInstanceRef))
            {
                Commands.UnlinkAllObjectCommand mUnlinkAllObjectCommand = new Commands.UnlinkAllObjectCommand(deletedStorageInstanceRef);
                //mUnlinkAllObjectCommand.DeletedStorageInstance = deletedStorageInstanceRef;
                mUnlinkAllObjectCommand.theAssociationEnd = AssociationEnd;

                PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                transactionContext.EnlistCommand(mUnlinkAllObjectCommand);

                stateTransition.Consistent = true;
            }


        }
        public override void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection)
        {
            throw new NotImplementedException();
        }
      

        /// <MetaDataID>{44FBFD64-3E25-4187-9C6A-DE55F768327F}</MetaDataID>
        public override void CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.LinkObjectsCommand mLinkObjectsCommand = new Commands.LinkObjectsCommand(roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(mLinkObjectsCommand);

        }
        /// <MetaDataID>{9C2B7262-B030-425E-A5A9-DDB0E525CAC1}</MetaDataID>
        /// <summary>UpdateStorageInstanceCommand </summary>
        public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {

            Commands.UpdateStorageInstanceCommand updateStorageInstanceCommand = new Commands.UpdateStorageInstanceCommand(storageInstanceRef);
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(updateStorageInstanceCommand);


        }

        public override void CreateUnlinkAllObjectCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef deletedOutStorageInstanceRef, OOAdvantech.MetaDataRepository.AssociationEnd AssociationEnd, OOAdvantech.MetaDataRepository.StorageCell LinkedObjectsStorageCell)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{F4802055-2BC4-4792-97CE-2679C47E7FC0}</MetaDataID>
        internal void DeleteStorageInstance(PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef)
        {

        }
        /// <MetaDataID>{49531E21-F3A1-4856-88B8-213211611817}</MetaDataID>
        public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
        {
            Commands.NewStorageInstanceCommand newStorageInstanceCommand = new Commands.NewStorageInstanceCommand(StorageInstance as StorageInstanceRef);
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(newStorageInstanceCommand);


        }


        /// <MetaDataID>{59214C05-14A6-4CF4-B3CB-195493D268FB}</MetaDataID>
        public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {

        }
        /// <MetaDataID>{48062702-BAEA-4646-9E29-83B4D9721EC5}</MetaDataID>
        internal Collections.Generic.Dictionary<object, object> StorageObjects = new Collections.Generic.Dictionary<object, object>(1000);
        /// <MetaDataID>{E1158FDD-78A9-45ED-B04B-908067526F22}</MetaDataID>
        protected void LoadStorageObjects()
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
                StorageInstanceRef storageInstanceRef = CreateStorageInstanceRef(NewObject, OID) as StorageInstanceRef;
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

            foreach (System.Collections.Generic.KeyValuePair<object,object> entry in StorageObjects)
            {
                StorageInstanceRef storageInstanceRef = entry.Value as StorageInstanceRef;
                storageInstanceRef.ResolveRelationships();

            }


        }

        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {
            throw new NotImplementedException();
        }

        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCells, OOAdvantech.MetaDataRepository.Roles storageCellsRole)
        {
            throw new NotImplementedException();
        }

        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(object ObjectID)
        {
            throw new NotImplementedException();
        }
    }
}
