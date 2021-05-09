namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
	
	/// <MetaDataID>{62DE77EA-8B76-4B06-BC3E-79374AA49A76}</MetaDataID>
	public class UnLinkObjectsCommand:PersistenceLayerRunTime.Commands.UnLinkObjectsCommand
	{
        public UnLinkObjectsCommand(PersistenceLayerRunTime.ObjectStorage objectStorage,PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index) :
			base(objectStorage,roleA,roleB,relationObject,linkInitiatorAssociationEnd, index)
		{

			//if ((roleA.MemoryInstance is OOAdvantech.MetaDataRepository.MetaObject && roleA.PersistentObjectID != null && (roleA.MemoryInstance as OOAdvantech.MetaDataRepository.MetaObject).Name == "List`1") ||
			//(roleB.MemoryInstance is OOAdvantech.MetaDataRepository.MetaObject && roleB.PersistentObjectID != null && (roleB.MemoryInstance as OOAdvantech.MetaDataRepository.MetaObject).Name == "List`1"))
			//{

			//}
		}
	}
}
