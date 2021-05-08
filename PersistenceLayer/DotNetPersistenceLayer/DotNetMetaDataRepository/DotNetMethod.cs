namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{68A07919-C575-4BC6-99C7-D8CEAA70DBCD}</MetaDataID>
    public class Method : MetaDataRepository.Method
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(WrMethod))
            {
                if (value == null)
                    WrMethod = default(System.Reflection.MethodInfo);
                else
                    WrMethod = (System.Reflection.MethodInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(WrMethod))
                return WrMethod;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{880f25ed-8108-4ec4-80c1-66b095370818}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }
        /// <MetaDataID>{17090585-9775-482F-92C4-EB1BC9AAC15D}</MetaDataID>
        protected Method()
        {
        }

        /// <MetaDataID>{78357ED3-AB40-4525-B3BB-1D7976904378}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier Owner
        {
            get
            {
                if (base.Owner != null)
                    return base.Owner;
                if (_Owner != null)
                    return _Owner;
                else
                {
                    _Owner = Type.GetClassifierObject(WrMethod.DeclaringType);
                    return _Owner;
                }
            }
        }
        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{2DD3CD5B-5207-4B29-86A0-C2613F5A53CA}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;

            }
        }
        /// <MetaDataID>{7A105D11-F94A-488D-AA0E-66E20B3DFAF3}</MetaDataID>
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




        /// <MetaDataID>{AE37DE86-C887-43DD-93AB-648712CD89C2}</MetaDataID>
        public Method(System.Reflection.MethodInfo method, Operation operation)
        {
            WrMethod = method;
            _Name = WrMethod.Name;
            _Specification = operation;
            operation.AddOperationImplementation(this);

            if (WrMethod.IsAbstract)
                _OverrideKind = MetaDataRepository.OverrideKind.Abstract;

            if (WrMethod.IsVirtual)
                _OverrideKind = MetaDataRepository.OverrideKind.Virtual;


            if (WrMethod.GetBaseDefinition() != WrMethod)
                _OverrideKind = MetaDataRepository.OverrideKind.Override;

            if (WrMethod.IsFinal)
                _OverrideKind = MetaDataRepository.OverrideKind.Sealed;


            MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = null;
            object[] attributes = WrMethod.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
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

                    if (Parameter.ParameterType.GetMetaData().IsGenericType)
                    {
                        string genericParam = null;
                        foreach (var type in Parameter.ParameterType.GetMetaData().GetGenericArguments())
                        {
                            if (genericParam != null)
                                genericParam += ",";
                            else
                                genericParam = "[";
                            genericParam += type.Name;
                        }
                        genericParam += "]";
                        StrIdentity += genericParam;
                    }
                }

                string genericArgs = null;
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
                if (genericArgs != null)
                    genericArgs += ">";

                if (StrIdentity == null)
                    StrIdentity = WrMethod.Name + genericArgs;
                else
                    StrIdentity = WrMethod.Name + genericArgs + StrIdentity + "]";

                _Identity = new MetaDataRepository.MetaObjectID("M:" + Namespace.Identity.ToString() + "." + StrIdentity);

            }


            if (WrMethod.DeclaringType.FullName != null)
            {
                string signature = "";
                foreach (var parameter in WrMethod.GetParameters())
                {
                    if (!string.IsNullOrEmpty(signature))
                        signature += ",";
                    signature += parameter.ParameterType.FullName;
                }

                MetaObjectMapper.AddMetaObject(this, WrMethod.DeclaringType.FullName + "." + WrMethod.Name + signature);
            }


        }


        /// <MetaDataID>{8AC4F8D1-5EB3-41BC-ADDA-3C16173184DD}</MetaDataID>
        internal System.Reflection.MethodInfo WrMethod;
    }
}
