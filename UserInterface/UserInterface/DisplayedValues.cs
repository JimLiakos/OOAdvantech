using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Transactions;
using System.Collections.Specialized;
using System.Globalization;

namespace OOAdvantech.UserInterface.Runtime
{

    /// <MetaDataID>{4FD19975-23C7-4566-AE6A-330E87BB73B8}</MetaDataID>
    ///<summary>
    /// Objects of this class cache data in user interface session. 
    /// This session usually corresponds to a transaction.
    /// There is one displayed object per object in user transaction.
    /// The displayed value has members where caching the values of object member.
    /// User interface subsystem to reduce the roundtrips in distributed, 
    /// when user change a value in user interface system update only the displayed values 
    /// and at the end (Ok button) send new values in business object with one shot.    
    /// 
    ///</summary>
    public class DisplayedValue : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, System.ComponentModel.INotifyPropertyChanged
    {
        /// <MetaDataID>{dba7114c-a060-495f-ad13-5594ca58bc4d}</MetaDataID>
        internal bool AssignedToLockControlSession = false;
        [MetaDataRepository.Association("ObjectMembers", typeof(Member), MetaDataRepository.Roles.RoleA, "79eeb7a7-8292-42f1-873e-6335b481e182")]
        [MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        [MetaDataRepository.RoleAMultiplicityRange(0)]
        [MetaDataRepository.IgnoreErrorCheck]
        internal readonly Dictionary<string, Member> Members = new Dictionary<string, Member>();


        /// <MetaDataID>{893bfa8f-8128-49e7-a12b-107dfa8e38ed}</MetaDataID>
        internal static int ObjsCount = 0;






        class ObjectChangeStateEvent
        {
            public readonly string Member;
            public CultureInfo Culture { get; }

            public bool UseDefaultCultureValue { get; }

            public readonly object Object;
            public readonly Transaction Transaction;



            public ObjectChangeStateEvent(object obj, string member)
            {
                this.Culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                this.UseDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                Object = obj;
                Member = member;
                Transaction = Transaction.Current;
            }
        }

        /// <MetaDataID>{1bb24b4d-9e08-4db0-a9de-05b25bb1e205}</MetaDataID>
        OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.Transactions.Transaction, OOAdvantech.Collections.Generic.List<ObjectChangeStateEvent>> TransctionedObjectChangeStateEvents;

        Queue<ObjectChangeStateEvent> AsynchObjectStateChanges = new Queue<ObjectChangeStateEvent>();

        /// <MetaDataID>{a43fbaa4-320a-4d6a-a345-2a7f34dae3c8}</MetaDataID>
        internal void RemoveEventHandler()
        {
            if (UserInterfaceSession.StartingUserInterfaceObjectConnection == null)
                return;

            if (_Value != null)
            {
                foreach (System.Reflection.EventInfo eventInfo in GetEventsFromHierarchy(_Value.GetType()))
                {
                    try
                    {
                        if (eventInfo.EventHandlerType == typeof(OOAdvantech.ObjectChangeStateHandle))
                        {
                            UserInterfaceSession.UnsubscribeFromEvent(eventInfo, _Value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));
                            //eventInfo.RemoveEventHandler(_Value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));
                        }
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }
                }
                if (_Value is System.ComponentModel.INotifyPropertyChanged)
                    (_Value as System.ComponentModel.INotifyPropertyChanged).PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnPropertyChanged);

                UserInterfaceSession.StartingUserInterfaceObjectConnection.RefreshPathDataDisplayers -= RefreshUserInterface;
            }
        }

