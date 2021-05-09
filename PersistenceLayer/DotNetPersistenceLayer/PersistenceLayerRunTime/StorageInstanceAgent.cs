namespace  OOAdvantech.PersistenceLayerRunTime
{
	
	using System;
	/// <MetaDataID>{E672197D-DC72-4789-99A3-AC61F698AD11}</MetaDataID>
	[Serializable]
	public class StorageInstanceAgent
	{
        /// <exclude>Excluded</exclude>
        MetaDataRepository.ValueTypePath _ValueTypePath;

        /// <MetaDataID>{2daa1a1c-9fc5-43b3-9429-2cc07c69294c}</MetaDataID>
        public MetaDataRepository.ValueTypePath ValueTypePath
        {
            get
            {
                return _ValueTypePath;
            }
        }
  

    

     //   public readonly int MemoryID = NextMemoryID;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{F112F4CE-EFA3-4590-A138-815A5A4196F5}</MetaDataID>
		private DotNetMetaDataRepository.Class _Class;
		/// <MetaDataID>{A89D58E0-736C-490C-851C-38C8A72A873C}</MetaDataID>
        public DotNetMetaDataRepository.Class Class
		{
			get
			{
				if(_Class==null)
				{
                    if (Remoting.RemotingServices.IsOutOfProcess(RealStorageInstanceRef))
                    {
                        Type type = ModulePublisher.ClassRepository.GetType(ClassFullName, Version);
                        _Class = DotNetMetaDataRepository.Type.GetClassifierObject(type) as DotNetMetaDataRepository.Class;
                    }
                    else
                        _Class = RealStorageInstanceRef.Class;
				}
				return _Class;
			}
		}
		/// <MetaDataID>{C63141A3-5821-43D1-8886-775683620FD1}</MetaDataID>
        private MetaDataRepository.MetaObjectID ClassMetaObjectID;

        /// <MetaDataID>{C63141A3-5821-43D1-8886-775683620FD1}</MetaDataID>
        private string ClassFullName;

        /// <MetaDataID>{d51b1c59-38a0-4b0b-8b68-8b6c1bbbd207}</MetaDataID>
        private string Version;

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{B6A1DCF7-971A-4062-91BE-2E839F8A3F2B}</MetaDataID>
		private object _MemoryInstance;
		/// <MetaDataID>{37644C00-EB01-4D91-A1E6-516B217F92F9}</MetaDataID>
        public object MemoryInstance
		{
			get
			{
				return _MemoryInstance;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{3BB7186E-358A-4337-B267-AC5745D4EEB2}</MetaDataID>
        private OOAdvantech.PersistenceLayer.ObjectID _PersistentObjectID;
		/// <MetaDataID>{0832D64B-2F29-4EE8-8EE0-C034506E5907}</MetaDataID>
        public OOAdvantech.PersistenceLayer.ObjectID PersistentObjectID
		{
			get
			{
				if(_PersistentObjectID==null)
					_PersistentObjectID=RealStorageInstanceRef.PersistentObjectID;
				return _PersistentObjectID;
			}
		}

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3BB7186E-358A-4337-B267-AC5745D4EEB2}</MetaDataID>
        private OOAdvantech.PersistenceLayer.ObjectID _ObjectID;
        /// <MetaDataID>{0832D64B-2F29-4EE8-8EE0-C034506E5907}</MetaDataID>
        public OOAdvantech.PersistenceLayer.ObjectID ObjectID
        {
            get
            {
                if (_ObjectID == null)
                    _ObjectID = RealStorageInstanceRef.ObjectID;
                return _ObjectID;
            }
        }

		/// <MetaDataID>{496E216E-00F1-42D0-9576-0FC0CCD10DDA}</MetaDataID>
        public ObjectStorage ObjectStorage;
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{E7E5C638-40F8-44F0-8E2D-4E18EBFD076D}</MetaDataID>
		private string _ObjectStorageURI;
		/// <MetaDataID>{42794881-DFA5-44B6-8993-D37A787009DD}</MetaDataID>
		public string ObjectStorageURI
		{
			get
			{
				return null;
			}
		}


//		/// <MetaDataID>{74A20522-CC96-4502-AAD4-978592F9F738}</MetaDataID>
//		public DotNetMetaDataRepository.Class Class
//		{
//			get
//			{
//				
//
//				if(Remoting.RemotingServices.IsOutOfProcess(RealStorageInstanceRef))
//				{
//					DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(ClassMetaObjectID) as DotNetMetaDataRepository.Class;
//				}
//				return null;
//			}
//		}
		/// <MetaDataID>{C4A9E93D-B266-4715-A990-09700DED6007}</MetaDataID>
		public static bool operator ==(StorageInstanceAgent l,StorageInstanceAgent r )
		{
			
			if(Object.Equals(r,null)&&Object.Equals(l,null))
				return true;
			if(!Object.Equals(r,null)&&Object.Equals(l,null))
				return false;
			if(Object.Equals(r,null)&&!Object.Equals(l,null))
				return false;


			return r.RealStorageInstanceRef==l.RealStorageInstanceRef;
		}
		/// <MetaDataID>{F7C3F8FE-8801-487C-801D-A98E44BCE65D}</MetaDataID>
		public static bool operator !=(StorageInstanceAgent l,StorageInstanceAgent r )
		{
			return !(l==r);
		}
        /// <MetaDataID>{46964f7e-6599-4c5f-8ad8-5005022e0d05}</MetaDataID>
        public readonly int MemoryID ;

        /// <MetaDataID>{d19b6fe1-987c-406f-a218-308dc0262cce}</MetaDataID>
        public readonly int ReferentialIntegrityCount;
		/// <MetaDataID>{19BCD8CB-B511-4184-90E5-409BF22F51CE}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private string Url;

        /// <MetaDataID>{0302bd5a-d577-4933-b1e7-38ad6165b19f}</MetaDataID>
        public readonly bool IsPersistent;

        /// <MetaDataID>{becf390f-57e6-41e5-b011-1ebb61060473}</MetaDataID>
        public readonly string StorageLocation;

        /// <MetaDataID>{f18d11fb-84ed-4660-bcae-55e7779dffaa}</MetaDataID>
        public readonly string StorageIdentity;

        /// <MetaDataID>{bd19a9f8-cc54-464b-b65d-ab6698b7aeba}</MetaDataID>
        public readonly string StorageName;

        /// <MetaDataID>{146915fd-ee2f-4d5c-98ba-6a2780516ba6}</MetaDataID>
        public readonly string StorageCellName;

        /// <MetaDataID>{d2fc120e-5bae-4b8b-9b1d-36347bdd13d7}</MetaDataID>
        public readonly string StorageType;

        /// <MetaDataID>{14eb42f5-e775-4e93-b347-716898af5093}</MetaDataID>
        public readonly int StorageCellSerialNumber;

        /// <MetaDataID>{094b566d-29ca-47ef-b6a5-f4c43c49d83e}</MetaDataID>
        public readonly MetaDataRepository.ObjectIdentityType ObjectIdentityType;

		/// <MetaDataID>{66E47FC2-6D42-4A88-976C-538DC2EAE54B}</MetaDataID>
        public StorageInstanceAgent(StorageInstanceRef storageInstanceRef)
		{

            StorageIdentity = storageInstanceRef.ObjectStorage.StorageMetaData.StorageIdentity;
            StorageLocation = storageInstanceRef.ObjectStorage.StorageMetaData.StorageLocation;
            StorageName = storageInstanceRef.ObjectStorage.StorageMetaData.StorageName;
            StorageType = storageInstanceRef.ObjectStorage.StorageMetaData.StorageType;

            StorageCellSerialNumber = storageInstanceRef.StorageInstanceSet.SerialNumber;
            StorageCellName = storageInstanceRef.StorageInstanceSet.Name;
            ObjectIdentityType = new MetaDataRepository.ObjectIdentityType(storageInstanceRef.StorageInstanceSet.ObjectIdentityType);

            MemoryID = storageInstanceRef.MemoryID;
         
			 if(storageInstanceRef==null)
				 throw new System.ArgumentNullException("the parameter storageInstanceRef must be not null");

             IsPersistent = storageInstanceRef.IsPersistent;

             ReferentialIntegrityCount = storageInstanceRef.ReferentialIntegrityCount;

             if (storageInstanceRef is StorageInstanceValuePathRef)
             {
                 _ValueTypePath = (storageInstanceRef as StorageInstanceValuePathRef).ValueTypePath;
                 storageInstanceRef = (storageInstanceRef as StorageInstanceValuePathRef).OriginalStorageInstanceRef;
             }
             else
                 _ValueTypePath = new MetaDataRepository.ValueTypePath();
            
			RealStorageInstanceRef=storageInstanceRef;

			ObjectStorage=RealStorageInstanceRef.ObjectStorage as ObjectStorage;
			 _PersistentObjectID=RealStorageInstanceRef.PersistentObjectID;
             _ObjectID = RealStorageInstanceRef.ObjectID;
			 _MemoryInstance=RealStorageInstanceRef.MemoryInstance;
             if (_MemoryInstance != null)
                 ClassFullName = _MemoryInstance.GetType().FullName;
             else
             {
                 Type type = RealStorageInstanceRef.Class.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                 ClassFullName = type.FullName;
             }
             
		}
        /// <MetaDataID>{d55861bf-0c0d-4d80-b2c1-2eb156266cf2}</MetaDataID>
        public MetaDataRepository.StorageCell GetStorageInstanceSet(ObjectStorage objectStorage)
        {
            return RealStorageInstanceRef.StorageInstanceSet;
        }
		/// <MetaDataID>{4EB3C68D-D1CB-4DED-9933-0EC574C5C77C}</MetaDataID>
		public readonly StorageInstanceRef RealStorageInstanceRef;
	}
}
