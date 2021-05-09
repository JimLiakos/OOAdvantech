using System.Reflection;
using System.Linq;
using System;
#if DeviceDotNet 
using System.PCL.Reflection;
#endif
namespace OOAdvantech.Transactions
{


    ///<summary>
    ///
    ///Enlist an object in transaction context and makes a code block transactional
    ///</summary>
    /// <MetaDataID>{57FE6E0A-1BDB-42FB-9A34-AB679208A73D}</MetaDataID>
    public sealed class ObjectStateTransition : System.IDisposable
    {

        #region IDisposable Members

        /// <MetaDataID>{abc8a0e5-7100-4bfa-adb2-d80f04cd1db6}</MetaDataID>
        public void Dispose()
        {
            SystemStateTransition.Dispose();
        }
        #endregion

        /// <MetaDataID>{3ee18bf4-bfe0-4881-84bf-467bfe892c49}</MetaDataID>
        public bool Consistent
        {
            set
            {
                SystemStateTransition.Consistent = value;
            }
            get
            {
                return SystemStateTransition.Consistent;
            }
        }


        /// <MetaDataID>{0AF376A9-647E-4467-9432-10BA6CED01BE}</MetaDataID>
        private object ObjectUnderStateTransition;
        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="transactionOption">This parameter defines the transactional behavior of the scope.</param>
        /// <MetaDataID>{4AA8A7AF-6EDD-41B6-A2DF-BD76746502E4}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, TransactionOption transactionOption)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition(transactionOption);
            try
            {
                EnlistObject(objectUnderStateTransition, default(System.Reflection.MemberInfo));
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }
        }
        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <MetaDataID>{3A1D9C5F-D7FC-48CB-9122-8A913DC34CA4}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if(objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");
 

            SystemStateTransition = new SystemStateTransition();

            try
            {
                EnlistObject(objectUnderStateTransition, default(System.Reflection.MemberInfo));
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }
        }


        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="transactionOption">This parameter defines the transactional behavior of the scope.</param>
        /// <param name="memberName">Defines the member in partial object enlistment</param>
        /// <MetaDataID>{4AA8A7AF-6EDD-41B6-A2DF-BD76746502E4}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, string memberName, TransactionOption transactionOption)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition(transactionOption);
            try
            {
                EnlistObject(objectUnderStateTransition, memberName, transactionOption);
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }

        }
        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="memberName">Defines the member in partial object enlistment</param>
        /// <MetaDataID>{3A1D9C5F-D7FC-48CB-9122-8A913DC34CA4}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, string memberName)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition();
            try
            {
                EnlistObject(objectUnderStateTransition, memberName, Transactions.TransactionOption.Required);
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }

        }


        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="transactionOption">This parameter defines the transactional behavior of the scope.</param>
        /// <param name="memberInfo">Defines the member in partial object enlistment</param>
        /// <MetaDataID>{4AA8A7AF-6EDD-41B6-A2DF-BD76746502E4}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, System.Reflection.MemberInfo memberInfo, TransactionOption transactionOption)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition(transactionOption);
            try
            {
                EnlistObject(objectUnderStateTransition, memberInfo);
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }


        }
        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="memberInfo">Defines the member in partial object enlistment</param>
        /// <MetaDataID>{3A1D9C5F-D7FC-48CB-9122-8A913DC34CA4}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, System.Reflection.MemberInfo memberInfo)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition();
            try
            {
                EnlistObject(objectUnderStateTransition, memberInfo);
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }

        }


        /// <MetaDataID>{6D3E1762-0F2C-4E3B-85FD-9A7D3DF66FBA}</MetaDataID>
        [System.Obsolete("System can't manipulate a transaction as transactional object", true)]
        public ObjectStateTransition(Transaction transaction)
        {

        }

        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="transactionObjectLockTimeOut">
        /// Defines the time we want system wait an other transaction to release the object in case of transaction object lock;
        /// </param>
        /// <MetaDataID>{72e1308f-a536-40f1-ab87-f2a12698d959}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, TimeSpan transactionObjectLockTimeOut)
        //: this(objectUnderStateTransition)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition(transactionObjectLockTimeOut);
            try
            {
                EnlistObject(objectUnderStateTransition, default(System.Reflection.MemberInfo));
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }


        }

        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="transactionOption">This parameter defines the transactional behavior of the scope.</param>
        /// <param name="transactionObjectLockTimeOut">
        /// Defines the time we want system wait an other transaction to release the object in case of transaction object lock;
        /// </param>
        /// <MetaDataID>{f739cc6f-e15f-4cb9-a19f-49444cdf4447}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, TransactionOption transactionOption, TimeSpan transactionObjectLockTimeOut)
        //: this(objectUnderStateTransition, transactionOption)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition(transactionOption, transactionObjectLockTimeOut);

            try
            {
                EnlistObject(objectUnderStateTransition, default(System.Reflection.MemberInfo));
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }

        }


        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="transactionOption">This parameter defines the transactional behavior of the scope.</param>
        /// <param name="memberName">Defines the member in partial object enlistment</param>
        /// <param name="transactionObjectLockTimeOut">
        /// Defines the time we want system wait an other transaction to release the object in case of transaction object lock;
        /// </param>
        /// <MetaDataID>{d7367ec5-0ae3-47e6-be87-6dfdfc03bd44}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, string memberName, TransactionOption transactionOption, TimeSpan transactionObjectLockTimeOut)
        //: this(objectUnderStateTransition, memberName, transactionOption)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition(transactionOption, transactionObjectLockTimeOut);

            try
            {
                EnlistObject(objectUnderStateTransition, memberName, transactionOption);
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }

        }

        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="memberName">Defines the member in partial object enlistment</param>
        /// <param name="transactionObjectLockTimeOut">
        /// Defines the time we want system wait an other transaction to release the object in case of transaction object lock;
        /// </param>
        /// <MetaDataID>{810a7894-7904-498a-af8d-7adab594cc16}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, string memberName, TimeSpan transactionObjectLockTimeOut)
        //: this(objectUnderStateTransition, memberName)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");
            SystemStateTransition = new SystemStateTransition(transactionObjectLockTimeOut);

            try
            {
                EnlistObject(objectUnderStateTransition, memberName, Transactions.TransactionOption.Required);
            }
            catch (System.Exception error)
            {
                SystemStateTransition.Consistent = true;
                SystemStateTransition.Dispose();
                throw;
            }


        }


        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="transactionOption">This parameter defines the transactional behavior of the scope.</param>
        /// <param name="memberInfo">Defines the member in partial object enlistment</param>
        /// <param name="transactionObjectLockTimeOut">
        /// Defines the time we want system wait an other transaction to release the object in case of transaction object lock;
        /// </param>
        /// <MetaDataID>{00014264-05ed-4212-9543-ec514ce77993}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, System.Reflection.MemberInfo memberInfo, TransactionOption transactionOption, TimeSpan transactionObjectLockTimeOut)
        //: this(objectUnderStateTransition, memberInfo, transactionOption)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");

            SystemStateTransition = new SystemStateTransition(transactionOption, transactionObjectLockTimeOut);

            EnlistObject(objectUnderStateTransition, memberInfo);
        }


        /// <summary>Initialize the object state transition for the object objectUnderStateTransition.</summary>
        /// <param name="objectUnderStateTransition">The object which will change its state.</param>
        /// <param name="memberInfo">Defines the member in partial object enlistment</param>
        /// <param name="transactionObjectLockTimeOut">
        /// Defines the time we want system wait an other transaction to release the object in case of transaction object lock;
        /// </param>
        /// <MetaDataID>{63a6d660-3e6f-4508-b122-bca1c3a7929f}</MetaDataID>
        public ObjectStateTransition(object objectUnderStateTransition, System.Reflection.MemberInfo memberInfo, TimeSpan transactionObjectLockTimeOut)
        //:this(objectUnderStateTransition, memberInfo)
        {
            if (objectUnderStateTransition == null)
                throw new OOAdvantech.Transactions.TransactionException("You can't make state transition to null object");
            Type objectType = objectUnderStateTransition.GetType();
            if (!IsTransactional(objectType))
                throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + objectUnderStateTransition.GetType().FullName + "'.");
            if (objectType.GetMetaData().IsValueType)
                throw new OOAdvantech.Transactions.TransactionException("The value type object '" + objectUnderStateTransition.GetType().FullName + "' can't participate in transaction .");
            SystemStateTransition = new SystemStateTransition(transactionObjectLockTimeOut);
            EnlistObject(objectUnderStateTransition, memberInfo);

        }

        /// <MetaDataID>{f6521192-007d-4755-abdc-b88474ade968}</MetaDataID>
        static System.Collections.Generic.Dictionary<Type, bool> TransactionalTypes = new System.Collections.Generic.Dictionary<Type, bool>();


        /// <MetaDataID>{e92be352-ab86-432f-9a37-cd73a6986ca3}</MetaDataID>
        static internal bool IsTransactional(System.Type type)
        {
            lock (TransactionalTypes)
            {
                if (type.IsValueType)
                    return false;
                bool isTransactional = false;
                if (!TransactionalTypes.TryGetValue(type, out isTransactional))
                {
                    if (type.GetMetaData().GetCustomAttributes(typeof(OOAdvantech.Transactions.TransactionalAttribute), true).Length > 0)
                        isTransactional = true;
                    else
                        isTransactional = false;
                    TransactionalTypes[type] = isTransactional;
                }
                return isTransactional;
            }

        }

        /// <MetaDataID>{b34d71d9-3983-4e27-9c38-f7d7d943ecac}</MetaDataID>
        void EnlistObject(object objectUnderStateTransition, string memberName, Transactions.TransactionOption transactionOption, params TimeSpan[] objectEnlistmentTimeOut)
        {
            ObjectUnderStateTransition = objectUnderStateTransition;
            if ((SystemStateTransition.TransactionOption == TransactionOption.Supported || SystemStateTransition.TransactionOption == TransactionOption.Suppress) && SystemStateTransition.StateTransitionTransaction == null)
                return;
            System.Reflection.MemberInfo memberInfo = null;
            if (!string.IsNullOrEmpty(memberName))
            {
                memberInfo = GetMemberInfo(objectUnderStateTransition.GetType(), memberName);
                if (memberInfo == null)
                    throw new OOAdvantech.Transactions.TransactionException("System can't fieds transactional member with name '" + memberName + "' on type '" + ObjectUnderStateTransition.GetType().FullName + "'");

            }
            EnlistObject(objectUnderStateTransition, memberInfo);


        }

        ///<summary>
        ///Check the object if it is locked for current transaction context and return true or false.
        ///
        ///</summary>
        /// <MetaDataID>{A5119080-CBAE-4533-91D3-E5E05D034712}</MetaDataID>
        public static bool IsLocked(object transactionalObject)
        {
            if (transactionalObject == null)
                return false;
            if (!IsTransactional(transactionalObject.GetType()))
                return false;
#if !DeviceDotNet 
            if (transactionalObject is System.MarshalByRefObject)
            {
                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(transactionalObject as System.MarshalByRefObject))
                {
                    string ChannelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(transactionalObject as System.MarshalByRefObject);
                    TransactionManager transactionManager = Remoting.RemotingServices.CreateRemoteInstance(ChannelUri, typeof(TransactionManager).ToString(), typeof(TransactionManager).FullName) as TransactionManager;
                    return transactionManager.IsLocked(transactionalObject, null);
                }
            }
