using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.Json.Serialization;
using OOAdvantech.Json.Utilities;

namespace OOAdvantech.Remoting.RestApi.Serialization
{

    /// <MetaDataID>{4d05c69e-47d6-4b5f-b872-1f410806a2bf}</MetaDataID>
    public class JsonType
    {

        //static Dictionary<Type, JsonType> JsonTypes = new Dictionary<Type, JsonType>();
        public string JsonTypeName { get; set; }

        public JsonType(Type type)
        {

        }

        public JsonType(Type type, string jsonTypeName) : this(type)
        {
            this.JsonTypeName = jsonTypeName;
        }

#if DeviceDotNet
        public static JsonType GetJsonType(Type type, TypeNameAssemblyFormatHandling assemblyFormat, ISerializationBinder binder, SerializeSession serializeSession)
        {
            JsonType jsonType = null;
            if (!serializeSession.JsonTypes.TryGetValue(type, out jsonType))
            {
                jsonType = new JsonType(type, ReflectionUtils.GetTypeName(type, assemblyFormat, binder));
                serializeSession.JsonTypes[type] = jsonType;
            }
            return jsonType;
        }
    }
#else
        public static JsonType GetJsonType(Type type, TypeNameAssemblyFormatHandling assemblyFormat, ISerializationBinder binder, SerializeSession serializeSession)
        {
            JsonType jsonType = null;
            if (!serializeSession.JsonTypes.TryGetValue(type, out jsonType))
            {
                jsonType = new JsonType(type, ReflectionUtils.GetTypeName(type, assemblyFormat, binder));
                serializeSession.JsonTypes[type] = jsonType;
            }
            return jsonType;
        }

        //public static JsonType GetJsonType(Type type, System.Runtime.Serialization.Formatters.FormatterAssemblyStyle assemblyFormat, System.Runtime.Serialization.SerializationBinder binder, SerializeSession serializeSession)
        //{
        //    JsonType jsonType = null;
        //    if (!serializeSession.JsonTypes.TryGetValue(type, out jsonType))
        //    {
        //        jsonType = new JsonType(type, ReflectionUtils.GetTypeName(type, assemblyFormat, binder));
        //        serializeSession.JsonTypes[type] = jsonType;
        //    }
        //    return jsonType;
        //}

    }
#endif
}
