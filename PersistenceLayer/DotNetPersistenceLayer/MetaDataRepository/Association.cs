namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{183ad672-5cb4-4f05-a93e-968631af2e55}</MetaDataID>
    public enum AssociationType
    {
        /// <summary>
        /// Role A is zero or one and role B is many.
        /// </summary>
        OneToMany = 0,
        /// <summary>
        /// Role A is many and role B is many.
        /// </summary>
        ManyToMany,
        /// <summary>
        /// Role A is zero or one and role B is zero or one.
        /// </summary>
        OneToOne,
        /// <summary>
        /// Role A is many and role B is zero or one.
        /// </summary>
        ManyToOne,
        None
    };

    /// <MetaDataID>{AF4C9386-EB2E-45EE-B801-50611D1FFC8B}</MetaDataID>
    /// <summary>An association defines a semantic relationship between classifiers. 
    /// The instances of an association are a set of tuples relating instances of the classifiers. 
    /// Each tuple value may appear at most once.
    /// In the metamodel, an Association is a declaration of a semantic relationship between Classifiers, such as Classes. 
    /// An Association has  two AssociationEnds. Each end is connected to a Classifier. 
    /// The Association represents a set of connections among instances of the Classifiers.
    /// An instance of an Association is a Link, which is a tuple of Instances drawn from the corresponding Classifiers. </summary>
    [BackwardCompatibilityID("{AF4C9386-EB2E-45EE-B801-50611D1FFC8B}")]
    [Persistent()]
    public class Association : Relationship
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_LinkClass))
            {
                if (value == null)
                    _LinkClass = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _LinkClass = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Specializations))
            {
                if (value == null)
                    _Specializations = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Association>);
                else
                    _Specializations = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Association>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_General))
            {
                if (value == null)
                    _General = default(OOAdvantech.Member<OOAdvantech.MetaDataRepository.Association>);
                else
                    _General = (OOAdvantech.Member<OOAdvantech.MetaDataRepository.Association>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_MultiplicityType))
            {
                if (value == null)
                    _MultiplicityType = default(OOAdvantech.MetaDataRepository.AssociationType);
                else
                    _MultiplicityType = (OOAdvantech.MetaDataRepository.AssociationType)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ConnectionsForClients))
            {
                if (value == null)
                    ConnectionsForClients = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>);
                else
                    ConnectionsForClients = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RoleA))
            {
                if (value == null)
                    _RoleA = default(OOAdvantech.MetaDataRepository.AssociationEnd);
                else
                    _RoleA = (OOAdvantech.MetaDataRepository.AssociationEnd)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_RoleB))
            {
                if (value == null)
                    _RoleB = default(OOAdvantech.MetaDataRepository.AssociationEnd);
                else
                    _RoleB = (OOAdvantech.MetaDataRepository.AssociationEnd)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Connections))
            {
                if (value == null)
                    _Connections = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>);
                else
                    _Connections = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(StorageCellsLinks))
            {
                if (value == null)
                    StorageCellsLinks = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCellsLink>);
                else
                    StorageCellsLinks = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCellsLink>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_LinkClass))
                return _LinkClass;

            if (member.Name == nameof(_Specializations))
                return _Specializations;

            if (member.Name == nameof(_General))
                return _General;

            if (member.Name == nameof(_MultiplicityType))
                return _MultiplicityType;

            if (member.Name == nameof(ConnectionsForClients))
                return ConnectionsForClients;

            if (member.Name == nameof(_RoleA))
                return _RoleA;

            if (member.Name == nameof(_RoleB))
                return _RoleB;

            if (member.Name == nameof(_Connections))
                return _Connections;

            if (member.Name == nameof(StorageCellsLinks))
                return StorageCellsLinks;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7812D14C-E4AD-4960-817C-DF236EDD45B2}</MetaDataID>
        protected Classifier _LinkClass;
        /// <summary>Identifies the association as a link class </summary>
        /// <MetaDataID>{55C91094-67AC-49A9-B952-FE95E7B2A372}</MetaDataID>
        [Association("AssociationClass", typeof(Classifier), Roles.RoleA, "{D12835A7-0E6D-45A9-A091-4AC1403B479E}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_LinkClass")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0, 1)]

        public virtual Classifier LinkClass
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _LinkClass;
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

                    _LinkClass = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }



        /// <MetaDataID>{ce623100-3782-4967-8730-902f4dee949e}</MetaDataID>
        public void RemoveSpecialization(Association association)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (!_Specializations.Contains(association))
                    return;
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _Specializations.Remove(association);
                    StateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <MetaDataID>{6e634888-3624-4d82-9881-5f8f6c664ef6}</MetaDataID>
        public void AddSpecialization(Association association)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (_Specializations.Contains(association))
                    return;
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _Specializations.Add(association);
                    StateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Association> _Specializations = new OOAdvantech.Collections.Generic.Set<Association>();
        [Association("SpecializedAssociations", typeof(Association), Roles.RoleB, "04b2946e-5c81-41d5-8d07-3d1b0fae5e56")]
        [PersistentMember("_Specializations")]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Association> Specializations
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Specializations.ToThreadSafeSet();
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <exclude>Excluded</exclude>
        protected Member<Association> _General = new Member<Association>();
        [Association("SpecializedAssociations", typeof(Association), Roles.RoleA, "04b2946e-5c81-41d5-8d07-3d1b0fae5e56")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_General")]
        [RoleAMultiplicityRange(0, 1)]
        public OOAdvantech.MetaDataRepository.Association General
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _General.Value;
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
                    if (_General.Value == value)
                        return;

                    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        _General.Value = value;
                        StateTransition.Consistent = true;
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }

        /// <exclude>Excluded</exclude>
        MetaDataRepository.AssociationType _MultiplicityType = AssociationType.None;
        /// <MetaDataID>{FE271BE5-29A0-4E2D-ACDB-8EFFDC685D2B}</MetaDataID>
        public virtual MetaDataRepository.AssociationType MultiplicityType
        {
            get
            {
                lock (LockObject)
                {
                    if (_MultiplicityType != AssociationType.None)
                        return _MultiplicityType;


                    //ReaderWriterLock.AcquireReaderLock(10000);
                    //try
                    //{
                    if (RoleA == null || RoleB == null)
                        return AssociationType.None;

                    if (RoleA.Multiplicity.IsMany)
                    {
                        if (RoleB.Multiplicity.IsMany)
                        {
                            _MultiplicityType = AssociationType.ManyToMany;
                        }
                        else
                        {
                            _MultiplicityType = AssociationType.ManyToOne;
                        }
                    }
                    else
                    {
                        if (RoleB.Multiplicity.IsMany)
                        {
                            _MultiplicityType = AssociationType.OneToMany;
                        }
                        else
                        {
                            _MultiplicityType = AssociationType.OneToOne;
                        }
                    }

                    return _MultiplicityType;
                    //}
                    //finally
                    //{
                    //    ReaderWriterLock.ReleaseReaderLock();
                    //}
                }
            }

        }



        /// <MetaDataID>{A22C1D7B-6CF5-4A6F-8CFD-77FD2888E154}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<AssociationEnd> ConnectionsForClients;







        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E51DB26D-F9EA-417C-ACA5-9B201C95F3D0}</MetaDataID>
        protected AssociationEnd _RoleA;
        /// <summary>Specifies an AssociationEnd  as being Role A in association </summary>
        /// <MetaDataID>{6DB8C5A6-069E-4EAF-8534-D0C9AB76BBD1}</MetaDataID>
        public virtual AssociationEnd RoleA
        {

            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_RoleA != null)
                        return _RoleA;
                    foreach (AssociationEnd CurrAssociationEnd in Connections)
                    {
                        if (CurrAssociationEnd.IsRoleA)
                            _RoleA = CurrAssociationEnd;
                        else
                            _RoleB = CurrAssociationEnd;

                    }
                    return _RoleA;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{61014FF2-1A33-4281-9B9F-566489F8B72C}</MetaDataID>
        protected AssociationEnd _RoleB;
        /// <summary>Specifies an AssociationEnd  as being Role B in association </summary>
        /// <MetaDataID>{58F29D72-3AE6-4651-A793-23E1408469C7}</MetaDataID>
        public virtual AssociationEnd RoleB
        {

            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_RoleB != null)
                        return _RoleB;


                    foreach (AssociationEnd CurrAssociationEnd in Connections)
                    {
                        if (!CurrAssociationEnd.IsRoleA)
                            _RoleB = CurrAssociationEnd;
                        else
                            _RoleA = CurrAssociationEnd;

                    }
                    return _RoleB;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<AssociationEnd> _Connections = new OOAdvantech.Collections.Generic.Set<AssociationEnd>();
        /// <summary>An Association consists of exactly two AssociationEnds, each of which represents a connection of the association to a Classifier. Each
        /// AssociationEnd specifies a set of properties that must be fulfilled for the relationship to be valid. The bulk of the structure of an Association is defined by its AssociationEnds. The classifiers belonging to the association are related
        /// to the AssociationEnds by the participant rolename association. </summary>
        /// <MetaDataID>{5F7BE3BC-C150-4725-BDAB-58D177E6980D}</MetaDataID>
        [Association("AssociationEndPoint", typeof(OOAdvantech.MetaDataRepository.AssociationEnd), Roles.RoleA, "{C9B51DC4-1D24-44EB-B343-6F48A766C46D}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_Connections")]
        [RoleAMultiplicityRange(2, 2)]
        public virtual OOAdvantech.Collections.Generic.Set<AssociationEnd> Connections
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (ConnectionsForClients != null)
                        return ConnectionsForClients;
                    else
                    {
                        if (_Connections.Count != 2)
                            return _Connections.ToThreadSafeSet();
                        ConnectionsForClients = _Connections.ToThreadSafeSet();
                        return ConnectionsForClients;
                    }
                }
                finally
                {


                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{4D178572-A2F0-44C0-9761-B20C02EC4E06}</MetaDataID>
        [Association("LinksInstanceTypeRelation", typeof(StorageCellsLink), Roles.RoleA, "{E5C85CF2-{ECFC605B-F4AA-41B4-BD22-130AC73F5BEA}")]
        [PersistentMember("_StorageCellsLinks")]
        [RoleAMultiplicityRange(0)]
        public Collections.Generic.Set<StorageCellsLink> StorageCellsLinks;

        /// <MetaDataID>{8a66a067-b035-4082-a797-89d5b9059b3e}</MetaDataID>
        public Collections.Generic.Set<StorageCellsLink> HierarchyStorageCellsLinks
        {
            get
            {
                if (Specializations.Count == 0)
                    return StorageCellsLinks;
                else
                {
                    Collections.Generic.Set<StorageCellsLink> storageCellsLinks = new OOAdvantech.Collections.Generic.Set<StorageCellsLink>();
                    storageCellsLinks.AddRange(StorageCellsLinks);


                    foreach (Association specializedAssociation in Specializations)
                        storageCellsLinks.AddRange(specializedAssociation.StorageCellsLinks);
                    return storageCellsLinks;
                }




            }
        }




        /// <MetaDataID>{84197FBF-9698-4711-934B-503CA7C7E1B5}</MetaDataID>
        bool HasPersistentRealization(AssociationEnd associationEnd)
        {
            if (associationEnd.Persistent == false)
            {
                if (associationEnd.Navigable)
                {
                    foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
                    {
                        if (associationEndRealization.Persistent)
                            return true;
                    }
                }
                return false;
            }
            else
                return true;

        }
        /// <summary>This flag is true if it is possible to be persistent object links of this association. 
        /// This can be happen when you declare AssociationeEnd or AssociationeEndRealization persistent. </summary>
        /// <MetaDataID>{358FBC28-C7E1-411B-97D5-4FD5760EE3FD}</MetaDataID>
        public bool HasPersistentObjectLink
        {
            set
            {
            }
            get
            {
                if (HasPersistentRealization(RoleA) || HasPersistentRealization(RoleB))
                    return true;
                else
                    return false;
            }
        }
        /// <MetaDataID>{41E3FF3B-F3D8-48D8-9F77-DCFE8C575552}</MetaDataID>
        public Association()
        {

        }
        /// <MetaDataID>{10B5104B-E9F5-485B-A698-7B34FCE54C24}</MetaDataID>
        public Association(string name, Classifier roleAClassifier, string roleAName, Classifier roleBClassifier, string roleBName)
        {
            Name = name;

            PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(this);

            //string name,Classifier specification
            System.Type[] ctorTypes = new System.Type[3] { typeof(string), typeof(Classifier), typeof(Roles) };//
            AssociationEnd roleA = objectStorage.NewObject(typeof(AssociationEnd), ctorTypes, roleAName, roleAClassifier, Roles.RoleA) as AssociationEnd;
            AssociationEnd roleB = objectStorage.NewObject(typeof(AssociationEnd), ctorTypes, roleBName, roleBClassifier, Roles.RoleB) as AssociationEnd;
            _Connections.Add(roleA);
            _Connections.Add(roleB);
            roleA.SetAssociation(this);
            roleB.SetAssociation(this);
        }

        /// <MetaDataID>{5efa8666-8168-4e79-874c-a73283a5433f}</MetaDataID>
        public Association(string name, AssociationEnd roleA, AssociationEnd roleB, string identity)
        {
            try
            {
                if (roleA.Role != OOAdvantech.MetaDataRepository.Roles.RoleA)
                    throw new System.ArgumentException("The association end roleA has RoleB definition", "roleA");
                if (roleB.Role != OOAdvantech.MetaDataRepository.Roles.RoleB)
                    throw new System.ArgumentException("The association end roleB has RoleA definition", "roleB");

                _Name = name;
                if (identity != null)
                    _Identity = new MetaObjectID(identity);


                _Connections.Add(roleA);
                _Connections.Add(roleB);
                roleA.SetAssociation(this);
                roleB.SetAssociation(this);
            }
            catch (System.Exception error)
            {
                throw;
            }
        }




        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        /// <MetaDataID>{362118F5-0F4B-42F0-8446-78F5635761B5}</MetaDataID>
        public override void Synchronize(MetaObject originMetaObject)
        {


            if (SynchronizerSession.IsSynchronized(this))
                return;
            _RoleA = null;
            _RoleB = null;

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {


                MetaDataRepository.Association originAssociation = (MetaDataRepository.Association)originMetaObject;
                //PersistenceLayer.PersistencyContext AppPersistencyContext=PersistenceLayer.PersistencyContext.CurrentPersistencyContext;
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    base.Synchronize(originMetaObject);
                    if (RoleA == null)
                    {
                        AssociationEnd theAssociationEnd = null;

                        theAssociationEnd = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociation.RoleA.Identity.ToString(), this) as MetaDataRepository.AssociationEnd;


                        if (theAssociationEnd == null)
                            theAssociationEnd = (AssociationEnd)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAssociation.RoleA, this);

                        _Connections.Add(theAssociationEnd);
                        theAssociationEnd.IsRoleA = true;
                    }
                    else
                    {
                        if (RoleA.Identity != originAssociation.RoleA.Identity)
                            throw new System.Exception("You can not change AssociationEnd"); //Message
                    }

                    if (RoleB == null)
                    {
                        AssociationEnd theAssociationEnd = null;
                        theAssociationEnd = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociation.RoleB.Identity.ToString(), this) as MetaDataRepository.AssociationEnd;
                        /*Collections.StructureSet aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).Execute("SELECT AssociationEnd FROM "+typeof(MetaDataRepository.AssociationEnd).FullName+" AssociationEnd WHERE MetaObjectIDStream = \""+OriginAssociation.RoleB.Identity.ToString()+"\" ");
                        foreach( Collections.StructureSet Rowset  in aStructureSet)
                        {
                            theAssociationEnd=(MetaDataRepository.AssociationEnd)Rowset.Members["AssociationEnd"].Value;
                            break;
                        }*/
                        if (theAssociationEnd == null)
                            theAssociationEnd = (AssociationEnd)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAssociation.RoleB, this);

                        _Connections.Add(theAssociationEnd);
                        theAssociationEnd.IsRoleA = false;
                    }
                    else
                    {
                        if (RoleB.Identity != originAssociation.RoleB.Identity)
                            throw new System.Exception("You can not change AssociationEnd"); //Message
                    }
                    SynchronizerSession.MetaObjectUnderSynchronization(this);

                    if (_LinkClass == null && originAssociation.LinkClass != null)
                    {
                        if (Properties != null)
                        {
                            var objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
                            if (objectStorage != null)
                            {
                                Collections.StructureSet aStructureSet = objectStorage.Execute("SELECT Class FROM " + typeof(MetaDataRepository.Class).FullName + " Class WHERE Class.MetaObjectIDStream = \"" + originAssociation.LinkClass.Identity.ToString() + "\" ");
                                foreach (Collections.StructureSet Rowset in aStructureSet)
                                {
                                    _LinkClass = (MetaDataRepository.Class)Rowset["Class"];
                                    break;
                                }
                            }
                        }
                        if (_LinkClass == null)
                        {
                            _LinkClass = (MetaDataRepository.Classifier)MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociation.LinkClass, this) as Classifier;
                            if (_LinkClass == null)
                                _LinkClass = (MetaDataRepository.Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAssociation.LinkClass, this);
                        }

                        _LinkClass.LinkAssociation = this;
                    }


                    RoleA.Synchronize(originAssociation.RoleA);
                    RoleB.Synchronize(originAssociation.RoleB);

                    if (originAssociation.General != null)
                    {
                        foreach (MetaDataRepository.AssociationEnd associationEnd in (RoleA.Namespace as MetaDataRepository.Classifier).GetRoles(true))
                        {

                            if (associationEnd.Association != null && associationEnd.Association.Identity == originAssociation.General.Identity)
                            {
                                _General.Value = associationEnd.Association;
                                break;
                            }
                        }
                        if (_General.Value == null)
                        {
                            foreach (MetaDataRepository.AssociationEnd associationEnd in (RoleB.Namespace as MetaDataRepository.Classifier).GetRoles(true))
                            {

                                if (associationEnd.Association != null && associationEnd.Association.Identity == originAssociation.General.Identity)
                                {
                                    _General.Value = associationEnd.Association;
                                    break;
                                }
                            }
                        }
                        //_General.AddSpecialization(this); 
                    }
                    _MultiplicityType = AssociationType.None;
                    _MultiplicityType = MultiplicityType;
                    StateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <MetaDataID>{E43E3ADF-1914-4B3D-A168-74134F9D9DF6}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }


        /// <MetaDataID>{c2386625-6c2a-4555-bd9f-b662409ae057}</MetaDataID>
        internal bool IsGeneral(Association association)
        {
            if (General == null)
                return false;
            if (General == association)
                return true;
            return General.IsGeneral(association);
        }
    }
}
