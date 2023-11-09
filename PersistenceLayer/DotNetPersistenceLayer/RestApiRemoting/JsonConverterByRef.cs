using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
//using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.Json.Linq;
using OOAdvantech.Json.Serialization;
using OOAdvantech.Json.Utilities;
namespace OOAdvantech.Remoting.RestApi
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
            bool isRoot = string.IsNullOrWhiteSpace(reader.Path);
            if (reader.Path != null && reader.Path.IndexOf("$value.MenuCanvasItems.$values[2].$value.MenuItem.$value.ItemOptions.$values[0].$value.Options.$values[0].$value.PricedSubjects.$values[0]") != -1)
            {

            }
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

                        if (typeFullName == null)
                        {

                        }

                        string typeName;
                        string assemblyName;

#if DeviceDotNet
                        var typeNameKey = OOAdvantech.Json.Utilities.ReflectionUtils.SplitFullyQualifiedTypeName(typeFullName);

                        typeName = typeNameKey.TypeName;
                        assemblyName = typeNameKey.AssemblyName;
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
                            string @ref = serializer.ReferenceResolver.GetReference(serializer, value);

                            string sessionChannelUri = null;
                            if (ServerSessionPart != null)
                                sessionChannelUri = ServerSessionPart.ChannelUri;


                            //sessionChannelUri =ChannelUri;
                            //if (!string.IsNullOrWhiteSpace(InternalChannelUri))
                            //    sessionChannelUri += "(" + InternalChannelUri + ")";

                            if (objRef.ChannelUri == sessionChannelUri)
                            {
                                var extObjectUri = ExtObjectUri.Parse(objRef.Uri, ServerSessionPart.ServerProcessIdentity);
                                //value = MethodCallMessage.GetObjectFromUri(extObjectUri);
                                value = ServerSessionPart.GetObjectFromUri(extObjectUri);

                                if (value == null)
                                    throw new System.Exception("The object with ObjUri '" + extObjectUri.TransientUri + "' has been disconnected or does not exist at the server.");
                            }
                            else
                            {
                                OOAdvantech.Remoting.ClientSessionPart clientSessionPart = RenewalManager.GetSession(objRef.ChannelUri, true, RemotingServices.CurrentRemotingServices);
                                OOAdvantech.Remoting.RestApi.Proxy restapiProxy = null;
                                lock (clientSessionPart)
                                {
                                    restapiProxy = clientSessionPart.GetProxy(objRef.Uri) as Proxy;
                                    if (restapiProxy == null)
                                    {
                                        restapiProxy = new Proxy(objRef);// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer));
                                        restapiProxy.ControlRemoteObjectLifeTime();
                                    }
                                    value = restapiProxy.GetTransparentProxy(objectType);
                                }
                            }
                            //  string @ref = serializer.ReferenceResolver.GetReference(serializer, value);
                        }
                        #endregion



                        reader.Read();

                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "$value")
                    {
                        if (reader.Path == "object►$value►MenuCanvasItems►$values►0►$value►Accent►$value►Heading►$value")
                        {

                        }
                        reader.Read();
                        if (isReference)
                            value = GetValueFromReference(reader, serializer);
                        else
                        {
                            if (specifiedType == null)
                                specifiedType = objectType;

                            value = GetPropertyValue(reader, specifiedType, serializer);

                            if (this.JsonPrimitiveContract != null)
                            {
                                if (specifiedType != objectType)
                                {

                                }
                                value = EnsureType(reader, value, CultureInfo.InvariantCulture, Contruct, objectType);
                            }
                        }

                        #region convert objectRef to transparentProxy
                        if (value is ObjRef)
                        {
                            ObjRef objRef = value as ObjRef;
                            if (objectType != typeof(ObjRef))
                            {


                                string @ref = serializer.ReferenceResolver.GetReference(serializer, value);

                                string sessionChannelUri = null;
                                if (ServerSessionPart != null)
                                    sessionChannelUri = ServerSessionPart.ChannelUri;


                                //sessionChannelUri =ChannelUri;
                                //if (!string.IsNullOrWhiteSpace(InternalChannelUri))
                                //    sessionChannelUri += "(" + InternalChannelUri + ")";

                                if (objRef.ChannelUri == sessionChannelUri)
                                {
                                    var extObjectUri = ExtObjectUri.Parse(objRef.Uri, ServerSessionPart.ServerProcessIdentity);
                                    //value = MethodCallMessage.GetObjectFromUri(extObjectUri);
                                    value = ServerSessionPart.GetObjectFromUri(extObjectUri);

                                    if (value == null)
                                        throw new System.Exception("The object with ObjUri '" + extObjectUri.TransientUri + "' has been disconnected or does not exist at the server.");
                                }
                                else
                                {

                                    OOAdvantech.Remoting.RestApi.Proxy restapiProxy = null;
                                    //if (objRef.TypeMetaData.Name == typeof(ServerSessionPart).Name)
                                    //{

                                    //    restapiProxy = new Proxy(objRef);// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer));
                                    //    //restapiProxy.ControlRemoteObjectLifeTime();
                                    //    value = restapiProxy.GetTransparentProxy(typeof(ServerSessionPart));
                                    //}
                                    //else
                                    {
                                        OOAdvantech.Remoting.ClientSessionPart clientSessionPart = RenewalManager.GetSession(objRef.ChannelUri, true, RemotingServices.CurrentRemotingServices);

                                        lock (clientSessionPart)
                                        {
                                            restapiProxy = clientSessionPart.GetProxy(objRef.Uri) as Proxy;
                                            if (restapiProxy == null)
                                            {
                                                restapiProxy = new Proxy(objRef);// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer));
                                                restapiProxy.ControlRemoteObjectLifeTime();
                                            }
                                            value = restapiProxy.GetTransparentProxy(objectType);
                                        }
                                    }
                                }
                                //  string @ref = serializer.ReferenceResolver.GetReference(serializer, value);
                            }
                        }
                        #endregion

                        reader.Read();
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "$values")
                    {

                        #region Read a collection  
                        if (specifiedType == typeof(Dictionary<,>))
                        {
                            #region Read dictionary
                            System.Collections.IDictionary dictionary = null;
                            if (DictionaryContract != null)
                                dictionary = DictionaryContract.DefaultCreator() as System.Collections.IDictionary;
                            else
                                dictionary = new Dictionary<object, object>();
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
                                    var keyType = objectType.GetGenericArguments()[0];
                                    reader.Read();
                                    key = GetPropertyValue(reader, keyType, serializer);
                                    reader.Read();
                                }
                                if (reader.TokenType == JsonToken.PropertyName && (reader.Value as string) == "value")
                                {
                                    var valueType = objectType.GetGenericArguments()[1];
                                    reader.Read();
                                    _obj = GetPropertyValue(reader, valueType, serializer);
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
                                #region read general object array with types
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
                                #region read typed collection
                                if (objectType != null && objectType.GetMetaData().IsGenericType && objectType.GetGenericTypeDefinition() == typeof(List<>))
                                    specifiedType = objectType;

                                if (objectType != null && objectType.GetMetaData().IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(IList<>)))
                                    specifiedType = typeof(List<>).MakeGenericType(objectType.GetMetaData().GetGenericArguments());

                                if (objectType.IsArray)
                                    specifiedType = objectType;

                                reader.Read();//start Array
                                value = GetPropertyValue(reader, specifiedType, serializer);
                                reader.Read(); // End of array
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
                //reader.Read();
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

                    return ConvertUtils.ConvertOrCast(value, culture, contract.NonNullableUnderlyingType);
                }
                catch (Exception ex)
                {
                    throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.FormatValueForPrint(value), targetType), ex);
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
                    {
                        return false;
                    }

                    if (entries.Length > 0 && entries[entries.Length - 1] == "$values" && ArrayContruct != null)
                    {
                        return false;
                    }
                    if (entries.Length > 0 && entries[entries.Length - 1] == "$values" && DictionaryContract != null)
                    {
                        return false;
                    }
                }
                //if (this.JsonPrimitiveContract != null)
                //    return false;

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

                    //if (entries.Length > 0 && entries[entries.Length - 1] == "$values" && ArrayContruct != null)
                    //    return false;

                    //if (entries.Length > 0 && entries[entries.Length - 1] == "$values" && DictionaryContract != null)
                    //    return false;


                    if (entries.Length > 0 && entries[entries.Length - 1] == "$value" && JsonPrimitiveContract != null)
                        return false;

                }

                return base.CanWrite;
            }
        }


        public object GetObjectRefValue(object _obj)
        {
            string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(_obj as MarshalByRefObject);

            if (uri == null)
            {
                var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(_obj as MarshalByRefObject) as OOAdvantech.Remoting.RestApi.Proxy;
                if (proxy != null)
                {
                    if (ServerSessionPart != null)
                    {
                        return ServerSessionPart.TransformToPublic(proxy.ObjectRef);
                        //return GetObjectRef(ServerSessionPart.TransformToPublic(proxy.ObjectRef));
                    }
                    else
                    {
                        return proxy.ObjectRef;
                        //return GetObjectRef(proxy.ObjectRef);
                    }

                }
                uri = System.Runtime.Remoting.RemotingServices.Marshal(_obj as MarshalByRefObject).URI;
                //System.Runtime.Remoting.RemotingServices.Unmarshal()
            }
            OOAdvantech.MetaDataRepository.ProxyType httpProxyType = null;
            Type type = _obj.GetType();
            bool typeAlreadyMarshaled = false;
            lock (ServerSessionPart)
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

            ObjRef byref = new ObjRef(uri, ServerSessionPart.ChannelUri, ServerSessionPart.InternalChannelUri, _obj.GetType().AssemblyQualifiedName, httpProxyType);
            byref.CachingObjectMemberValues(_obj);
            if (typeAlreadyMarshaled)
                byref.TypeMetaData = null;

            return byref;
            //return GetObjectRef(byref);

        }

        private static JsonSerializeObjtRef GetObjectRef(ObjRef _obj)
        {
            JToken token = null;
            Type type = _obj.GetType();
            JsonSerializeObjtRef objectRef = new JsonSerializeObjtRef();
            OOAdvantech.Json.Serialization.Proxy proxy = new OOAdvantech.Json.Serialization.Proxy();
            objectRef.Value = proxy;
            proxy.RealObject = _obj;
            return objectRef;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            SerializeSession.JsonWriter = writer;
            SerializeSession.JsonSerializer = serializer;

            if (writer.Path.Split('.').Last() == "$id")
            {
                JsonWriter.WriteValue(writer, ConvertUtils.GetTypeCode(value.GetType()), value);
                return;
            }
            if (writer.Path.Split('.').Last() == "User")
            {

            }
                if (ObjectContruct != null)
            {
                if (value is MarshalByRefObject)
                {
                    value = GetObjectRefValue(value);
                    serializer.Serialize(writer, value);
                    return;
                }

                writer.WriteStartObject();

                IReferenceResolver referenceResolver = serializer.ReferenceResolver;

                bool isReferenced = false;
                if (referenceResolver is ReferenceResolver)
                    isReferenced = (referenceResolver as ReferenceResolver).IsReferencedTs(serializer, value);
                else
                    isReferenced = referenceResolver.IsReferenced(serializer, value);

                JObject jObject = new JObject();
                JProperty typeProperty = null;
                //                if (!TypeEx)
                //                {
                //                    if (isReferenced)
                //                        typeProperty = new JProperty("__type", "ref");
                //                    else
                //                    {


                //#if DeviceDotNet
                //                    string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
                //#else
                //                        string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);
                //#endif
                //                        typeProperty = new JProperty("__type", typeName);
                //                    }


                //                    typeProperty.WriteTo(writer);
                //                }
                //if (TypeEx)
                {
                    JsonType jsonType = null;

#if DeviceDotNet
                    jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
#else
                    jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);
#endif


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


                JsonProperty valueProperty = new JsonProperty() { PropertyName = "$value" };




                if (isReferenced)
                {
                    //                    if (!TypeEx)
                    //                    {

                    //                        valueProperty.WritePropertyName(writer);
                    //                        writer.WriteStartObject();
                    //#if DeviceDotNet
                    //                    string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
                    //#else
                    //                        string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);
                    //#endif
                    //                        typeProperty = new JProperty("__type", typeName);
                    //                        typeProperty.WriteTo(writer);

                    //                        int refIndex = int.Parse(serializer.ReferenceResolver.GetReference(serializer, value));
                    //                        var indexProperty = new JProperty("index", refIndex);
                    //                        indexProperty.WriteTo(writer);


                    //                        writer.WriteEndObject();
                    //                    }
                    //                    else
                    {
                        if (value is System.MarshalByRefObject || value is ITransparentProxy)
                        {

                        }

                            int refIndex = int.Parse(serializer.ReferenceResolver.GetReference(serializer, value));
                        var indexProperty = new JProperty("ref", refIndex);
                        indexProperty.WriteTo(writer);
                    }
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
                    JsonContract contr = serializer.ContractResolver.ResolveContract(value.GetType());
                    if (contr is JsonPrimitiveContract)
                    {
                        writer.WriteStartObject();


                        IReferenceResolver referenceResolver = serializer.ReferenceResolver;
                        // if (TypeEx)
                        {

                            JsonType jsonType = null;
#if DeviceDotNet
                            if (value.GetType().GetMetaData().IsEnum)
                                jsonType = JsonType.GetJsonType(typeof(int), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
                            else
                                jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);

#else
                            if (value.GetType().GetMetaData().IsEnum)
                                jsonType = JsonType.GetJsonType(typeof(int), serializer._typeNameAssemblyFormat, serializer.Binder);
                            else
                                jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);

#endif


                            if (!(referenceResolver as ReferenceResolver).IsReferencedTs(serializer, jsonType))
                            {

                                //new JProperty("__type", ).WriteTo(writer);
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
                            //writer.WriteStartObject();
                            //new JProperty("__type", typeName);
                            //"JsonTypeName"
                        }
                        //                        else
                        //                        {
                        //#if DeviceDotNet
                        //                            string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
                        //#else
                        //                            string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);
                        //#endif
                        //                            JProperty typeProperty = new JProperty("__type", typeName);
                        //                            typeProperty.WriteTo(writer);
                        //                        }

                        JsonProperty valueProperty = new JsonProperty() { PropertyName = "$value" };

                        if (value is DateTime)
                        {

                        }

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
                string jsType = null;
                JProperty typeProperty = null;

#if DeviceDotNet
                string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
#else
                string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);
#endif
                IReferenceResolver referenceResolver = serializer.ReferenceResolver;

                //if (!TypeEx)
                //{
                //    typeProperty = new JProperty("__type", typeName);
                //    typeProperty.WriteTo(writer);
                //}
                //if (TypeEx)
                {
                    JsonType jsonType = null;

#if DeviceDotNet
                    jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);

#else
                    jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);

#endif
                    //var jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);
                    if (!(referenceResolver as ReferenceResolver).IsReferencedTs(serializer, jsonType))
                    {

                        //new JProperty("__type", ).WriteTo(writer);
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
                    //writer.WriteStartObject();
                    //new JProperty("__type", typeName);
                    //"JsonTypeName"
                }


                JsonProperty valueProperty = new JsonProperty() { PropertyName = "$values" };
                valueProperty.WritePropertyName(writer);
                writer.WriteStartArray();

                foreach (var itemValue in value as System.Collections.IEnumerable)
                {
                    serializer.Serialize(writer, itemValue);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            else if (this.DictionaryContract != null)
            {
                writer.WriteStartObject();


                //#if DeviceDotNet
                //                string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);
                //#else
                //                string typeName = ReflectionUtils.GetTypeName(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);
                //#endif

                //if (TypeEx)
                {
                    IReferenceResolver referenceResolver = serializer.ReferenceResolver;

                    //   var jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);

                    JsonType jsonType = null;

#if DeviceDotNet
                    jsonType = JsonType.GetJsonType(value.GetType(), serializer.TypeNameAssemblyFormatHandling, serializer.SerializationBinder);

#else
                    jsonType = JsonType.GetJsonType(value.GetType(), serializer._typeNameAssemblyFormat, serializer.Binder);

#endif

                    if (!(referenceResolver as ReferenceResolver).IsReferencedTs(serializer, jsonType))
                    {

                        //new JProperty("__type", ).WriteTo(writer);
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
                    //writer.WriteStartObject();
                    //new JProperty("__type", typeName);
                    //"JsonTypeName"
                }
                //else
                //{
                //    JProperty typeProperty = new JProperty("__type", typeName);
                //    typeProperty.WriteTo(writer);
                //}


                JsonProperty valueProperty = new JsonProperty() { PropertyName = "$values" };
                valueProperty.WritePropertyName(writer);
                writer.WriteStartArray();

                foreach (System.Collections.DictionaryEntry itemEntry in value as System.Collections.IDictionary)
                {
                    writer.WriteStartObject();

                    JsonProperty keyProperty = new JsonProperty() { PropertyName = "key" };
                    keyProperty.WritePropertyName(writer);
                    serializer.Serialize(writer, itemEntry.Key);
                    valueProperty = new JsonProperty() { PropertyName = "value" }; //JProperty("Value", itemValue.Value);
                    valueProperty.WritePropertyName(writer);
                    serializer.Serialize(writer, itemEntry.Value);

                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                writer.WriteEndObject();



            }
            else
                serializer.Serialize(writer, value);

            ////serializer.Serialize(writer, value);
            //TransportObject transportObject = new TransportObject();
            //transportObject.__value = value;
            //transportObject.__type = string.Format("{0},{1}", value.GetType().FullName, value.GetType().Assembly.GetName().Name);
            //serializer.Serialize(writer, transportObject);
            //////serializer.Serialize()
            ////JToken token = JToken.FromObject(value,serializer);

            ////JObject jObject = new JObject();

            ////jObject.Add("__type", string.Format("{0},{1}", value.GetType().FullName, value.GetType().Assembly.GetName().Name));

            ////jObject.Add("$value", token);

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


    #region deleted code JsonConverterByRef
    ///// <MetaDataID>{bb7e89d7-1263-43ea-ae65-fe9145a7b6f6}</MetaDataID>
    //public class JsonConverterByRef : JsonConverter
    //{
    //    private string ChannelUri;
    //    private string InternalChannelUri;
    //    private ServerSessionPart ServerSessionPart;

    //    public JsonConverterByRef(string channelUri, string internalChannelUri, ServerSessionPart serverSessionPart)
    //    {
    //        this.ChannelUri = channelUri;
    //        this.InternalChannelUri = internalChannelUri;
    //        this.ServerSessionPart = serverSessionPart;
    //    }



    //    public override bool CanConvert(Type objectType)
    //    {
    //        bool isMarshalByRefObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(IExtMarshalByRefObject)));
    //        return isMarshalByRefObject;
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void WriteJson(JsonWriter writer, object _obj, JsonSerializer serializer)
    //    {

    //        JToken t = null;
    //        SerializedData serializedData = null;
    //        string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(_obj as MarshalByRefObject);
    //        if (uri == null)
    //        {
    //            var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(_obj as MarshalByRefObject) as Proxy;
    //            if (proxy != null)
    //            {
    //                serializedData = new SerializedData();
    //                serializedData.Ref = proxy.ObjectRef;
    //                t = JToken.FromObject(serializedData);
    //                t.WriteTo(writer);
    //                return;
    //                //return Newtonsoft.Json.JsonConvert.SerializeObject(serializedData, new JsonConverterByRef(channelUri, internalChannelUri, serverSessionPart));
    //            }

    //            uri = System.Runtime.Remoting.RemotingServices.Marshal(_obj as MarshalByRefObject).URI;
    //        }
    //        MetaDataRepository.ProxyType httpProxyType = null;
    //        Type type = _obj.GetType();
    //        if (!ServerSessionPart.MarshalledTypes.TryGetValue(type, out httpProxyType))
    //        {
    //            httpProxyType = new MetaDataRepository.ProxyType(type);
    //            ServerSessionPart.MarshalledTypes[type] = httpProxyType;
    //        }
    //        ByRef byref = new ByRef(uri, ChannelUri, InternalChannelUri, _obj.GetType().AssemblyQualifiedName, httpProxyType);
    //        //byref.Uri = uri;
    //        //byref.ChannelUri = channelUri;
    //        //byref.ReturnTypeMetaData = httpProxyType;
    //        //byref.TypeName = _obj.GetType().AssemblyQualifiedName;
    //        byref.CachingObjectMemberValues(_obj);
    //        serializedData = new SerializedData();
    //        serializedData.Ref = byref;

    //        //SerializedData serializedData = new SerializedData();
    //        //string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(value as MarshalByRefObject);
    //        //if (uri == null)
    //        //    uri = System.Runtime.Remoting.RemotingServices.Marshal(value as MarshalByRefObject).URI;

    //        //MetaDataRepository.ProxyType httpProxyType = null;
    //        //Type type = value.GetType();
    //        ////if (!ServerSessionPart.MarshalledTypes.TryGetValue(type, out httpProxyType))
    //        ////{
    //        ////    httpProxyType = new MetaDataRepository.ProxyType(type);
    //        ////    ServerSessionPart.MarshalledTypes[type] = httpProxyType;
    //        ////}

    //        //ByRef byref = new ByRef(uri, ChannelUri, InternalChannelUri, value.GetType().AssemblyQualifiedName, httpProxyType);
    //        //serializedData.Ref = byref;
    //        t = JToken.FromObject(serializedData);
    //        t.WriteTo(writer);
    //    }
    //}

    #endregion

    /// <MetaDataID>{25378c3f-e22c-4069-bdde-c0bf43b6112e}</MetaDataID>
    public enum JsonContractType
    {
        Serialize,
        Deserialize

    }

    /// <MetaDataID>{4d05c69e-47d6-4b5f-b872-1f410806a2bf}</MetaDataID>
    public class JsonType
    {

        static Dictionary<Type, JsonType> JsonTypes = new Dictionary<Type, JsonType>();
        public string JsonTypeName { get; set; }

        public JsonType(Type type)
        {

        }

        public JsonType(Type type, string jsonTypeName) : this(type)
        {
            this.JsonTypeName = jsonTypeName;
        }

#if DeviceDotNet
        public static JsonType GetJsonType(Type type, TypeNameAssemblyFormatHandling assemblyFormat, ISerializationBinder binder)
        {
            JsonType jsonType = null;
            if (!JsonTypes.TryGetValue(type, out jsonType))
            {
                jsonType = new JsonType(type, ReflectionUtils.GetTypeName(type, assemblyFormat, binder));
                JsonTypes[type] = jsonType;
            }
            return jsonType;
        }
    }
#else
        public static JsonType GetJsonType(Type type, System.Runtime.Serialization.Formatters.FormatterAssemblyStyle assemblyFormat, System.Runtime.Serialization.SerializationBinder binder)
        {
            JsonType jsonType = null;
            if (!JsonTypes.TryGetValue(type, out jsonType))
            {
                jsonType = new JsonType(type, ReflectionUtils.GetTypeName(type, assemblyFormat, binder));
                JsonTypes[type] = jsonType;
            }
            return jsonType;
        }
    }
#endif
    /// <MetaDataID>{7411cc65-5463-4d51-8890-96ef9dba3ae6}</MetaDataID>
    public class ReferenceResolver : IReferenceResolver
    {
        IReferenceResolver InternalReferenceResolver;
        public ReferenceResolver()
        {
            InternalReferenceResolver = new OOAdvantech.Json.Serialization.DefaultReferenceResolver(); ;
        }
        public void AddReference(object context, string reference, object value)
        {
            InternalReferenceResolver.AddReference(context, reference, value);
        }

        public string GetReference(object context, object value)
        {
            string _ref = InternalReferenceResolver.GetReference(context, value);

            return (int.Parse(_ref) - 1).ToString();
        }

        public bool IsReferenced(object context, object value)
        {
            return false;
            //return InternalReferenceResolver.IsReferenced(context, value);
        }

        public object ResolveReference(object context, string reference)
        {
            reference = (int.Parse(reference) + 1).ToString();

            return InternalReferenceResolver.ResolveReference(context, reference);
        }

        internal bool IsReferencedTs(object context, object value)
        {
            return InternalReferenceResolver.IsReferenced(context, value);
        }
    }

    /// <MetaDataID>{646bd375-e44b-434b-8270-c1757c22490f}</MetaDataID>
    public class SerializeSession
    {

        public JsonReader JsonReader;
        public JsonWriter JsonWriter;
        public JsonSerializer JsonSerializer;
    }

    /// <summary>
    /// JsonContractResolver extend the functionality of json.net for Java script client and marshal byref objects
    /// </summary>
    /// <MetaDataID>{7ea5eabe-705f-4a18-b844-8e6e1fe76165}</MetaDataID>
    public class JsonContractResolver : Json.Serialization.DefaultContractResolver
    {

        SerializeSession SerializeSession = new SerializeSession();

        public int DesirializeIndex = 0;

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

        public JsonContractResolver(JsonContractType jsonContructType,/* string channelUri, string internalChannelUri,*/ ServerSessionPart serverSessionPart, JsonSerializationFormat serializationFormat = JsonSerializationFormat.NetJsonSerialization)
        {
            //TypeEx = typeEx;
            JsonContructType = jsonContructType;
            //this.ChannelUri = channelUri;
            //this.InternalChannelUri = internalChannelUri;
            this.ServerSessionPart = serverSessionPart;
            SerializationFormat = serializationFormat;
            if (SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization || SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerializationEx)
            {
                IsReference = true;
                // TypeEx = true;
            }
        }

        public JsonContractResolver(JsonContractType jsonContructType, /*string channelUri, string internalChannelUri,*/ ServerSessionPart serverSessionPart, Type[] argsTypes, JsonSerializationFormat serializationFormat = JsonSerializationFormat.NetJsonSerialization)
            : this(jsonContructType, /*channelUri, internalChannelUri,*/ serverSessionPart, serializationFormat)
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
                contract.Converter = new JsonSerializerEx(JsonContructType, SerializeSession, ServerSessionPart, SerializationFormat);
            else if (SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization|| SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerializationEx)
            {
                #region  Converter for java script client

                if (typeof(JsonType) != objectType)
                {
                    if (contract is JsonObjectContract)
                    {
                        var customConverter = new JsonConverterTypeScript(contract as JsonObjectContract, objectType, SerializeSession,/* ChannelUri, InternalChannelUri,*/ ServerSessionPart, RootArgsTypes);
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
                        contract.Converter = new JsonConverterTypeScript(contract as JsonArrayContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, RootArgsTypes);
                    else if (contract is JsonPrimitiveContract)
                        contract.Converter = new JsonConverterTypeScript(contract as JsonPrimitiveContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, RootArgsTypes);
                    else if (contract is JsonDictionaryContract)
                        contract.Converter = new JsonConverterTypeScript(contract as JsonDictionaryContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, RootArgsTypes);
                }
                #endregion

            }
            else if (SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerializationEx)
            {
                #region  Converter for java script client

                if (typeof(JsonType) != objectType)
                {
                    if (contract is JsonObjectContract)
                    {
                        var customConverter = new JsonConverterTypeScript(contract as JsonObjectContract, objectType, SerializeSession,/* ChannelUri, InternalChannelUri,*/ ServerSessionPart, RootArgsTypes);
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
                        contract.Converter = new JsonConverterTypeScript(contract as JsonArrayContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, RootArgsTypes);
                    //else if (contract is JsonPrimitiveContract)
                    //    contract.Converter = new JsonConverterTypeScript(contract as JsonPrimitiveContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, RootArgsTypes);
                    else if (contract is JsonDictionaryContract)
                        contract.Converter = new JsonConverterTypeScript(contract as JsonDictionaryContract,/* ChannelUri, InternalChannelUri,*/SerializeSession, ServerSessionPart, RootArgsTypes);
                }
                #endregion

            }

            return contract;
        }

    }

    /// <MetaDataID>{798c2871-32c1-4e06-8a54-633cc65e8091}</MetaDataID>
    public class JsonSerializerEx : OOAdvantech.Json.JsonConverter
    {
        JsonContractType JsonContructType;

        SerializeSession SerializeSession;
        //private string ChannelUri;
        //private string InternalChannelUri;
        private OOAdvantech.Remoting.RestApi.ServerSessionPart ServerSessionPart;
        JsonSerializationFormat SerializationFormat;
        public JsonSerializerEx(JsonContractType jsonContructType, SerializeSession serializeSession, OOAdvantech.Remoting.RestApi.ServerSessionPart serverSessionPart, JsonSerializationFormat serializationFormat = JsonSerializationFormat.NetJsonSerialization)
        {
            SerializeSession = serializeSession;
            JsonContructType = jsonContructType;
            //this.ChannelUri = channelUri;
            // this.InternalChannelUri = internalChannelUri;
            this.ServerSessionPart = serverSessionPart;
            SerializationFormat = serializationFormat;
        }

        public override bool CanConvert(Type objectType)
        {
            bool isMarshalByRefObject = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(IExtMarshalByRefObject)));

            isMarshalByRefObject |= OOAdvantech.MetaDataRepository.Classifier.GetClassifier(objectType).IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(OOAdvantech.Remoting.RestApi.Proxy)));

            isMarshalByRefObject = false;
            return isMarshalByRefObject;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jProxy = JObject.Load(reader);
            var jRealObject = jProxy.Property("RealObject").Value as JObject;
            var realObject = jRealObject.ToObject(typeof(object), serializer);
            if (realObject is ObjRef)
            {
                ObjRef objRef = realObject as ObjRef;

                string sessionChannelUri = null;// ChannelUri;
                if (ServerSessionPart != null)
                    sessionChannelUri = ServerSessionPart.ChannelUri;
                //if (!string.IsNullOrWhiteSpace(InternalChannelUri))
                //    sessionChannelUri += "(" + InternalChannelUri + ")";


                if (objRef.ChannelUri == sessionChannelUri || (!string.IsNullOrWhiteSpace(objRef.InternalChannelUri) && ServerSessionPart != null && objRef.InternalChannelUri == ServerSessionPart.InternalChannelUri))
                {
                    if (objRef.ChannelUri != sessionChannelUri)
                    {

                    }
                    var extObjectUri = ExtObjectUri.Parse(objRef.Uri, ServerSessionPart.ServerProcessIdentity);
                    //realObject = MethodCallMessage.GetObjectFromUri(extObjectUri);
                    realObject = ServerSessionPart.GetObjectFromUri(extObjectUri);

                    if (realObject == null)
                        throw new System.Exception("The object with ObjUri '" + extObjectUri.TransientUri + "' has been disconnected or does not exist at the server.");


                }
                else
                {


                    OOAdvantech.Remoting.ClientSessionPart clientSessionPart = RenewalManager.GetSession(objRef.ChannelUri, true, RemotingServices.CurrentRemotingServices);

                    OOAdvantech.Remoting.RestApi.Proxy restapiProxy = null;
                    lock (clientSessionPart)
                    {
                        restapiProxy = clientSessionPart.GetProxy(objRef.Uri) as Proxy;
                        if (restapiProxy == null)
                        {
                            restapiProxy = new Proxy(objRef);// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer));
                            restapiProxy.ControlRemoteObjectLifeTime();

                        }
                        else
                        {

                        }
                    }
                    realObject = restapiProxy.GetTransparentProxy(objectType);
                }
            }

            //Person per = realObject as Person;
            //per.Name = per.Name + "_1";

            OOAdvantech.Json.Serialization.Proxy proxy = new OOAdvantech.Json.Serialization.Proxy();
            proxy.RealObject = realObject;

            return proxy;
        }

        public override void WriteJson(JsonWriter writer, object _obj, JsonSerializer serializer)
        {
            string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(_obj as MarshalByRefObject);
            JToken token = null;
            if (uri == null)
            {
                var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(_obj as MarshalByRefObject) as OOAdvantech.Remoting.RestApi.Proxy;
                if (proxy != null)
                {
                    if (ServerSessionPart != null)
                        token = GetObjectRefToken(ServerSessionPart.TransformToPublic(proxy.ObjectRef), serializer);
                    else
                        token = GetObjectRefToken(proxy.ObjectRef, serializer);


                    token.WriteTo(writer);
                    return;
                }
                uri = System.Runtime.Remoting.RemotingServices.Marshal(_obj as MarshalByRefObject).URI;
                //System.Runtime.Remoting.RemotingServices.Unmarshal()
            }
            OOAdvantech.MetaDataRepository.ProxyType httpProxyType = null;
            Type type = _obj.GetType();
            bool typeAlreadyMarshaled = false;
            lock (ServerSessionPart)
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

            ObjRef byref = new ObjRef(uri, ServerSessionPart.ChannelUri, ServerSessionPart.InternalChannelUri, _obj.GetType().AssemblyQualifiedName, httpProxyType);
            byref.CachingObjectMemberValues(_obj);
            if (typeAlreadyMarshaled)
                byref.TypeMetaData = null;
            token = GetObjectRefToken(byref, serializer);
            token.WriteTo(writer);
        }

        private static JToken GetObjectRefToken(object _obj, JsonSerializer serializer)
        {


            JToken token = null;
            Type type = _obj.GetType();
            JsonSerializeObjtRef objectRef = new JsonSerializeObjtRef();
            OOAdvantech.Json.Serialization.Proxy proxy = new OOAdvantech.Json.Serialization.Proxy();
            objectRef.Value = proxy;
            token = null;
            token = JToken.FromObject(objectRef, serializer);
            JObject jObjectRef = (JObject)token;
            var jProxy = jObjectRef.Property("Value").Value as JObject;

            System.IO.StringWriter text = new System.IO.StringWriter();
            serializer.Serialize(text, _obj);

            string json = text.ToString();
            var jRealObject = JToken.Parse(json);

            //var jRealObject = JToken.FromObject(_obj);
            jProxy.Property("RealObject").Value = jRealObject;
            //string typeData = string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
            //(jRealObject as JObject).AddFirst(new JProperty("$type", typeData));

            return token;
        }
    }

    /// <MetaDataID>{2b7851b1-e8da-4690-ba7b-a85d2c577549}</MetaDataID>
    public class SerializationBinder : DefaultSerializationBinder
    {

        JsonSerializationFormat SerializationFormat;
        //bool Web;
        ThreadSafeStore<TypeNameKey, Type> _typeCache;
        public SerializationBinder(JsonSerializationFormat serializationFormat)
        {
            //  Web = web;

            SerializationFormat = serializationFormat;

            _typeCache = new ThreadSafeStore<TypeNameKey, Type>(GetTypeFromTypeNameKey);
        }
        public static Dictionary<string, Type> NamesTypesDictionary = new Dictionary<string, Type>();
        public static Dictionary<Type, string> TypesNamesDictionary = new Dictionary<Type, string>();


        private Type GetTypeFromTypeNameKey(TypeNameKey typeNameKey)
        {
            string assemblyName = typeNameKey.AssemblyName;
            string typeName = typeNameKey.TypeName;

            if (assemblyName != null)
            {
                Assembly assembly;

#if !(DOTNET || PORTABLE40 || PORTABLE)
                // look, I don't like using obsolete methods as much as you do but this is the only way
                // Assembly.Load won't check the GAC for a partial name
#pragma warning disable 618,612
                assembly = Assembly.LoadWithPartialName(assemblyName);
#pragma warning restore 618,612
#elif DOTNET || PORTABLE
                assembly = Assembly.Load(new AssemblyName(assemblyName));
#else
                assembly = Assembly.Load(assemblyName);
#endif

#if HAVE_APP_DOMAIN
                if (assembly == null)
                {
                    // will find assemblies loaded with Assembly.LoadFile outside of the main directory
                    Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly a in loadedAssemblies)
                    {
                        // check for both full name or partial name match
                        if (a.FullName == assemblyName || a.GetName().Name == assemblyName)
                        {
                            assembly = a;
                            break;
                        }
                    }
                }
#endif

                if (assembly == null)
                {
                    throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, assemblyName));
                }

                Type type = assembly.GetType(typeName);
                if (type == null)
                {
                    // if generic type, try manually parsing the type arguments for the case of dynamically loaded assemblies
                    // example generic typeName format: System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
                    if (typeName.IndexOf('`') >= 0)
                    {
                        try
                        {
                            type = GetGenericTypeFromTypeName(typeName, assembly);
                        }
                        catch (Exception ex)
                        {
                            throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName), ex);
                        }
                    }

                    if (type == null)
                    {
                        throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName));
                    }
                }

                return type;
            }
            else
            {
                return Type.GetType(typeName);
            }
        }
        private Type GetGenericTypeFromTypeName(string typeName, Assembly assembly)
        {
            Type type = null;
            int openBracketIndex = typeName.IndexOf('[');
            if (openBracketIndex >= 0)
            {
                string genericTypeDefName = typeName.Substring(0, openBracketIndex);
                Type genericTypeDef = assembly.GetType(genericTypeDefName);
                if (genericTypeDef != null)
                {
                    List<Type> genericTypeArguments = new List<Type>();
                    int scope = 0;
                    int typeArgStartIndex = 0;
                    int endIndex = typeName.Length - 1;
                    for (int i = openBracketIndex + 1; i < endIndex; ++i)
                    {
                        char current = typeName[i];
                        switch (current)
                        {
                            case '[':
                                if (scope == 0)
                                {
                                    typeArgStartIndex = i + 1;
                                }
                                ++scope;
                                break;
                            case ']':
                                --scope;
                                if (scope == 0)
                                {
                                    string typeArgAssemblyQualifiedName = typeName.Substring(typeArgStartIndex, i - typeArgStartIndex);

#if DeviceDotNet
                                    TypeNameKey typeNameKey = ReflectionUtils.SplitFullyQualifiedTypeName(typeArgAssemblyQualifiedName);
                                    genericTypeArguments.Add(BindToType(typeNameKey.AssemblyName, typeNameKey.TypeName));
#else
                                    string theTypeName;
                                    string assemblyName;
                                    ReflectionUtils.SplitFullyQualifiedTypeName(typeArgAssemblyQualifiedName, out theTypeName, out assemblyName);
                                    genericTypeArguments.Add(BindToType(assemblyName, theTypeName));
#endif
                                }
                                break;
                        }
                    }

                    type = genericTypeDef.MakeGenericType(genericTypeArguments.ToArray());
                }
            }

            return type;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            Type type = null;
            //if (assemblyName == null)
            if (NamesTypesDictionary.TryGetValue(typeName, out type))
                return type;

#if DeviceDotNet
            return _typeCache.Get(new TypeNameKey(assemblyName, typeName));
#else

            return base.BindToType(assemblyName, typeName);
#endif
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerialization || SerializationFormat == JsonSerializationFormat.TypeScriptJsonSerializationEx)
            {
                if (serializedType.GetMetaData().IsGenericType && serializedType.GetGenericTypeDefinition() == typeof(List<>))
                    serializedType = typeof(List<>);
                if (serializedType.GetMetaData().IsGenericType && serializedType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    serializedType = typeof(Dictionary<,>);
                if (TypesNamesDictionary.TryGetValue(serializedType, out typeName))
                    assemblyName = null;
                else if (serializedType.GetMetaData().IsGenericType && TypesNamesDictionary.TryGetValue(serializedType.GetGenericTypeDefinition(), out typeName))
                    assemblyName = null;
                else
                    base.BindToName(serializedType, out assemblyName, out typeName);
            }
            else
                base.BindToName(serializedType, out assemblyName, out typeName);
        }

    }



    /// <MetaDataID>{f7083664-584c-48c3-b49c-acfa86f7684a}</MetaDataID>
    internal class ThreadSafeStore<TKey, TValue>
    {
        private readonly object _lock = new object();
        private Dictionary<TKey, TValue> _store;
        private readonly Func<TKey, TValue> _creator;

        public ThreadSafeStore(Func<TKey, TValue> creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            _creator = creator;
            _store = new Dictionary<TKey, TValue>();
        }

        public TValue Get(TKey key)
        {
            TValue value;
            if (!_store.TryGetValue(key, out value))
            {
                return AddValue(key);
            }

            return value;
        }

        private TValue AddValue(TKey key)
        {
            TValue value = _creator(key);

            lock (_lock)
            {
                if (_store == null)
                {
                    _store = new Dictionary<TKey, TValue>();
                    _store[key] = value;
                }
                else
                {
                    // double check locking
                    TValue checkValue;
                    if (_store.TryGetValue(key, out checkValue))
                    {
                        return checkValue;
                    }

                    Dictionary<TKey, TValue> newStore = new Dictionary<TKey, TValue>(_store);
                    newStore[key] = value;

#if HAVE_MEMORY_BARRIER
                    Thread.MemoryBarrier();
#endif
                    _store = newStore;
                }

                return value;
            }
        }
    }


}

namespace OOAdvantech.Remoting.RestApi.EmbeddedBrowser
{
    /// <summary>
    /// Asynchronous java script call data
    /// used for calls from web browser controls
    /// </summary>
    /// <MetaDataID>{a8ce6c3d-8105-445e-84e4-5605b8b22069}</MetaDataID>
    public class JSCallData
    {
        /// <summary>
        /// Defines the Identity of call from web browser controls.
        /// </summary>
        public int CallID { get; set; }

        /// <summary>
        /// Defines the args of java script call web browser controls.
        /// </summary>
        public string Args { get; set; }


        /// <summary>
        /// Check  serialized call string for async call.
        /// </summary>
        /// <param name="serializedCall"></param>
        /// <returns></returns>
        public static bool IsAsyncCall(string serializedCall)
        {
            int nPos = serializedCall.IndexOf("\"CallID\":");
            if (nPos >= 0 && nPos < 7)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Desirialize async call data
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// Returns java script call object 
        /// </returns>
        public static JSCallData GetJSCallData(string value)
        {
            return JsonConvert.DeserializeObject<JSCallData>(value);
        }
    }



}
