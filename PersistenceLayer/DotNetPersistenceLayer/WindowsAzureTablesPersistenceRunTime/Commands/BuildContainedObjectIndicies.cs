using OOAdvantech.PersistenceLayerRunTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{
    /// <MetaDataID>{beac7e33-bc1c-4639-9ee2-094d44da66bf}</MetaDataID>
    public class BuildContainedObjectIndicies : PersistenceLayerRunTime.Commands.BuildContainedObjectIndicies
    {

        /// <MetaDataID>{112576b1-b784-48c4-871c-32c3bc39bdbf}</MetaDataID>
        public BuildContainedObjectIndicies(PersistenceLayerRunTime.IndexedCollection collection)
            : base(collection)
        {
            //saa sokar

        }
        /// <MetaDataID>{6533e648-d04b-4293-8ea8-3f3c78594277}</MetaDataID>

        public override void Execute()
        {



            List<OOAdvantech.PersistenceLayerRunTime.GroupIndexChange> groupIndexChanges = new List<OOAdvantech.PersistenceLayerRunTime.GroupIndexChange>();


            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Collection.RelResolver.Owner.ObjectStorage.StorageMetaData as Storage).GetEquivalentMetaObject(Collection.RelResolver.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            associationEnd = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
            groupIndexChanges = Collection.GetIndexChanges(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);
            System.Collections.Generic.List<IndexChange> indexesOfNewRelatedObject = Collection.GetTheIndexesOfAdditionalObject(Transactions.Transaction.Current.LocalTransactionUri);

            foreach (OOAdvantech.PersistenceLayerRunTime.GroupIndexChange currentGroupIndexChange in groupIndexChanges)
            {
                StorageInstanceRef collectionOwner = Collection.RelResolver.Owner as StorageInstanceRef;
                //var account = (collectionOwner.ObjectStorage as ObjectStorage).Account;
                //CloudTableClient tableClient = account.CreateCloudTableClient();

                foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in (associationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLinks((collectionOwner.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell), associationEnd.Role))
                {
                    string partitionKey = null;
                    List<RDBMSMetaDataRepository.IdentityColumn> objectIDColumns = null;

                    System.Collections.Generic.IList<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> storageCellLinkColumns = null;
                    RDBMSMetaDataRepository.Column indexerColumn = null;

                    if (storageCellsLink.ObjectLinksTable == null)
                    {
                        RDBMSMetaDataRepository.AssociationEnd referenceColumnsAssociationEnd = storageCellsLink.GetAssociationEndWithReferenceColumns();
                        if (referenceColumnsAssociationEnd.IsRoleA)
                        {
                            storageCellLinkColumns = referenceColumnsAssociationEnd.GetReferenceColumnsFor(storageCellsLink.RoleAStorageCell);
                            indexerColumn = referenceColumnsAssociationEnd.GetIndexerColumnFor(storageCellsLink.RoleAStorageCell);
                            partitionKey = storageCellsLink.RoleAStorageCell.SerialNumber.ToString();
                            objectIDColumns = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns.ToList();
                        }
                        else
                        {
                            storageCellLinkColumns = referenceColumnsAssociationEnd.GetReferenceColumnsFor(storageCellsLink.RoleBStorageCell, Collection.RelResolver.ValueTypePath.ToString());
                            indexerColumn = referenceColumnsAssociationEnd.GetIndexerColumnFor(storageCellsLink.RoleBStorageCell, Collection.RelResolver.ValueTypePath.ToString());
                            partitionKey = storageCellsLink.RoleBStorageCell.SerialNumber.ToString();
                            objectIDColumns = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns.ToList();
                        }
                    }
                    else
                    {
                        storageCellLinkColumns = associationEnd.GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable, Collection.RelResolver.ValueTypePath.ToString());
                        indexerColumn = associationEnd.GetIndexerColumnFor(storageCellsLink.ObjectLinksTable, Collection.RelResolver.ValueTypePath.ToString());
                        partitionKey = storageCellsLink.RoleAStorageCell.SerialNumber.ToString() + "L" + storageCellsLink.RoleBStorageCell.SerialNumber.ToString();

                    }

                    string filterScript = (collectionOwner.ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor("PartitionKey", "eq", partitionKey);
                    int i = 0;
                    foreach (RDBMSMetaDataRepository.IdentityColumn objectIDColumn in storageCellLinkColumns)
                    {
                        collectionOwner.ObjectID.GetPartValue(i);
                        filterScript += " and " + (collectionOwner.ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor(objectIDColumn.DataBaseColumnName, "eq", collectionOwner.ObjectID.GetPartValue(i));
                        i++;
                    }
                    var startIndex = currentGroupIndexChange.StartIndex;
                    var endIndex = currentGroupIndexChange.EndIndex;
                    var change = currentGroupIndexChange.Change;


                    filterScript += " and " + (collectionOwner.ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor(indexerColumn.DataBaseColumnName, "ge", startIndex);
                    filterScript += " and " + (collectionOwner.ObjectStorage as ObjectStorage).TypeDictionary.GenerateFilterConditionFor(indexerColumn.DataBaseColumnName, "le", endIndex);
                    RDBMSMetaDataRepository.Table table = storageCellLinkColumns[0].Namespace as RDBMSMetaDataRepository.Table;
                    System.Collections.Generic.IList<string> selectionColumns = new List<string>();
                    foreach (var column in table.ContainedColumns)
                    {
                        if (!selectionColumns.Contains(column.DataBaseColumnName))
                            selectionColumns.Add(column.DataBaseColumnName);
                    }

                    if (!selectionColumns.Contains("RowKey"))
                        selectionColumns.Add("RowKey");
                    if (!selectionColumns.Contains("PartitionKey"))
                        selectionColumns.Add("PartitionKey");



                    //CloudTable azureTable = tableClient.GetTableReference(table.DataBaseTableName);
                    if((collectionOwner.ObjectStorage as ObjectStorage).TableExist(table.DataBaseTableName))
                    //if (azureTable.Exists())
                    {

                        Azure.Data.Tables.TableClient azureTable_a = (collectionOwner.ObjectStorage as ObjectStorage).TablesAccount.GetTableClient(table.DataBaseTableName);
                        
                        //var query = new TableQuery<ElasticTableEntity>();
                        //var entities = azureTable.ExecuteQuery(query.Select(selectionColumns).Where(filterScript)).ToList();
                        var entities = azureTable_a.Query<Azure.Data.Tables.TableEntity>(filterScript,null ,selectionColumns).Select(x=>new ElasticTableEntity(x)).ToList();
                        foreach (ElasticTableEntity entity in entities)
                        {
                            int? sortIndex = null;
                            IndexChange indexChangeForNewObjectLink = indexesOfNewRelatedObject.Where(x => x.CollectionItem?.PersistentObjectID.ToString() == entity.RowKey).FirstOrDefault();

                            if (indexChangeForNewObjectLink != null && indexChangeForNewObjectLink.OldIndex == -1)
                            {
                                sortIndex=indexChangeForNewObjectLink.NewIndex;
                            }
                            else
                            {
                                sortIndex = entity.Properties[indexerColumn.DataBaseColumnName].Int32Value;
                                sortIndex += change;
                            }

                            StorageInstanceRef recordOwnerObject = null;
                            if (storageCellsLink.ObjectLinksTable == null)
                                recordOwnerObject = StorageInstanceRef.GetStorageInstanceRef(table, entity.PartitionKey, entity.RowKey);
                            if (recordOwnerObject != null)
                                Commands.UpdateDatabaseMassively.CurrentTransactionCommandUpdateMassively.SetStorageInstanceColumnValue(recordOwnerObject, indexerColumn.DataBaseColumnName, sortIndex);
                            else
                            {

                                entity[indexerColumn.DataBaseColumnName] = sortIndex;

                                var tableBatchOperation = (collectionOwner.ObjectStorage as ObjectStorage).GetTableBatchOperation(azureTable_a);
                                tableBatchOperation.Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.UpdateReplace, entity.Values));
                                
                                //(collectionOwner.ObjectStorage as ObjectStorage).GetTableBatchOperation(azureTable_a).Replace(entity);
                            }

                        }
                    }
                }
            }
            Collection.IndexRebuilded(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);

        }

    }
}
