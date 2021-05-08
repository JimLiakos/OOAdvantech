using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{4305fa69-0fc3-4ad9-ac7f-9cf3d82fe2b6}</MetaDataID>
    public abstract class StorageCellReference:StorageCell
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {


            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {


            return base.GetMemberValue(token, member);
        }

        [Serializable]
        public struct StorageCellReferenceMetaData
        {

            public override bool Equals(object obj)
            {
                if (obj is StorageCellReferenceMetaData)
                {
                    if (((StorageCellReferenceMetaData)obj).ObjectIdentityType == ObjectIdentityType &&
                        ((StorageCellReferenceMetaData)obj).SerialNumber == SerialNumber &&
                        ((StorageCellReferenceMetaData)obj).StorageName == StorageName &&
                        ((StorageCellReferenceMetaData)obj).StorageIdentity == StorageIdentity &&
                        ((StorageCellReferenceMetaData)obj).StorageType == StorageType &&
                        ((StorageCellReferenceMetaData)obj).StorageLocation == StorageLocation)
                    {
                        return true;

                    }
                }
                return false;
            }

            public override int GetHashCode()
            {
                int num = -1162279000;
                num = (-1521134295 * num) + GetHashCode(StorageIdentity);
                num = (-1521134295 * num) + GetHashCode(SerialNumber);
                num = (-1521134295 * num) + GetHashCode(StorageName);
                num = (-1521134295 * num) + GetHashCode(StorageType);
                num = (-1521134295 * num) + GetHashCode(ObjectIdentityType);
                num = (-1521134295 * num) + GetHashCode(StorageLocation);
                return num;
            }


            private int GetHashCode(object partValue)
            {
                if (partValue == null)
                    return 0;
                else
                    return partValue.GetHashCode();
            }

            public StorageCellReferenceMetaData(string storageIdentity, 
                                                int serialNumber,
                                                string storageName,
                                                string storageLocation,
                                                string storageType,
                                                ObjectIdentityType objectIdentityType)
            {
                
                StorageIdentity = storageIdentity;
                SerialNumber = serialNumber;
                StorageName=storageName;
                StorageLocation=storageLocation;
                StorageType=storageType;
                ObjectIdentityType=objectIdentityType;
            }
            public StorageCellReferenceMetaData(StorageCell storageCell)
            {
                StorageIdentity =storageCell.StorageIdentity;
                SerialNumber = storageCell.SerialNumber;
                StorageName = storageCell.StorageName;
                StorageLocation = storageCell.StorageLocation;
                StorageType = storageCell.StorageType;
                ObjectIdentityType = storageCell.ObjectIdentityType;

            }
            public StorageCellReferenceMetaData(StorageCellReferenceMetaData storageCellReferenceMetaData)
            {
                StorageIdentity = storageCellReferenceMetaData.StorageIdentity;
                SerialNumber = storageCellReferenceMetaData.SerialNumber;
                StorageName = storageCellReferenceMetaData.StorageName;
                StorageLocation = storageCellReferenceMetaData.StorageLocation;
                StorageType = storageCellReferenceMetaData.StorageType;
                ObjectIdentityType = storageCellReferenceMetaData.ObjectIdentityType;

            }


            public readonly string StorageIdentity;
            public readonly int SerialNumber;
            public readonly string StorageLocation;
            public readonly string StorageName;
            public readonly string StorageType;
            public readonly ObjectIdentityType ObjectIdentityType;


        }


        [MetaDataRepository.Association("ReferenceStorageCell", typeof(MetaDataRepository.StorageCell), MetaDataRepository.Roles.RoleA, "{4DCE58E3-DB45-4B19-AF4B-07F3B404FD8E}")]
        [MetaDataRepository.RoleAMultiplicityRange(1, 1)]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        public abstract MetaDataRepository.StorageCell RealStorageCell
        {
            get;
        }
    }
    /// <MetaDataID>{d1165ff3-6b09-4f8c-bafb-9e6923e83a95}</MetaDataID>
    public sealed class OnFlyStorageCellReference : StorageCellReference
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(StorageCellReferenceMetaData))
            {
                if (value == null)
                    StorageCellReferenceMetaData = default(OOAdvantech.MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData);
                else
                    StorageCellReferenceMetaData = (OOAdvantech.MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RealStorageCell))
            {
                if (value == null)
                    _RealStorageCell = default(OOAdvantech.MetaDataRepository.StorageCell);
                else
                    _RealStorageCell = (OOAdvantech.MetaDataRepository.StorageCell)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(StorageCellReferenceMetaData))
                return StorageCellReferenceMetaData;

            if (member.Name == nameof(_RealStorageCell))
                return _RealStorageCell;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{65a764d9-101f-405e-b34e-03267303e26a}</MetaDataID>
        StorageCellReference.StorageCellReferenceMetaData StorageCellReferenceMetaData;
        /// <MetaDataID>{a80295df-bcb2-4091-a1b4-b13070f4b9fb}</MetaDataID>
        public OnFlyStorageCellReference(StorageCellReference.StorageCellReferenceMetaData storageCellReferenceMetaData)
        {
            StorageCellReferenceMetaData=storageCellReferenceMetaData;
        }


        /// <MetaDataID>{04052f53-f7c4-4119-99cb-5b600a1cfe9f}</MetaDataID>
        public OnFlyStorageCellReference(StorageCell storageCell, MetaDataRepository.Storage storage)
        {
            StorageCellReferenceMetaData = new StorageCellReferenceMetaData( storageCell);
            _Namespace.Value = storage;
        }

        /// <MetaDataID>{bf3fb75c-11f0-4b0f-aa52-600b28022e91}</MetaDataID>
        public OnFlyStorageCellReference()
        {
        }
        /// <MetaDataID>{5cfc928d-e73b-4db7-9957-d4cbe8f41204}</MetaDataID>
        public override OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.MetaObjectID associationIdentity, OOAdvantech.MetaDataRepository.Roles linkedStorageCellsRole)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{aa1d3e75-cbe5-46df-8f74-b05c93787cd0}</MetaDataID>
        public override ObjectIdentityType ObjectIdentityType
        {
            get
            {
                 return StorageCellReferenceMetaData.ObjectIdentityType;
            }
            set
            {
            }
        }



        /// <MetaDataID>{5098b7d5-073e-4b31-b1cd-b7f17022153b}</MetaDataID>
        MetaDataRepository.StorageCell _RealStorageCell;


        /// <MetaDataID>{f904e2f1-05a9-40e8-8706-ba550e73a6ba}</MetaDataID>
        public override MetaDataRepository.StorageCell RealStorageCell
        {

            get
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    if (_RealStorageCell != null)
                        return _RealStorageCell;
                    PersistenceLayer.ObjectStorage ObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType);

                    _RealStorageCell = ObjectStorage.GetStorageCell(SerialNumber);
                    return _RealStorageCell;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }


        /// <MetaDataID>{63d019d8-c3f6-46d4-a192-5cedc49ac983}</MetaDataID>
        public override string StorageType
        {
            get
            {
                return  StorageCellReferenceMetaData.StorageType;
            }
            set
            {

            }
        }


        /// <MetaDataID>{5c4a5603-89da-45da-a453-ac5ebbcadad0}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }
        /// <MetaDataID>{c6b28f6e-0570-427b-980a-451f17e27c1e}</MetaDataID>
        public override void ActivateAllObjects()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsTypeOf(string ofTypeIdentity)
        {
            if (string.IsNullOrEmpty(ofTypeIdentity))
                return false;
            return RealStorageCell.IsTypeOf(ofTypeIdentity);
        }

        /// <MetaDataID>{6a00d6f6-6580-45ac-a1d6-35b780393ca1}</MetaDataID>
        public override bool AllObjectsInActiveMode
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{e68eaf99-319c-4075-9664-7605432d9190}</MetaDataID>
        public override string StorageLocation
        {
            get
            {
                return StorageCellReferenceMetaData.StorageLocation;
            }
            set
            {

            }
        }

        /// <MetaDataID>{bd1c074d-0932-4e7c-9e73-690d6b2a7ca5}</MetaDataID>
        public override string StorageIdentity
        {
            get
            {
                return StorageCellReferenceMetaData.StorageIdentity;
            }
            set
            {
                // base.StorageIntentity = value;
            }
        }

        /// <MetaDataID>{09a0eea4-bdc5-454a-9df9-e8a3fac1619d}</MetaDataID>
        public override string StorageName
        {
            get
            {
                return StorageCellReferenceMetaData.StorageName;
            }
            set
            {

            }
        }
        /// <MetaDataID>{b070429a-9a1f-47f4-834e-6b3f5e1c2da9}</MetaDataID>
        public override int SerialNumber
        {
            get
            {
                return StorageCellReferenceMetaData.SerialNumber;
            }
            set
            {

            }
        }



      
    }


}
