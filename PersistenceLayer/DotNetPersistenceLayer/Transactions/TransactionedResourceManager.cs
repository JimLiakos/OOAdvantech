namespace OOAdvantech.Transactions
{

#if DISTRIBUTED_TRANSACTIONS
    using OneWayAttribute = System.Runtime.Remoting.Messaging.OneWayAttribute;
#else
    /// <MetaDataID>{632b8b71-d200-4e5d-bc17-25829b6309e7}</MetaDataID>
    class OneWay:System.Attribute
    {
    }
#endif

    internal delegate void ActionRequest(ITransactionEnlistment transactionEnlistment);


    /// <MetaDataID>{FEFE5138-1B9A-40F5-8832-7AF9A4BE2ADA}</MetaDataID>
    /// <summary>Define the interface must be implemented from objects state managers to participate in transaction</summary>
	internal interface ObjectsStateManager
	{
		/// <summary>Transaction manager inform that the transaction aborted and ask from objects state manager to roll back the state of objects.</summary>
		/// <MetaDataID>{D6288EE3-B1D1-41C1-8852-B0E42C34181B}</MetaDataID>
		//[OneWay]
        void AbortRequest(ITransactionEnlistment transactionEnlistment);
		/// <summary>Transaction manager inform that the transaction committed and ask from objects state manager to release objects from transaction lock. This is phase two of the two-phase commit protocol.</summary>
		/// <MetaDataID>{774B83D6-5BD8-461D-92AF-8F2F5792CE1A}</MetaDataID>
		//[OneWay]
        void CommitRequest(ITransactionEnlistment transactionEnlistment);
        ///// <summary>Transaction manager ask from objects state manager to make all needed actions in order that the objects to be in a consistent state and durable for the persistent objects. This is phase two of the two-phase commit protocol.</summary>
        ///// <MetaDataID>{73BE4F25-51FF-4A48-B44C-3DF00467BC4A}</MetaDataID>
        ////[OneWay]
        //void PrepareRequest(ITransactionEnlistment transactionEnlistment);

        /// <summary>Transaction manager ask from objects state manager to make all needed actions in order that the objects to be in a consistent state and durable for the persistent objects. This is phase two of the two-phase commit protocol.</summary>
        /// <MetaDataID>{03C7CC4C-18C3-4012-853A-97E355C3581F}</MetaDataID>
       // [OneWay]
        void PrepareRequest(ITransactionEnlistment transactionEnlistment);
       

		
	}
}
