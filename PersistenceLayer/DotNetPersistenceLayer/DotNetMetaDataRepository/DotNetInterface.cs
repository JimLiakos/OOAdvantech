using System.Linq;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{290365C4-9BAE-49E3-8653-AD4A5AB4ED9B}</MetaDataID>
    public class Interface : MetaDataRepository.Interface
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_FullName))
            {
                if (value == null)
                    _FullName = default(string);
                else
                    _FullName = (string)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LinkClassRoleAProperty))
            {
                if (value == null)
                    _LinkClassRoleAProperty = default(System.Reflection.PropertyInfo);
                else
                    _LinkClassRoleAProperty = (System.Reflection.PropertyInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LinkClassRoleBProperty))
            {
                if (value == null)
                    _LinkClassRoleBProperty = default(System.Reflection.PropertyInfo);
                else
                    _LinkClassRoleBProperty = (System.Reflection.PropertyInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
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
                }
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
            if (member.Name == nameof(Refer))
            {
                if (value == null)
                    Refer = default(OOAdvantech.DotNetMetaDataRepository.Type);
                else
                    Refer = (OOAdvantech.DotNetMetaDataRepository.Type)value;
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
            if (member.Name == nameof(GeneralizationsLoaded))
            {
                if (value == null)
                    GeneralizationsLoaded = default(bool);
                else
                    GeneralizationsLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_FullName))
                return _FullName;

            if (member.Name == nameof(_LinkClassRoleAProperty))
                return _LinkClassRoleAProperty;

            if (member.Name == nameof(_LinkClassRoleBProperty))
                return _LinkClassRoleBProperty;

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(ExtensionMetaObjects))
                return GetExtensionMetaObjects();

            if (member.Name == nameof(RolesLoaded))
                return RolesLoaded;

            if (member.Name == nameof(Refer))
                return Refer;

            if (member.Name == nameof(FeaturesLoaded))
                return FeaturesLoaded;

            if (member.Name == nameof(GeneralizationsLoaded))
                return GeneralizationsLoaded;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        string _FullName;
        /// <MetaDataID>{fa70ef6d-de75-400b-a10a-f74ac70fcf80}</MetaDataID>
        public override string FullName
        {
            get
            {
                if (_FullName == null)
                    _FullName = base.FullName;
                return _FullName;
            }
        }

        /// <MetaDataID>{6dcc2f58-bf9b-4b88-8963-3f51d78c5c69}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{8F1C49FE-D8DD-4204-AE32-15C6AE049E39}</MetaDataID>
        internal void LoadHierarchyRealizations()
        {
            lock (SpecializationsLock)
            {
                foreach (MetaDataRepository.Generalization specialization in Specializations)
                    (specialization.Child as Interface).LoadHierarchyRealizations();

                foreach (MetaDataRepository.Realization realization in Realizations)
                    (realization.Implementor as Class).LoadHierarchyRealizations();
            }

        }
        /// <MetaDataID>{8d1b809f-c878-41f4-b771-c2d956e3c419}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Association LinkAssociation
        {
            get
            {
                if (!RolesLoaded)
                {
                    long loadRoles = Roles.Count;
                }
                return base.LinkAssociation;
            }
            set
            {
                base.LinkAssociation = value;
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7C5A4CBF-B32C-450B-BD95-695F5C00989F}</MetaDataID>
        private System.Reflection.PropertyInfo _LinkClassRoleAProperty;
        /// <MetaDataID>{5D7BD798-C1DD-4CDB-A13C-A12BCB67F3B8}</MetaDataID>
        public System.Reflection.PropertyInfo LinkClassRoleAProperty
        {
            get
            {
                if (_LinkClassRoleAProperty == null)
                    GetLinkClassRoleProperties();

                return _LinkClassRoleAProperty;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{705342FA-A1A9-4D98-ADA3-CBFD12BDFA5A}</MetaDataID>
        private System.Reflection.PropertyInfo _LinkClassRoleBProperty;
        /// <MetaDataID>{80E3524B-3354-499E-8C9D-B17ADAF72A2E}</MetaDataID>
        public System.Reflection.PropertyInfo LinkClassRoleBProperty
        {
            get
            {
                if (_LinkClassRoleBProperty == null)
                    GetLinkClassRoleProperties();



                return _LinkClassRoleBProperty;
            }
        }
        /// <MetaDataID>{8199BA83-680A-43A8-975D-4B90FE24FD24}</MetaDataID>
        protected Interface()
        {
        }

        /// <MetaDataID>{2165EF0A-5E41-48A4-B8BC-2216F8E8AAF7}</MetaDataID>
        void GetLinkClassRoleProperties()
        {
            if (ClassHierarchyLinkAssociation == null)
                throw new System.Exception("The interface " + FullName + "  isn't link class.");

            var features = Features;
            if (ClassHierarchyLinkAssociation != null)
            {
                //lock (InterfaceHierarcyObjectLock)
                {
                    if (_LinkClassRoleBProperty == null && _LinkClassRoleAProperty == null)
                    {
                        foreach (MetaDataRepository.Feature feature in features)
                        {
                            System.Reflection.PropertyInfo propertyInfo = null;
                            if (feature is Attribute && (feature as Attribute).wrMember.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), false).Length > 0)
                                propertyInfo = (feature as Attribute).PropertyMember;

                            if (propertyInfo != null)
                            {
                                MetaDataRepository.AssociationClassRole AssociationClassRole = propertyInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), true)[0] as MetaDataRepository.AssociationClassRole;
                                if (AssociationClassRole.IsRoleA)
                                    _LinkClassRoleAProperty = propertyInfo;
                                else
                                    _LinkClassRoleBProperty = propertyInfo;
                            }
                        }
                        if (_LinkClassRoleBProperty == null && _LinkClassRoleAProperty == null)
                        {
                            foreach (Interface _interface in GetGeneralClasifiers())
                            {
                                _interface.GetLinkClassRoleProperties();
                                _LinkClassRoleAProperty = _interface.LinkClassRoleAProperty;
                                _LinkClassRoleBProperty = _interface.LinkClassRoleBProperty;
                            }
                        }
                    }
                }
            }
        }


        /// <MetaDataID>{3EDDD1FF-9062-4477-A809-40B382E847F8}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private bool InErrorCheck = false;

        /// <MetaDataID>{6A3F04A2-6A10-48ED-BEC1-2D5E3FD364D3}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {
            if (InErrorCheck)
                return false;

            bool hasError = base.ErrorCheck(ref errors);
            try
            {

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
                    GetLinkClassRoleProperties();
                    if (_LinkClassRoleAProperty == null)
                    {
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: There isn't field definition for role A in Association class '" + FullName + "'.", FullName));
                        hasError = true;
                    }

                    if (_LinkClassRoleBProperty == null)
                    {
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: There isn't field definition for role B in Association class '" + FullName + "'.", FullName));
                        hasError = true;
                    }
                    if (_LinkClassRoleBProperty == null || _LinkClassRoleAProperty == null)
                        return hasError;




                    System.Type RoleAType = LinkAssociation.RoleA.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                    System.Type RoleBType = LinkAssociation.RoleB.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;

                    if (RoleAType != _LinkClassRoleAProperty.PropertyType)
                    {
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: RoleA type mismatch at " + _LinkClassRoleAProperty.DeclaringType + "." + _LinkClassRoleAProperty.Name + ".", FullName));
                        hasError = true;
                    }

                    if (RoleBType != _LinkClassRoleBProperty.PropertyType)
                    {
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: RoleB type mismatch at " + _LinkClassRoleBProperty.DeclaringType + "." + _LinkClassRoleBProperty.Name + ".", FullName));
                        hasError = true;
                    }



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
                }
                Collections.Generic.Set<MetaDataRepository.Classifier> supperClasifiers = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();
                if (ClassHierarchyLinkAssociation != null)
                    supperClasifiers.AddRange(GetAllGeneralClasifiers());

                foreach (MetaDataRepository.Classifier classifier in supperClasifiers)
                {
                    if (classifier.LinkAssociation != null && classifier.LinkAssociation != ClassHierarchyLinkAssociation)
                    {
                        hasError = true;
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: There are two association types in class hierarchy of '" + FullName +
                            "'. The '" + classifier.FullName + "' and the '" + ClassHierarchyLinkAssociation.LinkClass.FullName + "'.", FullName));
                    }
                }
            }
            finally
            {
                InErrorCheck = false;
            }



            return hasError;


        }


        /// <MetaDataID>{180575FC-B95A-41E6-82F2-7382E38AE5A7}</MetaDataID>
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
        /// <MetaDataID>{F2922A6E-3776-4505-B8C1-A583AA35F8A8}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Generalization> Generalizations
        {
            get
            {
                using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    lock (GeneralizationLock)
                    {
                        if (!GeneralizationsLoaded)
                            LoadGeneralizations();
                    }
                    return new Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>(_Generalizations.ToThreadSafeSet(), Collections.CollectionAccessType.ReadOnly);
                }

            }
        }
        /// <MetaDataID>{15554F42-E469-46FA-9D99-68C91B04D8E1}</MetaDataID>
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


            }
        }
        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{60A6731B-519E-4399-8EFF-55BF967480D2}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;
            }
        }


        /// <MetaDataID>{9B3EB1E2-81FD-4B9E-A4E1-9EC5E61FE90E}</MetaDataID>
        private System.Collections.Generic.List<object> ExtensionMetaObjects;
        /// <MetaDataID>{98E3F6A4-DE17-40E7-9BFD-43CF0C8AA9A6}</MetaDataID>
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

        /// <MetaDataID>{A8F47F8A-6F36-4549-BCE0-7B1BFEBAF714}</MetaDataID>
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



            //Task.Factory.StartNew(() =>
            //{

            //    foreach (MetaDataRepository.Classifier classifier in GetAllSpecializeClasifiers())
            //        classifier.RefreshClassHierarchyCollections();
            //});


            //OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            //try
            //{
            //    _Roles.Add(theAssociationEnd);

            //    ClassHierarchyAssociateRoles = null;
            //    ClassHierarchyRoles = null;
            //    foreach (MetaDataRepository.Classifier classifier in GetAllSpecializeClasifiers())
            //    {
            //        classifier.RefreshClassHierarchyCollections();
            //    }

            //}
            //finally
            //{
            //    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            //}
        }

        /// <MetaDataID>{0497B837-A958-47D3-9C60-7360E6E7E05E}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private bool RolesLoaded;





        //private readonly object InterfaceHierarcyObjectLock = new object();

        //private readonly object InterfaceObjectLock = new object();


        /// <MetaDataID>{AE671606-A6D7-448D-B2E6-AC8717F9C601}</MetaDataID>
        internal Type Refer;


        /// <exclude>Excluded</exclude>
        private bool FeaturesLoaded = false;

        ///// <MetaDataID>{34AFCE91-6A33-434A-A031-D5FA611B588D}</MetaDataID>
        //private void LoadFeatures()
        //{
        //    lock (_Features)
        //    {
        //        if (!FeaturesLoaded)
        //        {
        //            lock (Type.LoadDotnetMetadataLock)
        //            {
        //                Refer.GetFeatures(this, ref _Features);
        //                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
        //                try
        //                {

        //                    foreach (MetaDataRepository.Feature feature in _Features)
        //                        _OwnedElements.Add(feature);

        //                }
        //                finally
        //                {
        //                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
        //                }
        //                FeaturesLoaded = true;
        //            }
        //        }
        //    }
        //}
        /// <MetaDataID>{BD3230B7-B13A-4162-B31F-EF29898EA628}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private bool GeneralizationsLoaded = false;




        /// <MetaDataID>{87C35C48-B6E9-4BC7-8458-93E7F26A6789}</MetaDataID>
        private void LoadGeneralizations()
        {
            lock (GeneralizationLock)
            {
                using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if (!GeneralizationsLoaded)
                    {
                        _Generalizations = Refer.GetGeneralizations(this);
                        GeneralizationsLoaded = true;
                    }
                }
            }
        }

        public override Collections.Generic.Set<Generalization> Specializations
        {
            get
            {
                using (SystemStateTransition suppresStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    return new Collections.Generic.Set<Generalization>(base.Specializations);
                }
            }
        }
        /// <MetaDataID>{05DB0397-6131-48DE-9353-C9A9A42D1CA2}</MetaDataID>
        internal Interface(Type theType)
        {


            RolesLoaded = false;
            MetaObjectMapper.AddTypeMap(theType.WrType, this);

            _ImplementationUnit.Value = Assembly.GetComponent(theType.WrType.GetMetaData().Assembly);


            _Name = theType.Name;
            Refer = theType;
            Visibility = Refer.Visibility;

            if (!Refer.IsNestedType)
            {
                if (!string.IsNullOrEmpty(theType.WrType.Namespace))
                {
                    Namespace mNamespace = Type.GetNameSpace(theType.WrType.Namespace);
                    //Namespace mNamespace = (Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(theType.WrType.Namespace);
                    //if (mNamespace == null)
                    //    mNamespace = new Namespace(theType.WrType.Namespace);
                    mNamespace.AddOwnedElement(this);
                    SetNamespace(mNamespace);
                }
            }


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
                    else if (backwardCompatibilityID != null)
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
                    _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.ToString() + Refer.WrType.GetHashCode().ToString());

            }
            #endregion



            if (/*!Refer.WrType.IsGenericType &&*/ Refer.WrType.FullName != null)
                MetaObjectMapper.AddMetaObject(this, Refer.WrType.ToString());
#if !DeviceDotNet


#endif
            Refer.GenericTypeInit(this, ref _OwnedTemplateSignature, ref _TemplateBinding);
        }

        /// <MetaDataID>{A90ECF13-1071-4372-B0F7-C8FFCC325267}</MetaDataID>

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

    }
}
