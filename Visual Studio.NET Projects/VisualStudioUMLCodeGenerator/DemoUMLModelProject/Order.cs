using System;
using OOAdvantech.MetaDataRepository;
namespace DemoUMLModel
{
    /// <MetaDataID>57bc461d-65bf-452f-a123-6050c395199b</MetaDataID>
    public class Order : DemoUMLModel.IOrder
    {
        [Association("ClientOrders", Roles.RoleB, "1e63d3c8-9cf1-4e92-937f-e623f6c036d5")]
        public Client Client;
        /// <MetaDataID>05c8a9b8-d3d5-41a4-a7d4-4b080f711837</MetaDataID>
        public virtual string Name
        {
            get
            {
                return default(string);
            }
        }

        /// <MetaDataID>0815f3de-b3bf-4a1d-806f-f7f048e474d9</MetaDataID>
        public int Foo(int argA, DateTime argB)
        {
            return default(int);
        }
    }
}
