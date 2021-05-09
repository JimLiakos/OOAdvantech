namespace OOAdvantech.PersistenceLayerRunTime
{
    using System;
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
    using System.Collections.Generic;
    using Remoting;
#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif


    /// <summary>
    /// Object storage provide the basic functionality to create a persistent object,
    /// make a transient object persistent,retrieve objects.
    /// This class realized from run times for instance MSSQL run time,XML run time.      
    /// </summary>
    /// <MetaDataID>{F2ED599E-455E-4A46-9BA3-85467CC9D1EA}</MetaDataID>
    public abstract class ObjectStorage : PersistenceLayer.ObjectStorage,IObjectStorage, Remoting.IPersistentObjectLifeTimeController, MetaDataRepository.ObjectQueryLanguage.IObjectQueryPartialResolver
    {

        
        public override bool HasReferencialintegrityConstrain(object thePersistentObject)
        {
            StorageInstanceRef storageInstanceRef= StorageInstanceRef.GetStorageInstanceRef(thePersistentObject) as StorageInstanceRef;
            if (storageInstanceRef == null)
                return false;
            return storageInstanceRef.RuntimeReferentialIntegrityCount > 0;
        }

        /// <MetaDataID>{38ad40f0-1fd2-4bb9-a674-3bb7debc42cd}</MetaDataID>
        public static StorageProvider GetStorageProvider(string dataSourceName)
        {
            string storageProviderClassFullName = null; ;
            if (StorageProviders.TryGetValue(dataSourceName, out storageProviderClassFullName))
            {
                StorageProvider storageProvider = AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType(storageProviderClassFullName, "")) as StorageProvider;
                return storageProvider;
            }
            return null;
        }


        /// <MetaDataID>{8207b65e-f46b-4e15-ba5e-e635fef2ba24}</MetaDataID>
        public string StorageIdentity
        {
            get
            {
                return StorageMetaData.StorageIdentity;
            }
        }

        /// <MetaDataID>{8ef75c33-97da-417d-8636-df45c658b2e6}</MetaDataID>
        public abstract PersistenceLayer.ObjectID GetTemporaryObjectID();

        /// <MetaDataID>{88aed1f5-b3b9-49d2-938e-5e99a3a4e52f}</MetaDataID>
        public abstract DataLoader CreateDataLoader(DataNode dataNode, DataLoaderMetadata dataLoaderMetadata);

        /// <MetaDataID>{b5f8c4e1-e162-4be6-b15d-59b4bff1d5f6}</MetaDataID>
        public abstract MetaDataRepository.StorageCell GetStorageCell(StorageInstanceAgent storageInstanceAgent);



        /// <MetaDataID>{a37899a1-b947-4109-a5f3-d8d043b8bf1d}</MetaDataID>
        public override object GetObjectID(object persistentObject)
        {

            StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(persistentObject) as StorageInstanceRef;
            if (storageInstanceRef != null)
                return storageInstanceRef.PersistentObjectID;
            else
                return null;

        }
        /// <MetaDataID>{f7c55734-134b-4dd7-b22f-ff9e9fd629f3}</MetaDataID>
        public virtual OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery DistributeObjectQuery(Guid mainQueryIdentity,
                        OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> dataTrees,
                        QueryResultType queryResult,
                        OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode> selectListItems,
                        OOAdvantech.Collections.Generic.Dictionary<Guid, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> dataLoadersMetadata,
                        OOAdvantech.Collections.Generic.Dictionary<string, object> parameters,
                        System.Collections.Generic.List<string> usedAliases,
                        System.Collections.Generic.List<string> queryStorageIdentities)
        {

            OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DistributedObjectQuery objectQuery = new DistributedObjectQuery(mainQueryIdentity,dataTrees, queryResult, selectListItems, this, dataLoadersMetadata, parameters, usedAliases, queryStorageIdentities);
            return objectQuery;

        }





        /// <MetaDataID>{0960E9AC-7893-4133-B90B-4AD0AC975264}</MetaDataID>
        /// <summary>
        /// It is a trick to solve the protection problem from facade class. 
        /// This means that we want to protect the operation in the abstract layer from 
        /// client but we want to access the operation from implementation code.
        /// </summary>
        public void UnprotectedDelete(object thePersistentObject, OOAdvantech.PersistenceLayer.DeleteOptions deleteOption)
        {
            Delete(thePersistentObject, deleteOption);
        }

        /// <summary>Delete the object from storage.</summary>
        /// <param name="thePersistentObject">The persistent object, which will be deleted. Remember that the object has implicitly connection with storage instance.</param>
        /// <param name="deleteOption">
        /// The deleteOption argument defines what persistency system do 
        /// when the object delete failed. 
        /// If deleteOption is TryToDelete then there is nothing to do simply try.
        /// If deleteOption is EnsureObjectDeletion then abort transaction.</param>
        /// <MetaDataID>{BC3A0F5C-1AEE-44F9-8813-4A65C16549E2}</MetaDataID>
        protected override void Delete(object thePersistentObject, OOAdvantech.PersistenceLayer.DeleteOptions deleteOption)
        {
            if (thePersistentObject == null)
                return;
            StorageInstanceRef ObjStorageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(thePersistentObject) as StorageInstanceRef;
            if (ObjStorageInstanceRef == null)
                return;
            using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(thePersistentObject))
            {
                CreateDeleteStorageInstanceCommand(ObjStorageInstanceRef, deleteOption);
                StateTransition.Consistent = true; ;
            }


            //ServerSideStorageSession.Delete(thePersistentObject, deleteOption);
        }

        /// <MetaDataID>{eea4a467-5703-4013-8a5e-5f85fb16d00e}</MetaDataID>
        public override void MoveObject(object persistentObject)
        {
            if (persistentObject == null)
                return;
            StorageInstanceAgent movingStorageInstanceAgent = StorageInstanceRef.GetStorageInstanceAgent(persistentObject);
            if (movingStorageInstanceAgent == null)
                return;
            if (movingStorageInstanceAgent.StorageIdentity == StorageMetaData.StorageIdentity)
                return;

            CreateMoveStorageInstanceCommand(movingStorageInstanceAgent);
            movingStorageInstanceAgent.ObjectStorage.CreateMoveStorageInstanceCommand(movingStorageInstanceAgent);

        }

        /// <MetaDataID>{b8ed6a83-6355-4686-acd0-e56aaabf00bd}</MetaDataID>
        public abstract void CreateMoveStorageInstanceCommand(StorageInstanceAgent storageInstance);

        /// <summary>
        /// This method creates a new object with type that defined from parameter type. 
        /// </summary>
        /// <param name="type">Define the type of object that will be create. </param>
        /// <param name="ctorParams">With ctorParams you can call other than default object constructor.
        /// If you use the ctorParams the last parameter must be a type array with the types of 
        /// parameters of constructor, which you want to call. It is useful for type check. 
        /// You can avoid wrong constructor call. </param>
        /// <MetaDataID>{4F6177A5-06A8-4D95-842F-F4F22B4BCE43}</MetaDataID>
        public override object NewObject(Type type, Type[] paramsTypes, params object[] ctorParams)
        {

            using (OOAdvantech.Transactions.SystemStateTransition sysStateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                object newObject = null;
                try
                {
                    newObject = AccessorBuilder.CreateInstance(type, paramsTypes, ctorParams);
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception("Persistency System can't instadiate the " + type + " case: " + Error.Message, Error);
                }
                if (newObject == null)
                    throw (new System.Exception("PersistencyService can't instadiate the " + type));

                CommitTransientObjectState(newObject);

                //PersistenceLayerRunTime.StorageInstanceRef mStorageInstanceRef;

                //mStorageInstanceRef = CreateStorageInstanceRef(NewObject, null);

                //using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(NewObject))
                //{

                //    using (Transactions.ObjectStateTransition storageInstanceRefStateTransition = new Transactions.ObjectStateTransition(mStorageInstanceRef))
                //    {

                //        storageInstanceRefStateTransition.Consistent = true;
                //    }
                //    StateTransition.Consistent = true;
                //}
                sysStateTransition.Consistent = true;
                return newObject;
            }
        }


        /// <MetaDataID>{8A2226C4-979A-485F-96D8-AD4EFE7FA01D}</MetaDataID>
        public override object NewTransientObject(Type type, Type[] paramsTypes, params object[] ctorParams)
        {

            object newObject = null;
            try
            {
                newObject = AccessorBuilder.CreateInstance(type, paramsTypes, ctorParams);
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("Stortage can't instadiate the " + type + " case: " + Error.Message, Error);
            }
            if (newObject == null)
                throw (new System.Exception("Stortage can't instadiate the " + type));
            return newObject;

        }






        /// <summary>
        /// This method creates a new object with type that defined from parameter type. 
        /// </summary>
        /// <param name="type">Define the type of object that will be create. </param>
        /// <param name="ctorParams">With ctorParams you can call other than default object constructor.
        /// If you use the ctorParams the last parameter must be a type array with the types of 
        /// parameters of constructor, which you want to call. It is useful for type check. 
        /// You can avoid wrong constructor call. </param>
        /// <MetaDataID>{4F6177A5-06A8-4D95-842F-F4F22B4BCE43}</MetaDataID>
        public override object NewObject(Type type)
        {

            using (OOAdvantech.Transactions.SystemStateTransition sysStateTransition = new OOAdvantech.Transactions.SystemStateTransition())
            {
                object newObject = null;
                try
                {
                    newObject = AccessorBuilder.CreateInstance(type);
                }
                catch (System.Exception Error)
                {
                    throw new System.Exception("Persistency System can't instadiate the " + type + " case: " + Error.Message, Error);
                }
                if (newObject == null)
                    throw (new System.Exception("PersistencyService can't instadiate the " + type));

                CommitTransientObjectState(newObject);
                //PersistenceLayerRunTime.StorageInstanceRef mStorageInstanceRef;
                //mStorageInstanceRef = CreateStorageInstanceRef(NewObject, null);
                //using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(NewObject))
                //{
                //    StateTransition.Consistent = true;
                //}
                sysStateTransition.Consistent = true;
                return newObject;
            }
        }


        /// <MetaDataID>{8A2226C4-979A-485F-96D8-AD4EFE7FA01D}</MetaDataID>
        public override object NewTransientObject(Type type)
        {

            object newObject = null;
            try
            {
                newObject = AccessorBuilder.CreateInstance(type);
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("Stortage can't instadiate the " + type + " case: " + Error.Message, Error);
            }
            if (newObject == null)
                throw (new System.Exception("Stortage can't instadiate the " + type));
            return newObject;

        }




        /// <summary>This method can be used in the case where you want to create a transient 
        /// object and later decide to store in storage. 
        /// The process of transient object must be the as with the process that 
        /// runs the others object of storage.</summary>
        /// <param name="Object">Define the object which will be persistent.</param>
        /// <MetaDataID>{356CD92F-5062-4E99-A3BA-9818C450C805}</MetaDataID>
        public override void CommitTransientObjectState(object Object)
        {
            if (Object == null)
                return;

            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(Object as MarshalByRefObject))
                throw new System.Exception("The object " + Object.ToString() + " runs out of storage process and can't be saved in storage.\n" +
                                            "For more information check the examples for CommitTransientObjectState in help");
            if (StorageInstanceRef.GetStorageInstanceRef(Object) == null)
            {

                using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(Transactions.TransactionOption.Required))
                {
                    StorageInstanceRef storageInstanceRef = null;
                    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(Object))
                    {
                        storageInstanceRef = CreateStorageInstanceRef(Object, null);
                        using (Transactions.ObjectStateTransition storageInstanceRefStateTransition = new Transactions.ObjectStateTransition(storageInstanceRef))
                        {
                            if (Transactions.Transaction.Current.OriginTransaction == null)//It isn't nested transaction
                                (TransactionContext.CurrentTransactionContext as TransactionContext).TransientToPersistent(Object);

                            storageInstanceRefStateTransition.Consistent = true;
                        }
                        StateTransition.Consistent = true;
                    }
                    storageInstanceRef.ObjectID = GetTemporaryObjectID();
                    storageInstanceRef.ObjectActived();
                    storageInstanceRef.ObjectStateCommit();

                    stateTransition.Consistent = true;
                }



            }
            else
            {
                if (ObjectStorage.GetStorageOfObject(Object) != this)
                    throw new System.Exception("The object " + Object.ToString() + " is already persistent");

                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(Object))
                {
                    StateTransition.Consistent = true;
                }
            }


        }



        /// <MetaDataID>{F4B109B7-D4CB-4936-AEE8-7D97BE28E446}</MetaDataID>
        public event PersistenceLayer.ObjectStorageChangedEventHandler ObjectStorageChanged;

        #region Object storage change state commands
        /// <summary>
        ///This method creates and register to the transaction context a
        /// <see cref="OOAdvantech.PersistenceLayerRunTime.Commands.LinkObjectsCommand">LinkObjectsCommand</see> command. 
        /// After the execution of link command the link between two objects has saved in storage. 
        /// The code for the link command execution depends on the type of storage.
        /// Every type of storage creates a specialized link command object.
        /// </summary>
        /// <param name="roleA">
        /// This parameter defines the roleA object.The value must be not null
        /// </param> 
        /// <param name="roleB">
        /// This parameter defines the roleB object.The value must be not null
        /// </param>
        /// <param name="relationObject">
        /// This parameter defines the relation object.Some times associations have association classes.
        /// </param>
        /// <param name="linkInitiatorAssociationEnd">
        /// This parameter defines the meta data of object relationship, also
        /// defines indirectly the role object which initial create the link.
        /// The value must be not null.
        /// </param>
        /// <MetaDataID>{FC5C6CF5-C220-4296-B7F1-5FF398772F65}</MetaDataID>
        public abstract void CreateLinkCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index);


        /// <MetaDataID>{0d8f827a-3d37-4f60-9242-5d091ff25a51}</MetaDataID>
        public abstract void CreateUpdateLinkIndexCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index);

       


        /// <summary>
        ///This method creates and register to the transaction context a
        /// <see cref="OOAdvantech.PersistenceLayerRunTime.Commands.UnLinkObjectsCommand">UnLinkObjectsCommand</see> command. 
        /// After the execution of unlink command the link between two objects has removed from storage. 
        /// The code for the unlink command execution depends on the type of storage.
        /// Every type of storage creates a specialized unlink command object.
        /// </summary>
        /// <param name="roleA">
        /// This parameter defines the roleA object.The value must be not null
        /// </param> 
        /// <param name="roleB">
        /// This parameter defines the roleB object.The value must be not null
        /// </param>
        /// <param name="relationObject">
        /// This parameter defines the relation object.Some times associations have association classes.
        /// </param>
        /// <param name="linkInitiatorAssociationEnd">
        /// This parameter defines the meta data of object relationship, also
        /// defines indirectly the role object which initial removes the link.
        /// The value must be not null.
        /// </param>
        /// <MetaDataID>{E43F570B-D6F6-4893-AD20-31AD192F55D3}</MetaDataID>
        public abstract void CreateUnLinkCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index);

        ///<summary>
        ///This method creates and register to the transaction context a
        ///<see cref="OOAdvantech.PersistenceLayerRunTime.Commands.UpdateReferentialIntegrity">referecnial integrity count uptade command</see>
        /// The code for the referecnial integrity count uptade command execution depends on the type of storage.
        /// Every type of storage creates a specialized referecnial integrity count uptade command object.
        ///</summary>
        ///<param name="storageInstanceRef">
        ///this parameter define the storage instance which system want to update referecnial integrity count value
        ///</param>
        /// <MetaDataID>{C30AFC06-6BBF-4AA5-BD89-1B939A954937}</MetaDataID>
        public abstract void CreateUpdateReferentialIntegrity(StorageInstanceRef storageInstanceRef);

        ///<summary>
        ///This method creates and register to the transaction context a
        /// <see cref="OOAdvantech.PersistenceLayerRunTime.Commands.UnlinkAllObjectCommand">UnlinkAllObjectCommand</see> command. 
        /// After the execution of unlink all command the links between source and other objects, with link type corresponding
        /// to the association of association end has been removed. 
        /// The code for the unlink all command execution depends on the type of storage.
        /// Every type of storage creates a specialized unlink all command object.
        ///</summary>
        ///<param name="sourceStorageInstance">
        ///This parameter defines the source object. 
        ///All links of this object which type correspond to AssociationEnd type, will be deleted.  
        ///</param>
        ///<param name="associationEnd">
        ///This parameter define the type of links which will be removed.
        ///</param>
        /// <MetaDataID>{ABA8C6AD-573A-4A9A-934D-6E833CA7953E}</MetaDataID>
        public abstract void CreateUnlinkAllObjectCommand(StorageInstanceAgent sourceStorageInstance, DotNetMetaDataRepository.AssociationEnd associationEnd);


        /// <summary>
        ///This method creates and register to the transaction context a
        ///<see cref="OOAdvantech.PersistenceLayerRunTime.Commands.DeleteStorageInstanceCommand">delete storage instance command</see>
        /// The code for the delete storasge instance command execution depends on the type of storage.
        /// Every type of storage creates a specialized delete storasge instance command object.
        /// </summary>
        /// <param name="storageInstance">
        /// This parameter defines the object which we want to delete.
        /// </param>
        /// <param name="deleteOption">
        /// This parameter defines the type of deletion.
        /// For more information look at <see cref="OOAdvantech.PersistenceLayer.DeleteOptions">DeleteOptions</see> 
        /// </param>
        /// <MetaDataID>{AE6121FA-AEDE-4796-A273-4DACB522A1CA}</MetaDataID>
        public abstract void CreateDeleteStorageInstanceCommand(StorageInstanceRef storageInstance, PersistenceLayer.DeleteOptions deleteOption);



        /// <summary>
        ///This method creates and register to the transaction context a
        ///<see cref="OOAdvantech.PersistenceLayerRunTime.Commands.UpdateStorageInstanceCommand">update storage instance command</see>
        /// The code for the update storasge instance command execution depends on the type of storage.
        /// Every type of storage creates a specialized delete storasge instance command object.
        /// </summary>
        /// <param name="storageInstanceRef">
        /// This parameter defines the reference to storage instance which will be updated.
        /// The parameter must be not null. 
        /// </param>
        /// <MetaDataID>{DF681609-A1A5-4160-8CF8-AE49BCD81916}</MetaDataID>
        public abstract void CreateUpdateStorageInstanceCommand(StorageInstanceRef storageInstanceRef);



        /// <summary>
        ///This method creates and register to the transaction context a
        ///<see cref="OOAdvantech.PersistenceLayerRunTime.Commands.NewStorageInstanceCommand">new storage instance command</see>
        /// The code for the new storasge instance command execution depends on the type of storage.
        /// Every type of storage creates a specialized new storasge instance command object.
        /// At this time the storage instance reference connect only with the storage in which will be created the storage instance. 
        /// </summary>
        /// <param name="storageInstance">
        /// This parameter defenis a temporary storage instance ref object;
        /// The parameter must be not null. The ObjectID of storage instance ref object must be null.
        /// </param>
        /// <MetaDataID>{7F1939C0-3E14-484E-A5D4-9362ED82186E}</MetaDataID>
        public abstract void CreateNewStorageInstanceCommand(StorageInstanceRef storageInstance);


        /// <MetaDataID>{95c5ebf9-c734-4096-b424-5eb21381e1b0}</MetaDataID>
        public abstract void CreateUnlinkAllObjectCommand(PersistenceLayerRunTime.StorageInstanceAgent deletedOutStorageInstanceRef, MetaDataRepository.AssociationEnd AssociationEnd, MetaDataRepository.StorageCell LinkedObjectsStorageCell);
        #endregion



        /// <summary>
        /// This method creates and return a StorageInstanceRef.
        /// If the objectID is null the the storage instance reference is temporary object
        /// If the objectID parameter is not null the storage instance reference object 
        /// will be created for storage instance which goes from passive mode to operative mode.
        /// </summary>
        /// <param name="memoryInstance">
        /// This parameter define the memory instance which correspond to the storage instance and
        /// express the operative mode of object.
        /// The parameter must be not null. 
        /// </param>
        /// <param name="objectID">
        /// This parameter defines the identity of object in data base.
        /// If the value of parameter is null the the storage instance will be
        /// created from new storage instance command.
        /// </param>
        /// <MetaDataID>{ADF2E449-7927-4134-93B6-233056B07E5C}</MetaDataID>
        public abstract StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID);
        /// <MetaDataID>{65acd1ed-fab9-4587-8f86-0ab5c8c9099b}</MetaDataID>
        public abstract StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, PersistenceLayer.ObjectID objectID, MetaDataRepository.StorageCell storageCell);



        //TODO: Αυτός ο μηχανισμός διασφάλησης θα πρέπει να αλλάξει 
        /// <MetaDataID>{062A00CC-2629-4ABD-B0E5-746BDA34EC9E}</MetaDataID>
        public abstract void AbortChanges(TransactionContext theTransaction);
        /// <MetaDataID>{9EE22A52-93E4-4481-96C7-B5EA560709C9}</MetaDataID>
        public abstract void CommitChanges(TransactionContext theTransaction);

        /// <MetaDataID>{c860c7bf-d34f-4e51-b01f-af88028d8387}</MetaDataID>
        public abstract void PrepareForChanges(TransactionContext theTransaction);
        /// <MetaDataID>{01891C34-AEA2-4610-A3D1-9D7A089DD6B1}</MetaDataID>
        public abstract void BeginChanges(TransactionContext theTransaction);
        /// <MetaDataID>{5549A370-A8F9-486F-9289-359F5F128727}</MetaDataID>
        public abstract void MakeChangesDurable(TransactionContext theTransaction);


        /// <MetaDataID>{41A615CB-4774-4887-A613-DC843C7BFD52}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.PersistenceLayerRunTime.MemoryInstanceCollection _OperativeObjectCollections;

        ///<summary>
        ///This property defines an object collection with all operative objects of storage. 
        ///</summary>
        /// <MetaDataID>{079CBCE6-8101-4734-9DD6-D25510E904D1}</MetaDataID>
        public virtual MemoryInstanceCollection OperativeObjectCollections
        {
            get
            {
                if (_OperativeObjectCollections == null)
                {
                    _OperativeObjectCollections = new MemoryInstanceCollection(this);

                }
                return _OperativeObjectCollections;
            }
        }
        /// <summary></summary>
        /// <MetaDataID>{5CFE0FE5-90A4-412A-A393-D859D3256318}</MetaDataID>
        public ObjectStorage()
        {
        }
        /// <MetaDataID>{DD7E84C7-1126-4DAE-9BBE-DFE643CFAA60}</MetaDataID>
        protected void OnObjectStorageChanged()
        {

            if (ObjectStorageChanged != null)
                ObjectStorageChanged(this);
        }

        #region IPersistentObjectLifeTimeController Members

    
        /// <MetaDataID>{E1237AAE-2BC1-4A80-8C28-3161DD1A8AB2}</MetaDataID>
        

        #endregion
         public object GetObjectFromStorageCell(MetaDataRepository.StorageCell storageCell, PersistenceLayer.ObjectID objectID)
        {

            ObjectQuery query = new ObjectQuery(storageCell);
            var _object = query.GetObject(objectID);
            return _object;
        }
        class ObjectQuery : ObjectsContextQuery
        {
            MetaDataRepository.StorageCell StorageCell;
            public ObjectQuery(MetaDataRepository.StorageCell storageCell)
            : base(new OOAdvantech.Collections.Generic.Dictionary<string, object>())
            {
                StorageCell = storageCell;
            }

            public object GetObject(PersistenceLayer.ObjectID objectID)
            {
                MetaDataRepository.ObjectQueryLanguage.DataNode storageCellDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(this);
                storageCellDataNode.AssignedMetaObject = MetaDataRepository.Classifier.GetClassifier(StorageCell.Type.GetExtensionMetaObject<System.Type>());
                storageCellDataNode.Name = StorageCell.Type.Name;
                DataTrees.Add(storageCellDataNode);

                string errors = null;
                BuildDataNodeTree(ref errors);
                ObjectsContext = PersistenceLayer.ObjectStorage.OpenStorage(StorageCell.StorageName, StorageCell.StorageLocation, StorageCell.StorageType);
                AddSelectListItem(storageCellDataNode);
                OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell> lookUpStorageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                lookUpStorageCells.Add(StorageCell);

                storageCellDataNode.DataSource = new StorageDataSource(storageCellDataNode, GetDataLoadersMetaData(lookUpStorageCells, storageCellDataNode));


                SearchTerm searchTerm = new SearchTerm();

                OOAdvantech.Collections.Generic.List<SearchTerm> searchTerms = new OOAdvantech.Collections.Generic.List<SearchTerm>();
                searchTerms.Add(searchTerm);

                SearchFactor searchFactor = new SearchFactor();
                DataNode searchExpressionDataNode = storageCellDataNode;
                ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                comparisonTerms[0] = new ObjectComparisonTerm(searchExpressionDataNode, this);

                object constantValue = objectID;
                string parameterName = "p" + storageCellDataNode.GetHashCode().ToString();
                Parameters.Add(parameterName, constantValue);
                comparisonTerms[1] = new ParameterComparisonTerm(parameterName, this);
                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, comparisonTerms, this, true, searchFactor);
                searchTerm.AddSearchFactor(searchFactor);
                storageCellDataNode.AddSearchCondition(new SearchCondition(searchTerms, this));

                var objectContextReference = new QueryResultObjectContextReference();
                objectContextReference.ObjectQueryContext = this;
                QueryResultType = new QueryResultType(storageCellDataNode, objectContextReference);
                SinglePart relatedObjectMember = new SinglePart(storageCellDataNode, "storageCellObject", QueryResultType);
                QueryResultType.AddMember(relatedObjectMember);
                QueryResultType.DataFilter = storageCellDataNode.SearchCondition;

                //Distribute();
                LoadData();

                foreach (CompositeRowData rowData in QueryResultType.DataLoader)
                    return (relatedObjectMember as QueryResultPart).GetValue(rowData);

                //releatedObjects.Add((relatedObjectMember as QueryResultPart).GetValue(rowData));
                //foreach (System.Data.DataRow dataRow in relatedObjectDataNode.DataSource.DataTable.Rows)
                //    releatedObjects.Add(dataRow[relatedObjectDataNode.DataSource.ObjectIndex]);
                return null;
            }
            public override OOAdvantech.Collections.Generic.List<DataNode> SelectListItems
            {
                get
                {
                    return _SelectListItems;
                }
            }

        }

        /// <MetaDataID>{d201a826-e693-4b75-9ee3-60b6a0a1d922}</MetaDataID>
        public abstract void CreateBuildContainedObjectIndiciesCommand(OOAdvantech.PersistenceLayerRunTime.IndexedCollection collection);

        /// <MetaDataID>{92939cc4-ca3e-44ea-a7c9-c714b14c023c}</MetaDataID>
        protected System.Collections.Generic.List<MetaDataRepository.RelatedStorageCell> GetLinkedStorageCellsFromObjectsUnderTransaction(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> storageCells)
        {
            Dictionary<OOAdvantech.MetaDataRepository.StorageCell, OOAdvantech.MetaDataRepository.RelatedStorageCell> linkedStorageCells = new Dictionary<OOAdvantech.MetaDataRepository.StorageCell, OOAdvantech.MetaDataRepository.RelatedStorageCell>();

            if (Transactions.Transaction.Current == null)
                return new List<OOAdvantech.MetaDataRepository.RelatedStorageCell>();


            foreach (object memoryInstance in TransactionContext.CurrentTransactionContext.EnlistObjects)
            {
                StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(memoryInstance) as StorageInstanceRef;
                if (storageInstanceRef != null && storageInstanceRef.ObjectStorage == this && storageCells.Contains(storageInstanceRef.StorageInstanceSet))
                {
                    foreach (ObjectsLink objectsLink in storageInstanceRef.GetRelationChanges(associationEnd, valueTypePath))
                    {
                        //if (objectsLink.RelationObject!=null&& objectsLink.RelationObject.RealStorageInstanceRef == storageInstanceRef)
                        //{
                        //    MetaDataRepository.StorageCell relatedStorageCell = objectsLink.RoleB.GetStorageInstanceSet(this);
                        //    MetaDataRepository.StorageCell rootStorageCell = objectsLink.RoleA.GetStorageInstanceSet(this);
                        //    if (!linkedStorageCells.ContainsKey(relatedStorageCell))
                        //    {
                        //        bool withRelationTable = false;
                        //        if (associationEnd != null &&
                        //           (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                        //           associationEnd.Association.Specializations.Count > 0) &&
                        //           associationEnd.Association != relatedStorageCell.Type.ClassHierarchyLinkAssociation)
                        //        {
                        //            withRelationTable = true;
                        //        }
                        //        if (relatedStorageCell.StorageIdentity != rootStorageCell.StorageIdentity)
                        //            withRelationTable = true;

                        //        linkedStorageCells.Add(relatedStorageCell, new OOAdvantech.MetaDataRepository.RelatedStorageCell(relatedStorageCell, rootStorageCell, withRelationTable));
                        //        if (relatedStorageCell.StorageIdentity != StorageIdentity)
                        //        {
                        //            MetaDataRepository.StorageCellReference storageCellReference = GetStorageCellReference(relatedStorageCell);
                        //            if (storageCellReference == null)
                        //                storageCellReference = new MetaDataRepository.OnFlyStorageCellReference(relatedStorageCell, StorageMetaData as MetaDataRepository.Storage);
                        //            linkedStorageCells.Add(storageCellReference, new OOAdvantech.MetaDataRepository.RelatedStorageCell(storageCellReference, rootStorageCell, withRelationTable));
                        //        }
                        //    }
                        //}
                        //else 
                        if (associationEnd.IsRoleA)
                        {
                            MetaDataRepository.StorageCell relatedStorageCell = null;
                            MetaDataRepository.StorageCell rootStorageCell = null;
                            
                            if (associationEnd.Association.LinkClass == null)
                            {
                                relatedStorageCell = objectsLink.RoleA.GetStorageInstanceSet(this);
                                rootStorageCell = storageInstanceRef.StorageInstanceSet;// objectsLink.RoleB.GetStorageInstanceSet(this);
                            }
                            else if (associationEnd.Association.LinkClass != null && storageInstanceRef.Class.ClassHierarchyLinkAssociation == associationEnd.Association)
                            {
                                if (objectsLink.RoleA == null)
                                    continue;

                                ///TODO να γραφτεί Test case που το relation object θα είναι out of process 
                                relatedStorageCell = objectsLink.RoleA.GetStorageInstanceSet(this);
                                rootStorageCell = storageInstanceRef.StorageInstanceSet;// objectsLink.RoleB.GetStorageInstanceSet(this);
                            }
                            else
                            {
                                ///TODO να γραφτεί Test case που το relation object θα είναι out of process 
                                relatedStorageCell = objectsLink.RelationObject.GetStorageInstanceSet(this);
                                rootStorageCell = storageInstanceRef.StorageInstanceSet;// objectsLink.RoleB.GetStorageInstanceSet(this);
                            }



                            if (!linkedStorageCells.ContainsKey(relatedStorageCell))
                            {
                                bool withRelationTable = false;
                                if (associationEnd != null &&
                                   (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                                   associationEnd.Association.Specializations.Count > 0) &&
                                   associationEnd.Association.LinkClass == null)
                                {
                                    withRelationTable = true;
                                }
                                if (relatedStorageCell.StorageIdentity != rootStorageCell.StorageIdentity)
                                    withRelationTable = true;
                                linkedStorageCells.Add(relatedStorageCell, new OOAdvantech.MetaDataRepository.RelatedStorageCell(relatedStorageCell, rootStorageCell, objectsLink.Association.RoleA.Identity.ToString(), withRelationTable));
                                if (relatedStorageCell.StorageIdentity != StorageIdentity)
                                {
                                    MetaDataRepository.StorageCellReference storageCellReference = GetStorageCellReference(relatedStorageCell);
                                    if (storageCellReference == null)
                                        storageCellReference = new MetaDataRepository.OnFlyStorageCellReference(relatedStorageCell, StorageMetaData as MetaDataRepository.Storage);
                                    linkedStorageCells.Add(storageCellReference, new OOAdvantech.MetaDataRepository.RelatedStorageCell(storageCellReference, rootStorageCell, objectsLink.Association.RoleA.Identity.ToString(), withRelationTable));
                                }
                            }

                        }
                        else
                        {


                            MetaDataRepository.StorageCell relatedStorageCell = null;
                            MetaDataRepository.StorageCell rootStorageCell = null;
                            if (associationEnd.Association.LinkClass == null)
                            {
                                relatedStorageCell = objectsLink.RoleB.GetStorageInstanceSet(this);
                                rootStorageCell = storageInstanceRef.StorageInstanceSet;// objectsLink.RoleB.GetStorageInstanceSet(this);
                            }
                            else if (associationEnd.Association.LinkClass != null && storageInstanceRef.Class.ClassHierarchyLinkAssociation == associationEnd.Association)
                            {
                                if (objectsLink.RoleB == null)
                                    continue;
                                ///TODO να γραφτεί Test case που το relation object θα είναι out of process 
                                relatedStorageCell = objectsLink.RoleB.GetStorageInstanceSet(this);
                                rootStorageCell = storageInstanceRef.StorageInstanceSet;// objectsLink.RoleB.GetStorageInstanceSet(this);
                            }
                            else
                            {
                                ///TODO να γραφτεί Test case που το relation object θα είναι out of process 
                                relatedStorageCell = objectsLink.RelationObject.GetStorageInstanceSet(this);
                                rootStorageCell = storageInstanceRef.StorageInstanceSet;// objectsLink.RoleB.GetStorageInstanceSet(this);
                            }

                            //MetaDataRepository.StorageCell rootStorageCell = objectsLink.RoleA.GetStorageInstanceSet(this);
                            if (!linkedStorageCells.ContainsKey(relatedStorageCell))
                            {
                                bool withRelationTable = false;
                                if (associationEnd != null &&
                                   (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                                   associationEnd.Association.Specializations.Count > 0) &&
                                   associationEnd.Association.LinkClass == null)
                                {
                                    withRelationTable = true;
                                }
                                if (relatedStorageCell.StorageIdentity != rootStorageCell.StorageIdentity)
                                    withRelationTable = true;

                                linkedStorageCells.Add(relatedStorageCell, new OOAdvantech.MetaDataRepository.RelatedStorageCell(relatedStorageCell, rootStorageCell, objectsLink.Association.RoleB.Identity.ToString(), withRelationTable));
                                if (relatedStorageCell.StorageIdentity != StorageIdentity)
                                {
                                    MetaDataRepository.StorageCellReference storageCellReference = GetStorageCellReference(relatedStorageCell);
                                    if (storageCellReference == null)
                                        storageCellReference = new MetaDataRepository.OnFlyStorageCellReference(relatedStorageCell, StorageMetaData as MetaDataRepository.Storage);
                                    linkedStorageCells.Add(storageCellReference, new OOAdvantech.MetaDataRepository.RelatedStorageCell(storageCellReference, rootStorageCell, objectsLink.Association.RoleB.Identity.ToString(), withRelationTable));
                                }
                            }

                        }

                    }
                }
            }
            //foreach (MetaDataRepository.StorageCell storageCell in storageCells)
            //{
            //    MetaDataRepository.Classifier storageCellClassifier = storageCell.Type;
            //    if (!(storageCellClassifier.ImplementationUnit is OOAdvantech.DotNetMetaDataRepository.Assembly))
            //        storageCellClassifier = MetaDataRepository.Classifier.GetClassifier(ModulePublisher.ClassRepository.GetType(storageCell.Type.FullName, storageCell.Type.ImplementationUnit.Identity.ToString()));
            //    ////if (classifier == null)
            //    //    classifier = storageCellClassifier;
            //    //else if (storageCellClassifier.IsA(classifier))
            //    //    classifier = storageCellClassifier;





            //    foreach (StorageInstanceRef storageInstanceRef in StorageInstanceRef.GetNewObjectsUnderTransaction(storageCellClassifier))
            //    {
            //        if (storageInstanceRef.ObjectStorage == this)
            //        {

            //        }
            //    }
            //}
            

            return new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.RelatedStorageCell>(linkedStorageCells.Values);
        }

        abstract internal protected void UpdateOperativeObjects();

        /// <MetaDataID>{58845493-8ff9-440d-b716-bee430465f3d}</MetaDataID>
        internal protected abstract OOAdvantech.MetaDataRepository.StorageCellReference GetStorageCellReference(OOAdvantech.MetaDataRepository.StorageCell storageCell);
    }
}
