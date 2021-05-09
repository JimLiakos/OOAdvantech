using OOAdvantech.Remoting;
using System;
using System.Collections.Generic;
using System.Text;


#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif

namespace OOAdvantech.Transactions
{
    
    ///<summary>
    ///This class defines the server part of ObjectLockSession. 
    ///The work of session is to update session users 
    ///for the lock state of objects which assigned to the session. 
    ///</summary>
    /// <MetaDataID>{0bca7def-65d6-4c1a-bb42-92364620a14c}</MetaDataID>
    public class ServerLockControlSession : MarshalByRefObject, Remoting.IExtMarshalByRefObject
    {
        public delegate void ObjectLockEventHandler(object _object, string[] membersNames, Transaction transaction);
        /// <MetaDataID>{591106c9-2eda-4705-b4e0-d684aed97747}</MetaDataID>
        System.Guid SessionIdentity;
        /// <MetaDataID>{00b8d60f-4484-4aac-bb0c-db7913bde874}</MetaDataID>
        Transaction SessionTransaction;
        /// <MetaDataID>{5df4039a-e2f3-46b1-a4f8-b82d8547d423}</MetaDataID>
        WeakReferenceObjectLockedConsumer ObjectLockedConsumer;
        /// <MetaDataID>{ea80ed2b-cb58-422f-8dff-aad739c79d26}</MetaDataID>
        public ServerLockControlSession()
        {
            SessionTransaction = Transaction.Current;
            ObjectLockedConsumer = new WeakReferenceObjectLockedConsumer(this);
        }

        /// <MetaDataID>{eaa0bfe0-9706-4a75-bcf0-8938b3b00727}</MetaDataID>
        ~ServerLockControlSession()
        {
            ObjectLockedConsumer.Dispose();
        }
        //TODO μήπως θα ήταν καλυτερο να χρησιμοποιήσουμε μια ποιό γρήγορη collection στο search
        /// <MetaDataID>{3cfed7ea-4c9b-4bd3-92ef-dfb7c5be0afe}</MetaDataID>
        OOAdvantech.Collections.Generic.List<object> LockRequestObjects = new OOAdvantech.Collections.Generic.List<object>();

