namespace OOAdvantech.Transactions
{
    /// <MetaDataID>{E7D09B5C-3C3F-4BDA-AD45-22834BFAC4B6}</MetaDataID>
    /// <summary>T
    /// hrough this interface the transaction system propagate the two phase commit mechanism.   
    /// The transaction context extender classes must implement this interface.
    /// An example of transaction context extender is the extender for persistent object. 
    /// This context extender ensures that the state of persistent object is durable after 
    /// the transaction commit. </summary>
    public interface ITransactionContextExtender
    { 
        /// <summary>
        /// Raise enlistment event to the extender of transaction context. 
        /// The extender of transaction context must implement this operation to act when enlistment happen.
        /// </summary>
        /// <MetaDataID>{2E8DA797-86DF-4876-AE61-221C04B1F405}</MetaDataID>
        void EnlistObject(object TransactionedObject, System.Reflection.FieldInfo fieldInfo);
        /// <summary>
        /// Raise abort event to the extender of transaction context. 
        /// The extender of transaction context must implement this operation to act when transaction aborted. 
        /// </summary>
        /// <MetaDataID>{CEFF9E1E-DFDC-48A3-9201-82D4579FD493}</MetaDataID>
        void Abort();

        /// <summary>
        /// Raise commit event to the extender of transaction context. 
        /// The extender of transaction context must implement this operation 
        /// to act when transaction committed happen. 
        /// </summary>
        /// <MetaDataID>{5E47411A-A0B2-47C6-8672-44ACB1A04C9C}</MetaDataID>
        void Commit();

        /// <summary>
        /// Raise prepare event to the extender of transaction context. 
        /// The extender of transaction context must implement this operation 
        /// to act when prepare happen. 
        /// </summary>
        /// <MetaDataID>{DE88E23B-2D82-4F02-8508-45FC95F3B490}</MetaDataID>
        void Prepare();
    }
}
