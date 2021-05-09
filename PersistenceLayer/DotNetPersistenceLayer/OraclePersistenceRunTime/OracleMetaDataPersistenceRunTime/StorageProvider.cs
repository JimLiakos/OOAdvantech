using System;

using System.Collections.Generic;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
namespace OOAdvantech.OracleMetaDataPersistenceRunTime
{

    /// <MetaDataID>{e9b97520-3e4e-4912-95aa-a1f23641dd1e}</MetaDataID>
	public class StorageProvider : OOAdvantech.PersistenceLayerRunTime.StorageProvider	

	{
        


        //static Dictionary<string, Dictionary<string, MySqlConnection>> MySqlConnections = new Dictionary<string, Dictionary<string, MySqlConnection>>();
        //public static MySql.Data.MySqlClient.MySqlConnection GetMySqlConnection(string connectionString)
        //{
        //    connectionString = connectionString.ToLower();
        //    if (System.Transactions.Transaction.Current == null)
        //        return new MySqlConnection(connectionString);
        //    Dictionary<string, MySqlConnection> transactionConnections = null;
        //    if (!MySqlConnections.TryGetValue(System.Transactions.Transaction.Current.TransactionInformation.LocalIdentifier, out transactionConnections))
        //    {
        //        transactionConnections = new Dictionary<string, MySqlConnection>();
        //        MySqlConnection connection = new MySqlConnection(connectionString);
        //        transactionConnections.Add(connectionString, connection);
        //        MySqlConnections.Add(System.Transactions.Transaction.Current.TransactionInformation.LocalIdentifier, transactionConnections);
        //        System.Transactions.Transaction.Current.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(OnSystemTransactionCompleted);
        //        return connection;
        //    }
        //    else
        //    {
        //        MySqlConnection connection = null;
        //        if (!transactionConnections.TryGetValue(connectionString, out connection))
        //        {
        //            connection = new MySqlConnection(connectionString);
        //            transactionConnections.Add(connectionString, connection);
        //        }
        //        return connection;
        //    }
        //}


        //static void OnSystemTransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        //{
        //    System.Transactions.Transaction.Current.TransactionCompleted -= new System.Transactions.TransactionCompletedEventHandler(OnSystemTransactionCompleted);
        //    MySqlConnections.Remove(System.Transactions.Transaction.Current.TransactionInformation.LocalIdentifier);

        //}

        public override string GetNativeStorageID(string storageDataLocation)
        {
            throw new NotImplementedException();
        }
        public override OOAdvantech.PersistenceLayer.Storage AttachStorage(string storageName, string storageLocation, string nativeStorageConnectionString)
        {
            throw new NotImplementedException();
        }
        public override void DeleteStorage(string storageName, string storageLocation)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
		/// <exclude>Excluded</exclude>
        private System.Guid _ProviderID = System.Guid.Empty;
		/// <summary>The Provider identity. Globally unique. </summary>
		public override System.Guid ProviderID
		{
			get
			{
                if(_ProviderID==System.Guid.Empty)
				    _ProviderID=new System.Guid("{581233BA-4457-4d0d-882E-AD163FBFCC7F}");
                return _ProviderID;

			}
			set
			{
			}
		}
		 
		public override bool AllowEmbeddedStorage()
		{
			return false;
		}
		
		public override string GetHostComuterName(string StorageName, string StorageLocation)
		{
			return System.Net.Dns.GetHostName();
		}
		