#endif
            return LockedObjectEntry.IsLocked(transactionalObject, null);
        }


        /// <MetaDataID>{2248aabf-9858-4505-93e9-b79a92ca131b}</MetaDataID>
        public static bool IsLocked(object transactionalObject, string memberName)
        {
            if (transactionalObject == null)
                return false;
            if (!IsTransactional(transactionalObject.GetType()))
                return false;
#if !DeviceDotNet 
            if (transactionalObject is System.MarshalByRefObject)
            {
                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(transactionalObject as System.MarshalByRefObject))
                {
                    string ChannelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(transactionalObject as System.MarshalByRefObject);
                    TransactionManager transactionManager = Remoting.RemotingServices.CreateRemoteInstance(ChannelUri, typeof(TransactionManager).ToString(), typeof(TransactionManager).FullName) as TransactionManager;
                    return transactionManager.IsLocked(transactionalObject, GetMemberInfo(transactionalObject.GetType(), memberName));
                }
            }
#endif
            return LockedObjectEntry.IsLocked(transactionalObject, GetMemberInfo(transactionalObject.GetType(), memberName));

        }

        /// <MetaDataID>{a96c59e6-c46a-4884-a0c8-f355507ff2bb}</MetaDataID>
        SystemStateTransition SystemStateTransition;


        /// <summary>This constructor is a trick to avoid the code duplication.</summary>
        /// <MetaDataID>{4061C481-8A05-4B68-958C-175E855BD6AC}</MetaDataID>
        void EnlistObject(object objectUnderStateTransition, System.Reflection.MemberInfo memberInfo)
        {
            ObjectUnderStateTransition = objectUnderStateTransition;
            if ((SystemStateTransition.TransactionOption == TransactionOption.Supported || SystemStateTransition.TransactionOption == TransactionOption.Suppress) && SystemStateTransition.StateTransitionTransaction == null)
                return;

            try
            {
                if (memberInfo != null)
                {
                    object[] transactionalMembers = memberInfo.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).ToArray();
                    if (transactionalMembers.Length == 0)
                        throw new OOAdvantech.Transactions.TransactionException("the member '" + memberInfo.Name + "' isn't transactional member on type '" + ObjectUnderStateTransition.GetType().FullName + "'");

                    if (memberInfo is System.Reflection.PropertyInfo && string.IsNullOrEmpty((transactionalMembers[0] as TransactionalMemberAttribute).ImplentationField))
                        throw new OOAdvantech.Transactions.TransactionException("There isn't implementation field name for transactional property '" + memberInfo.Name + "'.\nDeclare the name of implementation field on TransactionalMember attribute.");
                    (SystemStateTransition.StateTransitionTransaction as TransactionRunTime).EnlistObject(ObjectUnderStateTransition, memberInfo);
                }
                else
                    (SystemStateTransition.StateTransitionTransaction as TransactionRunTime).EnlistObject(ObjectUnderStateTransition, memberInfo);



            }
            catch (System.Exception Error)
            {
                System.Diagnostics.Debug.WriteLine(Error.Message);
                throw;
            }

        }





        /// <MetaDataID>{117311ac-5589-409a-8c1e-0791a3360a99}</MetaDataID>
        private static System.Reflection.MemberInfo GetMemberInfo(System.Type type, string memberName)
        {
            //System.Reflection.MemberInfo memberInfo = type.GetMetaData().GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            //if (memberInfo.HasCustomAttribute(typeof(TransactionalMemberAttribute)))
            //    return memberInfo;
            foreach (System.Reflection.MemberInfo memberInfo in type.GetMetaData().GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (memberInfo.HasCustomAttribute(typeof(TransactionalMemberAttribute)))
                    return memberInfo;
            }
            if (type.GetMetaData().BaseType != typeof(object))
                return GetMemberInfo(type.GetMetaData().BaseType, memberName);
            return null;

        }


        /// <MetaDataID>{d3bb5e3d-f872-443d-a7d9-00a05d3d5f53}</MetaDataID>
        static public System.Collections.Generic.Dictionary<object, Transaction> GetLockTransactions(Transaction sessionTransaction, System.Collections.Generic.List<object> _objects)
        {
            throw new System.NotImplementedException();

        }

        /// <MetaDataID>{c8e96e10-16f3-4787-965b-0365897227ab}</MetaDataID>
        static internal System.Collections.Generic.List<System.Reflection.FieldInfo> GetPartialObjectLockFields(object _object)
        {
            
            Transaction scoopTransaction = Transaction.Current;
            if (scoopTransaction == null)
                return new System.Collections.Generic.List<System.Reflection.FieldInfo>(); 
            if (IsTransactional(_object.GetType()))
            {
                System.Collections.Generic.Dictionary<string, LockedObjectEntry> lockedObjectEntries = null;
                if (LockedObjectEntry.LockedObjects.TryGetValue(_object, out lockedObjectEntries))
                {
                    foreach (LockedObjectEntry lockedObjectEntry in lockedObjectEntries.Values)
                    {
                        if (lockedObjectEntry.TransactionContext.Transaction == scoopTransaction)
                            return lockedObjectEntry.PartialObjectLockFields;
                    }
                }
               
            }
            return new System.Collections.Generic.List<System.Reflection.FieldInfo>(); 
        }
        /// <MetaDataID>{f70eac74-9f7b-40d6-9529-8a3c58f4d42f}</MetaDataID>
        static public LockType GetTransactionLockType(object _object)
        {
            Transaction scoopTransaction=Transaction.Current;
            if(scoopTransaction==null)
                return LockType.Unlocked;
            if (IsTransactional(_object.GetType()))
            {
                System.Collections.Generic.Dictionary<string, LockedObjectEntry> lockedObjectEntries = null;
                if (LockedObjectEntry.LockedObjects.TryGetValue(_object, out lockedObjectEntries))
                {
                    foreach (LockedObjectEntry lockedObjectEntry in lockedObjectEntries.Values)
                    {
                        if (lockedObjectEntry.TransactionContext.Transaction == scoopTransaction)
                            return lockedObjectEntry.LockType;
                    }
                }
                return LockType.Unlocked;
            }
            return LockType.Unlocked;
        }
        /// <MetaDataID>{1EAB48CE-B8BC-4AAE-BA54-AF8636196A1A}</MetaDataID>
        static public System.Collections.Generic.List<Transaction> GetTransaction(object _object)
        {
            System.Collections.Generic.List<Transaction> transactions = new System.Collections.Generic.List<Transaction>();
            if (IsTransactional(_object.GetType()))
            {
              //  ExtensionProperties extensionProperties = ExtensionProperties.GetExtensionPropertiesFromObject(_object);
                System.Collections.Generic.Dictionary<string, LockedObjectEntry> lockedObjectEntries = null;
                if (LockedObjectEntry.LockedObjects.TryGetValue(_object, out lockedObjectEntries))
                {
                    foreach (LockedObjectEntry lockedObjectEntry in lockedObjectEntries.Values)
                    {
                        if (lockedObjectEntry.LockType != LockType.SharedFieldsParticipation)// !lockedObjectEntry.SharedFields)
                            transactions.Add(lockedObjectEntry.TransactionContext.Transaction);
                    }
                }
                return transactions;
            }
            else
                return transactions;

            //throw new OOAdvantech.Transactions.TransactionException("The Transaction attribute it isn't declared in type '" + _object.GetType().FullName + "'.");

        }






    }

}

