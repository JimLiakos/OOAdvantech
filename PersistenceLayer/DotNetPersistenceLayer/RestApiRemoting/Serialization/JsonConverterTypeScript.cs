using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OOAdvantech.Json;
using OOAdvantech.Json.Linq;
using OOAdvantech.Json.Serialization;
using OOAdvantech.Json.Utilities;

namespace OOAdvantech.Remoting.RestApi.Serialization
{
    /// <summary>
    /// Defines json converter for Java script client 
    /// </summary>
    /// <MetaDataID>{8ce00d61-708f-405b-a565-a1769587a929}</MetaDataID>
    public class JsonConverterTypeScript : JsonConverter
    {
        /// <MetaDataID>{d407cff3-6c38-430d-bc94-772aaf611154}</MetaDataID>
        JsonObjectContract ObjectContruct;
        /// <MetaDataID>{110d65ec-6749-4bef-837c-6c8746c9d3bc}</MetaDataID>
        JsonArrayContract ArrayContruct;
        //[ThreadStatic]
        //static JsonReader JsonReader;

        //[ThreadStatic]
        //static JsonWriter JsonWriter;
        //[ThreadStatic]
        //static JsonSerializer JsonSerializer;

        /// <MetaDataID>{23cab944-b794-406a-b62f-965373646c50}</MetaDataID>
        JsonContract Contruct;

        ///// <summary>
        ///// Defines the communication channel uri 
        ///// </summary>
        //private string ChannelUri;
        ///// <summary>
        ///// Defines the azure internal communication channel uri
        ///// </summary>
        //private string InternalChannelUri;
        /// <summary>
        /// Defines the server session part of communication channel 
        /// </summary>
        /// <MetaDataID>{50619113-83f3-4f04-be37-10e6dc7cc473}</MetaDataID>
        private ServerSessionPart ServerSessionPart;// = new ServerSessionPart(Guid.NewGuid());

        /// <MetaDataID>{0d6c10cf-3a91-4ef2-be9f-4c9077d254c3}</MetaDataID>
        object ServerSessionPartLock = new object();

        /// <MetaDataID>{ddfbf0ee-c47c-4a37-ad98-5f8cc148516c}</MetaDataID>
        Type[] RootArgsTypes;

        CachingMetaData CachingMetadata;


        /// <summary>
        /// Defines constructor with communication channel data
        /// </summary>
        /// <param name="channelUri">
        /// Defines the communication channel uri parameter
        /// </param>
        /// <param name="internalChannelUri">
        /// Defines the azure internal communication channel uri parameter
        /// </param>
        /// <param name="serverSessionPart">
        /// Defines the server session part of communication channel parameter
        /// </param>
        /// <MetaDataID>{50d948e0-6274-4e96-b15b-2637248cf1cc}</MetaDataID>
        JsonConverterTypeScript(SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, CachingMetaData cachingMetadata)
        {
            //ChannelUri = channelUri;
            //InternalChannelUri = internalChannelUri;
            ServerSessionPart = serverSessionPart;
            CachingMetadata = cachingMetadata;

            SerializeSession = serializeSession;

            //ExtensionDataGetter = (o) =>
            //{
            //    Dictionary<object, object> properties = new Dictionary<object, object>();
            //    properties["$id"] = JsonSerializer.GetReferenceResolver().GetReference(JsonSerializer, o);

            //    return properties;
            //};
        }
        /// <MetaDataID>{3fd028b1-a31a-474b-ae93-47480c04b3d4}</MetaDataID>
        SerializeSession SerializeSession;

        //private Dictionary<string, List<string>> CachingMetadata;

        /// <summary>
        /// Defines constructor for JsonObjectContract
        /// </summary>
        /// <param name="contract">
        /// Defines the json  contract for object
        /// </param>
        /// <param name="objectType">
        /// Defines the type of object witch will be converted
        /// </param>
        /// <param name="channelUri"> 
        /// Defines the communication channel uri parameter
        /// </param>
        /// <param name="internalChannelUri">
        /// Defines the azure internal communication channel uri parameter
        /// </param>
        /// <param name="serverSessionPart">
        /// Defines the server session part of communication channel parameter
        /// </param>
        /// <param name="rootArgsTypes">
        /// defines the Types root array
        /// </param>
        /// <MetaDataID>{72dab712-850e-493b-964d-46ed05c00e9b}</MetaDataID>
        public JsonConverterTypeScript(JsonObjectContract contract, Type objectType, SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, CachingMetaData cachingMetadata, Type[] rootArgsTypes)
            : this(serializeSession, serverSessionPart, cachingMetadata)
        {
            RootArgsTypes = rootArgsTypes;
            ObjectType = objectType;
            ObjectContruct = contract;
            Contruct = contract;
            //this.TypeEx = typeEx;

        }






        /// <summary>
        /// Defines constructor for JsonObjectContract
        /// </summary>
        /// <param name="contract">
        /// Defines the json  contract for array
        /// </param>
        /// <param name="objectType">
        /// Defines the type of object witch will be converted
        /// </param>
        /// <param name="channelUri"> 
        /// Defines the communication channel uri parameter
        /// </param>
        /// <param name="internalChannelUri">
        /// Defines the azure internal communication channel uri parameter
        /// </param>
        /// <param name="serverSessionPart">
        /// Defines the server session part of communication channel parameter
        /// </param>
        /// <param name="rootArgsTypes">
        /// defines the Types root array
        /// </param>
        /// <MetaDataID>{f68ee033-d1c0-49de-995d-138c71bcc77c}</MetaDataID>
        public JsonConverterTypeScript(JsonArrayContract arrayContruct, SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, CachingMetaData cachingMetadata, Type[] rootArgsTypes)
            : this(/*channelUri, internalChannelUri,*/serializeSession, serverSessionPart, cachingMetadata)
        {
            ArrayContruct = arrayContruct;
            Contruct = arrayContruct;
            RootArgsTypes = rootArgsTypes;
            //this.TypeEx = typeEx;
        }


        /// <summary>
        /// Defines constructor for JsonObjectContract
        /// </summary>
        /// <param name="contract">
        /// Defines the json  contract for primitive value
        /// </param>
        /// <param name="objectType">
        /// Defines the type of object witch will be converted
        /// </param>
        /// <param name="channelUri"> 
        /// Defines the communication channel uri parameter
        /// </param>
        /// <param name="internalChannelUri">
        /// Defines the azure internal communication channel uri parameter
        /// </param>
        /// <param name="serverSessionPart">
        /// Defines the server session part of communication channel parameter
        /// </param>
        /// <param name="rootArgsTypes">
        /// defines the Types root array
        /// </param>
        /// <MetaDataID>{0b46ca43-605a-4429-9801-6e7d30a37aa9}</MetaDataID>
        public JsonConverterTypeScript(JsonPrimitiveContract jsonPrimitiveContract,/* string channelUri, string internalChannelUri,*/SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, CachingMetaData cachingMetadata, Type[] rootArgsTypes)
            : this(/*channelUri, internalChannelUri,*/serializeSession, serverSessionPart, cachingMetadata)
        {
            this.JsonPrimitiveContract = jsonPrimitiveContract;
            Contruct = jsonPrimitiveContract;
            RootArgsTypes = rootArgsTypes;
            //this.TypeEx = typeEx;
        }



