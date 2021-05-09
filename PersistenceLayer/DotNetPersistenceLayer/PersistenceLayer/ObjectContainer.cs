namespace OOAdvantech.PersistenceLayer
{
    using System;

	/// <MetaDataID>{AC50744C-5E6C-4B27-A6ED-37FE87BA2C0C}</MetaDataID>
	/// <summary>
	/// ObjectCntainer is the class which must inherit all collection class which will be used,
	/// for  <see cref="OOAdvantech.MetaDataRepository.AssociationEnd">association end</see> with multiplicity type many relationships. 
	/// </summary>
	[Serializable]
	[OOAdvantech.Transactions.ContainByValue]
    public abstract class ObjectContainer 
	{
        /// <MetaDataID>{c6ac7586-7d44-4287-8e6e-bc989136f92c}</MetaDataID>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return GetObjectEnumerator();
        }
        /// <MetaDataID>{11907d08-4a05-4435-be1f-2d442e119b33}</MetaDataID>
        abstract protected System.Collections.IEnumerator GetObjectEnumerator();

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{884F8FF8-6BC7-42E1-A48F-37958B379350}</MetaDataID>
		internal protected ObjectCollection _theObjects;
			
		/// <MetaDataID>{5F981042-B824-4399-B839-3554BA98EB59}</MetaDataID>
		internal protected virtual ObjectCollection theObjects
		{
			get
			{
                if (_theObjects == null)
                {
#if !DeviceDotNet
                    //_PersistencyService = (IPersistencyService)AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.PersistencyService", "PersistenceLayerRunTime,  Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b"));

                    //if (System.Environment.Version.Major == 4)
                    //{
                    //    if (ObjectCollectionFactory.MonoStateObjectCollectionFactory == null)
                    //        AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectCollectionFactory", "PersistenceLayerRunTime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b"));
                    //    _theObjects = ObjectCollectionFactory.MonoStateObjectCollectionFactory.CreateOnMemoryCollection();
                    //}
                    //else
                    {

                        if (ObjectCollectionFactory.MonoStateObjectCollectionFactory == null)
                            AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectCollectionFactory", "PersistenceLayerRunTime,  Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b"));
                        _theObjects = ObjectCollectionFactory.MonoStateObjectCollectionFactory.CreateOnMemoryCollection();
                    }
#else
                    if (ObjectCollectionFactory.MonoStateObjectCollectionFactory == null)
                        AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectCollectionFactory", "PersistenceLayerRunTime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                    _theObjects = ObjectCollectionFactory.MonoStateObjectCollectionFactory.CreateOnMemoryCollection();
#endif
                }
                
            
				return _theObjects;
			}

		}


        /// <MetaDataID>{ec1de1d3-99de-4dca-8742-60313c60da71}</MetaDataID>
        protected void Init(System.Collections.ICollection collection)
        {

            if (ObjectCollectionFactory.MonoStateObjectCollectionFactory == null)
            {
#if DeviceDotNet

                AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectCollectionFactory", "PersistenceLayerRunTime, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null"));
#else
                AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectCollectionFactory", "PersistenceLayerRunTime,  Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b"));
#endif
                
                
            }
            _theObjects = ObjectCollectionFactory.MonoStateObjectCollectionFactory.CreateOnMemoryCollection(collection);
        }
            
    }
}
