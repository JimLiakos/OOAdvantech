namespace OOAdvantech.MSSQLPersistenceRunTime
{
	/// <MetaDataID>{17C973DE-0CF0-4B46-9016-E21235EC7B52}</MetaDataID>
	public class MemoryInstanceCollection : PersistenceLayerRunTime.MemoryInstanceCollection
	{
		/// <MetaDataID>{44D0870D-E84B-4642-9236-60135650BDF0}</MetaDataID>
		public MemoryInstanceCollection(ObjectStorage objectStorage):base( objectStorage)
		{

		}


		/// <MetaDataID>{A604B1E3-B2B2-4C0F-BF70-5D2D21201E11}</MetaDataID>
		protected override OOAdvantech.PersistenceLayerRunTime.ClassMemoryInstanceCollection CreateClassMemoryInstanceCollection(System.Type _Type)
		{
			return new ClassMemoryInstanceCollection(_Type,OwnerStorageSession);
		}
	}
}
