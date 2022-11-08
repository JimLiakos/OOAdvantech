using System;
using OOAdvantech.PersistenceLayer;

namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
	/// <MetaDataID>{A249F131-566D-4766-AD4D-10248E39F428}</MetaDataID>
	public class StorageProvider : OOAdvantech.PersistenceLayerRunTime.StorageProvider	
	{
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
		/// <MetaDataID>{A87CF9FD-D853-4EB1-B3FD-59BDA43DA541}</MetaDataID>
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
		/// <MetaDataID>{6609289D-C6F0-4A60-AE71-2AFF17B8CC61}</MetaDataID>
		public override bool AllowEmbeddedStorage()
		{
			return false;
		}
		/// <MetaDataID>{29A4CF6A-89BE-4CAB-A91A-1A23F12A2334}</MetaDataID>
		public override string GetHostComuterName(string StorageName, string StorageLocation)
		{
			return System.Net.Dns.GetHostName();
		}
		/// <MetaDataID>{2B22AE58-F4BA-4813-88F3-F5FDC71E4FC6}</MetaDataID>
		public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
		{
			return false;
		}


		

		/// <MetaDataID>{D9EFEB36-D09B-4ED5-82D2-46D05FBF16CF}</MetaDataID>
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
            string MasterDBConnectionString = "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";
            MasterDBConnectionString = MasterDBConnectionString.Replace("[DatabaseName]", "master");
            MasterDBConnectionString = MasterDBConnectionString.Replace("[Location]", storageLocation);

            //string ConnectionString="Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source="+StorageLocation;
            System.Data.SqlClient.SqlConnection MasterDBConnection = new System.Data.SqlClient.SqlConnection(MasterDBConnectionString);
            bool storageExist = false;
            System.Data.SqlClient.SqlCommand Command = null;
            System.Data.SqlClient.SqlDataReader DataReader = null;
            try
            {
                MasterDBConnection.Open();
                Command = new System.Data.SqlClient.SqlCommand("exec sp_databases", MasterDBConnection);
                DataReader = Command.ExecuteReader();

                foreach (System.Data.Common.DbDataRecord CurrRecord in DataReader)
                {
                    if (storageName.Equals(CurrRecord["DATABASE_NAME"].ToString()))
                    {
                        storageExist = true;
                        break;

                    }
                }

                DataReader.Close();
                if (!storageExist)
                {
                    string dataBaseCreationString = "CREATE DATABASE [#DatabaseName#] ALTER DATABASE [#DatabaseName#] MODIFY FILE   (NAME = [#DatabaseName#], SIZE = 20MB)";
                    dataBaseCreationString = dataBaseCreationString.Replace("#DatabaseName#", storageName);

                    Command = new System.Data.SqlClient.SqlCommand(dataBaseCreationString, MasterDBConnection);
                    Command.ExecuteNonQuery();
                }
                MasterDBConnection.Close();

                string connectionString = "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";
                connectionString = connectionString.Replace("[DatabaseName]", storageName);
                connectionString = connectionString.Replace("[Location]", storageLocation);
                System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
                int count = 10;
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                {
                    while (count > 0)
                    {
                        count--;
                        try
                        {
                            connection.Open();
                            connection.Close();
                            break;
                        }
                        catch (System.Exception error)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    transactionScope.Complete();
                }


            }
            catch (System.Exception Error)
            {
                throw new System.Exception("The storage location " + storageLocation + " can't be accessed.", Error);
            }
        }
		/// <MetaDataID>{211B1B2B-8B28-4E15-BD19-8E10B8005940}</MetaDataID>
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
                foreach (System.Xml.XmlElement xmlElement in storageServerConfig.GetElementsByTagName("OOAdvantech.MSSQLPersistenceRunTime.StorageProvider"))
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


        public  PersistenceLayerRunTime.ObjectStorage NewStorage( string storageName, string storageLocation,string connectionString)
        {
            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {
                    string SQLConnectionString = connectionString;
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
                    ObjectStorage storage = new ObjectStorage(storageName, storageLocation, connectionString,true);
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

            throw new System.Exception("OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider doesn't suport raw storage data functionality.");

        }

        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string StorageName, object rawStorageData)
        {
            throw new System.Exception("OOAdvantech.MSSQLFastPersistenceRunTime.StorageProvider doesn't suport raw storage data functionality.");
        }

        public OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string storageName, string storageLocation, string nativeStorageConnectionString)
        {
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(nativeStorageConnectionString);
            connection.Open();
            try
            {
                System.Data.Common.DbCommand command = connection.CreateCommand(); //new System.Data.SqlClient.SqlCommand("SELECT ID, ClassBLOBSID, ObjectData FROM ObjectBLOBS", Connection);
                command.CommandText = "SELECT COUNT(*)  FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = 'ObjectBLOBS'";
                if ((int)command.ExecuteScalar() == 1)
                {
                    command.CommandText = "SELECT COUNT(*)  FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = 'ClassBLOBS'";
                    if ((int)command.ExecuteScalar() == 1)
                    {

                        ObjectStorage objectStorage = new ObjectStorage(storageName, storageLocation, nativeStorageConnectionString,false);
                        return objectStorage;
                    }
                }

            }
            finally
            {
                connection.Close();
            }
            return null;
         
        }

        public override OOAdvantech.PersistenceLayer.ObjectStorage CreateNewLogicalStorage(OOAdvantech.PersistenceLayer.ObjectStorage hostingObjectStorage, string storageName)
        {
            throw new NotImplementedException();
        }


        public override void Repair(string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public override void Restore(IBackupArchive archive, string storageName, string storageLocation, string storageType, bool inProcess, string userName, string password, bool overrideObjectStorage)
        {
            throw new NotImplementedException();
        }
    }
}
