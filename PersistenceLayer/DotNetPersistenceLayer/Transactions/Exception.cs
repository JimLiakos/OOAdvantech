using System;

namespace OOAdvantech.Transactions
{
    /// <summary>
    /// </summary>
    /// <MetaDataID>{AD34B86A-BD81-44C8-A90F-F42055ECE43A}</MetaDataID>
	[Serializable] 
	public class AbortException:System.Exception
    {

#if DISTRIBUTED_TRANSACTIONS
        /// <MetaDataID>{22D53BC3-F398-46B1-8AE9-33E0F2C41549}</MetaDataID>
        AbortException( System.Runtime.Serialization.SerializationInfo info , System.Runtime.Serialization.StreamingContext context ):base(info,context) 
		{

            AbortReasons = info.GetValue("AbortReasons", typeof(OOAdvantech.Collections.Generic.List<System.Exception>)) as OOAdvantech.Collections.Generic.List<System.Exception>;
            

		}
        /// <MetaDataID>{29EA228D-AF6B-447E-A9A5-3E06AE575437}</MetaDataID>
		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue("AbortReasons",AbortReasons);
		}
#endif



        /// <MetaDataID>{E464C281-A267-4DB8-892E-CED32578AA4F}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<System.Exception> AbortReasons = null;
        /// <MetaDataID>{572F4B93-AE8D-49E5-AC46-E33DF263E62C}</MetaDataID>
        public AbortException(OOAdvantech.Collections.Generic.List<System.Exception> abortReasons, System.String message, System.Exception innerException)
            : base(message, innerException)
		{
			AbortReasons=abortReasons;
		}

        /// <MetaDataID>{C0EF2EED-C0B8-4608-9FEB-E9099FCF10C2}</MetaDataID>
        public AbortException(OOAdvantech.Collections.Generic.List<System.Exception> abortReasons, System.String message)
            : base(message)
		{
			AbortReasons=abortReasons;
		}

        /// <MetaDataID>{9E09B7DB-4FD3-48DA-98B4-82DBFC8F3511}</MetaDataID>
        public AbortException(OOAdvantech.Collections.Generic.List<System.Exception> abortReasons)
            : base()
		{
			AbortReasons=abortReasons;
		}
        /// <MetaDataID>{500D87BB-C04C-42FE-9193-C19A27712F30}</MetaDataID>
        public AbortException()
		{

		}
	}

    /// <MetaDataID>{a13d827c-8010-4741-8135-2df91fd799b5}</MetaDataID>
    [Serializable]
    public class TransactionException : System.Exception
    {

#if DISTRIBUTED_TRANSACTIONS
        TransactionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
#endif



        public OOAdvantech.Collections.Generic.List<System.Exception> AbortReasons = null;

        public TransactionException(System.String message, System.Exception innerException)
            : base(message, innerException)
        {

        }

        public TransactionException(System.String message)
            : base(message)
        {

        }
        public TransactionException()
        {

        }
    }


}
