namespace OOAdvantech.RDBMSMetaDataRepository
{
    using System;
    using MetaDataRepository;
    using Transactions;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>StorageCell defines a stack of objects in  database. 
    /// This notion has value in pattern of splintering off. </summary>
    /// <remarks>
    /// Objects (medium of information) classified in class that mapped 
    /// in tables for access or searching reasons. 
    /// To find the object (information) search in data base table 
    /// and does not search in mass storage of disk. 
    /// Some times there is table with million of records. 
    /// In the case of orders, there are orders of last year. 
    /// There are useful only for statistics but participate 
    /// in the searching for an active order. The better way is 
    /// to splintering off table with the aid of some criterions. 
    /// For example you use the criterion of date time also we can characterize 
    /// collections with the quality of search performance. 
    /// For example fast object collections and slow object collections. 
    /// We can tell to the Persistency System to move the object in slow memory. 
    /// </remarks>
    /// <MetaDataID>{EF3D7F38-0DB3-4B50-A93D-7EC81E86534A}</MetaDataID>
    [BackwardCompatibilityID("{EF3D7F38-0DB3-4B50-A93D-7EC81E86534A}")]
    [Persistent()]
    public class StorageCell : OOAdvantech.MetaDataRepository.StorageCell
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_AutoGenarated))
            {
                if (value == null)
                    _AutoGenarated = default(bool);
                else
                    _AutoGenarated = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_AllObjectsInActiveMode))
            {
                if (value == null)
                    _AllObjectsInActiveMode = default(bool);
                else
                    _AllObjectsInActiveMode = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ObjectsInActiveMode))
            {
                if (value == null)
                    ObjectsInActiveMode = default(System.Collections.Generic.List<object>);
                else
                    ObjectsInActiveMode = (System.Collections.Generic.List<object>)value;
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
            if (member.Name == nameof(_StorageIdentity))
            {
                if (value == null)
                    _StorageIdentity = default(string);
                else
                    _StorageIdentity = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_DeleteStoreProcedure))
            {
                if (value == null)
                    _DeleteStoreProcedure = default(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure);
                else
                    _DeleteStoreProcedure = (OOAdvantech.RDBMSMetaDataRepository.StoreProcedure)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_UpdateStoreProcedure))
            {
                if (value == null)
                    _UpdateStoreProcedure = default(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure);
                else
                    _UpdateStoreProcedure = (OOAdvantech.RDBMSMetaDataRepository.StoreProcedure)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_NewStoreProcedure))
            {
                if (value == null)
                    _NewStoreProcedure = default(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure);
                else
                    _NewStoreProcedure = (OOAdvantech.RDBMSMetaDataRepository.StoreProcedure)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectsCount))
            {
                if (value == null)
                    _ObjectsCount = default(int);
                else
                    _ObjectsCount = (int)value;
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
            if (member.Name == nameof(_SerialNumber))
            {
                if (value == null)
                    _SerialNumber = default(int);
                else
                    _SerialNumber = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_MappedTables))
            {
                if (value == null)
                    _MappedTables = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Table>);
                else
                    _MappedTables = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.Table>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ClassView))
            {
                if (value == null)
                    _ClassView = default(OOAdvantech.RDBMSMetaDataRepository.View);
                else
                    _ClassView = (OOAdvantech.RDBMSMetaDataRepository.View)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_MainTable))
            {
                if (value == null)
                    _MainTable = default(OOAdvantech.RDBMSMetaDataRepository.Table);
                else
                    _MainTable = (OOAdvantech.RDBMSMetaDataRepository.Table)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_AutoGenarated))
                return _AutoGenarated;

            if (member.Name == nameof(_AllObjectsInActiveMode))
                return _AllObjectsInActiveMode;

            if (member.Name == nameof(ObjectsInActiveMode))
                return ObjectsInActiveMode;

            if (member.Name == nameof(_ObjectIdentityType))
                return _ObjectIdentityType;

            if (member.Name == nameof(_StorageIdentity))
                return _StorageIdentity;

            if (member.Name == nameof(_DeleteStoreProcedure))
                return _DeleteStoreProcedure;

            if (member.Name == nameof(_UpdateStoreProcedure))
                return _UpdateStoreProcedure;

            if (member.Name == nameof(_NewStoreProcedure))
                return _NewStoreProcedure;

            if (member.Name == nameof(_ObjectsCount))
                return _ObjectsCount;

            if (member.Name == nameof(_TimePeriodEnd))
                return _TimePeriodEnd;

            if (member.Name == nameof(_TimePeriodStart))
                return _TimePeriodStart;

            if (member.Name == nameof(_SerialNumber))
                return _SerialNumber;

            if (member.Name == nameof(_MappedTables))
                return _MappedTables;

            if (member.Name == nameof(_ClassView))
                return _ClassView;

            if (member.Name == nameof(_MainTable))
                return _MainTable;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        bool _AutoGenarated;
        /// <MetaDataID>{881d2517-9dbc-49d9-8f00-7f4a54dee69f}</MetaDataID>
        [MetaDataRepository.PersistentMember("_AutoGenarated")]
        [MetaDataRepository.BackwardCompatibilityID("+64")]
        public bool AutoGenarated
        {
            get
            {
                return _AutoGenarated;
            }
            set
            {
                if (_AutoGenarated != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AutoGenarated = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }
        ///// <exclude>Excluded</exclude>
        //bool _PreloadObjects;
        ///// <MetaDataID>{b6d70064-f512-4a33-80d2-f4081218e2a6}</MetaDataID>
        //[PersistentMember("_PreloadObjects")]
        //[BackwardCompatibilityID("+63")]
        //public override bool PreloadObjects
        //{
        //    get
        //    {
        //        return _PreloadObjects;
        //    }
        //    set
        //    {
        //        if (_PreloadObjects != value)
        //        {

        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _PreloadObjects = value;
        //                stateTransition.Consistent = true;
        //            }

        //        }
        //    }
        //}
        /// <MetaDataID>{77d27353-3d89-4779-b4cd-ccb676f837ed}</MetaDataID>
        bool _AllObjectsInActiveMode;
        /// <MetaDataID>{de0a3378-ad4b-48d3-aade-19a551a72bc6}</MetaDataID>
        public override bool AllObjectsInActiveMode
        {
            get
            {
                return _AllObjectsInActiveMode;
            }
        }
        /// <MetaDataID>{439fabee-949e-4ef4-b4ad-9ceccfc39388}</MetaDataID>
        System.Collections.Generic.List<object> ObjectsInActiveMode = new System.Collections.Generic.List<object>();
        /// <MetaDataID>{be5d1e93-ef81-4536-9fec-e4330bfc541f}</MetaDataID>
        public override void ActivateAllObjects()
        {
            if (!_AllObjectsInActiveMode)
            {
                PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.OpenStorage((Namespace as Storage).StorageName, (Namespace as Storage).StorageLocation, (Namespace as Storage).StorageType);
                Collections.StructureSet aStructureSet = objectStorage.Execute(string.Format("SELECT _object FROM {0} _object", Type.FullName));
                foreach (Collections.StructureSet rowset in aStructureSet)
                    ObjectsInActiveMode.Add(rowset["_object"]);
                _AllObjectsInActiveMode = true;
            }
        }



        /// <MetaDataID>{5bddd141-b989-4216-8875-d8f2513fbcd6}</MetaDataID>
        ObjectIdentityType _ObjectIdentityType;

        /// <MetaDataID>{5e00b683-84ab-4888-ac43-ebc5772db204}</MetaDataID>
        public override ObjectIdentityType ObjectIdentityType
        {
            get
            {
                if (_ObjectIdentityType == null)
                {
                    System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                    foreach (IdentityColumn column in MainTable.ObjectIDColumns)
                        parts.Add(column);
                    _ObjectIdentityType = new MetaDataRepository.ObjectIdentityType(parts);
                }
                return _ObjectIdentityType;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ObjectIdentityType = value;
                    stateTransition.Consistent = true;
                }
            }
        }
        //        /// <exclude>Excluded</exclude>
        //MetaDataRepository.IObjectIdentityType _ObjectIdentityType;
        //public MetaDataRepository.IObjectIdentityType ObjectIdentityType
        //{
        //    get
        //    {
        //        if(_ObjectIdentityType==null)
        //        {
        //            System.Collections.Generic.List<MetaDataRepository.IIdentityPart> identityParts=new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.IIdentityPart>();
        //            foreach(IdentityColumn identityColumn in MainTable.ObjectIDColumns)
        //            {

        //            }
        //            _ObjectIdentityType=new MetaDataRepository.ObjectIdentityType();
        //            _ObjectIdentityType.Parts



        //    }
        //}


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{844B0503-6842-4FDB-96AA-A47CBE5BFE27}</MetaDataID>
        private string _StorageIdentity;
        /// <MetaDataID>{54C891BA-0D5B-46BF-A697-E2FCEDD3E2B6}</MetaDataID>
        [BackwardCompatibilityID("+62")]
        [PersistentMember(255, "_StorageIdentity")]
        public override string StorageIdentity
        {
            get
            {
                if (_StorageIdentity == null || _StorageIdentity.Trim().Length == 0)
                    return (Namespace as Storage).StorageIdentity;
                return _StorageIdentity;
            }
        }


        /// <MetaDataID>{B713ED70-27C5-4CCC-A6AD-956831161C35}</MetaDataID>
        [BackwardCompatibilityID("+61")]
        [PersistentMember("_Type")]
        public override MetaDataRepository.Class Type
        {
            get
            {

                return base.Type;
            }
            set
            {
                base.Type = value;
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



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{79E9482D-7826-4FC2-A54B-55C8C08F79BF}</MetaDataID>
        private StoreProcedure _DeleteStoreProcedure;
        /// <summary>DeleteStoreProcedure property defines a store procedure with parameters the object identity.
        /// This store procedure deletes a storage instance from storage. 
        /// Encapsulate the complexity of mapping of class on one or more tables. </summary>
        /// <MetaDataID>{8DAEE326-F6EA-4D0E-B906-610F7D6A3275}</MetaDataID>
        [Association("DeleteStoreProcedure", typeof(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure), Roles.RoleB, "{645C79FC-A5FF-483c-9B5A-E248E9F2BE26}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_DeleteStoreProcedure")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public StoreProcedure DeleteStoreProcedure
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _DeleteStoreProcedure;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

#if!DeviceDotNet
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9CE7F581-57CC-4447-9EB2-46DF0801B5AB}</MetaDataID>
        private StoreProcedure _UpdateStoreProcedure;
        /// <summary>UpdateStoreProcedure property defines a store procedure with parameter the identity 
        /// object and a number of parametrs with the state of object.
        /// This store procedure update the state of storage instance. 
        /// Encapsulate the complexity of mapping of class on one or more tables. </summary>
        /// <MetaDataID>{7EB41EB9-D1D5-4D13-8E4C-DE73446E15BB}</MetaDataID>
        [Association("UpdateStoreProcedure", typeof(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure), Roles.RoleB, "{25713F27-E517-4A2B-B21E-1079192D784E}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_UpdateStoreProcedure")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public StoreProcedure UpdateStoreProcedure
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _UpdateStoreProcedure;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{A4F514CC-1FF2-4935-AA77-7BE4CABB16CB}</MetaDataID>
        private StoreProcedure _NewStoreProcedure;
        /// <summary>NewStoreProcedure property defines a store procedure with parameters which contain the state of object.
        /// This store procedure create a new storage instance and return the object identity if it is produced from storage. 
        /// Encapsulate the complexity of mapping of class on one or more tables. </summary>
        /// <MetaDataID>{58BF2043-04EB-4F21-8DBE-0B5AB773C0A8}</MetaDataID>
        [Association("NewStoreProcedure", typeof(OOAdvantech.RDBMSMetaDataRepository.StoreProcedure), Roles.RoleB, "{9FDD6EFC-CF64-491E-A124-E1CBE7D39700}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_NewStoreProcedure")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public StoreProcedure NewStoreProcedure
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _NewStoreProcedure;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
#else
        object _UpdateStoreProcedure;
        object _NewStoreProcedure;
#endif
        /// <MetaDataID>{F6ADA124-47D3-4EBC-8AF6-DFB7BB35A248}</MetaDataID>
        public override Collections.Generic.List<MetaDataRepository.StorageCell> GetLinkedStorageCells(MetaDataRepository.MetaObjectID associationIdentity, MetaDataRepository.Roles linkedStorageCellsRole)
        {

            foreach (AssociationEnd associationEnd in Type.GetRoles(true))
            {
                if (associationEnd.Association.Identity == associationIdentity)
                {
                    Collections.Generic.List<MetaDataRepository.StorageCell> linkedStorageCells = new OOAdvantech.Collections.Generic.List<MetaDataRepository.StorageCell>();

                    foreach (MetaDataRepository.StorageCell storageCell in (associationEnd.Association as RDBMSMetaDataRepository.Association).GetLinkedStorageCells(this, linkedStorageCellsRole))
                    {
                        if (storageCell is StorageCellReference)
                            linkedStorageCells.Add((storageCell as StorageCellReference).RealStorageCell);
                        else
                            linkedStorageCells.Add(storageCell);
                    }
                    return linkedStorageCells;
                }
            }
            throw new System.Exception("System can't find association");

        }
        /// <summary>This method returns a collection with storage cell links 
        /// where the association end of storage cell has referential integrity attribute. </summary>
        /// <MetaDataID>{B2AEA729-51FD-4F5C-8EBD-54D0E7C76F00}</MetaDataID>
        public Collections.Generic.Set<StorageCellsLink> GetStorageCellsLinksWithRefIntegrity()
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                Collections.Generic.Set<StorageCellsLink> storageCellsLinks = new OOAdvantech.Collections.Generic.Set<StorageCellsLink>();

                //TODO πως λειτουργεί όταν υπάρχει σχέση της class με τον εαυτότης
                foreach (AssociationEnd associationEnd in Type.GetRoles(true))
                {

                    foreach (StorageCellsLink storageCellsLink in (associationEnd.Association as Association).GetStorageCellsLinks(this))
                    {
                        if (storageCellsLink.RoleAStorageCell == this)
                        {
                            if (storageCellsLink.RoleBStorageCell.Type.HasReferentialIntegrity(associationEnd))
                                storageCellsLinks.Add(storageCellsLink);
                        }
                        else
                        {
                            if (storageCellsLink.RoleAStorageCell.Type.HasReferentialIntegrity(associationEnd))
                                storageCellsLinks.Add(storageCellsLink);


                        }
                    }
                }
                return storageCellsLinks;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }
        /// <summary>If any of the association ends of storage cell class has change the  
        /// referential integrity attribute then this property is true and RDBMS run time subsystem must run 
        /// the code for reference count update. </summary>
        /// <MetaDataID>{AAFE2BA7-8705-402B-B5E8-BA8828C0786D}</MetaDataID>
        public bool NeededReferencialIntegrityUpdate
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    //TODO πως λειτουργεί όταν υπάρχει σχέση της class με τον ευτότης
                    foreach (AssociationEnd associationEnd in Type.GetRoles(true))
                    {

                        foreach (StorageCellsLink storageCellsLink in (associationEnd.Association as Association).GetStorageCellsLinks(this))
                        {

                            if (associationEnd.IsRoleA && storageCellsLink.RoleAHasReferencialIntegrity != storageCellsLink.RoleBStorageCell.Type.HasReferentialIntegrity(associationEnd))
                                return true;
                            if (!associationEnd.IsRoleA && storageCellsLink.RoleBHasReferencialIntegrity != storageCellsLink.RoleAStorageCell.Type.HasReferentialIntegrity(associationEnd))
                                return true;
                        }
                    }
                    return false;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <summary>Produce the identity </summary>
        /// <MetaDataID>{90DE1293-EEC0-4E57-A10D-85F97373ABD8}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {

                lock (identityLock)

                {
                    if (_Identity == null)
                        _Identity = new MetaDataRepository.MetaObjectID((_Namespace.Value as PersistenceLayer.Storage).StorageIdentity + "_" + SerialNumber.ToString());
                    return _Identity;
                }

            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{EF094CD1-7D61-4BB2-958D-C8585BFADDCA}</MetaDataID>
        private int _ObjectsCount = -1;
        /// <summary>Specify the number of objects that contains the storage cell. 
        /// It is valid only for the history classes. 
        /// It is transient and initialized from the the storage provider 
        /// when the storage opening at first time. 
        /// Before this time has value -1. </summary>
        /// <MetaDataID>{13CB27AF-8562-4D76-9B00-4075A1ECD379}</MetaDataID>
        public int ObjectsCount
        {
            get
            {
                return _ObjectsCount;
            }
            set
            {
                if (_ObjectsCount != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ObjectsCount = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        private System.DateTime _TimePeriodEnd;
        /// <summary>This property defines the end time of storage cell time period. 
        /// All object of storage cell created before this time. </summary>
        /// <MetaDataID>{301679CD-1B8B-40BC-8327-0BAAFA9047F2}</MetaDataID>
        [BackwardCompatibilityID("+40")]
        [PersistentMember("_TimePeriodEnd")]
        public System.DateTime TimePeriodEnd
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _TimePeriodEnd;
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
                    if (_TimePeriodEnd != value)
                    {
                        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        {
                            _TimePeriodEnd = value;
                            StateTransition.Consistent = true; ;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B29CC76F-7FA5-4C31-8752-6EA0467CA8EB}</MetaDataID>
        private System.DateTime _TimePeriodStart;
        /// <summary>This property defines the start time of storage cell time period. 
        /// All object of storage cell created after this time. </summary>
        /// <MetaDataID>{33C73644-859B-4EBB-B85B-51310F114DFD}</MetaDataID>
        [BackwardCompatibilityID("+39")]
        [PersistentMember("_TimePeriodStart")]
        public System.DateTime TimePeriodStart
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _TimePeriodStart;
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
                    if (_TimePeriodStart != value)
                    {
                        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        {
                            _TimePeriodStart = value;
                            StateTransition.Consistent = true;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <exclude>Excluded</exclude>

        private int _SerialNumber = -1;

        /// <summary>This property defines a unique number for storage cell in storage space. </summary>
        /// <MetaDataID>{7CE4C4DA-81C9-4EA4-9C78-E7BBA82755C9}</MetaDataID>
        [BackwardCompatibilityID("+53")]
        [PersistentMember("_SerialNumber")]
        public override int SerialNumber
        {
            get
            {
                return _SerialNumber;
            }
            set
            {
            }
        }
        /// <MetaDataID>{5F33226F-3FE2-4B27-9556-C0D16C94469B}</MetaDataID>
        protected StorageCell()
        {

        }
        /// <summary>This method defines the storage cell constructor. 
        /// Initialize the new object with the type and storage. </summary>
        /// <param name="type">This parameter defines the type of object which stored in storage cell. </param>
        /// <param name="storage">This parameter defines the meta data of storage which contains the new storage cell. </param>
        /// <MetaDataID>{8BF47725-9E8E-4EA5-B843-4930C9F8DC1A}</MetaDataID>
        public StorageCell(Class type, PersistenceLayer.Storage storage)
        {

            _Type = type;
            TimePeriodStart = System.DateTime.Now;
            ObjectsCount = 0;
            if (type.StorageCells.Count > 0)
                Name = type.CaseInsensitiveName + type.StorageCells.Count.ToString();
            else
                Name = type.CaseInsensitiveName;

            (storage as Namespace).AddOwnedElement(this);
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{0D272F3C-BAF3-4192-B85A-6FC657A35035}</MetaDataID>
        private Collections.Generic.Set<Table> _MappedTables = new OOAdvantech.Collections.Generic.Set<Table>();
        /// <summary>This property defines a collection with the tables which 
        /// use the storage cell to store the objects. 
        /// Remember one class can be mapped to one or more tables. </summary>
        /// <MetaDataID>{B942E1FA-FF50-4ECE-94A7-15598AAA6F9A}</MetaDataID>
        [Association("ClassTablesMapping", typeof(OOAdvantech.RDBMSMetaDataRepository.Table), Roles.RoleB, "{D6015F0F-1ED7-4714-B79B-F8F1ED042F90}")]
        [PersistentMember("_MappedTables")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1)]
        public Collections.Generic.Set<Table> MappedTables
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<Table>(_MappedTables, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{84534419-15FE-4594-9B82-CE7F39824EDB}</MetaDataID>
        private View _ClassView;
        /// <summary>Class view is a view which returns a record set, 
        /// where each row is the state of an instance of type of storage cell. 
        /// It is useful in case where the class mapped to more than table. </summary>
        /// <MetaDataID>{6BD3ECD9-8700-463A-8208-858DC377B60F}</MetaDataID>
        [Association("StorageCellInstancesOfClass", typeof(OOAdvantech.RDBMSMetaDataRepository.View), Roles.RoleA, "{C29F8127-052C-495F-BC85-48A18962FB94}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_ClassView")]
        [RoleAMultiplicityRange(1, 1)]
        public virtual View ClassView
        {
            get
            {

                return _ClassView;
            }
            set
            {
                _ClassView = value;
            }
        }



        /// <MetaDataID>{14015394-B0C5-413A-9DAC-6570E824B606}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private Table _MainTable;
        /// <summary>This property defines the main table of storage cell. </summary>
        /// <remarks>
        /// In case where the class mapped to more than one table then 
        /// there is a table which has the object identity and the part of state 
        /// which correspond to the class of storage cell, the main table  
        /// and other tables which have the others parts of state which correspond to 
        /// the parent classes of storage cell class (one table per class mapping style). 
        /// </remarks>
        /// <MetaDataID>{7FEDD034-8672-4FD3-BCEE-C8D7565FB39E}</MetaDataID>
        [Association("ProducetheIDs", typeof(OOAdvantech.RDBMSMetaDataRepository.Table), Roles.RoleA, "{F3F2A93A-D21F-4589-AC99-7CEAE6CDC7DB}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_MainTable")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1)]
        public Table MainTable
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_MainTable == null)
                        BuildMainTable();
                    return _MainTable;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
            set
            {
                // if (_MainTable == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (_MainTable != value && _MainTable != null)
                            _MappedTables.Remove(_MainTable);
                        _MainTable = value;
                        if (_MainTable != null)
                            _MappedTables.Add(_MainTable);
                        stateTransition.Consistent = true;
                    }
                }

            }

        }


        /// <summary>This method adds a mapped table to storage cell. 
        /// The mapped table belongs to parent class of the storage cell class. 
        /// The inheritance relationship with the parent classes declared as OneTablePerClass. </summary>
        /// <MetaDataID>{B185E8E7-7A38-4A51-9C7B-E69B706C44B4}</MetaDataID>
        internal void AddMappedTable(Table NewTable)
        {
            _MappedTables.Add(NewTable);
            if (_ClassView != null)
                _ClassView.AddJoinedTable(NewTable);
        }

        /// <summary>This method create - initalize the main table if the storage cell just created.
        /// Or check and update the mainTable </summary>
        /// <MetaDataID>{123BD766-A554-4397-B239-80E1E296903F}</MetaDataID>
        private void BuildMainTable()
        {
            bool hasOwnedTable = false;


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                #region Check the inheritance relationsips to find if has its owned table

                if (Type.Generalizations.Count == 0)
                    hasOwnedTable = true;

                foreach (Generalization CurrGeneralization in Type.Generalizations)
                {

                    if (CurrGeneralization.GeneralizationMappingType == GeneralizationMappingType.OneTablePerConcreteClass ||
                        CurrGeneralization.GeneralizationMappingType == GeneralizationMappingType.OneTablePerClass ||
                        CurrGeneralization.GeneralizationMappingType == GeneralizationMappingType.NotItialized)
                    {
                        hasOwnedTable = true;
                        break;
                    }
                }
                #endregion

                if (hasOwnedTable)
                {
                    #region Create main table if it isn't exist

                    if (_MainTable != null && _MainTable.TableCreator != this)
                    {
                        //Για να έρθει το συστημα σε αυτή την κατάσταση θα πρέπει να έχει διαγραφεί
                        //κάποια generalization σχέση ή να αλλάξει το mapping type. Θα πρέπει να προφιλάση το συστημα από αυτή την κατάσταση
                        //η Synchronize method της RDBMSMetaDataRepository.Class
                        throw new System.Exception("It can't be change the class hierarchy");
                    }
                    if (_MainTable == null)
                    {

                        _MainTable = new Table((Namespace as Storage).TablePrefix + this.Type.Name, this);
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_MainTable);
                        Namespace.MakeNameUnaryInNamesapce(_MainTable);
                        _MappedTables.Add(_MainTable);
                    }
                    #endregion
                }
                else
                {
                    #region Search for inheritance with OneTablePerHierarchy and set main table with the main table of parent class
                    foreach (Generalization CurrGeneralization in Type.Generalizations)
                    {
                        if (CurrGeneralization.GeneralizationMappingType == GeneralizationMappingType.OneTablePerHierarchy)
                        {

                            if (_MainTable != null && _MainTable != ((Class)CurrGeneralization.Parent).ActiveStorageCell.MainTable)
                            {
                                //Για να έρθει το συστημα σε αυτή την κατάσταση θα πρέπει να έχει αλλάξη
                                //κάποια generalization σχέση. Θα πρέπει να προφιλάση το συστημα από αυτή την κατάσταση
                                //η Synchronize method της RDBMSMetaDataRepository.Class
                                throw new System.Exception("It can't be change the class hierarchy");
                            }
                            _MainTable = ((Class)CurrGeneralization.Parent).ActiveStorageCell.MainTable;
                            break;
                        }
                    }
                    #endregion
                }
                _MainTable.BuildMappingElement();
                stateTransition.Consistent = true;
            }



        }
        /// <summary>This method update columns of main table for the persistent attribute of class, 
        /// the persistent association end, referencial integrity. </summary>
        /// <MetaDataID>{2487ACE7-FFCE-44DC-84AA-6D326277773E}</MetaDataID>
        private void UpdateMainTableColumns()
        {
            if (Type != null && Type.FullName == "FlavourBusinessManager.FoodTypeTag")
            {

            }

            if (Type != null && Type.FullName == "FlavourBusinessManager.ServicesContextResources.ServicePoint")
            {

            }
            if (Type != null && Type.FullName == "FlavourBusinessManager.ServicesContextResources.HallServicePoint")
            {

            }

            

            using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
            {
                bool ClassHierarchyMembers = false;
                #region Check the inheritance mapping type
                foreach (Generalization CurrGeneralization in Type.Generalizations)
                {
                    if (CurrGeneralization.GeneralizationMappingType == GeneralizationMappingType.OneTablePerConcreteClass)
                        ClassHierarchyMembers = true;
                    //TODO: Error Prone γίνεται σε περίπτωση multiply inheritance με την πρώτη OneTablePerConcreteClass και την δεύτερη OneTablePerHierarchy
                }
                #endregion

                #region Update attribute columns

                Collections.Generic.Set<MetaDataRepository.Attribute> attributes = Type.GetAttributes(ClassHierarchyMembers);

                System.Collections.Generic.List<ValuePathAttribute> valueTypePathAttributes = GetValueTypePathPersistentAttributes();

                if (!ClassHierarchyMembers)
                {
                    foreach (Attribute attribute in Type.GetRealizedAttributes())
                    {
                        if ((attribute.Owner is Class) && !(attribute.Owner as Class).Abstract)
                            continue;
                        else
                            attributes.Add(attribute);
                    }

                }
                //var ss = MainTable.ContainedColumns.Where(x => x.MappedAssociationEnd == null && x.MappedAttribute == null && x.IndexerAssociationEnd == null).ToList();
                foreach (Column column in MainTable.ContainedColumns)
                {


                    if (column.MappedAttribute != null)
                    {
                        if ((from valueTypePathAttribute in valueTypePathAttributes
                             where column.MappedAttribute == valueTypePathAttribute.Attribute && column.CreatorIdentity == valueTypePathAttribute.PathIdentity
                             select valueTypePathAttribute).Count() == 0)
                        {
                            MainTable.RemoveColumn(column);
                        }
                    }
                    else
                    {

                        if (!String.IsNullOrWhiteSpace(column.MappedAttributeRealizationIdentity))
                        {
                            var attributeRealization = Type.Features.OfType<AttributeRealization>().Where(x => x.Identity.ToString() == column.MappedAttributeRealizationIdentity).FirstOrDefault();
                            if (attributeRealization != null)
                                column.MappedAttribute = attributeRealization.Specification as Attribute;
                        }
                    }
                    if (column.MappedAssociationEnd == null)
                    {
                        if (!String.IsNullOrWhiteSpace(column.MappedAssociationEndRealizationIdentity))
                        {
                            var associationEndRealization = Type.Features.OfType<AssociationEndRealization>().Where(x => x.Identity.ToString() == column.MappedAssociationEndRealizationIdentity).FirstOrDefault();
                            if (associationEndRealization != null && associationEndRealization.Specification != null)
                                column.MappedAssociationEnd = associationEndRealization.Specification.GetOtherEnd() as AssociationEnd;
                        }
                    }
                    if (column.MappedAttribute != null && !attributes.Contains(column.MappedAttribute) && (Type != column.MappedAttribute.Owner && (column.MappedAttribute.Owner == null || !Type.IsA(column.MappedAttribute.Owner))))
                    {
                        //MainTable.RemoveColumn(column);
                    }
                    if (column.MappedAssociationEnd == null && column.MappedAttribute == null && column.IndexerAssociationEnd == null)
                    {
                        bool isAuxColumn = false;
                        foreach (var auxColumn in AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumns(this))
                        {
                            if (auxColumn.Type.FullName == column?.Type.FullName && auxColumn.Name == column.Name)
                            {
                                isAuxColumn = true;
                                break;
                            }
                        }
                        if (!isAuxColumn)
                            MainTable.RemoveColumn(column);
                    }
                }



                foreach (Attribute attribute in attributes.Where(x => ((x as RDBMSMetaDataRepository.Attribute).GetColumnsFor(this).Count() != 0)))
                {
                    //update existing columns
                    if (Type.IsPersistent(attribute))
                        attribute.AddColumnToTableOrUpdate(MainTable,Type.IsMultilingual(attribute) );
                }
                foreach (Attribute attribute in attributes.Where(x => ((x as RDBMSMetaDataRepository.Attribute).GetColumnsFor(this).Count() == 0)))
                {
                    //add new columns
                    if (Type.IsPersistent(attribute))
                        attribute.AddColumnToTableOrUpdate(MainTable,Type.IsMultilingual(attribute));
                }



                #endregion

                #region Update association relationship columns

                foreach (AssociationEnd associationEnd in Type.GetRoles(ClassHierarchyMembers))
                {
                    //TODO τι γίνεται στην περίπτωση που έχω realize ένα assotiation and που έρχεται από 
                    //interface και δέν έχω one table per concret class 
                    if (associationEnd.Association.HasPersistentObjectLink)
                        associationEnd.AddReferenceColumnsToTable(MainTable);
                }
                if (Type.ClassHierarchyLinkAssociation != null)
                {
                    (Type.ClassHierarchyLinkAssociation.RoleA as AssociationEnd).AddReferenceColumnsToTable(MainTable);
                    (Type.ClassHierarchyLinkAssociation.RoleB as AssociationEnd).AddReferenceColumnsToTable(MainTable);
                }
                #endregion

                #region Update referential integrity column
                if (Type.HasReferentialIntegrityRelations())
                {
                    if (MainTable.ReferentialIntegrityColumn == null)
                    {
                        MetaDataRepository.Primitive SystemInt32Type = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.Int32", this) as MetaDataRepository.Primitive;
                        if (SystemInt32Type == null || SystemInt32Type.Name == null)
                        {
                            //MetaDataRepository.Namespace mNamespace = new MetaDataRepository.Namespace();
                            //mNamespace.Name = "System";
                            //MetaDataRepository.Primitive mPrimitive = new MetaDataRepository.Primitive();
                            //mPrimitive.Name = "Int32";
                            //mNamespace.AddOwnedElement(mPrimitive);
                            MetaDataRepository.Primitive mPrimitive = MetaDataRepository.Classifier.GetClassifier(typeof(int)) as MetaDataRepository.Primitive;
                            SystemInt32Type = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(mPrimitive, this) as Primitive;
                            if (SystemInt32Type == null)
                                SystemInt32Type = (Primitive)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mPrimitive, this);
                            SystemInt32Type.Synchronize(mPrimitive);
                        }
                        Column referentialIntegrityColumn = new Column("ReferenceCount", SystemInt32Type, 0, true, false, 1);
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(referentialIntegrityColumn);
                        MainTable.ReferentialIntegrityColumn = referentialIntegrityColumn;
                        MainTable.AddOwnedElement(MainTable.ReferentialIntegrityColumn);
                    }
                }
                else
                {
                    if (MainTable.ReferentialIntegrityColumn != null)
                        MainTable.RemoveOwnedElement(MainTable.ReferentialIntegrityColumn);
                    MainTable.ReferentialIntegrityColumn = null;
                }
                #endregion
                StateTransition.Consistent = true; ;
            }
        }
        private List<ValuePathAttribute> GetValueTypePathPersistentAttributes(MetaDataRepository.Structure type, ValueTypePath pathIdentity)
        {
            System.Collections.Generic.List<ValuePathAttribute> attributes = new System.Collections.Generic.List<ValuePathAttribute>();
            foreach (MetaDataRepository.Attribute attribute in type.GetAttributes(true))
            {
                if (type.IsPersistent(attribute))
                {
                    if (attribute.Type is MetaDataRepository.Structure && (attribute.Type as MetaDataRepository.Structure).Persistent)
                    {

                        pathIdentity.Push(attribute.Identity);

                        attributes.AddRange(GetValueTypePathPersistentAttributes((attribute.Type as MetaDataRepository.Structure), pathIdentity));
                        pathIdentity.Pop();
                    }
                    else
                    {
                        pathIdentity.Push(attribute.Identity);

                        attributes.Add(new ValuePathAttribute() { PathIdentity = pathIdentity.ToString(), Attribute = attribute as Attribute });
                        pathIdentity.Pop();
                    }
                }
            }
            return attributes;
        }
        private List<ValuePathAttribute> GetValueTypePathPersistentAttributes()
        {
            System.Collections.Generic.List<ValuePathAttribute> attributes = new System.Collections.Generic.List<ValuePathAttribute>();
            foreach (MetaDataRepository.Attribute attribute in Type.GetAttributes(true))
            {
                if (Type.IsPersistent(attribute))
                {
                    if (attribute.Type is MetaDataRepository.Structure && (attribute.Type as MetaDataRepository.Structure).Persistent)
                    {
                        ValueTypePath pathIdentity = new ValueTypePath();
                        pathIdentity.Push(attribute.Identity);

                        attributes.AddRange(GetValueTypePathPersistentAttributes((attribute.Type as MetaDataRepository.Structure), pathIdentity));
                    }
                    else
                        attributes.Add(new ValuePathAttribute() { PathIdentity = "", Attribute = attribute as Attribute });
                }
            }
            return attributes;
        }
#if !DeviceDotNet
        /// <MetaDataID>{9DED76F3-CB32-4F94-9C89-CA26196989DF}</MetaDataID>
        void BuildUpdateStoreProcedure()
        {

            #region Create update store procedure object if doesn't exist
            if (_UpdateStoreProcedure == null)
            {
                _UpdateStoreProcedure = new StoreProcedure(this, StoreProcedure.Types.Update);
                Namespace.AddOwnedElement(_UpdateStoreProcedure);
                PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_UpdateStoreProcedure);
            }
            #endregion

            _UpdateStoreProcedure.Name = "Update_" + Name + "_Instance";
            int Position = 0;
            // Add Standard columns
            #region Sets the parameter for object identity parts and referential Integrity
            foreach (IdentityColumn CurrColumn in _MainTable.ObjectIDColumns)
            {
                MetaDataRepository.Parameter parameter = _UpdateStoreProcedure.SetParameter(CurrColumn.Name, CurrColumn.Type, "", Position);
                parameter.Direction = MetaDataRepository.Parameter.DirectionType.In;
                parameter.PutPropertyValue("Persistent", "SizeOf", CurrColumn.Length);
                Position++;
            }

            if (_MainTable.ReferentialIntegrityColumn != null)
            {
                MetaDataRepository.Parameter parameter = _UpdateStoreProcedure.SetParameter(_MainTable.ReferentialIntegrityColumn.Name, _MainTable.ReferentialIntegrityColumn.Type, "", Position);
                parameter.Direction = MetaDataRepository.Parameter.DirectionType.In;
                parameter.PutPropertyValue("Persistent", "SizeOf", 0);
                Position++;
            }
            #endregion

            // Add all persistent fields
            #region Sets parameters for the persistent attributes
            //foreach (MetaDataRepository.Attribute CurrAttribute in Type.GetAttributes(true))
            //{
            //    if (Type.IsPersistent(CurrAttribute))
            //    {
            System.Collections.Generic.List<string> parametersNames = new System.Collections.Generic.List<string>();
            foreach (MetaDataRepository.Attribute attribute in GetPersistentAttribute())
            {
                //string parameterName = attribute.CaseInsensitiveName;

                foreach (Column column in (attribute as Attribute).GetColumnsFor(this))
                {
                    string parameterName = column.Name;

                    if (parametersNames.Contains(parameterName))
                        continue;
                    parametersNames.Add(parameterName);
                    MetaDataRepository.Parameter parameter = _UpdateStoreProcedure.SetParameter(parameterName, attribute.Type, "", Position);
                    parameter.Direction = MetaDataRepository.Parameter.DirectionType.In;
                    MetaDataRepository.AttributeRealization attributeRealization = Type.GetAttributeRealization(attribute);
                    //TODO όταν σε ένα string type δεν δηλωθεί length τότε θα έπρεπέ και εδώ να μπένει default;
                    //Τι πρέπει να δηλωθεί στο string για να βγεί BLOB - TEXT
                    if (attributeRealization != null)
                        parameter.PutPropertyValue("Persistent", "SizeOf", attributeRealization.GetPropertyValue(typeof(int), "Persistent", "SizeOf"));
                    else
                        parameter.PutPropertyValue("Persistent", "SizeOf", attribute.GetPropertyValue(typeof(int), "Persistent", "SizeOf"));
                    parameter.PutPropertyValue("Persistent", "SizeOf", 0);
                    Position++;
                }
            }
            //    }
            //}
            #endregion

            #region Deletes the parameter at following posistions
            MetaDataRepository.Parameter Parameter = _UpdateStoreProcedure.GetParameterAt(Position);
            while (Parameter != null)
            {
                _UpdateStoreProcedure.DeleteParameter(Parameter);
                Position++;
                Parameter = _UpdateStoreProcedure.GetParameterAt(Position);
            }
            #endregion

        }
#endif
        /// <MetaDataID>{8f40d01e-611d-4af8-8b17-aaac953367cd}</MetaDataID>
        System.Collections.Generic.List<Attribute> GetPersistentAttribute()
        {
            System.Collections.Generic.List<Attribute> attributes = new System.Collections.Generic.List<Attribute>();
            foreach (MetaDataRepository.Attribute attribute in Type.GetAttributes(true))
            {
                if (Type.IsPersistent(attribute))
                {
                    if (attribute.Type is MetaDataRepository.Structure && (attribute.Type as MetaDataRepository.Structure).Persistent)
                        attributes.AddRange(GetPersistentAttribute(attribute.Type as MetaDataRepository.Structure));
                    else
                        attributes.Add(attribute as Attribute);
                }
            }
            return attributes;
        }
        /// <MetaDataID>{9ee926bd-3ef0-417f-b7e0-369b155b3c4d}</MetaDataID>
        System.Collections.Generic.List<Attribute> GetPersistentAttribute(MetaDataRepository.Structure structure)
        {
            System.Collections.Generic.List<Attribute> columns = new System.Collections.Generic.List<Attribute>();
            foreach (MetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (Type.IsPersistent(attribute))
                {
                    if (attribute.Type is MetaDataRepository.Structure && (attribute.Type as MetaDataRepository.Structure).Persistent)
                        columns.AddRange(GetPersistentAttribute(attribute.Type as MetaDataRepository.Structure));
                    else
                        columns.Add(attribute as Attribute);
                }
            }
            return columns;
        }

        /// <MetaDataID>{64c9e74d-ef77-4fef-a640-77f095f7bf4b}</MetaDataID>
        public System.Collections.Generic.List<MetaDataRepository.AssociationEnd> GetAssociatedRoles()
        {
            System.Collections.Generic.List<MetaDataRepository.AssociationEnd> roles = new System.Collections.Generic.List<MetaDataRepository.AssociationEnd>();
            foreach (MetaDataRepository.AssociationEnd associationEnd in Type.GetAssociateRoles(true))
                roles.Add(associationEnd);


            foreach (MetaDataRepository.Attribute attribute in Type.GetAttributes(true))
            {
                if (Type.IsPersistent(attribute))
                {
                    if (attribute.IsPersistentValueType)
                        roles.AddRange(GetAssociatedRoles(attribute.Type as MetaDataRepository.Structure));
                }
            }
            return roles;
        }
        /// <MetaDataID>{a3961690-48c3-4f68-bf96-86bccf7027e4}</MetaDataID>
        System.Collections.Generic.List<MetaDataRepository.AssociationEnd> GetAssociatedRoles(MetaDataRepository.Structure structure)
        {
            System.Collections.Generic.List<MetaDataRepository.AssociationEnd> roles = new System.Collections.Generic.List<MetaDataRepository.AssociationEnd>();
            foreach (MetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
                roles.Add(associationEnd);

            foreach (MetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (Type.IsPersistent(attribute))
                {
                    if (attribute.IsPersistentValueType)
                        roles.AddRange(GetAssociatedRoles(attribute.Type as MetaDataRepository.Structure));
                }
            }
            return roles;
        }
#if!DeviceDotNet
        /// <MetaDataID>{F85134E3-4944-4485-89A6-1128DFA0AE0A}</MetaDataID>
        void BuildNewStoreProcedure()
        {

            #region Creates the new store procedure
            if (_NewStoreProcedure == null)
            {
                _NewStoreProcedure = new StoreProcedure(this, StoreProcedure.Types.New);
                PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_NewStoreProcedure);
                Namespace.AddOwnedElement(_NewStoreProcedure);
            }
            #endregion

            _NewStoreProcedure.Name = "New_" + Name + "_Instance";

            int Position = 0;
            #region Adds object identity parameters
            foreach (IdentityColumn CurrColumn in _MainTable.ObjectIDColumns)
            {
                if (!CurrColumn.ProducedFromRDBMS)
                {
                    MetaDataRepository.Parameter parameter = _NewStoreProcedure.SetParameter(CurrColumn.Name, CurrColumn.Type, "", Position);
                    parameter.Direction = MetaDataRepository.Parameter.DirectionType.In;
                    parameter.PutPropertyValue("Persistent", "SizeOf", CurrColumn.Length);
                    Position++;
                }
            }
            #endregion

            #region Adds ReferentialIntegrity parameter
            if (_MainTable.ReferentialIntegrityColumn != null)
            {
                MetaDataRepository.Parameter parameter = _NewStoreProcedure.SetParameter(_MainTable.ReferentialIntegrityColumn.Name, _MainTable.ReferentialIntegrityColumn.Type, "", Position);
                parameter.Direction = MetaDataRepository.Parameter.DirectionType.In;
                parameter.PutPropertyValue("Persistent", "SizeOf", 0);
                Position++;
            }
            #endregion

            #region Add all persistent fields  parameters
            //foreach(MetaDataRepository.Attribute CurrAttribute in Type.GetAttributes(true))
            //{
            //    if(Type.IsPersistent(CurrAttribute))
            //    {
            System.Collections.Generic.List<string> parametersNames = new System.Collections.Generic.List<string>();
            foreach (MetaDataRepository.Attribute attribute in GetPersistentAttribute())
            {

                //Column column = (CurrAttribute as Attribute).GetColumnFor(this);

                //string parameterName = attribute.CaseInsensitiveName;
                foreach (Column column in (attribute as Attribute).GetColumnsFor(this))
                {
                    string parameterName = column.Name;

                    if (parametersNames.Contains(parameterName))
                        continue;

                    parametersNames.Add(parameterName);

                    MetaDataRepository.Parameter parameter = _NewStoreProcedure.SetParameter(parameterName, attribute.Type, "", Position);
                    parameter.Direction = MetaDataRepository.Parameter.DirectionType.In;
                    MetaDataRepository.AttributeRealization attributeRealization = Type.GetAttributeRealization(attribute);
                    //TODO όταν σε ένα string type δεν δηλωθεί length τότε θα έπρεπέ και εδώ να μπένει default;
                    //Τι πρέπει να δηλωθεί στο string για να βγεί BLOB - TEXT
                    if (attributeRealization != null)
                        parameter.PutPropertyValue("Persistent", "SizeOf", attributeRealization.GetPropertyValue(typeof(int), "Persistent", "SizeOf"));
                    else
                        parameter.PutPropertyValue("Persistent", "SizeOf", attribute.GetPropertyValue(typeof(int), "Persistent", "SizeOf"));
                    parameter.PutPropertyValue("Persistent", "SizeOf", 0);
                    Position++;
                }
            }
            //    }
            //}
            #endregion

            #region Add object identity parameters  as return value
            foreach (IdentityColumn CurrColumn in _MainTable.ObjectIDColumns)
            {
                if (CurrColumn.ProducedFromRDBMS)
                {
                    MetaDataRepository.Parameter parameter = _NewStoreProcedure.SetParameter(CurrColumn.Name, CurrColumn.Type, "", Position);
                    parameter.Direction = MetaDataRepository.Parameter.DirectionType.Out;
                    parameter.PutPropertyValue("Persistent", "SizeOf", CurrColumn.Length);
                    Position++;
                }
            }
            #endregion

            #region Remove the unused paramters

            MetaDataRepository.Parameter parameterForDeletion = _NewStoreProcedure.GetParameterAt(Position);
            while (parameterForDeletion != null)
            {
                _NewStoreProcedure.DeleteParameter(parameterForDeletion);
                Position++;
                parameterForDeletion = _NewStoreProcedure.GetParameterAt(Position);
            }
            #endregion
        }
        /// <MetaDataID>{37FC9C6D-45D5-42D2-8A9D-6CC2CAFA493E}</MetaDataID>
        void BuildDeleteStoreProcedure()
        {

            #region Creates delete store procudure
            if (_DeleteStoreProcedure == null)
            {
                _DeleteStoreProcedure = new StoreProcedure(this, StoreProcedure.Types.Delete);
                PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_DeleteStoreProcedure);
                Namespace.AddOwnedElement(_DeleteStoreProcedure);
            }
            #endregion

            _DeleteStoreProcedure.Name = "Delete_" + Name + "_Instance";

            #region Adds Object identity columns as parameter
            int Position = 0;
            foreach (IdentityColumn CurrColumn in _MainTable.ObjectIDColumns)
            {
                MetaDataRepository.Parameter parameter = _DeleteStoreProcedure.SetParameter(CurrColumn.Name, CurrColumn.Type, "", Position);
                parameter.Direction = MetaDataRepository.Parameter.DirectionType.In;
                parameter.PutPropertyValue("Persistent", "SizeOf", CurrColumn.Length);
                Position++;
            }
            #endregion

            #region Remove the unused paramters
            MetaDataRepository.Parameter Parameter = _DeleteStoreProcedure.GetParameterAt(Position);
            while (Parameter != null)
            {
                _NewStoreProcedure.DeleteParameter(Parameter);
                Position++;
                Parameter = _DeleteStoreProcedure.GetParameterAt(Position);
            }
            #endregion

        }

#endif

        /// <MetaDataID>{758B720B-A36C-4432-AE5B-8BA70C97834E}</MetaDataID>
        private void BuildClassView()
        {
            if (_ClassView == null) //Error prone δεν μπο΄ρεί να γίνει ενημέρωση με τις καινούριες αλλαγές
            {
                #region Create class view and adds source tables
                _ClassView = new View("SC_" + Name, Namespace);
                PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_ClassView);
                foreach (Table CurrTable in MappedTables)
                    _ClassView.AddJoinedTable(CurrTable);
                _ClassView.AddJoinedTable(MainTable);
                _ClassView.StorageCell = this;
                #endregion

                #region Add object identity columns
                foreach (Column CurrColumn in MainTable.ObjectIDColumns)
                {
                    Column NewColumnAlias = new Column(CurrColumn);
                    NewColumnAlias.Name = CurrColumn.Name;
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                    _ClassView.AddColumn(NewColumnAlias);
                }
                #endregion

                #region add storage cell identity and type identity columns
                Column storageCellColumn = null;
                foreach (Column column in _ClassView.ViewColumns)
                {
                    if (column.RealColumn != null && column.RealColumn is IdentityColumn &&
                        (column.RealColumn as IdentityColumn).ColumnType == "StorageCellID")
                    {
                        storageCellColumn = column;
                        break;
                    }
                }
                //The high low object identity mechanism contain storage cell identity column 
                //in object identity columns 
                MetaDataRepository.Primitive SystemInt32Type = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace("System.Int32", this) as MetaDataRepository.Primitive;

                if (storageCellColumn == null)
                {
                    if (SystemInt32Type == null || SystemInt32Type.Name == null)
                    {
                        //MetaDataRepository.Namespace mNamespace = new MetaDataRepository.Namespace();
                        //mNamespace.Name = "System";
                        //MetaDataRepository.Primitive mPrimitive = new MetaDataRepository.Primitive();
                        //mPrimitive.Name = "Int32";
                        //mNamespace.AddOwnedElement(mPrimitive);
                        MetaDataRepository.Primitive mPrimitive = MetaDataRepository.Classifier.GetClassifier(typeof(int)) as MetaDataRepository.Primitive;
                        SystemInt32Type = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(mPrimitive, this) as Primitive;
                        if (SystemInt32Type == null)
                            SystemInt32Type = (Primitive)MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mPrimitive, this);
                        SystemInt32Type.Synchronize(mPrimitive);
                    }
                    storageCellColumn = new Column("StorageCellID", SystemInt32Type);
                    storageCellColumn.DefaultValue = _SerialNumber.ToString();
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(storageCellColumn);
                    _ClassView.AddColumn(storageCellColumn);
                }
                bool TypeIDColumnExist = false;
                foreach (Column CurrColumn in MainTable.ContainedColumns)
                {
                    if (CurrColumn.Name == "TypeID")
                    {
                        Column NewColumnAlias = new Column(CurrColumn);
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                        _ClassView.AddColumn(NewColumnAlias);
                        TypeIDColumnExist = true;
                        break;
                    }
                }
                if (!TypeIDColumnExist)
                {
                    Column TypeIDColumn = new Column("TypeID", SystemInt32Type);
                    TypeIDColumn.DefaultValue = (Type as Class).TypeID.ToString();
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(TypeIDColumn);
                    _ClassView.AddColumn(TypeIDColumn);
                }

                #endregion
            }
            else
                _ClassView.Name = "SC_" + Name;

            System.Collections.Generic.List<Column> oldColumns = new System.Collections.Generic.List<Column>();
            #region Get the class view columns for the class view synchronization.
            foreach (Column column in _ClassView.ViewColumns)
            {
                column.Refresh();
                var oldcolumn = oldColumns.Where(x => x.Name == column.Name).FirstOrDefault();
                if (oldcolumn != null)
                {

                }
                //if(column.MappedAttribute==null&&column.MappedAssociationEnd==null
                //    &&column.MappedAssociationEndRealizationIdentity==null&&column.MappedAttributeRealizationIdentity==null
                //    &&column.RealColumn==null)
                //{
                //    _ClassView.RemoveColumn(column);
                //}
                //if(oldColumns..ContainsKey(column.Name))
                //{ 

                //}
                oldColumns.Add(column);
            }
            #endregion

            //All columns which will contain the view after synchronization, removed form the old columns collection.
            //At the end the code remove the remaining columns of old columns collection from class view

            #region The Object identity and storage cell identity columns always exist in class view.
            foreach (Column CurrColumn in MainTable.ObjectIDColumns)
            {
                var column = oldColumns.Where(x => x.Name == CurrColumn.Name).FirstOrDefault();
                if (column != null)
                    oldColumns.Remove(column);
                //if (oldColumns.ContainsKey(CurrColumn.Name))
                //    oldColumns.Remove(CurrColumn.Name);
            }
            foreach (var column in oldColumns.Where(x => x.Name == "StorageCellID" || x.Name == "TypeID").ToList())
                oldColumns.Remove(column);

            //if (oldColumns.ContainsKey("StorageCellID"))
            //    oldColumns.Remove("StorageCellID");
            //if (oldColumns.ContainsKey("TypeID"))
            //    oldColumns.Remove("TypeID");

            #endregion

            #region Update ReferentialIntegrity column
            if (Type.HasReferentialIntegrityRelations() && MainTable.ReferentialIntegrityColumn != null)
            {
                var oldcolumn = oldColumns.Where(x => x.Name == MainTable.ReferentialIntegrityColumn.Name).FirstOrDefault();
                if (oldcolumn != null)
                    oldColumns.Remove(oldcolumn);
                else
                {
                    Column NewColumnAlias = new Column(MainTable.ReferentialIntegrityColumn);
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                    _ClassView.AddColumn(NewColumnAlias);
                }

                //if (oldColumns.ContainsKey(MainTable.ReferentialIntegrityColumn.Name))
                //    oldColumns.Remove(MainTable.ReferentialIntegrityColumn.Name);
                //else
                //{
                //    Column NewColumnAlias = new Column(MainTable.ReferentialIntegrityColumn);
                //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                //    _ClassView.AddColumn(NewColumnAlias);
                //}
            }
            #endregion

            #region Update the columns for persistent attributes

            System.Collections.Generic.List<Column> assignedColumns = new System.Collections.Generic.List<Column>();
            foreach (Attribute attribute in GetPersistentAttribute())
            {
                //if (!Type.IsPersistent(CurrAttribute))
                //    continue;


                foreach (Column tableColumn in (attribute as Attribute).GetColumnsFor(this))
                {


                    if (assignedColumns.Contains(tableColumn))
                        continue;
                    assignedColumns.Add(tableColumn);

                    var oldcolumn = oldColumns.Where(x => x.Name == tableColumn.Name).FirstOrDefault();
                    if (oldcolumn != null)
                    {
                        oldColumns.Remove(oldcolumn);
                        continue;
                    }
                    else
                    {
                        Column NewColumnAlias = new Column(tableColumn);
                        NewColumnAlias.Name = tableColumn.Name;
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                        _ClassView.AddColumn(NewColumnAlias);
                    }

                    //if (oldColumns.ContainsKey(tableColumn.Name))
                    //{
                    //    oldColumns.Remove(tableColumn.Name);
                    //    continue;
                    //}
                    //else
                    //{
                    //    Column NewColumnAlias = new Column(tableColumn);
                    //    NewColumnAlias.Name = tableColumn.Name;
                    //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                    //    _ClassView.AddColumn(NewColumnAlias);
                    //}
                }
                //}
            }
            #endregion

            #region Update the columns for persistent associations
            Collections.Generic.Set<IdentityColumn> columns = null;
            foreach (MetaDataRepository.AssociationEnd CurrAssociationEnd in GetAssociatedRoles())
            {
                if (!CurrAssociationEnd.Association.HasPersistentObjectLink)
                    continue;

                columns = ((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).GetReferenceColumnsFor(this);
                foreach (Column column in columns)
                {
                    var oldcolumn = oldColumns.Where(x => x.Name == column.Name).FirstOrDefault();
                    if (oldcolumn != null)
                    {
                        oldColumns.Remove(oldcolumn);
                        continue;
                    }
                    else
                    {
                        Column NewColumnAlias = new Column(column);
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                        _ClassView.AddColumn(NewColumnAlias);
                    }

                    //if (oldColumns.ContainsKey(column.Name))
                    //{
                    //    oldColumns.Remove(column.Name);
                    //    continue;
                    //}
                    //else
                    //{
                    //    Column NewColumnAlias = new Column(column);
                    //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                    //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                    //    _ClassView.AddColumn(NewColumnAlias);
                    //}
                }

                if (((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn != null && columns.Count > 0)
                {
                    var indexerColumn = ((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn;
                    var oldcolumn = oldColumns.Where(x => x.Name == indexerColumn.Name).FirstOrDefault();
                    if (oldcolumn != null)
                    {
                        oldColumns.Remove(oldcolumn);
                        continue;
                    }
                    else
                    {
                        indexerColumn = new Column(((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).GetIndexerColumnFor(this, ""));
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(indexerColumn);
                        _ClassView.AddColumn(indexerColumn);

                    }

                    //if (oldColumns.ContainsKey(indexerColumn.Name))
                    //{
                    //    oldColumns.Remove(indexerColumn.Name);
                    //    continue;
                    //}
                    //else
                    //{

                    //    indexerColumn = new Column(((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).GetIndexerColumnFor(this, ""));
                    //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(indexerColumn);
                    //    _ClassView.AddColumn(indexerColumn);
                    //}
                }

            }
            columns = null;
            columns = GetLinkClassColumns(Type as Class);
            if (columns != null)
            {
                foreach (Column column in columns)
                {

                    var oldcolumn = oldColumns.Where(x => x.Name == column.Name).FirstOrDefault();
                    if (oldcolumn != null)
                    {
                        oldColumns.Remove(oldcolumn);
                        continue;
                    }
                    else
                    {
                        Column NewColumnAlias = new Column(column);
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                        NewColumnAlias.Name = column.Name;
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                        _ClassView.AddColumn(NewColumnAlias);
                    }


                    //if (oldColumns.ContainsKey(column.Name))
                    //{
                    //    oldColumns.Remove(column.Name);
                    //    continue;
                    //}
                    //else
                    //{
                    //    Column NewColumnAlias = new Column(column);
                    //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(column);
                    //    NewColumnAlias.Name = column.Name;
                    //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewColumnAlias);
                    //    _ClassView.AddColumn(NewColumnAlias);
                    //}
                }
            }
            #endregion

            #region Removes unused columns form class view
            foreach (var column in oldColumns)
            {
                //Column column = entry.Value as Column;
                _ClassView.RemoveColumn(column);
            }
            #endregion

        }

        /// <MetaDataID>{9C420D2E-4C8F-4C0C-B60E-E51C2F9FA4A5}</MetaDataID>
        Collections.Generic.Set<IdentityColumn> GetLinkClassColumns(Class _class)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                Collections.Generic.Set<IdentityColumn> columns = null;
                if (_class.ClassHierarchyLinkAssociation != null)
                {
                    columns = ((AssociationEnd)_class.ClassHierarchyLinkAssociation.RoleA).GetReferenceColumnsFor(this);
                    if (columns != null)
                        columns.AddRange(((AssociationEnd)_class.ClassHierarchyLinkAssociation.RoleB).GetReferenceColumnsFor(this));
                }

                return columns;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }

        }

        /// <MetaDataID>{dfd43db1-0faa-4046-85ae-7bc5cb5e2cb3}</MetaDataID>
        public override void Synchronize(MetaObject originMetaObject)
        {
            base.Synchronize(originMetaObject);

            if (Type.StorageCells.Count > 0)
                Name = Type.CaseInsensitiveName + Type.StorageCells.Count.ToString();
            else
                Name = Type.CaseInsensitiveName;

           


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (_SerialNumber == -1)
                {
                    if (_SerialNumber == -1)
                    {
                        _SerialNumber = 1;

                        foreach (MetaDataRepository.MetaObject metaObject in (Namespace as Storage).OwnedElements)
                        {
                            if (metaObject is StorageCell && (metaObject as StorageCell)._SerialNumber != -1 && metaObject != this)
                                if ((metaObject as StorageCell)._SerialNumber >= _SerialNumber)
                                    _SerialNumber = (metaObject as StorageCell)._SerialNumber + 1;
                        }
                    }
                }
                if (_MainTable == null)
                {
                    _MainTable = new Table((originMetaObject as StorageCell)._MainTable.Name, this);
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_MainTable);
                    _MappedTables.Add(_MainTable);
                }
                foreach (IdentityColumn column in _MainTable.ObjectIDColumns)
                    _MainTable.RemoveObjectIDColumn(column);

                foreach (IdentityColumn column in (originMetaObject as StorageCell)._MainTable.ObjectIDColumns)
                    _MainTable.AddObjectIDColumn(column.Name, column.Type.GetExtensionMetaObject<System.Type>(), column.Length, column.IsIdentity, column.IdentityIncrement);


                foreach (Column column in (originMetaObject as StorageCell)._MainTable.ContainedColumns)
                {
                    if (column.MappedAttribute != null)
                    {
                        Attribute attribute = GetColumnMappedAttribute(column, Type, new ValueTypePath());
                        Column newColumn = attribute.AddColumnToTableOrUpdate(_MainTable, column.Name,Type.IsMultilingual(attribute));
                        newColumn.IsIdentity = column.IsIdentity;
                        newColumn.IdentityIncrement = column.IdentityIncrement;
                        newColumn.Length = column.Length;
                        newColumn.AllowNulls = column.AllowNulls;
                        newColumn.CreatorIdentity = column.CreatorIdentity;
                      
                    }
                    if (column.MappedAssociationEnd != null)
                    {
                        AssociationEnd associationEnd = GetColumnMappedAssociationEnd(column, Type, new ValueTypePath());
                        IdentityColumn identityColumn = OOAdvantech.RDBMSMetaDataRepository.AutoProduceColumnsGenerator.GetObjectIdentityColumn(column.Name, column.Type, this);
                        identityColumn.Name = column.Name;
                        identityColumn.Length = column.Length;
                        identityColumn.IsIdentity = column.IsIdentity;
                        identityColumn.IdentityIncrement = column.IdentityIncrement;
                        identityColumn.CreatorIdentity = column.CreatorIdentity;
                        identityColumn.ColumnType = (column as IdentityColumn).ColumnType;
                        identityColumn.AllowNulls = column.AllowNulls;
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(identityColumn);
                        if (column.MappedAssociationEnd.IsRoleA)
                            (associationEnd.Association.RoleA as AssociationEnd).AddReferenceColumn(identityColumn);
                        else
                            (associationEnd.Association.RoleB as AssociationEnd).AddReferenceColumn(identityColumn);
                        _MainTable.AddColumn(identityColumn);
                    }

                    if (column.IndexerAssociationEnd != null)
                    {
                        AssociationEnd associationEnd = GetColumnIndexerAssociationEnd(column, Type, new ValueTypePath());
                        Column indexerColumn = new Column();
                        MetaDataRepository.Classifier systemType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(column.Type, this) as MetaDataRepository.Classifier;
                        if (systemType == null)
                        {
                            //System.Diagnostics.Debug.Assert(false);
                            systemType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(column.Type, this) as MetaDataRepository.Classifier;
                            systemType.Synchronize(column.Type);
                        }
                        indexerColumn.Name = column.Name;
                        indexerColumn.Type = systemType;
                        indexerColumn.Length = column.Length;
                        indexerColumn.IsIdentity = column.IsIdentity;
                        indexerColumn.IdentityIncrement = column.IdentityIncrement;
                        indexerColumn.CreatorIdentity = column.CreatorIdentity;
                        indexerColumn.AllowNulls = column.AllowNulls;

                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(indexerColumn);
                        if (column.IndexerAssociationEnd.IsRoleA)
                            (associationEnd.Association.RoleA as AssociationEnd).AddIndexerColumn(indexerColumn);
                        else
                            (associationEnd.Association.RoleB as AssociationEnd).AddIndexerColumn(indexerColumn);
                        _MainTable.AddColumn(indexerColumn);
                    }
                }


#if !DeviceDotNet
                if ((Namespace as RDBMSMetaDataRepository.Storage).SupportStoreProcedures)
                {
                    BuildUpdateStoreProcedure();
                    BuildNewStoreProcedure();
                    BuildDeleteStoreProcedure();
                }
                else
                {
                    if (_UpdateStoreProcedure != null)
                    {
                        PersistenceLayer.ObjectStorage.DeleteObject(_UpdateStoreProcedure);
                        _UpdateStoreProcedure = null;
                    }
                    if (_NewStoreProcedure != null)
                    {
                        PersistenceLayer.ObjectStorage.DeleteObject(_NewStoreProcedure);
                        _NewStoreProcedure = null;
                    }
                    if (_DeleteStoreProcedure != null)
                    {
                        PersistenceLayer.ObjectStorage.DeleteObject(_DeleteStoreProcedure);
                        _DeleteStoreProcedure = null;
                    }
                }

                if (!(Namespace as RDBMSMetaDataRepository.Storage).SupportForeignKeys)
                {
                    foreach (var mappedTable in MappedTables)
                        foreach (var foreignKey in mappedTable.ForeignKeys.ToList())
                            mappedTable.RemoveForeignKey(foreignKey);

                }
#endif

                if ((Namespace as RDBMSMetaDataRepository.Storage).SupportViews)
                    BuildClassView();
                else
                {
                    if (_ClassView != null)
                    {
                        PersistenceLayer.ObjectStorage.DeleteObject(_ClassView);
                        _ClassView = null;
                    }
                }
                stateTransition.Consistent = true;
            }

            //AddObjectIDColumn

            //originMetaObject

        }

        /// <MetaDataID>{793a2088-f1c1-4548-aedf-2d9021fc961e}</MetaDataID>
        private Attribute GetColumnMappedAttribute(Column column, OOAdvantech.MetaDataRepository.Classifier Type, ValueTypePath valueTypePathIdentity)
        {
            if (valueTypePathIdentity == null)
                valueTypePathIdentity = new ValueTypePath();

            foreach (Attribute attribute in Type.GetAttributes(true))
            {
                if (attribute.IsPersistentValueType)
                {
                    valueTypePathIdentity.Push(attribute.Identity);

                    try
                    {
                        Attribute mappedAttribute = GetColumnMappedAttribute(column, attribute.Type, valueTypePathIdentity);
                        if (mappedAttribute != null)
                            return mappedAttribute;

                    }
                    finally
                    {
                        valueTypePathIdentity.Pop();
                    }
                }
                else
                {
                    if (valueTypePathIdentity.Count == 0)
                    {
                        if (attribute.Identity == column.MappedAttribute.Identity && valueTypePathIdentity.ToString() == column.CreatorIdentity)
                            return attribute;

                    }
                    else
                    {
                        valueTypePathIdentity.Push(attribute.Identity);
                        try
                        {
                            if (attribute.Identity == column.MappedAttribute.Identity && valueTypePathIdentity.ToString() == column.CreatorIdentity)
                                return attribute;
                        }
                        finally
                        {
                            valueTypePathIdentity.Pop();
                        }
                    }
                }
            }
            return null;

        }
        /// <MetaDataID>{017023eb-2c25-4879-95bd-5d79a872e94e}</MetaDataID>
        private AssociationEnd GetColumnMappedAssociationEnd(Column column, OOAdvantech.MetaDataRepository.Classifier Type, ValueTypePath valueTypePathIdentity)
        {
            if (valueTypePathIdentity == null)
                valueTypePathIdentity = new ValueTypePath();

            foreach (AssociationEnd associationEnd in Type.GetRoles(true))
            {
                if (associationEnd.Association.Identity != column.MappedAssociationEnd.Association.Identity)
                    continue;
                if (!string.IsNullOrEmpty(column.CreatorIdentity))
                {
                    if (column.MappedAssociationEnd.IsRoleA)
                        valueTypePathIdentity.Push(associationEnd.Association.RoleA.Identity);
                    else
                        valueTypePathIdentity.Push(associationEnd.Association.RoleB.Identity);
                    try
                    {
                        if (column.CreatorIdentity == valueTypePathIdentity.ToString())
                            if (column.MappedAssociationEnd.IsRoleA)
                                return associationEnd.Association.RoleA as AssociationEnd;
                            else
                                return associationEnd.Association.RoleB as AssociationEnd;
                    }
                    finally
                    {
                        valueTypePathIdentity.Pop();
                    }
                }
                else
                {
                    if (column.CreatorIdentity == valueTypePathIdentity.ToString())
                    {
                        if (column.MappedAssociationEnd.IsRoleA)
                            return associationEnd.Association.RoleA as AssociationEnd;
                        else
                            return associationEnd.Association.RoleB as AssociationEnd;
                    }
                }
            }
            foreach (Attribute attribute in Type.GetAttributes(true))
            {
                if (attribute.IsPersistentValueType)
                {
                    valueTypePathIdentity.Push(attribute.Identity);

                    try
                    {
                        AssociationEnd mappedAssociationEnd = GetColumnMappedAssociationEnd(column, attribute.Type, valueTypePathIdentity);
                        if (mappedAssociationEnd != null)
                            return mappedAssociationEnd;

                    }
                    finally
                    {
                        valueTypePathIdentity.Pop();
                    }
                }
            }
            return null;

        }


        /// <MetaDataID>{e053b754-7da9-4c92-a514-200389c0ac1d}</MetaDataID>
        private AssociationEnd GetColumnIndexerAssociationEnd(Column column, OOAdvantech.MetaDataRepository.Classifier Type, ValueTypePath valueTypePathIdentity)
        {
            if (valueTypePathIdentity == null)
                valueTypePathIdentity = new ValueTypePath();

            foreach (AssociationEnd associationEnd in Type.GetRoles(true))
            {
                if (associationEnd.Association.Identity != column.IndexerAssociationEnd.Association.Identity)
                    continue;
                if (!string.IsNullOrEmpty(column.CreatorIdentity))
                {
                    if (column.IndexerAssociationEnd.IsRoleA)
                        valueTypePathIdentity.Push(associationEnd.Association.RoleA.Identity);
                    else
                        valueTypePathIdentity.Push(associationEnd.Association.RoleB.Identity);
                    try
                    {
                        if (column.CreatorIdentity == valueTypePathIdentity.ToString())
                            if (column.IndexerAssociationEnd.IsRoleA)
                                return associationEnd.Association.RoleA as AssociationEnd;
                            else
                                return associationEnd.Association.RoleB as AssociationEnd;
                    }
                    finally
                    {
                        valueTypePathIdentity.Pop();
                    }
                }
                else
                {
                    if (column.CreatorIdentity == valueTypePathIdentity.ToString())
                    {
                        if (column.IndexerAssociationEnd.IsRoleA)
                            return associationEnd.Association.RoleA as AssociationEnd;
                        else
                            return associationEnd.Association.RoleB as AssociationEnd;
                    }
                }
            }
            foreach (Attribute attribute in Type.GetAttributes(true))
            {
                if (attribute.IsPersistentValueType)
                {
                    valueTypePathIdentity.Push(attribute.Identity);

                    try
                    {
                        AssociationEnd indexerAssociationEnd = GetColumnIndexerAssociationEnd(column, attribute.Type, valueTypePathIdentity);
                        if (indexerAssociationEnd != null)
                            return indexerAssociationEnd;

                    }
                    finally
                    {
                        valueTypePathIdentity.Pop();
                    }
                }
            }
            return null;

        }



        /// <summary>This method build – update useful mapping element like 
        /// new delete update store procedures, tables, views etc. </summary>
        /// <MetaDataID>{8C0EC232-6FD3-44BE-8A9E-93DEE7EF0FF2}</MetaDataID>
        public void BuildMappingElement()
        {
           
      

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                //TODO Το RDBMS mapping subsystem δεν υποστιρίζει αλλάγες στο τομεά inheritance mapping (OneTablePerHierarchy,OneTablePerConcreteClass,OneTablePerClass)	
                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    if (_SerialNumber == -1)
                    {
                        _SerialNumber = 1;

                        foreach (MetaDataRepository.MetaObject metaObject in (Namespace as Storage).OwnedElements)
                        {
                            if (metaObject is StorageCell && (metaObject as StorageCell)._SerialNumber != -1 && metaObject != this)
                                if ((metaObject as StorageCell)._SerialNumber >= _SerialNumber)
                                    _SerialNumber = (metaObject as StorageCell)._SerialNumber + 1;
                        }
                    }
                    BuildMainTable();
                    UpdateMainTableColumns();
#if !DeviceDotNet

                    if ((Namespace as RDBMSMetaDataRepository.Storage).SupportStoreProcedures)
                    {
                        BuildUpdateStoreProcedure();
                        BuildNewStoreProcedure();
                        BuildDeleteStoreProcedure();
                    }
                    else
                    {
                        if (_UpdateStoreProcedure != null)
                        {
                            PersistenceLayer.ObjectStorage.DeleteObject(_UpdateStoreProcedure);
                            _UpdateStoreProcedure = null;
                        }
                        if (_NewStoreProcedure != null)
                        {
                            PersistenceLayer.ObjectStorage.DeleteObject(_NewStoreProcedure);
                            _NewStoreProcedure = null;
                        }
                        if (_DeleteStoreProcedure != null)
                        {
                            PersistenceLayer.ObjectStorage.DeleteObject(_DeleteStoreProcedure);
                            _DeleteStoreProcedure = null;
                        }
                    }
#endif
                    if (!(Namespace as RDBMSMetaDataRepository.Storage).SupportForeignKeys)
                    {
                        foreach (var mappedTable in MappedTables)
                            foreach (var foreignKey in mappedTable.ForeignKeys.ToList())
                                mappedTable.RemoveForeignKey(foreignKey);

                    }

                    if ((Namespace as RDBMSMetaDataRepository.Storage).SupportViews)
                        BuildClassView();
                    else
                    {
                        if (_ClassView != null)
                        {
                            PersistenceLayer.ObjectStorage.DeleteObject(_ClassView);
                            _ClassView = null;
                        }
                    }
                    StateTransition.Consistent = true; ;

                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <MetaDataID>{5BACA9FC-D419-4F92-B990-EB403E9475FF}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }




        /// <MetaDataID>{0aa7ab91-f70b-4a8a-9ba0-10d66a85c050}</MetaDataID>
        public bool OIDIsDBGenerated
        {
            get
            {
                if (MainTable.ObjectIDColumns[0].IsIdentity)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{b8a615dd-cb85-4f2c-b6b3-e6c77a59ec73}</MetaDataID>
        public void GetRelationshipColumns()
        {
            foreach (RDBMSMetaDataRepository.AssociationEnd associationEnd in GetAssociatedRoles())
            {
                System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
                if ((associationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceColumnsFor(this).Count > 0)
                {


                    foreach (var linkedStorageCell in (associationEnd.Association as RDBMSMetaDataRepository.Association).GetStorageCellsLinks(this))
                    {
                        if ((this as RDBMSMetaDataRepository.StorageCell).MappedTables.Contains(linkedStorageCell.ForeignKeys[0].OriginTable))
                        {
                            if (linkedStorageCell.RoleAStorageCell == this)
                            {
                                if (!referenceObjectIdentityTypes.Contains(linkedStorageCell.RoleBStorageCell.ObjectIdentityType))
                                    referenceObjectIdentityTypes.Add(linkedStorageCell.RoleBStorageCell.ObjectIdentityType);
                            }
                            else
                            {
                                if (!referenceObjectIdentityTypes.Contains(linkedStorageCell.RoleAStorageCell.ObjectIdentityType))
                                    referenceObjectIdentityTypes.Add(linkedStorageCell.RoleAStorageCell.ObjectIdentityType);
                            }
                        }
                    }

                    if (referenceObjectIdentityTypes.Count > 0)
                    {
                        foreach (var objectIdentityType in associationEnd.GetReferenceObjectIdentityTypes(referenceObjectIdentityTypes))
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                //if (!columnNames.Contains(part.Name))
                                //{
                                //    coloumns.Add(new DataColumn(part.Name, null, part.Type, associationEnd, "", part));
                                //    columnNames.Add(part.Name);
                                //}
                            }
                        }
                    }
                }

            }

        }
    }



    /// <MetaDataID>{b7bd8f5e-9884-4745-a094-a1fd1c96ba7e}</MetaDataID>
    struct ValuePathAttribute
    {
        public string PathIdentity;

        public Attribute Attribute;
    }
}
