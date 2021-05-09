namespace PersistenceLayerTestPrj
{
	/// <MetaDataID>{211CF2FF-F90A-4734-B88D-B097352A65CD}</MetaDataID>
	[OOAdvantech.MetaDataRepository.MetaObjectID("{211CF2FF-F90A-4734-B88D-B097352A65CD}")]
	[OOAdvantech.PersistenceLayer.Persistent("{211CF2FF-F90A-4734-B88D-B097352A65CD}")]
	public class Company:System.MarshalByRefObject,OOAdvantech.Remoting.IExtMarshalByRefObject
	{
		/// <MetaDataID>{FD1891C1-8BF2-4576-831B-A2E634667029}</MetaDataID>
		[OOAdvantech.MetaDataRepository.MetaObjectID("2")]
		[OOAdvantech.PersistenceLayer.PersistentField(OOAdvantech.PersistenceLayer.PersistencyFlag.LazyFetching)]
		public string Name="CosmoOte";
		
		public OOAdvantech.ExtensionProperties Properties;
		/// <MetaDataID>{7A84DF9E-B656-47FA-9483-1EF487BC8FC4}</MetaDataID>
 
		[OOAdvantech.MetaDataRepository.AssociationClass(typeof(Job))]
		[OOAdvantech.MetaDataRepository.MetaObjectID("1")]
		[OOAdvantech.PersistenceLayer.PersistentAssociation("Job",typeof(PersistenceLayerTestPrj.Employee),"Employees",OOAdvantech.PersistenceLayer.AssotiationEnd.RoleB,0,OOAdvantech.PersistenceLayer.Multiplicity.NoLimit)]
		public ObjectCollection Employees;
	
	
	}
}
