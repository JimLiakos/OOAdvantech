using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;

namespace OOAdvantech.Remoting
{


    /// <MetaDataID>{3a4bf143-474e-48e9-b010-53126fb83239}</MetaDataID>
    [MetaDataRepository.HttpVisible]
#if DeviceDotNet
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
#endif
    public interface IServerSessionPart
    {

        /// <MetaDataID>{3d84aa7f-8ae0-4340-85fe-4c781f6ec1e5}</MetaDataID>
        void ClientProcessTerminates();
        /// <MetaDataID>{dfe0ee1a-fd90-42c4-a6bd-7a0c3ffa62b6}</MetaDataID>
        object GetLifetimeService();
        /// <MetaDataID>{294a9d7f-3a43-4c11-b94c-369d07075322}</MetaDataID>
        Guid ServerProcessIdentity
        {
            get;
        }
        /// <MetaDataID>{bd801830-d719-4232-9f42-5324fd70ccc8}</MetaDataID>
        void Update(ref System.Collections.Generic.List<ExtObjectUri> jastCreatedProxies, System.Collections.Generic.List<string> collectedProxies, out System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>> pendingEvents);

        /// <MetaDataID>{9c120fd9-0499-4e5f-abca-87b642834b6e}</MetaDataID>
        void Subscribe(ExtObjectUri eventPublisherUri, EventInfoData eventInfo);

#if !DeviceDotNet

        /// <MetaDataID>{ed8bff6b-db0b-4fdb-8449-a0d983cf2019}</MetaDataID>
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>> GetPendingEvents();
#endif      
        /// <MetaDataID>{61c51300-00a8-4ebb-89e5-f30ec61212f0}</MetaDataID>
        void Subscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions);
        /// <MetaDataID>{f193a1fc-a05b-4f44-afdd-00a0ee760fcb}</MetaDataID>
        void Subscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions, IRemoteEventHandler remoteEventHandler);
        /// <MetaDataID>{407081c8-a319-4ac7-bb24-0aacc678f070}</MetaDataID>
        void Subscribe(ExtObjectUri eventPublisherUri, EventInfoData eventInfo, IRemoteEventHandler clientSessionPart);
        /// <MetaDataID>{f7768c52-64c2-43d4-a3e7-dd45290a0aeb}</MetaDataID>
        void Unsubscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions);

        /// <MetaDataID>{db84997d-df9a-4ca2-a09b-6a7e68db4749}</MetaDataID>
        void Unsubscribe(ExtObjectUri eventPublisherUri, EventInfoData eventInfo);

        /// <MetaDataID>{bf8d7efc-58de-4745-b157-d7f91aebb9f3}</MetaDataID>
        object GetObject(ExtObjectUri extObjectUri);

        /// <MetaDataID>{d42d960b-99f9-4609-a44f-cfd58a3a34df}</MetaDataID>
        MarshalByRefObject GetObjectFromUri(ExtObjectUri extObjectUri);


    }

    /// <MetaDataID>{c16622e8-7cee-4e49-9757-d908d2df697d}</MetaDataID>
    [Serializable]
    public struct RemoteEventSubscription
    {
        /// <MetaDataID>{c5495bb5-cc37-40f0-9364-70b6311bfd80}</MetaDataID>
        public ExtObjectUri ExtObjectUri;
        /// <MetaDataID>{d9797538-b0e0-4152-b479-cff54cd917df}</MetaDataID>
        public EventInfoData eventInfo;
        /// <MetaDataID>{dafc2feb-d9f5-4474-98e9-1b7e2d9a4a53}</MetaDataID>
        public bool AllowAsynchronous;
    }

    /// <MetaDataID>{f4ec4b67-3e27-4833-86e1-95711cfc2d96}</MetaDataID>
    public class ServerSessionPart : MarshalByRefObject, IWeakRefernceEventHandler, IServerSessionPart
    {


        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(1)]
        [MetaDataRepository.IgnoreErrorCheck]
        [MetaDataRepository.Association("", typeof(EventConsumerProxy), MetaDataRepository.Roles.RoleA, "efd51e12-9bb8-4c91-9ff1-85f3e7885c5b")]
        Dictionary<string, Dictionary<EventInfo, EventConsumerProxy>> EventConsumers = new Dictionary<string, Dictionary<EventInfo, EventConsumerProxy>>();

        /// <MetaDataID>{b492e405-d195-4286-9823-17e4c2ed8427}</MetaDataID>
        virtual internal bool EventCallback(EventInfo eventInfo, ExtObjectUri eventPublisherUri, object[] args, InvokeType invokeType)
        {
            return false;
        }
