using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Linq
{
    class BridgeListRemovedCode
    {

        #region Code which used to build composite row format and assigne the composite row indices


        /// <summary>
        /// This field defines the index of row in composite row 
        /// which use the dynamic type data retriever to retrieve the conventional type object.
        /// </summary>
        /// <MetaDataID>{4b6e222d-83c5-4ce2-863c-72428a9005ba}</MetaDataID>
        int ConventionTypeRowIndex = -1;
        /// <summary>
        /// This field defines the index of column in row 
        /// which use the dynamic type data retriever to retrieve the conventional type object.
        /// </summary>
        /// <MetaDataID>{f35a6456-d858-40cf-bace-7e42cc1fa7dc}</MetaDataID>
        int ConventionTypeColumnIndex = -1;


        /// <exclude>Excluded</exclude>
        System.Collections.Generic.LinkedList<DataRetrieveNode> _DataRetrievePath;
        /// <summary>
        /// Defines the path on Data tree where the system use to retrieve the values of objects of this dynamic type. 
        /// </summary>
        /// <MetaDataID>{544a52a8-13f6-4638-8150-d6b523be23e0}</MetaDataID>
        System.Collections.Generic.LinkedList<DataRetrieveNode> DataRetrievePath
        {
            get
            {
                if (_DataRetrievePath == null)
                    BuildDataRetrieveMetadata();
                return _DataRetrievePath;
            }
        }
        /// <MetaDataID>{a19e59c8-a4cd-4ec2-86f1-175b902b8d4b}</MetaDataID>
        public void BuildRetrieveDataPath(Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath, List<DataRetrieveNode> unAssignedNodes)
        {
            DataRetrieveNode dataRetrieveNode = new DataRetrieveNode(RootDataNode, null, dataNodeRowIndices, retrieveDataPath);
            BuildRetrieveDataPath(dataRetrieveNode, dataNodeRowIndices, retrieveDataPath, unAssignedNodes);

            #region Builds RetrieveDataPath for all properties where the type is dynamic type and the dynamic type retriever isn't enumerator and for GroupingMetaData if exist.
            if (Properties != null)
            {
                foreach (KeyValuePair<System.Reflection.PropertyInfo, DynamicTypeProperty> entry in Properties)
                {
                    if (entry.Value.PropertyTypeIsDynamic && !entry.Value.PropertyTypeIsEnumerable && entry.Key.PropertyType.Name != typeof(System.Linq.IGrouping<,>).Name)
                        entry.Value.PropertyType.BuildRetrieveDataPath(dataNodeRowIndices, retrieveDataPath, unAssignedNodes);
                    else
                    {
                        if (entry.Value.SourceDataNode is AggregateExpressionDataNode &&
                            entry.Value.SourceDataNode.ParentDataNode != dataRetrieveNode.DataNode &&
                            entry.Value.SourceDataNode.RealParentDataNode == dataRetrieveNode.DataNode &&
                            SelectionDataNodesAsBranch.Contains(entry.Value.SourceDataNode.ParentDataNode))
                        {
                            BuildRetrieveDataPath(new DataRetrieveNode(entry.Value.SourceDataNode.ParentDataNode, DataRetrieveNode.GetDataRetrieveNode(entry.Value.SourceDataNode.ParentDataNode.ParentDataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath), dataNodeRowIndices, retrieveDataPath, unAssignedNodes);
                        }
                    }
                }
            }
            if (GroupingMetaData != null && GroupingMetaData.GroupedDataRetrieve.RootDataNode.Type == DataNode.DataNodeType.Object)
                GroupingMetaData.GroupedDataRetrieve.BuildRetrieveDataPath(dataNodeRowIndices, retrieveDataPath, unAssignedNodes);
            #endregion
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
        public void BuildRetrieveDataPath(DataRetrieveNode dataRetrieveNode, Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataRetrieveNode> retrieveDataPath, List<DataRetrieveNode> unAssignedNodes)
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
                        if (SelectionDataNodesAsBranch.Contains(subDataNode) &&
                            DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Group)
                            subNodes.Add(new DataRetrieveNode(subDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices, retrieveDataPath));
                    }
                }
                //else if (RootDataNode == dataRetrieveNode.DataNode && dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Group && GroupingMetaData == null)
                //{
                //    foreach (DataNode groupKeyDataNode in dataRetrieveNode.DataNode.GroupKeyDataNodes)
                //    {
                //        if (SelectionDataNodesAsBranch.Contains(groupKeyDataNode) &&
                //            groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                //            subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices));

                //        //if (SelectionDataNodesAsBranch.Contains(groupKeyDataNode) &&
                //        //    groupKeyDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                //        //    subNodes.Add(new DataRetrieveNode(groupKeyDataNode.ParentDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices));
                //    }
                //}
                //else if (RootDataNode != dataRetrieveNode.DataNode && dataRetrieveNode.DataNode.Type == DataNode.DataNodeType.Group)
                //{
                //    foreach (DataNode groupKeyDataNode in dataRetrieveNode.DataNode.GroupKeyDataNodes)
                //    {
                //        if (SelectionDataNodesAsBranch.Contains(groupKeyDataNode) &&
                //            groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                //            subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices));

                //        //if (SelectionDataNodesAsBranch.Contains(groupKeyDataNode) &&
                //        //    groupKeyDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                //        //    subNodes.Add(new DataRetrieveNode(groupKeyDataNode.ParentDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, retrieveDataPath), dataNodeRowIndices));

                //    }
                //}
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






        /// <summary>
        /// This field keeps the count of columns which needed to host the dynamic built data.
        /// </summary>
        /// <MetaDataID>{fe7a533a-c0c5-4cad-ba58-36a8d2de1e4b}</MetaDataID>
        int ExtensionRowColumnsCount = 0;
        /// <summary>
        /// This method builds the RetrieveDataPath and 
        /// assign indices on composite row to the properties metadata 
        /// in case where the dynamic type data retriever refer to dynamic type (anonymous type,)
        /// or set the members ConventionTypeRowIndex,ConventionTypeColumnIndex with corresponding indices
        /// when the dynamic type data retriever refer to conventional type.
        /// </summary>
        /// <MetaDataID>{2d73e21c-42f7-49b8-96c3-1acf77238945}</MetaDataID>
        private void BuildDataRetrieveMetadata()
        {
            if (_DataRetrievePath == null)
            {
                _DataRetrievePath = new System.Collections.Generic.LinkedList<DataRetrieveNode>();
                List<DataRetrieveNode> unAssignedNodes = new List<DataRetrieveNode>();
                DataNode headerDataNode = RootDataNode;

                #region Gets header data node

                if (RootDataNode.Type == DataNode.DataNodeType.Group)
                {
                    if (MemberDataNode != null && MemberDataNode.Type == DataNode.DataNodeType.Key)
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
                if (headerDataNode == RootDataNode && Type.Name == typeof(System.Linq.IGrouping<,>).Name)
                {
                    //if (MemberDataNode != null)
                    //{
                    //    SelectionDataNodesAsBranch.Remove(RootDataNode);
                    //    foreach (DataNode groupingDataNode in RootDataNode.SubDataNodes)
                    //    {
                    //        if (groupingDataNode.Type == DataNode.DataNodeType.Object || groupingDataNode.Type == DataNode.DataNodeType.Group)
                    //        {
                    //            headerDataNode = groupingDataNode;
                    //            break;
                    //        }
                    //    }
                    //}
                    headerDataNode = (RootDataNode as GroupDataNode).GroupByDataNodeRoot;
                }
                if (RootDataNode.Type == DataNode.DataNodeType.Group && Type.Name == typeof(System.Linq.IGrouping<,>).Name)
                    headerDataNode = (RootDataNode as GroupDataNode).GroupByDataNodeRoot;


                #endregion


                DataRetrieveNode dataRetrieveNode = new DataRetrieveNode(headerDataNode, null, _DataNodeRowIndices, _DataRetrievePath);
                BuildRetrieveDataPath(dataRetrieveNode, _DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);

                #region Builds RetrieveDataPath for all properties where the type is dynamic type and the dynamic type retriever isn't enumerator and for GroupingMetaData if exist.

                if (Properties != null)
                {
                    foreach (KeyValuePair<System.Reflection.PropertyInfo, DynamicTypeProperty> entry in Properties)
                    {
                        if (entry.Value.PropertyTypeIsDynamic && !entry.Value.PropertyTypeIsEnumerable && entry.Key.PropertyType.Name != typeof(System.Linq.IGrouping<,>).Name)
                            entry.Value.PropertyType.BuildRetrieveDataPath(_DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);
                        else
                        {
                            if (entry.Value.SourceDataNode is AggregateExpressionDataNode &&
                                entry.Value.SourceDataNode.ParentDataNode != dataRetrieveNode.DataNode &&
                                entry.Value.SourceDataNode.RealParentDataNode == dataRetrieveNode.DataNode &&
                                SelectionDataNodesAsBranch.Contains(entry.Value.SourceDataNode.ParentDataNode))
                            {
                                BuildRetrieveDataPath(new DataRetrieveNode(entry.Value.SourceDataNode.ParentDataNode, DataRetrieveNode.GetDataRetrieveNode(entry.Value.SourceDataNode.ParentDataNode.ParentDataNode, _DataRetrievePath), _DataNodeRowIndices, _DataRetrievePath), _DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);
                            }
                        }
                    }
                }
                if (GroupingMetaData != null && GroupingMetaData.GroupedDataRetrieve.RootDataNode.Type == DataNode.DataNodeType.Object)
                    GroupingMetaData.GroupedDataRetrieve.BuildRetrieveDataPath(_DataNodeRowIndices, _DataRetrievePath, unAssignedNodes);

                #endregion

                if (_DataRetrievePath.Count != 0)
                {
                    int nextColumnIndexInExtensionRow = 0;
                    BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, _DataNodeRowIndices, _DataRetrievePath);
                    ExtensionRowColumnsCount = nextColumnIndexInExtensionRow;
                }
            }
        }


        /// <exclude>Excluded</exclude>
        Dictionary<DataNode, int> _DataNodeRowIndices = new Dictionary<DataNode, int>();
        /// <summary>
        /// DataNodeRowIndices dictionary keeps the row index in composite row for each DataNode.
        /// DynamicTypeDataRetrieve retrieves data and load them in a collection with composite row.
        /// Composite row is an array with DataTables rows. 
        /// </summary>
        /// <MetaDataID>{28281dec-ec6f-4f1a-8369-ae542c3923da}</MetaDataID>
        public Dictionary<DataNode, int> DataNodeRowIndices
        {
            get
            {
                return _DataNodeRowIndices;
            }
        }



        /// <summary>
        /// PropertiesIndices defines a dictionary which keeps the indices on composite row for each property. 
        /// Each property has two indices the first index used from system to access the row on in composite row 
        /// and the second index used from system to access the cell value on row.
        /// </summary>
        /// <MetaDataID>{5659a6df-03b0-4dcb-af6b-9d616fce0169}</MetaDataID>
        Dictionary<System.Reflection.PropertyInfo, int[]> PropertiesIndices;

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
        public void BuildPropertiesDataIndices(ref int nextColumnIndexInExtensionRow, Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath)
        {

            if (MemberDataNode != null)
            {
                if (MemberDataNode.Type == DataNode.DataNodeType.Object)
                {
                    if (TypeHelper.FindIEnumerable(typeof(ObjectType)) == null)
                    {
                        ConventionTypeRowIndex = dataNodeRowIndices[MemberDataNode];
                        ConventionTypeColumnIndex = MemberDataNode.DataSource.ObjectIndex;
                    }
                    else
                    {
                        ConventionTypeRowIndex = retrieveDataPath.Count;
                        ConventionTypeColumnIndex = nextColumnIndexInExtensionRow++;
                    }
                }
                if (MemberDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    if (MemberDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                        MemberDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                    {
                        ConventionTypeRowIndex = dataNodeRowIndices[MemberDataNode.ParentDataNode.ParentDataNode];
                        ConventionTypeColumnIndex = MemberDataNode.ParentDataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(DataSource.GetColumnName(MemberDataNode));
                    }
                    else
                    {
                        ConventionTypeRowIndex = dataNodeRowIndices[MemberDataNode.ParentDataNode];
                        ConventionTypeColumnIndex = MemberDataNode.ParentDataNode.DataSource.GetColumnIndex(MemberDataNode);
                    }
                }
                if (MemberDataNode is AggregateExpressionDataNode)
                {
                    ///TODO  Test Case
                    if (MemberDataNode.ParentDataNode.Type == DataNode.DataNodeType.Group && (MemberDataNode as GroupDataNode).GroupKeyDataNodes.Count == 0)
                    {
                        ConventionTypeRowIndex = dataNodeRowIndices[MemberDataNode.ParentDataNode];
                        ConventionTypeColumnIndex = MemberDataNode.ParentDataNode.DataSource.GetColumnIndex(MemberDataNode);
                    }
                }
            }

            if (_Properties != null)
            {
                foreach (KeyValuePair<System.Reflection.PropertyInfo, DynamicTypeProperty> entry in _Properties)
                {
                    System.Reflection.PropertyInfo propertyInfo = entry.Key;
                    if (PropertiesIndices != null && PropertiesIndices.ContainsKey(propertyInfo))
                        continue;

                    //In case where the property type is collection or Linq Anonymous Type then system allocate place in extension row. 
                    bool propertyValueInExtensionRow = entry.Value.PropertyTypeIsDynamic || entry.Value.PropertyTypeIsEnumerable;
                    if (PropertiesIndices == null)
                        PropertiesIndices = new Dictionary<System.Reflection.PropertyInfo, int[]>();

                    DataNode propertySourceDataNode = entry.Value.SourceDataNode;
                    //DerivedDataNode derivedDataNode = null;
                    //if (propertySourceDataNode is DerivedDataNode)
                    //{
                    //    derivedDataNode = propertySourceDataNode as DerivedDataNode;
                    //    propertySourceDataNode = derivedDataNode.OrgDataNode;
                    //}


                    if (DerivedDataNode.GetOrgDataNode(propertySourceDataNode).Type == DataNode.DataNodeType.Object)
                    {
                        if (propertyValueInExtensionRow)
                        {
                            PropertiesIndices[propertyInfo] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                            entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                            if (entry.Value.PropertyType != null && !entry.Value.PropertyTypeIsEnumerable)
                                entry.Value.PropertyType.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                        }
                        else
                        {
                            PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[propertySourceDataNode], propertySourceDataNode.DataSource.ObjectIndex };
                            entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                        }
                    }
                    else if (DerivedDataNode.GetOrgDataNode(propertySourceDataNode).Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (RootDataNode.Type == DataNode.DataNodeType.Group && (RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(propertySourceDataNode))
                        //||propertySourceDataNode is DerivedDataNode &&(RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains((propertySourceDataNode as DerivedDataNode).OrgDataNode)))
                        {
                            //group key
                            //if (entry.Value.SourceDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                            //    entry.Value.SourceDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                            //{
                            //DateTime member
                            if (propertySourceDataNode is DerivedDataNode)
                                propertySourceDataNode = (propertySourceDataNode as DerivedDataNode).OrgDataNode;

                            PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[RootDataNode], RootDataNode.DataSource.GetColumnIndex(propertySourceDataNode) };
                            entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                            //}
                            //else
                            //    PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[RootDataNode], RootDataNode.DataSource.DataTable.Columns.IndexOf(entry.Value.SourceDataNode.ParentDataNode.Alias + "_" + entry.Value.SourceDataNode.Name) };
                        }
                        else
                        {
                            string columnPrefix = "";
                            //Class member
                            if (propertySourceDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                propertySourceDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                            {

                                //if (!string.IsNullOrEmpty(entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.ValueTypePathDiscription))
                                //    columnPrefix = entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.ValueTypePathDiscription + "_";
                                //if (entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.DataLoadedInParentDataSource)
                                //    columnPrefix = entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.Alias + "_" + columnPrefix;
                                //int propertyValueIndex = -1;
                                //if (entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.DataTable != null)
                                //    propertyValueIndex = entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(columnPrefix + entry.Value.SourceDataNode.ParentDataNode.Name + "_" + entry.Value.SourceDataNode.Name);
                                int propertyValueIndex = propertySourceDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(propertySourceDataNode));
                                // propertyValueIndex = entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.GetColunmIndex(entry.Value.SourceDataNode);
                                //DateTime member
                                PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[propertySourceDataNode.ParentDataNode.ParentDataNode], propertyValueIndex };
                                entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                            }
                            else if (propertySourceDataNode.ParentDataNode.Type == DataNode.DataNodeType.Key)
                            {

                                //if (!string.IsNullOrEmpty(entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.ValueTypePathDiscription))
                                //    columnPrefix = entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.ValueTypePathDiscription + "_";
                                //if (entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.DataLoadedInParentDataSource)
                                //    columnPrefix = entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.Alias + "_" + columnPrefix;
                                //int propertyValueIndex = -1;
                                //if (entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.DataTable != null)
                                //    propertyValueIndex = entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(columnPrefix + entry.Value.SourceDataNode.ParentDataNode.Name + "_" + entry.Value.SourceDataNode.Name);
                                int propertyValueIndex = propertySourceDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(propertySourceDataNode));
                                // propertyValueIndex = entry.Value.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.GetColunmIndex(entry.Value.SourceDataNode);
                                //DateTime member
                                PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[propertySourceDataNode.ParentDataNode.ParentDataNode], propertyValueIndex };
                                entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                            }
                            else
                            {

                                //if (!string.IsNullOrEmpty(entry.Value.SourceDataNode.ParentDataNode.ValueTypePathDiscription))
                                //    columnPrefix = entry.Value.SourceDataNode.ParentDataNode.ValueTypePathDiscription + "_";

                                //int propertyValueIndex = -1;
                                //if (entry.Value.SourceDataNode.ParentDataNode.DataSource.DataTable != null)
                                //    propertyValueIndex = entry.Value.SourceDataNode.ParentDataNode.DataSource.DataTable.Columns.IndexOf(columnPrefix + entry.Value.SourceDataNode.Alias);
                                int propertyValueIndex = propertySourceDataNode.ParentDataNode.DataSource.GetColumnIndex(DerivedDataNode.GetOrgDataNode(propertySourceDataNode));
                                PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[propertySourceDataNode.ParentDataNode], propertyValueIndex };
                                entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                            }
                        }
                    }
                    if (propertySourceDataNode is AggregateExpressionDataNode)
                    {
                        PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[propertySourceDataNode.ParentDataNode], propertySourceDataNode.ParentDataNode.DataSource.GetColumnIndex(propertySourceDataNode) };//.DataTable.Columns.IndexOf(entry.Value.SourceDataNode.Alias) };
                    }
                    if (DerivedDataNode.GetOrgDataNode(propertySourceDataNode).Type == DataNode.DataNodeType.Key)
                    {

                        if (entry.Key.PropertyType.Name.IndexOf("<>f__AnonymousType") == 0)
                        {
                            PropertiesIndices[propertyInfo] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                            entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                            if (entry.Value.PropertyType != null && !entry.Value.PropertyTypeIsEnumerable)
                                entry.Value.PropertyType.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                        }
                        else
                        {
                            DataNode groupKeyDataNode = (propertySourceDataNode.ParentDataNode as GroupDataNode).GroupKeyDataNodes[0];
                            if (groupKeyDataNode.Type == DataNode.DataNodeType.Object)
                            {
                                if (propertyValueInExtensionRow)
                                {
                                    PropertiesIndices[propertyInfo] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                                    entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                                    if (entry.Value.PropertyType != null && !entry.Value.PropertyTypeIsEnumerable)
                                        entry.Value.PropertyType.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                                }
                                else
                                {
                                    PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[groupKeyDataNode], groupKeyDataNode.DataSource.ObjectIndex };
                                    entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                                }
                            }
                            else if (groupKeyDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            {
                                if (RootDataNode.Type == DataNode.DataNodeType.Group && (RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(groupKeyDataNode))
                                {
                                    PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[RootDataNode], RootDataNode.DataSource.GetColumnIndex(groupKeyDataNode) };
                                    entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                                }
                                else
                                {
                                    string columnPrefix = "";
                                    //Class member
                                    if (groupKeyDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                        groupKeyDataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                                    {


                                        int propertyValueIndex = groupKeyDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                                        PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[groupKeyDataNode.ParentDataNode.ParentDataNode], propertyValueIndex };
                                        entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                                    }
                                    else
                                    {
                                        int propertyValueIndex = groupKeyDataNode.ParentDataNode.DataSource.GetColumnIndex(groupKeyDataNode);
                                        PropertiesIndices[propertyInfo] = new int[2] { dataNodeRowIndices[groupKeyDataNode.ParentDataNode], propertyValueIndex };
                                        entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                                    }
                                }
                            }
                        }
                    }
                    if (DerivedDataNode.GetOrgDataNode(propertySourceDataNode).Type == DataNode.DataNodeType.Group)
                    {
                        PropertiesIndices[propertyInfo] = new int[2] { retrieveDataPath.Count, nextColumnIndexInExtensionRow++ };
                        entry.Value.PropertyIndices = PropertiesIndices[propertyInfo];
                        if (entry.Value.PropertyType != null && !entry.Value.PropertyTypeIsEnumerable && propertyInfo.PropertyType.Name != typeof(System.Linq.IGrouping<,>).Name)
                            entry.Value.PropertyType.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                    }
                }
            }
            if (GroupingMetaData != null)
            {
                if (GroupingMetaData.GroupedDataRetrieve.RootDataNode.Type == DataNode.DataNodeType.Object)
                    GroupingMetaData.GroupedDataRetrieve.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
                ConventionTypeRowIndex = retrieveDataPath.Count;
                ConventionTypeColumnIndex = nextColumnIndexInExtensionRow++;
                GroupingMetaData.GroupingResultRowIndex = ConventionTypeRowIndex;
                GroupingMetaData.GroupingResultColumnIndex = ConventionTypeColumnIndex;
                GroupingMetaData.BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, dataNodeRowIndices, retrieveDataPath);
            }
        }


        /// <summary>
        /// CtorParametersIndices defines a dictionary which keeps the indices on composite row for each paremeter of dynamic type costactor. 
        /// Each constactor parameter has two indices the first index used from system to access the row on in composite row 
        /// and the second index used from system to access the cell value on row.
        /// </summary>
        /// <MetaDataID>{6f277176-4632-4be8-be5b-390ae290c631}</MetaDataID>
        internal Dictionary<int, int[]> CtorParametersIndicesOld
        {
            get
            {
                if (_CtorParametersIndices != null)
                    return _CtorParametersIndices;

                _CtorParametersIndices = new Dictionary<int, int[]>();
                if (typeof(ObjectType).Name.IndexOf("<>f__AnonymousType") == 0)
                {
                    System.Reflection.ParameterInfo[] parameters = typeof(ObjectType).GetConstructors()[0].GetParameters();
                    int i = 0;
                    foreach (System.Reflection.ParameterInfo paramInfo in parameters)
                    {
                        System.Reflection.PropertyInfo property = typeof(ObjectType).GetProperty(paramInfo.Name);
                        if (!Properties[property].IsLocalScopeValue)
                            _CtorParametersIndices[i] = PropertiesIndices[property];
                        else
                        {
                            _CtorParametersIndices[i] = new int[2] { -1, -1 };
                            _CtorParametersLocalScopeValues[i] = Properties[property].LocalScopeValue;
                        }

                        i++;
                    }
                }
                else
                {
                }

                return _CtorParametersIndices;
            }

        }







        #endregion

        #region Retrieve Data

        /// <summary>
        /// Collect the data from data tree with start point the rows of parameter rows 
        /// and transform them in form which needed from linq type (DynamicType).  
        /// </summary>
        /// <param name="rows">
        /// This parameter defines the rows as start point for the data collection from the data tree
        /// </param>
        /// <param name="parentsDataNodesRows">
        /// parentsDataNodeRows dictionary keeps the data node row pairs. 
        /// This pairs is useful when the RetrieveDataPath of (dynamic type data retriever) property has Data nodes 
        /// which are parents (in data tree) of rows Data Node (parameter rows defines this rows). 
        /// </param>
        /// <returns>
        /// returns a collection with composite rows
        /// </returns>
        /// <MetaDataID>{2b443b22-2fdb-4e0f-ac9b-7ac22c558d09}</MetaDataID>
        List<System.Data.DataRow[]> GetAllDataAsSingleTable(System.Data.DataRow[] rows, DataNodesRows parentsdDataNodesRows)
        {
            List<System.Data.DataRow[]> compositeRows = new List<System.Data.DataRow[]>();

            BuildDataRetrieveMetadata();

            if (DataRetrievePath.Count == 0)
                return null;

            System.Data.DataRow[] compositeRow = null;
            if (ExtensionRowColumnsCount > 0)
            {
                compositeRow = new System.Data.DataRow[DataRetrievePath.Count + 1];
                compositeRow[DataRetrievePath.Count] = ExtensionRow;
            }
            else
                compositeRow = new System.Data.DataRow[DataRetrievePath.Count];


            if (rows == null)
            {
                LinkedListNode<DataRetrieveNode> dataRetrieveNode = DataRetrievePath.First;
                if (dataRetrieveNode.Value.DataNode.Type == DataNode.DataNodeType.Group && GroupingMetaData != null)
                    dataRetrieveNode = dataRetrieveNode.Next;
                if (dataRetrieveNode.Value.DataNode.DataSource.DataTable != null)
                {
                    rows = new System.Data.DataRow[dataRetrieveNode.Value.DataNode.DataSource.DataTable.Rows.Count];
                    int rowIndex = 0;
                    foreach (System.Data.DataRow row in dataRetrieveNode.Value.DataNode.DataSource.DataTable.Rows)
                    {
                        if (dataRetrieveNode.Value.DataNode.SearchCondition != null && dataRetrieveNode.Value.DataNode.SearchCondition.IsRemovedRow(row, dataRetrieveNode.Value.DataNode.DataSource))
                            continue;
                        compositeRow[dataRetrieveNode.Value.DataRowIndex] = row;
                        rows[rowIndex++] = row;
                        RetrieveData(compositeRow, dataRetrieveNode.Next, compositeRows);
                        for (int i = 0; i < DataRetrievePath.Count; i++)
                            compositeRow[i] = null;
                    }
                    LoadDynamicTypePropertiesValues(compositeRows, parentsdDataNodesRows);
                    if (GroupingMetaData != null)
                        GroupingMetaData.GroupCompositeRows(ref compositeRows, parentsdDataNodesRows, _DataNodeRowIndices, out GroupedData);
                }
            }
            else
            {
                LinkedListNode<DataRetrieveNode> dataRetrieveNode = DataRetrievePath.First;

                #region retreive parent dataNodes data
                DataNode sourceDataNode = RootDataNode;
                if (Properties == null)
                    sourceDataNode = MemberDataNode;

                while (RootDataNode != dataRetrieveNode.Value.DataNode && RootDataNode.IsSameOrParentDataNode(dataRetrieveNode.Value.DataNode) && parentsdDataNodesRows.ContainsKey(dataRetrieveNode.Value.DataNode))
                {
                    compositeRow[dataRetrieveNode.Value.DataRowIndex] = parentsdDataNodesRows[dataRetrieveNode.Value.DataNode];
                    dataRetrieveNode = dataRetrieveNode.Next;
                }

                #endregion

                if (rows.Length > 0 && rows[0].Table != dataRetrieveNode.Value.DataNode.DataSource.DataTable)
                    throw new System.ArgumentException("Wrong rows");

                // int rowRemoveIndex = dataRetrieveNode.Value.DataNode.DataSource.RowRemoveIndex;
                DataRetrieveNode[] dataRetrieveNodes = new DataRetrieveNode[DataRetrievePath.Count];
                DataRetrievePath.CopyTo(dataRetrieveNodes, 0);
                SearchCondition searchCondition = null;
                foreach (var dataNodeSearchCodition in sourceDataNode.SearchConditions)
                {
                    if (FilterDataCondition != null && dataNodeSearchCodition != null && dataNodeSearchCodition.ToString() == FilterDataCondition.ToString())
                    {
                        searchCondition = dataNodeSearchCodition;
                        //if (searchCondition != null)
                        //    rowRemoveIndex = dataRetrieveNode.Value.DataNode.DataSource.GetRowRemoveIndex(searchCondition);
                        break;
                    }
                }
                int i = 0;
                foreach (System.Data.DataRow row in rows)
                {
                    if (searchCondition != null && searchCondition.IsRemovedRow(row, dataRetrieveNode.Value.DataNode.DataSource))
                        continue;
                    //if (dataRetrieveNode.Value.DataNode.SearchCondition != null && dataRetrieveNode.Value.DataNode.SearchCondition.IsRemovedRow(row, rowRemoveIndex))
                    //  continue;
                    compositeRow[dataRetrieveNode.Value.DataRowIndex] = row;
                    RetrieveData(compositeRow, dataRetrieveNode.Next, compositeRows);
                    for (i = 0; i < DataRetrievePath.Count; i++)
                    {
                        if (!(RootDataNode != dataRetrieveNodes[i].DataNode && RootDataNode.IsSameOrParentDataNode(dataRetrieveNodes[i].DataNode)))
                            compositeRow[i] = null;
                    }
                }
                LoadDynamicTypePropertiesValues(compositeRows, parentsdDataNodesRows);
                if (GroupingMetaData != null)
                    GroupingMetaData.GroupCompositeRows(ref compositeRows, parentsdDataNodesRows, _DataNodeRowIndices, out GroupedData);

            }
            return compositeRows;
        }



        /// <summary>
        /// This method sets the extension data row  for each composite row of compositeRows collection.
        /// Extension row keeps the data which are built dynamically.
        /// Actually keeps the values of dynamic type properties.
        /// </summary>
        /// <param name="compositeRows">
        /// This parameter defines the collection of composite rows 
        /// where the method sets the extension row data .
        /// </param>
        /// <param name="dataNodesRows">
        ///  This parameter define a dictionary which keeps the DataNode-DataRow pairs.
        /// </param>
        /// <MetaDataID>{2864f129-390f-4d26-94f3-58ad2b36734a}</MetaDataID>
        private void LoadDynamicTypePropertiesValues(List<System.Data.DataRow[]> compositeRows, DataNodesRows dataNodesRows)
        {
            if (_Properties != null)
            {
                bool retrieveData = false;

                foreach (KeyValuePair<System.Reflection.PropertyInfo, DynamicTypeProperty> entry in _Properties)
                {
                    if ((entry.Value.PropertyTypeIsDynamic && !entry.Value.PropertyTypeIsEnumerable) ||
                        entry.Value.PropertyTypeIsEnumerable)
                    {
                        retrieveData = true;
                    }
                }
                if (retrieveData)
                {
                    foreach (System.Data.DataRow[] compositeRow in new List<System.Data.DataRow[]>(compositeRows))
                    {
                        //Dictionary<DataNode, System.Data.DataRow> dataNodesRows = new Dictionary<DataNode, System.Data.DataRow>();
                        //foreach (DataNode dataNode in DataNodeRowIndices.Keys)
                        //    dataNodesRows.Add(dataNode, compositeRow[DataNodeRowIndices[dataNode]]);

                        LoadDynamicTypePropertiesValues(compositeRow, dataNodesRows);
                    }
                }

                #region Παλιός κώδικας μην το σβήσεις πριν από όλα τα test cases
                //foreach (KeyValuePair<System.Reflection.PropertyInfo, DynamicTypeProperty> entry in _Properties)
                //{
                //    if (entry.Value.PropertyTypeIsDynamic && !entry.Value.PropertyTypeIsEnumerable)
                //    {
                //        if (entry.Value.PropertyInfo.PropertyType.Name == typeof(System.Linq.IGrouping<,>).Name)// RootDataNode && RootDataNode.Type == DataNode.DataNodeType.Group)
                //        {
                //            IDynamicTypeDataRetrieve GroupedDataEnumerator = entry.Value.PropertyType.GetRelatedDataEnumerator(MasterRow, parentsdDataNodesRows) as IDynamicTypeDataRetrieve;
                //            foreach (System.Data.DataRow[] compositeRow in new List<System.Data.DataRow[]>(compositeRows))
                //            {
                //                System.Data.DataRow groupAggregationRow = compositeRow[DataNodeRowIndices[RootDataNode]];
                //                // object key = entry.Value.PropertyType.GroupingMetaData.FastKeyConstructorInvoke.Invoke(null, _params);
                //                object groupData = GroupedDataEnumerator.GetGroupedData(groupAggregationRow);
                //                if (groupData == null)
                //                    compositeRows.Remove(compositeRow); //Temporary
                //                else
                //                    compositeRow[PropertiesIndices[entry.Key][0]][PropertiesIndices[entry.Key][1]] = groupData;
                //            }
                //            continue;
                //        }
                //        if (entry.Value.SourceDataNode.Type == DataNode.DataNodeType.Key && GroupingMetaData != null)
                //            continue;

                //        int dynamicTypeIndex = PropertiesIndices[entry.Key][1];
                //        foreach (System.Data.DataRow[] compositeRow in compositeRows)
                //            entry.Value.PropertyType.RetrieveDynamicTypeInstance(dynamicTypeIndex, DataNodeRowIndices, compositeRow);
                //    }
                //    else if (/*entry.Value.PropertyTypeIsDynamic &&*/ entry.Value.PropertyTypeIsEnumerable)
                //    {
                //        if (entry.Value.SourceDataNode == RootDataNode && RootDataNode.Type == DataNode.DataNodeType.Group)
                //            continue;
                //        // retrieves collection of objects 
                //        foreach (System.Data.DataRow[] compositeRow in compositeRows)
                //        {
                //            Dictionary<DataNode, System.Data.DataRow> dataNodesRows = new Dictionary<DataNode, System.Data.DataRow>();
                //            foreach (DataNode dataNode in DataNodeRowIndices.Keys)
                //                dataNodesRows.Add(dataNode, compositeRow[DataNodeRowIndices[dataNode]]);
                //            {
                //                if(entry.Value.SourceDataNode == RootDataNode&& RootDataNode.Recursive)
                //                    RetrieveCollectionPropertiesData(compositeRow, entry.Value, dataNodesRows, compositeRow[DataNodeRowIndices[entry.Value.SourceDataNode]]);// rows);
                //                else
                //                    RetrieveCollectionPropertiesData(compositeRow, entry.Value, dataNodesRows, compositeRow[DataNodeRowIndices[entry.Value.SourceDataNode.ParentDataNode]]);// rows);
                //            }
                //        }
                //    }
                //    else if (entry.Value.SourceDataNode.ParentDataNode != RootDataNode &&
                //                entry.Value.SourceDataNode.RealParentDataNode == RootDataNode &&
                //                entry.Value.SourceDataNode is AggregateExpressionDataNode &&
                //                SelectionDataNodesAsBranch.Contains(entry.Value.SourceDataNode.ParentDataNode))
                //    {
                //        foreach (System.Data.DataRow[] compositeRow in compositeRows)
                //        {
                //            ICollection<System.Data.DataRow> rows = RetrieveRelatedRows(compositeRow, entry.Value.SourceDataNode.ParentDataNode, DataNodeRowIndices);
                //        }
                //    }
                //}
                #endregion
            }
        }

        /// <MetaDataID>{65be47c7-6e4f-4645-aaa1-1cbc7212fd51}</MetaDataID>
        private void LoadDynamicTypePropertiesValues(System.Data.DataRow[] compositeRow, DataNodesRows parentsdDataNodesRows)
        {
            DataNodesRows dataNodesRows = new DataNodesRows(compositeRow, DataNodeRowIndices, parentsdDataNodesRows);
            foreach (KeyValuePair<System.Reflection.PropertyInfo, DynamicTypeProperty> entry in _Properties)
            {
                entry.Value.LoadPropertyValue(compositeRow, dataNodesRows);
                //if (entry.Value.PropertyTypeIsDynamic && !entry.Value.PropertyTypeIsEnumerable)
                //{
                //    if (entry.Value.PropertyInfo.PropertyType.Name == typeof(System.Linq.IGrouping<,>).Name)// RootDataNode && RootDataNode.Type == DataNode.DataNodeType.Group)
                //    {
                //        #region Retrieve IGroup instance
                //        entry.Value.LoadPropertyValue(compositeRow, dataNodesRows);
                //        if (compositeRow[PropertiesIndices[entry.Key][0]][PropertiesIndices[entry.Key][1]] == null)
                //            compositeRows.Remove(compositeRow); //Temporary

                //        //System.Data.DataRow masterRow = null;
                //        //if (RootDataNode == entry.Value.PropertyType.RootDataNode)
                //        //{
                //        //    masterRow = dataNodesRows[entry.Value.PropertyType.RootDataNode.ParentDataNode];
                //        //    masterRow = MasterRow;                  //in case where grouping type has the same RootDataNode as hosting type the grouped data  
                //        //    // is detail of Master row.
                //        //}
                //        //if (RootDataNode == entry.Value.SourceDataNode.ParentDataNode)
                //        //    masterRow = dataNodesRows[RootDataNode]; //in case where grouping type RootDataNode parent  is hosting type RootDataNode
                //        ////the master row contained in compositeRow 

                //        //IDynamicTypeDataRetrieve GroupedDataEnumerator = entry.Value.PropertyType.GetRelatedDataEnumerator(masterRow, dataNodesRows) as IDynamicTypeDataRetrieve;
                //        //System.Data.DataRow groupRow = dataNodesRows[entry.Value.SourceDataNode];
                //        //object groupInstance = GroupedDataEnumerator.GetGroupInstance(groupRow);
                //        //if (groupInstance == null)
                //        //    compositeRows.Remove(compositeRow); //Temporary
                //        //else
                //        //    compositeRow[PropertiesIndices[entry.Key][0]][PropertiesIndices[entry.Key][1]] = groupInstance;

                //        #endregion

                //    }
                //    else if (entry.Value.SourceDataNode.Type != DataNode.DataNodeType.Key || GroupingMetaData == null)
                //    {
                //        //int dynamicTypeIndex = PropertiesIndices[entry.Key][1];
                //        //entry.Value.PropertyType.LoadInstance(dynamicTypeIndex, DataNodeRowIndices, compositeRow);
                //        entry.Value.LoadPropertyValue(compositeRow, dataNodesRows);
                //    }
                //}
                //else if (/*entry.Value.PropertyTypeIsDynamic &&*/ entry.Value.PropertyTypeIsEnumerable)
                //{
                //    if (entry.Value.SourceDataNode == RootDataNode && RootDataNode.Type == DataNode.DataNodeType.Group)
                //        continue;
                //    // retrieves collection of objects 

                //    entry.Value.LoadPropertyValue(compositeRow, dataNodesRows);

                //    //if (entry.Value.SourceDataNode == RootDataNode && RootDataNode.Recursive)
                //    //    compositeRow[PropertiesIndices[entry.Value.PropertyInfo][0]][PropertiesIndices[entry.Value.PropertyInfo][1]] = entry.Value.GetRelatedDataCollection(compositeRow[DataNodeRowIndices[entry.Value.SourceDataNode]], dataNodesRows);
                //    //    //LoadPropertyCollection(compositeRow, entry.Value, dataNodesRows, compositeRow[DataNodeRowIndices[entry.Value.SourceDataNode]]);// rows);
                //    //else
                //    //    compositeRow[PropertiesIndices[entry.Value.PropertyInfo][0]][PropertiesIndices[entry.Value.PropertyInfo][1]] = entry.Value.GetRelatedDataCollection(compositeRow[DataNodeRowIndices[entry.Value.SourceDataNode.ParentDataNode]], dataNodesRows);
                //    //    //LoadPropertyCollection(compositeRow, entry.Value, dataNodesRows, compositeRow[DataNodeRowIndices[entry.Value.SourceDataNode.ParentDataNode]]);// rows);

                //}
                //else if (entry.Value.SourceDataNode.ParentDataNode != RootDataNode &&
                //            entry.Value.SourceDataNode.RealParentDataNode == RootDataNode &&
                //            entry.Value.SourceDataNode is AggregateExpressionDataNode &&
                //            SelectionDataNodesAsBranch.Contains(entry.Value.SourceDataNode.ParentDataNode))
                //{
                //    ICollection<System.Data.DataRow> rows = RetrieveRelatedRows(compositeRow, entry.Value.SourceDataNode.ParentDataNode, DataNodeRowIndices);
                //}
            }


        }

        /// <summary>
        /// This method return the new dynamic type(anonymous type) object.
        /// The values of new object retrieved from composite row
        /// </summary>
        /// <param name="compositeRow">
        /// This parameter defines the composite row.
        /// The composite row used from method to retrieve the values dynamic type (anonymous type) object and then load the just created object.
        /// </param>
        /// <param name="dataNodesRows">
        ///  This parameter define a dictionary which keeps the DataNode-DataRow pairs.
        /// </param>

        /// <MetaDataID>{f5f6eff0-1d19-4899-82be-87c519ee343c}</MetaDataID>
        object IDynamicTypeDataRetrieve.InstantiateObject(System.Data.DataRow[] compositeRow, DataNodesRows dataNodesRows)
        {
            if (Properties == null)
                return compositeRow[ConventionTypeRowIndex][ConventionTypeColumnIndex];
            else
            {
                LoadDynamicTypePropertiesValues(compositeRow, dataNodesRows);
                //foreach (KeyValuePair<System.Reflection.PropertyInfo, DynamicTypeProperty> entry in _Properties)
                //{
                //    entry.Value.LoadPropertyValue(compositeRow, dataNodesRows);
                //    //if (entry.Value.PropertyTypeIsDynamic && !entry.Value.PropertyTypeIsEnumerable)
                //    //    entry.Value.LoadPropertyValue(compositeRow,dataNodesRows);

                //    //if (entry.Value.PropertyTypeIsDynamic && entry.Value.PropertyTypeIsEnumerable)
                //    //{
                //    //    entry.Value.LoadPropertyValue(compositeRow, dataNodesRows);
                //    //    //ICollection<System.Data.DataRow> rows = RetrieveRelatedRows(entry.Value.SourceDataNode.ParentDataNode, entry.Value.SourceDataNode, dataNodesRows);
                //    //    ////Dictionary<DataNode, System.Data.DataRow> parentsdDataNodesRows = new Dictionary<DataNode, System.Data.DataRow>();
                //    //    ////foreach (DataNode dataNode in dataNodeRowIndices.Keys)
                //    //    ////    parentsdDataNodesRows.Add(dataNode, compositeRow[dataNodeRowIndices[dataNode]]);

                //    //    ////DataNodesRows parentsdDataNodesRows = new DataNodesRows(compositeRow, dataNodeRowIndices);
                //    //    //compositeRow[PropertiesIndices[entry.Value.PropertyInfo][0]][PropertiesIndices[entry.Value.PropertyInfo][1]] = entry.Value.GetRelatedDataCollection(dataNodesRows[entry.Value.SourceDataNode.ParentDataNode], dataNodesRows);
                //    //    ////LoadPropertyCollection(compositeRow, entry.Value, parentsdDataNodesRows, compositeRow[dataNodeRowIndices[entry.Value.SourceDataNode.ParentDataNode]]);//rows);
                //    //}
                //}
                CurrentCompositeRow = new CompositeRowData(compositeRow);
                return Current;
            }
            //compositeRow[compositeRow.Length - 1][dynamicTypeIndex] = Current;
        }


        ///<summary>
        ///This operation returns an enumerator on object collection.
        ///The objects of object collection created from the data of rows. 
        ///</summary>
        ///<param name="rows">
        ///The rows parameter defines the data which used from the dynamic type retriever 
        ///to create the collection with dynamic type objects.  
        ///</param>
        /// <param name="parentsDataNodesRows">
        /// parentsDataNodeRows dictionary keeps the data node row pairs. 
        /// This pairs is useful when the RetrieveDataPath of (dynamic type data retriever) property has Data nodes 
        /// which are parents (in data tree) of rows Data Node (parameter rows defines this rows). 
        /// </param>
        /// <MetaDataID>{8e77c087-2613-484f-8870-020cd4fbab96}</MetaDataID>
        public System.Collections.IEnumerator GetRelatedDataEnumerator(System.Data.DataRow row, DataNodesRows parentsdDataNodesRows)
        {
            MasterRow = row;
            RealEnum = null;
            //ICollection<System.Data.DataRow> rows = RootDataNode.RealParentDataNode.DataSource.GetRelatedRows(row, RootDataNode);
            ICollection<System.Data.DataRow> rows = null;
            if (RootDataNode.Type == DataNode.DataNodeType.Group && GroupingMetaData != null)
            {
                DataNode detailsDataNode = (RootDataNode as GroupDataNode).GroupByDataNodeRoot;
                //foreach (DataNode groupingDataNode in detailsDataNode.SubDataNodes)
                //{
                //    if (groupingDataNode.Type != DataNode.DataNodeType.Key)
                //    {
                //        detailsDataNode = groupingDataNode;
                //        break;
                //    }
                //}
                if ((RootDataNode as GroupDataNode).GroupByDataNodeRoot == RootDataNode.RealParentDataNode || RootDataNode.RealParentDataNode == null)
                {
                    System.Data.DataRow[] tableRows = new System.Data.DataRow[(RootDataNode as GroupDataNode).GroupByDataNodeRoot.DataSource.DataTable.Rows.Count];
                    (RootDataNode as GroupDataNode).GroupByDataNodeRoot.DataSource.DataTable.Rows.CopyTo(tableRows, 0);
                    rows = tableRows;
                }
                else
                    rows = RootDataNode.RealParentDataNode.DataSource.GetRelatedRows(row, detailsDataNode);
            }
            else
            {
                if (row.Table == RootDataNode.DataSource.DataTable) //Recursive load
                    rows = RootDataNode.DataSource.GetRelatedRows(row, RootDataNode);
                else
                    rows = RootDataNode.RealParentDataNode.DataSource.GetRelatedRows(row, RootDataNode);

            }
            System.Data.DataRow[] arrayOfRows = null;
            if (rows == null)
            {
                arrayOfRows = new System.Data.DataRow[0];
            }
            else
            {
                arrayOfRows = new System.Data.DataRow[rows.Count];
                rows.CopyTo(arrayOfRows, 0);
            }


            CompositeRows = GetAllDataAsSingleTable(arrayOfRows, parentsdDataNodesRows);
            if (Properties != null)
            {
                #region Gets constructor metadata
                if (ConstructorInfo == null)
                {
                    ConstructorInfo = typeof(ObjectType).GetConstructors()[0];
                    FastConstructorInvoke = AccessorBuilder.GetConstructorInvoker(ConstructorInfo);
                    Parameters = ConstructorInfo.GetParameters();
                }
                #endregion
            }
            return new DynamicTypeDataRetrieve<ObjectType>(this);

        }



        /// <MetaDataID>{42756539-a868-45a5-9c67-80a4d53ae2d2}</MetaDataID>
        public void Reset()
        {
            if (CompositeRows == null)
            {
                DataNodesRows parentsdDataNodesRows = new DataNodesRows(null, new Dictionary<DataNode, int>());

                CompositeRows = GetAllDataAsSingleTable(null, parentsdDataNodesRows);
                RealEnum = CompositeRows.GetEnumerator();
            }
            RealEnum = CompositeRows.GetEnumerator();


        }

        /// <MetaDataID>{a020af3b-446f-48c6-b1d0-b90e26b4e7af}</MetaDataID>
        public bool MoveNextOld()
        {
            if (CompositeRows == null)
                Reset();
            bool retval = RealEnum.MoveNext();

            // bool retvalA = QueryResult.DataLoader.MoveNext();

            RetrieveCurrent = true;
            _Current = default(ObjectType);
            if (retval)
            {
                if (RealEnum.Current is System.Data.DataRow[])
                    CurrentCompositeRow = new CompositeRowData(RealEnum.Current as System.Data.DataRow[]);
                else
                {
                    if (CurrentCompositeRow == null)
                    {
                        CurrentCompositeRow = new CompositeRowData(new System.Data.DataRow[1]);
                        System.Data.DataTable temporaryTable = new System.Data.DataTable(RootDataNode.Alias);
                        temporaryTable.Columns.Add("Key", typeof(object));
                        CurrentCompositeRow.CompositeRow[0] = temporaryTable.NewRow();
                        ConventionTypeRowIndex = 0;
                        ConventionTypeColumnIndex = 0;
                    }
                    System.Collections.Generic.KeyValuePair<object, List<System.Data.DataRow[]>> entry = (System.Collections.Generic.KeyValuePair<object, List<System.Data.DataRow[]>>)RealEnum.Current;
                    CurrentCompositeRow[0][0] = entry.Key;
                }

            }
            while (retval && Current == null)
            {
                retval = RealEnum.MoveNext();
                if (retval)
                {
                    if (RealEnum.Current is System.Data.DataRow[])
                        CurrentCompositeRow = new CompositeRowData(RealEnum.Current as System.Data.DataRow[]);
                    else
                    {
                        if (CurrentCompositeRow == null)
                        {
                            CurrentCompositeRow = new CompositeRowData(new System.Data.DataRow[1]);
                            System.Data.DataTable temporaryTable = new System.Data.DataTable(RootDataNode.Alias);
                            temporaryTable.Columns.Add("Key", typeof(object));
                            CurrentCompositeRow.CompositeRow[0] = temporaryTable.NewRow();
                            ConventionTypeRowIndex = 0;
                            ConventionTypeColumnIndex = 0;
                        }
                        System.Collections.Generic.KeyValuePair<object, List<System.Data.DataRow[]>> entry = (System.Collections.Generic.KeyValuePair<object, List<System.Data.DataRow[]>>)RealEnum.Current;
                        CurrentCompositeRow[0][0] = entry.Key;
                    }
                }
            }

            return retval;
        }



        /// <summary>
        /// This method retrieves recursively the data from data tree and load on composite row collection.
        /// </summary>
        /// <param name="compositeRow"></param>
        /// This parameter defines the composite row where method load the values when walk on RetrieveDataPath.
        /// <param name="dataRetrieveNode">
        /// This parameter defines the current node of RetrieveDataPath.
        /// </param>
        /// <param name="compositeRows">
        /// This parameter defines a collection of composite rows.
        /// The retrieve method add the composite row to this collection when reach at
        /// the end of RetrieveDataPath.
        /// </param>
        /// <MetaDataID>{6452eece-18d2-4c24-add3-3c42cd7dff27}</MetaDataID>
        public void RetrieveData(System.Data.DataRow[] compositeRow,
              System.Collections.Generic.LinkedListNode<DataRetrieveNode> dataRetrieveNode,
              System.Collections.Generic.List<System.Data.DataRow[]> compositeRows)
        {
            if (dataRetrieveNode == null)
            {
                if (ExtensionRowColumnsCount > 0)
                    compositeRow[DataRetrievePath.Count] = compositeRow[DataRetrievePath.Count].Table.NewRow();
                compositeRows.Add(compositeRow.Clone() as System.Data.DataRow[]);
            }
            else
            {
                ICollection<System.Data.DataRow> rows = null;
                //System.Data.DataRow[] rows = null;
                // Retrieves the related data rows of linkedListNode
                // int rowRemoveIndex = -1;
                DataSource rowsDataSource = null;
                if (GroupingMetaData == null && RootDataNode.Type == DataNode.DataNodeType.Group && (RootDataNode as GroupDataNode).GroupKeyDataNodes.Contains(dataRetrieveNode.Value.DataNode))
                {
                    if (compositeRow[DataNodeRowIndices[RootDataNode]] != null)
                    {
                        rowsDataSource = dataRetrieveNode.Value.DataNode.DataSource;//.RowRemoveIndex;
                        rows = RetrieveRelatedRows(RootDataNode, dataRetrieveNode.Value.DataNode, new DataNodesRows(compositeRow, DataNodeRowIndices));
                    }
                }
                else if (dataRetrieveNode.Previous.Value.DataNode.Type == DataNode.DataNodeType.Group && (dataRetrieveNode.Previous.Value.DataNode as GroupDataNode).GroupKeyDataNodes.Contains(dataRetrieveNode.Value.DataNode))
                {
                    if (compositeRow[DataNodeRowIndices[dataRetrieveNode.Previous.Value.DataNode]] != null)
                    {
                        rowsDataSource = dataRetrieveNode.Value.DataNode.DataSource;//.RowRemoveIndex;
                        rows = RetrieveRelatedRows(dataRetrieveNode.Previous.Value.DataNode, dataRetrieveNode.Value.DataNode, new DataNodesRows(compositeRow, DataNodeRowIndices));
                    }
                }
                else if (RootDataNode.Type == DataNode.DataNodeType.Group && GroupingMetaData != null)
                {
                    if (compositeRow[DataNodeRowIndices[dataRetrieveNode.Value.DataNode.RealParentDataNode]] != null)
                    {
                        rowsDataSource = dataRetrieveNode.Value.DataNode.DataSource;//.RowRemoveIndex;
                        rows = RetrieveRelatedRows(dataRetrieveNode.Value.DataNode.RealParentDataNode, dataRetrieveNode.Value.DataNode, new DataNodesRows(compositeRow, DataNodeRowIndices));
                    }
                }
                //else if (dataRetrieveNode.Value.DataNode.ParentDataNode.Type==DataNode.DataNodeType.Key&& compositeRow[DataNodeRowIndices[dataRetrieveNode.Value.DataNode.ParentDataNode.ParentDataNode]] != null)
                //{
                //    rowsDataSource = dataRetrieveNode.Value.DataNode.DataSource;//.RowRemoveIndex;
                //    rows = RetrieveRelatedRows(dataRetrieveNode.Value.DataNode.ParentDataNode, dataRetrieveNode.Value.DataNode, new DataNodesRows(compositeRow, DataNodeRowIndices));
                //}

                else if (compositeRow[DataNodeRowIndices[dataRetrieveNode.Value.MasterDataNode.DataNode]] != null)
                {
                    rowsDataSource = dataRetrieveNode.Value.DataNode.DataSource;//.RowRemoveIndex;
                    rows = RetrieveRelatedRows(dataRetrieveNode.Value.MasterDataNode.DataNode, dataRetrieveNode.Value.DataNode, new DataNodesRows(compositeRow, DataNodeRowIndices));
                }
                if (rows == null || rows.Count == 0)
                {
                    //TODO να γραφτεί test σενάριο για αυτήν την περίπτωση.
                    //Retrieving data walk end here there aren't rows to continue
                    if (MemberDataNode != null && (ConventionTypeRowIndex == dataRetrieveNode.Value.DataRowIndex || Properties == null))
                        return;
                    compositeRow[dataRetrieveNode.Value.DataRowIndex] = null;
                    if (ExtensionRowColumnsCount > 0)
                        compositeRow[DataRetrievePath.Count] = compositeRow[DataRetrievePath.Count].Table.NewRow();
                    //compositeRows.Add(compositeRow.Clone() as System.Data.DataRow[]);
                    return;
                }
                else
                {
                    //if (SourceTreeNodeSearchCondition.UnInitialized)
                    //    SourceTreeNodeSearchCondition.Value = _SourceCollectionExpression.FilterDataCondition;

                    SearchCondition searchCondition = null;
                    foreach (var dataNodeSearchCodition in dataRetrieveNode.Value.DataNode.SearchConditions)
                    {
                        if (dataNodeSearchCodition != null && FilterDataCondition != null && dataNodeSearchCodition == FilterDataCondition)
                        {
                            searchCondition = dataNodeSearchCodition;
                            break;
                        }
                    }

                    bool retrievingDataWalkEndHere = true;
                    foreach (System.Data.DataRow row in rows)
                    {
                        //if (linkedListNode.Value.DataNode.SearchCondition != null && linkedListNode.Value.DataNode.SearchCondition.IsRemovedRow(row, rowRemoveIndex))
                        //    continue;
                        if (searchCondition != null && searchCondition.IsRemovedRow(row, rowsDataSource))
                            continue;

                        retrievingDataWalkEndHere = false;
                        if (dataRetrieveNode.Value.DataNode.Type == DataNode.DataNodeType.Group && dataRetrieveNode.Value.DataNode == RootDataNode)
                            dataRetrieveNode = dataRetrieveNode.Next;
                        compositeRow[dataRetrieveNode.Value.DataRowIndex] = row;
                        RetrieveData(compositeRow, dataRetrieveNode.Next, compositeRows);
                    }
                    if (retrievingDataWalkEndHere)
                    {
                        //Retrieving data walk end here there aren't rows to continue
                        //TODO να γραφτεί test σενάριο για αυτήν την περίπτωση.
                        if (ExtensionRowColumnsCount > 0)
                            compositeRow[DataRetrievePath.Count] = compositeRow[DataRetrievePath.Count].Table.NewRow();
                        compositeRows.Add(compositeRow.Clone() as System.Data.DataRow[]);
                    }
                }
            }
        }


        ///<summary>
        ///This operation returns an enumerator on object collection.
        ///The objects of object collection created from the data of rows. 
        ///</summary>
        ///<param name="rows">
        ///The rows parameter defines the data which used from the dynamic type retriever 
        ///to create the collection with dynamic type objects.  
        ///</param>
        /// <param name="parentsDataNodesRows">
        /// parentsDataNodeRows dictionary keeps the data node row pairs. 
        /// This pairs is useful when the RetrieveDataPath of (dynamic type data retriever) property has Data nodes 
        /// which are parents (in data tree) of rows Data Node (parameter rows defines this rows). 
        /// </param>
        /// <MetaDataID>{8e77c087-2613-484f-8870-020cd4fbab96}</MetaDataID>
        public object GetRelatedDataCollection(System.Data.DataRow masterRow, DataNodesRows parentsDataNodesRows)
        {
            if (!PropertyTypeIsEnumerable)
                throw new System.Exception("method can load collection only for enumerable properties");


            System.Collections.IList collection = null;
            if (PropertyTypeIsDynamic)//  entry.Value.PropertyType!=null)//  (elementType.Name.IndexOf("<>f__AnonymousType") == 0)
                return PropertyType.GetRelatedDataEnumerator(masterRow, parentsDataNodesRows);
            else
            {

                ICollection<System.Data.DataRow> rows = SourceDataNode.RealParentDataNode.DataSource.GetRelatedRows(masterRow, SourceDataNode);
                //Δεν θα φτάσει ποτέ ο κωδικας εδώ γιατί οι callers αυτής της function με συνθήκες if αποκλύουν αυτό το γεγονός.  
                collection = Activator.CreateInstance(PropertyInfo.PropertyType) as System.Collections.IList;
                //int rowRemoveIndex = dynamicTypeProperty.SourceDataNode.DataSource.RowRemoveIndex;
                if (masterRow != null)
                {

                    foreach (System.Data.DataRow row in rows)
                    {
                        if (SourceDataNode.SearchCondition != null && SourceDataNode.SearchCondition.IsRemovedRow(row, SourceDataNode.DataSource))
                            continue;
                        collection.Add(row[SourceDataNode.DataSource.ObjectIndex]);
                    }
                }
                return collection;
            }


        }

        /// <exclude>Excluded</exclude>
        System.Data.DataRow _ExtensionRow;
        /// <MetaDataID>{ab1c9596-befc-4515-8880-0e1d323e1d26}</MetaDataID>
        /// <summary>
        /// Extension row keeps the data which built dynamically.
        /// Actually keeps the values of dynamic type properties.
        /// </summary>
        private System.Data.DataRow ExtensionRow
        {
            get
            {
                if (_ExtensionRow == null)
                {
                    System.Data.DataTable table = new System.Data.DataTable();
                    for (int i = 0; i != ExtensionRowColumnsCount; i++)
                        table.Columns.Add("column_" + i.ToString(), typeof(object));
                    _ExtensionRow = table.NewRow();
                }
                return _ExtensionRow;
            }
        }


        internal void LoadPropertyValue(System.Data.DataRow[] compositeRow, DataNodesRows dataNodesRows)
        {
            if (PropertyTypeIsDynamic && !PropertyTypeIsEnumerable)
            {
                if (PropertyInfo.PropertyType.Name == typeof(System.Linq.IGrouping<,>).Name)
                {
                    #region Retrieve IGroup instance
                    System.Data.DataRow masterRow = null;
                    masterRow = dataNodesRows[SourceDataNode.ParentDataNode];
                    IDynamicTypeDataRetrieve GroupedDataEnumerator = PropertyType.GetRelatedDataEnumerator(masterRow, dataNodesRows) as IDynamicTypeDataRetrieve;
                    System.Data.DataRow groupRow = dataNodesRows[SourceDataNode];
                    if (groupRow != null)
                    {
                        object groupInstance = GroupedDataEnumerator.GetGroupInstance(groupRow);
                        compositeRow[PropertyIndices[0]][PropertyIndices[1]] = groupInstance;
                    }
                    #endregion
                }
                else
                    compositeRow[PropertyIndices[0]][PropertyIndices[1]] = PropertyType.InstantiateObject(compositeRow, dataNodesRows);
            }
            else if (PropertyTypeIsEnumerable)
            {
                if (SourceDataNode == PropertyOwnerType.RootDataNode && PropertyOwnerType.RootDataNode.Recursive)
                    compositeRow[PropertyIndices[0]][PropertyIndices[1]] = GetRelatedDataCollection(dataNodesRows[SourceDataNode], dataNodesRows);
                else
                    compositeRow[PropertyIndices[0]][PropertyIndices[1]] = GetRelatedDataCollection(dataNodesRows[SourceDataNode.ParentDataNode], dataNodesRows);
            }
        }


        /// <MetaDataID>{a024f596-548e-4ffa-80f4-9d6f86be5865}</MetaDataID>
        System.Collections.Generic.Dictionary<object, object> GroupedData;

        ///<summary>
        ///Retrieve a collection of objects that have a common key.
        ///</summary>
        ///<param name="key">
        ///Defines the common key 
        ///</param>
        /// <MetaDataID>{fb043d07-87ca-4720-8498-01ef05d284e9}</MetaDataID>
        object GetGroupInstance(object key)
        {
            object groupData = null;
            GroupedData.TryGetValue(key, out groupData);
            return groupData;
        }




        ///<summary>
        ///Retrieve a collection of objects that have a common key.
        ///Key extracted from groupDataRow.
        ///</summary>
        ///<param name="groupDataRow">
        ///Defines the row with key data.
        ///</param>
        /// <MetaDataID>{1999f40c-a65d-4c49-92ec-18dbc3aaa063}</MetaDataID>
        public object GetGroupInstance(System.Data.DataRow groupDataRow)
        {

            return GetGroupInstance(GroupingMetaData.GetKey(groupDataRow));
        }



        /// <exclude>Excluded</exclude>
        CompositeRowData _CurrentCompositeRow;
        /// <MetaDataID>{b398ea78-4118-4ccc-9411-32c4dfa9bcbc}</MetaDataID>
        CompositeRowData CurrentCompositeRow
        {
            get
            {
                return _CurrentCompositeRow;
            }
            set
            {
                _CurrentCompositeRow = value;
                RetrieveCurrent = true;
            }
        }

        /// <MetaDataID>{da80ad52-195a-42b9-bd75-130f5e5e818a}</MetaDataID>
        List<System.Data.DataRow[]> CompositeRows;


        #endregion


        /// <MetaDataID>{1e91c1ce-a844-4773-ac31-f865afbe1500}</MetaDataID>
        public System.Collections.IEnumerator GetGroupedDataEnumerator(List<System.Data.DataRow[]> compositeRows, Dictionary<DataNode, int> dataNodeRowIndices, DataNodesRows parentsdDataNodesRows)
        {

            if (RootDataNode.Type == DataNode.DataNodeType.Group)
            {
                if (_DataRetrievePath == null)
                {
                    if (PropertiesIndices != null)
                        PropertiesIndices.Clear();
                }

                RealEnum = null;
                System.Data.DataRow[] arrayOfRows = new System.Data.DataRow[compositeRows.Count];
                int i = 0;
                foreach (var compositeRowData in compositeRows)
                    arrayOfRows[i++] = compositeRowData[dataNodeRowIndices[RootDataNode]];

                CompositeRows = GetAllDataAsSingleTable(arrayOfRows, parentsdDataNodesRows);

                if (Type.Name == typeof(IDynamicGrouping<,>).Name)
                {
                    System.Reflection.PropertyInfo propertyInfo = Type.GetProperty("GroupedData");
                    IList list = Activator.CreateInstance(typeof(System.Collections.Generic.List<>).MakeGenericType(propertyInfo.PropertyType)) as IList;
                    foreach (System.Data.DataRow[] compositeRow in CompositeRows)
                    {
                        list.Add(compositeRow[PropertiesIndices[propertyInfo][0]][PropertiesIndices[propertyInfo][1]]);
                    }
                    return list.GetEnumerator();
                }
                else
                {
                    if (Properties != null)
                    {
                        #region Gets constructor metadata
                        if (ConstructorInfo == null)
                        {
                            ConstructorInfo = typeof(ObjectType).GetConstructors()[0];
                            FastConstructorInvoke = AccessorBuilder.GetConstructorInvoker(ConstructorInfo);
                            Parameters = ConstructorInfo.GetParameters();
                        }
                        #endregion
                    }
                }
                return new DynamicTypeDataRetrieve<ObjectType>(this);

            }
            else
            {
                RealEnum = null;
                CompositeRows = compositeRows;
                LoadDynamicTypePropertiesValues(compositeRows, parentsdDataNodesRows);
                return new DynamicTypeDataRetrieve<ObjectType>(this);
            }

        }


        /// <summary>
        /// Convert the current composite row to an object. 
        /// If the type of returned object is dynamic create a new object 
        /// and initialize it through the constructor 
        /// otherwise return the retrieved object from composite row. 
        /// </summary>
        /// <MetaDataID>{eebd7290-8d93-44a1-b4d9-5bfc84b51b95}</MetaDataID>
        public ObjectType CurrentOld
        {
            get
            {
                if (!RetrieveCurrent)
                    return _Current;
                try
                {
                    lock (this)
                    {
                        if (Properties != null)
                        {
                            nullRow = true;
                            #region Gets constructor metadata
                            if (ConstructorInfo == null)
                            {
                                ConstructorInfo = typeof(ObjectType).GetConstructors()[0];
                                FastConstructorInvoke = AccessorBuilder.GetConstructorInvoker(ConstructorInfo);
                                Parameters = ConstructorInfo.GetParameters();
                            }
                            #endregion

                            object[] _params = new object[Parameters.Length];
                            if (typeof(ObjectType).Name.IndexOf("<>f__AnonymousType") == 0)
                            {
                                #region loads constractor parameters values
                                int i = 0;
                                foreach (System.Reflection.ParameterInfo paramInfo in Parameters)
                                {
                                    if (CtorParametersIndices[i][1] == -1 && CtorParametersIndices[i][0] == -1)
                                    {
                                        //when contructor parameter indexes is double -1 the parameter value comes from local scope variable 
                                        _params[i] = _CtorParametersLocalScopeValues[i];
                                    }
                                    else if (CurrentCompositeRow[CtorParametersIndices[i][0]] != null)
                                    {
                                        int valueIndex = CtorParametersIndices[i][1];
                                        _params[i] = CurrentCompositeRow[CtorParametersIndices[i][0]][CtorParametersIndices[i][1]];
                                        if (_params[i] is System.DBNull)
                                            _params[i] = AccessorBuilder.GetDefaultValue(paramInfo.ParameterType);
                                        else
                                        {
                                            nullRow = false;
                                            if (paramInfo.ParameterType.IsValueType && _params[i] != null)
                                            {
                                                if (paramInfo.ParameterType.IsGenericType &&
                                                    !paramInfo.ParameterType.IsGenericTypeDefinition &&
                                                    paramInfo.ParameterType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                                                {

                                                    i++;
                                                    continue;
                                                }
                                                else
                                                {
                                                    if (paramInfo.ParameterType.BaseType == typeof(System.Enum))
                                                        _params[i] = System.Enum.GetValues(paramInfo.ParameterType).GetValue((int)_params[i]);
                                                    else
                                                        _params[i] = System.Convert.ChangeType(_params[i], paramInfo.ParameterType);
                                                }
                                            }
                                        }
                                    }
                                    else
                                        _params[i] = AccessorBuilder.GetDefaultValue(paramInfo.ParameterType);
                                    i++;
                                }
                                #endregion
                                ObjectType retValue = (ObjectType)FastConstructorInvoke.Invoke(null, _params);
                                _Current = retValue;
                                if (nullRow)
                                {
                                    _Current = default(ObjectType);
                                    return _Current;
                                }
                                return retValue;
                            }
                            else
                            {
                                //PropertyInfos = 
                                ObjectType retValue = (ObjectType)FastConstructorInvoke.Invoke(null, new object[0]);
                                int i = 0;
                                foreach (System.Reflection.PropertyInfo propertyInfo in typeof(ObjectType).GetProperties())
                                {
                                    if (Properties.ContainsKey(propertyInfo))
                                    {
                                        if (CurrentCompositeRow[PropertiesIndices[propertyInfo][0]] != null)
                                        {
                                            if (Properties[propertyInfo].SourceDataNode == RootDataNode && Properties[propertyInfo].PropertyTypeIsEnumerable)
                                            {
                                                //Dictionary<DataNode, System.Data.DataRow> dataNodesRows = new Dictionary<DataNode, System.Data.DataRow>();
                                                //foreach (DataNode dataNode in DataNodeRowIndices.Keys)
                                                //    dataNodesRows.Add(dataNode, CurrentCompositeRow[DataNodeRowIndices[dataNode]]);
                                                DataNodesRows dataNodesRows = new DataNodesRows(CurrentCompositeRow.CompositeRow, DataNodeRowIndices);

                                                //Properties[propertyInfo]
                                                CurrentCompositeRow[PropertiesIndices[propertyInfo][0]][PropertiesIndices[propertyInfo][1]] = Properties[propertyInfo].GetRelatedDataCollection(CurrentCompositeRow.CompositeRow[DataNodeRowIndices[Properties[propertyInfo].SourceDataNode]], dataNodesRows);
                                                // LoadPropertyCollection(CurrentCompositeRow, Properties[propertyInfo], dataNodesRows, CurrentCompositeRow[DataNodeRowIndices[Properties[propertyInfo].SourceDataNode]]);// rows);
                                            }
                                            object propertyValue = CurrentCompositeRow[PropertiesIndices[propertyInfo][0]][PropertiesIndices[propertyInfo][1]];
                                            if (propertyValue is System.DBNull)
                                                propertyValue = AccessorBuilder.GetDefaultValue(propertyInfo.PropertyType);

                                            if (propertyValue != null)
                                                nullRow = false;
                                            propertyInfo.SetValue(retValue, propertyValue, null);
                                        }
                                        else
                                        {
                                            propertyInfo.SetValue(retValue, AccessorBuilder.GetDefaultValue(propertyInfo.PropertyType), null);
                                        }
                                        i++;
                                    }
                                }
                                _Current = retValue;
                                if (nullRow)
                                {
                                    _Current = default(ObjectType);
                                    return _Current;
                                }
                                return retValue;
                            }
                        }
                        else
                        {
                            if (CurrentCompositeRow[ConventionTypeRowIndex] == null)
                            {
                                _Current = default(ObjectType);
                                return default(ObjectType);
                            }
                            object value = CurrentCompositeRow[ConventionTypeRowIndex][ConventionTypeColumnIndex];
                            if (typeof(ObjectType).IsValueType && value != null && !(value is System.DBNull))
                                value = System.Convert.ChangeType(value, typeof(ObjectType));
                            ObjectType retValue;
                            if (value == null || value is System.DBNull)
                                retValue = default(ObjectType);
                            else
                                retValue = (ObjectType)value;
                            _Current = retValue;
                            return retValue;
                        }
                    }
                }
                finally
                {
                    RetrieveCurrent = false;
                }
            }
        }


    }


    interface InterfaceCode
    {

        /// <MetaDataID>{8bfde616-9402-4787-8a6a-ff673f52e3be}</MetaDataID>
        object GetGroupInstance(System.Data.DataRow groupDatakey);


        ///<summary>
        ///This operation returns an enumerator on object collection.
        ///The objects of object collection created from the data of rows. 
        ///</summary>
        ///<param name="rows">
        ///The rows parameter defines the data which used from the dynamic type retriever 
        ///to create the collection with dynamic type objects.  
        ///</param>
        /// <param name="parentsDataNodesRows">
        /// parentsDataNodeRows dictionary keeps the data node row pairs. 
        /// This pairs is useful when the RetrieveDataPath of (dynamic type data retriever) property has Data nodes 
        /// which are parents (in data tree) of rows Data Node (parameter rows defines this rows). 
        /// </param>
        /// <MetaDataID>{67ca84f1-ad25-4191-b7c7-8d14e07737df}</MetaDataID>
        /// <returns></returns>
        System.Collections.IEnumerator GetRelatedDataEnumerator(System.Data.DataRow row, DataNodesRows parentsDataNodesRows);



        /// <summary>
        /// This method return the new dynamic type(anonymous type) object.
        /// The values of new object retrieved from composite row
        /// </summary>
        /// <param name="compositeRow">
        /// This parameter defines the composite row.
        /// The composite row used from method to retrieve the values dynamic type (anonymous type) object and then load the just created object.
        /// </param>
        /// <param name="dataNodesRows">
        ///  This parameter define a dictionary which keeps the DataNode-DataRow pairs.
        /// </param>
        /// <MetaDataID>{0f894ef1-de06-4d65-bd96-bec3609c6fe0}</MetaDataID>
        object InstantiateObject(System.Data.DataRow[] compositeRow, DataNodesRows dataNodesRows);

        /// <summary>
        /// DataNodeRowIndices dictionary keeps the row index in composite row for each DataNode.
        /// DynamicTypeDataRetrieve retrieves data and load them in a collection with composite row.
        /// Composite row is an array with DataTables rows. 
        /// </summary>
        /// <MetaDataID>{fe78164f-a7eb-418b-a3df-8bb5ab066e1c}</MetaDataID>
        Dictionary<DataNode, int> DataNodeRowIndices
        {
            get;
        }



        /// <summary>
        /// This method builds the data node path for data retrieve.
        /// </summary>
        /// <param name="dataNodeRowIndices">
        /// The dataNodeRowIndices parameter define dictionary where the method set the 
        /// data node - index key value pair, the value of key value pair is the index of row in composite row 
        /// which keeps the data for data node.  
        /// </param>
        /// <param name="retrieveDataPath">
        /// This parameter defines the already builded RetrieveDataPath
        /// </param>
        /// <param name="unAssignedNodes">
        /// This parameter defines the unassinged which will be added at the end of RetrieveDataPath.
        /// </param>
        /// <MetaDataID>{e7a333eb-0af0-407e-ad74-8a6768d354f4}</MetaDataID>
        void BuildRetrieveDataPath(Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<DataRetrieveNode> filterDataPath, List<DataRetrieveNode> unAssignedNodes);

        /// <summary>
        /// Builds the  PropertiesIndices  dictionary.
        /// The indices used from dynamic type data retriever to find the values of dynamic type properties.
        /// </summary>
        /// <param name="nextExtensionRowIndex">
        /// This parameter defines the next available position in extension row.
        /// Extension row has columns with derived types 
        /// </param>
        /// <param name="dataNodeRowIndices">
        /// This parameter definens  the dictionary which keeps the row index in composite row for each DataNode.
        /// </param>
        /// <param name="retrieveDataPath">
        /// </param>
        /// <MetaDataID>{eb84a7d0-0882-44e2-9c84-9d99c5a6f8f4}</MetaDataID>
        void BuildPropertiesDataIndices(ref int nextExtensionRowIndex, Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath);

        ///<summary>
        ///This operation retrieve’s data from grouped composite rows collection and fills a collection with the grouped by objects.
        ///</summary>
        ///<returns>
        ///Returns a enumerator of grouped objects collection
        ///</returns>
        /// <MetaDataID>{8587e74b-4cd3-4dd2-8a50-d952338c3a08}</MetaDataID>
        System.Collections.IEnumerator GetGroupedDataEnumerator(List<System.Data.DataRow[]> compositeRows, Dictionary<DataNode, int> dataNodeRowIndices, DataNodesRows parentsdDataNodesRows);

    

    }



    class GroupingMetaDataCode
    {
        /// <MetaDataID>{c270eb59-fe00-4704-b1d7-817b0c4b0eb6}</MetaDataID>
        internal void GroupCompositeRows(ref List<System.Data.DataRow[]> compositeRows, DataNodesRows parentsdDataNodesRows, Dictionary<DataNode, int> dataNodeRowIndices, out Dictionary<object, object> groupedData)
        {
            groupedData = new Dictionary<object, object>();
            Dictionary<object, System.Data.DataRow[]> groupedDataRows = new Dictionary<object, System.Data.DataRow[]>();
            if (KeyCtorParametersIndices == null && KeyConstInfo != null)
            {
                System.Reflection.ParameterInfo[] parameters = KeyConstInfo.GetParameters();
                KeyCtorParametersIndices = new int[parameters.Length][];
                _keyCtorParams = new object[parameters.Length];
                int i = 0;
                foreach (System.Reflection.ParameterInfo paramInfo in parameters)
                {
                    KeyCtorParametersIndices[i] = KeyPropertiesIndices[KeyConstInfo.DeclaringType.GetProperty(paramInfo.Name)];
                    i++;
                }
            }

            List<System.Data.DataRow[]> keyGroupedRows = new List<System.Data.DataRow[]>();
            foreach (System.Data.DataRow[] composeDataRow in compositeRows)
            {
                if (groupedDataRows == null)
                    groupedDataRows = new Dictionary<object, System.Data.DataRow[]>();
                object key = null;
                if (CompositeKey)
                {
                    for (int i = 0; i != _keyCtorParams.Length; i++)
                    {
                        if (composeDataRow[KeyCtorParametersIndices[i][0]] == null)
                        {
                            _keyCtorParams[i] = null;
                            continue;
                        }

                        _keyCtorParams[i] = composeDataRow[KeyCtorParametersIndices[i][0]][KeyCtorParametersIndices[i][1]];
                    }
                    key = FastKeyConstructorInvoke.Invoke(null, _keyCtorParams);
                }
                else
                    key = composeDataRow[KeyIndices[0]][KeyIndices[1]];

                System.Data.DataRow[] keyData = null;
                if (!groupedDataRows.TryGetValue(key, out keyData))
                {
                    groupedDataRows[key] = composeDataRow;
                    keyGroupedRows.Add(composeDataRow);
                    composeDataRow[GroupedRowsRowIndex][GroupedRowsColumnIndex] = new List<System.Data.DataRow[]>(); //new List<System.Data.DataRow>();
                    (composeDataRow[GroupedRowsRowIndex][GroupedRowsColumnIndex] as List<System.Data.DataRow[]>).Add(composeDataRow);//[DataNodeRowIndices[GroupedDataRetrieve.RootDataNode]]);
                    composeDataRow[KeyTypeRowIndex][KeyTypeColumnIndex] = key;

                }
                else
                {
                    if (composeDataRow[dataNodeRowIndices[GroupedDataRetrieve.RootDataNode]] != null)
                        (keyData[GroupedRowsRowIndex][GroupedRowsColumnIndex] as List<System.Data.DataRow[]>).Add(composeDataRow);//[DataNodeRowIndices[GroupedDataRetrieve.RootDataNode]]);
                    else
                    {

                    }
                }


            }

            compositeRows = keyGroupedRows;
            {
                if (FastGroupingConstructorInvoke == null)
                {
                    System.Type groupingType = typeof(Grouping<,>).MakeGenericType(GroupingType.GetGenericArguments()[0], GroupingType.GetGenericArguments()[1]);
                    FastGroupingConstructorInvoke = AccessorBuilder.CreateConstructorInvoker(groupingType.GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)[0]);
                }
            }
            foreach (System.Data.DataRow[] composeDataRow in compositeRows)
            {
                object key = composeDataRow[KeyTypeRowIndex][KeyTypeColumnIndex];
                System.Collections.IEnumerator enumerator = GroupedDataRetrieve.GetGroupedDataEnumerator((composeDataRow[GroupedRowsRowIndex][GroupedRowsColumnIndex] as List<System.Data.DataRow[]>), dataNodeRowIndices, parentsdDataNodesRows);
                object resault = FastGroupingConstructorInvoke(null, new object[2] { key, enumerator });// composeDataRow[GroupingResultRowIndex][GroupingResultColumnIndex];
                groupedData[key] = resault;
                composeDataRow[GroupingResultRowIndex][GroupingResultColumnIndex] = resault;
            }
        }


        /// <summary>
        /// Builds the  KeyPropertiesIndices  dictionary.
        /// The indices used from dynamic type data retriever to find the values of dynamic type properties.
        /// </summary>
        /// <param name="nextExtensionRowIndex">
        /// This parameter defines the next available position in extension row.
        /// Extension row has columns with derived types 
        /// </param>
        /// <param name="dataNodeRowIndices">
        /// This parameter definens  the dictionary which keeps the row index in composite row for each DataNode.
        /// </param>
        /// <param name="retrieveDataPath">
        /// </param>
        /// <MetaDataID>{75026644-6dc4-4926-a224-09f1ce883ea6}</MetaDataID>
        public void BuildPropertiesDataIndices(ref int nextColumnIndexInExtensionRow, Dictionary<DataNode, int> dataNodeRowIndices, System.Collections.Generic.LinkedList<DataRetrieveNode> retrieveDataPath)
        {

            KeyTypeRowIndex = retrieveDataPath.Count;
            KeyTypeColumnIndex = nextColumnIndexInExtensionRow++;
            GroupedRowsRowIndex = retrieveDataPath.Count;
            GroupedRowsColumnIndex = nextColumnIndexInExtensionRow++;

            if (CompositeKey)
            {
                KeyPropertiesIndices = new Dictionary<System.Reflection.PropertyInfo, int[]>();
                foreach (DynamicTypeProperty keyProperty in KeyDynamicTypeDataRetrieve.Properties.Values)
                {
                    if (keyProperty.SourceDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (keyProperty.SourceDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                            keyProperty.SourceDataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                            keyProperty.SourceDataNode.ParentDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                        {
                            KeyPropertiesIndices[keyProperty.PropertyInfo] = new int[] { dataNodeRowIndices[keyProperty.SourceDataNode.ParentDataNode.ParentDataNode], keyProperty.SourceDataNode.ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(keyProperty.SourceDataNode) };
                        }
                        else
                            KeyPropertiesIndices[keyProperty.PropertyInfo] = new int[] { dataNodeRowIndices[keyProperty.SourceDataNode.ParentDataNode], keyProperty.SourceDataNode.ParentDataNode.DataSource.GetColumnIndex(keyProperty.SourceDataNode) };
                    }
                    else
                        KeyPropertiesIndices[keyProperty.PropertyInfo] = new int[] { dataNodeRowIndices[keyProperty.SourceDataNode], keyProperty.SourceDataNode.DataSource.ObjectIndex };
                }
            }
            else
            {
                if (GroupDataNode.GroupKeyDataNodes[0].Type == DataNode.DataNodeType.OjectAttribute)
                {
                    if (GroupDataNode.GroupKeyDataNodes[0].Type == DataNode.DataNodeType.OjectAttribute &&
                        GroupDataNode.GroupKeyDataNodes[0].ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                        GroupDataNode.GroupKeyDataNodes[0].ParentDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                    {
                        KeyIndices = new int[] { dataNodeRowIndices[GroupDataNode.GroupKeyDataNodes[0].ParentDataNode.ParentDataNode], GroupDataNode.GroupKeyDataNodes[0].ParentDataNode.ParentDataNode.DataSource.GetColumnIndex(GroupDataNode.GroupKeyDataNodes[0]) };
                    }
                    else
                        KeyIndices = new int[] { dataNodeRowIndices[GroupDataNode.GroupKeyDataNodes[0].ParentDataNode], GroupDataNode.GroupKeyDataNodes[0].ParentDataNode.DataSource.GetColumnIndex(GroupDataNode.GroupKeyDataNodes[0]) };
                }
                else
                {
                    if (GroupDataNode.GroupKeyDataNodes[0].ParticipateInSelectClause)
                        KeyIndices = new int[] { dataNodeRowIndices[GroupDataNode.GroupKeyDataNodes[0]], GroupDataNode.GroupKeyDataNodes[0].DataSource.ObjectIndex };
                    else
                        KeyIndices = new int[] { dataNodeRowIndices[GroupDataNode.GroupKeyDataNodes[0]], GroupDataNode.GroupKeyDataNodes[0].DataSource.ObjectIdentityColumnIndex };
                }



            }

        }
        /// <MetaDataID>{5027e679-86c5-468a-958b-d738768bad5d}</MetaDataID>
        int[][] KeyCtorParametersIndices = null;
        /// <exclude>Excluded</exclude>
        object[] _keyCtorParams = null;


        /// <MetaDataID>{f0aadb91-da78-4a69-a8c1-a842711bd807}</MetaDataID>
        AccessorBuilder.FastInvokeHandler FastGroupingConstructorInvoke;

        /// <MetaDataID>{dbd20ff1-d755-4ef2-af37-4e4a9703f437}</MetaDataID>
        bool CompositeKey;
        /// <MetaDataID>{5cf3b59d-3b90-43eb-af86-c370f31656fb}</MetaDataID>
        public AccessorBuilder.FastInvokeHandler _FastKeyConstructorInvoke;

        /// <MetaDataID>{bc01045e-a8e8-4a9d-8960-d5d9f378fc96}</MetaDataID>
        public AccessorBuilder.FastInvokeHandler FastKeyConstructorInvoke
        {
            get
            {
                return _FastKeyConstructorInvoke;
            }
        }


        /// <MetaDataID>{5a2eae6d-901e-4371-a795-839e30293599}</MetaDataID>
        public ConstructorInfo _KeyConstInfo;

        /// <MetaDataID>{5a2e0258-7fdd-4824-8d9f-8f29f7710a23}</MetaDataID>
        public ConstructorInfo KeyConstInfo
        {
            get
            {
                return _KeyConstInfo;
            }
        }

        /// <MetaDataID>{fdb59203-57f9-4628-b822-50d8f1e5f679}</MetaDataID>
        // public readonly Dictionary<System.Reflection.PropertyInfo, DynamicTypeProperty> KeyProperties = new Dictionary<System.Reflection.PropertyInfo, DynamicTypeProperty>();
        /// <MetaDataID>{3f4e90c4-bb7c-4cf5-a6b8-5847c9d1a2ab}</MetaDataID>
        Dictionary<System.Reflection.PropertyInfo, int[]> KeyPropertiesIndices;
        /// <MetaDataID>{5a8c7dfc-de53-44c1-842e-253e0eeedbbd}</MetaDataID>
        int[] KeyIndices;
        /// <MetaDataID>{77a0025f-79c7-4b89-91a7-f29cfa9a1ee6}</MetaDataID>
        int? GroupDataKeyIndex;
        /// <MetaDataID>{805613bb-4e6f-4653-b93b-f7b4f29748fa}</MetaDataID>
        internal List<DynamicTypeProperty> CtorParamsDataRetrieverProperies = new List<DynamicTypeProperty>();
        /// <MetaDataID>{dec984c1-98b9-49c1-9da2-322b8b1c0267}</MetaDataID>
        int KeyTypeRowIndex = -1;
        /// <MetaDataID>{807a8628-c92b-407c-8848-edfa4e39ade3}</MetaDataID>
        int KeyTypeColumnIndex = -1;


        /// <MetaDataID>{266e7f0b-a91f-4b88-96a7-39f2c4a04520}</MetaDataID>
        int GroupedRowsRowIndex = -1;
        /// <MetaDataID>{8c706830-973b-4b76-9832-120fa2a10d5f}</MetaDataID>
        int GroupedRowsColumnIndex = -1;

        /// <MetaDataID>{86f89c1c-b9ab-442f-a498-23ab83c03fcf}</MetaDataID>
        internal int GroupingResultRowIndex = -1;
        /// <MetaDataID>{6166858f-abcf-4198-b160-b1e6665ed4d4}</MetaDataID>
        internal int GroupingResultColumnIndex = -1;



        /// <MetaDataID>{829a98dc-1360-41e7-bfae-17e03e926701}</MetaDataID>
        IDynamicTypeDataRetrieve _KeyDynamicTypeDataRetrieve;
        /// <MetaDataID>{0f642ab4-8d29-4c33-9da7-908d092fc7de}</MetaDataID>
        internal IDynamicTypeDataRetrieve KeyDynamicTypeDataRetrieve
        {
            get
            {
                return _KeyDynamicTypeDataRetrieve;
            }
            set
            {
                _KeyDynamicTypeDataRetrieve = value;


                if (_KeyDynamicTypeDataRetrieve.Properties != null)
                {
                    CtorParamsDataRetrieverProperies.Clear();
                    CompositeKey = true;
                    _KeyConstInfo = _KeyDynamicTypeDataRetrieve.Type.GetConstructors()[0];
                    _FastKeyConstructorInvoke = AccessorBuilder.CreateConstructorInvoker(KeyConstInfo);
                    foreach (System.Reflection.ParameterInfo paramInfo in KeyConstInfo.GetParameters())
                    {
                        System.Reflection.PropertyInfo property = KeyConstInfo.DeclaringType.GetProperty(paramInfo.Name);
                        CtorParamsDataRetrieverProperies.Add(_KeyDynamicTypeDataRetrieve.Properties[property]);
                    }
                }

                //KeyProperties = value.Properties;
            }
        }

        /// <MetaDataID>{f2d1eba2-1442-49d7-b9b0-c1b0984206c7}</MetaDataID>
        int[] keyParamColumnIndicies = null;

        /// <MetaDataID>{00f00edc-0a9b-4bc5-8c7e-f5b2e6d27aa9}</MetaDataID>
        object[] KeyCtorParams = null;

        ///<summary>
        ///Return key object from groupDataRow
        ///</summary>
        ///<param name="groupDataRow">
        ///Defines the row with key data.
        ///</param>
        /// <MetaDataID>{9dbfbbfe-ce3d-4568-9607-3bb2b497c9d2}</MetaDataID>
        internal object GetKey(System.Data.DataRow groupDataRow)
        {
            int i = 0;
            if (CompositeKey)
            {

                if (keyParamColumnIndicies == null)
                {
                    #region Build key object constructor parameters indicies
                    System.Reflection.ParameterInfo[] parameters = KeyConstInfo.GetParameters();
                    KeyCtorParams = new object[parameters.Length];
                    keyParamColumnIndicies = new int[KeyCtorParams.Length];
                    i = 0;
                    foreach (System.Reflection.ParameterInfo paramInfo in parameters)
                    {
                        System.Reflection.PropertyInfo property = KeyConstInfo.DeclaringType.GetProperty(paramInfo.Name);
                        DynamicTypeProperty dynamicProperty = KeyDynamicTypeDataRetrieve.Properties[property];
                        DataNode dataNode = dynamicProperty.SourceDataNode;
                        if (dataNode.Type != DataNode.DataNodeType.Object)
                        {
                            if (dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                dataNode.ParentDataNode.Classifier.FullName == typeof(DateTime).FullName)
                            {
                                keyParamColumnIndicies[i] = GroupDataNode.DataSource.DataTable.Columns.IndexOf(dataNode.ParentDataNode.ParentDataNode.Alias + "_" + dataNode.ParentDataNode.Name + "_" + dataNode.AssignedMetaObject.Name);
                            }
                            else
                            {
                                keyParamColumnIndicies[i] = GroupDataNode.DataSource.GetColumnIndex(dataNode);
                            }

                        }
                        i++;
                    }
                    #endregion
                }
            }
            else
            {
                if (!GroupDataKeyIndex.HasValue)
                {
                    DataNode dataNode = GroupDataNode.GroupKeyDataNodes[0];
                    if (dataNode.Type != DataNode.DataNodeType.Object)
                        GroupDataKeyIndex = GroupDataNode.DataSource.GetColumnIndex(dataNode);
                }
            }
            object key = null;
            #region Retrieves constructor parameters values
            if (CompositeKey)
            {
                i = 0;
                foreach (System.Reflection.ParameterInfo paramInfo in KeyConstInfo.GetParameters())
                {
                    DynamicTypeProperty dynamicProperty = CtorParamsDataRetrieverProperies[i];// entry.Value.PropertyType.GroupingMetaData.KeyProperties[property];
                    if (dynamicProperty.SourceDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        ICollection<System.Data.DataRow> objectRows = GroupDataNode.DataSource.GetRelatedRows(groupDataRow, dynamicProperty.SourceDataNode);
                        if (objectRows.Count > 0)
                        {
                            foreach (System.Data.DataRow row in objectRows)
                            {
                                KeyCtorParams[i] = row[dynamicProperty.SourceDataNode.DataSource.ObjectIndex];
                                break;
                            }
                        }
                    }
                    else
                    {
                        object keyValue = groupDataRow[keyParamColumnIndicies[i]];
                        if (keyValue is System.DBNull)
                            keyValue = null;

                        KeyCtorParams[i] = keyValue;
                    }
                    i++;
                }
            #endregion

                key = FastKeyConstructorInvoke.Invoke(null, KeyCtorParams);
            }
            else
            {
                DataNode dataNode = GroupDataNode.GroupKeyDataNodes[0];
                if (dataNode.Type == DataNode.DataNodeType.Object)
                {
                    ICollection<System.Data.DataRow> objectRows = GroupDataNode.DataSource.GetRelatedRows(groupDataRow, dataNode);
                    if (objectRows.Count > 0)
                    {
                        foreach (System.Data.DataRow row in objectRows)
                        {
                            if (dataNode.ParticipateInSelectClause)
                                key = row[dataNode.DataSource.ObjectIndex];
                            else
                                key = row[dataNode.DataSource.ObjectIdentityColumnIndex];

                            break;
                        }
                    }
                }
                else
                {
                    key = groupDataRow[GroupDataKeyIndex.Value];
                }
            }
            return key;
        }

    }
}
