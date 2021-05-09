using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Transactions.Bridgea
{
    public enum NativeTransactionState
    {
        Commit,
        Abort,
        Indoubt,
        Active,
        HeuristicDecisionCommit,
        HeuristicDecisionAbort,
        HeuristicDecisionDamage,
        HeuristicDecisionDanger
    }

    public delegate void NativeTransactionEventRaise(object sender, NativeTransactionState newState);

    public class NativeTPMonitor : System.MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        public static void CommitTransaction(object nativeTransaction)
        {
            (nativeTransaction as System.Transactions.CommittableTransaction).Commit();
        }
        static System.Collections.Hashtable NativeTransactions = new System.Collections.Hashtable();

        public static void TransactionCompleted(object sender, NativeTransactionState newState)
        {
            if (newState != NativeTransactionState.Active)
            {
                if (NativeTransactions.Contains((sender as NativeTransactionOutcomeEvents).TransactionUri.ToLower()))
                    NativeTransactions.Remove((sender as NativeTransactionOutcomeEvents).TransactionUri.ToLower());
            }

        }

        public static object CreateTransaction(int Timeout, string Description, string transactionUri)
        {
            System.Transactions.TransactionOptions transactionOptions = new System.Transactions.TransactionOptions();
            transactionOptions.Timeout = new TimeSpan(0, 0, 0, 0, Timeout);
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead;

            System.Transactions.CommittableTransaction nativeTransaction = new System.Transactions.CommittableTransaction(transactionOptions);

            NativeTransactionOutcomeEvents nativeTransactionOutcomeEvents = new NativeTransactionOutcomeEvents(nativeTransaction, transactionUri);
            nativeTransactionOutcomeEvents.NativeTransactionEvent += new NativeTransactionEventRaise(TransactionCompleted);

            NativeTransactions[transactionUri.ToLower()] = nativeTransactionOutcomeEvents;


            return nativeTransaction;

        }
        public static void AbortTransaction(object nativeTransaction)
        {
            (nativeTransaction as System.Transactions.CommittableTransaction).Rollback();
        }
        public static System.Exception RunUnderTransaction(object target, string methodName, string[] argNames, object[] argValues, ref object result, string transactionURI /*,object nativeTransaction*/)
        {
            System.Transactions.Transaction currentTransaction = System.Transactions.Transaction.Current;
            try
            {
                System.Transactions.Transaction.Current = GetTransaction(transactionURI) as System.Transactions.Transaction;
                //string poo=System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                result = target.GetType().InvokeMember(methodName, System.Reflection.BindingFlags.InvokeMethod, null, target, argValues, null, null, argNames);
            }
            catch (System.Exception Error)
            {
                return Error;
            }
            finally
            {
                System.Transactions.Transaction.Current = currentTransaction;

            }
            return null;


        }
        public void ImportTransaction(string transactionUri, object nativeTransaction)
        {
            if (!NativeTransactions.Contains(transactionUri.ToLower()))
            {
                NativeTransactionOutcomeEvents nativeTransactionOutcomeEvents = new NativeTransactionOutcomeEvents(nativeTransaction, transactionUri);
                nativeTransactionOutcomeEvents.NativeTransactionEvent += new NativeTransactionEventRaise(TransactionCompleted);
                NativeTransactions[transactionUri.ToLower()] = nativeTransactionOutcomeEvents;
            }


        }
        public static void PropagateTransaction(object nativeTransaction, string transactionUri, System.MarshalByRefObject resourceManager)
        {

            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(resourceManager as System.MarshalByRefObject))
            {
                string channelUri = null;
                try
                {

                    channelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(resourceManager as System.MarshalByRefObject);
                    NativeTPMonitor nativeTPMonitor = Remoting.RemotingServices.CreateInstance(channelUri, typeof(NativeTPMonitor).FullName) as NativeTPMonitor;
                    nativeTPMonitor.ImportTransaction(transactionUri, nativeTransaction);
                }
                catch (System.Exception Error)
                {
                    //TODO Error prone   message  log file   exception
                    if (!System.Diagnostics.EventLog.SourceExists("TransactionSystem"))
                        System.Diagnostics.EventLog.CreateEventSource("TransactionSystem", "OOTransactionSystem", ".");
                    System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                    myLog.Source = "TransactionSystem";
                    System.Diagnostics.Debug.WriteLine("Can't export transaction to " + channelUri + ".");
                    //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                    myLog.WriteEntry("Can't export transaction to " + channelUri + ".", System.Diagnostics.EventLogEntryType.Error);
                    throw new System.Exception("Can't export transaction to " + channelUri + ".");
                }
            }


        }
        public static void EventSubscriptionOnNativeTransaction(object nativeTransaction, NativeTransactionEventRaise nativeTransactionEventRaise, string transactionUri)
        {
            (NativeTransactions[transactionUri.ToLower()] as NativeTransactionOutcomeEvents).NativeTransactionEvent += nativeTransactionEventRaise;
        }
        public static object GetTransaction(string transactionUri)
        {
            return (NativeTransactions[transactionUri.ToLower()] as NativeTransactionOutcomeEvents).NativeTransaction;
        }

        static System.ServiceProcess.ServiceController DTCServices;
        public static void Start()
        {
            if (DTCServices == null)
                DTCServices = new System.ServiceProcess.ServiceController("Distributed Transaction Coordinator");
            try
            {
                DTCServices.Refresh();
                if (DTCServices.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                    return;
            }
            catch (System.Exception Error)
            {

            }
            try
            {

                DTCServices.Start();
                int Count = 0;
                while (Count < 20)
                {

                    Count++;
                    DTCServices.Refresh();
                    if (DTCServices.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        break;
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (System.Exception Error)
            {

            }
        }
    }
}
