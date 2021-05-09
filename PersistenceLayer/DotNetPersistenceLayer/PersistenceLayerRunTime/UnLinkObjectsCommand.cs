namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
    /// <MetaDataID>{E0A3C621-FA72-42B1-B242-8BF52EC08754}</MetaDataID>
    /// <summary></summary>
    public abstract class UnLinkObjectsCommand : LinkCommand
    {
        protected bool Multilingual;
        protected System.Globalization.CultureInfo Culture;

        /// <MetaDataID>{5E4659DD-0083-4616-BA6B-4BEFE593CCFE}</MetaDataID>
        public override string Identity
        {
            get
            {

                if (IsMultilingualLink(RoleA, RoleB, this.LinkInitiatorAssociationEnd.Association as DotNetMetaDataRepository.Association))
                    return "mlunlink_" + CultureContext.CurrentNeutralCultureInfo.Name + "_" + ObjectStorage.StorageMetaData.StorageIdentity + RoleA.MemoryID.ToString() + RoleA.ValueTypePath.ToString() + RoleB.MemoryID.ToString() + RoleB.ValueTypePath.ToString() + this.LinkInitiatorAssociationEnd.Association.Identity;
                else
                    return "unlink" + ObjectStorage.StorageMetaData.StorageIdentity + RoleA.MemoryID.ToString() + RoleA.ValueTypePath.ToString() + RoleB.MemoryID.ToString() + RoleB.ValueTypePath.ToString() + this.LinkInitiatorAssociationEnd.Association.Identity;
            }
        }

        /// <MetaDataID>{846A486B-CBCA-47A5-A8E9-97D6ED6CA0A0}</MetaDataID>
        public static string GetIdentity(PersistenceLayerRunTime.ObjectStorage objectStorage, StorageInstanceAgent roleA, StorageInstanceAgent roleB, DotNetMetaDataRepository.Association association)
        {
            if (IsMultilingualLink(roleA, roleB, association))
                return "mlunlink_" + CultureContext.CurrentNeutralCultureInfo.Name + "_" + objectStorage.StorageMetaData.StorageIdentity + roleA.MemoryID.ToString() + roleA.ValueTypePath.ToString() + roleB.MemoryID.ToString() + roleB.ValueTypePath.ToString() + association.Identity;
            else
                return "unlink" + objectStorage.StorageMetaData.StorageIdentity + roleA.MemoryID.ToString() + roleA.ValueTypePath.ToString() + roleB.MemoryID.ToString() + roleB.ValueTypePath.ToString() + association.Identity;

        }




        /// <MetaDataID>{2BA6604E-B843-49AC-82E3-5410CEA71A40}</MetaDataID>
        private bool SubTransactionCmdsProduced;
        /// <MetaDataID>{E98B31C9-057A-4081-A62A-1EE4A87E3B2A}</MetaDataID>
        public UnLinkObjectsCommand(ObjectStorage objectStorage, StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index) :
            base(objectStorage, roleA, roleB, relationObject, linkInitiatorAssociationEnd)
        {
            SubTransactionCmdsProduced = false;
            if (linkInitiatorAssociationEnd.RealAssociationEnd.Indexer)
                if (linkInitiatorAssociationEnd.RealAssociationEnd.IsRoleA)
                    RoleAIndex = index;
                else
                    RoleBIndex = index;

            if (linkInitiatorAssociationEnd.RealAssociationEnd.GetOtherEnd().Indexer)
                if (linkInitiatorAssociationEnd.RealAssociationEnd.GetOtherEnd().IsRoleA)
                    RoleAIndex = index;
                else
                    RoleBIndex = index;
        }
        /// <MetaDataID>{2d009324-8b1b-4aa4-af4b-df9a948ea5db}</MetaDataID>
        internal protected int RoleAIndex = -1;
        /// <MetaDataID>{f9a354cd-5bc7-4b37-bb20-3af3af99a3f3}</MetaDataID>
        internal protected int RoleBIndex = -1;



        /// <MetaDataID>{90258763-09E7-45C8-B999-24C11669DABA}</MetaDataID>
        public override void Execute()
        {
            #region Preconditions Chechk
            if (RoleA == null || RoleB == null)
                throw (new System.Exception("You must set the objects that will be linked before the execution of command."));//Message
                                                                                                                              //if(theResolver==null)
                                                                                                                              //	throw (new System.Exception("The metadata of the command isn't set correctly."));//Message
            #endregion


            DotNetMetaDataRepository.AssociationEnd theOtherAssociationEnd = LinkInitiatorAssociationEnd.GetOtherEnd() as DotNetMetaDataRepository.AssociationEnd;
            MetaDataRepository.Association association = LinkInitiatorAssociationEnd.Association;

            //         if (LinkInitiatorAssociationEnd.IsRoleA)
            //{
            //	if(RoleB.Class.HasReferentialIntegrity(LinkInitiatorAssociationEnd as DotNetMetaDataRepository.AssociationEnd))
            //		RoleA.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
            //	if(RoleA.Class.HasReferentialIntegrity(theOtherAssociationEnd))
            //		RoleB.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
            //}
            //else
            //{
            //	if(RoleA.Class.HasReferentialIntegrity(LinkInitiatorAssociationEnd as DotNetMetaDataRepository.AssociationEnd))
            //		RoleB.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
            //	if(RoleB.Class.HasReferentialIntegrity(theOtherAssociationEnd))
            //		RoleA.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
            //}


            if (RoleB.Class.HasReferentialIntegrity(association.RoleA as DotNetMetaDataRepository.AssociationEnd))
            {
                if (RoleA.StorageIdentity == ObjectStorage.StorageIdentity)
                    RoleA.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
            }
            if (RoleA.Class.HasReferentialIntegrity(association.RoleB as DotNetMetaDataRepository.AssociationEnd))
            {
                if (RoleB.StorageIdentity == ObjectStorage.StorageIdentity)
                    RoleB.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
            }

            if (theOtherAssociationEnd.Navigable)
            {

                //check for navigable

                if (LinkInitiatorAssociationEnd.IsRoleA)
                {
                    if (LinkInitiatorAssociationEnd.Association.LinkClass == null)
                        RoleA.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theOtherAssociationEnd), RoleB.MemoryInstance, true);
                }
                else
                {
                    if (LinkInitiatorAssociationEnd.Association.LinkClass == null)
                        RoleB.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theOtherAssociationEnd), RoleA.MemoryInstance, true);
                }
            }
        }

        /// <MetaDataID>{5CC14AB1-703B-4406-8A7B-D5BAA7E0D83A}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {
            if (currentExecutionOrder <= 10)
                return;

            if (!SubTransactionCmdsProduced)
            {

                SubTransactionCmdsProduced = true;
                //TODO: Αυτός ο τρόπος που γινεται το cascade delete κάνει πολλές interpocess call
                //μια για να πάρει την class, μια για το IsCascadeDelete, μια για την ActiveStorageSession και
                //μια για το CreateDeleteStorageInstanceCommand σύνολο 5.
                //Θα μπορούσε και με μία κλήση αν προσθέσουμε μια method TryCascadeDelete στο StorageInstanceRef
                //TODO: Να ελεγχθεί όταν το cascade delete θα πρέπει να ακυρωθεί απο μια interstorage link

                //if(LinkInitiatorAssociationEnd.IsRoleA&&RoleB.Class.IsCascadeDelete(LinkInitiatorAssociationEnd as DotNetMetaDataRepository.AssociationEnd))
                //{
                //	PersistenceLayerRunTime.ITransactionContext transactionContext =PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                //	foreach(System.Collections.Generic.KeyValuePair<string,Commands.Command> entry in transactionContext.EnlistedCommands)
                //	{
                //		Command command=entry.Value as Command;
                //		if(/*command is InterSorageLinkObjectsCommand||*/command is LinkObjectsCommand)
                //		{
                //			if((command as LinkCommand).RoleA==RoleA||
                //				(command as LinkCommand).RoleB==RoleA)
                //				return;
                //		}
                //	}

                //	PersistenceLayerRunTime.ObjectStorage DestinationObjectStorage=RoleA.ObjectStorage;
                //                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(RoleA.RealStorageInstanceRef))
                //                {
                //                    DestinationObjectStorage.CreateDeleteStorageInstanceCommand(RoleA.RealStorageInstanceRef, PersistenceLayer.DeleteOptions.TryToDelete);
                //                    stateTransition.Consistent = true;
                //                }

                //}
                if (RoleA.Class.IsCascadeDelete(LinkInitiatorAssociationEnd.Association.RoleB as DotNetMetaDataRepository.AssociationEnd))
                {
                    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                    foreach (System.Collections.Generic.KeyValuePair<string, Commands.Command> entry in transactionContext.EnlistedCommands)
                    {
                        Command command = entry.Value as Command;
                        if (/*command is InterSorageLinkObjectsCommand||*/command is LinkObjectsCommand)
                        {
                            if ((command as LinkCommand).RoleA == RoleB ||
                                (command as LinkCommand).RoleB == RoleB)
                                return;
                        }
                    }

                    PersistenceLayerRunTime.ObjectStorage DestinationObjectStorage = RoleB.ObjectStorage;
                    using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(RoleB.RealStorageInstanceRef))
                    {

                        DestinationObjectStorage.CreateDeleteStorageInstanceCommand(RoleB.RealStorageInstanceRef, PersistenceLayer.DeleteOptions.TryToDelete);
                        stateTransition.Consistent = true;
                    }
                }

                if (RoleB.Class.IsCascadeDelete(LinkInitiatorAssociationEnd.Association.RoleA as DotNetMetaDataRepository.AssociationEnd))
                {
                    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                    foreach (System.Collections.Generic.KeyValuePair<string, Commands.Command> entry in transactionContext.EnlistedCommands)
                    {
                        Command command = entry.Value as Command;
                        if (/*command is InterSorageLinkObjectsCommand||*/command is LinkObjectsCommand)
                        {
                            if ((command as LinkCommand).RoleA == RoleA ||
                                (command as LinkCommand).RoleB == RoleA)
                                return;
                        }
                    }

                    PersistenceLayerRunTime.ObjectStorage DestinationObjectStorage = RoleA.ObjectStorage;
                    using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(RoleA.RealStorageInstanceRef))
                    {

                        DestinationObjectStorage.CreateDeleteStorageInstanceCommand(RoleA.RealStorageInstanceRef, PersistenceLayer.DeleteOptions.TryToDelete);
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{6DF60187-11A5-4789-B95B-DC8DCF78AF55}</MetaDataID>
        public override int ExecutionOrder
        {
            get
            {
                return 50;
            }
        }
    }
}


