using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.Linq
{ 
    /// <MetaDataID>{c5f305ca-b7ca-4c9b-8dc6-200649da79d2}</MetaDataID>
    public class DataNodesRows
    {
        /// <MetaDataID>{09dd335a-f9ca-46cd-b719-d6b2cdf4ff49}</MetaDataID>
        public readonly DataNodesRows ParentTypeDataNodesRows;
        /// <MetaDataID>{855f73c2-770a-4dcb-baaa-03095d306994}</MetaDataID>
        public DataNodesRows(IDataRow[] compositeRow, Dictionary<DataNode, int> dataNodeRowIndices)
        {
            CompositeRow = compositeRow;
            DataNodeRowIndices = dataNodeRowIndices;
        }
        /// <MetaDataID>{7f4d7290-2d65-41e5-90eb-dcad4271eb04}</MetaDataID>
        public DataNodesRows(IDataRow[] compositeRow, Dictionary<DataNode, int> dataNodeRowIndices, DataNodesRows parentTypeDataNodesRows)
        {
            CompositeRow = compositeRow;
            DataNodeRowIndices = dataNodeRowIndices;
            ParentTypeDataNodesRows = parentTypeDataNodesRows;
        }

        /// <MetaDataID>{3768309c-4336-49f3-b292-3db5f988dcfa}</MetaDataID>
        IDataRow[] CompositeRow;
        /// <MetaDataID>{e5ddefd3-5539-4496-a1c6-0b58662c84bd}</MetaDataID>
        internal Dictionary<DataNode, int> DataNodeRowIndices;
        /// <MetaDataID>{c50fcf77-5747-4b6f-9c86-a33ce79c826a}</MetaDataID>
        public IDataRow this[DataNode key]
        {
            get
            {
                if (DataNodeRowIndices.ContainsKey(key))
                    return CompositeRow[DataNodeRowIndices[key]];
                else if (ParentTypeDataNodesRows != null)
                    return ParentTypeDataNodesRows[key];
                return null;
            }
        }

        /// <MetaDataID>{a196b23d-7e99-4418-af55-6a14f10fb7d7}</MetaDataID>
        public bool ContainsKey(DataNode dataNode)
        {
            var ret = DataNodeRowIndices.ContainsKey(dataNode);
            if (!ret && ParentTypeDataNodesRows != null)
                ret = ParentTypeDataNodesRows.ContainsKey(dataNode);
            return ret;


        }
    }
}
