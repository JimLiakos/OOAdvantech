using OOAdvantech.RDBMSMetaDataRepository;
namespace OOAdvantech.MSSQLCompactPersistenceRunTime.Commands
{
	/// <MetaDataID>{C314EA3E-BCAB-4736-A6F5-5DD6C46369BD}</MetaDataID>
	public class UnlinkAllObjectCommand : PersistenceLayerRunTime.Commands.OnMemoryUnlinkAllObjectCommand
	{
		/// <MetaDataID>{3588848B-5C9C-4F92-A180-8B47D2592A5C}</MetaDataID>
		public UnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceRef deletedStorageInstance)
            :base(deletedStorageInstance)
		{
			int hfdh=0;
		}
		/// <MetaDataID>{4AD9BC6A-C5CF-4C35-A36A-1F079B6A7572}</MetaDataID>
		protected override PersistenceLayerRunTime.RelResolver theResolver
		{
			get
			{
                foreach (PersistenceLayerRunTime.RelResolver relResolcer in DeletedStorageInstance.RelResolvers)
                {
                    if (relResolcer.AssociationEnd == theAssociationEnd)
                        return relResolcer;
                }
                throw new System.Exception("System can't find the relation resolver for association end '" + theAssociationEnd.Name);
			}
		}

		/// <MetaDataID>{F5D0DF70-6F87-4149-836B-E7DFD3FE0851}</MetaDataID>
		public override void GetSubCommands(int currentExecutionOrder)
		{
			if(currentExecutionOrder<40)
				return ;
			if(!SubTransactionCmdsProduced)
			{
				base.GetSubCommands(currentExecutionOrder);

                RDBMSMetaDataRepository.Storage ObjectStorageMetadata = DeletedStorageInstance.ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage;
				RDBMSMetaDataRepository.Association Association=ObjectStorageMetadata.GetEquivalentMetaObject(theResolver.AssociationEnd.Association) as RDBMSMetaDataRepository.Association;

				if(Association.LinkClass==null)
				{
					foreach(RDBMSMetaDataRepository.StorageCellsLink CurrStorageCellsLink in Association.GetStorageCellsLinks(((StorageInstanceRef)DeletedStorageInstance).StorageInstanceSet))
					{
						RDBMSMetaDataRepository.AssociationEnd mAssociationEnd=((Storage)DeletedStorageInstance.ObjectStorage.StorageMetaData).GetEquivalentMetaObject(theAssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
						UnlinkAllObjectOfStorageCellLinkCmd mUnlinkAllObjectOfStorageCellLinkCmd=new UnlinkAllObjectOfStorageCellLinkCmd(CurrStorageCellsLink ,(StorageInstanceRef)DeletedStorageInstance,mAssociationEnd);

						PersistenceLayerRunTime.ITransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
						transactionContext.EnlistCommand(mUnlinkAllObjectOfStorageCellLinkCmd);

						RDBMSMetaDataRepository.StorageCellReference OutStorageCell=null;
						if(CurrStorageCellsLink.RoleAStorageCell.GetType()==typeof(RDBMSMetaDataRepository.StorageCellReference))
							OutStorageCell=CurrStorageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference;
						if(CurrStorageCellsLink.RoleBStorageCell.GetType()==typeof(RDBMSMetaDataRepository.StorageCellReference))
							OutStorageCell=CurrStorageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference;
						if(OutStorageCell!=null)
						{
							ObjectStorage objectStorage=(ObjectStorage)ObjectStorage.OpenStorage(OutStorageCell.StorageName,OutStorageCell.StorageLocation,"OOAdvantech.RDBMSPersistenceRunTime.StorageProvider");//Error Prone το πρόγραμμα κάνει την παραδοχή ότι η άλλη στοραγε είναι mssql
							objectStorage.CreateUnlinkAllObjectCommand(DeletedStorageInstance,mAssociationEnd,OutStorageCell.RealStorageCell);
						}
					}
				}
				base.GetSubCommands(currentExecutionOrder);
			}
				

		
		}
		/// <MetaDataID>{91350FCE-4B7E-42CE-894C-E7941C77B1F7}</MetaDataID>
		public override void Execute()
		{
			base.Execute();

	
		}
	}
}