#if !DeviceDotNet
#endif


        /// <MetaDataID>{7cb87445-c0ca-4109-98ac-91f1bd2b3c80}</MetaDataID>
        Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();


        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.Association("", typeof(MarshalByRefObject), MetaDataRepository.Roles.RoleA, "4b681cd1-0fb9-4a60-ac81-3cca474fc034")]
        [MetaDataRepository.IgnoreErrorCheck]
        public System.Collections.Generic.Dictionary<string, MarshalByRefObject> ControlledObjects = new System.Collections.Generic.Dictionary<string, MarshalByRefObject>();


        /// <exclude>Excluded</exclude>
        protected string _SessionIdentity;
        /// <MetaDataID>{e4fc3a85-631e-4027-8050-1a48f95d27c1}</MetaDataID>
        public string SessionIdentity
        {
            get
            {
                return _SessionIdentity;
            }
        }




        /// <MetaDataID>{e7311b57-029a-4795-b425-adf4c4c0a5b7}</MetaDataID>
        public System.Guid ClientProcessIdentity;
        /// <MetaDataID>{225728ef-eac7-40c9-b76f-48d837861267}</MetaDataID>
        public static System.Guid ServerProcessIdentity
        {
            get
            {
                return OOAdvantech.Remoting.RemotingServices.ProcessIdentity;
            }
        }




        /// <MetaDataID>{ea80ed2b-cb58-422f-8dff-aad739c79d26}</MetaDataID>
        public ServerSessionPart(System.Guid clientProcessIdentity)
        {
            ClientProcessIdentity = clientProcessIdentity;
            if(UseNetRemotingChamnel)
                new WeakReferenceEventPublisher(this, 5000);

            _SessionIdentity = clientProcessIdentity.ToString("N") + "." + ServerProcessIdentity.ToString("N");
        }

        /// <MetaDataID>{849b5e87-e3b4-4e8d-967a-ef572c475ed6}</MetaDataID>
        System.Guid IServerSessionPart.ServerProcessIdentity
        {
            get
            {
                return ServerProcessIdentity;
            }
        }

        public virtual bool  UseNetRemotingChamnel { get => true; }

        /// <MetaDataID>{150c32ae-b249-4174-a000-a4372d4a4cdc}</MetaDataID>
        ~ServerSessionPart()
        {
            ClientProcessTerminates();
        }





#if !DeviceDotNet
        /// <MetaDataID>{199b8695-d1ee-4a16-a1a7-30522d205357}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>> GetPendingEvents()
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>> pendingEvents = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>>();

            lock (EventConsumers)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<EventInfo, EventConsumerProxy>> entry in EventConsumers)
                {
                    foreach (EventConsumerProxy eventConsumer in entry.Value.Values)
                    {
                        if (eventConsumer.PendingEventArgs != null)
                        {
                            if (!pendingEvents.ContainsKey(eventConsumer.EventPublisherUri.Uri))
                                pendingEvents[eventConsumer.EventPublisherUri.Uri] = new OOAdvantech.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>();

                            System.Collections.Generic.List<object> marshaledArgs = eventConsumer.PendingEventArgs.ToList();
                            pendingEvents[eventConsumer.EventPublisherUri.Uri].Add(new EventInfoData(eventConsumer.EventInfo), marshaledArgs);
                            eventConsumer.PendingEventArgs = null;
                        }
                    }
                }
            }
            return pendingEvents;
        }



     


        //#if !DeviceDotNet
