using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    ///// <MetaDataID>{c5f305ca-b7ca-4c9b-8dc6-200649da79d2}</MetaDataID>
    //[Serializable]
    //public class DataNodesRowsP
    //{
    //    public readonly DataNodesRows ParentTypeDataNodesRows;
    //    public DataNodesRowsP(CompositeRowData compositeRow, Dictionary<DataNode, int> dataNodeRowIndices)
    //    {
    //        CompositeRow = compositeRow;
    //        DataNodeRowIndices = dataNodeRowIndices;
    //    }
    //    public DataNodesRowsP(CompositeRowData compositeRow, Dictionary<DataNode, int> dataNodeRowIndices, DataNodesRows parentTypeDataNodesRows)
    //    {
    //        CompositeRow = compositeRow;
    //        DataNodeRowIndices = dataNodeRowIndices;
    //        ParentTypeDataNodesRows = parentTypeDataNodesRows;
    //    }

    //    public readonly CompositeRowData CompositeRow;
    //    internal Dictionary<DataNode, int> DataNodeRowIndices;
    //    public RowData this[DataNode key]
    //    {
    //        get
    //        {
    //            if (DataNodeRowIndices.ContainsKey(key))
    //                return CompositeRow[DataNodeRowIndices[key]];
    //            else if (ParentTypeDataNodesRows != null)
    //                return ParentTypeDataNodesRows[key];
    //            return null;
    //        }
    //    }

    //    public bool ContainsKey(DataNode dataNode)
    //    {
    //        var ret = DataNodeRowIndices.ContainsKey(dataNode);
    //        if (!ret && ParentTypeDataNodesRows != null)
    //            ret = ParentTypeDataNodesRows.ContainsKey(dataNode);
    //        return ret;


    //    }
    //}


    /// <MetaDataID>{c5f305ca-b7ca-4c9b-8dc6-200649da79d2}</MetaDataID>
    [Serializable]
    public class DataNodesRows
    {
        public readonly DataNodesRows ParentTypeDataNodesRows;
      //  LinkedList<DataRetrieveNode> DataRetrievePath;
        internal DataNodesRows(CompositeRowData compositeRow, Dictionary<DataNode, int> dataNodeRowIndices)
        {
            CompositeRow = compositeRow;
            DataNodeRowIndices = new Dictionary<Guid, int>();
            foreach (var entry in dataNodeRowIndices)
                DataNodeRowIndices[entry.Key.Identity] = entry.Value;

          //  DataRetrievePath = dataRetrievePath;
        }
        internal DataNodesRows(CompositeRowData compositeRow, Dictionary<DataNode, int> dataNodeRowIndices, DataNodesRows parentTypeDataNodesRows)
        {
            CompositeRow = compositeRow;
            DataNodeRowIndices = new Dictionary<Guid, int>();
            foreach (var entry in dataNodeRowIndices)
                DataNodeRowIndices[entry.Key.Identity] = entry.Value;

            ParentTypeDataNodesRows = parentTypeDataNodesRows;
           // DataRetrievePath = dataRetrievePath;
        }

        public readonly CompositeRowData CompositeRow;
        internal Dictionary<Guid, int> DataNodeRowIndices;
        public RowData this[DataNode key]
        {
            get
            {
                if (DataNodeRowIndices.ContainsKey(key.Identity))
                    return CompositeRow[DataNodeRowIndices[key.Identity]];
                else if (ParentTypeDataNodesRows != null)
                    return ParentTypeDataNodesRows[key];
                return null;
            }
        }

        public bool ContainsKey(DataNode dataNode)
        {
            var ret = DataNodeRowIndices.ContainsKey(dataNode.Identity);
            if (!ret && ParentTypeDataNodesRows != null)
                ret = ParentTypeDataNodesRows.ContainsKey(dataNode);
            return ret;


        }

        //internal bool IsResultTypeDataNode(DataNode dataNode)
        //{
        //    foreach (var dataRetrieveNode in DataRetrievePath)
        //    {
        //        if (dataNode == dataRetrieveNode.DataNode && !dataRetrieveNode.OnlyForDataFilter)
        //            return true;
        //    }
        //    if (ParentTypeDataNodesRows != null)
        //        return ParentTypeDataNodesRows.IsResultTypeDataNode(dataNode);
        //    return false;
        //}
    }
}
