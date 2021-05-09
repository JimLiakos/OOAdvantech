using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectID = OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
namespace OOAdvantech.MSSQLCompactPersistenceRunTime.Commands
{
    /// <MetaDataID>{31f57168-9785-4953-a8ae-e2ff9f6d07e9}</MetaDataID>
    public class LinkObjectsCommand : PersistenceLayerRunTime.Commands.LinkObjectsCommand
    {
        public class IndexerItem
        {
            /// <MetaDataID>{cec62ba2-9a77-4d4a-9b69-e9d43ad64666}</MetaDataID>
            public int index;
            /// <MetaDataID>{51d6d729-2d41-40bd-8515-27c86122750f}</MetaDataID>
            public PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef;

        }
        public class IndexerItemsCollection
        {
            /// <MetaDataID>{38e7db0f-c28a-47cf-9d9d-842940af995a}</MetaDataID>
            public int LastIndex;
            /// <MetaDataID>{98d2ff2e-2d3a-49aa-a62f-aa3362224d5a}</MetaDataID>
            public System.Collections.Generic.List<IndexerItem> Items = new List<IndexerItem>();
        }

        /// <MetaDataID>{eb0f4787-cb8a-40ca-857e-c6bd2f71b6b7}</MetaDataID>
        public static Dictionary<string, Dictionary<PersistenceLayerRunTime.StorageInstanceRef, Dictionary<MetaDataRepository.AssociationEnd, IndexerItemsCollection>>> Indexer = new Dictionary<string, Dictionary<OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef, Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, IndexerItemsCollection>>>();
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{80E0A290-7601-4EF9-BA9C-534C02713C4F}</MetaDataID>
        private RDBMSMetaDataRepository.AssociationEnd _LinkInitiatorAssociationEnd;
        /// <MetaDataID>{971FD658-73C8-42E5-9CFC-71AB68E0A2DB}</MetaDataID>
        new private RDBMSMetaDataRepository.AssociationEnd LinkInitiatorAssociationEnd
        {
            get
            {
                if (_LinkInitiatorAssociationEnd == null)
                    _LinkInitiatorAssociationEnd = base.LinkInitiatorAssociationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                return _LinkInitiatorAssociationEnd;
            }
        }
        ///// <MetaDataID>{BC959ED3-AABD-4F5A-978F-C87AEF43389D}</MetaDataID>
        //private UnLinkObjectsCommand FindUnLinkObjectsCommand(StorageInstanceRef KeepsRelationColumns)
        //{
        //    return null;
        //}
        /// <MetaDataID>{0CD0E6BE-8E42-4668-AFF2-14F8F7F25422}</MetaDataID>
        public LinkObjectsCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
            :
           base(roleA, roleB, relationObject, linkInitiatorAssociationEnd, index)
        {

        }


        /// <MetaDataID>{E36D550A-294A-4D31-9EFF-8DB4B771BCF7}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {
            if (currentExecutionOrder <= 10)
                return;
            if (!SubTransactionCmdsProduced)
            {
                base.GetSubCommands(currentExecutionOrder);
                SubTransactionCmdsProduced = true;
                UpdateMappingDataIfNeeded();
            }
            else
                base.GetSubCommands(currentExecutionOrder);


        }




        /// <MetaDataID>{AFE64815-5E6B-4832-8539-69EACC437A24}</MetaDataID>
        private void LinkOnetoManyOneToOne(RDBMSMetaDataRepository.AssociationEnd associationEnd)
        {
            #region Precondition check
            if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany || associationEnd.Association.LinkClass != null)
                throw new System.Exception("It Can’t  link many to many relationship");
            #endregion

