using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{a71dc8b8-ba86-4cbd-b90a-32260eb0f94e}</MetaDataID>
    [ServiceContract]
    public interface IMessageDispatcher
    {
        /// <MetaDataID>{6716973d-622f-4e46-96db-19bc6b357811}</MetaDataID>
        [OperationContract]
        ResponseData MessageDispatch(RequestData request);
    }
}
