using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime
{
    /// <MetaDataID>{f47f101f-b4ff-4ead-acab-6555e99a1498}</MetaDataID>
    public class StorageInstanceRef:RDBMSPersistenceRunTime.StorageInstanceRef
    {

        public RDBMSMetaDataRepository.StorageCell StorageInstanceSet;

        public StorageInstanceRef(object memoryInstance, OOAdvantech.MetaDataRepository.StorageCell storageCell, ObjectStorage activeStorageSession, PersistenceLayer.ObjectID objectID)
            : base(memoryInstance, storageCell, activeStorageSession, objectID)
        {


        }



   
      



        protected override PersistenceLayerRunTime.RelResolver CreateRelationResolver(DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
        {
            return new RelResolver(this, thePersistentAssociationEnd, fastFieldAccessor);
        }
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef owner)
        {
            return new RelResolver(owner, thePersistentAssociationEnd, fastFieldAccessor);

        }

    }
}
