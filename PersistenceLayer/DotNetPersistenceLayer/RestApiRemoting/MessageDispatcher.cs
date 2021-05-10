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
using OOAdvantech.Transactions;
#if !DeviceDotNet
using System.Web;
#endif
using OOAdvantech.Json.Linq;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{30d646b7-5886-4ec2-8e1e-49026c836b6f}</MetaDataID>
    public class MessageDispatcher
    {
        internal static string ProcessName= System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        /// <MetaDataID>{a5db401d-2be2-44eb-9391-adc1b4ac6cb8}</MetaDataID>
        static MessageDispatcher()
        {
            Remoting.RemotingServices remotingServices = default(Remoting.RemotingServices);

            Serialization.SerializationBinder.NamesTypesDictionary["Array"] = typeof(object[]);
            Serialization.SerializationBinder.NamesTypesDictionary["String"] = typeof(string);
            Serialization.SerializationBinder.NamesTypesDictionary["Number"] = typeof(double);
            Serialization.SerializationBinder.NamesTypesDictionary["Array"] = typeof(List<>);
            Serialization.SerializationBinder.NamesTypesDictionary["Map"] = typeof(Dictionary<,>);
            Serialization.SerializationBinder.NamesTypesDictionary["OOAdvantech.MetaDataRepository.ProxyType"] = typeof(OOAdvantech.MetaDataRepository.ProxyType);
            Serialization.SerializationBinder.NamesTypesDictionary["OOAdvantech.Remoting.RestApi.ObjRef"] = typeof(OOAdvantech.Remoting.RestApi.ObjRef);

            Serialization.SerializationBinder.NamesTypesDictionary["OOAdvantech.Remoting.RestApi.RemotingServicesServer"] = typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer);
            Serialization.SerializationBinder.NamesTypesDictionary["OOAdvantech.Remoting.RestApi.TypesMetadataCommunicationSession"] = typeof(OOAdvantech.Remoting.RestApi.TypesMetadataCommunicationSession);

            Serialization.SerializationBinder.NamesTypesDictionary["OOAdvantech.Remoting.ExtObjectUri"] = typeof(OOAdvantech.Remoting.ExtObjectUri);
            Serialization.SerializationBinder.NamesTypesDictionary["OOAdvantech.Remoting.EventInfoData"] = typeof(OOAdvantech.Remoting.EventInfoData);
            //Serialization.SerializationBinder.NamesTypesDictionary["OOAdvantech.Authentication.UserInfo"] = typeof(OOAdvantech.Authentication.UserInfo);
            //Serialization.SerializationBinder.NamesTypesDictionary["OOAdvantech.Authentication.AuthUser"] = typeof(OOAdvantech.Authentication.AuthUser);




            Serialization.SerializationBinder.TypesNamesDictionary[typeof(bool)] = "Boolean";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(int)] = "Number";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(double)] = "Number";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(decimal)] = "Number";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(float)] = "Number";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(short)] = "Number";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(long)] = "Number";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(DateTime)] = "Date";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(string)] = "String";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(object[])] = "Array";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(List<>)] = "Array";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(Collections.Generic.Set<>)] = "Array";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(Dictionary<,>)] = "Map";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(OOAdvantech.MetaDataRepository.ProxyType)] = "OOAdvantech.MetaDataRepository.ProxyType";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(OOAdvantech.Remoting.RestApi.ObjRef)] = "OOAdvantech.Remoting.RestApi.ObjRef";
            Serialization.SerializationBinder.TypesNamesDictionary[typeof(OOAdvantech.Remoting.RestApi.TypesMetadataCommunicationSession)] = "OOAdvantech.Remoting.RestApi.TypesMetadataCommunicationSession";
            //Serialization.SerializationBinder.TypesNamesDictionary[typeof(OOAdvantech.Authentication.UserInfo)] = "OOAdvantech.Authentication.UserInfo";
            //Serialization.SerializationBinder.TypesNamesDictionary[typeof(OOAdvantech.Authentication.AuthUser)] = "OOAdvantech.Authentication.AuthUser";




        }

        /// <MetaDataID>{d57d77bf-a4d2-4388-877c-02c7575392b6}</MetaDataID>
        public static void Init()
        {

        }



        /// <MetaDataID>{c9d6ebae-8474-498a-a506-7aa27b944f63}</MetaDataID>
        public static Task<ResponseData> MessageDispatchAsync(RequestData request)
        {
            try
            {
                MethodCallMessage methodCallMessage = JsonConvert.DeserializeObject<MethodCallMessage>(request.details);// Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(request.details);
                if (request.ChannelUri == null)
                    request.ChannelUri = methodCallMessage.ChannelUri;
                methodCallMessage.CallContextDictionaryData = request.CallContextDictionaryData;

                //System.IO.File.AppendAllLines("/storage/emulated/0/test.txt", new string[] { "method name:" + methodCallMessage.MethodName });

#if !DeviceDotNet
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    methodCallMessage.X_Access_Token = HttpContext.Current.Request.Headers["X-Access-Token"];
                    methodCallMessage.X_Auth_Token = HttpContext.Current.Request.Headers["X-Auth-Token"];
                }
#endif

                if (methodCallMessage.IsRestApiManagerMethodCall)
                    return Task<ResponseData>.Run(() =>
                    {
                        if (request.CallContextDictionaryData != null)
                        {

                            foreach (var data in request.CallContextDictionaryData)
                                System.Runtime.Remoting.Messaging.CallContext.SetData(data.Key, data.Value);
                        }

                        var responseData = ProcessRestApiManagerMethodCall(request, methodCallMessage);
                        responseData.CallContextID = request.CallContextID;
                        return responseData;
                    });

                try
                {
                    ServerSessionPart serverSession = null;
                    ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);

                    #region Get server session
                    lock (ServerSessionPart.ServerSessions)
                    {
                        serverSession = ServerSessionPart.GetServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity), request.ChannelUri);
                        if (serverSession == null)
                        {
                            serverSession = new ServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity), request.ChannelUri, request.InternalChannelUri, methodCallMessage.Web);
                            responseMessage.ServerSessionObjectRef = serverSession.GetServerSesionObjectRef();

                        }
                    }
                    #endregion

                    serverSession.SessionTypesSync(methodCallMessage);

                    serverSession.SetConnectionState(request.PhysicalConnectionID, true);
                    serverSession.WebSocketEndPoint = request.EventCallBackChannel;


                    var authUser = serverSession.GetAuthData(methodCallMessage);

                    if (methodCallMessage.ChannelUri == "local-device" && !string.IsNullOrWhiteSpace(methodCallMessage.X_Auth_Token))
                    {

                        if (DeviceAuthentication.UnInitialized)
                        {
                            var instance = MonoStateClass.GetInstance(typeof(DeviceAuthentication), true);
                            (MonoStateClass.GetInstance(typeof(DeviceAuthentication)) as DeviceAuthentication).InternalAuthIDTokenChanged(methodCallMessage.X_Auth_Token, authUser);
                        }
                        else if (DeviceAuthentication.AuthUser == null && authUser != null)
                        {
                            var instance = MonoStateClass.GetInstance(typeof(DeviceAuthentication), true);
                            (MonoStateClass.GetInstance(typeof(DeviceAuthentication)) as DeviceAuthentication).InternalAuthIDTokenChanged(methodCallMessage.X_Auth_Token, authUser);
                        }
                    }
                    System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);

                    responseMessage.Web = methodCallMessage.Web;
                    responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                    responseMessage.ChannelUri = request.ChannelUri;
                    responseMessage.ServerSession = serverSession;
                    responseMessage.X_Access_Token = serverSession.X_Access_Token;


                    try
                    {
                        if (!string.IsNullOrWhiteSpace(methodCallMessage.X_Access_Token) && string.IsNullOrWhiteSpace(serverSession.X_Access_Token))
                        {
                            //Authentication error
                            responseMessage.Exception = new RestApiExceptionData();
                            responseMessage.Exception.ExceptionMessage = "X_Access_Token expired";
                            responseMessage.Exception.ExceptionCode = ExceptionCode.AccessTokenExpired;
                            return Task<ResponseData>.Run(() => { return new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) }; });

                        }

                        methodCallMessage.UnMarshal();

                        var methodInfo = methodCallMessage.MethodInfo;
                        object[] args = methodCallMessage.Args.ToArray(); 
                        var _params = methodInfo.GetParameters();
                        
                        if (request.CallContextDictionaryData != null)
                        {
                            foreach (var data in request.CallContextDictionaryData)
                                System.Runtime.Remoting.Messaging.CallContext.SetData(data.Key, data.Value);
                        }
                        
                        if (args.Length < _params.Length)
                        {
                            object[] m_args = new object[_params.Length];
                            args.CopyTo(m_args, 0);
                            for (int i= args.Length;i< _params.Length;i++)
                            {
                                if(_params[i].DefaultValue!=System.DBNull.Value)
                                    m_args[i] = _params[i].DefaultValue;
                            }
                            args = m_args;
                        }
                        object retVal = null;
                        if (request.GetCallContextData("Transaction") != null)
                        {

                            using (SystemStateTransition stateTransition = new SystemStateTransition(request.GetCallContextData("Transaction") as Transaction))
                            {
                                retVal = methodInfo.Invoke(methodCallMessage.Object, args); 
                                stateTransition.Consistent = true;
                            }

                        }
                        else
                        {
                            retVal = methodInfo.Invoke(methodCallMessage.Object, args);
                        }
                        if (request.CallContextDictionaryData != null)
                        {
                            foreach (var data in request.CallContextDictionaryData)
                                System.Runtime.Remoting.Messaging.CallContext.SetData(data.Key, null);
                        }

                        if (retVal is Task)
                        {
                            return Task<ResponseData>.Run(() =>
                            {
                                (retVal as Task).Wait();
                                object value = null;
                                var type = retVal.GetType().GetMetaData().GetGenericArguments()[0];
                                ITaskResultGetter getter = Activator.CreateInstance(typeof(TaskResultGetter<>).MakeGenericType(new[] { type })) as ITaskResultGetter;
                                value = getter.GetTaskResult((retVal as Task));
                                responseMessage.MethodInfo = methodInfo;
                                responseMessage.OutArgs = methodCallMessage.GetOutArgs(args);
                                responseMessage.RetVal = value;
                                responseMessage.ServerSession = serverSession;
                                responseMessage.Marshal();

                                string json = JsonConvert.SerializeObject(responseMessage);

                                return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = json };
                            });
                        }
                        else
                        {
                            responseMessage.MethodInfo = methodInfo;
                            responseMessage.OutArgs = methodCallMessage.GetOutArgs(args);
                            responseMessage.RetVal = retVal;
                            responseMessage.ServerSession = serverSession;
                            responseMessage.Marshal();

                            string json = JsonConvert.SerializeObject(responseMessage);
                            return Task<ResponseData>.Run(() => { return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = json }; });
                        }

                    }
                    catch (System.Reflection.TargetInvocationException error)
                    {
                        responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ServerError, error.InnerException);
                        return Task<ResponseData>.Run(() => {
                            return new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                            });
                    }
                    catch (Exception error)
                    {
                        responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ServerError, error);
                        return Task<ResponseData>.Run(() =>{
                            return new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                        });
                    }
                    finally
                    {
                        System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", null);
                    }

                }
                catch (Exception error)
                {
                    //System.IO.File.AppendAllLines("/storage/emulated/0/test.txt", new string[] { "Exception :  " + error.Message });
                    //System.IO.File.AppendAllLines("/storage/emulated/0/test.txt", new string[] { "Stack trace :  " + error.StackTrace });
                    throw new TypeLoadException("Error loading " + methodCallMessage.ObjectUri);
                }
            }
            catch (Exception error)
            {
                //System.IO.File.AppendAllLines("/storage/emulated/0/test.txt", new string[] { "Exception :  " + error.Message });
                throw;
            }

        }


        /// <MetaDataID>{f7a8229c-5601-4996-a6b8-a55b250a917e}</MetaDataID>
        public static ResponseData MessageDispatch(RequestData request)
        {

            if(request.RequestProcess!= "WaWorkerHost")
            {

            }
            try
            {
                MethodCallMessage methodCallMessage = JsonConvert.DeserializeObject<MethodCallMessage>(request.details);// Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(request.details);
                if (request.ChannelUri == null)
                    request.ChannelUri = methodCallMessage.ChannelUri;
                methodCallMessage.CallContextDictionaryData = request.CallContextDictionaryData;

#if !DeviceDotNet
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    methodCallMessage.X_Access_Token = HttpContext.Current.Request.Headers["X-Access-Token"];
                    methodCallMessage.X_Auth_Token = HttpContext.Current.Request.Headers["X-Auth-Token"];
                }
#endif

                if (methodCallMessage.IsRestApiManagerMethodCall)
                {
                    if (request.CallContextDictionaryData != null)
                    {
                        foreach (var data in request.CallContextDictionaryData)
                            System.Runtime.Remoting.Messaging.CallContext.SetData(data.Key, data.Value);
                    }
                    var responseData = ProcessRestApiManagerMethodCall(request, methodCallMessage);
                    responseData.CallContextID = request.CallContextID;
                    return responseData;
                }

                try
                {
                    ServerSessionPart serverSession = null;
                    ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);

                    #region Get server session
                    lock (ServerSessionPart.ServerSessions)
                    {

                        serverSession = ServerSessionPart.GetServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity), request.ChannelUri);
                        if (serverSession == null)
                        {
                            serverSession = new ServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity), request.ChannelUri, request.InternalChannelUri, methodCallMessage.Web);
                            responseMessage.ServerSessionObjectRef = serverSession.GetServerSesionObjectRef();
                        }
                    }
                    #endregion

                    serverSession.SessionTypesSync(methodCallMessage);

                    serverSession.SetConnectionState(request.PhysicalConnectionID, true);
                    serverSession.WebSocketEndPoint = request.EventCallBackChannel;

                    var authUser = serverSession.GetAuthData(methodCallMessage);
                    if (methodCallMessage.ChannelUri == "local-device" && !string.IsNullOrWhiteSpace(methodCallMessage.X_Auth_Token))
                    {
                        if (DeviceAuthentication.UnInitialized)
                        {
                            var instance = MonoStateClass.GetInstance(typeof(DeviceAuthentication), true);
                            (MonoStateClass.GetInstance(typeof(DeviceAuthentication)) as DeviceAuthentication).InternalAuthIDTokenChanged(methodCallMessage.X_Auth_Token, authUser);
                        }
                        else if (DeviceAuthentication.AuthUser == null && authUser != null)
                        {
                            var instance = MonoStateClass.GetInstance(typeof(DeviceAuthentication), true);
                            (MonoStateClass.GetInstance(typeof(DeviceAuthentication)) as DeviceAuthentication).InternalAuthIDTokenChanged(methodCallMessage.X_Auth_Token, authUser);
                        }

                    }
                    System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);

                    responseMessage.Web = methodCallMessage.Web;
                    responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                    responseMessage.ChannelUri = request.ChannelUri;
                    responseMessage.InternalChannelUri = request.InternalChannelUri;
                    responseMessage.ServerSession = serverSession;
                    responseMessage.X_Access_Token = serverSession.X_Access_Token;

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(methodCallMessage.X_Access_Token) && string.IsNullOrWhiteSpace(serverSession.X_Access_Token))
                        {
                            //Authentication error
                            responseMessage.Exception = new RestApiExceptionData();
                            responseMessage.Exception.ExceptionMessage = "X_Access_Token expired";
                            responseMessage.Exception.ExceptionCode = ExceptionCode.AccessTokenExpired;
                            return new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                        }

                        methodCallMessage.UnMarshal();

                     
                        var methodInfo = methodCallMessage.MethodInfo;
                        object[] args = methodCallMessage.Args.ToArray();
                        if (request.CallContextDictionaryData != null)
                        {
                            foreach (var data in request.CallContextDictionaryData)
                                System.Runtime.Remoting.Messaging.CallContext.SetData(data.Key, data.Value);
                        }

                        object retVal = methodInfo.Invoke(methodCallMessage.Object, args);
                        if (request.CallContextDictionaryData != null)
                        {
                            foreach (var data in request.CallContextDictionaryData)
                                System.Runtime.Remoting.Messaging.CallContext.SetData(data.Key, null);
                        }


                        responseMessage.MethodInfo = methodInfo;
                        responseMessage.OutArgs = methodCallMessage.GetOutArgs(args);
                        responseMessage.RetVal = retVal;
                        responseMessage.ServerSession = serverSession;
                        responseMessage.Marshal();

                        string json = JsonConvert.SerializeObject(responseMessage);
                        return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = json };
                    }
                    catch (System.Reflection.TargetInvocationException error)
                    {
                        responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ServerError, error.InnerException);
                        return new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                    }
                    catch (Exception error)
                    {
                        responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ServerError, error);
                        return new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage) };
                    }
                    finally
                    {
                        System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", null);
                    }

                }
                catch (Exception error)
                {
                    throw new TypeLoadException("Error loading " + methodCallMessage.ObjectUri);
                }
            }
            catch (Exception error)
            {

                throw;
            }

        }
        //static internal ReferenceResolver ReferenceResolver = new OOAdvantech.Remoting.RestApi.ReferenceResolver();

        #region deletede code ProcessRestApiManagerMethodCall

        ///// <MetaDataID>{a604830a-8da1-4ca7-a5fb-598f52831134}</MetaDataID>
        //private static ResponseData ProcessRestApiManagerMethodCall(MethodCallMessage methodCallMessage)
        //{

        //    ServerSessionPart serverSession = null;
        //    try
        //    {
        //        if (!methodCallMessage.IsRestApiManagerMethodCall)
        //            throw new NotSupportedException();


        //        ServerSessions.TryGetValue(methodCallMessage.ClientProcessIdentity, out serverSession);

        //        ReturnMessage responseMessage = new ReturnMessage();
        //        responseMessage.ChannelUri = methodCallMessage.ChannelUri;

        //        if (serverSession == null)
        //        {
        //            serverSession = new ServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity));
        //            ServerSessions[methodCallMessage.ClientProcessIdentity] = serverSession;

        //        }
        //        responseMessage.ChannelUri = methodCallMessage.ChannelUri;
        //        responseMessage.ServerSession = serverSession;
        //        //serverSession.GetAuthData(methodCallMessage);
        //        if (methodCallMessage.MethodName == StandardActions.CreateCommunicationSession)
        //        {

        //            responseMessage.ReturnType = typeof(IServerSessionPart).AssemblyQualifiedName;
        //            responseMessage.ReturnObjectJson = JsonMarshal(serverSession, methodCallMessage.ChannelUri, serverSession);


        //            string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(serverSession as MarshalByRefObject);
        //            if (uri == null)
        //                uri = System.Runtime.Remoting.RemotingServices.Marshal(serverSession as MarshalByRefObject).URI;

        //            Remoting.Tracker.WeakReferenceOnMarshaledObjects[uri] = new WeakReference(serverSession);

        //            ProxyType httpProxyType = null;
        //            Type instanceType = serverSession.GetType();
        //            if (!serverSession.MarshalledTypes.TryGetValue(instanceType, out httpProxyType))
        //            {
        //                httpProxyType = new ProxyType(instanceType);
        //                serverSession.MarshalledTypes[instanceType] = httpProxyType;
        //            }
        //            string internalChannelUri = System.Runtime.Remoting.Messaging.CallContext.GetData("internalChannelUri") as string;
        //            ByRef byref = new ByRef(uri, methodCallMessage.ChannelUri, internalChannelUri, serverSession.GetType().AssemblyQualifiedName, httpProxyType);
        //            //byref.Uri = uri;
        //            //byref.ChannelUri = methodCallMessage.ChannelUri;

        //            //byref.ReturnTypeMetaData = httpProxyType;
        //            //byref.TypeName = serverSession.GetType().AssemblyQualifiedName;
        //            SerializedData serializedData = new SerializedData();
        //            serializedData.Ref = byref;
        //            responseMessage.ReturnObjectJson = OOAdvantech.Json.JsonConvert.SerializeObject(serializedData);


        //        }

        //        Type type = methodCallMessage.MethodDeclaringType;
        //        if (methodCallMessage.MethodName == StandardActions.GetRemotingServicesServer)
        //        {
        //            if (Classifier.GetClassifier(type).IsA(Classifier.GetClassifier(typeof(MonoStateClass))))
        //            {

        //                MonoStateClass instance = MonoStateClass.GetInstance(type);
        //                if (instance == null)
        //                    instance = System.Activator.CreateInstance(type) as MonoStateClass;

        //                responseMessage.ReturnObjectJson = JsonMarshal(instance, methodCallMessage.ChannelUri, serverSession);
        //                responseMessage.ChannelUri = methodCallMessage.ChannelUri;
        //                responseMessage.ReturnType = type.FullName;
        //            }
        //        }

        //        string json = JsonConvert.SerializeObject(responseMessage);
        //        return new ResponseData() { details = json };
        //    }
        //    catch (Exception error)
        //    {
        //        ReturnMessage responseMessage = new ReturnMessage();
        //        responseMessage.ChannelUri = methodCallMessage.ChannelUri;

        //        responseMessage.Exception = new RestApiExceptionData();
        //        responseMessage.Exception.ExceptionMessage = error.Message;
        //        responseMessage.Exception.ExceptionCode = 12;
        //        string json = JsonConvert.SerializeObject(responseMessage);
        //        return new ResponseData() { details = json };
        //    }
        //}

        #endregion

        /// <MetaDataID>{a2f34a30-a6cd-406f-9aec-90b60f1dfba9}</MetaDataID>
        private static ResponseData ProcessRestApiManagerMethodCall(RequestData request, MethodCallMessage methodCallMessage)
        {

            ReturnMessage responseMessage = new ReturnMessage(request.ChannelUri);
            ServerSessionPart serverSession = null;
            try
            {
                if (!methodCallMessage.IsRestApiManagerMethodCall)
                    throw new NotSupportedException();

                string internalChannelUri = request.InternalChannelUri;// System.Runtime.Remoting.Messaging.CallContext.GetData("internalChannelUri") as string;
                bool initCommunicationSession = false;
                lock (ServerSessionPart.ServerSessions)
                {
                    serverSession = ServerSessionPart.GetServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity), request.ChannelUri);
                    if (serverSession == null)
                    {
                        serverSession = new ServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity), request.ChannelUri, request.InternalChannelUri, methodCallMessage.Web);
                        responseMessage.ServerSessionObjectRef = serverSession.GetServerSesionObjectRef(); ;
                        initCommunicationSession = true;
                    }
                }
                serverSession.WebSocketEndPoint = request.EventCallBackChannel;
                responseMessage.Web = methodCallMessage.Web;
                responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                responseMessage.ChannelUri = request.ChannelUri;
                responseMessage.ServerSession = serverSession;

                if (methodCallMessage.MethodName == StandardActions.Disconnected)
                {

                    System.Diagnostics.Debug.WriteLine("RestApp Disconnected");
                    serverSession.SetConnectionState(request.PhysicalConnectionID, false);
                    //serverSession.Connected = false;
                    //var dataContext = System.Runtime.Remoting.Messaging.CallContext.GetData("DataContext");
                    responseMessage.Web = methodCallMessage.Web;
                    responseMessage.ChannelUri = request.ChannelUri;
                    responseMessage.InternalChannelUri = request.InternalChannelUri;
                    responseMessage.ServerSession = serverSession;
                    responseMessage.RetVal = null;
                    responseMessage.Web = methodCallMessage.Web;
                    responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                    responseMessage.Marshal();
                    //responseMessage.ReturnObjectJson = JsonMarshal(dataContext, methodCallMessage.ChannelUri, serverSession);
                    responseMessage.ChannelUri = request.ChannelUri;
                    //responseMessage.ReturnType = dataContextType.FullName;
                    return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage), InitCommunicationSession = initCommunicationSession };
                }


                //if (methodCallMessage.MethodName == StandardActions.ReconfigureChannel)
                //{

                //    var datetime = DateTime.Now;
                //    string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
                //    System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel Send ReconfigureChannel  {0} {1} ", timestamp, request.SessionIdentity));


                //    serverSession.SetConnectionState( request.PhysicalConnectionID,false);
                //    //serverSession.Connected = false;
                //    //var dataContext = System.Runtime.Remoting.Messaging.CallContext.GetData("DataContext");
                //    responseMessage.Web = methodCallMessage.Web;
                //    responseMessage.ChannelUri = request.ChannelUri;
                //    responseMessage.InternalChannelUri = request.InternalChannelUri;
                //    responseMessage.ServerSession = serverSession;
                //    responseMessage.RetVal = null;
                //    responseMessage.Web = methodCallMessage.Web;
                //    responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                //    responseMessage.Marshal();
                //    //responseMessage.ReturnObjectJson = JsonMarshal(dataContext, methodCallMessage.ChannelUri, serverSession);
                //    responseMessage.ChannelUri = request.ChannelUri;
                //    //responseMessage.ReturnType = dataContextType.FullName;
                //    return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage), InitCommunicationSession = initCommunicationSession };
                //}
                serverSession.SetConnectionState(request.PhysicalConnectionID, true);
                //serverSession.Connected = true;


                if (methodCallMessage.MethodName == StandardActions.GetDataContext)
                {
                    request.SetCallContextData("GetDataContext", true);

                    var dataContext = System.Runtime.Remoting.Messaging.CallContext.GetData("DataContext");

                    Type dataContextType = dataContext.GetType();
                    responseMessage.Web = methodCallMessage.Web;

                    responseMessage.ChannelUri = request.ChannelUri;
                    responseMessage.InternalChannelUri = request.InternalChannelUri;
                    responseMessage.ServerSession = serverSession;
                    responseMessage.RetVal = dataContext;
                    responseMessage.Web = methodCallMessage.Web;
                    responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                    responseMessage.Marshal();
                    //responseMessage.ReturnObjectJson = JsonMarshal(dataContext, methodCallMessage.ChannelUri, serverSession);
                    responseMessage.ChannelUri = request.ChannelUri;
                    responseMessage.ReturnType = dataContextType.FullName;

                    string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(dataContext as MarshalByRefObject);

                    return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage), InitCommunicationSession = initCommunicationSession };
                }
                if (methodCallMessage.MethodName == StandardActions.GetTypesMetadata)
                {
                    //Marshal ato 
                    var dataContext = System.Runtime.Remoting.Messaging.CallContext.GetData("DataContext");

                    responseMessage.Web = methodCallMessage.Web;

                    responseMessage.ChannelUri = request.ChannelUri;
                    responseMessage.InternalChannelUri = request.InternalChannelUri;
                    responseMessage.ServerSession = serverSession;
                    lock (serverSession)
                    {
                        responseMessage.RetVal = serverSession.MarshaledTypes;
                    }
                    responseMessage.Web = methodCallMessage.Web;
                    responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                    responseMessage.Marshal();
                    //responseMessage.ReturnObjectJson = JsonMarshal(dataContext, methodCallMessage.ChannelUri, serverSession);


                    return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, SessionIdentity = request.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage), InitCommunicationSession = initCommunicationSession };
                }



                if (methodCallMessage.MethodName == StandardActions.CreateCommunicationSession)
                {
                    ObjRef byref = serverSession.GetServerSesionObjectRef();
                    responseMessage.ReturnType = typeof(IServerSessionPart).AssemblyQualifiedName;
                    //responseMessage.ReturnObjectJson = Newtonsoft.Json.JsonConvert.SerializeObject(byref);
                    //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Serialize, byref.ChannelUri, byref.InternalChannelUri, null) };
#if DeviceDotNet
                    //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, SerializationBinder = new OOAdvantech.Remoting.RestApi.SerializationBinder(methodCallMessage.Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, serverSession.ChannelUri, internalChannelUri, null, methodCallMessage.Web) };
                    var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Serialize, methodCallMessage.Web ? JsonSerializationFormat.TypeScriptJsonSerialization : JsonSerializationFormat.NetJsonSerialization, serverSession.ChannelUri, internalChannelUri, null);
