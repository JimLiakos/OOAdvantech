namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime.Commands
{
	/// <MetaDataID>{5C5EBFD6-EEEE-44A5-86CD-DC97CA70745D}</MetaDataID>
	public class DeleteStorageInstanceCommand : OOAdvantech.PersistenceLayerRunTime.Commands.DeleteStorageInstanceCommand
	{
		/// <MetaDataID>{C37DAC75-3A77-4593-8ADE-B59491B5E885}</MetaDataID>
		public DeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceForDeletion,PersistenceLayer.DeleteOptions deleteOption):base(storageInstanceForDeletion,deleteOption)
		{
		
		}
		/// <MetaDataID>{C5FFD006-35F7-40B7-9F32-E994A379A871}</MetaDataID>
		/// <summary>With this method execute the command. </summary>
		public override void Execute()
		{
		
			try
			{
				base.Execute();
			}
			catch(System.Exception Error)
			{
				if(DeleteOption==PersistenceLayer.DeleteOptions.TryToDelete)
					return;
				else
					throw  new System.Exception(Error.Message,Error);
			}

            var SQLConnection = (StorageInstanceForDeletion.ObjectStorage as AdoNetObjectStorage).Connection;
            string ID = (StorageInstanceForDeletion.ObjectStorage as AdoNetObjectStorage).GetSQLScriptForName("ID");
            string IDParName = (StorageInstanceForDeletion.ObjectStorage as AdoNetObjectStorage).GetAdoNetParameterName("ID");


			string deleteStatement=string.Format("DELETE FROM ObjectBLOBS WHERE({0}={1})",ID,IDParName);

            var deleteSqlCommand = SQLConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(deleteStatement, SQLConnection);
            deleteSqlCommand.CommandText = deleteStatement;
            var OIDParameter = deleteSqlCommand.CreateParameter();
            OIDParameter.ParameterName = IDParName;
            OIDParameter.DbType = DbType.Int32;
            deleteSqlCommand.Parameters.Add(OIDParameter);

                //deleteSqlCommand.Parameters.Add("@OID",System.Data.SqlDbType.Int);
			OIDParameter.Value=StorageInstanceForDeletion.PersistentObjectID.GetMemberValue("ObjectID");
			if(SQLConnection.State!= ConnectionState.Open)
				SQLConnection.Open();
            //SQLConnection.EnlistTransaction(System.Transactions.Transaction.Current);
			//TODO:Εαν χρησιμοποιήσουμε ένα connection αν κάθε transaction τότε θα γλιτώσουμε την παρακάτω εντολή που καταναλώνει χρόνο 
			//SQLConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			try
			{
				deleteSqlCommand.ExecuteNonQuery();
				using(Transactions.ObjectStateTransition stateTransition =new OOAdvantech.Transactions.ObjectStateTransition(StorageInstanceForDeletion))
				{
					StorageInstanceForDeletion.PersistentObjectID=null;
					stateTransition.Consistent=true;
				}
				
			}
	#if DEBUG
			catch(System.Exception Error)
			{
				throw new System.Exception(Error.Message,Error);
			}
#endif
			finally
			{
			}

		}
	}
}
