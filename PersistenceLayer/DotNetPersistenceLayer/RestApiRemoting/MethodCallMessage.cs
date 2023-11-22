using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository;

using System.Reflection;

#if !NetStandard
using System.ServiceModel;
#endif

using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi.Serialization;
#if !DeviceDotNet
using System.Web;
#endif
using OOAdvantech.Json.Linq;


namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{9ed60119-d232-457f-ab2f-0f0c3292e005}</MetaDataID>
    public class MethodCallMessage
    {


        /// <MetaDataID>{a108a388-7c42-4986-8a2e-89ef83418ba9}</MetaDataID>
        internal Dictionary<string, object> CallContextDictionaryData;

        /// <MetaDataID>{d291063e-1eda-49e7-9024-e2fdc553dae8}</MetaDataID>
        public void SetCallContextData(string name, object data)
        {
            if (CallContextDictionaryData == null)
                CallContextDictionaryData = new Dictionary<string, object>();

            CallContextDictionaryData[name] = data;
        }
        /// <MetaDataID>{e3291085-9a25-484d-b749-39da483e1d17}</MetaDataID>
        public object GetCallContextData(string name)
        {

            object data = null;
            if (CallContextDictionaryData != null && CallContextDictionaryData.TryGetValue(name, out data))
                return data;
            return null;

        }


        /// <MetaDataID>{f2ab8db4-540c-4d8d-9ebe-0a5bffe0d8c7}</MetaDataID>
        string _ClientProcessIdentity;

        /// <MetaDataID>{2c7977c8-d0cf-433a-9781-2e7029ea1648}</MetaDataID>
        public string ClientProcessIdentity
        {
            get
            {
                return _ClientProcessIdentity;
            }
            set
            {
                _ClientProcessIdentity = value; ;
            }
        }

        public List<string> CachedTypes { get; set; }


        /// <MetaDataID>{e7033167-6947-4571-ace9-5e46e7d18436}</MetaDataID>
        public string ChannelUri { get; set; }



        /// <MetaDataID>{b3a09b17-8746-476c-b3da-86e03b185272}</MetaDataID>
        [JsonIgnore]
        public string InternalChannelUri
        {
            get
            {
                string publicChannelUri = null;
                string internalchannelUri = null;
                ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);
                return internalchannelUri;
            }
        }


        /// <MetaDataID>{0d3f0510-4ac3-46bc-871d-b8b2b6bfd21d}</MetaDataID>
        [JsonIgnore]
        public string PublicChannelUri
        {
            get
            {
                string publicChannelUri = null;
                string internalchannelUri = null;
                ObjRef.GetChannelUriParts(ChannelUri, out publicChannelUri, out internalchannelUri);
                return publicChannelUri;
            }
        }


        /// <MetaDataID>{1b65779f-277f-452f-b6fd-6960dd606095}</MetaDataID>
        public bool ReAuthenticate { get; set; }


        /// <MetaDataID>{cbeb39cc-843f-46b6-807c-96cd09eb485b}</MetaDataID>
        public bool Web { get; set; }


        /// <MetaDataID>{472be67a-5430-4c28-823e-361db0e0f7e2}</MetaDataID>
        public string ObjectUri { get; set; }
        /// <MetaDataID>{3fefafa8-fff3-4254-a38a-dd53344d0595}</MetaDataID>
        [JsonIgnore]
        public ExtObjectUri ExtObjectUri { get; set; }
        /// <MetaDataID>{2db194cb-805a-4551-afe5-f28ce8452558}</MetaDataID>
        public string MethodName { get; set; }

        /// <MetaDataID>{c8a44c95-d2bd-46c3-96b6-cef4ff966f38}</MetaDataID>
        public string TypeName { get; set; }


        /// <MetaDataID>{c423f46c-bcc6-4b19-a447-6910c36f0615}</MetaDataID>
        public string JsonArgs;
        /// <MetaDataID>{28f9ce63-4977-48d8-8bb3-292779dbd747}</MetaDataID>
        [JsonIgnore]
        public object[] Args;
        /// <MetaDataID>{093af4f1-d47a-47f3-a4e9-9b42bbd760fa}</MetaDataID>
        public int ArgCount;
        /// <MetaDataID>{9b38adfc-8657-4518-b502-f7b1de268ecb}</MetaDataID>
        public object Object { get; set; }
        /// <MetaDataID>{d3d9f320-6ef2-4486-8b44-d2f65b770a27}</MetaDataID>
        public MethodCallMessage()
        {
        }
        /// <MetaDataID>{a432e57d-659a-4afb-8129-71fcb2cc0921}</MetaDataID>
        internal MethodCallMessage(string channelUri, string objectUri, string clientProcessIdentity, string typeName, string methodName, object[] args)
        {
            this.ClientProcessIdentity = clientProcessIdentity;

            this.ObjectUri = objectUri;
            this.MethodName = methodName;
            this.TypeName = typeName;
            this.Args = args;
            //if (channelUri.IndexOf("(") != -1)
            //    channelUri = channelUri.Substring(0, channelUri.IndexOf("("));

            this.ChannelUri = channelUri;
            if (args != null)
                ArgCount = args.Length;

        }


        /// <MetaDataID>{45a2aa57-04ae-410c-b104-9899920cb87f}</MetaDataID>
        [JsonIgnore]
        public MethodInfo MethodInfo
        {
            get
            {
                MethodInfo methodInfo = null;

                if (MethodName == StandardActions.SetSubscriptions)
                {
                    methodInfo=typeof(ServerSessionPart).GetMethod("Subscribe", new Type[1] { typeof(System.Collections.Generic.List<RemoteEventSubscription>) });
                    return methodInfo;
                }

                if (MethodName == "0f385a2523a040a7afb8ff8549ada905")
                {
                }

                if (MethodName == "0f385a2523a040a7afb8ff8549ada905")
                    return null;
                else
                {
                    methodInfo=GetMethod(MethodDeclaringType);
                    
                    if (methodInfo != null)
                        return methodInfo;

                    if (Web&&Object is ITransparentProxy)
                    {

                        Proxy proxy = (Object as ITransparentProxy).GetProxy() as Proxy;
                        if (proxy!=null)
                        {
                            var methods = proxy.ObjectRef.GetProxyType().Methods;
                            

                            if (methods?.Contains(MethodName)==true)
                            {

                            }
                            else
                            {
                                var interfaces = proxy.ObjectRef.GetProxyType().Interfaces;
                                if (interfaces!=null)
                                {


                                    var interfacesMethods = (from _interface in interfaces
                                                             where _interface.GetNativeType()!=null
                                                             from method in _interface.GetNativeType().GetMethods()
                                                             where method.Name==MethodName
                                                             select new { _interface, method }).ToList();

                                    foreach (var interfacesMethod in interfacesMethods)
                                    {
                                        methodInfo=GetMethod(interfacesMethod._interface.GetNativeType());
                                        if (methodInfo!=null)
                                            return methodInfo;
                                    }
                                }
                            }
                        }
                    }

                }
                return null;
            }
        }

        private MethodInfo GetMethod(Type methodDeclaringType)
        {
            MethodInfo methodInfo = null;
            var classifier = Classifier.GetClassifier(methodDeclaringType);

            var operations = classifier.GetOperations(MethodName);
            if (operations.Count > 0)
            {
                foreach (var operation in operations)
                {
                    if (ArgCount == operation.Parameters.Count)
                    {
                        methodInfo = operation.GetExtensionMetaObject<MethodInfo>();
                        return methodInfo;
                    }
                    var paramsWithoutDefaultValue = operation.Parameters.Select(x => x.GetExtensionMetaObject<ParameterInfo>()).Where(x => x.DefaultValue == System.DBNull.Value).ToList().Count;
                    if (ArgCount>=paramsWithoutDefaultValue&& ArgCount <operation.Parameters.Count)
                    {
                        methodInfo = operation.GetExtensionMetaObject<MethodInfo>();
                        return methodInfo;
                    }
                }
            }
            else
            {
                foreach (var attribute in classifier.GetAttributes(true))
                {
                    var propertyInfo = attribute.GetExtensionMetaObject<PropertyInfo>();
                    if (propertyInfo != null && propertyInfo.GetGetMethod() != null)
                    {
                        string getterMethodName = propertyInfo.GetGetMethod().Name;
                        if (getterMethodName == MethodName)
                        {
                            methodInfo = propertyInfo.GetGetMethod();
                        }
                    }
                    if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    {
                        string setterMethodName = propertyInfo.GetSetMethod().Name;
                        if (setterMethodName == MethodName)
                        {
                            methodInfo = propertyInfo.GetSetMethod();
                        }
                    }

                }
                if (methodInfo == null)
                {
                    foreach (var associatioEnd in classifier.GetAssociateRoles(true))
                    {
                        var propertyInfo = associatioEnd.GetExtensionMetaObject<PropertyInfo>();
                        if (propertyInfo != null && propertyInfo.GetGetMethod() != null)
                        {
                            string getterMethodName = propertyInfo.GetGetMethod().Name;
                            if (getterMethodName == MethodName)
                            {
                                methodInfo = propertyInfo.GetGetMethod();
                            }
                        }
                        if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                        {
                            string setterMethodName = propertyInfo.GetSetMethod().Name;
                            if (setterMethodName == MethodName)
                            {
                                methodInfo = propertyInfo.GetSetMethod();
                            }
                        }

                    }
                }

            }

            return methodInfo;

        }

        /// <MetaDataID>{dd240920-d312-4cad-ba8d-81d8811a0f5a}</MetaDataID>
        [JsonIgnore]
        public System.Type MethodDeclaringType
        {
            get
            {
                Type type = null;
                if (ObjectUri != null && ObjectUri.IndexOf("type(") == 0)
                {
                    string typeUri = ObjectUri.Substring("type(".Length);
                    typeUri = typeUri.Substring(0, typeUri.IndexOf(")"));
#if DeviceDotNet
                    string[] typeUriParts = typeUri.Split('/');
#else
                    string[] typeUriParts = typeUri.Split(Path.AltDirectorySeparatorChar);
#endif


                    Serialization.SerializationBinder.NamesTypesDictionary.TryGetValue(typeUriParts[1], out type);
                    if (type== null)
                        type = Type.GetType(typeUriParts[1] + "," + typeUriParts[0]);
                    if (type == null)
                        type = Type.GetType(typeUriParts[1]);
                    return type;
                }
                if (!string.IsNullOrWhiteSpace(TypeName))
                    type = Type.GetType(TypeName);
                if (type == null && Object != null)
                    return Object.GetType();

                //TODO Να γινει test case  για την περίπτωση που χριάζεται να είναι  version Independence
                return Type.GetType(TypeName);
            }
        }

        /// <MetaDataID>{43bd3bf8-c5e9-49ec-be33-ed4127c1203b}</MetaDataID>
        [JsonIgnore]
        public bool IsRestApiManagerMethodCall
        {
            get
            {
                if (MethodName == StandardActions.CreateCommunicationSession
                    || MethodName == StandardActions.GetRemotingServicesServer
                    || MethodName == StandardActions.GetDataContext
                    || MethodName == StandardActions.GetTypesMetadata
                    || MethodName == StandardActions.RenewEventCallbackChanel
                    || MethodName == StandardActions.GetCommunicationSessionWithTypesMetadata
                    || MethodName == StandardActions.GetAccessToken
                    || MethodName == StandardActions.SetSubscriptions
                    || MethodName == StandardActions.Disconnected)
                    //|| MethodName == StandardActions.ReconfigureChannel)
                    return true;
                return false;
            }
        }

        /// <MetaDataID>{bea641bb-02c6-4c94-a3cc-ec07dbbd1aef}</MetaDataID>
        public string X_Auth_Token { get; set; }
        /// <MetaDataID>{90277fa1-3cc8-4b15-86ff-e21d92a952d7}</MetaDataID>
        public string X_Access_Token { get; set; }
        ///// <MetaDataID>{fe35f11f-a5c5-4fb5-be2b-918de66f828e}</MetaDataID>
        //static JsonSerializer CustomJsonSerializer = JsonSerializer.Create(new JsonSerializerSettings() { ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor });

        /// <MetaDataID>{e3c7a726-729c-4195-bbf5-ae7e1a3467a9}</MetaDataID>
        private void AddValuePrperties(JObject transformedObject, IEnumerable<JProperty> properties)
        {
            foreach (JProperty jProperty in properties)
            {
                if (jProperty.Value is JObject)
                {
                    if ((jProperty.Value as JObject)["$value"] is JValue)
                    {
                        transformedObject.Add(jProperty.Name, (jProperty.Value as JObject)["$value"]);
                    }
                    else
                    {
                        if (jProperty.Name == "$value")
                        {
                            AddValuePrperties(transformedObject, (jProperty.Value as JObject).Properties());
                        }
                        else
                        {
                            JObject properyObject = new JObject();
                            transformedObject.Add(jProperty.Name, properyObject);
                            AddValuePrperties(properyObject, (jProperty.Value as JObject).Properties());
                        }
                    }
                }
                else if (jProperty.Value is JArray)
                {
                    JArray transformedjArray = TransformArray(jProperty.Value as JArray);
                    transformedObject.Add(jProperty.Name, transformedjArray);
                }
                else
                    transformedObject.Add(jProperty.Name, jProperty.Value);
            }
        }

        /// <MetaDataID>{197a3d7e-ffb2-41e8-ad5a-f3207a3cbc6a}</MetaDataID>
        private JArray TransformArray(JArray jArray)
        {
            JArray transformedjArray = new JArray();
            foreach (JToken item in jArray)
            {
                if (item is JObject)
                {
                    JObject properyObject = new JObject();
                    transformedjArray.Add(properyObject);
                    AddValuePrperties(properyObject, (item as JObject).Properties());
                }
                else if (item is JArray)
                {
                    transformedjArray.Add(TransformArray(item as JArray));
                }
                else
                    transformedjArray.Add(item.DeepClone());
            }
            return transformedjArray;

        }


        /// <MetaDataID>{74b20482-61b5-4b27-873a-6bcd7631b708}</MetaDataID>
        private string TransforWebJson(string JsonArgs)
        {
            JObject jArgs = JObject.Parse(JsonArgs);
            JObject jObject = new JObject();
            AddValuePrperties(jObject, jArgs.Properties());
            JsonArgs = jObject.ToString();
            return JsonArgs;
        }
        /// <MetaDataID>{91c47f40-eed3-43fd-a8fa-b0d0f76768c1}</MetaDataID>
        internal void UnMarshal()
        {
            ServerSessionPart serverSessionPart = null;
            try
            {

                serverSessionPart = ServerSessionPart.GetServerSessionPart(Guid.Parse(ClientProcessIdentity), ChannelUri);
                if (serverSessionPart == null)
                {
                    //System.IO.File.AppendAllLines("/storage/emulated/0/test.txt", new string[] { "serverSessionPart = null " });
                    throw new EndpointNotFoundException("There is not session for ChannelUri : " + ChannelUri);
                }
                if (ObjectUri != null)
                {

                    this.ExtObjectUri = ExtObjectUri.Parse(ObjectUri, serverSessionPart.ClientProcessIdentity);

                    this.Object = serverSessionPart.GetObjectFromUri(ExtObjectUri);// GetObjectFromUri(ExtObjectUri);
                }
                else
                {
                    //System.IO.File.AppendAllLines("/storage/emulated/0/test.txt", new string[] { "ObjectUri = null " });
                    throw new Exception("Invalid Method call message object uri must be not null");

                }

                if (this.Object == null && TypeName == "OOAdvantech.Remoting.IServerSessionPart")
                    this.Object = serverSessionPart;

            }
            catch (Exception error)
            {
                //System.IO.File.AppendAllLines("/storage/emulated/0/test.txt", new string[] { "Exception 1 :  " + error.Message });
                throw;
            }

            if (this.Object == null)
                throw new MissingServerObjectException("The object with ObjUri '" + ExtObjectUri.TransientUri + "' has been disconnected or does not exist at the server.", MissingServerObjectException.MissingServerObjectReason.CollectedFromGC);




            if (MethodInfo == null)
            {
                throw new System.Exception(string.Format("method '{0}' isn't implemented ", MethodName));
            }
            try
            {

                var methodInfo = MethodInfo;
                var jsonArgs = JsonArgs;
                object[] args = UnMarshalArguments(jsonArgs, methodInfo, serverSessionPart, Web);
                Args = args;
                ArgCount = Args.Length;
            }
            catch (Exception error)
            {
                //System.IO.File.AppendAllLines("/storage/emulated/0/test.txt", new string[] { "Exception 2 :  " + error.Message });

                throw;
            }
            //var jsonArgsObjects = JArray.Parse(JsonArgs2);
            //ArgCount = jsonArgsObjects.Count;
            //var parameters = MethodInfo.GetParameters();
            //object[] unmarshalledArgs = new object[parameters.Length];
            //for (int i = 0; i != jsonArgsObjects.Count; i++)
            //{
            //    Type parameterType = parameters[i].ParameterType;
            //    if (parameters[i].ParameterType.IsByRef)
            //        parameterType = parameterType.GetElementType();
            //    try
            //    {
            //        unmarshalledArgs[i] = jsonArgsObjects[i].ToObject(parameterType, CustomJsonSerializer);
            //    }
            //    catch (Exception error)
            //    {
            //        throw;
            //    }
            //}
            //Args = unmarshalledArgs;
        }

        internal static object[] UnMarshalArguments(string jsonArgs, MethodInfo methodInfo, ServerSessionPart serverSessionPart, bool web)
        {
            int i = 0;
            var parameters = methodInfo.GetParameters();
            Type[] argsTypes = (from parameter in parameters select parameter.ParameterType).ToArray();
#if DeviceDotNet
            //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, ChannelUri, InternalChannelUri, serverSessionPart, argsTypes, Web), SerializationBinder = new OOAdvantech.Remoting.RestApi.SerializationBinder(Web) };
            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, web ? JsonSerializationFormat.TypeScriptJsonSerialization : JsonSerializationFormat.NetTypedValuesJsonSerialization, serverSessionPart,null, argsTypes);
