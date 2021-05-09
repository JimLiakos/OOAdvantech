using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace OOAdvantech.Remoting.Sinks
{

    /// <MetaDataID>{5ca03515-a93c-406c-8bd9-8c3285d0025e}</MetaDataID>
	public enum SinkType
	{
		MessageSink,
		StreamSink
	}
    /// <MetaDataID>{89d3ce84-293a-4034-885d-d7800c4b305c}</MetaDataID>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class SupportSinkAttribute:Attribute
	{
		System.Type ClientSinkProviderType;
		System.Type ServerSinkProviderType;
		public SupportSinkAttribute(System.Type clientSinkProviderType, SinkType clientSinkType ,System.Type serverSinkProviderType,SinkType serverSinkType)
		{
			ClientSinkProviderType=clientSinkProviderType;
			ServerSinkProviderType=serverSinkProviderType;
		}
		public IClientChannelSinkProvider CreateClientSinkProvider()
		{
			if(ClientSinkProviderType==null)
				return null;
			return System.Activator.CreateInstance(ClientSinkProviderType) as IClientChannelSinkProvider;
		}
		public IServerChannelSinkProvider CreateServerSinkProvider()
		{
			if(ServerSinkProviderType==null)
				return null;
			return System.Activator.CreateInstance(ServerSinkProviderType) as IServerChannelSinkProvider;
		}
	}


	
	/// <MetaDataID>{17D86820-822F-4D5E-B7AC-69E7B6219F9A}</MetaDataID>
	/// <summary>The client sink filter the message for object that is type of IExtMarshalByRefObject. If it find remote object of this to travel on channel then add a proxy class between client code and real proxy for the life time control of the object.</summary>
	internal class ClientSink : BaseChannelObjectWithProperties, IClientChannelSink, IMessageSink
	{
     

		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{738ADB88-F086-4E4C-931F-EC852AE56BB1}</MetaDataID>
		private IMessageSink _NextSink;
		/// <MetaDataID>{36EEF35C-5FAB-4D1A-B10D-3237B0CEBEFA}</MetaDataID>
		/// <summary>Gets the next message sink in the sink chain.
		/// Implement IMessageSink member.</summary>
		public IMessageSink NextSink 
		{
			get 
			{
				return _NextSink;
			}
		}

		/// <summary>Asynchronously processes the given message.  
		/// Implement IMessageSink member.
		/// </summary>
		/// <returns>
		/// Returns an System.Runtime.Remoting.Messaging.IMessageCtrl interface that provides a way to control asynchronous messages after they have been dispatched.
		/// </returns>
		/// <param name="msg">The message to process.</param>
		/// <param name="replySink">The reply sink for the reply message.</param>
		/// <MetaDataID>{267E1445-75AF-423B-B99D-F78CD2A9C509}</MetaDataID>
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink) 
		{
			// only for method calls 
			if (msg as IMethodCallMessage != null) 
			{
				replySink =new ReplySink(replySink);
				IMessageCtrl MessageCtrl =_NextSink.AsyncProcessMessage(msg,replySink);
				return MessageCtrl ;
			} 
			else 
			{
				return _NextSink.AsyncProcessMessage(msg,replySink);
			}
		}

    
		/// <summary>Synchronously processes the given message.  
		/// Implement IMessageSink member.
		/// </summary>
		/// <returns>
		/// A reply message in response to the request.
		/// </returns>
		/// <param name="msg">Synchronously processes the given message.</param>
		/// <MetaDataID>{D4D0FA7B-309C-494E-A419-EA406AECDDA0}</MetaDataID>
		public IMessage SyncProcessMessage(IMessage msg) 
		{
			// only for method calls 
            if (msg as IMethodCallMessage != null)
            {
                System.Runtime.Remoting.Messaging.IMethodReturnMessage returnMessage =null;
                LogicalCallContext lcc = (LogicalCallContext)msg.Properties["__CallContext"];
                object localMachineCallData = lcc.GetData("{70C34BDE-7E97-41ff-B439-3FF8D4659AAF}");
                bool localMachineCall = false;
                if (localMachineCallData != null && (bool)localMachineCallData == true)
                    localMachineCall = true;
                try
                {

                    
                    msg = ModifyChannelData(msg);

                     returnMessage=(IMethodReturnMessage)_NextSink.SyncProcessMessage(msg);
                }
                finally
                {
                    lcc = (LogicalCallContext)returnMessage.Properties["__CallContext"];
                    lcc.FreeNamedDataSlot("{70C34BDE-7E97-41ff-B439-3FF8D4659AAF}");
                }


                if (returnMessage.Exception != null)
                    return returnMessage;

                int Count = returnMessage.Args.Length;
                object[] outArgs = new object[returnMessage.Args.Length];


                #region Wrap with life time control proxies, output arguments which they are proxies for remote object
                
                for (int i = 0; i != Count; i++)
                {
                    //TODO prone μάλλον θα έπρεπε μόνο για αυτά που έχουν attribute;
                    object arg = returnMessage.Args[i];
                    
                    outArgs[i] =Proxy.ControlLifeTime(arg);
                    //if (arg != null && arg is IExtMarshalByRefObject)
                    //{
                    //    System.Runtime.Remoting.Proxies.RealProxy realProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(arg);
                    //    if (realProxy != null)
                    //        outArgs[i] = Remoting.Proxy.GetObject(arg as IExtMarshalByRefObject);
                    //    else
                    //        outArgs[i] = arg;
                    //}
                    //else
                    //    outArgs[i] = arg;
                }
                #endregion

                #region Wrap with life time control proxy the return value, when it is proxy for remote object
                object returnValue = returnMessage.ReturnValue;
                System.Reflection.MemberInfo ee;
                if (!(returnMessage.MethodBase is System.Reflection.MethodInfo) || (returnMessage.MethodBase as System.Reflection.MethodInfo).ReturnType != typeof(void))
                {
                    if (!(returnMessage.MethodBase.DeclaringType == typeof(OOAdvantech.Remoting.RemotingServices) &&
                        (returnMessage.MethodName == "ReconnectWithObject"||returnMessage.MethodName == "ReconnectWithMonoStateClass")))
                        returnValue = Proxy.ControlLifeTime(returnValue,msg as IMethodCallMessage);
                    else
                    {

                    }
                }
                else
                {

                }
                //if (returnValue != null && returnValue is IExtMarshalByRefObject)
                //{
                //    System.Runtime.Remoting.Proxies.RealProxy realProxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(returnValue);
                //    if (realProxy != null)
                //        returnValue = Remoting.Proxy.GetObject(returnValue as IExtMarshalByRefObject);
                //}
                #endregion

                returnMessage = new System.Runtime.Remoting.Messaging.ReturnMessage(
                    returnValue,	//ReturnValue
                    outArgs,			//Object[] outArgs
                    outArgs.Length,					//int outArgsCount
                    returnMessage.LogicalCallContext,				//LogicalCallContext callCtx
                    (System.Runtime.Remoting.Messaging.IMethodCallMessage)msg);
                return returnMessage;
            }
            else
            {
                return _NextSink.SyncProcessMessage(msg);
            }
		}
  

        IMessage ModifyChannelData(IMessage msg )
        {
            IMethodCallMessage callMsg = msg as IMethodCallMessage;
            if (callMsg == null)
                return msg;


            try
            {
                Object[] args = callMsg.InArgs;
                for (int i = 0; i < args.Length; i++)
                    if (args[i] is MarshalByRefObject)
                    {

                        // Any method parameter that is a MarshalByRefObject is a callback facility
                        // so we want to be sure that the desired channel schema is used when the 
                        // callback is made.

                        ObjRef objRef = System.Runtime.Remoting.RemotingServices.Marshal(Remoting.RemotingServices.GetOrgObject((MarshalByRefObject)args[i]) as MarshalByRefObject);
                        Object[] data = objRef.ChannelInfo.ChannelData;
                        int firstChannelIndex = -1;
                        ChannelDataStore tcpChannel = null;
                        ChannelDataStore ipcChannel = null;
                        int tcpChannelIndex = -1;
                        int ipcChannelIndex = -1;

                        for (int j = 0; j < data.Length; j++)
                        {
                            if (data[j] is ChannelDataStore)
                            {
                                if (firstChannelIndex == -1)
                                    firstChannelIndex = j;
                                ChannelDataStore channelDataStore = data[j] as ChannelDataStore;

                                if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"tcp:/") == 0)
                                {
                                    tcpChannelIndex = j;
                                    tcpChannel = channelDataStore;
                                }
                                if (channelDataStore.ChannelUris[0].ToLower().IndexOf(@"ipc") == 0)
                                {
                                    ipcChannelIndex = j;
                                    ipcChannel = channelDataStore;
                                }
                            }
                        }
                        if (ipcChannelIndex != -1 && tcpChannelIndex != -1 && tcpChannel != null && ipcChannel != null && ipcChannelIndex < tcpChannelIndex)
                        {
                            data[ipcChannelIndex] = tcpChannel;
                            data[tcpChannelIndex] = ipcChannel;
                        }

                        //TODO τι γίνεται όταν υπάρχουν και άλλα channels όπως http:
                    }
            }
            catch (System.Exception error)
            {
                throw;
            }



            return msg;
        }

        string ChannelUri;
		/// <summary>Initializes a new instance of the OOAdvantech.Remoting.Sinks.ClientSink</summary>
		/// <param name="next">next message sink in the sink chain.</param>
		/// <MetaDataID>{663BA3CC-F73E-46F9-BDC2-A8D08D6D7F75}</MetaDataID>
        public ClientSink(string channelUri, IMessageSink messageSink) 
		{
            ChannelUri = channelUri;
            
            _NextSink = messageSink;
		}


		/// <MetaDataID>{70B776D4-E366-41CA-87A6-E56A206873F0}</MetaDataID>
		/// <summary>Gets the next client channel sink in the client sink chain.  
		/// Implement IClientChannelSink member.</summary>
		public IClientChannelSink NextChannelSink 
		{ 
			get 
			{ 
				throw new System.Runtime.Remoting.RemotingException("Wrong sequence.");
			} 
		}
		/// <summary>Requests asynchronous processing of a method call on the current sink.
		/// Implement IClientChannelSink member.</summary>
		/// <param name="sinkStack">A stack of channel sinks that called this sink.</param>
		/// <param name="msg">The message to process.</param>
		/// <param name="headers">The headers to add to the outgoing message heading to the server.</param>
		/// <param name="stream">The stream headed to the transport sink.</param>
		/// <MetaDataID>{99C0DD3E-3EF0-48C9-A0C0-41256A5F44C3}</MetaDataID>
		public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, 
			IMessage msg, 
			ITransportHeaders headers, 
			Stream stream) 
		{
			throw new System.Runtime.Remoting.RemotingException("Wrong sequence.");
		}
		/// <summary>Requests asynchronous processing of a response to a method call on the current sink.  
		/// Implement IClientChannelSink member.</summary>
		/// <param name="sinkStack">A stack of sinks that called this sink.</param>
		/// <param name="state">Information generated on the request side that is associated with this sink.</param>
		/// <param name="headers">The headers retrieved from the server response stream.</param>
		/// <param name="stream">The stream coming back from the transport sink.</param>
		/// <MetaDataID>{9F21C274-2F43-4E1B-ACFE-75C82E8FC869}</MetaDataID>
		public void AsyncProcessResponse(
			IClientResponseChannelSinkStack sinkStack, 
			object state, 
			ITransportHeaders headers, 
			Stream stream)
		{
			throw new System.Runtime.Remoting.RemotingException("Wrong sequence.");
		}

		/// <summary>Returns the System.IO.Stream onto which the provided message is to be serialized.  
		/// Implement IClientChannelSink member.
		/// </summary>
		/// <returns>
		/// The System.IO.Stream onto which the provided message is to be serialized.
		/// </returns>
		/// <param name="msg">The System.Runtime.Remoting.Messaging.IMethodCallMessage containing details about the method call.</param>
		/// <param name="headers">The headers to add to the outgoing message heading to the server.</param>
		/// <MetaDataID>{3447D703-26A4-460F-9AE4-58FAE496063B}</MetaDataID>
		public System.IO.Stream GetRequestStream(IMessage msg, 
			ITransportHeaders headers)
		{
			throw new System.Runtime.Remoting.RemotingException("Wrong sequence.");
		}

		/// <summary>Requests message processing from the current sink.  
		/// Implement IClientChannelSink member.</summary>
		/// <param name="msg">The message to process.</param>
		/// <param name="requestHeaders">The headers to add to the outgoing message heading to the server.</param>
		/// <param name="requestStream">The stream headed to the transport sink.</param>
		/// <param name="responseHeaders">When this method returns, contains an System.Runtime.Remoting.Channels.ITransportHeaders interface that holds the headers that the server returned. This parameter is passed uninitialized.</param>
		/// <param name="responseStream">When this method returns, contains a System.IO.Stream coming back from the transport sink. This parameter is passed uninitialized.</param>
		/// <MetaDataID>{E371B31B-D06C-4353-ADC5-9569E66EBE82}</MetaDataID>
		public void ProcessMessage(IMessage msg, 
			ITransportHeaders requestHeaders, 
			Stream requestStream, 
			out ITransportHeaders responseHeaders, 
			out Stream responseStream)
		{
			throw new System.Runtime.Remoting.RemotingException("Wrong sequence.");
		}
	}
}
