using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using OOAdvantech.UserInterface.Runtime;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using OOAdvantech.Transactions;


namespace OOAdvantech.UserInterface.Runtime
{

    /// <MetaDataID>{b739ed66-c2f4-41d9-b3bc-c94bc96923cd}</MetaDataID>
    public class UIProxy : RealProxy, System.Runtime.Remoting.IRemotingTypeInfo, IPathDataDisplayer, System.ComponentModel.INotifyPropertyChanged
    {

        public bool CrossSessionValue { get; private set; }
        public static UIProxy GetUIProxy(object obj)
        {
            if (obj == null)
                return null;
            return System.Runtime.Remoting.RemotingServices.GetRealProxy(obj) as UIProxy;
        }
        /// <MetaDataID>{a1db8797-2511-4837-89c8-a7b9a4de9a9f}</MetaDataID>
        public bool CanCastTo(Type castType, object o)
        {
            if (castType == typeof(UIProxy))
                return true;

            if (castType == Type)
                return true;
            bool CanCast = false;
            if (castType == typeof(System.ComponentModel.INotifyPropertyChanged))
                return true;
            if (castType == typeof(IPathDataDisplayer))
                return true;
            try
            {
                if (_RealTransparentProxy != null)
                    CanCast = castType.IsInstanceOfType(_RealTransparentProxy);
                else
                {
                    CanCast = castType == Type;
                    if (!CanCast)
                        CanCast = Type.IsSubclassOf(castType);
                }

            }
            finally
            {
            }
            if (!CanCast && DisplayedValue != null && castType == typeof(System.ComponentModel.INotifyPropertyChanged))
                return true;
            return CanCast;
        }

