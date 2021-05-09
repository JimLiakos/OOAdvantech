namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
    using ObjectID = OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
    //using OOAdvantech.RDBMSPersistenceRunTime;
	/// <MetaDataID>{4959CAE1-8C61-438F-95EF-D47E3DD7B739}</MetaDataID>
	public class UpdateReferentialIntegrity : PersistenceLayerRunTime.Commands.UpdateReferentialIntegrity
	{
        /// <MetaDataID>{f0b2091c-22df-455c-a906-574809ee01ff}</MetaDataID>
        public UpdateReferentialIntegrity(StorageInstanceRef updatedStorageInstanceRef)
        {
            UpdatedStorageInstanceRef = updatedStorageInstanceRef;
        }
		/// <MetaDataID>{C1B62F18-71C2-4D0D-9E29-6A87C138E3E4}</MetaDataID>
		public override void Execute()
		{

			if(!UpdatedStorageInstanceRef.HasReferentialIntegrityCountChange)
				return;

			ObjectStorage objectStorage=(ObjectStorage)UpdatedStorageInstanceRef.ObjectStorage;
            RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary = objectStorage.TypeDictionary;

			string UpdateQuery="UPDATE "+((StorageInstanceRef)UpdatedStorageInstanceRef).StorageInstanceSet.MainTable.Name ;
			UpdateQuery+=" Set ";
			UpdateQuery+=((StorageInstanceRef)UpdatedStorageInstanceRef).StorageInstanceSet.MainTable.ReferentialIntegrityColumn.DataBaseColumnName;
			UpdateQuery+=" = ";
			UpdateQuery+=UpdatedStorageInstanceRef.ReferentialIntegrityCount.ToString();
			UpdateQuery+=" WHERE(";
			int count=0;
			foreach(RDBMSMetaDataRepository.IdentityColumn column in ((StorageInstanceRef)UpdatedStorageInstanceRef).StorageInstanceSet.MainTable.ObjectIDColumns)
			{
				if(count!=0)
					UpdateQuery+=") AND (";
				UpdateQuery+=column.Name+" = "+TypeDictionary.ConvertToSQLString(((ObjectID)UpdatedStorageInstanceRef.ObjectID).GetMemberValue(column.ColumnType));
			}
			UpdateQuery+=")";
			System.Data.SqlClient.SqlConnection oleDbConnection=objectStorage.DBConnection;
            
			if(oleDbConnection.State!=System.Data.ConnectionState.Open)
				oleDbConnection.Open();
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);

            //oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand mOleDbCommand=oleDbConnection.CreateCommand();
			mOleDbCommand.CommandText=UpdateQuery;
			int RowsAffected=mOleDbCommand.ExecuteNonQuery();
			if(RowsAffected==0)
				throw new System.Exception("can't update the storage instance of '"+UpdatedStorageInstanceRef.MemoryInstance.ToString()+"' object.");
			//mOleDbConnection.Close();
		}
	}
}
