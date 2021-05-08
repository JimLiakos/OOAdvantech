using System.Collections.Generic;
using System;
using System.Linq;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{dba97d23-5b30-42ed-816a-57684a50d714}</MetaDataID>
    [Serializable]
    public class QueryResult
    {

        public void GetData()
        {
            DataLoader.LoadData();
        }

        [Association("", Roles.RoleA, "6d966e6e-bf2a-47fa-9e27-2f1cee4539a2")]
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


        [Association("", Roles.RoleA, "2a43d120-58b6-43b2-ad7e-ee4f5fb5fe34")]
        [IgnoreErrorCheck]
        public SearchCondition DataFilter;

        internal bool DataFilterApplied;

        /// <summary>
        /// Check the filter condition.
        /// If the filterCondition resolved from native dataBase system returns true otherwise returns false
        /// </summary>
        /// <param name="filterCondition">
        /// Defines the considered filterCondition
        /// </param>
        /// <param name="dataRetrievePath">
        /// Defines the query result data retrieve path
        /// </param>
        /// <returns>
        /// Returns true when filterCondition resolved from native dataBase system otherwise returns false
        /// </returns>
        private bool FilterConditionApplied(SearchCondition filterCondition, LinkedList<DataRetrieveNode> dataRetrievePath)
        {
            int numOfDataNodes = 0;
            bool hasCriterionsToApply = false;
            foreach (Criterion criterion in filterCondition.Criterions)
            {
                if (!filterCondition.GlobalCriterions.Contains(criterion) &&
                    !criterion.Applied)
                {
                    hasCriterionsToApply = true;
                    break;
                }
            }
            if (hasCriterionsToApply)
            {

                foreach (var criterion in filterCondition.Criterions)
                    criterion.Applied = false;
                return false;
            }
            foreach (var dataNode in filterCondition.DataNodes)
            {
                DataNode orgDataNode = DerivedDataNode.GetOrgDataNode(dataNode);
                while (orgDataNode.Type != DataNode.DataNodeType.Object && orgDataNode.Type != DataNode.DataNodeType.Group)
                    orgDataNode = orgDataNode.ParentDataNode;

                var exist = (from pathDataRetrieveNode in dataRetrievePath.ToArray()
                             where pathDataRetrieveNode.DataNode == orgDataNode
                             select pathDataRetrieveNode).Count() != 0;
                if (exist)
                    numOfDataNodes++;
            }
            if (numOfDataNodes > 1)
            {
                foreach (var criterion in filterCondition.Criterions)
                    criterion.Applied = false;

                return false;
            }
            return true;
        }

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
        internal bool DoesRowPassCondition(System.Data.DataRow[] compositeRow, LinkedListNode<DataRetrieveNode> linkedListNode)
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


        internal Guid Identity;

        public QueryResultRootReference RootTypeReference;
        /// <MetaDataID>{f569f116-3057-4e08-98d9-92162c8d80b6}</MetaDataID>
        public QueryResult(DataNode rootDataNode, QueryResultRootReference rootTypeReference)
        {
            Identity = Guid.NewGuid();
            RootTypeReference = rootTypeReference;
            RootDataNode = rootDataNode;
            DataLoader = new QueryResultDataLoader(this, rootTypeReference);

        }
        public QueryResult(DataNode rootDataNode)
        {
            Identity = Guid.NewGuid();
            RootTypeReference = new QueryResultRootReference(rootDataNode);
            RootTypeReference.Root = this;
            RootDataNode = rootDataNode;
            DataLoader = new QueryResultDataLoader(this, RootTypeReference);

        }

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
                    if (member.SourceDataNode is AggregateExpressionDataNode && (member.SourceDataNode as AggregateExpressionDataNode).SourceSearchCondition!=null)
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
                if (RootDataNode is GroupDataNode )
                {
                    if (!RootDataNode.DataSource.GroupedDataLoaded)
                    {
                        if (!dataNodes.Contains((RootDataNode as GroupDataNode).GroupedDataNode))
                            dataNodes.Add((RootDataNode as GroupDataNode).GroupedDataNode);
                    }
                    if (!RootDataNode.DataSource.GroupedDataLoaded&&(RootDataNode as GroupDataNode).GroupingSourceSearchCondition != null)
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
                        if (!dataNodes.Contains(dataNode))
                            dataNodes.Add(dataNode);
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
                return dataNodes;

            }
        }

        bool? _CanLoadDataLocal;

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
                            if ((CommonHeaderDataNode.ObjectQuery as DistributedObjectQuery).GetDataLoader(CommonHeaderDataNode) == null ||
                                !((CommonHeaderDataNode.ObjectQuery as DistributedObjectQuery).GetDataLoader(CommonHeaderDataNode) as StorageDataLoader).ExistOnlyLocalRoute(dataNode))
                            {
                                _CanLoadDataLocal = false; ;
                                return _CanLoadDataLocal.Value;
                            }
                            if (dataNode.ObjectQuery is DistributedObjectQuery)
                            {
                                var aggregateDataNodes = (from aggregateDataNode in dataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                                                          where !dataNode.DataSource.AggregateExpressionDataNodeResolved(aggregateDataNode) && !(dataNode.DataSource.DataLoaders[((dataNode.ObjectQuery as DistributedObjectQuery).ObjectStorage as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity] as StorageDataLoader).CheckAggregateFunctionForLocalResolve(aggregateDataNode)
                                                          select aggregateDataNode).ToList();
                                if (aggregateDataNodes.Count > 0)
                                {
                                    _CanLoadDataLocal = false; ;
                                    return _CanLoadDataLocal.Value;
                                }
                            }
                            
                        }

                        foreach (var member in Members.OfType<EnumerablePart>())
                            if (!member.Type.CanLoadDataLocal)
                            {
                                _CanLoadDataLocal = false;
                                return _CanLoadDataLocal.Value;
                            }
                        _CanLoadDataLocal = true;
                    }
                }
                return _CanLoadDataLocal.Value;
            }
        }

        /// <MetaDataID>{88f3ee6c-d028-43a9-b882-7d8cd1cb2c2c}</MetaDataID>
        internal DataNode CommonHeaderDataNode
        {
            get
            {
                DataNode commonHeaderDataNode = RootDataNode;
                if (DataFilter != null)
                    commonHeaderDataNode = RootDataNode.GetCommonHeaderDatanode(DataFilter);

                foreach (var member in Members)
                {
                    if (member is SinglePart)
                        if (!(member as SinglePart).DataNode.IsSameOrParentDataNode(commonHeaderDataNode))
                            commonHeaderDataNode = (member as SinglePart).DataNode;
                    if (member is CompositePart)
                        while (!(member as CompositePart).Type.CommonHeaderDataNode.IsSameOrParentDataNode(commonHeaderDataNode))
                        {
                            commonHeaderDataNode = commonHeaderDataNode.ParentDataNode;
                        }

                    if (member is EnumerablePart)
                        while (!(member as EnumerablePart).Type.CommonHeaderDataNode.IsSameOrParentDataNode(commonHeaderDataNode))
                        {

                            commonHeaderDataNode = commonHeaderDataNode.ParentDataNode;
                        }

                }
                return commonHeaderDataNode;

            }
        }




        /// <exclude>Excluded</exclude>

        System.Collections.Generic.LinkedList<DataRetrieveNode> _DataRetrievePath;

        /// <summary>
        /// Defines the path on Data tree where the system use to retrieve the values of objects of this dynamic type. 
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
        internal DataNode MembersRootDataNode
        {
            get
            {
                if (Members.Count == 0)
                    return RootDataNode;

                if (_MembersRootDataNode != null)
                    return _MembersRootDataNode;

                DataNode dataNode = null;
                foreach (var member in Members)
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

                    while (dataNode != null && dataNode.Type != DataNode.DataNodeType.Object && dataNode.Type != DataNode.DataNodeType.Group)
                        dataNode = dataNode.ParentDataNode;

                }
                _MembersRootDataNode = dataNode;
                return dataNode;
            }
        }


        /// <MetaDataID>{8dd8495d-0f12-42c3-9e12-7a264a631449}</MetaDataID>
        internal void BuildDataRetrieveMetadata()
        {


            if (_DataRetrievePath == null)
            {
                if (IsGroupingType)
                {

                    _DataRetrievePath = (RootDataNode as GroupDataNode).DataGrouping.RetrieveDataPath;
                    DataNodeRowIndices = (RootDataNode as GroupDataNode).DataGrouping.DataNodeRowIndices;

                }
                else
                {
                    //if (Members.Count == 0)
                    //    RetrieveSelectionDataNodesAsBranch(RootDataNode);
                    //else
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
                                if (member.SourceDataNode.ParentDataNode != null && member.SourceDataNode != RootDataNode)
                                    RetrieveSelectionDataNodesAsBranch(member.SourceDataNode.ParentDataNode);
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
                    if (DataFilter != null && !FilterConditionApplied(DataFilter, _DataRetrievePath))
                        DataFilter.BuildRetrieveDataPath(new DataRetrieveNode(RootDataNode.GetCommonHeaderDatanode(DataFilter), null, DataNodeRowIndices, _DataRetrievePath), DataNodeRowIndices, _DataRetrievePath, unAssignedNodes, true);
                    else
                        DataFilterApplied = true;

                    int nextColumnIndexInExtensionRow = 0;
                    BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, DataNodeRowIndices, DataRetrievePath);
                    ExtensionRowColumnsCount = nextColumnIndexInExtensionRow;
                }

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

        /// <summary>
        /// MembersIndices defines a dictionary which keeps the indices on composite row for each member. 
        /// Each member has two indices the first index used from system to access the row on in composite row 
        /// and the second index used from system to access the cell value on row.
        /// </summary>
        /// <MetaDataID>{5659a6df-03b0-4dcb-af6b-9d616fce0169}</MetaDataID>
        internal Dictionary<QueryResultPart, int[]> MembersIndices;


        /// <exclude>Excluded</exclude>
        internal Dictionary<DataNode, int> DataNodeRowIndices = new Dictionary<DataNode, int>();

        internal int ExtensionRowColumnsCount;

        /// <summary>
        /// Builds the  PropertiesIndices  dictionary.
        /// The indices used from dynamic type data retriever to find the values of dynamic type properties.
        /// </summary>
        /// <param name="nextColumnIndexInExtensionRow">
        /// This parameter defines the next available position in extension row.
        /// Extension row has columns with derived types 
        /// </param>
        /// <param name="dataNodeRowIndices">
        /// This parameter definens  the dictionary which keeps the row index in composite row for each DataNode.
        /// </param>
        /// <MetaDataID>{e2d20196-1370-4f71-b946-63cfddaca8d9}</MetaDataID>
        internal void BuildPropertiesDataIndices(ref int nextColumnIndexInExtensionRow, Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath)
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
                (Members[1] as EnumerablePart).Type.BuildPropertiesDataIndices(ref nextColumnIndex, (RootDataNode as GroupDataNode).DataGrouping.DataNodeRowIndices, (RootDataNode as GroupDataNode).DataGrouping.RetrieveDataPath);

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
                    if (RootDataNode.Type == DataNode.DataNodeType.OjectAttribute)
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
                            ConventionTypeColumnIndex = ValueDataNode.ParentDataNode.DataSource.GetColumnIndex(RootDataNode);
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

                    DataNode memberSourceDataNode = DerivedDataNode.GetOrgDataNode(member.SourceDataNode);
                    if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode).Type == DataNode.DataNodeType.Object)
                    {
                        if (memberValueInExtensionRow)
                        {
                            MembersIndices[member] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                            member.PartIndices = MembersIndices[member];
                            if (member is CompositePart)
                                (member as CompositePart).Type.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                        }
                        else
                        {
                            MembersIndices[member] = new int[2] { dataNodeRowIndices[memberSourceDataNode], memberSourceDataNode.DataSource.ObjectIndex };
                            member.PartIndices = MembersIndices[member];
                        }
                    }
                    else if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (RootDataNode.Type == DataNode.DataNodeType.Group && (RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(memberSourceDataNode))
                        {
                            if (memberSourceDataNode is DerivedDataNode)
                                memberSourceDataNode = (memberSourceDataNode as DerivedDataNode).OrgDataNode;

                            MembersIndices[member] = new int[2] { dataNodeRowIndices[RootDataNode], RootDataNode.DataSource.GetColumnIndex(memberSourceDataNode) };
                            member.PartIndices = MembersIndices[member];
                        }
                        else
                        {
                            string columnPrefix = "";
                            //Class member
                            if (memberSourceDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                memberSourceDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                            {
                                int propertyValueIndex = memberSourceDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(memberSourceDataNode));
                                MembersIndices[member] = new int[2] { dataNodeRowIndices[memberSourceDataNode.ParentDataNode.ParentDataNode], propertyValueIndex };
                                member.PartIndices = MembersIndices[member];
                            }
                            else if (memberSourceDataNode.ParentDataNode.Type == DataNode.DataNodeType.Key)
                            {

                                int propertyValueIndex = memberSourceDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(memberSourceDataNode));
                                MembersIndices[member] = new int[2] { dataNodeRowIndices[memberSourceDataNode.ParentDataNode.ParentDataNode], propertyValueIndex };
                                member.PartIndices = MembersIndices[member];
                            }
                            else
                            {
                                int propertyValueIndex = memberSourceDataNode.ParentDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(memberSourceDataNode));
                                MembersIndices[member] = new int[2] { dataNodeRowIndices[memberSourceDataNode.ParentDataNode], propertyValueIndex };
                                member.PartIndices = MembersIndices[member];
                            }
                        }
                    } 
                    else if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode) is AggregateExpressionDataNode)
                    {
                        MembersIndices[member] = new int[2] { dataNodeRowIndices[memberSourceDataNode.ParentDataNode], memberSourceDataNode.ParentDataNode.DataSource.GetColumnIndex(memberSourceDataNode) };//.DataTable.Columns.IndexOf(entry.Value.SourceDataNode.Alias) };
                        member.PartIndices = MembersIndices[member];
                    } 
                    else if (DerivedDataNode.GetOrgDataNode(memberSourceDataNode).Type == DataNode.DataNodeType.Key)
                    {

                        if (member is CompositePart)
                        {
                            MembersIndices[member] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                            member.PartIndices = MembersIndices[member];

                            (member as CompositePart).Type.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                        }
                        else
                        {
                            DataNode groupKeyDataNode = (memberSourceDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes[0];
                            if (groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                            {
                                if (memberValueInExtensionRow)
                                {
                                    MembersIndices[member] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                                    member.PartIndices = MembersIndices[member];
                                    if (member is CompositePart)
                                        (member as CompositePart).Type.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                                }
                                else
                                {
                                    MembersIndices[member] = new int[2] { dataNodeRowIndices[groupKeyDataNode], groupKeyDataNode.DataSource.ObjectIndex };
                                    member.PartIndices = MembersIndices[member];
                                }
                            }
                            else if (groupKeyDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            {
                                if (RootDataNode.Type == DataNode.DataNodeType.Group && (RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(groupKeyDataNode))
                                {
                                    MembersIndices[member] = new int[2] { dataNodeRowIndices[RootDataNode], RootDataNode.DataSource.GetColumnIndex(groupKeyDataNode) };
                                    member.PartIndices = MembersIndices[member];
                                }
                                else
                                {
                                    string columnPrefix = "";
                                    //Class member
                                    if (groupKeyDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                        groupKeyDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                                    {
                                        int propertyValueIndex = groupKeyDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                                        MembersIndices[member] = new int[2] { dataNodeRowIndices[groupKeyDataNode.ParentDataNode.ParentDataNode], propertyValueIndex };
                                        member.PartIndices = MembersIndices[member];
                                    }
                                    else
                                    {
                                        int propertyValueIndex = groupKeyDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                                        MembersIndices[member] = new int[2] { dataNodeRowIndices[groupKeyDataNode.ParentDataNode], propertyValueIndex };
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
                            (member as CompositePart).Type.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                        if (member is EnumerablePart && (member as EnumerablePart).Type.IsGroupingType)
                            (member as EnumerablePart).Type.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
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
                        SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));
                        RetrieveSelectionDataNodesAsBranch(subDataNode);
                        return;
                    }
                }
            }
            if (!SelectionDataNodesAsBranch.Contains(dataNode))//DerivedDataNode.GetOrgDataNode(dataNode)))
            {
                SelectionDataNodesAsBranch.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));

                foreach (var selectionDataNode in SelectionDataNodesAsBranch)
                {
                    if (!selectionDataNode.IsSameOrParentDataNode(dataNode))
                    {
                        if (dataNode.ParentDataNode != null)
                            RetrieveSelectionDataNodesAsBranch(dataNode.ParentDataNode);
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
                    dataNodeRowIndices[DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode)] = retrieveDataPath.Count;
                    retrieveDataPath.AddLast(dataRetrieveNode);
                }

                #region retrieves all sub data nodes where the Type is DataNode.DataNodeType.Object and participate in selection list as branch
                List<DataRetrieveNode> subNodes = new List<DataRetrieveNode>();

                if (DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Object)
                {
                    foreach (DataNode subDataNode in dataRetrieveNode.DataNode.SubDataNodes)
                    {
                        if (SelectionDataNodesAsBranch.Contains(subDataNode) &&
                            DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Group)
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
                                if (SelectionDataNodesAsBranch.Contains(groupKeyDataNode))
                                    subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));
                            }
                        }
                    }
                }

                #endregion

                #region Build path for subDataNodes
                if (subNodes.Count > 0)
                {
                    #region Continues recursively with first subdatanodes. The others are market as unassinged and will be added at the end.
                    DataRetrieveNode subDataNode = subNodes[0];
                    subNodes.RemoveAt(0);
                    if (subNodes.Count > 0)
                        unAssignedNodes.AddRange(subNodes);
                    BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, retrieveDataPath, unAssignedNodes);
                    #endregion
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


        /// <MetaDataID>{ceaa10c8-fdd6-465f-92df-7ee7f2f08563}</MetaDataID>
        private void BuildRetrieveDataPath(Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataRetrieveNode> retrieveDataPath, List<DataRetrieveNode> unAssignedNodes)
        {
            RetrieveSelectionDataNodesAsBranch(RootDataNode);
            foreach (var member in Members)
                RetrieveSelectionDataNodesAsBranch(member.SourceDataNode);

            DataRetrieveNode dataRetrieveNode = new DataRetrieveNode(RootDataNode, null, dataNodeRowIndices, retrieveDataPath);
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

        Guid RootDataNodeIdentity;

        [Association("", Roles.RoleA, "c853c68a-67a0-4195-a9f5-45b1caf3b5a6")]
        [IgnoreErrorCheck]
        public DataNode RootDataNode
        {
            get
            {
                return RootTypeReference.RootDataNode.HeaderDataNode.GetDataNode(RootDataNodeIdentity);
            }
            set
            {
                RootDataNodeIdentity = value.Identity;
            }
        }

        Guid ValueDataNodeIdentity;
        public DataNode ValueDataNode
        {
            get
            {
                return RootTypeReference.RootDataNode.HeaderDataNode.GetDataNode(ValueDataNodeIdentity);
            }
            set
            {
                ValueDataNodeIdentity = value.Identity;
            }
        }
        [Association("SourceData", Roles.RoleA, "4e655a66-daf9-424a-88b2-8ac713c7be78")]
        public QueryResult Source;
        /// <exclude>Excluded</exclude>
        public List<QueryResultPart> _Members = new List<QueryResultPart>();

        [Association("ResultTypeMember", Roles.RoleA, "54e81db7-2834-407e-9240-c71276960501"), RoleAMultiplicityRange(1)]
        public List<QueryResultPart> Members
        {
            get
            {
                return _Members;
            }
        }

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

        /// <MetaDataID>{b060f5b3-e1be-426a-af31-bad3d243efe9}</MetaDataID>
        public void AddMember(QueryResultPart queryResultPart)
        {
            _Members.Add(queryResultPart);
        }

        /// <MetaDataID>{5f7daa0b-db93-4f7f-9bef-55ff3f304765}</MetaDataID>
        public void RemoveMember(QueryResultPart queryResultPart)
        {
            _Members.Remove(queryResultPart);
        }

        internal void Update(QueryResult queryResultType)
        {
            RootDataNode = queryResultType.RootDataNode;
            _DataRetrievePath = queryResultType._DataRetrievePath;
            DataLoader = queryResultType.DataLoader;
            ConventionTypeRowIndex = queryResultType.ConventionTypeRowIndex;
            ConventionTypeColumnIndex = queryResultType.ConventionTypeColumnIndex;
            //DataLoader.Type = this;
            for (int i = 0; i != Members.Count; i++)
            {
                Members[i].PartIndices = queryResultType.Members[i].PartIndices;
                Members[i].SourceDataNode = queryResultType.Members[i].SourceDataNode;
                if (Members[i] is CompositePart)
                    (Members[i] as CompositePart).Type.Update((queryResultType.Members[i] as CompositePart).Type);
                if (Members[i] is EnumerablePart)
                    (Members[i] as EnumerablePart).Type.Update((queryResultType.Members[i] as EnumerablePart).Type);
            }

        }

        internal void AdddistributedQueryResult(QueryResult distributedQueryResult)
        {
            if (DataLoader.CompositeRows == null)
                Update(distributedQueryResult);
            else
            {
                //sometimes there are data sources of main query results with no data and there are  members with uninitialized indices 
                UpdateMembersIndices(distributedQueryResult);
                DataLoader.Merge(distributedQueryResult.DataLoader);
            }
        }

        /// <summary>
        /// This set the uninitialized members indices with the members indices of queryResultType parameter 
        /// </summary>
        /// <param name="queryResultType">
        /// Defines the query resut with the new data
        /// </param>
        private void UpdateMembersIndices(QueryResult queryResultType)
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
                    (Members[i] as CompositePart).Type.Update((queryResultType.Members[i] as CompositePart).Type);
                if (Members[i] is EnumerablePart)
                    (Members[i] as EnumerablePart).Type.Update((queryResultType.Members[i] as EnumerablePart).Type);
            }

        }





        Dictionary<string, QueryResultPart> _NamedMembers;
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
    }

    /// <MetaDataID>{84ee4e76-0e57-4cec-8f2b-f52df7698c9c}</MetaDataID>
    [Serializable]
    public class QueryResultRootReference
    {

        DataNode _RootDataNode;
        public QueryResultRootReference(DataNode rootDataNode)
        {
            _RootDataNode = rootDataNode;
        }
        public DataNode RootDataNode
        {
            get
            {
                if (Root != null && _Root.RootTypeReference == this)
                    return _RootDataNode;
                else
                    return Root.RootDataNode;
            }
        }
        QueryResult _Root;
        public QueryResult Root
        {
            get
            {
                return _Root;
            }
            set
            {

                _Root = value;
                if (_Root.RootTypeReference != this)
                    _RootDataNode = null;

            }

        }

        internal QueryResult GetType(Guid identity)
        {
            return GetType(_Root, identity);
        }

        QueryResult GetType(QueryResult queryResult, Guid identity)
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
                        QueryResult type = GetType((member as CompositePart).Type, identity);
                        if (type != null)
                            return type;
                    }

                    if (member is EnumerablePart)
                    {
                        QueryResult type = GetType((member as EnumerablePart).Type, identity);
                        if (type != null)
                            return type;
                    }
                }

            return null;

        }
    }
}
