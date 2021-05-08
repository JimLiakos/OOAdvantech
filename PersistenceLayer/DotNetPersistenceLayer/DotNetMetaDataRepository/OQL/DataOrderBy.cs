using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <MetaDataID>{10c4ed1e-9654-44a2-84c6-331ebc91b4e9}</MetaDataID>
    [Serializable]
    public class DataOrderBy
    {

        public override string ToString()
        {
            string text = null;
            foreach(var field in _Fields )
            {
                if (text != null)
                    text += ",";
                text += field.DataNode.FullName + "  " + field.OrderByType.ToString();

            }
            return text;
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<OrderByField> _Fields=new Collections.Generic.Set<OrderByField>();

        /// <MetaDataID>{bf72037a-6320-4b53-ab2a-81a8c8215286}</MetaDataID>
        [Association("OrderByFields", Roles.RoleA, "f72cf2db-8dd4-4146-8d0d-ce57af0b6874")]
        [RoleAMultiplicityRange(1)]
        [ImplementationMember(nameof(_Fields))]
        public IList<OrderByField> Fields
        {
            get
            {
                return _Fields.AsReadOnly();
            }
        }

        public System.Collections.Generic.List<DataNode> DataNodes
        {
            get
            {
                return (from field in Fields select field.DataNode).Distinct().ToList();
            }
        } 

        internal void BuildRetrieveDataPath(DataRetrieveNode dataRetrieveNode, Dictionary<DataNode, int> dataNodeRowIndices, LinkedList<DataRetrieveNode> dataRetrieveDataPath, System.Collections.Generic.List<DataRetrieveNode> unAssignedNodes)
        {
            System.Collections.Generic.List<DataRetrieveNode> subNodes = new System.Collections.Generic.List<DataRetrieveNode>();
            if ((DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).Type == DataNode.DataNodeType.Group) && DataNodeParticipateAsBranch(dataRetrieveNode.DataNode))
            {
                #region Checks for branch search criterions where doesn't applied from data base manage mechanism. If there aren't returns
                bool allAreLocalCriterion = true;
                bool hasCriterionsToApply = false;

             
               
                #endregion

                //dataNode.FilteredDataRowIndex = dataRetrieveDataPath.Count;

                if (DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).DataSource != null && DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).DataSource.DataTable != null)
                {
                    DerivedDataNode.GetOrgDataNode(dataRetrieveNode.DataNode).DataSource.DataTable.FilteredTable = true;
                    var exist = (from pathDataRetrieveNode in dataRetrieveDataPath.ToArray()
                                 where pathDataRetrieveNode.DataNode == dataRetrieveNode.DataNode
                                 select pathDataRetrieveNode).Count() != 0;


                    if (!exist)
                    {
                        dataNodeRowIndices[dataRetrieveNode.DataNode] = dataRetrieveDataPath.Count;
                        //dataRetrieveNode.OnlyForDataFilter = true;
                        dataRetrieveDataPath.AddLast(dataRetrieveNode);//new DataRetrieveNode(dataRetrieveNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.RealParentDataNode, dataRetrieveDataPath), dataNodeRowIndices));
                    }
                }
                #region RowRemove code
                //if (dataRetrieveNode.DataNode.DataSource != null)
                //    RowRemoveIndicies[dataRetrieveDataPath.Last.Value] = dataRetrieveNode.DataNode.DataSource.GetRowRemoveIndex(this);
                //else
                //    RowRemoveIndicies[dataRetrieveDataPath.Last.Value] = -1;
                #endregion

                #region retrieves all sub data nodes which participate in search condition as branch

                foreach (DataNode subDataNode in dataRetrieveNode.DataNode.SubDataNodes)
                {
                    if (DataNodeParticipateAsBranch(subDataNode) &&
                        (DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Object || DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Group))
                        subNodes.Add(new DataRetrieveNode(subDataNode, DataRetrieveNode.GetDataRetrieveNode(dataRetrieveNode.DataNode, dataRetrieveDataPath), dataNodeRowIndices, dataRetrieveDataPath));

                    if (DerivedDataNode.GetOrgDataNode(subDataNode).Type == DataNode.DataNodeType.Key)
                    {
                        foreach (DataNode groupKeyDataNode in subDataNode.SubDataNodes)
                        {
                            if (DataNodeParticipateAsBranch(groupKeyDataNode))
                                subNodes.Add(new DataRetrieveNode(groupKeyDataNode, DataRetrieveNode.GetDataRetrieveNode(groupKeyDataNode.RealParentDataNode, dataRetrieveDataPath), dataNodeRowIndices, dataRetrieveDataPath));
                        }
                    }
                }
                #endregion

                if (subNodes.Count > 0)
                {
                    #region Continues recursively with first subdatanodes the others are market as unassinged and will be added at the end.
                    DataRetrieveNode subDataNode = subNodes[0] as DataRetrieveNode;
                    subNodes.RemoveAt(0);
                    if (subNodes.Count > 0)
                        unAssignedNodes.AddRange(subNodes);
                    //CreateFilterPath(subDataNode, dataRetrieveDataPath, unAssignedNodes);
                    BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, dataRetrieveDataPath, unAssignedNodes);
                    #endregion
                }
                else if (unAssignedNodes.Count > 0)
                {
                    #region Continues recursively with first unassigned datanode the others will be added at the end.
                    DataRetrieveNode subDataNode = unAssignedNodes[0] as DataRetrieveNode;
                    unAssignedNodes.RemoveAt(0);
                    //CreateFilterPath(subDataNode, dataRetrieveDataPath, unAssignedNodes);
                    BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, dataRetrieveDataPath, unAssignedNodes);
                    #endregion
                }
            }
            else if (unAssignedNodes.Count > 0)
            {
                #region Continues recursively with first unassigned datanode the others will be added at the end.
                DataRetrieveNode subDataNode = unAssignedNodes[0] as DataRetrieveNode;
                unAssignedNodes.RemoveAt(0);
                //CreateFilterPath(subDataNode, dataRetrieveDataPath, unAssignedNodes);
                BuildRetrieveDataPath(subDataNode, dataNodeRowIndices, dataRetrieveDataPath, unAssignedNodes);
                #endregion
            }

        }


        private bool DataNodeParticipateAsBranch(DataNode dataNode)
        {

            foreach (DataNode orderByDataNode in DataNodes)
            {
                if (orderByDataNode.IsSameOrParentDataNode(dataNode))
                    return true;
            }
            return false;

        }
        internal void AddField(OrderByField orderByField)
        {
            if((from field in Fields
             where field.DataNode==orderByField.DataNode&&field.OrderByType==orderByField.OrderByType
             select field).FirstOrDefault()==null)
                _Fields.Add(orderByField);
        }

        internal void RemoveField(OrderByField orderByField)
        {
            var _orderByField = (from field in Fields
                                 where field.DataNode == orderByField.DataNode && field.OrderByType == orderByField.OrderByType
                                 select field).FirstOrDefault();
            if(_orderByField!=null)
                _Fields.Remove(_orderByField);
        }

        internal void CombineWith(DataOrderBy orderByFilter)
        {
            foreach (var field in orderByFilter.Fields)
                AddField(field);
                

        }

        internal List<CompositeRowData> SortCompositeRows(List<CompositeRowData> compositeRows, QueryResultType type)
        {
            List<int> collectionIndexes = null;
            if (Fields.Count == 0)
                return compositeRows;
            foreach (var field in Fields)
            {
                int rowIndex = DataRetrieveNode.GetDataSourceRowIndex(type.DataNodeRowIndices, field.DataNode);
                int columnIndex = field.DataNode.DataSourceColumnIndex;
                var fieldType = field.DataNode.Classifier.GetExtensionMetaObject<System.Type>();

                IQueryResultSort queryResultSort = null;
                if (collectionIndexes == null)
                    queryResultSort = QueryResultSort<int>.GetQueryResultSort(fieldType, compositeRows, rowIndex, columnIndex);
                else
                    queryResultSort = QueryResultSort<int>.GetQueryResultSort(fieldType, compositeRows, collectionIndexes, rowIndex, columnIndex);


                if (field.OrderByType == OrderByType.ASC)
                {
                    queryResultSort.quicksort();
                    collectionIndexes = queryResultSort.CollectionIndexes;
                }
                else
                {
                    queryResultSort.quicksortDesc();
                    collectionIndexes = queryResultSort.CollectionIndexes;
                }
            }

            return (from index in collectionIndexes
                    select compositeRows[index]).ToList();


        }
    }

    /// <MetaDataID>{c6b4c965-0b0c-4b56-af16-80ac8e5b582a}</MetaDataID>
    [Serializable]
    public class OrderByField
    {

        /// <summary>
        /// Used to retrieve DataNode indirect from query context
        /// </summary>
        /// <remarks>
        /// Because QueryResalt type object moved from main query to distributed query and vice versa
        /// Ther RootDataNode retrieved from query context 
        /// </remarks>
        internal Guid DataNodeIdentity;


        /// <exclude>Excluded</exclude>
        [NonSerialized]
        DataNode _DataNode;

        /// <MetaDataID>{c8261145-4403-4713-bdb1-29611c50664e}</MetaDataID>
        [Association("", Roles.RoleA, "5bfb11e4-8762-4ba8-b332-90d5e34d63b9")]
        [RoleAMultiplicityRange(1, 1)]
        [IgnoreErrorCheck]
        public DataNode DataNode
        {
            get
            {
                return _DataNode;
            }
            set
            {
                _DataNode = value;
                if (_DataNode != null)
                    DataNodeIdentity = _DataNode.Identity;
                else
                    DataNodeIdentity = Guid.Empty;


            }
        }

        /// <MetaDataID>{c312e7d6-f81f-47e4-8eb4-dffbc8ed9972}</MetaDataID>
        public OrderByType OrderByType=OrderByType.ASC;
    }


    /// <MetaDataID>{cc577e65-c35a-4916-ab3b-ed941796c5e1}</MetaDataID>
    [Serializable]
    public enum OrderByType
    {
        /// <summary>The OQL query retrieves data</summary>
        None = 0,
        ASC,
        DESC

    };
}
