namespace OOAdvantech.Remoting
{

    using System;
#if !DeviceDotNet
    using System.Runtime.Remoting.Channels;
    using System.Security.AccessControl;
#endif
    using System.Security;

    using System.Security.Principal;
    using OOAdvantech.Collections;
    using System.Collections;
    using System.Linq;
    using System.Collections.Generic;
    using System.Runtime.Remoting;


    /// <MetaDataID>{722a4957-0b4e-42ce-8e0b-c7abb243b931}</MetaDataID>
    public struct ServerSessionPartInfo
    {
        public IServerSessionPart ServerSessionPart;
        public Guid ServerProcessIdentity;
        public bool? BidirectionalChannel;
        public string SessionIdentity;
    }
    /// <MetaDataID>{f01f4074-5413-4e77-ba07-ffd15fa8025f}</MetaDataID>
    public interface IRemotingServices
    {
        /// <MetaDataID>{c32d8271-8e9a-40fa-940e-6118044eafd5}</MetaDataID>
        ServerSessionPartInfo GetServerSession(string channelUri, Guid processIdentity);
        /// <MetaDataID>{bd569110-4d23-4eb6-ad98-d355444bc7d7}</MetaDataID>
        ClientSessionPart CreateClientSessionPart(string channelUri, Guid clientProcessIdentity, ServerSessionPartInfo serverSessionPartInfo);// Guid serverProcessIdentity, IServerSessionPart serverSessionPart,bool bidirectionalChannel=false);
    }

    /// <MetaDataID>{A5D01C02-3512-4BD4-A268-DB92700745B9}</MetaDataID>
    public class RemotingServices : MarshalByRefObject, IExtMarshalByRefObject, IRemotingServices
    {



        public ClientSessionPart CreateClientSessionPart(string channelUri, Guid clientProcessIdentity, ServerSessionPartInfo serverSessionPartInfo)// Guid serverProcessIdentity, IServerSessionPart serverSessionPart, bool bidirectionalChanel = false)
        {
            return new ClientSessionPart(channelUri, clientProcessIdentity, serverSessionPartInfo, this);
        }



        /// <MetaDataID>{7e6c4772-5ef4-4153-9ae7-3b09598b8259}</MetaDataID>
        public static System.Guid ProcessIdentity = System.Guid.NewGuid();

#if DeviceDotNet
        public static bool IsOutOfProcess(MarshalByRefObject marshalByRefObject)
        {
            if (marshalByRefObject is OOAdvantech.Remoting.RestApi.ITransparentProxy&& (marshalByRefObject as OOAdvantech.Remoting.RestApi.ITransparentProxy).GetProxy()!=null)
                return true;
            else
                return false;

            
        }
#endif


        /// <MetaDataID>{b30478c9-cafd-4ee6-8f44-cb091f3582fb}</MetaDataID>
        public ServerSessionPartInfo GetServerSession(string channelUri, Guid sessionIdentity)
        {
#if !DeviceDotNet
            var serversSessionPart = GetRemotingServices(channelUri).GetServerSession(sessionIdentity);
            return new ServerSessionPartInfo() { ServerSessionPart = serversSessionPart, ServerProcessIdentity = serversSessionPart.ServerProcessIdentity };
#else
            throw new NotImplementedException();
#endif
        }

        /// <MetaDataID>{D3A5F3C0-9EA7-4F50-AD61-EBF8AAA2205E}</MetaDataID>
        public static string GetChannelUri(object _object)
        {
            if (_object == null)
                return null;
            System.Runtime.Remoting.Proxies.RealProxy RealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(_object);
            if (RealProxy == null)
                return null;

            if (RealProxy is IProxy)
                return (RealProxy as IProxy).ChannelUri;
            else
            {
#if !DeviceDotNet
                var channelsUris = GetChannelUris(System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(_object as MarshalByRefObject));
                if (channelsUris == null || channelsUris.Count == 0)
                    return null;


                return channelsUris[0];
#else
                return null;
#endif


            }

            //marshalByRefObject = ModifyChannelDatas(marshalByRefObject);
            //return GetChannelUri(System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(marshalByRefObject));

        }



        //ServerSessionPartInfo IRemotingServices.GetServerSession(string channelUri, Guid processIdentity)
        //{
        //    throw new NotImplementedException();
        //}

        //ClientSessionPart IRemotingServices.CreateClientSessionPart(string channelUri, Guid clientProcessIdentity, Guid serverProcessIdentity, IServerSessionPart serverSessionPart)
        //{
        //    throw new NotImplementedException();
        //}

        /// <MetaDataID>{83969911-24c8-42de-9f6c-6823796d3da0}</MetaDataID>
        [MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.Association("", typeof(ServerSessionPart), MetaDataRepository.Roles.RoleA, "fc866fc9-8270-4d58-a0d5-8725d14c127c")]
        [MetaDataRepository.IgnoreErrorCheck]
        static public System.Collections.Generic.Dictionary<System.Guid, System.WeakReference> Sessions = new System.Collections.Generic.Dictionary<Guid, WeakReference>();


        /// <MetaDataID>{a4a29594-d501-4201-b445-d881d0ef8d21}</MetaDataID>
        internal static string MonoStateClassChannelUri;

#if !DeviceDotNet