		public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
		{
			return false;
		}


		

		
		/// <summary>Open a session to access the storage that defined from the parameters. </summary>
		/// <param name="StorageName">The name of Object Storage </param>
		/// <param name="StorageLocation">This parameter contains the location of object storage. </param>
		public override PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, string StorageLocation, string userName = "", string password = "")
		{
			try
			{
				return new ObjectStorage(StorageName,StorageLocation,false,false);
			}
			catch(System.Exception Error)
			{
				int tt=0;
				throw new OOAdvantech.PersistenceLayer.StorageException(" Storage "+StorageName+" at location " +StorageLocation +" doesn't exist",OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist,Error );
				
			}
		
		}
        static public void CreateSQLDatabase(string storageName, string storageLocation)
        {

            //string connectionString = "Data Source=[Location];User ID=system;Password=astraxan;Unicode=True";
            string connectionString = "Data Source=[Location];User ID=system;Password=astraxan";
           // connectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=rocket)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=[Location])));User Id=[User];Password=astraxan;";
            connectionString = connectionString.Replace("[Location]", storageLocation).Replace("[User]","SYSTEM");

            //string ConnectionString="Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source="+StorageLocation;
            System.Data.Common.DbConnection systemDBConnection = new OracleConnection(connectionString);
            bool storageExist = false;
            System.Data.Common.DbCommand command = null;
            System.Data.Common.DbDataReader cataReader = null;
            try
            {
                systemDBConnection.Open();
                command = systemDBConnection.CreateCommand();
                command.CommandText = "select distinct owner from dba_objects";
                cataReader = command.ExecuteReader();

                foreach (System.Data.Common.DbDataRecord CurrRecord in cataReader)
                {
                    if (storageName.ToLower() == CurrRecord["owner"].ToString().ToLower())
                    {
                        storageExist = true;
                        break;

                    }
                }

                cataReader.Close();
                if (!storageExist)
                {
                    bool createTablespace = false;
                    bool createUser = false;

                    

                    command.CommandText = string.Format(@"select distinct count(*)
                                        from dba_tablespaces
                                        where lower(tablespace_name)=lower('{0}')", storageName);

                     
                    createTablespace = Convert.ToInt32(command.ExecuteScalar())==0;

                    command.CommandText = string.Format(@"select distinct count(*)
                                            from dba_users
                                            where lower(username)=lower('{0}')", storageName);
                    createUser= Convert.ToInt32(command.ExecuteScalar()) == 0;

                    if (createTablespace)
                    {
                        command.CommandText = @"select file_name FROM dba_data_files where lower( tablespace_name)=lower('sysaux')";
                        string  fileName = command.ExecuteScalar() as string;
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
                        fileName = fileInfo.DirectoryName + @"\"+storageName + ".dbf";
                        command.CommandText = string.Format(@"create tablespace {0} 
                                                            datafile '{1}'
                                                            size 10M", storageName, fileName);
                        command.ExecuteNonQuery();

                    }
                    if (createUser)
                    {
                        command.CommandText = string.Format(@"CREATE USER {0} IDENTIFIED BY astraxan 
                                              DEFAULT TABLESPACE {0}", storageName);
                        
                        command.ExecuteNonQuery();
                         
                        command.CommandText = string.Format("GRANT ALL PRIVILEGES  TO {0} WITH ADMIN OPTION", storageName);
                        command.ExecuteNonQuery();


                    }

                 
                }
                systemDBConnection.Close();
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("The storage location " + storageLocation + " can't be accessed.", Error);
            }
        }
		
		/// <summary>Create a new Object Storage with schema like original storage and open a storage session with it. </summary>
		/// <param name="OriginalStorage">Cloned Metada (scema) </param>
		/// <param name="StorageName">The name of new Object Storage </param>
		/// <param name="StorageLocation">This parameter contains the location of object storage.
		/// If it is null then the provider will look at Persistence Layer repository. </param>
		public override PersistenceLayerRunTime.ObjectStorage NewStorage(PersistenceLayer.Storage OriginalStorage, string StorageName, string StorageLocation, string userName = "", string password = "")
		{ 

            string sqlServerInstanceName = @"localhost\SQLExpress";
            if (OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.InStorageService)
            {
                
                System.Xml.XmlDocument storageServerConfig = new System.Xml.XmlDocument();
                storageServerConfig.Load(AppDomain.CurrentDomain.BaseDirectory + "StorageServerInstance.config");
                foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.OraclePersistenceRunTime.StorageProvider"))
                {
                    if (xmlElement.HasAttribute("DefaultInsatnceName") && !string.IsNullOrEmpty(xmlElement.GetAttribute("DefaultInsatnceName")))
                        sqlServerInstanceName = xmlElement.GetAttribute("DefaultInsatnceName");
                    break;
                }
                CreateSQLDatabase(StorageName, sqlServerInstanceName);
            }
            else
                CreateSQLDatabase(StorageName, sqlServerInstanceName);

             

            

            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {

                    string SQLConnectionString = "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";
                    SQLConnectionString = SQLConnectionString.Replace("[DatabaseName]", StorageName);
                    SQLConnectionString = SQLConnectionString.Replace("[Location]", sqlServerInstanceName);
                    System.Data.SqlClient.SqlConnection localConnection = null;

                    int count = 0;
                    while (localConnection == null || localConnection.State != System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            localConnection = new System.Data.SqlClient.SqlConnection(SQLConnectionString);
                            localConnection.Open();
                        }
                        catch (System.Exception error)
                        {
                        }
                        if (localConnection.State != System.Data.ConnectionState.Open)
                            System.Threading.Thread.Sleep(200);
                        if (count > 25)
                            break;
                        count++;
                    }
                    if (localConnection != null && localConnection.State == System.Data.ConnectionState.Open)
                        localConnection.Close();


                    ObjectStorage storage = new ObjectStorage(StorageName, StorageLocation, true,false);
                    transactionScope.Complete();
                    StateTransition.Consistent = true; 
                    return storage;
                }
            }
		}

        public override string GetInstanceName(string storageName, string storageLocation)
        {
            int npos =storageLocation.IndexOf('\\');
            if (npos == -1)
                return "default";
            else
                return storageLocation.Substring(npos + 1);
        }

        internal static string GetMSSQLServerName(string storageLocation)
        {
            int npos = storageLocation.IndexOf('\\');
            if (npos == -1)
                return "storageLocation";
            else
                return storageLocation.Substring(0,npos);
        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string StorageName, object rawStorageData)
        {

            throw new System.Exception("OOAdvantech.OracleMetaDataPersistenceRunTime.StorageProvider doesn't suport raw storage data functionality.");

        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            throw new System.Exception("OOAdvantech.OracleMetaDataPersistenceRunTime.StorageProvider doesn't suport raw storage data functionality.");
        }

        public override OOAdvantech.PersistenceLayer.ObjectStorage CreateNewLogicalStorage(OOAdvantech.PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName)
        {
            throw new NotImplementedException();
        }
    }
}