#else
                    var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Serialize, methodCallMessage.Web?JsonSerializationFormat.TypeScriptJsonSerialization:JsonSerializationFormat.NetTypedValuesJsonSerialization, serverSession.ChannelUri, internalChannelUri, null);// { TypeNameHandling = methodCallMessage.Web ? TypeNameHandling.None : TypeNameHandling.All, Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(methodCallMessage.Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, serverSession.ChannelUri, internalChannelUri, null, methodCallMessage.Web) };
#endif

                    //if (methodCallMessage.Web)
                    //{
                    //    jSetttings.ReferenceResolver = new ReferenceResolver();
                    //    jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                    //    jSetttings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    //}
                    responseMessage.ReturnObjectJson = JsonConvert.SerializeObject(byref, jSetttings);

                    return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, SessionIdentity = serverSession.SessionIdentity, details = JsonConvert.SerializeObject(responseMessage), InitCommunicationSession = initCommunicationSession };

                }
                if (methodCallMessage.MethodName == StandardActions.GetCommunicationSessionWithTypesMetadata)
                {

                    string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(serverSession as MarshalByRefObject);
                    if (uri == null)
                        uri = System.Runtime.Remoting.RemotingServices.Marshal(serverSession as MarshalByRefObject).URI;

                    Remoting.Tracker.WeakReferenceOnMarshaledObjects[uri] = new WeakReference(serverSession);

                    ProxyType httpProxyType = null;
                    Type instanceType = serverSession.GetType();
                    lock (serverSession)
                    {
                        if (!serverSession.MarshaledTypes.TryGetValue(instanceType.AssemblyQualifiedName, out httpProxyType))
                        {
                            httpProxyType = new ProxyType(instanceType);
                            serverSession.MarshaledTypes[instanceType.AssemblyQualifiedName] = httpProxyType;
                        }
                    }
                    //Marshal  
                    ObjRef byref = new ObjRef(uri, serverSession.ChannelUri, serverSession.InternalChannelUri, serverSession.GetType().AssemblyQualifiedName, httpProxyType);
                    var typesMetadataCommunicationSession = new TypesMetadataCommunicationSession() { ServerSessionPartRef = byref, MarshaledTypes = serverSession.MarshaledTypes };

                    responseMessage.ReturnType = typeof(IServerSessionPart).AssemblyQualifiedName;
                    //responseMessage.ReturnObjectJson = Newtonsoft.Json.JsonConvert.SerializeObject(byref);
                    //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Serialize, byref.ChannelUri, byref.InternalChannelUri, null) };
#if DeviceDotNet
                    //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, SerializationBinder = new OOAdvantech.Remoting.RestApi.SerializationBinder(methodCallMessage.Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, serverSession.ChannelUri, internalChannelUri, null, methodCallMessage.Web) };
                    var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Serialize, methodCallMessage.Web ? JsonSerializationFormat.TypeScriptJsonSerialization : JsonSerializationFormat.NetTypedValuesJsonSerialization, serverSession.ChannelUri, internalChannelUri, null);
