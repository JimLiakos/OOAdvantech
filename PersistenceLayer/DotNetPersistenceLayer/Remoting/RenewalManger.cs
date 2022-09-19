using System;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{0E57BDE7-C647-4167-90A6-D0385F999EEA}</MetaDataID>
    /// <summary>The .Net Remoting system manage object lifetime with leasing. For more information you can see the "Microsoft .NET Remoting: A Technical Overview" article.
    /// The main problem with this method is that the time expired if client didn't call the object. The .Net Remoting provides the sponsorship technique but produce traffic in network from many inter process calls for renew. 
    /// The job of renewal manager is make massive renew. This means one inter process call for all remote objects per channel.  The time interval between two massive renew is the half time of lesser time from the times RenewOnCallTime CurrentLeaseTime InitialLeaseTime.
    /// </summary>
    internal class RenewalManager
    {
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.Association("", typeof(ClientSessionPart), MetaDataRepository.Roles.RoleA, "ebfbc14b-cd03-433c-ab3a-33dfe20024c3")]
        public static System.Collections.Generic.Dictionary<string, ClientSessionPart> Sessions = new System.Collections.Generic.Dictionary<string, ClientSessionPart>();


        static object SessionsLock = new object();

        static System.Collections.Generic.Dictionary<string, Task<ClientSessionPart>> SessionsTasks = new System.Collections.Generic.Dictionary<string, Task<ClientSessionPart>>();


        public static System.Collections.Generic.Dictionary<string, ClientSessionPart> GetSessions()
        {
            lock (SessionsLock)
                return new System.Collections.Generic.Dictionary<string, ClientSessionPart>(Sessions);
        }


#if DeviceDotNet
        private static System.Timers.Timer timer = new System.Timers.Timer(new System.Timers.TimerCallback(SessionsCheck), null, TimeSpan.FromSeconds(4));
#else
        /// <MetaDataID>{C600BEFA-66D4-4DE7-86F4-913156CD4EAC}</MetaDataID>
        private static System.Threading.Timer timer = new System.Threading.Timer(new System.Threading.TimerCallback(SessionsCheck), null, 4000, 4000);

