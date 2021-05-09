using System;

namespace OOAdvantech.Transactions
{
	public enum XACTHEURISTIC 
			{
				XACTHEURISTIC_ABORT =0,
				XACTHEURISTIC_COMMIT,
				XACTHEURISTIC_DAMAGE,
				XACTHEURISTIC_DANGER,
			}

	[System.Runtime.InteropServices.GuidAttribute("799D5869-E7DE-41b4-8040-102417983710")]
	public interface ITransactionOutcomeEvents
	{
		void Aborted (string Reason);
		void Committed ();
		void HeuristicDecision ( XACTHEURISTIC dwDecision , string Reason);
		void Indoubt (  );

	}
	internal class NativeTransactionOutcomeEvents:ITransactionOutcomeEvents
	{

		public event NativeTransactionEventRaise NativeTransactionEvent;
        #if !PocketPC
		System.EnterpriseServices.ITransaction MTSTransaction;
        #endif
		public NativeTransactionOutcomeEvents(System.EnterpriseServices.ITransaction mtsTransaction)
		{
			MTSTransaction=mtsTransaction;
			if(mtsTransaction==null)
				throw new System.ArgumentException("You try to attach create events consumer to null transaction. The parameter ‘mtsTransaction’ must not be null.","mtsTransaction");
			TransactionManagmentSystem.TransactionManager.SetEventsConsumer(mtsTransaction,this);

		}
		public void Aborted (string Reason)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("************************************************************");
				System.Diagnostics.Debug.WriteLine("************** COM+ Transaction Aborted *******************");
				System.Diagnostics.Debug.WriteLine("************************************************************");
				if(NativeTransactionEvent!=null)
					NativeTransactionEvent(this,NativeTransactionState.Abort);
				NativeTransactionEvent=null;
				MTSTransaction=null;
			}
			catch(System.Exception  Error)//All exception must be caught. In other case the process will be terminated.
			{
			}
		}
		public void Committed ()
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("************************************************************");
				System.Diagnostics.Debug.WriteLine("************** COM+ Transaction Committed *******************");
				System.Diagnostics.Debug.WriteLine("************************************************************");
				if(NativeTransactionEvent!=null)
					NativeTransactionEvent(this,NativeTransactionState.Commit);
				NativeTransactionEvent=null;
				MTSTransaction=null;
			}
			catch(System.Exception  Error)//All exception must be caught. In other case the process will be terminated.
			{
				int hh=0;
			}


		}
		public void HeuristicDecision ( XACTHEURISTIC dwDecision , string Reason)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("************************************************************");
				System.Diagnostics.Debug.WriteLine("************** COM+ Transaction HeuristicDecision *******************");
				System.Diagnostics.Debug.WriteLine("************************************************************");
				NativeTransactionState nativeTransactionState;
				switch(dwDecision)
				{
					case XACTHEURISTIC.XACTHEURISTIC_COMMIT:
					{
						nativeTransactionState=NativeTransactionState.HeuristicDecisionCommit;
						break;
					}
					case XACTHEURISTIC.XACTHEURISTIC_ABORT:
					{
						nativeTransactionState=NativeTransactionState.HeuristicDecisionAbort;
						break;
					}
					case XACTHEURISTIC.XACTHEURISTIC_DAMAGE:
					{
						nativeTransactionState=NativeTransactionState.HeuristicDecisionDamage;
						break;
					}
					case XACTHEURISTIC.XACTHEURISTIC_DANGER:
					{
						nativeTransactionState=NativeTransactionState.HeuristicDecisionDanger;
						break;
					}
					default :
					{
						nativeTransactionState=NativeTransactionState.HeuristicDecisionAbort;
						break;
					}
				}
				if(NativeTransactionEvent!=null)
					NativeTransactionEvent(this,nativeTransactionState);
				NativeTransactionEvent=null;
				MTSTransaction=null;
			}
			catch(System.Exception  Error)//All exception must be caught. In other case the process will be terminated.
			{
			}
		}
		public void Indoubt (  )
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("************************************************************");
				System.Diagnostics.Debug.WriteLine("************** COM+ Transaction Indoubt *******************");
				System.Diagnostics.Debug.WriteLine("************************************************************");
				if(NativeTransactionEvent!=null)
					NativeTransactionEvent(this,NativeTransactionState.Indoubt);
				NativeTransactionEvent=null;
				MTSTransaction=null;
			}
			catch(System.Exception  Error)//All exception must be caught. In other case the process will be terminated.
			{
			}
		}
	}
}
