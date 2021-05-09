namespace PersistenceLayerTestPrj
{
	/// <MetaDataID>{21FAA608-30F0-4A82-84E7-E4AB59A063E3}</MetaDataID>
	[OOAdvantech.MetaDataRepository.MetaObjectID("{21FAA608-30F0-4A82-84E7-E4AB59A063E3}")]
	[OOAdvantech.PersistenceLayer.Persistent("{21FAA608-30F0-4A82-84E7-E4AB59A063E3}")]
	[OOAdvantech.MetaDataRepository.AssociationClass(typeof(Company),typeof(Employee),"Job")]
	public class Job:System.MarshalByRefObject,OOAdvantech.Remoting.IExtMarshalByRefObject
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{F150A7F4-887E-47DF-BE60-E64603D7387D}</MetaDataID>
		private Company _Employer;
		/// <MetaDataID>{97E093D7-8459-4583-A57E-9F2AF77FD987}</MetaDataID>
		[OOAdvantech.MetaDataRepository.AssociationClassRole(true,"_Employer")]
		public Company Employer
		{
			get
			{
				return _Employer;
			}
			set
			{
				_Employer=value;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{006D4EF4-D494-4D44-9A3D-F955A5EAD596}</MetaDataID>
		private Employee _Employee;
		/// <MetaDataID>{FFEE6304-8475-4566-A27C-0A6B29305528}</MetaDataID>
		[OOAdvantech.MetaDataRepository.AssociationClassRole(false,"_Employee")]
		public Employee Employee
		{
			get
			{
				return _Employee;
			}
			set
			{
				_Employee=value;
			}
		}
		/// <MetaDataID>{60015E2F-AA45-4A71-994B-0C55B3D3880E}</MetaDataID>
		[OOAdvantech.MetaDataRepository.MetaObjectID("2")]
		[OOAdvantech.PersistenceLayer.PersistentField(OOAdvantech.PersistenceLayer.PersistencyFlag.LazyFetching)]
		public string Name="Σκαφτιάς";

		public OOAdvantech.ExtensionProperties Properties;	
	}
}
