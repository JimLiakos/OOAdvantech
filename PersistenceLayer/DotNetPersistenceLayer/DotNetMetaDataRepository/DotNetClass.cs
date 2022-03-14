using System.Linq;
#if PORTABLE 
using System.PCL.Reflection;
#else
using System.Reflection;
#endif
namespace OOAdvantech.DotNetMetaDataRepository
{
    using System.Threading.Tasks;
    using MetaDataRepository;
    using Transactions;
    /// <MetaDataID>{6C4F40AA-77CD-4B81-AFAF-E83AAB3F9269}</MetaDataID>
    public class Class : MetaDataRepository.Class
    {




        private readonly object ClassObjectLock = new object();
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {

            if (member.Name == nameof(HierarchyRealizationsLoaded))
            {
                if (value == null)
                    HierarchyRealizationsLoaded = default(bool);
                else
                    HierarchyRealizationsLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_HasReferentialIntegrityRelations))
            {
                if (value == null)
                    _HasReferentialIntegrityRelations = default(OOAdvantech.Member<bool>);
                else
                    _HasReferentialIntegrityRelations = (OOAdvantech.Member<bool>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LinkClassRoleAFastFieldAccessor))
            {
                if (value == null)
                    _LinkClassRoleAFastFieldAccessor = default(OOAdvantech.AccessorBuilder.FieldPropertyAccessor);
                else
                    _LinkClassRoleAFastFieldAccessor = (OOAdvantech.AccessorBuilder.FieldPropertyAccessor)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LinkClassRoleBFastFieldAccessor))
            {
                if (value == null)
                    _LinkClassRoleBFastFieldAccessor = default(OOAdvantech.AccessorBuilder.FieldPropertyAccessor);
                else
                    _LinkClassRoleBFastFieldAccessor = (OOAdvantech.AccessorBuilder.FieldPropertyAccessor)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LinkClassRoleAField))
            {
                if (value == null)
                    _LinkClassRoleAField = default(System.Reflection.FieldInfo);
                else
                    _LinkClassRoleAField = (System.Reflection.FieldInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LinkClassRoleBField))
            {
                if (value == null)
                    _LinkClassRoleBField = default(System.Reflection.FieldInfo);
                else
                    _LinkClassRoleBField = (System.Reflection.FieldInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_NestedClasses))
            {
                if (value == null)
                    _NestedClasses = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>);
                else
                    _NestedClasses = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(Refer))
            {
                if (value == null)
                    Refer = default(OOAdvantech.DotNetMetaDataRepository.Type);
                else
                    Refer = (OOAdvantech.DotNetMetaDataRepository.Type)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            //if (member.Name == nameof(CachedClassHierarchyAssociateRoles))
            //{
            //    lock (ClassHierarcyObjectLock)
            //    {
            //        if (value == null)
            //            CachedClassHierarchyAssociateRoles = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>);
            //        else
            //            CachedClassHierarchyAssociateRoles = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>)value;
            //    }
            //    return ObjectMemberGetSet.MemberValueSetted;
            //}
            //if (member.Name == nameof(CachedAssociateRoles))
            //{
            //    lock (RolesLock)
            //    {
            //        if (value == null)
            //            CachedAssociateRoles = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>);
            //        else
            //            CachedAssociateRoles = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>)value;
            //        return ObjectMemberGetSet.MemberValueSetted;
            //    }
            //}
            if (member.Name == nameof(InErrorCheck))
            {
                if (value == null)
                    InErrorCheck = default(bool);
                else
                    InErrorCheck = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ExtensionMetaObjects))
            {
                lock (ExtensionMetaObjectsLock)
                {
                    if (value == null)
                        ExtensionMetaObjects = default(System.Collections.Generic.List<object>);
                    else
                        ExtensionMetaObjects = (System.Collections.Generic.List<object>)value;
                    return ObjectMemberGetSet.MemberValueSetted;
                }
            }
            if (member.Name == nameof(ClassInstance))
            {
                if (value == null)
                    ClassInstance = default(object);
                else
                    ClassInstance = (object)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_FullName))
            {
                if (value == null)
                    _FullName = default(string);
                else
                    _FullName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssociationEndFields))
            {
                if (value == null)
                    AssociationEndFields = default(System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, System.Reflection.FieldInfo>);
                else
                    AssociationEndFields = (System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, System.Reflection.FieldInfo>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssociationEndFastFieldsAccessors))
            {
                if (value == null)
                    AssociationEndFastFieldsAccessors = default(System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor>);
                else
                    AssociationEndFastFieldsAccessors = (System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AttributeFastFieldsAccessors))
            {
                if (value == null)
                    AttributeFastFieldsAccessors = default(System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.Attribute, OOAdvantech.AccessorBuilder.FieldPropertyAccessor>);
                else
                    AttributeFastFieldsAccessors = (System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.Attribute, OOAdvantech.AccessorBuilder.FieldPropertyAccessor>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(TransactionalMembers))
            {
                if (value == null)
                    TransactionalMembers = default(System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, System.Reflection.MemberInfo>);
                else
                    TransactionalMembers = (System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, System.Reflection.MemberInfo>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(FastFieldAccessors))
            {
                if (value == null)
                    FastFieldAccessors = default(System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor>);
                else
                    FastFieldAccessors = (System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssociationEndTransactionlMember))
            {
                if (value == null)
                    AssociationEndTransactionlMember = default(System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, System.Reflection.MemberInfo>);
                else
                    AssociationEndTransactionlMember = (System.Collections.Generic.Dictionary<OOAdvantech.DotNetMetaDataRepository.AssociationEnd, System.Reflection.MemberInfo>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(RolesLoaded))
            {
                if (value == null)
                    RolesLoaded = default(bool);
                else
                    RolesLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(RealizationsLoaded))
            {
                if (value == null)
                    RealizationsLoaded = default(bool);
                else
                    RealizationsLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(GeneralizationsLoaded))
            {
                if (value == null)
                    GeneralizationsLoaded = default(bool);
                else
                    GeneralizationsLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(FeaturesLoaded))
            {
                if (value == null)
                    FeaturesLoaded = default(bool);
                else
                    FeaturesLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ID))
            {
                if (value == null)
                    ID = default(string);
                else
                    ID = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(HierarchyRealizationsLoaded))
                return HierarchyRealizationsLoaded;

            if (member.Name == nameof(_HasReferentialIntegrityRelations))
                return _HasReferentialIntegrityRelations;

            if (member.Name == nameof(_LinkClassRoleAFastFieldAccessor))
                return _LinkClassRoleAFastFieldAccessor;

            if (member.Name == nameof(_LinkClassRoleBFastFieldAccessor))
                return _LinkClassRoleBFastFieldAccessor;

            if (member.Name == nameof(_LinkClassRoleAField))
                return _LinkClassRoleAField;

            if (member.Name == nameof(_LinkClassRoleBField))
                return _LinkClassRoleBField;

            if (member.Name == nameof(_NestedClasses))
                return _NestedClasses;

            if (member.Name == nameof(Refer))
                return Refer;

            //if (member.Name == nameof(CachedFeatures))
            //    return CachedFeatures;

            //if (member.Name == nameof(CachedClassHierarchyAssociateRoles))
            //    return CachedClassHierarchyAssociateRoles;

            //if (member.Name == nameof(CachedAssociateRoles))
            //    return CachedAssociateRoles;

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(ExtensionMetaObjects))
                return GetExtensionMetaObjects();

            if (member.Name == nameof(ClassInstance))
                return ClassInstance;

            if (member.Name == nameof(_FullName))
                return _FullName;

            if (member.Name == nameof(AssociationEndFields))
                return AssociationEndFields;

            if (member.Name == nameof(AssociationEndFastFieldsAccessors))
                return AssociationEndFastFieldsAccessors;

            if (member.Name == nameof(AttributeFastFieldsAccessors))
                return AttributeFastFieldsAccessors;

            if (member.Name == nameof(TransactionalMembers))
                return TransactionalMembers;

            if (member.Name == nameof(FastFieldAccessors))
                return FastFieldAccessors;

            if (member.Name == nameof(AssociationEndTransactionlMember))
                return AssociationEndTransactionlMember;

            if (member.Name == nameof(RolesLoaded))
                return RolesLoaded;

            if (member.Name == nameof(RealizationsLoaded))
                return RealizationsLoaded;

            if (member.Name == nameof(GeneralizationsLoaded))
                return GeneralizationsLoaded;

            if (member.Name == nameof(FeaturesLoaded))
                return FeaturesLoaded;

            if (member.Name == nameof(ID))
                return ID;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{68e45805-bcff-4102-ab14-63e27ff34287}</MetaDataID>
        public override bool Persistent
        {
            get
            {
                if (_Persistent && Refer.WrType.GetMetaData().Assembly == typeof(Class).GetMetaData().Assembly)
                    _Persistent = false;
                return _Persistent;
            }
            set
            {
                base.Persistent = value;
            }
        }

        /// <MetaDataID>{5737be57-d47b-4ed5-a133-967d92229805}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }

        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }

        /// <MetaDataID>{985E7F5D-3BB9-452E-B473-D4D8AF421DF4}</MetaDataID>
        bool HierarchyRealizationsLoaded = false;
        /// <MetaDataID>{3979268B-6125-49DA-823C-2D7926A33662}</MetaDataID>
        protected Class()
        {
        }
        public override Collections.Generic.Set<Generalization> Specializations => _Specializations.ToThreadSafeSet();
        /// <MetaDataID>{bb3f7aff-ef4a-4d32-ac49-e23d58d8a786}</MetaDataID>
        public override OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.Operation> Constractors
        {
            get
            {
                return base.Constractors;
            }
        }
        /// <MetaDataID>{012473E0-F8A2-4DFF-B290-5C263F09A9F2}</MetaDataID>
        internal void LoadHierarchyRealizations()
        {
            //lock (ClassHierarcyObjectLock)
            {
                using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if (HierarchyRealizationsLoaded)
                        return;
                    long count = Features.Count;
                    foreach (MetaDataRepository.Generalization specialization in _Specializations.ToThreadSafeSet())
                    {
                        (specialization.Child as Class).LoadHierarchyRealizations();
                    }
                    HierarchyRealizationsLoaded = true;
                }
            }
        }
        /// <MetaDataID>{9da49144-2d19-40d0-a27f-ae2e3bac0012}</MetaDataID>
        Member<bool> _HasReferentialIntegrityRelations = new Member<bool>();
        /// <MetaDataID>{93626F36-C20E-41C0-A210-BE84EB6FA857}</MetaDataID>
        public override bool HasReferentialIntegrityRelations()
        {
            //lock (ClassHierarcyObjectLock)
            {
                using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if (_HasReferentialIntegrityRelations.UnInitialized)
                    {
                        foreach (AssociationEnd associationEnd in GetAssociateRoles(true))
                        {
                            if (associationEnd.Specification is Class)
                                (associationEnd.Specification as Class).LoadHierarchyRealizations();
                            if (associationEnd.Specification is Interface)
                                (associationEnd.Specification as Interface).LoadHierarchyRealizations();

                        }
                        _HasReferentialIntegrityRelations.Value = base.HasReferentialIntegrityRelations();
                    }
                    return _HasReferentialIntegrityRelations.Value;
                }
            }
        }


        /// <MetaDataID>{1d96f000-67c4-4a45-971f-b9ff3d2245ac}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Association LinkAssociation
        {
            get
            {
                if (!RolesLoaded)
                {
                    long loadRoles = Roles.Count;
                }
                return _LinkAssociation;
            }
            set
            {

            }
        }



        #region Association Class Roles Fields
        /// <MetaDataID>{A18FB265-F60B-4A3B-8808-A8ECDA65EFB4}</MetaDataID>
        void GetLinkClassRoleFields()
        {
            if (ClassHierarchyLinkAssociation == null)
                throw new System.Exception("The class " + FullName + " it isn't link class.");

            using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                var features = Features;
                if (ClassHierarchyLinkAssociation != null)
                {
                    // lock (ClassHierarcyObjectLock)
                    {
                        foreach (MetaDataRepository.Feature feature in features)
                        {
                            System.Reflection.MemberInfo memberInfo = null;
                            if (feature is Attribute && (feature as Attribute).wrMember.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), false).Length > 0)
                                memberInfo = (feature as Attribute).wrMember;

                            if (feature is AttributeRealization && (feature as AttributeRealization).PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), false).Length > 0)
                                memberInfo = (feature as AttributeRealization).PropertyMember;

                            if (memberInfo != null)
                            {
                                if (memberInfo is System.Reflection.PropertyInfo && (memberInfo as System.Reflection.PropertyInfo).GetAccessors()[0].IsAbstract)
                                    continue;
                                MetaDataRepository.AssociationClassRole AssociationClassRole = memberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), true)[0] as MetaDataRepository.AssociationClassRole;
                                System.Reflection.FieldInfo FieldRole = null;
                                if (memberInfo is System.Reflection.FieldInfo)
                                    FieldRole = memberInfo as System.Reflection.FieldInfo;
                                else
                                    FieldRole = memberInfo.DeclaringType.GetMetaData().GetField(AssociationClassRole.ImplMemberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); // Error Prone if FieldRoleA==null;

                                if (AssociationClassRole.IsRoleA)
                                    _LinkClassRoleAField = FieldRole;
                                else
                                    _LinkClassRoleBField = FieldRole;
                            }
                        }
                        if (_LinkClassRoleBField == null && _LinkClassRoleAField == null)
                        {
                            foreach (Class _class in GetGeneralClasifiers().OfType<Class>())
                            {

                                if (_class.ClassHierarchyLinkAssociation != null)
                                {
                                    _class.GetLinkClassRoleFields();
                                    _LinkClassRoleAField = (_class).LinkClassRoleAField;
                                    _LinkClassRoleBField = (_class).LinkClassRoleBField;
                                }
                            }
                        }
                    }
                }
            }
            //			else
            //			{
            //				_LinkClassRoleAField=(ClassHierarchyLinkAssociation.LinkClass as Class).LinkClassRoleAField;
            //				_LinkClassRoleBField=(ClassHierarchyLinkAssociation.LinkClass as Class).LinkClassRoleBField;
            //			}

        }
        /// <MetaDataID>{CE5A6C85-2FD9-406E-9A1C-2CE432944C1F}</MetaDataID>
        void RolesFieldsTypeCheck()
        {
            using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                if (_LinkClassRoleAField == null)
                    throw new System.Exception("MDR Error: There isn't field definition for role A in Association class '" + FullName + "'.");

                if (_LinkClassRoleBField == null)
                    throw new System.Exception("MDR Error: There isn't field definition for role B in Association class '" + FullName + "'.");

                System.Type RoleAType = ClassHierarchyLinkAssociation.RoleA.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                System.Type RoleBType = ClassHierarchyLinkAssociation.RoleB.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                string ErrorMessage = null;
                if (RoleAType != _LinkClassRoleAField.FieldType)
                    ErrorMessage += "\nMDR Error: RoleA type mismatch at " + _LinkClassRoleAField.DeclaringType + "." + _LinkClassRoleAField.Name + ".\n";

                if (RoleBType != _LinkClassRoleBField.FieldType)
                    ErrorMessage += "\nMDR Error: RoleB type mismatch at " + _LinkClassRoleBField.DeclaringType + "." + _LinkClassRoleBField.Name + ".\n";
                if (ErrorMessage != null)
                {
                    _LinkClassRoleAField = null;
                    _LinkClassRoleBField = null;
                    throw new System.Exception(ErrorMessage);
                }
            }
        }



        /// <exclude>Excluded</exclude>
        AccessorBuilder.FieldPropertyAccessor _LinkClassRoleAFastFieldAccessor;
        /// <MetaDataID>{e2095037-066c-4ccc-a582-3a329fb0d3c4}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor LinkClassRoleAFastFieldAccessor
        {
            get
            {

                if (_LinkClassRoleAFastFieldAccessor != null)
                    return _LinkClassRoleAFastFieldAccessor;
                else
                {
                    if (LinkClassRoleAField == null)
                        return _LinkClassRoleAFastFieldAccessor;
                    _LinkClassRoleAFastFieldAccessor = AccessorBuilder.GetFieldAccessor(LinkClassRoleAField);
                    return _LinkClassRoleAFastFieldAccessor;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        AccessorBuilder.FieldPropertyAccessor _LinkClassRoleBFastFieldAccessor;
        /// <MetaDataID>{fcb749c5-0f74-4436-a828-dd1db9a39607}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor LinkClassRoleBFastFieldAccessor
        {
            get
            {
                if (_LinkClassRoleBFastFieldAccessor != null)
                    return _LinkClassRoleBFastFieldAccessor;
                else
                {
                    if (LinkClassRoleBField == null)
                        return _LinkClassRoleBFastFieldAccessor;
                    _LinkClassRoleBFastFieldAccessor = AccessorBuilder.GetFieldAccessor(LinkClassRoleBField);
                    return _LinkClassRoleBFastFieldAccessor;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{97987987-CB03-46F0-8A5A-1A6F72EC2CA9}</MetaDataID>
        private System.Reflection.FieldInfo _LinkClassRoleAField;
        /// <MetaDataID>{ADFABA38-6934-482B-AD13-274AD00DA339}</MetaDataID>
        public System.Reflection.FieldInfo LinkClassRoleAField
        {
            get
            {
                if (_LinkClassRoleAField == null)
                {
                    GetLinkClassRoleFields();
                    RolesFieldsTypeCheck();
                }

                return _LinkClassRoleAField;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{86D4C490-3B78-4667-B8CC-6AD3C655BC85}</MetaDataID>
        private System.Reflection.FieldInfo _LinkClassRoleBField;
        /// <MetaDataID>{A6101327-0A90-4A3A-BFE0-6CB89FAD0B17}</MetaDataID>
        public System.Reflection.FieldInfo LinkClassRoleBField
        {
            get
            {
                if (_LinkClassRoleBField == null)
                {
                    GetLinkClassRoleFields();
                    RolesFieldsTypeCheck();
                }
                return _LinkClassRoleBField;
            }
        }
        #endregion



        /// <MetaDataID>{2806C68E-0D98-4026-84B0-36C989D26514}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Realization> Realizations
        {
            get
            {
                //lock (ClassHierarcyObjectLock)
                {
                    if (!RealizationsLoaded)
                        LoadRealizations();
                    return _Realizations.ToThreadSafeSet();
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2DF6B4B5-64E9-42EE-89D4-965A551866E2}</MetaDataID>
        private Collections.Generic.Set<MetaDataRepository.Classifier> _NestedClasses;
        /// <MetaDataID>{057AC18D-3173-41B8-B8DC-0D6BA9277E55}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Classifier> NestedClasses
        {
            get
            {

                lock (ClassObjectLock)
                {
                    using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        if (_NestedClasses == null)
                        {
                            Collections.Generic.Set<MetaDataRepository.Classifier> nestedClasses;
                            lock (Type.LoadDotnetMetadataLock)
                            {
                                nestedClasses = Refer.GetNestedClassifier();
                            }
                            foreach (MetaDataRepository.Classifier classifier in nestedClasses)
                            {
                                if (classifier is Class)
                                    (classifier as Class).SetNamespace(this);

                                if (classifier is Structure)
                                    (classifier as Structure).SetNamespace(this);

                                if (classifier is Interface)
                                    (classifier as Interface).SetNamespace(this);
                                _OwnedElements.Add(classifier);
                            }
                            _NestedClasses = nestedClasses;
                        }
                    }
                }


                return _NestedClasses.ToThreadSafeSet();
            }
        }
        /// <MetaDataID>{A22BEB50-9C15-49B5-8EB0-0C957FFBF28D}</MetaDataID>
        internal Type Refer;
        /// <MetaDataID>{838E39E8-B051-4257-8821-0714B9277BC6}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.MetaObject> OwnedElements
        {
            get
            {
                int count = 0;
                if (!FeaturesLoaded)
                {
                    //Load produce Features meta data;
                    count = Features.Count;
                }
                count = NestedClasses.Count;
                return _OwnedElements.ToThreadSafeSet();
            }
        }


        /// <MetaDataID>{8CA44CDB-B36A-4A9E-BDFA-17612D25EB91}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Feature> Features
        {
            get
            {
                lock (FeaturesLock)
                {
                    using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        if (!FeaturesLoaded)
                        {
                            Refer.GetFeatures(this, ref _Features);
                            foreach (MetaDataRepository.Feature feature in _Features)
                                _OwnedElements.Add(feature);
                            FeaturesLoaded = true;
                        }
                        return _Features.ToThreadSafeSet();
                    }
                }
            }
        }


        ///// <MetaDataID>{a5ac0703-49ca-4e7e-841b-ee3c66929f74}</MetaDataID>
        //Collections.Generic.Set<MetaDataRepository.AssociationEnd> CachedClassHierarchyAssociateRoles;
        ///// <MetaDataID>{c0157872-741f-4d14-907e-5811e5fc73e7}</MetaDataID>
        //Collections.Generic.Set<MetaDataRepository.AssociationEnd> CachedAssociateRoles;

        ///// <MetaDataID>{fbaa019c-d8ab-4c0a-97e5-335b8138b193}</MetaDataID>
        //public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd> GetAssociateRoles(bool Inherit)
        //{
        //    lock (ClassHierarcyObjectLock)
        //    {
        //        if (Inherit && CachedClassHierarchyAssociateRoles != null)
        //            return CachedClassHierarchyAssociateRoles;
        //        if (!Inherit && CachedAssociateRoles != null)
        //            return CachedAssociateRoles;
        //    }

        //    if (Inherit)
        //    {
        //        CachedClassHierarchyAssociateRoles = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>(base.GetAssociateRoles(Inherit), OOAdvantech.Collections.CollectionAccessType.ReadOnly);
        //        return CachedClassHierarchyAssociateRoles;
        //    }
        //    else
        //    {
        //        CachedAssociateRoles = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>(base.GetAssociateRoles(Inherit), OOAdvantech.Collections.CollectionAccessType.ReadOnly);
        //        return CachedAssociateRoles;
        //    }


        //    //return base.GetAssociateRoles(Inherit);
        //}

        /// <MetaDataID>{88D4E578-64EC-4F44-B335-EB36DD4DC57E}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.AssociationEnd> Roles
        {
            get
            {
                lock (RolesLock)
                {

                    using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        if (RolesLoaded)
                            return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>(_Roles.ToThreadSafeSet(), OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                        try
                        {
                            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Suppress))
                            {

                                Refer.GetRoles(this);

                                object[] Attributes = Refer.WrType.GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.AssociationClass), false);
                                if (Attributes.Length > 0)
                                {
                                    MetaDataRepository.AssociationClass associationClass = (MetaDataRepository.AssociationClass)Attributes[0];
                                    _LinkAssociation = Refer.GetAssociation(associationClass);
                                }
                                RolesLoaded = true;
                                return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>(_Roles, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                            }
                        }
                        catch (System.Exception error)
                        {
                            throw;
                        }

                    }


                }


                //lock (Type.LoadDotnetMetadataLock)
                //{
                //    OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                //    try
                //    {

                //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Suppress))
                //        {
                //            ///LoadAssociatonEnds();
                //            Refer.GetRoles(this);

                //            object[] Attributes = Refer.WrType.GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.AssociationClass), false);
                //            if (Attributes.Length > 0)
                //            {
                //                MetaDataRepository.AssociationClass associationClass = (MetaDataRepository.AssociationClass)Attributes[0];
                //                _LinkAssociation = Refer.GetAssociation(associationClass);
                //            }
                //            RolesLoaded = true;

                //            return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>(_Roles, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                //        }


                //    }
                //    catch (System.Exception error)
                //    {
                //        throw;
                //    }
                //    finally
                //    {
                //        ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                //    }
                //}
            }
        }

        /// <MetaDataID>{8A175078-BC63-4F95-9BC7-25C47C30A9B2}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Generalization> Generalizations
        {
            get
            {
                using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    try
                    {

                        if (!GeneralizationsLoaded)
                            LoadGeneralizations();
                        return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>(_Generalizations.ToThreadSafeSet(), OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }
                }
            }
        }


        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{CBC13F50-9B79-4E51-B173-49656D666E8B}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;
            }
        }

        /// <MetaDataID>{4A5DF53E-11FB-4DB5-A8C1-01C15E447F7A}</MetaDataID>
        private bool InErrorCheck = false;
        /// <MetaDataID>{20D1477F-A5FE-41B6-ACB8-D25276001830}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {

            //TODO Εάν η class δεν είναι abstract και δέν υπάρχει implementation field για κάποιο associtionend 
            //uθα πρέπει να παράγει error στο error check
            if (InErrorCheck)
                return false;
            bool hasError = base.ErrorCheck(ref errors);
            try
            {
                InErrorCheck = true;
                System.Reflection.ConstructorInfo constructorInfo = Refer.WrType.GetMetaData().GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, new System.Type[0]);
                if (constructorInfo == null && _Persistent)
                {
                    errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: In persistent class '" + FullName + "' we must declare default constructor.", FullName));
                    hasError = true;
                }
                if (_Persistent)
                {
                    System.Reflection.FieldInfo extensionPropertiesField = ObjectStateManagerLink.GetExtensionPropertiesField(Refer.WrType);
                    if (extensionPropertiesField == null)
                    {
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: You can't declare class '" + FullName + "' persistent without OOAdvantech.ObjectStateManagerLink member in class hierarchy", FullName));
                        hasError = true;
                    }
                }


                System.Reflection.ConstructorInfo[] constructors = Refer.WrType.GetMetaData().GetConstructors();

                if (LinkAssociation == null)
                {
                    object[] Attributes = Refer.WrType.GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.AssociationClass), false);
                    if (Attributes.Length > 0)
                    {
                        MetaDataRepository.AssociationClass associationClass = (MetaDataRepository.AssociationClass)Attributes[0];
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: System can't find the Assosciation " + associationClass.AssocciationName + " for Class '" + FullName + "'", FullName));
                        hasError = true;
                    }
                }
                else
                {

                    object[] Attributes = Refer.WrType.GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.AssociationClass), false);
                    if (Attributes.Length > 0)
                    {
                        MetaDataRepository.AssociationClass associationClass = (MetaDataRepository.AssociationClass)Attributes[0];
                        if (associationClass.AssocciationName != LinkAssociation.Name)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: System can't find the Assosciation " + associationClass.AssocciationName + " for Class '" + FullName + "'", FullName));
                            hasError = true;
                        }
                    }

                    GetLinkClassRoleFields();
                    if (!Abstract)
                    {
                        if (_LinkClassRoleAField == null)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: There isn't field definition for role A in Association class '" + FullName + "'.", FullName));
                            hasError = true;
                        }

                        if (_LinkClassRoleBField == null)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: There isn't field definition for role B in Association class '" + FullName + "'.", FullName));
                            hasError = true;
                        }

                        if (_LinkClassRoleBField == null || _LinkClassRoleAField == null)
                            return hasError;




                        System.Type RoleAType = LinkAssociation.RoleA.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                        System.Type RoleBType = LinkAssociation.RoleB.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;

                        if (RoleAType != _LinkClassRoleAField.FieldType)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: RoleA type mismatch at " + _LinkClassRoleAField.DeclaringType + "." + _LinkClassRoleAField.Name + ".", FullName));
                            hasError = true;
                        }

                        if (RoleBType != _LinkClassRoleBField.FieldType)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: RoleB type mismatch at " + _LinkClassRoleBField.DeclaringType + "." + _LinkClassRoleBField.Name + ".", FullName));
                            hasError = true;
                        }

                        if (hasError)
                        {
                            _LinkClassRoleAField = null;
                            _LinkClassRoleBField = null;
                        }
                    }
                }
                Collections.Generic.Set<MetaDataRepository.Classifier> supperClasifiers = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();
                if (ClassHierarchyLinkAssociation != null)
                {
                    foreach (Interface _interface in GetAllInterfaces())
                    {
                        supperClasifiers.Add(_interface);
                        supperClasifiers.AddRange(_interface.GetAllGeneralClasifiers());
                    }
                    supperClasifiers.AddRange(GetAllGeneralClasifiers());
                }
                foreach (MetaDataRepository.Classifier classifier in supperClasifiers)
                {
                    if (classifier.LinkAssociation != null && classifier.LinkAssociation != ClassHierarchyLinkAssociation)
                    {
                        hasError = true;
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: There are two association types in class hierarchy of '" + FullName +
                            "'. The '" + classifier.FullName + "' and the '" + ClassHierarchyLinkAssociation.LinkClass.FullName + "'.", FullName));
                    }
                }



                return hasError;
            }
            finally
            {
                InErrorCheck = false;
            }
        }



        /// <MetaDataID>{083A7936-365D-4B76-8BED-3E4F76684EB0}</MetaDataID>
        private System.Collections.Generic.List<object> ExtensionMetaObjects;



        /// <MetaDataID>{650EACF7-095B-4B22-9A55-CF0A89E9E0FA}</MetaDataID>
        public void AddExtensionMetaObject(object Value)
        {
            lock (ExtensionMetaObjectsLock)
            {
                if (ExtensionMetaObjects == null)
                    GetExtensionMetaObjects();
                if (!ExtensionMetaObjects.Contains(Value))
                    ExtensionMetaObjects.Add(Value);
            }
        }
        /// <MetaDataID>{ED3ECEEC-DFED-4BF6-A101-00A83B2F341F}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            lock (ExtensionMetaObjectsLock)
            {
                if (ExtensionMetaObjects == null)
                {
                    ExtensionMetaObjects = new System.Collections.Generic.List<object>();
                    ExtensionMetaObjects.Add(Refer.WrType);
                }
                return ExtensionMetaObjects.ToList();
            }
        }


        /// <MetaDataID>{8E9A2ED0-BCB5-49D3-8204-5206388EC8FB}</MetaDataID>
        private void LoadGeneralizations()
        {
            using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                try
                {
                    lock (GeneralizationLock)
                    {
                        if (!GeneralizationsLoaded)
                        {
                            _Generalizations = Refer.GetGeneralizations(this);
                            GeneralizationsLoaded = true;
                        }
                    }
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
        }

        /// <MetaDataID>{68C58AEC-455C-4F6A-880F-DD254E2D0A89}</MetaDataID>
        private void LoadRealizations()
        {
            lock (RealizationsLock)
            {
                using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if (!RealizationsLoaded)
                    {
                        Refer.GetRealizations(ref _Realizations, this);
                        RealizationsLoaded = true;
                    }
                }
            }
        }


        /// <MetaDataID>{92AF7612-DD1B-4A72-895E-E8A07FE5E194}</MetaDataID>
        object ClassInstance;
        /// <MetaDataID>{6F56E0B5-0311-424A-B24B-3A57139BB095}</MetaDataID>
        public object GetDefaultValue(Attribute attribute, MetaDataRepository.ValueTypePath valueTypePath)
        {
            try
            {
                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = GetFastFieldAccessor(attribute);
                if (fastFieldAccessor.MemberInfo != null)
                {
                    if (ClassInstance == null)
                        ClassInstance = AccessorBuilder.CreateInstance(Refer.WrType);
                    if (valueTypePath.Count == 0)
                        return fastFieldAccessor.GetValue(ClassInstance);
                    else
                    {
                        var valueTypeAttribute = GetAttribute(valueTypePath.ToArray()[valueTypePath.Count - 1]) as Attribute;
                        fastFieldAccessor = GetFastFieldAccessor(valueTypeAttribute);
                        object value = fastFieldAccessor.GetValue(ClassInstance);
                        if (value == null)
                            return null;
                        return GetDefaultValue(attribute, valueTypeAttribute.Type, valueTypePath, value, 1);
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        public object GetDefaultValue(Attribute attribute, MetaDataRepository.Classifier ownerClassifier, MetaDataRepository.ValueTypePath valueTypePath, object value, int pathIndex)
        {
            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = null;
            if (valueTypePath.Count - 1 - pathIndex == 0)
            {
                fastFieldAccessor = GetFastFieldAccessor(attribute);
                return fastFieldAccessor.GetValue(value);

            }
            else
            {
                var valueTypeAttribute = ownerClassifier.GetAttribute(valueTypePath.ToArray()[valueTypePath.Count - 1 - pathIndex]) as Attribute;
                fastFieldAccessor = GetFastFieldAccessor(valueTypeAttribute);
                value = fastFieldAccessor.GetValue(value);
                if (value == null)
                    return null;
                return GetDefaultValue(attribute, valueTypeAttribute.Owner, valueTypePath, value, pathIndex + 1);
            }

            return null;
        }
        /// <exclude>Excluded</exclude>
        string _FullName;
        /// <MetaDataID>{35b3088e-f26c-4101-8638-9b30bb59f4e6}</MetaDataID>
        public override string FullName
        {
            get
            {
                if (_FullName == null)
                    _FullName = base.FullName;
                return _FullName;
            }
        }

        /// <MetaDataID>{a170dcd5-e24f-4edc-afcf-fea43b944283}</MetaDataID>
        public OOAdvantech.AccessorBuilder.FieldPropertyAccessor GetFastFieldAccessor(Attribute attribute)
        {
            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor;
            if (AttributeFastFieldsAccessors.TryGetValue(attribute, out fastFieldAccessor))
                return fastFieldAccessor;

            if (attribute.AttributeRealizations.Count == 0)
            {
                //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
                //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
                if (attribute.Persistent == true && Persistent && attribute.FieldMember == null)
                    throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + attribute.PropertyMember.DeclaringType.FullName + "." + attribute.PropertyMember.Name + "] realization.");
                AttributeFastFieldsAccessors[attribute] = attribute.FastFieldAccessor;
                return attribute.FastFieldAccessor;
            }

            foreach (MetaDataRepository.Feature feature in Features)
            {
                AttributeRealization attributeRealization = feature as AttributeRealization;
                if (attributeRealization != null)
                {
                    if (attributeRealization.Specification == attribute && attributeRealization.FieldMember != null)
                    {
                        AttributeFastFieldsAccessors[attribute] = attributeRealization.FastFieldAccessor;
                        return attributeRealization.FastFieldAccessor;
                    }
                }
            }
            foreach (MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
            {
                if (classifier is MetaDataRepository.Interface)
                    continue;
                Class _class = classifier as Class;

                fastFieldAccessor = _class.GetFastFieldAccessor(attribute);
                if (fastFieldAccessor != null)
                {
                    AttributeFastFieldsAccessors[attribute] = fastFieldAccessor;
                    return fastFieldAccessor;
                }
            }
            //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
            //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
            if (attribute.Persistent == true && Persistent && (attribute.FastFieldAccessor == null || attribute.FastFieldAccessor.GetValue == null))
                throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + attribute.PropertyMember.DeclaringType.FullName + "." + attribute.PropertyMember.Name + "] realization.");

            return attribute.FastFieldAccessor;
        }




        ///// <MetaDataID>{8A7B6FAB-4395-4CC4-87FD-6CA749D0D0C4}</MetaDataID>
        //public System.Reflection.FieldInfo GetFieldMember(Attribute attribute)
        //{
        //    if (attribute.AttributeRealizations.Count == 0)
        //    {
        //        //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
        //        //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
        //        if (attribute.Persistent == true && Persistent && attribute.FieldMember == null)
        //            throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + attribute.PropertyMember.DeclaringType.FullName + "." + attribute.PropertyMember.Name + "] realization.");
        //        return attribute.FieldMember;
        //    }
        //    foreach (MetaDataRepository.Feature feature in Features)
        //    {
        //        AttributeRealization attributeRealization = feature as AttributeRealization;
        //        if (attributeRealization != null)
        //        {
        //            if (attributeRealization.Specification == attribute && attributeRealization.FieldMember != null)
        //                return attributeRealization.FieldMember;
        //        }
        //    }
        //    foreach (MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
        //    {
        //        if (classifier is MetaDataRepository.Interface)
        //            continue;
        //        Class _class = classifier as Class;

        //        System.Reflection.FieldInfo fieldInfo = _class.GetFieldMember(attribute);
        //        if (fieldInfo != null)
        //            return fieldInfo;
        //    }
        //    //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
        //    //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
        //    if (attribute.Persistent == true && Persistent && attribute.FieldMember == null)
        //        throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + attribute.PropertyMember.DeclaringType.FullName + "." + attribute.PropertyMember.Name + "] realization.");

        //    return attribute.FieldMember;
        //}

        /// <MetaDataID>{A49AB916-89A4-4FD2-9A23-1DCDF6FBBACF}</MetaDataID>
        System.Collections.Generic.Dictionary<AssociationEnd, System.Reflection.FieldInfo> AssociationEndFields = new System.Collections.Generic.Dictionary<AssociationEnd, System.Reflection.FieldInfo>();

        /// <MetaDataID>{88daf0cc-fdcd-4aa5-bfba-f974b05ca435}</MetaDataID>
        System.Collections.Generic.Dictionary<AssociationEnd, AccessorBuilder.FieldPropertyAccessor> AssociationEndFastFieldsAccessors = new System.Collections.Generic.Dictionary<AssociationEnd, AccessorBuilder.FieldPropertyAccessor>();
        /// <MetaDataID>{88c371fc-6acd-4d10-ae1d-3923b352b787}</MetaDataID>
        System.Collections.Generic.Dictionary<Attribute, AccessorBuilder.FieldPropertyAccessor> AttributeFastFieldsAccessors = new System.Collections.Generic.Dictionary<Attribute, AccessorBuilder.FieldPropertyAccessor>();


        /// <MetaDataID>{e28c1b49-3ba6-46fe-b068-264f93b26870}</MetaDataID>
        System.Collections.Generic.Dictionary<AssociationEnd, System.Reflection.MemberInfo> TransactionalMembers = new System.Collections.Generic.Dictionary<AssociationEnd, System.Reflection.MemberInfo>();

        ///// <MetaDataID>{ACFBC2A9-0977-440E-82A8-E91098C385BE}</MetaDataID>
        //public System.Reflection.FieldInfo GetFieldMember(AssociationEnd associationEnd)
        //{
        //    if (associationEnd.AssociationEndRealizations.Count == 0)
        //    {
        //        //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
        //        //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
        //        if (associationEnd.Persistent == true && Persistent && associationEnd.FieldMember == null)
        //            throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + associationEnd.PropertyMember.DeclaringType.FullName + "." + associationEnd.PropertyMember.Name + "] realization.");

        //        return associationEnd.FieldMember;
        //    }
        //    if (AssociationEndFields.ContainsKey(associationEnd))
        //        return AssociationEndFields[associationEnd];

        //    foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
        //    {
        //        if (associationEndRealization.Namespace == this)
        //        {
        //            if (associationEndRealization.FieldMember != null)
        //            {
        //                AssociationEndFields[associationEnd] = associationEndRealization.FieldMember;
        //                return associationEndRealization.FieldMember;
        //            }
        //        }
        //    }
        //    foreach (MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
        //    {
        //        if (classifier is MetaDataRepository.Interface)
        //            continue;
        //        Class _class = classifier as Class;

        //        System.Reflection.FieldInfo fieldInfo = _class.GetFieldMember(associationEnd);
        //        if (fieldInfo != null)
        //        {
        //            AssociationEndFields[associationEnd] = fieldInfo;
        //            return fieldInfo;
        //        }
        //    }
        //    //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
        //    //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
        //    if (associationEnd.Persistent == true && Persistent && associationEnd.FieldMember == null)
        //        throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + associationEnd.PropertyMember.DeclaringType.FullName + "." + associationEnd.PropertyMember.Name + "] realization.");

        //    AssociationEndFields[associationEnd] = associationEnd.FieldMember;
        //    return associationEnd.FieldMember;
        //}

        /// <MetaDataID>{f2820545-f904-4d32-b071-de39db723f74}</MetaDataID>
        System.Collections.Generic.Dictionary<AssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor> FastFieldAccessors = new System.Collections.Generic.Dictionary<AssociationEnd, AccessorBuilder.FieldPropertyAccessor>();
        /// <MetaDataID>{8d7dfba3-836e-49e0-89af-7fcb85ec7a25}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor GetFastFieldAccessor(AssociationEnd associationEnd)
        {
            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = null;
            if (!FeaturesLoaded)
            {
                int count = (int)Features.Count;
            }
            if (AssociationEndFastFieldsAccessors.TryGetValue(associationEnd, out fastFieldAccessor))
                return fastFieldAccessor;

            if (associationEnd.AssociationEndRealizations.Count == 0)
            {
                //if (associationEnd.Name == "TypeView")
                //{

                //}
                //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
                //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
                if (associationEnd.Persistent == true && Persistent && associationEnd.FieldMember == null)
                    throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + associationEnd.PropertyMember.DeclaringType.FullName + "." + associationEnd.PropertyMember.Name + "] realization.");
                AssociationEndFastFieldsAccessors[associationEnd] = associationEnd.FastFieldAccessor;
                return associationEnd.FastFieldAccessor;
            }

            foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
            {
                //if (associationEnd.Name == "TypeView")
                //{

                //}
                if (associationEndRealization.Namespace == this)
                {
                    if (associationEndRealization.FieldMember != null)
                    {
                        AssociationEndFastFieldsAccessors[associationEnd] = associationEndRealization.FastFieldAccessor;
                        return associationEndRealization.FastFieldAccessor;
                    }
                }
            }
            //if (associationEnd.Name == "TypeView")
            //{

            //}
            foreach (MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
            {
                if (classifier is MetaDataRepository.Interface)
                    continue;
                Class _class = classifier as Class;

                fastFieldAccessor = _class.GetFastFieldAccessor(associationEnd);
                if (fastFieldAccessor != null)
                {
                    AssociationEndFastFieldsAccessors[associationEnd] = fastFieldAccessor;
                    return fastFieldAccessor;
                }
            }
            //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
            //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
            if (associationEnd.Persistent == true && Persistent && associationEnd.FieldMember == null)
                throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + associationEnd.PropertyMember.DeclaringType.FullName + "." + associationEnd.PropertyMember.Name + "] realization.");

            AssociationEndFastFieldsAccessors[associationEnd] = associationEnd.FastFieldAccessor;
            return associationEnd.FastFieldAccessor;
        }


        object TransactionalMembersLock = new object();
        //TransactionalMembers
        /// <MetaDataID>{6adc00d7-f1dc-4038-9712-d74c5b35a1b3}</MetaDataID>
        public System.Reflection.MemberInfo GetTransactionalMember(AssociationEnd associationEnd)
        {
            try
            {
                if (!FeaturesLoaded)
                {
                    int count = (int)Features.Count;
                }

                System.Reflection.MemberInfo transactionalMember = null;

                lock (TransactionalMembersLock)
                {
                    if (TransactionalMembers.TryGetValue(associationEnd, out transactionalMember))
                        return transactionalMember;
                }

                if (associationEnd.AssociationEndRealizations.Count == 0)
                {
                    if (associationEnd.FieldMember != null && associationEnd.FieldMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
                    {
                        lock (TransactionalMembersLock)
                        {
                            TransactionalMembers[associationEnd] = associationEnd.FieldMember;
                            return associationEnd.FieldMember;
                        }
                    }
                    if (associationEnd.PropertyMember != null && associationEnd.PropertyMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
                    {
                        lock (TransactionalMembersLock)
                        {
                            TransactionalMembers[associationEnd] = associationEnd.PropertyMember;
                            return associationEnd.PropertyMember;
                        }
                    }

                }

                foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
                {
                    if (associationEndRealization.Namespace == this)
                    {

                        if (associationEndRealization.FieldMember != null && associationEndRealization.FieldMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
                        {
                            lock (TransactionalMembersLock)
                            {
                                TransactionalMembers[associationEnd] = associationEndRealization.FieldMember;
                                return associationEndRealization.FieldMember;
                            }
                        }
                        if (associationEndRealization.PropertyMember != null && associationEndRealization.PropertyMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
                        {
                            lock (TransactionalMembersLock)
                            {
                                TransactionalMembers[associationEnd] = associationEndRealization.PropertyMember;
                                return associationEndRealization.PropertyMember;
                            }
                        }
                    }
                }
                foreach (MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
                {
                    if (classifier is MetaDataRepository.Interface)
                        continue;
                    Class _class = classifier as Class;

                    transactionalMember = _class.GetTransactionalMember(associationEnd);
                    if (transactionalMember != null)
                    {
                        lock (TransactionalMembersLock)
                        {
                            TransactionalMembers[associationEnd] = transactionalMember;
                            return transactionalMember;
                        }
                    }
                }

                if (associationEnd.FieldMember != null && associationEnd.FieldMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
                {
                    lock (TransactionalMembersLock)
                    {
                        TransactionalMembers[associationEnd] = associationEnd.FieldMember;
                        return associationEnd.FieldMember;
                    }
                }
                if (associationEnd.PropertyMember != null && associationEnd.PropertyMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
                {
                    lock (TransactionalMembersLock)
                    {
                        TransactionalMembers[associationEnd] = associationEnd.PropertyMember;
                        return associationEnd.PropertyMember;
                    }
                }
                lock (TransactionalMembersLock)
                {
                    TransactionalMembers[associationEnd] = null;
                    return null;
                }
            }
            catch (System.Exception error)
            {
                throw error;

            }
        }




        //public System.Reflection.MemberInfo GetTransactionlMember(AssociationEnd associationEnd)
        //{
        //    if (associationEnd.AssociationEndRealizations.Count == 0)
        //    {


        //        if (associationEnd.PropertyMember != null &&
        //            associationEnd.PropertyMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
        //        {
        //            return associationEnd.PropertyMember;
        //        }
        //        if (associationEnd.FieldMember != null &&
        //            associationEnd.FieldMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
        //        {
        //            return associationEnd.FieldMember;
        //        }
        //        return null;
        //    }
        //    if (AssociationEndTransactionlMember.ContainsKey(associationEnd))
        //        return AssociationEndTransactionlMember[associationEnd];

        //    foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
        //    {
        //        if (associationEndRealization.Namespace == this)
        //        {

        //            if (associationEndRealization.PropertyMember != null &&
        //                associationEndRealization.PropertyMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
        //            {
        //                AssociationEndTransactionlMember[associationEnd] = associationEndRealization.PropertyMember;
        //                return associationEndRealization.PropertyMember;
        //            }
        //            if (associationEndRealization.FieldMember != null &&
        //                associationEndRealization.FieldMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
        //            {
        //                AssociationEndTransactionlMember[associationEnd] = associationEndRealization.FieldMember;
        //                return associationEndRealization.FieldMember;
        //            }
        //        }
        //    }
        //    foreach (MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
        //    {

        //        foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
        //        {
        //            if (associationEndRealization.Namespace == classifier)
        //            {

        //                if (associationEndRealization.PropertyMember != null &&
        //                    associationEndRealization.PropertyMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
        //                {
        //                    AssociationEndTransactionlMember[associationEnd] = associationEnd.PropertyMember;
        //                    return associationEndRealization.PropertyMember;
        //                }
        //                if (associationEndRealization.FieldMember != null &&
        //                    associationEndRealization.FieldMember.GetCustomAttributes(typeof(TransactionalMemberAttribute), true).Length > 0)
        //                {
        //                    AssociationEndTransactionlMember[associationEnd] = associationEndRealization.FieldMember;
        //                    return associationEndRealization.FieldMember;
        //                }
        //            }
        //        }
        //    }
        //    AssociationEndTransactionlMember[associationEnd] = null;
        //    return null;

        //}



        /// <MetaDataID>{3cfe115a-077c-46ae-8d5f-1ec791cdefed}</MetaDataID>
        public AssociationEndRealization GetAssociationEndRealization(AssociationEnd associationEnd)
        {
            if (!FeaturesLoaded)
            {
                int count = (int)Features.Count;
            }
            if (associationEnd.AssociationEndRealizations.Count == 0)
            {
                return null;
            }

            foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
            {
                if (associationEndRealization.Namespace == this)
                {
                    if (associationEndRealization.FieldMember != null)
                        return associationEndRealization;
                }
            }
            foreach (MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
            {
                if (classifier is MetaDataRepository.Interface)
                    continue;
                Class _class = classifier as Class;

                AssociationEndRealization associationEndRealization = _class.GetAssociationEndRealization(associationEnd);
                if (associationEndRealization != null)
                    return associationEndRealization;
            }
            return null;
        }





        /// <MetaDataID>{45ae8496-8737-4394-a7ec-d4792132c651}</MetaDataID>
        System.Collections.Generic.Dictionary<AssociationEnd, System.Reflection.MemberInfo> AssociationEndTransactionlMember = new System.Collections.Generic.Dictionary<AssociationEnd, System.Reflection.MemberInfo>();

        /// <MetaDataID>{FF904D5B-BCA4-44A9-8970-C080FFBD916B}</MetaDataID>
        protected internal override void SetNamespace(MetaDataRepository.Namespace mNamespace)
        {

            lock (_Namespace)
            {
                _FullName = null;
                _Namespace.Value = mNamespace;
                _FullName = null;
            }
        }
        public override MetaDataRepository.Namespace Namespace
        {
            get
            {
                lock (_Namespace)
                {
                    return _Namespace;
                }
            }
        }

        /// <summary>This method retrieves all classes nested within the specified class and all of its nested classes.
        /// For example:  If Class A has 2 nested classes, NClass1 and NClass2, and NClass1 has a nested class, NestedCls, applying the GetAllNestedClasses method to Class A returns all 3 nested classes, NClass1, NClass2, and NestedCls, not just the first-level nested classes.
        /// To retrieve only the first-level nested classes for the specified class, use GetNestedClasses. </summary>
        /// <MetaDataID>{577CC8EC-A25B-4511-A484-DD957A30084D}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Classifier> GetAllNestedClasses()
        {


            Collections.Generic.Set<MetaDataRepository.Classifier> allNestedClasses = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();
            foreach (Class CurrClass in NestedClasses)
            {
                allNestedClasses.Add(CurrClass);
                allNestedClasses.AddRange(CurrClass.GetAllNestedClasses());
            }
            return allNestedClasses;
        }









        /// <MetaDataID>{66DA50F2-0634-4982-93C4-8D4DF43B090C}</MetaDataID>
        private bool RolesLoaded = false;
        /// <MetaDataID>{4A805E48-D77C-4AC1-B105-3E9CB8FB7B55}</MetaDataID>
        private bool RealizationsLoaded = false;
        /// <MetaDataID>{4F59E6BD-A5A0-4620-9AFC-37D08E3F41F8}</MetaDataID>
        private bool GeneralizationsLoaded = false;
        /// <MetaDataID>{151623F6-6B5F-4649-80D5-77924F7D3FF7}</MetaDataID>
        private bool FeaturesLoaded = false;


        /// <MetaDataID>{C9001141-8F83-421D-AEB7-D99093B9A931}</MetaDataID>
        internal void AddAssociationEnd(AssociationEnd theAssociationEnd)
        {

            _Roles.Add(theAssociationEnd);



            lock (ClassHierarchyLock)
            {
                ClassHierarchyAssociateRoles = null;
                ClassHierarchyRoles = null;
            }

            foreach (MetaDataRepository.Classifier classifier in GetAllSpecializeClasifiers())
                classifier.RefreshClassHierarchyCollections();


            //Task.Factory.StartNew(() => {
            //    foreach (MetaDataRepository.Classifier classifier in GetAllSpecializeClasifiers())
            //        classifier.RefreshClassHierarchyCollections();
            //});


        }

        /// <MetaDataID>{f44aed23-1ee7-468f-84c5-30a49a2b549d}</MetaDataID>
        string ID = System.Guid.NewGuid().ToString();

        /// <MetaDataID>{B240F117-197E-4B03-B433-DF8DD954266E}</MetaDataID>
        internal Class(Type theType)
        {
            try
            {

                //if (theType.WrType.FullName == "OOAdvantech.Remoting.RestApi.ServerSessionPart")
                //{
                //}
                //if(theType.WrType.FullName.IndexOf("System.Func")==0)
                //{

                //}
                //if (theType.WrType.FullName.IndexOf("System.Func`") == 0)
                //{

                //}
                if (!theType.WrType.GetMetaData().IsClass)
                    throw new System.Exception("the type '" + theType.WrType.FullName + "' isn't class");

                _ImplementationUnit.Value = Assembly.GetComponent(theType.WrType.GetMetaData().Assembly);

                RolesLoaded = false;
                _Persistent = false;
                _Abstract = theType.WrType.GetMetaData().IsAbstract;
                _Name = theType.Name;
                //if (theType.WrType.FullName == "OOAdvantech.RDBMSMetaDataRepository.Interface")
                //{

                //}
                Refer = theType;
                Visibility = Refer.Visibility;

                #region produce the object _Identity

                OOAdvantech.MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = Assembly.GetBackwardCompatibilityID(theType.WrType.GetMetaData().Assembly);

                object[] Attributes = Refer.WrType.GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
                if (Attributes.Length > 0)
                {
                    //There is  BackwardCompatibilityID declaration
                    string identityAsString = (Attributes[0] as MetaDataRepository.BackwardCompatibilityID).ToString();
                    if (identityAsString.Length > 0)
                    {
                        if (identityAsString[0] == '+')
                        {
                            //Extend namespace identity
                            identityAsString = identityAsString.Substring(1);
                            if (Namespace != null && Namespace.Identity.ToString().Trim().Length > 0)
                                identityAsString = Namespace.Identity.ToString().Trim() + "." + identityAsString;
                            else if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                                identityAsString = backwardCompatibilityID.ToString() + "." + identityAsString;


                        }
                        else if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                            identityAsString = backwardCompatibilityID.ToString() + "." + identityAsString;
                        _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(identityAsString);
                    }
                    else if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                        _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID.ToString() + "." + Refer.WrType.ToString());
                    else
                        _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.ToString());
                }
                else if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
                    _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID.ToString() + "." + Refer.WrType.ToString());
                else
                {
                    _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.ToString());
                    if (Refer.WrType.GetMetaData().IsGenericType && !Refer.WrType.GetMetaData().IsGenericTypeDefinition)
                    {
                        _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.ToString() + Refer.WrType.GetHashCode().ToString());

                        string ddf = Refer.WrType.AssemblyQualifiedName;
                        //foreach (System.Type argType in Refer.WrType.GetMetaData().GetGenericArguments())
                        //{
                        //    if (argType.IsGenericParameter)
                        //    {
                        //        _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.ToString() + Refer.WrType.GetHashCode().ToString());
                        //        break;
                        //    }
                        //}
                    }
                    else if (Refer.WrType.GetMetaData().ContainsGenericParameters)
                    {
                        _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.ToString() + Refer.WrType.GetHashCode().ToString());
                        string ddf = Refer.WrType.AssemblyQualifiedName;
                    }


                }
                #endregion

                if (/*!Refer.WrType.IsGenericType &&*/ Refer.WrType.FullName != null)
                    MetaObjectMapper.AddMetaObject(this, Refer.WrType.ToString());
#if !DeviceDotNet
                // LoadGeneralizations();
#endif

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Suppress))
                {
                    #region Generic type initialization
                    Refer.GenericTypeInit(this, ref _OwnedTemplateSignature, ref _TemplateBinding);
                    //if (Refer.IsGenericTypeDefinition)
                    //{
                    //    _OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);
                    //    _OwnedTemplateSignature.Template = this;
                    //    foreach (MetaDataRepository.TemplateParameter templateParameter in Refer.TemplateParameters)
                    //        _OwnedTemplateSignature.AddOwnedParameter(templateParameter);
                    //    if (Refer.TemplateParameters.Count == 0)
                    //        throw new System.Exception("There is generic type '" + Refer.WrType.FullName + "' without generic parameters");
                    //    if (_OwnedTemplateSignature.OwnedParameters.Count == 0)
                    //        throw new System.Exception("There is generic type '" + Refer.WrType.FullName + "' without generic parameters.");

                    //}
                    //else if (Refer.WrType.IsGenericType)
                    //{
                    //    System.Type genericTypeDefinition = Refer.WrType.Assembly.GetType(Refer.WrType.Namespace + "." + Refer.WrType.Name);
                    //    MetaDataRepository.Classifier genericClassifier = MetaObjectMapper.FindMetaObjectFor(genericTypeDefinition) as MetaDataRepository.Classifier;
                    //    if (genericClassifier == null)
                    //        genericClassifier = Type.CreateClassifierObject(genericTypeDefinition);

                    //    Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement>();
                    //    foreach (System.Type type in Refer.WrType.GetGenericArguments())
                    //    {
                    //        MetaDataRepository.IParameterableElement parameterableElement = MetaObjectMapper.FindMetaObjectFor(type) as MetaDataRepository.IParameterableElement;
                    //        if (parameterableElement == null)
                    //        {
                    //            if (type.IsGenericParameter)
                    //            {
                    //                parameterableElement = new MetaDataRepository.TemplateParameter(type.Name);
                    //                MetaObjectMapper.AddTypeMap(type, parameterableElement as MetaDataRepository.TemplateParameter);
                    //            }
                    //            else
                    //                parameterableElement = Type.CreateClassifierObject(type);
                    //        }
                    //        parameterSubstitutions.Add(parameterableElement);
                    //    }
                    //    _TemplateBinding = new OOAdvantech.MetaDataRepository.TemplateBinding(genericClassifier, parameterSubstitutions);
                    //}
                    #endregion

                    stateTransition.Consistent = true;
                }


                #region Gets persistency meta data
                object[] ObjectCustomAttributes = Refer.WrType.GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.Persistent), true);
                if (ObjectCustomAttributes.Length > 0)
                {

                    MetaDataRepository.Persistent mPersistent = ObjectCustomAttributes[0] as MetaDataRepository.Persistent;

                    if (mPersistent.ExtMetaData != null)
                    {
                        System.Xml.Linq.XDocument ExtMetaData = new System.Xml.Linq.XDocument();
                        try
                        {
                            if (!string.IsNullOrEmpty(mPersistent.ExtMetaData))
                                ExtMetaData = System.Xml.Linq.XDocument.Parse(mPersistent.ExtMetaData);
                        }
                        catch (System.Exception Error)
                        {
                        }
                        if (ExtMetaData.Elements().Count() > 0)
                        {
                            foreach (var currNode in ExtMetaData.Elements().First().Elements())
                            {
                                if (currNode.Value.Length > 0)
                                    PutPropertyValue(ExtMetaData.Elements().First().Name.ToString(), currNode.Name.ToString(), currNode.Value);
                            }
                        }
                    }
                    if (mPersistent.PersistencyType == MetaDataRepository.PersistencyType.historyClass)
                    {
                        PutPropertyValue("Persistence", "HistoryClass", true);
                        PutPropertyValue("Persistence", "NumberOfObject", mPersistent.NumberOfObject);
                    }
                    else
                        PutPropertyValue("Persistence", "HistoryClass", false);

                    _Persistent = true;
                }
                #endregion
#if !DeviceDotNet

                //LoadRealizations();
#endif


                if (Refer.IsNestedType)

                    return;
                if (!string.IsNullOrEmpty(theType.WrType.Namespace))
                {
                    Namespace mNamespace = Type.GetNameSpace(theType.WrType.Namespace);

                    mNamespace.AddOwnedElement(this);
                    SetNamespace(mNamespace);
                }
            }
            finally
            {
                var classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(theType.WrType);
                MetaObjectMapper.AddTypeMap(theType.WrType, this);

            }

            System.Diagnostics.Debug.WriteLine(Name);

        }


        ///// <MetaDataID>{58C555BA-4831-426C-81ED-94B4A9B98566}</MetaDataID>
        //void LoadAttributeRealization()
        //{


        //    #region Search for attribute realization in parent interfaces
        //    //TODO Στο .net μπορεί να υπάρχει property με το ιδιο όνομα σε ένα ή περισσότερα interface 
        //    //στην ιεραρχία άρα  η property της class μπορεί υλοποιεί πάνω από ένα interafce property. 
        //    //Aυτή την κατάσταση την αγνοεί το DotnetMetaDataRepository

        //    foreach (Interface _interface in GetAllInterfaces())
        //    {

        //        InterfaceMapping interfaceMapping = Refer.GetInterfaceMap(_interface.Refer.WrType);
        //        foreach (Attribute attribute in _interface.GetAttributes(false))
        //        {
        //            if (attribute.Accessors.Length == 0)
        //                continue;
        //            System.Reflection.MethodBase accessor = attribute.Accessors[0];
        //            System.Reflection.MethodBase accessorImplementation = null;
        //            for (int i = 0; i < interfaceMapping.InterfaceMethods.Length; i++)
        //            {
        //                if (interfaceMapping.InterfaceMethods[i] == accessor)
        //                {
        //                    accessorImplementation = interfaceMapping.TargetMethods[i];
        //                    break;
        //                }
        //            }
        //            if (accessorImplementation == null)
        //                continue;

        //            foreach (Attribute implementationAttribute in GetAttributes(false))
        //            {
        //                if (implementationAttribute.PropertyMember == null)
        //                    continue;
        //                if (implementationAttribute.Accessors.Length > 0)
        //                {
        //                    //if (Refer.GetOperationForMethod(implementationAttribute.Accessors[0]) == accessor)
        //                    if (implementationAttribute.Accessors[0] == accessorImplementation)
        //                    {
        //                        _Features.Remove(implementationAttribute);
        //                        AttributeRealization attributeRealization = new AttributeRealization(implementationAttribute.PropertyMember, attribute, this);
        //                        // AttributeRealizations.Add(attributeRealization);
        //                        _Features.Add(attributeRealization);
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        if (implementationAttribute.Accessors.Length > 1)
        //                        {
        //                            //if (Refer.GetOperationForMethod(implementationAttribute.Accessors[1]) == accessor)
        //                            if (implementationAttribute.Accessors[1] == accessorImplementation)
        //                            {
        //                                _Features.Remove(implementationAttribute);
        //                                AttributeRealization attributeRealization = new AttributeRealization(implementationAttribute.PropertyMember, attribute, this);
        //                                // AttributeRealizations.Add(attributeRealization);
        //                                _Features.Add(attributeRealization);
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    foreach (MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
        //    {
        //        if (!(classifier is MetaDataRepository.Class))
        //            continue;
        //        Class _class = classifier as Class;

        //        foreach (Attribute attribute in _class.GetAttributes(false))
        //        {
        //            if (attribute.PropertyMember != null)
        //            {
        //                System.Reflection.MethodInfo[] methodInfos = attribute.Accessors;
        //                if (methodInfos.Length > 0)
        //                {
        //                    System.Reflection.MethodBase accessor = attribute.Accessors[0];
        //                    foreach (Attribute implementationAttribute in GetAttributes(false))
        //                    {
        //                        if (implementationAttribute.PropertyMember == null)
        //                            continue;
        //                        if (implementationAttribute.Accessors.Length > 0)
        //                        {
        //                            if (implementationAttribute.Accessors[0].GetBaseDefinition() == accessor)
        //                            {
        //                                _Features.Remove(implementationAttribute);
        //                                _Features.Add(new AttributeRealization(implementationAttribute.PropertyMember, attribute, this));
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                if (implementationAttribute.Accessors.Length > 1)
        //                                {
        //                                    if (implementationAttribute.Accessors[1].GetBaseDefinition() == accessor)
        //                                    {
        //                                        _Features.Remove(implementationAttribute);
        //                                        _Features.Add(new AttributeRealization(implementationAttribute.PropertyMember, attribute, this));
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }


        //}
        ///// <MetaDataID>{3F452752-4421-4DF9-B72F-A9AD52FFE00D}</MetaDataID>
        //void LoadAssociationEndRealization()
        //{

        //    foreach (Interface _interface in GetAllInterfaces())
        //    {
        //        foreach (AssociationEnd associationEnd in _interface.GetAssociateRoles(false))
        //        {
        //            if (associationEnd.PropertyMember != null && associationEnd.Accessors.Length > 0)
        //            {
        //                System.Reflection.MethodInfo accessor = associationEnd.Accessors[0];
        //                //System.Reflection.InterfaceMapping interfaceMapping=Refer.WrType.GetInterfaceMap(_interface.Refer.WrType);
        //                //System.Reflection.MethodBase accessorImplementation=null;
        //                //for(int i=0;i<interfaceMapping.InterfaceMethods.Length;i++) 
        //                //{
        //                //    if(interfaceMapping.InterfaceMethods[i]==accessor)
        //                //    {
        //                //        //TODO Στο .net μπορεί να υπάρχει property με το ιδιο όνομα σε ένα ή περισσότερα interface 
        //                //        //στην ιεραρχία άρα  η property της class μπορεί υλοποιεί πάνω από ένα interafce property. 
        //                //        //Aυτή την κατάσταση την αγνοεί το DotnetMetaDataRepository
        //                //        accessorImplementation=interfaceMapping.TargetMethods[i];
        //                //        break;
        //                //    }
        //                //}
        //                //if(accessorImplementation!=null)
        //                //{
        //                foreach (Attribute implementationAttribute in GetAttributes(false))
        //                {
        //                    if (implementationAttribute.PropertyMember == null)
        //                        continue;
        //                    if (implementationAttribute.Accessors.Length > 0)
        //                    {
        //                        if (Refer.GetOperationForMethod(implementationAttribute.Accessors[0]) == accessor)
        //                        //if (implementationAttribute.Accessors[0] == accessorImplementation)
        //                        {
        //                            _Features.Remove(implementationAttribute);
        //                            AssociationEndRealization associationEndRealization = new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, this);
        //                            //AssociationEndRealizations.Add(associationEndRealization); 
        //                            _Features.Add(associationEndRealization);
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            if (implementationAttribute.Accessors.Length > 1)
        //                            {
        //                                if (Refer.GetOperationForMethod(implementationAttribute.Accessors[1]) == accessor)
        //                                //if (implementationAttribute.Accessors[1] == accessorImplementation)
        //                                {
        //                                    _Features.Remove(implementationAttribute);
        //                                    AssociationEndRealization associationEndRealization = new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, this);
        //                                    // AssociationEndRealizations.Add(associationEndRealization);
        //                                    _Features.Add(associationEndRealization);
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    foreach (MetaDataRepository.Classifier classifier in GetAllGeneralClasifiers())
        //    {
        //        if (!(classifier is MetaDataRepository.Class))
        //            continue;
        //        Class _class = classifier as Class;
        //        foreach (AssociationEnd associationEnd in _class.GetAssociateRoles(false))
        //        {
        //            if (associationEnd.PropertyMember != null && associationEnd.Accessors.Length > 0)
        //            {

        //                System.Reflection.MethodBase accessor = associationEnd.Accessors[0];
        //                foreach (Attribute implementationAttribute in GetAttributes(false))
        //                {
        //                    if (implementationAttribute.PropertyMember == null)
        //                        continue;
        //                    if (implementationAttribute.Accessors.Length > 0)
        //                    {
        //                        if (implementationAttribute.Accessors[0].GetBaseDefinition() == accessor)
        //                        {
        //                            _Features.Remove(implementationAttribute);
        //                            _Features.Add(new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, this));
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            if (implementationAttribute.Accessors.Length > 1)
        //                            {
        //                                if (implementationAttribute.Accessors[1].GetBaseDefinition() == accessor)
        //                                {
        //                                    _Features.Remove(implementationAttribute);
        //                                    _Features.Add(new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, this));
        //                                    break;
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //}



    }
}
