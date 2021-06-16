using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace OOAdvantech.Remoting.RestApi
{


    /// <MetaDataID>{56c89abc-dab1-4736-be6e-e31a1390b973}</MetaDataID>
    class StandardActions
    {
        /// <MetaDataID>{68cb2a39-2c60-4672-af3d-a44a510750ea}</MetaDataID>
        public const string CreateCommunicationSession = "40f00e031643eb943a91f14651eda2";

        /// <MetaDataID>{68cb2a39-2c60-4672-af3d-a44a510750ea}</MetaDataID>
        public const string GetCommunicationSessionWithTypesMetadata = "b9db7ed621ba434798fdd416740f69a6";


        /// <MetaDataID>{846bf7cf-05b7-48d2-88cb-1ea7cd31d06b}</MetaDataID>
        public const string GetRemotingServicesServer = "3e4f8e4151e943559bea0e141eb1a11c";

        /// <MetaDataID>{ca3d4a49-63ec-490c-b668-03ccf23ce515}</MetaDataID>
        public const string GetDataContext = "b54aeaffa5ac418fabe0a4a675ec454c";

        /// <MetaDataID>{9d246883-4559-44d0-acce-70fa62eee425}</MetaDataID>
        public const string GetTypesMetadata = "e8a30a72b9564fdb9c70abd571454774";

        /// <MetaDataID>{e5921ca3-b7ac-476b-a52f-df26fedc13d3}</MetaDataID>
        public const string GetAccessToken = "7102b73b057445c6acf5f7c19e03a845";


        public const string Disconnected = "29c7fa4c167e4aeda0dd4c7af1fc2c17";


        //public const string ReconfigureChannel = "dd4c6fc00f77493594ae84f25d4c1531";


    }

    /// <MetaDataID>{11ab1538-a68d-45b5-a177-fea3c952cf93}</MetaDataID>
    public class TypesMetadataCommunicationSession
    {
        /// <MetaDataID>{22167253-017d-4963-9f68-9e655d5197bb}</MetaDataID>
        public ObjRef ServerSessionPartRef { get; set; }
        /// <MetaDataID>{2b4a8308-e36c-4e00-94ab-246aa12d6358}</MetaDataID>
        public Dictionary<string, MetaDataRepository.ProxyType> MarshaledTypes { get; set; }
    }



    /// <MetaDataID>{d1332767-0d0a-4625-8fcf-636ea0b091cd}</MetaDataID>
    public class RemotingServices : IRemotingServices
    {
        /// <MetaDataID>{c126f0d3-58ec-470e-980f-5001ce046dd5}</MetaDataID>
        static RemotingServices()
        {
            MessageDispatcher.Init();
        }
        /// <MetaDataID>{c10242d2-605e-4ca1-8d54-4c5ea270f3bb}</MetaDataID>
        internal static HttpClient client = new HttpClient();
        ///// <MetaDataID>{d3ef6771-26d5-4f0c-ac86-e39013f944d0}</MetaDataID>
        //public static System.Guid ProcessIdentity = System.Guid.NewGuid();


        /// <summary>
        /// Creates remote object
        /// </summary>
        /// <typeparam name="T">
        /// Defines the type of object
        /// </typeparam>
        /// <param name="serverUrl">
        /// Defines the server url where new object will be created
        /// </param>
        /// <returns>
        /// Returns the new created remote object
        /// </returns>
        /// <MetaDataID>{13b7e930-c27e-416a-b112-43fb279d328b}</MetaDataID>
        [MetaDataRepository.BackwardCompatibilityID("+1")]
        public static T CreateRemoteInstance<T>(string serverUrl) where T : class
        {
#if DeviceDotNet
            object remoteObject = CreateRemoteInstance(serverUrl, typeof(T).FullName, typeof(T).GetMetaData().Assembly.FullName);
            return CastTransparentProxy<T>(remoteObject);

#else
            return CreateRemoteInstance(serverUrl, typeof(T).FullName, typeof(T).GetMetaData().Assembly.FullName) as T;
#endif
        }



        /// <MetaDataID>{2f16f479-84ac-4d39-ac10-7a403ccdad68}</MetaDataID>
        public static T CastTransparentProxy<T>(object obj) where T : class
        {
#if DeviceDotNet
            if (obj is OOAdvantech.Remoting.RestApi.ITransparentProxy)
                return ((obj as OOAdvantech.Remoting.RestApi.ITransparentProxy).GetProxy() as RestApi.Proxy).GetTransparentProxy(typeof(T)) as T;
#endif
            return obj as T;
        }



#if DeviceDotNet
#endif 

        /// <summary>
        /// Creates remote object
        /// </summary>
        /// <param name="serverUrl">
        /// Defines the server url where new object will be created
        /// </param>
        /// <param name="typeFullName">
        /// Defines the type full name of remote object which will be created
        /// </param>
        /// <param name="assemblyData">
        /// Defines the assembly of type of new object
        /// </param>
        /// <returns>
        /// Returns the new created remote object
        /// </returns>
        /// <MetaDataID>{40046d38-37d4-4c73-9744-306030f5bb7e}</MetaDataID>
        public static object CreateRemoteInstance(string serverUrl, string typeFullName, string assemblyData)
        {
            try
            {
                var remotingServices = GetRemotingServices(serverUrl);
                return remotingServices.CreateInstance(typeFullName, assemblyData, new Type[0]);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        public static string GetObjectUri(MarshalByRefObject marshalByRefObject)
        {
            if (marshalByRefObject == null)
                return null;
            System.Runtime.Remoting.Proxies.RealProxy RealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(marshalByRefObject);
            if (RealProxy == null)
                return null;
#if DeviceDotNet
            if (RealProxy is IProxy)
                return (RealProxy as IProxy).ObjectUri.TransientUri;
            else
                return null;
#else
            System.Runtime.Remoting.ObjRef ObjRef = null;
            if (RealProxy is Remoting.Proxy)
                return (RealProxy as Remoting.Proxy).URI.TransientUri;
            else if (RealProxy is IProxy)
                return (RealProxy as IProxy).ObjectUri.TransientUri;
            else
                ObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(marshalByRefObject);
            return ObjRef.URI;

#endif
        }

        public static ExtObjectUri GetObjRefForProxy(object @object)
        {
            MarshalByRefObject marshalByRefObject = @object as MarshalByRefObject;
            if (marshalByRefObject == null)
            {
                if (@object is ITransparentProxy)
                    return ((@object as ITransparentProxy).GetProxy() as IProxy).ObjectUri;

                return null;
            }
            System.Runtime.Remoting.Proxies.RealProxy RealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(marshalByRefObject);
            if (RealProxy == null)
                return null;
            if (RealProxy is IProxy)
                return (RealProxy as IProxy).ObjectUri;


            return null;
        }

        public static MarshalByRefObject RefreshCacheData(MarshalByRefObject obj)
        {

            System.Runtime.Remoting.Proxies.RealProxy RealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(obj);
            if (RealProxy == null)
                return null;
            if (RealProxy is IProxy)
            {
                var remotingServices = GetRemotingServices((RealProxy as IProxy).ChannelUri);
                return remotingServices.RefreshCacheData(obj);
            }

            return obj;
        }

        public static object GetPersistentObject(string serverUrl, string persistentUri)
        {
            try
            {
                var remotingServices = GetRemotingServices(serverUrl);
                return remotingServices.GetPersistentObject(persistentUri);

                //#if DeviceDotNet
                //                return RemotingServices.CastTransparentProxy <OOAdvantech.Remoting.MarshalByRefObject>( remotingServices.GetPersistentObject(persistentUri));
                //#else
                //                return remotingServices.GetPersistentObject(persistentUri);
                //#endif
            }
            catch (System.Exception error)
            {
                throw;
            }
        }


        /// <summary>
        /// Creates remote object
        /// </summary>
        /// <typeparam name="T">
        /// Defines the type of object
        /// </typeparam>
        /// <param name="serverUrl">
        /// Defines the server url where new object will be created
        /// </param>
        /// <param name="ctorParamsTypes">
        /// Defines the types of constructor parameters
        /// </param>
        /// <param name="ctorParamsValues">
        /// Defines the constructor parameters values
        /// </param>
        /// <returns>
        /// Returns the new created remote object
        /// </returns>
        /// <MetaDataID>{e2718af6-5ab4-4b89-8a83-ab029034130a}</MetaDataID>
        [MetaDataRepository.BackwardCompatibilityID("+2")]
        public static T CreateRemoteInstance<T>(string serverUrl, Type[] ctorParamsTypes, params object[] ctorParamsValues) where T : class
        {
            return CreateRemoteInstance(serverUrl, typeof(T).FullName, typeof(T).GetMetaData().Assembly.FullName, ctorParamsTypes, ctorParamsValues) as T;
        }



        /// <summary>
        /// Creates remote object
        /// </summary>
        /// <param name="serverUrl">
        /// Defines the server url where new object will be created
        /// </param>
        /// <param name="typeFullName">
        /// Defines the type full name of remote object which will be created
        /// </param>
        /// <param name="assemblyData">
        /// Defines the assembly of type of new object
        /// </param>
        /// <param name="ctorParamsTypes">
        /// Defines the types of constructor parameters
        /// </param>
        /// <param name="ctorParamsValues">
        /// Defines the constructor parameters values
        /// </param>
        /// <returns>
        /// Returns the new created remote object
        /// </returns>
        /// <MetaDataID>{F0E86382-21D4-4504-A21A-FFBB6669E6A8}</MetaDataID>
        public static object CreateRemoteInstance(string serverUrl, string typeFullName, string assemblyData, Type[] ctorParamsTypes, params object[] ctorParamsValues)
        {
            return GetRemotingServices(serverUrl).CreateInstance(typeFullName, assemblyData, ctorParamsTypes, ctorParamsValues);
        }

        public static void GetServerSessionPartMarshaledTypes(ClientSessionPart clientSessionPart)
        {
            string channelUri = clientSessionPart.ChannelUri;

            var methodCallMessage = new MethodCallMessage(channelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", clientSessionPart.ClientProcessIdentity.ToString(), "", StandardActions.GetTypesMetadata, new object[0]);

            RequestData requestData = new RequestData();

            requestData.ChannelUri = channelUri;
            requestData.SessionIdentity = clientSessionPart.SessionIdentity;

            requestData.details = OOAdvantech.Json.JsonConvert.SerializeObject(methodCallMessage);
            requestData.RequestType = RequestType.MethodCall;

            var myJson = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);

            requestData.ChannelUri = channelUri;
            var responseData = (clientSessionPart as ClientSessionPart).Channel.ProcessRequest(requestData);
            if (responseData != null)
            {

                var returnMessage = OOAdvantech.Json.JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);
                if (returnMessage.ServerSessionObjectRef != null)
                {

                }
#if DeviceDotNet
                //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null), SerializationBinder = new OOAdvantech.Remoting.RestApi.SerializationBinder() };
                var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.NetTypedValuesJsonSerialization, null);
#else
                var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.NetTypedValuesJsonSerialization, null);// { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null), Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder() };
#endif
                var proxyTypes = JsonConvert.DeserializeObject<object>(returnMessage.ReturnObjectJson, jSetttings) as System.Collections.Generic.Dictionary<string, MetaDataRepository.ProxyType>;

                foreach (var proxyTypeEntry in proxyTypes)
                    clientSessionPart.ProxyTypes[proxyTypeEntry.Key] = proxyTypeEntry.Value;

            }

        }

        public static string SerializeObjectRef(object remoteObject)
        {
            var channelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(remoteObject);
            var objectRef = RemotingServices.GetObjRefForProxy(remoteObject);
            if (!string.IsNullOrWhiteSpace(channelUri) && objectRef != null && !string.IsNullOrWhiteSpace(objectRef.PersistentUri))
                return channelUri + ";" + objectRef.PersistentUri;
            return null;
        }

        public static T DerializeObjectRef<T>(string objectRef) where T : class
        {
            if (string.IsNullOrWhiteSpace(objectRef))
                return default;
            var objectRefParts = objectRef.Split(';');
            if (objectRefParts.Length == 2)
            {
                var @object = RemotingServices.GetPersistentObject(objectRefParts[0], objectRefParts[1]);
                if (@object == null)
                    return default;
#if DeviceDotNet
                return RemotingServices.CastTransparentProxy<T>(@object);
#else
                return (T)@object;
#endif

            }
            else
                return default;

            //var channelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(remoteObject);
            //var objectRef = RemotingServices.GetObjRefForProxy(remoteObject);
            //if (!string.IsNullOrWhiteSpace(channelUri) && objectRef != null && !string.IsNullOrWhiteSpace(objectRef.PersistentUri))
            //    return channelUri + ";" + objectRef.PersistentUri;
            //return null;
        }

        /// <MetaDataID>{fa8b03c5-0949-49bb-8950-b5ca70f21946}</MetaDataID>
        public static IRemotingServer GetRemotingServices(string channelUri)
        {
            if (channelUri.IndexOf("ServerPublicUrl") == 0)
                channelUri = channelUri.Replace("ServerPublicUrl", ServerPublicUrl);



            string publicChannelUri = null;
            string internalchannelUri = null;
            ObjRef.GetChannelUriParts(channelUri, out publicChannelUri, out internalchannelUri);

            OOAdvantech.Remoting.ClientSessionPart clientSessionPart = RenewalManager.GetSession(channelUri, true, RemotingServices.CurrentRemotingServices);
            var serverSessionProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(clientSessionPart.ServerSessionPart) as Proxy;

            channelUri = clientSessionPart.ChannelUri;

            var methodCallMessage = new MethodCallMessage(channelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", clientSessionPart.ClientProcessIdentity.ToString(), "", StandardActions.GetRemotingServicesServer, new object[0]);

            RequestData requestData = new RequestData();

            requestData.ChannelUri = channelUri;
            requestData.SessionIdentity = clientSessionPart.SessionIdentity;

            requestData.details = OOAdvantech.Json.JsonConvert.SerializeObject(methodCallMessage);
            requestData.RequestType = RequestType.MethodCall;

            var myJson = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);

            requestData.ChannelUri = channelUri;
            var responseData = (clientSessionPart as ClientSessionPart).Channel.ProcessRequest(requestData);
            //Invoke(requestData.PublicChannelUri, requestData, null, null);
            if (responseData != null)
            {
                var returnMessage = OOAdvantech.Json.JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);
                if (returnMessage.ServerSessionObjectRef != null)
                    (clientSessionPart as ClientSessionPart).UpdateServerSessionPart(returnMessage.ServerSessionObjectRef);

#if DeviceDotNet
                //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null), SerializationBinder = new OOAdvantech.Remoting.RestApi.SerializationBinder() };
                var jSetttings = new OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.NetTypedValuesJsonSerialization, null);
#else
                //var jSetttings = new OOAdvantech.Json.JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null), Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder() };

                var jSetttings = new OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.NetTypedValuesJsonSerialization, null);
