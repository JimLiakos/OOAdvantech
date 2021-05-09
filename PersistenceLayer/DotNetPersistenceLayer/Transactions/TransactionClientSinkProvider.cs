using System;
using System.Collections;
using System.Runtime.Remoting.Channels;

namespace OOAdvantech.Transactions.Sinks
{
	
	/// <MetaDataID>{6FE93B3D-895D-41B2-87B6-E557050E9342}</MetaDataID>
	internal class ClientSinkProvider: IClientChannelSinkProvider,ICloneable 
	{

        //static ClientSinkProvider()
        //{
        //    InitProcessTCConnectionHandle dlg = new InitProcessTCConnectionHandle(InitProcessTCConnection);
        //    dlg.BeginInvoke(null, null);

        //}
	
		/// <MetaDataID>{9CA9D94E-2080-4430-85A8-C2F0CB51402F}</MetaDataID>
		private IClientChannelSinkProvider next = null;

		 
	

	
		/// <MetaDataID>{A64362C7-3878-42CB-9E1C-9A7315DA0EAB}</MetaDataID>
		public IClientChannelSink CreateSink(IChannelSender channel, 
			string url, object remoteChannelData)
		{
			IClientChannelSink nextsink = 
				next.CreateSink(channel,url,remoteChannelData);

			return new ClientSink(nextsink);
		}

	
		/// <MetaDataID>{A1CB4C44-F553-4D4E-B305-81B8521CE591}</MetaDataID>
		public IClientChannelSinkProvider Next
		{
			get { return next; }
			set { next = value; }
		}
		#region ICloneable Members

        /// <MetaDataID>{B27482CA-F4AB-4EBB-98D2-854468C169D0}</MetaDataID>
		public object Clone()
		{
			ClientSinkProvider Clone=new ClientSinkProvider();
			if(next!=null&&next is ICloneable)
				Clone.Next=((next as ICloneable).Clone() as IClientChannelSinkProvider);
			return Clone;
		}

		#endregion
	}
}