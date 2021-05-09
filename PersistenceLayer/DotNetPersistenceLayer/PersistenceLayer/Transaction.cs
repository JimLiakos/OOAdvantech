namespace OOAdvantech.PersistenceLayer
{
	/// <metadataid>{C0BB779C-2EEC-49D0-AA99-41B4A6298099}</metadataid>
	public class Transaction
	{
		public enum OnObjectChangeSate{NotSupported=0,Supported,Required,RequiresNew};
		public enum TransactionState{Commited=0,Aborted,Continue};

	}
}
