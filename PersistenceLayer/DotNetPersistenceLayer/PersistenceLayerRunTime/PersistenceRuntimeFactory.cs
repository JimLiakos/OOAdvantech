namespace OOAdvantech.PersistenceLayerRunTime
{
	/// <MetaDataID>{27E9A89B-AF20-4671-B02C-806CC1BC7413}</MetaDataID>
	public abstract class PersistenceRuntimeFactory
	{
		
		/// <MetaDataID>{E058F9E5-BF27-40D4-9F57-1585F04FE058}</MetaDataID>
		public abstract RelResolver CreateRelationResolver(MetaDataRepository.AssociationEnd thePersistentAssociationEnd);
		/// <MetaDataID>{F90B81D9-E95D-4F03-8540-B79D3326B997}</MetaDataID>
		public abstract void CreateUpdateReferentialIntegrity(StorageInstanceRef storageInstanceRef);
		/// <MetaDataID>{0C9F898F-2A50-46B3-A7F7-535020827318}</MetaDataID>
		/// <summary>UpdateStorageInstanceCommand </summary>
		public abstract void CreateDeleteStorageInstanceCommand(StorageInstanceRef StorageInstance, bool tryToDelete);
		/// <MetaDataID>{EC0C9C8A-6383-4EBB-A17C-159AC9A83F92}</MetaDataID>
		public abstract void CreateLinkCommand(StorageInstanceRef RoleA, StorageInstanceRef RoleB, StorageInstanceRef RelationObject, MetaDataRepository.AssociationEnd ownerAssociationEnd, StorageInstanceRef outStorageInstanceRef);
		/// <MetaDataID>{4AF26477-EC3B-44F3-BD17-7340125AC722}</MetaDataID>
		/// <summary>UpdateStorageInstanceCommand </summary>
		public abstract void CreateUpdateStorageInstanceCommand(StorageInstanceRef storageInstanceRef);
		/// <MetaDataID>{8FDEB673-E0D5-4409-AA91-5677BCC15850}</MetaDataID>
		public abstract void CreateUnlinkAllObjectCommand(StorageInstanceRef DeletedStorageInstanceRef,MetaDataRepository.AssociationEnd AssociationEnd);
		/// <MetaDataID>{3729EE55-48B8-4349-B457-846CB17124C4}</MetaDataID>
		public abstract StorageInstanceRef CreateStorageInstanceRef(object MemoryInstance);
		/// <MetaDataID>{E306363A-964A-4F3F-BB56-C50A4FBB9281}</MetaDataID>
		public abstract void CreateUnLinkCommand(StorageInstanceRef RoleA, StorageInstanceRef RoleB, StorageInstanceRef RelationObject, MetaDataRepository.AssociationEnd ownerAssociationEnd, StorageInstanceRef outStorageInstanceRef);
		/// <MetaDataID>{38650A1B-D459-45F7-8594-A149DB0F0FBB}</MetaDataID>
		/// <summary>Creates and return a new storage instance command for the storage instance reference. At this time the storage instance reference connect only with the storage in which will be created the storage instance. </summary>
		public abstract void CreateNewStorageInstanceCommand(StorageInstanceRef StorageInstance);
	}
}
