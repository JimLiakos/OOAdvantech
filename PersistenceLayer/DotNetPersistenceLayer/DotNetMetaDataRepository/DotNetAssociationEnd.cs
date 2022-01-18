using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using System;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;

#endif
using System.Linq;
namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{C9BE7F05-C134-4AFE-BDB5-4334236B2D68}</MetaDataID>
    public class AssociationEnd : MetaDataRepository.AssociationEnd
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
            if (FastPropertyAccessor != null)
                return FastPropertyAccessor.GetValue(_object);

            if (FastFieldAccessor != null)
                return FastFieldAccessor.GetValue(_object);


            throw new NotImplementedException();
        }

        bool? _Multilingual;
        public override bool Multilingual
        {
            get
            {
                if (_Multilingual != null)
                    return _Multilingual.Value;
                if (FieldMember != null && FieldMember.FieldType.IsGenericType &&
                    (FieldMember.FieldType.GetGenericTypeDefinition() == typeof(MultilingualMember<>) || FieldMember.FieldType.GetGenericTypeDefinition() == typeof(MultilingualSet<>)))
                {
                    _Multilingual = true;
                    return true;
                }
                _Multilingual= base.Multilingual;
                return _Multilingual.Value;
            }
            set => base.Multilingual = value;
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
            if (member.Name == nameof(WrMemberInfo))
            {
                if (value == null)
                    WrMemberInfo = default(System.Reflection.MemberInfo);
                else
                    WrMemberInfo = (System.Reflection.MemberInfo)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_SpecificationType))
            {
                if (value == null)
                    _SpecificationType = default(System.Type);
                else
                    _SpecificationType = (System.Type)value;
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
            if (member.Name == nameof(Accessors))
            {
                if (value == null)
                    Accessors = default(System.Reflection.MethodInfo[]);
                else
                    Accessors = (System.Reflection.MethodInfo[])value;
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
            if (member.Name == nameof(InitHasAssociationEndRealizations))
            {
                if (value == null)
                    InitHasAssociationEndRealizations = default(bool);
                else
                    InitHasAssociationEndRealizations = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_HasAssociationEndRealizations))
            {
                if (value == null)
                    _HasAssociationEndRealizations = default(bool);
                else
                    _HasAssociationEndRealizations = (bool)value;
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
            if (member.Name == nameof(AutoGeneratedIdentity))
            {
                if (value == null)
                    AutoGeneratedIdentity = default(bool);
                else
                    AutoGeneratedIdentity = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }


        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(WrMemberInfo))
                return WrMemberInfo;

            if (member.Name == nameof(_SpecificationType))
                return _SpecificationType;

            if (member.Name == nameof(_FieldMember))
                return _FieldMember;

            if (member.Name == nameof(ExtensionMetaObjects))
                return GetExtensionMetaObjects();

            if (member.Name == nameof(Accessors))
                return Accessors;

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(_FastFieldAccessor))
                return _FastFieldAccessor;

            if (member.Name == nameof(_FastPropertyAccessor))
                return _FastPropertyAccessor;

            if (member.Name == nameof(InitHasAssociationEndRealizations))
                return InitHasAssociationEndRealizations;

            if (member.Name == nameof(_HasAssociationEndRealizations))
                return _HasAssociationEndRealizations;

            if (member.Name == nameof(IgnoreErrorCheck))
                return IgnoreErrorCheck;

            if (member.Name == nameof(AutoGeneratedIdentity))
                return AutoGeneratedIdentity;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{2490c096-27b4-4e37-a1a4-760b46fd52ad}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        public override void ShallowSynchronize(MetaObject originClassifier)
        {

        }

        /// <MetaDataID>{677dcefe-06ae-44b8-b08a-ecd5c1856fb3}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier CollectionClassifier
        {
            get
            {
                return base.CollectionClassifier;
            }
        }

        /// <MetaDataID>{6a2ec6ac-1261-472d-9012-89b1ad1fc42d}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Association Association
        {
            get
            {
                return _Association;
            }
        }
        /// <MetaDataID>{6DA7F513-567D-4813-BAF9-2EAEB925021E}</MetaDataID>
        protected AssociationEnd()
        {
        }
        /// <MetaDataID>{D7CC5986-0F7D-4A61-B54A-740EF2A45E8A}</MetaDataID>
        internal System.Reflection.MemberInfo WrMemberInfo;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{F7577E49-7084-4888-A5BE-720E47AB004A}</MetaDataID>
        private System.Type _SpecificationType;
        /// <MetaDataID>{0992D446-70BA-4F33-988D-B96432BF4E66}</MetaDataID>
        public System.Type SpecificationType
        {
            get
            {
                if (_SpecificationType != null)
                    return _SpecificationType;
                else
                {
                    _SpecificationType = Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                    return _SpecificationType;
                }
            }
        }


        /// <MetaDataID>{E05AB6F1-598D-42A9-A4C0-018658B712A9}</MetaDataID>
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
                    _Namespace.Value = DotNetMetaDataRepository.Type.GetClassifierObject(WrMemberInfo.DeclaringType);
                    return _Namespace;
                }

            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B86CE1BB-7731-4049-8111-C8ABA35F72FA}</MetaDataID>
        private System.Reflection.FieldInfo _FieldMember;
        /// <MetaDataID>{6A934F5D-8EA1-4D21-A017-F59EE814223D}</MetaDataID>
        public System.Reflection.FieldInfo FieldMember
        {
            get
            {
                if (WrMemberInfo == null)
                    return null;
                if (_FieldMember != null)
                    return _FieldMember;
                if (WrMemberInfo is System.Reflection.FieldInfo)
                {
                    _FieldMember = WrMemberInfo as System.Reflection.FieldInfo;
                    return _FieldMember;
                }
                object[] Attributes = WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), false);
                if (Attributes.Length > 0)
                {
                    //TODO τί γίνεται όταν το implementation field είναι δηλωμένο λάθος.

                    _FieldMember = (Attributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(WrMemberInfo as System.Reflection.PropertyInfo);
                    return _FieldMember;
                }
                return null;

            }
        }

        /// <MetaDataID>{B4D1C221-3185-47DC-BF2E-192C28AF3EBE}</MetaDataID>
        private System.Collections.Generic.List<object> ExtensionMetaObjects;
        /// <MetaDataID>{8C1205AF-2EBA-4CBF-B59B-90C1E8671A02}</MetaDataID>
        public System.Reflection.MethodInfo[] Accessors;



        //		/// <exclude>Excluded</exclude>
        //		/// <MetaDataID>{7E655763-ABEB-44C4-B5CF-5B377C85C23C}</MetaDataID>
        //		private Type _SpecificationType;
        //		/// <MetaDataID>{D1EC6903-A7A5-4780-8612-199BF999AED9}</MetaDataID>
        //		public Type SpecificationType
        //		{
        //
        //			get
        //			{
        //				if(_SpecificationType!=null)
        //					return _SpecificationType;
        //				else
        //				{
        //					_SpecificationType=Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
        //					return _SpecificationType;
        //				}
        //			}
        //		}
        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{A069617F-8313-4F0D-A911-E2DFFD0E82E0}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                //TODO τι γίνεται όταν για κάποιο λόγο αλλάξη το identity το association end
                //και δεν άλλαξη το identity το association πχ όταν από το ένα μέρος
                //μιας association "RoleA" δεν δηλώνεται field ή property τότε το association end
                //θα πάρει ένα autogenerated identity. Ομως σε μια προσεχές αναβάθμιση δηλώσουμε 
                //field ή property με BackwardCompatibilityID τότε έχουμε πρόβλημα με τα Identities

                return _Identity;
            }
        }


        /// <MetaDataID>{E27613A6-2CC9-4459-8923-661633A36402}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MultiplicityRange Multiplicity
        {
            get
            {
                return _Multiplicity;
            }
        }


        /// <MetaDataID>{17537DE3-78C4-46E7-AD76-F87CF5FFB9E7}</MetaDataID>
        private bool InErrorCheck = false;
        /// <MetaDataID>{80EB5383-77FE-4E0E-8F8B-4238CAC72DE1}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
        {
            if (InErrorCheck || IgnoreErrorCheck)
                return false;
            try
            {


                InErrorCheck = true;
                bool hasError = base.ErrorCheck(ref errors);
                System.Type memberType;
                if (WrMemberInfo != null)
                {

                    if (WrMemberInfo is System.Reflection.FieldInfo)
                        memberType = (WrMemberInfo as System.Reflection.FieldInfo).FieldType;
                    else
                        memberType = (WrMemberInfo as System.Reflection.PropertyInfo).PropertyType;

                    if (FastFieldAccessor != null && FastFieldAccessor.MemberInfo is System.Reflection.FieldInfo)
                        memberType = (FastFieldAccessor.MemberInfo as System.Reflection.FieldInfo).FieldType;
                    if (memberType.GetMetaData().IsGenericType && memberType.GetGenericTypeDefinition() == typeof(OOAdvantech.Member<>))
                        memberType = memberType.GetMetaData().GetGenericArguments()[0];

                    object[] customAttributes = null;

                    #region Declaration check
                    if (Accessors != null && Accessors.Length > 0)
                    {
                        MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(WrMemberInfo.DeclaringType) as MetaDataRepository.Classifier;
                        Type type = null;
                        if (classifier is Interface)
                            type = (classifier as Interface).Refer;

                        if (classifier is Class)
                            type = (classifier as Class).Refer;

                        if (classifier is Structure)
                            type = (classifier as Structure).Refer;


                        System.Reflection.MemberInfo specificationMethod = type.GetOperationForMethod(Accessors[0]);
                        if (specificationMethod != Accessors[0])
                        {
                            if (specificationMethod.DeclaringType.GetMetaData().IsClass)
                            {
                                hasError = true;
                                if (this.PropertyMember != null)
                                    errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: Declaration error on Association end '" + GetOtherEnd().Specification.FullName + "." + Name + "'. We can't declare an override property as association end.", GetOtherEnd().Specification.FullName + "." + Name));

                            }
                            else if (specificationMethod.DeclaringType.GetMetaData().IsInterface)
                            {
                                hasError = true;
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: Declaration error on Association end '" + GetOtherEnd().Specification.FullName + "." + Name + "." + Name + "'. We can't declare an interface implementation property as association end.", GetOtherEnd().Specification.FullName + "." + Name));
                            }
                        }

                    }
                    if (Association.Name == null || Association.Name.Trim().Length == 0)
                    {
                        if (WrMemberInfo != null)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: There isn't Association Name for class member " + WrMemberInfo.DeclaringType.FullName + "." + WrMemberInfo.Name, GetOtherEnd().Specification.FullName + "." + Name));
                            hasError = true;
                        }
                    }



                    customAttributes = WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationAttribute), false);
                    if (customAttributes.Length > 0)
                    {
                        MetaDataRepository.AssociationAttribute associationAttribute = customAttributes[0] as MetaDataRepository.AssociationAttribute;
                        if ((GetOtherEnd() as AssociationEnd).WrMemberInfo != null)
                        {
                            customAttributes = (GetOtherEnd() as AssociationEnd).WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationAttribute), false);
                            if (customAttributes.Length > 0)
                            {
                                if (associationAttribute.IsRoleA == (customAttributes[0] as MetaDataRepository.AssociationAttribute).IsRoleA)
                                {
                                    hasError = true;
                                    errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The association ends '" + WrMemberInfo.DeclaringType.FullName + "." + WrMemberInfo.Name + "' and '" + (GetOtherEnd() as AssociationEnd).WrMemberInfo.DeclaringType.FullName + "." + (GetOtherEnd() as AssociationEnd).WrMemberInfo.Name + "' has the same role.", GetOtherEnd().Specification.FullName + "." + Name));
                                }
                                if (associationAttribute.Identity != (customAttributes[0] as MetaDataRepository.AssociationAttribute).Identity)
                                {
                                    hasError = true;
                                    errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The association ends '" + WrMemberInfo.DeclaringType.FullName + "." + WrMemberInfo.Name + "' and '" + (GetOtherEnd() as AssociationEnd).WrMemberInfo.DeclaringType.FullName + "." + (GetOtherEnd() as AssociationEnd).WrMemberInfo.Name + "' has different decalaration for Association Identity.", GetOtherEnd().Specification.FullName + "." + Name));
                                }
                            }
                        }
                    }

                    if (WrMemberInfo is System.Reflection.PropertyInfo)
                    {
                        customAttributes = WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), false);
                        if (customAttributes.Length > 0)
                        {
                            try
                            {
                                System.Reflection.FieldInfo fieldMember = (customAttributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(WrMemberInfo as System.Reflection.PropertyInfo);

                            }
                            catch (System.Exception error)
                            {
                                hasError = true;
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + error.Message, GetOtherEnd().Specification.FullName + "." + Name));
                            }
                        }
                    }
                    //Association.Identity.ToString()=="{82EF20D6-8AF9-494E-B661-0384D66A7F27}"
                    if (Multiplicity.IsMany && CollectionClassifier != null &&
                        CollectionClassifier.GetOperation("GetEnumerator", new string[0], true) != null &&
                        CollectionClassifier.GetOperation("GetEnumerator", new string[0], true).ReturnType != null)
                    {

                        MetaDataRepository.Classifier collectionObjectType = null;
                        MetaDataRepository.Classifier enumeratorType = CollectionClassifier.GetOperation("GetEnumerator", new string[0], true).ReturnType;
                        if (enumeratorType.TemplateBinding != null
                            && enumeratorType.TemplateBinding.Signature.OwnedParameters.Count == 1)
                        {

                            foreach (MetaDataRepository.TemplateParameter templateParameter in enumeratorType.TemplateBinding.Signature.OwnedParameters)
                            {
                                collectionObjectType = enumeratorType.TemplateBinding.GetActualParameterFor(templateParameter) as MetaDataRepository.Classifier;
                                break;
                            }
                            if (Association.LinkClass != null && !Association.LinkClass.IsA(collectionObjectType))
                            {
                                hasError = true;
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The Collection object type must be '" + Association.LinkClass.FullName + "' or super type of this.", GetOtherEnd().Specification.FullName + "." + Name));
                            }
                            if (Association.LinkClass == null && !Specification.IsA(collectionObjectType))
                            {
                                hasError = true;
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The Collection object type must be '" + Specification.FullName + "' or super type of this.", GetOtherEnd().Specification.FullName + "." + Name));

                            }

                        }

                    }

                    #endregion

                    #region Multiplicity check
                    if (Multiplicity.LowLimit > Multiplicity.HighLimit && !Multiplicity.NoHighLimit && !Multiplicity.Unspecified)
                    {
                        hasError = true;

                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: Wrong multiplicity at association end '" + GetOtherEnd().Specification.FullName + "." + Name +
                            "' the low limit is greater than high limit.", GetOtherEnd().Specification.FullName + "." + Name));
                    }

                    if (Persistent)
                    {

                        if (Multiplicity.IsMany && !memberType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                        {
                            hasError = true;
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The multiplicity of association end '" + GetOtherEnd().Specification.FullName + "." + Name +
                                    "' is many and the type isn't collection subclass of 'OOAdvantech.PersistenceLayer.ObjectContainer", GetOtherEnd().Specification.FullName + "." + Name));
                        }
                    }
                    else
                    {


                        if (Multiplicity.IsMany && Type.GetInterface(memberType, typeof(System.Collections.IEnumerable).FullName) == null
                            && !(memberType.GetMetaData().GetMethod("GetEnumerator", new System.Type[0]) != null && memberType.GetMetaData().GetMethod("GetEnumerator").DeclaringType != null &&
                            memberType.GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).DeclaringType.GetMetaData().IsGenericType && memberType.GetMetaData().GetMethod("GetEnumerator").DeclaringType.GetMetaData().GetGenericArguments().Length == 1)
                            && !memberType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                        {
                            // System.Windows.Forms.MessageBox.Show("Stop"); 
                            hasError = true;
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The multiplicity of association end '" + GetOtherEnd().Specification.FullName + "." + Name +
                                    "' is many and the type isn't collection subclass of 'System.Collections.IEnumerable", GetOtherEnd().Specification.FullName + "." + Name));
                        }


                    }

                    if (!Multiplicity.IsMany)
                    {
                        if (Association.LinkClass == null)
                        {

                            System.Type specificationType = Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                            if (memberType != specificationType)
                            {
                                hasError = true;
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: Association end '" + GetOtherEnd().Specification.FullName + "." + Name + "' type mismatch." +
                                    " Check type declaration at Association attribute." +
                                    "\nIn zero to one or exactly one relationships, the type of field or property must be the same as the type at Association attribute.", GetOtherEnd().Specification.FullName + "." + Name));
                            }
                        }
                        else
                        {
                            System.Type specificationType = Association.LinkClass.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                            if (memberType != specificationType)
                            {
                                hasError = true;
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: Association end '" + GetOtherEnd().Specification.FullName + "." + Name + "' type mismatch." +
                                    "\nIn zero to one or exactly one relationships with link type, the type of field or property must be the same as the type at AssociationClass attribute.", GetOtherEnd().Specification.FullName + "." + Name));
                            }

                        }
                    }
                    #endregion

                    #region Link class check
                    if (WrMemberInfo != null && Association.LinkClass != null && Association.LinkClass.LinkAssociation != Association)
                    {
                        customAttributes = WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass), false);
                        if (customAttributes.Length > 0)
                        {
                            System.Type declaredAssType = (customAttributes[0] as MetaDataRepository.AssociationClass).AssocciationClass;
                            if (declaredAssType != Association.LinkClass.GetExtensionMetaObject(typeof(System.Type)))
                            {

                                hasError = true;
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: Declaration error on Association end '" + GetOtherEnd().Specification.FullName + "." + Name + "' the type '" +
                                    Association.LinkClass.FullName + "' hasn't been declared as association type of '" + Association.Name + "' association.", GetOtherEnd().Specification.FullName + "." + Name));
                            }
                        }
                    }
                    #endregion


                }





                return hasError;
            }
            finally
            {
                InErrorCheck = false;
            }
        }

        /// <MetaDataID>{2749A6C9-1FAD-4EEB-B534-6B75596EB99D}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Operation Setter
        {
            get
            {
                if (PropertyMember != null && _Setter == null)
                {
                    System.Reflection.MethodInfo setMethod = PropertyMember.GetSetMethod(true);
                    if (setMethod != null)
                    {
                        _Setter = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(setMethod) as Operation;
                        if (_Setter == null)
                            _Setter = new Operation(setMethod, Namespace as OOAdvantech.MetaDataRepository.Classifier);
                    }
                }
                return _Setter;

            }
        }
        /// <MetaDataID>{BA0323C2-8D37-4ED5-B523-03ED2C0E716E}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Operation Getter
        {
            get
            {

                //Να ελεγχθεί τι κάνει το σύστημα με τις setter και getter operation στα associationrealization.
                if (PropertyMember != null && _Getter == null)
                {
                    System.Reflection.MethodInfo getMethod = PropertyMember.GetGetMethod(true);
                    if (getMethod != null)
                    {
                        _Getter = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(getMethod) as Operation;
                        if (_Getter == null)
                            _Getter = new Operation(getMethod, Namespace as OOAdvantech.MetaDataRepository.Classifier);
                    }

                }
                return _Getter;
            }
        }


        //		/// <MetaDataID>{5F36B3E8-86E6-47AD-AA3A-85D857AFBB17}</MetaDataID>
        //		private System.Type _SpecificationType;
        //		/// <MetaDataID>{34B62BBD-8538-489B-B95E-F3765430CEEC}</MetaDataID>
        //		public System.Type SpecificationType
        //		{
        //			get
        //			{
        //				if(_SpecificationType!=null)
        //					return _SpecificationType;
        //				else
        //				{
        //					_SpecificationType=Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
        //					return _SpecificationType;
        //				}
        //			}
        //		}



        /// <MetaDataID>{1F86A1CF-D20A-4C56-B101-FAF6093D8F8F}</MetaDataID>
        public void AddExtensionMetaObject(object Value)
        {
            lock (ExtensionMetaObjects)
            {
                GetExtensionMetaObjects();
                ExtensionMetaObjects.Add(Value);
            }


        }

        //		/// <MetaDataID>{A1BB6A82-C056-43BF-B039-159E7675981B}</MetaDataID>
        //		public override MetaDataRepository.MultiplicityRange Multiplicity
        //		{
        //			}
        //		}
        /// <MetaDataID>{B33E85FF-A6ED-49CB-BEC2-4A10FC0E4339}</MetaDataID>
        public override string Name
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (base.Name != null)
                        return base.Name;
                    if (IsRoleA)
                        return Association.Name + "RoleAName";
                    else
                        return Association.Name + "RoleBName";
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
                    base.Name = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);

                }
            }
        }


        /// <MetaDataID>{2D783A88-48AA-4697-83B7-7228E1D742FF}</MetaDataID>
        private void InitBehavioralProperties()
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (WrMemberInfo == null)
                    return;
                object[] CustomAttributes = WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationEndBehavior), false);
                if (CustomAttributes.Length == 0)
                    _HasBehavioralSettings = false;
                foreach (MetaDataRepository.AssociationEndBehavior associationEndBehavior in CustomAttributes)
                {
                    _HasBehavioralSettings = true;

                    if ((uint)(associationEndBehavior.PersistencyFlag & MetaDataRepository.PersistencyFlag.ReferentialIntegrity) != 0)
                        _ReferentialIntegrity = true;
                    else
                        _ReferentialIntegrity = false;

                    if ((uint)(associationEndBehavior.PersistencyFlag & MetaDataRepository.PersistencyFlag.CascadeDelete) != 0)
                        _CascadeDelete = true;
                    else
                        _CascadeDelete = false;


                    if ((uint)(associationEndBehavior.PersistencyFlag & MetaDataRepository.PersistencyFlag.AllowTransient) != 0)
                        _AllowTransient = true;
                    else
                        _AllowTransient = false;


                    uint temp = (uint)associationEndBehavior.PersistencyFlag;
                    if ((uint)(associationEndBehavior.PersistencyFlag & MetaDataRepository.PersistencyFlag.OnConstruction) != 0)
                        _LazyFetching = false;
                    else
                        _LazyFetching = true;


                    if ((uint)(associationEndBehavior.PersistencyFlag & MetaDataRepository.PersistencyFlag.OnConstruction) != 0 &&
                        (uint)(associationEndBehavior.PersistencyFlag & MetaDataRepository.PersistencyFlag.LazyFetching) != 0)
                    {
                        _TryOnObjectActivationFetching = true;
                    }

                    break;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <exclude>Excluded</exclude>
        AccessorBuilder.FieldPropertyAccessor _FastFieldAccessor;
        /// <MetaDataID>{e307445e-4d60-40d5-9e93-b810ffb2bd2f}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor FastFieldAccessor
        {
            get
            {
                if (Navigable)
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
                else
                    return null;
            }
        }
        /// <exclude>Excluded</exclude>
        AccessorBuilder.FieldPropertyAccessor _FastPropertyAccessor;
        /// <MetaDataID>{0cb6257c-3ad4-4fe9-8651-aebe74647fbc}</MetaDataID>
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



        /// <MetaDataID>{4274543A-127E-407D-B2F0-44CE7901FEBD}</MetaDataID>
        public System.Reflection.PropertyInfo PropertyMember
        {
            get
            {
                if (WrMemberInfo == null)
                    return null;
                if (WrMemberInfo is System.Reflection.PropertyInfo)
                    return WrMemberInfo as System.Reflection.PropertyInfo;
                else
                    return null;
            }
        }
        /// <MetaDataID>{f248c52d-8466-41d7-a793-069c6153ac64}</MetaDataID>
        public override object GetExtensionMetaObject(System.Type MetaObjectType)
        {

            if (MetaObjectType == typeof(System.Reflection.FieldInfo) || MetaObjectType.GetMetaData().IsSubclassOf(typeof(System.Reflection.FieldInfo)))
                return FieldMember;

            if (MetaObjectType == typeof(System.Reflection.PropertyInfo) || MetaObjectType.GetMetaData().IsSubclassOf(typeof(System.Reflection.PropertyInfo)))
                return PropertyMember;

            return base.GetExtensionMetaObject(MetaObjectType);
        }

        public static void RemoveObjectsLink(MetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        {
            RemoveValueTypeObjectsLink(associationEnd, ownerObject, relatedObject, null);
        }
        /// <MetaDataID>{c25e0c20-638a-4c89-bf23-8a4161799183}</MetaDataID>
        public static object RemoveValueTypeObjectsLink(MetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject, ValueTypePath valueTypePath)
        {
            Classifier _classifier = MetaDataRepository.Classifier.GetClassifier(ownerObject.GetType());
            //TODO να γραφτεί test case και κωδικας στην περίπτωση που το ownerObject ειναι OutOfProcess
            if (associationEnd.Navigable && !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(ownerObject as MarshalByRefObject))
            {
                if (_classifier is Class && (_classifier as Class).ClassHierarchyLinkAssociation == associationEnd.Association)
                {
                    Class _class = _classifier as Class;
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        if (associationEnd.IsRoleA)
                        {

                            object roleB = Member<object>.GetValue(_class.LinkClassRoleBFastFieldAccessor.GetValue, ownerObject);
                            relatedObject = ownerObject;
                            ownerObject = roleB;
                            _classifier = Type.GetClassifierObject(ownerObject.GetType()) as Class;

                        }
                        else
                        {
                            object roleA = Member<object>.GetValue(_class.LinkClassRoleAFastFieldAccessor.GetValue, ownerObject);
                            relatedObject = ownerObject;
                            ownerObject = roleA;
                            _classifier = Type.GetClassifierObject(ownerObject.GetType()) as Class;

                        }
                        stateTransition.Consistent = true;
                    }
                }

                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = null;

                if (valueTypePath != null && valueTypePath.Count > 0)
                {
                    var tmpValueTypePath = new ValueTypePath(valueTypePath);
                    var attribute = _classifier.GetAttributes(true).Where(x => x.Identity == tmpValueTypePath.Peek()).FirstOrDefault();

                    if (_classifier is Class)
                    {
                        fastFieldAccessor = (_classifier as Class).GetFastFieldAccessor(attribute as DotNetMetaDataRepository.Attribute);
                        if (fastFieldAccessor == null || fastFieldAccessor.MemberInfo == null)
                            return ownerObject;
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            var valueTypeObject = fastFieldAccessor.GetValue(ownerObject);
                            tmpValueTypePath.Pop();
                            valueTypeObject = RemoveValueTypeObjectsLink(associationEnd, valueTypeObject, relatedObject, tmpValueTypePath);
                            fastFieldAccessor.SetValue(ownerObject, valueTypeObject);
                            stateTransition.Consistent = true;
                        }
                    }
                    if (_classifier is Structure)
                    {
                        fastFieldAccessor = (_classifier as Class).GetFastFieldAccessor(attribute as DotNetMetaDataRepository.Attribute);
                        if (fastFieldAccessor == null || fastFieldAccessor.MemberInfo == null)
                            return ownerObject;

                        var valueTypeObject = fastFieldAccessor.GetValue(ownerObject);
                        tmpValueTypePath.Pop();
                        valueTypeObject = RemoveValueTypeObjectsLink(associationEnd, valueTypeObject, relatedObject, tmpValueTypePath);
                        fastFieldAccessor.SetValue(ownerObject, valueTypeObject);

                    }
                    return ownerObject;
                }



                if (_classifier is Class)
                    fastFieldAccessor = (_classifier as Class).GetFastFieldAccessor(associationEnd as DotNetMetaDataRepository.AssociationEnd);

                if (_classifier is Structure)
                    fastFieldAccessor = (_classifier as Structure).GetFastFieldAccessor(associationEnd as DotNetMetaDataRepository.AssociationEnd);

                if (fastFieldAccessor == null)
                    return null;

                if (associationEnd.Multiplicity.IsMany)
                {

                    object value = fastFieldAccessor.GetValue(ownerObject);
                    if (value is ICollectionMember)
                    {

                        System.Reflection.MemberInfo transactionalMember = null;
                        if (_classifier is Class)
                            transactionalMember = (_classifier as Class).GetTransactionalMember(associationEnd as AssociationEnd);
                        if (transactionalMember != null)
                        {
                            if (_classifier is Class)
                            {
                                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, transactionalMember, OOAdvantech.Transactions.TransactionOption.Supported))
                                {
                                    (value as ICollectionMember).RemoveImplicitly(relatedObject);

                                    MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                    if (operation != null)
                                    {
                                        try
                                        {
                                            System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                            AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                        }
                                        catch (Exception error)
                                        {
                                        }
                                    }

                                    if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                        (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                    else if (ownerObject != null)
                                    {

                                        ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                        if (extensionProperties != null)
                                            (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                    }

                                    stateTransition.Consistent = true;
                                }
                            }
                            else
                            {
                                (value as ICollectionMember).RemoveImplicitly(relatedObject);


                                MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                if (operation != null)
                                {
                                    try
                                    {
                                        System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                        AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                    }
                                    catch (Exception error)
                                    {
                                    }
                                }

                                if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                    (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                else if (ownerObject != null)
                                {
                                    ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                    if (extensionProperties != null)
                                        (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                }
                            }
                        }
                        else
                        {
                            if (Transactions.ObjectStateTransition.IsTransactional(ownerObject.GetType()))
                            {
                                using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                                {
                                    (value as ICollectionMember).RemoveImplicitly(relatedObject);

                                    MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                    if (operation != null)
                                    {
                                        try
                                        {
                                            System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                            AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                        }
                                        catch (Exception error)
                                        {
                                        }
                                    }

                                    if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                        (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                    else if (ownerObject != null)
                                    {
                                        ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                        if (extensionProperties != null)
                                            (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                    }
                                    stateTransition.Consistent = true;
                                }
                            }
                            else
                            {
                                (value as ICollectionMember).RemoveImplicitly(relatedObject);


                                MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                if (operation != null)
                                {
                                    try
                                    {
                                        System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                        AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                    }
                                    catch (Exception error)
                                    {
                                    }
                                }


                                if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                    (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                else if (ownerObject != null)
                                {
                                    ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                    if (extensionProperties != null)
                                        (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                }
                            }
                        }
                    }
                    else
                        throw new System.Exception("System in inconsistent state");
                }
                else
                {

                    System.Reflection.MemberInfo transactionalMember = null;
                    if (_classifier is Class)
                        transactionalMember = (_classifier as Class).GetTransactionalMember(associationEnd as AssociationEnd);
                    if (transactionalMember != null)
                    {
                        if (_classifier is Class)
                        {

                            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, transactionalMember, OOAdvantech.Transactions.TransactionOption.Supported))
                            {
                                var exixtingRelatedObject = Member<object>.GetValue(fastFieldAccessor.GetValue, ownerObject);
                                if (relatedObject == exixtingRelatedObject)
                                {
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref ownerObject, null);

                                    MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                    if (operation != null)
                                    {
                                        try
                                        {
                                            System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                            AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                        }
                                        catch (Exception error)
                                        {
                                        }
                                    }

                                    if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                        (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                    else if (ownerObject != null)
                                    {
                                        ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                        if (extensionProperties != null)
                                            (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                    }
                                }
                                else
                                {

                                }
                                stateTransition.Consistent = true;
                            }
                        }
                        else
                        {
                            var exixtingRelatedObject = Member<object>.GetValue(fastFieldAccessor.GetValue, ownerObject);
                            if (relatedObject == exixtingRelatedObject)
                            {
                                Member<object>.SetValueImplicitly(fastFieldAccessor, ref ownerObject, null);

                                MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                if (operation != null)
                                {
                                    try
                                    {
                                        System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                        AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                    }
                                    catch (Exception error)
                                    {
                                    }
                                }

                                if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                    (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                else if (ownerObject != null)
                                {
                                    ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                    if (extensionProperties != null)
                                        (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Transactions.ObjectStateTransition.IsTransactional(ownerObject.GetType()))
                        {
                            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                            {
                                var exixtingRelatedObject = Member<object>.GetValue(fastFieldAccessor.GetValue, ownerObject);
                                if (relatedObject == exixtingRelatedObject)
                                {
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref ownerObject, null);

                                    MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                    if (operation != null)
                                    {
                                        try
                                        {
                                            System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                            AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                        }
                                        catch (Exception error)
                                        {
                                        }
                                    }

                                    if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                        (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                    else if (ownerObject != null)
                                    {
                                        ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                        if (extensionProperties != null)
                                            (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                    }
                                }
                                else
                                {
                                }
                                stateTransition.Consistent = true;
                            }
                        }
                        else
                        {
                            var exixtingRelatedObject = Member<object>.GetValue(fastFieldAccessor.GetValue, ownerObject);
                            if (relatedObject == exixtingRelatedObject)
                            {
                                Member<object>.SetValueImplicitly(fastFieldAccessor, ref ownerObject, null);

                                MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                if (operation != null)
                                {
                                    try
                                    {
                                        System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                        AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                    }
                                    catch (Exception error)
                                    {
                                    }
                                }

                                if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                    (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                else if (ownerObject != null)
                                {
                                    ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                    if (extensionProperties != null)
                                        (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectRemoved(relatedObject, associationEnd);
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }

            }
            if (associationEnd.Association.General != null)
            {
                if (associationEnd.IsRoleA)
                {
                    if (associationEnd.Association.General.RoleA.Navigable)
                        RemoveValueTypeObjectsLink(associationEnd.Association.General.RoleA, ownerObject, relatedObject, valueTypePath);
                    if (associationEnd.Association.General.RoleB.Navigable)
                        RemoveValueTypeObjectsLink(associationEnd.Association.General.RoleB, relatedObject, ownerObject, valueTypePath);
                }
                else
                {
                    if (associationEnd.Association.General.RoleB.Navigable)
                        RemoveValueTypeObjectsLink(associationEnd.Association.General.RoleB, ownerObject, relatedObject, valueTypePath);
                    if (associationEnd.Association.General.RoleA.Navigable)
                        RemoveValueTypeObjectsLink(associationEnd.Association.General.RoleA, relatedObject, ownerObject, valueTypePath);
                }
            }

            return ownerObject;

            //TODO δεν δουλεύει καλά σε remoting mode

            //TODO δεν δουλεύει καλά σε remoting mode

        }
        /// <MetaDataID>{e966794f-849a-4ed5-bb40-8741ddca0d71}</MetaDataID>
        bool InitHasAssociationEndRealizations = true;
        /// <MetaDataID>{8882af5c-136c-4a8e-8844-6b11100d51c6}</MetaDataID>
        bool _HasAssociationEndRealizations;
        /// <MetaDataID>{05fbdb09-5d3f-4bb4-aa6f-ec4d6c31704e}</MetaDataID>
        public override bool HasAssociationEndRealizations
        {
            get
            {
                if (InitHasAssociationEndRealizations)
                {
                    _HasAssociationEndRealizations = base.HasAssociationEndRealizations;
                    InitHasAssociationEndRealizations = false;
                }
                return _HasAssociationEndRealizations;

            }
        }

        public static void AddObjectsLink(MetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        {
            AddValueTypeObjectsLink(associationEnd, ownerObject, relatedObject, null);
        }
        /// <MetaDataID>{7684ed84-f702-42ec-a1dd-a6544271463a}</MetaDataID>
        public static object AddValueTypeObjectsLink(MetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject, ValueTypePath valueTypePath)
        {
            //TODO να γραφτεί test case και κωδικας στην περίπτωση που το ownerObject ειναι OutOfProcess
            if (associationEnd.Navigable && !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(ownerObject as MarshalByRefObject))
            {
                var _classifier = Type.GetClassifierObject(ownerObject.GetType());
                Class _class = Type.GetClassifierObject(ownerObject.GetType()) as Class;
                if (_class.ClassHierarchyLinkAssociation == associationEnd.Association)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        if (associationEnd.IsRoleA)
                        {
                            Member<object>.SetValueImplicitly(_class.LinkClassRoleAFastFieldAccessor, ref ownerObject, relatedObject);

                            object roleB = Member<object>.GetValue(_class.LinkClassRoleBFastFieldAccessor.GetValue, ownerObject);
                            relatedObject = ownerObject;
                            ownerObject = roleB;
                            _class = Type.GetClassifierObject(ownerObject.GetType()) as Class;

                        }
                        else
                        {
                            Member<object>.SetValueImplicitly(_class.LinkClassRoleBFastFieldAccessor, ref ownerObject, relatedObject);
                            object roleA = Member<object>.GetValue(_class.LinkClassRoleAFastFieldAccessor.GetValue, ownerObject);
                            relatedObject = ownerObject;
                            ownerObject = roleA;
                            _class = Type.GetClassifierObject(ownerObject.GetType()) as Class;

                        }
                        stateTransition.Consistent = true;
                    }
                }


                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = null;


                if (valueTypePath != null && valueTypePath.Count > 0)
                {
                    var tmpValueTypePath = new ValueTypePath(valueTypePath);
                    var attribute = _classifier.GetAttributes(true).Where(x => x.Identity == tmpValueTypePath.Peek()).FirstOrDefault();

                    if (_classifier is Class)
                    {
                        fastFieldAccessor = (_classifier as Class).GetFastFieldAccessor(attribute as DotNetMetaDataRepository.Attribute);
                        if (fastFieldAccessor == null || fastFieldAccessor.MemberInfo == null)
                            return ownerObject;

                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            var valueTypeObject = fastFieldAccessor.GetValue(ownerObject);
                            tmpValueTypePath.Pop();
                            valueTypeObject = AddValueTypeObjectsLink(associationEnd, valueTypeObject, relatedObject, tmpValueTypePath);
                            fastFieldAccessor.SetValue(ownerObject, valueTypeObject);
                            stateTransition.Consistent = true;
                        }
                    }
                    if (_classifier is Structure)
                    {
                        fastFieldAccessor = (_classifier as Class).GetFastFieldAccessor(attribute as DotNetMetaDataRepository.Attribute);
                        if (fastFieldAccessor == null || fastFieldAccessor.MemberInfo == null)
                            return ownerObject;
                        var valueTypeObject = fastFieldAccessor.GetValue(ownerObject);
                        tmpValueTypePath.Pop();
                        valueTypeObject = AddValueTypeObjectsLink(associationEnd, valueTypeObject, relatedObject, tmpValueTypePath);
                        fastFieldAccessor.SetValue(ownerObject, valueTypeObject);

                    }
                    return ownerObject;
                }

                if (_classifier is Class)
                    fastFieldAccessor = (_classifier as Class).GetFastFieldAccessor(associationEnd as DotNetMetaDataRepository.AssociationEnd);

                if (_classifier is Structure)
                    fastFieldAccessor = (_classifier as Structure).GetFastFieldAccessor(associationEnd as DotNetMetaDataRepository.AssociationEnd);
                if (fastFieldAccessor == null)
                    return null;
                if (associationEnd.Multiplicity.IsMany)
                {
                    object value = fastFieldAccessor.GetValue(ownerObject);
                    if (value != null)
                    {
                        if (value is ICollectionMember)
                        {

                            System.Reflection.MemberInfo transactionalMember = null;
                            if (_classifier is Class)
                                transactionalMember = (_classifier as Class).GetTransactionalMember(associationEnd as AssociationEnd);

                            if (transactionalMember != null)
                            {
                                if (_classifier is Class)
                                {
                                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, transactionalMember, OOAdvantech.Transactions.TransactionOption.Supported))
                                    {
                                        (value as ICollectionMember).AddImplicitly(relatedObject);

                                        MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                        if (operation != null)
                                        {
                                            try
                                            {
                                                System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                                AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, false });
                                            }
                                            catch (Exception error)
                                            {
                                            }
                                        }

                                        if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                            (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                        else if (ownerObject != null)
                                        {
                                            ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                            if (extensionProperties != null)
                                                (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                        }


                                        stateTransition.Consistent = true;
                                    }
                                }
                                else
                                {
                                    (value as ICollectionMember).AddImplicitly(relatedObject);

                                    MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                    if (operation != null)
                                    {
                                        try
                                        {
                                            System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                            AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, true });
                                        }
                                        catch (Exception error)
                                        {
                                        }
                                    }

                                    if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                        (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                    else if (ownerObject != null)
                                    {
                                        ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                        if (extensionProperties != null)
                                            (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                    }

                                }
                            }
                            else
                            {
                                if (Transactions.ObjectStateTransition.IsTransactional(ownerObject.GetType()))
                                {
                                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                                    {
                                        (value as ICollectionMember).AddImplicitly(relatedObject);

                                        MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                        if (operation != null)
                                        {
                                            try
                                            {
                                                System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                                AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, true });
                                            }
                                            catch (Exception error)
                                            {
                                            }
                                        }

                                        if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                            (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                        else
                                        {
                                            ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                            if (extensionProperties != null)
                                                (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                        }
                                        stateTransition.Consistent = true;
                                    }
                                }
                                else
                                {
                                    (value as ICollectionMember).AddImplicitly(relatedObject);
                                    MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                    if (operation != null)
                                    {
                                        try
                                        {
                                            System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                            AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, true });
                                        }
                                        catch (Exception error)
                                        {
                                        }
                                    }
                                    if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                        (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                    else if (ownerObject != null)
                                    {
                                        ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                        if (extensionProperties != null)
                                            (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                    }
                                }
                            }
                        }
                        else
                            throw new System.Exception("System in inconsistent state");
                    }
                }
                else
                {

                    System.Reflection.MemberInfo transactionalMember = null;
                    if (_classifier is Class)
                        transactionalMember = (_classifier as Class).GetTransactionalMember(associationEnd as AssociationEnd);

                    if (transactionalMember != null)
                    {
                        if (Transactions.ObjectStateTransition.IsTransactional(ownerObject.GetType()))
                        {
                            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, transactionalMember, OOAdvantech.Transactions.TransactionOption.Supported))
                            {

                                object oldRelatedObject = Member<object>.GetValue(fastFieldAccessor.GetValue, ownerObject);
                                if (relatedObject != null && oldRelatedObject != null && oldRelatedObject != relatedObject)
                                    throw new System.Exception(string.Format("AssociationEnd '{0}' multiplicity mismatch", associationEnd.FullName));
                                Member<object>.SetValueImplicitly(fastFieldAccessor, ref ownerObject, relatedObject);
                                stateTransition.Consistent = true;
                            }
                        }
                        else
                        {
                            object oldRelatedObject = Member<object>.GetValue(fastFieldAccessor.GetValue, ownerObject);
                            if (relatedObject != null && oldRelatedObject != null && oldRelatedObject != relatedObject)
                                throw new System.Exception(string.Format("AssociationEnd '{0}' multiplicity mismatch", associationEnd.FullName));
                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref ownerObject, relatedObject);
                        }
                    }
                    else
                    {
                        if (Transactions.ObjectStateTransition.IsTransactional(ownerObject.GetType()))
                        {
                            using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                            {

                                object oldRelatedObject = Member<object>.GetValue(fastFieldAccessor.GetValue, ownerObject);
                                if (relatedObject != null && oldRelatedObject != null && oldRelatedObject != relatedObject)
                                    throw new System.Exception(string.Format("AssociationEnd '{0}' multiplicity mismatch", associationEnd.FullName));

                                Member<object>.SetValueImplicitly(fastFieldAccessor, ref ownerObject, relatedObject);

                                MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                                if (operation != null)
                                {
                                    try
                                    {
                                        System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                        AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, true });
                                    }
                                    catch (Exception error)
                                    {
                                    }
                                }

                                if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                    (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                else if (ownerObject != null)
                                {
                                    ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                    if (extensionProperties != null)
                                        (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                                }

                                stateTransition.Consistent = true;
                            }
                        }
                        else
                        {
                            object oldRelatedObject = Member<object>.GetValue(fastFieldAccessor.GetValue, ownerObject);
                            if (relatedObject != null && oldRelatedObject != null && oldRelatedObject != relatedObject)
                                throw new System.Exception(string.Format("AssociationEnd '{0}' multiplicity mismatch", associationEnd.FullName));

                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref ownerObject, relatedObject);

                            MetaDataRepository.Operation operation = (Classifier.GetClassifier(ownerObject.GetType()) as Class)?.ObjectsLink;
                            if (operation != null)
                            {
                                try
                                {
                                    System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                                    AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(ownerObject, new object[3] { relatedObject, associationEnd, true });
                                }
                                catch (Exception error)
                                {
                                }
                            }

                            if (ownerObject is PersistenceLayer.IObjectStateEventsConsumer)
                                (ownerObject as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                            else if (ownerObject != null)
                            {

                                ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(ownerObject);
                                if (extensionProperties != null)
                                    (extensionProperties as PersistenceLayer.IObjectStateEventsConsumer).LinkedObjectAdded(relatedObject, associationEnd);
                            }

                        }
                    }
                }

                if (associationEnd.Association.General != null)
                {
                    if (associationEnd.IsRoleA)
                    {
                        if (associationEnd.Association.General.RoleA.Navigable)
                            AddValueTypeObjectsLink(associationEnd.Association.General.RoleA, ownerObject, relatedObject, valueTypePath);
                        if (associationEnd.Association.General.RoleB.Navigable)
                            AddValueTypeObjectsLink(associationEnd.Association.General.RoleB, relatedObject, ownerObject, valueTypePath);
                    }
                    else
                    {
                        if (associationEnd.Association.General.RoleB.Navigable)
                            AddValueTypeObjectsLink(associationEnd.Association.General.RoleB, ownerObject, relatedObject, valueTypePath);
                        if (associationEnd.Association.General.RoleA.Navigable)
                            AddValueTypeObjectsLink(associationEnd.Association.General.RoleA, relatedObject, ownerObject, valueTypePath);
                    }
                }
            }

            return ownerObject;
            //TODO δεν δουλεύει καλά σε remoting mode

        }




        /// <MetaDataID>{BDACB24D-107F-4E04-904D-CA580E93C42D}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            lock (ExtensionMetaObjectsLock)
            {
                if (ExtensionMetaObjects == null)
                {
                    ReaderWriterLock.AcquireWriterLock(10000);

                    ExtensionMetaObjects = new System.Collections.Generic.List<object>();

                    if (WrMemberInfo != null)
                        ExtensionMetaObjects.Add(WrMemberInfo);
                }
                return ExtensionMetaObjects.ToList();
            }

        }

        /// <MetaDataID>{247E395A-2E2B-4345-ACF6-FEA40B16B964}</MetaDataID>
        public AssociationEnd(MetaDataRepository.Namespace _namespace, MetaDataRepository.Roles role, MetaDataRepository.Classifier specificationClasifier, System.Reflection.Assembly assembly, MetaDataRepository.AssociationAttribute associationAttribute, MetaDataRepository.MultiplicityRange multiplicity)
        {
            _Multiplicity = multiplicity;
            _Namespace.Value = _namespace;
            Accessors = new System.Reflection.MethodInfo[0];

            if (role == MetaDataRepository.Roles.RoleA)
                IsRoleA = true;
            else
                IsRoleA = false;
            _Specification = specificationClasifier;
            //if (specificationClasifier is Interface)
            //    (specificationClasifier as Interface).AddAssociationEnd(this);
            //if (specificationClasifier is Class)
            //    (specificationClasifier as Class).AddAssociationEnd(this);

            //if (specificationClasifier is Structure)
            //    (specificationClasifier as Structure).AddAssociationEnd(this);


            if (!string.IsNullOrEmpty(associationAttribute.Identity))
            {
                OOAdvantech.MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = Assembly.GetBackwardCompatibilityID(assembly);
                if (backwardCompatibilityID != null)
                    _Identity = new MetaDataRepository.MetaObjectID(backwardCompatibilityID + "." + associationAttribute.Identity.ToString() + Role.ToString());//Error prone 
                else
                    _Identity = new MetaDataRepository.MetaObjectID(associationAttribute.Identity.ToString() + Role.ToString());//Error prone 

            }
            else
            {
                if (IsRoleA)
                    _Identity = new MetaDataRepository.MetaObjectID("A:" + Name + "_" + Specification.Identity.ToString());
                else
                    _Identity = new MetaDataRepository.MetaObjectID("B:" + Name + "_" + Specification.Identity.ToString());
                AutoGeneratedIdentity = true;

            }
            MetaObjectMapper.AddMetaObject(this, FullName);




        }
        /// <MetaDataID>{c8756b06-ce72-427e-ad93-37df526de7b6}</MetaDataID>
        bool IgnoreErrorCheck = false;

        /// <MetaDataID>{83A3774B-CBAB-4148-9B56-6584A13D41A3}</MetaDataID>
        public AssociationEnd(System.Reflection.MemberInfo memberInfo, MetaDataRepository.AssociationAttribute associationAttribute)
        {

            if (memberInfo != null)
            {
                if (memberInfo.GetCustomAttributes(typeof(MetaDataRepository.IgnoreErrorCheckAttribute), true).Length > 0)
                    IgnoreErrorCheck = true;
            }
            _Indexer = associationAttribute.Indexer;
            if ((!(memberInfo is System.Reflection.FieldInfo)) && (!(memberInfo is System.Reflection.PropertyInfo)))
                throw new System.Exception("Invalid memberInfo. The memberInfo must be PropertyInfo or FieldInfo");

            TransactionalMember = memberInfo.GetCustomAttributes(typeof(OOAdvantech.Transactions.TransactionalMemberAttribute), true).Length > 0;
            if (memberInfo is System.Reflection.PropertyInfo)
            {
                System.Reflection.PropertyInfo mPropertyInfo = memberInfo as System.Reflection.PropertyInfo;

                if (mPropertyInfo.GetAccessors().Length > 0)
                    Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
                else
                    Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
            }
            else
            {
                System.Reflection.FieldInfo mFieldInfo = memberInfo as System.Reflection.FieldInfo;
                if (mFieldInfo.IsPublic)
                    Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
                if (mFieldInfo.IsFamily)
                    Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
                if (mFieldInfo.IsPrivate)
                    Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
            }

            if (memberInfo is System.Reflection.FieldInfo)
                Accessors = new System.Reflection.MethodInfo[0];
            if (memberInfo is System.Reflection.PropertyInfo)
                Accessors = (memberInfo as System.Reflection.PropertyInfo).GetAccessors(true);

            WrMemberInfo = memberInfo;
            if (WrMemberInfo != null)
            {
                DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(WrMemberInfo, this);
                Navigable = true;
                //InitializeLifetimeService();
                _Name = WrMemberInfo.Name;
            }

            MetaDataRepository.Roles role = associationAttribute.Role;
            MetaDataRepository.Classifier specificationClasifier;

            specificationClasifier = Type.GetClassifierObject(associationAttribute.OtherEndType);

            if (role == MetaDataRepository.Roles.RoleA)
                IsRoleA = true;
            else
                IsRoleA = false;
            _Specification = specificationClasifier;

            //if (specificationClasifier is Interface)
            //    (specificationClasifier as Interface).AddAssociationEnd(this);
            //if (specificationClasifier is Class)
            //    (specificationClasifier as Class).AddAssociationEnd(this);
            //if (specificationClasifier is Structure)
            //    (specificationClasifier as Structure).AddAssociationEnd(this);



            if (WrMemberInfo is System.Reflection.PropertyInfo &&
                (WrMemberInfo as System.Reflection.PropertyInfo).PropertyType.FullName != specificationClasifier.FullName)
            {
                _CollectionClassifier = Type.GetClassifierObject((WrMemberInfo as System.Reflection.PropertyInfo).PropertyType);
            }
            if (WrMemberInfo is System.Reflection.FieldInfo &&
                (WrMemberInfo as System.Reflection.FieldInfo).FieldType.FullName != specificationClasifier.FullName)
            {
                _CollectionClassifier = Type.GetClassifierObject((WrMemberInfo as System.Reflection.FieldInfo).FieldType);
            }

            if (!string.IsNullOrEmpty(associationAttribute.Identity))
                _Identity = new MetaDataRepository.MetaObjectID(associationAttribute.Identity.ToString() + Role.ToString());//Error prone 
            else
            {
                if (IsRoleA)
                    _Identity = new MetaDataRepository.MetaObjectID("A:" + Name + "_" + Specification.Identity.ToString());
                else
                    _Identity = new MetaDataRepository.MetaObjectID("B:" + Name + "_" + Specification.Identity.ToString());
                AutoGeneratedIdentity = true;

            }

            AssociationEnd existingAssociationEnd = MetaObjectMapper.FindMetaObject(this.Identity, memberInfo.DeclaringType.GetMetaData().Assembly) as AssociationEnd;
            MetaObjectMapper.AddMetaObject(this, FullName);

            System.Reflection.MemberInfo memberWithAttribute = WrMemberInfo;

            if (memberWithAttribute == null)
                memberWithAttribute = (GetOtherEnd() as AssociationEnd).WrMemberInfo;

            MetaDataRepository.RoleAMultiplicityRangeAttribute RoleAMultiplicity = null;
            MetaDataRepository.RoleBMultiplicityRangeAttribute RoleBMultiplicity = null;

            object[] CustomAttributes = memberWithAttribute.GetCustomAttributes(typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute), false);
            if (CustomAttributes.Length != 0)
                RoleAMultiplicity = CustomAttributes[0] as MetaDataRepository.RoleAMultiplicityRangeAttribute;




            CustomAttributes = memberWithAttribute.GetCustomAttributes(typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute), false);
            if (CustomAttributes.Length != 0)
                RoleBMultiplicity = CustomAttributes[0] as MetaDataRepository.RoleBMultiplicityRangeAttribute;
            if (IsRoleA)
            {
                if (RoleAMultiplicity != null)
                    _Multiplicity = RoleAMultiplicity.Multiplicity;
                else
                {
                    System.Type memberType = null;
                    if (WrMemberInfo is System.Reflection.PropertyInfo)
                        memberType = (WrMemberInfo as System.Reflection.PropertyInfo).PropertyType;
                    if (WrMemberInfo is System.Reflection.FieldInfo)
                        memberType = (WrMemberInfo as System.Reflection.FieldInfo).FieldType;

                    if (FindIEnumerable(memberType) == null)
                        _Multiplicity = new MetaDataRepository.MultiplicityRange(0, 1);
                    else
                        _Multiplicity = new MetaDataRepository.MultiplicityRange();

                }
            }
            else
            {
                if (RoleBMultiplicity != null)
                    _Multiplicity = RoleBMultiplicity.Multiplicity;
                else
                {
                    System.Type memberType = null;
                    if (WrMemberInfo is System.Reflection.PropertyInfo)
                        memberType = (WrMemberInfo as System.Reflection.PropertyInfo).PropertyType;
                    if (WrMemberInfo is System.Reflection.FieldInfo)
                        memberType = (WrMemberInfo as System.Reflection.FieldInfo).FieldType;

                    if (FindIEnumerable(memberType) == null)
                        _Multiplicity = new MetaDataRepository.MultiplicityRange(0, 1);
                    else
                        _Multiplicity = new MetaDataRepository.MultiplicityRange();
                }
            }


            InitBehavioralProperties();


        }

        /// <MetaDataID>{714d9248-4b37-47d2-bd30-37834a33eed0}</MetaDataID>
        internal static System.Type FindIEnumerable(System.Type seqType)
        {
            if ((seqType != null) && (seqType != typeof(string)))
            {
                if (seqType.IsArray)
                {
                    return typeof(System.Collections.Generic.IEnumerable<>).MakeGenericType(new[] { seqType.GetElementType() });
                }
                if (seqType.GetMetaData().IsGenericType)
                {
                    foreach (System.Type type in seqType.GetMetaData().GetGenericArguments())
                    {
                        System.Type type2 = typeof(System.Collections.Generic.IEnumerable<>).MakeGenericType(new[] { type });
                        if (type2.GetMetaData().IsAssignableFrom(seqType))
                        {
                            return type2;
                        }
                    }
                }
                System.Type[] interfaces = seqType.GetMetaData().GetInterfaces();
                if ((interfaces != null) && (interfaces.Length > 0))
                {
                    foreach (System.Type type3 in interfaces)
                    {
                        System.Type type4 = FindIEnumerable(type3);
                        if (type4 != null)
                        {
                            return type4;
                        }
                    }
                }
                if ((seqType.GetMetaData().BaseType != null) && (seqType.GetMetaData().BaseType != typeof(object)))
                {
                    return FindIEnumerable(seqType.GetMetaData().BaseType);
                }
            }
            return null;
        }
        /// <MetaDataID>{fc8d6a42-92b5-4c14-a767-81caf1ef76d5}</MetaDataID>
        bool AutoGeneratedIdentity;
        /// <MetaDataID>{AD7FF79A-B239-4C53-B765-58073ED69068}</MetaDataID>
        public void SetAssociation(MetaDataRepository.Association theAssociation)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                try
                {
                    //TODO να γραφτεί test case;
                    if (WrMemberInfo != null) // Error prone σε περίπτωση που σε μια association με navigate και από
                                              // τις δύο πλευρέw όταν από την μια δεν δηλωθεί custom attribute presistent
                    {
                        _Persistent = false;
                        object[] Attributes = WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), false);
                        if (Attributes.Length > 0)
                            _Persistent = true;

                        Attributes = WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.Persistent), false);
                        if (Attributes.Length > 0)
                            _Persistent = true;
                    }
                }
                catch (System.Exception Error)
                {
                    int lo = 0;
                }
                _Association = theAssociation;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }


    }
}
