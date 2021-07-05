using System;
using OOAdvantech.Collections.Generic;

namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{F4464283-7D7F-44DC-97EB-0B3FAE62C46E}</MetaDataID>
    public class AssociationEndRealization : OOAdvantech.MetaDataRepository.AssociationEndRealization
    {
        public override object GetObjectStateValue(object _object)
        {
            if (FastFieldAccessor != null)
                return FastFieldAccessor.GetValue(_object);

            if (FastPropertyAccessor != null)
                return FastPropertyAccessor.GetValue(_object);
            throw new NotImplementedException();
        }

        public override void SetObjectStateValue(object _object, object value)
        {
            if (FastFieldAccessor != null)
                FastFieldAccessor.SetValue(_object, value);

            if (FastPropertyAccessor != null)
                FastPropertyAccessor.SetValue(_object, value);
        }
        public override object GetValue(object _object)
        {

            if (FastPropertyAccessor!= null)
                return FastPropertyAccessor.GetValue(_object);

            if (FastFieldAccessor != null)
                return FastFieldAccessor.GetValue(_object);

            throw new NotImplementedException();
        }

        public override void SetValue(object _object, object value)
        {
         
            if (FastPropertyAccessor != null)
                FastPropertyAccessor.SetValue(_object, value);
            if (FastFieldAccessor != null)
                FastFieldAccessor.SetValue(_object, value);

        }
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(InErrorCheck))
            {
                if (value == null)
                    InErrorCheck = default(bool);
                else
                    InErrorCheck = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(FieldMemberLoaded))
            {
                if (value == null)
                    FieldMemberLoaded = default(bool);
                else
                    FieldMemberLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_FieldMember))
            {
                if (value == null)
                    _FieldMember = default(System.Reflection.FieldInfo);
                else
                    _FieldMember = (System.Reflection.FieldInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_FastFieldAccessor))
            {
                if (value == null)
                    _FastFieldAccessor = default(OOAdvantech.AccessorBuilder.FieldPropertyAccessor);
                else
                    _FastFieldAccessor = (OOAdvantech.AccessorBuilder.FieldPropertyAccessor)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_FastPropertyAccessor))
            {
                if (value == null)
                    _FastPropertyAccessor = default(OOAdvantech.AccessorBuilder.FieldPropertyAccessor);
                else
                    _FastPropertyAccessor = (OOAdvantech.AccessorBuilder.FieldPropertyAccessor)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_PropertyMember))
            {
                if (value == null)
                    _PropertyMember = default(System.Reflection.PropertyInfo);
                else
                    _PropertyMember = (System.Reflection.PropertyInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(IgnoreErrorCheck))
            {
                if (value == null)
                    IgnoreErrorCheck = default(bool);
                else
                    IgnoreErrorCheck = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(FieldMemberLoaded))
                return FieldMemberLoaded;

            if (member.Name == nameof(_FieldMember))
                return _FieldMember;

            if (member.Name == nameof(_FastFieldAccessor))
                return _FastFieldAccessor;

            if (member.Name == nameof(_FastPropertyAccessor))
                return _FastPropertyAccessor;

            if (member.Name == nameof(_PropertyMember))
                return _PropertyMember;

            if (member.Name == nameof(IgnoreErrorCheck))
                return IgnoreErrorCheck;


            return base.GetMemberValue(token, member);
        }

        public override object GetExtensionMetaObject(System.Type MetaObjectType)
        {

            if (MetaObjectType == typeof(System.Reflection.FieldInfo) || MetaObjectType.GetMetaData().IsSubclassOf(typeof(System.Reflection.FieldInfo)))
                return FieldMember;

            if (MetaObjectType == typeof(System.Reflection.PropertyInfo) || MetaObjectType.GetMetaData().IsSubclassOf(typeof(System.Reflection.PropertyInfo)))
                return PropertyMember;

            return base.GetExtensionMetaObject(MetaObjectType);
        }

        /// <MetaDataID>{2d7f6a2a-2fad-4359-8c09-9910c530dadc}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        { 
            //base.Synchronize(OriginMetaObject);
        }
        /// <MetaDataID>{0D6FBAEA-9534-49B9-989E-CC15BA2388A7}</MetaDataID>
        protected AssociationEndRealization()
        {  
        }
        /// <MetaDataID>{F7A09A12-F2AC-4D96-83F6-FCCB9EA36AA6}</MetaDataID>
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
                    _Owner = Type.GetClassifierObject(PropertyMember.DeclaringType);
                    return _Owner;
                }
            }
        }


        public override bool Multilingual
        {
            get
            {
                if (FieldMember != null && FieldMember.FieldType.IsGenericType &&
                    (FieldMember.FieldType.GetGenericTypeDefinition() == typeof(MultilingualMember<>) || FieldMember.FieldType.GetGenericTypeDefinition() == typeof(MultilingualSet<>)))
                    return true;
                return base.Multilingual;
            }
            set => base.Multilingual = value;
        }

        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{53AB956F-8304-4371-B55E-B2B6938B6433}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                return _Identity;
            }
        }
        /// <MetaDataID>{FED1FC74-7DC1-40E8-AD81-34C8A6AD10F7}</MetaDataID>
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
                    _Namespace.Value = DotNetMetaDataRepository.Type.GetClassifierObject(PropertyMember.DeclaringType);
                    return _Namespace;
                }
            }
        }

        /// <MetaDataID>{4DF05CD5-D5BE-4B84-A2BD-5F98E9310004}</MetaDataID>
        private bool InErrorCheck = false;
        /// <MetaDataID>{4A6754A2-88CE-4E77-9F88-F0AFD5CE5465}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {
           
            if (InErrorCheck || IgnoreErrorCheck)
                return false;
            try
            {
                InErrorCheck = true;
                bool hasError = base.ErrorCheck(ref errors);
                if (_Persistent||(Specification.Persistent==true&&!(Namespace as Class).Abstract))
                {
                    object[] customAttributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), false);
                    if (customAttributes.Length > 0)
                    {
                        try
                        {
                            System.Reflection.FieldInfo fieldMember = (customAttributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(_PropertyMember);

                        }
                        catch (System.Exception error)
                        {
                            hasError = true;
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + error.Message, FullName));
                        }
                    }
                    System.Reflection.FieldInfo fieldInfo = null;
                    try
                    {
                        fieldInfo = FieldMember;
                    }
                    catch (System.Exception error)
                    {
                    }
                    if (fieldInfo == null)
                    {
                        if (Namespace is Class)
                        {
                            try
                            {
                                OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = (Namespace as Class).GetFastFieldAccessor(Specification as AssociationEnd);
                            }
                            catch (System.Exception error)
                            {
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + error.Message, FullName));
                                hasError = true;
                            }
                        }

                        if (Namespace is Structure)
                        {
                            try
                            {
                                OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = (Namespace as Structure).GetFastFieldAccessor(Specification as AssociationEnd);
                            }
                            catch (System.Exception error)
                            {
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + error.Message, FullName));
                                hasError = true;
                            }
                        }
                    }

                    if (Specification.Multiplicity.IsMany && !PropertyMember.PropertyType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                    {
                        if (fieldInfo == null)
                        {
                            hasError = true;
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The multiplicity of association end '" + Specification.GetOtherEnd().Specification.FullName + "." + Name +
                                    "' is many and the type isn't collection subclass of 'OOAdvantech.PersistenceLayer.ObjectContainer", FullName));
                        }
                        else
                        {
                            if(!fieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                            {
                                hasError = true;
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The multiplicity of association end '" + Specification.GetOtherEnd().Specification.FullName + "." + Name +
                                        "' is many and the type isn't collection subclass of 'OOAdvantech.PersistenceLayer.ObjectContainer", FullName));

                            }

                        }
                    }
                    if (Specification.HasBehavioralSettings && this.HasBehavioralSettings)
                    {
                        hasError = true;
                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: You can't declare BehavioralSettings at '" +
                            PropertyMember.DeclaringType.FullName + "." + PropertyMember.Name +
                            "' because the BehavioralSettings already declared at association end '" +
                            (_Specification as AssociationEnd).PropertyMember.DeclaringType.FullName + "." + (_Specification as AssociationEnd).PropertyMember.Name +
                            "' and can't be change.", FullName));
                    }


                    if (_PropertyMember != null && fieldInfo != null)
                    {
                        System.Type fieldType = fieldInfo.FieldType;
                        if (fieldType.GetMetaData().IsGenericType)
                        {
                            if (fieldType.GetGenericTypeDefinition() == typeof(OOAdvantech.Member<>))
                                fieldType = fieldType.GetMetaData().GetGenericArguments()[0];
                        }

                        if (fieldType != typeof(object) && fieldType != (_PropertyMember as System.Reflection.PropertyInfo).PropertyType)
                        {
                            if (fieldType != (_PropertyMember as System.Reflection.PropertyInfo).PropertyType)
                            {
                                if (TypeHelper.GetElementType(fieldType) != TypeHelper.GetElementType((_PropertyMember as System.Reflection.PropertyInfo).PropertyType))
                                {
                                    if (!MetaDataRepository.Classifier.GetClassifier(fieldType).IsA(MetaDataRepository.Classifier.GetClassifier((_PropertyMember as System.Reflection.PropertyInfo).PropertyType)))
                                    {

                                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "Type mismatch between property '" + _PropertyMember.DeclaringType + "." + _PropertyMember.Name +
                                    "' and implementation field '" + fieldInfo.DeclaringType + "." + fieldInfo.Name + "'.", FullName));
                                        return true;
                                    }
                                }
                            }



                            if (fieldType.GetMetaData().IsInterface)
                            {
                                System.Collections.Generic.List<System.Type> interfaces = new System.Collections.Generic.List<System.Type>((_PropertyMember as System.Reflection.PropertyInfo).PropertyType.GetMetaData().GetInterfaces());
                                if (!interfaces.Contains(fieldType))
                                {
                                    errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "The types of property '" + _PropertyMember.DeclaringType + "." + _PropertyMember.Name +
                                        "' and implementation field '" + fieldInfo.DeclaringType + "." + fieldInfo.Name + "' are incompatible.", FullName));
                                    return true;
                                }
                            }
                            else
                            {
                                if (fieldType.GetMetaData().IsValueType)
                                    if (!(_PropertyMember as System.Reflection.PropertyInfo).PropertyType.GetMetaData().IsSubclassOf(fieldType))
                                    {
                                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "The types of property '" + _PropertyMember.DeclaringType + "." + _PropertyMember.Name +
                                            "' and implementation field '" + fieldInfo.DeclaringType + "." + fieldInfo.Name + "' are incompatible.", FullName));
                                        return true;
                                    }
                            }
                        }
                    }

                }
                return hasError;
            }
            finally
            {
                InErrorCheck = false;
            }
        }




        /// <MetaDataID>{BCBCE805-3F65-438F-8A19-A30B5F2B88E8}</MetaDataID>
        private bool FieldMemberLoaded = false;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{224E415C-0C83-42C4-B2B4-0516C50CA707}</MetaDataID>
        private System.Reflection.FieldInfo _FieldMember;
        /// <MetaDataID>{8EAE04CF-082E-40A0-A9F2-1766C272E303}</MetaDataID>
        public System.Reflection.FieldInfo FieldMember
        {
            get
            {
                if (_FieldMember != null)
                    return _FieldMember;
                object[] customAttributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), false);
                if (customAttributes.Length > 0)
                {
                    //TODO τί γίνεται όταν το implementation field είναι δηλωμένο λάθος.
                    _FieldMember = (customAttributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(_PropertyMember);
                    _Persistent = true;
                }
                else
                {
                    customAttributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.ImplementationMember), false);
                    if (customAttributes.Length > 0)
                    {
                        //TODO τί γίνεται όταν το implementation field είναι δηλωμένο λάθος.
                        _FieldMember = (customAttributes[0] as MetaDataRepository.ImplementationMember).GetImplementationField(_PropertyMember);
                        //_Persistent = true;
                    }
                }
                return _FieldMember;
            }
        }


        /// <exclude>Excluded</exclude>
        AccessorBuilder.FieldPropertyAccessor _FastFieldAccessor;
        /// <MetaDataID>{0d0cfb68-1de8-45cc-9984-9f904c63d0a9}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor FastFieldAccessor
        {
            get
            {
                if (_FastFieldAccessor != null)
                    return _FastFieldAccessor;
                else
                {
                    if (FieldMember == null)
                        return _FastFieldAccessor;
                    _FastFieldAccessor = AccessorBuilder.GetFieldAccessor(FieldMember);
                    return _FastFieldAccessor;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        AccessorBuilder.FieldPropertyAccessor _FastPropertyAccessor;
        /// <MetaDataID>{7f27b5f5-20b5-49fa-9081-1a0bdecec7ef}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor FastPropertyAccessor
        {
            get
            {
                if (_FastPropertyAccessor != null)
                    return _FastPropertyAccessor;
                else
                {
                    if (PropertyMember == null)
                        return _FastPropertyAccessor;
                    _FastPropertyAccessor = AccessorBuilder.GetPropertyAccessor(PropertyMember);
                    return _FastPropertyAccessor;
                }
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5D5BE2D2-8D7B-4A7D-AAC7-938057BBE98A}</MetaDataID>
        private System.Reflection.PropertyInfo _PropertyMember;
        /// <MetaDataID>{F92F0D37-60CC-4E37-BC96-F6E1E243335F}</MetaDataID>
        public System.Reflection.PropertyInfo PropertyMember
        {
            get
            {
                return _PropertyMember;
            }
        }

        /// <MetaDataID>{1021e032-0736-4f9f-ab7f-e1ae39c611be}</MetaDataID>
        bool IgnoreErrorCheck;
        /// <MetaDataID>{DDA462D7-9508-4BFE-B423-9CCAFACFE21B}</MetaDataID>
        public AssociationEndRealization(System.Reflection.PropertyInfo property, AssociationEnd associationEnd, MetaDataRepository.Classifier owner)
        {
            _Owner = owner;
            _Namespace.Value = owner;
            _Name = associationEnd.Name;
            _Specification = associationEnd;
            associationEnd.AddAssociationEndRealization(this);
            
            if (property != null)
            {
                if (property.GetCustomAttributes(typeof(MetaDataRepository.IgnoreErrorCheckAttribute), true).Length > 0)
                    IgnoreErrorCheck = true;
            }

            _PropertyMember = property;
            DotNetMetaDataRepository.MetaObjectMapper.RemoveType(_PropertyMember);
            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(_PropertyMember, this);
            if(property!=null)
                _Name = property.Name;
            TransactionalMember = _PropertyMember.GetCustomAttributes(typeof(OOAdvantech.Transactions.TransactionalMemberAttribute), true).Length > 0;

            //MetaDataRepository.PersistencyFlag persistencyFlag= (associationEnd as DotNetMetaDataRepository.AssociationEnd).HasBehavioralSettings

            object[] customAttributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.AssociationEndBehavior), false);
            object[] parentCustomAttributes = (associationEnd as DotNetMetaDataRepository.AssociationEnd).WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationEndBehavior), false);
            if (customAttributes.Length != 0)
            {
                MetaDataRepository.PersistencyFlag persistencyFlag = (customAttributes[0] as MetaDataRepository.AssociationEndBehavior).PersistencyFlag;
                bool _override=false;
                if ((uint)(persistencyFlag & MetaDataRepository.PersistencyFlag.Override) != 0)
                    _override = true;
                else if (parentCustomAttributes.Length != 0)
                {
                    MetaDataRepository.PersistencyFlag parentPersistencyFlag = (parentCustomAttributes[0] as MetaDataRepository.AssociationEndBehavior).PersistencyFlag;
                    persistencyFlag = persistencyFlag | parentPersistencyFlag;
                }


                    _HasBehavioralSettings = true;
                if ((uint)(persistencyFlag & MetaDataRepository.PersistencyFlag.ReferentialIntegrity) != 0)
                    _ReferentialIntegrity = true;
                else
                {
                    _ReferentialIntegrity = false;
                    if (!_override&& (associationEnd as DotNetMetaDataRepository.AssociationEnd).HasBehavioralSettings)
                        ReferentialIntegrity = (associationEnd as DotNetMetaDataRepository.AssociationEnd).ReferentialIntegrity;
                }

                if ((uint)(persistencyFlag & MetaDataRepository.PersistencyFlag.CascadeDelete) != 0)
                    _CascadeDelete = true;
                else
                {
                    _CascadeDelete = false;
                    if (!_override && (associationEnd as DotNetMetaDataRepository.AssociationEnd).HasBehavioralSettings)
                        _CascadeDelete = (associationEnd as DotNetMetaDataRepository.AssociationEnd).CascadeDelete;
                }

                if ((uint)(persistencyFlag & MetaDataRepository.PersistencyFlag.AllowTransient) != 0)
                    _AllowTransient = true;
                else
                {
                    _AllowTransient = false;
                    if (!_override && (associationEnd as DotNetMetaDataRepository.AssociationEnd).HasBehavioralSettings)
                        _AllowTransient = (associationEnd as DotNetMetaDataRepository.AssociationEnd).AllowTransient;
                }


                if ((uint)(persistencyFlag & MetaDataRepository.PersistencyFlag.OnConstruction) != 0)
                    _LazyFetching = false;
                else
                {
                    _LazyFetching = true;
                    if (!_override && (associationEnd as DotNetMetaDataRepository.AssociationEnd).HasBehavioralSettings)
                        _LazyFetching = (associationEnd as DotNetMetaDataRepository.AssociationEnd).LazyFetching;


                }

                if ((uint)(persistencyFlag & MetaDataRepository.PersistencyFlag.OnConstruction) != 0 &&
                (uint)(persistencyFlag & MetaDataRepository.PersistencyFlag.LazyFetching) != 0)
                {
                    _TryOnObjectActivationFetching = true;
                }
            }
            else
                _HasBehavioralSettings = false;
             

            customAttributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), false);
            if (customAttributes.Length > 0)
                _Persistent = true;

            MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = null;
            object[] attributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
            if (attributes.Length > 0)
                backwardCompatibilityID = (MetaDataRepository.BackwardCompatibilityID)attributes[0];
            if ( backwardCompatibilityID != null&& !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
            {
                string identityAsString = backwardCompatibilityID.ToString();
                if (identityAsString[0] == '+')
                {
                    identityAsString = identityAsString.Substring(1);
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("AsR_" + Namespace.Identity + "." + identityAsString);
                }
                else
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("AsR_" + identityAsString);
            }
            else
            {
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("AsR_"+Namespace.Identity + "." + PropertyMember.Name);
            }
            if (_PropertyMember.DeclaringType.FullName != null)
                MetaObjectMapper.AddMetaObject(this, _PropertyMember.DeclaringType.FullName + "." + _PropertyMember.Name);

        }
    }
}
