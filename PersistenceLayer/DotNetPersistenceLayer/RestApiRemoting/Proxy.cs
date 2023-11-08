using OOAdvantech.Remoting;
using System;
using System.Reflection;
using OOAdvantech.MetaDataRepository;
using System.Collections;
using OOAdvantech.Json;
using OOAdvantech.Json.Linq;
using System.Runtime.Remoting.Messaging;
using OOAdvantech.Remoting.RestApi.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;

using System.IO.Compression;
using System.IO;
using System.Text;
using System.Diagnostics;
#if PORTABLE
using System.PCL.Reflection;
#else

using System.Reflection;
#endif

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{dcb19a8a-97b0-4034-9465-b73c9f189832}</MetaDataID>
    public class Proxy : System.Runtime.Remoting.Proxies.RealProxy, IProxy, System.Runtime.Remoting.IRemotingTypeInfo, ITransparentProxy
    {
        /// <MetaDataID>{76551f3e-fdc2-4928-bb34-bbe2b089f204}</MetaDataID>
        string Uri;
        /// <MetaDataID>{94715f0c-f788-4418-8591-f9a516149525}</MetaDataID>
        public string ChannelUri { get; set; }

        internal TypeScriptProxy TypeScriptProxy;

        //public string PublicChannelUri
        //{
        //    get
        //    {
        //        if (ChannelUri.IndexOf("(") != -1)
        //            return ChannelUri.Substring(0, ChannelUri.IndexOf("("));
        //        else
        //            return ChannelUri;

        //    }
        //}

        //public string InternalChannelUri { get; set; }

        /// <MetaDataID>{7657cf2e-2e32-4326-8943-6102b93c62bd}</MetaDataID>
        string _TypeFullName;
        /// <MetaDataID>{f98b2269-d219-491b-9276-a8613951733d}</MetaDataID>
        System.Type Type;

        /// <MetaDataID>{00022c8a-af64-441a-8952-430fc0634a87}</MetaDataID>
        System.Type ServerObjectType;

        ///// <MetaDataID>{f950e8f9-b5f6-47e3-a4aa-7502a491ec67}</MetaDataID>
        //protected Proxy(string objectUri, string channelUri, string internalChannelUri, System.Type type)
        //          : base(typeof(MarshalByRefObject))
        //{
        //    _TypeFullName = type.FullName;
        //    Type = type;
        //    Uri = objectUri;
        //    InternalChannelUri = internalChannelUri;
        //    ChannelUri = channelUri;
        //}


        /// <MetaDataID>{d4a593a6-00d4-41b2-ad64-e0a8322d57cb}</MetaDataID>
        public ObjRef ObjectRef;

        /// <MetaDataID>{cbed457e-a960-4e96-9cbe-04def95ff9df}</MetaDataID>
        internal ProxyType ProxyType;
        /// <MetaDataID>{d3eb0a29-f618-46ad-a29e-e65ff00a3190}</MetaDataID>
        public Proxy(ObjRef objectRef, Type type = null) : base(typeof(MarshalByRefObject))
        {
            EventConsumingResolver = new EventConsumingResolver(this, RemotingServices.CurrentRemotingServices);
            ObjectRef = objectRef;
            Uri = objectRef.Uri;
            ChannelUri = objectRef.ChannelUri;
            //this.InternalChannelUri = objectRef.InternalChannelUri;
            ProxyType = objectRef.GetProxyType();


            if (type != null)
            {
                Type = type;
#if !DeviceDotNet
                if (ProxyType != null && ProxyType.AssemblyQualifiedName != null)
                    ServerObjectType = System.Type.GetType(ProxyType.AssemblyQualifiedName, false);
#endif
            }
            else
                Type = System.Type.GetType(ProxyType.AssemblyQualifiedName);


        }

        /// <MetaDataID>{e7c8af5b-e697-4b57-8bfb-c9039c6a7c3b}</MetaDataID>
        public void ReconnectToServerObject(ObjRef objectRef)
        {
            if (ObjectRef.Uri != objectRef.Uri)
            {
                ObjectRef = objectRef;
                Uri = objectRef.Uri;
                ChannelUri = objectRef.ChannelUri;



                int nPos = Uri.IndexOf("#MonoStateClass#");
                if (nPos != -1)
                {
                    Guid sessionID = RenewalManager.GetSession(ChannelUri, false, RemotingServices.CurrentRemotingServices).ClientProcessIdentity;
                    string persistentUri = Uri.Substring(nPos);
                    persistentUri = persistentUri.Replace("#MonoStateClass#", "");
                    nPos = persistentUri.IndexOf(@"\");
                    string monoStateClassChannelUri = persistentUri.Substring(nPos + 1);
                    _TypeFullName = persistentUri.Substring(0, nPos);
                    _ObjectUri = new ExtObjectUri(Uri, null, _TypeFullName, ChannelUri, sessionID);

                }
                else
                {
                    nPos = Uri.IndexOf("#PID#");
                    if (nPos != -1)
                    {
                        string persistentUri = Uri.Substring(nPos);
                        persistentUri = persistentUri.Replace("#PID#", "");
                        _ObjectUri = new ExtObjectUri(Uri, persistentUri, null, null, RenewalManager.GetSession(ChannelUri, true, RemotingServices.CurrentRemotingServices).ClientProcessIdentity);
                    }
                    else
                        _ObjectUri = new ExtObjectUri(Uri, null, null, null, RenewalManager.GetSession(ChannelUri, true, RemotingServices.CurrentRemotingServices).ClientProcessIdentity);

                    if (ProxyType?.Name == "PreparationStation")
                    {
                        if (nPos == -1)
                        {

                        }
                    }
                }
            }

        }


        /// <MetaDataID>{d43cf8ba-6993-4319-8e65-0b497c15843f}</MetaDataID>
        internal void ControlRemoteObjectLifeTime()
        {

            int nPos = Uri.IndexOf("#MonoStateClass#");
            if (nPos != -1)
            {
                //Guid sessionID = Guid.Empty;
                //if (type == typeof(OOAdvantech.Remoting.RestApi.RemotingServicesServer))
                //{
                //    ClientSessionPart clientSessionPart = RenewalManager.GetSession(ChannelUri, false, RemotingServices.CurrentRemotingServices);
                //    if (clientSessionPart != null)
                //        sessionID = clientSessionPart.SessionIdentity;
                //}
                //else
                var session = RenewalManager.GetSession(ChannelUri, false, RemotingServices.CurrentRemotingServices);
                if (session == null)
                {

                }
                Guid sessionID = session.ClientProcessIdentity;


                string persistentUri = Uri.Substring(nPos);
                persistentUri = persistentUri.Replace("#MonoStateClass#", "");
                nPos = persistentUri.IndexOf(@"\");
                string monoStateClassChannelUri = persistentUri.Substring(nPos + 1);
                _TypeFullName = persistentUri.Substring(0, nPos);

                _ObjectUri = new ExtObjectUri(Uri, null, _TypeFullName, ChannelUri, sessionID);

            }
            else
            {
                if (ProxyType?.Name == "PreparationStation")
                {
                }
                nPos = Uri.IndexOf("#PID#");
                if (nPos != -1)
                {
                    string persistentUri = Uri.Substring(nPos);
                    persistentUri = persistentUri.Replace("#PID#", "");
                    _ObjectUri = new ExtObjectUri(Uri, persistentUri, null, null, RenewalManager.GetSession(ChannelUri, true, RemotingServices.CurrentRemotingServices).ClientProcessIdentity);
                }
                else
                    _ObjectUri = new ExtObjectUri(Uri, null, null, null, RenewalManager.GetSession(ChannelUri, true, RemotingServices.CurrentRemotingServices).ClientProcessIdentity);

                if (ProxyType?.Name == "PreparationStation")
                {
                    if (nPos == -1)
                    {

                    }
                }

            }
            RenewalManager.AddProxy(this, RemotingServices.CurrentRemotingServices);
        }

        /// <MetaDataID>{655e01bc-d385-4c83-9371-1d2796c3580c}</MetaDataID>
        public object Target;


        /// <MetaDataID>{2bfebbd9-d7f1-416d-b176-04a3a3c82c67}</MetaDataID>
        ExtObjectUri _ObjectUri;

        /// <MetaDataID>{24638f92-9d5a-4cca-9ada-672771cfb378}</MetaDataID>
        ExtObjectUri IProxy.ObjectUri
        {
            get
            {
                return _ObjectUri;
            }
        }

        /// <MetaDataID>{747cef73-2182-44f0-bf17-4dff10cd8424}</MetaDataID>
        public string TypeFullName
        {
            get
            {
                return ProxyType.FullName;
            }
            set
            {

            }

        }


        /// <MetaDataID>{6df651ca-9e96-4f58-95d0-96259844da4e}</MetaDataID>
        public string TypeName
        {
            get
            {
                return ProxyType.Name;
            }
            set
            {

            }

        }

        /// <MetaDataID>{b7429903-e63c-4a10-8943-8c36b7d25c73}</MetaDataID>
        public EventConsumingResolver EventConsumingResolver { get; set; }

        /// <MetaDataID>{449d5173-3254-49e5-9077-efa18c6db7ee}</MetaDataID>
        string IProxy.Uri => Uri;

        //string IProxy.ChannelUri { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //EventConsumingResolver IProxy.EventConsumingResolver { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

#if DeviceDotNet
        /// <MetaDataID>{1e88ef55-2ba9-4822-a580-4338e5a68ab9}</MetaDataID>
        public object Invoke(Type type, string methodName, object[] args, Type[] argsTypes)
        {
            if (type == typeof(ITransparentProxy) && methodName == "GetProxy")
                return this;



            var clientSessionPart = RenewalManager.GetSession(ChannelUri, false, RemotingServices.CurrentRemotingServices);


            if (methodName == "GetLifetimeService")
            {

            }

            bool LocalCall;
            object retval = TryInvokeLocal(type, methodName, args, argsTypes, out LocalCall);
            if (LocalCall)
                return retval;

            var methodCallMessage = new MethodCallMessage(ChannelUri, Uri, clientSessionPart.ClientProcessIdentity.ToString(), type.FullName, methodName, args);
            methodCallMessage.Marshal();

            RequestData requestData = new RequestData();
            requestData.ChannelUri = ChannelUri;
            requestData.SessionIdentity= clientSessionPart.SessionIdentity;
            string myJson = null;

            //System.Net.Http.StringContent content = null;

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;


            string X_Auth_Token = null;
            string X_Access_Token = null;

        #region Gets authentication data
            if (authUser != null)
            {
                var exp = authUser.ExpirationTime.ToString();
                if (authUser.AuthToken != clientSessionPart.X_Auth_Token)
                {
                    clientSessionPart.X_Access_Token = null;
                    clientSessionPart.X_Auth_Token = null;
                }
                if (!string.IsNullOrWhiteSpace(clientSessionPart.X_Access_Token))
                {
                    methodCallMessage.X_Access_Token = clientSessionPart.X_Access_Token;
                    X_Access_Token = clientSessionPart.X_Access_Token;
                }
                else
                {
                    clientSessionPart.X_Auth_Token = authUser.AuthToken;
                    methodCallMessage.X_Auth_Token = authUser.AuthToken;
                    X_Auth_Token = clientSessionPart.X_Auth_Token;
                }
            }
        #endregion

            Dictionary<string, List<string>> cachingMetadata = System.Runtime.Remoting.Messaging.CallContext.GetData("CachingMetadata") as  Dictionary<string, List<string>>;
            if (cachingMetadata != null)
                requestData.CachingMetadata=Json.JsonConvert.SerializeObject(cachingMetadata);


            requestData.details = JsonConvert.SerializeObject(methodCallMessage);

            // string channelUri = ChannelUri;
            //string internalchannelUri = null;
            //if (channelUri.IndexOf("(") != -1)
            //{
            //    internalchannelUri = channelUri.Substring(channelUri.IndexOf("(") + 1);
            //    internalchannelUri = internalchannelUri.Substring(0, internalchannelUri.Length - 1);
            //    channelUri = channelUri.Substring(0, channelUri.IndexOf("("));
            //    requestData.InternalChannelUri = internalchannelUri;
            //}
            var responseData = (clientSessionPart as ClientSessionPart).Channel.ProcessRequest(requestData);
            //var responseData = RemotingServices.Invoke(channelUri, requestData, X_Auth_Token, X_Access_Token);
            if (responseData != null)// .IsSuccessStatusCode)
            {



                ReturnMessage returnMessage = null;

                try
                {
                    returnMessage = OOAdvantech.Json.JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);
                    if (returnMessage.ServerSessionObjectRef != null)
                    {

                    }
                    try
                    {
                        returnMessage = JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);
                        if (cachingMetadata != null)
                        {
                            //var cmp = StringCompression.Compress(returnMessage.ReturnObjectJson);
                            var bytes = Encoding.UTF8.GetBytes(returnMessage.ReturnObjectJson);
                            Stopwatch timer = new Stopwatch();
                            timer.Start();
                            bytes = WebSocketChannel.Compress(bytes);
                            bytes = WebSocketChannel.DeCompress(bytes);
                            timer.Stop();
                            var json = Encoding.UTF8.GetString(bytes);

                            var sp = timer.ElapsedMilliseconds;

                            //var mBuffer = StringCompression.Zip(returnMessage.ReturnObjectJson);
                            //var json = StringCompression.Unzip(mBuffer);
                        }
                    }
                    catch (Exception error)
                    {

                        throw;
                    }
                }
                catch (Exception error)
                {

                    throw;
                }

                if (!string.IsNullOrWhiteSpace(returnMessage.X_Access_Token))
                    clientSessionPart.X_Access_Token = returnMessage.X_Access_Token;
                else
                    clientSessionPart.X_Access_Token = null;

                retval = UnMarshal(type.GetMetaData().GetMethod(methodName, argsTypes), returnMessage, args);
                if (returnMessage.ServerSessionObjectRef != null)
                    (clientSessionPart as ClientSessionPart).UpdateServerSessionPart(returnMessage.ServerSessionObjectRef);


                return retval;

            }
            else
                throw new InvalidOperationException();


            return null;
        }
#endif
        /// <MetaDataID>{17ea5e1f-d078-4dad-b2e3-ee1af7f5920a}</MetaDataID>
        private object TryInvokeLocal(Type type, string methodName, object[] args, Type[] argsTypes, out bool localCall)
        {


            System.Reflection.MethodBase methodBase = type.GetRuntimeMethod(methodName, argsTypes);
            localCall = false;
            object[] outArgs = null;
            object returnValue = null;

            if (methodBase.Name.IndexOf("get_") == 0)
            {
                string propertyName = methodBase.Name.Substring("get_".Length);
                var propInfo = methodBase.DeclaringType.GetMetaData().GetProperty(propertyName);
                object value = null;
                if (propInfo != null && ObjectRef.MembersValues != null && ObjectRef.MembersValues.TryGetValue(propertyName, out value))
                {
                    if (ObjectRef.InvalidMembersValues)
                    {

                        var remotingServices = RemotingServices.GetRemotingServices((this).ChannelUri);
                        var _object = GetTransparentProxy(Type);
                        remotingServices.RefreshCacheData(_object as MarshalByRefObject);
                        ObjectRef.InvalidMembersValues = false;
                        ObjectRef.MembersValues.TryGetValue(propertyName, out value);
                    }

                    outArgs = new object[0];

                    if (value != null && !propInfo.PropertyType.IsInstanceOfType(value))
                    {
#if !DeviceDotNet

                        value = System.Convert.ChangeType(value, propInfo.PropertyType);
#else
                        if (value is TransparentProxy)
                            value = ((value as OOAdvantech.Remoting.RestApi.ITransparentProxy).GetProxy() as RestApi.Proxy).GetTransparentProxy(propInfo.PropertyType);
                        else
                        {
                            if (propInfo.PropertyType.BaseType == typeof(System.Enum))
                                value = System.Enum.ToObject(propInfo.PropertyType, (int)System.Convert.ChangeType(value, typeof(int)));
                            else
                                value = System.Convert.ChangeType(value, propInfo.PropertyType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
                        }
#endif
                        ObjectRef.MembersValues[propertyName] = value;
                    }
                    returnValue = value;
                    localCall = true;
                }
            }

            if (!localCall)
            {
                if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                    methodBase.Name == "GetType")
                {
                    outArgs = new object[0];
                    if (ServerObjectType == null)
                        returnValue = Type;
                    else
                        returnValue = ServerObjectType;

                    localCall = true;
                }
                else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                    methodBase.Name == "GetHashCode")
                {

                    outArgs = new object[0];
                    if (string.IsNullOrWhiteSpace(_ObjectUri.PersistentUri))
                    {

                        if (string.IsNullOrEmpty(_ObjectUri.TransientUri))
                            returnValue = Uri.GetHashCode();
                        else
                            returnValue = _ObjectUri.TransientUri.GetHashCode();
                    }
                    else
                        returnValue = _ObjectUri.PersistentUri.GetHashCode();


                    localCall = true;
                }
                else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                   methodBase.Name == "Equals")
                {
                    bool IsEquals = true;
                    Object obj = args[0];

                    if (!(obj is MarshalByRefObject))
                    {
                        IsEquals = false;
                    }
                    else
                    {
                        var objProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(obj) as Proxy;
                        ExtObjectUri objUri = null;
                        if (objProxy == null)
                            IsEquals = false;
                        else
                        {
                            objUri = objProxy._ObjectUri;

                            if (objUri == _ObjectUri && _ObjectUri != null)
                                IsEquals = true;
                            else
                                IsEquals = false;
                        }
                    }
                    outArgs = new object[0];
                    returnValue = IsEquals;
                    localCall = true;
                }
                else if (methodBase.Name.IndexOf("add_") == 0)
                {

                    if (methodBase.DeclaringType == typeof(ITransparentProxy))
                    {
                        System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetEvent(methodBase.Name.Substring("add_".Length));
                        eventInfo.AddEventHandler(this, args[0] as System.Delegate);
                        outArgs = new object[0];
                        localCall = true;
                    }
                    else
                    {
                        System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetMetaData().GetEvent(methodBase.Name.Substring("add_".Length));
                        if (eventInfo != null && eventInfo.AddMethod == methodBase)
                        {
                            EventConsumingResolver.EventConsumerSubscription(args[0] as System.Delegate, eventInfo);
                            outArgs = new object[0];
                            localCall = true;
                        }
                    }
                }
                else if (methodBase.Name.IndexOf("remove_") == 0)
                {
                    if (methodBase.DeclaringType == typeof(ITransparentProxy))
                    {
                        System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetEvent(methodBase.Name.Substring("remove_".Length));
                        eventInfo.RemoveEventHandler(this, args[0] as System.Delegate);
                        outArgs = new object[0];
                        localCall = true;
                    }
                    else
                    {
                        System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetMetaData().GetEvent(methodBase.Name.Substring("remove_".Length));
                        if (eventInfo != null && eventInfo.RemoveMethod == methodBase)
                        {
                            EventConsumingResolver.EventConsumerUnsubscribe(args[0] as System.Delegate, eventInfo);
                            outArgs = new object[0];
                            localCall = true;
                        }
                    }
                }
            }

            if (localCall)
                return returnValue;


            return null;
        }

        /// <MetaDataID>{d4f19be6-02ed-4f06-acc2-21b6711b4fcf}</MetaDataID>
        public T GetValue<T>(object v)
        {
            if (v != null)
                return (T)v;
            else
                return default(T);
        }
        ///// <MetaDataID>{d2014d43-f3cc-4bc8-a374-ed4485b02b74}</MetaDataID>
        //static JsonSerializer CustomJsonSerializer = JsonSerializer.Create(new JsonSerializerSettings() { ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor });

