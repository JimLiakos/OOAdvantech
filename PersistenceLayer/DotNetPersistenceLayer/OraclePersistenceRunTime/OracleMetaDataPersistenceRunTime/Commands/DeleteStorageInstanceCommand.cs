using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.OracleMetaDataPersistenceRunTime.Commands
{
    /// <MetaDataID>{0f6ff5a6-e69f-4c13-b93d-f468fe327161}</MetaDataID>
    public class DeleteStorageInstanceCommand : OOAdvantech.PersistenceLayerRunTime.Commands.DeleteStorageInstanceCommand
    {
        /// <MetaDataID>{C37DAC75-3A77-4593-8ADE-B59491B5E885}</MetaDataID>
        public DeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceForDeletion, PersistenceLayer.DeleteOptions deleteOption)
            : base(storageInstanceForDeletion, deleteOption)
        {

        }
        /// <MetaDataID>{C5FFD006-35F7-40B7-9F32-E994A379A871}</MetaDataID>
        /// <summary>With this method execute the command. </summary>
        public override void Execute()
        {

            try
            {
                base.Execute();
            }
            catch (System.Exception Error)
            {
                if (DeleteOption == PersistenceLayer.DeleteOptions.TryToDelete)
                    return;
                else
                    throw new System.Exception(Error.Message, Error);
            }

            System.Data.Common.DbConnection SQLConnection = (StorageInstanceForDeletion.ObjectStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).Connection;

            string deleteStatement =string.Format( "DELETE FROM ObjectBLOBS WHERE(ID=@OID)",StorageInstanceForDeletion.ObjectStorage.StorageMetaData.StorageName);

            System.Data.Common.DbCommand deleteSqlCommand = SQLConnection.CreateCommand();// new System.Data.SqlClient.SqlCommand(deleteStatement, SQLConnection);
            deleteSqlCommand.CommandText = deleteStatement;
            System.Data.Common.DbParameter OIDParameter = deleteSqlCommand.CreateParameter();
            OIDParameter.ParameterName = "@OID";
            OIDParameter.DbType = System.Data.DbType.Int32;
            deleteSqlCommand.Parameters.Add(OIDParameter);

            //deleteSqlCommand.Parameters.Add("@OID",System.Data.SqlDbType.Int);
            OIDParameter.Value = StorageInstanceForDeletion.ObjectID;
            if (SQLConnection.State != System.Data.ConnectionState.Open)
                SQLConnection.Open();
            //SQLConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            //TODO:Εαν χρησιμοποιήσουμε ένα connection αν κάθε transaction τότε θα γλιτώσουμε την παρακάτω εντολή που καταναλώνει χρόνο 
            //SQLConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            try
            {
                deleteSqlCommand.ExecuteNonQuery();
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(StorageInstanceForDeletion))
                {
                    StorageInstanceForDeletion.ObjectID = null;
                    stateTransition.Consistent = true;
                }

            }
#if DEBUG
            catch (System.Exception Error)
            {
                throw new System.Exception(Error.Message, Error);
            }
#endif
            finally
            {
            }

        }
    }
}
