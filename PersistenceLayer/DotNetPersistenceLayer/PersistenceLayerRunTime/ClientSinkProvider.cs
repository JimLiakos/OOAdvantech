using System;
using System.Collections;
using System.Runtime.Remoting.Channels;

namespace PersistenceLayerRunTime.Remoting.Sinks
{
	/// <MetaDataID>{81F590D4-C07C-4c9f-B886-FD01DEA28C61}</MetaDataID>
	public class ClientSinkProvider: IClientChannelSinkProvider 
	{

		/// <MetaDataID>{F365C0C1-5434-401F-98F6-6514BBE57246}</MetaDataID>
		private IClientChannelSinkProvider next = null;

		
		/// <MetaDataID>{D54DF695-5DFF-4D95-84F5-648F4E3F5E3F}</MetaDataID>
		public ClientSinkProvider(IDictionary properties, 
			ICollection providerData)
		{
			// not needed
		}

		/// <MetaDataID>{1BD3A2D8-4F56-440D-90DA-19FEEEFBAE6B}</MetaDataID>
		public IClientChannelSink CreateSink(IChannelSender channel, 
			string url, object remoteChannelData)
		{
			IClientChannelSink nextsink = 
				next.CreateSink(channel,url,remoteChannelData);

			return new ClientSink(nextsink);
		}

		/// <MetaDataID>{02B5129F-3137-436F-AFBA-4CE728318A2A}</MetaDataID>
		public IClientChannelSinkProvider Next
		{
			get { return next; }
			set { next = value; }
		}
	}
}