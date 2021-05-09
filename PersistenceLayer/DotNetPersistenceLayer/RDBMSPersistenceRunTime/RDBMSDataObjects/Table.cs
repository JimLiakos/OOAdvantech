using OOAdvantech.Transactions;
//using OOAdvantech.MSSQLPersistenceRunTime;
namespace OOAdvantech.RDBMSDataObjects
{

    /// <MetaDataID>{24c4f698-6c90-40dd-bb76-b6986a250e40}</MetaDataID>
    public enum TableType
    {
        StorageTable,
        VaraibleTable,
        TemporaryTable
    }
    /// <MetaDataID>{BD46811B-3ED3-4409-8188-E28DF32DF68C}</MetaDataID>
    public class Table : MetaDataRepository.Namespace
    {

        /// <MetaDataID>{893bd900-0a4e-4472-854c-712a63831792}</MetaDataID>
        TableType TableType;


        /// <MetaDataID>{ed30e865-669b-425a-982c-991922456dc8}</MetaDataID>
        protected Table()
        {
            TableType = TableType.StorageTable;
            int tt = 0;
        }

        /// <MetaDataID>{8F46D910-268A-415B-B9ED-251C9FE8008C}</MetaDataID>
        public Column GetColumn(string ColumnName)
        {
            foreach (Column CurrColumn in Columns)
            {
                if (CurrColumn.Name.ToLower() == ColumnName.ToLower())
                    return CurrColumn;
            }
            return null;
        }

