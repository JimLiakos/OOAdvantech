namespace PersistenceLayerRunTime
{
	/// <MetaDataID>{A0489897-0BF9-494F-AC5E-B2D6229D6716}</MetaDataID>
	public abstract class AssociationObjectCommand : TransactionCommand
	{
		/// <MetaDataID>{E42F768B-C15C-4E78-90B4-A2E36568BC4B}</MetaDataID>
		public MetaDataRepository.Class AssociationClass;
		/// <MetaDataID>{F86BDFA6-84FF-4AFE-A917-6D90B2A66CC4}</MetaDataID>
		public StorageInstanceRef AssociationRoleB;
		/// <MetaDataID>{AF75639A-4BEA-46AE-AE09-410E4CB0B0BF}</MetaDataID>
		public StorageInstanceRef AssociationObject;
		/// <MetaDataID>{2FB0673A-97C4-42C2-9FB4-4E65967503DE}</MetaDataID>
		public StorageInstanceRef AssociationRoleA;
	}
}
