namespace PersistenceLayerTestPrj
{
	using MetaDataRepository=OOAdvantech.MetaDataRepository;
	using PersistenceLayer=OOAdvantech.PersistenceLayer;

	/// <MetaDataID>{CDF47ABA-BEC6-472D-8475-E96ECBE18B5D}</MetaDataID>
	/// <summary></summary>
	[MetaDataRepository.MetaObjectID("{CDF47ABA-BEC6-472D-8475-E96ECBE18B5D}")]
	[PersistenceLayer.Persistent("<ExtMetaData><RDBMSInheritanceMapping>OneTablePerConcreteClass</RDBMSInheritanceMapping></ExtMetaData>")]
	public class Person: System.MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
	{
	
		/// <MetaDataID>{F74F79B4-3FB7-4304-9812-7E74AA95F6F5}</MetaDataID>
		public void AdvanceAge()
		{

			using (OOAdvantech.Transactions.ObjectStateTransition StateTransition=new OOAdvantech.Transactions.ObjectStateTransition(this))
			{
				Age++;
				StateTransition.SetComplete();
			}
			int count =23;
			while(count<21)
			{
				count++;
				Person mPerson=OOAdvantech.PersistenceLayer.StorageSession.GetStorageOfObject(Properties).NewObject(typeof(Person)) as Person;
			}



		}
	
		/// <MetaDataID>{0EA3501C-23CE-4859-858E-272584438C8B}</MetaDataID>
		public PersistenceLayerTestPrj.ObjectCollection TheAddress=new  PersistenceLayerTestPrj.ObjectCollection();
		/// <MetaDataID>{EBE8C846-EBF2-4B6D-8A12-712715C6B587}</MetaDataID>
		public void AddAdrress()
		{
			//long ObjectStateTransitionID=OOAdvantech.PersistenceLayer.PersistencyContext.CurrentPersistencyContext.BeginObjectStateTransition(this,OOAdvantech.Transactions.TransactionOption.Required);			
			
			OOAdvantech.PersistenceLayer.StructureSet aStructureSet=OOAdvantech.PersistenceLayer.StorageSession.GetStorageOfObject(this).Execute(
				"SELECT theAddress FROM "+typeof(PersistenceLayerTestPrj.Address).FullName+" theAddress WHERE theAddress.City ='Patra'");
			
			PersistenceLayerTestPrj.Address mAddress=null;
			
			foreach( OOAdvantech.PersistenceLayer.StructureSet Rowset  in aStructureSet)
			{
				mAddress=Rowset.Members["theAddress"].Value as PersistenceLayerTestPrj.Address;
				int k=0;
			}
			TheAddress.Add(mAddress);
/*
			using (OOAdvantech.Transactions.ObjectStateTransition StateTransition=new OOAdvantech.Transactions.ObjectStateTransition(this))
			{
				long count =0;
				//while(count<1000)
				{
					count++;
					Address mAddress=AppPersistencyContext.NewObject(typeof(Address),OOAdvantech.PersistenceLayer.StorageSession.GetStorageOfObject(Properties)) as Address;
					TheAddress.Add(mAddress);
				}
				StateTransition.SetComplete();
			}*/
			//OOAdvantech.PersistenceLayer.PersistencyContext.CurrentPersistencyContext.CommitObjectStateTransition(this,ObjectStateTransitionID);
			/*
			OOAdvantech.PersistenceLayer.PersistencyContext.CurrentPersistencyContext.BeginObjectStateTransition(this,OOAdvantech.Transactions.TransactionOption.Required);
			PersistenceLayerTestPrj.Address mAddress=
			(PersistenceLayerTestPrj.Address)OOAdvantech.PersistenceLayer.PersistencyContext.CurrentPersistencyContext.NewObject("PersistenceLayerTestPrj.Address","",theStorageInstanceRef.ActiveStorageSession);
			TheAddress.Add(mAddress); 
			OOAdvantech.PersistenceLayer.PersistencyContext.CurrentPersistencyContext.CommitObjectStateTransition(this);
			*/

		}
		/// <MetaDataID>{4A711E47-8A12-4298-BED0-A85E3CFE4BA7}</MetaDataID>
		public OOAdvantech.ExtensionProperties Properties;
		/// <summary></summary>
		/// <MetaDataID>{67F08549-00EC-429B-AD7F-8F1424C52470}</MetaDataID>
		[MetaDataRepository.MetaObjectID("5")]
		[PersistenceLayer.PersistentAssociation("PersonChildren",typeof(PersistenceLayerTestPrj.Person),"TheParents",PersistenceLayer.AssotiationEnd.RoleB,1,PersistenceLayer.Multiplicity.NoLimit)]
		public PersistenceLayerTestPrj.ObjectCollection TheParents;
		/// <summary></summary>
		/// <MetaDataID>{D9DBBB01-B2D8-42AE-BA5A-13234D4BCC6E}</MetaDataID>
		[MetaDataRepository.MetaObjectID("6")]
		[PersistenceLayer.PersistentAssociation("PersonChildren",typeof(PersistenceLayerTestPrj.Person),"TheChildrens",PersistenceLayer.AssotiationEnd.RoleA,0,PersistenceLayer.Multiplicity.NoLimit)]
		public PersistenceLayerTestPrj.ObjectCollection TheChildrens;
		/// <summary></summary>
		/// <MetaDataID>{525B86D6-C0A2-406D-921E-E9E5B3943670}</MetaDataID>
		[MetaDataRepository.MetaObjectID("2")]
		[PersistenceLayer.PersistentField(PersistenceLayer.PersistencyFlag.LazyFetching)]
		public string Name;
		/// <summary></summary>
		/// <MetaDataID>{078B5C86-71AF-4B1C-9F20-EE90E65D81CB}</MetaDataID>
		[MetaDataRepository.MetaObjectID("1")]
		[PersistenceLayer.PersistentField()]
		public long Age;

	/*	public long mAge
		{
			get
			{
				return Age;
			}
		} */
			
		/// <summary></summary>
		/// <MetaDataID>{F297A273-A699-4FD2-B652-C34A2F929374}</MetaDataID>
		 public Person()
		{
		 
			Name="Kitsos";
			Age=32;
		}
		/// <MetaDataID>{63B137B0-1357-49C4-AE3A-A9C1C87DB465}</MetaDataID>
		 public Person(string firstName, string lastName, int age)
		{
		 
			Name="Kitsos";
			Age=32;
		}

	}
}