        /// <summary>
        /// Defines constructor for JsonObjectContract
        /// </summary>
        /// <param name="contract">
        /// Defines the json  contract for dictionary
        /// </param>
        /// <param name="objectType">
        /// Defines the type of object witch will be converted
        /// </param>
        /// <param name="channelUri"> 
        /// Defines the communication channel uri parameter
        /// </param>
        /// <param name="internalChannelUri">
        /// Defines the azure internal communication channel uri parameter
        /// </param>
        /// <param name="serverSessionPart">
        /// Defines the server session part of communication channel parameter
        /// </param>
        /// <param name="rootArgsTypes">
        /// defines the Types root array
        /// </param>
        /// <MetaDataID>{2667fa29-7fcf-40b8-a034-74c38e98e3f8}</MetaDataID>
        public JsonConverterTypeScript(JsonDictionaryContract jsonDictionaryContract,/* string channelUri, string internalChannelUri,*/SerializeSession serializeSession, ServerSessionPart serverSessionPart, CachingMetaData cachingMetadata, Type[] rootArgsTypes)
             : this(/*channelUri, internalChannelUri,*/serializeSession, serverSessionPart, cachingMetadata)
        {
            this.DictionaryContract = jsonDictionaryContract;
            RootArgsTypes = rootArgsTypes;
            Contruct = jsonDictionaryContract;
            //this.TypeEx = typeEx;
        }

        //public JsonConverterTypeScript(JsonObjectContract contract, Type objectType, string channelUri, string internalChannelUri, ServerSessionPart serverSessionPart, Type[] rootArgsTypes, bool typeEx) : this(contract, objectType, channelUri, internalChannelUri, serverSessionPart, rootArgsTypes,typeEx)
        //{
        //    this.TypeEx = typeEx;
        //}

        /// <MetaDataID>{77f79c57-88d7-42fd-bf7a-33aa9c581923}</MetaDataID>
        Type ObjectType;

        /// <MetaDataID>{92206a2a-f44c-4f3c-9211-276e1ae2dfcc}</MetaDataID>
        private JsonPrimitiveContract JsonPrimitiveContract;

        /// <MetaDataID>{6dcd7ece-2bf8-436a-8231-5e2de7e14859}</MetaDataID>
        private JsonDictionaryContract DictionaryContract;

