using OOAdvantech.MetaDataRepository;
namespace OOAdvantech.RDBMSMetaDataRepository
{
	
	/// <MetaDataID>{94525257-7EF2-48AB-B6FA-59450EA070F6}</MetaDataID>
	[BackwardCompatibilityID("{94525257-7EF2-48AB-B6FA-59450EA070F6}")]
	[Persistent()]
	[AssociationClass(typeof(OOAdvantech.RDBMSMetaDataRepository.Column),typeof(OOAdvantech.RDBMSMetaDataRepository.Attribute),"AttributeColumnMapping")]
	public class AttributeColumnMapping : MetaDataRepository.MetaObject
	{
		/// <MetaDataID>{43B1EAEB-42CA-4D38-8ACA-F0AFD38E6479}</MetaDataID>
		public RDBMSMetaDataRepository.Attribute Attribute
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return RoleB;
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
					RoleB=value;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}

		/// <MetaDataID>{A28302C8-21EF-43E0-9E7B-DCFD7A545ED1}</MetaDataID>
		public Column Column
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return RoleA;
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
					RoleA=value;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}

		/// <MetaDataID>{86C81D55-784F-4139-AE13-BCA8BDFE1072}</MetaDataID>
		[AssociationClassRole(Roles.RoleA)]
		private RDBMSMetaDataRepository.Column RoleA;
		/// <MetaDataID>{F82B4819-C13D-40D9-9B76-77FBF87DCB07}</MetaDataID>
		[AssociationClassRole(Roles.RoleB)]
		private RDBMSMetaDataRepository.Attribute RoleB;
		/// <MetaDataID>{679120F1-2687-48F5-A682-7B1BE5DF5925}</MetaDataID>
		[Association("ValidInRDBMSObjectCollection",typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell),Roles.RoleB,"{CCA28434-E0CF-4654-AC64-C23E388947B1}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
		[PersistentMember("ObjectCollection")]
		[RoleAMultiplicityRange(0)]
		[RoleBMultiplicityRange(1,1)]
		public StorageCell StorageCell;

		/// <MetaDataID>{0AAADBD0-E0B2-424B-96F3-13A7978086AF}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			return new System.Collections.ArrayList();
		
		}
	
	}
}
