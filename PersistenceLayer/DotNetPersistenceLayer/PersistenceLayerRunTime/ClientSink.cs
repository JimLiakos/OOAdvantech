using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace PersistenceLayerRunTime.Remoting.Sinks
{


	
	/// <MetaDataID>{17D86820-822F-4D5E-B7AC-69E7B6219F9A}</MetaDataID>
	public class ClientSink : BaseChannelObjectWithProperties, IClientChannelSink, IMessageSink
	{	
		/// <MetaDataID>{386AE2B1-2924-42D6-940D-5C76F4FD83AD}</MetaDataID>
		private IMessageSink _nextMsgSink;

		/// <MetaDataID>{267E1445-75AF-423B-B99D-F78CD2A9C509}</MetaDataID>
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink) 
		{
			// only for method calls 
			if (msg as IMethodCallMessage != null) 
			{
				replySink =new ReplySink(replySink);

				IMessageCtrl MessageCtrl =_nextMsgSink.AsyncProcessMessage(msg,replySink);
				return MessageCtrl ;
			} 
			else 
			{
				return _nextMsgSink.AsyncProcessMessage(msg,replySink);
			}
		}

		/// <MetaDataID>{D4D0FA7B-309C-494E-A419-EA406AECDDA0}</MetaDataID>
		public IMessage SyncProcessMessage(IMessage msg) 
		{
			// only for method calls 
			if (msg as IMethodCallMessage != null) 
			{
		
				MethodCallMessageWrapper MethodMessageWrapper=new MethodCallMessageWrapper((IMethodCallMessage)msg);
				int Count =((IMethodCallMessage)msg).Args.Length;
				Object[] inArgs=new object[Count ];
				for(int i=0;i!=Count ;i++) 
				{
					object CurrArg=((IMethodCallMessage)msg).Args[i];
					if(CurrArg!=null&&CurrArg is MarshalByRefObject)
					{
						Type type= CurrArg.GetType();
						object RealProxy= System.Runtime.Remoting.RemotingServices.GetRealProxy(CurrArg);
						if(RealProxy!=null&& RealProxy is Proxy)
							inArgs[i]=((Proxy)RealProxy).RealTransparentProxy;
						else
							inArgs[i]=((IMethodCallMessage)msg).Args[i];
					}
					else
						inArgs[i]=((IMethodCallMessage)msg).Args[i];
				}
				 
				MethodMessageWrapper.Args=inArgs;
				System.Runtime.Remoting.Messaging.IMethodReturnMessage ReturnMessage=(IMethodReturnMessage)_nextMsgSink.SyncProcessMessage(MethodMessageWrapper);
				Count =ReturnMessage.Args.Length;
				object[] outArgs=new object[ReturnMessage.Args.Length];
				for(int i=0;i!=Count ;i++) 
				{
					object CurrArg=ReturnMessage.Args[i];
					if(CurrArg!=null&&CurrArg is MarshalByRefObject)// error prone μάλλον θα έπρεπε μόνο για αυτά που έχουν attribute;
					{
						Type type=CurrArg.GetType();
						System.Runtime.Remoting.Proxies.RealProxy RealProxy=System.Runtime.Remoting.RemotingServices.GetRealProxy(CurrArg);
						if(RealProxy!=null)
						{
							Remoting.Proxy proxy=new Remoting.Proxy(typeof(System.MarshalByRefObject));
							proxy.AttachToObject(CurrArg as PersistenceLayer.ExtMarshalByRefObject);
							outArgs[i]=proxy.GetTransparentProxy(); 
						}
						else
							outArgs[i]=CurrArg;
					}
					else
						outArgs[i]=CurrArg;
				}

				object ReturnValue=ReturnMessage.ReturnValue;
				if(ReturnValue!=null&&ReturnValue is MarshalByRefObject&&!(ReturnValue is System.Runtime.Remoting.Lifetime.ILease))
				{
					Type type=ReturnValue.GetType();
					System.Runtime.Remoting.Proxies.RealProxy RealProxy=System.Runtime.Remoting.RemotingServices.GetRealProxy(ReturnValue);
					if(RealProxy!=null)
					{
						Remoting.Proxy proxy=new Remoting.Proxy(typeof(System.MarshalByRefObject));
						proxy.AttachToObject(ReturnValue as PersistenceLayer.ExtMarshalByRefObject);
						ReturnValue=proxy.GetTransparentProxy(); 
					}
				}
				if(ReturnMessage.Exception!=null)
					ReturnMessage=new System.Runtime.Remoting.Messaging.ReturnMessage(ReturnMessage.Exception,(IMethodCallMessage)msg); // error prone είναι σίγουρα IMethodCallMessage ή τιποτα άλλο
				else
					ReturnMessage=new System.Runtime.Remoting.Messaging.ReturnMessage(
						ReturnValue,	//ReturnValue
						outArgs,			//Object[] outArgs
						outArgs.Length,					//int outArgsCount
						ReturnMessage.LogicalCallContext,				//LogicalCallContext callCtx
						(System.Runtime.Remoting.Messaging.IMethodCallMessage)msg);
				return ReturnMessage;
			} 
			else 
			{
				return _nextMsgSink.SyncProcessMessage(msg);
			}
		}

		/// <MetaDataID>{663BA3CC-F73E-46F9-BDC2-A8D08D6D7F75}</MetaDataID>
		public ClientSink (object next) 
		{
			if (next as IMessageSink != null) 
			{
				_nextMsgSink = (IMessageSink) next;
			}
		}


		/// <MetaDataID>{8A706401-A975-45FC-9894-189CA1CE59D9}</MetaDataID>
		public IMessageSink NextSink 
		{
			get 
			{
				return _nextMsgSink;
			}
		}

		/// <MetaDataID>{70B776D4-E366-41CA-87A6-E56A206873F0}</MetaDataID>
		public IClientChannelSink NextChannelSink 
		{ 
			get 
			{ 
				throw new RemotingException("Wrong sequence.");
			} 
		}

		// Methods
		/// <MetaDataID>{99C0DD3E-3EF0-48C9-A0C0-41256A5F44C3}</MetaDataID>
		public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, 
			IMessage msg, 
			ITransportHeaders headers, 
			Stream stream) 
		{
			throw new RemotingException("Wrong sequence.");
		}

		/// <MetaDataID>{9F21C274-2F43-4E1B-ACFE-75C82E8FC869}</MetaDataID>
		public void AsyncProcessResponse(
			IClientResponseChannelSinkStack sinkStack, 
			object state, 
			ITransportHeaders headers, 
			Stream stream)
		{
			throw new RemotingException("Wrong sequence.");
		}

		/// <MetaDataID>{3447D703-26A4-460F-9AE4-58FAE496063B}</MetaDataID>
		public System.IO.Stream GetRequestStream(IMessage msg, 
			ITransportHeaders headers)
		{
			throw new RemotingException("Wrong sequence.");
		}

		/// <MetaDataID>{E371B31B-D06C-4353-ADC5-9569E66EBE82}</MetaDataID>
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
