namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
	
	/// <MetaDataID>{E8B4CFF2-D869-4BAA-B4F0-DBC231D23E10}</MetaDataID>
	public class NewStorageInstanceCommand : OOAdvantech.PersistenceLayerRunTime.Commands.NewStorageInstanceCommand
	{
		/// <MetaDataID>{29AD4F0F-4788-4D87-97F2-0C81938A7A27}</MetaDataID>
		public NewStorageInstanceCommand(StorageInstanceRef storageInstanceRef) :base(storageInstanceRef)
		{

		}
		protected bool UpdateCommandProduced=false;
		public override void GetSubCommands(int currentExecutionOrder)
		{
			if(UpdateCommandProduced)
				return;

			UpdateStorageInstanceCommand updateStorageInstanceCommand=new  UpdateStorageInstanceCommand(OnFlyStorageInstance);
			updateStorageInstanceCommand.FromNewCommand=true;

			PersistenceLayerRunTime.ITransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
			transactionContext.EnlistCommand(updateStorageInstanceCommand);
			UpdateCommandProduced=true;

        }
		
		/// <MetaDataID>{72513422-ADD0-4579-8FCD-25A38B9C4FC9}</MetaDataID>
		/// <summary>With this method execute the command. </summary>
		public override void Execute()
		{
             (OnFlyStorageInstance.ObjectStorage as OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.ObjectStorage).GetNextOID();
            int tt = (OnFlyStorageInstance.ObjectStorage as OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime.ObjectStorage).GetNextOID();
            OnFlyStorageInstance.PersistentObjectID = OnFlyStorageInstance.ObjectID;// new ObjectID( (OnFlyStorageInstance.ObjectStorage as AdoNetObjectStorage).NextOID);
		

			
			
		
		}
	}
}
