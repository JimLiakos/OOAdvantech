using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{ecdb7768-48a2-43e3-b21b-636f2b1ea537}</MetaDataID>
    public class StorageMetadata : Microsoft.Azure.Cosmos.Table.TableEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageMetadata"/> class.
        /// Your entity type must expose a parameter-less constructor
        /// </summary>
        public StorageMetadata()
        {
        }

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
}
