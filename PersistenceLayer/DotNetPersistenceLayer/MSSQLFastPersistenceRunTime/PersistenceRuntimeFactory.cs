namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
	/// <MetaDataID>{F24BBE18-EE08-4EF3-964A-58D85258A85E}</MetaDataID>
	public class PersistenceRuntimeFactory : OOAdvantech.PersistenceLayerRunTime.PersistenceRuntimeFactory
	{
		/// <MetaDataID>{50610DD0-62D5-4F65-9E9C-9F1562C033A4}</MetaDataID>
		public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object MemoryInstance)
		{
			//return new StorageInstanceRef(memoryInstance,this,objectID);
			return null;
		}
		/// <MetaDataID>{662F71CA-39DA-4162-A347-10E0C19E7C82}</MetaDataID>
		public override void CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceRef RoleA, PersistenceLayerRunTime.StorageInstanceRef RoleB, PersistenceLayerRunTime.StorageInstanceRef RelationObject, MetaDataRepository.AssociationEnd ownerAssociationEnd, PersistenceLayerRunTime.StorageInstanceRef outStorageInstanceRef)
		{
//
//			PersistenceLayerRunTime.Commands.LinkObjectsCommand mLinkObjectsCommand=new Commands.LinkObjectsCommand();
//			mLinkObjectsCommand.RoleA=RoleA;
//			mLinkObjectsCommand.RoleB=RoleB;
//			mLinkObjectsCommand.RelationObject=RelationObject;
//			mLinkObjectsCommand.OwnerAssociationEnd=ownerAssociationEnd;
//			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
//			transactionContext.EnlistCommand(mLinkObjectsCommand);

		
		}
		/// <MetaDataID>{BC4919C4-E0D5-439F-93FA-A196ABBA736F}</MetaDataID>
		public override void CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceRef RoleA, PersistenceLayerRunTime.StorageInstanceRef RoleB, PersistenceLayerRunTime.StorageInstanceRef RelationObject, MetaDataRepository.AssociationEnd ownerAssociationEnd, PersistenceLayerRunTime.StorageInstanceRef outStorageInstanceRef)
		{
//			PersistenceLayerRunTime.Commands.UnLinkObjectsCommand mLinkObjectsCommand=new Commands.UnLinkObjectsCommand();
//			mLinkObjectsCommand.RoleA=RoleA;
//			mLinkObjectsCommand.RoleB=RoleB;
//			mLinkObjectsCommand.RelationObject=RelationObject;
//			mLinkObjectsCommand.OwnerAssociationEnd=ownerAssociationEnd;
//			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
//			transactionContext.EnlistCommand(mLinkObjectsCommand);
		}
		/// <MetaDataID>{F4CB6DBA-9BF2-4720-A8E6-4D44D051FB93}</MetaDataID>
		public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceRef DeletedStorageInstanceRef, MetaDataRepository.AssociationEnd AssociationEnd)
		{

			Commands.UnlinkAllObjectCommand mUnlinkAllObjectCommand = new Commands.UnlinkAllObjectCommand();
			mUnlinkAllObjectCommand.DeletedStorageInstance=DeletedStorageInstanceRef;
			mUnlinkAllObjectCommand.theAssociationEnd=AssociationEnd;
			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
			transactionContext.EnlistCommand(mUnlinkAllObjectCommand);
		
		}
		/// <MetaDataID>{94CD7CB7-CCB4-4229-8558-D0C3F2AF9497}</MetaDataID>
		public override PersistenceLayerRunTime.RelResolver CreateRelationResolver(MetaDataRepository.AssociationEnd thePersistentAssociationEnd)
		{
			//return new RelResolver(this,associationEnd);
			return null;
		}
		/// <MetaDataID>{BA5F0C9F-70E0-4AC7-970F-120139A92D37}</MetaDataID>
		/// <summary>Creates and return a new storage instance command for the storage instance reference. At this time the storage instance reference connect only with the storage in which will be created the storage instance. </summary>
		public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
		{
			Commands.NewStorageInstanceCommand newStorageInstanceCommand=new Commands.NewStorageInstanceCommand(StorageInstance as StorageInstanceRef);
			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
			transactionContext.EnlistCommand(newStorageInstanceCommand);
		}
		/// <MetaDataID>{C6AB279B-4F1C-4E58-B3A6-28D8754EC595}</MetaDataID>
		public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
		{
		
		}
		/// <MetaDataID>{F795595D-65BD-41CA-9FB2-63EC6245D00A}</MetaDataID>
		/// <summary>UpdateStorageInstanceCommand </summary>
		public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
		{
			Commands.UpdateStorageInstanceCommand updateStorageInstanceCommand=new  Commands.UpdateStorageInstanceCommand(storageInstanceRef);
			PersistenceLayerRunTime.TransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
			transactionContext.EnlistCommand(updateStorageInstanceCommand);

		
		}
		public override void CreateDeleteStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef StorageInstance, bool tryToDelete)
		{

		}

		


	
	}

}
