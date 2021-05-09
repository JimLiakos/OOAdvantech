using OOAdvantech.Transactions;
namespace OOAdvantech.MSSQLPersistenceRunTime.MSSQLDataObjects
{
    /// <MetaDataID>{C65F8366-83BA-40DC-A00B-5B1E1FC752A4}</MetaDataID>
    public class Key : MetaDataRepository.MetaObject
    {

      
        /// <MetaDataID>{56785DC9-1FE4-4628-A682-2E6135FBA208}</MetaDataID>
        public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
        {
            try
            {
                RenameKeyScript = null;

                RDBMSMetaDataRepository.Key OriginKey = (RDBMSMetaDataRepository.Key)OriginMetaObject;
                IsPrimaryKey = OriginKey.IsPrimaryKey;
                if (Name != OriginKey.Name && !_NewKey)
                    RenameKeyScript = (Namespace.Namespace as DataBase).RDBMSSchema.GetRenameScript(this, OriginKey.Name);

                Name = OriginKey.Name;
                if (OwnerTable == null || OriginKey.OriginTable.Identity != OwnerTable.Identity)
                    OwnerTable = (Table)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginKey.OriginTable, this);
                foreach (RDBMSMetaDataRepository.Column CurrOriginKeyColumn in OriginKey.Columns)
                {
                    foreach (Column CurrColumn in OwnerTable.Columns)
                    {
                        if (CurrColumn.Name == CurrOriginKeyColumn.Name)
                        {
                            bool _continue = false;
                            foreach (Column column in Columns)
                                if (column.Name == CurrColumn.Name)
                                    _continue = true;
                            if (_continue)
                                continue;
                            Columns.Add(CurrColumn);
                        }
                    }
                }
                if (!IsPrimaryKey)
                {
                    ReferencedTable = (Table)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginKey.ReferedTable, this);
                    foreach (RDBMSMetaDataRepository.Column CurrOriginKeyColumn in OriginKey.ReferedColumns)
                    {
                        foreach (Column CurrColumn in ReferencedTable.Columns)
                        {
                            if (CurrColumn.Name == CurrOriginKeyColumn.Name)
                            {
                                bool _continue = false;
                                foreach (Column column in ReferedColumns)
                                    if (column.Name == CurrColumn.Name)
                                        _continue = true;
                                if (_continue)
                                    continue;
                                ReferedColumns.Add(CurrColumn);
                            }

                        }
                    }

                }
                OriginKey.DataBaseKeyName = Name;


