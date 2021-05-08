using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{

    ///<summary>
    ///This class collects data from data tree for query result type
    ///</summary>
    /// <MetaDataID>{111898c8-f94f-41d6-adcd-a19026da9f30}</MetaDataID>
    [Serializable]
    public class QueryResultDataLoader : /*IEnumerator<CompositeRowData>,*/ IEnumerable<CompositeRowData>
    {
        ///<summary>
        ///Defines the ObjecQuery context reference wich connect QueryResultType with objectQuery
        ///The query result can be moved between the master query and distributed query and the query engine can change the QueryResultType context
        ///</summary>
        public QueryResultObjectContextReference ObjectQueryContextReference;


        /// <summary>
        /// Initialize the object with ObjectQuery context and query result type
        /// </summary>
        /// <param name="type">
        /// Defines the query result type 
        /// </param>
        /// <param name="objectQueryContextReference">
        /// Defines the ObjecQuery context reference wich connect QueryResultType with objectQuery
        /// The query result can be moved between the master query and distributed query and the query engine can change the QueryResultType context
        /// </param>
        /// <MetaDataID>{430d8ddb-7ef1-4364-94e1-aba1a44d041a}</MetaDataID>
        public QueryResultDataLoader(QueryResultType type, QueryResultObjectContextReference objectQueryContextReference)
        {
            ObjectQueryContextReference = objectQueryContextReference;
            TypeIdentity = type.Identity;
        }

        Guid TypeIdentity;

        /// <summary>
        /// Initial object for enumerable member 
        /// </summary>
        /// <param name="type">
        /// Defines the query result type 
        /// </param>
        /// <param name="objectQueryContextReference">
        /// Defines the ObjecQuery context reference wich connect QueryResultType with objectQuery
        /// The query result can be moved between the master query and distributed query and the query engine can change the QueryResultType context
        /// </param>
        /// <param name="dataNodesRows">
        /// Defines a dictionary with the rows of DataNodes of parent query result type data loader
        /// </param>
        /// <param name="parentQueryResultDataLoader">
        /// Defines the data loader of query result type where member is enumerable memember
        /// </param>
        /// <param name="ownerEnumerableMember">
        /// Defines the enumerable member where data loader loads the data
        /// </param>
        /// <MetaDataID>{8576c43e-f077-4a2f-910a-82fb7d3efba5}</MetaDataID>
        public QueryResultDataLoader(QueryResultType type,
            QueryResultObjectContextReference objectQueryContextReference,
            DataNodesRows dataNodesRows,
            QueryResultDataLoader parentQueryResultDataLoader,
            EnumerablePart ownerEnumerableMember)
        {
            OwnerEnumerableMember = ownerEnumerableMember;
            TypeIdentity = type.Identity;
            ObjectQueryContextReference = objectQueryContextReference;
            DataNodesRows = dataNodesRows;
            if (DataNodesRows != null)
            {

                _DataRetrievePath = new LinkedList<DataRetrieveNode>();

                //Force QueryResultType to load DataRetrievePath
                LinkedList<DataRetrieveNode> dataRetrievePath = Type.DataRetrievePath;
                _DataNodeRowIndices = new Dictionary<DataNode, int>(Type.DataNodeRowIndices);

                foreach (var pathNode in dataRetrievePath)
                {
                    DataRetrieveNode masterDataNode = pathNode.MasterDataNode;
                    if (masterDataNode != null)
                        masterDataNode = DataRetrieveNode.GetDataRetrieveNode(masterDataNode.DataNode, _DataRetrievePath);
                    _DataRetrievePath.AddLast(new DataRetrieveNode(pathNode.DataNode, masterDataNode, DataNodeRowIndices, _DataRetrievePath));
                }


                #region  In case where there is a gap between the parentQueryResultDataLoader DataNodes and Type.RootDataNode the data Loader must rebuild  DataRetrievePath

                //Get the the data nodes where there are not in DataRetrievePath and are necessary to get the related data to the  parentQueryResultDataLoader DataNodesRows
                List<DataNode> missingDataNodes = new List<DataNode>();
                {
                    DataNode dataNode = Type.RootDataNode;
                    while (dataNode.ParentDataNode != null && !parentQueryResultDataLoader.DataRetrievePathContainsDataNode(dataNode.ParentDataNode))
                    {
                        missingDataNodes.Insert(0, dataNode.ParentDataNode);
                        dataNode = dataNode.ParentDataNode;
                    }
                }
                if (missingDataNodes.Count > 0)
                {
                    var rootDataRetrieveNode = _DataRetrievePath.First;
                    LinkedListNode<DataRetrieveNode> masterDataRetrieveNode = null;
                    foreach (var dataNode in missingDataNodes)
                    {
                        DataRetrieveNode existingDataRetrieveNode = DataRetrieveNode.GetDataRetrieveNode(dataNode, _DataRetrievePath);
                        LinkedListNode<DataRetrieveNode> dataRetrieveDataNode = null;

                        if (existingDataRetrieveNode == null)
                            _DataNodeRowIndices[dataNode] = _DataRetrievePath.Count;

                        if (masterDataRetrieveNode == null)
                            dataRetrieveDataNode = _DataRetrievePath.AddFirst(new DataRetrieveNode(dataNode, null, DataNodeRowIndices, _DataRetrievePath));
                        else
                            dataRetrieveDataNode = _DataRetrievePath.AddAfter(masterDataRetrieveNode, new DataRetrieveNode(dataNode, masterDataRetrieveNode.Value, DataNodeRowIndices, _DataRetrievePath));

                        if (existingDataRetrieveNode != null)
                        {
                            #region Replace DataRetrieveNode that already exist with the new DataRetrieveNode
                            var dataRetrieveNodes = (from dataRetrieveNode in _DataRetrievePath
                                                     where dataRetrieveNode.MasterDataNode == existingDataRetrieveNode
                                                     select dataRetrieveNode).ToArray();

                            foreach (var dataRetrieveNode in dataRetrieveNodes)
                                dataRetrieveNode.MasterDataNode = dataRetrieveDataNode.Value;
                            _DataRetrievePath.Remove(existingDataRetrieveNode);

                            #endregion
                        }
                        masterDataRetrieveNode = dataRetrieveDataNode;
                    }
                    rootDataRetrieveNode.Value.MasterDataNode = masterDataRetrieveNode.Value;

                }
                #endregion
            }
            else
            {

            }
        }

        /// <summary>
        /// Defines the type of data which loads the data loader
        /// </summary>
        [Association("", Roles.RoleB, "6d966e6e-bf2a-47fa-9e27-2f1cee4539a2")]
        [IgnoreErrorCheck]
        public QueryResultType Type
        {
            get
            {
                return ObjectQueryContextReference.GetType(TypeIdentity);
            }
        }

        /// <summary>
        /// Defines the enumerable member where data loader loads the data
        /// </summary>
        EnumerablePart OwnerEnumerableMember;

        ///<summary>
        /// Defines a dictionary with the rows of DataNodes of parent query result type data loader
        ///</summary>
        /// <MetaDataID>{e52711a1-971c-43ae-925d-fb669752ecf0}</MetaDataID>
        public DataNodesRows DataNodesRows;




        /// <summary>
        /// Check if dataNode is member of DataRetrievePath
        /// </summary>
        /// <param name="dataNode"></param>
        /// <returns></returns>
        bool DataRetrievePathContainsDataNode(DataNode dataNode)
        {
            if ((from dataRetrieveNode in _DataRetrievePath
                 where dataRetrieveNode.DataNode == dataNode
                 select dataRetrieveNode).Count() > 0)
                return true;
            else
                return false;

        }


        /// <exclude>Excluded</exclude>
        [NonSerialized]
        System.Collections.Generic.LinkedList<DataRetrieveNode> _DataRetrievePath;

        /// <summary>
        /// Defines the path on data tree where the system use to retrieve the values of objects of this dynamic type. 
        /// </summary>
        /// <MetaDataID>{65c92d9c-266f-4484-b26d-43100f2cd56f}</MetaDataID>
        System.Collections.Generic.LinkedList<DataRetrieveNode> DataRetrievePath
        {
            get
            {
                if (_DataRetrievePath == null)
                {
                    _DataRetrievePath = new LinkedList<DataRetrieveNode>();
                    foreach (var pathNode in Type.DataRetrievePath)
                        DataRetrievePath.AddLast(new DataRetrieveNode(pathNode, _DataRetrievePath));

                    _DataNodeRowIndices = Type.DataNodeRowIndices;
                }
                return _DataRetrievePath;
            }
        }

        ///<summary>
        ///This dictionary has the row indices on composite row for each DataNode of query result type  
        ///</summary>
        /// <exclude>Excluded</exclude>
        Dictionary<DataNode, int> _DataNodeRowIndices;
        Dictionary<DataNode, int> DataNodeRowIndices
        {
            get
            {
                if (_DataNodeRowIndices == null)
                {
                    bool loaded = DataRetrievePath != null;
                }
                return _DataNodeRowIndices;
            }
        }

        /// <exclude>Excluded</exclude>
        [NonSerialized]
        IDataRow _ExtensionRow;


        /// <summary>
        /// Extension row keeps the data which built dynamically.
        /// Actually keeps the values of dynamic type properties.
        /// </summary>
        /// <MetaDataID>{ab1c9596-befc-4515-8880-0e1d323e1d26}</MetaDataID>
        private IDataRow ExtensionRow
        {
            get
            {
                if (_ExtensionRow == null)
                {
                    IDataTable table = DataSource.DataObjectsInstantiator.CreateDataTable();
                    for (int i = 0; i != Type.ExtensionRowColumnsCount; i++)
                        table.Columns.Add("column_" + i.ToString(), typeof(object));
                    _ExtensionRow = table.NewRow();
                }
                return _ExtensionRow;
            }
        }

        /// <exclude>Excluded</exclude>
        internal List<CompositeRowData> _CompositeRows;

        /// <summary>
        /// Defines a collection with composite rows where its item contains the data for query result type instance
        /// </summary>
        internal List<CompositeRowData> CompositeRows
        {
            get
            {
                return _CompositeRows;
            }
            set
            {
                _CompositeRows = value;
            }
        }


        ///<summary>
        ///Loads composite rows 
        ///</summary>
        /// <MetaDataID>{6477b1e0-0c00-4fa3-9af6-045b675acbb4}</MetaDataID>
        internal void LoadData()
        {

            if (DataRetrievePath.Count != 0)
            {
                if (Type.IsGroupingType)
                {
                    GroupingEntry groupingEntry = DataNodesRows.CompositeRow[Type.Members[1].PartIndices[0]][Type.Members[1].PartIndices[1]] as GroupingEntry;
                    if (groupingEntry != null)
                        CompositeRows = groupingEntry.GroupedCompositeRows;

                    if (Type.OrderByFilter != null)
                        CompositeRows = Type.OrderByFilter.SortCompositeRows(CompositeRows, Type);

                    return;
                }

                bool _continue = LoadNextCompositeRow();
                while (_continue)
                    _continue = LoadNextCompositeRow();

                if (Type.OrderByFilter != null)
                    CompositeRows = Type.OrderByFilter.SortCompositeRows(CompositeRows, Type);

            }
        }


        ///<summary>
        ///Retrieves related rows, the master row is one from rows of composite row.
        ///</summary>
        ///<param name="compositeRow">
        ///Defines composite row which contains the master row.
        ///</param>
        ///<param name="dataNodeRowIndices">
        ///dataNodeRowIndices dictionary keeps the row index in composite row for each DataNode.
        ///</param>
        ///<param name="detailsDataNode">
        ///Defines details rows DataNode
        ///</param>
        ///<param name="masterDataNode">
        ///Defines master row DataNode
        ///</param>
        /// <MetaDataID>{79d8b0ed-fd3e-474f-8241-461f2fbc618b}</MetaDataID>
        private ICollection<IDataRow> RetrieveRelatedRows(DataNode masterDataNode, DataNode detailsDataNode, DataNodesRows dataNodesRows)
        {
            if (dataNodesRows[masterDataNode] == null)
                return new List<IDataRow>();

            return masterDataNode.DataSource.GetRelatedRows(dataNodesRows[masterDataNode].DataRow, detailsDataNode);
        }


        /// <summary>
        /// This method get one composite row each where call it.
        /// The composite row has the data for on query result type instance
        /// If there isn't any more data return false other wise return true
        /// </summary>
        /// <returns>
        /// If reach to the end of data returns false otherwise return true
        /// </returns>
        /// <MetaDataID>{e574280e-d70b-481f-acd4-ee384f7b85df}</MetaDataID>
        internal bool LoadNextCompositeRow()
        {
            lock (this)
            {
                if (CompositeRowsLoaded)
                    return false;
                if (CompositeRows == null)
                {
                    if (DataNodesRows == null)
                    {
                        if (DataRetrievePath.First.Value.DataNode.DataSource.DataTable == null)
                            DataRetrievePath.First.Value.SetRows(new IDataRow[0]);
                        else
                            DataRetrievePath.First.Value.SetRows(DataRetrievePath.First.Value.DataNode.DataSource.DataTable.Rows.ToArray());
                    }
                    else
                    {
                        if (DataRetrievePath.First.Value.DataNode.ParentDataNode == null)
                            DataRetrievePath.First.Value.SetRows(new IDataRow[1] { DataNodesRows[DataRetrievePath.First.Value.DataNode].DataRow });
                        else
                            DataRetrievePath.First.Value.SetRows(DataRetrievePath.First.Value.DataNode.ParentDataNode.DataSource.GetRelatedRows(DataNodesRows[DataRetrievePath.First.Value.DataNode.ParentDataNode].DataRow, DataRetrievePath.First.Value.DataNode).ToArray());
                    }
                    CompositeRows = new List<CompositeRowData>();

                    if (Type.IsGroupingType)
                    {
                        if (DataNodesRows != null)
                        {
                            GroupingEntry groupingEntry = DataNodesRows.CompositeRow[Type.Members[1].PartIndices[0]][Type.Members[1].PartIndices[1]] as GroupingEntry;
                            if (groupingEntry != null)
                                CompositeRows = groupingEntry.GroupedCompositeRows;
                        }
                    }
                }

                var compositeRow = GetCompositeRow();
                while (compositeRow != null && compositeRow[DataNodeRowIndices[Type.MembersRootDataNode]] == null)
                    compositeRow = GetCompositeRow();

                if (compositeRow != null)
                {
                    LoadMembersValue(compositeRow);
                    compositeRow.EndofLoad();
                    CompositeRows.Add(compositeRow);
                    return true;
                }
                else
                {
                    CompositeRowsLoaded = true;
                    return false;
                }
            }
        }
        /// <summary>
        /// Retrieves the data for the next composite row 
        /// </summary>
        /// <returns>
        /// Reternus the composite row
        /// </returns>
        /// <MetaDataID>{bcfdc913-1904-433f-bb6d-63cabf1229da}</MetaDataID>
        internal CompositeRowData GetCompositeRow()
        {
            bool compositeRowPassFilter = false;
            CompositeRowData compositeRow = null;
            if (DataRetrievePath.First.Value.DataRows == null || DataRetrievePath.First.Value.CurrentDataRowIndex == DataRetrievePath.First.Value.DataRows.Length)
                return compositeRow;
            if (Type.ExtensionRowColumnsCount > 0)
            {
                compositeRow = new CompositeRowData(new IDataRow[DataRetrievePath.Count + 1]);
                compositeRow.CompositeRow[DataRetrievePath.Count] = ExtensionRow.Table.NewRow();
            }
            else
                compositeRow = new CompositeRowData(new IDataRow[DataRetrievePath.Count]);
            bool next = LoadCompositeRow(ref compositeRow, DataRetrievePath.First, out compositeRowPassFilter);
            return compositeRow;
        }

        ///// <summary>
        ///// Loads on composite row the data for query result type members 
        ///// </summary>
        ///// <param name="compositeRow">
        ///// Defines the composite row which keeps the data for query result type instance 
        ///// </param>
        ///// <param name="type">
        ///// Defienes
        ///// </param>
        ///// <MetaDataID>{48cbf383-8c03-4ef1-bc9c-79b70d6d58e1}</MetaDataID>
        //private void LoadMembersValue(System.Data.DataRow[] compositeRow, QueryResultType type)
        //{
        //    foreach (QueryResultPart member in type.Members)
        //    {
        //        if (member is CompositePart)
        //            LoadMembersValue(compositeRow, (member as CompositePart).Type);
        //        if (member is EnumerablePart)
        //            compositeRow[type.MembersIndices[member][0]][type.MembersIndices[member][1]] = new QueryResultDataLoader((member as EnumerablePart).Type, ObjectQueryContextReference, new DataNodesRows(new CompositeRowData(compositeRow), DataNodeRowIndices, DataNodesRows), this, member as EnumerablePart);
        //    }
        //}



        /// <summary>
        /// Loads on composite row the data for query result type members 
        /// </summary>
        /// <param name="compositeRow">
        /// Defines the composite row which keeps the data for query result type instance 
        /// </param>
        /// <MetaDataID>{fa58edd5-dadb-4989-9e55-823eafe7cad6}</MetaDataID>
        private void LoadMembersValue(CompositeRowData compositeRow)
        {
            foreach (QueryResultPart member in Type.Members)
            {
                if (member is CompositePart)
                    (member as CompositePart).Type.DataLoader.LoadMembersValue(compositeRow);
                if (member is EnumerablePart)
                {
                    compositeRow[Type.MembersIndices[member][0]][Type.MembersIndices[member][1]] = new QueryResultDataLoader((member as EnumerablePart).Type, ObjectQueryContextReference, new DataNodesRows(compositeRow, DataNodeRowIndices, DataNodesRows), this, member as EnumerablePart);
                    (compositeRow[Type.MembersIndices[member][0]][Type.MembersIndices[member][1]] as QueryResultDataLoader).LoadData();
                }
            }
        }

        /// <summary>
        /// If composite rows loaded then is true otherwise is false
        /// </summary>
        /// <MetaDataID>{6c01c528-c43e-4935-9e20-16ba3c2e6170}</MetaDataID>
        bool CompositeRowsLoaded = false;

        //private bool LoadCompositeRow(CompositeRowData compositeRow, LinkedListNode<DataRetrieveNode> linkedListNode)
        //{
        //    if (linkedListNode == null)
        //        return false;//reach to the end
        //    if (linkedListNode.Value.DataRows == null)
        //        linkedListNode.Value.SetRows(RetrieveRelatedRows(linkedListNode.Value.MasterDataNode.DataNode, linkedListNode.Value.DataNode, new DataNodesRows(compositeRow, Type.DataNodeRowIndices)).ToArray());

        //    if (linkedListNode.Value.DataRows.Length == 0)
        //    {
        //        linkedListNode.Value.SetRows(null);
        //        return false;
        //    }

        //    compositeRow.CompositeRow[Type.DataNodeRowIndices[linkedListNode.Value.DataNode]] = linkedListNode.Value.DataRows[linkedListNode.Value.CurrentDataRowIndex];
        //    if (!LoadCompositeRow(compositeRow, linkedListNode.Next))
        //        linkedListNode.Value.CurrentDataRowIndex++;

        //    if (linkedListNode.Value.CurrentDataRowIndex == linkedListNode.Value.DataRows.Length)
        //    {
        //        linkedListNode.Value.SetRows(null);
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// Travers the link list and load compositerow with the related rows
        /// </summary>
        /// <param name="compositeRow">
        /// Defines the DataRow array where the method load the datarow for DataRetrieveNode   
        /// </param>
        /// <param name="linkedListNode">
        /// Defines a linked list node where the value is a  DataRetrieveNode
        /// </param>
        /// <param name="compositeRowPassFilter">
        /// Defines a output parameter which at the end of call tells if the loaded composite row pasa the data filter.
        /// </param>
        /// <returns>
        /// If there are remaining rows in DataRetrieveNode to load on the next compositerow returns true
        /// If reach to the end and DataRetrieveNode reseted return false.
        /// </returns>
        /// <MetaDataID>{98c27d6b-53bb-465b-9d45-0ba80b228466}</MetaDataID>
        private bool LoadCompositeRow(ref CompositeRowData compositeRow, LinkedListNode<DataRetrieveNode> linkedListNode, out bool compositeRowPassFilter)
        {
            if (Type.DataFilter == null)
                compositeRowPassFilter = true;
            else
                compositeRowPassFilter = false;

            if (linkedListNode.Value.IsReset)
                linkedListNode.Value.SetRows(RetrieveRelatedRows(linkedListNode.Value.MasterDataNode.DataNode, linkedListNode.Value.DataNode, new DataNodesRows(compositeRow, DataNodeRowIndices)).ToArray());
            do
            {
                if (linkedListNode.Value.DataRows.Length != 0)
                {
                    compositeRow.CompositeRow[DataNodeRowIndices[linkedListNode.Value.DataNode]] = linkedListNode.Value.DataRows[linkedListNode.Value.CurrentDataRowIndex];
                    int rtr = 0;
                }

                if (linkedListNode.Next == null)
                {
                    compositeRowPassFilter = Type.DoesRowPassCondition(compositeRow.CompositeRow, linkedListNode);
                    linkedListNode.Value.CurrentDataRowIndex++;
                }
                else
                {
                    compositeRowPassFilter = Type.DoesRowPassCondition(compositeRow.CompositeRow, linkedListNode);
                    if (compositeRowPassFilter)
                    {
                        if (!LoadCompositeRow(ref compositeRow, linkedListNode.Next, out compositeRowPassFilter))
                            linkedListNode.Value.CurrentDataRowIndex++;
                    }
                    else
                        linkedListNode.Value.CurrentDataRowIndex++;

                }
                //Go to the next row in case where the current compositeRow doesn't pass the DataFilter
            }
            while (!compositeRowPassFilter && !linkedListNode.Value.IsReset);

            if (linkedListNode.Value.IsReset)//end of DataRetrieveNodeRows
            {
                if (linkedListNode.List.First == linkedListNode && !compositeRowPassFilter)
                    compositeRow = null;
                return false;
            }
            if (linkedListNode.Value.OnlyForDataFilter && compositeRowPassFilter)//DataRetrieveNodeRows created only for data filter
            {
                linkedListNode.Value.Reset();
                return false;
            }

            return true;

        }

        /// <MetaDataID>{f61380a9-d438-4615-be58-0968e0490cc2}</MetaDataID>
        public IEnumerator GetEnumerator()
        {
            return new QueryResultLoaderEnumartor(this);
        }
        IEnumerator<CompositeRowData> IEnumerable<CompositeRowData>.GetEnumerator()
        {
            return new QueryResultLoaderEnumartor(this);
        }



        #region Removed code


        //LinkedListNode<DataRetrieveNode>
        //int CompositeRowIndex = 0;
        //[NonSerialized]
        //CompositeRowData CurrentCompositeRow;


        //public object Current
        //{
        //    get 
        //    {
        //        if (MasterTypeDataLoader != null)
        //            return MasterTypeDataLoader.Current;
        //        else
        //            return CurrentCompositeRow; 
        //    }
        //}



        //public bool MoveNext()
        //{

        //    if (CompositeRowsEnum != null)
        //    {
        //        bool ret = CompositeRowsEnum.MoveNext();
        //        if (ret)
        //            CurrentCompositeRow = CompositeRowsEnum.Current;
        //        return ret;
        //    }
        //    else
        //    {

        //        var compositeRow = GetCurrentCompositeRow();

        //        while(compositeRow !=null&&compositeRow[Type.DataNodeRowIndices[Type.MembersRootDataNode]]==null)
        //            compositeRow = GetCurrentCompositeRow();

        //        if (compositeRow != null)
        //        {
        //            CurrentCompositeRow = compositeRow;
        //            LoadMembersValue(CurrentCompositeRow);
        //            CurrentCompositeRow.EndofLoad();
        //            return true;
        //        }
        //        else
        //        {
        //            CurrentCompositeRow = null;
        //            return false;
        //        }
        //    }
        //}


        //QueryResultDataLoader _MasterTypeDataLoader;
        //public QueryResultDataLoader MasterTypeDataLoader
        //{
        //    get
        //    {
        //        if (_MasterTypeDataLoader != null && _MasterTypeDataLoader.MasterTypeDataLoader != null)
        //            return _MasterTypeDataLoader.MasterTypeDataLoader;
        //        else
        //            return _MasterTypeDataLoader;
        //    }
        //    set
        //    {
        //        _MasterTypeDataLoader = value;
        //    }
        //}



        #region IEnumerator Members
        //public void Reset()
        //{

        //    if (DataRetrievePath.Count != 0)
        //    {
        //        foreach (var dataRetrieveNode in DataRetrievePath)
        //            dataRetrieveNode.CurrentDataRowIndex = 0;


        //        int nextColumnIndexInExtensionRow = 0;
        //        //BuildPropertiesDataIndices(ref nextColumnIndexInExtensionRow, DataNodeRowIndices, Type.DataRetrievePath);
        //        //ExtensionRowColumnsCount = nextColumnIndexInExtensionRow;
        //        if (DataNodesRows == null)
        //            DataRetrievePath.First.Value.SetRows(DataRetrievePath.First.Value.DataNode.DataSource.DataTable.Rows.OfType<System.Data.DataRow>().ToArray());
        //        else
        //            DataRetrievePath.First.Value.SetRows(DataRetrievePath.First.Value.DataNode.ParentDataNode.DataSource.GetRelatedRows(DataNodesRows[DataRetrievePath.First.Value.DataNode.ParentDataNode].DataRow, DataRetrievePath.First.Value.DataNode).ToArray());

        //    }
        //}

        #endregion

        #region IEnumerator<DataRow[]> Members

        //public CompositeRowData Current
        //{
        //    get
        //    {
        //        if (MasterTypeDataLoader != null)
        //            return MasterTypeDataLoader.Current;
        //        else
        //            return CurrentCompositeRow;
        //    }
        //}

        #endregion

        #region IDisposable Members

        //public void Dispose()
        //{

        //}

        #endregion

        #region IEnumerator Members

        //object IEnumerator.Current
        //{
        //    get { return Current; }
        //}

        #endregion

        #endregion

        /// <summary>
        /// Merge composite rows
        /// </summary>
        /// <param name="queryResultDataLoader">
        /// Defines the queryResult dataLoader which contains the extra compositeRows
        /// </param>
        internal void Merge(QueryResultDataLoader queryResultDataLoader)
        {
            queryResultDataLoader.ObjectQueryContextReference.ObjectQueryContext = this.ObjectQueryContextReference.ObjectQueryContext;

            if (Type.IsGroupingType)
            {
                CompositeRows.AddRange(queryResultDataLoader.CompositeRows);
            }
            else if (Type.RootDataNode == Type.RootDataNode.HeaderDataNode && Type.RootDataNode.Type == DataNode.DataNodeType.Group)
            {
                if (CompositeRows.Count == 0)
                {
                    CompositeRows.AddRange(queryResultDataLoader.CompositeRows);
                }
                else if (Type.RootDataNode.ParentDataNode == null && CompositeRows.Count == 1 && queryResultDataLoader.CompositeRows.Count == 1 && (Type.RootDataNode as GroupDataNode).GroupKeyDataNodes.Count == 0)
                {
                    foreach (QueryResultPart member in Type.Members)
                    {
                        if (member.SourceDataNode.Type == DataNode.DataNodeType.Average)
                        {
                            AverageValue averageValue = CompositeRows[0][member.PartIndices[0]][member.PartIndices[1]] as AverageValue;
                            decimal avrgSum = averageValue.Average * averageValue.AverageCount;
                            if (averageValue.AverageCount == -1)
                                throw new Exception("bad average data");
                            int count = averageValue.AverageCount;
                            AverageValue mergeAverageValue = queryResultDataLoader.CompositeRows[0][member.PartIndices[0]][member.PartIndices[1]] as AverageValue;

                            if (mergeAverageValue.AverageCount == -1)
                                throw new Exception("bad average data");
                            count += mergeAverageValue.AverageCount;
                            avrgSum += mergeAverageValue.Average * mergeAverageValue.AverageCount;

                            averageValue.Average = avrgSum / count;
                            averageValue.AverageCount = count;
                            CompositeRows[0][member.PartIndices[0]][member.PartIndices[1]] = averageValue;
                        }
                    }

                    switch (Type.ValueDataNode.Type)
                    {
                        case DataNode.DataNodeType.Count:
                            {

                                int count = (int)(CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex]);
                                count += (int)(queryResultDataLoader.CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex]);
                                CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex] = count;

                                break;
                            }
                        case DataNode.DataNodeType.Sum:
                            {

                                decimal sum = (decimal)(CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex]);
                                sum += (decimal)(queryResultDataLoader.CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex]);
                                CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex] = sum;

                                break;
                            }
                        case DataNode.DataNodeType.Average:
                            {

                                break;
                            }
                        case DataNode.DataNodeType.Min:
                            {

                                decimal min = (decimal)(CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex]);
                                decimal mergeMin = (decimal)(queryResultDataLoader.CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex]);
                                if (mergeMin < min)
                                    CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex] = mergeMin;

                                break;
                            }
                        case DataNode.DataNodeType.Max:
                            {
                                decimal max = (decimal)(CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex]);
                                decimal mergeMax = (decimal)(queryResultDataLoader.CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex]);
                                if (mergeMax > max)
                                    CompositeRows[0][Type.ConventionTypeRowIndex][Type.ConventionTypeColumnIndex] = mergeMax;

                                break;
                            }

                    }

                }
                else
                {
                    var groupEntryIndices = Type.RootDataNodeIndices;
                    Dictionary<MultiPartKey, List<CompositeRowData>> groupingData = new Dictionary<MultiPartKey, List<CompositeRowData>>();
                    foreach (var compositeRowData in CompositeRows)
                    {
                        MultiPartKey key = (compositeRowData[groupEntryIndices[0]][groupEntryIndices[1]] as GroupingEntry).GroupingKey;
                        List<CompositeRowData> compositeRowDataCollection = null;
                        if (!groupingData.TryGetValue(key, out compositeRowDataCollection))
                        {
                            compositeRowDataCollection = new List<CompositeRowData>();
                            groupingData[key] = compositeRowDataCollection;
                        }
                        compositeRowDataCollection.Add(compositeRowData);
                    }
                    foreach (var compositeRowData in queryResultDataLoader.CompositeRows)
                    {
                        MultiPartKey key = (compositeRowData[groupEntryIndices[0]][groupEntryIndices[1]] as GroupingEntry).GroupingKey;
                        List<CompositeRowData> compositeRowDataCollection = null;
                        if (!groupingData.TryGetValue(key, out compositeRowDataCollection))
                        {
                            compositeRowDataCollection = new List<CompositeRowData>();
                            groupingData[key] = compositeRowDataCollection;
                            CompositeRows.Add(compositeRowData);
                        }
                        compositeRowDataCollection.Add(compositeRowData);
                    }
                    foreach (var entry in groupingData)
                    {
                        if (entry.Value.Count > 1)
                        {
                            foreach (QueryResultPart member in Type.Members)
                            {
                                if (member.SourceDataNode.Type == DataNode.DataNodeType.Average)
                                {
                                    AverageValue averageValue = (entry.Value[0][member.PartIndices[0]][member.PartIndices[1]]) as AverageValue;
                                    decimal avrgSum = averageValue.Average * averageValue.AverageCount;
                                    if (averageValue.AverageCount == -1)
                                        throw new Exception("bad average data");
                                    int count = averageValue.AverageCount;
                                    for (int i = 1; i < entry.Value.Count; i++)
                                    {
                                        AverageValue mergeAverageValue = entry.Value[i][member.PartIndices[0]][member.PartIndices[1]] as AverageValue;
                                        if (mergeAverageValue.AverageCount == -1)
                                            throw new Exception("bad average data");
                                        count += mergeAverageValue.AverageCount;
                                        avrgSum += mergeAverageValue.Average * mergeAverageValue.AverageCount;
                                    }
                                    averageValue.Average = avrgSum / count;
                                    averageValue.AverageCount = count;
                                    entry.Value[0][member.PartIndices[0]][member.PartIndices[1]] = averageValue;
                                }
                            }
                            foreach (QueryResultPart member in Type.Members)
                            {
                                switch (member.SourceDataNode.Type)
                                {
                                    case DataNode.DataNodeType.Count:
                                        {
                                            int count = (int)(entry.Value[0][member.PartIndices[0]][member.PartIndices[1]]);
                                            for (int i = 1; i < entry.Value.Count; i++)
                                                count += (int)(entry.Value[i][member.PartIndices[0]][member.PartIndices[1]]);
                                            entry.Value[0][member.PartIndices[0]][member.PartIndices[1]] = count;

                                            break;
                                        }
                                    case DataNode.DataNodeType.Sum:
                                        {
                                            decimal sum = (decimal)(entry.Value[0][member.PartIndices[0]][member.PartIndices[1]]);
                                            for (int i = 1; i < entry.Value.Count; i++)
                                                sum += (decimal)(entry.Value[i][member.PartIndices[0]][member.PartIndices[1]]);
                                            entry.Value[0][member.PartIndices[0]][member.PartIndices[1]] = sum;
                                            break;
                                        }
                                    case DataNode.DataNodeType.Average:
                                        {

                                            break;
                                        }
                                    case DataNode.DataNodeType.Min:
                                        {
                                            decimal min = (decimal)(entry.Value[0][member.PartIndices[0]][member.PartIndices[1]]);
                                            for (int i = 1; i < entry.Value.Count; i++)
                                            {
                                                decimal value = (decimal)(entry.Value[i][member.PartIndices[0]][member.PartIndices[1]]);
                                                if (value < min)
                                                    min = value;
                                            }
                                            entry.Value[0][member.PartIndices[0]][member.PartIndices[1]] = min;
                                            break;

                                        }
                                    case DataNode.DataNodeType.Max:
                                        {

                                            decimal max = (decimal)(entry.Value[0][member.PartIndices[0]][member.PartIndices[1]]);
                                            for (int i = 1; i < entry.Value.Count; i++)
                                            {
                                                decimal value = (decimal)(entry.Value[i][member.PartIndices[0]][member.PartIndices[1]]);
                                                if (value > max)
                                                    max = value;
                                            }
                                            entry.Value[0][member.PartIndices[0]][member.PartIndices[1]] = max;
                                            break;
                                        }



                                }

                            }
                            foreach (QueryResultPart member in Type.Members)
                            {
                                if ((member is EnumerablePart) && (member as EnumerablePart).Type.IsGroupingType)
                                {
                                    //decimal sum = (decimal)(entry.Value[0][member.PartIndices[0]][member.PartIndices[1]]);
                                    for (int i = 1; i < entry.Value.Count; i++)
                                    {
                                        QueryResultDataLoader GroupedDataLoader = (QueryResultDataLoader)(entry.Value[i][member.PartIndices[0]][member.PartIndices[1]]);

                                        ((QueryResultDataLoader)entry.Value[0][member.PartIndices[0]][member.PartIndices[1]]).Merge(GroupedDataLoader);
                                    }

                                    break;
                                }

                            }
                        }
                    }


                }



            }
            else
            {

                CompositeRows.AddRange(queryResultDataLoader.CompositeRows);
            }

        }




        internal QueryResultDataLoader Clone(Dictionary<object, object> clonedObjects)
        {
            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as QueryResultDataLoader;

            QueryResultDataLoader newQueryResultDataLoader = new QueryResultDataLoader(Type.Clone(clonedObjects), ObjectQueryContextReference.Clone(clonedObjects));
            clonedObjects[this] = newQueryResultDataLoader;


            return newQueryResultDataLoader;
        }
    }


    /// <MetaDataID>{a3d322be-1aa0-40b5-ba30-6088e0be8553}</MetaDataID>
    class QueryResultLoaderEnumartor : IEnumerator<CompositeRowData>
    {
        /// <MetaDataID>{ddf4e03c-609b-4894-8129-ff8a6ade701f}</MetaDataID>
        internal QueryResultDataLoader QueryResultDataLoader;
        /// <MetaDataID>{6564c88b-9faa-4180-8805-041efd38fb13}</MetaDataID>
        int i = 0;
        /// <MetaDataID>{e8fa2f10-2515-4ce8-9626-b86957708cae}</MetaDataID>
        public QueryResultLoaderEnumartor(QueryResultDataLoader queryResultDataLoader)
        {
            QueryResultDataLoader = queryResultDataLoader;
        }

        /// <MetaDataID>{99e311db-b404-4a04-9fd9-9d8920fc0086}</MetaDataID>
        public CompositeRowData Current
        {
            get { return _Current; }
        }


        /// <MetaDataID>{16957618-175c-471f-b701-1acd0ce41cd8}</MetaDataID>
        public void Dispose()
        {

        }

        /// <MetaDataID>{804036be-6603-499f-b63f-c8640692b7de}</MetaDataID>
        CompositeRowData _Current;
        /// <MetaDataID>{5f592bcf-cd40-4c1e-b5f5-af6b026fa07a}</MetaDataID>
        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        /// <MetaDataID>{eb726e2c-807b-4f7a-aee9-a9d82a989ba0}</MetaDataID>
        public bool MoveNext()
        {
            lock (QueryResultDataLoader)
            {
                if (QueryResultDataLoader.CompositeRows != null)
                    if (i < QueryResultDataLoader.CompositeRows.Count)
                    {
                        _Current = QueryResultDataLoader.CompositeRows[i];
                        i++;
                        return true;
                    }
                if (QueryResultDataLoader.LoadNextCompositeRow())
                {
                    _Current = QueryResultDataLoader.CompositeRows[i];
                    i++;
                    return true;
                }
                else
                    return false;
            }


        }

        /// <MetaDataID>{69a6bedc-ac42-4bf9-95d3-a503ff4db839}</MetaDataID>
        public void Reset()
        {
            i = 0;
        }


        /// <MetaDataID>{5da2fa90-ee6b-46f7-b462-b6ff7d8f8257}</MetaDataID>
        internal DataNodesRows DataNodesRows
        {
            get
            {
                return QueryResultDataLoader.DataNodesRows;
            }
        }
    }

    /// <MetaDataID>{19824808-dcfc-4a4b-acb4-d8b1efd017cc}</MetaDataID>
    [Serializable]
    public class CompositeRowData
    {
        /// <MetaDataID>{4c3712a1-5386-4b0f-8285-58e37148d638}</MetaDataID>
        RowData[] CompositeRowVales;
        /// <MetaDataID>{f6cb2c1a-e125-44e0-bd97-06cf4827b1eb}</MetaDataID>
        public CompositeRowData(IDataRow[] compositeRow)
        {
            CompositeRow = compositeRow;
            CompositeRowVales = new RowData[CompositeRow.Length];
        }
        /// <MetaDataID>{0fa11cb3-212e-4173-afb9-8594e323578c}</MetaDataID>
        internal void EndofLoad()
        {
            for (int i = 0; i != CompositeRow.Length; i++)
            {
                if (CompositeRow != null && CompositeRow[i] != null)
                    CompositeRowVales[i] = new RowData(CompositeRow[i]);

            }
        }


        /// <MetaDataID>{10f58f5e-e886-4f69-8d6a-83b277aa61f1}</MetaDataID>
        [NonSerialized]
        internal readonly IDataRow[] CompositeRow;
        /// <MetaDataID>{b3ad0611-6513-435b-81c3-38a04bc3bedf}</MetaDataID>
        public RowData this[int indexer]
        {
            get
            {
                if (CompositeRowVales[indexer] == null && CompositeRow != null && CompositeRow[indexer] != null)
                    CompositeRowVales[indexer] = new RowData(CompositeRow[indexer]);
                else if (CompositeRow != null &&
                ((CompositeRowVales[indexer] == null && CompositeRow[indexer] != null) ||
                  (CompositeRowVales[indexer] != null && CompositeRow[indexer] != CompositeRowVales[indexer].DataRow)))
                    CompositeRowVales[indexer] = new RowData(CompositeRow[indexer]);

                return CompositeRowVales[indexer];
            }
        }


    }
    /// <MetaDataID>{4eca56c3-8d28-4c39-b0f6-0669be0ee0e7}</MetaDataID>
    [Serializable]
    public class RowData : System.Runtime.Serialization.IDeserializationCallback
    {
        /// <MetaDataID>{6e78bb19-9608-416a-a67b-4e2ad4479fe2}</MetaDataID>
        object[] DataRowValues;
        /// <MetaDataID>{927a0853-fb9e-4db8-8cfb-9acdd48a9230}</MetaDataID>
        public RowData(IDataRow row)
        {

            DataRow = row;
            DataRowValues = DataRow.ItemArray;
        }
        /// <MetaDataID>{3929750a-e175-4a45-9552-24414ebb0fca}</MetaDataID>
        [NonSerialized]
        internal readonly IDataRow DataRow;
        /// <MetaDataID>{c7d044a3-3f09-418e-ab50-cfbbd16aa35a}</MetaDataID>
        public object this[int indexer]
        {
            get
            {
                if (DataRow == null && DataRowValues != null)
                    return DataRowValues[indexer];

                return DataRow[indexer];
            }
            set
            {
                if (DataRow == null && DataRowValues != null)
                    DataRowValues[indexer] = value;
                else
                {
                    if (DataRowValues != null)
                        DataRowValues[indexer] = value;
                    DataRow[indexer] = value;
                }
            }
        }

        #region IDeserializationCallback Members

        /// <MetaDataID>{c4585a34-5573-4ce8-ba9c-61b0d0077508}</MetaDataID>
        public void OnDeserialization(object sender)
        {
            if (DataRowValues != null)
                for (int i = 0; i != DataRowValues.Length; i++)
                {
#if !DeviceDotNet
                    DataRowValues[i] = OOAdvantech.Remoting.Proxy.ControlLifeTime(DataRowValues[i]);
#endif
                }

        }

        #endregion
    }


    /// <MetaDataID>{a52d9199-f573-49c0-9b13-83206d69e823}</MetaDataID>
    public interface IQueryResultSort
    {
        void quicksort();
        void quicksortDesc();
        List<int> CollectionIndexes { get; }
    }


    /// <MetaDataID>{4b8485a6-f48c-4a8b-a98e-ca5abbae97b5}</MetaDataID>
    public class QueryResultSort<T> : IList<T>, IQueryResultSort
    {

        private QueryResultSort<T> Ref;

        public QueryResultSort(QueryResultSort<T> less)
        {
            if (less.Ref != null)
                this.Ref = less.Ref;
            else
                this.Ref = less;

            SortRowIndex = less.SortRowIndex;
            SortColumnndex = less.SortColumnndex;
        }

        List<CompositeRowData> Data;

        public List<int> _CollectionIndexes = new List<int>();
        public List<int> CollectionIndexes
        {
            get
            {
                return _CollectionIndexes;
            }
        }

        int SortColumnndex;

        int SortRowIndex;
        private int v;

        public static IQueryResultSort GetQueryResultSort(Type type, List<CompositeRowData> data, int rowIndex, int columnIndex)
        {
            if (type == typeof(bool))
                return new QueryResultSort<bool>(data, rowIndex, columnIndex);
            if (type == typeof(int))
                return new QueryResultSort<int>(data, rowIndex, columnIndex);
            if (type == typeof(long))
                return new QueryResultSort<long>(data, rowIndex, columnIndex);
            if (type == typeof(short))
                return new QueryResultSort<short>(data, rowIndex, columnIndex);
            if (type == typeof(uint))
                return new QueryResultSort<uint>(data, rowIndex, columnIndex);
            if (type == typeof(ulong))
                return new QueryResultSort<ulong>(data, rowIndex, columnIndex);
            if (type == typeof(ushort))
                return new QueryResultSort<ushort>(data, rowIndex, columnIndex);
            if (type == typeof(double))
                return new QueryResultSort<double>(data, rowIndex, columnIndex);
            if (type == typeof(decimal))
                return new QueryResultSort<decimal>(data, rowIndex, columnIndex);
            if (type == typeof(DateTime))
                return new QueryResultSort<DateTime>(data, rowIndex, columnIndex);
            if (type == typeof(string))
                return new QueryResultSort<string>(data, rowIndex, columnIndex);
            return null;
        }
        public QueryResultSort(List<CompositeRowData> data, int rowIndex, int columnIndex)
        {
            Data = data;
            SortColumnndex = columnIndex;
            SortRowIndex = rowIndex;
            int count = Data.Count;
            for (int i = 0; i < count; i++)
                CollectionIndexes.Add(i);
        }

        public QueryResultSort(List<CompositeRowData> data, List<int> collectionIndexes, int rowIndex, int columnIndex)
        {
            Data = data;
            _CollectionIndexes = collectionIndexes;
            SortColumnndex = columnIndex;
            SortRowIndex = rowIndex;
        }


        public static IQueryResultSort GetQueryResultSort(Type type, List<CompositeRowData> data, List<int> collectionIndexes, int rowIndex, int columnIndex)
        {
            if (type == typeof(bool))
                return new QueryResultSort<bool>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(int))
                return new QueryResultSort<int>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(long))
                return new QueryResultSort<long>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(short))
                return new QueryResultSort<short>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(uint))
                return new QueryResultSort<uint>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(ulong))
                return new QueryResultSort<ulong>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(ushort))
                return new QueryResultSort<ushort>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(double))
                return new QueryResultSort<double>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(decimal))
                return new QueryResultSort<decimal>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(DateTime))
                return new QueryResultSort<DateTime>(data, collectionIndexes, rowIndex, columnIndex);
            if (type == typeof(string))
                return new QueryResultSort<string>(data, collectionIndexes, rowIndex, columnIndex);
            return null;
        }
        public int Count
        {
            get
            {
                return this.CollectionIndexes.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public T this[int index]
        {
            get
            {
                if (Ref != null)
                    return (T)Ref.Data[CollectionIndexes[index]][SortRowIndex][SortColumnndex];
                else
                    return (T)Data[CollectionIndexes[index]][SortRowIndex][SortColumnndex];



            }
            set { /* set the specified index to value here */ }
        }

        public void RemoveAt(int pos)
        {
            CollectionIndexes.RemoveAt(pos);

        }

        public void Add(int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }


        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            //array = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                object value = this.GetSortValueAsObject(i);
                if (value == null || value is DBNull)
                    array[i] = default(T);
                else
                    array[i] = (T)value;

            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new QueryResultEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new QueryResultEnumerator<T>(this);
        }

        internal T GetSortValue(int index)
        {
            if (Ref != null)
                return (T)Ref.Data[CollectionIndexes[index]][SortRowIndex][SortColumnndex];
            else
                return (T)Data[CollectionIndexes[index]][SortRowIndex][SortColumnndex];


            //    if (Ref!=null)
            //        return (T)Ref.Data[CollectionIndexes[index], sortIndex];
            //    else
            //        return (T)Data[CollectionIndexes[index], sortIndex];
        }

        internal object GetSortValueAsObject(int index)
        {
            if (Ref != null)
            {
                RowData row = Ref.Data[CollectionIndexes[index]][SortRowIndex];
                if (row == null)
                    return DBNull.Value;
                return row[SortColumnndex];
            }
            else
            {
                RowData row = Data[CollectionIndexes[index]][SortRowIndex];
                if (row == null)
                    return DBNull.Value;
                return row[SortColumnndex];
            }
            //    if (Ref!=null)
            //        return (T)Ref.Data[CollectionIndexes[index], sortIndex];
            //    else
            //        return (T)Data[CollectionIndexes[index], sortIndex];
        }

        internal void AddIndexObject(int x)
        {
            if (CollectionIndexes.Contains(x))
            {

            }
            CollectionIndexes.Add(x);

        }

        internal int GetIndexOrg(int i)
        {
            return CollectionIndexes[i];
        }

        public static List<CompositeRowData> ToList(List<CompositeRowData> data, List<int> collectionIndexes)
        {

            return (from index in collectionIndexes
                    select data[index]).ToList();
        }
        public static QueryResultSort<T> quicksort(QueryResultSort<T> a)
        {

            Random r = new Random();
            QueryResultSort<T> less = new QueryResultSort<T>(a);
            QueryResultSort<T> greater = new QueryResultSort<T>(a);

            if (a.Count <= 1)
                return a;

            int pos = r.Next(a.Count);

            object pivotValue = a.GetSortValueAsObject(pos);
            int pivotPos = a.GetIndexOrg(pos);

            //a.RemoveAt(pos);

            int i = 0;
            foreach (object value in (IEnumerable)a)
            {
                //value <= pivotValue

                if (i != pos)
                {

                    int res = 0;
                    if ((value == null || value is DBNull) && (pivotValue == null || pivotValue is DBNull))
                        res = 0;
                    else if (value == null || value is DBNull)
                        res = -1;
                    else if (pivotValue == null || pivotValue is DBNull)
                        res = 1;
                    else
                        res = System.Collections.Generic.Comparer<T>.Default.Compare((T)value, (T)pivotValue);



                    if (res == 0)
                    {
                        if (i < pos)
                            less.AddIndexObject(a.GetIndexOrg(i));
                        else
                            greater.AddIndexObject(a.GetIndexOrg(i));
                    }
                    else if (res == -1)
                    {

                        less.AddIndexObject(a.GetIndexOrg(i));
                        var er = less.GetSortValueAsObject(less.Count - 1);
                        //if (value != er)
                        //{

                        //}
                    }
                    else
                    {
                        greater.AddIndexObject(a.GetIndexOrg(i));
                        var er = greater.GetSortValueAsObject(greater.Count - 1);
                        //if (value != er)
                        //{

                        //}
                    }
                }
                i++;
            }


            return concat(quicksort(less), pivotPos, quicksort(greater)) as QueryResultSort<T>;
        }

        public static QueryResultSort<T> quicksortDesc(QueryResultSort<T> a)
        {

            Random r = new Random();
            QueryResultSort<T> before = new QueryResultSort<T>(a);
            QueryResultSort<T> after = new QueryResultSort<T>(a);

            if (a.Count <= 1)
                return a;

            int pos = r.Next(a.Count);

            object pivotValue = a.GetSortValueAsObject(pos);
            int pivotPos = a.GetIndexOrg(pos);

            //a.RemoveAt(pos);

            int i = 0;
            foreach (object value in (IEnumerable)a)
            {
                //value <= pivotValue

                if (i != pos)
                {
                    int res = 0;
                    if ((value == null || value is DBNull) && (pivotValue == null || pivotValue is DBNull))
                        res = 0;
                    else if (value == null || value is DBNull)
                        res = -1;
                    else if (pivotValue == null || pivotValue is DBNull)
                        res = 1;
                    else
                        res = System.Collections.Generic.Comparer<T>.Default.Compare((T)value, (T)pivotValue);

                    if (res == 0)
                    {
                        if (i < pos)
                            before.AddIndexObject(a.GetIndexOrg(i));
                        else
                            after.AddIndexObject(a.GetIndexOrg(i));
                    }
                    else if (res == 1)
                    {

                        before.AddIndexObject(a.GetIndexOrg(i));
                        var er = before.GetSortValueAsObject(before.Count - 1);
                        //if (value != er)
                        //{

                        //}
                    }
                    else
                    {
                        after.AddIndexObject(a.GetIndexOrg(i));
                        var er = after.GetSortValueAsObject(after.Count - 1);
                        //if (value != er)
                        //{

                        //}
                    }
                }
                i++;
            }


            return concatDesc(quicksortDesc(before), pivotPos, quicksortDesc(after)) as QueryResultSort<T>;
        }

        public static QueryResultSort<T> concatDesc(QueryResultSort<T> before, int pivotPos, QueryResultSort<T> after)
        {

            QueryResultSort<T> sorted = new QueryResultSort<T>(before);
            int i = 0;
            foreach (object value in (IEnumerable)before)
                sorted.AddIndexObject(before.GetIndexOrg(i++));

            sorted.AddIndexObject(pivotPos);
            i = 0;
            foreach (object value in (IEnumerable)after)
                sorted.AddIndexObject(after.GetIndexOrg(i++));
            return sorted;
        }



        public static QueryResultSort<T> concat(QueryResultSort<T> less, int pivotPos, QueryResultSort<T> greater)
        {

            QueryResultSort<T> sorted = new QueryResultSort<T>(less);
            int i = 0;
            foreach (object value in (IEnumerable)less)
                sorted.AddIndexObject(less.GetIndexOrg(i++));

            sorted.AddIndexObject(pivotPos);
            i = 0;
            foreach (object value in (IEnumerable)greater)
                sorted.AddIndexObject(greater.GetIndexOrg(i++));
            return sorted;
        }


        public List<int> bubblesort(List<int> a)
        {
            int temp;

            // foreach(int i in a)
            for (int i = 1; i <= a.Count; i++)
                for (int j = 0; j < a.Count - i; j++)
                    if (a[j] > a[j + 1])
                    {
                        temp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = temp;

                    }




            return a;
        }

        public List<int> mergesort(List<int> m)
        {
            List<int> result = new List<int>();
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            if (m.Count <= 1)
                return m;


            int middle = m.Count / 2;
            for (int i = 0; i < middle; i++)
                left.Add(m[i]);
            for (int i = middle; i < m.Count; i++)
                right.Add(m[i]);

            left = mergesort(left);
            right = mergesort(right);

            if (left[left.Count - 1] <= right[0])
                return append(left, right);

            result = merge(left, right);
            return result;

        }

        public List<int> append(List<int> a, List<int> b)
        {
            List<int> result = new List<int>(a);

            foreach (int x in b)
                result.Add(x);
            return result;
        }

        public List<int> merge(List<int> a, List<int> b)
        {
            List<int> s = new List<int>();
            while (a.Count > 0 && b.Count > 0)
            {
                if (a[0] < b[0])
                {

                    s.Add(a[0]);
                    a.RemoveAt(0);
                }
                else
                {

                    s.Add(b[0]);
                    b.RemoveAt(0);
                }
            }
            while (a.Count > 0)
            {
                s.Add(a[0]);
                a.RemoveAt(0);
            }

            while (b.Count > 0)
            {
                s.Add(b[0]);
                b.RemoveAt(0);
            }
            return s;
        }

        #region "heapsort"



        int[] heapSort(int[] numbers, int array_size)
        {
            int i, temp;

            for (i = (array_size / 2) - 1; i >= 0; i--)
                siftDown(numbers, i, array_size);

            for (i = array_size - 1; i >= 1; i--)
            {
                temp = numbers[0];
                numbers[0] = numbers[i];
                numbers[i] = temp;
                siftDown(numbers, 0, i - 1);
            }

            return numbers;

        }


        void siftDown(int[] numbers, int root, int bottom)
        {
            int done, maxChild, temp;

            done = 0;
            while ((root * 2 <= bottom) && (done == 0))
            {
                if (root * 2 == bottom)
                    maxChild = root * 2;
                else if (numbers[root * 2] > numbers[root * 2 + 1])
                    maxChild = root * 2;
                else
                    maxChild = root * 2 + 1;

                if (numbers[root] < numbers[maxChild])
                {
                    temp = numbers[root];
                    numbers[root] = numbers[maxChild];
                    numbers[maxChild] = temp;
                    root = maxChild;
                }
                else
                    done = 1;
            }


        }

        public void quicksort()
        {
            var sorted = QueryResultSort<T>.quicksort(this);
            _CollectionIndexes = sorted.CollectionIndexes;
        }


        public void quicksortDesc()
        {
            var sorted = QueryResultSort<T>.quicksortDesc(this);
            _CollectionIndexes = sorted.CollectionIndexes;
        }


        #endregion
    }

    /// <MetaDataID>{49fc004a-6d35-4e11-852a-e2c8d3f69892}</MetaDataID>
    class QueryResultEnumerator<T> : IEnumerator<T>
    {

        int index;

        QueryResultSort<T> Source;
        public QueryResultEnumerator(QueryResultSort<T> source)
        {
            index = -1;
            Source = source;
        }

        public T Current
        {
            get
            {
                return Source.GetSortValue(index);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Source.GetSortValueAsObject(index);
            }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            index++;
            if (Source.Count > index)
                return true;
            else
                return false;

        }

        public void Reset()
        {
            index = 0;
        }


    }

}
