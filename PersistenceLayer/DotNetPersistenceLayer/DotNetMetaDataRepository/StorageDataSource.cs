namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{e26b1927-7064-4784-aee8-85e06c4f73a3}</MetaDataID>
    public class StorageDataSource : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataSource
    {
        internal override bool IsEmpty
        {
            get 
            {
                return StorageCells.Count == 0;
            }
        }
        public override bool AllObjectsInActiveMode
        {
            get
            {
                bool AllObjectsInActiveMode = true;
                foreach (MetaDataRepository.StorageCell storageCell in StorageCells)
                {
                    if (!storageCell.AllObjectsInActiveMode)
                    {
                        AllObjectsInActiveMode = false;
                        break;
                    }
                }
                return AllObjectsInActiveMode;
            }
        }
        /// <MetaDataID>{ff4d03c1-7f10-4480-b937-ecbbff53e46a}</MetaDataID>
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> StorageCells;
        /// <MetaDataID>{adf5bfaa-74a1-44ba-a4e8-291a86b55116}</MetaDataID>
        public StorageDataSource(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, Collections.Generic.Set<StorageCell> storageCells)
        {

            Collections.Generic.Dictionary<string, Collections.Generic.Set<StorageCell>> storagCellsDictionary = new Collections.Generic.Dictionary<string, OOAdvantech.Collections.Generic.Set<StorageCell>>();
            StorageCells = storageCells;

            foreach (StorageCell storageCell in storageCells)
            {
                if (!storagCellsDictionary.ContainsKey(storageCell.StorageIntentity))
                    storagCellsDictionary[storageCell.StorageIntentity] = new OOAdvantech.Collections.Generic.Set<StorageCell>();

                storagCellsDictionary[storageCell.StorageIntentity].Add(storageCell);

            }

            foreach (System.Collections.Generic.KeyValuePair<string, Collections.Generic.Set<StorageCell>> entry in storagCellsDictionary)
            {

                foreach (StorageCell storageCell in entry.Value)
                {
                    Storage storage = storageCell.Namespace as Storage;
                    //PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storage.StorageName, storage.StorageLocation, storage.StorageType);
                    DataLoader dataLoader = storage.CreateDataLoader(dataNode, entry.Value) as DataLoader; //ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage.DataLoader", "", dataNode, entry.Value, new Type[2] { typeof(MetaDataRepository.ObjectQueryLanguage.DataNode), typeof(Collections.Generic.Set<StorageCell>) }) as DataLoader;
                    DataLoaders[entry.Key] = dataLoader;
                    break;
                }
            }
            DataNode = dataNode;
        }

    }
}