#else
                    var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Serialize, methodCallMessage.Web ? JsonSerializationFormat.TypeScriptJsonSerialization:JsonSerializationFormat.NetTypedValuesJsonSerialization, serverSession.ChannelUri, internalChannelUri, null);// { TypeNameHandling = methodCallMessage.Web ? TypeNameHandling.None : TypeNameHandling.All, Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(methodCallMessage.Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, serverSession.ChannelUri, internalChannelUri, null, methodCallMessage.Web) };

                    //if (methodCallMessage.Web)
                    //{
                    //    jSetttings.ReferenceResolver = new ReferenceResolver();
                    //    jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                    //    jSetttings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    //}


#endif

                    responseMessage.ReturnObjectJson = JsonConvert.SerializeObject(typesMetadataCommunicationSession, jSetttings);


                }

                Type type = methodCallMessage.MethodDeclaringType;
                if (methodCallMessage.MethodName == StandardActions.GetRemotingServicesServer)
                {
                    var typeClassifier = Classifier.GetClassifier(type);
                    var monoStateClass = Classifier.GetClassifier(typeof(MonoStateClass));
                    if (typeClassifier.IsA(monoStateClass))
                    {

                        MonoStateClass instance = MonoStateClass.GetInstance(type, true);
                        responseMessage.Web = methodCallMessage.Web;
                        responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                        responseMessage.ChannelUri = request.ChannelUri;
                        responseMessage.InternalChannelUri = request.InternalChannelUri;
                        responseMessage.ServerSession = serverSession;
                        responseMessage.RetVal = instance;
                        responseMessage.Marshal();
                        if (responseMessage.ReturnObjectJson == null)
                        {

                        }
                    }
                    if (responseMessage.ReturnObjectJson == null)
                    {
                        bool dfd = typeClassifier.IsA(monoStateClass);
                    }
                    serverSession.WebSocketEndPoint = request.EventCallBackChannel;
                }
                if (methodCallMessage.MethodName == StandardActions.GetAccessToken)
                {
                    responseMessage.Web = methodCallMessage.Web;
                    responseMessage.ReAuthenticate = true;
                    responseMessage.ChannelUri = request.ChannelUri;
                    responseMessage.InternalChannelUri = request.InternalChannelUri;
                    responseMessage.ServerSession = serverSession;
                    var authUser = serverSession.GetAuthData(methodCallMessage);
                    if (methodCallMessage.ChannelUri == "local-device" && !string.IsNullOrWhiteSpace(methodCallMessage.X_Auth_Token))
                    {
                        if (DeviceAuthentication.UnInitialized)
                            (MonoStateClass.GetInstance(typeof(DeviceAuthentication), true) as DeviceAuthentication).InternalAuthIDTokenChanged(methodCallMessage.X_Auth_Token, authUser);
                        else if (DeviceAuthentication.AuthUser == null && authUser != null)
                            (MonoStateClass.GetInstance(typeof(DeviceAuthentication), true) as DeviceAuthentication).InternalAuthIDTokenChanged(methodCallMessage.X_Auth_Token, authUser);

                    }
                    responseMessage.X_Access_Token = serverSession.X_Access_Token;
                    responseMessage.Marshal();
                }

                string json = JsonConvert.SerializeObject(responseMessage);
                return new ResponseData(request.ChannelUri) { IsSucceeded = responseMessage.Exception == null, SessionIdentity = request.SessionIdentity, details = json, InitCommunicationSession = initCommunicationSession };
            }
            catch (Exception error)
            {
                responseMessage = new ReturnMessage(request.ChannelUri);
                responseMessage.Web = methodCallMessage.Web;
                responseMessage.ReAuthenticate = methodCallMessage.ReAuthenticate;
                responseMessage.ChannelUri = request.ChannelUri;
                responseMessage.InternalChannelUri = request.InternalChannelUri;
                if (serverSession != null)
                {
                    responseMessage.ServerSession = serverSession;
                    responseMessage.ChannelUri = request.ChannelUri;
                }

                responseMessage.Exception = new RestApiExceptionData(ExceptionCode.ServerError, error);
                string json = JsonConvert.SerializeObject(responseMessage);
                return new ResponseData(request.ChannelUri) { details = json };
            }
        }


        /// <MetaDataID>{f6a0e01f-730e-44d2-8e16-6f02c277364b}</MetaDataID>
        public static async Task<string> TryProcessMessageAsync(string messageData, object dataContext)
        {
            try
            {
                //System.Runtime.Remoting.Messaging.CallContext.SetData("DataContext", dataContext);
                RequestData request = new RequestData();
                request.SetCallContextData("DataContext", dataContext);
                request.RequestType = RequestType.MethodCall;
                request.details = messageData;
                ResponseData responseData = await MessageDispatchAsync(request);
                string returnMessageData = responseData.details;
                return returnMessageData;

            }
            finally
            {
                //System.Runtime.Remoting.Messaging.CallContext.SetData("DataContext", null);
            }

        }

        /// <MetaDataID>{b3fc4eac-9e5a-44c9-bc7b-74d8909a14fd}</MetaDataID>
        public static bool TryProcessMessage(string messageData, object dataContext, out string returnMessageData)
        {
            try
            {
                System.Runtime.Remoting.Messaging.CallContext.SetData("DataContext", dataContext);
                RequestData request = new RequestData();
                request.RequestType = RequestType.MethodCall;
                request.details = messageData;
                ResponseData responseData = MessageDispatch(request);
                returnMessageData = responseData.details;
                return true;

            }
            finally
            {
                System.Runtime.Remoting.Messaging.CallContext.SetData("DataContext", null);
            }

            //returnMessageData = null;
            //ReturnMessage responseMessage = new ReturnMessage();
            //ServerSessionPart serverSession = null;

            //MethodCallMessage methodCallMessage = JsonConvert.DeserializeObject<MethodCallMessage>(request.details);// Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(request.details);

            //if (methodCallMessage.ChannelUri == null)
            //    return false;

            //ServerSessions.TryGetValue(methodCallMessage.ClientProcessIdentity, out serverSession);

            ////methodCallMessage.X_Access_Token = HttpContext.Current.Request.Headers["X-Access-Token"];
            ////methodCallMessage.X_Auth_Token = HttpContext.Current.Request.Headers["X-Auth-Token"];

            //if (methodCallMessage.IsRestApiManagerMethodCall)
            //{
            //    if (methodCallMessage.MethodName == StandardActions.GetDataContext)
            //    {
            //        Type type = dataContext.GetType();


            //        responseMessage.ChannelUri = methodCallMessage.ChannelUri;

            //        responseMessage.ChannelUri = methodCallMessage.ChannelUri;
            //        responseMessage.ServerSession = serverSession;
            //        responseMessage.RetVal = dataContext;
            //        responseMessage.Marshall();
            //        //responseMessage.ReturnObjectJson = JsonMarshal(dataContext, methodCallMessage.ChannelUri, serverSession);
            //        responseMessage.ChannelUri = methodCallMessage.ChannelUri;
            //        responseMessage.ReturnType = type.FullName;
            //        returnMessageData = JsonConvert.SerializeObject(responseMessage);
            //        return true;
            //    }

            //    returnMessageData = ProcessRestApiManagerMethodCallNew(methodCallMessage).details;
            //    return true;
            //}

            //try
            //{
            //    if (serverSession == null)
            //    {
            //        serverSession = new ServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity));
            //        ServerSessions[methodCallMessage.ClientProcessIdentity] = serverSession;
            //    }

            //    var authUser = serverSession.GetAuthData(methodCallMessage);

            //    System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);

            //    responseMessage.ChannelUri = methodCallMessage.ChannelUri;
            //    responseMessage.ServerSession = serverSession;
            //    try
            //    {
            //        ProxyType httpProxyType = null;
            //        if (!ServerSessions.TryGetValue(methodCallMessage.ClientProcessIdentity, out serverSession))
            //        {
            //            serverSession = new ServerSessionPart(Guid.Parse(methodCallMessage.ClientProcessIdentity));
            //            ServerSessions[methodCallMessage.ClientProcessIdentity] = serverSession;
            //        }
            //        if (!string.IsNullOrWhiteSpace(methodCallMessage.X_Access_Token) && string.IsNullOrWhiteSpace(serverSession.X_Access_Token))
            //        {
            //            responseMessage.ChannelUri = methodCallMessage.ChannelUri;

            //            responseMessage.Exception = new RestApiExceptionData();
            //            responseMessage.Exception.ExceptionMessage = "X_Access_Token expired";
            //            responseMessage.Exception.ExceptionCode = 12;
            //            returnMessageData = JsonConvert.SerializeObject(responseMessage);
            //            return true;
            //            //return new ResponseData() { details = JsonConvert.SerializeObject(responseMessage) };
            //        }

            //        methodCallMessage.UnmarshallNew();

            //        var methodInfo = methodCallMessage.MethodInfo;
            //        object[] args = methodCallMessage.Args.ToArray();
            //        object retVal = methodInfo.Invoke(methodCallMessage.Object, args);


            //        //object[] outArgs = methodCallMessage.GetOutArgs(args);
            //        //responseMessage.JsonOutArgs = Newtonsoft.Json.JsonConvert.SerializeObject(outArgs);
            //        //responseMessage.ChannelUri = methodCallMessage.ChannelUri;
            //        //responseMessage.ReturnObjectJson = JsonMarshal(retVal, methodCallMessage.ChannelUri, serverSession);
            //        //responseMessage.ReturnType = methodInfo.ReturnType.FullName;

            //        responseMessage.MethodInfo = methodInfo;
            //        responseMessage.OutArgs = methodCallMessage.GetOutArgs(args);
            //        responseMessage.RetVal = retVal;
            //        responseMessage.ServerSession = serverSession;
            //        responseMessage.Marshall();



            //        responseMessage.X_Access_Token = serverSession.X_Access_Token;


            //        if (!serverSession.MarshalledTypes.TryGetValue(methodInfo.ReturnType, out httpProxyType))
            //        {

            //            httpProxyType = new ProxyType(methodInfo.ReturnType);
            //            responseMessage.ReturnTypeMetaData = httpProxyType;
            //            serverSession.MarshalledTypes[methodInfo.ReturnType] = httpProxyType;
            //        }
            //        string json = JsonConvert.SerializeObject(responseMessage, new JsonConverterByRef(responseMessage.ChannelUri, "", serverSession));
            //        returnMessageData = json;
            //        return true;
            //        //return new ResponseData() { details = json };
            //    }
            //    finally
            //    {
            //        System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", null);
            //    }

            //    //string javaScriptProxy = GetJavaScriptProxy(type);
            //}
            //catch (Exception error)
            //{
            //    throw new TypeLoadException("Error loading " + methodCallMessage.ObjectUri);
            //}

            //return false;
        }

        #region deletede code JsonMarshal
        ///// <MetaDataID>{f94aafc3-838a-4cd3-94b3-d98e380d0be2}</MetaDataID>
        //private static string JsonMarshal(object _obj, string channelUri, ServerSessionPart serverSessionPart)
        //{

        //    if (_obj == null)
        //        return null;
        //    string internalChannelUri = System.Runtime.Remoting.Messaging.CallContext.GetData("internalChannelUri") as string;

        //    if (_obj is IExtMarshalByRefObject)
        //    {

        //        SerializedData serializedData = null;
        //        string uri = System.Runtime.Remoting.RemotingServices.GetObjectUri(_obj as MarshalByRefObject);
        //        if (uri == null)
        //        {
        //            var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(_obj as MarshalByRefObject) as Proxy;
        //            if (proxy != null)
        //            {
        //                serializedData = new SerializedData();
        //                serializedData.Ref = proxy.ObjectRef;
        //                return JsonConvert.SerializeObject(serializedData, new JsonConverterByRef(channelUri, internalChannelUri, serverSessionPart));
        //            }
        //            uri = System.Runtime.Remoting.RemotingServices.Marshal(_obj as MarshalByRefObject).URI;
        //        }
        //        ProxyType httpProxyType = null;
        //        Type type = _obj.GetType();
        //        if (!serverSessionPart.MarshalledTypes.TryGetValue(type, out httpProxyType))
        //        {
        //            httpProxyType = new ProxyType(type);
        //            serverSessionPart.MarshalledTypes[type] = httpProxyType;
        //        }
        //        ByRef byref = new ByRef(uri, channelUri, internalChannelUri, _obj.GetType().AssemblyQualifiedName, httpProxyType);
        //        byref.CachingObjectMemberValues(_obj);
        //        //byref.Uri = uri;
        //        //byref.ChannelUri = channelUri;
        //        //byref.ReturnTypeMetaData = httpProxyType;
        //        //byref.TypeName = _obj.GetType().AssemblyQualifiedName;
        //        serializedData = new SerializedData();
        //        serializedData.Ref = byref;
        //        return JsonConvert.SerializeObject(serializedData, new JsonConverterByRef(channelUri, internalChannelUri, serverSessionPart));
        //    }
        //    else
        //    {
        //        SerializedData serializedData = new SerializedData();
        //        serializedData.Value = _obj;
        //        return JsonConvert.SerializeObject(serializedData, new JsonConverterByRef(channelUri, internalChannelUri, serverSessionPart));
        //    }

        //}

        #endregion

    }


