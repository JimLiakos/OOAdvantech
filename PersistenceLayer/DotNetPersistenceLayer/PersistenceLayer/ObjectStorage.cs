namespace OOAdvantech.PersistenceLayer
{

	public delegate void ObjectStorageChangedEventHandler(object sender);

    /// <MetaDataID>{4274E115-61AF-4E45-BAA3-8978B222B144}</MetaDataID>
    /// <summary>Provide the basic functionality for storage metadata. 
    /// With this interface you can retrieve information about the storage name location and type 
    /// also you can add a new component "assembly" to the storage.</summary>
	public interface Storage
	{
		/// <MetaDataID>{EB010418-8709-4D17-B158-5A23B4EDAAB2}</MetaDataID>
		/// <summary>Define the identity of storage. 
		/// Actually it is global unique ID as string.</summary>
		string StorageIdentity
		{
			get;
 
		}

        /// <MetaDataID>{b8a909b5-02fe-47ab-8620-f3f1ff3b6281}</MetaDataID>
        string Culture
        {
            get;
            set;
        }


        //		/// <summary>With this method construct or update the places 
        //		/// that use the persistent classes to store their instances.</summary>
        //		/// <MetaDataID>{074133C7-E786-4D4D-956A-9BCE7499A1C1}</MetaDataID>
        //		void Build();
        /// <summary>With this method add a new component "assembly" to storage
        /// or update an existing.</summary>
        /// <MetaDataID>{96B07A32-DC39-4E1B-8D2D-1B714EABB546}</MetaDataID>
        void RegisterComponent(string assemblyFullName,System.Collections.Generic.List<string> types=null);

        /// <MetaDataID>{b2c63a22-6d73-47e2-a3e0-6786a2a3d420}</MetaDataID>
		void RegisterComponent(string[] assembliesFullNames);


        /// <MetaDataID>{f81306b9-1a68-42e9-b406-a6fbf84dd95a}</MetaDataID>
        void RegisterComponent(string assemblyFullName, string mappingDataResourceName, System.Collections.Generic.List<string> types = null);
        /// <MetaDataID>{f77bf71a-52a7-4c3d-ac3e-785bf0569095}</MetaDataID>
        void RegisterComponent(string assemblyFullName, System.Xml.Linq.XDocument mappingData);
        /// <MetaDataID>{e16fed5d-221a-4506-93ba-c091add5eeac}</MetaDataID>
        void RegisterComponent(string[] assembliesFullNames, System.Collections.Generic.Dictionary<string, System.Xml.Linq.XDocument> assembliesMappingData);



		/// <MetaDataID>{1a0a18f8-a051-4c7a-a897-52ce9faa2130}</MetaDataID>
		/// <summary>This string used to specify the location of new storage. 
		/// Maybe it is a SQL server name or URL (Uniform Resource Locator). </summary>
		string StorageLocation
		{
			get;
			set;
		}
	
		/// <MetaDataID>{e0c69d4d-d8eb-44e8-885b-9a43022d6389}</MetaDataID>
		/// <summary>Specify the type of storage. 
		/// Actual is the full name of Storage Provider for example
		/// OOAdvantech.MSSQLPersistenceRunTime.StorageProvider.
		/// </summary>
		string StorageType
		{
			get;
			set;
		}
	
		/// <MetaDataID>{c6412028-790a-4c80-b77a-8c2c14410f4f}</MetaDataID>
		/// <summary>The name of Object Storage</summary>
		string StorageName
		{
			get;
			set;
		}

        /// <MetaDataID>{eb216bd0-0bae-44a2-b671-4c4905ec61cf}</MetaDataID>
        string NativeStorageID
        {
            get;
            set;
        }

        bool CheckForVersionUpgrate(string fullName);
    }

}
