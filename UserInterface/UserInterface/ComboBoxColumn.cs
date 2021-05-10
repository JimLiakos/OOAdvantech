namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.Transactions;
    /// <MetaDataID>{455252C0-8601-4773-A5CB-C2E40B62F9AA}</MetaDataID>
    public class ComboBoxColumn : OOAdvantech.UserInterface.Column, ICollectionView
    {

        /// <exclude>Excluded</exclude>
        bool _AutoInsert;

        /// <MetaDataID>{448f68aa-0ee6-4c16-b4b1-543071b29a48}</MetaDataID>
        [MetaDataRepository.PersistentMember("_AutoInsert")]
        [MetaDataRepository.BackwardCompatibilityID("+3")]
        public bool AutoInsert
        {
            get
            {
                return _AutoInsert;
            }
            set
            {
                if (_AutoInsert != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AutoInsert = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        private OperationCall _InsertOperation;
        /// <MetaDataID>{999f7a3d-11f5-4782-81f4-c7b2e4e783a7}</MetaDataID>
        [MetaDataRepository.PersistentMember("_InsertOperation")]
        [ MetaDataRepository.BackwardCompatibilityID("+2")]
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
        /// <MetaDataID>{A21524A6-435F-4FA0-9CD0-3255F355632B}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_SearchOperation")]
        public OperationCall SearchOperation
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


        /// <MetaDataID>{4a3b8f47-e56c-42e1-9678-1089690a0dc2}</MetaDataID>
        protected ComboBoxColumn()
        {
        }
        /// <MetaDataID>{A0426ACA-456B-4876-A57C-ED1F292CADAA}</MetaDataID>
        public ComboBoxColumn(Column copyColumn)
            : base(copyColumn)
        {
        }
    }
}
