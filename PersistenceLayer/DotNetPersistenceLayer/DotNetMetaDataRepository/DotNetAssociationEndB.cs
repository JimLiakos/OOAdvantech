namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{C9BE7F05-C134-4AFE-BDB5-4334236B2D68}</MetaDataID>
    public class AssociationEnd : MetaDataRepository.AssociationEnd
    {
        /// <MetaDataID>{2490c096-27b4-4e37-a1a4-760b46fd52ad}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        /// <MetaDataID>{677dcefe-06ae-44b8-b08a-ecd5c1856fb3}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier CollectionClassifier
        {
            get
            {

                return base.CollectionClassifier;
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
                if (_Namespace != null)
                    return _Namespace;
                else
                {
                    _Namespace = DotNetMetaDataRepository.Type.GetClassifierObject(WrMemberInfo.DeclaringType);
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
        private System.Collections.ArrayList ExtensionMetaObjects;
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
        public override bool ErrorCheck(ref System.Collections.ArrayList errors)
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
                            if (specificationMethod.DeclaringType.IsClass)
                            {
                                hasError = true;
                                if (this.PropertyMember != null)
                                    errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: Declaration error on Association end '" + GetOtherEnd().Specification.FullName + "." + Name + "'. We can't declare an override property as association end.", GetOtherEnd().Specification.FullName + "." + Name));

                            }
                            else if (specificationMethod.DeclaringType.IsInterface)
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
                                collectionObjectType = enumeratorType.TemplateBinding.GetActualParameterFor(templateParameter);
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
                        if (Multiplicity.IsMany && !memberType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                        {
                            hasError = true;
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: The multiplicity of association end '" + GetOtherEnd().Specification.FullName + "." + Name +
                                    "' is many and the type isn't collection subclass of 'OOAdvantech.PersistenceLayer.ObjectContainer", GetOtherEnd().Specification.FullName + "." + Name));
                        }
                    }
                    else
                    {


                        if (Multiplicity.IsMany && Type.GetInterface(memberType, typeof(System.Collections.IEnumerable).FullName) == null
                            && !(memberType.GetMethod("GetEnumerator", new System.Type[0]) != null && memberType.GetMethod("GetEnumerator").ReflectedType != null &&
                            memberType.GetMethod("GetEnumerator", new System.Type[0]).ReflectedType.IsGenericType && memberType.GetMethod("GetEnumerator").ReflectedType.GetGenericArguments().Length == 1)
                            && !memberType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
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
                            _Setter = new Operation(setMethod);
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
                            _Getter = new Operation(getMethod);
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
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                GetExtensionMetaObjects();
                ExtensionMetaObjects.Add(Value);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
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

                    uint temp = (uint)associationEndBehavior.PersistencyFlag;
                    if ((uint)(associationEndBehavior.PersistencyFlag & MetaDataRepository.PersistencyFlag.OnConstruction) != 0)
                        _LazyFetching = false;
                    else
                        _LazyFetching = true;
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
        public AccessorBuilder.FieldPropertyAccessor FastFieldAccessor
        {
            get
            {
                if (_FastFieldAccessor.GetValue != null)
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
        public AccessorBuilder.FieldPropertyAccessor FastPropertyAccessor
        {
            get
            {
                if (_FastPropertyAccessor.GetValue != null)
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
        public override object GetExtensionMetaObject(System.Type MetaObjectType)
        {


            if (MetaObjectType == typeof(System.Reflection.FieldInfo))
                return FieldMember;

            if (MetaObjectType == typeof(System.Reflection.PropertyInfo))
                return PropertyMember;

            return base.GetExtensionMetaObject(MetaObjectType);
        }


        static void RemoveObjectsLink(MetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        {
            Class _class = (DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ownerObject.GetType()) as Class);
            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = _class.GetFastFieldAccessor(associationEnd as DotNetMetaDataRepository.AssociationEnd);
            if (fastFieldAccessor.InitializationRequired)
            {
                object value = fastFieldAccessor.GetValue(ownerObject);
                if (value is ICollectionMember)
                {

                    System.Reflection.MemberInfo transactionalMember = _class.GetTransactionalMember(associationEnd as AssociationEnd);
                    if (transactionalMember != null)
                    {

                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, transactionalMember, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            (value as ICollectionMember).RemoveImplicitly(relatedObject);
                            stateTransition.Consistent = true;
                        }
                    }
                    else
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            (value as ICollectionMember).RemoveImplicitly(relatedObject);
                            stateTransition.Consistent = true;
                        }
                    }



                }
            }



            //TODO δεν δουλεύει καλά σε remoting mode

            //TODO δεν δουλεύει καλά σε remoting mode

        }
        static void AddObjectsLink(MetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        {

            Class _class = Type.GetClassifierObject(ownerObject.GetType()) as Class;
            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = _class.GetFastFieldAccessor(associationEnd as DotNetMetaDataRepository.AssociationEnd);

            if (fastFieldAccessor.MemberInfo == null)
                return;
            if (associationEnd.Multiplicity.IsMany)
            {
                object value = fastFieldAccessor.GetValue(ownerObject);
                if (value is ICollectionMember)
                {

                    System.Reflection.MemberInfo transactionalMember = _class.GetTransactionalMember(associationEnd as AssociationEnd);
                    if (transactionalMember != null)
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, transactionalMember, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            (value as ICollectionMember).AddImplicitly(relatedObject);
                            stateTransition.Consistent = true;
                        }
                    }
                    else
                    {
                        using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            (value as ICollectionMember).AddImplicitly(relatedObject);
                            stateTransition.Consistent = true;
                        }
                    }
                }
                else
                    throw new System.Exception("System in inconsistent state");

            }
            else
            {

                System.Reflection.MemberInfo transactionalMember = _class.GetTransactionalMember(associationEnd as AssociationEnd);
                if (transactionalMember != null)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, transactionalMember, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        Member<object>.SetValue(fastFieldAccessor, ref ownerObject, relatedObject);
                        stateTransition.Consistent = true;
                    }
                }
                else
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(ownerObject, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        Member<object>.SetValue(fastFieldAccessor, ref ownerObject, relatedObject);
                        stateTransition.Consistent = true;
                    }
                }
            }

            //TODO δεν δουλεύει καλά σε remoting mode

        }




        /// <MetaDataID>{BDACB24D-107F-4E04-904D-CA580E93C42D}</MetaDataID>
        public override System.Collections.ArrayList GetExtensionMetaObjects()
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (ExtensionMetaObjects == null)
                {
                    ExtensionMetaObjects = new System.Collections.ArrayList();

                    if (WrMemberInfo != null)
                        ExtensionMetaObjects.Add(WrMemberInfo);
                }
                return ExtensionMetaObjects;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }

        /// <MetaDataID>{247E395A-2E2B-4345-ACF6-FEA40B16B964}</MetaDataID>
        public AssociationEnd(MetaDataRepository.Namespace _namespace, MetaDataRepository.Roles role, MetaDataRepository.Classifier specificationClasifier, MetaDataRepository.AssociationAttribute associationAttribute, MetaDataRepository.MultiplicityRange multiplicity)
        {
            _Multiplicity = multiplicity;
            _Namespace = _namespace;
            Accessors = new System.Reflection.MethodInfo[0];

            if (role == MetaDataRepository.Roles.RoleA)
                IsRoleA = true;
            else
                IsRoleA = false;
            _Specification = specificationClasifier;
            if (specificationClasifier is Interface)
                (specificationClasifier as Interface).AddAssociationEnd(this);
            if (specificationClasifier is Class)
                (specificationClasifier as Class).AddAssociationEnd(this);

            if (specificationClasifier is Structure)
                (specificationClasifier as Structure).AddAssociationEnd(this);


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
            MetaObjectMapper.AddMetaObject(this, FullName);




        }
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
            System.Type memberType = associationAttribute.OtherEndType;
            specificationClasifier = Type.GetClassifierObject(memberType);

            if (role == MetaDataRepository.Roles.RoleA)
                IsRoleA = true;
            else
                IsRoleA = false;
            _Specification = specificationClasifier;

            if (specificationClasifier is Interface)
                (specificationClasifier as Interface).AddAssociationEnd(this);
            if (specificationClasifier is Class)
                (specificationClasifier as Class).AddAssociationEnd(this);
            if (specificationClasifier is Structure)
                (specificationClasifier as Structure).AddAssociationEnd(this);



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
                    _Multiplicity = new MetaDataRepository.MultiplicityRange();
            }
            else
            {
                if (RoleBMultiplicity != null)
                    _Multiplicity = RoleBMultiplicity.Multiplicity;
                else
                    _Multiplicity = new MetaDataRepository.MultiplicityRange();
            }


            InitBehavioralProperties();


        }
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