#if !DeviceDotNet
        /// <MetaDataID>{b40dd512-8165-4633-97ca-db0df6fd4e7d}</MetaDataID>
        public override IMessage Invoke(IMessage msg)
        {


            var clientSessionPart = RenewalManager.GetSession(ChannelUri, false, RemotingServices.CurrentRemotingServices);


            if ((msg as System.Runtime.Remoting.Messaging.IMethodMessage).MethodBase.Name == "GetLifetimeService")
            {
            }


            if (msg is IMethodCallMessage)
            {
                bool LocalCall;
                var ReturnMessage = TryInvokeLocal(msg, out LocalCall) as IMethodReturnMessage; ;
                if (LocalCall)
                    return ReturnMessage;
            }

            var methodCallMessage = new MethodCallMessage(ChannelUri, Uri, clientSessionPart.ClientProcessIdentity.ToString("N"), (msg as IMethodMessage).TypeName, (msg as IMethodMessage).MethodBase.Name, (msg as IMethodMessage).Args);
            methodCallMessage.Marshal();

            RequestData requestData = new RequestData();
            requestData.SessionIdentity = clientSessionPart.SessionIdentity;
            requestData.ChannelUri = ChannelUri;

            if ((msg as System.Runtime.Remoting.Messaging.IMethodMessage).MethodBase.Name == "get_GraphicMenus")
            {

            }



            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;


            string X_Auth_Token = null;
            string X_Access_Token = null;

            #region Gets authentication data
            if (authUser != null)
            {
                if (authUser.AuthToken != clientSessionPart.X_Auth_Token)
                {
                    clientSessionPart.X_Access_Token = null;
                    clientSessionPart.X_Auth_Token = null;
                }
                if (!string.IsNullOrWhiteSpace(clientSessionPart.X_Access_Token))
                {
                    methodCallMessage.X_Access_Token = clientSessionPart.X_Access_Token;
                    X_Access_Token = clientSessionPart.X_Access_Token;
                }
                else
                {
                    clientSessionPart.X_Auth_Token = authUser.AuthToken;
                    methodCallMessage.X_Auth_Token = authUser.AuthToken;
                    X_Auth_Token = clientSessionPart.X_Auth_Token;
                }
            }
            #endregion

            Dictionary<string, List<string>> cachingMetadata = System.Runtime.Remoting.Messaging.CallContext.GetData("CachingMetadata") as Dictionary<string, List<string>>;
            if (cachingMetadata != null)
                requestData.CachingMetadata = Json.JsonConvert.SerializeObject(cachingMetadata);

            if (methodCallMessage.MethodName=="RefreshTest")
            {

            }

            requestData.RequestType = RequestType.MethodCall;
            if (this.ObjectRef.MembersValues?.Count > 0==true)
                requestData.HasCachingMembers=true;

            requestData.details = JsonConvert.SerializeObject(methodCallMessage);


            requestData.ChannelUri = ChannelUri;

            var responseData = (clientSessionPart as ClientSessionPart).Channel.ProcessRequest(requestData); //RemotingServices.Invoke(requestData.PublicChannelUri, requestData, X_Auth_Token, X_Access_Token);

            if (responseData != null)// .IsSuccessStatusCode)
            {

                if (methodCallMessage.MethodName=="GetDelayedServingBatchesAtTheCounter")
                {

                }
                ReturnMessage returnMessage = null;
                try
                {
                    returnMessage = JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);
                    if (cachingMetadata != null)
                    {
                        // var cmp = StringCompression.Compress(returnMessage.ReturnObjectJson);
                        var bytes = Encoding.UTF8.GetBytes(returnMessage.ReturnObjectJson);
                        Stopwatch timer = new Stopwatch();
                        timer.Start();
                        bytes = WebSocketChannel.Compress(bytes);
                        bytes = WebSocketChannel.DeCompress(bytes);
                        timer.Stop();
                        var json = Encoding.UTF8.GetString(bytes);

                        var sp = timer.ElapsedMilliseconds;

                        //var mBuffer = StringCompression.Zip(returnMessage.ReturnObjectJson);
                        //var json = StringCompression.Unzip(mBuffer);
                    }
                }
                catch (Exception error)
                {

                    throw;
                }
                if (returnMessage.Exception != null && returnMessage.Exception.ExceptionCode == ExceptionCode.AccessTokenExpired)
                {
                    methodCallMessage.X_Auth_Token = authUser.AuthToken;
                    methodCallMessage.X_Access_Token = null;
                    requestData.RequestType = RequestType.MethodCall;
                    requestData.details = JsonConvert.SerializeObject(methodCallMessage);
                    requestData.ChannelUri = ChannelUri;
                    responseData = (clientSessionPart as ClientSessionPart).Channel.ProcessRequest(requestData); //RemotingServices.Invoke(requestData.PublicChannelUri, requestData, X_Auth_Token, X_Access_Token);
                    if (responseData != null)// .IsSuccessStatusCode)
                    {
                        try
                        {
                            returnMessage = JsonConvert.DeserializeObject<ReturnMessage>(responseData.details);


                        }
                        catch (Exception error)
                        {
                            throw;
                        }
                    }
                    else
                        throw new InvalidOperationException();

                }
                if (!string.IsNullOrWhiteSpace(returnMessage.X_Access_Token))
                    clientSessionPart.X_Access_Token = returnMessage.X_Access_Token;
                else
                    clientSessionPart.X_Access_Token = null;
                var message = UnMarshal((msg as IMethodCallMessage), returnMessage);
                if (responseData.UpdateCaching)
                {

                    Dictionary<string, List<string>> cachingMembers = new Dictionary<string, List<string>>();
                    cachingMembers[""]=this.ObjectRef.MembersValues.Keys.ToList();

                    try
                    {
                        if (cachingMembers.Count>0)
                            CallContext.SetData("CachingMetadata", cachingMembers);


                        RemotingServices.RefreshCacheData(GetTransparentProxy(base.GetProxiedType()) as MarshalByRefObject);
                    }
                    catch (Exception error)
                    {
                    }

                }
                return message;
            }
            else
                throw new InvalidOperationException();
        }
