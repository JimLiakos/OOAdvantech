namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
    using MetaDataRepository.ObjectQueryLanguage;
    //using OOAdvantech.RDBMSPersistenceRunTime;

    /// <MetaDataID>{2E7AE9F5-98AD-480C-82B4-6EBE82400E9D}</MetaDataID>
    public class DataLoader : RDBMSPersistenceRunTime.DataLoader
    {

        //protected override PersistenceLayer.ObjectID GetTemporaryObjectID()
        //{
        //    return new OOAdvantech.RDBMSPersistenceRunTime.ObjectID(System.Guid.NewGuid(), 0);
        //}

        ///// <MetaDataID>{fa9bfd82-7aee-4c76-be72-0f16b70705fc}</MetaDataID>
        //protected override bool CanAggregateFanctionsResolvedLocally(DataNode aggregateFunctionDataNode)
        //{
        //    return true;
        //}
    

        /// <MetaDataID>{BA7812A0-A136-42ED-8E06-1D388CAC28DA}</MetaDataID>
        public DataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode, dataLoaderMetadata)
        { 
        }


    }
}