namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	/// <MetaDataID>{25E3581E-2FBF-4B60-8B87-100F9ED4348A}</MetaDataID>
	public abstract class InterSorageLinkObjectsCommand : LinkCommand 
	{
		/// <MetaDataID>{814BBA54-9470-49AA-BDD3-202E6889CBFF}</MetaDataID>
		protected ObjectStorage  CommandInitiatorStorage;
		/// <MetaDataID>{E6D63C64-F682-4AD0-88FA-2E698251CB7E}</MetaDataID>
		 public InterSorageLinkObjectsCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, ObjectStorage commandInitiatorStorage):
			base(roleA,roleB,relationObject,linkInitiatorAssociationEnd)
		{
			#region Preconditions Chechk
			if(roleA==null||roleB==null)
				throw new System.Exception("Error on objects that will be linked. Must be not null.");//Message
			if(linkInitiatorAssociationEnd==null)
				throw new System.Exception("The metadata of the command isn't set correctly.");//Message
			//if(commandProducerAssociationEnd.Association.LinkClass!=null&&relationObject==null) 
			#endregion
			 CommandInitiatorStorage=commandInitiatorStorage;
		}
		/// <MetaDataID>{5BF3F1CF-C124-41EC-A24F-1FE463BF7F51}</MetaDataID>
		public override string Identity
		{
			get
			{
				return "link"+RoleA.MemoryID.ToString()+RoleB.MemoryID.ToString()+this.LinkInitiatorAssociationEnd.Association.Identity;
			}
		}


		/// <MetaDataID>{20AE53D3-BCCC-4557-9ADC-08F2D0A26971}</MetaDataID>
		public override void Execute()
		{
            try
            {
                MetaDataRepository.Association association = LinkInitiatorAssociationEnd.Association;
                MetaDataRepository.Roles commandInitiatorRole;
                if (LinkInitiatorAssociationEnd.IsRoleA)
                    commandInitiatorRole = MetaDataRepository.Roles.RoleA;
                else
                    commandInitiatorRole = MetaDataRepository.Roles.RoleB;


                if (RoleA.ObjectStorage == CommandInitiatorStorage)
                {
                    if (RoleB.Class.HasReferentialIntegrity(association.RoleA as DotNetMetaDataRepository.AssociationEnd))
                        RoleA.RealStorageInstanceRef.ReferentialIntegrityLinkAdded();
                }
                else
                {
                    if (RoleA.Class.HasReferentialIntegrity(association.RoleB as DotNetMetaDataRepository.AssociationEnd))
                        RoleB.RealStorageInstanceRef.ReferentialIntegrityLinkAdded();
                }

                if (association.LinkClass == null)
                {
                    if (commandInitiatorRole == MetaDataRepository.Roles.RoleA)
                    {
                        if (RoleA.ObjectStorage == CommandInitiatorStorage)
                        {
                            if (association.RoleB.Navigable)
                                RoleA.RealStorageInstanceRef.SetObjectsLink(new AssociationEndAgent(association.RoleB as DotNetMetaDataRepository.AssociationEnd), RoleB.MemoryInstance, true);
                        }
                    }
                    else
                    {
                        if (RoleB.ObjectStorage == CommandInitiatorStorage)
                        {
                            if (association.RoleA.Navigable)
                                RoleB.RealStorageInstanceRef.SetObjectsLink(new AssociationEndAgent(association.RoleA as DotNetMetaDataRepository.AssociationEnd), RoleA.MemoryInstance, true);
                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                throw error;
            }


		}

	


	}
}