#if !DeviceDotNet
    /// <MetaDataID>{3923fa74-ff3a-4ede-89c9-597ff2020025}</MetaDataID>
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class WCFMessageDispatcher : IMessageDispatcher
    {

        /// <MetaDataID>{a5ceb4e4-2e1e-485b-8ffc-437e93d73762}</MetaDataID>
        public ResponseData MessageDispatch(RequestData request)
        {
            try
            {
                System.Runtime.Remoting.Messaging.CallContext.SetData("internalChannelUri", request.InternalChannelUri);
                System.Runtime.Remoting.Messaging.CallContext.SetData("PublicChannelUri", request.PublicChannelUri);
                try
                {
                    return OOAdvantech.Remoting.RestApi.MessageDispatcher.MessageDispatch(request);
                }
                finally
                {
                    System.Runtime.Remoting.Messaging.CallContext.SetData("internalChannelUri", null);
                    System.Runtime.Remoting.Messaging.CallContext.SetData("PublicChannelUri", null);
                }
            }
            catch (Exception error)
            {
                ResponseData responseData = new ResponseData(request.ChannelUri) { CallContextID = request.CallContextID, SessionIdentity = request.SessionIdentity, details = error.Message };
                return responseData;

            }
        }
    }
#endif





    /// <MetaDataID>{d5e91dd5-d94e-433a-ac9a-2d058b522a9d}</MetaDataID>
    interface ITaskResultGetter
    {
        /// <MetaDataID>{2ec1f40a-739a-490e-b90e-ed7469980a08}</MetaDataID>
        object GetTaskResult(Task task);
    }
    /// <MetaDataID>{6b4ddf69-8a81-4215-9678-dd0d4447a698}</MetaDataID>
    class TaskResultGetter<T> : ITaskResultGetter
    {
        /// <MetaDataID>{689648e4-6f5d-4542-89e4-3acc38505d95}</MetaDataID>
        public object GetTaskResult(Task task)
        {
            return (task as Task<T>).Result;
        }
    }




}