using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{64a04d2d-958b-48aa-a981-84fbdeb3081f}</MetaDataID>
    public class StorageCell : MetaDataRepository.StorageCell
    {

        /// <MetaDataID>{d4a6f4e2-5a23-4f49-84b4-ea7bb511969e}</MetaDataID>
        public override void ActivateAllObjects()
        {

        }
        /// <MetaDataID>{9f05c410-75b1-4875-9531-3bf244231fe8}</MetaDataID>
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

        /// <MetaDataID>{64cdb9bc-0779-4d2f-9cf2-20f405bb571a}</MetaDataID>
        public override OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.MetaObjectID associationIdentity, OOAdvantech.MetaDataRepository.Roles linkedStorageCellsRole)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{e384c491-9880-44ed-8343-30ea7a5cc995}</MetaDataID>
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
        /// <MetaDataID>{b4b6fde8-1140-4660-8295-c238e7246145}</MetaDataID>
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
        /// <MetaDataID>{3b032dd0-65b1-4f6f-a5c7-ce75bb84040e}</MetaDataID>
        public override string StorageIdentity
        {
            get
            {
                return _StorageIntentity;
            }
        }
        /// <MetaDataID>{d4b564b8-d62c-4782-b775-fbe54f2b5a63}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