        /// <MetaDataID>{961b7f49-0285-4f0a-aa78-ac8f80c3a0c6}</MetaDataID>
        public string TypeName
        {
            get
            {
                return GetProxiedType().ToString();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        public static T GetRealObject<T>(object _obj) where T : class
        {
            object realObject = null;
            UIProxy uiProxy = UIProxy.GetUIProxy(_obj);
            if (uiProxy != null)
                realObject = uiProxy.RealTransparentProxy;
            else
                realObject = _obj;

            if (realObject is T)
                return (T)realObject;
            else
                return default(T);
        }


        public object RealTransparentProxy
        {
            get
            {
                return _RealTransparentProxy;
            }
        }
        /// <MetaDataID>{3be717b9-f23f-48ff-b272-245c09789078}</MetaDataID>
        internal object _RealTransparentProxy;

        /// <MetaDataID>{282582c8-2abd-440a-9122-5db99d0e0dcb}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier Classifier;

        System.Type Type;
        /// <MetaDataID>{86db018e-8e87-4b88-a1c2-00534b892aad}</MetaDataID>
        string URI;
        /// <MetaDataID>{c88291d1-4c5b-4c67-a32e-9631c64dc372}</MetaDataID>
        public UIProxy(Type type)
            : base(typeof(MarshalByRefObject))
        {
            IntPtr str = (IntPtr)GetType().BaseType.GetMethod("GetDefaultStub", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[0]);
            Type = type;
        }

        static IntPtr _defaultStub;
        static IntPtr _defaultStubValue = new IntPtr(-1);
        static object _defaultStubData;
        static UIProxy()
        {
            _defaultStub = (IntPtr)typeof(RealProxy).GetMethod("GetDefaultStub", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[0]);
            _defaultStubData = _defaultStubValue;
        }
        public readonly DisplayedValue DisplayedValue;
        public UIProxy(DisplayedValue displayedValue, Type type)
            : base(type)
        {

            DisplayedValue = displayedValue;
            Type = type;


            object theRealObject = DisplayedValue.Value;

            // RealProxy uses the Type to generate a transparent proxy.
            _RealTransparentProxy = theRealObject;
            Classifier = UserInterfaceObjectConnection.GetClassifier(type);
            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(theRealObject as MarshalByRefObject))
            {
                ObjRef myObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(theRealObject as MarshalByRefObject);
                URI = myObjRef.URI;
            }



        }
        public UIProxy(DisplayedValue displayedValue, Type type, UserInterfaceObjectConnection userInterfaceObjectConnection, bool crossSessionValue = false)
            : this(displayedValue, type)
        {
            _UserInterfaceObjectConnection = userInterfaceObjectConnection;

            if (_RealTransparentProxy is IConnectedCommand)
                (_RealTransparentProxy as IConnectedCommand).UserInterfaceObjectConnection = userInterfaceObjectConnection;

        }



        public UIProxy(object realObject, Type type)
            : base(type)
        {

            Type = type;


            object theRealObject = realObject;

            // RealProxy uses the Type to generate a transparent proxy.
            _RealTransparentProxy = theRealObject;
            //Classifier = UserInterfaceObjectConnection.GetClassifier(type);
            //if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(theRealObject as MarshalByRefObject))
            //{
            //    ObjRef myObjRef = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(theRealObject as MarshalByRefObject);
            //    URI = myObjRef.URI;
            //}



        }

        //public UIProxy(object realTransparentProxy, Type type) : base(type)
        //{
        //    this._RealTransparentProxy = realTransparentProxy;
        //    Type = type;
        //    this.CrossSessionValue = crossSessionValue;
        //}
        public UIProxy(UIProxy uiProxy, bool crossSessionValue) : base(uiProxy.Type)
        {
            DisplayedValue = uiProxy.DisplayedValue;
            this._RealTransparentProxy = uiProxy.RealTransparentProxy;
            Type = uiProxy.Type;
            _UserInterfaceObjectConnection = uiProxy.UserInterfaceObjectConnection;
            this.CrossSessionValue = crossSessionValue;
        }
        object _mtp;
        public override object GetTransparentProxy()
        {
            //try
            //{
            //    if (_mtp == null)
            //    {
            //        GetType().BaseType.GetField("_tp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, null);
            //        _mtp = typeof(RemotingServices).GetMethod("CreateTransparentProxy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, null, new Type[4] { typeof(RealProxy), typeof(Type), typeof(IntPtr), typeof(object) }, null).Invoke(null, new object[4] { this, Type, _defaultStub, _defaultStubData });
            //    }
            //}
            //catch (Exception error)
            //{
            //    throw;
            //}
            return base.GetTransparentProxy();
        }

        /// <MetaDataID>{545458ba-9d09-400a-9e8d-21567f476301}</MetaDataID>
        public System.Runtime.Remoting.Messaging.IMessage TryInvokeLocal(System.Runtime.Remoting.Messaging.IMessage msg, out bool LocalCall)
        {

            //string HostName=System.Net.Dns.GetHostName();
            IMethodReturnMessage ReturnMessage = null;
            System.Reflection.MethodBase methodBase = (msg as IMethodCallMessage).MethodBase;
            LocalCall = false;
            object[] outArgs = null;
            object ReturnValue = null;

            string typeName = Type.Name;
            if (methodBase.DeclaringType == typeof(IPathDataDisplayer) || methodBase.DeclaringType == typeof(INotifyPropertyChanged))
            {

                ReturnValue = methodBase.Invoke(this, (msg as IMethodCallMessage).Args);
                outArgs = new object[0];
                LocalCall = true;
            }
            else
            {

                if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                    methodBase.Name == "GetType")
                {

                    outArgs = new object[0];
                    ReturnValue = Type;// _RealTransparentProxy.GetType();



                    //ReturnValue = CodeInjection.EmiProxyType(Type);// typeof(MarshalByRefObject);
                    LocalCall = true;
                }
                else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                    methodBase.Name == "GetHashCode")
                {

                    outArgs = new object[0];
                    if (_RealTransparentProxy != null && !CrossSessionValue)
                        ReturnValue = _RealTransparentProxy.GetHashCode();
                    else
                        ReturnValue = GetHashCode();

                    LocalCall = true;
                }
                else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                   methodBase.Name == "Equals")
                {

                    bool IsEquals = true;
                    Object obj = (msg as IMethodCallMessage).Args[0];

                    if (!(obj is MarshalByRefObject))
                    {
                        IsEquals = false;
                    }
                    else if (RemotingServices.GetObjectUri(obj as MarshalByRefObject) == URI && URI != null)
                    {
                        IsEquals = true;
                    }
                    else if (obj == GetTransparentProxy())
                    {
                        IsEquals = true;
                    }
                    else if (_RealTransparentProxy != null)
                        IsEquals = _RealTransparentProxy.Equals(obj);
                    else if (_RealTransparentProxy == null && obj != null)
                        IsEquals = false;

                    outArgs = new object[0];
                    ReturnValue = IsEquals;
                    LocalCall = true;
                }
            }

