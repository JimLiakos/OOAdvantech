namespace OOAdvantech.MSSQLPersistenceRunTime.MSSQLDataObjects
{
	/// <MetaDataID>{6B7403C3-15D0-4F01-A624-AA0EA21D0A5A}</MetaDataID>
	public class StoreProcedure : MetaDataRepository.Namespace
	{
		protected StoreProcedure()
		{
		}

		/// <MetaDataID>{58E7C460-63AF-4345-9426-F594B292D4F0}</MetaDataID>
		string StoreProcedureDefinition;
		/// <MetaDataID>{E5A9B13B-0DA6-43E1-8F8F-B6A8543AA315}</MetaDataID>
		public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
		{
			RDBMSMetaDataRepository.StoreProcedure OriginStoreProcedure=(RDBMSMetaDataRepository.StoreProcedure)OriginMetaObject;
            DataBase dataBase = _Namespace as DataBase;
            StoreProcedureDefinition = dataBase.RDBMSSchema.BuildStoreProcedureBodyCode(OriginStoreProcedure, NewStoreProcedure);
			int k=0;

		}
		/// <MetaDataID>{ECC837D4-A048-4A6E-B436-DFFF12C27C83}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{
				return base.Identity;
			}
		}

		/// <MetaDataID>{CD3B58A0-558E-40FC-A33B-8A7B762F39AA}</MetaDataID>
		/// <exclude>Excluded</exclude>
		//private string ConnectionString=null;
		/// <MetaDataID>{3D667887-F13A-4152-B2EF-A78F3C81EECA}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private bool NewStoreProcedure=true;
		/// <MetaDataID>{40FB33EC-F197-4D3B-BF13-9AD6F2E02BBD}</MetaDataID>
		public StoreProcedure(string name, bool newStoreProcedure)
		{
			_Name=name;
			NewStoreProcedure=newStoreProcedure;
			//ConnectionString=connectionString;
		}
		/// <MetaDataID>{52197B73-BAB5-4562-B5FF-B8AF9D87C289}</MetaDataID>
		public void Update()
		{

			//System.Data.SqlClient.SqlConnection Connection=new System.Data.SqlClient.SqlConnection(ConnectionString);
			if(StoreProcedureDefinition==null)
				return;
			DataBase dataBase =_Namespace as DataBase;
			System.Data.Common.DbConnection connection=dataBase.Connection;

			try
			{

				//if(System.EnterpriseServices.ContextUtil.Transaction!=null)
				//	Connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
                System.Data.Common.DbCommand command = connection.CreateCommand();// new System.Data.SqlClient.SqlCommand(StoreProcedureDefinition, Connection);
                command.CommandText = StoreProcedureDefinition;
				command.ExecuteNonQuery();
				NewStoreProcedure=false;
			}
			catch(System.Exception Error)
			{
				//Connection.Close();
				throw new System.Exception("It can't save the Storeprocedure '"+Name+"' changes.");
			}
			//Connection.Close();



		}

	}
}
