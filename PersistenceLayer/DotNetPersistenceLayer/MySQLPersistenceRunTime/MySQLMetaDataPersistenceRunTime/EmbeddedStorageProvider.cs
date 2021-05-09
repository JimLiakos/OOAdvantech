using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace OOAdvantech.MySQLMetaDataPersistenceRunTime
{

    /// <MetaDataID>{f5acad06-8064-4032-b92f-8606dec921f5}</MetaDataID>
    public class EmbeddedStorageProvider : StorageProvider
    {

    
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string storageName, string storageLocation)
        {
            try
            {
                return new ObjectStorage(storageName, storageLocation, false, true);
            }
            catch (System.Exception Error)
            {
                int tt = 0;
                throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + storageName + " at location " + storageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist, Error);

            }
        }
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string storageName, string storageLocation)
        {

            string sqlServerInstanceName = storageLocation;

            CreateSQLDatabase(storageName, sqlServerInstanceName);





            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {

                    string connectionString = "Server=[Location];UserId=root;Password=astraxan;";
                    connectionString = connectionString.Replace("[Location]", sqlServerInstanceName);
                    System.Data.Common.DbConnection localConnection = null;

                    int count = 0;
                    while (localConnection == null || localConnection.State != System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            localConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);// StorageProvider.GetMySqlConnection(SQLConnectionString);
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


                    ObjectStorage storage = new ObjectStorage(storageName, storageLocation, true, true);
                    transactionScope.Complete();
                    StateTransition.Consistent = true;
                    return storage;
                }
            }
        }




    }
}
