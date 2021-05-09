using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging ;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Security.Principal;

namespace OOAdvantech.Remoting.Sinks
{

	/// <MetaDataID>{871B3B2E-315C-4627-9BB1-CA5E3E7C940C}</MetaDataID>
	/// <summary>The server sink filter the message for objects that is type of IExtMarshalByRefObject. If it find remote object of this to travel on channel then add a proxy class between client code and real proxy for the life time control of the object.</summary>
    internal class ServerSink : BaseChannelObjectWithProperties,
        IServerChannelSink, IChannelSinkBase
    {
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DABA2D14-7678-4E0B-A776-75467B1D136E}</MetaDataID>
        private System.Runtime.Remoting.Channels.IServerChannelSink _NextChannelSink;
        /// <summary>Gets the next server channel sink in the server sink chain.  
        /// Implement IServerChannelSink member
        /// </summary>
        /// <MetaDataID>{28FB33EE-BCC5-41AB-BE28-8AF85B380D5D}</MetaDataID>
        public System.Runtime.Remoting.Channels.IServerChannelSink NextChannelSink
        {

            get { return _NextChannelSink; }
            set { _NextChannelSink = value; }
        }


        /// <MetaDataID>{15442945-A36B-4DBE-93B0-036B14E265FC}</MetaDataID>
        /// <summary>Initializes a new instance of the OOAdvantech.Remoting.Sinks.ServerSink
        /// </summary>
        /// <param name="next">The next server channel sink in the server sink chain.  
        /// </param>
        public ServerSink(IServerChannelSink next, IChannelReceiver channel)
        {
            _NextChannelSink = next;
            Channel = channel;


        }
        IChannelReceiver Channel;




