using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.PersistenceLayerRunTime;
using OOAdvantech.Transactions;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{3dc902aa-cda5-4694-8f5e-ad91a896bb47}</MetaDataID>
    public class StorageProvider : PersistenceLayerRunTime.StorageProvider
    {
        // static Dictionary<string, StorageCredentials> ImplicitOpenStorageCredentials = new Dictionary<string, StorageCredentials>();
        static Dictionary<string, Azure.Data.Tables.TableSharedKeyCredential> ImplicitOpenStorageCredentials = new Dictionary<string, Azure.Data.Tables.TableSharedKeyCredential>();
        //Set Implicit Open Storage Credentials
        //public static void SetImplicitOpenStorageCredentials(string storageLocation, StorageCredentials storageCredentials)
        //{
        //    ImplicitOpenStorageCredentials[storageLocation] = storageCredentials;
        //}
        //Set Implicit Open Storage Credentials
        public static void SetImplicitOpenStorageCredentials(string storageLocation, Azure.Data.Tables.TableSharedKeyCredential storageCredentials)
        {
            ImplicitOpenStorageCredentials[storageLocation] = storageCredentials;
        }
        public StorageProvider()
        {


            //var cloudTableClient = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient();
            ////cloudTableClient.ListTables()
            //CloudTable table = cloudTableClient.GetTableReference("Person");
            //var query = new TableQuery<TableEntity>();


            //var entities = table.ExecuteQuery(query).ToList();
            //int trt = 0;

        }
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

        public override bool AllowEmbeddedStorage()
        {
            return true;
        }

        public override PersistenceLayer.Storage AttachStorage(string storageName, string storageLocation, string nativeStorageConnectionString)
        {
            throw new NotImplementedException();
        }

        public override PersistenceLayer.ObjectStorage CreateNewLogicalStorage(PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName)
        {
            throw new NotImplementedException();
        }

        public override void DeleteStorage(string storageName, string storageLocation)
        {
            throw new NotImplementedException();
        }

        public override string GetHostComuterName(string StorageName, string StorageLocation)
        {
#if DeviceDotNet
            return "default";
#else
            return System.Net.Dns.GetHostName();
#endif
        }

        public override string GetInstanceName(string StorageName, string StorageLocation)
        {
            return "";
        }

        public override string GetNativeStorageID(string storageDataLocation)
        {
            throw new NotImplementedException();
        }

        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }

        public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string StorageName, object rawStorageData)
        {
            throw new NotImplementedException();
        }

        //CloudStorageAccount Account;
        public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string StorageName, string storageLocation, string userName = "", string password = "")
        {
            //CloudStorageAccount account = null;

            //if (StorageLocation.ToLower() == @"DevStorage".ToLower() && (string.IsNullOrWhiteSpace(userName) || userName == "devstoreaccount1"))
            //    account = CloudStorageAccount.DevelopmentStorageAccount;
            //else
            //    account = new CloudStorageAccount(new StorageCredentials(userName, password), true);
            //Account = account;
            // Azure.Data.Tables.TableServiceClient tablesAccount = new Azure.Data.Tables.TableServiceClient(account.ToString(true));
            Azure.Data.Tables.TableServiceClient tablesAccount = null;
            Azure.Data.Tables.TableSharedKeyCredential azureTablesCredential = null;
            if (storageLocation.ToLower() == @"DevStorage".ToLower() && (string.IsNullOrWhiteSpace(userName) || userName == "devstoreaccount1"))
                tablesAccount = new Azure.Data.Tables.TableServiceClient(Storage.DevelopmentStorage);
            else
            {
                Uri endPoint = new Uri(string.Format("https://{0}.table.core.windows.net", storageLocation));
                if (string.IsNullOrWhiteSpace(userName) && OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider.ImplicitOpenStorageCredentials.ContainsKey(storageLocation))
                    azureTablesCredential = StorageProvider.ImplicitOpenStorageCredentials[storageLocation];
                else
                    azureTablesCredential = new Azure.Data.Tables.TableSharedKeyCredential(userName, password);

                tablesAccount = new Azure.Data.Tables.TableServiceClient(endPoint, azureTablesCredential);
            }

            //CloudTableClient tableClient = account.CreateCloudTableClient();
            //CloudTable table = tableClient.GetTableReference("StoragesMetadata");
            //if (!table.Exists())
            //    table.CreateIfNotExists();
            Azure.Data.Tables.TableClient storagesMetadataTable_a = tablesAccount.GetTableClient("StoragesMetadata");

            Azure.Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", "StoragesMetadata"));
            bool storagesMetadataTable_exist = queryTableResults.Count() > 0;

            if (!storagesMetadataTable_exist)
                storagesMetadataTable_a.CreateIfNotExists();


            StorageMetadata storageMetadata = (from storageMetada in storagesMetadataTable_a.Query<StorageMetadata>()
                                               where storageMetada.StorageName == StorageName //&& storageMetada.UnderConstruction == false
                                               select storageMetada).FirstOrDefault();


            if (storageMetadata == null)
            {
                storageMetadata = new StorageMetadata("AAA", Guid.NewGuid().ToString());
                storageMetadata.StorageName = StorageName;
                storageMetadata.StoragePrefix = StorageName;

                //TableOperation insertOperation = TableOperation.Insert(storageMetadata);
                //var result = table.Execute(insertOperation);
                storagesMetadataTable_a.AddEntity(storageMetadata);
            }
            else
            {
                while (storageMetadata.UnderConstruction)
                {
                    storagesMetadataTable_a.UpdateEntity(storageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
                    System.Threading.Thread.Sleep(100);
                    storageMetadata = (from storageMetada in storagesMetadataTable_a.Query<StorageMetadata>()
                                       where storageMetada.StorageName == StorageName //&& storageMetada.UnderConstruction == false
                                       select storageMetada).FirstOrDefault();
                }
                PersistenceLayer.ObjectStorage existinObjectStorage = null;
                try
                {
                    existinObjectStorage = OpenStorage(StorageName, storageLocation, userName, password);

                }
                catch (OOAdvantech.PersistenceLayer.StorageException Error)
                {
                }
                catch (System.Exception Error)
                {
                }

                if (existinObjectStorage != null)
                    throw new OOAdvantech.PersistenceLayer.StorageException(string.Format("Storage with name '{0}' already exist", StorageName), PersistenceLayer.StorageException.ExceptionReason.StorageAlreadyExist);
            }

            PersistenceLayer.ObjectStorage metaDataStorage = PersistenceLayer.ObjectStorage.NewStorage(StorageName, storageLocation,
                         "OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.StorageProvider", true);
            string assemblyFullName = typeof(OOAdvantech.WindowsAzureTablesPersistenceRunTime.Storage).Assembly.FullName;
            metaDataStorage.StorageMetaData.RegisterComponent(assemblyFullName);

            Storage storage = new Storage(StorageName, storageLocation, this.GetType().FullName, storageMetadata);
            metaDataStorage.CommitTransientObjectState(storage);

            ObjectStorage objectStorage = new ObjectStorage(storage, tablesAccount,userName, password);

            {
                storageMetadata.UnderConstruction = false;
                storageMetadata.StorageIdentity = objectStorage.StorageIdentity;
                //TableOperation replaceOperation = TableOperation.Replace(storageMetadata);
                //var result = table.Execute(replaceOperation);

                storagesMetadataTable_a.UpdateEntity(storageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
            }

            return objectStorage;

        }
        static protected string OpenStorageLock = "OpenStorageLock";

        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            throw new NotImplementedException();
        }



        static protected System.Collections.Generic.Dictionary<string, ObjectStorage> OpenStorages = new System.Collections.Generic.Dictionary<string, ObjectStorage>();
        public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string storageName, string storageLocation, string userName = "", string password = "")
        {
            lock (OpenStorageLock)
            {
                ObjectStorage objectStorage = null;
                if (OpenStorages.TryGetValue(storageName.ToLower(), out objectStorage))
                    return objectStorage;

                Azure.Data.Tables.TableServiceClient tablesAccount = null;

                if (storageLocation.ToLower() == @"DevStorage".ToLower() && (string.IsNullOrWhiteSpace(userName) || userName == "devstoreaccount1"))
                    tablesAccount = new Azure.Data.Tables.TableServiceClient(Storage.DevelopmentStorage);
                else
                {
                    Uri endPoint = new Uri(string.Format("https://{0}.table.core.windows.net", storageLocation));
                    if (string.IsNullOrWhiteSpace(userName) && OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider.ImplicitOpenStorageCredentials.ContainsKey(storageLocation))
                        tablesAccount = new Azure.Data.Tables.TableServiceClient(endPoint, StorageProvider.ImplicitOpenStorageCredentials[storageLocation]);
                    else
                        tablesAccount = new Azure.Data.Tables.TableServiceClient(endPoint, new Azure.Data.Tables.TableSharedKeyCredential(userName, password));
                }


                //CloudStorageAccount account = null;
                //if (storageLocation.ToLower() == @"DevStorage".ToLower() && (string.IsNullOrWhiteSpace(userName) || userName == "devstoreaccount1"))
                //    account = CloudStorageAccount.DevelopmentStorageAccount;
                //else
                //{
                //    if (string.IsNullOrWhiteSpace(userName) && OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider.ImplicitOpenStorageCredentials.ContainsKey(storageLocation))
                //        account = new CloudStorageAccount(OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider.ImplicitOpenStorageCredentials[storageLocation], true);
                //    else
                //        account = new CloudStorageAccount(new StorageCredentials(userName, password), true);
                //}
                //CloudTableClient tableClient = account.CreateCloudTableClient();

                //CloudTable storagesMetadataTable = tableClient.GetTableReference("StoragesMetadata");

                Azure.Data.Tables.TableClient storagesMetadataTable_a = tablesAccount.GetTableClient("StoragesMetadata");

                Azure.Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", "StoragesMetadata"));
                bool storagesMetadataTable_exist = queryTableResults.Count() > 0;



                if (storagesMetadataTable_exist)
                {

                    StorageMetadata storageMetadata = (from storageMetada in storagesMetadataTable_a.Query<StorageMetadata>()
                                                       where storageMetada.StorageName == storageName
                                                       select storageMetada).FirstOrDefault();

                    if (storageMetadata == null)
                        throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + storageName + " at location " + storageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist, null);

                    while (storageMetadata.UnderConstruction)
                    {
                        System.Threading.Thread.Sleep(100);
                        storageMetadata = (from storageMetada in storagesMetadataTable_a.Query<StorageMetadata>()
                                           where storageMetada.StorageName == storageName
                                           select storageMetada).FirstOrDefault();
                    }
                    //global::Azure.Data.Tables.TableServiceClient tablesAccount = new Azure.Data.Tables.TableServiceClient(account.ToString(true));
                    objectStorage = OpenStorage(storageName, storageLocation, tablesAccount,userName,password, storageMetadata);


                    if (string.IsNullOrWhiteSpace(storageMetadata.StorageIdentity))
                    {
                        storageMetadata.StorageIdentity = objectStorage.StorageIdentity;
                        storagesMetadataTable_a.UpdateEntity(storageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
                        //TableOperation insertOperation = TableOperation.Replace(storageMetadata);
                        //var result = storagesMetadataTable.Execute(insertOperation);
                    }


                    return objectStorage;

                }
                else
                {
                    throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + storageName + " at location " + storageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist, null);
                }


                //CloudStorageAccount account = OpenStorageAccount();
                //return null;
            }
        }

        private ObjectStorage OpenStorage(string storageName, string StorageLocation, Azure.Data.Tables.TableServiceClient tablesAcount, string userName, string pasword, StorageMetadata storageMetadata)
        {
            ObjectStorage objectStorage = null;
            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                Storage storage = null;
                PersistenceLayer.ObjectStorage metaDataObjectStorage = new AzureTableMetaDataPersistenceRunTime.ObjectStorage(storageName, StorageLocation, false, tablesAcount, storageMetadata);

                //PersistenceLayer.ObjectStorage.OpenStorage(storageName, StorageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.StorageProvider");

                Collections.StructureSet aStructureSet = metaDataObjectStorage.Execute("SELECT ObjectStorage FROM " + typeof(Storage).FullName + " ObjectStorage ");
                storage = null;
                foreach (Collections.StructureSet Rowset in aStructureSet)
                {
                    storage = (Storage)Rowset["ObjectStorage"];
                    break;
                }


                try
                {
                    var tabeClient = tablesAcount.GetTableClient("AuthUserRefCloudTable");
                    var ddd = tabeClient.Query<Azure.Data.Tables.TableEntity>().ToList();

                }
                catch (Exception error)
                {


                }
                if (storage == null)
                    throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + storageName + " at location " + StorageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist);

                storage.AzureStorageMetadata = storageMetadata;

                objectStorage = new ObjectStorage(storage, tablesAcount,userName,pasword);
                InitializeDataPartioningMetaData(objectStorage, storage);
                objectStorage.ActivateObjectsInPreoadedStorageCells();
                stateTransition.Consistent = true;
                if (objectStorage != null)
                    OpenStorages[storageName.ToLower()] = objectStorage;

                storage.StorageLocation = StorageLocation;


                return objectStorage;
            }
        }

        private void InitializeDataPartioningMetaData(ObjectStorage objectStorage, Storage storage)
        {

        }
        static byte[] Buffer = new byte[32768];
        public override void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password, bool overrideObjectStorage)
        {
            System.IO.Stream memoryStream = System.IO.File.Open(archive.LocalFileName, FileMode.Open, FileAccess.Read);

            try
            {
                #region Reads header type and must be 0 for backup storage meta data
                memoryStream.Position = 0;
                memoryStream.Read(Buffer, 0, 1);
                int headerType = Buffer[0];
                #endregion

                #region Reads storage meta data  bytes length
                memoryStream.Read(Buffer, 0, 4);
                int streamLength = BitConverter.ToInt32(Buffer, 0);
                memoryStream.Read(Buffer, 0, streamLength);
                #endregion


                int offset = 0;
                string storageMetaDataXml = BinaryFormatter.BinaryFormatter.ToString(Buffer, offset, ref offset);

                #region Gets backup meta data like storage name etc.
                XDocument storageMetaDataDoc = XDocument.Parse(storageMetaDataXml);

                string backupStorageName = storageMetaDataDoc.Root.Attribute("StorageName").Value;
                string backupStorageLocation = storageMetaDataDoc.Root.Attribute("StorageLocation").Value;
                string backupStorageType = storageMetaDataDoc.Root.Attribute("StorageType").Value;
                string backupTablePrefix = storageMetaDataDoc.Root.Attribute("TablePrefix").Value;
                string backupStoragePrefix = storageMetaDataDoc.Root.Attribute("StoragePrefix").Value;
                string backupStorageIdentity = storageMetaDataDoc.Root.Attribute("StorageIdentity").Value;
                #endregion

                if (string.IsNullOrWhiteSpace(storageName))
                    storageName = backupStorageName;


                //CloudStorageAccount account = null;
                //if (storageLocation.ToLower() == @"DevStorage".ToLower() && (string.IsNullOrWhiteSpace(userName) || userName == "devstoreaccount1"))
                //    account = CloudStorageAccount.DevelopmentStorageAccount;
                //else
                //    account = new CloudStorageAccount(new StorageCredentials(userName, password), true);
                //CloudTableClient cloudTablesClient = account.CreateCloudTableClient();


                Azure.Data.Tables.TableServiceClient tablesAccount = null;
                if (storageLocation.ToLower() == @"DevStorage".ToLower() && (string.IsNullOrWhiteSpace(userName) || userName == "devstoreaccount1"))
                    tablesAccount = new Azure.Data.Tables.TableServiceClient(Storage.DevelopmentStorage);
                else
                {
                    Uri endPoint = new Uri(string.Format("https://{0}.table.core.windows.net", storageLocation));
                    tablesAccount = new Azure.Data.Tables.TableServiceClient(endPoint, new Azure.Data.Tables.TableSharedKeyCredential(userName, password));
                }

            
                //CloudTable storagesMetadataTable = cloudTablesClient.GetTableReference("StoragesMetadata");
                Azure.Data.Tables.TableClient storagesMetadataTable_a = tablesAccount.GetTableClient("StoragesMetadata");

                Azure.Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", "StoragesMetadata"));
                bool storagesMetadataTable_exist = queryTableResults.Count() > 0;

                if (!storagesMetadataTable_exist)
                    storagesMetadataTable_a.CreateIfNotExists();


                //if (!storagesMetadataTable.Exists())
                //    storagesMetadataTable.CreateIfNotExists();

                StorageMetadata storageMetadata = (from storageMetada in storagesMetadataTable_a.Query<StorageMetadata>()
                                                   where storageMetada.StorageName == storageName && storageMetada.StorageIdentity == backupStorageIdentity
                                                   select storageMetada).FirstOrDefault();

                StorageMetadata otherStorageWithSamePrefix = (from storageMetada in storagesMetadataTable_a.Query<StorageMetadata>()
                                                              where storageMetada.StoragePrefix == storageName && storageMetada.StorageIdentity != backupStorageIdentity
                                                              select storageMetada).FirstOrDefault();

                StorageMetadata otherStorageWithSameName = (from storageMetada in storagesMetadataTable_a.Query<StorageMetadata>()
                                                            where storageMetada.StorageName == storageName && storageMetada.StorageIdentity != backupStorageIdentity
                                                            select storageMetada).FirstOrDefault();


                if (otherStorageWithSameName != null)
                    throw new OOAdvantech.PersistenceLayer.StorageException(string.Format("Storage with name '{0}' already exist", storageName), PersistenceLayer.StorageException.ExceptionReason.StorageAlreadyExist);

                if (otherStorageWithSamePrefix != null)
                    throw new OOAdvantech.PersistenceLayer.StorageException(string.Format("Storage with storage prefix '{0}' already exist", storageName), PersistenceLayer.StorageException.ExceptionReason.Unknown);

                string storagePrefix = null;
                string replacedDataStoragePrefix = null;
                List<string> replacedDataTables = new List<string>();
                if (storageMetadata == null)
                {
                    #region Creates storage meta data entry.

                    storageMetadata = new StorageMetadata("AAA", Guid.NewGuid().ToString());
                    storageMetadata.StorageName = storageName;

                    string uniqueStoragePrefix = MakeStoragePrefixUnique(storagesMetadataTable_a, storageName);

                    storageMetadata.StoragePrefix = uniqueStoragePrefix;
                    storageMetadata.StorageIdentity = backupStorageIdentity;
                    storagesMetadataTable_a.AddEntity(storageMetadata);
                    storagePrefix = storageMetadata.StoragePrefix;

                    #endregion
                }
                else
                {
                    #region provides new storage prefix for restore and change object storage state to under construction;

                    string uniqueStoragePrefix = MakeStoragePrefixUnique(storagesMetadataTable_a, storageMetadata.StorageName);
                    if (overrideObjectStorage)
                        storagePrefix = storageMetadata.StoragePrefix;
                    else
                        storagePrefix = uniqueStoragePrefix;


                    replacedDataStoragePrefix = storageMetadata.StoragePrefix;

                    try
                    {
                        var replacedDataStorage = OpenStorage(storageMetadata.StorageName, storageLocation, tablesAccount, userName, password, storageMetadata) as ObjectStorage;//.StorageMetaData as Storage;
                        replacedDataTables = replacedDataStorage.GetAzureStorageDataTablesNames();

                    }
                    catch (Exception errors)
                    {
                        //Catches errors in case where the storage did not build correctly
                    }

                    lock (OpenStorageLock)
                    {
                        OpenStorages.Remove(storageMetadata.StorageName);
                    }
                    storageMetadata.UnderConstruction = true;
                    storagesMetadataTable_a.UpdateEntity(storageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);

                    #endregion
                }


                RestoredStorageManager restoredStorageManager = new RestoredStorageManager(storageName, storageLocation, storagePrefix, backupStoragePrefix, backupTablePrefix, replacedDataStoragePrefix, replacedDataTables);

                string tableName = null;
                List<Azure.Data.Tables.TableEntity> deserializedEntities = null;

                int testI = 0;
                while (memoryStream.Position < memoryStream.Length)
                {
                    ObjectStorage.DeserializeTable(memoryStream, out tableName, out deserializedEntities);
                    tableName = restoredStorageManager.ResolveTableName(tableName);
                    storageMetadata.AddTemporaryTable(tableName);
                    storagesMetadataTable_a.UpdateEntity(storageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);

                    TransferTableRecords(tableName, deserializedEntities, tablesAccount);

                    //testI++;

                    //if (testI > 5)
                    //    throw new Exception("Test Exception");
                }

                storageMetadata.StoragePrefix = storagePrefix;
                ObjectStorage restoredObjectStorage = OpenStorage(storageName, storageLocation, tablesAccount, userName, password, storageMetadata);

                restoredStorageManager.UpdateStorageMetaData(restoredObjectStorage);

                lock (OpenStorageLock)
                {
                    OpenStorages[storageName.ToLower()] = restoredObjectStorage;
                }
                {

                    storageMetadata.UnderConstruction = false;
                    storageMetadata.StoragePrefix = storagePrefix;

                    storageMetadata.ClearTemporaryTables();
                    storageMetadata.AddTemporaryTables(restoredStorageManager.GetReplacedTablesNames());

                    //storagesMetadataTable.UpdateEntity(storageMetadata);
                    storagesMetadataTable_a.UpdateEntity(storageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
                }

                //foreach (string replacesDataTableName in restoredStorageManager.GetReplacedTablesNames())
                //{
                //    var cloudTable = tableClient.GetTableReference(replacesDataTableName);
                //    cloudTable.DeleteIfExists();
                //}
                //{
                //    storageMetadata.RemoveTemporaryTables(storageMetadata.GetTemporaryTablesNames());
                //    TableOperation insertOperation = TableOperation.Replace(storageMetadata);
                //    var result = storagesMetadataTable.Execute(insertOperation);
                //}
            }
            finally
            {
                memoryStream.Close();
            }
        }

        /// <summary>
        /// Look for unique storage in azure storage context
        /// </summary>
        /// <param name="storagesMetadataTable">
        /// Defines the cloud table which contains the storages metadata
        /// </param>
        /// <param name="storagePrefix">
        /// Defines the storage prefix as template for unique prefix 
        /// </param>
        /// <returns>
        /// return the unique storage prefix
        /// </returns>
        private static string MakeStoragePrefixUnique(Azure.Data.Tables.TableClient storagesMetadataTable, string storagePrefix)
        {
            int count = 0;
            StorageMetadata storageMetadataWithStoragePrefix = (from storageMetadataEntry in storagesMetadataTable.Query<StorageMetadata>()
                                                                where storageMetadataEntry.StoragePrefix == storagePrefix
                                                                select storageMetadataEntry).FirstOrDefault();
            while (storageMetadataWithStoragePrefix != null)
            {
                count++;
                storageMetadataWithStoragePrefix = (from storageMetadataEntry in storagesMetadataTable.Query<StorageMetadata>()
                                                    where storageMetadataEntry.StoragePrefix == storagePrefix + count.ToString()
                                                    select storageMetadataEntry).FirstOrDefault();
                storagePrefix = storagePrefix + count.ToString();
            }

            return storagePrefix;

        }

        internal void TransferTableRecords(string cloudTableName, List<Azure.Data.Tables.TableEntity> tableEntities, Azure.Data.Tables.TableServiceClient tablesAccount)
        {

            try
            {
                //CloudTableClient tableClient = account.CreateCloudTableClient();
                //CloudTable azureTable = tableClient.GetTableReference(cloudTableName);
                //if (!azureTable.Exists())
                //{
                //    azureTable = tableClient.GetTableReference(cloudTableName);
                //    azureTable.CreateIfNotExists();
                //}
                //else
                //{
                //    azureTable = tableClient.GetTableReference(cloudTableName);
                //    azureTable.DeleteIfExists();
                //    azureTable.CreateIfNotExists();

                //}


                var azureTable_a = tablesAccount.GetTableClient(cloudTableName);
                Azure.Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", cloudTableName));
                bool azureTable_exist = queryTableResults.Count() > 0;

                if (!azureTable_exist)
                {
                    azureTable_a.CreateIfNotExists();
                }
                else
                {
                    azureTable_a.Delete();

                    do
                    {
                        System.Threading.Thread.Sleep(100);
                        queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", cloudTableName));
                        azureTable_exist = queryTableResults.Count() > 0;

                    } while (azureTable_exist);
                    azureTable_a.CreateIfNotExists();
                }


                List<List<Azure.Data.Tables.TableTransactionAction>> tableBatchOperations = new List<List<Azure.Data.Tables.TableTransactionAction>>();

                foreach (var entity in tableEntities)
                {
                    if (tableBatchOperations.Count == 0 || tableBatchOperations.Last().Count == 100)
                        tableBatchOperations.Add(new List<Azure.Data.Tables.TableTransactionAction>());


                    tableBatchOperations.Last().Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.Add, entity));
                }

                foreach (var tableBatchOperation in tableBatchOperations)
                    azureTable_a.SubmitTransaction(tableBatchOperation);

            }
            catch (Exception error)
            {
                throw;
            }
        }



        class RestoredStorageManager
        {
            string BackupTablePrefix;
            string ReplacedDataTablePrefix;
            string TablePrefix;
            string ClassBLOBDataTableName;
            string ObjectBLOBDataTableName;
            string MetadataIdentityTableName;


            string BackupClassBLOBDataTableName;
            string BackupObjectBLOBDataTableName;
            string BackupMetadataIdentityTableName;

            string ReplacedDataClassBLOBDataTableName;
            string ReplacedDataObjectBLOBDataTableName;
            string ReplacedDataMetadataIdentityTableName;



            string StoragePrefix;
            string BackupStoragePrefix;
            string StorageName;
            string StorageLocation;
            string ReplacedDataStoragePrefix;
            List<string> ReplacedDataTablesNames;


            public RestoredStorageManager(string storageName, string storageLocation, string storagePrefix, string backupStoragePrefix, string backupTablePrefix, string replacedDataStoragePrefix, List<string> replacedDataTables)
            {
                StorageName = storageName;
                StorageLocation = storageLocation;
                ReplacedDataTablesNames = replacedDataTables;

                ReplacedDataStoragePrefix = replacedDataStoragePrefix;
                StoragePrefix = storagePrefix;
                BackupStoragePrefix = backupStoragePrefix;

                TablePrefix = Storage.GetTablePrefixFor(storagePrefix);
                ClassBLOBDataTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetClassBLOBDataTableNameFor(storagePrefix);
                ObjectBLOBDataTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetObjectBLOBDataTableNameFor(storagePrefix);
                MetadataIdentityTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetMetadataIdentityTableNameFro(storagePrefix);

                BackupTablePrefix = backupTablePrefix;
                BackupClassBLOBDataTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetClassBLOBDataTableNameFor(backupStoragePrefix);
                BackupObjectBLOBDataTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetObjectBLOBDataTableNameFor(backupStoragePrefix);
                BackupMetadataIdentityTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetMetadataIdentityTableNameFro(backupStoragePrefix);

                if (!string.IsNullOrWhiteSpace(ReplacedDataStoragePrefix))
                {
                    ReplacedDataTablePrefix = Storage.GetTablePrefixFor(ReplacedDataStoragePrefix);
                    ReplacedDataClassBLOBDataTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetClassBLOBDataTableNameFor(replacedDataStoragePrefix);
                    ReplacedDataObjectBLOBDataTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetObjectBLOBDataTableNameFor(replacedDataStoragePrefix);
                    ReplacedDataMetadataIdentityTableName = AzureTableMetaDataPersistenceRunTime.Storage.GetMetadataIdentityTableNameFro(replacedDataStoragePrefix);
                }


            }

            internal string ResolveTableName(string tableName)
            {
                if (StoragePrefix != BackupStoragePrefix)
                {
                    if (tableName == BackupClassBLOBDataTableName && BackupClassBLOBDataTableName != ClassBLOBDataTableName)
                        return ClassBLOBDataTableName;
                    else if (tableName == BackupObjectBLOBDataTableName && BackupObjectBLOBDataTableName != ObjectBLOBDataTableName)
                        return ObjectBLOBDataTableName;
                    else if (tableName == BackupMetadataIdentityTableName && BackupMetadataIdentityTableName != MetadataIdentityTableName)
                        return MetadataIdentityTableName;
                    else
                    {
                        if (tableName.IndexOf(BackupTablePrefix) != 0)
                            throw new InvalidDataException("Table prefix mismatch");
                        return TablePrefix + tableName.Substring(BackupTablePrefix.Length);
                    }
                }
                else
                    return tableName;
            }
            internal List<string> GetReplacedTablesNames()
            {
                List<string> replacedTablesNames = new List<string>();
                if (!string.IsNullOrWhiteSpace(ReplacedDataStoragePrefix))
                {
                    replacedTablesNames.Add(ReplacedDataClassBLOBDataTableName);
                    replacedTablesNames.Add(ReplacedDataObjectBLOBDataTableName);
                    replacedTablesNames.Add(ReplacedDataMetadataIdentityTableName);
                    replacedTablesNames.AddRange(ReplacedDataTablesNames);
                }
                return replacedTablesNames;

            }
            internal void UpdateStorageMetaData(PersistenceLayer.ObjectStorage objectStorage)
            {
                if (StoragePrefix != BackupStoragePrefix)
                {
                    #region Changes restored table metadata name

                    Storage storage = objectStorage.StorageMetaData as Storage;
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        storage.SetStorageName(StorageName);
                        foreach (var tableMetaData in (storage as MetaDataRepository.Namespace).OwnedElements.OfType<RDBMSMetaDataRepository.Table>())
                        {
                            tableMetaData.Name = TablePrefix + tableMetaData.Name.Substring(BackupTablePrefix.Length);
                            tableMetaData.DataBaseTableName = tableMetaData.Name;
                        }
                        stateTransition.Consistent = true;
                    }

                    #endregion

                }
            }
        }


        public override void Repair(string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password)
        {
            //CloudStorageAccount account = null;
            //if (storageLocation.ToLower() == @"DevStorage".ToLower() && (string.IsNullOrWhiteSpace(userName) || userName == "devstoreaccount1"))
            //    account = CloudStorageAccount.DevelopmentStorageAccount;
            //else
            //    account = new CloudStorageAccount(new StorageCredentials(userName, password), true);

            // Azure.Data.Tables.TableServiceClient tablesAcount = new Azure.Data.Tables.TableServiceClient(account.ToString(true));

            Azure.Data.Tables.TableServiceClient tablesAccount = null;
            if (storageLocation.ToLower() == @"DevStorage".ToLower() && (string.IsNullOrWhiteSpace(userName) || userName == "devstoreaccount1"))
                tablesAccount = new Azure.Data.Tables.TableServiceClient(OOAdvantech.WindowsAzureTablesPersistenceRunTime.Storage.DevelopmentStorage);
            else
            {
                Uri endPoint = new Uri(string.Format("https://{0}.table.core.windows.net", storageLocation));
                tablesAccount = new Azure.Data.Tables.TableServiceClient(endPoint, new Azure.Data.Tables.TableSharedKeyCredential(userName, password));
            }
            //CloudTableClient tableClient = account.CreateCloudTableClient();
            //CloudTable storagesMetadataTable = tableClient.GetTableReference("StoragesMetadata");
            Azure.Data.Tables.TableClient storagesMetadataTable_a = tablesAccount.GetTableClient("StoragesMetadata");

            //storagesMetadataTable_a.CreateIfNotExists(); 
            //if (!storagesMetadataTable.Exists())
            //    storagesMetadataTable.CreateIfNotExists();

            StorageMetadata storageMetadata = (from storageMetada in storagesMetadataTable_a.Query<StorageMetadata>()
                                               where storageMetada.StorageName == storageName
                                               select storageMetada).FirstOrDefault();

            var objectStorage = OpenStorage(storageMetadata.StorageName, storageLocation, tablesAccount, userName, password, storageMetadata) as ObjectStorage;//.StorageMetaData as Storage;

            var azureStorageDataTablesNames = objectStorage.GetAzureStorageDataTablesNames();

            if (storageMetadata.UnderConstruction)
            {
                storageMetadata.UnderConstruction = false;
                //storagesMetadataTable.UpdateEntity(storageMetadata);
                storagesMetadataTable_a.UpdateEntity(storageMetadata, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
            }

        }


    }



    //if(tableBatchOperation!=null)
    //    azureTable.ExecuteBatch(tableBatchOperation);
}

