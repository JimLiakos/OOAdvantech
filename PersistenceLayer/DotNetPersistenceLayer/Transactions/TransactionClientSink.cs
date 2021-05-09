using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace OOAdvantech.Transactions.Sinks
{


	
	
	/// <MetaDataID>{FEE02BFC-F43C-444E-B0C3-345BFD012FA9}</MetaDataID>
	internal class ClientSink : BaseChannelObjectWithProperties, IClientChannelSink, IMessageSink
	{	
	
        static internal bool DisableTransactionMarshalling=false;
		/// <MetaDataID>{8AFCCC84-F839-4E4E-BBCA-72FD7431DCAE}</MetaDataID>
		private IMessageSink _nextMsgSink;

	
		/// <MetaDataID>{008FF33C-244B-4856-9129-35539067CC5B}</MetaDataID>
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink) 
		{
			
				return _nextMsgSink.AsyncProcessMessage(msg,replySink);
			
		}

	
		/// <MetaDataID>{4FC30C51-AB14-43CE-A4A7-DCF897EEE47A}</MetaDataID>
		public IMessage SyncProcessMessage(IMessage msg) 
		{
			// only for method calls 
			if (msg as IMethodCallMessage != null)
            {
                #region Debug informations
                //string Member=(msg as IMethodCallMessage).MethodName;
                //if("FieldGetter"==Member)
                //    Member="get_"+((msg as IMethodCallMessage).Args[1] as string);
                //if("FieldSetter"==Member)
                //    Member="set_"+((msg as IMethodCallMessage).Args[1] as string);
                //System.Diagnostics.Debug.WriteLine("Client side " + System.Net.Dns.GetHostName()+"//"+(msg as IMethodCallMessage).MethodBase.DeclaringType.FullName+"."+Member);
                #endregion

                //if you call TransactionCoordinator do not propagate transaction
				if((msg as IMethodCallMessage).MethodBase.DeclaringType.FullName!=typeof(TransactionCoordinator).FullName) 
				{

                    if (Transaction.Current == null || DisableTransactionMarshalling || (Transaction.Current as TransactionRunTime).InMarshal)
                    {
                        ((LogicalCallContext)msg.Properties["__CallContext"]).FreeNamedDataSlot("{C922799B-1584-4d5f-966B-64E9032D0017}");
                        ((LogicalCallContext)msg.Properties["__CallContext"]).FreeNamedDataSlot("{D149458B-0EED-4444-8C1D-B993A67942C1}");
                        
                        return _nextMsgSink.SyncProcessMessage(msg);
                    }
                    else
                    {


                        //TODO καλήτερα να φορτωθεί το data slot με GUID και να προβλευθεί
                        //κάτι για την απίθανη περίπτωση που υπάρχει slot αυτό το GUID στο logical context.

                        #region Load transaction data in logical call context and propagate message
                        
                        LogicalCallContext lcc = (LogicalCallContext)msg.Properties["__CallContext"];
                        //  transactionData = lcc.GetData("Transaction") as OOAdvantech.Transactions.TransactionStream;
                        try
                        {
                            lcc.SetData("{C922799B-1584-4d5f-966B-64E9032D0017}", (Transaction.Current as TransactionRunTime).Marshal());
                            lcc.SetData("{D149458B-0EED-4444-8C1D-B993A67942C1}", (int)LogicalThread.ObjectEnlistmentTimeOut.TotalMilliseconds);
                            
                        }
                        catch (System.Exception error)
                        {
                            return new System.Runtime.Remoting.Messaging.ReturnMessage(new System.Exception("Transaction system can’t marshal current transaction. Check if the OOAdvantech transaction coordinator service runs."), (msg as IMethodCallMessage));
                        }
                        IMessage ret_msg = _nextMsgSink.SyncProcessMessage(msg);
                        lcc=(LogicalCallContext)ret_msg.Properties["__CallContext"];
                        lcc.FreeNamedDataSlot("{C922799B-1584-4d5f-966B-64E9032D0017}");
                        lcc.FreeNamedDataSlot("{D149458B-0EED-4444-8C1D-B993A67942C1}");

                        

                        return ret_msg;
                        
                        #endregion
                    }

                }
                else
                    return _nextMsgSink.SyncProcessMessage(msg);
				
			} 
			else 
				return _nextMsgSink.SyncProcessMessage(msg);
		}

	
		/// <MetaDataID>{777B4750-0B1F-488F-BEBB-DD8036126CA8}</MetaDataID>
		public ClientSink (object next) 
		{
			if (next as IMessageSink != null) 
			{
				_nextMsgSink = (IMessageSink) next;
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
