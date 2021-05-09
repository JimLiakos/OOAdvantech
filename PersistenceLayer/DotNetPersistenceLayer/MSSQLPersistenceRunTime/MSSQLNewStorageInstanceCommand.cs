namespace OOAdvantech.MSSQLPersistenceRunTime.Commands
{
    //using OOAdvantech.RDBMSPersistenceRunTime;
    /// <MetaDataID>{B9143B93-4CF7-4FF3-AE47-5686AF4A8EFF}</MetaDataID>
    /// <summary>Produced any time we call the NewObject method of storage session. 
    /// Its work is to produce one or more records at mapping tables. 
    /// The new records host the state of object.
    /// To do that uses a store procedure with name new_xxxx_instance. </summary>
    public class NewStorageInstanceCommand : PersistenceLayerRunTime.Commands.NewStorageInstanceCommand
    {
        /// <MetaDataID>{80BE8225-93C1-4CC0-8162-5D81CBD43C13}</MetaDataID>
        public NewStorageInstanceCommand(StorageInstanceRef storageInstanceRef)
            : base(storageInstanceRef)
        {

        }
        /// <MetaDataID>{26BA1FAA-2616-4D30-A47B-47F10C70F5C4}</MetaDataID>
        private bool SplitCommandProduced = false;
        /// <MetaDataID>{16D89272-9C26-49E3-A0DF-D6C80902DF35}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {
            if (currentExecutionOrder < 0)
                return;
            base.GetSubCommands(currentExecutionOrder);

            #region Produce split command if needed
            if (!SplitCommandProduced)
            {
                SplitCommandProduced = true;

                ObjectStorage objectStorage = (ObjectStorage)OnFlyStorageInstance.ObjectStorage;
                RDBMSMetaDataRepository.Class theClass = OnFlyStorageInstance.Class.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Class)) as RDBMSMetaDataRepository.Class;
                if (theClass.HistoryClass)
                {
                    theClass.ActiveStorageCell.ObjectsCount++;
                    if (theClass.ActiveStorageCell.ObjectsCount > theClass.SplitLimit)
                    {
                        PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                        if (!transactionContext.ContainCommand(SplitClassActiveStorageCell.GetIdentity(theClass, objectStorage)))
                            transactionContext.EnlistCommand(new SplitClassActiveStorageCell(theClass, objectStorage));

                    }
                }
            }
            #endregion
        }
        /// <MetaDataID>{36DC9E27-F116-4EDE-922D-76418D6269D0}</MetaDataID>
        public override void Execute()
        {
            ObjectStorage objectStorage = (ObjectStorage)OnFlyStorageInstance.ObjectStorage;
            #region Prepare database connection
            System.Data.SqlClient.SqlConnection oleDbConnection = objectStorage.DBConnection;
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
            //if(System.Transactions.Transaction.Current!=null)
            //oleDbConnection.EnlistTransaction(System.Transactions.Transaction.Current);

            //oleDbConnection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction)System.EnterpriseServices.ContextUtil.Transaction);
            #endregion

            #region Prepare command for new storage instance
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(OnFlyStorageInstance.Class) as RDBMSMetaDataRepository.Class;
            ((StorageInstanceRef)OnFlyStorageInstance).StorageInstanceSet = rdbmsMetadataClass.ActiveStorageCell;
            ((StorageInstanceRef)OnFlyStorageInstance).OleDbCommand = oleDbConnection.CreateCommand();
            ((StorageInstanceRef)OnFlyStorageInstance).OleDbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            RDBMSMetaDataRepository.StoreProcedure newStoreProcedure = rdbmsMetadataClass.ActiveStorageCell.NewStoreProcedure;
            ((StorageInstanceRef)OnFlyStorageInstance).OleDbCommand.CommandText = newStoreProcedure.Name;
            foreach (MetaDataRepository.Parameter parameter in newStoreProcedure.Parameters)
            {

                System.Data.SqlClient.SqlParameter oleDbParameter = ((StorageInstanceRef)OnFlyStorageInstance).OleDbCommand.Parameters.Add("@" + parameter.Name, null);
                if (parameter.Direction == MetaDataRepository.Parameter.DirectionType.InOut)
                    oleDbParameter.Direction = System.Data.ParameterDirection.InputOutput;
                if (parameter.Direction == MetaDataRepository.Parameter.DirectionType.Out)
                    oleDbParameter.Direction = System.Data.ParameterDirection.Output;
                if (parameter.Direction == MetaDataRepository.Parameter.DirectionType.In)
                    oleDbParameter.Direction = System.Data.ParameterDirection.Input;
                if (parameter.Name == "ReferenceCount" && oleDbParameter.Value == null)
                    oleDbParameter.Value = 0;

            }
            #endregion

            #region Load parametes values of command
            ((StorageInstanceRef)OnFlyStorageInstance).SaveObjectState();

            int ObjCellID = (OnFlyStorageInstance as StorageInstanceRef).StorageInstanceSet.SerialNumber;
            ObjectID objectID = new ObjectID(System.Guid.Empty, ObjCellID);

            foreach (RDBMSMetaDataRepository.IdentityColumn column in ((StorageInstanceRef)OnFlyStorageInstance).StorageInstanceSet.MainTable.ObjectIDColumns)
            {
                if (!column.ProducedFromRDBMS)
                    ((StorageInstanceRef)OnFlyStorageInstance).OleDbCommand.Parameters["@" + column.Name].Value = objectID.GetMemberValue(column.ColumnType);
            }
            #endregion

            ((StorageInstanceRef)OnFlyStorageInstance).OleDbCommand.ExecuteNonQuery();

            #region Load ProducedFromRDBMS members of ObjectID
            foreach (RDBMSMetaDataRepository.IdentityColumn column in ((StorageInstanceRef)OnFlyStorageInstance).StorageInstanceSet.MainTable.ObjectIDColumns)
            {
                if (column.ProducedFromRDBMS)
                    objectID.SetMemberValue(column.ColumnType, ((StorageInstanceRef)OnFlyStorageInstance).OleDbCommand.Parameters["@" + column.Name].Value);
            }
            #endregion

            OnFlyStorageInstance.ObjectID = objectID;

        }
    }
}
