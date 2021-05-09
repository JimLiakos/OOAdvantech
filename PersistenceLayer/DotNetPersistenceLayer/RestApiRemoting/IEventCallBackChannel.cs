using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{796528a3-d08e-4bfb-8506-8c0d692f0e71}</MetaDataID>
    public interface IEventCallBackChannel
    {
        /// <MetaDataID>{4c4ab166-4e4d-4d96-a385-6a79b68fb28e}</MetaDataID>
        void SendResponceAsync(ResponseData responseData);

        /// <MetaDataID>{0a185aec-d4af-440c-830d-21424e3d6bd6}</MetaDataID>
        Task<ResponseData> SendRequestWithResponceAsync(RequestData requestData);
    }
}
