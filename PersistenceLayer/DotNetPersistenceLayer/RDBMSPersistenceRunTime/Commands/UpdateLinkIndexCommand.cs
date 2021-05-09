using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime.Commands
{
    /// <MetaDataID>{0a0738cf-b065-4a5c-9d55-6f4936f96a05}</MetaDataID>
   public class UpdateLinkIndexCommand :PersistenceLayerRunTime.Commands.UpdateLinkIndexCommand
    {
       public UpdateLinkIndexCommand(ObjectStorage objectStorage, PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent  roleB, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index) :
           base(objectStorage, roleA, roleB, linkInitiatorAssociationEnd, index)
       {

       }
        public override void GetSubCommands(int currentExecutionOrder)
        {
            
        }
        new private RDBMSMetaDataRepository.AssociationEnd LinkInitiatorAssociationEnd
        {
            get
            {

                if (!(base.LinkInitiatorAssociationEnd is RDBMSMetaDataRepository.AssociationEnd))
                    base.LinkInitiatorAssociationEnd = (ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(base.LinkInitiatorAssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                return base.LinkInitiatorAssociationEnd as RDBMSMetaDataRepository.AssociationEnd;
            }
        }
        RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink = null;
        string vaueTypePath;
        /// <MetaDataID>{502555A3-C60E-44DA-B601-E3B381AA486D}</MetaDataID>
        public override void Execute()
        {
            RDBMSMetaDataRepository.Association Association = LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association;
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();

            #region Preconditions Chechk
            if (RoleA == null || RoleB == null)
                throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
            #endregion

            objectCollectionsLink = Association.GetStorageCellsLink(ObjectStorage.GetStorageCell(RoleA), ObjectStorage.GetStorageCell(RoleB), vaueTypePath, true);
            if (ObjectStorage.GetStorageCell(RoleA).ObjectIdentityType != ObjectStorage.GetStorageCell(RoleB).ObjectIdentityType)
                UpdateManyToMany(LinkInitiatorAssociationEnd);
            else if (MetaDataRepository.AssociationType.ManyToMany == LinkInitiatorAssociationEnd.Association.MultiplicityType ||
                LinkInitiatorAssociationEnd.Association.LinkClass != null ||
                RoleA.ObjectStorage != RoleB.ObjectStorage ||
                objectCollectionsLink.ObjectLinksTable != null)
                UpdateManyToMany(LinkInitiatorAssociationEnd);
            else
                UpdateOnetoManyOneToOne(LinkInitiatorAssociationEnd);

        }
        private void UpdateManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
        {


            RDBMSMetaDataRepository.Association association = (RDBMSMetaDataRepository.Association)mAssociationEnd.Association;


            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns = null;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns = null;

            #region Gets metadata informations

                roleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
                roleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);

            
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> relationObjectIDColumns = null;
            #endregion

            if (association.LinkClass == null || objectCollectionsLink.ObjectLinksTable != null)
            {
                Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleBColumnsValues = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object>();
                Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleAColumnsValues = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object>();

                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
                    roleAColumnsValues[column] = (RoleB.PersistentObjectID as OOAdvantech.PersistenceLayer.ObjectID).GetMemberValue(column.ColumnType);
                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
                    roleBColumnsValues[column] = (RoleA.PersistentObjectID as OOAdvantech.PersistenceLayer.ObjectID).GetMemberValue(column.ColumnType);

                //if (RelationObject != null)
                //    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.AddRelationRow(objectCollectionsLink, RelationObject.PersistentObjectID as PersistenceLayer.ObjectID, roleAColumnsValues, RoleAIndex, roleBColumnsValues, RoleBIndex);
                //else


                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetRelationRow(objectCollectionsLink, null, roleAColumnsValues, RoleAIndex, roleBColumnsValues, RoleBIndex);
                //objectCollectionsLink.ObjectsLinksCount++;
                return;
            }
            //else
            //{
            //    StorageInstanceRef recordOwnerObject = RelationObject.RealStorageInstanceRef as StorageInstanceRef;

            //    foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
            //    {
            //        Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.DataBaseColumnName, ((OOAdvantech.PersistenceLayer.ObjectID)RoleB.PersistentObjectID).GetMemberValue(column.ColumnType));
            //        recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.PersistenceLayer.ObjectID)RoleB.PersistentObjectID).GetMemberValue(column.ColumnType);
            //    }
            //    foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
            //    {
            //        Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.DataBaseColumnName, ((OOAdvantech.PersistenceLayer.ObjectID)RoleA.PersistentObjectID).GetMemberValue(column.ColumnType));
            //        recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.PersistenceLayer.ObjectID)RoleA.PersistentObjectID).GetMemberValue(column.ColumnType);
            //    }
            //    if (objectCollectionsLink.Type.RoleA.Indexer)
            //    {

            //        Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, (objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath).DataBaseColumnName, RoleAIndex);
            //        recordOwnerObject.RelationshipColumnsValues[(objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath)] = RoleAIndex;
            //    }
            //    if (objectCollectionsLink.Type.RoleB.Indexer)
            //    {
            //        Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, (objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath).DataBaseColumnName, RoleBIndex);
            //        recordOwnerObject.RelationshipColumnsValues[(objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath)] = RoleBIndex;
            //    }
            //    return;
            //}

            #region Open database connection and execute the command
            var oleDbConnection = (RoleA.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            //System.Data.SqlClient.SqlConnection oleDbConnection = ((ObjectStorage)RoleA.ObjectStorage).DBConnection;
            if (oleDbConnection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                oleDbConnection.Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //oleDbConnection .EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            var oleDbCommand = oleDbConnection.CreateCommand();
            oleDbCommand.CommandText = "";// LinkQuery;

            int insertedRows = 0;
            var myReader = oleDbCommand.ExecuteReader();
            while (myReader.Read())
            {
                insertedRows = (int)myReader["ROW_COUNT"];

                break;
            }
            myReader.Close();
            if (insertedRows != 0 && association.LinkClass == null)
                objectCollectionsLink.ObjectsLinksCount++;
            #endregion



            int lo = 0;
        }
        private void UpdateOnetoManyOneToOne(RDBMSMetaDataRepository.AssociationEnd associationEnd)
        {
            #region Precondition check
            if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany || associationEnd.Association.LinkClass != null)
                throw new System.Exception("It Can’t  link many to many relationship");
            #endregion

            associationEnd = objectCollectionsLink.GetAssociationEndWithReferenceColumns();
            StorageInstanceRef recordOwnerObject = null;
            StorageInstanceRef assignedObject = null;
            #region Gets metadata informations

            if (associationEnd.IsRoleA)
            {
                recordOwnerObject = RoleA.RealStorageInstanceRef as StorageInstanceRef;
                assignedObject = RoleB.RealStorageInstanceRef as StorageInstanceRef;
            }
            else
            {
                recordOwnerObject = RoleB.RealStorageInstanceRef as StorageInstanceRef;
                assignedObject = RoleA.RealStorageInstanceRef as StorageInstanceRef;
            }

            //TODO τι γίνεται στην περίπτωση που έχουμε one table per class και οι columns του association end βρίσκονται σε table  της parent class

            //vaueTypePath = "";
            //if (RoleA.ValueTypePath.Count > 0)
            //    vaueTypePath = RoleA.ValueTypePath.ToString();
            //else
            //    vaueTypePath = RoleB.ValueTypePath.ToString();
            IList<RDBMSMetaDataRepository.IdentityColumn> linkColumns = associationEnd.GetReferenceColumnsFor(recordOwnerObject.StorageInstanceSet, vaueTypePath);
            //foreach (RDBMSMetaDataRepository.Column column in linkColumns)
            //{
            //    KeepColumnTable = column.Namespace as RDBMSMetaDataRepository.Table;
            //    break;
            //}


            #endregion



            #region Open database connection and execute the command

            //System.Data.Common.DbConnection oleDbConnection = (recordOwnerObject.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            //if (oleDbConnection.State != System.Data.ConnectionState.Open)
            //    oleDbConnection.Open();
            if (associationEnd.Indexer)
            {
                PersistenceLayerRunTime.StorageInstanceRef collectionOwner = null;
                if (associationEnd.IsRoleA)
                    collectionOwner = RoleB.RealStorageInstanceRef;
                else
                    collectionOwner = RoleA.RealStorageInstanceRef;

                int index = -1;
                if (associationEnd.IsRoleA)
                    index = RoleAIndex;
                else
                    index = RoleBIndex;

                if (associationEnd.IsRoleA)
                    RoleAIndex = index;
                else
                    RoleBIndex = index;

                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, associationEnd.GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath).DataBaseColumnName, index);
                recordOwnerObject.RelationshipColumnsValues[associationEnd.GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath)] = index;
            }
            //foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            //{
            //    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.DataBaseColumnName, ((OOAdvantech.PersistenceLayer.ObjectID)assignedObject.PersistentObjectID).GetMemberValue(column.ColumnType));
            //    recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.PersistenceLayer.ObjectID)assignedObject.PersistentObjectID).GetMemberValue(column.ColumnType);
            //}

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

  

    }
}
