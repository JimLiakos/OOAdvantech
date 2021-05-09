namespace OOAdvantech.RDBMSMetaDataRepository
{
    using MetaDataRepository;
    using OOAdvantech.Transactions;
    using System.Collections.Generic;
    using System.Linq;

    /// <MetaDataID>{D34A93F6-EBDE-464F-857D-16829A20A595}</MetaDataID>
    /// <summary>
    /// The Table defines a place in database. 
    /// In table stored the instances of class or a part of instances. 
    /// Class mapped in one or more tables, 
    /// also in same table can be mapped one or more 
    /// classes in proportion of mapping model. 
    /// </summary>
    [BackwardCompatibilityID("{D34A93F6-EBDE-464F-857D-16829A20A595}")]
    [Persistent()]
    public class Table : MetaDataRepository.Namespace
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_TableCreator))
            {
                if (value == null)
                    _TableCreator = default(OOAdvantech.MetaDataRepository.MetaObject);
                else
                    _TableCreator = (OOAdvantech.MetaDataRepository.MetaObject)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ReferentialIntegrityColumn))
            {
                if (value == null)
                    _ReferentialIntegrityColumn = default(OOAdvantech.RDBMSMetaDataRepository.Column);
                else
                    _ReferentialIntegrityColumn = (OOAdvantech.RDBMSMetaDataRepository.Column)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectIDColumns))
            {
                if (value == null)
                    _ObjectIDColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn>);
                else
                    _ObjectIDColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.IdentityColumn>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ContainedColumns))
            {
                if (value == null)
                    _ContainedColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    _ContainedColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ForeignKeys))
            {
                if (value == null)
                    _ForeignKeys = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Key>);
                else
                    _ForeignKeys = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Key>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_PrimaryKey))
            {
                if (value == null)
                    _PrimaryKey = default(OOAdvantech.RDBMSMetaDataRepository.Key);
                else
                    _PrimaryKey = (OOAdvantech.RDBMSMetaDataRepository.Key)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_DataBaseTableName))
            {
                if (value == null)
                    _DataBaseTableName = default(string);
                else
                    _DataBaseTableName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_TableCreator))
                return _TableCreator;

            if (member.Name == nameof(_ReferentialIntegrityColumn))
                return _ReferentialIntegrityColumn;

            if (member.Name == nameof(_ObjectIDColumns))
                return _ObjectIDColumns;

            if (member.Name == nameof(_ContainedColumns))
                return _ContainedColumns;

            if (member.Name == nameof(_ForeignKeys))
                return _ForeignKeys;

            if (member.Name == nameof(_PrimaryKey))
                return _PrimaryKey;

            if (member.Name == nameof(_DataBaseTableName))
                return _DataBaseTableName;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{3d1a2335-a9c1-46eb-88e1-20bba939a75a}</MetaDataID>
        public override void MakeNameUnaryInNamesapce(MetaObject metaObject)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                bool ensureUnaryName = true;
                string unaryName = metaObject.CaseInsensitiveName;
                int count = 1;
                while (ensureUnaryName)
                {
                    ensureUnaryName = false;
                    foreach (MetaDataRepository.MetaObject ownedElement in _OwnedElements)
                    {
                        if (ownedElement == metaObject)
                            continue;
                        if (ownedElement.CaseInsensitiveName.ToLower() == metaObject.CaseInsensitiveName.ToLower())
                        {
                            metaObject.Name = unaryName + "_" + count.ToString();
                            ensureUnaryName = true;
                            count++;
                            break;
                        }
                    }
                }
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }

        }

        /// <summary>This property defines the identity of Table. </summary>
        /// <remarks>
        /// In case where there is corresponding table in relation data base 
        /// the identity of table is the identity of storage plus dot plus the name of corresponding table. 
        /// In other case the identity is the identity of storage plus dot plus name. 
        /// The identity built with this method because it is useful for synchronization 
        /// of data base schema with RDBMS mapping data. 
        /// </remarks>
        /// <MetaDataID>{06662E3D-7671-4841-A481-9B70481CC3F3}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    //if (_Identity != null)
                    //    return _Identity;
                    //if (!string.IsNullOrEmpty(MetaObjectIDStream))
                    //{
                    //    _Identity = new MetaObjectID(MetaObjectIDStream);
                    //    return _Identity;
                    //}

                    MetaDataRepository.MetaObjectID identity = null;
                    if (_DataBaseTableName != null && _DataBaseTableName.Trim().Length > 0)
                    {
                        if (Namespace != null)
                            identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + _DataBaseTableName);
                        else
                            identity = new MetaDataRepository.MetaObjectID(_DataBaseTableName);
                    }
                    else
                    {
                        if (Namespace != null)
                            identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + Name);
                        else
                            identity = new MetaDataRepository.MetaObjectID(Name);

                        return new MetaObjectID(identity.ToString().ToLower().Trim());
                    }
                    identity = new MetaObjectID(identity.ToString().ToLower().Trim());
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
        /// <MetaDataID>{70A94D59-2B59-4DFC-AE88-D96E5DF410D8}</MetaDataID>
        private MetaObject _TableCreator;
        ///<summary>
        ///This property defines the Meta object which creates the table.
        ///The table creator is a bridge between relational world table 
        ///and object oriented world class if table stores objects 
        ///or association if table store objects links.
        ///</summary>
        /// <MetaDataID>{61D241D9-4229-4E79-B095-9C4460DECB23}</MetaDataID>
        [Association("RDBMSTableOwnership", typeof(OOAdvantech.MetaDataRepository.MetaObject), Roles.RoleA, "{4D230AD9-81AF-4E84-9F15-FBA4BAA1DACB}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_TableCreator")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(0)]
        public MetaObject TableCreator
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _TableCreator;

                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7B3EDE73-F339-45EE-8304-F36F8BD2A8B9}</MetaDataID>
        private Column _ReferentialIntegrityColumn;
        /// <MetaDataID>{5CF39D9F-7323-4EC2-80BB-4C83C78987CD}</MetaDataID>
        [Association("ReferentialIntegrityColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleA, "{764D613D-687E-4FFD-9DC1-C3641FBEE2CE}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_ReferentialIntegrityColumn")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0)]
        public Column ReferentialIntegrityColumn
        {
            set
            {

                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_ReferentialIntegrityColumn == value)
                        return;
                    if (_ReferentialIntegrityColumn != null && value != null && value != _ReferentialIntegrityColumn)
                        throw new System.Exception("Referential integrity column already exist");

                    using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        if (_ReferentialIntegrityColumn == null && value != null)
                            AddOwnedElement(value);
                        if (_ReferentialIntegrityColumn != null && value == null)
                            RemoveOwnedElement(_ReferentialIntegrityColumn);
                        _ReferentialIntegrityColumn = value;
                        stateTransition.Consistent = true;
                    }

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _ReferentialIntegrityColumn;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{CCDA4CE9-6F53-4A1F-9E3E-DA981B6D6D4C}</MetaDataID>
        private Collections.Generic.Set<IdentityColumn> _ObjectIDColumns = new OOAdvantech.Collections.Generic.Set<IdentityColumn>();
        /// <MetaDataID>{34632C23-973D-49B3-A29D-A4135750C045}</MetaDataID>
        [Association("TableObjectIDColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.IdentityColumn), Roles.RoleA, "{4F5C824C-6713-4EC0-8603-5E1F8A9A17F8}")]
        [PersistentMember("_ObjectIDColumns")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        public Collections.Generic.Set<IdentityColumn> ObjectIDColumns
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new OOAdvantech.Collections.Generic.Set<IdentityColumn>(_ObjectIDColumns, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4CE412DA-D061-497A-B014-DBDB9222FEC9}</MetaDataID>
        private Collections.Generic.Set<Column> _ContainedColumns = new OOAdvantech.Collections.Generic.Set<Column>();
        /// <MetaDataID>{7DFB8619-A090-485A-8937-5C2900FCB1E9}</MetaDataID>
        [Association("TableColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleA, "{1A115B08-D2F5-463E-8170-93D92445C7B0}")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [PersistentMember("_ContainedColumns")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<Column> ContainedColumns
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if(FullName== "FlavourBusinesses.TFlavourBusinessesOrganization")
                    {

                    }

                    return _ContainedColumns.ToThreadSafeSet();// new Collections.Generic.Set<Column>(_ContainedColumns, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{32E71DAD-B8B1-4C24-A7AD-3411232A1734}</MetaDataID>
        private Collections.Generic.Set<Key> _ForeignKeys = new OOAdvantech.Collections.Generic.Set<Key>();
        /// <MetaDataID>{70DADE89-A836-4A84-B0A8-36C769A9ADC5}</MetaDataID>
        [Association("TableForeignKeys", typeof(OOAdvantech.RDBMSMetaDataRepository.Key), Roles.RoleA, "{D608DA75-891E-40D2-94FD-25A4B3C5F493}")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [PersistentMember("_ForeignKeys")]
        [RoleAMultiplicityRange(0)]
        public Collections.Generic.Set<Key> ForeignKeys
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new OOAdvantech.Collections.Generic.Set<Key>(_ForeignKeys, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{22E5E70D-5243-4293-AFDB-365FB79AC245}</MetaDataID>
        private Key _PrimaryKey;
        /// <MetaDataID>{0641A819-82A9-4EC1-AB38-D70719DA1676}</MetaDataID>
        [Association("TablePrimaryKey", typeof(OOAdvantech.RDBMSMetaDataRepository.Key), Roles.RoleA, "{6E5344B0-1F03-4926-AFBF-4BBB8A5255BF}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_PrimaryKey")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(0)]
        public Key PrimaryKey
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _PrimaryKey;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5F1C6A37-54B5-44BD-9188-829677253AA4}</MetaDataID>
        private string _DataBaseTableName;
        /// <summary>Define the name of table in database. </summary>
        /// <remarks>
        /// It is useful because when we register an assembly in storage 
        /// which already exist. We force the system to update the mapping data 
        /// at first time and then to change the schema of data base. 
        /// If the name of table changes the DataBaseTableName gives to the system the chance 
        /// to find the table in database schema and change its name at second step of registration.
        /// </remarks>
        /// <MetaDataID>{8CC7BD5F-3EFC-4593-83AB-B13A0AD90D35}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        [PersistentMember(255, "_DataBaseTableName")]
        public string DataBaseTableName
        {

            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_DataBaseTableName != null)
                        return _DataBaseTableName;
                    else
                        return Name;
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
                    if (_DataBaseTableName != value)
                    {
                        using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        {
                            _DataBaseTableName = value;
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

        /// <MetaDataID>{4AFA6570-9C0E-44E0-9AE3-4A92F55B789E}</MetaDataID>
        public Table(string name, StorageCellsLink tableCreator)
        {
            _Name = name;
            _TableCreator = tableCreator;
            _TableCreator.Namespace.AddOwnedElement(this);
            _Namespace.Value = TableCreator.Namespace;
            (_Namespace.Value as Storage).MakeNameUnaryInNamesapce(this);
            _DataBaseTableName = _Name;
        }

        /// <MetaDataID>{770A89E9-858F-4D06-A979-EC1FBCC1EBD0}</MetaDataID>
        public Table(string name, StorageCell tableCreator)
        {
            _TableCreator = tableCreator;
            _TableCreator.Namespace.AddOwnedElement(this);
            _Namespace.Value = TableCreator.Namespace;
            _Name = name;
            (_Namespace.Value as Storage).MakeNameUnaryInNamesapce(this);
            _DataBaseTableName = _Name;
        }

        /// <MetaDataID>{A98CF321-8CB5-40E5-A634-2754CA0C79C1}</MetaDataID>
        public Table()
        {
        }





        /// <MetaDataID>{FDD582F2-56B6-4ECB-BCC3-59443F8F15C8}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }


        /// <MetaDataID>{880E3BF6-9D47-4ED7-A9E2-CB6368A600EA}</MetaDataID>
        public void RemoveForeignKey(Key ForeignKey)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _ForeignKeys.Remove(ForeignKey);
                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <MetaDataID>{9886EC45-72F4-47D3-9105-A4A1B9C13724}</MetaDataID>
        public void AddForeignKey(Key ForeignKey)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    ForeignKey.IsPrimaryKey = false;
                    _ForeignKeys.Add(ForeignKey);
                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }



        /// <MetaDataID>{60CC0F0A-7C18-4DA4-90AC-A4AF0BEE67AE}</MetaDataID>
        public void BuildMappingElement()
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if ((TableCreator is StorageCell) && !(TableCreator as StorageCell).Type.StorageCells.Contains(TableCreator as StorageCell))
                    throw new System.Exception("The '" + TableCreator.Name + "' StorageCell isn't initialized properly ");

                #region Create the standard columns and primary key for a mapping Table if there aren't
                if (_ObjectIDColumns.Count == 0)
                {

                    _PrimaryKey = (Key)PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(Key));
                    _PrimaryKey.PrimaryKeyCreator = this;
                    //_PrimaryKey.OriginTable=this;
                    _PrimaryKey.IsPrimaryKey = true;
                    using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        var objectIdentityType = AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this);
                        foreach (MetaDataRepository.IdentityPart identityPart in objectIdentityType.Parts)
                        {
                            IdentityColumn identityColumn = AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetIdentityColumn(identityPart, this);
                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(identityColumn);
                            _ObjectIDColumns.Add(identityColumn);
                            AddOwnedElement(identityColumn);
                            _PrimaryKey.AddColumn(identityColumn);
                            identityColumn.AllowNulls = false;
                            identityColumn.ObjectIdentityType = objectIdentityType;
                        }
                        foreach (Column column in AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumns(this))
                        {
                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                            column.AllowNulls = false;
                            AddColumn(column);
                        }
                        StateTransition.Consistent = true;
                    }

                }
                #endregion

                Name = (Namespace as Storage).TablePrefix + (TableCreator as StorageCell).Type.Name;

                (Namespace as Storage).MakeNameUnaryInNamesapce(this);
                _PrimaryKey.Name = "PK_" + Name;


            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }


        }
        /// <MetaDataID>{8CCBD2E2-D394-41AF-96A5-421FFC7A9D87}</MetaDataID>
        public void RemoveColumn(Column column)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    if (_ContainedColumns.Contains(column))
                    {
                        _ContainedColumns.Remove(column);
                        RemoveOwnedElement(column);
                    }
                    StateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{B0F95CA2-A2E2-4E94-ADE6-CB786FFD1537}</MetaDataID>
        public void AddColumn(Column Column)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            List<Column> removeColumns = new List<Column>();

            try
            {
                foreach (Column currColumn in _ContainedColumns)
                {
                    if (Column.Name == currColumn.Name)
                        if (Column != currColumn)
                        {

                            if (Column.MappedAttribute != null && Column.MappedAttribute == currColumn.MappedAttribute)
                            {
                                bool creatorDoesNotExist = false;
                                ValueTypePath valueTypePath = new ValueTypePath(currColumn.CreatorIdentity);
                                foreach (MetaDataRepository.MetaObjectID identity in valueTypePath.ToArray())
                                {
                                    if (MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(identity.ToString(), this) == null)
                                    {
                                        creatorDoesNotExist = true;
                                        break;
                                    }
                                }
                                if (creatorDoesNotExist)
                                {
                                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                                    {
                                        _ContainedColumns.Remove(currColumn);
                                        RemoveOwnedElement(currColumn);
                                        currColumn.MappedAttribute = null;
                                        stateTransition.Consistent = true;
                                    }
                                    continue;
                                }

                            }
                            if (Column.MappedAssociationEnd != null && Column.MappedAssociationEnd == currColumn.MappedAssociationEnd)
                            {
                                bool creatorDoesNotExist = false;
                                ValueTypePath valueTypePath = new ValueTypePath(currColumn.CreatorIdentity);
                                foreach (MetaDataRepository.MetaObjectID identity in valueTypePath.ToArray())
                                {
                                    if (MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(identity.ToString(), this) == null)
                                    {
                                        creatorDoesNotExist = true;
                                        break;
                                    }
                                }
                                if (creatorDoesNotExist)
                                {

                                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                                    {
                                        _ContainedColumns.Remove(currColumn);
                                        RemoveOwnedElement(currColumn);
                                        currColumn.MappedAssociationEnd = null;
                                        stateTransition.Consistent = true;
                                    }

                                    continue;
                                }
                            }

                            if (currColumn.MappedAttribute != null && currColumn.MappedAttribute.Owner == null)
                                removeColumns.Add(currColumn);
                            else if (currColumn.MappedAssociationEnd != null && currColumn.MappedAssociationEnd.Namespace == null)
                                removeColumns.Add(currColumn);
                            else if(currColumn.MappedAssociationEnd==null&&
                                currColumn.MappedAttribute==null&& 
                                !AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumns(this).Select(x=>x.Name).ToList().Contains(Column.Name))
                            {
                                removeColumns.Add(currColumn);
                            }
                            else 
                                throw new System.Exception("Column '" + Column.Name + "' already exists.");
                        }
                }
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, Transactions.TransactionOption.Supported))
                {
                    foreach (var removeColumn in removeColumns)
                    {
                        removeColumn.MappedAttribute = null;
                        removeColumn.MappedAssociationEnd = null;
                        _ContainedColumns.Remove(removeColumn);
                        RemoveOwnedElement(removeColumn);
                        OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(removeColumn);
                    }
                    _ContainedColumns.Add(Column);
                    AddOwnedElement(Column);
                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <MetaDataID>{259555d6-5fce-4b5a-840b-dbda39489c9b}</MetaDataID>
        public void RemoveObjectIDColumn(IdentityColumn column)
        {
            using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
            {
                if (_ObjectIDColumns.Contains(column))
                {
                    _ObjectIDColumns.Remove(column);
                    RemoveOwnedElement(column);
                    if (_PrimaryKey != null && _PrimaryKey.Columns.Contains(column))
                        _PrimaryKey.RemoveColumn(column);
                }
                StateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{c2733ab2-fd3e-494e-bffa-bc7c87443dc6}</MetaDataID>
        public void AddObjectIDColumn(string columnName, System.Type dataType, int lenght, bool isIdentityColumn, int identityIncrement)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                Classifier classifier = Classifier.GetClassifier(dataType);
                IdentityColumn identityColumn = OOAdvantech.RDBMSMetaDataRepository.AutoProduceColumnsGenerator.GetObjectIdentityColumn(columnName, classifier, this);
                identityColumn.Name = columnName;
                identityColumn.Length = lenght;
                identityColumn.IsIdentity = isIdentityColumn;
                identityColumn.IdentityIncrement = identityIncrement;

                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(identityColumn);
                _ObjectIDColumns.Add(identityColumn);
                AddOwnedElement(identityColumn);
                if (_PrimaryKey == null)
                {
                    _PrimaryKey = (Key)PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(Key));
                    _PrimaryKey.PrimaryKeyCreator = this;
                    _PrimaryKey.IsPrimaryKey = true;
                    _PrimaryKey.Name = "PK_" + Name;
                }
                _PrimaryKey.AddColumn(identityColumn);



                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{c4b8d19b-4381-48b6-9050-5401268f9d91}</MetaDataID>
        public Column GetColumn(string columnName)
        {
            foreach (Column column in _ContainedColumns)
                if (column.Name.ToLower() == columnName.ToLower())
                    return column;

            return null;
        }

        /// <MetaDataID>{6c7ae0e8-f914-48a7-8e64-e1514f603287}</MetaDataID>
        public bool ContainColumn(string columnName)
        {
            foreach (Column column in _ContainedColumns)
            {
                if (column.Name.ToLower() == columnName)
                    return true;
            }
            return false;
        }

        /// <MetaDataID>{9dcb2af3-938c-454c-8e40-368e9c79423b}</MetaDataID>
        internal ObjectIdentityType GetObjectIdentityType(IdentityColumn identityColumn)
        {
            System.Collections.Generic.List<IIdentityPart> parts = null;
            bool objectIdentityTypeFinded = false;
            foreach (var foreignKey in ForeignKeys)
            {
                parts = new System.Collections.Generic.List<IIdentityPart>();
                objectIdentityTypeFinded = false;
                foreach (IdentityColumn column in foreignKey.Columns)
                {
                    parts.Add(column);
                    if (column == identityColumn)
                        objectIdentityTypeFinded = true;
                }
                if (objectIdentityTypeFinded)
                    return new ObjectIdentityType(parts);
            }
            parts = new System.Collections.Generic.List<IIdentityPart>();
            foreach (IdentityColumn column in ObjectIDColumns)
            {
                parts.Add(column);
                if (column == identityColumn)
                    objectIdentityTypeFinded = true;
            }
            if (objectIdentityTypeFinded)
                return new ObjectIdentityType(parts);

            if (identityColumn.MappedAssociationEnd != null)
            {
                //parts = new System.Collections.Generic.List<IIdentityPart>();

                return identityColumn.MappedAssociationEnd.GetReferenceObjectIdentityType();
            }

            return null;
        }
    }
}
