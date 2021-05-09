namespace OOAdvantech.PersistenceLayerRunTime
{
	/// <MetaDataID>{BCA073E8-BFE0-45F9-80D3-0C7CA6E371CE}</MetaDataID>
	public class Exceptiona : System.Exception
	{
		/// <MetaDataID>{F34865AF-792C-446E-8C32-8AB21A27C4A9}</MetaDataID>
	
	
		/// <summary>dfg</summary>
		/// <MetaDataID>{78D407E7-7657-427C-86E7-AB42262C09F0}</MetaDataID>
		 public Exceptiona(string message, long errorID):base(message)
		{
			//:base(message)
			

		}
		/// <MetaDataID>{6FD4ADBD-F082-40B0-B890-70B2CCAEA441}</MetaDataID>
		 public Exceptiona(string message, long errorID, System.Exception inerrException):base(message,inerrException)
		{
			//:base(message,inerrException)
			

		}
		/// <MetaDataID>{F218E7DF-E354-4EED-AF8C-D53C52140C1A}</MetaDataID>
		public long ErrorID;

		
	}
}
