using System;
using System.Collections.Generic;
using System.Text;

#if PORTABLE
    using System.PCL.Reflection;
#else
using System.Reflection;
#endif

namespace OOAdvantech.Remoting
{

    /// <MetaDataID>{3BB13FF6-049A-4e8b-92E2-2746C25AB2A1}</MetaDataID>
    public class EventConsumerProxy
    {

        /// <MetaDataID>{2dc022e9-b3a2-4f87-8601-82ebd49b5f76}</MetaDataID>
        public readonly ExtObjectUri EventPublisherUri;
        /// <MetaDataID>{b60f54c6-5a44-478e-b605-afc45679cfe3}</MetaDataID>
        public readonly System.Reflection.EventInfo EventInfo;
        /// <MetaDataID>{5362c87a-bbed-4696-8a0a-0eddc2b6bade}</MetaDataID>
        public readonly object AutoGenEventHandler;
        /// <MetaDataID>{70edf773-b61c-493b-9f88-d85f984eb55c}</MetaDataID>
        internal ClientSessionPart ClientSessionPart;
        ///// <MetaDataID>{a01150a9-6824-4782-8958-44fa0ce59647}</MetaDataID>
        //internal bool WithoutRemoteCall;
        /// <MetaDataID>{60589943-1a70-46bd-8e34-d2721f66c1b5}</MetaDataID>
        internal object[] PendingEventArgs;

        internal IRemoteEventHandler RemoteEventHandler;
        internal ServerSessionPart ServerSessionPart;
        EventConsumerHandler EventConsumerHandler;
        public readonly MarshalByRefObject EventPublisherObject;
        InvokeType InvokeType;
        /// <MetaDataID>{13fa09ba-d79c-42e5-8ddf-034cd1a7334b}</MetaDataID>
        public EventConsumerProxy(ExtObjectUri eventPublisherUri, MarshalByRefObject eventPublisherObject, System.Reflection.EventInfo eventInfo, ServerSessionPart serverSessionPart, IRemoteEventHandler clientSessionPart)
        {
            EventPublisherObject = eventPublisherObject;
            EventPublisherUri = eventPublisherUri;
            EventInfo = eventInfo;

            object[] eventPublishAttributeAttributes = EventInfo.GetCustomAttributes(typeof(RemoteEventPublishAttribute), true);

            if (eventPublishAttributeAttributes != null && eventPublishAttributeAttributes.Length > 0)
                InvokeType = (eventPublishAttributeAttributes[0] as RemoteEventPublishAttribute).InvokeType;

            ClientSessionPart = clientSessionPart as ClientSessionPart;
            ServerSessionPart = serverSessionPart;
            RemoteEventHandler = clientSessionPart;


#if DeviceDotNet


            string evenProxyTypeFullName = eventInfo.DeclaringType.Namespace + ".Proxies.CNSPr_" + eventInfo.DeclaringType.Name + "_" + eventInfo.Name;

            Type transparentProxyType = eventInfo.DeclaringType.GetMetaData().Assembly.GetType(evenProxyTypeFullName);

            var declaringType = eventInfo.DeclaringType.BaseType;
            while (transparentProxyType == null && declaringType != null)
            {


                var superTypeEventInfo = declaringType.GetEvent(eventInfo.Name);
                if (superTypeEventInfo != null && superTypeEventInfo.EventHandlerType == eventInfo.EventHandlerType)
                {
                    evenProxyTypeFullName = superTypeEventInfo.DeclaringType.Namespace + ".Proxies.CNSPr_" + superTypeEventInfo.DeclaringType.Name + "_" + eventInfo.Name;

                    transparentProxyType = superTypeEventInfo.DeclaringType.GetMetaData().Assembly.GetType(evenProxyTypeFullName);
                }
                if (declaringType.BaseType == typeof(object) || declaringType.BaseType == null)
                    break;

                declaringType = declaringType.BaseType;
            }
            if (transparentProxyType == null)
            {
                foreach (var _interface in eventInfo.DeclaringType.GetInterfaces())
                {
                    declaringType = _interface;
                    var superTypeEventInfo = declaringType.GetEvent(eventInfo.Name);
                    if (superTypeEventInfo != null && superTypeEventInfo.EventHandlerType == eventInfo.EventHandlerType)
                    {
                        evenProxyTypeFullName = superTypeEventInfo.DeclaringType.Namespace + ".Proxies.CNSPr_" + superTypeEventInfo.DeclaringType.Name + "_" + eventInfo.Name;

                        transparentProxyType = superTypeEventInfo.DeclaringType.GetMetaData().Assembly.GetType(evenProxyTypeFullName);
                    }

                    if (transparentProxyType != null)
                        break;
                }
            }

            if (transparentProxyType != null)
            {
                var ctor = transparentProxyType.GetMetaData().GetConstructor(BindingFlags.Public | BindingFlags.Instance, new Type[0]);
                EventConsumerHandler = ctor.Invoke(new object[0]) as EventConsumerHandler;
                EventConsumerHandler.SetEventConsumerProxy(this);
                EventConsumerHandler.AddEventHandler(eventPublisherObject, EventInfo);
            }
#if DEBUG
            else
                throw new Exception(string.Format("Missing remote event consumer proxy type for '{0}'.{1} Declare [GenerateEventConsumerProxy] attribute to event {0}", eventInfo.DeclaringType.FullName + "." + eventInfo.Name, System.Environment.NewLine));
#endif



#else
            OOAdvantech.CommonEventHandler.EventHandlerFactory factory = new OOAdvantech.CommonEventHandler.EventHandlerFactory("OOAdvantechRemoteEvents");
            AutoGenEventHandler = factory.CreateEventHandler(eventInfo);
            //if (EventInfo.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.WithoutRemoteCallAttribute), true).Length > 0)
            //    WithoutRemoteCall = true;
            //else
            //    WithoutRemoteCall = false;

            
            // Create a delegate, which points to the custom event handler
            System.Delegate customEventDelegate = System.Delegate.CreateDelegate(eventInfo.EventHandlerType, AutoGenEventHandler, "CustomEventHandler");

            

            // Map our own event handler to the common event
            System.Reflection.EventInfo commonEventInfo = AutoGenEventHandler.GetType().GetEvent("CommonEvent");
            System.Delegate commonDelegate = System.Delegate.CreateDelegate(commonEventInfo.EventHandlerType, this, "EventCallback");
            commonEventInfo.AddEventHandler(AutoGenEventHandler, commonDelegate);

            eventInfo.AddEventHandler(eventPublisherObject, customEventDelegate);

#endif


        }


