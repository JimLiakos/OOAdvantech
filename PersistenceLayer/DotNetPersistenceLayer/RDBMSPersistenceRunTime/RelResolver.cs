using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime
{
    /// <MetaDataID>{75493f79-aa5f-410c-b13a-dfee12289d8e}</MetaDataID>
    public class RelResolver : PersistenceLayerRunTime.RelResolver
    {

        /// <MetaDataID>{f3191ca0-b03c-44f2-b5fe-ebda83c20d7d}</MetaDataID>
        public RelResolver(PersistenceLayerRunTime.StorageInstanceRef theOwner, DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
            : base(theOwner, associationEnd, fastFieldAccessor)
        {
        }

        public override int IndexOf(object memoryInstance)
        {
            if (IsCompleteLoaded)
                return base.IndexOf(memoryInstance);
            RDBMSMetaDataRepository.AssociationEnd associationEnd = (Owner.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            //var referenceColumnsAssociationEnd = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
            RDBMSMetaDataRepository.StorageCellsLink storageCellsLink = null;
            var objectStorage=Owner.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
             PersistenceLayerRunTime.StorageInstanceAgent roleA = null;
             PersistenceLayerRunTime.StorageInstanceAgent roleB = null;
             System.Collections.Generic.IList<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> storageCellLinkColumns = null;
             System.Collections.Generic.IList<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> ownerObjectIDColumns = null;
             RDBMSMetaDataRepository.Column indexerColumn = null;
            PersistenceLayerRunTime.StorageInstanceAgent  indexOfObject=null;
            if (OwnerStorageInstanceAgent == null)
                OwnerStorageInstanceAgent = new OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent(Owner);
            if (AssociationEnd.IsRoleA)
            {
                roleA = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(memoryInstance);
                roleB = OwnerStorageInstanceAgent;
                indexOfObject=roleA;
            }
            else
            {
                roleA = OwnerStorageInstanceAgent;
                roleB = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(memoryInstance);
                indexOfObject=roleB;
            }
            storageCellsLink = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLink(objectStorage.GetStorageCell(roleA), objectStorage.GetStorageCell(roleB), ValueTypePath.ToString(), false);
            if (storageCellsLink.ObjectLinksTable == null)
            {
                RDBMSMetaDataRepository.AssociationEnd referenceColumnsAssociationEnd = storageCellsLink.GetAssociationEndWithReferenceColumns();
                if (referenceColumnsAssociationEnd.IsRoleA)
                {
                    storageCellLinkColumns = referenceColumnsAssociationEnd.GetReferenceColumnsFor(storageCellsLink.RoleAStorageCell);
                    indexerColumn = referenceColumnsAssociationEnd.GetIndexerColumnFor(storageCellsLink.RoleAStorageCell);
                    ownerObjectIDColumns = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns;
                }
                else
                {
                    storageCellLinkColumns = referenceColumnsAssociationEnd.GetReferenceColumnsFor(storageCellsLink.RoleBStorageCell, ValueTypePath.ToString());
                    indexerColumn = referenceColumnsAssociationEnd.GetIndexerColumnFor(storageCellsLink.RoleBStorageCell, ValueTypePath.ToString());
                    ownerObjectIDColumns = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns;
                }
            }
            else
            {
                storageCellLinkColumns = associationEnd.GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable, ValueTypePath.ToString());
                ownerObjectIDColumns = (associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable);
                indexerColumn = associationEnd.GetIndexerColumnFor(storageCellsLink.ObjectLinksTable, ValueTypePath.ToString());
            }
            RDBMSMetaDataRepository.Table table = storageCellLinkColumns[0].Namespace as RDBMSMetaDataRepository.Table;
            string indexOfSqlScript = (Owner.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.RDBMSSQLScriptGenarator.IndexOfSqlScript(OwnerStorageInstanceAgent, indexOfObject, table, ownerObjectIDColumns, storageCellLinkColumns, indexerColumn);
            var connection = (Owner.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            if (connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = indexOfSqlScript;
            object indexValue = command.ExecuteScalar();
            if (indexValue is int || indexValue is short || indexValue is long || indexValue is uint || indexValue is ushort || indexValue is ulong)
                return (int)indexValue;

            else
                return -1;

        }
        /// <MetaDataID>{6b4b3f83-a4ce-4e46-a105-1b93211a698d}</MetaDataID>
        public override System.Collections.Generic.List<object> GetLinkedStorageInstanceRefs(bool OperativeObjectOnly)
        {
            System.Collections.Generic.List<object> Objects = null;
            if (IsCompleteLoaded)
            {
                if (AssociationEnd.Multiplicity.IsMany)
                    Objects = InternalLoadedRelatedObjects;
                else
                {
                    Objects = new OOAdvantech.Collections.Generic.List<object>();
                    if (RelatedObject != null)
                        Objects.Add(RelatedObject);
                }
            }
            else
                Objects = GetLinkedObjects("");
            System.Collections.Generic.List<object> StorageInstanceRefs = new System.Collections.Generic.List<object>(Objects.Count);
            foreach (object _objcet in Objects)
                StorageInstanceRefs.Add(StorageInstanceRef.GetStorageInstanceRef(_objcet));
            return StorageInstanceRefs;

        }

    }
}
