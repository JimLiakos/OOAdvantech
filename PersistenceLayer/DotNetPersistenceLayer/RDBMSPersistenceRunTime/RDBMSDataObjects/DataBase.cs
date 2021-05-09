//using OOAdvantech.MetaDataLoadingSystem;
using OOAdvantech.Transactions;
using OOAdvantech.DotNetMetaDataRepository;
using System.Collections.Generic;
namespace OOAdvantech.RDBMSDataObjects
{
    /// <MetaDataID>{447E314D-79B8-4FF3-9694-A8DC0C9A173F}</MetaDataID>
    public abstract class DataBase : MetaDataRepository.Namespace
    {
        /// <MetaDataID>{859948ad-169f-4ad4-b063-28659baf466a}</MetaDataID>
        public static RDBMSDataObjects.DataBase GetDataBase(string connectionString,string rdbmsType)
        {
            if (rdbmsType!=null&&rdbmsType.Trim().ToLower() == "mssql")
            {
                OOAdvantech.RDBMSPersistenceRunTime.StorageProvider storageProvider=OOAdvantech.AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider","")) as OOAdvantech.RDBMSPersistenceRunTime.StorageProvider;
                return storageProvider.GetDataBase(connectionString);

                

            }
            return null;
        }
        /// <MetaDataID>{0FD4D190-B595-414A-B15C-E4917C52BF50}</MetaDataID>
        protected DataBase()
        {
        }
        /// <MetaDataID>{7F41BB29-D180-42CB-B8BC-5D6EE3F593D1}</MetaDataID>
        public RDBMSMetaDataRepository.Storage Storage;
        /// <MetaDataID>{AAFA96EC-7067-4853-A9C2-FAE26FE14E27}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Collections.Generic.Set<StoreProcedure> _StoreProcedures = new OOAdvantech.Collections.Generic.Set<StoreProcedure>();
        /// <MetaDataID>{3E2CF41D-4FA3-419F-A137-033A85295845}</MetaDataID>
        public Collections.Generic.Set<StoreProcedure> StoreProcedures
        {
            get
            {
                return null;
            }
        }
        /// <MetaDataID>{1dce9a83-4349-4bef-b984-ba64a3cc42af}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                if (Storage != null)
                    return Storage.Identity;
                return base.Identity;
            }
        }
        /// <MetaDataID>{8467295E-7BB9-4ED7-BA57-F2E97B2C018C}</MetaDataID>
        void ReadDatabaseStoreProcedures()
        {

            _StoreProcedures.RemoveAll();

            foreach (string storeProcedureName in RDBMSSQLScriptGenarator.GetStoreProcedureNames())
            {
                StoreProcedure aStoreProcedure = new StoreProcedure(storeProcedureName, false);
                _StoreProcedures.Add(aStoreProcedure);
                AddOwnedElement(aStoreProcedure);
            }
           
        }
        /// <MetaDataID>{E1268E54-90A0-472B-A69D-7374B021C6CA}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Collections.Generic.Set<View> _Views = new OOAdvantech.Collections.Generic.Set<View>();
        /// <MetaDataID>{A914685A-EE8F-4207-A824-2A8BE6586A80}</MetaDataID>
        public Collections.Generic.Set<View> Views
        {
            get
            {
                if (_Views.Count == 0)
                    ReadDatabaseViews();
                return new OOAdvantech.Collections.Generic.Set<View>(_Views,OOAdvantech.Collections.CollectionAccessType.ReadOnly);


            }
        }



        /// <MetaDataID>{28EEF435-6D5C-48BB-8E67-025B70DE4CF6}</MetaDataID>
        void ReadDatabaseViews()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Views.RemoveAll();
                foreach (System.Collections.Generic.KeyValuePair<string, string> view in RDBMSSQLScriptGenarator.GetViewsNamesAndDefinition())
                {
                    if(string.IsNullOrEmpty( view.Value))
                        throw new System.Exception("Wrong view definition SQLscript");
                    View aView = new View(view.Key, view.Value);
                    _Views.Add(aView);
                    AddOwnedElement(aView);
                } 
                stateTransition.Consistent = true;
            }
        
        }



       


        /// <exclude>Excluded</exclude>
        private Collections.Generic.Set<Table> _Tables = new OOAdvantech.Collections.Generic.Set<Table>();
        /// <MetaDataID>{F2072FC4-F45B-48EF-9A71-01AF09579D54}</MetaDataID>
        public Collections.Generic.Set<Table> Tables
        {
            get
            {
                if (_Tables.Count == 0)
                    ReadDatabaseTables();
                return new OOAdvantech.Collections.Generic.Set<Table>(_Tables,OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }






        /// <MetaDataID>{E4178578-681F-49F5-8069-DF78A2BD43A6}</MetaDataID>
        void ReadDatabaseTables()
        {
            //Table ll=new Table("","",false,this);

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Tables.RemoveAll();

                foreach (string tableName in RDBMSSQLScriptGenarator.GetTablesNames())
                {

                    Table aTable = new Table(tableName, false, this);
                    _Tables.Add(aTable);
                    AddOwnedElement(aTable);
                }

                stateTransition.Consistent = true;
            }
        

        }
        /// <MetaDataID>{0B45A051-6C29-4D8D-8D41-B2D7D36A0F19}</MetaDataID>
        public Table GetTable(string TableName)
        {
            foreach (Table CurrTable in Tables)
            {
                if (CurrTable.Name.ToLower() == TableName.ToLower())
                    return CurrTable;
            }
            return null;
        }
   

        /// <MetaDataID>{BD0E6EDA-ADDD-4EB5-86B7-80D119BC257B}</MetaDataID>
        void ManyToManyTransferRelationData(RDBMSMetaDataRepository.StorageCellsLink storageCellsLink)
        {
            RDBMSMetaDataRepository.Table roleATable = null;
            RDBMSMetaDataRepository.Table roleBTable = null;
            RDBMSMetaDataRepository.Table relationTable = null;
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> roleBColumns = (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.RoleBStorageCell);
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns = (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.RoleAStorageCell);

            #region Data transfer  preparation
            foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
            {
                roleATable = roleAColumn.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }
            foreach (RDBMSMetaDataRepository.IdentityColumn roleBColumn in roleBColumns)
            {
                roleBTable = roleBColumn.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }
            if (roleATable == null)
                roleATable = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;

            if (roleBTable == null)
                roleBTable = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;
            relationTable = storageCellsLink.ObjectLinksTable;

            Table dbRelationTable = GetTable(relationTable.Name);
            if (dbRelationTable == null)
            {
                Table theNewTable = new Table(relationTable.Name, true, this);
                _Tables.Add(theNewTable);
                AddOwnedElement(theNewTable);
                theNewTable.Synchronize(relationTable);
                theNewTable.Update();
            }
            #endregion


           System.Collections.Generic.List<string> transferDataSQLScripts = CreateManyToManyTransferDataSQLStatement(storageCellsLink, relationTable, roleBColumns, roleAColumns);

            #region Runs SQL command to transfer data
           foreach (string SLQScript in transferDataSQLScripts)
           {
               System.Diagnostics.Debug.WriteLine(SLQScript);
               var transferCommand = Connection.CreateCommand(); // new System.Data.SqlClient.SqlCommand(transferDataSQLStatement, Connection);
               transferCommand.CommandText = SLQScript;
               object resault = transferCommand.ExecuteNonQuery();
           }
            #endregion


            #region Drop old foreign keys and add the new foreign keys
            Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> referenceColumns = null;
            if (roleAColumns.Count == 0)
                referenceColumns = roleBColumns;
            else
                referenceColumns = roleAColumns;

            foreach (RDBMSMetaDataRepository.Key foreignKey in (referenceColumns[0].Namespace as RDBMSMetaDataRepository.Table).ForeignKeys)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn referenceColumn in referenceColumns)
                {
                    if (foreignKey.Columns.Contains(referenceColumn))
                    {
                        (referenceColumns[0].Namespace as RDBMSMetaDataRepository.Table).RemoveForeignKey(foreignKey);
                        break;
                    }
                }
            }
            storageCellsLink.UpdateForeignKeys();
            #endregion



        }

        /// <MetaDataID>{6ebd4d91-7508-4150-b86f-eea92b0df3e2}</MetaDataID>
        private System.Collections.Generic.List<string> CreateManyToManyTransferDataSQLStatement(OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink storageCellsLink, OOAdvantech.RDBMSMetaDataRepository.Table relationTable, OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> roleBColumns, OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn> roleAColumns)
        {
            string transferDataSQLStatement = null;
            string insertClause = null;
            string selectClause = null;
            string fromClause = null;

            #region Build INSERT and SELECT clause of transfer data SQL statement.
            RDBMSMetaDataRepository.Column roleAIndexerColumn = (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(storageCellsLink.RoleAStorageCell, "");
            RDBMSMetaDataRepository.Column roleBIndexerColumn = (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(storageCellsLink.RoleBStorageCell, "");
            foreach (RDBMSMetaDataRepository.IdentityColumn relationTableColumn in (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationTable))
            {
                
                if (insertClause == null)
                    insertClause = @"INSERT INTO " + RDBMSSQLScriptGenarator.GetSQLScriptForName(relationTable.DataBaseTableName) + " (";
                else
                    insertClause += ",";

                insertClause += RDBMSSQLScriptGenarator.GetSQLScriptForName( relationTableColumn.DataBaseColumnName );
                if (selectClause == null)
                    selectClause = "\nSELECT ";
                else
                    selectClause += ",";
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == relationTableColumn.ColumnType)
                    {
                        foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                        {
                            if (viewColumn.RealColumn == identityColumn)
                            {
                                selectClause += RDBMSSQLScriptGenarator.GetSQLScriptForName( (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.DataBaseViewName) + "." +RDBMSSQLScriptGenarator.GetSQLScriptForName( viewColumn.DataBaseColumnName );

                                break;
                            }
                        }
                        break;
                    }
                }

            }

            foreach (RDBMSMetaDataRepository.IdentityColumn relationTableColumn in (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationTable))
            {
                insertClause += "," + RDBMSSQLScriptGenarator.GetSQLScriptForName(relationTableColumn.DataBaseColumnName) ;

                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                {
                    if (identityColumn.ColumnType == relationTableColumn.ColumnType)
                    {
                        foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                        {
                            if (viewColumn.RealColumn == identityColumn)
                            {
                                selectClause += "," + RDBMSSQLScriptGenarator.GetSQLScriptForName((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.DataBaseViewName) + "." + RDBMSSQLScriptGenarator.GetSQLScriptForName(viewColumn.DataBaseColumnName);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if (roleAIndexerColumn != null)
            {
                foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                {
                    if (viewColumn.RealColumn == roleAIndexerColumn)
                    {
                        selectClause += "," + RDBMSSQLScriptGenarator.GetSQLScriptForName((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.DataBaseViewName) + "." + RDBMSSQLScriptGenarator.GetSQLScriptForName(viewColumn.DataBaseColumnName);
                        break;
                    }
                }
                RDBMSMetaDataRepository.Column column = (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(relationTable);
                insertClause += "," + RDBMSSQLScriptGenarator.GetSQLScriptForName(column.DataBaseColumnName);
            }
            if (roleBIndexerColumn != null)
            {
                foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                {
                    if (viewColumn.RealColumn == roleBIndexerColumn)
                    {
                        selectClause += "," + RDBMSSQLScriptGenarator.GetSQLScriptForName((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.DataBaseViewName) + "." + RDBMSSQLScriptGenarator.GetSQLScriptForName(viewColumn.DataBaseColumnName );
                        break;
                    }
                }
                RDBMSMetaDataRepository.Column column = (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetIndexerColumnFor(relationTable);
                insertClause += "," + RDBMSSQLScriptGenarator.GetSQLScriptForName(column.DataBaseColumnName);
            }

            insertClause += ")";
            #endregion


            #region Build FROM clause of transfer data SQL statement.
            Collections.Generic.Set<RDBMSMetaDataRepository.Column> referenceColumns = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>();
            RDBMSMetaDataRepository.View referenceColumnsTable = null;
            RDBMSMetaDataRepository.StorageCell tableWithPrimaryKeyColumns = null;
            if (roleAColumns.Count == 0)
            {
                tableWithPrimaryKeyColumns = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell);
                referenceColumnsTable = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView;
                foreach (RDBMSMetaDataRepository.Column roleBColumn in roleBColumns)
                {
                    foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                    {
                        if (viewColumn.RealColumn == roleBColumn)
                        {
                            referenceColumns.Add(viewColumn);
                            break;
                        }
                    }
                }
            }
            else
            {

                tableWithPrimaryKeyColumns = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell);
                referenceColumnsTable = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView;
                foreach (RDBMSMetaDataRepository.Column roleAColumn in roleAColumns)
                {
                    foreach (RDBMSMetaDataRepository.Column viewColumn in (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ClassView.ViewColumns)
                    {
                        if (viewColumn.RealColumn == roleAColumn)
                        {
                            referenceColumns.Add(viewColumn);
                            break;
                        }
                    }
                }
            }


            foreach (RDBMSMetaDataRepository.Column referenceColumn in referenceColumns)
            {
                //referenceColumnsTable = referenceColumn.Namespace as RDBMSMetaDataRepository.View;
                if (fromClause == null)
                    fromClause = "\nFROM " + RDBMSSQLScriptGenarator.GetSQLScriptForName(tableWithPrimaryKeyColumns.ClassView.DataBaseViewName) +  " INNER JOIN " + RDBMSSQLScriptGenarator.GetSQLScriptForName(referenceColumnsTable.DataBaseViewName) + " ON ";
                else
                    fromClause += ",";

                fromClause += RDBMSSQLScriptGenarator.GetSQLScriptForName((referenceColumnsTable).DataBaseViewName )+ "." + RDBMSSQLScriptGenarator.GetSQLScriptForName(referenceColumn.DataBaseColumnName) + " = ";
                foreach (RDBMSMetaDataRepository.IdentityColumn identityColumn in tableWithPrimaryKeyColumns.MainTable.ObjectIDColumns)
                {
                    foreach (RDBMSMetaDataRepository.Column viewColumn in tableWithPrimaryKeyColumns.ClassView.ViewColumns)
                    {
                        if (identityColumn.ColumnType == (referenceColumn.RealColumn as RDBMSMetaDataRepository.IdentityColumn).ColumnType && viewColumn.RealColumn != null && viewColumn.RealColumn == identityColumn)
                        {
                            fromClause += RDBMSSQLScriptGenarator.GetSQLScriptForName( tableWithPrimaryKeyColumns.ClassView.DataBaseViewName) + "." + RDBMSSQLScriptGenarator.GetSQLScriptForName(viewColumn.DataBaseColumnName) ;
                            break;
                        }
                    }
                }
            }
            #endregion

            transferDataSQLStatement = insertClause + selectClause + fromClause;
            return new System.Collections.Generic.List<string>() { transferDataSQLStatement };
        }

        /// <MetaDataID>{4BAA8901-CB1D-4B9F-88D3-B5E43968AB16}</MetaDataID>
        void OneToManyTransferRelationData(RDBMSMetaDataRepository.StorageCellsLink storageCellsLink)
        {
            //Transfer data from OnetoOne RoleA columns to OneToMany RoleBcolumns
            RDBMSMetaDataRepository.Table roleATable = null;
            RDBMSMetaDataRepository.Table roleBTable = null;
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> roleBColumns = (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.RoleBStorageCell);
            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> roleAColumns = (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.RoleAStorageCell);

            #region Data transfer  preparation

            foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
            {
                roleATable = roleAColumn.Namespace as RDBMSMetaDataRepository.Table;
                break;
            }
            foreach (RDBMSMetaDataRepository.IdentityColumn roleBColumn in roleBColumns)
            {
                roleBTable = roleBColumn.Namespace as RDBMSMetaDataRepository.Table;
                Table dbTable = GetTable((roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName);
                dbTable.Synchronize(roleBColumn.Namespace);
                dbTable.Update();
                break;
            }
            #endregion


             System.Collections.Generic.List<string> transferSQLScripts =RDBMSSQLScriptGenarator. CreateOneToManyTransferDataSQLStatement(roleATable, roleBTable, roleBColumns, roleAColumns);

            #region Execute data transfer statament
             foreach (string transferSQLScript in transferSQLScripts)
             {
                 var tranferDataCommand = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(transferQuery, Connection);
                 tranferDataCommand.CommandText = transferSQLScript;
                 object resault = tranferDataCommand.ExecuteNonQuery();
             }
            #endregion

            #region Remove unused foreign key and build new foreign key
            foreach (RDBMSMetaDataRepository.Key foreignKey in roleATable.ForeignKeys)
            {
                foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
                {
                    if (foreignKey.Columns.Contains(roleAColumn))
                    {
                        roleATable.RemoveForeignKey(foreignKey);
                        break;
                    }
                }
            }
            storageCellsLink.UpdateForeignKeys();
            #endregion

        }

        //public List<string> CreateOneToManyTransferDataSQLStatement(RDBMSMetaDataRepository.Table roleATable, RDBMSMetaDataRepository.Table roleBTable, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleBColumns, Collections.Generic.Set<RDBMSMetaDataRepository.IdentityColumn> roleAColumns)
        //{
        //    #region Build data transfer SQL statement
        //    string transferQuery = null;
        //    string fromClause = null;
        //    foreach (RDBMSMetaDataRepository.IdentityColumn roleBColumn in roleBColumns)
        //    {
        //        if (transferQuery == null)
        //            transferQuery += "UPDATE " + RDBMSSQLScriptGenarator.GetSQLScriptForName((roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName) + " \r\nset ";
        //        else
        //            transferQuery += ",";
        //        foreach (RDBMSMetaDataRepository.IdentityColumn roleAObjectIDColumn in roleATable.ObjectIDColumns)
        //        {
        //            if (roleBColumn.ColumnType == roleAObjectIDColumn.ColumnType)
        //                transferQuery += RDBMSSQLScriptGenarator.GetSQLScriptForName((roleBColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName)  + "." +RDBMSSQLScriptGenarator.GetSQLScriptForName( roleBColumn.DataBaseColumnName) + " = " +
        //                    RDBMSSQLScriptGenarator.GetSQLScriptForName(roleAObjectIDColumn.Namespace.Name) + "." + RDBMSSQLScriptGenarator.GetSQLScriptForName(roleAObjectIDColumn.DataBaseColumnName);
        //        }
        //    }

        //    foreach (RDBMSMetaDataRepository.IdentityColumn roleAColumn in roleAColumns)
        //    {
        //        if (fromClause == null)
        //            fromClause += "\nFROM " +RDBMSSQLScriptGenarator.GetSQLScriptForName( roleATable.Name) + " WHERE " + RDBMSSQLScriptGenarator.GetSQLScriptForName(roleBTable.Name) + " ";
        //        else
        //            fromClause += " AND ";
        //        foreach (RDBMSMetaDataRepository.IdentityColumn roleBObjectIDColumn in roleBTable.ObjectIDColumns)
        //        {
        //            if (roleAColumn.ColumnType == roleBObjectIDColumn.ColumnType)
        //                fromClause += RDBMSSQLScriptGenarator.GetSQLScriptForName( (roleAColumn.Namespace as RDBMSMetaDataRepository.Table).DataBaseTableName) + "." + RDBMSSQLScriptGenarator.GetSQLScriptForName(roleAColumn.DataBaseColumnName) + " = " +
        //                    RDBMSSQLScriptGenarator.GetSQLScriptForName(roleBObjectIDColumn.Namespace.Name) + "." + RDBMSSQLScriptGenarator.GetSQLScriptForName(roleBObjectIDColumn.DataBaseColumnName);
        //        }
        //    }
        //    transferQuery += fromClause;
        //    #endregion
        //    return new List<string>() { transferQuery };
        //}



        /// <MetaDataID>{b2fe7e23-4c36-41e8-9e71-cc870232836c}</MetaDataID>
        void UpdateRefenceCount(RDBMSMetaDataRepository.StorageCell storageCell, RDBMSMetaDataRepository.StorageCellsLink storageCellsLink)
        {

            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> referenceColums = null;
            RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd = null;
            if (storageCell == storageCellsLink.RoleAStorageCell && storageCellsLink.RoleBStorageCell.Type.HasReferentialIntegrity(storageCellsLink.Type.RoleA))
                storageCellAssociationEnd = storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd;

            if (storageCell == storageCellsLink.RoleBStorageCell && storageCellsLink.RoleAStorageCell.Type.HasReferentialIntegrity(storageCellsLink.Type.RoleB))
                storageCellAssociationEnd = storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd;
            if (storageCellAssociationEnd == null)
                return;



            System.Collections.Generic.List<string> updateQueries =new System.Collections.Generic.List<string>();
            if (storageCellsLink.Type.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany && storageCellsLink.Type.LinkClass == null)
            {
                RDBMSMetaDataRepository.Table sourceTable = storageCell.MainTable;
                Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> sourceTableColums = (storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable);
                RDBMSMetaDataRepository.Table referecneTable = null;
                Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> referecneTableColums = storageCellAssociationEnd.GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable);
                if (storageCellAssociationEnd.IsRoleA)
                    referecneTable = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;
                else
                    referecneTable = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;

                updateQueries.AddRange(RDBMSSQLScriptGenarator.BuildManyManyRelationshipReferenceCountCommand(sourceTable, sourceTableColums, referecneTableColums, referecneTable));


            }
            else if (storageCellsLink.Type.LinkClass != null)
            {
                foreach (RDBMSMetaDataRepository.StorageCell relationObjectsStorageCell in storageCellsLink.AssotiationClassStorageCells)
                {
                  

                    RDBMSMetaDataRepository.Table sourceTable = storageCell.MainTable;
                    Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> sourceTableColums = (storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationObjectsStorageCell);
                    RDBMSMetaDataRepository.Table referecneTable = null;
                    Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> referecneTableColums = storageCellAssociationEnd.GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable);
                    if (storageCellAssociationEnd.IsRoleA)
                        referecneTable = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;
                    else
                        referecneTable = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;

                   // updateQueries.AddRange(RDBMSSQLScriptGenarator.BuildManyManyRelationshipReferenceCountCommand(sourceTable, sourceTableColums, referecneTableColums, referecneTable));
                }
            }
            else if ((storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityType(storageCellAssociationEnd.GetOtherEnd().Specification)!=null)
            {
                if (storageCellAssociationEnd.IsRoleA)
                    updateQueries.AddRange(RDBMSSQLScriptGenarator.BuildManyRelationshipReferenceCountCommand(storageCell.MainTable, (storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.RoleBStorageCell), storageCellAssociationEnd.GetOtherEnd()));
                else
                    updateQueries.AddRange(RDBMSSQLScriptGenarator.BuildManyRelationshipReferenceCountCommand(storageCell.MainTable, (storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.RoleAStorageCell), storageCellAssociationEnd.GetOtherEnd()));
            }
            else if ((storageCellAssociationEnd).GetReferenceColumnsFor(storageCell).Count > 0)
                updateQueries.AddRange(RDBMSSQLScriptGenarator.BuildOneRelationshipReferenceCountCommand(storageCell, storageCellAssociationEnd));

            foreach (string updateQuery in updateQueries)
            {
                var referenceCountCommand = Connection.CreateCommand();//new System.Data.SqlClient.SqlCommand(updateQuery, Connection);
                referenceCountCommand.CommandText = updateQuery;
                object resault = referenceCountCommand.ExecuteNonQuery();
            }
        }


        /// <MetaDataID>{54E1B4FA-0EF6-403A-AA2D-E7FAE9A56E94}</MetaDataID>
        void UpdateRelations()
        {
            string Query = "SELECT storageCellsLink FROM " + typeof(RDBMSMetaDataRepository.StorageCellsLink).FullName + " storageCellsLink";
            Collections.StructureSet structureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(Storage).Execute(Query);

            foreach (Collections.StructureSet Rowset in structureSet)
            {
                RDBMSMetaDataRepository.StorageCellsLink storageCellsLink = (RDBMSMetaDataRepository.StorageCellsLink)Rowset["storageCellsLink"];
                if (storageCellsLink.RoleAMultiplicityIsMany != storageCellsLink.Type.RoleA.Multiplicity.IsMany ||
                    storageCellsLink.RoleBMultiplicityIsMany != storageCellsLink.Type.RoleB.Multiplicity.IsMany)
                {
                    if (storageCellsLink.MultiplicityType == MetaDataRepository.AssociationType.OneToOne
                        && storageCellsLink.Type.MultiplicityType == MetaDataRepository.AssociationType.OneToMany)
                        OneToManyTransferRelationData(storageCellsLink);
                    else if ((storageCellsLink.MultiplicityType == MetaDataRepository.AssociationType.OneToOne ||
                        storageCellsLink.MultiplicityType == MetaDataRepository.AssociationType.OneToMany ||
                        storageCellsLink.MultiplicityType == MetaDataRepository.AssociationType.ManyToOne) &&
                        storageCellsLink.Type.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany)
                        ManyToManyTransferRelationData(storageCellsLink);

                    (storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).RemoveUnusedReferenceColums();
                    (storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).RemoveUnusedReferenceColums();

                }
            }

            foreach (MetaDataRepository.MetaObject metaobject in Storage.OwnedElements)
            {
                if (metaobject is RDBMSMetaDataRepository.StorageCell)
                {
                    RDBMSMetaDataRepository.StorageCell storageCell = metaobject as RDBMSMetaDataRepository.StorageCell;
                    if (storageCell.NeededReferencialIntegrityUpdate && storageCell.Type.HasReferentialIntegrityRelations())
                    {
                        //System.Collections.ArrayList referenceCountUpdates = new System.Collections.ArrayList();
                        Table dbTable = GetTable(storageCell.MainTable.DataBaseTableName);

                        dbTable.Synchronize(storageCell.MainTable);
                        dbTable.Update();
                        string resetReferenceCountStatament=null;
                        resetReferenceCountStatament = GetResetReferenceCountStatement(storageCell.MainTable.Name);
                        var referenceCountCommand =Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(, Connection);
                        referenceCountCommand.CommandText = resetReferenceCountStatament;
                        object resault = referenceCountCommand.ExecuteNonQuery();
                        foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in storageCell.GetStorageCellsLinksWithRefIntegrity())
                            UpdateRefenceCount(storageCell, storageCellsLink);
                    }
                }
            }
            foreach (Collections.StructureSet Rowset in structureSet)
            {
                RDBMSMetaDataRepository.StorageCellsLink storageCellsLink = (RDBMSMetaDataRepository.StorageCellsLink)Rowset["storageCellsLink"];
                storageCellsLink.UpdateRolesSate();
            }

        }

        /// <MetaDataID>{5558980a-9202-4e81-a29c-fc384413be7b}</MetaDataID>
        private static string GetResetReferenceCountStatement(string tableName)
        {
            return string.Format("UPDATE {0} SET   ReferenceCount = 0", tableName);
        }

        /// <exclude>Excluded</exclude>
        public OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection _Connection;
        /// <MetaDataID>{4a72d2ba-6a04-4915-a0c7-145a8857f3d8}</MetaDataID>
        RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage AdoNetObjectStorage;
        /// <MetaDataID>{53FEAF46-E458-40C0-9630-A5C08F86FAA5}</MetaDataID>
        public OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection Connection
        {
            get
            {
                
                if (Storage==null&&_Connection != null)
                    return _Connection;
                if(AdoNetObjectStorage==null&&Storage!=null)
                    AdoNetObjectStorage = (PersistenceLayer.ObjectStorage.GetStorageOfObject(Storage) as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage);
                if (AdoNetObjectStorage != null)
                    return AdoNetObjectStorage.Connection;
                else
                    return null;
            }

        }
        public OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection GetConnectionFor(PersistenceLayerRunTime.TransactionContext theTransaction)
        {
            if (AdoNetObjectStorage == null)
                AdoNetObjectStorage = (PersistenceLayer.ObjectStorage.GetStorageOfObject(Storage) as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage);
            return AdoNetObjectStorage.GetConnectionFor(theTransaction);
        }
        /// <MetaDataID>{ef225fbf-9ca6-4c4e-acc4-86309c1fada5}</MetaDataID>
        public OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection SchemaUpdateConnection
        {
            get
            {

                if (AdoNetObjectStorage == null)
                    AdoNetObjectStorage = (PersistenceLayer.ObjectStorage.GetStorageOfObject(Storage) as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage);
                return AdoNetObjectStorage.SchemaUpdateConnection;
            }

        }

        /// <MetaDataID>{b4d34b36-33cc-41b8-b3aa-15af413ad731}</MetaDataID>
        public virtual RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get
            {
                return (PersistenceLayer.ObjectStorage.GetStorageOfObject(Storage) as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).TypeDictionary;
            }
        }

        /// <MetaDataID>{54233eed-04bf-431a-84f6-8156f485b04f}</MetaDataID>
        public void Update(System.Collections.Generic.List<MetaDataRepository.StorageCell> storageCells, System.Collections.Generic.List<MetaDataRepository.StorageCellsLink> storageCellsLinks)
        {
            Collections.Generic.Set<RDBMSMetaDataRepository.Table> NewTables = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.Table>();
            Collections.Generic.Set<RDBMSMetaDataRepository.View> NewViews = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.View>();


            foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in storageCellsLinks)
            {
                if (!(storageCellsLink.RoleAStorageCell is RDBMSMetaDataRepository.StorageCellReference) && !storageCells.Contains(storageCellsLink.RoleAStorageCell))
                    storageCells.Add(storageCellsLink.RoleAStorageCell);
                if (!(storageCellsLink.RoleBStorageCell is RDBMSMetaDataRepository.StorageCellReference) && !storageCells.Contains(storageCellsLink.RoleBStorageCell))
                    storageCells.Add(storageCellsLink.RoleBStorageCell);
                foreach (MetaDataRepository.StorageCell storageCell in storageCellsLink.AssotiationClassStorageCells)
                {
                    if (!(storageCell is RDBMSMetaDataRepository.StorageCellReference) && !storageCells.Contains(storageCell))
                        storageCells.Add(storageCell);

                }
                if (storageCellsLink.ObjectLinksTable != null)
                {
                    RDBMSMetaDataRepository.Table AssTable = storageCellsLink.ObjectLinksTable;
                    if (!NewTables.Contains(AssTable))
                        NewTables.Add(AssTable);
                }
            }
            foreach (MetaDataRepository.StorageCell storageCell in storageCells)
            {
                if (storageCell is RDBMSMetaDataRepository.StorageCell && !NewTables.Contains((storageCell as RDBMSMetaDataRepository.StorageCell).MainTable))
                    NewTables.Add((storageCell as RDBMSMetaDataRepository.StorageCell).MainTable);
                if (storageCell is RDBMSMetaDataRepository.StorageCell && !NewViews.Contains((storageCell as RDBMSMetaDataRepository.StorageCell).ClassView))
                    NewViews.Add((storageCell as RDBMSMetaDataRepository.StorageCell).ClassView);
            }


            ReadDatabaseTables();

            MetaDataRepository.ContainedItemsSynchronizer tablesSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(NewTables, _Tables, this);
            tablesSynchronizer.FindModifications();
            foreach (MetaDataRepository.AddCommand CurrAddCommand in tablesSynchronizer.AddedObjectsCommands)
            {
                Table theNewTable = new Table(CurrAddCommand.MissingMetaObject.Name, true, this);
                _Tables.Add(theNewTable);
                AddOwnedElement(theNewTable);

            }
            tablesSynchronizer.Synchronize();
            System.Collections.Generic.List<Table> tablesForUpdate = new System.Collections.Generic.List<Table>();
            foreach (Table currTable in _Tables)
            {
                foreach (RDBMSMetaDataRepository.Table rdbmsTable in NewTables)
                {
                    if (rdbmsTable.Identity == currTable.Identity)
                    {
                        if (!tablesForUpdate.Contains(currTable))
                            tablesForUpdate.Add(currTable);
                        break;
                    }
                }
            } 
            tablesSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(NewTables, tablesForUpdate, this);

            foreach (Table CurrTable in tablesForUpdate)
                CurrTable.SynchronizeKeys = true;
            tablesSynchronizer.Synchronize();
            foreach (Table CurrTable in tablesForUpdate)
                CurrTable.SynchronizeKeys = false;

            foreach (Table CurrTable in tablesForUpdate)
                CurrTable.Update();
            foreach (Table CurrTable in tablesForUpdate)
                CurrTable.UpdateTableKeys();
            foreach (Table CurrTable in tablesForUpdate)
                CurrTable.DropUnusedColumns();

            ReadDatabaseViews();
            MetaDataRepository.ContainedItemsSynchronizer viewsSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(NewViews, _Views, this);
            viewsSynchronizer.FindModifications();
            foreach (MetaDataRepository.AddCommand CurrAddCommand in viewsSynchronizer.AddedObjectsCommands)
            {
                View theNewView = new View(CurrAddCommand.MissingMetaObject.Name, true);
                _Views.Add(theNewView);
                AddOwnedElement(theNewView);
            }

            viewsSynchronizer.Synchronize();
            System.Collections.Generic.List<View> viewsForUpdate = new System.Collections.Generic.List<View>();
            foreach (View CurrView in _Views)
            {
                foreach (RDBMSMetaDataRepository.View rdbmsVew in NewViews)
                {
                    if (rdbmsVew.Identity == CurrView.Identity)
                    {
                        if (!viewsForUpdate.Contains(CurrView))
                            viewsForUpdate.Add(CurrView);
                        break;
                    }
                }
            }
            foreach (View CurrView in viewsForUpdate)
            {
                if (CurrView.Name.IndexOf("SC_") == 0)
                    CurrView.Update();
            }
        }

        /// <MetaDataID>{9367CA6D-35D7-41A6-9DE8-EF9DCA6A721B}</MetaDataID>
        public void Update()
        {

            try
            {
                if (Connection.State != RDBMSMetaDataPersistenceRunTime.ConnectionState.Open)
                    Connection.Open();
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("The storage '" + Storage.StorageName + "\\" + Name + "' can't be accessed.", Error);
            }

            //if(System.EnterpriseServices.ContextUtil.Transaction!=null)
            //    Connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);


            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
            {
                _OwnedElements.RemoveAll();
               // ReadDatabaseStoreProcedures();
                ReadDatabaseTables();

                

                //TODO:Thread safe
                try
                {

                    Collections.Generic.Set<RDBMSMetaDataRepository.Table> NewTables = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.Table>();
                    Collections.Generic.Set<RDBMSMetaDataRepository.View> NewViews = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.View>();
                    Collections.Generic.Set<RDBMSMetaDataRepository.StoreProcedure> NewStoreProcedures = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.StoreProcedure>();

                    Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();

                    //System.Collections.Hashtable ManyToManyAssociations = new System.Collections.Hashtable();
                    foreach (MetaDataRepository.Component CurrComponent in Storage.Components)
                    {
                        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName( CurrComponent.Identity.ToString()));
                        if (assembly == typeof(OOAdvantech.MetaDataRepository.Component).GetMetaData().Assembly)
                            continue;
                       
                        if (assembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), true).Length == 0)
                            continue;

                        foreach (MetaDataRepository.MetaObject CurrMetaObject in CurrComponent.Residents)
                        {
                            if (typeof(RDBMSMetaDataRepository.Interface).GetMetaData().IsInstanceOfType(CurrMetaObject))
                            {
                                RDBMSMetaDataRepository.Interface _Interface = (RDBMSMetaDataRepository.Interface)CurrMetaObject;
                                if (!_Interface.HasPersistentObjects || (_Interface.GetAttributes(true).Count == 0 && _Interface.GetAttributes(true).Count==0))
                                    continue;
                            }

                            if (CurrMetaObject is MetaDataRepository.Classifier)
                            {
                                foreach (MetaDataRepository.AssociationEnd associationEnd in (CurrMetaObject as MetaDataRepository.Classifier).GetAssociateRoles(true))
                                {

                                    foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellLink in ((RDBMSMetaDataRepository.Association)associationEnd.Association).ObjectLinksStorages)
                                    {

                                        if (storageCellLink.ObjectLinksTable!=null)
                                        {
                                            RDBMSMetaDataRepository.Table AssTable = storageCellLink.ObjectLinksTable;
                                            if (!NewTables.Contains(AssTable))
                                            {
                                                string rr = AssTable.Name;
                                                NewTables.Add(AssTable);
                                            }
                                        }
                                    }

                                }
                            }

                            if (CurrMetaObject is RDBMSMetaDataRepository.Class)
                            {
                                RDBMSMetaDataRepository.Class CurrClass = (RDBMSMetaDataRepository.Class)CurrMetaObject;
                                if (!CurrClass.HasPersistentObjects)
                                    continue;
                                if (!CurrClass.Persistent || CurrClass.Abstract)
                                    continue;
                                bool tt = CurrClass.HistoryClass;
                                storageCells.AddRange(CurrClass.StorageCells);

                                foreach (MetaDataRepository.StorageCell storageCell in CurrClass.StorageCells)
                                {
                                    if (storageCell is RDBMSMetaDataRepository.StorageCell&& !NewViews.Contains((storageCell as RDBMSMetaDataRepository.StorageCell).ClassView))
                                        NewViews.Add((storageCell as RDBMSMetaDataRepository.StorageCell).ClassView);
                                }
                                if (CurrClass.ConcreteClassView!=null&&!NewViews.Contains(CurrClass.ConcreteClassView))
                                    NewViews.Add(CurrClass.ConcreteClassView);
                                foreach (MetaDataRepository.StorageCell storageCell in CurrClass.StorageCells)
                                {
                                    if (storageCell is RDBMSMetaDataRepository.StorageCell && !NewTables.Contains((storageCell as RDBMSMetaDataRepository.StorageCell).MainTable))
                                    {
                                        string rr = (storageCell as RDBMSMetaDataRepository.StorageCell).MainTable.Name;
                                        NewTables.Add((storageCell as RDBMSMetaDataRepository.StorageCell).MainTable);
                                    }
                                }
   
//#if!DeviceDotNet
//                                if (!NewStoreProcedures.Contains(CurrClass.ActiveStorageCell.NewStoreProcedure))
//                                    NewStoreProcedures.Add(CurrClass.ActiveStorageCell.NewStoreProcedure);
//                                if (!NewStoreProcedures.Contains(CurrClass.ActiveStorageCell.UpdateStoreProcedure))
//                                    NewStoreProcedures.Add(CurrClass.ActiveStorageCell.UpdateStoreProcedure);
//                                if (!NewStoreProcedures.Contains(CurrClass.ActiveStorageCell.DeleteStoreProcedure))
//                                    NewStoreProcedures.Add(CurrClass.ActiveStorageCell.DeleteStoreProcedure);
//#endif
                            }
                        }
                    }



                    MetaDataRepository.ContainedItemsSynchronizer TablesSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(NewTables, _Tables, this);
                    TablesSynchronizer.FindModifications();
                    foreach (MetaDataRepository.AddCommand CurrAddCommand in TablesSynchronizer.AddedObjectsCommands)
                    {
                        Table theNewTable = new Table(CurrAddCommand.MissingMetaObject.Name, true, this);
                        _Tables.Add(theNewTable);
                        AddOwnedElement(theNewTable);

                    }
                    TablesSynchronizer.Synchronize();
                    foreach (Table CurrTable in _Tables)
                        CurrTable.SynchronizeKeys = true;
                    TablesSynchronizer.Synchronize();
                    foreach (Table CurrTable in _Tables)
                        CurrTable.SynchronizeKeys = false;
                    foreach (Table CurrTable in _Tables)
                        CurrTable.Update();
                    foreach (Table CurrTable in _Tables)
                        CurrTable.UpdateTableKeys();
                    foreach (Table CurrTable in _Tables)
                        CurrTable.DropUnusedColumns();




                    ReadDatabaseViews();
                    MetaDataRepository.ContainedItemsSynchronizer ViewsSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(NewViews, _Views, this);
                    ViewsSynchronizer.FindModifications();
                    foreach (MetaDataRepository.AddCommand CurrAddCommand in ViewsSynchronizer.AddedObjectsCommands)
                    {
                        View theNewView = new View(CurrAddCommand.MissingMetaObject.Name, true);
                        _Views.Add(theNewView);
                        AddOwnedElement(theNewView);

                    }
                    ViewsSynchronizer.Synchronize();
                    System.Collections.Generic.List<View> ViewsForUpdate = new System.Collections.Generic.List<View>();
                    foreach (View CurrView in _Views)
                    {
                        if (!ViewsForUpdate.Contains(CurrView))
                            ViewsForUpdate.Add(CurrView);
                    }
                    ViewsForUpdate.Sort(new SortViews());
                    System.Collections.Generic.List<string> vewsNames = new System.Collections.Generic.List<string>();
                    foreach (View CurrView in ViewsForUpdate)
                        vewsNames.Add(CurrView.Name);
                    //int i = 0;
                    foreach (View CurrView in ViewsForUpdate)
                    {
                        if(CurrView.Name.IndexOf("SC_")==0)
                            CurrView.Update();
                    }

                    UpdateRelations();

                    TablesSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(NewTables, _Tables, this);
                    TablesSynchronizer.FindModifications();
                    foreach (MetaDataRepository.AddCommand CurrAddCommand in TablesSynchronizer.AddedObjectsCommands)
                    {
                        Table theNewTable = new Table(CurrAddCommand.MissingMetaObject.Name, true, this);
                        _Tables.Add(theNewTable);
                        AddOwnedElement(theNewTable);

                    }
                    TablesSynchronizer.Synchronize();
                    foreach (Table CurrTable in _Tables)
                        CurrTable.SynchronizeKeys = true;
                    TablesSynchronizer.Synchronize();
                    foreach (Table CurrTable in _Tables)
                        CurrTable.SynchronizeKeys = false;
                    foreach (Table CurrTable in _Tables)
                        CurrTable.Update();
                    foreach (Table CurrTable in _Tables)
                        CurrTable.UpdateTableKeys();
                    foreach (Table CurrTable in _Tables)
                        CurrTable.DropUnusedColumns();

                    ViewsSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(NewViews, _Views, this);
                    ViewsSynchronizer.FindModifications();
                    foreach (MetaDataRepository.AddCommand CurrAddCommand in ViewsSynchronizer.AddedObjectsCommands)
                    {
                        View theNewView = new View(CurrAddCommand.MissingMetaObject.Name, true);
                        _Views.Add(theNewView);
                        AddOwnedElement(theNewView);

                    }
                    ViewsSynchronizer.Synchronize();
                    ViewsForUpdate = new System.Collections.Generic.List<View>();
                    foreach (View CurrView in _Views)
                    {
                        if (!ViewsForUpdate.Contains(CurrView))
                            ViewsForUpdate.Add(CurrView);
                    }
                    ViewsForUpdate.Sort(new SortViews());
                    vewsNames = new System.Collections.Generic.List<string>();
                    foreach (View CurrView in ViewsForUpdate)
                        vewsNames.Add(CurrView.Name);
                    //int i = 0;
                    foreach (View CurrView in ViewsForUpdate)
                    {
                        if (CurrView.Name.IndexOf("SC_") == 0)
                            CurrView.Update();
                    }

                }
                finally
                {
                    //if(Connection!=null&&Connection.State==System.Data.ConnectionState.Open)
                    //Connection.Close();
                }

                int k = 1;
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{0b209bc0-80ea-4632-a9f4-0956687d2e90}</MetaDataID>
        public abstract IRDBMSSQLScriptGenarator RDBMSSQLScriptGenarator
        {
            get;
        }
        /// <MetaDataID>{FB46C297-6703-4692-9440-417CE892B10C}</MetaDataID>
        public DataBase(string name)
        {
            Name = name;
        }
        /// <MetaDataID>{df0f7508-31a9-4030-acac-29c8887b5ace}</MetaDataID>
        public DataBase(string name, OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection connection)
        {

            Name = name;
            _Connection = connection;
        }

        /// <MetaDataID>{E357C8F7-A255-4BFB-A776-1874E5DFAABE}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }


    }

    /// <MetaDataID>{11a56877-dcef-4ed6-97d1-532c8e901c90}</MetaDataID>
    public class ForeignKeyData
    {
        /// <MetaDataID>{095e6427-cf9c-4e5d-b201-cbb14c243422}</MetaDataID>
        public ForeignKeyData(string name,string tableName, string referenceTableName)
        {
            Name = name;
            TableName = tableName;
            ReferenceTableName = referenceTableName;
            ReferedColumnsNames = new System.Collections.Generic.List<string>();
            ColumnsNames = new System.Collections.Generic.List<string>();
        }
        /// <MetaDataID>{60dd62c6-67e1-4750-a398-16c01aa5663c}</MetaDataID>
        public string Name;
        /// <MetaDataID>{d555d8a8-4ff1-45eb-b9d0-d7107f7dd5c7}</MetaDataID>
        public string ReferenceTableName;
        /// <MetaDataID>{e1d1046d-0177-4d80-a724-c6ea3f50189b}</MetaDataID>
        public string TableName;
        /// <MetaDataID>{9b584fc5-efb8-4f92-b5af-ff32b4da3900}</MetaDataID>
        public System.Collections.Generic.List<string> ReferedColumnsNames;
        /// <MetaDataID>{969249b5-5ccf-4f6c-9daa-377bccd0b011}</MetaDataID>
        public System.Collections.Generic.List<string> ColumnsNames;
    }



    /// <MetaDataID>{05136708-818f-41b0-87b6-c76c9ab159c2}</MetaDataID>
    public class PrimaryKeyData
    {
        /// <MetaDataID>{a1d5bff2-71e6-4f22-8453-b393e08396bf}</MetaDataID>
        public PrimaryKeyData(string name, string tableName)
        {
            Name = name;
            TableName = tableName;
            ColumnsNames = new System.Collections.Generic.List<string>();
        }
        /// <MetaDataID>{76d645e0-ce8d-48b4-971f-76546ec319e0}</MetaDataID>
        public string Name;
        /// <MetaDataID>{dd6255a3-d2e0-49fb-bc95-adfb8f45a341}</MetaDataID>
        public string TableName;
        /// <MetaDataID>{2cfd46eb-333c-4c24-9012-cd5acbde723e}</MetaDataID>
        public System.Collections.Generic.List<string> ColumnsNames;
    }


    /// <MetaDataID>{a16f37af-7560-44f1-b91a-e176614ad7cd}</MetaDataID>
    public class ColumnDefaultValueData
    {
        /// <MetaDataID>{a7e59316-53bf-4136-8550-e6f6ad48f14a}</MetaDataID>
        public ColumnDefaultValueData(string columnName, object defaultValue)
        {
            ColumnName = columnName;
            DefaultValue = defaultValue;
        }
        /// <MetaDataID>{37825e36-7148-4b1e-9944-48cf2b830a69}</MetaDataID>
        public readonly string ColumnName;
        /// <MetaDataID>{e858d74f-30ad-4e3a-81f6-22e439331bce}</MetaDataID>
        public readonly object DefaultValue;
    }
    /// <MetaDataID>{d8b010f3-9bb6-4c63-b955-7b01621ae217}</MetaDataID>
    public class ColumnData
    {
        /// <MetaDataID>{25e09bf2-f6ad-4749-ba96-c7ce31a21de4}</MetaDataID>
        public ColumnData(string name, string typeName, int length, bool nullable, bool identity, int identityIncrement)
        {
            Name = name;
            TypeName = typeName;
            Length = length;
            Nullable = nullable;
            Identity = identity;
            IdentityIncrement = identityIncrement;
        }
        /// <MetaDataID>{2e501944-100e-430a-bb85-afa8bed06501}</MetaDataID>
        public string Name;
        /// <MetaDataID>{4ec23cf6-68d9-40c7-866b-4d3eb59375a6}</MetaDataID>
        public string TypeName;
        /// <MetaDataID>{a4f8c789-c73c-4c95-b64c-d12e64de0f18}</MetaDataID>
        public int Length;
        /// <MetaDataID>{f5ce39c0-6843-476c-869e-7fe5f5d24b77}</MetaDataID>
        public bool Nullable;
        /// <MetaDataID>{3c52bb03-17a7-4784-bfde-f2a22d997a6b}</MetaDataID>
        public bool Identity;
        /// <MetaDataID>{a82a2b1e-6aa3-409a-9f58-d9f878ea686e}</MetaDataID>
        public int IdentityIncrement;
    }


 
}