#endif


        /// <MetaDataID>{de8d3d6d-75bf-4579-8e3c-57ee9ed708ef}</MetaDataID>
        public virtual MarshalByRefObject GetObjectFromUri(ExtObjectUri extObjectUri)
        {
            MarshalByRefObject eventPublisherObject = null;
            lock (ControlledObjects)
            {
                if (ControlledObjects.TryGetValue(extObjectUri.TransientUri, out eventPublisherObject))
                    return eventPublisherObject;

                return null;
            }

            return eventPublisherObject;


            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (ControlledObjects.ContainsKey(extObjectUri.TransientUri))

                    return ControlledObjects[extObjectUri.TransientUri];
                return null;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }

        }

        /// <MetaDataID>{06a8bd80-4e74-4cf4-b51f-b20adb381614}</MetaDataID>
        public void Unsubscribe(ExtObjectUri eventPublisherUri, EventInfoData eventInfoData)
        {
            var eventInfo = eventInfoData.EventInfo;

            MarshalByRefObject eventPublisherObject = GetObjectFromUri(eventPublisherUri);
            if (eventPublisherObject == null)
                return;

            lock (EventConsumers)
            {

                if (EventConsumers.ContainsKey(eventPublisherUri.Uri) && EventConsumers[eventPublisherUri.Uri].ContainsKey(eventInfo))
                {
                    EventConsumerProxy eventConsumerProxy = EventConsumers[eventPublisherUri.Uri][eventInfo];
                    EventConsumers[eventPublisherUri.Uri].Remove(eventInfo);
                    if (EventConsumers[eventPublisherUri.Uri].Count == 0)
                        EventConsumers.Remove(eventPublisherUri.Uri);

                    eventConsumerProxy.RemoveSubscription(eventPublisherObject);

                }
            }
        }

        /// <MetaDataID>{6006ab78-9646-4005-abcb-91db780bcfb7}</MetaDataID>
        public void Subscribe(ExtObjectUri eventPublisherUri, EventInfoData eventInfoData, IRemoteEventHandler clientSessionPart)
        {
            System.Reflection.EventInfo eventInfo = eventInfoData.EventInfo;

            MarshalByRefObject eventPublisherObject = GetObjectFromUri(eventPublisherUri);
            if (eventPublisherObject == null)
                throw new System.Exception("There isn't object with ObjUri '" + eventPublisherUri.TransientUri + "' under control.");


            lock (EventConsumers)
            {
                //Creates (if doesn't exist) a collection wich will host the candidated EventConsumers for eventPublisherObject 
                if (!EventConsumers.ContainsKey(eventPublisherUri.Uri))
                    EventConsumers[eventPublisherUri.Uri] = new System.Collections.Generic.Dictionary<EventInfo, EventConsumerProxy>();
                //Creates (if doesn't exist) an EventConsumers for event of eventInfo. 
                if (!EventConsumers[eventPublisherUri.Uri].ContainsKey(eventInfo))
                {
                    EventConsumers[eventPublisherUri.Uri][eventInfo] = new EventConsumerProxy(eventPublisherUri, eventPublisherObject, eventInfo, this, clientSessionPart);
                }
                else
                {
                    EventConsumerProxy eventConsumerProxy = EventConsumers[eventPublisherUri.Uri][eventInfo];
                    if (eventConsumerProxy.RemoteEventHandler == null && clientSessionPart != null)
                    {
                        eventConsumerProxy.RemoteEventHandler = clientSessionPart;
                        eventConsumerProxy.ClientSessionPart = clientSessionPart as ClientSessionPart;
                    }


                }
            }
        }

        /// <MetaDataID>{dc9653cf-6c95-4f2f-b597-97eef5b6d222}</MetaDataID>
        public void Subscribe(ExtObjectUri eventPublisherUri, EventInfoData eventInfo)
        {
            Subscribe(eventPublisherUri, eventInfo, null);
        }
        /// <MetaDataID>{656bfad3-3b1e-4a4f-be9c-023dbd9775c2}</MetaDataID>
        public virtual void ClientProcessTerminates()
        {
            lock (this)
            {
                foreach (ClientSessionPart clientSessionPart in RenewalManager.Sessions.Values)
                {
                    if (clientSessionPart.ServerProcessIdentity == ClientProcessIdentity)
                    {
                        clientSessionPart.ServerProcessTerminates();
                        break;
                    }
                }
            }

            lock (EventConsumers)
            {
                foreach (System.Collections.Generic.Dictionary<EventInfo, EventConsumerProxy> eventPublisherConsumers in EventConsumers.Values)
                {
                    foreach (EventConsumerProxy eventConsumerProxy in eventPublisherConsumers.Values)
                    {
#if DeviceDotNet
                        eventConsumerProxy.RemoveSubscription(eventConsumerProxy.EventPublisherObject);

#else

                       // eventConsumerProxy.RemoveSubscription(eventConsumerProxy.EventPublisherObject);

                        System.Delegate customEventDelegate = System.Delegate.CreateDelegate(eventConsumerProxy.EventInfo.EventHandlerType, eventConsumerProxy.AutoGenEventHandler, "CustomEventHandler");
                        var _object = GetObjectFromUri(eventConsumerProxy.EventPublisherUri);
                        if(_object!=null)
                            eventConsumerProxy.EventInfo.RemoveEventHandler(_object, customEventDelegate);

                        System.Reflection.EventInfo commonEventInfo = eventConsumerProxy.AutoGenEventHandler.GetType().GetEvent("CommonEvent");
                        System.Delegate commonDelegate = System.Delegate.CreateDelegate(commonEventInfo.EventHandlerType, eventConsumerProxy, "EventCallback");
                        commonEventInfo.RemoveEventHandler(eventConsumerProxy.AutoGenEventHandler, commonDelegate);
#endif
                    }
                }
                EventConsumers.Clear();
                ControlledObjects.Clear();
            }
#if !DeviceDotNet
#endif


        }

        /// <MetaDataID>{8ff4ce80-b416-4d84-a79a-46c66669ccfd}</MetaDataID>
        public void Update(ref System.Collections.Generic.List<ExtObjectUri> jastCreatedProxies, System.Collections.Generic.List<string> collectedProxies, out System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>> pendingEvents)
        {

#if !DeviceDotNet
            pendingEvents = GetPendingEvents();
            System.Runtime.Remoting.Lifetime.ILease lease = GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
            lease.Renew(System.TimeSpan.FromMinutes(5.5));
#else
            pendingEvents = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<EventInfoData, System.Collections.Generic.List<object>>>();
#endif
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                foreach (ExtObjectUri extObjectUri in jastCreatedProxies)
                {


                    MarshalByRefObject controlledObject = null;
                    if (!ControlledObjects.ContainsKey(extObjectUri.TransientUri))
                    {
                        if (Tracker.WeakReferenceOnMarshaledObjects.ContainsKey(extObjectUri.TransientUri))
                        {
                            controlledObject = Tracker.WeakReferenceOnMarshaledObjects[extObjectUri.TransientUri].Target as MarshalByRefObject;
                            if (controlledObject == null)
                            {
                                extObjectUri.Disconnected = true;
                                Tracker.WeakReferenceOnMarshaledObjects.Remove(extObjectUri.TransientUri);
                            }
                            else
                            {
                                extObjectUri.Disconnected = false;
                                ControlledObjects.Add(extObjectUri.TransientUri, controlledObject);
                            }

                        }
                        else
                        {

                        }
                    }
                }
                foreach (string objectUri in collectedProxies)
                {
                    if (ControlledObjects.ContainsKey(objectUri))
                    {

                        lock (EventConsumers)
                        {
                            if (EventConsumers.ContainsKey(objectUri))
                            {
                                foreach (EventInfo eventInfo in EventConsumers[objectUri].Keys)
                                    Unsubscribe(new ExtObjectUri(objectUri, null, null, null, ClientProcessIdentity), new EventInfoData(eventInfo));
                            }
                        }
#if !DeviceDotNet
#endif
                        ControlledObjects.Remove(objectUri);
                    }

                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }


        /// <MetaDataID>{5edff37d-5d86-40ca-ac8c-e772e5893dfb}</MetaDataID>
        System.DateTime LastEventTime = System.DateTime.Now;

        /// <MetaDataID>{de0787ab-2ff9-4ca0-bc69-3acf654252df}</MetaDataID>
        void IWeakRefernceEventHandler.OnTimeTick()
        {

            System.TimeSpan timeSpan = System.DateTime.Now - LastEventTime;
            if (timeSpan.TotalMilliseconds > 4000)
            {
                LastEventTime = System.DateTime.Now;
                Renew();
            }

        }

        /// <MetaDataID>{1aca21bd-b05a-4029-b5dd-0276e2ae27b6}</MetaDataID>
        private void Renew()
        {

#if !DeviceDotNet
            System.Runtime.Remoting.Lifetime.ILease lease = null;
            foreach (MarshalByRefObject obj in ControlledObjects.Values)
            {
                lease = obj.GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
                if (lease != null)
                    lease.Renew(System.TimeSpan.FromMinutes(5.5));
            }
            lease = GetLifetimeService() as System.Runtime.Remoting.Lifetime.ILease;
            if (lease == null || lease.CurrentState == System.Runtime.Remoting.Lifetime.LeaseState.Expired)
            {
                System.GC.Collect(System.GC.GetGeneration(this));
                System.GC.Collect();
            }
#endif


        }



        /// <MetaDataID>{ab4f77d8-ae0a-4ee6-be31-5f9702a78044}</MetaDataID>
        public object GetObject(ExtObjectUri extObjectUri)
        {

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (ControlledObjects.ContainsKey(extObjectUri.TransientUri))

                    return ControlledObjects[extObjectUri.TransientUri];
                return null;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }
#if DeviceDotNet
    
        public void Subscribe(List<RemoteEventSubscription> eventSubscriptions)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(List<RemoteEventSubscription> eventSubscriptions, IRemoteEventHandler remoteEventHandler)
        {
            throw new NotImplementedException();
        }

     

        public void Unsubscribe(List<RemoteEventSubscription> eventSubscriptions)
        {
            throw new NotImplementedException();
        }



#else
        /// <MetaDataID>{a6d16c7b-e1c8-49ce-9f83-1c47b253baff}</MetaDataID>
        public void Subscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions)
        {
            foreach (RemoteEventSubscription eventSubscription in eventSubscriptions)
                Subscribe(eventSubscription.ExtObjectUri, eventSubscription.eventInfo);

        }

        /// <MetaDataID>{005af59a-1b0a-4db4-87b4-513153bf7e2c}</MetaDataID>
        public void Subscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions, IRemoteEventHandler remoteEventHandler)
        {
            foreach (RemoteEventSubscription eventSubscription in eventSubscriptions)
            {
                if (eventSubscription.AllowAsynchronous)
                    Subscribe(eventSubscription.ExtObjectUri, eventSubscription.eventInfo);
                else
                    Subscribe(eventSubscription.ExtObjectUri, eventSubscription.eventInfo, remoteEventHandler);
            }
        }

        /// <MetaDataID>{4714d5a5-da62-4c80-abc4-ee61a23ef961}</MetaDataID>
        public void Unsubscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions)
        {
            foreach (RemoteEventSubscription eventSubscription in eventSubscriptions)
                Unsubscribe(eventSubscription.ExtObjectUri, eventSubscription.eventInfo);
        }
#endif
    }
}

