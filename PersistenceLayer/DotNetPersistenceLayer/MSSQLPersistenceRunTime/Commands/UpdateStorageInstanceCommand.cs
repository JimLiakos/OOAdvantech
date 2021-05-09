namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
    using ObjectID = RDBMSPersistenceRunTime.ObjectID;
    //using OOAdvantech.RDBMSPersistenceRunTime;
	/// <MetaDataID>{13BAB95C-2F11-47EB-B35A-E152E09CEF94}</MetaDataID>
	[MetaDataRepository.BackwardCompatibilityID("{13BAB95C-2F11-47EB-B35A-E152E09CEF94}")]
	public class UpdateStorageInstanceCommand : PersistenceLayerRunTime.Commands.UpdateStorageInstanceCommand
	{
		/// <MetaDataID>{EA69F944-77DF-4BD2-8A10-95329A58641B}</MetaDataID>
		public UpdateStorageInstanceCommand (PersistenceLayerRunTime.StorageInstanceRef updatedStorageInstanceRef):base(updatedStorageInstanceRef)
		{
		}

		
		/// <MetaDataID>{BA72A311-AE1E-4119-8B3B-BDCB40F46F9C}</MetaDataID>
		public override void Execute()
		{

			if(!UpdatedStorageInstanceRef.HasChangeState())
				return;

			ObjectStorage objectStorage=(ObjectStorage)UpdatedStorageInstanceRef.ObjectStorage;
			System.Data.SqlClient.SqlConnection oleDbConnection=objectStorage.DBConnection;
			if(oleDbConnection.State != System.Data.ConnectionState.Open)
				oleDbConnection.Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			RDBMSMetaDataRepository.StoreProcedure updateStoreProcedure=((StorageInstanceRef)UpdatedStorageInstanceRef).StorageInstanceSet.UpdateStoreProcedure;
			System.Data.SqlClient.SqlCommand oleDbCommand=oleDbConnection.CreateCommand();
			((StorageInstanceRef)UpdatedStorageInstanceRef).OleDbCommand=oleDbCommand;
			oleDbCommand.CommandType=System.Data.CommandType.StoredProcedure;
			oleDbCommand.CommandText=updateStoreProcedure.Name;
			foreach(MetaDataRepository.Parameter parameter in updateStoreProcedure.Parameters)
			{
				System.Data.SqlClient.SqlParameter oleDbParameter=oleDbCommand.Parameters.Add("@"+parameter.Name,null);
				if(parameter.Direction==MetaDataRepository.Parameter.DirectionType.InOut)
					oleDbParameter.Direction=System.Data.ParameterDirection.InputOutput;
				if(parameter.Direction==MetaDataRepository.Parameter.DirectionType.Out)
					oleDbParameter.Direction=System.Data.ParameterDirection.Output;
				if(parameter.Direction==MetaDataRepository.Parameter.DirectionType.In)
					oleDbParameter.Direction=System.Data.ParameterDirection.Input;
			}
			((StorageInstanceRef)UpdatedStorageInstanceRef).SaveObjectState();
			foreach(RDBMSMetaDataRepository.IdentityColumn column in (UpdatedStorageInstanceRef as StorageInstanceRef).StorageInstanceSet.MainTable.ObjectIDColumns)
			{
				if(oleDbCommand.Parameters.Contains("@"+column.Name))
				{
					oleDbCommand.Parameters["@"+column.Name].Value=(UpdatedStorageInstanceRef.ObjectID as ObjectID).GetMemberValue(column.ColumnType);
				}
				else
				{
					oleDbCommand.Parameters.Add(
						"@"+column.Name,
						(UpdatedStorageInstanceRef.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
				}
			}
			oleDbCommand.ExecuteNonQuery();
		}
	}
}
