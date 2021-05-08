using System.Collections.Generic;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{5d77acc7-36d9-4a7b-bf3e-ba871b859739}</MetaDataID>
    public class ObjectsDataRetriever
    {

        public ObjectsDataRetriever(OOAdvantech.Collections.Generic.List<object> objects, DataNode objectsDataNode)
        {
            Objects = objects;
            ObjectsDataNode = objectsDataNode;
        }
        [RoleAMultiplicityRange(1)]
        [Association("", Roles.RoleA, "8e9bbfa3-1968-462b-b074-3d48b01d82bd")]
        public List<object> Objects;
        
         
        [Association("", Roles.RoleA, "1074256b-a3e0-47ad-9b57-76b00ea89a1f")]
        public DistributedObjectQuery ObjectQuery;

        [Association("", Roles.RoleA, "84987434-3f07-4a61-80db-24e447d4985a")]
        public DataNode ObjectsDataNode;
    }
}
