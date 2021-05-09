namespace OOAdvantech.MySQLPersistenceRunTime.MSSQLDataObjects
{
	
	public abstract class StoreprocedureCodeBuilder
	{
		
		public abstract string BuildStoreProcedureBodyCode(RDBMSMetaDataRepository.StoreProcedure storeProcedure ,bool create);
		
		public static StoreprocedureCodeBuilder StoreprocedureBuilder=new UUIDStoreprocedureBuilder();// new HighLowStoreprocedureBuilder();


	}
}
