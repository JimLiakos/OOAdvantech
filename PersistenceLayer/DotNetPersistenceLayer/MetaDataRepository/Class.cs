using System.Linq;
using OOAdvantech.Collections.Generic;

namespace OOAdvantech.MetaDataRepository
{

    /// <MetaDataID>{75479CE2-6FF0-4B75-9B62-F0190C9EA6E6}</MetaDataID>
    /// <summary>A class is a description of a set of objects that share the same attributes, operations, methods, relationships, and semantics. A class may use a set of interfaces to specify collections of operations it provides to its environment. In the metamodel, a Class describes a set of Objects sharing a collection of Features, including Operations, Attributes, and Methods that are common to the set of Objects. Furthermore, a Class may realize zero or more Interfaces; this means that its full descriptor (see "Inheritance" on page 2-69 for the definition) must contain Method for every Operation from every realized Interface (it may contain additional operations as well). </summary>
    [BackwardCompatibilityID("{75479CE2-6FF0-4B75-9B62-F0190C9EA6E6}")]
    [Persistent()]
    public class Class : MetaDataRepository.Classifier, InterfaceImplementor
    {
        /// <MetaDataID>{6fc44e88-ae51-469b-b6cb-685819b2794e}</MetaDataID>
        public virtual bool IsMultilingual(Attribute attribute)
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
                //TODO Προυποθέτει ότι έχει φορτοθεί η class πλήρως για την περίπτωση .Net

                foreach (AttributeRealization attributeRealization in Features.OfType<AttributeRealization>())
                    attributeMultilingualism[attributeRealization.Specification] = attributeRealization.Multilingual;

                foreach (Class _class in GetAllGeneralClasifiers().OfType<Class>())
                {
                    foreach (AttributeRealization attributeRealization in _class.Features.OfType<AttributeRealization>())
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
            return false;
        }




        public bool IsMultilingual(AssociationEnd associationEnd)
        {
            if (associationEnd.Multilingual)
                return true;
            else
            {
                bool isMultilingual;
                lock (MembersSpecializationPropertiesLock)
                {
                    if (AssociationEndMultilingualism != null)
                    {
                        if (AssociationEndMultilingualism.TryGetValue(associationEnd, out isMultilingual))
                            return isMultilingual;
                        else
                            return false;
                    }
                }

                var associationEndMultilingualism = new OOAdvantech.Collections.Generic.Dictionary<AssociationEnd, bool>();
                foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
                {

                    if (associationEndRealization.Multilingual && (associationEndRealization.Namespace is Classifier) && IsA(associationEndRealization.Namespace as Classifier))
                    {
                        associationEndMultilingualism[associationEnd] = true;
                        break;
                    }
                }

                //TODO Προυποθέτει ότι έχει φορτvθεί η class πλήρvς για την περίπτωση .Net

                foreach (AssociationEndRealization associationEndRealization in Features.OfType<AssociationEndRealization>())
                    associationEndMultilingualism[associationEndRealization.Specification] = associationEndRealization.Multilingual;

                foreach (Class _class in GetAllGeneralClasifiers().OfType<Class>())
                    foreach (AssociationEndRealization associationEndRealization in _class.Features.OfType<AssociationEndRealization>())
                        if (!associationEndMultilingualism.ContainsKey(associationEndRealization.Specification) || associationEndMultilingualism[associationEndRealization.Specification] == false)
                            associationEndMultilingualism[associationEndRealization.Specification] = associationEndRealization.Multilingual;

                lock (MembersSpecializationPropertiesLock)
                {
                    if (AssociationEndMultilingualism == null)
                        AssociationEndMultilingualism = new Dictionary<AssociationEnd, bool>(associationEndMultilingualism);

                    if (AssociationEndMultilingualism.TryGetValue(associationEnd, out isMultilingual))
                        return isMultilingual;
                    else
                        AssociationEndMultilingualism[associationEnd] = false;
                    return AssociationEndMultilingualism[associationEnd];
                }

            }
        }


