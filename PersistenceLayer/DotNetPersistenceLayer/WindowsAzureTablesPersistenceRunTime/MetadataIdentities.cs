namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{34f61f48-a15e-48ec-ab2a-68ee2409c94c}</MetaDataID>
    public class MetadataIdentities : Microsoft.Azure.Cosmos.Table.TableEntity
    {

        /// <MetaDataID>{ddc52708-cfe0-44e2-a0d2-d3879bdcb0e5}</MetaDataID>
        public MetadataIdentities()
        {

        }

        /// <MetaDataID>{404bfd82-36a9-4265-9cff-89d98f80d8e5}</MetaDataID>
        public MetadataIdentities(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }


        //public static System.Collections.Generic.List<string> ListOfColumns = new System.Collections.Generic.List<string>() { "NextOID" };
        public static Collections.Generic.List<RDBMSMetaDataRepository.Column> ListOfColumns = new Collections.Generic.List<RDBMSMetaDataRepository.Column>() { new RDBMSMetaDataRepository.Column("NextOID", MetaDataRepository.Classifier.GetClassifier(typeof(int)))};

        /// <MetaDataID>{78d7d923-af54-4890-a23a-6611af7cd084}</MetaDataID>
        public int NextOID { get; set; }

    }
}