namespace Family
{
	using System;
	using OOAdvantech.MetaDataRepository;
	using OOAdvantech.PersistenceLayer;
	using OOAdvantech.Remoting;

	using OOAdvantech.Transactions;
	using System.Runtime.Serialization;
    using OOAdvantech.Collections.Generic;

	/// <MetaDataID>{CDF47ABA-BEC6-472D-8475-E96ECBE18B5D}</MetaDataID>
	[BackwardCompatibilityID("{CDF47ABA-BEC6-472D-8475-E96ECBE18B5D}")]
	[Persistent()]
	public class Person: MarshalByRefObject, IExtMarshalByRefObject,System.Runtime.Serialization.ISerializable
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{45E603D0-9072-490B-942A-EEC826CEB301}</MetaDataID>
		private OOAdvantech.Collections.Generic.Set<Dog> _Dogs;
		/// <MetaDataID>{CF25598F-B71C-4F97-81B7-F56FCA80FD07}</MetaDataID>
		[BackwardCompatibilityID("+20")]
		[Association("PersonDog",Roles.RoleA,false)]
		[AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]
		[PersistentMember("_Dogs")]
		[RoleAMultiplicityRange(0)]
		public OOAdvantech.Collections.Generic.Set<Dog> Dogs
		{
			get
			{
				return new Set<Dog>(_Dogs);
			
			}
		}
		/// <MetaDataID>{D3B3B699-DAAC-45E7-9C8E-17D1C6D83C64}</MetaDataID>
		public void ReAssignDogsObjectCollection()
		{
            _Dogs = new Set<Dog>();
		}
		/// <MetaDataID>{EC6594FD-608A-4A20-A0DC-70831C4F1CFA}</MetaDataID>
		public void AddDog(Dog dog)
		{
			using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
			{
				_Dogs.Add(dog);
				objStateTransition.Consistent=true;
			}
		
		}
		/// <MetaDataID>{E6C8E5A1-68D3-4D6E-A7C5-0E3AFEFD160F}</MetaDataID>
		public void RemoveDog(Dog dog)
		{
			using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
			{
				_Dogs.Remove(dog);
				objStateTransition.Consistent=true;
			}
		
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{2F96C4F9-C834-4051-AE62-A575E76F795E}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<Person> _Childrens = new Set<Person>();
		/// <MetaDataID>{3BFC1193-A355-4923-AAC2-06B2B1F44215}</MetaDataID>
		[BackwardCompatibilityID("+6")]
        [Association("PersonChildren", Roles.RoleA, false)]
		[AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]
		[PersistentMember("_Childrens")]
		[RoleAMultiplicityRange(0)]
		public Set<Person> Childrens
		{
			get
			{
                return new Set<Person>(_Childrens);
			}
		}

		/// <MetaDataID>{6002652D-E35B-4447-86EB-EF0635A061E3}</MetaDataID>
		public bool HasChild(Person child)
		{
			return _Childrens.Contains(child);
		}

		/// <MetaDataID>{559704E2-D2EE-447B-8302-CB4375B6F34B}</MetaDataID>
		public void AddChild(Person child)
		{
//			if(child==null)
//				return;
//			if(!_Childrens.Contains(child))
			{
				using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
				{
					_Childrens.Add(child);
					objStateTransition.Consistent=true;;
				}
			}
		}

		/// <MetaDataID>{8D29C240-43BB-4228-8F03-D21391FFB1D2}</MetaDataID>
		public void RemoveChild(Person child)
		{
//			if(child==null)
//				return;
//			if(_Childrens.Contains(child))
			{
				using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
				{
					_Childrens.Remove(child);
					objStateTransition.Consistent=true;;
				}
			}
		}

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{AC0699B1-7196-4279-A27A-228F5F626885}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<Person> _Parents = new Set<Person>();
		/// <MetaDataID>{D33B1B60-C1B6-47E7-8C1E-564EB06D0749}</MetaDataID>
		[BackwardCompatibilityID("+5")]
		[Association("PersonChildren",Roles.RoleB,false)]
		[AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]
		[PersistentMember("_Parents")]
		[RoleBMultiplicityRange(0,2)]
        public Set<Person> Parents
		{
			get
			{
                return new Set<Person>(_Parents);
			}
		}

		/// <MetaDataID>{995859B0-5C24-4A53-A34F-CA69E0CD20EC}</MetaDataID>
		public void AddParent(Person parent)
		{
//			if(parent==null)
//				return;
//			if(!_Parents.Contains(parent))
			{
				using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
				{
					_Parents.Add(parent);
					objStateTransition.Consistent=true;
				}
			}
		}

		/// <MetaDataID>{C74B80F7-D319-46E8-BD43-FED951CF7559}</MetaDataID>
		public void RemoveParent(Person parent)
		{
//			if(parent==null)
//				return;
//			if(_Parents.Contains(parent))
			{
				using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
				{
					_Parents.Remove(parent);
					objStateTransition.Consistent=true;;
				}
			}
		}

		


		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{8F89067F-DAD1-4EA0-9F5E-3ED454126D1B}</MetaDataID>
		private string _Name;
		/// <MetaDataID>{3B4A7FC1-B0BB-4E10-A8C3-40E30CBA552A}</MetaDataID>
		[BackwardCompatibilityID("+2")]
		[PersistentMember(255,"_Name")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if(_Name!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Name=value;
						objStateTransition.Consistent=true;;
					}
				}
			}
		}
		/// <MetaDataID>{E42B0B04-372C-4B62-8FB6-86DE7820B6B7}</MetaDataID>
		 public Person()
		{
		
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{44FB1745-1807-4CFB-8C2B-7EF186ABF2BF}</MetaDataID>
		private Address _Address;

		/// <MetaDataID>{8F592F50-3A70-4EEF-862E-089B60EF4E92}</MetaDataID>
		[BackwardCompatibilityID("+4")]
		[Association("LiveAt",Roles.RoleA,false)]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.ReferentialIntegrity)]
		[PersistentMember("_Address")]
		[RoleAMultiplicityRange(1,1)]
		[RoleBMultiplicityRange(0)]
		public virtual Address Address
		{
			get
			{
				return _Address;
			}
			set
			{
				if(_Address!=value)
				{
					using( ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Address=value;
						objStateTransition.Consistent=true;;
					}
				}
			}
		}

		/*
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{B413CFBF-77D5-4F89-801F-814E71DE3954}</MetaDataID>
		private string _Name;

		
		/// <MetaDataID>{52B5DE83-E81E-496A-84AB-8AAEEB1BE8F8}</MetaDataID>
		[PersistentMember("_Name")]
		[BackwardCompatibilityID("+2")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if(_Name!=value)
				{
					using(ObjectStateTransition objStateTransition=new ObjectStateTransition(this))
					{
						_Name=value;
						objStateTransition.Consistent=true;;
					}
				}
			}
		}*/



		/// <MetaDataID>{174A5EF1-81A2-441A-8FFA-3233E7211E99}</MetaDataID>

		[BackwardCompatibilityID("+1")]
		[PersistentMember()]
		public long Age;

		/// <MetaDataID>{F93573B4-CB56-4C3E-9D75-A188CBC40F16}</MetaDataID>
		public void AddAdrress()
		{
			//Pers
		
		}
		/// <MetaDataID>{3B161158-FA0F-4C0E-9931-1EA026B9F32C}</MetaDataID>
		 public Person(string name)
		{
			_Name=name;
		
		}

	
		/// <MetaDataID>{5F979B54-EF99-4C27-98F2-D26F65C3A17D}</MetaDataID>
		public void AdvanceAge()
		{
			return;
			using (OOAdvantech.Transactions.ObjectStateTransition StateTransition=new OOAdvantech.Transactions.ObjectStateTransition(this))
			{
				Age++;
				StateTransition.Consistent=true;;
			}
			int count =23;
			while(count<21)
			{
				count++;
				Person mPerson=OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(Person)) as Person;
			}



		}



		/// <MetaDataID>{CF6851AB-A10A-462D-B0CD-32A6D6075C57}</MetaDataID>
		 private Person(SerializationInfo info, StreamingContext context)
		{
			_Name=(string)info.GetString("_Name");
			Age=(int)info.GetInt64("Age");
			//_Address=(Address)info.GetValue("_Address",typeof(Family.Address));
			


			int ere=0;
		}
	
		/// <MetaDataID>{EBE8C846-EBF2-4B6D-8A12-712715C6B587}</MetaDataID>
		public OOAdvantech.ObjectStateManagerLink Properties;
		#region ISerializable Members

		/// <MetaDataID>{3D601C98-4962-4442-AB73-46D35525CB58}</MetaDataID>
		public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			System.Byte sss;
			
			info.AddValue("_Name",_Name);
			info.AddValue("Age",Age);
			//info.AddValue("_Address",_Address);
			// TODO:  Add Person.GetObjectData implementation
		}

		#endregion
	}
}
