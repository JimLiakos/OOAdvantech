namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{F4464283-7D7F-44DC-97EB-0B3FAE62C46E}</MetaDataID>
	public class AssociationEndRealization : OOAdvantech.MetaDataRepository.AssociationEndRealization
	{

		bool InErrorCheck=false;
		public override bool ErrorCheck(ref string errors)
		{
			if(InErrorCheck)
				return false;
			try
			{
				string Iden=Identity.ToString();
				InErrorCheck=true;
				bool hasError=base.ErrorCheck(ref errors); 

//				if(WrMemberInfo!=null&&Multiplicity.IsMany&&!WrMemberInfo.DeclaringType.IsSubclassOf(PersistenceLayer.ObjectContainer)&&this.Persistent)
//				{
//					"
//
//				}


				return hasError;
			}
			finally
			{
				InErrorCheck=false;
			}
		}




		/// <MetaDataID>{BCBCE805-3F65-438F-8A19-A30B5F2B88E8}</MetaDataID>
		private bool FieldMemberLoaded=false;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{224E415C-0C83-42C4-B2B4-0516C50CA707}</MetaDataID>
		private System.Reflection.FieldInfo _FieldMember;
		/// <MetaDataID>{8EAE04CF-082E-40A0-A9F2-1766C272E303}</MetaDataID>
		public System.Reflection.FieldInfo FieldMember
		{
			get
			{
				return _FieldMember;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{5D5BE2D2-8D7B-4A7D-AAC7-938057BBE98A}</MetaDataID>
		private System.Reflection.PropertyInfo _PropertyMember;
		/// <MetaDataID>{F92F0D37-60CC-4E37-BC96-F6E1E243335F}</MetaDataID>
		public System.Reflection.PropertyInfo PropertyMember
		{
			get
			{
				return _PropertyMember;
			}
		}
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{
				

				MetaDataRepository.MetaObjectID oldIdentity=_Identity;
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{

					if(_Identity!=null)
						return _Identity;

					string identityAsString=null;
					object[] Attributes=_PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID),false);
					bool plus=false;
					if(Attributes.Length>0)
					{
						
						identityAsString=((MetaDataRepository.BackwardCompatibilityID)Attributes[0]).ToString();
						if(identityAsString.Length>0)
							if(identityAsString[0]=='+')
							{
								plus=true;
								identityAsString=identityAsString.Substring(1);
							}
					}
					if(identityAsString==null||identityAsString.Length==0)
					{
						_Identity=new MetaDataRepository.MetaObjectID(Owner.Identity.ToString()+"."+Name);//+"."+Specification.Identity);//+"."+otherAssociationEndID);//Error prone 
						return _Identity;
					}
					else
					{
						if(plus)
							_Identity=new MetaDataRepository.MetaObjectID(Owner.Identity.ToString()+"."+identityAsString);
						else
							_Identity=new MetaDataRepository.MetaObjectID(identityAsString);

					}
					return _Identity;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
					if(oldIdentity==null)
					{
						MetaObjectMapper.AddMetaObject(this,_PropertyMember.DeclaringType.FullName+"."+_PropertyMember.Name);
					}
				}
			}
		}


		/// <MetaDataID>{DDA462D7-9508-4BFE-B423-9CCAFACFE21B}</MetaDataID>
		public AssociationEndRealization(System.Reflection.PropertyInfo property, AssociationEnd associationEnd,MetaDataRepository.Classifier owner)
		{
			_Owner=owner;
			Name=associationEnd.Name;
			_Specification=associationEnd;
			associationEnd.AddAssociationEndRealization(this);

			_PropertyMember=property;
			

			 object[] customAttributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.AssociationEndBehavior),false);
			if(customAttributes.Length!=0) 
			{
				MetaDataRepository.PersistencyFlag persistencyFlag=(customAttributes[0] as MetaDataRepository.AssociationEndBehavior).PersistencyFlag;
				_HasBehavioralSettings=true;
				if((uint)(persistencyFlag&MetaDataRepository.PersistencyFlag.ReferentialIntegrity)!=0)
					_ReferentialIntegrity=true;
				else
					_ReferentialIntegrity=false;
					
				if((uint)(persistencyFlag&MetaDataRepository.PersistencyFlag.CascadeDelete)!=0)
					_CascadeDelete=true;
				else
					_CascadeDelete=false;
			
				if((uint)(persistencyFlag&MetaDataRepository.PersistencyFlag.OnConstruction)!=0)
					_LazyFetching=false;
				else
					_LazyFetching=true;
			}
			else
				_HasBehavioralSettings=false;

			 customAttributes =_PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember),false);
			 if(customAttributes.Length>0)
			 {
				 _FieldMember=(customAttributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(_PropertyMember as System.Reflection.PropertyInfo);
				 _Persistent=true;
				 
			 }

		}
	
	}
}
