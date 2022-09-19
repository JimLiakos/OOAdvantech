using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Runtime.InteropServices;


namespace RoseMetaDataRepository
{
    /// <MetaDataID>{2ADC3006-049C-44B8-9555-F0D3133261B7}</MetaDataID>
    internal class RoseObjectProxy : RealProxy, System.Runtime.Remoting.IRemotingTypeInfo
    {
        internal object _RoseObject;

         
        /// <MetaDataID>{F1C902EB-99FE-4D4C-8F2B-4219C9A6710A}</MetaDataID>
        RoseObjectProxy(object theRealObject)
            : base(theRealObject.GetType())
        {
            // RealProxy uses the Type to generate a transparent proxy.
            _RoseObject = theRealObject;
            //RoseObjects.Add(this); 

        }
        /// <MetaDataID>{B7A2F8F1-43D8-45AA-A3A8-F1879F70F2C4}</MetaDataID>
        public System.Runtime.Remoting.Messaging.IMessage TryInvokeLocal(System.Runtime.Remoting.Messaging.IMessage msg, out bool LocalCall)
        {
            //string HostName=System.Net.Dns.GetHostName();
            IMethodReturnMessage ReturnMessage = null;
            System.Reflection.MethodBase methodBase = (msg as IMethodCallMessage).MethodBase;
            LocalCall = false;
            object[] outArgs = null;
            object ReturnValue = null;

            if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                methodBase.Name == "GetType")
            {
                outArgs = new object[0];
                ReturnValue = _RoseObject.GetType();
                LocalCall = true;
            }
            else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
                methodBase.Name == "GetHashCode")
            {
                outArgs = new object[0];
                ReturnValue = _RoseObject.GetHashCode();
                LocalCall = true;
            }
            else if (methodBase.DeclaringType.FullName == typeof(object).FullName &&
               methodBase.Name == "Equals")
            {
                bool IsEquals = true;
                Object obj = (msg as IMethodCallMessage).Args[0];

                if (obj == GetTransparentProxy())
                {
                    IsEquals = true;
                }
                else
                {
                    if (System.Runtime.Remoting.RemotingServices.GetRealProxy((msg as IMethodCallMessage).Args[0]) is RoseObjectProxy)
                        IsEquals = _RoseObject.Equals((System.Runtime.Remoting.RemotingServices.GetRealProxy((msg as IMethodCallMessage).Args[0]) as RoseObjectProxy)._RoseObject);
                    else
                        IsEquals = _RoseObject.Equals((msg as IMethodCallMessage).Args[0]);
                }
                outArgs = new object[0];
                ReturnValue = IsEquals;
                LocalCall = true;
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


        /// <MetaDataID>{2E65D15E-A889-4FB2-86EA-F550EA173307}</MetaDataID>
        public override IMessage Invoke(IMessage msg)
        {
            Object ret = null;

            IMethodMessage MethodMessage = msg as IMethodMessage;
            IMethodReturnMessage retMsg = null;
            Object[] outArgs = null;
            Object[] args = new object[MethodMessage.Args.Length];

            if (MethodMessage != null)
            {
                int i = 0;
                foreach (object value in MethodMessage.Args)
                {
                    try
                    {
                        args[i] = value;
                        RoseObjectProxy rp = System.Runtime.Remoting.RemotingServices.GetRealProxy(value) as RoseObjectProxy;
                        if (rp != null)
                            args[i] = rp._RoseObject;


                    }
                    catch (System.Exception error)
                    {

                    }
                    i++;


                }
                bool LocalCall;
                retMsg = TryInvokeLocal(msg, out LocalCall) as IMethodReturnMessage; ;
                if (LocalCall)
                    return retMsg;
                if (MethodMessage.MethodName == "FieldGetter")
                {
                    string fieldName = args[1] as string;
                    string declarationType = args[0] as string;
                    ret = GetValue(ModulePublisher.ClassRepository.GetType(declarationType, "").GetField(fieldName));


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
                    string fieldName = args[1] as string;
                    string declarationType = args[0] as string;
                    SetValue(ModulePublisher.ClassRepository.GetType(declarationType, "").GetField(fieldName), args[2]);

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
                        ret = GetValue(property);
                    else if (property != null && propertySet)
                        SetValue(property, args[0]);
                    else
                        ret = _RoseObject.GetType().InvokeMember(MethodMessage.MethodName, System.Reflection.BindingFlags.InvokeMethod, null, _RoseObject, args);
                }


            }
            if (ret != null && ret.GetType().IsCOMObject)
                ret = RoseObjectProxy.ControlRoseObject(ret);



            retMsg = new ReturnMessage(
                ret,           //Object ret
                outArgs,       //Object[] outArgs
                0,             //int outArgsCount
                null,          //LogicalCallContext callCtx
                (IMethodCallMessage)msg   //IMethodCallMessage mcm
                );
            return retMsg;

        }
        /// <MetaDataID>{B3AD281E-0225-4E5B-87CC-F8A4E42069A9}</MetaDataID>
        ~RoseObjectProxy()
        {
            Release();
        }

        /// <MetaDataID>{74110183-F0EA-497F-8C68-478BEB2B97F1}</MetaDataID>
        private void SetValue(System.Reflection.MemberInfo memberInfo, object value)
        {

            //if (value is MarshalByRefObject &&
            //    OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(value as MarshalByRefObject) &&
            //    System.Runtime.Remoting.RemotingServices.GetRealProxy(value) is UIProxy)
            //    value = (System.Runtime.Remoting.RemotingServices.GetRealProxy(value) as UIProxy)._RoseObject;

            if (memberInfo is System.Reflection.FieldInfo)
            {
                System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                fieldInfo.SetValue(_RoseObject, value);
            }
            else
            {
                System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                propertyInfo.SetValue(_RoseObject, value, null);
            }
        }

        /// <MetaDataID>{2D70F856-1799-4115-83EC-B72197D5EE01}</MetaDataID>
        private object GetValue(System.Reflection.MemberInfo memberInfo)
        {
            if (memberInfo is System.Reflection.FieldInfo)
            {
                System.Reflection.FieldInfo fieldInfo = memberInfo as System.Reflection.FieldInfo;
                return fieldInfo.GetValue(_RoseObject);
            }
            else
            {
                try
                {
                    System.Reflection.PropertyInfo propertyInfo = memberInfo as System.Reflection.PropertyInfo;
                    return propertyInfo.GetValue(_RoseObject, null);
                }
                catch (System.Exception error)
                {
                    return null;

                }
            }



        }



        /// <MetaDataID>{3F088DE4-079D-4D49-A5DA-1CF0B621817A}</MetaDataID>
        public void Release()
        {
            if (_RoseObject != null)
                Marshal.ReleaseComObject(this._RoseObject);
            _RoseObject = null;
        }

        public static void ReeasAll()
        {
            foreach (WeakReference weakReference in ControledObjects)
            {
                if (weakReference != null & weakReference.IsAlive)
                {
                    RoseObjectProxy proxy = weakReference.Target as RoseObjectProxy;
                    if (proxy != null)
                    {
                        try
                        {
                            proxy.Release();
                        }
                        catch (Exception error)
                        {
                        }
                    }
                }
            }
        }





        /// <MetaDataID>{46E41F4B-9F96-4EB9-B901-7C95CCDF6012}</MetaDataID>
        public bool CanCastTo(Type castType, object o)
        {
            if (castType == typeof(RoseObjectProxy))
                return true;
            bool CanCast = false;
            try
            {
                CanCast = castType.IsInstanceOfType(_RoseObject);
            }
            finally
            {
            }
            return CanCast;
        }

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
        static System.Collections.Generic.List<WeakReference> ControledObjects = new List<WeakReference>();

        /// <MetaDataID>{987F7BC9-4682-429D-B39E-13784750BBBD}</MetaDataID>
        internal static object ControlRoseObject(object roseObject)
        {
            if (roseObject == null)
                return null;
            //if (ControledObjects.ContainsKey(roseObject.GetHashCode()) && ControledObjects[roseObject.GetHashCode()].IsAlive)
            //    return (ControledObjects[roseObject.GetHashCode()].Target as RoseObjectProxy).GetTransparentProxy() as object;
            RoseObjectProxy roseObjectProxy = new RoseObjectProxy(roseObject);
            ControledObjects.Add(new WeakReference(roseObjectProxy));
            return roseObjectProxy.GetTransparentProxy();

        }
    }
}
