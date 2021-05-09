using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{ecdb7768-48a2-43e3-b21b-636f2b1ea537}</MetaDataID>
    public class Person : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageMetadata"/> class.
        /// Your entity type must expose a parameter-less constructor
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageMetadata"/> class.
        /// Defines the PK and RK.
        /// </summary>
        /// <param name="partitionKey">The last name.</param>
        /// <param name="rowKey">The first name.</param>
        public Person(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }


        public string StorageName { get; set; }

        
        public byte[] Data { get; set; }

    }
}
