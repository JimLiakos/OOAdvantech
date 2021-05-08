using System.Linq;
using OOAdvantech.Collections.Generic;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{FBA514DB-0444-4524-9775-00CDF1E850E6}</MetaDataID>
    public class Structure : DataType, InterfaceImplementor
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(ClassHierarchyInterfaces))
            {
                if (value == null)
                    ClassHierarchyInterfaces = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Interface>);
                else
                    ClassHierarchyInterfaces = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Interface>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssociationEndsLoadPolicy))
            {
                if (value == null)
                    _AssociationEndsLoadPolicy = default(System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, int>);
                else
                    _AssociationEndsLoadPolicy = (System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, int>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssociationEndsPersistency))
            {
                if (value == null)
                    AssociationEndsPersistency = default(OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, bool>);
                else
                    AssociationEndsPersistency = (OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, bool>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AttributePersistency))
            {
                if (value == null)
                    AttributePersistency = default(OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.Attribute, bool>);
                else
                    AttributePersistency = (OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.Attribute, bool>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssoctionEndReferentialIntegrities))
            {
                if (value == null)
                    AssoctionEndReferentialIntegrities = default(System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, bool>);
                else
                    AssoctionEndReferentialIntegrities = (System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, bool>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Realizations))
            {
                if (value == null)
                    _Realizations = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Realization>);
                else
                    _Realizations = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Realization>)value;
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

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(ClassHierarchyInterfaces))
                return ClassHierarchyInterfaces;

            if (member.Name == nameof(AssociationEndsLoadPolicy))
                return AssociationEndsLoadPolicy;

            if (member.Name == nameof(AssociationEndsPersistency))
                return AssociationEndsPersistency;

            if (member.Name == nameof(AttributePersistency))
                return AttributePersistency;

            if (member.Name == nameof(AssoctionEndReferentialIntegrities))
                return AssoctionEndReferentialIntegrities;

            if (member.Name == nameof(_Realizations))
                return _Realizations;

            if (member.Name == nameof(_Persistent))
                return _Persistent;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{56e9092b-c355-43eb-8eec-1167900f326a}</MetaDataID>
        public Method AddMethod(Operation specification)
        {
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    Method method = new Method(specification);// objectStorage.NewObject(typeof(Method), ctorTypes, specification) as Method;
                    OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    if (objectStorage != null)
                        objectStorage.CommitTransientObjectState(method);
                    stateTransition.Consistent = true;
                    return method;
                }
            }
        }

        /// <MetaDataID>{afabbeaa-53d9-4fb0-971d-72cefb34e9e5}</MetaDataID>
        public AttributeRealization AddAttributeRealization(Attribute specification)
        {
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    AttributeRealization attributeRealization = new AttributeRealization(specification);// objectStorage.NewObject(typeof(Method), ctorTypes, specification) as Method;
                    _Features.Add(attributeRealization);
                    OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    if (objectStorage != null)
                        objectStorage.CommitTransientObjectState(attributeRealization);
                    stateTransition.Consistent = true;
                    return attributeRealization;
                }
            }
        }
        /// <MetaDataID>{ffaf025b-8976-495a-aa21-ab753e1b9134}</MetaDataID>
        public AssociationEndRealization AddAssociationEndRealization(AssociationEnd specification)
        {
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    AssociationEndRealization associationEndRealization = new AssociationEndRealization(specification);// objectStorage.NewObject(typeof(Method), ctorTypes, specification) as Method;
                    _Features.Add(associationEndRealization);
                    OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    if (objectStorage != null)
                        objectStorage.CommitTransientObjectState(associationEndRealization);
                    stateTransition.Consistent = true;
                    return associationEndRealization;
                }
            }
        }
        /// <MetaDataID>{9ea175b6-c21c-488b-ab68-f626d2c879df}</MetaDataID>
        public virtual void RemoveMethod(Method method)
        {
            if (method == null)
                return;
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {

                    method.Specification.RemoveOperationImplementation(method);
                    _Features.Remove(method);
                    stateTransition.Consistent = true;
                }
            }


        }

        /// <MetaDataID>{6ced88b5-a7d3-448e-a19c-77980582cd35}</MetaDataID>
        public virtual void RemoveAttributeRealization(AttributeRealization attributeRealization)
        {
            if (attributeRealization == null)
                return;
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    attributeRealization.Specification.RemoveAttributeRealization(attributeRealization);
                    _Features.Remove(attributeRealization);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{54e51e9e-bc29-4da9-8c20-cc3d9229ad6a}</MetaDataID>
        public virtual void RemoveAssociationEndRealization(AssociationEndRealization associationEndRealization)
        {
            if (associationEndRealization == null)
                return;
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    associationEndRealization.Specification.RemoveAssociationEndRealization(associationEndRealization);
                    _Features.Remove(associationEndRealization);
                    stateTransition.Consistent = true;
                }
            }


        }


        public bool IsMultilingual(Attribute attribute)
        {

            if (attribute.Multilingual)
                return true;
            else
            {
                lock (MembersSpecializationPropertiesLock)
                {
                    bool isMultilingual = false;
                    if (AttributeMultilingualism != null)
                    {
                        if (AttributeMultilingualism.TryGetValue(attribute, out isMultilingual))
                            return isMultilingual;
                        else
                            return false;
                    }
                }

                var attributeMultilingualism = new OOAdvantech.Collections.Generic.Dictionary<Attribute, bool>();
                foreach (AttributeRealization attributeRealization in attribute.AttributeRealizations)
                {
                    lock (MembersSpecializationPropertiesLock)
                    {
                        if (attributeRealization.Multilingual && (attributeRealization.Namespace is Classifier) && IsA(attributeRealization.Namespace as Classifier))
                        {
                            attributeMultilingualism[attribute] = true;
                            break;
                        }
                    }
                }
                //TODO Προυποθέτει ότι έχει φορτοθεί η class πλήρος για την περίπτωση .Net

                foreach (AttributeRealization attributeRealization in Features.OfType<AttributeRealization>())
                    attributeMultilingualism[attributeRealization.Specification] = attributeRealization.Multilingual;

                foreach (Structure _struct in GetAllGeneralClasifiers().OfType<Structure>())
                {
                    foreach (AttributeRealization attributeRealization in Features.OfType<AttributeRealization>())
                        if (!attributeMultilingualism.ContainsKey(attributeRealization.Specification) || attributeMultilingualism[attributeRealization.Specification] == false)
                            attributeMultilingualism[attributeRealization.Specification] = attributeRealization.Multilingual;
                }

                lock (MembersSpecializationPropertiesLock)
                {
                    if (AttributeMultilingualism == null)
                        AttributeMultilingualism = new Dictionary<Attribute, bool>(attributeMultilingualism);
                    bool isMultilingual = false;
                    if (AttributeMultilingualism.TryGetValue(attribute, out isMultilingual))
                        return isMultilingual;
                    else
                    {
                        AttributeMultilingualism[attribute] = attribute.Multilingual;
                        return false;
                    }
                }
            }
        }



        /// <MetaDataID>{e42037a0-c8ea-41d7-b50e-cb56d0e2606f}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<Interface> ClassHierarchyInterfaces = null;
        /// <MetaDataID>{e858fe63-914c-4abd-8778-c5aa57608759}</MetaDataID>
        private System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, int> _AssociationEndsLoadPolicy = null;

        
        private System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, int> AssociationEndsLoadPolicy
        {
            get
            {
                lock (MembersSpecializationPropertiesLock)
                {
                    if (_AssociationEndsLoadPolicy != null)
                        return _AssociationEndsLoadPolicy;
                }

                var associationEndsLoadPolicy = new OOAdvantech.Collections.Generic.Dictionary<AssociationEnd, int>();
                foreach (var associationEndRealization in Features.OfType<AssociationEndRealization>().Where(x => x.HasBehavioralSettings))
                {
                    if (associationEndRealization.TryOnObjectActivationFetching)
                        associationEndsLoadPolicy[associationEndRealization.Specification] = 2;
                    else
                        if (associationEndRealization.LazyFetching)
                        associationEndsLoadPolicy[associationEndRealization.Specification] = 0;
                    else
                        associationEndsLoadPolicy[associationEndRealization.Specification] = 1;
                }
                foreach (Structure _struct in GetAllGeneralClasifiers().OfType<Structure>())
                {
                    foreach (var associationEndRealization in _struct.Features.OfType<AssociationEndRealization>().Where(x => x.HasBehavioralSettings))
                    {
                        if (associationEndRealization.TryOnObjectActivationFetching)
                            associationEndsLoadPolicy[associationEndRealization.Specification] = 2;
                        else
                    if (associationEndRealization.LazyFetching)
                            associationEndsLoadPolicy[associationEndRealization.Specification] = 0;
                        else
                            associationEndsLoadPolicy[associationEndRealization.Specification] = 1;

                    }
                }
                lock (MembersSpecializationPropertiesLock)
                {
                    if (_AssociationEndsLoadPolicy == null)
                        _AssociationEndsLoadPolicy = new Dictionary<AssociationEnd, int>(associationEndsLoadPolicy);
                    return _AssociationEndsLoadPolicy;
                }
            }
        }

        /// <MetaDataID>{f888b7fe-2e3f-42e1-87c0-0e3a198ebb3d}</MetaDataID>
        private Collections.Generic.Dictionary<AssociationEnd, bool> AssociationEndsPersistency = null;
        /// <MetaDataID>{121319cb-62a6-4194-9b19-14112dad7895}</MetaDataID>
        private Collections.Generic.Dictionary<Attribute, bool> AttributePersistency = null;

        /// <MetaDataID>{17309d8a-b48b-4798-8e84-37d93e21fc27}</MetaDataID>
        public bool IsPersistent(Attribute attribute)
        {
            if (attribute.Persistent)
                return true;
            else
            {
                lock (MembersSpecializationPropertiesLock)
                {
                    bool isPersistent = false;
                    if (AttributePersistency != null)
                    {
                        if (AttributePersistency.TryGetValue(attribute, out isPersistent))
                            return isPersistent;
                        else
                            return false;
                    }
                }
                var attributesPersistency = new OOAdvantech.Collections.Generic.Dictionary<Attribute, bool>();
                foreach (AttributeRealization attributeRealization in attribute.AttributeRealizations)
                {
                    if (attributeRealization.Persistent && (attributeRealization.Namespace is Classifier) && IsA(attributeRealization.Namespace as Classifier))
                    {
                        attributesPersistency[attribute] = true;
                        break;
                    }
                }
                //TODO Προυποθέτει ότι έχει φορτωθεί η class πλήρως για την περίπτωση .Net
                foreach (var attributeRealization in Features.OfType<AttributeRealization>())
                    attributesPersistency[attributeRealization.Specification] = attributeRealization.Persistent;

                foreach (Structure _struct in GetAllGeneralClasifiers().OfType<Structure>())
                    foreach (var attributeRealization in _struct.Features.OfType<AttributeRealization>())
                        if (!attributesPersistency.ContainsKey(attributeRealization.Specification) || attributesPersistency[attributeRealization.Specification] == false)
                            attributesPersistency[attributeRealization.Specification] = attributeRealization.Persistent;

                lock (MembersSpecializationPropertiesLock)
                {
                    if (AttributePersistency == null)
                        AttributePersistency = new OOAdvantech.Collections.Generic.Dictionary<Attribute, bool>(attributesPersistency);

                    if (!AttributePersistency.ContainsKey(attribute))
                        AttributePersistency[attribute] = attribute.Persistent;
                    return AttributePersistency[attribute];
                }
            }

        }

        /// <MetaDataID>{8ce0862e-8af7-4b14-b5e2-2b44e6264224}</MetaDataID>
        public bool IsPersistent(AssociationEnd associationEnd)
        {
            if (associationEnd.Persistent)
                return true;
            else
            {

                bool isPersistent = false;
                lock (MembersSpecializationPropertiesLock)
                {
                    if (AssociationEndsPersistency != null)
                    {
                        if (AssociationEndsPersistency.TryGetValue(associationEnd, out isPersistent))
                            return isPersistent;
                        else
                            return false;
                    }
                }
                var associationEndsPersistency = new OOAdvantech.Collections.Generic.Dictionary<AssociationEnd, bool>();
                foreach (var associationEndRealization in Features.OfType<AssociationEndRealization>())
                    associationEndsPersistency[associationEndRealization.Specification] = associationEndRealization.Persistent;

                foreach (Structure _struct in GetAllGeneralClasifiers().OfType<Structure>())
                    foreach (var associationEndRealization in _struct.Features.OfType<AssociationEndRealization>())
                        if (!associationEndsPersistency.ContainsKey(associationEndRealization.Specification) || associationEndsPersistency[associationEndRealization.Specification] == false)
                            associationEndsPersistency[associationEndRealization.Specification] = associationEndRealization.Persistent;

                lock (MembersSpecializationPropertiesLock)
                {
                    if (AssociationEndsPersistency == null)
                        AssociationEndsPersistency = new Dictionary<AssociationEnd, bool>(associationEndsPersistency);
                    if (AssociationEndsPersistency.TryGetValue(associationEnd, out isPersistent))
                        return isPersistent;
                    AssociationEndsPersistency[associationEnd] = associationEnd.Persistent;
                    return associationEnd.Persistent;
                }
            }
        }


        private System.Collections.Generic.Dictionary<AssociationEnd, bool> AssoctionEndReferentialIntegrities = null;

        public bool HasReferentialIntegrity(AssociationEnd associationEnd)
        {
            lock (MembersSpecializationPropertiesLock)
            {
                if (AssoctionEndReferentialIntegrities != null)
                    if (AssoctionEndReferentialIntegrities.ContainsKey(associationEnd))
                        return AssoctionEndReferentialIntegrities[associationEnd];
                    else
                        return false;
            }

            //TODO θα πρέπει να τσεκαριστεί ότι το association end έχει σχέσει με την class
            //Δεν μπορέις να σβήσεις ένα object όπου υπάρχει
            //relation object μεταξύ αυτού και κάποιου άλλου γιάτι το
            //relation object θα μετατραπεί σε object χωρίς έννοια.
            //θα πρέπει πρώτα να σβήσεις το relation object.

            var assoctionEndReferentialIntegrities = new System.Collections.Generic.Dictionary<AssociationEnd, bool>();

            if (associationEnd.HasBehavioralSettings)
                assoctionEndReferentialIntegrities[associationEnd] = associationEnd.ReferentialIntegrity;

            foreach (AssociationEndRealization associationEndRealization in Features.OfType<AssociationEndRealization>())
                if (associationEndRealization.Specification == associationEnd && associationEndRealization.HasBehavioralSettings)
                    assoctionEndReferentialIntegrities[associationEnd] = associationEndRealization.ReferentialIntegrity;

            foreach (Structure _struct in GetAllGeneralClasifiers().OfType<Structure>())
                foreach (AssociationEndRealization associationEndRealization in _struct.Features.OfType<AssociationEndRealization>())
                    if (associationEndRealization.Specification == associationEnd && associationEndRealization.HasBehavioralSettings)
                        assoctionEndReferentialIntegrities[associationEnd] = associationEndRealization.ReferentialIntegrity;


            if (!assoctionEndReferentialIntegrities.ContainsKey(associationEnd))
                assoctionEndReferentialIntegrities[associationEnd] = false;

            lock (MembersSpecializationPropertiesLock)
            {
                if (AssoctionEndReferentialIntegrities == null)
                    AssoctionEndReferentialIntegrities = new System.Collections.Generic.Dictionary<AssociationEnd, bool>(assoctionEndReferentialIntegrities);
                bool referentialIntegrity = false;
                AssoctionEndReferentialIntegrities.TryGetValue(associationEnd, out referentialIntegrity);
                return referentialIntegrity;
            }
        }



        /// <MetaDataID>{620127e3-0150-4f6e-a942-47a7007c6c77}</MetaDataID>
        public AttributeRealization GetAttributeRealization(Attribute attribute)
        {
            foreach (MetaDataRepository.Feature feature in Features)
            {
                AttributeRealization attributeRealization = feature as AttributeRealization;
                if (attributeRealization != null && attribute == attributeRealization.Specification)
                    return attributeRealization;
            }
            //TODO υπάρχει περίπτωση να μην έρθουν σορταρισμένες κατά σειρά στην ιεραρχία
            foreach (Classifier classifier in GetGeneralClasifiers())
            {
                if (classifier is Interface)
                    continue;
                Class _class = classifier as Class;
                AttributeRealization attributeRealization = _class.GetAttributeRealization(attribute);
                if (attributeRealization != null && attribute == attributeRealization.Specification)
                    return attributeRealization;
            }
            return null;

        }



        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        /// <MetaDataID>{60b8dc4e-9574-4919-95a4-ec9a387365a5}</MetaDataID>
        public override void Synchronize(MetaObject OriginMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            RefreshClassHierarchyCollections();
            try
            {
                if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                    return;
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    MetaDataRepository.Structure originClass = (MetaDataRepository.Structure)OriginMetaObject;
                    #region Sychronize realization relationship
                    ContainedItemsSynchronizer RealizationSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(originClass.Realizations, _Realizations, this);
                    RealizationSynchronizer.FindModifications();
                    RealizationSynchronizer.ExecuteAddCommand();
                    RealizationSynchronizer.ExecuteDeleteCommand();
                    RealizationSynchronizer.Synchronize();
                    foreach (MetaDataRepository.Realization realization in _Realizations)
                        realization.Implementor = this;
                    #endregion

                    base.Synchronize(OriginMetaObject);
                    MetaDataRepository.SynchronizerSession.MetaObjectUnderSynchronization(this);


                    _Persistent = originClass.Persistent;
                    //_Abstract = originClass.Abstract;


                    //ReferentialIntegrity.Loaded = false;
                    //ClassHierarchyLinkAssociationCashed = false;
                    //ClassHierarchyInterfaces = null;
                    //AssociationEndLoadPolicy = null;
                    //AssociationEndPersistency = null;

                    BuildCaseInsensitiveNames();

                    StateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }




        /// <summary>This method creates a new realize relation and adds it in structure. </summary>
        /// <param name="relationName">Name of the relation being added </param>
        /// <param name="_interface">Name of the interface with which to create the realize relation.
        /// If this name is not unique, you must use the qualified name (for example, 
        /// Namespace::Namespace(...)::supplier_name  CPlusplus Style or Namespace.Namespace(...).supplier_name CSharp Style) </param>
        /// <MetaDataID>{e2df8c96-d6b1-452b-8cb4-b6356849be21}</MetaDataID>
        public Realization AddRealizeRel(string relationName, Interface _interface)
        {
            lock (RealizationsLock)
            {

                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    System.Type[] ctorTypes = new System.Type[3] { typeof(string), typeof(Interface), typeof(Class) };

                    PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    Realization realization = objectStorage.NewObject(typeof(Realization), ctorTypes, relationName, _interface, this) as Realization;
                    _Realizations.Add(realization);
                    stateTransition.Consistent = true;
                    return realization;

                }
            }
        }

        /// <summary>This method deletes a realize relation from a class. </summary>
        /// <param name="realizeRel">Realize relation being deleted. </param>
        /// <MetaDataID>{6e8780bd-2508-4522-a47e-e7a963437d83}</MetaDataID>
        public void RemoveRealizeRel(Realization realizeRel)
        {
            if (realizeRel == null)
                return;
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _Realizations.Remove(realizeRel);
                    stateTransition.Consistent = true;
                }
            }
        }

        protected readonly object RealizationsLock = new object();
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{D6FB782F-5776-4D33-A2D7-AADE315F5FAC}</MetaDataID>
        protected OOAdvantech.Collections.Generic.Set<Realization> _Realizations = new OOAdvantech.Collections.Generic.Set<Realization>();
        /// <MetaDataID>{191D4060-A06B-46B2-9F09-EFB8C7E0C802}</MetaDataID>
        [MetaDataRepository.BackwardCompatibilityID("+1")]
        [MetaDataRepository.PersistentMember("_Realizations")]
        public virtual OOAdvantech.Collections.Generic.Set<Realization> Realizations
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Realizations.ToThreadSafeSet();
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }


        /// <exclude>Excluded</exclude>
        protected bool _Persistent;
        /// <MetaDataID>{2054C64A-3091-4831-9959-68DE339E5E22}</MetaDataID>
        [BackwardCompatibilityID("+12")]
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



        public override OOAdvantech.Collections.Generic.Set<AssociationEnd> GetRoles(bool Inherit)
        {
            lock (ClassHierarchyLock)
            {
                if (ClassHierarchyRoles != null && Inherit)
                    ClassHierarchyRoles.ToThreadSafeSet(); ;
            }


            System.Collections.Generic.List<AssociationEnd> roles = new System.Collections.Generic.List<AssociationEnd>();
            foreach (AssociationEnd associationEnd in Roles)
                roles.Add(associationEnd);

            if (Inherit == false)
                return new OOAdvantech.Collections.Generic.Set<AssociationEnd>(roles);


            foreach (Generalization generalization in Generalizations)
                roles.AddRange(generalization.Parent.GetRoles(Inherit));

            foreach (Realization realization in Realizations)
                roles.AddRange(realization.Abstarction.GetRoles(Inherit));

            lock (ClassHierarchyLock)
            {
                if (ClassHierarchyRoles == null)
                    ClassHierarchyRoles = new OOAdvantech.Collections.Generic.Set<AssociationEnd>(roles);
                return ClassHierarchyRoles.ToThreadSafeSet();
            }
        }

        public override OOAdvantech.Collections.Generic.Set<AssociationEnd> GetAssociateRoles(bool Inherit)
        {
            lock (ClassHierarchyLock)
            {
                if (Inherit && ClassHierarchyAssociateRoles != null)
                    return ClassHierarchyAssociateRoles;

                if (!Inherit && ClassAssociateRoles != null)
                    return ClassAssociateRoles;
            }

            OOAdvantech.Collections.Generic.Set<AssociationEnd> associateRoles = new Collections.Generic.Set<AssociationEnd>();
            foreach (AssociationEnd associationEnd in Roles)
                associateRoles.Add(associationEnd.GetOtherEnd());

            if (Inherit == false)
            {
                lock (ClassHierarchyLock)
                {
                    ClassAssociateRoles = new OOAdvantech.Collections.Generic.Set<AssociationEnd>(associateRoles, Collections.CollectionAccessType.ReadOnly);
                    return ClassAssociateRoles;
                }
            }
            else
            {
                foreach (Generalization generalization in Generalizations)
                    associateRoles.AddRange(generalization.Parent.GetAssociateRoles(Inherit));

                foreach (Realization realization in Realizations)
                    associateRoles.AddRange(realization.Abstarction.GetAssociateRoles(Inherit));

                lock (ClassHierarchyLock)
                {
                    ClassHierarchyAssociateRoles = new OOAdvantech.Collections.Generic.Set<AssociationEnd>(associateRoles, Collections.CollectionAccessType.ReadOnly);
                    return ClassHierarchyAssociateRoles;
                }
            }

        }

        public override Set<Feature> GetFeatures(bool Inherit)
        {
            lock (ClassHierarchyLock)
            {
                if (ClassHierarchyFeatures != null && Inherit)
                    return ClassHierarchyFeatures.ToThreadSafeSet();
            }
            Set<Feature> features = new Set<Feature>();
            lock (FeaturesLock)
            {

                foreach (Feature mFeature in Features)
                    features.Add(mFeature);

                if (Inherit == false)
                    return new OOAdvantech.Collections.Generic.Set<Feature>(features);

                foreach (Generalization generalization in Generalizations)
                    features.AddRange(generalization.Parent.GetFeatures(Inherit));

                foreach (Realization realization in Realizations)
                    features.AddRange(realization.Abstarction.GetFeatures(Inherit));


            }
            lock (ClassHierarchyLock)
            {

                if (ClassHierarchyFeatures == null)
                    ClassHierarchyFeatures = new OOAdvantech.Collections.Generic.Set<Feature>(features);

                return ClassHierarchyFeatures.ToThreadSafeSet();
            }
        }

        public Dictionary<Attribute, bool> AttributeMultilingualism { get; private set; }


        /// <MetaDataID>{F04ADFC8-63CE-46EF-85C0-48471F9A3657}</MetaDataID>
        /// <summary>This method retrieves all realized interfaces of Class hierarchy. 
        /// Note this method is recursive.  
        /// To retrieve only interfaces that realized from  this Class call the Getnterfaces. </summary>
        public OOAdvantech.Collections.Generic.Set<Interface> GetAllInterfaces()
        {
            OOAdvantech.Collections.Generic.Set<Interface> interfaces = new OOAdvantech.Collections.Generic.Set<Interface>();
            foreach (Generalization generalization in Generalizations)
            {
                if (generalization.Parent != null)
                {
                    OOAdvantech.Collections.Generic.Set<Interface> parentCalssInterfaces = (generalization.Parent as InterfaceImplementor).GetAllInterfaces();
                    foreach (Interface _interface in parentCalssInterfaces)
                    {
                        if (interfaces.Contains(_interface))
                            continue;
                        else
                            interfaces.Add(_interface);
                    }
                }
            }
            foreach (Realization realization in Realizations)
            {
                if (realization.Abstarction != null)
                {
                    if (interfaces.Contains(realization.Abstarction))
                        continue;
                    else
                    {
                        interfaces.Add(realization.Abstarction);
                        foreach (Interface _interface in realization.Abstarction.GetAllGeneralClasifiers())
                        {
                            if (interfaces.Contains(_interface))
                                continue;
                            else
                                interfaces.Add(_interface);
                        }
                    }
                }
            }
            return interfaces;
        }
        /// <MetaDataID>{A5A43860-156A-41CB-ACA4-FA67B32246AF}</MetaDataID>
        /// <summary>This method retrieves only the interfaces that realized from  this Class. </summary>
        public OOAdvantech.Collections.Generic.Set<Interface> GetInterfaces()
        {
            OOAdvantech.Collections.Generic.Set<Interface> interfaces = new OOAdvantech.Collections.Generic.Set<Interface>();
            foreach (Realization realization in Realizations)
                interfaces.Add(realization.Abstarction);
            return interfaces;

        }
    }
}
