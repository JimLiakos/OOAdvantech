namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{B363B99A-8B38-4F3F-A4D4-E8660E2B5E14}</MetaDataID>
	/// <summary>A method is the implementation of an operation. It specifies the algorithm or procedure that effects the results of an operation.
	/// In the metamodel, a Method is a declaration of a named piece of behavior in a Class and realizes one (directly) or a set (indirectly) of Operations of the Classifier. </summary>
	[BackwardCompatibilityID("{B363B99A-8B38-4F3F-A4D4-E8660E2B5E14}")]
	[Persistent()]
	public class Method : BehavioralFeature
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_OverrideKind))
            {
                if (value == null)
                    _OverrideKind = default(OOAdvantech.MetaDataRepository.OverrideKind);
                else
                    _OverrideKind = (OOAdvantech.MetaDataRepository.OverrideKind)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Specification))
            {
                if (value == null)
                    _Specification = default(OOAdvantech.MetaDataRepository.Operation);
                else
                    _Specification = (OOAdvantech.MetaDataRepository.Operation)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_OverrideKind))
                return _OverrideKind;

            if (member.Name == nameof(_Specification))
                return _Specification;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        protected OverrideKind _OverrideKind;
        /// <MetaDataID>{8ff7f339-3ab7-4c45-a057-6223cbec5ec5}</MetaDataID>
        [PersistentMember("_OverrideKind")]
        [ BackwardCompatibilityID("+2")]
        public virtual OverrideKind OverrideKind
        {
            get
            {
                return _OverrideKind;
            }
            set
            {
                _OverrideKind = value;
            }
        }


       
        
    
		//TODO να γραφτεί sychronize method
        /// <MetaDataID>{05c5c831-81e5-441b-bb8b-6bfab6ecbe98}</MetaDataID>
		public Method()
		{
		}

        /// <MetaDataID>{1d1e453a-e623-4386-a3c9-816b210220f4}</MetaDataID>
		public Method(Operation operation)
		{
			_Specification=operation;
            operation.AddOperationImplementation(this);
		}
	
		/// <MetaDataID>{8A5796BC-012E-463C-AF91-4B823A839E68}</MetaDataID>
		public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
		{
			return new System.Collections.Generic.List<object>(); 
		}

        public override void Synchronize(MetaObject originMetaObject)
        {
            _OverrideKind = (originMetaObject as Method).OverrideKind;
            base.Synchronize(originMetaObject);
        }

		/// <MetaDataID>{2033F988-B116-4A21-8CE3-718A4D8EA098}</MetaDataID>
		/// <exclude>Excluded</exclude>
		protected Operation _Specification;
	
		/// <summary>Designates an Operation that the Method implements. The Operation must be owned by the Classifier that owns the Method or be inherited by it. The signatures of the Operation and Method must match. </summary>
		/// <MetaDataID>{3B8D64EF-5B2E-4895-9D59-268446B3EAEE}</MetaDataID>
		public virtual Operation Specification
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Specification;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
	}

    /// <MetaDataID>{5bd7614f-0e7d-47cf-ab68-83d23ca4d93f}</MetaDataID>
    public enum OverrideKind
    {
        None = 0,
        Abstract = 1,
        Virtual = 2,
        Override = 4,
        New = 8,
        Sealed = 16,
    }
}
