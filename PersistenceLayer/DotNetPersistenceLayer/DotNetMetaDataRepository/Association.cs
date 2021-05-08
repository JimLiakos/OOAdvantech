namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{1B6A6274-09FB-4353-A9E7-C33E30E02C50}</MetaDataID>
	public class Association : MetaDataRepository.Association
	{

	


		//internal bool Persistent=false ;

		/// <MetaDataID>{399ECD48-53FC-4494-BD70-444C5F7257F6}</MetaDataID>
		private System.Collections.ArrayList ExtensionMetaObjects;
		/// <MetaDataID>{43A38E06-A672-4C75-8A0B-FC6FC73802C1}</MetaDataID>
		public void AddExtensionMetaObject(object Value)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{

				if(ExtensionMetaObjects==null)
					ExtensionMetaObjects=new System.Collections.ArrayList();
				ExtensionMetaObjects.Add(Value);
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}

		/// <MetaDataID>{A38A961D-60E2-4FF5-B06C-4A6606046AEF}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			ReaderWriterLock.AcquireReaderLock(10000);
			try
			{
				if(ExtensionMetaObjects==null)
					return new OOAdvantech.Collections.ArrayList();
				return new OOAdvantech.Collections.ArrayList(ExtensionMetaObjects);
			}
			finally
			{
				ReaderWriterLock.ReleaseReaderLock(); 
			}
		}


		internal void SetIdentity(string identity)
		{
			if(identity[0]=='+')
				_Identity=new MetaDataRepository.MetaObjectID(RoleA.Specification.Identity.ToString()+"."+identity.Substring(1)+"."+RoleB.Specification.Identity.ToString());
			else
				_Identity=new MetaDataRepository.MetaObjectID(identity);
		}

		/// <MetaDataID>{15410F1B-CE63-4EAF-B8A1-8CAC62CD4137}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{

			get
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				MetaDataRepository.MetaObjectID oldIdentity=_Identity;
				try
				{
					if(_Identity==null)
					{
						
						
						_Identity=new MetaDataRepository.MetaObjectID("A:"+RoleA.Specification.Identity.ToString()+"."+Name+"."+RoleB.Specification.Identity.ToString());
					}
					return _Identity;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				
				}
			}
		}
		/// <MetaDataID>{9B0CA6E1-D91B-4F9A-8877-722A2D2F73BF}</MetaDataID>
		public Association(MetaDataRepository.AssociationAttribute associationAttribute,AssociationEnd roleA,AssociationEnd roleB)
		{
			Name=associationAttribute.AssociationName;
			if(associationAttribute.Identity!=null&&associationAttribute.Identity.Trim().Length>0)
			{
				if(associationAttribute.Identity[0]=='+')
					_Identity=new MetaDataRepository.MetaObjectID(RoleA.Specification.Identity.ToString()+"."+associationAttribute.Identity.Substring(1)+"."+RoleB.Specification.Identity.ToString());
				else
					_Identity=new MetaDataRepository.MetaObjectID(associationAttribute.Identity);
			}
			SetRoleAAssociationEnd(roleA);
			SetRoleBAssociationEnd(roleB);
			roleA.SetAssociation(this);
			roleB.SetAssociation(this);

		}
	
		/// <MetaDataID>{5A681DC6-6B18-42AE-9B62-C3A7663430FC}</MetaDataID>
		void SetRoleBAssociationEnd(AssociationEnd theAssociationEnd)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(RoleB==null)
					_Connections.Add(theAssociationEnd);
				else
					if(RoleB!=theAssociationEnd)
						throw new System.Exception("You can not change AssociationEnd");
				theAssociationEnd.IsRoleA=false;
				if(((AssociationEnd)theAssociationEnd).WrMemberInfo!=null)
				{
					object[] Attributes =((AssociationEnd)theAssociationEnd).WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass),false);
					if(Attributes.Length>0)
					{
						System.Type AssociationType=((MetaDataRepository.AssociationClass)Attributes[0]).AssocciationClass;

						if(AssociationType.IsClass||AssociationType.IsInterface)
						{
							MetaDataRepository.Classifier AssociationClass=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(((MetaDataRepository.AssociationClass)Attributes[0]).AssocciationClass) as MetaDataRepository.Classifier;// Error prone εάν το type δεν είναι class 
							if(AssociationClass==null)
							{
								if(AssociationType.IsClass)
                                    AssociationClass=new Class(new Type(AssociationType));
								if(AssociationType.IsInterface)
									AssociationClass=new Interface(new Type(AssociationType));

							}
							_LinkClass=AssociationClass;
						}
					}
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
		/// <MetaDataID>{AC450E1B-9AFB-4629-BE6B-8BF9A539EA02}</MetaDataID>
		void SetRoleAAssociationEnd(MetaDataRepository.AssociationEnd theAssociationEnd)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(RoleA==null)
					_Connections.Add(theAssociationEnd);
				else
					if(RoleA!=theAssociationEnd)
						throw new System.Exception("You can not change AssociationEnd");
				theAssociationEnd.IsRoleA=true;
				if(((AssociationEnd)theAssociationEnd).WrMemberInfo!=null)
				{

					object[] Attributes =((AssociationEnd)theAssociationEnd).WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass),false);
					if(Attributes.Length>0)
					{
						System.Type AssociationType=((MetaDataRepository.AssociationClass)Attributes[0]).AssocciationClass;

						if(AssociationType.IsClass||AssociationType.IsInterface)
						{
							MetaDataRepository.Classifier AssociationClass=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(((MetaDataRepository.AssociationClass)Attributes[0]).AssocciationClass ) as MetaDataRepository.Classifier;// Error prone εάν το type δεν είναι class 
							if(AssociationClass==null)
							{
								if(AssociationType.IsClass)
									AssociationClass=new Class(new Type(AssociationType));
								if(AssociationType.IsInterface)
									AssociationClass=new Interface(new Type(AssociationType));

							}
							_LinkClass=AssociationClass;
							//AssociationClass.LinkAssociation=this;
						}
					}
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
	}
}
