namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{aed42d02-1cb4-430f-8d34-00a789cda1f3}</MetaDataID>
    public class QueryResultPart
    {
        /// <MetaDataID>{ee0f1e4e-7cfe-4ef5-bf31-084d11ed67b0}</MetaDataID>
        public override string ToString()
        {
            return Name;
        }
        /// <MetaDataID>{2ac14eb0-ad47-4c24-b413-2fecba2c8b65}</MetaDataID>
        public QueryResultPart(string name)
        {
            _Name = name;
        }

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{554085b2-f0db-4901-848b-0508c6d3460a}</MetaDataID>
        public string Name
        {
            get
            {
                return _Name;
            }
            

        }
    }
}
