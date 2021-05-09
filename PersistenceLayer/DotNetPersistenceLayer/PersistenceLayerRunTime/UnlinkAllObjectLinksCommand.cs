namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
	///<summary>
	///This class defines a command to storage. 
	///The job of objects of this class is to remove the links with the target objects. 
	///The type of relations defined from the association object relation.  
	///</summary>
	/// <MetaDataID>{6C2CDAC3-2CD0-4F9D-8065-00436B41340D}</MetaDataID>
	public abstract class UnlinkAllObjectCommand : Command 
	{
        /// <MetaDataID>{ded4c63c-82df-4263-9c0d-d9f78c972431}</MetaDataID>
        public UnlinkAllObjectCommand(StorageInstanceAgent deletedStorageInstance)
        {
            DeletedStorageInstance = deletedStorageInstance;
        }
		
		///<summary>
		///This member defines the type of links which will be removed. 
		///</summary>
		/// <MetaDataID>{6F277547-6781-4862-91E4-FF522FF7FE66}</MetaDataID>
		public MetaDataRepository.AssociationEnd theAssociationEnd;

		///<summary>
		///This member defines the source object of links. 
		///The links which will be removed must be type of association 
		///of association end and one from role object must be the source object  
		///</summary>
		/// <MetaDataID>{851B5A35-EFC6-4139-B0F2-815C06C41C68}</MetaDataID>
		public readonly StorageInstanceAgent DeletedStorageInstance;


		/// <MetaDataID>{7D147397-8597-4CC4-81A3-B4746C3DE610}</MetaDataID>
		public override string Identity
		{
			get
			{
                return "unlinkall" + DeletedStorageInstance.MemoryID.ToString() + DeletedStorageInstance.ValueTypePath.ToString() + theAssociationEnd.Identity;
			}
		}


        bool SubTransactionCmdsProduced;

        /// <MetaDataID>{253D0C92-5CE6-43E8-BC20-F08B4506A19C}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
		{

            if (currentExecutionOrder <= 10)
                return;

            if (!SubTransactionCmdsProduced)
            {

                SubTransactionCmdsProduced = true;
               
                if (DeletedStorageInstance.Class.IsCascadeDelete(theAssociationEnd as DotNetMetaDataRepository.AssociationEnd))
                {
                    //PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                    //foreach (System.Collections.Generic.KeyValuePair<string, Commands.Command> entry in transactionContext.EnlistedCommands)
                    //{
                    //    Command command = entry.Value as Command;
                    //    if (/*command is InterSorageLinkObjectsCommand||*/command is LinkObjectsCommand)
                    //    {
                    //        if ((command as LinkCommand).RoleA == RoleB ||
                    //            (command as LinkCommand).RoleB == RoleB)
                    //            return;
                    //    }
                    //}

                    //PersistenceLayerRunTime.ObjectStorage DestinationObjectStorage = RoleB.ObjectStorage;
                    //using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(RoleB.RealStorageInstanceRef))
                    //{

                    //    DestinationObjectStorage.CreateDeleteStorageInstanceCommand(RoleB.RealStorageInstanceRef, PersistenceLayer.DeleteOptions.TryToDelete);
                    //    stateTransition.Consistent = true;
                    //}
                }

                //if (RoleB.Class.IsCascadeDelete(LinkInitiatorAssociationEnd.Association.RoleA as DotNetMetaDataRepository.AssociationEnd))
                //{
                //    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                //    foreach (System.Collections.Generic.KeyValuePair<string, Commands.Command> entry in transactionContext.EnlistedCommands)
                //    {
                //        Command command = entry.Value as Command;
                //        if (/*command is InterSorageLinkObjectsCommand||*/command is LinkObjectsCommand)
                //        {
                //            if ((command as LinkCommand).RoleA == RoleB ||
                //                (command as LinkCommand).RoleB == RoleB)
                //                return;
                //        }
                //    }

                //    PersistenceLayerRunTime.ObjectStorage DestinationObjectStorage = RoleB.ObjectStorage;
                //    using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(RoleB.RealStorageInstanceRef))
                //    {

                //        DestinationObjectStorage.CreateDeleteStorageInstanceCommand(RoleA.RealStorageInstanceRef, PersistenceLayer.DeleteOptions.TryToDelete);
                //        stateTransition.Consistent = true;
                //    }
                //}
            }

        }

	}
}
