using System;

namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{F875931E-BDB1-4FED-9E14-CB29A26432E7}</MetaDataID>
	public class AttributeRealization : OOAdvantech.MetaDataRepository.AttributeRealization
    {
        public override object GetValue(object _object)
        {

            if (FastPropertyAccessor != null)
                return FastPropertyAccessor.GetValue(_object);

            if (FastFieldAccessor != null)
                return FastFieldAccessor.GetValue(_object);

            throw new NotImplementedException();
        }
        public override bool Multilingual
        {
            get
            {
                if (FieldMember != null && FieldMember.FieldType.IsGenericType && FieldMember.FieldType.GetGenericTypeDefinition() == typeof(MultilingualMember<>))
                    return true;

                return base.Multilingual;
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
            if (member.Name == nameof(_FieldMember))
            {
                if (value == null)
                    _FieldMember = default(System.Reflection.FieldInfo);
                else
                    _FieldMember = (System.Reflection.FieldInfo)value;
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
            if (member.Name == nameof(FieldMemberLoaded))
            {
                if (value == null)
                    FieldMemberLoaded = default(bool);
                else
                    FieldMemberLoaded = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(InErrorCheck))
                return InErrorCheck;

            if (member.Name == nameof(_FastFieldAccessor))
                return _FastFieldAccessor;

            if (member.Name == nameof(_FastPropertyAccessor))
                return _FastPropertyAccessor;

            if (member.Name == nameof(_FieldMember))
                return _FieldMember;

            if (member.Name == nameof(_PropertyMember))
                return _PropertyMember;

            if (member.Name == nameof(FieldMemberLoaded))
                return FieldMemberLoaded;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{f91759ea-0f79-4051-a045-2da0974f95d4}</MetaDataID>
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            //base.Synchronize(OriginMetaObject);
        }
        public override void ShallowSynchronize(MetaDataRepository.MetaObject originClassifier)
        {

        }

        /// <MetaDataID>{2C682556-D26A-4496-9B57-8F000F167C21}</MetaDataID>
		protected AttributeRealization()
		{ 
		}


        public override object GetExtensionMetaObject(System.Type MetaObjectType)
        {

            if (MetaObjectType == typeof(System.Reflection.FieldInfo) || MetaObjectType.GetMetaData().IsSubclassOf(typeof(System.Reflection.FieldInfo)))
                return FieldMember;

            if (MetaObjectType == typeof(System.Reflection.PropertyInfo) || MetaObjectType.GetMetaData().IsSubclassOf(typeof(System.Reflection.PropertyInfo)))
                return PropertyMember;

            return base.GetExtensionMetaObject(MetaObjectType);
        }

		/// <MetaDataID>{70B7CCA1-D906-4BA9-974C-75401C873BB5}</MetaDataID>
		public override OOAdvantech.MetaDataRepository.Classifier Type
		{
			get
			{
				return Specification.Type;
			}
		}
        /// <MetaDataID>{e82c05b7-9362-4a7a-9dc2-052316b66599}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.TemplateParameter ParameterizedType
        {
            get
            {
                return Specification.ParameterizedType;
            }
            set
            {
                base.ParameterizedType = value;
            }
        }
		/// <MetaDataID>{0A4B3AB4-0426-40AB-A9D4-CCD190C57AFB}</MetaDataID>
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
                    _Owner = DotNetMetaDataRepository.Type.GetClassifierObject(PropertyMember.DeclaringType);
                    return _Owner;
                }
            }		
		}
		/// <summary>Produce the identity of class from the .net metada </summary>
		/// <MetaDataID>{36649EC6-EF0B-4BA3-8165-55082364FDA7}</MetaDataID>
		public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
		{
			get
			{
                return _Identity;

			}
		}
		/// <MetaDataID>{268F2438-43D9-4E28-A4A7-72BA01F53B0C}</MetaDataID>
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
                    _Namespace.Value = Owner;
                    return _Namespace;
                }
            }
		}
		/// <MetaDataID>{3E4EAF6A-FBFA-42F1-A849-7D7C28C987D1}</MetaDataID>
		private bool InErrorCheck;
		/// <MetaDataID>{FEB300C2-485A-46AC-AD54-7F623A400975}</MetaDataID>
        public override bool ErrorCheck(ref System.Collections.Generic.List<MetaDataError> errors)
		{
			if(InErrorCheck)
				return false;
			try
			{
				InErrorCheck=true;
				bool hasError=base.ErrorCheck(ref errors);
                if (_Persistent || (Specification.Persistent == true && !(Namespace as Class).Abstract))
				{
					object[] customAttributes =_PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember),false);
					if(customAttributes.Length>0)
					{
						try
						{
							System.Reflection.FieldInfo fieldMember=(customAttributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(_PropertyMember);
					
						}
						catch(System.Exception error)
						{
							hasError=true;
							errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: "+error.Message,FullName));
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
					if(fieldInfo==null)
					{
                        if (Namespace is Class)
                        {
                            try
                            {
                                OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = (Namespace as Class).GetFastFieldAccessor(Specification as Attribute);
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
                                OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = (Namespace as Structure).GetFastFieldAccessor(Specification as Attribute);
                            }
                            catch (System.Exception error)
                            {
                                errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + error.Message, FullName));
                                hasError = true;
                            }
                        }
                        return hasError;

					}
                    else if(fieldInfo.FieldType.GetMetaData().IsInterface||(!fieldInfo.FieldType.GetMetaData().IsValueType&&!MetaDataRepository.Attribute.ByValueTypes.Contains(fieldInfo.FieldType.FullName)))
					{
                        if (!Member<object>.IsMember(fieldInfo.FieldType))
                        {

                            //	System.Collections.ArrayList interfaces=new System.Collections.ArrayList((wrMember as System.Reflection.PropertyInfo).PropertyType.GetInterfaces());
                            errors.Add(new MetaDataRepository.MetaObject.MetaDataError("MDR Error: " + "The types of property '" + _PropertyMember.DeclaringType + "." + _PropertyMember.Name +
                                "' can't be persistent member. ", FullName));
                            hasError = true;
                        }
					}
               

				}
				return hasError;
			}
			finally
			{
				InErrorCheck=false;
			}
		}

        /// <exclude>Excluded</exclude>
        AccessorBuilder.FieldPropertyAccessor _FastFieldAccessor;
        /// <MetaDataID>{d0eb9615-63a9-4765-88ea-2ea8705b61c5}</MetaDataID>
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
        /// <MetaDataID>{c5dc8c69-c7a4-4295-a9b3-1dfff66350ac}</MetaDataID>
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
		/// <MetaDataID>{DDB01746-9197-4644-95C7-7CB3F8F7DC1D}</MetaDataID>
		private System.Reflection.FieldInfo _FieldMember;
		/// <MetaDataID>{7D852C7F-E325-4540-8232-5A3D21A74149}</MetaDataID>
		public System.Reflection.FieldInfo FieldMember
		{
			get
			{
				if( PropertyMember==null)
					return null;
				if( _FieldMember!=null)
					return _FieldMember;

				object[] Attributes=PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember),false);
				if(Attributes.Length>0)
				{
					//TODO τί γίνεται όταν το implementation field είναι δηλωμένο λάθος.
					_FieldMember=(Attributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(PropertyMember as System.Reflection.PropertyInfo);
					return _FieldMember;
				}
				return null;
			}
		}
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{5BCDEBA0-7BE2-4B6B-8CB7-6274688CE932}</MetaDataID>
		private System.Reflection.PropertyInfo _PropertyMember;
		/// <MetaDataID>{3DEFEF4C-A999-4679-9B20-07B53E645B6D}</MetaDataID>
		public System.Reflection.PropertyInfo PropertyMember
		{
			get
			{
				return _PropertyMember;
			}
		}
		/// <MetaDataID>{AFD78227-5182-4D45-9416-73DE13150281}</MetaDataID>
		private bool FieldMemberLoaded;
		/// <MetaDataID>{C4C7A9D4-C550-4917-845B-F6EE78BC8424}</MetaDataID>
        public AttributeRealization(System.Reflection.PropertyInfo property, Attribute attribute, MetaDataRepository.Classifier owner)
        {
            _Owner = owner;
            _Namespace.Value = owner;
            _Name = attribute.Name;
            _Specification = attribute;
            attribute.AddAttributeRealization(this);
            _PropertyMember = property;
            if (_PropertyMember != null)
            { 
                _Name = _PropertyMember.Name;
                DotNetMetaDataRepository.MetaObjectMapper.RemoveType(_PropertyMember);
                DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(_PropertyMember, this);
            }
             
            object[] customAttributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), false);
            if (customAttributes.Length > 0)
            {
                if(!property.PropertyType.GetMetaData().IsValueType)
                    PutPropertyValue("Persistent", "SizeOf", (customAttributes[0] as MetaDataRepository.PersistentMember).Length);
                _Persistent = true;
            }
            TransactionalMember = _PropertyMember.GetCustomAttributes(typeof(OOAdvantech.Transactions.TransactionalMemberAttribute), true).Length > 0;


            MetaDataRepository.BackwardCompatibilityID backwardCompatibilityID = null;
            object[] attributes = _PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID), false);
            if (attributes.Length > 0)
                backwardCompatibilityID = (MetaDataRepository.BackwardCompatibilityID)attributes[0];

            if (backwardCompatibilityID != null && !string.IsNullOrEmpty(backwardCompatibilityID.ToString()))
            {
                string identityAsString = backwardCompatibilityID.ToString();
                if (identityAsString[0] == '+')
                { 
                    identityAsString = identityAsString.Substring(1);
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("AR_" + Namespace.Identity + "." + identityAsString);
                }
                else
                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("AR_" + identityAsString);
            } 
            else
            {
                string indexerIdentity = null;
                foreach (System.Reflection.ParameterInfo parameterInfo in _PropertyMember.GetIndexParameters())
                {
                    if (indexerIdentity == null)
                        indexerIdentity = "(";
                    else
                        indexerIdentity += ",";
                    indexerIdentity += parameterInfo.ParameterType.FullName;
                }
                indexerIdentity += ")";
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID("AR_" + Namespace.Identity + "." + PropertyMember.Name + indexerIdentity);

            }
            if (_PropertyMember.DeclaringType.FullName!=null)
                MetaObjectMapper.AddMetaObject(this, _PropertyMember.DeclaringType.FullName + "." + _PropertyMember.Name);


        }
	
	}
}
