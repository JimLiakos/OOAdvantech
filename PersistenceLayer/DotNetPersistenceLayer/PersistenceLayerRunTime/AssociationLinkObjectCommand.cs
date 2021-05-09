namespace PersistenceLayerRunTime
{
	/// <MetaDataID>{0E5912A1-AF2A-4DA0-B9E6-C0E9E59A690A}</MetaDataID>
	/// <summary>LinkObjectCommand</summary>
	public class AssociationLinkObjectCommand : AssociationObjectCommand 
	{
		/// <MetaDataID>{78853B66-AEDC-40C8-BFF9-7B1923A52F07}</MetaDataID>
		public AssociationLinkObjectCommand ()
		{


		}
	

		/// <MetaDataID>{1D00AE81-B08A-4068-8E7A-7DB7E7AEEED6}</MetaDataID>
		public override int Order
		{
			get
			{
				return 3;
			}
		}
		/// <MetaDataID>{96CF20DA-42FE-4A5A-B2DA-50854DE56206}</MetaDataID>
		public override TransactionCommand MergeWith(TransactionCommand MergeCandidate,ref bool RemoveThis)
		{
			RemoveThis=false;
			return MergeCandidate;
		}
		/// <summary>Return the auto-produced commands from the main command. Some times must be produced new commands as result of initial command. For instance when delete a link between tow object and relation characterized as cascade delete.</summary>
		/// <MetaDataID>{2F377D24-7A3B-4832-AECE-7FABDE43AC89}</MetaDataID>
		public override TransactionCommandCollection GetSubTransactionCmds(int CurrentOrder)
		{
			return new TransactionCommandCollection();
		}

		/// <MetaDataID>{A20EFBB5-1777-4FCD-9267-1D4C2260F49F}</MetaDataID>
		private void ObjectsLinkInMemory(StorageInstanceRef AssociationEndStorageInstanceRef,PersistentAssociationEnd PersistentAssociationEnd)
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
				ObjectCollection.Add(AssociationObject.MemoryInstance,false);
				PersistenceLayer.PersistencyContext.CurrentPersistencyContext.CommitObjectStateTransition(AssociationEndStorageInstanceRef.MemoryInstance,ObjectStateTransitionID);
			}
			else
			{
				long ObjectStateTransitionID=PersistenceLayer.PersistencyContext.CurrentPersistencyContext.BeginObjectStateTransition(AssociationEndStorageInstanceRef.MemoryInstance,PersistenceLayer.Transaction.OnObjectChangeSate.Required);
				AssociationEndStorageInstanceRef.SetObjectLink(PersistentAssociationEnd.FieldMember,AssociationObject.MemoryInstance,false);
				PersistenceLayer.PersistencyContext.CurrentPersistencyContext.CommitObjectStateTransition(AssociationEndStorageInstanceRef.MemoryInstance,ObjectStateTransitionID);
			}
		}
		/// <summary>With this method execute the command.</summary>
		/// <MetaDataID>{272E7D23-4970-462D-B78E-6872ECD8ECCF}</MetaDataID>
		public override void Execute()
		{
			#region Preconditions Chechk
			if(AssociationRoleA==null||AssociationRoleB==null||AssociationObject==null)
				throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
			if(AssociationClass==null)
				throw (new System.Exception("The metadata of the command isn't set correctly."));//Message
			if(AssociationClass.LinkAssociation==null)
				throw (new System.Exception("AssociationClass with out link association."));//Message


			/*
			if(theResolver.AssociationEndMetaData.PersistentAttribute.IsRoleA)
			{
				System.Reflection.FieldInfo  FieldRoleA=RoleA.MemoryInstance.GetType().GetField("RoleA",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo  FieldRoleB=RoleA.MemoryInstance.GetType().GetField("RoleB",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
				if(FieldRoleA==null)
					throw (new System.Exception("The Association Class "+RoleA.MemoryInstance.GetType().Name+" hasn't RoleA Field of the command isn't set correctly."));//Message
				if(FieldRoleB==null)
					throw (new System.Exception("The Association Class "+RoleA.MemoryInstance.GetType().Name+" hasn't RoleB Field of the command isn't set correctly."));//Message
			}
			else
			{
				System.Reflection.FieldInfo  FieldRoleA=RoleB.MemoryInstance.GetType().GetField("RoleA",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
				System.Reflection.FieldInfo  FieldRoleB=RoleB.MemoryInstance.GetType().GetField("RoleB",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
				if(FieldRoleA==null)
					throw (new System.Exception("The Association Class "+RoleA.MemoryInstance.GetType().Name+" hasn't RoleA Field of the command isn't set correctly."));//Message
				if(FieldRoleB==null)
					throw (new System.Exception("The Association Class "+RoleA.MemoryInstance.GetType().Name+" hasn't RoleB Field of the command isn't set correctly."));//Message
			}*/
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
				AssociationRoleA.ReferentialIntegrityLinkAdded();
			if(RoleAReferentialIntegrity)
				AssociationRoleB.ReferentialIntegrityLinkAdded();
			/******************** Referential Integrity *******************/


			/******************** Complete the objects link in memory *******************/
			
			if(AssociationClass.LinkAssociation.RoleA.Navigable)
			{
				PersistentAssociationEnd PersistentAssociationEnd=new PersistentAssociationEnd(
					AssociationClass.LinkAssociation.RoleA);
				ObjectsLinkInMemory(AssociationRoleB,PersistentAssociationEnd);
			}
			if(AssociationClass.LinkAssociation.RoleB.Navigable)
			{
				PersistentAssociationEnd PersistentAssociationEnd=new PersistentAssociationEnd(
					AssociationClass.LinkAssociation.RoleB);
				ObjectsLinkInMemory(AssociationRoleA,PersistentAssociationEnd);
			}
			/******************** Complete the objects link in memory *******************/
		}
	}
}
