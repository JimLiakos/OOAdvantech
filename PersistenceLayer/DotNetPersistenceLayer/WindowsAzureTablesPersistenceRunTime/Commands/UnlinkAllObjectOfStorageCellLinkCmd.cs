using System;

using OOAdvantech.Collections.Generic;
using OOAdvantech.PersistenceLayerRunTime;
using OOAdvantech.RDBMSMetaDataRepository;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{


    //using ObjectID = OOAdvantech.PersistenceLayer.ObjectID;

    /// <MetaDataID>{FA56914D-92B1-4EF1-93F7-E09A655A8FE0}</MetaDataID>
    public class UnlinkAllObjectOfStorageCellLinkCmd : PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand
    {
        PersistenceLayerRunTime.ObjectStorage ObjectStorage;

        /// <MetaDataID>{0AF125BC-F029-487B-A331-33210334BB96}</MetaDataID>
        private RDBMSMetaDataRepository.StorageCellsLink StorageCellsLink;
        /// <MetaDataID>{E952FF71-031A-4C1C-9FFC-E9E29931BF05}</MetaDataID>
        public UnlinkAllObjectOfStorageCellLinkCmd(PersistenceLayerRunTime.ObjectStorage objectStorage, RDBMSMetaDataRepository.StorageCellsLink storageCellsLink, PersistenceLayerRunTime.StorageInstanceAgent deletedStorageInstance, MetaDataRepository.AssociationEnd associationEnd)
            : base(deletedStorageInstance)
        {
            ObjectStorage = objectStorage;
            StorageCellsLink = storageCellsLink;
            theAssociationEnd = associationEnd;
        }

        /// <MetaDataID>{AE7F43D3-C137-4667-888B-A516F7FCB39B}</MetaDataID>
        public override int ExecutionOrder
        {
            get
            {
                return 50;
            }
        }
        public override string Identity
        {
            get
            {
                return "unlinkallstoragecell" + DeletedStorageInstance.MemoryID.ToString() + theAssociationEnd.Identity + StorageCellsLink.Identity;
            }
        }


        /// <MetaDataID>{13CDCF90-1CE8-4E8E-BFAF-DA5EFF5C1439}</MetaDataID>
        public override void Execute()
        {
            if (StorageCellsLink.ObjectLinksTable == null)
                //if (MetaDataRepository.AssociationType.ManyToMany != StorageCellsLink.Type.MultiplicityType)
                UnlinkOnetoMany_OneToOne();
            else
                UnlinkManyToMany();


        }

        /// <summary>
        /// Deletes row that refer to deleted object
        /// </summary>
        /// <param name="storageCellsLink">
        /// Defines the logical link between storageCells 
        /// </param>
        /// <param name="objectLinksTable">
        /// Defines the table witch contains the objects link rows
        /// </param>
        /// <param name="deletedObjectRefColumns">
        /// Defines the reference columns for deleted object
        /// </param>
        /// <param name="deletedObjectID">
        /// Defines the object identity of deleted Object
        /// </param>
        void DeleteTableRowsForManyToManyRelation( StorageCellsLink storageCellsLink,
                                                    Table objectLinksTable,
                                                    Set<RDBMSMetaDataRepository.IdentityColumn> deletedObjectRefColumns,
                                                    PersistenceLayer.ObjectID deletedObjectID)
        {

            string partitionKey = storageCellsLink.RoleAStorageCell.SerialNumber.ToString() + "L" + storageCellsLink.RoleBStorageCell.SerialNumber.ToString();

            string cloudTableName = objectLinksTable.Name;
            System.Collections.Generic.IList<string> selectionColumns = new List<string>();
            //string UpdateQuery="UPDATE "+TableName +" SET ";
            string filterScript = (ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor("PartitionKey", "eq", partitionKey);
            foreach (RDBMSMetaDataRepository.IdentityColumn currColumn in deletedObjectRefColumns)
            {
                filterScript += " and ";
                filterScript += (ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor(currColumn.Name, "eq", ((ObjectID)deletedObjectID).GetMemberValue(currColumn.ColumnType));
            }

            foreach (var column in objectLinksTable.ContainedColumns)
            {
                if (!selectionColumns.Contains(column.DataBaseColumnName))
                    selectionColumns.Add(column.DataBaseColumnName);
            }

            if (!selectionColumns.Contains("RowKey"))
                selectionColumns.Add("RowKey");
            if (!selectionColumns.Contains("PartitionKey"))
                selectionColumns.Add("PartitionKey");

            var account = (ObjectStorage as ObjectStorage).Account;
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable azureTable = tableClient.GetTableReference(cloudTableName);
            if (azureTable.Exists())
            {
                var query = new TableQuery<ElasticTableEntity>();

                System.Collections.Generic.List<ElasticTableEntity> entities = null;
                if (string.IsNullOrWhiteSpace(filterScript))
                    entities = azureTable.ExecuteQuery(query.Select(selectionColumns)).ToList();
                else
                    entities = azureTable.ExecuteQuery(query.Select(selectionColumns).Where(filterScript)).ToList();

                foreach (ElasticTableEntity entity in entities)
                    (ObjectStorage as ObjectStorage).GetTableBatchOperation(azureTable).Delete(entity);

            }
        }




        /// <summary>
        /// Set rows reference object identity column with null
        /// </summary>
        /// <param name="storageCellWithColumns">
        /// Defines the storageCell which contains the reference data
        /// </param>
        /// <param name="keepObjectsLinkColumnTable">
        /// Defines the table witch contains the objects link rows
        /// </param>
        /// <param name="deletedObjectRefColumns">
        /// Defines the reference columns for deleted object
        /// </param>
        /// <param name="deletedObjectID">
        /// Defines the object identity of deleted Object
        /// </param>
        /// <MetaDataID>{420F725D-1F41-4CB0-9F61-2CDDA464F99E}</MetaDataID>
        void UpdateTableRowsForOneToMany_OneToOne(StorageCell storageCellWithColumns, RDBMSMetaDataRepository.Table keepObjectsLinkColumnTable,
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> deletedObjectRefColumns,
            PersistenceLayer.ObjectID deletedObjectID)
        {
            string partitionKey = storageCellWithColumns.SerialNumber.ToString();
            string cloudTableName = keepObjectsLinkColumnTable.Name;

           //Linq.Storage storage=new Linq.Storage(   PersistenceLayer.ObjectStorage.GetStorageOfObject(keepObjectsLinkColumnTable));
           // var storgeCell = (from storageCell in storage.GetObjectCollection<StorageCell>()
           //                   from table in storageCell.MappedTables
           //                   where table == keepObjectsLinkColumnTable || table == storageCell.MainTable
           //                   select storageCell).FirstOrDefault();

           // if(storgeCell!=null)!
           // {

           // }
             
            System.Collections.Generic.IList<string> selectionColumns = new List<string>();
            string filterScript = (ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor("PartitionKey", "eq", partitionKey);
            foreach (RDBMSMetaDataRepository.IdentityColumn currColumn in deletedObjectRefColumns)
            {
                filterScript += " and ";
                filterScript += (ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor(currColumn.Name, "eq", ((ObjectID)deletedObjectID).GetMemberValue(currColumn.ColumnType));
            }
            foreach (var column in keepObjectsLinkColumnTable.ContainedColumns)
            {
                if (!selectionColumns.Contains(column.DataBaseColumnName))
                    selectionColumns.Add(column.DataBaseColumnName);
            }

            if (!selectionColumns.Contains("RowKey"))
                selectionColumns.Add("RowKey");
            if (!selectionColumns.Contains("PartitionKey"))
                selectionColumns.Add("PartitionKey");

            var account = (ObjectStorage as ObjectStorage).Account;
            lock (account)
            {
                CloudTableClient tableClient = account.CreateCloudTableClient();
                CloudTable azureTable = tableClient.GetTableReference(cloudTableName);
                if (azureTable.Exists())
                {
                    var query = new TableQuery<ElasticTableEntity>();

                    System.Collections.Generic.List<ElasticTableEntity> entities = null;
                    if (string.IsNullOrWhiteSpace(filterScript))
                        entities = azureTable.ExecuteQuery(query.Select(selectionColumns)).ToList();
                    else
                        entities = azureTable.ExecuteQuery(query.Select(selectionColumns).Where(filterScript)).ToList();

                    foreach (ElasticTableEntity entity in entities)
                    {
                        StorageInstanceRef RecordOwnerObject = StorageInstanceRef.GetStorageInstanceRef(keepObjectsLinkColumnTable, entity.PartitionKey, entity.RowKey);
                        if (RecordOwnerObject != null)
                        {
                            foreach (RDBMSMetaDataRepository.IdentityColumn currColumn in deletedObjectRefColumns)
                                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(RecordOwnerObject, currColumn.DataBaseColumnName, DBNull.Value);
                        }
                        else
                        {
                            foreach (RDBMSMetaDataRepository.IdentityColumn currColumn in deletedObjectRefColumns)
                                entity[currColumn.Name] = null;
                            (ObjectStorage as ObjectStorage).GetTableBatchOperation(azureTable).Replace(entity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unlink all for onetoMany or oneToOne relation
        /// </summary>
        /// <MetaDataID>{8FD9F502-12E5-469B-BBD5-E64ED343C871}</MetaDataID>
        private void UnlinkOnetoMany_OneToOne()
        {

            AssociationEnd keepAssociationEndColumns = null;
            if (theAssociationEnd.Multiplicity.IsMany)
                keepAssociationEndColumns = theAssociationEnd as RDBMSMetaDataRepository.AssociationEnd; 
            if (keepAssociationEndColumns == null && theAssociationEnd.GetOtherEnd().Multiplicity.IsMany)
                keepAssociationEndColumns = theAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd;
            if (keepAssociationEndColumns == null)
                keepAssociationEndColumns = theAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd;

            RDBMSMetaDataRepository.StorageCell StorageCellWithColumns = null;
            if (keepAssociationEndColumns.IsRoleA)
                StorageCellWithColumns = StorageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell;
            else
                StorageCellWithColumns = StorageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell;
            if (StorageCellWithColumns.GetType() == typeof(RDBMSMetaDataRepository.StorageCellReference))
                return;
            RDBMSMetaDataRepository.Table keepColumnTable = null;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns = null;
            ObjectID deletedObjectID = (ObjectID)DeletedStorageInstance.PersistentObjectID;
            
            //TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class
            
            linkColumns = keepAssociationEndColumns.GetReferenceColumnsFor(StorageCellWithColumns);
            
            foreach (RDBMSMetaDataRepository.Column column in linkColumns)
            {
                keepColumnTable = column.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }

            UpdateTableRowsForOneToMany_OneToOne(StorageCellWithColumns, keepColumnTable, linkColumns,  deletedObjectID);

        }


        /// <summary>
        /// Unlink all for relation with association table
        /// </summary>
        /// <MetaDataID>{382629ED-5316-4E3D-A9D9-B03CC7D7BD7D}</MetaDataID>
        private void UnlinkManyToMany()
        {

            if (theAssociationEnd.Association.LinkClass == null)
            {
                RDBMSMetaDataRepository.AssociationEnd associationEndWithDeletedObjectIdentityColumns = theAssociationEnd as RDBMSMetaDataRepository.AssociationEnd;

                RDBMSMetaDataRepository.Table objectLinksTable = StorageCellsLink.ObjectLinksTable;

                ObjectID deletedObjectID = (ObjectID)DeletedStorageInstance.PersistentObjectID;

                ////TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class

                Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> deletedObjectIdentityColumns = associationEndWithDeletedObjectIdentityColumns.GetReferenceColumnsFor(objectLinksTable);
                DeleteTableRowsForManyToManyRelation(StorageCellsLink, objectLinksTable, deletedObjectIdentityColumns,  deletedObjectID);
            }
            else
                throw new NotImplementedException();

        }

        /// <MetaDataID>{0FF5AD67-A5C7-4DA1-BBE3-3B5FC0AB5777}</MetaDataID>
        public override void GetSubCommands(int currentOrder)
        {

        }

    }
}
