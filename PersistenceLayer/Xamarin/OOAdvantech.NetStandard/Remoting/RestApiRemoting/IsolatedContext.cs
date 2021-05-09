using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{7005020f-3bd7-407b-b427-faae8c0c576b}</MetaDataID>
    public class IsolatedContext
    {
        internal static ResponseData DispatchMessage(RequestData request)
        {
            var responseData = MessageDispatcher.MessageDispatch(request);
            responseData.BidirectionalChannel = true;
            return responseData;

        }
    }
}
