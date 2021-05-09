namespace OOAdvantech.RDBMSMetaDataRepository
{
    using OOAdvantech.MetaDataRepository;
    

    /// <MetaDataID>{7e020c7e-580a-453b-ae6b-0d742029f7e1}</MetaDataID>
    internal class SortColumns : System.Collections.IComparer
    {
        /// <MetaDataID>{9a3b5121-6d93-4bdb-ab91-7862eb98d68d}</MetaDataID>
        public int Compare(object x, object y)
        {
            if (!(x is Column || y is Column)) throw new System.ArgumentException("The objects to compare must be of type '" + typeof(Column).FullName + "'");
            return ((Column)x).Name.CompareTo(((Column)y).Name);
        }
    }

    /// <MetaDataID>{B7614333-1737-4A86-96E8-60AABAD0A59F}</MetaDataID>
    [BackwardCompatibilityID("{B7614333-1737-4A86-96E8-60AABAD0A59F}")]
    [Persistent()]
    public class Column : MetaDataRepository.MetaObject
    {

        /// <exclude>Excluded</exclude>
        string _MappedAssociationEndRealizationIdentity;

        /// <MetaDataID>{74bf5a4b-ade1-4858-bcb4-473951e2765c}</MetaDataID>
        [PersistentMember(nameof(_MappedAssociationEndRealizationIdentity))]
        [BackwardCompatibilityID("+30")]
        public string MappedAssociationEndRealizationIdentity
        {
            get => _MappedAssociationEndRealizationIdentity;
            set
            {

                if (_MappedAssociationEndRealizationIdentity != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _MappedAssociationEndRealizationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _MappedAttributeRealizationIdentity;

        /// <MetaDataID>{7ad68cf8-cf7c-4b62-b160-fae7f851d26c}</MetaDataID>
        [PersistentMember(nameof(_MappedAttributeRealizationIdentity))]
        [BackwardCompatibilityID("+29")]
        public string MappedAttributeRealizationIdentity
        {
            get => _MappedAttributeRealizationIdentity;
            set
            {

                if (_MappedAttributeRealizationIdentity != value)
                {
                    using (Transactions. ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _MappedAttributeRealizationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{41eddcb4-a850-4ed2-b3b8-47136ce82a4e}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_IndexerAssociationEnd))
            {
                if (value == null)
                    _IndexerAssociationEnd = default(OOAdvantech.Member<OOAdvantech.RDBMSMetaDataRepository.AssociationEnd>);
                else
                    _IndexerAssociationEnd = (OOAdvantech.Member<OOAdvantech.RDBMSMetaDataRepository.AssociationEnd>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ReferenceColumns))
            {
                if (value == null)
                    _ReferenceColumns = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>);
                else
                    _ReferenceColumns = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Column>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_AllowNulls))
            {
                if (value == null)
                    _AllowNulls = default(bool);
                else
                    _AllowNulls = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_IsIdentity))
            {
                if (value == null)
                    _IsIdentity = default(bool);
                else
                    _IsIdentity = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_IdentityIncrement))
            {
                if (value == null)
                    _IdentityIncrement = default(int);
                else
                    _IdentityIncrement = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_DefaultValue))
            {
                if (value == null)
                    _DefaultValue = default(string);
                else
                    _DefaultValue = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_CreatorIdentity))
            {
                if (value == null)
                    _CreatorIdentity = default(string);
                else
                    _CreatorIdentity = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Length))
            {
                if (value == null)
                    _Length = default(int);
                else
                    _Length = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Type))
            {
                if (value == null)
                    _Type = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _Type = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RealColumn))
            {
                if (value == null)
                    _RealColumn = default(OOAdvantech.RDBMSMetaDataRepository.Column);
                else
                    _RealColumn = (OOAdvantech.RDBMSMetaDataRepository.Column)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_MappedAssociationEnd))
            {
                if (value == null)
                    _MappedAssociationEnd = default(OOAdvantech.Member<OOAdvantech.RDBMSMetaDataRepository.AssociationEnd>);
                else
                    _MappedAssociationEnd = (OOAdvantech.Member<OOAdvantech.RDBMSMetaDataRepository.AssociationEnd>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_MappedAttribute))
            {
                if (value == null)
                    _MappedAttribute = default(OOAdvantech.Member<OOAdvantech.RDBMSMetaDataRepository.Attribute>);
                else
                    _MappedAttribute = (OOAdvantech.Member<OOAdvantech.RDBMSMetaDataRepository.Attribute>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_DataBaseColumnName))
            {
                if (value == null)
                    _DataBaseColumnName = default(string);
                else
                    _DataBaseColumnName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{ce90ef35-a7b0-4e51-84df-78c83ca87adf}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_IndexerAssociationEnd))
                return _IndexerAssociationEnd;

            if (member.Name == nameof(_ReferenceColumns))
                return _ReferenceColumns;

            if (member.Name == nameof(_AllowNulls))
                return _AllowNulls;

            if (member.Name == nameof(_IsIdentity))
                return _IsIdentity;

            if (member.Name == nameof(_IdentityIncrement))
                return _IdentityIncrement;

            if (member.Name == nameof(_DefaultValue))
                return _DefaultValue;

            if (member.Name == nameof(_CreatorIdentity))
                return _CreatorIdentity;

            if (member.Name == nameof(_Length))
                return _Length;

            if (member.Name == nameof(_Type))
                return _Type;

            if (member.Name == nameof(_RealColumn))
                return _RealColumn;

            if (member.Name == nameof(_MappedAssociationEnd))
                return _MappedAssociationEnd;

            if (member.Name == nameof(_MappedAttribute))
                return _MappedAttribute;

            if (member.Name == nameof(_DataBaseColumnName))
                return _DataBaseColumnName;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        Member<AssociationEnd> _IndexerAssociationEnd = new Member<AssociationEnd>();

        [Association("ColumnForIndex", Roles.RoleB, "750984d2-8340-486f-b377-131c2f9a7128")]
        [MetaDataRepository.PersistentMember("_IndexerAssociationEnd")]
        public AssociationEnd IndexerAssociationEnd
        {
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_IndexerAssociationEnd.Value != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _IndexerAssociationEnd.Value = value;
                            stateTransition.Consistent = true;
                        }
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
                    if (_IndexerAssociationEnd.Value == null && _RealColumn != null)
                        return _RealColumn.IndexerAssociationEnd;
                    return _IndexerAssociationEnd.Value;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3ADBD34B-332D-4888-91D8-E63C22BFB444}</MetaDataID>
        private Collections.Generic.Set<Column> _ReferenceColumns = new OOAdvantech.Collections.Generic.Set<Column>();
        /// <MetaDataID>{66CB8AB9-41A3-4651-B53D-6CF12F5FF2D6}</MetaDataID>
        [Association("AssignedColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleB, "{DA6D3B16-3815-46CF-83D6-3D0937DD227C}")]
        [PersistentMember("_ReferenceColumns")]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<Column> ReferenceColumns
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<Column>(_ReferenceColumns, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }
        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{2A220294-83F7-4843-9F5E-3C3F482EA655}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_Identity != null)
                        return _Identity;

                    //if (!string.IsNullOrEmpty(MetaObjectIDStream))
                    //{
                    //    _Identity = new MetaObjectID(MetaObjectIDStream);
                    //    return _Identity;
                    //}
                    MetaDataRepository.MetaObjectID identity = null;
                    if (_DataBaseColumnName != null && DataBaseColumnName.Trim().Length > 0)
                    {
                        if (Namespace != null)
                            identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + _DataBaseColumnName);
                        else
                            identity = new MetaDataRepository.MetaObjectID(_DataBaseColumnName);
                        identity = new MetaObjectID(identity.ToString().ToLower().Trim());

                    }
                    else
                    {
                        if (Namespace != null)
                            identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + Name);
                        else
                            identity = new MetaDataRepository.MetaObjectID(Name);
                        identity = new MetaObjectID(identity.ToString().ToLower().Trim());

                        _Identity = identity;
                        return base.Identity;
                    }
                    if (base.Identity == null || identity.ToString() != base.Identity.ToString())
                    {
                        _Identity = null;
                        //using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        //{
                        SetIdentity(identity);
                        //    stateTransition.Consistent = true;
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
        /// <MetaDataID>{1E32A8C7-94B3-4D3A-89CD-02A15DD95F85}</MetaDataID>
        protected bool _AllowNulls = true;
        ///<summary>
        ///The AllowNulls property exposes the ability of a data type to accept NULL as a value.
        ///</summary>
        ///<remarks>
        ///If true, the column  can accept NULL as a value. 
        ///If false is not allowed.
        ///</remarks>
        /// <MetaDataID>{261C5466-D420-436C-AB9A-74970928F156}</MetaDataID>
        [BackwardCompatibilityID("+14")]
        [PersistentMember("_AllowNulls")]
        public bool AllowNulls
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _AllowNulls;
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
                    if (_AllowNulls != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _AllowNulls = value;
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

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{BB73288D-D6AF-4A4E-9B11-BAD20E94178E}</MetaDataID>
        private bool _IsIdentity = false;
        /// <MetaDataID>{445132BE-FB68-49F6-9808-877F3288C75D}</MetaDataID>
        [BackwardCompatibilityID("+15")]
        [PersistentMember("_IsIdentity")]
        public bool IsIdentity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _IsIdentity;
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
                    if (_IsIdentity != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _IsIdentity = value;
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
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{A136C3F8-EA4B-43DA-9D8F-B380741FAB08}</MetaDataID>
        private int _IdentityIncrement = 1;
        /// <MetaDataID>{BED1D86E-C884-47BB-A4BB-2D826C1B3B9A}</MetaDataID>
        [BackwardCompatibilityID("+16")]
        [PersistentMember("_IdentityIncrement")]
        public int IdentityIncrement
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _IdentityIncrement;
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
                    if (_IdentityIncrement != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _IdentityIncrement = value;
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
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{D0EBD7B9-84AA-4806-AA26-5729D39E8192}</MetaDataID>
        private string _DefaultValue;
        /// <MetaDataID>{561A6444-BAC7-4792-90D0-ED4E461FA307}</MetaDataID>
        [BackwardCompatibilityID("+25")]
        [PersistentMember("_DefaultValue")]
        public string DefaultValue
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _DefaultValue;
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

                    if (_DefaultValue != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _DefaultValue = value;
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





        /// <exclude>Excluded</exclude>
        private string _CreatorIdentity;
        /// <MetaDataID>{c296bc4a-0b12-4e5e-b792-ede95cb298f9}</MetaDataID>

        /// <MetaDataID>{7CE85F7C-B311-4cc3-BD74-6C981575B4D6}</MetaDataID>
        [BackwardCompatibilityID("+28")]
        [PersistentMember("_CreatorIdentity")]
        public string CreatorIdentity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_CreatorIdentity == null)
                        return "";
                    return _CreatorIdentity;
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

                    if (_CreatorIdentity != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _CreatorIdentity = value;
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


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8619CA3E-727A-424A-ADD1-4646B3D4FEB6}</MetaDataID>
        private int _Length = 0;
        /// <MetaDataID>{FA962E0F-C3B6-43C5-9AA9-717886723199}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [PersistentMember("_Length")]
        public int Length
        {
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_Length != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _Length = value;
                            stateTransition.Consistent = true;
                        }
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
                    return _Length;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6F07EA9A-1E71-446C-B8BC-064506CE238F}</MetaDataID>
        protected OOAdvantech.MetaDataRepository.Classifier _Type;
        /// <MetaDataID>{EF9251EB-7E63-4A62-8D4F-4E3AEA119FC0}</MetaDataID>
        [Association("ColumnType", typeof(OOAdvantech.MetaDataRepository.Classifier), Roles.RoleA, "{A8BB21A2-1E88-42FD-A977-5D163FEB4D52}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Type")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(0)]
        public Classifier Type
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    //					if(MappedAttribute!=null)
                    //						return MappedAttribute.Type;
                    return _Type;
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
                    if (_Type != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _Type = value;
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



        //		/// <MetaDataID>{C33CDCE0-37D7-4810-B766-A65C15E52D96}</MetaDataID>
        //		[BackwardCompatibilityID("+25")]
        //		[PersistentMember()]
        //		public string DefaultValue;
        /// <MetaDataID>{7E65EF43-9906-40DB-81A2-3DE3C0304231}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Column _RealColumn;
        /// <MetaDataID>{042B6267-8977-4A48-82D9-368F46D42819}</MetaDataID>
        [Association("AssignedColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.Column), Roles.RoleA, "{DA6D3B16-3815-46CF-83D6-3D0937DD227C}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_RealColumn")]
        [RoleAMultiplicityRange(0, 1)]
        public Column RealColumn
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _RealColumn;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{6C2D9CEB-783C-404B-A5B9-DD36DE7154EC}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Member<AssociationEnd> _MappedAssociationEnd = new Member<AssociationEnd>();
        /// <MetaDataID>{7D0C5765-0140-4BAF-9D12-D7CC1BAF5F4B}</MetaDataID>

        [Association("AssociationEndColumn", typeof(OOAdvantech.RDBMSMetaDataRepository.AssociationEnd), Roles.RoleB, "{732E13CB-2C8D-4E7B-8219-89B25403A7CE}")]
        [AssociationEndBehavior(PersistencyFlag.AllowTransient)]
        [PersistentMember("_MappedAssociationEnd")]
        [RoleBMultiplicityRange(0, 1)]
        public AssociationEnd MappedAssociationEnd
        {
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_MappedAssociationEnd.Value != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _MappedAssociationEnd.Value = value;
                            stateTransition.Consistent = true;
                        }
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
                    if (_MappedAssociationEnd.Value == null && _RealColumn != null)
                        return _RealColumn.MappedAssociationEnd;
                    return _MappedAssociationEnd.Value;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{23DFECA4-DDF0-494E-ACD0-C0DF5C1946C3}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Member<Attribute> _MappedAttribute = new Member<Attribute>();
        /// <MetaDataID>{E0C01F0E-5CC7-4A85-A4AC-2F8DC26104C7}</MetaDataID>
        [Association("AttributeColumnMapping", typeof(OOAdvantech.RDBMSMetaDataRepository.Attribute), Roles.RoleB, "{453C9F8A-B2A0-4DFB-916F-8D93A26F8473}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_MappedAttribute")]
        [RoleBMultiplicityRange(0, 1)]
        public Attribute MappedAttribute
        {
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_MappedAttribute.Value != value)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _MappedAttribute.Value = value;
                            stateTransition.Consistent = true;
                        }
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
                    if (_MappedAttribute.Value == null && _RealColumn != null)
                        return _RealColumn.MappedAttribute;
                    return _MappedAttribute.Value;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }



            }
        }
        /// <MetaDataID>{ff9dad29-fdf2-4412-a037-b8e73860b012}</MetaDataID>
        protected override void SetNamespace(Namespace mNamespace)
        {

            base.SetNamespace(mNamespace);
            _Identity = null;
        }

        /// <MetaDataID>{bc9fb217-6d39-485b-b23a-ea40bd5c39f6}</MetaDataID>
        public bool DataBaseColumnNameHasValue
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_DataBaseColumnName);
            }
        }
        /// <MetaDataID>{DBA32A1E-F47E-4092-9973-91F64472003B}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private string _DataBaseColumnName;
        /// <MetaDataID>{434F295F-275C-4966-8F85-359C3453AC14}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        [PersistentMember(255, "_DataBaseColumnName")]
        public string DataBaseColumnName
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (!string.IsNullOrWhiteSpace(_DataBaseColumnName))
                        return _DataBaseColumnName;
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
                    if (_DataBaseColumnName != value)
                    {
                        _Identity = null;
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, Transactions.TransactionOption.Supported))
                        {
                            _DataBaseColumnName = value;
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

        /// <MetaDataID>{A15FE914-A330-4461-9C42-7A9724AF17B3}</MetaDataID>
        public Column()
        {

        }

        /// <MetaDataID>{448b008e-3d0f-4a71-8c0a-4613b2df3e78}</MetaDataID>
        public Column(string name, MetaDataRepository.Classifier type)
        {
            _Type = type;
            _Name = name;
        }
        /// <MetaDataID>{48D9F097-AF5B-4355-9064-D0771DB6B23A}</MetaDataID>
        public Column(string name, MetaDataRepository.Classifier type, int length, bool allowNulls, bool isIdentity, int identityIncrement)
        {
            _AllowNulls = allowNulls;
            _IsIdentity = isIdentity;
            _IdentityIncrement = identityIncrement;
            _Type = type;
            _Name = name;
            _Length = length;
        }


        //public Column(Attribute attribute, int length, bool allowNulls, bool isIdentity, int identityIncrement)
        //    : this(attribute, length, allowNulls, isIdentity, identityIncrement, attribute.Identity.ToString())
        //{

        //}
        /// <MetaDataID>{FA6991B8-BF59-49F8-BCB8-2A1FF6094B36}</MetaDataID>
        public Column(Attribute attribute, int length, bool allowNulls, bool isIdentity, int identityIncrement, string creatorIDentity)
        {
            //TODO να γραφτεί test case που θα υπάρχει ένα persistent attribute σε μια class
            //και θα υπάρχει ένα new attribute persistent με το ίδιο όνομα σε μια subclass
            _CreatorIdentity = creatorIDentity;
            _AllowNulls = allowNulls;
            _IsIdentity = isIdentity;
            _IdentityIncrement = identityIncrement;
            _Type = attribute.Type;
            if (attribute.Owner is Structure)
                _Name = attribute.Owner.Name + "_" + attribute.CaseInsensitiveName;
            else
                _Name = attribute.CaseInsensitiveName;


            _MappedAttribute.Value = attribute;
            _Length = length;

        }


        /// <MetaDataID>{A90DF439-B1E5-48A9-9BAF-D719DAACBEF6}</MetaDataID>
        public Column(Column Column)
        {
            if (Column == null)
                throw new System.Exception("The _RealColumn have to not null");
            _RealColumn = Column;
            _Name = Column.Name;
            _Type = Column.Type;
        }
        /// <MetaDataID>{EF504230-0C76-4759-98FD-8FBFEB0CE665}</MetaDataID>
        public void Refresh()
        {
            if (_RealColumn != null)
            {
                _Name = _RealColumn.Name;
                _Type = _RealColumn.Type;
            }
        }
        /// <MetaDataID>{3A97047E-05AE-4724-A67D-7DCD82DC1B4A}</MetaDataID>
        public override string Name
        {
            get
            {
                return base._Name;
            }
            set
            {


                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_Name != value)
                    {
                        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _Name = value;
                            _CaseInsensitiveName = null;
                            StateTransition.Consistent = true; ;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }

     


        //		/// <MetaDataID>{996D5F07-C09F-4467-87A0-952B26AB3C9C}</MetaDataID>
        //		public int Length
        //		{
        //			get
        //			{
        //
        //				OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
        //				try
        //				{
        //					if(MappedAttribute==null)
        //						return 0;
        //					MetaDataRepository.AttributeRealization attributeRealization =((Namespace as Table).TableCreator as StorageCell).Type.GetAttributeRealization(MappedAttribute);
        //					object mLength=null;
        //					if(attributeRealization!=null)
        //						mLength=attributeRealization.GetPropertyValue(typeof(int),"Persistent","SizeOf");
        //					else
        //                        mLength= MappedAttribute.GetPropertyValue(typeof(int),"Persistent","SizeOf");
        //					if(mLength!=null)
        //						return(int)mLength;
        //					else
        //						return 0;
        //				}
        //				finally
        //				{
        //					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
        //				}
        //
        //			}
        //		}
        //		

        /// <MetaDataID>{134AEA64-F71A-4D01-975A-EEA797D21262}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();

        }

    }
}
