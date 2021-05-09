using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.PersistenceLayerRunTime
{

    /// <MetaDataID>{d75defb0-b90e-4871-ad97-966a7a883260}</MetaDataID>
    public class ObjectsLink
    {

        /// <MetaDataID>{67f8b062-8b24-476d-bffc-5093bbdd8889}</MetaDataID>
        public override int GetHashCode()
        {
            int num = -1162279000;
            num = (-1521134295 * num) + GetHashCode(RoleA.RealStorageInstanceRef);
            num = (-1521134295 * num) + GetHashCode(RoleB.RealStorageInstanceRef);
            num = (-1521134295 * num) + GetHashCode(Association.Identity);
            num = (-1521134295 * num) + GetHashCode(Change);
            return num;
        } 
          
        /// <MetaDataID>{90f57d42-a9c4-4018-afe2-e0f9fe9ddf05}</MetaDataID>
        private int GetHashCode(object partValue)
        {
            if (partValue == null)
                return 0;
            else
                return partValue.GetHashCode();
        }
        /// <MetaDataID>{883f0a3a-6189-4165-b2b0-c3e344bae8fb}</MetaDataID>
        public static bool operator ==(ObjectsLink left, ObjectsLink right)
        {
            if (!(left is ObjectsLink) && !(right is ObjectsLink))
                return true;
            if (!(left is ObjectsLink) || !(right is ObjectsLink))
                return false;
            if (left.RoleA.RealStorageInstanceRef == right.RoleA.RealStorageInstanceRef &&
                left.RoleB.RealStorageInstanceRef == right.RoleB.RealStorageInstanceRef &&
                left.Association.Identity == right.Association.Identity &&
                left.Change == right.Change)
                return true;
            else
                return false;
        }
        /// <MetaDataID>{7cf4a8ca-1d9c-4fde-a04b-cabbccb698a0}</MetaDataID>
        public static bool operator !=(ObjectsLink left, ObjectsLink right)
        {
            return !(left == right);
        }
        /// <MetaDataID>{c28b0eb7-abf1-4173-9130-c45fb86a0526}</MetaDataID>
        public override bool Equals(object obj)
        {
            return (obj as ObjectsLink)==this;
        }

        /// <MetaDataID>{841066c3-5c47-495e-91e9-6b17f2e828c3}</MetaDataID>
        public ObjectsLink(MetaDataRepository.Association association, StorageInstanceAgent roleA, StorageInstanceAgent roleB, TypeOfChange change)
        {
            Association = association;
            RoleA = roleA;
            RoleB = roleB;
            Change = change;
        }

        /// <MetaDataID>{d26c27b6-720d-4de5-97ab-964546e454f9}</MetaDataID>
        public ObjectsLink(MetaDataRepository.Association association, StorageInstanceAgent roleA, StorageInstanceAgent roleB,StorageInstanceAgent relationObject,  TypeOfChange change)
        {
            Association = association;
            RoleA = roleA;
            RoleB = roleB;
            RelationObject = relationObject;
            Change = change;
        }
        public enum TypeOfChange
        {
            Added,
            Removed
        }
        /// <MetaDataID>{0909e815-9f52-44d7-879e-e428e339ee10}</MetaDataID>
        public readonly StorageInstanceAgent RoleA;
        /// <MetaDataID>{728dc956-7a95-4cf4-8471-4ee4e0d4d04e}</MetaDataID>
        public readonly StorageInstanceAgent RoleB;
        /// <MetaDataID>{217a7fb2-72e1-4421-87f2-771f8cb3c94f}</MetaDataID>
        public readonly StorageInstanceAgent RelationObject;
        /// <MetaDataID>{ccb9749a-76e6-447a-abf5-9d71fea59621}</MetaDataID>
        public readonly TypeOfChange Change;
        /// <MetaDataID>{536cf555-4706-48b5-bfa9-99ae559919ae}</MetaDataID>
        public readonly MetaDataRepository.Association Association;



    }
}
