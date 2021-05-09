namespace OOAdvantech.RDBMSMetaDataRepository
{
	using OOAdvantech.MetaDataRepository;

	/// <MetaDataID>{057E9C91-C627-4C2D-90D9-F5B3E6ED10B4}</MetaDataID>
	[AssociationClass(typeof(OOAdvantech.RDBMSMetaDataRepository.Column),typeof(OOAdvantech.RDBMSMetaDataRepository.AssociationEnd),"AssociationEndColumn")]
	public class AssociationEndColumn : MetaDataRepository.MetaObject
	{
		/// <MetaDataID>{2A16C537-C797-465B-B410-1B8F48C98CEF}</MetaDataID>
		[Association("AssociationEndObjectLinks",typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCellsLink),Roles.RoleA,"{D806B58C-4F34-4017-A3B4-51E81426DEFB}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
		[PersistentMember("ObjectLinks")]
		[RoleAMultiplicityRange(1,1)]
		[RoleBMultiplicityRange(0)]
		public StorageCellsLink ObjectLinks;
		/// <MetaDataID>{A5ABE70C-7B97-4D7E-983B-12F7F809C2CC}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			return null;
		}
		/// <MetaDataID>{B0F6FF40-DD3E-4850-9B51-4E612D5C1C6A}</MetaDataID>
		[Association("AssociationEndObjectsCell",typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell),Roles.RoleA,"{CDD5CBD6-FB97-45A8-A6EB-5CAF755691B1}")]
		[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
		[PersistentMember("ObjectsCell")]
		[RoleAMultiplicityRange(1,1)]
		[RoleBMultiplicityRange(0)]
		public StorageCell ObjectsCell;
		/// <MetaDataID>{772914E8-26B6-491E-ACD5-014D58E51E20}</MetaDataID>
		[AssociationClassRole(Roles.RoleB)]
		private AssociationEnd RoleB;
		/// <MetaDataID>{6415D1BB-83DA-4E18-BACF-C3597135900C}</MetaDataID>
		public AssociationEnd MappedAssociationEnd
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
		/// <MetaDataID>{19E05ABA-426D-498A-B11E-5076B59E1427}</MetaDataID>
		public Column MappedColumn
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
		/// <MetaDataID>{0D156AB3-E762-4EF8-ACDA-7F3DA9BC3F0D}</MetaDataID>
		[AssociationClassRole(Roles.RoleA)]
		private Column RoleA;
	}
}
