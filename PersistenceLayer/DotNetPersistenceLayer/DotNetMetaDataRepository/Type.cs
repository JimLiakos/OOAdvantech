using OOAdvantech.Transactions;
using OOAdvantech;
using System.Linq;
using System;
#if PORTABLE
using System.PCL.Reflection;
#else
using System.Reflection;

#endif

namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{931BC991-203C-442B-9294-374E8BBADEF6}</MetaDataID>
    /// <summary>Type is a class which the main job is to translate the meta data from the .net reflection point of view to the meta data repository point of view. </summary>
    public class Type
    {
        /// <MetaDataID>{02a641aa-79ca-4fd5-9437-a128fc1f4b9b}</MetaDataID>
        public static string LoadDotnetMetadataLock = "LoadDotnetMetadataLock";
        /// <MetaDataID>{C27B9743-7A8B-4C9E-8DBF-796CD7FAA288}</MetaDataID>
        protected Type()
        {
        }
        /// <MetaDataID>{4787706B-3EE0-4DB3-996F-F8D3F28103E2}</MetaDataID>
        //private OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock=new OOAdvantech.Synchronization.ReaderWriterLock();

        /// <summary>This attribute specifies the visibility of the type from the viewpoint of its container.
        /// Possibilities are:
        /// AccessPublic - Client can access the type.
        /// AccessPrivate - Only the Container can access the type.
        /// AccessComponent – Code from the same assembly can access the type. </summary>
        /// <MetaDataID>{0819A993-58CA-4CEE-9666-EB502F528F9B}</MetaDataID>
        internal OOAdvantech.MetaDataRepository.VisibilityKind Visibility;
        /// <MetaDataID>{5450FAE7-18C5-4FDA-9666-E45185052AAC}</MetaDataID>
        /// <summary>Define the name of type </summary>
        internal string Name
        {
            get
            {
                return WrType.Name;
            }
        }
        /// <MetaDataID>{fe654513-f316-4261-81af-91b21b5c3b2f}</MetaDataID>
        internal bool IsGenericTypeDefinition
        {
            get
            {

                return WrType.GetMetaData().IsGenericTypeDefinition;
            }
        }
        /// <MetaDataID>{845c717a-04af-448b-99f8-4bb0c6bc7d23}</MetaDataID>
        System.Collections.Generic.List<MetaDataRepository.TemplateParameter> _TemplateParameters;
        /// <MetaDataID>{876a8778-f885-4e98-b8e2-5f17d48e975e}</MetaDataID>
        internal System.Collections.Generic.List<MetaDataRepository.TemplateParameter> TemplateParameters
        {
            get
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if (_TemplateParameters == null)
                    {
                        _TemplateParameters = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.TemplateParameter>();
                        foreach (System.Type parameter in WrType.GetMetaData().GetGenericArguments())
                        {
                            MetaDataRepository.TemplateParameter templateParameter = MetaObjectMapper.FindMetaObjectFor(parameter) as MetaDataRepository.TemplateParameter;
                            if (templateParameter == null)
                            {
                                templateParameter = new TemplateParameter(parameter.Name);
                                MetaObjectMapper.AddTypeMap(parameter, templateParameter);
                            }
                            _TemplateParameters.Add(templateParameter);
                        }
                    }
                    return _TemplateParameters;
                    stateTransition.Consistent = true;
                }

            }
        }

        /// <summary>This method search on class hierarchy for the operation data of this method.
        /// The operation is something like the declaration and method the implementation. </summary>
        /// <param name="method">.net metada for the method. 
        /// The System.Reflection.MethodInfo notion 
        /// isn’t equivalent with OOAdvantech.MetaDataRepository.Method. </param>
        /// <MetaDataID>{8BF373ED-E38C-4E16-A7D6-DAF8E61D502C}</MetaDataID>
        internal System.Reflection.MethodInfo GetOperationForMethod(System.Reflection.MethodInfo method)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                if (method.IsAbstract)
                    return method;
                if (!method.IsVirtual)
                    return method;
                if (method.IsConstructor)
                    return method;

                #region Search in interfaces for the operation
                foreach (System.Type CurrInterface in method.DeclaringType.GetMetaData().GetInterfaces())
                {
                    foreach (System.Reflection.MethodInfo CurrMethod in
                            CurrInterface.GetMetaData().GetMethods(BindingFlags.Public
                                                    | BindingFlags.NonPublic
                                                    | BindingFlags.Instance
                                                    | BindingFlags.DeclaredOnly))
                    {
                        //TODO Στο .net μπορεί να υπάρχει operation με το ιδιο signature σε ένα ή περισσότερα interface 
                        //στην ιεραρχία άρα η method υλοποιεί πάνω από μία interafce operation. αυτή την κατάσταση την αγνοεί
                        //το DotnetMetaDataRepository
                        if (IsMethodsEqual(method, CurrMethod))
                            return CurrMethod;
                    }

                }
                #endregion

                #region Search in super classes for the operation

                System.Collections.Generic.List<System.Type> GeneralizationTypes = new System.Collections.Generic.List<System.Type>();
                System.Type GeneralizationType = method.DeclaringType.GetMetaData().BaseType;
                while (GeneralizationType != null)
                {
                    GeneralizationTypes.Add(GeneralizationType);
                    GeneralizationType = GeneralizationType.GetMetaData().BaseType;
                }
                //Search hierarchy from up to down
                for (int i = GeneralizationTypes.Count - 1; i >= 0; i--)
                {
                    foreach (System.Reflection.MethodInfo CurrMethod in ((System.Type)GeneralizationTypes[i]).GetMetaData().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        if (IsMethodsEqual(method, CurrMethod))
                        {
                            if (CurrMethod.IsAbstract || CurrMethod.IsVirtual)
                                return CurrMethod;
                        }
                    }
                }
                #endregion

                return method;
                stateTransition.Consistent = true;
            }

        }
        /// <summary>Check if the two methods has the same signature </summary>
        /// <MetaDataID>{3E619032-270C-4CE7-A841-4122056C1E5E}</MetaDataID>
        private static bool IsMethodsEqual(System.Reflection.MethodInfo FirstMethod, System.Reflection.MethodInfo SecondMethod)
        {

            if (FirstMethod.Name != SecondMethod.Name)
                return false;
            if (FirstMethod.ReturnType != SecondMethod.ReturnType)
                return false;
            System.Reflection.ParameterInfo[] FirstMethodParameters = FirstMethod.GetParameters();
            System.Reflection.ParameterInfo[] SecondMethodParameters = SecondMethod.GetParameters();
            if (FirstMethodParameters.Length != SecondMethodParameters.Length)
                return false;
            for (long i = 0; i < FirstMethodParameters.Length; i++)
            {
                if (FirstMethodParameters[i].ParameterType != SecondMethodParameters[i].ParameterType)
                    return false;
            }
            return true;
        }
        /// <MetaDataID>{83D59FB5-9D39-4824-B318-924C4FCBB79C}</MetaDataID>
        private static bool IsImplementationMethod(System.Type interafceType, System.Reflection.MethodInfo interafceMethod, System.Reflection.MethodInfo classMethod, out bool explicitly)
        {
            explicitly = false;
            if (classMethod.ContainsGenericParameters)
                return false;
            if (interafceMethod.ReturnType != classMethod.ReturnType)
                return false;


            if (interafceMethod.Name != classMethod.Name)
            {
                if (interafceType.FullName + "." + interafceMethod.Name != classMethod.Name)
                {
                    return false;
                }
                explicitly = true;
            }
            System.Reflection.ParameterInfo[] FirstMethodParameters = interafceMethod.GetParameters();
            System.Reflection.ParameterInfo[] SecondMethodParameters = classMethod.GetParameters();
            if (FirstMethodParameters.Length != SecondMethodParameters.Length)
            {
                explicitly = false;
                return false;
            }
            for (long i = 0; i < FirstMethodParameters.Length; i++)
            {
                if (FirstMethodParameters[i].ParameterType != SecondMethodParameters[i].ParameterType)
                {
                    explicitly = false;
                    return false;
                }
            }
            return true;
        }


        /// <MetaDataID>{534E815B-323F-43A4-B251-E7A3724CF516}</MetaDataID>
        internal System.Reflection.MethodInfo[] DotNetMethods;




        /// <MetaDataID>{f02691a8-7131-4154-a183-70efb218625c}</MetaDataID>
        public static System.DateTime StartTime = System.DateTime.MinValue;
        /// <MetaDataID>{4db97f58-7f25-48cd-b625-0709c3622e29}</MetaDataID>
        public static System.TimeSpan timeSpan;
        /// <MetaDataID>{d6b1470c-4d53-44a2-9855-bec072ee2cfd}</MetaDataID>
        bool FeaturesLoaded;
        /// <summary>Translate the members of type in features and return a collaction with them. </summary>
        /// <remarks>
        /// Features divided in tow categories the structural and behavioral. 
        /// The structural correspond to the properties and fields 
        /// and behavioral correspond to the methods.
        /// From the structural features excluded the properties and fields, 
        /// which are marked as association ends.
        /// </remarks>
        /// <param name="typeClassifier">This parameter defines the classifier of features. 
        /// It is useful because some features has constructor 
        /// with the classifier as parameter </param>
        /// <returns>A collection with features </returns>
        /// <MetaDataID>{92D74456-8431-4929-AA33-85F9D4491BC4}</MetaDataID>
        internal void GetFeatures(MetaDataRepository.Classifier typeClassifier, ref OOAdvantech.Collections.Generic.Set<MetaDataRepository.Feature> features)
        {
            //   System.Diagnostics.Debug.WriteLine(WrType.Namespace + "." + WrType.Name);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                //lock (WrType)
                {
                    if (FeaturesLoaded)
                        return;

                    System.DateTime start = System.DateTime.Now;
                    features = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Feature>();

                    #region Load behavioral feature, operations and methods
#if !DeviceDotNet
#endif
                    DotNetMethods = WrType.GetMetaData().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
                    foreach (System.Reflection.MethodInfo methodInfo in DotNetMethods)
                    {
                        if (methodInfo.DeclaringType != WrType)
                            continue;
                        if (methodInfo.IsSpecialName)
                            continue;
                        System.Reflection.MethodInfo abstractMethodInfo = GetOperationForMethod(methodInfo);
                        if (WrType != abstractMethodInfo.DeclaringType)
                        {
                            var classifier = Type.GetClassifierObject(abstractMethodInfo.DeclaringType);
                            classifier.GetOperations(false);
                        }
                    }

                    foreach (System.Reflection.MethodInfo methodInfo in DotNetMethods)
                    {
                        if (methodInfo.DeclaringType != WrType)
                            continue;
                        if (methodInfo.IsSpecialName)
                            continue;

                        Operation operation = null;
                        System.Reflection.MethodInfo abstractMethodInfo = GetOperationForMethod(methodInfo);
                        lock (abstractMethodInfo)
                        {
                            if (abstractMethodInfo != null)
                            {
                                operation = (Operation)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(abstractMethodInfo);
                                if (operation != null && WrType == abstractMethodInfo.DeclaringType)
                                {

                                }
                                if (operation == null)
                                {
                                    if (WrType == abstractMethodInfo.DeclaringType)
                                    {
                                        operation = new Operation(abstractMethodInfo, typeClassifier);
                                        features.Add(operation);
                                    }

                                }
                                else
                                {
                                    if (operation.WrMethod.DeclaringType == WrType)
                                        features.Add(operation);
                                }
                            }
                        }


                        if (!methodInfo.IsAbstract)
                        {
                            lock (methodInfo)
                            {

                                Method method = operation.Implementetions.OfType<Method>().Where(x => x.WrMethod == methodInfo).FirstOrDefault();
                                if(method==null)
                                    method = new Method(methodInfo, operation);
                                else
                                {

                                }

                                features.Add(method);
                                if (operation.WrMethod == method.WrMethod)
                                    features.Add(operation);
                            }
                        }
                        else
                            features.Add(operation);
                    }

                    foreach (var ctor in WrType.GetMetaData().GetConstructors())
                    {
                        Operation operation = (Operation)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ctor);

                        if (operation == null)
                        {
                            operation = new Operation(ctor, typeClassifier);
                            features.Add(operation);
                        }
                        else
                        {
                            if (operation.WrMethod.DeclaringType == WrType)
                                features.Add(operation);
                        }

                    }
#if !DeviceDotNet
                    System.Reflection.ConstructorInfo[] constructorInfos = WrType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
#else
                    System.Reflection.ConstructorInfo[] constructorInfos = WrType.GetMetaData().GetConstructors();
#endif
                    foreach (System.Reflection.ConstructorInfo constructorInfo in constructorInfos)
                    {
                        Operation operation = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(constructorInfo) as Operation;
                        if (operation == null)
                            operation = new Operation(constructorInfo, typeClassifier);
                        features.Add(operation);
                    }

                    #endregion

                    #region Load attributes from fields
                    System.Reflection.FieldInfo[] FieldInfos = WrType.GetMetaData().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
                    foreach (System.Reflection.FieldInfo mFieldInfo in FieldInfos)
                    {

                        if (mFieldInfo.DeclaringType != WrType)
                            continue;
                        if (mFieldInfo.Name.IndexOf("k__BackingField") != -1)
                            continue;
                        bool HasAssocitionAttribute = mFieldInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationAttribute), false).Length > 0;
                        if (!HasAssocitionAttribute)
                        {

                            Attribute mAttribute = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mFieldInfo) as Attribute;

                            if (mAttribute == null)
                                mAttribute = new Attribute(mFieldInfo, typeClassifier);
                            features.Add(mAttribute);
                        }

                    }
                    #endregion

                    #region Load attributes from properties

                    System.Reflection.PropertyInfo[] PropertyInfos = WrType.GetMetaData().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
                    foreach (System.Reflection.PropertyInfo mPropertyInfo in PropertyInfos)
                    {
                        if (mPropertyInfo.DeclaringType != WrType)
                            continue;
                        bool HasAssocitionAttribute = mPropertyInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationAttribute), false).Length > 0;
                        if (!HasAssocitionAttribute)
                        {
                            if (DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mPropertyInfo) is AssociationEndRealization)
                                continue;

                            if (DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mPropertyInfo) is AttributeRealization)
                                continue;
                            Attribute mAttribute = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mPropertyInfo) as Attribute;

                            if (mAttribute == null)
                                mAttribute = new Attribute(mPropertyInfo, typeClassifier);
                            features.Add(mAttribute);
                        }
                    }

                    #endregion



                    Collections.Generic.Set<MetaDataRepository.Interface> interfaces = null;
                    if (typeClassifier is Class)
                        interfaces = (typeClassifier as Class).GetAllInterfaces();
                    else if (typeClassifier is Structure)
                        interfaces = (typeClassifier as Structure).GetAllInterfaces();
                    else
                        interfaces = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Interface>();


                    #region Load AssociationEnd Realizations
                 
                    foreach (Interface _interface in interfaces)
                    {
                        foreach (AssociationEnd associationEnd in _interface.GetAssociateRoles(false))
                        {
                            if (associationEnd.PropertyMember != null && associationEnd.Accessors.Length > 0)
                            {
                                System.Reflection.MethodInfo accessor = associationEnd.Accessors[0];
                                foreach (MetaDataRepository.Feature implementationFeature in features)
                                {
                                    if (!(implementationFeature is MetaDataRepository.Attribute))
                                        continue;
                                    Attribute implementationAttribute = implementationFeature as Attribute;

                                    if (implementationAttribute.PropertyMember == null)
                                        continue;
                                    if (implementationAttribute.Accessors.Length > 0)
                                    {

#if DeviceDotNet
                                        if ((accessor.IsVirtual || accessor.IsAbstract) && GetOperationForMethod(implementationAttribute.Accessors[0]).ToString() == accessor.ToString())
                                        {
                                            features.Remove(implementationAttribute);
                                            AssociationEndRealization associationEndRealization = new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, typeClassifier);
                                            features.Add(associationEndRealization);
                                            break;
                                        }
                                        else
                                        {
                                            if (implementationAttribute.Accessors.Length > 1)
                                            {
                                                if ((accessor.IsVirtual || accessor.IsAbstract) && GetOperationForMethod(implementationAttribute.Accessors[1]).ToString() == accessor.ToString())
                                                {
                                                    features.Remove(implementationAttribute);
                                                    AssociationEndRealization associationEndRealization = new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, typeClassifier);
                                                    features.Add(associationEndRealization);
                                                    break;
                                                }
                                            }
                                        }

#else

                                        if (GetOperationForMethod(implementationAttribute.Accessors[0]) == accessor)
                                        {
                                            features.Remove(implementationAttribute);
                                            AssociationEndRealization associationEndRealization = new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, typeClassifier);
                                            features.Add(associationEndRealization);
                                            break;
                                        }
                                        else
                                        {
                                            if (implementationAttribute.Accessors.Length > 1)
                                            {
                                                if (GetOperationForMethod(implementationAttribute.Accessors[1]) == accessor)
                                                {
                                                    features.Remove(implementationAttribute);
                                                    AssociationEndRealization associationEndRealization = new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, typeClassifier);
                                                    features.Add(associationEndRealization);
                                                    break;
                                                }
                                            }
                                        }
#endif
                                    }
                                }
                            }
                        }
                    }
                    foreach (MetaDataRepository.Classifier classifier in typeClassifier.GetAllGeneralClasifiers())
                    {
                        if (!(classifier is MetaDataRepository.Class))
                            continue;
                        Class _class = classifier as Class;
                        foreach (AssociationEnd associationEnd in _class.GetAssociateRoles(false))
                        {
                            if (associationEnd.PropertyMember != null && associationEnd.Accessors.Length > 0)
                            {

                                System.Reflection.MethodBase accessor = associationEnd.Accessors[0];
                                foreach (MetaDataRepository.Feature implementationFeature in features)
                                {
                                    if (!(implementationFeature is MetaDataRepository.Attribute))
                                        continue;
                                    Attribute implementationAttribute = implementationFeature as Attribute;

                                    if (implementationAttribute.PropertyMember == null)
                                        continue;
                                    if (implementationAttribute.Accessors.Length > 0)
                                    {
                                        //accessor.IsVirtual&&implementationAttribute.Accessors[0].GetBaseDefinition().ToString()==accessor.ToString()
#if DeviceDotNet
                                        if (accessor.IsVirtual && implementationAttribute.Accessors[0].GetBaseDefinition().ToString() == accessor.ToString())
                                        {
                                            features.Remove(implementationAttribute);
                                            features.Add(new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, typeClassifier));
                                            break;
                                        }
                                        else
                                        {
                                            if (implementationAttribute.Accessors.Length > 1)
                                            {
                                                if (accessor.IsVirtual && implementationAttribute.Accessors[1].GetBaseDefinition().ToString() == accessor.ToString())
                                                {
                                                    features.Remove(implementationAttribute);
                                                    features.Add(new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, typeClassifier));
                                                    break;
                                                }
                                            }
                                        }
#else
                                        if (implementationAttribute.Accessors[0].GetBaseDefinition() == accessor)
                                        {
                                            features.Remove(implementationAttribute);
                                            features.Add(new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, typeClassifier));
                                            break;
                                        }
                                        else
                                        {
                                            if (implementationAttribute.Accessors.Length > 1)
                                            {
                                                if (implementationAttribute.Accessors[1].GetBaseDefinition() == accessor)
                                                {
                                                    features.Remove(implementationAttribute);
                                                    features.Add(new AssociationEndRealization(implementationAttribute.PropertyMember, associationEnd, typeClassifier));
                                                    break;
                                                }
                                            }
                                        }
#endif

                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Load Attributes Realizations
                    #region Search for attribute realization in parent interfaces
                    //TODO Στο .net μπορεί να υπάρχει property με το ιδιο όνομα σε ένα ή περισσότερα interface 
                    //στην ιεραρχία άρα  η property της class μπορεί υλοποιεί πάνω από ένα interafce property. 
                    //Aυτή την κατάσταση την αγνοεί το DotnetMetaDataRepository

                    foreach (Interface _interface in interfaces)
                    {

                        InterfaceMapping interfaceMapping = GetInterfaceMap(_interface.Refer.WrType);
                        if (interfaceMapping.InterfaceMethods == null)
                            continue;
                        foreach (Attribute attribute in _interface.GetAttributes(false))
                        {
                            if (attribute.Accessors.Length == 0)
                                continue;
                            System.Reflection.MethodBase accessor = attribute.Accessors[0];
                            System.Reflection.MethodBase accessorImplementation = null;
                            for (int i = 0; i < interfaceMapping.InterfaceMethods.Length; i++)
                            {
                                if (interfaceMapping.InterfaceMethods[i] == accessor)
                                {
                                    accessorImplementation = interfaceMapping.TargetMethods[i];
                                    break;
                                }
                            }
                            if (accessorImplementation == null)
                                continue;

                            foreach (MetaDataRepository.Feature implementationFeature in features)
                            {
                                if (!(implementationFeature is MetaDataRepository.Attribute))
                                    continue;
                                Attribute implementationAttribute = implementationFeature as Attribute;

                                if (implementationAttribute.PropertyMember == null)
                                    continue;
                                if (implementationAttribute.Accessors.Length > 0)
                                {
                                    //if (Refer.GetOperationForMethod(implementationAttribute.Accessors[0]) == accessor)

#if DeviceDotNet
                                    if ((accessorImplementation.IsVirtual || accessorImplementation.IsAbstract) && 
                                        implementationAttribute.Accessors[0].ToString() == accessorImplementation.ToString())
                                    {
                                        features.Remove(implementationAttribute);
                                        AttributeRealization attributeRealization = new AttributeRealization(implementationAttribute.PropertyMember, attribute, typeClassifier);
                                        features.Add(attributeRealization);
                                        break;
                                    }
                                    else
                                    {
                                        if (implementationAttribute.Accessors.Length > 1)
                                        {
                                            //if (Refer.GetOperationForMethod(implementationAttribute.Accessors[1]) == accessor)
                                            if (implementationAttribute.Accessors[1] == accessorImplementation)
                                            {
                                                features.Remove(implementationAttribute);
                                                AttributeRealization attributeRealization = new AttributeRealization(implementationAttribute.PropertyMember, attribute, typeClassifier);
                                                features.Add(attributeRealization);
                                                break;
                                            }
                                        }
                                    }

#else

                                    if (implementationAttribute.Accessors[0] == accessorImplementation)
                                    {
                                        features.Remove(implementationAttribute);
                                        AttributeRealization attributeRealization = new AttributeRealization(implementationAttribute.PropertyMember, attribute, typeClassifier);
                                        features.Add(attributeRealization);
                                        break;
                                    }
                                    else
                                    {
                                        if (implementationAttribute.Accessors.Length > 1)
                                        {
                                            //if (Refer.GetOperationForMethod(implementationAttribute.Accessors[1]) == accessor)
                                            if (implementationAttribute.Accessors[1] == accessorImplementation)
                                            {
                                                features.Remove(implementationAttribute);
                                                AttributeRealization attributeRealization = new AttributeRealization(implementationAttribute.PropertyMember, attribute, typeClassifier);
                                                features.Add(attributeRealization);
                                                break;
                                            }
                                        }
                                    }
#endif
                                }
                            }
                        }
                    }
                    #endregion

                    foreach (MetaDataRepository.Classifier classifier in typeClassifier.GetAllGeneralClasifiers())
                    {
                        if (!(classifier is MetaDataRepository.Class))
                            continue;
                        Class _class = classifier as Class;

                        foreach (Attribute attribute in _class.GetAttributes(false))
                        {
                            if (attribute.PropertyMember != null)
                            {
                                System.Reflection.MethodInfo[] methodInfos = attribute.Accessors;
                                if (methodInfos.Length > 0)
                                {
                                    System.Reflection.MethodBase accessor = attribute.Accessors[0];

                                    foreach (MetaDataRepository.Feature implementationFeature in features)
                                    {
                                        if (!(implementationFeature is MetaDataRepository.Attribute))
                                            continue;
                                        Attribute implementationAttribute = implementationFeature as Attribute;

                                        if (implementationAttribute.PropertyMember == null)
                                            continue;
                                        if (implementationAttribute.Accessors.Length > 0)
                                        {

#if DeviceDotNet
                                            if (accessor.IsVirtual && implementationAttribute.Accessors[0].GetBaseDefinition().ToString() == accessor.ToString())
                                            {
                                                features.Remove(implementationAttribute);
                                                features.Add(new AttributeRealization(implementationAttribute.PropertyMember, attribute, typeClassifier));
                                                break;
                                            }
                                            else
                                            {
                                                if (implementationAttribute.Accessors.Length > 1)
                                                {
                                                    if (accessor.IsVirtual && implementationAttribute.Accessors[1].GetBaseDefinition().ToString() == accessor.ToString())
                                                    {
                                                        features.Remove(implementationAttribute);
                                                        features.Add(new AttributeRealization(implementationAttribute.PropertyMember, attribute, typeClassifier));
                                                        break;
                                                    }
                                                }
                                            }
#else

                                            if (implementationAttribute.Accessors[0].GetBaseDefinition() == accessor)
                                            {
                                                features.Remove(implementationAttribute);
                                                features.Add(new AttributeRealization(implementationAttribute.PropertyMember, attribute, typeClassifier));
                                                break;
                                            }
                                            else
                                            {
                                                if (implementationAttribute.Accessors.Length > 1)
                                                {
                                                    if (implementationAttribute.Accessors[1].GetBaseDefinition() == accessor)
                                                    {
                                                        features.Remove(implementationAttribute);
                                                        features.Add(new AttributeRealization(implementationAttribute.PropertyMember, attribute, typeClassifier));
                                                        break;
                                                    }
                                                }
                                            }
#endif
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    //System.Diagnostics.Debug.WriteLine(WrType.Namespace + "." + WrType.Name+" "+((System.TimeSpan)(System.DateTime.Now - start)).TotalMilliseconds.ToString());
                    timeSpan += System.DateTime.Now - start;

                    FeaturesLoaded = true;

                }

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{D51CC165-E90B-4BAE-AB85-55D0AB76515C}</MetaDataID>
        internal bool IsNestedType
        {
            get
            {
                if (WrType.GetMetaData().IsNestedPrivate || WrType.GetMetaData().IsNestedPublic || WrType.GetMetaData().IsNestedFamily || WrType.GetMetaData().IsNestedAssembly)
                    return true;
                return false;
            }
        }
        /// <summary>This method retrieves the first-level nested class collection from the class and returns it. 
        /// To retrieve all nested classes of the specified class and all of its nested classes, use GetAllNestedClasses. </summary>
        /// <MetaDataID>{76D46072-679F-429D-89DE-5A9D786B2145}</MetaDataID>
        internal OOAdvantech.Collections.Generic.Set<MetaDataRepository.Classifier> GetNestedClassifier()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                lock (Type.LoadDotnetMetadataLock)
                {
                    System.Type[] NestedTypes = WrType.GetMetaData().GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
                    OOAdvantech.Collections.Generic.Set<MetaDataRepository.Classifier> NestedClassifier = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();
                    foreach (System.Type CurrType in NestedTypes)
                    {
                        MetaDataRepository.Classifier mClassifier = GetClassifierObject(CurrType);
                        NestedClassifier.Add(mClassifier);
                    }

                    return NestedClassifier;
                }
                stateTransition.Consistent = true;
            }

        }
        static public MetaDataRepository.MetaObject GetClassifierMember(MetaDataRepository.Classifier classifier, System.Reflection.MemberInfo member)
        {
            MetaDataRepository.MetaObject classifierMember = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(member);
            if (classifierMember != null)
                return classifierMember;


            foreach (MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
            {
                if (associationEnd.GetExtensionMetaObject(member.GetType()) == member)
                    return associationEnd;
                else
                    foreach (var associationEndRealization in associationEnd.AssociationEndRealizations)
                    {
                        if (associationEndRealization.GetExtensionMetaObject(member.GetType()) == member)
                            return associationEndRealization;
                    }
            }
            foreach (MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
            {
                if (attribute.GetExtensionMetaObject(member.GetType()) == member)
                    return attribute;
                else
                {
                    foreach (var attributeRealization in attribute.AttributeRealizations)
                    {
                        if (attributeRealization.GetExtensionMetaObject(member.GetType()) == member)
                            return attributeRealization;
                    }
                }
            }
            return null;

        }
        /// <summary>This method creates the corresponding classifier for the type. 
        /// If  the type is class create class if it is enumerator create Enumerator. </summary>
        /// <param name="type">Define the type for which the system creates corresponding classifier.
        /// This parameter must be not null </param>
        /// <returns>Returns the corresponding classifier object for the type </returns>
        /// <MetaDataID>{9CBD14B5-003B-47D9-B95E-7A4D2E2D033E}</MetaDataID>
        static public MetaDataRepository.Classifier GetClassifierObject(System.Type type)
        {

            if (type == null)
            {
                return null;
            }
            bool isGen = type.GetMetaData().IsGenericType;
            bool isGenpar = type.GetMetaData().IsGenericTypeDefinition;
            string fullName = type.FullName;

            MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as MetaDataRepository.Classifier;
            if (classifier != null)
                return classifier;
            lock (type)
            {
                classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as MetaDataRepository.Classifier;
                if (classifier != null)
                    return classifier;

                try
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        if (type.FullName == "DontWaitApp.ApplicationSettings")
                        {
                            string tt = type.AssemblyQualifiedName;
                        }
                        classifier = MetaObjectMapper.FindMetaObjectFor(type) as MetaDataRepository.Classifier;
                        if (classifier != null)
                            return classifier;
#if !DeviceDotNet
#endif
                        if (type == null)
                            throw new System.ArgumentNullException("type");

                        if (type.IsGenericParameter)
                        {
                            //System.Windows.Forms.MessageBox.Show("asdas");
                            throw new System.ArgumentNullException("type");
                        }
                        if (type.GetMetaData().IsClass)
                        {
                            return new Class(new Type(type));
                        }
                        else if (type.GetMetaData().IsPrimitive)
                        {
                            return new Primitive(new Type(type));
                        }
                        else if (type.GetMetaData().IsInterface)
                        {
                            return new Interface(new Type(type));
                        }
                        else if (type.GetMetaData().IsEnum)
                        {
                            return new Enumeration(new Type(type));
                        }
                        else if (type.GetMetaData().IsValueType/*structs*/)
                        {
                            return new Structure(new Type(type));
                        }
                        else if (type == typeof(System.Enum))
                        {
                            return new Enumeration(new Type(type));
                        }
                        throw new System.Exception("System can't create classifier for type '" + type.FullName + "'.");

                        stateTransition.Consistent = true;
                    }
                }
                catch (System.Exception error)
                {
                    classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as MetaDataRepository.Classifier;
                    if (classifier != null)
                        return classifier;

                    throw;

                }
            }

        }

        /// <MetaDataID>{7ACF9653-B608-45F0-B632-7F8734E9D0DD}</MetaDataID>
        MetaDataRepository.AssociationAttribute GetAssociationAttribute(System.Reflection.MemberInfo Member)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {



                object[] objectCustomAttributes = Member.GetCustomAttributes(typeof(MetaDataRepository.AssociationAttribute), false);
                object[] associationClassAttributes = Member.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass), false);
                if (objectCustomAttributes.Length == 0)
                    return null;
                else
                {
                    MetaDataRepository.AssociationAttribute associationAttribute = objectCustomAttributes[0] as MetaDataRepository.AssociationAttribute;
                    if (associationAttribute._OtherEndType == null)
                    {
                        System.Type memberType = null;
                        if (Member is System.Reflection.FieldInfo)
                            memberType = (Member as System.Reflection.FieldInfo).FieldType;
                        if (Member is System.Reflection.PropertyInfo)
                            memberType = (Member as System.Reflection.PropertyInfo).PropertyType;

                        if (AssociationEnd.FindIEnumerable(memberType) != null)
                        {
                            if (AssociationEnd.FindIEnumerable(memberType).GetMetaData().GetGenericArguments()[0].FullName.IndexOf("System.Collections.Generic.KeyValuePair") == 0)
                                associationAttribute.OtherEndType = AssociationEnd.FindIEnumerable(memberType).GetMetaData().GetGenericArguments()[0].GetMetaData().GetGenericArguments()[1];
                            else
                                associationAttribute.OtherEndType = AssociationEnd.FindIEnumerable(memberType).GetMetaData().GetGenericArguments()[0];
                        }
                        else
                            associationAttribute.OtherEndType = memberType;

                        if (associationClassAttributes.Length > 0)
                        {
                            if ((associationClassAttributes[0] as MetaDataRepository.AssociationClass).AssocciationClass == associationAttribute.OtherEndType)
                            {
                                foreach (var fieldInfo in associationAttribute.OtherEndType.GetMetaData().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                                {
                                    var associationClassRole = fieldInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), false).OfType<MetaDataRepository.AssociationClassRole>().FirstOrDefault();
                                    if (associationClassRole != null && associationClassRole.IsRoleA == associationAttribute.IsRoleA)
                                        associationAttribute.OtherEndType = fieldInfo.FieldType;
                                }

                                foreach (var propertyInfo in associationAttribute.OtherEndType.GetMetaData().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                                {
                                    var associationClassRole = propertyInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), false).OfType<MetaDataRepository.AssociationClassRole>().FirstOrDefault();
                                    if (associationClassRole != null && associationClassRole.IsRoleA == associationAttribute.IsRoleA)
                                        associationAttribute.OtherEndType = propertyInfo.PropertyType;

                                }



                            }
                        }

                    }

                    return associationAttribute;
                }

                stateTransition.Consistent = true;
            }

        }
        /// <summary>
        /// </summary>
        /// <MetaDataID>{4A0AFE80-08A9-4786-8CC2-B7DC23AA5CB5}</MetaDataID>
        internal AssociationEnd SearchForAssociationEnd(string associotionIdentity, System.Reflection.MemberInfo excludeMember, bool searchForName, MetaDataRepository.Roles role)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                if (associotionIdentity == null || associotionIdentity.Trim().Length == 0)
                    return null;
                System.Collections.Generic.List<MemberInfo> memberInfos = WrType.GetMetaData().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).OfType<MemberInfo>().ToList();
                memberInfos.AddRange(WrType.GetMetaData().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

                foreach (System.Reflection.MemberInfo memberInfo in memberInfos)
                {
                    MetaDataRepository.AssociationAttribute associationAttribute = GetAssociationAttribute(memberInfo);

                    #region Check if the member has declared as association of associotionIdentity if not continue to the next member

                    if (associationAttribute == null)
                        continue;


                    if (memberInfo.DeclaringType != WrType)
                        continue;
                    if (memberInfo == excludeMember)
                        continue;

                    if (searchForName && associationAttribute.AssociationName != associotionIdentity)
                        continue;
                    if (!searchForName && associationAttribute.Identity != associotionIdentity)
                        continue;

                    if (searchForName && associationAttribute.AssociationName == associotionIdentity && associationAttribute.Role != role)
                        continue;
                    if (!searchForName && associationAttribute.Identity == associotionIdentity && associationAttribute.Role != role)
                        continue;


                    #endregion

                    AssociationEnd associationEnd = (AssociationEnd)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(memberInfo);
                    if (associationEnd != null)
                        return associationEnd;
                    else
                        return CreateAssociationEnd(memberInfo, associationAttribute);
                }

                return null;
            }
        }
        /// <MetaDataID>{35EE5A94-9D96-4E11-8B0C-CDAF91C11501}</MetaDataID>
        AssociationEnd CreateAssociationEnd(System.Reflection.MemberInfo memberInfo, MetaDataRepository.AssociationAttribute associationAttribute)
        {
            return new AssociationEnd(memberInfo, associationAttribute);
        }
        /// <MetaDataID>{75B09C0F-2CF7-43DB-9DAD-DCF59FCC1AAC}</MetaDataID>
        internal Association GetAssociation(MetaDataRepository.AssociationClass associationClass)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                MetaDataRepository.Classifier roleAClass = GetClassifierObject(associationClass.AssocciationEndRoleA);
                foreach (AssociationEnd CurrAssociationEnd in roleAClass.Roles)
                {
                    if (CurrAssociationEnd.Association.Name == associationClass.AssocciationName)
                        return CurrAssociationEnd.Association as Association;

                }
                MetaDataRepository.Classifier RoleBClass = GetClassifierObject(associationClass.AssocciationEndRoleB);

                foreach (AssociationEnd CurrAssociationEnd in RoleBClass.Roles)
                {
                    if (CurrAssociationEnd.Association.Name == associationClass.AssocciationName)
                        return CurrAssociationEnd.Association as Association;
                }
                return null;
                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{B704522C-84DB-40A2-B6BB-AD17A39DD84C}</MetaDataID>
        internal void GetRoles(MetaDataRepository.Classifier typeClassifier)
        {

            //OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                lock (LoadDotnetMetadataLock)
                {
                    if (WrType.GetMetaData().IsGenericType)
                        return;
                    System.DateTime start = System.DateTime.Now;

                    System.Collections.Generic.List<MemberInfo> typeMembers = WrType.GetMetaData().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).OfType<MemberInfo>().ToList();
                    typeMembers.AddRange(WrType.GetMetaData().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
                    typeMembers = typeMembers.Where(x => x.DeclaringType == WrType && GetAssociationAttribute(x) != null).ToList();
                    foreach (System.Reflection.MemberInfo memberInfo in typeMembers)
                    {

                        MetaDataRepository.AssociationAttribute associationAttribute = GetAssociationAttribute(memberInfo);
                        string memberName = memberInfo.Name;
                        AssociationEnd associationEnd = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(memberInfo) as AssociationEnd;
                        if (associationEnd == null)
                        {
                            associationEnd = CreateAssociationEnd(memberInfo, associationAttribute);

                            associationEnd.IsRoleA = associationAttribute.IsRoleA;
                            AssociationEnd otherAssociationEnd = null;

                            Type otherAssociationEndType = null;
                            MetaDataRepository.Classifier otherAssociationEndNamespace = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(associationAttribute.OtherEndType) as MetaDataRepository.Classifier;
                            if (otherAssociationEndNamespace == null)
                                otherAssociationEndNamespace = DotNetMetaDataRepository.Type.GetClassifierObject(associationAttribute.OtherEndType);

                            if (otherAssociationEndNamespace is Class)
                                otherAssociationEndType = (otherAssociationEndNamespace as Class).Refer;


                            if (otherAssociationEndNamespace is Interface)
                                otherAssociationEndType = (otherAssociationEndNamespace as Interface).Refer;
                            if (otherAssociationEndType == null && otherAssociationEndNamespace != null)
                                throw new MetaDataRepository.MetaDataException(new MetaDataRepository.MetaObject.MetaDataError("Error at " + memberInfo.DeclaringType.FullName + "." + memberInfo.Name + " you can't declare Association with type other than interface or class", memberInfo.DeclaringType.FullName + "." + memberInfo.Name));


                            MetaDataRepository.Roles otherEndRole;
                            if (associationAttribute.IsRoleA)
                                otherEndRole = OOAdvantech.MetaDataRepository.Roles.RoleB;
                            else
                                otherEndRole = OOAdvantech.MetaDataRepository.Roles.RoleA;

                            if (associationAttribute.Identity != null && associationAttribute.Identity.Trim().Length > 0)
                                otherAssociationEnd = otherAssociationEndType.SearchForAssociationEnd(associationAttribute.Identity, memberInfo, false, otherEndRole);
                            else
                                otherAssociationEnd = otherAssociationEndType.SearchForAssociationEnd(associationAttribute.AssociationName, memberInfo, true, otherEndRole);


                            if (otherAssociationEnd == null)
                            {
                                MetaDataRepository.MultiplicityRange RoleAMultiplicity = null;
                                MetaDataRepository.MultiplicityRange RoleBMultiplicity = null;

                                object[] customAttributes = memberInfo.GetCustomAttributes(typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute), false);
                                if (customAttributes.Length != 0)
                                    RoleAMultiplicity = (customAttributes[0] as MetaDataRepository.RoleAMultiplicityRangeAttribute).Multiplicity;

                                customAttributes = memberInfo.GetCustomAttributes(typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute), false);
                                if (customAttributes.Length != 0)
                                    RoleBMultiplicity = (customAttributes[0] as MetaDataRepository.RoleBMultiplicityRangeAttribute).Multiplicity;
                                if (RoleAMultiplicity == null)
                                    RoleAMultiplicity = new MetaDataRepository.MultiplicityRange();
                                if (RoleBMultiplicity == null)
                                    RoleBMultiplicity = new MetaDataRepository.MultiplicityRange();


                                if (associationEnd.Role == MetaDataRepository.Roles.RoleA)
                                    otherAssociationEnd = new AssociationEnd(otherAssociationEndNamespace, MetaDataRepository.Roles.RoleB, typeClassifier, memberInfo.DeclaringType.GetMetaData().Assembly, associationAttribute, RoleBMultiplicity);
                                else
                                    otherAssociationEnd = new AssociationEnd(otherAssociationEndNamespace, MetaDataRepository.Roles.RoleA, typeClassifier, memberInfo.DeclaringType.GetMetaData().Assembly, associationAttribute, RoleAMultiplicity);
                            }
                            else
                                MetaObjectMapper.RemoveMetaObject(otherAssociationEnd.Association);

                            Association association = null;
                            if (associationEnd.IsRoleA)
                                association = new Association(memberInfo.DeclaringType.GetMetaData().Assembly, associationAttribute, associationEnd, otherAssociationEnd);
                            else
                                association = new Association(memberInfo.DeclaringType.GetMetaData().Assembly, associationAttribute, otherAssociationEnd, associationEnd);
                        }
                    }
                    timeSpan += System.DateTime.Now - start;
                }
                stateTransition.Consistent = true;
            }

            //finally
            //{
            //    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            //}

        }

        /// <MetaDataID>{49DE3D59-79F1-4DF1-AB85-FA635B099DA1}</MetaDataID>
        internal System.Type WrType;

        /// <summary>This method retrieves the parent interfaces of type and base type, 
        /// creates the corresponding Generalization relationships and return a collection with them. </summary>
        /// <MetaDataID>{3E1986CA-8CA2-47E6-A575-D0556FA42451}</MetaDataID>
        internal OOAdvantech.Collections.Generic.Set<MetaDataRepository.Generalization> GetGeneralizations(MetaDataRepository.Classifier typeClassifier)
        {




            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                OOAdvantech.Collections.Generic.Set<MetaDataRepository.Generalization> generalizations = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Generalization>();
                System.Collections.Generic.List<System.Type> superTypes = new System.Collections.Generic.List<System.Type>();
                if (WrType.GetMetaData().IsInterface)
                {
                    //Gets all  supper types in interface hierarchy and removes the indirect supper types 
                    //There isn't other way to catch the parent interfaces, from the interface
                    System.Type[] allSuperTypes = WrType.GetMetaData().GetInterfaces();
                    superTypes.AddRange(allSuperTypes);
                    foreach (System.Type type in allSuperTypes)
                    {
                        foreach (System.Type indirectSuperType in type.GetMetaData().GetInterfaces())
                        {
                            if (superTypes.Contains(indirectSuperType))
                                superTypes.Remove(indirectSuperType);
                        }
                    }
                }
                else if (WrType.GetMetaData().IsClass)
                {
                    //In case where type is class the answer of the super type is simple. 
                    //There is only single inheritance

                    if (WrType.GetMetaData().BaseType != null)
                        superTypes.Add(WrType.GetMetaData().BaseType);
                }
                foreach (System.Type superType in superTypes)
                {
                    //Creates the generalization relationships
                    MetaDataRepository.Classifier superClassifier = GetClassifierObject(superType);
                    generalizations.Add(new MetaDataRepository.Generalization("", superClassifier, typeClassifier));

                }

                stateTransition.Consistent = true;
                return generalizations;
            }


            //finally
            //{
            //    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            //}

        }
        /// <MetaDataID>{62A5BA4B-D9E2-4C62-9125-14319759532A}</MetaDataID>
        internal Type(System.Type type)
        {
            //System.Diagnostics.Debug.WriteLine((System.DateTime.Now - OOAdvantech.DotNetMetaDataRepository.Type.StartTime).TotalSeconds);
            if (StartTime == System.DateTime.MinValue)
                StartTime = System.DateTime.Now;


            WrType = type;
            if (WrType.GetMetaData().IsPublic || WrType.GetMetaData().IsNestedPublic)
                Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
            if (WrType.GetMetaData().IsNestedFamily)
                Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
            if (WrType.GetMetaData().IsNestedAssembly || WrType.GetMetaData().IsNotPublic)
                Visibility = MetaDataRepository.VisibilityKind.AccessComponent;
            if (WrType.GetMetaData().IsNestedPrivate)
                Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
        }


        /// <MetaDataID>{D104DF92-84BD-4F2D-AC40-CE3661656CBE}</MetaDataID>
        internal InterfaceMapping GetInterfaceMap(System.Type interfaceType)
        {
            InterfaceMapping interfaceMapping = new InterfaceMapping();
#if !DeviceDotNet
            try
            {
                //TODO : υπάρχει πρόβλημα με τα generic type
                System.Reflection.InterfaceMapping reflectionInterfaceMapping = WrType.GetInterfaceMap(interfaceType);
                interfaceMapping.InterfaceMethods = reflectionInterfaceMapping.InterfaceMethods;
                interfaceMapping.InterfaceType = reflectionInterfaceMapping.InterfaceType;
                interfaceMapping.TargetMethods = reflectionInterfaceMapping.TargetMethods;
                interfaceMapping.TargetType = reflectionInterfaceMapping.TargetType;


            }
            catch (System.Exception error)
            {
                //TODO : υπάρχει πρόβλημα με τα generic type


            }
#else

            System.Collections.Generic.List<System.Reflection.MethodInfo> interfaceMethods = new System.Collections.Generic.List<System.Reflection.MethodInfo>();
            System.Collections.Generic.List<System.Reflection.MethodInfo> targetMethods = new System.Collections.Generic.List<System.Reflection.MethodInfo>();

            foreach (System.Reflection.MethodInfo interfaceMethodInfo in
                    interfaceType.GetMetaData().GetMethods(BindingFlags.Public
                                | BindingFlags.NonPublic
                                | BindingFlags.Instance
                                | BindingFlags.DeclaredOnly))
            {
                bool findImplicitMethod = false;
                System.Reflection.MethodInfo mappedMethod = null;
                foreach (System.Reflection.MethodInfo methodInfo in
                        WrType.GetMetaData().GetMethods(BindingFlags.Public
                                    | BindingFlags.NonPublic
                                    | BindingFlags.Instance
                                    | BindingFlags.DeclaredOnly))
                {
                    bool explicitly = false;
                    if (IsImplementationMethod(interfaceType, interfaceMethodInfo, methodInfo, out explicitly))
                    {
                        mappedMethod = methodInfo;
                        if (explicitly)
                            break;
                    }

                    int tt = 0;

                }
                if (mappedMethod != null)
                {
                    interfaceMethods.Add(interfaceMethodInfo);
                    targetMethods.Add(mappedMethod);
                }

            }
            System.Reflection.MethodInfo[] methods = new System.Reflection.MethodInfo[interfaceMethods.Count];
            for (int i = 0; i < interfaceMethods.Count; i++)
                methods[i] = interfaceMethods[i];
            interfaceMapping.InterfaceMethods = methods;

            methods = new System.Reflection.MethodInfo[targetMethods.Count];
            for (int i = 0; i < targetMethods.Count; i++)
                methods[i] = targetMethods[i];
            interfaceMapping.TargetMethods = methods;

#endif
            return interfaceMapping;
        }

        /// <MetaDataID>{5f228d9a-522c-4dff-9eba-42988c5bb96d}</MetaDataID>
        internal static System.Type GetInterface(System.Type type, string interfaceFullName)
        {
#if !DeviceDotNet
            return type.GetInterface(interfaceFullName);
#else
            foreach (System.Type _interface in type.GetMetaData().GetInterfaces())
            {
                if (_interface.FullName == interfaceFullName)
                    return _interface;
            }
            return null;
#endif
        }

        /// <MetaDataID>{57bcce58-c6ae-4591-999c-5582716d3d55}</MetaDataID>
        internal void GenericTypeInit(MetaDataRepository.Classifier classifier, ref OOAdvantech.MetaDataRepository.TemplateSignature _OwnedTemplateSignature, ref OOAdvantech.MetaDataRepository.TemplateBinding _TemplateBinding)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                lock (WrType)
                {
                    if (IsGenericTypeDefinition)
                    {
                        if (_OwnedTemplateSignature != null)
                            return;
                        OOAdvantech.MetaDataRepository.TemplateSignature ownedTemplateSignature = new MetaDataRepository.TemplateSignature(classifier);
                        ownedTemplateSignature.Template = classifier;
                        foreach (MetaDataRepository.TemplateParameter templateParameter in TemplateParameters)
                            ownedTemplateSignature.AddOwnedParameter(templateParameter);
                        if (TemplateParameters.Count == 0)
                            throw new System.Exception("There is generic type '" + WrType.FullName + "' without generic parameters");
                        if (ownedTemplateSignature.OwnedParameters.Count == 0)
                            throw new System.Exception("There is generic type '" + WrType.FullName + "' without generic parameters.");
                        _OwnedTemplateSignature = ownedTemplateSignature;


                    }
                    else if (WrType.GetMetaData().IsGenericType)
                    {

                        if (_TemplateBinding != null)
                            return;
                        System.Type genericTypeDefinition = WrType.GetMetaData().Assembly.GetType(WrType.Namespace + "." + WrType.Name);
                        if (genericTypeDefinition == null)
                            return;
                        MetaDataRepository.Classifier genericClassifier = Type.GetClassifierObject(genericTypeDefinition);

                        Collections.Generic.List<MetaDataRepository.IParameterableElement> parameterSubstitutions = new Collections.Generic.List<OOAdvantech.MetaDataRepository.IParameterableElement>();
                        int i = 0;
                        foreach (System.Type type in WrType.GetMetaData().GetGenericArguments())
                        {

                            MetaDataRepository.IParameterableElement parameterableElement = MetaObjectMapper.FindMetaObjectFor(type) as MetaDataRepository.IParameterableElement;
                            if (parameterableElement == null)
                            {
                                if (type.IsGenericParameter)
                                {
                                    parameterableElement = new TemplateParameter(genericTypeDefinition.GetMetaData().GetGenericArguments()[i].Name);
                                    MetaObjectMapper.AddTypeMap(type, parameterableElement as MetaDataRepository.TemplateParameter);
                                }
                                else
                                    parameterableElement = Type.GetClassifierObject(type);
                            }
                            parameterSubstitutions.Add(parameterableElement);
                            i++;
                        }
                        _TemplateBinding = new OOAdvantech.MetaDataRepository.TemplateBinding(genericClassifier, parameterSubstitutions);
                    }
                }
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{b5e005bc-b611-4e54-a776-161147e38f1e}</MetaDataID>
        internal static Collections.Generic.Set<MetaDataRepository.Classifier> GetClassifierObjects(System.Type[] types)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                lock (Type.LoadDotnetMetadataLock)
                {
                    Collections.Generic.Set<MetaDataRepository.Classifier> classifiers = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Classifier>();

                    foreach (System.Type currType in types)
                    {
                        if ("<PrivateImplementationDetails>" == currType.Name)
                            continue;
                        if (currType.Namespace == null)
                            continue;
                        if (currType.Name.Length > 0 && currType.Name[0] == '<' && currType.FullName.IndexOf("+<") != -1)
                            continue;
                        if (currType.FullName.IndexOf("<>") != -1)
                            continue;


                        string TypeName = currType.Name;

                        MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(currType) as MetaDataRepository.Classifier;
                        if (classifier == null)
                            classifier = Type.GetClassifierObject(currType);
                        long count = classifier.Generalizations.Count;
                        if (classifier is Class)
                            count = (classifier as Class).Realizations.Count;
                        classifiers.Add(classifier);

                    }
                    foreach (MetaDataRepository.MetaObject CurrMetaObject in classifiers)
                    {
                        if (CurrMetaObject.Namespace != null)
                        {
                            MetaDataRepository.MetaObjectID identity = CurrMetaObject.Namespace.Identity;
                        }
                        if (CurrMetaObject is MetaDataRepository.Classifier)
                        {
                            int count = (CurrMetaObject as MetaDataRepository.Classifier).Generalizations.Count;
                        }
                    }
                    return classifiers;
                }
                stateTransition.Consistent = true;
            }


        }

        /// <MetaDataID>{d2e83ee2-c918-4a52-9785-132bc7de004c}</MetaDataID>
        internal void GetRealizations(ref Collections.Generic.Set<MetaDataRepository.Realization> realizations, MetaDataRepository.Class _class)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                realizations = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Realization>();
                System.Collections.Generic.List<System.Type> SuperTypes = new System.Collections.Generic.List<System.Type>();
                System.Type[] AllSuperTypes = WrType.GetMetaData().GetInterfaces();
                SuperTypes.AddRange(AllSuperTypes);
                foreach (System.Type mType in AllSuperTypes)
                {
                    foreach (System.Type IndirectSuperType in mType.GetMetaData().GetInterfaces())
                    {
                        if (SuperTypes.Contains(IndirectSuperType))
                            SuperTypes.Remove(IndirectSuperType);
                    }
                }
                if (WrType.GetMetaData().BaseType != null)
                {
                    foreach (System.Type IndirectSuperType in WrType.GetMetaData().BaseType.GetMetaData().GetInterfaces())
                    {
                        if (SuperTypes.Contains(IndirectSuperType))
                            SuperTypes.Remove(IndirectSuperType);
                    }


                }
                foreach (System.Type mType in SuperTypes)
                {
                    int ie = 0;
                    if (mType.FullName == "System.Collections.Generic.IEnumerable`1[[System.Collections.Generic.KeyValuePair`2[[System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],TSource], mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]")
                    {

                        ie++;
                    }

                    Interface mInterface = GetClassifierObject(mType) as Interface;
                    MetaDataRepository.Realization mRealization = new MetaDataRepository.Realization("", mInterface, _class);
                    realizations.Add(mRealization);

                }

                stateTransition.Consistent = true;
            }



        }
        /// <MetaDataID>{a8889dd5-f5cd-4680-8661-b69325ce5831}</MetaDataID>
        internal void GetRealizations(ref Collections.Generic.Set<MetaDataRepository.Realization> realizations, MetaDataRepository.Structure _struct)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                //lock (Type.LoadDotnetMetadataLock)
                {
                    realizations = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Realization>();
                    System.Collections.Generic.List<System.Type> SuperTypes = new System.Collections.Generic.List<System.Type>();
                    System.Type[] AllSuperTypes = WrType.GetMetaData().GetInterfaces();
                    SuperTypes.AddRange(AllSuperTypes);
                    foreach (System.Type mType in AllSuperTypes)
                    {
                        foreach (System.Type IndirectSuperType in mType.GetMetaData().GetInterfaces())
                        {
                            if (SuperTypes.Contains(IndirectSuperType))
                                SuperTypes.Remove(IndirectSuperType);
                        }
                    }
                    foreach (System.Type mType in SuperTypes)
                    {
                        Interface mInterface = GetClassifierObject(mType) as Interface;
                        MetaDataRepository.Realization mRealization = new MetaDataRepository.Realization("", mInterface, _struct);
                        realizations.Add(mRealization);

                    }
                }
                stateTransition.Consistent = true;
            }



        }

        static object NameSpacesLock = new object();

        internal static Namespace GetNameSpace(string @namespace)
        {
            lock (NameSpacesLock)
            {
                Namespace mNamespace = (Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(@namespace);
                if (mNamespace == null)
                    mNamespace = new Namespace(@namespace);

                return mNamespace;
            }
        }
    }
}
