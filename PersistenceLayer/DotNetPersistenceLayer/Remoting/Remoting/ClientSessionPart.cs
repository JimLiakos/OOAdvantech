namespace OOAdvantech.Remoting
{
    using System;
    using System.Threading.Tasks;

#if !DeviceDotNet
    using System.Net.Sockets;
    using System.Runtime.Remoting;

#endif




    /// <MetaDataID>{0187304e-4f4b-446e-8338-1dfd4af6ed87}</MetaDataID>
    public class ClientSessionPart : MarshalByRefObject, IWeakRefernceEventHandler, IRemoteEventHandler
    {

        /// <MetaDataID>{1ec1e747-bf77-496f-ab0a-b73c8d61d5b9}</MetaDataID>
        [MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        [MetaDataRepository.RoleAMultiplicityRange(1, 1)]
        [MetaDataRepository.Association("", typeof(ServerSessionPart), MetaDataRepository.Roles.RoleA, "89837d73-64ac-4009-9217-8f561b66020a")]
        [MetaDataRepository.IgnoreErrorCheck]
        internal OOAdvantech.Remoting.IServerSessionPart ServerSessionPart;

        /// <MetaDataID>{e6f17026-2e3c-40d9-9947-2778391f68c8}</MetaDataID>
        [MetaDataRepository.Association("", typeof(IProxy), MetaDataRepository.Roles.RoleA, "aeddc231-2938-4c8d-b623-30b7e36baa4c")]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.IgnoreErrorCheck]
        protected System.Collections.Generic.List<string> CollectedProxies = new System.Collections.Generic.List<string>();

        /// <MetaDataID>{850fa0a3-e4a4-401c-be93-fde29956817f}</MetaDataID>
        [MetaDataRepository.Association("", typeof(IProxy), MetaDataRepository.Roles.RoleA, "59857e21-c10a-4f88-8376-b0c3b86264d2")]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.IgnoreErrorCheck]
        protected System.Collections.Generic.Dictionary<string, System.WeakReference> JustCreatedProxies = new System.Collections.Generic.Dictionary<string, System.WeakReference>();

        /// <MetaDataID>{76a47b4e-b584-4ad9-ad66-3b469058afe1}</MetaDataID>
        [MetaDataRepository.Association("SessionProxies", typeof(IProxy), MetaDataRepository.Roles.RoleA, "72d444ac-2dfa-4f93-b43d-e22e3116a58f")]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(1)]
        [MetaDataRepository.IgnoreErrorCheck]
        protected System.Collections.Generic.Dictionary<string, System.WeakReference> Proxies = new System.Collections.Generic.Dictionary<string, System.WeakReference>();



        /// <MetaDataID>{77c5cd09-c91d-4a87-93ef-803d35098ce8}</MetaDataID>
        public virtual bool AllowedBidirectionalCall
        {
            get
            {
                return false;
            }
        }



        /// <MetaDataID>{f19ece80-eb77-4b03-acea-13c401612ecc}</MetaDataID>
        public virtual void Unsubscribe(IProxy proxy, EventInfoData eventInfo)
        {
            ServerSessionPart.Unsubscribe(proxy.ObjectUri, eventInfo);
        }



        /// <MetaDataID>{2be9b0e6-8a07-4447-9985-c6c46b0a7f3c}</MetaDataID>
        public virtual void Subscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions)
        {
            bool allowAsynchronous = true;
            foreach (RemoteEventSubscription eventSubscription in eventSubscriptions)
            {
                if (!eventSubscription.AllowAsynchronous)
                {
                    allowAsynchronous = false;
                    break;
                }
            }
            try
            {
                //bool sychronizeSession = false;
                //lock (this)
                //{
                //    sychronizeSession = JustCreatedProxies.ContainsKey(proxy.URI.TransientUri);
                //}
                //if (sychronizeSession)
                SynchronizeSession().Wait();

                if (Remoting.RemotingServices.HasHostProcessNetworkAccess)
                    ServerSessionPart.Subscribe(eventSubscriptions, this);
                else if (AllowedBidirectionalCall)
                    ServerSessionPart.Subscribe(eventSubscriptions);
                else
                {
                    if (allowAsynchronous)
                    {
                        ServerSessionPart.Subscribe(eventSubscriptions);
                    }
#if !DeviceDotNet
                    else
                    {

                        lock (this)
                        {

                            if (RemoteEventHandler == null)
                                RemoteEventHandler = Remoting.RemotingServices.CreateRemoteInstance("tcp://localhost:9060", typeof(RemoteEventHandler).FullName, "", new System.Type[1] { typeof(IRemoteEventHandler) }, new object[1] { this }) as IRemoteEventHandler;
                        }
                        ServerSessionPart.Subscribe(eventSubscriptions, RemoteEventHandler);
                    }
#else
                    else
                        throw new NotSupportedException();
#endif

                }
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{0387aad3-a419-4227-b50f-131be1dda44d}</MetaDataID>
        public virtual void Unsubscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions)
        {

            try
            {

                ServerSessionPart.Unsubscribe(eventSubscriptions);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }




        /// <MetaDataID>{33f4bd09-67f6-4a90-b559-11b495ab56ff}</MetaDataID>
        IRemoteEventHandler RemoteEventHandler;
        /// <MetaDataID>{2ff2c01d-ddfc-4d1e-a0bc-6cd8829b1a90}</MetaDataID>
        public virtual void Subscribe(IProxy proxy, EventInfoData eventInfoData, bool allowAsynchronous)
        {
            try
            {

                bool sychronizeSession = false;
                lock (this)
                {
                    sychronizeSession = JustCreatedProxies.ContainsKey(proxy.ObjectUri.TransientUri);
                }
                if (sychronizeSession)
                    SynchronizeSession().Wait();

                if (Remoting.RemotingServices.HasHostProcessNetworkAccess)
                    ServerSessionPart.Subscribe(proxy.ObjectUri, eventInfoData, this);
                else if (AllowedBidirectionalCall)
                    ServerSessionPart.Subscribe(proxy.ObjectUri, eventInfoData);
                else
                {
                    if (allowAsynchronous)
                    {
                        ServerSessionPart.Subscribe(proxy.ObjectUri, eventInfoData);
                    }
#if !DeviceDotNet
                    else
                    {

                        lock (this)
                        {
                            if (RemoteEventHandler == null)
                                RemoteEventHandler = Remoting.RemotingServices.CreateRemoteInstance("tcp://localhost:9060", typeof(RemoteEventHandler).FullName, "", new System.Type[1] { typeof(IRemoteEventHandler) }, new object[1] { this }) as IRemoteEventHandler;
                        }
                        ServerSessionPart.Subscribe(proxy.ObjectUri, eventInfoData, RemoteEventHandler);
                    }
#else
                    else
                        throw new NotSupportedException();
#endif

                }
            }
            catch (System.Exception error)
            {
                throw;
            }

        }

#if !DeviceDotNet

        /// <MetaDataID>{c51be643-1bed-498d-961c-682365ee7e63}</MetaDataID>
        internal void PublishSessionEvents()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>> entry in ServerSessionPart.GetPendingEvents())
            {
                foreach (System.Collections.Generic.KeyValuePair<EventInfoData, System.Collections.Generic.List<object>> eventEntry in entry.Value)
                    EventCallback(entry.Key, eventEntry.Key.EventInfo, eventEntry.Value);
            }
        }
