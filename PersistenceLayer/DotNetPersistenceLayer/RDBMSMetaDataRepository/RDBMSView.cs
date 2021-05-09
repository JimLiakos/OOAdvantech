namespace OOAdvantech.RDBMSMetaDataRepository
{
    using System.Linq;
    using MetaDataRepository;
    using OOAdvantech.Transactions;
    /// <MetaDataID>{0DEB2D97-D329-491B-ACC1-2F43ECBFB035}</MetaDataID>
    /// <summary>A view is a virtual table whose contents are defined by a query. 
    /// Like a real table, a view consists of a set of named columns and rows of data. 
    /// In MetaDataRepository help us to view the instance of class as rows of single table. There are two types of view.
    /// The first type used to retrieve the instance of class, the second used to retrieve the instance of class and all subclass. </summary>
    [BackwardCompatibilityID("{0DEB2D97-D329-491B-ACC1-2F43ECBFB035}")]
    [Persistent()]
    public class View : MetaDataRepository.Namespace
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(ViewStorageCell))
            {
                if (value == null)
                    ViewStorageCell = default(OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.RDBMSMetaDataRepository.View, OOAdvantech.RDBMSMetaDataRepository.StorageCell>);
                else
                    ViewStorageCell = (OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.RDBMSMetaDataRepository.View, OOAdvantech.RDBMSMetaDataRepository.StorageCell>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_DataBaseViewName))
            {
                if (value == null)
                    _DataBaseViewName = default(string);
                else
                    _DataBaseViewName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_JoinedTables))
            {
                if (value == null)
                    _JoinedTables = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Table>);
                else
                    _JoinedTables = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Table>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageCell))
            {
                if (value == null)
                    _StorageCell = default(OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                else
                    _StorageCell = (OOAdvantech.RDBMSMetaDataRepository.StorageCell)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_SubViews))
            {
                if (value == null)
                    _SubViews = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.View>);
                else
                    _SubViews = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.View>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(TmpViewColumns))
            {
                if (value == null)
                    TmpViewColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    TmpViewColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ViewColumns))
            {
                if (value == null)
                    _ViewColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    _ViewColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(ViewStorageCell))
                return ViewStorageCell;

            if (member.Name == nameof(_DataBaseViewName))
                return _DataBaseViewName;

            if (member.Name == nameof(_JoinedTables))
                return _JoinedTables;

            if (member.Name == nameof(_StorageCell))
                return _StorageCell;

            if (member.Name == nameof(_SubViews))
                return _SubViews;

            if (member.Name == nameof(TmpViewColumns))
                return TmpViewColumns;

            if (member.Name == nameof(_ViewColumns))
                return _ViewColumns;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{d1af7cc8-a82a-4978-970f-bc63f75614cf}</MetaDataID>
        public Collections.Generic.Dictionary<View, StorageCell> ViewStorageCell;
        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{9CA35056-8782-4817-B3C5-923DDBF289F1}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    MetaDataRepository.MetaObjectID identity = null;
                    if (_DataBaseViewName != null && _DataBaseViewName.Trim().Length > 0)
                    {
                        if (Namespace != null)
                            identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + _DataBaseViewName);
                        else
                            identity = new MetaDataRepository.MetaObjectID(_DataBaseViewName);
                        identity = new MetaObjectID(identity.ToString().ToLower().Trim());

                    }
                    else
                    {
                        if (Namespace != null)
                            identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + Name);
                        else
                            identity = new MetaDataRepository.MetaObjectID(Name);
                        identity = new MetaObjectID(identity.ToString().ToLower().Trim());

                        return identity;
                    }
                    if (base.Identity == null || identity.ToString() != base.Identity.ToString())
                    {
                        _Identity = null;
                        //using (OOAdvantech.Transactions.ObjectStateTransition stateTransition=new OOAdvantech.Transactions.ObjectStateTransition(this))
                        //{
                        SetIdentity(identity);
                        //stateTransition.Consistent=true;
                        //}
                    }
                    return _Identity;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5C658C13-5A6C-4E40-8305-E40FF8E9D7AB}</MetaDataID>
        private string _DataBaseViewName;
        /// <MetaDataID>{798902B1-4B7E-47F6-A649-CCD5F850D29B}</MetaDataID>
        [BackwardCompatibilityID("+25")]
        [PersistentMember(255, "_DataBaseViewName")]
        public string DataBaseViewName
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _DataBaseViewName;

                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_DataBaseViewName != value)
                    {
                        using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        {
                            _DataBaseViewName = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{1C3CDC58-A8B4-4FF5-9C97-292061413B00}</MetaDataID>
        public View(string name, Namespace _namespace)
        {
            _namespace.AddOwnedElement(this);
            _Name = name;
            _DataBaseViewName = name;
        }

        /// <MetaDataID>{f507b081-b4a3-4881-a2c3-218a9f0e952b}</MetaDataID>
        public View(string name)
        {
            _Name = name;
            _DataBaseViewName = name;
        }


        /// <MetaDataID>{5CB17E82-BEEB-4225-B041-050222C0D396}</MetaDataID>
        protected View()
        {
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{0175CB81-32BA-4D44-8D1B-A336CA8AE60E}</MetaDataID>
        private Collections.Generic.Set<Table> _JoinedTables = new OOAdvantech.Collections.Generic.Set<Table>();
        /// <MetaDataID>{54366391-C1A9-4662-8840-1CCEADE2E0BF}</MetaDataID>
        [Association("RetrieveFromTable", typeof(OOAdvantech.RDBMSMetaDataRepository.Table), Roles.RoleA, "{956F9573-3205-4618-9D75-2E20A2AEDC3D}")]
        [PersistentMember("_JoinedTables")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<Table> JoinedTables
        {
            get
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    return new Collections.Generic.Set<Table>(_JoinedTables);
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }

        /// <MetaDataID>{2A5DA962-D291-4EDC-8FEC-D57E92C3D159}</MetaDataID>
        public void AddJoinedTable(Table JoinedTable)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                _JoinedTables.Add(JoinedTable);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{5aa60b5d-69d4-4247-a350-6924f06620d7}</MetaDataID>
        StorageCell _StorageCell;
        /// <MetaDataID>{3C816148-65D0-4F4F-99B6-4255A61FE648}</MetaDataID>
        [Association("StorageCellInstancesOfClass", typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell), Roles.RoleB, "{C29F8127-052C-495F-BC85-48A18962FB94}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_StorageCell")]
        [RoleBMultiplicityRange(0, 1)]
        public StorageCell StorageCell
        {
            get
            {
                return _StorageCell;
            }
            set
            {
                _StorageCell = value;
            }
        }
        /// <MetaDataID>{22BF62AD-A1A8-4BBC-A41A-353527E26C2C}</MetaDataID>
        public int CreationOrder
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    int SubViewCreationOrder = 0;
                    foreach (View CurrView in SubViews)
                    {
                        if (CurrView.CreationOrder > SubViewCreationOrder)
                            SubViewCreationOrder = CurrView.CreationOrder;
                    }
                    return SubViewCreationOrder + 1;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{47D5536F-647D-4A09-A4C7-E405BB0B266F}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Collections.Generic.Set<View> _SubViews = new OOAdvantech.Collections.Generic.Set<View>();
        /// <MetaDataID>{B8D763C9-02DA-4435-B2F6-0DC3B14BD75B}</MetaDataID>
        [Association("SubView", typeof(OOAdvantech.RDBMSMetaDataRepository.View), Roles.RoleA, "{C1311155-5065-4DD7-9448-F3BB065EA0FF}")]
        [PersistentMember("_SubViews")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<View> SubViews
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new OOAdvantech.Collections.Generic.Set<View>(_SubViews);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{8FDB9085-87B5-42CD-8549-88D4246350C9}</MetaDataID>
        public System.Collections.Generic.List<string> ViewColumnsNames
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    System.Collections.Generic.List<string> concreteClassViewColumnNames = new System.Collections.Generic.List<string>();
                    foreach (Column CurrColumn in ViewColumns)
                    {
                        if (string.IsNullOrEmpty(CurrColumn.DataBaseColumnName))
                            concreteClassViewColumnNames.Add(CurrColumn.Name);
                        else
                            concreteClassViewColumnNames.Add(CurrColumn.DataBaseColumnName);

                    }
                    return concreteClassViewColumnNames;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{6e58fd6b-1c19-46ea-85fa-c4d066d89cb0}</MetaDataID>
        Collections.Generic.Set<Column> TmpViewColumns;

        /// <MetaDataID>{288C1084-07FA-4EDA-9C61-087967C4FDB3}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Collections.Generic.Set<Column> _ViewColumns = new OOAdvantech.Collections.Generic.Set<Column>();
        /// <MetaDataID>{B4E2161B-6172-4F57-AF97-BF2455CEBA7A}</MetaDataID>
        [Association("ViewColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleA, "{BD85B68A-9590-427D-AC44-A524CEF110B2}")]
        [PersistentMember("_ViewColumns")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<Column> ViewColumns
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (TmpViewColumns != null)
                        return TmpViewColumns;
                    System.Collections.Generic.List<Column> ColumnAliases = new System.Collections.Generic.List<Column>(); 
                    if (_ViewColumns.Count == 0 && _SubViews.Count > 0)
                    {
                        foreach (View CurrSubView in _SubViews)
                        {
                            foreach (Column column in CurrSubView.ViewColumns)
                            {
                                AddColumn(column);
                                //bool exist = false;
                                //foreach (Column viewColumn in _ViewColumns)
                                //    if (viewColumn.Name == column.Name && column.Type.FullName == viewColumn.Type.FullName)
                                //    {
                                //        exist = true;
                                //        break;
                                //    }

                                //if (!exist)
                                //    _ViewColumns.Add(column);
                            }
                            
                        }
                        return _ViewColumns;
                    }
                    else if (_SubViews.Count > 0)
                        return _ViewColumns;

                    foreach (Column CurrColumnAlias in _ViewColumns)
                        ColumnAliases.Add(CurrColumnAlias);
                    ColumnAliases = ColumnAliases.OrderBy(column => column.Name).ToList();// (from Column in ColumnAliases orderby Column.Name select Column).ToList();
                    TmpViewColumns = new Collections.Generic.Set<Column>();
                    foreach (Column CurrColumnAlias in ColumnAliases)
                        TmpViewColumns.Add(CurrColumnAlias);
                    return TmpViewColumns;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }


        /// <MetaDataID>{0be7f3f3-10c3-4b19-88f2-5d2ed0527acb}</MetaDataID>
        public void RemoveColumn(Column column)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (!_ViewColumns.Contains(column))
                {
                    foreach (Column CurrColumn in _ViewColumns)
                    {
                        if (CurrColumn.Name == column.Name)
                        {
                            using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                            {
                                TmpViewColumns = null;
                                _ViewColumns.Remove(CurrColumn);
                                RemoveOwnedElement(CurrColumn);
                                StateTransition.Consistent = true; ;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        TmpViewColumns = null;
                        _ViewColumns.Remove(column);
                        StateTransition.Consistent = true; ;
                    }
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }


        }


        /// <MetaDataID>{C249E097-413B-4E1B-882C-2F12C5243058}</MetaDataID>
        public void AddColumn(Column column)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (!_ViewColumns.Contains(column))
                {
                    System.Collections.Generic.List<string> columnNames = new System.Collections.Generic.List<string>();
                    foreach (Column CurrColumn in _ViewColumns)
                    {
                        if (CurrColumn.Name == column.Name)
                        {
                            if (CurrColumn.Type.FullName == column.Type.FullName)
                                return;
                        }
                        if (CurrColumn.RealColumn != null && CurrColumn.RealColumn.Name == column.Name)
                        {
                            if (CurrColumn.RealColumn.Type.FullName == column.Type.FullName)
                                return;
                        }
                        columnNames.Add(CurrColumn.Name);
                        //Error prone replace return with  throw  and execute split senario
                        //throw new System.Exception("Column '"+NewColumnAlias.Name+"' already exists."); 
                        // in splite we have double Columns one from previous objectcell and one from the new
                    }
                    using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this,OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        TmpViewColumns = null;
                        int count=1;
                        string columnName=column.Name;
                        while (columnNames.Contains(columnName))
                        {
                            columnName += "_" + count.ToString();
                            count++;
                        }
                        if (column.Namespace != null)
                        {
                            Column newColumn = new Column(column);
                            if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this)!=null)
                                PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(newColumn);
                            newColumn.Name = columnName;
                            _ViewColumns.Add(newColumn);
                            AddOwnedElement(newColumn);
                        }
                        else
                        {
                            column.Name = columnName;
                            _ViewColumns.Add(column);
                            AddOwnedElement(column);
                        }
                        StateTransition.Consistent = true; ;
                    }

                    if (PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties) != null)
                    {
                        // For non transient views
                        PersistenceLayer.ObjectStorage.CommitObjectState(column);
                    }
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }


        /// <MetaDataID>{F16048FC-A650-45D9-BB4D-1E60BC65EBF8}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();

        }
        //public void RefreshViewColumns()
        //{

        //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this,TransactionOption.Supported))
        //    {
        //        _ViewColumns.Clear(); 
        //        stateTransition.Consistent = true;
        //    }
        
        //}

        /// <MetaDataID>{70BAEF28-06CE-4116-8353-D75574133477}</MetaDataID>
        public void AddSubView(View subView)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (subView == null)
                    throw new System.ArgumentNullException("subView");
                if (!_SubViews.Contains(subView))
                {
                    using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this,OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        _SubViews.Add(subView);
                       // RefreshViewColumns();
                        StateTransition.Consistent = true; ;
                    }
                }
                
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <MetaDataID>{B0DBF967-6EC2-4A5A-8017-A25BF108A1DA}</MetaDataID>
        public void RemoveSubView(View SubView)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _SubViews.Remove(SubView);
                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
    }
}