#else
        public IMessage Invoke(IMessage msg)
        {
            throw new NotImplementedException();
        }
#endif


        /// <MetaDataID>{730ec44e-4050-4a5b-b924-7a95f1cb115c}</MetaDataID>
        internal static object CastRemoteObject(object returnValue, Type type)
        {
#if DeviceDotNet
            if (returnValue != null && !type.GetMetaData().IsAssignableFrom(returnValue.GetType()))
            {
                return ((returnValue as ITransparentProxy).GetProxy() as Proxy).GetTransparentProxy(type);


            }
            else
                return returnValue;
#else
            return returnValue;
#endif
        }


#if !DeviceDotNet
        /// <MetaDataID>{7daeb6f7-2e82-4df6-9f1b-94c94d1fd0d9}</MetaDataID>
        public System.Runtime.Remoting.Messaging.IMessage TryInvokeLocal(System.Runtime.Remoting.Messaging.IMessage msg, out bool localCall)
        {
            //string HostName=System.Net.Dns.GetHostName();
            IMethodReturnMessage ReturnMessage = null;
            System.Reflection.MethodBase methodBase = (msg as IMethodCallMessage).MethodBase;
            localCall = false;
            object[] outArgs = null;
            object returnValue = null;

            if (methodBase.Name.IndexOf("get_") == 0)
            {
                string propertyName = methodBase.Name.Substring("get_".Length);
                var propInfo = methodBase.DeclaringType.GetProperty(propertyName);
                object value = null;
                if (propInfo != null && ObjectRef.MembersValues != null && ObjectRef.MembersValues.TryGetValue(propertyName, out value))
                {

                    if (value != null && !propInfo.PropertyType.IsInstanceOfType(value))
                    {
#if !DeviceDotNet
                        if (propInfo.PropertyType.BaseType == typeof(System.Enum))
                            value = System.Enum.ToObject(propInfo.PropertyType, (int)System.Convert.ChangeType(value, typeof(int)));
                        else
                        {
                            if (propInfo.PropertyType.IsGenericType && !propInfo.PropertyType.IsGenericTypeDefinition && propInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {

                                value = System.Convert.ChangeType(value, propInfo.PropertyType.GenericTypeArguments[0]);

                                value = Activator.CreateInstance(propInfo.PropertyType, value);
                            }
                            else
                                value = System.Convert.ChangeType(value, propInfo.PropertyType);
                        }
#else
                        value = System.Convert.ChangeType(value, propInfo.PropertyType, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
#endif
                        ObjectRef.MembersValues[propertyName] = value;
                    }


                    outArgs = new object[0];
                    returnValue = value;
                    localCall = true;
                }
            }
            if (methodBase.Name.IndexOf("set_") == 0)
            {
                string propertyName = methodBase.Name.Substring("set_".Length);
                var propInfo = methodBase.DeclaringType.GetProperty(propertyName);
                object value = null;
                if (propInfo != null && ObjectRef.MembersValues != null && ObjectRef.MembersValues.TryGetValue(propertyName, out value))
                {
                    ObjectRef.MembersValues[propertyName] = (msg as IMethodCallMessage).Args[0];
                }
            }
            if (!localCall)
            {
                if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                    methodBase.Name == "GetType")
                {
                    outArgs = new object[0];
                    if (ServerObjectType == null)
                        returnValue = Type;
                    else
                        returnValue = ServerObjectType;

                    localCall = true;
                }
                else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                    methodBase.Name == "GetHashCode")
                {

                    outArgs = new object[0];
                    if (string.IsNullOrEmpty(_ObjectUri.PersistentUri))
                    {

                        if (string.IsNullOrEmpty(_ObjectUri.TransientUri))
                            returnValue = Uri.GetHashCode();
                        else
                            returnValue = _ObjectUri.TransientUri.GetHashCode();
                    }
                    else
                        returnValue = _ObjectUri.PersistentUri.GetHashCode();


                    localCall = true;
                }
                else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                   methodBase.Name == "Equals")
                {
                    bool IsEquals = true;
                    Object obj = (msg as IMethodCallMessage).Args[0];

                    if (!(obj is MarshalByRefObject))
                    {
                        IsEquals = false;
                    }
                    else
                    {
                        var objProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(obj) as Proxy;
                        ExtObjectUri objUri = null;
                        if (objProxy == null)
                            IsEquals = false;
                        else
                        {
                            objUri = objProxy._ObjectUri;

                            if (objUri == _ObjectUri && _ObjectUri != null)
                                IsEquals = true;
                            else
                                IsEquals = false;
                        }
                    }
                    outArgs = new object[0];
                    returnValue = IsEquals;
                    localCall = true;
                }
                else if (methodBase.Name.IndexOf("add_") == 0)
                {
                    if (methodBase.DeclaringType == typeof(ITransparentProxy))
                    {
                        System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetEvent(methodBase.Name.Substring("add_".Length));
                        eventInfo.AddEventHandler(this, (msg as IMethodCallMessage).Args[0] as System.Delegate);
                        outArgs = new object[0];
                        localCall = true;
                    }
                    else
                    {
                        System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetEvent(methodBase.Name.Substring("add_".Length));
                        if (eventInfo != null && eventInfo.GetAddMethod() == methodBase)
                        {
                            EventConsumingResolver.EventConsumerSubscription((msg as IMethodCallMessage).Args[0] as System.Delegate, eventInfo);
                            outArgs = new object[0];
                            localCall = true;
                        }
                    }
                }
                else if (methodBase.Name.IndexOf("remove_") == 0)
                {
                    if (methodBase.DeclaringType == typeof(ITransparentProxy))
                    {
                        System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetEvent(methodBase.Name.Substring("remove_".Length));
                        eventInfo.RemoveEventHandler(this, (msg as IMethodCallMessage).Args[0] as System.Delegate);
                        outArgs = new object[0];
                        localCall = true;
                    }
                    else

                    {
                        System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetEvent(methodBase.Name.Substring("remove_".Length));
                        if (eventInfo != null && eventInfo.GetRemoveMethod() == methodBase)
                        {
                            EventConsumingResolver.EventConsumerUnsubscribe((msg as IMethodCallMessage).Args[0] as System.Delegate, eventInfo);

                            outArgs = new object[0];
                            localCall = true;
                        }
                    }
                }
                else if (methodBase.DeclaringType == typeof(ITransparentProxy) && methodBase.Name == "GetProxy")
                {
                    returnValue = this;
                    outArgs = new object[0];
                    localCall = true;
                }

            }

            if (localCall)
            {
                ReturnMessage = new System.Runtime.Remoting.Messaging.ReturnMessage(
                    returnValue,    //ReturnValue
                    outArgs,            //Object[] outArgs
                    outArgs.Length,                 //int outArgsCount
                    (LogicalCallContext)msg.Properties["__CallContext"],                //LogicalCallContext callCtx
                    (System.Runtime.Remoting.Messaging.IMethodCallMessage)msg);
                return ReturnMessage;

            }




            return null;
        }


        /// <MetaDataID>{ac8ceada-ed1e-4696-8818-6460013c8c22}</MetaDataID>
        private static IMessage UnMarshal(IMethodCallMessage methodCallMessage, ReturnMessage returnMessage)
        {
            if (returnMessage.Exception != null)
            {
                System.Exception exception = new ServerException(returnMessage.Exception.ExceptionMessage + Environment.NewLine + returnMessage.Exception.ServerStackTrace, returnMessage.Exception.HResult, returnMessage.Exception.ErrorCode);

                if (returnMessage.Exception.ExceptionCode == ExceptionCode.MissingCollectedFromGC)
                    exception = new MissingServerObjectException(returnMessage.Exception.ExceptionMessage, MissingServerObjectException.MissingServerObjectReason.CollectedFromGC);


                if (returnMessage.Exception.ExceptionCode == ExceptionCode.MissingDeletedFromStorage)
                    exception = new MissingServerObjectException(returnMessage.Exception.ExceptionMessage, MissingServerObjectException.MissingServerObjectReason.DeletedFromStorage);



                return new System.Runtime.Remoting.Messaging.ReturnMessage(exception, methodCallMessage);
            }
            MethodInfo method = methodCallMessage.MethodBase as MethodInfo;

            object[] args = methodCallMessage.Args;
            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.NetTypedValuesJsonSerialization, null, null);// { TypeNameHandling = TypeNameHandling.All, ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null) };


            object[] outArgs = null;
            if (returnMessage.JsonOutArgs != null)
                outArgs = JsonConvert.DeserializeObject<object>(returnMessage.JsonOutArgs, jSetttings) as object[];
            else
                outArgs = new object[0];

            var parameters = method.GetParameters();
            int i = 0;
            int k = 0;
            foreach (var parameter in parameters)
            {
                Type parameterType = parameter.ParameterType;
                if (parameter.ParameterType.IsByRef)
                {
                    parameterType = parameterType.GetElementType();
                    if (parameterType.IsPrimitive && outArgs[i] != null)
                        outArgs[k] = Convert.ChangeType(outArgs[k], parameterType);
                    args[i] = outArgs[k++];
                }
                i++;
            }
            object retObject = null;
            if (returnMessage.ReturnObjectJson.IndexOf("\"MembersValues\":{\"")!= -1)
            {

            }
            if (returnMessage.ReturnObjectJson != null && returnMessage.ReturnObjectJson != "{}")
            {
                if (method.ReturnType != typeof(void))
                {

                    retObject = JsonConvert.DeserializeObject(returnMessage.ReturnObjectJson, method.ReturnType, jSetttings);
                    retObject = Proxy.CastRemoteObject(retObject, method.ReturnType);
                }
                else
                    retObject = JsonConvert.DeserializeObject(returnMessage.ReturnObjectJson, typeof(object), jSetttings);

            }

            var message = new System.Runtime.Remoting.Messaging.ReturnMessage(
                                                             retObject, //Object ret
                                                             args,            //Object[] outArgs 
                                                             args.Length,     //int outArgsCount
                                                             (methodCallMessage as IMethodCallMessage).LogicalCallContext,          //LogicalCallContext callCtx
                                                             (IMethodCallMessage)methodCallMessage);

            return message;
        }

