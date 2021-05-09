namespace OOAdvantech.MSSQLPersistenceRunTime.MSSQLDataObjects
{
	/// <MetaDataID>{5689747B-8B25-4F5B-9DD0-6D07795D9A7C}</MetaDataID>
	public abstract class StoreprocedureCodeBuilder
	{
		/// <MetaDataID>{75C01A90-2B71-4B14-A761-D848EA25745A}</MetaDataID>
		public abstract string BuildStoreProcedureBodyCode(RDBMSMetaDataRepository.StoreProcedure storeProcedure ,bool create);
		/// <MetaDataID>{16A2927A-89A6-4EA6-A390-725C2D5EEA9B}</MetaDataID>
		public static StoreprocedureCodeBuilder StoreprocedureBuilder=new UUIDStoreprocedureBuilder();// new HighLowStoreprocedureBuilder();


	}
}
