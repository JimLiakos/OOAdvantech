namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{6C4F40AA-77CD-4B81-AFAF-E83AAB3F9269}</MetaDataID>
	public class Class : MetaDataRepository.Class
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
				System.Reflection.ConstructorInfo[]  constructors=Refer.WrType.GetConstructors();
				if(constructors.Length>0)
				{
					bool defaultConstructorExist=false;
					foreach(System.Reflection.ConstructorInfo constructor in constructors)
					{
						if(constructor.GetParameters().Length==0)
						{
							defaultConstructorExist=true;
							break;
						}
					}
					if(!defaultConstructorExist&&_Persistent)
					{
						errors+="\nIn persistent class '"+FullName+"' we must declare default constructor.\n";
						hasError= true;
					}
					if(LinkAssociation==null)
					{
						object[] Attributes =Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass),false);
						if(Attributes.Length>0)
						{
							MetaDataRepository.AssociationClass associationClass=(MetaDataRepository.AssociationClass)Attributes[0];
							errors+="\nSystem can't find the Assosciation "+associationClass.AssocciationName+" for Class '"+FullName+"'";
							hasError= true;
						}
					}
					else
					{

						object[] Attributes =Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass),false);
						if(Attributes.Length>0)
						{
							MetaDataRepository.AssociationClass associationClass=(MetaDataRepository.AssociationClass)Attributes[0];
							if(associationClass.AssocciationName!=LinkAssociation.Name)
							{
								errors+="\nSystem can't find the Assosciation "+associationClass.AssocciationName+" for Class '"+FullName+"'";
								hasError= true;
							}
						}

						GetLinkClassRoleFields();
						if(_LinkClassRoleAField==null)
						{
							errors+="\nThere isn't field definition for role A in Association class '"+FullName+"'.";
							hasError=true;
						}

						if(_LinkClassRoleBField==null)
						{
							errors+="\nThere isn't field definition for role B in Association class '"+FullName+"'.";
							hasError=true;
						}
						if(_LinkClassRoleBField==null||_LinkClassRoleAField==null)
							return hasError;



						System.Type RoleAType=LinkAssociation.RoleA.Specification.GetExtensionMetaObject(typeof(System.Type))as System.Type;
						System.Type RoleBType=LinkAssociation.RoleB.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
						
						if(RoleAType!=_LinkClassRoleAField.FieldType)
						{
							errors+="\nRoleA type mismatch at "+_LinkClassRoleAField.DeclaringType+"."+_LinkClassRoleAField.Name+".";
							hasError=true;
						}

						if(RoleBType!=_LinkClassRoleBField.FieldType)
						{
							errors+="\nRoleB type mismatch at "+_LinkClassRoleBField.DeclaringType+"."+_LinkClassRoleBField.Name+".";
							hasError=true;
						}
						
						if(hasError)
						{
							_LinkClassRoleAField=null;
							_LinkClassRoleBField=null;
						}
					}
					MetaDataRepository.MetaObjectCollection supperClasifiers=new OOAdvantech.MetaDataRepository.MetaObjectCollection();
					if(ClassHierarchyLinkAssociation!=null)
					{
						foreach(Interface _interface in GetAllInterfaces())
						{
							supperClasifiers.Add(_interface);
							supperClasifiers.AddCollection(_interface.GetAllGeneralClasifiers());
						}
						supperClasifiers.AddCollection(GetAllGeneralClasifiers());
					}
					foreach(MetaDataRepository.Classifier classifier in supperClasifiers)
					{
						if(classifier.LinkAssociation!=null&&classifier.LinkAssociation!=ClassHierarchyLinkAssociation)
						{
							hasError=true;
							errors+="\nThere are two association types in class hierarchy of '"+FullName+
							"'. The '"+  classifier.FullName+ "' and the '"+ClassHierarchyLinkAssociation.LinkClass.FullName+"'.";
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

		
//		/// <MetaDataID>{27F51EA4-DC71-42A7-96E7-DEABD0E4F9CD}</MetaDataID>
//		public override bool Persistent
//		{
//			get
//			{
//				return base.Persistent;
//			}
//			set
//			{
//				base.Persistent=value;
//			}
//		}

		/// <MetaDataID>{3E880A4C-9A94-4E3E-91A1-9C2C932707E2}</MetaDataID>
		public override MetaDataRepository.Association  LinkAssociation
		{

			get
			{
				return base.LinkAssociation;
			}
			set
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					_LinkAssociation=value;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}

		
		/// <MetaDataID>{775C7800-AB64-4556-9FAE-C44E4C225A14}</MetaDataID>
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
					object[] Attributes=Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID),false);
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
						if(Namespace==null)
							_Identity=new MetaDataRepository.MetaObjectID(Refer.WrType.FullName);
						else
						{
							if(Namespace.Identity.ToString().Length==0)
								_Identity=new MetaDataRepository.MetaObjectID(Refer.WrType.FullName);
							else
								_Identity=new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString()+"."+Refer.WrType.Name);
						}
						return _Identity;
					}
					else
						if(Namespace.Identity.ToString().Length>0&&plus)
							_Identity=new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString()+"."+_Identity.ToString());

					return _Identity;
				}
				finally
				{

					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
					if(oldIdentity==null)
                        MetaObjectMapper.AddMetaObject(this,Refer.WrType.FullName);
				}
			}
		}
		/// <MetaDataID>{083A7936-365D-4B76-8BED-3E4F76684EB0}</MetaDataID>
		private System.Collections.ArrayList ExtensionMetaObjects;

		/// <MetaDataID>{650EACF7-095B-4B22-9A55-CF0A89E9E0FA}</MetaDataID>
		public void AddExtensionMetaObject(object Value)
		{
			if(ExtensionMetaObjects==null)
				GetExtensionMetaObjects();
			ExtensionMetaObjects.Add(Value);
		}
		/// <MetaDataID>{ED3ECEEC-DFED-4BF6-A101-00A83B2F341F}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			if(ExtensionMetaObjects==null)
			{
				ExtensionMetaObjects=new System.Collections.ArrayList();
				ExtensionMetaObjects.Add(Refer.WrType);
			}
			return new System.Collections.ArrayList(ExtensionMetaObjects);
		}
		/// <MetaDataID>{68C58AEC-455C-4F6A-880F-DD254E2D0A89}</MetaDataID>
		private void LoadRealizations()
		{

			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(!RealizationsLoaded)
				{
					RealizationsLoaded=true;
					_Realizations=new MetaObjectCollection();
					MetaDataRepository.MetaObjectCollection Generalizations=new MetaObjectCollection();
					System.Collections.ArrayList SuperTypes=new System.Collections.ArrayList();
					System.Type[] AllSuperTypes= Refer.WrType.GetInterfaces();
					SuperTypes.AddRange(AllSuperTypes);
					foreach(System.Type mType in AllSuperTypes)
					{
						foreach(System.Type IndirectSuperType in 	mType.GetInterfaces())
						{
							if(SuperTypes.Contains(IndirectSuperType))
								SuperTypes.Remove(IndirectSuperType);
						}
					}
					foreach(System.Type mType in SuperTypes)
					{
						Interface mInterface=(Interface)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mType); 
						if(mInterface==null)
							mInterface=new Interface(new Type(mType));
						MetaDataRepository.Realization mRealization=new MetaDataRepository.Realization("",mInterface,this);
						_Realizations.Add(mRealization);
						
					}
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}


		}
		/// <MetaDataID>{1A8D9359-77AF-4262-883F-F7CB0CD83D1D}</MetaDataID>
		public override  MetaDataRepository.MetaObjectCollection  Realizations
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					if(_Realizations==null)
						LoadRealizations();
					MetaDataRepository.MetaObjectCollection ReturnRealizations= new MetaObjectCollection();
					ReturnRealizations.AddCollection(_Realizations);
					return ReturnRealizations;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
		
	
		/// <MetaDataID>{16E4B3CF-8434-49FD-9F07-334F4BE796F9}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection Features
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					//if(_Features==null)
					LoadFeatures();
					MetaDataRepository.MetaObjectCollection ReturnFeatures=new MetaDataRepository.MetaObjectCollection(_Features,MetaDataRepository.MetaObjectCollection.AccessType.ReadOnly);
					return ReturnFeatures;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
		/// <MetaDataID>{790C8492-9DEF-4AD8-9FD1-1476FB8E515D}</MetaDataID>
		private System.Reflection.FieldInfo _LinkClassRoleAField;
		/// <MetaDataID>{C9138955-14BB-4EB5-B1BD-7F031C48757A}</MetaDataID>
		public System.Reflection.FieldInfo LinkClassRoleAField
		{
			get
			{
				if(_LinkClassRoleAField==null)
				{
					GetLinkClassRoleFields();
					RolesFieldsTypeCheck();
				}

				
				return _LinkClassRoleAField;
			}
		}

		/// <MetaDataID>{0FEC612B-4CC2-4F40-8E27-69766F614C4F}</MetaDataID>
		private System.Reflection.FieldInfo _LinkClassRoleBField;
		/// <MetaDataID>{7110ED9B-6FCE-40F2-AC83-CDDFBA7D7F1A}</MetaDataID>
		public System.Reflection.FieldInfo LinkClassRoleBField
		{
			get
			{
				if(_LinkClassRoleBField==null)
				{
					GetLinkClassRoleFields();
					RolesFieldsTypeCheck();
				}
				return _LinkClassRoleBField;
			}
		}
		/// <MetaDataID>{A18FB265-F60B-4A3B-8808-A8ECDA65EFB4}</MetaDataID>
		void GetLinkClassRoleFields()
		{
			if(ClassHierarchyLinkAssociation==null)
				throw new System.Exception("The class "+FullName+" it isn't link class.");
			
			foreach (MetaDataRepository.Feature feature in Features)
			{
				System.Reflection.MemberInfo memberInfo=null;
				if(feature is Attribute&&(feature as Attribute).wrMember.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole),false).Length>0)
					memberInfo=(feature as Attribute).wrMember;

				if(feature is AttributeRealization&&(feature as AttributeRealization).PropertyMember.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole),false).Length>0)
					memberInfo=(feature as AttributeRealization).PropertyMember;

				if(memberInfo!=null)
				{
					MetaDataRepository.AssociationClassRole AssociationClassRole=memberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole),true)[0] as MetaDataRepository.AssociationClassRole;
					System.Reflection.FieldInfo  FieldRole=null;
					if(memberInfo is System.Reflection.FieldInfo)
						FieldRole=memberInfo as System.Reflection.FieldInfo;
					else
						FieldRole=memberInfo.DeclaringType.GetField(AssociationClassRole.ImplMemberName,System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance); // Error Prone if FieldRoleA==null;

					if(AssociationClassRole.IsRoleA)
						_LinkClassRoleAField=FieldRole;
					else
						_LinkClassRoleBField=FieldRole;
				}
			}
			if(_LinkClassRoleAField==null)
			{
				foreach(Class _class in GetGeneralClasifiers())
				{
					_LinkClassRoleAField=_class.LinkClassRoleAField;
					if(_LinkClassRoleAField!=null)
						break;
				}
			}
			if(_LinkClassRoleBField==null)
			{
				foreach(Class _class in GetGeneralClasifiers())
				{
					_LinkClassRoleBField=_class.LinkClassRoleBField;
					if(_LinkClassRoleAField!=null)
						break;
				}
			}


			
		}
		/// <MetaDataID>{CE5A6C85-2FD9-406E-9A1C-2CE432944C1F}</MetaDataID>
		void RolesFieldsTypeCheck()
		{
			if(_LinkClassRoleAField==null)
				throw new System.Exception("There isn't field definition for role A in Association class '"+FullName+"'.");

			if(_LinkClassRoleBField==null)
				throw new System.Exception("There isn't field definition for role B in Association class '"+FullName+"'.");

			System.Type RoleAType=ClassHierarchyLinkAssociation.RoleA.Specification.GetExtensionMetaObject(typeof(System.Type))as System.Type;
			System.Type RoleBType=ClassHierarchyLinkAssociation.RoleB.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
			string ErrorMessage=null;
			if(RoleAType!=_LinkClassRoleAField.FieldType)
				ErrorMessage+="RoleA type mismatch at "+_LinkClassRoleAField.DeclaringType+"."+_LinkClassRoleAField.Name+".\n";

			if(RoleBType!=_LinkClassRoleBField.FieldType)
				ErrorMessage+="RoleB type mismatch at "+_LinkClassRoleBField.DeclaringType+"."+_LinkClassRoleBField.Name+".\n";
			if(ErrorMessage!=null)
			{
				_LinkClassRoleAField=null;
				_LinkClassRoleBField=null;
				throw new System.Exception(ErrorMessage);
			}
		}

		/// <MetaDataID>{94D044FD-07D1-4F00-BE0A-F0E0E508EAFD}</MetaDataID>
		public override MetaDataRepository.Namespace Namespace
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Namespace;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}

			}
		}


		/// <MetaDataID>{ACFBC2A9-0977-440E-82A8-E91098C385BE}</MetaDataID>
		public System.Reflection.FieldInfo GetFieldMember(AssociationEnd associationEnd)
		{
			foreach(MetaDataRepository.Feature feature in Features)
			{
				AssociationEndRealization associationEndRealization=feature as AssociationEndRealization;
				if(associationEndRealization!=null)
				{
					if(associationEndRealization.Specification==associationEnd&&associationEndRealization.FieldMember!=null)
						return associationEndRealization.FieldMember;
				}
			}
			foreach(Class _class in GetGeneralClasifiers())
			{
				System.Reflection.FieldInfo fieldInfo=_class.GetFieldMember(associationEnd);
				if(fieldInfo!=null)
					return fieldInfo;
			}
			//TODO: ο παρακάτω κώδικας αποτελή μέρος του κώδικα ελέχγου όρθοτητας του persistent metamodel και πρέπει
			//να μεταφερθεί εκεί όστε να μην είναι δυνατόν να κάνω register το assembly σε ένα storage.
			if(associationEnd.Persistent==true&&Persistent&&associationEnd.FieldMember==null)
				throw new System.Exception("Class:["+Name+"] You must declare PersistentMember Attribute for ["+associationEnd.PropertyMember.DeclaringType.FullName+"."+associationEnd.PropertyMember.Name+"] realization."); 

			return associationEnd.FieldMember;
		}

		/// <MetaDataID>{FF904D5B-BCA4-44A9-8970-C080FFBD916B}</MetaDataID>
		public void SetNamespace(MetaDataRepository.Namespace OwnerNamespace)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{

				_Namespace=OwnerNamespace;
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
		/// <MetaDataID>{F479B966-7290-49D7-8C1F-655E9D48F2AB}</MetaDataID>
		private MetaDataRepository.MetaObjectCollection _NestedClasses;
		
		/// <summary>This method retrieves all classes nested within the specified class and all of its nested classes.
		/// For example:  If Class A has 2 nested classes, NClass1 and NClass2, and NClass1 has a nested class, NestedCls, applying the GetAllNestedClasses method to Class A returns all 3 nested classes, NClass1, NClass2, and NestedCls, not just the first-level nested classes.
		/// To retrieve only the first-level nested classes for the specified class, use GetNestedClasses. </summary>
		/// <MetaDataID>{577CC8EC-A25B-4511-A484-DD957A30084D}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection GetAllNestedClasses()
		{

			ReaderWriterLock.AcquireReaderLock(10000);
			try
			{
				MetaDataRepository.MetaObjectCollection AllNestedClasses=new MetaObjectCollection();
				foreach(Class CurrClass in NestedClasses)
				{
					AllNestedClasses.Add(CurrClass);
					AllNestedClasses.AddCollection(CurrClass.GetAllNestedClasses());
				}
				return AllNestedClasses;
			}
			finally
			{
				ReaderWriterLock.ReleaseReaderLock();
			}
		}
		/// <summary>This method retrieves the first-level nested class collection from the class and returns it in the specified object. To retrieve all nested classes of the specified class and all of its nested classes, use GetAllNestedClasses. </summary>
		/// <MetaDataID>{B69AB082-3F05-4B10-B8FB-185D33A9DD00}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection NestedClasses
		{
			get
			{
				System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{

					if(_NestedClasses==null)
					{
						_NestedClasses=Refer.GetNestedClassifier();
						foreach(Class CurrClass in _NestedClasses)
							CurrClass.SetNamespace(this);
						_OwnedElements.AddCollection(_NestedClasses);
					}
					MetaDataRepository.MetaObjectCollection ReturnCollection=new MetaObjectCollection();
					ReturnCollection.AddCollection(_NestedClasses);
					return ReturnCollection;
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}
		/// <MetaDataID>{3FF25ACA-E237-4475-96ED-F051DAE0015F}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection OwnedElements
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					if(_OwnedElements==null)
						_OwnedElements=new MetaDataRepository.MetaObjectCollection();
					return base.OwnedElements;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			
			}
		}
		/// <MetaDataID>{5C38CD06-D5B1-4C15-B6AE-A07449846606}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection Roles
		{
			get
			{
				
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					LoadAssociatonEnds();
					MetaDataRepository.MetaObjectCollection roles=new MetaDataRepository.MetaObjectCollection();
					roles.AddCollection(_Roles);
					return roles;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
		/// <MetaDataID>{8E9A2ED0-BCB5-49D3-8204-5206388EC8FB}</MetaDataID>
		private void LoadGeneralizations()
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(!GeneralizationsLoaded)
				{
					GeneralizationsLoaded=true;
					_Generalizations=Refer.Generalizations;
					foreach(MetaDataRepository.Generalization Generalization in _Generalizations)
						Generalization.Child=this;
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
		/// <MetaDataID>{E8BE80F7-9740-49DF-8456-FBF2BB5012A5}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection Generalizations
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					if(_Generalizations==null)
						LoadGeneralizations();
					MetaDataRepository.MetaObjectCollection generalizations=new MetaDataRepository.MetaObjectCollection(_Generalizations,MetaDataRepository.MetaObjectCollection.AccessType.ReadOnly);
					//generalizations.AddCollection(_Generalizations);
					return generalizations;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
		/// <MetaDataID>{AD470D27-5893-4D2D-A60F-4DA9E6D548EC}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection Specializations
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					if(_Specializations==null)
						_Specializations=new MetaObjectCollection();

					MetaDataRepository.MetaObjectCollection specializations=new MetaDataRepository.MetaObjectCollection();
					specializations.AddCollection(_Specializations);
					return specializations;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
		}
		/// <MetaDataID>{A22BEB50-9C15-49B5-8EB0-0C957FFBF28D}</MetaDataID>
		internal Type Refer;

		/// <MetaDataID>{66DA50F2-0634-4982-93C4-8D4DF43B090C}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private bool RolesLoaded=false;
		/// <MetaDataID>{4A805E48-D77C-4AC1-B105-3E9CB8FB7B55}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private bool RealizationsLoaded=false;
		/// <MetaDataID>{4F59E6BD-A5A0-4620-9AFC-37D08E3F41F8}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private bool GeneralizationsLoaded=false;
		/// <MetaDataID>{151623F6-6B5F-4649-80D5-77924F7D3FF7}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private bool FeaturesLoaded=false;
		/// <MetaDataID>{AEE7B033-A7AE-4DCA-8600-89CAFE62E4B6}</MetaDataID>
		private void LoadAssociatonEnds()
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(!RolesLoaded)
				{
					RolesLoaded=true;
					
					Refer.GetRoles(this);
//					MetaDataRepository.MetaObjectCollection AssociationRoles=Refer.GetRoles(this);//.Roles;
//				
//					foreach(AssociationEnd  OtherAssociationEnd in AssociationRoles)
//					{
//						OtherAssociationEnd.SetNamespace(this);
//						if(OtherAssociationEnd.Association==null)
//						{
//							((AssociationEnd)OtherAssociationEnd).SetAssociation(new Association());
//							//(OtherAssociationEnd.Association as Association).Persistent=false;
//							((Association)OtherAssociationEnd.Association).SetRoleAAssociationEnd(OtherAssociationEnd);
//							AssociationEnd MyAssociationEnd=new AssociationEnd(NullFieldInfo);
//							MyAssociationEnd.SetSpecification(this);
//							MyAssociationEnd.SetAssociation(OtherAssociationEnd.Association);
//							((Association)MyAssociationEnd.Association).SetRoleBAssociationEnd(MyAssociationEnd);
//						}
//						else
//						{
//							AssociationEnd MyAssociationEnd=null;
//							if(OtherAssociationEnd.IsRoleA)
//								MyAssociationEnd=(AssociationEnd)OtherAssociationEnd.Association.RoleB;
//							else
//								MyAssociationEnd=(AssociationEnd)OtherAssociationEnd.Association.RoleA;
//							if(MyAssociationEnd==null)
//							{
//								MyAssociationEnd=new AssociationEnd(NullFieldInfo);
//								MyAssociationEnd.SetSpecification(this);
//								MyAssociationEnd.SetAssociation(OtherAssociationEnd.Association);
//								if(OtherAssociationEnd.IsRoleA)
//									((Association)MyAssociationEnd.Association).SetRoleBAssociationEnd(MyAssociationEnd);
//								else
//									((Association)MyAssociationEnd.Association).SetRoleAAssociationEnd(MyAssociationEnd);
//							}
//						}
//						_Roles.Add(OtherAssociationEnd.GetOtherEnd());
//						if(OtherAssociationEnd.GetOtherEnd()!=null)
//							if(OtherAssociationEnd.GetOtherEnd().GetOtherEnd()==OtherAssociationEnd)
//							{
//								int lop=0;
//								lop++;
//							}
//						if(!OtherAssociationEnd.GetOtherEnd().Navigable)
//						{
//							if(typeof(Interface).IsInstanceOfType(OtherAssociationEnd.Specification))
//							{
//								Interface mInterface=(Interface)OtherAssociationEnd.Specification;
//								mInterface.AddAssociationEnd((AssociationEnd)OtherAssociationEnd);
//							}
//							if(typeof(Class).IsInstanceOfType(OtherAssociationEnd.Specification))
//							{
//								Class mClass=(Class)OtherAssociationEnd.Specification;
//								mClass.AddAssociationEnd((AssociationEnd)OtherAssociationEnd);
//							}
//						}
//					}
//					object[] Attributes =Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass),false);
//					if(Attributes.Length>0)
//					{
//						MetaDataRepository.AssociationClass mAssociationClass=(MetaDataRepository.AssociationClass)Attributes[0];
//						MetaDataRepository.Classifier RoleAClass=(MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mAssociationClass.AssocciationEndRoleA);
//						if(RoleAClass==null)
//						{
//							if(mAssociationClass.AssocciationEndRoleA.IsClass)
//								RoleAClass=new Class(new Type(mAssociationClass.AssocciationEndRoleA));
//							if(mAssociationClass.AssocciationEndRoleA.IsInterface)
//								RoleAClass=new Interface(new Type(mAssociationClass.AssocciationEndRoleA));
//						}
//						foreach(AssociationEnd CurrAssociationEnd in RoleAClass.Roles)
//						{
//							if(CurrAssociationEnd.Association.Name==mAssociationClass.AssocciationName)
//							{
//								_LinkAssociation=CurrAssociationEnd.Association;
//								break;
//							}
//						}
//						if(_LinkAssociation==null)
//						{
//							MetaDataRepository.Classifier RoleBClass=(MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mAssociationClass.AssocciationEndRoleB);
//							if(RoleBClass==null)
//							{
//								if(mAssociationClass.AssocciationEndRoleB.IsClass)
//									RoleBClass=new Class(new Type(mAssociationClass.AssocciationEndRoleB));
//								if(mAssociationClass.AssocciationEndRoleA.IsInterface)
//									RoleBClass=new Interface(new Type(mAssociationClass.AssocciationEndRoleB));
//							}
//							foreach(AssociationEnd CurrAssociationEnd in RoleBClass.Roles)
//							{
//								if(CurrAssociationEnd.Association.Name==mAssociationClass.AssocciationName)
//								{
//									_LinkAssociation=CurrAssociationEnd.Association;
//									break;
//								}
//							}
//						}
//					}

				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}
		void LoadAttributeRealization()
		{
			LoadGeneralizations();
			LoadRealizations();
			System.Collections.Hashtable propertyAttributes=new System.Collections.Hashtable();
			foreach(Attribute classAttribute in	GetAttributes(false))
			{
				if (classAttribute.wrMember is System.Reflection.PropertyInfo)
					propertyAttributes.Add(classAttribute.wrMember,classAttribute);

			}

			System.Reflection.BindingFlags bindingFlags  =System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.DeclaredOnly;
			foreach(Interface _interface in GetAllInterfaces())
			{
				foreach(Attribute attribute in _interface.GetAttributes(false))
				{
					System.Reflection.MethodBase accessor=attribute .PropertyMember.GetAccessors(true)[0];
					System.Reflection.InterfaceMapping interfaceMapping=Refer.WrType.GetInterfaceMap(_interface.Refer.WrType);
					System.Reflection.MethodBase accessorImplementation=null;
					for(int i=0;i<interfaceMapping.InterfaceMethods.Length;i++) 
					{
						if(interfaceMapping.InterfaceMethods[i]==accessor)
						{
							accessorImplementation=interfaceMapping.TargetMethods[i];
							break;
						}
					}
					if(accessorImplementation!=null)
					{
						foreach(System.Reflection.PropertyInfo propertyInfo in Refer.WrType.GetProperties(bindingFlags)) 
						{
							System.Reflection.MethodInfo getMethod=propertyInfo.GetGetMethod(true);
							System.Reflection.MethodInfo setMethod=propertyInfo.GetSetMethod(true);
							//if((getMethod!=null&&getMethod.GetBaseDefinition()==accessor)||setMethod!=null&&setMethod.GetBaseDefinition()==accessor)
							if(propertyInfo.GetGetMethod(true)==accessorImplementation||propertyInfo.GetSetMethod(true)==accessorImplementation)
							{
								if(propertyAttributes.Contains(propertyInfo))
									_Features.Remove(propertyAttributes[propertyInfo] as Attribute);
									
								_Features.Add(new AttributeRealization(propertyInfo, attribute,this));
								break;
							}
						}
					}
				}
			}
			foreach(Class  _class in GetAllGeneralClasifiers())
			{
				foreach(Attribute attribute in _class.GetAttributes(false))
				{
					if(attribute.PropertyMember!=null)
					{
						System.Reflection.MethodBase accessor=attribute.PropertyMember.GetAccessors(true)[0];
						foreach(System.Reflection.PropertyInfo propertyInfo in Refer.WrType.GetProperties(bindingFlags)) 
						{
							System.Reflection.MethodInfo getMethod=propertyInfo.GetGetMethod(true);
							System.Reflection.MethodInfo setMethod=propertyInfo.GetSetMethod(true);
							if((getMethod!=null&&getMethod.GetBaseDefinition()==accessor)||setMethod!=null&&setMethod.GetBaseDefinition()==accessor)
							{
								if(propertyAttributes.Contains(propertyInfo))
									_Features.Remove(propertyAttributes[propertyInfo] as Attribute);
 								_Features.Add(new AttributeRealization(propertyInfo, attribute,this));
								break;
							}
						}
					}
				}
			}
			

		}
		/// <MetaDataID>{3F452752-4421-4DF9-B72F-A9AD52FFE00D}</MetaDataID>
		void LoadAssociationEndRealization()
		{
			LoadGeneralizations();
			LoadRealizations();
			System.Reflection.BindingFlags bindingFlags  =System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.DeclaredOnly;
			System.Collections.Hashtable propertyAttributes=new System.Collections.Hashtable();
			foreach(Attribute classAttribute in	GetAttributes(false))
			{
				if (classAttribute.wrMember is System.Reflection.PropertyInfo)
					propertyAttributes.Add(classAttribute.wrMember,classAttribute);

			}

			foreach(Interface _interface in GetAllInterfaces())
			{
				foreach(AssociationEnd associationEnd in _interface.GetAssociateRoles(false))
				{
					System.Reflection.MethodBase accessor=associationEnd.PropertyMember.GetAccessors(true)[0];
					System.Reflection.InterfaceMapping interfaceMapping=Refer.WrType.GetInterfaceMap(_interface.Refer.WrType);
					System.Reflection.MethodBase accessorImplementation=null;
					for(int i=0;i<interfaceMapping.InterfaceMethods.Length;i++) 
					{
						if(interfaceMapping.InterfaceMethods[i]==accessor)
						{
							//TODO Στο .net μπορεί να υπάρχει property με το ιδιο όνομα σε ένα ή περισσότερα interface 
							//στην ιεραρχία άρα  η property της class μπορεί υλοποιεί πάνω από ένα interafce property. 
							//Aυτή την κατάσταση την αγνοεί το DotnetMetaDataRepository

							accessorImplementation=interfaceMapping.TargetMethods[i];
							break;
						}
					}
					if(accessorImplementation!=null)
					{
						foreach(System.Reflection.PropertyInfo propertyInfo in Refer.WrType.GetProperties(bindingFlags)) 
						{
							System.Reflection.MethodInfo getMethod=propertyInfo.GetGetMethod(true);
							System.Reflection.MethodInfo setMethod=propertyInfo.GetSetMethod(true);
							//if((getMethod!=null&&getMethod.GetBaseDefinition()==accessor)||setMethod!=null&&setMethod.GetBaseDefinition()==accessor)
							if(propertyInfo.GetGetMethod(true)==accessorImplementation||propertyInfo.GetSetMethod(true)==accessorImplementation)
							{
								if(propertyAttributes.Contains(propertyInfo))
									_Features.Remove(propertyAttributes[propertyInfo] as Attribute);

								_Features.Add(new AssociationEndRealization(propertyInfo, associationEnd,this));
								break;
							}
						}
					}
				}
			}
			foreach(Class  _class in GetAllGeneralClasifiers())
			{
				foreach(AssociationEnd associationEnd in _class.GetAssociateRoles(false))
				{
					if(associationEnd.PropertyMember!=null)
					{
						System.Reflection.MethodBase accessor=associationEnd.PropertyMember.GetAccessors(true)[0];
						foreach(System.Reflection.PropertyInfo propertyInfo in Refer.WrType.GetProperties(bindingFlags)) 
						{
							System.Reflection.MethodInfo getMethod=propertyInfo.GetGetMethod(true);
							System.Reflection.MethodInfo setMethod=propertyInfo.GetSetMethod(true);
							if((getMethod!=null&&getMethod.GetBaseDefinition()==accessor)||setMethod!=null&&setMethod.GetBaseDefinition()==accessor)
							{
								if(propertyAttributes.Contains(propertyInfo))
									_Features.Remove(propertyAttributes[propertyInfo] as Attribute);

								 AssociationEndRealization associationEndRealization =new AssociationEndRealization(propertyInfo, associationEnd,this);
								_Features.Add(associationEndRealization);

								break;
							}
						}
					}
				}
			}
		}
		/// <MetaDataID>{C304A0C0-D2A6-402A-8DB2-AFCB008EE76C}</MetaDataID>
		private void LoadFeatures()
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				if(!FeaturesLoaded)
				{
					FeaturesLoaded=true;
					_Features=Refer.Features;

					if(_OwnedElements==null)
					{
						_OwnedElements=new MetaObjectCollection();
						_OwnedElements.AddCollection(_Features);
					}
					LoadAssociationEndRealization();
					LoadAttributeRealization();

				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}


		}


		/// <MetaDataID>{C9001141-8F83-421D-AEB7-D99093B9A931}</MetaDataID>
		public void AddAssociationEnd(AssociationEnd theAssociationEnd)
		{
			System.Threading.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				_Roles.Add(theAssociationEnd);
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}

		/// <MetaDataID>{B240F117-197E-4B03-B433-DF8DD954266E}</MetaDataID>
		 public Class(Type theType)
		{
			 if(!theType.WrType.IsClass)
				 throw new System.Exception("the type '"+theType.WrType.FullName+"' isn't class");
			RolesLoaded=false;
			_Roles=new MetaObjectCollection();
			_Persistent=false;
			_Specializations=new MetaObjectCollection();
			_ClientDependencies=new MetaObjectCollection();
			_SupplierDependencies=new MetaObjectCollection();
			
			Name=theType.Name;
		
			Refer=theType;
			Visibility=Refer.Visibility;
			DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(theType.WrType,this);
			LoadGeneralizations();

			object[] ObjectCustomAttributes=Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.Persistent),true);
			foreach (System.Attribute CurrAttribute in ObjectCustomAttributes)
			{
				if(typeof(MetaDataRepository.Persistent)==CurrAttribute.GetType())
				{
					MetaDataRepository.Persistent mPersistent=(MetaDataRepository.Persistent)CurrAttribute;
					if(mPersistent.ExtMetaData!=null)
					{
						System.Xml.XmlDocument ExtMetaData = new System.Xml.XmlDocument();
						try
						{
							if(mPersistent.ExtMetaData!=null&&mPersistent.ExtMetaData.Length>0)
								ExtMetaData.InnerXml=mPersistent.ExtMetaData;
						}
						catch(System.Exception Error)
						{
							int tt=0;

						}
						if(ExtMetaData.ChildNodes.Count>0)
						{
							foreach(System.Xml.XmlNode CurrNode in ExtMetaData.ChildNodes[0].ChildNodes)
							{
								if(CurrNode.InnerText.Length>0)
									PutPropertyValue(ExtMetaData.ChildNodes[0].Name,CurrNode.Name,CurrNode.InnerText);
							}
						}

					}
					if(mPersistent.PersistencyType==MetaDataRepository.PersistencyType.historyClass)
					{
						PutPropertyValue("Persistence","HistoryClass",true);
						PutPropertyValue("Persistence","NumberOfObject",mPersistent.NumberOfObject);
					}
					else
						PutPropertyValue("Persistence","HistoryClass",false);

					_Persistent=true;
					break;
				}
			}
			

            LoadRealizations();
			//LoadAssociatonEnds();

			

			if(Refer.isNestedType)
				return;
			
			Namespace mNamespace=(Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(theType.WrType.Namespace);
			if(mNamespace==null)
				mNamespace=new Namespace(theType.WrType.Namespace);
			mNamespace.AddOwnedElement(this);
			SetNamespace(mNamespace);

			object[] Attributes =Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass),false);
			if(Attributes.Length>0)
			{
				MetaDataRepository.AssociationClass mAssociationClass=(MetaDataRepository.AssociationClass)Attributes[0];
				MetaDataRepository.Classifier RoleAClass=(MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mAssociationClass.AssocciationEndRoleA);

				if(RoleAClass==null)
				{
					if(mAssociationClass.AssocciationEndRoleA.IsClass)
						RoleAClass=new Class(new Type(mAssociationClass.AssocciationEndRoleA));
					if(mAssociationClass.AssocciationEndRoleA.IsInterface)
						RoleAClass=new Interface(new Type(mAssociationClass.AssocciationEndRoleA));
				}

				foreach(AssociationEnd CurrAssociationEnd in RoleAClass.Roles)
				{
					if(CurrAssociationEnd.Association.Name==mAssociationClass.AssocciationName)
					{
						_LinkAssociation=CurrAssociationEnd.Association;
						break;
					}
				}
				if(_LinkAssociation==null)
				{
					MetaDataRepository.Classifier RoleBClass=(MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mAssociationClass.AssocciationEndRoleB);
					if(RoleBClass==null)
					{
						if(mAssociationClass.AssocciationEndRoleB.IsClass)
							RoleBClass=new Class(new Type(mAssociationClass.AssocciationEndRoleB));
						if(mAssociationClass.AssocciationEndRoleA.IsInterface)
							RoleBClass=new Interface(new Type(mAssociationClass.AssocciationEndRoleB));
					}
					foreach(AssociationEnd CurrAssociationEnd in RoleBClass.Roles)
					{
						if(CurrAssociationEnd.Association.Name==mAssociationClass.AssocciationName)
						{
							_LinkAssociation=CurrAssociationEnd.Association;
							break;
						}
					}
				}
				//if(_LinkAssociation==null)
				//	throw new System.Exception("can't find the Assosciation "+mAssociationClass.AssocciationName);
			}

			
		}
	}
}
