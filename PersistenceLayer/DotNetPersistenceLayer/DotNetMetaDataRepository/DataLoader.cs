namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{43275459-51FE-4F03-A5E0-4165FC9CF9F4}</MetaDataID>
    public class DataLoader
    {
        /// <MetaDataID>{455BE866-38C9-4F75-9081-2D2AE324AFDE}</MetaDataID>
        public Collections.Generic.Set<StorageCell> StorageCells=new OOAdvantech.Collections.Generic.Set<StorageCell>();

        public void AddStorageCell(StorageCell storageCell)
        {
            StorageCells.Add(storageCell);

        }
    }
}
