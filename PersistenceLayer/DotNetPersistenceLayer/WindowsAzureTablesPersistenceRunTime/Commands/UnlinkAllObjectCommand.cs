using OOAdvantech.RDBMSMetaDataRepository;
namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{
    /// <MetaDataID>{C314EA3E-BCAB-4736-A6F5-5DD6C46369BD}</MetaDataID>
    public class UnlinkAllObjectCommand : PersistenceLayerRunTime.Commands.OnMemoryUnlinkAllObjectCommand
    {

        ObjectStorage ObjectStorage;
        /// <MetaDataID>{3588848B-5C9C-4F92-A180-8B47D2592A5C}</MetaDataID>
        public UnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceAgent deletedStorageInstance, ObjectStorage objectStorage)
            : base(deletedStorageInstance)
        {
            ObjectStorage = objectStorage;

        }


        public override string Identity
        {
            get
            {
                return "unlinkall" + ObjectStorage.StorageIdentity + DeletedStorageInstance.MemoryID.ToString() + DeletedStorageInstance.ValueTypePath.ToString() + theAssociationEnd.Identity;
                return base.Identity;
            }
        }
        /// <MetaDataID>{4AD9BC6A-C5CF-4C35-A36A-1F079B6A7572}</MetaDataID>
        protected override PersistenceLayerRunTime.RelResolver theResolver
        {
            get
            {
                foreach (PersistenceLayerRunTime.RelResolver relResolcer in DeletedStorageInstance.RealStorageInstanceRef.RelResolvers)
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
            if (currentExecutionOrder < 40)
                return;
            if (!SubTransactionCmdsProduced)
            {
                if (DeletedStorageInstance.StorageIdentity == ObjectStorage.StorageIdentity)
                    base.GetSubCommands(currentExecutionOrder);

                RDBMSMetaDataRepository.Storage ObjectStorageMetadata = ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage;
                RDBMSMetaDataRepository.Association Association = ObjectStorageMetadata.GetEquivalentMetaObject(theAssociationEnd.Association) as RDBMSMetaDataRepository.Association;

                if (Association.LinkClass == null)
                {
                    foreach (RDBMSMetaDataRepository.StorageCellsLink currStorageCellsLink in Association.GetStorageCellsLinks(((RDBMSMetaDataRepository.StorageCell)DeletedStorageInstance.RealStorageInstanceRef.StorageInstanceSet)))
                    {

                        foreach (var table in (this.DeletedStorageInstance.RealStorageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MappedTables)
                        {
                            var columns = (Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(table);
                        }


                        RDBMSMetaDataRepository.AssociationEnd mAssociationEnd = ((Storage)DeletedStorageInstance.ObjectStorage.StorageMetaData).GetEquivalentMetaObject(theAssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                        UnlinkAllObjectOfStorageCellLinkCmd mUnlinkAllObjectOfStorageCellLinkCmd = new UnlinkAllObjectOfStorageCellLinkCmd(ObjectStorage, currStorageCellsLink, DeletedStorageInstance, mAssociationEnd);

                        PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                        transactionContext.EnlistCommand(mUnlinkAllObjectOfStorageCellLinkCmd);


                        RDBMSMetaDataRepository.StorageCellReference OutStorageCell = null;
                        if (currStorageCellsLink.RoleAStorageCell.GetType() == typeof(RDBMSMetaDataRepository.StorageCellReference))
                            OutStorageCell = currStorageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference;
                        if (currStorageCellsLink.RoleBStorageCell.GetType() == typeof(RDBMSMetaDataRepository.StorageCellReference))
                            OutStorageCell = currStorageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference;
                        if (OutStorageCell != null)
                        {

                            PersistenceLayerRunTime.ObjectStorage objectStorage = PersistenceLayerRunTime.ObjectStorage.OpenStorage(OutStorageCell.StorageName, OutStorageCell.StorageLocation, OutStorageCell.StorageType) as PersistenceLayerRunTime.ObjectStorage;//Error Prone το πρόγραμμα κάνει την παραδοχή ότι η άλλη στοραγε είναι mssql
                            objectStorage.CreateUnlinkAllObjectCommand(DeletedStorageInstance, mAssociationEnd, OutStorageCell.RealStorageCell);
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
