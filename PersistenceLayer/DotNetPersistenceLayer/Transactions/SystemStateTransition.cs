using System;

namespace OOAdvantech.Transactions
{

    /// <summary>
    /// Makes a code block transactional
    /// </summary>
    /// <MetaDataID>{69770F13-5E1F-4b18-A2EB-6A0B30C2B42D}</MetaDataID>
    public sealed class SystemStateTransition : System.IDisposable
    {
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{419526D9-D4EB-4759-9284-307168A1513B}</MetaDataID>
        private bool _Consistent = false;
        /// <summary>
        /// Indicates that all operations within the scope are completed successfully.
        /// </summary>
        /// <MetaDataID>{4296D484-D8FA-4CED-B1F0-4269E0BA3146}</MetaDataID>
        public bool Consistent
        {
            set
            {
                _Consistent = value;
            }
            get
            {
                return _Consistent;
            }
        }

        /// <MetaDataID>{c1f99031-d8ce-484e-b727-4656ee14cda8}</MetaDataID>
        internal LogicalThread LogicalThread;

        bool _Completed = false;

        internal bool Completed
        {
            get
            {
                lock(this)
                {
                    return _Completed;
                }
            }
        }
        internal void StateTransitionCompleted()
        {
            lock (this)
            {
                LogicalThread = null;
                _Completed = true;
            }
        }





        ///<summary>
        ///Initializes a new instance of the SystemStateTransition class with the specified requirements
        ///</summary>
        ///<param name="transactionOption">
        ///This parameter defines the transactional behavior of the scope
        ///</param>
        /// <MetaDataID>{7D158754-AFCA-41BE-B7BE-A5E774D8AB75}</MetaDataID>
        public SystemStateTransition(TransactionOption transactionOption)
        {
            //EntryCodeCookie = new OOAdvantech.Transactions.LogicalThread.EntryCodeCookie(2);
            //TODO είναι χρήσιμο πληροφορία για την method "HasOpenScoops" της class LogicalThread αλλά καθυστερεί το προγραμμα 
            if (GetType() == typeof(SystemStateTransition))
                IinitializeTransaction(transactionOption);
        }

        ///<summary>
        ///Initializes a new instance of the SystemStateTransition class and sets 
        ///the specified transaction as the ambient transaction, 
        ///so that transactional work done inside the scope uses this transaction. 
        ///</summary>
        ///<param name="transaction">
        ///The transaction to be set as the ambient transaction, 
        ///so that transactional work done inside the scope uses this transaction.
        ///</param>
        /// <MetaDataID>{071C65E4-C37B-46E4-9D4B-B7D011620145}</MetaDataID>
        public SystemStateTransition(Transaction transaction)
        {
            //EntryCodeCookie = new OOAdvantech.Transactions.LogicalThread.EntryCodeCookie(2);
            //TODO είναι χρήσιμο πληροφορία για την method "HasOpenScoops" της class LogicalThread αλλά καθυστερεί το προγραμμα 
            if (transaction.InternalTransaction != null)
                transaction = transaction.InternalTransaction;

            Transaction ActiveTransaction = LogicalThread.GetActiveTransaction();
            if (ActiveTransaction != null && ActiveTransaction != transaction &&
                !(ActiveTransaction as TransactionRunTime).IsNestedTransaction(transaction as TransactionRunTime))
                throw new OOAdvantech.Transactions.TransactionException("LogicalThread has already transaction");
            LogicalThread.InitSystemStateTransitionWithTransaction(transaction, this);

        }

        ///<summary>
        ///Initializes a new instance of the SystemStateTransition class. 
        ///</summary>
        /// <MetaDataID>{41F18CBD-91B6-4582-BA48-44EEEBEF6F1A}</MetaDataID>
        public SystemStateTransition()
        {

            //EntryCodeCookie = new OOAdvantech.Transactions.LogicalThread.EntryCodeCookie(2);
            //TODO είναι χρήσιμο πληροφορία για την method "HasOpenScoops" της class LogicalThread αλλά καθυστερεί το προγραμμα 

            if (GetType() == typeof(SystemStateTransition))
            {
                IinitializeTransaction(TransactionOption.Required);
            }
        }

        /// <MetaDataID>{b0ddcd6d-17e1-4037-8995-252871af7db7}</MetaDataID>
        internal protected TimeSpan ObjectEnlistmentTimeOut = TimeSpan.Zero;

