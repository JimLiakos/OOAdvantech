using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace OOAdvantech.MetaDataLoadingSystem
{
    /// <MetaDataID>{0bda90ab-115a-4221-820c-154f77e2a5c7}</MetaDataID>
    public class StorageCellReference : OOAdvantech.MetaDataRepository.StorageCellReference
    {

        protected StorageCellReference()
        {
        }

        public override OOAdvantech.MetaDataRepository.ObjectIdentityType ObjectIdentityType
        {
            get
            {
                return RealStorageCell.ObjectIdentityType;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public XElement StorageCellReferenceElement;
        public StorageCellReference(XElement storageCellReferenceElement)
        {

            StorageCellReferenceElement = storageCellReferenceElement;
        }
        public StorageCellReference(MetaDataRepository.StorageCell realStorageCell)
        {
            _RealStorageCell = realStorageCell;
        }

        public string OID
        {
            get
            {
                return StorageCellReferenceElement.GetAttribute("OID");
            }
        }
        public override OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.MetaObjectID associationIdentity, OOAdvantech.MetaDataRepository.Roles linkedStorageCellsRole)
        {
            throw new NotImplementedException();
        }

        MetaDataRepository.StorageCell _RealStorageCell;
        public override MetaDataRepository.StorageCell RealStorageCell
        {

            get
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    if (_RealStorageCell != null)
                        return _RealStorageCell;

                    var storageMetaData = PersistenceLayer.StorageServerInstanceLocator.Current.GetSorageMetaData(StorageIdentity);
                    PersistenceLayer.ObjectStorage ObjectStorage = null;
                    if (!string.IsNullOrWhiteSpace( storageMetaData.StorageIdentity)&& storageMetaData.MultipleObjectContext)
                        ObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageMetaData.StorageName, storageMetaData.StorageLocation, storageMetaData.StorageType);
                    else
                        ObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType);


                    _RealStorageCell = ObjectStorage.GetStorageCell(SerialNumber);
                    return _RealStorageCell;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }




        public override bool IsTypeOf(string ofTypeIdentity)
        {
            if (string.IsNullOrWhiteSpace(ofTypeIdentity))
                return false;
            return RealStorageCell.IsTypeOf(ofTypeIdentity);
        }


        public override string StorageType
        {
            get
            {
                if (_RealStorageCell != null)
                    return _RealStorageCell.StorageType;
                return StorageCellReferenceElement.Parent.Parent.GetAttribute("StorageType");
            }
            set
            {

            }
        }


        /// <MetaDataID>{5c4a5603-89da-45da-a453-ac5ebbcadad0}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new List<object>(); 
        }
        /// <MetaDataID>{c6b28f6e-0570-427b-980a-451f17e27c1e}</MetaDataID>
        public override void ActivateAllObjects()
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{6a00d6f6-6580-45ac-a1d6-35b780393ca1}</MetaDataID>
        public override bool AllObjectsInActiveMode
        {
            get
            {
                return false;
            }
        }

        public override string StorageLocation
        {
            get
            {
                if (_RealStorageCell != null)
                    return _RealStorageCell.StorageLocation;

                return StorageCellReferenceElement.Parent.Parent.GetAttribute("StorageLocation");
            }
            set
            {

            }
        }

        public override string StorageIdentity
        {
            get
            {
                if (_RealStorageCell != null)
                    return _RealStorageCell.StorageIdentity;

                return StorageCellReferenceElement.Parent.Parent.GetAttribute("StorageIdentity");
            }
            set
            {
                // base.StorageIntentity = value;
            }
        }

        public override string StorageName
        {
            get
            {
                if (_RealStorageCell != null)
                    return _RealStorageCell.StorageName;

                return StorageCellReferenceElement.Parent.Parent.GetAttribute("StorageName");
            }
            set
            {

            }
        }
        public override int SerialNumber
        {
            get
            {
                if (_RealStorageCell != null)
                    return _RealStorageCell.SerialNumber;

                string serialNumberStr = StorageCellReferenceElement.GetAttribute("SerialNumber");
                int serialNumber = 0;
                int.TryParse(serialNumberStr, out serialNumber);
                return serialNumber;
            }
            set
            {

            }
        }


        internal void SetNamespace(Storage storage)
        {
            _Namespace.Value = storage;
        }
    }
}
