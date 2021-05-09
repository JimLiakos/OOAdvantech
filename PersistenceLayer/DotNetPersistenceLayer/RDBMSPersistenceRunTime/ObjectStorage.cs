using System;
using System.Collections.Generic;
using System.Linq;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.RDBMSPersistenceRunTime
{
    /// <MetaDataID>{fa245c26-1db6-491b-b883-fcb3c504b8d2}</MetaDataID>
    public abstract class ObjectStorage : PersistenceLayerRunTime.ObjectStorage
    {
        /// <MetaDataID>{5ab0ea6a-8bf9-4f73-9992-4fb42473066f}</MetaDataID>
        public override MetaDataRepository.ObjectQueryLanguage.DataLoader CreateDataLoader(DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
        {
            return new RDBMSPersistenceRunTime.DataLoader(dataNode, dataLoaderMetadata);
        }

        /// <exclude>Excluded</exclude>
        private OOAdvantech.PersistenceLayerRunTime.MemoryInstanceCollection _OperativeObjectCollections;

        /// <MetaDataID>{92428a9b-e4ff-45e0-86a5-49c980175ec8}</MetaDataID>
        public override OOAdvantech.PersistenceLayerRunTime.MemoryInstanceCollection OperativeObjectCollections
        {
            get
            {
                if (_OperativeObjectCollections == null)
                    _OperativeObjectCollections = new RDBMSPersistenceRunTime.MemoryInstanceCollection(this);
                return _OperativeObjectCollections;
            }
        }

        protected override void UpdateOperativeObjects()
        {
            
        }

        /// <MetaDataID>{0b251822-e211-4710-8c91-90327e148bdf}</MetaDataID>
        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
        {
            MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
            mStructureSet.Open(OQLStatement, parameters);
            return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);
        }

        #region Create commands
        /// <MetaDataID>{a5f4ded8-433e-4666-8261-a3f261594fd3}</MetaDataID>
        public override void CreateUnLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {

            PersistenceLayerRunTime.Commands.LinkCommand mUnLinkObjectsCommand = null;
            if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage != this)
                throw new System.ArgumentException("There isn't object from this storage");

            string cmdIdentity = PersistenceLayerRunTime.Commands.UnLinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            if (!PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command))
            {
                if (roleA.ObjectStorage == roleB.ObjectStorage && roleB.ObjectStorage == this)
                {

                    mUnLinkObjectsCommand = new Commands.UnLinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                }
                else
                {
                    throw new NotImplementedException();
                    //mUnLinkObjectsCommand=new Commands.InterSorageUnLinkObjectsCommand(roleA,roleB,relationObject,linkInitiatorAssociationEnd,this);
                }
                PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(mUnLinkObjectsCommand);
            }
        }

        public override void CreateUpdateLinkIndexCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent roleA, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent roleB, OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent relationObject, OOAdvantech.PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.UpdateLinkIndexCommand linkObjectsCommand = null;

            string cmdIdentity = Commands.UpdateLinkIndexCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command);

            linkObjectsCommand = command as PersistenceLayerRunTime.Commands.UpdateLinkIndexCommand;
            if (linkObjectsCommand == null)
            {

                linkObjectsCommand = new Commands.UpdateLinkIndexCommand(this, roleA, roleB,  linkInitiatorAssociationEnd, index);
                PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(linkObjectsCommand);
            }
            else
            {

                if (linkInitiatorAssociationEnd.RealAssociationEnd.IsRoleA)
                    linkObjectsCommand.RoleAIndex = index;
                else
                    linkObjectsCommand.RoleBIndex = index;
            }
        }

        /// <MetaDataID>{5C19603B-A3DE-427E-9EF9-56E5152AB358}</MetaDataID>
        public override void CreateLinkCommand(PersistenceLayerRunTime.StorageInstanceAgent roleA, PersistenceLayerRunTime.StorageInstanceAgent roleB, PersistenceLayerRunTime.StorageInstanceAgent relationObject, PersistenceLayerRunTime.AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            PersistenceLayerRunTime.Commands.LinkObjectsCommand linkObjectsCommand = null;

            string cmdIdentity = Commands.LinkObjectsCommand.GetIdentity(this, roleA, roleB, linkInitiatorAssociationEnd.RealAssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command);

            linkObjectsCommand = command as PersistenceLayerRunTime.Commands.LinkObjectsCommand;
            if (linkObjectsCommand == null)
            {

                linkObjectsCommand = new Commands.LinkObjectsCommand(this, roleA, roleB, relationObject, linkInitiatorAssociationEnd, index);
                PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(linkObjectsCommand);
            }
            else
            {

                if (linkInitiatorAssociationEnd.RealAssociationEnd.IsRoleA)
                    linkObjectsCommand.RoleAIndex = index;
                else
                    linkObjectsCommand.RoleBIndex = index;
            }
        }



        /// <MetaDataID>{0f857e9a-c689-404a-ad67-9f5c5e4d8149}</MetaDataID>
        public override void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection)
        {
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            RDBMSPersistenceRunTime.Commands.BuildContainedObjectIndicies buildContainedObjectIndicies = new RDBMSPersistenceRunTime.Commands.BuildContainedObjectIndicies(collection);
            transactionContext.EnlistCommand(buildContainedObjectIndicies);
        }


        /// <MetaDataID>{3D6C46B8-7EA3-4784-9B9F-5047CC506733}</MetaDataID>
        public override void CreateUpdateReferentialIntegrity(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {
            //TODO:Μεγάλο πρόβλημα αν το transaction έχει πολλές μεταβολές δηλαδή πολλά commands

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;

            Commands.UpdateReferentialIntegrity updateReferentialIntegrity = new Commands.UpdateReferentialIntegrity(storageInstanceRef as RDBMSPersistenceRunTime.StorageInstanceRef);
            //updateReferentialIntegrity.UpdatedStorageInstanceRef=storageInstanceRef;
            transactionContext.EnlistCommand(updateReferentialIntegrity);
        }

        /// <MetaDataID>{82ab93cd-98f5-4410-851b-281d04584711}</MetaDataID>
        public override OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, OOAdvantech.PersistenceLayer.ObjectID objectID, OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            return new RDBMSPersistenceRunTime.StorageInstanceRef(memoryInstance, storageCell, this, objectID);
        }

        /// <MetaDataID>{c998482c-401a-4867-b821-292298169677}</MetaDataID>
        public override PersistenceLayerRunTime.StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, OOAdvantech.PersistenceLayer.ObjectID objectID)
        {
            RDBMSMetaDataRepository.StorageCell storageCell = ((StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(MetaDataRepository.Classifier.GetClassifier(memoryInstance.GetType())) as RDBMSMetaDataRepository.Class).ActiveStorageCell;
            return new RDBMSPersistenceRunTime.StorageInstanceRef(memoryInstance, storageCell, this, objectID);
        }

        /// <MetaDataID>{7cbc10bc-836a-47e5-8392-e1e4c3e510c0}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceAgent sourceStorageInstance, DotNetMetaDataRepository.AssociationEnd associationEnd)
        {
            if (Remoting.RemotingServices.IsOutOfProcess(associationEnd))
                associationEnd = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(associationEnd.Identity) as DotNetMetaDataRepository.AssociationEnd;

            Commands.UnlinkAllObjectCommand mUnlinkAllObjectCommand = new Commands.UnlinkAllObjectCommand(sourceStorageInstance);
            //mUnlinkAllObjectCommand.DeletedStorageInstance=sourceStorageInstance;
            mUnlinkAllObjectCommand.theAssociationEnd = associationEnd;

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(mUnlinkAllObjectCommand);
        }



        /// <MetaDataID>{2efacb64-633d-4614-914a-05ec37b425b6}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceAgent DeletedOutStorageInstanceRef, MetaDataRepository.AssociationEnd AssociationEnd, MetaDataRepository.StorageCell linkedObjectsStorageCell)
        {
            RDBMSMetaDataRepository.StorageCell StorageCell = DeletedOutStorageInstanceRef.RealStorageInstanceRef.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell;
            if (Remoting.RemotingServices.IsOutOfProcess(AssociationEnd))
                AssociationEnd = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;

            RDBMSMetaDataRepository.AssociationEnd inStorageAssociationEnd = (_StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(AssociationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            RDBMSMetaDataRepository.Association inStorageAssociation = inStorageAssociationEnd.Association as RDBMSMetaDataRepository.Association;
            RDBMSMetaDataRepository.StorageCellsLink StorageCellsLink = null;
            if (AssociationEnd.IsRoleA)
                StorageCellsLink = inStorageAssociation.GetStorageCellsLink(StorageCell, linkedObjectsStorageCell as RDBMSMetaDataRepository.StorageCell, "");
            else
                StorageCellsLink = inStorageAssociation.GetStorageCellsLink(linkedObjectsStorageCell as RDBMSMetaDataRepository.StorageCell, StorageCell, "");
            //PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand Temp=new Commands.OutStorageUnlinkAllObjectCmd(StorageCellsLink,(StorageInstanceRef)DeletedOutStorageInstanceRef,inStorageAssociationEnd,this);
            PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand Temp = new Commands.UnlinkAllObjectCommand(DeletedOutStorageInstanceRef);

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;

            transactionContext.EnlistCommand(Temp);
        }


        /// <summary>UpdateStorageInstanceCommand </summary>
        /// <MetaDataID>{5129589a-b7fe-4cbb-9759-dbfaffa866e3}</MetaDataID>
        public override void CreateUpdateStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
        {

            Commands.UpdateStorageInstanceCommand updateStorageInstanceCommand = new Commands.UpdateStorageInstanceCommand(storageInstanceRef);

            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            transactionContext.EnlistCommand(updateStorageInstanceCommand);
        }


        /// <MetaDataID>{326f6792-38ea-40ba-abff-cd0a94557e45}</MetaDataID>
        public override void CreateNewStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef StorageInstance)
        {
            Commands.NewStorageInstanceCommand newStorageInstanceCommand = new Commands.NewStorageInstanceCommand(StorageInstance as RDBMSPersistenceRunTime.StorageInstanceRef);
            PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistCommand(newStorageInstanceCommand);
        }


        /// <MetaDataID>{ec8927cc-2ed7-4673-a721-1d449b5cb52a}</MetaDataID>
        public override void CreateDeleteStorageInstanceCommand(PersistenceLayerRunTime.StorageInstanceRef storageInstance, PersistenceLayer.DeleteOptions deleteOption)
        {
            Commands.DeleteStorageInstanceCommand Command = new Commands.DeleteStorageInstanceCommand(storageInstance, deleteOption);
            PersistenceLayerRunTime.ITransactionContext transactionContext = PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext;
            if (!transactionContext.ContainCommand(Command.Identity))
                transactionContext.EnlistCommand(Command);
        }

        #endregion


        /// <MetaDataID>{638c8bea-335f-45b2-9d89-05cf981ca7fa}</MetaDataID>
        public override Collections.StructureSet Execute(string OQLStatement)
        {
            using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                MetaDataRepository.ObjectQueryLanguage.StructureSet mStructureSet = new MetaDataRepository.ObjectQueryLanguage.StructureSet(this);
                mStructureSet.Open(OQLStatement, null);
                //mStructureSet.GetData();
                stateTransition.Consistent = true;
                return new MetaDataRepository.ObjectQueryLanguage.StructureSetAgent(mStructureSet);
            }

        }



        /// <MetaDataID>{36062295-a252-4579-b579-18dd7205ae00}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = null;
            if (classifier is MetaDataRepository.Class)
            {
                storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                MetaDataRepository.Class storageClass = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Class)) as MetaDataRepository.Class;
                storageCells.AddRange( storageClass.StorageCellsOfThisType);
            }
            else if (classifier is MetaDataRepository.Interface)
            {
                storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                MetaDataRepository.Interface storageInterface = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(classifier.Identity.ToString(), typeof(MetaDataRepository.Interface)) as MetaDataRepository.Interface;
                storageCells.AddRange(  storageInterface.StorageCellsOfThisType);
            }
            if(storageCells==null)
                throw new Exception("The method or operation is implemented for classes and interfaces.");
            foreach (var storageReference in (StorageMetaData as MetaDataRepository.Storage).LinkedStorages)
            {
                var openStorage = storageReference.OpenObjectSorage();
                if(openStorage!=this)
                    storageCells.AddRange(storageReference.OpenObjectSorage().GetStorageCells(classifier));
            }

            return storageCells;
        }

        /// <MetaDataID>{af7fe93f-cd73-4f88-8bde-b0ddca4dd759}</MetaDataID>
        public override string GetPersistentObjectUri(object obj)
        {
            if (obj is StorageInstanceRef)
            {
                StorageInstanceRef storageInstanceRef = obj as StorageInstanceRef;
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            else if (PersistencyService.ClassOfObjectIsPersistent(obj))
            {
                PersistenceLayer.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(obj);
                if (storageInstanceRef != null && storageInstanceRef.PersistentObjectID != null)
                    return _StorageMetaData.StorageIdentity + "\\" + storageInstanceRef.PersistentObjectID.ToString();
            }
            return null;
        }

        /// <MetaDataID>{be429673-277f-40d6-859b-14b6eeabf84b}</MetaDataID>
        public override PersistenceLayer.ObjectID GetTemporaryObjectID()
        {
            return new OOAdvantech.RDBMSPersistenceRunTime.ObjectID(System.Guid.NewGuid(), 0);
        }

        /// <MetaDataID>{7b0963e0-8d9c-4762-94d8-cb4fe2e0586e}</MetaDataID>
        protected override OOAdvantech.MetaDataRepository.StorageCellReference GetStorageCellReference(OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            foreach (var ownedElement in (_StorageMetaData as Storage).OwnedElements)
            {
                if (ownedElement is MetaDataRepository.StorageCellReference)
                {
                    if ((ownedElement as MetaDataRepository.StorageCellReference).StorageIdentity == storageCell.StorageIdentity &&
                        (ownedElement as MetaDataRepository.StorageCellReference).SerialNumber == storageCell.SerialNumber)
                        return ownedElement as MetaDataRepository.StorageCellReference;
                }
            }

            return null;
        }

        /// <MetaDataID>{c6f756ad-892f-4ba1-ae2f-de75a420f111}</MetaDataID>
        public void ActivateObjectsInPreoadedStorageCells()
        {
            //throw new NotImplementedException();
        }
        /// <MetaDataID>{d56444b8-5910-4a29-8d83-c1bc518ac8dc}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(int storageCellSerialNumber)
        {

            PersistenceLayer.ObjectStorage ObjectStorage = PersistenceLayer.ObjectStorage.OpenStorage(StorageMetaData.StorageName, StorageMetaData.StorageLocation, StorageMetaData.StorageType);
            string Query = "SELECT StorageCell FROM " + typeof(RDBMSMetaDataRepository.StorageCell).FullName + " StorageCell WHERE StorageCell.SerialNumber = " + storageCellSerialNumber.ToString();

            Collections.StructureSet aStructureSet = PersistenceLayer.ObjectStorage.GetStorage(ObjectStorage.StorageMetaData).Execute(Query);
            foreach (Collections.StructureSet Rowset in aStructureSet)
            {
                //TODO: να τσεκαριστή εάν υπάρχει συμβατότητα μεταξύ της class που τρέχει τοπικά
                //και αυτής που τρέχει remotely
                //WHERE oid = "+StorageCellID.ToString();
                RDBMSMetaDataRepository.StorageCell storageCell = (RDBMSMetaDataRepository.StorageCell)Rowset["StorageCell"];
                return storageCell;
            }
            return null;
        }

        /// <MetaDataID>{47cc827a-9e15-4588-bc47-639ae3d11ba1}</MetaDataID>
        public ObjectStorage(RDBMSPersistenceRunTime.Storage theStorageMetaData)
        { 
            SetObjectStorage(theStorageMetaData);
        }
        /// <MetaDataID>{b90826e3-d97e-44d7-a30f-d1ba6b560f06}</MetaDataID>
        void SetObjectStorage(RDBMSPersistenceRunTime.Storage theStorageMetaData)
        {

            if (_StorageMetaData != null)
                throw new System.Exception("StorageMetaData already set");
            _StorageMetaData = theStorageMetaData;
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{233FD07F-F3C3-4CC9-8818-9BE35E73E27E}</MetaDataID>
        protected OOAdvantech.PersistenceLayer.Storage _StorageMetaData;
        /// <MetaDataID>{BD41260F-65BB-4804-B762-4CA92838AB9D}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.Storage StorageMetaData
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _StorageMetaData;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }

        /// <MetaDataID>{05cf5580-cdf2-43a8-b932-527851f6b820}</MetaDataID>
        public RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get
            {
                return ((StorageMetaData as RDBMSPersistenceRunTime.Storage).MetadataStorage as RDBMSMetaDataPersistenceRunTime.AdoNetObjectStorage).TypeDictionary;
            }
        }


         

        /// <MetaDataID>{294e731a-5354-48f0-b6d9-5d4b9657a4c6}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery DistributeObjectQuery(Guid queryIdentity,OOAdvantech.Collections.Generic.List<DataNode> dataTrees, QueryResultType queryResult, OOAdvantech.Collections.Generic.List<DataNode> selectListItems, OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoaderMetadata> dataLoadersMetadata, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters, List<string> usedAliases, List<string> queryStorageIdentities)
        {
            OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery objectQuery = new DistributedObjectQuery(queryIdentity, dataTrees, queryResult, selectListItems, this, dataLoadersMetadata, parameters, usedAliases, queryStorageIdentities);
            return objectQuery;
        }


        /// <MetaDataID>{f689f527-e26f-4e2e-a689-980f90292a79}</MetaDataID>
        internal protected abstract void TransferOnTemporaryTableRecords(IDataTable dataTable);
        /// <MetaDataID>{0840dac4-fc79-40f1-bb2f-51347ceadf77}</MetaDataID>
        internal protected abstract void TransferTableRecords(IDataTable dataTable,RDBMSMetaDataRepository.Table tableMetadata);
        /// <MetaDataID>{54be16a7-22f6-48fe-b4eb-1b32c0e7b61b}</MetaDataID>
        internal protected abstract void UpdateTableRecords(IDataTable dataTable, List<string> OIDColumns);
        /// <MetaDataID>{7e5bc390-9a7e-482f-b8d5-97b856d0d510}</MetaDataID>
        internal protected abstract void DeleteTableRecords(IDataTable dataTable);

        /// <MetaDataID>{ec1d2e8e-95de-4ef4-90cd-e17e286226b7}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ValueTypePath valueTypePath, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> rootStorageCells, string ofTypeIdentity = null)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                List<MetaDataRepository.StorageCellsLink> storageCellsLinks = new List<OOAdvantech.MetaDataRepository.StorageCellsLink>();
                Dictionary<MetaDataRepository.StorageCell, MetaDataRepository.RelatedStorageCell> linkedStorageCells = new Dictionary<OOAdvantech.MetaDataRepository.StorageCell, OOAdvantech.MetaDataRepository.RelatedStorageCell>();
                foreach (OOAdvantech.MetaDataRepository.RelatedStorageCell relatedStorageCell in base.GetLinkedStorageCellsFromObjectsUnderTransaction(associationEnd, valueTypePath, rootStorageCells))
                {
                    if (!linkedStorageCells.ContainsKey(relatedStorageCell.StorageCell))
                    {
                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                        {
                            if (relatedStorageCell.StorageCell.IsTypeOf(ofTypeIdentity))
                                linkedStorageCells.Add(relatedStorageCell.StorageCell, relatedStorageCell);
                        }
                        else
                            linkedStorageCells.Add(relatedStorageCell.StorageCell, relatedStorageCell);
                    }
                }
                associationEnd = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(associationEnd.Identity.ToString(), typeof(MetaDataRepository.AssociationEnd)) as RDBMSMetaDataRepository.AssociationEnd;
                foreach (MetaDataRepository.StorageCell rootStorageCell in rootStorageCells)
                {
                    if (rootStorageCell is MetaDataRepository.StorageCellReference)
                        continue;
                    bool withRelationTable = false;
                    if (associationEnd != null &&
                      (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                      associationEnd.Association.Specializations.Count > 0) &&
                      associationEnd.Association != rootStorageCell.Type.ClassHierarchyLinkAssociation)
                    {
                        withRelationTable = true;
                    }

                    RDBMSMetaDataRepository.Association storageAssociation = associationEnd.Association as RDBMSMetaDataRepository.Association;
                    foreach (MetaDataRepository.StorageCellsLink storageCellsLink in storageAssociation.HierarchyStorageCellsLinks)
                    {
                        if (storageCellsLinks.Contains(storageCellsLink)||valueTypePath.ToString()!=storageCellsLink.ValueTypePath)
                            continue;

                        bool hasRelationTable = (storageCellsLink as RDBMSMetaDataRepository.StorageCellsLink).ObjectLinksTable != null | withRelationTable;

                        #region relation through association class
                        if (storageCellsLink.AssotiationClassStorageCells.Contains(rootStorageCell))
                        {
                            storageCellsLinks.Add(storageCellsLink);
                            if (associationEnd.Role == MetaDataRepository.Roles.RoleA)
                            {
                                if (storageCellsLink.RoleAStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                                {
                                    if (!linkedStorageCells.ContainsKey((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell))
                                        linkedStorageCells.Add((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell,storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                }
                                else
                                {
                                    if(!linkedStorageCells.ContainsKey(storageCellsLink.RoleAStorageCell))
                                        linkedStorageCells.Add(storageCellsLink.RoleAStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleAStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                }
                            }
                            else
                            {

                                if (storageCellsLink.RoleBStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                                {
                                    if (!linkedStorageCells.ContainsKey((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell))
                                        linkedStorageCells.Add((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                }
                                else
                                {
                                    if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleBStorageCell))
                                        linkedStorageCells.Add(storageCellsLink.RoleBStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleBStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                }
                            }
                        }
                        #endregion

                        if (associationEnd.Role == MetaDataRepository.Roles.RoleA && storageCellsLink.RoleBStorageCell == rootStorageCell)
                        {
                            storageCellsLinks.Add(storageCellsLink);
                            if (storageCellsLink.RoleAStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                            {
                                if (!linkedStorageCells.ContainsKey((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell))
                                {
                                    if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                    {
                                        if (storageCellsLink.RoleAStorageCell.IsTypeOf(ofTypeIdentity))
                                            linkedStorageCells.Add((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                    }
                                    else
                                        linkedStorageCells.Add((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleAStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));

                                }
                                if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleAStorageCell))
                                {
                                    if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                    {
                                        if (storageCellsLink.RoleAStorageCell.IsTypeOf(ofTypeIdentity))
                                            linkedStorageCells.Add(storageCellsLink.RoleAStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleAStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                    }
                                    else
                                        linkedStorageCells.Add(storageCellsLink.RoleAStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleAStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));

                                }
                            }
                            else
                            {
                                if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleAStorageCell))
                                {
                                    if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                    {
                                        if (storageCellsLink.RoleAStorageCell.IsTypeOf(ofTypeIdentity))
                                            linkedStorageCells.Add(storageCellsLink.RoleAStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleAStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                    }
                                    else
                                        linkedStorageCells.Add(storageCellsLink.RoleAStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleAStorageCell, rootStorageCell, storageCellsLink.Type.RoleA.Identity.ToString(), hasRelationTable));
                                }
                            }
                        }
                        else
                        {
                            if (storageCellsLink.RoleAStorageCell == rootStorageCell)
                            {
                                storageCellsLinks.Add(storageCellsLink);
                                if (storageCellsLink.RoleBStorageCell is RDBMSMetaDataRepository.StorageCellReference)
                                {
                                    if (!linkedStorageCells.ContainsKey((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell))
                                    {
                                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                        {
                                            if (storageCellsLink.RoleBStorageCell.IsTypeOf(ofTypeIdentity))
                                                linkedStorageCells.Add((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                        }
                                        else
                                            linkedStorageCells.Add((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, new MetaDataRepository.RelatedStorageCell((storageCellsLink.RoleBStorageCell as RDBMSMetaDataRepository.StorageCellReference).RealStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));

                                    }
                                    if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleBStorageCell))
                                    {
                                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                        {
                                            if (storageCellsLink.RoleBStorageCell.IsTypeOf(ofTypeIdentity))
                                                linkedStorageCells.Add(storageCellsLink.RoleBStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleBStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                        }
                                        else
                                            linkedStorageCells.Add(storageCellsLink.RoleBStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleBStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                    }
                                }
                                else
                                {
                                    if (!linkedStorageCells.ContainsKey(storageCellsLink.RoleBStorageCell))
                                    {
                                        if (!string.IsNullOrWhiteSpace(ofTypeIdentity))
                                        {
                                            if (storageCellsLink.RoleBStorageCell.IsTypeOf(ofTypeIdentity))
                                                linkedStorageCells.Add(storageCellsLink.RoleBStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleBStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));
                                        }
                                        else
                                            linkedStorageCells.Add(storageCellsLink.RoleBStorageCell, new MetaDataRepository.RelatedStorageCell(storageCellsLink.RoleBStorageCell, rootStorageCell, storageCellsLink.Type.RoleB.Identity.ToString(), hasRelationTable));

                                    }
                                }
                            }
                        }
                    }
                }
                return new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell>(linkedStorageCells.Values);
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }

        }


        /// <MetaDataID>{d5a776c2-f4b2-4b1a-b21a-197df5070f33}</MetaDataID>
        public override Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCells, MetaDataRepository.Roles storageCellsRole, string ofTypeIdentity = null)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                association = (StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(association) as OOAdvantech.MetaDataRepository.Association;
                Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> StorageCells = new Collections.Generic.Set<MetaDataRepository.RelatedStorageCell>();
                foreach (MetaDataRepository.StorageCell storageCell in relatedStorageCells)
                {
                    foreach (MetaDataRepository.StorageCellsLink storageCellsLink in association.StorageCellsLinks)
                    {
                        if (storageCellsRole == MetaDataRepository.Roles.RoleA && storageCellsLink.RoleAStorageCell == storageCell)
                        {
                            StorageCells.AddRange( (from assotiationClassStorageCell in storageCellsLink.AssotiationClassStorageCells
                                                    select new MetaDataRepository.RelatedStorageCell(assotiationClassStorageCell, storageCell,association.RoleA.Identity.ToString(),false)).ToList());
                        }
                        else
                        {
                            if (storageCellsLink.RoleBStorageCell == storageCell)
                                StorageCells.AddRange((from assotiationClassStorageCell in storageCellsLink.AssotiationClassStorageCells
                                                       select new MetaDataRepository.RelatedStorageCell(assotiationClassStorageCell, storageCell, association.RoleB.Identity.ToString(), false)).ToList());
                        }
                    }
                }
                return StorageCells;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }

        /// <MetaDataID>{eaa67188-097a-49e9-95be-9dbdaac78772}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(OOAdvantech.PersistenceLayerRunTime.StorageInstanceAgent storageInstanceAgent)
        {
            if (storageInstanceAgent.StorageIdentity == StorageMetaData.StorageIdentity)
                return storageInstanceAgent.RealStorageInstanceRef.StorageInstanceSet;
            RDBMSMetaDataRepository.Class _class = (StorageMetaData as Storage).GetEquivalentMetaObject(storageInstanceAgent.Class) as RDBMSMetaDataRepository.Class;

            return _class.GetStorageCellReference(storageInstanceAgent.StorageCellName, 
                                            storageInstanceAgent.StorageCellSerialNumber, 
                                            storageInstanceAgent.StorageName, 
                                            storageInstanceAgent.ObjectIdentityType,
                                            storageInstanceAgent.StorageIdentity,
                                            storageInstanceAgent.StorageLocation, 
                                            storageInstanceAgent.StorageType);


        }



     
    }
}
