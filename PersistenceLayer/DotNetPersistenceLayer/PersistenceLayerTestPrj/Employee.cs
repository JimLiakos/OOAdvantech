


namespace PersistenceLayerTestPrj
{
	/// <MetaDataID>{3BC8EFC3-659A-4D84-8F1C-EC6C5FDAAE15}</MetaDataID>
	/// <summary></summary>
	[OOAdvantech.MetaDataRepository.MetaObjectID("{3BC8EFC3-659A-4D84-8F1C-EC6C5FDAAE15}")]
	[OOAdvantech.PersistenceLayer.Persistent("<ExtMetaData><RDBMSInheritanceMapping>OneTablePerClass</RDBMSInheritanceMapping></ExtMetaData>")]
	public class Employee :PersistenceLayerTestPrj.Person
	{
		/// <MetaDataID>{E39FFC7B-1146-4074-B79B-8569C257E322}</MetaDataID>
		[OOAdvantech.MetaDataRepository.MetaObjectID("1")]
		[OOAdvantech.PersistenceLayer.PersistentField()]
		public double Salary;
		/// <MetaDataID>{E082D95F-F533-42B7-AD5C-69C2859FA5B0}</MetaDataID>
		[OOAdvantech.MetaDataRepository.MetaObjectID("4")]
		[OOAdvantech.PersistenceLayer.PersistentAssociation("Job",typeof(PersistenceLayerTestPrj.Company),"Employer",OOAdvantech.PersistenceLayer.AssotiationEnd.RoleA,OOAdvantech.PersistenceLayer.PersistencyFlag.ReferentialIntegrity|OOAdvantech.PersistenceLayer.PersistencyFlag.OnConstruction,0,1)]
		public Job Employer;

		public Job GetEmployer()
		{
			if(Employer==null)
				OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties).LazyFetching("Employer",GetType());
			return Employer;
		}
	
		/// <summary></summary>
		/// <MetaDataID>{1B2B6DEA-349D-44A4-9F65-AD995ED48EF7}</MetaDataID>
		 public Employee()
		{
			Salary=33000; 
			 //TheParents=new Person();
		}
	
	

		/// <summary></summary>
		/// <MetaDataID>{97990E3E-ADF9-46DA-BA7B-0D8C7D6EF03E}</MetaDataID>
		public void Foo()
		{
			OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties).LazyFetching("TheAddress",typeof(Employee));
			
			//long ObjectStateTransitionID=OOAdvantech.PersistenceLayer.PersistencyContext.CurrentPersistencyContext.BeginObjectStateTransition
				//(this,OOAdvantech.Transactions.TransactionOption.Supported);

			//OOAdvantech.PersistenceLayer.PersistencyContext.CurrentPersistencyContext.CommitObjectStateTransition(this,ObjectStateTransitionID);
		}
	}
}
