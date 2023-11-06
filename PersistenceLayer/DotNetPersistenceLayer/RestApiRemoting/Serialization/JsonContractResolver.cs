using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json.Serialization;

namespace OOAdvantech.Remoting.RestApi.Serialization
{
    /// <summary>
    /// JsonContractResolver extend the functionality of json.net for Java script client and marshal byref objects
    /// </summary>
    /// <MetaDataID>{7ea5eabe-705f-4a18-b844-8e6e1fe76165}</MetaDataID>
    public class JsonContractResolver : Json.Serialization.DefaultContractResolver
    {

        SerializeSession SerializeSession = new SerializeSession();

        public int DesirializeIndex = 0;
        private CachingMetaData CachingMetadata;
        JsonContractType JsonContructType;

        private OOAdvantech.Remoting.RestApi.ServerSessionPart ServerSessionPart;
        JsonSerializationFormat SerializationFormat;
        private Type[] RootArgsTypes;
        bool IsReference;
        //bool TypeEx;

        //public JsonContractResolver(JsonContractType jsonContructType, string channelUri, string internalChannelUri, ServerSessionPart serverSessionPart, bool web = false)
        //{
        //    JsonContructType = jsonContructType;
        //    this.ChannelUri = channelUri;
        //    this.InternalChannelUri = internalChannelUri;
        //    this.ServerSessionPart = serverSessionPart;
        //    Web = web;
        //}

        public JsonContractResolver(JsonContractType jsonContructType,/* string channelUri, string internalChannelUri,*/ ServerSessionPart serverSessionPart, CachingMetaData cachingMetadata, JsonSerializationFormat serializationFormat = JsonSerializationFormat.NetJsonSerialization)
        {
            CachingMetadata = cachingMetadata;
            //TypeEx = typeEx;
            JsonContructType = jsonContructType;
            //this.ChannelUri = channelUri;
            //this.InternalChannelUri = internalChannelUri;
            this.ServerSessionPart = serverSessionPart;
            SerializationFormat = serializationFormat;
            if (SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization || SerializationFormat == JsonSerializationFormat.NetTypedValuesJsonSerialization)
            {
                IsReference = true;
                // TypeEx = true;
            }
            this.SerializeSession.SerializationFormat = SerializationFormat;
        }

        public JsonContractResolver(JsonContractType jsonContructType, ServerSessionPart serverSessionPart, CachingMetaData cachingMetadata, Type[] argsTypes, JsonSerializationFormat serializationFormat = JsonSerializationFormat.NetJsonSerialization)
            : this(jsonContructType,  serverSessionPart, cachingMetadata, serializationFormat)
        {
            this.RootArgsTypes = argsTypes;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType != null && objectType.FullName == "OOAdvantech.Remoting.ExtObjectUri")
            {

            }
            JsonContract contract = base.CreateContract(objectType);
            bool isMarshalByRefObject = false;

            #region Is Type MarshalByRefObject

