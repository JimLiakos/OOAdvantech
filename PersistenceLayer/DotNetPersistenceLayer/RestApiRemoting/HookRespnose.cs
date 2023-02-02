using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{74f3d8bd-a984-4071-8361-0ff3be4d2c7d}</MetaDataID>
    public class HookRespnose
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public string Content { get; set; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    }
}
