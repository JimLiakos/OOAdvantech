using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{2c4d982f-a773-46ce-92ad-b65e9965bbab}</MetaDataID>
    [Serializable]
    public class EnumerablePart : QueryResultPart
    {

        /// <MetaDataID>{249c47bc-8710-4da4-9e3b-bf304756af34}</MetaDataID>
        public EnumerablePart(QueryResultType queryResultPart, string name, QueryResultType ownerType)
            : base(name, ownerType)
        {
            _Type = queryResultPart;
        }
        /// <exclude>Excluded</exclude>
        QueryResultType _Type;
        [Association("", Roles.RoleA, "6dbcc817-536f-44b5-8e05-00659595d20c")]
        [IgnoreErrorCheck]
        public QueryResultType Type
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
            set
            {

            }
        }

        internal override QueryResultPart Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {

            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as QueryResultPart;
            QueryResultPart newQueryResultPart = new EnumerablePart(_Type.Clone(clonedObjects) as QueryResultType, Name, OwnerType.Clone(clonedObjects) as QueryResultType);
            clonedObjects[this] = newQueryResultPart;
            return newQueryResultPart;
        }
    }
}