        /// <MetaDataID>{d3afc2c8-475d-43f7-804a-ed5d86eeae18}</MetaDataID>
        public SystemStateTransition(TimeSpan transactionObjectLockTimeOut)
            : this()
        {
            if (transactionObjectLockTimeOut == TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("transactionObjectLockTimeOut");

            ObjectEnlistmentTimeOut = transactionObjectLockTimeOut;
        }
        /// <MetaDataID>{1eb5e3b9-9e31-4d59-8080-d145057a6338}</MetaDataID>
        public SystemStateTransition(Transaction transaction, TimeSpan transactionObjectLockTimeOut)
            : this(transaction)
        {
            if (transactionObjectLockTimeOut == TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("transactionObjectLockTimeOut");

            ObjectEnlistmentTimeOut = transactionObjectLockTimeOut;

        }
        /// <summary></summary>
        /// <param name="transactionOption"></param>
        /// <param name="transactionObjectLockTimeOut">
        /// Defines the time we want system wait an other transaction to release the object in case of transaction object lock;
        /// </param>
        /// <MetaDataID>{cfb522d8-4fd9-4384-ac9d-0b43968ab995}</MetaDataID>
        public SystemStateTransition(TransactionOption transactionOption, TimeSpan transactionObjectLockTimeOut)
            : this(transactionOption)
        {
            if (transactionObjectLockTimeOut == TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("transactionObjectLockTimeOut");

            ObjectEnlistmentTimeOut = transactionObjectLockTimeOut;
        }


        ///<summary>
        ///Defines the transactional behavior of the scope
        ///</summary>
        /// <MetaDataID>{8EB60853-2E9C-4D2A-A090-C0D91E9C15AD}</MetaDataID>
        internal TransactionOption TransactionOption = TransactionOption.Required;


        /// <MetaDataID>{447EE597-AB2F-4B7F-ADA8-D8FFEB010E97}</MetaDataID>
        void IinitializeTransaction(TransactionOption transactionOption)
        {
            TransactionOption = transactionOption;

            LogicalThread.InitSystemStateTransition(transactionOption, this);
            if ((transactionOption == TransactionOption.Supported || transactionOption == TransactionOption.Suppress) && StateTransitionTransaction == null)
                return;
        }



        /// <MetaDataID>{08617f27-a885-4639-bf4b-22231cc76c1e}</MetaDataID>
        internal bool IsTransactionInitiator = false;
        /// <MetaDataID>{063F3C96-0D3C-4A98-8B42-1616278C3AD7}</MetaDataID>
        /// <summary>Define transaction which used for the state transition. </summary>

        public Transaction StateTransitionTransaction;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. Called when program go out of the using scoop define the end of object state transition. </summary>
        /// <MetaDataID>{2A4E625D-6F91-4936-AD84-15ADB229B55B}</MetaDataID>
        public void Dispose()
        {
            bool completed = false;
            try
            {
                LogicalThread.CheckStateTransitionCompleted(this);// ObjectStateTransitionTransaction,ObjectStateTransitionID);
                if (_Consistent)
                {
                    try
                    {
                        if (IsTransactionInitiator)
                        {
                            LogicalThread.StateTransitionCompleted(this);
                            completed = true;
                            (StateTransitionTransaction as TransactionRunTime).Commit();
                        }
                    }
                    catch (System.Exception Error)
                    {
                        throw;
                    }
                }
                else
                {
                    if (StateTransitionTransaction != null && StateTransitionTransaction.Status == TransactionStatus.Aborted)
                        return;
                    if (TransactionOption != TransactionOption.Supported && TransactionOption != TransactionOption.Suppress)
                        StateTransitionTransaction.Abort(null);
                    else if (StateTransitionTransaction != null)
                        StateTransitionTransaction.Abort(null);
                    else
                        return;


                    if (System.Diagnostics.Debugger.IsAttached)
                    {

                        System.Diagnostics.Debug.WriteLine("\n\rWarning!!!");
                        System.Diagnostics.Debug.WriteLine("Transaction aborted ");
#if DISTRIBUTED_TRANSACTIONS
                        LogicalThread.EntryCodeCookie entryCodeCookie = new OOAdvantech.Transactions.LogicalThread.EntryCodeCookie(1);
                        System.Diagnostics.Debug.WriteLine("Type: " + entryCodeCookie.CalledFunction.GetMethod().DeclaringType.FullName);
                        System.Diagnostics.Debug.WriteLine("Method: " + entryCodeCookie.CalledFunction.GetMethod());
                        System.Diagnostics.Debug.WriteLine("FileName:" + entryCodeCookie.CalledFunction.GetFileName());
                        System.Diagnostics.Debug.WriteLine("Line:" + entryCodeCookie.CalledFunction.GetFileLineNumber());
                        System.Diagnostics.Debug.WriteLine("Warning!!!\n\r");
#endif
                    }

                }


            }
            finally
            {
                if (!completed)
                    LogicalThread.StateTransitionCompleted(this);
            }


        }

    }
}
