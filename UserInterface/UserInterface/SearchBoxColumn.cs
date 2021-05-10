namespace OOAdvantech.UserInterface
{
    using MetaDataRepository;
    using Transactions;
    /// <MetaDataID>{AA63DE6D-0523-44BD-B48E-1232DACDA6CC}</MetaDataID>
    [BackwardCompatibilityID("{AA63DE6D-0523-44BD-B48E-1232DACDA6CC}")]
    [Persistent()]
    public class SearchBoxColumn : Column, UserInterface.ICollectionView
    {
        /// <exclude>Excluded</exclude>
        private OperationCall _InsertOperation;
        /// <MetaDataID>{9f9ab149-88d2-4dc1-9574-96f132584132}</MetaDataID>
        [MetaDataRepository.PersistentMember("_InsertOperation")]
        [ MetaDataRepository.BackwardCompatibilityID("+3")]
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
        /// <MetaDataID>{BD3DBF8A-3B97-40A8-AC57-14331CC73277}</MetaDataID>
        [BackwardCompatibilityID("+2")]
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
        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{33DAB410-B1D0-4728-92AE-5A2C8D309314}</MetaDataID>
        //private OperationCall _SearchOperation;
        ///// <MetaDataID>{6B859C45-E1F8-40E6-9BDC-92EB6C99A9E5}</MetaDataID>
        //[Association("Searching",typeof(OOAdvantech.UserInterface.OperationCall),Roles.RoleA,"{8D79C02D-26E1-4D77-B3D3-8BA42A89EFA2}")]
        //[AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.CascadeDelete)]
        //[PersistentMember("_SearchOperation")]
        //[RoleAMultiplicityRange(1,1)]
        //[RoleBMultiplicityRange(0)]
        //public OperationCall SearchOperation
        //{
        //    get
        //    {
        //        return _SearchOperation;
        //    }
        //    set
        //    {
        //        if (_SearchOperation != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _SearchOperation = value;
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //    }
        //}
        /// <MetaDataID>{91C74ADB-515C-449F-AA76-70C0913AF0F1}</MetaDataID>
        protected SearchBoxColumn()
        {
        }
        /// <MetaDataID>{A0426ACA-456B-4876-A57C-ED1F292CADAA}</MetaDataID>
        public SearchBoxColumn(Column copyColumn)
            : base(copyColumn)
        {
        }
    }
}
