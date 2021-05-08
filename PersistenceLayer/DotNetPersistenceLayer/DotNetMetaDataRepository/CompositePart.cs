namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{88edb9f8-c119-4554-9c8a-7f266c66382a}</MetaDataID>
    public class CompositePart : QueryResultPart
    {

        public CompositePart(QueryResult type, string name):base(name)
        {
            _Type = type;
        }

        /// <exclude>Excluded</exclude>
        QueryResult _Type;
        [Association("", Roles.RoleA, "32fb768d-8c9e-46e2-b4de-336e141172e5")]
        public QueryResult Type
        {
            get
            {
                return _Type;
            }
        }
    }
}
