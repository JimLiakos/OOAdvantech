namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{A8B9711B-77A7-4180-BE77-034711CD99FD}</MetaDataID>
	public class Attribute : MetaDataRepository.Attribute
	{

		bool InErrorCheck=false;
		public override bool ErrorCheck(ref string errors)
		{
			if(InErrorCheck)
				return false;
			try
			{
				InErrorCheck=true;
				bool hasError=base.ErrorCheck(ref errors); 
				try
				{
					if(Persistent)
					{
						System.Reflection.FieldInfo fieldInfo=FieldMember;
						if(fieldInfo==null)
						{
							errors+="\n"+"System can't find implementation member for '"+Owner.FullName+"."+Name +"'";
							return true;
						}
						if(wrMember is System.Reflection.PropertyInfo)
						{
							if(fieldInfo.FieldType!=typeof(object)&&fieldInfo.FieldType!=(wrMember as System.Reflection.PropertyInfo).PropertyType)
							{
								if(fieldInfo.FieldType!=(wrMember as System.Reflection.PropertyInfo).PropertyType)
								{
									errors+="\n"+"Type mismatch between property '"+wrMember.DeclaringType+"."+wrMember.Name+
										"' and implementation field '"+fieldInfo.DeclaringType+"."+fieldInfo.Name+"'.";
									return true;
								}


								if(fieldInfo.FieldType.IsInterface)
								{
									System.Collections.ArrayList interfaces=new System.Collections.ArrayList((wrMember as System.Reflection.PropertyInfo).PropertyType.GetInterfaces());
									if(!interfaces.Contains(fieldInfo.FieldType))
									{
										errors+="\n"+"The types of property '"+wrMember.DeclaringType+"."+wrMember.Name+
											"' and implementation field '"+fieldInfo.DeclaringType+"."+fieldInfo.Name+"' are incompatible.";
										return true;
									}
								}
								else
								{
									if(!(wrMember as System.Reflection.PropertyInfo).PropertyType.IsSubclassOf(fieldInfo.FieldType))
									{
										errors+="\n"+"The types of property '"+wrMember.DeclaringType+"."+wrMember.Name+
											"' and implementation field '"+fieldInfo.DeclaringType+"."+fieldInfo.Name+"' are incompatible.";
										return true;
									}
								}
							}
						}
					}
				}
				catch(System.Exception error)
				{
					errors+="\n"+error.Message;
					return true;
				}
				return hasError;
			}
			finally
			{
				InErrorCheck=false;
			}
		}

		public override OOAdvantech.MetaDataRepository.Classifier Owner
		{
			get
			{
				if(base.Owner!=null)
					return base.Owner;

				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					_Owner=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(wrMember.DeclaringType) as MetaDataRepository.Classifier;
					if(_Owner!=null)
						return _Owner;
					if(wrMember.DeclaringType.IsInterface)
					{
						Interface _interface=new Interface(new DotNetMetaDataRepository.Type(wrMember.DeclaringType));
						_Owner=_interface;
					}
					if(wrMember.DeclaringType.IsClass)
					{
						Class _class=new Class(new DotNetMetaDataRepository.Type(wrMember.DeclaringType));
						_Owner=_class;
					}
					if(wrMember.DeclaringType.IsPrimitive)
					{
						Primitive _primitive=new Primitive(new DotNetMetaDataRepository.Type(wrMember.DeclaringType));
						_Owner=_primitive;
					}
					if(wrMember.DeclaringType.IsPrimitive)
					{
						Primitive _primitive=new Primitive(new DotNetMetaDataRepository.Type(wrMember.DeclaringType));
						_Owner=_primitive;
					}

					return _Owner;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}

			}
		}

		public override OOAdvantech.MetaDataRepository.Namespace Namespace
		{
			get
			{
				if(base.Namespace!=null)
					return base.Namespace;

				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					_Namespace=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(wrMember.DeclaringType) as MetaDataRepository.Namespace;
					if(_Namespace!=null)
						return _Namespace;
					if(wrMember.DeclaringType.IsInterface)
					{
						Interface _interface=new Interface(new DotNetMetaDataRepository.Type(wrMember.DeclaringType));
						_Namespace=_interface;

					}
					if(wrMember.DeclaringType.IsClass)
					{
						Class _class=new Class(new DotNetMetaDataRepository.Type(wrMember.DeclaringType));
						_Namespace=_class;
					}
					if(wrMember.DeclaringType.IsPrimitive)
					{
						Primitive _primitive=new Primitive(new DotNetMetaDataRepository.Type(wrMember.DeclaringType));
						_Namespace=_primitive;
					}
					return _Namespace;

				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}


			}
		}


		/// <MetaDataID>{0BCC397D-65AB-4DF3-BE66-59253A0138ED}</MetaDataID>
		public override MetaDataRepository.Classifier Type
		{

			get
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{

					if(_Type==null)
					{
						System.Type AttributeType=null;
						if(typeof(System.Reflection.PropertyInfo).IsInstanceOfType(wrMember))
							AttributeType=((System.Reflection.PropertyInfo)wrMember).PropertyType;

						if(typeof(System.Reflection.FieldInfo ).IsInstanceOfType(wrMember))
							AttributeType=((System.Reflection.FieldInfo)wrMember).FieldType;

						MetaDataRepository.MetaObject mMetaObject=MetaObjectMapper.FindMetaObjectFor(AttributeType);
						if(mMetaObject==null)
						{
							Type mType=new Type(AttributeType);
							if(AttributeType.IsClass)
							{
								Class mClass=new Class(mType);
								mMetaObject=mClass;
							}else if(AttributeType.IsPrimitive)
							{
								Primitive mPrimitive=new Primitive (mType);
								mMetaObject=mPrimitive;
							}else if(AttributeType.IsInterface)
							{
								Interface mInterface=new Interface(mType);
								mMetaObject=mInterface;
							}else if(AttributeType.IsEnum)
							{
								Enumeration mEnumeration=new Enumeration(mType);
								mMetaObject=mEnumeration;
 
							}else if(AttributeType.IsValueType/*structs*/)
							{
								Structure mStructure=new Structure(mType);
								mMetaObject=mStructure;
							}

						}
						if(mMetaObject!=null)
							_Type=(MetaDataRepository.Classifier)mMetaObject;
					}
					return _Type;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}
		/// <MetaDataID>{2C4141F5-4F7E-47A3-BC62-5F46AD19E7E5}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				MetaDataRepository.MetaObjectID oldIdentity=_Identity;
				try
				{
					if(_Identity!=null)
						return _Identity;

					bool plus=false;
					object[] Attributes=wrMember.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID),false);
					if(Attributes.Length>0)
					{
						// Error prone πολύ σπατάλη σε πόρους μνήμης
						_Identity=new OOAdvantech.MetaDataRepository.MetaObjectID(Attributes[0].ToString());
						string identityAsString=_Identity.ToString().Trim();
						if(identityAsString.Length>0)
							if(identityAsString[0]=='+')
							{
								plus=true;
								identityAsString=identityAsString.Substring(1);
								_Identity=new OOAdvantech.MetaDataRepository.MetaObjectID(identityAsString);
							}
					}


					string indexerIdentity=null;
					if(wrMember is System.Reflection.PropertyInfo)
					{
						System.Reflection.ParameterInfo[] indexParams=(wrMember as System.Reflection.PropertyInfo).GetIndexParameters();
						foreach(System.Reflection.ParameterInfo indexParam in indexParams)
						{
							if(indexerIdentity!=null)
								indexerIdentity+=",";
							indexerIdentity+=indexParam.ParameterType.FullName;
						}
					}
					if(indexerIdentity!=null)
						indexerIdentity="("+indexerIdentity+")";



					if(_Identity==null)
					{
						
						_Identity=new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString()+"."+wrMember.Name+indexerIdentity);
						return _Identity;
					}
					else
					{
						if(plus)
							_Identity=new MetaDataRepository.MetaObjectID(Namespace.Identity+"."+_Identity.ToString());
					}
					return _Identity;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
					if(_Identity.ToString()=="System.Xml.XmlDocument.PreserveWhitespace"&&oldIdentity==null)
					{
						int erter=0;
						object tt=MetaObjectMapper.FindMetaObject(_Identity);
						erter++;
						
					}

					if(oldIdentity==null)
						MetaObjectMapper.AddMetaObject(this,wrMember.DeclaringType.FullName+"."+wrMember.Name);
				}
			}
		}
		/// <MetaDataID>{4DCAFC6B-DE67-45EC-A667-B2A8560D75E5}</MetaDataID>
		internal System.Reflection.MemberInfo wrMember;
	

		public System.Reflection.FieldInfo _FieldMember;
		public System.Reflection.FieldInfo FieldMember
		{
			get
			{
				if( wrMember==null)
					return null;
				if( _FieldMember!=null)
					return _FieldMember;
                if(wrMember is System.Reflection.FieldInfo)
				{
					_FieldMember =wrMember as System.Reflection.FieldInfo;
					return _FieldMember ;
				}

				object[] Attributes=wrMember.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember),false);
				if(Attributes.Length>0)
				{
					_FieldMember=(Attributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(wrMember as System.Reflection.PropertyInfo);
					return _FieldMember;
				}
				return null;
			}
		}
		public System.Reflection.PropertyInfo PropertyMember
		{
			get
			{
				if( wrMember==null)
					return null;
				if(wrMember is System.Reflection.PropertyInfo)
					return wrMember as System.Reflection.PropertyInfo;
				else
					return null;
			}
		}

		/// <MetaDataID>{10E070C3-D158-4AA7-9C3B-5E5D614AF360}</MetaDataID>
		 public Attribute(System.Reflection.FieldInfo mFieldInfo)
		{
			 Persistent=false;
			 wrMember=mFieldInfo;

			 string fullName=wrMember.DeclaringType.FullName+"."+wrMember.Name;
			 if(fullName=="System.Xml.XmlValidatingReader.Item")
			 {
				 int erter=0;
			 }

			 DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(wrMember,this);

			 
			 Name=wrMember.Name;
			 object[] ObjectCustomAttributes=mFieldInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember), true);
			 if(ObjectCustomAttributes.Length>0)
				  Persistent=true;

			 if(mFieldInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole),true).Length>0)
			 {
				 MetaDataRepository.AssociationClassRole AssociationClassRole =mFieldInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole),true)[0] as MetaDataRepository.AssociationClassRole;
                 PutPropertyValue("MetaData","AssociationClassRole",true);
				 PutPropertyValue("MetaData","IsRoleA",AssociationClassRole.IsRoleA);
				 PutPropertyValue("MetaData","ImplMemberNameA",AssociationClassRole.ImplMemberName);
			 }
			

		}
		/// <MetaDataID>{324BF60E-9B4A-495F-BE02-28D76396BE49}</MetaDataID>
		 public Attribute(System.Reflection.PropertyInfo mPropertyInfo)
		{
			 Persistent=false;
			 wrMember=mPropertyInfo;
			 string fullName=wrMember.DeclaringType.FullName+"."+wrMember.Name;
			 if(fullName=="System.Xml.XmlValidatingReader.Item")
			 {
				 int erter=0;
			 }

			 DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(wrMember,this);
			 
			 Name=wrMember.Name;
			 object[] ObjectCustomAttributes=mPropertyInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember),true);
			 if(ObjectCustomAttributes.Length>0)
				 Persistent=true;
			 if(mPropertyInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole),true).Length>0)
			 {
				 MetaDataRepository.AssociationClassRole AssociationClassRole =mPropertyInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole),true)[0] as MetaDataRepository.AssociationClassRole;
				 PutPropertyValue("MetaData","AssociationClassRole",true);
				 PutPropertyValue("MetaData","IsRoleA",AssociationClassRole.IsRoleA);
				 PutPropertyValue("MetaData","ImplMemberNameA",AssociationClassRole.ImplMemberName);
			 }
			 
			


		 }
		/// <MetaDataID>{812F2068-1904-4106-AFFF-45BF03FAC9E3}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			ReaderWriterLock.AcquireReaderLock(10000);
			try
			{
				System.Collections.ArrayList ExtensionMetaObjects=new System.Collections.ArrayList(1);
				ExtensionMetaObjects.Add(wrMember);
				return ExtensionMetaObjects; 
			}
			finally
			{
				ReaderWriterLock.ReleaseReaderLock(); 
			}
		}
	}
}
