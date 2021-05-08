using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{3a4bf143-474e-48e9-b010-53126fb83239}</MetaDataID>
    [MetaDataRepository.HttpVisible]
    public interface IServerSessionPart
    {
        void ClientProcessTerminates();
        object GetLifetimeService();
        Guid ServerProcessIdentity
        {
            get;
        }
        void Subscribe(ExtObjectUri eventPublisherUri, System.Reflection.EventInfo eventInfo);


        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<System.Reflection.EventInfo, System.Collections.ArrayList>> GetPendingEvents();
        void Unsubscribe(ExtObjectUri eventPublisherUri, System.Reflection.EventInfo eventInfo);
        void Update(ref System.Collections.Generic.List<ExtObjectUri> jastCreatedProxies, System.Collections.Generic.List<string> collectedProxies, out System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<System.Reflection.EventInfo, System.Collections.ArrayList>> pendingEvents);
        void Subscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions);
        void Subscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions, IRemoteEventHandler remoteEventHandler);
        void Subscribe(ExtObjectUri eventPublisherUri, System.Reflection.EventInfo eventInfo, IRemoteEventHandler clientSessionPart);
        void Unsubscribe(System.Collections.Generic.List<RemoteEventSubscription> eventSubscriptions);
        object GetObject(ExtObjectUri extObjectUri);
    }

    /// <MetaDataID>{c16622e8-7cee-4e49-9757-d908d2df697d}</MetaDataID>
    [Serializable]
    public struct RemoteEventSubscription
    {
        /// <MetaDataID>{c5495bb5-cc37-40f0-9364-70b6311bfd80}</MetaDataID>
        public ExtObjectUri ExtObjectUri;
        /// <MetaDataID>{d9797538-b0e0-4152-b479-cff54cd917df}</MetaDataID>
        public System.Reflection.EventInfo eventInfo;
        /// <MetaDataID>{dafc2feb-d9f5-4474-98e9-1b7e2d9a4a53}</MetaDataID>
        public bool AllowAsynchronous;
    }
}
