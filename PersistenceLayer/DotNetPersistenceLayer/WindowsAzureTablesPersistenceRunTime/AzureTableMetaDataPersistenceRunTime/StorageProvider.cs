using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Cosmos.Table;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.PersistenceLayerRunTime;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{3dc902aa-cda5-4694-8f5e-ad91a896bb47}</MetaDataID>
    public class StorageProvider : PersistenceLayerRunTime.StorageProvider
    {


        /// <MetaDataID>{5dbf7d44-f9d6-456d-9364-7f01d2703ba5}</MetaDataID>
        public StorageProvider()
        {

            //var cloudTableClient = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient();
            ////cloudTableClient.ListTables()
            //CloudTable table = cloudTableClient.GetTableReference("Person");
            //var query = new TableQuery<TableEntity>();


            //var entities = table.ExecuteQuery(query).ToList();
            //int trt = 0;

        }
        /// <MetaDataID>{da4edc90-7354-40e9-939a-7efd6b041ba8}</MetaDataID>
        public override Guid ProviderID
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{701dddf1-0a79-464a-a018-31cc88887df3}</MetaDataID>
        public override bool AllowEmbeddedStorage()
        {
            return true;
        }

        /// <MetaDataID>{a522ddc5-d679-4569-82a8-ee24c242871d}</MetaDataID>
        public override PersistenceLayer.Storage AttachStorage(string storageName, string storageLocation, string nativeStorageConnectionString)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{eb225c35-2734-4152-aa93-6910093b8ea4}</MetaDataID>
        public override PersistenceLayer.ObjectStorage CreateNewLogicalStorage(PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{86d28256-f366-4497-8eb0-bdd72bfc0fbb}</MetaDataID>
        public override void DeleteStorage(string storageName, string storageLocation)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{5b063015-c166-4523-b9f1-17da66a8efb2}</MetaDataID>
        public override string GetHostComuterName(string StorageName, string StorageLocation)
        {
#if DeviceDotNet
            return "default";
#else
            return System.Net.Dns.GetHostName();
#endif
        }

        /// <MetaDataID>{2915029c-6eaa-4dcc-a3f8-5168fe76300f}</MetaDataID>
        public override string GetInstanceName(string StorageName, string StorageLocation)
        {
            return "";
        }

        /// <MetaDataID>{eba3ccde-2d1e-434b-a386-4315fd9047f3}</MetaDataID>
        public override string GetNativeStorageID(string storageDataLocation)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{10baea32-a6df-4aa0-a464-848b8a8b3d5a}</MetaDataID>
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }

        /// <MetaDataID>{3e2a5c54-cab8-46f6-873f-f6672e353ed5}</MetaDataID>
        public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string StorageName, object rawStorageData)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{e35ca566-6809-441e-a7aa-c3834a4f700b}</MetaDataID>
        public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string storageName, string storageLocation, string userName = "", string password = "")
        {
            CloudStorageAccount account = null;

            if (storageLocation.ToLower() == @"DevStorage".ToLower() && string.IsNullOrWhiteSpace(userName))
                account = CloudStorageAccount.DevelopmentStorageAccount;
            else
                account = new CloudStorageAccount(new StorageCredentials(userName, password), true);
            ObjectStorage objectStorage = new ObjectStorage(storageName, storageLocation, true, account);

            return objectStorage;
        }

        /// <MetaDataID>{cae7c13e-65e1-4bf0-88ff-7c599591530b}</MetaDataID>
        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{d12ddd26-dbf7-43df-868e-020d53f29b53}</MetaDataID>
        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string storageName, string storageLocation, string userName = "", string password = "")
        {

            //string connectionString = @"DefaultEndpointsProtocol=http;AccountName=asfameazure;AccountKey=pJL6v5+z9tRTOxpg/tzuh71j19s/16rKMiPSlTyLmJdqkIrdms/EV5ZO/ptz8ZCQYNaOC7Kba+gtQl8X1qVZ7g==";
            //var sdsd= CloudStorageAccount.Parse(connectionString);

            CloudStorageAccount account = null;
            if (storageLocation.ToLower() == @"DevStorage".ToLower() && string.IsNullOrWhiteSpace(userName))
                account = CloudStorageAccount.DevelopmentStorageAccount;
            else
                account = new CloudStorageAccount(new StorageCredentials(userName, password), true);
            ObjectStorage objectStorage = new ObjectStorage(storageName, storageLocation, false, account);

            return objectStorage;
        }

        public override void Repair(string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password, bool overrideObjectStorage )
        {
            throw new NotImplementedException();
        }
    }
}