#else

        private static object UnMarshal(MethodInfo method, ReturnMessage returnMessage, object[] args)
        {
            if (returnMessage.Exception != null)
            {
                if (returnMessage.Exception.ExceptionCode == ExceptionCode.ConnectionError)
                    throw new System.Net.WebException(returnMessage.Exception.ExceptionMessage, System.Net.WebExceptionStatus.ConnectFailure);
                else
                    throw new ServerException(returnMessage.Exception.ExceptionMessage + Environment.NewLine + returnMessage.Exception.ServerStackTrace, returnMessage.Exception.HResult,"");

            }

            //var jSetttings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, SerializationBinder = new OOAdvantech.Remoting.RestApi.SerializationBinder(false), ContractResolver = new JsonContractResolver(JsonContractType.Deserialize, null, null, null) };
            var jSetttings = new Serialization.JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.NetTypedValuesJsonSerialization,null, null);

            var outArgs = new object[0];
            if (!string.IsNullOrWhiteSpace(returnMessage.JsonOutArgs))
                outArgs = JsonConvert.DeserializeObject(returnMessage.JsonOutArgs, jSetttings) as object[];

            var parameters = method.GetParameters();
            int i = 0;
            int k = 0;
            foreach (var parameter in parameters)
            {
                Type parameterType = parameter.ParameterType;
                if (parameter.ParameterType.IsByRef)
                {
                    parameterType = parameterType.GetElementType();
                    if (parameterType.GetMetaData().IsPrimitive && outArgs[i] != null)
                        outArgs[k] = Convert.ChangeType(outArgs[k], parameterType);
                    args[i] = outArgs[k++];
                }
                i++;
            }
            object retObject = null;
            if (returnMessage.ReturnObjectJson != null && returnMessage.ReturnObjectJson != "{}")
            {
                if (method.ReturnType != typeof(void))
                {
                    retObject = JsonConvert.DeserializeObject(returnMessage.ReturnObjectJson, method.ReturnType, jSetttings);
                    retObject = Proxy.CastRemoteObject(retObject, method.ReturnType);
                }
                else
                    retObject = JsonConvert.DeserializeObject(returnMessage.ReturnObjectJson, jSetttings);

            }

            return retObject;
            //var message = new System.Runtime.Remoting.Messaging.ReturnMessage(
            //                                                 retObject, //Object ret
            //                                                 args,            //Object[] outArgs 
            //                                                 args.Length,     //int outArgsCount
            //                                                 (methodCallMessage as IMethodCallMessage).LogicalCallContext,          //LogicalCallContext callCtx
            //                                                 (IMethodCallMessage)methodCallMessage);

            //return message;
        }
