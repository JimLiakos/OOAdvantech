namespace OOAdvantech.MetaDataRepository
{
    using System;
    using Transactions;

    /// <MetaDataID>{F3D343D7-7DD1-4C50-A2BD-0B9DFF7B2105}</MetaDataID>
    /// <summary>An association end is an endpoint of an association, which connects the association to a classifier. Each association end is part of one association.
    /// In the metamodel, an AssociationEnd is part of an Association and specifies the connection of an Association to a Classifier. It has a name and defines a set of properties of the connection (e.g., which Classifier the Instances must conform to, their multiplicity, and if they may be reached from another Instance via this connection). </summary>
    [BackwardCompatibilityID("{F3D343D7-7DD1-4C50-A2BD-0B9DFF7B2105}")]
    [Persistent()]
    public class AssociationEnd : MetaObject
    {

        /// <exclude>Excluded</exclude>
        bool _Multilingual;

        /// <MetaDataID>{8e152cc8-e633-41a0-ac90-2d92f7963634}</MetaDataID>
        /// <summary>Declare that the value of attribute of the objects will be Multilingual. </summary>
        [PersistentMember(nameof(_Multilingual))]
        [BackwardCompatibilityID("+34")]
        public virtual bool Multilingual
        {
            get => _Multilingual;
            set
            {
                if (_Multilingual != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Multilingual = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{19eb64ec-6e33-4537-91ff-03010e695c93}</MetaDataID>
        public virtual object GetObjectStateValue(object _object)
        {

            throw new NotImplementedException();
        }

        /// <MetaDataID>{98840bc2-9b88-40e0-bf79-9e5dca6c142f}</MetaDataID>
        public virtual void SetObjectStateValue(object _object, object value)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{8af3df14-f99d-43e7-b18c-991457865a9c}</MetaDataID>
        public virtual object GetValue(object _object)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{b3b20118-5b16-4de8-87b5-63dbc373bebc}</MetaDataID>
        public virtual void SetValue(object _object, object value)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{ef1de372-e391-4438-84e8-a78f58f6cfec}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_AllowTransient))
            {
                if (value == null)
                    _AllowTransient = default(bool);
                else
                    _AllowTransient = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Indexer))
            {
                if (value == null)
                    _Indexer = default(bool);
                else
                    _Indexer = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_CollectionClassifier))
            {
                if (value == null)
                    _CollectionClassifier = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _CollectionClassifier = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_HasBehavioralSettings))
            {
                if (value == null)
                    _HasBehavioralSettings = default(bool);
                else
                    _HasBehavioralSettings = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Persistent))
            {
                if (value == null)
                    _Persistent = default(bool);
                else
                    _Persistent = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_AssociationEndRealizations))
            {
                if (value == null)
                    _AssociationEndRealizations = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEndRealization>);
                else
                    _AssociationEndRealizations = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEndRealization>)value;
                _HasLazyFetchingRealization = (bool?)null;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_IsRoleA))
            {
                if (value == null)
                    _IsRoleA = default(bool);
                else
                    _IsRoleA = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LazyFetching))
            {
                if (value == null)
                    _LazyFetching = default(bool);
                else
                    _LazyFetching = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_TryOnObjectActivationFetching))
            {
                if (value == null)
                    _TryOnObjectActivationFetching = default(bool);
                else
                    _TryOnObjectActivationFetching = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_CascadeDelete))
            {
                if (value == null)
                    _CascadeDelete = default(bool);
                else
                    _CascadeDelete = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ReferentialIntegrity))
            {
                if (value == null)
                    _ReferentialIntegrity = default(bool);
                else
                    _ReferentialIntegrity = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Multiplicity))
            {
                if (value == null)
                    _Multiplicity = default(OOAdvantech.MetaDataRepository.MultiplicityRange);
                else
                    _Multiplicity = (OOAdvantech.MetaDataRepository.MultiplicityRange)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Setter))
            {
                if (value == null)
                    _Setter = default(OOAdvantech.MetaDataRepository.Operation);
                else
                    _Setter = (OOAdvantech.MetaDataRepository.Operation)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Getter))
            {
                if (value == null)
                    _Getter = default(OOAdvantech.MetaDataRepository.Operation);
                else
                    _Getter = (OOAdvantech.MetaDataRepository.Operation)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Navigable))
            {
                if (value == null)
                    _Navigable = default(bool);
                else
                    _Navigable = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Association))
            {
                if (value == null)
                    _Association = default(OOAdvantech.MetaDataRepository.Association);
                else
                    _Association = (OOAdvantech.MetaDataRepository.Association)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Specification))
            {
                if (value == null)
                    _Specification = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _Specification = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(TransactionalMember))
            {
                if (value == null)
                    TransactionalMember = default(bool);
                else
                    TransactionalMember = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(TheOtherEnd))
            {
                if (value == null)
                    TheOtherEnd = default(OOAdvantech.MetaDataRepository.AssociationEnd);
                else
                    TheOtherEnd = (OOAdvantech.MetaDataRepository.AssociationEnd)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{2c896ce1-16d2-4232-8b61-b976356addde}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_AllowTransient))
                return _AllowTransient;

            if (member.Name == nameof(_Indexer))
                return _Indexer;

            if (member.Name == nameof(_CollectionClassifier))
                return _CollectionClassifier;

            if (member.Name == nameof(_HasBehavioralSettings))
                return _HasBehavioralSettings;

            if (member.Name == nameof(_Persistent))
                return _Persistent;

            if (member.Name == nameof(_AssociationEndRealizations))
                return _AssociationEndRealizations;

            if (member.Name == nameof(_IsRoleA))
                return _IsRoleA;

            if (member.Name == nameof(_LazyFetching))
                return _LazyFetching;

            if (member.Name == nameof(_TryOnObjectActivationFetching))
                return _TryOnObjectActivationFetching;

            if (member.Name == nameof(_CascadeDelete))
                return _CascadeDelete;

            if (member.Name == nameof(_ReferentialIntegrity))
                return _ReferentialIntegrity;

            if (member.Name == nameof(_Multiplicity))
                return _Multiplicity;

            if (member.Name == nameof(_Setter))
                return _Setter;

            if (member.Name == nameof(_Getter))
                return _Getter;

            if (member.Name == nameof(_Navigable))
                return _Navigable;

            if (member.Name == nameof(_Association))
                return _Association;

            if (member.Name == nameof(_Specification))
                return _Specification;

            if (member.Name == nameof(TransactionalMember))
                return TransactionalMember;

            if (member.Name == nameof(TheOtherEnd))
                return TheOtherEnd;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        protected bool _AllowTransient;
        ///<summary>
        /// Allow objects link where the first is presistent and second is transient under presistent association
        ///</summary>
        /// <MetaDataID>{f9029eff-b090-4296-a0bf-4e4c2f4c4a4e}</MetaDataID>
        [PersistentMember("_AllowTransient"), BackwardCompatibilityID("+33")]
        public virtual bool AllowTransient
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (!_HasBehavioralSettings)
                        throw new System.NotSupportedException("It hasn't BehavioralSettings");
                    return _AllowTransient;
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

                    _AllowTransient = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{c900bdfc-370b-4471-824e-28089dca8ed9}</MetaDataID>
        public override Component ImplementationUnit
        {
            get
            {
                if (Navigable)
                {

                    if (Namespace != null)
                        return Namespace.ImplementationUnit;
                    else
                        return base.ImplementationUnit;
                }
                else
                    return null;
            }
        }
        /// <exclude>Excluded</exclude>
        protected bool _Indexer = false;

        /// <MetaDataID>{ec3274f7-e5b8-4ca5-b6e0-18c0d599720e}</MetaDataID>
        [PersistentMember("_Indexer")]
        [BackwardCompatibilityID("+32")]
        public virtual bool Indexer
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Indexer;

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
                    _Indexer = value;

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }


        /// <MetaDataID>{2ef8cc46-a593-43df-882f-946e0276d305}</MetaDataID>
        public Collections.Generic.List<ObjectIdentityType> GetReferenceObjectIdentityTypes(System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes)
        {
            Collections.Generic.List<ObjectIdentityType> referenceObjectIdentityTypes = new OOAdvantech.Collections.Generic.List<ObjectIdentityType>();
            if (objectIdentityTypes.Count == 0)
            {
                throw new System.Exception("There aren't ObjectIdentityTypes");
                //System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                //foreach (IIdentityPart part in GetReferenceObjectIdentityType().Parts)
                //    parts.Add(new IdentityPart(part.Name, part.PartTypeName, part.Type));
                //referenceObjectIdentityTypes.Add(new ObjectIdentityType(parts));
            }
            else
            {
                foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in objectIdentityTypes)
                {
                    referenceObjectIdentityTypes.Add(GetReferenceObjectIdentityType(objectIdentityType));
                    //System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                    //foreach (IIdentityPart part in objectIdentityType.Parts)
                    //{
                    //    if (IsRoleA)
                    //        parts.Add(new IdentityPart(Association.CaseInsensitiveName + "_" + part.Name + "B", part.PartTypeName, part.Type));
                    //    else
                    //        parts.Add(new IdentityPart(Association.CaseInsensitiveName + "_" + part.Name + "A", part.PartTypeName, part.Type));
                    //}
                    //referenceObjectIdentityTypes.Add(new ObjectIdentityType(parts));
                }
            }
            return referenceObjectIdentityTypes;
        }

        /// <MetaDataID>{34279987-01da-4830-9f50-74619ca9728e}</MetaDataID>
        public string ReferenceStorageIdentityColumnName
        {
            get
            {
                if (IsRoleA)
                    return Association.CaseInsensitiveName + "_StorageIdentityB";
                else
                    return Association.CaseInsensitiveName + "_StorageIdentityA";
            }
        }
        /// <MetaDataID>{6d803fdc-8387-446a-a462-3abb72925660}</MetaDataID>
        public ObjectIdentityType GetReferenceObjectIdentityType(ObjectIdentityType objectIdentityType)
        {

            if (objectIdentityType == null)
            {
                throw new System.Exception("There isn't ObjectIdentityType");
                //System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                //foreach (IIdentityPart part in GetReferenceObjectIdentityType().Parts)
                //    parts.Add(new IdentityPart(part.Name, part.PartTypeName, part.Type));
                //referenceObjectIdentityTypes.Add(new ObjectIdentityType(parts));
            }
            else
            {
                System.Collections.Generic.List<IIdentityPart> parts = new System.Collections.Generic.List<IIdentityPart>();
                foreach (IIdentityPart part in objectIdentityType.Parts)
                {
                    if (IsRoleA)
                        parts.Add(new IdentityPart(Association.CaseInsensitiveName + "_" + part.Name + "B", part.PartTypeName, part.Type));
                    else
                        parts.Add(new IdentityPart(Association.CaseInsensitiveName + "_" + part.Name + "A", part.PartTypeName, part.Type));
                }
                return new ObjectIdentityType(parts);
            }

        }

        /// <exclude>Excluded</exclude>
        protected Classifier _CollectionClassifier = null;
        /// <MetaDataID>{1a79dbe8-a2bc-41cd-aff2-f2ebd8c8e9b8}</MetaDataID>
        public virtual Classifier CollectionClassifier
        {
            get
            {
                return _CollectionClassifier;
            }
        }

        /// <MetaDataID>{04D024F9-C841-4E78-A71E-8C17B667787A}</MetaDataID>
        public MetaDataRepository.Roles Role
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (IsRoleA)
                        return Roles.RoleA;
                    else
                        return Roles.RoleB;

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

                    if (value == Roles.RoleA)
                        _IsRoleA = true;
                    else
                        _IsRoleA = false;

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{FD855B48-58E3-42CA-B60A-E99C4C15D563}</MetaDataID>
        protected bool _HasBehavioralSettings = false;
        /// <MetaDataID>{BC50F1B3-AC99-4C64-92EB-8AFDB785EA6D}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_HasBehavioralSettings")]
        public bool HasBehavioralSettings
        {
            get
            {
                return _HasBehavioralSettings;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4C563316-B9C9-4B1D-831A-F6991F7430D4}</MetaDataID>
        protected bool _Persistent = false;
        /// <summary>This flag is true if AssociatonEnd declared as persistent or some of the AssociatonEndRealization. </summary>
        /// <MetaDataID>{7AB62057-D861-4618-B2A1-779877CCE1ED}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [PersistentMember("_Persistent")]
        public virtual bool Persistent
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Persistent;
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
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        _Persistent = value;
                        stateTransition.Consistent = true;
                    }

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }

        }


        bool? _HasLazyFetchingRealization;
        /// <MetaDataID>{4852DEA5-EA41-47EB-9C1C-D861846038DD}</MetaDataID>
        public bool HasLazyFetchingRealization
        {
            get
            {
                if (_HasLazyFetchingRealization.HasValue)
                    return _HasLazyFetchingRealization.Value;
                if (HasBehavioralSettings && !LazyFetching)
                {
                    _HasLazyFetchingRealization = false;
                    return false;
                }

                foreach (MetaDataRepository.AssociationEndRealization associationEndRealization in AssociationEndRealizations)
                {
                    if (associationEndRealization.HasBehavioralSettings && !associationEndRealization.LazyFetching)
                    {
                        _HasLazyFetchingRealization = false;
                        return false;
                    }

                }
                _HasLazyFetchingRealization = true;
                return true;
            }
        }

        object AssociationEndRealizationsCacheLock = new object();
        OOAdvantech.Collections.Generic.Set<AssociationEndRealization> AssociationEndRealizationsCache;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{76BFC65D-097E-4F01-ACFE-9700E6512964}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<AssociationEndRealization> _AssociationEndRealizations = new OOAdvantech.Collections.Generic.Set<AssociationEndRealization>();
        /// <MetaDataID>{FF75E099-CF46-4BA4-B0B2-825E085EF3B2}</MetaDataID>
        [Association("AssociationEndRealization", typeof(OOAdvantech.MetaDataRepository.AssociationEndRealization), Roles.RoleB, "{B48643D9-F660-492C-852D-E150BBE16B37}")]
        [PersistentMember("_AssociationEndRealizations")]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<AssociationEndRealization> AssociationEndRealizations
        {
            get
            {

                lock (AssociationEndRealizationsCacheLock)
                {
                    if (AssociationEndRealizationsCache != null)
                        return AssociationEndRealizationsCache;
                }
                if (Navigable)
                {
                    foreach (Classifier classifier in (Namespace as Classifier).GetAllSpecializeClasifiers())
                    {
                        int featuresCount = classifier.GetFeatures(true).Count;
                    }
                }
                lock (AssociationEndRealizationsCacheLock)
                {
                    AssociationEndRealizationsCache = _AssociationEndRealizations.ToThreadSafeSet();
                }
                return AssociationEndRealizationsCache;
            }
        }


        protected internal override void SetNamespace(Namespace mNamespace)
        {

            if (Namespace is Classifier)
                (Namespace as Classifier).ClassHierarchyChanged -= AssociationEnd_ClassHierarchyChanged;

            base.SetNamespace(mNamespace);
            if (Namespace is Classifier)
                (Namespace as Classifier).ClassHierarchyChanged += AssociationEnd_ClassHierarchyChanged;
        }

        private void AssociationEnd_ClassHierarchyChanged(object sender)
        {
            lock (AssociationEndRealizationsCacheLock)
            {
                AssociationEndRealizationsCache = null;
            }
        }


        /// <MetaDataID>{7bec15a0-6ee8-4f1d-913e-674c5c59dc91}</MetaDataID>
        public virtual bool HasAssociationEndRealizations
        {
            get
            {
                return AssociationEndRealizations.Count != 0;
            }
        }

        /// <MetaDataID>{3D039476-4D83-4E20-93AB-C3B32BD2BDB1}</MetaDataID>
        public void AddAssociationEndRealization(AssociationEndRealization associationEndRealization)
        {
            _HasLazyFetchingRealization = (bool?)null;
            lock (AssociationEndRealizationsCacheLock)
            {
                AssociationEndRealizationsCache = null;
            }
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
            {
                _AssociationEndRealizations.Add(associationEndRealization);
                stateTransition.Consistent = true;
            }


        }
        /// <MetaDataID>{2F8A09B4-C8D8-4614-9554-F60F2E7E2C24}</MetaDataID>
        public void RemoveAssociationEndRealization(AssociationEndRealization associationEndRealization)
        {
            _HasLazyFetchingRealization = (bool?)null;
            lock (AssociationEndRealizationsCacheLock)
            {
                AssociationEndRealizationsCache = null;
            }

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
            {
                _AssociationEndRealizations.Remove(associationEndRealization);
                stateTransition.Consistent = true;
            }


        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B950D3DF-C0D4-4843-AE02-5CEC3D0A79ED}</MetaDataID>
        protected bool _IsRoleA = false;
        /// <MetaDataID>{F272DFBB-730E-4E2A-89A0-6EFB9F17E7D0}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        [PersistentMember("_IsRoleA")]
        public bool IsRoleA
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _IsRoleA;
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

                    _IsRoleA = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }
        /// <MetaDataID>{59BA3D0F-E57F-42BC-A7EF-F3EA625FC7BE}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected bool _LazyFetching = true;
        /// <MetaDataID>{A5EB10AC-37AF-461C-834D-2B6C1298D046}</MetaDataID>
        [BackwardCompatibilityID("+20")]
        [PersistentMember("_LazyFetching")]
        public virtual bool LazyFetching
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    //if(!_HasBehavioralSettings)
                    //    throw new System.NotSupportedException("It doesn't has BehavioralSettings");
                    return _LazyFetching;
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

                    _LazyFetching = true;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }


        /// <exclude>Excluded</exclude>
        protected bool _TryOnObjectActivationFetching;
        /// <MetaDataID>{6bb8f820-20eb-4a97-9d34-fb9d43ef016a}</MetaDataID>
        [BackwardCompatibilityID("+21")]
        [PersistentMember("_TryOnObjectActivationFetching")]
        public bool TryOnObjectActivationFetching
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _TryOnObjectActivationFetching;
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

                    _TryOnObjectActivationFetching = true;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }

        /// <MetaDataID>{4F37530E-B4A2-4877-9837-868C604B250D}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected bool _CascadeDelete = false;
        /// <MetaDataID>{E1D5A593-6BA2-468A-A5A9-75C8C23E51FA}</MetaDataID>
        [BackwardCompatibilityID("+18")]
        [PersistentMember("_CascadeDelete")]
        public virtual bool CascadeDelete
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (!_HasBehavioralSettings)
                        throw new System.NotSupportedException("It hasn't BehavioralSettings");
                    return _CascadeDelete;
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

                    _CascadeDelete = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{50CA24D6-8FCF-4285-B916-8DC99C787287}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected bool _ReferentialIntegrity = false;
        /// <MetaDataID>{03FC4C37-66BF-463D-9BA4-52022D14D020}</MetaDataID>
        [BackwardCompatibilityID("+19")]
        [PersistentMember("_ReferentialIntegrity")]
        public virtual bool ReferentialIntegrity
        {

            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (!_HasBehavioralSettings)
                        throw new System.NotSupportedException("It hasn't BehavioralSettings");
                    return _ReferentialIntegrity;
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

                    _ReferentialIntegrity = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }


        /// <MetaDataID>{4F24C98F-6B05-4C69-AC36-6BBAA3F7E98C}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected MultiplicityRange _Multiplicity;
        /// <MetaDataID>{D46B0276-0C55-4273-A95F-610DCA6C60E1}</MetaDataID>
        /// <summary>When placed on a target end, specifies the number of target instances that may be associated with a single source instance across the given Association. </summary>
        [Association("AssociationEndMultiplicity", typeof(OOAdvantech.MetaDataRepository.MultiplicityRange), Roles.RoleA, "{85CEEE4B-DC2A-46F4-83EA-6199F9F028E5}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Multiplicity")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public virtual MultiplicityRange Multiplicity
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (_Multiplicity == null)
                    {
                        //PersistenceLayer.PersistencyContext AppPersistencyContext=PersistenceLayer.PersistencyContext.CurrentPersistencyContext;
                        //long ObjectStateTransitionID=AppPersistencyContext.BeginObjectStateTransition(this,Transactions.TransactionOption.Required);
                        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            if (PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) != null)
                                _Multiplicity = (MultiplicityRange)PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(MultiplicityRange));
                            else
                                _Multiplicity = new MultiplicityRange();


                            StateTransition.Consistent = true; ;
                        }
                        //AppPersistencyContext.CommitObjectStateTransition(this,ObjectStateTransitionID);
                    }
                    return _Multiplicity;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{18960E1A-5557-453C-999B-88F0F7644756}</MetaDataID>
        internal protected void SetSpecification(Classifier value)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (_Specification != value)
                {
                    if (_Specification != null)
                        _Specification.RemoveRole(this);
                    if (value != null)
                        value.AddRole(this);
                }
                _Specification = value;

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{fd49dc6f-731d-486b-b50a-70f681d2691d}</MetaDataID>
        internal void SetAssociation(Association value)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (_Association != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                    {

                        _Association = value;
                        TheOtherEnd = null;
                        stateTransition.Consistent = true;
                    }
                }


            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }


        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        /// <MetaDataID>{CCBE7A75-A6B0-4A02-BA50-2D7DDFB2F3E4}</MetaDataID>
        public override void Synchronize(MetaObject OriginMetaObject)
        {

            if (SynchronizerSession.IsSynchronized(this))
                return;

            base.Synchronize(OriginMetaObject);

            MetaDataRepository.AssociationEnd originAssociationEnd = (MetaDataRepository.AssociationEnd)OriginMetaObject;
            using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    _Persistent = originAssociationEnd.Persistent;
                    Navigable = originAssociationEnd.Navigable;
                    _HasBehavioralSettings = originAssociationEnd.HasBehavioralSettings;
                    if (_HasBehavioralSettings)
                    {
                        _CascadeDelete = originAssociationEnd.CascadeDelete;
                        _ReferentialIntegrity = originAssociationEnd.ReferentialIntegrity;
                        _LazyFetching = originAssociationEnd.LazyFetching;
                        _AllowTransient = originAssociationEnd.AllowTransient;
                    }



                    if (_Association == null)
                    {
                        _Association = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Association.Identity.ToString(), this) as MetaDataRepository.Association;
                        if (_Association == null)
                            _Association = (Association)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAssociationEnd.Association, this);
                    }
                    else
                    {
                        if (_Association.Identity != originAssociationEnd.Association.Identity)
                            throw new System.Exception("Association in inconsistent state");
                    }
                    SynchronizerSession.MetaObjectUnderSynchronization(this);

                    if (_Specification == null)
                    {
                        if (originAssociationEnd.Specification != null)
                        {
                            //TODO τι γινεται όταν τα meta data είναι transient

                            _Specification = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Specification, this) as MetaDataRepository.Classifier;
                            if (_Specification == null)
                            {
                                _Specification = (Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAssociationEnd.Specification, this);
                                _Specification.ShallowSynchronize(originAssociationEnd.Specification);
                            }
                            _Specification.AddRole(this);

                        }
                    }

                    if (_Multiplicity == null)
                    {
                        if (PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) != null)
                            _Multiplicity = (MultiplicityRange)PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(MultiplicityRange));
                        else
                            _Multiplicity = new MultiplicityRange();

                    }

                    using (Transactions.ObjectStateTransition InerStateTransition = new OOAdvantech.Transactions.ObjectStateTransition(_Multiplicity, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        _Multiplicity.LowLimit = originAssociationEnd.Multiplicity.LowLimit;
                        _Multiplicity.HighLimit = originAssociationEnd.Multiplicity.HighLimit;
                        _Multiplicity.NoHighLimit = originAssociationEnd.Multiplicity.NoHighLimit;
                        _Multiplicity.Unspecified = originAssociationEnd.Multiplicity.Unspecified;

                        InerStateTransition.Consistent = true; ;
                    }
                    _Indexer = originAssociationEnd.Indexer;
                    Navigable = originAssociationEnd.Navigable;

                }
                catch (System.Exception error)
                {
                    throw;

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

                _Association.Synchronize(originAssociationEnd.Association);
                StateTransition.Consistent = true; ;
            }



        }
        /// <MetaDataID>{2B306D7D-0B27-4638-865C-AE5E31B56869}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Operation _Setter;
        /// <summary>This is the operation through client set the linked object. </summary>
        /// <MetaDataID>{B23EAD5D-0644-4F35-B768-321472735356}</MetaDataID>
        [Association("WriteAssociationEnd", typeof(OOAdvantech.MetaDataRepository.Operation), Roles.RoleA, "{23B4DAFC-8F2D-4BC3-A5A3-4CA95F4DB36A}")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0, 1)]
        public virtual Operation Setter
        {
            get
            {
                return _Setter;
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{11F2B291-1B2A-4966-B165-E4ACD7E7FE6A}</MetaDataID>
        protected Operation _Getter;
        /// <summary>This is the operation through client read the linked Object. </summary>
        /// <MetaDataID>{8676BC47-75D2-4BBA-A328-BA216CF0C838}</MetaDataID>
        [Association("ReadAssociationEnd", typeof(OOAdvantech.MetaDataRepository.Operation), Roles.RoleA, "{4227F265-E6A9-465D-B8FD-16AE14381A1E}")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0, 1)]
        public virtual Operation Getter
        {
            get
            {
                return _Getter;
            }
        }
        /// <MetaDataID>{9de8fba9-9033-4c8e-bec6-ca77fae9e95e}</MetaDataID>
        public override MetaObjectID Identity
        {
            get
            {
                if (Association != null && string.IsNullOrEmpty(MetaObjectIDStream))
                {

                    return new OOAdvantech.MetaDataRepository.MetaObjectID(Association.Identity + Role.ToString());

                }
                return base.Identity;
            }
        }
        /// <MetaDataID>{486BAFBA-1AC0-42D7-A9B9-7575507989A4}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }

        /// <MetaDataID>{c69ed8f8-576b-4dab-99f1-9e97829812bd}</MetaDataID>
        protected bool _Navigable;
        /// <summary>When placed on a target end, specifies whether traversal from a source instance to its associated target instances is possible. Specification of each direction across the Association is independent. A value of true means that
        /// the association can be navigated by the source class and the target rolename can be used in navigation expressions. </summary>
        /// <MetaDataID>{6DD31528-CD6C-42B4-804D-F40B142438C6}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember("_Navigable")]
        public virtual bool Navigable
        {
            get
            {
                return _Navigable;
            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        _Navigable = value;
                        stateTransition.Consistent = true;
                    }

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }

        /// <MetaDataID>{C6CCC0B9-214C-45AE-A618-A7F8CDF6248F}</MetaDataID>
        public AssociationEnd()
        {
            IsRoleA = false;
            _ReferentialIntegrity = false;
            _CascadeDelete = false;
            _LazyFetching = true;
        }
        /// <MetaDataID>{E7F47A65-98A9-443B-A9F8-A70FACB7F1FC}</MetaDataID>
        public AssociationEnd(string name, Classifier specification, Roles roleType)
        {
            Name = name;
            if (roleType == Roles.RoleA)
                _IsRoleA = true;
            else
                _IsRoleA = false;

            _Specification = specification;
            _Specification.AddRole(this);
            _ReferentialIntegrity = false;
            _CascadeDelete = false;
            _LazyFetching = true;


        }
        /// <MetaDataID>{a8e647a7-6083-4111-bbf8-0db9b4ad7789}</MetaDataID>
        protected void ReAssigneRole()
        {
            if (_Specification != null)// &&!_Specification.Roles.Contains(this))
                _Specification.AddRole(this);
        }

        /// <MetaDataID>{8F716BA4-0F0D-4F6F-85AA-9DC6AA28C62A}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Association _Association;
        /// <MetaDataID>{5FCE10AE-2EE9-44AD-89F2-870E9EB30BED}</MetaDataID>
        /// <summary>Specifies an association belonging to the AssociationEnd. </summary>
        [Association("AssociationEndPoint", typeof(OOAdvantech.MetaDataRepository.Association), Roles.RoleB, "{C9B51DC4-1D24-44EB-B343-6F48A766C46D}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Association")]
        [RoleBMultiplicityRange(1, 1)]
        public virtual Association Association
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Association;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }

        }
        /// <MetaDataID>{C2CE7BB9-C230-42BB-AF4C-36522ECF5A99}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Classifier _Specification;
        /// <summary>Designates the Classifier that specify the Operations that may be applied to an Instance accessed by the AssociationEnd across the Association. </summary>
        /// <MetaDataID>{EC563270-A478-4E66-9A60-84175D3E4B77}</MetaDataID>
        [Association("ClassifierRole", typeof(OOAdvantech.MetaDataRepository.Classifier), Roles.RoleA, "{1213BEE4-382C-4EE0-A62B-E013FCA9B4C3}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Specification")]
        [RoleAMultiplicityRange(1, 1)]
        public virtual Classifier Specification
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Specification;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }


        }
        /// <MetaDataID>{7878bf73-ca3c-45ed-9d52-6dcf4f0cb3ad}</MetaDataID>
        public bool TransactionalMember;
        /// <MetaDataID>{b92c504d-f8cf-41f9-b180-6efb63c1430d}</MetaDataID>
        AssociationEnd TheOtherEnd;
        /// <MetaDataID>{522D2B81-23B4-448F-A80F-6E0D1C592738}</MetaDataID>
        public virtual AssociationEnd GetOtherEnd()
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (TheOtherEnd != null)
                    return TheOtherEnd;
                foreach (AssociationEnd CurrAssociationEnd in _Association.Connections)
                {
                    if (CurrAssociationEnd != this)
                    {
                        TheOtherEnd = CurrAssociationEnd;
                        return TheOtherEnd;
                    }
                }
                throw new System.Exception("Association in inconsistent state");
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }
    }
}
