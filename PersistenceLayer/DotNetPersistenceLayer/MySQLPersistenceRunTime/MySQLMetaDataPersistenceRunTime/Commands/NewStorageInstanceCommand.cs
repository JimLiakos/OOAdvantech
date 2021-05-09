using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MySQLMetaDataPersistenceRunTime.Commands
{
    /// <MetaDataID>{64db44b6-3d00-4b52-a46a-d913f3bb8b8f}</MetaDataID>
    public class NewStorageInstanceCommand :RDBMSMetaDataPersistenceRunTime.Commands.NewStorageInstanceCommand
    {

        public NewStorageInstanceCommand(RDBMSMetaDataPersistenceRunTime.StorageInstanceRef storageInstanceRef)
            : base(storageInstanceRef)
        {
        }
        public override void GetSubCommands(int currentExecutionOrder)
        {

            //base.GetSubCommands(currentExecutionOrder);
            if (UpdateCommandProduced)
                return;


            Commands.UpdateStorageInstanceCommand updateStorageInstanceCommand = new Commands.UpdateStorageInstanceCommand(OnFlyStorageInstance);
            updateStorageInstanceCommand.FromNewCommand = true;

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(updateStorageInstanceCommand);


            //(OnFlyStorageInstance.ActiveStorageSession as StorageSession).CreateUpdateStorageInstanceCommand(OnFlyStorageInstance);

            UpdateCommandProduced = true;
        }
    }
}
