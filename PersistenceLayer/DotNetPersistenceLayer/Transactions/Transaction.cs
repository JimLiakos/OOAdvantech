using System.Security.Principal;
using System.Threading.Tasks;
using OOAdvantech.Remoting;
using System;
#if DeviceDotNet
using System.Reflection;
using System.PCL.Reflection;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#endif
namespace OOAdvantech.Transactions
{
    /// <summary>Specifies the automatic transaction type requested by the component.</summary>
    /// <MetaDataID>{6bc6bc27-3354-488b-8d2f-796c2be666d2}</MetaDataID>
	public enum TransactionOption
    {
        /// <summary>
        /// The ambient transaction context is suppressed when creating the scope. 
        /// Alloperations within the scope are done without an ambient transaction contex
        ///</summary>
        Suppress = 0,
        NotSupported = 0,
        ///<summary>Shares a transaction, if one exists.</summary>
        Supported,
        /// <summary>Shares a transaction, if one exists, and creates a new transaction, if necessary.</summary>
        Required,
        /// <summary>Creates a new transaction, regardless of the state of the current context.</summary>
        RequiresNew,
        /// <summary>Creates a new transaction that is nested in transaction if one exists, 
        /// if the parent aborted then aborted and nested transaction automatically, 
        /// if nested aborted doesn't means that the parent also must be aborted.</summary>
        RequiredNested

    };



    ///// <summary>
    ///// Describes the current status of a distributed transaction.
    ///// </summary>
    ///// <MetaDataID>{40af26dc-bede-40e5-bb0b-61f4fcfa15fb}</MetaDataID>
    //public enum TransactionStatus
    //{
    //    /// <summary>
    //    ///The status of the transaction is unknown, because some participants must
    //    ///still be polled.
    //    /// </summary>
    //    Continue = 0,

    //    /// <summary>
    //    /// The transaction has been committed.
    //    /// </summary>
    //    Committed = 1,

    //    /// <summary>
    //    /// The transaction has been rolled back.
    //    /// </summary>
    //    Aborted = 2,


    //    /// <summary>
    //    /// The status of the transaction is unknown.
    //    /// </summary>
    //    InDoubt = 3,
    //}


    ///// <MetaDataID>{E910EF37-6A06-405D-B2CF-09C9F8B643F2}</MetaDataID>
    //internal interface ITransactionStatusNotification
    //{
    //    //[System.Runtime.Remoting.Messaging.OneWay]
    //    /// <MetaDataID>{358E7134-C8E4-448C-A29F-AE413E3D17B9}</MetaDataID>
    //    void OnTransactionCompletted(OOAdvantech.Transactions.TransactionStatus transactionState);
    //    /// <MetaDataID>{BCC372AF-DAFE-461C-8B80-DD5924566B5A}</MetaDataID>
    //    void AttachToNativeTransaction(object nativeTransacion);
    //}
    /// <MetaDataID>{CDA1CBD9-2776-4FD4-980D-4E197A3DE817}</MetaDataID>
    internal class TransactionStatusNotifier : MarshalByRefObject, ITransactionStatusNotification, Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{5BE3199C-CAAC-455B-8411-76C8E69F6012}</MetaDataID>
        internal readonly ITransactionStatusNotification TransactionStatusNotification;
        /// <MetaDataID>{AC7FFA63-708D-4AAE-9EE9-FD8B667BF73A}</MetaDataID>
        public TransactionStatusNotifier(ITransactionStatusNotification transactionStatusNotification)
        {
            TransactionStatusNotification = transactionStatusNotification;
            if (TransactionStatusNotification == null)
                throw new System.ArgumentNullException("transactionStatusNotification");
        }
        /// <MetaDataID>{60b1c86b-3903-4aa0-8f7b-b8388530af5c}</MetaDataID>
        bool Stopped = false;
        /// <MetaDataID>{6922e344-0007-4bd5-849d-2827260d9d52}</MetaDataID>
        public void StopNotification()
        {
            Stopped = true;
        }


#region ITransactionStatusNotification Members

