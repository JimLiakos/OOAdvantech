using System.Collections.Generic;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{dba97d23-5b30-42ed-816a-57684a50d714}</MetaDataID>
    public class QueryResult
    {
        [Association("SourceData", Roles.RoleA, "4e655a66-daf9-424a-88b2-8ac713c7be78")]
        public QueryResult Source;
        /// <MetaDataID>{b218b576-9dfd-4dd3-a4c7-274d619de808}</MetaDataID>
        public List<QueryResultPart> _Members =new List<QueryResultPart>();

        [Association("ResultTypeMember", Roles.RoleA, "54e81db7-2834-407e-9240-c71276960501"), RoleAMultiplicityRange(1)]
        public List<QueryResultPart>  Members
        {
            get
            {
                return _Members;
            }
        }

        /// <MetaDataID>{b060f5b3-e1be-426a-af31-bad3d243efe9}</MetaDataID>
        public void AddMember(QueryResultPart queryResultPart)
        {
            _Members.Add(queryResultPart);
        }

        /// <MetaDataID>{5f7daa0b-db93-4f7f-9bef-55ff3f304765}</MetaDataID>
        public void RemoveMember(QueryResultPart queryResultPart)
        {
            _Members.Remove(queryResultPart);
        }
    }
}
