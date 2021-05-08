namespace OOAdvantech.MetaDataRepository
{

    ///<summary>
    /// RelatedStorageCell is a lighter version of storage cells link nad contains data
    /// about relation between two storage cells.
    /// it is usefull for parent data node sub datanode relation through Association
    ///</summary>
    /// <MetaDataID>{8070dfec-a1c8-49dc-880a-4c47553b9c08}</MetaDataID>
    public struct RelatedStorageCell
    {


        public readonly string AssociationEndIdentity;
        ///<summary>
        ///Defines the storage cell for sub datanode
        ///</summary>
        /// <MetaDataID>{f3657791-2b1b-48df-b6b9-3456e221d4f1}</MetaDataID>
        public readonly StorageCell StorageCell;
        /// <summary>
        /// Defines the storage cell for parent Datanode  
        /// </summary>
        /// <MetaDataID>{83351074-9d94-411a-a5d4-042312b16138}</MetaDataID>
        public readonly StorageCell RootStorageCell;

        ///<summary>
        ///if it is true the query angine use relation table to load relation able
        ///else use foreign columns on parent or childe table 
        ///</summary>
        /// <MetaDataID>{8f2bdae5-a8ea-481a-8ab0-3fa99664074c}</MetaDataID>
        public readonly bool ThrougthRelationTable;

        /// <MetaDataID>{007458f3-2656-4d89-a630-2332434ee17f}</MetaDataID>
        public RelatedStorageCell(StorageCell storageCell, StorageCell rootStorageCell, string associationEndIdentity, bool througthRelationTable)
        {
            RootStorageCell = rootStorageCell;
            StorageCell = storageCell;
            ThrougthRelationTable = througthRelationTable;
            AssociationEndIdentity = associationEndIdentity;
        }

    }
    /// <MetaDataID>{C8805239-81B3-49AF-9DDA-A8634232862B}</MetaDataID>
    public class StorageCellsLink : MetaObject
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_ValueTypePath))
            {
                if (value == null)
                    _ValueTypePath = default(string);
                else
                    _ValueTypePath = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Type))
            {
                if (value == null)
                    _Type = default(OOAdvantech.MetaDataRepository.Association);
                else
                    _Type = (OOAdvantech.MetaDataRepository.Association)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RoleBStorageCell))
            {
                if (value == null)
                    _RoleBStorageCell = default(OOAdvantech.MetaDataRepository.StorageCell);
                else
                    _RoleBStorageCell = (OOAdvantech.MetaDataRepository.StorageCell)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RoleAStorageCell))
            {
                if (value == null)
                    _RoleAStorageCell = default(OOAdvantech.MetaDataRepository.StorageCell);
                else
                    _RoleAStorageCell = (OOAdvantech.MetaDataRepository.StorageCell)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_AssotiationClassStorageCells))
            {
                if (value == null)
                    _AssotiationClassStorageCells = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>);
                else
                    _AssotiationClassStorageCells = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_ValueTypePath))
                return _ValueTypePath;

            if (member.Name == nameof(_Type))
                return _Type;

            if (member.Name == nameof(_RoleBStorageCell))
                return _RoleBStorageCell;

            if (member.Name == nameof(_RoleAStorageCell))
                return _RoleAStorageCell;

            if (member.Name == nameof(_AssotiationClassStorageCells))
                return _AssotiationClassStorageCells;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{6673dfe1-86bf-460d-ad73-d919f764de59}</MetaDataID>
        protected StorageCellsLink()
        {

        }
        /// <exclude>Excluded</exclude>
        string _ValueTypePath;
        /// <MetaDataID>{db58e58e-39d3-4eb3-a317-bb6be19d7931}</MetaDataID>
        [PersistentMember("_ValueTypePath")]
        [BackwardCompatibilityID("+4")]
        public string ValueTypePath
        {
            get
            {

                return _ValueTypePath;
            }
            set
            {
                _ValueTypePath = value;

            }
        }
        /// <MetaDataID>{d1f49982-b26a-4f85-9d0e-b8c5b9aada63}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }
        /// <MetaDataID>{A8F3B617-0D0C-45EA-8846-8C0C19C4B8E8}</MetaDataID>
        public StorageCellsLink(Association type, StorageCell roleAStorageCell, StorageCell roleBStorageCell)
        {
            _Name = type.Name;
            _Type = type;
            _RoleAStorageCell = roleAStorageCell;
            _RoleBStorageCell = roleBStorageCell;
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{DC02E108-287C-4530-B5B2-8F02F3C5AFBD}</MetaDataID>
        protected Association _Type;
        /// <MetaDataID>{D1C11C24-8928-4F81-8721-B6CA55065A0B}</MetaDataID>
        [Association("LinksInstanceTypeRelation", typeof(Association), Roles.RoleB, "{E5C85CF2-{ECFC605B-F4AA-41B4-BD22-130AC73F5BEA}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Type")]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.MetaDataRepository.Association Type
        {
            get
            {
                return _Type;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C11B4D29-C029-4622-A8B8-D8BDCCE94AEE}</MetaDataID>
        private StorageCell _RoleBStorageCell;
        /// <MetaDataID>{DAA94328-6A5A-490E-AF54-C39C159C8230}</MetaDataID>
        [Association("RoleBStorageCell", typeof(StorageCell), Roles.RoleB, "{BDE55936-1DA1-4D6A-B8A4-446BF1141003}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_RoleBStorageCell")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.MetaDataRepository.StorageCell RoleBStorageCell
        {
            get
            {
                return _RoleBStorageCell;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{75D518AB-110A-4E43-A562-9883E6E5E1BF}</MetaDataID>
        private StorageCell _RoleAStorageCell;
        /// <MetaDataID>{0CABF01B-E2BE-46BD-A8E8-0CD2B02285FE}</MetaDataID>
        [Association("RoleAStorageCell", typeof(StorageCell), Roles.RoleB, "{25B36609-E774-4969-B9BB-BC113E859E61}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_RoleAStorageCell")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.MetaDataRepository.StorageCell RoleAStorageCell
        {
            get
            {
                return _RoleAStorageCell;
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{95E1F001-8BAD-4E4F-82BE-AD516783D6DC}</MetaDataID>
        protected Collections.Generic.Set<StorageCell> _AssotiationClassStorageCells = new OOAdvantech.Collections.Generic.Set<StorageCell>();
        /// <MetaDataID>{6D4C3F90-9C96-46E0-99B1-C4D4FEA83E54}</MetaDataID>
        [Association("StorageCellLinkAssotiationClass", typeof(StorageCell), Roles.RoleB, "{5C183C1D-BCD0-4BFA-92A3-40EB55B51C22}")]
        [PersistentMember("_AssotiationClassStorageCells")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> AssotiationClassStorageCells
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return new OOAdvantech.Collections.Generic.Set<StorageCell>(_AssotiationClassStorageCells, Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }


    }
}