        /// <MetaDataID>{60491c20-2ecc-4c81-97b9-ff20c10566fa}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(ObjectActivationOperationLoaded))
            {
                if (value == null)
                    ObjectActivationOperationLoaded = default(bool);
                else
                    ObjectActivationOperationLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(CommitObjectStateInStorageOperationLoaded))
            {
                if (value == null)
                    CommitObjectStateInStorageOperationLoaded = default(bool);
                else
                    CommitObjectStateInStorageOperationLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(DeleteObjectOperationLoaded))
            {
                if (value == null)
                    DeleteObjectOperationLoaded = default(bool);
                else
                    DeleteObjectOperationLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_DeleteObject))
            {
                if (value == null)
                    _DeleteObject = default(OOAdvantech.MetaDataRepository.Operation);
                else
                    _DeleteObject = (OOAdvantech.MetaDataRepository.Operation)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ObjectActivation))
            {
                if (value == null)
                    _ObjectActivation = default(OOAdvantech.MetaDataRepository.Operation);
                else
                    _ObjectActivation = (OOAdvantech.MetaDataRepository.Operation)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_CommitObjectStateInStorage))
            {
                if (value == null)
                    _CommitObjectStateInStorage = default(OOAdvantech.MetaDataRepository.Operation);
                else
                    _CommitObjectStateInStorage = (OOAdvantech.MetaDataRepository.Operation)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_StorageCells))
            {
                if (value == null)
                    _StorageCells = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>);
                else
                    _StorageCells = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
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
            if (member.Name == nameof(_Abstract))
            {
                if (value == null)
                    _Abstract = default(bool);
                else
                    _Abstract = (bool)value;
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
            if (member.Name == nameof(_Realizations))
            {
                if (value == null)
                    _Realizations = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Realization>);
                else
                    _Realizations = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Realization>)value;
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
            if (member.Name == nameof(_HasReferentialIntegrityRelations))
            {
                if (value == null)
                    _HasReferentialIntegrityRelations = null;
                else
                    _HasReferentialIntegrityRelations = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ClassHierarchyInterfaces))
            {
                lock (_Realizations)
                {
                    if (value == null)
                        ClassHierarchyInterfaces = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Interface>);
                    else
                        ClassHierarchyInterfaces = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Interface>)value;
                }
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssociationEndsLoadPolicy))
            {
                lock (MembersSpecializationPropertiesLock)
                {
                    if (value == null)
                        _AssociationEndsLoadPolicy = default(OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, int>);
                    else
                        _AssociationEndsLoadPolicy = (OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, int>)value;
                }
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssociationEndsPersistency))
            {
                lock (MembersSpecializationPropertiesLock)
                {
                    if (value == null)
                        AssociationEndsPersistency = default(OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, bool>);
                    else
                        AssociationEndsPersistency = (OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.AssociationEnd, bool>)value;
                }
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AttributePersistency))
            {
                lock (_Features)
                {
                    if (value == null)
                        AttributePersistency = default(OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.Attribute, bool>);
                    else
                        AttributePersistency = (OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.Attribute, bool>)value;
                    return ObjectMemberGetSet.MemberValueSetted;
                }
            }
            //if (member.Name == nameof(_Methods))
            //{
            //    if (value == null)
            //        _Methods = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Method>);
            //    else
            //        _Methods = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Method>)value;
            //    return ObjectMemberGetSet.MemberValueSetted;
            //}
            if (member.Name == nameof(InErrorCheck))
            {
                if (value == null)
                    InErrorCheck = default(bool);
                else
                    InErrorCheck = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(PersistentAssociateRoles))
            {
                lock (MembersSpecializationPropertiesLock)
                {
                    if (value == null)
                        PersistentAssociateRoles = default(System.Collections.Generic.List<OOAdvantech.MetaDataRepository.AssociationEnd>);
                    else
                        PersistentAssociateRoles = (System.Collections.Generic.List<OOAdvantech.MetaDataRepository.AssociationEnd>)value;
                }
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(PersistentAttributes))
            {
                if (value == null)
                    PersistentAttributes = default(System.Collections.Generic.List<OOAdvantech.MetaDataRepository.Attribute>);
                else
                    PersistentAttributes = (System.Collections.Generic.List<OOAdvantech.MetaDataRepository.Attribute>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{111cd7e7-6ee2-40c9-a779-f8d2328edab1}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(ObjectActivationOperationLoaded))
                return ObjectActivationOperationLoaded;

            if (member.Name == nameof(CommitObjectStateInStorageOperationLoaded))
                return CommitObjectStateInStorageOperationLoaded;

            if (member.Name == nameof(DeleteObjectOperationLoaded))
                return DeleteObjectOperationLoaded;

            if (member.Name == nameof(_DeleteObject))
                return _DeleteObject;

            if (member.Name == nameof(_ObjectActivation))
                return _ObjectActivation;

            if (member.Name == nameof(_CommitObjectStateInStorage))
                return _CommitObjectStateInStorage;

            if (member.Name == nameof(_StorageCells))
                return _StorageCells;

            if (member.Name == nameof(ClassHierarchyLinkAssociationCashed))
                return ClassHierarchyLinkAssociationCashed;

            if (member.Name == nameof(_ClassHierarchyLinkAssociation))
                return _ClassHierarchyLinkAssociation;

            if (member.Name == nameof(_Abstract))
                return _Abstract;

            if (member.Name == nameof(_Persistent))
                return _Persistent;

            if (member.Name == nameof(_Realizations))
                return _Realizations;

            if (member.Name == nameof(AssoctionEndReferentialIntegrities))
                return AssoctionEndReferentialIntegrities;

            if (member.Name == nameof(_HasReferentialIntegrityRelations))
                return _HasReferentialIntegrityRelations;

            if (member.Name == nameof(ClassHierarchyInterfaces))
                return ClassHierarchyInterfaces;

            if (member.Name == nameof(AssociationEndsLoadPolicy))
                return AssociationEndsLoadPolicy;

            if (member.Name == nameof(AssociationEndsPersistency))
                return AssociationEndsPersistency;

            if (member.Name == nameof(AttributePersistency))
                return AttributePersistency;

            //if (member.Name == nameof(_Methods))
            //    return _Methods;

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(PersistentAssociateRoles))
                return PersistentAssociateRoles;

            if (member.Name == nameof(PersistentAttributes))
                return PersistentAttributes;


            return base.GetMemberValue(token, member);
        }


        /// <MetaDataID>{591f74ad-4d39-4bf4-984f-5b9cae33cc93}</MetaDataID>
        public bool AllowTransient(AssociationEnd associationEnd)
        {
            foreach (MetaDataRepository.Feature feature in Features)
            {
                AssociationEndRealization associationEndRealization = feature as AssociationEndRealization;
                if (associationEndRealization != null)
                {
                    if (associationEndRealization.Specification == associationEnd && associationEndRealization.HasBehavioralSettings)
                        return associationEndRealization.AllowTransient;
                }
            }
            //TODO υπάρχει περίπτωση να μην έρθουν σορταρισμένες κατά σειρά στην ιεραρχία
            foreach (Classifier classifier in GetAllGeneralClasifiers())
            {
                if (classifier is Interface)
                    continue;
                Class _class = classifier as Class;

                foreach (MetaDataRepository.Feature feature in _class.Features)
                {
                    AssociationEndRealization associationEndRealization = feature as AssociationEndRealization;
                    if (associationEndRealization != null)
                    {
                        if (associationEndRealization.Specification == associationEnd && associationEndRealization.HasBehavioralSettings)
                            return associationEndRealization.AllowTransient;
                    }
                }
            }
            if (associationEnd.HasBehavioralSettings)
                return associationEnd.AllowTransient;
            else
                return false;
        }
        /// <MetaDataID>{db9da311-7a8a-4baf-a270-e6984787e8e8}</MetaDataID>
        public override MetaObject GetMember(string memberName)
        {
            foreach (Feature feature in Features)
            {
                if (feature.Name == memberName)
                {
                    if (feature is AttributeRealization)
                        return (feature as AttributeRealization).Specification;
                    else if (feature is AssociationEndRealization)
                        return (feature as AssociationEndRealization).Specification;
                    else
                        return feature;
                }
            }
            foreach (AssociationEnd associationEnd in GetAssociateRoles(false))
            {
                if (associationEnd.Name == memberName)
                    return associationEnd;
            }
            foreach (Generalization generalization in Generalizations)
            {
                if (generalization.Parent != null)
                {
                    MetaObject member = generalization.Parent.GetMember(memberName);
                    if (member != null)
                        return member;
                }
            }
            foreach (Realization realization in Realizations)
            {
                if (realization.Abstarction != null)
                {
                    MetaObject member = realization.Abstarction.GetMember(memberName);
                    if (member != null)
                        return member;
                }
            }

            return null;

        }
        /// <MetaDataID>{d4cd60a7-ef72-4fea-b0bd-f52ad15a4a38}</MetaDataID>
        bool ObjectActivationOperationLoaded = false;

        /// <MetaDataID>{ad5d8362-9425-4e94-a7c6-fe2f86b0480e}</MetaDataID>
        bool CommitObjectStateInStorageOperationLoaded = false;

        /// <MetaDataID>{ba4005f8-07a0-4733-aa52-a84f74d6e218}</MetaDataID>
        bool DeleteObjectOperationLoaded = false;

        bool ObjectsLinkOperationLoaded = false;


        /// <MetaDataID>{fca7cfb7-2bfd-405f-a57d-26e968023696}</MetaDataID>
        bool BeforeCommitObjectStateInStorageOperationLoaded = false;


        private Operation _ObjectsLink;

        public Operation ObjectsLink
        {
            get
            {
                if (!ObjectsLinkOperationLoaded)
                {

                    LoadPersistenceLayerOperationCalls();
                    ObjectActivationOperationLoaded = true;
                    CommitObjectStateInStorageOperationLoaded = true;
                    DeleteObjectOperationLoaded = true;
                    ObjectsLinkOperationLoaded = true;
                }

                return _ObjectsLink;
            }
        }

        /// <exclude>Excluded</exclude>
        private Operation _DeleteObject;
        /// <MetaDataID>{a369be4c-ee15-4eaa-9645-ad74f7549ef6}</MetaDataID>
        public Operation DeleteObject
        {
            get
            {

                if (!DeleteObjectOperationLoaded)
                {

                    LoadPersistenceLayerOperationCalls();
                    ObjectActivationOperationLoaded = true;
                    CommitObjectStateInStorageOperationLoaded = true;
                    DeleteObjectOperationLoaded = true;
                }

                return _DeleteObject;

            }
        }

        private void LoadPersistenceLayerOperationCalls()
        {
            foreach (Operation operation in GetOperations(true))
            {
                if (operation.GetPropertyValue<string>("OparetionType", "ObjectActivationCall").ToLower() == "true".ToLower())
                {
                    _ObjectActivation = operation;
                    ObjectActivationOperationLoaded = true;
                    if (ObjectActivationOperationLoaded && BeforeCommitObjectStateInStorageOperationLoaded && CommitObjectStateInStorageOperationLoaded && DeleteObjectOperationLoaded && ObjectsLinkOperationLoaded)
                        break;
                }
                if (operation.GetPropertyValue<string>("OparetionType", "CommitObjectStateInStorageCall").ToLower() == "true".ToLower())
                {
                    _CommitObjectStateInStorage = operation;
                    CommitObjectStateInStorageOperationLoaded = true;
                    if (ObjectActivationOperationLoaded && BeforeCommitObjectStateInStorageOperationLoaded && CommitObjectStateInStorageOperationLoaded && DeleteObjectOperationLoaded && ObjectsLinkOperationLoaded)
                        break;
                }
                if (operation.GetPropertyValue<string>("OparetionType", "BeforeCommitObjectStateInStorageCall").ToLower() == "true".ToLower())
                {
                    _BeforeCommitObjectStateInStorage = operation;
                    BeforeCommitObjectStateInStorageOperationLoaded = true;
                    if (ObjectActivationOperationLoaded && BeforeCommitObjectStateInStorageOperationLoaded && CommitObjectStateInStorageOperationLoaded && DeleteObjectOperationLoaded && ObjectsLinkOperationLoaded)
                        break;
                }

                if (operation.GetPropertyValue<string>("OparetionType", "DeleteObjectCall").ToLower() == "true".ToLower())
                {
                    _DeleteObject = operation;
                    DeleteObjectOperationLoaded = true;
                    if (ObjectActivationOperationLoaded && BeforeCommitObjectStateInStorageOperationLoaded && CommitObjectStateInStorageOperationLoaded && DeleteObjectOperationLoaded && ObjectsLinkOperationLoaded)
                        break;
                }

                if (operation.GetPropertyValue<string>("OparetionType", "ObjectsLinkCall")!=null&&operation.GetPropertyValue<string>("OparetionType", "ObjectsLinkCall").ToLower() == "true".ToLower())
                {
                    _ObjectsLink = operation;
                    ObjectsLinkOperationLoaded = true;
                    if (ObjectActivationOperationLoaded && BeforeCommitObjectStateInStorageOperationLoaded && CommitObjectStateInStorageOperationLoaded && DeleteObjectOperationLoaded && ObjectsLinkOperationLoaded)
                        break;
                }
            }

            ObjectActivationOperationLoaded = true;
            CommitObjectStateInStorageOperationLoaded = true;
            DeleteObjectOperationLoaded = true;
            BeforeCommitObjectStateInStorageOperationLoaded = true;
            ObjectsLinkOperationLoaded = true;
        }









        /// <exclude>Excluded</exclude>
        private Operation _ObjectActivation;
        /// <summary>This operation will be called from the system when object goes from the passive in object storage to operative mode in memory. </summary>
        /// <MetaDataID>{8F301EFD-9D97-459C-AE0C-B6D941B8B4A5}</MetaDataID>
        public Operation ObjectActivation
        {
            get
            {

                if (!ObjectActivationOperationLoaded)
                {
                    LoadPersistenceLayerOperationCalls();
                    ObjectActivationOperationLoaded = true;
                    CommitObjectStateInStorageOperationLoaded = true;
                    DeleteObjectOperationLoaded = true;
                }

                return _ObjectActivation;

            }
        }





        /// <exclude>Excluded</exclude>
        private Operation _BeforeCommitObjectStateInStorage;


        /// <MetaDataID>{c2a9465b-96c8-4b47-b242-6dfcff540fd2}</MetaDataID>
        public Operation BeforeCommitObjectStateInStorage
        {
            get
            {
                if (!BeforeCommitObjectStateInStorageOperationLoaded)
                {
                    LoadPersistenceLayerOperationCalls();
                    ObjectActivationOperationLoaded = true;
                    CommitObjectStateInStorageOperationLoaded = true;
                    DeleteObjectOperationLoaded = true;
                }
                return _BeforeCommitObjectStateInStorage;
            }
        }






        /// <exclude>Excluded</exclude>
        private Operation _CommitObjectStateInStorage;

        /// <MetaDataID>{64f323c5-2f4d-4f95-aa5a-c864b271aa18}</MetaDataID>
        public Operation CommitObjectStateInStorage
        {
            get
            {
                if (!CommitObjectStateInStorageOperationLoaded)
                {
                    LoadPersistenceLayerOperationCalls();
                    ObjectActivationOperationLoaded = true;
                    CommitObjectStateInStorageOperationLoaded = true;
                    DeleteObjectOperationLoaded = true;
                }
                return _CommitObjectStateInStorage;
            }
        }


        /// <MetaDataID>{B2195685-90FB-47C3-988E-D837A1C94971}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<Classifier> GetAllGeneralClasifiers()
        {

            lock (GeneralizationLock)
            {
                if (ClassifierHierarchyClassifiers != null)
                    return ClassifierHierarchyClassifiers;

                OOAdvantech.Collections.Generic.Set<Classifier> generalClasifiers = new OOAdvantech.Collections.Generic.Set<Classifier>();
                foreach (Generalization generalization in Generalizations)
                {
                    if (generalization.Parent == null)
                        continue;
                    generalClasifiers.AddRange(generalization.Parent.GetAllGeneralClasifiers());
                    generalClasifiers.Add(generalization.Parent);
                }
                foreach (Interface _interface in GetAllInterfaces())
                    generalClasifiers.Add(_interface);
                ClassifierHierarchyClassifiers = new Collections.Generic.Set<Classifier>(generalClasifiers, Collections.CollectionAccessType.ReadOnly);
                return ClassifierHierarchyClassifiers;
            }

        }
        /// <MetaDataID>{491B1D3A-A9E4-41F7-9866-22C0B6C31BC4}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<Classifier> GetGeneralClasifiers()
        {
            lock (ClassHierarchyLock)
            {
                if (ParentClassifiers != null)
                    return ParentClassifiers;
            }

            OOAdvantech.Collections.Generic.Set<Classifier> generalClassifiers = new OOAdvantech.Collections.Generic.Set<Classifier>();

            lock (GeneralizationLock)
            {
                foreach (Generalization generalization1 in Generalizations)
                    generalClassifiers.Add(generalization1.Parent);
                foreach (Interface _interface in GetInterfaces())
                    generalClassifiers.Add(_interface);
            }

            lock (ClassHierarchyLock)
            {
                ParentClassifiers = generalClassifiers;
                return ParentClassifiers;
            }

        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{85AADADD-4AE3-40D0-ADA7-BC964C51E988}</MetaDataID>
        protected Collections.Generic.Set<StorageCell> _StorageCells = new OOAdvantech.Collections.Generic.Set<StorageCell>();
        /// <summary>The member StorageCells defines a collection with storage cells, 
        /// the locals and storage cell references. </summary>
        /// <MetaDataID>{1C3524AA-F8E4-414B-843F-7D63075E6986}</MetaDataID>
        [Association("StoreObjectInCell", typeof(OOAdvantech.MetaDataRepository.StorageCell), MetaDataRepository.Roles.RoleA, "{E86388A2-3151-4935-BCF8-9A19CCFE6122}")]
        [PersistentMember("_StorageCells")]
        [RoleAMultiplicityRange(0)]
        public virtual Collections.Generic.Set<StorageCell> StorageCells
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<StorageCell>(_StorageCells, Collections.CollectionAccessType.ReadOnly);
            }
        }
        /// <MetaDataID>{EC7EC8F8-3519-405B-840D-7EA962843399}</MetaDataID>
        public override bool IsA(MetaDataRepository.Classifier classifier)
        {

            if (classifier is Interface)
            {
                lock (RealizationsLock)
                {
                    return (GetAllInterfaces() as System.Collections.IList).Contains(classifier);
                }
            }
            else
                return base.IsA(classifier);
        }

        /// <MetaDataID>{A783D869-0D9F-4A7A-A659-8711481C8C47}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private bool ClassHierarchyLinkAssociationCashed = false;
        /// <exclude>Excluded</exclude>
        /// Cache data
        ///  
        /// <MetaDataID>{2D4ADC8E-A49B-4AC9-9BD3-7E02063D52F4}</MetaDataID>
        private Association _ClassHierarchyLinkAssociation;
        /// <MetaDataID>{39BF9A7E-2C87-457F-8F0A-7B3332F7FBDF}</MetaDataID>
        public override Association ClassHierarchyLinkAssociation
        {
            get
            {
                lock (GeneralizationLock)
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
                            foreach (Interface _interface in GetAllInterfaces())
                            {
                                _ClassHierarchyLinkAssociation = _interface.ClassHierarchyLinkAssociation;
                                if (_ClassHierarchyLinkAssociation != null)
                                    return _ClassHierarchyLinkAssociation;
                            }

                            foreach (Classifier _classifier in GetAllGeneralClasifiers())
                            {
                                _ClassHierarchyLinkAssociation = _classifier.ClassHierarchyLinkAssociation;
                                if (_ClassHierarchyLinkAssociation != null)
                                    return _ClassHierarchyLinkAssociation;
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
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C27E8345-B8B0-46AE-AA8A-775B3556DBE0}</MetaDataID>
        protected bool _Abstract;
        /// <MetaDataID>{CCC4A14E-77F3-44B9-9DA4-9A78324D11D7}</MetaDataID>
        [BackwardCompatibilityID("+13")]
        [PersistentMember("_Abstract")]
        public virtual bool Abstract
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Abstract;
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
                        _Abstract = value;
                        stateTransition.Consistent = true;
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9C042ABE-A534-4F89-8EC1-06E1A689F680}</MetaDataID>
        protected bool _Persistent;
        /// <MetaDataID>{15CF762C-29B8-4A7D-88BA-64A88C9636D5}</MetaDataID>
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



        protected readonly object RealizationsLock = new object();
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8236A4E3-E39C-408E-82D2-02727E627C67}</MetaDataID>
        protected OOAdvantech.Collections.Generic.Set<Realization> _Realizations = new OOAdvantech.Collections.Generic.Set<Realization>();
        /// <MetaDataID>{1579F94B-3381-4D6B-B4E4-7B1330F5A158}</MetaDataID>
        [BackwardCompatibilityID("+28")]
        [PersistentMember("_Realizations")]
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







        /// <MetaDataID>{706C6F13-DD3B-493A-BDB3-09F88E8E37C8}</MetaDataID>
        private System.Collections.Generic.Dictionary<AssociationEnd, bool> AssoctionEndReferentialIntegrities = null;

        /// <MetaDataID>{F24F2FDD-C9A9-4193-A147-810156C7A57F}</MetaDataID>
        public bool HasReferentialIntegrity(AssociationEnd associationEnd)
        {
            lock (MembersSpecializationPropertiesLock)
            {
                if (AssoctionEndReferentialIntegrities != null)
                    if (AssoctionEndReferentialIntegrities.ContainsKey(associationEnd))
                        return AssoctionEndReferentialIntegrities[associationEnd];
                    else if (associationEnd.HasBehavioralSettings)
                        return associationEnd.ReferentialIntegrity;
                    else
                        return false;
            }


            //TODO θα πρέπει να τσεκαριστεί ότι το association end έχει σχέσει με την class
            //Δεν μπορέις να σβήσεις ένα object όπου υπάρχει
            //relation object μεταξύ αυτού και κάποιου άλλου γιάτι το
            //relation object θα μετατραπεί σε object χωρίς έννοια.
            //θα πρέπει πρώτα να σβήσεις το relation object.

            var assoctionEndReferentialIntegrities = new System.Collections.Generic.Dictionary<AssociationEnd, bool>();


            foreach (AssociationEndRealization associationEndRealization in Features.OfType<AssociationEndRealization>())
                if (associationEndRealization.HasBehavioralSettings)
                    assoctionEndReferentialIntegrities[associationEndRealization.Specification] = associationEndRealization.ReferentialIntegrity;

            foreach (Class _class in GetAllGeneralClasifiers().OfType<Class>())
                foreach (AssociationEndRealization associationEndRealization in _class.Features.OfType<AssociationEndRealization>())
                    if (associationEndRealization.HasBehavioralSettings && !assoctionEndReferentialIntegrities.ContainsKey(associationEndRealization.Specification))
                        assoctionEndReferentialIntegrities[associationEndRealization.Specification] = associationEndRealization.ReferentialIntegrity;


            lock (MembersSpecializationPropertiesLock)
            {
                if (AssoctionEndReferentialIntegrities == null)
                    AssoctionEndReferentialIntegrities = new System.Collections.Generic.Dictionary<AssociationEnd, bool>(assoctionEndReferentialIntegrities);
                bool referentialIntegrity = false;
                AssoctionEndReferentialIntegrities.TryGetValue(associationEnd, out referentialIntegrity);
                return referentialIntegrity;
            }
        }

        /// <MetaDataID>{93adc42e-2f70-45df-87e9-15a70cc070b9}</MetaDataID>
        public bool IsTryOnObjectActivationFetching(AssociationEnd associationEnd)
        {
            lock (MembersSpecializationPropertiesLock)
            {
                if (AssociationEndsLoadPolicy.ContainsKey(associationEnd))
                    return AssociationEndsLoadPolicy[associationEnd] == 2;
                if (associationEnd.HasBehavioralSettings)
                    return associationEnd.TryOnObjectActivationFetching;
                else
                    return true;
            }
        }

        /// <MetaDataID>{7FD2740D-B702-4FE7-A4DC-FC284F514C11}</MetaDataID>
        public bool IsLazyFetching(AssociationEnd associationEnd)
        {

            lock (MembersSpecializationPropertiesLock)
            {
                if (AssociationEndsLoadPolicy.ContainsKey(associationEnd))
                    return AssociationEndsLoadPolicy[associationEnd] == 0;

                if (associationEnd.HasBehavioralSettings)
                    return associationEnd.LazyFetching;
                else
                    return true;
            }
        }

        /// <MetaDataID>{14F1D688-68FB-4659-9559-49F4987C0816}</MetaDataID>
        public AttributeRealization GetAttributeRealization(Attribute attribute)
        {
            if (attribute == null)
                return null;

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


        public AssociationEndRealization GetAssociationEndRealization(AssociationEnd associationEnd)
        {
            if (associationEnd == null)
                return null;
            foreach (AssociationEndRealization associationEndRealization in Features.OfType<AssociationEndRealization>())
            {
                if (associationEndRealization != null && associationEnd == associationEndRealization.Specification)
                    return associationEndRealization;
            }


            //TODO υπάρχει περίπτωση να μην έρθουν σορταρισμένες κατά σειρά στην ιεραρχία
            foreach (Classifier classifier in GetGeneralClasifiers())
            {
                if (classifier is Interface)
                    continue;
                Class _class = classifier as Class;
                AssociationEndRealization associationEndRealization = _class.GetAssociationEndRealization(associationEnd);
                if (associationEndRealization != null && associationEnd == associationEndRealization.Specification)
                    return associationEndRealization;
            }
            return null;

        }

        /// <MetaDataID>{93A800DF-8082-432D-8EB4-9C0B04F6D01E}</MetaDataID>
        public bool IsCascadeDelete(AssociationEnd associationEnd)
        {
            foreach (AssociationEndRealization associationEndRealization in Features.OfType<AssociationEndRealization>())
            {
                if (associationEndRealization.Specification == associationEnd && associationEndRealization.HasBehavioralSettings)
                    return associationEndRealization.CascadeDelete;
            }
            //TODO υπάρχει περίπτωση να μην έρθουν σορταρισμένες κατά σειρά στην ιεραρχία
            foreach (Class _class in GetAllGeneralClasifiers().OfType<Class>())
            {
                foreach (AssociationEndRealization associationEndRealization in _class.Features.OfType<AssociationEndRealization>())
                {
                    if (associationEndRealization.Specification == associationEnd && associationEndRealization.HasBehavioralSettings)
                        return associationEndRealization.CascadeDelete;
                }
            }
            if (associationEnd.HasBehavioralSettings)
                return associationEnd.CascadeDelete;
            else
                return false;
        }
        /// <MetaDataID>{19da0c1b-42f8-453c-b427-16181b8aed2f}</MetaDataID>
        public override Feature GetFeature(string identity, bool inherit)
        {

            Feature feature = base.GetFeature(identity, inherit);
            if (feature != null)
                return feature;

            if (!inherit)
                return null;

            foreach (Interface _interface in GetInterfaces())
            {
                feature = _interface.GetFeature(identity, inherit);
                if (feature != null)
                    return feature;
            }
            return null;
        }




        /// <MetaDataID>{9A8139D8-2084-4953-9CE9-D677054859E9}</MetaDataID>
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
                foreach (var attributeRealization in Features.OfType<AttributeRealization>().Where(x => x.Specification != null))
                    attributesPersistency[attributeRealization.Specification] = attributeRealization.Persistent;

                foreach (Class _class in GetAllGeneralClasifiers().OfType<Class>())
                    foreach (var attributeRealization in _class.Features.OfType<AttributeRealization>())
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




        /// <MetaDataID>{C4372067-3C3A-45D0-A068-EE57F7E47ADE}</MetaDataID>
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

                foreach (Class _class in GetAllGeneralClasifiers().OfType<Class>())
                    foreach (var associationEndRealization in _class.Features.OfType<AssociationEndRealization>())
                        if (!associationEndsPersistency.ContainsKey(associationEndRealization.Specification) || associationEndsPersistency[associationEndRealization.Specification] == false)
                            associationEndsPersistency[associationEndRealization.Specification] = associationEndRealization.Persistent;

                lock (MembersSpecializationPropertiesLock)
                {
                    if (AssociationEndsPersistency == null)
                        AssociationEndsPersistency = new Dictionary<AssociationEnd, bool>(associationEndsPersistency);
                    if (AssociationEndsPersistency.TryGetValue(associationEnd, out isPersistent))
                        return isPersistent;
                    if (associationEnd.Association == ClassHierarchyLinkAssociation && Persistent)
                    {
                        AssociationEndsPersistency[associationEnd] = true;
                        return true;
                    }
                    else
                    {
                        AssociationEndsPersistency[associationEnd] = false;
                        return false;
                    }
                }

            }


        }


        /// <MetaDataID>{F0D03E80-845F-4865-ACBD-FA0048DD5732}</MetaDataID>
        private bool? _HasReferentialIntegrityRelations;

        /// <MetaDataID>{61777AB2-4666-4A35-ABA8-23AE8A53F04F}</MetaDataID>
        public virtual bool HasReferentialIntegrityRelations()
        {
            lock (RolesLock)
            {

                if (!_HasReferentialIntegrityRelations.HasValue)
                {

                    foreach (AssociationEnd associationEnd in GetRoles(true))
                    {
                        if (associationEnd.HasBehavioralSettings && associationEnd.ReferentialIntegrity)
                        {
                            _HasReferentialIntegrityRelations = true;
                            return true;
                        }
                        else
                        {
                            foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
                            {
                                if (associationEndRealization.HasBehavioralSettings && associationEndRealization.ReferentialIntegrity)
                                {
                                    _HasReferentialIntegrityRelations = true;
                                    return true;
                                }
                            }
                        }
                        if (associationEnd.Association.LinkClass != null)
                        {
                            //Δεν μπορέις να σβήσεις ένα object όπου υπάρχει
                            //relation object μεταξύ αυτού και κάποιου άλλου γιάτι το
                            //relation object θα μετατραπεί σε object χωρίς έννοια.
                            //θα πρέπει πρώτα να σβήσεις το relation object.
                            //Υπάρχει περίπτωση κάποιο από τις sub type της 
                            //link class να έχει referential integrity. 
                            //Για αυτό το λόγο θα πρέπει η class αυτή πρέπει 
                            //να είναι έτοιμη να δεχτεί referential integrity count advance.
                            _HasReferentialIntegrityRelations = true;

                            return _HasReferentialIntegrityRelations.Value;
                        }
                    }
                    _HasReferentialIntegrityRelations = false;
                    return false;
                }
                return _HasReferentialIntegrityRelations.Value;
            }

        }
        /// Cache data
        /// <MetaDataID>{9B50601D-AF7F-4A81-9C47-D479605FE342}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<Interface> ClassHierarchyInterfaces = null;
        /// <MetaDataID>{ba98a481-c914-4a45-a685-1ee994005e63}</MetaDataID>
        private Collections.Generic.Dictionary<AssociationEnd, int> _AssociationEndsLoadPolicy = null;

        /// <MetaDataID>{e1ae2237-bda6-4350-b7b2-3902744f030c}</MetaDataID>
        private Collections.Generic.Dictionary<AssociationEnd, int> AssociationEndsLoadPolicy
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
                foreach (Class _class in GetAllGeneralClasifiers().OfType<Class>())
                {
                    foreach (var associationEndRealization in _class.Features.OfType<AssociationEndRealization>().Where(x => x.HasBehavioralSettings))
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


        /// <MetaDataID>{053b7770-a3db-4131-aeb6-83d387d91032}</MetaDataID>
        private Collections.Generic.Dictionary<AssociationEnd, bool> AssociationEndsPersistency = null;
        /// <MetaDataID>{ed1e4ee5-2024-4ba0-a466-3ac14a5f8fdf}</MetaDataID>
        private Collections.Generic.Dictionary<Attribute, bool> AttributePersistency = null;

        private Collections.Generic.Dictionary<Attribute, bool> AttributeMultilingualism = null;

        private Collections.Generic.Dictionary<AssociationEnd, bool> AssociationEndMultilingualism = null;


        /// <MetaDataID>{B72B71BD-90B1-42FF-8EBA-DFC1DD253641}</MetaDataID>
        /// <summary>This method retrieves the all realized interfaces of Class hierarchy. Note this method is recursive.  
        /// To retrieve only interfaces that realized from  this Class call the GetAllInterfaces. </summary>
        public OOAdvantech.Collections.Generic.Set<Interface> GetAllInterfaces()
        {


            lock (RealizationsLock)
            {

                if (ClassHierarchyInterfaces != null)
                    return ClassHierarchyInterfaces;
                else
                {
                    var interfaces = new OOAdvantech.Collections.Generic.Set<Interface>();
                    foreach (Generalization generalization in Generalizations)
                    {
                        if (generalization.Parent != null)
                        {
                            OOAdvantech.Collections.Generic.Set<Interface> parentCalssInterfaces = (generalization.Parent as Class).GetAllInterfaces();
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
                    ClassHierarchyInterfaces = new Set<Interface>(interfaces, Collections.CollectionAccessType.ReadOnly);
                    return ClassHierarchyInterfaces;
                }
            }



        }
        /// <MetaDataID>{77242655-A827-4127-B9B9-C7714681CA04}</MetaDataID>
        /// <summary>This method retrieves only the interfaces that realized from  this Class. </summary>
        public OOAdvantech.Collections.Generic.Set<Interface> GetInterfaces()
        {
            OOAdvantech.Collections.Generic.Set<Interface> interfaces = new OOAdvantech.Collections.Generic.Set<Interface>();
            foreach (Realization realization in Realizations.ToThreadSafeList())
                interfaces.Add(realization.Abstarction);
            return interfaces;
        }
        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{6DE30F1A-4E3E-4226-8ED0-AA2B8737F6E5}</MetaDataID>
        //private OOAdvantech.Collections.Generic.Set<Method> _Methods = new OOAdvantech.Collections.Generic.Set<Method>();
        ///// <summary>Mitsos </summary>
        ///// <MetaDataID>{DB032D95-7746-4A19-BA71-235ADFC5DD50}</MetaDataID>
        //[Association("ClassImplementation", typeof(OOAdvantech.MetaDataRepository.Method), MetaDataRepository.Roles.RoleA, "{42A65BA1-99AF-466A-A58B-9444F1E61903}")]
        //[PersistentMember("_Methods")]
        //[RoleAMultiplicityRange(0)]
        //[RoleBMultiplicityRange(1, 1)]
        //public OOAdvantech.Collections.Generic.Set<Method> Methods
        //{
        //    get
        //    {

        //        ReaderWriterLock.AcquireReaderLock(10000);
        //        try
        //        {
        //            return new OOAdvantech.Collections.Generic.Set<Method>(_Methods, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
        //        }
        //        finally
        //        {
        //            ReaderWriterLock.ReleaseReaderLock();
        //        }
        //    }
        //}
        /// <MetaDataID>{324f9bec-d655-4314-a34f-87a8b5020ca3}</MetaDataID>
        public override void RefreshClassHierarchyCollections()
        {
            _HasReferentialIntegrityRelations = null;
            lock (GeneralizationLock)
            {
                ClassHierarchyLinkAssociationCashed = false;
            }
            lock (RealizationsLock)
            {
                ClassHierarchyInterfaces = null;
            }
            lock (MembersSpecializationPropertiesLock)
            {
                _AssociationEndsLoadPolicy = null;
                AssociationEndMultilingualism = null;
                AssociationEndsPersistency = null;
                PersistentAssociateRoles = null; //for refresh
                PersistentAttributes = null; //for refresh
            }
            base.RefreshClassHierarchyCollections();
        }




        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        /// <MetaDataID>{D0ACF9B2-2AA4-4146-9A1D-ADE2980B99A8}</MetaDataID>
        public override void Synchronize(MetaObject originMetaObject)
        {


            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            RefreshClassHierarchyCollections();


            try
            {
                if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                    return;
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {

                    if (!(originMetaObject is MetaDataRepository.Class))
                    {
                        string tt = originMetaObject.GetType().FullName;
                    }
                    MetaDataRepository.Class originClass = (MetaDataRepository.Class)originMetaObject;
                    #region Sychronize realization relationship
                    ContainedItemsSynchronizer RealizationSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(originClass.Realizations, _Realizations, this);
                    RealizationSynchronizer.FindModifications();
                    RealizationSynchronizer.ExecuteAddCommand();
                    RealizationSynchronizer.ExecuteDeleteCommand();
                    RealizationSynchronizer.Synchronize();
                    foreach (MetaDataRepository.Realization realization in _Realizations)
                        realization.Implementor = this;
                    #endregion

                    base.Synchronize(originMetaObject);
                    MetaDataRepository.SynchronizerSession.MetaObjectUnderSynchronization(this);


                    _Persistent = originClass.Persistent;
                    _Abstract = originClass.Abstract;


                    _HasReferentialIntegrityRelations = null;
                    lock (GeneralizationLock)
                    {
                        ClassHierarchyLinkAssociationCashed = false;
                    }
                    lock (RealizationsLock)
                    {
                        ClassHierarchyInterfaces = null;
                    }

                    lock (MembersSpecializationPropertiesLock)
                    {

                        _AssociationEndsLoadPolicy = null;
                        AssociationEndsPersistency = null;
                    }

                    BuildCaseInsensitiveNames();

                    StateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{C8B5C952-C5A3-4E02-8754-7A3694A31070}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private bool InErrorCheck = false;

        /// <MetaDataID>{EBEBC28B-7C24-41CB-8696-491888AE5AB6}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {
            if (InErrorCheck)
                return false;
            try
            {
                InErrorCheck = true;
                bool hasErrors = base.ErrorCheck(ref errors);
                if (Persistent)
                {
                    bool hasPersistentMember = false;
                    foreach (Attribute attribute in GetAttributes(true))
                    {
                        if (IsPersistent(attribute))
                            hasPersistentMember = true;
                    }
                    foreach (AssociationEnd associationEnd in GetAssociateRoles(true))
                    {
                        if (IsPersistent(associationEnd))
                            hasPersistentMember = true;
                    }
                    if (!hasPersistentMember)
                    {
                        //hasErrors = true;
                        //errors.Add(new MetaObject.MetaDataError("MDR Error: The class '" + FullName + "' is persistent and it hasn't persistent members", FullName));
                    }

                    System.Collections.Generic.List<string> recursivePaths = new System.Collections.Generic.List<string>();
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd role in GetAssociateRoles(true))
                    {

                        System.Collections.Generic.List<Classifier> classifiers = new System.Collections.Generic.List<Classifier>();
                        classifiers.Add(this);

                        if (!IsLazyFetching(role) && !IsTryOnObjectActivationFetching(role))
                            Check(role, classifiers, this.FullName, recursivePaths, 0);
                    }
                    foreach (string recursivePath in recursivePaths)
                    {
                        errors.Add(new MetaObject.MetaDataError(string.Format("MDR Error: Recursive auto Object Activation path {0}", recursivePath), FullName));
                        hasErrors = true;
                    }
                }
                return hasErrors;

            }
            finally
            {
                InErrorCheck = false;
            }
        }

        /// <MetaDataID>{705f95ba-4684-45ea-ad82-702777eb7750}</MetaDataID>
        private static bool Check(AssociationEnd role, System.Collections.Generic.List<Classifier> classifiers, string path, System.Collections.Generic.List<string> recursivePaths, int recursionStep)
        {

            bool decrease = false;
            try
            {
                //  if (!role.Multiplicity.IsMany)
                {
                    foreach (Classifier classifier in classifiers)
                    {
                        if (classifier.IsA(role.Specification))
                        {
                            if (recursionStep < 1)
                            {
                                decrease = true;
                                recursionStep++;
                            }
                            else
                            {

                                if (!recursivePaths.Contains(path + "." + role.Name))
                                    recursivePaths.Add(path + "." + role.Name);
                                return true;
                            }
                        }
                    }
                    foreach (AssociationEnd associationEndRole in role.Specification.GetAssociateRoles(true))
                    {
                        if (!associationEndRole.Multiplicity.IsMany && associationEndRole.GetOtherEnd() == role)
                            continue;

                        bool onConstruction = !associationEndRole.LazyFetching;
                        bool tryOnObjectActivationFetching = associationEndRole.TryOnObjectActivationFetching;
                        if (role.Specification is Class)
                        {
                            onConstruction = !(role.Specification as Class).IsLazyFetching(associationEndRole);
                            if (onConstruction)
                                tryOnObjectActivationFetching = (role.Specification as Class).IsTryOnObjectActivationFetching(associationEndRole);


                        }
                        if (!onConstruction || tryOnObjectActivationFetching)
                        {
                            foreach (OOAdvantech.MetaDataRepository.Classifier specClassifier in role.Specification.GetAllSpecializeClasifiers())
                            {
                                if (specClassifier is OOAdvantech.MetaDataRepository.Class)
                                    if (!(specClassifier as OOAdvantech.MetaDataRepository.Class).IsLazyFetching(associationEndRole))
                                    {
                                        onConstruction = true;
                                        tryOnObjectActivationFetching = (specClassifier as Class).IsTryOnObjectActivationFetching(associationEndRole);
                                        if (onConstruction && !tryOnObjectActivationFetching)
                                            break;
                                    }

                            }
                        }
                        if (onConstruction && !tryOnObjectActivationFetching)
                        {
                            bool removeClasifier = false;
                            try
                            {
                                if (!classifiers.Contains(role.Specification))
                                {
                                    classifiers.Add(role.Specification);
                                    removeClasifier = true;
                                }
                                if (Check(associationEndRole, classifiers, path + "." + role.Name, recursivePaths, recursionStep))
                                    break;

                            }
                            finally
                            {
                                if (removeClasifier)
                                    classifiers.Remove(role.Specification);
                            }
                        }
                    }
                }
                return false;
            }
            catch (System.Exception error)
            {
                return false;
            }
            finally
            {
                if (decrease)
                    recursionStep--;
            }

        }



        /// <MetaDataID>{D013D2D6-0245-4ABA-9331-F190209B5FEA}</MetaDataID>
        public Class()
        {
            _Persistent = false;
            Visibility = VisibilityKind.AccessPublic;
            _Abstract = false;
            _HasReferentialIntegrityRelations = null;

        }
        /// <MetaDataID>{80602734-23C8-4E2C-B43C-08E73808BBBC}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<AssociationEnd> GetRoles(bool Inherit)
        {
            lock (ClassHierarchyLock)
            {
                if (ClassHierarchyRoles != null && Inherit)
                    ClassHierarchyRoles.ToThreadSafeSet();
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

        /// <MetaDataID>{671f6bc0-9ffb-44ae-a9ac-aecf973bd2a4}</MetaDataID>
        public override Set<Feature> GetFeatures(bool Inherit)
        {

            lock (ClassHierarchyLock)
            {
                if (ClassHierarchyFeatures != null && Inherit)
                    return ClassHierarchyFeatures.ToThreadSafeSet();
            }
            OOAdvantech.Collections.Generic.Set<Feature> features = new OOAdvantech.Collections.Generic.Set<Feature>();
            lock (FeaturesLock)
            {


                foreach (Feature mFeature in Features)
                    features.Add(mFeature);

                if (Inherit == false)
                    return new OOAdvantech.Collections.Generic.Set<Feature>(features);

                foreach (Generalization generalization in Generalizations)
                {
                    if (generalization.Parent!=null)
                        features.AddRange(generalization.Parent.GetFeatures(Inherit));
                }

                foreach (Realization realization in Realizations)
                {
                    if (realization.Abstarction!=null)
                        features.AddRange(realization.Abstarction.GetFeatures(Inherit));

                }
                lock (ClassHierarchyLock)
                {
                    if (ClassHierarchyFeatures == null)
                        ClassHierarchyFeatures = new OOAdvantech.Collections.Generic.Set<Feature>(features);
                    return ClassHierarchyFeatures.AsReadOnly();
                }
            }
        }


            /// <MetaDataID>{bc6f5d42-f273-4954-92f7-ce466abe33d6}</MetaDataID>
            System.Collections.Generic.List<AssociationEnd> PersistentAssociateRoles;
            /// <MetaDataID>{5b60f960-b19a-418b-ab07-caa7a2bdeaaf}</MetaDataID>
            public System.Collections.Generic.List<AssociationEnd> GetPersistentAssociateRoles()
            {
                lock (MembersSpecializationPropertiesLock)
                {
                    if (PersistentAssociateRoles != null)
                        return PersistentAssociateRoles;
                }
                var persistentAssociateRoles = new System.Collections.Generic.List<AssociationEnd>();
                foreach (AssociationEnd associationEnd in GetAssociateRoles(true))
                {
                    if (!IsPersistent(associationEnd))
                        continue;
                    persistentAssociateRoles.Add(associationEnd);
                }
                lock (MembersSpecializationPropertiesLock)
                {
                    PersistentAssociateRoles = persistentAssociateRoles;
                    return PersistentAssociateRoles;
                }

            }


            /// <MetaDataID>{34a591cb-fa29-4020-8b6a-aabf75c3765e}</MetaDataID>
            System.Collections.Generic.List<Attribute> PersistentAttributes;

            /// <MetaDataID>{0f22fb60-b626-4e55-8c77-4b3fe4609080}</MetaDataID>
            public System.Collections.Generic.List<Attribute> GetPersistentAttributes()
            {
                lock (MembersSpecializationPropertiesLock)
                {
                    if (PersistentAttributes != null)
                        return PersistentAttributes;
                }
                var persistentAttributes = new System.Collections.Generic.List<Attribute>();
                foreach (Attribute attribute in GetAttributes(true))
                {
                    if (!IsPersistent(attribute))
                        continue;
                    persistentAttributes.Add(attribute);
                }

                lock (MembersSpecializationPropertiesLock)
                {
                    PersistentAttributes = persistentAttributes;
                    return PersistentAttributes;
                }
            }


        /// <MetaDataID>{927C80F2-9E9A-4B31-A581-849CBFF969EE}</MetaDataID>
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
        /*/// <MetaDataID>{FB3A95E2-A5EF-4C5B-93EE-B9788E71C119}</MetaDataID>
        [MetaDataRepository.PersistentMember("13")]
        public bool Abstract;*/
        /// <summary>This method creates a new realize relation and adds it to a class. </summary>
        /// <param name="relationName">Name of the relation being added </param>
        /// <param name="_interface">Name of the interface with which to create the realize relation.
        /// If this name is not unique, you must use the qualified name (for example, 
        /// Namespace::Namespace(...)::supplier_name  CPlusplus Style or Namespace.Namespace(...).supplier_name CSharp Style) </param>
        /// <MetaDataID>{7827FEE5-9D00-404A-864B-BAAECC65A2D5}</MetaDataID>
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

        /// <summary>This method deletes an method from a class. </summary>
        /// <param name="method">Method being deleted from the class. </param>
        /// <MetaDataID>{05FBF7DE-F290-42A5-86DF-44B4A456A174}</MetaDataID>
        public virtual void RemoveMethod(Method method)
        {
            if (method == null)
                return;
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {

                    // _Methods.Remove(method);
                    method.Specification.RemoveOperationImplementation(method);
                    _Features.Remove(method);
                    stateTransition.Consistent = true;
                }
            }
            MetaObjectChangeState();
        }



        /// <MetaDataID>{44ecbde8-037f-43e1-be6b-2889c2e78fcc}</MetaDataID>
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
            MetaObjectChangeState();


        }
        /// <MetaDataID>{d08164de-de72-467a-bdfc-b2c54564a278}</MetaDataID>
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

            MetaObjectChangeState();
        }



        /// <summary>This method deletes a realize relation from a class. </summary>
        /// <param name="realizeRel">Realize relation being deleted. </param>
        /// <MetaDataID>{663DC2B8-6C7A-410A-8728-2752300135B3}</MetaDataID>
        public void RemoveRealizeRel(Realization realizeRel)
        {
            if (realizeRel == null)
                return;
            lock (RealizationsLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _Realizations.Remove(realizeRel);
                    stateTransition.Consistent = true;
                }
            }

        }
        /// <summary>This method deletes an nested class from a class. </summary>
        /// <param name="theNestedClass">Nested class being deleted. </param>
        /// <MetaDataID>{B96CB972-8897-4100-95FE-90789F7E702D}</MetaDataID>
        public void RemoveNestedClass(Class theNestedClass)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _OwnedElements.Remove(theNestedClass);
                    StateTransition.Consistent = true; ;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <summary>This method retrieves all classes nested within the specified class and all of its nested classes.
        /// For example:  If Class A has 2 nested classes, NClass1 and NClass2, and NClass1 has a nested class, NestedCls, applying the GetAllNestedClasses method to Class A returns all 3 nested classes, NClass1, NClass2, and NestedCls, not just the first-level nested classes.
        /// To retrieve only the first-level nested classes for the specified class, use GetNestedClasses. </summary>
        /// <MetaDataID>{643B775D-C7DB-440D-B97B-5A7C325C42BC}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Classifier> GetAllNestedClasses()
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                OOAdvantech.Collections.Generic.Set<Classifier> nestedClasses = new OOAdvantech.Collections.Generic.Set<Classifier>(NestedClasses);
                foreach (Class _class in nestedClasses)
                    nestedClasses.AddRange(_class.GetAllNestedClasses());
                return nestedClasses;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }
        /// <summary>This method retrieves the first-level nested class collection from the class and returns it in the specified object. To retrieve all nested classes of the specified class and all of its nested classes, use GetAllNestedClasses. </summary>
        /// <MetaDataID>{D742815B-4003-4AB0-B6BE-9B78936DAF21}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Classifier> NestedClasses
        {
            set
            {
            }
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    OOAdvantech.Collections.Generic.Set<Classifier> nestedClasses = new OOAdvantech.Collections.Generic.Set<Classifier>();
                    foreach (MetaObject metaObject in _OwnedElements)
                    {
                        if (metaObject is Classifier)
                            nestedClasses.Add(metaObject as Classifier);
                    }
                    return nestedClasses;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <summary>This method creates a new nested class and adds it to a class. </summary>
        /// <param name="Name">Name of the class being added to the class </param>
        /// <MetaDataID>{8FCA0E35-9E78-4C1C-9ED8-A4E650E63E50}</MetaDataID>
        public Class AddNestedClass(string Name)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    MetaDataRepository.Class Class = new MetaDataRepository.Class(); //(MetaDataRepository.Class)PersistenceLayer.PersistencyContext.CurrentPersistencyContext.NewObject("MetaDataRepository.Class","",theStorageInstanceRef.ActiveStorageSession);
                    Class.Name = Name;
                    _OwnedElements.Add(Class);
                    StateTransition.Consistent = true; ;
                    return Class;/**/
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }


        /// <summary>Add a method that is the implementation of an operation. It specifies the algorithm or procedure that effects the results of an operation. The Operation must be member of Class or Member of supperclass or member of realized interface </summary>
        /// <param name="specification">The Operation wa method which implement the method </param>
        /// <MetaDataID>{A0026316-38E7-482B-8241-4B714F7A78E6}</MetaDataID>
        public Method AddMethod(Operation specification)
        {

            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    Method method = new Method(specification);// objectStorage.NewObject(typeof(Method), ctorTypes, specification) as Method;
                    _Features.Remove(method);
                    OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    if (objectStorage != null)
                        objectStorage.CommitTransientObjectState(method);
                    stateTransition.Consistent = true;
                    return method;
                }
            }
        }

        /// <MetaDataID>{a5e92631-6be6-449f-9f43-5488a871933c}</MetaDataID>
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
        /// <MetaDataID>{7bfd37e7-7ebe-4fb7-adbe-94edf124c6a7}</MetaDataID>
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
    }
}
