namespace OOAdvantech.DotNetMetaDataRepository
{
    using System.Linq;
    using System.Threading.Tasks;
    using MetaDataRepository;
    using OOAdvantech.Transactions;


    /// <MetaDataID>{C7E0F1CF-CA78-4AC4-9AD3-BD00C5A6F278}</MetaDataID>
    public class Structure : MetaDataRepository.Structure
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
            if (member.Name == nameof(StructPersistent))
            {
                if (value == null)
                    StructPersistent = default(OOAdvantech.Member<bool>);
                else
                    StructPersistent = (OOAdvantech.Member<bool>)value;
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
            if (member.Name == nameof(RolesLoaded))
            {
                if (value == null)
                    RolesLoaded = default(bool);
                else
                    RolesLoaded = (bool)value;
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

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_FullName))
                return _FullName;

            if (member.Name == nameof(StructPersistent))
                return StructPersistent;

            if (member.Name == nameof(AssociationEndFields))
                return AssociationEndFields;

            if (member.Name == nameof(AssociationEndFastFieldsAccessors))
                return AssociationEndFastFieldsAccessors;

            if (member.Name == nameof(RealizationsLoaded))
                return RealizationsLoaded;

            if (member.Name == nameof(GeneralizationsLoaded))
                return GeneralizationsLoaded;

            if (member.Name == nameof(RolesLoaded))
                return RolesLoaded;

            if (member.Name == nameof(ExtensionMetaObjects))
                return GetExtensionMetaObjects();

            if (member.Name == nameof(Refer))
                return Refer;

            if (member.Name == nameof(FeaturesLoaded))
                return FeaturesLoaded;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        string _FullName;
        /// <MetaDataID>{508aa6e5-5de4-4286-85c3-808c7a96006b}</MetaDataID>
        public override string FullName
        {
            get
            {
                if (_FullName == null)
                    _FullName = base.FullName;
                return _FullName;
            }
        }

        /// <MetaDataID>{7ba479bb-dff4-4a73-a05c-3b7374203c18}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {

        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{ceb0d410-e40f-4d15-a488-5ef57ba973af}</MetaDataID>
        Member<bool> StructPersistent = new Member<bool>();
        /// <MetaDataID>{14cf8d16-1120-41f5-b773-79bd341f037d}</MetaDataID>
        public override bool Persistent
        {
            get
            {
                if (StructPersistent.UnInitialized)
                {
                    if (_Persistent && Refer.WrType.GetMetaData().Assembly == typeof(Class).GetMetaData().Assembly)
                        _Persistent = false;
                    StructPersistent.Value = _Persistent;
                }
                return StructPersistent.Value;
            }
            set
            {
                base.Persistent = value;
            }
        }
        /// <MetaDataID>{40d8fa26-a81e-4f28-8902-0d7b9ea84276}</MetaDataID>
        System.Collections.Generic.Dictionary<AssociationEnd, System.Reflection.FieldInfo> AssociationEndFields = new System.Collections.Generic.Dictionary<AssociationEnd, System.Reflection.FieldInfo>();
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

        //    //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
        //    //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
        //    if (associationEnd.Persistent == true && Persistent && associationEnd.FieldMember == null)
        //        throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + associationEnd.PropertyMember.DeclaringType.FullName + "." + associationEnd.PropertyMember.Name + "] realization.");

        //    AssociationEndFields[associationEnd] = associationEnd.FieldMember;
        //    return associationEnd.FieldMember;
        //}




