using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using OOAdvantech.Collections;
using OOAdvantech.DotNetMetaDataRepository;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.PersistenceLayerRunTime;
using OOAdvantech.RDBMSMetaDataRepository;
using OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{


    /// <MetaDataID>{5afec19d-78b9-4d20-89f1-fb5b1453b9bc}</MetaDataID>
    public class ObjectStorage : PersistenceLayerRunTime.ObjectStorage
    {



        /// <MetaDataID>{5afac087-3f51-4547-bc1f-7cd25a6a50e6}</MetaDataID>
        void SetObjectStorage(Storage theStorageMetaData)
        {

            if (_StorageMetaData != null)
                throw new System.Exception("StorageMetaData already set");
            _StorageMetaData = theStorageMetaData;
        }

        /// <exclude>Excluded</exclude>
        protected Storage _StorageMetaData;



        //public readonly CloudStorageAccount Account;
        public readonly Azure.Data.Tables.TableServiceClient TablesAccount;
        /// <MetaDataID>{3f203356-42c6-44fa-b0bc-cb8422f90cc2}</MetaDataID>
        internal ObjectStorage(Storage theStorageMetaData, Azure.Data.Tables.TableServiceClient tablesAccount, string userName, string pasword)
        {
            //Account = account;
            TablesAccount = tablesAccount;
            SetObjectStorage(theStorageMetaData);
            UserName = userName;
            Pasword = pasword;
        }


        string UserName;
        string Pasword;

        /// <MetaDataID>{fa8859af-abc4-46ef-864b-be94f908bfa6}</MetaDataID>
        public override PersistenceLayer.Storage StorageMetaData
        {
            get
            {
                return _StorageMetaData;
            }
        }

        TypeDictionary _TypeDictionary;
        public TypeDictionary TypeDictionary
        {
            get
            {
                if (_TypeDictionary == null)
                    _TypeDictionary = new TypeDictionary();
                return _TypeDictionary;
            }
        }

        /// <MetaDataID>{deae242f-8ca7-4891-92e3-b5e3acaf536f}</MetaDataID>
        public override void AbortChanges(TransactionContext theTransaction)
        {
            lock (TableBatchOperations_a)
            {
                TableBatchOperations_a.Remove(theTransaction.Transaction.LocalTransactionUri.ToLower());
            }
        }

        /// <MetaDataID>{2773b19e-7768-4762-9e2e-8166d2910565}</MetaDataID>
        public override void BeginChanges(TransactionContext theTransaction)
        {
            lock (TableBatchOperations_a)
            {

                Dictionary<Azure.Data.Tables.TableClient, List<List<Azure.Data.Tables.TableTransactionAction>>> transactionTableBatchOperations = null;
                if (!TableBatchOperations_a.TryGetValue(theTransaction.Transaction.LocalTransactionUri.ToLower(), out transactionTableBatchOperations))
                {
                    transactionTableBatchOperations = new Dictionary<Azure.Data.Tables.TableClient, List<List<Azure.Data.Tables.TableTransactionAction>>>();
                    TableBatchOperations_a[theTransaction.Transaction.LocalTransactionUri.ToLower()] = transactionTableBatchOperations;
                }
            }
        }

        /// <MetaDataID>{957ac9f6-3739-4a03-a926-31aa0a843c5e}</MetaDataID>
        public override void CommitChanges(TransactionContext theTransaction)
        {
            lock (TableBatchOperations_a)
            {
                TableBatchOperations_a.Remove(theTransaction.Transaction.LocalTransactionUri.ToLower());
            }
        }

        /// <MetaDataID>{a86dcd77-b2df-4ea9-bd1c-f09a65ab3628}</MetaDataID>
        public override void CreateBuildContainedObjectIndiciesCommand(IndexedCollection collection)
        {

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            WindowsAzureTablesPersistenceRunTime.Commands.BuildContainedObjectIndicies buildContainedObjectIndicies = new WindowsAzureTablesPersistenceRunTime.Commands.BuildContainedObjectIndicies(collection);
            transactionContext.EnlistCommand(buildContainedObjectIndicies);

        }



        /// <MetaDataID>{61d86519-6e1a-4fcc-905a-146d210f368c}</MetaDataID>
        public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, DeleteOptions deleteOption)
        {
            Commands.DeleteStorageInstanceCommand Command = new Commands.DeleteStorageInstanceCommand(storageInstance, deleteOption);
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            if (!transactionContext.ContainCommand(Command.Identity))
                transactionContext.EnlistCommand(Command);
        }

        /// <MetaDataID>{3928fd3b-b615-4ca6-bc54-95615ba8c8a2}</MetaDataID>
        public override void CreateLinkCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.LinkObjectsCommand linkObjectsCommand = null;

            string cmdIdentity = Commands.LinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command);

            linkObjectsCommand = command as PersistenceLayerRunTime.Commands.LinkObjectsCommand;
            if (linkObjectsCommand == null)
            {

                linkObjectsCommand = new Commands.LinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(linkObjectsCommand);
            }
            else
            {

                if (linkInitiatorAssociationEnd.RealAssociationEnd.IsRoleA)
                    linkObjectsCommand.RoleAIndex = index;
                else
                    linkObjectsCommand.RoleBIndex = index;
            }
        }

        /// <MetaDataID>{9a19c39a-d673-482a-83de-2d9c8bc8bc84}</MetaDataID>
        public override void CreateMoveStorageInstanceCommand(StorageInstanceAgent storageInstance)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{55c6bc3f-6167-4651-8f17-b443c347b50e}</MetaDataID>
        public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance)
        {
            Commands.NewStorageInstanceCommand newStorageInstanceCommand = new Commands.NewStorageInstanceCommand(storageInstance as StorageInstanceRef);
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(newStorageInstanceCommand);
        }

        /// <MetaDataID>{2ec71d8c-cf3c-4829-bab9-fc8dc98479f9}</MetaDataID>
        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID)
        {
            RDBMSMetaDataRepository.StorageCell storageCell = ((StorageMetaData as Storage).GetEquivalentMetaObject(MetaDataRepository.Classifier.GetClassifier(memoryInstance.GetType())) as RDBMSMetaDataRepository.Class).ActiveStorageCell;
            return new StorageInstanceRef(memoryInstance, storageCell, this, objectID);

        }

        static DateTime AzureTableDateTimeMinVal = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal void TransferTableRecords(IDataTable dataTable, Table table)
        {


            RDBMSMetaDataRepository.StorageCell storageCell = table.TableCreator as RDBMSMetaDataRepository.StorageCell;
            string partitionKey = null;
            RDBMSMetaDataRepository.StorageCellsLink storageCellsLink = table.TableCreator as RDBMSMetaDataRepository.StorageCellsLink;
            if (storageCell != null)
                partitionKey = storageCell.SerialNumber.ToString();
            else
                partitionKey = storageCellsLink.RoleAStorageCell.SerialNumber.ToString() + "L" + storageCellsLink.RoleBStorageCell.SerialNumber.ToString();

            string cloudTableName = table.DataBaseTableName;


            //CloudTableClient tableClient = Account.CreateCloudTableClient();
            // CloudTable azureTable = tableClient.GetTableReference(cloudTableName);
            Azure.Data.Tables.TableClient azureTable_a = TablesAccount.GetTableClient(cloudTableName);
            //if (!azureTable.Exists())
            //{
            //    azureTable = tableClient.GetTableReference(table.Name);
            //    azureTable.CreateIfNotExists();
            //    table.DataBaseTableName = azureTable.Name;

            //}
            if (!TableExist(cloudTableName))
            {
                azureTable_a.CreateIfNotExists();
                table.DataBaseTableName = azureTable_a.Name;
            }


            //TableBatchOperation tableBatchOperation = null;

            foreach (IDataRow row in dataTable.Rows)
            {
                //if(tableBatchOperation==null)
                //    tableBatchOperation= new TableBatchOperation();

                Guid OID = Guid.NewGuid();
                if (storageCell != null)
                    OID = (Guid)row[table.ObjectIDColumns[0].Name];
                ElasticTableEntity entity = new ElasticTableEntity(new Azure.Data.Tables.TableEntity());
                entity.RowKey = OID.ToString().ToLower();
                entity.PartitionKey = partitionKey;

                foreach (System.Data.DataColumn column in dataTable.Columns)
                {

                    if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                    {
                        StorageInstanceRef storageInstanceRef = row[column.ColumnName] as StorageInstanceRef;
                        storageInstanceRef.TableEntity = entity;
                        continue;
                    }
                    if (table.ObjectIDColumns.Count > 0 && column.ColumnName == table.ObjectIDColumns[0].Name)
                        continue;

                    object value = row[column.ColumnName];

                    if (value is DateTime && ((DateTime)value == DateTime.MinValue))
                        value = DBNull.Value;

                    if (value is DateTime && ((DateTime)value < AzureTableDateTimeMinVal))
                        throw new ArgumentOutOfRangeException(column.ColumnName, "You can't store on azure table, date time less than " + AzureTableDateTimeMinVal.ToString());

                    if (value is DateTime)
                        value = ((DateTime)value).ToUniversalTime();



                    if (value is DBNull)
                        entity.SetNull(column.ColumnName, row.Table.Columns[column.ColumnName].DataType);
                    else
                        entity[column.ColumnName] = value;
                }


                //GetTableBatchOperation(azureTable).Insert(entity);
                var tableBatchOperation = GetTableBatchOperation(azureTable_a);
                tableBatchOperation.Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.Add, entity.Values));

            }

            //if(tableBatchOperation!=null)
            //    azureTable.ExecuteBatch(tableBatchOperation);
        }





        /// <MetaDataID>{716651a0-f252-4f89-ad4b-8ff73c22bda8}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(StorageInstanceAgent sourceStorageInstance, DotNetMetaDataRepository.AssociationEnd associationEnd)
        {
            if (Remoting.RemotingServices.IsOutOfProcess(associationEnd))
                associationEnd = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(associationEnd.Identity) as DotNetMetaDataRepository.AssociationEnd;

            Commands.UnlinkAllObjectCommand mUnlinkAllObjectCommand = new Commands.UnlinkAllObjectCommand(sourceStorageInstance, this);
            //mUnlinkAllObjectCommand.DeletedStorageInstance=sourceStorageInstance;
            mUnlinkAllObjectCommand.theAssociationEnd = associationEnd;

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(mUnlinkAllObjectCommand);
        }


        /// <MetaDataID>{ed28148a-24c1-4699-89dd-4616ab3959f0}</MetaDataID>
        public override void CreateUnLinkCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.LinkCommand mUnLinkObjectsCommand = null;
            if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage != this)
                throw new System.ArgumentException("There isn't object from this storage");

            string cmdIdentity = PersistenceLayerRunTime.Commands.UnLinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command))
            {
                if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage == this)
                {

                    mUnLinkObjectsCommand = new Commands.UnLinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                }
                else
                {
                    throw new NotImplementedException();
                    //mUnLinkObjectsCommand=new Commands.InterSorageUnLinkObjectsCommand(roleA,roleB,relationObject,linkInitiatorAssociationEnd,this);
                }
                PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(mUnLinkObjectsCommand);
            }

        }

        /// <MetaDataID>{515a7ca5-2739-45f5-af0b-2d5b2fc5373a}</MetaDataID>
        public override void CreateUpdateLinkIndexCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{1a749457-a83b-4d5c-9b6d-7a47479ca83f}</MetaDataID>
        public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;

            Commands.UpdateReferentialIntegrity updateReferentialIntegrity = new Commands.UpdateReferentialIntegrity(storageInstanceRef as StorageInstanceRef);
            //updateReferentialIntegrity.UpdatedStorageInstanceRef=storageInstanceRef;
            transactionContext.EnlistCommand(updateReferentialIntegrity);
        }

        /// <MetaDataID>{63d5cdea-3ecc-4f51-ad74-a6529219099c}</MetaDataID>
        public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            Commands.UpdateStorageInstanceCommand updateStorageInstanceCommand = new Commands.UpdateStorageInstanceCommand(storageInstanceRef);

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(updateStorageInstanceCommand);
        }

        /// <MetaDataID>{049266c6-c5ca-41ea-8a6e-07dd5a8d790a}</MetaDataID>
        public override Collections.StructureSet Execute(string OQLStatement)
        {
            throw new NotImplementedException();
        }

        public override Collections.StructureSet Execute(string OQLStatement, Collections.Generic.Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        internal void UpdateTableRecords(IDataTable dataTable, System.Collections.Generic.List<string> oIDColumns)
        {

            if (dataTable.Rows.Count > 0)
            {
                //CloudTableClient tableClient = Account.CreateCloudTableClient();
                string cloudTableName = dataTable.TableName;

                Azure.Data.Tables.TableClient azureTable_a = TablesAccount.GetTableClient(cloudTableName);


                //CloudTable azureTable = tableClient.GetTableReference(cloudTableName);
                //if (!azureTable.Exists())
                //    azureTable.CreateIfNotExists();

                if (!TableExist(cloudTableName))
                    azureTable_a.CreateIfNotExists();

                string partitionKey = null;

                foreach (IDataRow row in dataTable.Rows)
                {
                    StorageInstanceRef storageInstanceRef = row["StorageInstanceRef"] as StorageInstanceRef;

                    RDBMSMetaDataRepository.StorageCell storageCell = storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell;
                    RDBMSMetaDataRepository.Table table = storageCell.MainTable;
                    if (table != null && table.DataBaseTableName != null && table.DataBaseTableName.ToLower() == "TjimliakosgmailcomOptionChange".ToLower())
                    {

                    }
                    partitionKey = storageCell.SerialNumber.ToString();

                    Guid OID = Guid.NewGuid();
                    if (storageCell != null)
                        OID = (Guid)row[table.ObjectIDColumns[0].Name];
                    ElasticTableEntity entity = storageInstanceRef.TableEntity;
                    entity.RowKey = OID.ToString().ToLower();
                    entity.PartitionKey = partitionKey;

                    foreach (System.Data.DataColumn column in dataTable.Columns)
                    {
                        if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                            continue;

                        if (column.ColumnName == "StorageInstanceRef")
                            continue;

                        if (table.ObjectIDColumns.Count > 0 && column.ColumnName == table.ObjectIDColumns[0].Name)
                            continue;

                        object value = row[column.ColumnName];

                        if (value is DateTime && ((DateTime)value == DateTime.MinValue))
                            value = DBNull.Value;

                        if (value is DateTime && ((DateTime)value < AzureTableDateTimeMinVal))
                            throw new ArgumentOutOfRangeException(column.ColumnName, "You can't store on azure table, date time less than " + AzureTableDateTimeMinVal.ToString());

                        if (value is DateTime)
                            value = ((DateTime)value).ToUniversalTime();

                        if (value is DBNull)
                            entity.SetNull(column.ColumnName, row.Table.Columns[column.ColumnName].DataType);
                        else
                            entity[column.ColumnName] = value;

                    }
                    //GetTableBatchOperation(azureTable).InsertOrReplace(entity);
                    var tableBatchOperation = GetTableBatchOperation(azureTable_a);
                    tableBatchOperation.Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.UpsertReplace, entity.Values));

                }
            }
        }

        public void ClearTemporaryFiles()
        {
            //CloudTableClient tableClient = Account.CreateCloudTableClient();
            //CloudTable storagesMetadataTable = tableClient.GetTableReference("StoragesMetadata");
            var storagesMetadataTable_a = TablesAccount.GetTableClient("StoragesMetadata");

            foreach (var cloudTableName in (StorageMetaData as Storage).AzureStorageMetadata.GetTemporaryTablesNames())
            {
                if (string.IsNullOrWhiteSpace(cloudTableName))
                    continue;

                //CloudTable azureTable = tableClient.GetTableReference(cloudTableName);
                //azureTable.DeleteIfExists();
                Azure.Data.Tables.TableClient azureTable_a = TablesAccount.GetTableClient(cloudTableName);
                azureTable_a.Delete();

            }

            (StorageMetaData as Storage).AzureStorageMetadata.RemoveTemporaryTables((StorageMetaData as Storage).AzureStorageMetadata.GetTemporaryTablesNames());

            //TableOperation updateOperation = TableOperation.Replace((StorageMetaData as Storage).AzureStorageMetadata);
            //var result = storagesMetadataTable.Execute(updateOperation);

            var result = storagesMetadataTable_a.UpdateEntity((StorageMetaData as Storage).AzureStorageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);


        }



        /// <MetaDataID>{56a4e228-96ee-437d-b730-ce5230da8535}</MetaDataID>
        public override object GetObject(string persistentObjectUri)
        {
            string[] persistentObjectUriParts = persistentObjectUri.Split('\\');
            string storageIdentity = persistentObjectUriParts[0];
            string storageSerialNumber = persistentObjectUriParts[1];
            string objectIdentityAsString = persistentObjectUriParts[2];

            MetaDataRepository.StorageCell storageCell = GetStorageCell(int.Parse(storageSerialNumber));

            if (storageCell == null)
                return null;
            var objectID = storageCell.ObjectIdentityType.Parse(objectIdentityAsString, storageIdentity);

            var operativeObjectCollection = OperativeObjectCollections[storageCell.Type.GetExtensionMetaObject<System.Type>()];
            var storageInstanceRef = operativeObjectCollection[objectID];
            if (storageInstanceRef != null)
            {
                //storageInstanceRef.WaitUntilObjectIsActive();
                return storageInstanceRef.MemoryInstance;
            }
            else
            {
                object @object = GetObjectFromStorageCell(storageCell, objectID);
                return @object;
            }


            return null;

        }

        internal List<string> GetAzureStorageDataTablesNames()
        {

            List<string> azureStorageDataTables = new List<string>();
            //CloudTableClient tableClient = Account.CreateCloudTableClient();
            string prefix = (this.StorageMetaData as WindowsAzureTablesPersistenceRunTime.Storage).TablePrefix;
            string prefixFilter = Azure.Data.Tables.TableServiceClient.CreateQueryFilter(x => x.Name.CompareTo(prefix) >= 0
                                     && x.Name.CompareTo(prefix + char.MaxValue) <= 0);

            var dataTables = TablesAccount.Query(prefixFilter).ToList();

            //var dataTables = tableClient.ListTables((this.StorageMetaData as WindowsAzureTablesPersistenceRunTime.Storage).TablePrefix).ToList();
            var tables = (from table in (StorageMetaData as MetaDataRepository.Namespace).OwnedElements.OfType<RDBMSMetaDataRepository.Table>()
                          select table).ToList();


            foreach (var dataTable in dataTables)
            {
                var tableMetaData = (from table in tables
                                     where table.DataBaseTableName == dataTable.Name
                                     select table).FirstOrDefault();


                if (tableMetaData != null)
                    azureStorageDataTables.Add(dataTable.Name);
            }
            return azureStorageDataTables;
        }

        internal void DeleteTableRecords(IDataTable dataTable)
        {
            if (dataTable.Rows.Count > 0)
            {

                //CloudTableClient tableClient = Account.CreateCloudTableClient();
                string cloudTableName = dataTable.TableName;

                //CloudTable azureTable = tableClient.GetTableReference(cloudTableName);
                //if (!azureTable.Exists())
                //    azureTable.CreateIfNotExists();

                Azure.Data.Tables.TableClient azureTable_a = TablesAccount.GetTableClient(cloudTableName);
                if (!TableExist(cloudTableName))
                    azureTable_a.CreateIfNotExists();

                string partitionKey = null;

                foreach (IDataRow row in dataTable.Rows)
                {
                    StorageInstanceRef storageInstanceRef = row["StorageInstanceRef"] as StorageInstanceRef;

                    RDBMSMetaDataRepository.StorageCell storageCell = storageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell;
                    RDBMSMetaDataRepository.Table table = storageCell.MainTable;
                    if (table != null && table.DataBaseTableName != null && table.DataBaseTableName.ToLower() == "TjimliakosgmailcomOptionChange".ToLower())
                    {

                    }
                    partitionKey = storageCell.SerialNumber.ToString();

                    Guid OID = Guid.NewGuid();
                    if (storageCell != null)
                        OID = (Guid)row[table.ObjectIDColumns[0].Name];
                    ElasticTableEntity entity = storageInstanceRef.TableEntity;
                    entity.RowKey = OID.ToString().ToLower();
                    entity.PartitionKey = partitionKey;
                    ////foreach (System.Data.DataColumn column in dataTable.Columns)
                    ////{
                    ////    if (column.ColumnName == "StorageInstanceRef_" + dataTable.GetHashCode().ToString())
                    ////        continue;

                    ////    if (column.ColumnName == "StorageInstanceRef")
                    ////        continue;

                    ////    if (table.ObjectIDColumns.Count > 0 && column.ColumnName == table.ObjectIDColumns[0].Name)
                    ////        continue;
                    ////    if (row[column.ColumnName] != DBNull.Value)
                    ////        entity[column.ColumnName] = row[column.ColumnName];
                    ////    else
                    ////        entity[column.ColumnName] = null;

                    ////}

                    //GetTableBatchOperation(azureTable).Delete(entity);
                    var tableBatchOperation = GetTableBatchOperation(azureTable_a);
                    tableBatchOperation.Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.Delete, entity.Values));

                }
            }
        }

        /// <MetaDataID>{88bbfd07-cc25-4c87-80fb-b3a81bd1389d}</MetaDataID>
        public override string GetPersistentObjectUri(object obj)
        {
            if (obj is StorageInstanceRef)
            {
                StorageInstanceRef storageInstanceRef = obj as StorageInstanceRef;
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.StorageInstanceSet.SerialNumber.ToString() + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            else if (PersistencyService.ClassOfObjectIsPersistent(obj))
            {
                PersistenceLayer.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(obj);
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.StorageInstanceSet.SerialNumber.ToString() + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            return null;
        }




        internal void ActivateObjectsInPreoadedStorageCells()
        {

        }




        /// <MetaDataID>{13d370ab-1bc3-4df4-8ce8-3dc630b38690}</MetaDataID>
        public override PersistenceLayer.ObjectID GetTemporaryObjectID()
        {
            return new ObjectID(System.Guid.NewGuid());
        }

        /// <MetaDataID>{59ea159e-8116-47e8-b40e-a7913ac73d89}</MetaDataID>
        public override void MakeChangesDurable(TransactionContext theTransaction)
        {

            lock (TableBatchOperations_a)
            {
                //foreach (var transactionTableBatchOperationsEntry in TableBatchOperations[theTransaction.Transaction.LocalTransactionUri.ToLower()])
                //{
                //    foreach (var tableBatchOperation in transactionTableBatchOperationsEntry.Value)
                //    {
                //        try
                //        {


                //            transactionTableBatchOperationsEntry.Key.ExecuteBatch(tableBatchOperation);
                //        }
                //        catch (Microsoft.Azure.Cosmos.Table.StorageException storageException)
                //        {
                //            int? httpStatusCode = storageException.RequestInformation.HttpStatusCode;

                //            if (httpStatusCode == 412)
                //            {
                //                int npos = storageException.RequestInformation.HttpStatusMessage.IndexOf(":");
                //                if (npos != -1)
                //                {
                //                    int tableOparationindex = int.Parse(storageException.RequestInformation.HttpStatusMessage.Substring(0, npos));

                //                    TableOperation tableOperation = tableBatchOperation[tableOparationindex];

                //                    string partitionKey = tableOperation.Entity.PartitionKey;
                //                    string rowKey = tableOperation.Entity.RowKey;
                //                    tableBatchOperation.RemoveAt(tableOparationindex);
                //                }
                //            }

                //            //TODO export critical error to log file
                //            throw;


                //            // Gives you the index of the failed operation in a azure table batch operation

                //            // int failedOperationIndex = storageException..RequestInformation..GetFailedOperationIndex();

                //        }
                //    }
                //}

                //TableBatchOperations.Remove(theTransaction.Transaction.LocalTransactionUri.ToLower());


                if (TableBatchOperations_a.ContainsKey(theTransaction.Transaction.LocalTransactionUri.ToLower()))
                {
                    DateTime start = DateTime.Now;

                    foreach (var transactionTableBatchOperationsEntry in TableBatchOperations_a[theTransaction.Transaction.LocalTransactionUri.ToLower()])
                    {
                        foreach (var tableBatchOperation in transactionTableBatchOperationsEntry.Value)
                        {

                            bool canRetry = false;
                            int retryCount = 5;
                            Retry:

                            try
                            {

                                if(tableBatchOperation.Count>0)
                                    transactionTableBatchOperationsEntry.Key.SubmitTransaction(tableBatchOperation);
                                else
                                {

                                }
                            }
                            catch (Azure.Data.Tables.TableTransactionFailedException tableTransactionError)
                            {
                                if (tableTransactionError.ErrorCode == "InternalError" && retryCount > 0)
                                {

                                    retryCount--;
                                    System.Threading.Thread.Sleep(100);
                                    goto Retry;
                                }

                                if (!canRetry)
                                    throw;
                            }
                            catch (Exception error)
                            {
                                if (!canRetry)
                                    throw;
                            }

                        }
                    }
                    TimeSpan timeSpan = DateTime.Now - start;
                    //System.Windows.Forms.MessageBox.Show(timeSpan.ToString());
                    TableBatchOperations_a.Remove(theTransaction.Transaction.LocalTransactionUri.ToLower());
                }

            }
        }

        //System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<CloudTable, System.Collections.Generic.List<TableBatchOperation>>> TableBatchOperations = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<CloudTable, System.Collections.Generic.List<TableBatchOperation>>>();

        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Azure.Data.Tables.TableClient, System.Collections.Generic.List<System.Collections.Generic.List<Azure.Data.Tables.TableTransactionAction>>>> TableBatchOperations_a = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Azure.Data.Tables.TableClient, System.Collections.Generic.List<System.Collections.Generic.List<Azure.Data.Tables.TableTransactionAction>>>>();
        //TableBatchOperation GetTableBatchOperation(CloudTable azureTable)
        //{
        //    lock (TableBatchOperations)
        //    {
        //        string localTransactionUri = "null_transaction";

        //        if (OOAdvantech.Transactions.Transaction.Current != null)
        //            localTransactionUri = OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri.ToLower();

        //        System.Collections.Generic.Dictionary<CloudTable, System.Collections.Generic.List<TableBatchOperation>> transactionTableBatchOperations = null;
        //        if (!TableBatchOperations.TryGetValue(localTransactionUri, out transactionTableBatchOperations))
        //        {
        //            transactionTableBatchOperations = new System.Collections.Generic.Dictionary<CloudTable, System.Collections.Generic.List<TableBatchOperation>>();
        //            TableBatchOperations[localTransactionUri] = transactionTableBatchOperations;
        //        }

        //        System.Collections.Generic.List<TableBatchOperation> tableBatchOperations = null;

        //        if (!transactionTableBatchOperations.TryGetValue(azureTable, out tableBatchOperations))
        //        {
        //            tableBatchOperations = new System.Collections.Generic.List<TableBatchOperation>();
        //            transactionTableBatchOperations[azureTable] = tableBatchOperations;
        //        }


        //        if (tableBatchOperations.Count == 0 || tableBatchOperations.Last().Count == 100)
        //            tableBatchOperations.Add(new TableBatchOperation());
        //        return tableBatchOperations.Last();
        //    }
        //    //return tableBatchOperation;

        //}


        internal System.Collections.Generic.List<Azure.Data.Tables.TableTransactionAction> GetTableBatchOperation(Azure.Data.Tables.TableClient azureTable)
        {
            lock (TableBatchOperations_a)
            {
                string localTransactionUri = "null_transaction";

                if (OOAdvantech.Transactions.Transaction.Current != null)
                    localTransactionUri = OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri.ToLower();

                Dictionary<Azure.Data.Tables.TableClient, List<List<Azure.Data.Tables.TableTransactionAction>>> transactionTableBatchOperations = null;

                if (!TableBatchOperations_a.TryGetValue(localTransactionUri, out transactionTableBatchOperations))
                {
                    transactionTableBatchOperations = new Dictionary<Azure.Data.Tables.TableClient, List<List<Azure.Data.Tables.TableTransactionAction>>>();
                    TableBatchOperations_a[localTransactionUri] = transactionTableBatchOperations;
                }

                List<List<Azure.Data.Tables.TableTransactionAction>> tableBatchOperations = null;

                if (!transactionTableBatchOperations.TryGetValue(azureTable, out tableBatchOperations))
                {
                    tableBatchOperations = new List<List<Azure.Data.Tables.TableTransactionAction>>();
                    transactionTableBatchOperations[azureTable] = tableBatchOperations;
                }



                if (tableBatchOperations.Count == 0 || tableBatchOperations.Last().Count == 100)
                    tableBatchOperations.Add(new List<Azure.Data.Tables.TableTransactionAction>());
                return tableBatchOperations.Last();
            }
            //return tableBatchOperation;

        }

        /// <MetaDataID>{7cf92080-f2b8-4140-94ed-3b1589bce361}</MetaDataID>
        public override void PrepareForChanges(TransactionContext theTransaction)
        {

        }

        public override MetaDataRepository.StorageCell GetStorageCell(StorageInstanceAgent storageInstanceAgent)
        {
            if (storageInstanceAgent.StorageIdentity == StorageMetaData.StorageIdentity)
                return storageInstanceAgent.RealStorageInstanceRef.StorageInstanceSet;
            RDBMSMetaDataRepository.Class _class = (StorageMetaData as Storage).GetEquivalentMetaObject(storageInstanceAgent.Class) as RDBMSMetaDataRepository.Class;

            if (_class == null)
            {

            }
            return _class.GetStorageCellReference(storageInstanceAgent.StorageCellName,
                                            storageInstanceAgent.StorageCellSerialNumber,
                                            storageInstanceAgent.StorageName,
                                            storageInstanceAgent.ObjectIdentityType,
                                            storageInstanceAgent.StorageIdentity,
                                            storageInstanceAgent.StorageLocation,
                                            storageInstanceAgent.StorageType);
        }

        public override void CreateUnlinkAllObjectCommand(StorageInstanceAgent deletedOutStorageInstanceRef, MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.StorageCell LinkedObjectsStorageCell)
        {

            RDBMSMetaDataRepository.AssociationEnd rdbmsAssociationEnd = (this.StorageMetaData as Storage).GetEquivalentMetaObject(associationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            foreach (var storageCellsLink in (rdbmsAssociationEnd.Association as RDBMSMetaDataRepository.Association).HierarchyStorageCellsLinks)
            {
                if (associationEnd.IsRoleA &&
                    (storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink).RoleAStorageCell == LinkedObjectsStorageCell &&
                    (storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink).RoleBStorageCell.StorageIdentity == deletedOutStorageInstanceRef.StorageIdentity &&
                    deletedOutStorageInstanceRef.StorageCellSerialNumber == (storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink).RoleBStorageCell.SerialNumber)
                {
                    UnlinkAllObjectOfStorageCellLinkCmd mUnlinkAllObjectOfStorageCellLinkCmd = new UnlinkAllObjectOfStorageCellLinkCmd(this, storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink, deletedOutStorageInstanceRef, rdbmsAssociationEnd);
                    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;

                    if (!transactionContext.EnlistedCommands.ContainsKey(mUnlinkAllObjectOfStorageCellLinkCmd.Identity))
                        transactionContext.EnlistCommand(mUnlinkAllObjectOfStorageCellLinkCmd);


                    break;
                }

                if (!associationEnd.IsRoleA &&
                  (storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink).RoleBStorageCell == LinkedObjectsStorageCell &&
                  (storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink).RoleAStorageCell.StorageIdentity == deletedOutStorageInstanceRef.StorageIdentity &&
                  deletedOutStorageInstanceRef.StorageCellSerialNumber == (storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink).RoleAStorageCell.SerialNumber)
                {
                    UnlinkAllObjectOfStorageCellLinkCmd mUnlinkAllObjectOfStorageCellLinkCmd = new UnlinkAllObjectOfStorageCellLinkCmd(this, storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink, deletedOutStorageInstanceRef, rdbmsAssociationEnd);
                    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;

                    if (!transactionContext.EnlistedCommands.ContainsKey(mUnlinkAllObjectOfStorageCellLinkCmd.Identity))
                        transactionContext.EnlistCommand(mUnlinkAllObjectOfStorageCellLinkCmd);

                    break;
                }

            }


            //PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand unlinkAllObject = new Commands.UnlinkAllObjectCommand(deletedOutStorageInstanceRef, this);
            //PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            //unlinkAllObject.theAssociationEnd = associationEnd;
            //transactionContext.EnlistCommand(unlinkAllObject);

        }

        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID, MetaDataRepository.StorageCell storageCell)
        {
            return new StorageInstanceRef(memoryInstance, storageCell, this, objectID);
        }

        protected override MetaDataRepository.StorageCellReference GetStorageCellReference(MetaDataRepository.StorageCell storageCell)
        {
            throw new NotImplementedException();
        }

        public override Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {
            throw new NotImplementedException();
        }

        public override Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ValueTypePath valueTypePath, Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> rootStorageCells, string ofTypeIdentity = null)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                List<MetaDataRepository.StorageCellsLink> storageCellsLinks = new List<OOAdvantech.MetaDataRepository.StorageCellsLink>();
                Dictionary<MetaDataRepository.StorageCell, MetaDataRepository.RelatedStorageCell> linkedStorageCells = new Dictionary<OOAdvantech.MetaDataRepository.StorageCell, OOAdvantech.MetaDataRepository.RelatedStorageCell>();
                foreach (OOAdvantech.MetaDataRepository.RelatedStorageCell relatedStorageCell in base.GetLinkedStorageCellsFromObjectsUnderTransaction(associationEnd, valueTypePath, rootStorageCells))
                {
                    if (!linkedStorageCells.ContainsKey(relatedStorageCell.StorageCell))
                        linkedStorageCells.Add(relatedStorageCell.StorageCell, relatedStorageCell);
                }
                associationEnd = (StorageMetaData as Storage).GetEquivalentMetaObject(associationEnd.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                foreach (MetaDataRepository.StorageCell rootStorageCell in rootStorageCells)
                {
                    if (rootStorageCell is MetaDataRepository.StorageCellReference)
                        continue;
                    bool withRelationTable = false;
                    if (associationEnd != null &&
                      (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                      associationEnd.Association.Specializations.Count > 0) &&
                      associationEnd.Association != rootStorageCell.Type.ClassHierarchyLinkAssociation)
                    {
                        withRelationTable = true;
                    }

                    RDBMSMetaDataRepository.Association storageAssociation = associationEnd.Association as RDBMSMetaDataRepository.Association;
                    foreach (MetaDataRepository.StorageCellsLink storageCellsLink in storageAssociation.HierarchyStorageCellsLinks)
                    {
                        if (storageCellsLinks.Contains(storageCellsLink) || valueTypePath.ToString() != storageCellsLink.ValueTypePath)
                            continue;

                        bool hasRelationTable = (storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink).ObjectLinksTable != null | withRelationTable;

                        #region relation through association class
                        if (storageCellsLink.AssotiationClassStorageCells.Contains(rootStorageCell))
                        {
                            storageCellsLinks.Add(storageCellsLink);
                            if (associationEnd.Role == MetaDataRepository.Roles.RoleA)
                            {
                                if (storageCellsLink.RoleAStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                                {
                                    if (!linkedStorageCells.ContainsKey((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell))
                                        linkedStorageCells.Add((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                }
                                else
                                {
                                    if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleAStorageCell))
                                        linkedStorageCells.Add(storageCellsLink.RoleAStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleAStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                }
                            }
                            else
                            {

                                if (storageCellsLink.RoleBStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                                {
                                    if (!linkedStorageCells.ContainsKey((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell))
                                        linkedStorageCells.Add((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                }
                                else
                                {
                                    if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleBStorageCell))
                                        linkedStorageCells.Add(storageCellsLink.RoleBStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleBStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                }
                            }
                        }
                        #endregion

                        if (associationEnd.Role == MetaDataRepository.Roles.RoleA && storageCellsLink.RoleBStorageCell == rootStorageCell)
                        {
                            storageCellsLinks.Add(storageCellsLink);
                            if (storageCellsLink.RoleAStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                            {
                                if (!linkedStorageCells.ContainsKey((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell))
                                    linkedStorageCells.Add((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleAStorageCell))
                                    linkedStorageCells.Add(storageCellsLink.RoleAStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleAStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                            }
                            else
                            {
                                if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleAStorageCell))
                                    linkedStorageCells.Add(storageCellsLink.RoleAStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleAStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                            }
                        }
                        else
                        {
                            if (storageCellsLink.RoleAStorageCell == rootStorageCell)
                            {
                                storageCellsLinks.Add(storageCellsLink);
                                if (storageCellsLink.RoleBStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                                {
                                    if (!linkedStorageCells.ContainsKey((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell))
                                        linkedStorageCells.Add((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                    if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleBStorageCell))
                                        linkedStorageCells.Add(storageCellsLink.RoleBStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleBStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                }
                                else
                                {
                                    if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleBStorageCell))
                                        linkedStorageCells.Add(storageCellsLink.RoleBStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleBStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                }
                            }
                        }
                    }
                }
                return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell>(linkedStorageCells.Values);
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }

        }


        public override Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(Classifier classifier)
        {
            Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = null;
            if (classifier is MetaDataRepository.Class)
            {
                storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                MetaDataRepository.Class storageClass = (StorageMetaData as Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Class)) as MetaDataRepository.Class;
                storageCells.AddRange(storageClass.StorageCellsOfThisType);
            }
            else if (classifier is MetaDataRepository.Interface)
            {
                storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                MetaDataRepository.Interface storageInterface = (StorageMetaData as Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Interface)) as MetaDataRepository.Interface;
                storageCells.AddRange(storageInterface.StorageCellsOfThisType);
            }
            if (storageCells == null)
                throw new Exception("The method or operation is implemented for classes and interfaces.");
            foreach (var storageReference in (StorageMetaData as MetaDataRepository.Storage).LinkedStorages)
            {
                var openStorage = storageReference.OpenObjectSorage();
                if (openStorage != this)
                    storageCells.AddRange(storageReference.OpenObjectSorage().GetStorageCells(classifier));
            }

            return storageCells;
        }

        public override Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> GetRelationObjectsStorageCells(MetaDataRepository.Association association, Collections.Generic.Set<MetaDataRepository.StorageCell> relatedStorageCells, Roles storageCellsRole, string ofTypeIdentity = null)
        {

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                association = (StorageMetaData as Storage).GetEquivalentMetaObject(association) as OOAdvantech.MetaDataRepository.Association;
                Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.RelatedStorageCell>();
                foreach (MetaDataRepository.StorageCell storageCell in relatedStorageCells)
                {
                    foreach (MetaDataRepository.StorageCellsLink storageCellsLink in association.StorageCellsLinks)
                    {
                        if (storageCellsRole == MetaDataRepository.Roles.RoleA && storageCellsLink.RoleAStorageCell == storageCell)
                        {

                            storageCells.AddRange((from assotiationClassStorageCells in storageCellsLink.AssotiationClassStorageCells
                                                   select new RelatedStorageCell(assotiationClassStorageCells, storageCell, association.RoleB.Identity.ToString(), false)).ToList());

                        }
                        else
                        {
                            if (storageCellsLink.RoleBStorageCell == storageCell)
                                storageCells.AddRange((from assotiationClassStorageCells in storageCellsLink.AssotiationClassStorageCells
                                                       select new RelatedStorageCell(assotiationClassStorageCells, storageCell, association.RoleA.Identity.ToString(), false)).ToList());
                        }
                    }
                }
                return storageCells;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }

        public override MetaDataRepository.StorageCell GetStorageCell(int storageCellSerialNumber)
        {

            try
            {
                PersistenceLayer.ObjectStorage ObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(StorageMetaData.StorageName, StorageMetaData.StorageLocation, StorageMetaData.StorageType);
                string Query = "SELECT StorageCell FROM " + typeof(RDBMSMetaDataRepository.StorageCell).FullName + " StorageCell WHERE StorageCell.SerialNumber = " + storageCellSerialNumber.ToString();

                Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorage(ObjectStorage.StorageMetaData).Execute(Query);
                foreach (Collections.StructureSet Rowset in aStructureSet)
                {
                    //TODO: να τσεκαριστή εάν υπάρχει συμβατότητα μεταξύ της class που τρέχει τοπικά
                    //και αυτής που τρέχει remotely
                    //WHERE oid = "+StorageCellID.ToString();
                    RDBMSMetaDataRepository.StorageCell storageCell = (RDBMSMetaDataRepository.StorageCell)Rowset["StorageCell"];
                    return storageCell;
                }
                
                return null;
            }
            catch (Exception error)
            {

                throw;
            }

            //throw new NotImplementedException();
        }

        public override MetaDataRepository.StorageCell GetStorageCell(object ObjectID)
        {
            throw new NotImplementedException();
        }

        public override MetaDataRepository.ObjectQueryLanguage.DataLoader CreateDataLoader(DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
        {
            return new DataLoader(dataNode, dataLoaderMetadata);
        }
        static byte[] Buffer = new byte[66536];
        static byte[] MembersBuffer = new byte[16384];


        public override void Backup(IBackupArchive archive)
        {


            OOAdvantech.Linq.Storage storage = new Linq.Storage(PersistenceLayer.ObjectStorage.GetStorageOfObject(StorageMetaData));


            if (System.IO.File.Exists(archive.LocalFileName))
                System.IO.File.Delete(archive.LocalFileName);

            System.IO.Stream memoryStream = System.IO.File.Open(archive.LocalFileName, FileMode.CreateNew);
            int offset = 0;
            try
            {

                //CloudTableClient tableClient = Account.CreateCloudTableClient();
                AzureTableMetaDataPersistenceRunTime.Storage metaDataStorage = null;

                if (TablesAccount.AccountName == "devstoreaccount1")
                    metaDataStorage = PersistenceLayer.ObjectStorage.OpenStorage(this.StorageMetaData.StorageName, this.StorageMetaData.StorageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.StorageProvider").StorageMetaData as AzureTableMetaDataPersistenceRunTime.Storage;
                else
                    metaDataStorage = PersistenceLayer.ObjectStorage.OpenStorage(this.StorageMetaData.StorageName, this.StorageMetaData.StorageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.StorageProvider", UserName, Pasword).StorageMetaData as AzureTableMetaDataPersistenceRunTime.Storage;


                var classBLOBDataColumns = AzureTableMetaDataPersistenceRunTime.ClassBLOBData.ListOfColumns;
                var classBLOBDataTable = TablesAccount.GetTableClient(metaDataStorage.ClassBLOBDataTableName);


                var objectBLOBDataColumns = AzureTableMetaDataPersistenceRunTime.ObjectBLOBData.ListOfColumns;
                var objectBLOBDataTable = TablesAccount.GetTableClient(metaDataStorage.ObjectBLOBDataTableName);

                var metadataIdentitiesColumns = AzureTableMetaDataPersistenceRunTime.MetadataIdentities.ListOfColumns;
                var metadataIdentitiesTable = TablesAccount.GetTableClient(metaDataStorage.MetadataIdentityTableName);

                XDocument storageMetaDataDoc = XDocument.Parse("<StorageMetaData/>");
                storageMetaDataDoc.Root.SetAttributeValue("StorageName", StorageMetaData.StorageName);
                storageMetaDataDoc.Root.SetAttributeValue("StorageLocation", StorageMetaData.StorageLocation);
                storageMetaDataDoc.Root.SetAttributeValue("StorageType", StorageMetaData.StorageType);
                storageMetaDataDoc.Root.SetAttributeValue("StorageIdentity", StorageMetaData.StorageIdentity);


                storageMetaDataDoc.Root.SetAttributeValue("TablePrefix", (this.StorageMetaData as WindowsAzureTablesPersistenceRunTime.Storage).TablePrefix);
                storageMetaDataDoc.Root.SetAttributeValue("StoragePrefix", (this.StorageMetaData as WindowsAzureTablesPersistenceRunTime.Storage).AzureStorageMetadata.StoragePrefix);

                string storageMetaDataXml = storageMetaDataDoc.ToString();


                BinaryFormatter.BinaryFormatter.Serialize(storageMetaDataXml, Buffer, offset, ref offset);

                memoryStream.Write(new byte[1] { 0 }, 0, 1);
                byte[] headerΣtream = BitConverter.GetBytes(offset);
                memoryStream.Write(headerΣtream, 0, headerΣtream.Length);
                memoryStream.Write(Buffer, 0, offset);





                SerializeTable(memoryStream, classBLOBDataColumns, classBLOBDataTable);
                SerializeTable(memoryStream, objectBLOBDataColumns, objectBLOBDataTable);
                SerializeTable(memoryStream, metadataIdentitiesColumns, metadataIdentitiesTable);

                string prefix = (this.StorageMetaData as WindowsAzureTablesPersistenceRunTime.Storage).TablePrefix;
                string prefixFilter = Azure.Data.Tables.TableServiceClient.CreateQueryFilter(x => x.Name.CompareTo(prefix) >= 0
                                         && x.Name.CompareTo(prefix + char.MaxValue) <= 0);

                var dataTables = TablesAccount.Query(prefixFilter).ToList();
                var tables = (from table in storage.GetObjectCollection<RDBMSMetaDataRepository.Table>()
                              select table).ToList();

                foreach (var dataTable in dataTables)
                {
                    var tableMetaData = (from table in tables
                                         where table.DataBaseTableName == dataTable.Name
                                         select table).FirstOrDefault();
                    if (tableMetaData != null)
                    {
                        var columns = tableMetaData.ContainedColumns.Select(x => x.Name).ToArray();
                        List<Column> tableColumns = tableMetaData.ContainedColumns.ToList();

                        SerializeTable(memoryStream, tableColumns, TablesAccount.GetTableClient(dataTable.Name));

                    }
                }

            }
            finally
            {
                memoryStream.Close();
            }

            //CloudBlobClient blobClient = Account.CreateCloudBlobClient();
            //var container = blobClient.GetContainerReference("backup");

            //try
            //{
            //    CloudBlockBlob blob = container.GetBlockBlobReference("StorageBackup");
            //    blob.DeleteIfExists();
            //    memoryStream.Position = 0;
            //    blob.PutBlock(Guid.NewGuid().ToString("N"), memoryStream, null);


            //    SerializeTable(memoryStream, objectBLOBDataColumns, objectBLOBDataTable);
            //    memoryStream.Position = 0;
            //    blob.PutBlock(Guid.NewGuid().ToString("N"), memoryStream, null);

            //    blob = container.GetBlockBlobReference("StorageBackup");



            //}
            //catch (Exception error)
            //{


            //}
            memoryStream = System.IO.File.Open(archive.LocalFileName, FileMode.Open, FileAccess.Read);
            memoryStream.Position = 0;


            //DeserializeTable(memoryStream, out tableName, out deserializedEntities);
            //DeserializeTable(memoryStream, out tableName, out deserializedEntities);
            //DeserializeTable(memoryStream, out tableName, out deserializedEntities);


            memoryStream.Read(Buffer, 0, 1);
            int headerType = Buffer[0];
            memoryStream.Read(Buffer, 0, 4);
            int streamLength = BitConverter.ToInt32(Buffer, 0);
            memoryStream.Read(Buffer, 0, streamLength);

            offset = 0;
            string rStorageMetaDataXml = BinaryFormatter.BinaryFormatter.ToString(Buffer, offset, ref offset);
            string tableName = null;
            List<Azure.Data.Tables.TableEntity> deserializedEntities = null;

            while (memoryStream.Position < memoryStream.Length)
            {

                DeserializeTable(memoryStream, out tableName, out deserializedEntities);
            }
            memoryStream.Close();




            //var metaDataTebles = tableClient.ListTables((this.StorageMetaData as WindowsAzureTablesPersistenceRunTime.Storage).AzureStorageMetadata.StoragePrefix).ToList();

            //var dataTebles = tableClient.ListTables((this.StorageMetaData as WindowsAzureTablesPersistenceRunTime.Storage).TablePrefix).ToList();

        }

        static internal void DeserializeTable(Stream memoryStream, out string tableName, out List<Azure.Data.Tables.TableEntity> deserializedEntities)
        {

            deserializedEntities = new List<Azure.Data.Tables.TableEntity>();
            memoryStream.Read(Buffer, 0, 1);
            int headerType = Buffer[0];
            memoryStream.Read(Buffer, 0, 4);
            int streamLength = BitConverter.ToInt32(Buffer, 0);
            memoryStream.Read(Buffer, 0, streamLength);

            int offset = 0;
            tableName = BinaryFormatter.BinaryFormatter.ToString(Buffer, offset, ref offset);
            List<AzureTableMetaDataPersistenceRunTime.Member> members = new List<AzureTableMetaDataPersistenceRunTime.Member>();
            while (offset < streamLength)
            {
                string memberName = BinaryFormatter.BinaryFormatter.ToString(Buffer, offset, ref offset);

                string memberType = BinaryFormatter.BinaryFormatter.ToString(Buffer, offset, ref offset);
                AzureTableMetaDataPersistenceRunTime.Member member = new AzureTableMetaDataPersistenceRunTime.Member(memberName, (EdmType)Enum.Parse(typeof(EdmType), memberType));
                members.Add(member);
            }


            while (memoryStream.Position < memoryStream.Length)
            {
                memoryStream.Read(Buffer, 0, 1);
                headerType = Buffer[0];
                if (headerType == 1)
                {
                    memoryStream.Position = memoryStream.Position - 1;
                    break;
                }

                memoryStream.Read(Buffer, 0, 4);
                streamLength = BitConverter.ToInt32(Buffer, 0);
                memoryStream.Read(Buffer, 0, streamLength);
                Azure.Data.Tables.TableEntity entity = new Azure.Data.Tables.TableEntity();
                offset = 0;
                foreach (var member in members)
                    member.LoadMemberData(Buffer, offset, out offset, entity);
                deserializedEntities.Add(entity);
            }
        }

        private static void SerializeTable(System.IO.Stream memoryStream, List<RDBMSMetaDataRepository.Column> columns, Azure.Data.Tables.TableClient cloudTable)
        {
            var tableColumns = columns.Select(x => x.DataBaseColumnName).ToList();
            if (!tableColumns.Contains("PartitionKey"))
                tableColumns.Add("PartitionKey");
            if (!tableColumns.Contains("RowKey"))
                tableColumns.Add("RowKey");
            //var query = new TableQuery<ElasticTableEntity>();
            var tableEntities = cloudTable.Query<Azure.Data.Tables.TableEntity>(default(string), default(int?), tableColumns).Select(x => new ElasticTableEntity(x)).ToList();

            Dictionary<string, AzureTableMetaDataPersistenceRunTime.Member> tableMembersDictionary = new Dictionary<string, AzureTableMetaDataPersistenceRunTime.Member>();
            List<AzureTableMetaDataPersistenceRunTime.Member> tableMembers = new List<AzureTableMetaDataPersistenceRunTime.Member>();

            AzureTableMetaDataPersistenceRunTime.Member member = new AzureTableMetaDataPersistenceRunTime.Member("PartitionKey", EdmType.String);
            tableMembersDictionary[member.Name] = member;
            tableMembers.Add(member);
            member = new AzureTableMetaDataPersistenceRunTime.Member("RowKey", EdmType.String);
            tableMembersDictionary[member.Name] = member;
            tableMembers.Add(member);
            foreach (var column in columns)
            {
                member = new AzureTableMetaDataPersistenceRunTime.Member(column.DataBaseColumnName, TypeDictionary.GetEdmType(column.Type.GetExtensionMetaObject<System.Type>()));
                tableMembersDictionary[column.DataBaseColumnName] = member;
                tableMembers.Add(member);
            }

            int offset = 0;
            BinaryFormatter.BinaryFormatter.Serialize(cloudTable.Name, MembersBuffer, offset, ref offset);

            foreach (var aMember in tableMembers)
            {
                BinaryFormatter.BinaryFormatter.Serialize(aMember.Name, MembersBuffer, offset, ref offset);
                BinaryFormatter.BinaryFormatter.Serialize(aMember.EdmType.ToString(), MembersBuffer, offset, ref offset);
            }

            byte[] membersBuffer = new byte[offset];
            System.Buffer.BlockCopy(MembersBuffer, 0, membersBuffer, 0, offset);


            offset = 0;
            //BinaryFormatter.BinaryFormatter.Serialize(membersBuffer, BinaryFormatter.ByteStreamType.Medium, Buffer, offset, ref offset);
            memoryStream.Write(new byte[1] { 1 }, 0, 1);
            byte[] headerΣtream = BitConverter.GetBytes(membersBuffer.Length);
            memoryStream.Write(headerΣtream, 0, headerΣtream.Length);
            memoryStream.Write(membersBuffer, 0, membersBuffer.Length);
            foreach (ElasticTableEntity entity in tableEntities)
            {
                offset = 0;
                foreach (var aMember in tableMembers)
                {
                    if (aMember.Name == "PartitionKey" && !string.IsNullOrWhiteSpace(entity.PartitionKey))
                        aMember.SaveMemberData(Buffer, entity.PartitionKey, offset, ref offset);
                    else if (aMember.Name == "RowKey" && !string.IsNullOrWhiteSpace(entity.RowKey))
                        aMember.SaveMemberData(Buffer, entity.RowKey, offset, ref offset);
                    else
                    {
                        aMember.SaveMemberData(Buffer, entity.Properties[aMember.Name], offset, ref offset);
                    }
                }
                memoryStream.Write(new byte[1] { 2 }, 0, 1);
                headerΣtream = BitConverter.GetBytes(offset);
                memoryStream.Write(headerΣtream, 0, headerΣtream.Length);
                memoryStream.Write(Buffer, 0, offset);
            }


        }

        protected override void UpdateOperativeObjects()
        {
        }

        object AzureStorageTabesLock = new object();
        List<string> AzureStorageTabes = null;
        internal bool TableExist(string cloudTableName)
        {
            lock (AzureStorageTabesLock)
            {
                if (AzureStorageTabes == null)
                {
                    AzureStorageTabes = TablesAccount.Query().Select(x => x.Name).ToList();
                    return AzureStorageTabes.Contains(cloudTableName);
                }
                if (AzureStorageTabes.Contains(cloudTableName))
                    return true;
                else
                {
                    Azure.Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = TablesAccount.Query(String.Format("TableName eq '{0}'", cloudTableName));
                    bool azureTable_exist = queryTableResults.Count() > 0;
                    if (azureTable_exist)
                    {
                        AzureStorageTabes.Add(cloudTableName);
                        return true;
                    }
                    else
                        return false;

                }
            }
        }
    }
}
