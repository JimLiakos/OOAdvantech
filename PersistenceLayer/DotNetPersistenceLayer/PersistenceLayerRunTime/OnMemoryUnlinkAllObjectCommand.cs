using System.Diagnostics;

namespace OOAdvantech.PersistenceLayerRunTime.Commands
{
    /// <MetaDataID>{65EF8648-2591-46B1-AE73-6287CDB0F71D}</MetaDataID>
    public abstract class OnMemoryUnlinkAllObjectCommand : UnlinkAllObjectCommand
    {
        /// <MetaDataID>{56f95ad5-7dd5-4084-9f5a-6c0b9b99b55e}</MetaDataID>
        public OnMemoryUnlinkAllObjectCommand(StorageInstanceAgent deletedStorageInstance)
            : base(deletedStorageInstance)
        {
        }

        /// <MetaDataID>{3085FDDA-561C-490F-9A21-20A1E02E832D}</MetaDataID>
        protected abstract RelResolver theResolver
        {
            get;
        }
        /// <MetaDataID>{C58BB877-2111-489B-B56E-1E7DC012FEBB}</MetaDataID>
        public override int ExecutionOrder
        {
            get
            {
                return 50;
            }
        }

        /// <MetaDataID>{AAEA5636-C048-4C02-AC08-AD1FC27632B3}</MetaDataID>
        private System.Collections.Generic.List<object> LinkedObjects;

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6B7C3318-CAD8-4E42-9352-0B428187CB09}</MetaDataID>
        protected internal bool SubTransactionCmdsProduced = false;
        /// <MetaDataID>{A427F5E5-5A6F-4B25-84D4-3B7DAE8F8C0B}</MetaDataID>
        public override void GetSubCommands(int currentExecutionOrder)
        {
            if (currentExecutionOrder < 40)
                return;
            if (!SubTransactionCmdsProduced)
            {
                SubTransactionCmdsProduced = true;

                if (theAssociationEnd.Navigable && theResolver.Owner.Class.IsCascadeDelete(theResolver.AssociationEnd))
                {
                    if (LinkedObjects == null)
                        LinkedObjects = theResolver.GetLinkedStorageInstanceRefsUnderTransaction(false);

                    System.Collections.Generic.Dictionary<object, object> objectsInLinkCommands = new System.Collections.Generic.Dictionary<object, object>();
                    PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
                    foreach (System.Collections.Generic.KeyValuePair<string, Commands.Command> entry in transactionContext.EnlistedCommands)
                    {
                        Command command = entry.Value as Command;
                        if (/*command is InterSorageLinkObjectsCommand||*/command is LinkObjectsCommand)
                        {
                            objectsInLinkCommands[(command as LinkCommand).RoleA.MemoryInstance] = (command as LinkCommand).RoleA;
                            objectsInLinkCommands[(command as LinkCommand).RoleB.MemoryInstance] = (command as LinkCommand).RoleB;
                        }
                    }
                    if (theResolver.Multilingual)
                    {
                        foreach (MultilingualObjectLink multilingualObjectLink in LinkedObjects)
                        {
                            if (theAssociationEnd.Association.LinkClass == null)
                            {
                                if (!objectsInLinkCommands.ContainsKey((multilingualObjectLink.LinkedObject as StorageInstanceRef).MemoryInstance))
                                {
                                    ((PersistenceLayerRunTime.ObjectStorage)(multilingualObjectLink.LinkedObject as StorageInstanceRef).ObjectStorage).CreateDeleteStorageInstanceCommand(multilingualObjectLink.LinkedObject as StorageInstanceRef, PersistenceLayer.DeleteOptions.TryToDelete);
                                }
                            }
                        }
                    }
                    else
                    {

                        foreach (StorageInstanceRef CurrStorageInstanceRef in LinkedObjects)
                        {
                            if (theAssociationEnd.Association.LinkClass == null)
                            {
                                if (!objectsInLinkCommands.ContainsKey(CurrStorageInstanceRef.MemoryInstance))
                                {
                                    ((PersistenceLayerRunTime.ObjectStorage)CurrStorageInstanceRef.ObjectStorage).CreateDeleteStorageInstanceCommand(CurrStorageInstanceRef, PersistenceLayer.DeleteOptions.TryToDelete);
                                }
                            }
                        }
                    }
                }
                #region All Associations with link class is by default cascade delete to relation object
                if (theAssociationEnd.Association.LinkClass != null)
                {
                    if (LinkedObjects == null)
                        LinkedObjects = theResolver.GetLinkedStorageInstanceRefsUnderTransaction(false);

                    foreach (StorageInstanceRef CurrStorageInstanceRef in LinkedObjects)
                        ((PersistenceLayerRunTime.ObjectStorage)CurrStorageInstanceRef.ObjectStorage).CreateDeleteStorageInstanceCommand(CurrStorageInstanceRef, PersistenceLayer.DeleteOptions.EnsureObjectDeletion);

                }
                #endregion

            }

        }

