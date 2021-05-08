namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{7946118c-a8b5-4efa-a8c7-cc56a20b9896}</MetaDataID>
    public class StorageMetaData
    {
        /// <MetaDataID>{a91f7b9f-dfb8-43ed-aa66-82a6f7e52aaa}</MetaDataID>
        public string NativeStorageID { get; set; }

        /// <MetaDataID>{20396d4b-d350-4306-b338-b33397004cba}</MetaDataID>
        public string StorageIdentity { get; set; }

        /// <MetaDataID>{7d6020cb-4f2a-4da9-9c99-656ff905d07e}</MetaDataID>
        public string StorageLocation { get; set; }

        /// <MetaDataID>{cf6d5324-9978-4179-b793-75cb214bd739}</MetaDataID>
        public string StorageType { get; set; }

        /// <MetaDataID>{714c7d54-f3c5-44ea-9786-7ad9fc0aea1a}</MetaDataID>
        public string StorageName { get; set; }

        /// <MetaDataID>{6fc10cce-7519-495f-b69e-78bff293807e}</MetaDataID>
        public bool MultipleObjectContext { get; set; }

        /// <MetaDataID>{50eeb441-1d30-4ea2-83bf-e594dd962b1d}</MetaDataID>
        public EndPoint[] ServerEndPoints { get; set; }
        public StorageMetaData()
        {
            ServerEndPoints = new EndPoint[0];
        }

    }
}