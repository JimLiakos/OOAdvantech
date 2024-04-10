namespace OOAdvantech.DotNetMetaDataRepository
{
    using System.Linq;
    using System.Reflection;
    /// <MetaDataID>{751FF8D0-262C-4C13-BAE8-DAC4F2EC2DFA}</MetaDataID>
    public class Operation : MetaDataRepository.Operation
    {
        public override ObjectMemberGetSet SetMemberValue(object token, MemberInfo member, object value)
        {
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
            if (member.Name == nameof(ParameterLoaded))
            {
                if (value == null)
                    ParameterLoaded = default(bool);
                else
                    ParameterLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(WrMethod))
            {
                if (value == null)
                    WrMethod = default(System.Reflection.MethodBase);
                else
                    WrMethod = (System.Reflection.MethodBase)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, MemberInfo member)
        {

            if (member.Name == nameof(ExtensionMetaObjects))
                return GetExtensionMetaObjects();

            if (member.Name == nameof(ParameterLoaded))
                return ParameterLoaded;

            if (member.Name == nameof(WrMethod))
                return WrMethod;


            return base.GetMemberValue(token, member);
        }




        /// <MetaDataID>{e147dde8-d911-40c3-b00c-5423ad6a5516}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {

        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{3452ecac-f4e0-43ca-a812-c02b0a605ff7}</MetaDataID>
        public override void AddOperationImplementation(OOAdvantech.MetaDataRepository.Method method)
        {
            if (!Implementetions.Contains(method))
                Implementetions.Add(method);
        }
        /// <MetaDataID>{5d6969da-fcdb-4e4d-a676-8d05a3752711}</MetaDataID>
        public override void RemoveOperationImplementation(OOAdvantech.MetaDataRepository.Method method)
        {
            if (Implementetions.Contains(method))
                Implementetions.Add(method);
        }
        /// <MetaDataID>{F8CF58E8-005B-4F3F-9FB4-F9ACEF00FBB5}</MetaDataID>
        private System.Collections.Generic.List<object> ExtensionMetaObjects;
        /// <MetaDataID>{812F2068-1904-4106-AFFF-45BF03FAC9E3}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            lock(ExtensionMetaObjectsLock)
            {
                if (ExtensionMetaObjects == null)
                {
                    ExtensionMetaObjects = new System.Collections.Generic.List<object>(1);
                    ExtensionMetaObjects.Add(WrMethod);
                }
                return ExtensionMetaObjects.ToList();
            }
        }

        /// <MetaDataID>{615ad488-1bc8-4642-ab2b-cf3caee50c32}</MetaDataID>
        bool ParameterLoaded = false;
        /// <MetaDataID>{6e490fd9-1bea-4ac9-a5d1-39e560222bb9}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.Parameter> Parameters
        {
            get
            {
                lock (_Parameters)
                {
                    if (!ParameterLoaded)
                    {
                        foreach (System.Reflection.ParameterInfo parameterInfo in WrMethod.GetParameters())
                        {
                            _Parameters.Add(new Parameter(parameterInfo));
                        }
                        ParameterLoaded = true;

                    }
                    return _Parameters;
                }


            }
        }

        /// <MetaDataID>{2AF4FF31-D95F-42B4-BEDF-40DA9A6C1DB7}</MetaDataID>
        protected Operation()
        {
        }


        /// <MetaDataID>{9B99D586-08F8-45FE-B17C-D664283CA897}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier Owner
        {
            get
            {
                if (base.Owner != null)
                    return base.Owner;

                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    if (_Owner != null)
                        return _Owner;
                    else
                    {
                        _Owner = Type.GetClassifierObject(WrMethod.DeclaringType);
                        return _Owner;
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{BA8600CD-933F-4FBF-92EB-A4DC01E81D91}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Namespace Namespace
        {
            get
            {
                if (base.Namespace != null)
                    return base.Namespace;

                if (_Namespace.Value != null)
                    return _Namespace;
                else
                {
                    _Namespace.Value = Type.GetClassifierObject(WrMethod.DeclaringType);
                    return _Namespace;
                }
            }
        }
        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{0E7AF3C8-9E6E-46E9-941B-8FD9849E1B95}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;
            }
        }




        /// <MetaDataID>{FCC5C06C-45B5-4331-964F-6383637678B2}</MetaDataID>
        public Operation(System.Reflection.MethodBase Method, MetaDataRepository.Classifier ownerClassifier)
        {

            //if(Method.DeclaringType.FullName+"."+Method.Name == "OOAdvantech.RDBMSMetaDataRepository.AutoProduceColumnsGenerator.GetObjectIdentityType")
            //{

            //}
            WrMethod = Method;

            if (WrMethod.IsPublic)
                Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
            if (WrMethod.IsFamily)
                Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
            if (WrMethod.IsPrivate)
                Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
            _IsStatic = WrMethod.IsStatic;


            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(WrMethod, this);
            _Namespace.Value = ownerClassifier;
            _Owner = ownerClassifier;


            _Name = WrMethod.Name;
            if (Method is ConstructorInfo)
                _Name = ownerClassifier.Name;



            object[] attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.ObjectActivationCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "ObjectActivationCall", "True");
            else
                PutPropertyValue("OparetionType", "ObjectActivationCall", "False");


            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.ObjectsLinkCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "ObjectsLinkCall", "True");
            else
                PutPropertyValue("OparetionType", "ObjectsLinkCall", "False");


            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.DeleteObjectCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "DeleteObjectCall", "True");
            else
                PutPropertyValue("OparetionType", "DeleteObjectCall", "False");


            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.DeleteObjectCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "DeleteObjectCall", "True");
            else
                PutPropertyValue("OparetionType", "DeleteObjectCall", "False");

            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.CommitObjectStateInStorageCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "CommitObjectStateInStorageCall", "True");
            else
                PutPropertyValue("OparetionType", "CommitObjectStateInStorageCall", "False");

            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.BeforeCommitObjectStateInStorageCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "BeforeCommitObjectStateInStorageCall", "True");
            else
                PutPropertyValue("OparetionType", "BeforeCommitObjectStateInStorageCall", "False");



            if (_Name.IndexOf('.') != -1)
                _Name = _Name.Substring(_Name.LastIndexOf('.') + 1);



            MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = null;
            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
            if (attributes.Length > 0)
                backwardCompatibilityID = (MetaDataRepository.BackwardCompatibilityID)attributes[0];

            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
            {
                string identityAsString = backwardCompatibilityID.ToString();
                if (identityAsString[0] == '+')
                {
                    identityAsString = identityAsString.Substring(1);
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(Namespace.Identity + "." + identityAsString);
                }
                else
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(identityAsString);
            }
            else
            {
                string StrIdentity = null;
#if DeviceDotNet
                if (!WrMethod.ContainsGenericParameters)
                {
                    foreach (System.Reflection.ParameterInfo Parameter in WrMethod.GetParameters())
                    {
                        if (StrIdentity == null)
                            StrIdentity = "[";
                        else
                            StrIdentity += ",";
                        StrIdentity += Parameter.ParameterType.Name;
                    }
                }
#else
                foreach (System.Reflection.ParameterInfo Parameter in WrMethod.GetParameters())
                {
                    if (StrIdentity == null)
                        StrIdentity = "[";
                    else
                        StrIdentity += ",";
                    StrIdentity += Parameter.ParameterType.Name;

                    if (Parameter.ParameterType.IsGenericType || Parameter.ParameterType.IsGenericTypeDefinition)
                    {
                        StrIdentity += "[";
                        int i = 0;
                        foreach (var parType in Parameter.ParameterType.GetGenericArguments())
                        {
                            if (i != 0)
                                StrIdentity += ",";
                            i++;
                            StrIdentity += parType.Name;
                        }
                        StrIdentity += "]";
                    }
                }
#endif


                string genericArgs = null;
                if (WrMethod is MethodInfo)
                {
                    foreach (System.Type type in WrMethod.GetGenericArguments())
                    {
                        if (genericArgs == null)
                            genericArgs = "<";
                        else
                            genericArgs += ",";
                        if (string.IsNullOrEmpty(type.FullName))
                            genericArgs += type.Name;
                        else
                            genericArgs += type.FullName;
                    }
                }
                if (genericArgs != null)
                    genericArgs += ">";

                if (StrIdentity == null)
                    StrIdentity = WrMethod.Name + genericArgs;
                else
                    StrIdentity = WrMethod.Name + genericArgs + StrIdentity + "]";

                _Identity = new MetaDataRepository.MetaObjectID("O:" + Namespace.Identity.ToString() + "." + StrIdentity);
            }


            //if (WrMethod is System.Reflection.MethodInfo && (WrMethod as System.Reflection.MethodInfo).ReturnType.IsGenericParameter)
            //{
            //    _ParameterizedReturnType = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor((WrMethod as System.Reflection.MethodInfo).ReturnType) as MetaDataRepository.TemplateParameter;
            //    if (_ParameterizedReturnType == null)
            //        _ParameterizedReturnType = new TemplateParameter(new Type((WrMethod as System.Reflection.MethodInfo).ReturnType));


            //}
            //else
            //{
            //    if (WrMethod is System.Reflection.MethodInfo)
            //    {
            //        ReturnType = (MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor((WrMethod as System.Reflection.MethodInfo).ReturnType);
            //        if (ReturnType == null)
            //        {
            //            // Error Prone είναι λάθος όταν πρόκεται για int,long,enum κλπ.
            //            ReturnType = Type.GetClassifierObject((WrMethod as System.Reflection.MethodInfo).ReturnType);
            //        }
            //    }
            //}

        }

        private readonly object ReturnTypeLock = new object();
        public override MetaDataRepository.Classifier ReturnType
        {
            get
            {

                lock (ReturnTypeLock)
                {
                    if (base._ReturnType == null)
                    {
                        if (WrMethod is System.Reflection.MethodInfo && (WrMethod as System.Reflection.MethodInfo).ReturnType.IsGenericParameter)
                        {
                            _ParameterizedReturnType = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor((WrMethod as System.Reflection.MethodInfo).ReturnType) as MetaDataRepository.TemplateParameter;
                            if (_ParameterizedReturnType == null)
                                _ParameterizedReturnType = new TemplateParameter(new Type((WrMethod as System.Reflection.MethodInfo).ReturnType));

                        }
                        else
                        {
                            if (WrMethod is System.Reflection.MethodInfo)
                            {
                                _ReturnType = (MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor((WrMethod as System.Reflection.MethodInfo).ReturnType);
                                if (_ReturnType == null)
                                {
                                    // Error Prone είναι λάθος όταν πρόκεται για int,long,enum κλπ.
                                    _ReturnType = Type.GetClassifierObject((WrMethod as System.Reflection.MethodInfo).ReturnType);
                                }
                            }
                        }

                        if (WrMethod is System.Reflection.MethodInfo && (WrMethod as System.Reflection.MethodInfo).ReturnType.IsGenericParameter)
                            base._ReturnType = Type.GetClassifierObject((WrMethod as System.Reflection.MethodInfo).ReturnType);
                    }
                }

                return base._ReturnType;
            }
            set => base.ReturnType = value;
        }


        public override MetaDataRepository.TemplateParameter ParameterizedReturnType
        {
            get
            {
                lock (ReturnTypeLock)
                {
                    if (_ParameterizedReturnType == null && WrMethod is System.Reflection.MethodInfo && (WrMethod as System.Reflection.MethodInfo).ReturnType.IsGenericParameter)
                    {
                        _ParameterizedReturnType = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor((WrMethod as System.Reflection.MethodInfo).ReturnType) as MetaDataRepository.TemplateParameter;
                        if (_ParameterizedReturnType == null)
                            _ParameterizedReturnType = new TemplateParameter(new Type((WrMethod as System.Reflection.MethodInfo).ReturnType));
                    }
                }
                return _ParameterizedReturnType;
            }

            //=> base.ParameterizedReturnType;
            set => base.ParameterizedReturnType = value;
        }

        /// <MetaDataID>{68976A8E-C39A-489C-995D-695BEBF35695}</MetaDataID>
        public Operation(System.Reflection.MethodBase Method)
        {

            WrMethod = Method;
            MetaObjectMapper.AddTypeMap(WrMethod, this);

            if (WrMethod.IsPublic)
                Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
            if (WrMethod.IsFamily)
                Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
            if (WrMethod.IsPrivate)
                Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
            _IsStatic = WrMethod.IsStatic;

            if (WrMethod.IsAbstract)
                _OverrideKind = MetaDataRepository.OverrideKind.Abstract;

            if (WrMethod.IsVirtual)
                _OverrideKind = MetaDataRepository.OverrideKind.Virtual;


            if ((WrMethod is MethodInfo) && (WrMethod as MethodInfo).GetBaseDefinition() != WrMethod)
                _OverrideKind = MetaDataRepository.OverrideKind.Override;

            if (WrMethod.IsFinal)
                _OverrideKind = MetaDataRepository.OverrideKind.Sealed;

            

            object[] attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.ObjectActivationCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "ObjectActivationCall", "True");
            else
                PutPropertyValue("OparetionType", "ObjectActivationCall", "False");

            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.ObjectsLinkCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "ObjectsLinkCall", "True");
            else
                PutPropertyValue("OparetionType", "ObjectsLinkCall", "False");

            


            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.DeleteObjectCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "DeleteObjectCall", "True");
            else
                PutPropertyValue("OparetionType", "DeleteObjectCall", "False");


            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.CommitObjectStateInStorageCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "CommitObjectStateInStorageCall", "True");
            else
                PutPropertyValue("OparetionType", "CommitObjectStateInStorageCall", "False");



            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.BeforeCommitObjectStateInStorageCall), false);
            if (attributes.Length > 0)
                PutPropertyValue("OparetionType", "BeforeCommitObjectStateInStorageCall", "True");
            else
                PutPropertyValue("OparetionType", "BeforeCommitObjectStateInStorageCall", "False");



            _Name = WrMethod.Name;
            if (_Name.IndexOf('.') != -1)
                _Name = _Name.Substring(_Name.LastIndexOf('.') + 1);
            if (Method is System.Reflection.ConstructorInfo)
                _Name = Method.DeclaringType.Name;

            MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = null;
            attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
            if (attributes.Length > 0)
                backwardCompatibilityID = (MetaDataRepository.BackwardCompatibilityID)attributes[0];

            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
            {
                string identityAsString = backwardCompatibilityID.ToString();
                if (identityAsString[0] == '+')
                {
                    identityAsString = identityAsString.Substring(1);
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(Namespace.Identity + "." + identityAsString);
                }
                else
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(identityAsString);
            }
            else
            {
                string StrIdentity = null;
                foreach (System.Reflection.ParameterInfo Parameter in WrMethod.GetParameters())
                {
                    if (StrIdentity == null)
                        StrIdentity = "[";
                    else
                        StrIdentity += ",";
                    StrIdentity += Parameter.ParameterType.Name;
                }
                if (StrIdentity == null)
                    StrIdentity = WrMethod.Name;
                else
                    StrIdentity = WrMethod.Name + StrIdentity + "]";

                _Identity = new MetaDataRepository.MetaObjectID("O:" + Namespace.Identity.ToString() + "." + StrIdentity);
            }
            if (WrMethod.DeclaringType.FullName != null)
                MetaObjectMapper.AddMetaObject(this, WrMethod.DeclaringType.FullName + "." + WrMethod.Name);



            //if (WrMethod is System.Reflection.MethodInfo && (WrMethod as System.Reflection.MethodInfo).ReturnType.IsGenericParameter)
            //{
            //    _ParameterizedReturnType = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor((WrMethod as System.Reflection.MethodInfo).ReturnType) as MetaDataRepository.TemplateParameter;
            //    if (_ParameterizedReturnType == null)
            //        _ParameterizedReturnType = new TemplateParameter(new Type((WrMethod as System.Reflection.MethodInfo).ReturnType));

            //}
            //else
            //{
            //    if (WrMethod is System.Reflection.MethodInfo)
            //        ReturnType = Type.GetClassifierObject((WrMethod as System.Reflection.MethodInfo).ReturnType);
            //}


        }


        /// <MetaDataID>{9C906243-8508-4D3C-B65C-28DA64214AE5}</MetaDataID>
        internal System.Reflection.MethodBase WrMethod;


    }
}