        /// <MetaDataID>{F40D5B9E-BE47-449D-85B1-6FEEDE807673}</MetaDataID>
        void ITransactionStatusNotification.OnTransactionCompletted(TransactionStatus transactionState)
        {
            if (!Stopped)
                TransactionStatusNotification.OnTransactionCompletted(transactionState);
        }

        /// <MetaDataID>{06FDCC6D-5C52-492D-B2A5-D8F5AD4E3C5C}</MetaDataID>
        void ITransactionStatusNotification.AttachToNativeTransaction(object nativeTransacion)
        {
            if (!Stopped)
                TransactionStatusNotification.AttachToNativeTransaction(nativeTransacion);
        }

#endregion
    }


    /// <MetaDataID>{D6FBB762-ED64-4FE8-B785-6E854B9D054E}</MetaDataID>
    /// <summary>
    /// Represents a transaction.
    /// </summary>
    public abstract class Transaction : OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{f6edbfe5-6340-4a71-ae97-7353f4b8662b}</MetaDataID>
        public abstract bool IsNestedTransaction(Transaction transaction);
        delegate void ExecuteUnderTransactionHandle(System.Delegate method, Transaction transaction, params object[] args);

        /// <MetaDataID>{645949dd-fc18-4f6c-95a3-f59b9b87736d}</MetaDataID>
        public static void RunAsynch(System.Delegate method, params object[] args)
        {
            OOAdvantech.Transactions.Transaction transaction = OOAdvantech.Transactions.Transaction.Current;
            object[] _params = args;
            var culture = CultureContext.CurrentCultureInfo;
            var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
            Task.Factory.StartNew(() => {
                using (CultureContext cultureContext = new CultureContext(culture, useDefaultCultureValue))
                {
                    ExecuteUnderTransaction(method, transaction, _params);
                }
            });
        }

        /// <MetaDataID>{aa0697ea-2bd9-475c-872b-ae11e3136ccd}</MetaDataID>
        static void ExecuteUnderTransaction(System.Delegate method, Transaction transaction, object[] args)
        {
            if (transaction != null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                {
#if DeviceDotNet
                    method.GetMethodInfo().Invoke(method.Target, args);
#else
                    method.Method.Invoke(method.Target, args);
#endif
                    stateTransition.Consistent = true;
                }
            }
            else
            {
#if DeviceDotNet
                method.GetMethodInfo().Invoke(method.Target, args);
#else
                method.Method.Invoke(method.Target, args);
#endif
            }
        }

#if !DeviceDotNet
        /// <MetaDataID>{ba52ac74-821e-4077-bb0f-92020c79ca38}</MetaDataID>
        internal readonly WindowsIdentity TransactionInitiatorUser = WindowsIdentity.GetCurrent();
