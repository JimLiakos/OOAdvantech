namespace OOAdvantech.RDBMSPersistenceRunTime.Commands
{
	/// <MetaDataID>{51BB53E8-CA8D-4752-9D72-B57E62F5C48E}</MetaDataID>
	public class UpdateStorageSchema : PersistenceLayerRunTime.Commands.Command
	{
		/// <MetaDataID>{81471910-9BE5-4611-9317-EF6F6ED7D0AA}</MetaDataID>
        public PersistenceLayerRunTime.ObjectStorage UpdatingStorage;
		/// <summary>Priority defines the order in which will be executed the command.</summary>
		/// <MetaDataID>{0E14CD33-E430-480C-A8F7-2FEA0BE09A4D}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 15;
			}
		}
		/// <MetaDataID>{4F784CAE-5F7E-4934-9B4A-5C598FE82FEB}</MetaDataID>
		public override void GetSubCommands(int currentOrder)
		{
			
		}
        /// <MetaDataID>{5b19f637-2592-4ab8-be27-bf0dd949cdca}</MetaDataID>
        public void UpdateStorageCell(MetaDataRepository.StorageCell storageCell)
        {
            if (!StorageCells.Contains(storageCell))
                StorageCells.Add(storageCell);
        }

        /// <MetaDataID>{420afa16-033f-4b23-bf39-4b889168f9c9}</MetaDataID>
        public void UpdateStorageCellsLinks(MetaDataRepository.StorageCellsLink storageCellsLink)
        {
            if (!StorageCellsLinks.Contains(storageCellsLink))
                StorageCellsLinks.Add(storageCellsLink);
        }
        System.Collections.Generic.List<MetaDataRepository.StorageCell> StorageCells=new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell>();
        System.Collections.Generic.List<MetaDataRepository.StorageCellsLink> StorageCellsLinks=new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCellsLink>();

		/// <MetaDataID>{2F737F99-5BB2-4C57-A6D1-C33F897D5C7B}</MetaDataID>
		public UpdateStorageSchema(PersistenceLayerRunTime.ObjectStorage Storage)
		{
			 UpdatingStorage=Storage;
		
		}
		/// <MetaDataID>{7BFFED5B-5D81-422C-864E-8ABF2F4C340E}</MetaDataID>
		public override void Execute()
		{

			Storage aObjectStorage=UpdatingStorage.StorageMetaData as Storage;
            if (StorageCells.Count == 0 && StorageCellsLinks.Count == 0)
                aObjectStorage.UpdateSchema();
            else
                aObjectStorage.UpdateSchema(StorageCells, StorageCellsLinks);

		}

		public override string Identity
		{
			get
			{
				return "UpdateSchema"+UpdatingStorage.StorageMetaData.StorageIdentity;
			}
		}
        /// <MetaDataID>{b1c29b3c-72c1-4adf-88d6-22c0b3cd52c4}</MetaDataID>
		public static string GetIdentity(OOAdvantech.PersistenceLayerRunTime.ObjectStorage updatingStorage)
		{
			return "UpdateSchema"+updatingStorage.StorageMetaData.StorageIdentity;
		}
	}
}
