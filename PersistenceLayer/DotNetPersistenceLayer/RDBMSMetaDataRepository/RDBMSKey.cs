namespace OOAdvantech.RDBMSMetaDataRepository
{
    using MetaDataRepository;
    /// <MetaDataID>{0B939C22-589B-4E9D-AC5A-2AC94080E4A6}</MetaDataID>
    [BackwardCompatibilityID("{0B939C22-589B-4E9D-AC5A-2AC94080E4A6}")]
    [Persistent()]
    public class Key : MetaDataRepository.MetaObject
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Identity))
            {
                if (value == null)
                    _Identity = default(string);
                else
                    _Identity = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_IsPrimaryKey))
            {
                if (value == null)
                    _IsPrimaryKey = default(bool);
                else
                    _IsPrimaryKey = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(OriginTable))
            {
                if (value == null)
                    OriginTable = default(OOAdvantech.RDBMSMetaDataRepository.Table);
                else
                    OriginTable = (OOAdvantech.RDBMSMetaDataRepository.Table)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(PrimaryKeyCreator))
            {
                if (value == null)
                    PrimaryKeyCreator = default(OOAdvantech.RDBMSMetaDataRepository.Table);
                else
                    PrimaryKeyCreator = (OOAdvantech.RDBMSMetaDataRepository.Table)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_DataBaseKeyName))
            {
                if (value == null)
                    _DataBaseKeyName = default(string);
                else
                    _DataBaseKeyName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Columns))
            {
                if (value == null)
                    _Columns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    _Columns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ReferedColumns))
            {
                if (value == null)
                    _ReferedColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    _ReferedColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ForeignKeyCreator))
            {
                if (value == null)
                    ForeignKeyCreator = default(OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink);
                else
                    ForeignKeyCreator = (OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ReferedTable))
            {
                if (value == null)
                    ReferedTable = default(OOAdvantech.RDBMSMetaDataRepository.Table);
                else
                    ReferedTable = (OOAdvantech.RDBMSMetaDataRepository.Table)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Identity))
                return _Identity;

            if (member.Name == nameof(_IsPrimaryKey))
                return _IsPrimaryKey;

            if (member.Name == nameof(OriginTable))
                return OriginTable;

            if (member.Name == nameof(PrimaryKeyCreator))
                return PrimaryKeyCreator;

            if (member.Name == nameof(_DataBaseKeyName))
                return _DataBaseKeyName;

            if (member.Name == nameof(_Columns))
                return _Columns;

            if (member.Name == nameof(_ReferedColumns))
                return _ReferedColumns;

            if (member.Name == nameof(ForeignKeyCreator))
                return ForeignKeyCreator;

            if (member.Name == nameof(ReferedTable))
                return ReferedTable;


            return base.GetMemberValue(token, member);
        }

        string _Identity;
        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{C8137D8A-E3AA-4F2F-98B6-700F2BFC03FF}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                //return base.Identity;

                if (IsPrimaryKey)
                {
                    string identity = "primarykey";
                    foreach (Column column in _Columns)
                        identity += "_" + column.DataBaseColumnName.ToLower();

                    return new MetaObjectID(identity.ToLower());
                }
                else
                {
                    string identity = "foreignkey";
                    foreach (Column column in _Columns)
                    {
                        if(column.Namespace !=null)
                            identity += "_" + (column.Namespace as Table).DataBaseTableName.ToLower() + "_" + column.DataBaseColumnName.ToLower();
                        else
                            if(OriginTable!=null)
                                identity += "_" + OriginTable.DataBaseTableName.ToLower() + "_" + column.DataBaseColumnName.ToLower();
                        
                    }
                    foreach (Column column in _ReferedColumns)
                    {
                        if (column.Namespace != null)
                            identity += "_" + (column.Namespace as Table).DataBaseTableName.ToLower() + "_" + column.DataBaseColumnName.ToLower();
                        else if (ReferedTable != null)
                            identity += "_" + ReferedTable.DataBaseTableName.ToLower() + "_" + column.DataBaseColumnName.ToLower();
                    }
                    if (string.IsNullOrEmpty(identity))
                    {
                        if (_Identity == null)
                            _Identity = System.Guid.NewGuid().ToString();
                        return new MetaObjectID(_Identity);
                    }
                    else
                    {
                        _Identity = identity;
                        return new MetaDataRepository.MetaObjectID(identity.ToLower());
                    }

                }
                
                if (_DataBaseKeyName == null || _DataBaseKeyName.Length == 0)
                    return new MetaDataRepository.MetaObjectID(_Name.ToLower());
                else
                    return new MetaDataRepository.MetaObjectID(_DataBaseKeyName.ToLower());

            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{787334EC-2CF1-4DCC-AB6F-9ECE0E704816}</MetaDataID>
        private bool _IsPrimaryKey;
        /// <MetaDataID>{8694A0E0-E356-4D82-B301-D59E57CA0FC5}</MetaDataID>
        [BackwardCompatibilityID("+15")]
        [PersistentMember("_IsPrimaryKey")]
        public bool IsPrimaryKey
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _IsPrimaryKey;
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
                    _IsPrimaryKey = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        //error prone thread safety
        /// <MetaDataID>{4182A1BD-E6B2-483A-8EE2-744AAB81C89F}</MetaDataID>
        [Association("TableForeignKeys", typeof(OOAdvantech.RDBMSMetaDataRepository.Table), Roles.RoleB, "{D608DA75-891E-40D2-94FD-25A4B3C5F493}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("OriginTable")]
        [RoleBMultiplicityRange(1, 1)]
        public Table OriginTable;
        /// <MetaDataID>{08957AA1-4884-4A59-BEB8-99AC0D8F327E}</MetaDataID>
        [Association("TablePrimaryKey", typeof(OOAdvantech.RDBMSMetaDataRepository.Table), Roles.RoleA, "{6093CC6C-A8E5-4CDA-ACA8-DE48AA605483}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_PrimaryKey")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public Table PrimaryKeyCreator;
        /// <MetaDataID>{053D8349-9A9C-4DEC-AA82-31C3E83F9FF3}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private string _DataBaseKeyName;
        /// <MetaDataID>{5832E22E-1635-4561-BA6C-DCDDBFACCD60}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        [PersistentMember(255, "_DataBaseKeyName")]
        public string DataBaseKeyName
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    //if (_DataBaseKeyName != null)
                        return _DataBaseKeyName;
                    //else
                    //    return Name;
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
                    if (_DataBaseKeyName != value)
                    {
                        using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        {
                            _DataBaseKeyName = value;
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
        /// <MetaDataID>{C59518A4-4973-4B18-9E0E-EB081C28E491}</MetaDataID>
        public void AddColumn(Column Column)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _Columns.Add(Column);
                    StateTransition.Consistent = true; ;
                }

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{7827B105-C84D-42BE-975A-705701D81D5C}</MetaDataID>
        public void RemoveColumn(Column Column)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    _Columns.Remove(Column);
                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }

        //TODO Πρέπει να είναι ordered
        /// <MetaDataID>{0C615CE3-F78B-4B78-A5D4-492CF1C79791}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Collections.Generic.Set<Column> _Columns;
        /// <MetaDataID>{CC1D48F4-6E9D-4B2A-8FFF-04A2BFE180D3}</MetaDataID>
        [Association("KeyColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleA, true, "{A757D927-3907-4868-90F0-8F14CD9AA57B}")]
        [PersistentMember("_Columns")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<Column> Columns
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new OOAdvantech.Collections.Generic.Set<Column>(_Columns, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{38EBBCC0-2083-4A4C-BFFC-F65774657950}</MetaDataID>
        public Key()
        {
            _ReferedColumns = new OOAdvantech.Collections.Generic.Set<Column>();
            _Columns = new OOAdvantech.Collections.Generic.Set<Column>();
        }

        /// <MetaDataID>{D9E3714C-D9D4-405C-AD01-648D11BDFDAD}</MetaDataID>
        public void AddReferedColumn(Column ReferedColumn)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _ReferedColumns.Add(ReferedColumn);
                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{A6009DBA-B35A-4BED-AD3B-965234AB451E}</MetaDataID>
        public void RemoveReferedColumn(Column ReferedColumn)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _ReferedColumns.Remove(ReferedColumn);
                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        //TODO Πρέπει να είναι ordered
        /// <MetaDataID>{AED26434-22CC-4D2B-B631-8EB28D81BE1D}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Collections.Generic.Set<Column> _ReferedColumns = new OOAdvantech.Collections.Generic.Set<Column>();
        /// <MetaDataID>{5DBE2F4E-2895-4374-84D1-01583CF7C474}</MetaDataID>
        [Association("ReferenceColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleA,true, "{F1BC3F5E-BED1-4F8C-B12B-A1A219EFC750}")]
        [PersistentMember("_ReferedColumns")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<Column> ReferedColumns
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new OOAdvantech.Collections.Generic.Set<Column>(_ReferedColumns);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{6647970F-0168-4801-A846-BDD790E24DC1}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }
        //Error Prone thread Safety
        /// <MetaDataID>{40C0F5CF-5637-44E4-A5F0-65EA5D107541}</MetaDataID>
        [Association("AssociationEndForeignKeys", typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink), Roles.RoleB, "{B6930749-B236-4342-8A32-0B278E017352}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("ForeignKeyCreator")]
        [RoleBMultiplicityRange(0, 1)]
        public StorageCellsLink ForeignKeyCreator;



        /// <MetaDataID>{34372FB7-729E-4FEE-930C-637D4FA48F35}</MetaDataID>
        [Association("ReferenceTable", typeof(OOAdvantech.RDBMSMetaDataRepository.Table), Roles.RoleA, "{E86A6CAB-1E94-4E49-B7DC-FA4ECDFAFAB7}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("ReferedTable")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(0)]
        public Table ReferedTable;
    }
}