#endif
        ///<summary>
        /// Gets the ambient transaction.
        ///</summary>
        /// <MetaDataID>{FC49D53D-C5D3-494E-BBD5-C0EEF3B9734C}</MetaDataID>
        public static Transaction Current
        {
            get
            {
                return LogicalThread.GetActiveTransaction();
            }
        }
        /// <MetaDataID>{595bde85-2b82-4f20-960c-21778882cca2}</MetaDataID>
        static bool DTCConnected;
        /// <summary>
        /// With PreActivateDTCConnection system establish a connection between Process and DTC
        /// </summary>
        /// <MetaDataID>{93ceb3eb-1aac-4860-b7f2-e5e2d06318b9}</MetaDataID>
        public static void PreActivateDTCConnection()
        {
            if (!DTCConnected)
            {
                InitProcessDTCConnectionHandle dlg = new InitProcessDTCConnectionHandle(InitProcessDTCConnection);
                dlg.BeginInvoke(null, null);
            }

        }
        delegate void InitProcessDTCConnectionHandle();
        /// <MetaDataID>{d013ab94-95bb-4186-b8b9-ac4b39ba8e38}</MetaDataID>
        static void InitProcessDTCConnection()
        {

            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                try
                {
                    OOAdvantech.Transactions.TransactionRunTime.LocalTransactionCoordinator.InitProcessConnection();
                    DTCConnected = true;
                }
                catch (System.Exception error)
                {
                }
                ts.Complete();
            }
        }
        /// <summary>
        /// This property defines the outer transaction of nested transaction.
        /// In case where the transaction isn’t nested the value of this property is null.
        /// </summary>
        /// <MetaDataID>{c1d96c01-45db-46cb-9671-07acce346650}</MetaDataID>
        public abstract Transaction OriginTransaction
        {
            get;
        }
        /// <MetaDataID>{aebe614f-edac-422e-a385-e64b96150cb4}</MetaDataID>
        public abstract System.Collections.Generic.List<Transaction> NestedTransactions
        {
            get;
        }

        /// <MetaDataID>{e2d46971-654f-4c61-b42e-4033c23f44a9}</MetaDataID>
        public override bool Equals(object obj)
        {
            if (obj is Transaction)
            {
                return (obj as Transaction).LocalTransactionUri == LocalTransactionUri;
            }
            else
                return false;
        }
        /// <MetaDataID>{c0b0bec9-164e-42b1-9fbc-fcb4a12efcf1}</MetaDataID>
        public override int GetHashCode()
        {
            if (InternalTransaction != null)

                return InternalTransaction.GetHashCode();
            else
                return base.GetHashCode();
        }


        /// <summary>
        /// Tests whether two specified Transaction instances are
        /// equivalent.
        /// </summary>
        /// <param name="left">
        /// The Transaction instance that is to the left of the equality
        /// operator.
        /// </param>
        /// <param name="right">
        /// The Transaction instance that is to the right of the
        /// equality operator.
        /// </param>
        /// <returns>
        /// true if left and left are equal; otherwise, false.
        /// </returns>
        /// <MetaDataID>{efc62f21-23f1-474e-87f8-684ecd80e6c5}</MetaDataID>
        public static bool operator ==(Transaction left, Transaction right)
        {
            if (!(left is Transaction) && !(right is Transaction))
                return true;
            if (!(left is Transaction) && (right is Transaction))
                return false;
            if ((left is Transaction) && !(right is Transaction))
                return false;
            if (left.Equals(right))// && left.UnitMeasure == right.UnitMeasure)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Tests whether two specified Transaction instances are
        /// equivalent.
        /// </summary>
        /// <param name="left">
        /// The Transaction instance that is to the left of the equality
        /// operator.
        /// </param>
        /// <param name="right">
        /// The Transaction instance that is to the right of the
        /// equality operator.
        /// </param>
        /// <returns>
        /// true if left and left are equal; otherwise, false.
        /// </returns>
        /// <MetaDataID>{4e9f90d6-942a-47ea-81c5-2c3e05c50284}</MetaDataID>
        public static bool operator !=(Transaction left, Transaction right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Indicates that the transaction is completed.
        /// An attempt to subscribe this event on a transaction that has been completed raise exception.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// An attempt to subscribe this event on a transaction that has been completed.
        /// </exception>
        public abstract event OOAdvantech.Transactions.TransactionCompletedEventHandler TransactionCompleted;

        /// <summary>
        /// Indicates that the transaction distributed. 
        /// An attempt to subscribe this event on a transaction that has been completed raise exception.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// An attempt to subscribe this event on a transaction that has been completed.
        /// </exception>
        public abstract event OOAdvantech.Transactions.TransactionDistributedEventHandler TransactionDistributed;


        public abstract bool HasChangesOnElistedObjects(bool checkOnlyPersistentClassInstances = false);
        /// <summary>
        /// Blocks the current thread until the transaction complete, using
        /// 32-bit signed integer to measure the time interval in milliseconds and specifying whether to
        /// exit the synchronization domain before the wait.
        /// </summary>
        /// <param name="timeout">
        /// A 32-bit signed integer that represents the number of milliseconds to wait, or
        /// a 32-bit signed integer with -1 to wait indefinitely.
        /// </param>
        /// <returns>
        /// true if the transaction complete; otherwise, false.
        /// </returns>
        /// <MetaDataID>{3049872a-676e-4917-b8c9-40575e832150}</MetaDataID>
        public abstract bool WaitToComplete(int millisecondsTimeout);
        /// <summary>
        /// Blocks the current thread until the transaction complete, using
        /// a System.TimeSpan to measure the time interval and specifying whether to
        /// exit the synchronization domain before the wait.
        /// </summary>
        /// <param name="timeout">
        /// A System.TimeSpan that represents the number of milliseconds to wait, or
        /// a System.TimeSpan that represents -1 milliseconds to wait indefinitely.
        /// </param>
        /// <returns>
        /// true if the transaction complete; otherwise, false.
        /// </returns>
        /// <MetaDataID>{364ca907-c06b-4cd8-986d-0d299788734a}</MetaDataID>
        public abstract bool WaitToComplete(System.TimeSpan timeout);


        ///<summary>
        ///Gets the status of the transaction.
        ///</summary>
        /// <MetaDataID>{3C7B689E-F121-4AF8-ABCB-89690071D329}</MetaDataID>
        public virtual TransactionStatus Status
        {
            get
            {
                if (InternalTransaction != null)
                    return InternalTransaction.Status;
                else
                    throw new System.NotImplementedException("The method or operation is not implemented.");

            }
        }
        /// <summary>Define the identity of transaction. </summary>
        /// <MetaDataID>{8555EBEA-7771-41A6-BD78-A89B454144CC}</MetaDataID>
        public virtual string LocalTransactionUri
        {

            get
            {
                if (InternalTransaction != null)
                    return InternalTransaction.LocalTransactionUri;
                else
                    throw new System.NotImplementedException("The method or operation is not implemented.");
            }
        }
        /// <summary>Define the identity of the escalated transaction. </summary>
        /// <MetaDataID>{6F918D1A-482B-4262-A49D-F75C431D5D5F}</MetaDataID>
        public virtual string GlobalTransactionUri
        {
            get
            {
                if (InternalTransaction != null)
                    return InternalTransaction.GlobalTransactionUri;
                else
                    throw new System.NotImplementedException("The method or operation is not implemented.");
            }
        }

        /// <MetaDataID>{d8dfb020-9e94-416d-8d7b-f93f8fd52dec}</MetaDataID>
        public bool OpenStateTransitionsInOtherThreads
        {
            get
            {
                return LogicalThread.HasOpenScoopsInOtherThreads(this);
            }
        }

        /// <summary>Aborts the transaction. </summary>
        /// <param name="exception">
        /// An explanation of why a rollback occurred.
        /// </param>
        /// <MetaDataID>{DBCF0A28-BF88-4A56-A315-6A33FF963C86}</MetaDataID>
        public virtual void Abort(System.Exception exception)
        {
            if (InternalTransaction != null)
                InternalTransaction.Abort(exception);
            else
                throw new System.NotImplementedException("The method or operation is not implemented.");
        }


        /// <summary>Aborts the transaction. </summary>
        /// <MetaDataID>{efeacf6a-596a-4f7e-ad81-fbb1180b2bdf}</MetaDataID>
        public virtual void Abort()
        {
            Abort(null);

        }
        /// <MetaDataID>{19CE1759-733B-4E4C-8319-E805AF1AB4E0}</MetaDataID>
        internal protected Transaction InternalTransaction;

        /// <MetaDataID>{9AA56A15-F1BA-42CA-82EC-B6FD00DBE5E9}</MetaDataID>
        internal Transaction(Transaction internalTransaction)
        {
            InternalTransaction = internalTransaction;
        }
        /// <MetaDataID>{B146C53C-0698-41E3-A36B-2F8AD54CE47F}</MetaDataID>
        internal Transaction()
        {
        }
        /// <MetaDataID>{cdf28feb-1dee-4646-bb8b-0656b77115c2}</MetaDataID>
        public abstract string Marshal();
    }


}
