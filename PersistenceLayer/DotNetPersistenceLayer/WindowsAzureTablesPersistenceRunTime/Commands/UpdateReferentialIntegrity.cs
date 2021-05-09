using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{
    /// <MetaDataID>{32fbc5ff-3f1b-4b35-9295-837061da0daf}</MetaDataID>
    public class UpdateReferentialIntegrity : PersistenceLayerRunTime.Commands.UpdateReferentialIntegrity
    {
        /// <MetaDataID>{f97b69e2-2a85-40ab-b0f1-ee77d0363686}</MetaDataID>
        public UpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef updatedStorageInstanceRef)
        {
            UpdatedStorageInstanceRef = updatedStorageInstanceRef;
        }
         
        public override void Execute()
        {
            //TODO: κατι θα πρέπει να γίνει αν δεν υπάρχει mapping column για reference count
            if((UpdatedStorageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable.ReferentialIntegrityColumn!=null)
                Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.SetStorageInstanceColumnValue(UpdatedStorageInstanceRef, (UpdatedStorageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable.ReferentialIntegrityColumn.Name, UpdatedStorageInstanceRef.ReferentialIntegrityCount);
        }
    }
}
