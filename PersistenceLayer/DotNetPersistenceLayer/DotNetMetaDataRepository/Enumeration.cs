namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{03F6215E-82ED-4CF9-A26C-34FF630F1D67}</MetaDataID>
	public class Enumeration : MetaDataRepository.Enumeration
	{
		/// <MetaDataID>{224BF7CD-5A70-4FB2-9FEB-031030C049FE}</MetaDataID>
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
		/// <MetaDataID>{5BA9E8E0-F184-45F8-B1F8-0623AB606523}</MetaDataID>
		public Enumeration(Type theWrType)
		{
			Refer=theWrType;
			DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(Refer.WrType,this);
			
		}
		/// <MetaDataID>{FE811F67-6A18-4DCD-9679-8981DA124405}</MetaDataID>
		internal Type Refer;
	}
}
