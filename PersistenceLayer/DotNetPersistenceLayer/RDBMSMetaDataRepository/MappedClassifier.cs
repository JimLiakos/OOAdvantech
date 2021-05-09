namespace OOAdvantech.RDBMSMetaDataRepository
{
	/// <MetaDataID>{2ED25507-B746-41FF-B650-0E2A43AC250F}</MetaDataID>
	public interface MappedClassifier
	{
		/// <summary>
		/// This property defines a view. 
		/// This view returns a record set where each record is an object 
		/// which support the mapping type.
		/// </summary>
		/// <MetaDataID>{E5AD0EE5-650A-4CF3-A5D5-55231602F6CB}</MetaDataID>
		[MetaDataRepository.Association("MappingTypeView",typeof(OOAdvantech.RDBMSMetaDataRepository.View),MetaDataRepository.Roles.RoleA,"{CFD346AA-0804-4A45-A654-8651D80CF995}")]
		[MetaDataRepository.RoleAMultiplicityRange(1,1)]
		[MetaDataRepository.RoleBMultiplicityRange(0,1)]
		View TypeView
		{
			get;
		} 
		
		/// <MetaDataID>{F40652C3-C792-4D8E-8DB4-D939F8D6C8AD}</MetaDataID>
		bool HasPersistentObjects
		{
			get;
		}
        ///// <MetaDataID>{015F4CD0-1451-4E20-B682-38752EFFB644}</MetaDataID>
        //View GetTypeView(Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells);
		/// <MetaDataID>{06F1F2CB-C969-43BB-9DFC-93A322F10E22}</MetaDataID>
		Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(System.DateTime TimePeriodStartDate, System.DateTime TimePeriodEndDate);
		/// <MetaDataID>{C7C1AFB1-3507-4D14-94BB-C46EBEF19FA5}</MetaDataID>
		StorageCell GetStorageCell(object ObjectID);


        ///<summary>
        ///This property defines a collection with columns of object identity.
        ///</summary>
        /// <MetaDataID>{A632A4D3-2359-4F61-A498-043BEE4F9CA8}</MetaDataID>
        System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType> ObjectIdentityTypes
        {
            get;
        }

        /// <MetaDataID>{30fb15f3-6df7-48ca-b7c1-df8240f4c55c}</MetaDataID>
        System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> GetObjectIdentityTypes(System.Collections.Generic.List<MetaDataRepository.StorageCell> storageCells);



		/// <MetaDataID>{D640047B-73DA-4525-91E0-16A1B8D5977F}</MetaDataID>
		string Name
		{
			get;
		}

		/// <summary>Define a collection with the storage cells 
		/// which the type of storage cell is subtype of classifier. </summary>
		/// <MetaDataID>{AEF5AB36-7322-4636-9F59-A36BA19ABA12}</MetaDataID>
        Collections.Generic.Set<MetaDataRepository.StorageCell> ClassifierLocalStorageCells
		{
			get;
		}
	}
}
