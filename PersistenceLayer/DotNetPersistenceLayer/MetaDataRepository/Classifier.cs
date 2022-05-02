using System.Linq;
namespace OOAdvantech.MetaDataRepository
{
    using System.Reflection;
    using Transactions;
#if !DeviceDotNet
    using Map = OOAdvantech.Collections.Map;
#else
    //using Map = System.Collections.Hashtable;
#endif



    public delegate void ClassHierarchyChangedHandler(object sender);

    /// <summary>A classifier is an element that describes behavioral and structural features; it comes in several specific forms, including class, data type, interface, component, artifact, and others that are defined in other metamodel packages.
    /// In the metamodel, a Classifier declares a collection of Features, such as Attributes, Methods, and Operations. It has a name, which is unique in the Namespace enclosing
    /// the Classifier. Classifier is an abstract metaclass.
    /// Classifier is a child of Namespace.
    /// As a Namespace, a Classifier may declare other Classifiers nested in its scope. Nested Classifiers may be accessed by other Classifiers only if the nested Classifiers have
    /// adequate visibility. There are no data value or state consequences of nested Classifiers
    /// (i.e., it is not an aggregation or composition). </summary>
    /// <MetaDataID>{1A46C6A7-2B03-4BC9-A3C0-28301BB83676}</MetaDataID>
    [BackwardCompatibilityID("{1A46C6A7-2B03-4BC9-A3C0-28301BB83676}")]
    [Persistent()]
    public class Classifier : Namespace, ITemplateable, IParameterableElement
    {

        public event ClassHierarchyChangedHandler ClassHierarchyChanged;