        /// <MetaDataID>{f118b08a-7c68-4316-93e2-9be86d74b1f5}</MetaDataID>
        bool CopyNeeded = true;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{F134F99D-2390-45B6-868E-C477CACC322B}</MetaDataID>
        private object _Value;
        /// <MetaDataID>{565C268F-1040-43A0-AFA7-B2EC9B10E12F}</MetaDataID>
        public object Value
        {
            get
            {
                if (CopyNeeded)
                {
                    if (_Value != null)
                    {
                        Type type = _Value.GetType();
                        if (type.IsValueType && !type.IsPrimitive)
                            return UISession.CopyValue(_Value);
                        else
                        {
                            CopyNeeded = false;
                            return _Value;
                        }
                    }
                    else if (_Value != null)
                    {
                        CopyNeeded = false;
                        return _Value;
                    }

                }
                return _Value;
            }
            set
            {
                if (UserInterfaceSession.StartingUserInterfaceObjectConnection != null && _Value != null && _Value != value)
                {
                    if (_Value != null)
                    {
                        foreach (System.Reflection.EventInfo eventInfo in GetEventsFromHierarchy(_Value.GetType()))
                        {
                            if (eventInfo.EventHandlerType == typeof(OOAdvantech.ObjectChangeStateHandle))
                            {

                                UserInterfaceSession.UnsubscribeFromEvent(eventInfo, _Value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));
                                //eventInfo.RemoveEventHandler(_Value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));

                            }
                        }
                        if (value is System.ComponentModel.INotifyPropertyChanged)
                            (value as System.ComponentModel.INotifyPropertyChanged).PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(OnPropertyChanged);

                    }
                }
                if (UserInterfaceSession.StartingUserInterfaceObjectConnection != null && _Value != value)
                {
                    if (value != null)
                    {
                        foreach (System.Reflection.EventInfo eventInfo in GetEventsFromHierarchy(value.GetType()))
                        {
                            if (eventInfo.EventHandlerType == typeof(OOAdvantech.ObjectChangeStateHandle))
                            {
                                UserInterfaceSession.SubscribeOnEvent(eventInfo, value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));
                                //eventInfo.AddEventHandler(value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));

                            }
                        }
                        if (value is System.ComponentModel.INotifyPropertyChanged)
                            (value as System.ComponentModel.INotifyPropertyChanged).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnPropertyChanged);
                    }
                }

                if (_Value != value)
                {
                    if (IsEnumerable)
                    {
                        //TODO δεν ξέρω αν θα έρθει από εδώ ο κωδικας ποτε
                        Members["Items"].ReleasePathDataDisplayer();
                        System.Type memberType = Members["Items"].MemberType;
                        System.Type ownerType = Members["Items"].OwnerType;
                        Members.Remove("Items");
                        Members.Add("Items", new Member("Items", this, ownerType, memberType));
                        int loadedCount = Members["Items"].ValuesCollection.Count;
                    }
                }
                _Value = value;
            }

        }



        /// <exclude>Excluded</exclude>
        UserInterfaceObjectConnection _ViewControlObject;
        /// <MetaDataID>{2a4aea8f-70bf-4f01-a3e4-9ffe1ce4afb5}</MetaDataID>
        //internal UserInterfaceObjectConnection ViewControlObject
        //{
        //    get
        //    {
        //        return _ViewControlObject;

        //    }
        //    set
        //    {

        //        _ViewControlObject = value;


        //    }
        //}

        internal UIProxy CrossSessionValue;

        /// <MetaDataID>{7b9bb6bb-5836-439e-a2dc-e68e302b0d3b}</MetaDataID>
        public readonly bool ChangeStateExist = true;
        /// <MetaDataID>{83aabd93-feca-4f2c-ae49-36b0cc5c2dda}</MetaDataID>
        internal UISession UserInterfaceSession;
        /// <MetaDataID>{06A8C29D-1978-4B3C-8695-A54BB6DCAB9C}</MetaDataID>
        internal DisplayedValue(object value, UISession userInterfaceSession)
        {

            try
            {
                if (value is System.DBNull)
                {
                    int tt = 0;
                }

                UIProxy UIProxy = UIProxy.GetUIProxy(value);
                if (UIProxy != null)
                {
                    if (UIProxy.CrossSessionValue)
                        CrossSessionValue = UIProxy;

                    value = UIProxy._RealTransparentProxy;
                }
                UserInterfaceSession = userInterfaceSession;
                _Value = value;
                if (UserInterfaceSession != null && _Value != null && !_Value.GetType().IsValueType)
                    UserInterfaceSession.Add(this);
                ObjsCount++;
                // ViewControlObject = viewControlObject;


                _Value = value;


                if (_Value != null)
                {
                    Type valueType = _Value.GetType();

                    //System.Reflection.MethodInfo methodInfo = valueType.GetType().GetMethod("GetEnumerator", new System.Type[0]);
                    Type elementType = FindIEnumerable(valueType);
                    if (elementType != null)
                    {
                        Members.Add("Items", new Member("Items", this, valueType, GetElementType(valueType)));
                        int loadedCount = Members["Items"].ValuesCollection.Count;
                        IsEnumerable = true;
                    }
                    else
                        IsEnumerable = false;

                    foreach (System.Reflection.EventInfo eventInfo in GetEventsFromHierarchy(_Value.GetType()))
                    {
                        if (eventInfo.EventHandlerType == typeof(OOAdvantech.ObjectChangeStateHandle))
                        {
                            ChangeStateExist = true;

                            //eventInfo.AddEventHandler(_Value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));
                            UserInterfaceSession.SubscribeOnEvent(eventInfo, _Value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));

                        }
                    }
                    if (_Value is System.ComponentModel.INotifyPropertyChanged)
                        (_Value as System.ComponentModel.INotifyPropertyChanged).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnPropertyChanged);
                    if (UserInterfaceSession != null && UserInterfaceSession.StartingUserInterfaceObjectConnection != null)
                        UserInterfaceSession.StartingUserInterfaceObjectConnection.RefreshPathDataDisplayers += RefreshUserInterface;
                }

            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        private void RefreshUserInterface(object sender, EventArgs e)
        {
            while (AsynchObjectStateChanges.Count > 0)
            {
                var ObjectChangeStateEvent = AsynchObjectStateChanges.Dequeue();

                using (CultureContext cultureContext = new CultureContext(ObjectChangeStateEvent.Culture, ObjectChangeStateEvent.UseDefaultCultureValue))
                {

                    if (ObjectChangeStateEvent.Transaction != null && ObjectChangeStateEvent.Transaction.Status == TransactionStatus.Continue)
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(ObjectChangeStateEvent.Transaction))
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(ObjectChangeStateEvent.Member))
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, Member> entry in Members)
                                    {
                                        if (UserInterfaceSession.CanAccessValue(_Value, _Value.GetType(), entry.Key, null))
                                            UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), entry.Key), _Value.GetType(), entry.Key, entry.Key);
                                        else
                                        {

                                        }
                                    }
                                }
                                else
                                    UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), ObjectChangeStateEvent.Member), _Value.GetType(), ObjectChangeStateEvent.Member, ObjectChangeStateEvent.Member);
                            }
                            catch (System.Exception error)
                            {
                                throw;
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ObjectChangeStateEvent.Member))
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, Member> entry in Members)
                                UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), entry.Key), _Value.GetType(), entry.Key, entry.Key);

                        }
                        else
                            UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), ObjectChangeStateEvent.Member), _Value.GetType(), ObjectChangeStateEvent.Member, ObjectChangeStateEvent.Member);
                    }
                }
            }
        }



        /// <MetaDataID>{f1fc7682-b45a-4d5e-a8be-98b6d799d926}</MetaDataID>
        internal static System.Type GetElementType(System.Type seqType)
        {
            System.Type type = FindIEnumerable(seqType);
            if (type == null)
            {
                return seqType;
            }
            return type.GetGenericArguments()[0];
        }

        /// <MetaDataID>{47d2ecba-28f1-4892-8c53-42586130c343}</MetaDataID>
        internal static System.Type FindIEnumerable(System.Type seqType)
        {
            if ((seqType != null) && (seqType != typeof(string)))
            {
                if (seqType.IsArray)
                {
                    return typeof(IEnumerable<>).MakeGenericType(new[] { seqType.GetElementType() });
                }
                if (seqType.IsGenericType)
                {
                    foreach (System.Type type in seqType.GetGenericArguments())
                    {
                        System.Type type2 = typeof(IEnumerable<>).MakeGenericType(new[] { type });
                        if (type2.IsAssignableFrom(seqType))
                        {
                            return type2;
                        }
                    }
                }
                System.Type[] interfaces = seqType.GetInterfaces();
                if ((interfaces != null) && (interfaces.Length > 0))
                {
                    foreach (System.Type type3 in interfaces)
                    {
                        System.Type type4 = FindIEnumerable(type3);
                        if (type4 != null)
                        {
                            return type4;
                        }
                    }
                }
                if ((seqType.BaseType != null) && (seqType.BaseType != typeof(object)))
                {
                    return FindIEnumerable(seqType.BaseType);
                }
            }
            return null;
        }

        /// <MetaDataID>{38c0e421-8c91-4830-a160-d9497965a713}</MetaDataID>
        public readonly bool IsEnumerable;

        /// <MetaDataID>{bf7a3cae-009b-40e8-87b8-de257c3beb33}</MetaDataID>
        internal DisplayedValue(object value, Type valueType, UISession userInterfaceSession)
            : this(value, userInterfaceSession)
        {
            if (value is System.DBNull)
            {
                int tt = 0;
            }

            if (FindIEnumerable(valueType) != null && !Members.ContainsKey("Items"))
            {
                Members.Add("Items", new Member("Items", this, valueType, FindIEnumerable(valueType)));
                int loadedCount = Members["Items"].ValuesCollection.Count;
                IsEnumerable = true;
                // #if WPFExtension
                if (value is INotifyCollectionChanged)
                    (value as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(DisplayedValue_CollectionChanged);
                // #endif

            }
            else
                IsEnumerable = FindIEnumerable(valueType) != null;
        }


        /// <MetaDataID>{47d01260-8c5c-413a-9863-fb13fa89f6cb}</MetaDataID>
        void DisplayedValue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    DisplayedValue itemDisplayedValue = null;
                    if (!UserInterfaceSession.TryGetDisplayedValue(item, out itemDisplayedValue))
                        itemDisplayedValue = new DisplayedValue(item, UserInterfaceSession);
                    Members["Items"].Add(itemDisplayedValue, e.NewStartingIndex);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {

                foreach (var item in e.OldItems)
                {
                    DisplayedValue itemDisplayedValue = null;
                    if (UserInterfaceSession.TryGetDisplayedValue(item, out itemDisplayedValue))
                    {
                        Members["Items"].Remove(itemDisplayedValue);
                    }
                }


            }
            //Members[Items
        }



        /// <MetaDataID>{ab618da9-ad27-4410-bcc9-69e493eeefc9}</MetaDataID>
        System.Collections.Generic.List<System.Reflection.EventInfo> GetEventsFromHierarchy(System.Type type)
        {


            System.Collections.Generic.List<System.Reflection.EventInfo> events = new List<System.Reflection.EventInfo>();
            while (type != typeof(object) && type != null)
            {
                foreach (System.Reflection.EventInfo eventInfo in type.GetEvents())
                {
                    if (type == eventInfo.DeclaringType)
                        events.Add(eventInfo);
                }

                type = type.BaseType;
            }
            return events;

        }


        /// <MetaDataID>{2c936b0c-128d-402d-811f-5f0ca34ad9a9}</MetaDataID>
        OOAdvantech.UserInterface.PathNode _RootObjectNode;
        /// <MetaDataID>{f713cee2-ef4a-45f2-9b3d-66d449ebce4b}</MetaDataID>
        internal OOAdvantech.UserInterface.PathNode RootObjectNode
        {
            get
            {
                try
                {

                    if (_RootObjectNode == null)
                        _RootObjectNode = new OOAdvantech.UserInterface.PathNode("Root", null);
                    else
                    {

                    }

                    if (Types != null)
                    {
                        foreach (System.Type type in Types)
                        {
                            foreach (string path in UserInterfaceSession.TypesPaths[type])
                            {
                                if (path.IndexOf("RealObject.") == 0)
                                    _RootObjectNode.AddPath(path.Substring("RealObject.".Length));
                                else
                                    _RootObjectNode.AddPath(path);
                            }
                        }
                    }
                }
                catch (System.Exception error)
                {

                }
                return _RootObjectNode;

            }
            set
            {
            }

        }

        /// <MetaDataID>{84ecb82b-9208-4d15-82db-ff0b1d1d3ac1}</MetaDataID>
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnObjectChangeState(sender, e.PropertyName);
        }

        /// <MetaDataID>{f8a426a8-7d42-497a-8722-63a3c2a6e40e}</MetaDataID>
        [MetaDataRepository.AllowEventCallAsynchronous]
        internal void OnObjectChangeState(object _object, string member)
        {
            if (UserInterfaceSession.State == UISession.SessionState.Terminated)
                return;
            if (OOAdvantech.Transactions.Transaction.Current != null && OOAdvantech.Transactions.Transaction.Current.Status != TransactionStatus.Continue)
                return;



            //System.Diagnostics.Debug.WriteLine("OnObjectChangeState(object _object, string member)");
            if (string.IsNullOrEmpty(member) || Members.ContainsKey(member))
            {
                try
                {
                    UserInterfaceSession.StartControlValuesUpdate();

                    OOAdvantech.Transactions.Transaction displayedValueMasterTransaction = UserInterfaceSession.Transaction;
                    if (displayedValueMasterTransaction != null)
                    {
                        while (displayedValueMasterTransaction.OriginTransaction != null)
                            displayedValueMasterTransaction = displayedValueMasterTransaction.OriginTransaction;
                    }


                    OOAdvantech.Transactions.Transaction changeStateTransaction = OOAdvantech.Transactions.Transaction.Current;

                    if (changeStateTransaction != null)
                    {
                        while (changeStateTransaction.OriginTransaction != null)
                            changeStateTransaction = changeStateTransaction.OriginTransaction;
                    }

                    if (changeStateTransaction != null && changeStateTransaction != displayedValueMasterTransaction)
                    {
                        #region create object change state event. this event will take action when transaction comleted
                        if (TransctionedObjectChangeStateEvents == null)
                            TransctionedObjectChangeStateEvents = new OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.Transactions.Transaction, OOAdvantech.Collections.Generic.List<ObjectChangeStateEvent>>();

                        if (!TransctionedObjectChangeStateEvents.ContainsKey(changeStateTransaction))
                        {
                            changeStateTransaction.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(ChangeStateTransactionTransactionCompletted);
                            TransctionedObjectChangeStateEvents[changeStateTransaction] = new OOAdvantech.Collections.Generic.List<ObjectChangeStateEvent>();

                        }
                        TransctionedObjectChangeStateEvents[changeStateTransaction].Add(new ObjectChangeStateEvent(_object, member));
                        #endregion
                    }
                    else
                    {
                        #region Update user interface now
                        //The object state event raised in the same transaction with user interface session transaction
                        if (_Value != null)
                        {
                            if (changeStateTransaction == null && changeStateTransaction != displayedValueMasterTransaction &&
                                displayedValueMasterTransaction != null && displayedValueMasterTransaction.Status == TransactionStatus.Continue)
                            {
                                using (SystemStateTransition stateTransition = new SystemStateTransition(displayedValueMasterTransaction))
                                {
                                    if (UserInterfaceSession.MainThreadID != System.Threading.Thread.CurrentThread.ManagedThreadId)
                                    {
                                        AsynchObjectStateChanges.Enqueue(new ObjectChangeStateEvent(_object, member));
                                    }
                                    else
                                    {

                                        if (string.IsNullOrEmpty(member))
                                        {
                                            #region Update all members
                                            bool formUpdated = false;
                                            try
                                            {

                                                if (Value is MarshalByRefObject)
                                                {
                                                    OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(Value as MarshalByRefObject, RootObjectNode.Paths);

                                                    Member _member = null;
                                                    if (Value != null)
                                                        _member = new OOAdvantech.UserInterface.Runtime.Member(RootObjectNode.Name, null, null, Value.GetType());

                                                    UserInterfaceSession.LoadDisplayedValues(RootObjectNode, structureSet, _member, -1);
                                                    formUpdated = true;

                                                }

                                            }
                                            catch (System.Exception error)
                                            {
                                                throw;
                                            }

                                            if (!formUpdated)
                                            {
                                                foreach (string memberName in new List<string>(Members.Keys))
                                                {
                                                    if (!Members[memberName].SuspendMemberValueUpdate)
                                                        UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), memberName), _Value.GetType(), memberName, memberName);
                                                }


                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region  Update only the defined member
                                            if (Members.ContainsKey(member) && !Members[member].SuspendMemberValueUpdate)
                                                UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), member), _Value.GetType(), member, member);
                                            #endregion
                                        }
                                    }
                                    stateTransition.Consistent = true;
                                }
                            }
                            else
                            {
                                if (UserInterfaceSession.MainThreadID != System.Threading.Thread.CurrentThread.ManagedThreadId)
                                {
                                    AsynchObjectStateChanges.Enqueue(new ObjectChangeStateEvent(_object, member));
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(member))
                                    {
                                        #region Update all members
                                        bool formUpdated = false;
                                        try
                                        {

                                            if (Value is MarshalByRefObject)
                                            {
                                                OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(Value as MarshalByRefObject, RootObjectNode.Paths);

                                                Member _member = null;
                                                if (Value != null)
                                                    _member = new OOAdvantech.UserInterface.Runtime.Member(RootObjectNode.Name, null, null, Value.GetType());

                                                UserInterfaceSession.LoadDisplayedValues(RootObjectNode, structureSet, _member, -1);
                                                formUpdated = true;

                                            }

                                        }
                                        catch (System.Exception error)
                                        {
                                            throw;
                                        }

                                        if (!formUpdated)
                                        {
                                            foreach (string memberName in new List<string>(Members.Keys))
                                            {
                                                if (!Members[memberName].SuspendMemberValueUpdate)
                                                    UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), memberName), _Value.GetType(), memberName, memberName);
                                            }


                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region  Update only the defined member
                                        if (Members.ContainsKey(member) && !Members[member].SuspendMemberValueUpdate)
                                            UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), member), _Value.GetType(), member, member);
                                        #endregion
                                    }
                                }
                            }

                        }
                        #endregion
                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
                finally
                {
                    UserInterfaceSession.EndControlValuesUpdate();
                }

            }



        }

        /// <MetaDataID>{b5ee784e-6233-4615-892e-d30699b5105e}</MetaDataID>
        Transactions.Transaction _LockTransaction;
        /// <MetaDataID>{370be3d2-45b5-4562-80c0-304525719f41}</MetaDataID>
        internal Transactions.Transaction LockTransaction
        {
            get
            {

                return _LockTransaction;
            }
            set
            {

                if (_LockTransaction == value)
                    return;

                if (_LockTransaction != null && _LockTransaction.OriginTransaction == value)
                    return;


                //TODO υπάρχει περίπτωση να γίνει commit/abord κατα μεχρι να γίνει subscribe το even;
                if (value == null || value.Status == Transactions.TransactionStatus.Continue)
                {

                    if (_LockTransaction != null && _LockTransaction != value)
                        _LockTransaction.TransactionCompleted -= new Transactions.TransactionCompletedEventHandler(LockTransactionCompletted);
                    if (UserInterfaceSession.Transaction != null && UserInterfaceSession.Transaction.Status != Transactions.TransactionStatus.Continue)
                        return;

                    _LockTransaction = value;
                    if (_LockTransaction != null)
                        _LockTransaction.TransactionCompleted += new Transactions.TransactionCompletedEventHandler(LockTransactionCompletted);

                }

            }
        }
        /// <MetaDataID>{80dfafdc-c12e-4c74-9788-191011631d46}</MetaDataID>
        void LockTransactionCompletted(Transactions.Transaction transaction)
        {


            _LockTransaction.TransactionCompleted -= new Transactions.TransactionCompletedEventHandler(LockTransactionCompletted);
            if (transaction.OriginTransaction != null && UserInterfaceSession.Transaction != transaction.OriginTransaction)
            {
                if (this.UserInterfaceSession == UISession.GetUserInterfaceSession(transaction))
                {
                    foreach (Member member in Members.Values)
                        member.Locked = false;

                }
                _LockTransaction = null;
                LockTransaction = transaction.OriginTransaction;
                //_LockTransaction.TransactionCompleted += new Transactions.TransactionCompletedEventHandler(LockTransactionCompletted);
                return;
            }
            if (UserInterfaceSession.Transaction != null && UserInterfaceSession.Transaction.Status == Transactions.TransactionStatus.Continue)
            {
                OOAdvantech.Transactions.Transaction lockTransactionRoot = transaction;
                while (lockTransactionRoot.OriginTransaction != null)
                    lockTransactionRoot = lockTransactionRoot.OriginTransaction;
                if (transaction.OriginTransaction == null || lockTransactionRoot == UserInterfaceSession.Transaction)
                {
                    try
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(UserInterfaceSession.Transaction))
                        {

                            if (this.UserInterfaceSession != UISession.GetUserInterfaceSession(transaction))
                            {
                                try
                                {
                                    this.UserInterfaceSession.StartUpdateCashingData();
                                    if (Value is MarshalByRefObject)
                                    {
                                        OOAdvantech.Collections.StructureSet structureSet = OOAdvantech.Remoting.RemoteObjectValuesCollector.GetValues(Value as MarshalByRefObject, RootObjectNode.Paths);
                                        Member member = new OOAdvantech.UserInterface.Runtime.Member(RootObjectNode.Name, null, null, Value.GetType());
                                        UserInterfaceSession.LoadDisplayedValues(RootObjectNode, structureSet, member, -1);

                                    }
                                    else
                                    {
                                        foreach (System.Collections.Generic.KeyValuePair<string, Member> entry in Members)
                                        {
                                            if (!entry.Value.SuspendMemberValueUpdate)
                                                UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), entry.Key), _Value.GetType(), entry.Key, entry.Key);
                                        }
                                    }
                                }
                                finally
                                {
                                    this.UserInterfaceSession.EndUpdateCashingData();
                                }
                            }
                            foreach (Member member in Members.Values)
                                member.Locked = false;
                            stateTransition.Consistent = true;
                        }
                    }
                    catch (System.Exception error)
                    {
                    }
                }
            }
            else if (UserInterfaceSession.Transaction == null)
            {
                foreach (Member member in Members.Values)
                    member.Locked = false;
            }

            _LockTransaction = null;

        }



        /// <MetaDataID>{8199b837-f203-442e-ba6f-6d4038a3be9f}</MetaDataID>
        void ChangeStateTransactionTransactionCompletted(OOAdvantech.Transactions.Transaction transaction)
        {
            if (_Value != null)
            {
                System.Globalization.CultureInfo culture = this._UIProxy?.UserInterfaceObjectConnection?.Culture;
                if (culture == null)
                    culture = UserInterfaceSession?.StartingFormObjectConnection?.Culture;
                if (culture == null)
                    culture = OOAdvantech.CultureContext.CurrentCultureInfo;

                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, false))
                {
                    try
                    {
                        UserInterfaceSession.StartControlValuesUpdate();
                        if (UserInterfaceSession.Transaction != null)
                        {

                            using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(UserInterfaceSession.Transaction))
                            {
                                //UserInterfaceSession.StartingFormObjectConnection

                                try
                                {
                                    foreach (ObjectChangeStateEvent ObjectChangeStateEvent in TransctionedObjectChangeStateEvents[transaction])
                                    {
                                        if (string.IsNullOrEmpty(ObjectChangeStateEvent.Member))
                                        {
                                            foreach (System.Collections.Generic.KeyValuePair<string, Member> entry in Members)
                                                UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), entry.Key), _Value.GetType(), entry.Key, entry.Key);

                                        }
                                        else
                                            UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), ObjectChangeStateEvent.Member), _Value.GetType(), ObjectChangeStateEvent.Member, ObjectChangeStateEvent.Member);
                                    }
                                }
                                catch (System.Exception error)
                                {
                                    throw;
                                }

                                stateTransition.Consistent = true;
                            }
                        }
                        else
                        {
                            foreach (ObjectChangeStateEvent ObjectChangeStateEvent in TransctionedObjectChangeStateEvents[transaction])
                            {
                                if (string.IsNullOrEmpty(ObjectChangeStateEvent.Member))
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, Member> entry in Members)
                                        UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), entry.Key), _Value.GetType(), entry.Key, entry.Key);

                                }
                                else
                                    UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), ObjectChangeStateEvent.Member), _Value.GetType(), ObjectChangeStateEvent.Member, ObjectChangeStateEvent.Member);

                            }

                        }
                    }
                    finally
                    {
                        UserInterfaceSession.EndControlValuesUpdate();

                    }
                }
            }


            TransctionedObjectChangeStateEvents.Remove(transaction);
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(ChangeStateTransactionTransactionCompletted);

        }

        /// <MetaDataID>{9c6f2bbb-3cc8-41d5-b659-a6d8c7e6781f}</MetaDataID>
        ~DisplayedValue()
        {
            ObjsCount--;

        }





        delegate void UpdateUserInterfaceHandle();


        /// <MetaDataID>{b09779cd-437b-400e-8887-c4ba00ff8992}</MetaDataID>
        internal void UpdateUserInterface()
        {
            if (UserInterfaceSession.StartingFormObjectConnection.InvokeRequired)
            {
                UpdateUserInterfaceHandle updateUserInterfaceHandle = new UpdateUserInterfaceHandle(UpdateUserInterface);
                UserInterfaceSession.StartingFormObjectConnection.SynchroInvoke(updateUserInterfaceHandle, new object[0]);

            }
            else
            {
                //TODO ύπάρχει πρόβλημα όταν η display value φτιαχνεται από ένα dialog που κλείνει αλλά δε ανόικη σε αυτό η transaction 
                // που ανοίκει το display value
                if (UserInterfaceSession.Transaction != null)
                {

                    using (SystemStateTransition unloadTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(UserInterfaceSession.Transaction))
                        {
                            foreach (string memberName in new List<string>(Members.Keys))
                                UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), memberName), _Value.GetType(), memberName, memberName);
                            stateTransition.Consistent = true;
                        }
                        unloadTransition.Consistent = true;
                    }

                }
                else
                {
                    foreach (string memberName in new List<string>(Members.Keys))
                        UserInterfaceSession.UpdateValue(_Value, UISession.GetValue(_Value, _Value.GetType(), memberName), _Value.GetType(), memberName, memberName);

                }
            }


        }

        /// <MetaDataID>{46291cad-e403-4cc6-b04d-bd716bacc64b}</MetaDataID>
        internal bool _HasLockRequest = false;

        /// <MetaDataID>{5153f3f3-9a3f-4f13-b815-fffc0ae04f6a}</MetaDataID>
        internal bool HasLockRequest
        {
            set
            {
                _HasLockRequest = value;

                //if (_HasLockRequest)
                //{
                //    if (!AssignedToLockControlSession)
                //        this.UserInterfaceSession.AttachTransactionLockEvents(this);
                //}
            }
            get
            {
                return _HasLockRequest;
            }
        }


        /// <MetaDataID>{e33bc55e-89ab-4b29-bcfc-06ba299ca8e9}</MetaDataID>
        System.Collections.Generic.List<Type> Types = null;
        /// <MetaDataID>{1fe3d567-b38e-48d4-9f3a-c64bf7e1806e}</MetaDataID>
        internal void AddType(Type type)
        {
            if (type == null)
                return;
            if (Types == null)
            {
                Types = new List<Type>();
                Types.Add(type);
            }
            else
            {
                if (!Types.Contains(type))
                    Types.Add(type);
            }

        }

        /// <MetaDataID>{534b8d4a-55cb-4a96-a4a7-927e1dc4af02}</MetaDataID>
        internal void AttatchEventHandlers()
        {
            if (_Value != null)
            {
                foreach (System.Reflection.EventInfo eventInfo in GetEventsFromHierarchy(_Value.GetType()))
                {
                    if (eventInfo.EventHandlerType == typeof(OOAdvantech.ObjectChangeStateHandle))
                        UserInterfaceSession.SubscribeOnEvent(eventInfo, _Value, new OOAdvantech.ObjectChangeStateHandle(OnObjectChangeState));
                }

                if (_Value is System.ComponentModel.INotifyPropertyChanged)
                    (_Value as System.ComponentModel.INotifyPropertyChanged).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnPropertyChanged);

            }

        }



        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        /// <MetaDataID>{a5d7edad-62d1-4fee-ac94-87106005abd1}</MetaDataID>
        UIProxy _UIProxy;
        /// <MetaDataID>{4053e666-bf1e-4171-8e3c-7a384ea07de6}</MetaDataID>
        public UIProxy UIProxy
        {
            get
            {
                if (_UIProxy == null && Value != null)
                    _UIProxy = new UIProxy(this, Value.GetType());

                return _UIProxy;
            }
        }

        //public UIProxy GetUIProxy(UserInterfaceObjectConnection userInterfaceObjectConnection, Type type)
        //{

        //     if (_UIProxy == null && Value != null)
        //     {
        //            _UIProxy = new UIProxy(this, type);

        //        return _UIProxy
        //}

        /// <MetaDataID>{b10512c8-0158-4a68-b866-4379f67b5c8f}</MetaDataID>
        Dictionary<UserInterfaceObjectConnection, UIProxy> _UIProxies = new Dictionary<UserInterfaceObjectConnection, UIProxy>();
        /// <MetaDataID>{1429b3e4-cd5a-4c7a-afba-2c990fccc1b0}</MetaDataID>
        public UIProxy GetUIProxy(UserInterfaceObjectConnection userInterfaceObjectConnection, bool create = true)
        {
            UIProxy dynamicUIProxy = null;
            if (Value != null && !_UIProxies.TryGetValue(userInterfaceObjectConnection, out dynamicUIProxy))
            {

                if (create)
                {
                    if (Value is MarshalByRefObject)
                    {
                        dynamicUIProxy = new UIProxy(this, Value.GetType(), userInterfaceObjectConnection);
                        _UIProxies[userInterfaceObjectConnection] = dynamicUIProxy;
                    }
                }

            }
            return dynamicUIProxy;
        }





        /// <MetaDataID>{b10512c8-0158-4a68-b866-4379f67b5c8f}</MetaDataID>
        Dictionary<UserInterfaceObjectConnection, DynamicUIProxy> _DynamicUIProxies = new Dictionary<UserInterfaceObjectConnection, DynamicUIProxy>();
        /// <MetaDataID>{1429b3e4-cd5a-4c7a-afba-2c990fccc1b0}</MetaDataID>
        public DynamicUIProxy GetDynamicUIProxy(UserInterfaceObjectConnection userInterfaceObjectConnection, Type type)
        {
            DynamicUIProxy dynamicUIProxy = null;
            if (Value != null && !_DynamicUIProxies.TryGetValue(userInterfaceObjectConnection, out dynamicUIProxy))
            {
                dynamicUIProxy = CodeInjection.CreateProxy(type, this, userInterfaceObjectConnection) as DynamicUIProxy;
                _DynamicUIProxies[userInterfaceObjectConnection] = dynamicUIProxy;

            }
            return dynamicUIProxy;
        }






    }
}