        /// <MetaDataID>{3af68e94-b019-487a-9d4b-2906d752d4ca}</MetaDataID>
        System.Collections.Generic.Dictionary<AssociationEnd, AccessorBuilder.FieldPropertyAccessor> AssociationEndFastFieldsAccessors = new System.Collections.Generic.Dictionary<AssociationEnd, AccessorBuilder.FieldPropertyAccessor>();
        /// <MetaDataID>{adb9119f-652e-4058-a6b1-738830886875}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor GetFastFieldAccessor(AssociationEnd associationEnd)
        {
            if (!associationEnd.HasAssociationEndRealizations)
            {
                //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
                //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
                if (associationEnd.Persistent == true && Persistent && associationEnd.FieldMember == null)
                    throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + associationEnd.PropertyMember.DeclaringType.FullName + "." + associationEnd.PropertyMember.Name + "] realization.");

                return associationEnd.FastFieldAccessor;
            }
            if (AssociationEndFastFieldsAccessors.ContainsKey(associationEnd))
                return AssociationEndFastFieldsAccessors[associationEnd];

            foreach (AssociationEndRealization associationEndRealization in associationEnd.AssociationEndRealizations)
            {
                if (associationEndRealization.Namespace == this)
                {
                    if (associationEndRealization.FieldMember != null)
                    {
                        AssociationEndFastFieldsAccessors[associationEnd] = associationEndRealization.FastFieldAccessor;
                        return associationEndRealization.FastFieldAccessor;
                    }
                }
            }