        /// <MetaDataID>{7fd580f5-ef40-4053-8386-95aa00db8bc4}</MetaDataID>
        public override bool CanConvert(Type objectType)
        {
            ObjectType = objectType;

            bool isMarshalByRefObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(IExtMarshalByRefObject)));
            return !isMarshalByRefObject;

        }

        /// <MetaDataID>{c3088a6b-4c27-4abf-bfb4-8838c1cdcd5e}</MetaDataID>
        internal object ObjectCreator(params object[] args)
        {

            object createdObject = this.ObjectContruct.DefaultCreator();

            string reference = SerializeSession.JsonSerializer.ReferenceResolver.GetReference(SerializeSession.JsonSerializer, createdObject);
            return createdObject;
        }

        //static JsonSerializer Serializer;
        //private bool TypeEx;

        /// <MetaDataID>{15415828-2f21-4008-b7b3-b51b5c9dba7a}</MetaDataID>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader.Path == "$value.Shapes")
            {

            }
            if (objectType == typeof(CachingMembers))
            {

            }


                bool isRoot = string.IsNullOrWhiteSpace(reader.Path);
            SerializeSession.JsonSerializer = serializer;
            SerializeSession.JsonReader = reader;
            object value = null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                if (reader.TokenType == JsonToken.EndObject)
                {
                    return null;
                }
                Type specifiedType = null; //defines the specified type in type token
                bool isReference = false;
                while (reader.TokenType == JsonToken.PropertyName)
                {
                    if (IsTypeToken(reader))
                    {
                        string typeFullName = GetTypeFullName(reader, serializer);
                        string typeName;
                        string assemblyName;
#if DeviceDotNet
                        var typeNameKey = OOAdvantech.Json.Utilities.ReflectionUtils.SplitFullyQualifiedTypeName(typeFullName);
#if NetStandard
                        typeName = typeNameKey.Value2;
                        assemblyName = typeNameKey.Value1;
#else
                        typeName = typeNameKey.TypeName;
                        assemblyName = typeNameKey.AssemblyName;
#endif

#elif Json4
                        var typeNameKey = OOAdvantech.Json.Utilities.ReflectionUtils.SplitFullyQualifiedTypeName(typeFullName);
                        typeName = typeNameKey.Value2;
                        assemblyName = typeNameKey.Value1;
#else

                        OOAdvantech.Json.Utilities.ReflectionUtils.SplitFullyQualifiedTypeName(typeFullName, out typeName, out assemblyName);
#endif
                        if (typeFullName == "ref")
                            isReference = true;
                        else
                        {
#if DeviceDotNet
                            //Type objectDeclareType = serializer.SerializationBinder.BindToType(assemblyName, typeName);
                            if (specifiedType == null)// || (objectDeclareType != null && objectDeclareType != specifiedType)
                            {
                                specifiedType = serializer.SerializationBinder.BindToType(assemblyName, typeName);
                                if(specifiedType==null)
                                    System.Diagnostics.Debug.Assert(false, $"missing type : {typeFullName} ");
                            }
#else
                            if (specifiedType == null)// || (objectDeclareType != null && objectDeclareType != specifiedType))
                                specifiedType = serializer.Binder.BindToType(assemblyName, typeName);
#endif
                        }
                        reader.Read();
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "ref")
                    {
                        value = GetValueFromReference(reader, serializer);


                        #region convert objectRef to transparentProxy
                        if (value is ObjRef)
                        {
                            ObjRef objRef = value as ObjRef;
                            if (objectType != typeof(ObjRef))
                                value = GetTrasparentProxy(objRef, objectType);
                        }
                        #endregion
                        reader.Read();

                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "$value")
                    {

                        reader.Read();
                        if (isReference)
                            value = GetValueFromReference(reader, serializer);
                        else
                        {
                            Type valueType = specifiedType;

                            if (valueType == null)
                                valueType = objectType;
                            if (valueType == typeof(CachingMembers))
                            {
                                var memberValues = new CachingMembers();
                                reader.Read();
                                while (reader.TokenType == JsonToken.PropertyName)
                                {
                                    string propertyName = reader.Value as string;
                                    reader.Read();
                                    object entryValue = GetPropertyValue(reader, typeof(object), serializer);
                                    reader.Read();
                                    memberValues[propertyName] = entryValue;
                                }
                                value = memberValues;

                            }
                            else
                            {


                                value = GetPropertyValue(reader, valueType, serializer);

                                if (this.JsonPrimitiveContract != null)
                                    value = EnsureType(reader, value, CultureInfo.InvariantCulture, Contruct, objectType);
                            }
                        }

                        #region convert objectRef to transparentProxy
                        if (value is ObjRef)
                        {
                            ObjRef objRef = value as ObjRef;
                            if (objectType != typeof(ObjRef))
                                value = GetTrasparentProxy(objRef, objectType);

                        }
                        #endregion

                        reader.Read();
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "$values")
                    {


                        Type valueType = specifiedType;

                        if (valueType == null)
                            valueType = objectType;



                        #region Read a collection  
                        if (valueType == typeof(CachingMembers))
                        {

                        }
                        if (valueType == typeof(Dictionary<,>) || valueType.GetMetaData().IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                        {
                            #region Reads dictionary
                            System.Collections.IDictionary dictionary = null;
                            if (SerializeSession.SerializationFormat == JsonSerializationFormat.NetTypedValuesJsonSerialization)
                                dictionary = serializer.ContractResolver.ResolveContract(valueType).DefaultCreator() as System.Collections.IDictionary;
                            else
                            {
                                if (DictionaryContract != null)
                                    dictionary = DictionaryContract.DefaultCreator() as System.Collections.IDictionary;
                                else
                                    dictionary = new Dictionary<object, object>();
                            }
                            value = dictionary;
                            reader.Read();//start Array
                            reader.Read();//Array sub item start object
                            while (reader.TokenType != JsonToken.EndArray)
                            {
                                if (reader.Path.IndexOf("MembersValues") != -1)
                                {

                                }
                                object key = null;
                                object _obj = null;
                                reader.Read();//Property
                                if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "key")
                                {
                                    var keyType = dictionary.GetType().GetGenericArguments()[0];
                                    reader.Read();
                                    key = GetPropertyValue(reader, keyType, serializer);
                                    reader.Read();
                                }
                                if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "value")
                                {
                                    var entryValueType = dictionary.GetType().GetGenericArguments()[1];
                                    reader.Read();
                                    _obj = GetPropertyValue(reader, entryValueType, serializer);
                                    reader.Read();
                                }
                                if (key?.ToString() == "Roles")
                                {

                                }
                                dictionary.Add(key, _obj);
                                reader.Read();
                            }
                            reader.Read();
                            #endregion
                        }
                        else
                        {
                            if (isRoot && RootArgsTypes != null)
                            {
                                #region Reads general object array with types
                                reader.Read();//start Array
                                reader.Read();//Array sub item
                                List<object> array = new List<object>();
                                int i = 0;
                                while (reader.TokenType != JsonToken.EndArray)
                                {
                                    object _obj = null;
                                    _obj = serializer.Deserialize(reader, RootArgsTypes[i]);
                                    i++;
                                    array.Add(_obj);
                                    reader.Read(); ;//Array next sub item
                                }
                                reader.Read();// End of array

                                if (objectType == typeof(object[]))
                                    value = array.ToArray();
                                else
                                    value = array;
                                #endregion
                            }
                            else
                            {
                                #region Reads typed collection

                                System.Collections.IList collection = null;
                                Type elementType = null;
                                Type collectionType = null;
                                if (SerializeSession.SerializationFormat == JsonSerializationFormat.NetTypedValuesJsonSerialization)
                                {
                                    if (valueType.IsArray)
                                    {
                                        collection = new List<object>();
                                        elementType = valueType.GetElementType();
                                        collectionType = valueType;
                                    }
                                    else
                                    {
                                        collection = serializer.ContractResolver.ResolveContract(valueType).DefaultCreator() as System.Collections.IList;
                                        elementType = valueType.GetGenericArguments()[0];
                                        collectionType = valueType;
                                    }
                                }
                                else
                                {
                                    if (objectType != null && objectType.GetMetaData().IsGenericType && objectType.GetGenericTypeDefinition() == typeof(List<>))
                                        valueType = objectType;

                                    if (objectType != null && objectType.GetMetaData().IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(IList<>)))
                                        valueType = typeof(List<>).MakeGenericType(objectType.GetMetaData().GetGenericArguments());

                                    if (objectType.IsArray)
                                    {
                                        valueType = objectType;
                                        collection = new List<object>();
                                        elementType = objectType.GetElementType();
                                        collectionType = objectType;
                                    }
                                    else if (valueType == typeof(List<>))
                                    {
                                        collectionType = valueType;
                                        elementType = typeof(object);
                                        collection = new List<object>();

                                    }
                                    else
                                    {
                                        collectionType = objectType;
                                        elementType = objectType.GetGenericArguments()[0];
                                        try
                                        {
                                            collection = serializer.ContractResolver.ResolveContract(valueType).DefaultCreator() as System.Collections.IList;
                                        }
                                        catch (Exception error)
                                        {
                                            throw;
                                        }
                                    }
                                }

                                value = collection;
                                reader.Read();//start Array
                                reader.Read();//Array sub item start object
                                while (reader.TokenType != JsonToken.EndArray)
                                {
                                    object _obj = GetPropertyValue(reader, elementType, serializer);
                                    collection.Add(_obj);
                                    reader.Read();
                                }
                                reader.Read();

                                if (collectionType.IsArray)
                                {
                                    Array _array = Array.CreateInstance(elementType, collection.Count);
                                    int i = 0;
                                    foreach (var element in collection)
                                        _array.SetValue(element, i++);
                                    value = _array;
                                }

                                #endregion
                            }
                        }
                        #endregion

                    }
                    else if (reader.TokenType == JsonToken.PropertyName)
                    {
                        reader.ReadAsString();
                        reader.Read();
                    }
                    else
                        reader.Read();

                }
            }
            else if (JsonPrimitiveContract != null)
            {
                value = reader.Value;
                value = EnsureType(reader, value, CultureInfo.InvariantCulture, Contruct, objectType);
            }
            return value;
        }

        /// <MetaDataID>{c5ad1a13-ad53-46c0-8dbd-119e573ca91a}</MetaDataID>
        private object GetTrasparentProxy(ObjRef objRef, Type type)
        {

            object value;
            string sessionChannelUri = null;
            if (ServerSessionPart != null)
                sessionChannelUri = ServerSessionPart.ChannelUri;

            if (objRef.ChannelUri == sessionChannelUri)
            {
                var extObjectUri = ExtObjectUri.Parse(objRef.Uri, ServerSessionPart.ServerProcessIdentity);
                value = ServerSessionPart.GetObjectFromUri(extObjectUri);

                if (value == null)
                    throw new System.Exception("The object with ObjUri '" + extObjectUri.TransientUri + "' has been disconnected or does not exist at the server.");
            }
            else
            {
                OOAdvantech.Remoting.ClientSessionPart clientSessionPart = RenewalManager.GetSession(objRef.ChannelUri, true, RemotingServices.CurrentRemotingServices);
                lock (clientSessionPart)
                {


                    value = (clientSessionPart as ClientSessionPart)?.TryGetLocalObject(objRef);
                    if (value != null)
                        return value;
                    OOAdvantech.Remoting.RestApi.Proxy proxy = clientSessionPart.GetProxy(ExtObjectUri.Parse(objRef.Uri, clientSessionPart.ServerProcessIdentity).Uri) as Proxy;
                    if (proxy == null)
                    {
                        proxy = new Proxy(objRef, type);// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer));
                        proxy.ControlRemoteObjectLifeTime();
                    }
                    else
                    {
                        proxy.ReconnectToServerObject(objRef);

                        proxy.ObjectRef.MembersValues = objRef.MembersValues;
                    }
                    value = proxy.GetTransparentProxy(type);
                }
            }

            return value;
        }

        /// <MetaDataID>{3ba26fdd-d1e0-4819-be56-337947568321}</MetaDataID>
        private string GetTypeFullName(JsonReader reader, JsonSerializer serializer)
        {
            string propertyName = reader.Value as string;
            //if (!TypeEx && (propertyName == "__type" || propertyName == "$type"))
            //    return reader.ReadAsString();

            if (propertyName == "__type")
            {
                string typeFullName = reader.ReadAsString();
                reader.Read();
                string type_id = reader.ReadAsString();

                serializer.ReferenceResolver.AddReference(serializer, type_id, typeFullName);
                return typeFullName;
            }
            if (propertyName == "type_ref")
            {
                string type_id = (int.Parse(reader.ReadAsString())).ToString();
                string typeFullName = serializer.ReferenceResolver.ResolveReference(serializer, type_id) as string;
                if (typeFullName == null)
                {
                    typeFullName = serializer.ReferenceResolver.ResolveReference(serializer, type_id) as string;
                    throw new TypeMismatchException(string.Format("Invalid type_ref '{0}'  Check the types of JSON in client and servers side. must be exactly the same.", type_id));
                }
                return typeFullName;
            }
            return "";


            //if (propertyName == "type_ref")
            //{
            //    string indexValue = reader.ReadAsString();
            //    indexValue = (int.Parse(indexValue) + 1).ToString();

            //    var value = serializer.ReferenceResolver.ResolveReference(serializer, indexValue) as JsonType;
            //}
        }

        /// <MetaDataID>{b0471beb-64b6-461f-b338-6a5fd63661ba}</MetaDataID>
        private bool IsTypeToken(JsonReader reader)
        {
            string value = reader.Value as string;
            //if (!TypeEx)
            //    return reader.TokenType == JsonToken.PropertyName && (value == "__type" || value == "$type");
            //else
            return reader.TokenType == JsonToken.PropertyName && (value == "type_ref" || value == "__type");
        }

        /// <MetaDataID>{c9465b91-32aa-4cac-8912-8de66aa2157d}</MetaDataID>
        private object GetValueFromReference(JsonReader reader, JsonSerializer serializer)
        {
            object value = null;
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    {
                        while (reader.TokenType != JsonToken.EndObject)
                        {
                            reader.Read();
                            if (reader.TokenType == JsonToken.PropertyName && ((reader.Value as string) == "__type" || (reader.Value as string) == "$type"))
                            {
                                string typeFullName = reader.ReadAsString();
                            }
                            else if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "index")
                            {

                                string indexValue = reader.ReadAsString();
                                //indexValue = (int.Parse(indexValue) + 1).ToString();

                                value = serializer.ReferenceResolver.ResolveReference(serializer, indexValue);


                            }
                        }
                        return value;
                    }
                case JsonToken.PropertyName:
                    {
                        if ((reader.Value as string) == "ref")
                        {
                            string indexValue = reader.ReadAsString();
                            value = serializer.ReferenceResolver.ResolveReference(serializer, indexValue);
                        }
                        return value;
                    }
                default:
                    throw JsonSerializationException.Create(reader, "Unexpected token while deserializing object: " + reader.TokenType);
            }
        }

        /// <MetaDataID>{1decc7ba-8a93-490c-ab67-e4a14978635a}</MetaDataID>
        private object GetPropertyValue(JsonReader reader, Type memberType, JsonSerializer serializer)
        {

            switch (reader.TokenType)
            {
                // populate a typed object or generic dictionary/array
                // depending upon whether an objectType was supplied
                case JsonToken.StartObject:
                    {
                        if (memberType == typeof(OOAdvantech.Remoting.RestApi.ObjRef))
                        {

                        }
                        var value = serializer.Deserialize(reader, memberType);

                        if (memberType == typeof(OOAdvantech.Remoting.RestApi.ObjRef))
                        {

                        }
                        //#OOAdvantech#
                        if (value is JsonSerializeObjtRef)
                            value = (value as JsonSerializeObjtRef).GetRealObject();
                        return value;
                    }
                case JsonToken.StartArray:
                    return serializer.Deserialize(reader, memberType);
                case JsonToken.Integer:
                case JsonToken.Float:
                    {
                        if (memberType == typeof(decimal))
                        {

                        }
                        return EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, Contruct, memberType);
                    }
                case JsonToken.Boolean:
                case JsonToken.Bytes:
                    return EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, Contruct, memberType);
                case JsonToken.Date:
                    {
                        var value = EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, Contruct, memberType);
                        if (value is DateTime)
                        {
                            if (((DateTime)value) == DateTime.MinValue)
                                return value;
                            if (((DateTime)value) == DateTime.MaxValue)
                                return value;
                            return ((DateTime)value).ToLocalTime();
                        }
                        return value;
                    }
                case JsonToken.String:
                    string s = (string)reader.Value;

                    // convert empty string to null automatically for nullable types
                    if (CoerceEmptyStringToNull(memberType, Contruct, s))
                    {
                        return null;
                    }

                    // string that needs to be returned as a byte array should be base 64 decoded
                    if (memberType == typeof(byte[]))
                    {
                        return Convert.FromBase64String(s);
                    }

                    return EnsureType(reader, s, CultureInfo.InvariantCulture, Contruct, memberType);
                case JsonToken.StartConstructor:
                    string constructorName = reader.Value.ToString();

                    return EnsureType(reader, constructorName, CultureInfo.InvariantCulture, Contruct, memberType);
                case JsonToken.Null:
                case JsonToken.Undefined:
