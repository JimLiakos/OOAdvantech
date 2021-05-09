namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	/// <MetaDataID>{81E1577F-F61A-4130-9256-B3215E703188}</MetaDataID>
	/// <summary></summary>
	public abstract class NewStorageInstanceCommand : Command
	{
		/// <MetaDataID>{8F966923-C66B-4C6C-A904-7FB16DCCC3A9}</MetaDataID>
		public override string Identity
		{
			get
			{
				return "new"+OnFlyStorageInstance.MemoryID.ToString();
			}
		}
		/// <MetaDataID>{6E94CC21-FF74-412F-B1F4-4DFF7FD7D137}</MetaDataID>
		public static string GetIdentity(StorageInstanceRef storageInstanceRef)
		{
			return "new"+storageInstanceRef.MemoryID.ToString();
		}


		/// <MetaDataID>{3076248A-2CDF-4B44-904C-2219A0005678}</MetaDataID>
		protected bool RelationCommandsProduced=false;

		/// <MetaDataID>{DF595828-4614-4F37-8789-EB9DF0943D47}</MetaDataID>
		protected bool LinkClassCommandsProduced=false;

		/// <MetaDataID>{CF7B1092-C49D-4251-90C6-DEAD15A237DB}</MetaDataID>
		public NewStorageInstanceCommand(StorageInstanceRef storageInstanceRef )
		{
			OnFlyStorageInstance=storageInstanceRef;
		}

		/// <MetaDataID>{97339A95-63E9-407F-93AD-246381651223}</MetaDataID>
		public override void GetSubCommands(int currentExecutionOrder)
		{
			
			#region Preconditions Chechk
			if(OnFlyStorageInstance==null)
				throw (new System.Exception("From NewStorageInstanceCommand: You try to catch changes of a nothing object."));
			#endregion


			//CommandCollection SubCommands=new CommandCollection();
			if(currentExecutionOrder<0)
				return ;
			if(!RelationCommandsProduced)
			{
				RelationCommandsProduced=true;


                OOAdvantech.MetaDataRepository.Operation operation = OnFlyStorageInstance.Class.BeforeCommitObjectStateInStorage;
                if (operation != null)
                {
                    System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                    OOAdvantech.AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(OnFlyStorageInstance.MemoryInstance, new object[0]);
                }
                else if (OnFlyStorageInstance.MemoryInstance is OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer)
                    (OnFlyStorageInstance.MemoryInstance as OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer).BeforeCommitObjectState();

                OnFlyStorageInstance.MakeRelationChangesCommands();
			}
			if(!LinkClassCommandsProduced&&currentExecutionOrder==10)
			{
				LinkClassCommandsProduced=true;
				ProduceLinkClassCommands(OnFlyStorageInstance.Class);
			}
		}


		/// <MetaDataID>{E229E277-D6D6-419D-9D42-0D7FE4E80F90}</MetaDataID>
		void ProduceLinkClassCommands(DotNetMetaDataRepository.Class  _class )
		{
			//TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
//			foreach(MetaDataRepository.Generalization generalization in _class.Generalizations)
//				ProduceLinkClassCommands(generalization.Parent as DotNetMetaDataRepository.Class);
			if(_class.ClassHierarchyLinkAssociation!=null&&_class.Persistent)
			{
				//Error Prone TypeCheck
				PersistenceLayerRunTime.Commands.LinkCommand mLinkObjectsCommand =null;
				StorageInstanceAgent roleA=null,roleB=null;
				
				GetRolesObject(_class,OnFlyStorageInstance,ref roleA,ref roleB);


		

				if( _class.ClassHierarchyLinkAssociation.RoleA.Navigable)
					roleB.RealStorageInstanceRef.SetObjectsLink(new AssociationEndAgent((DotNetMetaDataRepository.AssociationEnd) _class.ClassHierarchyLinkAssociation.RoleA),OnFlyStorageInstance.MemoryInstance,true);
				if( _class.ClassHierarchyLinkAssociation.RoleB.Navigable)
					roleA.RealStorageInstanceRef.SetObjectsLink(new AssociationEndAgent((DotNetMetaDataRepository.AssociationEnd) _class.ClassHierarchyLinkAssociation.RoleB),OnFlyStorageInstance.MemoryInstance,true);
				


				//Error prone μπορεί το associationobject να είναι σε storage διαφορετική και απο αυτη του
				//RoleA object και roleB object.

				ObjectStorage roleAObjectStorage,roleBObjectStorage;
				roleAObjectStorage=roleA.ObjectStorage;
				roleBObjectStorage=roleB.ObjectStorage;


				DotNetMetaDataRepository.AssociationEnd navigableassociationEnd=null;
				if(_class.ClassHierarchyLinkAssociation.RoleA.Navigable)
				{
					navigableassociationEnd=_class.ClassHierarchyLinkAssociation.RoleA as DotNetMetaDataRepository.AssociationEnd;
				}
				else if(_class.ClassHierarchyLinkAssociation.RoleB.Navigable)
				{
					navigableassociationEnd=_class.ClassHierarchyLinkAssociation.RoleB as DotNetMetaDataRepository.AssociationEnd;
				}
				else
					throw new System.Exception("Error association with no navigable association end"); 


				
				if(roleAObjectStorage==roleBObjectStorage)
				{
					if(navigableassociationEnd.IsRoleA)
						roleBObjectStorage.CreateLinkCommand( roleA,roleB,new StorageInstanceAgent(OnFlyStorageInstance),new AssociationEndAgent(navigableassociationEnd),-1);
					else
						roleAObjectStorage.CreateLinkCommand(roleA,roleB,new StorageInstanceAgent(OnFlyStorageInstance),new AssociationEndAgent(navigableassociationEnd),-1);

				}
				else
				{
					((PersistenceLayerRunTime.ObjectStorage)roleAObjectStorage).CreateLinkCommand(roleA,roleB,new StorageInstanceAgent(OnFlyStorageInstance),new AssociationEndAgent(navigableassociationEnd),-1);
                    ((PersistenceLayerRunTime.ObjectStorage)roleBObjectStorage).CreateLinkCommand(roleA, roleB, new StorageInstanceAgent(OnFlyStorageInstance), new AssociationEndAgent(navigableassociationEnd), -1);

//
//					mLinkObjectsCommand=new LinkObjectsCommand(roleA,roleB,new StorageInstanceAgent(OnFlyStorageInstance),new AssociationEndAgent(navigableassociationEnd));
//					TransactionContext.CurrentTransactionContext.EnlistCommand(mLinkObjectsCommand );
				}
				int hhj=0;
			}
		}
		/// <MetaDataID>{B3F346FE-6299-41C1-9573-85C0C44AE441}</MetaDataID>
		protected void GetRolesObject(DotNetMetaDataRepository.Class linkClass,StorageInstanceRef RelationObject,ref StorageInstanceAgent roleA,ref StorageInstanceAgent roleB)
		{
			//roleA=StorageInstanceRef.GetStorageInstanceAgent(linkClass.LinkClassRoleAField.GetValue(RelationObject.MemoryInstance));
            roleA = StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(linkClass.LinkClassRoleAFastFieldAccessor.GetValue, RelationObject.MemoryInstance));

			//roleB=StorageInstanceRef.GetStorageInstanceAgent(linkClass.LinkClassRoleBField.GetValue(RelationObject.MemoryInstance));
            roleB = StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(linkClass.LinkClassRoleBFastFieldAccessor.GetValue,RelationObject.MemoryInstance));