        /// <MetaDataID>{ff8f9187-4423-4223-b0ed-5431695cce67}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, MemberInfo member, object value)
        {
            if (member.Name == nameof(ClassHerarchyCaseInsensitiveUniqueNames))
            {
                if (value == null)
                    ClassHerarchyCaseInsensitiveUniqueNames = default(System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.MetaObject, string>);
                else
                    ClassHerarchyCaseInsensitiveUniqueNames = (System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.MetaObject, string>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(dotNetType))
            {
                if (value == null)
                    dotNetType = default(OOAdvantech.Member<System.Type>);
                else
                    dotNetType = (OOAdvantech.Member<System.Type>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(GetTypeClassifierFastInvoke))
            {
                if (value == null)
                    GetTypeClassifierFastInvoke = default(OOAdvantech.AccessorBuilder.FastInvokeHandler);
                else
                    GetTypeClassifierFastInvoke = (OOAdvantech.AccessorBuilder.FastInvokeHandler)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_TemplateBinding))
            {
                if (value == null)
                    _TemplateBinding = default(OOAdvantech.MetaDataRepository.TemplateBinding);
                else
                    _TemplateBinding = (OOAdvantech.MetaDataRepository.TemplateBinding)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LinkAssociation))
            {
                if (value == null)
                    _LinkAssociation = default(OOAdvantech.MetaDataRepository.Association);
                else
                    _LinkAssociation = (OOAdvantech.MetaDataRepository.Association)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ParentClassifiers))
            {
                if (value == null)
                    ParentClassifiers = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>);
                else
                    ParentClassifiers = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ClassifierHierarchyClassifiers))
            {
                lock (GeneralizationLock)
                {
                    if (value == null)
                        ClassifierHierarchyClassifiers = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>);
                    else
                        ClassifierHierarchyClassifiers = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>)value;
                }
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Generalizations))
            {
                if (value == null)
                    _Generalizations = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>);
                else
                    _Generalizations = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_OwnedTemplateSignature))
            {
                if (value == null)
                    _OwnedTemplateSignature = default(OOAdvantech.MetaDataRepository.TemplateSignature);
                else
                    _OwnedTemplateSignature = (OOAdvantech.MetaDataRepository.TemplateSignature)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ClassHierarchyAssociateRoles))
            {
                if (value == null)
                    ClassHierarchyAssociateRoles = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>);
                else
                    ClassHierarchyAssociateRoles = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }


            if (member.Name == nameof(ClassHierarchyFeatures))
            {
                if (value == null)
                    ClassHierarchyFeatures = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature>);
                else
                    ClassHierarchyFeatures = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature>)value;
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
            if (member.Name == nameof(ChildClassifiers))
            {
                if (value == null)
                    ChildClassifiers = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>);
                else
                    ChildClassifiers = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Specializations))
            {
                if (value == null)
                    _Specializations = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>);
                else
                    _Specializations = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Roles))
            {
                if (value == null)
                    _Roles = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>);
                else
                    _Roles = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.AssociationEnd>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Features))
            {
                if (value == null)
                    _Features = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature>);
                else
                    _Features = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{5ca98ab1-43d4-4bbc-a2b7-4c8c9260195b}</MetaDataID>
        public override object GetMemberValue(object token, MemberInfo member)
        {

            if (member.Name == nameof(ClassHerarchyCaseInsensitiveUniqueNames))
                return ClassHerarchyCaseInsensitiveUniqueNames;

            if (member.Name == nameof(dotNetType))
                return dotNetType;

            if (member.Name == nameof(GetTypeClassifierFastInvoke))
                return GetTypeClassifierFastInvoke;

            if (member.Name == nameof(_TemplateBinding))
                return _TemplateBinding;

            if (member.Name == nameof(_LinkAssociation))
                return _LinkAssociation;

            if (member.Name == nameof(ParentClassifiers))
                return ParentClassifiers;

            if (member.Name == nameof(ClassifierHierarchyClassifiers))
                return ClassifierHierarchyClassifiers;

            if (member.Name == nameof(_Generalizations))
                return _Generalizations;

            if (member.Name == nameof(_OwnedTemplateSignature))
                return _OwnedTemplateSignature;

            if (member.Name == nameof(ClassHierarchyAssociateRoles))
                return ClassHierarchyAssociateRoles;

            if (member.Name == nameof(ClassHierarchyRoles))
                return ClassHierarchyRoles;

            if (member.Name == nameof(ClassHierarchyFeatures))
                return ClassHierarchyFeatures;

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(ChildClassifiers))
                return ChildClassifiers;

            if (member.Name == nameof(_Specializations))
                return _Specializations;

            if (member.Name == nameof(_Roles))
                return _Roles;

            if (member.Name == nameof(_Features))
                return _Features;


            return base.GetMemberValue(token, member);
        }


        /// <MetaDataID>{04612abb-0e3b-4fb2-b590-2bef4b944ede}</MetaDataID>
        private object CaseInsensitiveNamesLock = new object();
        /// <MetaDataID>{35b50afd-9a2a-4e42-8d54-840a50af448b}</MetaDataID>
        System.Collections.Generic.Dictionary<MetaObject, string> ClassHerarchyCaseInsensitiveUniqueNames;
        /// <MetaDataID>{c54e52a2-3b4d-495d-8cd0-60350c09de88}</MetaDataID>
        public string GetClassHerarchyCaseInsensitiveUniqueNames(MetaDataRepository.Attribute attribute)
        {
            lock (CaseInsensitiveNamesLock)
            {
                if (ClassHerarchyCaseInsensitiveUniqueNames == null)
                    RetrieveClassHerarchyCaseInsensitiveUniqueNames();
                return ClassHerarchyCaseInsensitiveUniqueNames[attribute];
            }

        }
        /// <MetaDataID>{d1267ab4-beac-4238-9c3a-7c232bfb41bd}</MetaDataID>
        public string GetClassHerarchyCaseInsensitiveUniqueNames(MetaDataRepository.AssociationEnd associationEnd)
        {
            lock (CaseInsensitiveNamesLock)
            {
                if (ClassHerarchyCaseInsensitiveUniqueNames == null)
                    RetrieveClassHerarchyCaseInsensitiveUniqueNames();
                return ClassHerarchyCaseInsensitiveUniqueNames[associationEnd];
            }
        }
        /// <MetaDataID>{4a419642-a456-45f5-9f2b-bbcb63234559}</MetaDataID>
        public string GetClassHerarchyCaseInsensitiveUniqueNames(MetaDataRepository.Association association)
        {
            lock (CaseInsensitiveNamesLock)
            {
                if (ClassHerarchyCaseInsensitiveUniqueNames == null)
                    RetrieveClassHerarchyCaseInsensitiveUniqueNames();

                ///TODO προσωρινά γιατί δεν έχουν φορτωθεί οι associations των structure attribute
                if (!ClassHerarchyCaseInsensitiveUniqueNames.ContainsKey(association))
                    return association.Name;

                return ClassHerarchyCaseInsensitiveUniqueNames[association];
            }
        }




        /// <MetaDataID>{4e4092ca-ca40-4a7e-a958-9352aab5f9f1}</MetaDataID>
        private void RetrieveClassHerarchyCaseInsensitiveUniqueNames()
        {
            lock (CaseInsensitiveNamesLock)
            {
                if (ClassHerarchyCaseInsensitiveUniqueNames == null)
                {
                    var classHerarchyCaseInsensitiveUniqueNames = new System.Collections.Generic.Dictionary<MetaObject, string>();
                    //TODO: Να γραφτεί test case οπου αναζητούντε data από δύο διαφορετικές storage
                    //που έχουν φτιάξει διαφορετικό CaseInsensitiveName για το ίδιο attribute
                    OOAdvantech.Collections.Generic.Dictionary<string, MetaObject> members = new Collections.Generic.Dictionary<string, MetaObject>();
                    foreach (Attribute attribute in GetAttributes(true))
                    {
                        string caseInsensitiveName = attribute.CaseInsensitiveName;
                        if (members.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                        {
                            caseInsensitiveName = attribute.Owner.Name + "_" + attribute.Name;
                            classHerarchyCaseInsensitiveUniqueNames[attribute] = caseInsensitiveName;
                            //attribute.CaseInsensitiveName = caseInsensitiveName;
                        }
                        int count = 1;
                        while (members.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                        {
                            caseInsensitiveName = caseInsensitiveName + "_" + count.ToString();
                            count++;
                        }
                        members.Add(caseInsensitiveName.Trim().ToLower(), attribute);
                        classHerarchyCaseInsensitiveUniqueNames[attribute] = caseInsensitiveName;
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
                        classHerarchyCaseInsensitiveUniqueNames[associationEnd.Association] = caseInsensitiveName;
                    }
                    if (ClassHierarchyLinkAssociation != null)
                        classHerarchyCaseInsensitiveUniqueNames[ClassHierarchyLinkAssociation] = ClassHierarchyLinkAssociation.Name;


                    foreach (AssociationEnd associationEnd in GetAssociateRoles(true))
                    {
                        string caseInsensitiveName = associationEnd.CaseInsensitiveName;
                        if (members.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                        {
                            caseInsensitiveName = associationEnd.Namespace.Name + "_" + associationEnd.Name;
                            classHerarchyCaseInsensitiveUniqueNames[associationEnd] = caseInsensitiveName;
                            //attribute.CaseInsensitiveName = caseInsensitiveName;
                        }
                        int count = 1;
                        while (members.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                        {
                            caseInsensitiveName = caseInsensitiveName + "_" + count.ToString();
                            count++;
                        }
                        members.Add(caseInsensitiveName.Trim().ToLower(), associationEnd);
                        classHerarchyCaseInsensitiveUniqueNames[associationEnd] = caseInsensitiveName;
                    }


                    ClassHerarchyCaseInsensitiveUniqueNames = classHerarchyCaseInsensitiveUniqueNames;
                }

            }
        }



        /// <MetaDataID>{56e732d5-64be-4cee-82a3-28d8e7e07ee8}</MetaDataID>
        Member<System.Type> dotNetType = new Member<System.Type>();
        /// <MetaDataID>{96a9fa15-6e41-448d-ba8a-1ea2b3807718}</MetaDataID>
        public override object GetExtensionMetaObject(System.Type MetaObjectType)
        {

            if (MetaObjectType == typeof(System.Type))
            {
                if (dotNetType.UnInitialized)
                {
                    System.Type type = base.GetExtensionMetaObject(MetaObjectType) as System.Type;
                    if (type != null)
                        dotNetType.Value = type;
                    else
                    {

                        try
                        {
                            //TODO : δεν χρησιμοποιήθηκε το FullName γιατι δεν ενημερωνεται σωστά στο RDBMSMetadatarepository 
                            //dotNetType.Value = System.Reflection.Assembly.Load(ImplementationUnit.FullName).GetType(FullName);
#if !DeviceDotNet
                            if (_ImplementationUnit.Value != null)
                                dotNetType.Value = System.Reflection.Assembly.Load(ImplementationUnit.Identity.ToString()).GetType(FullName);
                            else
                                dotNetType.Value = ModulePublisher.ClassRepository.GetType(FullName, "");
#else
                            if (_ImplementationUnit.Value != null)
                                dotNetType.Value = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(ImplementationUnit.Identity.ToString())).GetType(FullName);
                            else
                                dotNetType.Value = ModulePublisher.ClassRepository.GetType(FullName, "");

#endif

                        }
                        catch (System.Exception error)
                        {
                        }
                    }
                }
                return (System.Type)dotNetType;
            }


            return base.GetExtensionMetaObject(MetaObjectType);
        }
        /// <MetaDataID>{889cf0fb-d14b-451b-a3fe-391f2f2222b6}</MetaDataID>
        public bool IsTemplate
        {
            get
            {

                if (OwnedTemplateSignature != null && TemplateBinding == null)
                    return true;
                if (TemplateBinding != null)
                {
                    foreach (TemplateParameterSubstitution parameterSubstitution in TemplateBinding.ParameterSubstitutions)
                    {
                        if (parameterSubstitution.ActualParameters[0] is TemplateParameter)
                            return true;
                    }
                }
                return false;
            }
        }
        /// <MetaDataID>{b2e46f9e-a81a-43d3-9513-374b406f3d4d}</MetaDataID>
        public bool IsTemplateInstantiation
        {
            get
            {
                if (TemplateBinding != null)
                {
                    foreach (TemplateParameterSubstitution parameterSubstitution in TemplateBinding.ParameterSubstitutions)
                    {
                        if (parameterSubstitution.ActualParameters[0] is TemplateParameter)
                            return false;
                    }
                    return true;
                }
                return false;
            }
        }

        /// <MetaDataID>{25a47492-5aed-499d-aa46-796d7baec76a}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler GetTypeClassifierFastInvoke;

        /// <MetaDataID>{809f5780-83b2-4cfc-a530-4747a7d1f6cb}</MetaDataID>
        static Classifier()
        {

            //#if Net4
            //            GetTypeClassifierFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Type", "DotNetMetaDataRepository, Culture=neutral, PublicKeyToken=00a88b51a86dbd3c").GetMethod("GetClassifierObject"));
            //#else
#if !DeviceDotNet
            GetTypeClassifierFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Type", "DotNetMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=11a79ce55c18c4e7").GetMethod("GetClassifierObject"));
#else
            GetTypeClassifierFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Type", "DotNetMetaDataRepository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").GetMetaData().GetMethod("GetClassifierObject"));
#endif
            //#endif
        }
        /// <MetaDataID>{ca41beb6-02f1-4682-8c8d-c82bdff2f062}</MetaDataID>
        public static Classifier GetClassifier(System.Type type)
        {
            return GetTypeClassifierFastInvoke.Invoke(null, new object[1] { type }) as Classifier;
        }
        /// <MetaDataID>{70b81fff-439d-4672-aa51-56e678ec9b55}</MetaDataID>
        public override string ToString()
        {
            return FullName;
        }
        /// <MetaDataID>{908e4d8e-fa51-48d0-8535-8dd6aaa7a20a}</MetaDataID>
        public virtual Collections.Generic.List<Operation> Constractors
        {
            get
            {
                return GetOperations(Name);
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{23B883D8-3250-4578-9684-D3F340356093}</MetaDataID>
        protected TemplateBinding _TemplateBinding;
        /// <MetaDataID>{F63BEC73-A03D-430A-8AEB-6B5FFBB4CE0D}</MetaDataID>
        [BackwardCompatibilityID("+60")]
        [PersistentMember("_TemplateBinding")]
        public TemplateBinding TemplateBinding
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _TemplateBinding;
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
                    if (_TemplateBinding != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _TemplateBinding = value;
                            stateTransition.Consistent = true;
                        }

                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{31F3B015-105D-4904-AC45-DF9DC6BB7B48}</MetaDataID>
        protected Association _LinkAssociation;
        /// <summary>Specifies the corresponding association if the specified object is a link class. ##</summary>
        /// <MetaDataID>{E16A1125-E780-44B7-A33A-A43334447053}</MetaDataID>
        [Association("AssociationClass", typeof(Association), MetaDataRepository.Roles.RoleB, "{D12835A7-0E6D-45A9-A091-4AC1403B479E}"), AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_LinkAssociation")]
        [RoleBMultiplicityRange(0, 1)]
        public virtual Association LinkAssociation
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _LinkAssociation;
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
                        _LinkAssociation = value;
                        stateTransition.Consistent = true;
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{14D56810-E0F1-4828-A5E6-047132F6190D}</MetaDataID>
        internal OOAdvantech.Collections.Generic.Set<Classifier> ParentClassifiers = null;
        /// <MetaDataID>{AF06C332-6F92-44CA-A8E7-38A2E4433090}</MetaDataID>
        protected OOAdvantech.Collections.Generic.Set<Classifier> ClassifierHierarchyClassifiers;

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E81C4E39-9928-4BFF-A3A0-0C279B5FB887}</MetaDataID>
        protected OOAdvantech.Collections.Generic.Set<Generalization> _Generalizations = new OOAdvantech.Collections.Generic.Set<Generalization>();
        /// <summary>Designates a Generalization whose parent GeneralizableElement is the immediate ancestor of the current GeneralizableElement. </summary>
        /// <MetaDataID>{D96A0B4F-C342-4346-9436-07E3624E163E}</MetaDataID>
        [Association("Generalization", typeof(OOAdvantech.MetaDataRepository.Generalization), MetaDataRepository.Roles.RoleB, "{31EE00E4-640A-493C-8662-08326CE3D600}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_Generalizations")]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.Set<Generalization> Generalizations
        {
            get
            {



                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                {

                    foreach (var generalization in _Generalizations.ToThreadSafeSet())
                    {
                        if (generalization.Parent == this)
                        {
                            _Generalizations.Remove(generalization);
                            _Specializations.Add(generalization);
                        }
                    }
                    stateTransition.Consistent = true;
                    
                }
                return _Generalizations.ToThreadSafeSet();


            }
        }


        /// <MetaDataID>{C6E5DBD0-F50F-483C-9FD0-D538D17F9755}</MetaDataID>
        public virtual Collections.Generic.Set<StorageCell> StorageCellsOfThisType
        {
            get
            {
                return new OOAdvantech.Collections.Generic.Set<StorageCell>();
            }
        }

        /// <MetaDataID>{44b1275f-3d23-4342-a817-2b15b4902d22}</MetaDataID>
        public bool IsBindedClassifier
        {
            get
            {
                return TemplateBinding != null;
            }
        }

        /// <MetaDataID>{7427308b-c1d8-47f0-ba41-a806157b257f}</MetaDataID>
        public bool HasemplateParameter
        {
            get
            {
                if (OwnedTemplateSignature != null)
                    return true;
                if (TemplateBinding == null)
                    return false;
                foreach (TemplateParameterSubstitution parameterSubstitution in TemplateBinding.ParameterSubstitutions)
                {
                    if (parameterSubstitution.ActualParameters[0] is TemplateParameter)
                        return true;
                }

                return false;
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9A3A9EB1-40F1-4A1C-A13D-392DFA35BBBE}</MetaDataID>
        protected TemplateSignature _OwnedTemplateSignature;
        /// <MetaDataID>{36F23D5E-7DBE-42C3-AF19-C2014B7388C4}</MetaDataID>
        [BackwardCompatibilityID("+59")]
        [PersistentMember("_OwnedTemplateSignature")]
        public virtual TemplateSignature OwnedTemplateSignature
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _OwnedTemplateSignature;
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
                    if (_OwnedTemplateSignature != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _OwnedTemplateSignature = value;
                            stateTransition.Consistent = true;
                        }

                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{3E48FA1E-7017-4230-928F-2AF856C47583}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Classifier> GetAllSpecializeClasifiers()
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
            return classes;
        }


        /// <MetaDataID>{A36C7185-4571-472A-8336-6FDDDB47F4C1}</MetaDataID>
        ///Cache data
        internal protected OOAdvantech.Collections.Generic.Set<AssociationEnd> ClassHierarchyAssociateRoles;
        /// <MetaDataID>{D2E3633B-7138-4BE8-8752-1C98AE3E9704}</MetaDataID>
        ///Cache data
        internal protected OOAdvantech.Collections.Generic.Set<AssociationEnd> ClassHierarchyRoles;
        /// <MetaDataID>{7344BD39-12F4-48E3-B5CC-B3B64ABCADE8}</MetaDataID>
        ///Cache data
        internal OOAdvantech.Collections.Generic.Set<MetaDataRepository.AssociationEnd> ClassAssociateRoles;





        /// <MetaDataID>{e5718393-5c34-4bb1-a765-40cd13aaff86}</MetaDataID>
        internal Collections.Generic.Set<Feature> ClassHierarchyFeatures;

        /// <MetaDataID>{8111B229-1C12-4E5D-8176-F92D11259053}</MetaDataID>
        public virtual Association ClassHierarchyLinkAssociation
        {
            get
            {
                return null;
            }
        }
        /// <summary>This method ensures that all members have case insensitive unique name. </summary>
        /// <remarks>
        /// Some of object oriented languages are case insensitive. 
        /// Case insensitive means that “name” and “Name” are the same thing. 
        /// The BuildCaseInsensitiveNames in that cases change the second from “Name” to “Name_1” 
        /// or other case insensitive unique name in namespace.
        /// </remarks>
        /// <MetaDataID>{8BF2D570-04F4-4319-80BE-3794E0232C7D}</MetaDataID>
        public override void BuildCaseInsensitiveNames()
        {

            lock (FeaturesLock)
            {
                OOAdvantech.Collections.Generic.Dictionary<string, MetaObject> members = new Collections.Generic.Dictionary<string, MetaObject>();
                foreach (Feature feature in Features)
                {
                    //TODO θα πρέπει να φτιαχτεί η equal fanctionality στην operation 
                    if (feature is Method || feature is Operation)
                        continue;

                    string caseInsensitiveName = feature.CaseInsensitiveName;
                    int count = 1;
                    while (members.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                    {
                        caseInsensitiveName = feature.CaseInsensitiveName + "_" + count.ToString();
                        count++;
                    }
                    members.Add(caseInsensitiveName.Trim().ToLower(), feature);
                    if (caseInsensitiveName.Trim().ToLower() != feature.Name.ToLower())
                        feature.CaseInsensitiveName = caseInsensitiveName;
                }
                foreach (AssociationEnd associationEnd in GetAssociateRoles(false))
                {
                    string caseInsensitiveName = associationEnd.CaseInsensitiveName;
                    int count = 1;
                    while (members.ContainsKey(caseInsensitiveName.Trim().ToLower()))
                    {
                        caseInsensitiveName = associationEnd.CaseInsensitiveName + "_" + count.ToString();
                        count++;
                    }

                    members.Add(caseInsensitiveName.Trim().ToLower(), associationEnd);
                    if (caseInsensitiveName.Trim().ToLower() != associationEnd.Name.ToLower())
                        associationEnd.CaseInsensitiveName = caseInsensitiveName;
                }
            }

        }


        /// <MetaDataID>{DACF102E-4DE7-4DFD-B1B7-2A6FAAD3FD7E}</MetaDataID>
        /// <summary>InErrorCheck attribute protected the ErrorCheck method from recursive call. </summary>
        private bool InErrorCheck = false;
        /// <MetaDataID>{AD71339A-535B-4557-A5D7-63846EAE2A6B}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {
            if (InErrorCheck)
                return false;
            try
            {
                InErrorCheck = true;
                bool hasError = base.ErrorCheck(ref errors);
                foreach (Feature feature in Features)
                    hasError |= feature.ErrorCheck(ref errors);

                foreach (AssociationEnd associationEnd in GetAssociateRoles(false))
                    hasError |= associationEnd.ErrorCheck(ref errors);

                return hasError;
            }
            catch (System.Exception error)
            {
                errors.Add(new MetaObject.MetaDataError("MDR Error: " + error.Message + "\r\n" + error.StackTrace, FullName));
                return true;

            }
            finally
            {
                InErrorCheck = false;
            }
        }

        /// <MetaDataID>{C6D274B9-E662-4E3E-9F0A-0A799E76D038}</MetaDataID>
        public Classifier()
        {

        }

        /// <MetaDataID>{2CDEE71F-6E85-4CC4-924E-D1ABB39AFF69}</MetaDataID>
        public virtual bool IsA(MetaDataRepository.Classifier classifier)
        {
            //ReaderWriterLock.AcquireReaderLock(10000);
            //try
            //{
            if (classifier == this)
                return true;
            var allGeneralClassifiers = GetAllGeneralClasifiers().ToList();
            return allGeneralClassifiers.Contains(classifier);
            //}
            //finally
            //{
            //    ReaderWriterLock.ReleaseReaderLock();
            //}
        }




        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        /// <MetaDataID>{22FB8DFB-E282-4DD8-86E2-BA7DF62E6C5D}</MetaDataID>
        public override void Synchronize(MetaObject originMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                //TODO με κάποιο τρόπο θα πρέπει να λέω όταν είναι απραίτητο ότι έχει αλλάξει
                //κάτι στην ιεραρχία και πρέπει να αναπαραχτούν τα cash data.
                RefreshClassHierarchyCollections();

                if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                    return;
                base.Synchronize(originMetaObject);
                MetaDataRepository.SynchronizerSession.MetaObjectUnderSynchronization(this);
                Classifier OriginClassifier = (Classifier)originMetaObject;


                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {

                    #region Sychronize generalization relationship
                    long count = Generalizations.Count;

                    ContainedItemsSynchronizer GeneralizationSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(OriginClassifier.Generalizations, _Generalizations, this);
                    GeneralizationSynchronizer.FindModifications();
                    GeneralizationSynchronizer.ExecuteAddCommand();
                    GeneralizationSynchronizer.ExecuteDeleteCommand();
                    GeneralizationSynchronizer.Synchronize();
                    foreach (MetaDataRepository.Generalization CurrGeneralization in _Generalizations)
                        CurrGeneralization.Child = this;

                    #endregion

                    #region Sychronize pecialization relationship
                    //TODO: Νομίζω ότι δεν χριάζεται και ότι μπορεί να δημιουργίσει προβλήματα
                    //					ContainedItemsSynchronizer SpecializationSynchronizer=new ContainedItemsSynchronizer(OriginClassifier.Specializations,_Specializations,this);
                    //					SpecializationSynchronizer.FindModifications();
                    //					SpecializationSynchronizer.ExecuteAddCommand();
                    //					SpecializationSynchronizer.ExecuteDeleteCommand();
                    //					SpecializationSynchronizer.Synchronize();
                    //					foreach( MetaDataRepository.Generalization CurrGeneralization in _Specializations)
                    //						CurrGeneralization.Parent=this;

                    #endregion

                    #region Sychronize feature relationship

                    count = Features.Count;
                    ContainedItemsSynchronizer FeatureSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(OriginClassifier.Features, _Features, this);
                    FeatureSynchronizer.FindModifications();
                    if(FeatureSynchronizer.DeletedObjectsCommands.Count>0)
                    {

                    }
                    FeatureSynchronizer.ExecuteAddCommand();
                    FeatureSynchronizer.ExecuteDeleteCommand();
                    foreach (Feature CurrFeature in _Features)
                        CurrFeature.SetOwner(this);
                    FeatureSynchronizer.Synchronize();


                    #endregion

                    #region Sychronize roles relationship
                    count = Roles.Count;
                    count = OriginClassifier.Roles.Count;

                    if (Identity.ToString() == "{0705a4fa-af2e-4a58-b666-e1bba0e07f2a}")
                    {

                    }


                    ContainedItemsSynchronizer RolesSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(OriginClassifier.Roles, _Roles, this);
                    RolesSynchronizer.FindModifications();
                    foreach (MetaDataRepository.DeleteCommand deleteCommand in new System.Collections.Generic.List<DeleteCommand>(RolesSynchronizer.DeletedObjectsCommands))
                    {
                        if((deleteCommand.CandidateForDeleteObject as AssociationEnd).Association!=null &&
                            (deleteCommand.CandidateForDeleteObject as AssociationEnd).Association.Connections.Count>0)
                        {
                            if ((deleteCommand.CandidateForDeleteObject as AssociationEnd).Navigable
                                && !(deleteCommand.CandidateForDeleteObject as AssociationEnd).GetOtherEnd().Navigable
                                && deleteCommand.CandidateForDeleteObject.ImplementationUnit != null &&
                                deleteCommand.CandidateForDeleteObject.ImplementationUnit.Identity != ImplementationUnit.Identity)
                                RolesSynchronizer.DeletedObjectsCommands.Remove(deleteCommand);
                            else
                            {
                                (deleteCommand.CandidateForDeleteObject as AssociationEnd).Specification.RemoveRole((deleteCommand.CandidateForDeleteObject as AssociationEnd).GetOtherEnd());

                                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(deleteCommand.CandidateForDeleteObject);

                            }
                        }
                       
                    }
                    RolesSynchronizer.GetAddedObject();
                    foreach (var addCommand in RolesSynchronizer.AddedObjectsCommands.ToList())
                    {
                        if (addCommand.AddedObject != null)
                            (addCommand.AddedObject as AssociationEnd).SetSpecification(null);
                    }



                    RolesSynchronizer.ExecuteAddCommand();
                    RolesSynchronizer.ExecuteDeleteCommand();
                    RolesSynchronizer.Synchronize();
                    foreach (AssociationEnd CurrRole in _Roles)
                    {
                        CurrRole.SetSpecification(this);
                        if (CurrRole.GetOtherEnd() != null)
                        {
                            CurrRole.GetOtherEnd().SetNamespace(this);
                        }
                    }

                    #endregion


                    if (OriginClassifier.IsTemplate)
                    {

                        if (_OwnedTemplateSignature == null)
                            _OwnedTemplateSignature = new OOAdvantech.MetaDataRepository.TemplateSignature(this);
                        int i = 0;
                        foreach (var templateParameter in OriginClassifier.OwnedTemplateSignature.OwnedParameters)
                        {
                            if (_OwnedTemplateSignature.OwnedParameters.Count > i)
                                _OwnedTemplateSignature.OwnedParameters[i].Name = templateParameter.Name;
                            else
                                _OwnedTemplateSignature.AddOwnedParameter(new OOAdvantech.MetaDataRepository.TemplateParameter(templateParameter.Name));
                            i++;
                        }

                    }


                    StateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <summary>This method deletes an association from classifier. </summary>
        /// <MetaDataID>{B5167BE1-3816-454D-A503-293F09366EA8}</MetaDataID>
        public virtual void RemoveAssociation(Association theAssociation)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
            {
                if (_Roles.Contains(theAssociation.RoleA))
                    _Roles.Remove(theAssociation.RoleA);

                if (_Roles.Contains(theAssociation.RoleB))
                    _Roles.Remove(theAssociation.RoleB);

                stateTransition.Consistent = true;
            }


        }
        /// <summary>This method retrieves the parent Classifiers  of the Classifier. 
        /// Note that this method is not recursive.  
        /// This method does not retrieve the grandparents, 
        /// or any other ancestors, of the Classifier. 
        /// To retrieve all ancestors,  Call GetAllGeneralClasifiers method. </summary>
        /// <MetaDataID>{5B717ADD-2735-40AF-BC5E-EF8B4DC85C77}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Classifier> GetGeneralClasifiers()
        {

            lock (ClassHierarchyLock)
            {
                if (ParentClassifiers != null)
                    return ParentClassifiers;
            }
            OOAdvantech.Collections.Generic.Set<Classifier> generalClassifiers = new OOAdvantech.Collections.Generic.Set<Classifier>();

            foreach (Generalization generalization in Generalizations)
                generalClassifiers.Add(generalization.Parent);


            lock (ClassHierarchyLock)
            {
                ParentClassifiers = generalClassifiers;
                return ParentClassifiers;
            }
        }
        /// <MetaDataID>{42148176-cfed-4f35-bef3-25df9f792bf0}</MetaDataID>
        public virtual void RefreshClassHierarchyCollections()
        {
            //TODO με κάποιο τρόπο θα πρέπει να λέω όταν είναι απραίτητο ότι έχει αλλάξει
            //κάτι στην ιεραρχία και πρέπει να αναπαραχτούν τα cash data.

            lock (ClassHierarchyLock)
            {
                ClassHierarchyAssociateRoles = null; //for refresh
                ClassHierarchyRoles = null; //for refresh
                ParentClassifiers = null;
                ClassifierHierarchyClassifiers = null;
                ClassHierarchyFeatures = null;
            }



            foreach (MetaDataRepository.Generalization generalization in Specializations)
            {
                if (generalization.Child != null)
                    generalization.Child.RefreshClassHierarchyCollections();
            }
        }
        /// <summary>This method deletes an attribute from classifier. </summary>
        /// <param name="theAttribute">Attribute being deleted from the classifier. </param>
        /// <MetaDataID>{63A680D2-5DA8-4873-BE16-60E6CC2FE14B}</MetaDataID>
        public virtual void RemoveAttribute(Attribute theAttribute)
        {
            RemoveFeature(theAttribute);
        }
        /// <MetaDataID>{e78a6185-ce07-485d-9a80-918d26b751e9}</MetaDataID>
        public virtual void RemoveFeature(Feature feature)
        {
            lock (FeaturesLock)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _Features.Remove(feature);
                    stateTransition.Consistent = true;
                }
            }
            RefreshClassHierarchyCollections();
            MetaObjectChangeState();

        }




        /// <summary>This method retrieves the all parent Classifiers of the Classifier. 
        /// Note this method is recursive.  
        /// This method retrieves also the grandparents, of the Classifier. 
        /// To retrieve only parent Classifiers call the GetGeneralClasifiers method.. </summary>
        /// <MetaDataID>{4D3466D1-D7EA-4A26-A16C-BCF1076078B0}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Classifier> GetAllGeneralClasifiers()
        {
            lock (GeneralizationLock)
            {
                if (ClassifierHierarchyClassifiers != null)
                {
                    return ClassifierHierarchyClassifiers;
                }
                else
                {
                    OOAdvantech.Collections.Generic.Set<Classifier> generalClasifiers = new OOAdvantech.Collections.Generic.Set<Classifier>();
                    foreach (Generalization generalization in Generalizations)
                    {
                        if (generalization.Parent == null)
                            continue;
                        generalClasifiers.AddRange(generalization.Parent.GetAllGeneralClasifiers());
                        generalClasifiers.Add(generalization.Parent);
                    }
                    ClassifierHierarchyClassifiers = new Collections.Generic.Set<Classifier>(generalClasifiers, Collections.CollectionAccessType.ReadOnly);

                    return ClassifierHierarchyClassifiers;

                }
            }

        }
        /// <summary>This method deletes an operation from  classifier.</summary>
        /// <param name="theOperation">Operation being deleted from the classifier. </param>
        /// <MetaDataID>{F6C0A99A-4903-41C5-89CB-5E7DBEDEF894}</MetaDataID>
        public virtual void RemoveOperation(Operation theOperation)
        {
            RemoveFeature(theOperation);
        }



        /// <summary>This method retrieves the collection of components assigned to a classifier. </summary>
        /// <MetaDataID>{DCE5F896-F465-423F-B319-5FBBEA3E8A35}</MetaDataID>
        public OOAdvantech.Collections.Generic.Set<Classifier> GetAssignedComponents()
        {
            return null;
        }

        /// <MetaDataID>{4d1ef61d-7eaa-47e3-8d22-2c9c664068e9}</MetaDataID>
        internal OOAdvantech.Collections.Generic.Set<Classifier> ChildClassifiers = null;

        /// <summary>This method retrieves the SpecializedClasifiers belonging to the Clasifier. </summary>
        /// <MetaDataID>{F4EE8C95-0415-436C-83A6-3A75EB464E3E}</MetaDataID>
        public OOAdvantech.Collections.Generic.Set<Classifier> GetSpecializedClasifiers()
        {

            lock (ClassHierarchyLock)
            {
                if (ChildClassifiers != null)
                    return ChildClassifiers;
            }
            var childClassifiers = new OOAdvantech.Collections.Generic.Set<Classifier>();

            foreach (Generalization specialization in Specializations)
                childClassifiers.Add(specialization.Child);

            lock (ClassHierarchyLock)
            {
                ChildClassifiers = childClassifiers;
                return ChildClassifiers;
            }

        }
        /// <summary>This method adds an association to  Classifier and returns it in the specified object. </summary>
        /// <param name="supplierRoleName">Name of the supplier role in the association </param>
        /// <param name="supplierRoleClass">Name of the Classifier, use case, or actor to which to attach the association
        /// Note: If this name is not unique, you must use the qualified name (for example, 
        /// Namespace::Namespace(...)::supplier_name  CPlusplus Style or Namespace.Namespace(...).supplier_name CSharp Style) </param>
        /// <MetaDataID>{F1A99894-D53D-44D2-8810-8701952D673A}</MetaDataID>
        public virtual Association AddAssociation(string supplierRoleName, MetaDataRepository.Roles supplierRole, Classifier supplierRoleClass, string associationIdentity)
        {

            lock (RolesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    Association association = null;
                    //public Association(string name,AssociationEnd roleA,AssociationEnd roleA)
                    if (supplierRole == MetaDataRepository.Roles.RoleA)
                    {
                        PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                        System.Type[] ctorTypes = new System.Type[5] { typeof(string), typeof(Classifier), typeof(string), typeof(Classifier), typeof(string) };
                        association = objectStorage.NewObject(typeof(Association), ctorTypes, "", supplierRoleClass, supplierRoleName, this, "") as Association;
                        _Roles.Add(association.RoleB);
                    }
                    else
                    {
                        PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                        System.Type[] ctorTypes = new System.Type[5] { typeof(string), typeof(Classifier), typeof(string), typeof(Classifier), typeof(string) };
                        association = objectStorage.NewObject(typeof(Association), ctorTypes, supplierRoleName, this, "", supplierRoleClass, "") as Association;
                        _Roles.Add(association.RoleA);

                    }
                    stateTransition.Consistent = true;
                    return association;
                }
            }
        }
        /// <summary>This function creates a new attribute and adds it to  Classifier. </summary>
        /// <param name="attributeName">Name of the attribute being added to the Classifier. </param>
        /// <param name="attributeType">Type of attribute being added to the Classifier.
        ///  If this name is not unique, you must use the qualified name (for example, 
        /// Namespace::Namespace(...)::supplier_name  CPlusplus Style or Namespace.Namespace(...).supplier_name CSharp Style) </param>
        /// <param name="initialValue">Initial value of the attribute </param>
        /// <MetaDataID>{702A8A79-7292-4D22-A43C-7BC817052CD4}</MetaDataID>
        public virtual Attribute AddAttribute(string attributeName, Classifier attributeType, string initialValue)
        {
            lock (FeaturesLock)
            {
                Attribute attribute = null;
                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {

                    System.Type[] ctorTypes = new System.Type[2] { typeof(string), typeof(Classifier) };
                    PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties);
                    attribute = objectStorage.NewObject(typeof(Attribute), ctorTypes, attributeName, attributeType) as Attribute;
                    _Features.Add(attribute);
                    attribute.SetOwner(this);
                    stateTransition.Consistent = true;

                }
                RefreshClassHierarchyCollections();
                return attribute;
            }
        }
        /// <summary>This function creates a new operation and adds it to Classifier. </summary>
        /// <param name="operationName">Name of the operation being added to the Classifier. </param>
        /// <param name="opertionType">Type of operation being added to the Classifier.
        /// If this name is not unique, you must use the qualified name (for example, 
        /// Namespace::Namespace(...)::supplier_name  CPlusplus Style or Namespace.Namespace(...).supplier_name CSharp Style) </param>
        /// <MetaDataID>{38714D68-2814-4B49-87A0-86C154BFE3A1}</MetaDataID>
        public virtual Operation AddOperation(string operationName, Classifier opertionType)
        {
            lock (FeaturesLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {

                    System.Type[] ctorTypes = new System.Type[2] { typeof(string), typeof(Classifier) };
                    PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    Operation operation = objectStorage.NewObject(typeof(Operation), ctorTypes, operationName, opertionType) as Operation;
                    _Features.Add(operation);
                    operation.SetOwner(this);
                    stateTransition.Consistent = true;
                    return operation;
                }
            }
        }





        /// <MetaDataID>{36baf1bd-2611-407f-a8f4-c410676310e2}</MetaDataID>
        protected readonly object ClassHierarchyLock = new object();

        /// <MetaDataID>{f854a34d-4685-4677-bdd4-e4b13b9f3034}</MetaDataID>
        protected readonly object MembersSpecializationPropertiesLock = new object();

        /// <summary>This method retrieves the roles of the Classifiers associated with the specified classifier and returns them in the specified object. </summary>
        /// <param name="Inherit">Specifies whether to search this member's inheritance chain to find the association end. </param>
        /// <MetaDataID>{9DEA3566-C254-4A9D-9434-EDF976539016}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<AssociationEnd> GetAssociateRoles(bool Inherit)
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
                lock (ClassHierarchyLock)
                {
                    ClassHierarchyAssociateRoles = new OOAdvantech.Collections.Generic.Set<AssociationEnd>(associateRoles, Collections.CollectionAccessType.ReadOnly);
                    return ClassHierarchyAssociateRoles;
                }
            }
        }

        /// <summary>This function deletes an Generalization Relation from  Classifier. </summary>
        /// <param name="theGeneralization">Generalization Relation being deleted from the Classifier. </param>
        /// <MetaDataID>{6DBADC18-2669-4874-A85F-760310160A8E}</MetaDataID>
        public virtual void RemoveGeneralization(Generalization theGeneralization)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _Generalizations.Remove(theGeneralization);
                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <summary>This method creates a new InheritRelation and adds it to Classifier. </summary>
        /// <param name="name">Name of the relationship being added to the Classifier. </param>
        /// <param name="parentClassifier">Name of the parent class from which the Classifier inherits its properties and methods
        /// If this name is not unique, you must use the qualified name (for example, 
        /// Namespace::Namespace(...)::supplier_name  CPlusplus Style or Namespace.Namespace(...).supplier_name CSharp Style) </param>
        /// <MetaDataID>{FC498718-ED2E-4AD3-857C-B4C430C571C7}</MetaDataID>
        public virtual Generalization AddGeneralization(string name, Classifier parentClassifier)
        {
            lock (GeneralizationLock)
            {
                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    System.Type[] ctorTypes = new System.Type[3] { typeof(string), typeof(Classifier), typeof(Classifier) };
                    PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    Generalization generalization = objectStorage.NewObject(typeof(Generalization), ctorTypes, name, parentClassifier, this) as Generalization;
                    _Generalizations.Add(generalization);
                    stateTransition.Consistent = true;
                    return generalization;
                }
            }

        }
        /// <MetaDataID>{6a27edb4-8746-46f1-9847-4867749c256d}</MetaDataID>
        virtual internal protected void AddRole(AssociationEnd associationEnd)
        {
            lock (RolesLock)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                {

                    _Roles.Add(associationEnd);
                    stateTransition.Consistent = true;
                }
            }



        }
        /// <MetaDataID>{edcb9ef0-2fb3-49d6-ae5b-973341e74304}</MetaDataID>
        virtual internal protected void RemoveRole(AssociationEnd associationEnd)
        {
            lock (RolesLock)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                {
                    if (_Roles.Contains(associationEnd))
                        _Roles.Remove(associationEnd);
                    stateTransition.Consistent = true;
                }
            }


        }
        /// <MetaDataID>{20B25610-2C79-4497-AB8C-D9D55FB778EB}</MetaDataID>
        internal void AddSpecialization(Generalization theRelationship)
        {

            lock (SpecializationsLock)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                {
                    _Specializations.Add(theRelationship);
                    stateTransition.Consistent = true;
                }
            }
            foreach(var classifier in  GetGeneralClasifiers())
                classifier.RunClassHierarchyChanged();
            ClassHierarchyChanged?.Invoke(this);


        }

        /// <MetaDataID>{086eb674-7a3a-4230-a593-efae55665263}</MetaDataID>
        private void RunClassHierarchyChanged()
        {
            ClassHierarchyChanged?.Invoke(this);
        }

        /// <MetaDataID>{0e3e775e-fdd6-4ffe-92fa-fb3322111f8f}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Feature> GetFeatures(bool Inherit)
        {


            lock (ClassHierarchyLock)
            {
                if (Inherit && ClassHierarchyFeatures != null)
                    return ClassHierarchyFeatures;
            }

            OOAdvantech.Collections.Generic.Set<Feature> features = new OOAdvantech.Collections.Generic.Set<Feature>();

            foreach (Feature mFeature in Features)
                features.Add(mFeature);

            if (Inherit == false)
                return new OOAdvantech.Collections.Generic.Set<Feature>(features);
            foreach (Generalization generalization in Generalizations)
                features.AddRange(generalization.Parent.GetFeatures(Inherit));

            lock (ClassHierarchyLock)
            {
                if (ClassHierarchyFeatures == null)
                    ClassHierarchyFeatures = new OOAdvantech.Collections.Generic.Set<Feature>(features, Collections.CollectionAccessType.ReadOnly);
                return ClassHierarchyFeatures;
            }
        }
        /// <MetaDataID>{E1E0C931-6F23-409A-9544-7B749035CE20}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Operation> GetOperations(bool Inherit)
        {
            return new OOAdvantech.Collections.Generic.Set<Operation>(GetFeatures(Inherit).OfType<Operation>().ToList());

            //ReaderWriterLock.AcquireReaderLock(10000);
            //try
            //{

            //    //TODO:Δεν είναι και πολύ καλλή ιδέα κυρίως στην περίπτωση που κάποιο καλέση την AddAssociation 
            //    //σε κάποια από τις supper classes
            //    if (ClassHierarchyOperations != null && Inherit)
            //        return ClassHierarchyOperations;

            //    OOAdvantech.Collections.Generic.Set<Operation> operations = new OOAdvantech.Collections.Generic.Set<Operation>();

            //    if (Inherit == true)
            //    {
            //        foreach (Generalization CurrGeneralization in Generalizations)
            //            operations.AddRange(CurrGeneralization.Parent.GetOperations(Inherit));
            //    }

            //    foreach (MetaDataRepository.Feature mFeature in Features)
            //        if (mFeature is Operation)
            //            operations.Add(mFeature as Operation);

            //    if (Inherit == true)
            //        ClassHierarchyOperations = operations;
            //    return operations;
            //}
            //finally
            //{
            //    ReaderWriterLock.ReleaseReaderLock();
            //}


        }



        /// <summary>This method retrieves the roles of the classifier and superclassifier and returns them in the specified objectcollection. </summary>
        /// <param name="Inherit">Specifies whether to search this member's inheritance chain to find the association end. </param>
        /// <MetaDataID>{0889F60D-0D73-4EF9-B1C6-CD1B1B0B5C58}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<AssociationEnd> GetRoles(bool Inherit)
        {

            if (Inherit == false)
                return Roles;

            lock (ClassHierarchyLock)
            {
                if (ClassHierarchyRoles != null && Inherit)
                    return ClassHierarchyRoles;
            }

            OOAdvantech.Collections.Generic.Set<AssociationEnd> roles = new Collections.Generic.Set<AssociationEnd>();

            foreach (AssociationEnd associationEnd in Roles)
                roles.Add(associationEnd);

            foreach (Generalization generalization in Generalizations)
                roles.AddRange(generalization.Parent.GetRoles(Inherit));

            lock (ClassHierarchyLock)
            {
                ClassHierarchyRoles = new OOAdvantech.Collections.Generic.Set<AssociationEnd>(roles, Collections.CollectionAccessType.ReadOnly);
                return ClassHierarchyRoles;
            }

            ////TODO:Δεν είναι και πολύ καλλή ιδέα κυρίως στην περίπτωση που κάποιο καλέση την AddAssociation 
            ////σε κάποια από τις supper classes
            //if (ClassHierarchyRoles != null && Inherit)
            //    return ClassHierarchyRoles;

            //OOAdvantech.Collections.Generic.Set<AssociationEnd> classHierarchyRoles = new OOAdvantech.Collections.Generic.Set<AssociationEnd>();
            //classHierarchyRoles.AddRange(Roles);
            //if (Inherit == false)
            //    return classHierarchyRoles;
            //foreach (Generalization generalization in Generalizations)
            //    classHierarchyRoles.AddRange(generalization.Parent.GetRoles(Inherit));

            //ClassHierarchyRoles = classHierarchyRoles;
            //return classHierarchyRoles;

        }

        /// <MetaDataID>{1c5d1f2a-d5b2-42e0-b2ba-770f77281396}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Attribute> GetRealizedAttributes()
        {

            OOAdvantech.Collections.Generic.Set<Attribute> attributes = new OOAdvantech.Collections.Generic.Set<Attribute>();
            foreach (MetaDataRepository.Feature mFeature in Features)
                if (mFeature is AttributeRealization)
                    attributes.Add((mFeature as AttributeRealization).Specification);
            return attributes;

        }
        /// <summary>
        /// Gets the classifier member 
        /// </summary>
        /// <param name="memberIdentity">
        /// Defines the member identity 
        /// </param>
        /// <returns>
        /// Returns the member as MetaObject
        /// </returns>
        /// <MetaDataID>{ead77f62-c15c-44bd-976d-a9a3c9e75a9d}</MetaDataID>
        public MetaObject GetMember(MetaObjectID memberIdentity)
        {
            MetaObject memberMetaObject = null;
            foreach (var attribute in GetAttributes(true))
            {
                if (attribute.Identity == memberIdentity)
                {
                    memberMetaObject = attribute;
                    break;
                }
            }
            if (memberMetaObject == null)
            {
                foreach (var associationEnd in GetAssociateRoles(true))
                {
                    if (associationEnd.Identity == memberIdentity)
                    {
                        memberMetaObject = associationEnd;
                        break;
                    }
                }
                if (memberMetaObject == null && ClassHierarchyLinkAssociation != null)
                {
                    if (ClassHierarchyLinkAssociation.RoleA.Identity == memberIdentity)
                        memberMetaObject = ClassHierarchyLinkAssociation.RoleA;

                    if (ClassHierarchyLinkAssociation.RoleB.Identity == memberIdentity)
                        memberMetaObject = ClassHierarchyLinkAssociation.RoleB;
                }
            }
            return memberMetaObject;
        }



        /// <summary>This method retrieves the collection of Attributes contained from classifier. </summary>
        /// <param name="Inherit">Specifies whether to search this member's inheritance chain to find the association end. </param>
        /// <MetaDataID>{F078E629-514C-4B83-BB6F-24A2EDA5DC4B}</MetaDataID>
        public virtual OOAdvantech.Collections.Generic.Set<Attribute> GetAttributes(bool Inherit)
        {
            return new OOAdvantech.Collections.Generic.Set<Attribute>(GetFeatures(Inherit).OfType<Attribute>().ToList());

        }

        /// <MetaDataID>{5e0ba341-0e62-40b4-9b41-4db350f3dd3c}</MetaDataID>
        protected readonly object SpecializationsLock = new object();

        /// <MetaDataID>{60d7d2a8-5731-4e2e-b4e7-89f33d7f7cc4}</MetaDataID>
        protected readonly object GeneralizationLock = new object();


        /// <MetaDataID>{4AC51B55-E213-4A67-B4DD-5E42A448F459}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<Generalization> _Specializations = new OOAdvantech.Collections.Generic.Set<Generalization>();
        /// <summary>Designates a Generalization whose child GeneralizableElement is the immediate descendent of the current GeneralizableElement. </summary>
        /// <MetaDataID>{18082BA6-8A54-45F5-A426-F0D398A9E2AE}</MetaDataID>
        [Association("Specialization", typeof(OOAdvantech.MetaDataRepository.Generalization), MetaDataRepository.Roles.RoleB, "{E591FA9E-B3D8-4699-B6FC-3BD5BB2EA4AC}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching)]
        [PersistentMember("_Specializations")]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.Set<Generalization> Specializations
        {

            get
            {

                return _Specializations.ToThreadSafeSet();

            }
        }
        /// <MetaDataID>{d936b27c-692b-408e-89f8-31a379bdfe18}</MetaDataID>
        protected readonly object RolesLock = new object();
        /// <MetaDataID>{A666B867-944C-443A-A26E-43D1D202169D}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<AssociationEnd> _Roles = new OOAdvantech.Collections.Generic.Set<AssociationEnd>();
        /// <summary>Inverse of specification on association to AssociationEnd. Denotes that the Classifier participates in an Association. </summary>
        /// <MetaDataID>{8CE92882-3C35-452D-B675-1FA3D8A899C9}</MetaDataID>
        [Association("ClassifierRole", typeof(OOAdvantech.MetaDataRepository.AssociationEnd), MetaDataRepository.Roles.RoleB, "{1213BEE4-382C-4EE0-A62B-E013FCA9B4C3}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.LazyFetching | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_Roles")]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.Set<AssociationEnd> Roles
        {

            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                        return new OOAdvantech.Collections.Generic.Set<AssociationEnd>(_Roles.ToThreadSafeSet().Where(role => role.Association != null && role.Association.Connections.Count >= 2).ToList());//, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                }
                catch (System.Exception)
                {
                    throw;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }


        /// <MetaDataID>{ec96548b-7b71-449f-858e-9a240d389294}</MetaDataID>
        protected readonly object FeaturesLock = new object();

        /// <MetaDataID>{308F7728-6E8D-480F-8B67-87E200F85EDB}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<Feature> _Features = new OOAdvantech.Collections.Generic.Set<Feature>();
        /// <summary>An ordered list of Features, like Attribute, Operation, Method owned by the Classifier. </summary>
        /// <MetaDataID>{29973E45-AD39-4942-9752-FE2A5FA87C53}</MetaDataID>
        [Association("ClassifierMember", MetaDataRepository.Roles.RoleA, "{38679851-A962-46C4-A940-20522E07D301}")]
        [PersistentMember("_Features")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [RoleAMultiplicityRange(0)]
        public virtual OOAdvantech.Collections.Generic.Set<Feature> Features
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Features.ToThreadSafeSet();
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }
        /// <MetaDataID>{3567ac6c-c45d-437d-a8bf-ee6103420bd9}</MetaDataID>
        public Collections.Generic.List<Operation> GetOperations(string oparationName)
        {
            return GetOperations(oparationName, true);
        }
        /// <MetaDataID>{948b464c-678b-4c3e-baf8-cfe37e8393a5}</MetaDataID>
        public virtual Collections.Generic.List<Operation> GetOperations(string oparationName, bool caseSensitive)
        {
            Collections.Generic.List<Operation> operations = new OOAdvantech.Collections.Generic.List<Operation>();
            //if (IsBindedClassifier)
            {
                foreach (Operation operation in GetOperations(true))
                {
                    if (operation.Name == oparationName || (!caseSensitive && operation.Name.ToLower() == oparationName.ToLower()))
                        operations.Add(operation);
                }
            }
            return operations;
        }
        /// <MetaDataID>{21ffadb3-696f-4f25-88c3-254b02bc90b2}</MetaDataID>
        public virtual Operation GetOperation(string oparationName, string[] parametersTypes, bool caseSensitive)
        {

            foreach (Operation operation in GetOperations(false))
            {
                if (operation.Name == oparationName || (!caseSensitive && operation.Name.ToLower() == oparationName.ToLower()))
                {
                    if (operation.Parameters.Count == parametersTypes.Length)
                    {
                        int i = 0;
                        bool equal = true;
                        foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
                        {
                            if (parameter.Type.FullName != parametersTypes[i++])
                            {
                                equal = false;
                                break;
                            }
                        }
                        if (equal)
                            return operation;
                    }
                }
            }
            foreach (Classifier classifier in GetGeneralClasifiers())
            {
                Operation operation = classifier.GetOperation(oparationName, parametersTypes, caseSensitive);
                if (operation != null)
                    return operation;
            }
            return null;


        }


        /// <MetaDataID>{21a232fa-7c6a-48c5-b526-da5e226bfbeb}</MetaDataID>
        public virtual AssociationEnd GetRole(string associationEndIdentity)
        {
            foreach (AssociationEnd associationEnd in Roles)
            {
                if (associationEnd.Identity.ToString() == associationEndIdentity)
                    return associationEnd;
            }
            return null;
        }

        /// <MetaDataID>{f1bc40ff-37fa-436a-96a8-8c8a1d31040d}</MetaDataID>
        public virtual Feature GetFeature(string identity, bool inherit)
        {
            foreach (Feature feature in GetFeatures(inherit))
            {
                if (feature.Identity.ToString() == identity)
                    return feature;
            }

            return null;
        }


        /// <MetaDataID>{7d69b696-af87-42eb-ae7a-35bf683947d1}</MetaDataID>
        internal Attribute GetAttribute(MetaObjectID attributeIdentity)
        {
            foreach (Attribute attribute in GetAttributes(true))
            {
                if (attribute.Identity == attributeIdentity)
                    return attribute;
            }
            return null;
        }

        /// <MetaDataID>{b2bcd2fd-f1b4-4691-91f5-791f109e9bfa}</MetaDataID>
        virtual public MetaObject GetMember(string memberName)
        {

            foreach (Feature feature in Features)
            {
                if (feature.Name == memberName)
                {
                    if (feature is AttributeRealization)
                        return (feature as AttributeRealization).Specification;
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
            return null;

        }
    }
}