            if (LocalCall)
            {
                ReturnMessage = new System.Runtime.Remoting.Messaging.ReturnMessage(
                    ReturnValue,	//ReturnValue
                    outArgs,			//Object[] outArgs
                    outArgs.Length,					//int outArgsCount
                    (LogicalCallContext)msg.Properties["__CallContext"],				//LogicalCallContext callCtx
                    (System.Runtime.Remoting.Messaging.IMethodCallMessage)msg);
                return ReturnMessage;

            }
            return null;
        }
        static internal bool InInvoke = false;
        //IPathDataDisplayer 

        /// <MetaDataID>{d71e290f-a8de-401b-ab58-3f5c3006077a}</MetaDataID>
        public override IMessage Invoke(IMessage msg)
        {
            try
            {
                InInvoke = true;
                Object ret = null;

                IMethodMessage MethodMessage = msg as IMethodMessage;
                IMethodReturnMessage retMsg = null;
                Object[] outArgs = null;
                if (MethodMessage != null)
                {
                    bool LocalCall;
                    retMsg = TryInvokeLocal(msg, out LocalCall) as IMethodReturnMessage;
                    if (LocalCall)
                        return retMsg;
                    if (MethodMessage.MethodName == "FieldGetter")
                    {
                        string fieldName = MethodMessage.Args[1] as string;
                        string declarationType = MethodMessage.Args[0] as string;

                        ret = GetValue(ModulePublisher.ClassRepository.GetType(declarationType, "").GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));


                        outArgs = new object[3] { null, null, ret };
                        retMsg = new ReturnMessage(
                                typeof(void),           //Object ret
                                outArgs,       //Object[] outArgs
                                outArgs.Length,             //int outArgsCount
                                null,          //LogicalCallContext callCtx
                                (IMethodCallMessage)MethodMessage   //IMethodCallMessage mcm
                                );
                        return retMsg;
                    }
                    else if (MethodMessage.MethodName == "FieldSetter")
                    {
                        string fieldName = MethodMessage.Args[1] as string;
                        string declarationType = MethodMessage.Args[0] as string;
                        SetValue(ModulePublisher.ClassRepository.GetType(declarationType, "").GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance), MethodMessage.Args[2]);

                        outArgs = new object[3] { null, null, null };
                        retMsg = new ReturnMessage(
                                typeof(void),           //Object ret
                                outArgs,       //Object[] outArgs
                                outArgs.Length,             //int outArgsCount
                                null,          //LogicalCallContext callCtx
                                (IMethodCallMessage)MethodMessage   //IMethodCallMessage mcm
                                );
                        return retMsg;
                    }
                    else
                    {
                        System.Reflection.PropertyInfo property = null;
                        bool propertySet = false;

                        if (MethodMessage.MethodName.IndexOf("get_") == 0)
                        {
                            string propertyName = MethodMessage.MethodName.Substring("get_".Length);

                            property = MethodMessage.MethodBase.DeclaringType.GetProperty(propertyName);
                        }
                        else if (MethodMessage.MethodName.IndexOf("set_") == 0)
                        {
                            string propertyName = MethodMessage.MethodName.Substring("set_".Length);
                            property = MethodMessage.MethodBase.DeclaringType.GetProperty(propertyName);
                            if (property != null)
                                propertySet = true;
                        }
                        if (property != null && !propertySet)
                        {
                            if (_RealTransparentProxy != null)
                            {
                                property = _RealTransparentProxy.GetType().GetProperty(property.Name);
                                ret = GetValue(property);
                            }
                            else
                            {
                                ret = OOAdvantech.AccessorBuilder.GetDefaultValue(property.PropertyType);
                            }

                        }
                        else if (property != null && propertySet)
                            SetValue(property, MethodMessage.Args[0]);
                        else
                        {
                            if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.Culture != null)
                            {
                                using (OOAdvantech.CultureContext cultureContext = new CultureContext(UserInterfaceObjectConnection.Culture, UserInterfaceObjectConnection.UseDefaultCultureWhenValueMissing))
                                {
                                    if (MethodMessage.MethodBase.DeclaringType == typeof(System.ComponentModel.INotifyPropertyChanged))
                                    {
                                        if (ObjectChangeStateManager == null)
                                            ObjectChangeStateManager = new ObjectChangeStateManager(this);//new NotifyPropertyChangedBridge(this));
                                        ret = MethodMessage.MethodBase.DeclaringType.InvokeMember(MethodMessage.MethodName, System.Reflection.BindingFlags.InvokeMethod, null, ObjectChangeStateManager.PathDataDisplayer, MethodMessage.Args);
                                    }
                                    else if (_RealTransparentProxy != null)
                                        ret = MethodMessage.MethodBase.Invoke(_RealTransparentProxy, MethodMessage.Args);
                                    else
                                        ret = OOAdvantech.AccessorBuilder.GetDefaultValue((MethodMessage.MethodBase as System.Reflection.MethodInfo).ReturnType);

                                    ret = GetWPFCompatibleValue(MethodMessage.MethodBase, DisplayedValue.UserInterfaceSession.GetDisplayedValue(ret));
                                }
                            }
                            else
                            {
                                if (MethodMessage.MethodBase.DeclaringType == typeof(System.ComponentModel.INotifyPropertyChanged))
                                {
                                    if (ObjectChangeStateManager == null)
                                        ObjectChangeStateManager = new ObjectChangeStateManager(this);//new NotifyPropertyChangedBridge(this));
                                    ret = MethodMessage.MethodBase.DeclaringType.InvokeMember(MethodMessage.MethodName, System.Reflection.BindingFlags.InvokeMethod, null, ObjectChangeStateManager.PathDataDisplayer, MethodMessage.Args);
                                }
                                else if (_RealTransparentProxy != null)
                                    ret = MethodMessage.MethodBase.Invoke(_RealTransparentProxy, MethodMessage.Args);
                                else
                                    ret = OOAdvantech.AccessorBuilder.GetDefaultValue((MethodMessage.MethodBase as System.Reflection.MethodInfo).ReturnType);
                            }
                        }
                    }
                }

                if (ret is IConnectedCommand)
                    (ret as IConnectedCommand).UserInterfaceObjectConnection = UserInterfaceObjectConnection;

                if (ret is IConnectedCommand && !string.IsNullOrWhiteSpace((ret as IConnectedCommand).Name))
                {

                }


                retMsg = new ReturnMessage(
                ret,           //Object ret
                outArgs,       //Object[] outArgs
                0,             //int outArgsCount
                null,          //LogicalCallContext callCtx
                (IMethodCallMessage)msg   //IMethodCallMessage mcm
                );
                return retMsg;
            }
            finally
            {
                InInvoke = false;
            }

        }

        /// <MetaDataID>{8ff3f80e-abcc-4689-bc4c-4fceb08c8468}</MetaDataID>
        private void SetValue(System.Reflection.MemberInfo memberInfo, object value)
        {
            if (_RealTransparentProxy == null)
                return;
            UIProxy uiProxy = UIProxy.GetUIProxy(value);
            if (uiProxy != null)
                value = uiProxy._RealTransparentProxy;

            if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.Culture != null)
            {
                using (OOAdvantech.CultureContext cultureContext = new CultureContext(UserInterfaceObjectConnection.Culture, UserInterfaceObjectConnection.UseDefaultCultureWhenValueMissing))
                {
                    if (WPF)
                        UserInterfaceObjectConnection.SetValue(DisplayedValue.Value, value, MetaDataRepository.Classifier.GetClassifier(_RealTransparentProxy.GetType()), memberInfo.Name);
                    else
                        DisplayedValue.UserInterfaceSession.SetValue(DisplayedValue.Value, value, Type, memberInfo.Name);
                }
            }
            else
            {
                if (WPF)
                    UserInterfaceObjectConnection.SetValue(DisplayedValue.Value, value, MetaDataRepository.Classifier.GetClassifier(_RealTransparentProxy.GetType()), memberInfo.Name);
                else
                    DisplayedValue.UserInterfaceSession.SetValue(DisplayedValue.Value, value, Type, memberInfo.Name);
            }

            //UserInterfaceObjectConnection userInterfaceObjectConnection = UserInterfaceObjectConnection.CurrentViewControlObject;
            //if (UserInterfaceObjectConnection != null)
            //    userInterfaceObjectConnection = UserInterfaceObjectConnection;


            //if (value is MarshalByRefObject &&
            //    OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(value as MarshalByRefObject) &&
            //    System.Runtime.Remoting.RemotingServices.GetRealProxy(value) is UIProxy)
            //    value = (System.Runtime.Remoting.RemotingServices.GetRealProxy(value) as UIProxy)._RealTransparentProxy;
            //    UISession.s.SetValue(_RealTransparentProxy, value, Classifier.GetExtensionMetaObject(typeof(Type)) as Type, memberInfo.Name);
        }

        ObjectChangeStateManager ObjectChangeStateManager = null;
        /// <MetaDataID>{907c35d5-0f0a-48a7-a30c-52d82ac0119e}</MetaDataID>
        private object GetValue(System.Reflection.MemberInfo memberInfo)
        {
            if (_RealTransparentProxy == null)
            {
                if (memberInfo is System.Reflection.PropertyInfo)
                    return OOAdvantech.AccessorBuilder.GetDefaultValue((memberInfo as System.Reflection.PropertyInfo).PropertyType);

                if (memberInfo is System.Reflection.FieldInfo)
                    return OOAdvantech.AccessorBuilder.GetDefaultValue((memberInfo as System.Reflection.FieldInfo).FieldType);

            }
            else if (DisplayedValue == null)
            {

                if (memberInfo is System.Reflection.PropertyInfo)
                    return OOAdvantech.AccessorBuilder.GetPropertyAccessor(memberInfo as System.Reflection.PropertyInfo).GetValue(_RealTransparentProxy);

                if (memberInfo is System.Reflection.FieldInfo)
                    return OOAdvantech.AccessorBuilder.GetFieldAccessor(memberInfo as System.Reflection.FieldInfo).GetValue(_RealTransparentProxy);

            }


            DisplayedValue displayedValue = null;

            if (UserInterfaceObjectConnection != null && UserInterfaceObjectConnection.Culture != null)
            {
                using (OOAdvantech.CultureContext cultureContext = new CultureContext(UserInterfaceObjectConnection.Culture, UserInterfaceObjectConnection.UseDefaultCultureWhenValueMissing))
                {
                    if (WPF)
                    {
                        if (ObjectChangeStateManager == null)
                            ObjectChangeStateManager = new ObjectChangeStateManager(this);
                        if (OOAdvantech.Transactions.Transaction.Current != null && UserInterfaceObjectConnection.Transaction != null && UserInterfaceObjectConnection.Transaction.IsNestedTransaction(Transaction.Current))
                        {
                            displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(DisplayedValue.Value, MetaDataRepository.Classifier.GetClassifier(_RealTransparentProxy.GetType()), memberInfo.Name, ObjectChangeStateManager);
                            return GetWPFCompatibleValue(memberInfo, displayedValue);
                        }
                        else
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                            {
                                displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(DisplayedValue.Value, MetaDataRepository.Classifier.GetClassifier(_RealTransparentProxy.GetType()), memberInfo.Name, ObjectChangeStateManager);
                                stateTransition.Consistent = true;
                                return GetWPFCompatibleValue(memberInfo, displayedValue);
                            }
                        }
                    }
                    else
                        displayedValue = DisplayedValue.UserInterfaceSession.GetDisplayedValue(DisplayedValue.Value, memberInfo.DeclaringType, memberInfo.Name, ObjectChangeStateManager);

                }
            }
            else
            {
                if (WPF)
                {
                    if (ObjectChangeStateManager == null)
                        ObjectChangeStateManager = new ObjectChangeStateManager(this);
                    if (OOAdvantech.Transactions.Transaction.Current != null && UserInterfaceObjectConnection.Transaction != null && UserInterfaceObjectConnection.Transaction.IsNestedTransaction(Transaction.Current))
                    {
                        displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(DisplayedValue.Value, MetaDataRepository.Classifier.GetClassifier(_RealTransparentProxy.GetType()), memberInfo.Name, ObjectChangeStateManager);
                        return GetWPFCompatibleValue(memberInfo, displayedValue);
                    }
                    else
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                        {
                            displayedValue = UserInterfaceObjectConnection.GetDisplayedValue(DisplayedValue.Value, MetaDataRepository.Classifier.GetClassifier(_RealTransparentProxy.GetType()), memberInfo.Name, ObjectChangeStateManager);
                            stateTransition.Consistent = true;
                            return GetWPFCompatibleValue(memberInfo, displayedValue);
                        }
                    }
                }
                else
                    displayedValue = DisplayedValue.UserInterfaceSession.GetDisplayedValue(DisplayedValue.Value, memberInfo.DeclaringType, memberInfo.Name, ObjectChangeStateManager);

            }



            if (memberInfo is System.Reflection.PropertyInfo && ((memberInfo as System.Reflection.PropertyInfo).PropertyType.IsSubclassOf(typeof(string)) ||
                (memberInfo as System.Reflection.PropertyInfo).PropertyType == typeof(string)))
                return displayedValue.Value;

            if (memberInfo is System.Reflection.FieldInfo && ((memberInfo as System.Reflection.FieldInfo).FieldType.IsSubclassOf(typeof(string)) ||
                (memberInfo as System.Reflection.FieldInfo).FieldType == typeof(string)))
                return displayedValue.Value;


            object retValue = displayedValue.UIProxy;
            if (retValue is DisplayedValue)
                retValue = (retValue as DisplayedValue).Value;
            if (retValue is UIProxy)
                retValue = (retValue as UIProxy).GetTransparentProxy();
            return retValue;
        }

        private object GetWPFCompatibleValue(System.Reflection.MemberInfo memberInfo, DisplayedValue displayedValue)
        {
            if (memberInfo is System.Reflection.PropertyInfo && ((memberInfo as System.Reflection.PropertyInfo).PropertyType.IsSubclassOf(typeof(string)) ||
                (memberInfo as System.Reflection.PropertyInfo).PropertyType == typeof(string)))
                return displayedValue.Value;

            if (memberInfo is System.Reflection.PropertyInfo && ((memberInfo as System.Reflection.PropertyInfo).PropertyType.IsSubclassOf(typeof(string)) ||
              (memberInfo as System.Reflection.PropertyInfo).PropertyType.IsValueType))
                return displayedValue.Value;

            if (memberInfo is System.Reflection.FieldInfo && ((memberInfo as System.Reflection.FieldInfo).FieldType.IsSubclassOf(typeof(string)) ||
                (memberInfo as System.Reflection.FieldInfo).FieldType == typeof(string)))
                return displayedValue.Value;
            if (displayedValue.Members.ContainsKey("Items"))
            {

                Transaction transaction = UserInterfaceObjectConnection.ScoopTransaction;
                using (SystemStateTransition cancelScoopTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if (transaction != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
                        {
                            IList list = null;
                            foreach (var item in displayedValue.Members["Items"].ValuesCollection)
                            {
                                object proxyItem = null;
                                if (item.Value == null || item.Value is System.String || item.Value is Enum || item.Value.GetType().IsValueType)
                                    proxyItem = item.Value;
                                else
                                {

                                    UIProxy uiProxy = null;
                                    if (item.CrossSessionValue != null)
                                        uiProxy = item.CrossSessionValue;
                                    else
                                    {
                                        uiProxy = item.GetUIProxy(UserInterfaceObjectConnection);
                                        if (uiProxy != null)
                                            uiProxy.CrossSessionValue = CrossSessionValue;
                                    }
                                    if (uiProxy != null)
                                        proxyItem = uiProxy.GetTransparentProxy();
                                    else
                                        proxyItem = proxyItem = item.Value;

                                }
                                if (list == null)
                                {
                                    if (displayedValue.Value.GetType().IsGenericType && typeof(System.Collections.ObjectModel.ReadOnlyCollection<>) == displayedValue.Value.GetType().GetGenericTypeDefinition())
                                        list = Activator.CreateInstance(typeof(List<>).MakeGenericType(displayedValue.Value.GetType().GetGenericArguments()[0])) as IList;
                                    else
                                        list = Activator.CreateInstance(displayedValue.Value.GetType()) as IList;//typeof(System.Collections.ObjectModel.ObservableCollection<>).MakeGenericType(elemetnType)) as IList;
                                }
                                list.Add(proxyItem);
                            }
                            stateTransition.Consistent = true;
                            if (list == null)
                            {
                                if (list == null)
                                {
                                    if (displayedValue.Value != null)
                                    {
                                        if (displayedValue.Value.GetType().IsGenericType && typeof(System.Collections.ObjectModel.ReadOnlyCollection<>) == displayedValue.Value.GetType().GetGenericTypeDefinition())
                                            list = Activator.CreateInstance(typeof(List<>).MakeGenericType(displayedValue.Value.GetType().GetGenericArguments()[0])) as IList;
                                        else
                                            list = Activator.CreateInstance(displayedValue.Value.GetType()) as IList;//typeof(System.Collections.ObjectModel.ObservableCollection<>).MakeGenericType(elemetnType)) as IList;
                                    }
                                }
                                return list;
                            }

                            else
                            {
                                if (displayedValue.Value.GetType().IsGenericType && typeof(System.Collections.ObjectModel.ReadOnlyCollection<>) == displayedValue.Value.GetType().GetGenericTypeDefinition())
                                    return list.GetType().GetMethod("AsReadOnly").Invoke(list, new object[0]);
                                return list;
                            }
                        }
                    }
                    else
                    {
                        IList list = null;
                        foreach (var item in displayedValue.Members["Items"].ValuesCollection)
                        {
                            object proxyItem = null;
                            if (item.Value == null || item.Value is System.String || item.Value is Enum || item.Value.GetType().IsValueType)
                                proxyItem = item.Value;
                            else
                            {


                                UIProxy uiProxy = null;
                                if (item.CrossSessionValue != null)
                                    uiProxy = item.CrossSessionValue;
                                else
                                {
                                    uiProxy = item.GetUIProxy(UserInterfaceObjectConnection);
                                    if (uiProxy != null)
                                        uiProxy.CrossSessionValue = CrossSessionValue;
                                }

                                if (uiProxy != null)
                                    proxyItem = uiProxy.GetTransparentProxy();
                                else
                                    proxyItem = proxyItem = item.Value;

                            }
                            if (list == null)
                                list = Activator.CreateInstance(displayedValue.Value.GetType()) as IList;//typeof(System.Collections.ObjectModel.ObservableCollection<>).MakeGenericType(elemetnType)) as IList;
                            list.Add(proxyItem);
                        }

                        if (list == null)
                            return displayedValue.Value;
                        else
                            return list;

                    }
                }
            }


            Type memperType = null;
            
            if(memberInfo is System.Reflection.PropertyInfo)
                memperType = (memberInfo as System.Reflection.PropertyInfo).PropertyType;

            if (memberInfo is System.Reflection.FieldInfo)
                memperType = (memberInfo as System.Reflection.FieldInfo).FieldType;

            if (memberInfo is System.Reflection.MethodInfo)
                memperType = (memberInfo as System.Reflection.MethodInfo).ReturnType;


            object retValue = displayedValue;
            if (!DynamicUIProxy.TypeExcluded(memperType))
            {
                if (displayedValue.CrossSessionValue != null)
                    retValue = displayedValue.CrossSessionValue;
                else
                {

                    retValue = displayedValue.GetUIProxy(_UserInterfaceObjectConnection);
                    if ((retValue is UIProxy))
                        (retValue as UIProxy).CrossSessionValue = CrossSessionValue;
                }
                if (retValue == null)
                    retValue = displayedValue;
            }

            if (retValue is DisplayedValue)
                retValue = (retValue as DisplayedValue).Value;
            if (retValue is UIProxy)
                retValue = (retValue as UIProxy).GetTransparentProxy();
            return retValue;
        }
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


        static bool WPF = true;
        private void AddObservableCollection(System.Reflection.MemberInfo memberInfo, DisplayedValue displayedValue, Type elemetnType)
        {
            IList list = null;
            foreach (var item in displayedValue.Members["Items"].ValuesCollection)
            {
                object proxyItem = null;
                if (item.Value == null || item.Value is System.String || item.Value is Enum || item.Value.GetType().IsValueType)
                    proxyItem = item.Value;
                else
                {
                    UIProxy uiProxy = item.GetUIProxy(UserInterfaceObjectConnection);
                    if (uiProxy != null)
                        proxyItem = uiProxy.GetTransparentProxy();
                    else
                        proxyItem = proxyItem = item.Value;

                }
                if (list == null)
                    list = Activator.CreateInstance(displayedValue.Value.GetType()) as IList;//typeof(System.Collections.ObjectModel.ObservableCollection<>).MakeGenericType(elemetnType)) as IList;
                list.Add(proxyItem);
            }
            if (list == null)
            {
                list = Activator.CreateInstance(typeof(System.Collections.ObjectModel.ObservableCollection<>).MakeGenericType(elemetnType)) as IList;
            }
            displayedValue.Members["ObservableCollection"] = new Member("ObservableCollection", displayedValue, memberInfo.DeclaringType, (memberInfo as System.Reflection.PropertyInfo).PropertyType);
            DisplayedValue displayedObservableCollection = null;
            if (!displayedValue.UserInterfaceSession.TryGetDisplayedValue(list, out displayedObservableCollection))
                displayedObservableCollection = new Runtime.DisplayedValue(list, displayedValue.UserInterfaceSession);
            displayedValue.Members["ObservableCollection"][0] = displayedObservableCollection;
        }

        //private object GetProxy(object obj)
        //{
        //    if (obj is System.MarshalByRefObject && !typeof(T).IsCOMObject)
        //        obj = new UIProxy(realObject as System.MarshalByRefObject, typeof(T)).GetTransparentProxy() as T;
        //    return;


        //}

        public static Type GetClassifierType(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            Type type = classifier.GetExtensionMetaObject<Type>();
            if (type == null)
                type = ModulePublisher.ClassRepository.GetType(classifier.FullName, classifier.ImplementationUnit.AssemblyString);
            if (type != null)
                return type;
            else
            {
                return typeof(object);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        #region IPathDataDisplayer Members

        public object Path
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        public void LoadControlValues()
        {

        }

        public void SaveControlValues()
        {

        }

        UserInterfaceObjectConnection _UserInterfaceObjectConnection;

        public UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _UserInterfaceObjectConnection;
            }
            set
            {
                _UserInterfaceObjectConnection = value;
            }
        }

        public Collections.Generic.List<string> Paths
        {
            get
            {
                return new Collections.Generic.List<string>();
            }
        }

        public bool HasLockRequest
        {
            get
            {
                return true;
            }
        }

        public void DisplayedValueChanged(object sender, MemberChangeEventArg change)
        {
            if (change.Member.Name == "Items" && change.Member.Owner.Members.ContainsKey("ObservableCollection"))
            {
                if (change.Type == ChangeType.ItemsAdded)
                {
                    Type elemetnType = FindIEnumerable(change.Member.Owner.Value.GetType());
                    elemetnType = elemetnType.GetGenericArguments()[0];
                    (change.Member.Owner.Members["ObservableCollection"][0].Value as IList).Insert(change.Index, change.Value.GetDynamicUIProxy(UserInterfaceObjectConnection, elemetnType));
                }
                if (change.Type == ChangeType.ItemsRemoved)
                {
                    Type elemetnType = FindIEnumerable(change.Member.Owner.Value.GetType());
                    elemetnType = elemetnType.GetGenericArguments()[0];
                    (change.Member.Owner.Members["ObservableCollection"][0].Value as IList).RemoveAt(change.Index);
                }

            }
            else
            {
                ///if(change
                if (PropertyChanged != null)
                {
                    if (change.Member != null)
                    {
                        string memberName = change.Member.Name;
                        if (change.Member.Name == "Items" && DisplayedValue != null)
                        {
                            foreach (Member member in DisplayedValue.Members.Values)
                            {
                                if (member.HasValuesCollection && member[0].Members["Items"] == change.Member)
                                {
                                    memberName = member.Name;
                                    break;
                                }
                            }
                        }
                        if (UserInterfaceObjectConnection.State != ViewControlObjectState.Passive)
                        {

                            PropertyChanged(this.GetTransparentProxy(), new PropertyChangedEventArgs(memberName));
                        }
                    }

                }
            }

        }

        public void LockStateChange(object sender)
        {

        }

        #endregion

    }

    /// <MetaDataID>{1c39c11a-0836-42e1-95e3-652e06ef58d1}</MetaDataID>
    class NotifyPropertyChangedBridge : IPathDataDisplayer, System.ComponentModel.INotifyPropertyChanged
    {
        UIProxy UiProxy;
        public NotifyPropertyChangedBridge(UIProxy uiProxy)
        {
            UiProxy = uiProxy;

        }
        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IPathDataDisplayer Members

        public object Path
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        public void LoadControlValues()
        {

        }

        public void SaveControlValues()
        {

        }

        public UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return null;
            }
        }

        public Collections.Generic.List<string> Paths
        {
            get
            {
                return new Collections.Generic.List<string>();
            }
        }

        public bool HasLockRequest
        {
            get
            {
                return false;
            }
        }

        public void DisplayedValueChanged(object sender, MemberChangeEventArg change)
        {
            if (PropertyChanged != null)
            {
                if (change.Member != null)
                    PropertyChanged(UiProxy.GetTransparentProxy(), new PropertyChangedEventArgs(change.Member.Name));

            }

        }

        public void LockStateChange(object sender)
        {

        }

        #endregion
    }




}
