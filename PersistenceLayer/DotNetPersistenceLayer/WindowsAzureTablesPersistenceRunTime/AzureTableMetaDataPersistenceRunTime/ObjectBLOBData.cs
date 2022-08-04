using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{5987006d-6ce7-4702-9ba7-57d8c87add80}</MetaDataID>
    public class ObjectBLOBData : Microsoft.Azure.Cosmos.Table.TableEntity,Azure.Data.Tables.ITableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBLOBData" /> class.
        /// Your entity type must expose a parameter-less constructor
        /// </summary>
        /// <MetaDataID>{02986e07-a622-457c-b905-426a8b6341ae}</MetaDataID>
        public ObjectBLOBData()
        {
        }

        public static List<RDBMSMetaDataRepository.Column> ListOfColumns = new List<RDBMSMetaDataRepository.Column>() { new RDBMSMetaDataRepository.Column("ObjectData", MetaDataRepository.Classifier.GetClassifier(typeof(byte[]))), new RDBMSMetaDataRepository.Column("ClassBLOBSID", MetaDataRepository.Classifier.GetClassifier(typeof(string))) };


   
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBLOBData" /> class.
        /// Defines the PK and RK.
        /// </summary>
        /// <param name="partitionKey">The last name.</param>
        /// <param name="rowKey">The first name.</param>
        /// <MetaDataID>{e1595b35-af2b-497c-870a-3d0446384779}</MetaDataID>
        public ObjectBLOBData(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }



        /// <MetaDataID>{2746d151-5c1a-4623-bd98-5f077305dafe}</MetaDataID>
        public byte[] ObjectData { get; set; }


        /// <MetaDataID>{6a00075a-e044-4944-a4a8-c5df33dac37a}</MetaDataID>
        public string ClassBLOBSID { get; set; }
        DateTimeOffset? ITableEntity.Timestamp { get; set; }
        ETag ITableEntity.ETag { get; set; }
    }
}
