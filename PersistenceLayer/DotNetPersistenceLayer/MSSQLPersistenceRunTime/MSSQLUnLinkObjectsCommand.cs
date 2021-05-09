namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
    using ObjectID = OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
    //using OOAdvantech.RDBMSPersistenceRunTime;
  //  using OOAdvantech.RDBMSPersistenceRunTime;
    /// <MetaDataID>{0DE6D82D-2163-4460-885A-FFA7DAD71AFE}</MetaDataID>
    public class UnLinkObjectsCommand : PersistenceLayerRunTime.Commands.UnLinkObjectsCommand
    {
        public UnLinkObjectsCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
            :base(roleA, roleB, relationObject, linkInitiatorAssociationEnd, index)
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
                    _OwnerAssociationEnd = base.LinkInitiatorAssociationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                return _OwnerAssociationEnd;
            }
        }

        /// <MetaDataID>{07430BD8-A10A-475A-87CD-4907C7D0BD41}</MetaDataID>
        private void UnlinkOnetoManyOneToOne(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
        {

            RDBMSMetaDataRepository.AssociationEnd associationEnd = (mAssociationEnd.Association as RDBMSMetaDataRepository.Association).GetAssociationEndWithReferenceColumns();
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
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> linkColumns = associationEnd.GetReferenceColumnsFor(RecordOwnerObject.StorageInstanceSet);
            foreach (RDBMSMetaDataRepository.Column column in linkColumns)
            {
                KeepColumnTable = column.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }


            #endregion

            string UpdateQuery;
            #region Build SQL command

            UpdateQuery = "UPDATE " + KeepColumnTable.Name + " SET ";

            string WhereClause = " WHERE ";
            bool FirstValue = true;
            string existCriterion = null;
            string listOwnerCriterion = null;
            foreach (RDBMSMetaDataRepository.IdentityColumn column in linkColumns)
            {
                if (!FirstValue)
                {
                    WhereClause += " AND ";
                    existCriterion += " AND ";
                    UpdateQuery += ",";
                }
                FirstValue = false;
                UpdateQuery += column.Name + " = NULL ";
                WhereClause += column.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)AssignedObject.ObjectID).GetMemberValue(column.ColumnType));
                existCriterion += column.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)AssignedObject.ObjectID).GetMemberValue(column.ColumnType));
                listOwnerCriterion += column.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)AssignedObject.ObjectID).GetMemberValue(column.ColumnType));

            }
            if (associationEnd.Indexer)
                UpdateQuery += "," + associationEnd.IndexerColumn.Name + " = NULL ";


            foreach (RDBMSMetaDataRepository.IdentityColumn column in KeepColumnTable.ObjectIDColumns)
            {

                if (!FirstValue)
                {
                    WhereClause += " AND ";
                    existCriterion += " AND ";
                }
                FirstValue = false;
                WhereClause += column.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)RecordOwnerObject.ObjectID).GetMemberValue(column.ColumnType));
                existCriterion += column.Name + " = " + TypeDictionary.ConvertToSQLString(((ObjectID)RecordOwnerObject.ObjectID).GetMemberValue(column.ColumnType));
            }
            string getRoleIndex = @"declare @roleIndex int;
                                   select @roleIndex=[IndexerColumn] 
                                   from [KeepColumnTable] 
                                   WHERE ([ExistCriterion])";

            string roleUpdateRelationIndex = @"UPDATE [KeepColumnTable]
                                            SET     [IndexerColumn] = [IndexerColumn] - 1
                                            WHERE  ([IndexerColumn] >= @roleIndex and [ListOwnerCriterion])";


            UpdateQuery += WhereClause;


            if (associationEnd.Indexer)
            {
                getRoleIndex=getRoleIndex.Replace("[IndexerColumn]", associationEnd.IndexerColumn.Name);
                getRoleIndex = getRoleIndex.Replace("[KeepColumnTable]", KeepColumnTable.Name);
                getRoleIndex = getRoleIndex.Replace("[ExistCriterion]", existCriterion);
                roleUpdateRelationIndex=roleUpdateRelationIndex.Replace("[IndexerColumn]", associationEnd.IndexerColumn.Name);
                roleUpdateRelationIndex = roleUpdateRelationIndex.Replace("[KeepColumnTable]", KeepColumnTable.Name);
                roleUpdateRelationIndex = roleUpdateRelationIndex.Replace("[ListOwnerCriterion]", listOwnerCriterion);
                UpdateQuery = getRoleIndex + "\n" + UpdateQuery + "\n" + roleUpdateRelationIndex;
            }

            #endregion

            int RowAffected;
            #region Open database connection and execute the command
            System.Data.SqlClient.SqlConnection oleDbConnection = (RecordOwnerObject.ObjectStorage as ObjectStorage).DBConnection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            System.Data.SqlClient.SqlCommand oleDbCommand = oleDbConnection.CreateCommand();
            oleDbCommand.CommandText = UpdateQuery;
            RowAffected = oleDbCommand.ExecuteNonQuery();
            #endregion




        }
        /// <MetaDataID>{8AC4B73B-2664-4367-B3B8-8819091E4234}</MetaDataID>
        private void UnlinkManyToMany(RDBMSMetaDataRepository.AssociationEnd mAssociationEnd)
        {
            RDBMSMetaDataRepository.Association association = (RDBMSMetaDataRepository.Association)mAssociationEnd.Association;


            #region Gets metadata informations

            string vaueTypePath = "";
            if (RoleA.ValueTypePath.Count > 0)
                vaueTypePath = RoleA.ValueTypePath.ToString();
            else
                vaueTypePath = RoleB.ValueTypePath.ToString();

            RDBMSMetaDataRepository.StorageCellsLink objectCollectionsLink = association.GetStorageCellsLink(((StorageInstanceRef)RoleA.RealStorageInstanceRef).StorageInstanceSet, ((StorageInstanceRef)RoleB.RealStorageInstanceRef).StorageInstanceSet, vaueTypePath);
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> roleAColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleA).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns = ((RDBMSMetaDataRepository.AssociationEnd)association.RoleB).GetReferenceColumnsFor(objectCollectionsLink.ObjectLinksTable);
            #endregion

            if (association.LinkClass != null)
                return;

            #region Build SQL command
            string DeleteQuery = "";

            string indexRoleBCriterion = null;
            string indexRoleACriterion = null;

            bool setRoleAIndex = false;
            bool setRoleBIndex = false;
            if (association.RoleA.Indexer)
                setRoleAIndex = true;

            if (association.RoleB.Indexer)
                setRoleBIndex = true;

            string getRoleAIndex = @"declare @roleAIndex int;
                                   select @roleAIndex=[IndexerColumn] 
                                   from [AssociationTable] 
                                   WHERE ([ExistCriterion])";

            string getRoleBIndex = @"declare @roleBIndex int;
                                   select @roleBIndex=[IndexerColumn] 
                                   from [AssociationTable] 
                                   WHERE ([ExistCriterion])";


            string roleBUpdateRelationIndex = @"UPDATE [AssociationTable]
                                            SET     [IndexerColumn] = [IndexerColumn] - 1
                                            WHERE  ([IndexerColumn] >= @roleBIndex and [ListOwnerCriterion])";
            string roleAUpdateRelationIndex = @"UPDATE [AssociationTable]
                                            SET     [IndexerColumn] = [IndexerColumn] - 1
                                            WHERE  ([IndexerColumn] >= @roleAIndex and [ListOwnerCriterion])";



            DeleteQuery += "DELETE FROM [AssociationTable] " +
                        "WHERE ([ExistCriterion])";

            string existCriterion = null;

            foreach (RDBMSMetaDataRepository.IdentityColumn column in roleAColumns)
            {
                if (existCriterion != null)
                    existCriterion += " AND ";
                string column_value = TypeDictionary.ConvertToSQLString((RoleB.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
                existCriterion += " " + column.Name + " = " + column_value;

                if (setRoleAIndex)
                {
                    if (indexRoleACriterion != null)
                        indexRoleACriterion += " AND ";
                    column_value = TypeDictionary.ConvertToSQLString((RoleB.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
                    indexRoleACriterion += " " + column.Name + " = " + column_value;
                }
            }
           

            foreach (RDBMSMetaDataRepository.IdentityColumn column in roleBColumns)
            {
                if (existCriterion != null)
                    existCriterion += " AND ";
                string column_value = TypeDictionary.ConvertToSQLString((RoleA.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
                existCriterion += " " + column.Name + " = " + column_value;

                if (setRoleBIndex)
                {
                    if (indexRoleBCriterion != null)
                        indexRoleBCriterion += " AND ";
                    column_value = TypeDictionary.ConvertToSQLString((RoleB.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
                    indexRoleBCriterion += " " + column.Name + " = " + column_value;
                }
            }


            DeleteQuery = DeleteQuery.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
            DeleteQuery = DeleteQuery.Replace("[ExistCriterion]", existCriterion);


            if (setRoleAIndex)
            {
                getRoleAIndex = getRoleAIndex.Replace("[ExistCriterion]", existCriterion);
                getRoleAIndex = getRoleAIndex.Replace("[IndexerColumn]", (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                getRoleAIndex = getRoleAIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[IndexerColumn]", (association.RoleA as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                roleAUpdateRelationIndex = roleAUpdateRelationIndex.Replace("[ListOwnerCriterion]", indexRoleACriterion);
                DeleteQuery = getRoleAIndex + "\n" + roleAUpdateRelationIndex + "\n" + DeleteQuery;
            }

            if (setRoleBIndex)
            {

                getRoleBIndex = getRoleBIndex.Replace("[ExistCriterion]", existCriterion);
                getRoleBIndex = getRoleBIndex.Replace("[IndexerColumn]", (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                getRoleBIndex = getRoleBIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[AssociationTable]", objectCollectionsLink.ObjectLinksTable.Name);
                roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[IndexerColumn]", (association.RoleB as RDBMSMetaDataRepository.AssociationEnd).IndexerColumn.Name);
                roleBUpdateRelationIndex = roleBUpdateRelationIndex.Replace("[ListOwnerCriterion]", indexRoleBCriterion);
                DeleteQuery = getRoleBIndex + "\n" + roleAUpdateRelationIndex + "\n" + DeleteQuery;
            }
            #endregion

            #region Open database connection and execute the command
            System.Data.SqlClient.SqlConnection oleDbConnection = ((ObjectStorage)RoleA.ObjectStorage).DBConnection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);

            //			oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            System.Data.SqlClient.SqlCommand oleDbCommand = oleDbConnection.CreateCommand();
            oleDbCommand.CommandText = DeleteQuery;
            object Result = oleDbCommand.ExecuteNonQuery();
            #endregion
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