                int k = 0;
            }
            catch (System.Exception error)
            {
                throw;
            }

        }

        /// <MetaDataID>{97F03E1A-4812-4CB3-9809-1D236CB8974F}</MetaDataID>
        public override MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                if (_Identity == null)
                    _Identity = new MetaDataRepository.MetaObjectID(Name);
                return _Identity;
            }
        }
        //TODO Πρέπει να είναι ordered
        /// <MetaDataID>{57CCE3B9-A2CB-424D-95CE-751062CA3EE0}</MetaDataID>
        public Collections.Generic.Set<Column> ReferedColumns =  new OOAdvantech.Collections.Generic.Set<Column>();

        //TODO Πρέπει να είναι ordered
        /// <MetaDataID>{86BBE38C-C1C0-4371-85CB-BB307E373648}</MetaDataID>
        public Collections.Generic.Set<Column> Columns = new OOAdvantech.Collections.Generic.Set<Column>();
        /// <MetaDataID>{038B736C-A711-4693-B274-60E56A792392}</MetaDataID>
        public Table ReferencedTable;


        /// <MetaDataID>{922DC8A6-F07E-4F4A-BA7D-98B275DBFFFB}</MetaDataID>
        public override System.Collections.ArrayList GetExtensionMetaObjects()
        {
            return new System.Collections.ArrayList();
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{42D11984-7017-43E9-8111-41295D96460A}</MetaDataID>
        public bool IsPrimaryKey;

        /// <MetaDataID>{BFEF7E43-5B30-44D5-AC55-9E8C07C18943}</MetaDataID>
        bool _NewKey;
        /// <MetaDataID>{963FC8F2-6F1A-46df-B5DE-89351D126963}</MetaDataID>
        /// <exclude>Excluded</exclude>
        public bool NewKey
        {
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _NewKey = value; 
                    stateTransition.Consistent = true;
                }
            }
            get
            {
                return _NewKey;
            }
        }
        /// <MetaDataID>{9D50F334-D394-45A2-B81F-5E95AB4F7146}</MetaDataID>
        public Table OwnerTable;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B207FF2A-42E8-4FED-AED7-B4B138ADA5D3}</MetaDataID>
        //	private string ConnectionString;
        /// <MetaDataID>{862501DC-E5AC-45FE-95D0-1F0ABC7F08C0}</MetaDataID>
        public Key(string name, bool isPrimaryKey, Table ownerTable, bool newKey)
        {
            InitKey(name, isPrimaryKey, ownerTable, newKey, null);
        }

        /// <MetaDataID>{416E9D18-13E0-4104-94D3-0D135E0A9010}</MetaDataID>
        public Key()
        {
            InitKey("", false, null,  true, null);
        }
        /// <MetaDataID>{F588B761-6822-4401-9B05-362FBF59446B}</MetaDataID>
        public Key(string name, bool isPrimaryKey, Table ownerTable, bool newKey, Table referencedTable)
        {
            InitKey(name, isPrimaryKey, ownerTable,  newKey, referencedTable);
        }
        /// <MetaDataID>{2F9F1661-EEA3-4573-ABF9-FC8D461A9FC1}</MetaDataID>
        void InitKey(string name, bool isPrimaryKey, Table ownerTable,  bool newKey, Table referencedTable)
        {
            Name = name;
            IsPrimaryKey = isPrimaryKey;
            OwnerTable = ownerTable;
            _Namespace = ownerTable;
            //	ConnectionString=connectionString;
            NewKey = newKey;
            ReferencedTable = referencedTable;
            if (!NewKey)
                ReadKeyColumns();
        }
        ///// <MetaDataID>{c532580f-38ad-4978-88b8-648a714e6137}</MetaDataID>
        //public string GetDropKeyScript()
        //{
        //    return (Namespace.Namespace as DataBase).GetDropKeyScript(this);
        //}
        /// <MetaDataID>{4e97a5ed-fa29-432f-a322-27ed3b081fec}</MetaDataID>
        //public string GetAddKeyScript()
        //{
        //    return (Namespace.Namespace as DataBase).GetAddKeyScript(this);
        //}


        /// <MetaDataID>{fad732a1-86cf-4563-a687-6414fa366f02}</MetaDataID>
        public string RenameKeyScript = null;

        /// <MetaDataID>{DB91A1AB-AA09-4A02-92AD-8B767AA52AE1}</MetaDataID>
        void ReadForeignKeyColumns()
        {

            System.Collections.Generic.List<string> columnsNames=new System.Collections.Generic.List<string>();
            System.Collections.Generic.List<string> referedColumnsNames = new System.Collections.Generic.List<string>();
            (Namespace.Namespace as DataBase).RDBMSSchema.ReadForeignKeyColumns(this, columnsNames, referedColumnsNames);
            foreach (string columnName in columnsNames)
            {
                Column column = OwnerTable.GetColumn(columnName);
                Columns.Add(column);
            }
            foreach (string referenceColumnName in referedColumnsNames)
            {
                Column column = ReferencedTable.GetColumn(referenceColumnName);
                ReferedColumns.Add(column);
            }

            //System.Collections.Hashtable Name_Key_map = new System.Collections.Hashtable();
            //DataBase dataBase = _Namespace.Namespace as DataBase;
            //System.Data.SqlClient.SqlConnection Connection = dataBase.Connection;

            //string CommandString = "exec sp_fkeys @pktable_name ='" + ReferencedTable.Name + "' ,@fktable_name = '" + OwnerTable.Name + "'";
            //System.Data.SqlClient.SqlCommand Command = new System.Data.SqlClient.SqlCommand(CommandString, Connection);
            //System.Data.SqlClient.SqlDataReader DataReader = Command.ExecuteReader();
            //foreach (System.Data.Common.DbDataRecord CurrRecord in DataReader)
            //{
            //    if (CurrRecord["FK_NAME"].Equals(Name))
            //    {
            //        string PKColumnName = (string)CurrRecord["PKCOLUMN_NAME"];
            //        string FKColumnName = (string)CurrRecord["FKCOLUMN_NAME"];


            //        Column column = OwnerTable.GetColumn(FKColumnName);
            //        //TODO Πρέπει να αλλάξει οπσδιποτε γιατί είναι πολύ error prone 
            //        //Το σωστό είναι  Πρέπει να είναι ordered
            //        if (column.ColumnType == null)
            //            column.ColumnType = Columns.Count.ToString();

            //        Columns.Add(column);

            //        column = ReferencedTable.GetColumn(PKColumnName);
            //        //TODO Πρέπει να αλλάξει οπσδιποτε γιατί είναι πολύ error prone 
            //        //Το σωστό είναι  Πρέπει να είναι ordered
            //        if (column.ColumnType == null)
            //            column.ColumnType = ReferedColumns.Count.ToString();
            //        ReferedColumns.Add(column);
            //        int kj = 0;

            //    }
            //}
            //DataReader.Close();
            //Connection.Close();
        }

        /// <MetaDataID>{9CF5A051-76E0-4B47-8823-49BAFD0F96DD}</MetaDataID>
        void ReadPrimaryKeyColumns()
        {
            System.Collections.Generic.List<string> columnsNames = new System.Collections.Generic.List<string>();
            (Namespace.Namespace as DataBase).RDBMSSchema.ReadPrimaryKeyColumns(this, columnsNames);
            foreach (string columnName in columnsNames)
            {
                Column column = OwnerTable.GetColumn(columnName);
                Columns.Add(column);
            }
        }

        /// <MetaDataID>{33D558C2-3687-4363-AAF4-0D77CF1E05DF}</MetaDataID>
        void ReadKeyColumns()
        {
            if (IsPrimaryKey)
                ReadPrimaryKeyColumns();
            else
                ReadForeignKeyColumns();

        }
    }

    /*
         ALTER TABLE dbo.T_Address1 ADD CONSTRAINT
        PK_T_Address1 PRIMARY KEY CLUSTERED 
        (
        ObjCellID,
        IntObjID
        ) ON [PRIMARY]
	
        */



}
