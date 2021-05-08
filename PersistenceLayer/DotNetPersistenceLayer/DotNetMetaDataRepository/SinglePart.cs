namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{e2e946c5-8dea-4336-b2c4-3e521d578aac}</MetaDataID>
    public class SinglePart : QueryResultPart
    {
        /// <MetaDataID>{59899afa-7af1-49d8-9a4f-c80040574228}</MetaDataID>
        public SinglePart(DataNode dataNode, string name)
            : base(name)
        {
            _DataNode = dataNode;
        }

        /// <MetaDataID>{88fdb0e9-c2cc-4463-ba97-4824e2503db8}</MetaDataID>
        DataNode _DataNode;
        [Association("ReferToDataNode", Roles.RoleA, "ee1e626a-b9a4-4bad-8981-eba60318e117")]
        public DataNode DataNode
        {
            get
            {
                return _DataNode;
            }
        }
    }
}
