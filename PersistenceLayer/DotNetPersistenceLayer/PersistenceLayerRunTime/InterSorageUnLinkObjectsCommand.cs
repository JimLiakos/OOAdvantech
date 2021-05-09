namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	/// <MetaDataID>{A70E9E55-9E33-43F6-90D7-C226D5912BA5}</MetaDataID>
	public abstract class InterSorageUnLinkObjectsCommand : LinkCommand
	{
		/// <MetaDataID>{DFDA99D6-833D-420E-9731-566187919789}</MetaDataID>
		protected ObjectStorage  CommandInitiatorStorage;
	

		/// <MetaDataID>{BDDD3130-AA58-4AEF-84B7-939908106ABE}</MetaDataID>
		public override string Identity
		{
			get
			{
				return "unlink"+RoleA.MemoryID.ToString()+RoleB.MemoryID.ToString()+this.LinkInitiatorAssociationEnd.Association.Identity;
			}
		}

		/// <MetaDataID>{969934D0-9196-488D-A0ED-FF87E1D5F4A3}</MetaDataID>
		 public InterSorageUnLinkObjectsCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject,AssociationEndAgent linkInitiatorAssociationEnd,ObjectStorage commandInitiatorStorage):
			base(roleA,roleB,relationObject,linkInitiatorAssociationEnd)
		{
			 CommandInitiatorStorage=commandInitiatorStorage;

		}
		/// <MetaDataID>{2DB9CA45-9A14-4841-8B3C-E4BC12118B39}</MetaDataID>
		bool SubTransactionCmdsProduced=false;

		/// <MetaDataID>{1ED9E03C-21EC-40FC-B0E4-5C02741360B8}</MetaDataID>
		public override void GetSubCommands(int currentExecutionOrder)
		{
			if(currentExecutionOrder<=10)
				return ;

			if(!SubTransactionCmdsProduced)
			{

				SubTransactionCmdsProduced=true;
				//TODO: Αυτός ο τρόπος που γινεται το cascade delete κάνει πολλές interpocess call
				//μια για να πάρει την class, μια για το IsCascadeDelete, μια για την ActiveStorageSession και
				//μια για το CreateDeleteStorageInstanceCommand σύνολο 5.
				//Θα μπορούσε και με μία κλήση αν προσθέσουμε μια method TryCascadeDelete στο StorageInstanceRef

				//TODO: Δεδομένο του ότι στις interstorage links παράγονται δυπλα unlink command τότε και η 
				//cascade delete διαδικασία θα γίνει δύο φορές η command στο τέλος θα είναι μία γιατί θα φρενάρει στο
				//μηχανισμό απαοφηγής δυπών command από το transaction context object.
				//Υπάρχει η δυνατότητα αποφηγής της δυπλήε διαδικασίας cascade delete.
				//TODO: Να γραφτεί test case για cascade delete σε interstorge links;

				if(LinkInitiatorAssociationEnd.IsRoleA&&RoleB.Class.IsCascadeDelete(LinkInitiatorAssociationEnd as DotNetMetaDataRepository.AssociationEnd))
				{
					if(!Remoting.RemotingServices.IsOutOfProcess(RoleA.RealStorageInstanceRef))
					{

						PersistenceLayerRunTime.ITransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                        foreach (System.Collections.Generic.KeyValuePair<string, Commands.Command> entry in transactionContext.EnlistedCommands)
						{
							Command command=entry.Value as Command;
							if(command is InterSorageLinkObjectsCommand||command is LinkObjectsCommand)
							{
								if((command as LinkCommand).RoleA==RoleA||
									(command as LinkCommand).RoleB==RoleA)
									return;
							}
						}

						PersistenceLayerRunTime.ObjectStorage DestinationObjectStorage=RoleA.ObjectStorage;
						if(DestinationObjectStorage==CommandInitiatorStorage)
							DestinationObjectStorage.CreateDeleteStorageInstanceCommand(RoleA.RealStorageInstanceRef,PersistenceLayer.DeleteOptions.TryToDelete);
					}
				}
				else if(RoleA.Class.IsCascadeDelete(LinkInitiatorAssociationEnd as DotNetMetaDataRepository.AssociationEnd))
				{
					if(!Remoting.RemotingServices.IsOutOfProcess(RoleB.RealStorageInstanceRef))
					{
						PersistenceLayerRunTime.ITransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                        foreach (System.Collections.Generic.KeyValuePair<string, Commands.Command> entry in transactionContext.EnlistedCommands)
						{
							Command command=entry.Value as Command;
							if(command is InterSorageLinkObjectsCommand||command is LinkObjectsCommand)
							{
								if((command as LinkCommand).RoleA==RoleB||
									(command as LinkCommand).RoleB==RoleB)
									return;
							}
						}


						PersistenceLayerRunTime.ObjectStorage DestinationObjectStorage=RoleB.ObjectStorage;
						if(DestinationObjectStorage==CommandInitiatorStorage)
							DestinationObjectStorage.CreateDeleteStorageInstanceCommand(RoleB.RealStorageInstanceRef,PersistenceLayer.DeleteOptions.TryToDelete);
					}
				}
			}
		}
	


		/// <MetaDataID>{E16E0BC8-F953-49FE-BDD4-AAADD96C2F82}</MetaDataID>
		public override void Execute()
		{
	
			MetaDataRepository.Association association=LinkInitiatorAssociationEnd.Association;
			MetaDataRepository.Roles commandInitiatorRole;

			if(LinkInitiatorAssociationEnd.IsRoleA)
				commandInitiatorRole=MetaDataRepository.Roles.RoleA;
			else
				commandInitiatorRole=MetaDataRepository.Roles.RoleB;


			if(RoleA.ObjectStorage==CommandInitiatorStorage)
			{
				if(RoleB.Class.HasReferentialIntegrity(association.RoleA as DotNetMetaDataRepository.AssociationEnd))
					RoleA.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
			}
			else
			{
				if(RoleA.Class.HasReferentialIntegrity(association.RoleB as DotNetMetaDataRepository.AssociationEnd))
					RoleB.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
			}

			if(association.LinkClass==null)
			{
				if(commandInitiatorRole==MetaDataRepository.Roles.RoleA)
				{
					if(RoleA.ObjectStorage==CommandInitiatorStorage)
					{
						if(association.RoleB.Navigable)
							RoleA.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(association.RoleB as DotNetMetaDataRepository.AssociationEnd),RoleB.MemoryInstance,true);
					}
				}
				else
				{
					if(RoleB.ObjectStorage==CommandInitiatorStorage)
					{
						if(association.RoleA.Navigable)
							RoleB.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(association.RoleA as DotNetMetaDataRepository.AssociationEnd),RoleA.MemoryInstance,true);
					}
				}
			}

		}
	}
}