#endif
                var returnValue = JsonConvert.DeserializeObject<IRemotingServer>(returnMessage.ReturnObjectJson, jSetttings);
                if (System.Runtime.Remoting.RemotingServices.GetRealProxy(returnValue) != null)
                    return Proxy.CastRemoteObject(returnValue, typeof(IRemotingServer)) as IRemotingServer;

                ObjRef remoteRef = null;
                if (returnValue is ObjRef)
                    remoteRef = returnValue as ObjRef;
                else
                {
                    SerializedData serData = OOAdvantech.Json.JsonConvert.DeserializeObject<SerializedData>(returnMessage.ReturnObjectJson);
                    remoteRef = serData.Ref;// Newtonsoft.Json.JsonConvert.DeserializeObject<SerializedData>(returnMessage.ReturnObjectJson);
                }
                Proxy proxy = null;
                lock (clientSessionPart)
                {
                    proxy = clientSessionPart.GetProxy(remoteRef.Uri) as Proxy;
                    if (proxy == null)
                    {
                        proxy = new Proxy(remoteRef);// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer));
                        proxy.ControlRemoteObjectLifeTime();
                    }
                    else
                    {
                        proxy.ReconnectToServerObject(remoteRef);

                    }
                }
                return proxy.GetTransparentProxy(typeof(IRemotingServer)) as IRemotingServer;
            }
            return null;
        }

        //private static ClientSessionPart CreateCommunicationSession(string channelUri)
        //{
        //    //40f00e031643eb943a91f14651eda2


        //    var methodCallMessage = new MethodCallMessage(channelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", "", "", StandardActions.CreateCommunicationSession, new object[0]);
        //    methodCallMessage.SessionID = RemotingServices.ProcessIdentity.ToString();
        //    RequestData requestData = new RequestData();
        //    requestData.details = Newtonsoft.Json.JsonConvert.SerializeObject(methodCallMessage);

        //    var myJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        //    var content = new StringContent(myJson, Encoding.UTF8, "application/json");

        //    var response = client.PostAsync(channelUri + "/api/RestApiMessages", content).Result;
        //    if (response.IsSuccessStatusCode)
        //    {

        //        var task = response.Content.ReadAsStringAsync();
        //        task.Wait();
        //        var responseDataString = task.Result;
        //        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseData>(responseDataString);
        //        var returnMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);

        //        ByRef remoteRef= Newtonsoft.Json.JsonConvert.DeserializeObject<ByRef>(returnMessage.ReturnObjectJson);

        //        Proxy proxy = new Proxy(remoteRef);// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.IServerSessionPart));


        //        IServerSessionPart serverSessionPart = proxy.GetTransparentProxy() as IServerSessionPart;// (GetTransparentProxy() as IRomotingServer).GetServerSession(RemotingServices.ProcessIdentity);
        //        ClientSessionPart clientSessionPart = RenewalManager.NewSession(channelUri,Guid.Parse( methodCallMessage.SessionID),Guid.Parse( returnMessage.SessionID), serverSessionPart, RemotingServices.CurrentRemotingServices);
        //        return clientSessionPart;
        //        //return proxy.GetTransparentProxy() as IRomotingServer;

        //    }
        //    return null;

        //}

        /// <MetaDataID>{12131c48-9d78-4f0b-86f2-d7c770aee1a4}</MetaDataID>
        private static object Unmarshal(ReturnMessage returnMessage)
        {

            return null;
        }



        /// <MetaDataID>{bac85bb7-de27-4314-b7cb-9d21be644d14}</MetaDataID>
        public static ResponseData Invoke(string requestUri, RequestData requestData, string X_Auth_Token, string X_Access_Token, Binding binding = null)
        {

            if (binding == null)
                binding = Binding.DefaultBinding;

            if (requestUri.Trim().IndexOf("http://") == 0)
                requestUri = "ws://" + requestUri.Substring("http://".Length);

            if (requestUri.Trim().IndexOf("ws://") == 0 || requestUri.Trim().IndexOf("wss://") == 0)
            {
                #region Uses web socket request channel
                WebSocketClient webSocket = WebSocketClient.EnsureConnection(requestUri + "WebSocketMessages", binding);
                if (webSocket.State != WebSocketState.Open)
                {
                    ReturnMessage responseMessage = new ReturnMessage(requestData.ChannelUri);
                    var restApiException = new RestApiExceptionData();
                    if (webSocket.SocketException != null)
                    {
                        restApiException.ExceptionMessage = webSocket.SocketException.Message;
                        restApiException.ServerStackTrace = webSocket.SocketException.StackTrace;
                        restApiException.ExceptionCode = ExceptionCode.ConnectionError;
                        responseMessage.Exception = restApiException;
                    }
                    else
                    {
                        restApiException.ExceptionMessage = webSocket.LastError;
                        responseMessage.Exception = restApiException;
                    }
                    return new ResponseData(requestData.ChannelUri) { IsSucceeded = responseMessage.Exception == null, CallContextID = requestData.CallContextID, SessionIdentity = requestData.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                }

                RequestData request = new RequestData() { CallContextID = requestData.CallContextID, ChannelUri = requestData.ChannelUri, CallContextDictionaryData = requestData.CallContextDictionaryData, details = requestData.details, RequestType = requestData.RequestType, SessionIdentity = requestData.SessionIdentity };

                var task = webSocket.SendRequestAsync(request);
                if (!task.Wait(System.TimeSpan.FromSeconds(25)))
                {
                    if (!task.Wait(Binding.DefaultBinding.SendTimeout))
                    {
                        webSocket.RejectRequest(task);
                        throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                    }

                }

                return task.Result;
                #endregion
            }


            if (requestUri.Trim().IndexOf("http://") == 0)
            {
                #region Uses HTTP request channel

                var methodCallMessage = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(methodCallMessage, Encoding.UTF8, "application/json");

                if (X_Auth_Token != null)
                    content.Headers.Add("X-Auth-Token", X_Auth_Token);
                if (X_Access_Token != null)
                    content.Headers.Add("X-Access-Token", X_Access_Token);

                var response = client.PostAsync(requestUri + "/RestApiMessages", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var task = response.Content.ReadAsStringAsync();
                    task.Wait();
                    var responseDataString = task.Result;
                    ResponseData responseData = OOAdvantech.Json.JsonConvert.DeserializeObject<ResponseData>(responseDataString);
                    return responseData;
                }

                #endregion
            }

#if !DeviceDotNet

            #region Uses tcp channel
            if (requestUri.Trim().IndexOf("net.tcp://") == 0)
            {
                try
                {
                    var ResponseData = GetWcfMessageDispatcherProxy(requestUri + "WCFMessageDispatcher").MessageDispatch(requestData);
                    return ResponseData;
                }
                catch (Exception error)
                {
                }
            }
            #endregion
#endif
            return null;
        }


#if !DeviceDotNet
        /// <MetaDataID>{77992f1a-a7be-4a1f-885b-426900f6d835}</MetaDataID>
        private static IMessageDispatcher GetWcfMessageDispatcherProxy(string serviceUrl)
        {
            System.ServiceModel.NetTcpBinding binding = new System.ServiceModel.NetTcpBinding(System.ServiceModel.SecurityMode.None);
            System.ServiceModel.EndpointAddress endpointAddress
                = new System.ServiceModel.EndpointAddress(serviceUrl);

            return new System.ServiceModel.ChannelFactory<IMessageDispatcher>
                (binding, endpointAddress).CreateChannel();
        }
#endif



        /// <MetaDataID>{c62ee0ca-0bdb-48ed-8aa3-59a187fbad23}</MetaDataID>
        public ServerSessionPartInfo GetServerSession(string channelUri, Guid processIdentity)
        {
            var methodCallMessage = new MethodCallMessage(channelUri, "type(RestApiRemoting/OOAdvantech.Remoting.RestApi.RemotingServicesServer)", "", "", StandardActions.CreateCommunicationSession, new object[0]);
            methodCallMessage.ClientProcessIdentity = processIdentity.ToString("N");
            RequestData requestData = new RequestData();
            requestData.SessionIdentity = null;// methodCallMessage.ClientProcessIdentity;
            requestData.details = OOAdvantech.Json.JsonConvert.SerializeObject(methodCallMessage);
            requestData.RequestType = RequestType.MethodCall;

            var myJson = OOAdvantech.Json.JsonConvert.SerializeObject(requestData);
            //var content = new StringContent(myJson, Encoding.UTF8, "application/json");

            //var response = client.PostAsync(channelUri + "/RestApiMessages", content).Result;
            //string internalchannelUri = null;
            requestData.ChannelUri = channelUri;

            //if (channelUri.IndexOf("(") != -1)
            //{
            //    //internalchannelUri = channelUri.Substring(channelUri.IndexOf("(") + 1);
            //    //internalchannelUri = internalchannelUri.Substring(0, internalchannelUri.Length - 1);
            //    channelUri = channelUri.Substring(0, channelUri.IndexOf("("));
            ////    requestData.InternalChannelUri = internalchannelUri;
            ////    requestData.PublicChannelUri = channelUri;
            //}
            ////else

            var responseData = Invoke(requestData.PublicChannelUri, requestData, null, null, Binding.DefaultBinding);
            if (responseData != null)
            {

                //var task = response.Content.ReadAsStringAsync();
                //task.Wait();
                //var responseDataString = response;
                //var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseData>(responseDataString);
                var returnMessage = OOAdvantech.Json.JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);
                if (returnMessage.ServerSessionObjectRef != null)
                {

                }

                if (returnMessage.Exception != null)
                {
                    if (returnMessage.Exception.ExceptionCode == ExceptionCode.ConnectionError)
                        throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.ConnectFailure);
#if DeviceDotNet
                    if (returnMessage.Exception.ExceptionCode == ExceptionCode.AccessTokenExpired)
                        throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.RequestCanceled);
#else
                    if (returnMessage.Exception.ExceptionCode == ExceptionCode.AccessTokenExpired)
                        throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.TrustFailure);
