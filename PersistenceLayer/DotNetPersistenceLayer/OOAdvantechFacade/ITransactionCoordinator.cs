using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Transactions
{
    /// <MetaDataID>{c18e1d27-ea54-4b0d-86c5-881b41085b75}</MetaDataID>
    public interface ITransactionCoordinator
    {
        /// <MetaDataID>{cecb08d2-b14f-4e0f-a4e8-6d0c9b2391de}</MetaDataID>
        void InitProcessConnection();
        /// <MetaDataID>{5736bfe9-6710-4453-8b10-f494d7121948}</MetaDataID>
        bool CommunicationCheck(string globalTransactionUri);
        /// <MetaDataID>{e3236f90-c5c6-42f9-a88a-e5aa280c0c96}</MetaDataID>
        void Enlist(ObjectsStateManager objectsStateManager, ref string globalTransactionUri);
        /// <MetaDataID>{2467a9c3-6175-42ad-8df3-47be753d203f}</MetaDataID>
        void ExportAsNestedTransaction(System.TimeSpan timeSpan, int initiatorProcessID, string originTransactionGlobalUri, ObjectsStateManager objectsStateManager, System.Transactions.Transaction nativeTransaction, ITransactionStatusNotification transactionStatusNotification, out string globalTransactionUri, out IEnlistmentsController enlistmentsController);

        void TransactionImport(string globalTransactionUri, ITransactionStatusNotification transactionStatusNotification);

        void ReOrderDerivedTransactions(ref string globalTransactionUri);

        void ExportTransaction(System.TimeSpan timeSpan, int initiatorProcessID, ObjectsStateManager objectsStateManager, System.Transactions.Transaction nativeTransaction, ITransactionStatusNotification transactionStatusNotification, out string globalTransactionUri, out IEnlistmentsController enlistmentsController);

        string AttachToNativeTransaction(System.Transactions.Transaction nativeTransaction);

        void Abort(string globalTransactionUri, System.Exception exception);

        Transactions.ITransactionCoordinator EnlistToRoot(IEnlistmentsController remoteEnlistmentsController, ref string globalTransactionUri);
         
       
    }
}
