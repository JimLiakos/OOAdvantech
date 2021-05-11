using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Json.Serialization
{
    /// <MetaDataID>{d6c91cd6-5053-470c-b8d7-91846c37120f}</MetaDataID>
     class JsonSerializeObjtRef
    {
        public Proxy Value { get; set; }

        internal object GetRealObject()
        {
            return Value.RealObject;
        }
    }
    /// <MetaDataID>{f6245321-e3a6-4f73-b253-a5e6f85117b6}</MetaDataID>
    public class Proxy
    {
        public Object RealObject { get; set; }
    }

    /// <MetaDataID>{c54e6cbf-bb94-493d-b67e-737ff8fb9eae}</MetaDataID>
    public class TransparentProxy
    {

    }
}
