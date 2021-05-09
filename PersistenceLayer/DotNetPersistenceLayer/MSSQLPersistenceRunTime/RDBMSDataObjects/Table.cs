using OOAdvantech.Transactions;
//using OOAdvantech.MSSQLPersistenceRunTime;
namespace OOAdvantech.RDBMSDataObjects
{

    /// <MetaDataID>{7ddb1bdc-c061-4249-83a7-06fdfee176dd}</MetaDataID>
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
                if (CurrColumn.Name == ColumnName)
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
                    return new OOAdvantech.MetaDataRepository.MetaObjectID(Namespace.Identity + "." + Name);
                return base.Identity;
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
            if (_Keys.Count == 0 && !IsNewTable)
            {
                //DataBase dataBase =_Namespace as DataBase;
                //System.Data.SqlClient.SqlConnection Connection=dataBase.Connection;

                //System.Data.SqlClient.SqlConnection Connection=new System.Data.SqlClient.SqlConnection(ConnectionString);
                try
                {
                    System.Collections.Hashtable Name_Key_map = new System.Collections.Hashtable();

                    //if(Connection.State!=System.Data.ConnectionState.Open)
                    //    Connection.Open();
                    //System.Data.SqlClient.SqlCommand Command=new System.Data.SqlClient.SqlCommand("exec sp_pkeys "+Name,Connection);
                    //System.Data.SqlClient.SqlDataReader  DataReader= Command.ExecuteReader();
                    //// Load table Primary Keys

                    System.Collections.Generic.List<string> primaryKeys = (_Namespace as DataBase).RDBMSSchema.RetrieveDatabasePrimaryKeys(this);// new System.Collections.Generic.List<string>();

                    //foreach(System.Data.Common.DbDataRecord CurrRecord in DataReader)
                    //{
                    //    primaryKeys.Add((string)CurrRecord["PK_NAME"]);
                    //}
                    //DataReader.Close();
                    foreach (string KeyName in primaryKeys)
                    {
                        Key aKey = null;
                        if (!Name_Key_map.Contains(KeyName))
                        {
                            aKey = new Key(KeyName, true, this, false);
                            _Keys.Add(aKey);
                            AddOwnedElement(aKey);
                            Name_Key_map.Add(aKey.Name, aKey);
                        }
                    }

                    // Load table Foreign Keys

                    //                    Command.CommandText=@"SELECT     F_Keys.name AS FK_NAME, FK_Tables.name AS FK_TABLE_NAME, REF_Tables.name AS REF_TABLE_NAME 
                    //										FROM         sysobjects FK_Tables INNER JOIN 
                    //										sysreferences r INNER JOIN 
                    //										sysobjects F_Keys ON r.constid = F_Keys.id ON FK_Tables.id = r.fkeyid INNER JOIN 
                    //										sysobjects REF_Tables ON r.rkeyid = REF_Tables.id 
                    //										WHERE     (F_Keys.xtype = 'F') AND (FK_Tables.name = N'"+Name+"')";

                    //                    DataReader= Command.ExecuteReader();
                    //                    primaryKeys.Clear();

                    //                    System.Collections.Hashtable fKeys=new System.Collections.Hashtable();

                    //                    foreach(System.Data.Common.DbDataRecord CurrRecord in DataReader)
                    //                        fKeys.Add((string)CurrRecord["FK_NAME"],(string)CurrRecord["REF_TABLE_NAME"]);

                    //                    DataReader.Close();


                    foreach (ForeignKeyData foreignKeyData in (_Namespace as DataBase).RDBMSSchema.RetrieveDatabaseForeignKeys(this))
                    {
                        Key aKey = null;
                        if (!Name_Key_map.Contains(foreignKeyData.Name))
                        {
                            aKey = new Key(foreignKeyData.Name, false, this, false, ((DataBase)Namespace).GetTable(foreignKeyData.ReferenceTableName));
                            _Keys.Add(aKey);
                            AddOwnedElement(aKey);
                            Name_Key_map.Add(aKey.Name, aKey);
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
                foreach (ColumnData columnData in (_Namespace as DataBase).RDBMSSchema.RetreiveTableColumns(Name))
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
                        object defaultValue = _class.GetDefaultValue(attribute);
                        columnsDefaultValues.Add(new ColumnDefaultValueData(addedColumn.Name, defaultValue));
                    }
                }
                return GetColumnsDefaultValuesSetScript(Name, columnsDefaultValues);

            }
        }
        /// <MetaDataID>{3631b982-c44e-41fd-b7ee-69f7cc39a641}</MetaDataID>
        string ChangeTableNameScript;
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


   
         /// <MetaDataID>{5143a8df-16a1-45f1-98b7-84dc14b3e177}</MetaDataID>
        protected string GetColumnsDefaultValuesSetScript(string tableName, System.Collections.Generic.List<ColumnDefaultValueData> columnsDefaultValues)
        {
            string defaultValuesSetScript=null;
            foreach (ColumnDefaultValueData columnDefaultValueData in columnsDefaultValues)
            {
                if (defaultValuesSetScript == null)
                    defaultValuesSetScript = "UPDATE [" + tableName + "] \r\n SET ";
                else
                    defaultValuesSetScript += ",";
                defaultValuesSetScript += columnDefaultValueData.ColumnName + " = " + (_Namespace as DataBase).RDBMSSchema.ConvertToSQLString(columnDefaultValueData.DefaultValue);
            }

            return defaultValuesSetScript;
        }
        /// <MetaDataID>{092ab21d-7c19-4cd0-83ef-3f4c2454244e}</MetaDataID>
        System.Collections.Generic.List<Column> NewColums = new System.Collections.Generic.List<Column>();

