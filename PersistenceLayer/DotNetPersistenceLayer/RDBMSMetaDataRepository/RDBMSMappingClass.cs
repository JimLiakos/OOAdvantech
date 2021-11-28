using System.Linq;
namespace OOAdvantech.RDBMSMetaDataRepository
{
    using MetaDataRepository;
    using OOAdvantech.Collections.Generic;
    using OOAdvantech.Transactions;


    /// <summary>The class Class in this namespace is a specialization of metadata repository Class.
    /// The Class bridge the object oriented world with relation data base world. Used from  persistency system RTs (run time)s which use relational databases to store the objects. </summary>
    /// <MetaDataID>{D132DD45-BD82-45E7-9FE4-E3E760018FB9}</MetaDataID>
    [BackwardCompatibilityID("{D132DD45-BD82-45E7-9FE4-E3E760018FB9}")]
    [Persistent()]
    public class Class : OOAdvantech.MetaDataRepository.Class, MappedClassifier
    {
        public override MetaDataRepository.Component ImplementationUnit
        {
            get
            {
                var implementationUnit = base.ImplementationUnit;
                if (implementationUnit != null && implementationUnit.Context == null)
                {
                    if (PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
                    {
                        OOAdvantech.Linq.Storage theStorage = new Linq.Storage(PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

                        var storage = (from metastorage in theStorage.GetObjectCollection<Storage>() select metastorage).FirstOrDefault();
                        implementationUnit.Context = storage;

                    }
                }
                return implementationUnit;
            }
        }
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_TypeView))
            {
                if (value == null)
                    _TypeView = default(OOAdvantech.RDBMSMetaDataRepository.View);
                else
                    _TypeView = (OOAdvantech.RDBMSMetaDataRepository.View)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LocalStorageCells))
            {
                if (value == null)
                    _LocalStorageCells = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCell>);
                else
                    _LocalStorageCells = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCell>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageCellReferences))
            {
                if (value == null)
                    _StorageCellReferences = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellReference>);
                else
                    _StorageCellReferences = (OOAdvantech.Collections.Generic.Set<OOAdvantech.RDBMSMetaDataRepository.StorageCellReference>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectIdentityTypes))
            {
                if (value == null)
                    _ObjectIdentityTypes = default(System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>);
                else
                    _ObjectIdentityTypes = (System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ConcreteClassView))
            {
                if (value == null)
                    _ConcreteClassView = default(OOAdvantech.RDBMSMetaDataRepository.View);
                else
                    _ConcreteClassView = (OOAdvantech.RDBMSMetaDataRepository.View)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_TypeID))
            {
                if (value == null)
                    _TypeID = default(int);
                else
                    _TypeID = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_SplitLimit))
            {
                if (value == null)
                    _SplitLimit = default(int);
                else
                    _SplitLimit = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_HistoryClass))
            {
                if (value == null)
                    _HistoryClass = default(bool);
                else
                    _HistoryClass = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ActiveStorageCell))
            {
                if (value == null)
                    _ActiveStorageCell = default(OOAdvantech.RDBMSMetaDataRepository.StorageCell);
                else
                    _ActiveStorageCell = (OOAdvantech.RDBMSMetaDataRepository.StorageCell)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(StopRecursiveSpliting))
            {
                if (value == null)
                    StopRecursiveSpliting = default(bool);
                else
                    StopRecursiveSpliting = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_TypeView))
                return _TypeView;

            if (member.Name == nameof(_LocalStorageCells))
                return _LocalStorageCells;

            if (member.Name == nameof(_StorageCellReferences))
                return _StorageCellReferences;

            if (member.Name == nameof(_ObjectIdentityTypes))
                return _ObjectIdentityTypes;

            if (member.Name == nameof(_ConcreteClassView))
                return _ConcreteClassView;

            if (member.Name == nameof(_TypeID))
                return _TypeID;

            if (member.Name == nameof(_SplitLimit))
                return _SplitLimit;

            if (member.Name == nameof(_HistoryClass))
                return _HistoryClass;

            if (member.Name == nameof(_ActiveStorageCell))
                return _ActiveStorageCell;

            if (member.Name == nameof(StopRecursiveSpliting))
                return StopRecursiveSpliting;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{BAD4E86E-CFCE-4099-97D0-6DA1BEBB0778}</MetaDataID>
        [BackwardCompatibilityID("+45")]
        [PersistentMember("_StorageCells")]
        public override Collections.Generic.Set<MetaDataRepository.StorageCell> StorageCells
        {
            get
            {
                return base.StorageCells;
            }
        }
        /// <MetaDataID>{d5ccfa2b-c318-4196-a87f-cef0c4688cdc}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> StorageCellsOfThisType
        {
            get
            {
                return ClassifierLocalStorageCells;
            }
        }
        /// <MetaDataID>{b7eeed48-4fbf-4dd7-9f6a-3ed875da3d7d}</MetaDataID>
        internal bool ContainsValueTypeAsMember(OOAdvantech.MetaDataRepository.Structure memberStructure)
        {
            foreach (Attribute attribute in GetAttributes(true))
            {
                if (!IsPersistent(attribute) || (attribute.Type is Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;
                if (attribute.Type is Structure &&
                    (attribute.Type as Structure).Persistent)
                {

                    if (memberStructure == (attribute.Type as Structure))
                        return true;
                    else
                    {
                        if (ContainsValueTypeAsMember(attribute.Type as Structure, memberStructure))
                            return true;
                    }
                }
            }
            return false;
        }
        /// <MetaDataID>{c33e92d1-6c8f-45cf-81ba-c3c5cb7beca3}</MetaDataID>
        bool ContainsValueTypeAsMember(OOAdvantech.MetaDataRepository.Structure structure, OOAdvantech.MetaDataRepository.Structure memberStructure)
        {


            foreach (Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;
                if (attribute.Type is Structure &&
                    (attribute.Type as Structure).Persistent)
                {

                    if (memberStructure == (attribute.Type as Structure))
                        return true;
                    else
                    {
                        if (ContainsValueTypeAsMember(attribute.Type as Structure, memberStructure))
                            return true;
                    }
                }
            }
            return false;


        }

        /// <MetaDataID>{4c2cca11-3a30-4989-a1bc-e360c2685281}</MetaDataID>
        System.Collections.Generic.List<Attribute> GetAttributes(Structure structure)
        {
            System.Collections.Generic.List<Attribute> attributes = new System.Collections.Generic.List<Attribute>();

            foreach (Attribute attribute in structure.GetAttributes(true))
            {
                if (attribute.Type is Structure && (attribute.Type as Structure).Persistent)
                    attributes.AddRange(GetAttributes(attribute.Type as Structure));
                else
                    attributes.Add(attribute);
            }
            return attributes;

        }
        /// <summary>This method ensures that all members have case insensitive unique name. </summary>
        /// <remarks>
        /// This method extends the functionality of method of parent class. 
        /// It makes CaseInsensitiveName of attributes unique in class hierarchy.
        /// </remarks>
        /// <MetaDataID>{8BF2D570-04F4-4319-80BE-3794E0232C7D}</MetaDataID>
        public override void BuildCaseInsensitiveNames()
        {
            try
            {

                //TODO: Να γραφτεί test case οπου αναζητούντε data από δύο διαφορετικές storage
                //που έχουν φτιάξει διαφορετικό CaseInsensitiveName για το ίδιο attribute
                OOAdvantech.Collections.Generic.Dictionary<string, Attribute> members = new Collections.Generic.Dictionary<string, Attribute>();
                foreach (Attribute attribute in GetAttributes(true))
                {
                    //if (attribute.Type is Structure&&(attribute.Type as Structure).Persistent)
                    //{
                    //    foreach (Attribute structureAttribute in GetAttributes(attribute.Type as Structure))
                    //    {
                    //        string caseInsensitiveName = structureAttribute.CaseInsensitiveName;
                    //        if (members.Contains(caseInsensitiveName.Trim().ToLower()))
                    //        {
                    //            //TODO πρόβλημα όταν είναι structure μεσα σε structure
                    //            caseInsensitiveName = attribute.Owner.Name+"_"+ attribute.Name+ "_" + structureAttribute.Name;
                    //            structureAttribute.CaseInsensitiveName = caseInsensitiveName;
                    //        }

                    //        int count = 1;
                    //        while (members.Contains(caseInsensitiveName.Trim().ToLower()) && members[caseInsensitiveName.Trim().ToLower()]!=structureAttribute)
                    //        {
                    //            caseInsensitiveName = structureAttribute.CaseInsensitiveName + "_" + count.ToString();
                    //            count++;
                    //        }
                    //        if(!members.Contains(caseInsensitiveName.Trim().ToLower()))
                    //            members.Add(caseInsensitiveName.Trim().ToLower(), structureAttribute);
                    //        if (caseInsensitiveName.Trim().ToLower() != structureAttribute.Name.ToLower())
                    //            structureAttribute.CaseInsensitiveName = caseInsensitiveName;
                    //    }

                    //}
                    //else
                    {
                        string caseInsensitiveName = attribute.CaseInsensitiveName;
                        if (members.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                        {
                            caseInsensitiveName = attribute.Owner.Name + "_" + attribute.Name;
                            attribute.CaseInsensitiveName = caseInsensitiveName;
                        }

                        int count = 1;
                        while (members.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                        {
                            caseInsensitiveName = attribute.CaseInsensitiveName + "_" + count.ToString();
                            count++;
                        }
                        members.Add(caseInsensitiveName.Trim().ToLower(), attribute);
                        if (caseInsensitiveName.Trim().ToLower() != attribute.Name.ToLower())
                            attribute.CaseInsensitiveName = caseInsensitiveName;
                    }
                }


                System.Collections.Generic.Dictionary<string, MetaDataRepository.Association> associations = new System.Collections.Generic.Dictionary<string, MetaDataRepository.Association>();
                foreach (AssociationEnd associationEnd in GetRoles(true))
                {
                    string caseInsensitiveName = associationEnd.Association.CaseInsensitiveName;
                    int count = 1;
                    while (associations.ContainsKey(caseInsensitiveName.Trim().ToLower()) && associations[caseInsensitiveName.Trim().ToLower()] != associationEnd.Association)
                    {
                        caseInsensitiveName = associationEnd.Association.CaseInsensitiveName + "_" + count.ToString();
                        count++;
                    }
                    if (!associations.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                        associations.Add(caseInsensitiveName.Trim().ToLower(), associationEnd.Association);
                    if (caseInsensitiveName.Trim().ToLower() != associationEnd.Association.Name.ToLower())
                        associationEnd.Association.CaseInsensitiveName = caseInsensitiveName;
                }
                foreach (AssociationEnd associationEnd in GetAssociateRoles(true))
                {
                    string caseInsensitiveName = associationEnd.Association.CaseInsensitiveName;
                    int count = 1;
                    while (associations.ContainsKey(caseInsensitiveName.Trim().ToLower()) && associations[caseInsensitiveName.Trim().ToLower()] != associationEnd.Association)
                    {
                        caseInsensitiveName = associationEnd.Association.CaseInsensitiveName + "_" + count.ToString();
                        count++;
                    }
                    if (!associations.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                        associations.Add(caseInsensitiveName.Trim().ToLower(), associationEnd.Association);
                    if (caseInsensitiveName.Trim().ToLower() != associationEnd.Association.Name.ToLower())
                        associationEnd.Association.CaseInsensitiveName = caseInsensitiveName;
                }

                base.BuildCaseInsensitiveNames();
            }
            catch (System.Exception error)
            {
                throw;
            }

        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C138F2DA-70C5-4EB8-8BBB-D58E1A752FD4}</MetaDataID>
        private View _TypeView;
        /// <summary>This property defines a view. 
        /// This view returns a record set where each record is an object 
        /// which support the mapping type. </summary>
        /// <MetaDataID>{839E6A86-FC63-47EC-9324-58ABDC354D6C}</MetaDataID>
        [BackwardCompatibilityID("+44")]
        [PersistentMember("_TypeView")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        public View TypeView
        {
            get
            {
                return null;
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_TypeView == null)
                    {
                        if (ImplementationUnit.Context == null)
                            return null;
                        using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                        {

                            _TypeView = new View(CaseInsensitiveName, ImplementationUnit.Context);
                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_TypeView);
                            stateTransition.Consistent = true;
                        }
                    }
                    return _TypeView;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1D3694E0-46F0-466A-971F-E191230A3CD9}</MetaDataID>
        private Collections.Generic.Set<StorageCell> _LocalStorageCells = new OOAdvantech.Collections.Generic.Set<StorageCell>();
        /// <summary>The member LocalStorageCells defines a collection with storage cells where the object of storage cells lives in storage. </summary>
        /// <MetaDataID>{0F9F07F9-3AA2-47E7-A447-87907085963E}</MetaDataID>
        [Association("StorageCellsInLocalStorage", typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell), MetaDataRepository.Roles.RoleA, "{EF9FC10D-57C3-4C8B-AF43-D63C17F7C1AB}")]
        [PersistentMember("_LocalStorageCells")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        public Collections.Generic.Set<StorageCell> LocalStorageCells
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new Collections.Generic.Set<StorageCell>(_LocalStorageCells, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{AC63490C-DB0A-4317-A13C-A5F94C108BF5}</MetaDataID>
        private Collections.Generic.Set<StorageCellReference> _StorageCellReferences = new OOAdvantech.Collections.Generic.Set<StorageCellReference>();

        /// <summary>The member StorageCellReferences defines a collection with storage cells, 
        /// which are reference to the storage cell in other storage. </summary>
        /// <MetaDataID>{6B3428F6-7244-4CCC-9A96-326A8D1F3B46}</MetaDataID>
        [Association("StorageCellInOtherStorage", typeof(StorageCellReference), MetaDataRepository.Roles.RoleA, "{879AB004-19DC-41A2-BF9E-56333B89B9C1}")]
        [PersistentMember("_StorageCellReferences")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        public Collections.Generic.Set<StorageCellReference> StorageCellReferences
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return new OOAdvantech.Collections.Generic.Set<StorageCellReference>(_StorageCellReferences, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <summary>Define a collection with the storage cells 
        /// which the type of storage cell is subtype of classifier. </summary>
        /// <MetaDataID>{C2FC45B5-AAA9-45EF-B43E-FD667A51D14B}</MetaDataID>
        public Collections.Generic.Set<MetaDataRepository.StorageCell> ClassifierLocalStorageCells
        {
            get
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>();
                    storageCells.AddRange(_StorageCells);//storageCells.AddCollection(_LocalStorageCells);
                    foreach (MetaDataRepository.Generalization generalization in Specializations)
                    {
                        Class subClass = generalization.Child as Class;
                        foreach (StorageCell storageCell in subClass.ClassifierLocalStorageCells)
                            storageCells.Add(storageCell);
                    }
                    return storageCells;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }




        /// <MetaDataID>{6f69428a-cf70-4f5b-94a8-4d8376cca171}</MetaDataID>
        public System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> GetObjectIdentityTypes(System.Collections.Generic.List<MetaDataRepository.StorageCell> storageCells)
        {
            System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
            foreach (StorageCell storageCell in (this as MappedClassifier).ClassifierLocalStorageCells)
            {
                MetaDataRepository.ObjectIdentityType objectIdentityType = storageCell.ObjectIdentityType;
                if (!objectIdentityTypes.Contains(objectIdentityType))
                    objectIdentityTypes.Add(objectIdentityType);
            }
            return objectIdentityTypes;
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E656C956-102C-46FF-9EA2-E1542B80A371}</MetaDataID>
        System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType> _ObjectIdentityTypes;

        ///<summary>
        ///This property defines a collection with columns of object identity.
        ///</summary>
        ///<MetaDataID>{B066B9D9-6329-44EC-A3FC-C58774C69B63}</MetaDataID>
        public System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType> ObjectIdentityTypes
        {
            get
            {
                if (_ObjectIdentityTypes != null)
                    return _ObjectIdentityTypes;
                //TODO Θα πρέπει να διασφαλιστή ότι όλη class ιεραρχία έχει κοινό ObjectID Format
                if (base.Persistent)
                {
                    _ObjectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
                    foreach (StorageCell storageCell in StorageCells)
                    {
                        if (!_ObjectIdentityTypes.Contains(storageCell.ObjectIdentityType))
                            _ObjectIdentityTypes.Add(storageCell.ObjectIdentityType);
                    }

                }
                else
                {
                    //foreach (Generalization specializationRelation in Specializations)
                    //{
                    //    if ((specializationRelation.Child as Class).Persistent)
                    //    {
                    //        _ObjectIDColumns = new System.Collections.Generic.List<IdentityColumn>((specializationRelation.Child as Class).ActiveStorageCell.MainTable.ObjectIDColumns);
                    //        break;
                    //    }
                    //}
                    //_ObjectIDColumns = new System.Collections.Generic.List<IdentityColumn>(AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityColumns(this));
                }
                return _ObjectIdentityTypes;
            }
        }
        //public System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> ObjectIDColumns
        //{
        //    get
        //    {
        //        if (_ObjectIDColumns != null)
        //            return _ObjectIDColumns;
        //        _ObjectIDColumns = new System.Collections.Generic.List<ObjectIdentityType>();
        //        foreach (StorageCell storageCell in (this as MappedClassifier).LocalStorageCells)
        //        {
        //            MetaDataRepository.ObjectIdentityType objectIdentityType=storageCell.ObjectIdentityType;
        //            if (!_ObjectIDColumns.Contains(objectIdentityType))
        //                _ObjectIDColumns.Add(objectIdentityType);
        //        }




        //        ////TODO Θα πρέπει να διασφαλιστή ότι όλη class ιεραρχία έχει κοινό ObjectID Format
        //        //if (Persistent)
        //        //{
        //        //    _ObjectIDColumns = new OOAdvantech.Collections.Generic.Set<IdentityColumn>(ActiveStorageCell.MainTable.ObjectIDColumns);
        //        //}
        //        //else
        //        //{
        //        //    foreach (Generalization specializationRelation in Specializations)
        //        //    {
        //        //        if ((specializationRelation.Child as Class).Persistent)
        //        //        {
        //        //            _ObjectIDColumns = new OOAdvantech.Collections.Generic.Set<IdentityColumn>((specializationRelation.Child as Class).ActiveStorageCell.MainTable.ObjectIDColumns);
        //        //            break;
        //        //        }
        //        //    }
        //        //    _ObjectIDColumns = new OOAdvantech.Collections.Generic.Set<IdentityColumn>(AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityColumns(this));
        //        //}
        //        return _ObjectIDColumns;
        //    }
        //}
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{D98A3B4C-F07C-4B77-BA3F-916CFB8AEFA2}</MetaDataID>
        private View _ConcreteClassView;

        /// <summary>This property defines a view. 
        /// This view returns a record set where each record is an object 
        /// which is instance of this class. </summary>
        /// <MetaDataID>{9AFAF454-DE74-4FA2-9DCE-2D6101163062}</MetaDataID>
        [Association("InstancesType", typeof(OOAdvantech.RDBMSMetaDataRepository.View), MetaDataRepository.Roles.RoleA, "{976FEC1D-DE53-4C14-904A-7EBF2604B3AC}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_ConcreteClassView")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0)]
        public View ConcreteClassView
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _ConcreteClassView;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1B5F08C5-6F0D-4462-AC80-C749CBDC7B03}</MetaDataID>
        private int _TypeID = -1;

        /// <summary>TypeID define the identity of class with an integer number. 
        /// The number is unique in object storage. </summary>
        /// <MetaDataID>{7748A073-118E-43BA-9DF2-3F4D8131D24C}</MetaDataID>
        [BackwardCompatibilityID("+36")]
        [PersistentMember("_TypeID")]
        public virtual int TypeID
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_TypeID == -1)
                        throw new System.NotSupportedException("Class:" + FullName + ". The TypeID isn't initialized yet.");
                    return _TypeID;

                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <summary>If there is possibility object of this type, 
        /// to be persistent then this property is true else is false. </summary>
        /// <MetaDataID>{03CE08EB-D9EA-4621-A413-83362FA4BF88}</MetaDataID>
        public bool HasPersistentObjects
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (base.Persistent)
                        return true;
                    else
                    {
                        foreach (Generalization generalizationRelation in Specializations)
                        {
                            Class subClass = generalizationRelation.Child as Class;
                            if (subClass.HasPersistentObjects)
                                return true;
                        }
                        return false;
                    }
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        //public override Set<MetaDataRepository.AssociationEnd> Roles
        //{
        //    get
        //    {
        //        return new Set<MetaDataRepository.AssociationEnd>((from role in base.Roles
        //                                                           where role.Association != null && role.Association.Connections.Count == 2
        //                                                           select role).ToList());


        //        try
        //        {
        //            return base.Roles;
        //        }
        //        catch (System.Exception error)
        //        {
        //            if (this.FullName == "System.Object" && base.Roles.Count > 0)
        //            {
        //                var sds = base.Roles[0].GetOtherEnd();
        //            }

        //            return new Set<MetaDataRepository.AssociationEnd>((from role in base.Roles
        //                                                               where role.Association != null && role.Association.Connections.Count == 2
        //                                                               select role).ToList());
        //        }

        //    }
        //}
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{D55E078E-8967-4654-80E1-5A2064054EA0}</MetaDataID>
        private int _SplitLimit = 0;

        /// <summary>This property defines the maximum number of objects 
        /// per time period storage cell. </summary>
        /// <MetaDataID>{AA0E9798-11F3-4E1C-B463-C59B8E10EFFA}</MetaDataID>
        [BackwardCompatibilityID("+22")]
        [PersistentMember("_SplitLimit")]
        public int SplitLimit
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_HistoryClass)
                        return _SplitLimit;
                    foreach (MetaDataRepository.Generalization CurrGeneralization in Generalizations)
                    {
                        Class ParentClass = (Class)CurrGeneralization.Parent;
                        if (ParentClass.HistoryClass)
                            return ParentClass.SplitLimit;
                    }
                    return 0;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{63781D2B-323E-457B-A390-73146B675D49}</MetaDataID>
        private bool _HistoryClass = false;

        /// <summary>History class means that the system split the place where, 
        /// there are the objects of class and store the objects in terms of 
        /// the date time attribute which is declared for this reason. </summary>
        /// <remarks>
        /// The technique of history class is similar to the data partitioning method. 
        /// Split the data with time criterion. 
        /// If system searches for data of specific time period then with this technique, 
        /// search in dramatically less mass of data because ignore, 
        /// the irrelevant time periods data.
        /// </remarks>
        /// <MetaDataID>{C2C448BD-C8F4-423A-BD9B-9E2E0E4B51C7}</MetaDataID>
        [BackwardCompatibilityID("+21")]
        [PersistentMember("_HistoryClass")]
        public bool HistoryClass
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_HistoryClass)
                        return _HistoryClass;
                    foreach (MetaDataRepository.Generalization CurrGeneralization in Generalizations)
                    {
                        Class ParentClass = (Class)CurrGeneralization.Parent;
                        if (ParentClass.HistoryClass)
                            return true;
                    }
                    return false;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }
        /// <MetaDataID>{0BC30D12-EE2F-47DE-B0BF-0A1DA722EC0B}</MetaDataID>
        public Class()
        {

        }

        /// <summary>This method returns a collection with 
        /// storage cell of this class and sub classes 
        /// where contain objects of this time period. </summary>
        /// <param name="TimePeriodStartDate">This parameter defines the start date of time period. </param>
        /// <param name="TimePeriodEndDate">This parameter defines the end date of time period. </param>
        /// <MetaDataID>{82FCB8DD-F8A6-447D-803D-3362CE6CA698}</MetaDataID>
        public Collections.Generic.Set<MetaDataRepository.StorageCell> GetStorageCells(System.DateTime TimePeriodStartDate, System.DateTime TimePeriodEndDate)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new Collections.Generic.Set<MetaDataRepository.StorageCell>();
                foreach (StorageCell storageCell in _LocalStorageCells)
                {
                    if (storageCell == ActiveStorageCell)
                        continue;
                    //Start date of storage cell belongs to time period
                    if (storageCell.TimePeriodStart >= TimePeriodStartDate && storageCell.TimePeriodStart <= TimePeriodEndDate)
                        storageCells.Add(storageCell);
                    //End date of storage cell belongs to time period
                    if (storageCell.TimePeriodEnd >= TimePeriodStartDate && storageCell.TimePeriodEnd <= TimePeriodEndDate)
                        storageCells.Add(storageCell);

                    //Time period is subset of storage time period
                    if (TimePeriodStartDate >= storageCell.TimePeriodStart && TimePeriodEndDate <= storageCell.TimePeriodEnd)
                        storageCells.Add(storageCell);

                }

                //Check if ActiveStorageCell has object in time period
                if (ActiveStorageCell.TimePeriodStart < TimePeriodEndDate)
                    storageCells.Add(ActiveStorageCell);

                foreach (Generalization CurrGeneralization in Specializations)
                    storageCells.AddRange(((Class)CurrGeneralization.Child).GetStorageCells(TimePeriodStartDate, TimePeriodEndDate));

                return storageCells;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        public void RemoveStorageCell(StorageCell storageCell)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (_StorageCells.Contains(storageCell))
                    _StorageCells.Remove(storageCell);
                if (_ActiveStorageCell == storageCell)
                    _ActiveStorageCell = null;

                stateTransition.Consistent = true;
            }


        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{22D02D41-CCFE-4921-958D-4CD4915B93D6}</MetaDataID>
        private StorageCell _ActiveStorageCell;
        /// <summary>This property defines the storage cell where system store the object in current time. </summary>
        /// <MetaDataID>{DDB49E4D-9D31-47FD-89C8-239E44B33346}</MetaDataID>
        [Association("StoreObjetcsIn", typeof(OOAdvantech.RDBMSMetaDataRepository.StorageCell), MetaDataRepository.Roles.RoleA, "{70C0FFE8-784A-4E52-AC72-3BB91DCE2562}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_ActiveStorageCell")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(0)]
        public StorageCell ActiveStorageCell
        {
            get
            {
                lock (LockObject)
                {
                    if (_ActiveStorageCell != null)
                        return _ActiveStorageCell;
                    if (!base.Persistent)
                        throw new System.Exception("the class " + FullName + " isn't  Persistent");
                    if (_ActiveStorageCell == null)
                    {
                        _ActiveStorageCell = new StorageCell(this, ImplementationUnit.Context);
                        _ActiveStorageCell.AutoGenarated = true;
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_ActiveStorageCell);
                        _StorageCells.Add(_ActiveStorageCell);
                        _LocalStorageCells.Add(_ActiveStorageCell);
                    }
                    return _ActiveStorageCell;
                }
            }
            set
            {
                lock (LockObject)
                {
                    if (_StorageCells.Contains(value))
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {

                            _ActiveStorageCell = value;
                            stateTransition.Consistent = true;
                        }
                    }
                    else
                        throw new System.Exception("ActiveStortageCell must be one of Class StorageCells");

                }

            }
        }


        ///// <summary>This method return a view. 
        ///// This view returns a record set where each record is an object 
        ///// which support the type of class. </summary>
        ///// <param name="storageCells">The storageCells paramete define a collection of storage cells. </param>
        ///// <remarks>
        ///// The view will pump the data from the storage cells of storage cell collection.
        ///// The parameter must be not null or emtpy and all storage cell types must be subtype of class.
        ///// </remarks>
        ///// <MetaDataID>{7A982A22-032F-400D-BBBC-6F180642CD1E}</MetaDataID>
        //public View GetTypeView(Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells)
        //{

        //    ReaderWriterLock.AcquireReaderLock(10000);
        //    try
        //    {

        //        #region Precondition check
        //        if (storageCells == null || storageCells.Count == 0)
        //            throw new System.ArgumentException("The parameter 'storageCells' must be not null or empty");
        //        #endregion

        //        //TODO σε περιπτωση που υπαρχεί out storage collection και δεν υπάρχει επικοινωνεία με
        //        //τον άλλο server θα πρέπει να εγειρεται κατάλληλη exception
        //        View view = new View("Temp_Abstract_" + CaseInsensitiveName);
        //        foreach (Column CurrColumn in TypeView.ViewColumns)
        //            view.AddColumn(CurrColumn);


        //        foreach (MetaDataRepository. StorageCell storageCell in storageCells)
        //        {
        //            if (!storageCell.Type.IsA(this))
        //                throw new System.ArgumentException("Storage Cell '" + storageCell.Name + "' type isn't subtype of +'" + FullName + "'.");

        //            if (storageCell is StorageCellReference)
        //            {
        //                StorageCellReference OutStorageCell = storageCell as StorageCellReference;
        //                view.AddSubView(OutStorageCell.ConcreteClassView);

        //                if (view.ViewStorageCell == null)
        //                    view.ViewStorageCell = new OOAdvantech.Collections.Generic.Dictionary<View, StorageCell>();
        //                view.ViewStorageCell.Add(OutStorageCell.ConcreteClassView, storageCell);

        //            }
        //            else
        //            {
        //                view.AddSubView(storageCell.ClassView);

        //                if (view.ViewStorageCell == null)
        //                    view.ViewStorageCell = new OOAdvantech.Collections.Generic.Dictionary<View, StorageCell>();
        //                view.ViewStorageCell.Add(storageCell.ClassView, storageCell);
        //            }
        //        }
        //        return view;


        //    }
        //    finally
        //    {
        //        ReaderWriterLock.ReleaseReaderLock();
        //    }
        //}

        /// <MetaDataID>{EA519032-5B14-4A7A-A4FD-4DE3B69B3BA0}</MetaDataID>
        private bool StopRecursiveSpliting = false;

        /// <MetaDataID>{9717CAF9-A4FB-4BF0-8410-AA2DB4C6F958}</MetaDataID>
        internal void SplitActiveStorageCellWithoutLimitCheck()
        {
            //OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            lock (LockObject)
            {
                //try
                //{
                if (StopRecursiveSpliting)
                    return;
                StopRecursiveSpliting = true;
                try
                {
                    using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                    {
                        PersistenceLayer.Storage storageMetadata = null;
                        _ActiveStorageCell.TimePeriodEnd = System.DateTime.Now;

                        //split parent class if the inheritance relationsip has OneTablePerHierarchy attribute
                        //We must split the parent class firstly because the active storage cell class, share 
                        //with the parent active storage cell the main table
                        foreach (Generalization CurrSpecialization in Specializations)
                        {
                            if (CurrSpecialization.GeneralizationMappingType == GeneralizationMappingType.OneTablePerHierarchy)
                                ((Class)CurrSpecialization.Child).SplitActiveStorageCellWithoutLimitCheck();
                        }

                        storageMetadata = _ActiveStorageCell.Namespace as PersistenceLayer.Storage;
                        _ActiveStorageCell = new StorageCell(this, storageMetadata);
                        _ActiveStorageCell.AutoGenarated = true;
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_ActiveStorageCell);
                        _StorageCells.Add(_ActiveStorageCell);
                        _LocalStorageCells.Add(_ActiveStorageCell);
                        BuildMappingElement();

                        foreach (Generalization CurrGeneralization in Generalizations)
                        {
                            if (CurrGeneralization.GeneralizationMappingType == GeneralizationMappingType.OneTablePerHierarchy)
                                ((Class)CurrGeneralization.Parent).SplitActiveStorageCellWithoutLimitCheck();
                        }

                        foreach (Generalization CurrGeneralization in Generalizations)
                            CurrGeneralization.BuildMappingElement();
                        foreach (Generalization CurrGeneralization in Specializations)
                            CurrGeneralization.BuildMappingElement();

                        StateTransition.Consistent = true;
                    }
                }
                finally
                {
                    StopRecursiveSpliting = false;
                }
            }
            //finally
            //{
            //    //ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            //}


        }
        /// <summary>The main work is to make a new storage cell. 
        /// It participates in splintering of mechanism with intent 
        /// to make faster the searching of data. </summary>
        /// <MetaDataID>{3ECD780D-415A-4ED4-967C-13552AE80077}</MetaDataID>
        public void SplitActiveStorageCell()
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (!base.Persistent)
                    throw new System.Exception("You can’t apply splintering of mechanism on a non Persistent class.");
                if (!HistoryClass)
                    throw new System.Exception("You can’t apply splintering of mechanism on a non history class.");
                if (ActiveStorageCell.ObjectsCount == -1)
                    throw new System.Exception("You can’t apply splintering of mechanism on Storage Cell with ObjectsCount uninitialized.");
                if (SplitLimit >= ActiveStorageCell.ObjectsCount)
                    throw new System.Exception("You can’t apply splintering of mechanism because the Active Storage Cell ObjectsCount is less than Number of Objects Limit.");
                SplitActiveStorageCellWithoutLimitCheck();
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }







        /// <MetaDataID>{E4C5B72D-32B0-4D5B-8DD2-4F2F80536275}</MetaDataID>
        public StorageCellReference GetStorageCellReference(string storageCellName, int storageCellID, string storageName, MetaDataRepository.ObjectIdentityType objectIdentityType, string storageIdentity, string storageLocation, string storageType)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {


                foreach (StorageCellReference storageCellReference in _StorageCellReferences)
                {
                    if (storageCellReference.StorageIdentity == storageIdentity &&
                        storageCellReference.SerialNumber == storageCellID)
                        return storageCellReference;
                }
                using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    RDBMSMetaDataRepository.StorageCellReference storageCellReference = new RDBMSMetaDataRepository.StorageCellReference(this, storageCellName, storageCellID, objectIdentityType, storageName, storageIdentity, storageLocation, storageType);
                    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(storageCellReference);
                    _StorageCells.Add(storageCellReference);
                    _StorageCellReferences.Add(storageCellReference);

                    stateTransition.Consistent = true;
                    return storageCellReference;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <summary>This method return the storage of object with ObjectID the value of objectID parameter
        /// If there isn't storage cell for the objectID the system throw exception </summary>
        /// <MetaDataID>{E74441CF-6E7D-42D7-8B63-5C8DC8C79737}</MetaDataID>
        public StorageCell GetStorageCell(object objectID)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                foreach (StorageCell CurrStorageCell in StorageCells)
                {
                    if (CurrStorageCell.SerialNumber.Equals(objectID))
                        return CurrStorageCell;
                }
                throw new System.Exception("There is not StorageCell with ObjectID '" + objectID.ToString() + "'");
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }



        /// <summary>
        /// The method BuildMappingElement constructs or updates 
        /// the mapping and relational data base elements like tables views store procedures storage cells.
        /// If the class is not persistent then constructs only a RDBMS view.
        /// </summary>
        /// <MetaDataID>{EAD8D549-FCF5-4A8C-A6CE-C52D81159101}</MetaDataID>
        public void BuildMappingElement()
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                BuildCaseInsensitiveNames();

                _ObjectIdentityTypes = null;

                using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    #region Initialize the TypeID
                    if (_TypeID == -1)
                    {
                        _TypeID = 1;
                        foreach (Component component in (ImplementationUnit.Context as MetaDataRepository.Storage).Components)
                        {
                            foreach (MetaDataRepository.MetaObject metaObject in component.Residents)
                            {
                                if (metaObject is Class && (metaObject as Class)._TypeID != -1 && metaObject != this)
                                {
                                    if ((metaObject as Class)._TypeID >= _TypeID)
                                        _TypeID = (metaObject as Class)._TypeID + 1;
                                }
                            }
                        }
                    }
                    #endregion

                    if (base.Persistent && !Abstract)
                    {
                        #region Create ActiveStorageCell if not exist
                        if (_ActiveStorageCell == null)
                        {
                            //Force class to create ActiveStorageCell
                            _ActiveStorageCell = ActiveStorageCell;
                        }
                        #endregion


                        foreach (StorageCell storageCell in StorageCells.OfType<StorageCell>())
                            storageCell.BuildMappingElement();

                        if (false)
                        {
                            #region Build concreteclass view
                            //if (ConcreteClassView == null)
                            //{
                            //    if (Name == null)
                            //        throw new System.Exception("Build Error :You must set the OutStorageStorageCell Name");
                            //    _ConcreteClassView = new View("CO_" + CaseInsensitiveName, (MetaDataRepository.Namespace)ImplementationUnit.Context);
                            //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(ConcreteClassView);
                            //}
                            //else
                            //    ConcreteClassView.Name = "CO_" + Name;

                            //foreach (StorageCell CurrStorageCell in StorageCells)
                            //    ConcreteClassView.AddSubView(CurrStorageCell.ClassView);
                            #endregion


                            #region Build abstract class view
                            //if (_TypeView == null)
                            //{
                            //    _TypeView = new View(CaseInsensitiveName, ImplementationUnit.Context as MetaDataRepository.Namespace);
                            //    if (Name == null)
                            //        throw new System.Exception("Build Error :You must set the OutStorageStorageCell Name");
                            //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_TypeView);
                            //}
                            //else
                            //{
                            //    if (_TypeView.Namespace == null)
                            //        (ImplementationUnit.Context as Namespace).AddOwnedElement(_TypeView);
                            //    _TypeView.Name = Name;
                            //}
                            //_TypeView.AddSubView(ConcreteClassView);

                            #endregion

                        }

                    }
                    else
                    {
                        PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);

                        #region Build abstract class view

                        //#region Create abstract class view if there isn't
                        //if (_TypeView == null)
                        //{
                        //    _TypeView = new View(CaseInsensitiveName, ImplementationUnit.Context as MetaDataRepository.Namespace);
                        //    if (Name == null)
                        //        throw new System.Exception("Build Error :You must set the OutStorageStorageCell Name");
                        //    objectStorage.CommitTransientObjectState(_TypeView);
                        //}
                        //else
                        //    _TypeView.Name = Name;
                        //#endregion

                        //System.Collections.Hashtable oldColumns_DeleteCandidate = new System.Collections.Hashtable();
                        //foreach (Column viewColumn in TypeView.ViewColumns)
                        //    oldColumns_DeleteCandidate.Add(viewColumn.Name, viewColumn);

                        //#region Get view columns
                        //Collections.Generic.Set <Column> newColumns = new OOAdvantech.Collections.Generic.Set<Column>();
                        //foreach (Column column in AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this).Parts)
                        //    newColumns.Add(column);

                        //newColumns.AddRange(AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumns(this));
                        //foreach (MetaDataRepository.AssociationEnd CurrAssociationEnd in GetAssociateRoles(true))
                        //{
                        //    foreach (Column column in ((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).GetReferenceColumns(this))
                        //        newColumns.Add(column);

                        //    if (((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn != null && ((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).GetReferenceColumns(this).Count > 0)
                        //    {
                        //        Column indexerColumn = new Column(((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn.Name, ((AssociationEnd)CurrAssociationEnd).IndexerColumn.Type);
                        //        newColumns.Add(indexerColumn);
                        //    }

                        //}
                        //foreach (MetaDataRepository.Attribute attribute in GetAttributes(true))
                        //{
                        //    if (attribute.Type is MetaDataRepository.Primitive || attribute.Type.FullName.Trim() == typeof(string).FullName)
                        //    {
                        //        Column column = new Column();
                        //        if (attribute.Type is MetaDataRepository.Structure)
                        //            column.Name = attribute.Owner + "_" + attribute.CaseInsensitiveName;
                        //        else
                        //            column.Name = attribute.CaseInsensitiveName;

                        //        column.Type = attribute.Type;
                        //        newColumns.Add(column);
                        //    }
                        //}
                        //if (ClassHierarchyLinkAssociation != null)
                        //{
                        //    foreach (Column column in ((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).GetReferenceColumns(this))
                        //        newColumns.Add(column);
                        //    foreach (Column column in ((AssociationEnd)ClassHierarchyLinkAssociation.RoleB).GetReferenceColumns(this))
                        //        newColumns.Add(column);

                        //    if (((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).IndexerColumn != null)
                        //    {
                        //        Column indexerColumn = new Column(((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).IndexerColumn.Name, ((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).IndexerColumn.Type);
                        //        newColumns.Add(indexerColumn);
                        //    }
                        //    if (((AssociationEnd)ClassHierarchyLinkAssociation.RoleB).IndexerColumn != null)
                        //    {
                        //        Column indexerColumn = new Column(((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).IndexerColumn.Name, ((AssociationEnd)ClassHierarchyLinkAssociation.RoleB).IndexerColumn.Type);
                        //        newColumns.Add(indexerColumn);
                        //    }


                        //}
                        //#endregion

                        //#region Add new columns to view
                        //foreach (Column column in newColumns)
                        //{
                        //    if (oldColumns_DeleteCandidate.Contains(column.Name))
                        //        oldColumns_DeleteCandidate.Remove(column.Name);
                        //    else
                        //    {
                        //        objectStorage.CommitTransientObjectState(column);
                        //        _TypeView.AddColumn(column);
                        //    }
                        //}
                        //#endregion

                        //#region Remove unused columns
                        //foreach (System.Collections.DictionaryEntry entry in oldColumns_DeleteCandidate)
                        //{
                        //    Column column = entry.Value as Column;
                        //    _TypeView.RemoveColumn(column);
                        //}
                        //#endregion

                        #endregion
                    }
                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <summary>
        ///  Synchronize MetaObject. 
        ///  This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. 
        ///  Synchronize is the main operation for Metadata repositories syncrhonization. 
        /// </summary>
        /// <MetaDataID>{43312B30-CBB5-4E54-B88C-52128E85A74C}</MetaDataID>
        public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (OriginMetaObject.FullName.IndexOf("System.Func") != -1)
            {
            }


            if (OriginMetaObject.FullName == "FlavourBusinessManager.HumanResources.ServingShiftWork")
            {

            }
            var m_name = OriginMetaObject.FullName;

            var sds = Features;

            if (Name == "ItemPreparation")
            {
                var reals = Features.OfType<AssociationEndRealization>().ToArray();


                //var ere = reals[1].Specification.AssociationEndRealizations.ToArray();
            }
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                MetaDataRepository.Class OriginClass = (MetaDataRepository.Class)OriginMetaObject;

                if (OriginClass.IsTemplate)
                {
                    if (_Name != OriginClass.Name)
                    {
                        _Name = OriginClass.Name;
                        _CaseInsensitiveName = null;
                    }


                    if (_Namespace == null && PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties) != null)
                        PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties).LazyFetching("Namespace", typeof(MetaDataRepository.MetaObject));
                    if (_Namespace.Value == null && OriginMetaObject.Namespace != null)
                    {
                        _Namespace.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginMetaObject.Namespace, this) as MetaDataRepository.Namespace;


                        if (_Namespace.Value == null)
                        {
                            _Namespace.Value = (Namespace)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(OriginMetaObject.Namespace, this);
                            if (_Namespace.Value != null)
                                _Namespace.Value.ShallowSynchronize(OriginMetaObject.Namespace);
                        }
                        if (_Namespace.Value != null)
                            _Namespace.Value.AddOwnedElement(this);
                    }



                    if (_ImplementationUnit.Value == null && OriginMetaObject.ImplementationUnit != null)
                    {
                        _ImplementationUnit.Value = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginMetaObject.ImplementationUnit, this) as MetaDataRepository.Component;
                        if (_ImplementationUnit.Value == null)
                            _ImplementationUnit.Value = (Component)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(OriginMetaObject.ImplementationUnit, this);
                    }
                    return;
                }

                using (OOAdvantech.Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    base.Synchronize(OriginMetaObject);

                    foreach (MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
                    {
                        foreach (MetaDataRepository.Classifier originClassifier in OriginClass.GetAllGeneralClasifiers())
                        {
                            if (classifier.Identity == originClassifier.Identity)
                                classifier.Synchronize(originClassifier);
                        }

                    }

                    object Value = GetPropertyValue(typeof(bool), "Persistence", "HistoryClass");
                    if (Value != null)
                    {
                        _HistoryClass = (bool)Value;
                        if (_HistoryClass)
                            _SplitLimit = (int)GetPropertyValue(typeof(int), "Persistence", "NumberOfObject");
                    }
                    else
                        _HistoryClass = false;

                    //TODO θα πρέπει να ορισθουν οι αλλαγές που μπορούν να γίνουν οσο αφορά την class ιεραρχία
                    //και οσο αφορά το mapping type της ιεραρχίας.

                    string MappingType = (string)GetPropertyValue(typeof(string), "ExtMetaData", "InheritanceMapping");
                    //"OneTablePerClass","OneTablePerHierarchy","OneTablePerConcreteClass"
                    Generalization GeneralizeRelationship = null;

                    foreach (MetaDataRepository.Generalization CurrGeneralize in Generalizations)
                    {
                        GeneralizeRelationship = (Generalization)CurrGeneralize;
                        break;
                    }
                    if (GeneralizeRelationship != null)
                    {
                        using (OOAdvantech.Transactions.ObjectStateTransition GeneralizeRelationshipStateTransition = new OOAdvantech.Transactions.ObjectStateTransition(GeneralizeRelationship))
                        {
                            if (base.Persistent && !Abstract)
                            {
                                switch (MappingType)
                                {
                                    case "OneTablePerClass":
                                        {
                                            //The OneTablePerClass mapping model doesn’t used in history classes.
                                            if (HistoryClass || ((Class)GeneralizeRelationship.Parent).HistoryClass)
                                                GeneralizeRelationship.GeneralizationMappingType = GeneralizationMappingType.OneTablePerConcreteClass;
                                            else
                                                GeneralizeRelationship.GeneralizationMappingType = GeneralizationMappingType.OneTablePerClass;
                                            break;
                                        }
                                    case "OneTablePerHierarchy":
                                        {
                                            GeneralizeRelationship.GeneralizationMappingType = GeneralizationMappingType.OneTablePerHierarchy;
                                            break;
                                        }
                                    case "OneTablePerConcreteClass":
                                        {
                                            GeneralizeRelationship.GeneralizationMappingType = GeneralizationMappingType.OneTablePerConcreteClass;
                                            break;
                                        }
                                    default:
                                        {
                                            GeneralizeRelationship.GeneralizationMappingType = GeneralizationMappingType.OneTablePerConcreteClass;
                                            break;
                                        }
                                }
                            }
                            else
                                GeneralizeRelationship.GeneralizationMappingType = GeneralizationMappingType.OneTablePerConcreteClass;
                            GeneralizeRelationshipStateTransition.Consistent = true; ;
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

        /// <MetaDataID>{ce5d09ce-3497-48fc-ab0c-7ab72de4277d}</MetaDataID>
        internal void UpdateMappingElement(Class _class)
        {
            if (_class.StorageCells.Count == 0)
                return;
            if (_class.ActiveStorageCell.MappedTables.Count == 0)
                return;
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                BuildCaseInsensitiveNames();

                _ObjectIdentityTypes = null;

                using (OOAdvantech.Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    #region Initialize the TypeID
                    if (_TypeID == -1)
                    {
                        _TypeID = 1;
                        foreach (Component component in (ImplementationUnit.Context as Storage).Components)
                        {
                            foreach (MetaDataRepository.MetaObject metaObject in component.Residents)
                            {
                                if (metaObject is Class && (metaObject as Class)._TypeID != -1 && metaObject != this)
                                {
                                    if ((metaObject as Class)._TypeID >= _TypeID)
                                        _TypeID = (metaObject as Class)._TypeID + 1;
                                }
                            }
                        }
                    }
                    #endregion

                    if (base.Persistent && !Abstract)
                    {
                        StorageCell storageCell = null;
                        #region Create ActiveStorageCell if not exist
                        if (_ActiveStorageCell == null)
                        {
                            //Force class to create ActiveStorageCell
                            _ActiveStorageCell = ActiveStorageCell;
                            storageCell = _ActiveStorageCell;
                        }
                        else
                        {

                            storageCell = new StorageCell(this, _ActiveStorageCell.Namespace as PersistenceLayer.Storage);
                            _ActiveStorageCell = storageCell;
                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(storageCell);
                            _StorageCells.Add(storageCell);
                            _LocalStorageCells.Add(storageCell);
                        }
                        #endregion

                        storageCell.Synchronize(_class.ActiveStorageCell);
                        //foreach (StorageCell storageCell in StorageCells)
                        //    storageCell.BuildMappingElement();


                        #region Build concreteclass view
                        if (ConcreteClassView == null)
                        {
                            if (Name == null)
                                throw new System.Exception("Build Error :You must set the OutStorageStorageCell Name");
                            _ConcreteClassView = new View("CO_" + CaseInsensitiveName, (MetaDataRepository.Namespace)ImplementationUnit.Context);
                            PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(ConcreteClassView);
                        }
                        else
                            ConcreteClassView.Name = "CO_" + Name;

                        foreach (StorageCell CurrStorageCell in StorageCells)
                            ConcreteClassView.AddSubView(CurrStorageCell.ClassView);


                        foreach (Column viewColumn in ConcreteClassView.ViewColumns)
                            ConcreteClassView.RemoveColumn(viewColumn);
                        #endregion


                        #region Build abstract class view
                        //if (_TypeView == null)
                        //{
                        //    _TypeView = new View(CaseInsensitiveName, ImplementationUnit.Context as MetaDataRepository.Namespace);
                        //    if (Name == null)
                        //        throw new System.Exception("Build Error :You must set the OutStorageStorageCell Name");
                        //    PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(_TypeView);
                        //}
                        //else
                        //{
                        //    if (_TypeView.Namespace == null)
                        //        (ImplementationUnit.Context as Namespace).AddOwnedElement(_TypeView);
                        //    _TypeView.Name = Name;
                        //}

                        //_TypeView.AddSubView(ConcreteClassView);

                        //foreach (Column viewColumn in _TypeView.ViewColumns)
                        //    _TypeView.RemoveColumn(viewColumn);

                        //foreach (Column CurrColumn in ConcreteClassView.ViewColumns)
                        //    _TypeView.AddColumn(CurrColumn);
                        #endregion


                        #region Update parent classes and interfaces views
                        //foreach (MetaDataRepository.Classifier CurrClassifier in GetAllGeneralClasifiers())
                        //{
                        //    if (CurrClassifier is Class)
                        //    {
                        //        Class CurrClass = CurrClassifier as Class;
                        //        if (CurrClass.TypeView != null)
                        //            CurrClass.TypeView.AddSubView(TypeView);	// που δεν είναι Presistent
                        //    }
                        //}

                        //foreach (MetaDataRepository.Classifier CurrClassifier in GetInterfaces())
                        //{
                        //    if (CurrClassifier is Interface)
                        //    {
                        //        Interface _Interface = CurrClassifier as Interface;
                        //        if (_Interface.TypeView != null)
                        //            _Interface.TypeView.AddSubView(ConcreteClassView);	// που δεν είναι Presistent
                        //    }
                        //}
                        #endregion
                    }
                    else
                    {
                        PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);

                        #region Build abstract class view

                        //#region Create abstract class view if there isn't
                        //if (_TypeView == null)
                        //{
                        //    _TypeView = new View(CaseInsensitiveName, ImplementationUnit.Context as MetaDataRepository.Namespace);
                        //    if (Name == null)
                        //        throw new System.Exception("Build Error :You must set the OutStorageStorageCell Name");
                        //    objectStorage.CommitTransientObjectState(_TypeView);
                        //}
                        //else
                        //    _TypeView.Name = Name;
                        //#endregion

                        //System.Collections.Hashtable oldColumns_DeleteCandidate = new System.Collections.Hashtable();
                        //foreach (Column viewColumn in TypeView.ViewColumns)
                        //    oldColumns_DeleteCandidate.Add(viewColumn.Name, viewColumn);

                        //#region Get view columns
                        //Collections.Generic.Set<Column> newColumns = new OOAdvantech.Collections.Generic.Set<Column>();
                        //foreach (Column column in AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetObjectIdentityType(this).Parts)
                        //    newColumns.Add(column);

                        //newColumns.AddRange(AutoProduceColumnsGenerator.CurrentAutoProduceColumnsGenerator.GetAuxiliaryColumns(this));
                        //foreach (MetaDataRepository.AssociationEnd CurrAssociationEnd in GetAssociateRoles(true))
                        //{
                        //    foreach (Column column in ((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).GetReferenceColumns(this))
                        //        newColumns.Add(column);

                        //    if (((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn != null && ((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).GetReferenceColumns(this).Count > 0)
                        //    {
                        //        Column indexerColumn = new Column(((AssociationEnd)CurrAssociationEnd.GetOtherEnd()).IndexerColumn.Name, ((AssociationEnd)CurrAssociationEnd).IndexerColumn.Type);
                        //        newColumns.Add(indexerColumn);
                        //    }

                        //}
                        //foreach (MetaDataRepository.Attribute attribute in GetAttributes(true))
                        //{
                        //    if (attribute.Type is MetaDataRepository.Primitive || attribute.Type.FullName.Trim() == typeof(string).FullName)
                        //    {
                        //        Column column = new Column();
                        //        if (attribute.Type is MetaDataRepository.Structure)
                        //            column.Name = attribute.Owner + "_" + attribute.CaseInsensitiveName;
                        //        else
                        //            column.Name = attribute.CaseInsensitiveName;

                        //        column.Type = attribute.Type;
                        //        newColumns.Add(column);
                        //    }
                        //}
                        //if (ClassHierarchyLinkAssociation != null)
                        //{
                        //    foreach (Column column in ((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).GetReferenceColumns(this))
                        //        newColumns.Add(column);
                        //    foreach (Column column in ((AssociationEnd)ClassHierarchyLinkAssociation.RoleB).GetReferenceColumns(this))
                        //        newColumns.Add(column);

                        //    if (((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).IndexerColumn != null)
                        //    {
                        //        Column indexerColumn = new Column(((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).IndexerColumn.Name, ((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).IndexerColumn.Type);
                        //        newColumns.Add(indexerColumn);
                        //    }
                        //    if (((AssociationEnd)ClassHierarchyLinkAssociation.RoleB).IndexerColumn != null)
                        //    {
                        //        Column indexerColumn = new Column(((AssociationEnd)ClassHierarchyLinkAssociation.RoleA).IndexerColumn.Name, ((AssociationEnd)ClassHierarchyLinkAssociation.RoleB).IndexerColumn.Type);
                        //        newColumns.Add(indexerColumn);
                        //    }


                        //}
                        //#endregion

                        //#region Add new columns to view
                        //foreach (Column column in newColumns)
                        //{
                        //    if (oldColumns_DeleteCandidate.Contains(column.Name))
                        //        oldColumns_DeleteCandidate.Remove(column.Name);
                        //    else
                        //    {
                        //        objectStorage.CommitTransientObjectState(column);
                        //        _TypeView.AddColumn(column);
                        //    }
                        //}
                        //#endregion

                        //#region Remove unused columns
                        //foreach (System.Collections.DictionaryEntry entry in oldColumns_DeleteCandidate)
                        //{
                        //    Column column = entry.Value as Column;
                        //    _TypeView.RemoveColumn(column);
                        //}
                        //#endregion

                        #endregion
                    }
                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
    }
}
