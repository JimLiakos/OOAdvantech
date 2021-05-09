using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
    /// <MetaDataID>{d6f0de9e-6751-4071-a7cc-adb3d28c4033}</MetaDataID>
    public class EmbeddedStorageProvider:StorageProvider
    {
        /// <MetaDataID>{6c34a9a7-9242-4cce-bcc5-401dc4750db7}</MetaDataID>
        public override bool IsEmbeddedStorage(string StorageName, string StorageLocation)
        {
            return true;
        }
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage OpenStorage(string storageName, string storageLocation, string userName = "", string password = "")
        {
            try
            {  
                return new ObjectStorage(storageName, storageLocation, false, true);
            }
            catch (System.Exception error)
            {
                int tt = 0;
                if (error is System.Data.SqlClient.SqlException)
                {
                    foreach (System.Data.SqlClient.SqlError sqlError in (error as System.Data.SqlClient.SqlException).Errors)
                    {
                        if (sqlError.Number == 4060)
                            throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + storageName + " at location " + storageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist, error);

                    }

                }
                throw;
                

            }
        }
        public override OOAdvantech.PersistenceLayerRunTime.ObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage OriginalStorage, string storageName, string storageLocation, string userName = "", string password = "")
        {

            string sqlServerInstanceName = storageLocation;
           
                CreateSQLDatabase(storageName, sqlServerInstanceName);





            using (Transactions.SystemStateTransition StateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope(Transactions.TransactionInterop.GetSystemTransaction(Transactions.Transaction.Current) as System.Transactions.Transaction))
                {

                    string SQLConnectionString = "Data Source=[Location];Initial Catalog=[DatabaseName];Integrated Security=True";
                    SQLConnectionString = SQLConnectionString.Replace("[DatabaseName]", storageName);
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


                    ObjectStorage storage = new ObjectStorage(storageName, storageLocation, true, true);
                    transactionScope.Complete();
                    StateTransition.Consistent = true;
                    return storage;
                }
            }
        }


   
        
    }
}
