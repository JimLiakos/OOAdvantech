using System;

namespace OOAdvantech.MSSQLFastPersistenceRunTime.Commands
{
	/// <summary>
	/// 
	/// </summary>
	public class BulkInsertCommand:PersistenceLayerRunTime.Commands.Command
	{
		public override int ExecutionOrder
		{
			get
			{
				return 60;
			}
		}
		public override void GetSubCommands(int currentExecutionOrder)
		{

		}
 
		public override void Execute()
		{
			try
			{
				//bool contition=false;
				//if(contition)
				{
					//StorageSession.bulktest();
					StorageSession.Engine.BulkInsertData();
				}
			}
			catch(System.Exception err)
			{
				int rtee=0;
			}
			

		}
 

		StorageSession StorageSession;
		public BulkInsertCommand(StorageSession storageSession)
		{
			StorageSession=storageSession;
			// 
			// TODO: Add constructor logic here
			//
		}
	}
}