#endif
        /// <MetaDataID>{23129494-3e76-49a3-9786-e4e25a34faca}</MetaDataID>
        System.DateTime CretionTimeStamp = System.DateTime.Now;

        /// <MetaDataID>{ac7cc006-971e-4cc4-a7ca-9a1b6f60ad08}</MetaDataID>
        bool _SuspendSessionRemove;
        /// <MetaDataID>{235ed42c-40b8-4498-8cfc-d0b9a5d7ee1a}</MetaDataID>
        internal bool SuspendSessionRemove
        {
            get
            {
                if (!_SuspendSessionRemove)
                {
                    if ((System.DateTime.Now - CretionTimeStamp).Minutes < 2)
                        return true;
                }
                return _SuspendSessionRemove;
            }
            set
            {

                _SuspendSessionRemove = value;
            }
        }



        /// <MetaDataID>{5a5c77df-f86b-49da-88ec-00b33f780c43}</MetaDataID>
        internal bool HasNoProxies
        {
            get
            {

                if (CollectedProxies.Count == 0 && Proxies.Count == 0 && JustCreatedProxies.Count == 0)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{ed0fec0e-b473-4110-8e8a-b851a317bfe1}</MetaDataID>
        public string X_Auth_Token { get; internal set; }
        /// <MetaDataID>{e733c4a2-6c5c-4699-b17a-9e098fbd5b4a}</MetaDataID>
        public string X_Access_Token { get; internal set; }



        /// <MetaDataID>{9cc70b38-23df-4fe6-9043-b3f8f9689a5b}</MetaDataID>
        ~ClientSessionPart()
        {

        }

        /// <MetaDataID>{05645191-0240-4d0e-b1e6-1722196381fe}</MetaDataID>
        protected IRemotingServices RemotingServices;
        ///// <MetaDataID>{cf7efc78-6234-44f2-999c-28feeb8db034}</MetaDataID>
        //Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();
#if !DeviceDotNet
        /// <MetaDataID>{f5bdfa16-11b3-484b-ac88-6a15274b1fcb}</MetaDataID>
        public readonly System.Security.Principal.WindowsIdentity WindowsIdentity;
#endif
        /// <MetaDataID>{fa7f4df3-f13b-42a7-8efa-df5b9157755c}</MetaDataID>
        internal string ChannelUri;
        /// <MetaDataID>{d38f857b-1992-4aef-8185-3230228c019c}</MetaDataID>
        internal string SessionIdentity
        {
            get
            {
                return _SessionIdentity;
            }
        }
        /// <MetaDataID>{7dbc8940-de28-4aa3-88a4-d822983a99e3}</MetaDataID>
        protected string _SessionIdentity;


        /// <MetaDataID>{5fa27d40-31de-48f9-a94d-80163f7839cd}</MetaDataID>
        internal Guid ClientProcessIdentity;
        /// <MetaDataID>{38ca89f4-0cad-42f4-9066-ca6a2dd84518}</MetaDataID>
        ClientSessionPart(string channelUri, IRemotingServices remoteServices)
        {
            RemotingServices = remoteServices;
#if !DeviceDotNet
            WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
#endif
            ChannelUri = channelUri;
        }
        /// <MetaDataID>{6a552326-7d5f-43e2-b094-af9a70e34582}</MetaDataID>
        public void AttatchToServerSessionPart()
        {
            var serverSessionPartInfo = RemotingServices.GetServerSession(ChannelUri, Remoting.RemotingServices.ProcessIdentity);
            ServerSessionPart = serverSessionPartInfo.ServerSessionPart;
            ServerProcessIdentity = serverSessionPartInfo.ServerProcessIdentity;
            ClientProcessIdentity = Remoting.RemotingServices.ProcessIdentity;
            _SessionIdentity = Remoting.RemotingServices.ProcessIdentity.ToString("N") + "." + ServerProcessIdentity.ToString("N");
            new WeakReferenceEventPublisher(this, 5000);
#if !DeviceDotNet
            System.AppDomain.CurrentDomain.ProcessExit += new System.EventHandler(OnProcessExit);
#endif
        }





        /// <MetaDataID>{655625d4-85a9-402f-b43b-bbc06f430787}</MetaDataID>
        public ClientSessionPart(string channelUri, Guid clientProcessIdentity, ServerSessionPartInfo serverSessionPartInfo, IRemotingServices remotingServices)
        {
            Guid serverProcessIdentity = serverSessionPartInfo.ServerProcessIdentity;
            IServerSessionPart serverSessionPart = serverSessionPartInfo.ServerSessionPart;


            ChannelUri = channelUri;


            ClientProcessIdentity = clientProcessIdentity;
            ServerProcessIdentity = serverProcessIdentity;
            ServerSessionPart = serverSessionPart;
            RemotingServices = remotingServices;

            _SessionIdentity = clientProcessIdentity.ToString("N") + "." + ServerProcessIdentity.ToString("N");
            new WeakReferenceEventPublisher(this, 5000);
#if !DeviceDotNet
            WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.AppDomain.CurrentDomain.ProcessExit += new System.EventHandler(OnProcessExit);
#endif

        }

        /// <MetaDataID>{0ba947f1-138c-4fbf-afaf-49c85f6e6fcc}</MetaDataID>
        protected virtual void OnProcessExit(object sender, System.EventArgs e)
        {
            ServerSessionPart.ClientProcessTerminates();

        }
        /// <MetaDataID>{cba2ffe0-22a6-41ad-9024-6e9e7f97c4e5}</MetaDataID>
        public System.Guid ServerProcessIdentity;
        /// <MetaDataID>{2d1c8152-e360-41e0-baf9-39b2c81009cc}</MetaDataID>
        internal IProxy GetProxy(string objectUri)
        {

            System.WeakReference weakReference = null;
            lock (Proxies)
            {

                if (Proxies.TryGetValue(objectUri, out weakReference))
                    return weakReference.Target as IProxy;
                else
                    return null;
            }

        }

        /// <MetaDataID>{71ea5b49-0d6d-489d-8801-e3aba1a65004}</MetaDataID>
        internal void AddProxy(IProxy proxy)
        {

            lock (this)
            {
                if (Proxies.ContainsKey(proxy.ObjectUri.TransientUri))
                {
                    IProxy oldProxy = Proxies[proxy.ObjectUri.TransientUri].Target as IProxy;
                    if (oldProxy != null && oldProxy != proxy)
                        throw new System.Exception("Alread exist proxy for uri : " + proxy.ObjectUri);
                    if (oldProxy == null)
                    {
                        ServerProcessTerminate = false;
                        WeakReference proxyReference = new System.WeakReference(proxy);
                        JustCreatedProxies[proxy.ObjectUri.TransientUri] = proxyReference;
                        Proxies[proxy.ObjectUri.TransientUri] = proxyReference;

                    }
                }
                else
                {
                    ServerProcessTerminate = false;
                    WeakReference proxyReference = new System.WeakReference(proxy);
                    Proxies.Add(proxy.ObjectUri.TransientUri, proxyReference);
                    JustCreatedProxies.Add(proxy.ObjectUri.TransientUri, proxyReference);

                }
            }

        }
        /// <MetaDataID>{32d37e9b-6593-4fce-be7d-c2463fc0c479}</MetaDataID>
        internal void ProxyFinalized(IProxy proxy)
        {

            lock (this)
            {
                if (Proxies.ContainsKey(proxy.ObjectUri.TransientUri))
                    Proxies.Remove(proxy.ObjectUri.TransientUri);
                if (JustCreatedProxies.ContainsKey(proxy.ObjectUri.TransientUri))
                    JustCreatedProxies.Remove(proxy.ObjectUri.TransientUri);
                CollectedProxies.Add(proxy.ObjectUri.TransientUri);
            }

        }
        /// <MetaDataID>{29d61127-a58d-44af-b5e4-4fe7cb3f768e}</MetaDataID>
        bool OnUpdateServerSessionPart;
        /// <MetaDataID>{ea585c32-017c-4df8-bbd6-cc36d4dcb664}</MetaDataID>
        protected virtual Task SynchronizeSession()
        {
            return Task.Factory.StartNew(() =>
              {
                  System.Collections.Generic.List<ExtObjectUri> justCreatedProxies = new System.Collections.Generic.List<ExtObjectUri>();
                  System.Collections.Generic.List<string> collectedProxies = new System.Collections.Generic.List<string>();


                  lock (JustCreatedProxies)
                  {
                      foreach (System.Collections.Generic.KeyValuePair<string, System.WeakReference> entry in JustCreatedProxies)
                      {
                          IProxy proxy = entry.Value.Target as IProxy;
                          if (proxy != null)
                              justCreatedProxies.Add(proxy.ObjectUri);
                      }
                      collectedProxies = new System.Collections.Generic.List<string>(CollectedProxies);

                  }

                  System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<System.Reflection.EventInfo, System.Collections.Generic.List<object>>> pendingEvents = null;
                  try
                  {
                      if (!ServerProcessTerminate && ServerSessionPart != null && (justCreatedProxies.Count != 0 || collectedProxies.Count != 0 || Proxies.Count != 0))
                      {
                          lock (this)
                          {
                              if (OnUpdateServerSessionPart)
                                  return;
                              OnUpdateServerSessionPart = true;
                          }
                          try
                          {
                            //ServerSessionPart.Update(ref jastCreatedProxies, collectedProxies, out pendingEvents);
                            //System.Runtime.Remoting.Lifetime.ILease lease = GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                            //if (lease != null)
                            //    lease.Renew(System.TimeSpan.FromMinutes(5.5));
                            //lease = ServerSessionPart.GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                            //if (lease != null)
                            //    lease.Renew(System.TimeSpan.FromMinutes(5.5));
                        }
                          catch (Exception error)
                          {

                              throw;
                          }
                          finally
                          {
                              OnUpdateServerSessionPart = false;
                          }


                      }

                  }
                  catch (System.Exception error)
                  {
                      System.GC.Collect();
                      try
                      {
                          ServerSessionPart = this.RemotingServices.GetServerSession(ChannelUri, Remoting.RemotingServices.ProcessIdentity).ServerSessionPart;

                      }
                      catch (System.Exception newError)
                      {

                      }

#if !DeviceDotNet
                    if (error is SocketException &&
                        ((error as SocketException).SocketErrorCode == SocketError.ConnectionReset || (error as SocketException).SocketErrorCode == SocketError.ConnectionRefused))
                      {
                          ServerProcessTerminates();
                        //TODO έχει χαθεί η επικοινωνία ή πρέπει να κλείσω το session ή να επανασυνδεθώ
                    }
#endif

                    if (error.Message.IndexOf("Failed to connect to an IPC Port") == 0)
                      {
                          try
                          {
                              string chaneluri = Remoting.RemotingServices.GetChannelUri(ServerSessionPart);
                              ServerProcessTerminates();
                          }
                          catch (Exception innerError)
                          {
                          }
                      }

                  }
                  lock (JustCreatedProxies)
                  {
                      foreach (string objectUri in collectedProxies)
                          CollectedProxies.Remove(objectUri);
                      foreach (ExtObjectUri extObjectUri in justCreatedProxies)
                      {
                          if (extObjectUri.Disconnected)
                          {
                              if (Proxies.ContainsKey(extObjectUri.TransientUri))
                                  Proxies.Remove(extObjectUri.TransientUri);
                              if (CollectedProxies.Contains(extObjectUri.TransientUri))
                                  CollectedProxies.Remove(extObjectUri.TransientUri);
                          }
                          JustCreatedProxies.Remove(extObjectUri.TransientUri);
                      }
                  }

                  if (pendingEvents != null)
                  {
                      foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<System.Reflection.EventInfo, System.Collections.Generic.List<object>>> entry in pendingEvents)
                      {
                          foreach (System.Collections.Generic.KeyValuePair<System.Reflection.EventInfo, System.Collections.Generic.List<object>> eventEntry in entry.Value)
                              EventCallback(entry.Key, eventEntry.Key, eventEntry.Value);
                      }
                  }
              });
        }

        /// <MetaDataID>{64dfb36c-b016-4380-988a-1cd796725a01}</MetaDataID>
        System.DateTime LastEventTime = System.DateTime.Now;
        /// <MetaDataID>{edd8144b-2c41-4c63-a48a-f43d1f2da82a}</MetaDataID>
        void IWeakRefernceEventHandler.OnTimeTick()
        {

            System.TimeSpan timeSpan = System.DateTime.Now - LastEventTime;
            if (timeSpan.TotalMilliseconds > 4000)
            {
                LastEventTime = System.DateTime.Now;

                if (!SynchronizeSession().Wait(3500))
                {

                }
            }

        }






        /// <MetaDataID>{eaf80ba5-36a7-4fb7-9aab-2576fe079690}</MetaDataID>
        public void EventCallback(string objectUri, System.Reflection.EventInfo eventInfo, System.Collections.Generic.List<object> args)
        {
            IProxy proxy = null;
            lock (this)
            {
                if (Proxies.ContainsKey(objectUri))
                {
                    proxy = Proxies[objectUri].Target as IProxy;
                }
            }
            if (proxy != null)
                proxy.EventConsumingResolver.PublishEvent(eventInfo, args);

        }



        /// <MetaDataID>{4a817fa2-850a-4661-ba86-51194a573c27}</MetaDataID>
        bool ServerProcessTerminate;
        /// <MetaDataID>{3e63a3a0-a394-4319-8cab-9cedc311fd00}</MetaDataID>
        internal void ServerProcessTerminates()
        {
            if (ServerProcessTerminate)
                return;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            ServerProcessTerminate = true;

#if !DeviceDotNet
            System.AppDomain.CurrentDomain.ProcessExit -= new System.EventHandler(OnProcessExit);
#endif

            CollectedProxies.Clear();


            ServerSessionPart serverSessionPart = null;
            System.WeakReference serverSessionPartReference = null;
            if (Remoting.RemotingServices.Sessions.TryGetValue(ServerProcessIdentity, out serverSessionPartReference))
            {
                serverSessionPart = serverSessionPartReference.Target as ServerSessionPart;
                if (serverSessionPart != null)
                    serverSessionPart.ClientProcessTerminates();
            }

        }



        /// <MetaDataID>{16051bf1-9c82-4248-b75a-d870b752d4db}</MetaDataID>
        public void ThereArePendingEvents(int pendingEventsHandlingCode)
        {
#if !DeviceDotNet
            PublishSessionEvents();
#endif
        }




    }

    /// <MetaDataID>{2c1be763-7627-4c54-9bac-5bca234f8146}</MetaDataID>
    interface IWeakRefernceEventHandler
    {
        /// <MetaDataID>{05bb6b0c-4b3d-403a-914e-8566ba0faa73}</MetaDataID>
        void OnTimeTick();
    }
    /// <MetaDataID>{f73b5ccb-f449-4aea-ba3d-53dc7db82b4b}</MetaDataID>
    class WeakReferenceEventPublisher
    {

        /// <MetaDataID>{96f075ef-392e-4f59-aaa8-a8ac677dc93f}</MetaDataID>
        System.WeakReference WeakRefernceEventHandler;

#if !DeviceDotNet
        /// <MetaDataID>{e18ba3d1-ec72-4dfa-a78a-38c910888bfa}</MetaDataID>
        System.Timers.Timer Timer;
#else

        System.Timers.Timer Timer;
#endif

        /// <MetaDataID>{9026519d-b30f-4ef1-81f1-4cff51d7eb8d}</MetaDataID>
        public WeakReferenceEventPublisher(IWeakRefernceEventHandler weakEventRefernceHandler, double interval)
        {
            WeakRefernceEventHandler = new System.WeakReference(weakEventRefernceHandler);

#if !DeviceDotNet
            Timer = new System.Timers.Timer(interval);
            Timer.Enabled = true;
            Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);

#else
            Timer = new System.Timers.Timer(new System.Timers.TimerCallback(OnTimer), null, TimeSpan.FromMilliseconds(interval));
            //Timer.Start();

#endif
        }

#if !DeviceDotNet
        /// <MetaDataID>{afa49235-aa24-45bc-8a62-d7a4b846180e}</MetaDataID>
        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Timer.Enabled = false;
                IWeakRefernceEventHandler weakEventRefernceHandler = WeakRefernceEventHandler.Target as IWeakRefernceEventHandler;
                if (weakEventRefernceHandler != null)
                {
                    weakEventRefernceHandler.OnTimeTick();
                    Timer.Enabled = true;
                }
                else
                {
                    Timer.Enabled = false;
                    Timer.Elapsed -= new System.Timers.ElapsedEventHandler(Timer_Elapsed);

                }
            }
            catch
            {
                Timer.Enabled = true;
            }
        }
#else
        void OnTimer(object state)
        {
            IWeakRefernceEventHandler weakEventRefernceHandler = WeakRefernceEventHandler.Target as IWeakRefernceEventHandler;
            if (weakEventRefernceHandler != null)
                weakEventRefernceHandler.OnTimeTick();
            else
            {
                Timer.Stop();
            }
        }
#endif

    }
}

