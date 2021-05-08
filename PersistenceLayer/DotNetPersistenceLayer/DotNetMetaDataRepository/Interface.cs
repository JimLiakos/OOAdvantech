namespace OOAdvantech.DotNetMetaDataRepository
{
	/// <MetaDataID>{290365C4-9BAE-49E3-8653-AD4A5AB4ED9B}</MetaDataID>
	public class Interface : MetaDataRepository.Interface
	{
		/// <MetaDataID>{A8CCF55A-D5C2-4B88-9BB3-3111CB7286CA}</MetaDataID>
		public override MetaDataRepository.MetaObjectID Identity
		{
			get
			{
				MetaDataRepository.MetaObjectID oldIdentity=_Identity;
				if(_Identity!=null)
					return _Identity;
				object[] Attributes=Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.BackwardCompatibilityID),false);
				if(Attributes.Length>0)
					_Identity=new OOAdvantech.MetaDataRepository.MetaObjectID(Attributes[0].ToString());

				if(_Identity==null)
				{
					if(this.Namespace==null)
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
					_Identity=new MetaDataRepository.MetaObjectID(Namespace.Identity.ToString()+"."+_Identity.ToString());
				
				if(oldIdentity==null)
					MetaObjectMapper.AddMetaObject(this,Refer.WrType.FullName);

				return _Identity;			
			}
		}
		private System.Collections.ArrayList ExtensionMetaObjects;
		/// <MetaDataID>{98E3F6A4-DE17-40E7-9BFD-43CF0C8AA9A6}</MetaDataID>
		public override System.Collections.ArrayList GetExtensionMetaObjects()
		{
			if(ExtensionMetaObjects==null)
			{
				ExtensionMetaObjects=new System.Collections.ArrayList();
				ExtensionMetaObjects.Add(Refer.WrType);
			}
			return new System.Collections.ArrayList(ExtensionMetaObjects);
			
		}
		/// <MetaDataID>{33789F98-DA07-402B-9BFD-D360BE21ADE1}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection Features
		{
			get
			{
			//	if(_Features==null)
				LoadFeatures();
				MetaDataRepository.MetaObjectCollection ReturnFeatures=new MetaObjectCollection();
				ReturnFeatures.AddCollection(_Features);
				return ReturnFeatures;
			}
		}
		public void AddAssociationEnd(AssociationEnd theAssociationEnd)
		{
			_Roles.Add(theAssociationEnd);
		}

		private bool RolesLoaded;
		private void LoadAssociatonEnds()
		{
			if(!RolesLoaded)
			{
				
				System.Reflection.FieldInfo NullFieldInfo=null;
				Refer.GetRoles(this);
//				MetaDataRepository.MetaObjectCollection AssociationRoles=Refer.Roles;
//				
//				foreach(AssociationEnd  OtherAssociationEnd in AssociationRoles)
//				{
//					OtherAssociationEnd.SetNamespace(this);
//					if(OtherAssociationEnd.Association==null)
//					{
//						((AssociationEnd)OtherAssociationEnd).SetAssociation(new Association());
//						//(OtherAssociationEnd.Association as Association).Persistent=false;
//						((Association)OtherAssociationEnd.Association).SetRoleAAssociationEnd(OtherAssociationEnd);
//						AssociationEnd MyAssociationEnd=new AssociationEnd(NullFieldInfo);
//						MyAssociationEnd.SetSpecification(this);
//						MyAssociationEnd.SetAssociation(OtherAssociationEnd.Association);
//						((Association)MyAssociationEnd.Association).SetRoleBAssociationEnd(MyAssociationEnd);
//					}
//					else
//					{
//						AssociationEnd MyAssociationEnd=null;
//						if(OtherAssociationEnd.IsRoleA)
//							MyAssociationEnd=(AssociationEnd)OtherAssociationEnd.Association.RoleB;
//						else
//							MyAssociationEnd=(AssociationEnd)OtherAssociationEnd.Association.RoleA;
//						if(MyAssociationEnd==null)
//						{
//							MyAssociationEnd=new AssociationEnd(NullFieldInfo);
//							MyAssociationEnd.SetSpecification(this);
//							MyAssociationEnd.SetAssociation(OtherAssociationEnd.Association);
//							if(OtherAssociationEnd.IsRoleA)
//								((Association)MyAssociationEnd.Association).SetRoleBAssociationEnd(MyAssociationEnd);
//							else
//								((Association)MyAssociationEnd.Association).SetRoleAAssociationEnd(MyAssociationEnd);
//						}
//					}
//					_Roles.Add(OtherAssociationEnd.GetOtherEnd());
//					if(OtherAssociationEnd.GetOtherEnd()!=null)
//						if(OtherAssociationEnd.GetOtherEnd().GetOtherEnd()==OtherAssociationEnd)
//						{
//							int lop=0;
//							lop++;
//						}
//					
//				}
//
//				object[] Attributes =Refer.WrType.GetCustomAttributes(typeof(MetaDataRepository.AssociationClass),false);
//				if(Attributes.Length>0)
//				{
//					MetaDataRepository.AssociationClass mAssociationClass=(MetaDataRepository.AssociationClass)Attributes[0];
//					MetaDataRepository.Classifier RoleAClass=(MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mAssociationClass.AssocciationEndRoleA);
//					if(RoleAClass==null)
//					{
//						if(mAssociationClass.AssocciationEndRoleA.IsClass)
//							RoleAClass=new Class(new Type(mAssociationClass.AssocciationEndRoleA));
//						if(mAssociationClass.AssocciationEndRoleA.IsInterface)
//							RoleAClass=new Interface(new Type(mAssociationClass.AssocciationEndRoleA));
//					}
//					foreach(AssociationEnd CurrAssociationEnd in RoleAClass.Roles)
//					{
//						if(CurrAssociationEnd.Association.Name==mAssociationClass.AssocciationName)
//						{
//							_LinkAssociation=CurrAssociationEnd.Association;
//							break;
//						}
//					}
//					if(_LinkAssociation==null)
//					{
//						MetaDataRepository.Classifier RoleBClass=(MetaDataRepository.Classifier)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(mAssociationClass.AssocciationEndRoleB);
//						if(RoleBClass==null)
//						{
//							if(mAssociationClass.AssocciationEndRoleB.IsClass)
//								RoleBClass=new Class(new Type(mAssociationClass.AssocciationEndRoleB));
//							if(mAssociationClass.AssocciationEndRoleA.IsInterface)
//								RoleBClass=new Interface(new Type(mAssociationClass.AssocciationEndRoleB));
//						}
//						foreach(AssociationEnd CurrAssociationEnd in RoleBClass.Roles)
//						{
//							if(CurrAssociationEnd.Association.Name==mAssociationClass.AssocciationName)
//							{
//								_LinkAssociation=CurrAssociationEnd.Association;
//								break;
//							}
//						}
//					}
//				}

				RolesLoaded=true;
			}
		}

		/// <MetaDataID>{2F9E3E09-FF46-4333-A41D-F9039D5FED30}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection Roles
		{
			get
			{
				System.Reflection.FieldInfo NullFieldInfo=null;
				LoadAssociatonEnds();
				MetaDataRepository.MetaObjectCollection ReturnCollection=new MetaObjectCollection();
				ReturnCollection.AddCollection(_Roles);
				return ReturnCollection;
			
			}
		}
		/// <MetaDataID>{17A24539-C28F-48DE-B99C-905ECBC49041}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection Generalizations
		{
			get
			{
				if(_Generalizations==null)
					LoadGeneralizations();
				MetaDataRepository.MetaObjectCollection ReturnCollection=new MetaObjectCollection();
				ReturnCollection.AddCollection(_Generalizations);
				return ReturnCollection;
			}

		}
		
		/// <MetaDataID>{9ADF88EB-0355-4DCC-AFAC-EAFF08F438A2}</MetaDataID>
		public override MetaDataRepository.MetaObjectCollection Specializations
		{
			get
			{
				if(_Specializations==null)
					_Specializations=new MetaObjectCollection();

				MetaDataRepository.MetaObjectCollection ReturnCollection=new MetaObjectCollection();
				ReturnCollection.AddCollection(_Specializations);
				return ReturnCollection;

			
			}
		}

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
	
		/// <MetaDataID>{AE671606-A6D7-448D-B2E6-AC8717F9C601}</MetaDataID>
		internal Type Refer;
		bool FeaturesLoaded=false;

		/// <MetaDataID>{34AFCE91-6A33-434A-A031-D5FA611B588D}</MetaDataID>
		private void LoadFeatures()
		{
			if(!FeaturesLoaded)
			{
				FeaturesLoaded=true;
				_Features=Refer.Features;
//				foreach(MetaDataRepository.Feature mFeature in _Features)
//				{
//					if(typeof(DotNetMetaDataRepository.Operation).IsInstanceOfType(mFeature))
//						((DotNetMetaDataRepository.Operation)mFeature).SetNamespace(this);
//
//					if(typeof(DotNetMetaDataRepository.Method).IsInstanceOfType(mFeature))
//						((DotNetMetaDataRepository.Method)mFeature).SetNamespace(this);
//					if(typeof(Attribute).IsInstanceOfType(mFeature))
//						((Attribute)mFeature).SetNamespace(this);
//				}
				if(_OwnedElements==null)
				{
					_OwnedElements=new MetaObjectCollection();
					_OwnedElements.AddCollection(_Features);
				}
			}

		}

		/// <MetaDataID>{87C35C48-B6E9-4BC7-8458-93E7F26A6789}</MetaDataID>
		private void LoadGeneralizations()
		{
			if(_Generalizations==null)
			{
				_Generalizations=Refer.Generalizations;
				foreach(MetaDataRepository.Generalization Generalization in _Generalizations)
					Generalization.Child=this;
			}

		}
		/// <MetaDataID>{05DB0397-6131-48DE-9353-C9A9A42D1CA2}</MetaDataID>
		 public Interface(Type theType)
		{
			 RolesLoaded=false;
			 _Roles=new MetaObjectCollection();
			_Specializations=new MetaObjectCollection();
			DotNetMetaDataRepository.MetaObjectMapper.AddTypeMap(theType.WrType,this);

			Name=theType.Name;
			Refer=theType;
			Visibility=Refer.Visibility;
			LoadGeneralizations();
			if(Refer.isNestedType)
				return;
			Namespace mNamespace=(Namespace)DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(theType.WrType.Namespace);
			if(mNamespace==null)
				mNamespace=new Namespace(theType.WrType.Namespace);
			mNamespace.AddOwnedElement(this);
			SetNamespace(mNamespace);
			 _Realizations=new MetaObjectCollection();  


			 
	
		}
		/// <MetaDataID>{A90ECF13-1071-4372-B0F7-C8FFCC325267}</MetaDataID>
		public void SetNamespace(MetaDataRepository.Namespace OwnerNamespace)
		{
			_Namespace=OwnerNamespace;
		}

	}
}