        /// <MetaDataID>{72CDD6D3-67E8-497E-91FB-6E6A9E5CDE36}</MetaDataID>
        public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
        {
            using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
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
                    MetaDataRepository.ContainedItemsSynchronizer KeysSynchronizer = new MetaDataRepository.ContainedItemsSynchronizer(OriginKeys, _Keys, this);
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

                    MetaDataRepository.ContainedItemsSynchronizer ColumnsSynchronizer = new MetaDataRepository.ContainedItemsSynchronizer(OriginTableColumns, _Columns, this);
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
                    //System.Collections.Generic.List<ColumnDefaultValueData> columnsDefaultValues = new System.Collections.Generic.List<ColumnDefaultValueData>();
                    //foreach (MetaDataRepository.AddCommand addCommand in ColumnsSynchronizer.AddedObjectsCommands)
                    //{
                    //    Column addedColumn = (Column)addCommand.AddedObject;
                    //    if (addedColumn.OriginColumn.MappedAttribute != null)
                    //    {
                    //        DotNetMetaDataRepository.Attribute attribute = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(addedColumn.OriginColumn.MappedAttribute.Identity) as DotNetMetaDataRepository.Attribute;
                    //        DotNetMetaDataRepository.Class _class = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject((OriginTable.TableCreator as RDBMSMetaDataRepository.StorageCell).Type.Identity) as DotNetMetaDataRepository.Class;
                    //        object defaultValue = _class.GetDefaultValue(attribute);
                    //        columnsDefaultValues.Add(new ColumnDefaultValueData(addedColumn.Name, defaultValue));
                    //    }
                    //}
                    //ColumnsDefaultValuesSetScript = GetColumnsDefaultValuesSetScript(Name, columnsDefaultValues);

                    foreach (Column column in _Columns)
                    {
                        if (column.Name != column.DataBaseColumnName &&!string.IsNullOrEmpty(column.DataBaseColumnName))
                            ChangeColumnNameScript += (_Namespace as DataBase).RDBMSSchema.GetChangeColumnNameScript(Name, column.DataBaseColumnName, column.Name) + "\n";
                    }
                    _Name = OriginTable.Name;
                }
                if (OriginTable.DataBaseTableName != Name)
                {
                    ChangeTableNameScript = (_Namespace as DataBase).RDBMSSchema.GetChangeTableNameScript(OriginTable.DataBaseTableName, Name) + "\n";
                    OriginTable.DataBaseTableName = Name;
                }
                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{67EE5F54-1B13-44CE-BDE6-E24E0B96CD5E}</MetaDataID>
        public override System.Collections.ArrayList GetExtensionMetaObjects()
        {
            return new System.Collections.ArrayList();
        }
        /// <exclude>Excluded</exclude>
        private bool _IsNewTable = true;
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
            _Namespace = dataBase;
            Name = name;
            _IsNewTable = newTable;
            ReadColumns();
        }



        /// <MetaDataID>{c2ef5a76-461d-4abf-a825-d74cbed2a91d}</MetaDataID>
        public Table(string name, bool newTable, TableType tableType, DataBase dataBase)
        {
            _Namespace = dataBase;
            Name = name;
            _IsNewTable = newTable;
            ReadColumns();
            TableType = tableType;
        }


        /// <MetaDataID>{FF504503-B422-4EFB-BC19-8B67D46EFA54}</MetaDataID>
        ~Table()
        {
            System.Diagnostics.Debug.WriteLine("~Table()");
            int hh = 0;
        }
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
            (_Namespace as DataBase).RDBMSSchema.DropColumns(Name, new System.Collections.Generic.List<Column>(DropedColumns));
            DropedColumns.RemoveAll();
        }
        /// <MetaDataID>{DE4B73FB-BA8E-4EC4-AE41-99F83EA6EA40}</MetaDataID>
        public void UpdateTableKeys()
        {
            if (_Keys.Count == 0)
                return;

            DataBase dataBase = _Namespace as DataBase;
            System.Data.Common.DbConnection connection = dataBase.Connection;
            try
            {
                foreach (Key key in DropedKeys)
                {
                    string dropKeyScript = dataBase.RDBMSSchema.GetDropKeyScript(key);
                    System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(dropKeyScript, Connection);
                    command.CommandText = dropKeyScript;
                    command.ExecuteNonQuery();
                }
                DropedKeys.Clear();
                foreach (Key key in _Keys)
                {
                    if (key.NewKey)
                    {

                        foreach (string addKeyScript in dataBase.RDBMSSchema.GetAddKeyScript(key))//.GetAddKeyScript();
                        {
                            System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(addKeyScript, Connection);
                            command.CommandText = addKeyScript;
                            command.ExecuteNonQuery();
                        }
                        key.NewKey = false;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(key.RenameKeyScript))
                        {
                            System.Data.Common.DbCommand command = connection.CreateCommand();//  new System.Data.SqlClient.SqlCommand(CurrKey.RenameKeyScript, Connection);
                            command.CommandText = key.RenameKeyScript;
                            command.ExecuteNonQuery();
                        }
                    }
                    key.NewKey = false;
                }
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("It can't save the table '" + Name + "' changes for keys.");
            }
        }
        /// <MetaDataID>{E6CEB726-27CE-424E-9B42-82887C11EFF1}</MetaDataID>
        public void Update()
        {
            DataBase dataBase = _Namespace as DataBase;
            string CreateOrAlterTableScript = dataBase.RDBMSSchema.GetDataBaseUpdateScript(Name, NewColums, IsNewTable, TableType);
            System.Data.Common.DbConnection connection = dataBase.Connection;
            try
            {
                System.Data.Common.DbCommand command = null;
                int resault = 0;
                if (!string.IsNullOrEmpty(ChangeTableNameScript))
                {
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(ChangeTableNameScript, connection);
                    command.CommandText = ChangeTableNameScript;
                    resault = command.ExecuteNonQuery();
                }
                if (!string.IsNullOrEmpty(CreateOrAlterTableScript))
                {
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(CreateOrAlterTableScript, connection);
                    command.CommandText = CreateOrAlterTableScript;
                    resault = command.ExecuteNonQuery();
                }
                if (!string.IsNullOrEmpty(ChangeColumnNameScript))
                {
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(ChangeColumnNameScript, connection);
                    command.CommandText = ChangeColumnNameScript;
                    resault = command.ExecuteNonQuery();
                }
                if (!IsNewTable && !string.IsNullOrEmpty(ColumnsDefaultValuesSetScript))
                {
                    command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand(ColumnsDefaultValuesSetScript, connection);
                    command.CommandText = ColumnsDefaultValuesSetScript;
                    resault = command.ExecuteNonQuery();
                }
                
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("It can't save the table '" + Name + "' changes.",Error);
            }
            try
            {
                foreach (Key key in _Keys)
                {
                    if (key.IsPrimaryKey && key.NewKey)
                    {
                        foreach (string addKeyScript in dataBase.RDBMSSchema.GetAddKeyScript(key))//.GetAddKeyScript();
                        {
                            System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(addKeyScript, connection);
                            command.CommandText = addKeyScript;
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
            if (ColumnsFormatHasChanged)
                (_Namespace as DataBase).RDBMSSchema.UpdateColumnsFormat(this);

            IsNewTable = false;
        }


    }
}
