using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{


    /// <summary>
    /// Defines the αppropriate Json serializer settings for channel data serialization
    /// </summary>
    /// /// <MetaDataID>{c359595e-70da-439c-95be-bc62cdaed989}</MetaDataID>
    public class JsonSerializerSettings : OOAdvantech.Json.JsonSerializerSettings
    {

        /// <summary>
        /// Initial json settings 
        /// </summary>
        /// <param name="jsonContructType">
        /// Defines the type of json action  Serialize,Deserialize
        /// </param>
        /// <param name="serializationFormat">
        /// Defines the type of json serialization format, .net or type script format.
        /// </param>
        /// <param name="serverSessionPart">
        /// Defines the channel server session format
        /// </param>
        /// <param name="argsTypes"></param>
        public JsonSerializerSettings(JsonContractType jsonContructType, JsonSerializationFormat serializationFormat,/* string channelUri, string internalChannelUri,*/ ServerSessionPart serverSessionPart, Type[] argsTypes = null)
        {

            if(serializationFormat==JsonSerializationFormat.NetJsonSerialization)
                serializationFormat = JsonSerializationFormat.TypeScriptJsonSerializationEx;
            // serializationFormat = JsonSerializationFormat.TypeScriptJsonSerializationEx;

            SerializationFormat = serializationFormat;
            JsonContructType = jsonContructType;
            TypeNameHandling =( serializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization|| serializationFormat == JsonSerializationFormat.TypeScriptJsonSerializationEx) ? OOAdvantech.Json.TypeNameHandling.None : OOAdvantech.Json.TypeNameHandling.All;
            Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(serializationFormat);
            ContractResolver = new JsonContractResolver(jsonContructType,/* channelUri, internalChannelUri,*/ serverSessionPart, argsTypes, serializationFormat);


            if (serializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization|| serializationFormat == JsonSerializationFormat.TypeScriptJsonSerializationEx)
            {
                if (jsonContructType == JsonContractType.Serialize)
                    ReferenceResolver = new ReferenceResolver();

                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                DateTimeZoneHandling = OOAdvantech.Json.DateTimeZoneHandling.Utc;
            }
        }

        public void ClearObjectRefIds()
        {
            if (SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization && JsonContructType == JsonContractType.Serialize)
            {
                ReferenceResolver = new ReferenceResolver();
            }
        }

        JsonSerializationFormat SerializationFormat;
        JsonContractType JsonContructType;


        public JsonSerializerSettings(JsonContractType jsonContructType, JsonSerializationFormat serializationFormat, string channelUri, string internalChannelUri, ServerSessionPart serverSessionPart)
            : this(jsonContructType, serializationFormat,/* channelUri,internalChannelUri,*/ serverSessionPart, null)
        {

            if (serverSessionPart != null)
            {
                if (serverSessionPart.ChannelUri != channelUri)
                {

                }

            }
        }
    }


    public enum JsonSerializationFormat
    {
        NetJsonSerialization,
        TypeScriptJsonSerialization,
        TypeScriptJsonSerializationEx

    }
}
