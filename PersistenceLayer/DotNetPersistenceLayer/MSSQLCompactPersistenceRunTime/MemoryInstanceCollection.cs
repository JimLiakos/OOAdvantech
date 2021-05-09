using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime
{
    /// <MetaDataID>{b814a65a-5191-468c-a0f9-57db6649abc8}</MetaDataID>
    public class MemoryInstanceCollection : PersistenceLayerRunTime.MemoryInstanceCollection
    {
        /// <MetaDataID>{2d1f3e8a-d326-4b01-a6bc-4089a470107a}</MetaDataID>
        public MemoryInstanceCollection(ObjectStorage objectStorage)
            : base(objectStorage)
        {

        }


        protected override OOAdvantech.PersistenceLayerRunTime.ClassMemoryInstanceCollection CreateClassMemoryInstanceCollection(System.Type _Type)
        {
            return new ClassMemoryInstanceCollection(_Type, OwnerStorageSession);
        }
    }
}