#endif
                    throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.UnknownError);
                }

                // var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null) };
                //ObjRef remoteRef = JsonConvert.DeserializeObject<ObjRef>(returnMessage.ReturnObjectJson);

                var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.NetTypedValuesJsonSerialization, null);
                ObjRef remoteRef = JsonConvert.DeserializeObject<ObjRef>(returnMessage.ReturnObjectJson, jSetttings);

                //JsonConvert.DeserializeObject(returnMessage.ReturnObjectJson, jSetttings) as ByRef;
                //if (remoteRef == null)
                //{
                //    SerializedData serData = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializedData>(returnMessage.ReturnObjectJson);
                //    remoteRef = serData.Ref;// Newtonsoft.Json.JsonConvert.DeserializeObject<ByRef>(returnMessage.ReturnObjectJson);
                //}

                Proxy proxy = new Proxy(remoteRef, typeof(IServerSessionPart));// new Proxy(remoteRef.Uri, remoteRef.ChannelUri, typeof(OOAdvantech.Remoting.IServerSessionPart));


                IServerSessionPart serverSessionPart = proxy.GetTransparentProxy(typeof(IServerSessionPart)) as IServerSessionPart;// (GetTransparentProxy() as IRomotingServer).GetServerSession(RemotingServices.ProcessIdentity);
                return new ServerSessionPartInfo() { SessionIdentity = responseData.SessionIdentity, ServerSessionPart = serverSessionPart, ServerProcessIdentity = Guid.Parse(returnMessage.ServerProcessIdentity), BidirectionalChannel = responseData.BidirectionalChannel };
            }
            return default(ServerSessionPartInfo);
        }

        /// <MetaDataID>{cf27488f-0faa-4d28-a0ed-2285b9cf63a0}</MetaDataID>
        public OOAdvantech.Remoting.ClientSessionPart CreateClientSessionPart(string channelUri, Guid clientProcessIdentity, ServerSessionPartInfo serverSessionPartInfo)
        {
            return new ClientSessionPart(channelUri, clientProcessIdentity, serverSessionPartInfo, this);
        }

        /// <MetaDataID>{85cdd857-abb2-4360-b1dd-5a3e8ffdffd9}</MetaDataID>
        public static IRemotingServices CurrentRemotingServices = new RemotingServices();

        /// <MetaDataID>{60322d9a-fa98-4ecc-8527-d4a9290d1103}</MetaDataID>
        public static bool RunInAzureRole { get; set; }
        /// <MetaDataID>{c2129459-c11b-4d3a-b9bb-a7b2021467cf}</MetaDataID>
        public static string ServerPublicUrl { get; set; }


