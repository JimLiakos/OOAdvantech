using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{88edb9f8-c119-4554-9c8a-7f266c66382a}</MetaDataID>
    [Serializable]
    public class CompositePart : QueryResultPart
    {

        /// <MetaDataID>{cf685eff-53a1-421a-a3ae-f2a5ccefca7a}</MetaDataID>
        public CompositePart(QueryResultType type, string name, QueryResultType ownerType)
            : base(name, ownerType)
        {
            _Type = type;
        }

        /// <exclude>Excluded</exclude>
        QueryResultType _Type;
        [Association("", Roles.RoleA, "32fb768d-8c9e-46e2-b4de-336e141172e5")]
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
            get { return _Type.RootDataNode; }
            set { }
        }


        internal override QueryResultPart Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {
        
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as QueryResultPart;
            QueryResultPart newQueryResultPart = new CompositePart(_Type.Clone(clonedObjects) as QueryResultType, Name, OwnerType.Clone(clonedObjects) as QueryResultType);
            clonedObjects[this] = newQueryResultPart;
            return newQueryResultPart;
        
        }
    }
}