#if !(DOTNET || PORTABLE40 || PORTABLE)
                    if (memberType == typeof(DBNull))
                    {
                        return DBNull.Value;
                    }
#endif

                    return EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, Contruct, memberType);
                case JsonToken.Raw:
                    return new JRaw((string)reader.Value);
                case JsonToken.Comment:
                    // ignore
                    break;
                default:
                    throw JsonSerializationException.Create(reader, "Unexpected token while deserializing object: " + reader.TokenType);
            }


            var _value = serializer.Deserialize(reader, memberType);
            return _value;
        }

        /// <MetaDataID>{fcfc9682-4570-4ea9-bf16-aabce0da0b69}</MetaDataID>
        private static bool CoerceEmptyStringToNull(Type objectType, JsonContract contract, string s)
        {
            return string.IsNullOrEmpty(s) && objectType != null && objectType != typeof(string) && objectType != typeof(object) && contract != null && contract.IsNullable;
        }
        /// <MetaDataID>{51446941-6931-4268-a507-7f415ebde45f}</MetaDataID>
        private object EnsureType(JsonReader reader, object value, CultureInfo culture, JsonContract contract, Type targetType)
        {

            if (value == null && targetType != null)
                return OOAdvantech.AccessorBuilder.GetDefaultValue(targetType);
            if (targetType == null)
            {
                return value;
            }

            Type valueType = ReflectionUtils.GetObjectType(value);

            // type of value and type of target don't match
            // attempt to convert value's type to target's type
            if (valueType != targetType)
            {
                if (value == null && contract.IsNullable)
                {
                    return null;
                }

                try
                {
                    if (contract.IsConvertable)
                    {
                        JsonPrimitiveContract primitiveContract = (JsonPrimitiveContract)contract;

                        if (contract.IsEnum)
                        {
                            if (value is string)
                            {
                                return Enum.Parse(contract.NonNullableUnderlyingType, value.ToString(), true);
                            }
                            if (ConvertUtils.IsInteger(primitiveContract.TypeCode))
                            {
                                return Enum.ToObject(contract.NonNullableUnderlyingType, value);
                            }
                        }

#if DeviceDotNet

#if HAVE_BIG_INTEGER
                        if (value is BigInteger)
                        {
                            return ConvertUtils.FromBigInteger((BigInteger)value, contract.NonNullableUnderlyingType);
                        }

#endif

#else
#if !(PORTABLE || PORTABLE40 || NET35 || NET20)

                        if (value is System.Numerics.BigInteger)
                        {
                            return ConvertUtils.FromBigInteger((System.Numerics.BigInteger)value, contract.NonNullableUnderlyingType);
                        }

#endif
#endif

                        if (targetType == typeof(System.DateTime))
                        {
                            //value = "0000-12-31T22:25:08.000Z";

                            DateTime dt = DateTime.Parse("0000-12-31T22:25:08.000Z", null, System.Globalization.DateTimeStyles.RoundtripKind);

                            if (DateTimeUtils.TryParseDateTime(new StringReference((value as string).ToCharArray(), 0, (value as string).Length), SerializeSession.JsonReader.DateTimeZoneHandling, SerializeSession.JsonReader.DateFormatString, SerializeSession.JsonReader.Culture, out dt))
                            {
                                return dt;
                            }
                            else
                                return ConvertUtils.ConvertOrCast(value, culture, contract.NonNullableUnderlyingType);

                        }

                        // this won't work when converting to a custom IConvertible
                        return Convert.ChangeType(value, contract.NonNullableUnderlyingType, culture);
                    }

                    return ConvertUtils.ConvertOrCast(value, culture, targetType);
                }
                catch (Exception ex)
                {
#if NetStandard
                    throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(value), targetType), ex);
#elif Json4
                    throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(value), targetType), ex);
