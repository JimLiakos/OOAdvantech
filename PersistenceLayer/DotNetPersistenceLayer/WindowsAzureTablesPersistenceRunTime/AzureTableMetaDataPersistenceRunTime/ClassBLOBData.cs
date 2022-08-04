using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{ecdb7768-48a2-43e3-b21b-636f2b1ea537}</MetaDataID>
    public class ClassBLOBData :Microsoft.Azure.Cosmos.Table.TableEntity,Azure.Data.Tables.ITableEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassBLOBData" /> class.
        /// Your entity type must expose a parameter-less constructor
        /// </summary>
        /// <MetaDataID>{1b785e20-b377-4286-8e76-9f3f593c5993}</MetaDataID>
        public ClassBLOBData()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ClassBLOBData" /> class.
        /// Defines the PK and RK.
        /// </summary>
        /// <param name="partitionKey">The last name.</param>
        /// <param name="rowKey">The first name.</param>
        /// <MetaDataID>{d76811bf-b312-48fc-8966-9b8f4cae2918}</MetaDataID>
        public ClassBLOBData(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        

        public static List<RDBMSMetaDataRepository.Column> ListOfColumns = new List<RDBMSMetaDataRepository.Column>() { new RDBMSMetaDataRepository.Column("ClassData" ,MetaDataRepository.Classifier.GetClassifier(typeof(byte[]))) , new RDBMSMetaDataRepository.Column("MetaObjectIdentity", MetaDataRepository.Classifier.GetClassifier(typeof(string)))};

        /// <MetaDataID>{167e91eb-852f-4a8b-86c8-5aef201baafa}</MetaDataID>
        public byte[] ClassData { get; set; }


        /// <MetaDataID>{f3b3e37e-e7f1-4dff-87bb-ad84f711eee2}</MetaDataID>
        public string MetaObjectIdentity { get; set; }
        DateTimeOffset? ITableEntity.Timestamp { get ; set; }
        ETag ITableEntity.ETag { get ; set ; }
    }
}