        IMessage ModifyChannelData(IMessage msg)
        {
            return msg;
            IMethodReturnMessage returnMsg = msg as IMethodReturnMessage;
            if (returnMsg == null || returnMsg.Exception != null)
                return msg;
            if (Remoting.RemotingServices.ServerChannels.Count == 0)
                return msg;

            Object[] args = returnMsg.OutArgs;
            for (int i = 0; i < args.Length; i++)
                if (args[i] is MarshalByRefObject)
                {
                    // Any method parameter that is a MarshalByRefObject has channel data
                    // so we want to be sure that the desired channel schema is used when the 
                    // callback is made.

                    ObjRef objRef = System.Runtime.Remoting.RemotingServices.Marshal(Remoting.RemotingServices.GetOrgObject((MarshalByRefObject)args[i]) as MarshalByRefObject);
                    Object[] data = objRef.ChannelInfo.ChannelData;

                    int firstChannelIndex = -1;
                    ChannelDataStore tcpChannel = null;
                    ChannelDataStore ipcChannel = null;
                    for (int j = 0; j < data.Length; j++)
                    {
                        if (data[j] is ChannelDataStore)
                        {
                            if (firstChannelIndex == -1)
                                firstChannelIndex = j;
                            ChannelDataStore channelDataStore = data[j] as ChannelDataStore;
                            if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"tcp:/") == 0)
                                tcpChannel = channelDataStore;
                            if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"ipc") == 0)
                                ipcChannel = channelDataStore;
                        }
                    }
                    if (tcpChannel != null)
                        data[firstChannelIndex] = tcpChannel;

                    if (ipcChannel != null)
                        data[firstChannelIndex + 1] = ipcChannel;

                }


            Object retValue = returnMsg.ReturnValue;
            if (retValue is MarshalByRefObject)
            {
                ObjRef objRef = System.Runtime.Remoting.RemotingServices.Marshal(Remoting.RemotingServices.GetOrgObject((MarshalByRefObject)retValue) as MarshalByRefObject);
                Object[] data = objRef.ChannelInfo.ChannelData;

                int firstChannelIndex = -1;
                ChannelDataStore tcpChannel = null;
                ChannelDataStore ipcChannel = null;
                for (int j = 0; j < data.Length; j++)
                {
                    if (data[j] is ChannelDataStore)
                    {
                        if (firstChannelIndex == -1)
                            firstChannelIndex = j;
                        ChannelDataStore channelDataStore = data[j] as ChannelDataStore;
                        if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"tcp:/") == 0)
                            tcpChannel = channelDataStore;
                        if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"ipc") == 0)
                            ipcChannel = channelDataStore;
                    }
                }
                if (tcpChannel != null)
                    data[firstChannelIndex] = tcpChannel;

                if (ipcChannel != null)
                    data[firstChannelIndex + 1] = ipcChannel;

            }

            return msg;
        }



        /// <MetaDataID>{C1B2EA9E-6014-4627-92A4-B0F76F8AC980}</MetaDataID>
        /// <summary>Requests processing from the current sink of the response from a method call sent asynchronously.
        /// Implement IServerChannelSink member
        /// </summary>
        /// <param name="sinkStack">A stack of sinks leading back to the server transport sink.
        /// </param>
        /// <param name="state">Information generated on the request side that is associated with this sink.
        /// </param>
        /// <param name="msg">The response message.
        /// </param>
        /// <param name="headers">
        /// </param>The headers to add to the return message heading to the client.
        /// <param name="stream">The stream heading back to the transport sink.
        /// </param>
        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, Object state, IMessage msg, ITransportHeaders headers, Stream stream)
        {
            // restore the priority
            ThreadPriority priority = (ThreadPriority)state;
            Console.WriteLine("  -> Post-execution change back to {0}", priority);
            Thread.CurrentThread.Priority = priority;
        }

        /// <MetaDataID>{5A2B088D-17CC-4E68-AB88-BCCD3890F0CB}</MetaDataID>
        ///<summary>Returns the System.IO.Stream onto which the provided response message is to be serialized.  
        ///Implement IServerChannelSink member
        ///</summary>
        ///<param name="sinkStack">A stack of sinks leading back to the server transport sink.
        ///</param>
        ///<param name="state">The state that has been pushed to the stack by this sink.
        ///</param>
        ///<param name="msg">The response message to serialize. 
        ///</param>
        ///<param name="headers">The headers to put in the response stream to the client.
        ///</param>
        ///<returns>The System.IO.Stream onto which the provided response message is to be serialized. 
        ///</returns>
        public Stream GetResponseStream(System.Runtime.Remoting.Channels.IServerResponseChannelSinkStack sinkStack, System.Object state, System.Runtime.Remoting.Messaging.IMessage msg, System.Runtime.Remoting.Channels.ITransportHeaders headers)
        {
            return null;
        }

        /// <MetaDataID>{845DFA1E-0C8B-427C-9A79-32EC7496EFCF}</MetaDataID>
        /// <summary>Requests message processing from the current sink.  
        /// Implement IServerChannelSink member
        /// </summary>
        /// <param name="sinkStack">A stack of channel sinks that called the current sink.
        /// </param>
        /// <param name="requestMsg">The message that contains the request.
        /// </param>
        /// <param name="requestHeaders">Headers retrieved from the incoming message from the client.
        /// </param>
        /// <param name="requestStream">The stream that needs to be to processed and passed on to the deserialization sink.
        /// </param>
        /// <param name="responseMsg">When this method returns, contains an System.Runtime.Remoting.Messaging.IMessage that holds the response message. This parameter is passed uninitialized.
        /// </param>
        /// <param name="responseHeaders">When this method returns, contains an System.Runtime.Remoting.Channels.ITransportHeaders that holds the headers that are to be added to return message heading to the client. This parameter is passed uninitialized. 
        /// </param>
        /// <param name="responseStream">When this method returns, contains a System.IO.Stream that is heading back to the transport sink. This parameter is passed uninitialized. 
        /// </param>
        /// <returns>A System.Runtime.Remoting.Channels.ServerProcessing status value that provides information about how message was processed.  
        /// </returns>
        public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack,
            IMessage requestMsg,
            ITransportHeaders requestHeaders,
            Stream requestStream,
            out IMessage responseMsg,
            out ITransportHeaders responseHeaders,
            out Stream responseStream)
        {

            if (requestMsg as IMethodCallMessage != null)
            {
                MethodCallMessageWrapper MethodMessageWrapper = new MethodCallMessageWrapper((IMethodCallMessage)requestMsg);
                int Count = ((IMethodCallMessage)requestMsg).Args.Length;
                Object[] inArgs = new object[Count];

                for (int i = 0; i != Count; i++)
                {
                    //TODO Τι γίνεται αν ερθεί ένα proxy απο object που βρίσκεται στο ίδιο proccess άλλα σε άλλο context
                    object CurrArg = ((IMethodCallMessage)requestMsg).Args[i];
                    inArgs[i] = Proxy.ControlLifeTime(CurrArg);
                    //if(CurrArg!=null&&CurrArg is IExtMarshalByRefObject)
                    //{
                    //    Type type= CurrArg.GetType();
                    //    if(System.Runtime.Remoting.RemotingServices.GetRealProxy(CurrArg)!=null)
                    //        inArgs[i]=Remoting.Proxy.GetObject(CurrArg as IExtMarshalByRefObject);
                    //    else
                    //        inArgs[i]=CurrArg;
                    //}
                    //else
                    //    inArgs[i]=CurrArg;
                }

                MethodMessageWrapper.Args = inArgs;
                ServerProcessing spres;
                //WindowsIdentity oldWindowsIdentity = Remoting.RemotingServices._CallerWindowsIdentity;
                try
                {


                    // Remoting.RemotingServices._CallerWindowsIdentity = (WindowsIdentity)Thread.CurrentPrincipal.Identity;
                    spres = _NextChannelSink.ProcessMessage(sinkStack,
                                    MethodMessageWrapper, requestHeaders, requestStream,
                                    out responseMsg, out responseHeaders, out responseStream);
                }
                finally
                {
                    //Remoting.RemotingServices._CallerWindowsIdentity = oldWindowsIdentity;
                }
                responseMsg = ModifyChannelData(responseMsg);

                return spres;

            }
            else
                return _NextChannelSink.ProcessMessage(sinkStack,
                    requestMsg, requestHeaders, requestStream,
                    out responseMsg, out responseHeaders, out responseStream);
        }
    }
}
