using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace OOAdvantech.UserInterface.Runtime.Sinks
{




    /// <MetaDataID>{FEE02BFC-F43C-444E-B0C3-345BFD012FA9}</MetaDataID>
    internal class ClientSink : BaseChannelObjectWithProperties, IClientChannelSink, IMessageSink
    {

        static internal bool DisableTransactionMarshalling = false;
        /// <MetaDataID>{8AFCCC84-F839-4E4E-BBCA-72FD7431DCAE}</MetaDataID>
        private IMessageSink _nextMsgSink;


        /// <MetaDataID>{008FF33C-244B-4856-9129-35539067CC5B}</MetaDataID>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {

            return _nextMsgSink.AsyncProcessMessage(msg, replySink);

        }
        [ThreadStatic]
        static System.Collections.Generic.Dictionary<string, bool> InInvokeObjects = new System.Collections.Generic.Dictionary<string, bool>();

        /// <MetaDataID>{4FC30C51-AB14-43CE-A4A7-DCF897EEE47A}</MetaDataID>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            try
            {

                // only for method calls 
                if (msg as IMethodCallMessage != null)
                {
                    UISession uiSession = UISession.GetUserInterfaceSession(Transactions.Transaction.Current);
                    if (uiSession == null)
                    {
                        //try
                        //{
                        //    System.Diagnostics.Debug.WriteLine("Remote Call : " + (msg as IMethodCallMessage).MethodBase.DeclaringType.FullName + "." + (msg as IMethodCallMessage).MethodName);
                        //}
                        //catch (System.Exception error)
                        //{

                        //}
                        return _nextMsgSink.SyncProcessMessage(msg);
                    }

                    object _object = OOAdvantech.Remoting.Proxy.GetObject(ChannelUri, (msg as IMethodCallMessage).Uri);
                    if (_object == null || _object.GetType().IsCOMObject)
                    {
                        //try
                        //{
                        //    System.Diagnostics.Debug.WriteLine("Remote Call : " + (msg as IMethodCallMessage).MethodBase.DeclaringType.FullName + "." + (msg as IMethodCallMessage).MethodName);
                        //}
                        //catch (System.Exception error)
                        //{

                        //}
                        return _nextMsgSink.SyncProcessMessage(msg);
                    }

                    DisplayedValue displayedValue = null;
                    if (!uiSession.TryGetDisplayedValue(_object, out displayedValue))
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine("Remote Call : " + (msg as IMethodCallMessage).MethodBase.DeclaringType.FullName + "." + (msg as IMethodCallMessage).MethodName);
                        }
                        catch (System.Exception error)
                        {

                        }
                        return _nextMsgSink.SyncProcessMessage(msg);
                    }


                    lock (ChannelUri)
                    {
                        if (InInvokeObjects == null)
                            InInvokeObjects = new System.Collections.Generic.Dictionary<string, bool>();

                    }
                    if (InInvokeObjects.ContainsKey((msg as IMethodCallMessage).Uri))
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine("Remote Call : " + (msg as IMethodCallMessage).MethodBase.DeclaringType.FullName + "."+(msg as IMethodCallMessage).MethodName);
                        }
                        catch (System.Exception error)
                        {

                        }
                        return _nextMsgSink.SyncProcessMessage(msg);
                    }


                    try
                    {
                        InInvokeObjects.Add((msg as IMethodCallMessage).Uri, true);


                        Object ret = null;
                        IMethodMessage MethodMessage = msg as IMethodMessage;
                        IMethodReturnMessage retMsg = null;
                        Object[] outArgs = null;


                        if (MethodMessage.MethodName == "FieldGetter")
                        {
                            string fieldName = MethodMessage.Args[1] as string;
                            string declarationType = MethodMessage.Args[0] as string;
                            ret = GetValue(_object, ModulePublisher.ClassRepository.GetType(declarationType, "").GetField(fieldName));

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
                            SetValue(_object, ModulePublisher.ClassRepository.GetType(declarationType, "").GetField(fieldName), MethodMessage.Args[2]);

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
                                ret = GetValue(_object, property);
                            else if (property != null && propertySet)
                                SetValue(_object, property, MethodMessage.Args[0]);
                            else
                                return _nextMsgSink.SyncProcessMessage(msg);
                            retMsg = new ReturnMessage(
                                    ret,           //Object ret
                                    outArgs,       //Object[] outArgs
                                    0,             //int outArgsCount
                                    null,          //LogicalCallContext callCtx
                                    (IMethodCallMessage)msg   //IMethodCallMessage mcm
                                    );
                            return retMsg;

                        }
                    }
                    finally
                    {
                        InInvokeObjects.Remove((msg as IMethodCallMessage).Uri);
                    }



                    return _nextMsgSink.SyncProcessMessage(msg);

                }
                else
                    return _nextMsgSink.SyncProcessMessage(msg);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        private void SetValue(object _object, System.Reflection.MemberInfo memberInfo, object value)
        {
            Runtime.UISession.CurrentUserInterfaceSession.SetValue(_object, value, _object.GetType(), memberInfo.Name);
        }

        /// <MetaDataID>{907c35d5-0f0a-48a7-a30c-52d82ac0119e}</MetaDataID>
        private object GetValue(object _object, System.Reflection.MemberInfo memberInfo)
        {
            object retValue = UISession.CurrentUserInterfaceSession.GetDisplayedValue(_object, _object.GetType(), memberInfo.Name, null);
            if (retValue is DisplayedValue)
                retValue = (retValue as DisplayedValue).Value;
            return retValue;
        }

        string ChannelUri;
        /// <MetaDataID>{777B4750-0B1F-488F-BEBB-DD8036126CA8}</MetaDataID>
        public ClientSink(string channelUri, object next)
        {
            ChannelUri = channelUri;
            if (next as IMessageSink != null)
            {
                _nextMsgSink = (IMessageSink)next;
            }
        }



        /// <MetaDataID>{32151A6B-2B81-446B-ADC4-63DDED812183}</MetaDataID>
        public IMessageSink NextSink
        {
            get
            {
                return _nextMsgSink;
            }
        }


        /// <MetaDataID>{3F26A831-0421-49CA-838F-7CCAD4FA65D7}</MetaDataID>
        public IClientChannelSink NextChannelSink
        {
            get
            {
                throw new RemotingException("Wrong sequence.");
            }
        }


        /// <MetaDataID>{9055EDF8-A027-4782-A1AE-F05AD589B269}</MetaDataID>
        public void AsyncProcessRequest(IClientChannelSinkStack sinkStack,
            IMessage msg,
            ITransportHeaders headers,
            Stream stream)
        {
            throw new RemotingException("Wrong sequence.");
        }


        /// <MetaDataID>{700CD239-876D-42E5-886E-389D87F7C9D8}</MetaDataID>
        public void AsyncProcessResponse(
            IClientResponseChannelSinkStack sinkStack,
            object state,
            ITransportHeaders headers,
            Stream stream)
        {
            throw new RemotingException("Wrong sequence.");
        }


        /// <MetaDataID>{AABD8BC4-6198-4B64-B9E6-D933A1018871}</MetaDataID>
        public System.IO.Stream GetRequestStream(IMessage msg,
            ITransportHeaders headers)
        {
            throw new RemotingException("Wrong sequence.");
        }


        /// <MetaDataID>{877292B2-1152-403B-A1E5-5CDE99E50B96}</MetaDataID>
        public void ProcessMessage(IMessage msg,
            ITransportHeaders requestHeaders,
            Stream requestStream,
            out ITransportHeaders responseHeaders,
            out Stream responseStream)
        {
            throw new RemotingException("Wrong sequence.");
        }



    }
}
