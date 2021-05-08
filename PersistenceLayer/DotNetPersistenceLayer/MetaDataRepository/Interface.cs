using OOAdvantech.Transactions;
namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{8B4322B6-2FA2-4E9B-A08B-1DA72864A727}</MetaDataID>
    /// <summary>An interface is a named set of operations that characterize the behavior of an element.
    /// In the metamodel, an Interface contains a set of Operations that together define a service offered by a Classifier realizing the Interface. A Classifier may offer several services, which means that it may realize several Interfaces, and several Classifiers may realize the same Interface.
    /// An Interface may participate in an Association. Interface can access the other end of Association through Getter Setter Operations.  Interface may have pseudo Attribute  which can be accessed through Getter Setter Operations. </summary>
    [BackwardCompatibilityID("{8B4322B6-2FA2-4E9B-A08B-1DA72864A727}")]
    [Persistent()]
    public class Interface : Classifier
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(ClassHierarchyLinkAssociationCashed))
            {
                if (value == null)
                    ClassHierarchyLinkAssociationCashed = default(bool);
                else
                    ClassHierarchyLinkAssociationCashed = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ClassHierarchyLinkAssociation))
            {
                if (value == null)
                    _ClassHierarchyLinkAssociation = default(OOAdvantech.MetaDataRepository.Association);
                else
                    _ClassHierarchyLinkAssociation = (OOAdvantech.MetaDataRepository.Association)value;
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

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(ClassHierarchyLinkAssociationCashed))
                return ClassHierarchyLinkAssociationCashed;

            if (member.Name == nameof(_ClassHierarchyLinkAssociation))
                return _ClassHierarchyLinkAssociation;

            if (member.Name == nameof(_Realizations))
                return _Realizations;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{c7023d65-11f1-45db-993f-dc21dced9e75}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<Classifier> GetAllSpecializeClasifiers()
        {
            OOAdvantech.Collections.Generic.Set<Classifier> classes = new OOAdvantech.Collections.Generic.Set<Classifier>();

            foreach (MetaDataRepository.Generalization specializeRelation in Specializations)
            {
                if (specializeRelation.Child != null)
                {
                    classes.Add(specializeRelation.Child);
                    classes.AddRange(specializeRelation.Child.GetAllSpecializeClasifiers());
                }
            }

            foreach (MetaDataRepository.Realization realization in Realizations)
            {
                if ((realization.Implementor as Classifier) != null)
                {
                    classes.Add(realization.Implementor as Classifier);
                    classes.AddRange((realization.Implementor as Classifier).GetAllSpecializeClasifiers());
                }
            }

            return classes;

        }
        /// <MetaDataID>{f036514e-4e56-4490-a701-09193ca21fed}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<StorageCell> StorageCellsOfThisType
        {
            get
            {
                Collections.Generic.Set<StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<StorageCell>();
                foreach (MetaDataRepository.Realization realization in Realizations)
                {
                    if (realization.Implementor is MetaDataRepository.Class && (realization.Implementor as MetaDataRepository.Class).Persistent)
                    {
                        storageCells.AddRange((realization.Implementor as Class).StorageCellsOfThisType);
                    }
                }
                return storageCells;
            }
        }
        /// <MetaDataID>{2EB29462-5CF3-4E97-8A88-91C29676EEC7}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private bool ClassHierarchyLinkAssociationCashed = false;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5D184081-1629-45CB-B3EF-B3EABF20AAC8}</MetaDataID>
        private Association _ClassHierarchyLinkAssociation;
        /// <MetaDataID>{E2E60471-D6CC-4FE3-861A-B73AAE1DDAAA}</MetaDataID>
        public override Association ClassHierarchyLinkAssociation
        {
            get
            {
                try
                {
                    if (ClassHierarchyLinkAssociationCashed)
                        return _ClassHierarchyLinkAssociation;
                    else
                    {
                        if (LinkAssociation != null && LinkAssociation.LinkClass == this)
                        {
                            _ClassHierarchyLinkAssociation = LinkAssociation;
                            return LinkAssociation;
                        }
                        foreach (Classifier _classifier in GetAllGeneralClasifiers())
                        {
                            _ClassHierarchyLinkAssociation = _classifier.ClassHierarchyLinkAssociation;
                            if (_ClassHierarchyLinkAssociation != null)
                                break;
                        }

                        return _ClassHierarchyLinkAssociation;
                    }
                }
                finally
                {
                    ClassHierarchyLinkAssociationCashed = true;
                }
            }
        }
        /// <MetaDataID>{C0258C2A-FDDE-4ADD-8F32-EC2D8EC1250F}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<Realization> _Realizations = new OOAdvantech.Collections.Generic.Set<Realization>();
        /// <MetaDataID>{D44CB6CE-30B7-4BC4-B866-C2E122EEBE29}</MetaDataID>
        [Association("AbstarctPart", typeof(OOAdvantech.MetaDataRepository.Realization), MetaDataRepository.Roles.RoleB, "{B378BE2A-2470-4C56-B2FB-4CF3776BE55F}")]
        [PersistentMember("_Realizations")]
        [RoleBMultiplicityRange(0)]
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

        /// <MetaDataID>{0CB42A25-38CB-4929-AA6C-3D5314A54303}</MetaDataID>
        internal void AddRealization(MetaDataRepository.Realization theRealization)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
            {
                _Realizations.Add(theRealization);
                stateTransition.Consistent = true;
            }


        }
    }
}
