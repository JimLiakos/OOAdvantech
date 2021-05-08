using OOAdvantech.Remoting;
using System;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
	/// <MetaDataID>{6C9F9430-04E9-4DB8-BB2A-CDA4F5165A99}</MetaDataID>
    [System.Serializable]
    public class StructureSetAgent : Collections.StructureSet,System.Runtime.Serialization.IDeserializationCallback
	{
         
        /// <MetaDataID>{38E233C4-B89D-4251-94CF-B264DD270199}</MetaDataID>
        /// <summary>Define an indexer. 
        /// You give the member name and return the value of structure instance member.
        /// If you haven't call the MoveNext method or the MoveNext return false and call indexer then indexer raise exception.</summary>
        public object this[string Index]
        {
            get
            {
                if (LocalData)
                {
                    if (RowEnumerator.Current is CompositeRowData)
                    {
                        QueryResultPart member = QueryResult.GetMember(Index);
                        if (member is EnumerablePart)
                            return new StructureSet(((RowEnumerator.Current as CompositeRowData)[member.PartIndices[0]][member.PartIndices[1]] as QueryResultDataLoader).Type, ((RowEnumerator.Current as CompositeRowData)[member.PartIndices[0]][member.PartIndices[1]] as QueryResultDataLoader).GetEnumerator(), null);
                        else
                            return (RowEnumerator.Current as CompositeRowData)[member.PartIndices[0]][member.PartIndices[1]];
                    }
                    else
                    {
                        object mvalue = Members[Index].Value;
                        return mvalue;
                    }
                }
                else
                {
                    return ServerSideStructureSet[Index];
                }
            }


        }
        public bool ContainsMember(string memberName)
        {
            if (RowEnumerator.Current is CompositeRowData)
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
        //public bool IsLocked(
        //{
        //    if (LocalData)
        //    {
        //        return (Members[memberIndex] as Member).IsLocked;
                
        //    }
        //    else
        //    {
        //        return ServerSideStructureSet.IsLocked(memberIndex);
        //    }

        //}

        //public bool HasLockRequest(string memberIndex)
        //{
        //    if (LocalData)
        //    {
        //        return (Members[memberIndex] as Member).HasLockRequest;

        //    }
        //    else
        //    {
        //        return ServerSideStructureSet.HasLockRequest(memberIndex);
        //    }

        //}
      


        //public Transactions.Transaction GetLockTransaction(string memberIndex)
        //{
        //    if (LocalData)
        //    {
        //        return (Members[memberIndex] as Member).LockTransaction;

        //    }
        //    else
        //    {
        //        return ServerSideStructureSet.GetLockTransaction(memberIndex);
        //    }

        //}

        /// <MetaDataID>{69208db6-c8e3-49bb-a956-74155ee3c2b6}</MetaDataID>
        ObjectsContext _SourceStorageSession;
        /// <MetaDataID>{60272b7b-8f2d-4d82-91c5-1e8134aee32c}</MetaDataID>
        public ObjectsContext SourceStorageSession
        {
            get
            {
                if (LocalData)
                {
                    return _SourceStorageSession;
                }
                else
                {
                    return ServerSideStructureSet.SourceStorageSession;
                }
            }
            set
            {
                _SourceStorageSession = value;
            }
        }
        /// <MetaDataID>{7fe974dd-42ed-4f9d-9dee-2c9d9f491f7a}</MetaDataID>
        [System.NonSerialized]
        OOAdvantech.Collections.MemberList _Members;
        /// <MetaDataID>{ba7e2941-8fba-4cbc-bbd5-e5a942615c4b}</MetaDataID>
        public OOAdvantech.Collections.MemberList Members
        {
            get
            {
                if (LocalData)
                {
                    return _Members;
                }
                else
                {
                    return (ServerSideStructureSet as StructureSet).Members;
                }


                return _Members;
            }
        }

        /// <MetaDataID>{9ac98a7d-07df-4d9d-bd6c-32b7007879d2}</MetaDataID>
        bool LocalData;
		/// <MetaDataID>{808A0F31-7394-4DDD-8A0B-576075EDE449}</MetaDataID>
        [System.NonSerialized]
		private System.Collections.IEnumerator RowEnumerator;
		/// <MetaDataID>{7DC39F09-5657-4910-A354-137B0A3A7FF1}</MetaDataID>
        public StructureSetAgent(Collections.StructureSet serverSideStructureSet)
        {
            _SourceStorageSession = serverSideStructureSet.SourceStorageSession; ;
            ServerSideStructureSet = serverSideStructureSet;
            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(ServerSideStructureSet as MarshalByRefObject))
                LocalData = true;
            else
            {
                LocalData = false;
                return;

            }




        }
        public StructureSetAgent(System.Collections.Generic.List<Collections.StructureSet> structureSets)
        {
            if (structureSets.Count == 1)
            {
                Collections.StructureSet serverSideStructureSet = structureSets[0];
                _SourceStorageSession = serverSideStructureSet.SourceStorageSession; ;
                ServerSideStructureSet = serverSideStructureSet;
                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(ServerSideStructureSet as MarshalByRefObject))
                    LocalData = true;
                else
                {
                    LocalData = false;
                    return;

                }
            }
        }

        ///// <MetaDataID>{8b158ac5-c5b0-4b8a-a947-86fb28df2f2d}</MetaDataID>
        //public StructureSetAgent(System.Collections.Generic.List<Collections.StructureSet> structureSets)
        //{
         
        //    LocalData = true;
        //    System.Data.DataSet dataSet = null;
        //    string rootTableName = null;
        //    if (structureSets.Count == 0)
        //        return;


        //    foreach (Collections.StructureSet serverSideStructureSet in structureSets)
        //    {

        //        MetaDataRepository.ObjectQueryLanguage.StructureSet.StructureSetData dataSource = (serverSideStructureSet as MetaDataRepository.ObjectQueryLanguage.StructureSet).GetData() as MetaDataRepository.ObjectQueryLanguage.StructureSet.StructureSetData;
        //        rootTableName = dataSource.RootTableName;
        //        serverSideStructureSet.Close();
        //        if (dataSet == null)
        //        {
        //            dataSet = new System.Data.DataSet();

        //            foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.StreamedTable streamedTable in dataSource.Data)
        //                dataSet.Tables.Add(new MetaDataRepository.ObjectQueryLanguage.DataLoader.DataTable(streamedTable));
        //            foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.RelationData relation in dataSource.Relations)
        //            {
        //                System.Data.DataTable parentTable = dataSet.Tables[relation.ParentTableName];
        //                System.Data.DataTable childTable = dataSet.Tables[relation.ChildTableName];
        //                System.Data.DataColumn[] parentColumns = new System.Data.DataColumn[relation.ParentColumns.Count];
        //                System.Data.DataColumn[] childColumns = new System.Data.DataColumn[relation.ChildColumns.Count];
        //                int i = 0;
        //                foreach (string parentColumnName in relation.ParentColumns)
        //                    parentColumns[i++] = parentTable.Columns[parentColumnName];
        //                i = 0;
        //                foreach (string childColumnName in relation.ChildColumns)
        //                    childColumns[i++] = childTable.Columns[childColumnName];

        //                dataSet.Relations.Add(relation.Name, parentColumns, childColumns, false);//=true;
        //            }
        //            _Members = dataSource.Members;
        //        }
        //        else
        //        {
        //            foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.StreamedTable streamedTable in dataSource.Data)
        //            {
        //                System.Data.DataTable table=new MetaDataRepository.ObjectQueryLanguage.DataLoader.DataTable(streamedTable);
        //                dataSet.Tables[table.TableName].Merge(table);
        //            }

        //        }

                
        //    }
            
        //    RowEnumerator = dataSet.Tables[rootTableName].Rows.GetEnumerator();


        //}


        /// <MetaDataID>{217a277f-4bad-4ec1-9653-0e0859b0efe9}</MetaDataID>
        public System.Collections.IEnumerator GetEnumerator()
        {
            if (LocalData)
            {
                if (RowEnumerator == null)
                    GetRemoteData();
                return new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.StructureSet.RowEnumerator(this);
            }
            else
            {
                if(ServerSideStructureSet==null)
                    return new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.StructureSet.RowEnumerator(null);


                return ServerSideStructureSet.GetEnumerator();
            }
        }
        //public StructureSetAgent(PersistenceLayerRunTime.StructureSet.DataBlock dataSource, System.Data.DataTable dataTable,System.Collections.IEnumerator rowEnumerator)
        //{
        //    DataSource = dataSource;
        //    RowEnumerator = rowEnumerator;
        //    if (Members == null)
        //        Members = new MemberListAgent(DataSource, dataTable, RowEnumerator);
        //}
		/// <MetaDataID>{42E442AB-299E-4E28-8A52-EBC84AA8DE51}</MetaDataID>
        private Collections.StructureSet ServerSideStructureSet;
	
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{3DC7604E-C813-479C-9B16-16C661244F28}</MetaDataID>
		private int _PageSize;
		/// <MetaDataID>{9D13D1A4-64EC-4E7B-9217-5585830BF553}</MetaDataID>
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
		/// <exclude>Excluded</exclude>
		/// <MetaDataID>{C07633F3-8623-41B7-85A7-DC01CBD20694}</MetaDataID>
		private int _PageCount;
		/// <MetaDataID>{0B16DA79-B84A-4B9B-9CD9-48499A1269C2}</MetaDataID>
		public  int PageCount
		{
			get
			{
				throw new System.NotImplementedException("Paging feature doesn,t supported yet");
			}
		}
		/// <MetaDataID>{1BD97A04-9452-4CAD-800A-2F1808F0926B}</MetaDataID>
		public void Close()
		{
            if (!LocalData)
                ServerSideStructureSet.Close();
                
		
		}

	
		/// <MetaDataID>{BD4B4CC8-B92C-4323-B3DF-3B3A976FBC58}</MetaDataID>
		public void MoveFirst()
		{
            if (LocalData)
            {
                if (RowEnumerator == null)
                    GetRemoteData();

                if (RowEnumerator == null)
                    return;
                RowEnumerator.Reset();
                if (Members is MetaDataRepository.ObjectQueryLanguage.MemberList)
                    (Members as MetaDataRepository.ObjectQueryLanguage.MemberList).DataRecord = RowEnumerator.Current as IDataRow;
            }
            else
            {
                
                   ServerSideStructureSet.MoveFirst();
            }

		
		}
	
		/// <MetaDataID>{85A68AB3-0085-47CF-B06D-81E622068B59}</MetaDataID>
		public bool MoveNext()
		{

            if (LocalData)
            {
                if (RowEnumerator == null)
                    return false;
                bool HasMoreRows = RowEnumerator.MoveNext();
                if (!HasMoreRows)
                    return false;
                if (Members is MetaDataRepository.ObjectQueryLanguage.MemberList)
                    (Members as MetaDataRepository.ObjectQueryLanguage.MemberList).DataRecord = RowEnumerator.Current as IDataRow;
                return true;
            }
            else
            {
                return ServerSideStructureSet.MoveNext();
            }

			
		
		}

        QueryResultType QueryResult;
        /// <MetaDataID>{470eeb7e-0076-47b8-9e3b-fa010a2ee3dd}</MetaDataID>
        IDataSet DataSet;
        /// <MetaDataID>{f7eb5862-3b32-4f94-b525-8879ead37fb5}</MetaDataID>
        private void GetRemoteData()
        {
            if (ServerSideStructureSet == null)
                return;
            object serverSideData = (ServerSideStructureSet as MetaDataRepository.ObjectQueryLanguage.StructureSet).GetData();
            if (serverSideData is QueryResultType)
            {
                QueryResult = serverSideData as QueryResultType;
                RowEnumerator = QueryResult.DataLoader.GetEnumerator();
            }
            else
            {

                MetaDataRepository.ObjectQueryLanguage.StructureSet.StructureSetData structureSetData = serverSideData as MetaDataRepository.ObjectQueryLanguage.StructureSet.StructureSetData;



                //##################################################
                //int i = 0;
                //foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.StreamedTable streamedTable in structureSetData.Data)
                //    DataSet.Tables.Add(new MetaDataRepository.ObjectQueryLanguage.DataLoader.DataTable(streamedTable));
                //foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.RelationData relation in structureSetData.Relations)
                //{
                //    System.Data.DataTable parentTable = DataSet.Tables[relation.ParentTableName];
                //    System.Data.DataTable childTable = DataSet.Tables[relation.ChildTableName];
                //    System.Data.DataColumn[] parentColumns = new System.Data.DataColumn[relation.ParentColumns.Count];
                //    System.Data.DataColumn[] childColumns = new System.Data.DataColumn[relation.ChildColumns.Count];
                //    i = 0;
                //    foreach (string parentColumnName in relation.ParentColumns)
                //        parentColumns[i++] = parentTable.Columns[parentColumnName];
                //    i = 0;
                //    foreach (string childColumnName in relation.ChildColumns)
                //        childColumns[i++] = childTable.Columns[childColumnName];

                //    DataSet.Relations.Add(relation.Name, parentColumns, childColumns, false);//=true;
                //}
                //##################################################

                _Members = structureSetData.Members;

                #region Retrieves DataTree from members
                System.Collections.Generic.List<DataNode> dataTrees = new System.Collections.Generic.List<DataNode>();
                foreach (var member in _Members)
                {
                    if (member is Member && !dataTrees.Contains((member as Member).MemberMedata.HeaderDataNode))
                        dataTrees.Add((member as Member).MemberMedata.HeaderDataNode);
                }
                #endregion

                #region validate DataNodes in client context
                string validationErrors = null;
                foreach (DataNode dataNode in dataTrees)
                {
                    if (!dataNode.Validate(ref validationErrors))
                    {
                        ///TODO: Θα πρέπει να γραφτεί test case όπου θα χτηπάμε query σε
                        ///concreete class που θα υπάρχει στο server ενώ στον client  θσ 
                        ///υπάρχει μόνο το abstraction.

                    }
                }
                #endregion

                #region Create DataSet and add all tables
                DataSet = DataSource.DataObjectsInstantiator.CreateDataSet();
                System.Collections.Generic.Dictionary<System.Guid, DataSource> dataSources = new System.Collections.Generic.Dictionary<System.Guid, DataSource>();
                foreach (DataNode dataNode in dataTrees)
                {
                    dataNode.GetDataSources(ref dataSources);
                    foreach (var dataSource in dataSources.Values)
                    {
                        if (!dataSource.DataLoadedInParentDataSource)
                        {
                            DataSet.AddTable(dataSource.DataTable);
                            foreach (var relationshipData in dataSource.RelationshipsData.Values)
                            {
                                if (relationshipData.AssotiationTableRelationshipData != null && relationshipData.AssotiationTableRelationshipData.Data != null)
                                    DataSet.AddTable(relationshipData.AssotiationTableRelationshipData.Data);
                            }
                        }
                        if (dataSource.ParentRelationshipData != null && dataSource.ParentRelationshipData.AssotiationTableRelationshipData.Data != null)
                            DataSet.AddTable(dataSource.ParentRelationshipData.AssotiationTableRelationshipData.Data);

                    }
                }
                #endregion

                dataTrees[0].BuildTablesRelations();

                RowEnumerator = DataSet.Tables[structureSetData.RootTableName].Rows.GetEnumerator();
            }

        }

		/// <MetaDataID>{A8FC1B9B-62C5-4CFF-89B3-2FB9D722B1E1}</MetaDataID>
		public void MoveToPage(int pageNumber)
		{
			throw new System.NotImplementedException("Paging feature doesn,t supported yet");

		}
		/// <MetaDataID>{46166EE4-D6BD-48C6-9F99-FD5440E764D9}</MetaDataID>
		public bool MoveNextPage()
		{
			throw new System.NotImplementedException("Paging feature doesn,t supported yet");

		}
		/// <MetaDataID>{9496078C-D4BA-409D-A5CF-EF1A1AE05420}</MetaDataID>
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







        #region IDeserializationCallback Members
#if !PORTABLE
        /// <MetaDataID>{a5d13c78-c918-4de4-b432-a129770994d5}</MetaDataID>
        void System.Runtime.Serialization.IDeserializationCallback.OnDeserialization(object sender)
        {
            LocalData = true;
        }
#endif

#endregion

#region StructureSet Members

        /// <MetaDataID>{f653f0d4-bfea-49b1-b8a8-07960842d412}</MetaDataID>
        public IDataSet TransformToDataSet()
        {
            if (DataSet == null)
                MoveFirst();
            return DataSet;
        }

#endregion
    }
}
