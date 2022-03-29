namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{65666A43-54A9-4036-9085-A6176BA493CE}</MetaDataID>
	/// <summary>A realization is a relationship between classes, interfaces that connects a client element with a supplier element.  A realization relationship between classes and interfaces and between components and interfaces shows that the class realizes the operations offered by the interface. </summary>
	[BackwardCompatibilityID("{65666A43-54A9-4036-9085-A6176BA493CE}")]
	[Persistent()]
	public class Realization : Relationship
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Implementor))
            {
                if (value == null)
                    _Implementor = default(OOAdvantech.MetaDataRepository.InterfaceImplementor);
                else
                    _Implementor = (OOAdvantech.MetaDataRepository.InterfaceImplementor)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Abstarction))
            {
                if (value == null)
                    _Abstarction = default(OOAdvantech.MetaDataRepository.Interface);
                else
                    _Abstarction = (OOAdvantech.MetaDataRepository.Interface)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Implementor))
                return _Implementor;

            if (member.Name == nameof(_Abstarction))
                return _Abstarction;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{6b08c726-b781-41b0-8166-3fe8f513e053}</MetaDataID>
        public override MetaObjectID Identity
        {
            get
            {
				lock (identityLock)

				{
					if (MetaObjectIDStream != null)
                        if (MetaObjectIDStream.Length > 0)
                            _Identity = new MetaObjectID(MetaObjectIDStream);
                    if (_Identity == null)
                        _Identity = new MetaObjectID((Implementor as MetaObject).Identity.ToString() + "." + Abstarction.Identity.ToString());
                    return _Identity;
                }
            }
        }

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{79AB6873-F752-412A-B9CC-0A724E6CE3A7}</MetaDataID>
		protected  InterfaceImplementor _Implementor;

		//		[BackwardCompatibilityID("+2")]
		//		[Association("ImplementorPart",typeof(OOAdvantech.MetaDataRepository.Class),Roles.RoleA)]
		//		[AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.ReferentialIntegrity)]
		//		[PersistentMember("_Implementor")]
		//		[RoleAMultiplicityRange(1,1)]

		/// <MetaDataID>{AB209CC4-025E-4893-A905-A9D39446A8EB}</MetaDataID>
		[BackwardCompatibilityID("+4")]
        [Association("ImplementorPart", typeof(MetaDataRepository.InterfaceImplementor), Roles.RoleA, "{B7175039-FF22-434D-93BA-94065C932934}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.ReferentialIntegrity)]
		[PersistentMember("_Implementor")]
		[RoleAMultiplicityRange(1,1)]
		public InterfaceImplementor Implementor
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Implementor;
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
						_Implementor=value;
						stateTransition.Consistent=true;
					}
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}
		/// <MetaDataID>{60C7B0F2-2754-4C66-99A7-7CE1D932654E}</MetaDataID>
		public override void Synchronize(MetaObject OriginMetaObject)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				
				Realization originRealization=(Realization)OriginMetaObject;
				if(Abstarction==null)
				{
					Abstarction=MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originRealization.Abstarction,this) as MetaDataRepository.Interface;
                    if (Abstarction == null)// && (originRealization.Implementor as MetaDataRepository.MetaObject).ImplementationUnit == originRealization.Abstarction.ImplementationUnit && originRealization.Abstarction.ImplementationUnit != null)
					{
						Abstarction=MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originRealization.Abstarction,this) as Interface;
                        if (Abstarction != null)
                        {
                            Abstarction.ShallowSynchronize(originRealization.Abstarction);
                            Abstarction.Name = originRealization.Abstarction.Name;
                        }
					}
                    if (Abstarction != null)
					    Abstarction.AddRealization(this);
					
				}
                if (Implementor == null)
                    Implementor = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace((originRealization.Implementor as MetaObject).Identity.ToString(), this) as MetaDataRepository.Class;
                if (Abstarction != null)
				    base.Synchronize(OriginMetaObject);
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		
		}
		/// <MetaDataID>{E87AEB52-730D-4DAC-AEA5-689FDBFA0AFE}</MetaDataID>
		public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
		{
			return new System.Collections.Generic.List<object>(); 
		}
		/// <MetaDataID>{C057616A-0F69-4FCA-A208-24D00BCDDD74}</MetaDataID>
		 public Realization()
		{
		}

		/// <MetaDataID>{D2095C5B-1321-4843-8482-D168C03D9253}</MetaDataID>
		 public Realization(string name, Interface _interface, Class _class)
		{
			_Abstarction=_interface;
			 _Abstarction.AddRealization(this);
			_Implementor=_class;
			_Name=name;
		}

         /// <MetaDataID>{24f73c9f-54a7-4918-b983-f1092e500771}</MetaDataID>
        public Realization(string name, Interface _interface, Structure _struct)
        {
            _Abstarction = _interface;
            _Abstarction.AddRealization(this);
            _Implementor = _struct;
            _Name = name;
        }

		/// <MetaDataID>{2431ADC9-A96B-4A2C-8AC4-101AF6EC28CF}</MetaDataID>
		/// <exclude>Excluded</exclude>
		protected Interface _Abstarction;
		/// <MetaDataID>{793CB870-37BF-4A0C-962F-C7F943C34EE9}</MetaDataID>
		[BackwardCompatibilityID("+1")]
        [Association("AbstarctPart", typeof(OOAdvantech.MetaDataRepository.Interface), Roles.RoleA, "{B378BE2A-2470-4C56-B2FB-4CF3776BE55F}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
		[PersistentMember("_Abstarction")]
		[RoleAMultiplicityRange(1,1)]
		public virtual Interface Abstarction
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Abstarction;
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
						_Abstarction=value;
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
}
