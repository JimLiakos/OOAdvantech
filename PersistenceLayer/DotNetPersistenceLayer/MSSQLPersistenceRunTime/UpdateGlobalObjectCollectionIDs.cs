namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
	/// <MetaDataID>{4AC1A3DD-B7B8-4739-8DA8-F15B44BEE1F0}</MetaDataID>
	public class UpdateGlobalObjectCollectionIDs : PersistenceLayerRunTime.Commands.Command
	{

		public override string Identity
		{
			get
			{
				return GetType().FullName+GetHashCode().ToString();
			}
		}

		/// <MetaDataID>{315A71C7-0BC8-4F68-B8A9-B6D4B2BE3B67}</MetaDataID>
		public UpdateGlobalObjectCollectionIDs(ObjectStorage updatingStorage,RDBMSMetaDataRepository.StorageCell newStorageCell)
		{
			
			UpdatingStorage=updatingStorage;
			NewStorageCell=newStorageCell;
		}
		/// <MetaDataID>{BF67D7FA-1BD1-4689-ABFE-14380BF91E99}</MetaDataID>
		public ObjectStorage UpdatingStorage;
		/// <MetaDataID>{829F21A3-0D40-4ABF-96F7-8CD79B97D920}</MetaDataID>
		public RDBMSMetaDataRepository.StorageCell NewStorageCell;
		/// <summary>Priority defines the order in which will be executed the command.</summary>
		/// <MetaDataID>{92DF9FF8-853E-4D6A-B1DD-629111AC48F9}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 40;
			}
		}
		/// <MetaDataID>{1C61C298-A021-4F28-B669-F8CD4993068F}</MetaDataID>
		public override void GetSubCommands(int currentOrder)
		{
			
		}
		/// <MetaDataID>{9F21E12B-2887-4D59-ACE5-DEBDB988C1D1}</MetaDataID>
		public override void Execute()
		{
			string CommandText=null;
			string NewStorageCellID=null;
			if(NewStorageCell .GetType()==typeof(RDBMSMetaDataRepository.StorageCellReference))
			{
				RDBMSMetaDataRepository.StorageCellReference OutStorageCell=NewStorageCell as RDBMSMetaDataRepository.StorageCellReference;
				NewStorageCellID=System.Convert.ToString((int)System.Convert.ChangeType(OutStorageCell.StorageCellID,typeof(int)),16);
				while(NewStorageCellID.Length<8)
					NewStorageCellID='0'+NewStorageCellID;
				NewStorageCellID="0x"+	OutStorageCell.StorageID+NewStorageCellID;
				NewStorageCellID=NewStorageCellID.Replace("-","");
				NewStorageCellID=NewStorageCellID.ToUpper();
			}
			else
			{
				NewStorageCellID=System.Convert.ToString((int)System.Convert.ChangeType(PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(NewStorageCell.Properties).ObjectID,typeof(int)),16);
				while(NewStorageCellID.Length<8)
					NewStorageCellID='0'+NewStorageCellID;
				NewStorageCellID="0x"+	UpdatingStorage.StorageMetaData.StorageIdentity+NewStorageCellID;
				NewStorageCellID=NewStorageCellID.Replace("-","");
				NewStorageCellID=NewStorageCellID.ToUpper();
			}
			string InsertCommandText="\nif not exists "+
				"(select T_GlobalObjectCollectionIDs.ObjectCollectionID "+
				"from T_GlobalObjectCollectionIDs "+
				"where T_GlobalObjectCollectionIDs.ObjectCollectionID = @ObjectCollectionID)"+
				"begin "+
				"INSERT INTO T_GlobalObjectCollectionIDs "+
				"          (InStoragelID, ObjectCollectionID,OutStorageID) "+
				"VALUES     (@InStoragelID, @ObjectCollectionID,@OutStorageID) "+
				"end ";
			InsertCommandText=InsertCommandText.Replace("@InStoragelID",PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(NewStorageCell.Properties).ObjectID.ToString());
			InsertCommandText=InsertCommandText.Replace("@ObjectCollectionID",NewStorageCellID);
			InsertCommandText=InsertCommandText.Replace("@OutStorageID","0");
			CommandText+=InsertCommandText;
            //UpdatingStorage.DBConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //UpdatingStorage.DBConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
			System.Data.SqlClient.SqlCommand  Command=UpdatingStorage.DBConnection.CreateCommand();
			Command.CommandText=CommandText;
			Command.ExecuteNonQuery();
	
		}
	}
}
