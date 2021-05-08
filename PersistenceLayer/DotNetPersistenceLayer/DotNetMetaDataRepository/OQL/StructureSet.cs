namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
    using Remoting;
    using System;
#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif

    /// <MetaDataID>{F5B7322C-F1D3-4CE4-84BB-159E8427FCD8}</MetaDataID>
    public class StructureSet : MarshalByRefObject, Collections.StructureSet, Remoting.IExtMarshalByRefObject
    {

        [System.Serializable]
        public class StructureSetData
        {
            public OOAdvantech.Collections.MemberList Members;
            public Collections.Generic.List<DataLoader.StreamedTable> Data = new Collections.Generic.List<DataLoader.StreamedTable>();
            public Collections.Generic.List<DataLoader.RelationData> Relations = new Collections.Generic.List<DataLoader.RelationData>();
            public string RootTableName;
        }
#if !DeviceDotNet
#endif

        /// <MetaDataID>{f61e6034-7bd1-4d72-ab6c-9cf4bfa725f6}</MetaDataID>
        protected OOAdvantech.Collections.MemberList _Members;
        /// <MetaDataID>{5873ee92-5daf-4a46-aaed-c60a5c5f5996}</MetaDataID>
        public OOAdvantech.Collections.MemberList Members
        {
            get
            {
                return _Members;
            }
        }

        /// <MetaDataID>{0d2cdaa5-153e-4871-ae51-483aa61dc43b}</MetaDataID>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return new RowEnumerator(this);
        }
        /// <MetaDataID>{5af48baa-f959-4cc4-843a-4a34cb3fa08c}</MetaDataID>
        ObjectsContext _SourceStorageSession;
        /// <MetaDataID>{d59781ad-be46-4214-898c-29a777899cbc}</MetaDataID>
        public ObjectsContext SourceStorageSession
        {
            get
            {
                return _SourceStorageSession;
            }
        }

        public bool ContainsMember(string memberName)
        {
            if (DataRowEnumerator.Current is CompositeRowData)
            {
                if (QueryResult.GetMember(memberName) != null)
                    return true;
                else
                    return false;
            }
            return Members != null && Members.Members.ContainsKey(memberName);

        }

        public bool IsDerivedMember(string memberName)
        {
            return Members[memberName].DerivedMember;
        }
        /// <MetaDataID>{38E233C4-B89D-4251-94CF-B264DD270199}</MetaDataID>
        /// <summary>Define an indexer. 
        /// You give the member name and return the value of structure instance member.
        /// If you haven't call the MoveNext method or the MoveNext return false and call indexer then indexer raise exception.</summary>
        public object this[string Index]
        {
            get
            {
                if (DataRowEnumerator.Current is CompositeRowData)
                {
                    QueryResultPart member = QueryResult.GetMember(Index);
                    if (member == null)
                        throw new System.Exception("The given key was not present in the dictionary");
                    if (member is EnumerablePart)
                        return new StructureSet(((DataRowEnumerator.Current as CompositeRowData)[member.PartIndices[0]][member.PartIndices[1]] as QueryResultDataLoader).Type, ((DataRowEnumerator.Current as CompositeRowData)[member.PartIndices[0]][member.PartIndices[1]] as QueryResultDataLoader).GetEnumerator(), mOQLStatement);
                    else
                        return (DataRowEnumerator.Current as CompositeRowData)[member.PartIndices[0]][member.PartIndices[1]];
                }
                else
                {
                    object mvalue = Members[Index].Value;
                    return mvalue;
                }
            }
        }
        //public bool IsLocked(string memberIndex)
        //{
        //    return (Members[memberIndex] as Member).IsLocked;

        //}
        //public bool HasLockRequest(string memberIndex)
        //{
        //    return (Members[memberIndex] as Member).HasLockRequest;

        //}
        //public Transactions.Transaction GetLockTransaction(string memberIndex)
        //{
        //    return (Members[memberIndex] as Member).LockTransaction;

        //}





        /// <MetaDataID>{B8FBF12B-7E23-429A-9355-249CEC59CE82}</MetaDataID>
        /// <summary></summary>
        public class RowEnumerator : System.Collections.IEnumerator, Remoting.IExtMarshalByRefObject
        {

            public void Reset()
            {
                if (TheStructureSet != null)
                    TheStructureSet.MoveFirst();
            }

            /// <summary></summary>
            /// <MetaDataID>{8192BDFA-2BAF-4ABC-81A1-8D4F77EE989F}</MetaDataID>
            public bool MoveNext()
            {
                if (TheStructureSet == null)
                    return false;
                return TheStructureSet.MoveNext();
            }

            /// <MetaDataID>{D0131618-67A0-4A0E-B5DC-A8D20AA024F7}</MetaDataID>
            /// <summary></summary>
            private Collections.StructureSet TheStructureSet;
            /// <summary></summary>
            /// <MetaDataID>{D076274C-EBE5-4807-87C6-474677A0A165}</MetaDataID>
            /// <param name="theStructureSet"></param>
            public RowEnumerator(Collections.StructureSet theStructureSet)
            {
                if (theStructureSet != null)
                {
                    TheStructureSet = theStructureSet;
                    TheStructureSet.MoveFirst();
                }
            }
            /// <MetaDataID>{11D07394-A257-4F56-849C-F3F1F49472FA}</MetaDataID>
            /// <summary></summary>
            public object Current
            {
                get
                {
                    return TheStructureSet;
                }
            }
        }









        /// <MetaDataID>{6CD0A13A-1EAD-4EB3-A9B4-723DDAEC1BDF}</MetaDataID>
        public object GetData()
        {
            if (QueryResult == null)
                QueryResult = mOQLStatement.QueryResultType;

            if (QueryResult != null)
                return QueryResult;


            StructureSetData structureSetData = new StructureSetData();
            MoveFirst();

            structureSetData.Members = Members;

            System.Collections.Generic.Dictionary<System.Guid, DataSource> dataSources = new System.Collections.Generic.Dictionary<System.Guid, DataSource>();
            foreach (DataNode dataNode in mOQLStatement.DataTrees)
                dataNode.GetDataSources(ref dataSources);
            foreach (DataSource dataSource in dataSources.Values)
                dataSource.CachingRelationshipData();


            //#########################
            //foreach (DataLoader.DataTable table in mOQLStatement.ObjectQueryData.Tables)
            //{
            //    if (table.OwnerDataSource.DataNode.BranchParticipateInSelectClause)
            //        structureSetData.Data.Add(table.SerializeTable());
            //}
            //foreach (System.Data.DataRelation relation in mOQLStatement.ObjectQueryData.Relations)
            //{
            //    DataLoader.RelationData relationData = new DataLoader.RelationData();
            //    foreach (System.Data.DataColumn childColumn in relation.ChildColumns)
            //        relationData.ChildColumns.Add(childColumn.ColumnName);
            //    if (!(relation.ChildTable as DataLoader.DataTable).OwnerDataSource.DataNode.BranchParticipateInSelectClause)
            //        continue;

            //    relationData.ChildTableName = relation.ChildTable.TableName;

            //    foreach (System.Data.DataColumn parentColumn in relation.ParentColumns)
            //        relationData.ParentColumns.Add(parentColumn.ColumnName);

            //    if (!(relation.ParentTable as DataLoader.DataTable).OwnerDataSource.DataNode.BranchParticipateInSelectClause)
            //        continue;

            //    relationData.ParentTableName = relation.ParentTable.TableName;
            //    relationData.Name = relation.RelationName;
            //    structureSetData.Relations.Add(relationData);
            //}
            //#########################

            structureSetData.RootTableName = (Members as MemberList).RootDataNode.DataSource.DataTable.TableName;
            return structureSetData;
        }
