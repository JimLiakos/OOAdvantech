using Azure;
using Azure.Data.Tables;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using static OOAdvantech.RDBMSMetaDataRepository.StoreProcedure;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{ecdb7768-48a2-43e3-b21b-636f2b1ea537}</MetaDataID>
    public class StorageMetadata : Azure.Data.Tables.ITableEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageMetadata"/> class.
        /// Your entity type must expose a parameter-less constructor
        /// </summary>
        public StorageMetadata()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageMetadata"/> class.
        /// Defines the PK and RK.
        /// </summary>
        /// <param name="partitionKey">The last name.</param>
        /// <param name="rowKey">The first name.</param>
        public StorageMetadata(string partitionKey, string rowKey)
        {
            UnderConstruction = true;
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }




        public string StorageName { get; set; }

        public string StorageIdentity { get; set; }


        public string StoragePrefix { get; set; }

        public bool UnderConstruction { get; set; }


        public string TemporaryTables { get; set; }
        DateTimeOffset? ITableEntity.Timestamp { get; set; }
        public ETag ETag { get; set; }

        public void AddTemporaryTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Invalid tableName");

            if (!string.IsNullOrWhiteSpace(TemporaryTables))
                TemporaryTables += ";";
            TemporaryTables += tableName;
        }

        public void AddTemporaryTables(List<string> tableNames)
        {
            foreach (string tableName in tableNames)
            {
                AddTemporaryTable(tableName);
            }
        }

        public void RemoveTemporaryTable(string tableName)
        {
            List<string> tableNames = TemporaryTables.Split(';').ToList();
            if (tableNames.Contains(tableName))
            {
                tableNames.Remove(tableName);
                TemporaryTables = null;
                AddTemporaryTables(tableNames);
            }
        }

        public void RemoveTemporaryTables(List<string> tableNames)
        {
            foreach (var tableName in tableNames)
                RemoveTemporaryTable(tableName);
        }

        public void ClearTemporaryTables()
        {
            TemporaryTables = null;
        }
        public List<string> GetTemporaryTablesNames()
        {
            if (string.IsNullOrWhiteSpace(TemporaryTables))
                return new List<string>();
            List<string> tableNames = TemporaryTables.Split(';').ToList();
            return tableNames;
        }

    }

    ///// <MetaDataID>{ae66d492-5ba0-4348-9fe4-89a17fe7ee22}</MetaDataID>
    //[BackwardCompatibilityID("{ae66d492-5ba0-4348-9fe4-89a17fe7ee22}")]
    //[Persistent()]
    //public class StorageAgent : PersistenceLayer.Storage
    //{

    //    public StorageAgent(Storage storage)
    //    {
    //        Storage = storage;
    //        StateManagerLink = ObjectStateManagerLink.GetExtensionPropertiesFromObject(storage);

    //    }

    //    /// <exclude>Excluded</exclude>
    //    OOAdvantech.ObjectStateManagerLink StateManagerLink;
    //    public string StorageIdentity => Storage.StorageIdentity;

    //    public string Culture { get => Storage.Culture; set => Storage.Culture = value; }
    //    public string StorageLocation { get => Storage.StorageLocation; set => Storage.StorageLocation = value; }
    //    public string StorageType { get => Storage.StorageType; set => Storage.StorageType = value; }
    //    public string StorageName { get => Storage.StorageName; set => Storage.StorageName = value; }
    //    public string NativeStorageID { get => Storage.NativeStorageID; set => Storage.NativeStorageID = value; }
    //    public Storage Storage { get; private set; }

    //    public bool CheckForVersionUpgrate(string fullName)
    //    {
    //        return Storage.CheckForVersionUpgrate(fullName);
    //    }

    //    public void RegisterComponent(string assemblyFullName, List<string> types = null)
    //    {
    //        Storage.RegisterComponent(assemblyFullName, types);
    //    }

    //    public void RegisterComponent(string[] assembliesFullNames)
    //    {
    //        Storage.RegisterComponent(assembliesFullNames);

    //    }

    //    public void RegisterComponent(string assemblyFullName, string mappingDataResourceName, List<string> types = null)
    //    {
    //        Storage.RegisterComponent(assemblyFullName, mappingDataResourceName, types);

    //    }

    //    public void RegisterComponent(string assemblyFullName, XDocument mappingData)
    //    {
    //        Storage.RegisterComponent(assemblyFullName, mappingData);

    //    }

    //    public void RegisterComponent(string[] assembliesFullNames, Dictionary<string, XDocument> assembliesMappingData)
    //    {
    //        Storage.RegisterComponent(assembliesFullNames, assembliesMappingData);

    //    }
    //}

}
