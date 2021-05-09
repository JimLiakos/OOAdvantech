namespace OOAdvantech.MetaDataLoadingSystem
{
	/// <MetaDataID>{36C55C2E-DACF-42A6-B244-29DBBD196260}</MetaDataID>
	public class PersistenceRuntimeFactory : OOAdvantech.PersistenceLayerRunTime.PersistenceRuntimeFactory
	{

		/// <MetaDataID>{5D5271FA-1889-48B6-9DE5-2D1E709A72BC}</MetaDataID>
		public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
		{
			
		}

		/// <MetaDataID>{B8631D93-37FC-4B61-BAB9-0912029DFBC3}</MetaDataID>
		public override PersistenceLayerRunTime.RelResolver CreateRelationResolver(MetaDataRepository.AssociationEnd thePersistentAssociationEnd)
		{
			return null;
		
		}
		/// <MetaDataID>{20768833-0E24-4D5A-9B98-50CD7884C353}</MetaDataID>
		public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceRef DeletedStorageInstanceRef, MetaDataRepository.AssociationEnd AssociationEnd)
		{
		
		}

		/// <MetaDataID>{F5D66F1C-3252-405C-A197-7353F47A930B}</MetaDataID>
		public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object MemoryInstance)
		{
			return null;
		
		}
		/// <MetaDataID>{306DCDD2-72AF-4969-BFFE-1756097B38A3}</MetaDataID>
		public override void CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceRef RoleA, PersistenceLayerRunTime.StorageInstanceRef RoleB, PersistenceLayerRunTime.StorageInstanceRef RelationObject, MetaDataRepository.AssociationEnd ownerAssociationEnd, PersistenceLayerRunTime.StorageInstanceRef outStorageInstanceRef)
		{
		
		}
		/// <summary>Creates and return a new storage instance command for the storage instance reference. At this time the storage instance reference connect only with the storage in which will be created the storage instance. </summary>
		/// <MetaDataID>{6465881C-2E33-4243-8242-F52BD703EE42}</MetaDataID>
		public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
		{
		
		}
		/// <summary>UpdateStorageInstanceCommand </summary>
		/// <MetaDataID>{C347FCF3-1F6B-4FEC-9F60-CD7DC8C280E3}</MetaDataID>
		public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
		{
		
		}

		/// <MetaDataID>{66FB61D4-72B6-4903-89E1-824468F1EE8F}</MetaDataID>
		public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance, bool tryToDelete)
		{
		
		}


		/// <MetaDataID>{79039F1C-BB31-481D-B3B3-0B6BB2E825EF}</MetaDataID>
		public override void CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceRef RoleA, PersistenceLayerRunTime.StorageInstanceRef RoleB, PersistenceLayerRunTime.StorageInstanceRef RelationObject, MetaDataRepository.AssociationEnd ownerAssociationEnd, PersistenceLayerRunTime.StorageInstanceRef outStorageInstanceRef)
		{
		
		}

	}
}
