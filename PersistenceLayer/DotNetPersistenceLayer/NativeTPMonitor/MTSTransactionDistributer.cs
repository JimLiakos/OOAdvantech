using System;

namespace OOAdvantech.Transactions.Bridge
{
//	[System.Runtime.InteropServices.GuidAttribute("D0E288A0-6410-4a65-A472-1EDEA95E0AFE")]
//	public interface IMTSTransactionPropagator
//	{
//		void PropagateTransaction ( string transactionUri , string channelUri);
//		void CatchTransaction( string transactionUri, string ChannelURI, byte[] transactionStream);
//	}

	
	/// <summary>
	/// </summary>
	/// <MetaDataID>{163893B9-8186-4961-9BD0-665E93F56157}</MetaDataID>
	[System.EnterpriseServices.Transaction(System.EnterpriseServices.TransactionOption.Required)]
	[System.Runtime.InteropServices.GuidAttribute("163893B9-8186-4961-9BD0-665E93F56157")]
	public class MTSTransactionPropagator:System.EnterpriseServices.ServicedComponent //,IMTSTransactionPropagator
	{
		public MTSTransactionPropagator()
		{
			int hh=0;
		}

/*		object[] argValues = new object [] {"Mouse", "Micky"};
		String [] argNames = new String [] {"lastName", "firstName"};
		t.InvokeMember ("PrintName", BindingFlags.InvokeMethod, null, null, argValues, null, null, argNames);
*/
		public System.Exception RunUnderTransaction(object target,string methodName,string[]argNames ,object[] argValues,ref object result )
		{
			try
			{
				result=target.GetType().InvokeMember(methodName,System.Reflection.BindingFlags.InvokeMethod,null,target,argValues,null,null,argNames);
			}
			catch(System.Exception Error)
			{
				return Error;
			}
			return null;
		}

//		/// <MetaDataID>{D1820B0F-1D52-4246-9BC3-95BBE40A1984}</MetaDataID>
//		public void PropagateTransactiona( string transactionUri , string channelUri )
//		{
//			string ComputerName=null;
//			try
//			{
//				
//				channelUri=channelUri.ToLower();
//				channelUri=channelUri.Trim();
//				int ComputerNameStartPos= channelUri.IndexOf("tcp://");
//				if(ComputerNameStartPos==-1)
//					throw new System.Exception("It can't be find computer name");
//				ComputerNameStartPos+="tcp://".Length;
//				int ComputerNameEndPos=channelUri.IndexOf(":",ComputerNameStartPos);
//				if(ComputerNameEndPos==-1)
//					throw new System.Exception("It can't be find computer identity");
//				ComputerName=channelUri.Substring(ComputerNameStartPos,ComputerNameEndPos-ComputerNameStartPos);
//
//				//object tt= System.EnterpriseServices.ContextUtil.Transaction;//
//				bool contition=false;
//				if(contition)
//				{
//					System.Data.OleDb.OleDbConnection Connection=new System.Data.OleDb.OleDbConnection(
//						"Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=master;Data Source="+ComputerName);
//					Connection.Open();
//					Connection.Close();
//				}
//				//System.Data.OleDb.OleDbCommand Command=Connection.CreateCommand();
//				//Command.CommandText="Select * From Categories";
//				//Command.ExecuteReader();
//
//				
//
//
//
//				TransactionManagmentSystem.TransactionManager.PropagateTransaction(transactionUri , ComputerName,channelUri);
//			}
//			catch(System.Exception Error)
//			{
//				throw new System.Exception("TPMonitor can't propagate the native transaction to the machine with identity '"+ComputerName+"'.",Error);  
//			}
//
//		}
//		public void CatchTransaction( string transactionUri,string ChannelURI,byte[] transactionStream)
//		{
//			NativeTPMonitor nativeTPMonitor=Remoting.RemotingServices.CreateInstance(ChannelURI,typeof(NativeTPMonitor).FullName) as NativeTPMonitor;
//			nativeTPMonitor.ImportTransaction(transactionUri,transactionStream);
//		}
	}
}
