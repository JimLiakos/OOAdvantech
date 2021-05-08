using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{

    /// <summary>
    /// GroupDataNode is a DataNode which has extra functionality to resolve the group by expression
    /// </summary>
    /// <MetaDataID>{b8ca46f7-b0c1-482b-8e4f-c9161e65aed2}</MetaDataID>
    [Serializable]
    public class GroupDataNode : DataNode
    {

        internal override bool IsDataSource
        {
            get
            {
                return true;
            }
        }

        internal override bool IsDataSourceMember
        {
            get
            {
                return false;
            }
        }


        /// <exclude>Excluded</exclude>
        internal DataGrouping _DataGrouping;
        [Association("", Roles.RoleA, "ba7a1e3c-f965-4500-a1f8-2d0af6678d08")]
        [IgnoreErrorCheck]
        internal DataGrouping DataGrouping
        {
            get
            {
                return _DataGrouping;
            }
            set
            {
                _DataGrouping = value;
            }
        }

        /// <exclude>Excluded</exclude>
        public DataNode _GroupedDataNode;
        /// <MetaDataID>{6509db80-2b4b-4993-92d3-a697223a5d86}</MetaDataID>
        /// <summary>
        /// The GroupedDataNode is the datanode which retreives the data for grouping.   
        /// </summary>
        [Association("", Roles.RoleA, "90682aca-d60b-4731-89e6-30a5418f86f6")]
        [IgnoreErrorCheck]
        public DataNode GroupedDataNode
        {
            get
            {
                return _GroupedDataNode;
            }
            set
            {
                _GroupedDataNode = value;
            }
        }



        [Association("", Roles.RoleA, "2d09aec6-ae63-49e2-bb3d-3fd24e108136")]
        [IgnoreErrorCheck]
        public System.Collections.Generic.List<DataNode> GroupKeyDataNodes = new System.Collections.Generic.List<DataNode>();


        /// <MetaDataID>{af8c4ff8-24a5-4789-8920-6b07dd291601}</MetaDataID>
        public override void AddSearchCondition(SearchCondition searchCondition)
        {
            base.AddSearchCondition(searchCondition);
        }

        /// <MetaDataID>{786d0588-8ba3-44cd-a0b7-07c0a8235f7b}</MetaDataID>
        public GroupDataNode(ObjectQuery objectQuery)
            : base(objectQuery)
        {
        }
        protected void Copy(GroupDataNode newDataNode, Dictionary<object, object> clonedObjects)
        {
            base.Copy(newDataNode, clonedObjects);

            newDataNode._GroupedDataNode = _GroupedDataNode.Clone(clonedObjects);
            if (_GroupingResultSearchCondition.Value != null)
                newDataNode._GroupingResultSearchCondition.Value = _GroupingResultSearchCondition.Value.Clone(clonedObjects);
            if (_GroupingSourceSearchCondition.Value != null)
                newDataNode._GroupingSourceSearchCondition.Value = _GroupingSourceSearchCondition.Value.Clone(clonedObjects);
            foreach (DataNode keyDataNode in GroupKeyDataNodes)
                newDataNode.GroupKeyDataNodes.Add(keyDataNode.Clone(clonedObjects));

        }
        protected GroupDataNode(Guid identity) : base(identity)
        {

        }

        internal override DataNode Clone(Dictionary<object, object> clonedObjects)
        {

            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as DataNode;

            var newDataNode = new GroupDataNode(Identity);
            clonedObjects[this] = newDataNode;
            Copy(newDataNode, clonedObjects);
            return newDataNode;

        }
        /// <MetaDataID>{02424f02-a940-4c47-98c7-5441aab15b2e}</MetaDataID>
        public GroupDataNode(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.ObjectQuery objectQuery, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Path path)
            : base(objectQuery, path)
        {

        }
        #region Group DataNode code


        /// <MetaDataID>{2a922cfc-bec7-4748-9fe6-b3ca9bb586d8}</MetaDataID>
        public override List<SearchCondition> BranchSearchConditions
        {
            get
            {
                return SearchConditions;
            }
        }
        /// <summary>
        /// Defines a constrain condition wich derived from all search conditions of data node 
        ///  </summary>
        /// <MetaDataID>{69eadac8-bd21-47e8-ae34-f8a4ca580efb}</MetaDataID>
        public override SearchCondition SearchCondition
        {
            get
            {
                if (_SearchCondition.UnInitialized)
                {
                    var branchSearchConditions = BranchSearchConditions;

                    if (branchSearchConditions.Count > 0)
                    {
                        if (FilterNotActAsLoadConstraint)
                        {
                            _SearchCondition.Value = null;
                            return null;
                        }

                        if (branchSearchConditions.Count == 1)
                        {
                            if (branchSearchConditions[0] == null)
                                return branchSearchConditions[0];
                            if (branchSearchConditions[0] != GroupingSourceSearchCondition)
                                return branchSearchConditions[0];
                        }

                        foreach (var searchCondition in branchSearchConditions)
                        {
                            if (searchCondition == GroupingSourceSearchCondition &&
                                searchCondition != null)
                                continue;

                            if (searchCondition == null)
                            {
                                _SearchCondition.Value = null;
                                return null;
                            }
                            bool allContainsAsPart = true;
                            foreach (var innerSearchCondition in branchSearchConditions)
                            {
                                if (innerSearchCondition == null)
                                {
                                    _SearchCondition.Value = null;
                                    return null;
                                }
                                if (searchCondition.ToString() == innerSearchCondition.ToString())
                                    continue;
                                if (innerSearchCondition != null && !innerSearchCondition.ContainsSearchCondition(searchCondition))
                                {
                                    allContainsAsPart = false;
                                    break;
                                }
                            }
                            if (allContainsAsPart)
                            {
                                _SearchCondition.Value = searchCondition;
                                return searchCondition;
                            }
                        }

                    }
                    else if (RealParentDataNode != null)
                    {
                        _SearchCondition.Value = RealParentDataNode.SearchCondition;
                        return RealParentDataNode.SearchCondition;
                    }
                    _SearchCondition.Value = null;
                    return null;
                }
                else
                    return _SearchCondition.Value;
            }
        }

        ///// <summary>
        ///// Traverse data and apply search condition on them. If not passed the rows marked to as removed rows
        ///// </summary>
        ///// <MetaDataID>{f9eb6b66-bcb4-4adf-954f-f0fa02569c6f}</MetaDataID>
        //internal override void FilterData()
        //{
        //    foreach (var searchCondition in SearchConditions)
        //    {
        //        if (searchCondition != null)
        //        {
        //            DataNode commonHeaderDataNode = GetCommonHeaderDatanode(GroupingResultSearchCondition);
        //            GroupingResultSearchCondition.FilterData(commonHeaderDataNode);
        //        }        
        //    }
        //}
        /// <summary>
        /// Retrievs all storage where query pump objects
        /// </summary>
        /// <param name="objectQueryStorages">
        /// Defines dictionary collection with object context matada distribution manager
        /// </param>
        /// <MetaDataID>{7ecbfb99-976f-4aa2-9ad3-6a5a3f53a4f8}</MetaDataID>
        internal override void GetObjectsContexts(OOAdvantech.Collections.Generic.Dictionary<string, ObjectsContextMetadataDistributionManager> queryObjectsContexts)
        {
            if (_DataSource == null && Type == DataNodeType.Group)
            {
                Collections.Generic.Dictionary<string, DataLoaderMetadata> dataLoadersMetadata = new Collections.Generic.Dictionary<string, DataLoaderMetadata>();
                foreach (System.Collections.Generic.KeyValuePair<string, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> entry in (GroupedDataNode.DataSource as StorageDataSource).DataLoadersMetadata)
                {
                    DataLoaderMetadata dataLoaderMetadata = new DataLoaderMetadata(HeaderDataNode.GetDataNode(entry.Value.DataNodeIdentity), entry.Value.QueryStorageID, entry.Value.ObjectsContextIdentity, entry.Value.StorageName, entry.Value.StorageLocation, entry.Value.StorageType);
                    dataLoadersMetadata.Add(entry.Key, dataLoaderMetadata);
                    foreach (StorageCell storageCell in entry.Value.StorageCells)
                        dataLoaderMetadata.AddStorageCell(storageCell);
                }
                _DataSource = new StorageDataSource(this, dataLoadersMetadata);
            }
            if (DataSource is StorageDataSource)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata> entry in (DataSource as StorageDataSource).DataLoadersMetadata)
                {
                    string storageName = null;
                    string storageLocation = null;
                    string storageType = null;
                    string storageIdentity2 = null;
                    if (entry.Value.StorageCells.Count > 0)
                    {
                        entry.Value.StorageCells[0].GetStorageConnectionData(out storageIdentity2, out storageName, out storageLocation, out storageType);
                        ///TODO Ο κώδικας γράφτηκε γιατί έχει προβλημα όταν η storage είναι πάνω σε memory stream   
                        PersistenceLayer.ObjectStorage objectStorage = null;
                        if (entry.Value.StorageCells[0].StorageIdentity == (ObjectQuery as ObjectsContextQuery).ObjectsContext.Identity)
                            objectStorage = (ObjectQuery as ObjectsContextQuery).ObjectStorage;
                        else
                            objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageName, storageLocation, storageType);

                        if (!queryObjectsContexts.ContainsKey(entry.Value.ObjectsContextIdentity))
                        {
                            //Collections.Generic.Dictionary<Guid, DataLoaderMetadata> dataSoureceStorageCells = new OOAdvantech.Collections.Generic.Dictionary<Guid, DataLoaderMetadata>();
                            //dataSoureceStorageCells.Add(DataSource.Identity, entry.Value);
                            ObjectsContextMetadataDistributionManager objectsContextMetadataDistributionManager = new ObjectsContextMetadataDistributionManager(entry.Value.ObjectsContextIdentity, objectStorage);
                            queryObjectsContexts.Add(objectsContextMetadataDistributionManager.ObjectsContextIdentity, objectsContextMetadataDistributionManager);
                            queryObjectsContexts[entry.Value.ObjectsContextIdentity].DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                        }
                        else
                            queryObjectsContexts[entry.Value.ObjectsContextIdentity].DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                    }
                    else
                    {
                        PersistenceLayer.ObjectStorage objectStorage = null;
                        if (entry.Value.ObjectsContextIdentity == (ObjectQuery as ObjectsContextQuery).ObjectsContext.Identity)
                            objectStorage = (ObjectQuery as ObjectsContextQuery).ObjectStorage;
                        else
                            objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(entry.Value.StorageName, entry.Value.StorageLocation, entry.Value.StorageType);
                        if (!queryObjectsContexts.ContainsKey(entry.Value.ObjectsContextIdentity))
                        {
                            ObjectsContextMetadataDistributionManager objectsContextMetadataDistributionManager = new ObjectsContextMetadataDistributionManager(entry.Value.ObjectsContextIdentity, objectStorage);
                            queryObjectsContexts.Add(objectsContextMetadataDistributionManager.ObjectsContextIdentity, objectsContextMetadataDistributionManager);
                            objectsContextMetadataDistributionManager.DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                        }
                        else
                            queryObjectsContexts[entry.Value.ObjectsContextIdentity].DataLoadersMetadata.Add(DataSource.Identity, entry.Value);
                    }
                }
            }
            foreach (DataNode dataNode in SubDataNodes)
            {

                dataNode.GetObjectsContexts(queryObjectsContexts);
            }
        }

        ///<summary>
        ///Traverse data tree and collect data source and load them in dataSources dictionary
        ///</summary>
        ///<param name="dataSources">
        ///Defines a Dat
        internal override void GetDataSources(ref System.Collections.Generic.Dictionary<Guid, DataSource> dataSources)
        {
            if (!InDataSources)
            {
                InDataSources = true;
                try
                {
                    if (dataSources == null)
                        dataSources = new System.Collections.Generic.Dictionary<Guid, DataSource>();
                    if (_DataSource != null)
                        dataSources[_DataSource.Identity] = _DataSource;
                    GroupedDataNodeRoot.GetDataSources(ref dataSources);
                }
                finally
                {
                    InDataSources = false;
                }
            }
        }

        ///<summary>
        ///This method add DataNode to Group Key DataNodes collection
        ///</summary>
        /// <MetaDataID>{d54cafa2-2ef2-40ce-965a-0754be40464b}</MetaDataID>
        public void AddGroupKeyDataNode(DataNode dataNode)
        {
            dataNode.ParticipateInGroopByAsKey = true;
            GroupKeyDataNodes.Add(dataNode);
        }




        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6ebfb9b9-11fb-49c3-887c-2e7866c79c37}</MetaDataID>
        Member<SearchCondition> _GroupingResultSearchCondition = new Member<SearchCondition>();

        ///<summary>
        ///Defines a search condition which applied on grouping result
        ///</summary>
        /// <MetaDataID>{1dd51b6c-ef06-462f-97d8-1b87c9000c9d}</MetaDataID>
        public virtual SearchCondition GroupingResultSearchCondition
        {
            get
            {
                if (_GroupingResultSearchCondition.UnInitialized)
                {
                    _GroupingResultSearchCondition.Value = SearchCondition;

                    // temporary excluded
                    //_GroupingResultSearchCondition.Value = GetGroupingResultSearchCondition(SearchCondition);
                }

                return _GroupingResultSearchCondition;
            }
        }

        ///<summary>
        ///Extract grouping results searchCondition from searchCondition parameter   
        ///</summary>
        /// <MetaDataID>{b72d2583-e364-4c28-918a-0064ba26b4c3}</MetaDataID>
        private SearchCondition GetGroupingResultSearchCondition(SearchCondition searchCondition)
        {
            if (searchCondition == null)
                return null;
            bool groupDataSourceNodeSearchCondition = true;
            foreach (DataNode dataNode in searchCondition.DataNodes)
            {
                if (!dataNode.IsParentDataNode(this))
                {
                    groupDataSourceNodeSearchCondition = false;
                    break;
                }
            }

            if (!groupDataSourceNodeSearchCondition && searchCondition.SearchTerms.Count == 1 && searchCondition.SearchTerms[0].SearchFactors.Count > 1)
            {
                List<SearchFactor> searchFactors = new List<SearchFactor>();
                foreach (SearchFactor searchFactor in searchCondition.SearchTerms[0].SearchFactors)
                {
                    if (searchFactor.SearchCondition != null)
                    {
                        var partialSearchCondition = GetGroupingResultSearchCondition(searchFactor.SearchCondition);
                        if (partialSearchCondition != null)
                            searchFactors.Add(new SearchFactor(partialSearchCondition));
                    }
                    else
                    {
                        SearchTerm searchTerm = new SearchTerm(new List<SearchFactor>() { searchFactor });
                        var partialSearchCondition = GetGroupingResultSearchCondition(new SearchCondition(new List<SearchTerm>() { searchTerm }, ObjectQuery));
                        if (partialSearchCondition != null)
                            searchFactors.Add(searchFactor);
                    }
                }
                if (searchFactors.Count > 0)
                {
                    SearchTerm searchTerm = new SearchTerm(searchFactors);
                    var newSearchCondition = new SearchCondition(new List<SearchTerm>() { searchTerm }, ObjectQuery);
                    return newSearchCondition;

                }
            }


            if (groupDataSourceNodeSearchCondition)
                return searchCondition;
            else
                return null;

        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{c682a2d7-26e6-4c74-9cc7-be5061c687d7}</MetaDataID>
        Member<SearchCondition> _GroupingSourceSearchCondition = new Member<SearchCondition>();

        ///<summary>
        ///Defines search condition which used from query engine to filter source data of group by expression
        ///</summary>
        /// <MetaDataID>{13af120c-0999-4855-944a-cde2e88ced1b}</MetaDataID>
        public SearchCondition GroupingSourceSearchCondition
        {
            get
            {
                if (_GroupingSourceSearchCondition.UnInitialized)
                {
                    _GroupingSourceSearchCondition.Value = null;
                    //_GroupingSourceSearchCondition.Value = GetGroupingSourceSearchCondition(SearchCondition);
                    //if (_GroupingSourceSearchCondition.Value != null &&
                    //    GroupedDataNode.SearchCondition != null &&
                    //    GroupedDataNode.SearchCondition == _GroupingSourceSearchCondition.Value)
                    //    _GroupingSourceSearchCondition.Value = GroupedDataNode.SearchCondition;
                }

                return _GroupingSourceSearchCondition.Value;
            }

            set
            {
                if (GroupedDataNode.HasGroupResultCriterions(value))
                    value = GroupedDataNode.RemoveGroupResultCriterions(value);

                _GroupingSourceSearchCondition.Value = value;
                //foreach (DataNode keyDataNode in GroupKeyDataNodes)
                //    keyDataNode.AddSearchCondition(value);

                GroupedDataNode.AddSearchCondition(value);

            }
        }


        /// <MetaDataID>{dcca76dc-ce2f-489b-a891-3cd671577a72}</MetaDataID>
        private SearchCondition GetGroupingSourceSearchCondition(SearchCondition searchCondition)
        {
            if (searchCondition == null)
                return null;
            bool groupDataNodeSearchCondition = true;
            foreach (DataNode dataNode in searchCondition.DataNodes)
            {
                if (dataNode.IsParentDataNode(this))
                {
                    groupDataNodeSearchCondition = false;
                    break;
                }
            }

            if (!groupDataNodeSearchCondition && searchCondition.SearchTerms.Count == 1 && searchCondition.SearchTerms[0].SearchFactors.Count > 1)
            {
                List<SearchFactor> searchFactors = new List<SearchFactor>();
                foreach (SearchFactor searchFactor in searchCondition.SearchTerms[0].SearchFactors)
                {
                    if (searchFactor.SearchCondition != null)
                    {
                        var partialSearchCondition = GetGroupingSourceSearchCondition(searchFactor.SearchCondition);
                        if (partialSearchCondition != null)
                            searchFactors.Add(new SearchFactor(partialSearchCondition));
                    }
                    else
                    {
                        SearchTerm searchTerm = new SearchTerm(new List<SearchFactor>() { searchFactor });
                        var partialSearchCondition = GetGroupingSourceSearchCondition(new SearchCondition(new List<SearchTerm>() { searchTerm }, ObjectQuery));
                        if (partialSearchCondition != null)
                            searchFactors.Add(searchFactor);
                    }
                }
                if (searchFactors.Count > 0)
                {
                    SearchTerm searchTerm = new SearchTerm(searchFactors);
                    var newSearchCondition = new SearchCondition(new List<SearchTerm>() { searchTerm }, ObjectQuery);
                    return newSearchCondition;

                }
            }

            if (groupDataNodeSearchCondition)
                return searchCondition;
            else
                return null;
        }


        #region Group Data code removed
        //public class GroupingDataNode
        //{
        //    public GroupingDataNode(DataNode dataNode, GroupingDataNode parentGroupingDataNode)
        //    {
        //        DataNode = dataNode;
        //        ParentGroupingDataNode = parentGroupingDataNode;
        //    }
        //    public GroupingDataNode(DataNode dataNode, System.Collections.Generic.LinkedList<GroupingDataNode> groupingPath)
        //    {
        //        DataNode = dataNode;
        //        if (dataNode.RealParentDataNode != null)
        //        {
        //            GroupingDataNode[] groupingDataNodes = new GroupingDataNode[groupingPath.Count];
        //            groupingPath.CopyTo(groupingDataNodes, 0);
        //            foreach (GroupingDataNode groupingDataNode in groupingDataNodes)
        //            {
        //                if (groupingDataNode.DataNode == dataNode.RealParentDataNode)
        //                {
        //                    ParentGroupingDataNode = groupingDataNode;
        //                    break;
        //                }
        //            }
        //            if (ParentGroupingDataNode == null)
        //                ParentGroupingDataNode = new GroupingDataNode(dataNode.RealParentDataNode, groupingPath);
        //        }
        //    }


        //    public readonly DataNode DataNode;
        //    public readonly GroupingDataNode ParentGroupingDataNode;

        //}







        ///// <MetaDataID>{4a53696d-bc23-489b-a13c-52a5b95eaa96}</MetaDataID>
        //private void CreateGroupPath(GroupingDataNode groupingDataNode, System.Collections.Generic.LinkedList<GroupingDataNode> groupingDataPath, System.Collections.Generic.List<DataNode> unAssignedNodes)
        //{
        //    if (groupingDataNode.DataNode.Type == DataNode.DataNodeType.Namespace)
        //    {
        //        CreateGroupPath(new GroupingDataNode(groupingDataNode.DataNode.SubDataNodes[0], groupingDataPath), groupingDataPath, unAssignedNodes);
        //        return;
        //    }

        //    System.Collections.Generic.List<GroupingDataNode> subNodes = new System.Collections.Generic.List<GroupingDataNode>();
        //    if ((groupingDataNode.DataNode.Type == DataNode.DataNodeType.Object || groupingDataNode.DataNode.Type == DataNode.DataNodeType.Group || groupingDataNode.DataNode.Type == DataNode.DataNodeType.OjectAttribute) &&
        //        groupingDataNode.DataNode.BranchParticipateInAggregateFanction)
        //    {
        //        if (groupingDataNode.DataNode.Type == DataNode.DataNodeType.Object || groupingDataNode.DataNode.Type == DataNode.DataNodeType.Group)
        //        {
        //            groupingDataNode.DataNode.GroupedDataRowIndex = groupingDataPath.Count;
        //            groupingDataPath.AddLast(groupingDataNode);
        //        }
        //        if (groupingDataNode.DataNode.Type == DataNode.DataNodeType.OjectAttribute)
        //            groupingDataNode.DataNode.GroupedDataRowIndex = groupingDataNode.DataNode.RealParentDataNode.GroupedDataRowIndex;

        //        #region retrieves all sub data nodes which participate in search condition as branch
        //        if (groupingDataNode.DataNode.Type == DataNode.DataNodeType.Group)
        //        {
        //            foreach (DataNode subDataNode in groupingDataNode.DataNode.GroupKeyDataNodes)
        //            {
        //                if (subDataNode.BranchParticipateInAggregateFanction &&
        //                    (subDataNode.Type == DataNode.DataNodeType.Object || subDataNode.Type == DataNode.DataNodeType.OjectAttribute))
        //                    subNodes.Add(new GroupingDataNode(subDataNode, groupingDataPath.Last.Value));
        //            }
        //        }
        //        else
        //        {
        //            foreach (DataNode subDataNode in groupingDataNode.DataNode.SubDataNodes)
        //            {
        //                if (subDataNode.BranchParticipateInAggregateFanction &&
        //                    (subDataNode.Type == DataNode.DataNodeType.Object || subDataNode.Type == DataNode.DataNodeType.OjectAttribute))
        //                    subNodes.Add(new GroupingDataNode(subDataNode, groupingDataPath));
        //            }
        //        }
        //        #endregion

        //        if (subNodes.Count > 0)
        //        {
        //            #region Continues recursively with first subdatanodes the others are market as unassinged and will be added at the end.
        //            GroupingDataNode subDataNode = subNodes[0];
        //            subNodes.RemoveAt(0);
        //            if (subNodes.Count > 0)
        //            {
        //                foreach (GroupingDataNode subNode in subNodes)
        //                    unAssignedNodes.Add(subNode.DataNode);

        //            }
        //            CreateGroupPath(subDataNode, groupingDataPath, unAssignedNodes);
        //            #endregion
        //        }
        //        else if (unAssignedNodes.Count > 0)
        //        {
        //            #region Continues recursively with first unassigned datanode the others will be added at the end.
        //            GroupingDataNode subDataNode = new GroupingDataNode(unAssignedNodes[0], groupingDataPath);
        //            unAssignedNodes.RemoveAt(0);
        //            CreateGroupPath(subDataNode, groupingDataPath, unAssignedNodes);
        //            #endregion
        //        }
        //    }
        //}


        ///// <MetaDataID>{250aba75-649e-4aed-845c-5a0f93be1bce}</MetaDataID>
        //private void CreateAggregationDataPath(DataNode dataNode, System.Collections.Generic.LinkedList<GroupingDataNode> groupingDataPath, System.Collections.Generic.List<DataNode> unAssignedNodes)
        //{
        //    if (dataNode.Type == DataNode.DataNodeType.Namespace)
        //    {
        //        CreateGroupPath(new GroupingDataNode(dataNode.SubDataNodes[0], groupingDataPath), groupingDataPath, unAssignedNodes);
        //        return;
        //    }

        //    System.Collections.Generic.List<DataNode> subNodes = new System.Collections.Generic.List<DataNode>();
        //    if ((dataNode.Type == DataNode.DataNodeType.Object || dataNode.Type == DataNode.DataNodeType.OjectAttribute) &&
        //        dataNode.BranchParticipateInAggregateFanction)
        //    {


        //        if (dataNode.Type == DataNode.DataNodeType.Object)
        //        {
        //            dataNode.GroupedDataRowIndex = groupingDataPath.Count;
        //            groupingDataPath.AddLast(new GroupingDataNode(dataNode, groupingDataPath));
        //        }
        //        if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
        //        {
        //            dataNode.GroupedDataRowIndex = dataNode.RealParentDataNode.GroupedDataRowIndex;
        //            //dataNode.DataSourceColumnIndex = dataNode.RealParentDataNode.DataSource.GetColunmIndex(dataNode);// DataTable.Columns.IndexOf(dataNode.Name);
        //        }
        //        else
        //            //if (dataNode.ParticipateInSelectClause)
        //            //    dataNode.DataSourceColumnIndex = dataNode.DataSource.ObjectIndex;//dataNode.DataSource.DataTable.Columns.IndexOf(dataNode.DataSource.ObjectIndex);
        //            //else
        //            //    dataNode.DataSourceColumnIndex = dataNode.DataSource.DataTable.Columns.IndexOf("ObjectID");//mark



        //            #region retrieves all sub data nodes which participate in search condition as branch

        //            foreach (DataNode subDataNode in dataNode.SubDataNodes)
        //            {
        //                if (subDataNode.BranchParticipateInAggregateFanction &&
        //                    (subDataNode.Type == DataNode.DataNodeType.Object || subDataNode.Type == DataNode.DataNodeType.OjectAttribute))
        //                    subNodes.Add(subDataNode);
        //            }
        //            #endregion

        //        if (subNodes.Count > 0)
        //        {
        //            #region Continues recursively with first subdatanodes the others are market as unassinged and will be added at the end.
        //            DataNode subDataNode = subNodes[0] as DataNode;
        //            subNodes.RemoveAt(0);
        //            if (subNodes.Count > 0)
        //                unAssignedNodes.AddRange(subNodes);
        //            CreateGroupPath(new GroupingDataNode(subDataNode, groupingDataPath), groupingDataPath, unAssignedNodes);
        //            #endregion
        //        }
        //        else if (unAssignedNodes.Count > 0)
        //        {
        //            #region Continues recursively with first unassigned datanode the others will be added at the end.
        //            DataNode subDataNode = unAssignedNodes[0];
        //            unAssignedNodes.RemoveAt(0);
        //            CreateGroupPath(new GroupingDataNode(subDataNode, groupingDataPath), groupingDataPath, unAssignedNodes);
        //            #endregion
        //        }
        //    }
        //}
        #endregion


        /// <MetaDataID>{908c4d40-d7db-46d0-bddb-f843d6c9439e}</MetaDataID>
        /// <summary>GroupedDataNodeRoot is the first data node between the grouping data node and group by data node in data tree hierarchy </summary>
        public DataNode GroupedDataNodeRoot
        {
            get
            {
                DataNode commonAncestor = GroupedDataNode;
                foreach (var dataNode in GroupKeyDataNodes)
                {
                    while (!dataNode.IsSameOrParentDataNode(commonAncestor))
                    {
                        if (commonAncestor.Type == DataNodeType.Group)
                        {
                            GroupDataNode groupingDataNode = commonAncestor as GroupDataNode;
                            while (groupingDataNode.Type == DataNodeType.Group &&
                                !dataNode.IsSameOrParentDataNode(groupingDataNode.GroupedDataNodeRoot) &&
                                (groupingDataNode as GroupDataNode).GroupedDataNodeRoot.Type == DataNodeType.Group)
                            {
                                groupingDataNode = groupingDataNode.GroupedDataNodeRoot as GroupDataNode;
                            }
                            if (dataNode.IsSameOrParentDataNode(groupingDataNode.GroupedDataNodeRoot))
                                break;
                        }



                        commonAncestor = commonAncestor.ParentDataNode;
                    }
                }
                if (RealParentDataNode != null)
                {
                    foreach (var subdataNode in RealParentDataNode.RealSubDataNodes)
                    {
                        if (commonAncestor.IsParentDataNode(subdataNode))
                        {
                            commonAncestor = subdataNode;
                            break;
                        }
                    }
                }

                if (commonAncestor.IsParentDataNode(this))
                {
                    while (commonAncestor.ParentDataNode != this)
                        commonAncestor = commonAncestor.RealParentDataNode;
                }
                return commonAncestor;// SubDataNodes[0];
            }
        }


        /// <MetaDataID>{fd72c2de-36c7-4250-a2f4-08723ea99626}</MetaDataID>
        //public System.Collections.Generic.List<DataNode> GroupKeyDataNodes = new System.Collections.Generic.List<DataNode>();


        /// <MetaDataID>{88b5f8bb-dba8-4d99-afde-4330216ac672}</MetaDataID>
        public System.Collections.Generic.List<DataNode> GroupingDataNodes
        {
            get
            {
                System.Collections.Generic.List<DataNode> groupingDataNodes = new System.Collections.Generic.List<DataNode>();
                if (Type != DataNodeType.Group)
                    return groupingDataNodes;
                foreach (DataNode dataNode in SubDataNodes)
                {
                    if (dataNode.Type == DataNodeType.Object)
                    {
                        dataNode.ParticipateInGroopByAsGrouped = true;
                        groupingDataNodes.Add(dataNode);
                    }
                    if (dataNode.Type == DataNodeType.Group)
                    {
                        dataNode.ParticipateInGroopByAsGrouped = true;
                        groupingDataNodes.Add(dataNode);
                        groupingDataNodes.AddRange((dataNode as GroupDataNode).GroupingDataNodes);
                    }
                }
                return groupingDataNodes;
            }
        }




        /// <summary>
        /// This method participates in route building mechanism.
        /// Building route mechanism walk on data tree through BuildRoute until find the routeEndDataNode 
        /// then unwind function call and build the DataNode route.
        /// </summary>
        /// <param name="routeEndDataNode">
        /// Defines the last DataNode of built route 
        /// </param>
        /// <param name="passThroughDataNodes">
        /// Defines a collection that keeps all Data Nodes where mechanism passes from them to find routeEndDataNode.
        /// </param>
        /// <param name="route">
        /// Defines the built route
        /// </param>
        /// <MetaDataID>{5af64969-f8eb-482a-867b-1d6bc3005b61}</MetaDataID>
        public virtual bool BuildRoute(DataNode routeEndDataNode, System.Collections.Generic.List<DataNode> passThroughDataNodes, System.Collections.Generic.Stack<DataNode> route)
        {
            if (routeEndDataNode == this)
            {
                route.Push(routeEndDataNode);
                return true;
            }
            else
            {

                foreach (DataNode subDataNode in GroupKeyDataNodes)
                {
                    if (passThroughDataNodes.Contains(subDataNode))
                        continue;
                    passThroughDataNodes.Add(this);
                    if (subDataNode.BuildRoute(routeEndDataNode, passThroughDataNodes, route))
                    {
                        route.Push(this);
                        return true;
                    }
                }

                foreach (DataNode subDataNode in SubDataNodes)
                {
                    if (passThroughDataNodes.Contains(subDataNode))
                        continue;
                    passThroughDataNodes.Add(this);
                    if (subDataNode.BuildRoute(routeEndDataNode, passThroughDataNodes, route))
                    {
                        route.Push(this);
                        return true;
                    }
                }

            }

            if (ParentDataNode != null)
            {
                if (ParentDataNode == routeEndDataNode)
                {
                    route.Push(ParentDataNode);
                    route.Push(this);
                    return true;
                }
                passThroughDataNodes.Add(this);
                if (passThroughDataNodes.Contains(ParentDataNode))
                    return false;
                if (ParentDataNode.BuildRoute(routeEndDataNode, passThroughDataNodes, route))
                {
                    route.Push(this);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;

        }

        #endregion



        /// <MetaDataID>{a24b004a-1355-440d-a346-8154182ce846}</MetaDataID>
        public DataNode KeyDataNode
        {
            get
            {
                foreach (var dataNode in SubDataNodes)
                {
                    if (dataNode.Type == DataNodeType.Key)
                        return dataNode;
                }
                return null;
            }

        }
    }



}
