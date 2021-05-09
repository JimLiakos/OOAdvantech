namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{0187304e-4f4b-446e-8338-1dfd4af6ed87}</MetaDataID>
    public class ClientSessionPart : IWeakRefernceEventHandler
    {
        [MetaDataRepository.Association("", typeof(Proxy), MetaDataRepository.Roles.RoleA, "aeddc231-2938-4c8d-b623-30b7e36baa4c")]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.IgnoreErrorCheck]
        protected System.Collections.Generic.List<string> CollectedProxies = new System.Collections.Generic.List<string>();

        [MetaDataRepository.Association("", typeof(Proxy), MetaDataRepository.Roles.RoleA, "59857e21-c10a-4f88-8376-b0c3b86264d2")]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.IgnoreErrorCheck]
        protected System.Collections.Generic.Dictionary<string, System.WeakReference> JustCreatedProxies=new System.Collections.Generic.Dictionary<string,System.WeakReference>();

        [MetaDataRepository.Association("SessionProxies", typeof(Proxy), MetaDataRepository.Roles.RoleA, "72d444ac-2dfa-4f93-b43d-e22e3116a58f")]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(1)]
        [MetaDataRepository.IgnoreErrorCheck]
        protected System.Collections.Generic.Dictionary<string, System.WeakReference> Proxies = new System.Collections.Generic.Dictionary<string, System.WeakReference>();


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
        ~ClientSessionPart()
        {

        }


        Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();

        System.Guid SessionIdentity=new System.Guid();
        ServerSessionPart ServerSessionPart;
        public ClientSessionPart(string channelUri)
        {
            ServerSessionPart=Remoting.RemotingServices.GetServerSession(channelUri, SessionIdentity);
            new WeakReferenceEventPublisher(this, 5000);
        }
        internal Proxy GetProxy(string objectUri)
        {

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {

                if (Proxies.ContainsKey(objectUri))
                    return Proxies[objectUri].Target as Proxy;
                else
                    return null;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }

        internal void AddProxy(Proxy proxy)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (Proxies.ContainsKey(proxy.URI))
                {
                    Proxy oldProxy = Proxies[proxy.URI].Target as Proxy;
                    if (oldProxy != null && oldProxy != proxy)
                        throw new System.Exception("Alread exist proxy for uri : " + proxy.URI);
                    if (oldProxy == null)
                    {
                        Proxies.Add(proxy.URI, new System.WeakReference(proxy));
                        JustCreatedProxies.Add(proxy.URI, new System.WeakReference(proxy));
                    }
                }
                else
                {
                    Proxies.Add(proxy.URI, new System.WeakReference(proxy));
                    JustCreatedProxies.Add(proxy.URI, new System.WeakReference(proxy));

                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        internal void ProxyFinalized(Proxy proxy)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (Proxies.ContainsKey(proxy.URI))
                    Proxies.Remove(proxy.URI);
                if (JustCreatedProxies.ContainsKey(proxy.URI))
                    JustCreatedProxies.Remove(proxy.URI);
                CollectedProxies.Add(proxy.URI);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

            

        }
        void SynchronizeSesion()
        {
            System.Collections.Generic.List<ExtObjectUri> jastCreatedProxies = new System.Collections.Generic.List<ExtObjectUri>();
            System.Collections.Generic.List<string> collectedProxies = new System.Collections.Generic.List<string>();


            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.WeakReference> entry in JustCreatedProxies)
                {
                    Proxy proxy = entry.Value.Target as Proxy;
                    if (proxy != null)
                        jastCreatedProxies.Add(new ExtObjectUri(entry.Key,true));
                }
                collectedProxies = new System.Collections.Generic.List<string>(CollectedProxies);

            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            ServerSessionPart.Update(ref jastCreatedProxies, collectedProxies);

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                foreach (string objectUri in collectedProxies)
                    CollectedProxies.Remove(objectUri);
                foreach (ExtObjectUri extObjectUri in jastCreatedProxies)
                {
                    if (!extObjectUri.Disconnected&& JustCreatedProxies.ContainsKey(extObjectUri.TransientUri))
                    {
                        Proxy proxy = JustCreatedProxies[extObjectUri.TransientUri].Target as Proxy;
                        if(proxy!=null)
                            proxy.PersistentUri = extObjectUri.PersistentUri; 

                    }
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
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }




        }

        System.DateTime lastEventTime = System.DateTime.Now;
        void IWeakRefernceEventHandler.OnTimeTick()
        {
            System.TimeSpan timeSpan = System.DateTime.Now - lastEventTime;

            if (timeSpan.TotalMilliseconds > 4000)
                SynchronizeSesion();
            
        }



  

    }

    /// <MetaDataID>{2c1be763-7627-4c54-9bac-5bca234f8146}</MetaDataID>
    interface IWeakRefernceEventHandler
    {
        void OnTimeTick();
    }
    /// <MetaDataID>{f73b5ccb-f449-4aea-ba3d-53dc7db82b4b}</MetaDataID>
    class WeakReferenceEventPublisher
    {

        System.WeakReference WeakRefernceEventHandler;
        System.Timers.Timer Timer;


        public WeakReferenceEventPublisher(IWeakRefernceEventHandler weakEventRefernceHandler,double interval)
        {
            WeakRefernceEventHandler = new System.WeakReference(weakEventRefernceHandler);
            Timer = new System.Timers.Timer(interval);
            Timer.Enabled = true;
            Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);


        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IWeakRefernceEventHandler weakEventRefernceHandler = WeakRefernceEventHandler.Target as IWeakRefernceEventHandler;
            if (weakEventRefernceHandler != null)
                weakEventRefernceHandler.OnTimeTick();
            else
            {
                Timer.Enabled = false;
                Timer.Elapsed -= new System.Timers.ElapsedEventHandler(Timer_Elapsed);

            }
        }

    }
}
