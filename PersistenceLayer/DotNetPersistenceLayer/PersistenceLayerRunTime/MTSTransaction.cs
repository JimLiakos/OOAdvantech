namespace OOAdvantech.PersistenceLayerRunTime
{
	using CommandOrder = System.Int32;
    /// <MetaDataID>{A7A4CEDD-FDF8-4E34-9C54-2F20C55FB69F}</MetaDataID>
	//[System.EnterpriseServices.Transaction(System.EnterpriseServices.TransactionOption.Required)]
	public class COMPlusTransaction1 //: System.EnterpriseServices.ServicedComponent
	{
		/// <MetaDataID>{0D19C5F9-5C69-4E19-9E2A-5B4AE6551C0B}</MetaDataID>
		public COMPlusTransaction1()
		{
			//
			// TODO: Add constructor logic here
			//
		}


	
		/// <MetaDataID>{EA023A54-0B8E-4134-94A9-A28CE234E676}</MetaDataID>
		//[System.EnterpriseServices.AutoComplete]
		public void PrepareUnderNatinveTransaction(PersistenceLayerRunTime.TransactionContext Transaction )
		{   
			Transaction.Prepare();
		}
		/// <MetaDataID>{7B7B9B25-7B08-4509-95C5-FA549E6E68B6}</MetaDataID>
		public CommandOrder ProcessCommandsUnderNatinveTransaction(PersistenceLayerRunTime.TransactionContext transactionContext, CommandOrder order)
		{
			return transactionContext.ProcessCommands(order);
		}
		/// <MetaDataID>{E4643394-563F-4874-9FAD-919A7ADB9487}</MetaDataID>
		~COMPlusTransaction1()
		{ 
			int lo =0;
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
