

using Microsoft.Azure.Cosmos.Table;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{a2789092-809a-42f4-b4a2-df740e641652}</MetaDataID>
    public class CloudBlockBlobArchive : PersistenceLayer.IBackupArchive
    {




        CloudStorageAccount CloudStorageAccount;
        string BlobUrl;
        public CloudBlockBlobArchive(string blobUrl, CloudStorageAccount cloudStorageAccount)
        {
            BlobUrl = blobUrl;
            CloudStorageAccount = cloudStorageAccount;
        }
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