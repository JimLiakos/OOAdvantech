namespace OOAdvantech.MetaDataRepository
{
	/// <MetaDataID>{6D1FDB60-F312-4E47-9599-06AA0D1335D8}</MetaDataID>
	public class AddCommand
	{
		/// <MetaDataID>{CADCF766-67D4-4D4B-9FF4-176A31E0E3A3}</MetaDataID>
		public MetaObject MissingMetaObject;
		/// <MetaDataID>{AD601966-0891-42CE-8EC1-0C1CF0F6903B}</MetaDataID>
		public MetaObject AddedObject;
		/// <MetaDataID>{8C0A09C0-DD9D-42B7-B7EC-F07E1FCEC826}</MetaDataID>
		public System.Collections.IList UpdateCollection;
		

		/// <MetaDataID>{5FD79C96-15FA-4C74-AB77-D197B96D87C0}</MetaDataID>
		 public AddCommand(MetaObject missingMetaObject, System.Collections.IList theUpdateCollection)
		{
			MissingMetaObject=missingMetaObject;
			UpdateCollection=theUpdateCollection;
		}

	}
}