            associationEnd = (associationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
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

            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns = associationEnd.GetReferenceColumnsFor(recordOwnerObject.StorageInstanceSet, vaueTypePath);
            //foreach (RDBMSMetaDataRepository.Column column in linkColumns)
            //{
            //    KeepColumnTable = column.Namespace as RDBMSMetaDataRepository.Table;
            //    break;
            //}


            #endregion



            #region Open database connection and execute the command

            System.Data.Common.DbConnection oleDbConnection = ((recordOwnerObject.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
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
                //string transactionUri = Transactions.Transaction.Current.LocalTransactionUri;

                //if (!Indexer.ContainsKey(transactionUri) || !Indexer[transactionUri].ContainsKey(collectionOwner) || !Indexer[transactionUri][collectionOwner].ContainsKey(associationEnd))
                //{
                //    int count = 0;

                //    foreach (RDBMSMetaDataRepository.StorageCell storageCell in assignedObject.StorageInstanceSet.GetLinkedStorageCells(associationEnd.Association.Identity, associationEnd.Role))
                //    {
                //        Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> storageCellLinkColumns = associationEnd.GetReferenceColumnsFor(storageCell);
                //        RDBMSMetaDataRepository.Table table = storageCellLinkColumns[0].Namespace as RDBMSMetaDataRepository.Table;
                //        string countCalculation = ((recordOwnerObject.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.RDBMSSchema.BuildCountItemsInTableStatament(assignedObject, table, storageCellLinkColumns);
                //        System.Data.Common.DbCommand oleDbCommand = oleDbConnection.CreateCommand();
                //        oleDbCommand.CommandText = countCalculation;
                //        count += (int)oleDbCommand.ExecuteScalar();
                //    }
                //    if (!Indexer.ContainsKey(transactionUri))
                //    {
                //        Indexer[transactionUri] = new Dictionary<OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef, Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, IndexerItemsCollection>>();
                //        Indexer[transactionUri][collectionOwner] = new Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, IndexerItemsCollection>();
                //        Indexer[transactionUri][collectionOwner][associationEnd] = new IndexerItemsCollection();
                //    }
                //    else
                //    {
                //        if (!Indexer[transactionUri].ContainsKey(collectionOwner))
                //        {
                //            Indexer[transactionUri][collectionOwner] = new Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, IndexerItemsCollection>();
                //            Indexer[transactionUri][collectionOwner][associationEnd] = new IndexerItemsCollection();
                //        }
                //    }
                //    Indexer[transactionUri][collectionOwner][associationEnd].LastIndex = count;


                //}
                //if (index == -1)
                //{
                //    index = Indexer[transactionUri][collectionOwner][associationEnd].LastIndex;
                //    Indexer[transactionUri][collectionOwner][associationEnd].LastIndex = Indexer[transactionUri][collectionOwner][associationEnd].LastIndex + 1;
                //}

                //if (index != -1 && index < Indexer[transactionUri][collectionOwner][associationEnd].LastIndex)
                //{

                //    foreach (RDBMSMetaDataRepository.StorageCell storageCell in assignedObject.StorageInstanceSet.GetLinkedStorageCells(associationEnd.Association.Identity, associationEnd.Role))
                //    {

                //        Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> storageCellLinkColumns = associationEnd.GetReferenceColumnsFor(storageCell);
                //        RDBMSMetaDataRepository.Table table = storageCellLinkColumns[0].Namespace as RDBMSMetaDataRepository.Table;

                //        string rebuildItemsIndexStatament = ((recordOwnerObject.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.RDBMSSchema.RebuildItemsIndexInTableStatament(assignedObject, table, storageCellLinkColumns, index, associationEnd.IndexerColumn);
                //        System.Data.Common.DbCommand oleDbCommand = oleDbConnection.CreateCommand();
                //        oleDbCommand.CommandText = rebuildItemsIndexStatament;
                //        oleDbCommand.ExecuteNonQuery();
                //    }
                //}





                if (associationEnd.IsRoleA)
                    RoleAIndex = index;
                else
                    RoleBIndex = index;

                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, associationEnd.IndexerColumn.DataBaseColumnName, index);
                recordOwnerObject.RelationshipColumnsValues[associationEnd.IndexerColumn] = index;

   
            }
            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.Name, ((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)assignedObject.ObjectID).GetMemberValue(column.ColumnType));
                recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)assignedObject.ObjectID).GetMemberValue(column.ColumnType);
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

        /// <MetaDataID>{386626BF-F008-49CB-90F5-CF171658278F}</MetaDataID>
        private void UpdateMappingDataIfNeeded()
        {

            //Get storage cells link for roleA,roleB storage cells
            RDBMSMetaDataRepository.Association Association = LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association;
            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();
            RDBMSMetaDataRepository.StorageCellsLink ObjectCollectionsLink = Association.GetStorageCellsLink(((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet, ((StorageInstanceRef)RoleB.RealStorageInstanceRef).StorageInstanceSet, vaueTypePath, true);

            //if storage cells ling produce new table then you must update MSSQL database schema. 
            //			if(ObjectCollectionsLink.NewTableCreated)
            //				this.OwnerTransactiont.EnlistCommand(new UpdateStorageSchema((ObjectStorage )RoleA.ObjectStorage));

            if (Association.LinkClass != null)
            {
                //Adds the storage cell of relation object to the association class storage cells of storage cells link 
                if (!ObjectCollectionsLink.AssotiationClassStorageCells.Contains(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet))
                {
                    ObjectCollectionsLink.AddAssotiationClassStorageCell(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet);
                    ObjectCollectionsLink.UpdateForeignKeys();
                    OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema((ObjectStorage)RoleA.ObjectStorage);
                    if (!OwnerTransactiont.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
                        OwnerTransactiont.EnlistCommand(updateStorageSchema);
                }
            }
            else if (StorageInstanceRef.GetStorageInstanceRef(ObjectCollectionsLink.Properties).ObjectID == null)
            {
                ObjectCollectionsLink.UpdateForeignKeys();

                OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema updateStorageSchema = new OOAdvantech.RDBMSPersistenceRunTime.Commands.UpdateStorageSchema((ObjectStorage)RoleA.ObjectStorage);
                if (!OwnerTransactiont.EnlistedCommands.ContainsKey(updateStorageSchema.Identity))
                    OwnerTransactiont.EnlistCommand(updateStorageSchema);
            }




        }

        /// <MetaDataID>{502555A3-C60E-44DA-B601-E3B381AA486D}</MetaDataID>
        public override void Execute()
        {
            #region Preconditions Chechk
            if (RoleA == null || RoleB == null)
                throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
            #endregion

            base.Execute();
            if (MetaDataRepository.AssociationType.ManyToMany != LinkInitiatorAssociationEnd.Association.MultiplicityType && LinkInitiatorAssociationEnd.Association.LinkClass == null)
                LinkOnetoManyOneToOne(LinkInitiatorAssociationEnd);
            else
                LinkManyToMany(LinkInitiatorAssociationEnd);
        }
        /// <MetaDataID>{0F0C2C6E-9BA1-4B4C-B4E9-92A8B288EE6E}</MetaDataID>
        private void LinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
        {

            RDBMSMetaDataRepository.Association association = (RDBMSMetaDataRepository.Association)mAssociationEnd.Association;

            string vaueTypePath = "";
            RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink = null;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns = null;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns = null;

            #region Gets metadata informations

            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();
            objectCollectionsLink = association.GetStorageCellsLink(((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet, ((StorageInstanceRef)RoleB.RealStorageInstanceRef).StorageInstanceSet, vaueTypePath);
            if (RelationObject != null)
            {
                roleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor((RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet);
                roleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor((RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet);
            }
            else
            {
                roleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
                roleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);

            }
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> relationObjectIDColumns = null;
            if (association.LinkClass != null)
                relationObjectIDColumns = (RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet.MainTable.ObjectIDColumns;
            #endregion

            if (association.LinkClass == null)
            {
                Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleAColumnsValues = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object>();
                Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleBColumnsValues = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object>();

                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
                    roleAColumnsValues[column] = (RoleB.ObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).GetMemberValue(column.ColumnType);
                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
                    roleBColumnsValues[column] = (RoleA.ObjectID as ObjectID).GetMemberValue(column.ColumnType);




                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.AddRelationRow(objectCollectionsLink, roleAColumnsValues, RoleAIndex, roleBColumnsValues, RoleBIndex);
                objectCollectionsLink.ObjectsLinksCount++;
                return;
            }
            else
            {
                StorageInstanceRef recordOwnerObject = RelationObject.RealStorageInstanceRef as StorageInstanceRef;

                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
                {
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.Name, ((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)RoleB.ObjectID).GetMemberValue(column.ColumnType));
                    recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)RoleB.ObjectID).GetMemberValue(column.ColumnType);
                }
                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
                {
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.Name, ((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)RoleA.ObjectID).GetMemberValue(column.ColumnType));
                    recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.RDBMSPersistenceRunTime.ObjectID)RoleA.ObjectID).GetMemberValue(column.ColumnType);
                }
                if (objectCollectionsLink.Type.RoleA.Indexer)
                {
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, (objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, RoleAIndex);
                    recordOwnerObject.RelationshipColumnsValues[(objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn] = RoleAIndex;
                }
                if (objectCollectionsLink.Type.RoleB.Indexer)
                {
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, (objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.DataBaseColumnName, RoleBIndex);
                    recordOwnerObject.RelationshipColumnsValues[(objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn] = RoleBIndex;
                }
                return;
            }

            #region Open database connection and execute the command
            System.Data.Common.DbConnection oleDbConnection = ((RoleA.ObjectStorage as ObjectStorage).StorageMetaData as RDBMSPersistenceRunTime.Storage).StorageDataBase.Connection;
            //System.Data.SqlClient.SqlConnection oleDbConnection = ((ObjectStorage)RoleA.ObjectStorage).DBConnection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //oleDbConnection .EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            System.Data.Common.DbCommand oleDbCommand = oleDbConnection.CreateCommand();
            oleDbCommand.CommandText = "";// LinkQuery;

            int insertedRows = 0;
            System.Data.Common.DbDataReader myReader = oleDbCommand.ExecuteReader();
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

    }
}

//INSERT INTO RelationTable
//(IDRoleA, IDRoleB)
//SELECT     IDRoleA, IDRoleB
//FROM         TemporaryRelationTable
//WHERE NOT EXISTS(
//    SELECT IDRoleA, IDRoleB
//    FROM RelationTable  
//    WHERE  (TemporaryRelationTable.IDRoleA = IDRoleA) AND (TemporaryRelationTable.IDRoleB = IDRoleB)
//                 )