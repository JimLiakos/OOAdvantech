using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime.Commands
{
    /// <MetaDataID>{e2748e88-13d6-4672-b014-1c726dfaa9b5}</MetaDataID>
    public class UpdateStorageInstanceCommand : PersistenceLayerRunTime.Commands.UpdateStorageInstanceCommand
    {
        /// <MetaDataID>{b1828ad0-537d-4428-a361-2e16676bff3e}</MetaDataID>
        public UpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef updatedStorageInstanceRef)
            : base(updatedStorageInstanceRef)
        {
        }
        /// <MetaDataID>{91c18b07-4e61-4057-96d2-884d40a0faf1}</MetaDataID>
        internal bool FromNewCommand = false;
        /// <MetaDataID>{6D66B69B-AA5A-4AE6-B84A-826D4871F319}</MetaDataID>
        /// <summary>With this method execute the command. </summary>
        public override void Execute()
        {
            Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.UpdateStorageInstance(UpdatedStorageInstanceRef);
        }

    }
}