#if !DeviceDotNet
#endif
        /// <MetaDataID>{CB3E5553-2199-46F0-8E39-723071D98C79}</MetaDataID>
		public void Close()
        {

            MainDataReader.Clear();
            MainDataReader = null;
            //			mOleDbConnection=null;
            mOQLStatement = null;

            DataRowEnumerator = null;
            _SourceStorageSession = null;
            _Members = null;

        }
        /// <MetaDataID>{57492156-ECCB-4C9B-ABCB-F7EF77DA1070}</MetaDataID>
        private ObjectQuery mOQLStatement;




        /// <MetaDataID>{209FE914-A7F3-4BAF-8F14-FCFC0A0441C3}</MetaDataID>
        public void MoveToPage(int pageNumber)
        {
            throw new System.NotImplementedException("Paging feature doesn,t supported yet");

        }
        /// <MetaDataID>{9A12C91E-FF98-48A4-A0FA-EC53092BBF2D}</MetaDataID>
        public bool MoveNextPage()
        {
            throw new System.NotImplementedException("Paging feature doesn,t supported yet");

        }
        /// <MetaDataID>{8713D7B4-5489-4E26-9623-2209606F087B}</MetaDataID>
        public int PagingActivated
        {
            get
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");

            }
            set
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");
            }
        }



        /// <MetaDataID>{C63D08EA-4697-4CCF-8534-793359DEB43F}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private int _PageCount;
        /// <MetaDataID>{0948AEC4-4BDD-4546-98A9-D5F697B63E28}</MetaDataID>
        public int PageCount
        {
            get
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");
            }
        }

        /// <MetaDataID>{CB7B0FF2-A712-4B82-9EA7-65E619C49DEE}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private int _PageSize;
        /// <MetaDataID>{19867C92-4CB1-4149-AE18-9D7E016AEAB7}</MetaDataID>
        public int PageSize
        {
            get
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");
            }
            set
            {
                throw new System.NotImplementedException("Paging feature doesn,t supported yet");
            }
        }
        /// <MetaDataID>{8BC6A6EF-B81D-4AA4-890C-229BA7C7D51D}</MetaDataID>
        protected void BuildStructureSetMembers()
        {
            if (_Members == null)
                _Members = new MemberList(mOQLStatement.DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode);
        }




        /// <MetaDataID>{929CF566-DF47-4231-812D-946065B30E7A}</MetaDataID>
        private System.Collections.IEnumerator DataRowEnumerator;
        /// <MetaDataID>{15AB3FCC-05D6-43A6-B803-1D90E20D2F93}</MetaDataID>
        public StructureSet(PersistenceLayer.ObjectStorage objectStorage)
        {
            _SourceStorageSession = objectStorage;
        }

        /// <MetaDataID>{ba8f9f69-55b7-447d-8156-69b6d8c24c80}</MetaDataID>
        public StructureSet(OOAdvantech.Collections.Generic.List<object> objectCollection, System.Type type, System.Collections.Generic.List<string> paths)
        {
            try
            {

                QueryOnRootObject queryOnRootObject = new QueryOnRootObject(objectCollection, type, paths);
                this.mOQLStatement = queryOnRootObject;
                queryOnRootObject.LoadData();

            }
            catch (System.Exception error)
            {

            }


        }

        /// <MetaDataID>{6c2e256f-6c8c-4a0c-b8f0-976bf1aeea4e}</MetaDataID>
        public StructureSet(object rootObject, System.Collections.Generic.List<string> paths)
        {
            try
            {

                QueryOnRootObject queryOnRootObject = new QueryOnRootObject(rootObject, paths);
                this.mOQLStatement = queryOnRootObject;

                // queryOnRootObject.BuildDataSources();
                queryOnRootObject.Distribute();
                queryOnRootObject.LoadData();


            }
            catch (System.Exception error)
            {
                throw;

            }


        }




        /// <MetaDataID>{3D7683D3-582A-4B5D-9EE7-5D12D24DCBB6}</MetaDataID>
        DataNode RootDataNode;
        /// <MetaDataID>{55EE620C-8D08-4468-8598-9960C36DF8BC}</MetaDataID>
        public StructureSet(MemberList memberList, System.Collections.IEnumerator dataRowEnumerator, ObjectQuery oqlStatement)
        {
            _Members = memberList;


            DataRowEnumerator = dataRowEnumerator;
            mOQLStatement = oqlStatement;
        }
        QueryResultType QueryResult;
        public StructureSet(QueryResultType queryResult, System.Collections.IEnumerator dataRowEnumerator, ObjectQuery oqlStatement)
        {

            QueryResult = queryResult;

            DataRowEnumerator = dataRowEnumerator;
            mOQLStatement = oqlStatement;
        }
        /// <MetaDataID>{37d4b3af-1d9e-4036-af6d-36a4f51f2864}</MetaDataID>
        public StructureSet(ObjectQuery oqlStatement)
        {
            mOQLStatement = oqlStatement;
        }



        /// <MetaDataID>{33F636EB-AA08-4057-87B6-6ECEC91B76E1}</MetaDataID>
        ~StructureSet()
        {
            try
            {
                //System.Diagnostics.Debug.WriteLine("\n\n\n********************  ~StructureSet() ********************************");
                MainDataReader.Clear();
            }
            catch (System.Exception Error)
            {

            }
            //if(mOleDbConnection!=null)
            //if(mOleDbConnection.State==System.Data.ConnectionState.Open)
            //	mOleDbConnection.Close();

        }
        /// <summary>The default position of the StructureSet is prior to the first record. Therefore, you must call MoveNext to begin accessing any data.
        /// Return Value true if there are more rows; otherwise, false</summary>
        /// <MetaDataID>{E9353DF6-2662-4BFE-8B4B-EA180FB86B49}</MetaDataID>
        public bool MoveNext()
        {
            if (QueryResult == null && mOQLStatement != null)
                QueryResult = mOQLStatement.QueryResultType;
            if (QueryResult != null)
            {
                bool HasMoreRows = DataRowEnumerator.MoveNext();

                return HasMoreRows;
            }
            else
            {
                bool HasMoreRows = DataRowEnumerator.MoveNext();
                if (!HasMoreRows)
                    return false;

                if (Members == null)
                    BuildStructureSetMembers();

                #region RowRemove code
                //while (mOQLStatement != null && mOQLStatement.DataTrees[0].SearchCondition != null && HasMoreRows && mOQLStatement.DataTrees[0].SearchCondition.IsRemovedRow(DataRowEnumerator.Current as IDataRow, -1))//(Members as MemberList).RootDataNode.DataSource.RowRemoveIndex)) #region RowRemove code
                //{
                //    HasMoreRows = DataRowEnumerator.MoveNext();
                //}
                #endregion

                if (!HasMoreRows)
                    return false;
                (Members as MemberList).DataRecord = DataRowEnumerator.Current as IDataRow;

                return HasMoreRows;
            }

        }

        /// <MetaDataID>{57377E78-34AB-477D-8601-46F43A84211A}</MetaDataID>
        public void MoveFirst()
        {
            if (QueryResult == null && mOQLStatement != null)
                QueryResult = mOQLStatement.QueryResultType;
            if (Members == null && QueryResult == null)
                BuildStructureSetMembers();

            if (DataRowEnumerator == null)
            {
                if (QueryResult != null)
                    DataRowEnumerator = QueryResult.DataLoader.GetEnumerator();
                else
                {

                    if ((Members as MemberList).RootDataNode.DataSource.DataTable != null)
                        DataRowEnumerator = (Members as MemberList).RootDataNode.DataSource.DataTable.Rows.GetEnumerator();
                    else
                        DataRowEnumerator = DataSource.DataObjectsInstantiator.CreateDataTable(false).Rows.GetEnumerator();
                }
            }

            DataRowEnumerator.Reset();

        }
        ///// <MetaDataID>{fa530394-396c-47aa-9ba0-bc30e15bba4e}</MetaDataID>
        //public System.Data.DataSet TransformToDataSet()
        //{
        //    return (Members as MemberList).RootDataNode.DataSource.DataTable.DataSet;
        //}

        //	/// <MetaDataID>{33F8DBB6-A37D-4881-9AD8-0B2990BEDF7C}</MetaDataID>
        //	private System.Data.SqlClient.SqlConnection mOleDbConnection;
        /// <MetaDataID>{7DD8A758-5CBE-4E87-B6BE-5897B0A649EE}</MetaDataID>
        private IDataSet MainDataReader = DataSource.DataObjectsInstantiator.CreateDataSet();




        /// <summary>Mitsos</summary>
        /// <param name="Query">Kitsos Lala</param>
        /// <MetaDataID>{73A00762-8246-45FF-8634-F4CAE53F3961}</MetaDataID>
        public void Open(string Query, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
        {


            MainDataReader.Clear();
            mOQLStatement = new ObjectsContextQuery(parameters);
            lock (DotNetMetaDataRepository.Type.LoadDotnetMetadataLock)
            {
                (mOQLStatement as ObjectsContextQuery).Build(Query, SourceStorageSession as PersistenceLayer.ObjectStorage);
            }
            var objectContextReference = new QueryResultObjectContextReference();
            objectContextReference.ObjectQueryContext = mOQLStatement;

            mOQLStatement.QueryResultType = new QueryResultType(mOQLStatement.DataTrees[0], objectContextReference);
            BuildQueryResult(mOQLStatement.QueryResultType);
            mOQLStatement.QueryResultType.DataFilter = mOQLStatement.DataTrees[0].SearchCondition;


            //(mOQLStatement as ObjectsContextQuery).Distribute();
            (mOQLStatement).LoadData();


            //(mOQLStatement.DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode).DataSource.LoadData();

            //(mOQLStatement.DataTrees[0] as MetaDataRepository.ObjectQueryLanguage.DataNode).LoadData(MainDataReader);
            //DataRowEnumerator= MainDataReader.Tables[0].Rows.GetEnumerator();
            return;


        }

        private void BuildQueryResult(QueryResultType queryResult)
        {
            DataNode rootDataNode = queryResult.RootDataNode;

            var OQLStatement = queryResult.RootDataNode.ObjectQuery;
            while (rootDataNode.Type == DataNode.DataNodeType.Namespace)
                rootDataNode = rootDataNode.SubDataNodes[0];

            RootDataNode = rootDataNode;
            System.Collections.Generic.Dictionary<string, QueryResultPart> Members = new System.Collections.Generic.Dictionary<string, QueryResultPart>();
            BuildMemberList(rootDataNode, Members, queryResult);


            foreach (DataNode selectionDataNode in OQLStatement.SelectListItems)
            {
                if (selectionDataNode.IsPathNode(rootDataNode) && !selectionDataNode.MembersFetchingObjectActivation)
                {
                    DataPath dataPath = new DataPath();

                    DataNode parent = selectionDataNode.ParentDataNode;
                    if (selectionDataNode.Type == DataNode.DataNodeType.Object)//if (parent != rootDataNode && selectionDataNode.Type!=DataNode.DataNodeType.OjectAttribute)
                    {

                        //if (selectionDataNode.AssignedMetaObject is AssociationEnd &&
                        //       selectionDataNode.ParentDataNode.DataSource.RelationshipsData.ContainsKey(selectionDataNode.AssignedMetaObject.Identity.ToString()))
                        //    dataPath.Push(selectionDataNode.Alias + "_AssociationTable");
                        //dataPath.Push(selectionDataNode.Alias);
                        dataPath.Push(selectionDataNode);

                    }

                    while (parent != rootDataNode)
                    {
                        //TODO να ελχθεί αν ορθος έφυγε το if
                        //if (parent.ParentDataNode != rootDataNode)
                        {

                            //if (parent.AssignedMetaObject is AssociationEnd &&
                            //       parent.ParentDataNode.DataSource.RelationshipsData.ContainsKey(parent.AssignedMetaObject.Identity.ToString()))
                            //{
                            //    dataPath.Push(parent.Alias + "_AssociationTable");
                            //}
                            //dataPath.Push(parent.Alias);
                            dataPath.Push(parent);
                        }
                        parent = parent.ParentDataNode;
                    }


                    if (selectionDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        string memberName = null;
                        if (selectionDataNode.Alias != null && selectionDataNode.Alias.Trim().Length > 0)
                            memberName = selectionDataNode.Alias;
                        else
                            memberName = selectionDataNode.Name;
                        if (!Members.ContainsKey(memberName))
                        {
                            SinglePart singlePart = new SinglePart(selectionDataNode, memberName, queryResult);
                            Members.Add(singlePart.Name, singlePart);
                        }
                    }

                    if (selectionDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        string memberName = null;
                        if (selectionDataNode.Alias != null && selectionDataNode.Alias.Trim().Length > 0)
                            memberName = selectionDataNode.Alias;
                        else
                            memberName = selectionDataNode.Name;
                        if (!Members.ContainsKey(memberName))
                        {
                            SinglePart singlePart = new SinglePart(selectionDataNode, memberName, queryResult);
                            Members.Add(singlePart.Name, singlePart);
                        }
                    }

                }
            }

            foreach (var member in Members.Values)
                queryResult.AddMember(member);
        }

        private void BuildMemberList(DataNode dataNode, System.Collections.Generic.Dictionary<string, QueryResultPart> Members, QueryResultType queryResultType)
        {
            var OQLStatement = dataNode.ObjectQuery;
            if (queryResultType.RootDataNode != dataNode)
            {
                if (OQLStatement.SelectListItems.Contains(dataNode) && dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {



                    string memberName = null;
                    if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                        memberName = dataNode.Alias;
                    else
                        memberName = dataNode.Name;
                    SinglePart singlePart = new SinglePart(dataNode, memberName, queryResultType);
                    Members.Add(memberName, singlePart);

                    if (RootDataNode == null)
                        RootDataNode = dataNode.ParentDataNode;



                }
                else if (OQLStatement.SelectListItems.Contains(dataNode)
                    && dataNode.ParentDataNode != null && dataNode.ParentDataNode.Type != DataNode.DataNodeType.Object
                    && dataNode.Type == DataNode.DataNodeType.Object
                    && (!(dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    || dataNode == RootDataNode))
                {
                    RootDataNode = dataNode;
                    string memberName = null;
                    if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                        memberName = dataNode.Alias;
                    else
                        memberName = dataNode.Name;
                    SinglePart singlePart = new SinglePart(dataNode, memberName, queryResultType);
                    //queryResultType.Members.Add(singlePart);
                    queryResultType.AddMember(singlePart);

                    if (RootDataNode == null)
                        RootDataNode = dataNode.ParentDataNode;

                }
                else if (dataNode.BranchParticipateInSelectClause
                    && dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    if (dataNode.MembersFetchingObjectActivation)// && (dataNode.DataSource.DataTable == null || dataNode.DataSource.DataTable.Rows.Count == 0))
                        return;
                    RootDataNode = dataNode.ParentDataNode;
                    QueryResultType enumerablePartType = new QueryResultType(dataNode, queryResultType.ObjectQueryContextReference);
                    BuildQueryResult(enumerablePartType);
                    string memberName = null;
                    if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                        memberName = dataNode.Alias;
                    else
                        memberName = dataNode.Name;

                    EnumerablePart member = new EnumerablePart(enumerablePartType, memberName, queryResultType);

                    Members.Add(member.Name, member);
                    return;

                }
                else if (dataNode.BranchParticipateInSelectClause
               && dataNode.ParentDataNode != null && dataNode.ParentDataNode.Type == DataNode.DataNodeType.Object
               && dataNode.Type == DataNode.DataNodeType.Object)
                {
                    if (dataNode.MembersFetchingObjectActivation)//&& (dataNode.DataSource.DataTable == null || dataNode.DataSource.DataTable.Rows.Count == 0))
                        return;
                    string memberName = null;
                    if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                        memberName = dataNode.Alias;
                    else
                        memberName = dataNode.Name;
                    SinglePart singlePart = new SinglePart(dataNode, memberName, queryResultType);
                    Members.Add(memberName, singlePart);

                    return;

                }
                else if (dataNode.Type == DataNode.DataNodeType.Count)
                {
                    string memberName = null;
                    if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                        memberName = dataNode.Alias;
                    else
                        memberName = dataNode.Name;
                    SinglePart singlePart = new SinglePart(dataNode, memberName, queryResultType);
                    Members.Add(memberName, singlePart);

                    if (RootDataNode == null)
                        RootDataNode = dataNode.ParentDataNode;


                }

                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                    BuildMemberList(subDataNode, Members, queryResultType);

            }
            else
            {
                //if (RootDataNode.Recursive)
                //    Members.Add(RootDataNode.Name, new RecursiveMember(RootDataNode, this));


                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                    BuildMemberList(subDataNode, Members, queryResultType);

                if (OQLStatement.SelectListItems.Contains(dataNode))
                {
                    SinglePart member = new SinglePart(dataNode, "Object", queryResultType);
                    //ObjectTypeMembers.Add(member);
                    Members.Add(member.Name, member);

                    string memberName = null;
                    if (dataNode.Alias != null && dataNode.Alias.Trim().Length > 0)
                        memberName = dataNode.Alias;
                    else
                        memberName = dataNode.Name;
                    SinglePart singlePart = new SinglePart(dataNode, memberName, queryResultType);
                    Members.Add(memberName, singlePart);

                }

            }
        }
    }
}
