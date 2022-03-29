namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{4CB15518-9079-4AC6-9251-5058EB1274DB}</MetaDataID>
    /// <summary>A term of convenience for a Relationship other than Association, Generalization, Realization or metarelationship (such as the relationship between a Classifier and one of its
    /// Instances).
    /// A dependency states that the implementation or functioning of one or more elements requires the presence of one or more other elements.
    /// In the metamodel, a Dependency is a directed relationship from a client (or clients) to a supplier (or suppliers) stating that the client is dependent on the supplier (i.e., the client element requires the presence and knowledge of the supplier element). The kinds of Dependency are Abstraction, Binding, Permission, and Usage. Various stereotypes of those elements are predefined. </summary>
    [BackwardCompatibilityID("{4CB15518-9079-4AC6-9251-5058EB1274DB}")]
    [Persistent()]
    public class Dependency : Relationship
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Client))
            {
                if (value == null)
                    _Client = default(OOAdvantech.Member<OOAdvantech.MetaDataRepository.MetaObject>);
                else
                    _Client = (OOAdvantech.Member<OOAdvantech.MetaDataRepository.MetaObject>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Supplier))
            {
                if (value == null)
                    _Supplier = default(OOAdvantech.Member<OOAdvantech.MetaDataRepository.MetaObject>);
                else
                    _Supplier = (OOAdvantech.Member<OOAdvantech.MetaDataRepository.MetaObject>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Client))
                return _Client;

            if (member.Name == nameof(_Supplier))
                return _Supplier;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{d2a977e2-0162-42d0-bc8a-26ed6bdecf1e}</MetaDataID>
        protected Dependency()
        {
        }
        /// <MetaDataID>{37bfae34-f11a-49a2-ac4e-9cc39c41ec0a}</MetaDataID>
		public Dependency(string name, MetaObject client, MetaObject supplier)
        {
            if (client == null && supplier == null)
                throw new System.ArgumentNullException("client and supplier");
            if (client == null)
                throw new System.ArgumentNullException("client");
            if (supplier == null)
                throw new System.ArgumentNullException("supplier");

            _Name = name;
            _Client.Value = client;
            _Supplier.Value = supplier;
        }

        /// <MetaDataID>{4C1D3AE5-69F6-4606-A14C-4F1DA8AB96E8}</MetaDataID>
        public override MetaObjectID Identity
        {
            get
            {
                lock (identityLock)
                {
                    if (MetaObjectIDStream != null)
                        if (MetaObjectIDStream.Length > 0)
                            _Identity = new MetaObjectID(MetaObjectIDStream);
                    if (_Identity == null)
                        _Identity = new MetaObjectID(Client.Identity.ToString() + "." + Supplier.Identity.ToString());
                    return _Identity;
                }


            }
        }
        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        /// <MetaDataID>{F9EAABF1-C3DD-4825-9EC8-919C11B5A0D5}</MetaDataID>
        public override void Synchronize(MetaDataRepository.MetaObject OriginMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                base.Synchronize(OriginMetaObject);
                Dependency OriginDependency = (Dependency)OriginMetaObject;
                if (Supplier == null)
                    PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Properties).LazyFetching("Supplier", GetType());
                if (Supplier == null)
                {
                    var objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
                    if (objectStorage != null)
                    {
                        Collections.StructureSet aStructureSet = objectStorage.Execute("SELECT Dependency FROM " + typeof(MetaDataRepository.Dependency).FullName + " Dependency WHERE Dependency.MetaObjectIDStream = \"" + OriginDependency.Supplier.Identity.ToString() + "\" ");
                        foreach (Collections.StructureSet Rowset in aStructureSet)
                        {
                            Supplier = (MetaDataRepository.Classifier)Rowset["Classifier"];
                            break;
                        }
                    }
                    if (Supplier == null)
                        Supplier = (Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(OriginDependency.Supplier, this);
                }
                Supplier.Synchronize(OriginDependency.Supplier);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <MetaDataID>{0432C8D2-8C51-4EFE-8D5E-BC4BE11CAAF1}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }

        /// <MetaDataID>{D4203238-80BA-49BF-8EBC-55F8B4FFA722}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Member<MetaObject> _Client = new Member<MetaObject>();
        /// <summary>The Meta object that is affected by the supplier Meta object. In some cases (such as a Trace Abstraction) the direction is unimportant and serves only to distinguish the two elements. </summary>
        /// <MetaDataID>{157D4296-DA10-4D79-AB67-9713D8F5B4D0}</MetaDataID>
        [Association("ClientDependency", typeof(OOAdvantech.MetaDataRepository.MetaObject), Roles.RoleA, "{54E7CCBA-6C18-4E9A-A69B-7C26B1ACFF41}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_Client")]
        [RoleAMultiplicityRange(1, 1)]
        public virtual MetaObject Client
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Client;
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

                    _Client.Value = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{5BC27BE0-C93D-456C-916D-32E2EC971019}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Member<MetaObject> _Supplier = new Member<MetaObject>();

        /// <summary>Inverse of client. Designates the element that is unaffected by a change. In a two-way relationship (such as some Refinement Abstractions) this would be the more general meta object. In an undirected situation, such as a Trace Abstraction, the choice of client and supplier may be irrelevant. </summary>
        /// <MetaDataID>{0415F06D-20DD-4E93-8A37-3A4AB44C6DB6}</MetaDataID>
        [Association("SupplierDependency", typeof(OOAdvantech.MetaDataRepository.MetaObject), Roles.RoleA, "{945370AF-442A-490A-A336-9EE77D07F621}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_Supplier")]
        [RoleAMultiplicityRange(1, 1)]
        public virtual MetaObject Supplier
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Supplier;
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

                    _Supplier.Value = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
    }
}
