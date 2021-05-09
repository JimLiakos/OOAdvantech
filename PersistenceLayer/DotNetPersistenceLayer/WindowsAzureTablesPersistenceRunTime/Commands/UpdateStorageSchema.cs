using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{
    /// <MetaDataID>{6129442a-2bd4-49a2-953f-008613b629ea}</MetaDataID>
    public class UpdateStorageSchema : PersistenceLayerRunTime.Commands.Command
    {
        public override int ExecutionOrder
        {
            get
            {
                return 15;
            }
        }

        public override string Identity
        {
            get
            {
                return "UpdateSchema" + UpdatingStorage.StorageMetaData.StorageIdentity;
            }
        }

        public override void Execute()
        {

        }

        public override void GetSubCommands(int currentExecutionOrder)
        {

        }
        public static string GetIdentity(OOAdvantech.PersistenceLayerRunTime.ObjectStorage updatingStorage)
        {
            return "UpdateSchema" + updatingStorage.StorageMetaData.StorageIdentity;
        }

        public PersistenceLayerRunTime.ObjectStorage UpdatingStorage;

        public UpdateStorageSchema(PersistenceLayerRunTime.ObjectStorage Storage)
        {
            UpdatingStorage = Storage;

        }

        public void UpdateStorageCellsLinks(MetaDataRepository.StorageCellsLink storageCellsLink)
        {
            if (!StorageCellsLinks.Contains(storageCellsLink))
                StorageCellsLinks.Add(storageCellsLink);
        }
        System.Collections.Generic.List<MetaDataRepository.StorageCell> StorageCells = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell>();
        System.Collections.Generic.List<MetaDataRepository.StorageCellsLink> StorageCellsLinks = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCellsLink>();

    }
}
