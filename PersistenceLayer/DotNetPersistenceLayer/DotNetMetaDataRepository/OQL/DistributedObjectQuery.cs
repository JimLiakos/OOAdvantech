namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{

    using OOAdvantech.DotNetMetaDataRepository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PartialRelationIdentity = System.String;
    delegate void LoadDataLocallyHandeler();

    /// <MetaDataID>{52c3d9df-6a9e-4f80-88ae-283a0d8183a1}</MetaDataID>
    public class DistributedObjectQuery : ObjectQuery
    {

        /// <summary>
        /// All objects links are restored in ObjectQuery main thread.
        /// </summary>
        static SerializeTaskScheduler ObjectQueryMainThreadTaskScheduler = new SerializeTaskScheduler();

        static DistributedObjectQuery()
        {
            ObjectQueryMainThreadTaskScheduler = new SerializeTaskScheduler();
            ObjectQueryMainThreadTaskScheduler.RunAsync();
        }

        internal override bool UseStorageIdintityInTablesRelations
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{1939af8a-7779-4e81-9062-107b2830c0a5}</MetaDataID>
        public bool QueryResultLoadedLocaly
        {
            get
            {
                if (QueryResultType == null)
                    return false;
                else
                    return QueryResultType.CanLoadDataLocal;
            }
        }

        /// <exclude>Excluded</exclude>
        string _Identity;
        /// <MetaDataID>{63f0a2e2-c663-4e38-ba31-003a01b8018d}</MetaDataID>
        public string Identity
        {
            get
            {
                if (ObjectsContext != null)
                    return (ObjectsContext as ObjectsContext).Identity;
                else
                {
                    if (_Identity == null)
                        _Identity = System.Guid.NewGuid().ToString();
                    return _Identity;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ObjectsContextIdentity;
        /// <MetaDataID>{63f0a2e2-c663-4e38-ba31-003a01b8018d}</MetaDataID>
        public string ObjectsContextIdentity
        {
            get
            {
                if (_ObjectsContextIdentity == null)
                {
                    if (ObjectsContext != null)
                        _ObjectsContextIdentity = (ObjectsContext as ObjectsContext).Identity;
                }
                return _ObjectsContextIdentity;
            }
        }

        /// <MetaDataID>{ee05f867-4589-4aea-827c-6a26bce2e750}</MetaDataID>
        public Dictionary<Uri, List<ObjectsDataRetriever>> BuildDataRetrievingModel(List<ObjectsDataRetriever> objectsDataRetrievers)
        {

            foreach (var objectsDataRetriever in objectsDataRetrievers)
            {
                DataNode dataNode = DataTrees[0].GetDataNode(objectsDataRetriever.ObjectsDataNode.Identity);
                (dataNode.DataSource.DataLoaders[Identity] as RootObjectDataLoader).LoadObjects(objectsDataRetriever.Objects);
            }
            return null;
        }
        /// <MetaDataID>{63d16bd1-2a5f-45d6-81ca-a330405bc365}</MetaDataID>
        public override string ToString()
        {
            if (ObjectsContext != null)
                return ObjectsContext.ToString();
            return base.ToString();
        }
        /// <MetaDataID>{c608ca45-35a5-4b51-b22a-f6de9baaadf9}</MetaDataID>
        bool ObjectRelationLinksLoaded;
        /// <MetaDataID>{61233bac-b25a-474c-897c-87eb4c3a8b73}</MetaDataID>
        public void LoadData(OOAdvantech.Collections.Generic.Dictionary<string, DistributedObjectQuery> distributedObjectQueries)
        {
            try
            {
                System.Collections.Generic.List<DataLoader> dataLoaders = new System.Collections.Generic.List<DataLoader>();
                foreach (DataLoader dataLoader in DataLoaders.Values)
                    dataLoaders.Add(dataLoader);
                dataLoaders.Sort(new DataLoader.DataLoaderSorting());

                Dictionary<DataLoader, Task> retrieveFromStorageTasks = new Dictionary<DataLoader, Task>();

                foreach (DataLoader dataloader in dataLoaders)
                {
                    if (!dataloader.DataLoaded)
                    {
                        dataloader.RetrieveFromStorage();

                        foreach (var criterion in (dataloader as StorageDataLoader).GlobalResolveCriterions)
                            criterion.Applied = false;
                        foreach (var criterion in (dataloader as StorageDataLoader).LocalResolvedCriterions.Keys)
                            criterion.Applied = true;
                    }
                }

                //foreach (DataLoader dataloader in dataLoaders)
                //{
                //    if (!dataloader.DataLoaded)
                //    {
                //        Task task = new Task(() => dataloader.RetrieveFromStorage());
                //        retrieveFromStorageTasks[dataloader] = task;
                //        task.Start();
                //    }
                //}
                //Task.WaitAll(retrieveFromStorageTasks.Values.ToArray());


                ObjectQueryDataSet = DataSource.DataObjectsInstantiator.CreateDataSet();
                foreach (DataLoader dataloader in dataLoaders)
                {
                    if (dataloader.DataNode.ValueTypePath.Count > 0 || dataloader.Data == null)
                        continue;
                    if (dataloader.Data.DataSet != null)
                        (dataloader.Data.DataSet as IDataSet).RemoveTable(dataloader.Data);
                    (ObjectQueryDataSet as IDataSet).AddTable(dataloader.Data);
                }

            }
            finally
            {
                foreach (DataLoader dataLoader in DataLoaders.Values)
                {
                    try
                    {
                        dataLoader.OnAllQueryDataLoaded();
                    }
                    catch (Exception error)
                    {
                    }
                }
                MemoryObjectsContext.RemoveAllQueryMemoryContext(QueryIdentity);
            }
        }


        /// <MetaDataID>{8c1b91db-e332-4d90-982b-c0a66ad073b7}</MetaDataID>
        public OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader GetDataLoader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        {
            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                return GetDataLoader(dataNode.ParentDataNode);
            MetaDataRepository.ObjectQueryLanguage.DataLoader dataLoader = null;
            if (dataNode.DataSource != null)
                dataNode.DataSource.DataLoaders.TryGetValue((ObjectsContext as ObjectsContext).Identity, out dataLoader);
            //if (dataNode.ValueTypePath.Count > 0)
            //{
            //   dataLoader= GetDataLoader(dataNode.ParentDataNode);
            //   if (dataLoader != null)
            //       dataNode.DataSource.DataLoaders[(ObjectsContext as ObjectsContext).Identity] = dataLoader;
            //}
            return dataLoader;
        }



        ///// <MetaDataID>{8179db47-3fb0-4b15-bf03-bee4861c0a3c}</MetaDataID>
        //protected internal override void LoadData()
        //{
        //    //System.Threading.ThreadPool
        //    try
        //    {
        //        bool localLoad = QueryResultType.CanLoadDataLocal;
        //        System.Collections.Generic.List<DataLoader> dataLoaders = new System.Collections.Generic.List<DataLoader>();
        //        foreach (DataLoader dataLoader in DataLoaders.Values)
        //            dataLoaders.Add(dataLoader);
        //        dataLoaders.Sort(new DataLoader.DataLoaderSorting());
        //        bool filterData = false;
        //        foreach (DataLoader dataloader in dataLoaders)
        //        {
        //            dataloader.RetrieveFromStorage();
        //            if ((dataloader as StorageDataLoader).LocalOnMemoryResolvedCriterions.Count > 0)
        //                filterData = true;
        //        }

        //        //ToDo θα πρέπει να χτίζωνται μόνο οι σχέσεις που ειναι απαραίτητες
        //        ObjectQueryData = new System.Data.DataSet();
        //        foreach (DataLoader dataloader in dataLoaders)
        //        {
        //            if (dataloader.DataNode.ValueTypePath.Count > 0 || dataloader.Data == null)
        //                continue;
        //            if (dataloader.Data.DataSet != null)
        //                dataloader.Data.DataSet.Tables.Remove(dataloader.Data);
        //            ObjectQueryData.Tables.Add(dataloader.Data);
        //        }
        //        DataTrees[0].BuildTablesRelations();
        //        if (filterData)
        //        {
        //            FilterData();

        //            ObjectQueryData.Relations.Clear();
        //            ObjectQueryData.Tables.Clear();
        //        }
        //    }
        //    finally
        //    {


        //        foreach (DataLoader dataLoader in DataLoaders.Values)
        //        {
        //            try
        //            {
        //                dataLoader.OnAllQueryDataLoaded();
        //            }
        //            catch (Exception error)
        //            {
        //            }
        //        }
        //    }
        //}

        /// <MetaDataID>{6c8bdd14-a517-4313-9b13-a597a466d084}</MetaDataID>
        internal System.Collections.Generic.List<string> QueryStorageIdentities;

        /// <summary>
        /// This method checks all DataTree DataNodes and look for prefetching members.
        /// Update DataTree metadata to resolve the members prefetching.
        /// Returns prefetching metadata in master query and dispatches them across distributed queries where participate for the query resolve
        /// </summary>
        /// <param name="queryStorageIdentities"></param>
        /// <returns>
        /// Returns prefetching metadata in master query
        /// </returns>
        /// <MetaDataID>{e36863a7-dd06-4633-aca4-6b01b29d7dcb}</MetaDataID>
        public Collections.Generic.List<DataNodeRelatedDataLoadersMetadata> RetrieveDataNodesPrefatchingData(ref System.Collections.Generic.List<string> queryStorageIdentities)
        {
            #region Retrieve prefetching data node

            Collections.Generic.List<DataNodeRelatedDataLoadersMetadata> dataNodesPrefetchingData = new Collections.Generic.List<DataNodeRelatedDataLoadersMetadata>();
            string errorOutput = "";
            foreach (DataNode dataNode in DataTrees)
            {
                dataNode.Validate(ref errorOutput);
                dataNodesPrefetchingData.AddRange(dataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
            }

            #endregion

            #region Create local storage dataLoaders

            foreach (DataNodeRelatedDataLoadersMetadata dataNodePrefetchingData in dataNodesPrefetchingData)
            {
                foreach (System.Collections.Generic.KeyValuePair<DataNode, Dictionary<string, DataLoaderMetadata>> entry in dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata)
                {
                    foreach (DataLoaderMetadata dataLoaderMetadata in entry.Value.Values)
                    {
                        if (dataLoaderMetadata.ObjectsContextIdentity == (ObjectsContext as ObjectsContext).Identity && !entry.Key.DataSource.DataLoaders.ContainsKey(dataLoaderMetadata.ObjectsContextIdentity))
                        {
                            entry.Key.ObjectQuery = this;
                            entry.Key.DataSource.DataLoaders.Add(dataLoaderMetadata.ObjectsContextIdentity, ObjectsContext.CreateDataLoader(entry.Key, dataLoaderMetadata));
                        }
                    }
                }
            }

            #endregion

            QueryStorageIdentities = queryStorageIdentities;
            return dataNodesPrefetchingData;
        }

        /// <MetaDataID>{35e4d470-e79f-496e-b19b-90088e1c9242}</MetaDataID>
        protected void GetObjectTypeDataNodes(DataNode dataNode, System.Collections.Generic.List<DataNode> objectTyeDataNodes)
        {
            if (dataNode.Type == DataNode.DataNodeType.Object)
            {
                objectTyeDataNodes.Add(dataNode);
                return;
            }
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                GetObjectTypeDataNodes(subDataNode, objectTyeDataNodes);


        }

        /// <MetaDataID>{c8924fdc-c469-4998-abc7-011c614ed121}</MetaDataID>
        public readonly MetaDataRepository.ObjectQueryLanguage.IObjectQueryPartialResolver ObjectsContext;
        /// <MetaDataID>{f53ced1d-adbe-4110-9395-4591e36919c3}</MetaDataID>
        public DistributedObjectQuery(Guid queryIdentity,
            OOAdvantech.Collections.Generic.List<DataNode> dataTrees,
            QueryResultType queryResult,
            OOAdvantech.Collections.Generic.List<DataNode> selectListItems,
            MetaDataRepository.ObjectQueryLanguage.IObjectQueryPartialResolver objectsContext,
            OOAdvantech.Collections.Generic.Dictionary<Guid, MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> dataLoadersMetadata,
            OOAdvantech.Collections.Generic.Dictionary<string, object> parameters,
            System.Collections.Generic.List<string> usedAliases,
            System.Collections.Generic.List<string> queryStorageIdentities)
            : base(queryIdentity)
        {
            foreach (DataNode dataNode in selectListItems)
                dataNode.ParticipateInSelectClause = true;

            QueryStorageIdentities = queryStorageIdentities;
            ObjectsContext = objectsContext;
            UsedAliases = usedAliases;
            Parameters = parameters;
            DataTrees = dataTrees;
            QueryResultType = queryResult;
            _SelectListItems = selectListItems;

            #region Retrieve metadata for DataNodes tree

            string errorOutput = "";
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataTrees)
            {
                dataNode.ObjectQuery = this;
                dataNode.Validate(ref errorOutput);
                if (!string.IsNullOrEmpty(errorOutput))
                    throw new System.Exception(errorOutput);
            }

            #endregion

            #region Creates dataLoaders


            System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, StorageCellReference>> storageCells = new Dictionary<string, Dictionary<int, StorageCellReference>>();

            if (ObjectsContext is PersistenceLayer.ObjectStorage)
                foreach (MetaDataRepository.MetaObject metaobject in ((ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData as MetaDataRepository.Namespace).OwnedElements)
                {

                    if ((metaobject is MetaDataRepository.StorageCellReference))
                    {
                        StorageCell storageCell = metaobject as StorageCell;
                        if (!storageCells.ContainsKey(storageCell.StorageIdentity))
                            storageCells[storageCell.StorageIdentity] = new Dictionary<int, StorageCellReference>();
                        storageCells[storageCell.StorageIdentity][storageCell.SerialNumber] = storageCell as MetaDataRepository.StorageCellReference;
                    }
                }


            foreach (System.Collections.Generic.KeyValuePair<Guid, MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> entry in dataLoadersMetadata)
            {
                DataLoaderMetadata dataLoaderMetadata = new DataLoaderMetadata(entry.Value, this);
                OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataSource dataSource = GetDataSource(entry.Key);
                dataSource.DataNode.ObjectQuery = this;
                if (dataLoaderMetadata.StorageCellReferencesMetaData.Count > 0)
                {
                    foreach (MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData storageCellReferenceMetaData in dataLoaderMetadata.StorageCellReferencesMetaData)
                    {
                        if (storageCells.ContainsKey(storageCellReferenceMetaData.StorageIdentity) && storageCells[storageCellReferenceMetaData.StorageIdentity].ContainsKey(storageCellReferenceMetaData.SerialNumber))
                        {
                            MetaDataRepository.StorageCellReference storageCellReference = storageCells[storageCellReferenceMetaData.StorageIdentity][storageCellReferenceMetaData.SerialNumber];
                            dataLoaderMetadata.AddStorageCell(storageCellReference);
                        }
                        else
                        {
                            var storageCellReference = new MetaDataRepository.OnFlyStorageCellReference(storageCellReferenceMetaData);
                            dataLoaderMetadata.AddStorageCell(storageCellReference);
                        }
                    }
                }


                dataSource.DataLoaders.Add(dataLoaderMetadata.ObjectsContextIdentity, objectsContext.CreateDataLoader(dataSource.DataNode, dataLoaderMetadata));
            }

            #endregion


            #region Resolve execution model for aggregate function data nodes

            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataTrees)
                dataNode.ResolveAggregateFunctionsExecutionModel();

            #endregion


            //foreach (DataLoader dataLoader in DataLoaders.Values)
            //    dataLoader.BeforeLoadData();

        }
        /// <MetaDataID>{38e7dd94-e189-49ad-940b-c2d39922ce99}</MetaDataID>
        public DistributedObjectQuery(Guid queryIdentity,
                                      OOAdvantech.Collections.Generic.List<DataNode> dataTrees,
                                      QueryResultType queryResult,
                                      OOAdvantech.Collections.Generic.List<DataNode> selectListItems,
                                      OOAdvantech.Collections.Generic.Dictionary<string, object> parameters,
                                       System.Collections.Generic.List<string> usedAliases)
            : base(queryIdentity)
        {
            foreach (DataNode dataNode in selectListItems)
                dataNode.ParticipateInSelectClause = true;
            UsedAliases = usedAliases;
            Parameters = parameters;
            DataTrees = dataTrees;
            DataTrees[0].ObjectQuery = this;

            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode dataNode in DataTrees)
            {

                string errorOutput = "";
                dataNode.ObjectQuery = this;
                dataNode.Validate(ref errorOutput);
                if (!string.IsNullOrEmpty(errorOutput))
                    throw new System.Exception(errorOutput);
                BuildDataLoaders(dataNode);
            }


        }

        /// <MetaDataID>{a3ef6902-6fbc-4f17-b9c7-cec26435c795}</MetaDataID>
        private void BuildDataLoaders(DataNode dataNode)
        {
            if (dataNode.Type == DataNode.DataNodeType.Object)
            {

                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                    BuildDataLoaders(subDataNode);
                dataNode.DataSource.DataLoaders[Identity] = new RootObjectDataLoader(dataNode);
            }
        }

        /// <MetaDataID>{69b23415-f48a-4c76-a76f-6d90659c7a93}</MetaDataID>
        public DataSource GetDataSource(Guid dataSourceIdentity)
        {
            foreach (DataNode dataNode in DataTrees)
            {
                DataSource dataSource = GetDataSource(dataSourceIdentity, dataNode);
                if (dataSource != null)
                    return dataSource;
            }
            return null;
        }

        /// <MetaDataID>{b70f293e-8848-4044-8d06-b794fac78f37}</MetaDataID>
        public DataNode GetDataNode(Guid dataDataNodeIdentity)
        {
            foreach (DataNode dataNode in DataTrees)
            {
                DataNode tempDataNode = dataNode.GetDataNode(dataDataNodeIdentity);
                if (tempDataNode != null)
                    return tempDataNode;
            }
            return null;
        }
        /// <MetaDataID>{99421314-c1e1-457a-93ec-ced1a69985e1}</MetaDataID>
        private DataSource GetDataSource(Guid dataSourceIdentity, DataNode dataNode)
        {
            if (dataNode.DataSource != null && dataNode.DataSource.Identity == dataSourceIdentity)
                return dataNode.DataSource;
            else
            {
                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                {
                    DataSource dataSource = GetDataSource(dataSourceIdentity, subDataNode);
                    if (dataSource != null)
                        return dataSource;
                }
                return null;
            }

        }

        /// <MetaDataID>{75b234a1-998d-4e08-8933-f68458124285}</MetaDataID>
        private void GetDataLoaders(DataNode dataNode, ref OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoader> dataLoaders)
        {
            if (dataNode.Type == DataNode.DataNodeType.Key || dataNode is DerivedDataNode)
                return;
            if (dataNode.DataSource != null)
            {
                // data source has only one data loader 
                foreach (System.Collections.Generic.KeyValuePair<string, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader> entry in dataNode.DataSource.DataLoaders)
                    if ((entry.Value.DataNode.DataSource as DataSource).HasInObjectContextData)
                        dataLoaders.Add(dataNode.DataSource.Identity, entry.Value);
            }

            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                GetDataLoaders(subDataNode, ref dataLoaders);

        }


        /// <MetaDataID>{ff1c77e0-67ed-44d6-a84b-3dedc050a7f2}</MetaDataID>
        public OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoader> DataLoaders
        {
            get
            {
                OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoader> dataLoaders = new OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoader>();
                foreach (DataNode dataNode in DataTrees)
                    GetDataLoaders(dataNode, ref dataLoaders);
                return dataLoaders;

            }
        }

        /// <MetaDataID>{4d7bb0aa-84be-4094-8ca1-81714f42254d}</MetaDataID>
        internal OOAdvantech.Collections.Generic.Dictionary<string, DistributedObjectQuery> DistributedObjectQueries;
        /// <MetaDataID>{df75134d-4ae8-4d76-9cfe-afa307de2083}</MetaDataID>
        public void LoadObjectRelationLinks(OOAdvantech.Collections.Generic.Dictionary<string, DistributedObjectQuery> distributedObjectQueries)
        {
            if (ObjectRelationLinksLoaded)
                return;
            DistributedObjectQueries = distributedObjectQueries;


            //The object relation links must be loaded in synchronized context
            //In case multiple threads force the relation resolvers to load the related objects there is a possibility for deadlock.
            //This happens when we have association with both association end navigable.The first thread access the first "relation resolver"
            //locks and wait the objects storage.The second  thread access the opposite "relation resolver" and wait the objects storage.when
            //the query finish with the objects retrieve, restore the objects links.the deadlocks are unavoidable.
            //If  all objects links restored in one thread  isn't necessary for synchronization locks of "relation resolvers" without lock there aren't deadlocks


            try
            {
                DataTrees[0].LoadObjectRelationLinks();
            }
            catch (Exception error)
            {
                throw;
            }
            //var task = ObjectQueryMainThreadTaskScheduler.AddTask(() =>
            //{
            //    try
            //    {
            //        DataTrees[0].LoadObjectRelationLinks();
            //        return true;
            //    }
            //    catch (Exception error)
            //    {
            //        throw;
            //    }
            //});
            //task.Wait();
            ObjectRelationLinksLoaded = true;
        }


        /// <exclude>Excluded</exclude>
        Dictionary<string, Dictionary<int, StorageCellReference>> _StorageCellReferences;
        /// <MetaDataID>{c99a7bc5-d23a-4f23-93d7-8eac84e66eb4}</MetaDataID>
        Dictionary<string, Dictionary<int, StorageCellReference>> StorageCellReferences
        {
            get
            {
                if (_StorageCellReferences == null)
                {
                    _StorageCellReferences = new Dictionary<string, Dictionary<int, StorageCellReference>>();
                    foreach (MetaDataRepository.MetaObject metaobject in ((ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData as MetaDataRepository.Namespace).OwnedElements)
                    {
                        if ((metaobject is MetaDataRepository.StorageCellReference))
                        {
                            StorageCell storageCell = metaobject as StorageCell;
                            if (!_StorageCellReferences.ContainsKey(storageCell.StorageIdentity))
                                _StorageCellReferences[storageCell.StorageIdentity] = new Dictionary<int, StorageCellReference>();
                            _StorageCellReferences[storageCell.StorageIdentity][storageCell.SerialNumber] = storageCell as MetaDataRepository.StorageCellReference;
                        }
                    }
                }
                return _StorageCellReferences;
            }
        }

        /// <summary>Notify DistributedQuery for Master Query changes</summary>
        /// <param name="dataTrees">
        /// Defines a list with datatrees header DataNodes
        /// </param>
        /// <param name="selectListItems">
        /// Defines a collection with DataNodes which participate in selection list.
        /// </param>
        /// <param name="dataLoadersMetadata">
        /// Defines all DataLoadersMetadata for distributed query.
        /// The distributed query create a new DataLoader if doesn't exist for each DataLoaderMetaData, 
        /// or synchronize the already exist DataLoader.
        /// </param>
        /// <MetaDataID>{446ebef3-85e1-4c85-9991-9947a33eaa94}</MetaDataID>
        internal void NotifyMasterQueryMetaDataChange(OOAdvantech.Collections.Generic.List<DataNode> dataTrees, OOAdvantech.Collections.Generic.List<DataNode> selectListItems, OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoaderMetadata> dataLoadersMetadata)
        {
            foreach (DataNode dataNode in selectListItems)
                dataNode.ParticipateInSelectClause = true;
            //System.Collections.Generic.List<DataNode> validateDataNodes = new List<DataNode>();
            //int maxDataTreeDeep = 0;
            Dictionary<Guid, DataSource> masterQueryDataSources = new Dictionary<Guid, DataSource>();
            foreach (System.Collections.Generic.KeyValuePair<Guid, DataLoaderMetadata> entry in dataLoadersMetadata)
            {
                DataSource dataSource = GetDataSource(entry.Key, dataTrees[0]);
                DataNode queryMananagerDataNode = dataSource.DataNode;

                masterQueryDataSources[entry.Key] = dataSource;

                #region Synchronize master-distribute query DataNodes state
                DataNode inStorageDataNode = DataTrees[0].GetDataNode(queryMananagerDataNode.Identity);
                if (queryMananagerDataNode.MembersFetchingObjectActivation)
                {

                    if (inStorageDataNode != null && !inStorageDataNode.MembersFetchingObjectActivation)
                    {
                        inStorageDataNode.MembersFetchingObjectActivation = true;
                        AddSelectListItem(inStorageDataNode);
                    }
                }
                foreach (var subDataNodeIdentity in queryMananagerDataNode.BackRouteMemberFetchingObjectActivation)
                {
                    if (!inStorageDataNode.BackRouteMemberFetchingObjectActivation.Contains(subDataNodeIdentity))
                    {
                        inStorageDataNode.BackRouteActivationMembersFetching(inStorageDataNode.GetDataNode(subDataNodeIdentity));
                        AddSelectListItem(inStorageDataNode);
                    }

                }
                if (queryMananagerDataNode.FilterNotActAsLoadConstraint)
                {
                    if (inStorageDataNode != null && !inStorageDataNode.FilterNotActAsLoadConstraint)
                        inStorageDataNode.FilterNotActAsLoadConstraint = true;
                }

                #endregion

            }

            while (dataLoadersMetadata.Count > 0)
            {
                foreach (Guid dataSourceIdentity in new List<Guid>(dataLoadersMetadata.Keys))
                {
                    StorageDataSource dataSource = masterQueryDataSources[dataSourceIdentity] as StorageDataSource;

                    DataLoaderMetadata dataLoaderMetadata = dataLoadersMetadata[dataSourceIdentity];

                    if (DataTrees[0].GetDataNode(dataSource.DataNode.Identity) == null)
                    {

                        if (DataTrees[0].GetDataNode(dataSource.DataNode.ParentDataNode.Identity) != null)
                        {
                            dataSource.DataNode.ParentDataNode = DataTrees[0].GetDataNode(dataSource.DataNode.ParentDataNode.Identity);

                            dataLoaderMetadata = new DataLoaderMetadata(dataLoaderMetadata, this);
                            (dataSource.DataNode.DataSource as StorageDataSource).DataLoadersMetadata[dataLoaderMetadata.ObjectsContextIdentity] = dataLoaderMetadata;
                            dataSource.DataLoaders.Add(dataLoaderMetadata.ObjectsContextIdentity, ObjectsContext.CreateDataLoader(dataSource.DataNode, dataLoaderMetadata));
                            dataSource.DataNode.ObjectQuery = this;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        dataSource = DataTrees[0].GetDataNode(dataSource.DataNode.Identity).DataSource as StorageDataSource;
                        dataLoaderMetadata = new DataLoaderMetadata(dataLoaderMetadata, this);
                        if (!dataSource.DataLoadersMetadata.ContainsKey(dataLoaderMetadata.ObjectsContextIdentity))
                        {
                            dataSource.DataLoadersMetadata[dataLoaderMetadata.ObjectsContextIdentity] = dataLoaderMetadata;
                            dataSource.DataLoaders.Add(dataLoaderMetadata.ObjectsContextIdentity, ObjectsContext.CreateDataLoader(dataSource.DataNode, dataLoaderMetadata));
                        }
                        else
                            dataSource.DataLoadersMetadata[dataLoaderMetadata.ObjectsContextIdentity].UpdateWithExtraMetadataInfo(dataLoaderMetadata);
                    }
                    if (dataLoaderMetadata.StorageCellReferencesMetaData.Count > 0)
                    {
                        foreach (MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData storageCellReferenceMetaData in dataLoaderMetadata.StorageCellReferencesMetaData)
                        {
                            MetaDataRepository.StorageCellReference storageCellReference = StorageCellReferences[storageCellReferenceMetaData.StorageIdentity][storageCellReferenceMetaData.SerialNumber];
                            dataLoaderMetadata.AddStorageCell(storageCellReference);
                        }
                    }
                    dataLoadersMetadata.Remove(dataSourceIdentity);

                    //foreach (var dataNode in new List<DataNode>(dataLoaderMetadata.DataNode.SubDataNodes))
                    //{
                    //    if (dataNode.ValueTypePath.Count > 0)
                    //    {
                    //        var valueTypeDataNode = DataTrees[0].GetDataNode(dataNode.Identity);
                    //        if (valueTypeDataNode == null && DataTrees[0].GetDataNode(dataNode.ParentDataNode.Identity) != null)
                    //        {
                    //            dataNode.ParentDataNode = DataTrees[0].GetDataNode(dataNode.ParentDataNode.Identity);
                    //            dataNode.ObjectQuery = this;
                    //            (dataNode.DataSource as StorageDataSource).DataLoadersMetadata = new Dictionary<string, DataLoaderMetadata>((dataNode.ParentDataNode.DataSource as StorageDataSource).DataLoadersMetadata);
                    //            //dataNode.DataSource = StorageObjectQuery.CreateValueTypeDataSource(dataNode, dataNode.ParentDataNode, dataNode.AssignedMetaObject as MetaDataRepository.Attribute) as StorageDataSource;
                    //            foreach (DataLoaderMetadata dataLoaderMetaData in (dataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values.ToArray())
                    //            {
                    //                if (dataLoaderMetaData.ObjectsContextIdentity == (ObjectsContext as ObjectsContext).Identity && !dataNode.DataSource.DataLoaders.ContainsKey(dataLoaderMetaData.ObjectsContextIdentity))
                    //                    dataNode.DataSource.DataLoaders.Add(dataLoaderMetaData.ObjectsContextIdentity, ObjectsContext.CreateDataLoader(dataNode, dataLoaderMetaData));
                    //            }

                    //        }
                    //    }
                    //}

                }

            }
            //Synchronization(dataTrees[0]);
            string errorOutput = "";
            DataTrees[0].Validate(ref errorOutput);
            foreach (DataNode selectedDataNode in selectListItems)
            {
                DataNode dataNode = DataTrees[0].GetDataNode(selectedDataNode.Identity);
                if (!SelectListItems.Contains(dataNode))
                    SelectListItems.Add(dataNode);
            }
        }

        ///// <MetaDataID>{4a28902c-2093-436f-bf91-88290e989369}</MetaDataID>
        //private void Synchronization(DataNode dataNode)
        //{
        //    if (DataTrees[0].GetDataNode(dataNode.Identity) == null)
        //    {
        //        dataNode.ParentDataNode = DataTrees[0].GetDataNode(dataNode.ParentDataNode.Identity);

        //    }
        //    else
        //    {
        //        foreach (DataNode subDataNode in new List<DataNode>(dataNode.SubDataNodes))
        //            Synchronization(subDataNode);
        //    }
        //}

        /// <MetaDataID>{21a53218-b9ca-40e6-971b-0d7d48e18f95}</MetaDataID>
        private int GetDataNodeDeep(DataNode dataNode)
        {
            int i = 1;
            if (dataNode.ParentDataNode != null)
                i += GetDataNodeDeep(dataNode.ParentDataNode);
            return i;

        }


        ///<summary>
        ///This method seearch for data loader of subDataNodeIdentity and load object link between data loader objects and parent datanode out storage dataloader objects 
        ///</summary>
        ///<param name="subDataNodeIdentity">
        ///Defines the identity of subDataNode for objectsLinks loading between subdatanode objects and parent.
        ///</param>
        ///<param name="streamedInterStorageObjectLinks">
        ///Defines a dictionary with relation part identity as key and objects links relation data.
        ///The objects links relation data are StreamedTable and must be transformed to real table
        ///</param>
        ///<param name="referenceObjectIdentityTypes">
        /// This parameter defines a collaction with reference ObjectIdentityTypes of ObjectIdentities which contains the data table for each relation part
        ///</param>
        ///<returns>
        ///Returns a dictionary with relation part identity as key and objects links relation data as streamed tables.
        ///The object links now has linked object from both stoarage.
        ///The related object from datanode parent storage now has been moved to OwnerObject column 
        ///and the just retrieved from dataloader object loaded to RelatedObject column .
        ///</returns>
        /// <MetaDataID>{db32d7cd-4bd3-4f56-ac7b-c5af13e010e8}</MetaDataID>
        internal Dictionary<string, DataLoader.StreamedTable> LoadParentSubDataNodeObjectRelationLinks(Guid subDataNodeIdentity, Dictionary<PartialRelationIdentity, DataLoader.StreamedTable> streamedInterStorageObjectLinks, Dictionary<PartialRelationIdentity, List<ObjectIdentityType>> relationTableObjectIdentityTypes)
        {
            Dictionary<PartialRelationIdentity, IDataTable> interStorageObjectLinks = new Dictionary<PartialRelationIdentity, IDataTable>();
            foreach (var streamedTableEntry in streamedInterStorageObjectLinks)
                interStorageObjectLinks[streamedTableEntry.Key] = DataSource.DataObjectsInstantiator.CreateDataTable(streamedTableEntry.Value);
            DataNode dataNode = DataTrees[0].GetDataNode(subDataNodeIdentity);
            string storageIdentity = (ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity;
            interStorageObjectLinks = (dataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader).LoadInterStorageObjectLinksWithParent(interStorageObjectLinks, relationTableObjectIdentityTypes);

            streamedInterStorageObjectLinks.Clear();
            foreach (var interStorageObjectLinksEntry in interStorageObjectLinks)
                streamedInterStorageObjectLinks[interStorageObjectLinksEntry.Key] = interStorageObjectLinksEntry.Value.SerializeTable();

            return streamedInterStorageObjectLinks;
        }



        ///<summary>
        ///This method search for data loader of subDataNodeIdentity and load object link between data loader objects and parent datanode out storage dataloader objects 
        ///</summary>
        ///<param name="subDataNodeIdentity">
        ///Defines the identity of subDataNode for objectsLinks loading between subdatanode objects and parent.
        ///</param>
        ///<param name="interStorageObjectLinks">
        ///Defines a dictionary with relation part identity as key and objects links relation data.
        ///</param>
        ///<param name="referenceObjectIdentityTypes">
        /// This parameter defines a collaction with reference ObjectIdentityTypes of ObjectIdentities which contains the data table for each relation part
        ///</param>
        ///<returns>
        ///Returns a dictionary with relation part identity as key and objects links relation data.
        ///The object links now has linked object from both stoarage.
        ///The related object from datanode parent storage now has been moved to OwnerObject column 
        ///and the just retrieved from dataloader object loaded to RelatedObject column 
        ///</returns>
        /// <MetaDataID>{ca371560-cdde-4ffb-b6b4-690b377256f7}</MetaDataID>
        internal Dictionary<PartialRelationIdentity, IDataTable> LoadParentSubDataNodeObjectRelationLinks(Guid subDataNodeIdentity, Dictionary<PartialRelationIdentity, IDataTable> interStorageObjectLinks, Dictionary<PartialRelationIdentity, List<ObjectIdentityType>> referenceObjectIdentityTypes)
        {

            DataNode dataNode = DataTrees[0].GetDataNode(subDataNodeIdentity);
            string storageIdentity = (ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity;
            interStorageObjectLinks = (dataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader).LoadInterStorageObjectLinksWithParent(interStorageObjectLinks, referenceObjectIdentityTypes);
            return interStorageObjectLinks;
        }

        ///<summary>
        ///This method search for data loader of subDataNodeIdentity and subDataNode dataloader retrieve and load related objects
        ///</summary>
        ///<returns>
        ///
        ///</returns>
        /// <MetaDataID>{460b47cd-ddd0-4070-b8d0-b2c912ab3e32}</MetaDataID>
        internal Dictionary<PartialRelationIdentity, IDataTable> GetParentSubDataNodeObjectRelationLinks(Guid subDataNodeIdentity, Dictionary<PartialRelationIdentity, IDataTable> interStorageObjectLinks, Dictionary<PartialRelationIdentity, List<ObjectIdentityType>> relationTableObjectIdentityTypes)
        {
            DataNode dataNode = DataTrees[0].GetDataNode(subDataNodeIdentity);
            string storageIdentity = (ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity;
            interStorageObjectLinks = (dataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader).GetRelatedObject(interStorageObjectLinks, relationTableObjectIdentityTypes);
            return interStorageObjectLinks;
        }

        /// <MetaDataID>{f26004b5-bb90-4b3a-90b7-aa13df9c198b}</MetaDataID>
        internal Dictionary<string, DataLoader.StreamedTable> GetParentSubDataNodeObjectRelationLinks(Guid subDataNodeIdentity, Dictionary<PartialRelationIdentity, DataLoader.StreamedTable> streamedInterStorageObjectLinks, Dictionary<PartialRelationIdentity, List<ObjectIdentityType>> relationTableObjectIdentityTypes)
        {
            Dictionary<PartialRelationIdentity, IDataTable> interStorageObjectLinks = new Dictionary<PartialRelationIdentity, IDataTable>();
            foreach (var streamedTableEntry in streamedInterStorageObjectLinks)
                interStorageObjectLinks[streamedTableEntry.Key] = DataSource.DataObjectsInstantiator.CreateDataTable(streamedTableEntry.Value);

            DataNode dataNode = DataTrees[0].GetDataNode(subDataNodeIdentity);
            string storageIdentity = (ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity;
            streamedInterStorageObjectLinks.Clear();
            foreach (var interStorageObjectLinksEntry in (dataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader).GetRelatedObject(interStorageObjectLinks, relationTableObjectIdentityTypes))
                streamedInterStorageObjectLinks[interStorageObjectLinksEntry.Key] = interStorageObjectLinksEntry.Value.SerializeTable();
            return streamedInterStorageObjectLinks;
        }


        /// <MetaDataID>{dd5c1eb9-a7a1-43a5-99f5-f0a9dba5f79b}</MetaDataID>
        internal bool CriterionsForDataLoaderResolvedLocally(DataLoader dataLoader)
        {
            foreach (StorageDataLoader objectQueryDataLoader in this.DataLoaders.Values)
            {
                if (!(objectQueryDataLoader as StorageDataLoader).CriterionsForDataLoaderResolvedLocally(dataLoader))
                    return false;
            }
            return true;
        }



        /// <MetaDataID>{96c54c1f-8ed6-4649-bb83-1cea45f59830}</MetaDataID>
        public List<Guid> GlobalResolvedCriteria()
        {
            List<Guid> globalResolvedCriteria = new List<Guid>();
            foreach (var criterion in (from searchCondition in DataTrees[0].BranchSearchConditions
                                       where searchCondition != null
                                       from criterion in searchCondition.Criterions
                                       select criterion))
            {
                if (!criterion.Applied)
                    globalResolvedCriteria.Add(criterion.Identity);
            }
            return globalResolvedCriteria;

            //return (from storageDataLoader in DataLoaders.Values.OfType<StorageDataLoader>()
            //        from criterion in storageDataLoader.GlobalResolveCriterions
            //        select criterion.Identity ).ToList();
        }

        /// <MetaDataID>{0cf55008-d3db-44b8-bb2f-fcbb2f9aba41}</MetaDataID>
        public void ActivatePassiveObjects()
        {
            foreach (StorageDataLoader objectQueryDataLoader in this.DataLoaders.Values)
                objectQueryDataLoader.ActivateObjects();
        }

        /// <MetaDataID>{0f048f5d-0b8d-4e68-a18f-203de6aff425}</MetaDataID>
        internal void UpdateDataLoadingModel(Guid dataNodeIdentity)
        {
            var dataNode = GetDataNode(dataNodeIdentity);
            if (dataNode != null && dataNode.AssignedMetaObject != null && dataNode.DataSource != null)
                dataNode.DataSource.UpdateDataLoadingModel();
        }

        /// <MetaDataID>{8c33790e-c906-4886-b4d1-cc50bafda934}</MetaDataID>
        internal bool IsGlobalResolvedCriterion(Criterion criterion)
        {

            foreach (var dataLoader in this.DataLoaders.Values)
            {
                if ((dataLoader as StorageDataLoader).GlobalResolveCriterions.Contains(criterion))
                    return true;
            }
            return false;

        }

        internal void LoadQueryResult(Collections.Generic.Dictionary<PartialRelationIdentity, DistributedObjectQuery> distributedObjectQueries)
        {
            //ToDo θα πρέπει να χτίζωνται μόνο οι σχέσεις που ειναι απαραίτητες
            if (QueryResultType != null)
            {
                System.Collections.Generic.List<DataLoader> dataLoaders = new System.Collections.Generic.List<DataLoader>();
                foreach (DataLoader dataLoader in DataLoaders.Values)
                {
                    dataLoaders.Add(dataLoader);
                    if (dataLoader is StorageDataLoader)// resets LocalOnMemoryResolvedCriterions 
                    {
                        //When a criterion shared between two data loaders, the dataloader thet resolve criterion localy, sets criterion applied property true
                        //For that the data loader where resolves criterion on memory reset criterion applied property false
                        foreach (var criterion in (dataLoader as StorageDataLoader).LocalOnMemoryResolvedCriterions.Keys)
                        {
                            if (criterion.Applied)
                                criterion.Applied = false;
                        }
                    }
                }
                dataLoaders.Sort(new DataLoader.DataLoaderSorting());


                bool localLoad = QueryResultType.CanLoadDataLocal;
                if (localLoad)
                {

                    DataTrees[0].BuildTablesRelations();

                    foreach (DataNode dataNode in DataTrees)
                        OrganizeData(dataNode);
                    QueryResultType.DataLoader.LoadData();
                }
                if (ObjectQueryDataSet != null)
                {
                    ObjectQueryDataSet.Relations.Clear();
                    ObjectQueryDataSet.Tables.Clear();
                }
            }
        }

        public bool AllQueryDataLoaded
        {
            get
            {
                return true;
            }
        }
    }

    /// <MetaDataID>{e51955ae-8664-43b2-b2a2-b84faaa3fd70}</MetaDataID>
    public interface IObjectQueryPartialResolver
    {
        /// <MetaDataID>{2d4cad34-23df-4137-a602-8cd5a4595b79}</MetaDataID>
        ObjectQueryLanguage.DistributedObjectQuery DistributeObjectQuery(Guid mainQueryIdentity,
            Collections.Generic.List<MetaDataRepository.ObjectQueryLanguage.DataNode> dataTrees,
            QueryResultType queryResult,
            Collections.Generic.List<MetaDataRepository.ObjectQueryLanguage.DataNode> selectListItems,
            OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoaderMetadata> dataLoadersMetadata,
            OOAdvantech.Collections.Generic.Dictionary<string, object> parameters,
            System.Collections.Generic.List<string> usedAliases,
            System.Collections.Generic.List<string> queryStorageIdentities);

        /// <MetaDataID>{ae87703a-e1eb-4ff6-b20d-3be99507b7de}</MetaDataID>
        DataLoader CreateDataLoader(DataNode dataNode, DataLoaderMetadata dataLoaderMetadata);
    }

    /// <MetaDataID>{ccaa5e66-4b77-4474-b7ef-612883e10805}</MetaDataID>
    public static class ObjectDataNodeRelationPrefetching
    {
        /// <summary>
        /// This method sets the associations 
        /// fields of relate object to reproduce the objects link in memory. 
        /// System must be load the data sources before call this function. 
        /// </summary>
        /// <MetaDataID>{4ae648ca-054d-4907-876c-f1e101126d1f}</MetaDataID>
        internal static void LoadObjectRelationLinks(this DataNode dataNode)
        {
            using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.NotSupported))
            {
                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Object &&
                        subDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        if ((subDataNode.ParentLoadsObjectsLinks ||
                            (dataNode.IsThereBackOnConstructionRoute(subDataNode) && (subDataNode.DataSource as StorageDataSource).ObjectActivation)) &&
                            dataNode.DataSource.ThereAreObjectsToActivate)
                        {
                            (dataNode.DataSource as StorageDataSource).LoadObjectRelationLinks(subDataNode);
                        }
                        else if (subDataNode.ValueTypePath.Count > 0)
                        {
                            if ((subDataNode.DataSource as StorageDataSource).ObjectActivation)
                                (dataNode.DataSource as StorageDataSource).LoadObjectRelationLinks(subDataNode);
                        }
                    }
                    subDataNode.LoadObjectRelationLinks();
                }
                stateTransition.Consistent = true;
            }
        }


        //#region Prefetching data mechanism

        /// <MetaDataID>{17b1e60a-3068-48bd-a6de-6590033932ae}</MetaDataID>
        static internal bool IsThereBackOnConstructionRoute(this DataNode parentDataNode, DataNode subDataNode)
        {
            if (subDataNode.Type == DataNode.DataNodeType.Object &&
                        subDataNode.AssignedMetaObject is AssociationEnd &&
                        (subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Navigable &&
                        !(subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().HasLazyFetchingRealization)
            {
                if (parentDataNode.ObjectQuery.SelectListItems.Contains(subDataNode))
                {
                    return true;
                }
            }
            if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.Classifier.ClassHierarchyLinkAssociation != null &&
                        subDataNode.AssignedMetaObject is AssociationEnd &&
                        ((subDataNode.AssignedMetaObject as AssociationEnd).Identity == subDataNode.Classifier.ClassHierarchyLinkAssociation.RoleA.Identity ||
                        (subDataNode.AssignedMetaObject as AssociationEnd).Identity == subDataNode.Classifier.ClassHierarchyLinkAssociation.RoleB.Identity))
            {
                if (parentDataNode.ObjectQuery.SelectListItems.Contains(subDataNode))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Create prefetching data node for data node and sub data node.
        /// </summary>
        /// <param name="queryStorageIdentities">
        /// Define a collection with storage identities of storages where participate in query  
        /// </param>
        /// <MetaDataID>{11301c3c-1478-46b6-9ff8-39ac48f985f3}</MetaDataID>
        static internal System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNodeRelatedDataLoadersMetadata> CreatePrefetchingMemberDataNodes(this DataNode referenceDataNode, ref System.Collections.Generic.List<string> queryStorageIdentities)
        {
            DataNodeRelatedDataLoadersMetadata dataNodePrefetchingData = null;
            System.Collections.Generic.List<DataNodeRelatedDataLoadersMetadata> prefetchingMemberDataNodes = new System.Collections.Generic.List<DataNodeRelatedDataLoadersMetadata>();

            #region Mark as back route activation member fetching if needed

            if (referenceDataNode.Type == DataNode.DataNodeType.Object)
            {

                foreach (DataNode subDataNode in referenceDataNode.SubDataNodes)
                {
                    if (subDataNode.DataSource != null)
                        prefetchingMemberDataNodes.AddRange(subDataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
                    if (referenceDataNode.IsThereBackOnConstructionRoute(subDataNode))
                    {
                        if (!referenceDataNode.ObjectQuery.SelectListItems.Contains(referenceDataNode))
                        {
                            referenceDataNode.ObjectQuery.AddSelectListItem(referenceDataNode);

                            if (subDataNode.AssignedMetaObject is AssociationEnd &&
                                (subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
                                subDataNode.Classifier.ClassHierarchyLinkAssociation != (subDataNode.AssignedMetaObject as AssociationEnd).Association) // Relation between assoction class and (RoleA or RoleB) class is always one to one
                                referenceDataNode.FilterNotActAsLoadConstraint = true;//

                            referenceDataNode.BackRouteActivationMembersFetching(subDataNode);
                        }
                        else
                        {
                            if (subDataNode.AssignedMetaObject is AssociationEnd &&
                                (subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
                                subDataNode.Classifier.ClassHierarchyLinkAssociation != (subDataNode.AssignedMetaObject as AssociationEnd).Association) // Relation between assoction class and (RoleA or RoleB) class is always one to one
                            {
                                referenceDataNode.BackRouteActivationMembersFetching(subDataNode);
                                referenceDataNode.FilterNotActAsLoadConstraint = true;
                            }
                        }
                    }
                }
            }
            #endregion

            if (referenceDataNode.Type == DataNode.DataNodeType.Group)
            {
                GroupDataNode groupDataNode = referenceDataNode as GroupDataNode;
                prefetchingMemberDataNodes.AddRange(groupDataNode.GroupedDataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
            } 

            if (referenceDataNode.DataSource == null || !referenceDataNode.DataSource.ThereAreObjectsToActivate)
                return prefetchingMemberDataNodes;

            if (referenceDataNode.Type == DataNode.DataNodeType.Object && (referenceDataNode.DataSource as StorageDataSource).HasInObjectContextData &&
                (referenceDataNode.DataSource as StorageDataSource).ObjectActivation)
            {
                foreach (MetaDataRepository.AssociationEnd associationEnd in referenceDataNode.DataSource.PrefetchingAssociationEnds)
                {

                    if (associationEnd.Association.HasPersistentObjectLink && !associationEnd.HasLazyFetchingRealization && associationEnd.Navigable)
                    {
                        DataNode dataNode = referenceDataNode.GetPrefetchingSubDataNodeFor(associationEnd);
                        if (dataNode != null)
                        {
                            StorageDataSource dataSource = null;
                            if (dataNode.DataSource == null)
                            {
                                #region Create data source

                                dataSource = ObjectsContextQuery.CreateRelatedObjectDataSource(dataNode, referenceDataNode.DataSource, dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, queryStorageIdentities) as StorageDataSource;
                                if (dataSource.HasOutObjectContextData || dataSource.HasInObjectContextData)
                                {
                                    if (dataNode.DataSource != null)
                                        dataSource = dataNode.DataSource as StorageDataSource;
                                    else
                                        dataNode.DataSource = dataSource;


                                    if (dataNodePrefetchingData == null)
                                    {
                                        dataNodePrefetchingData = new DataNodeRelatedDataLoadersMetadata(referenceDataNode);
                                        prefetchingMemberDataNodes.Add(dataNodePrefetchingData);
                                    }
                                    dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[dataNode] = new Dictionary<string, DataLoaderMetadata>();
                                    foreach (DataLoaderMetadata dataLoaderMetadata in dataSource.DataLoadersMetadata.Values)
                                        dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[dataNode].Add(dataLoaderMetadata.ObjectsContextIdentity, dataLoaderMetadata);
                                    if ((dataNode.DataSource as StorageDataSource).HasInObjectContextData)
                                        prefetchingMemberDataNodes.AddRange(dataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
                                }
                                else
                                {
                                    if (referenceDataNode.ObjectQuery.SelectListItems.Contains(dataNode))
                                        referenceDataNode.ObjectQuery.SelectListItems.Remove(dataNode);
                                    dataNode.ParentDataNode = null;
                                    dataSource = null;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Update data source data loading metada
                                dataSource = dataNode.DataSource as StorageDataSource;
                                if (dataNode.AutoGenaratedForMembersFetching)
                                {
                                    List<DataLoaderMetadata> dataNodePrefetcingData = dataSource.AddNewDataLoadersMetaData(ObjectsContextQuery.GetRelatedDataLoadersMetada(dataNode, referenceDataNode.DataSource, dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, queryStorageIdentities));
                                    if (dataNodePrefetcingData.Count > 0)
                                    {
                                        if (dataNodePrefetchingData == null)
                                        {
                                            dataNodePrefetchingData = new DataNodeRelatedDataLoadersMetadata(referenceDataNode);
                                            prefetchingMemberDataNodes.Add(dataNodePrefetchingData);
                                        }
                                        dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[dataNode] = dataNodePrefetcingData.ToDictionary(x => x.ObjectsContextIdentity);
                                        if ((dataNode.DataSource as StorageDataSource).HasInObjectContextData)
                                            prefetchingMemberDataNodes.AddRange(dataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                foreach (MetaDataRepository.Attribute attribute in referenceDataNode.Classifier.GetAttributes(true))
                {
                    if (attribute.IsPersistentValueType && attribute.HasPersistentRealization)
                    {
                        DataNode dataNode = referenceDataNode.GetValueTypeSubDataNodeFor(attribute);
                        if (dataNode != null)
                        {
                            if (dataNode.DataSource == null)
                            {
                                #region Create data source
                                StorageDataSource dataSource = ObjectsContextQuery.CreateValueTypeDataSource(dataNode, referenceDataNode.DataSource) as StorageDataSource;
                                if (dataSource.HasOutObjectContextData || dataSource.HasInObjectContextData)
                                {
                                    if (dataNode.DataSource != null)
                                    {
                                        (dataNode.DataSource as StorageDataSource).DataLoadersMetadata = (dataSource as StorageDataSource).DataLoadersMetadata;
                                        dataSource = dataNode.DataSource as StorageDataSource;
                                    }
                                    else
                                        dataNode.DataSource = dataSource;

                                    if (dataNodePrefetchingData == null)
                                    {
                                        dataNodePrefetchingData = new DataNodeRelatedDataLoadersMetadata(referenceDataNode);
                                        prefetchingMemberDataNodes.Add(dataNodePrefetchingData);
                                    }
                                    dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[dataNode] = new Dictionary<string, DataLoaderMetadata>();
                                    foreach (DataLoaderMetadata dataLoaderMetadata in dataSource.DataLoadersMetadata.Values)
                                        dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[dataNode].Add(dataLoaderMetadata.ObjectsContextIdentity, dataLoaderMetadata);

                                    if ((dataNode.DataSource as StorageDataSource).HasInObjectContextData)
                                        prefetchingMemberDataNodes.AddRange(dataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
                                }
                                else
                                {
                                    //Remove DataNode from data tree
                                    if (referenceDataNode.ObjectQuery.SelectListItems.Contains(dataNode))
                                        referenceDataNode.ObjectQuery.SelectListItems.Remove(dataNode);
                                    dataNode.ParentDataNode = null;
                                    dataSource = null;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Update data source data loading metada
                                List<DataLoaderMetadata> dataNodePrefetcingData = (dataNode.DataSource as StorageDataSource).AddNewDataLoadersMetaData((referenceDataNode.DataSource as StorageDataSource).DataLoadersMetadata);

                                if (dataNodePrefetcingData.Count > 0)
                                {
                                    if (dataNodePrefetchingData == null)
                                    {
                                        dataNodePrefetchingData = new DataNodeRelatedDataLoadersMetadata(referenceDataNode);
                                        prefetchingMemberDataNodes.Add(dataNodePrefetchingData);
                                    }
                                    dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[dataNode] = new Dictionary<string, DataLoaderMetadata>();
                                    foreach (DataLoaderMetadata dataLoaderMetadata in dataNodePrefetcingData)
                                        dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[dataNode].Add(dataLoaderMetadata.ObjectsContextIdentity, dataLoaderMetadata);


                                    if ((dataNode.DataSource as StorageDataSource).HasInObjectContextData)
                                        prefetchingMemberDataNodes.AddRange(dataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
                                }
                                #endregion
                            }
                        }
                    }
                }
            }

            if (referenceDataNode.Classifier != null && (referenceDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation != null && (referenceDataNode.ObjectQuery.SelectListItems.Contains(referenceDataNode) || referenceDataNode.MembersFetchingObjectActivation))
            {

                foreach (DataNode roleDataNode in referenceDataNode.CreateLinkClassSubDataNodes())
                {
                    if (roleDataNode.DataSource == null)
                    {
                        #region Create data source
                        StorageDataSource dataSource = ObjectsContextQuery.CreateRelatedObjectDataSource(roleDataNode, referenceDataNode.DataSource, roleDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, queryStorageIdentities) as StorageDataSource;
                        roleDataNode.DataSource = dataSource;
                        if (dataSource.HasInObjectContextData || dataSource.HasOutObjectContextData)
                        {
                            if (dataNodePrefetchingData == null)
                            {
                                dataNodePrefetchingData = new DataNodeRelatedDataLoadersMetadata(referenceDataNode);
                                prefetchingMemberDataNodes.Add(dataNodePrefetchingData);
                            }
                            dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[roleDataNode] = new Dictionary<string, DataLoaderMetadata>();
                            foreach (DataLoaderMetadata dataLoaderMetadata in dataSource.DataLoadersMetadata.Values)
                                dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[roleDataNode].Add(dataLoaderMetadata.ObjectsContextIdentity, dataLoaderMetadata);
                            if ((roleDataNode.DataSource as StorageDataSource).HasInObjectContextData)
                                prefetchingMemberDataNodes.AddRange(roleDataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
                        }
                        else
                            roleDataNode.ParentDataNode = null;
                        #endregion
                    }
                    else
                    {
                        #region Update data source data loading metada
                        if (roleDataNode.AutoGenaratedForMembersFetching)
                        {
                            List<DataLoaderMetadata> dataNodePrefetcingData = (roleDataNode.DataSource as StorageDataSource).AddNewDataLoadersMetaData(ObjectsContextQuery.GetRelatedDataLoadersMetada(roleDataNode, referenceDataNode.DataSource, roleDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, queryStorageIdentities));
                            if (dataNodePrefetcingData.Count > 0)
                            {
                                if (dataNodePrefetchingData == null)
                                {
                                    dataNodePrefetchingData = new DataNodeRelatedDataLoadersMetadata(referenceDataNode);
                                    prefetchingMemberDataNodes.Add(dataNodePrefetchingData);
                                }
                                dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata[roleDataNode] = dataNodePrefetcingData.ToDictionary(x => x.ObjectsContextIdentity);
                                if ((roleDataNode.DataSource as StorageDataSource).HasInObjectContextData)
                                    prefetchingMemberDataNodes.AddRange(roleDataNode.CreatePrefetchingMemberDataNodes(ref queryStorageIdentities));
                            }

                        }
                        #endregion
                    }
                }
            }
            if (dataNodePrefetchingData != null)
            {
                var relatedStorageIdentities = (from subDanodeDatalodersMetadataEntry in dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata
                                                from dataLoaderMetadata in subDanodeDatalodersMetadataEntry.Value.Values
                                                where dataLoaderMetadata.ObjectsContextIdentity != ((dataNodePrefetchingData.DataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectsContext).Identity
                                                select dataLoaderMetadata.ObjectsContextIdentity).Distinct();

                #region Adds  DataLoadersMedata for out of storage relation 
                //In case where there are out of storage relation to load prefetching members
                //the main query will be dispatch the DataLoadersMetada with out of storage  StorageCells
                //also must be dispatch the DataLoadersMetada with StorageCellReference for PrefetchingData DataNode
                foreach (var releatedStorageIdentity in relatedStorageIdentities)
                {
                    if ((dataNodePrefetchingData.DataNode.DataSource as StorageDataSource).DataLoadersMetadata.ContainsKey(releatedStorageIdentity))
                        dataNodePrefetchingData.DataNodeNewDataLoadersMetadata.Add(releatedStorageIdentity, (dataNodePrefetchingData.DataNode.DataSource as StorageDataSource).DataLoadersMetadata[releatedStorageIdentity]);
                }
                #endregion

                #region removed code
                //foreach (System.Collections.Generic.List<DataLoaderMetadata> dataLoadersMetadata in dataNodePrefetchingData.SubDataNodesNewDataLoadersMetadata.Values)
                //{
                //    foreach (DataLoaderMetadata dataLoaderMetadata in dataLoadersMetadata)
                //    {
                //        if (dataLoaderMetadata.ObjectsContextIdentity != ((theDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectsContext).Identity)
                //        {
                //            if (!dataNodePrefetchingData.DataLoadersMetadata.ContainsKey(dataLoaderMetadata.ObjectsContextIdentity))
                //            {
                //                if ((dataNodePrefetchingData.DataNode.DataSource as StorageDataSource).DataLoadersMetadata.ContainsKey(dataLoaderMetadata.ObjectsContextIdentity))
                //                    dataNodePrefetchingData.DataLoadersMetadata.Add(dataLoaderMetadata.ObjectsContextIdentity, (dataNodePrefetchingData.DataNode.DataSource as StorageDataSource).DataLoadersMetadata[dataLoaderMetadata.ObjectsContextIdentity]);

                //            }
                //        }
                //    }
                //}
                #endregion
            }
            return prefetchingMemberDataNodes;

        }

        /// <summary>When you load a relation object in memory system must be 
        /// load the related object also.The CreateLinkClassSubDataNodes 
        /// method checks the existence of data nodes in data tree for related 
        /// objects. If there aren't create and add them in select list. </summary>
        /// <MetaDataID>{8865D564-5AE1-45B3-8335-8408435083B1}</MetaDataID>
        static System.Collections.Generic.List<DataNode> CreateLinkClassSubDataNodes(this DataNode theDataNode)
        {
            System.Collections.Generic.List<DataNode> dataNodes = new System.Collections.Generic.List<DataNode>();
            //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
            bool RoleAExist = false;
            bool RoleBExist = false;

            foreach (DataNode currentDataNode in theDataNode.SubDataNodes)
            {

                var assignedMetaObjectIdenty = currentDataNode.AssignedMetaObjectIdenty;
                if (assignedMetaObjectIdenty == (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Identity && !RoleAExist)
                {
                    if (currentDataNode.AssignedMetaObject == null)
                        currentDataNode.AssignedMetaObject = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA;
                    RoleAExist = true;
                    if (!theDataNode.ObjectQuery.SelectListItems.Contains(currentDataNode))
                    {
                        currentDataNode.MembersFetchingObjectActivation = true;
                        theDataNode.ObjectQuery.AddSelectListItem(currentDataNode);
                        if (theDataNode.ObjectQuery is DistributedObjectQuery)
                            dataNodes.Add(currentDataNode);
                    }
                    //if ((currentDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Count == 0)//.ContainsKey((currentDataNode.ObjectQuery as DistributedObjectQuery).Identity))
                    {
                        //currentDataNode.DataSource = null;
                        dataNodes.Add(currentDataNode);
                    }

                }
                if (assignedMetaObjectIdenty == (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Identity && !RoleBExist)
                {
                    if (currentDataNode.AssignedMetaObject == null)
                        currentDataNode.AssignedMetaObject = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB;

                    RoleBExist = true;
                    if (!theDataNode.ObjectQuery.SelectListItems.Contains(currentDataNode))
                    {
                        currentDataNode.MembersFetchingObjectActivation = true;
                        theDataNode.ObjectQuery.AddSelectListItem(currentDataNode);
                        if (theDataNode.ObjectQuery is DistributedObjectQuery)
                            dataNodes.Add(currentDataNode);
                    }
                    //if ((currentDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Count == 0)// .ContainsKey((currentDataNode.ObjectQuery as DistributedObjectQuery).Identity))
                    {
                        //currentDataNode.DataSource = null;
                        dataNodes.Add(currentDataNode);
                    }
                }

            }
            if (!RoleAExist)
            {
                DataNode MyDataNode = new DataNode(theDataNode.ObjectQuery);
                MyDataNode.ParentDataNode = theDataNode;
                MyDataNode.AssignedMetaObject = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA;
                MyDataNode.Name = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Name;
                MyDataNode.Alias = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Name + MyDataNode.GetHashCode().ToString();
                MyDataNode.MembersFetchingObjectActivation = true;
                MyDataNode.AutoGenaratedForMembersFetching = true;
                theDataNode.ObjectQuery.AddSelectListItem(MyDataNode);
                dataNodes.Add(MyDataNode);
            }
            if (!RoleBExist)
            {
                DataNode MyDataNode = new DataNode(theDataNode.ObjectQuery);
                MyDataNode.ParentDataNode = theDataNode;
                MyDataNode.AssignedMetaObject = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB;
                MyDataNode.Name = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Name;
                MyDataNode.Alias = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Name + MyDataNode.GetHashCode().ToString();
                MyDataNode.MembersFetchingObjectActivation = true;
                MyDataNode.AutoGenaratedForMembersFetching = true;
                theDataNode.ObjectQuery.AddSelectListItem(MyDataNode);
                dataNodes.Add(MyDataNode);
            }
            return dataNodes;
        }

        /// <MetaDataID>{54ec17b4-f6cf-474f-b113-c80ed4da0eea}</MetaDataID>
        static DataNode GetSubDataNodeFor(this DataNode theDataNode, MetaDataRepository.Attribute valueTypeAttribute)
        {
            foreach (DataNode subDatNode in theDataNode.SubDataNodes)
                if (subDatNode.AssignedMetaObjectIdenty == valueTypeAttribute.Identity)
                    return subDatNode;

            return null;
        }

        ///// <MetaDataID>{9cc5ca15-7f0d-46ae-baaf-72ea0a37bf53}</MetaDataID>
        //static DataNode GetSubDataNodeFor(this DataNode theDataNode, MetaDataRepository.AssociationEnd valueTypeAttribute)
        //{
        //    foreach (DataNode subDatNode in theDataNode.SubDataNodes)
        //        if (subDatNode.AssignedMetaObjectIdenty == valueTypeAttribute.Identity)
        //            return subDatNode;

        //    return null;
        //}
        ///// <MetaDataID>{cda05834-ab56-4340-8f6f-4768ac72f65e}</MetaDataID>
        //static System.Collections.Generic.List<DataNode> GetLinkClassSubDataNodes(this DataNode theDataNode)
        //{
        //    System.Collections.Generic.List<DataNode> dataNodes = new System.Collections.Generic.List<DataNode>();
        //    bool RoleAExist = false;
        //    bool RoleBExist = false;
        //    foreach (DataNode currentDataNode in theDataNode.SubDataNodes)
        //    {
        //        var assignedMetaObjectIdenty = currentDataNode.AssignedMetaObjectIdenty;
        //        if (assignedMetaObjectIdenty == (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA.Identity && !RoleAExist)
        //        {
        //            if (currentDataNode.AssignedMetaObject == null)
        //                currentDataNode.AssignedMetaObject = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleA;
        //            RoleAExist = true;
        //            dataNodes.Add(currentDataNode);
        //        }
        //        if (assignedMetaObjectIdenty == (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB.Identity && !RoleBExist)
        //        {
        //            if (currentDataNode.AssignedMetaObject == null)
        //                currentDataNode.AssignedMetaObject = (theDataNode.Classifier as MetaDataRepository.Classifier).ClassHierarchyLinkAssociation.RoleB;
        //            RoleBExist = true;
        //            dataNodes.Add(currentDataNode);
        //        }
        //    }
        //    return dataNodes;

        //}
        /// <summary>
        /// Returns the value type dataNode 
        /// </summary>
        /// <MetaDataID>{3587E023-471E-4EEB-8B50-CE1B097BB6D0}</MetaDataID>
        static DataNode GetValueTypeSubDataNodeFor(this DataNode theDataNode, MetaDataRepository.Attribute valueTypeAttribute)
        {
            if (!valueTypeAttribute.IsPersistentValueType)
                return null;

            #region Checks if already exist sub data node for association end
            foreach (DataNode subDatNode in theDataNode.SubDataNodes)
                if (subDatNode.AssignedMetaObjectIdenty == valueTypeAttribute.Identity)
                {
                    if (subDatNode.AssignedMetaObject == null)
                        subDatNode.AssignedMetaObject = valueTypeAttribute;

                    if (!theDataNode.ObjectQuery.SelectListItems.Contains(subDatNode))
                    {
                        theDataNode.ObjectQuery.AddSelectListItem(subDatNode);
                        subDatNode.MembersFetchingObjectActivation = true;
                        return subDatNode;
                    }
                    else
                        subDatNode.MembersFetchingObjectActivation = true;
                    return subDatNode;
                }
            #endregion

            #region Abord recursion
            //TODO να γραφτούν test cases για αυτές τις περιπτώσεις
            //TODO δεν νομίζω να γίνονεται recursion στα structure

            ////You can't go back on relationship because you go into recursive loop
            //if (CheckForRecursion(associationEnd.Association))
            //    return;
            //if (AssignedMetaObject is MetaDataRepository.AssociationEnd)
            //    if (((MetaDataRepository.AssociationEnd)AssignedMetaObject).Association.Identity == associationEnd.Association.Identity)
            //        return;
            ////Check parents datanode for auto generated datanode with the same association end
            ////if there is data node with the same association and return to avoid recursive loop
            //if (ParentDataNode != null && (ParentDataNode as DataNode).IsThereAutoGenDataNodeInHierarchy(associationEnd))
            //    return;
            #endregion

            #region Add new sub data node for  valueTypeAttribute
            DataNode MyDataNode = new DataNode(theDataNode.ObjectQuery);

            MyDataNode.AssignedMetaObject = valueTypeAttribute;
            MyDataNode.Name = valueTypeAttribute.Name;
            MyDataNode.ParentDataNode = theDataNode;
            MyDataNode.Alias = valueTypeAttribute.Name + MyDataNode.GetHashCode().ToString();
            MyDataNode.MembersFetchingObjectActivation = true;
            MyDataNode.AutoGenaratedForMembersFetching = true;
            theDataNode.ObjectQuery.AddSelectListItem(MyDataNode);
            return MyDataNode;
            #endregion
        }


        /// <summary>Some relationships between classes marked as on 
        /// contractions. So when system load an object in 
        /// memory must be load and related object of relationship with 
        /// on construction marking. 
        /// This method extends the data node tree to load the on construction related object. </summary>
        /// <MetaDataID>{3587E023-471E-4EEB-8B50-CE1B097BB6D0}</MetaDataID>
        static DataNode GetPrefetchingSubDataNodeFor(this DataNode theDataNode, MetaDataRepository.AssociationEnd associationEnd)
        {

            if (associationEnd.HasLazyFetchingRealization)
                return null;

            #region Checks if already exist sub data node for association end

            foreach (DataNode subDatNode in theDataNode.SubDataNodes)
                if (subDatNode.Type != DataNode.DataNodeType.Count &&
                    subDatNode.AssignedMetaObjectIdenty == associationEnd.Identity)
                {
                    if (subDatNode.AssignedMetaObject == null)
                        subDatNode.AssignedMetaObject = associationEnd;

                    if (!associationEnd.Multiplicity.IsMany)
                    {
                        if (!theDataNode.ObjectQuery.SelectListItems.Contains(subDatNode))
                        {
                            theDataNode.ObjectQuery.AddSelectListItem(subDatNode);
                            subDatNode.MembersFetchingObjectActivation = true;
                            return subDatNode;
                        }
                        else
                            subDatNode.MembersFetchingObjectActivation = true;
                        return subDatNode;
                    }
                    else //if (!subDatNode.BranchParticipateInWereClause)
                    {
                        if (!theDataNode.ObjectQuery.SelectListItems.Contains(subDatNode))
                        {
                            theDataNode.ObjectQuery.AddSelectListItem(subDatNode);
                            subDatNode.MembersFetchingObjectActivation = true;
                            subDatNode.FilterNotActAsLoadConstraint = true;
                            return subDatNode;
                        }
                        else
                        {
                            if (!subDatNode.MembersFetchingObjectActivation)
                            {
                                subDatNode.MembersFetchingObjectActivation = true;
                                subDatNode.FilterNotActAsLoadConstraint = true;
                                return subDatNode;
                            }
                        }

                        return subDatNode;
                    }
                }
            #endregion

            #region Abord recursion
            //TODO να γραφτούν test cases για αυτές τις περιπτώσεις

            //You can't go back on relationship because you go into recursive loop
            if (theDataNode.CheckForRecursion(associationEnd))
                return null;
            if (theDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                if (((MetaDataRepository.AssociationEnd)theDataNode.AssignedMetaObject).Association.Identity == associationEnd.Association.Identity &&
                    !associationEnd.Multiplicity.IsMany)
                    return null;
            //Check parents datanode for auto generated datanode with the same association end
            //if there is data node with the same association and return to avoid recursive loop
            if (theDataNode.ParentDataNode != null && (theDataNode.ParentDataNode as DataNode).IsThereAutoGenDataNodeInHierarchy(associationEnd))
                return null;
            #endregion

            #region Add new sub data node for  association end
            DataNode MyDataNode = new DataNode(theDataNode.ObjectQuery);
            MyDataNode.ParentDataNode = theDataNode;

            if (associationEnd.Association.LinkClass == null)
                MyDataNode.Name = associationEnd.Name;
            else
                MyDataNode.Name = associationEnd.Association.Name;
            MyDataNode.AssignedMetaObject = associationEnd;
            MyDataNode.Alias = associationEnd.Name + MyDataNode.GetHashCode().ToString();
            MyDataNode.MembersFetchingObjectActivation = true;
            MyDataNode.AutoGenaratedForMembersFetching = true;
            theDataNode.ObjectQuery.AddSelectListItem(MyDataNode);
            return MyDataNode;
            #endregion

        }



    }


    /// <summary>
    /// Keeps information about related DataLoadersMettaData 
    /// Usually produced after query destribution
    /// </summary>
    /// /// 
    /// <MetaDataID>{05518dde-a3e5-43ed-8811-36f169b0d05b}</MetaDataID>
    [Serializable]
    public class DataNodeRelatedDataLoadersMetadata
    {

        ///DataNodeRelatedDataLoadersMetadata

        /// <summary>
        /// Defines the DataLoaders metadata updates of Datanode 
        /// </summary>
        [Association("", Roles.RoleA, "5268f5d6-0821-4ef9-afd6-7f788778efa8")]
        [IgnoreErrorCheck]
        public Dictionary<string, DataLoaderMetadata> DataNodeNewDataLoadersMetadata = new Dictionary<string, DataLoaderMetadata>();

        /// <summary>
        ///Defines the DataNode where the retrieved objects have prefetching members
        /// </summary>
        [Association("", Roles.RoleA, "8d40dc10-fd4f-4730-b607-99461d4c591b")]
        [IgnoreErrorCheck]
        public DataNode DataNode;

        /// <summary>Defines the prefetching members datanodes and  its data loaders metadata updates</summary>
        [Association("", Roles.RoleA, "ef7d53ab-13f8-4871-9f26-d66cfd44a21f")]
        [IgnoreErrorCheck]
        public Dictionary<DataNode, Dictionary<string, DataLoaderMetadata>> SubDataNodesNewDataLoadersMetadata = new Dictionary<DataNode, Dictionary<string, DataLoaderMetadata>>();
        /// <MetaDataID>{34bcd57d-6e23-4d05-9463-157cf2c8c342}</MetaDataID>
        public DataNodeRelatedDataLoadersMetadata(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
        {
            DataNode = dataNode;
        }

        ///// <MetaDataID>{2b73350a-b348-4042-adcc-de69424f66dd}</MetaDataID>
        //public System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> DataLoadersMetadata = new Dictionary<string, DataLoaderMetadata>();



    }
}
