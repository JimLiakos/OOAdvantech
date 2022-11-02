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
        JsonObjectContract ObjectContruct;
        JsonArrayContract ArrayContruct;
        //[ThreadStatic]
        //static JsonReader JsonReader;

        //[ThreadStatic]
        //static JsonWriter JsonWriter;
        //[ThreadStatic]
        //static JsonSerializer JsonSerializer;

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
        private ServerSessionPart ServerSessionPart;// = new ServerSessionPart(Guid.NewGuid());

        object ServerSessionPartLock = new object();

        Type[] RootArgsTypes;

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
        JsonConverterTypeScript(/*string channelUri, string internalChannelUri,*/SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart)
        {
            //ChannelUri = channelUri;
            //InternalChannelUri = internalChannelUri;
            ServerSessionPart = serverSessionPart;

            SerializeSession = serializeSession;

            //ExtensionDataGetter = (o) =>
            //{
            //    Dictionary<object, object> properties = new Dictionary<object, object>();
            //    properties["$id"] = JsonSerializer.GetReferenceResolver().GetReference(JsonSerializer, o);

            //    return properties;
            //};
        }
        SerializeSession SerializeSession;

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
        public JsonConverterTypeScript(JsonObjectContract contract, Type objectType, /*string channelUri, string internalChannelUri,*/SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, Type[] rootArgsTypes)
            : this(/*channelUri, internalChannelUri,*/serializeSession, serverSessionPart)
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
        public JsonConverterTypeScript(JsonArrayContract arrayContruct,/* string channelUri, string internalChannelUri,*/SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, Type[] rootArgsTypes)
            : this(/*channelUri, internalChannelUri,*/serializeSession, serverSessionPart)
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
        public JsonConverterTypeScript(JsonPrimitiveContract jsonPrimitiveContract,/* string channelUri, string internalChannelUri,*/SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, Type[] rootArgsTypes)
            : this(/*channelUri, internalChannelUri,*/serializeSession, serverSessionPart)
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
        public JsonConverterTypeScript(JsonDictionaryContract jsonDictionaryContract,/* string channelUri, string internalChannelUri,*/SerializeSession serializeSession, ServerSessionPart serverSessionPart, Type[] rootArgsTypes)
             : this(/*channelUri, internalChannelUri,*/serializeSession, serverSessionPart)
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

        Type ObjectType;

        private JsonPrimitiveContract JsonPrimitiveContract;

        private JsonDictionaryContract DictionaryContract;

        public override bool CanConvert(Type objectType)
        {
            ObjectType = objectType;

            bool isMarshalByRefObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(IExtMarshalByRefObject)));
            return !isMarshalByRefObject;

        }

        internal object ObjectCreator(params object[] args)
        {

            object createdObject = this.ObjectContruct.DefaultCreator();

            string reference = SerializeSession.JsonSerializer.ReferenceResolver.GetReference(SerializeSession.JsonSerializer, createdObject);
            return createdObject;
        }

        //static JsonSerializer Serializer;
        //private bool TypeEx;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader.Path == "$value.Shapes")
            {

            }


            bool isRoot = string.IsNullOrWhiteSpace(reader.Path);
            SerializeSession.JsonSerializer = serializer;
            SerializeSession.JsonReader = reader;
            object value = null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
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
                                specifiedType = serializer.SerializationBinder.BindToType(assemblyName, typeName);
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


                            value = GetPropertyValue(reader, valueType, serializer);

                            if (this.JsonPrimitiveContract != null)
                                value = EnsureType(reader, value, CultureInfo.InvariantCulture, Contruct, objectType);
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

        private bool IsTypeToken(JsonReader reader)
        {
            string value = reader.Value as string;
            //if (!TypeEx)
            //    return reader.TokenType == JsonToken.PropertyName && (value == "__type" || value == "$type");
            //else
            return reader.TokenType == JsonToken.PropertyName && (value == "type_ref" || value == "__type");
        }

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

        private static bool CoerceEmptyStringToNull(Type objectType, JsonContract contract, string s)
        {
            return string.IsNullOrEmpty(s) && objectType != null && objectType != typeof(string) && objectType != typeof(object) && contract != null && contract.IsNullable;
        }
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
                        _obj = new TypeScriptProxy(_obj);
                        uri = System.Runtime.Remoting.RemotingServices.Marshal(_obj as MarshalByRefObject).URI;

                        if (ServerSessionPart == null || !ServerSessionPart.MarshaledTypes.TryGetValue((proxy as Proxy).ObjectRef.TypeName, out httpProxyType))
                        {
                            httpProxyType = (proxy as Proxy).ObjectRef.TypeMetaData;
                            if (ServerSessionPart != null && httpProxyType != null)
                                ServerSessionPart.MarshaledTypes[(proxy as Proxy).ObjectRef.TypeName] = httpProxyType;

                            var objectRef = new ObjRef(uri, ServerSessionPart.ChannelUri, ServerSessionPart.InternalChannelUri, (proxy as Proxy).ObjectRef.TypeName, (proxy as Proxy).ObjectRef.TypeMetaData);

                            objectRef.MembersValues = (proxy as Proxy).ObjectRef.MembersValues;

                            return objectRef;
                        }
                        else
                        {
                            var objectRef = new ObjRef(uri, ServerSessionPart.ChannelUri, ServerSessionPart.InternalChannelUri, (proxy as Proxy).ObjectRef.TypeName, null);
                            return objectRef;
                        }

                    }
                    else
                    {
                        if (ServerSessionPart == null || !ServerSessionPart.MarshaledTypes.TryGetValue((proxy as Proxy).ObjectRef.TypeName, out httpProxyType))
                        {
                            httpProxyType = (proxy as Proxy).ObjectRef.TypeMetaData;
                            if (ServerSessionPart != null && httpProxyType != null)
                                ServerSessionPart.MarshaledTypes[(proxy as Proxy).ObjectRef.TypeName] = httpProxyType;
                            var objectRef = new ObjRef((proxy as Proxy).ObjectRef.Uri, (proxy as Proxy).ObjectRef.ChannelUri, (proxy as Proxy).ObjectRef.InternalChannelUri, (proxy as Proxy).ObjectRef.TypeName, (proxy as Proxy).ObjectRef.TypeMetaData);



                            objectRef.MembersValues = (proxy as Proxy).ObjectRef.MembersValues;

                            return objectRef;
                        }
                        else
                        {
                            var objectRef = new ObjRef((proxy as Proxy).ObjectRef.Uri, (proxy as Proxy).ObjectRef.ChannelUri, (proxy as Proxy).ObjectRef.InternalChannelUri, (proxy as Proxy).ObjectRef.TypeName, null);
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
            }

            ObjRef byref = new ObjRef(uri, serverChannelUri, internalChannelUri, _obj.GetType().AssemblyQualifiedName, httpProxyType);
            byref.CachingObjectMemberValues(_obj);
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

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IMultilingualMember)
                value = new Multilingual(value as IMultilingualMember);

            SerializeSession.JsonWriter = writer;
            SerializeSession.JsonSerializer = serializer;

            var pathEntries = writer.Path.Split('.');
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
            else if (this.DictionaryContract != null)
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
                    serializer.Serialize(writer, itemEntry.Value);

                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                #endregion

                writer.WriteEndObject();



            }
            else
                serializer.Serialize(writer, value);



        }

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
        public TypeMismatchException(string message) : base(message)
        {

        }
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
        }
    }

}
