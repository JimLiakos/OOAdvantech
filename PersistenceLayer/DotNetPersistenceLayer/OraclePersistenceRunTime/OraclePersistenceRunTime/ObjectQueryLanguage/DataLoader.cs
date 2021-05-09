namespace OOAdvantech.OraclePersistenceRunTime.ObjectQueryLanguage
{
    using SubDataNodeIdentity = System.Guid;
    using MetaDataRepository.ObjectQueryLanguage;
    using ComparisonTermsType = MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType;
    using System.Collections.Generic;
    using OOAdvantech.RDBMSDataObjects;
    using ObjectID = RDBMSPersistenceRunTime.ObjectID;
    //using OOAdvantech.RDBMSPersistenceRunTime;



    /// <MetaDataID>{a12e1db5-71b3-4fb2-833f-1691887255dc}</MetaDataID>
    public class DataLoader : RDBMSPersistenceRunTime.DataLoader
    {
        //protected override PersistenceLayer.ObjectID GetTemporaryObjectID()
        //{
        //    return new OOAdvantech.RDBMSPersistenceRunTime.ObjectID(System.Guid.NewGuid(), 0);
        //}

        /// <MetaDataID>{fa9bfd82-7aee-4c76-be72-0f16b70705fc}</MetaDataID>
        protected override bool CanAggregateFanctionsResolvedLocally(DataNode aggregateFunctionDataNode)
        {
            return true;
        }


        /// <MetaDataID>{BA7812A0-A136-42ED-8E06-1D388CAC28DA}</MetaDataID>
        public DataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode, dataLoaderMetadata)
        {
        }
    }
}
