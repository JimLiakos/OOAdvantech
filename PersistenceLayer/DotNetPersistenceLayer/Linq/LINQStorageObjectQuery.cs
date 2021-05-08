using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using OOAdvantech.Linq.Translators;
using OOAdvantech.MetaDataRepository;

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{0a487ac7-80d8-458e-b533-33c11ef84ba9}</MetaDataID>
    class LINQStorageObjectQuery : ObjectsContextQuery, ILINQObjectQuery
    {
        ////internal Dictionary<DataNode, Dictionary<System.Reflection.PropertyInfo, ILINQObjectQuery>> DervideMembersLINQObjectQueries = new Dictionary<DataNode, Dictionary<System.Reflection.PropertyInfo, ILINQObjectQuery>>();
        /////<summary>
        /////Defines a dictionary with derived mebers DataNode where created under the dictionory key dataNode.
        /////Derived member is class member wich defines a linq query expresion and returns a query result.
        /////In case where derived member participate in linq query the main query embed the derived member query.
        /////</summary>
        ///// <MetaDataID>{f3941b0d-0fc3-4ae5-a281-603c56444351}</MetaDataID>
        ////internal Dictionary<DataNode, Dictionary<System.Reflection.PropertyInfo, DataNode>> DervideMembersDataNodeRoots = new Dictionary<DataNode, Dictionary<System.Reflection.PropertyInfo, DataNode>>();
        ///// <MetaDataID>{f45d8889-8151-4539-bf48-6fb34c86affc}</MetaDataID>
        //internal Storage DataContext;

        /// <MetaDataID>{a8f3d34e-c872-4ad3-ae4d-91e17a1df5fb}</MetaDataID>
        public OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery ObjectQuery { get { return this; } }

        /// <MetaDataID>{52b87c77-06fc-46c1-95f5-bae1c57ba4c1}</MetaDataID>
        QueryTranslator ILINQObjectQuery.Translator
        {
            get
            {
                return _Translator;
            }
        }
        /// <MetaDataID>{91633159-8c5a-4a2d-b371-3f28fa43c215}</MetaDataID>
        internal QueryTranslator _Translator;

        //LINQStorageObjectQuery MasterQuery;
        ///// <MetaDataID>{3a3d2731-dc4e-4531-b250-d02946f7e96f}</MetaDataID>
        //public LINQStorageObjectQuery(System.Linq.Expressions.Expression query, LINQStorageObjectQuery masterQuery)
        //    : base(new OOAdvantech.Collections.Generic.Dictionary<string, object>())
        //{
        //    MasterQuery = masterQuery;
        //    ObjectStorage = masterQuery.ObjectStorage;
        //    DataContext = masterQuery.DataContext;
        //    _Translator = new QueryTranslator(this);
        //    _Translator.Translate(query);

        //    QueryResult.ParticipateInQueryResults(null);
        //    foreach (QueryExpressions.FetchingExpressionTreeNode fetchingExpression in _Translator.FetchingExpressions)
        //        fetchingExpression.ParticipateInSelectList();

        //    if (masterQuery!=null)
        //    {
        //        foreach (var dataNode in DataTrees)
        //            RemoveFreeSearchCondition(dataNode);
        //    }


        //    //var tt = DataTrees[0].BranchSearchConditions;
        //}

        /// <MetaDataID>{e4971541-6b2e-4bca-bdda-c3982dcfc3cf}</MetaDataID>
        private void RemoveFreeSearchCondition(DataNode dataNode)
        {
            if (dataNode.SearchConditions.Contains(null))
                dataNode.SearchConditions.Remove(null);

            foreach (var subDataNode in dataNode.SubDataNodes)
                RemoveFreeSearchCondition(subDataNode);
        }


        public LINQStorageObjectQuery()
            : base(new OOAdvantech.Collections.Generic.Dictionary<string, object>())
        {

        }



        public LINQStorageObjectQuery(System.Linq.Expressions.Expression query, OOAdvantech.Collections.Generic.List<object> objectCollection, Type collectionItemType)
            : base(new OOAdvantech.Collections.Generic.Dictionary<string, object>())
        {
            ObjectsContext = new MetaDataRepository.MemoryObjectsContext(objectCollection, collectionItemType, QueryIdentity);
            _Translator = new QueryTranslator(this);
            _Translator.Translate(query);

            var objectContextReference = new QueryResultObjectContextReference();
            objectContextReference.ObjectQueryContext = this;

            _QueryResultType = new QueryResultType(QueryResult.RootDataNode.HeaderDataNode, objectContextReference);
            QueryResult.ParticipateInQueryResults(null);
            foreach (QueryExpressions.FetchingExpressionTreeNode fetchingExpression in _Translator.FetchingExpressions)
                fetchingExpression.ParticipateInSelectList();

            foreach (var refreshExpression in _Translator.RefreshExpressions)
                refreshExpression.ParticipateInSelectList();

            QueryResult.QueryResult = _QueryResultType;
            BuildQueryresultDataTransform(_QueryResultType, QueryResult, null, new Dictionary<IDynamicTypeDataRetrieve, QueryResultType>());

#if DEBUG
            LastQueryResult = _QueryResultType;

#endif
        }

        /// <MetaDataID>{065ba4b2-2fc1-4821-bc40-7021c840758c}</MetaDataID>
        public LINQStorageObjectQuery(System.Linq.Expressions.Expression query, OOAdvantech.ObjectsContext objectsContext)
            : base(new OOAdvantech.Collections.Generic.Dictionary<string, object>())
        {
            ObjectsContext = objectsContext;

            _Translator = new QueryTranslator(this);
            _Translator.Translate(query);

            var objectContextReference = new QueryResultObjectContextReference();
            objectContextReference.ObjectQueryContext = this;

            _QueryResultType = new QueryResultType(QueryResult.RootDataNode.HeaderDataNode, objectContextReference);
            QueryResult.ParticipateInQueryResults(null);

            foreach (var fetchingExpression in _Translator.FetchingExpressions)
                fetchingExpression.ParticipateInSelectList();

            foreach (var refreshExpression in _Translator.RefreshExpressions)
                refreshExpression.ParticipateInSelectList();

            QueryResult.QueryResult = _QueryResultType;
            BuildQueryresultDataTransform(_QueryResultType, QueryResult, null, new Dictionary<IDynamicTypeDataRetrieve, QueryResultType>());

#if DEBUG
            LastQueryResult = _QueryResultType;

            try
            {
                var dfd = QueryResult.OrderByFilter;
            }
            catch (Exception error)
            {


            }

#endif
        }
        /// <MetaDataID>{9b867627-ed30-49cd-bf8c-c7eb1782fc2e}</MetaDataID>
        public override QueryResultType QueryResultType
        {
            get
            {
                return base.QueryResultType;
            }
            set
            {

            }
        }
        /// <MetaDataID>{8c7d3327-b691-4dbe-b1c1-06a34c93bfe2}</MetaDataID>
        private void BuildQueryresultDataTransform(QueryResultType queryResult, IDynamicTypeDataRetrieve dataRetriever, SearchCondition searchCondition, Dictionary<IDynamicTypeDataRetrieve, QueryResultType> queryResultTypes)
        {

            if (dataRetriever.CollectionProviderMethodExpression != null)
                queryResult.DataFilter = SearchCondition.JoinSearchConditions(searchCondition, dataRetriever.CollectionProviderMethodExpression.FilterDataCondition);
            searchCondition = queryResult.DataFilter;
            if (dataRetriever.CollectionProviderMethodExpression != null &&
                dataRetriever.CollectionProviderMethodExpression.SourceCollection != null &&
                dataRetriever.CollectionProviderMethodExpression.SourceCollection.DynamicTypeDataRetrieve != null)
            {
                QueryResultType type = new QueryResultType(dataRetriever.CollectionProviderMethodExpression.SourceCollection.DynamicTypeDataRetrieve.RootDataNode, queryResult.ObjectQueryContextReference);

                dataRetriever.CollectionProviderMethodExpression.SourceCollection.DynamicTypeDataRetrieve.QueryResult = type;
                BuildQueryresultDataTransform(type, dataRetriever.CollectionProviderMethodExpression.SourceCollection.DynamicTypeDataRetrieve, searchCondition, queryResultTypes);

                queryResultTypes[dataRetriever.CollectionProviderMethodExpression.SourceCollection.DynamicTypeDataRetrieve] = type;

                type.OrderByFilter = dataRetriever.OrderByFilter;



                //foreach (var dataNode in dataRetriever.OrderByDataNodes)
                //    type.AddOrderByDataNode(dataNode);

                //if (dataRetriever.CollectionProviderMethodExpression.SourceCollection is Linq.QueryExpressions.WhereExpressionTreeNode)

            }
            


            if (dataRetriever.Properties != null)
            {
                foreach (var property in dataRetriever.Properties.Values)
                {
                    if (property.IsLocalScopeValue)
                        continue;
                    else if (!property.PropertyTypeIsEnumerable && (!property.PropertyTypeIsDynamic || (property.PropertyType.Properties == null && property.PropertyType.GroupingMetaData == null)))
                    {
                        SinglePart singlePart = new SinglePart(property.SourceDataNode, property.PropertyName, queryResult);
                        queryResult.AddMember(singlePart);
                        property.QueryResultMember = singlePart;
                    }
                    else if (property.PropertyTypeIsEnumerable)
                    {
                        QueryResultType type = null;
                        if (property.PropertyType == null)
                        {
                            type = new QueryResultType(property.SourceDataNode, queryResult.ObjectQueryContextReference);
                            type.DataFilter = SearchCondition.JoinSearchConditions(searchCondition, property.TreeNode.FilterDataCondition);
                            type.ValueDataNode = property.SourceDataNode;
                        }
                        else if (!queryResultTypes.TryGetValue(property.PropertyType, out type))
                        {
                            if (property.PropertyType.Properties == null && property.PropertyType.MemberDataNode != null)
                                type = new QueryResultType(property.PropertyType.MemberDataNode, queryResult.ObjectQueryContextReference);
                            else
                                type = new QueryResultType(property.PropertyType.RootDataNode, queryResult.ObjectQueryContextReference);
                            property.PropertyType.QueryResult = type;
                            BuildQueryresultDataTransform(type, property.PropertyType, searchCondition, queryResultTypes);
                            queryResultTypes[property.PropertyType] = type;
                        }
                        EnumerablePart compositePart = new EnumerablePart(type, property.PropertyName, queryResult);

                        queryResult.AddMember(compositePart);
                        property.QueryResultMember = compositePart;
                    }
                    else if (property.PropertyTypeIsDynamic)
                    {
                        QueryResultType type = null;
                        if (!queryResultTypes.TryGetValue(property.PropertyType, out type))
                        {
                            type = new QueryResultType(property.PropertyType.RootDataNode, queryResult.ObjectQueryContextReference);
                            property.PropertyType.QueryResult = type;
                            BuildQueryresultDataTransform(type, property.PropertyType, searchCondition, queryResultTypes);
                        }
                        if (property.PropertyType.GroupingMetaData != null)
                        {
                            EnumerablePart compositePart = new EnumerablePart(type, property.PropertyName, queryResult);
                            queryResultTypes[property.PropertyType] = type;
                            queryResult.AddMember(compositePart);
                            property.QueryResultMember = compositePart;
                        }
                        else
                        {
                            CompositePart compositePart = new CompositePart(type, property.PropertyName, queryResult);
                            queryResultTypes[property.PropertyType] = type;
                            queryResult.AddMember(compositePart);
                            property.QueryResultMember = compositePart;
                        }
                    }
                }
            }
            else if (dataRetriever.GroupingMetaData != null)
            {

                //QueryResult type = new QueryResult(dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve.RootDataNode);
                //queryResultTypes[dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve] = type;
                //dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve.QueryResult = type;
                //CompositePart compositePart = new CompositePart(type, "Key");
                //BuildQueryresultDataTransform(type, dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve,null, queryResultTypes);
                //queryResult.AddMember(compositePart);


                //if (!queryResultTypes.TryGetValue(dataRetriever.GroupingMetaData.GroupedDataRetrieve, out type))
                //{
                //    type = new QueryResult(dataRetriever.GroupingMetaData.GroupedDataRetrieve.RootDataNode);
                //    BuildQueryresultDataTransform(type, dataRetriever.GroupingMetaData.GroupedDataRetrieve,null, queryResultTypes);
                //}
                //queryResultTypes[dataRetriever.GroupingMetaData.GroupedDataRetrieve] = type;
                //EnumerablePart groupedDataPart = new EnumerablePart(type, "GroupedData");
                //queryResult.AddMember(groupedDataPart);



                QueryResultType type = null;
                if (dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve != null && !queryResultTypes.TryGetValue(dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve, out type))
                {
                    type = new QueryResultType(dataRetriever.GroupingMetaData.GroupDataNode.KeyDataNode, queryResult.ObjectQueryContextReference);
                    queryResultTypes[dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve] = type;
                    dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve.QueryResult = type;
                    BuildQueryresultDataTransform(type, dataRetriever.GroupingMetaData.KeyDynamicTypeDataRetrieve, null, queryResultTypes);
                    CompositePart compositePart = new CompositePart(type, "Key", queryResult);
                    queryResult.AddMember(compositePart);
                }
                else
                {
                    SinglePart singlePat = new SinglePart(dataRetriever.GroupingMetaData.GroupDataNode.KeyDataNode, "Key", queryResult);
                    queryResult.AddMember(singlePat);
                }

                if (!queryResultTypes.TryGetValue(dataRetriever.GroupingMetaData.GroupedDataRetrieve, out type))
                {
                    type = new QueryResultType(dataRetriever.GroupingMetaData.GroupedDataRetrieve.RootDataNode, queryResult.ObjectQueryContextReference);
                    dataRetriever.GroupingMetaData.GroupedDataRetrieve.QueryResult = type;
                    BuildQueryresultDataTransform(type, dataRetriever.GroupingMetaData.GroupedDataRetrieve, null, queryResultTypes);
                }
                queryResultTypes[dataRetriever.GroupingMetaData.GroupedDataRetrieve] = type;
                EnumerablePart groupedDataPart = new EnumerablePart(type, "GroupedData", queryResult);
                queryResult.AddMember(groupedDataPart);
            }
            if (dataRetriever.Properties == null &&
                dataRetriever.GroupingMetaData == null &&
                dataRetriever.MemberDataNode != null)
            {
                queryResult.ValueDataNode = dataRetriever.MemberDataNode;
                if (dataRetriever.MemberDataNode.Type == DataNode.DataNodeType.Average)
                {
                    SinglePart averageMember = new SinglePart(dataRetriever.MemberDataNode, "Average", queryResult);
                    queryResult.AddMember(averageMember);
                }

                if (dataRetriever.MemberDataNode.Type == DataNode.DataNodeType.Group)
                {

                    var sourceCollectionDataRetrieve = (dataRetriever.CollectionProviderMethodExpression as Linq.QueryExpressions.SelectExpressionTreeNode).SourceCollection.DynamicTypeDataRetrieve;





                    QueryResultType type = null;
                    if (!queryResultTypes.TryGetValue(sourceCollectionDataRetrieve, out type))
                    {
                        type = new QueryResultType(sourceCollectionDataRetrieve.RootDataNode, queryResult.ObjectQueryContextReference);
                        sourceCollectionDataRetrieve.QueryResult = type;
                        BuildQueryresultDataTransform(type, sourceCollectionDataRetrieve, searchCondition, queryResultTypes);
                    }

                    if (sourceCollectionDataRetrieve.GroupingMetaData != null)
                    {
                        EnumerablePart compositePart = new EnumerablePart(type, "GroupedData", queryResult);
                        queryResultTypes[sourceCollectionDataRetrieve] = type;
                        queryResult.AddMember(compositePart);
                    }




                }

            }

            // foreach group by member adds group by key member
            var groupByMembers = (from member in QueryResultType.Members
                                 where member.SourceDataNode.Type == DataNode.DataNodeType.Group
                                 select member).ToList();
            foreach (var groupByMember in groupByMembers)
            {
                if (groupByMember != null)
                {
                    if ((from member in QueryResultType.Members
                         where member.SourceDataNode.Type == DataNode.DataNodeType.Key
                         select member).FirstOrDefault() == null)
                    {
                        QueryResultType.InsertMember(0, ((groupByMember as EnumerablePart).Type.Members[0]));
                    }

                }
            }


            queryResult.OrderByFilter = dataRetriever.OrderByFilter;

        }

        public static Type GetType(Type key, Type element)
        {
            return typeof(SelectedGroupedData<,>).MakeGenericType(key, element);

        }


        //public LINQStorageObjectQuery(System.Linq.Expressions.Expression query, object dataSourceRootObject)
        //    : base(new OOAdvantech.Collections.Generic.Dictionary<string, object>())
        //{
        //    //ObjectStorage = dataContext.ObjectStorage;
        //    //DataContext = dataContext;
        //    _Translator = new QueryTranslator(this);
        //    _Translator.Translate(query);
        //    var tt = DataTrees[0].BranchSearchConditions;
        //}


        ///// <MetaDataID>{fa300826-7c7a-4b5d-b908-481d178482b5}</MetaDataID>
        //internal LINQStorageObjectQuery(System.Linq.Expressions.Expression query)
        //    : base(new OOAdvantech.Collections.Generic.Dictionary<string, object>())
        //{
        //    _Translator = new QueryTranslator(this);
        //    _Translator.Translate(query);

        //    QueryResult.ParticipateInQueryResults(null);
        //    foreach (QueryExpressions.FetchingExpressionTreeNode fetchingExpression in _Translator.FetchingExpressions)
        //        fetchingExpression.ParticipateInSelectList(); 

        //    foreach (DataNode dataNode in _Translator.RootPaths)
        //        DataTrees.Add(dataNode);
        //    string errors = null;
        //    BuildDataNodeTree(ref errors);
        //    //_Translator.BuildSearchCondition();
        //    foreach (DataNode dataNode in DataTrees)
        //        dataNode.MergeSearchConditions();

        //}
        ///// <MetaDataID>{4682e822-4f75-414b-ba75-983518bb29c2}</MetaDataID>
        //public LINQStorageObjectQuery()
        //    : base(null)
        //{
        //}
        /// <MetaDataID>{8e819211-c513-405c-8732-eee6f9725e59}</MetaDataID>
        public override OOAdvantech.Collections.Generic.List<DataNode> SelectListItems
        {
            get
            {
                return _SelectListItems;
            }
        }

        /// <MetaDataID>{0244b9a3-cb9f-4a87-8db2-fa5bb1facd9b}</MetaDataID>
        internal void Execute()
        {
#if DeviceDotNet
            OOAdvantech.Collections.Generic.List<DataNode> dataTrees = new OOAdvantech.Collections.Generic.List<DataNode>();
            foreach (DataNode dataNode in DataTrees)
                dataTrees.AddRange(dataNode.RemoveNamespacesDataNodes());
            DataTrees = dataTrees;

            foreach (DataNode dataNode in DataTrees)
                CreateDataSources(dataNode, null);


            LoadData();
            foreach (DataNode dataNode in DataTrees)
                BuildEmptyDataSource(dataNode);
#else
            if (ObjectsContext is MemoryObjectsContext && (ObjectsContext as MemoryObjectsContext).ObjectsHasOutProcessCommonMemoryContext)
            {
                LINQStorageObjectQuery remoteLINQObjectQuery = Remoting.RemotingServices.CreateRemoteInstance<LINQStorageObjectQuery>((ObjectsContext as MemoryObjectsContext).CommonChannelUri);
                MemoryObjectsContext outProcessMemoryObjectsContext = Remoting.RemotingServices.CreateRemoteInstance<MemoryObjectsContext>((ObjectsContext as MemoryObjectsContext).CommonChannelUri,
                                                                                                                                    new Type[3] { typeof(Collections.Generic.List<object>), typeof(Type), typeof(Guid) },
                                                                                                                                      (ObjectsContext as MemoryObjectsContext).ObjectCollection, (ObjectsContext as MemoryObjectsContext).CollectionItemType, remoteLINQObjectQuery.QueryIdentity);
                MemoryObjectsContext.RemoveAllQueryMemoryContext(QueryIdentity);

                OOAdvantech.Collections.Generic.Dictionary<DataNode, Type> dataTreesRootsTypes = new OOAdvantech.Collections.Generic.Dictionary<DataNode, Type>();
                OOAdvantech.Collections.Generic.List<DataNode> dataTrees = new OOAdvantech.Collections.Generic.List<DataNode>(DataTrees);
                QueryResultType queryResultType = QueryResultType;

                remoteLINQObjectQuery.StorageServerExecute(ref dataTrees, ref queryResultType, Parameters, outProcessMemoryObjectsContext);
                _QueryResultType.AddDistributedQueryResult(queryResultType);

                foreach (DataNode dataNode in DataTrees)
                    dataNode.ObjectQuery = this;

            }
            else if (ObjectsContext.GetType().GetMetaData().GetField("ServerSideObjectStorage") != null)
            {

                ObjectsContext = ObjectsContext.GetType().GetMetaData().GetField("ServerSideObjectStorage").GetValue(ObjectStorage) as PersistenceLayer.ObjectStorage;
                LINQStorageObjectQuery remoteLINQObjectQuery = Remoting.RemotingServices.CreateRemoteInstance<LINQStorageObjectQuery>(Remoting.RemotingServices.GetChannelUri(ObjectStorage));
                OOAdvantech.Collections.Generic.Dictionary<DataNode, Type> dataTreesRootsTypes = new OOAdvantech.Collections.Generic.Dictionary<DataNode, Type>();
                OOAdvantech.Collections.Generic.List<DataNode> dataTrees = new OOAdvantech.Collections.Generic.List<DataNode>(DataTrees);
                QueryResultType queryResultType = QueryResultType;
                remoteLINQObjectQuery.StorageServerExecute(ref dataTrees, ref queryResultType, Parameters, ObjectStorage);
                _QueryResultType.AddDistributedQueryResult(queryResultType);

                foreach (DataNode dataNode in DataTrees)
                    dataNode.ObjectQuery = this;


            }
            else
            {
                OOAdvantech.Collections.Generic.List<DataNode> dataTrees = new OOAdvantech.Collections.Generic.List<DataNode>();
                foreach (DataNode dataNode in DataTrees)
                    dataTrees.AddRange(dataNode.RemoveNamespacesDataNodes());
                DataTrees = dataTrees;

                foreach (DataNode dataNode in DataTrees)
                    CreateDataSources(dataNode, null);


                LoadData();
                foreach (DataNode dataNode in DataTrees)
                    BuildEmptyDataSource(dataNode);
            }
#endif

        }

        /// <MetaDataID>{ceb1ff29-24f7-49ce-9270-379ed37d8142}</MetaDataID>
        private void BuildEmptyDataSource(DataNode dataNode)
        {
            if (dataNode.Type == DataNode.DataNodeType.Object && dataNode.DataSource == null)
                dataNode.DataSource = new StorageDataSource(dataNode, DataSource.DataObjectsInstantiator.CreateDataTable(false));
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                BuildEmptyDataSource(subDataNode);
        }

        ///<summary>
        ///Execute linq query in storage server process
        ///</summary>
        ///<param name="dataTrees">
        ///Defines meta data (DataNode Trees) for data retrieving grouping and filtering
        ///</param>
        ///<param name="parameters">
        ///Defines the parameter values of linq query
        ///</param>
        ///<param name="objectStorage">
        ///Defines the storage of data wich use the linq query to retrieve data.
        ///</param>
        ///<returns> 
        ///Returns formed linq serialized data.
        ///</returns>
        /// <MetaDataID>{45f24edd-55f3-421d-9dc9-d6372b7d2a9f}</MetaDataID>
        public void StorageServerExecute(ref OOAdvantech.Collections.Generic.List<DataNode> dataTrees,
                                                     ref QueryResultType queryResultType,
                                                    OOAdvantech.Collections.Generic.Dictionary<string, object> parameters,
                                                    OOAdvantech.ObjectsContext objectsContext)
        {
            DataTrees = dataTrees;
            foreach (DataNode dataNode in DataTrees)
                dataNode.ObjectQuery = this;
            _QueryResultType = queryResultType;
            _QueryResultType.ObjectQueryContextReference.ObjectQueryContext = this;
            string errors = null;
            BuildDataNodeTree(ref errors);
            Parameters = parameters;
            ObjectsContext = objectsContext;
            Execute();


            System.Collections.Generic.Dictionary<Guid, DataSource> dataSources = new Dictionary<Guid, DataSource>();
            foreach (DataNode dataNode in DataTrees)
                dataNode.GetDataSources(ref dataSources);
            foreach (DataSource dataSource in dataSources.Values)
                dataSource.CachingRelationshipData();

            if (QueryResultType != null)
                QueryResultType.GetData();

            queryResultType = QueryResultType;

        }




        #region ILINQObjectQuery Members
        ///// <exclude>Excluded</exclude>
        //IDynamicTypeDataRetrieve _SearchResult;
        ///// <summary>
        ///// This property defines the result of query as enumerator 
        ///// </summary>
        ///// <MetaDataID>{c32e9f2d-b58e-48da-b8d5-5b0dec6bc025}</MetaDataID>
        //internal System.Collections.IEnumerator SearchResult
        //{
        //    get
        //    {
        //        return _SearchResult as System.Collections.IEnumerator;
        //    }
        //}


        /// <exclude>Excluded</exclude>
        IDynamicTypeDataRetrieve _QueryResult;
        /// <MetaDataID>{1fc8776f-cb87-442a-ac49-1dcb8efa660c}</MetaDataID>
        public IDynamicTypeDataRetrieve QueryResult
        {
            get
            {
                return _QueryResult;
            }
            set
            {
                _QueryResult = value;
            }
        }

        ///// <MetaDataID>{34495578-8c59-4e84-a3b2-101831d0dea1}</MetaDataID>
        //IDynamicTypeDataRetrieve ILINQObjectQuery.SearchResult
        //{
        //    get
        //    {
        //        return _SearchResult;
        //    }
        //    set
        //    {
        //        _SearchResult = value;
        //    }
        //}

        #endregion
    }


}