#else
              throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.FormatValueForPrint(value), targetType), ex);
#endif

                }
            }

            return value;
        }


        /// <MetaDataID>{7af9a7b7-00ac-4846-8caa-a7d90f80bf2a}</MetaDataID>
        public override bool CanRead
        {
            get
            {

                if (SerializeSession.JsonReader != null)
                {
                    var entries = SerializeSession.JsonReader.Path.Split('.');

                    if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && ObjectContruct != null)
                        return false;
                    //if (entries.Length > 0 && entries[entries.Length - 1] == "$values" && ArrayContruct != null)
                    //    return false;
                    //if (entries.Length > 0 && entries[entries.Length - 1] == "$values" && DictionaryContract != null)
                    //    return false;
                    if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && ArrayContruct != null)
                        return false;
                    if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && DictionaryContract != null)
                        return false;
                    if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && JsonPrimitiveContract != null)
                        return false;
                }

                if (ObjectType == null || ObjectType == typeof(object))
                    return true;
                if (this.ObjectContruct != null)
                    return true;
                return false;
                //eturn base.CanRead;
            }
        }

        /// <MetaDataID>{70f8decb-87ee-48b1-bdd9-48514c0a0820}</MetaDataID>
        public override bool CanWrite
        {
            get
            {
                if (SerializeSession.JsonWriter != null)
                {
                    string[] entries = new string[0];
                    if (!string.IsNullOrWhiteSpace(SerializeSession.JsonWriter.Path))
                        entries = SerializeSession.JsonWriter.Path.Split('.');

                    if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && ObjectContruct != null)
                        return false;
                    if (entries.Length > 0 && entries[entries.Length - 1] == "$values" && ArrayContruct != null)
                        return true;//false;
                    if (entries.Length > 0 && entries[entries.Length - 1] == "$values" && DictionaryContract != null)
                        return true;//false;;

                    //if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && ArrayContruct != null)
                    //    return false;
                    //if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && DictionaryContract != null)
                    //    return false;

                    if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && JsonPrimitiveContract != null)
                        return false;

                }

                return base.CanWrite;
            }
        }


        /// <MetaDataID>{9f327915-9bc6-4a9f-9bac-96bdf29e6dd2}</MetaDataID>
        public ObjRef GetObjectRefValue(object _obj)
        {
            OOAdvantech.MetaDataRepository.ProxyType httpProxyType = null;
            string uri = null;
            if (_obj is ITransparentProxy)
            {
                var proxy = (_obj as ITransparentProxy).GetProxy();
                if (proxy is Proxy)
                {
                    if (ServerSessionPart?.ChannelUri == "local-device")
                    {
                        if ((proxy as Proxy).TypeScriptProxy == null)
                            (proxy as Proxy).TypeScriptProxy = new TypeScriptProxy(_obj);

                        _obj = (proxy as Proxy).TypeScriptProxy;
                        uri = System.Runtime.Remoting.RemotingServices.Marshal(_obj as MarshalByRefObject).URI;

                        if (ServerSessionPart == null || !ServerSessionPart.MarshaledTypes.TryGetValue((proxy as Proxy).ObjectRef.TypeName.FullName, out httpProxyType))
                        {
                            httpProxyType = (proxy as Proxy).ObjectRef.TypeMetaData;
                            //if (ServerSessionPart != null && httpProxyType != null)
                            //    ServerSessionPart.MarshaledTypes[(proxy as Proxy).ObjectRef.TypeName] = httpProxyType;

                            var objectRef = new ObjRef(uri, ServerSessionPart.ChannelUri, ServerSessionPart.InternalChannelUri, (proxy as Proxy).ObjectRef.TypeName.FullName, (proxy as Proxy).ObjectRef.TypeMetaData);

                            objectRef.MembersValues = (proxy as Proxy).ObjectRef.MembersValues;

                            return objectRef;
                        }
                        else
                        {
                            var objectRef = new ObjRef(uri, ServerSessionPart.ChannelUri, ServerSessionPart.InternalChannelUri, (proxy as Proxy).ObjectRef.TypeName.FullName, null);
                            objectRef.MembersValues = (proxy as Proxy).ObjectRef.MembersValues;
                            return objectRef;
                        }

                    }
                    else
                    {
                        if (ServerSessionPart == null || !ServerSessionPart.MarshaledTypes.TryGetValue((proxy as Proxy).ObjectRef.TypeName.FullName, out httpProxyType))
                        {
                            httpProxyType = (proxy as Proxy).ObjectRef.TypeMetaData;
                            //if (ServerSessionPart != null && httpProxyType != null)
                            //    ServerSessionPart.MarshaledTypes[(proxy as Proxy).ObjectRef.TypeName] = httpProxyType;
                            var objectRef = new ObjRef((proxy as Proxy).ObjectRef.Uri, (proxy as Proxy).ObjectRef.ChannelUri, (proxy as Proxy).ObjectRef.InternalChannelUri, (proxy as Proxy).ObjectRef.TypeName.FullName, (proxy as Proxy).ObjectRef.TypeMetaData);



                            objectRef.MembersValues = (proxy as Proxy).ObjectRef.MembersValues;

                            return objectRef;
                        }
                        else
                        {
                            var objectRef = new ObjRef((proxy as Proxy).ObjectRef.Uri, (proxy as Proxy).ObjectRef.ChannelUri, (proxy as Proxy).ObjectRef.InternalChannelUri, (proxy as Proxy).ObjectRef.TypeName.FullName, null);
                            return objectRef;
                        }
                    }


                }
            }
            uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(_obj as MarshalByRefObject);

            if (uri == null)
            {

                var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(_obj as MarshalByRefObject) as OOAdvantech.Remoting.RestApi.Proxy;
                if (proxy != null)
                {
                    if (ServerSessionPart != null)
                        return ServerSessionPart.TransformToPublic(proxy.ObjectRef);
                    else
                    {
                        if (this.ServerSessionPart != null && this.ServerSessionPart.ChannelUri == proxy.ObjectRef.ChannelUri)
                        {

                        }
                        return proxy.ObjectRef;
                    }

                }
                uri = System.Runtime.Remoting.RemotingServices.Marshal(_obj as MarshalByRefObject).URI;
            }

            Type type = _obj.GetType();
            bool typeAlreadyMarshaled = false;

            string serverChannelUri = null;
            string internalChannelUri = null;
            if (ServerSessionPart == null)
            {
#if !DeviceDotNet
                if (ContextMessageDispatcher.Current != null)
                {
                    serverChannelUri = RemotingServices.ServerPublicUrl + string.Format("({0})", ContextMessageDispatcher.Current.ContextID);
                    internalChannelUri = ContextMessageDispatcher.Current.ContextID;
                }
                else
                    serverChannelUri = RemotingServices.ServerPublicUrl;
#else
                serverChannelUri = RemotingServices.ServerPublicUrl;
#endif
            }
            else
            {
                serverChannelUri = ServerSessionPart.ChannelUri;
                internalChannelUri = ServerSessionPart.InternalChannelUri;
            }

            lock (ServerSessionPartLock)
            {
                if (ServerSessionPart == null || !ServerSessionPart.MarshaledTypes.TryGetValue(type.AssemblyQualifiedName, out httpProxyType))
                {
                    httpProxyType = new OOAdvantech.MetaDataRepository.ProxyType(type);
                    if (ServerSessionPart != null)
                        ServerSessionPart.MarshaledTypes[type.AssemblyQualifiedName] = httpProxyType;
                }
                else
                {
                    if (httpProxyType.Paired)
                        typeAlreadyMarshaled = true;

                }

                if (ServerSessionPart?.ChannelUri != null && ServerSessionPart?.ChannelUri != "local-device")
                    typeAlreadyMarshaled = true;

            }

            ObjRef byref = new ObjRef(uri, serverChannelUri, internalChannelUri, _obj.GetType().AssemblyQualifiedName, httpProxyType);

            byref.CachingObjectMemberValues(_obj, CachingMetadata);
            if (typeAlreadyMarshaled)
                byref.TypeMetaData = null;

            return byref;
            //return GetObjectRef(byref);

        }

        private static JsonSerializeObjtRef GetObjectRef(ObjRef _obj)
        {


            Type type = _obj.GetType();
            JsonSerializeObjtRef objectRef = new JsonSerializeObjtRef();
            OOAdvantech.Json.Serialization.Proxy proxy = new OOAdvantech.Json.Serialization.Proxy();
            objectRef.Value = proxy;
            proxy.RealObject = _obj;
            return objectRef;

        }

        /// <MetaDataID>{c3cc64fd-449d-47a7-872e-65a06811ff46}</MetaDataID>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IMultilingualMember)
                value = new Multilingual(value as IMultilingualMember);



            SerializeSession.JsonWriter = writer;
            SerializeSession.JsonSerializer = serializer;

            var pathEntries = writer.Path.Split('.');

            string last = pathEntries.Last();
            string property = null;

            if (!string.IsNullOrWhiteSpace(last) && last.IndexOf("$value") == -1 &&
                last.IndexOf("MembersValues") == -1 &&
                last.IndexOf("TypeMetaData") == -1)
            {
                property = last;
            }

            if (pathEntries.Last() == "$id")
            {
                JsonWriter.WriteValue(writer, ConvertUtils.GetTypeCode(value.GetType()), value);
                return;
            }
            if (pathEntries.Length >= 2 && pathEntries[pathEntries.Length - 2] == "$value" && JsonPrimitiveContract != null && SerializeSession.SerializationFormat == JsonSerializationFormat.NetTypedValuesJsonSerialization)
            {
                JsonWriter.WriteValue(writer, ConvertUtils.GetTypeCode(value.GetType()), value);
                return;
            }


            if (ObjectContruct != null) //|| (DictionaryContract != null && SerializeSession.SerializationFormat == JsonSerializationFormat.NetTypedValuesJsonSerialization))
            {
                try
                {

                    if (!string.IsNullOrWhiteSpace(property))
                    {
                        //if(this.SerializeSession.Path.Peek()==property)
                        //{

                        //}
                        //if (property == "value")
                        //{

                        //}
                        //if(property == "ServicePoint")
                        //{

                        //}
                        //this.SerializeSession.Path.Push(property);
                    }

                    IReferenceResolver referenceResolver = serializer.ReferenceResolver;
                    bool isReferenced = false;
                    if (value is System.MarshalByRefObject || value is ITransparentProxy)
                    {
#if DeviceDotNet
                    if (value is System.MarshalByRefObject &&!(value is OOAdvantech.Remoting.MarshalByRefObject))
                    {
                        System.Diagnostics.Debug.Fail(value.GetType().FullName + " is invalid MarshalByRefObject", "You can use OOAdvantech.Remoting.MarshalByRefObject");
                        throw new Exception(value.GetType().FullName +" is invalid MarshalByRefObject. You can use OOAdvantech.Remoting.MarshalByRefObject");
                    }
#endif
                        if (pathEntries.Length > 15)
                        {

                        }
                        if (referenceResolver is ReferenceResolver)
                        {
                            isReferenced = (referenceResolver as ReferenceResolver).IsReferencedTs(serializer, value);
                            ObjRef byref = (referenceResolver as ReferenceResolver).GetPoxyObjRef(value);
                            if (byref == null)
                            {
                                byref = GetObjectRefValue(value);
                                (referenceResolver as ReferenceResolver).AssignePoxyObjRef(value, byref);
                                serializer.Serialize(writer, byref);
                            }
                            else
                                serializer.Serialize(writer, byref);
                        }
                        else
                        {
                            value = GetObjectRefValue(value);
                            serializer.Serialize(writer, value);
                        }
                        return;
                    }

                    writer.WriteStartObject();




                    if (referenceResolver is ReferenceResolver)
                        isReferenced = (referenceResolver as ReferenceResolver).IsReferencedTs(serializer, value);
                    else
                        isReferenced = referenceResolver.IsReferenced(serializer, value);

                    JObject jObject = new JObject();
                    JsonType jsonType = null;

#if DeviceDotNet
                jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
#elif Json4
                    jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
#else
                jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder, SerializeSession);
#endif

                    WriteJsonType(writer, serializer, jsonType);

                    JsonProperty valueProperty = new JsonProperty() { PropertyName = "$value" };
                    if (isReferenced)
                    {
                        int refIndex = int.Parse(serializer.ReferenceResolver.GetReference(serializer, value));
                        var indexProperty = new JProperty("ref", refIndex);
                        indexProperty.WriteTo(writer);
                    }
                    else
                    {
                        valueProperty.WritePropertyName(writer);
                        serializer.Serialize(writer, value);
                    }

                    writer.WriteEndObject();
                }
                finally
                {
                    //if (!string.IsNullOrWhiteSpace(property))
                    //    this.SerializeSession.Path.Pop();
                }

            }
            else if (this.JsonPrimitiveContract != null)
            {
                if (value != null)
                {
                    JsonContract contruct = serializer.ContractResolver.ResolveContract(value.GetType());
                    if (contruct is JsonPrimitiveContract)
                    {
                        writer.WriteStartObject();


                        JsonType jsonType = null;
#if DeviceDotNet
                        if (value.GetType().GetMetaData().IsEnum)
                            jsonType = JsonType.GetJsonType(typeof(int), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
                        else
                            jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);

#elif Json4
                        if (value.GetType().GetMetaData().IsEnum)
                            jsonType = JsonType.GetJsonType(typeof(int), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
                        else
                            jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
#else
                        if (value.GetType().GetMetaData().IsEnum)
                            jsonType = JsonType.GetJsonType(typeof(int), serializer._typeNameAssemblyFormat, serializer.Binder, SerializeSession);
                        else
                            jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder, SerializeSession);

#endif

                        WriteJsonType(writer, serializer, jsonType);
                        JsonProperty valueProperty = new JsonProperty() { PropertyName = "$value" };

                        valueProperty.WritePropertyName(writer);
                        writer.DateFormatString = serializer.DateFormatString;
                        serializer.Serialize(writer, value);
                        writer.WriteEndObject();
                    }
                    else
                        serializer.Serialize(writer, value);
                }
                else
                    serializer.Serialize(writer, value);
            }

            else if (ArrayContruct != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(property))
                    {
                        if (property == "value")
                        {

                        }
                        if (property == "ServicePoint")
                        {

                        }
                        //this.SerializeSession.Path.Push(property);
                    }
                    writer.WriteStartObject();

