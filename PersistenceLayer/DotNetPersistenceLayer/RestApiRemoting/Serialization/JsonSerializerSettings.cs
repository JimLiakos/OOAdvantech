﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi.Serialization
{


    /// <summary>
    /// Defines the αppropriate Json serializer settings for channel data serialization
    /// </summary>
    /// /// <MetaDataID>{c359595e-70da-439c-95be-bc62cdaed989}</MetaDataID>
    public class JsonSerializerSettings : OOAdvantech.Json.JsonSerializerSettings
    {

        
        public static OOAdvantech.Json.JsonSerializerSettings TypeRefSerializeSettings
        {
            get
            {
                var jSetttings = new OOAdvantech.Json.JsonSerializerSettings() { ReferenceLoopHandling = OOAdvantech.Json.ReferenceLoopHandling.Serialize, TypeNameHandling = OOAdvantech.Json.TypeNameHandling.None, Binder = new OOAdvantech.Remoting.RestApi.Serialization.SerializationBinder(OOAdvantech.Remoting.RestApi.Serialization.JsonSerializationFormat.TypeScriptJsonSerialization), ContractResolver = new OOAdvantech.Remoting.RestApi.Serialization.JsonContractResolver(OOAdvantech.Remoting.RestApi.Serialization.JsonContractType.Serialize, null, OOAdvantech.Remoting.RestApi.Serialization.JsonSerializationFormat.TypeScriptJsonSerialization), ReferenceResolver = new OOAdvantech.Remoting.RestApi.Serialization.ReferenceResolver() };
                jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                jSetttings.DateTimeZoneHandling = Json.DateTimeZoneHandling.Utc;
                return jSetttings;
            }
        }


        public static OOAdvantech.Json.JsonSerializerSettings TypeRefDeserializeSettings
        {
            get
            {
                var jSetttings = new OOAdvantech.Json.JsonSerializerSettings() { TypeNameHandling = OOAdvantech.Json.TypeNameHandling.None, ContractResolver = new OOAdvantech.Remoting.RestApi.Serialization.JsonContractResolver(OOAdvantech.Remoting.RestApi.Serialization.JsonContractType.Deserialize, null, OOAdvantech.Remoting.RestApi.Serialization.JsonSerializationFormat.TypeScriptJsonSerialization) };
                jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                jSetttings.DateTimeZoneHandling = Json.DateTimeZoneHandling.Utc;
                return jSetttings;
            }
        }


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

            //if(serializationFormat==JsonSerializationFormat.NetJsonSerialization)
            //    serializationFormat = JsonSerializationFormat.NetTypedValuesJsonSerialization;
            //// serializationFormat = JsonSerializationFormat.TypeScriptJsonSerializationEx;

            SerializationFormat = serializationFormat;
            JsonContructType = jsonContructType;
            TypeNameHandling =( serializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization|| serializationFormat == JsonSerializationFormat.NetTypedValuesJsonSerialization) ? OOAdvantech.Json.TypeNameHandling.None : OOAdvantech.Json.TypeNameHandling.All;
            Binder = new SerializationBinder(serializationFormat);
            ContractResolver = new JsonContractResolver(jsonContructType,/* channelUri, internalChannelUri,*/ serverSessionPart, argsTypes, serializationFormat);

            MissingMemberHandling = Json.MissingMemberHandling.Error;

            if (serializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization|| serializationFormat == JsonSerializationFormat.NetTypedValuesJsonSerialization)
            {
                if (jsonContructType == JsonContractType.Serialize)
                {
                    ReferenceResolver = new ReferenceResolver();
                    ReferenceLoopHandling =OOAdvantech.Json.ReferenceLoopHandling.Serialize;
                }

                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                DateTimeZoneHandling = OOAdvantech.Json.DateTimeZoneHandling.Utc;
                if (jsonContructType == JsonContractType.Deserialize)
                    DateTimeZoneHandling = OOAdvantech.Json.DateTimeZoneHandling.Local;

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


    /// <MetaDataID>{f4e724dc-aa49-4451-a004-5b260f300a31}</MetaDataID>
    public enum JsonSerializationFormat
    {
        NetJsonSerialization,
        TypeScriptJsonSerialization,
        NetTypedValuesJsonSerialization

    }

    /// <MetaDataID>{25378c3f-e22c-4069-bdde-c0bf43b6112e}</MetaDataID>
    public enum JsonContractType
    {
        Serialize,
        Deserialize

    }
}