using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.OracleMetaDataPersistenceRunTime.Commands
{
    /// <MetaDataID>{94614dd3-787d-4236-a11c-54d764e620ae}</MetaDataID>
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