#if !DeviceDotNet
        /// <MetaDataID>{907fb2fc-6975-49ff-b163-8e1ffccb3923}</MetaDataID>
        public static string GetLocalIPAddress()
        {
            //var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        return ip.ToString();
            //    }
            //}
            return null;
        }



#endif


        static bool LeaseTimeIsSet;
        public static void SetDebugLeaseTime()
        {
            if (!LeaseTimeIsSet)
            {
                LeaseTimeIsSet = true;
#if !DeviceDotNet
                System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime = TimeSpan.FromSeconds(200);
                System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(100);
                System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout = TimeSpan.FromSeconds(100);/**/
#endif
            }
        }
        public static void SetProductionLeaseTime()
        {
#if !DeviceDotNet
            System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime = TimeSpan.FromMinutes(20);
            System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime = TimeSpan.FromMinutes(10);
            System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout = TimeSpan.FromMinutes(10);/**/
#endif

        }


        /// <MetaDataID>{88df8d65-b441-42be-8609-7e756c603689}</MetaDataID>
        public static IInternalEndPointResolver InternalEndPointResolver { get; set; }

        //CreateObject<T>(string typeUri , ...args: any[]): T {

        //    return <T>this.AuthInvoke("type(" + typeUri + ")", "0f385a2523a040a7afb8ff8549ada905", args);
    }

    /// <MetaDataID>{8b1dfb84-8341-4cd8-ad1b-b273fe333d21}</MetaDataID>
    public interface IInternalEndPointResolver
    {
        /// <MetaDataID>{70184b51-e6db-4360-a049-b6650ca1bd5e}</MetaDataID>
        string GetInternalEndPointUrl(string contextID);


        /// <MetaDataID>{73b06385-e136-41b0-bc9a-b015693689a0}</MetaDataID>
        bool CanBeResolvedLocal(TransportData transportData);

        /// <MetaDataID>{f352c6df-d9ac-4356-aca9-be65e3464d6a}</MetaDataID>
        string GetRoleInstanceServerUrl(TransportData transportData);

        string GetRoleInstanceServerUrl(string contextID);
        string TranslateToPublic(string channelUri);
    }




}