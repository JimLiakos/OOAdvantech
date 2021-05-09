namespace OOAdvantech.Transactions
{
	/// <MetaDataID>{A7A4CEDD-FDF8-4E34-9C54-2F20C55FB69F}</MetaDataID>
	
	internal class NativeTransactionDistributer 
	{
		
		private NativeTransactionDistributer()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static void Distribute(object nativeTransaction, ResourceManager resourceManager,string transactionUri)
		{
			if(nativeTransaction==null)
				throw new System.Exception("There isn't native tansaction");
			//System.EnterpriseServices.ITransaction Tranaction=System.EnterpriseServices.ContextUtil.Transaction as System.EnterpriseServices.ITransaction;
			if(resourceManager is EnlistmentsController||resourceManager is TransactionContext)
			{
				EnlistmentsController enlistmentsController=resourceManager as EnlistmentsController;
				TransactionContext transactionContext=resourceManager as TransactionContext;
				OOAdvantech.Transactions.Bridge.NativeTPMonitor.PropagateTransaction(nativeTransaction,transactionUri,resourceManager as System.MarshalByRefObject);
				if(enlistmentsController!=null)
					enlistmentsController.AttachToNativeTransaction();
				if(transactionContext!=null)
					transactionContext.AttachToNativeTransaction();
			}
		}
		
		public static void Distribute(object nativeTransaction,EnlistmentsController transactionEnlistments)
		{
			//EnlistmentsController mtransactionEnlistments=
			if(nativeTransaction==null)
				throw new System.Exception("There isn't native tansaction");
			//System.EnterpriseServices.ITransaction Tranaction=System.EnterpriseServices.ContextUtil.Transaction as System.EnterpriseServices.ITransaction;
			(transactionEnlistments as EnlistmentsController).NativeTransaction=nativeTransaction ;
			string transactionUri=transactionEnlistments.TransactionUri;
			
			foreach(System.Collections.DictionaryEntry currDictionaryEntry in transactionEnlistments .Enlistments)
			{
				ResourceManager resourceManager=currDictionaryEntry.Key as ResourceManager;
				if(resourceManager is EnlistmentsController||resourceManager is TransactionContext)
				{
					EnlistmentsController enlistmentsController=resourceManager as EnlistmentsController;
					TransactionContext transactionContext=resourceManager as TransactionContext;
					OOAdvantech.Transactions.Bridge.NativeTPMonitor.PropagateTransaction(nativeTransaction,transactionUri,resourceManager as System.MarshalByRefObject);
					if(enlistmentsController!=null)
						enlistmentsController.AttachToNativeTransaction();
					if(transactionContext!=null)
						transactionContext.AttachToNativeTransaction();
				}
			}
			

		}
		~NativeTransactionDistributer()
		{ 
			int lo =0;
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
