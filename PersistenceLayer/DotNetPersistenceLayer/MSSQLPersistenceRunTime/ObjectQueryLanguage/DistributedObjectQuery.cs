using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
    
    public class DistributedObjectQuery:MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery
    {
        public DistributedObjectQuery(
           OOAdvantech.Collections.Generic.List<MetaDataRepository.ObjectQueryLanguage.DataNode> dataTrees,
            OOAdvantech.Collections.Generic.List<MetaDataRepository.ObjectQueryLanguage.DataNode> selectListItems,
           MetaDataRepository.ObjectQueryLanguage.IObjectQueryPartialResolver objectStorage,
           OOAdvantech.Collections.Generic.Dictionary<Guid, MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> dataLoadersMetadata,
           OOAdvantech.Collections.Generic.Dictionary<string, object> parameters):
            base(dataTrees,selectListItems, objectStorage, dataLoadersMetadata, parameters)
        {

        }
        protected override void LoadData()
        {
            base.LoadData();
        }
    }
}
