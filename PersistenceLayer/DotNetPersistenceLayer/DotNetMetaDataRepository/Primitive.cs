namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{DC9F97E5-C304-44C5-B442-F42A0BCC11AE}</MetaDataID>
	public class Primitive : MetaDataRepository.Primitive
	{
		/// <MetaDataID>{E484C0BB-7EDC-41EB-8EBC-61259D2A29DC}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				MetaDataRepository.MetaObjectID oldIdentity=_Identity;
				try
				{
					if(_Identity==null)
						_Identity=new MetaDataRepository.MetaObjectID(Refer.WrType.FullName);
					return _Identity;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
					if(oldIdentity==null)
						MetaObjectMapper.AddMetaObject(this,Refer.WrType.FullName);
				}
			}
		}
		/// <MetaDataID>{B7AA2F9B-BB26-4786-B512-E1742DF78DB3}</MetaDataID>
		 public Primitive(Type theWrType)
		{
			Refer=theWrType;
			 Name=Refer.Name;
			 DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(Refer.WrType,this);

			 Namespace mNamespace=(Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(Refer.WrType.Namespace);
			 if(mNamespace==null)
				 mNamespace=new Namespace(Refer.WrType.Namespace);
			 mNamespace.AddOwnedElement(this);
			 SetNamespace(mNamespace);
			 
			 _Generalizations=new MetaDataRepository.MetaObjectCollection();
			 _Features=new MetaDataRepository.MetaObjectCollection();
			 _Roles=new MetaDataRepository.MetaObjectCollection();
			 _Specializations=new MetaDataRepository.MetaObjectCollection();
			 
		}
		/// <MetaDataID>{B2942194-7866-4E9A-9ABD-1C74E31686C6}</MetaDataID>
		public void SetNamespace(MetaDataRepository.Namespace OwnerNamespace)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				_Namespace=OwnerNamespace;
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}


		/// <MetaDataID>{58169265-9FC3-4B2B-9EB8-62D778A0DC61}</MetaDataID>
		internal Type Refer;
	}
}
