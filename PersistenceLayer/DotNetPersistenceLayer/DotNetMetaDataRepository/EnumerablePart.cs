using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{2c4d982f-a773-46ce-92ad-b65e9965bbab}</MetaDataID>
    [Serializable]
    public class EnumerablePart : QueryResultPart
    {

        /// <MetaDataID>{249c47bc-8710-4da4-9e3b-bf304756af34}</MetaDataID>
        public EnumerablePart(QueryResult queryResultPart, string name):base(name)
        {
            _Type = queryResultPart;
        }
        /// <exclude>Excluded</exclude>
        QueryResult _Type;
        [Association("", Roles.RoleA, "6dbcc817-536f-44b5-8e05-00659595d20c")]
        public QueryResult Type
        {
            set
            {
            
            }
            get
            {
                return _Type;
            }
        }

        public override DataNode SourceDataNode
        {
            get 
            {
                return _Type.RootDataNode;
            }
        }
    }
}
