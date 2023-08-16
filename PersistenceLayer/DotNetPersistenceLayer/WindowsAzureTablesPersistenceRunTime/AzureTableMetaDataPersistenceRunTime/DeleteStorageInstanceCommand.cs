

using System.Linq;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
	/// <MetaDataID>{5C5EBFD6-EEEE-44A5-86CD-DC97CA70745D}</MetaDataID>
	public class DeleteStorageInstanceCommand : OOAdvantech.PersistenceLayerRunTime.Commands.DeleteStorageInstanceCommand
	{
		/// <MetaDataID>{C37DAC75-3A77-4593-8ADE-B59491B5E885}</MetaDataID>
		public DeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceForDeletion,PersistenceLayer.DeleteOptions deleteOption):base(storageInstanceForDeletion,deleteOption)
		{
			if (StorageInstanceForDeletion.MemoryInstance is OOAdvantech.MetaDataRepository.Parameter)
			{

			}

		}

		/// <MetaDataID>{C5FFD006-35F7-40B7-9F32-E994A379A871}</MetaDataID>
		/// <summary>With this method execute the command. </summary>
		public override void Execute()
		{
			if(StorageInstanceForDeletion.MemoryInstance is OOAdvantech.MetaDataRepository.Parameter)
            {

            }
		
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

            var objectBLOBDataTable = ((StorageInstanceForDeletion.ObjectStorage as ObjectStorage).StorageMetaData as Storage).ObjectBLOBDataTable;
            ObjectBLOBData objectBLOBData = (StorageInstanceForDeletion as StorageInstanceRef).ObjectBLOBData;

            try
            {
				//Microsoft.Azure.Cosmos.Table.TableOperation insertOperation = Microsoft.Azure.Cosmos.Table.TableOperation.Delete(objectBLOBData);

				var TableBatchOperation = ((StorageInstanceForDeletion as StorageInstanceRef).ObjectStorage as ObjectStorage).GetTableBatchOperation(objectBLOBDataTable);
				if (objectBLOBData.RowKey == "e8d1ff5e-98bd-46db-a6cf-bbbbc5304aea")
                {

                }
				foreach (var tableOperation in TableBatchOperation.Where(x => x.Entity.RowKey == objectBLOBData.RowKey).ToList())
					TableBatchOperation.Remove(tableOperation);


				//TableBatchOperation.Delete(objectBLOBData);
				TableBatchOperation.Add(new Azure.Data.Tables.TableTransactionAction(Azure.Data.Tables.TableTransactionActionType.Delete, objectBLOBData));

				//objectBLOBDataTable.Execute(insertOperation);
				StorageInstanceForDeletion.PersistentObjectID = null;
				(StorageInstanceForDeletion as StorageInstanceRef).ObjectBLOBData = null;

			}
#if DEBUG
            catch (System.Exception Error)
            {
				if(Error.InnerException is System.Net.WebException && 
					(Error.InnerException as System.Net.WebException).Response is System.Net.HttpWebResponse &&
					((Error.InnerException as System.Net.WebException).Response as System.Net.HttpWebResponse).StatusCode==System.Net.HttpStatusCode.NotFound)
                {
					return;
                }
				else
					throw new System.Exception(Error.Message, Error);
            }
#endif
            finally
            {
            }
		}
	}
}
