using System;
using System.Collections.Generic;
using System.Linq;

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{

    /// <summary>
    /// DataRetrieveNode objects used to keeps index the infos 
    /// the objects used from the system to retrieve the data and then 
    /// system will use data to initialize properties of dynamic type object
    /// </summary>
    /// <MetaDataID>{f96a8fe2-3130-46e7-ba5e-79f960489c9f}</MetaDataID>
    [Serializable]
    internal class DataRetrieveNode
    {
        DataRetrieveNode(DataNode dataNode, System.Collections.Generic.LinkedList<DataRetrieveNode> dataRetrievePath,Dictionary<DataNode, int>  dataNodeRowIndices)
        {
            DataNode = dataNode;
            DataRetrievePath = dataRetrievePath;
            DataNodeRowIndices = dataNodeRowIndices;

        }
        //internal DataRetrieveNode Clone(Dictionary<object, object> clonedObjects)
        //{
        //    DataRetrieveNode newDataRetrieveNode = new DataRetrieveNode();
        //    clonedObjects[this] = newDataRetrieveNode;
        //    newDataRetrieveNode._CurrentDataRowIndex = _CurrentDataRowIndex;
        //    newDataRetrieveNode._DataRowIndex = _DataRowIndex;

        //    object masterDataNode=null;
        //    if (clonedObjects.TryGetValue(_MasterDataNode, out masterDataNode))
        //        newDataRetrieveNode._MasterDataNode = masterDataNode as DataRetrieveNode;
        //    else
        //        newDataRetrieveNode._MasterDataNode = _MasterDataNode.Clone(clonedObjects);
        //}

        /// <MetaDataID>{8f9a2b1b-191e-4d1b-bb77-f3e138a17fc1}</MetaDataID>
        internal static int GetDataSourceRowIndex(Dictionary<DataNode, int> dataNodeRowIndices, DataNode dataNode)
        {
            int dataSourceRowIndex = -1;
            if (dataNodeRowIndices.TryGetValue(dataNode, out dataSourceRowIndex))
                return dataSourceRowIndex;

            dataSourceRowIndex = -1;
            DataNode dataSourceDataNode = dataNode;
            while (dataSourceDataNode.Type == OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.OjectAttribute)
                dataSourceDataNode = dataSourceDataNode.ParentDataNode;

            if (dataNodeRowIndices.ContainsKey(dataSourceDataNode))
                dataSourceRowIndex = dataNodeRowIndices[dataSourceDataNode];
            dataNodeRowIndices[dataNode] = dataSourceRowIndex;

            //dataSourceRowIndex  -1 means no data loaded for DataNode
            return dataSourceRowIndex;
        }
        /// <MetaDataID>{064d1065-c823-4d02-9488-608371903f50}</MetaDataID>
        internal static DataRetrieveNode GetDataRetrieveNode(DataNode dataNode, LinkedList<DataRetrieveNode> linkList)
        {
            if (dataNode != null && DerivedDataNode.GetOrgDataNode(dataNode).Type == DataNode.DataNodeType.Key)
                dataNode = DerivedDataNode.GetOrgDataNode(dataNode).ParentDataNode;

            foreach (DataRetrieveNode dataRetrieveNode in linkList.ToArray())
            {
                if (dataNode == dataRetrieveNode.DataNode)
                    return dataRetrieveNode;
            }
            return null;
        }

        /// <MetaDataID>{dacc9ebd-ca9d-4bdc-9c6d-be079d07dd0d}</MetaDataID>
        public readonly DataNode DataNode;
        /// <MetaDataID>{81abb8c3-cdd8-42bb-bdec-275e9041ad49}</MetaDataID>
         DataRetrieveNode _MasterDataNode;
        public DataRetrieveNode MasterDataNode
        {
            get
            {
                return _MasterDataNode;
            }
            internal set
            {
                if (_MasterDataNode != null && _MasterDataNode != value)
                {
                    if(value!=null&&_MasterDataNode.DataNode==value.DataNode&&DataRetrievePath.ToList().Contains(value))
                        _MasterDataNode = value;
                    else
                        throw new System.Exception("MasterDataNode already exist");
                }
                _MasterDataNode = value;
            }

        }



        /// <exclude>Excluded</exclude>
        int? _DataRowIndex;
        /// <summary>
        /// Define the index of row in composite row.
        /// This row keeps the data for the data node  	
        /// </summary>
        /// <MetaDataID>{b234e897-f625-416b-a4a0-b1cc9f44966e}</MetaDataID>
        public int DataRowIndex
        {
            get
            {
                if (!_DataRowIndex.HasValue)
                    _DataRowIndex = DataNodeRowIndices[DataNode];
                return _DataRowIndex.Value;

            }
        }



        /// <MetaDataID>{6963f2f7-658e-485b-a59a-66056e893ca4}</MetaDataID>
        public readonly Dictionary<DataNode, int> DataNodeRowIndices;


        LinkedList<DataRetrieveNode> DataRetrievePath;

        /// <MetaDataID>{2238dfd9-4f64-48d0-9b1a-a73075b138e1}</MetaDataID>
        public DataRetrieveNode(DataNode dataNode, DataRetrieveNode masterDataNode, Dictionary<DataNode, int> dataNodeRowIndices, LinkedList<DataRetrieveNode> dataRetrievePath)
        {
            DataNode = dataNode;// DerivedDataNode.GetOrgDataNode(dataNode);
            DataNodeRowIndices = dataNodeRowIndices;
            MasterDataNode = masterDataNode;
            DataRetrievePath = dataRetrievePath;

        }

        public DataRetrieveNode(DataRetrieveNode dataRetrieveNode, LinkedList<DataRetrieveNode> dataRetrievePath)
        {
            DataNode = dataRetrieveNode.DataNode;// DerivedDataNode.GetOrgDataNode(dataNode);
            DataNodeRowIndices = dataRetrieveNode.DataNodeRowIndices;
            _MasterDataNode = dataRetrieveNode.MasterDataNode;
            if (_MasterDataNode != null)
                _MasterDataNode = DataRetrieveNode.GetDataRetrieveNode(_MasterDataNode.DataNode, dataRetrievePath);

            OnlyForDataFilter = dataRetrieveNode.OnlyForDataFilter;
            DataRetrievePath = dataRetrievePath;

        }


        internal IDataRow[] DataRows;
        int _CurrentDataRowIndex;

        internal int CurrentDataRowIndex
        {
            get
            {
                return _CurrentDataRowIndex;
            }
            set
            {
                _CurrentDataRowIndex = value;
                if (DataRows != null && DataRows.Length <= _CurrentDataRowIndex)
                    Reset();
            }
        }
        internal bool IsReset
        {
            get
            {
                if (DataRows == null)
                    return true;
                else
                    return false;
            }
        }

        internal void Reset()
        {
            DataRows = null;
        }
        internal void SetRows(IDataRow[] dataRows)
        {
            if (dataRows != null)
                DataRows = dataRows;
            else
                DataRows = null;

            _CurrentDataRowIndex = 0;
        }

        internal bool OnlyForDataFilter;

        internal DataRetrieveNode Clone(Dictionary<object, object> clonedObjects,System.Collections.Generic.LinkedList<DataRetrieveNode> dataRetrievePath,Dictionary<DataNode, int> dataNodeRowIndices )
        {
            if(clonedObjects.ContainsKey(this))
                return clonedObjects[this] as DataRetrieveNode;
            object createdDataNode = null;

            if (!clonedObjects.TryGetValue(DataNode, out createdDataNode))
                createdDataNode =DataNode.Clone(clonedObjects);

            DataRetrieveNode newDataRetrieveNode = new DataRetrieveNode(createdDataNode as DataNode,dataRetrievePath,dataNodeRowIndices);
            clonedObjects[this] = newDataRetrieveNode;
            newDataRetrieveNode._CurrentDataRowIndex = _CurrentDataRowIndex;
            newDataRetrieveNode._DataRowIndex = _DataRowIndex;

            object createdMasterDataNode=null;
            if (clonedObjects.TryGetValue(_MasterDataNode, out createdMasterDataNode))
                newDataRetrieveNode._MasterDataNode = createdMasterDataNode as DataRetrieveNode;
            else
                newDataRetrieveNode._MasterDataNode = _MasterDataNode.Clone(clonedObjects,dataRetrievePath,dataNodeRowIndices);

            newDataRetrieveNode.OnlyForDataFilter = OnlyForDataFilter;
            

            return newDataRetrieveNode;
        }

       
    }
    /// <summary>
    /// DataRetrieveNode objects used to keeps index the infos 
    /// the objects used from the system to retrieve the data and then 
    /// system will use data to initialize properties of dynamic type object
    /// </summary>
    /// <MetaDataID>{f96a8fe2-3130-46e7-ba5e-79f960489c9f}</MetaDataID>
    [Serializable]
    internal class DataRetrieveNodeold
    {

        /// <MetaDataID>{8f9a2b1b-191e-4d1b-bb77-f3e138a17fc1}</MetaDataID>
        internal static int GetDataSourceRowIndex(Dictionary<DataNode, int> dataNodeRowIndices, DataNode dataNode)
        {
            int dataSourceRowIndex = -1;
            if (dataNodeRowIndices.TryGetValue(dataNode, out dataSourceRowIndex))
                return dataSourceRowIndex;
            DataNode dataSourceDataNode = dataNode;
            while (dataSourceDataNode.Type == OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.OjectAttribute)
                dataSourceDataNode = dataSourceDataNode.ParentDataNode;
            dataSourceRowIndex = dataNodeRowIndices[dataSourceDataNode];
            dataNodeRowIndices[dataNode] = dataSourceRowIndex;
            return dataSourceRowIndex;
        }
        /// <MetaDataID>{064d1065-c823-4d02-9488-608371903f50}</MetaDataID>
        internal static DataRetrieveNode GetDataRetrieveNode(DataNode dataNode, LinkedList<DataRetrieveNode> linkList)
        {
            if (dataNode != null && DerivedDataNode.GetOrgDataNode(dataNode).Type == DataNode.DataNodeType.Key)
                dataNode = DerivedDataNode.GetOrgDataNode(dataNode).ParentDataNode;

            foreach (DataRetrieveNode dataRetrieveNode in linkList.ToArray())
            {
                if (dataNode == dataRetrieveNode.DataNode)
                    return dataRetrieveNode;
            }
            return null;
        }

        /// <MetaDataID>{dacc9ebd-ca9d-4bdc-9c6d-be079d07dd0d}</MetaDataID>
        public readonly DataNode DataNode;
        /// <MetaDataID>{81abb8c3-cdd8-42bb-bdec-275e9041ad49}</MetaDataID>
        public readonly DataRetrieveNode MasterDataNode;

        /// <exclude>Excluded</exclude>
        int? _DataRowIndex;
        /// <summary>
        /// Define the index of row in composite row.
        /// This row keeps the data for the data node  	
        /// </summary>
        /// <MetaDataID>{b234e897-f625-416b-a4a0-b1cc9f44966e}</MetaDataID>
        public int DataRowIndex
        {
            get
            {
                if (!_DataRowIndex.HasValue)
                    _DataRowIndex = DataNodeRowIndices[DataNode];
                return _DataRowIndex.Value;

            }
        }



        /// <MetaDataID>{6963f2f7-658e-485b-a59a-66056e893ca4}</MetaDataID>
        public readonly Dictionary<DataNode, int> DataNodeRowIndices;

        /// <MetaDataID>{2238dfd9-4f64-48d0-9b1a-a73075b138e1}</MetaDataID>
        public DataRetrieveNodeold(DataNode dataNode, DataRetrieveNode masterDataNode, Dictionary<DataNode, int> dataNodeRowIndices)
        {
            DataNode = dataNode;// DerivedDataNode.GetOrgDataNode(dataNode);
            DataNodeRowIndices = dataNodeRowIndices;
            MasterDataNode = masterDataNode;
        }

        public DataRetrieveNodeold(DataRetrieveNode dataRetrieveNode)
        {
            DataNode = dataRetrieveNode.DataNode;// DerivedDataNode.GetOrgDataNode(dataNode);
            DataNodeRowIndices = dataRetrieveNode.DataNodeRowIndices;
            MasterDataNode = dataRetrieveNode.MasterDataNode;
        }

        internal IDataRow[] DataRows;
        internal int CurrentDataRowIndex;
        internal void SetRows(IDataRow[] dataRows)
        {
            if (dataRows != null)
                DataRows = dataRows;
            else
                DataRows = null;
    
            CurrentDataRowIndex = 0;
        }
    }



}
