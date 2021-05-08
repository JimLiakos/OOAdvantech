namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{C7E0F1CF-CA78-4AC4-9AD3-BD00C5A6F278}</MetaDataID>
	public class Structure : MetaDataRepository.Structure
	{
		/// <MetaDataID>{B119DC41-EE09-4119-8897-879215558B01}</MetaDataID>
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

		/// <MetaDataID>{216A9024-BEA6-4D07-9F7A-B6EEEAEFD324}</MetaDataID>
		public Structure(Type theWrType)
		{
			Refer=theWrType;
			Name=Refer.Name;
			Namespace mNamespace=(Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(Refer.WrType.Namespace);
			if(mNamespace==null)
				mNamespace=new Namespace(Refer.WrType.Namespace);
			mNamespace.AddOwnedElement(this);
			SetNamespace(mNamespace);
			DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(Refer.WrType,this);
			_Generalizations=new MetaDataRepository.MetaObjectCollection();
			_Features=new MetaDataRepository.MetaObjectCollection();
			_Roles=new MetaDataRepository.MetaObjectCollection();
			_Specializations=new MetaDataRepository.MetaObjectCollection();

		}
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


		/// <MetaDataID>{E8FEC636-6029-478B-889E-48EC163D2E46}</MetaDataID>
		internal Type Refer;
	}
}
