namespace OOAdvantech.RDBMSMetaDataRepository
{
    using MetaDataRepository;
    using System.Linq;
    using OOAdvantech.Transactions;
    using System.Collections.Generic;
    /// <MetaDataID>{1AF24240-9F49-4291-9BA1-7CF8786B8345}</MetaDataID>
    [BackwardCompatibilityID("{1AF24240-9F49-4291-9BA1-7CF8786B8345}")]
    [Persistent()]
    public class StorageCellsLink : MetaDataRepository.StorageCellsLink
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_RoleBHasReferencialIntegrity))
            {
                if (value == null)
                    _RoleBHasReferencialIntegrity = default(bool);
                else
                    _RoleBHasReferencialIntegrity = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AutoRelationTableGeneration))
            {
                if (value == null)
                    AutoRelationTableGeneration = default(bool);
                else
                    AutoRelationTableGeneration = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RoleAHasReferencialIntegrity))
            {
                if (value == null)
                    _RoleAHasReferencialIntegrity = default(bool);
                else
                    _RoleAHasReferencialIntegrity = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectsLinksCount))
            {
                if (value == null)
                    _ObjectsLinksCount = default(int);
                else
                    _ObjectsLinksCount = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RoleBMultiplicityIsMany))
            {
                if (value == null)
                    _RoleBMultiplicityIsMany = default(bool);
                else
                    _RoleBMultiplicityIsMany = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RoleAMultiplicityIsMany))
            {
                if (value == null)
                    _RoleAMultiplicityIsMany = default(bool);
                else
                    _RoleAMultiplicityIsMany = (bool)value;
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
            if (member.Name == nameof(_NewTableCreated))
            {
                if (value == null)
                    _NewTableCreated = default(bool);
                else
                    _NewTableCreated = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_IsFull))
            {
                if (value == null)
                    _IsFull = default(bool);
                else
                    _IsFull = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectLinksTable))
            {
                if (value == null)
                    _ObjectLinksTable = default(OOAdvantech.RDBMSMetaDataRepository.Table);
                else
                    _ObjectLinksTable = (OOAdvantech.RDBMSMetaDataRepository.Table)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_RoleBHasReferencialIntegrity))
                return _RoleBHasReferencialIntegrity;

            if (member.Name == nameof(AutoRelationTableGeneration))
                return AutoRelationTableGeneration;

            if (member.Name == nameof(_RoleAHasReferencialIntegrity))
                return _RoleAHasReferencialIntegrity;

            if (member.Name == nameof(_ObjectsLinksCount))
                return _ObjectsLinksCount;

            if (member.Name == nameof(_RoleBMultiplicityIsMany))
                return _RoleBMultiplicityIsMany;

            if (member.Name == nameof(_RoleAMultiplicityIsMany))
                return _RoleAMultiplicityIsMany;

            if (member.Name == nameof(_ForeignKeys))
                return _ForeignKeys;

            if (member.Name == nameof(_NewTableCreated))
                return _NewTableCreated;

            if (member.Name == nameof(_IsFull))
                return _IsFull;

            if (member.Name == nameof(_ObjectLinksTable))
                return _ObjectLinksTable;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{f5d25b7b-90c3-42e7-b9e1-4881b4bebcdd}</MetaDataID>
        public List<Table> UsedAssociationTables
        {
            get
            {
                List<Table> tables = new List<Table>();
                if (_ObjectLinksTable != null)
                {
                    tables.Add(_ObjectLinksTable);
                    return tables;
                }

                foreach (StorageCell storageCell in this.AssotiationClassStorageCells)
                    tables.Add(storageCell.MainTable);

                return tables;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{FB7FBDB8-63EE-4F3D-8886-224E6DE72BE8}</MetaDataID>
        private bool _RoleBHasReferencialIntegrity;
        /// <MetaDataID>{D2DE40D0-ABF3-45DD-9AE0-0C71AE4FA1F8}</MetaDataID>
        [BackwardCompatibilityID("+25")]
        [PersistentMember("_RoleBHasReferencialIntegrity")]
        public bool RoleBHasReferencialIntegrity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _RoleBHasReferencialIntegrity;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{882bc374-da85-4fee-929b-8c85220a9be9}</MetaDataID>
        [BackwardCompatibilityID("+26")]
        [PersistentMember("AutoRelationTableGeneration ")]
        public bool AutoRelationTableGeneration = true;

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DB833B8D-479D-47FE-AFC2-F93EB4FA5645}</MetaDataID>
        private bool _RoleAHasReferencialIntegrity;
        /// <MetaDataID>{1A180033-943B-427A-B261-DFEEC29A59AC}</MetaDataID>
        [BackwardCompatibilityID("+24")]
        [PersistentMember("_RoleAHasReferencialIntegrity")]
        public bool RoleAHasReferencialIntegrity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _RoleAHasReferencialIntegrity;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7513766F-C066-44D1-BC4E-50AD0A9E97C4}</MetaDataID>
        private int _ObjectsLinksCount = -1;
        /// <summary>Specify the number of object links. 
        /// It is valid only for the ManytoMany association of history classes. 
        /// It is transient and initialized from the the storage provider 
        /// when the storage opening at first time. 
        /// Before this time has value -1. </summary>
        /// <MetaDataID>{730D81C9-BB69-430A-98AB-86B63EB277A3}</MetaDataID>
        public int ObjectsLinksCount
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _ObjectsLinksCount;

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
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ObjectsLinksCount = value;
                        stateTransition.Consistent = true;
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{20BEE680-F985-4109-8A98-D5E2F0E7E172}</MetaDataID>
        public OOAdvantech.MetaDataRepository.AssociationType MultiplicityType
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (RoleAMultiplicityIsMany)
                    {
                        if (RoleBMultiplicityIsMany)
                        {
                            return AssociationType.ManyToMany;
                        }
                        else
                        {
                            return AssociationType.ManyToOne;
                        }
                    }
                    else
                    {
                        if (RoleBMultiplicityIsMany)
                        {
                            return AssociationType.OneToMany;
                        }
                        else
                        {
                            return AssociationType.OneToOne;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{191F8A5A-E810-42B9-82ED-3B0F6A08816D}</MetaDataID>
        private bool _RoleBMultiplicityIsMany;
        /// <MetaDataID>{E17F2D55-C3F2-4F16-BE91-6ED6F9901A63}</MetaDataID>
        [BackwardCompatibilityID("+21")]
        [PersistentMember("_RoleBMultiplicityIsMany")]
        public bool RoleBMultiplicityIsMany
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _RoleBMultiplicityIsMany;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9E93D22D-126E-4D74-B404-8B0D5B4E0AEF}</MetaDataID>
        private bool _RoleAMultiplicityIsMany;
        /// <MetaDataID>{7F09141F-852B-4AFE-A34B-B183A2835F62}</MetaDataID>
        [BackwardCompatibilityID("+20")]
        [PersistentMember("_RoleAMultiplicityIsMany")]
        public bool RoleAMultiplicityIsMany
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _RoleAMultiplicityIsMany;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        ///<summary>
        ///
        ///</summary>
        /// <MetaDataID>{8C6564DD-57A2-45C8-BB43-0849A0A76BAD}</MetaDataID>
        public void UpdateRolesSate()
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _RoleAMultiplicityIsMany = _Type.RoleA.Multiplicity.IsMany;
                    _RoleBMultiplicityIsMany = _Type.RoleB.Multiplicity.IsMany;
                    _RoleAHasReferencialIntegrity = RoleBStorageCell.Type.HasReferentialIntegrity(_Type.RoleA);
                    _RoleBHasReferencialIntegrity = RoleAStorageCell.Type.HasReferentialIntegrity(_Type.RoleB);
                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C985D24B-52AC-4F1E-A6E9-3CB4A0C275F4}</MetaDataID>
        private Collections.Generic.Set<Key> _ForeignKeys = new OOAdvantech.Collections.Generic.Set<Key>();
        /// <MetaDataID>{BD198996-8969-4011-B275-2FFDC48263FF}</MetaDataID>
        [MetaDataRepository.Association("AssociationEndForeignKeys", typeof(OOAdvantech.RDBMSMetaDataRepository.Key), Roles.RoleA, "{B6930749-B236-4342-8A32-0B278E017352}")]
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


        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{7F394963-AF9D-48B7-BCE0-DC9CF7F92DF7}</MetaDataID>
        //private OOAdvantech.MetaDataRepository.MetaObjectCollection _AssotiationClassStorageCells = new OOAdvantech.MetaDataRepository.MetaObjectCollection();
        ///// <MetaDataID>{81C40A36-4CE5-44AA-81F3-A2E36C59587C}</MetaDataID>
        //[Association("StorageCellLinkAssotiationClass", typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell), Roles.RoleA, "{5C183C1D-BCD0-4BFA-92A3-40EB55B51C22}")]
        //[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        //[PersistentMember("_AssotiationClassStorageCells")]
        //[RoleAMultiplicityRange(0)]
        //[RoleBMultiplicityRange(0)]
        //public OOAdvantech.MetaDataRepository.MetaObjectCollection AssotiationClassStorageCells
        //{
        //    get
        //    {
        //        try
        //        {
        //            ReaderWriterLock.AcquireReaderLock(10000);
        //            return new MetaDataRepository.MetaObjectCollection(_AssotiationClassStorageCells, MetaDataRepository.MetaObjectCollection.AccessType.ReadOnly);
        //        }
        //        finally
        //        {
        //            ReaderWriterLock.ReleaseReaderLock();
        //        }
        //    }
        //}

        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{5BD73705-0DFC-47AF-A534-6EFA1811361E}</MetaDataID>
        //private StorageCell _RoleBStorageCell;
        ///// <MetaDataID>{561D1BB5-C2EB-4F29-8134-DE794C03AC4E}</MetaDataID>
        //[Association("RoleBStorageCell",typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell),Roles.RoleA,"{BDE55936-1DA1-4D6A-B8A4-446BF1141003}")]
        //[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        //[PersistentMember("_RoleBStorageCell")]
        //[RoleAMultiplicityRange(1,1)]
        //[RoleBMultiplicityRange(0)]
        //public StorageCell RoleBStorageCell
        //{
        //    get
        //    {
        //        ReaderWriterLock.AcquireReaderLock(10000);
        //        try
        //        {
        //            return _RoleBStorageCell;
        //        }
        //        finally
        //        {
        //            ReaderWriterLock.ReleaseReaderLock();
        //        }

        //    }
        //}
        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{E04E9341-062E-4313-925F-F8A70D0FAB6E}</MetaDataID>
        //private StorageCell _RoleAStorageCell;
        ///// <MetaDataID>{5CB88264-1308-48B9-874A-60FBEA5063F3}</MetaDataID>
        //[Association("RoleAStorageCell",typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell),Roles.RoleA,"{25B36609-E774-4969-B9BB-BC113E859E61}")]
        //[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        //[PersistentMember("_RoleAStorageCell")]
        //[RoleAMultiplicityRange(1,1)]
        //[RoleBMultiplicityRange(0)]
        //public StorageCell RoleAStorageCell
        //{
        //    get
        //    {
        //        ReaderWriterLock.AcquireReaderLock(10000);
        //        try
        //        {
        //            return _RoleAStorageCell;
        //        }
        //        finally
        //        {
        //            ReaderWriterLock.ReleaseReaderLock();
        //        }

        //    }
        //}
        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{D1155609-8E9A-4B5E-A69A-5C37FC313A93}</MetaDataID>
        //private Association _Type;
        ///// <MetaDataID>{0659C7C7-CC96-4CFA-93E9-2B356B23B57F}</MetaDataID>
        //[Association("LinksInstanceTypeRelation",typeof(OOAdvantech.RDBMSMetaDataRepository.Association),Roles.RoleB,"{ECFC605B-F4AA-41B4-BD22-130AC73F5BEA}")]
        //[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        //[PersistentMember("_Type")]
        //[RoleBMultiplicityRange(1,1)]
        //public Association Type
        //{
        //    get
        //    {
        //        ReaderWriterLock.AcquireReaderLock(10000);
        //        try
        //        {
        //            return _Type;
        //        }
        //        finally
        //        {
        //            ReaderWriterLock.ReleaseReaderLock();
        //        }

        //    }
        //}




        /// <MetaDataID>{724626AF-55BA-402C-95A7-A4C2DA4A585F}</MetaDataID>
        public void AddAssotiationClassStorageCell(MetaDataRepository.StorageCell storageCell)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (!storageCell.Type.IsA(Type.LinkClass))
                    throw new System.Exception("Association link class type mismatch");

                if (!_AssotiationClassStorageCells.Contains(storageCell))
                {
                    using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        _AssotiationClassStorageCells.Add(storageCell);
                        stateTransition.Consistent = true;
                    }
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <MetaDataID>{55FD8784-BB52-4A9B-8BD0-5F2FE3CC313D}</MetaDataID>
        public void RemoveAssotiationClassStorageCell(StorageCell storageCell)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (_AssotiationClassStorageCells.Contains(storageCell))
                {
                    using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        _AssotiationClassStorageCells.Remove(storageCell);
                        stateTransition.Consistent = true;
                    }
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{F821DC42-465B-4CE2-BD8C-3FBA97158D7D}</MetaDataID>
        private bool _NewTableCreated = false;
        /// <MetaDataID>{15D96EC1-E32F-4F1F-B080-6F8F01DF6976}</MetaDataID>
        public bool NewTableCreated
        {
            get
            {

                if (ObjectLinksTable == null)
                    return false;
                else return _NewTableCreated;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{0334DA6B-B624-40DD-944F-88777383D4B3}</MetaDataID>
        private bool _IsFull;
        /// <MetaDataID>{BD631415-98D5-46C4-B34D-F7B53D9DCBE2}</MetaDataID>
        [BackwardCompatibilityID("+13")]
        [PersistentMember("_IsFull")]
        public bool IsFull
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_IsFull == true)
                        return _IsFull;
                    else
                    {
                        if (ObjectsLinksCount == -1)
                            return _IsFull;

                        int SplitLinkLimit = 0;
                        if ((RoleAStorageCell.Type as Class).HistoryClass)
                            SplitLinkLimit = (RoleAStorageCell.Type as Class).SplitLimit;
                        //if((Type.RoleA.Specification as Class).HistoryClass)
                        //    SplitLinkLimit=((Class)Type.RoleA.Specification).SplitLimit;
                        if ((RoleBStorageCell.Type as Class).HistoryClass)
                            if ((RoleBStorageCell.Type as Class).SplitLimit < SplitLinkLimit)
                                SplitLinkLimit = (RoleBStorageCell.Type as Class).SplitLimit;


                        //if(((Class)Type.RoleB.Specification).HistoryClass)
                        //    if(((Class)Type.RoleB.Specification).SplitLimit<SplitLinkLimit)
                        //        SplitLinkLimit=((Class)Type.RoleB.Specification).SplitLimit;

                        if (ObjectsLinksCount >= SplitLinkLimit)
                        {
                            using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                            {
                                _IsFull = true;
                                stateTransition.Consistent = true;
                            }
                        }
                        return _IsFull;

                    }

                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }

        }




        /// <MetaDataID>{F1E900D7-C203-46F2-BEC5-2C4CD2710936}</MetaDataID>
        public StorageCellsLink()
        {

        }
        /// <MetaDataID>{D6A94F62-E001-4115-A6DB-3D88F1F7477D}</MetaDataID>
        public StorageCellsLink(Association type, MetaDataRepository.StorageCell roleAStorageCell, MetaDataRepository.StorageCell roleBStorageCell)
            : base(type, roleAStorageCell, roleBStorageCell)
        {

            _RoleAMultiplicityIsMany = type.RoleA.Multiplicity.IsMany;
            _RoleBMultiplicityIsMany = type.RoleB.Multiplicity.IsMany;
            _RoleAHasReferencialIntegrity = RoleBStorageCell.Type.HasReferentialIntegrity(_Type.RoleA);
            _RoleBHasReferencialIntegrity = RoleAStorageCell.Type.HasReferentialIntegrity(_Type.RoleB);

        }
        /// <MetaDataID>{423F7F2E-1BFA-49CF-9F86-12693F440027}</MetaDataID>
        private void CreateForeignKeyIfNotExist(IList<IdentityColumn> foreignKeyColumns, Collections.Generic.Set<IdentityColumn> primaryKeyColumns, MetaDataRepository.Roles role)
        {
            Table fkOwnerTable = null;
            Table fkRefernceTable = null;
            foreach (Column column in foreignKeyColumns)
            {
                fkOwnerTable = column.Namespace as Table;
                break;
            }
            foreach (Column column in primaryKeyColumns)
            {
                fkRefernceTable = column.Namespace as Table;
                break;
            }
            bool foreignKeyAlreadyExist = false;
            if (fkOwnerTable == null)
                return;
            foreach (Key foreignKey in fkOwnerTable.ForeignKeys)
            {

                if (foreignKey.ReferedTable == fkRefernceTable)
                {
                    bool _continue = false;
                    foreach (IdentityColumn column in foreignKeyColumns)
                    {
                        foreach (IdentityColumn fkcolumn in foreignKey.Columns)
                        {
                            if (fkcolumn.ColumnType == column.ColumnType && fkcolumn != column)
                                _continue = true;
                        }
                    }
                    if (!_continue)
                    {
                        foreignKeyAlreadyExist = true;
                        break;
                    }
                }
            }
            if (foreignKeyAlreadyExist)
                return;
            else
            {
                MetaDataRepository.ValueTypePath valueTypePath = MetaDataRepository.ValueTypePath.GetValueTypePathFromString(ValueTypePath);

                string path = "";


                MetaDataRepository.MetaObjectID[] identities = valueTypePath.ToArray();
                for (int i = 0; i != valueTypePath.Count; i++)
                {
                    MetaDataRepository.MetaObjectID identity = identities[valueTypePath.Count - i - 1];
                    if (!string.IsNullOrEmpty(path))
                        path += "_";
                    path += MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(identity.ToString(), this).Name;
                }

                if (!string.IsNullOrEmpty(path))
                    path += "_";


                string foreignKeyName = null;
                if (role == MetaDataRepository.Roles.RoleA)
                    foreignKeyName = "FKA_" + fkOwnerTable.Name + "_" + path + (Type as Association).Name + "_" + fkRefernceTable.Name;
                else
                    foreignKeyName = "FKB_" + fkOwnerTable.Name + "_" + path + (Type as Association).Name + "_" + fkRefernceTable.Name;


                Key foreignKey = (Key)PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(Key));
                foreignKey.IsPrimaryKey = false;
                foreignKey.Name = foreignKeyName;
                foreignKey.ReferedTable = fkRefernceTable;
                foreignKey.OriginTable = fkOwnerTable;
                fkOwnerTable.AddForeignKey(foreignKey);
                foreignKey.ForeignKeyCreator = this;
                _ForeignKeys.Add(foreignKey);
                foreach (Column column in primaryKeyColumns)
                    foreignKey.AddReferedColumn(column);
                foreach (Column column in foreignKeyColumns)
                    foreignKey.AddColumn(column);
                _ForeignKeys.Add(foreignKey);
            }
        }
        /// <MetaDataID>{28FB1A1F-E5AD-4E13-8390-EA65C7F0A82A}</MetaDataID>
        public void UpdateReferencialIntegrityCountFor(StorageCell storageCell)
        {

            //			update StorageCellMainTable
            //			set StorageCellMainTable.ReferenceCount=StorageCellMainTable.ReferenceCount+ReferenceCountTable.ReferenceCount
            //			FROM         StorageCellMainTable INNER JOIN 
            //			(SELECT     StorageCellMainTable.ObjectID, COUNT(*) AS ReferenceCount
            //			FROM         ReferenceColumnTable INNER JOIN
            //			StorageCellMainTable ON ReferenceColumnTable.ReferenceColumn = StorageCellMainTable.ObjectID
            //			GROUP BY StorageCellMainTable.ObjectID) ReferenceCountTable ON StorageCellMainTable.ObjectID = ReferenceCountTable.ObjectID
            //
            //			ReferenceColumn =StorageCellRoleAssociationEnd.OtherEnd.GetReferenceColumn(); 

        }

        /// <MetaDataID>{617F24C6-D761-4439-91E8-264FE3001072}</MetaDataID>
        public void UpdateForeignKeys()
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (Type.RoleA == null && Type.RoleB == null) //Relation removed
                    return;
                //TODO Αυτό έαν η σχέση βρίσκεται σε class που δεν σώζει στο main table πχ one table per class
                //Τοτε ο παρακάτω κώδικας έχει σφάλμα
                using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    if (Type.LinkClass != null)
                    {

                        foreach (StorageCell linkClassStorageCell in this.AssotiationClassStorageCells)
                        {

                            System.Collections.Generic.List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                            foreach (MetaDataRepository.IIdentityPart part in (Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(linkClassStorageCell))
                                parts.Add(part);
                            MetaDataRepository.ObjectIdentityType roleAObjectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);

                            parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                            foreach (MetaDataRepository.IIdentityPart part in (Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(linkClassStorageCell))
                                parts.Add(part);
                            MetaDataRepository.ObjectIdentityType roleBObjectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);
                            if ((RoleAStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType != roleAObjectIdentityType ||
                            (RoleBStorageCell as RDBMSMetaDataRepository.StorageCell).ObjectIdentityType != roleBObjectIdentityType)
                            {
                                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                                {
                                    if (_ObjectLinksTable == null)
                                    {
                                        string path = null;
                                        path = BuiltPathString();
                                        System.Type[] ctorParameters = { typeof(string), typeof(StorageCellsLink) };
                                        _ObjectLinksTable = new Table((Namespace as Storage).TablePrefix + path + "_" + (Type as Association).Name, this);
                                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_ObjectLinksTable);
                                        (Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).AddReferenceColumnsToTable(_ObjectLinksTable, ValueTypePath, new ValueTypePath(ValueTypePath), (RoleBStorageCell as StorageCell).ObjectIdentityType);
                                        (Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).AddReferenceColumnsToTable(_ObjectLinksTable, ValueTypePath, new ValueTypePath(ValueTypePath), (RoleAStorageCell as StorageCell).ObjectIdentityType);

                                        foreach (IdentityColumn part in linkClassStorageCell.ObjectIdentityType.Parts)
                                            _ObjectLinksTable.AddObjectIDColumn(part.Name, (part as IIdentityPart).Type, part.Length, part.IsIdentity, part.IdentityIncrement);
                                    }
                                    StateTransition.Consistent = true;
                                }

                                if (!(RoleAStorageCell is StorageCellReference))
                                    CreateForeignKeyIfNotExist((Type.RoleA as AssociationEnd).GetReferenceColumnsFor(ObjectLinksTable), (RoleBStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleB);
                                if (!(RoleBStorageCell is StorageCellReference))
                                    CreateForeignKeyIfNotExist((Type.RoleB as AssociationEnd).GetReferenceColumnsFor(ObjectLinksTable), (RoleAStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleA);

                            }
                            else
                            {
                                if (!(linkClassStorageCell is StorageCellReference) && !(RoleAStorageCell is StorageCellReference))
                                    CreateForeignKeyIfNotExist((Type.RoleA as AssociationEnd).GetReferenceColumnsFor(linkClassStorageCell, ValueTypePath), (RoleBStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleB);
                                if (!(linkClassStorageCell is StorageCellReference) && !(RoleBStorageCell is StorageCellReference))
                                    CreateForeignKeyIfNotExist((Type.RoleB as AssociationEnd).GetReferenceColumnsFor(linkClassStorageCell, ValueTypePath), (RoleAStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleA);
                            }
                        }
                    }
                    if (Type.LinkClass == null && (Type.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany || RoleAStorageCell.ObjectIdentityType != RoleBStorageCell.ObjectIdentityType))
                    {
                        if (_ObjectLinksTable == null)
                        {
                            string path = null;
                            path = BuiltPathString();
                            System.Type[] ctorParameters = { typeof(string), typeof(StorageCellsLink) };
                            _ObjectLinksTable = new Table((Namespace as Storage).TablePrefix + path + (Namespace as Storage).CompositeNameSeparatorSign + (Type as Association).Name, this);
                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_ObjectLinksTable);
                            (Type.RoleA as RDBMSMetaDataRepository.AssociationEnd).AddReferenceColumnsToTable(_ObjectLinksTable, ValueTypePath, new ValueTypePath(ValueTypePath), (RoleBStorageCell as StorageCell).ObjectIdentityType);
                            (Type.RoleB as RDBMSMetaDataRepository.AssociationEnd).AddReferenceColumnsToTable(_ObjectLinksTable, ValueTypePath, new ValueTypePath(ValueTypePath), (RoleAStorageCell as StorageCell).ObjectIdentityType);

                        }
                        //else
                        //{
                        //    var mTable = Namespace.OwnedElements.OfType<Table>().Where(x => x.Name == (Namespace as Storage).TablePrefix + BuiltPathString() + (Type as Association).Name).FirstOrDefault();
                        //    if (mTable != null && mTable != _ObjectLinksTable)
                        //    {
                        //        _ObjectLinksTable = mTable;

                        //        foreach (var rtable in Namespace.OwnedElements.OfType<Table>().Where(x => x.Name.LastIndexOf((Namespace as Storage).TablePrefix + BuiltPathString() + "_" + (Type as Association).Name) == 0).ToArray())
                        //        {
                        //            Namespace.RemoveOwnedElement(rtable);
                        //            PersistenceLayer.ObjectStorage.DeleteObject(rtable);
                        //        }
                        //    }
                        //}
                        if (!(RoleBStorageCell is StorageCellReference))
                            CreateForeignKeyIfNotExist((Type.RoleA as AssociationEnd).GetReferenceColumnsFor(ObjectLinksTable), (RoleBStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleB);
                        if (!(RoleAStorageCell is StorageCellReference))
                            CreateForeignKeyIfNotExist((Type.RoleB as AssociationEnd).GetReferenceColumnsFor(ObjectLinksTable), (RoleAStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleA);
                    }
                    else if (Type.LinkClass == null && ObjectLinksTable != null)
                    {
                        if (!(RoleBStorageCell is StorageCellReference))
                            CreateForeignKeyIfNotExist((Type.RoleA as AssociationEnd).GetReferenceColumnsFor(ObjectLinksTable), (RoleBStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleB);
                        if (!(RoleAStorageCell is StorageCellReference))
                            CreateForeignKeyIfNotExist((Type.RoleB as AssociationEnd).GetReferenceColumnsFor(ObjectLinksTable), (RoleAStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleA);
                    }
                    else if (Type.LinkClass == null && Type.MultiplicityType == MetaDataRepository.AssociationType.ManyToOne)
                    {
                        if (!(RoleAStorageCell is StorageCellReference) && !(RoleBStorageCell is StorageCellReference))
                            CreateForeignKeyIfNotExist((Type.RoleA as AssociationEnd).GetReferenceColumnsFor(RoleAStorageCell as StorageCell, ValueTypePath), (RoleBStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleB);
                    }
                    else if (Type.LinkClass == null && (Type.MultiplicityType == MetaDataRepository.AssociationType.OneToOne))
                    {
                        if (!(RoleAStorageCell is StorageCellReference) && !(RoleBStorageCell is StorageCellReference))
                        {
                            if ((Type.RoleA as AssociationEnd).GetReferenceColumnsFor(RoleAStorageCell as StorageCell, ValueTypePath).Count > 0)
                                CreateForeignKeyIfNotExist((Type.RoleA as AssociationEnd).GetReferenceColumnsFor(RoleAStorageCell as StorageCell, ValueTypePath), (RoleBStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleB);
                            else
                                CreateForeignKeyIfNotExist((Type.RoleB as AssociationEnd).GetReferenceColumnsFor(RoleBStorageCell as StorageCell, ValueTypePath), (RoleAStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleA);
                        }
                    }
                    else if (Type.LinkClass == null && Type.MultiplicityType == MetaDataRepository.AssociationType.OneToMany)
                    {
                        if (!(RoleAStorageCell is StorageCellReference) && !(RoleBStorageCell is StorageCellReference))
                            CreateForeignKeyIfNotExist((Type.RoleB as AssociationEnd).GetReferenceColumnsFor(RoleBStorageCell as StorageCell, ValueTypePath), (RoleAStorageCell as StorageCell).MainTable.ObjectIDColumns, Roles.RoleA);
                    }
                    stateTransition.Consistent = true;
                }


            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }



        /// <MetaDataID>{32542856-B254-4B43-975D-FE60DDB7EB45}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Table _ObjectLinksTable;
        /// <MetaDataID>{E0A90971-38DB-47A6-945A-EAED55D5B542}</MetaDataID>
        [MetaDataRepository.Association("MappingTable", typeof(OOAdvantech.RDBMSMetaDataRepository.Table), Roles.RoleB, "{9AF50FCE-482D-4597-954D-0CFEB85F3000}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_ObjectLinksTable")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0, 1)]
        public Table ObjectLinksTable
        {
            get
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);

                try
                {
                    if (_ObjectLinksTable != null)
                        return _ObjectLinksTable;
                    _NewTableCreated = false;
                    if (Type.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany
                        || RoleAStorageCell is StorageCellReference
                        || RoleBStorageCell is StorageCellReference
                        || (RoleAStorageCell as StorageCell).ObjectIdentityType != (RoleBStorageCell as StorageCell).ObjectIdentityType)
                    {
                        if (Type.LinkClass != null)
                        {
                            return _ObjectLinksTable;
                        }

                        if (!AutoRelationTableGeneration || _ObjectLinksTable != null)
                            return _ObjectLinksTable;



                        using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        {
                            if (_ObjectLinksTable == null)
                            {
                                string path = null;
                                path = BuiltPathString();
                                System.Type[] ctorParameters = { typeof(string), typeof(StorageCellsLink) };
                                _ObjectLinksTable = new Table((Namespace as Storage).TablePrefix + path + (Namespace as Storage).CompositeNameSeparatorSign + (Type as Association).Name, this);
                                PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_ObjectLinksTable);
                            }
                            StateTransition.Consistent = true;
                        }
                        ((AssociationEnd)Type.RoleA).AddReferenceColumnsToTable(_ObjectLinksTable);
                        ((AssociationEnd)Type.RoleB).AddReferenceColumnsToTable(_ObjectLinksTable);
                        _NewTableCreated = true;
                        return _ObjectLinksTable;

                    }
                    else
                        return null;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        _ObjectLinksTable = value;
                        StateTransition.Consistent = true; ;
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }

        /// <MetaDataID>{e997a21a-7cb7-4fa3-9ee2-28dc37d3a47b}</MetaDataID>
        private string BuiltPathString()
        {
            string path = null;
            if (!string.IsNullOrEmpty(ValueTypePath))
            {
                foreach (MetaDataRepository.MetaObjectID identity in MetaDataRepository.ValueTypePath.GetValueTypePathFromString(ValueTypePath))
                {
                    if (!string.IsNullOrEmpty(path))
                        path += "_";
                    path += MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(identity.ToString(), this).Name;
                }
            }
            return path;
        }
        /// <MetaDataID>{21C49B64-631A-4561-BF81-368429B9789E}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return null;
        }


        public AssociationEnd GetAssociationEndWithReferenceColumns()
        {
            if ((Type.RoleA as AssociationEnd).GetReferenceColumnsFor(RoleAStorageCell as StorageCell, ValueTypePath).Count > 0)
                return Type.RoleA as AssociationEnd;
            else
                return Type.RoleB as AssociationEnd;
        }


    }
}
