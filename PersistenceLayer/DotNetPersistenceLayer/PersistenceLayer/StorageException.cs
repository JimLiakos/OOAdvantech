using System;

namespace OOAdvantech.PersistenceLayer 
{
	/// <summary>
	/// </summary>
	/// <MetaDataID>{9CA08110-B435-4511-9EF9-3514984C1DC9}</MetaDataID>
	[Serializable] 
	public class StorageException: System.Exception
	{
		public enum ExceptionReason
		{
			Unknown=0,
			StorageDoesnotExist,
			CanotConnectWithStorage,
			StorageProviderError,
            StorageAlreadyExist,
            StorageMetadataDotNetTypeMismatch,
			StorageOpensOnlyInSingleObjectContext

        }

		/// <MetaDataID>{3A19C33F-101E-4FD8-B277-88470102E6D7}</MetaDataID>
		protected  StorageException()
		{

		}
#if !DeviceDotNet 
		/// <MetaDataID>{E0A6CBB2-7251-4FC7-A692-3B9ED7C06067}</MetaDataID>
		protected  StorageException( System.Runtime.Serialization.SerializationInfo info , System.Runtime.Serialization.StreamingContext context ):base(info,context) 
		{

			Reason=(ExceptionReason)info.GetValue("Reason",typeof(ExceptionReason));
		}

		/// <MetaDataID>{D561143E-616C-4D4E-8925-844C3FE4041E}</MetaDataID>
		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue("Reason",Reason);
		}
#endif
		/// <summary>dfg</summary>
		/// <MetaDataID>{78D407E7-7657-427C-86E7-AB42262C09F0}</MetaDataID>
		public StorageException(string message, ExceptionReason reason):base(message)
		{
			Reason=reason;
			//:base(message)
			

		}
		/// <MetaDataID>{6FD4ADBD-F082-40B0-B890-70B2CCAEA441}</MetaDataID>
		public StorageException(string message, ExceptionReason reason, System.Exception inerrException):base(message,inerrException)
		{
			Reason=reason;

		}
		/// <MetaDataID>{F218E7DF-E354-4EED-AF8C-D53C52140C1A}</MetaDataID>
		public ExceptionReason Reason=ExceptionReason.Unknown;
	}
}
