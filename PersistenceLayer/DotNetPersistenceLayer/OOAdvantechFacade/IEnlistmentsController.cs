using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Transactions
{

    public delegate void TransactionEndedEventHandler(TransactionStatus transactionState);
    /// <MetaDataID>{e07fd05d-b990-4025-8b4e-39915be8efc5}</MetaDataID>
    public interface IEnlistmentsController
    {
           
        /// <MetaDataID>{37395a2a-6b52-4d91-ba30-482855049b99}</MetaDataID>
        void EnlistObjectsStateManager(ObjectsStateManager objectsStateManager);
        /// <MetaDataID>{9cb7991d-871b-4e7c-9850-cc5efc0c5822}</MetaDataID>
        void DetatchEnlistments();
        event TransactionEndedEventHandler TransactionCompletted;
        /// <MetaDataID>{7f1a7585-ce2e-4e8e-a227-9803a3f73b27}</MetaDataID>
        ObjectsStateManagerStatus AbortRequest();
        /// <MetaDataID>{da7fddcc-eadd-46b8-bfdb-1b258c37da34}</MetaDataID>
        ObjectsStateManagerStatus AbortRequest(System.Exception exception);
        /// <MetaDataID>{24e649e4-48b0-4a94-b6fb-f20632fb6a94}</MetaDataID>
        ObjectsStateManagerStatus PrepareRequest();
        /// <MetaDataID>{dc4c1542-8901-4f4b-a0d9-667db66558a3}</MetaDataID>
        ObjectsStateManagerStatus CommitRequest();
        /// <MetaDataID>{5d89e6b9-e748-4dc7-9712-bbba3d953cac}</MetaDataID>
        void AttachToNativeTransaction(System.Transactions.Transaction nativeTransaction);

        ITransactionCoordinator TransactionInitiator
        {
            get;
            set;
        }
        bool StillActive
        {
            get;
        }
        ObjectsStateManagerStatus Status
        {
            get;
        }
        object NativeTransaction
        {
            get;
        }
        System.DateTime LastStatusChangeTimestamp
        {
            get;
        }
        string GlobalTransactionUri
        {
            get;
            set;
        }

        bool HasChangesOnElistedObjects(bool checkOnlyPersistentClassInstances);

    }
}
