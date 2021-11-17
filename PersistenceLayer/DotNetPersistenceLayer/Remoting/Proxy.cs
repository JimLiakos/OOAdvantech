namespace OOAdvantech.Remoting
{
    using System;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Messaging;
    using System.Linq;
    using System.Runtime.Remoting.Contexts;
    using System.Collections;
    using System.Reflection;
    using Json;

    /// <MetaDataID>{30FE77F8-D725-4C93-9912-2301D9D22462}</MetaDataID>
    /// <summary>Proxy class is an extension of real proxy. It has the advantage of reconnect with remote object. Client and server machine maybe loose the connection if lease time expired then the server object disconnected from network. If the client and server machine regain the connection the proxy class try to reconnect with remote object, and reconnect with the object if isn't collected from GC. If remote object is persistent then the proxy will reconnect and in case where the object collected. The persistency system will build the object from the storage instance. </summary>
    public class Proxy : System.Runtime.Remoting.Proxies.RealProxy, IRemotingTypeInfo, IProxy
    {



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DAFD58D6-A80F-4376-930D-AAD101AE5627}</MetaDataID>
        internal MarshalByRefObject _RealTransparentProxy;
        /// <MetaDataID>{32CAEC80-7416-439D-9C2D-1FCD7FB5ED1E}</MetaDataID>
        public MarshalByRefObject RealTransparentProxy
        {
            get
            {
                //System.Threading.Monitor.Enter(this);
                //try
                //{

                //    System.TimeSpan TimeDistanceFromLastRenew = System.DateTime.Now - LastRenewTime;
                //    if (TimeDistanceFromLastRenew.TotalMilliseconds > 6000)
                //        _RealTransparentProxy = null;
                //    if (_RealTransparentProxy == null)
                //        Reconnect();
                //}
                //finally
                //{
                //    System.Threading.Monitor.Exit(this);
                //}

                return _RealTransparentProxy;
            }
            set
            {
                System.Threading.Monitor.Enter(this);
                try
                {
                    _RealTransparentProxy = value;
                }
                finally
                {
                    System.Threading.Monitor.Exit(this);
                }

            }
        }

        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{EFCAFC7A-7047-4C4F-A7D2-1F27EC334719}</MetaDataID>
        //private System.DateTime _LastRenewTime = System.DateTime.Now;
        ///// <MetaDataID>{C9F24954-03FF-46DE-8C53-EE198D4EE8BC}</MetaDataID>
        ///// <summary>The time of last renew. Update every time we call the object and when the renewal manager renew successfully the lease of remote object. If the system tries to marshal the proxy and the time from last renew expired the proxy ensure the connection throw Reconnect method. </summary>
        //internal protected System.DateTime LastRenewTime
        //{
        //    get
        //    {
        //        return _LastRenewTime;
        //    }
        //    set
        //    {
        //        System.TimeSpan TimeDistanceFromLastRenew = System.DateTime.Now - LastRenewTime;
        //        if (TimeDistanceFromLastRenew.TotalMilliseconds <= 6000)
        //            _LastRenewTime = value;
        //    }

        //}


        /// <summary>Creates an System.Runtime.Remoting.ObjRef for the specified object type, and registers it with the remoting infrastructure as a client-activated object. </summary>
        /// <returns>A new instance of System.Runtime.Remoting.ObjRef created for the specified type. </returns>
        /// <param name="requestedType">The object type that an System.Runtime.Remoting.ObjRef is created for. </param>
        /// <MetaDataID>{A4EF91B9-BE02-42F9-817B-7AD713E50E3B}</MetaDataID>
        public override ObjRef CreateObjRef(Type requestedType)
        {
            //Call propagation to the real proxy of remote object.
            return RealProxy.CreateObjRef(requestedType);
        }

        /// <summary>Requests an unmanaged reference to the object represented by the current proxy instance. </summary>
        /// <returns>A pointer to a if the object reference is requested for communication with unmanaged objects in the current process through COM, or a pointer to a cached or newly generated IUnknown COM interface if the object reference is requested for marshaling to a remote location. </returns>
        /// <param name="fIsMarshalled">true if the object reference is requested for marshaling to a remote location; false if the object reference is requested for communication with unmanaged objects in the current process through COM. </param>
        /// <MetaDataID>{CDF4BBFE-278D-4607-A6BD-E1427B2F2172}</MetaDataID>
        public override System.IntPtr GetCOMIUnknown(bool fIsMarshalled)
        {
            //Call propagation to the real proxy of remote object.
            return RealProxy.GetCOMIUnknown(fIsMarshalled);
        }

        /// <summary>Stores an unmanaged proxy of the object represented by the current instance. </summary>
        /// <param name="i">A pointer to the IUnknown interface for the object represented by the current proxy instance. </param>
        /// <MetaDataID>{09687FDA-9452-4BE0-ADB9-787F10AC9E42}</MetaDataID>
        public override void SetCOMIUnknown(System.IntPtr i)
        {
            //Call propagation to the real proxy of remote object.
            RealProxy.SetCOMIUnknown(i);
        }

        /// <summary>Requests a COM interface with the specified ID. </summary>
        /// <returns>A pointer to the requested interface. </returns>
        /// <param name="iid">A reference to the requested interface. </param>
        /// <MetaDataID>{C179A004-09A3-43B6-B4EC-A8D62D7712B4}</MetaDataID>
        public override System.IntPtr SupportsInterface(ref Guid iid)
        {
            //Call propagation to the real proxy of remote object.
            return RealProxy.SupportsInterface(ref iid);
        }






        /// <summary>
        /// Adds the transparent proxy of the object represented by the current instance of 
        /// System.Runtime.Remoting.Proxies.RealProxy 
        /// to the specified System.Runtime.Serialization.SerializationInfo . </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo into which the transparent proxy is serialized. </param>
        /// <param name="context">The source and destination of the serialization. </param>
        /// <MetaDataID>{BEE09188-5E66-417F-A0B2-1025EC2D3A00}</MetaDataID>

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            try
            {
                //Call propagation to the real proxy of remote object.
                System.Runtime.Remoting.RemotingServices.GetRealProxy(RealTransparentProxy).GetObjectData(info, context);
            }
            catch (Remoting.RemotingException Error)
            {
                throw new RemotingException(RemotingException.ExceptionCause.ParameterLostConnection);
            }
            int hh = 0;
        }


        /// <MetaDataID>{3da97cd6-18c2-42d5-a7e3-4045821945a0}</MetaDataID>
        static void ControlLifeTimeMember(object obj, System.Type type, ref bool someObjectsControlled)
        {
            if (obj == null)
                return;
            foreach (System.Reflection.FieldInfo fieldInfo in type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                object value = fieldInfo.GetValue(obj);
                if (value == null)
                    continue;
                value = ControlLifeTime(value, ref someObjectsControlled);
                if (someObjectsControlled)
                    fieldInfo.SetValue(obj, value);

            }

        }

        /// <MetaDataID>{411de32c-b4af-4136-93a2-16fbfbbadeb4}</MetaDataID>
        internal static IRemotingServices RemotingServices = new RemotingServices();
        /// <MetaDataID>{d93bfa38-0dcd-4df7-b788-56b791cfdb8b}</MetaDataID>
        static public object ControlLifeTime(object obj)
        {

            bool someObjectsControlled = false;
            return ControlLifeTime(obj, ref someObjectsControlled);
        }
        /// <MetaDataID>{ccd0c9c6-2546-4690-83dc-77e4c58cd67c}</MetaDataID>
        internal static object ControlLifeTime(object obj, IMethodCallMessage methodCallMessage)
        {
            ClientSessionPart clientSessionPart = null;
            try
            {

                if (methodCallMessage is IMethodCallMessage && obj is IExtMarshalByRefObject)
                {
                    if (methodCallMessage.MethodBase.DeclaringType == typeof(RemotingServices) && methodCallMessage.MethodName == "CreateInstance")
                    {
                        string uri = methodCallMessage.Properties["__Uri"] as string;
                        if (uri != null && uri.IndexOf("/OOAdvantechRemotingServer/RemotingServices") != -1)
                        {
                            string channelUri = uri.Replace("/OOAdvantechRemotingServer/RemotingServices", "");
                            clientSessionPart = RenewalManager.GetSession(channelUri, true, true, RemotingServices);
                        }
                    }
                }
                bool someObjectsControlled = false;
                return ControlLifeTime(obj, ref someObjectsControlled);
            }
            finally
            {
                if (clientSessionPart != null)
                    clientSessionPart.SuspendSessionRemove = false;

            }
        }
        /// <MetaDataID>{0df1a617-608c-47ca-91c5-9f83e97af630}</MetaDataID>
        static object ControlLifeTime(object obj, ref bool someObjectsControlled)
        {
            if ((obj as IServerSessionPart) != null)
                return obj;

            if (obj is IExtMarshalByRefObject)
            {
                System.Runtime.Remoting.Proxies.RealProxy realProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(obj);
                if (realProxy != null)
                {
                    someObjectsControlled = true;
                    return Remoting.Proxy.GetObject(obj as IExtMarshalByRefObject);
                }
                else
                    return obj;
            }
            else if (obj is System.Delegate && (obj as System.Delegate).Target is IExtMarshalByRefObject)
            {
                System.Runtime.Remoting.Proxies.RealProxy realProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy((obj as System.Delegate).Target);
                if (realProxy != null)
                {
                    someObjectsControlled = true;
                    object target = Remoting.Proxy.GetObject((obj as System.Delegate).Target as IExtMarshalByRefObject);
                    return System.Delegate.CreateDelegate(obj.GetType(), target, (obj as System.Delegate).Method);
                }
                else
                    return obj;


            }
            else
            {
                if (obj != null)
                {
                    System.Type type = obj.GetType();
                    if (!type.IsPrimitive && type.IsValueType)
                        ControlLifeTimeMember(obj, type, ref someObjectsControlled);
                }
                return obj;
            }
        }


        /// <MetaDataID>{05533DED-DF35-4959-B24C-A63943141DE1}</MetaDataID>
        public static object GetObject(string channelUri, string objectUri)
        {

            Proxy proxy = RenewalManager.GetProxy(channelUri, objectUri) as Proxy;
            if (proxy != null)
                return proxy.GetTransparentProxy();
            else
                return null;

        }
        /// <MetaDataID>{25FE344B-B25C-4C9B-8082-9583343DDE98}</MetaDataID>
        public static object GetObject(IExtMarshalByRefObject theRealObject)
        {
            lock (RenewalManager.Sessions)
            {
                System.Runtime.Remoting.Channels.IChannel[] Channels = System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels;
                ObjRef myObjRef = System.Runtime.Remoting.RemotingServices.Marshal(theRealObject as MarshalByRefObject);

                string objectChannelUri = Remoting.RemotingServices.GetChannelUri(theRealObject as MarshalByRefObject);
                if (Remoting.RemotingServices.GetServerChannelUris().Contains(objectChannelUri))
                    throw new System.Exception("Object collected from GC A"); // Error prone

                Proxy proxy = RenewalManager.GetProxy(objectChannelUri, myObjRef.URI) as Proxy;
                if (proxy != null)
                    return proxy.GetTransparentProxy();
                proxy = new Proxy(theRealObject);
                RenewalManager.AddProxy(proxy, Proxy.RemotingServices);
                proxy = RenewalManager.GetProxy(objectChannelUri, myObjRef.URI) as Proxy;
                return proxy.GetTransparentProxy();
            }
        }


        /// <MetaDataID>{49EF05E3-1E29-466D-BD56-860494A6715A}</MetaDataID>
        private void Reconnect()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Recconect");

                IExtMarshalByRefObject marshalByRefObject = null;



                try
                {

                    if (!string.IsNullOrEmpty(URI.MonoStateClassChannelUri))
                        marshalByRefObject = Remoting.RemotingServices.ReconnectWithObject(URI.MonoStateClassChannelUri, URI) as IExtMarshalByRefObject;
                    else
                        marshalByRefObject = Remoting.RemotingServices.ReconnectWithObject(ChannelUri, URI) as IExtMarshalByRefObject;

                }
                catch (System.Exception error)
                {
                }




                if (marshalByRefObject == null)
                    throw new Remoting.RemotingException(Remoting.RemotingException.ExceptionCause.CannotReconnect, "Object with Uri " + URI + " Disconnected ");

                string channelUri = Remoting.RemotingServices.GetChannelUri(marshalByRefObject as MarshalByRefObject);
                string objectUri = Remoting.RemotingServices.GetObjectUri(marshalByRefObject as MarshalByRefObject);

                if (channelUri != ChannelUri || URI.TransientUri != objectUri)
                {
                    RenewalManager.RemoveProxy(this);
                    AttachToObject(marshalByRefObject as MarshalByRefObject);
                    RenewalManager.AddProxy(this, RemotingServices);
                }
                else
                    AttachToObject(marshalByRefObject as MarshalByRefObject);


                Proxy proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(marshalByRefObject) as Proxy;

                if (_RealTransparentProxy == null)
                    throw new Remoting.RemotingException(Remoting.RemotingException.ExceptionCause.CannotReconnect, "Object with Uri " + URI + " Disconnected ");
                // _LastRenewTime = System.DateTime.Now;
                RenewalManager.AddProxy(this, RemotingServices);


            }
            catch (System.Exception Error)
            {
                throw new Remoting.RemotingException(Remoting.RemotingException.ExceptionCause.CannotReconnect, "Object with Uri " + URI + " Disconnected ");
            }
        }


        /// <MetaDataID>{13CBD072-D3D0-48CD-8F2B-01D56D3F1657}</MetaDataID>
        /// <summary>URI of the remote object. </summary>
        internal protected ExtObjectUri URI;




        /// <exclude>Excluded</exclude>
        string _TypeFullName;
        //string MonoStateClassChannelUri;

        /// <MetaDataID>{D4377CD3-DE58-4E6C-BDAA-2A45A7C3F0F2}</MetaDataID>
        /// <summary>Gets only the fully-qualified type name of the server object in an System.Runtime.Remoting.ObjRef . </summary>
        public string TypeName
        {
            get
            {
                if (string.IsNullOrEmpty(_TypeFullName))
                    return GetProxiedType().ToString();
                else
                    return _TypeFullName;

            }

            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>Checks whether the proxy representing the specified object type can be cast to the type represented by the System.Runtime.Remoting.IRemotingTypeInfo interface. </summary>
        /// <returns>true if cast will succeed; otherwise, false. </returns>
        /// <param name="castType">The type to cast to. </param>
        /// <param name="o">The object for which to check casting. </param>
        /// <MetaDataID>{CBB99B58-03E7-4CE0-A97C-0E69E380D957}</MetaDataID>
        public bool CanCastTo(Type castType, Object o)
        {
            if (castType == typeof(Proxy))
                return true;
            System.Threading.Monitor.Enter(this);
            bool CanCast = false;
            try
            {
                CanCast = castType.IsInstanceOfType(_RealTransparentProxy);
            }
            finally
            {
                System.Threading.Monitor.Exit(this);
            }
            return CanCast;

        }



        /// <MetaDataID>{EEBF805E-46FE-4EC1-9EB5-DB8E00EBD620}</MetaDataID>
        public Proxy(IExtMarshalByRefObject theRealObject)
            : base(GetType(theRealObject))
        {

            EventConsumingResolver = new EventConsumingResolver(this, RemotingServices);
            //  theRealObject = Remoting.RemotingServices.ModifyChannelData(theRealObject as MarshalByRefObject);

            if (System.Runtime.Remoting.RemotingServices.IsTransparentProxy(theRealObject))
                typeof(System.Runtime.Remoting.Proxies.RealProxy).GetProperty("IdentityObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, typeof(System.Runtime.Remoting.Proxies.RealProxy).GetProperty("IdentityObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(System.Runtime.Remoting.RemotingServices.GetRealProxy(theRealObject), null), null);
            else if (theRealObject is MarshalByRefObject)
                typeof(System.Runtime.Remoting.Proxies.RealProxy).GetProperty("IdentityObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, typeof(MarshalByRefObject).GetProperty("Identity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(theRealObject, null), null);


            theRealObject = ModifyChannelData(theRealObject);

            ObjectType = theRealObject.GetType();
            AttachToObject(theRealObject as MarshalByRefObject);
        }

        /// <MetaDataID>{fd0a52ae-1a13-49b2-b9a1-fe9168440e9d}</MetaDataID>
        private static Type GetType(IExtMarshalByRefObject theRealObject)
        {
            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(theRealObject as MarshalByRefObject))
                return typeof(MarshalByRefObject);// System.Runtime.Remoting.RemotingServices.GetRealProxy(theRealObject).GetProxiedType();
            else
                return theRealObject.GetType();

        }

        /// <MetaDataID>{3edd7eb4-e231-456c-8fcc-ddad507c20b6}</MetaDataID>
        private static IExtMarshalByRefObject ModifyChannelData(IExtMarshalByRefObject theRealObject)
        {
            return theRealObject;
            if (!Remoting.RemotingServices.IsInRemoteMachine(theRealObject as MarshalByRefObject))
            {
                System.Runtime.Remoting.ObjRef remoteObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(theRealObject as MarshalByRefObject);
                if (remoteObjRef != null)
                {
                    Object[] data = remoteObjRef.ChannelInfo.ChannelData;
                    int firstChannelIndex = -1;

                    System.Runtime.Remoting.Channels.ChannelDataStore tcpChannel = null;
                    System.Runtime.Remoting.Channels.ChannelDataStore ipcChannel = null;
                    for (int j = 0; j < data.Length; j++)
                    {
                        if (data[j] is System.Runtime.Remoting.Channels.ChannelDataStore)
                        {
                            if (firstChannelIndex == -1)
                                firstChannelIndex = j;
                            System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore = data[j] as System.Runtime.Remoting.Channels.ChannelDataStore;
                            if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"tcp:/") == 0)
                                tcpChannel = channelDataStore;
                            if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"ipc") == 0)
                                ipcChannel = channelDataStore;
                        }
                    }
                    if (data[firstChannelIndex] != ipcChannel)
                    {
                        if (ipcChannel != null)
                            data[firstChannelIndex] = ipcChannel;
                        if (tcpChannel != null)
                            data[firstChannelIndex + 1] = tcpChannel;

                        #region Rebuild channel sink
                        //Αυτός ο τρόpος που γίνεται rebuild η channel sink δεν είναι ασφαλές γιατί χρησιμοποιεί μη
                        //ντοκιουμενταρισμένες operations και class του .net framework

                        //Class:        System.Runtime.Remoting.IdentityHolder
                        //operation:    static void RemoveIdentity(string uri, bool bResetURI)

                        System.Type type = typeof(System.Runtime.Remoting.RemotingServices).Assembly.GetType("System.Runtime.Remoting.IdentityHolder");
                        foreach (System.Reflection.MethodInfo method in type.GetMethods(System.Reflection.BindingFlags.Public |
                                                        System.Reflection.BindingFlags.NonPublic |
                                                        System.Reflection.BindingFlags.Static |
                                                        System.Reflection.BindingFlags.DeclaredOnly))
                        {
                            if (method.Name == "RemoveIdentity" && method.GetParameters().Length == 2)
                            {
                                method.Invoke(null, new object[2] { remoteObjRef.URI, false });
                                break;
                            }
                        }
                        theRealObject = System.Runtime.Remoting.RemotingServices.Unmarshal(remoteObjRef) as IExtMarshalByRefObject;
                        #endregion
                    }

                }

            }
            return theRealObject;
        }
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <MetaDataID>{BF3E80E2-BB4D-4077-8A2F-5A2877F69D3E}</MetaDataID>
        ~Proxy()
        {
            RenewalManager.RemoveProxy(this);
        }

        /// <MetaDataID>{6FD12753-BC27-4763-93BC-63B2B0C92083}</MetaDataID>
        private System.Runtime.Remoting.Messaging.IMessageSink MessageSink;
        /// <MetaDataID>{D848A3E5-0E38-4FCB-BCA9-7E477D429E48}</MetaDataID>
        /// <summary>Through channel uri the proxy locate the server process and try to reconnect with remote object. </summary>
        public string ChannelUri { get; set; }
        /// <MetaDataID>{A1019B10-022A-4C46-9D4E-5F63EEA715EA}</MetaDataID>
        private Type ObjectType = null;
        /// <MetaDataID>{E06C8236-E852-43C2-8B6E-C8CA2F378F5D}</MetaDataID>
        /// <summary>Realproxy defines the real proxy of remote object which attached to this proxy.
        /// Realproxy must be not null after object constriction. </summary>
        private System.Runtime.Remoting.Proxies.RealProxy RealProxy = null;

        /// <summary>Through this method the proxy connected with remote object. After this time all calls propagated to the attached remote object. </summary>
        /// <param name="theRealObject">Parameter defines the remote object which will be attached the proxy. </param>
        /// <MetaDataID>{02D6427A-8CFD-4695-86A3-5FC6270F8DA5}</MetaDataID>
        private void AttachToObject(MarshalByRefObject theRealObject)
        {
            System.Threading.Monitor.Enter(this);
            try
            {
                if (!Remoting.RemotingServices.IsOutOfProcess(theRealObject as MarshalByRefObject))
                    throw new System.ArgumentException("The object isn't remote object");
                _RealTransparentProxy = theRealObject;
                ObjectType = theRealObject.GetType();
                MessageSink = System.Runtime.Remoting.RemotingServices.GetEnvoyChainForProxy(_RealTransparentProxy as MarshalByRefObject);
                // Get 'ObjRef', for transmission serialization between application domains.
                ObjRef myObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(theRealObject as MarshalByRefObject);


                #region Reorder channels of myObjRef at the client side



                Remoting.RemotingServices.ReorderClientSideObjRefChannelData(myObjRef);


                #endregion


                ChannelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(theRealObject as MarshalByRefObject);

                RealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(_RealTransparentProxy);
                var clientProcessIdentity = Remoting.RemotingServices.ProcessIdentity;
                if (ObjectType != typeof(ServerSessionPart))
                    clientProcessIdentity = RenewalManager.GetSession(ChannelUri, true, RemotingServices).ClientProcessIdentity;

                URI = ExtObjectUri.Parse(myObjRef.URI, clientProcessIdentity);

            }
            catch (System.Exception Error)
            {
                int hh = 0;
            }
            finally
            {
                System.Threading.Monitor.Exit(this);
            }
        }
        /// <MetaDataID>{7D9FA94A-1EDA-41F2-90B1-2E34A8C029CA}</MetaDataID>
        public System.Runtime.Remoting.Messaging.IMessage TryInvokeLocal(System.Runtime.Remoting.Messaging.IMessage msg, out bool localCall)
        {
            //string HostName=System.Net.Dns.GetHostName();
            IMethodReturnMessage ReturnMessage = null;
            System.Reflection.MethodBase methodBase = (msg as IMethodCallMessage).MethodBase;
            localCall = false;
            object[] outArgs = null;
            object returnValue = null;

            if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                methodBase.Name == "GetType")
            {
                outArgs = new object[0];
                returnValue = ObjectType;
                localCall = true;
            }
            else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                methodBase.Name == "GetHashCode")
            {
                outArgs = new object[0];
                if (string.IsNullOrEmpty(URI.PersistentUri))
                {

                    if (string.IsNullOrEmpty(URI.TransientUri))
                        returnValue = _RealTransparentProxy.GetHashCode();
                    else
                        returnValue = URI.TransientUri.GetHashCode();
                }
                else
                    returnValue = URI.PersistentUri.GetHashCode();


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
                    string objUri = Remoting.RemotingServices.GetObjectUri(obj as MarshalByRefObject);

                    if (objUri == URI.TransientUri && URI != null)
                        IsEquals = true;
                    else
                        IsEquals = false;
                }

                outArgs = new object[0];
                returnValue = IsEquals;
                localCall = true;
            }
            else if (methodBase.Name.IndexOf("add_") == 0)
            {
                System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetEvent(methodBase.Name.Substring("Add_".Length));
                if (eventInfo != null && eventInfo.GetAddMethod() == methodBase)
                {
                    EventConsumingResolver.EventConsumerSubscription((msg as IMethodCallMessage).Args[0] as System.Delegate, eventInfo);
                    outArgs = new object[0];
                    localCall = true;
                }
            }
            else if (methodBase.Name.IndexOf("remove_") == 0)
            {
                System.Reflection.EventInfo eventInfo = methodBase.DeclaringType.GetEvent(methodBase.Name.Substring("remove_".Length));
                if (eventInfo != null && eventInfo.GetRemoveMethod() == methodBase)
                {
                    EventConsumingResolver.EventConsumerUnsubscribe((msg as IMethodCallMessage).Args[0] as System.Delegate, eventInfo);

                    outArgs = new object[0];
                    localCall = true;
                }
            }

            if (localCall)
            {
                ReturnMessage = new System.Runtime.Remoting.Messaging.ReturnMessage(
                    returnValue,	//ReturnValue
                    outArgs,			//Object[] outArgs
                    outArgs.Length,					//int outArgsCount
                    (LogicalCallContext)msg.Properties["__CallContext"],				//LogicalCallContext callCtx
                    (System.Runtime.Remoting.Messaging.IMethodCallMessage)msg);
                return ReturnMessage;

            }




            return null;
        }
        /// <exclude>Excluded</exclude>
        ClientSessionPart _ClientSessionPart;
        /// <MetaDataID>{d26a7327-6c4f-40d3-b234-1f37151b01ef}</MetaDataID>
        ClientSessionPart ClientSessionPart
        {
            get
            {
                if (_ClientSessionPart == null)
                    _ClientSessionPart = RenewalManager.GetSession(ChannelUri, RemotingServices);
                return _ClientSessionPart;
            }
        }


        /// <MetaDataID>{f35ed44c-2898-4359-8b23-87115f13cd4c}</MetaDataID>
        public ExtObjectUri ObjectUri
        {
            get
            {
                return URI;
            }
        }

        /// <MetaDataID>{53955d65-1863-4664-bf29-c83c50382a82}</MetaDataID>
        public EventConsumingResolver EventConsumingResolver { get; set; }

        string IProxy.Uri => URI?.TransientUri;

        //EventConsumingResolver IProxy.EventConsumingResolver { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //string IProxy.ChannelUri { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //ExtObjectUri IProxy.ObjectUri => URI;


        /// <MetaDataID>{95401A0A-911E-42BD-B2D6-007AB6056D33}</MetaDataID>
        /// <summary>Invokes the method specified in the provided System.Runtime.Remoting.Messaging.IMessage on the remote object represented by the current instance. </summary>
        /// <param name="msg">An System.Runtime.Remoting.Messaging.IMessage containing an System.Collections.IDictionary of information about the method call. </param>
        /// <returns>The message returned by the invoked method, containing the return value and any out or ref parameters. </returns>
        public override System.Runtime.Remoting.Messaging.IMessage Invoke(System.Runtime.Remoting.Messaging.IMessage msg)
        {
            // Error Prone τι error θα επιστραφή στην περίπτωση που το myMarshalByRefObject είναι null
            //System.Runtime.Remoting.Proxies.RealProxy RealObjectProxy= System.Runtime.Remoting.RemotingServices.GetRealProxy(RealTransparentProxy);
            try
            {
                System.Collections.IDictionary myDictionary = msg.Properties;
                myDictionary["__Uri"] = URI.TransientUri;
                IMethodReturnMessage ReturnMessage = null;

                LogicalCallContext lcc = null;
                PendingEvent data = null;

                try
                {
                    IMessage mymessage = null;
                    ICloneable erww = msg as ICloneable;
                    if (msg is IMethodCallMessage)
                    {


                        mymessage = new MethodCallMessageWrapper(msg as IMethodCallMessage);
                        bool LocalCall;
                        ReturnMessage = TryInvokeLocal(msg, out LocalCall) as IMethodReturnMessage; ;
                        if (LocalCall)
                            return ReturnMessage;
                    }
                    System.Exception Exception = null;



                    try
                    {
                        //if (_RealTransparentProxy != null)
                        //{
                        //    object[] args = (msg as IMethodCallMessage).Args.Clone() as object[];
                        //    object retval = (msg as IMethodCallMessage).MethodBase.Invoke(_RealTransparentProxy, args);



                        //    ReturnMessage = new System.Runtime.Remoting.Messaging.ReturnMessage(retval, args, args.Length, (msg as IMethodCallMessage).LogicalCallContext, (msg as IMethodCallMessage));
                        //}
                        //else
                        if (Remoting.RemotingServices.ImpersonateToInitSessionUser && ClientSessionPart != null
                            && System.Security.Principal.WindowsIdentity.GetCurrent() != ClientSessionPart.WindowsIdentity)
                        {
                            //Impersonate ot the session initiator user  
                            using (ClientSessionPart.WindowsIdentity.Impersonate())
                            {

                                if (RealProxy != null)
                                {
                                    if (msg is IMethodCallMessage)
                                    {

                                        try
                                        {
                                            object[] args = (msg as IMethodCallMessage).Args.Clone() as object[];
                                            var ret = (msg as IMethodCallMessage).MethodBase.Invoke(RealProxy.GetTransparentProxy(), args);
                                            ReturnMessage = new ReturnMessage(
                                                               ret, //Object ret
                                                               args,            //Object[] outArgs 
                                                               args.Length,     //int outArgsCount
                                                               (msg as IMethodCallMessage).LogicalCallContext,          //LogicalCallContext callCtx
                                                               (IMethodCallMessage)msg   //IMethodCallMessage mcm
                                                               );

                                        }
                                        catch (Exception error)
                                        {

                                            ReturnMessage = new ReturnMessage(error, (IMethodCallMessage)msg);
                                        }
                                    }
                                    else
                                        ReturnMessage = RealProxy.Invoke(msg) as IMethodReturnMessage;
                                }
                                else
                                    ReturnMessage = MessageSink.SyncProcessMessage(msg) as IMethodReturnMessage;
                            }
                        }
                        else
                        {
                            if (RealProxy != null)
                                ReturnMessage = RealProxy.Invoke(msg) as IMethodReturnMessage;
                            else
                                ReturnMessage = MessageSink.SyncProcessMessage(msg) as IMethodReturnMessage;

                        }

                        Exception = ReturnMessage.Exception;

                    }
                    catch (System.Exception Error)
                    {
                        Exception = Error;
                    }



                    if (ReturnMessage != null && ReturnMessage.ReturnValue is AsyncResult)
                    {

                        string Error = "Asynchronous call error." + "\nHappen when call asynchronously an object which type is subtype of '" + typeof(IExtMarshalByRefObject).FullName +
                            "'\nInstead of  'DelegateType MethodCall=new DelegateType(Object.Method)'\n" +
                            "you can use 'DelegateType MethodCall=new DelegateType((OOAdvantech.Remoting.RemotingServices.GetOrgObject(Object) as ObjectType).Method)'";
                        throw new System.Exception(Error);
                    }
                    if (Exception != null)
                    {
                        string DeclaringType = " ";
                        if (Exception.TargetSite != null)
                            DeclaringType = Exception.TargetSite.DeclaringType.FullName;
                        if (DeclaringType.IndexOf("System.Runtime.Remoting") != -1 || DeclaringType.IndexOf("OOAdvantech.Remoting") != -1 || Exception is System.Runtime.Remoting.RemotingException || Exception is System.Runtime.Remoting.RemotingException)
                        {
                            try
                            {
                                System.Runtime.Remoting.Lifetime.ILease lease = _RealTransparentProxy.GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                            }
                            catch (System.Exception)
                            {
                                try
                                {
                                    Reconnect();
                                    //msg.Properties["__Uri"] = null;
                                    msg.Properties["__Uri"] = URI.TransientUri;
                                    //object retval=(msg as IMethodCallMessage).MethodBase.Invoke(_RealTransparentProxy, (msg as IMethodCallMessage).Args);
                                    //ReturnMessage = new System.Runtime.Remoting.Messaging.ReturnMessage(retval, (msg as IMethodCallMessage).Args, (msg as IMethodCallMessage).Args.Length, (msg as IMethodCallMessage).LogicalCallContext, (msg as IMethodCallMessage));

                                    //ReturnMessage = System.Runtime.Remoting.RemotingServices.ExecuteMessage(_RealTransparentProxy, msg as IMethodCallMessage);


                                    if (RealProxy != null)
                                        ReturnMessage = RealProxy.Invoke(msg) as IMethodReturnMessage;
                                    //ReturnMessage = RealProxy.Invoke(mymessage) as IMethodReturnMessage;
                                    if (ReturnMessage == null)
                                        ReturnMessage = MessageSink.SyncProcessMessage(msg) as IMethodReturnMessage;
                                    int ttt = 0;

                                }
                                catch (System.Exception Error)
                                {

                                }
                            }


                            if (ReturnMessage == null)
                            {
                                throw new System.Exception(Exception.Message, Exception);
                            }

                            lcc = (LogicalCallContext)ReturnMessage.Properties["__CallContext"];
                            data = lcc.GetData("{5AC92626-E8F3-42ee-A050-483C33C56BF0}") as PendingEvent;
                            if (data != null && data.Pending)
                            {
                                RenewalManager.GetSession(ChannelUri, RemotingServices).PublishSessionEvents();
                            }

                            return ReturnMessage;
                        }
                        else
                        {

                            if (ReturnMessage == null)
                            {
                                throw new System.Exception(Exception.Message, Exception);
                            }
                        }

                    }
                    //else
                    //    LastRenewTime = System.DateTime.Now;

                }

                catch (System.Exception Error)
                {
                    throw Error;
                }
                finally
                {
                }


                lcc = (LogicalCallContext)ReturnMessage.Properties["__CallContext"];
                data = lcc.GetData("{5AC92626-E8F3-42ee-A050-483C33C56BF0}") as OOAdvantech.Remoting.PendingEvent; ;
                if (data != null && data.Pending)
                {
                    RenewalManager.GetSession(ChannelUri, RemotingServices).PublishSessionEvents();

                }



                return ReturnMessage;
            }
            catch (System.Exception error)
            {
                throw;
            }

        }

        IMessage IProxy.Invoke(IMessage msg)
        {
            throw new NotImplementedException();
        }
    }



    #region Event consuming resolver

    /// <MetaDataID>{bcabc6da-2de8-4479-a648-da061ac6b5c2}</MetaDataID>
    internal class EventConsuming
    {
        public bool AllAsynchronousCounsumers = true;
        public System.Collections.Generic.List<Delegate> EventConsumers = new System.Collections.Generic.List<Delegate>();
    }

    /// <MetaDataID>{ff215007-4002-4738-9ebe-1ce531dcda56}</MetaDataID>
    public class EventConsumingResolver
    {
        /// <MetaDataID>{3f6cceea-70a5-41e0-b1dd-6846debab498}</MetaDataID>
        IProxy Proxy;

        /// <MetaDataID>{c008b42c-4f0a-4f91-bbc4-beab36dd1174}</MetaDataID>
        IRemotingServices RemotingServices;
        /// <MetaDataID>{9ba480c0-c825-4899-afde-cf07df28dd8f}</MetaDataID>
        public EventConsumingResolver(IProxy proxy, IRemotingServices remotingServices)
        {
            Proxy = proxy;
            RemotingServices = remotingServices;
        }
        /// <MetaDataID>{96e428c2-cfd9-4a31-84ac-16e3b52804b9}</MetaDataID>
        System.Collections.Generic.Dictionary<System.Reflection.EventInfo, EventConsuming> EventsInvocationLists = null;

        /// <MetaDataID>{def51759-90a7-4b0a-92bc-a3c02395f933}</MetaDataID>
        public void PublishEvent(System.Reflection.EventInfo eventInfo, System.Collections.Generic.List<object> args)
        {
            if (EventsInvocationLists != null && EventsInvocationLists[eventInfo] != null)
            {
                foreach (System.Delegate eventHandler in EventsInvocationLists[eventInfo].EventConsumers)
                {
                    try
                    {
                        eventHandler.Method.Invoke(eventHandler.Target, args.ToArray());
                    }
                    catch (System.Exception error)
                    {
                    }
                }
            }
        }

        /// <MetaDataID>{030454ee-b6b3-449f-a308-fdf3d772f6de}</MetaDataID>
        internal void EventConsumerUnsubscribe(System.Delegate removedEventHandler, System.Reflection.EventInfo eventInfo, out bool sessionUnsubscribe)
        {
            sessionUnsubscribe = false;
            if (EventsInvocationLists != null && EventsInvocationLists.ContainsKey(eventInfo))
            {
                foreach (System.Delegate eventHandler in EventsInvocationLists[eventInfo].EventConsumers)
                {

                    if (eventHandler.Target == removedEventHandler.Target && eventHandler.Method == removedEventHandler.Method)
                    {
                        EventsInvocationLists[eventInfo].EventConsumers.Remove(eventHandler);
                        if (EventsInvocationLists[eventInfo].EventConsumers.Count == 0)
                            sessionUnsubscribe = true;

                        break;

                    }
                }
            }
        }
        /// <MetaDataID>{afdd1bfe-9cb9-4750-b6b7-8e7aee0a72e7}</MetaDataID>
        internal void EventConsumerUnsubscribe(System.Delegate removedEventHandler, EventInfo eventInfo)
        {
            bool sessionUnsubscribe = false;
            EventConsumerUnsubscribe(removedEventHandler, eventInfo, out sessionUnsubscribe);
            if (sessionUnsubscribe)
                RenewalManager.GetSession(Proxy.ChannelUri, RemotingServices).Unsubscribe(Proxy, new EventInfoData(eventInfo));
        }

        /// <MetaDataID>{228c8e61-405d-4204-8cba-25dfbe1756b4}</MetaDataID>
        internal void EventConsumerSubscription(Delegate eventHandler, System.Reflection.EventInfo eventInfo)
        {
            bool proceedToSessionSubscription;
            bool allowAsynchronous;
            EventConsumerSubscription(eventHandler, eventInfo, out proceedToSessionSubscription, out allowAsynchronous);
            if (proceedToSessionSubscription)
                RenewalManager.GetSession(Proxy.ChannelUri, RemotingServices).Subscribe(Proxy, new EventInfoData(eventInfo), allowAsynchronous);
        }
        /// <MetaDataID>{e3a189c2-ec37-4b1e-aff3-a8f3dcbe1b3d}</MetaDataID>
        internal void EventConsumerSubscription(Delegate eventHandler, System.Reflection.EventInfo eventInfo, out bool proceedToSessionSubscription, out bool allowAsynchronous)
        {
            proceedToSessionSubscription = false;
            allowAsynchronous = false;

            if (EventsInvocationLists == null)
                EventsInvocationLists = new System.Collections.Generic.Dictionary<System.Reflection.EventInfo, EventConsuming>();
            if (!EventsInvocationLists.ContainsKey(eventInfo) || EventsInvocationLists[eventInfo].EventConsumers.Count == 0)
            {
                if (!EventsInvocationLists.ContainsKey(eventInfo))
                    EventsInvocationLists[eventInfo] = new EventConsuming();
                proceedToSessionSubscription = true;
                if (Remoting.RemotingServices.HasHostProcessNetworkAccess)
                    allowAsynchronous = false;
                else
                {
                    if (eventHandler.Method.GetCustomAttributes(typeof(MetaDataRepository.AllowEventCallAsynchronousAttribute), true).Length > 0)
                    {
                        if (EventsInvocationLists[eventInfo].AllAsynchronousCounsumers)
                            allowAsynchronous = true;
                        else
                            allowAsynchronous = false;

                    }
                    else
                        allowAsynchronous = false;
                }
            }
            else
            {
                if (eventHandler.Method.GetCustomAttributes(typeof(MetaDataRepository.AllowEventCallAsynchronousAttribute), true).Length == 0)
                    if (EventsInvocationLists[eventInfo].AllAsynchronousCounsumers)
                    {
                        proceedToSessionSubscription = true;
                        EventsInvocationLists[eventInfo].AllAsynchronousCounsumers = false;
                        allowAsynchronous = false;
                    }
            }
            EventsInvocationLists[eventInfo].EventConsumers.Add(eventHandler);
        }
    }

    #endregion

    /// <MetaDataID>{1C2C9703-8B99-4499-812A-0122BE664247}</MetaDataID>
    [Serializable]
    public class ExtObjectUri
    {

     
        /// <MetaDataID>{46f11066-6e7c-4432-95d6-92440ceb411f}</MetaDataID>
        public ExtObjectUri(string transientUri, string persistentUri, string monoStateTypeFullName, string monoStateClassChannelUri, Guid sessionIDentity)
        {
            Disconnected = false;
            TransientUri = transientUri;
            PersistentUri = persistentUri;
            MonoStateTypeFullName = monoStateTypeFullName;
            SessionIDentity = sessionIDentity;
            MonoStateClassChannelUri = monoStateClassChannelUri;
        }

        /// <MetaDataID>{ce05a3dc-88ad-491d-9257-ef9a10e480c1}</MetaDataID>
        [JsonConstructor]
        public ExtObjectUri(string transientUri, string persistentUri, string monoStateTypeFullName, string monoStateClassChannelUri, Guid sessionIDentity, bool disconnected)
        {
            Disconnected = disconnected;
            TransientUri = transientUri;
            PersistentUri = persistentUri;
            MonoStateTypeFullName = monoStateTypeFullName;
            SessionIDentity = sessionIDentity;
            MonoStateClassChannelUri = monoStateClassChannelUri;
        }

        [JsonIgnore]
        public string Uri
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PersistentUri))
                    return TransientUri;
                else
                    return PersistentUri;
            }
        }

        /// <MetaDataID>{eb1039e1-db8e-4c73-b698-0c85c8f3541b}</MetaDataID>
        public static bool operator ==(ExtObjectUri left, ExtObjectUri right)
        {
            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                return true;
            if (object.ReferenceEquals(left, null))
                return false;
            if (object.ReferenceEquals(right, null))
                return false;

            if (left.Uri == right.Uri)
                return true;

            if (!string.IsNullOrEmpty(left.PersistentUri) && left.PersistentUri == right.PersistentUri)
                return true;
            return false;

        }
        /// <MetaDataID>{44134853-0f61-4b60-b0dd-093f06162877}</MetaDataID>
        public static bool operator !=(ExtObjectUri left, ExtObjectUri right)
        {
            return !(left == right);
        }

        /// <MetaDataID>{7268A84B-2884-428A-B3A9-676F2F395605}</MetaDataID>

        public readonly string TransientUri;
        /// <MetaDataID>{0F03E365-ED70-4E09-97DD-7A689131BB96}</MetaDataID>
        /// <summary>PersistentUri has information about the identity and the location of remote object. With the PersistentUri the proxy can regain the connection with remote object and in the case where the object collected from GC of server process. </summary>

        public readonly string PersistentUri;
        /// <MetaDataID>{1956A09C-3935-4451-B43C-A11AA9C29E79}</MetaDataID>
        /// 
        public bool Disconnected;

        /// <MetaDataID>{4cd9600a-fab9-47bd-a4ef-f840b9ba6387}</MetaDataID>
        public readonly string MonoStateTypeFullName;

        /// <MetaDataID>{bf6348e3-0450-4143-8ae3-ba411330c908}</MetaDataID>
        public readonly Guid SessionIDentity;

        /// <MetaDataID>{50560e8f-7b9f-4f5c-87f9-3a29c1c08856}</MetaDataID>
        public readonly string MonoStateClassChannelUri;

        /// <MetaDataID>{abc2b61b-4087-43fd-91bb-a267f8743cda}</MetaDataID>
        internal static ExtObjectUri Parse(string objUri, Guid clientProcessIdentity)
        {
            int nPos = objUri.IndexOf("#MonoStateClass#");
            if (nPos != -1)
            {
                string persistentUri = objUri.Substring(nPos);
                persistentUri = persistentUri.Replace("#MonoStateClass#", "");
                nPos = persistentUri.IndexOf(@"\");
                string monoStateClassChannelUri = persistentUri.Substring(nPos + 1);
                string typeFullName = persistentUri.Substring(0, nPos);
                return new ExtObjectUri(objUri, null, typeFullName, monoStateClassChannelUri, clientProcessIdentity);
            }
            else
            {
                nPos = objUri.IndexOf("#PID#");
                if (nPos != -1)
                {
                    string persistentUri = objUri.Substring(nPos);
                    persistentUri = persistentUri.Replace("#PID#", "");
                    return new ExtObjectUri(objUri, persistentUri, null, null, clientProcessIdentity);
                }
                else
                    return new ExtObjectUri(objUri, null, null, null, clientProcessIdentity);
            }
        }
    }

    /// <MetaDataID>{66bab9e6-6033-44f6-aac3-9e05333ddbf2}</MetaDataID>
    public struct EventSubscrioption
    {
        public System.Reflection.EventInfo EventInfo;
        public object Obj;
        public Delegate Handler;
        public EventSubscrioption(System.Reflection.EventInfo eventInfo, object obj, Delegate handler)
        {
            EventInfo = eventInfo;
            Obj = obj;
            Handler = handler;

        }
    }

    /// <MetaDataID>{2bcecd7a-8c41-4d2e-a478-e2e626c64827}</MetaDataID>
    public class EventInfoData
    {
        /// <MetaDataID>{0a5e50af-0843-46bb-a821-a09328ebad63}</MetaDataID>
        public EventInfoData()
        {
        }

        /// <MetaDataID>{33d9ca74-2106-4c07-bd8c-4c4a78abb7f6}</MetaDataID>
        [JsonIgnore]
        private EventInfo _EventInfo;

        /// <MetaDataID>{1ba4a438-49c0-498b-bbff-7a80e7b061fb}</MetaDataID>
        [JsonIgnore]
        public EventInfo EventInfo
        {
            get
            {
                if (_EventInfo == null && !string.IsNullOrEmpty(DeclaringType))
                    _EventInfo = Type.GetType(DeclaringType).GetEvent(EventName);
                return _EventInfo;
            }
        }

        /// <MetaDataID>{301ecd91-912c-4f36-9e93-01bb4464e05a}</MetaDataID>
        public override int GetHashCode()
        {
            if (EventInfo != null)
                return EventInfo.GetHashCode();
            return base.GetHashCode();
        }
        /// <MetaDataID>{9863c628-1647-4494-98df-1d0e541fc13a}</MetaDataID>
        public static bool operator ==(EventInfoData left, EventInfoData right)
        {

            if (!(left is EventInfoData) && !(right is EventInfoData))
                return true;

            if (!(left is EventInfoData) || !(right is EventInfoData))
                return false;

            //    if (left != null && right == null)
            //    return false;

            //if (left == null && right != null)
            //    return false;

            if (left.DeclaringType == right.DeclaringType && left.EventName == right.EventName)
                return true;
            else
                return false;

        }
        /// <MetaDataID>{66dc4577-7f63-4f65-9ee0-a2c0f3a031fb}</MetaDataID>
        public static bool operator !=(EventInfoData left, EventInfoData right)
        {
            return !(left == right);
        }
        /// <MetaDataID>{ff0f4b40-1e0c-4e7f-8cae-bdb3ebe4051a}</MetaDataID>
        [JsonConstructor]
        public EventInfoData(string declaringType, string eventName)
        {
            DeclaringType = declaringType;
            EventName = eventName;
        }
        /// <MetaDataID>{1ac24b49-2585-4abc-9b43-e8638ffd56a8}</MetaDataID>
        public EventInfoData(System.Reflection.EventInfo eventInfo)
        {
            _EventInfo = eventInfo;
            DeclaringType = _EventInfo.DeclaringType.AssemblyQualifiedName;
            EventName = eventInfo.Name;
        }
        /// <MetaDataID>{c72e1dda-da2c-4e70-89f4-cd3d7274d3ba}</MetaDataID>
        public readonly string DeclaringType;

        /// <MetaDataID>{a08a415e-2a17-4f92-960a-b920dc7acd07}</MetaDataID>
        public readonly string EventName;
    }



    /// <MetaDataID>{63f2e6e8-e8fa-4808-8814-7e8100f81a43}</MetaDataID>
    public interface IProxy
    {
        string Uri { get; }
        /// <MetaDataID>{7de248c7-74a5-4fd0-8d45-1c98e415a8fd}</MetaDataID>
        EventConsumingResolver EventConsumingResolver { get; set; }
        /// <MetaDataID>{d9cd4d5c-1240-4526-868b-ff629e762a40}</MetaDataID>
        string ChannelUri { get; set; }
        /// <MetaDataID>{d5eaf7c3-eda2-4119-b78f-7b5d0ecdf957}</MetaDataID>
        ExtObjectUri ObjectUri { get; }

        /// <MetaDataID>{6c477cf5-e0f2-4c79-b313-2a5e318e6e68}</MetaDataID>
        IMessage Invoke(IMessage msg);



    }


#if DeviceDotNet

    public class MarshalByRefObject : System.MarshalByRefObject,IExtMarshalByRefObject
    {

    }
#endif
}
