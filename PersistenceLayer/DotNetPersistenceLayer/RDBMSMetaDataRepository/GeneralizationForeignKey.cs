namespace OOAdvantech.RDBMSMetaDataRepository
{
	using MetaDataRepository;
	/// <MetaDataID>{A13D7E7D-8E3E-486B-B36D-0A29F89ED45C}</MetaDataID>
	[BackwardCompatibilityID("{A13D7E7D-8E3E-486B-B36D-0A29F89ED45C}")]
	[Persistent()]
	[AssociationClass(typeof(OOAdvantech.RDBMSMetaDataRepository.Key),typeof(OOAdvantech.RDBMSMetaDataRepository.Generalization),"GeneralizationForeignKey")]
	public class GeneralizationForeignKey : OOAdvantech.MetaDataRepository.MetaObject
	{
		/// <MetaDataID>{8129917E-04F6-4DA5-B2E0-AF19F8BBC590}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			return null;
		}
		/*Error Prone Thread safity*/
		/// <MetaDataID>{773D9175-117B-465B-A447-17852DE9F240}</MetaDataID>
		[Association("GeneralizationForeignKeyObjectsCell",typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell),Roles.RoleA,"{FC271900-7CB3-4852-A1AA-7BED8DD4BD24}")]
		[PersistentMember("ObjectsCells")]
		[RoleAMultiplicityRange(1,1)]
		[RoleBMultiplicityRange(0)]
		public StorageCell ObjectsCells;
	
		/// <MetaDataID>{64D84BCF-4608-4AF8-80B8-BD5BAD533CD6}</MetaDataID>
		[AssociationClassRole(Roles.RoleB)]
		private Generalization RoleB;
		/// <MetaDataID>{5462F916-C5B3-419B-A1A9-C50BFE740F81}</MetaDataID>
		[AssociationClassRole(Roles.RoleA)]
		private Key RoleA;
	}
}
