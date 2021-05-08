namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{C9BE7F05-C134-4AFE-BDB5-4334236B2D68}</MetaDataID>
	public class AssociationEnd : MetaDataRepository.AssociationEnd
	{

		bool InErrorCheck=false;
		public override bool ErrorCheck(ref string errors)
		{
			if(InErrorCheck)
				return false;
			try
			{
				string Iden=Identity.ToString();
				InErrorCheck=true;
				bool hasError=base.ErrorCheck(ref errors); 
				if(Association.Name==null||Association.Name.Length==0)
				{
					if(WrMemberInfo!=null)
					{
						errors+="There isn't Association Name for class member "+WrMemberInfo.DeclaringType.FullName+"."+WrMemberInfo.Name;  
						hasError= true;
					}
				}
				System.Type memberTpe;
				if(WrMemberInfo !=null)
				{
					if(WrMemberInfo is System.Reflection.FieldInfo)
						memberTpe=(WrMemberInfo as System.Reflection.FieldInfo).FieldType;
					else
						memberTpe=(WrMemberInfo as System.Reflection.PropertyInfo).PropertyType;

					if(Multiplicity.LowLimit>Multiplicity.HighLimit&&!Multiplicity.NoHighLimit&&!Multiplicity.Unspecified)
					{
						hasError= true;
						
						errors+="\nWrong multiplicity at association end '"+GetOtherEnd().Specification.FullName+"."+Name+
							"' the low limit is greater than high limit.";
					}


					if(Multiplicity.IsMany&&!memberTpe.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
					{
						hasError= true;
						errors+="\nThe multiplicity of association end '"+GetOtherEnd().Specification.FullName+"."+Name+
								"' is many and the type isn't collection subclass of 'OOAdvantech.PersistenceLayer.ObjectContainer";
					}

					if(!Multiplicity.IsMany)
					{
						System.Type specificationType=Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
						if(memberTpe!=specificationType)
						{
							hasError= true;
							errors+="\nAssociation end '"+GetOtherEnd().Specification.FullName+"."+Name+"' type mismatch."+
								" Check type declaration at Association attribute."+
								"\nIn zero to one or exactly one relationships, the type of field or property must be the same with the type at Association attribute." ;
						}
					}
					
					if(WrMemberInfo!=null&&Association.LinkClass!=null&&Association.LinkClass.LinkAssociation!=Association)
					{
						object[]  customAttributes= WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass),false);
						if(customAttributes.Length>0)
						{
							System.Type declaredAssType= (customAttributes[0] as MetaDataRepository.AssociationClass).AssocciationClass;
							if(declaredAssType==Association.LinkClass.GetExtensionMetaObject(typeof(System.Type)))
							{

								hasError= true;
								errors+="\nError on Association end '"+GetOtherEnd().Specification.FullName+"."+Name+"' the type '"+
									Association.LinkClass.FullName+"' hasn't been declared as association type of '"+Association.Name+"' association.";
							}
						}
					}

				}

				if(WrMemberInfo!=null)
				{
					object[]  customAttributes=WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationAttribute),false);
					if(customAttributes.Length>0)
					{
						MetaDataRepository.AssociationAttribute associationAttribute=customAttributes[0] as  MetaDataRepository.AssociationAttribute;
						if((GetOtherEnd() as AssociationEnd).WrMemberInfo!=null)
						{
							customAttributes=(GetOtherEnd() as AssociationEnd).WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationAttribute),false);
							if(customAttributes.Length>0)
							{
								if(associationAttribute.IsRoleA==(customAttributes[0] as MetaDataRepository.AssociationAttribute).IsRoleA)
								{
									hasError=true;
									errors+="\nThe association ends '"+WrMemberInfo.DeclaringType.FullName+"."+WrMemberInfo.Name+"' and '"+(GetOtherEnd() as AssociationEnd).WrMemberInfo.DeclaringType.FullName+"."+(GetOtherEnd() as AssociationEnd).WrMemberInfo.Name+"' has the same role."; 
								}
							}
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

		/// <MetaDataID>{2749A6C9-1FAD-4EEB-B534-6B75596EB99D}</MetaDataID>
		public OOAdvantech.MetaDataRepository.Operation Setter
		{
			get
			{
				return _Setter;
			}
		}
		/// <MetaDataID>{BA0323C2-8D37-4ED5-B523-03ED2C0E716E}</MetaDataID>
		public OOAdvantech.MetaDataRepository.Operation Getter
		{
			get
			{
				/*if(PropertyMember!=null&&_Getter==null)
				{
					System.Reflection.MethodInfo getMethod= PropertyMember.GetGetMethod(true);
					if(GetMethod!=null)
					{
						System.Reflection.MethodInfo baseDefinition= GetMethod.GetBaseDefinition();
						

						Operation getterOperation=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(baseDefinition) as Operation;
						if(getterOperation==null)
							getterOperation=new Operation(baseDefinition);
						if(!getMethod.IsAbstract)
							getterOperation.i


					}

				}*/
				return _Getter;
			}
		}
	
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{C35BF073-E660-4F2C-A525-DBA6501BC955}</MetaDataID>
		private System.Reflection.FieldInfo _FieldMember;
		/// <MetaDataID>{F2695D9F-98D7-4127-94EB-A24BE7D63537}</MetaDataID>
		internal System.Reflection.FieldInfo FieldMember
		{
			get
			{
				if( WrMemberInfo==null)
					return null;
				if( _FieldMember!=null)
					return _FieldMember;
				if(WrMemberInfo is System.Reflection.FieldInfo)
				{
					_FieldMember =WrMemberInfo as System.Reflection.FieldInfo;
					return _FieldMember ;
				}
				object[] Attributes=WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember),false);
				if(Attributes.Length>0)
				{
					_FieldMember=(Attributes[0] as MetaDataRepository.PersistentMember).GetImplementationField(WrMemberInfo as System.Reflection.PropertyInfo);
					return _FieldMember;
				}
				return null;
			}
		}
		/// <MetaDataID>{5F36B3E8-86E6-47AD-AA3A-85D857AFBB17}</MetaDataID>
		private System.Type _SpecificationType;
		/// <MetaDataID>{34B62BBD-8538-489B-B95E-F3765430CEEC}</MetaDataID>
		public System.Type SpecificationType
		{
			get
			{
				if(_SpecificationType!=null)
					return _SpecificationType;
				else
				{
					_SpecificationType=Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
					return _SpecificationType;
				}
			}
		}


		/// <MetaDataID>{B4D1C221-3185-47DC-BF2E-192C28AF3EBE}</MetaDataID>
		private System.Collections.ArrayList ExtensionMetaObjects;
		/// <MetaDataID>{1F86A1CF-D20A-4C56-B101-FAF6093D8F8F}</MetaDataID>
		public void AddExtensionMetaObject(object Value)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
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
		/// <MetaDataID>{A1BB6A82-C056-43BF-B039-159E7675981B}</MetaDataID>
		public override MetaDataRepository.MultiplicityRange Multiplicity
		{
			get
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					if(_Multiplicity==null)
					{
						
						if(WrMemberInfo==null)
						{
							MetaDataRepository.MultiplicityRange mMultiplicityRange=GetOtherEnd().Multiplicity;
							if(_Multiplicity==null)
								_Multiplicity=new MetaDataRepository.MultiplicityRange();
							return _Multiplicity;
						}
	
						MetaDataRepository.RoleAMultiplicityRangeAttribute RoleAMultiplicity=null;
						MetaDataRepository.RoleBMultiplicityRangeAttribute RoleBMultiplicity=null;

						object[] CustomAttributes= WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.RoleAMultiplicityRangeAttribute),false);
						if(CustomAttributes.Length!=0)
							RoleAMultiplicity= CustomAttributes[0] as MetaDataRepository.RoleAMultiplicityRangeAttribute;

						
						CustomAttributes= WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.RoleBMultiplicityRangeAttribute),false);
						if(CustomAttributes.Length!=0)
							RoleBMultiplicity= CustomAttributes[0] as MetaDataRepository.RoleBMultiplicityRangeAttribute;
						if(IsRoleA)
						{
							if(RoleAMultiplicity!=null)
								_Multiplicity=RoleAMultiplicity.Multiplicity;
							else
								_Multiplicity=new MetaDataRepository.MultiplicityRange();
							if(RoleBMultiplicity!=null)
							{
								MetaDataRepository.MultiplicityRange OtherEndMultiplicityRange=GetOtherEnd().Multiplicity;
								OtherEndMultiplicityRange.HighLimit=RoleBMultiplicity.Multiplicity.HighLimit;
								OtherEndMultiplicityRange.LowLimit=RoleBMultiplicity.Multiplicity.LowLimit;
								OtherEndMultiplicityRange.NoHighLimit=RoleBMultiplicity.Multiplicity.NoHighLimit;
								OtherEndMultiplicityRange.Unspecified=RoleBMultiplicity.Multiplicity.Unspecified;
							}
						}
						else
						{
							if(RoleBMultiplicity!=null)
								_Multiplicity=RoleBMultiplicity.Multiplicity;
							else
								_Multiplicity=new MetaDataRepository.MultiplicityRange();

							if(RoleAMultiplicity!=null)
							{
								MetaDataRepository.MultiplicityRange OtherEndMultiplicityRange=GetOtherEnd().Multiplicity;
								OtherEndMultiplicityRange.HighLimit=RoleAMultiplicity.Multiplicity.HighLimit;
								OtherEndMultiplicityRange.LowLimit=RoleAMultiplicity.Multiplicity.LowLimit;
								OtherEndMultiplicityRange.NoHighLimit=RoleAMultiplicity.Multiplicity.NoHighLimit;
								OtherEndMultiplicityRange.Unspecified=RoleAMultiplicity.Multiplicity.Unspecified;
							}
						}
						return _Multiplicity;
					}
					return _Multiplicity;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}

			}
		}
		/// <MetaDataID>{B33E85FF-A6ED-49CB-BEC2-4A10FC0E4339}</MetaDataID>
		public override string Name
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					if(base.Name!=null)
						return base.Name;
					if(IsRoleA)
						return Association.Name+"RoleAName";
					else
						return Association.Name+"RoleBName";
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock(); 
				}
			}
			set
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					base.Name=value;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);

				}
			}
		}
		/// <MetaDataID>{8AA6ECBB-C8DF-423D-982A-7E11330E3EA4}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{

				MetaDataRepository.MetaObjectID oldIdentity=_Identity;
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{

					if(_Identity!=null)
						return _Identity;
					if(WrMemberInfo==null)
					{
						if(IsRoleA)
							_Identity=new MetaDataRepository.MetaObjectID(GetOtherEnd().Name+GetOtherEnd().Specification.Identity.ToString()+".NoNameRoleA."+Specification.Identity+"."+GetOtherEnd().Name);//Error prone 
						else
							_Identity=new MetaDataRepository.MetaObjectID( GetOtherEnd().Specification.Identity.ToString()+".NoNameRoleB."+Specification.Identity+"."+GetOtherEnd().Name);//Error prone 
						
						return _Identity;
					}
					object[] Attributes=WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID),false);
					bool plus=false;
					if(Attributes.Length>0)
					{
						_Identity=new OOAdvantech.MetaDataRepository.MetaObjectID(((MetaDataRepository.BackwardCompatibilityID)Attributes[0]).ToString());
						string identityAsString=_Identity.ToString().Trim();
						if(identityAsString.Length>0)
							if(identityAsString[0]=='+')
							{
								plus=true;
								identityAsString=identityAsString.Substring(1);
								_Identity=new OOAdvantech.MetaDataRepository.MetaObjectID(identityAsString);
							}
					}
					if(_Identity==null)
					{
						/*AssociationEnd otherAssociationEnd=	GetOtherEnd() as AssociationEnd;
						
						string otherAssociationEndID=otherAssociationEnd.Name;
						if(otherAssociationEnd.WrMemberInfo!=null)
						{
							Attributes=otherAssociationEnd.WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID),false);
							if(Attributes.Length>0)
							{
								string identityAsString=Attributes[0].ToString();
								if(identityAsString.Length>0)
									if(identityAsString[0]=='+')
										otherAssociationEndID=identityAsString.Substring(1);
							}
						}*/
						_Identity=new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString()+"."+WrMemberInfo.Name);//+"."+Specification.Identity);//+"."+otherAssociationEndID);//Error prone 
						return _Identity;
					}
					else
					{
						if(plus)
						{
							/*AssociationEnd otherAssociationEnd=	GetOtherEnd() as AssociationEnd;
							string otherAssociationEndID=otherAssociationEnd.Name;
							if(otherAssociationEnd.WrMemberInfo!=null)
							{
								Attributes=otherAssociationEnd.WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID),false);
								if(Attributes.Length>0)
								{
									string identityAsString=Attributes[0].ToString();
									if(identityAsString.Length>0)
										if(identityAsString[0]=='+')
											otherAssociationEndID=identityAsString.Substring(1);
								}
							}*/
							_Identity=new MetaDataRepository.MetaObjectID(Namespace.Identity+"."+_Identity.ToString());//+"."+Specification.Identity);//+"."+otherAssociationEndID);//Error prone 

						}
					}
					return _Identity;
				}
				catch(System.Exception error)
				{
					int tt=0;
					throw error;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
					if(oldIdentity==null)
					{
						if(WrMemberInfo!=null)
							MetaObjectMapper.AddMetaObject(this,WrMemberInfo.DeclaringType.FullName+"."+WrMemberInfo.Name);
						else
							MetaObjectMapper.AddMetaObject(this,FullName);
					}
				}
			}
		}

		/// <MetaDataID>{D7CC5986-0F7D-4A61-B54A-740EF2A45E8A}</MetaDataID>
		internal System.Reflection.MemberInfo WrMemberInfo;

		/// <MetaDataID>{2D783A88-48AA-4697-83B7-7228E1D742FF}</MetaDataID>
		private void SetDynamicProperties()
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);

			try
			{
				if(WrMemberInfo==null)
					return;
				object[] CustomAttributes=WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationEndBehavior),false);
				if(CustomAttributes.Length==0)
					_HasBehavioralSettings=false;
				foreach(MetaDataRepository.AssociationEndBehavior associationEndBehavior in CustomAttributes)
				{
					_HasBehavioralSettings=true;
					
					if((uint)(associationEndBehavior.PersistencyFlag&MetaDataRepository.PersistencyFlag.ReferentialIntegrity)!=0)
						_ReferentialIntegrity=true;
					else
						_ReferentialIntegrity=false;
					
					if((uint)(associationEndBehavior.PersistencyFlag&MetaDataRepository.PersistencyFlag.CascadeDelete)!=0)
						_CascadeDelete=true;
					else
						_CascadeDelete=false;

					uint temp=(uint)associationEndBehavior.PersistencyFlag;
					if((uint)(associationEndBehavior.PersistencyFlag&MetaDataRepository.PersistencyFlag.OnConstruction)!=0)
						_LazyFetching=false;
					else
						_LazyFetching=true;
					break;
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);

			}
		}
		/// <MetaDataID>{EF4EA378-3E69-401D-B832-202C13272899}</MetaDataID>
		public AssociationEnd(System.Reflection.PropertyInfo thePropertyInfo)
		{
		
			System.Reflection.MethodInfo accessor= thePropertyInfo.GetAccessors(true)[0] as System.Reflection.MethodInfo;
			
			if(accessor.GetBaseDefinition()!=accessor)
				throw new System.Exception("You can't declare Association in override property "+thePropertyInfo.DeclaringType+"."+thePropertyInfo.Name); 
			
			WrMemberInfo=thePropertyInfo;
			if( WrMemberInfo!=null)
			{
				DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(WrMemberInfo,this);
				Navigable=true;
				SetDynamicProperties();
				Name=WrMemberInfo.Name;
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
					_Namespace=DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(WrMemberInfo.DeclaringType) as MetaDataRepository.Namespace;
					if(_Namespace!=null)
						return _Namespace;
					if(WrMemberInfo.DeclaringType.IsInterface)
					{
						Interface _interface=new Interface(new DotNetMetaDataRepository.Type(WrMemberInfo.DeclaringType));
						_Namespace=_interface;

					}
					if(WrMemberInfo.DeclaringType.IsClass)
					{
						Class _class=new Class(new DotNetMetaDataRepository.Type(WrMemberInfo.DeclaringType));
						_Namespace=_class;
					}
					if(WrMemberInfo.DeclaringType.IsPrimitive)
					{
						Primitive _primitive=new Primitive(new DotNetMetaDataRepository.Type(WrMemberInfo.DeclaringType));
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


		/// <MetaDataID>{4274543A-127E-407D-B2F0-44CE7901FEBD}</MetaDataID>
		public System.Reflection.PropertyInfo PropertyMember
		{
			get
			{
				if( WrMemberInfo==null)
					return null;
				if(WrMemberInfo is System.Reflection.PropertyInfo)
					return WrMemberInfo as System.Reflection.PropertyInfo;
				else
					return null;
			}
		}
		/// <MetaDataID>{BDACB24D-107F-4E04-904D-CA580E93C42D}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			ReaderWriterLock.AcquireReaderLock(10000);
			try
			{
				if(ExtensionMetaObjects==null)
				{
					ExtensionMetaObjects=new System.Collections.ArrayList();

					if(WrMemberInfo!=null)
						ExtensionMetaObjects.Add(WrMemberInfo);
				}
				return ExtensionMetaObjects;
			}
			finally
			{
				ReaderWriterLock.ReleaseReaderLock(); 
			}
		}
		/// <MetaDataID>{83A3774B-CBAB-4148-9B56-6584A13D41A3}</MetaDataID>
		public AssociationEnd(System.Reflection.FieldInfo theFieldInfo)
		{
			WrMemberInfo=theFieldInfo;
			 if( WrMemberInfo!=null)
			 {
				 Navigable=true;
				 SetDynamicProperties();
				 Name=WrMemberInfo.Name;
				 DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(WrMemberInfo,this);
				 
			 }
			
		}
		/// <MetaDataID>{AD7FF79A-B239-4C53-B765-58073ED69068}</MetaDataID>
		public void SetAssociation(MetaDataRepository.Association theAssociation)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{


				try
				{
					if(WrMemberInfo!=null) // Error prone σε περίπτωση που σε μια association με navigate και από
						// τις δύο πλευρέw όταν από την μια δεν δηλωθεί custom attribute presistent
					{
						_Persistent=false;
						object[] Attributes=WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.PersistentMember),false);
						if(Attributes.Length>0)
							_Persistent=true;

						Attributes=WrMemberInfo.GetCustomAttributes(typeof(MetaDataRepository.Persistent),false);
						if(Attributes.Length>0)
							_Persistent=true;
					}
				}
				catch(System.Exception Error)
				{
					int lo=0;
				}
				_Association=theAssociation;
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
		/// <MetaDataID>{F2C15911-7305-44D6-AF33-6D547135DDBC}</MetaDataID>
		public void SetSpecification(MetaDataRepository.Classifier theSpecificationClassifier)
		{
			/*if(typeof(Interface).IsInstanceOfType(theSpecificationClassifier))
			{
				Interface mInterface=(Interface)theSpecificationClassifier;
				mInterface.AddAssociationEnd(this);
			}

			if(typeof(Class).IsInstanceOfType(theSpecificationClassifier))
			{
				Class mClass=(Class)theSpecificationClassifier;
				mClass.AddAssociationEnd(this);
			}*/
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{


				_Specification=theSpecificationClassifier;
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);

			}
			
		}
		/// <MetaDataID>{5078328E-B350-4FB9-9D39-0516CB4A7C8E}</MetaDataID>
		public void SetNamespace(MetaDataRepository.Namespace theNamespace)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				_Namespace=theNamespace;
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}


	}
}
