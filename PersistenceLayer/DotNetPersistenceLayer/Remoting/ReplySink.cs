using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;


namespace OOAdvantech.Remoting.Sinks
{
	/// <summary>The ReplySink sink filter the message for object that is type of IExtMarshalByRefObject in asynchronous call.  If it find remote object of this to travel on channel then add a proxy class between client code and real proxy for the life time control of the object.</summary>
	/// <MetaDataID>{79B41127-0578-46AA-A067-D68FA2A33DA9}</MetaDataID>
	internal class ReplySink:IMessageSink
	{
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{8EDF2923-F1A7-4317-B299-D367CB929779}</MetaDataID>
		private IMessageSink _NextSink;
		/// <MetaDataID>{FE6FA122-DA1F-49E5-942F-06135BA0FBA0}</MetaDataID>
		/// <summary>Gets the next message sink in the sink chain.
		/// Implement IMessageSink member.</summary>
		public IMessageSink NextSink
		{
			get
			{
				return _NextSink;
			}
		}
		/// <MetaDataID>{4DCA89C3-7577-4E9B-B407-A71B488A9E5C}</MetaDataID>
		public ReplySink(IMessageSink next)
		{
			_NextSink=next;
		}

		/// <MetaDataID>{3D7A146A-3DB3-4600-A1C6-5CCE029A28DA}</MetaDataID>
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
		//	return _NextSink.SyncProcessMessage(msg);

			if(msg is IMethodReturnMessage)
			{
				
				IMethodReturnMessage OrgMsg= (IMethodReturnMessage)msg;
				if(OrgMsg.Exception!=null)
                    if(_NextSink==null)
						throw OrgMsg.Exception;
				int Count =OrgMsg.Args.Length;
				Object[] Args=new object[Count ];
				for(int i=0;i!=Count ;i++) 
				{
					object CurrArg=OrgMsg.Args[i];
					if(CurrArg!=null&&CurrArg is MarshalByRefObject)
					{ 
						Type type= CurrArg.GetType();
						if(System.Runtime.Remoting.RemotingServices.GetRealProxy(CurrArg)!=null)
						{
							Args[i]=Remoting.Proxy.GetObject(CurrArg as IExtMarshalByRefObject);
						}
						else
							Args[i]=CurrArg;
					}
					else
						Args[i]=CurrArg;
				}
				object ReturnValue=OrgMsg.ReturnValue;
				if(ReturnValue!=null&&ReturnValue is IExtMarshalByRefObject)
				{
					Type type= ReturnValue.GetType();
					if(System.Runtime.Remoting.RemotingServices.GetRealProxy(ReturnValue)!=null)
					{
						ReturnValue=Remoting.Proxy.GetObject(ReturnValue as IExtMarshalByRefObject);
					}
				}
				ReturnMessage ReturnMessage=new ReturnMessage(ReturnValue,Args,Args.Length,null,null);
				return _NextSink.SyncProcessMessage(ReturnMessage);
			}

			return _NextSink.SyncProcessMessage(msg);
		}

		/// <MetaDataID>{D440AFBE-09F7-4436-9F52-0EEB4A06A82E}</MetaDataID>
		/// <summary>Asynchronously processes the given message.
		/// Implement IMessageSink member.
		/// Has no sense in the ReplySink.
		/// </summary>
		/// <param name="msg">The message to process. 
		/// </param>
		/// <param name="replySink">The reply sink for the reply message. 
		/// </param>
		/// <returns>Returns an System.Runtime.Remoting.Messaging.IMessageCtrl interface that provides a way to control asynchronous messages after they have been dispatched.
		/// </returns>
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink) 
		{
			return _NextSink.AsyncProcessMessage(msg,replySink);
		
		}


	}
}
