using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime.Commands
{
    /// <MetaDataID>{af67a621-0e83-4bf3-bce9-f28b994837e4}</MetaDataID>
    class UpdateReferentialIntegrity : PersistenceLayerRunTime.Commands.UpdateReferentialIntegrity
    {
        public UpdateReferentialIntegrity(StorageInstanceRef updatedStorageInstanceRef)
        {
            UpdatedStorageInstanceRef = updatedStorageInstanceRef;
        }
         
        public override void Execute()
        {
            Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.UpdateStorageInstance(UpdatedStorageInstanceRef);
            
        }
    }
}
