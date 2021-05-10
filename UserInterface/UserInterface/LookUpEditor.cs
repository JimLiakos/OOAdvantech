using OOAdvantech.Transactions;
using OOAdvantech.MetaDataRepository;
namespace OOAdvantech.UserInterface
{
    /// <MetaDataID>{32fa68e7-69e5-47a2-83be-8b852504c06d}</MetaDataID>
    public class LookUpEditor : OOAdvantech.UserInterface.ColumnEditor, OOAdvantech.UserInterface.ICollectionView
    {

        /// <exclude>Excluded</exclude>
        private OperationCall _InsertOperation;
        /// <MetaDataID>{e75416a6-5996-427c-ab89-fb311058e9d6}</MetaDataID>
        [MetaDataRepository.PersistentMember("_InsertOperation")]
        [MetaDataRepository.BackwardCompatibilityID("+2")]
        public OperationCall InsertOperation
        {
            get
            {
                return _InsertOperation;
            }
            set
            {
                if (_InsertOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _InsertOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        private OperationCall _SearchOperation;

        /// <MetaDataID>{2b2750ce-7fc3-4530-839e-b8cab92f5ad1}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_SearchOperation")]
        public OOAdvantech.UserInterface.OperationCall SearchOperation
        {
            get
            {
                return _SearchOperation;
            }
            set
            {
                if (_SearchOperation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SearchOperation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }

}
