using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <summary>
    /// The role of DataGrouping class is to resolve group by and aggregation expressions
    /// </summary>
    /// <MetaDataID>{4bb1c1d4-da44-4612-bd1d-5fb8afa1fdde}</MetaDataID>
    [Serializable]
    class DataGrouping
    { 

        /// <summary>
        /// Load DataSource with the result of group by expression and aggregation expression
        /// </summary>
        /// <param name="dataNode"></param>
        /// <MetaDataID>{9db4b8e9-4aa7-4555-806a-d1683b25038b}</MetaDataID>
        public static void LoadGroupingData(DataNode dataNode)
        {
            DataGrouping dataGrouping = new DataGrouping(dataNode);
            dataGrouping.LoadGroupingData();
            if (dataNode is GroupDataNode)
                (dataNode as GroupDataNode).DataGrouping = dataGrouping;

        }
        /// <summary>
        /// Data node has grouping result data to load 
        /// </summary>
        /// <param name="dataNode">
        /// Defines the data node which checked if it is Grouping DataNode or has AggreegationExpressionDataNode as SubDataNode. 
        /// </param>
        /// <returns>
        /// Returns true when dataNode parameter is Grouping DataNode or has AggreegationExpressionDataNode as SubDataNode. 
        /// </returns>
        /// <MetaDataID>{c2b9af9f-8e6c-438e-ab26-c612957387f0}</MetaDataID>
        public static bool HasGroupingDataResultSubDataNodes(DataNode dataNode)
        {
            if (dataNode.Type == DataNode.DataNodeType.Group)
                return true;

            foreach (DataNode subDataNode in dataNode.SubDataNodes)
            {
                if (subDataNode is AggregateExpressionDataNode)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Defines the data node  which DataGrouping  uses to calculate grouping expression and aggregation expression data 
        /// Loads result on DataSource of DataGroupingDataNode  
        /// </summary>
        /// <MetaDataID>{65c6f24f-8747-4db3-a077-ded3a45d957c}</MetaDataID>
        DataNode DataGroupingDataNode;
        /// <summary>
        /// Create an DataGrouping instance and initialize DataGroupingDataNode 
        ///  </summary>
        /// <param name="dataGroupingDataNode">
        ///  this parameter defines the data node  which DataGrouping  uses to calculate grouping expression and aggregation expression data and loads them on its DataSource  
        ///  </param>
        /// <MetaDataID>{b5912ebb-c752-4c2c-9635-3b1cef816b01}</MetaDataID>
        DataGrouping(DataNode dataGroupingDataNode)
        {
            DataGroupingDataNode = dataGroupingDataNode;
        }

        List<DataNode> RetrieveDataPathDataNodes = new List<DataNode>();

        /// <summary>
        /// Defines that the dataNode participates in RetrivieDataPath
        /// </summary>
        /// <param name="dataNode">
        /// Defines the DataNode which participates in RetrivieDataPath
        /// </param>
        internal void DataNodeParticipateInRetrieveDataPath(DataNode dataNode)
        {
            if (!RetrieveDataPathDataNodes.Contains(dataNode))//DerivedDataNode.GetOrgDataNode(dataNode)))
            {
                foreach (var selectionDataNode in RetrieveDataPathDataNodes)
                {
                    if (!selectionDataNode.IsSameOrParentDataNode(dataNode))
                    {
                        if (dataNode.ParentDataNode != null)
                            DataNodeParticipateInRetrieveDataPath(dataNode.ParentDataNode);
                        break;
                    }
                    else
                    {
                        DataNode parent = selectionDataNode.ParentDataNode;
                        while (parent != dataNode)
                        {
                            DataNodeParticipateInRetrieveDataPath(selectionDataNode.ParentDataNode);
                            parent = parent.ParentDataNode;
                        }
                        break;
                    }
                }

                RetrieveDataPathDataNodes.Add(dataNode);//DerivedDataNode.GetOrgDataNode(dataNode));

            }
        }



        /// <summary>
        /// This method builds the data node path for data retrieve.
        /// The method called for root data node as start entry point end then called recursively 
        /// for all sub data nodes which participate in RetrieveDataPathDataNodes collection 
        /// </summary>
        /// <param name="RetrieveDataPathDataNodes">
        /// This parameter defines the next DataRetrieveDataNode wich will be added on RetrieveDataPath
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
            DataNode rootDataNode = DataGroupingDataNode;

            if (DataGroupingDataNode is GroupDataNode)
                rootDataNode = (DataGroupingDataNode as GroupDataNode).GroupedDataNodeRoot;

            if ((DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Group) &&
              RetrieveDataPathDataNodes.Contains(dataRetrieveNode.DataNode))
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
                        if (RetrieveDataPathDataNodes.Contains(subDataNode) && DataRetrieveNode.GetDataRetrieveNode(subDataNode, retrieveDataPath) == null &&
                            (DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Group))
                            subNodes.Add(new DataRetrieveNode(subDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));
                    }
                }
                else if (DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Group)
                {
                    foreach (DataNode groupKeyDataNode in (dataRetrieveNode.DataNode as GroupDataNode).GroupKeyDataNodes)
                    {
                        if (RetrieveDataPathDataNodes.Contains(groupKeyDataNode) &&
                            groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                            subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));

                    }


                    foreach (DataNode subDataNode in dataRetrieveNode.DataNode.SubDataNodes)
                    {

                        if (DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Key)
                        {
                            foreach (DataNode groupKeyDataNode in subDataNode.SubDataNodes)
                            {
                                if (RetrieveDataPathDataNodes.Contains(groupKeyDataNode) && DataRetrieveNode.GetDataRetrieveNode(subDataNode, retrieveDataPath) == null)
                                    subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));
                            }
                        }
                    }
                }
                DataRetrieveNode parentDataNode = null;
                if (dataRetrieveNode.DataNode.ParentDataNode != null && RetrieveDataPathDataNodes.Contains(dataRetrieveNode.DataNode.ParentDataNode) && DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode.ParentDataNode, retrieveDataPath) == null)
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
              RetrieveDataPathDataNodes.Contains(dataRetrieveNode.DataNode) &&
              rootDataNode.ParentDataNode.Type == DataNode.DataNodeType.Group && (rootDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes.Count == 0)
            {
                //int count = (from order in storage.GetObjectCollection<Order>()
                //                  select order).Count();
                dataRetrieveNode = new DataRetrieveNode(rootDataNode.ParentDataNode, null, dataNodeRowIndices, retrieveDataPath);
                dataNodeRowIndices[dataRetrieveNode.DataNode] = retrieveDataPath.Count;
                retrieveDataPath.AddLast(dataRetrieveNode);
                return;
            }
        }


        /// <MetaDataID>{35186213-bc08-4165-9a0b-bb915937245c}</MetaDataID>
        /// <summary>
        /// Builds data retrieve path to reads data for grouping 
        /// </summary>
        /// <param name="groupingDataPath">
        /// groupingDataPath parameter defines a link list with data retrieve nodes which used to collect data for grouping.
        /// </param>
        /// <param name="dataRetrieveNode">
        /// Defines the next data retrieve node of groupingDataPath
        /// </param>
        /// <param name="unAssignedNodes">
        /// unAssignedNodes defines pending nodes to add in groupingDataPath.
        /// </param>
        public void BuildRetrieveDataPathOld(System.Collections.Generic.LinkedList<DataRetrieveNode> groupingDataPath, DataRetrieveNode dataRetrieveNode, System.Collections.Generic.List<DataRetrieveNode> unAssignedNodes)
        {
            Dictionary<DataNode, int> dataNodeRowIndices = null;
            if (groupingDataPath.Count > 0)
                dataNodeRowIndices = groupingDataPath.First.Value.DataNodeRowIndices;
            else
            {
                if (dataRetrieveNode != null)
                    dataNodeRowIndices = dataRetrieveNode.DataNodeRowIndices;
                else
                    dataNodeRowIndices = new Dictionary<DataNode, int>();
            }

            if (dataRetrieveNode == null)
                dataRetrieveNode = new DataRetrieveNode(this.DataGroupingDataNode, null, dataNodeRowIndices, groupingDataPath);
            if (dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Namespace)
            {
                BuildRetrieveDataPathOld(groupingDataPath, new DataRetrieveNode(dataRetrieveNode.DataNode.SubDataNodes[0], null, dataNodeRowIndices, groupingDataPath), unAssignedNodes);
                return;
            }

            System.Collections.Generic.List<DataRetrieveNode> subNodes = new System.Collections.Generic.List<DataRetrieveNode>();

            if ((dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Object || dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Group || dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.OjectAttribute) &&
               (dataRetrieveNode.DataNode.BranchParticipateInMemberAggregateFunctionOn(DataGroupingDataNode) || (dataRetrieveNode.DataNode == DataGroupingDataNode&&dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Object) || dataRetrieveNode.DataNode.BranchParticipateInGroopByAsKeyOn(DataGroupingDataNode as GroupDataNode) || dataRetrieveNode.DataNode.BranchParticipateAsGroopedDataNodeOn(DataGroupingDataNode as GroupDataNode)))
            {
                if (dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Object || dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Group)
                {
                    dataNodeRowIndices[dataRetrieveNode.DataNode] = groupingDataPath.Count;
                    groupingDataPath.AddLast(dataRetrieveNode);
                }


                #region retrieves all sub data nodes which participate in search condition as branch
                if (dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Group)
                {
                    foreach (DataNode subDataNode in (dataRetrieveNode.DataNode as GroupDataNode).GroupKeyDataNodes)
                    {
                        if ((subDataNode.BranchParticipateInMemberAggregateFunctionOn(DataGroupingDataNode) || subDataNode.BranchParticipateInGroopByAsKeyOn(DataGroupingDataNode as GroupDataNode)) &&
                            (subDataNode.Type == DataNode.DataNodeType.Object))
                            subNodes.Add(new DataRetrieveNode(subDataNode, dataRetrieveNode, dataNodeRowIndices, groupingDataPath));
                    }
                }
                else
                {
                    foreach (DataNode subDataNode in dataRetrieveNode.DataNode.SubDataNodes)
                    {
                        if (((subDataNode.BranchParticipateInMemberAggregateFunctionOn(DataGroupingDataNode) && !DataGroupingDataNode.DataSource.GroupedDataLoaded) ||
                            subDataNode.BranchParticipateInGroopByAsKeyOn(DataGroupingDataNode as GroupDataNode) ||
                            subDataNode.BranchParticipateAsGroopedDataNodeOn(DataGroupingDataNode as GroupDataNode)) &&
                            subDataNode.Type == DataNode.DataNodeType.Object)
                            subNodes.Add(new DataRetrieveNode(subDataNode, dataRetrieveNode, dataNodeRowIndices, groupingDataPath));
                    }
                }
                #endregion

                if (subNodes.Count > 0)
                {
                    #region Continues recursively with first subdatanodes the others are market as unassinged and will be added at the end.
                    DataRetrieveNode subDataNode = subNodes[0];
                    subNodes.RemoveAt(0);

                    unAssignedNodes.AddRange(subNodes);

                    BuildRetrieveDataPathOld(groupingDataPath, subDataNode, unAssignedNodes);
                    #endregion
                }
                else if (unAssignedNodes.Count > 0)
                {
                    #region Continues recursively with first unassigned datanode the others will be added at the end.
                    DataRetrieveNode subDataNode = unAssignedNodes[0];
                    unAssignedNodes.RemoveAt(0);
                    BuildRetrieveDataPathOld(groupingDataPath, subDataNode, unAssignedNodes);
                    #endregion
                }
            }
        }


        /// <summary>
        /// Grouping collections foreach group key Loaded
        /// </summary>
        /// <MetaDataID>{c6cda8a6-a6b7-448d-839d-45c8b9e90db9}</MetaDataID>
        internal bool GroupingCollectionsLoaded;

        ///<summary>
        ///Retrieve group data for group data node and load the data source table 
        ///</summary>
        /// <MetaDataID>{2ee9d2db-46c1-408d-bd06-03a612ff2e89}</MetaDataID>
        internal void LoadGroupingData()
        {
            if (DataGroupingDataNode.Type == DataNode.DataNodeType.Group)
            {
                if (!DataGroupingDataNode.DataSource.GroupedDataLoaded)
                    CalculateAndLoadGroupingResults();
            }
            else
            {
                bool runAggregateFunctions = false;
                foreach (DataNode dataNode in DataGroupingDataNode.SubDataNodes)
                {
                    if (dataNode is AggregateExpressionDataNode && !DataGroupingDataNode.DataSource.AggregateExpressionDataNodeResolved(dataNode as AggregateExpressionDataNode))
                    {
                        runAggregateFunctions = true;
                        break;
                    }
                }
                if (runAggregateFunctions)
                    LoadAggregateFunctionsResult();
            }
        }

        
        /// <summary>
        /// Defines a flag that indicates whether the data grouping mechanism should apply data filter on the data source
        /// </summary>
        /// <MetaDataID>{da2740a1-f141-4fe4-917a-03fe8edad14c}</MetaDataID>
        bool DataFilterApplied = false;
        /// <summary>
        /// Calculate grouping result and load them on GroupingDataNode DataSource 
        /// Also load grouping collections for each group key 
        /// </summary>
        /// <MetaDataID>{7e759df9-642f-4cd6-be82-652e65c02161}</MetaDataID>
        private void CalculateAndLoadGroupingResults()
        {
            //if ((DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition != null)
            //{
            //    var commonHeaderDataNode = DataGroupingDataNode.GetCommonHeaderDatanode((DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition);
            //    (DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition.FilterData(commonHeaderDataNode);
            //}

            //System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath = new System.Collections.Generic.LinkedList<DataRetrieveNode>();
            //Dictionary<DataNode, int> dataNodeRowIndices = new Dictionary<DataNode, int>();

            #region Builds the data retrieve path for the data which will grouped

            BuildDataRetrieveMetadata();
            
            #endregion

            int columnsCount = DataGroupingDataNode.DataSource.DataTable.Columns.Count;
            //groupDataIndecies defines a index table wich map the indecies of columns in group table 
            //with the columns  indecies in composite rows. 
            int[,] groupDataIndecies = new int[columnsCount, 2];

            #region Creates the grouping data keys indecies
            for (int i = 0; i < columnsCount; i++)
            {
                groupDataIndecies[i, 0] = -1;
                groupDataIndecies[i, 1] = -1;
            }

            if (DataGroupingDataNode.ParentDataNode != null)
            {

                foreach (ObjectIdentityType identityType in DataGroupingDataNode.ParentDataNode.DataSource.ObjectIdentityTypes)
                {
                    foreach (IIdentityPart identityPart in identityType.Parts)
                    {
                        int i = DataGroupingDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetDataTreeUniqueColumnName(DataGroupingDataNode.RealParentDataNode, identityPart));
                        groupDataIndecies[i, 0] = RetrieveDataPath.First.Value.DataNodeRowIndices[DataGroupingDataNode.RealParentDataNode];// RealParentDataNode.GroupedDataRowIndex;
                        groupDataIndecies[i, 1] = DataGroupingDataNode.RealParentDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetColumnName(DataGroupingDataNode.RealParentDataNode, identityPart));
                    }
                }
            }
            foreach (DataNode groupKeyDataNode in (DataGroupingDataNode as GroupDataNode).GroupKeyDataNodes)
            {
                if (groupKeyDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    int i = DataGroupingDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                    if (groupKeyDataNode.ParentDataNode.Type == DataNode.DataNodeType.Object)
                        groupDataIndecies[i, 0] = RetrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode.ParentDataNode]; //groupKeyDataNode.ParentDataNode.GroupedDataRowIndex;
                    else
                        groupDataIndecies[i, 0] = RetrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode.ParentDataNode.ParentDataNode]; //groupKeyDataNode.ParentDataNode.GroupedDataRowIndex;

                    if (groupKeyDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        groupDataIndecies[i, 1] = groupKeyDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                    else
                        groupDataIndecies[i, 1] = groupKeyDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                }
                else if (groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                {

                    foreach (ObjectIdentityType identityType in groupKeyDataNode.DataSource.ObjectIdentityTypes)
                    {
                        foreach (IIdentityPart identityPart in identityType.Parts)
                        {
                            int i = DataGroupingDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetDataTreeUniqueColumnName(groupKeyDataNode, identityPart));
                            groupDataIndecies[i, 0] = RetrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode];
                            if(groupKeyDataNode.DataSource.DataTable != null)
                                groupDataIndecies[i, 1] = groupKeyDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetColumnName(groupKeyDataNode, identityPart));
                            else
                            {


                            }
                        }
                    }
                }
            }
            #endregion

             
            System.Collections.Generic.List<IDataRow[]> composedRows = new System.Collections.Generic.List<IDataRow[]>();

            #region Retrieves Group data
            foreach (IDataRow row in RetrieveDataPath.First.Value.DataNode.DataSource.DataTable.Rows)
            {
                IDataRow[] composedRow = new IDataRow[RetrieveDataPath.Count];
                composedRow[RetrieveDataPath.First.Value.DataRowIndex] = row;
                //composedRow[groupingDataPath.First.Value.DataNode.GroupedDataRowIndex] = row;
                //RetrieveData(composedRow, groupingDataPath.First.Next, composedRows);
                RetrieveData(composedRow, RetrieveDataPath.First.Next, composedRows, RetrieveDataPath.First.Value.DataNodeRowIndices);
            }
            #endregion
            DataFilterApplied = true;
            //System.Collections.Generic.Dictionary<GroupingKey, System.Collections.Generic.List<System.Data.DataRow[]>> dataGroup = new System.Collections.Generic.Dictionary<GroupingKey, System.Collections.Generic.List<System.Data.DataRow[]>>();
            System.Collections.Generic.Dictionary<MultiPartKey, GroupingEntry> dataGroup = new Dictionary<MultiPartKey, GroupingEntry>();
            foreach (IDataRow[] composedRow in composedRows)
            {
                MultiPartKey groupingKey;

                #region Constructs the groupingKey
                groupingKey = GetGroupingKey(DataNodeRowIndices, composedRow);
                #endregion

                System.Collections.Generic.List<CompositeRowData> groupedCompositeRows = null;
                GroupingEntry groupingEntry = null;
                if (!dataGroup.TryGetValue(groupingKey, out groupingEntry))
                {
                    groupingEntry = new GroupingEntry(groupingKey, new System.Collections.Generic.List<CompositeRowData>());
                    groupedCompositeRows = groupingEntry.GroupedCompositeRows;// new System.Collections.Generic.List<System.Data.DataRow[]>();
                    //    groupingKey.GroupedRows = groupedCompositeRows;
                    dataGroup[groupingKey] = groupingEntry;

                }
                var compositeRowData = new CompositeRowData(composedRow);
                compositeRowData.EndofLoad();

                //Load grouping collection
                groupingEntry.GroupedCompositeRows.Add(compositeRowData);
            }
            System.Collections.Generic.List<AggregateExpressionDataNode> aggregateDataNodes = new System.Collections.Generic.List<AggregateExpressionDataNode>();
            foreach (DataNode subDataNode in DataGroupingDataNode.SubDataNodes)
            {
                if (subDataNode is AggregateExpressionDataNode)
                    aggregateDataNodes.Add(subDataNode as AggregateExpressionDataNode);
            }

            foreach (var entry in dataGroup)
            {
                IDataRow row = DataGroupingDataNode.DataSource.DataTable.NewRow();
                row[DataLoader.GetGroupingDataColumnName(DataGroupingDataNode)] = entry.Value;
                //entry.Key.DataRow = row;
                var composedRow = entry.Value.GroupedCompositeRows[0];

                #region load key values on data row
                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    if (groupDataIndecies[columnIndex, 0] != -1)
                        row[columnIndex] = composedRow[groupDataIndecies[columnIndex, 0]][groupDataIndecies[columnIndex, 1]];
                }
                #endregion

                #region Load aggregation expression results
                foreach (AggregateExpressionDataNode aggregateDataNode in aggregateDataNodes)
                {
                    if (aggregateDataNode.Type == DataNode.DataNodeType.Count)
                    {
                        int count = entry.Value.GroupedCompositeRows.Count;
                        row[aggregateDataNode.DataSourceColumnIndex] = count;
                    }
                    if (aggregateDataNode.Type == DataNode.DataNodeType.Sum)
                    {
                        object obj = aggregateDataNode.CalculateValue(DataNodeRowIndices, entry.Value.GroupedCompositeRows);
                        row[aggregateDataNode.DataSourceColumnIndex] = obj;
                    }
                    if (aggregateDataNode.Type == DataNode.DataNodeType.Average)
                    {
                        object obj = aggregateDataNode.CalculateValue(DataNodeRowIndices, entry.Value.GroupedCompositeRows);
                        row[aggregateDataNode.DataSourceColumnIndex] = obj;
                    }
                    if (aggregateDataNode.Type == DataNode.DataNodeType.Max)
                    {
                        object obj = aggregateDataNode.CalculateValue(DataNodeRowIndices, entry.Value.GroupedCompositeRows);
                        row[aggregateDataNode.DataSourceColumnIndex] = obj;
                    }
                    if (aggregateDataNode.Type == DataNode.DataNodeType.Min)
                    {
                        object obj = aggregateDataNode.CalculateValue(DataNodeRowIndices, entry.Value.GroupedCompositeRows);
                        row[aggregateDataNode.DataSourceColumnIndex] = obj;
                    }

                }
                #endregion

                DataGroupingDataNode.DataSource.DataTable.Rows.Add(row);
            }
            if (DataGroupingDataNode.ObjectQuery is DistributedObjectQuery)
            {
                DataGroupingDataNode.DataSource.DataLoaders[((DataGroupingDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectsContext).Identity]._Data = DataGroupingDataNode.DataSource.DataTable;
                DataGroupingDataNode.DataSource.DataLoaders[((DataGroupingDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectsContext).Identity].GroupedDataLoaded = true;
            }
            GroupingCollectionsLoaded = true;

        }

        /// <MetaDataID>{afcd83c6-0a72-4831-a81f-6b28dc2db816}</MetaDataID>
        private MultiPartKey GetGroupingKey(Dictionary<DataNode, int> dataNodeRowIndices, IDataRow[] composedRow)
        {
            MultiPartKey groupingKey;
            if (DataGroupingDataNode.ParentDataNode != null)
                groupingKey = new MultiPartKey((DataGroupingDataNode as GroupDataNode).GroupKeyDataNodes.Count + 1);
            else
                groupingKey = new MultiPartKey((DataGroupingDataNode as GroupDataNode).GroupKeyDataNodes.Count);
            
            int i = 0;
          
            foreach (DataNode groupKeyDataNode in (DataGroupingDataNode as GroupDataNode).GroupKeyDataNodes)
            {
                int dataSourceRowIndex = -1;
                dataSourceRowIndex = DataRetrieveNode.GetDataSourceRowIndex(dataNodeRowIndices, groupKeyDataNode);
                if (composedRow[dataSourceRowIndex] == null)
                    groupingKey.KeyPartsValues[i++] = null;
                else
                {
                    if (groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        string prefix = "";
                        if (groupKeyDataNode.DataSource.DataLoadedInParentDataSource)
                            prefix = groupKeyDataNode.Alias + "_";

                        var globalObjectID = StorageDataSource.GetGlobalObjectID(composedRow[dataNodeRowIndices[groupKeyDataNode]], groupKeyDataNode.DataSource.ObjectIdentityTypes,prefix);
                        groupingKey.KeyPartsValues[i++] = globalObjectID;
                    }
                    else
                        groupingKey.KeyPartsValues[i++] = composedRow[dataSourceRowIndex][groupKeyDataNode.DataSourceColumnIndex];
                }
            }
            
            if (DataGroupingDataNode.ParentDataNode != null)
            {
                string prefix = "";
                if (DataGroupingDataNode.RealParentDataNode.DataSource.DataLoadedInParentDataSource)
                    prefix = DataGroupingDataNode.RealParentDataNode.Alias + "_";
                var globalObjectID = StorageDataSource.GetGlobalObjectID(composedRow[dataNodeRowIndices[DataGroupingDataNode.RealParentDataNode]], DataGroupingDataNode.RealParentDataNode.DataSource.ObjectIdentityTypes, prefix);
                groupingKey.KeyPartsValues[i++] = globalObjectID.Value;// composedRow[dataNodeRowIndices[DataGroupingDataNode.RealParentDataNode]][DataGroupingDataNode.RealParentDataNode.DataSourceColumnIndex];
            }
            return groupingKey;
        }

        /// <MetaDataID>{8a964d28-8c3b-4690-a0ed-ace6746fc2d4}</MetaDataID>
        private bool FilterConditionApplied(SearchCondition filterCondition)
        {

            bool hasCriterionsToApply = false;
            foreach (Criterion criterion in filterCondition.Criterions)
            {
                if (/*!filterCondition.GlobalCriterions.Contains(criterion) &&*/!criterion.Applied)
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
            else
                return true;

        }

        /// <MetaDataID>{53b63e6b-2a07-4471-b56d-d952f9bec249}</MetaDataID>
        [NonSerialized]
        internal System.Collections.Generic.LinkedList<DataRetrieveNode> RetrieveDataPath = new System.Collections.Generic.LinkedList<DataRetrieveNode>();
        /// <MetaDataID>{c56904da-c4c9-4d89-9109-b3d4b1cdd998}</MetaDataID>
        internal Dictionary<DataNode, int> DataNodeRowIndices = new Dictionary<DataNode, int>();


        /// <summary>
        /// Load grouping collection foreach group key
        /// </summary>
        /// <MetaDataID>{0c72ff03-93f2-4ba7-97b3-9c901f22cda5}</MetaDataID>
        internal void LoadGroupingCollections()
        {
            try
            {
                //if ((DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition != null)
                //{
                //    var commonHeaderDataNode = DataGroupingDataNode.GetCommonHeaderDatanode((DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition);
                //    (DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition.FilterData(commonHeaderDataNode);
                //}
                #region Builds the data retrieve path for the data which will grouped

                BuildDataRetrieveMetadata();

                #endregion

                Dictionary<DataNode, List<int[]>> groupKeyIndecies;
                int keyPartsNum;
                GetGroupKeyIndices(out groupKeyIndecies, out keyPartsNum);

                System.Collections.Generic.List<IDataRow[]> composedRows = new System.Collections.Generic.List<IDataRow[]>();

                #region Retrieves Group data
                foreach (IDataRow row in RetrieveDataPath.First.Value.DataNode.DataSource.DataTable.Rows)
                {
                    IDataRow[] composedRow = new IDataRow[RetrieveDataPath.Count];
                    composedRow[RetrieveDataPath.First.Value.DataRowIndex] = row;
                    //composedRow[groupingDataPath.First.Value.DataNode.GroupedDataRowIndex] = row;
                    //RetrieveData(composedRow, groupingDataPath.First.Next, composedRows);
                    RetrieveData(composedRow, RetrieveDataPath.First.Next, composedRows, RetrieveDataPath.First.Value.DataNodeRowIndices);
                }
                #endregion

                System.Collections.Generic.Dictionary<MultiPartKey, GroupingEntry> dataGroup = new Dictionary<MultiPartKey, GroupingEntry>();

                foreach (IDataRow[] composedRow in composedRows)
                {

                    MultiPartKey groupingKey = GetGroupingKey(DataNodeRowIndices, composedRow);

                    #region Constructs the groupingKey
                    //if (DataGroupingDataNode.ParentDataNode != null)
                    //    groupingKey = new GroupingKey((DataGroupingDataNode as GroupDataNode).GroupKeyDataNodes.Count + 1);
                    //else
                    //    groupingKey = new GroupingKey((DataGroupingDataNode as GroupDataNode).GroupKeyDataNodes.Count);

                    //groupingKey = new GroupingKey(keyPartsNum);

                    //int i = 0;

                    //foreach (var keyColumnsIndices in groupKeyIndecies.Values)
                    //{
                    //    foreach (var keyColumnIndices in keyColumnsIndices)
                    //        groupingKey.KeyPartsValues[i++] = composedRow[keyColumnIndices[1]][keyColumnIndices[2]];
                    //}

                    #endregion

                    GroupingEntry groupingEntry = null;
                    if (!dataGroup.TryGetValue(groupingKey, out groupingEntry))
                    {
                        groupingEntry = new GroupingEntry(groupingKey, new System.Collections.Generic.List<CompositeRowData>());
                        dataGroup[groupingKey] = groupingEntry;
                    }
                    var compositeRowData = new CompositeRowData(composedRow);
                    compositeRowData.EndofLoad();
                    
                    //Load grouping collection
                    groupingEntry.GroupedCompositeRows.Add(compositeRowData);
                }
                System.Collections.Generic.List<AggregateExpressionDataNode> aggregateDataNodes = new System.Collections.Generic.List<AggregateExpressionDataNode>();
                foreach (DataNode subDataNode in DataGroupingDataNode.SubDataNodes)
                {
                    if (subDataNode is AggregateExpressionDataNode)
                        aggregateDataNodes.Add(subDataNode as AggregateExpressionDataNode);
                }


                foreach (IDataRow dataRow in DataGroupingDataNode.DataSource.DataTable.Rows)
                {
                   // var groupingKey = DataGroupingDataNode.DataSource.GetGroupingKey(dataRow);
                    var groupingKey = (dataRow[DataLoader.GetGroupingDataColumnName(DataGroupingDataNode)] as GroupingEntry).GroupingKey;
                    //var groupingKey = new GroupingKey(keyPartsNum);
                    //int i = 0;
                    //foreach (var keyColumnsIndices in groupKeyIndecies.Values)
                    //{
                    //    foreach (var keyColumnIndices in keyColumnsIndices)
                    //        groupingKey.KeyPartsValues[i++] = dataRow[keyColumnIndices[0]];
                    //}
                    dataRow[DataLoader.GetGroupingDataColumnName(DataGroupingDataNode)] = dataGroup[groupingKey];
                }
                //foreach (System.Collections.Generic.KeyValuePair<GroupingKey, System.Collections.Generic.List<System.Data.DataRow[]>> entry in dataGroup)
                //{
                //    System.Data.DataRow row = DataGroupingDataNode.DataSource.DataTable.NewRow();
                //    row[DataLoader.GetGroupingDataColumnName(DataGroupingDataNode)] = entry.Key;
                //    //entry.Key.DataRow = row;
                //    System.Data.DataRow[] composedRow = entry.Value[0];

                //    #region load key values on data row
                //    for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                //    {
                //        if (groupDataIndecies[columnIndex, 0] != -1)
                //            row[columnIndex] = composedRow[groupDataIndecies[columnIndex, 0]][groupDataIndecies[columnIndex, 1]];
                //    }
                //    #endregion

                //    #region Load aggregation expression results
                //    foreach (AggregateExpressionDataNode aggregateDataNode in aggregateDataNodes)
                //    {
                //        if (aggregateDataNode.Type == DataNode.DataNodeType.Count)
                //        {
                //            int count = entry.Value.Count;
                //            row[aggregateDataNode.DataSourceColumnIndex] = count;
                //        }
                //        if (aggregateDataNode.Type == DataNode.DataNodeType.Sum)
                //        {
                //            object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, entry.Value);
                //            row[aggregateDataNode.DataSourceColumnIndex] = obj;
                //        }
                //        if (aggregateDataNode.Type == DataNode.DataNodeType.Average)
                //        {
                //            object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, entry.Value);
                //            row[aggregateDataNode.DataSourceColumnIndex] = obj;
                //        }
                //        if (aggregateDataNode.Type == DataNode.DataNodeType.Max)
                //        {
                //            object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, entry.Value);
                //            row[aggregateDataNode.DataSourceColumnIndex] = obj;
                //        }
                //        if (aggregateDataNode.Type == DataNode.DataNodeType.Min)
                //        {
                //            object obj = aggregateDataNode.CalculateValue(dataNodeRowIndices, entry.Value);
                //            row[aggregateDataNode.DataSourceColumnIndex] = obj;
                //        }

                //    }
                //    #endregion

                //    DataGroupingDataNode.DataSource.DataTable.Rows.Add(row);
                //}
                if (DataGroupingDataNode.ObjectQuery is DistributedObjectQuery)
                {
                    DataGroupingDataNode.DataSource.DataLoaders[((DataGroupingDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity]._Data = DataGroupingDataNode.DataSource.DataTable;
                    DataGroupingDataNode.DataSource.DataLoaders[((DataGroupingDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity].GroupedDataLoaded = true;
                }
            }
            finally
            {
                GroupingCollectionsLoaded = true;
            }

        }

        private void BuildDataRetrieveMetadata()
        {
            var unAssignedNodes = new List<DataRetrieveNode>();

            if ((DataGroupingDataNode as GroupDataNode).GroupedDataNodeRoot != DataGroupingDataNode.RealParentDataNode && DataGroupingDataNode.RealParentDataNode != null && DataGroupingDataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
            {
                DataNodeParticipateInRetrieveDataPath(DataGroupingDataNode.RealParentDataNode);
                DataNodeRowIndices[DataGroupingDataNode.RealParentDataNode] = RetrieveDataPath.Count;
                RetrieveDataPath.AddLast(new DataRetrieveNode(DataGroupingDataNode.RealParentDataNode, null, DataNodeRowIndices, RetrieveDataPath));
            }

            DataNodeParticipateInRetrieveDataPath((DataGroupingDataNode as GroupDataNode).GroupedDataNodeRoot);
            foreach (DataNode dataNode in DataGroupingDataNode.SubDataNodes)
            {
                if (dataNode is AggregateExpressionDataNode)
                {
                    if ((dataNode as AggregateExpressionDataNode).ArithmeticExpression != null)
                    {
                        foreach (var arithmeticExpressionDataNode in (dataNode as AggregateExpressionDataNode).ArithmeticExpression.ArithmeticExpressionDataNodes)
                            DataNodeParticipateInRetrieveDataPath(arithmeticExpressionDataNode);
                    }
                    else
                    {
                        foreach (var aggregateExpressionDataNode in (dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                            DataNodeParticipateInRetrieveDataPath(aggregateExpressionDataNode);
                    }
                }
            }

            foreach (DataNode keyDataNode in (DataGroupingDataNode as GroupDataNode).GroupKeyDataNodes)
                DataNodeParticipateInRetrieveDataPath(keyDataNode);

            DataNode headerDataNode = DataGroupingDataNode.RealParentDataNode;

            if (DataFilter != null && !FilterConditionApplied(DataFilter))
            {
                foreach (var filterDataNode in DataFilter.DataNodes)
                    DataNodeParticipateInRetrieveDataPath(filterDataNode);

                //var filterHeaderDataNode = (DataGroupingDataNode as GroupDataNode).GroupedDataNode.GetCommonHeaderDatanode(dataFilter);
                //if (headerDataNode.IsParentDataNode(filterHeaderDataNode))
                //    DataNodeParticipateInRetrieveDataPath(filterHeaderDataNode);
            }
            else
                DataFilterApplied = true;

            if ((DataGroupingDataNode as GroupDataNode).GroupedDataNodeRoot == headerDataNode )
                BuildRetrieveDataPath(new DataRetrieveNode(headerDataNode, null, DataNodeRowIndices, RetrieveDataPath), DataNodeRowIndices, RetrieveDataPath, unAssignedNodes);
            else
                BuildRetrieveDataPath(new DataRetrieveNode((DataGroupingDataNode as GroupDataNode).GroupedDataNodeRoot, DataRetrieveNode.GetDataRetrieveNode(headerDataNode, RetrieveDataPath), DataNodeRowIndices, RetrieveDataPath), DataNodeRowIndices, RetrieveDataPath, unAssignedNodes);

            //if (dataFilter == null)
            //    DataFilterApplied = true;
            //if (dataFilter != null && !DataFilterApplied)
            //    dataFilter.BuildRetrieveDataPath(DataRetrieveNode.GetDataRetrieveNode((DataGroupingDataNode as GroupDataNode).GroupedDataNode.GetCommonHeaderDatanode(dataFilter), RetrieveDataPath), DataNodeRowIndices, RetrieveDataPath, unAssignedNodes, true);
            //else
            //    DataFilterApplied = true;
        }

        /// <MetaDataID>{a5ade450-4005-4db2-ad3b-7f2903e521b9}</MetaDataID>
        private void GetGroupKeyIndices(out Dictionary<DataNode, List<int[]>> groupKeyIndecies, out int keyPartsNum)
        {
            int columnsCount = DataGroupingDataNode.DataSource.DataTable.Columns.Count;
            //groupDataIndecies defines a index table wich map the indecies of columns in group table 
            //with the columns  indecies in composite rows. 
            int[,] groupDataIndecies = new int[columnsCount, 2];

            //List<int[]> groupKeyIndecies = new List<int[]>();
            groupKeyIndecies = new Dictionary<DataNode, List<int[]>>();

            #region Creates the grouping data keys indecies
            for (int i = 0; i < columnsCount; i++)
            {
                groupDataIndecies[i, 0] = -1;
                groupDataIndecies[i, 1] = -1;
            }
            keyPartsNum = 0;

            if (DataGroupingDataNode.ParentDataNode != null)
            {
                List<int[]> keyColumnsIndices = new List<int[]>();
                groupKeyIndecies[DataGroupingDataNode.ParentDataNode] = keyColumnsIndices;

                foreach (ObjectIdentityType identityType in DataGroupingDataNode.ParentDataNode.DataSource.ObjectIdentityTypes)
                {
                    foreach (IIdentityPart identityPart in identityType.Parts)
                    {
                        int[] keyColumnIndices = new int[3];
                        int i = DataGroupingDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetDataTreeUniqueColumnName(DataGroupingDataNode.RealParentDataNode, identityPart));
                        groupDataIndecies[i, 0] = RetrieveDataPath.First.Value.DataNodeRowIndices[DataGroupingDataNode.RealParentDataNode];// RealParentDataNode.GroupedDataRowIndex;
                        groupDataIndecies[i, 1] = DataGroupingDataNode.RealParentDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetColumnName(DataGroupingDataNode.RealParentDataNode, identityPart));
                        keyColumnIndices[0] = i;
                        keyColumnIndices[1] = groupDataIndecies[i, 0];
                        keyColumnIndices[2] = groupDataIndecies[i, 1];
                        keyColumnsIndices.Add(keyColumnIndices);
                        keyPartsNum++;
                    }
                }
            }
            foreach (DataNode groupKeyDataNode in (DataGroupingDataNode as GroupDataNode).GroupKeyDataNodes)
            {
                if (groupKeyDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    int[] keyColumnIndices = new int[3];
                    int i = DataGroupingDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                    if (groupKeyDataNode.ParentDataNode.Type == DataNode.DataNodeType.Object)
                        groupDataIndecies[i, 0] = RetrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode.ParentDataNode]; //groupKeyDataNode.ParentDataNode.GroupedDataRowIndex;
                    else
                        groupDataIndecies[i, 0] = RetrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode.ParentDataNode.ParentDataNode]; //groupKeyDataNode.ParentDataNode.GroupedDataRowIndex;

                    if (groupKeyDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        groupDataIndecies[i, 1] = groupKeyDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                    else
                        groupDataIndecies[i, 1] = groupKeyDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                    keyColumnIndices[0] = i;
                    keyColumnIndices[1] = groupDataIndecies[i, 0];
                    keyColumnIndices[2] = groupDataIndecies[i, 1];
                    groupKeyIndecies[groupKeyDataNode] = new List<int[]> { keyColumnIndices };
                    keyPartsNum++;
                }
                else if (groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                {
                    List<int[]> keyColumnsIndices = new List<int[]>();
                    groupKeyIndecies[groupKeyDataNode] = keyColumnsIndices;
                    foreach (ObjectIdentityType identityType in groupKeyDataNode.DataSource.ObjectIdentityTypes)
                    {
                        foreach (IIdentityPart identityPart in identityType.Parts)
                        {
                            int[] keyColumnIndices = new int[3];
                            int i = DataGroupingDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetDataTreeUniqueColumnName(groupKeyDataNode, identityPart));
                            groupDataIndecies[i, 0] = RetrieveDataPath.First.Value.DataNodeRowIndices[groupKeyDataNode];
                            groupDataIndecies[i, 1] = groupKeyDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetColumnName(groupKeyDataNode, identityPart));
                            keyColumnIndices[0] = i;
                            keyColumnIndices[1] = groupDataIndecies[i, 0];
                            keyColumnIndices[2] = groupDataIndecies[i, 1];
                            keyColumnsIndices.Add(keyColumnIndices);
                            keyPartsNum++;
                        }
                    }
                }
            }
            #endregion
        }


        /// <summary>
        /// Calculate and load aggregation functions results.
        /// </summary>
        /// <MetaDataID>{fef375bd-b695-4980-b34d-e776088b5ea4}</MetaDataID>
        private void LoadAggregateFunctionsResult()
        {

            //System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath = new System.Collections.Generic.LinkedList<DataRetrieveNode>();
            // Dictionary<DataNode, int> dataNodeRowIndices = new Dictionary<DataNode, int>();
           // DataNodeRowIndices[DataGroupingDataNode] = RetrieveDataPath.Count;
            //retrieveDataPath.AddLast(new DataRetrieveNode(RootDataNode, null, dataNodeRowIndices));
            DataNodeParticipateInRetrieveDataPath(DataGroupingDataNode );

            System.Collections.Generic.List<AggregateExpressionDataNode> aggregateDataNodes = new List<AggregateExpressionDataNode>();
            if (DataGroupingDataNode.ObjectQuery is DistributedObjectQuery)
            {
                aggregateDataNodes = (from aggregateDataNode in DataGroupingDataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                                      where !DataGroupingDataNode.DataSource.AggregateExpressionDataNodeResolved(aggregateDataNode) && (DataGroupingDataNode.DataSource.DataLoaders[((DataGroupingDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity] as StorageDataLoader).CheckAggregateFunctionForLocalResolve(aggregateDataNode)
                                      select aggregateDataNode).ToList();
            }
            else
            {
                aggregateDataNodes = (from aggregateDataNode in DataGroupingDataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                                      where !DataGroupingDataNode.DataSource.AggregateExpressionDataNodeResolved(aggregateDataNode)
                                      select aggregateDataNode).ToList();
            }


            foreach (DataNode dataNode in aggregateDataNodes)
            {
                if (dataNode is AggregateExpressionDataNode)
                {
                    if ((dataNode as AggregateExpressionDataNode).ArithmeticExpression != null)
                    {
                        foreach (var arithmeticExpressionDataNode in (dataNode as AggregateExpressionDataNode).ArithmeticExpression.ArithmeticExpressionDataNodes)
                            DataNodeParticipateInRetrieveDataPath(arithmeticExpressionDataNode);

                        foreach (var aggregateExpressionDataNode in (dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                            DataNodeParticipateInRetrieveDataPath(aggregateExpressionDataNode);
                    }
                    else
                    {
                        foreach (var aggregateExpressionDataNode in (dataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes)
                            DataNodeParticipateInRetrieveDataPath(aggregateExpressionDataNode);
                    }
                }
            }

            BuildRetrieveDataPath(new DataRetrieveNode(DataGroupingDataNode, null, DataNodeRowIndices, RetrieveDataPath), DataNodeRowIndices, RetrieveDataPath, new List<DataRetrieveNode>());

            if (aggregateDataNodes.Count > 0)
            {
                System.Collections.Generic.List<IDataRow[]> composedRows = new System.Collections.Generic.List<IDataRow[]>();
                foreach (IDataRow row in RetrieveDataPath.First.Value.DataNode.DataSource.DataTable.Rows)
                {
                    composedRows.Clear();
                    IDataRow[] composedRow = new IDataRow[RetrieveDataPath.Count];
                    composedRow[RetrieveDataPath.First.Value.DataRowIndex] = row;
                    RetrieveData(composedRow, RetrieveDataPath.First.Next, composedRows, RetrieveDataPath.First.Value.DataNodeRowIndices);
                    composedRows.Clear();
                    composedRow = new IDataRow[RetrieveDataPath.Count];
                    composedRow[RetrieveDataPath.First.Value.DataRowIndex] = row;

                    RetrieveData(composedRow, RetrieveDataPath.First.Next, composedRows, RetrieveDataPath.First.Value.DataNodeRowIndices);

                    foreach (AggregateExpressionDataNode aggregateDataNode in aggregateDataNodes)
                    {
                        if (aggregateDataNode.Type == DataNode.DataNodeType.Count)
                        {
                            int count = composedRows.Count;
                            if (aggregateDataNode.SourceSearchCondition != null)
                            {
                                count = 0;
                                foreach (var compositeRow in composedRows)
                                {
                                    if (aggregateDataNode.SourceSearchCondition.DoesRowPassCondition(compositeRow, DataNodeRowIndices))
                                        count++;
                                }
                                row[aggregateDataNode.DataSourceColumnIndex] = count;
                            }
                            else
                                row[aggregateDataNode.DataSourceColumnIndex] = count;
                        }
                        if (aggregateDataNode.Type == DataNode.DataNodeType.Sum)
                        {
                            object obj = aggregateDataNode.CalculateValue(DataNodeRowIndices, composedRows);
                            row[aggregateDataNode.DataSourceColumnIndex] = obj;
                        }
                        if (aggregateDataNode.Type == DataNode.DataNodeType.Average)
                        {
                            object obj = aggregateDataNode.CalculateValue(DataNodeRowIndices, composedRows);
                            row[aggregateDataNode.DataSourceColumnIndex] = obj;
                        }
                        if (aggregateDataNode.Type == DataNode.DataNodeType.Max)
                        {
                            object obj = aggregateDataNode.CalculateValue(DataNodeRowIndices, composedRows);
                            row[aggregateDataNode.DataSourceColumnIndex] = obj;
                        }
                        if (aggregateDataNode.Type == DataNode.DataNodeType.Min)
                        {
                            object obj = aggregateDataNode.CalculateValue(DataNodeRowIndices, composedRows);
                            row[aggregateDataNode.DataSourceColumnIndex] = obj;
                        }

                    }
                }
                if (DataGroupingDataNode.ObjectQuery is DistributedObjectQuery)
                {
                    foreach (AggregateExpressionDataNode aggregateDataNode in aggregateDataNodes)
                        DataGroupingDataNode.DataSource.DataLoaders[((DataGroupingDataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity].AggregateExpressionDataNodeResolved(aggregateDataNode);


                }
            }



        }


        /// <summary>
        /// Defines the filter for gouping source data 
        /// </summary>
        SearchCondition DataFilter
        {
            get
            {
                if (DataGroupingDataNode is GroupDataNode)
                    return (DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition;
                else
                    return null;
            }
        }

        ///<summary>
        ///Load data in composedrow collection based on groupingDataPath
        ///</summary>
        ///<param name="composedRow">
        ///Defines current composedRow
        ///</param>
        ///<param name="composedRows">
        ///Defines composedRow collection
        ///</param>
        ///<param name="groupingDataPath">
        ///Define the data path which used for data retrieve  
        ///</param>
        /// <MetaDataID>{e478a6dc-578d-4406-b30a-4fd1ef3d1f6e}</MetaDataID>
        private void RetrieveData(IDataRow[] composedRow,
                          System.Collections.Generic.LinkedListNode<DataRetrieveNode> groupingDataPath,
                          System.Collections.Generic.List<IDataRow[]> composedRows, Dictionary<DataNode, int> dataNodeRowIndices)
        {

            if (groupingDataPath == null)
            {
                if (DataFilter != null&&!DataFilterApplied)
                {
                    if (DataFilter.DoesRowPassCondition(composedRow, dataNodeRowIndices))
                        composedRows.Add(composedRow.Clone() as IDataRow[]);
                }
                else
                    composedRows.Add(composedRow.Clone() as IDataRow[]);
            }
            else
            {
                System.Collections.Generic.ICollection<IDataRow> rows = null;
                if (composedRow[groupingDataPath.Value.DataNodeRowIndices[groupingDataPath.Previous.Value.DataNode]] != null)
                    rows = groupingDataPath.Value.MasterDataNode.DataNode.DataSource.GetRelatedRows(composedRow[groupingDataPath.Value.MasterDataNode.DataRowIndex], groupingDataPath.Value.DataNode);
                if (rows == null || rows.Count == 0)
                {
                    //TODO να γραφτεί test σενάριο για αυτήν την περίπτωση.
                    composedRow[groupingDataPath.Value.DataRowIndex] = null;
                    return;
                }
                else
                {
                    foreach (IDataRow row in rows)
                    {
                        #region RowRemove code
                        //if (groupingDataPath.Value.DataNode.SearchCondition != null && groupingDataPath.Value.DataNode.SearchCondition.IsRemovedRow(row, -1))
                        //    continue;
                        //if (DataGroupingDataNode is GroupDataNode && (DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition != null &&
                        //    ((groupingDataPath.Value.DataNode.SearchCondition != null &&
                        //    !groupingDataPath.Value.DataNode.SearchCondition.ContainsSearchCondition((DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition)) ||
                        //    groupingDataPath.Value.DataNode.SearchCondition == null))
                        //{
                        //    if ((DataGroupingDataNode as GroupDataNode).GroupingSourceSearchCondition.IsRemovedRow(row, -1))
                        //        continue;
                        //}
                        #endregion

                        composedRow[groupingDataPath.Value.DataRowIndex] = row;
                        RetrieveData(composedRow, groupingDataPath.Next, composedRows, dataNodeRowIndices);
                    }
                }
            }
        }



    }
}
