//using OOAdvantech.MetaDataLoadingSystem;
namespace OOAdvantech.RDBMSDataObjects
{
    /// <MetaDataID>{447E314D-79B8-4FF3-9694-A8DC0C9A173F}</MetaDataID>
    public abstract class DataBase : MetaDataRepository.Namespace
    {
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

            foreach (string storeProcedureName in RDBMSSchema.GetStoreProcedureNames())
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
            _Views.RemoveAll();
            foreach (string viewName in RDBMSSchema.GetViewsNames())
            {
                View aView = new View(viewName, false);
                _Views.Add(aView);
                AddOwnedElement(aView);
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
            _Tables.RemoveAll();

            foreach (string tableName in RDBMSSchema.GetTablesNames())
            {

                Table aTable = new Table(tableName, false, this);
                _Tables.Add(aTable);
                AddOwnedElement(aTable);
            }


        }
        /// <MetaDataID>{0B45A051-6C29-4D8D-8D41-B2D7D36A0F19}</MetaDataID>
        public Table GetTable(string TableName)
        {
            foreach (Table CurrTable in Tables)
            {
                if (CurrTable.Name == TableName)
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


            string transferDataSQLStatement = RDBMSSchema.CreateManyToManyTransferDataSQLStatement(storageCellsLink, relationTable, roleBColumns, roleAColumns);

            #region Runs SQL command to transfer data

            System.Diagnostics.Debug.WriteLine(transferDataSQLStatement);
            System.Data.Common.DbCommand transferCommand = Connection.CreateCommand(); // new System.Data.SqlClient.SqlCommand(transferDataSQLStatement, Connection);
            transferCommand.CommandText = transferDataSQLStatement;
            object resault = transferCommand.ExecuteNonQuery();
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


            string transferQuery = RDBMSSchema.CreateOneToManyTransferDataSQLStatement(roleATable, roleBTable, roleBColumns, roleAColumns);

            #region Execute data transfer statament
            System.Data.Common.DbCommand tranferDataCommand = Connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(transferQuery, Connection);
            tranferDataCommand.CommandText = transferQuery;
            object resault = tranferDataCommand.ExecuteNonQuery();
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

    


        /// <MetaDataID>{b2fe7e23-4c36-41e8-9e71-cc870232836c}</MetaDataID>
        void UpdateRefenceCount(RDBMSMetaDataRepository.StorageCell storageCell, RDBMSMetaDataRepository.StorageCellsLink storageCellsLink)
        {

            Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> referenceColums = null;
            RDBMSMetaDataRepository.AssociationEnd storageCellAssociationEnd = null;
            if (storageCell == storageCellsLink.RoleAStorageCell && storageCell.Type.HasReferentialIntegrity(storageCellsLink.Type.RoleA))
                storageCellAssociationEnd = storageCellsLink.Type.RoleA as RDBMSMetaDataRepository.AssociationEnd;

            if (storageCell == storageCellsLink.RoleBStorageCell && storageCell.Type.HasReferentialIntegrity(storageCellsLink.Type.RoleB))
                storageCellAssociationEnd = storageCellsLink.Type.RoleB as RDBMSMetaDataRepository.AssociationEnd;
            if (storageCellAssociationEnd == null)
                return;



            string updateQuery = null;
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

                updateQuery += RDBMSSchema.BuildManyManyRelationshipReferenceCountCommand(sourceTable, sourceTableColums, referecneTableColums, referecneTable);


            }
            else if (storageCellsLink.Type.LinkClass != null)
            {
                foreach (RDBMSMetaDataRepository.StorageCell relationObjectsStorageCell in storageCellsLink.AssotiationClassStorageCells)
                {
                    if (updateQuery != null)
                        updateQuery += "\r\n\r\n";

                    RDBMSMetaDataRepository.Table sourceTable = storageCell.MainTable;
                    Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> sourceTableColums = (storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(relationObjectsStorageCell);
                    RDBMSMetaDataRepository.Table referecneTable = null;
                    Collections.Generic.Set <RDBMSMetaDataRepository.IdentityColumn> referecneTableColums = storageCellAssociationEnd.GetReferenceColumnsFor(storageCellsLink.ObjectLinksTable);
                    if (storageCellAssociationEnd.IsRoleA)
                        referecneTable = (storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;
                    else
                        referecneTable = (storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).MainTable;

                    updateQuery += RDBMSSchema.BuildManyManyRelationshipReferenceCountCommand(sourceTable, sourceTableColums, referecneTableColums, referecneTable);
                }
            }
            else if ((storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumns(storageCellAssociationEnd.GetOtherEnd().Specification).Count > 0)
            {
                if (storageCellAssociationEnd.IsRoleA)
                    updateQuery = RDBMSSchema.BuildManyRelationshipReferenceCountCommand(storageCell.MainTable, (storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.RoleBStorageCell), storageCellAssociationEnd.GetOtherEnd());
                else
                    updateQuery = RDBMSSchema.BuildManyRelationshipReferenceCountCommand(storageCell.MainTable, (storageCellAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(storageCellsLink.RoleAStorageCell), storageCellAssociationEnd.GetOtherEnd());
            }
            else if ((storageCellAssociationEnd).GetReferenceColumnsFor(storageCell).Count > 0)
                updateQuery = RDBMSSchema.BuildOneRelationshipReferenceCountCommand(storageCell, storageCellAssociationEnd);

            System.Data.Common.DbCommand referenceCountCommand = Connection.CreateCommand();//new System.Data.SqlClient.SqlCommand(updateQuery, Connection);
            referenceCountCommand.CommandText = updateQuery;
            object resault = referenceCountCommand.ExecuteNonQuery();



        }


        /// <MetaDataID>{54E1B4FA-0EF6-403A-AA2D-E7FAE9A56E94}</MetaDataID>
        void UpdateRelations()
        {
            string Query = "SELECT storageCellsLink FROM " + typeof(RDBMSMetaDataRepository.StorageCellsLink).FullName + " storageCellsLink";
            Collections.StructureSet structureSet = PersistenceLayer.ObjectStorage.GetStorageOfObject(Storage).Execute(Query);

            foreach (Collections.StructureSet Rowset in structureSet)
            {
                RDBMSMetaDataRepository.StorageCellsLink storageCellsLink = (RDBMSMetaDataRepository.StorageCellsLink)Rowset.Members["storageCellsLink"].Value;
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
                        System.Collections.ArrayList referenceCountUpdates = new System.Collections.ArrayList();
                        Table dbTable = GetTable(storageCell.MainTable.DataBaseTableName);

                        dbTable.Synchronize(storageCell.MainTable);
                        dbTable.Update();
                        string resetReferenceCountStatament=null;
                        resetReferenceCountStatament = GetResetReferenceCountStatement(storageCell.MainTable.Name);
                        System.Data.Common.DbCommand referenceCountCommand =Connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(, Connection);
                        referenceCountCommand.CommandText = resetReferenceCountStatament;
                        object resault = referenceCountCommand.ExecuteNonQuery();
                        foreach (RDBMSMetaDataRepository.StorageCellsLink storageCellsLink in storageCell.GetStorageCellsLinksWithRefIntegrity())
                            UpdateRefenceCount(storageCell, storageCellsLink);
                    }
                }
            }
            foreach (Collections.StructureSet Rowset in structureSet)
            {
                RDBMSMetaDataRepository.StorageCellsLink storageCellsLink = (RDBMSMetaDataRepository.StorageCellsLink)Rowset.Members["storageCellsLink"].Value;
                storageCellsLink.UpdateRolesSate();
            }

        }

        /// <MetaDataID>{5558980a-9202-4e81-a29c-fc384413be7b}</MetaDataID>
        private static string GetResetReferenceCountStatement(string tableName)
        {
            return string.Format("UPDATE {0} SET   ReferenceCount = 0", tableName);
        }
        
        /// <MetaDataID>{53FEAF46-E458-40C0-9630-A5C08F86FAA5}</MetaDataID>
        public System.Data.Common.DbConnection Connection
        {
            get
            {
                return (PersistenceLayer.ObjectStorage.GetStorageOfObject(Storage) as MSSQLFastPersistenceRunTime.AdoNetObjectStorage).Connection;
            }

        }
        

        /// <MetaDataID>{9367CA6D-35D7-41A6-9DE8-EF9DCA6A721B}</MetaDataID>
        public void Update()
        {

            try
            {
                if (Connection.State != System.Data.ConnectionState.Open)
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
                ReadDatabaseStoreProcedures();
                ReadDatabaseTables();

                UpdateRelations();

                //TODO:Thread safe
                try
                {

                    Collections.Generic.Set<RDBMSMetaDataRepository.Table> NewTables = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.Table>();
                    Collections.Generic.Set<RDBMSMetaDataRepository.View> NewViews = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.View>();
                    Collections.Generic.Set<RDBMSMetaDataRepository.StoreProcedure> NewStoreProcedures = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.StoreProcedure>();

                    Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();

                    System.Collections.Hashtable ManyToManyAssociations = new System.Collections.Hashtable();
                    foreach (MetaDataRepository.Component CurrComponent in Storage.Components)
                    {
                        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(CurrComponent.Identity.ToString());
                        if (assembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), true).Length == 0)
                            continue;

                        foreach (MetaDataRepository.MetaObject CurrMetaObject in CurrComponent.Residents)
                        {
                            if (typeof(RDBMSMetaDataRepository.Interface).IsInstanceOfType(CurrMetaObject))
                            {
                                RDBMSMetaDataRepository.Interface _Interface = (RDBMSMetaDataRepository.Interface)CurrMetaObject;
                                if (!_Interface.HasPersistentObjects)
                                    continue;
                                if (!NewViews.Contains(_Interface.TypeView))
                                    NewViews.Add(_Interface.TypeView);

                            }

                            if (CurrMetaObject is MetaDataRepository.Classifier)
                            {
                                foreach (MetaDataRepository.AssociationEnd CurrAssociationEnd in (CurrMetaObject as MetaDataRepository.Classifier).GetAssociateRoles(true))
                                {

                                    if (CurrAssociationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany && CurrAssociationEnd.Association.LinkClass == null)
                                    {
                                        foreach (RDBMSMetaDataRepository.StorageCellsLink CurrObjectCollectionLink in ((RDBMSMetaDataRepository.Association)CurrAssociationEnd.Association).ObjectLinksStorages)
                                        {
                                            RDBMSMetaDataRepository.Table AssTable = CurrObjectCollectionLink.ObjectLinksTable;
                                            if (!NewTables.Contains(AssTable))
                                            {
                                                string rr = AssTable.Name;
                                                NewTables.Add(AssTable);
                                            }
                                        }
                                    }
                                }
                            }

                            if (typeof(RDBMSMetaDataRepository.Class).IsInstanceOfType(CurrMetaObject))
                            {
                                RDBMSMetaDataRepository.Class CurrClass = (RDBMSMetaDataRepository.Class)CurrMetaObject;
                                if (!CurrClass.HasPersistentObjects)
                                    continue;
                                if (!NewViews.Contains(CurrClass.TypeView))
                                    NewViews.Add(CurrClass.TypeView);

                                if (!CurrClass.Persistent || CurrClass.Abstract)
                                    continue;
                                bool tt = CurrClass.HistoryClass;


                                storageCells.AddRange(CurrClass.StorageCells);

                                if (!NewViews.Contains(CurrClass.ActiveStorageCell.ClassView))
                                    NewViews.Add(CurrClass.ActiveStorageCell.ClassView);
                                if (!NewViews.Contains(CurrClass.ConcreteClassView))
                                    NewViews.Add(CurrClass.ConcreteClassView);
                                if (!NewTables.Contains(CurrClass.ActiveStorageCell.MainTable))
                                {
                                    string rr = CurrClass.ActiveStorageCell.MainTable.Name;
                                    NewTables.Add(CurrClass.ActiveStorageCell.MainTable);
                                }
#if!NETCompactFramework
                                if (!NewStoreProcedures.Contains(CurrClass.ActiveStorageCell.NewStoreProcedure))
                                    NewStoreProcedures.Add(CurrClass.ActiveStorageCell.NewStoreProcedure);
                                if (!NewStoreProcedures.Contains(CurrClass.ActiveStorageCell.UpdateStoreProcedure))
                                    NewStoreProcedures.Add(CurrClass.ActiveStorageCell.UpdateStoreProcedure);
                                if (!NewStoreProcedures.Contains(CurrClass.ActiveStorageCell.DeleteStoreProcedure))
                                    NewStoreProcedures.Add(CurrClass.ActiveStorageCell.DeleteStoreProcedure);
#endif
                            }
                        }
                    }



                    MetaDataRepository.ContainedItemsSynchronizer TablesSynchronizer = new MetaDataRepository.ContainedItemsSynchronizer(NewTables, _Tables, this);
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
                    MetaDataRepository.ContainedItemsSynchronizer ViewsSynchronizer = new MetaDataRepository.ContainedItemsSynchronizer(NewViews, _Views, this);
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
                    foreach (View CurrView in ViewsForUpdate)
                        CurrView.Update();


                    MetaDataRepository.ContainedItemsSynchronizer StoreProceduresSynchronizer = new MetaDataRepository.ContainedItemsSynchronizer(NewStoreProcedures, _StoreProcedures, this);
                    StoreProceduresSynchronizer.FindModifications();
                    foreach (MetaDataRepository.AddCommand CurrAddCommand in StoreProceduresSynchronizer.AddedObjectsCommands)
                    {
                        StoreProcedure theNewStoreProcedure = new StoreProcedure(CurrAddCommand.MissingMetaObject.Name, true);
                        _StoreProcedures.Add(theNewStoreProcedure);
                        AddOwnedElement(theNewStoreProcedure);

                    }
                    StoreProceduresSynchronizer.Synchronize();
                    foreach (StoreProcedure CurrStoreProcedure in _StoreProcedures)
                        CurrStoreProcedure.Update();
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

        public abstract IRDBMSSchema RDBMSSchema
        {
            get;
        }
        /// <MetaDataID>{FB46C297-6703-4692-9440-417CE892B10C}</MetaDataID>
        public DataBase(string name)
        {
            Name = name;
            //_Connection=(PersistenceLayer.ObjectStorage.GetStorageOfObject(theObjectStorage) as MSSQLFastPersistenceRunTime.ObjectStorage).Connection;
        }
        /// <MetaDataID>{E357C8F7-A255-4BFB-A776-1874E5DFAABE}</MetaDataID>
        public override System.Collections.ArrayList GetExtensionMetaObjects()
        {
            return new System.Collections.ArrayList();
        }


    }

    /// <MetaDataID>{ea13652c-38ce-4b6a-bb3e-377e038bfcb4}</MetaDataID>
    public struct ForeignKeyData
    {
        public ForeignKeyData(string name, string referenceTableName)
        {
            Name = name;
            ReferenceTableName = referenceTableName;
        }
        public string Name;
        public string ReferenceTableName;

    }

    /// <MetaDataID>{9cbe9e13-f617-4a43-bb07-8291cf88c7f0}</MetaDataID>
    public struct ColumnDefaultValueData
    {
        public ColumnDefaultValueData(string columnName, object defaultValue)
        {
            ColumnName = columnName;
            DefaultValue = defaultValue;
        }
        public readonly string ColumnName;
        public readonly object DefaultValue;
    }
    /// <MetaDataID>{10ee0979-5904-484a-ab9a-cf631b706bc8}</MetaDataID>
    public struct ColumnData
    {
        public ColumnData(string name, string typeName, int length, bool nullable, bool identity, int identityIncrement)
        {
            Name = name;
            TypeName = typeName;
            Length = length;
            Nullable = nullable;
            Identity = identity;
            IdentityIncrement = identityIncrement;

        }
        public string Name;
        public string TypeName;
        public int Length;
        public bool Nullable;
        public bool Identity;
        public int IdentityIncrement;
    }

 
}