            if (JsonContructType == JsonContractType.Serialize)
            {
                isMarshalByRefObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(IExtMarshalByRefObject)));
                isMarshalByRefObject |= OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(OOAdvantech.Remoting.IServerSessionPart)));

                if (objectType == typeof(OOAdvantech.Json.Serialization.TransparentProxy))
                    isMarshalByRefObject = true;
            }
            if (JsonContructType == JsonContractType.Deserialize)
                isMarshalByRefObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(OOAdvantech.Json.Serialization.Proxy)));
            #endregion



            if (isMarshalByRefObject && SerializationFormat == JsonSerializationFormat.NetJsonSerialization)   //Converter for .Net client code and marshal by reference object. 
                contract.Converter = new JsonSerializerEx(JsonContructType, SerializeSession, ServerSessionPart,CachingMetadata, SerializationFormat);
            else if (SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization)
            {
                #region  Converter for java script client

                if (typeof(JsonType) != objectType)
                {
                    if (contract is JsonObjectContract)
                    {
                        var customConverter = new JsonConverterTypeScript(contract as JsonObjectContract, objectType, SerializeSession,/* ChannelUri, InternalChannelUri,*/ ServerSessionPart,CachingMetadata, RootArgsTypes);
                        contract.Converter = customConverter;

                        if (JsonContructType == JsonContractType.Serialize && IsReference)
                            contract.IsReference = true;

                        if (!isMarshalByRefObject)
                        {
                            //contract.Converter = new JsonConverterTypeScript(contract as JsonArrayContract, ChannelUri, InternalChannelUri, ServerSessionPart, RootArgsTypes);
                            // (contract as JsonObjectContract).OverrideCreator = new ObjectConstructor<object>(customConverter.ObjectCreator);
                        }
                        else
                            (contract as JsonObjectContract).OverrideCreator = new ObjectConstructor<object>(customConverter.ObjectCreator);

                        //var objContract = (JsonObjectContract)contract;
                        //if (objContract.ExtensionDataSetter == null)
                        //{

                        //    objContract.ExtensionDataSetter = (o, key, value) =>
                        //    {


                        //    };
                        //}
                    }
                    else if (contract is JsonArrayContract)
                        contract.Converter = new JsonConverterTypeScript(contract as JsonArrayContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart,CachingMetadata, RootArgsTypes);
                    else if (contract is JsonPrimitiveContract)
                        contract.Converter = new JsonConverterTypeScript(contract as JsonPrimitiveContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, CachingMetadata, RootArgsTypes);
                    else if (contract is JsonDictionaryContract)
                        contract.Converter = new JsonConverterTypeScript(contract as JsonDictionaryContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, CachingMetadata, RootArgsTypes);
                }
                #endregion

            }
            else if (SerializationFormat == JsonSerializationFormat.NetTypedValuesJsonSerialization)
            {
                #region  Converter for java script client

                if (typeof(JsonType) != objectType)
                {
                    if (contract is JsonObjectContract)
                    {
                        var customConverter = new JsonConverterTypeScript(contract as JsonObjectContract, objectType, SerializeSession, ServerSessionPart, CachingMetadata, RootArgsTypes);
                        contract.Converter = customConverter;

                        if (JsonContructType == JsonContractType.Serialize && IsReference)
                            contract.IsReference = true;

                        if (!isMarshalByRefObject)
                        {
                            //contract.Converter = new JsonConverterTypeScript(contract as JsonArrayContract, ChannelUri, InternalChannelUri, ServerSessionPart, RootArgsTypes);
                            // (contract as JsonObjectContract).OverrideCreator = new ObjectConstructor<object>(customConverter.ObjectCreator);
                        }
                        else
                            (contract as JsonObjectContract).OverrideCreator = new ObjectConstructor<object>(customConverter.ObjectCreator);

                      
                    }
                    else if (contract is JsonArrayContract)
                        contract.Converter = new JsonConverterTypeScript(contract as JsonArrayContract,SerializeSession, ServerSessionPart, CachingMetadata, RootArgsTypes);
                    else if (contract is JsonPrimitiveContract)
                    {
                        string path = "";
                        if (JsonContructType == JsonContractType.Serialize && SerializeSession.JsonReader != null)
                            path = SerializeSession.JsonWriter.Path;

                        if (JsonContructType == JsonContractType.Deserialize && SerializeSession.JsonReader != null)
                            path = SerializeSession.JsonReader.Path;

                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            if (path.LastIndexOf("key") == path.Length - "key".Length)
                                contract.Converter = new JsonConverterTypeScript(contract as JsonPrimitiveContract,SerializeSession, ServerSessionPart, CachingMetadata, RootArgsTypes);
                            if (path.LastIndexOf("value") == path.Length - "value".Length)
                                contract.Converter = new JsonConverterTypeScript(contract as JsonPrimitiveContract,SerializeSession, ServerSessionPart, CachingMetadata, RootArgsTypes);
                        }



                        //contract.Converter = new JsonConverterTypeScript(contract as JsonPrimitiveContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, RootArgsTypes);
                    }
                    else if (contract is JsonDictionaryContract)
                        contract.Converter = new JsonConverterTypeScript(contract as JsonDictionaryContract,SerializeSession, ServerSessionPart, CachingMetadata, RootArgsTypes);
                }
                #endregion

            }
            
            return contract;
        }

    }

}
