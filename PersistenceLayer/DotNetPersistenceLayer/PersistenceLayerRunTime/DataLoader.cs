using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.PersistenceLayerRunTime
{
    using RelationPartIdentity = System.String;
    using OOAdvantech.MetaDataRepository;
    /// <summary>
    /// StorageDataLoader at this layer of abstraction implement the operation of objects link initialization. 
    /// An object link represents an object reference stored as value of member in object or as entry in a collection.
    /// </summary>
    /// <MetaDataID>{fbcf0b91-5d2b-4e5b-b622-a9a646f9297f}</MetaDataID>
    abstract public class StorageDataLoader : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.StorageDataLoader
    {
        /// <MetaDataID>{d7d6d5d9-2b09-4874-a1cd-5412a7f341e7}</MetaDataID>
        public System.Collections.Generic.List<string> QueryStorageIdentities
        {
            get
            {  
                if (DataNode.ObjectQuery is MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery)
                    return (DataNode.ObjectQuery as MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery).QueryStorageIdentities;
                throw new System.NotSupportedException();
            }
        }
        /// <MetaDataID>{43c38d7f-95bf-4681-b1c4-2293c087c5c4}</MetaDataID>
        public StorageDataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode, dataLoaderMetadata)
        {

        }
        Parser.Parser par;
   
        #region Objects under transaction

        ///<summary>
        ///Defines all storage objects under the transaction
        ///</summary>
        /// <MetaDataID>{329846f1-65d4-48e4-b232-b05f5ed5dd0b}</MetaDataID>
        public System.Collections.Generic.Dictionary<OOAdvantech.PersistenceLayer.ObjectID, StorageInstanceRef> ObjectUnderTransaction = new Dictionary<OOAdvantech.PersistenceLayer.ObjectID, StorageInstanceRef>();
        /// <MetaDataID>{dec758bf-bd4c-43bc-9c1e-3e093c5a148c}</MetaDataID>
        public object GetObjectUnderTransaction(OOAdvantech.PersistenceLayer.ObjectID objectID)
        {
            if (DataNode.ValueTypePath.Count > 0)
                return (GetDataLoader(DataNode.ParentDataNode) as StorageDataLoader).GetObjectUnderTransaction(objectID);
            StorageInstanceRef storageInstanceRef = null;
            ObjectUnderTransaction.TryGetValue(objectID, out storageInstanceRef);
            if (storageInstanceRef != null)
                return storageInstanceRef.MemoryInstance;
            else
                return null;

        }

        /// <MetaDataID>{96fbfc8a-59ba-41c1-9970-2b99df47e317}</MetaDataID>
        List<MetaDataRepository.StorageCell> _StorageCellOfObjectUnderTransaction = new List<OOAdvantech.MetaDataRepository.StorageCell>();
        /// <MetaDataID>{2e4b56c6-35a9-47a1-9727-d89395d0e881}</MetaDataID>
        protected List<MetaDataRepository.StorageCell> StorageCellOfObjectUnderTransaction
        {
            get
            {
                if (_UpdatedObjects == null)
                    LoadUpdatedObjects();
                if (_NewObjects == null)
                    LoadNewObjectsCollection();
                if (_CandidateForDeleteObjects == null)
                    LoadCandidateForDeleteObjects();
                return _StorageCellOfObjectUnderTransaction;
            }
        }

        /// <MetaDataID>{553533c3-679f-4de3-89cf-b3f4f475a396}</MetaDataID>
        List<StorageInstanceRef> _NewObjects;
        /// <MetaDataID>{1736b081-504b-4faa-8bde-3e0b38fee441}</MetaDataID>
        public System.Collections.Generic.List<StorageInstanceRef> NewObjects
        {
            get
            {
                if (DataLoaderMetadata.MemoryCell != null)
                    return new List<StorageInstanceRef>();

                if (DataNode.Type==MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Object&& DataNode.Classifier == null)
                    throw new System.Exception("Invalid DataLoader Metadata the DataNode Classifier is null"); 
                if (_NewObjects == null)
                    LoadNewObjectsCollection();
                return _NewObjects;
            }
        }


        //public override bool HasTransientObjects
        //{
        //    get 
        //    {
        //        return NewObjects.Count > 0;
        //    }
        //}





        /// <MetaDataID>{8a3f6eb4-12e3-4a95-aff7-554a972a9150}</MetaDataID>
        private void LoadNewObjectsCollection()
        {
            System.Collections.Generic.List<PersistenceLayerRunTime.StorageInstanceRef> candidateForDeleteObjects = CandidateForDeleteObjects;
            _NewObjects = new List<StorageInstanceRef>();
            foreach (StorageInstanceRef storageInstanceRef in StorageInstanceRef.GetNewObjectsUnderTransaction(DataNode.Classifier))
            {
                if (storageInstanceRef.ObjectStorage.StorageMetaData.StorageIdentity == DataLoaderMetadata.ObjectsContextIdentity)
                {
                    if (!candidateForDeleteObjects.Contains(storageInstanceRef))
                    {
                        if (!_NewObjects.Contains(storageInstanceRef))
                        {
                            if (storageInstanceRef.ObjectID == null)
                                storageInstanceRef.ObjectID = (ObjectStorage as PersistenceLayerRunTime.ObjectStorage).GetTemporaryObjectID();
                            _NewObjects.Add(storageInstanceRef);
                            if (!_StorageCellOfObjectUnderTransaction.Contains(storageInstanceRef.StorageInstanceSet))
                                _StorageCellOfObjectUnderTransaction.Add(storageInstanceRef.StorageInstanceSet);
                        }
                    }
                }
            }
        }

        /// <MetaDataID>{fda6eedc-d8d3-4364-b0b6-5f4313a6a474}</MetaDataID>
        List<StorageInstanceRef> _UpdatedObjects;
        /// <MetaDataID>{603f0cff-1e96-46e0-be56-d81f7951a702}</MetaDataID>
        public System.Collections.Generic.List<StorageInstanceRef> UpdatedObjects
        {
            get
            {
                
                if (_UpdatedObjects == null)
                    LoadUpdatedObjects();
                return _UpdatedObjects;
            }
        }
        /// <MetaDataID>{3ed5555c-7c79-4e94-adac-a519b4208a6c}</MetaDataID>
        private void LoadUpdatedObjects()
        {
            _UpdatedObjects = new List<StorageInstanceRef>();
            if (DataNode.Type==MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Object&&DataLoaderMetadata.MemoryCell != null)
            {
                foreach (var objectData in DataLoaderMetadata.MemoryCell.Objects.Values)
                    _UpdatedObjects.Add(objectData.StorageInstanceRef as PersistenceLayerRunTime.StorageInstanceRef);
            }
            else if (TransactionContext.CurrentTransactionContext != null)
            {

                foreach (object memoryInstance in TransactionContext.CurrentTransactionContext.EnlistObjects)
                {
                    StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(memoryInstance) as StorageInstanceRef;
                    if (storageInstanceRef != null &&storageInstanceRef.ObjectStorage==ObjectStorage&&storageInstanceRef.Class.IsA(DataNode.Classifier))
                    {
                        if (storageInstanceRef.PersistentObjectID != null)
                        {
                            _UpdatedObjects.Add(storageInstanceRef);
                            if (!ObjectUnderTransaction.ContainsKey(storageInstanceRef.PersistentObjectID))
                                ObjectUnderTransaction.Add(storageInstanceRef.PersistentObjectID, storageInstanceRef);
                            if (!_StorageCellOfObjectUnderTransaction.Contains(storageInstanceRef.StorageInstanceSet))
                                _StorageCellOfObjectUnderTransaction.Add(storageInstanceRef.StorageInstanceSet);
                        }

                    }
                }
            }
        }

        /// <MetaDataID>{31279941-cf96-433d-9ebe-b60822bce7bb}</MetaDataID>
        System.Collections.Generic.List<StorageInstanceRef> _CandidateForDeleteObjects;
        /// <MetaDataID>{2271536f-64cb-497f-9d6f-1c989970e6bb}</MetaDataID>
        public System.Collections.Generic.List<StorageInstanceRef> CandidateForDeleteObjects
        {
            get
            {
                if (_CandidateForDeleteObjects == null)
                    LoadCandidateForDeleteObjects();
                return _CandidateForDeleteObjects;
            }
        }

        /// <MetaDataID>{c1bbbb25-c646-4d7d-b9e6-ff914f1b49aa}</MetaDataID>
        private void LoadCandidateForDeleteObjects()
        {
            _CandidateForDeleteObjects = new List<StorageInstanceRef>();
            OOAdvantech.Transactions.Transaction transaction = OOAdvantech.Transactions.Transaction.Current;
            while (transaction != null)
            {
                ITransactionContext transactionContext = TransactionContext.GetTransactionContext(transaction);

                if (transactionContext != null)
                {
                    foreach (Commands.Command command in transactionContext.EnlistedCommands.Values)
                    {
                        if (command is Commands.DeleteStorageInstanceCommand &&
                            (command as Commands.DeleteStorageInstanceCommand).StorageInstanceForDeletion.Class.IsA(DataNode.Classifier))
                        {
                            _CandidateForDeleteObjects.Add((command as Commands.DeleteStorageInstanceCommand).StorageInstanceForDeletion);
                        }
                    }
                }

                transaction = transaction.OriginTransaction;
            }
        }

        ///// <MetaDataID>{5b28ad73-9582-4a68-9835-f4bbbcb74c91}</MetaDataID>
        //protected abstract PersistenceLayer.ObjectID GetTemporaryObjectID();
        class RelationIdentity
        {
            public static RelationIdentity GetRelationIdentity(MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ValueTypePath valueTypePath)
            {
                return new RelationIdentity(associationEnd, valueTypePath);
            }
            MetaDataRepository.AssociationEnd AssociationEnd;
            MetaDataRepository.ValueTypePath ValueTypePath;
            public RelationIdentity(MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ValueTypePath valueTypePath)
            {
                AssociationEnd = associationEnd;
                ValueTypePath = valueTypePath;
            }
            string RelationIdentityAsString;
            public override int GetHashCode()
            {
                if(RelationIdentityAsString==null)
                    RelationIdentityAsString=AssociationEnd.Identity.ToString() + ValueTypePath.ToString();
                return RelationIdentityAsString.GetHashCode();
            }


            public static bool operator ==(RelationIdentity left, RelationIdentity right)
            {
                if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                    return true;
                if (object.ReferenceEquals(left, null))
                    return false;
                if (object.ReferenceEquals(right, null))
                    return false;
                return left.ValueTypePath == right.ValueTypePath & left.AssociationEnd.Identity == right.AssociationEnd.Identity;

            }
            public static bool operator !=(RelationIdentity left, RelationIdentity right)
            {
                return !(left == right);
            }

        }
        /// <MetaDataID>{59cbfb40-cee6-4cd7-8b90-905ff4560163}</MetaDataID>
        public void GetRelationChanges()
        {
            MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = DataNode;
            while (dataNode != null && dataNode.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.Type == MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Object)
                dataNode = dataNode.ParentDataNode;
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    (GetDataLoader(dataNode) as StorageDataLoader).GetRelationChanges(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, subDataNode);
            }
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type == MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Object)
                {
                    if (subDataNode.DataSource != null)
                        (GetDataLoader(subDataNode) as StorageDataLoader).GetRelationChanges();
                }
            }
        }

        /// <MetaDataID>{b0ea8779-8df6-4ce6-a810-930b9c72c0e8}</MetaDataID>
        System.Collections.Generic.Dictionary<RelationIdentity, List<ObjectsLink>> RelationsChanges = new Dictionary<RelationIdentity, List<ObjectsLink>>();
        /// <MetaDataID>{f01cc326-4c63-4b1d-bc45-3dafe63f4fdd}</MetaDataID>
        protected override bool HasOutStorageRelationOnCurrentTrasaction(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode)
        {
            MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = DataNode;
            while (dataNode != null && dataNode.AssignedMetaObject is MetaDataRepository.Attribute && dataNode.Type == OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Object)
                dataNode = dataNode.ParentDataNode;


            foreach (ObjectsLink relationChange in (dataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).GetRelationChanges(associationEnd, relatedDataNode))
            {
                if (relationChange.RoleA != null && relationChange.RoleA.ObjectStorage != ObjectStorage)
                    return true;
                if (relationChange.RoleB != null && relationChange.RoleB.ObjectStorage != ObjectStorage)
                    return true;
            }
            return false;

        }
        /// <MetaDataID>{c17b2d4e-ffe8-4fbc-bdd4-5726211904a0}</MetaDataID>
        public System.Collections.Generic.Dictionary<MetaDataRepository.ObjectQueryLanguage.DataNode, bool> ThereIsSubDataNodeRelationWithTransactionNewObject = new Dictionary<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode, bool>();
        /// <MetaDataID>{200ec4cf-d9f0-4cea-a5b4-a00175b68ab7}</MetaDataID>
        public bool ThereIsRelationWithTransactionNewObject = false;

        /// <MetaDataID>{0904fa0a-26aa-4577-9383-1e7ffac2d86c}</MetaDataID>
        public System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> OutStorageTransientObjectIdentityTypes = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();

        //public System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> InStorageTransientObjectIdentityTypes = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
        /// <MetaDataID>{c47e4abb-df5a-4c8b-95f4-9143040aab71}</MetaDataID>
        public List<ObjectsLink> GetRelationChanges(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode)
        {


            if (DataNode != null && DataNode.AssignedMetaObject is MetaDataRepository.Attribute && DataNode.Type == MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Object)
                return (GetDataLoader(DataNode.ParentDataNode) as StorageDataLoader).GetRelationChanges(associationEnd,relatedDataNode);

            MetaDataRepository.ValueTypePath valueTypePath = null;
            if (DataNode.IsParentDataNode(relatedDataNode))
                valueTypePath = DataNode.ParentDataNode.ValueTypePath;
            else
                valueTypePath = relatedDataNode.ParentDataNode.ValueTypePath;



            List<ObjectsLink> relationChanges = null;
            bool dontUpdateRelatedDataNodeForNewObjectsRelation = false;

            if (!RelationsChanges.TryGetValue(RelationIdentity.GetRelationIdentity(associationEnd, valueTypePath), out relationChanges))
            {
                relationChanges = new List<ObjectsLink>();
                if (DataNode.IsParentDataNode(relatedDataNode))
                {
                    if ((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association == DataNode.Classifier.ClassHierarchyLinkAssociation && !DataNode.ThroughRelationTable)
                    {
                        RelationsChanges[RelationIdentity.GetRelationIdentity(associationEnd, valueTypePath)] = relationChanges;
                        return relationChanges;
                    }
                }
                else
                {
                    if ((relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association == DataNode.Classifier.ClassHierarchyLinkAssociation && !relatedDataNode.ThroughRelationTable)
                    {
                        RelationsChanges[RelationIdentity.GetRelationIdentity(associationEnd, valueTypePath)] = relationChanges;
                        return relationChanges;
                    }
                }


                if (NewObjects.Count == 0 && UpdatedObjects.Count == 0)
                {
                    if (valueTypePath.Count == 0 &&
                        associationEnd.GetOtherEnd().Navigable &&
                         !DataNode.IsSameOrParentDataNode(relatedDataNode))
                    {
                        foreach (ObjectsLink objectsLink in (relatedDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).GetRelationChanges(associationEnd.GetOtherEnd(), DataNode))
                            relationChanges.Add(objectsLink);
                    }
                    RelationsChanges[RelationIdentity.GetRelationIdentity(associationEnd, valueTypePath)] = relationChanges;
                    return relationChanges;
                }

                foreach (StorageInstanceRef storageInstanceRef in UpdatedObjects)
                {
                    List<ObjectsLink> objectRelationChanges = storageInstanceRef.GetRelationChanges(associationEnd, valueTypePath);
                    #region Check if there is relation with new object in storage
                    foreach (ObjectsLink objectsLink in objectRelationChanges)
                    {
                        if (!dontUpdateRelatedDataNodeForNewObjectsRelation)
                        {
                            if (associationEnd.IsRoleA && objectsLink.RoleA.ObjectStorage == ObjectStorage && !objectsLink.RoleA.IsPersistent)
                            {
                                (relatedDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ThereIsRelationWithTransactionNewObject = true;
                                dontUpdateRelatedDataNodeForNewObjectsRelation = true;
                            }
                            if (!associationEnd.IsRoleA && objectsLink.RoleB.ObjectStorage == ObjectStorage && !objectsLink.RoleB.IsPersistent)
                            {
                                (relatedDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ThereIsRelationWithTransactionNewObject = true;
                                dontUpdateRelatedDataNodeForNewObjectsRelation = true;
                            }
                        }


                        if (dontUpdateRelatedDataNodeForNewObjectsRelation)
                            break;
                    }
                    #endregion

                    relationChanges.AddRange(objectRelationChanges);
                }

                bool dontUpdateDataNodeForNewObjectsRelation = false;
                if (!relatedDataNode.DataSource.DataLoaders.ContainsKey(DataLoaderMetadata.ObjectsContextIdentity) || (relatedDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ThereIsRelationWithTransactionNewObject)
                    dontUpdateRelatedDataNodeForNewObjectsRelation = true;
                foreach (StorageInstanceRef storageInstanceRef in NewObjects)
                {
                    List<ObjectsLink> objectRelationChanges = storageInstanceRef.GetRelationChanges(associationEnd, valueTypePath);

                    #region Check if there is relation with new object in storage
                    foreach (ObjectsLink objectsLink in objectRelationChanges)
                    {
                        if (!dontUpdateRelatedDataNodeForNewObjectsRelation)
                        {
                            if (associationEnd.IsRoleA)
                            {
                                if (!objectsLink.RoleA.IsPersistent)
                                {
                                    (relatedDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ThereIsRelationWithTransactionNewObject = true;
                                    dontUpdateRelatedDataNodeForNewObjectsRelation = true;

                                    if (objectsLink.RoleA.ObjectStorage != ObjectStorage)
                                    {

                                        if (!(GetDataLoader(relatedDataNode) as StorageDataLoader).OutStorageTransientObjectIdentityTypes.Contains(objectsLink.RoleA.ObjectID.ObjectIdentityType))
                                            (GetDataLoader(relatedDataNode) as StorageDataLoader).OutStorageTransientObjectIdentityTypes.Add(objectsLink.RoleA.ObjectID.ObjectIdentityType);
                                    }
                                    //else
                                    //{
                                    //    if (!(GetDataLoader(relatedDataNode) as StorageDataLoader).InStorageTransientObjectIdentityTypes.Contains(objectsLink.RoleA.TransientObjectID.ObjectIdentityType))
                                    //        (GetDataLoader(relatedDataNode) as StorageDataLoader).InStorageTransientObjectIdentityTypes.Add(objectsLink.RoleA.TransientObjectID.ObjectIdentityType);

                                    //}
                                }
                            }
                            if (!associationEnd.IsRoleA)
                            {
                                if (!objectsLink.RoleB.IsPersistent)
                                {
                                    (relatedDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ThereIsRelationWithTransactionNewObject = true;
                                    dontUpdateRelatedDataNodeForNewObjectsRelation = true;
                                    if (objectsLink.RoleB.ObjectStorage != ObjectStorage)
                                    {
                                        if (!(GetDataLoader(relatedDataNode) as StorageDataLoader).OutStorageTransientObjectIdentityTypes.Contains(objectsLink.RoleB.ObjectID.ObjectIdentityType))
                                            (GetDataLoader(relatedDataNode) as StorageDataLoader).OutStorageTransientObjectIdentityTypes.Add(objectsLink.RoleB.ObjectID.ObjectIdentityType);
                                    }
                                    //else
                                    //{
                                    //    if (!(GetDataLoader(relatedDataNode) as StorageDataLoader).InStorageTransientObjectIdentityTypes.Contains(objectsLink.RoleB.TransientObjectID.ObjectIdentityType))
                                    //        (GetDataLoader(relatedDataNode) as StorageDataLoader).InStorageTransientObjectIdentityTypes.Add(objectsLink.RoleB.TransientObjectID.ObjectIdentityType);


                                    //}
                                }
                            }
                        }
                        if (!dontUpdateDataNodeForNewObjectsRelation)
                        {
                            if (DataNode == relatedDataNode)
                                ThereIsRelationWithTransactionNewObject = true;
                            else
                                ThereIsSubDataNodeRelationWithTransactionNewObject[relatedDataNode] = true;
                            dontUpdateDataNodeForNewObjectsRelation = true;
                        }
                        if (dontUpdateRelatedDataNodeForNewObjectsRelation)
                            break;
                    }
                    #endregion

                    relationChanges.AddRange(objectRelationChanges);
                }




                if (valueTypePath.Count == 0 &&
                    associationEnd.GetOtherEnd().Navigable &&
                    !DataNode.IsSameOrParentDataNode(relatedDataNode))
                {
                    foreach (ObjectsLink objectsLink in (relatedDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).GetRelationChanges(associationEnd.GetOtherEnd(), DataNode))
                    {
                        if (!relationChanges.Contains(objectsLink))
                            relationChanges.Add(objectsLink);
                    }
                }
                RelationsChanges[RelationIdentity.GetRelationIdentity(associationEnd, valueTypePath)] = relationChanges;
            }
            return relationChanges;

        }

        #endregion


        /// <summary></summary>
        /// <param name="associationEnd"></param>
        /// <param name="valueTypePath"></param>
        /// <param name="ownerObject"></param>
        /// <param name="relatedObject"></param>
        /// <MetaDataID>{849f5111-c1fa-4b07-9cb7-8b59f56176ce}</MetaDataID>
        protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath, object ownerObject, object relatedObject)
        {
            StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(ownerObject) as StorageInstanceRef;
            foreach (PersistenceLayerRunTime.RelResolver relResolver in storageInstanceRef.RelResolvers)
            {
                if (associationEnd.Identity == relResolver.AssociationEnd.Identity && (relResolver.Owner is PersistenceLayerRunTime.StorageInstanceValuePathRef) && valueTypePath.ToString() == (relResolver.Owner as PersistenceLayerRunTime.StorageInstanceValuePathRef).ValueTypePath.ToString() + ".(" + associationEnd.Identity.ToString() + ")")
                {
                    lock (relResolver)
                    {
                        if (!relResolver.IsCompleteLoaded)
                        {
                            storageInstanceRef.InitializeRelatedObject(relResolver, relatedObject);
                            storageInstanceRef.SetMemberValue(relResolver, relatedObject);
                            relResolver._IsCompleteLoaded = true;
                        }
                        else
                        {
                        }
                    }
                    break;
                }
            }

        }
        /// <MetaDataID>{7906aa5d-7b77-4698-8e8e-3868ec8afc59}</MetaDataID>
        protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath, object ownerObject, System.Collections.Generic.List<object> relatedObjects)
        {
            StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(ownerObject) as StorageInstanceRef;
            foreach (PersistenceLayerRunTime.RelResolver relResolver in storageInstanceRef.RelResolvers)
            {
                if (associationEnd.Identity == relResolver.AssociationEnd.Identity && (relResolver.Owner is PersistenceLayerRunTime.StorageInstanceValuePathRef) && valueTypePath.ToString() == (relResolver.Owner as PersistenceLayerRunTime.StorageInstanceValuePathRef).ValueTypePath.ToString() + ".(" + associationEnd.Identity.ToString() + ")")
                {
                    PersistenceLayer.ObjectContainer theObjectContainer = storageInstanceRef.GetMemoryInstanceMemberValue(relResolver) as PersistenceLayer.ObjectContainer;
                    if (theObjectContainer == null)
                    {
                        theObjectContainer = (PersistenceLayer.ObjectContainer)AccessorBuilder.CreateInstance(relResolver.FieldInfo.FieldType);//Error Prone 
                        storageInstanceRef.SetMemberValue(relResolver, theObjectContainer);

                    }
                    PersistenceLayer.ObjectCollection objects = StorageInstanceRef.GetObjectCollection(theObjectContainer);
                    if (objects == null)
                        objects = new OnMemoryObjectCollection();
                    StorageInstanceRef.SetObjectCollection(theObjectContainer, objects);
                    foreach (object obj in relatedObjects)
                        objects.Add(obj);
                    break;
                }
            }


        }
        /// <MetaDataID>{fbc99416-58d0-49ac-abb0-e2e4ee18a300}</MetaDataID>
        protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        {

            MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ownerObject.GetType()) as MetaDataRepository.Classifier;
            if (classifier is DotNetMetaDataRepository.Class)
            {
                StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(ownerObject) as StorageInstanceRef;
                foreach (PersistenceLayerRunTime.RelResolver relResolver in storageInstanceRef.RelResolvers)
                {
                    if (associationEnd.Identity == relResolver.AssociationEnd.Identity)
                    {
                        lock (relResolver)
                        {
                            if (!relResolver.IsCompleteLoaded)
                            {
                                storageInstanceRef.InitializeRelatedObject(relResolver, relatedObject);
                                storageInstanceRef.SetMemberValue(relResolver, relatedObject);
                                relResolver._IsCompleteLoaded = true;
                            }
                            else
                            {
                            }
                        }
                        break;
                    }
                }
            }

        }
        /// <MetaDataID>{2754a8bf-987f-4f7a-9a02-c5acf725cbc5}</MetaDataID>
        protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, System.Collections.Generic.List<object> relatedObjects)
        {


            MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ownerObject.GetType()) as MetaDataRepository.Classifier;
            if (classifier is DotNetMetaDataRepository.Class)
            {
                StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(ownerObject) as StorageInstanceRef;
                foreach (PersistenceLayerRunTime.RelResolver relResolver in storageInstanceRef.RelResolvers)
                {
                    if (associationEnd.Identity == relResolver.AssociationEnd.Identity)
                    {
                        relResolver.CompleteLoad(relatedObjects);

                        break;
                    }
                }
            }
        }
        abstract public MetaDataRepository.ObjectIdentityType ObjectIdentityTypeForNewObject
        {
            get;
        }
        /// <MetaDataID>{75604d4c-2a02-476f-b182-b4ac598cb32f}</MetaDataID>
        /// <summary>
        /// Retrieves relation parts and ObjectIdentityTypes which used 
        /// for the relation of dataloader with related DataNode DataLoader
        /// </summary>
        public Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> GetRelationPartsObjectIdentityTypes(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode)
        {
            Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> _objectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            

            if (DataNode.IsParentDataNode(relatedDataNode))
            {
                Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>> dataNodeRelatedStorageCells = null;

                if ((GetDataLoader(relatedDataNode) as StorageDataLoader).DataLoaderMetadata.RelatedStorageCells.TryGetValue(DataNode.Identity, out dataNodeRelatedStorageCells))
                {
                    var relatedDataLoader = (GetDataLoader(relatedDataNode) as StorageDataLoader);
                    foreach (KeyValuePair<StorageCellReference.StorageCellReferenceMetaData, Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>> relatedStorageCells in dataNodeRelatedStorageCells)
                    {
                        StorageCellReference.StorageCellReferenceMetaData rootStorageCellReferenceMetaData = relatedStorageCells.Key;
                        Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>> relationPartsStorageCellReferenceMetaData = relatedStorageCells.Value;
                        foreach (var relationPartStorageCellsEntry in relationPartsStorageCellReferenceMetaData)
                        {
                            List<MetaDataRepository.ObjectIdentityType> relationPartObjectIdentityTypes = null;
                            List<StorageCellReference.StorageCellReferenceMetaData> relationPartStorageCells = relationPartStorageCellsEntry.Value;
                            RelationPartIdentity relationPartIdentity = relationPartStorageCellsEntry.Key;
                            foreach (var storageCellReferenceMetaData in relationPartStorageCells)
                            {
                                foreach (var storageCell in DataLoaderMetadata.StorageCells)
                                {
                                    if (storageCell.StorageIdentity == storageCellReferenceMetaData.StorageIdentity && storageCell.SerialNumber == storageCellReferenceMetaData.SerialNumber)
                                    {
                                        if (!_objectIdentityTypes.TryGetValue(relationPartIdentity, out relationPartObjectIdentityTypes))
                                        {
                                            relationPartObjectIdentityTypes = new List<ObjectIdentityType>();
                                            _objectIdentityTypes[relationPartIdentity] = relationPartObjectIdentityTypes;
                                        }
                                        if (!relationPartObjectIdentityTypes.Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]))
                                            relationPartObjectIdentityTypes.Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //if(relatedDataNode.DataSource.HasInObjectContextData||relatedDataNode.DataSource.HasOutObjectContextData)

                Dictionary<StorageCellReference.StorageCellReferenceMetaData, Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>>> relatedStorageCells = null;
                if (DataLoaderMetadata.RelatedStorageCells.TryGetValue(relatedDataNode.Identity, out relatedStorageCells))
                {
                    foreach (var relationPartsStorageCellsEntry in relatedStorageCells)
                    {
                        StorageCellReference.StorageCellReferenceMetaData dataLoaderStorageCellMetaData = relationPartsStorageCellsEntry.Key;
                        Dictionary<RelationPartIdentity, List<StorageCellReference.StorageCellReferenceMetaData>> relationPartsStorageCellReferenceMetaData = relationPartsStorageCellsEntry.Value;
                        foreach (var relationPartStorageCellsEntry in relationPartsStorageCellReferenceMetaData)
                        {
                            List<MetaDataRepository.ObjectIdentityType> relationPartObjectIdentityTypes = null;
                            RelationPartIdentity relationPartIdentity = relationPartStorageCellsEntry.Key;
                            if (!_objectIdentityTypes.TryGetValue(relationPartIdentity, out relationPartObjectIdentityTypes))
                            {
                                relationPartObjectIdentityTypes = new List<ObjectIdentityType>();
                                _objectIdentityTypes[relationPartIdentity] = relationPartObjectIdentityTypes;
                            }
                            //foreach (var StorageCellReferenceMetaData in relationPartStorageCell.Value)

                            if (!relationPartObjectIdentityTypes.Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(dataLoaderStorageCellMetaData.ObjectIdentityType)]))
                                relationPartObjectIdentityTypes.Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(dataLoaderStorageCellMetaData.ObjectIdentityType)]);
                        }
                    }
                }
                else
                {

                }
            }

            if (NewObjects.Count > 0)
            {
                RelationPartIdentity relationPartIdentity = null;
                if (DataNode.IsParentDataNode(relatedDataNode))
                    relationPartIdentity = (DataNode.AssignedMetaObject as AssociationEnd).Identity.ToString();
                else
                    relationPartIdentity = (relatedDataNode.AssignedMetaObject as AssociationEnd).Identity.ToString();
                
                List<MetaDataRepository.ObjectIdentityType> relationPartObjectIdentityTypes = null;
                if (!_objectIdentityTypes.TryGetValue(relationPartIdentity, out  relationPartObjectIdentityTypes))
                {
                    relationPartObjectIdentityTypes = new List<ObjectIdentityType>();
                    _objectIdentityTypes[relationPartIdentity] = relationPartObjectIdentityTypes;
                }

                if (!_objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(ObjectIdentityTypeForNewObject)]))
                    _objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(ObjectIdentityTypeForNewObject)]);
            }

            #region Remove Code

            //Dictionary<string, List<MetaDataRepository.ObjectIdentityType>> objectIdentityTypes = new Dictionary<string, List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
            //MetaDataRepository.Roles dataNodeDataLoaderRole;
            //MetaDataRepository.Roles relationPartIdentityAsociatioEndRole;
            //if (DataNode.IsParentDataNode(relatedDataNode))
            //{
            //    dataNodeDataLoaderRole = (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Role;
            //    relationPartIdentityAsociatioEndRole = (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Role;
            //}
            //else
            //{
            //    dataNodeDataLoaderRole = (relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Role;
            //    relationPartIdentityAsociatioEndRole = (relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Role;
            //}
            //foreach (MetaDataRepository.StorageCellsLink storageCellsLink in GetStorageCellsLinks(relatedDataNode))
            //{
            //    if (relationPartIdentityAsociatioEndRole == OOAdvantech.MetaDataRepository.Roles.RoleA)
            //        relationPartIdentity = storageCellsLink.Type.RoleA.Identity.ToString();
            //    else
            //        relationPartIdentity = storageCellsLink.Type.RoleB.Identity.ToString();
            //    if (!objectIdentityTypes.ContainsKey(relationPartIdentity))
            //        objectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>();
            //    if (DataNode.Classifier.ClassHierarchyLinkAssociation != null &&
            //        relatedDataNode.IsParentDataNode(DataNode) &&
            //        DataNode.Classifier.ClassHierarchyLinkAssociation == (relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
            //    {
            //        foreach (MetaDataRepository.StorageCell storageCell in storageCellsLink.AssotiationClassStorageCells)
            //        {
            //            if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]))
            //                objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]);
            //        }
            //    }
            //    else if (DataNode.Classifier.ClassHierarchyLinkAssociation != null &&
            //        DataNode.IsParentDataNode(relatedDataNode) &&
            //        DataNode.Classifier.ClassHierarchyLinkAssociation == (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
            //    {
            //        foreach (MetaDataRepository.StorageCell storageCell in storageCellsLink.AssotiationClassStorageCells)
            //        {
            //            if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]))
            //                objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCell.ObjectIdentityType)]);
            //        }
            //    }
            //    else
            //    {
            //        if (dataNodeDataLoaderRole == OOAdvantech.MetaDataRepository.Roles.RoleA)
            //        {
            //            if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCellsLink.RoleAStorageCell.ObjectIdentityType)]))
            //                objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCellsLink.RoleAStorageCell.ObjectIdentityType)]);
            //        }
            //        else
            //        {
            //            if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCellsLink.RoleBStorageCell.ObjectIdentityType)]))
            //                objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(storageCellsLink.RoleBStorageCell.ObjectIdentityType)]);
            //        }
            //    }

            //}

            //#region relation object identity types for relations under transactions
            //if (DataNode.IsParentDataNode(relatedDataNode))
            //    relationPartIdentity = (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity.ToString();
            //else
            //    relationPartIdentity = (relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Identity.ToString();

            //if (DataNode.IsParentDataNode(relatedDataNode) && ThereIsRelationWithTransactionNewObject)
            //{
            //    if (!objectIdentityTypes.ContainsKey(relationPartIdentity))
            //        objectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>();

            //    if (NewObjects.Count > 0 && !objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypeForNewObject))
            //        objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(ObjectIdentityTypeForNewObject)]);

            //    foreach (MetaDataRepository.ObjectIdentityType outStorageTransientObjectIdentityType in OutStorageTransientObjectIdentityTypes)
            //    {
            //        if (!objectIdentityTypes[relationPartIdentity].Contains(outStorageTransientObjectIdentityType))
            //            objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(outStorageTransientObjectIdentityType)]);

            //    }
            //}
            //else
            //{
            //    if (ThereIsSubDataNodeRelationWithTransactionNewObject.ContainsKey(relatedDataNode) &&
            //        ThereIsSubDataNodeRelationWithTransactionNewObject[relatedDataNode])
            //    {
            //        if (!objectIdentityTypes.ContainsKey(relationPartIdentity))
            //            objectIdentityTypes[relationPartIdentity] = new List<MetaDataRepository.ObjectIdentityType>();

            //        if (!objectIdentityTypes[relationPartIdentity].Contains(ObjectIdentityTypeForNewObject))
            //            objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(ObjectIdentityTypeForNewObject)]);
            //    }
            //}
            //#endregion

            ////Empty data source
            //if (objectIdentityTypes.Count == 0)
            //{
            //    objectIdentityTypes[relationPartIdentity] = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
            //    objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(ObjectIdentityTypeForNewObject)]);
            //}


            #endregion


            //if (_objectIdentityTypes.Count == 0)
            //{
            //    _objectIdentityTypes[relationPartIdentity] = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
            //    _objectIdentityTypes[relationPartIdentity].Add(ObjectIdentityTypes[ObjectIdentityTypes.IndexOf(ObjectIdentityTypeForNewObject)]);
            //}
            return _objectIdentityTypes;
        }

        abstract public Collections.Generic.Set<MetaDataRepository.StorageCellsLink> GetStorageCellsLinks(MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode);


    }
}