#if DeviceDotNet
                string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
#elif Json4
                    string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
#else
                string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);
#endif

#if DeviceDotNet
                JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);

#elif Json4
                    JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
#else
                JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder, SerializeSession);
#endif

                    WriteJsonType(writer, serializer, jsonType);

                    #region Write values in array
                    JsonProperty valueProperty = new JsonProperty() { PropertyName = "$values" };
                    valueProperty.WritePropertyName(writer);
                    writer.WriteStartArray();

                    foreach (var itemValue in value as System.Collections.IEnumerable)
                    {
                        serializer.Serialize(writer, itemValue);
                    }
                    writer.WriteEndArray();
                    #endregion

                    writer.WriteEndObject();
                }
                finally
                {
                    //if (!string.IsNullOrWhiteSpace(property))
                    //{
                    //    this.SerializeSession.Path.Push(property);
                    //}

                }
            }
            else if (this.DictionaryContract != null)
            {




                if (value is CachingMembers)
                {
                    writer.WriteStartObject();

                    if ((value as CachingMembers).Count > 0)
                    {
#if DeviceDotNet
                JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
#elif Json4
                        JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
#else
                JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder, SerializeSession);
#endif
                        WriteJsonType(writer, serializer, jsonType);
                        ///writer.WriteStartObject();
                        if ((value as CachingMembers).Count == 8)
                        {
                        }
                        JsonProperty valueProperty = new JsonProperty() { PropertyName = "$value" };
                        valueProperty.WritePropertyName(writer);
                        writer.WriteStartObject();

                        IReferenceResolver referenceResolver = serializer.ReferenceResolver;
                        JProperty typeIdProperty = new JProperty("$id", referenceResolver.GetReference(serializer, value));
                        typeIdProperty.WriteTo(writer);

                        foreach (var entry in (value as CachingMembers))
                        {
                            valueProperty = new JsonProperty() { PropertyName = entry.Key };
                            valueProperty.WritePropertyName(writer);
                            serializer.Serialize(writer, entry.Value);
                        }
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                    else
                    {
                        writer.WriteEndObject();
                    }
                }
                else
                {
                    writer.WriteStartObject();


#if DeviceDotNet
                JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
#elif Json4
                    JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder, SerializeSession);
#else
                JsonType jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder, SerializeSession);