            //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
            //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
            if (associationEnd.Persistent == true && Persistent && associationEnd.FieldMember == null)
                throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + associationEnd.PropertyMember.DeclaringType.FullName + "." + associationEnd.PropertyMember.Name + "] realization.");

            AssociationEndFastFieldsAccessors[associationEnd] = associationEnd.FastFieldAccessor;
            return associationEnd.FastFieldAccessor;
        }




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




        /// <MetaDataID>{aa601724-6245-4236-afbb-d4a83d426ac6}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor GetFastFieldAccessor(Attribute attribute)
        {
            if (!attribute.HasAttributeRealizations)
            {
                //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
                //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
                if (attribute.Persistent == true && Persistent && attribute.FieldMember == null)
                    throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + attribute.PropertyMember.DeclaringType.FullName + "." + attribute.PropertyMember.Name + "] realization.");
                return attribute.FastFieldAccessor;
            }
            foreach (MetaDataRepository.Feature feature in Features)
            {
                AttributeRealization attributeRealization = feature as AttributeRealization;
                if (attributeRealization != null)
                {
                    if (attributeRealization.Specification == attribute && attributeRealization.FieldMember != null)
                        return attributeRealization.FastFieldAccessor;
                }
            }
            foreach (MetaDataRepository.Classifier classifier in GetGeneralClasifiers())
            {
                if (classifier is MetaDataRepository.Interface)
                    continue;
                Class _class = classifier as Class;

                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = _class.GetFastFieldAccessor(attribute);
                if (fastFieldAccessor != null)
                    return fastFieldAccessor;
            }
            //TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
            //να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
            if (attribute.Persistent == true && Persistent && attribute.FieldMember == null)
                throw new System.Exception("Class:[" + Name + "] You must declare PersistentMember Attribute for [" + attribute.PropertyMember.DeclaringType.FullName + "." + attribute.PropertyMember.Name + "] realization.");

            return attribute.FastFieldAccessor;
        }








        /// <MetaDataID>{BDDC4B43-15ED-4D6A-ADD7-66454B29601A}</MetaDataID>
        protected Structure()
        {
        }
        /// <MetaDataID>{8a31bab2-1abc-4d87-9df7-98dae844e257}</MetaDataID>
        private bool RealizationsLoaded = false;
        /// <MetaDataID>{3b683f13-3bee-44f4-bbcc-085ceb98a8cd}</MetaDataID>
        private bool GeneralizationsLoaded = false;

        //private void LoadGeneralizations()
        //{
        //    OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
        //    try
        //    {
        //        if (!GeneralizationsLoaded)
        //        {
        //            _Generalizations = Refer.GetGeneralizations(this);
        //            GeneralizationsLoaded = true;
        //        }
        //    }
        //    finally
        //    {
        //        ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
        //    }
        //}

        /// <MetaDataID>{2fbeeb93-15a2-4196-b454-f431309312fa}</MetaDataID>
        private void LoadRealizations()
        {
            lock (RealizationsLock)
            {
                if (!RealizationsLoaded)
                {

                    Refer.GetRealizations(ref _Realizations, this);
                    RealizationsLoaded = true;
                }
            }
        }

        public override Collections.Generic.Set<Generalization> Specializations => new Collections.Generic.Set<Generalization>(base.Specializations);

        /// <MetaDataID>{20008781-ba52-461e-a360-8f4b77c793dc}</MetaDataID>
        private bool RolesLoaded = false;

        //public override OOAdvantech.MetaDataRepository.MetaObjectCollection Generalizations
        //{
        //    get
        //    {
        //        ReaderWriterLock.AcquireReaderLock(10000);
        //        try
        //        {
        //            //if(_Generalizations==null)
        //            LoadGeneralizations();
        //            return new MetaDataRepository.MetaObjectCollection(_Generalizations, MetaDataRepository.MetaObjectCollection.AccessType.ReadOnly);
        //        }
        //        finally
        //        {
        //            ReaderWriterLock.ReleaseReaderLock();
        //        }
        //    }
        //}

        /// <MetaDataID>{a736f2ec-156a-4450-844c-89754aca9679}</MetaDataID>
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

        /// <MetaDataID>{c7fc84cd-645c-4765-b84c-357c04334c9c}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.AssociationEnd> Roles
        {
            get
            {

                lock (RolesLock)
                {
                    if (RolesLoaded)
                    {
                        return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>(_Roles.ToThreadSafeSet(), OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                    }
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



                try
                {
                    lock (RolesLock)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Suppress))
                        {
                            ///LoadAssociatonEnds();
                            if (!RolesLoaded)
                            {
                                lock (Type.LoadDotnetMetadataLock)
                                {
                                    RolesLoaded = true;
                                    Refer.GetRoles(this);

                                    object[] Attributes = Refer.WrType.GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.AssociationClass), false);
                                    if (Attributes.Length > 0)
                                    {
                                        MetaDataRepository.AssociationClass associationClass = (MetaDataRepository.AssociationClass)Attributes[0];
                                        _LinkAssociation = Refer.GetAssociation(associationClass);
                                    }
                                }
                            }
                            return new OOAdvantech.Collections.Generic.Set<MetaDataRepository.AssociationEnd>(_Roles, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                            stateTransition.Consistent = true;
                        }
                    }


                }
                catch (System.Exception error)
                {
                    throw;
                }

            }
        }
        private readonly object StructureHierarcyObjectLock = new object();

        //private readonly object StructureObjectLock = new object();


        /// <MetaDataID>{acff13de-30fe-42de-b4fa-bf3209c313b2}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Feature> Features
        {
            get
            {
                lock (FeaturesLock)
                {
                    if (!FeaturesLoaded)
                    {
                        lock (Type.LoadDotnetMetadataLock)
                        {
                            Refer.GetFeatures(this, ref _Features);
                        }
                        foreach (MetaDataRepository.Feature feature in _Features)
                            _OwnedElements.Add(feature);
                        FeaturesLoaded = true;
                    }
                    return _Features.ToThreadSafeSet();
                }

            }
        }


        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{70C79E45-1507-41FC-ABE2-CE0F8E1413D7}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;

            }
        }


        /// <MetaDataID>{216A9024-BEA6-4D07-9F7A-B6EEEAEFD324}</MetaDataID>
        internal Structure(Type theWrType)
        {
            Refer = theWrType;
            _ImplementationUnit.Value = Assembly.GetComponent(theWrType.WrType.GetMetaData().Assembly);


            _Name = Refer.Name;
            if (!string.IsNullOrEmpty(theWrType.WrType.Namespace))
            {
                Namespace mNamespace = Type.GetNameSpace(Refer.WrType.Namespace);
                //Namespace mNamespace = (Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(Refer.WrType.Namespace);
                //if (mNamespace == null)
                //    mNamespace = new Namespace(Refer.WrType.Namespace);
                mNamespace.AddOwnedElement(this);
                SetNamespace(mNamespace);
            }
            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(Refer.WrType, this);

            if (Refer.WrType.GetMetaData().GetCustomAttributes(typeof(MetaDataRepository.Persistent), true).Length > 0)
                _Persistent = true;
            else
                _Persistent = false;



            #region produce the object _Identity

            OOAdvantech.MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = Assembly.GetBackwardCompatibilityID(theWrType.WrType.GetMetaData().Assembly);

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
                    foreach (System.Type argType in Refer.WrType.GetMetaData().GetGenericArguments())
                    {
                        if (argType.IsGenericParameter)
                        {
                            _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.ToString() + Refer.WrType.GetHashCode().ToString());
                            break;
                        }

                    }
                }
            }
            #endregion

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Suppress))
            {
                #region Generic type initialization
                Refer.GenericTypeInit(this, ref _OwnedTemplateSignature, ref _TemplateBinding);
                #endregion

                stateTransition.Consistent = true;
            }

            //object[] Attributes = Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
            //if (Attributes.Length > 0)
            //{
            //    //There is  BackwardCompatibilityID declaration
            //    string identityAsString = (Attributes[0] as MetaDataRepository.BackwardCompatibilityID).ToString();
            //    if (identityAsString.Length > 0)
            //    {
            //        if (identityAsString[0] == '+')
            //        {
            //            //Extend namespace identity
            //            identityAsString = identityAsString.Substring(1);
            //            if (Namespace != null && Namespace.Identity.ToString().Trim().Length > 0)
            //                identityAsString = Namespace.Identity.ToString().Trim() + "." + identityAsString;
            //        }
            //        _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(identityAsString);

            //    }
            //    else
            //    {
            //        _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.FullName);

            //    }
            //}
            //else
            //{
            //    _Identity = new MetaDataRepository.MetaObjectID(Refer.WrType.FullName);

            //}
            if (Refer.WrType.FullName != null)
                MetaObjectMapper.AddMetaObject(this, Refer.WrType.FullName);



        }

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



        /// <MetaDataID>{64b48bcd-032c-4189-adb7-fc5f5089612a}</MetaDataID>
        private System.Collections.Generic.List<object> ExtensionMetaObjects;


        /// <MetaDataID>{23d13844-5b8a-42a7-b758-4eb6522b12a8}</MetaDataID>
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



        /// <MetaDataID>{E8FEC636-6029-478B-889E-48EC163D2E46}</MetaDataID>
        internal Type Refer;




        /// <MetaDataID>{b2b143c5-c80b-4597-afe1-66580729bcfc}</MetaDataID>
        private bool FeaturesLoaded = false;


        //void LoadAttributeRealization()
        //{
        //    //  LoadGeneralizations();
        //    LoadRealizations();


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

        //void LoadAssociationEndRealization()
        //{
        //    // LoadGeneralizations();
        //    LoadRealizations();
        //    foreach (Interface _interface in GetAllInterfaces())
        //    {
        //        foreach (AssociationEnd associationEnd in _interface.GetAssociateRoles(false))
        //        {
        //            if (associationEnd.PropertyMember != null && associationEnd.Accessors.Length > 0)
        //            {
        //                System.Reflection.MethodInfo accessor = associationEnd.Accessors[0];
        //                foreach (Attribute implementationAttribute in GetAttributes(false))
        //                {
        //                    if (implementationAttribute.PropertyMember == null)
        //                        continue;
        //                    if (implementationAttribute.Accessors.Length > 0)
        //                    {
        //                        if (Refer.GetOperationForMethod(implementationAttribute.Accessors[0]) == accessor)
        //                        {
        //                            _Features.Remove(implementationAttribute);
        //                            AssociationEndRealization associationEndRealization = new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, this);
        //                            _Features.Add(associationEndRealization);
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            if (implementationAttribute.Accessors.Length > 1)
        //                            {
        //                                if (Refer.GetOperationForMethod(implementationAttribute.Accessors[1]) == accessor)
        //                                {
        //                                    _Features.Remove(implementationAttribute);
        //                                    AssociationEndRealization associationEndRealization = new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, this);
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
