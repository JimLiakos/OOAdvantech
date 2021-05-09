namespace PersistenceLayerRunTime
{
	/// <MetaDataID>{8F4E8F80-8F20-49F6-9336-919706A23623}</MetaDataID>
	public class DeleteAssociationLinkObjectCommand : AssociationObjectCommand 
	{
		bool SubTransactionCmdsProduced;
		/// <MetaDataID>{79317858-DEB6-4BA7-96B6-10CC0FA6C2CB}</MetaDataID>
		public DeleteAssociationLinkObjectCommand()
		{
			SubTransactionCmdsProduced=false;
		}

		/// <MetaDataID>{98A73D22-99DB-4154-834F-D315C7659EB3}</MetaDataID>
		public override int Order
		{
			get
			{ 
				return 4;
			}
		}
		/// <summary>With this method execute the command.</summary>
		/// <MetaDataID>{05DC9FF9-B225-4F12-AA0E-896314FCD422}</MetaDataID>
		public override void Execute()
		{
			#region Preconditions Chechk
			if(AssociationRoleA==null||AssociationRoleB==null||AssociationObject==null)
				throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
			if(AssociationClass==null)
				throw (new System.Exception("The metadata of the command isn't set correctly."));//Message
			if(AssociationClass.LinkAssociation==null)
				throw (new System.Exception("AssociationClass with out link association."));//Message
			#endregion

			/******************** Referential Integrity *******************/
			bool RoleAReferentialIntegrity=false;
			bool RoleBReferentialIntegrity=false;
			object tempObject=AssociationClass.LinkAssociation.RoleA.GetPropertyValue(typeof(bool),"Persistency","ReferentialIntegrity");
			if(tempObject!=null)
				RoleAReferentialIntegrity=(bool)tempObject;
			tempObject=AssociationClass.LinkAssociation.RoleB.GetPropertyValue(typeof(bool),"Persistency","ReferentialIntegrity");
			if(tempObject!=null)
				RoleBReferentialIntegrity=(bool)tempObject;
		
			if(RoleAReferentialIntegrity)
				AssociationRoleA.ReferentialIntegrityLinkRemoved();
			if(RoleAReferentialIntegrity)
				AssociationRoleB.ReferentialIntegrityLinkRemoved();
			/******************** Referential Integrity *******************/


			/******************** Complete the objects link in memory *******************/
			if(AssociationClass.LinkAssociation.RoleA.Navigable)
			{
				PersistentAssociationEnd PersistentAssociationEnd=new PersistentAssociationEnd(
					AssociationClass.LinkAssociation.RoleA);
				ObjectsUnLinkInMemory(AssociationRoleB,PersistentAssociationEnd);
			}
			if(AssociationClass.LinkAssociation.RoleB.Navigable)
			{
				PersistentAssociationEnd PersistentAssociationEnd=new PersistentAssociationEnd(
					AssociationClass.LinkAssociation.RoleB);
				ObjectsUnLinkInMemory(AssociationRoleA,PersistentAssociationEnd);
			}
			/******************** Complete the objects link in memory *******************/
		}

		/// <MetaDataID>{F7F44B70-7D29-4008-8CC9-E03B68872E87}</MetaDataID>
		private void ObjectsUnLinkInMemory(StorageInstanceRef AssociationEndStorageInstanceRef,PersistentAssociationEnd PersistentAssociationEnd)
		{
			if(PersistentAssociationEnd.FieldMember.FieldType==typeof(PersistenceLayer.ObjectContainer)||PersistentAssociationEnd.FieldMember.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
			{
				PersistenceLayer.ObjectContainer theObjectContainer;
				theObjectContainer=(PersistenceLayer.ObjectContainer)PersistentAssociationEnd.FieldMember.GetValue(AssociationEndStorageInstanceRef.MemoryInstance);
				if(theObjectContainer==null)
					throw new System.Exception("The object container "+PersistentAssociationEnd.FieldMember.Name+" rato must be initialized at construction time."); 
				System.Reflection.FieldInfo mFieldInfo =theObjectContainer.GetType().GetField("theObjects",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
				PersistenceLayerRunTime.ObjectCollection ObjectCollection=(PersistenceLayerRunTime.ObjectCollection)mFieldInfo.GetValue(theObjectContainer);

				long ObjectStateTransitionID=PersistenceLayer.PersistencyContext.CurrentPersistencyContext.BeginObjectStateTransition(AssociationEndStorageInstanceRef.MemoryInstance,PersistenceLayer.Transaction.OnObjectChangeSate.Required);
				ObjectCollection.Remove(AssociationObject.MemoryInstance,false);
				PersistenceLayer.PersistencyContext.CurrentPersistencyContext.CommitObjectStateTransition(AssociationEndStorageInstanceRef.MemoryInstance,ObjectStateTransitionID);
			}
			else
			{
				long ObjectStateTransitionID=PersistenceLayer.PersistencyContext.CurrentPersistencyContext.BeginObjectStateTransition(AssociationEndStorageInstanceRef.MemoryInstance,PersistenceLayer.Transaction.OnObjectChangeSate.Required);
				AssociationEndStorageInstanceRef.SetObjectLink(PersistentAssociationEnd.FieldMember,null,false);
				PersistenceLayer.PersistencyContext.CurrentPersistencyContext.CommitObjectStateTransition(AssociationEndStorageInstanceRef.MemoryInstance,ObjectStateTransitionID);
			}
		}

	
		/// <summary>Return the auto-produced commands from the main command. Some times must be produced new commands as result of initial command. For instance when delete a link between tow object and relation characterized as cascade delete.</summary>
		/// <MetaDataID>{566692C5-E714-4C22-BBFD-3D8C76D18A62}</MetaDataID>
		public override TransactionCommandCollection GetSubTransactionCmds(int CurrentOrder)
		{
			if(CurrentOrder<=0)
				return new PersistenceLayerRunTime.TransactionCommandCollection();

			if(!SubTransactionCmdsProduced)
			{
				SubTransactionCmdsProduced=true;
				TransactionCommandCollection mTransactionCommandCollection=new TransactionCommandCollection();

				bool RoleACascadeDelete=false;
				bool RoleBCascadeDelete=false;
				object tempObject=AssociationClass.LinkAssociation.RoleA.GetPropertyValue(typeof(bool),"Persistency","CascadeDelete");
				if(tempObject!=null)
					RoleACascadeDelete=(bool)tempObject;
				tempObject=AssociationClass.LinkAssociation.RoleB.GetPropertyValue(typeof(bool),"Persistency","CascadeDelete");
				if(tempObject!=null)
					RoleBCascadeDelete=(bool)tempObject;
		
				if(RoleACascadeDelete)
				{
						PersistenceLayerRunTime.StorageSession DestinationObjectStorageSession=
							(PersistenceLayerRunTime.StorageSession)AssociationRoleA.ActiveStorageSession;
						DeleteStorageInstanceCommand DeleteCommand=DestinationObjectStorageSession.CreateDeleteStorageInstanceCommand(AssociationRoleA);
						DeleteCommand.TryToDelete=true;
						mTransactionCommandCollection.Add(DeleteCommand);
				}
						
				if(RoleBCascadeDelete)
				{
					PersistenceLayerRunTime.StorageSession DestinationObjectStorageSession=
						(PersistenceLayerRunTime.StorageSession)AssociationRoleB.ActiveStorageSession;
					DeleteStorageInstanceCommand DeleteCommand=DestinationObjectStorageSession.CreateDeleteStorageInstanceCommand(AssociationRoleB);
					DeleteCommand.TryToDelete=true;
					mTransactionCommandCollection.Add(DeleteCommand);
				}
				return mTransactionCommandCollection;
			}
			else
				return new TransactionCommandCollection();
		}
		/// <MetaDataID>{AE77A74E-8212-4BA4-A0BA-49251C9EEBDB}</MetaDataID>
		public override TransactionCommand MergeWith(TransactionCommand MergeCandidate,ref bool RemoveThis)
		{
			RemoveThis=false;
			return MergeCandidate;
		}
	}
}
