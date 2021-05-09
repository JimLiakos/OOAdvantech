using System;
using System.Collections;
using System.Runtime.Remoting.Channels;

namespace OOAdvantech.Transactions.Sinks
{
	
	/// <MetaDataID>{91304D95-88DD-48A1-B1CA-7ECBACCDA88C}</MetaDataID>
	internal class ServerSinkProvider: IServerChannelSinkProvider,ICloneable 
	{
	
		/// <MetaDataID>{50D3B3C8-8E6B-4401-868D-C493873F3A86}</MetaDataID>
		private IServerChannelSinkProvider next = null;

	
		

	
		/// <MetaDataID>{2E59D8AC-8ECA-4346-86A3-3B85B5D9626D}</MetaDataID>
		public void GetChannelData (IChannelDataStore channelData)
		{
			// not needed
		}

	
		/// <MetaDataID>{BC6CA8C1-E0E5-4D12-A055-811ADB13AA50}</MetaDataID>
		public IServerChannelSink CreateSink (IChannelReceiver channel)
		{

			

			IServerChannelSink nextSink = next.CreateSink(channel);
			return new ServerSink(nextSink);
		}

	
		/// <MetaDataID>{0FCCAED3-6228-470C-BF50-A5F0436B2715}</MetaDataID>
		public IServerChannelSinkProvider Next
		{
			get { return next; }
			set { next = value; }
		}
		#region ICloneable Members

        /// <MetaDataID>{61633BA2-3A29-498C-B18F-66EC79ED9CD3}</MetaDataID>
		public object Clone()
		{
			ServerSinkProvider Clone=new ServerSinkProvider();
			if(next!=null&&next is ICloneable)
				Clone.Next=((next as ICloneable).Clone() as IServerChannelSinkProvider);
			return Clone;
		}

		#endregion
	}
}