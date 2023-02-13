using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;

#if !DeviceDotNet
using System.Security.Cryptography;
#endif

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{63f2e6e8-e8fa-4808-8814-7e8100f81a43}</MetaDataID>
    public interface IProxy
    {

        string Uri { get; }
        /// <MetaDataID>{d9cd4d5c-1240-4526-868b-ff629e762a40}</MetaDataID>
        string ChannelUri { get; set; }
        /// <MetaDataID>{d5eaf7c3-eda2-4119-b78f-7b5d0ecdf957}</MetaDataID>
        ExtObjectUri ObjectUri { get; }

        /// <MetaDataID>{6c477cf5-e0f2-4c79-b313-2a5e318e6e68}</MetaDataID>
        IMessage Invoke(IMessage msg);


        /// <MetaDataID>{7de248c7-74a5-4fd0-8d45-1c98e415a8fd}</MetaDataID>
        EventConsumingResolver EventConsumingResolver { get; set; }


        ///// <MetaDataID>{c89837de-4066-4a8d-b244-afc8bd0f41ef}</MetaDataID>
        //void PublishEvent(EventInfo eventInfo, System.Collections.Generic.List<object> args);

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
                        int i = 0;
                        foreach (ParameterInfo parameter in eventHandler.GetMethodInfo().GetParameters())
                        {
                            if (args.Count > i)
                                args[i] = ValidateArgumentType(args[i], parameter.ParameterType);

                            i++;
                        }
                        eventHandler.GetMethodInfo().Invoke(eventHandler.Target, args.ToArray());
                    }
                    catch (System.Exception error)
                    {
                    }
                }
            }
        }

        private object ValidateArgumentType(object obj, Type objType)
        {
            if (obj is OOAdvantech.Remoting.RestApi.ITransparentProxy)
                return ((obj as OOAdvantech.Remoting.RestApi.ITransparentProxy).GetProxy() as RestApi.Proxy) .GetTransparentProxy(objType);
            else
                return obj;


        }

        /// <MetaDataID>{030454ee-b6b3-449f-a308-fdf3d772f6de}</MetaDataID>
        internal void EventConsumerUnsubscribe(System.Delegate removedEventHandler, System.Reflection.EventInfo eventInfo, out bool sessionUnsubscribe)
        {
            sessionUnsubscribe = false;
            if (EventsInvocationLists != null && EventsInvocationLists.ContainsKey(eventInfo))
            {
                foreach (System.Delegate eventHandler in EventsInvocationLists[eventInfo].EventConsumers)
                {

                    if (eventHandler.Target == removedEventHandler.Target && eventHandler.GetMethodInfo() == removedEventHandler.GetMethodInfo())
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
                    if (eventHandler.GetMethodInfo().GetCustomAttributes(typeof(MetaDataRepository.AllowEventCallAsynchronousAttribute), true).Length > 0)
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
                if (eventHandler.GetMethodInfo().GetCustomAttributes(typeof(MetaDataRepository.AllowEventCallAsynchronousAttribute), true).Length == 0)
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

        [Json.JsonIgnore]
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
        [Json.JsonIgnore]
        private EventInfo _EventInfo;

        /// <MetaDataID>{1ba4a438-49c0-498b-bbff-7a80e7b061fb}</MetaDataID>
        [Json.JsonIgnore]
        public EventInfo EventInfo
        {
            get
            {
                try
                {
                    if (_EventInfo == null && !string.IsNullOrEmpty(DeclaringType))
                        _EventInfo = Type.GetType(DeclaringType).GetMetaData().GetEvent(EventName);
                }
                catch (Exception error)
                {
                }

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

            //if (left != null && right == null)
            if ((left is EventInfoData) && !(right is EventInfoData))
                return false;

            //if (left == null && right != null)
            if (!(left is EventInfoData) && (right is EventInfoData))
                return false;

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

}


