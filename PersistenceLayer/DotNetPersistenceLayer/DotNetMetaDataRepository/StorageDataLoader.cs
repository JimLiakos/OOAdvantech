namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{a59cccbd-fbac-40b2-81d5-20fe12323a2b}</MetaDataID>
    public abstract class StorageDataLoader : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader
    {

        /// <MetaDataID>{25087e64-8435-4be3-b0ee-8c9ce3314016}</MetaDataID>
        public StorageDataLoader(DataNode dataNode, Collections.Generic.Set<StorageCell> storageCells) :base(dataNode)
        {
            if (storageCells.Count == 0)
                throw new System.Exception("The data loader must contains one or more storage cells.");
            StorageCells = new OOAdvantech.Collections.Generic.Set<StorageCell>(storageCells);
            
        }

    }
}
