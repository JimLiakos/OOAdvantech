using System;

namespace OOAdvantech.MSSQLPersistenceRunTime
{
	/// <MetaDataID>{572A840B-94EE-4A70-9B2D-DAA7BBFE103C}</MetaDataID>
	[System.EnterpriseServices.Transaction(System.EnterpriseServices.TransactionOption.Required)]
	public class COMPlusTransactiona : System.EnterpriseServices.ServicedComponent
	{
	
		/// <MetaDataID>{EA023A54-0B8E-4134-94A9-A28CE234E676}</MetaDataID>
		[System.EnterpriseServices.AutoComplete]
		public void UpdateDataBaseMetadata(Storage aObjectStorage)
		{ 
			try
			{
				aObjectStorage.UpdateDataBaseMetadata();
			}
			catch(System.Exception Error)
			{
				int lo=0;
				throw Error;

			}
			
		}
	}
		
}
