using OOAdvantech.Transactions;
using OOAdvantech.MetaDataRepository;
using System;

namespace OOAdvantech.RDBMSMetaDataRepository
{
    /// <MetaDataID>{76533F1F-A963-4906-86D3-3A4803EE25F8}</MetaDataID>
    public class StorageCellReference : OOAdvantech.MetaDataRepository.StorageCellReference
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_StorageType))
            {
                if (value == null)
                    _StorageType = default(string);
                else
                    _StorageType = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ObjectIdentityTypeParts))
            {
                if (value == null)
                    ObjectIdentityTypeParts = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.IdentityPart>);
                else
                    ObjectIdentityTypeParts = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.IdentityPart>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectIdentityType))
            {
                if (value == null)
                    _ObjectIdentityType = default(OOAdvantech.MetaDataRepository.ObjectIdentityType);
                else
                    _ObjectIdentityType = (OOAdvantech.MetaDataRepository.ObjectIdentityType)value;
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
            if (member.Name == nameof(_TimePeriodEnd))
            {
                if (value == null)
                    _TimePeriodEnd = default(System.DateTime);
                else
                    _TimePeriodEnd = (System.DateTime)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_TimePeriodStart))
            {
                if (value == null)
                    _TimePeriodStart = default(System.DateTime);
                else
                    _TimePeriodStart = (System.DateTime)value;
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
            if (member.Name == nameof(RealConcreteClassView))
            {
                if (value == null)
                    RealConcreteClassView = default(OOAdvantech.RDBMSMetaDataRepository.View);
                else
                    RealConcreteClassView = (OOAdvantech.RDBMSMetaDataRepository.View)value;
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
            if (member.Name == nameof(_SerialNumber))
            {
                if (value == null)
                    _SerialNumber = default(int);
                else
                    _SerialNumber = (int)value;
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

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_StorageType))
                return _StorageType;

            if (member.Name == nameof(ObjectIdentityTypeParts))
                return ObjectIdentityTypeParts;

            if (member.Name == nameof(_ObjectIdentityType))
                return _ObjectIdentityType;

            if (member.Name == nameof(_StorageName))
                return _StorageName;

            if (member.Name == nameof(_TimePeriodEnd))
                return _TimePeriodEnd;

            if (member.Name == nameof(_TimePeriodStart))
                return _TimePeriodStart;

            if (member.Name == nameof(_StorageLocation))
                return _StorageLocation;

            if (member.Name == nameof(RealConcreteClassView))
                return RealConcreteClassView;

            if (member.Name == nameof(_RealStorageCell))
                return _RealStorageCell;

            if (member.Name == nameof(_SerialNumber))
                return _SerialNumber;

            if (member.Name == nameof(_StorageIdentity))
                return _StorageIdentity;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        string _StorageType;
        /// <MetaDataID>OOAdvantech.RDBMSMetaDataRepository.StorageCellReference.11</MetaDataID>
        [MetaDataRepository.PersistentMember("_StorageType"), MetaDataRepository.BackwardCompatibilityID("+11")]
        public override string StorageType
        {
            get
            {
                return _StorageType;
            }
            set
            {
            }
        }
        /// <MetaDataID>{3953341f-f5c1-400f-b097-050386fe8023}</MetaDataID>
        [BackwardCompatibilityID("+61")]
        [PersistentMember("_Type")]
        public override MetaDataRepository.Class Type
        {
            get
            {
                if (_Type != null)
                {
                    
                    if (_Type.GetExtensionMetaObject<Type>() == null)
                    {
                        

                        var @Class  =(Namespace as Storage).GetEquivalentMetaObject(RealStorageCell.TypeIdentity.ToString(), typeof(MetaDataRepository.Class));
                        //OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage((Namespace as Storage).StorageName, (Namespace as Storage).StorageLocation, (Namespace as Storage).StorageType);

                        throw new Exception("MetaData error on " + GetType().Name + ": " + FullName + " Type: " + _Type.FullName);
                    }
                }

                return base.Type;
            }
            set
            {
                base.Type = value;
            }
        }

        [MetaDataRepository.PersistentMember]
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(1)]
        [MetaDataRepository.Association("StorageCellObjectIdentityTypeParts", typeof(MetaDataRepository.IdentityPart), MetaDataRepository.Roles.RoleA, "256dc414-e22a-40ab-95ae-040505b43c98")]
        protected Collections.Generic.Set<MetaDataRepository.IdentityPart> ObjectIdentityTypeParts = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.IdentityPart>();
        /// <MetaDataID>{36e6244d-1a60-4a56-9db4-0461c2176155}</MetaDataID>
        [OOAdvantech.MetaDataRepository.CommitObjectStateInStorageCall]
        void OnCommitTransintObjectState()
        {

            PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
            foreach (MetaDataRepository.IdentityPart part in ObjectIdentityTypeParts)
            {
                if (!PersistenceLayer.ObjectStorage.IsPersistent(part))
                    objectStorage.CommitTransientObjectState(part);
            }

        }
        /// <MetaDataID>{64a323c5-172a-4087-8c00-eba9524e95a5}</MetaDataID>
        MetaDataRepository.ObjectIdentityType _ObjectIdentityType;
        /// <MetaDataID>{62a9e3d0-739d-43ca-ac11-2ebbb764ed30}</MetaDataID>

        [MetaDataRepository.BackwardCompatibilityID("+15")]
        public override MetaDataRepository.ObjectIdentityType ObjectIdentityType
        {
            get
            {

                if (_ObjectIdentityType == null)
                {
                    System.Collections.Generic.List<MetaDataRepository.IIdentityPart> parts = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                    foreach (MetaDataRepository.IIdentityPart part in ObjectIdentityTypeParts)
                        parts.Add(part);
                    _ObjectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);
                }

                return _ObjectIdentityType;
            }
            set
            {



            }
        }


        /// <exclude>Excluded</exclude>
        string _StorageName;
        /// <MetaDataID>{468F5C0E-4611-48EF-B472-27078200304C}</MetaDataID>
        [MetaDataRepository.PersistentMember("_StorageName")]
        [MetaDataRepository.BackwardCompatibilityID("+5")]
        public override string StorageName
        {
            get
            {
                return _StorageName;
            }
            set
            {
            }
        }
        /// <exclude>Excluded</exclude>
        System.DateTime _TimePeriodEnd;

        /// <MetaDataID>{94df29e7-9879-44d3-833a-2d011038d347}</MetaDataID>
        /// <summary>This property defines the end time of storage cell time period. 
        /// All object of storage cell created before this time.</summary>
        [MetaDataRepository.PersistentMember("_TimePeriodEnd")]
        [MetaDataRepository.BackwardCompatibilityID("+14")]
        public System.DateTime TimePeriodEnd
        {
            get
            {
                return _TimePeriodEnd;
            }
            set
            {
            }
        }

        /// <MetaDataID>{98b79b61-ec12-47e5-aad1-d68b53560d7f}</MetaDataID>
        System.DateTime _TimePeriodStart;
        /// <MetaDataID>{8b40c27e-45bb-4d1b-8bb0-12436a0b42ae}</MetaDataID>
        /// <summary>This property defines the start time of storage cell time period. 
        /// All object of storage cell created after this time.</summary>
        [MetaDataRepository.PersistentMember("_TimePeriodStart")]
        [MetaDataRepository.BackwardCompatibilityID("+13")]
        public System.DateTime TimePeriodStart
        {
            get
            {
                return _TimePeriodStart;
            }
            set
            {
            }
        }
        /// <exclude>Excluded</exclude>
        string _StorageLocation;
        /// <MetaDataID>{899BEEFC-F5EF-4F72-9E68-368E2CC95BBC}</MetaDataID>
        [MetaDataRepository.PersistentMember("_StorageLocation")]
        public override string StorageLocation
        {
            set
            {

            }
            get
            {
                return _StorageLocation;
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
        /// <MetaDataID>{6a00d6f6-6580-45ac-a1d6-35b780393ca1}</MetaDataID>
        public override bool AllObjectsInActiveMode
        {
            get
            {
                return false;
            }
        }
        /// <MetaDataID>{e0b2aed6-b425-487d-a224-420736722219}</MetaDataID>
        public override OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.MetaObjectID associationIdentity, OOAdvantech.MetaDataRepository.Roles linkedStorageCellsRole)
        {
            return RealStorageCell.GetLinkedStorageCells(associationIdentity, linkedStorageCellsRole);
        }
        /// <MetaDataID>{489EBE4D-5471-4CB2-B8D9-C854047719C8}</MetaDataID>
        private View RealConcreteClassView;

        /// <MetaDataID>{C46F14D8-346B-4447-A82D-E8AD4A5DEDD9}</MetaDataID>
        public virtual View ConcreteClassView
        {

            get
            {

                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (RealConcreteClassView == null && RealStorageCell is StorageCell)
                        RealConcreteClassView = (RealStorageCell as StorageCell).ClassView;
                    return RealConcreteClassView;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
            set
            {

            }
        }




        /// <MetaDataID>{32FB6500-37A8-4B34-88E1-2D540D215306}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private MetaDataRepository.StorageCell _RealStorageCell;

        /// <MetaDataID>{60320cf6-852d-48fc-a2eb-6cc2cf8b10e4}</MetaDataID>
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
                    if(!string.IsNullOrWhiteSpace( storageMetaData.StorageIdentity))
                        ObjectStorage=PersistenceLayer.ObjectStorage.OpenStorage(storageMetaData.StorageName, storageMetaData.StorageLocation, storageMetaData.StorageType);
                    else
                        ObjectStorage=PersistenceLayer.ObjectStorage.OpenStorage(StorageName, StorageLocation, StorageType);


                    _RealStorageCell = ObjectStorage.GetStorageCell(SerialNumber);
                    //string Query = "SELECT StorageCell FROM " + typeof(StorageCell).FullName + " StorageCell WHERE StorageCell.SerialNumber = " + SerialNumber.ToString();

                    //Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorage(ObjectStorage.StorageMetaData).Execute(Query);
                    //foreach (Collections.StructureSet Rowset in aStructureSet)
                    //{
                    //    //TODO: να τσεκαριστή εάν υπάρχει συμβατότητα μεταξύ της class που τρέχει τοπικά
                    //    //και αυτής που τρέχει remotely
                    //    //WHERE oid = "+StorageCellID.ToString();
                    //    StorageCell storageCell = (StorageCell)Rowset.Members["StorageCell"].Value;
                    //    if (storageCell.StorageIntentity == StorageIntentity)
                    //        _RealStorageCell = storageCell;
                    //    break;
                    //}

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

        ///// <MetaDataID>{D820E6E8-7A1B-4D9A-BA4C-88AEBE577C3C}</MetaDataID>
        //public StorageCellReference(Class type, StorageCell StorageCell, PersistenceLayer.Storage LiveInObjectStorage)
        //{
        //    _Type = type;

        //    Name = LiveInObjectStorage.StorageName + "." + StorageCell.Name;
        //    _SerialNumber = StorageCell.SerialNumber;
        //    _StorageIntentity = LiveInObjectStorage.StorageIdentity;
        //    _StorageLocation = LiveInObjectStorage.StorageLocation;
        //    _StorageName = LiveInObjectStorage.StorageName;
        //    //_ObjectIdentityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(StorageCell.ObjectIdentityType);
        //    foreach (MetaDataRepository.IIdentityPart part in StorageCell.ObjectIdentityType.Parts)
        //        ObjectIdentityTypeParts.Add(new OOAdvantech.MetaDataRepository.IdentityPart(part));

        //    _TimePeriodStart = StorageCell.TimePeriodStart;
        //    _TimePeriodEnd = StorageCell.TimePeriodEnd;
        //    type.ImplementationUnit.Context.AddOwnedElement(this);
        //    SetIdentity(new MetaDataRepository.MetaObjectID("ref_" + _StorageIntentity + "_" + SerialNumber.ToString()));

        //}


        /// <MetaDataID>{f277803d-910b-40a5-8c88-c8a44d14a748}</MetaDataID>
        public StorageCellReference(OOAdvantech.RDBMSMetaDataRepository.Class type,
                                    string orgStorageCellName,
                                    int orgStorageCellID,
                                    MetaDataRepository.ObjectIdentityType objectIdentityType,
                                    string orgStorageName,
                                    string orgStorageIdentity,
                                    string orgStorageLocation,
                                    string orgStorageType)
        {
            _Type = type;

            Name = orgStorageName + "." + orgStorageCellName;
            _SerialNumber = orgStorageCellID;
            _StorageIdentity = orgStorageIdentity;
            _StorageLocation = orgStorageLocation;
            _StorageName = orgStorageName;
            _StorageType = orgStorageType;
            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                ObjectIdentityTypeParts.Add(new OOAdvantech.MetaDataRepository.IdentityPart(part));

            //TimePeriodStart = StorageCell.TimePeriodStart;
            //TimePeriodEnd = StorageCell.TimePeriodEnd;
            type.ImplementationUnit.Context.AddOwnedElement(this);
            SetIdentity(new MetaDataRepository.MetaObjectID("ref_" + _StorageIdentity + "_" + SerialNumber.ToString()));

        }

        /// <MetaDataID>{1144AD88-4D46-450C-8FCF-817B9F8F1452}</MetaDataID>
        protected StorageCellReference()
        {

        }


        /// <MetaDataID>{877DC59C-045D-4280-8394-AE8317BB6663}</MetaDataID>
        public bool IsSurrogateOf(StorageCell StorageCell, PersistenceLayer.Storage LiveInObjectStorage)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(StorageCell.Properties).PersistentObjectID.Equals(SerialNumber) && LiveInObjectStorage.StorageIdentity == _StorageIdentity)
                    return true;
                return false;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }



        /// <exclude>Excluded</exclude>
        int _SerialNumber;
        /// <MetaDataID>{4d7b03f3-8fe7-4f78-8724-ed67d47a9ade}</MetaDataID>
        [MetaDataRepository.PersistentMember("_SerialNumber")]
        public override int SerialNumber
        {
            set
            {

            }
            get
            {
                return _SerialNumber;
            }
        }

        /// <exclude>Excluded</exclude>
        string _StorageIdentity;
        /// <MetaDataID>{6d52d0e5-3743-4779-ac7d-a49e196a0293}</MetaDataID>
        [MetaDataRepository.PersistentMember("_StorageIdentity")]
        public override string StorageIdentity
        {
            set
            {

            }
            get
            {
                return _StorageIdentity;
            }
        }
    }

}
