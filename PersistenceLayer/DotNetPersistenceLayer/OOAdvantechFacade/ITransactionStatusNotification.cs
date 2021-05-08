using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Transactions
{
    /// <summary>
    /// Describes the current status of a distributed transaction.
    /// </summary>
    /// <MetaDataID>{40af26dc-bede-40e5-bb0b-61f4fcfa15fb}</MetaDataID>
    public enum TransactionStatus
    {
        /// <summary>
        ///The status of the transaction is unknown, because some participants must
        ///still be polled.
        /// </summary>
        Continue = 0,

        /// <summary>
        /// The transaction has been committed.
        /// </summary>
        Committed = 1,

        /// <summary>
        /// The transaction has been rolled back.
        /// </summary>
        Aborted = 2,


        /// <summary>
        /// The status of the transaction is unknown.
        /// </summary>
        InDoubt = 3,
    }
    /// <MetaDataID>{E910EF37-6A06-405D-B2CF-09C9F8B643F2}</MetaDataID>
    public interface ITransactionStatusNotification
    {
        //[System.Runtime.Remoting.Messaging.OneWay]
        /// <MetaDataID>{358E7134-C8E4-448C-A29F-AE413E3D17B9}</MetaDataID>
        void OnTransactionCompletted(OOAdvantech.Transactions.TransactionStatus transactionState);
        /// <MetaDataID>{BCC372AF-DAFE-461C-8B80-DD5924566B5A}</MetaDataID>
        void AttachToNativeTransaction(object nativeTransacion);
    }
}
