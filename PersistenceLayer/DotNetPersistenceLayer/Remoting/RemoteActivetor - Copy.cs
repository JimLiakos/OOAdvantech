namespace OOAdvantech.Remoting
{

    using System;
    using System.Runtime.Remoting.Channels;
    using System.Security;
    using System.Security.AccessControl;
    using System.Security.Principal;



    /// <MetaDataID>{A5D01C02-3512-4BD4-A268-DB92700745B9}</MetaDataID>
    public class RemotingServices : System.MarshalByRefObject, IExtMarshalByRefObject
    {
        /// <MetaDataID>{83969911-24c8-42de-9f6c-6823796d3da0}</MetaDataID>
        [MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.Association("", typeof(ServerSessionPart), MetaDataRepository.Roles.RoleA, "fc866fc9-8270-4d58-a0d5-8725d14c127c")]
        [MetaDataRepository.IgnoreErrorCheck]
        static public System.Collections.Generic.Dictionary<System.Guid, System.WeakReference> Sessions = new System.Collections.Generic.Dictionary<Guid, WeakReference>();

        /// <MetaDataID>{7e6c4772-5ef4-4153-9ae7-3b09598b8259}</MetaDataID>
        public static System.Guid ProcessIdentity = System.Guid.NewGuid();

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


        /// <MetaDataID>{C31810E1-4F3A-43E9-B128-BFF1F5ABF83C}</MetaDataID>
        public static RemotingServices GetRemotingServices(string ChannelUri)
        {
            if (ServerChannelURIs.Count == 0)
                RegisterChannel(-1, null);
            if (ChannelUri == null || ChannelUri.Length == 0)
                throw new System.ArgumentException("Invalid ChannelUri", "ChannelUri");
            if (GetServerChannelUris().Contains(ChannelUri))
                return new RemotingServices();
            else
                return System.Activator.GetObject(typeof(Remoting.RemotingServices), ChannelUri + "/OOAdvantechRemotingServer/RemotingServices") as Remoting.RemotingServices; ;
        }

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

        /// <MetaDataID>{a4a29594-d501-4201-b445-d881d0ef8d21}</MetaDataID>
        internal static string MonoStateClassChannelUri;
        /// <MetaDataID>{4b1268bb-8144-41fb-9dff-768de2e3fc27}</MetaDataID>
        internal static System.Runtime.Remoting.Channels.ChannelDataStore ServerChannelUriForRemoteMachine;

        ///// <MetaDataID>{1C005B3D-CD4D-454B-8FB4-15C2E886D701}</MetaDataID>
        //  static System.Runtime.Remoting.Channels.IChannel Channel;

        /// <MetaDataID>{4E62FDDC-9F76-431D-9061-6F48357A18BB}</MetaDataID>
        public static void RegisterChannel(int tcpPort, string ipcPort)
        {

            System.Collections.IDictionary props = null;
            System.Runtime.Remoting.Channels.IChannel channel = null;
            System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore = null;
            string channelUri = null;
            if (tcpPort != -1)
            {
                props = new System.Collections.Hashtable();

                props["port"] = tcpPort;
                props["secure"] = true;
                props["impersonate"] = true;
                props["tokenImpersonationLevel"] = "Impersonation";



                channel =
                    new System.Runtime.Remoting.Channels.Tcp.TcpChannel(props, CreateClientSinkChain(), CreateServerSinkChain());


                ChannelServices.RegisterChannel(channel, true);

                channelDataStore = (channel as System.Runtime.Remoting.Channels.Tcp.TcpChannel).ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
                channelUri = channelDataStore.ChannelUris[0].ToLower();
                OOAdvantech.Remoting.RemotingServices.AddServerChannelUri(channelUri);
                string portDescription = channelUri.Replace("tcp://", "");
                portDescription = portDescription.Substring(portDescription.IndexOf(':') + 1);

                OOAdvantech.Remoting.RemotingServices.AddServerChannelUri("tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription);

                MonoStateClassChannelUri = "tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription;
                ServerChannelUriForRemoteMachine = new ChannelDataStore(new string[1] { "tcp://" + System.Net.Dns.GetHostName() + ":" + portDescription });


            }
            else
            {
                props = new System.Collections.Hashtable();
                props["secure"] = true;
                props["impersonate"] = true;
                props["tokenImpersonationLevel"] = "Impersonation";

                channel =
                   new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(props, CreateClientSinkChain());


                ChannelServices.RegisterChannel(channel, true);

                //channelDataStore = (channel as System.Runtime.Remoting.Channels.Tcp.TcpChannel).ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
                //channelUri = channelDataStore.ChannelUris[0].ToLower();
            }


            props = new System.Collections.Hashtable();
            if (string.IsNullOrEmpty(ipcPort))
                props["portName"] = "PID" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            else
                props["portName"] = ipcPort.Trim();
            props["secure"] = true;
            props["impersonate"] = true;
            props["tokenImpersonationLevel"] = "Impersonation";
            // props["authorizedGroup"] = "Everyone";
            channel = new System.Runtime.Remoting.Channels.Ipc.IpcClientChannel(props, CreateClientSinkChain());
            ChannelServices.RegisterChannel(channel, true);
            props["authorizedGroup"] = "Everyone";// "NtGroup";
            channel = new System.Runtime.Remoting.Channels.Ipc.IpcServerChannel(props, CreateServerSinkChain());// GetSecurityDescriptor());
            ChannelServices.RegisterChannel(channel, true);



            // var properties = new System.Collections.Hashtable();
            //props["portName"] = ipcPort.Trim();
            //props["priority"] = "20";
            //props["secure"] = true;
            //props["authorizedGroup"] = "Everyone";
            //channel = new System.Runtime.Remoting.Channels.Ipc.IpcServerChannel(props, null);
            //ChannelServices.RegisterChannel(channel, true);



            channelDataStore = (channel as System.Runtime.Remoting.Channels.Ipc.IpcServerChannel).ChannelData as System.Runtime.Remoting.Channels.ChannelDataStore;
            channelUri = channelDataStore.ChannelUris[0];

            OOAdvantech.Remoting.RemotingServices.AddServerChannelUri(channelUri);

#if DEBUG
            System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime = TimeSpan.FromSeconds(20);
            System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(10);
            System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout = TimeSpan.FromSeconds(10);/**/
#endif



        }

        /// <MetaDataID>{7b949d0c-f484-4993-8a80-32882bedfaa9}</MetaDataID>
        static CommonSecurityDescriptor GetSecurityDescriptor()
        {
            // This is the wellknown sid for network sid
            string networkSidSddlForm = @"S-1-5-2";
            // Local administrators sid
            SecurityIdentifier localAdminSid = new SecurityIdentifier(
                WellKnownSidType.BuiltinAdministratorsSid, null);
            // Local Power users sid
            SecurityIdentifier powerUsersSid = new SecurityIdentifier(
                WellKnownSidType.BuiltinPowerUsersSid, null);
            // Network sid
            SecurityIdentifier networkSid = new SecurityIdentifier(networkSidSddlForm);

            // Local Power users sid
            SecurityIdentifier everyone = new SecurityIdentifier(
                WellKnownSidType.WorldSid, null);


            DiscretionaryAcl dacl = new DiscretionaryAcl(false, false, 1);

            // Disallow access from off machine
            dacl.AddAccess(AccessControlType.Deny, networkSid, -1,
                InheritanceFlags.None, PropagationFlags.None);

            // Allow acces only from local administrators and power users
            dacl.AddAccess(AccessControlType.Allow, localAdminSid, -1,
                InheritanceFlags.None, PropagationFlags.None);
            dacl.AddAccess(AccessControlType.Allow, powerUsersSid, -1,
                InheritanceFlags.None, PropagationFlags.None);

            //dacl.AddAccess(AccessControlType.Allow, everyone, -1,
            //    InheritanceFlags.None, PropagationFlags.None);


            CommonSecurityDescriptor securityDescriptor =
                new CommonSecurityDescriptor(false, false,
                        ControlFlags.GroupDefaulted |
                        ControlFlags.OwnerDefaulted |
                        ControlFlags.DiscretionaryAclPresent,
                        null, null, null, dacl);
            return securityDescriptor;
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
        private static System.Runtime.Remoting.Channels.IServerChannelSinkProvider CreateServerSinkChain()
        {
            IServerChannelSinkProvider serverSinkChain = (ServerChannelSinkProviderChain as ICloneable).Clone() as IServerChannelSinkProvider;
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
        /// <MetaDataID>{D3A5F3C0-9EA7-4F50-AD61-EBF8AAA2205E}</MetaDataID>
        public static string GetChannelUri(MarshalByRefObject marshalByRefObject)
        {
            if (marshalByRefObject == null)
                return null;
            System.Runtime.Remoting.Proxies.RealProxy RealProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(marshalByRefObject);
            if (RealProxy == null)
                return null;

            System.Runtime.Remoting.ObjRef ObjRef = null;
            if (RealProxy is Proxy)
                return (RealProxy as Proxy).ChannelUri;
            else
            {

                if (!RemotingServices.IsInRemoteMachine(marshalByRefObject as MarshalByRefObject))
                {
                    System.Runtime.Remoting.ObjRef remoteObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(marshalByRefObject as MarshalByRefObject);
                    if (remoteObjRef != null)
                    {
                        Object[] data = remoteObjRef.ChannelInfo.ChannelData;
                        int firstChannelIndex = -1;
                        ChannelDataStore tcpChannel = null;
                        ChannelDataStore ipcChannel = null;
                        int tcpChannelIndex = -1;
                        int ipcChannelIndex = -1;

                        for (int j = 0; j < data.Length; j++)
                        {
                            if (data[j] is ChannelDataStore)
                            {
                                if (firstChannelIndex == -1)
                                    firstChannelIndex = j;
                                ChannelDataStore channelDataStore = data[j] as ChannelDataStore;

                                if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"tcp:/") == 0)
                                {
                                    tcpChannelIndex = j;
                                    tcpChannel = channelDataStore;
                                }
                                if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"ipc") == 0)
                                {
                                    ipcChannelIndex = j;
                                    ipcChannel = channelDataStore;
                                }
                            }
                        }
                        if (ipcChannelIndex != -1 && tcpChannelIndex != -1 && tcpChannel != null && ipcChannel != null && ipcChannelIndex > tcpChannelIndex)
                        {
                            data[ipcChannelIndex] = tcpChannel;
                            data[tcpChannelIndex] = ipcChannel;

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
                            marshalByRefObject = System.Runtime.Remoting.RemotingServices.Unmarshal(remoteObjRef) as MarshalByRefObject;
                            #endregion

                        }


                        //Object[] data = remoteObjRef.ChannelInfo.ChannelData;
                        //int firstChannelIndex = -1;

                        //System.Runtime.Remoting.Channels.ChannelDataStore tcpChannel = null;
                        //System.Runtime.Remoting.Channels.ChannelDataStore ipcChannel = null;
                        //for (int j = 0; j < data.Length; j++)
                        //{
                        //    if (data[j] is System.Runtime.Remoting.Channels.ChannelDataStore)
                        //    {
                        //        if (firstChannelIndex == -1)
                        //            firstChannelIndex = j;
                        //        System.Runtime.Remoting.Channels.ChannelDataStore channelDataStore = data[j] as System.Runtime.Remoting.Channels.ChannelDataStore;
                        //        if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"tcp:/") == 0)
                        //            tcpChannel = channelDataStore;
                        //        if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"ipc") == 0)
                        //            ipcChannel = channelDataStore;
                        //    }
                        //}
                        //if (data[firstChannelIndex] != ipcChannel)
                        //{
                        //    if (ipcChannel != null)
                        //        data[firstChannelIndex] = ipcChannel;
                        //    if (tcpChannel != null)
                        //        data[firstChannelIndex + 1] = tcpChannel;

                        //    #region Rebuild channel sink
                        //    //Αυτός ο τρόpος που γίνεται rebuild η channel sink δεν είναι ασφαλές γιατί χρησιμοποιεί μη
                        //    //ντοκιουμενταρισμένες operations και class του .net framework

                        //    //Class:        System.Runtime.Remoting.IdentityHolder
                        //    //operation:    static void RemoveIdentity(string uri, bool bResetURI)

                        //    System.Type type = typeof(System.Runtime.Remoting.RemotingServices).Assembly.GetType("System.Runtime.Remoting.IdentityHolder");
                        //    foreach (System.Reflection.MethodInfo method in type.GetMethods(System.Reflection.BindingFlags.Public |
                        //                                    System.Reflection.BindingFlags.NonPublic |
                        //                                     System.Reflection.BindingFlags.Static |
                        //                                    System.Reflection.BindingFlags.DeclaredOnly))
                        //    {
                        //        if (method.Name == "RemoveIdentity" && method.GetParameters().Length == 2)
                        //        {
                        //            method.Invoke(null, new object[2] { remoteObjRef.URI, false });
                        //            break;
                        //        }
                        //    }
                        //    marshalByRefObject = System.Runtime.Remoting.RemotingServices.Unmarshal(remoteObjRef) as MarshalByRefObject;
                        //    #endregion
                        //}

                    }

                }
                else
                {
                    System.Runtime.Remoting.ObjRef remoteObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(marshalByRefObject as MarshalByRefObject);
                    if (remoteObjRef != null)
                    {
                        Object[] data = remoteObjRef.ChannelInfo.ChannelData;
                        int firstChannelIndex = -1;
                        ChannelDataStore tcpChannel = null;
                        ChannelDataStore ipcChannel = null;
                        int tcpChannelIndex = -1;
                        int ipcChannelIndex = -1;

                        for (int j = 0; j < data.Length; j++)
                        {
                            if (data[j] is ChannelDataStore)
                            {
                                if (firstChannelIndex == -1)
                                    firstChannelIndex = j;
                                ChannelDataStore channelDataStore = data[j] as ChannelDataStore;

                                if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"tcp:/") == 0)
                                {
                                    tcpChannelIndex = j;
                                    tcpChannel = channelDataStore;
                                }
                                if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"ipc") == 0)
                                {
                                    ipcChannelIndex = j;
                                    ipcChannel = channelDataStore;
                                }
                            }
                        }
                        if (ipcChannelIndex != -1 && tcpChannelIndex != -1 && tcpChannel != null && ipcChannel != null && ipcChannelIndex < tcpChannelIndex)
                        {
                            data[ipcChannelIndex] = tcpChannel;
                            data[tcpChannelIndex] = ipcChannel;
                            #region Rebuild channel sink
                            //Αυτός ο τρόπος που γίνεται rebuild η channel sink δεν είναι ασφαλές γιατί χρησιμοποιεί μη
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
                            marshalByRefObject = System.Runtime.Remoting.RemotingServices.Unmarshal(remoteObjRef) as MarshalByRefObject;
                            #endregion

                        }
                    }
                }



                return GetChannelUri(System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(marshalByRefObject));
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
                    ServerSessionPart session = GetServerSession(extObjectUri.SessionIDentity);
                    if (session != null)
                        obj = session.GetObject(extObjectUri);
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

        /// <MetaDataID>{e2718af6-5ab4-4b89-8a83-ab029034130a}</MetaDataID>
        [MetaDataRepository.BackwardCompatibilityID("+2")]
        public static T CreateInstance<T>(string ChannelUri, Type[] paramsTypes, params object[] ctorParams) where T : class
        {
            return CreateInstance(ChannelUri, typeof(T).FullName, typeof(T).Assembly.FullName, paramsTypes, ctorParams) as T;
        }

        /// <MetaDataID>{F0E86382-21D4-4504-A21A-FFBB6669E6A8}</MetaDataID>
        public static object CreateInstance(string ChannelUri, string TypeFullName, string assemblyData, Type[] paramsTypes, params object[] ctorParams)
        {
            return GetRemotingServices(ChannelUri).CreateInstance(TypeFullName, assemblyData, paramsTypes, ctorParams);
        }
        /// <MetaDataID>{13b7e930-c27e-416a-b112-43fb279d328b}</MetaDataID>
        [MetaDataRepository.BackwardCompatibilityID("+1")]
        public static T CreateInstance<T>(string ChannelUri) where T : class
        {
            return CreateInstance(ChannelUri, typeof(T).FullName, typeof(T).Assembly.FullName) as T;
        }
        /// <MetaDataID>{40046d38-37d4-4c73-9744-306030f5bb7e}</MetaDataID>
        public static object CreateInstance(string ChannelUri, string TypeFullName, string assemblyData)
        {
            try
            {
                return GetRemotingServices(ChannelUri).CreateInstance(TypeFullName, assemblyData);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }
        /// <MetaDataID>{28e684cc-160b-4307-9c51-5ec432620dcc}</MetaDataID>
        public virtual object CreateInstance(string TypeFullName, string assemblyData)
        {

            //typeof(RemotingException).TypeInitializer
            object NewInstance = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(TypeFullName, assemblyData));
            if (NewInstance != null && (NewInstance as System.MarshalByRefObject) == null)
                throw new Exception("The " + TypeFullName + " isn't type of System.MarshalByRefObject");
            return NewInstance;

        }
        /// <MetaDataID>{0FAF35C4-6303-4D08-9576-5CB6335F23CA}</MetaDataID>
        static private System.Collections.ArrayList ServerChannelURIs = new System.Collections.ArrayList();
        /// <MetaDataID>{46A1C16A-C717-450C-9189-F661EC96A282}</MetaDataID>
        public static void AddServerChannelUri(string ChannelUri)
        {
            ChannelUri = ChannelUri.ToLower();
            if (!ServerChannelURIs.Contains(ChannelUri))
                ServerChannelURIs.Add(ChannelUri);
        }
        /// <MetaDataID>{96074F87-E3AE-4E0A-9793-D892A56773DC}</MetaDataID>
        public static System.Collections.ArrayList GetServerChannelUris()
        {
            return ServerChannelURIs.Clone() as System.Collections.ArrayList;
        }







        /// <MetaDataID>{b30478c9-cafd-4ee6-8f44-cb091f3582fb}</MetaDataID>
        internal static ServerSessionPart GetServerSession(string channelUri, Guid sessionIdentity)
        {
            return GetRemotingServices(channelUri).GetServerSession(sessionIdentity);

        }
        /// <MetaDataID>{8c1f84a3-1b1e-44db-bac9-5c5ed95f4943}</MetaDataID>
        static OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();

        /// <MetaDataID>{92028471-8fc9-4422-af67-0a25bf194190}</MetaDataID>
        public ServerSessionPart GetServerSession(Guid clientProcessIdentity)
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

        /// <MetaDataID>{132fd33e-6641-4b52-8e94-9a5cdddac427}</MetaDataID>
        internal static IExtMarshalByRefObject ModifyChannelData(MarshalByRefObject marshalByRefObject)
        {
            if (!RemotingServices.IsInRemoteMachine(marshalByRefObject as MarshalByRefObject))
            {
                System.Runtime.Remoting.ObjRef remoteObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(marshalByRefObject as MarshalByRefObject);
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
                    // if (data[firstChannelIndex] != ipcChannel)
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
                        marshalByRefObject = System.Runtime.Remoting.RemotingServices.Unmarshal(remoteObjRef) as MarshalByRefObject;
                        return marshalByRefObject as IExtMarshalByRefObject;
                        #endregion
                    }

                }

            }
            return marshalByRefObject as IExtMarshalByRefObject;

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
                    proxy.EventConsumerSubscription(eventSubscrioption.Handler, eventSubscrioption.EventInfo, out proceedToSessionSubscription, out allowAsynchronous);
                    if (proceedToSessionSubscription)
                    {
                        RemoteEventSubscription remoteEventSubscription = new RemoteEventSubscription();
                        remoteEventSubscription.AllowAsynchronous = allowAsynchronous;
                        remoteEventSubscription.eventInfo = eventSubscrioption.EventInfo;
                        remoteEventSubscription.ExtObjectUri = proxy.URI;
                        channelSubscriptions.Add(remoteEventSubscription);
                    }
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<RemoteEventSubscription>> entry in sessionSubscriptions)
                RenewalManager.GetSession(entry.Key).Subscribe(entry.Value);
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

                    proxy.EventConsumerUnsubscribe(eventSubscrioption.Handler, eventSubscrioption.EventInfo, out proceedToSessionUnsubscribe);
                    if (proceedToSessionUnsubscribe)
                    {
                        RemoteEventSubscription remoteEventSubscription = new RemoteEventSubscription();
                        remoteEventSubscription.eventInfo = eventSubscrioption.EventInfo;
                        remoteEventSubscription.ExtObjectUri = proxy.URI;
                        channelSubscriptions.Add(remoteEventSubscription);
                    }
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<RemoteEventSubscription>> entry in sessionSubscriptions)
                RenewalManager.GetSession(entry.Key).Unsubscribe(entry.Value);
        }
    }
}
