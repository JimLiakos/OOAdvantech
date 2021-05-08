namespace OOAdvantech.MetaDataRepository
{
    using Transactions;
	/// <MetaDataID>{820685EE-F385-46D7-8013-2E031C238C88}</MetaDataID>
    [BackwardCompatibilityID("{820685EE-F385-46D7-8013-2E031C238C88}")]
    [Persistent()]
    public abstract class StructuralFeature : Feature
    {
        /// <MetaDataID>{0ec9c7cb-d74a-4892-9544-3f649978f219}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_ParameterizedType))
            {
                if (value == null)
                    _ParameterizedType = default(OOAdvantech.MetaDataRepository.TemplateParameter);
                else
                    _ParameterizedType = (OOAdvantech.MetaDataRepository.TemplateParameter)value;
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

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{6f996be7-0349-4abb-93e5-3dd5dcb8feb9}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_ParameterizedType))
                return _ParameterizedType;

            if (member.Name == nameof(_Type))
                return _Type;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{EB4464DA-96E1-40F7-AB89-A0043E3C34C5}</MetaDataID>
        protected TemplateParameter _ParameterizedType;
        /// <MetaDataID>{9E5CCAFF-B9EF-4362-BF83-34BE681B4353}</MetaDataID>
        [Association("StructuralParameterizedType", Roles.RoleA, "{BB676FB0-4895-4650-95E7-114633763700}")]
        [PersistentMember("_ParameterizedType")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0)]
        public virtual TemplateParameter ParameterizedType
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _ParameterizedType;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    if (_ParameterizedType != value)
                    {

                        OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                        try
                        {

                            _ParameterizedType = value;
                        }
                        finally
                        {
                            ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                        }
                    }
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <MetaDataID>{4EB4D882-738D-4087-AE06-FF8032BDB962}</MetaDataID>
        public StructuralFeature(string name, Classifier type)
		{
			Name=name;
			_Type=type;
		}
		/// <MetaDataID>{1868B2D3-464A-4DA3-AC44-5367CA42BD61}</MetaDataID>
		public StructuralFeature ()
		{

		}

        /// <MetaDataID>{946FE9EE-D6B6-4BD2-8288-B48AB58DFB07}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Classifier _Type;
        /// <summary>Designates the classifier whose instances is the return value of the operation. Must be a Class, Interface, or DataType. The actual type may be a descendant of the declared type or (for an Interface) a Class that realizes the declared type.
        /// </summary>
        /// <MetaDataID>{6E9A2E03-167C-45AB-828F-88E39D2C079D}</MetaDataID>
        [Association("FeatureType", Roles.RoleA, "{FBB96C7F-C4EB-401C-B9A2-EF730EE21A8F}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Type")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.MetaDataRepository.Classifier Type
		{
			set
			{
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        _Type = value;
                        StateTransition.Consistent = true; ;
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
					return _Type;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}

        /// <MetaDataID>{445ffe77-83f2-411a-88e8-7dbd0a4d63f9}</MetaDataID>
        public abstract bool Persistent { get; set; }
    }
}
