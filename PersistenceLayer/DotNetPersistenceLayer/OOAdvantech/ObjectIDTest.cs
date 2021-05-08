using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech
{
    public class ObjectIDTest : System.MarshalByRefObject, Remoting.IExtMarshalByRefObject
    {
        public object GetObject()
        {
            return new OOAdvantech.PersistenceLayer.ObjectID(Guid.NewGuid(),1);
        }
    }
}
