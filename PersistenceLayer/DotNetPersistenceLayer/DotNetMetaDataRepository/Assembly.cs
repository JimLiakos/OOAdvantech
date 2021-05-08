
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using OOAdvantech.MetaDataRepository;

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{E2B5EB16-BE9B-489E-8DBA-4F2731432826}</MetaDataID>
    public class Assembly : MetaDataRepository.Component
    {
        public override ObjectMemberGetSet SetMemberValue(object token, MemberInfo member, object value)
        {
            if (member.Name == nameof(DynamicBindingResidents))
            {
                if (value == null)
                    DynamicBindingResidents = default(System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>);
                else
                    DynamicBindingResidents = (System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(ResidentsLoaded))
            {
                if (value == null)
                    ResidentsLoaded = default(bool);
                else
                    ResidentsLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(AssembliesBackwardCompatibilityIDs))
            {
                if (value == null)
                    AssembliesBackwardCompatibilityIDs = default(System.Collections.Generic.Dictionary<System.Reflection.Assembly, OOAdvantech.MetaDataRepository.BackwardCompatibilityID>);
                else
                    AssembliesBackwardCompatibilityIDs = (System.Collections.Generic.Dictionary<System.Reflection.Assembly, OOAdvantech.MetaDataRepository.BackwardCompatibilityID>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(WrAssembly))
            {
                if (value == null)
                    WrAssembly = default(System.Reflection.Assembly);
                else
                    WrAssembly = (System.Reflection.Assembly)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, MemberInfo member)
        {

            if (member.Name == nameof(DynamicBindingResidents))
                return DynamicBindingResidents;

            if (member.Name == nameof(ResidentsLoaded))
                return ResidentsLoaded;

            if (member.Name == nameof(AssembliesBackwardCompatibilityIDs))
                return AssembliesBackwardCompatibilityIDs;

            if (member.Name == nameof(WrAssembly))
                return WrAssembly;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{bb55f511-30fe-44b4-aaf8-0a86ac8d1316}</MetaDataID>
        public override string FullName
        {
            get
            {
                return WrAssembly.FullName;
            }
        }

        /// <MetaDataID>{bf005d1a-7ffc-4af9-af0f-71c38ab8852d}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        public override void ShallowSynchronize(MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{8a29ad38-211a-40ca-b388-b4e999ae92a5}</MetaDataID>
        public static OOAdvantech.MetaDataRepository.Classifier FindClassifier(string fullName, string assemblyName, bool caseSensitive, System.Reflection.Assembly dotNetAssembly)
        {

            System.Type type = null;
            if (dotNetAssembly != null && !string.IsNullOrEmpty(assemblyName))
            {
                OOAdvantech.DotNetMetaDataRepository.Assembly assembly = Assembly.GetComponent(dotNetAssembly) as OOAdvantech.DotNetMetaDataRepository.Assembly;

                if (assembly.Name == assemblyName ||
                    (assemblyName != null && !caseSensitive && assembly.FullName.ToLower() == assemblyName.ToLower()) ||
                    (assemblyName != null && assembly.FullName == assemblyName))
                {
                    type = dotNetAssembly.GetType(fullName, false, !caseSensitive);
                    if (type == null)
                        return null;
                    else
                        return OOAdvantech.MetaDataRepository.Classifier.GetClassifier(type);
                }
                else
                {
                    foreach (OOAdvantech.MetaDataRepository.Dependency dependency in assembly.ClientDependencies)
                    {
                        OOAdvantech.DotNetMetaDataRepository.Assembly referenceAssembly = dependency.Supplier as OOAdvantech.DotNetMetaDataRepository.Assembly;
                        if (referenceAssembly.Name == assemblyName)
                        {
                            type = referenceAssembly.WrAssembly.GetType(fullName, false, !caseSensitive);
                            if (type == null)
                                return null;
                            else
                                return OOAdvantech.MetaDataRepository.Classifier.GetClassifier(type);
                        }
                    }
                }
            }
            try
            {
                type = ModulePublisher.ClassRepository.GetType(fullName, assemblyName);
            }
            catch (System.Exception error)
            {
            }
            if (type == null)
                return null;
            Assembly assemply = MetaObjectMapper.FindMetaObjectFor(type.GetMetaData().Assembly) as Assembly;
            if (assemply == null)
                assemply = new Assembly(type.GetMetaData().Assembly);
            return assemply.GetClassifier(fullName, true);

        }

        //public static OOAdvantech.MetaDataRepository.Classifier GetClassifierForType(System.Type type)
        //{
        //    if (type == null)
        //        return null;
        //    Assembly assemply = MetaObjectMapper.FindMetaObjectFor(type.Assembly) as Assembly;
        //    if (assemply == null)
        //        assemply = new Assembly(type.Assembly);
        //    long count = assemply.Residents.Count;

        //    MetaDataRepository.Classifier classifier = MetaObjectMapper.FindMetaObjectFor(type) as MetaDataRepository.Classifier;
        //    if (classifier == null)
        //        classifier = Type.CreateClassifierObject(type);
        //    return classifier;
        //}
        /// <MetaDataID>{72010222-E83B-4B71-96A7-222E9268066F}</MetaDataID>
        public override void AddOwnedElement(OOAdvantech.MetaDataRepository.MetaObject ownedElement)
        {
            base.AddOwnedElement(ownedElement);
        }
        /// <MetaDataID>{e52783d9-858e-4c10-8fd1-89abc7455db2}</MetaDataID>
        public override void AddResident(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            if (metaObject == null)
                return;
            DynamicBindingResidents[metaObject.FullName] = metaObject;
        }
        /// <MetaDataID>{9ff25d4a-9602-4e05-8e73-40a11d3174f5}</MetaDataID>
        public override void RemoveResident(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            if (metaObject == null)
                return;
            if (DynamicBindingResidents.ContainsKey(metaObject.FullName))
                DynamicBindingResidents.Remove(metaObject.FullName);


        }
        /// <MetaDataID>{96773ad9-1c46-4bae-8ab8-d9beb22b8972}</MetaDataID>
        System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject> DynamicBindingResidents = new System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>();

        /// <MetaDataID>{DB064570-A25A-4B46-B0B5-C8D8EA2BA7A6}</MetaDataID>
        public override MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;
            }
        }
        //Collections.Generic.Dictionary<string, MetaDataRepository.Classifier> NamedClassifiers;
        //Collections.Generic.Dictionary<string, MetaDataRepository.Classifier> CaseInSensitiveNamedClassifiers;

        /// <MetaDataID>{dda60486-1b4d-4827-b5c9-20170b919f3d}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier GetClassifier(string fullName, bool caseSensitive)
        {
            //TODO δεν δουλεύει το caseSensitive
            if (DynamicBindingResidents.ContainsKey(fullName))
                return DynamicBindingResidents[fullName] as MetaDataRepository.Classifier;
            System.Type type = null;
            try
            {
#if !DeviceDotNet
                type = WrAssembly.GetType(fullName, false, !caseSensitive);
#else
                type = WrAssembly.GetType(fullName);
#endif
            }
            catch (System.Exception error)
            {
                return null;
            }
            if (type == null)
                return null;
            else
                return MetaDataRepository.Classifier.GetClassifier(type);
        }
        public static int count;
        /// <MetaDataID>{5ae18675-9868-449f-8cc7-516d0ea7a8d4}</MetaDataID>
        static void InitObjectMembers(object _object, AccessorBuilder.TypeMetadata typeMetadata)
        {
            if (typeMetadata.Fields != null && typeMetadata.Fields.Length > 0)
            {
                System.Type type = typeMetadata.Fields[0].FieldInfo.DeclaringType;



                if (!typeMetadata.ExtensionMetadataLoaded)
                {

                    count++;
                    Class _class = GetClassifier(type) as Class;
                    // System.Diagnostics.Debug.WriteLine(_class.FullName);
                    lock (type)
                    {

                        System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, MetaDataRepository.MetaObject> extensionFiledInfo = new System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, OOAdvantech.MetaDataRepository.MetaObject>();

                        foreach (MetaDataRepository.Feature feature in _class.Features)
                        {
                            //TODO το field μπορεί να είναι σε attribute και assosiationend ταφτόχρονα.
                            if (feature is Attribute && (feature as Attribute).FieldMember != null)
                                extensionFiledInfo[(feature as Attribute).FieldMember] = feature;
                            if (feature is AttributeRealization && (feature as AttributeRealization).FieldMember != null)
                                extensionFiledInfo[(feature as AttributeRealization).FieldMember] = feature;
                            if (feature is AssociationEndRealization && (feature as AssociationEndRealization).FieldMember != null)
                                extensionFiledInfo[(feature as AssociationEndRealization).FieldMember] = feature;

                        }
                        foreach (AssociationEnd associationEnd in _class.GetAssociateRoles(false))
                        {
                            try
                            {
                                if (associationEnd.FieldMember != null)
                                    extensionFiledInfo[associationEnd.FieldMember] = associationEnd;
                            }
                            catch (System.Exception error)
                            {

                            }
                        }

                        foreach (AssociationEndRealization associationEndRealization in _class.Features.OfType<AssociationEndRealization>())
                        {
                            if (associationEndRealization.FieldMember != null)
                                extensionFiledInfo[associationEndRealization.FieldMember] = associationEndRealization.Specification as AssociationEnd;
                        }


                        for (int i = 0; i < typeMetadata.Fields.Length; i++)
                        {
                            AccessorBuilder.FieldMetadata fieldMetadata = typeMetadata.Fields[i];
                            if (!typeMetadata.ExtensionMetadataLoaded && fieldMetadata.FieldAccessor.InitializationRequired)
                            {
                                extensionFiledInfo.TryGetValue(fieldMetadata.FieldInfo, out fieldMetadata.ExtensionMetadata);
                                typeMetadata.Fields[i] = fieldMetadata;
                            }
                        }

                        typeMetadata.ExtensionMetadataLoaded = true;
                        AccessorBuilder.SetTypeMetadata(type, typeMetadata);
                    }

                }
                //return;

                //lock (type)
                //{
                //    if (!typeMetadata.ExtensionMetadataLoaded)
                //    {
                //        for (int i = 0; i < typeMetadata.Fields.Length; i++)
                //        {
                //            AccessorBuilder.FieldMetadata fieldMetadata = typeMetadata.Fields[i];
                //            if (!typeMetadata.ExtensionMetadataLoaded && fieldMetadata.FieldAccessor.InitializationRequired)
                //            {
                //                extensionFiledInfo.TryGetValue(fieldMetadata.FieldInfo, out fieldMetadata.ExtensionMetadata);
                //                typeMetadata.Fields[i] = fieldMetadata;
                //            }
                //        }
                //        typeMetadata.ExtensionMetadataLoaded = true;
                //        AccessorBuilder.SetTypeMetadata(type, typeMetadata);
                //    }

                //}


                foreach (AccessorBuilder.FieldMetadata fieldMetadata in typeMetadata.Fields)
                {
                    if (fieldMetadata.FieldAccessor.InitializationRequired)
                    {
                        OOAdvantech.IMemberInitialization memberInitialization = fieldMetadata.GetValue(_object) as OOAdvantech.IMemberInitialization;
                        try
                        {
                            if (memberInitialization == null)
                                memberInitialization = AccessorBuilder.CreateInstance(fieldMetadata.FieldInfo.FieldType) as OOAdvantech.IMemberInitialization;
                        }
                        catch (System.Exception error)
                        {
                            continue;
                        }
                        if (memberInitialization == null)
                            continue;

                        memberInitialization.SetOwner(_object);
                        memberInitialization.SetMetadata(fieldMetadata.ExtensionMetadata);
                    }
                }
            }
            if (typeMetadata.BaseTypeMetadata != null)
                InitObjectMembers(_object, (AccessorBuilder.TypeMetadata)typeMetadata.BaseTypeMetadata);
        }
        /// <MetaDataID>{cab4b3cc-803b-4707-8742-0e2f24f19859}</MetaDataID>
        public static void InitObject(object _object, object typeMetadata)
        {

            OOAdvantech.ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(_object);
            if (extensionProperties != null && extensionProperties.ObjectMembersInitialized)
                return;

            AccessorBuilder.TypeMetadata m_typeMetada;

            System.Type type = _object.GetType();
            if (typeMetadata != null)
                m_typeMetada = (AccessorBuilder.TypeMetadata)typeMetadata;
            else
                m_typeMetada = AccessorBuilder.LoadTypeMetadata(_object.GetType());



            if (m_typeMetada.InitializationRequired)
                InitObjectMembers(_object, m_typeMetada);

            if (extensionProperties != null)
                extensionProperties.ObjectMembersInitialized = true;



        }
        /// <MetaDataID>{fa10bf05-3f3c-4391-9b38-0498e57438dd}</MetaDataID>
        static public MetaDataRepository.Component GetComponent(System.Reflection.Assembly assembly)
        {
            lock (assembly)
            {
                Assembly component = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(assembly) as DotNetMetaDataRepository.Assembly;
                if (component == null)
                    component = new DotNetMetaDataRepository.Assembly(assembly);
                return component;
            }
        }
        /// <MetaDataID>{06d9c429-401c-4824-ac2e-6cd2e07b77b3}</MetaDataID>
        static public MetaDataRepository.Classifier GetClassifier(System.Type type)
        {
            if (type == null)
                return null;
            else
            {
                //Assembly assemply = MetaObjectMapper.FindMetaObjectFor(type.Assembly) as Assembly;
                //if (assemply == null)
                //    assemply = new Assembly(type.Assembly);
                //long count = assemply.Residents.Count;
                return Type.GetClassifierObject(type);
            }


        }

        /// <MetaDataID>{84519540-82C2-4E56-9109-A2BC76656237}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Dependency> ClientDependencies
        {
            get
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_ClientDependencies.Count == 0)
                    {
                        System.Collections.Generic.List<System.Reflection.AssemblyName> ReferencedAssemblies = new System.Collections.Generic.List<System.Reflection.AssemblyName>();
#if !DeviceDotNet
                        ReferencedAssemblies.AddRange(WrAssembly.GetReferencedAssemblies());
                        foreach (System.Reflection.AssemblyName RefAssemblyName in ReferencedAssemblies)
                        {
                            try
                            {

                                System.Reflection.Assembly RefAssembly = null;

                                foreach (System.Reflection.Assembly CurrAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
                                {
                                    if (CurrAssembly.FullName == RefAssemblyName.FullName)
                                        RefAssembly = CurrAssembly;
                                }

                                if (RefAssembly == null)
                                    RefAssembly = System.Reflection.Assembly.Load(RefAssemblyName);

                                Assembly supplier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(RefAssembly) as Assembly;
                                if (supplier == null)
                                    supplier = new Assembly(RefAssembly);
                                AddDependency("", supplier);

                            }
                            catch (System.Exception Error)
                            {
                                throw new System.Exception(Error.Message, Error);
                                int lo = 0;
                            }

                        }

#else
                        System.Collections.Generic.List<System.Reflection.Assembly> referencedAssemblies = GetReferencedAssembliesFrom(WrAssembly);

                        ModulePublisher.AssemblyReferences references=null;
                        foreach (object customAttribute in WrAssembly.GetCustomAttributes(false))
                        {
                            if (customAttribute is ModulePublisher.AssemblyReferences)
                            {
                                references = customAttribute as ModulePublisher.AssemblyReferences;
                                break;
                            }
                        }
                        if (references != null)
                        {
                            foreach (string assemblyName in references.References)
                            {

                                System.Reflection.Assembly refAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName( assemblyName));
                                referencedAssemblies.Add(refAssembly);
                            }
                        }

                        foreach(var refAssembly in referencedAssemblies)
                        {
                            Assembly supplier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(refAssembly) as Assembly;
                            if (supplier == null)
                                supplier = new Assembly(refAssembly);
                            AddDependency("", supplier);

                        }

#endif

                        //System.Reflection.Assembly.Load(
                        //WrAssembly.GetReferencedAssemblies()
                    }
                    Collections.Generic.Set<MetaDataRepository.Dependency> dependencyCollection = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Dependency>(_ClientDependencies, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                    return dependencyCollection;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }


            }
        }

#if DeviceDotNet
        private List<System.Reflection.Assembly> GetReferencedAssembliesFrom(System.Reflection.Assembly wrAssembly)
        {
            List<System.Reflection.Assembly> referenceAssemblies =new List<System.Reflection.Assembly>();
            foreach (var type  in wrAssembly.DefinedTypes)
            {
                foreach (var referenceAssembly in GetReferencedAssembliesFromType(type.AsType()))
                {
                    if (referenceAssembly != wrAssembly && !referenceAssemblies.Contains(referenceAssembly))
                        referenceAssemblies.Add(referenceAssembly);
                }
            }

            return referenceAssemblies;
        }

        private List<System.Reflection.Assembly> GetReferencedAssembliesFromType(System.Type type)
        {
            List<System.Reflection.Assembly> referenceAssemblies = new List<System.Reflection.Assembly>();
            if (type.GetMetaData().BaseType != null)
            {
                var baseTypeAssembly = type.GetMetaData().BaseType.GetTypeInfo().Assembly;
                if (baseTypeAssembly != type.GetMetaData().Assembly)
                    referenceAssemblies.Add(baseTypeAssembly);
            }
            foreach(System.Type _interface in type.GetTypeInfo().ImplementedInterfaces)
            {
                var baseTypeAssembly = _interface.GetTypeInfo().Assembly;
                if (baseTypeAssembly != type.GetMetaData().Assembly)
                    referenceAssemblies.Add(baseTypeAssembly);

            }

            foreach (var fieldType in  (from fieldInfo in type.GetTypeInfo().DeclaredFields
                select fieldInfo.FieldType))
            {

                var fieldTypeAssembly = fieldType.GetTypeInfo().Assembly;
                if (fieldTypeAssembly != type.GetMetaData().Assembly&& referenceAssemblies.Contains(fieldTypeAssembly))
                    referenceAssemblies.Add(fieldTypeAssembly);
            }

            foreach (var propertyType in (from propertyInfo in type.GetTypeInfo().DeclaredProperties
                                       select propertyInfo.PropertyType))
            {

                var propertyTypeAssembly = propertyType.GetTypeInfo().Assembly;
                if (propertyTypeAssembly != type.GetMetaData().Assembly&& !referenceAssemblies.Contains(propertyTypeAssembly))
                    referenceAssemblies.Add(propertyTypeAssembly);
            }


            foreach (var methodInfo in type.GetTypeInfo().DeclaredMethods)
            {

                var methodReturnTypeAssembly = methodInfo.ReturnType.GetTypeInfo().Assembly;
                if (methodReturnTypeAssembly != type.GetMetaData().Assembly && !referenceAssemblies.Contains(methodReturnTypeAssembly))
                    referenceAssemblies.Add(methodReturnTypeAssembly);


                foreach (var parameterType in (from parameterInfo in methodInfo.GetParameters()
                                              select parameterInfo.ParameterType))
                {
                    var propertyTypeAssembly = parameterType.GetTypeInfo().Assembly;
                    if (propertyTypeAssembly != type.GetMetaData().Assembly && !referenceAssemblies.Contains(propertyTypeAssembly))
                        referenceAssemblies.Add(propertyTypeAssembly);
                }
            }


            return referenceAssemblies;

        }
#endif

        /// <MetaDataID>{D4A1B641-3AA5-46D1-9E06-C8C2815C6CE2}</MetaDataID>
        bool ResidentsLoaded = false;
        /// <MetaDataID>{0397760F-A4B5-4DE2-BE70-43B879BCAD59}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.MetaObject> Residents
        {

            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (!ResidentsLoaded)
                        LoadResidents();

                    Collections.Generic.Set<MetaDataRepository.MetaObject> ReturnCollection = null;
                    if (DynamicBindingResidents.Count == 0)
                        ReturnCollection = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>(_Residents, OOAdvantech.Collections.CollectionAccessType.ReadOnly);
                    else
                    {
                        ReturnCollection = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>(_Residents);
                        foreach (System.Collections.Generic.KeyValuePair<string, MetaDataRepository.MetaObject> entry in DynamicBindingResidents)
                            ReturnCollection.Add(entry.Value);

                    }
                    return ReturnCollection;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{4E554A44-B9C1-4BE3-BC04-C8F094AA35DB}</MetaDataID>
        public override bool HasPersistentClasses
        {
            get
            {
                //Error prone 
                if (WrAssembly == null || WrAssembly.GetType(typeof(OOAdvantech.MetaDataRepository.Component).FullName) != null)
                    return false;

                LoadResidents();

                foreach (MetaDataRepository.MetaObject metaObject in Residents)
                {
                    if (metaObject is Class && (metaObject as Class).Persistent)
                        return true;

                }
                return false;
            }
        }


        /// <MetaDataID>{4EC6A395-58A8-410D-8144-CEFA2F1C8618}</MetaDataID>
        protected Assembly()
        {
        }

        /// <MetaDataID>{1A720964-DADF-45C1-9260-50E67F45DF81}</MetaDataID>
        ~Assembly()
        {
            int k = 0;
        }

        /// <MetaDataID>{30A9469D-D0C8-4665-AAB5-A1D0298A4684}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {

            object[] objects = WrAssembly.GetCustomAttributes(typeof(MetaDataRepository.BuildAssemblyMetadata), false);
            if (objects.Length != 0)
            {
                if (!ResidentsLoaded)
                    LoadResidents();
            }
            return base.ErrorCheck(ref errors);
        }

        /// <MetaDataID>{932389da-4df0-4795-837b-4234322343f6}</MetaDataID>
        static System.Collections.Generic.Dictionary<System.Reflection.Assembly, OOAdvantech.MetaDataRepository.BackwardCompatibilityID> AssembliesBackwardCompatibilityIDs = new System.Collections.Generic.Dictionary<System.Reflection.Assembly, OOAdvantech.MetaDataRepository.BackwardCompatibilityID>();
        /// <MetaDataID>{3a5af45a-9913-4bb1-9c25-fa3ce4c083a7}</MetaDataID>
        static internal OOAdvantech.MetaDataRepository.BackwardCompatibilityID GetBackwardCompatibilityID(System.Reflection.Assembly dotNetAssembly)
        {
            OOAdvantech.MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = null;

            try
            {
                if (!AssembliesBackwardCompatibilityIDs.TryGetValue(dotNetAssembly, out backwardCompatibilityID))
                {
                    object[] customAttributes = dotNetAssembly.GetCustomAttributes(true);
                    foreach (object customAttribute in customAttributes)
                    {
                        if (customAttribute.GetType().FullName == "OOAdvantech.MetaDataRepository.BackwardCompatibilityID")
                        {
                            backwardCompatibilityID = new OOAdvantech.MetaDataRepository.BackwardCompatibilityID(customAttribute.ToString());
                            break;
                        }
                    }
                    AssembliesBackwardCompatibilityIDs[dotNetAssembly] = backwardCompatibilityID;
                }
            }
            catch (System.Exception error)
            {
            }
            return backwardCompatibilityID;
        }
        /// <MetaDataID>{9c1d8845-23b2-4fc3-9a21-1a337e2239a7}</MetaDataID>
        public static string GetIdentity(System.Reflection.Assembly dotNetAssembly)
        {
            OOAdvantech.MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = GetBackwardCompatibilityID(dotNetAssembly);
            if (backwardCompatibilityID != null)
                return backwardCompatibilityID.ToString();
            else
                return dotNetAssembly.FullName;

        }
        /// <MetaDataID>{64F2E67F-0C69-4E80-8A12-7E68CC0172E4}</MetaDataID>
       public  Assembly(System.Reflection.Assembly dotNetAssembly)
        {
            BuildAssemblyMetadata buildAssemblyMetadata = null;
            try
            {
                buildAssemblyMetadata = dotNetAssembly.GetCustomAttributes(typeof(BuildAssemblyMetadata), false).OfType<BuildAssemblyMetadata>().FirstOrDefault();
            }
            catch (Exception error)
            {
            }
            if (buildAssemblyMetadata != null)
                _MappingVersion = buildAssemblyMetadata.MappingVersion;

            //lock (Type.LoadDotnetMetadataLock)
            {
                try
                {
                    _Name = dotNetAssembly.GetName().Name;


                    this._AssemblyString = dotNetAssembly.GetName().FullName;
                    WrAssembly = dotNetAssembly;

                    PutPropertyValue("MetaData", "AssemblyFullName", dotNetAssembly.FullName);


                    OOAdvantech.MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = GetBackwardCompatibilityID(dotNetAssembly);
                    if (backwardCompatibilityID != null)
                        _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID.ToString());
                    else
                        _Identity = new MetaDataRepository.MetaObjectID(WrAssembly.FullName);

                    if (DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(WrAssembly) != null)
                        throw new System.Exception("MetaObject for " + WrAssembly.FullName + " already exist");
                    DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(WrAssembly, this);


                    MetaObjectMapper.AddMetaObject(this, WrAssembly.FullName);
                }
                catch (System.Exception Error)
                {

                    if (WrAssembly != null)
                    {
                        DotNetMetaDataRepository.MetaObjectMapper.RemoveType(WrAssembly);
                        foreach (System.Type type in WrAssembly.GetTypes())
                            DotNetMetaDataRepository.MetaObjectMapper.RemoveType(type);
                    }
                    throw new System.Exception(Error.Message, Error);
                }
            }
        }
        ///<summary>
        ///This member defines the wrapped .net Assemply.
        ///The Assemply in metadata repository and pump information from wrapped .net Assemply,
        ///to build meta objects in metadata repository context.
        ///</summary>
        /// <MetaDataID>{978951BF-4621-4BB9-88D5-0CFB89548106}</MetaDataID>
        public System.Reflection.Assembly WrAssembly;
        /// <MetaDataID>{B4A99CF1-304F-4E9A-95AA-1F4463325BFB}</MetaDataID>
        /// <summary>This method takes the types of assembly construct the corresponding .net meta data repository wrapper class (Class,Interface,Structure etc) and load the Residents collection. </summary>
        private void LoadResidents()
        {
            lock (Type.LoadDotnetMetadataLock)
            {
                //OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
                //try
                //{
                if (!ResidentsLoaded)
                {

                    if (_Residents == null)
                        _Residents = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>();

                    using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                    {

                        foreach (MetaDataRepository.Classifier classifier in Type.GetClassifierObjects(WrAssembly.GetTypes()))
                            _Residents.Add(classifier);

                        ResidentsLoaded = true;
                        stateTransition.Consistent = true;
                    }

                }
                //}
                //finally
                //{
                //    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                //}
            }
        }

    }
}
