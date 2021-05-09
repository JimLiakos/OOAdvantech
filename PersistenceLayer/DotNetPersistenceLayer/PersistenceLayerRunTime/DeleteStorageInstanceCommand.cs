
namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	/// <MetaDataID>{9314A1D6-323F-423F-BAE1-104866FDA334}</MetaDataID>
	/// <summary></summary>
	public abstract class DeleteStorageInstanceCommand : Command
	{
		/// <MetaDataID>{90684F56-5E32-462B-AAAF-701488D39F60}</MetaDataID>
		public override string Identity
		{
			get
			{
				return "delete"+StorageInstanceForDeletion.MemoryID.ToString();
			}
		}
 

		/// <MetaDataID>{930649F5-29A4-471A-B548-B741E00EB77A}</MetaDataID>
		public override void Execute()
		{
			if(StorageInstanceForDeletion.ReferentialIntegrityCount>0)
				throw new System.Exception("The object '"+StorageInstanceForDeletion.MemoryInstance.ToString()+"' can't be deleted. Referential Integrity Error.");
			StorageInstanceForDeletion.OnObjectDeleted();
		}
		/// <MetaDataID>{3AE7C4F6-3885-4811-954B-A6F960F786C3}</MetaDataID>
		public PersistenceLayer.DeleteOptions DeleteOption;
		/// <MetaDataID>{5C125866-A718-48B1-96CF-16314FDA7FD3}</MetaDataID>
		private bool SubTransactionCmdsProduced;
		/// <MetaDataID>{35FB5621-2DC9-4A70-B311-FC3F340245F8}</MetaDataID>
        public DeleteStorageInstanceCommand(StorageInstanceRef storageInstanceForDeletion, PersistenceLayer.DeleteOptions deleteOption)
		{
			DeleteOption=deleteOption;
			SubTransactionCmdsProduced=false;
			StorageInstanceForDeletion=storageInstanceForDeletion;
		}
		/// <MetaDataID>{E59E87A2-4C3B-48DD-ADD0-167F5224A14E}</MetaDataID>
		public StorageInstanceRef StorageInstanceForDeletion;
		/// <MetaDataID>{6D4FF38E-EBB3-4976-A828-9777DA3ED726}</MetaDataID>
		public override void GetSubCommands(int CurrentOrder)
		{
			if(StorageInstanceForDeletion.ReferentialIntegrityCount>0)
				return ;

			if(!SubTransactionCmdsProduced)
			{
				StorageInstanceForDeletion.ObjectDeleting();
				SubTransactionCmdsProduced =true;
				foreach(RelResolver relResolver in StorageInstanceForDeletion.RelResolvers)
					((ObjectStorage)StorageInstanceForDeletion.ObjectStorage).CreateUnlinkAllObjectCommand(new StorageInstanceAgent(relResolver.Owner),relResolver.AssociationEnd);
				ProduceLinkClassCommands(StorageInstanceForDeletion.Class);
			}			
		}
		/// <MetaDataID>{B7A6062E-CAF6-4F49-AF7C-CD912F93592E}</MetaDataID>
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
				
				
				GetRolesObject(_class,StorageInstanceForDeletion,ref roleA,ref roleB);

				if( _class.ClassHierarchyLinkAssociation.RoleA.Navigable)
					roleB.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent((DotNetMetaDataRepository.AssociationEnd) _class.ClassHierarchyLinkAssociation.RoleA),StorageInstanceForDeletion.MemoryInstance,true);
				if( _class.ClassHierarchyLinkAssociation.RoleB.Navigable)
					roleA.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent((DotNetMetaDataRepository.AssociationEnd) _class.ClassHierarchyLinkAssociation.RoleB),StorageInstanceForDeletion.MemoryInstance,true);
				
				//Error prone μπορεί το associationobject να είναι σε storage διαφορετική και απο αυτη του
				//RoleA object και RoleB object.



				PersistenceLayer.ObjectStorage roleAObjectStorage,roleBObjectStorage;
				roleAObjectStorage=roleA.ObjectStorage;
				roleBObjectStorage=roleB.ObjectStorage;
				StorageInstanceAgent relationObject=new StorageInstanceAgent(StorageInstanceForDeletion);

				//error prone υπάρχει περίπτωση σε association class κανένα AssociationEnd να μην έχει navigation
				DotNetMetaDataRepository.AssociationEnd associationEnd=null;
				if(_class.ClassHierarchyLinkAssociation.RoleA.Navigable)
					associationEnd=_class.ClassHierarchyLinkAssociation.RoleA as DotNetMetaDataRepository.AssociationEnd;
				else
					associationEnd=_class.ClassHierarchyLinkAssociation.RoleB as DotNetMetaDataRepository.AssociationEnd;

				if(roleAObjectStorage==roleBObjectStorage)
				{
					((PersistenceLayerRunTime.ObjectStorage)StorageInstanceForDeletion.ObjectStorage).CreateUnLinkCommand(roleA,roleB,null,new AssociationEndAgent(associationEnd),-1);
				}
				else
				{
					
					((ObjectStorage)roleAObjectStorage).CreateUnLinkCommand(roleA,roleB,relationObject,new AssociationEndAgent(associationEnd),-1);
					((ObjectStorage)roleBObjectStorage).CreateUnLinkCommand(roleA,roleB,relationObject,new AssociationEndAgent(associationEnd),-1);
//					
//					PersistenceLayerRunTime.Commands.LinkCommand unLinkObjectsCommand=new UnLinkObjectsCommand(roleA,roleB,null,new AssociationEndAgent(associationEnd));
//					TransactionContext.CurrentTransactionContext.EnlistCommand(unLinkObjectsCommand);
//					
				}


				int hhj=0;


			}
		}

		/// <MetaDataID>{66AF8CA5-E0A2-4748-9CDD-7A600B179670}</MetaDataID>
		protected void GetRolesObject(DotNetMetaDataRepository.Class linkClass,StorageInstanceRef RelationObject,ref StorageInstanceAgent roleA,ref StorageInstanceAgent roleB)
		{



            
            
			//roleA=StorageInstanceRef.GetStorageInstanceAgent(linkClass.LinkClassRoleAField.GetValue(RelationObject.MemoryInstance));
            roleA=StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(linkClass.LinkClassRoleAFastFieldAccessor.GetValue, RelationObject.MemoryInstance));
			//roleB=StorageInstanceRef.GetStorageInstanceAgent(linkClass.LinkClassRoleBField.GetValue(RelationObject.MemoryInstance));
            roleB=StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(linkClass.LinkClassRoleBFastFieldAccessor.GetValue,RelationObject.MemoryInstance ));

			/*
			MetaDataRepository.MetaObjectCollection Attributes= RelationObject.Class.GetAttributes(true);
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


					System.Type linkClassType=linkClass.GetExtensionMetaObject(typeof(System.Type))as System.Type;
					System.Reflection.FieldInfo  FieldRole=null;
					if(MemberInfo is System.Reflection.FieldInfo)
						FieldRole=MemberInfo as System.Reflection.FieldInfo;
					else
						FieldRole=linkClassType.GetField(AssociationClassRole.ImplMemberName,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance); // Error Prone if FieldRoleA==null;
					object MemoryInstanceRole=FieldRole.GetValue(RelationObject.MemoryInstance);

					if(AssociationClassRole.IsRoleA) // Error Prone το exception που εγείρεται όταν δεν είναι συμπληρομένα τα RoleA RoleB δεν λέει τύποτα στο χρήστη.
						RoleA=StorageInstanceRef.GetStorageInstanceRef(MemoryInstanceRole) as StorageInstanceRef; 
					else
						RoleB=StorageInstanceRef.GetStorageInstanceRef(MemoryInstanceRole)as StorageInstanceRef; 
				}
			}*/
		}
		
		/// <MetaDataID>{5092C1E3-DBB0-4B26-8F96-3A9FFD4F1876}</MetaDataID>
		public override int ExecutionOrder
		{
			get 
			{
				return 70;
			}
		}
	}
}



