using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{
    /// <MetaDataID>{68779a85-492b-4f61-aeba-9174291a7966}</MetaDataID>
    public class DeleteStorageInstanceCommand : PersistenceLayerRunTime.Commands.DeleteStorageInstanceCommand
    {
        /// <MetaDataID>{F95D4413-9829-4D68-A19B-0BE8626D35A8}</MetaDataID>
        public DeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceForDeletion, PersistenceLayer.DeleteOptions deleteOption)
            : base(storageInstanceForDeletion, deleteOption)
        {

        }
        /// <MetaDataID>{17445198-1965-4472-953E-CB3DF3C70EB1}</MetaDataID>
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
            Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.DeleteStorageInstance(StorageInstanceForDeletion);

            //ObjectStorage objectStorage = (ObjectStorage)StorageInstanceForDeletion.ObjectStorage;
            //System.Data.SqlClient.SqlConnection oleDbConnection = objectStorage.DBConnection;
            //RDBMSMetaDataRepository.StoreProcedure deletetoreProcedure = ((StorageInstanceRef)StorageInstanceForDeletion).StorageInstanceSet.DeleteStoreProcedure;
            //if (oleDbConnection.State != System.Data.ConnectionState.Open)
            //    oleDbConnection.Open();
            ////oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);
            ////oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            //System.Data.SqlClient.SqlCommand oleDbCommand = oleDbConnection.CreateCommand();
            //oleDbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //oleDbCommand.CommandText = deletetoreProcedure.Name;
            ////2102511377
            //foreach (RDBMSMetaDataRepository.IdentityColumn column in (StorageInstanceForDeletion as StorageInstanceRef).StorageInstanceSet.MainTable.ObjectIDColumns)
            //{
            //    if (oleDbCommand.Parameters.Contains("@" + column.Name))
            //    {
            //        //oleDbCommand.Parameters.Add("@"+column.Name+((ObjectID)StorageInstanceForDeletion.ObjectID).GetMemberValue(column.ColumnType));
            //        oleDbCommand.Parameters["@" + column.Name].Value = (StorageInstanceForDeletion.ObjectID as ObjectID).GetMemberValue(column.ColumnType);
            //    }
            //    else
            //    {
            //        oleDbCommand.Parameters.Add(
            //            "@" + column.Name,
            //            (StorageInstanceForDeletion.ObjectID as ObjectID).GetMemberValue(column.ColumnType));
            //    }

            //}
            //try
            //{
            //    int RowsAffected = oleDbCommand.ExecuteNonQuery();
            //    int kd = 0;
            //}
            //catch (System.Exception Error)
            //{
            //    throw;
            //    int lo = 0;
            //}
        }
    }
}
