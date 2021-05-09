namespace OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage
{
	/// <MetaDataID>{305F51FA-FD8D-4118-BD70-84E837ACAC60}</MetaDataID>
	public class ObjectsDataSource:DataSource
	{
		/// <MetaDataID>{49EADA8E-CD3F-4A21-AEB7-62CE991A22BC}</MetaDataID>
        public Collections.Generic.Set<MetaDataRepository.StorageCell> StorageCells;
		/// <MetaDataID>{8657AE7D-78BC-489F-9296-955D0416B98E}</MetaDataID>
		internal ObjectsDataSource(DataNode dataNode)
		{
			DataNode=dataNode;
			View =dataNode.Classifier.TypeView;
			StorageCells=dataNode.Classifier.LocalStorageCells;
		
		}
		/// <MetaDataID>{509A536F-51EC-4B4A-B435-6982098E7F0C}</MetaDataID>
		internal ObjectsDataSource(DataNode dataNode, Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells)
		{
			DataNode=dataNode;
			if(storageCells==null)
			{
				//TODO αυτή η γραμμή κώδικα είναι προσορινή 
				throw new System.Exception("storageCells is null");
				View =dataNode.Classifier.TypeView;
			}
			else
				View =dataNode.Classifier.GetTypeView(storageCells);
			StorageCells=storageCells;
            foreach (RDBMSMetaDataRepository.StorageCell storageCell in StorageCells)
            {
                if (storageCell is RDBMSMetaDataRepository.StorageCellReference)
                {
                    _HasOutStorageCell = true;
                    break;
                }
            }
		}
	}
}
