namespace OOAdvantech.PersistenceLayer
{
	using System;
	/// <MetaDataID>{B914BA0E-C662-4654-BCC9-52DBB15977AE}</MetaDataID>
	/// <summary></summary>
	public abstract class PersistencyContext :System.MarshalByRefObject, Remoting.IExtMarshalByRefObject
	{
		/// <summary>Commit all object changes.  The parameter TransactionType defines the type of the transaction in witch the object will commit it's state. For more information you can look at Transaction.OnObjectChangeSate enumerator documentation. </summary>
		/// <param name="Object">Define the object that will be persistent.</param>
		/// <param name="StorageSession">Define the storage session with storage in which will be creating the object.</param>
		/// <MetaDataID>{BBA350B3-DC1F-456B-99B7-CF4FFE913A94}</MetaDataID>
		public abstract void CommitObjectState(object Object, StorageSession StorageSession);
		/// <summary>This method creates a new object with type that defined from parameter Type in storage of the StorageSession Storage.</summary>
		/// <param name="Type">Define the type of object that will be creating.</param>
		/// <param name="Storage">Define the storage session with storage in which will be creating the object.</param>
		/// <MetaDataID>{C06899D2-0947-4FA5-9053-EF570B80D86F}</MetaDataID>
		public abstract object NewObject(System.Type Type, StorageSession Storage,params object[] ctorParams);
		/// <summary>This method creates a new object with type that defined from parameter Type in storage of the StorageSession Storage.</summary>
		/// <param name="Type">Define the type of object that will be creating.</param>
		/// <param name="Version">Define version of module that implement the type.</param>
		/// <param name="Storage">Define the storage session with storage in which will be creating the object.</param>
		/// <MetaDataID>{AB1ED766-9ECD-4774-B9D4-4EF832E35FEE}</MetaDataID>
		public abstract object NewObject(string Type, string Version, StorageSession Storage,params object[] ctorParams);
		/// <summary></summary>
		/// <MetaDataID>{493DDF5C-62E8-492D-880D-70F7FEC69983}</MetaDataID>
		
		/// <summary></summary>
		/// <MetaDataID>{FBBC6B24-2C3F-4E6C-94AE-8DDF74F7B442}</MetaDataID>
		protected static PersistencyContext _CurrentPersistencyContext;
		/// <summary></summary>
		/// <MetaDataID>{B940F5B9-6D9B-4FC6-88D4-B5E9A18F1F4D}</MetaDataID>
		public static PersistencyContext CurrentPersistencyContext
		{
			get
			{
				
				if (null == _CurrentPersistencyContext)
					_CurrentPersistencyContext =(PersistencyContext) ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.PersistenceLayerRunTime.ClientPersistencyContext","");
				return _CurrentPersistencyContext;
			}
		}

		/// <param name="Object">Define the object that will commit its state. </param>
		/// <MetaDataID>{12019BF7-15A6-4007-A37C-66D49A329E76}</MetaDataID>
		public abstract void CommitObjectState(object Object);
		
		/// <summary>The object deleted if passes all the criteria for object deletion (Referential integrity et cetera).
		/// This method tries to delete the corresponding storage instance of object if it pass all the criteria for object deletion (Referential integrity et cetera).</summary>
		/// <param name="thePersistentObject">The persistent object, which will be deleted. Remember that the object has implicitly connection with storage instance.</param>
		/// <MetaDataID>{c59b1cee-210c-4452-bf63-c323e53834a7}</MetaDataID>
		public abstract void DeleteObject(object thePersistentObject);


		public abstract StorageSession NewStorage(ObjectStorage OriginalStorage, string StorageName, string StorageLocation, string StorageType,bool InProcess);

		/// <summary>This method creates a new storage with name, type and location that defined from parameters StorageName, StorageType and StorageLocation.
		/// The schema of storage will be the same with the OriginalStorage.</summary>
		/// <param name="OriginalStorage">Define the Meta data of new storage. 
		/// The Persistency Context at first time will be cloning the OriginalStorage Meta data.
		/// After that will be create the schema of new Storage. For example tables for relational database, DTD data for XML files et cetera.</param>
		/// <param name="StorageName">Define the name of new storage.</param>
		/// <param name="StorageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). The format of string depends from the type of new storage</param>
		/// <param name="StorageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider.</param>
		/// <MetaDataID>{f8c60075-405a-4371-b704-6b8f651a2239}</MetaDataID>
		public abstract StorageSession NewStorage(ObjectStorage OriginalStorage, string StorageName, string StorageLocation, string StorageType);
	
 

		/// <summary>Create a storage access session.</summary>
		/// <param name="StorageName">The name of Object Storage</param>
		/// <param name="StorageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). 
		/// If it is null then the PersistencyContext will look at Persistence Layer repository.</param>
		/// <param name="StorageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider.
		/// If it is null then the PersistencyContext will look at Persistence Layer repository.</param>
		/// <MetaDataID>{7b7d8f1a-102b-4859-b2b5-e1e17c112430}</MetaDataID>
		public abstract StorageSession OpenStorage(string StorageName, string StorageLocation, string StorageType);


	}
}
