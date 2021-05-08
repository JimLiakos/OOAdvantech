using System;
using System.Xml.Linq;
using OOAdvantech.Transactions;

namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{FB3940EF-0D72-4B54-A29E-ACB999EC452C}</MetaDataID>
	/// <summary>Attributes define the characteristics of instances of a class. Each instances of a class has the same attributes, but the values of the attributes may be different. </summary>
	[BackwardCompatibilityID("{FB3940EF-0D72-4B54-A29E-ACB999EC452C}")]
	[Persistent()]
	public class Attribute : StructuralFeature
    {

        /// <exclude>Excluded</exclude>
        bool _Multilingual;

        /// <MetaDataID>{63c70197-be26-4dce-9e75-17b60c4ffea7}</MetaDataID>
        /// <summary>Declare that the value off attribute of the objects will be Multilingual. </summary>
        [PersistentMember(nameof(_Multilingual))]
        [BackwardCompatibilityID("+10")]
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

        /// <MetaDataID>{d55f6550-9a43-45b4-9050-b92c1cad66c7}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_AttributeRealizations))
            {
                if (value == null)
                    _AttributeRealizations = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AttributeRealization>);
                else
                    _AttributeRealizations = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AttributeRealization>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ByValueTypes))
            {
                if (value == null)
                    ByValueTypes = default(System.Collections.Generic.List<string>);
                else
                    ByValueTypes = (System.Collections.Generic.List<string>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(InErrorCheck))
            {
                if (value == null)
                    InErrorCheck = default(bool);
                else
                    InErrorCheck = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Setter))
            {
                if (value == null)
                    _Setter = default(OOAdvantech.MetaDataRepository.Operation);
                else
                    _Setter = (OOAdvantech.MetaDataRepository.Operation)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Getter))
            {
                if (value == null)
                    _Getter = default(OOAdvantech.MetaDataRepository.Operation);
                else
                    _Getter = (OOAdvantech.MetaDataRepository.Operation)value;
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
            if (member.Name == nameof(TransactionalMember))
            {
                if (value == null)
                    TransactionalMember = default(bool);
                else
                    TransactionalMember = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{c2f1318c-7891-4677-99d3-a4bb0fee6b25}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_AttributeRealizations))
                return _AttributeRealizations;

            if (member.Name == nameof(ByValueTypes))
                return ByValueTypes;

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(_Setter))
                return _Setter;

            if (member.Name == nameof(_Getter))
                return _Getter;

            if (member.Name == nameof(_Persistent))
                return _Persistent;

            if (member.Name == nameof(TransactionalMember))
                return TransactionalMember;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{00e0c220-601d-42a7-8b41-d3d020eb6a3c}</MetaDataID>
        public virtual bool HasAttributeRealizations
        {
            get
            {
                return AttributeRealizations.Count != 0;
            }
        }

        /// <MetaDataID>{d89b3dc5-eae3-4bcd-bcaa-1af7b41c29ee}</MetaDataID>
        /// <summary>
        /// Check 
        /// </summary>
        public bool HasPersistentRealization
        {
            get
            {
                if (Persistent)
                    return true;

                foreach (MetaDataRepository.AttributeRealization attributeRealization in AttributeRealizations)
                {
                    if (attributeRealization.Persistent)
                        return true;
                }
                return false;
            }
        }
		/// <MetaDataID>{4CBAE86F-E9B4-4178-BC42-C549ABD53093}</MetaDataID>
		public void AddAttributeRealization(AttributeRealization attributeRealization)
		{

            using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this,OOAdvantech.Transactions.TransactionOption.Supported))
            {
                _AttributeRealizations.Add(attributeRealization); 
                stateTransition.Consistent = true;
            }
        
		
		}
		/// <MetaDataID>{3BF13658-88C4-4A42-AE49-42D5AE54AB96}</MetaDataID>
		public void RemoveAttributeRealization(AttributeRealization attributeRealization)
		{
            using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
            {
                _AttributeRealizations.Remove(attributeRealization);
                stateTransition.Consistent = true;
            }
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{8F62A852-1699-4D50-A4A4-9C74BCA2167C}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<AttributeRealization> _AttributeRealizations = new OOAdvantech.Collections.Generic.Set<AttributeRealization>();

        /// <MetaDataID>{1a110c72-4927-47ea-8627-441305555896}</MetaDataID>
        public virtual object GetObjectStateValue(object _object)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{0ba80dcf-902f-4480-b0da-3ebfc624a1f6}</MetaDataID>
        public virtual void SetObjectStateValue(object _object, object value)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{62e90354-9e5d-4fd1-8511-602da5cb7c96}</MetaDataID>
        public virtual object GetValue(object _object)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{403262d0-b725-4a5b-ab7d-62352fac0e52}</MetaDataID>
        public virtual void SetValue(object _object, object value)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{5AB71EE7-EF4E-484B-8923-0D39E30BC592}</MetaDataID>
        [Association("AttributeRealization",typeof(OOAdvantech.MetaDataRepository.AttributeRealization),Roles.RoleB,"{226A9AEB-287E-44F3-99B3-8978546593BD}")]
		[PersistentMember("_AttributeRealizations")]
		[RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.Set<AttributeRealization> AttributeRealizations
		{
			get
			{
                return _AttributeRealizations.ToThreadSafeSet();
			}
		}
        /// <MetaDataID>{ebe67c93-2e5f-4d87-89ec-ed49ac274aaa}</MetaDataID>
		static public System.Collections.Generic.List<string> ByValueTypes=new System.Collections.Generic.List<string>(){typeof(System.String).FullName,typeof(System.Xml.Linq.XDocument).FullName,"System.Drawing.Image"};
		/// <MetaDataID>{1C478443-0064-4946-964F-57A2D51DD71C}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private bool InErrorCheck=false;
		/// <MetaDataID>{E0F723FB-4E7B-41EA-94E1-BDBF79D8642E}</MetaDataID>
		public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
		{
			if(InErrorCheck)
				return false;
			try
			{
				InErrorCheck=true;
				bool hasError=base.ErrorCheck(ref errors); 

				if(Persistent&&(Type is Interface||(Type is Class)&&(Type as Class).Abstract)&&!ByValueTypes.Contains(Type.FullName))
				{
					errors.Add(new MetaObject.MetaDataError("The type of attribute '"+Owner.FullName+"."+ Name+"' must be not interface or abstract class",null));  
					return true;
				}
				return hasError;
			}
			finally
			{
				InErrorCheck=false;
			}
		}


		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{E8ACFD65-7F75-4EFA-81D0-5D4AB53C24B4}</MetaDataID>
		protected Operation _Setter;
		/// <summary>This is the operation through client write the attribute. </summary>
		/// <MetaDataID>{810446FA-C7D4-432E-940E-DF7701B0AB02}</MetaDataID>
		[Association("WriteAttributeAccess",typeof(OOAdvantech.MetaDataRepository.Operation),Roles.RoleA,"{BAF0B8DD-C6B7-4582-8A2D-16519F1F084E}")]
		[PersistentMember("_Setter")]
		[RoleAMultiplicityRange(0,1)]
		[RoleBMultiplicityRange(0,1)]
		public virtual Operation Setter
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Getter;
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
					if(_Setter==value)
						return;

                    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
					{
						_Setter=value;
						StateTransition.Consistent=true;;
					}
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}

			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{0A623DED-4B2E-49EC-8E97-28E58B91D896}</MetaDataID>
		protected Operation _Getter;
		/// <summary>This is the operation through client read the attribute. </summary>
		/// <MetaDataID>{B40E7B00-0D7D-4480-811F-C5B9F87B7FD9}</MetaDataID>
		[Association("ReadAttributeAccess",typeof(OOAdvantech.MetaDataRepository.Operation),Roles.RoleA,"{4BC0657F-4390-443C-AC86-78C653A83EC3}")]
		[PersistentMember("_Getter")]
		[RoleAMultiplicityRange(0,1)]
		[RoleBMultiplicityRange(0,1)]
		public virtual Operation Getter
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Getter;
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
					if(_Getter==value)
						return;

                    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
					{
						_Getter=value;
						StateTransition.Consistent=true;;
					}
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}

			}
		}



        /// <MetaDataID>{95589a8a-c146-4c90-a48c-1d2c2d88e4e4}</MetaDataID>
        protected bool _Persistent;
		/// <MetaDataID>{3FABC33D-9AC8-49DF-AF1F-4484BEB140D1}</MetaDataID>
		[BackwardCompatibilityID("+12")]
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
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        _Persistent = value;
                        stateTransition.Consistent = true;
                    }

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }



		/// <MetaDataID>{C275F314-8466-49CB-B8F8-6FDB2E91EC12}</MetaDataID>
		 public Attribute(string name, Classifier type):base(name,type)
		{

		}
		/// <MetaDataID>{BFEAF855-EDD7-49B1-848D-3E277A8C885D}</MetaDataID>
		public Attribute()
		{

		}
        /// <MetaDataID>{4cc21fb0-3db3-4259-ba70-526e6b8420d5}</MetaDataID>
        public bool TransactionalMember;


		/// <MetaDataID>{112DC302-D051-4827-A4B2-5A4169882D1E}</MetaDataID>
		/// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
		public override void Synchronize(MetaObject originMetaObject)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
							
				//PersistenceLayer.PersistencyContext AppPersistencyContext=PersistenceLayer.PersistencyContext.CurrentPersistencyContext;
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
				{
					try
					{

                        if (_Name != originMetaObject.Name)
                        {
                            _Name = originMetaObject.Name;
                            _CaseInsensitiveName = null;
                        }
                        Visibility = originMetaObject.Visibility;
                        string xml = originMetaObject.GetDynamicPropertiesAsXmlString();
                        if (string.IsNullOrEmpty(xml))
                        {
                            if (XMLDynamicProperties != null)
                                XMLDynamicProperties = null;
                        }
                        else
                        {
                            XMLDynamicProperties = XDocument.Parse(xml);
                        }
                        if (_ImplementationUnit.Value == null && originMetaObject.ImplementationUnit != null)
                        {
                            _ImplementationUnit.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originMetaObject.ImplementationUnit, this) as MetaDataRepository.Component;
                            if (_ImplementationUnit.Value == null)
                                _ImplementationUnit.Value = (Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originMetaObject.ImplementationUnit, this);
                        }

                        Attribute originAttribute = null;
                        if (originMetaObject is AttributeRealization)
                            originAttribute = (originMetaObject as AttributeRealization).Specification;
                        else
                            originAttribute =originMetaObject as Attribute;
						Persistent=originAttribute.Persistent;

                        Multilingual = originAttribute.Multilingual;

                        //						if(_Type==null)
                        //							PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties).LazyFetching("Type",typeof(MetaDataRepository.StructuralFeature));

                        Classifier originType = originAttribute.Type;
                        if (originType != null)
                        {
                            _ParameterizedType = null;
                            if (_Type == null || _Type.Identity != originAttribute.Type.Identity)
                            {
                                _Type = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAttribute.Type, this) as MetaDataRepository.Classifier;
                                if (_Type == null)
                                {
                                    //TODO να τσεκαριστή με test case αν το type θα έχει σωστό namespace και η διαδικασία συχρονισμού δέν θα προχωρίσει 

                                    _Type = (Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAttribute.Type, this);
                                    _Type.ShallowSynchronize(originAttribute.Type);
                                }
                            }
                            if(_Type!=null)
                                _Type.ShallowSynchronize(originAttribute.Type);
                        }
                        else
                            _ParameterizedType = new TemplateParameter(originAttribute.ParameterizedType.Name);

//						if(_Type!=null)
//							_Type.Synchronize(OriginAttribute.Type);
					}
					catch(System.Exception error)
					{
						throw ;
					}

					StateTransition.Consistent=true;;
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
	
		}
		/// <MetaDataID>{BE232D53-A9B6-47E2-AFCC-61D1F357B1CC}</MetaDataID>
		public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
		{
			return new System.Collections.Generic.List<object>(); 
		}





        /// <summary>
        /// Is true when the type of attribute is structure and the structure defined as persistend
        /// </summary>
        /// <MetaDataID>{c53e2e52-f323-4d65-b556-589b6b8561a7}</MetaDataID>
        public bool IsPersistentValueType
        {
            get
            {
                if ( ((Type is MetaDataRepository.Structure) && (Type as MetaDataRepository.Structure).Persistent))
                    return true;
                else
                    return false;
            }
        }


    }
}
