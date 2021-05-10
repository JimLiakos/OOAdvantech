namespace OOAdvantech.UserInterface
{
    using OOAdvantech.MetaDataRepository;
    /// <MetaDataID>{E9E642DB-3818-4091-9E12-6ABE14D912BD}</MetaDataID>
    public interface ICollectionView
    {
        [Association("Insert", Roles.RoleA, "9de53f25-9fbe-4551-a908-4a0fdcdba8ad")]
        [AssociationEndBehavior(MetaDataRepository.PersistencyFlag.OnConstruction | MetaDataRepository.PersistencyFlag.CascadeDelete)]
        OperationCall InsertOperation
        {
            get;
            set;
        }
       
        /// <MetaDataID>{6B859C45-E1F8-40E6-9BDC-92EB6C99A9E5}</MetaDataID>
        [Association("Searching", Roles.RoleA, "{8D79C02D-26E1-4D77-B3D3-8BA42A89EFA2}")]
        [AssociationEndBehavior(MetaDataRepository.PersistencyFlag.OnConstruction | MetaDataRepository.PersistencyFlag.CascadeDelete)]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(0)]
        OperationCall SearchOperation
        {
            get;
            set;
        }
    }
}
