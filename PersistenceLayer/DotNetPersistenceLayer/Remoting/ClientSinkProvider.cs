using System;
using System.Collections;
using System.Runtime.Remoting.Channels;

namespace OOAdvantech.Remoting.Sinks
{
    /// <MetaDataID>{D784A8C1-15B3-450E-AF24-326435644EEC}</MetaDataID>
    /// <summary>Creates client channel sinks for the client channel through which remoting messages flow.
    /// For informations look at IClientChannelSinkProvider interface on microsoft help.
    /// The client sink which create filter the message for object that is type of IExtMarshalByRefObject. If it find remote object of this to travel on channel then add a proxy class between client code and real proxy for the life time control of the object. wewe </summary>
    public class ClientSinkProvider : System.Runtime.Remoting.Channels.IClientChannelSinkProvider, ICloneable
    {

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3E6269B4-A321-411C-B439-E937D16E4195}</MetaDataID>
        private IClientChannelSinkProvider _Next;
        /// <MetaDataID>{0694B90B-F51B-440A-AC7E-5E4D0EF7FDEB}</MetaDataID>
        public IClientChannelSinkProvider Next
        {
            set
            {
                _Next = value;
            }
            get
            {
                return _Next;
            }

        }

        /// <MetaDataID>{D1FEA863-FB9A-4993-8CBB-F3030368FA25}</MetaDataID>
        /// <summary>Creates a sink chain. </summary>
        /// <returns>The first sink of the newly formed channel sink chain, or null indicating that this provider will not or can't provide a connection for this endpoint. </returns>
        /// <param name="channel">Channel for which the current sink chain is being constructed. </param>
        /// <param name="url">The URL of the object to connect to. This parameter can be null if the connection is based entirely on the information contained in the remoteChannelData parameter. </param>
        /// <param name="remoteChannelData">A channel data object describing a channel on the remote server. </param>
        public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            IClientChannelSink nextsink = _Next.CreateSink(channel, url, remoteChannelData);
            return new ClientSink(url, nextsink as System.Runtime.Remoting.Messaging.IMessageSink);
        }
        /// <MetaDataID>{EF34184C-333A-4D35-A44F-43842EC5E148}</MetaDataID>
        public object Clone()
        {
            ClientSinkProvider Clone = new ClientSinkProvider();
            if (_Next != null && _Next is ICloneable)
                Clone.Next = ((_Next as ICloneable).Clone() as IClientChannelSinkProvider);
            return Clone;
        }
    }
}
