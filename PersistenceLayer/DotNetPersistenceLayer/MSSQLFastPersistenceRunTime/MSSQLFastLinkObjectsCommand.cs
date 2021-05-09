namespace OOAdvantech.MSSQLFastPersistenceRunTime.Commands
{
	/// <MetaDataID>{6A6346C0-12BD-41BB-95FF-35E0BDD3C9F3}</MetaDataID>
	public class LinkObjectsCommand : OOAdvantech.PersistenceLayerRunTime.Commands.LinkObjectsCommand
	{
		public LinkObjectsCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA,PersistenceLayerRunTime.StorageInstanceAgent roleB,PersistenceLayerRunTime.StorageInstanceAgent relationObject,PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd,int index):
            base(roleA, roleB, relationObject, linkInitiatorAssociationEnd, index)
		{ 

		}


	}
}
