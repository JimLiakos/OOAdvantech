namespace OOAdvantech.PersistenceLayer
{
    /// <MetaDataID>{ee3f870c-1955-48fc-81f5-4e48eda01c5c}</MetaDataID>
    public interface IRawStorageData
    {

        /// <MetaDataID>{13e39958-0b54-45d5-a0cc-8dc3937d3f50}</MetaDataID>
        object RawData { get; set; }
        bool IsReadonly { get;}

        /// <MetaDataID>{e5ecf876-91ca-4cce-b2dc-0502e39d33ff}</MetaDataID>
        void SaveRawData();

        string StorageLocation { get; }
        
        string StorageName { get; }

    }
}