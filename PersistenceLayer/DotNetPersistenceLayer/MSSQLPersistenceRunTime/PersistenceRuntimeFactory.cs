namespace OOAdvantech.MSSQLPersistenceRunTime
{
	/// <MetaDataID>{6021DDB7-4B38-4FB5-9F94-FE5A048D2B49}</MetaDataID>
	public class PersistenceRuntimeFactory : OOAdvantech.PersistenceLayerRunTime.PersistenceRuntimeFactory
	{
		/// <MetaDataID>{802123E3-E205-46A2-ADBA-F891CDBF61F3}</MetaDataID>
		public override void CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceRef RoleA, PersistenceLayerRunTime.StorageInstanceRef RoleB, PersistenceLayerRunTime.StorageInstanceRef RelationObject, MetaDataRepository.AssociationEnd ownerAssociationEnd, PersistenceLayerRunTime.StorageInstanceRef outStorageInstanceRef)
		{
			//if(Remoting.RemotingServices.IsOutOfProcess(ownerAssociationEnd))
				//ownerAssociationEnd=(StorageMetaData as MSSQLPersistenceRunTime.Storage).GetEquivalentMetaObject(ownerAssociationEnd) as RDBMSMetaDataRepository.RDBMSMappingAssociationEnd;
//
//			PersistenceLayerRunTime.Commands.LinkCommand mUnLinkObjectsCommand=null;
//			if(outStorageInstanceRef==null)
//			{
//				mUnLinkObjectsCommand=new Commands.UnLinkObjectsCommand();
//				mUnLinkObjectsCommand.RoleA=RoleA;
//				mUnLinkObjectsCommand.RoleB=RoleB;
//				mUnLinkObjectsCommand.RelationObject=RelationObject;
//				mUnLinkObjectsCommand.OwnerAssociationEnd=ownerAssociationEnd;
//			}
//			else
//			{
//				mUnLinkObjectsCommand=new Commands.InterSorageUnLinkObjectsCommand();
//				mUnLinkObjectsCommand.RoleA=RoleA;
//				mUnLinkObjectsCommand.RoleB=RoleB;
//				mUnLinkObjectsCommand.RelationObject=RelationObject;
//				mUnLinkObjectsCommand.OwnerAssociationEnd=ownerAssociationEnd as RDBMSMetaDataRepository.RDBMSMappingAssociationEnd;
//				(mUnLinkObjectsCommand as Commands.InterSorageUnLinkObjectsCommand).OutStorageInstanceRef=outStorageInstanceRef;
//			}
//			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
//			transactionContext.EnlistCommand(mUnLinkObjectsCommand);
		}
		/// <MetaDataID>{71401BE1-A642-466C-AC3C-F840B84161C0}</MetaDataID>
		public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
		{
			//TODO:Μεγάλο πρόβλημα αν το transaction έχει πολλές μεταβολές δηλαδή πολλά commands
			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;

			Commands.UpdateReferentialIntegrity updateReferentialIntegrity=new Commands.UpdateReferentialIntegrity();
			updateReferentialIntegrity.UpdatedStorageInstanceRef=storageInstanceRef;
			transactionContext.EnlistCommand(updateReferentialIntegrity);

		
		}
		/// <MetaDataID>{F2EB0BAE-DBA9-4B0B-8D50-05375503D396}</MetaDataID>
		public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object MemoryInstance)
		{
			return null;
			//return new StorageInstanceRef(memoryInstance,this,objectID);
		}
		/// <MetaDataID>{EBE9A2AC-11C7-46EC-85E2-D5412C930E17}</MetaDataID>
		public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance, bool tryToDelete)
		{
		
		}
		/// <MetaDataID>{9C40DC99-5948-4C5B-BCB6-F994B1856F4F}</MetaDataID>
		public override void CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceRef RoleA, PersistenceLayerRunTime.StorageInstanceRef RoleB, PersistenceLayerRunTime.StorageInstanceRef RelationObject, MetaDataRepository.AssociationEnd ownerAssociationEnd, PersistenceLayerRunTime.StorageInstanceRef outStorageInstanceRef)
		{
//			if(Remoting.RemotingServices.IsOutOfProcess(ownerAssociationEnd))
//				ownerAssociationEnd=(StorageMetaData as MSSQLPersistenceRunTime.Storage).GetEquivalentMetaObject(ownerAssociationEnd) as RDBMSMetaDataRepository.RDBMSMappingAssociationEnd;

//			PersistenceLayerRunTime.Commands.LinkCommand mLinkObjectsCommand=null;
//			if(outStorageInstanceRef==null)
//			{
//				mLinkObjectsCommand=new Commands.LinkObjectsCommand();
//				mLinkObjectsCommand.RoleA=RoleA;
//				mLinkObjectsCommand.RoleB=RoleB;
//				mLinkObjectsCommand.RelationObject=RelationObject;
//				mLinkObjectsCommand.OwnerAssociationEnd=ownerAssociationEnd;
//			}
//			else
//			{
//				mLinkObjectsCommand=new Commands.InterSorageLinkObjectsCommand();
//				mLinkObjectsCommand.RoleA=RoleA;
//				mLinkObjectsCommand.RoleB=RoleB;
//				mLinkObjectsCommand.RelationObject=RelationObject;
//				mLinkObjectsCommand.OwnerAssociationEnd=ownerAssociationEnd as RDBMSMetaDataRepository.RDBMSMappingAssociationEnd;
//				(mLinkObjectsCommand as Commands.InterSorageLinkObjectsCommand).OutStorageInstanceRef=outStorageInstanceRef;
//			}
//			
//			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
//			transactionContext.EnlistCommand(mLinkObjectsCommand);

		}
		/// <MetaDataID>{D08A5366-D04B-4641-8D2A-21CD32565C77}</MetaDataID>
		public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceRef DeletedStorageInstanceRef, MetaDataRepository.AssociationEnd AssociationEnd)
		{
//			if(Remoting.RemotingServices.IsOutOfProcess(AssociationEnd))
//				AssociationEnd=(StorageMetaData as MSSQLPersistenceRunTime.Storage).GetEquivalentMetaObject(AssociationEnd) as RDBMSMetaDataRepository.RDBMSMappingAssociationEnd;

			Commands.UnlinkAllObjectCommand mUnlinkAllObjectCommand = new Commands.UnlinkAllObjectCommand();
			mUnlinkAllObjectCommand.DeletedStorageInstance=DeletedStorageInstanceRef;
			mUnlinkAllObjectCommand.theAssociationEnd=AssociationEnd;

			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
			transactionContext.EnlistCommand(mUnlinkAllObjectCommand );

		
		}
		/// <MetaDataID>{BE4C1F52-B790-40F5-8205-5DC6F6472535}</MetaDataID>
		public override PersistenceLayerRunTime.RelResolver CreateRelationResolver(MetaDataRepository.AssociationEnd thePersistentAssociationEnd)
		{
			return null;
		}
		/// <MetaDataID>{AAC3354B-3C89-45A2-9E93-5A71B5B6F685}</MetaDataID>
		/// <summary>UpdateStorageInstanceCommand </summary>
		public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
		{
		
		}

		/// <MetaDataID>{7005F124-B20B-49FE-AB9B-AD69DB472C17}</MetaDataID>
		/// <summary>Creates and return a new storage instance command for the storage instance reference. At this time the storage instance reference connect only with the storage in which will be created the storage instance. </summary>
		public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
		{
		
		}
	}
}
	
