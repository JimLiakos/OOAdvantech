using System;
using OOAdvantech.MetaDataRepository;
namespace DemoUMLModel
{
    /// <MetaDataID>8938d4c4-6191-4f10-adae-f3d4a309eb6a</MetaDataID>
    public interface IOrder
    {
        [Association("OrderItem", Roles.RoleA, "a0fc7a98-3383-4698-b086-de5787b6c346")]
        [OOAdvantech.MetaDataRepository.AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1)]
        System.Collections.Generic.List<IOrderItem> OrderItems
        {
            get;
        }
        /// <MetaDataID>6dc9a478-247e-40a8-98ab-d22f4733828a</MetaDataID>
        [PersistentMember(""), BackwardCompatibilityID("+1")]
        string Name
        {
            get;
        }


        /// <MetaDataID>548a5830-c6f4-4a6a-8247-257b59a87dcf</MetaDataID>
        int Foo(int argA, DateTime argB);

    }
}
