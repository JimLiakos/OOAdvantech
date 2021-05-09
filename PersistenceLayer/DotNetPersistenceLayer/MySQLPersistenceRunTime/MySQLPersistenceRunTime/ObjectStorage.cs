using System;
using System.Data;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using ObjectID = OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
using Commands = OOAdvantech.RDBMSPersistenceRunTime.Commands;
using System.Data.Common;
namespace OOAdvantech.MySQLPersistenceRunTime
{




    /// <MetaDataID>{30648d7b-81dc-4b18-9443-b75286253978}</MetaDataID>
    public class ObjectStorage : RDBMSPersistenceRunTime.ObjectStorage
    {
        public override DataLoader CreateDataLoader(DataNode dataNode, StorageDataSource.DataLoaderMetadata dataLoaderMetadata)
        {
            return new ObjectQueryLanguage.DataLoader(dataNode, dataLoaderMetadata);
        }
        public ObjectStorage(RDBMSPersistenceRunTime.Storage theStorageMetaData)
        {
            SetObjectStorage(theStorageMetaData);

        }

        protected override void TransferTableRecords(System.Data.DataTable dataTable)
        {
            System.Data.Common.DbCommand command = Connection.CreateCommand();
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();

            command.CommandText = "SET foreign_key_checks = 0";
            command.ExecuteNonQuery();

            try
            {
                command = Connection.CreateCommand();
                string commandText = null;
                string values = null;


                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = "@" + column.ColumnName;
                    command.Parameters.Add(parameter);
                    if (commandText == null)
                    {
                        commandText = @"INSERT INTO `" + StorageMetaData.StorageName + "`.`" + dataTable.TableName + "`(";
                        values = "VALUES (";
                    }
                    else
                    {
                        commandText += ",";
                        values += ",";
                    }

                    commandText += column.ColumnName;
                    values += "@" + column.ColumnName;

                }
                command.CommandText = commandText + ")" + values + ")";


                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    foreach (System.Data.DataColumn column in dataTable.Columns)
                        command.Parameters["@" + column.ColumnName].Value = row[column.ColumnName];

                    command.ExecuteNonQuery();

                }
            }
            finally
            {
                command = Connection.CreateCommand();
                command.CommandText = "SET foreign_key_checks = 1";
                command.ExecuteNonQuery();
                
            }

        }
        protected override void UpdateTableRecords(System.Data.DataTable dataTable, System.Collections.Generic.List<string> OIDColumns)
        {
            System.Data.Common.DbCommand command = Connection.CreateCommand();
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();


            command.CommandText = "SET foreign_key_checks = 0";
            command.ExecuteNonQuery();

            try
            {
                command = Connection.CreateCommand();
                string commandText = null;
                //UPDATE    T_LiquidProduct
                //SET              TypeID =@TypeID 
                //WHERE     (T_LiquidProduct.ObjectID = 'SDSD')
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    if (!OIDColumns.Contains(column.ColumnName))
                    {

                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = "@" + column.ColumnName;
                        command.Parameters.Add(parameter);
                        if (commandText == null)
                            commandText = @"UPDATE  `" + StorageMetaData.StorageName + "`.`" + dataTable.TableName + "` \r\nSET ";
                        else
                            commandText += ",";

                        commandText += column.ColumnName + " = @" + column.ColumnName;
                    }
                    else
                    {

                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = "@" + column.ColumnName;
                        command.Parameters.Add(parameter);

                    }
                }
                string filter = null;
                foreach (string OIDColumn in OIDColumns)
                {
                    if (filter == null)
                        filter = "\nWHERE ";
                    else
                        filter += " AND ";

                    filter += OIDColumn + " = @" + OIDColumn;
                }
                commandText += filter;
                command.CommandText = commandText;


                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    foreach (System.Data.DataColumn column in dataTable.Columns)
                        command.Parameters["@" + column.ColumnName].Value = row[column.ColumnName];
                    command.ExecuteNonQuery();
                }

            }
            finally
            {
                command = Connection.CreateCommand();
                command.CommandText = "SET foreign_key_checks = 1";
                command.ExecuteNonQuery();

            }
        }
        protected override void DeleteTableRecords(System.Data.DataTable dataTable)
        {
            System.Data.Common.DbCommand command = Connection.CreateCommand();
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();

            command.CommandText = "SET foreign_key_checks = 1";
            command.ExecuteNonQuery();


            try
            {
                command = Connection.CreateCommand();
                string filter = null;
                foreach (System.Data.DataColumn OIDColumn in dataTable.Columns)
                {
                    if (filter == null)
                        filter = "\nWHERE ";
                    else
                        filter += " AND ";

                    filter += OIDColumn.ColumnName + " = @" + OIDColumn.ColumnName;
                    DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = "@" + OIDColumn.ColumnName;
                    command.Parameters.Add(parameter);

                }
                command.CommandText = "DELETE FROM `" + StorageMetaData.StorageName + "`.`" + dataTable.TableName + "` " + filter;
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    foreach (System.Data.DataColumn column in dataTable.Columns)
                        command.Parameters["@" + column.ColumnName].Value = row[column.ColumnName];
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
            finally
            {
                command = Connection.CreateCommand();
                command.CommandText = "SET foreign_key_checks = 0";
                command.ExecuteNonQuery();

            }


        }

        

        //public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery DistributeObjectQuery(OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> dataTrees,
        //    OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> selectListItems,
        //    OOAdvantech.Collections.Generic.Dictionary<Guid, MetaDataRepository.ObjectQueryLanguage.StorageDataSource.DataLoaderMetadata> storageCells)
        //{
        //    //foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in dataTrees)
        //    //    InitializeMetaData(dataNode);

        //    string errorOutput = "";

        //    OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> objectTypeDataNodes = new OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode>();
        //    foreach (OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in dataTrees)
        //        GetObjectTypeDataNodes(dataNode, objectTypeDataNodes);
        //    dataTrees = objectTypeDataNodes;




        //    foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in dataTrees)
        //    {
        //        System.Type type = ModulePublisher.ClassRepository.GetType(dataNode.FullName, "");
        //        MetaDataRepository.Classifier classifier = null;
        //        if (type != null)
        //            classifier = MetaDataRepository.Classifier.GetClassifier(type);

        //        if (classifier == null)
        //            throw new Exception(errorOutput += "There isn't type '" + dataNode.FullName + "'");

        //        //MetaDataRepository.Namespace mNamespace = GetNamespace(StorageMetaData, dataNode.Name);
        //        //dataNode.AssignedMetaObject = mNamespace;
        //        dataNode.Validate(ref errorOutput);
        //        if (!string.IsNullOrEmpty(errorOutput))
        //            throw new System.Exception(errorOutput);
        //    }


        //    MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery distributedObjectQuery = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery(dataTrees, selectListItems);
        //    foreach (System.Collections.Generic.KeyValuePair<Guid, MetaDataRepository.ObjectQueryLanguage.StorageDataSource.DataLoaderMetadata> entry in storageCells)
        //    {
        //        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataSource dataSource = distributedObjectQuery.GetDataSource(entry.Key);
        //        dataSource.DataLoaders.Add(StorageMetaData.StorageIdentity, new ObjectQueryLanguage.DataLoader(dataSource.DataNode, entry.Value));
        //    }

        //    OOAdvantech.Collections.Generic.Dictionary<Guid, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader> dataLoaders = distributedObjectQuery.DataLoaders;
        //    return distributedObjectQuery;
        //}
        //        MetaDataRepository.Namespace GetNamespace(PersistenceLayer.Storage objectStorage, string _namespace)
        //        {
        //            //TODO θα πρέπει να βρεθεί ένας τρόπος να αντιμετωπίζεται ενιαία τα meta data. 
        //            //Τώρα υπάρχουν δύο περιπτώσεις, η περίπτωση που διαβάζουμε τα meta data από το storage όπου 
        //            //το πρόβλημα είναι ότι αργότερα πρέπει να κάνω access τα fields από τα objects πρέπει DotnetMetadaRepository classes 
        //            //και δεύτερον η περίπτωση που δεν υπάρχουν metadata στην storage και εκεί δεν ξέρω 
        //            //αν το assembly που έχει τα types που θέλει to query έχει φορτωθεί.       


        //#if !NETCompactFramework
        //            MetaDataRepository.MetaObjectID metaObjectID = new MetaDataRepository.MetaObjectID(_namespace);
        //            MetaDataRepository.Namespace namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //            System.Collections.ArrayList assemblies = new System.Collections.ArrayList();
        //            if (namespaceMetaData == null)
        //            {
        //                foreach (System.Reflection.Assembly dotNetAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
        //                {

        //                    if (dotNetAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false).Length > 0)
        //                    {
        //                        DotNetMetaDataRepository.Assembly assembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(dotNetAssembly) as DotNetMetaDataRepository.Assembly;
        //                        if (assembly == null)
        //                            assembly = new DotNetMetaDataRepository.Assembly(dotNetAssembly);
        //                        assemblies.Add(assembly);
        //                        long load = assembly.Residents.Count;
        //                        namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //                        if (namespaceMetaData != null)
        //                            return namespaceMetaData;
        //                    }
        //                }
        //            }
        //            if (namespaceMetaData == null)
        //            {
        //                foreach (DotNetMetaDataRepository.Assembly assembly in assemblies)
        //                {
        //                    foreach (MetaDataRepository.Dependency dependency in assembly.ClientDependencies)
        //                    {
        //                        DotNetMetaDataRepository.Assembly referAssembly = dependency.Supplier as DotNetMetaDataRepository.Assembly;
        //                        if (referAssembly.WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false).Length > 0)
        //                        {
        //                            long load = referAssembly.Residents.Count;
        //                            namespaceMetaData = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(metaObjectID) as MetaDataRepository.Namespace;
        //                            if (namespaceMetaData != null)
        //                                return namespaceMetaData;
        //                        }
        //                    }
        //                }
        //            }
        //            return namespaceMetaData;
        //#else
        //                return null;
        //#endif

        //        }



        //void InitializeMetaData(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        //{



        //    if (dataNode.AssignedMetaObject == null && dataNode.AssignedMetaObjectIdenty != null)
        //    {
        //        string query = "SELECT MetaObject FROM " + typeof(MetaDataRepository.MetaObject).FullName + " MetaObject WHERE MetaObject.MetaObjectIDStream = \"" + dataNode.AssignedMetaObjectIdenty + "\"";
        //        Collections.StructureSet structureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(StorageMetaData).Execute(query);
        //        foreach (Collections.StructureSet Rowset in structureSet)
        //        {
        //            dataNode.AssignedMetaObject = (MetaDataRepository.MetaObject)Rowset.Members["MetaObject"].Value;
        //            break;
        //        }
        //    }
        //    foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in dataNode.SubDataNodes)
        //        InitializeMetaData(subDataNode);
        //}



        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        
        public override Collections.Generic.Set<MetaDataRepository.StorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCells, MetaDataRepository.Roles storageCellsRole)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                association = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(association) as OOAdvantech.MetaDataRepository.Association;
                Collections.Generic.Set<MetaDataRepository.StorageCell> StorageCells = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>();
                foreach (MetaDataRepository.StorageCell storageCell in relatedStorageCells)
                {
                    foreach (MetaDataRepository.StorageCellsLink storageCellsLink in association.StorageCellsLinks)
                    {
                        if (storageCellsRole == MetaDataRepository.Roles.RoleA && storageCellsLink.RoleAStorageCell == storageCell)
                            StorageCells.AddRange(storageCellsLink.AssotiationClassStorageCells);
                        else
                        {
                            if (storageCellsLink.RoleBStorageCell == storageCell)
                                StorageCells.AddRange(storageCellsLink.AssotiationClassStorageCells);
                        }
                    }
                }
                return StorageCells;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }




        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            if (classifier is MetaDataRepository.Class)
            {
                MetaDataRepository.Class storageClass = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Class)) as MetaDataRepository.Class;
                return storageClass.StorageCellsOfThisType;
            }
            else if (classifier is MetaDataRepository.Interface)
            {
                MetaDataRepository.Interface storageInterface = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Interface)) as MetaDataRepository.Interface;
                return storageInterface.StorageCellsOfThisType;
            }
            throw new Exception("The method or operation is not implemented.");
        }

        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(object objectID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override object GetObject(string persistentObjectUri)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetPersistentObjectUri(object obj)
        {
            if (obj is StorageInstanceRef)
            {
                StorageInstanceRef storageInstanceRef = obj as StorageInstanceRef;
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            else if (PersistencyService.ClassOfObjectIsPersistent(obj))
            {
                PersistenceLayer.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(obj);
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            return null;
        }
        
        string OleDBConnectionString = null;
        
        public System.Data.Common.DbConnection Connection
        {
            get
            {
                return ((StorageMetaData as RDBMSPersistenceRunTime.Storage).MetadataStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).Connection;
            }
        }

        
        public RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get
            {
                return ((StorageMetaData as RDBMSPersistenceRunTime.Storage).MetadataStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).TypeDictionary;
            }
        }


        
        public OOAdvantech.Collections.Map DatabaseConectios = new OOAdvantech.Collections.Map();


        /// <exclude>Excluded</exclude>
        
        private OOAdvantech.PersistenceLayerRunTime.MemoryInstanceCollection _OperativeObjectCollections;
        
        public override OOAdvantech.PersistenceLayerRunTime.MemoryInstanceCollection OperativeObjectCollections
        {
            get
            {

                if (_OperativeObjectCollections == null)
                    _OperativeObjectCollections = new RDBMSPersistenceRunTime.MemoryInstanceCollection(this);
                return _OperativeObjectCollections;
            }
        }


        /// <exclude>Excluded</exclude>
        
        protected OOAdvantech.PersistenceLayer.Storage _StorageMetaData;
        
        public override OOAdvantech.PersistenceLayer.Storage StorageMetaData
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _StorageMetaData;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }

        
        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
        {
            MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
            mStructureSet.Open(OQLStatement, parameters);
            //mStructureSet.GetData();
            return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);

        }

        
        public override void AbortChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }



        
        public override void BeginChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }



        
        public RDBMSMetaDataRepository.StorageCellReference GetOutStorageObjColl(PersistenceLayerRunTime.StorageInstanceRef OutStorageInstanceRef)
        {

            if (OutStorageInstanceRef.GetType() == typeof(StorageInstanceRef))
            {
                RDBMSMetaDataRepository.StorageCellReference storageCell = ((RDBMSPersistenceRunTime.Storage)_StorageMetaData).GetEquivalentMetaObject("ref_" + (OutStorageInstanceRef as StorageInstanceRef).StorageInstanceSetIdentity, typeof(RDBMSMetaDataRepository.StorageCellReference)) as RDBMSMetaDataRepository.StorageCellReference;
                if (storageCell != null)
                    return storageCell;
                RDBMSMetaDataRepository.Class theClass = ((RDBMSPersistenceRunTime.Storage)_StorageMetaData).GetEquivalentMetaObject(OutStorageInstanceRef.Class) as RDBMSMetaDataRepository.Class;
                return theClass.GetStorageCellReference((OutStorageInstanceRef as StorageInstanceRef).StorageInstanceSet);
            }
            return null; //Error prone
        }
        
        public override void CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {

            PersistenceLayerRunTime.Commands.LinkCommand mUnLinkObjectsCommand = null;
            if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage != this)
                throw new System.ArgumentException("There isn't object from this storage");

            if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage == this)
            {
                mUnLinkObjectsCommand = new Commands.UnLinkObjectsCommand(roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
            }
            else
            {
                throw new NotImplementedException();
                //mUnLinkObjectsCommand=new Commands.InterSorageUnLinkObjectsCommand(roleA,roleB,relationObject,linkInitiatorAssociationEnd,this);
            }
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(mUnLinkObjectsCommand);
        }

        
        public override void CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.LinkCommand mLinkObjectsCommand = null;
            if (roleA.ObjectStorage == roleB.ObjectStorage)
            {
                mLinkObjectsCommand = new Commands.LinkObjectsCommand(roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
            }
            else
            {
                throw new NotImplementedException();
                //mLinkObjectsCommand=new Commands.InterSorageLinkObjectsCommand(roleA,roleB,relationObject,linkInitiatorAssociationEnd,this);
            }

            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(mLinkObjectsCommand);

        }
        public override void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection)
        {
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            RDBMSPersistenceRunTime.Commands.BuildContainedObjectIndicies buildContainedObjectIndicies = new RDBMSPersistenceRunTime.Commands.BuildContainedObjectIndicies(collection);
            transactionContext.EnlistCommand(buildContainedObjectIndicies);

        }


        
        public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            //TODO:Μεγάλο πρόβλημα αν το transaction έχει πολλές μεταβολές δηλαδή πολλά commands

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;

            Commands.UpdateReferentialIntegrity updateReferentialIntegrity = new Commands.UpdateReferentialIntegrity(storageInstanceRef as StorageInstanceRef);
            //updateReferentialIntegrity.UpdatedStorageInstanceRef=storageInstanceRef;
            transactionContext.EnlistCommand(updateReferentialIntegrity);
        }




        
        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, object objectID)
        {
            return new StorageInstanceRef(memoryInstance, this, objectID);

        }
        
        public override void CommitChanges(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }

        
        public override void MakeChangesDurable(PersistenceLayerRunTime.TransactionContext theTransaction)
        {

        }

        
        public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceRef sourceStorageInstance, DotNetMetaDataRepository.AssociationEnd associationEnd)
        {
            if (Remoting.RemotingServices.IsOutOfProcess(associationEnd))
                associationEnd = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(associationEnd.Identity) as DotNetMetaDataRepository.AssociationEnd;

            Commands.UnlinkAllObjectCommand mUnlinkAllObjectCommand = new Commands.UnlinkAllObjectCommand(sourceStorageInstance);
            //mUnlinkAllObjectCommand.DeletedStorageInstance=sourceStorageInstance;
            mUnlinkAllObjectCommand.theAssociationEnd = associationEnd;

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(mUnlinkAllObjectCommand);



        }

        
        public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceRef DeletedOutStorageInstanceRef, MetaDataRepository.AssociationEnd AssociationEnd, MetaDataRepository.StorageCell linkedObjectsStorageCell)
        {
            RDBMSMetaDataRepository.StorageCell StorageCell = GetOutStorageObjColl(DeletedOutStorageInstanceRef);
            if (Remoting.RemotingServices.IsOutOfProcess(AssociationEnd))
                AssociationEnd = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;

            RDBMSMetaDataRepository.AssociationEnd inStorageAssociationEnd = (_StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            RDBMSMetaDataRepository.Association inStorageAssociation = inStorageAssociationEnd.Association as RDBMSMetaDataRepository.Association;
            RDBMSMetaDataRepository.StorageCellsLink StorageCellsLink = null;
            if (AssociationEnd.IsRoleA)
                StorageCellsLink = inStorageAssociation.GetStorageCellsLink(StorageCell, linkedObjectsStorageCell as RDBMSMetaDataRepository.StorageCell, "");
            else
                StorageCellsLink = inStorageAssociation.GetStorageCellsLink(linkedObjectsStorageCell as RDBMSMetaDataRepository.StorageCell, StorageCell, "");
            //PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand Temp=new Commands.OutStorageUnlinkAllObjectCmd(StorageCellsLink,(StorageInstanceRef)DeletedOutStorageInstanceRef,inStorageAssociationEnd,this);
            PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand Temp = new Commands.UnlinkAllObjectCommand((StorageInstanceRef)DeletedOutStorageInstanceRef);

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;

            transactionContext.EnlistCommand(Temp);

        }

        /// <summary>UpdateStorageInstanceCommand </summary>
        
        public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            
            Commands.UpdateStorageInstanceCommand updateStorageInstanceCommand = new Commands.UpdateStorageInstanceCommand(storageInstanceRef);

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(updateStorageInstanceCommand);
        }

        
        public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
        {
            Commands.NewStorageInstanceCommand newStorageInstanceCommand = new Commands.NewStorageInstanceCommand(StorageInstance as StorageInstanceRef);
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(newStorageInstanceCommand);
        }

        
        public System.Collections.Hashtable GetObjects(System.Data.DataTable StorageInstances, System.Type objectsType)
        {

            System.Collections.Hashtable MemoryInstances = new OOAdvantech.Collections.Hashtable(StorageInstances.Rows.Count);

            foreach (System.Data.DataRow StorageInstance in StorageInstances.Rows)
            {
                object FieldValue = StorageInstance["ObjectID"];
                if (FieldValue is System.DBNull)
                    continue;
                if (FieldValue == null)
                    continue;

                System.Guid IntObjID = (System.Guid)FieldValue;
                int ObjCellID = (int)StorageInstance["StorageCellID"];
                ObjectID mObjectID = new ObjectID(IntObjID, ObjCellID);
                StorageInstanceRef mStorageInstanceRef = OperativeObjectCollections[objectsType][mObjectID] as StorageInstanceRef;
                if (mStorageInstanceRef != null)
                {
                    MemoryInstances.Add(mObjectID.IntObjID, mStorageInstanceRef.MemoryInstance);
                    continue;
                }
                mStorageInstanceRef = (StorageInstanceRef)CreateStorageInstanceRef(System.Activator.CreateInstance(objectsType), mObjectID);
                //mStorageInstanceRef.ObjectID=mObjectID;
                RDBMSMetaDataRepository.Class Class = mStorageInstanceRef.Class.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Class)) as RDBMSMetaDataRepository.Class;
                mStorageInstanceRef.StorageInstanceSet = Class.GetStorageCell(mObjectID.ObjCellID);
                mStorageInstanceRef.DbDataRecord = StorageInstance;
                mStorageInstanceRef.LoadObjectState("");
                //mStorageInstanceRef.SnapshotStorageInstance();
                mStorageInstanceRef.ObjectActived();
                MemoryInstances.Add(mObjectID.IntObjID, mStorageInstanceRef.MemoryInstance);
            }
            return MemoryInstances;
        }

        
        public override Collections.StructureSet Execute(string OQLStatement)
        {
            using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
                mStructureSet.Open(OQLStatement, null);
                //mStructureSet.GetData();
                stateTransition.Consistent = true;
                return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);
            }

        }
        
        public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, PersistenceLayer.DeleteOptions deleteOption)
        {

            Commands.DeleteStorageInstanceCommand Command = new Commands.DeleteStorageInstanceCommand(storageInstance, deleteOption);
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            if (!transactionContext.ContainCommand(Command.Identity))
                transactionContext.EnlistCommand(Command);

        }
    
        
        public static void StoreMetatData(System.IO.Stream metaDataStream, byte[] pointer, System.Data.SqlClient.SqlConnection Conn)
        {
            int bufferLen = 4096;  // The size of the "chunks" of the image.

            System.Data.SqlClient.SqlCommand appendToMetatData = new System.Data.SqlClient.SqlCommand("UPDATETEXT MetaDataTable.MetaData @Pointer @Offset NULL @Bytes", Conn);

            System.Data.SqlClient.SqlParameter ptrParm = appendToMetatData.Parameters.Add("@Pointer", System.Data.SqlDbType.Binary, 16);
            ptrParm.Value = pointer;
            System.Data.SqlClient.SqlParameter photoParm = appendToMetatData.Parameters.Add("@Bytes", System.Data.SqlDbType.Image, bufferLen);
            System.Data.SqlClient.SqlParameter offsetParm = appendToMetatData.Parameters.Add("@Offset", System.Data.SqlDbType.Int);
            offsetParm.Value = 0;

            //''''''''''''''''''''''''''''''''''
            // Read the image in and write it to the database 128 (bufferLen) bytes at a time.
            // Tune bufferLen for best performance. Larger values write faster, but
            // use more system resources.

            //FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            //System.IO.BinaryReader br = new System.IO.BinaryReader(metaDataStream);


            byte[] buffer = new byte[bufferLen];
            //br.ReadBytes(bufferLen);
            metaDataStream.Position = 0;
            int readedLength = metaDataStream.Read(buffer, 0, bufferLen);
            int offset_ctr = 0;
            while (readedLength > 0)
            {
                if (readedLength == bufferLen)
                    photoParm.Value = buffer;
                else
                {
                    byte[] tmpBuffer = new byte[readedLength];
                    for (int i = 0; i != readedLength; i++)
                        tmpBuffer[i] = buffer[i];
                    photoParm.Value = tmpBuffer;
                }
                appendToMetatData.ExecuteNonQuery();
                offset_ctr += bufferLen;
                offsetParm.Value = offset_ctr;
                readedLength = metaDataStream.Read(buffer, 0, bufferLen);
            }
            //	br.Close();
        }

        
        void SetObjectStorage(RDBMSPersistenceRunTime.Storage theStorageMetaData)
        {

            if (_StorageMetaData != null)
                throw new System.Exception("StorageMetaData already set");
            _StorageMetaData = theStorageMetaData;
             
            //_StorageMetaData.ObjectStorageChanged+=new PersistenceLayer.ObjectStorageChangedEventHandler(OnObjectStorageChanged);

        }
        /*
        
        public override PersistenceLayer.ObjectStorage StorageMetaData
        {
            get
            {
                ObjectStorage mObjectStorage=null;
                mObjectStorage.ComponentRegistered+=new PersistenceLayer.ComponentRegisteredEventHandler(OnComponentRegistered);
                return null;
            }
        }*/

        
        internal int ActiveObjectsCount
        {
            get
            {
                int activeObjectsCount = 0;
                foreach (RDBMSPersistenceRunTime.ClassMemoryInstanceCollection classActiveObjects in OperativeObjectCollections)
                    activeObjectsCount += classActiveObjects.StorageInstanceRefs.Count;
                return activeObjectsCount;
            }

        }
        // Returns a column name that is of length totalSize.
        
        private string GetColumnName(long totalSize, long index)
        {
            string si = index.ToString();
            string pad;
            if (si.Length >= (totalSize - 1)) return "c" + si;
            pad = new string('c', (int)totalSize - si.Length);
            return pad + si;
        }
        // Returns a column value of specified columnType and dataSize.
        // Note dataSize only applies to string and byte[] data types.
        // For all numeric data types, I return their appropriate maximum values.
        
        private object GetColumnValue(Type columnType, long dataSize)
        {
            if (columnType == typeof(Boolean)) return false;
            if (columnType == typeof(Byte)) return Byte.MaxValue;
            if (columnType == typeof(Char)) return 'x';
            if (columnType == typeof(DateTime)) return DateTime.Now;
            if (columnType == typeof(Decimal)) return Decimal.MaxValue;
            if (columnType == typeof(Double)) return Double.MaxValue;
            if (columnType == typeof(Int16)) return Int16.MaxValue;
            if (columnType == typeof(Int32)) return Int32.MaxValue;
            if (columnType == typeof(Int64)) return Int64.MaxValue;
            if (columnType == typeof(Single)) return Single.MaxValue;
            if (columnType == typeof(String)) return new string('x', (int)dataSize);
            if (columnType == typeof(TimeSpan)) return new TimeSpan(1, 1, 1, 1, 1);
            if (columnType == typeof(Guid)) return new Guid("{3381E62F-20C6-462c-87DF-CC11F37C0EC6}");
            if (columnType == typeof(Byte[]))
            {
                byte[] b = new byte[dataSize];
                for (long i = 0; i < b.Length; i++) b[i] = (byte)(i % (((int)byte.MaxValue) + 1));
                return b;
            }
            return null;
        }
        
        private DataSet GenerateDataSet(long columnCount, long rowCount, long cnameSize, long dataSize, bool attributed, Type columnType)
        {
            DataSet ds = new DataSet("d");
            DataTable t = new DataTable("t");
            long r, c;
            object[] values = null;

            // Create row values array.
            values = new object[columnCount];
            for (c = 1; c <= columnCount; c++)
            {
                t.Columns.Add(GetColumnName(cnameSize, c), columnType);
                if (attributed) t.Columns[(int)(c - 1)].ColumnMapping = MappingType.Attribute;
                values[c - 1] = GetColumnValue(columnType, dataSize);
            }

            for (r = 1; r <= rowCount; r++)
            {
                t.Rows.Add(values);
            }

            t.AcceptChanges();
            ds.Tables.Add(t);
            ds.AcceptChanges();

            return ds;

        }
        
        static byte[] Buffer = new byte[4096];

        
        public byte[] GetRelatedRows(Guid[] objectIDs)
        {
            System.Data.SqlClient.SqlConnection mOleDbConnection = new System.Data.SqlClient.SqlConnection(Connection.ConnectionString);

            //   string SQLQuery = "SELECT  T_OrderDetail.ObjectID,T_OrderDetail.ObjectID as obj2,T_OrderDetail.ObjectID as obj3,T_OrderDetail.ObjectID as obj4 FROM T_OrderDetail";
            string SQLQuery = "SELECT  T_OrderDetail.ObjectID FROM T_OrderDetail";
            SQLQuery = "SELECT     ObjectID, ReferenceCount, TypeID, Name, OrderDetails_ObjectIDB FROM         T_OrderDetail";


            //TOP 50 PERCENT

            System.Data.DataSet MainDataReader = new System.Data.DataSet();
            mOleDbConnection.Open();
            System.Data.SqlClient.SqlCommand DataCommand = mOleDbConnection.CreateCommand();//((StorageSession)SourceStorageSession).OleDbConnection.CreateCommand();
            DataCommand.CommandText = SQLQuery;
            System.Data.SqlClient.SqlDataAdapter myAdapter = new System.Data.SqlClient.SqlDataAdapter();
            myAdapter.SelectCommand = DataCommand;
            myAdapter.Fill(MainDataReader);
            mOleDbConnection.Close();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            int offset = 0;
            foreach (DataRow dataRow in MainDataReader.Tables[0].Rows)
            {
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize((Guid)dataRow[0], Buffer, offset, ref offset, true);
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(((int)dataRow[1]), Buffer, offset, ref offset, true);
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(((int)dataRow[2]), Buffer, offset, ref offset, true);
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(((string)dataRow[3]), Buffer, offset, ref offset);
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(((Guid)dataRow[4]), Buffer, offset, ref offset, true);

                memoryStream.Write(Buffer, 0, offset);
                offset = 0;
            }
            memoryStream.Position = 0;
            byte[] buffer = memoryStream.ToArray();
            memoryStream.Close();
            return buffer;

            System.Data.DataTable table = new System.Data.DataTable();


            System.TimeSpan NewTimeSpan;
            System.TimeSpan ComiteTimeSpan;
            System.TimeSpan ComiteTimeSpanB;
            System.DateTime before;
            System.DateTime After;
            before = System.DateTime.Now;

            table.Columns.Add("ObjectID", typeof(System.Guid));
            foreach (System.Guid objectID in objectIDs)
            {
                table.Rows.Add(table.NewRow()[0] = objectID);
            }
            MainDataReader.Tables.Add(table);

            System.Data.DataRelation relation = new System.Data.DataRelation("test", table.Columns[0], MainDataReader.Tables[0].Columns["OrderDetails_ObjectIDB"]);
            System.Data.DataTable retTable = new DataTable();
            foreach (DataColumn col in MainDataReader.Tables[0].Columns)
            {
                retTable.Columns.Add(col.ColumnName, col.DataType);

            }
            //System.Data.DataTable retTable= MainDataReader.Tables[0].Clone();
            MainDataReader.Relations.Add(relation);
            foreach (System.Data.DataRow row in MainDataReader.Tables[0].Rows)
            {
                DataRow newRow = retTable.NewRow();
                int i = 0;
                foreach (object obj in row.ItemArray)
                {
                    if (obj is DBNull)
                        continue;
                    else
                        newRow[i] = obj;
                    i++;
                }
                retTable.Rows.Add(newRow);


                //if (row.GetParentRow(relation) != null)
                //    retTable.Rows.Add(row.ItemArray);
                 
            }
            After = System.DateTime.Now;
            ComiteTimeSpan = After - before;
            System.Diagnostics.Debug.WriteLine("1 " + ComiteTimeSpan.ToString());
            System.Data.DataSet dataSet = new DataSet();
            dataSet.Tables.Add(retTable);

            DataSet ds = null;
            PersistenceLayerRunTime.DataSetSurrogate dss = null;
            ds = GenerateDataSet(5, 66000, 4, 50, false, typeof(Guid));
            bool k = true;

            if (k)
                dataSet = ds;

            return null;



        }

        
        public void GetData(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, RDBMSMetaDataRepository.StorageCell storageCell)
        {
            int ert = 0;

        }
    }
  

}

