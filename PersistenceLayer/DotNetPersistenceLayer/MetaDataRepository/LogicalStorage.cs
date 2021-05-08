using OOAdvantech.Collections.Generic;
using OOAdvantech.Transactions;
namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{0a7e9472-33c4-4368-9059-13daa82274ce}</MetaDataID>
    [BackwardCompatibilityID("{0a7e9472-33c4-4368-9059-13daa82274ce}")]
    [ Persistent()]
    public class LogicalStorage : OOAdvantech.MetaDataRepository.Storage
    {

        /// <MetaDataID>{402bebc1-90ed-4ed8-853a-9533cf8f34f4}</MetaDataID>
        public void AddStorage(StorageReference storage)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Storages.Add(storage); 
                stateTransition.Consistent = true;
            }
        
        }

        /// <MetaDataID>{7f2436b7-52af-4226-b216-53f9005e6d2a}</MetaDataID>
        public void DeleteStorage(StorageReference storage)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Storages.Remove(storage);
                stateTransition.Consistent = true;
            }
        }

        /// <exclude>Excluded</exclude>
        Set<StorageReference> _Storages=new Set<StorageReference>();
        [Association("ContextStorages", Roles.RoleB, "1d911a41-ef48-4966-b5e1-6a1e05c2264a")]
        [PersistentMember("_Storages")] 
        [RoleAMultiplicityRange(1)]
        public Set<StorageReference> Storages
        {
            get
            {
                return _Storages.AsReadOnly();
            }
        }

        public override void RegisterComponent(string assemblyFullName)
        {
            throw new System.NotImplementedException();
        }

        public override void RegisterComponent(string[] assembliesFullNames)
        {
            throw new System.NotImplementedException();
        }

        public override void RegisterComponent(string assemblyFullName, System.Xml.Linq.XDocument mappingData)
        {
            throw new System.NotImplementedException();
        }

        public override void RegisterComponent(string assemblyFullName, string mappingDataResourceName)
        {
            throw new System.NotImplementedException();
        }

        public override void RegisterComponent(string[] assembliesFullNames, System.Collections.Generic.Dictionary<string, System.Xml.Linq.XDocument> assembliesMappingData)
        {
            throw new System.NotImplementedException();
        }
    }
}
