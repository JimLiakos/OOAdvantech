namespace OOAdvantech.Transactions
{
    /// <summary>
    ///  The implemation of this intarface from a class means that 
    ///  the instances of class manipulates their state in a transaction context.
    /// </summary>
    /// <MetaDataID>{F7FFDD8E-FAB3-4D6A-B07F-CE0AB7E82C7F}</MetaDataID>
    public interface ITransactionalObject
    {
        ///<summary>
        ///The MergeChanges called when a nested transaction committed. 
        ///The instance must be transfer the changes in master transaction if use the (record change style) 
        ///or transfer origin state of nested transaction to the master transaction if the there isn’t origin state for it, 
        ///in case where use (mark origin state style).
        ///</summary>
        ///<param name="masterTransaction">
        ///Difines the master transaction.
        ///</param>
        ///<param name="nestedTransaction">
        ///Difines the nested committed transaction.
        ///</param>
        /// <MetaDataID>{8AAC87FB-2652-437D-80AA-B7E343434038}</MetaDataID>
        void MergeChanges(Transaction masterTransaction, Transaction nestedTransaction);
        ///<summary>
        ///The MarkChanges called when the instance enlisted in transaction. 
        ///</summary>
        ///<param name="transaction">
        ///This parameter defines the transaction where the instance enlisted.
        ///The instance mark the original state
        ///</param>
        /// <MetaDataID>{CE2FC4E9-1356-4EA2-9758-ABC7AD7632F4}</MetaDataID>
        void MarkChanges(Transaction transaction);
        /// <summary>
        /// The MarkChanges called when the instance enlisted partially in transaction. 
        /// The instance mark the original state for the fields of fiels parameter.
        ///  </summary>
        /// <param name="transaction">
        /// This parameter defines the transaction where the instance enlisted partially
        ///  </param>
        /// <param name="fields">
        ///  Defines the fields where the instance mark the origin state
        ///  </param>
        /// <MetaDataID>{27bf615a-2f2e-46df-8851-fb3f317eef22}</MetaDataID>
        void MarkChanges(Transaction transaction, System.Reflection.FieldInfo[] fields);
        ///<summary>
        ///The UndoChanges called when the transaction rollback the instance must be returns 
        ///to the original state. 
        ///</summary>
        ///<param name="transaction">
        ///Defines the rollback transaction.
        ///</param>
        /// <MetaDataID>{EBB9E78C-DB9E-44FF-AA32-910F7B835C61}</MetaDataID>
        void UndoChanges(OOAdvantech.Transactions.Transaction transaction);
        ///<summary>
        ///The CommitChanges called when the transaction committed the instance can drop the original state data. 
        ///</summary>
        ///<param name="transaction">
        ///Defines the committed transaction.
        ///</param>
        /// <MetaDataID>{C131BF2B-D514-41A6-86E9-9D4F7AEFDF6C}</MetaDataID>
        void CommitChanges(Transaction transaction);


        /// <MetaDataID>{15d8b33d-e72a-4f2a-9949-135de8af945a}</MetaDataID>
        void EnsureSnapshot(Transaction transaction);
        /// <MetaDataID>{5dc722f2-601c-4e69-ace7-eaabecd3fb7a}</MetaDataID>
        bool ObjectHasGhanged(TransactionRunTime transaction);
    }

    /// <MetaDataID>{a7452d69-a2e3-4f67-ad62-136f0bc62dc1}</MetaDataID>
    public interface ITransactionNotification
    {
        void OnTransactionCompletted(Transaction transaction);
    }
}
