namespace Family
{
	using System;
	using OOAdvantech.MetaDataRepository;
	using OOAdvantech.PersistenceLayer;
	using OOAdvantech.Remoting;
	using OOAdvantech.Transactions;

	/// <MetaDataID>{5F1BAA68-E984-4F7C-9E80-06EAAE7E2FC0}</MetaDataID>
	[BackwardCompatibilityID("{5F1BAA68-E984-4F7C-9E80-06EAAE7E2FC0}")]
	[Persistent()]
	public class Dog: MarshalByRefObject, IExtMarshalByRefObject
	{
        /// <MetaDataID>{67daffb9-d241-4d6f-bf3a-2205b8630bca}</MetaDataID>
		public override string ToString()
		{
			return "Class: "+GetType().FullName+"  '"+_Name+"'";
		}

		/// <MetaDataID>{CC63EED3-89FB-4D42-813E-5C269A6152ED}</MetaDataID>
		public Dog()
		{

		}
		/// <MetaDataID>{FA3001C9-1CA8-439F-8166-D42175F3FC39}</MetaDataID>
		 public Dog(string name, string race)
		{
			_Name=name;
			_Race=race;
		}

		/// <MetaDataID>{D9F42761-CCFF-4082-B3C5-C150B512484D}</MetaDataID>
		public OOAdvantech.ObjectStateManagerLink Properties;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{13C3EAF1-08FC-4FF2-BEB7-B8ED2BF123DC}</MetaDataID>
		private Person _Owner;
		/// <MetaDataID>{16CE3E24-953C-4C71-91C0-7811E6B711DB}</MetaDataID>
		[BackwardCompatibilityID("+3")]
		[Association("PersonDog",Roles.RoleB,false)]
		[AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]
		[PersistentMember("_Owner")]
		[RoleBMultiplicityRange(1,1)]
		public Person Owner
		{
			get
			{
				if(_Owner==null)
				{
					StorageInstanceRef storageInstanceRef =StorageInstanceRef.GetStorageInstanceRef(Properties);
					if(storageInstanceRef!=null)
						storageInstanceRef.LazyFetching("Owner",typeof(Dog));
				}
				return _Owner;
			}
			set
			{
				using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
				{
					_Owner=value;
					objStateTransition.Consistent=true;
				}

			}
		}

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{D9D3290C-D97E-4C3F-A58E-B2813924885F}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{2FACA6F4-37C2-46F4-90C8-3AFE436957B2}</MetaDataID>
		[BackwardCompatibilityID("+1")]
		[PersistentMember(255,"_Name")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
				{
					_Name=value;
					objStateTransition.Consistent=true;
				}
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{58BF0A66-59A9-495D-BFC8-45167F83581C}</MetaDataID>
		private string _Race;
		/// <MetaDataID>{5E27D6C4-B26F-49D3-94A6-0EAD6F1FD144}</MetaDataID>
		[BackwardCompatibilityID("+2")]
		[PersistentMember(255,"_Race")]
		public string Race
		{
			get
			{
				return _Race;
			}
			set
			{
				using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
				{
					_Race=value;
					objStateTransition.Consistent=true;
				}
			}
		}
	}
}
