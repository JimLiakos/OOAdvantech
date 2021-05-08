using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{


    ///<summary>
    ///Defines a query results Type
    ///the QueryResultType contains members where can be native type or query result type or enumerblae 
    ///</summary>
    ///<MetaDataID>{dba97d23-5b30-42ed-816a-57684a50d714}</MetaDataID>
    [Serializable]

    public class QueryResultType
    {

        /// <exclude>Excluded</exclude>
        DataOrderBy _OrderByFilter;

        [RoleAMultiplicityRange(0, 1)]
        [Association("QueryResultOrderBy", Roles.RoleA, "6088b515-488a-4581-8170-5c64b5d005f0")]
        [ImplementationMember(nameof(_OrderByFilter))]
        public DataOrderBy OrderByFilter
        {
            get
            {
                if (_OrderByFilter != null)
                {
                    foreach (var field in _OrderByFilter.Fields)
                    {
                        if (field.DataNode == null || ObjectQueryContextReference.GetDataNode(field.DataNodeIdentity) != field.DataNode)
                            field.DataNode = ObjectQueryContextReference.GetDataNode(field.DataNodeIdentity);
                    }
                }
                return _OrderByFilter;
            }
            set
            {
                if (_OrderByFilter != null && value == null)
                    return;
                if (_OrderByFilter != null && value != _OrderByFilter)
                    throw new NotSupportedException("multiple order by filter doesn't supported");

                    _OrderByFilter = value;
            }
        }


        // public void AddOrderBy

        /// <MetaDataID>{cc2ee422-0b6f-4649-bae0-cbe2866b2fe7}</MetaDataID>
        virtual internal QueryResultType Clone(Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as QueryResultType;
            QueryResultType newQueryResultType = new QueryResultType();
            clonedObjects[this] = newQueryResultType;

            newQueryResultType._CanLoadDataLocal = _CanLoadDataLocal;

            newQueryResultType._OrderByFilter = _OrderByFilter;
            foreach (var entry in DataNodeRowIndices)
                newQueryResultType.DataNodeRowIndices.Add(entry.Key.Clone(clonedObjects), entry.Value);

            if (_DataRetrievePath != null)
            {
                foreach (var dataRetrieveDataNode in _DataRetrievePath)
                {
                    object newDataRetrieveDataNode = null;
                    if (clonedObjects.TryGetValue(dataRetrieveDataNode, out newDataRetrieveDataNode))
                        newQueryResultType._DataRetrievePath.AddLast(newDataRetrieveDataNode as DataRetrieveNode);
                    else
                        newQueryResultType._DataRetrievePath.AddLast(dataRetrieveDataNode.Clone(clonedObjects, newQueryResultType._DataRetrievePath, newQueryResultType.DataNodeRowIndices));
                }
            }

            newQueryResultType.ExtensionRowColumnsCount = ExtensionRowColumnsCount;
            newQueryResultType.Identity = Identity;

            newQueryResultType.ObjectQueryContextReference = ObjectQueryContextReference.Clone(clonedObjects);

            newQueryResultType.RootDataNodeIdentity = RootDataNodeIdentity;
            newQueryResultType.RootDataNodeIndices = RootDataNodeIndices;
            newQueryResultType.ValueDataNodeIdentity = ValueDataNodeIdentity;
            newQueryResultType._CanLoadDataLocal = _CanLoadDataLocal;
            newQueryResultType._OrderByFilter = _OrderByFilter;
            if (_MembersRootDataNode != null)
                newQueryResultType._MembersRootDataNode = _MembersRootDataNode.Clone(clonedObjects);
            foreach (var member in _Members)
            {
                newQueryResultType._Members.Add(member.Clone(clonedObjects));

            }


            newQueryResultType.ConventionTypeColumnIndex = ConventionTypeColumnIndex;
            newQueryResultType.ConventionTypeRowIndex = ConventionTypeRowIndex;
            if (DataFilter != null)
                newQueryResultType.DataFilter = DataFilter.Clone(clonedObjects);
            newQueryResultType.DataFilterApplied = DataFilterApplied;
            foreach (var entry in DataFilterConstraintParts)
                newQueryResultType.DataFilterConstraintParts[entry.Key.Clone(clonedObjects)] = entry.Value.Clone(clonedObjects);

            newQueryResultType.DataLoader = DataLoader.Clone(clonedObjects);

            if (MembersIndices != null)
            {
                foreach (var membersIndicesEntry in MembersIndices)
                    newQueryResultType.MembersIndices[membersIndicesEntry.Key.Clone(clonedObjects)] = membersIndicesEntry.Value;
            }
            foreach (DataNode dataNode in SelectionDataNodesAsBranch)
                newQueryResultType.SelectionDataNodesAsBranch.Add(dataNode.Clone(clonedObjects));

            return newQueryResultType;
        }
        /// <MetaDataID>{634313a9-4f87-4626-9c51-6030d101b70c}</MetaDataID>
        public void GetData()
        {
            DataLoader.LoadData();
        }

        /// <summary>
        /// Defines the DataLoader which load and format the data for query result type
        /// </summary>
        [Association("", typeof(QueryResultDataLoader), Roles.RoleA, "6d966e6e-bf2a-47fa-9e27-2f1cee4539a2")]
        [IgnoreErrorCheck]
        public QueryResultDataLoader DataLoader;

        /// <MetaDataID>{e2867bf7-2009-4e38-8031-df358a02d5d6}</MetaDataID>
        public override string ToString()
        {
            string description = null;
            foreach (var part in Members)
            {
                if (description != null)
                    description += ",";
                description += part.ToString();
            }
            return RootDataNode.Name + " (" + description + ")";
        }


        #region Query results data filtering


        /// <MetaDataID>{d88b6406-206d-46a1-ba6a-2f0b48a447d9}</MetaDataID>
        public SearchCondition _DataFilter;

        [Association("", Roles.RoleA, "2a43d120-58b6-43b2-ad7e-ee4f5fb5fe34")]
        [IgnoreErrorCheck]
        public SearchCondition DataFilter
        {
            get
            {
                return _DataFilter;
            }
            set
            {
                _DataFilter = value;
            }
        }

        /// <MetaDataID>{a4102205-6c62-47f9-b123-d86e0c0a3244}</MetaDataID>
        internal bool DataFilterApplied;


        /// <summary>
        /// This method check if the composite row pass the data filter
        /// checks the composite row even when doesn't loaded complite 
        /// </summary>
        /// <param name="compositeRow">
        /// Defines the checked Row
        /// </param>
        /// <param name="linkedListNode">
        /// Defines the node of DataRetrievPath
        /// </param>
        /// <returns>
        /// Return true if the row pass the data filter
        /// </returns>
        /// <MetaDataID>{19ef7e17-db6d-42e0-a7f5-0c052112552b}</MetaDataID>
        internal bool DoesRowPassCondition(IDataRow[] compositeRow, LinkedListNode<DataRetrieveNode> linkedListNode)
        {
            if (DataFilter == null || DataFilterApplied)
                return true;
            if (linkedListNode.Next == null)
                return DataFilter.DoesRowPassCondition(compositeRow, DataNodeRowIndices);
            else
            {
                SearchCondition dataFilter = GetDataFilterConstraintPart(linkedListNode);
                //Gets the datafilter wich can be applied to the row for the current Node and previous of DataRetrievePath which loads data in row
                if (linkedListNode.Previous != null)
                {
                    SearchCondition previusNodeDataFilter = GetDataFilterConstraintPart(linkedListNode.Previous);
                    if (previusNodeDataFilter == dataFilter)
                        return true; // DataFilter allready applied
                }
                if (dataFilter != null && !dataFilter.DoesRowPassCondition(compositeRow, DataNodeRowIndices))
                    return false; // If constraintPart of data filter reject the composite row return false
                else
                    return true; // else continue to complet load of composite row
            }

        }
        /// <summary>
        /// Defines a dictionary with dataRetrievePath dataNodes as key and constraint search condition sa value
        /// </summary>
        /// <MetaDataID>{206ab127-85ff-4f65-992a-8871edd007b1}</MetaDataID>
        Dictionary<DataNode, SearchCondition> DataFilterConstraintParts = new Dictionary<DataNode, SearchCondition>();

        /// <summary>
        /// This method provides the constraint data filter as part of main DataFilter for DataRetrievePath node.
        /// Some times a constraint part of DataFilter can reject composite row before complete load
        /// </summary>
        /// <param name="linkedListNode">
        /// Defines the DataretrievePath node
        /// </param>
        /// <returns>
        /// The constraint part of DataFilter 
        /// </returns>
        /// <MetaDataID>{3756aacc-0b49-4516-ac60-159c3fc2d700}</MetaDataID>
        private SearchCondition GetDataFilterConstraintPart(LinkedListNode<DataRetrieveNode> linkedListNode)
        {
            if (DataFilter == null)
                return null;
            SearchCondition dataFilterConstraintPart = null;
            if (!DataFilterConstraintParts.TryGetValue(linkedListNode.Value.DataNode, out dataFilterConstraintPart))
            {
                List<DataNode> dataNodes = new List<DataNode>();
                LinkedListNode<DataRetrieveNode> currentLinkedListNode = linkedListNode;
                do
                {
                    dataNodes.Add(currentLinkedListNode.Value.DataNode);
                    currentLinkedListNode = currentLinkedListNode.Previous;
                }
                while (currentLinkedListNode != null);
                dataFilterConstraintPart = GetDataFilterConstraintPart(DataFilter, dataNodes);
                DataFilterConstraintParts[linkedListNode.Value.DataNode] = dataFilterConstraintPart;
            }

            return dataFilterConstraintPart;
        }



        /// <summary>
        /// This method extract the constraint data filter part from search condition which can be applied to the dataNodes paraqmeter
        /// </summary>
        /// <param name="searchCondition">
        /// This parameter defines the search condion which use the method to extract the constraint part
        /// </param>
        /// <param name="dataNodes">
        /// Defines the datanodes which is available to acts the data filter constraint part
        /// </param>
        /// <returns>
        /// return the constraint part of search condition
        /// </returns>
        /// <MetaDataID>{079f7ee5-e855-4d4a-ae0f-fbff0c6c5a80}</MetaDataID>
        private SearchCondition GetDataFilterConstraintPart(SearchCondition searchCondition, List<DataNode> dataNodes)
        {
            bool dataNodeMismatch = false;
            foreach (var dataNode in searchCondition.DataNodes)
            {
                DataNode filterDataNode = dataNode;
                while (filterDataNode.Type != DataNode.DataNodeType.Object && filterDataNode.Type != DataNode.DataNodeType.Group && filterDataNode.ParentDataNode != null)
                    filterDataNode = filterDataNode.ParentDataNode;


                if (!dataNodes.Contains(filterDataNode))
                {
                    dataNodeMismatch = true;
                    break;
                }
            }
            if (dataNodeMismatch)
            {
                if (searchCondition.SearchTerms.Count > 1)
                    return null;
                {
                    foreach (var searchFactor in searchCondition.SearchTerms[0].SearchFactors)
                    {
                        SearchCondition dataFilterConstraintPart = GetDataFilterConstraintPart(searchFactor, dataNodes);
                        if (dataFilterConstraintPart != null)
                            return dataFilterConstraintPart;
                    }
                    return null;
                }

            }
            else
                return searchCondition;
        }
        /// <summary>
        /// This method extract the constraint data filter part from search condition factor which can be applied to the dataNodes paraqmeter
        /// </summary>
        /// <param name="searchFactor">
        /// This parameter defines the search condion factor which use the method to extract the constraint part
        /// </param>
        /// <param name="dataNodes">
        /// Defines the datanodes which is available to acts the data filter constraint part
        /// </param>
        /// <returns>
        /// return the constraint part of search condition
        /// </returns>
        /// <MetaDataID>{9bc22507-b2a3-4e45-831b-e173b8a9ce9f}</MetaDataID>
        private SearchCondition GetDataFilterConstraintPart(SearchFactor searchFactor, List<DataNode> dataNodes)
        {

            bool dataNodeMismatch = false;
            foreach (var dataNode in searchFactor.DataNodes)
            {
                DataNode filterDataNode = dataNode;
                while (filterDataNode.Type != DataNode.DataNodeType.Object && filterDataNode.Type != DataNode.DataNodeType.Group && filterDataNode.ParentDataNode != null)
                    filterDataNode = filterDataNode.ParentDataNode;


                if (!dataNodes.Contains(filterDataNode))
                {
                    dataNodeMismatch = true;
                    break;
                }
            }

            if (dataNodeMismatch)
            {
                if (searchFactor.SearchCondition == null)
                    return null;
                SearchCondition searchCondition = searchFactor.SearchCondition;
                if (searchCondition.SearchTerms.Count > 1)
                    return null;
                {
                    foreach (var searchFactorAsConstraintPart in searchCondition.SearchTerms[0].SearchFactors)
                    {
                        SearchCondition dataFilterConstraintPart = GetDataFilterConstraintPart(searchFactorAsConstraintPart, dataNodes);
                        if (dataFilterConstraintPart != null)
                            return dataFilterConstraintPart;
                    }
                    return null;
                }

            }
            else
                return new SearchCondition(new List<SearchTerm>() { new SearchTerm(new List<SearchFactor>() { searchFactor }) }, searchFactor.DataNodes[0].ObjectQuery);
        }

        #endregion

        /// <summary>
        /// defines the identity of query result type
        /// </summary>
        /// <MetaDataID>{b78c2894-8d81-4f2b-a9ce-4c879cf1c3c7}</MetaDataID>
        internal Guid Identity;


        ///<summary>
        ///Defines the ObjecQuery context reference wich connect QueryResultType with objectQuery
        ///The query result can be moved between the master query and distributed query and the query engine can change the QueryResultType context
        ///</summary>
        /// <MetaDataID>{d83f2808-7212-4a60-ade3-2aad7b0e0194}</MetaDataID>
        public QueryResultObjectContextReference ObjectQueryContextReference;


        ///<summary>
        ///Initialize new instance with root data node and  object context connection
        ///</summary>
        ///<param name="rootDataNode">
        ///Defines the DataNode which use the QueryResultType to construct  the data retrieve graph
        ///</param>
        ///<param name="objectQueryContextReference">
        ///Defines the ObjecQuery context reference wich connect QueryResultType with objectQuery
        ///The query result can be moved between the master query and distributed query and the query engine can change the QueryResultType context
        ///</param>
        /// <MetaDataID>{f569f116-3057-4e08-98d9-92162c8d80b6}</MetaDataID>
        public QueryResultType(DataNode rootDataNode, QueryResultObjectContextReference objectQueryContextReference)
        {

            Identity = Guid.NewGuid();
            ObjectQueryContextReference = objectQueryContextReference;
            RootDataNode = rootDataNode;
            DataLoader = new QueryResultDataLoader(this, objectQueryContextReference);

        }

        /// <MetaDataID>{fb2a1d36-1aec-4d98-b936-944056ace4d7}</MetaDataID>
        public QueryResultType()
        {
            // TODO: Complete member initialization
        }

        /// <summary>
        /// Defines all Memembers of this type in query resulats hierarchy 
        /// </summary>
        /// <MetaDataID>{7a286936-e48d-41c8-95c5-dd1a65086a32}</MetaDataID>
        public List<QueryResultPart> MembersWhereParticipate
        {
            get
            {
                return GetMembersWhereParticipate(this.ObjectQueryContextReference.ObjectQueryContext.QueryResultType);
            }
        }

        /// <summary>
        /// This method traverse hierarcy of QueryResultType to find the members of this type
        /// </summary>
        /// <param name="queryResultType">
        /// Defines the query results hierarcy root Type
        /// </param>
        /// <returns>
        /// List of members
        /// </returns>
        /// <MetaDataID>{620e8e1f-d802-458f-8cd1-9b220ef684ce}</MetaDataID>
        private List<QueryResultPart> GetMembersWhereParticipate(QueryResultType queryResultType)
        {
            List<QueryResultPart> thisTypeMembers = new List<QueryResultPart>();
            foreach (var member in queryResultType.Members)
            {
                if ((member is EnumerablePart && (member as EnumerablePart).Type.Identity == Identity) || (member is CompositePart && (member as CompositePart).Type.Identity == Identity))
                    thisTypeMembers.Add(member);
            }
            return thisTypeMembers;
        }

        ///<summary>
        ///Defines all data nodes which uses QueryResultType to retrieve data  
        ///</summary>
        /// <MetaDataID>{de0a15b9-5fbd-4ba7-9548-49546a4e63fe}</MetaDataID>
        public List<DataNode> DataLoaderDataNodes
        {
            get
            {
                List<DataNode> dataNodes = new List<DataNode>();
                foreach (var member in Members)
                {
                    DataNode memberDataNode = null;

                    if (member is SinglePart || member is EnumerablePart)
                    {
                        if (member is SinglePart)
                            memberDataNode = (member as SinglePart).DataNode;
                        if (member is EnumerablePart)
                            memberDataNode = (member as EnumerablePart).Type.RootDataNode;

                        while (DerivedDataNode.GetOrgDataNode(memberDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                            memberDataNode = memberDataNode.ParentDataNode;
                        if (DerivedDataNode.GetOrgDataNode(memberDataNode) is AggregateExpressionDataNode)
                            memberDataNode = memberDataNode.ParentDataNode;
                        if (DerivedDataNode.GetOrgDataNode(memberDataNode).Type == DataNode.DataNodeType.Key)
                            memberDataNode = memberDataNode.ParentDataNode;
                        if (!dataNodes.Contains(memberDataNode))
                            dataNodes.Add(memberDataNode);
                    }

                    if (member is CompositePart)
                        foreach (DataNode dataNode in (member as CompositePart).Type.DataLoaderDataNodes)
                            if (!dataNodes.Contains(dataNode))
                                dataNodes.Add(dataNode);
                    if (member is EnumerablePart)
                        foreach (DataNode dataNode in (member as EnumerablePart).Type.DataLoaderDataNodes)
                            if (!dataNodes.Contains(dataNode))
                                dataNodes.Add(dataNode);
                    if (member.SourceDataNode is AggregateExpressionDataNode && (member.SourceDataNode as AggregateExpressionDataNode).SourceSearchCondition != null)
                    {
                        foreach (var dataNode in (member.SourceDataNode as AggregateExpressionDataNode).SourceSearchCondition.DataNodes)
                        {
                            var dataFilterDataNode = dataNode;
                            while (DerivedDataNode.GetOrgDataNode(dataFilterDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                                dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                            if (DerivedDataNode.GetOrgDataNode(dataFilterDataNode) is AggregateExpressionDataNode)
                                dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                            if (DerivedDataNode.GetOrgDataNode(dataFilterDataNode).Type == DataNode.DataNodeType.Key)
                                dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                            if (!dataNodes.Contains(dataFilterDataNode))
                                dataNodes.Add(dataFilterDataNode);
                        }
                    }
                }
                if (RootDataNode is GroupDataNode)
                {
                    if (!RootDataNode.DataSource.GroupedDataLoaded)
                    {
                        if (!dataNodes.Contains((RootDataNode as GroupDataNode).GroupedDataNode))
                            dataNodes.Add((RootDataNode as GroupDataNode).GroupedDataNode);
                    }
                    if (!RootDataNode.DataSource.GroupedDataLoaded && (RootDataNode as GroupDataNode).GroupingSourceSearchCondition != null)
                    {

                        foreach (var dataNode in (RootDataNode as GroupDataNode).GroupingSourceSearchCondition.DataNodes)
                        {
                            var dataFilterDataNode = dataNode;
                            while (DerivedDataNode.GetOrgDataNode(dataFilterDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                                dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                            if (DerivedDataNode.GetOrgDataNode(dataFilterDataNode) is AggregateExpressionDataNode)
                                dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                            if (DerivedDataNode.GetOrgDataNode(dataFilterDataNode).Type == DataNode.DataNodeType.Key)
                                dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                            if (!dataNodes.Contains(dataFilterDataNode))
                                dataNodes.Add(dataFilterDataNode);
                        }
                    }

                }
                if (ValueDataNode is AggregateExpressionDataNode && !ValueDataNode.ParentDataNode.DataSource.AggregateExpressionDataNodeResolved(ValueDataNode as AggregateExpressionDataNode))
                {
                    foreach (var dataNode in (ValueDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                    {
                        var dataFilterDataNode = dataNode;
                        while (DerivedDataNode.GetOrgDataNode(dataFilterDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                            dataFilterDataNode = dataFilterDataNode.ParentDataNode;

                        if (DerivedDataNode.GetOrgDataNode(dataFilterDataNode) is AggregateExpressionDataNode)
                            dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                        if (DerivedDataNode.GetOrgDataNode(dataFilterDataNode).Type == DataNode.DataNodeType.Key)
                            dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                        if (!dataNodes.Contains(dataFilterDataNode))
                            dataNodes.Add(dataFilterDataNode);
                    }
                }



                if (DataFilter != null)
                {
                    foreach (var dataNode in DataFilter.DataNodes)
                    {
                        var dataFilterDataNode = dataNode;
                        while (DerivedDataNode.GetOrgDataNode(dataFilterDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                            dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                        if (DerivedDataNode.GetOrgDataNode(dataFilterDataNode) is AggregateExpressionDataNode)
                            dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                        if (DerivedDataNode.GetOrgDataNode(dataFilterDataNode).Type == DataNode.DataNodeType.Key)
                            dataFilterDataNode = dataFilterDataNode.ParentDataNode;
                        if (!dataNodes.Contains(dataFilterDataNode))
                            dataNodes.Add(dataFilterDataNode);
                    }

                }

                foreach (var dataNode in new List<DataNode>(dataNodes))
                {
                    DataNode parentDataNode = dataNode.ParentDataNode;
                    while (parentDataNode != null &&
                        !dataNodes.Contains(parentDataNode) &&
                        parentDataNode.IsParentDataNode(RootDataNode))
                    {

                        dataNodes.Add(parentDataNode);
                        parentDataNode = parentDataNode.ParentDataNode;
                    }
                }
                if (!dataNodes.Contains(RootDataNode))
                    dataNodes.Add(RootDataNode);

                return (from dataLoaderDataNode in dataNodes
                        where dataLoaderDataNode.Type != DataNode.DataNodeType.Key
                        select dataLoaderDataNode).ToList();
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _CanLoadDataLocal;

        ///<summary>
        ///This property defines when query result can load data locally in distributed query context  
        ///</summary>
        /// <MetaDataID>{bc40d95a-45ca-4300-a167-20cbc9bbd676}</MetaDataID>
        public bool CanLoadDataLocal
        {
            get
            {
                if (!_CanLoadDataLocal.HasValue)
                {
                    if ((RootDataNode).DataSource.DataTable == null)
                        _CanLoadDataLocal = false;
                    else
                    {
                        foreach (DataNode dataNode in DataLoaderDataNodes)
                        {
                            StorageDataLoader dataLoader = (RootDataNode.ObjectQuery as DistributedObjectQuery).GetDataLoader(RootDataNode) as StorageDataLoader;
                            if (dataLoader != null && dataLoader.DataLoaderMetadata.RelatedMemoryCells != null)
                            {
                                foreach (var memoryCellEntry in dataLoader.DataLoaderMetadata.RelatedMemoryCells)
                                {
                                    foreach (var memoryCell in memoryCellEntry.Value)
                                        if (memoryCell.IsPartialLoadedMemoryCell)
                                        {
                                            foreach (DataNode relatedDataNode in DataLoaderDataNodes)
                                            {
                                                if (relatedDataNode.Identity == memoryCellEntry.Key)
                                                {
                                                    _CanLoadDataLocal = false;
                                                    return _CanLoadDataLocal.Value;
                                                }
                                            }
                                        }
                                }
                            }
                            if (dataNode.DataSource.DataLoaders.Count > 0 && ((RootDataNode.ObjectQuery as DistributedObjectQuery).GetDataLoader(RootDataNode) == null ||
                                !((RootDataNode.ObjectQuery as DistributedObjectQuery).GetDataLoader(RootDataNode) as StorageDataLoader).ExistOnlyLocalRoute(dataNode)))
                            {
                                _CanLoadDataLocal = false;
                                return _CanLoadDataLocal.Value;
                            }
                            if (dataNode.ObjectQuery is DistributedObjectQuery)
                            {
                                var aggregateDataNodes = (from aggregateDataNode in dataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                                                          where !dataNode.DataSource.AggregateExpressionDataNodeResolved(aggregateDataNode) && !(dataNode.DataSource.DataLoaders[((dataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectsContext).Identity] as StorageDataLoader).CheckAggregateFunctionForLocalResolve(aggregateDataNode)
                                                          select aggregateDataNode).ToList();
                                if (aggregateDataNodes.Count > 0)
                                {
                                    _CanLoadDataLocal = false; ;
                                    return _CanLoadDataLocal.Value;
                                }
                            }

                        }

                        _CanLoadDataLocal = true;
                    }
                }
                return _CanLoadDataLocal.Value;
            }
        }

        /////<summary>
        /////Defines the header DataNode which uses the dataRetrieve mechanism to load data  
        /////</summary>
        ///// <MetaDataID>{88f3ee6c-d028-43a9-b882-7d8cd1cb2c2c}</MetaDataID>
        //internal DataNode CommonHeaderDataNode
        //{
        //    get
        //    {
        //        DataNode commonHeaderDataNode = RootDataNode;
        //        if (DataFilter != null)
        //            commonHeaderDataNode = RootDataNode.GetCommonHeaderDatanode(DataFilter);

        //        foreach (var member in Members)
        //        {
        //            if (member is SinglePart)
        //                if (!(member as SinglePart).DataNode.IsSameOrParentDataNode(commonHeaderDataNode))
        //                    commonHeaderDataNode = (member as SinglePart).DataNode;
        //            if (member is CompositePart)
        //                while (!(member as CompositePart).Type.CommonHeaderDataNode.IsSameOrParentDataNode(commonHeaderDataNode))
        //                    commonHeaderDataNode = commonHeaderDataNode.ParentDataNode;

        //            if (member is EnumerablePart)
        //                while (!(member as EnumerablePart).Type.CommonHeaderDataNode.IsSameOrParentDataNode(commonHeaderDataNode))
        //                    commonHeaderDataNode = commonHeaderDataNode.ParentDataNode;

        //        }
        //        return commonHeaderDataNode;

        //    }
        //}




        /// <exclude>Excluded</exclude>
        System.Collections.Generic.LinkedList<DataRetrieveNode> _DataRetrievePath;

        /// <summary>
        /// Defines the path on data tree where the system use to retrieve the values of objects of this dynamic type. 
        /// </summary>
        /// <MetaDataID>{544a52a8-13f6-4638-8150-d6b523be23e0}</MetaDataID>
        internal System.Collections.Generic.LinkedList<DataRetrieveNode> DataRetrievePath
        {
            get
            {
                if (_DataRetrievePath == null)
                    BuildDataRetrieveMetadata();
                return _DataRetrievePath;
            }
        }
        /// <exclude>Excluded</exclude>
        DataNode _MembersRootDataNode;

        /// <MetaDataID>{faab0627-30a0-495b-9815-923385bc65ab}</MetaDataID>
        /// <summary>
        /// Defines the common parent DataNode for all Memmbers;
        /// </summary>
        internal DataNode MembersRootDataNode
        {
            get
            {
                if (Members.Count == 0)
                {
                    DataNode rootDataNode = ValueDataNode;
                    while (!rootDataNode.IsDataSource)
                        rootDataNode = rootDataNode.ParentDataNode;
                    return rootDataNode;
                }
                if (_MembersRootDataNode != null)
                    return _MembersRootDataNode;

                DataNode dataNode = null;
                foreach (var member in Members)
                {
                    if (!(member is EnumerablePart))
                    {
                        if (dataNode == null)
                            dataNode = member.SourceDataNode;

                        if (dataNode.IsSameOrParentDataNode(member.SourceDataNode))
                        {
                            DataNode rootDataNode = member.SourceDataNode;
                            while (!dataNode.IsSameOrParentDataNode(rootDataNode) && rootDataNode.ParentDataNode != null)
                                rootDataNode = rootDataNode.ParentDataNode;

                            if (dataNode.IsSameOrParentDataNode(rootDataNode))
                                dataNode = rootDataNode;
                        }

                        while (dataNode != null && DerivedDataNode.GetOrgDataNode(dataNode).Type != DataNode.DataNodeType.Object && DerivedDataNode.GetOrgDataNode(dataNode).Type != DataNode.DataNodeType.Group)
                            dataNode = dataNode.ParentDataNode;
                    }
                }

                _MembersRootDataNode = dataNode;
                if (_MembersRootDataNode == null)
                    _MembersRootDataNode = RootDataNode;

                return _MembersRootDataNode;
            }
        }


        /// <MetaDataID>{8dd8495d-0f12-42c3-9e12-7a264a631449}</MetaDataID>
        /// <summary>
        /// This method creates data retrieve path DataNode rows indices and members values indices
        /// </summary>
        internal void BuildDataRetrieveMetadata()
        {

            if (_DataRetrievePath == null)
            {

                if (Members != null && RootDataNode.Type == DataNode.DataNodeType.Group)
                {
                    foreach (var member in Members)
                    {
                        DataNode groupDataNode = RootDataNode.SubDataNodes.OfType<DerivedDataNode>().Where(deriveDataNode => deriveDataNode.OrgDataNode == (RootDataNode as GroupDataNode).GroupedDataNode).First();
                        if (member.SourceDataNode.IsSameOrParentDataNode(groupDataNode))
                        {
                            if (!(RootDataNode as GroupDataNode).DataGrouping.GroupingCollectionsLoaded)
                                (RootDataNode as GroupDataNode).DataGrouping.LoadGroupingCollections();
                        }
                        if (member.SourceDataNode.Type == DataNode.DataNodeType.Group)
                        {
                            if (!(RootDataNode as GroupDataNode).DataGrouping.GroupingCollectionsLoaded)
                                (RootDataNode as GroupDataNode).DataGrouping.LoadGroupingCollections();
                        }

                    }
                }
                if (IsGroupingType)
                {

                    _DataRetrievePath = (RootDataNode as GroupDataNode).DataGrouping.RetrieveDataPath;
                    DataNodeRowIndices = (RootDataNode as GroupDataNode).DataGrouping.DataNodeRowIndices;

                }
                else
                {

                    SelectionDataNodesAsBranch.Add(RootDataNode);

                    if (RootDataNode.Type == DataNode.DataNodeType.Group && IsGroupingType)
                    {
                        RetrieveSelectionDataNodesAsBranch((RootDataNode as GroupDataNode).GroupedDataNode);

                        foreach (DataNode dataNode in (RootDataNode as GroupDataNode).GroupKeyDataNodes)
                            RetrieveSelectionDataNodesAsBranch(dataNode);
                    }
                    else
                    {
                        if (Members.Count == 0)
                            RetrieveSelectionDataNodesAsBranch(ValueDataNode);
                        foreach (var member in Members)
                        {
                            if (!(member is EnumerablePart))
                                RetrieveSelectionDataNodesAsBranch(member.SourceDataNode);

                            if ((member.SourceDataNode.Type == DataNode.DataNodeType.Key && member.SourceDataNode.ParentDataNode == RootDataNode) ||
                                (member.SourceDataNode.Type == DataNode.DataNodeType.Group && member.SourceDataNode == RootDataNode))
                            {
                                if (member.SourceDataNode.Type == DataNode.DataNodeType.Key)
                                {
                                    foreach (DataNode dataNode in member.SourceDataNode.SubDataNodes)
                                        RetrieveSelectionDataNodesAsBranch(dataNode);
                                }
                                else if (member.SourceDataNode.ParentDataNode != null && (member.SourceDataNode.ParentDataNode is GroupDataNode))
                                {
                                    foreach (DataNode dataNode in (member.SourceDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes)
                                        RetrieveSelectionDataNodesAsBranch(dataNode);
                                }
                            }

                            if (member is EnumerablePart)
                            {
                                if ((member as EnumerablePart).Type.Members.Count > 0)
                                {
                                    DataNode dataNode = (member as EnumerablePart).Type.RootDataNode;
                                    while (dataNode != RootDataNode && dataNode != null && !SelectionDataNodesAsBranch.Contains(dataNode.ParentDataNode))
                                        dataNode = dataNode.ParentDataNode;
                                    if (dataNode != null)
                                    {
                                        //TODO να τσεκαριστουν τα test case που γίνεται αυτό
                                        //if ((member as EnumerablePart).Type.RootDataNode.Type != DataNode.DataNodeType.Group)
                                        //{
                                        //    (member as EnumerablePart).Type.RootDataNode = dataNode;
                                        //    member.SourceDataNode = dataNode;
                                        //}
                                    }
                                }
                                //if (member.SourceDataNode.ParentDataNode != null && member.SourceDataNode != RootDataNode)
                                //    RetrieveSelectionDataNodesAsBranch(member.SourceDataNode.ParentDataNode);
                            }
                        }
                    }


                    DataNode headerDataNode = RootDataNode;

                    #region Gets header data node
                    if (RootDataNode is GroupDataNode)
                    {
                        if (IsGroupingType && (RootDataNode as GroupDataNode).GroupedDataNode.Type == DataNode.DataNodeType.Key)
                        {
                            SelectionDataNodesAsBranch.Remove(RootDataNode);
                            foreach (DataNode groupingDataNode in RootDataNode.SubDataNodes)
                            {
                                if (groupingDataNode.Type != DataNode.DataNodeType.Key)
                                {
                                    headerDataNode = groupingDataNode;
                                    break;
                                }
                            }
                        }
                    }
                    while (headerDataNode.ParentDataNode != null && SelectionDataNodesAsBranch.Contains(headerDataNode.ParentDataNode))
                        headerDataNode = headerDataNode.ParentDataNode;
                    if (headerDataNode == RootDataNode && IsGroupingType)
                    {
                        headerDataNode = (RootDataNode as GroupDataNode).GroupedDataNodeRoot;
                    }
                    if (RootDataNode.Type == DataNode.DataNodeType.Group && IsGroupingType)
                        headerDataNode = (RootDataNode as GroupDataNode).GroupedDataNodeRoot;
                    #endregion

                    _DataRetrievePath = new System.Collections.Generic.LinkedList<DataRetrieveNode>();
                    if (DataFilter != null && !DetailsDataFilterAppliedOnMasterData && !DataFilter.FilterConditionApplied(SelectionDataNodesAsBranch))
                    {
                        var filterHeaderDataNode = RootDataNode.GetCommonHeaderDatanode(DataFilter);
                        if (headerDataNode.IsParentDataNode(filterHeaderDataNode))
                            RetrieveSelectionDataNodesAsBranch(filterHeaderDataNode);
                        //headerDataNode = filterHeaderDataNode;
                    }
                    else
                        DataFilterApplied = true;

                    List<DataRetrieveNode> unAssignedNodes = new List<DataRetrieveNode>();
                    DataRetrieveNode dataRetrieveNode = new DataRetrieveNode(headerDataNode, null, DataNodeRowIndices, _DataRetrievePath);
                    BuildRetrieveDataPath(dataRetrieveNode, DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);

                    if (!IsGroupingType)
                    {
                        foreach (var member in Members)
                        {
                            if (member is CompositePart && !(member as CompositePart).Type.IsGroupingType)
                                (member as CompositePart).Type.BuildRetrieveDataPath(DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);
                            else if (member.SourceDataNode is AggregateExpressionDataNode &&
                                    member.SourceDataNode.ParentDataNode != dataRetrieveNode.DataNode &&
                                    member.SourceDataNode.RealParentDataNode == dataRetrieveNode.DataNode &&
                                    SelectionDataNodesAsBranch.Contains(member.SourceDataNode.ParentDataNode))
                            {
                                BuildRetrieveDataPath(new DataRetrieveNode(member.SourceDataNode.ParentDataNode, DataRetrieveNode.GetDataRetrieveNode(member.SourceDataNode.ParentDataNode.ParentDataNode, _DataRetrievePath), DataNodeRowIndices, _DataRetrievePath), DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);
                            }
                        }
                    }

                    if (OrderByFilter != null)
                        OrderByFilter.BuildRetrieveDataPath(DataRetrieveNode.GetDataRetrieveNode(RootDataNode.GetCommonHeaderDatanode(OrderByFilter), _DataRetrievePath), DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);


                    if (DataFilter != null && !DetailsDataFilterAppliedOnMasterData && !DataFilterApplied)
                        DataFilter.BuildRetrieveDataPath(DataRetrieveNode.GetDataRetrieveNode(RootDataNode.GetCommonHeaderDatanode(DataFilter), _DataRetrievePath), DataNodeRowIndices, _DataRetrievePath, unAssignedNodes, true);
                    else
                        DataFilterApplied = true;


                    int nextColumnIndexInExtensionRow = 0;
                    BuildMembersDataIndices(ref nextColumnIndexInExtensionRow, DataNodeRowIndices, DataRetrievePath);
                    ExtensionRowColumnsCount = nextColumnIndexInExtensionRow;
                }

                if (DataNodeRowIndices.ContainsKey(RootDataNode))
                    RootDataNodeIndices = new int[2] { DataNodeRowIndices[RootDataNode], RootDataNode.DataSource.ObjectIndex };

            }
        }
        /// <summary>
        /// This field defines the index of row in composite row 
        /// which use the dynamic type data retriever to retrieve the conventional type object.
        /// </summary>
        /// <MetaDataID>{4b6e222d-83c5-4ce2-863c-72428a9005ba}</MetaDataID>
        public int ConventionTypeRowIndex = -1;
        /// <summary>
        /// This field defines the index of column in row 
        /// which use the dynamic type data retriever to retrieve the conventional type object.
        /// </summary>
        /// <MetaDataID>{f35a6456-d858-40cf-bace-7e42cc1fa7dc}</MetaDataID>
        public int ConventionTypeColumnIndex = -1;


        /// <MetaDataID>{939502ae-ae83-40ad-8203-574c9489f9e1}</MetaDataID>
        public int[] RootDataNodeIndices;

        /// <summary>
        /// MembersIndices defines a dictionary which keeps the indices on composite row for each member. 
        /// Each member has two indices the first index used from system to access the row on in composite row 
        /// and the second index used from system to access the cell value on row.
        /// </summary>
        /// <MetaDataID>{5659a6df-03b0-4dcb-af6b-9d616fce0169}</MetaDataID>
        internal Dictionary<QueryResultPart, int[]> MembersIndices;


        ///<summary>
        ///This dictionary has the row indices on composite row for each DataNode of query result type  
        ///</summary>
        /// <exclude>Excluded</exclude>
        internal Dictionary<DataNode, int> DataNodeRowIndices = new Dictionary<DataNode, int>();

        /// <MetaDataID>{cd68190e-7f2d-475a-af2d-75eac037023c}</MetaDataID>
        internal int ExtensionRowColumnsCount;

        /// <summary>
        /// Builds the  MembersIndices  dictionary.
        /// The indices used from dynamic type data retriever to find the values of dynamic type members.
        /// </summary>
        /// <param name="nextColumnIndexInExtensionRow">
        /// This parameter defines the next available position in extension row.
        /// Extension row has columns with derived types 
        /// </param>
        /// <param name="dataNodeRowIndices">
        /// This parameter definens  the dictionary which keeps the row index in composite row for each DataNode.
        /// </param>
        /// <MetaDataID>{e2d20196-1370-4f71-b946-63cfddaca8d9}</MetaDataID>
        internal void BuildMembersDataIndices(ref int nextColumnIndexInExtensionRow, Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath)
        {
            if (IsGroupingType)
            {
                if ((RootDataNode as GroupDataNode).DataGrouping == null)
                    DataGrouping.LoadGroupingData(RootDataNode);

                if (!(RootDataNode as GroupDataNode).DataGrouping.GroupingCollectionsLoaded)
                    (RootDataNode as GroupDataNode).DataGrouping.LoadGroupingCollections();

                var name = MetaDataRepository.ObjectQueryLanguage.DataLoader.GetGroupingDataColumnName(RootDataNode);
                Members[0].PartIndices = new int[2] { dataNodeRowIndices[RootDataNode], RootDataNode.DataSource.DataTable.Columns.IndexOf(name) };
                Members[1].PartIndices = new int[2] { dataNodeRowIndices[RootDataNode], RootDataNode.DataSource.DataTable.Columns.IndexOf(name) };

                int nextColumnIndex = 0;

                if (Members[0] is CompositePart)
                    (Members[0] as CompositePart).Type.BuildMembersDataIndices(ref nextColumnIndex, (RootDataNode as GroupDataNode).DataGrouping.DataNodeRowIndices, (RootDataNode as GroupDataNode).DataGrouping.RetrieveDataPath);
                else if (Members[0] is SinglePart)
                {
                    var keyDataNode = (Members[0] as SinglePart).DataNode.SubDataNodes[0];
                    if (DerivedDataNode.GetOrgDataNode(keyDataNode).Type == DataNode.DataNodeType.Object)
                        Members[0].PartIndices = new int[2] { dataNodeRowIndices[keyDataNode], keyDataNode.DataSource.ObjectIndex };


                }

                (Members[1] as EnumerablePart).Type.BuildMembersDataIndices(ref nextColumnIndex, (RootDataNode as GroupDataNode).DataGrouping.DataNodeRowIndices, (RootDataNode as GroupDataNode).DataGrouping.RetrieveDataPath);

                BuildDataRetrieveMetadata();
                return;

            }
            else
            {
                if (ValueDataNode != null)
                {
                    if (ValueDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        ConventionTypeRowIndex = dataNodeRowIndices[ValueDataNode];
                        ConventionTypeColumnIndex = ValueDataNode.DataSource.ObjectIndex;
                    }
                    if (ValueDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (ValueDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                            ValueDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                        {
                            ConventionTypeRowIndex = dataNodeRowIndices[ValueDataNode.ParentDataNode.ParentDataNode];
                            ConventionTypeColumnIndex = ValueDataNode.ParentDataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetColumnName(RootDataNode));
                        }
                        else
                        {
                            ConventionTypeRowIndex = dataNodeRowIndices[ValueDataNode.ParentDataNode];
                            ConventionTypeColumnIndex = ValueDataNode.ParentDataNode.DataSource.GetColumnIndex(ValueDataNode);
                        }
                    }
                    if (ValueDataNode is AggregateExpressionDataNode)
                    {
                        ///TODO  Test Case
                        if (ValueDataNode.ParentDataNode.Type == DataNode.DataNodeType.Group && (ValueDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes.Count == 0)
                        {
                            ConventionTypeRowIndex = dataNodeRowIndices[ValueDataNode.ParentDataNode];
                            ConventionTypeColumnIndex = ValueDataNode.ParentDataNode.DataSource.GetColumnIndex(ValueDataNode);
                        }
                        else if (ValueDataNode.ParentDataNode.Type == DataNode.DataNodeType.Object)
                        {
                            ConventionTypeRowIndex = dataNodeRowIndices[ValueDataNode.ParentDataNode];
                            ConventionTypeColumnIndex = ValueDataNode.ParentDataNode.DataSource.GetColumnIndex(ValueDataNode);

                        }
                    }
                }


                foreach (var member in Members)
                {

                    if (MembersIndices != null && MembersIndices.ContainsKey(member))
                        continue;

                    //In case where the property type is collection or Linq Anonymous Type then system allocate place in extension row. 
                    bool memberValueInExtensionRow = member is CompositePart || member is EnumerablePart;
                    if (MembersIndices == null)
                        MembersIndices = new Dictionary<QueryResultPart, int[]>();

                    DataNode memberSourceDataNode = member.SourceDataNode;
                    if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode).Type == DataNode.DataNodeType.Object)
                    {
                        if (memberValueInExtensionRow)
                        {
                            MembersIndices[member] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                            member.PartIndices = MembersIndices[member];
                            if (member is CompositePart)
                                (member as CompositePart).Type.BuildMembersDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                        }
                        else
                        {
                            MembersIndices[member] = new int[2] { dataNodeRowIndices[memberSourceDataNode], memberSourceDataNode.DataSource.ObjectIndex };
                            member.PartIndices = MembersIndices[member];
                        }
                    }
                    else if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (RootDataNode.Type == DataNode.DataNodeType.Key)
                        //RootDataNode.Type == DataNode.DataNodeType.Group&& (RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(memberSourceDataNode))
                        {
                            //memberSourceDataNode = DerivedDataNode.GetOrgDataNode(memberSourceDataNode);

                            DataNode rowDataNode = RootDataNode.ParentDataNode;
                            int propertyValueIndex = rowDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(memberSourceDataNode));
                            MembersIndices[member] = new int[2] { dataNodeRowIndices[rowDataNode], propertyValueIndex };
                            member.PartIndices = MembersIndices[member];
                        }
                        else
                        {
                            if (memberSourceDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                memberSourceDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                            {
                                DataNode rowDataNode = memberSourceDataNode.ParentDataNode.ParentDataNode;
                                int propertyValueIndex = rowDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(memberSourceDataNode));
                                MembersIndices[member] = new int[2] { dataNodeRowIndices[rowDataNode], propertyValueIndex };
                                member.PartIndices = MembersIndices[member];
                            }
                            else if (memberSourceDataNode.ParentDataNode.Type == DataNode.DataNodeType.Key)
                            {
                                DataNode rowDataNode = memberSourceDataNode.ParentDataNode.ParentDataNode;
                                int propertyValueIndex = rowDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(memberSourceDataNode));
                                MembersIndices[member] = new int[2] { dataNodeRowIndices[rowDataNode], propertyValueIndex };
                                member.PartIndices = MembersIndices[member];
                            }
                            else
                            {
                                DataNode rowDataNode = memberSourceDataNode.ParentDataNode;
                                int propertyValueIndex = rowDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(memberSourceDataNode));
                                MembersIndices[member] = new int[2] { dataNodeRowIndices[rowDataNode], propertyValueIndex };
                                member.PartIndices = MembersIndices[member];
                            }
                        }
                    }
                    else if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode) is AggregateExpressionDataNode)
                    {
                        DataNode rowDataNode = memberSourceDataNode.ParentDataNode;
                        int propertyValueIndex = rowDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(memberSourceDataNode));
                        MembersIndices[member] = new int[2] { dataNodeRowIndices[rowDataNode], propertyValueIndex };//.DataTable.Columns.IndexOf(entry.Value.SourceDataNode.Alias) };
                        member.PartIndices = MembersIndices[member];
                    }
                    else if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode).Type == DataNode.DataNodeType.Key)
                    {

                        //var name = MetaDataRepository.ObjectQueryLanguage.DataLoader.GetGroupingDataColumnName(DerivedDataNode.GetOrgDataNode(memberSourceDataNode).ParentDataNode);
                        //MembersIndices[member] = new int[2] { dataNodeRowIndices[DerivedDataNode.GetOrgDataNode(memberSourceDataNode).ParentDataNode], DerivedDataNode.GetOrgDataNode(memberSourceDataNode).ParentDataNode.DataSource.DataTable.Columns.IndexOf(name) };
                        //member.PartIndices = MembersIndices[member];
                        if (member is CompositePart)
                        {
                            MembersIndices[member] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                            member.PartIndices = MembersIndices[member];

                            (member as CompositePart).Type.BuildMembersDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                        }
                        else
                        {
                            DataNode groupKeyDataNode = memberSourceDataNode.SubDataNodes[0];
                            if (DerivedDataNode.GetOrgDataNode(groupKeyDataNode).Type == DataNode.DataNodeType.Object)
                            {
                                if (memberValueInExtensionRow)
                                {
                                    MembersIndices[member] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                                    member.PartIndices = MembersIndices[member];
                                    if (member is CompositePart)
                                        (member as CompositePart).Type.BuildMembersDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                                }
                                else
                                {
                                    MembersIndices[member] = new int[2] { dataNodeRowIndices[groupKeyDataNode], groupKeyDataNode.DataSource.ObjectIndex };
                                    member.PartIndices = MembersIndices[member];
                                }
                            }
                            else if (DerivedDataNode.GetOrgDataNode(groupKeyDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                            {
                                if (RootDataNode.Type == DataNode.DataNodeType.Group && (RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(groupKeyDataNode))
                                {
                                    MembersIndices[member] = new int[2] { dataNodeRowIndices[RootDataNode], RootDataNode.DataSource.GetColumnIndex(groupKeyDataNode) };
                                    member.PartIndices = MembersIndices[member];
                                }
                                else
                                {
                                    if (groupKeyDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                        groupKeyDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                                    {
                                        DataNode rowDataNode = groupKeyDataNode.ParentDataNode.ParentDataNode;
                                        int propertyValueIndex = rowDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                                        MembersIndices[member] = new int[2] { dataNodeRowIndices[rowDataNode], propertyValueIndex };
                                        member.PartIndices = MembersIndices[member];
                                    }
                                    else if (DerivedDataNode.GetOrgDataNode(groupKeyDataNode).Type == DataNode.DataNodeType.OjectAttribute && groupKeyDataNode.ParentDataNode.Type == DataNode.DataNodeType.Key)
                                    {
                                        DataNode rowDataNode = groupKeyDataNode.ParentDataNode.ParentDataNode;
                                        int propertyValueIndex = rowDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(groupKeyDataNode));
                                        MembersIndices[member] = new int[2] { dataNodeRowIndices[rowDataNode], propertyValueIndex };
                                        member.PartIndices = MembersIndices[member];

                                    }
                                    else
                                    {
                                        DataNode rowDataNode = groupKeyDataNode.ParentDataNode;
                                        int propertyValueIndex = rowDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                                        MembersIndices[member] = new int[2] { dataNodeRowIndices[rowDataNode], propertyValueIndex };
                                        member.PartIndices = MembersIndices[member];
                                    }
                                }
                            }
                        }
                    }
                    else if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode).Type == DataNode.DataNodeType.Group)
                    {
                        MembersIndices[member] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                        member.PartIndices = MembersIndices[member];
                        if (member is CompositePart)
                            (member as CompositePart).Type.BuildMembersDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                        if (member is EnumerablePart && (member as EnumerablePart).Type.IsGroupingType)
                            (member as EnumerablePart).Type.BuildMembersDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                    }
                }
            }
            //if (Type.IsGroupingType != null)
            //{
            //    if (GroupingMetaData.GroupedDataRetrieve.RootDataNode.Type == DataNode.DataNodeType.Object)
            //        GroupingMetaData.GroupedDataRetrieve.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
            //    ConventionTypeRowIndex = retrieveDataPath.Count;
            //    ConventionTypeColumnIndex = nextColumnIndexInExtensionRow++;
            //    GroupingMetaData.GroupingResultRowIndex = ConventionTypeRowIndex;
            //    GroupingMetaData.GroupingResultColumnIndex = ConventionTypeColumnIndex;
            //    GroupingMetaData.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
            //}
        }




        /// <summary>
        /// SelectionDataNodesAsBranch defines all DataNodes which used 
        /// from the system to retrieve the data from the data tree. 
        /// Usually are the root DataNode and all DataNodes between the root DataNode  
        /// and member DataNode or DynamicType properties DataNodes.
        /// </summary>
        /// <MetaDataID>{23a0e408-51f9-4c80-b8ec-189829a981f2}</MetaDataID>
        List<DataNode> SelectionDataNodesAsBranch = new List<DataNode>();

        /// <summary>
        /// This method retrieves all precedents DataNodes in data tree and add them to the SelectionDataNodesAsBranch collection. 
        /// Usually stop on root DataNode that is already added in SelectionDataNodesAsBranch collection 
        /// or on precedent DataNode which is already added in SelectionDataNodesAsBranch collection.
        /// </summary>
        /// <param name="dataNode">
        /// dataNode parameter defines the start point for the backward search.  
        /// </param>
        /// <MetaDataID>{fa3ff3f5-d0ea-43bc-8992-49effba9b8b8}</MetaDataID>
        internal void RetrieveSelectionDataNodesAsBranch(DataNode dataNode)
        {

            if (RootDataNode.Type == DataNode.DataNodeType.Key)
            {
                if (dataNode.Type == DataNode.DataNodeType.Key)
                {
                    if (!SelectionDataNodesAsBranch.Contains(dataNode.ParentDataNode))
                        SelectionDataNodesAsBranch.Add(dataNode.ParentDataNode);
                }

                if ((RootDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes.Contains(DerivedDataNode.GetOrgDataNode(dataNode)) && !IsGroupingType)
                {
                    if (!SelectionDataNodesAsBranch.Contains(dataNode))//DerivedDataNode.GetOrgDataNode(dataNode)))
                        SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));
                    return;
                }
            }
            if (RootDataNode.Type == DataNode.DataNodeType.Group)
            {
                if ((RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(DerivedDataNode.GetOrgDataNode(dataNode)) && !IsGroupingType)
                {
                    if (!SelectionDataNodesAsBranch.Contains(dataNode))//DerivedDataNode.GetOrgDataNode(dataNode)))
                        SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));
                    return;
                }
                foreach (DataNode subDataNode in RootDataNode.SubDataNodes)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Group && (subDataNode as GroupDataNode).GroupKeyDataNodes.Contains(DerivedDataNode.GetOrgDataNode(dataNode)))
                    {
                        if (!SelectionDataNodesAsBranch.Contains(dataNode))
                            SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));
                        RetrieveSelectionDataNodesAsBranch(subDataNode);
                        return;
                    }
                }
                if (DerivedDataNode.GetOrgDataNode(dataNode.ParentDataNode) == (RootDataNode as GroupDataNode).GroupedDataNode)
                {
                    if (!SelectionDataNodesAsBranch.Contains(dataNode))
                        SelectionDataNodesAsBranch.Add(dataNode);
                    if (!SelectionDataNodesAsBranch.Contains(dataNode.ParentDataNode))
                        SelectionDataNodesAsBranch.Add(dataNode.ParentDataNode);
                    return;
                }
            }
            if (!SelectionDataNodesAsBranch.Contains(dataNode))//DerivedDataNode.GetOrgDataNode(dataNode)))
            {

                SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));
                if (SelectionDataNodesAsBranch.Count == 1)
                    return;

                foreach (var selectionDataNode in SelectionDataNodesAsBranch)
                {
                    if (!selectionDataNode.IsSameOrParentDataNode(dataNode))
                    {
                        if (dataNode.ParentDataNode != null)
                            RetrieveSelectionDataNodesAsBranch(dataNode.ParentDataNode);
                        break;
                    }
                    else
                    {
                        DataNode parent = selectionDataNode.ParentDataNode;
                        while (parent != dataNode)
                        {
                            RetrieveSelectionDataNodesAsBranch(parent);
                            parent = parent.ParentDataNode;
                        }
                        break;
                    }
                }
            }
        }




        /// <summary>
        /// This method builds the data node path for data retrieve.
        /// The method called for root data node as start entry point end then called recursively 
        /// for all sub data nodes which participate in SelectionDataNodesAsBranch collection 
        /// </summary>
        /// <param name="dataNode">
        /// This parameter defines the data node which is candidate in RetrieveDataPath
        /// </param>
        /// <param name="dataNodeRowIndices">
        /// The dataNodeRowIndices parameter define dictionary where the method set the 
        /// data node - index key value pair, the value of key value pair is the index of row in composite row 
        /// which keeps the data for data node.  
        /// </param>
        /// <param name="retrieveDataPath">
        /// This parameter defines the already built RetrieveDataPath
        /// </param>
        /// <param name="unAssignedNodes">
        /// This parameter defines the unassinged which will be added at the end of RetrieveDataPath.
        /// </param>
        /// <MetaDataID>{1194dcc2-b27d-4491-aab2-20171b7cdfa3}</MetaDataID>
        void BuildRetrieveDataPath(DataRetrieveNode dataRetrieveNode, Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataRetrieveNode> retrieveDataPath, List<DataRetrieveNode> unAssignedNodes)
        {
            if ((DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Group) &&
              SelectionDataNodesAsBranch.Contains(dataRetrieveNode.DataNode))
            {
                if (!dataNodeRowIndices.ContainsKey(dataRetrieveNode.DataNode))
                {
                    dataNodeRowIndices[dataRetrieveNode.DataNode] = retrieveDataPath.Count;
                    retrieveDataPath.AddLast(dataRetrieveNode);
                }

                #region retrieves all sub data nodes where the Type is DataNode.DataNodeType.Object and participate in selection list as branch
                List<DataRetrieveNode> subNodes = new List<DataRetrieveNode>();

                if (DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Object)
                {
                    foreach (DataNode subDataNode in dataRetrieveNode.DataNode.SubDataNodes)
                    {
                        if (SelectionDataNodesAsBranch.Contains(subDataNode) && DataRetrieveNode.GetDataRetrieveNode(subDataNode, retrieveDataPath) == null &&
                            (DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Group))
                            subNodes.Add(new DataRetrieveNode(subDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));
                    }
                }
                else if (DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Group)
                {
                    foreach (DataNode groupKeyDataNode in (dataRetrieveNode.DataNode as GroupDataNode).GroupKeyDataNodes)
                    {
                        if (SelectionDataNodesAsBranch.Contains(groupKeyDataNode) &&
                            groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                            subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));

                    }


                    foreach (DataNode subDataNode in dataRetrieveNode.DataNode.SubDataNodes)
                    {

                        if (DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Key)
                        {
                            foreach (DataNode groupKeyDataNode in subDataNode.SubDataNodes)
                            {
                                if (SelectionDataNodesAsBranch.Contains(groupKeyDataNode) && DataRetrieveNode.GetDataRetrieveNode(groupKeyDataNode, retrieveDataPath) == null)
                                    subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));
                            }
                        }
                        if (subDataNode is DerivedDataNode
                            && DerivedDataNode.GetOrgDataNode(subDataNode) == (dataRetrieveNode.DataNode as GroupDataNode).GroupedDataNode
                            && SelectionDataNodesAsBranch.Contains(subDataNode))
                        {
                            subNodes.Add(new DataRetrieveNode(subDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));
                            if (!(RootDataNode as GroupDataNode).DataGrouping.GroupingCollectionsLoaded)
                                (RootDataNode as GroupDataNode).DataGrouping.LoadGroupingCollections();

                        }
                    }
                }
                DataRetrieveNode parentDataNode = null;
                if (dataRetrieveNode.DataNode.ParentDataNode != null && SelectionDataNodesAsBranch.Contains(dataRetrieveNode.DataNode.ParentDataNode) && DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode.ParentDataNode, retrieveDataPath) == null)
                    parentDataNode = new DataRetrieveNode(dataRetrieveNode.DataNode.ParentDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath);

                #endregion

                #region Build path for subDataNodes
                if (subNodes.Count > 0)
                {
                    #region Continues recursively with first subdatanodes. The others are market as unassinged and will be added at the end.
                    DataRetrieveNode subDataNode = subNodes[0];
                    subNodes.RemoveAt(0);
                    if (subNodes.Count > 0)
                        unAssignedNodes.AddRange(subNodes);
                    if (parentDataNode != null)
                        unAssignedNodes.Add(parentDataNode);
                    BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, retrieveDataPath, unAssignedNodes);
                    #endregion
                }
                if (parentDataNode != null)
                {
                    BuildRetrieveDataPath(parentDataNode, dataNodeRowIndices, retrieveDataPath, unAssignedNodes);
                }
                else if (unAssignedNodes.Count > 0)
                {
                    #region Continues recursively with first unassigned datanode the others will be added at the end.
                    DataRetrieveNode subDataNode = unAssignedNodes[0];
                    unAssignedNodes.RemoveAt(0);
                    BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, retrieveDataPath, unAssignedNodes);
                    #endregion
                }
                #endregion


            }
            else if ((dataRetrieveNode.DataNode is AggregateExpressionDataNode) &&
              SelectionDataNodesAsBranch.Contains(dataRetrieveNode.DataNode) &&
              RootDataNode.ParentDataNode.Type == DataNode.DataNodeType.Group && (RootDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes.Count == 0)
            {
                //int count = (from order in storage.GetObjectCollection<Order>()
                //                  select order).Count();
                dataRetrieveNode = new DataRetrieveNode(RootDataNode.ParentDataNode, null, dataNodeRowIndices, retrieveDataPath);
                dataNodeRowIndices[dataRetrieveNode.DataNode] = retrieveDataPath.Count;
                retrieveDataPath.AddLast(dataRetrieveNode);
                return;
            }
        }


        /// <summary>
        /// This method builds the data node path for data retrieve.
        /// The method called for root data node as start entry point end then called recursively 
        /// for all sub data nodes which participate in SelectionDataNodesAsBranch collection 
        /// </summary>
        /// <param name="dataNodeRowIndices">
        /// The dataNodeRowIndices parameter define dictionary where the method set the 
        /// data node - index key value pair, the value of key value pair is the index of row in composite row 
        /// which keeps the data for data node.  
        /// </param>
        /// <param name="retrieveDataPath">
        /// This parameter defines the already built RetrieveDataPath
        /// </param>
        /// <param name="unAssignedNodes">
        /// This parameter defines the unassinged which will be added at the end of RetrieveDataPath.
        /// </param>
        /// <MetaDataID>{ceaa10c8-fdd6-465f-92df-7ee7f2f08563}</MetaDataID>
        private void BuildRetrieveDataPath(Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataRetrieveNode> retrieveDataPath, List<DataRetrieveNode> unAssignedNodes)
        {
            RetrieveSelectionDataNodesAsBranch(RootDataNode);
            foreach (var member in Members)
            {
                if (!(member is EnumerablePart))
                    RetrieveSelectionDataNodesAsBranch(member.SourceDataNode);

                if ((member.SourceDataNode.Type == DataNode.DataNodeType.Key && member.SourceDataNode.ParentDataNode == RootDataNode) ||
                    (member.SourceDataNode.Type == DataNode.DataNodeType.Group && member.SourceDataNode == RootDataNode))
                {
                    if (member.SourceDataNode.Type == DataNode.DataNodeType.Key)
                    {
                        foreach (DataNode dataNode in member.SourceDataNode.SubDataNodes)
                            RetrieveSelectionDataNodesAsBranch(dataNode);
                    }
                    else if (member.SourceDataNode.ParentDataNode != null && (member.SourceDataNode.ParentDataNode is GroupDataNode))
                    {
                        foreach (DataNode dataNode in (member.SourceDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes)
                            RetrieveSelectionDataNodesAsBranch(dataNode);
                    }
                }
            }

            DataRetrieveNode dataRetrieveNode = null;
            if (RootDataNode.Type == DataNode.DataNodeType.Key)
                dataRetrieveNode = new DataRetrieveNode(RootDataNode.ParentDataNode, null, dataNodeRowIndices, retrieveDataPath);
            else
                dataRetrieveNode = new DataRetrieveNode(RootDataNode, null, dataNodeRowIndices, retrieveDataPath);

            BuildRetrieveDataPath(dataRetrieveNode, dataNodeRowIndices, retrieveDataPath, unAssignedNodes);



            foreach (var member in Members)
            {
                if (member is CompositePart && !(member as CompositePart).Type.IsGroupingType)
                    (member as CompositePart).Type.BuildRetrieveDataPath(dataNodeRowIndices, retrieveDataPath, unAssignedNodes);
                else
                {
                    if (member.SourceDataNode is AggregateExpressionDataNode &&
                        member.SourceDataNode.ParentDataNode != dataRetrieveNode.DataNode &&
                        member.SourceDataNode.RealParentDataNode == dataRetrieveNode.DataNode &&
                        SelectionDataNodesAsBranch.Contains(member.SourceDataNode.ParentDataNode))
                    {
                        BuildRetrieveDataPath(new DataRetrieveNode(member.SourceDataNode.ParentDataNode, DataRetrieveNode.GetDataRetrieveNode(member.SourceDataNode.ParentDataNode.ParentDataNode, _DataRetrievePath), DataNodeRowIndices, _DataRetrievePath), DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);
                    }
                }
            }

        }

        /// <MetaDataID>{ecf1e19f-a4af-4478-8dea-9219aca8b521}</MetaDataID>
        /// <summary>
        /// Used to retrieve RootDataNode indirect from query context
        /// </summary>
        /// <remarks>
        /// Because QueryResalt type object moved from main query to distributed query and vice versa
        /// Ther RootDataNode retrieved from query context 
        /// </remarks>
        Guid RootDataNodeIdentity;

        // string RootDataNodeName;

        [Association("", Roles.RoleA, "c853c68a-67a0-4195-a9f5-45b1caf3b5a6")]
        [IgnoreErrorCheck]
        public DataNode RootDataNode
        {
            get
            {
                return ObjectQueryContextReference.GetDataNode(RootDataNodeIdentity);
            }
            set
            {
                RootDataNodeIdentity = value.Identity;
                //RootDataNodeName = value.FullName;
            }
        }

        /// <MetaDataID>{50df4ebd-c546-4f2d-ba2b-5c8fc7068e0a}</MetaDataID>
        /// <MetaDataID>{ecf1e19f-a4af-4478-8dea-9219aca8b521}</MetaDataID>
        /// <summary>
        /// Used to retrieve ValueDataNode indirect from query context
        /// </summary>
        /// <remarks>
        /// Because QueryResalt type object moved from main query to distributed query and vice versa
        /// Ther ValueDataNode retrieved from query context 
        /// </remarks>
        Guid ValueDataNodeIdentity;
        /// <summary>
        /// The ValueDataNode defines the data node for native object 
        /// Some times QueryResult refered to the native object type
        /// The only reason of QueryResultType exist is the infos which provides for to retrieves native objects
        /// </summary>
        /// <MetaDataID>{ef7be917-53e2-4eec-aefd-579de82482a6}</MetaDataID>
        public DataNode ValueDataNode
        {
            get
            {
                return ObjectQueryContextReference.GetDataNode(ValueDataNodeIdentity);
            }
            set
            {
                ValueDataNodeIdentity = value.Identity;
            }
        }

        /// <exclude>Excluded</exclude>
        public List<QueryResultPart> _Members = new List<QueryResultPart>();

        /// <summary>
        /// Defines the members of QueryResultType 
        /// </summary>
        [Association("ResultTypeMember", Roles.RoleA, "54e81db7-2834-407e-9240-c71276960501"), RoleAMultiplicityRange(1)]
        public List<QueryResultPart> Members
        {
            get
            {
                return _Members;
            }
        }

        ///<summary>
        ///When QueryResultType is Grouping Type, dictionary (Key,GroupedData) the value of property is true otherwise is false
        ///</summary>
        /// <MetaDataID>{4464f6c9-70af-44fc-95c2-d209044f8d0c}</MetaDataID>
        internal bool IsGroupingType
        {
            get
            {

                if (Members.Count == 2 &&
                    Members[0].Name == "Key" &&
                    !(Members[0] is EnumerablePart) &&
                     Members[1].Name == "GroupedData" &&
                    Members[1] is EnumerablePart)
                {
                    return true;
                }
                else
                    return false;

            }
        }

        ///<summary>
        ///Adds a member to members list
        ///</summary>
        ///<param name="member">
        ///Defines the new member
        ///</param>
        /// <MetaDataID>{b060f5b3-e1be-426a-af31-bad3d243efe9}</MetaDataID>
        public void AddMember(QueryResultPart member)
        {
            if (!_Members.Contains(member))
                _Members.Add(member);
        }


        public void InsertMember(int index, QueryResultPart member)
        {
            _Members.Insert(index, member);
        }
        ///<summary>
        ///Removes a member from member list
        ///</summary>
        ///<param name="member">
        ///Defines the the member which will be removed
        ///</param>
        /// <MetaDataID>{5f7daa0b-db93-4f7f-9bef-55ff3f304765}</MetaDataID>
        public void RemoveMember(QueryResultPart member)
        {
            _Members.Remove(member);
        }



        /// <summary>
        /// Adds distributed query query results to the main query result
        /// </summary>
        /// <param name="distributedQueryResult"></param>
        /// <MetaDataID>{fe3be17d-9aa1-4248-9730-98f7d528c062}</MetaDataID>
        // <MetaDataID>{aa3f6a74-42b5-44fb-8655-fffced83270b}</MetaDataID>
        internal void AddDistributedQueryResult(QueryResultType distributedQueryResult)
        {
            if (DataLoader.CompositeRows == null)
                GetData(distributedQueryResult);
            else
            {
                //sometimes there are data sources of main query results with no data and there are  members with uninitialized indices 
                UpdateMembersIndices(distributedQueryResult);
                DataLoader.Merge(distributedQueryResult.DataLoader);
            }
        }

        ///<summary>
        ///This method get data from distributed query result type and transfer to the main query query result type
        ///</summary>
        ///<param name="distributedQueryResult">
        ///Defines the query result which collected from distributed query
        ///</param>
        /// <MetaDataID>{5509a8e1-4366-4c2f-9782-c66143d7796d}</MetaDataID>
        void GetData(QueryResultType distributedQueryResult)
        {
            distributedQueryResult.ObjectQueryContextReference.ObjectQueryContext = this.ObjectQueryContextReference.ObjectQueryContext;

            RootDataNode = distributedQueryResult.RootDataNode;

            DataLoader = distributedQueryResult.DataLoader;
            ConventionTypeRowIndex = distributedQueryResult.ConventionTypeRowIndex;
            ConventionTypeColumnIndex = distributedQueryResult.ConventionTypeColumnIndex;
            if (distributedQueryResult._DataRetrievePath != null)
                _DataRetrievePath = distributedQueryResult._DataRetrievePath;

            RootDataNodeIndices = distributedQueryResult.RootDataNodeIndices;
            for (int i = 0; i != Members.Count; i++)
            {
                Members[i].PartIndices = distributedQueryResult.Members[i].PartIndices;
                if (Members[i] is CompositePart)
                    (Members[i] as CompositePart).Type.GetData((distributedQueryResult.Members[i] as CompositePart).Type);
                if (Members[i] is EnumerablePart)
                    (Members[i] as EnumerablePart).Type.GetData((distributedQueryResult.Members[i] as EnumerablePart).Type);
            }
        }


        /// <summary>
        /// This set the uninitialized members indices with the members indices of queryResultType parameter 
        /// </summary>
        /// <param name="queryResultType">
        /// Defines the query resut with the new data
        /// </param>
        /// <MetaDataID>{811c9dba-bfd1-43f6-81bc-35bc30df2cbd}</MetaDataID>
        private void UpdateMembersIndices(QueryResultType queryResultType)
        {
            if (ConventionTypeRowIndex == -1 || ConventionTypeColumnIndex == -1)
            {
                ConventionTypeRowIndex = queryResultType.ConventionTypeRowIndex;
                ConventionTypeColumnIndex = queryResultType.ConventionTypeColumnIndex;
            }
            //DataLoader.Type = this;
            for (int i = 0; i != Members.Count; i++)
            {
                if (Members[i].PartIndices[0] == -1 || Members[i].PartIndices[1] == -1)
                    Members[i].PartIndices = queryResultType.Members[i].PartIndices;
                if (Members[i] is CompositePart)
                    (Members[i] as CompositePart).Type.GetData((queryResultType.Members[i] as CompositePart).Type);
                if (Members[i] is EnumerablePart)
                    (Members[i] as EnumerablePart).Type.GetData((queryResultType.Members[i] as EnumerablePart).Type);
            }

        }





        /// <exclude>Excluded</exclude>
        Dictionary<string, QueryResultPart> _NamedMembers;

        ///<summary>
        ///This method returns the member by name
        ///</summary>
        ///<param name="memberName">
        ///This paremeter defines the member name 
        ///</param>
        ///<returns>
        ///Return the member with the member name
        ///</returns>
        /// <MetaDataID>{602074f9-077c-4e1f-832b-d6be8f71fcdb}</MetaDataID>
        internal QueryResultPart GetMember(string memberName)
        {
            if (_NamedMembers == null)
            {
                _NamedMembers = new Dictionary<string, QueryResultPart>();
                foreach (var member in Members)
                {
                    if (member is EnumerablePart)
                        _NamedMembers[(member as EnumerablePart).SourceDataNode.Name] = member;

                    _NamedMembers[member.Name] = member;
                }
            }
            QueryResultPart memberRet = null;
            _NamedMembers.TryGetValue(memberName, out memberRet);
            return memberRet;
        }

        /// <summary>
        /// This property is true where 
        /// </summary>
        /// <MetaDataID>{4dcece09-43d8-4c7d-8cab-f3fdd1685ffc}</MetaDataID>

        internal bool DetailsDataFilterAppliedOnMasterData
        {
            get
            {
                bool dataFilterAppliedOnTypeHierarchy = false;
                foreach (var enumerablePart in MembersWhereParticipate.OfType<EnumerablePart>())
                {
                    if (enumerablePart.OwnerType.DataFilter != DataFilter)
                        return false;
                    else
                        dataFilterAppliedOnTypeHierarchy = true;
                }
                return dataFilterAppliedOnTypeHierarchy;
            }
        }
    }

    ///<summary>
    ///This class connect query result types with the ObjectQuery Context
    ///</summary>
    /// <MetaDataID>{84ee4e76-0e57-4cec-8f2b-f52df7698c9c}</MetaDataID>
    [Serializable]
    public class QueryResultObjectContextReference
    {
        /// <MetaDataID>{e4e0572b-2170-4220-bed9-4486c4f9fe3e}</MetaDataID>
        public readonly Guid Identity = Guid.NewGuid();
        /// <MetaDataID>{7b01db93-80aa-43eb-82c9-bae290418de8}</MetaDataID>
        public QueryResultObjectContextReference()
        {

        }

        /// <MetaDataID>{1ea6ee84-1661-4899-a449-cf9a8160d326}</MetaDataID>
        QueryResultObjectContextReference(Guid identity)
        {
            Identity = identity;
        }

        /// <exclude>Excluded</exclude>
        [NonSerialized]
        ObjectQuery _ObjectQueryContext;

        /// <summary>
        /// Defines the context where will be resolved  the query result
        /// </summary>
        /// <MetaDataID>{3829896e-ca67-4819-a9b2-c38f2a3943fc}</MetaDataID>
        public ObjectQuery ObjectQueryContext
        {
            get
            {
                return _ObjectQueryContext;
            }
            internal set
            {
                _ObjectQueryContext = value;

            }
        }

        /// <summary>
        /// This method parse query reslut and return query result of identity 
        /// </summary>
        /// <param name="identity">
        /// Defines the identity of query result 
        /// </param>
        /// <returns>
        /// the query result with identity the identity of parameter
        /// </returns>
        /// <MetaDataID>{d100c9d4-1ebd-479d-86bf-5e37450ac8d3}</MetaDataID>
        internal QueryResultType GetType(Guid identity)
        {
            return GetType(_ObjectQueryContext.QueryResultType, identity);
        }

        /// <MetaDataID>{95a60b11-e2d1-4c72-ab44-2d04b3d77a4b}</MetaDataID>
        QueryResultType GetType(QueryResultType queryResult, Guid identity)
        {
            if (identity == queryResult.Identity)
                return queryResult;
            else
                foreach (var member in queryResult.Members)
                {
                    if (member is CompositePart && (member as CompositePart).Type.Identity == identity)
                        return (member as CompositePart).Type;

                    if (member is EnumerablePart && (member as EnumerablePart).Type.Identity == identity)
                        return (member as EnumerablePart).Type;

                    if (member is CompositePart)
                    {
                        QueryResultType type = GetType((member as CompositePart).Type, identity);
                        if (type != null)
                            return type;
                    }

                    if (member is EnumerablePart)
                    {
                        QueryResultType type = GetType((member as EnumerablePart).Type, identity);
                        if (type != null)
                            return type;
                    }
                }

            return null;

        }
        /// <summary>
        /// Look for data node on ObjectQuery DataNode trees 
        /// </summary>
        /// <param name="dataNodeIdentity">
        /// defines the identity of data node we want
        /// </param>
        /// <returns>
        /// Return the DataNode with identity the identity of parameter 
        /// </returns>
        /// <MetaDataID>{33cdb392-7d7b-4c9b-b703-794349184e33}</MetaDataID>
        internal DataNode GetDataNode(Guid dataNodeIdentity)
        {
            foreach (var rootDataNode in this.ObjectQueryContext.DataTrees)
            {
                DataNode dataNode = rootDataNode.GetDataNode(dataNodeIdentity);
                if (dataNode != null)
                    return dataNode;
            }
            return null;
        }

        /// <MetaDataID>{f03a1d5b-9c5d-4fc1-9fd0-1709ec2da83a}</MetaDataID>
        internal QueryResultObjectContextReference Clone(Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as QueryResultObjectContextReference;
            else
            {
                clonedObjects[this] = new QueryResultObjectContextReference(Identity);
                return clonedObjects[this] as QueryResultObjectContextReference;
            }


        }
    }
}
