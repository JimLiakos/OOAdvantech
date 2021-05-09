using System;
using System.Collections.Generic;
using System.Text;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;


namespace OOAdvantech.OracleMetaDataPersistenceRunTime
{

    /// <MetaDataID>{807d20a7-38ce-4ecd-a222-191b2588b704}</MetaDataID>
    public class EmbeddedStorageProvider : StorageProvider
    {

    
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
            catch (System.Exception Error)
            {
                int tt = 0;
                throw new OOAdvantech.PersistenceLayer.StorageException(" Storage " + storageName + " at location " + storageLocation + " doesn't exist", OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist, Error);

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

                    //string connectionString = "Data Source=[Location];User ID=[User];Password=astraxan;Unicode=True";
                    string connectionString = "Data Source=[Location];User ID=[User];Password=astraxan";
                  //  connectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=rocket)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=[Location])));User Id=[User];Password=astraxan;";
                    connectionString = connectionString.Replace("[Location]", sqlServerInstanceName).Replace("[User]",storageName);
                    System.Data.Common.DbConnection localConnection = null;
                    

                    int count = 0;
                    while (localConnection == null || localConnection.State != System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            localConnection = new OracleConnection(connectionString);// StorageProvider.GetMySqlConnection(SQLConnectionString);
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


                    ObjectStorage storage = new ObjectStorage(storageName, storageLocation, true,true);
                    transactionScope.Complete();
                    StateTransition.Consistent = true;
                    return storage;
                }
            }
        }




    }
}
