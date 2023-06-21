namespace OOAdvantech.Remoting
{
	/// <MetaDataID>{A5F9E40D-4D3F-408D-93E4-6B49BE55403B}</MetaDataID>
	public class RemotingException:System.Exception
	{
		public enum ExceptionCause
		{
			CannotReconnect =0,
			ParameterLostConnection,
			ExceptionCatcher,
			MissingServerObject
			
		}
		/// <MetaDataID>{C05F05AC-6CD0-4E47-AD1E-02A9FF42D5C2}</MetaDataID>
		public RemotingException (ExceptionCause exceptionCause,System.String message , System.Exception innerException ):base(message,innerException)
		{
			RemotingExceptionCause=exceptionCause;
		}
		/// <MetaDataID>{65799150-2AA2-44E3-A825-F3311B103350}</MetaDataID>
		public RemotingException (ExceptionCause exceptionCause,System.String message ):base(message)
		{
			RemotingExceptionCause=exceptionCause;
		}
		/// <MetaDataID>{377AB056-5B37-4047-B675-437C36D8D644}</MetaDataID>
		public RemotingException (ExceptionCause exceptionCause):base()
		{
			RemotingExceptionCause=exceptionCause;
		}
		/// <MetaDataID>{66CCE606-4F27-422E-8AFC-248D240020FA}</MetaDataID>
		public ExceptionCause RemotingExceptionCause;
	}




	/// <MetaDataID>{595c337d-cfce-4795-9dcb-5d5efa9c7a98}</MetaDataID>
	public class MissingServerObjectException : System.Exception
	{
		public MissingServerObjectException(string message, MissingServerObjectReason reason) : base(message)
		{
			Reason = reason;
		}
		public readonly MissingServerObjectReason Reason;

		public enum MissingServerObjectReason
		{
			CollectedFromGC,
			DeletedFromStorage
		}
	}

	
}
