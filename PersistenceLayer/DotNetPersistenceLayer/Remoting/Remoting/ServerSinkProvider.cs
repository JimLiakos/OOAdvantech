using System;
using System.Collections;
using System.Runtime.Remoting.Channels;

namespace OOAdvantech.Remoting.Sinks
{
    /// <MetaDataID>{59B310FE-92F2-416C-88D3-5B820E352C4E}</MetaDataID>
    /// <summary>Creates server channel sinks for the server channel through which remoting messages flow.
    /// For informations look at IServerChannelSinkProvider interface on microsoft help.
    /// The server sink which created filter the message for objects that is type of IExtMarshalByRefObject. If it find remote object of this to travel on channel then add a proxy class between client code and real proxy for the life time control of the object. </summary>
    internal class ServerSinkProvider : IServerChannelSinkProvider, ICloneable 
	{
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{23A87DEF-3D59-4DC9-BFA0-07695D85A32B}</MetaDataID>
        private System.Runtime.Remoting.Channels.IServerChannelSinkProvider _Next;

        /// <summary>Gets or sets the next sink provider in the channel sink provider chain. </summary>
        /// <MetaDataID>{F29F3819-DC6B-4F91-B8CF-37F07872C437}</MetaDataID>
        public System.Runtime.Remoting.Channels.IServerChannelSinkProvider Next
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



        /// <MetaDataID>{E8A44E80-8CBB-482D-BA89-14AE729F9B97}</MetaDataID>
        /// <summary>Creates a sink chain. </summary>
        /// <param name="channel">The channel for which to create the channel sink chain. </param>
        /// <returns>The first sink of the newly formed channel sink chain, or null , indicating that this provider will not or cannot provide a connection for this endpoint. </returns>
		public IServerChannelSink CreateSink (IChannelReceiver channel)
		{
			IServerChannelSink nextSink = _Next.CreateSink(channel);
            return new ServerSink(nextSink,channel);
		}
		#region ICloneable Members

        /// <MetaDataID>{9C63E138-A9E2-4300-A192-60A56A44F3D6}</MetaDataID>
		public object Clone()
		{
			ServerSinkProvider Clone=new ServerSinkProvider();
			if(_Next!=null&&_Next is ICloneable)
				Clone.Next=((_Next as ICloneable).Clone() as IServerChannelSinkProvider);
			return Clone;
		}
		#endregion

        #region IServerChannelSinkProvider Members


        /// <MetaDataID>{D9CD15E2-1FCF-45AE-9D95-8D28B1B892D0}</MetaDataID>
        public void GetChannelData(System.Runtime.Remoting.Channels.IChannelDataStore channelData)
        {
          
        }


        #endregion

        

        
    }
}