#endif
                    WriteJsonType(writer, serializer, jsonType);
                    #region Write key value pair in array
                    JsonProperty valueProperty = new JsonProperty() { PropertyName = "$values" };
                    valueProperty.WritePropertyName(writer);

                    writer.WriteStartArray();
                    foreach (System.Collections.DictionaryEntry itemEntry in value as System.Collections.IDictionary)
                    {
                        writer.WriteStartObject();

                        JsonProperty keyProperty = new JsonProperty() { PropertyName = "key" };
                        keyProperty.WritePropertyName(writer);
                        serializer.Serialize(writer, itemEntry.Key);
                        valueProperty = new JsonProperty() { PropertyName = "value" };
                        valueProperty.WritePropertyName(writer);
                        //if (pathEntries.Last() == "MembersValues")
                        //{
                        //    this.SerializeSession.Path.Push(itemEntry.Key.ToString());
                        //    serializer.Serialize(writer, itemEntry.Value);
                        //    this.SerializeSession.Path.Pop();
                        //}
                        //else
                        serializer.Serialize(writer, itemEntry.Value);


                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                    #endregion

                    writer.WriteEndObject();
                }
            }
            else
                serializer.Serialize(writer, value);




        }

        /// <MetaDataID>{1c911dd6-a829-4b5b-a5e5-d502f2d01d13}</MetaDataID>
        private static void WriteJsonType(JsonWriter writer, JsonSerializer serializer, JsonType jsonType)
        {
            IReferenceResolver referenceResolver = serializer.ReferenceResolver;

            if (!(referenceResolver as ReferenceResolver).IsReferencedTs(serializer, jsonType))
            {
                JProperty typeValueProperty = new JProperty("__type", jsonType.JsonTypeName);
                typeValueProperty.WriteTo(writer);
                JProperty typeIdProperty = new JProperty("type_id", referenceResolver.GetReference(serializer, jsonType));
                typeIdProperty.WriteTo(writer);

            }
            else
            {
                int refIndex = int.Parse(referenceResolver.GetReference(serializer, jsonType));
                JProperty typeValueProperty = new JProperty("type_ref", refIndex);
                typeValueProperty.WriteTo(writer);
            }
        }

        /// <MetaDataID>{bf599d0f-c643-4211-a7fa-526848e14795}</MetaDataID>
        private bool ShouldSerialize(JsonWriter writer, JsonProperty property, object target)
        {
            if (property.ShouldSerialize == null)
            {
                return true;
            }

            bool shouldSerialize = property.ShouldSerialize(target);

            //if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
            //{
            //    TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "ShouldSerialize result for property '{0}' on {1}: {2}".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, property.DeclaringType, shouldSerialize)), null);
            //}

            return shouldSerialize;
        }
        /// <MetaDataID>{1d9a8c6e-7b59-42ac-a7e8-550079cf851d}</MetaDataID>
        private bool IsSpecified(JsonWriter writer, JsonProperty property, object target)
        {
            if (property.GetIsSpecified == null)
            {
                return true;
            }

            bool isSpecified = property.GetIsSpecified(target);

            //if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
            //{
            //    TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "IsSpecified result for property '{0}' on {1}: {2}".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, property.DeclaringType, isSpecified)), null);
            //}

            return isSpecified;
        }
    }


    //public class Multilingual
    //{
    //    public Multilingual(IMultilingualMember multilingualMember)
    //    {
    //        Values = new Dictionary<string, object>();

    //        foreach (System.Collections.DictionaryEntry dictionaryEntry in multilingualMember.Values)
    //        {
    //            Values[(dictionaryEntry.Key as CultureInfo).Name] = dictionaryEntry.Value;
    //        }
    //        if (multilingualMember.DefaultLanguage != null)
    //            DefaultLanguage = multilingualMember.DefaultLanguage.Name;

    //    }
    //    public Dictionary<string, object> Values;

    //    public string DefaultLanguage;
    //}


    /// <MetaDataID>{b6994935-c6aa-47d4-a796-6b94c309ceed}</MetaDataID>
    class TypeMismatchException : Exception
    {
        /// <MetaDataID>{65428a89-be9b-41f1-acdf-6b4df6274c3f}</MetaDataID>
        public TypeMismatchException(string message) : base(message)
        {

        }
        /// <MetaDataID>{f470a9c0-fd82-447d-99cc-8b9794be23d8}</MetaDataID>
        public TypeMismatchException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    /// <MetaDataID>{bd100da9-c2b0-4ea1-9d8f-84a0cfdb7036}</MetaDataID>
    internal class TypeScriptProxy : MarshalByRefObject, IExtMarshalByRefObject
    {
        public readonly object TransparentProxy;
        public TypeScriptProxy(object transparentProxy)
        {
            TransparentProxy = transparentProxy;
            var proxy = (transparentProxy as ITransparentProxy).GetProxy();

            if (!string.IsNullOrWhiteSpace(proxy?.ObjectUri?.PersistentUri))
            {
                string uri = System.Runtime.Remoting.RemotingServices.Marshal(this as MarshalByRefObject).URI;
                PersistentObjectsTransparentProxies[uri] = this;
            }
        }

        internal static Dictionary<string, TypeScriptProxy> PersistentObjectsTransparentProxies = new Dictionary<string, TypeScriptProxy>();
    }


    public class MemberValuesJsonConverter : JsonConverter<Dictionary<string, object>>
    {

        public override Dictionary<string, object> ReadJson(JsonReader reader, Type objectType, Dictionary<string, object> existingValue, bool hasExistingValue, Json.JsonSerializer serializer)
        {

            var memberValues = new Dictionary<string, object>();

            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                while (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = reader.Value as string;
                    reader.Read();
                    object value = serializer.Deserialize(reader);
                    reader.Read();
                    memberValues[propertyName] = value;
                }

            }
            if (memberValues.Count > 0)
            {

            }
            return memberValues;
        }

        public override void WriteJson(JsonWriter writer, Dictionary<string, object> value, Json.JsonSerializer serializer)
        {
            writer.WriteStartObject();
            if (value.Count == 8)
            {

            }
            foreach (var entry in value)
            {
                JsonProperty valueProperty = new JsonProperty() { PropertyName = entry.Key };
                valueProperty.WritePropertyName(writer);
                serializer.Serialize(writer, entry.Value);
            }
            writer.WriteEndObject();

        }
    }


}
