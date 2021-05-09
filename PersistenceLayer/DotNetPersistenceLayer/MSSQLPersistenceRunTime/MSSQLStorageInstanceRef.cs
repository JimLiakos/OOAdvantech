namespace OOAdvantech.MSSQLPersistenceRunTime
{
    
	/// <MetaDataID>{F8ED25A2-C2E7-4214-BC16-DBD19449DA4E}</MetaDataID>
    public class StorageInstanceRef : RDBMSPersistenceRunTime.StorageInstanceRef
	{

        
		/// <MetaDataID>{509374ED-0250-4A58-91EF-80970EFF0D8D}</MetaDataID>
        public StorageInstanceRef(object memoryInstance, OOAdvantech.MetaDataRepository.StorageCell storageCell, ObjectStorage activeStorageSession, PersistenceLayer.ObjectID objectID)
            : base(memoryInstance, storageCell, activeStorageSession, objectID)
        {


        }

		/// <MetaDataID>{C84718E0-94C7-4518-ABDA-2AF28B337DB3}</MetaDataID>
        protected override PersistenceLayerRunTime.RelResolver CreateRelationResolver(DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
		{
            return new RDBMSPersistenceRunTime.RelResolver(this, thePersistentAssociationEnd, fastFieldAccessor);
		}
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef owner)
        {
            return new RDBMSPersistenceRunTime.RelResolver(owner, thePersistentAssociationEnd, fastFieldAccessor);
            
        }
		/// <MetaDataID>{634CECE8-1DBD-4775-9B2D-868F14009085}</MetaDataID>
		public string StorageInstanceSetIdentity
		{
			get
			{
				return StorageInstanceSet.Identity.ToString();
			}
		}
		/// <MetaDataID>{1EF210B7-836C-448C-B75B-FC1573C9C5B6}</MetaDataID>
		/// <MetaDataID>{A4EBCC66-14FE-466F-BDEE-E3013C161669}</MetaDataID>
		/// <MetaDataID>{09FE4875-30B3-405E-9E0B-38C925ABD6D4}</MetaDataID>


      
    }
}
