namespace OOAdvantech.PersistenceLayerRunTime.ClientSide
{
	/// <MetaDataID>{6C9F9430-04E9-4DB8-BB2A-CDA4F5165A99}</MetaDataID>
    internal class StructureSetAgent : Collections.StructureSet
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
                    object mvalue = Members[Index].Value;
                    return mvalue;
                }
                else
                {
                    return ServerSideStructureSet[Index];
                }
            }
        }

        ObjectsContext _SourceStorageSession;
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

        OOAdvantech.Collections.MemberList _Members;
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
                    return ServerSideStructureSet.Members;
                }


                return _Members;
            }
        }

        bool LocalData;
		/// <MetaDataID>{808A0F31-7394-4DDD-8A0B-576075EDE449}</MetaDataID>
		private System.Collections.IEnumerator RowEnumerator;
		/// <MetaDataID>{7DC39F09-5657-4910-A354-137B0A3A7FF1}</MetaDataID>
        public StructureSetAgent(Collections.StructureSet serverSideStructureSet,ObjectsContext objectsContext)
        {
            _SourceStorageSession = objectsContext;
            ServerSideStructureSet = serverSideStructureSet;
            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(ServerSideStructureSet as System.MarshalByRefObject))
                LocalData = true;
            else
                LocalData = false;

            
            MetaDataRepository.ObjectQueryLanguage.StructureSet.StructureSetData dataSource = (serverSideStructureSet as MetaDataRepository.ObjectQueryLanguage.StructureSet).GetData() as MetaDataRepository.ObjectQueryLanguage.StructureSet.StructureSetData;
            serverSideStructureSet.Close();
            System.Data.DataSet dataSet =new System.Data.DataSet();

            foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.StreamedTable streamedTable in dataSource.Data)
                dataSet.Tables.Add(new MetaDataRepository.ObjectQueryLanguage.DataLoader.DataTable(streamedTable));
            foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.RelationData relation in dataSource.Relations)
            {
                System.Data.DataTable parentTable = dataSet.Tables[relation.ParentTableName];
                System.Data.DataTable childTable = dataSet.Tables[relation.ChildTableName];
                System.Data.DataColumn[] parentColumns =new System.Data.DataColumn[relation.ParentColumns.Count];
                System.Data.DataColumn[] childColumns =new System.Data.DataColumn[relation.ChildColumns.Count];
                int i = 0;
                foreach (string parentColumnName in relation.ParentColumns)
                    parentColumns[i++] = parentTable.Columns[parentColumnName];
                i = 0;
                foreach (string childColumnName in relation.ChildColumns)
                    childColumns[i++] = childTable.Columns[childColumnName];

                dataSet.Relations.Add(relation.Name, parentColumns, childColumns,false);//=true;
            }
            _Members = dataSource.Members;
            RowEnumerator = dataSet.Tables[dataSource.RootTableName].Rows.GetEnumerator();


        }


        public StructureSetAgent(System.Collections.Generic.List<Collections.StructureSet> structureSets)
        {
            LocalData = true;
            System.Data.DataSet dataSet = null;
            string rootTableName = null;

            foreach (Collections.StructureSet serverSideStructureSet in structureSets)
            {

                MetaDataRepository.ObjectQueryLanguage.StructureSet.StructureSetData dataSource = (serverSideStructureSet as MetaDataRepository.ObjectQueryLanguage.StructureSet).GetData() as MetaDataRepository.ObjectQueryLanguage.StructureSet.StructureSetData;
                rootTableName = dataSource.RootTableName;
                serverSideStructureSet.Close();
                if (dataSet == null)
                {
                    dataSet = new System.Data.DataSet();

                    foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.StreamedTable streamedTable in dataSource.Data)
                        dataSet.Tables.Add(new MetaDataRepository.ObjectQueryLanguage.DataLoader.DataTable(streamedTable));
                    foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.RelationData relation in dataSource.Relations)
                    {
                        System.Data.DataTable parentTable = dataSet.Tables[relation.ParentTableName];
                        System.Data.DataTable childTable = dataSet.Tables[relation.ChildTableName];
                        System.Data.DataColumn[] parentColumns = new System.Data.DataColumn[relation.ParentColumns.Count];
                        System.Data.DataColumn[] childColumns = new System.Data.DataColumn[relation.ChildColumns.Count];
                        int i = 0;
                        foreach (string parentColumnName in relation.ParentColumns)
                            parentColumns[i++] = parentTable.Columns[parentColumnName];
                        i = 0;
                        foreach (string childColumnName in relation.ChildColumns)
                            childColumns[i++] = childTable.Columns[childColumnName];

                        dataSet.Relations.Add(relation.Name, parentColumns, childColumns, false);//=true;
                    }
                    _Members = dataSource.Members;
                }
                else
                {
                    foreach (MetaDataRepository.ObjectQueryLanguage.DataLoader.StreamedTable streamedTable in dataSource.Data)
                    {
                        System.Data.DataTable table=new MetaDataRepository.ObjectQueryLanguage.DataLoader.DataTable(streamedTable);
                        dataSet.Tables[table.TableName].Merge(table);
                    }

                }

                
            }
            RowEnumerator = dataSet.Tables[rootTableName].Rows.GetEnumerator();


        }


        public System.Collections.IEnumerator GetEnumerator()
        {
            if (LocalData)
                return new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.StructureSet.RowEnumerator(this);
            else
                return ServerSideStructureSet.GetEnumerator();
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
                RowEnumerator.Reset();
                if (Members is MetaDataRepository.ObjectQueryLanguage.MemberList)
                    (Members as MetaDataRepository.ObjectQueryLanguage.MemberList).DataRecord = RowEnumerator.Current as System.Data.DataRow;
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
                bool HasMoreRows = RowEnumerator.MoveNext();
                if (!HasMoreRows)
                    return false;
                if (Members is MetaDataRepository.ObjectQueryLanguage.MemberList)
                    (Members as MetaDataRepository.ObjectQueryLanguage.MemberList).DataRecord = RowEnumerator.Current as System.Data.DataRow;
                return true;
            }
            else
            {
                return ServerSideStructureSet.MoveNext();
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



      
    }
}