#endif

        /// <MetaDataID>{5BC03B60-12E8-4E22-88CD-D6D741F43538}</MetaDataID>
        /// <summary>This member keeps a dictionary with weak reference on proxies that controls the renewal manager. Weak reference allow garbage collector, collect references object. </summary>
        //static private System.Collections.Hashtable WeakReferenceProxies=new System.Collections.Hashtable(200);
        ///// <MetaDataID>{CAC0347A-C0A1-4388-ABEF-25FB195E28EE}</MetaDataID>
        ///// <summary>Defines the collection of uri's which failed to renew. We use to avoid the memory allocation and free for each massive renew. Has value only in renew method call.</summary>
        //static private System.Collections.ArrayList RemovedWeakReference=new System.Collections.ArrayList(200);

        /// <summary>This method remove the proxy from renewal manager control.</summary>
        /// <param name="Uri">Parameter defines the uri of attached object of proxy, which will be removed.</param>
        /// <MetaDataID>{C75387C9-BC3F-4548-A998-DC6DBD84C56C}</MetaDataID>
        public static void RemoveProxy(IProxy proxy)
        {
            ClientSessionPart clientSessionPart = null;
            string channelUri = proxy.ChannelUri;


            lock (SessionsLock)
            {
                Sessions.TryGetValue(channelUri, out clientSessionPart);
            }

            if (clientSessionPart != null)
                clientSessionPart.ProxyFinalized(proxy);

        }

        /// <summary>Return the proxy object with uri of parameter. </summary>
        /// <param name="Uri">Parameter defines an uri of object. The proxy of this object will be returned from method if exist.</param>
        /// <MetaDataID>{96B4B249-8C8B-4E52-BC5A-5E36E861279B}</MetaDataID>
        public static IProxy GetProxy(string channelUri, string objectUri)
        {
            ClientSessionPart clientSession = null;
            lock (SessionsLock)
            {
                Sessions.TryGetValue(channelUri, out clientSession);
            }
            if (clientSession == null)
                return null;
            else
                return clientSession.GetProxy(objectUri);



            //if(!System.Threading.Monitor.TryEnter(WeakReferenceProxies,20000))
            //    System.Diagnostics.Debug.WriteLine("DeadLock");
            //try
            //{
            //    if(WeakReferenceProxies.Contains(Uri))
            //    {
            //        WeakReference proxyReference=WeakReferenceProxies[Uri] as WeakReference;
            //        if(proxyReference.IsAlive)
            //            return proxyReference.Target as Proxy;
            //    }
            //}
            //finally
            //{
            //    System.Threading.Monitor.Exit(WeakReferenceProxies);
            //}

            return null;
        }

        /// <summary>This method add a proxy of remote object, which the renewal manager will include in the next massive renew.</summary>
        /// <param name="proxy">Parameter defines the proxy object for remote object. The proxy must be attached with an object. If doesn't the renewal manager raise an exception.If there is another proxy attached with the same object then the old proxy removed from the renewal manager control and the new proxy added.</param>
        /// <MetaDataID>{8D260CEE-5104-41C7-9B1C-6414B9C0962E}</MetaDataID>
        public static void AddProxy(IProxy proxy, IRemotingServices remotingServices)
        {
            string channelUri = proxy.ChannelUri;
            ClientSessionPart clientSessionPart = GetSession(channelUri, remotingServices);
            clientSessionPart.AddProxy(proxy);

            //Task<ClientSessionPart> task = null;
            //ClientSessionPart clientSessionPart = null;
            //lock (SessionsLock)
            //{


            //    if (!Sessions.TryGetValue(channelUri, out clientSessionPart))
            //    {

            //        if (!SessionsTasks.TryGetValue(channelUri, out task))
            //        {
            //            task = new Task<ClientSessionPart>(() =>
            //            {
            //                var serverSessionPartInfo = remotingServices.GetServerSession(channelUri, Remoting.RemotingServices.ProcessIdentity);
            //                var serverSessionPart = serverSessionPartInfo.ServerSessionPart;
            //                var serverProcessIdentity = serverSessionPartInfo.ServerProcessIdentity;
            //                var clientProcessIdentity = Remoting.RemotingServices.ProcessIdentity;
            //                bool bidirectionalChannel = false;
            //                if (serverSessionPartInfo.BidirectionalChannel.HasValue)
            //                    bidirectionalChannel = serverSessionPartInfo.BidirectionalChannel.Value;

            //                //clientSessionPart = new ClientSessionPart(channelUri, clientProcessIdentity, serverProcessIdentity, serverSessionPart, remotingServices);
            //                clientSessionPart = remotingServices.CreateClientSessionPart(channelUri, clientProcessIdentity, serverSessionPartInfo);
            //                Sessions[channelUri] = clientSessionPart;

            //                return clientSessionPart;
            //            });
            //            SessionsTasks[channelUri] = task;
            //        }

            //    }
            //}
            //if (task != null)
            //{
            //    if (!task.Wait(System.TimeSpan.FromSeconds(5)))
            //        if (!task.Wait(System.TimeSpan.FromMinutes(1)))
            //            throw new System.TimeoutException(string.Format("SendTimeout {0} expired", System.TimeSpan.FromMinutes(1)));

            //    clientSessionPart = task.Result;
            //}

            //clientSessionPart.AddProxy(proxy);

            //if (!System.Threading.Monitor.TryEnter(Sessions, 2000))
            //    System.Diagnostics.Debug.WriteLine("DeadLock");
            //try
            //{

            //    string channelUri = proxy.ChannelUri;
            //    ClientSessionPart clientSessionPart = null;
            //    if (!Sessions.TryGetValue(channelUri, out clientSessionPart))
            //    {
            //        Task<ClientSessionPart> task = null;
            //        if (SessionsTasks.TryGetValue(channelUri, out task))
            //            SessionsTasks[channelUri] =
            //            var serverSessionPartInfo = remotingServices.GetServerSession(channelUri, Remoting.RemotingServices.ProcessIdentity);
            //        var serverSessionPart = serverSessionPartInfo.ServerSessionPart;
            //        var serverProcessIdentity = serverSessionPartInfo.ServerProcessIdentity;
            //        var clientProcessIdentity = Remoting.RemotingServices.ProcessIdentity;
            //        bool bidirectionalChannel = false;
            //        if (serverSessionPartInfo.BidirectionalChannel.HasValue)
            //            bidirectionalChannel = serverSessionPartInfo.BidirectionalChannel.Value;

            //        //clientSessionPart = new ClientSessionPart(channelUri, clientProcessIdentity, serverProcessIdentity, serverSessionPart, remotingServices);
            //        clientSessionPart = remotingServices.CreateClientSessionPart(channelUri, clientProcessIdentity, serverSessionPartInfo);
            //        Sessions[channelUri] = clientSessionPart;
            //        clientSessionPart.AddProxy(proxy);

            //        //clientSessionPart = NewSession(channelUri, clientProcessIdentity, serverProcessIdentity, serverSessionPart, remotingServices);

            //        //Sessions[channelUri].AttatchToServerSessionPart();
            //    }
            //    else
            //        clientSessionPart.AddProxy(proxy);



            //}
            //finally
            //{
            //    System.Threading.Monitor.Exit(Sessions);
            //}
        }
        //   static System.DateTime LastRenewTime=DateTime.Now;
        /// <summary>This method called from timer for massive renews. </summary>
        /// <MetaDataID>{04446B53-1D1A-41D3-8E92-7AD83738BD98}</MetaDataID>
        static public void SessionsCheck(object state)
        {

            System.Collections.Generic.Dictionary<string, ClientSessionPart> sessions = GetSessions();


            try
            {
                System.Collections.Generic.List<string> sessionsForRemove = new System.Collections.Generic.List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, ClientSessionPart> entry in sessions)
                {
                    if (entry.Value.HasNoProxies && !entry.Value.SuspendSessionRemove)
                        sessionsForRemove.Add(entry.Key);
                }
                foreach (string channelUri in sessionsForRemove)
                {
                    lock (SessionsLock)
                        Sessions.Remove(channelUri);
                }
            }
            catch (System.Exception error)
            {

            }





            //System.Collections.Hashtable Channels =new System.Collections.Hashtable();
            //if(!System.Threading.Monitor.TryEnter(WeakReferenceProxies,20000))
            //{
            //    System.Diagnostics.Debug.WriteLine("DeadLock");
            //    return;
            //}

            //try
            //{
            //    TimeSpan timeSpan=DateTime.Now-LastRenewTime;
            //    if(timeSpan.TotalMilliseconds<4000)
            //        return;
            //    else
            //        LastRenewTime=DateTime.Now;



            //    foreach(System.Collections.DictionaryEntry CurrEntry in WeakReferenceProxies)
            //    {
            //        WeakReference WeakReference=CurrEntry.Value as WeakReference;

            //        if(WeakReference!=null&&WeakReference.IsAlive)
            //        {
            //            Proxy proxy=WeakReference.Target as Proxy;
            //            if (proxy!=null)
            //            {

            //                if(proxy!=null&&proxy._RealTransparentProxy==null)
            //                    continue;

            //                string ChannelUri=proxy.ChannelUri;
            //                if(ChannelUri!=null)
            //                {
            //                    if(!Channels.Contains(ChannelUri))
            //                        Channels[ChannelUri]=new System.Collections.ArrayList(100);

            //                    Remoting.ExtObjectUri extObjectUri=new Remoting.ExtObjectUri(proxy.URI,proxy.PersistentUri==null);
            //                    ((System.Collections.ArrayList)Channels[ChannelUri]).Add(extObjectUri);
            //                    continue;
            //                }
            //            }
            //            else
            //                RemovedWeakReference.Add(CurrEntry.Key);
            //        }

            //    }
            //    foreach(string  CurrUri in RemovedWeakReference)
            //        WeakReferenceProxies.Remove(CurrUri);
            //    RemovedWeakReference.Clear();
            //}
            //finally
            //{
            //    System.Threading.Monitor.Exit(WeakReferenceProxies);
            //}
            //System.Collections.Generic.List<string> channelUrisForRemove = new System.Collections.Generic.List<string>();
            //foreach(System.Collections.DictionaryEntry CurrDictionaryEntry in Channels)
            //{
            //    System.Collections.ArrayList ObjectUris=(System.Collections.ArrayList)CurrDictionaryEntry.Value;
            //    string channelUri = (string)CurrDictionaryEntry.Key;
            //    try
            //    {
            //        string RemotingServicesUri = channelUri + "/OOAdvantechRemotingServer/RemotingServices";
            //        OOAdvantech.Remoting.RemotingServices theRemotingServices = System.Activator.GetObject(typeof(OOAdvantech.Remoting.RemotingServices), RemotingServicesUri) as OOAdvantech.Remoting.RemotingServices;

            //        //System.Collections.ArrayList CollectedUris;
            //        theRemotingServices.RenewObjects(ref ObjectUris);
            //    }
            //    catch (System.Runtime.Remoting.RemotingException remotingError)
            //    {
            //        if (channelUri.ToLower().Trim().IndexOf("ipc:") == 0)
            //        {
            //            foreach (ExtObjectUri extObjectUri in ObjectUris)
            //                WeakReferenceProxies.Remove(extObjectUri.TransientUri);
            //        }
            //    }
            //    catch (System.Exception Error)
            //    {
            //        int hh = 0;
            //        //CollectedUris=((System.Collections.ArrayList)CurrDictionaryEntry.Value).Clone() as System.Collections.ArrayList;
            //    }
            //    System.DateTime Now=System.DateTime.Now;
            //    try
            //    {
            //        if(!System.Threading.Monitor.TryEnter(WeakReferenceProxies,20000))
            //            System.Diagnostics.Debug.WriteLine("DeadLock");
            //        foreach(Remoting.ExtObjectUri  CurrExtObjectUri in ObjectUris)
            //        {
            //            if(!CurrExtObjectUri.Disconnected)
            //            {
            //                if(WeakReferenceProxies.Contains(CurrExtObjectUri.TransientUri))
            //                {
            //                    Proxy proxy= (WeakReferenceProxies[CurrExtObjectUri.TransientUri] as WeakReference).Target as Proxy;
            //                    if(proxy==null)
            //                        continue;
            //                    if(CurrExtObjectUri.PersistentUriRequest&&CurrExtObjectUri.PersistentUri!=null)
            //                        proxy.PersistentUri=CurrExtObjectUri.PersistentUri;
            //                    //proxy.LastRenewTime=System.DateTime.Now;
            //                }
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        System.Threading.Monitor.Exit(WeakReferenceProxies);
            //    }

            //}
            //foreach (string channelUri in channelUrisForRemove)
            //    Channels.Remove(channelUri);


        }


        /// <MetaDataID>{30f14629-7ebd-4edb-aa20-7b3b09b6b711}</MetaDataID>
        internal static ClientSessionPart GetSession(string channelUri, IRemotingServices remotingServices)
        {
            return GetSession(channelUri, false, remotingServices);
        }
        /// <MetaDataID>{e7823178-829f-4efa-b13d-e42685846878}</MetaDataID>
        internal static ClientSessionPart GetSession(string channelUri, bool create, IRemotingServices remotingServices)
        {
            return GetSession(channelUri, create, create, remotingServices);

        }
        //internal static ClientSessionPart pNewSession(string channelUri, Guid clientProcessIdentity, Guid serverProcessIdentity, Remoting.IServerSessionPart serverSessionPart, IRemotingServices remotingServices)
        //{

        //    if (!System.Threading.Monitor.TryEnter(Sessions, 20000))
        //        System.Diagnostics.Debug.WriteLine("DeadLock");
        //    try
        //    {

        //        if (!Sessions.ContainsKey(channelUri))
        //        {
        //            var clientSessionPart = new ClientSessionPart(channelUri, clientProcessIdentity, serverProcessIdentity, serverSessionPart, remotingServices);
        //            Sessions.Add(channelUri, clientSessionPart);
        //        }
        //        return Sessions[channelUri];
        //    }
        //    finally
        //    {
        //        System.Threading.Monitor.Exit(Sessions);
        //    }
        //}

        /// <MetaDataID>{af69898e-6c33-4253-b632-4b2e4e4b18de}</MetaDataID>
        internal static ClientSessionPart GetSession(string sessionIdentity)
        {
            lock (SessionsLock)
            {
                return (from clientSessionPart in Sessions.Values
                        where clientSessionPart.SessionIdentity == sessionIdentity
                        select clientSessionPart).FirstOrDefault();
            }
        }

        /// <MetaDataID>{72f20427-0d16-4ebc-bb25-66f7aea8b44f}</MetaDataID>
        internal static ClientSessionPart GetSession(string channelUri, bool create, bool suspendSessionRemove, IRemotingServices remotingServices)
        {


            Task<ClientSessionPart> task = null;
            ClientSessionPart clientSessionPart = null;

            var sessions = GetSessions();
            {

                if (!sessions.TryGetValue(channelUri, out clientSessionPart))
                {
                    if (!create)
                        return null;
                    lock (SessionsTasks)
                    {
                        if (!SessionsTasks.TryGetValue(channelUri, out task))
                        {
                            task = Task.Factory.StartNew<ClientSessionPart>(() =>
                            {
                                try
                                {
                                    var serverSessionPartInfo = remotingServices.GetServerSession(channelUri, Remoting.RemotingServices.ProcessIdentity);
                                    var serverSessionPart = serverSessionPartInfo.ServerSessionPart;
                                    var proxyChannelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(serverSessionPart);
                                    var serverProcessIdentity = serverSessionPartInfo.ServerProcessIdentity;
                                    var clientProcessIdentity = Remoting.RemotingServices.ProcessIdentity;
                                    bool bidirectionalChannel = false;
                                    if (serverSessionPartInfo.BidirectionalChannel.HasValue)
                                        bidirectionalChannel = serverSessionPartInfo.BidirectionalChannel.Value;

                                    //clientSessionPart = new ClientSessionPart(channelUri, clientProcessIdentity, serverProcessIdentity, serverSessionPart, remotingServices);
                                    clientSessionPart = remotingServices.CreateClientSessionPart(channelUri, clientProcessIdentity, serverSessionPartInfo);
                                    if (clientSessionPart == null)
                                    {
                                        System.Diagnostics.Debug.Write("clientSessionPart==null");
                                    }
                                    lock (SessionsLock)
                                        Sessions[channelUri] = clientSessionPart;

                                    if (!string.IsNullOrEmpty(proxyChannelUri) && channelUri != proxyChannelUri)
                                    {
                                        lock (SessionsLock)
                                            Sessions[proxyChannelUri] = clientSessionPart;
                                    }
                                    return clientSessionPart;
                                }
                                catch (Exception error)
                                {

                                    throw;
                                }
                            });

                            SessionsTasks[channelUri] = task;
                        }
                    }

                }
            }
            if (task != null)
            {

                try
                {


                    if (!task.Wait(System.TimeSpan.FromSeconds(10)))
                        if (!task.Wait(System.TimeSpan.FromMinutes(2.5)))
                        {
                            lock (SessionsTasks)
                                SessionsTasks.Remove(channelUri);
                            throw new System.TimeoutException(string.Format("SendTimeout {0} expired", System.TimeSpan.FromMinutes(2.5)));
                        }

                    clientSessionPart = task.Result;
                }
                finally
                {
                    lock (SessionsTasks)
                        SessionsTasks.Remove(channelUri);
                }

            }

            return clientSessionPart;



            //if (!System.Threading.Monitor.TryEnter(Sessions, 2000))
            //    System.Diagnostics.Debug.WriteLine("DeadLock");
            //try
            //{
            //    ClientSessionPart clientSessionPart = null;
            //    if (!Sessions.TryGetValue(channelUri, out clientSessionPart))

            //    {


            //        if (!create)
            //            return null;
            //        else
            //        {
            //            var serverSessionPartInfo = remotingServices.GetServerSession(channelUri, Remoting.RemotingServices.ProcessIdentity);
            //            var serverSessionPart = serverSessionPartInfo.ServerSessionPart;

            //            var serverProcessIdentity = serverSessionPartInfo.ServerProcessIdentity;
            //            var clientProcessIdentity = Remoting.RemotingServices.ProcessIdentity;


            //            clientSessionPart = (from theClientSessionPart in Sessions.Values
            //                                 where theClientSessionPart.SessionIdentity == serverSessionPartInfo.SessionIdentity// theClientSessionPart.ClientProcessIdentity == Remoting.RemotingServices.ProcessIdentity && theClientSessionPart.ServerProcessIdentity == serverProcessIdentity
            //                                 select theClientSessionPart).FirstOrDefault();

            //            if (clientSessionPart != null)
            //                Sessions[channelUri] = clientSessionPart;
            //            else
            //            {
            //                bool bidirectionalChannel = false;
            //                if (serverSessionPartInfo.BidirectionalChannel.HasValue)
            //                    bidirectionalChannel = serverSessionPartInfo.BidirectionalChannel.Value;

            //                clientSessionPart = remotingServices.CreateClientSessionPart(RemotingServices.GetChannelUri(serverSessionPart), clientProcessIdentity, serverSessionPartInfo);
            //                Sessions[channelUri] = clientSessionPart;
            //            }
            //        }
            //    }

            //    return clientSessionPart;
            //}
            //finally
            //{
            //    System.Threading.Monitor.Exit(Sessions);
            //}
        }
    }
}
