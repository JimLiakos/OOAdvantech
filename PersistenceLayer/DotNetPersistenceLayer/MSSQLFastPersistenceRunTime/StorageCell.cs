using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MSSQLFastPersistenceRunTime
{
    /// <MetaDataID>{7fac326e-9a5d-464e-81ba-1a925d5e9197}</MetaDataID>
    public class StorageCell : MetaDataRepository.StorageCell
    {
        public override bool AllObjectsInActiveMode
        {
            get 
            { 
                return true; 
            }
        }
        protected StorageCell()
        {
        }

        internal PersistenceLayerRunTime.ClassMemoryInstanceCollection Objects;
        public StorageCell(string storageIntentity, MetaDataRepository.Class _class, MetaDataRepository.Namespace _namespace, PersistenceLayerRunTime.ClassMemoryInstanceCollection objects)
        {
            Objects = objects;
            _Namespace = _namespace;
            _StorageIntentity = storageIntentity;
            _Type = _class;
        }
        string _StorageIntentity;
        public override string StorageIntentity
        {
            get
            {
                return _StorageIntentity;
            }
        }
        public override System.Collections.ArrayList GetExtensionMetaObjects()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
