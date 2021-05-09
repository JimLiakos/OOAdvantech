using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime
{
    /// <MetaDataID>{00a4e398-dfc2-46ea-9638-d260dd7c6bec}</MetaDataID>
    public class StorageCell : MetaDataRepository.StorageCell
    {
      
        public override void ActivateAllObjects()
        {
            
        }
        public override OOAdvantech.MetaDataRepository.ObjectIdentityType ObjectIdentityType
        {
            get
            {
                return new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ID", "ID", typeof(int)) });
            }
            set
            {
                
            }
        }

        public override bool IsTypeOf(string ofTypeIdentity)
        {

            if (ofTypeIdentity != null)
            {
                var ofTypeClassifier = StorageCell.GetOfTypeClassifier(ofTypeIdentity, Type);
                if (ofTypeClassifier != null && Type.IsA(ofTypeClassifier))
                    return true;
            }
            return false;
        }

      


        public override OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.MetaObjectID associationIdentity, OOAdvantech.MetaDataRepository.Roles linkedStorageCellsRole)
        {
            throw new NotImplementedException();
        }
        public override int SerialNumber
        {
            get 
            {
                return GetHashCode();
            }
            set
            {
            }
        }
        public override bool AllObjectsInActiveMode
        {
            get 
            { 
                return true; 
            }
        }
        /// <MetaDataID>{35213782-6fc5-4843-95ef-7aa2d8e648f7}</MetaDataID>
        protected StorageCell()
        {
        }

        /// <MetaDataID>{b3451f20-4b12-44b6-a551-a193b5068ec5}</MetaDataID>
        internal PersistenceLayerRunTime.ClassMemoryInstanceCollection Objects;
        /// <MetaDataID>{026a4b4e-8f63-4531-b86c-7379d7a8141f}</MetaDataID>
        public StorageCell(string storageIntentity, MetaDataRepository.Class _class, MetaDataRepository.Namespace _namespace, PersistenceLayerRunTime.ClassMemoryInstanceCollection objects)
        {
            Objects = objects;
            _Namespace.Value = _namespace;
            _StorageIntentity = storageIntentity;
            _Type = _class;
        }
        /// <MetaDataID>{1187a27d-d19a-40f9-9994-c06c035df12d}</MetaDataID>
        string _StorageIntentity;
        public override string StorageIdentity
        {
            get
            {
                return _StorageIntentity;
            }
        }
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
