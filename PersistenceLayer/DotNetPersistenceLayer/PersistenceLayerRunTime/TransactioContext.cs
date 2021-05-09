namespace  OOAdvantech.PersistenceLayerRunTime
{
	///<summary>
	///TransactionContextProvider class is a Factory Class
	///Provides to the transaction system a transaction contentext extension
	///</summary>
	/// <MetaDataID>{46D14DB9-059D-424B-8239-51C5D56AFBEB}</MetaDataID>
	public class TransactioContextProvider:Transactions.ITransactioContextProvider
	{
		/// <MetaDataID>{EC2DCE07-AB97-4B46-9ACD-0AAF616C4114}</MetaDataID>
		public OOAdvantech.Transactions.ITransactionContextExtender CreateTransactionContext(Transactions.Transaction transaction)
		{
			return new  TransactionContext(transaction);
		}
	}
}
