using System;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{e2e946c5-8dea-4336-b2c4-3e521d578aac}</MetaDataID>
    [Serializable]
    public class SinglePart : QueryResultPart
    {
        /// <MetaDataID>{59899afa-7af1-49d8-9a4f-c80040574228}</MetaDataID>
        public SinglePart(DataNode dataNode, string name, QueryResultType ownerType)
            : base(name, ownerType)
        {
            DataNodeIDentity = dataNode.Identity;
        }

        /// <MetaDataID>{88fdb0e9-c2cc-4463-ba97-4824e2503db8}</MetaDataID>
        Guid DataNodeIDentity;
        [Association("ReferToDataNode", Roles.RoleA, "ee1e626a-b9a4-4bad-8981-eba60318e117")]
        public DataNode DataNode
        {
            get
            {
                return OwnerType.RootDataNode.HeaderDataNode.GetDataNode(DataNodeIDentity);
            }
        }
        public override DataNode SourceDataNode
        {
            get { return OwnerType.RootDataNode.HeaderDataNode.GetDataNode(DataNodeIDentity); }
            set { DataNodeIDentity = value.Identity; }
        }


        internal override QueryResultPart Clone(System.Collections.Generic.Dictionary<object, object> clonedObjects)
        {

            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as QueryResultPart;
            QueryResultPart newQueryResultPart = new SinglePart(DataNode.Clone(clonedObjects), Name, OwnerType.Clone(clonedObjects) as QueryResultType);
            clonedObjects[this] = newQueryResultPart;
            return newQueryResultPart;
        }
    }
}
