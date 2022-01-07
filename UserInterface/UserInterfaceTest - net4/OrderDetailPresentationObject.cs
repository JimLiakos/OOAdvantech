using System;
using System.Collections.Generic;
using System.Text;

namespace UserInterfaceTest
{
    /// <MetaDataID>{206e9a3d-5f2e-4d77-a273-1157623d3a1c}</MetaDataID>
    public class OrderDetailPresentationObject:OOAdvantech.UserInterface.Runtime.PresentationObject<AbstractionsAndPersistency.IOrderDetail>
    {
  
        
        public OrderDetailPresentationObject(AbstractionsAndPersistency.IOrderDetail orderDetail):base(orderDetail)
        {

        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        public string Name
        {
            get
            {
                return RealObject.Name;
            }
            set
            {
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);


            }
        }
        public string StoragePlaceName
        {
            get
            {
                return "Rt";
            }
        }
    }
}
