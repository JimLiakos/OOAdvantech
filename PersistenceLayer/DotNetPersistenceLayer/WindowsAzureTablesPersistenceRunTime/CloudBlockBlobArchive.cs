


namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{a2789092-809a-42f4-b4a2-df740e641652}</MetaDataID>
    public class CloudBlockBlobArchive : PersistenceLayer.IBackupArchive
    {



       // Azure.Storage.Blobs.BlobServiceClient BlobsAccount;
        //CloudStorageAccount CloudStorageAccount;
        string BlobUrl;
        //public CloudBlockBlobArchive(string blobUrl, Azure.Storage.Blobs.BlobServiceClient blobsAccount)
        //{
        //    BlobUrl = blobUrl;
        //    BlobsAccount = blobsAccount;
        //}
        public CloudBlockBlobArchive(string localFileName)
        {
            _LocalFileName = localFileName;
        }
         

        readonly string _LocalFileName;

        public string LocalFileName => _LocalFileName;

        public void Write(string blockID, byte[] dataChunk)
        {

            //throw new System.NotImplementedException();
        }
    }
}