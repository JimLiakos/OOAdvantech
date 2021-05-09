using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging ;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace OOAdvantech.Transactions.Sinks
{

	
	/// <MetaDataID>{12E17AF8-6998-457B-9E9C-1956F84A8D54}</MetaDataID>
	internal class ServerSink : BaseChannelObjectWithProperties, 
		IServerChannelSink, IChannelSinkBase
	{

        static internal bool DisableTransactionMarshalling = false;
		/// <MetaDataID>{4EF9A0B6-588E-44CD-AABC-6B9FF1D7F90D}</MetaDataID>
		private IServerChannelSink _next;

	
		/// <MetaDataID>{596A5B42-B7BD-4296-ACDB-F99EE3D6A206}</MetaDataID>
		public ServerSink (IServerChannelSink next) 
		{
			_next = next;
		}

	
		/// <MetaDataID>{D4C29109-6888-42B5-A32F-BABA7981C770}</MetaDataID>
		public void AsyncProcessResponse (IServerResponseChannelSinkStack sinkStack, Object state , IMessage msg , ITransportHeaders headers , Stream stream ) 
		{
			// restore the priority
			ThreadPriority priority = (ThreadPriority) state;
			Console.WriteLine("  -> Post-execution change back to {0}",priority);
			Thread.CurrentThread.Priority = priority;
		}

	
		/// <MetaDataID>{32D069BA-BF52-4DC9-A297-3C04AB0ECFF8}</MetaDataID>
		public Stream GetResponseStream ( System.Runtime.Remoting.Channels.IServerResponseChannelSinkStack sinkStack , System.Object state , System.Runtime.Remoting.Messaging.IMessage msg , System.Runtime.Remoting.Channels.ITransportHeaders headers ) 
		{
			return null;
		}
		
	
		/// <MetaDataID>{0090CC34-D7C0-43F9-8AC7-511412228D81}</MetaDataID>
		string ters="Liakos";
		/// <MetaDataID>{3D2D8DFD-2A8A-45E3-BDE3-79E5D2B86AFA}</MetaDataID>
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
                #region Debug information
                //string Member=(requestMsg as IMethodCallMessage).MethodName;
                //if("FieldGetter"==Member)
                //    Member="get_"+((requestMsg as IMethodCallMessage).Args[1] as string);
                //if("FieldSetter"==Member)
                //    Member="set_"+((requestMsg as IMethodCallMessage).Args[1] as string);
                //System.Diagnostics.Debug.WriteLine("Server side " + System.Net.Dns.GetHostName()+"//"+(requestMsg as IMethodCallMessage).MethodBase.DeclaringType.FullName+"."+Member);
                #endregion

  
                #region Gets transaction data
                string globalTransactionUri = null;
                int objectEnlistmentTimeOut=0;
                LogicalCallContext lcc = null;
                if (!DisableTransactionMarshalling)
                {
                    lcc = (LogicalCallContext)requestMsg.Properties["__CallContext"];
                    globalTransactionUri = lcc.GetData("{C922799B-1584-4d5f-966B-64E9032D0017}") as string;
                    object value = lcc.GetData("{D149458B-0EED-4444-8C1D-B993A67942C1}");
                    if(value !=null)
                        objectEnlistmentTimeOut = (int)value;
                    
                }
                #endregion

                if (globalTransactionUri != null)
                {
                    #region Run in transaction scoop
                    lcc.FreeNamedDataSlot("{C922799B-1584-4d5f-966B-64E9032D0017}");
                    lcc.FreeNamedDataSlot("{D149458B-0EED-4444-8C1D-B993A67942C1}");
                    Transaction  transaction=TransactionRunTime.UnMarshal(globalTransactionUri);
                    if (objectEnlistmentTimeOut == 0)
                    {
                        using (SystemStateTransition StateTransition = new SystemStateTransition(transaction))
                        {
                            ServerProcessing serverProcessing = _next.ProcessMessage(sinkStack,
                                requestMsg, requestHeaders, requestStream,
                                out responseMsg, out responseHeaders, out responseStream);
                            StateTransition.Consistent = true;
                            return serverProcessing;
                        }
                    }
                    else
                    {
                        using (SystemStateTransition StateTransition = new SystemStateTransition(transaction,TimeSpan.FromMilliseconds(objectEnlistmentTimeOut)))
                        {
                            ServerProcessing serverProcessing = _next.ProcessMessage(sinkStack,
                                requestMsg, requestHeaders, requestStream,
                                out responseMsg, out responseHeaders, out responseStream);
                            StateTransition.Consistent = true;
                            return serverProcessing;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Run out of tranaction scoop 
                    return _next.ProcessMessage(sinkStack,
                        requestMsg, requestHeaders, requestStream,
                        out responseMsg, out responseHeaders, out responseStream);
                    #endregion
                }
            }
            else
                return _next.ProcessMessage(sinkStack,
                    requestMsg, requestHeaders, requestStream,
                    out responseMsg, out responseHeaders, out responseStream);
		}
   

		/// <MetaDataID>{FE1F254F-8332-4F7F-A42E-6B30A1CDC24B}</MetaDataID>
		public IServerChannelSink NextChannelSink 
		{
			get {return _next;}
			set {_next = value;}
		}
	}
}
