using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime.Commands
{
    /// <MetaDataID>{beac7e33-bc1c-4639-9ee2-094d44da66bf}</MetaDataID>
    public class BuildContainedObjectIndicies : PersistenceLayerRunTime.Commands.BuildContainedObjectIndicies
    {
        class GroupIndexChange
        {
            public int StartIndex;
            public int EndIndex;
            public int Change;

        }
        /// <MetaDataID>{112576b1-b784-48c4-871c-32c3bc39bdbf}</MetaDataID>
        public BuildContainedObjectIndicies(PersistenceLayerRunTime.IndexedCollection collection)
            : base(collection)
        {


        }
        /// <MetaDataID>{6533e648-d04b-4293-8ea8-3f3c78594277}</MetaDataID>
        static int Descend(GroupIndexChange x, GroupIndexChange y)
        {
            return y.StartIndex.CompareTo(x.StartIndex);
        }
        public override void Execute()
        {
            List<GroupIndexChange > groupIndexChanges=new List<GroupIndexChange>(); 
            
            System.Data.Common.DbConnection oleDbConnection = ((Collection.RelResolver.Owner.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();

            RDBMSMetaDataRepository.AssociationEnd associationEnd = Collection.RelResolver.AssociationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
            associationEnd=(associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
            List<OOAdvantech.PersistenceLayerRunTime.IndexChange> indexChanges = Collection.GetIndexChanges(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);
            indexChanges.Sort(new Comparison<PersistenceLayerRunTime.IndexChange>(PersistenceLayerRunTime.IndexChange.Compare));

            GroupIndexChange groupIndexChange = null;
            int change = 0;
            foreach (PersistenceLayerRunTime.IndexChange indexChange in indexChanges)
            {
                if (indexChange.OldIndex == -1)
                    continue;
                if (groupIndexChange == null || change != indexChange.NewIndex - indexChange.OldIndex)
                {
                    if (groupIndexChange != null)
                        groupIndexChanges.Add(groupIndexChange);
                    groupIndexChange = new GroupIndexChange();
                    groupIndexChange.StartIndex = indexChange.OldIndex;
                    groupIndexChange.EndIndex = indexChange.OldIndex;
                    change = indexChange.NewIndex - indexChange.OldIndex;
                    groupIndexChange.Change = change;
                }
                if (groupIndexChange != null)
                    groupIndexChange.EndIndex = indexChange.OldIndex;

            }
            if (groupIndexChange != null)
                groupIndexChanges.Add(groupIndexChange);


            groupIndexChanges.Sort(new Comparison<GroupIndexChange>(BuildContainedObjectIndicies.Descend)); 

            foreach (GroupIndexChange currentGroupIndexChange in groupIndexChanges)
            {
                StorageInstanceRef collectionOwner = Collection.RelResolver.Owner as StorageInstanceRef;
                foreach (RDBMSMetaDataRepository.StorageCell storageCell in collectionOwner.StorageInstanceSet.GetLinkedStorageCells(associationEnd.Association.Identity, associationEnd.Role))
                {

                    Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> storageCellLinkColumns = associationEnd.GetReferenceColumnsFor(storageCell);
                    RDBMSMetaDataRepository.Table table = storageCellLinkColumns[0].Namespace as RDBMSMetaDataRepository.Table;

                    string rebuildItemsIndexStatament = ((collectionOwner.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.RDBMSSchema.RebuildItemsIndexInTableStatament(collectionOwner, table, storageCellLinkColumns, associationEnd.IndexerColumn, currentGroupIndexChange.StartIndex, currentGroupIndexChange.EndIndex, currentGroupIndexChange.Change);
                    System.Data.Common.DbCommand oleDbCommand = oleDbConnection.CreateCommand();
                    oleDbCommand.CommandText = rebuildItemsIndexStatament;
                    int affectedRows= oleDbCommand.ExecuteNonQuery();
                }
            }
         
        }

    }
}
