using System;
using System.Collections.Generic;

using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime.Commands
{
    /// <MetaDataID>{beac7e33-bc1c-4639-9ee2-094d44da66bf}</MetaDataID>
    public class BuildContainedObjectIndicies : PersistenceLayerRunTime.Commands.BuildContainedObjectIndicies
    {
     
        /// <MetaDataID>{112576b1-b784-48c4-871c-32c3bc39bdbf}</MetaDataID>
        public BuildContainedObjectIndicies(PersistenceLayerRunTime.IndexedCollection collection)
            : base(collection)
        {


        }
        /// <MetaDataID>{6533e648-d04b-4293-8ea8-3f3c78594277}</MetaDataID>
       
        public override void Execute()
        {
            List<OOAdvantech.PersistenceLayerRunTime.GroupIndexChange> groupIndexChanges = new List<OOAdvantech.PersistenceLayerRunTime.GroupIndexChange>();

            var connection = (Collection.RelResolver.Owner.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                connection.Open();

            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Collection.RelResolver.Owner.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(Collection.RelResolver.AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            associationEnd = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
            groupIndexChanges = Collection.GetIndexChanges(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);
            //indexChanges.Sort(new Comparison<PersistenceLayerRunTime.IndexChange>(PersistenceLayerRunTime.IndexChange.Compare));

            //OOAdvantech.PersistenceLayerRunTime.GroupIndexChange groupIndexChange = null;
            //int change = 0;
            //foreach (PersistenceLayerRunTime.IndexChange indexChange in indexChanges)
            //{
            //    if (indexChange.OldIndex == -1)
            //        continue;
            //    if (groupIndexChange == null || change != indexChange.NewIndex - indexChange.OldIndex)
            //    {
            //        if (groupIndexChange != null)
            //            groupIndexChanges.Add(groupIndexChange);
            //        groupIndexChange = new GroupIndexChange();
            //        groupIndexChange.StartIndex = indexChange.OldIndex;
            //        groupIndexChange.EndIndex = indexChange.OldIndex;
            //        change = indexChange.NewIndex - indexChange.OldIndex;
            //        groupIndexChange.Change = change;
            //    }
            //    if (groupIndexChange != null)
            //        groupIndexChange.EndIndex = indexChange.OldIndex;

            //}
            //if (groupIndexChange != null)
            //    groupIndexChanges.Add(groupIndexChange);

            ////(collectionOwner.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell)
            //groupIndexChanges.Sort(new Comparison<GroupIndexChange>(BuildContainedObjectIndicies.Descend));

            foreach (OOAdvantech.PersistenceLayerRunTime.GroupIndexChange currentGroupIndexChange in groupIndexChanges)
            {
                StorageInstanceRef collectionOwner = Collection.RelResolver.Owner as StorageInstanceRef;
                foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in (associationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLinks((collectionOwner.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell), associationEnd.Role))
                {

                    System.Collections.Generic.IList<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> storageCellLinkColumns = null;
                    RDBMSMetaDataRepository.Column indexerColumn = null;

                    if (storageCellsLink.ObjectLinksTable == null)
                    {
                        RDBMSMetaDataRepository.AssociationEnd referenceColumnsAssociationEnd = storageCellsLink.GetAssociationEndWithReferenceColumns();
                        if (referenceColumnsAssociationEnd.IsRoleA)
                        {
                            storageCellLinkColumns = referenceColumnsAssociationEnd.GetReferenceColumnsFor(storageCellsLink.RoleAStorageCell);
                            indexerColumn = referenceColumnsAssociationEnd.GetIndexerColumnFor(storageCellsLink.RoleAStorageCell);
                        }
                        else
                        {
                            storageCellLinkColumns = referenceColumnsAssociationEnd.GetReferenceColumnsFor(storageCellsLink.RoleBStorageCell, Collection.RelResolver.ValueTypePath.ToString());
                            indexerColumn = referenceColumnsAssociationEnd.GetIndexerColumnFor(storageCellsLink.RoleBStorageCell, Collection.RelResolver.ValueTypePath.ToString());
                        }
                    }
                    else
                    {
                        storageCellLinkColumns = associationEnd.GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable, Collection.RelResolver.ValueTypePath.ToString());
                        indexerColumn = associationEnd.GetIndexerColumnFor(storageCellsLink.ObjectLinksTable, Collection.RelResolver.ValueTypePath.ToString());
                    }

                    RDBMSMetaDataRepository.Table table = storageCellLinkColumns[0].Namespace as RDBMSMetaDataRepository.Table;
                    string rebuildItemsIndexStatament = (collectionOwner.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.RDBMSSQLScriptGenarator.RebuildItemsIndexInTableStatament(collectionOwner, table, storageCellLinkColumns, indexerColumn, currentGroupIndexChange.StartIndex, currentGroupIndexChange.EndIndex, currentGroupIndexChange.Change);
                    var oleDbCommand = connection.CreateCommand();
                    oleDbCommand.CommandText = rebuildItemsIndexStatament;
                    int affectedRows = oleDbCommand.ExecuteNonQuery();

                }
            }

        }

    }
}
