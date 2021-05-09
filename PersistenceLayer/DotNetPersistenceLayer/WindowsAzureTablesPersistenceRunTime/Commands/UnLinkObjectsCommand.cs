using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Microsoft.Azure.Cosmos.Table;
//using ObjectID = OOAdvantech.PersistenceLayer.ObjectID;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{
    /// <MetaDataID>{ec0e26a6-73c4-4488-bef8-2f23dd41266c}</MetaDataID>
    public class UnLinkObjectsCommand : PersistenceLayerRunTime.Commands.UnLinkObjectsCommand
    {
        /// <MetaDataID>{40c7d25a-10fd-4391-9564-4450e6ea3a19}</MetaDataID>
        public UnLinkObjectsCommand(PersistenceLayerRunTime.ObjectStorage objectStorage, PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
            : base(objectStorage, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index)
        {

        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{297E7CB4-82AB-41B9-8490-4137550E2736}</MetaDataID>
        private RDBMSMetaDataRepository.AssociationEnd _OwnerAssociationEnd;
        /// <MetaDataID>{872B8D3C-40F1-41A9-BA06-A1D02A3CE974}</MetaDataID>
        new private RDBMSMetaDataRepository.AssociationEnd OwnerAssociationEnd
        {
            get
            {
                if (_OwnerAssociationEnd == null)
                    _OwnerAssociationEnd = (ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(LinkInitiatorAssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                return _OwnerAssociationEnd;
            }
        }

        /// <MetaDataID>{07430BD8-A10A-475A-87CD-4907C7D0BD41}</MetaDataID>
        private void UnlinkOnetoManyOneToOne(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
        {

            #region Precondition check
            if (mAssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany || mAssociationEnd.Association.LinkClass != null)
                throw new System.Exception("It Can’t  link many to many relationship");
            #endregion



            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();

            RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink = (mAssociationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLink(((RDBMSMetaDataRepository.StorageCell)RoleA.RealStorageInstanceRef.StorageInstanceSet), ((RDBMSMetaDataRepository.StorageCell)RoleB.RealStorageInstanceRef.StorageInstanceSet), vaueTypePath);
            RDBMSMetaDataRepository.AssociationEnd associationEnd = objectCollectionsLink.GetAssociationEndWithReferenceColumns();
            StorageInstanceRef RecordOwnerObject = null;
            StorageInstanceRef AssignedObject = null;
            RDBMSMetaDataRepository.Table KeepColumnTable = null;

            #region Gets metadata informations

            if (associationEnd.IsRoleA)
            {
                RecordOwnerObject = RoleA.RealStorageInstanceRef as StorageInstanceRef;
                AssignedObject = RoleB.RealStorageInstanceRef as StorageInstanceRef;
            }
            else
            {
                RecordOwnerObject = RoleB.RealStorageInstanceRef as StorageInstanceRef;
                AssignedObject = RoleA.RealStorageInstanceRef as StorageInstanceRef;
            }

            //TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class


            IList<RDBMSMetaDataRepository.IdentityColumn> linkColumns = associationEnd.GetReferenceColumnsFor(RecordOwnerObject.StorageInstanceSet, vaueTypePath);



            #endregion



            int RowAffected;
            #region Open database connection and execute the command

            /// replace reference columns values with null only if referred to removed assigned object
            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(RecordOwnerObject, column.Name, DBNull.Value);
                if(RecordOwnerObject.RelationshipColumnsValues[column] != ((OOAdvantech.PersistenceLayer.ObjectID)AssignedObject.PersistentObjectID).GetMemberValue(column.ColumnType))
                {
                    return;
                }
            }


            if (associationEnd.Indexer)
            {
                //PersistenceLayerRunTime.StorageInstanceRef collectionOwner = null;
                //if (associationEnd.IsRoleA)
                //    collectionOwner = RoleB.RealStorageInstanceRef;
                //else
                //    collectionOwner = RoleA.RealStorageInstanceRef;


                //int index = -1;

                //string transactionUri = Transactions.Transaction.Current.LocalTransactionUri;
                //if (LinkObjectsCommand.Indexer.ContainsKey(transactionUri) && LinkObjectsCommand.Indexer[transactionUri].ContainsKey(collectionOwner) && LinkObjectsCommand.Indexer[transactionUri][collectionOwner].ContainsKey(associationEnd))
                //    LinkObjectsCommand.Indexer[transactionUri][collectionOwner][associationEnd].LastIndex = -1;
                //{

                //foreach (RDBMSMetaDataRepository.StorageCell storageCell in AssignedObject.StorageInstanceSet.GetLinkedStorageCells(associationEnd.Association.Identity, associationEnd.Role))
                //{

                //    Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> storageCellLinkColumns = associationEnd.GetReferenceColumnsFor(storageCell);
                //    RDBMSMetaDataRepository.Table table = storageCellLinkColumns[0].Namespace as RDBMSMetaDataRepository.Table;

                //    string countCalculation = ((RecordOwnerObject.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.RDBMSSchema.RebuildItemsIndexInTableStatament(AssignedObject, table, storageCellLinkColumns, index, associationEnd.IndexerColumn);
                //    System.Data.Common.DbCommand oleDbCommand = oleDbConnection.CreateCommand();
                //    oleDbCommand.CommandText = countCalculation;
                //    oleDbCommand.ExecuteNonQuery();
                //}
                //}


                //Indexer[transactionUri][collectionOwner][associationEnd].LastIndex = Indexer[transactionUri][collectionOwner][associationEnd].LastIndex + 1;
                //foreach (IndexerItem item in Indexer[transactionUri][collectionOwner][associationEnd].Items)
                //{
                //    if (item.index >= index)
                //    {
                //        item.index++;
                //        Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(item.StorageInstanceRef, associationEnd.IndexerColumn.Name, item.index);
                //    }
                //}

                //if (associationEnd.IsRoleA)
                //{
                //    RoleAIndex = index;
                //    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(RecordOwnerObject, associationEnd.IndexerColumn.Name, index);
                //}
                //else
                //{
                //    RoleBIndex = index;
                //    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(RecordOwnerObject, associationEnd.IndexerColumn.Name, index);
                //}
                //IndexerItem indexerItem = new IndexerItem();
                //indexerItem.StorageInstanceRef = RecordOwnerObject;
                //indexerItem.index = index;
                //Indexer[transactionUri][collectionOwner][associationEnd].Items.Add(indexerItem);
                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(RecordOwnerObject, associationEnd.GetIndexerColumnFor(RecordOwnerObject.StorageInstanceSet, vaueTypePath).DataBaseColumnName, DBNull.Value);
                RecordOwnerObject.RelationshipColumnsValues[associationEnd.GetIndexerColumnFor(RecordOwnerObject.StorageInstanceSet, vaueTypePath)] = DBNull.Value;

            }
            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(RecordOwnerObject, column.Name, DBNull.Value);
                RecordOwnerObject.RelationshipColumnsValues[column] = DBNull.Value;
            }

            #endregion

            #region Postcondition check
            //if (RowAffected == 0)
            //{
            //    oleDbCommand = oleDbConnection.CreateCommand();
            //    oleDbCommand.CommandText = "SELECT count(*) FROM " + KeepColumnTable.Name + " " + WhereClause;
            //    int Rows = (int)oleDbCommand.ExecuteNonQuery();
            //    if (Rows > 0)
            //        throw new System.Exception("Multiplicity constraint mismatch at association '" + associationEnd.Association.Name + "' on object " + RecordOwnerObject.MemoryInstance.ToString());
            //}
            #endregion



        }
        /// <MetaDataID>{8AC4B73B-2664-4367-B3B8-8819091E4234}</MetaDataID>
        private void UnlinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
        {

            RDBMSMetaDataRepository.Association association = (RDBMSMetaDataRepository.Association)mAssociationEnd.Association;

            bool setRoleAIndex = false;
            bool setRoleBIndex = false;
            if (association.RoleA.Indexer)
            {
                setRoleAIndex = true;
                throw new NotImplementedException();
            }

            if (association.RoleB.Indexer)
            {
                setRoleBIndex = true;
                throw new NotImplementedException();
            }


            #region Gets meta data informations

            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();

            RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink = association.GetStorageCellsLink(((RDBMSMetaDataRepository.StorageCell)RoleA.RealStorageInstanceRef.StorageInstanceSet), ((RDBMSMetaDataRepository.StorageCell)RoleB.RealStorageInstanceRef.StorageInstanceSet), vaueTypePath);
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
            #endregion


            string cloudTableName = objectCollectionsLink.ObjectLinksTable.Name;
            var account = (ObjectStorage as ObjectStorage).Account;
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable azureTable = tableClient.GetTableReference(cloudTableName);

            string filterScript = null;

            if (azureTable.Exists())
            {

                IList<string> selectionColumns = new List<string>();

                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
                {
                    if (filterScript != null)
                        filterScript += " and ";

                    filterScript += " " + (this.ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor(column.Name, "eq", (RoleB.PersistentObjectID as ObjectID).GetMemberValue(column.ColumnType));
                    selectionColumns.Add(column.Name);
                }

                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
                {
                    if (filterScript != null)
                        filterScript += " and ";

                    filterScript += " " + (this.ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor(column.Name, "eq", (RoleA.PersistentObjectID as ObjectID).GetMemberValue(column.ColumnType));

                    selectionColumns.Add(column.Name);
                }
                var query = new TableQuery<ElasticTableEntity>();
                List<ElasticTableEntity> entities = azureTable.ExecuteQuery(query.Select(selectionColumns).Where(filterScript)).ToList();

                foreach (var entity in entities)
                {
                    (this.ObjectStorage as ObjectStorage).GetTableBatchOperation(azureTable).Delete(entity);
                }
            }

            return;

            //#region Build SQL command
            //string DeleteQuery = "";

            //string indexRoleBCriterion = null;
            //string indexRoleACriterion = null;

            //bool setRoleAIndex = false;
            //bool setRoleBIndex = false;
            //if (association.RoleA.Indexer)
            //    setRoleAIndex = true;

            //if (association.RoleB.Indexer)
            //    setRoleBIndex = true;

            //string getRoleAIndex = @"declare @roleAIndex int;
            //                       select @roleAIndex=[IndexerColumn] 
            //                       from [AssociationTable] 
            //                       WHERE ([ExistCriterion])";

            //string getRoleBIndex = @"declare @roleBIndex int;
            //                       select @roleBIndex=[IndexerColumn] 
            //                       from [AssociationTable] 
            //                       WHERE ([ExistCriterion])";


            //string roleBUpdateRelationIndex = @"UPDATE [AssociationTable]
            //                                SET     [IndexerColumn] = [IndexerColumn] - 1
            //                                WHERE  ([IndexerColumn] >= @roleBIndex and [ListOwnerCriterion])";
            //string roleAUpdateRelationIndex = @"UPDATE [AssociationTable]
            //                                SET     [IndexerColumn] = [IndexerColumn] - 1
            //                                WHERE  ([IndexerColumn] >= @roleAIndex and [ListOwnerCriterion])";


            //OOAdvantech.RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary = (RoleA.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.TypeDictionary;


            //DeleteQuery += "DELETE FROM [AssociationTable] " +
            //            "WHERE ([ExistCriterion])";

            //string existCriterion = null;

            //foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
            //{
            //    if (existCriterion != null)
            //        existCriterion += " AND ";
            //    string column_value = TypeDictionary.ConvertToSQLString((RoleB.PersistentObjectID as ObjectID).GetMemberValue(column.ColumnType));
            //    existCriterion += " " + column.Name + " = " + column_value;

            //    if (setRoleAIndex)
            //    {
            //        if (indexRoleACriterion != null)
            //            indexRoleACriterion += " AND ";
            //        column_value = TypeDictionary.ConvertToSQLString((RoleB.PersistentObjectID as ObjectID).GetMemberValue(column.ColumnType));
            //        indexRoleACriterion += " " + column.Name + " = " + column_value;
            //    }
            //}


            //foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
            //{
            //    if (existCriterion != null)
            //        existCriterion += " AND ";
            //    string column_value = TypeDictionary.ConvertToSQLString((RoleA.PersistentObjectID as ObjectID).GetMemberValue(column.ColumnType));
            //    existCriterion += " " + column.Name + " = " + column_value;

            //    if (setRoleBIndex)
            //    {
            //        if (indexRoleBCriterion != null)
            //            indexRoleBCriterion += " AND ";
            //        column_value = TypeDictionary.ConvertToSQLString((RoleB.PersistentObjectID as ObjectID).GetMemberValue(column.ColumnType));
            //        indexRoleBCriterion += " " + column.Name + " = " + column_value;
            //    }
            //}


            //DeleteQuery = DeleteQuery.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
            //DeleteQuery = DeleteQuery.Replace("[ExistCriterion]", existCriterion);


            //if (setRoleAIndex)
            //{
            //    getRoleAIndex = getRoleAIndex.Replace("[ExistCriterion]", existCriterion);
            //    getRoleAIndex = getRoleAIndex.Replace("[IndexerColumn]", (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable).DataBaseColumnName);
            //    getRoleAIndex = getRoleAIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
            //    roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
            //    roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[IndexerColumn]", (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable).DataBaseColumnName);
            //    roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[ListOwnerCriterion]", indexRoleACriterion);
            //    DeleteQuery = getRoleAIndex + "\n" + roleAUpdateRelationIndex + "\n" + DeleteQuery;
            //}

            //if (setRoleBIndex)
            //{

            //    getRoleBIndex = getRoleBIndex.Replace("[ExistCriterion]", existCriterion);
            //    getRoleBIndex = getRoleBIndex.Replace("[IndexerColumn]", (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable).DataBaseColumnName);
            //    getRoleBIndex = getRoleBIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
            //    roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
            //    roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[IndexerColumn]", (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(objectCollectionsLink.ObjectLinksTable).DataBaseColumnName);
            //    roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[ListOwnerCriterion]", indexRoleBCriterion);
            //    DeleteQuery = getRoleBIndex + "\n" + roleAUpdateRelationIndex + "\n" + DeleteQuery;
            //}
            //#endregion

            //#region Open database connection and execute the command
            //var oleDbConnection = (RoleA.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;

            //if (oleDbConnection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
            //    oleDbConnection.Open();
            ////oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);

            ////			oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            //var oleDbCommand = oleDbConnection.CreateCommand();
            //oleDbCommand.CommandText = DeleteQuery;
            //object Result = oleDbCommand.ExecuteNonQuery();
            //#endregion
        }


        /// <MetaDataID>{DF0A40A3-E351-40B4-8271-78874E2ADA0F}</MetaDataID>
        public override void Execute()
        {

            #region Preconditions Chechk
            if (RoleA == null || RoleB == null)
                throw new System.Exception("You must set the objects that will be linked before the execution of command.");//Message
            #endregion

            base.Execute();
            if (MetaDataRepository.AssociationType.ManyToMany != OwnerAssociationEnd.Association.MultiplicityType && OwnerAssociationEnd.Association.LinkClass == null)
                UnlinkOnetoManyOneToOne(OwnerAssociationEnd);
            else
                UnlinkManyToMany(OwnerAssociationEnd);




        }
    }

}
