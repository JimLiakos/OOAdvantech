using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
namespace OOAdvantech.Security
{
    /// <MetaDataID>{59770bc3-47d1-49d3-ab33-7e4bbf39e3a0}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{59770bc3-47d1-49d3-ab33-7e4bbf39e3a0}"), MetaDataRepository.Persistent()]
    public class Subject
    {
        IOutOfStorageAccountInfo _AcountInfo;
        [Association("ExternalAccount", Roles.RoleA, "21585daa-73ae-4656-86d6-b08273fc7be8"), MetaDataRepository.PersistentMember("_AcountInfo")]
        public IOutOfStorageAccountInfo AcountInfo
        {
            get
            {
                return _AcountInfo;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _AcountInfo = value; 
                    stateTransition.Consistent = true;
                }
        
            }
        }
     
        /// <MetaDataID>{2f5968ca-cee9-44ba-a750-61698afa8665}</MetaDataID>
        private OOAdvantech.ObjectStateManagerLink Properties =null;

        /// <exclude>Excluded</exclude>
        string _FullName;
        /// <MetaDataID>{059c62b0-e817-4974-afe3-60f00cd002fd}</MetaDataID>
        /// <summary>FullName is the name of security server plus name of subject.</summary>
        [PersistentMember("_FullName")]
        [BackwardCompatibilityID("+1")]
        public string FullName
        {
            get
            {
                return default(string);
            }
            set
            {
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6659a54c-cee8-4f43-8856-5b55b16b874f}</MetaDataID>
        protected string _Name;
        /// <MetaDataID>{e20f4196-e5c1-4e49-bc05-aac55e558417}</MetaDataID>
        [PersistentMember("_Name")]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
    }
}
