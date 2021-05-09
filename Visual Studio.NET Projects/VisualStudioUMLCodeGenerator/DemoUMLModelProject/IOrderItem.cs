using OOAdvantech.MetaDataRepository;
namespace DemoUMLModel
{
    /// <MetaDataID>2f531f6a-7810-4a52-8d05-49331b2ab715</MetaDataID>
    public interface IOrderItem
    {
        [Association("OrderItem", Roles.RoleB, "a0fc7a98-3383-4698-b086-de5787b6c346"), RoleBMultiplicityRange(1, 1)]
        [OOAdvantech.MetaDataRepository.AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        IOrder Order
        {
            get;
        }
    }
}