#endif
        /// <MetaDataID>{09db6d1d-fbae-4a7a-aac1-6c7410a1f2a7}</MetaDataID>
        internal static Proxy GetProxy(string channelUri, string returnObjectUri, string returnType, ProxyType returnTypeMetaData)
        {

            return null;
        }

        ///// <MetaDataID>{3e47e9ab-82cb-4baf-9692-480a815f5b67}</MetaDataID>
        //public void PublishEvent(EventInfo eventInfo, System.Collections.Generic.List<object> args)
        //{
        //    throw new NotImplementedException();
        //}

#if DeviceDotNet
        public object GetTransparentProxy()
        {
            return GetTransparentProxy(typeof(ITransparentProxy));
        }
#endif

        /// <MetaDataID>{2848ceb3-ecf9-4d96-b804-470d4171998a}</MetaDataID>
        System.Collections.Generic.Dictionary<Type, object> TransaprentProxies = new System.Collections.Generic.Dictionary<Type, object>();

        public event ProxyRecconectedHandle Reconnected;




        /// <MetaDataID>{e8224819-2cd1-4055-8daa-a16cbfede6d5}</MetaDataID>
        public void RaiseReconnectEvent()
        {

            if (Reconnected != null)
            {


            }
            Reconnected?.Invoke(GetTransparentProxy());

        }


        /// <MetaDataID>{003d55d7-d4e4-4a4c-95bd-292272c6bb5c}</MetaDataID>
        internal object GetTransparentProxy(Type type)
        {

#if DeviceDotNet
            //OOAdvantech.Remoting.Proxies.Pr_IServerSessionPart
            //OOAdvantech.Remoting.Poxies.Pr_IServerSessionPart
            object transparentProxy = null;

            if (TransaprentProxies.TryGetValue(type, out transparentProxy))
                return transparentProxy;

            if (typeof(OOAdvantech.Json.Serialization.Proxy) == type ||
                typeof(object) == type ||
                typeof(ITransparentProxy) == type)//(typeof(OOAdvantech.Remoting.RestApi.Proxy) == type)
            {
                transparentProxy = new TransparentProxy(this);
                TransaprentProxies[type] = transparentProxy;

                return transparentProxy;
            }
            else
            {

#if DeviceDotNet
                if (type ==typeof(OOAdvantech.Remoting.MarshalByRefObject))
                    return new OOAdvantech.Remoting.MarshalByRefObject(this);
#endif

                Type transparentProxyType = type.GetMetaData().Assembly.GetType(type.Namespace + ".Proxies.Pr_" + type.Name);
                if (transparentProxyType == null)
                    throw new NotSupportedException("Missing client side proxy type for  " + type.FullName);
                var ctor = transparentProxyType.GetMetaData().GetConstructor(BindingFlags.Public | BindingFlags.Instance, new Type[] { typeof(Proxy) });
                transparentProxy = ctor.Invoke(new object[] { this });
                TransaprentProxies[type] = transparentProxy;

                return transparentProxy;
            }





#else
            return GetTransparentProxy();
#endif

        }

        /// <MetaDataID>{1cbec8cc-9816-4c7e-942e-944a59c0edd0}</MetaDataID>
        public bool CanCastTo(Type fromType, object o)
        {
            if (typeof(ITransparentProxy) == fromType)
                return true;

            bool canCastTo = ProxyType.CanCastTo(fromType);

            return canCastTo;
        }

        /// <MetaDataID>{a8d708b5-4f7d-473a-ae31-b7665c0fa3bf}</MetaDataID>
        public IProxy GetProxy()
        {
            return this;
        }

        public void InvalidateCachedData()
        {

            ObjectRef.InvalidMembersValues = true;
        }

        //IMessage IProxy.Invoke(IMessage msg)
        //{
        //    throw new NotImplementedException();
        //}

        //void IProxy.PublishEvent(EventInfo eventInfo, List<object> args)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public delegate void ProxyRecconectedHandle(object sender);

    /// <MetaDataID>{476fb7cd-4f96-431e-9247-c9d292a9d2d4}</MetaDataID>
    public interface ITransparentProxy
    {
        /// <MetaDataID>{869a67ea-bf5b-416b-810c-d1b8f5360cf7}</MetaDataID>
        IProxy GetProxy();

        event ProxyRecconectedHandle Reconnected;
    }


    /// <MetaDataID>{e67bcfdc-ddd5-4d65-8e84-ccf90b354f4d}</MetaDataID>
    public class TransparentProxy : ITransparentProxy
    {
        /// <MetaDataID>{2bc641e5-fc86-4e9d-8e2e-d630e52c5827}</MetaDataID>
        public IProxy GetProxy()
        {
            return Proxy;
        }

        /// <MetaDataID>{fc132962-481b-42c9-9429-332212dfb22b}</MetaDataID>
        private OOAdvantech.Remoting.RestApi.Proxy Proxy;

        public event ProxyRecconectedHandle Reconnected;

        /// <MetaDataID>{d6187cfb-128a-4aeb-89ee-d32978f6a7c6}</MetaDataID>
        public void RaiseReConnectEvent()
        {
            Reconnected?.Invoke(this);
        }

        /// <MetaDataID>{3355515c-4d4d-4e5b-a273-513b59531820}</MetaDataID>
        public TransparentProxy(OOAdvantech.Remoting.RestApi.Proxy proxy)
        {
            this.Proxy = proxy;
        }
    }

#if DeviceDotNet

    //class GenericDecorator :System.Reflection.DispatchProxy
    //{
    //    public object Wrapped { get; set; }
    //    public Action<MethodInfo, object[]> Start { get; set; }
    //    public Action<MethodInfo, object[], object> End { get; set; }
    //    protected override object Invoke(MethodInfo targetMethod, object[] args)
    //    {
    //        Start?.Invoke(targetMethod, args);
    //        object result = targetMethod.Invoke(Wrapped, args);
    //        End?.Invoke(targetMethod, args, result);
    //        return result;
    //    }
    //}
#endif

    /// <MetaDataID>{b955d0ad-6a9e-4997-9fb2-d6cb1d615070}</MetaDataID>
    public class ServerException : System.Exception
    {
        public readonly string ErrorCode;
        public ServerException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }



        /// <MetaDataID>{e9c8b835-f235-4465-91d5-179d858725af}</MetaDataID>
        public ServerException(string message, int hResult, string errorCode) : base(message)
        {
            this.HResult = hResult;
            ErrorCode = errorCode;
        }

    }

}