        /// <summary>
        /// Assign the objects who client wants to know the lock state. 
        /// </summary>
        /// <param name="lockRequestObjects">
        /// This parameter defines a collection with the objects where client wants to know lock state 
        /// </param>
        /// <param name="remoteCall">
        /// This parameter defines the position of caller. 
        /// If the caller is out of process then the value of parameter is true
        /// </param>
        /// <returns>
        /// Every entry has the object as key and the lock Transaction.
        /// This objects collection is subset of the lockRequestObjects collection.  
        /// </returns>
        /// <MetaDataID>{4cbbe878-26e7-4b94-831e-4ef511199790}</MetaDataID>
        public OOAdvantech.Collections.Generic.Dictionary<object, object> Assign(OOAdvantech.Collections.Generic.List<object> lockRequestObjects, bool remoteCall)
        {
            OOAdvantech.Collections.Generic.Dictionary<object, object> lockedObjects = new OOAdvantech.Collections.Generic.Dictionary<object, object>();
            foreach (object _object in lockRequestObjects)
            {
                List<Transaction> transactions = ObjectStateTransition.GetTransaction(_object);
                if (transactions.Count > 0 && transactions[0] != SessionTransaction && SessionTransaction != null)
                {
                    Transaction lockTransaction = transactions[0];

                    //while (lockTransaction != null && lockTransaction != SessionTransaction)
                    //    lockTransaction = lockTransaction.OriginTransaction;

                    if (lockTransaction != SessionTransaction)
                    {
                        bool stop = false;
                        if (transactions[0].Status != TransactionStatus.Continue)
                            stop = true;
                        if (remoteCall)
                        {
                            string transactionStream = null;
                            try
                            {
                                //υπάρχει περίπτωση λόγω multithread να έχει γ;iνει commit/abord
                                transactionStream = (transactions[0] as TransactionRunTime).Marshal();
                                lockedObjects.Add(_object, transactionStream);
                            }
                            catch
                            {

                            }
                        }
                        else
                            lockedObjects.Add(_object, transactions[0]);
                    }
                }
                LockRequestObjects.Add(_object);
            }
            return lockedObjects;
        }
        public event ObjectLockEventHandler ObjectLocked;
        /// <MetaDataID>{0a642e14-8a9f-4704-aed6-87c699a54bc9}</MetaDataID>
        void OnObjectLocked(object _object, System.Reflection.MemberInfo[] memberInfos, Transaction transaction)
        {
            try
            {
                if (LockRequestObjects.Contains(_object) && ObjectLocked != null && transaction != SessionTransaction)
                {
                    Transaction lockTransaction = transaction;
                    //while (lockTransaction != null && lockTransaction != SessionTransaction)
                    //    lockTransaction = lockTransaction.OriginTransaction;
                    if (lockTransaction != SessionTransaction)
                    {
                        if (memberInfos != null)
                        {
                            string[] membersNames = new string[memberInfos.Length];
                            int i = 0;
                            foreach (System.Reflection.MemberInfo memberInfo in memberInfos)
                                membersNames[i++] = memberInfo.Name;

                            OOAdvantech.EventUnderProtection.Invoke<ObjectLockEventHandler>(ref ObjectLocked, EventUnderProtection.ExceptionHandling.RemoveDisconnectedEventHandlers | EventUnderProtection.ExceptionHandling.IgnoreExceptions, _object, membersNames, transaction);
                        }
                        else
                            OOAdvantech.EventUnderProtection.Invoke<ObjectLockEventHandler>(ref ObjectLocked, EventUnderProtection.ExceptionHandling.RemoveDisconnectedEventHandlers | EventUnderProtection.ExceptionHandling.IgnoreExceptions, _object, null, transaction);
                    }


                }
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <summary>
        /// This class used from ServerObjectLockSession object for weak reference event subscription to TransactionContext static ObjectLocked event. 
        /// The weak reference consumer subscribes a method to event publisher at the construction 
        /// and propagates the event consuming to the ServerObjectLockSession object through a weak reference. 
        /// When the ServerObjectLockSession object collect from GC the weak reference consumer unsubscribed.         
        /// </summary>
        class WeakReferenceObjectLockedConsumer : IDisposable
        {
            System.WeakReference WeakRefernceEventHandler;
            /// <summary>
            /// Constructor method takes as parameter the real object lock consumer
            /// </summary>
            /// <param name="serverLockControlSession">
            /// This parameter defines the real ObjectLocked consumer
            /// </param>
            public WeakReferenceObjectLockedConsumer(ServerLockControlSession serverLockControlSession)
            {
                WeakRefernceEventHandler = new WeakReference(serverLockControlSession);
                TransactionContext.ObjectLocked += new Transactions.ObjectLockEventHandler(ObjectLocked);

            }
            public void Dispose()
            {
                TransactionContext.ObjectLocked -= new Transactions.ObjectLockEventHandler(ObjectLocked);
            }
            /// <summary>
            /// ObjectLocked event consumer 
            /// </summary>
            /// <param name="_object"></param>
            /// <param name="memberInfo"></param>
            /// <param name="transaction"></param>
            void ObjectLocked(object _object, System.Reflection.MemberInfo[] memberInfo, Transaction transaction)
            {

                ServerLockControlSession serverLockControlSession = WeakRefernceEventHandler.Target as ServerLockControlSession;
                if (serverLockControlSession != null)
                    serverLockControlSession.OnObjectLocked(_object, memberInfo, transaction);
                else
                    TransactionContext.ObjectLocked -= new Transactions.ObjectLockEventHandler(ObjectLocked);
            }

        }

    }


}