#else
            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, web ? JsonSerializationFormat.TypeScriptJsonSerialization : JsonSerializationFormat.NetTypedValuesJsonSerialization, serverSessionPart,null, argsTypes);// { TypeNameHandling = Web ? TypeNameHandling.None : TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, ChannelUri, InternalChannelUri, serverSessionPart, argsTypes, Web), Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(Web) };
#endif
            //if (Web)
            //{
            //    //jSetttings.ReferenceResolver = new ReferenceResolver();
            //    jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
            //    jSetttings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //}

            var args = JsonConvert.DeserializeObject<object[]>(jsonArgs, jSetttings);
            foreach (var argValue in args)
            {
                Type parameterType = parameters[i].ParameterType;
                if (parameterType.GetMetaData().IsPrimitive)
                    args[i] = Convert.ChangeType(args[i], parameterType);
                else if (parameterType == typeof(decimal))
                    args[i] = Convert.ChangeType(args[i], parameterType);
                else if (parameterType.GetMetaData().IsEnum && args[i] is long)
                    args[i] = (int)(long)args[i];
                else if (parameterType.GetMetaData().IsEnum && args[i] is ulong)
                    args[i] = (int)(ulong)args[i];

                i++;
            }

            return args;
        }

        #region Removed code

        ///// <MetaDataID>{50fceb52-e782-4cf2-af71-84399bebd872}</MetaDataID>
        //internal void Unmarshall()
        //{

        //    var serverSessionPart = MessageDispatcher.ServerSessions[ClientProcessIdentity];
        //    this.ExtObjectUri = ExtObjectUri.Parse(ObjectUri, serverSessionPart.ClientProcessIdentity);

        //    this.Object = GetObjectFromUri(ExtObjectUri);
        //    var jsonArgsObjects = JArray.Parse(JsonArgs);
        //    ArgCount = jsonArgsObjects.Count;
        //    var parameters = MethodInfo.GetParameters();
        //    object[] unmarshalledArgs = new object[parameters.Length];
        //    for (int i = 0; i != jsonArgsObjects.Count; i++)
        //    {
        //        Type parameterType = parameters[i].ParameterType;
        //        if (parameters[i].ParameterType.IsByRef)
        //            parameterType = parameterType.GetElementType();
        //        try
        //        {
        //            unmarshalledArgs[i] = jsonArgsObjects[i].ToObject(parameterType, CustomJsonSerializer);
        //        }
        //        catch (Exception error)
        //        {
        //            throw;
        //        }
        //    }
        //    Args = unmarshalledArgs;
        //}

        #endregion

        /// <MetaDataID>{88f17102-8b5d-468c-afbb-3f9c35cf2d12}</MetaDataID>
        public static object GetObjectFromUri(ExtObjectUri extObjectUri)
        {
            object @object = null;
            if (Remoting.Tracker.WeakReferenceOnMarshaledObjects.ContainsKey(extObjectUri.TransientUri))
            {
                WeakReference WeakReferenceOnMarshaledObject = Remoting.Tracker.WeakReferenceOnMarshaledObjects[extObjectUri.TransientUri] as WeakReference;
                @object = WeakReferenceOnMarshaledObject.Target;
                if(@object==null)
                {

                }
            }

            if (@object == null && extObjectUri.PersistentUri != null)
            {
                string[] persistentObjectUriParts = extObjectUri.PersistentUri.Split('\\');
                string storageIdentity = persistentObjectUriParts[0];
                var storageMetaData = PersistenceLayer.StorageServerInstanceLocator.Current.GetSorageMetaData(storageIdentity);
                if (storageMetaData.MultipleObjectContext)
                {
                    var objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageMetaData.StorageName, storageMetaData.StorageLocation, storageMetaData.StorageType);
                    @object = objectStorage.GetObject(extObjectUri.PersistentUri);




                    if (@object == null)
                        throw new MissingServerObjectException("The object with ObjUri '" + extObjectUri.PersistentUri + "' has been disconnected or does not exist at the server.", MissingServerObjectException.MissingServerObjectReason.DeletedFromStorage);

                    var objRef = System.Runtime.Remoting.RemotingServices.Marshal(@object as MarshalByRefObject);
                    Remoting.Tracker.WeakReferenceOnMarshaledObjects[extObjectUri.TransientUri] = new WeakReference(@object);

                }

            }
            if (@object == null && !string.IsNullOrWhiteSpace(extObjectUri.MonoStateTypeFullName))
            {
                Type monoStateType = ModulePublisher.ClassRepository.GetType(extObjectUri.MonoStateTypeFullName, "");
                object NewInstance = MonoStateClass.GetInstance(monoStateType, true);

                if (NewInstance != null && (NewInstance as System.MarshalByRefObject) == null)
                    throw new Exception("The " + extObjectUri.MonoStateTypeFullName + " isn't type of System.MarshalByRefObject");
                @object = (IExtMarshalByRefObject)NewInstance;

            }
            else if (@object == null)
            {
                throw new MissingServerObjectException("The object with ObjUri '" + extObjectUri.TransientUri + "' has been disconnected or does not exist at the server.", MissingServerObjectException.MissingServerObjectReason.CollectedFromGC);
            }
            if (@object is TypeScriptProxy)
                @object = (@object as TypeScriptProxy).TransparentProxy;


            return @object;
        }



        /// <MetaDataID>{1ca3925e-6601-42fd-8d89-1428e024f690}</MetaDataID>
        internal object[] GetOutArgs(object[] args)
        {
            var parameters = MethodInfo.GetParameters();
            List<object> outArgs = new List<object>();
            int i = 0;
            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType.IsByRef)
                    outArgs.Add(args[i]);
                i++;
            }
            return outArgs.ToArray();
        }

        /// <MetaDataID>{cfeaf1fa-b207-44f6-91cc-1336b3f13a1b}</MetaDataID>
        internal void Marshal()
        {
            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Serialize, Web ? JsonSerializationFormat.TypeScriptJsonSerialization : JsonSerializationFormat.NetTypedValuesJsonSerialization, null,null);
            // { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Serialize, null, null, null) };
            JsonArgs = JsonConvert.SerializeObject(Args, jSetttings);
        }

        //internal string GetPublicChannelUri()
        //{
        //    if (ChannelUri.IndexOf("(") != -1)
        //        return ChannelUri.Substring(0, ChannelUri.IndexOf("("));
        //    else
        //        return ChannelUri;
        //}
    }

}
