namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{E8BD88C8-01FA-4B77-B767-BDB9DBF52170}</MetaDataID>
	public class Namespace : MetaDataRepository.Namespace
	{
		/// <MetaDataID>{AF047830-C63A-44EF-AB9D-5BAAC17ECBBF}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				MetaDataRepository.MetaObjectID oldIdentity=_Identity;
				try
				{
					if(_Identity!=null)
						return _Identity;
					_Identity=new MetaDataRepository.MetaObjectID(FullName);
					return _Identity;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
					if(oldIdentity==null)
						MetaObjectMapper.AddMetaObject(this,FullName);
				}
			}
		}
		/// <MetaDataID>{B0D3CACE-0B22-4A5C-9543-2436ADCBE101}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection OwnedElements
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					MetaDataRepository.MetaObjectCollection ReturnMetaObjects = new MetaObjectCollection();
					ReturnMetaObjects.AddCollection(_OwnedElements);
					return ReturnMetaObjects;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
		/// <MetaDataID>{000126DF-CD40-4822-96F8-45CC8F3EA08D}</MetaDataID>
		public void AddOwnedElement(MetaDataRepository.MetaObject theMetaObject)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				_OwnedElements.Add(theMetaObject);
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}

			
		}
		/// <MetaDataID>{E4F913F0-0B8E-4964-B53D-1C459A03CBAF}</MetaDataID>
		 public Namespace(string NamespaceName)
		{
			int nPos=NamespaceName.LastIndexOf('.');
			if(nPos!=-1)
			{
				Name=NamespaceName.Substring(nPos+1);
				string NewNameSpaceName=NamespaceName.Substring(0,nPos);
				Namespace NewNameSpace=(Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(NewNameSpaceName);
				if(NewNameSpace==null)
					NewNameSpace=new Namespace(NewNameSpaceName);
				_Namespace=NewNameSpace;
				NewNameSpace.AddOwnedElement(this);
			}
			else
				Name=NamespaceName;
			DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(NamespaceName,this);
			
			_OwnedElements=new MetaObjectCollection();

			 

		}
	}
}
