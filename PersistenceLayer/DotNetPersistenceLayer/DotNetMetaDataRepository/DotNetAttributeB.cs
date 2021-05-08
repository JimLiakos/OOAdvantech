namespace OOAdvantech.DotNetMetaDataRepository
{
    /// <MetaDataID>{A8B9711B-77A7-4180-BE77-034711CD99FD}</MetaDataID>
    public class Attribute : MetaDataRepository.Attribute
    {


        /// <MetaDataID>{3f196ced-0424-4095-b68b-c6fa2177730b}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        /// <MetaDataID>{3C9D8C77-F5E6-4C20-A805-86CDD61E8DB7}</MetaDataID>
        protected Attribute()
        {
        }
        /// <MetaDataID>{14ef9601-fe51-4cc8-9048-1e9f048adbc5}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Operation Getter
        {


            get
            {
                //Να ελεγχθεί τι κάνει το σύστημα με τις setter και getter operation στα AttributeRealization.
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

            set
            {
                //base.Getter = value;
            }
        }
        /// <MetaDataID>{9cf52633-ab54-4224-a971-6db7b6a40d66}</MetaDataID>
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
            set
            {

            }
        }

        bool FieldMemberLoaded = false;

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{418541DA-1DBE-426F-8322-1A3970FAD047}</MetaDataID>
        private System.Reflection.FieldInfo _FieldMember;
        /// <MetaDataID>{0E67C577-D320-444F-B82E-96C689043189}</MetaDataID>
        internal System.Reflection.FieldInfo FieldMember
        {
            get
            {
                if (wrMember == null)
                    return null;
                if (_FieldMember != null || FieldMemberLoaded)
                    return _FieldMember;

                FieldMemberLoaded = true;
                if (wrMember is System.Reflection.FieldInfo)
                {
                    _FieldMember = wrMember as System.Reflection.FieldInfo;
                    return _FieldMember;
                }

                object[] Attributes = wrMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), false);
                if (Attributes.Length > 0)
                {
                    //TODO τί γίνεται όταν το implementation field είναι δηλωμένο λάθος.
                    _FieldMember = (Attributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(wrMember as System.Reflection.PropertyInfo);
                    return _FieldMember;
                }
                return null;
            }
        }
        /// <MetaDataID>{496EF3CD-CC85-41C0-BCD0-DD39DE7DD2A4}</MetaDataID>
        public System.Reflection.MethodInfo[] Accessors;
        /// <MetaDataID>{81945BC9-FD77-42F9-BE72-2D75382F515F}</MetaDataID>
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
                    _Owner = DotNetMetaDataRepository.Type.GetClassifierObject(wrMember.DeclaringType);
                    return _Owner;
                }
            }
        }
        /// <summary/>
        /// <MetaDataID>{5B678368-047F-4418-90E9-C4948F4BCB74}</MetaDataID>
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
                    _Namespace = Owner;
                    return _Namespace;
                }
            }
        }
        /// <summary>Produce the identity of class from the .net metada </summary>
        /// <MetaDataID>{BB397B76-3DA9-4099-9818-F07EDD7795C7}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {

                return _Identity;
            }
        }
        /// <MetaDataID>{70110cd2-7ae9-4a8e-81bc-674db799953d}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.TemplateParameter ParameterizedType
        {
            get
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    if (_ParameterizedType == null)
                    {

                    }
                    return _ParameterizedType;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
            set
            {

            }
        }
        /// <MetaDataID>{73397D03-09D1-42C3-80B0-12B466AC7E40}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.Classifier Type
        {
            get
            {
                return _Type;
            }
            set
            {
            }
        }
        /// <MetaDataID>{F101036A-ACAF-46E8-A71E-F3DB6F53BDF5}</MetaDataID>
        public void SetNamespace(MetaDataRepository.Namespace theNamespace)
        {

        }


        /// <MetaDataID>{A21DE2C1-BDBA-49DF-9CF1-6EC9C385D6FD}</MetaDataID>
        private bool InErrorCheck = false;
        /// <MetaDataID>{8D0F01DE-60D9-4D08-84E6-AF83E9138FF4}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.ArrayList errors)
        {
            if (InErrorCheck)
                return false;
            try
            {
                InErrorCheck = true;
                bool hasError = base.ErrorCheck(ref errors);
                try
                {
                    if (wrMember != null)
                    {
                        object[] CustomAttributes = wrMember.GetCustomAttributes(typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute), false);
                        if (CustomAttributes.Length != 0)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "At '" + Owner.FullName + "." + Name + "' you can declare multiplicity attribute only on association end member.", FullName));
                            hasError = true;
                        }
                        CustomAttributes = wrMember.GetCustomAttributes(typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute), false);
                        if (CustomAttributes.Length != 0)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "At '" + Owner.FullName + "." + Name + "' you can declare multiplicity attribute only on association end member.", FullName));
                            hasError = true;
                        }


                    }
                    if (Persistent)
                    {
                        System.Reflection.FieldInfo fieldInfo = FieldMember;
                        if (fieldInfo == null)
                        {
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "System can't find implementation member for '" + Owner.FullName + "." + Name + "'", FullName));
                            return true;
                        }
                        if (fieldInfo.FieldType.IsInterface || (!fieldInfo.FieldType.IsValueType && !ByValueTypes.Contains(fieldInfo.FieldType.FullName)))
                        {

                            //	System.Collections.ArrayList interfaces=new System.Collections.ArrayList((wrMember as System.Reflection.PropertyInfo).PropertyType.GetInterfaces());
                            if (!Member<object>.IsMember(fieldInfo.FieldType))
                            {
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "The types of property '" + wrMember.DeclaringType + "." + wrMember.Name +
                                    "' can't be persistent member. ", FullName));
                                hasError = true;
                            }
                        }

                        if (wrMember is System.Reflection.PropertyInfo)
                        {

                            if (fieldInfo.FieldType != typeof(object) && fieldInfo.FieldType != (wrMember as System.Reflection.PropertyInfo).PropertyType)
                            {
                                if (fieldInfo.FieldType != (wrMember as System.Reflection.PropertyInfo).PropertyType)
                                {
                                    errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "Type mismatch between property '" + wrMember.DeclaringType + "." + wrMember.Name +
                                        "' and implementation field '" + fieldInfo.DeclaringType + "." + fieldInfo.Name + "'.", FullName));
                                    return true;
                                }


                                if (fieldInfo.FieldType.IsInterface)
                                {
                                    System.Collections.ArrayList interfaces = new System.Collections.ArrayList((wrMember as System.Reflection.PropertyInfo).PropertyType.GetInterfaces());
                                    if (!interfaces.Contains(fieldInfo.FieldType))
                                    {
                                        errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "The types of property '" + wrMember.DeclaringType + "." + wrMember.Name +
                                            "' and implementation field '" + fieldInfo.DeclaringType + "." + fieldInfo.Name + "' are incompatible.", FullName));
                                        return true;
                                    }
                                }
                                else
                                {
                                    if (fieldInfo.FieldType.IsValueType)
                                        if (!(wrMember as System.Reflection.PropertyInfo).PropertyType.IsSubclassOf(fieldInfo.FieldType))
                                        {
                                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "The types of property '" + wrMember.DeclaringType + "." + wrMember.Name +
                                                "' and implementation field '" + fieldInfo.DeclaringType + "." + fieldInfo.Name + "' are incompatible.", FullName));
                                            return true;
                                        }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception error)
                {
                    errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + error.Message, FullName));
                    return true;
                }
                return hasError;
            }
            finally
            {
                InErrorCheck = false;
            }
        }

        //		/// <MetaDataID>{8D42C545-0583-42A9-BA6D-49A4AD6FFA86}</MetaDataID>
        //		public override OOAdvantech.MetaDataRepository.Classifier Owner
        //		{
        //		}

        //		/// <MetaDataID>{C6B46907-9A1E-41C5-BFA6-61AF7DD22E38}</MetaDataID>
        //		public override OOAdvantech.MetaDataRepository.Namespace Namespace
        //		{
        //		}


        //		/// <MetaDataID>{0BCC397D-65AB-4DF3-BE66-59253A0138ED}</MetaDataID>
        //		public override MetaDataRepository.Classifier Type
        //		{
        //
        //		}
        //		/// <MetaDataID>{2C4141F5-4F7E-47A3-BC62-5F46AD19E7E5}</MetaDataID>
        //		public override MetaDataRepository.MetaObjectID Identity
        //		{
        //		}
        /// <MetaDataID>{4DCAFC6B-DE67-45EC-A667-B2A8560D75E5}</MetaDataID>
        internal System.Reflection.MemberInfo wrMember;

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


        /// <MetaDataID>{D8E2FB22-57E4-42D1-92A8-9915795082DB}</MetaDataID>
        public System.Reflection.PropertyInfo PropertyMember
        {
            get
            {
                if (wrMember == null)
                    return null;
                if (wrMember is System.Reflection.PropertyInfo)
                    return wrMember as System.Reflection.PropertyInfo;
                else
                    return null;
            }
        }
        /// <MetaDataID>{13CE6F8B-C0D1-4711-904E-04115AF005C2}</MetaDataID>
        public void AddExtensionMetaObject(object Value)
        {
            if (Value == null)
                return;

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                GetExtensionMetaObjects();
                if (!ExtensionMetaObjects.Contains(Value))
                    ExtensionMetaObjects.Add(Value);

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }


        /// <MetaDataID>{10E070C3-D158-4AA7-9C3B-5E5D614AF360}</MetaDataID>
        public Attribute(System.Reflection.FieldInfo mFieldInfo, MetaDataRepository.Classifier ownerClassifier)
        {
            Persistent = false;
            _Namespace = ownerClassifier;
            _Owner = ownerClassifier;


            if (mFieldInfo.IsPublic)
                Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
            if (mFieldInfo.IsFamily)
                Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
            if (mFieldInfo.IsPrivate)
                Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;
            _IsStatic = mFieldInfo.IsStatic;


            wrMember = mFieldInfo;
            Accessors = new System.Reflection.MethodInfo[0];

            if (!mFieldInfo.FieldType.IsGenericParameter)
                _Type = DotNetMetaDataRepository.Type.GetClassifierObject(mFieldInfo.FieldType);
            else
            {
                _ParameterizedType = MetaObjectMapper.FindMetaObjectFor(mFieldInfo.FieldType) as MetaDataRepository.TemplateParameter;
                if (_ParameterizedType == null)
                    _ParameterizedType = new TemplateParameter(new Type(mFieldInfo.FieldType));
            }




            //string fullName = wrMember.DeclaringType.FullName + "." + wrMember.Name;
            //if (fullName == "System.Xml.XmlValidatingReader.Item")
            //{
            //    int erter = 0;
            //}

            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(wrMember, this);


            _Name = wrMember.Name;
            object[] ObjectCustomAttributes = mFieldInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), true);
            if (ObjectCustomAttributes.Length > 0)
                Persistent = true;

            if (mFieldInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), true).Length > 0)
            {
                MetaDataRepository.AssociationClassRole AssociationClassRole = mFieldInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), true)[0] as MetaDataRepository.AssociationClassRole;
                PutPropertyValue("MetaData", "AssociationClassRole", true);
                PutPropertyValue("MetaData", "IsRoleA", AssociationClassRole.IsRoleA);
                PutPropertyValue("MetaData", "ImplMemberNameA", AssociationClassRole.ImplMemberName);
            }






            MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = null;
            object[] attributes = wrMember.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
            if (attributes.Length > 0)
                backwardCompatibilityID = (MetaDataRepository.BackwardCompatibilityID)attributes[0];

            if (backwardCompatibilityID != null)
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
                string indexerIdentity = null;
                if (wrMember is System.Reflection.PropertyInfo)
                {
                    System.Reflection.ParameterInfo[] indexParams = (wrMember as System.Reflection.PropertyInfo).GetIndexParameters();
                    foreach (System.Reflection.ParameterInfo indexParam in indexParams)
                    {
                        if (indexerIdentity != null)
                            indexerIdentity += ",";
                        indexerIdentity += indexParam.ParameterType.FullName;
                    }
                }
                if (indexerIdentity != null)
                    indexerIdentity = "(" + indexerIdentity + ")";
                _Identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + wrMember.Name + indexerIdentity);
            }
            if (wrMember.DeclaringType.FullName != null)
                MetaObjectMapper.AddMetaObject(this, wrMember.DeclaringType.FullName + "." + wrMember.Name);



        }

        /// <MetaDataID>{324BF60E-9B4A-495F-BE02-28D76396BE49}</MetaDataID>
        public Attribute(System.Reflection.PropertyInfo mPropertyInfo, MetaDataRepository.Classifier ownerClassifier)
        {
            _Namespace = ownerClassifier;
            _Owner = ownerClassifier;

            Persistent = false;
            wrMember = mPropertyInfo;
            if (mPropertyInfo.GetAccessors().Length > 0)
                Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
            else
                Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;





            //if (WrMethod.IsPublic)
            //    Visibility = MetaDataRepository.VisibilityKind.AccessPublic;
            //if (WrMethod.IsFamily)
            //    Visibility = MetaDataRepository.VisibilityKind.AccessProtected;
            //if (WrMethod.IsPrivate)
            //    Visibility = MetaDataRepository.VisibilityKind.AccessPrivate;

            //string fullName = wrMember.DeclaringType.FullName + "." + wrMember.Name;
            //if (fullName == "System.Xml.XmlValidatingReader.Item")
            //{
            //    int erter = 0;
            //}

            if (!mPropertyInfo.PropertyType.IsGenericParameter)
                _Type = DotNetMetaDataRepository.Type.GetClassifierObject(mPropertyInfo.PropertyType);
            else
            {
                _ParameterizedType = MetaObjectMapper.FindMetaObjectFor(mPropertyInfo.PropertyType) as MetaDataRepository.TemplateParameter;
                if (_ParameterizedType == null)
                    _ParameterizedType = new TemplateParameter(new Type(mPropertyInfo.PropertyType));
            }


            Accessors = mPropertyInfo.GetAccessors(true);
            _IsStatic = Accessors[0].IsStatic;


            DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(wrMember, this);

            _Name = wrMember.Name;
            object[] ObjectCustomAttributes = mPropertyInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), true);
            if (ObjectCustomAttributes.Length > 0)
                Persistent = true;
            if (mPropertyInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), true).Length > 0)
            {
                MetaDataRepository.AssociationClassRole AssociationClassRole = mPropertyInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole), true)[0] as MetaDataRepository.AssociationClassRole;
                PutPropertyValue("MetaData", "AssociationClassRole", true);
                PutPropertyValue("MetaData", "IsRoleA", AssociationClassRole.IsRoleA);
                PutPropertyValue("MetaData", "ImplMemberNameA", AssociationClassRole.ImplMemberName);
            }

            MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = null;
            object[] attributes = wrMember.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
            if (attributes.Length > 0)
                backwardCompatibilityID = (MetaDataRepository.BackwardCompatibilityID)attributes[0];

            if (backwardCompatibilityID != null)
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
                string indexerIdentity = null;
                if (wrMember is System.Reflection.PropertyInfo)
                {
                    System.Reflection.ParameterInfo[] indexParams = (wrMember as System.Reflection.PropertyInfo).GetIndexParameters();
                    foreach (System.Reflection.ParameterInfo indexParam in indexParams)
                    {
                        if (indexerIdentity != null)
                            indexerIdentity += ",";
                        indexerIdentity += indexParam.ParameterType.FullName;
                    }
                }
                if (indexerIdentity != null)
                    indexerIdentity = "(" + indexerIdentity + ")";
                _Identity = new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString() + "." + wrMember.Name + indexerIdentity);
            }
            if (wrMember.DeclaringType.FullName != null)
                MetaObjectMapper.AddMetaObject(this, wrMember.DeclaringType.FullName + "." + wrMember.Name);




        }


        public override object GetExtensionMetaObject(System.Type MetaObjectType)
        {


            if (MetaObjectType == typeof(System.Reflection.FieldInfo))
                return FieldMember;

            if (MetaObjectType == typeof(System.Reflection.PropertyInfo))
                return PropertyMember;
            
            return base.GetExtensionMetaObject(MetaObjectType);
        }
        /// <MetaDataID>{F8CF58E8-005B-4F3F-9FB4-F9ACEF00FBB5}</MetaDataID>
        private System.Collections.ArrayList ExtensionMetaObjects;
        /// <MetaDataID>{812F2068-1904-4106-AFFF-45BF03FAC9E3}</MetaDataID>
        public override System.Collections.ArrayList GetExtensionMetaObjects()
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (ExtensionMetaObjects == null)
                {
                    ExtensionMetaObjects = new System.Collections.ArrayList(1);
                    ExtensionMetaObjects.Add(wrMember);
                }
                return ExtensionMetaObjects;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }
    }
}
