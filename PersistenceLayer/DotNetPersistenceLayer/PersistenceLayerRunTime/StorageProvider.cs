namespace OOAdvantech.PersistenceLayerRunTime
{
	using System;
	using PersistenceLayer;
	/// <metadataid>{FAE6A6C7-595F-43F8-9DD7-EDCB10680AC2}</metadataid>
	/// <summary>Storage provider is a communication channel with the runtime that specialize the Persistence Layer Framework for corresponding type of storage.</summary>
	public abstract class StorageProvider
	{
        /// <MetaDataID>{b98ab0c2-c4dc-453b-91dc-85177240e06f}</MetaDataID>
        public abstract string GetInstanceName(string StorageName, string StorageLocation);
		/// <MetaDataID>{ED3627D3-C3E4-41EE-BE80-A6737F3302E8}</MetaDataID>
		public abstract string GetHostComuterName(string StorageName, string StorageLocation);
		/// <metadataid>{1F881E78-5378-4777-AA99-BF60BA6525FB}</metadataid>
		/// <summary>The Provider identity. Globally unique.</summary>
		public abstract Guid ProviderID
		{
			get;
			set;
		}
		
		
		
		/// <metadataid>{c2cda9b0-ff9d-4b8e-8b75-29992ff4366b}</metadataid>
		/// <summary>The Provider Name</summary>
		public string Name;
		/// <summary>Create a new Object Storage with schema like original storage and open a storage session with it.</summary>
		/// <param name="OriginalStorage">Cloned Metada (scema)</param>
		/// <param name="StorageName">The name of new Object Storage</param>
		/// <param name="StorageLocation">This parameter contains the location of object storage.
		/// If it is null then the provider will look at Persistence Layer repository.</param>
		/// <MetaDataID>{30f429c3-f188-4601-9e98-feea9e4519ac}</MetaDataID>
		public abstract ObjectStorage NewStorage(Storage OriginalStorage, string storageName, string storageLocation, string userName = "", string password = "");

        /// <summary>Create a new Object Storage with schema like original storage and open a storage session with it.</summary>
        /// <param name="OriginalStorage">Cloned Metada (scema)</param>
        /// <param name="StorageName">The name of new Object Storage</param>
        /// <param name="rawStorageData">This parameter defines an object which contains the raw data for stored object.
        /// It can be an xml document a file or memory stream etc. the storage loaded in process by default. 
        /// </param>
        /// <MetaDataID>{10626489-2361-49ff-b2d8-d512479f84ba}</MetaDataID>
        public abstract ObjectStorage NewStorage(Storage OriginalStorage, string StorageName, object rawStorageData);

	
		/// <summary>Create a storage access session.</summary>
		/// <param name="storageName">The name of Object Storage</param>
		/// <param name="storageLocation">This parameter contains the location of object storage.
		/// If it is null then the provider will look at Persistence Layer repository.</param>
		/// <MetaDataID>{5b4a57c1-3433-4187-acde-1c2593544664}</MetaDataID>
		public abstract ObjectStorage OpenStorage(string storageName, string storageLocation, string userName = "", string password = "");


        /// <summary>Create a storage access session.</summary>
        /// <param name="StorageName">The name of Object Storage</param>
        /// <param name="rawStorageData">This parameter defines an object which contains the raw data for stored object.
        /// It can be an xml document a file or memory stream etc. the storage loaded in process by default. 
        /// </param>
        /// <MetaDataID>{69b0cf16-33ce-4b79-a0f9-1d8100b7f966}</MetaDataID>
        public abstract ObjectStorage OpenStorage(string StorageName, object rawStorageData);


		/// <MetaDataID>{6F567FFB-30C2-41D4-83F1-77C15F1D01DF}</MetaDataID>
		public abstract bool IsEmbeddedStorage(string StorageName, string StorageLocation);

		/// <MetaDataID>{734C03D6-8BC5-480A-ACB6-388A938D1C0D}</MetaDataID>
		public abstract bool AllowEmbeddedStorage();

        /// <MetaDataID>{bfd973b6-9da9-468d-9e06-40d657947cac}</MetaDataID>
        public abstract void DeleteStorage(string storageName, string storageLocation);



        /// <MetaDataID>{9036fcc0-51cd-4f91-8b60-921d7d2d99d6}</MetaDataID>
        public abstract Storage AttachStorage(string storageName, string storageLocation, string nativeStorageConnectionString);

        /// <MetaDataID>{164435ae-5529-4a36-ac98-5480a9b5fd48}</MetaDataID>
        public abstract string GetNativeStorageID(string storageDataLocation);


        public abstract OOAdvantech.PersistenceLayer.ObjectStorage CreateNewLogicalStorage(OOAdvantech.PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName);

        public abstract void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password);

        public abstract void Repair(string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password);
    }
}