        /// <MetaDataID>{C31810E1-4F3A-43E9-B128-BFF1F5ABF83C}</MetaDataID>
        public static RemotingServices GetRemotingServices(string ChannelUri)
        {
            //if (ServerChannelURIs.Count == 0)
            //{
            //    RegisterChannel(-1,false);//register tcp client sinks
            //    RegisterChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(),false);//regiser IPC
            //}


            bool tcpClientChannelRegister = false;
            bool ipcChannelRegister = false;

            foreach (var channel in System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels)
            {
                if (channel is System.Runtime.Remoting.Channels.Tcp.TcpChannel || channel is System.Runtime.Remoting.Channels.Tcp.TcpClientChannel)
                    tcpClientChannelRegister = true;
                if (channel is System.Runtime.Remoting.Channels.Ipc.IpcServerChannel)
                    if ((channel as System.Runtime.Remoting.Channels.Ipc.IpcServerChannel).GetChannelUri().ToLower() == ("ipc://" + "PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString()).ToLower())
                        ipcChannelRegister = true;
            }

            if (!tcpClientChannelRegister && ChannelUri.Trim().ToLower().IndexOf("tcp://") == 0)
                RegisterTcpClientChannel();
            if (!ipcChannelRegister && ChannelUri.ToLower().Trim().IndexOf("ipc://") == 0)
                RegisterIpcClientChannel();


            //RegisterChannel("PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), false);//regiser ipc

            if (ChannelUri == null || ChannelUri.Length == 0)
                throw new System.ArgumentException("Invalid ChannelUri", "ChannelUri");
            if (GetServerChannelUris().Contains(ChannelUri))
                return new RemotingServices();
            else
                return System.Activator.GetObject(typeof(Remoting.RemotingServices), ChannelUri + "/OOAdvantechRemotingServer/RemotingServices") as Remoting.RemotingServices; ;
        }

   
  
        /// <MetaDataID>{40A08F34-4319-4BCA-BDFC-88DB4AA06A5F}</MetaDataID>
        static public IPersistentObjectLifeTimeController PersistentObjectLifeTimeController;
        /// <MetaDataID>{12862FA2-2CB5-4684-B3F1-6FA9C7346769}</MetaDataID>
        /// <summary>This field defines a chain with cleint sink providers. 
        /// The last is the provider for remote object life time control. 
        /// The message to remote object travel though the sink which created from provider of chain. </summary>
        static public IClientChannelSinkProvider ClientChannelSinkProviderChain;
        /// <summary>This field defines a chain with server sink providers. 
        /// The first is the provider for remote object life time control. 
        /// The message to remote object travel though the sink which created from provider of chain. </summary>
        /// <MetaDataID>{8DC64637-5E3E-4246-8BB0-509E197FF426}</MetaDataID>
        static public IServerChannelSinkProvider ServerChannelSinkProviderChain;

        // public static bool ProcessUserAnonymous { get; private set; }

 
        /// <summary>
        /// This member defines the user impersonation for  implicitly created Session
        /// When is true all calls to the remote object impersonate to the user which initiate the session
        /// </summary>
        internal static bool ImpersonateToInitSessionUser;
        /// <MetaDataID>{99fb2839-26de-4995-8fe4-a5e744d72555}</MetaDataID>
        static bool HostMachineAddressInitialized = false;
        /// <MetaDataID>{86149734-9fee-4c41-a86b-b758d01faad3}</MetaDataID>
        static string[] _HostMachineAddress;

        /// <MetaDataID>{397de550-34bf-4f1b-bfb7-4a23c0b00bd8}</MetaDataID>
        static string[] HostMachineAddress
        {
            get
            {
                lock (Sessions)
                {
                    if (HostMachineAddressInitialized)
                        return _HostMachineAddress;
                    else
                    {
                        string hostName = System.Net.Dns.GetHostName();
                        System.Net.IPAddress[] IPAddresses = System.Net.Dns.GetHostAddresses(hostName);
                        _HostMachineAddress = new string[1 + IPAddresses.Length];
                        _HostMachineAddress[0] = hostName.Trim().ToLower();
                        int i = 1;
                        foreach (System.Net.IPAddress IPAddress in IPAddresses)
                            _HostMachineAddress[i++] = IPAddress.ToString();
                        HostMachineAddressInitialized = true;
                        return _HostMachineAddress;

                    }
                }
            }
        }
        /// <MetaDataID>{3f03f3a2-052a-4a6d-b940-2f89f23c4251}</MetaDataID>
        static public readonly WindowsIdentity ServerWindowsIdentity;
        /// <MetaDataID>{3091F525-9C49-4C13-9790-C55E6D44F333}</MetaDataID>
        static RemotingServices()
        {
            ServerWindowsIdentity = WindowsIdentity.GetCurrent();
             
            Tracker = new Tracker();
            System.Runtime.Remoting.Services.TrackingServices.RegisterTrackingHandler(Tracker);

            Type RemotingServicesType = typeof(RemotingServices);//ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.RemotingServices","");
            //Error prone οχι chycle dependancy να γίνει κάτι ανάλογο με τα sinks.
            System.Runtime.Remoting.WellKnownServiceTypeEntry wellKnownServiceTypeEntry = new System.Runtime.Remoting.WellKnownServiceTypeEntry(RemotingServicesType.FullName, RemotingServicesType.Assembly.FullName, "RemotingServices", System.Runtime.Remoting.WellKnownObjectMode.Singleton);


            //TODO είνα λάθος να ορίζεται εδώ το ApplicationName 
            System.Runtime.Remoting.RemotingConfiguration.ApplicationName = "OOAdvantechRemotingServer";
            System.Runtime.Remoting.RemotingConfiguration.RegisterWellKnownServiceType(wellKnownServiceTypeEntry);


        #region Register sink Providers
            InsertServerChannelSinkProvider(new Sinks.ServerSinkProvider(),
                typeof(BinaryServerFormatterSinkProvider));

            InsertClientChannelSinkProvider(new Sinks.ClientSinkProvider(),
                typeof(BinaryClientFormatterSinkProvider));
        #endregion

            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (System.Reflection.Assembly assembly in assemblies)
            {

                Object[] SupportSinkAttributes = assembly.GetCustomAttributes(typeof(Sinks.SupportSinkAttribute), true);
                foreach (Sinks.SupportSinkAttribute supportSink in SupportSinkAttributes)
                {
        #region Register SinkProvider
                    IServerChannelSinkProvider serverChannelSinkProvider = supportSink.CreateServerSinkProvider();
                    if (serverChannelSinkProvider != null)
                    {
                        InsertServerChannelSinkProvider(serverChannelSinkProvider,
                            typeof(Sinks.ServerSinkProvider));
                    }
                    IClientChannelSinkProvider clientChannelSinkProvider = supportSink.CreateClientSinkProvider() as IClientChannelSinkProvider;
                    if (clientChannelSinkProvider != null)
                    {
                        InsertClientChannelSinkProvider(clientChannelSinkProvider,
                            typeof(Sinks.ClientSinkProvider));
                    }
        #endregion

                }
            }
        }

        /// <summary>
        /// Defines all server channels
        /// </summary>
        /// <MetaDataID>{4b1268bb-8144-41fb-9dff-768de2e3fc27}</MetaDataID>
        internal static List<System.Runtime.Remoting.Channels.IChannelReceiver> ServerChannels = new List<IChannelReceiver>();

        /// <summary>
        /// Registers tcp client channel.
        /// Always client channel marked as secure and impersonate
        /// </summary>
        /// <MetaDataID>{8a54fd8d-e544-4240-a007-e9fd2f0efd95}</MetaDataID>
        public static void RegisterIpcClientChannel()
        {
        #region Unregister all ipc client channels
            foreach (var registerChanel in System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels)
            {
                if (registerChanel is System.Runtime.Remoting.Channels.Ipc.IpcClientChannel)
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(registerChanel);

                if (registerChanel is System.Runtime.Remoting.Channels.Ipc.IpcChannel)
                {
                    System.Runtime.Remoting.Channels.Ipc.IpcServerChannel ipcServerChannel = registerChanel.GetType().GetField("_serverChannel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(registerChanel) as System.Runtime.Remoting.Channels.Ipc.IpcServerChannel;

                    System.Collections.IDictionary serverChannelprops = new System.Collections.Hashtable();
                    serverChannelprops["name"] = ipcServerChannel.ChannelName;
                    serverChannelprops["portName"] = ipcServerChannel.GetChannelUri().Replace("ipc://", "");
                    serverChannelprops["secure"] = ipcServerChannel.IsSecured;
                    if (ipcServerChannel.IsSecured)
                    {
                        bool impersonate = (bool)ipcServerChannel.GetType().GetField("_impersonate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcServerChannel);
                        if (impersonate)
                        {
                            serverChannelprops["impersonate"] = true;
                            serverChannelprops["tokenImpersonationLevel"] = "Impersonation";
                        }
                    }
                    SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
                    // Get the NT account related to the SID
                    NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;

                    serverChannelprops["authorizedGroup"] = account.Value;
                    IServerChannelSinkProvider serverSinkProvider = ipcServerChannel.GetType().GetField("_sinkProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcServerChannel) as IServerChannelSinkProvider;
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(registerChanel);
                    if (serverSinkProvider is BinaryServerFormatterSinkProvider)
                        serverSinkProvider = null;
                    ipcServerChannel = new System.Runtime.Remoting.Channels.Ipc.IpcServerChannel(serverChannelprops, serverSinkProvider);
                    System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(ipcServerChannel, (bool)serverChannelprops["secure"]);

                }

            }
        #endregion

            System.Collections.IDictionary props = new System.Collections.Hashtable();
            props["name"] = "oorem ipc client";
            props["secure"] = true;
            props["impersonate"] = true;
            props["tokenImpersonationLevel"] = "Impersonation";
            System.Runtime.Remoting.Channels.IChannel channel = new System.Runtime.Remoting.Channels.Ipc.IpcClientChannel(props, CreateClientSinkChain());
            ChannelServices.RegisterChannel(channel, true);
        }
        /// <summary>
        /// Registers tcp client channel.
        /// Always client channel marked as secure and impersonate
        /// </summary>
        public static void RegisterTcpClientChannel()
        {
        #region Unregister all tcp client channels
            foreach (var registerChanel in System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels)
            {
                if (registerChanel is System.Runtime.Remoting.Channels.Tcp.TcpClientChannel)
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(registerChanel);

                if (registerChanel is System.Runtime.Remoting.Channels.Tcp.TcpChannel)
                {
                    System.Runtime.Remoting.Channels.Tcp.TcpServerChannel tcpServerChannel = registerChanel.GetType().GetField("_serverChannel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(registerChanel) as System.Runtime.Remoting.Channels.Tcp.TcpServerChannel;

                    System.Collections.IDictionary serverChannelprops = new System.Collections.Hashtable();
                    serverChannelprops["name"] = tcpServerChannel.ChannelName;
                    serverChannelprops["port"] = int.Parse(tcpServerChannel.GetChannelUri().Replace("tcp://", ""));
                    serverChannelprops["secure"] = tcpServerChannel.IsSecured;
                    if (tcpServerChannel.IsSecured)
                    {
                        bool impersonate = (bool)tcpServerChannel.GetType().GetField("_impersonate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(tcpServerChannel);
                        if (impersonate)
                        {
                            serverChannelprops["impersonate"] = true;
                            serverChannelprops["tokenImpersonationLevel"] = "Impersonation";
                        }
                    }
                    SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
                    // Get the NT account related to the SID
                    NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;

                    serverChannelprops["authorizedGroup"] = account.Value;
                    IServerChannelSinkProvider serverSinkProvider = tcpServerChannel.GetType().GetField("_sinkProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(tcpServerChannel) as IServerChannelSinkProvider;
                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(registerChanel);
                    if (serverSinkProvider is BinaryServerFormatterSinkProvider)
                        serverSinkProvider = null;
                    tcpServerChannel = new System.Runtime.Remoting.Channels.Tcp.TcpServerChannel(serverChannelprops, serverSinkProvider);
                    System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(tcpServerChannel, (bool)serverChannelprops["secure"]);

                }

            }
        #endregion

            System.Collections.IDictionary props = new System.Collections.Hashtable();
            ///Always impersonate is set
            props["name"] = "oorem tcp client";
            props["secure"] = true;
            props["impersonate"] = true;
            props["tokenImpersonationLevel"] = "Impersonation";
            System.Runtime.Remoting.Channels.IChannel channel = new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(props, CreateClientSinkChain());
            ChannelServices.RegisterChannel(channel, true);
        }



        #region Security channel infos
        //<channel ref="tcp" port="8001" secure="true" impersonate="true" authorizationModule="Server.AuthorizationModule,Server"/>
        #endregion

        /// <summary>
        ///Registers secure client-server ipc channel
        /// </summary>
        /// <param name="tcpPort">
        /// The ipc port which use the ipc channel to listen
        /// </param>
        /// <param name="impersonate">
        /// When it is true the server channel impersonate to client user
        /// </param>
        /// <MetaDataID>{832353c9-2554-4295-826b-702a20c95059}</MetaDataID>
        public static void RegisterSecureIpcChannel(string ipcPort, bool impersonate)
        {
            RegisterIpcClientChannel();



            if (string.IsNullOrEmpty(ipcPort))
                ipcPort = "PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();

            System.Collections.IDictionary props = new System.Collections.Hashtable();
            props = new System.Collections.Hashtable();
            props["name"] = "oorem ipc server";
            props["portName"] = ipcPort.Trim();
            props["secure"] = true;
            if (impersonate)
            {
                props["impersonate"] = true;
                props["tokenImpersonationLevel"] = "Impersonation";
            }
            SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            // Get the NT account related to the SID
            NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;
            props["authorizedGroup"] = account.Value;

            var ipcServerChannel = new System.Runtime.Remoting.Channels.Ipc.IpcServerChannel(props, CreateServerSinkChain(impersonate));//GetSecurityDescriptor()
            ChannelServices.RegisterChannel(ipcServerChannel, true);
            System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore = ipcServerChannel.ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
            string channelUri = channelDataStore.ChannelUris[0];
            OOAdvantech.Remoting.RemotingServices.AddServerChannelUri(channelUri);
            ServerChannels.Add(ipcServerChannel);

#endif
#if DEBUG && !DeviceDotNet
            SetLeaseTime();
#endif
#if !DeviceDotNet


        }


        /// <summary>
        ///Registers secure client-server tcp channel
        /// </summary>
        /// <param name="tcpPort">
        /// The tcp port which use the tcp channel to listen
        /// </param>
        /// <param name="impersonate">
        /// When it is true the server channel impersonate to client user
        /// </param>
        /// <MetaDataID>{4E62FDDC-9F76-431D-9061-6F48357A18BB}</MetaDataID>
        public static void RegisterSecureTcpChannel(int tcpPort, bool impersonate)
        {
            RegisterTcpClientChannel();

            System.Collections.IDictionary props = null;
            props = new System.Collections.Hashtable();
            props["name"] = "oorem tcp server";
            if (ServerChannelURIs.Count > 0)
                props["name"] = "oorem tcp server " + ServerChannelURIs.Count.ToString();
            props["secure"] = true;
            props["port"] = tcpPort;
            if (impersonate)
            {
                props["impersonate"] = true;
                props["tokenImpersonationLevel"] = "Impersonation";
            }
            SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            // Get the NT account related to the SID
            NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;
            props["authorizedGroup"] = account.Value;

            System.Runtime.Remoting.Channels.Tcp.TcpServerChannel tcpServerChannel = new System.Runtime.Remoting.Channels.Tcp.TcpServerChannel(props, CreateServerSinkChain(impersonate), new AuthorizeRemotingConnection());

            ChannelServices.RegisterChannel(tcpServerChannel, true);

            System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore = tcpServerChannel.ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
            string channelUri = channelDataStore.ChannelUris[0].ToLower();
            OOAdvantech.Remoting.RemotingServices.AddServerChannelUri(channelUri);
            string portDescription = channelUri.Replace("tcp://", "");
            portDescription = portDescription.Substring(portDescription.IndexOf(':') + 1);

            OOAdvantech.Remoting.RemotingServices.AddServerChannelUri("tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription);

            MonoStateClassChannelUri = "tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription;
            ServerChannels.Insert(0, tcpServerChannel);// new ChannelDataStore(new string[1] { "tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription });
#endif
#if DEBUG && !DeviceDotNet
            SetLeaseTime();
#endif

#if !DeviceDotNet
        }

        /// <summary>
        /// This method register a security free ipc server channel and default 
        /// </summary>
        /// <param name="ipcPort">
        /// Defines the IPC port name 
        /// </param>
        public static void RegisterFreeIpcChannel(string ipcPort)
        {
            RegisterIpcClientChannel();
            CheckIPCServerChannelsAutorityGroup();

            if (string.IsNullOrEmpty(ipcPort))
                ipcPort = "PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            System.Collections.IDictionary props = new System.Collections.Hashtable();
            props = new System.Collections.Hashtable();
            props["name"] = "oorem ipc server";
            props["portName"] = ipcPort.Trim();
            props["secure"] = false;

            SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            // Get the NT account related to the SID
            NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;
            props["authorizedGroup"] = account.Value;

            System.Runtime.Remoting.Channels.Ipc.IpcServerChannel ipcServerChannel = new System.Runtime.Remoting.Channels.Ipc.IpcServerChannel(props, CreateServerSinkChain(false));//GetSecurityDescriptor()
            ChannelServices.RegisterChannel(ipcServerChannel, false);
            System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore = ipcServerChannel.ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
            string channelUri = channelDataStore.ChannelUris[0];
            OOAdvantech.Remoting.RemotingServices.AddServerChannelUri(channelUri);

            ServerChannels.Add(ipcServerChannel);// new ChannelDataStore(new string[1] { "ipc://" + ipcPort });





#endif
#if DEBUG && !DeviceDotNet
            SetLeaseTime();
#endif

#if !DeviceDotNet

        }

        /// <summary>
        /// Re order channel data channels in order where the first channel is the channel of already istablished session with server process 
        /// </summary>
        /// <param name="objRef">
        /// Defines the object reference where method makes the reorder 
        /// </param>
        internal static void ReorderClientSideObjRefChannelData(ObjRef objRef)
        {
            var channelsUris = OOAdvantech.Remoting.RemotingServices.GetChannelUris(objRef);
            foreach (var channelUri in channelsUris)
            {
                ClientSessionPart clientSessionPart = RenewalManager.GetSession(channelUri, Proxy.RemotingServices);
                if (clientSessionPart != null)
                {
                    if (channelsUris[0] != channelUri)
                    {
                        System.Runtime.Remoting.Channels.ChannelDataStore serverChannelDataStore = (from channelData in objRef.ChannelInfo.ChannelData.OfType<System.Runtime.Remoting.Channels.ChannelDataStore>()
                                                                                                    from strChannelUri in channelData.ChannelUris
                                                                                                    where strChannelUri == channelUri
                                                                                                    select channelData).FirstOrDefault();
                        if (serverChannelDataStore != null)
                            OOAdvantech.Remoting.RemotingServices.ReorderChannels(objRef, serverChannelDataStore);
                    }
                }
            }
        }

        /// <summary>
        /// Re order channel data channels in order where the register server channels are at head of channel list 
        /// </summary>
        /// <param name="objRef">
        /// Defines the object reference where method makes the reorder 
        /// </param>
        internal static void ReorderServerSideObjRefCahnnelData(System.Runtime.Remoting.ObjRef objRef)
        {
            if (RemotingServices.ServerChannels.Count > 0)
            {
                for (int i = RemotingServices.ServerChannels.Count; i > 0; i--)
                {
                    var serverChannel = RemotingServices.ServerChannels[i - 1];
                    RemotingServices.ReorderChannels(objRef, serverChannel.ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore);
                }
            }
        }

        ///// <MetaDataID>{1C005B3D-CD4D-454B-8FB4-15C2E886D701}</MetaDataID>
        //  static System.Runtime.Remoting.Channels.IChannel Channel;

        //        public static void RegisterClientChannel(bool secure, bool impersonate)
        //        {
        //            System.Collections.IDictionary props = null;
        //            System.Runtime.Remoting.Channels.Tcp.TcpClientChannel channel = null;
        //            props = new System.Collections.Hashtable();
        //            props["secure"] = secure;
        //            //props["impersonate"] = impersonate;
        //            //if (impersonate)
        //            //    props["tokenImpersonationLevel"] = "Impersonation";
        //            channel = new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(props, CreateClientSinkChain());
        //            ChannelServices.RegisterChannel(channel, true);

        //#if DEBUG
        //            SetLeaseTime();
        //#endif

        //        }


        //        public static void RegisterTcpServerChannel(string channelName,int tcpPort, bool secure, bool impersonate)
        //        {

        //            string channelUri = null;
        //            System.Collections.IDictionary props = new System.Collections.Hashtable();
        //            props["port"] = tcpPort;
        //            props["secure"] = secure;
        //            if (!string.IsNullOrEmpty(channelName))
        //                props["name"] = channelName;
        //            if (impersonate)
        //            {
        //                props["impersonate"] = true;
        //                props["tokenImpersonationLevel"] = "Impersonation";
        //            }

        //            SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
        //            // Get the NT account related to the SID
        //            NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;
        //            props["authorizedGroup"] = account.Value;

        //            System.Runtime.Remoting.Channels.IChannel channel = new System.Runtime.Remoting.Channels.Tcp.TcpServerChannel(props, CreateServerSinkChain(impersonate));
        //            ChannelServices.RegisterChannel(channel, secure);

        //            System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore  = (channel as System.Runtime.Remoting.Channels.Tcp.TcpServerChannel).ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
        //            channelUri = channelDataStore.ChannelUris[0].ToLower();
        //            OOAdvantech.Remoting.RemotingServices.AddServerChannelUri(channelUri);
        //            string portDescription = channelUri.Replace("tcp://", "");
        //            portDescription = portDescription.Substring(portDescription.IndexOf(':') + 1);

        //            OOAdvantech.Remoting.RemotingServices.AddServerChannelUri("tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription);

        //            MonoStateClassChannelUri = "tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription;
        //            ServerChannelUriForRemoteMachine = new ChannelDataStore(new string[1] { "tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription });
        //#if DEBUG
        //            SetLeaseTime();
        //#endif




        //        }






        //        /// <MetaDataID>{924fb2ab-6ce0-48f5-92bf-c096e4581fac}</MetaDataID>
        //        public static void RegisterChannel(int tcpPort, string ipcPort)
        //        {

        //            System.Collections.IDictionary props = null;
        //            System.Runtime.Remoting.Channels.IChannel channel = null;
        //            System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore = null;
        //            string channelUri = null;
        //            if (tcpPort != -1)
        //            {
        //                props = new System.Collections.Hashtable();

        //                props["port"] = tcpPort;
        //                props["secure"] = true;
        //                props["impersonate"] = true;
        //                props["tokenImpersonationLevel"] = "Impersonation";



        //                channel =
        //                    new System.Runtime.Remoting.Channels.Tcp.TcpChannel(props, CreateClientSinkChain(), CreateServerSinkChain(true));


        //                ChannelServices.RegisterChannel(channel, true);

        //                channelDataStore = (channel as System.Runtime.Remoting.Channels.Tcp.TcpChannel).ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
        //                channelUri = channelDataStore.ChannelUris[0].ToLower();
        //                OOAdvantech.Remoting.RemotingServices.AddServerChannelUri(channelUri);
        //                string portDescription = channelUri.Replace("tcp://", "");
        //                portDescription = portDescription.Substring(portDescription.IndexOf(':') + 1);

        //                OOAdvantech.Remoting.RemotingServices.AddServerChannelUri("tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription);

        //                MonoStateClassChannelUri = "tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription;
        //                ServerChannelUriForRemoteMachine = new ChannelDataStore(new string[1] { "tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription });


        //            }
        //            else
        //            {
        //                props = new System.Collections.Hashtable();
        //                props["secure"] = true;
        //                props["impersonate"] = true;
        //                props["tokenImpersonationLevel"] = "Impersonation";

        //                channel =
        //                   new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(props, CreateClientSinkChain());


        //                ChannelServices.RegisterChannel(channel, true);

        //                //channelDataStore = (channel as System.Runtime.Remoting.Channels.Tcp.TcpChannel).ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
        //                //channelUri = channelDataStore.ChannelUris[0].ToLower();
        //            }
        //            //props = new System.Collections.Hashtable();

        //            //if (string.IsNullOrEmpty(ipcPort))
        //            //    props["portName"] = "PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
        //            //else
        //            //    props["portName"] = ipcPort.Trim();
        //            //props["secure"] = true;
        //            //props["impersonate"] = true;
        //            //props["tokenImpersonationLevel"] = "Impersonation";
        //            //// props["authorizedGroup"] = "Everyone";
        //            //channel = new System.Runtime.Remoting.Channels.Ipc.IpcClientChannel(props, CreateClientSinkChain());
        //            //ChannelServices.RegisterChannel(channel, true);
        //            //props["authorizedGroup"] = "Everyone";// "NtGroup";
        //            //channel = new System.Runtime.Remoting.Channels.Ipc.IpcServerChannel(props, CreateServerSinkChain(), GetSecurityDescriptor());
        //            //ChannelServices.RegisterChannel(channel, true);



        //            //channelDataStore = (channel as System.Runtime.Remoting.Channels.Ipc.IpcServerChannel).ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
        //            //channelUri = channelDataStore.ChannelUris[0];

        //            //OOAdvantech.Remoting.RemotingServices.AddServerChannelUri(channelUri);

        //#if DEBUG
        //            SetLeaseTime();
        //#endif



        //        }



        /// <summary>
        /// Set lease time for remote object
        /// </summary>
        /// <param name="remoteObject">
        /// Defines remote object where new lease time will be set
        /// </param>
        /// <param name="reNewTime">
        /// Define new lease time for renew 
        /// </param>
        public static void SetLeaseTime(System.MarshalByRefObject remoteObject, System.TimeSpan reNewTime)
        {
            if (remoteObject is IExtMarshalByRefObject)
            {

                ClientSessionPart clientSessionPart = RenewalManager.GetSession(GetChannelUri(remoteObject),Proxy.RemotingServices);
                if (Remoting.RemotingServices.ImpersonateToInitSessionUser && clientSessionPart != null)
                {
                    using (clientSessionPart.WindowsIdentity.Impersonate())
                    {
                        System.Runtime.Remoting.Lifetime.ILease lease = (Remoting.RemotingServices.GetOrgObject(remoteObject) as System.MarshalByRefObject).GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                        lease.Renew(reNewTime);
                    }
                }
                else
                {
                    System.Runtime.Remoting.Lifetime.ILease lease = (Remoting.RemotingServices.GetOrgObject(remoteObject) as System.MarshalByRefObject).GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                    lease.Renew(reNewTime);
                }
            }
            else
            {
                System.Runtime.Remoting.Lifetime.ILease lease = (Remoting.RemotingServices.GetOrgObject(remoteObject) as System.MarshalByRefObject).GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                lease.Renew(reNewTime);
            }
        }



        /// <MetaDataID>{b7082798-dc57-4e76-b5ed-da30c360fe18}</MetaDataID>
        static bool LeaseTimeIsSet;
        /// <MetaDataID>{04a5390a-3a49-46f3-ab5c-a3a7d35a9902}</MetaDataID>
        protected static void SetLeaseTime()
        {
            if (!LeaseTimeIsSet)
            {
                LeaseTimeIsSet = true;
                System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime = TimeSpan.FromSeconds(200);
                System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(100);
                System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout = TimeSpan.FromSeconds(100);/**/
            }
        }


    



        /// <MetaDataID>{5EB3AC34-AAE4-4FD2-BEFD-951BAD4C9863}</MetaDataID>
        static System.Runtime.Remoting.Channels.IClientChannelSinkProvider CreateClientSinkChain()
        {
            IClientChannelSinkProvider clientSinkChain = (ClientChannelSinkProviderChain as ICloneable).Clone() as IClientChannelSinkProvider;
            IClientChannelSinkProvider endOfClientSinkChain = clientSinkChain;
            while (endOfClientSinkChain.Next != null)
                endOfClientSinkChain = endOfClientSinkChain.Next;
            endOfClientSinkChain.Next = new BinaryClientFormatterSinkProvider();
            return clientSinkChain;
        }

        /// <MetaDataID>{3DAAD15E-2598-435C-A02B-CF347F5E5E62}</MetaDataID>
        private static System.Runtime.Remoting.Channels.IServerChannelSinkProvider CreateServerSinkChain(bool impersionate)
        {
            IServerChannelSinkProvider serverSinkChain = (ServerChannelSinkProviderChain as ICloneable).Clone() as IServerChannelSinkProvider;
            IServerChannelSinkProvider remotingServerSinkProvider = serverSinkChain;
            BinaryServerFormatterSinkProvider binaryServerSinkProvider = new BinaryServerFormatterSinkProvider();
            binaryServerSinkProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            binaryServerSinkProvider.Next = serverSinkChain;
            return binaryServerSinkProvider;
        }
        //public static void UnRegisterChannel()
        //{
        //    if(Channel==null)
        //        return;
        //    foreach(IChannel  channel in ChannelServices.RegisteredChannels)
        //    {
        //        if(channel==Channel)
        //        {
        //            ChannelServices.UnregisterChannel(channel);
        //            break;
        //        }
        //    }
        //    Channel=null;
        //}


        /// <MetaDataID>{A5D74C67-B4E4-49BD-82CB-BF7787793567}</MetaDataID>
        private static void InsertServerChannelSinkProvider(System.Runtime.Remoting.Channels.IServerChannelSinkProvider serverChannelSinkProvide, Type afterSinkType)
        {
            if (!(serverChannelSinkProvide is ICloneable))
                throw new System.ArgumentException("The object doesn't support ICloneable interface", "serverChannelSinkProvide");

            if (ServerChannelSinkProviderChain != null)
            {
                IServerChannelSinkProvider sinkProvider = ServerChannelSinkProviderChain;
                IServerChannelSinkProvider tmpSinkProvider = sinkProvider;
                while (tmpSinkProvider != null)
                {
                    if (tmpSinkProvider == serverChannelSinkProvide)
                        return;
                    tmpSinkProvider = tmpSinkProvider.Next;
                }

                while (sinkProvider.GetType() != afterSinkType)
                {
                    if (sinkProvider.Next != null)
                        sinkProvider = sinkProvider.Next;
                    else
                        break;
                }
                serverChannelSinkProvide.Next = sinkProvider.Next;
                sinkProvider.Next = serverChannelSinkProvide;
            }
            else
            {
                IServerChannelSinkProvider HeadSink = null;
                if (!(serverChannelSinkProvide is Sinks.ServerSinkProvider))
                {
                    HeadSink = new OOAdvantech.Remoting.Sinks.ServerSinkProvider();
                    HeadSink.Next = serverChannelSinkProvide;
                }
                else
                    HeadSink = serverChannelSinkProvide;
                ServerChannelSinkProviderChain = HeadSink;
            }
        }

        /// <MetaDataID>{AA52EDB6-89DE-42C6-B377-02CAA9AB9FA4}</MetaDataID>
        private static void InsertClientChannelSinkProvider(System.Runtime.Remoting.Channels.IClientChannelSinkProvider clientChannelSinkProvide, Type BeforeSinkType)
        {
            if (!(clientChannelSinkProvide is ICloneable))
                throw new System.ArgumentException("The object doesn't support ICloneable interface", "clientChannelSinkProvide");
            if (ClientChannelSinkProviderChain != null)
            {

                IClientChannelSinkProvider tmpSinkProvider = ClientChannelSinkProviderChain;
                while (tmpSinkProvider != null)
                {
                    if (tmpSinkProvider == clientChannelSinkProvide)
                        return;
                    tmpSinkProvider = tmpSinkProvider.Next;
                }

                IClientChannelSinkProvider sinkProvider = ClientChannelSinkProviderChain;
                while (sinkProvider.Next != null && sinkProvider.Next.GetType() != BeforeSinkType)
                {
                    if (sinkProvider.Next != null)
                        sinkProvider = sinkProvider.Next;
                    else
                        break;
                }
                if (sinkProvider.Next != null && sinkProvider.Next.GetType() == BeforeSinkType)
                {
                    clientChannelSinkProvide.Next = sinkProvider.Next;
                    sinkProvider.Next = clientChannelSinkProvide;
                }
                else
                {
                    clientChannelSinkProvide.Next = ClientChannelSinkProviderChain;
                    ClientChannelSinkProviderChain = clientChannelSinkProvide;
                }
            }
            else
            {
                IClientChannelSinkProvider headClientChannelSinkProvider = null;
                if (clientChannelSinkProvide is Sinks.ClientSinkProvider)
                {
                    headClientChannelSinkProvider = clientChannelSinkProvide;
                }
                else
                {
                    headClientChannelSinkProvider = clientChannelSinkProvide;
                    clientChannelSinkProvide.Next = new Sinks.ClientSinkProvider();
                }
                ClientChannelSinkProviderChain = headClientChannelSinkProvider;
            }
            /*if(Channel!=null)
            {
                UnRegisterChannel();
                RegisterChannel(Port);
            }*/


        }
        /// <MetaDataID>{e643b859-0d71-4a33-a499-c372a4673e35}</MetaDataID>
        public static bool IsTheLocalMachine(string computerName)
        {
            for (int hostMachineAddressIndex = 0; hostMachineAddressIndex < HostMachineAddress.Length; hostMachineAddressIndex++)
            {
                if (HostMachineAddress[hostMachineAddressIndex].ToLower() == computerName.ToLower())
                    return true;
            }
            if (computerName.Trim().ToLower() == "localhost" || computerName.Trim().ToLower() == "127.0.0.1")
                return true;
            else
                return false;
        }


        /// <MetaDataID>{509bd98a-ebd2-493e-921b-02d5b80e7e6a}</MetaDataID>
        public static bool IsInRemoteMachine(MarshalByRefObject marshalByRefObject)
        {

            if (marshalByRefObject == null)
                return false;

            if (!System.Runtime.Remoting.RemotingServices.IsTransparentProxy(marshalByRefObject))
                return false;

            System.Runtime.Remoting.ObjRef remoteObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(GetOrgObject(marshalByRefObject) as MarshalByRefObject);
            if (remoteObjRef == null)
                return false;
            else
            {
                Object[] data = remoteObjRef.ChannelInfo.ChannelData;
                for (int j = 0; j < data.Length; j++)
                {
                    if (data[j] is System.Runtime.Remoting.Channels.ChannelDataStore)
                    {
                        for (int channelUriIndexer = 0; channelUriIndexer < (data[j] as System.Runtime.Remoting.Channels.ChannelDataStore).ChannelUris.Length; channelUriIndexer++)
                        {
                            string channelUri = (data[j] as System.Runtime.Remoting.Channels.ChannelDataStore).ChannelUris[channelUriIndexer].ToLower();
                            int npos = channelUri.IndexOf(@"tcp:/");
                            if (npos == 0)
                            {
                                channelUri = channelUri.Substring(6);
                                npos = channelUri.IndexOf(":");
                                if (npos == -1)
                                    continue;
                                string remoteMachineAddress = channelUri.Substring(0, npos).Trim().ToLower();
                                for (int hostMachineAddressIndex = 0; hostMachineAddressIndex < HostMachineAddress.Length; hostMachineAddressIndex++)
                                {
                                    if (HostMachineAddress[hostMachineAddressIndex] == remoteMachineAddress)
                                        return false;
                                }
                                return true;
                            }
                            else
                                continue;
                        }
                    }
                }

            }
            return false;


        }
        /// <MetaDataID>{50C8A2E2-77E0-4E84-99D4-E83823F0D88F}</MetaDataID>
        public static bool IsOutOfProcess(MarshalByRefObject marshalByRefObject)
        {
            if (marshalByRefObject == null)
                return false;

            if (!System.Runtime.Remoting.RemotingServices.IsTransparentProxy(marshalByRefObject))
                return false;

            System.Runtime.Remoting.ObjRef ObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(GetOrgObject(marshalByRefObject) as MarshalByRefObject);
            if (ObjRef == null)
                return false;
            else
                return true;
        }
        /// <MetaDataID>{5F02ECC8-7DC7-4821-BE29-6F8A3FA77090}</MetaDataID>
        public static string GetObjectUri(MarshalByRefObject marshalByRefObject)
        {
            if (marshalByRefObject == null)
                return null;
            System.Runtime.Remoting.Proxies.RealProxy RealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(marshalByRefObject);
            if (RealProxy == null)
                return null;

            System.Runtime.Remoting.ObjRef ObjRef = null;
            if (RealProxy is Proxy)
                return (RealProxy as Proxy).URI.TransientUri;
            else if (RealProxy is IProxy)
                return (RealProxy as IProxy).ObjectUri.TransientUri;
            else 
                ObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(marshalByRefObject);
            return ObjRef.URI;

        }
        /// <MetaDataID>{701843F1-ECE0-4F98-B078-0FD2691594A1}</MetaDataID>
        public static object GetOrgObject(MarshalByRefObject marshalByRefObject)
        {
            if (marshalByRefObject == null)
                return null;

            Proxy RealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(marshalByRefObject) as Proxy;
            if (RealProxy == null)
                return marshalByRefObject;
            return RealProxy.RealTransparentProxy;
        }
        /// <MetaDataID>{B91B2A2F-6944-4FD2-98C6-5E223DF8A026}</MetaDataID>
        internal static string GetChannelUri(System.Runtime.Remoting.ObjRef ObjRef)
        {
            if (ObjRef == null)
                return null;
            if (ObjRef.ChannelInfo != null)
            {
                bool Continue = false;
                object[] ChannelData = ObjRef.ChannelInfo.ChannelData;
                foreach (object Object in ChannelData)
                {
                    if (Object is System.Runtime.Remoting.Channels.ChannelDataStore)
                    {
                        System.Runtime.Remoting.Channels.ChannelDataStore ChannelDataStore = Object as System.Runtime.Remoting.Channels.ChannelDataStore;
                        string ChannelUri = ChannelDataStore.ChannelUris[0];
                        return ChannelUri;
                    }
                }
            }
            return null;
        }

        internal static System.Collections.Generic.List<string> GetChannelUris(System.Runtime.Remoting.ObjRef ObjRef)
        {
            if (ObjRef == null)
                return null;
            System.Collections.Generic.List<string> channelUris = new System.Collections.Generic.List<string>();
            if (ObjRef.ChannelInfo != null)
            {

                object[] ChannelData = ObjRef.ChannelInfo.ChannelData;
                foreach (object Object in ChannelData)
                {
                    if (Object is System.Runtime.Remoting.Channels.ChannelDataStore)
                    {
                        System.Runtime.Remoting.Channels.ChannelDataStore ChannelDataStore = Object as System.Runtime.Remoting.Channels.ChannelDataStore;
                        string channelUri = ChannelDataStore.ChannelUris[0];
                        channelUris.Add(channelUri);
                    }
                }
            }
            return channelUris;
        }



        /// <summary>
        /// Re orders channelin channel data and makes the channel of serverChannelData first of all
        /// </summary>
        /// <param name="objRef"> 
        /// Defines the object reference where method makes the reorder 
        /// </param>
        /// <param name="serverChannelData">
        /// defines the first channel
        /// </param>
        internal static void ReorderChannels(System.Runtime.Remoting.ObjRef objRef, System.Runtime.Remoting.Channels.ChannelDataStore serverChannelData)
        {
            object[] channelData = objRef.ChannelInfo.ChannelData;
            bool reorderServerChannels = false;
            for (int i = 0; i != channelData.Length; i++)
            {
                if (channelData[i] == serverChannelData)
                {
                    reorderServerChannels = true;
                    break;
                }
            }
            if (reorderServerChannels)
            {
                object[] reOrderedchannelData = new object[objRef.ChannelInfo.ChannelData.Length];
                int startIndex = 0;
                if (channelData.Length > 0 && !(channelData[0] is System.Runtime.Remoting.Channels.ChannelDataStore))
                {
                    reOrderedchannelData[0] = channelData[0];
                    startIndex = 1;
                }

                reOrderedchannelData[startIndex] = serverChannelData;
                System.Collections.Generic.List<System.Runtime.Remoting.Channels.ChannelDataStore> serverUrls = new System.Collections.Generic.List<System.Runtime.Remoting.Channels.ChannelDataStore>();
                serverUrls.Add(serverChannelData);
                int k = startIndex + 1;
                for (int i = startIndex; i != channelData.Length; i++)
                {
                    if (channelData[i] is System.Runtime.Remoting.Channels.ChannelDataStore && serverUrls.Contains((channelData[i] as System.Runtime.Remoting.Channels.ChannelDataStore)))
                        continue;
                    reOrderedchannelData[k++] = channelData[i];

                }
                for (int i = 0; i != channelData.Length; i++)
                    channelData[i] = reOrderedchannelData[i];
            }
        }


   
        /// <MetaDataID>{11ba0eb1-e783-4eee-8358-a7463300d73f}</MetaDataID>
        internal static IExtMarshalByRefObject ReconnectWithObject(string channelUri, ExtObjectUri extObjectUri)
        {
            return GetRemotingServices(channelUri).ReconnectWithObject(extObjectUri);

        }


        /// <MetaDataID>{3C4552B5-0DA4-415B-BFFB-5940DD24DAAC}</MetaDataID>
        public IExtMarshalByRefObject ReconnectWithObject(ExtObjectUri extObjectUri)
        {
            try
            {
                object obj = null;
                if (Remoting.Tracker.WeakReferenceOnMarshaledObjects.ContainsKey(extObjectUri.TransientUri))
                {
                    WeakReference WeakReferenceOnMarshaledObject = Remoting.Tracker.WeakReferenceOnMarshaledObjects[extObjectUri.TransientUri] as WeakReference;


                    obj = WeakReferenceOnMarshaledObject.Target;

                    if (obj == null)
                        Remoting.Tracker.WeakReferenceOnMarshaledObjects.Remove(extObjectUri.TransientUri);
                }
                else
                {
                    IServerSessionPart session = GetServerSession(extObjectUri.SessionIDentity);
                    if (session != null)
                        obj = session.GetObjectFromUri(extObjectUri);
                    if (obj == null && !string.IsNullOrEmpty(extObjectUri.PersistentUri))
                        obj = PersistentObjectLifeTimeController.GetObject(extObjectUri.PersistentUri);
                }
                if (obj != null)
                    return obj as IExtMarshalByRefObject;
                if (!string.IsNullOrEmpty(extObjectUri.MonoStateTypeFullName))
                    return ReconnectWithMonoStateClass(extObjectUri.MonoStateTypeFullName);
            }
            catch (System.Exception Error)
            {
            }
            throw new System.Exception("Object Collected");
        }

        /// <MetaDataID>{8129F0EF-B7C1-4AA0-9454-D15E11BA3CED}</MetaDataID>
        static private System.Runtime.Remoting.Services.ITrackingHandler Tracker = null;

        /// <MetaDataID>{BB43C5E9-598A-4AA3-BB61-F1D9A248099B}</MetaDataID>
        public void RenewObjects(ref System.Collections.ArrayList MarshaledByRefObjects)
        {
            //DisconnectedUris=new System.Collections.ArrayList();
            //System.Collections.ArrayList CollectedObjected=new System.Collections.ArrayList();

            foreach (Remoting.ExtObjectUri CurrExtObjectUri in MarshaledByRefObjects)
            {

                try
                {

                    if (Remoting.Tracker.WeakReferenceOnMarshaledObjects.ContainsKey(CurrExtObjectUri.TransientUri))
                    {
                        WeakReference WeakReferenceOnMarshaledObject = Remoting.Tracker.WeakReferenceOnMarshaledObjects[CurrExtObjectUri.TransientUri] as WeakReference;
                        object referedObject = null;
                        if (WeakReferenceOnMarshaledObject.IsAlive)
                            referedObject = WeakReferenceOnMarshaledObject.Target;
                        if (referedObject != null)
                        {
                            System.Runtime.Remoting.Lifetime.ILease Lease = (referedObject as MarshalByRefObject).GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                            if (Lease != null)
                            {
                                Lease.Renew(System.TimeSpan.FromSeconds(20));
                                CurrExtObjectUri.Disconnected = false;
                            }
                            else
                                CurrExtObjectUri.Disconnected = true;

                            //if (CurrExtObjectUri.PersistentUriRequest && referedObject.GetType().GetCustomAttributes(typeof(MonoStateClassAttribute), true).Length > 0)
                            //    CurrExtObjectUri.PersistentUri = "#MonoStateClass#" + MonoStateClassChannelUri;
                            //else if (PersistentObjectLifeTimeController != null && CurrExtObjectUri.PersistentUriRequest)
                            //{
                            //    CurrExtObjectUri.PersistentUri = PersistentObjectLifeTimeController.GetPersistentObjectUri(referedObject);
                            //    System.Diagnostics.Debug.WriteLine(CurrExtObjectUri.PersistentUri);
                            //}
                        }
                        else
                        {
                            Remoting.Tracker.WeakReferenceOnMarshaledObjects.Remove(CurrExtObjectUri.TransientUri);
                        }
                    }
                    else
                        CurrExtObjectUri.Disconnected = true;

                }
                catch (System.Exception Error)
                {
                    CurrExtObjectUri.Disconnected = true;
                }

            }
        }

        /// <MetaDataID>{36673014-1939-45FE-BA25-E6B438E5B274}</MetaDataID>
        public virtual object CreateInstance(string TypeFullName, string assemblyData, Type[] paramsTypes, params object[] ctorParams)
        {

            //typeof(RemotingException).TypeInitializer


            object NewInstance = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(TypeFullName, assemblyData), paramsTypes, ctorParams);
            if (NewInstance != null && (NewInstance as System.MarshalByRefObject) == null)
                throw new Exception("The " + TypeFullName + " isn't type of System.MarshalByRefObject");
            return NewInstance;

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
            return CreateRemoteInstance(serverUrl, typeof(T).FullName, typeof(T).Assembly.FullName, ctorParamsTypes, ctorParamsValues) as T;
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
            return CreateRemoteInstance(serverUrl, typeof(T).FullName, typeof(T).Assembly.FullName) as T;
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
        /// <returns>
        /// Returns the new created remote object
        /// </returns>
        /// <MetaDataID>{40046d38-37d4-4c73-9744-306030f5bb7e}</MetaDataID>
        public static object CreateRemoteInstance(string serverUrl, string typeFullName, string assemblyData)
        {
            try
            {
                return GetRemotingServices(serverUrl).CreateInstance(typeFullName, assemblyData);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates remote object
        /// </summary>
        /// <param name="serverUrl">
        /// Defines the server url where new object will be created
        /// </param>
        /// <param name="assemblyQualifiedName">
        ///  Gets the assembly-qualified name of the System.Type, which includes the name
        ///  of the assembly from which the System.Type was loaded.
        /// </param>
        /// <returns></returns>
        public static object CreateRemoteInstance(string serverUrl, string assemblyQualifiedName)
        {
            try
            {
                int nPos = assemblyQualifiedName.IndexOf(",");
                string typeFullName = null;
                string assemblyData = "";

                if (nPos != -1)
                {
                    typeFullName = assemblyQualifiedName.Substring(0, nPos);
                    assemblyData = assemblyQualifiedName.Substring(nPos + 1).Trim();
                }
                else
                    typeFullName = assemblyQualifiedName;
                return GetRemotingServices(serverUrl).CreateInstance(typeFullName, assemblyData);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }
        /// <MetaDataID>{28e684cc-160b-4307-9c51-5ec432620dcc}</MetaDataID>
        public virtual object CreateInstance(string typeFullName, string assemblyData)
        {

            //typeof(RemotingException).TypeInitializer
            var type = ModulePublisher.ClassRepository.GetType(typeFullName, assemblyData);

            if (type == null && ModulePublisher.ClassRepository.LoadAssembly(assemblyData) == null)
                throw new Exception(string.Format("RunTime can't load assembly '{0}'", assemblyData));
            else
                if (type == null)
                throw new Exception(string.Format("Type '{0}' doesn't declared on Assembly '{1}'", typeFullName, assemblyData));


            object NewInstance = AccessorBuilder.CreateInstance(type);
            if (NewInstance != null && (NewInstance as MarshalByRefObject) == null)
                throw new Exception("The " + typeFullName + " isn't type of System.MarshalByRefObject");
            return NewInstance;

        }
        /// <MetaDataID>{0FAF35C4-6303-4D08-9576-5CB6335F23CA}</MetaDataID>
        static private List<object> ServerChannelURIs = new List<object>();
        /// <MetaDataID>{46A1C16A-C717-450C-9189-F661EC96A282}</MetaDataID>
        public static void AddServerChannelUri(string ChannelUri)
        {
            ChannelUri = ChannelUri.ToLower();
            if (!ServerChannelURIs.Contains(ChannelUri))
                ServerChannelURIs.Add(ChannelUri);
        }
        /// <MetaDataID>{96074F87-E3AE-4E0A-9793-D892A56773DC}</MetaDataID>
        public static List<object> GetServerChannelUris()
        {
            return ServerChannelURIs.ToList();
        }

        /// <MetaDataID>{e0bb7a93-0b3f-4d38-be47-efee67b3e31c}</MetaDataID>
        public static bool IsSeverChannelRegiter
        {
            get
            {
                return ServerChannelURIs.Count > 0;
            }
        }

        public static bool IsIpcSeverChannelRegiter(string port)
        {

            foreach (string serverChannel in ServerChannelURIs)
            {
                if (serverChannel.ToLower() == "ipc://" + port.ToLower())
                    return true;
            }

            return false;
        }

        public static bool IsTcpSeverChannelRegiter(int port)
        {

            foreach (string serverChannel in ServerChannelURIs)
            {
                if (serverChannel.ToLower() == "tcp://" + port.ToString().ToLower())
                    return true;
            }

            return false;
        }




    
        /// <MetaDataID>{8c1f84a3-1b1e-44db-bac9-5c5ed95f4943}</MetaDataID>
        static OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();

        /// <MetaDataID>{92028471-8fc9-4422-af67-0a25bf194190}</MetaDataID>
        public IServerSessionPart GetServerSession(Guid clientProcessIdentity)
        {


            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                ServerSessionPart serverSessionPart = null;
                if (Sessions.ContainsKey(clientProcessIdentity))
                {
                    serverSessionPart = Sessions[clientProcessIdentity].Target as ServerSessionPart;
                    serverSessionPart = Sessions[clientProcessIdentity].Target as ServerSessionPart;
                    if (serverSessionPart == null)
                    {
                        serverSessionPart = new ServerSessionPart(clientProcessIdentity);
                        Sessions[clientProcessIdentity] = new WeakReference(serverSessionPart);
                    }
                }
                else
                {
                    serverSessionPart = new ServerSessionPart(clientProcessIdentity);
                    Sessions[clientProcessIdentity] = new WeakReference(serverSessionPart);
                }

                System.Runtime.Remoting.RemotingServices.Marshal(serverSessionPart);
                System.Runtime.Remoting.Lifetime.ILease lease = GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                if (lease != null)
                    lease.Renew(System.TimeSpan.FromMinutes(0.5));
                return serverSessionPart;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }



        }

        //internal static IExtMarshalByRefObject ReconnectWithMonoStateClass(string channelUri, string typeFullName)
        //{
        //    return GetRemotingServices(channelUri).ReconnectWithMonoStateClass(typeFullName);

        //}

        /// <MetaDataID>{2f855e6b-4154-4f89-975e-3c30d18ddf85}</MetaDataID>
        IExtMarshalByRefObject ReconnectWithMonoStateClass(string typeFullName)
        {
            object NewInstance = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(typeFullName, ""));
            if (NewInstance != null && (NewInstance as System.MarshalByRefObject) == null)
                throw new Exception("The " + typeFullName + " isn't type of System.MarshalByRefObject");
            return (IExtMarshalByRefObject)NewInstance;

        }

        /// <MetaDataID>{32b20f0f-c1df-480a-a5cf-ae66c1412298}</MetaDataID>
        internal static string GetHostAddress(string channelUri)
        {
            channelUri = channelUri.ToLower();
            if (channelUri.IndexOf("tcp://") == 0)
            {
                channelUri = channelUri.Replace("tcp://", "");
                channelUri = channelUri.Substring(0, channelUri.IndexOf(":"));
                return channelUri;
            }
            return null;
        }

        /// <MetaDataID>{97242364-1d16-462a-a3aa-8bf1b8dc9aa8}</MetaDataID>
        internal static bool HasHostProcessNetworkAccess
        {
            get
            {
                foreach (string channelUri in GetServerChannelUris())
                {
                    if (channelUri.IndexOf(@"tcp:/") == 0)
                        return true;
                }
                return false;
            }
        }

        /// <MetaDataID>{99b79cdb-1768-46f4-b07a-a8d59cc6326b}</MetaDataID>
        public static void SubscribeEventConsumers(System.Collections.Generic.List<EventSubscrioption> subscriptions)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<RemoteEventSubscription>> sessionSubscriptions = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<RemoteEventSubscription>>();
            foreach (EventSubscrioption eventSubscrioption in subscriptions)
            {

                System.Collections.Generic.List<RemoteEventSubscription> channelSubscriptions = null;
                string channelUri = GetChannelUri(eventSubscrioption.Obj as MarshalByRefObject);

                if (!sessionSubscriptions.TryGetValue(channelUri, out channelSubscriptions))
                {
                    channelSubscriptions = new System.Collections.Generic.List<RemoteEventSubscription>();
                    sessionSubscriptions[channelUri] = channelSubscriptions;
                }
                Proxy proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(eventSubscrioption.Obj) as Proxy;
                if (proxy == null)
                {
                    try
                    {
                        eventSubscrioption.EventInfo.AddEventHandler(eventSubscrioption.Obj, eventSubscrioption.Handler);
                    }
                    catch (Exception error)
                    {
                    }
                }
                else
                {
                    bool proceedToSessionSubscription;
                    bool allowAsynchronous;
                    proxy.EventConsumingResolver.EventConsumerSubscription(eventSubscrioption.Handler, eventSubscrioption.EventInfo, out proceedToSessionSubscription, out allowAsynchronous);
                    if (proceedToSessionSubscription)
                    {
                        RemoteEventSubscription remoteEventSubscription = new RemoteEventSubscription();
                        remoteEventSubscription.AllowAsynchronous = allowAsynchronous;
                        remoteEventSubscription.eventInfo =new EventInfoData( eventSubscrioption.EventInfo);
                        remoteEventSubscription.ExtObjectUri = proxy.URI;
                        channelSubscriptions.Add(remoteEventSubscription);
                    }
                }
            }

            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<RemoteEventSubscription>> entry in sessionSubscriptions)
            {
                
                RenewalManager.GetSession(entry.Key, Proxy.RemotingServices).Subscribe(entry.Value);
            }
        }

        /// <MetaDataID>{0793fb1c-8ce6-4ed8-89d0-b06758d81b1a}</MetaDataID>
        public static void UnsubscribeEventConsumers(System.Collections.Generic.List<EventSubscrioption> unsubscriptions)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<RemoteEventSubscription>> sessionSubscriptions = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<RemoteEventSubscription>>();
            foreach (EventSubscrioption eventSubscrioption in unsubscriptions)
            {

                System.Collections.Generic.List<RemoteEventSubscription> channelSubscriptions = null;
                string channelUri = GetChannelUri(eventSubscrioption.Obj as MarshalByRefObject);
                if (!sessionSubscriptions.TryGetValue(channelUri, out channelSubscriptions))
                {
                    channelSubscriptions = new System.Collections.Generic.List<RemoteEventSubscription>();
                    sessionSubscriptions[channelUri] = channelSubscriptions;
                }
                Proxy proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(eventSubscrioption.Obj) as Proxy;
                if (proxy == null)
                {
                    try
                    {
                        eventSubscrioption.EventInfo.AddEventHandler(eventSubscrioption.Obj, eventSubscrioption.Handler);
                    }
                    catch (Exception error)
                    {
                    }
                }
                else
                {
                    bool proceedToSessionUnsubscribe = false;

                    proxy.EventConsumingResolver.EventConsumerUnsubscribe(eventSubscrioption.Handler, eventSubscrioption.EventInfo, out proceedToSessionUnsubscribe);
                    if (proceedToSessionUnsubscribe)
                    {
                        RemoteEventSubscription remoteEventSubscription = new RemoteEventSubscription();
                        remoteEventSubscription.eventInfo =new EventInfoData( eventSubscrioption.EventInfo);
                        remoteEventSubscription.ExtObjectUri = proxy.URI;
                        channelSubscriptions.Add(remoteEventSubscription);
                    }
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<RemoteEventSubscription>> entry in sessionSubscriptions)
                RenewalManager.GetSession(entry.Key, Proxy.RemotingServices).                                                                        Unsubscribe(entry.Value);
        }


        static IChannel[] RegisteredChannels;
        internal static void CheckIPCServerChannelsAutorityGroup()
        {
            lock (Sessions)
            {
                if (RegisteredChannels == null || RegisteredChannels.Length != System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length)
                {
                    SetIPCServerRegisteredChannelsAuthorizedGroup();
                    RegisteredChannels = new IChannel[System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length];
                    for (int i = 0; i != RegisteredChannels.Length; i++)
                        RegisteredChannels[i] = System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels[i];
                }
                else
                {
                    bool recheck = false;
                    for (int i = 0; i != RegisteredChannels.Length; i++)
                    {
                        if (RegisteredChannels[i] != System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels[i])
                        {
                            recheck = true;
                            break;
                        }
                    }
                    if (recheck)
                    {
                        SetIPCServerRegisteredChannelsAuthorizedGroup();
                        RegisteredChannels = new IChannel[System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.Length];
                        for (int i = 0; i != RegisteredChannels.Length; i++)
                            RegisteredChannels[i] = System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels[i];
                    }
                }
            }

        }


        private static void SetIPCServerRegisteredChannelsAuthorizedGroup()
        {
            foreach (var registerChanel in System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels.ToArray())
            {
                if (registerChanel is System.Runtime.Remoting.Channels.Ipc.IpcChannel)
                {
                    SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
                    // Get the NT account related to the SID
                    NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;


                    System.Runtime.Remoting.Channels.Ipc.IpcServerChannel ipcServerChannel = registerChanel.GetType().GetField("_serverChannel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(registerChanel) as System.Runtime.Remoting.Channels.Ipc.IpcServerChannel;
                    string authorizedGroup = ipcServerChannel.GetType().GetField("_authorizedGroup", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcServerChannel) as string;
                    if (account.Value != authorizedGroup)
                    {
                        System.Runtime.Remoting.Channels.Ipc.IpcClientChannel ipcClientChannel = registerChanel.GetType().GetField("_clientChannel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(registerChanel) as System.Runtime.Remoting.Channels.Ipc.IpcClientChannel;

                        System.Collections.IDictionary serverChannelprops = new System.Collections.Hashtable();
                        serverChannelprops["name"] = ipcServerChannel.ChannelName;
                        serverChannelprops["portName"] = ipcServerChannel.GetChannelUri().Replace("ipc://", "");
                        serverChannelprops["secure"] = ipcServerChannel.IsSecured;
                        if (ipcServerChannel.IsSecured)
                        {
                            bool impersonate = (bool)ipcServerChannel.GetType().GetField("_impersonate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcServerChannel);
                            if (impersonate)
                            {
                                serverChannelprops["impersonate"] = true;
                                serverChannelprops["tokenImpersonationLevel"] = "Impersonation";
                            }
                        }
                        serverChannelprops["authorizedGroup"] = account.Value;

                        bool isSecure = ipcServerChannel.IsSecured;

                        IServerChannelSinkProvider serverSinkProvider = ipcServerChannel.GetType().GetField("_sinkProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcServerChannel) as IServerChannelSinkProvider;
                        IClientChannelSinkProvider clientSinkProvider = ipcClientChannel.GetType().GetField("_sinkProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcClientChannel) as IClientChannelSinkProvider;

                        System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(registerChanel);
                        if (clientSinkProvider is BinaryClientFormatterSinkProvider)
                            clientSinkProvider = null;
                        if (serverSinkProvider is BinaryServerFormatterSinkProvider)
                            serverSinkProvider = null;
                        System.Runtime.Remoting.Channels.Ipc.IpcChannel ipcChannel = new System.Runtime.Remoting.Channels.Ipc.IpcChannel(serverChannelprops, clientSinkProvider, serverSinkProvider);
                        ChannelServices.RegisterChannel(ipcChannel, isSecure);
                    }
                }
                else if (registerChanel is System.Runtime.Remoting.Channels.Ipc.IpcServerChannel)
                {
                    SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
                    // Get the NT account related to the SID
                    NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;

                    System.Runtime.Remoting.Channels.Ipc.IpcServerChannel ipcServerChannel = registerChanel as System.Runtime.Remoting.Channels.Ipc.IpcServerChannel;
                    string authorizedGroup = ipcServerChannel.GetType().GetField("_authorizedGroup", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcServerChannel) as string;
                    if (account.Value != authorizedGroup)
                    {

                        System.Collections.IDictionary serverChannelprops = new System.Collections.Hashtable();
                        serverChannelprops["name"] = ipcServerChannel.ChannelName;
                        serverChannelprops["portName"] = ipcServerChannel.GetChannelUri().Replace("ipc://", "");
                        serverChannelprops["secure"] = ipcServerChannel.IsSecured;
                        if (ipcServerChannel.IsSecured)
                        {
                            bool impersonate = (bool)ipcServerChannel.GetType().GetField("_impersonate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcServerChannel);
                            if (impersonate)
                            {
                                serverChannelprops["impersonate"] = true;
                                serverChannelprops["tokenImpersonationLevel"] = "Impersonation";
                            }
                        }
                        serverChannelprops["authorizedGroup"] = account.Value;

                        bool isSecure = ipcServerChannel.IsSecured;

                        IServerChannelSinkProvider serverSinkProvider = ipcServerChannel.GetType().GetField("_sinkProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ipcServerChannel) as IServerChannelSinkProvider;

                        System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(registerChanel);
                        if (serverSinkProvider is BinaryServerFormatterSinkProvider)
                            serverSinkProvider = null;

                        ipcServerChannel = new System.Runtime.Remoting.Channels.Ipc.IpcServerChannel(serverChannelprops, serverSinkProvider);
                        ChannelServices.RegisterChannel(ipcServerChannel, isSecure);

                    }

                }

            }
        }


        //public ClientSessionPart CreateClientSessionPart(string channelUri, Guid clientProcessIdentity, Guid serverProcessIdentity, IServerSessionPart serverSessionPart)
        //{
        //    return new ClientSessionPart(channelUri, clientProcessIdentity, serverProcessIdentity, serverSessionPart, this);
        //}


        ///// <MetaDataID>{132fd33e-6641-4b52-8e94-9a5cdddac427}</MetaDataID>
        //internal static IExtMarshalByRefObject ModifyChannelData(MarshalByRefObject marshalByRefObject)
        //{
        //    return marshalByRefObject as IExtMarshalByRefObject;
        //    if (!RemotingServices.IsInRemoteMachine(marshalByRefObject as MarshalByRefObject))
        //    {
        //        System.Runtime.Remoting.ObjRef remoteObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(marshalByRefObject as MarshalByRefObject);
        //        if (remoteObjRef != null)
        //        {
        //            Object[] data = remoteObjRef.ChannelInfo.ChannelData;
        //            int firstChannelIndex = -1;

        //            System.Runtime.Remoting.Channels.ChannelDataStore tcpChannel = null;
        //            System.Runtime.Remoting.Channels.ChannelDataStore ipcChannel = null;
        //            for (int j = 0; j < data.Length; j++)
        //            {
        //                if (data[j] is System.Runtime.Remoting.Channels.ChannelDataStore)
        //                {
        //                    if (firstChannelIndex == -1)
        //                        firstChannelIndex = j;
        //                    System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore = data[j] as System.Runtime.Remoting.Channels.ChannelDataStore;
        //                    if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"tcp:/") == 0)
        //                        tcpChannel = channelDataStore;
        //                    if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"ipc") == 0)
        //                        ipcChannel = channelDataStore;
        //                }
        //            }
        //            // if (data[firstChannelIndex] != ipcChannel)
        //            {
        //                if (ipcChannel != null)
        //                    data[firstChannelIndex] = ipcChannel;
        //                if (tcpChannel != null)
        //                    data[firstChannelIndex + 1] = tcpChannel;

        //                #region Rebuild channel sink
        //                //Αυτός ο τρόpος που γίνεται rebuild η channel sink δεν είναι ασφαλές γιατί χρησιμοποιεί μη
        //                //ντοκιουμενταρισμένες operations και class του .net framework

        //                //Class:        System.Runtime.Remoting.IdentityHolder
        //                //operation:    static void RemoveIdentity(string uri, bool bResetURI)

        //                System.Type type = typeof(System.Runtime.Remoting.RemotingServices).Assembly.GetType("System.Runtime.Remoting.IdentityHolder");
        //                foreach (System.Reflection.MethodInfo method in type.GetMethods(System.Reflection.BindingFlags.Public |
        //                                                System.Reflection.BindingFlags.NonPublic |
        //                                                 System.Reflection.BindingFlags.Static |
        //                                                System.Reflection.BindingFlags.DeclaredOnly))
        //                {
        //                    if (method.Name == "RemoveIdentity" && method.GetParameters().Length == 2)
        //                    {
        //                        method.Invoke(null, new object[2] { remoteObjRef.URI, false });
        //                        break;
        //                    }
        //                }
        //                marshalByRefObject = System.Runtime.Remoting.RemotingServices.Unmarshal(remoteObjRef) as MarshalByRefObject;
        //                return marshalByRefObject as IExtMarshalByRefObject;
        //                #endregion
        //            }

        //        }

        //    }
        //    return marshalByRefObject as IExtMarshalByRefObject;

        //}

        ///// <MetaDataID>{7b949d0c-f484-4993-8a80-32882bedfaa9}</MetaDataID>
        //static CommonSecurityDescriptor GetSecurityDescriptor()
        //{
        //    // This is the wellknown sid for network sid
        //    string networkSidSddlForm = @"S-1-5-2";
        //    // Local administrators sid
        //    SecurityIdentifier localAdminSid = new SecurityIdentifier(
        //        WellKnownSidType.BuiltinAdministratorsSid, null);
        //    // Local Power users sid
        //    SecurityIdentifier powerUsersSid = new SecurityIdentifier(
        //        WellKnownSidType.BuiltinPowerUsersSid, null);
        //    // Network sid
        //    SecurityIdentifier networkSid = new SecurityIdentifier(networkSidSddlForm);

        //    // Local Power users sid
        //    SecurityIdentifier everyone = new SecurityIdentifier(
        //        WellKnownSidType.WorldSid, null);


        //    DiscretionaryAcl dacl = new DiscretionaryAcl(false, false, 1);

        //    // Disallow access from off machine
        //    dacl.AddAccess(AccessControlType.Deny, networkSid, -1,
        //        InheritanceFlags.None, PropagationFlags.None);

        //    // Allow acces only from local administrators and power users
        //    dacl.AddAccess(AccessControlType.Allow, localAdminSid, -1,
        //        InheritanceFlags.None, PropagationFlags.None);
        //    dacl.AddAccess(AccessControlType.Allow, powerUsersSid, -1,
        //        InheritanceFlags.None, PropagationFlags.None);

        //    //dacl.AddAccess(AccessControlType.Allow, everyone, -1,
        //    //    InheritanceFlags.None, PropagationFlags.None);


        //    CommonSecurityDescriptor securityDescriptor =
        //        new CommonSecurityDescriptor(false, false,
        //                ControlFlags.GroupDefaulted |
        //                ControlFlags.OwnerDefaulted |
        //                ControlFlags.DiscretionaryAclPresent,
        //                null, null, null, dacl);
        //    return securityDescriptor;
        //}


#else
        internal static bool HasHostProcessNetworkAccess
        {
            get
            {
                return false;
            }
        }

#endif

    }

#if !DeviceDotNet
    /// <MetaDataID>{14ea6796-ecc8-4954-9cc6-f19b01a1f392}</MetaDataID>
    public class AuthorizeRemotingConnection : IAuthorizeRemotingConnection
    {

    #region IAuthorizeRemotingConnection Members

        public bool IsConnectingEndPointAuthorized(System.Net.EndPoint endPoint)
        {
            return true;
        }

        public bool IsConnectingIdentityAuthorized(IIdentity identity)
        {
            return true;
        }

    #endregion
    }
#endif
}
