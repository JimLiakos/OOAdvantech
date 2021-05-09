using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;

namespace OOAdvantech.Remoting.RestApi.Serialization
{
    /// <MetaDataID>{646bd375-e44b-434b-8270-c1757c22490f}</MetaDataID>
    public class SerializeSession
    {

        public JsonReader JsonReader;
        public JsonWriter JsonWriter;
        public JsonSerializer JsonSerializer;

        public JsonSerializationFormat SerializationFormat;

        public Dictionary<Type, JsonType> JsonTypes = new Dictionary<Type, JsonType>();
    }


}
