using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime
{
    /// <MetaDataID>{b814a65a-5191-468c-a0f9-57db6649abc8}</MetaDataID>
    public class MemoryInstanceCollection : PersistenceLayerRunTime.MemoryInstanceCollection
    {
        /// <MetaDataID>{2d1f3e8a-d326-4b01-a6bc-4089a470107a}</MetaDataID>
        public MemoryInstanceCollection(PersistenceLayerRunTime.ObjectStorage objectStorage)
            : base(objectStorage)
        {
             
        }


        /// <MetaDataID>{4390250b-7082-4d14-8085-d9d79ba0d1bd}</MetaDataID>
        protected override OOAdvantech.PersistenceLayerRunTime.ClassMemoryInstanceCollection CreateClassMemoryInstanceCollection(System.Type _Type)
        { 
            return new PersistenceLayerRunTime.ClassMemoryInstanceCollection(_Type, OwnerStorageSession);
        }
    }
}
