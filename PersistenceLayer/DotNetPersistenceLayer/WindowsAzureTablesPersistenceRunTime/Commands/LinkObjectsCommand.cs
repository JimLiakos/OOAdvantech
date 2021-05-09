using System;
using System.Collections.Generic;
using System.Text;
//using ObjectID = OOAdvantech.PersistenceLayer.ObjectID;
namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
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
                    _LinkInitiatorAssociationEnd = (ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(base.LinkInitiatorAssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                return _LinkInitiatorAssociationEnd;
            }
        }
        ///// <MetaDataID>{BC959ED3-AABD-4F5A-978F-C87AEF43389D}</MetaDataID>
        //private UnLinkObjectsCommand FindUnLinkObjectsCommand(StorageInstanceRef KeepsRelationColumns)
        //{
        //    return null;
        //}
        /// <MetaDataID>{0CD0E6BE-8E42-4668-AFF2-14F8F7F25422}</MetaDataID>
        public LinkObjectsCommand(PersistenceLayerRunTime.ObjectStorage objectStorage, PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
            : base(objectStorage, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index)
        {
            _LinkInitiatorAssociationEnd = (objectStorage.StorageMetaData as Storage).GetEquivalentMetaObject(linkInitiatorAssociationEnd.Identity.ToString(), typeof(RDBMSMetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
        }


        /// <MetaDataID>{E36D550A-294A-4D31-9EFF-8DB4B771BCF7}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {
            //if (currentExecutionOrder <= 10)
            //    return;
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
            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.DataBaseColumnName, ((OOAdvantech.PersistenceLayer.ObjectID)assignedObject.PersistentObjectID).GetMemberValue(column.ColumnType));
                recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.PersistenceLayer.ObjectID)assignedObject.PersistentObjectID).GetMemberValue(column.ColumnType);
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


            if (LinkInitiatorAssociationEnd == null)
                AddUnknownAssociationMetadataToStore();
            //Get storage cells link for roleA,roleB storage cells
            RDBMSMetaDataRepository.Association Association = LinkInitiatorAssociationEnd.Association as RDBMSMetaDataRepository.Association;

            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();
            objectCollectionsLink = Association.GetStorageCellsLink(ObjectStorage.GetStorageCell(RoleA), ObjectStorage.GetStorageCell(RoleB), vaueTypePath, true);

            //if storage cells ling produce new table then you must update MSSQL database schema. 
            //			if(ObjectCollectionsLink.NewTableCreated)
            //				this.OwnerTransactiont.EnlistCommand(new UpdateStorageSchema((ObjectStorage )RoleA.ObjectStorage));

            if (Association.LinkClass != null)
            {
                System.Collections.Generic.List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                foreach (MetaDataRepository.IIdentityPart part in (objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet))
                    parts.Add(part);
                MetaDataRepository.ObjectIdentityType roleAObjectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);

                parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                foreach (MetaDataRepository.IIdentityPart part in (objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet))
                    parts.Add(part);
                MetaDataRepository.ObjectIdentityType roleBObjectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);
                if ((objectCollectionsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType != roleAObjectIdentityType ||
                (objectCollectionsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType != roleBObjectIdentityType)
                {


                }



                //Adds the storage cell of relation object to the association class storage cells of storage cells link 
                if (!objectCollectionsLink.AssotiationClassStorageCells.Contains(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet))
                {
                    objectCollectionsLink.AddAssotiationClassStorageCell(((StorageInstanceRef)RelationObject.RealStorageInstanceRef).StorageInstanceSet);
                    objectCollectionsLink.UpdateForeignKeys();
                    Commands.UpdateStorageSchema updateStorageSchema = null;
                    if (!OwnerTransactiont.EnlistedCommands.ContainsKey(Commands.UpdateStorageSchema.GetIdentity(ObjectStorage)))
                    {
                        updateStorageSchema = new UpdateStorageSchema(RoleA.ObjectStorage as ObjectStorage);
                        OwnerTransactiont.EnlistCommand(updateStorageSchema);
                    }
                    else
                        updateStorageSchema = OwnerTransactiont.EnlistedCommands[(Commands.UpdateStorageSchema.GetIdentity(ObjectStorage))] as Commands.UpdateStorageSchema;
                    updateStorageSchema.UpdateStorageCellsLinks(objectCollectionsLink);

                }
            }
            else if (StorageInstanceRef.GetStorageInstanceRef(objectCollectionsLink.Properties).PersistentObjectID == null)
            {
                objectCollectionsLink.UpdateForeignKeys();

                Commands.UpdateStorageSchema updateStorageSchema = null;
                if (!OwnerTransactiont.EnlistedCommands.ContainsKey(Commands.UpdateStorageSchema.GetIdentity(ObjectStorage)))
                {
                    updateStorageSchema = new UpdateStorageSchema(ObjectStorage as ObjectStorage);
                    OwnerTransactiont.EnlistCommand(updateStorageSchema);
                }
                else
                    updateStorageSchema = OwnerTransactiont.EnlistedCommands[(Commands.UpdateStorageSchema.GetIdentity(ObjectStorage))] as Commands.UpdateStorageSchema;
                updateStorageSchema.UpdateStorageCellsLinks(objectCollectionsLink);
            }

        }

        private void AddUnknownAssociationMetadataToStore()
        {
            if (this.LinkInitiatorAssociationEndAgent.IsRoleA)
            {

                var classifier = (ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(LinkInitiatorAssociationEndAgent.RoleASpecificationIdentity.ToString(), typeof(MetaDataRepository.Classifier)) as MetaDataRepository.Classifier;

            }
            else
            {
                var classifier = (ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(LinkInitiatorAssociationEndAgent.RoleBSpecificationIdentity.ToString(), typeof(MetaDataRepository.Classifier)) as MetaDataRepository.Classifier;
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

            if (ObjectStorage.GetStorageCell(RoleA).ObjectIdentityType != ObjectStorage.GetStorageCell(RoleB).ObjectIdentityType)
                LinkManyToMany(LinkInitiatorAssociationEnd);
            else if (MetaDataRepository.AssociationType.ManyToMany == LinkInitiatorAssociationEnd.Association.MultiplicityType ||
                LinkInitiatorAssociationEnd.Association.LinkClass != null ||
                RoleA.ObjectStorage != RoleB.ObjectStorage ||
                objectCollectionsLink.ObjectLinksTable != null)
                LinkManyToMany(LinkInitiatorAssociationEnd);
            else
                LinkOnetoManyOneToOne(LinkInitiatorAssociationEnd);

        }
        /// <MetaDataID>{9f4ba287-4744-4b36-a406-5e2458c3a2b9}</MetaDataID>
        RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink = null;
        /// <MetaDataID>{2c194892-cd44-40b6-b0af-26e2fa5488dd}</MetaDataID>
        string vaueTypePath = "";
        /// <MetaDataID>{0F0C2C6E-9BA1-4B4C-B4E9-92A8B288EE6E}</MetaDataID>
        private void LinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
        {


            RDBMSMetaDataRepository.Association association = (RDBMSMetaDataRepository.Association)mAssociationEnd.Association;


            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns = null;
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns = null;

            #region Gets metadata informations

            if (RelationObject != null)
            {
                if (objectCollectionsLink.ObjectLinksTable != null)
                {
                    roleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
                    roleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
                }
                else
                {
                    roleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor((RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet);
                    roleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor((RelationObject.RealStorageInstanceRef as StorageInstanceRef).StorageInstanceSet);
                }
            }
            else
            {
                roleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
                roleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);

            }
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> relationObjectIDColumns = null;
            if (association.LinkClass != null)
                relationObjectIDColumns = (RelationObject.RealStorageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns;
            #endregion

            if (association.LinkClass == null || objectCollectionsLink.ObjectLinksTable != null)
            {
                Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleBColumnsValues = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object>();
                Dictionary<RDBMSMetaDataRepository.IdentityColumn, object> roleAColumnsValues = new Dictionary<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn, object>();

                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
                    roleAColumnsValues[column] = (RoleB.PersistentObjectID as OOAdvantech.PersistenceLayer.ObjectID).GetMemberValue(column.ColumnType);
                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
                    roleBColumnsValues[column] = (RoleA.PersistentObjectID as OOAdvantech.PersistenceLayer.ObjectID).GetMemberValue(column.ColumnType);

                if (RelationObject != null)
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.AddRelationRow(objectCollectionsLink, RelationObject.PersistentObjectID as PersistenceLayer.ObjectID, roleAColumnsValues, RoleAIndex, roleBColumnsValues, RoleBIndex);
                else
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.AddRelationRow(objectCollectionsLink, null, roleAColumnsValues, RoleAIndex, roleBColumnsValues, RoleBIndex);
                objectCollectionsLink.ObjectsLinksCount++;
                return;
            }
            else
            {
                StorageInstanceRef recordOwnerObject = RelationObject.RealStorageInstanceRef as StorageInstanceRef;

                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
                {
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.DataBaseColumnName, ((OOAdvantech.PersistenceLayer.ObjectID)RoleB.PersistentObjectID).GetMemberValue(column.ColumnType));
                    recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.PersistenceLayer.ObjectID)RoleB.PersistentObjectID).GetMemberValue(column.ColumnType);
                }
                foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
                {
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, column.DataBaseColumnName, ((OOAdvantech.PersistenceLayer.ObjectID)RoleA.PersistentObjectID).GetMemberValue(column.ColumnType));
                    recordOwnerObject.RelationshipColumnsValues[column] = ((OOAdvantech.PersistenceLayer.ObjectID)RoleA.PersistentObjectID).GetMemberValue(column.ColumnType);
                }
                if (objectCollectionsLink.Type.RoleA.Indexer)
                {

                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, (objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath).DataBaseColumnName, RoleAIndex);
                    recordOwnerObject.RelationshipColumnsValues[(objectCollectionsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath)] = RoleAIndex;
                }
                if (objectCollectionsLink.Type.RoleB.Indexer)
                {
                    Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(recordOwnerObject, (objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath).DataBaseColumnName, RoleBIndex);
                    recordOwnerObject.RelationshipColumnsValues[(objectCollectionsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(recordOwnerObject.StorageInstanceSet, vaueTypePath)] = RoleBIndex;
                }
                return;
            }

            #region Open database connection and execute the command
            //var oleDbConnection = (RoleA.ObjectStorage .StorageMetaData as Storage).StorageDataBase.Connection;
            //    var oleDbCommand = oleDbConnection.CreateCommand();
            //oleDbCommand.CommandText = "";// LinkQuery;

            //int insertedRows = 0;
            //var myReader = oleDbCommand.ExecuteReader();
            //while (myReader.Read())
            //{
            //    insertedRows = (int)myReader["ROW_COUNT"];

            //    break;
            //}
            //myReader.Close();
            //if (insertedRows != 0 && association.LinkClass == null)
            //    objectCollectionsLink.ObjectsLinksCount++;
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