        /// <MetaDataID>{e95ae37f-d578-4d20-95cc-45134b794bdd}</MetaDataID>
        public void EventCallback(System.Type EventType, object[] args)
        {
            try
            {
                if (ServerSessionPart != null && ServerSessionPart.EventCallback(EventInfo, EventPublisherUri, args, InvokeType))
                    return;
#if !DeviceDotNet
                if (ClientSessionPart == null && RemoteEventHandler == null)
                {
                    System.Runtime.Remoting.Messaging.CallContext.SetData("{5AC92626-E8F3-42ee-A050-483C33C56BF0}", new PendingEvent(true));
                    PendingEventArgs = args;
                }
                else
                {
                    if (ClientSessionPart != null)
                        ClientSessionPart.EventCallback(EventPublisherUri.TransientUri, EventInfo, new System.Collections.Generic.List<object>(args));
                    else
                    {
                        PendingEventArgs = args;
                        RemoteEventHandler.ThereArePendingEvents(0);
                    }
                }
#endif
            }
            catch (System.Exception error)
            {
                System.GC.Collect();
                throw;
            }
        }

        /// <MetaDataID>{01af2eb1-5b02-40df-900b-c9daa6750ee5}</MetaDataID>
        ~EventConsumerProxy()
        {

        }


        /// <MetaDataID>{2426a609-c369-4185-be29-009f1731f323}</MetaDataID>
        internal void RemoveSubscription(MarshalByRefObject eventPublisherObject)
        {
#if DeviceDotNet

            EventConsumerHandler.RemoveEventHandler(eventPublisherObject, EventInfo);

#else
            System.Delegate customEventDelegate = System.Delegate.CreateDelegate(EventInfo.EventHandlerType, AutoGenEventHandler, "CustomEventHandler");
            EventInfo.RemoveEventHandler(eventPublisherObject, customEventDelegate);
            System.Reflection.EventInfo commonEventInfo = AutoGenEventHandler.GetType().GetEvent("CommonEvent");
            System.Delegate commonDelegate = System.Delegate.CreateDelegate(commonEventInfo.EventHandlerType, this, "EventCallback");
            commonEventInfo.RemoveEventHandler(AutoGenEventHandler, commonDelegate);
#endif

        }
    }

    /// <MetaDataID>{4e65e195-fc41-4e68-b9c7-f67044c8d5bd}</MetaDataID>
    public abstract class EventConsumerHandler
    {
        public abstract void AddEventHandler(object target, System.Reflection.EventInfo eventInfo);

        public abstract void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo);

        public object Invoke(Type type, string methodName, object[] args, Type[] argsTypes)
        {
            EventConsumerProxy.EventCallback(type, args);
            return null;
        }

        EventConsumerProxy EventConsumerProxy;
        internal void SetEventConsumerProxy(EventConsumerProxy eventConsumerProxy)
        {
            EventConsumerProxy = eventConsumerProxy;
        }
    }
}
