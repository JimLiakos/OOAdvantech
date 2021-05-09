using OOAdvantech.Remoting;
using System;
using System.Reflection;
namespace OOAdvantech.Transactions
{

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
    using MarshalByRefObject = System.MarshalByRefObject;
#endif

    /// <MetaDataID>{ED70A12C-AE44-4150-AA2D-12EB530DE2C7}</MetaDataID>
    public class TransactionManager:MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
	{
        /// <MetaDataID>{de619b69-32ad-459d-9403-9a78cc074ce3}</MetaDataID>
        static TransactionManager()
        {
            #if !DeviceDotNet
            if(System.Transactions.TransactionManager.MaximumTimeout==System.TimeSpan.FromMinutes(10))
                TransactionmanagerHelper.OverrideMaximumTimeout(TimeSpan.FromMinutes(0));
            #endif
        }
        public static class TransactionmanagerHelper
        {
            public static void OverrideMaximumTimeout(TimeSpan timeout)
            {
#if !DeviceDotNet

                //TransactionScope inherits a *maximum* timeout from Machine.config.  There's no way to override it from
                //code unless you use reflection.  Hence this code!
                //TransactionManager._cachedMaxTimeout
                var type = typeof(System.Transactions.TransactionManager);
                var cachedMaxTimeout = type.GetField("_cachedMaxTimeout", BindingFlags.NonPublic | BindingFlags.Static);
                cachedMaxTimeout.SetValue(null, true);

                //TransactionManager._maximumTimeout
                var maximumTimeout = type.GetField("_maximumTimeout", BindingFlags.NonPublic | BindingFlags.Static);
                maximumTimeout.SetValue(null, timeout);
#endif
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C75E31C3-49E6-40E0-9DC6-44F92E123811}</MetaDataID>
        private static Collections.Generic.List<ITransactionContextProvider> _TransactionedContextProviders=new OOAdvantech.Collections.Generic.List<ITransactionContextProvider>();
        /// <MetaDataID>{8738304A-6CA2-4CDD-B3A6-EB68B5BD90A5}</MetaDataID>
        internal static Collections.Generic.List<ITransactionContextProvider> TransactionedContextProviders
        {
            get
            {
                return new OOAdvantech.Collections.Generic.List<ITransactionContextProvider>(_TransactionedContextProviders);
            }
        }
        /// <MetaDataID>{4480225a-05bf-431b-91ee-104653516535}</MetaDataID>
        public static System.TimeSpan ObjectEnlistmentTimeOut = System.TimeSpan.FromSeconds(30);

        /// <MetaDataID>{58a72ae1-6e32-42c4-a49e-6cfeb16d563e}</MetaDataID>
        public static string GetTransactionChannelUri(Transaction transaction)
        {
            if(string.IsNullOrEmpty(transaction.GlobalTransactionUri))
                return null;
            return TransactionCoordinator.GetChannelUri(transaction.GlobalTransactionUri);
        }

        /// <MetaDataID>{891B28AD-F9BC-4349-B7D4-4C0040F28C29}</MetaDataID>
        /// <summary>This method register to the transaction system a transaction context 
        /// provider for the transaction context extention. </summary>
        public static void RegisterTransactionContextProvider(ITransactionContextProvider transactionedContextProvider)
        {

             if(!_TransactionedContextProviders.Contains(transactionedContextProvider))
                _TransactionedContextProviders.Add(transactionedContextProvider);
        }

        ///// <MetaDataID>{A1D6A359-1DE9-4D4C-BE35-25897C8632F3}</MetaDataID>
        //public static System.Collections.ArrayList GetTransactionContexts(Transactions.Transaction transaction)
        //{
        //    System.Collections.ArrayList transactionContexts = new System.Collections.ArrayList();
        //    if (transaction.GlobalTransactionUri == null&&(transaction as TransactionRunTime).TransactionContext!=null)
        //    {
        //        transactionContexts.Add((transaction as TransactionRunTime).TransactionContext);
        //        return transactionContexts;
        //    }
        //    System.Collections.ArrayList objectsStateManagers = null;
        //    try
        //    {
        //        objectsStateManagers = TransactionRunTime.LocalTransactionCoordinator.GetEnlistedObjectsStateManagers(transaction.GlobalTransactionUri);
        //    }
        //    catch (System.Exception error)
        //    {
        //        throw;

        //    }

        //    foreach(ObjectsStateManager objectsStateManager in objectsStateManagers)
        //    {
        //        if(objectsStateManager is EnlistmentsController)
        //            GetTransactionContexts(objectsStateManager as EnlistmentsController, transactionContexts);
        //        if(objectsStateManager is ITransactionContext)
        //            transactionContexts.Add(objectsStateManager);
             
        //    }
        //    if((transaction as TransactionRunTime).TransactionContext!=null&&!transactionContexts.Contains((transaction as TransactionRunTime).TransactionContext))
        //        transactionContexts.Add((transaction as TransactionRunTime).TransactionContext);
        //    return transactionContexts;
        //}

        ///// <MetaDataID>{1E2FD511-6345-491D-A77C-57032FB6F957}</MetaDataID>
        //private static void GetTransactionContexts(EnlistmentsController enlistmentsController, System.Collections.ArrayList transactionContexts)
        //{
        //    foreach (ObjectsStateManager enlistedObjectsStateManager in enlistmentsController.GetEnlistedObjectsStateManagers())
        //    {
        //        if (enlistedObjectsStateManager is ITransactionContext)
        //            transactionContexts.Add(enlistedObjectsStateManager);
        //        else if (enlistedObjectsStateManager is EnlistmentsController)
        //            GetTransactionContexts(enlistedObjectsStateManager as EnlistmentsController, transactionContexts);
        //    }
        //}

        /// <MetaDataID>{9B2875C0-DC02-4560-AC3A-32D5ABA790E8}</MetaDataID>
        public static ITransactionContext GetTransactionContext(Transactions.Transaction transaction)
        {
            return (transaction as TransactionRunTime).TransactionContext;
        }

        /// <MetaDataID>{9A80D30F-0822-4E1D-88F6-C8AE21C8D582}</MetaDataID>
        public void EnlistObjectOnActiveTransaction(object transactionedObject, System.Reflection.MemberInfo fieldInfo)
        {
#if !DeviceDotNet
#region Preconditions Chechk
            if(Transaction.Current==null)
                throw new OOAdvantech.Transactions.TransactionException("There isn't trabsaction on call context.");

            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(transactionedObject as System.MarshalByRefObject))
                throw new System.ArgumentException("The EnlistObjectOnActiveTransaction can enlist, only in process transactionedObject","transactionedObject");
#endregion

            (Transaction.Current as TransactionRunTime).EnlistObject(transactionedObject,fieldInfo);
#endif
        }





#if !DeviceDotNet
        /// <MetaDataID>{e78a6b9c-2fa3-497d-99e1-eee618e40989}</MetaDataID>
        public static Transaction UnMarshal(string globalTransactionUri)
        {
            return TransactionRunTime.UnMarshal(globalTransactionUri);
            
        }
#endif

        /// <MetaDataID>{19bd620c-7ab1-4b65-bd6b-1e0f12abdbf7}</MetaDataID>
        public bool IsLocked(object transactionalObject, System.Reflection.MemberInfo memberInfo)
        {
            return LockedObjectEntry.IsLocked(transactionalObject, memberInfo);
        }

    }
}