/*
			MetaDataRepository.MetaObjectCollection Attributes= linkClass.GetAttributes(false);
			System.Type linkClassType=linkClass.GetExtensionMetaObject(typeof(System.Type))as System.Type;
			
			foreach (MetaDataRepository.Attribute CurrAttribute in Attributes)
			{
				object Value=CurrAttribute.GetPropertyValue(typeof(bool),"MetaData","AssociationClassRole");
				bool IsAssociationClassRole =false;
				if(Value!=null)
					IsAssociationClassRole =(bool)Value;
				if(IsAssociationClassRole)
				{
					System.Reflection.MemberInfo MemberInfo=CurrAttribute.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
					MetaDataRepository.AssociationClassRole AssociationClassRole=MemberInfo.GetCustomAttributes(typeof(MetaDataRepository.AssociationClassRole),true)[0] as MetaDataRepository.AssociationClassRole;
					
					System.Reflection.FieldInfo  FieldRole=null;
					if(MemberInfo is System.Reflection.FieldInfo)
						FieldRole=MemberInfo as System.Reflection.FieldInfo;
					else
						FieldRole=linkClassType.GetField(AssociationClassRole.ImplMemberName,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance); // Error Prone if FieldRoleA==null;
					object MemoryInstanceRole=FieldRole.GetValue(RelationObject.MemoryInstance);


					if(AssociationClassRole.IsRoleA) // Error Prone το exception που εγείρεται όταν δεν είναι συμπληρομένα τα RoleA RoleB δεν λέει τύποτα στο χρήστη.
					{
						//Type check
						if(MemoryInstanceRole==null||!(linkClass.LinkAssociation.RoleA.Specification.GetExtensionMetaObject(typeof(System.Type))as System.Type).IsInstanceOfType(MemoryInstanceRole))
							throw new System.Exception("Wrong value in "+FieldRole.DeclaringType.FullName+"."+FieldRole.Name);

						roleA=StorageInstanceRef.GetStorageInstanceAgent(MemoryInstanceRole); 

						//Persistency check
						if(roleA==null)
							throw new System.Exception("the object at "+FieldRole.DeclaringType.FullName+"."+FieldRole.Name+" is transient.");//Error Prone ποιό καλά διατυπωμένο μήνημα χριάζεται
					}
					else
					{
						//Type check
						if(MemoryInstanceRole==null||!(linkClass.LinkAssociation.RoleB.Specification.GetExtensionMetaObject(typeof(System.Type))as System.Type).IsInstanceOfType(MemoryInstanceRole))
							throw new System.Exception("Wrong value in "+FieldRole.DeclaringType.FullName+"."+FieldRole.Name);//Error Prone ποιό καλά διατυπωμένο μήνημα χριάζεται
						
						roleB=StorageInstanceRef.GetStorageInstanceAgent(MemoryInstanceRole); 

						//Persistency check
						if(roleB==null)
							throw new System.Exception("the object at "+FieldRole.DeclaringType.FullName+"."+FieldRole.Name+" is transient.");//Error Prone ποιό καλά διατυπωμένο μήνημα χριάζεται
					}
				}
			}*/
		}

		/// <MetaDataID>{44AEF350-702E-4F43-A433-63C3431D0BE8}</MetaDataID>
		public override int ExecutionOrder
		{
			get 
			{
				return 10;
			}
		}
		/// <summary></summary>
		/// <MetaDataID>{89AE50E6-33DB-49C8-81D5-84B79133A91A}</MetaDataID>
		public StorageInstanceRef OnFlyStorageInstance;
	
	}
}
