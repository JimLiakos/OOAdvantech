using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;
namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{1DBDACA8-E550-4A43-93D7-DC19675C7AD0}</MetaDataID>
    [BackwardCompatibilityID("{1DBDACA8-E550-4A43-93D7-DC19675C7AD0}")]
    [Persistent()]
    public abstract class Storage : Namespace, PersistenceLayer.Storage
    {
        /// <exclude>Excluded</exclude>
        protected string _Culture;
        /// <MetaDataID>{bcaac3fd-1ac9-479e-8eac-38f0f43f19eb}</MetaDataID>
        [PersistentMember(nameof(_Culture))]
        [BackwardCompatibilityID("+25")]
        public virtual string Culture
        {
            get
            {
                return _Culture;
            }
            set
            {
                _Culture = value;
            }
        }

        /// <MetaDataID>{d579fd50-54b6-4c5b-8df0-e45b9c6dfd7d}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {

            if (member.Name == nameof(_NativeStorageID))
            {
                if (value == null)
                    _NativeStorageID = default(string);
                else
                    _NativeStorageID = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LinkedStorages))
            {
                if (value == null)
                    _LinkedStorages = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageReference>);
                else
                    _LinkedStorages = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageReference>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Components))
            {
                if (value == null)
                    _Components = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Component>);
                else
                    _Components = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Component>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageLocation))
            {
                if (value == null)
                    _StorageLocation = default(string);
                else
                    _StorageLocation = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageIdentity))
            {
                if (value == null)
                    _StorageIdentity = default(string);
                else
                    _StorageIdentity = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageType))
            {
                if (value == null)
                    _StorageType = default(string);
                else
                    _StorageType = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageName))
            {
                if (value == null)
                    _StorageName = default(string);
                else
                    _StorageName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{52be7fc9-ec51-4d04-a2cf-f53df5a36f67}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_NativeStorageID))
                return _NativeStorageID;

            if (member.Name == nameof(_LinkedStorages))
                return _LinkedStorages;

            if (member.Name == nameof(_Components))
                return _Components;

            if (member.Name == nameof(_StorageLocation))
                return _StorageLocation;

            if (member.Name == nameof(_StorageIdentity))
                return _StorageIdentity;

            if (member.Name == nameof(_StorageType))
                return _StorageType;

            if (member.Name == nameof(_StorageName))
                return _StorageName;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        protected string _NativeStorageID;

        /// <MetaDataID>{5a7b4545-d506-436f-83de-ad229913ec27}</MetaDataID>
        [PersistentMember("_NativeStorageID"), BackwardCompatibilityID("+4")]
        public string NativeStorageID
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _NativeStorageID;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {
                _NativeStorageID = value;
            }
        }
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<StorageReference> _LinkedStorages = new OOAdvantech.Collections.Generic.Set<StorageReference>();

        [Association("ContextStorages", Roles.RoleA, "1d911a41-ef48-4966-b5e1-6a1e05c2264a")]
        [PersistentMember("_LinkedStorages")]
        [RoleAMultiplicityRange(1)]
        public virtual OOAdvantech.Collections.Generic.Set<StorageReference> LinkedStorages
        {
            get
            {
                return _LinkedStorages.AsReadOnly();
            }
        }


        /// <MetaDataID>{402bebc1-90ed-4ed8-853a-9533cf8f34f4}</MetaDataID>
        public void AddLinkedStorage(OOAdvantech.PersistenceLayer.ObjectStorage objectStorage)
        {
            string storageName = objectStorage.StorageMetaData.StorageName;
            string storageType = objectStorage.StorageMetaData.StorageType;
            string storageDataLocation = objectStorage.StorageMetaData.StorageLocation;
            string nativeStorageID = objectStorage.StorageMetaData.NativeStorageID;

            if (storageName == StorageName &&
                storageType == StorageType &&
                storageDataLocation == StorageLocation)
                return;


            foreach (var storageReference in _LinkedStorages)
            {
                if (!string.IsNullOrEmpty(storageReference.NativeStorageID) && storageReference.NativeStorageID == nativeStorageID)
                    return;

                if (storageReference.StorageName == storageName &&
                    storageReference.StorageType == storageType &&
                    storageReference.StorageLocation == storageDataLocation)
                {
                    return;
                }
            }

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {


                OOAdvantech.MetaDataRepository.StorageReference storageReference = new OOAdvantech.MetaDataRepository.StorageReference();
                storageReference.Name = storageName;
                storageReference.StorageName = storageName;
                storageReference.StorageType = storageType;
                storageReference.StorageLocation = storageDataLocation;
                storageReference.NativeStorageID = nativeStorageID;
                AddLinkedStorage(storageReference);
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(storageReference);
                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{402bebc1-90ed-4ed8-853a-9533cf8f34f4}</MetaDataID>
        public void AddLinkedStorage(StorageReference storage)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _LinkedStorages.Add(storage);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{7f2436b7-52af-4226-b216-53f9005e6d2a}</MetaDataID>
        public void DeleteStorage(StorageReference storage)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _LinkedStorages.Remove(storage);
                stateTransition.Consistent = true;
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C7D19F7B-86CC-4786-8D3C-DAAE78BE5DFF}</MetaDataID>
        protected OOAdvantech.Collections.Generic.Set<Component> _Components = new OOAdvantech.Collections.Generic.Set<Component>();
        /// <MetaDataID>{6CF5556F-A66B-4783-92CA-4B5F8B4163E1}</MetaDataID>
        [Association("StorageComponent", typeof(OOAdvantech.MetaDataRepository.Component), Roles.RoleA, "{32C3C895-BD31-4CB4-983A-3D342EF936E1}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_Components")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<Component> Components
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Components.ToThreadSafeSet();
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{26c17793-3581-469e-ab52-5ff51786e511}</MetaDataID>
        [AssociationEndBehavior(PersistencyFlag.LazyFetching)]
        public override Component ImplementationUnit
        {
            get
            {
                return base.ImplementationUnit;
            }
        }
        /// <MetaDataID>{eada1571-8db5-4dc9-b6f0-9e60651ff1f6}</MetaDataID>
        [AssociationEndBehavior(PersistencyFlag.LazyFetching)]
        public override Namespace Namespace
        {
            get
            {
                return base.Namespace;
            }
        }
        ///// <MetaDataID>{84921445-9ba3-4ab7-ac16-fc3e49df4037}</MetaDataID>
        //public abstract object CreateDataLoader(object dataNode,object searchCondition, Collections.Generic.Set<StorageCell> storageCells);



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{ECE7E791-D1D0-42DC-B5FC-909365E6BCDC}</MetaDataID>
        protected string _StorageLocation;
        /// <MetaDataID>{8CC4C4E2-FC15-4D7D-9D05-4CCAE689BE9C}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember("_StorageLocation")]
        public string StorageLocation
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _StorageLocation;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {
                _StorageLocation = value;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{147CB664-C0C0-4798-83BB-92B39B65240A}</MetaDataID>
        protected string _StorageIdentity = System.Guid.NewGuid().ToString();
        /// <MetaDataID>{D37FD7A4-627B-42B1-B47A-2BC6EA117E5F}</MetaDataID>
        [BackwardCompatibilityID("+31")]
        [PersistentMember(255, "_StorageIdentity")]
        public string StorageIdentity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _StorageIdentity;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5FFBD4E3-1E77-4FD8-B4D7-756F4604DDB9}</MetaDataID>
        [BackwardCompatibilityID("+32")]
        [PersistentMember(255, "_StorageIdentity")]
        protected string _StorageType;
        /// <MetaDataID>{F210B11A-19BF-4F92-B0AA-BC7EBC4A7249}</MetaDataID>
        public string StorageType
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _StorageType;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    _StorageType = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{EBBB042A-A1E3-43B9-BBC4-52F056248D83}</MetaDataID>
        protected string _StorageName;
        /// <MetaDataID>{E128CD12-8BCB-469D-B51F-C0A9C49E7867}</MetaDataID>
        [BackwardCompatibilityID("+23")]
        [PersistentMember("_StorageName")]
        public virtual string StorageName
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _StorageName;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    _StorageName = value;
                    _Name = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{9d795bc6-cc38-4855-9696-3ee92efd1741}</MetaDataID>
        public override string Name
        {
            get
            {
                return StorageName;
            }
            set
            {
                StorageName = value;
            }
        }
        /// <MetaDataID>{5D63290C-FE34-4350-9BC3-05118EDF3807}</MetaDataID>
        public abstract void RegisterComponent(string assemblyFullName);

        /// <MetaDataID>{48475591-AD5D-40F4-8739-1C5C11A1E594}</MetaDataID>
        public abstract void RegisterComponent(string[] assembliesFullNames);


        /// <MetaDataID>{5b2652e1-89ee-41b6-a137-ef68979a6305}</MetaDataID>
        public abstract void RegisterComponent(string assemblyFullName, System.Xml.Linq.XDocument mappingData);
        /// <MetaDataID>{cc2369af-c102-4755-8ec8-3a74352bfa12}</MetaDataID>
        public abstract void RegisterComponent(string assemblyFullName, string mappingDataResourceName);

        /// <MetaDataID>{d739df29-b78b-4083-ba3a-9c354aa08337}</MetaDataID>
        public abstract void RegisterComponent(string[] assembliesFullNames, System.Collections.Generic.Dictionary<string, System.Xml.Linq.XDocument> assembliesMappingData);

        public abstract bool CheckForVersionUpgrate(string fullName);

    }
}