        ///// <MetaDataID>{53515775-5B4B-499D-B19A-D09323E544F1}</MetaDataID>
        //public void RemoveLinkFromOperativeObjects(System.Collections.ArrayList OperativeObjectsRef)
        //{

        //}

        /// <MetaDataID>{A327C703-5F73-4D42-964A-65E1DE683BE9}</MetaDataID>
        public override void Execute()
        {

            DotNetMetaDataRepository.AssociationEnd theOtherAssociationEnd = theResolver.AssociationEnd.GetOtherEnd() as DotNetMetaDataRepository.AssociationEnd;
            if (DeletedStorageInstance.Class.HasReferentialIntegrity(theResolver.AssociationEnd))
            {
                if (LinkedObjects == null)
                    LinkedObjects = theResolver.GetLinkedStorageInstanceRefsUnderTransaction(false);

                if (theResolver.Multilingual)
                {
                    foreach (MultilingualObjectLink multilingualObjectLink in LinkedObjects)
                    {
                        StorageInstanceRef storageInstanceRef = multilingualObjectLink.LinkedObject as StorageInstanceRef;
                        storageInstanceRef.ReferentialIntegrityLinkRemoved();
                    }
                }
                else
                {
                    foreach (StorageInstanceRef CurrStorageInstanceRef in LinkedObjects)
                        CurrStorageInstanceRef.ReferentialIntegrityLinkRemoved();
                }
            }

            {
                if (LinkedObjects == null)
                    LinkedObjects = theResolver.GetLinkedStorageInstanceRefsUnderTransaction(false);
                if (theResolver.Multilingual)
                {
                    if (theResolver.AssociationEnd.Association.LinkClass != null)
                    {
                        foreach (StorageInstanceRef currStorageInstanceRef in LinkedObjects)
                        {
                            StorageInstanceAgent roleA = null, roleB = null;
                            currStorageInstanceRef.GetRolesObject(ref roleA, ref roleB);
                            if (theResolver.AssociationEnd.IsRoleA)
                                roleA.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theOtherAssociationEnd), currStorageInstanceRef.MemoryInstance, true);
                            else
                                roleB.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theOtherAssociationEnd), currStorageInstanceRef.MemoryInstance, true);
                        }
                    }
                    else
                    {
                        foreach (MultilingualObjectLink multilingualObjectLink in LinkedObjects)
                        {
                            
                            {
                                StorageInstanceRef storageInstanceRef = multilingualObjectLink.LinkedObject as StorageInstanceRef;
                                using (CultureContext cultureContext = new CultureContext(multilingualObjectLink.Culture, false))
                                {
                                    if (theOtherAssociationEnd.Navigable)
                                        storageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theOtherAssociationEnd), DeletedStorageInstance.MemoryInstance, true);

                                    DeletedStorageInstance.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theResolver.AssociationEnd), storageInstanceRef.MemoryInstance, true);
                                }
                            }

                        }
                    }
                }
                else
                {

                    foreach (StorageInstanceRef currStorageInstanceRef in LinkedObjects)
                    {
                        if(currStorageInstanceRef==null)
                        {
                            Debug.Assert(currStorageInstanceRef==null);
                            continue;
                        }
                        if (theResolver.AssociationEnd.Association.LinkClass != null)
                        {
                            StorageInstanceAgent roleA = null, roleB = null;
                            currStorageInstanceRef.GetRolesObject(ref roleA, ref roleB);
                            if (theResolver.AssociationEnd.IsRoleA)
                                roleA.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theOtherAssociationEnd), currStorageInstanceRef.MemoryInstance, true);
                            else
                                roleB.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theOtherAssociationEnd), currStorageInstanceRef.MemoryInstance, true);

                        }
                        else
                        {
                            currStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theOtherAssociationEnd), DeletedStorageInstance.MemoryInstance, true);
                            DeletedStorageInstance.RealStorageInstanceRef.ClearObjectsLink(new AssociationEndAgent(theResolver.AssociationEnd), currStorageInstanceRef.MemoryInstance, true);


                        }
                    }
                }

            }
        }
    }
}
