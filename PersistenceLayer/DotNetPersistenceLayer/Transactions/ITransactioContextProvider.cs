namespace OOAdvantech.Transactions
{
    /// <MetaDataID>{398995B6-CF82-4D81-9CE3-721D1BFBF30E}</MetaDataID>
    /// <summary>
    /// Through this interface implementation subsystems can provide class factory 
    /// which instantiate the transaction context extender for a specific transaction.
    /// The transaction context extender providers register to the transaction system 
    /// through the RegisterTransactionContextProvider of transaction manager class. 
    /// </summary>
	public interface ITransactionContextProvider
	{

        /// <MetaDataID>{D5434704-6543-45F2-AF96-30C230EB5A91}</MetaDataID>
        /// <summary>
        /// This method returns a transaction context extender 
        /// for the transaction of transaction parameter. 
        /// </summary>
        /// <param name="transaction">
        /// This parameter defines the transaction of transaction context extender. 
        /// </param>
        ITransactionContextExtender CreateTransactionContext(Transaction transaction);
	}
}
