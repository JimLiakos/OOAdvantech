namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
	/// <MetaDataID>{51BB53E8-CA8D-4752-9D72-B57E62F5C48E}</MetaDataID>
	public class UpdateStorageSchema : PersistenceLayerRunTime.Commands.Command
	{
		/// <MetaDataID>{81471910-9BE5-4611-9317-EF6F6ED7D0AA}</MetaDataID>
        public PersistenceLayerRunTime.ObjectStorage UpdatingStorage;
		/// <summary>Priority defines the order in which will be executed the command.</summary>
		/// <MetaDataID>{0E14CD33-E430-480C-A8F7-2FEA0BE09A4D}</MetaDataID>
		public override int ExecutionOrder
		{
			get
			{
				return 20;
			}
		}
		/// <MetaDataID>{4F784CAE-5F7E-4934-9B4A-5C598FE82FEB}</MetaDataID>
		public override void GetSubCommands(int currentOrder)
		{
			
		}
		/// <MetaDataID>{2F737F99-5BB2-4C57-A6D1-C33F897D5C7B}</MetaDataID>
		public UpdateStorageSchema(PersistenceLayerRunTime.ObjectStorage Storage)
		{
			 UpdatingStorage=Storage;
		
		}
		/// <MetaDataID>{7BFFED5B-5D81-422C-864E-8ABF2F4C340E}</MetaDataID>
		public override void Execute()
		{
			Storage aObjectStorage=UpdatingStorage.StorageMetaData as Storage;
			aObjectStorage.UpdateDataBaseMetadata();
		}

		public override string Identity
		{
			get
			{
				return "UpdateSchema"+UpdatingStorage.StorageMetaData.StorageIdentity;
			}
		}
		public static string GetIdentity(ObjectStorage updatingStorage)
		{
			return "UpdateSchema"+updatingStorage.StorageMetaData.StorageIdentity;
		}
	}
}