        /// <MetaDataID>{7CEDE0C1-D8E8-4769-95BC-E9517DB6223D}</MetaDataID>
        public override MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                if (Namespace != null)
                    return new OOAdvantech.MetaDataRepository.MetaObjectID(Namespace.Identity.ToString().ToLower() + "." + Name.ToString().ToLower());
                return new OOAdvantech.MetaDataRepository.MetaObjectID(base.Identity.ToString().ToLower());
                if (_Identity == null)
                    _Identity = new MetaDataRepository.MetaObjectID(Name);
                return _Identity;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{030B67E5-4175-422A-80C3-8BB46E0A51A0}</MetaDataID>
        private Collections.Generic.Set<Key> _Keys = new OOAdvantech.Collections.Generic.Set<Key>();
        /// <MetaDataID>{20AE16ED-A3F9-4112-AC58-3893CBA957C6}</MetaDataID>
        public Collections.Generic.Set<Key> Keys
        {
            get
            {
                ReadTableKeys();
                return new OOAdvantech.Collections.Generic.Set<Key>(_Keys, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }
        /// <MetaDataID>{0F76C6C3-CF51-4E3D-BBC5-2026D469FC0D}</MetaDataID>
        void ReadTableKeys()
        {
            ReadColumns();
            if (_Keys.Count == 0 && !IsNewTable)
            {
                try
                {
                    System.Collections.Generic.Dictionary<string,Key>  Name_Key_map = new System.Collections.Generic.Dictionary<string, Key>();
                    PrimaryKeyData primaryKey = (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.RetrieveDatabasePrimaryKeys(this);// new System.Collections.Generic.List<string>();
                    if (primaryKey != null)
                    {
                        string KeyName = primaryKey.Name;
                        Key pKey = null;
                        if (!Name_Key_map.ContainsKey(KeyName))
                        {
                            pKey = new Key(KeyName, true, this, false);
                            _Keys.Add(pKey);
                            AddOwnedElement(pKey);
                            Name_Key_map.Add(pKey.Name, pKey);

                            foreach (string columnName in primaryKey.ColumnsNames)
                            {
                                Column column = GetColumn(columnName);
                                pKey.Columns.Add(column);
                            }
                        }
                    }

                    foreach (ForeignKeyData foreignKeyData in (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.RetrieveDatabaseForeignKeys(this))
                    {
                        Key aKey = null;
                        if (!Name_Key_map.ContainsKey(foreignKeyData.Name))
                        {
                            Table refTable = ((DataBase)Namespace).GetTable(foreignKeyData.ReferenceTableName);
                            aKey = new Key(foreignKeyData.Name, false, this, false, refTable);
                            _Keys.Add(aKey);
                            AddOwnedElement(aKey);
                            Name_Key_map.Add(aKey.Name, aKey);
                            foreach (string columnName in foreignKeyData.ColumnsNames)
                            {
                                Column column = GetColumn(columnName);
                                aKey.Columns.Add(column);
                            }
                            foreach (string referenceColumnName in foreignKeyData.ReferedColumnsNames)
                            {
                                Column column = refTable.GetColumn(referenceColumnName);
                                aKey.ReferedColumns.Add(column);
                            }

                            var identity = aKey.Identity;
                        }
                    }
                }
                catch (System.Exception Error)
                {
                    System.Diagnostics.Debug.Assert(false);
                    throw new System.Exception("Table with name '" + Name + "'failed to read its Keys", Error);
                }
            }

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{62BD347F-5213-4C46-9521-4BAE06F14E64}</MetaDataID>
        private Collections.Generic.Set<Column> _Columns = new OOAdvantech.Collections.Generic.Set<Column>();
        /// <MetaDataID>{84A8F8A4-4C08-41FB-96F9-0F2628917318}</MetaDataID>
        public Collections.Generic.Set<Column> Columns
        {
            get
            {
                if (_Columns.Count == 0 && !IsNewTable)
                    ReadColumns();
                return new OOAdvantech.Collections.Generic.Set<Column>(_Columns, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }


        /// <MetaDataID>{44DA9307-993F-44A3-8242-FC34526743B7}</MetaDataID>
        void ReadColumns()
        {
            if (string.IsNullOrEmpty(Name))
                return;
            if (_Columns.Count == 0 && !IsNewTable)
            {
                foreach (ColumnData columnData in (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.RetreiveTableColumns(Name))
                {
                    Column column = new Column(columnData.Name, columnData.TypeName, columnData.Length, columnData.Nullable, columnData.Identity, false);
                    column.IdentityIncrement = columnData.IdentityIncrement;
                    _Columns.Add(column);
                    AddOwnedElement(column);
                }
            }
        }

        /// <MetaDataID>{319A9DA6-CAC3-4569-BC69-B4749BB31046}</MetaDataID>
        public void AddColumn(Column column)
        {
            foreach (Column CurrColumn in _Columns)
            {
                if (CurrColumn.Name == column.Name)
                    throw new System.Exception("Column with name '" + column.Name + "' already exist int table.");
            }
            _Columns.Add(column);
            AddOwnedElement(column);
        }
        ///// <MetaDataID>{53D08F69-8C93-4162-8E51-1491E9A37C09}</MetaDataID>
        //public string GetScript()
        //{
        //    return (_Namespace as DataBase).GetDataBaseUpdateScript(Name, NewColums, IsNewTable, TableType);
        //    //if (AddedColumnsScript == null)
        //    //    return null;
        //    //if (NewTable)
        //    //{
        //    //    if (TableType == TableType.VaraibleTable)
        //    //        return "CREATE TABLE @" + Name + " (\n" + AddedColumnsScript + ")";
        //    //    else if (TableType == TableType.TemporaryTable)
        //    //        return "CREATE TABLE #" + Name + " (\n" + AddedColumnsScript + ")";
        //    //    else
        //    //        return "CREATE TABLE " + Name + " (\n" + AddedColumnsScript + ")";
        //    //}
        //    //else
        //    //{
        //    //    if (TableType == TableType.VaraibleTable)
        //    //        return "ALTER TABLE @" + Name + " ADD \n" + AddedColumnsScript;
        //    //    else if (TableType == TableType.TemporaryTable)
        //    //        return "ALTER TABLE #" + Name + " ADD \n" + AddedColumnsScript;
        //    //    else
        //    //        return "ALTER TABLE " + Name + " ADD \n" + AddedColumnsScript;
        //    //}

        //}

        /// <MetaDataID>{9baba094-a5f8-4e88-b48b-46da2f213aaa}</MetaDataID>
        string ColumnsDefaultValuesSetScript
        {
            get
            {
                System.Collections.Generic.List<ColumnDefaultValueData> columnsDefaultValues = new System.Collections.Generic.List<ColumnDefaultValueData>();
                foreach (Column addedColumn in NewColums)
                {
                    if (addedColumn.OriginColumn.MappedAttribute != null)
                    {
                        DotNetMetaDataRepository.Attribute attribute = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(addedColumn.OriginColumn.MappedAttribute.Identity) as DotNetMetaDataRepository.Attribute;
                        DotNetMetaDataRepository.Class _class = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(((addedColumn.OriginColumn.Namespace as RDBMSMetaDataRepository.Table).TableCreator as RDBMSMetaDataRepository.StorageCell).Type.Identity) as DotNetMetaDataRepository.Class;
                        object defaultValue = _class.GetDefaultValue(attribute, new MetaDataRepository.ValueTypePath(addedColumn.OriginColumn.CreatorIdentity));
                        columnsDefaultValues.Add(new ColumnDefaultValueData(addedColumn.Name, defaultValue));
                    }
                }
                return (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.GetColumnsDefaultValuesSetScript(Name, columnsDefaultValues);

            }
        }
        ///// <MetaDataID>{3631b982-c44e-41fd-b7ee-69f7cc39a641}</MetaDataID>
        //string ChangeTableNameScript;
        /// <MetaDataID>{ad8416bd-3ef8-4b6b-a508-29395ed2d4ce}</MetaDataID>
        string ChangeColumnNameScript;
        /// <MetaDataID>{AC885A0B-9165-4B2D-8871-5E09EE8E6B07}</MetaDataID>
        internal bool SynchronizeKeys = false;
        /// <MetaDataID>{781A4176-B21D-41A5-B902-2B606F69D73D}</MetaDataID>
        /// <exclude>Excluded</exclude>
        //private string AddedColumnsScript;
        /// <MetaDataID>{94f44617-c498-4e5b-8bd4-80cce7a1307e}</MetaDataID>
        private Collections.Generic.Set<Key> DropedKeys = new OOAdvantech.Collections.Generic.Set<Key>();
        /// <MetaDataID>{70809bc5-fff2-4fac-a8d0-a6ead56da0f5}</MetaDataID>
        private Collections.Generic.Set<Column> DropedColumns = new OOAdvantech.Collections.Generic.Set<Column>();
        /// <MetaDataID>{efda2b7f-c95c-40cc-a553-d4003218e880}</MetaDataID>
        public bool ColumnsFormatHasChanged
        {
            get
            {
                bool updateColumnsFormat = false;
                foreach (Column column in _Columns)
                    updateColumnsFormat |= column.HasChangeColumnFormat;
                return updateColumnsFormat;
            }
        }




        /// <MetaDataID>{092ab21d-7c19-4cd0-83ef-3f4c2454244e}</MetaDataID>
        System.Collections.Generic.List<Column> NewColums = new System.Collections.Generic.List<Column>();

        /// <MetaDataID>{72CDD6D3-67E8-497E-91FB-6E6A9E5CDE36}</MetaDataID>
        public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
        {
            using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, TransactionOption.Supported))
            {
                long Count = Columns.Count;
                if (!(OriginMetaObject is RDBMSMetaDataRepository.Table))
                    throw new System.Exception("You can't synchronize with object that doesn't support 'RDBMSMetaDataRepository.Table'");
                RDBMSMetaDataRepository.Table OriginTable = OriginMetaObject as RDBMSMetaDataRepository.Table;
                ReadTableKeys();
                if (SynchronizeKeys)
                {
                    Collections.Generic.Set<RDBMSMetaDataRepository.Key> OriginKeys = new OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Key>();
                    if (OriginTable.PrimaryKey != null)
                        OriginKeys.Add(OriginTable.PrimaryKey);
                    OriginKeys.AddRange(OriginTable.ForeignKeys);
                    MetaDataRepository.ContainedItemsSynchronizer KeysSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(OriginKeys, _Keys, this);
                    KeysSynchronizer.FindModifications();
                    KeysSynchronizer.ExecuteAddCommand();
                    foreach (MetaDataRepository.DeleteCommand deleteCommand in KeysSynchronizer.DeletedObjectsCommands)
                        DropedKeys.Add(deleteCommand.CandidateForDeleteObject as Key);
                    KeysSynchronizer.ExecuteDeleteCommand();
                    KeysSynchronizer.Synchronize();
                }
                else
                {

                    Collections.Generic.Set<RDBMSMetaDataRepository.Column> OriginTableColumns = new OOAdvantech.Collections.Generic.Set<RDBMSMetaDataRepository.Column>();
                    foreach (RDBMSMetaDataRepository.Column column in OriginTable.ObjectIDColumns)
                        OriginTableColumns.Add(column);
                    if (OriginTable.ReferentialIntegrityColumn != null)
                        OriginTableColumns.Add(OriginTable.ReferentialIntegrityColumn);

                    foreach (RDBMSMetaDataRepository.Column column in OriginTable.ContainedColumns)
                        OriginTableColumns.Add(column);

                    MetaDataRepository.ContainedItemsSynchronizer ColumnsSynchronizer = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(OriginTableColumns, _Columns, this);
                    ColumnsSynchronizer.FindModifications();
                    ColumnsSynchronizer.ExecuteAddCommand();
                    foreach (MetaDataRepository.DeleteCommand deleteCommand in ColumnsSynchronizer.DeletedObjectsCommands)
                        DropedColumns.Add(deleteCommand.CandidateForDeleteObject as Column);

                    foreach (Column column in _Columns)
                        AddOwnedElement(column);

                    ColumnsSynchronizer.ExecuteDeleteCommand();

                    ColumnsSynchronizer.Synchronize();

                    foreach (MetaDataRepository.AddCommand addCommand in ColumnsSynchronizer.AddedObjectsCommands)
                    {
                        Column addedColumn = (Column)addCommand.AddedObject;
                        NewColums.Add(addedColumn);
                    }

                    foreach (Column column in _Columns)
                    {
                        (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.GetValidateColumnName(column);
                        if (column.Name.ToLower() != column.DataBaseColumnName.ToLower() && !string.IsNullOrEmpty(column.DataBaseColumnName))
                            ChangeColumnNameScript += (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.GetChangeColumnNameScript(Name, column.DataBaseColumnName, column.Name) + "\n";
                    }

                    _Name = OriginTable.Name;
                    (Namespace as DataBase).RDBMSSQLScriptGenarator.GetValidateTableName(this);
                    OriginTable.DataBaseTableName = _Name;
                }

                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{67EE5F54-1B13-44CE-BDE6-E24E0B96CD5E}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }
        /// <exclude>Excluded</exclude>
        private bool _IsNewTable = true;
        /// <MetaDataID>{3c8eae42-ff5c-4523-86cd-21c78a02bd75}</MetaDataID>
        private bool IsNewTable
        {
            get
            {
                return _IsNewTable;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _IsNewTable = value;
                    stateTransition.Consistent = true;
                }

            }
        }

        /// <MetaDataID>{D9C31236-A798-4B5A-8B2D-C8129FE3A093}</MetaDataID>
        /// <exclude>Excluded</exclude>
        //private string ConnectionString=null;
        /// <MetaDataID>{D364F7F6-5B48-4D13-82CA-FBE87C95B148}</MetaDataID>
        public Table(string name, bool newTable, DataBase dataBase)
        {
            _Namespace.Value = dataBase;
            Name = name;
            _IsNewTable = newTable;
            ReadColumns();
        }



        /// <MetaDataID>{c2ef5a76-461d-4abf-a825-d74cbed2a91d}</MetaDataID>
        public Table(string name, bool newTable, TableType tableType, DataBase dataBase)
        {
            _Namespace.Value = dataBase;
            Name = name;
            _IsNewTable = newTable;
            ReadColumns();
            TableType = tableType;
        }


        ///// <MetaDataID>{FF504503-B422-4EFB-BC19-8B67D46EFA54}</MetaDataID>
        //~Table()
        //{
        //    System.Diagnostics.Debug.WriteLine("~Table()");
        //    int hh = 0;
        //}

        /// <MetaDataID>{1a13f14f-8617-46e7-b669-5d00de84dba6}</MetaDataID>
        public Collections.Generic.Set<Key> GetReferenceKeys()
        {
            Collections.Generic.Set<Key> referenceKeys = new OOAdvantech.Collections.Generic.Set<Key>();
            foreach (MetaDataRepository.MetaObject metaObject in Namespace.OwnedElements)
            {
                Table table = metaObject as Table;
                if (table != null && table != this)
                {
                    foreach (Key key in table.Keys)
                    {
                        if (key.ReferencedTable == this)
                            referenceKeys.Add(key);
                    }
                }
            }
            return referenceKeys;

        }
        /// <MetaDataID>{e16ea80b-952b-468d-ac55-52a68bbb02f4}</MetaDataID>
        public void DropUnusedColumns()
        {
            DataBase dataBase = _Namespace.Value as DataBase;
            var connection = dataBase.SchemaUpdateConnection;
            if (connection != dataBase.Connection)
            {
#if !DeviceDotNet
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
#endif
                    (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.DropColumns(Name, new System.Collections.Generic.List<Column>(DropedColumns));
                    DropedColumns.RemoveAll();
#if !DeviceDotNet
                    transactionScope.Complete();
                }
#endif
            }
            else
            {
                (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.DropColumns(Name, new System.Collections.Generic.List<Column>(DropedColumns));
                DropedColumns.RemoveAll();

            }
        }
        /// <MetaDataID>{DE4B73FB-BA8E-4EC4-AE41-99F83EA6EA40}</MetaDataID>
        public void UpdateTableKeys()
        {
            if (_Keys.Count == 0)
                return;

            DataBase dataBase = _Namespace.Value as DataBase;
            var connection = dataBase.SchemaUpdateConnection;

            if (connection != dataBase.Connection)
            {
#if !DeviceDotNet
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
#endif
                    bool closeCollection = false;
                    try
                    {
                        foreach (Key key in DropedKeys)
                        {
                            foreach (string dropKeyScript in dataBase.RDBMSSQLScriptGenarator.GetDropKeyScript(key))
                            {
                                var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(dropKeyScript, Connection);
                                command.CommandText = dropKeyScript;
                                command.ExecuteNonQuery();
                            }
                        }
                        DropedKeys.Clear();
                        foreach (Key key in _Keys)
                        {
                            if (key.NewKey)
                            {
                                if (connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Closed)
                                {
                                    connection.Open();
                                    closeCollection = true;
                                }
                                foreach (string addKeyScript in dataBase.RDBMSSQLScriptGenarator.GetAddKeyScript(key))//.GetAddKeyScript();
                                {
                                    var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(addKeyScript, Connection);
                                    command.CommandText = addKeyScript;
                                    command.ExecuteNonQuery();
                                }
                                key.NewKey = false;
                            }
                            else
                            {
                                if (key.RenameKeyScripts != null)
                                {
                                    foreach (string renameKeyScript in key.RenameKeyScripts)
                                    {
                                        var command = connection.CreateCommand();//  new System.Data.SqlClient.SqlCommand(CurrKey.RenameKeyScript, Connection);
                                        command.CommandText = renameKeyScript;
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                            key.NewKey = false;
                            if (closeCollection)
                                connection.Close();
                        }
                    }
                    catch (System.Exception Error)
                    {
                        throw new System.Exception("It can't save the table '" + Name + "' changes for keys.");
                    }
#if !DeviceDotNet
                    transactionScope.Complete();
                }
#endif
            }
            else
            {

                try
                {
                    foreach (Key key in DropedKeys)
                    {
                        foreach (string dropKeyScript in dataBase.RDBMSSQLScriptGenarator.GetDropKeyScript(key))
                        {
                            var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(dropKeyScript, Connection);
                            command.CommandText = dropKeyScript;
                            command.ExecuteNonQuery();
                        }
                    }
                    DropedKeys.Clear();
                    foreach (Key key in _Keys)
                    {
                        if (key.NewKey)
                        {
                            foreach (string addKeyScript in dataBase.RDBMSSQLScriptGenarator.GetAddKeyScript(key))//.GetAddKeyScript();
                            {
                                var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(addKeyScript, Connection);
                                command.CommandText = addKeyScript;
                                command.ExecuteNonQuery();
                            }
                            key.NewKey = false;
                        }
                        else
                        {
                            if (key.RenameKeyScripts != null)
                            {
                                foreach (string sqlScript in key.RenameKeyScripts)
                                {
                                    var command = connection.CreateCommand();//  new System.Data.SqlClient.SqlCommand(CurrKey.RenameKeyScript, Connection);
                                    command.CommandText = sqlScript;
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        key.NewKey = false;
                    }
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception("It can't save the table '" + Name + "' changes for keys.", Error);
                }
            }
        }
        /// <MetaDataID>{E6CEB726-27CE-424E-9B42-82887C11EFF1}</MetaDataID>
        public void Update()
        {
            DataBase dataBase = _Namespace.Value as DataBase;
            var connection = dataBase.SchemaUpdateConnection;
            if (connection != dataBase.Connection)
            {
#if !DeviceDotNet
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
#endif
                    bool closeCollection = false;
                    OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseCommand command = null;
                    int resault = 0;
                    if (!string.IsNullOrEmpty(ChangeColumnNameScript))
                    {
                        if (connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Closed)
                        {
                            connection.Open();
                            closeCollection = true;
                        }
                        command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(ChangeColumnNameScript, connection);
                        command.CommandText = ChangeColumnNameScript;
                        System.Diagnostics.Debug.WriteLine(command.CommandText);
                        resault = command.ExecuteNonQuery();
                    }
                    try
                    {
                        foreach (string createOrAlterTableScript in dataBase.RDBMSSQLScriptGenarator.GetTableDDLScript(Name, NewColums, IsNewTable, TableType))
                        {

                            //if (!string.IsNullOrEmpty(ChangeTableNameScript))
                            //{
                            //    if (connection.State == System.Data.ConnectionState.Closed)
                            //    {
                            //        connection.Open();
                            //        closeCollection = true;
                            //    }

                            //    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(ChangeTableNameScript, connection);
                            //    command.CommandText = ChangeTableNameScript;
                            //    System.Diagnostics.Debug.WriteLine(command.CommandText);
                            //    resault = command.ExecuteNonQuery();
                            //}
                            if (!string.IsNullOrEmpty(createOrAlterTableScript))
                            {
                                if (connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Closed)
                                {
                                    connection.Open();
                                    closeCollection = true;
                                }

                                command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(CreateOrAlterTableScript, connection);
                                command.CommandText = createOrAlterTableScript;
                                System.Diagnostics.Debug.WriteLine(command.CommandText);
                                resault = command.ExecuteNonQuery();
                            }
                        }
                      
                        if (!IsNewTable && !string.IsNullOrEmpty(ColumnsDefaultValuesSetScript))
                        {
                            if (connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Closed)
                            {
                                connection.Open();
                                closeCollection = true;
                            }
                            command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(ColumnsDefaultValuesSetScript, connection);
                            command.CommandText = ColumnsDefaultValuesSetScript;
                            System.Diagnostics.Debug.WriteLine(command.CommandText);
                            resault = command.ExecuteNonQuery();
                        }
                        NewColums.Clear();
                        IsNewTable = false;
                    }
                    catch (System.Exception Error)
                    {
                        throw new System.Exception("It can't save the table '" + Name + "' changes.", Error);
                    }
                    try
                    {
                        foreach (Key key in _Keys)
                        {
                            if (key.IsPrimaryKey && key.NewKey)
                            {
                                foreach (string addKeyScript in dataBase.RDBMSSQLScriptGenarator.GetAddKeyScript(key))//.GetAddKeyScript();
                                {
                                    if (connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Closed)
                                    {
                                        connection.Open();
                                        closeCollection = true;
                                    }
                                    command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(addKeyScript, connection);
                                    command.CommandText = addKeyScript;
                                    System.Diagnostics.Debug.WriteLine(command.CommandText);
                                    command.ExecuteNonQuery();
                                }
                                key.NewKey = false;
                            }
                        }
                    }
                    catch (System.Exception Error)
                    {
                        throw new System.Exception("It can't save the table '" + Name + "' changes for keys.", Error);
                    }
                    if (closeCollection)
                        connection.Close();
#if !DeviceDotNet
                    transactionScope.Complete();
                }
#endif
            }
            else
            {
                try
                {
                    OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseCommand command = null;
                    bool closeCollection = false;
                    int resault = 0;
                    if (!string.IsNullOrEmpty(ChangeColumnNameScript))
                    {
                        if (connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Closed)
                        {
                            connection.Open();
                            closeCollection = true;
                        }

                        command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(ChangeColumnNameScript, connection);
                        command.CommandText = ChangeColumnNameScript;
                        System.Diagnostics.Debug.WriteLine(command.CommandText);
                        resault = command.ExecuteNonQuery();
                    }

                    foreach (string createOrAlterTableScript in dataBase.RDBMSSQLScriptGenarator.GetTableDDLScript(Name, NewColums, IsNewTable, TableType))
                    {
                        //if (!string.IsNullOrEmpty(ChangeTableNameScript))
                        //{
                        //    if (connection.State == System.Data.ConnectionState.Closed)
                        //    {
                        //        connection.Open();
                        //        closeCollection = true;
                        //    }
                        //    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(ChangeTableNameScript, connection);
                        //    command.CommandText = ChangeTableNameScript;
                        //    System.Diagnostics.Debug.WriteLine(command.CommandText);
                        //    resault = command.ExecuteNonQuery();
                        //}
                        if (!string.IsNullOrEmpty(createOrAlterTableScript))
                        {
                            if (connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Closed)
                            {
                                connection.Open();
                                closeCollection = true;
                            }

                            command = connection.CreateCommand();
                            command.CommandText = createOrAlterTableScript;
                            System.Diagnostics.Debug.WriteLine(command.CommandText);
                            resault = command.ExecuteNonQuery();
                        }
                    }
                    if (!IsNewTable && !string.IsNullOrEmpty(ColumnsDefaultValuesSetScript))
                    {
                        if (connection.State == RDBMSMetaDataPersistenceRunTime.ConnectionState.Closed)
                        {
                            connection.Open();
                            closeCollection = true;
                        }
                        command = connection.CreateCommand();
                        command.CommandText = ColumnsDefaultValuesSetScript;
                        System.Diagnostics.Debug.WriteLine(command.CommandText);
                        resault = command.ExecuteNonQuery();
                    }
                    NewColums.Clear();
                    IsNewTable = false;
                    if (closeCollection)
                        connection.Close();
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception("It can't save the table '" + Name + "' changes.", Error);
                }
                try
                {
                    foreach (Key key in _Keys)
                    {
                        if (key.IsPrimaryKey && key.NewKey)
                        {
                            foreach (string addKeyScript in dataBase.RDBMSSQLScriptGenarator.GetAddKeyScript(key))//.GetAddKeyScript();
                            {
                                var command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(addKeyScript, connection);
                                command.CommandText = addKeyScript;
                                System.Diagnostics.Debug.WriteLine(command.CommandText);
                                command.ExecuteNonQuery();
                            }
                            key.NewKey = false;
                        }
                    }
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception("It can't save the table '" + Name + "' changes for keys.", Error);
                }
            }
            if (ColumnsFormatHasChanged)
                (_Namespace.Value as DataBase).RDBMSSQLScriptGenarator.UpdateColumnsFormat(this);

            IsNewTable = false;
        }



        /// <MetaDataID>{eb70d163-ac4c-4100-8e77-e2de9763bd66}</MetaDataID>
        public static void CreateTemporaryTable(string temporaryTableName, MetaDataRepository.ObjectQueryLanguage.IDataTable dataTable, OOAdvantech.RDBMSDataObjects.DataBase dataBase)
        {

            MetaDataRepository.MetaObjectsStack oldMetaObjectCreator = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator;
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new RDBMSPersistenceRunTime.MetaObjectsStack();
            OOAdvantech.RDBMSMetaDataRepository.Table table = null;

            OOAdvantech.RDBMSDataObjects.Table tableMetaData = null;
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            //{
            if (dataTable.ExtendedProperties.ContainsKey("TableMetaData"))
                table = dataTable.ExtendedProperties["TableMetaData"] as OOAdvantech.RDBMSMetaDataRepository.Table;
            else
            {
                table = new OOAdvantech.RDBMSMetaDataRepository.Table();
                table.Name = temporaryTableName;
                foreach (MetaDataRepository.ObjectQueryLanguage.IDataColumn column in dataTable.Columns)
                    table.AddColumn(new OOAdvantech.RDBMSMetaDataRepository.Column(column.ColumnName, OOAdvantech.MetaDataRepository.Classifier.GetClassifier(column.DataType)));
            }
            tableMetaData = new OOAdvantech.RDBMSDataObjects.Table("SynchronizeTable", true, OOAdvantech.RDBMSDataObjects.TableType.TemporaryTable, dataBase);
            tableMetaData.Synchronize(table);
            tableMetaData.Name = temporaryTableName;
            foreach (Column column in tableMetaData.Columns)
            {
                if (column.IdentityColumn)
                    column.IdentityColumn = false;
            }

            //    stateTransition.Consistent = true;
            //}

            tableMetaData.Update();
            OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
            MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = oldMetaObjectCreator;


        }
    }
}
