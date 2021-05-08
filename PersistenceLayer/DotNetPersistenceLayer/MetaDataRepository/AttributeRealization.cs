using System;
using OOAdvantech.Transactions;

namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{3F854AFA-4903-437D-A556-206BB7D5C02E}</MetaDataID>
	[BackwardCompatibilityID("{3F854AFA-4903-437D-A556-206BB7D5C02E}")]
	[Persistent()]
	public class AttributeRealization : StructuralFeature
    {
        /// <exclude>Excluded</exclude>
        bool _Multilingual;
        /// <MetaDataID>{92eba69b-66f3-402a-891b-e034a4d460d9}</MetaDataID>
        /// <summary>Declare that the value off attribute of the objects will be Multilingual. </summary>
        [PersistentMember(nameof(_Multilingual))]
        [BackwardCompatibilityID("+4")]
        public virtual bool Multilingual
        {
            get => _Multilingual;
            set
            {
                if (_Multilingual != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Multilingual = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{2a0f380d-d8b1-4a1c-b6cd-ac146e06ca44}</MetaDataID>
        public virtual object GetValue(object _object)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{54aefb87-b003-442e-adfc-f6922679e374}</MetaDataID>
        public virtual void SetValue(object _object, object value)
        {
            throw new NotImplementedException();
        }


        /// <MetaDataID>{a78d8d98-bf07-4274-82bb-c2f0c4aa1773}</MetaDataID>
        public virtual object GetObjectStateValue(object _object)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{161d599a-f3c9-48a4-a573-509663fbf9e9}</MetaDataID>
        public virtual void SetObjectStateValue(object _object, object value)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{89bff6e2-35d2-4f8f-8e00-38b894aee482}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(TransactionalMember))
            {
                if (value == null)
                    TransactionalMember = default(bool);
                else
                    TransactionalMember = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Persistent))
            {
                if (value == null)
                    _Persistent = default(bool);
                else
                    _Persistent = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            if (member.Name == nameof(_Multilingual))
            {
                if (value == null)
                    _Multilingual = default(bool);
                else
                    _Multilingual = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Specification))
            {
                if (value == null)
                    _Specification = default(OOAdvantech.MetaDataRepository.Attribute);
                else
                    _Specification = (OOAdvantech.MetaDataRepository.Attribute)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{5bc7b8e8-81b9-4725-97c3-25deb9ea8b22}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(TransactionalMember))
                return TransactionalMember;

            if (member.Name == nameof(_Persistent))
                return _Persistent;

            if (member.Name == nameof(_Specification))
                return _Specification;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{298fb5d5-6a2c-44d9-bd98-626489226c59}</MetaDataID>
        public bool TransactionalMember; 

        /// <MetaDataID>{4ca6e5b8-7c48-49e7-be07-9e36af43675c}</MetaDataID>
        public override Classifier Type
        {
            get
            {
                return Specification.Type;
            }
            set
            {
                
            }
        }
        /// <MetaDataID>{5bcd4538-ccd2-47a0-bbd2-d979b9bdd6eb}</MetaDataID>
        public override TemplateParameter ParameterizedType
        {
            get
            {
                return base.ParameterizedType;
            }
            set
            {
                base.ParameterizedType = value;
            }
        }
        /// <MetaDataID>{304e5161-ebf5-4029-b6cf-34bcd741093c}</MetaDataID>
        protected AttributeRealization()
        {

        }
        /// <MetaDataID>{6375df74-eb1e-4f99-9794-e1bb7569784a}</MetaDataID>
        public AttributeRealization(Attribute specification)
		{
            _Specification = specification;
            specification.AddAttributeRealization(this);
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{C9230427-92A2-45E5-B74B-85A392DD42C5}</MetaDataID>
		protected bool _Persistent;
		/// <summary>Declare that the link between the objects will be persistent. </summary>
		/// <MetaDataID>{CA91E7F5-CEEA-4960-96D6-82DE08F1FDF1}</MetaDataID>
		[BackwardCompatibilityID("+3")]
		[PersistentMember("_Persistent")]
		public override bool Persistent
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Persistent;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
			set
			{
				OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
					{
						_Persistent=value;
						stateTransition.Consistent=true;
					}
								
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}
		/// <MetaDataID>{3CA0BA1D-AD7E-45B6-BACB-45ECD2244FF2}</MetaDataID>
		public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
		{
            return new System.Collections.Generic.List<object>();
		}
 
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{50AFC609-0B81-4D91-A94A-4A9EAE367E52}</MetaDataID>
		protected Attribute _Specification;
		/// <MetaDataID>{B5003333-642B-4538-80E4-C4F29C2CB91B}</MetaDataID>
		[BackwardCompatibilityID("+1")]
		[Association("AttributeRealization",typeof(OOAdvantech.MetaDataRepository.Attribute),Roles.RoleA,"{226A9AEB-287E-44F3-99B3-8978546593BD}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
		[PersistentMember("_Specification")]
		[RoleAMultiplicityRange(1,1)]
		public virtual Attribute Specification
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
			set
			{
				OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					if(_Specification==value)
						return;

                    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
					{
						_Specification=value;
						StateTransition.Consistent=true;;
					}
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}
			/// <MetaDataID>{007D2390-0BA8-43D0-8877-CC4B4AF58ECF}</MetaDataID>
			/// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
		public override void Synchronize(MetaObject OriginMetaObject)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{

                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
				{
					base.Synchronize(OriginMetaObject);

//					if(Name=="Name")
//					{
//						object mLength=GetPropertyValue(typeof(int),"Persistent","SizeOf");
//						if(mLength==null)
//						{
//							int ertert=0;
//						}
//						int wert=0;
//						
//					}
					AttributeRealization originAttributeRealization=(AttributeRealization)OriginMetaObject;
					_Persistent=originAttributeRealization.Persistent;
                    _Multilingual= originAttributeRealization.Multilingual;

                    //if(_Specification==null)
                    //{
                    //	if(originAttributeRealization.Specification!=null)
                    //	{
                    //		//Error prone τι γινεται όταν τα meta data είναι transient
                    //		_Specification=MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAttributeRealization.Specification.Identity.ToString(),this) as MetaDataRepository.Attribute;
                    //		if(_Specification==null)
                    //		{
                    //			_Specification=(Attribute)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAttributeRealization.Specification,this);
                    //			_Specification.Name=originAttributeRealization.Specification.Name;
                    //		}
                    //		_Specification.AddAttributeRealization(this);
                    //	}
                    //}

                    if (originAttributeRealization.Specification != null)
                    {
                        Attribute specification = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAttributeRealization.Specification.Identity.ToString(), this) as MetaDataRepository.Attribute;
                        if (specification == null)
                        {
                            specification = (Attribute)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAttributeRealization.Specification, this);
                            specification.Name = originAttributeRealization.Specification.Name;
                        }
                        if (_Specification != specification)
                        {
                            if (_Specification != null && _Specification.AttributeRealizations.Contains(this))
                                _Specification.RemoveAttributeRealization(this);

                            _Specification = specification;
                            _Specification.AddAttributeRealization(this);
                        }
                    }
                    else
                    {
                        if (_Specification != null && _Specification.AttributeRealizations.Contains(this))
                            _Specification.RemoveAttributeRealization(this);

                        _Specification = null;
                    }
                    stateTransition.Consistent=true;
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
	}
}
