using System;
using System.Collections.Generic;
using System.Text;
using SubDataNodeIdentity = System.Guid;
using System.Collections;
using PartialRelationIdentity = System.String;
using System.Linq;

//#if !PORTABLE
//using System.Data;
//#endif 


namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    /// <summary>
    /// ObjectKeyRelationColumns define a relation metadata for relation between the group data and data node with key object.  
    /// Query engine use this relation to retrieve the key object because the key 
    /// object participate in grouped data DataSource  with object identity columns.  
    /// </summary>
    /// <MetaDataID>{40fc8c3b-c0b2-415b-bb2b-20904672a441}</MetaDataID>
    [Serializable]
    public class ObjectKeyRelationColumns
    {

        /// <MetaDataID>{5005545f-4bb2-45e4-af39-e8246e7c5d90}</MetaDataID>
        public ObjectKeyRelationColumns(ObjectIdentityType objectIdentityType, List<string> groupingColumnsNames, List<string> groupedDataColumnsNames, DataSource dataSource)
        {
            ObjectIdentityType = objectIdentityType;
            DataSource = dataSource;
            foreach (MetaDataRepository.IIdentityPart identityPart in ObjectIdentityType.Parts)
            {
                _GroupingColumnsNames.AddRange(groupingColumnsNames);
                _GroupedDataColumnsNames.AddRange(groupedDataColumnsNames);
            }
        }
        /// <summary>
        /// Defines the the identity type of key object 
        /// </summary>
        /// <MetaDataID>{3ecf5931-2a0c-4b17-9acf-2a1fea0e7a47}</MetaDataID>
        public ObjectIdentityType ObjectIdentityType;

        /// <summary>
        /// Defines the relationship Name 
        /// </summary>
        /// <MetaDataID>{033d0e7d-e1b0-49c7-8ed6-d13dab82e39e}</MetaDataID>
        public string RelationName;
        /// <exclude>Excluded</exclude>
        List<string> _GroupingColumnsNames = new List<string>();
        /// <summary>Defines the columns names in Group DataSource </summary>
        /// <MetaDataID>{c9300710-cd71-4070-8894-963cea5ef53b}</MetaDataID>
        public List<string> GroupingColumnsNames
        {
            get
            {
                return _GroupingColumnsNames;
            }
        }

        /// <exclude>Excluded</exclude>
        List<string> _GroupedDataColumnsNames = new List<string>();
        ///<summary>
        ///Defines the columns Names in SataSourece with objects
        ///</summary>
        /// <MetaDataID>{cd0fa294-9e04-4d3a-80ab-079c839d0b85}</MetaDataID>
        public List<string> GroupedDataColumnsNames
        {
            get
            {
                return _GroupedDataColumnsNames;
            }

        }
        /// <summary> 
        /// Defines the object DataSource
        /// </summary>
        /// <MetaDataID>{7bc22def-b764-4143-80d7-3d61261abd64}</MetaDataID>
        public DataSource DataSource;
    }
    ///<summary>
    /// DataSource object collects data from all storages where participate in query. 
    /// Load them in a DataSet Table and creates relations with the tables of sub data nodes DataSources. 
    /// </summary>
    /// <MetaDataID>{C1E57F9C-3FD3-4BCA-B50A-DB43221A20BC}</MetaDataID>
    [Serializable]
    public abstract class DataSource : System.Runtime.Serialization.IDeserializationCallback, System.Runtime.Serialization.ISerializable
    {
        static DataSource()
        {
            DataSource.DataObjectsInstantiator = new DataObjectsInstantiator();
        }


#if !DeviceDotNet


        /// <MetaDataID>{d6ec2716-044d-42ef-914f-c3fa5d3b61c0}</MetaDataID>
        protected DataSource(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {

            _DataLoadedInParentDataSource = (Nullable<bool>)info.GetValue("DataLoadedInParentDataSource", typeof(Nullable<bool>));
            _GroupByKeyRelationColumns = info.GetValue("GroupByKeyRelationColumns", typeof(Dictionary<System.Guid, Dictionary<ObjectIdentityType, ObjectKeyRelationColumns>>)) as Dictionary<System.Guid, Dictionary<ObjectIdentityType, ObjectKeyRelationColumns>>;
            _ObjectIdentityColumnIndex = (Nullable<int>)info.GetValue("ObjectIdentityColumnIndex", typeof(Nullable<int>));
            _ObjectIndex = (Nullable<int>)info.GetValue("ObjectIndex", typeof(Nullable<int>));
            _ParentRelationshipData = info.GetValue("ParentRelationshipData", typeof(DataNodesRelationshipData)) as DataNodesRelationshipData;
            _RecursiveDetailsColumns = info.GetValue("RecursiveDetailsColumns", typeof(System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>>)) as System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>>;
            _RecursiveMasterColumns = info.GetValue("RecursiveMasterColumns", typeof(System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>>)) as System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>>;
            _RelationshipsData = info.GetValue("RelationshipsData", typeof(Dictionary<SubDataNodeIdentity, DataNodesRelationshipData>)) as Dictionary<SubDataNodeIdentity, DataNodesRelationshipData>;
            //_RowRemoveIndex = (Nullable<int>)info.GetValue("RowRemoveIndex", typeof(Nullable<int>)); //#region RowRemove code
            DataNode = info.GetValue("DataNode", typeof(DataNode)) as DataNode;
            _Identity = (System.Guid)info.GetValue("Identity", typeof(System.Guid));
            RetrieveRowsThroughParentChildRelation = (Nullable<bool>)info.GetValue("RetrieveRowsThroughParentChildRelation", typeof(Nullable<bool>));

            _DataSourceRelationsColumnsWithParent = info.GetValue("DataSourceRelationsColumnsWithParent", typeof(System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>>)) as System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>>;
            _DataSourceRelationsColumnsWithSubDataNodes = info.GetValue("DataSourceRelationsColumnsWithSubDataNodes", typeof(Dictionary<Guid, Dictionary<string, Dictionary<ObjectIdentityType, List<string>>>>)) as Dictionary<Guid, Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>>;


            DataLoader.StreamedTable streamedTable = (DataLoader.StreamedTable)info.GetValue("StreamedTable", typeof(DataLoader.StreamedTable));
            if (streamedTable.StreamedData != null)
            {
                GetTableFromStreamedTable = true;
                StreamedTable = streamedTable;
                StreamedTableName = info.GetValue("StreamedTableName", typeof(string)) as string;
            }

        }




#endif


#if !PORTABLE
        /// <MetaDataID>{3507d0ed-f7dc-45f5-addf-d211284d2b42}</MetaDataID>
        public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {

            ///TODO θα πρέπει να φυγεί είναι άχριστος η διακίνη των δεδομένων γίνεται μέσο QueryResultDataLoader
            info.AddValue("DataLoadedInParentDataSource", _DataLoadedInParentDataSource);
            info.AddValue("GroupByKeyRelationColumns", _GroupByKeyRelationColumns);
            info.AddValue("ObjectIdentityColumnIndex", _ObjectIdentityColumnIndex);
            info.AddValue("ObjectIndex", _ObjectIndex);
            if (DataNode.BranchParticipateInSelectClause)
                info.AddValue("ParentRelationshipData", _ParentRelationshipData);
            else
                info.AddValue("ParentRelationshipData", null);

            info.AddValue("RecursiveDetailsColumns", _RecursiveDetailsColumns);
            info.AddValue("RecursiveMasterColumns", _RecursiveMasterColumns);
            if (DataNode.BranchParticipateInSelectClause)
                info.AddValue("RelationshipsData", _RelationshipsData);
            else
                info.AddValue("RelationshipsData", new Dictionary<SubDataNodeIdentity, DataNodesRelationshipData>());

            //info.AddValue("RowRemoveIndex", _RowRemoveIndex); #region RowRemove code
            info.AddValue("DataNode", DataNode);
            info.AddValue("Identity", Identity);
            info.AddValue("RetrieveRowsThroughParentChildRelation", RetrieveRowsThroughParentChildRelation);
            info.AddValue("DataSourceRelationsColumnsWithParent", _DataSourceRelationsColumnsWithParent);
            info.AddValue("DataSourceRelationsColumnsWithSubDataNodes", _DataSourceRelationsColumnsWithSubDataNodes);


            if (DataNode.ObjectQuery.QueryResultType == null && _DataTable != null && DataNode.BranchParticipateInSelectClause)
            {

                info.AddValue("StreamedTable", (_DataTable as DataTable).SerializeTable());
                info.AddValue("StreamedTableName", _DataTable.TableName);
            }
            else
                info.AddValue("StreamedTable", new DataLoader.StreamedTable());



        }
        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// DataLoader collection marked as [NonSerialized] and must be initialized when object deserialized.
        /// </summary>
        /// <param name="sender">
        /// The object that initiated the callback. The functionality for this parameter
        /// is not currently implemented.
        /// </param>
        /// <MetaDataID>{3a2004a3-3e77-44a2-9a50-ff600ddb01a7}</MetaDataID>
        public virtual void OnDeserialization(object sender)
        {
            DataLoaders = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoader>();
        }
#endif
        /// <exclude>Excluded</exclude>
        Guid _Identity = Guid.NewGuid();
        /// <summary>
        /// The Identity is useful when the query distributed and main query retrieves DataLoaders from storages.
        /// </summary>
        /// <MetaDataID>{10daaab2-4bba-4782-ba21-25084717a038}</MetaDataID>
        public Guid Identity
        {
            get
            {
                return _Identity;
            }
        }



        /// <MetaDataID>{67eb131e-4918-43ff-9b19-c29c0ba8e32f}</MetaDataID>
        public DataSource(DataNode dataNode)
        {
            DataNode = dataNode;
        }

        /// <MetaDataID>{59b67241-45f4-4b7a-a58e-b2e93d3216c1}</MetaDataID>
        public DataSource(DataNode dataNode, DataSource orgDataSource)
        {
            _Identity = orgDataSource.Identity;
            DataNode = dataNode;
            DataLoaders = orgDataSource.DataLoaders;
            _Identity = orgDataSource.Identity;
            _GroupByKeyRelationColumns = orgDataSource.GroupByKeyRelationColumns;
            _DataLoadedInParentDataSource = orgDataSource.DataLoadedInParentDataSource;

            _ObjectIdentityTypes = orgDataSource.ObjectIdentityTypes;
            _RecursiveDetailsColumns = orgDataSource.RecursiveDetailsColumns;
            _RecursiveMasterColumns = orgDataSource.RecursiveMasterColumns;
            RetrieveRowsThroughParentChildRelation = orgDataSource.RetrieveRowsThroughParentChildRelation;

        }


        /// <exclude>Excluded</exclude>
        int? _ObjectIndex;

        ///<summary>
        ///Defines the index of columns in DataTable where loaded the object of objects DataNode
        ///</summary>
        /// <MetaDataID>{3a3c32e0-3e45-410b-bf3f-53ca4abfcecb}</MetaDataID>
        public int ObjectIndex
        {
            get
            {

                if (!_ObjectIndex.HasValue)
                {
                    DataNode nonValueTypeDataNode = DataNode;
                    while (nonValueTypeDataNode.ValueTypePath.Count > 0)
                        nonValueTypeDataNode = nonValueTypeDataNode.RealParentDataNode;
                    if (DataNode.ValueTypePath.Count != 0 && nonValueTypeDataNode.DataSource != null && nonValueTypeDataNode.DataSource.DataTable != null && nonValueTypeDataNode.DataSource.DataLoadedInParentDataSource)
                    {
                        if (DataTable == null)
                            _ObjectIndex = -1;
                        else
                            _ObjectIndex = DataTable.Columns.IndexOf(nonValueTypeDataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object");
                    }
                    else
                        if (DataLoadedInParentDataSource && DataNode.ValueTypePath.Count == 0)
                    {
                        DataNode dataNode = DataNode;
                        while (dataNode.Type == DataNode.DataNodeType.Object && dataNode.ValueTypePath.Count > 0)
                            dataNode = dataNode.RealParentDataNode;
                        if (DataTable == null)
                            _ObjectIndex = -1;
                        else
                            _ObjectIndex = DataTable.Columns.IndexOf(dataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object");
                    }
                    else
                    {
                        if (DataTable == null)
                            _ObjectIndex = -1;
                        else
                        {
                            if (DataNode is GroupDataNode)
                            {
                                var name = MetaDataRepository.ObjectQueryLanguage.DataLoader.GetGroupingDataColumnName(DataNode);
                                _ObjectIndex = DataTable.Columns.IndexOf(name);
                            }
                            else
                                _ObjectIndex = DataTable.Columns.IndexOf(DataNode.ValueTypePathDiscription + "Object");
                        }
                    }
                    //if(_ObjectIndex.Value==-1&&DataNode.ParticipateInSelectClause)
                }
                return _ObjectIndex.Value;
            }
        }





        /// <MetaDataID>{b3594e20-ab68-4d03-be83-08b5b85d8cfb}</MetaDataID>
        int? _ObjectIdentityColumnIndex;

        /// <MetaDataID>{10b01fba-45e8-4436-99dc-861fed974c47}</MetaDataID>
        public int ObjectIdentityColumnIndex
        {
            get
            {
                if (!_ObjectIdentityColumnIndex.HasValue)
                {
                    DataNode nonValueTypeDataNode = DataNode;
                    while (nonValueTypeDataNode.ValueTypePath.Count > 0)
                        nonValueTypeDataNode = nonValueTypeDataNode.RealParentDataNode;
                    if (DataLoadedInParentDataSource && DataNode.ValueTypePath.Count == 0)
                    {
                        DataNode dataNode = DataNode;
                        while (dataNode.Type == DataNode.DataNodeType.Object && dataNode.ValueTypePath.Count > 0)
                            dataNode = dataNode.RealParentDataNode;
                        _ObjectIdentityColumnIndex = DataTable.Columns.IndexOf("@" + dataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object_Identity");
                    }
                    else
                    {
                        _ObjectIdentityColumnIndex = DataTable.Columns.IndexOf("@" + DataNode.ValueTypePathDiscription + "Object_Identity");
                    }
                }
                return _ObjectIdentityColumnIndex.Value;
            }
        }

        #region RowRemove code

        ///// <exclude>Excluded</exclude>
        //int? _RowRemoveIndex;

        ///// <MetaDataID>{abafbe0d-471b-43d2-8c3e-6f823357922b}</MetaDataID>
        //public int RowRemoveIndex
        //{
        //    get
        //    {
        //        if (!_RowRemoveIndex.HasValue)
        //        {
        //            if (DataTable != null)
        //                _RowRemoveIndex = DataTable.Columns.IndexOf(DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove");
        //            else
        //                _RowRemoveIndex = -1;
        //        }
        //        return _RowRemoveIndex.Value;
        //    }
        //}




        /// <MetaDataID>{2e0643b6-03ab-413d-a53b-f85bcb3ed778}</MetaDataID>
        //public int GetRowRemoveIndex(SearchCondition searchCondition)
        //{
        //    if (searchCondition == DataNode.SearchCondition)
        //    {
        //        if (!_RowRemoveIndex.HasValue)
        //        {
        //            if (DataTable != null)
        //                _RowRemoveIndex = DataTable.Columns.IndexOf(DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove");
        //            else
        //                _RowRemoveIndex = -1;
        //        }
        //        return _RowRemoveIndex.Value;
        //    }
        //    else
        //    {
        //        if (DataTable != null)
        //            return DataTable.Columns.IndexOf(DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove_" + DataNode.SearchConditions.IndexOf(searchCondition));
        //        else
        //            return -1;
        //    }
        //}
        #endregion

        //public int GetRowRemoveIndex(SearchCondition searchCondition)
        //{
        //    if (DataNode.SearchCondition == searchCondition)
        //        return -1;
        //    int srhcIndex = DataNode.ObjectQuery.SearchConditions.IndexOf(searchCondition);

        //    if (DataTable != null)
        //        return DataTable.Columns.IndexOf("SC" + srhcIndex.ToString() + "_" + DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove_"+DataNode.SearchConditions.IndexOf(searchCondition).ToString());
        //    else
        //        _RowRemoveIndex = -1;

        //    return -1;
        //}



        ///// <summary>
        ///// This method sets the association 
        ///// fields of relate object to reproduce the objects link in memory. 
        ///// System must be load the data sources before call this function. 
        ///// </summary>
        ///// <param name="subDataNode">
        ///// This parameter defines the data node with the related objects.
        ///// </param>
        ///// <MetaDataID>{f8110e6e-d6ff-46f3-83d1-e332acd53d29}</MetaDataID>
        //abstract internal void LoadObjectRelationLinks(DataNode subDataNode);

        /// <MetaDataID>{740b7f7d-86e5-4646-bb3a-ee4e4da29ba2}</MetaDataID>
        public DataNode _DataNode;
        [Association("DataNodeData", typeof(DataNode), Roles.RoleB, "{84E78DE2-29BF-49DA-9341-BD03B53B7190}")]
        [RoleBMultiplicityRange(1, 1)]
        public DataNode DataNode
        {
            get
            {
                return _DataNode;
            }
            internal set
            {
                _DataNode = value;
            }
        }

        /// <summary>
        /// Defines a dictionary with dataloaders.
        /// You must give the object storage identity to take the corresponding data loader 
        /// </summary>
        [Association("LoadDataSource", typeof(DataLoader), Roles.RoleA, "{99167E92-DDD6-4646-B73B-9BA640932C5F}")]
        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(0)]
        [IgnoreErrorCheck]
        [NonSerialized]
        public OOAdvantech.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader> DataLoaders = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoader>();

        ///// <MetaDataID>{9EE07C2A-0E28-45F5-B00C-99D01C0B2D2D}</MetaDataID>
        ///// Dispatch the call to the data loaders to load data locally.
        ///// The CollectRemoteDataInProcess method collects data in central process.
        //public void LoadDataLocally()
        //{
        //    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
        //    {
        //        DataLoader dataLoader = entry.Value;
        //        dataLoader.RetrieveFromStorage();
        //    }
        //}

        /// <exclude>Excluded</exclude>
        bool? _DataLoadedInParentDataSource;

        /// <MetaDataID>{e9391daa-07d7-4d32-ac50-389c00ccfc26}</MetaDataID>
        public bool DataLoadedInParentDataSource
        {
            get
            {
                if (_DataLoadedInParentDataSource.HasValue)
                    return _DataLoadedInParentDataSource.Value;
                else
                {
                    _DataLoadedInParentDataSource = false;
                    foreach (var dataLoader in DataLoaders.Values)
                    {
                        if (!dataLoader.DataLoadedInParentDataSource)
                        {
                            _DataLoadedInParentDataSource = false;
                            break;
                        }
                        else
                        {
                            _DataLoadedInParentDataSource = true;
                        }
                    }
                    return _DataLoadedInParentDataSource.Value;
                }
            }
            set
            {

                _DataLoadedInParentDataSource = value;
            }
        }

        /// <MetaDataID>{41dbd013-9af1-4447-bd91-5a1bc0594d61}</MetaDataID>
        public bool LoadDataInParentDataSourcePermited
        {
            get
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                    !(DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany &&
                      (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass == null)
                {
                    return true;

                }
                else if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                      (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.LinkClass != null &&
                     DataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation == (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
                {
                    return true;
                }
                else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && DataNode.Type == DataNode.DataNodeType.Object)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public static IDataObjectsInstantiator DataObjectsInstantiator;

        /// <MetaDataID>{1c7554d4-b54f-425f-9ee8-f61629f83d7a}</MetaDataID>
        [NonSerialized]
        bool GetTableFromStreamedTable;

#if !DeviceDotNet
        /// <MetaDataID>{74d41ea3-5e86-4f5e-af8a-8b74c7819136}</MetaDataID>
        DataLoader.StreamedTable StreamedTable;
#endif

        /// <MetaDataID>{c1a5e7d2-315e-48fb-bb64-340c8850744f}</MetaDataID>
        string StreamedTableName;
        /// <exclude>Excluded</exclude>
        [NonSerialized]
        protected IDataTable _DataTable;
        /// <MetaDataID>{D521D616-8886-472F-B5E1-3DCFDCDA87AF}</MetaDataID>
        public IDataTable DataTable
        {
            get
            {
#if !DeviceDotNet
                if (_DataTable == null && GetTableFromStreamedTable)
                {
                    _DataTable = DataObjectsInstantiator.CreateDataTable(StreamedTable);
                    _DataTable.TableName = StreamedTableName;
                    GetTableFromStreamedTable = false;
                }
#endif



                if (DataLoadedInParentDataSource)
                    return DataNode.ParentDataNode.DataSource.DataTable;
                if ((DataNode.ObjectQuery is DistributedObjectQuery))
                {
                    if (_DataTable == null)
                    {
                        foreach (DataLoader dataLoader in DataLoaders.Values)
                        {
                            _DataTable = dataLoader.Data;
                            break;
                        }
                    }


                    if (_DataTable == null && DataNode.Type == DataNode.DataNodeType.Group && !GroupedDataLoaded)
                    {
                        CreateGroupDataSchema();
                        DataNode.ObjectQuery.ObjectQueryDataSet.AddTable(_DataTable);
                    }
                }
                return _DataTable;
            }
            set
            {
                _DataTable = value;
            }
        }

        /// <MetaDataID>{58ced79b-a591-4024-a07f-6df5755dd1e7}</MetaDataID>
        Dictionary<SubDataNodeIdentity, DataNodesRelationshipData> _RelationshipsData = null;
        /// <summary> 
        /// Defines relationship data with subDataNode.
        /// In case where subDataNode is ValueType then continue check  recursively to the valueType subDataNodes
        /// </summary>
        /// <MetaDataID>{c8ac8d48-1b49-455c-afef-9c0208b752cc}</MetaDataID>
        public Dictionary<SubDataNodeIdentity, DataNodesRelationshipData> RelationshipsData
        {
            get
            {
                if (DataLoadedInParentDataSource)
                {
                    Dictionary<Guid, DataNodesRelationshipData> relationshipsData = new Dictionary<SubDataNodeIdentity, DataNodesRelationshipData>();

                    DataNode parentDataNode = DataNode.ParentDataNode;
                    while (parentDataNode.DataSource.DataLoadedInParentDataSource)
                        parentDataNode = parentDataNode.ParentDataNode;

                    //foreach (var dataNodeIdentity in parentDataNode.DataSource.RelationshipsData.Keys)
                    //{
                    //    bool relationshipDataAdded = false;
                    //    foreach (DataNode dataNode in DataNode.SubDataNodes)
                    //    {
                    //        if (dataNode.ThroughRelationTable && dataNode.Identity == dataNodeIdentity)
                    //        {
                    //            relationshipsData.Add(dataNodeIdentity, parentDataNode.DataSource.RelationshipsData[dataNodeIdentity]);
                    //            relationshipDataAdded = true;
                    //            break;
                    //        }
                    //    }

                    foreach (var dataNodeIdentity in parentDataNode.DataSource.RelationshipsData.Keys)
                    {
                        bool relationshipDataAdded = false;
                        foreach (DataNode dataNode in DataNode.SubDataNodes)
                        {
                            if (dataNode.ThroughRelationTable && dataNode.Identity == dataNodeIdentity)
                            {
                                relationshipsData.Add(dataNodeIdentity, parentDataNode.DataSource.RelationshipsData[dataNodeIdentity]);
                                relationshipDataAdded = true;
                                break;
                            }
                        }
                        if (relationshipDataAdded)
                            continue;
                        if (DataNode.ValueTypePath.Count > 0)
                        {
                            foreach (DataNode dataNode in DataNode.GetValueTypeRelatedDataNodes())
                            {
                                if (dataNode.ThroughRelationTable && dataNode.Identity == dataNodeIdentity)
                                {
                                    relationshipsData.Add(dataNodeIdentity, parentDataNode.DataSource.RelationshipsData[dataNodeIdentity]);
                                    break;
                                }
                            }
                        }
                    }
                    if (_RelationshipsData == null)
                    {
                        _RelationshipsData = relationshipsData;
                    }
                    else
                    {
                        foreach (var entry in relationshipsData)
                        {
                            if (!_RelationshipsData.ContainsKey(entry.Key))
                                _RelationshipsData[entry.Key] = entry.Value;

                        }
                    }
                    return _RelationshipsData;
                }
                else
                {
                    if (_RelationshipsData == null || _RelationshipsData.Count == 0 && DataNode.ObjectQuery is DistributedObjectQuery)
                    {
                        _RelationshipsData = new OOAdvantech.Collections.Generic.Dictionary<SubDataNodeIdentity, DataNodesRelationshipData>();
                        foreach (DataLoader dataLoader in DataLoaders.Values)
                        {
                            foreach (KeyValuePair<SubDataNodeIdentity, AssotiationTableRelationshipData> entry in dataLoader.RelationshipsData)
                            {
                                DataNodesRelationshipData dataNodesRelationshipData = new DataNodesRelationshipData(DataNode.GetDataNode(entry.Value.DetailDataNodeIdentity));
                                _RelationshipsData[entry.Key] = dataNodesRelationshipData;
                                dataNodesRelationshipData.AssotiationTableRelationshipData = entry.Value;
                            }
                        }
                    }
                    return _RelationshipsData;
                }
                if (DataNode.Type == DataNode.DataNodeType.Object && DataNode.ValueTypePath.Count > 0)
                    return DataNode.ParentDataNode.DataSource.RelationshipsData;
                return _RelationshipsData;
            }
        }

        /// <summary>
        ///Defines property where when is true, DataSource data used from query engine mechanism to resolve a global resolved criterion
        /// </summary>
        /// <MetaDataID>{bd0dc2d0-9d92-453b-b6c2-09e7fe0e6f96}</MetaDataID>
        public bool ParticipateInGlobalResolvedCriterion
        {
            get
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                {
                    DataLoader dataLoader = entry.Value;
                    if (dataLoader.ParticipateInGlobalResolvedCriterion)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Defines property where when is true, DataSource data used from query engine mechanism to resolve a global resolved aggregation expression
        ///  </summary>
        /// <MetaDataID>{91ba069c-edb4-402f-9b59-7bba6fb1e6b7}</MetaDataID>
        public bool ParticipateInGlobalResolvedAggregateExpression
        {
            get
            {
                ///TODO να γραφτεί ένα testCase σενάριο όπου μία distributed query υπολογίζει τις aggregation function locally και η άλλες  
                ///distributed query δεν μπορουν να τίς επιλύσουν locally 
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                {
                    DataLoader dataLoader = entry.Value;
                    if (dataLoader.ParticipateInGlobalResolvedAggregateExpression)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Defines property where when is true, DataSource data used from query engine mechanism to resolve a global resolved group by expression
        ///  </summary>
        /// <MetaDataID>{a903b17e-f598-4b74-9ba0-b9edf9ad0565}</MetaDataID>
        public bool ParticipateInGlobalResolvedGroup
        {
            get
            {
                ///TODO να γραφτεί ένα testCase σενάριο όπου μία distributed query υπολογίζει τις aggregation function locally και η άλλες  
                ///distributed query δεν μπορουν να τίς επιλύσουν locally 
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                {
                    DataLoader dataLoader = entry.Value;
                    if (dataLoader.ParticipateInGlobalResolvedGroup)
                        return true;
                }
                return false;
            }
        }



        /// <summary>
        /// This proprty defines that DataSource must be collect data from distributed query when is true.
        /// </summary>
        /// <MetaDataID>{07867fe5-0f3d-4d66-bcdc-962229c47d1b}</MetaDataID>
        bool RetrievesDataInMainQuery
        {
            get
            {
                if (!DataNode.BranchParticipateInSelectClause &&
                    !ParticipateInGlobalResolvedCriterion &&
                    !ParticipateInGlobalResolvedGroup &&
                    !ParticipateInGlobalResolvedAggregateExpression)
                {
                    return false;
                }
                //TODO Αυτή η συνθήκη είναι επίρεπης σε σφάλμα δεν ξεθαρίζει ότι η DataNode δημιουργήθηκε μον για member fetching λόγους
                //if (!DataNode.AutoGenaratedForMembersFetching)
                if (DataNode.Type == DataNode.DataNodeType.Object && DataNode.AssignedMetaObject == null)
                    return false;
                return true;
            }
        }

        /// <exclude>Excluded</exclude>
        protected DataNodesRelationshipData _ParentRelationshipData;
        /// <MetaDataID>{0ca29072-bf2f-49b2-bc0d-87e5406a3879}</MetaDataID>
        public DataNodesRelationshipData ParentRelationshipData
        {
            get
            {
                if (_ParentRelationshipData == null && DataNode.ObjectQuery is DistributedObjectQuery)
                    foreach (DataLoader dataLoader in DataLoaders.Values)
                    {
                        if (dataLoader.ParentRelationshipData != null)
                        {
                            _ParentRelationshipData = new DataNodesRelationshipData(DataNode.ParentDataNode);
                            _ParentRelationshipData.AssotiationTableRelationshipData = dataLoader.ParentRelationshipData;
                        }
                    }
                return _ParentRelationshipData;
            }
        }

        /// <MetaDataID>{0a72123b-afe8-4974-8cb7-a96261b9e4c8}</MetaDataID>
        /// <summary>This method collects all data from the contexts which are 
        /// in different processes even in different machines, 
        /// in the process of main context of OQL query. </summary>
        internal void CollectRemoteDataInProcess(IDataSet dataSet, OOAdvantech.Collections.Generic.List<StorageDataLoader> queryResultsDataLoaders)
        {
            //if (!DataNode.BranchParticipateInSelectClause && !(DataNode.BranchParticipateInWereClause&&ParticipateInGlobalResolvedCriterion) && (!DataNode.BranchParticipateInAggregateFanction || DataNode.AggregateFanctionResultsCalculatedLocally))
            //    return;
            if (DataLoadedInParentDataSource)
                return;
            if (!RetrievesDataInMainQuery)
                return;

            if (DataNode.Type == DataNode.DataNodeType.Group && !GroupedDataLoaded)
            {

                CreateGroupDataSchema();


                foreach (var dataLoader in DataLoaders.Values)
                {
                    if (!dataLoader.GroupedDataLoaded)
                    {
                        #region Gets the empty table

                        IDataTable dataTable = null;

#if !DeviceDotNet
                        if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(dataLoader))
                            dataTable = DataObjectsInstantiator.CreateDataTable(dataLoader.RemoteData);
                        else
                        {
                            dataTable = dataLoader.Data;
                            if (dataTable == null)
                                continue;
                            if (dataTable.DataSet != null)
                            {
                                dataTable.RemoveTableRelations();
                                dataTable.DataSet.Tables.Remove(dataTable);
                            }
                        }
#else
                        dataTable = dataLoader.Data;
#endif
                        if (dataSet != dataTable.DataSet)
                            dataSet.Tables.Add(dataTable);

                        _DataTable = dataTable;

                        #endregion

                        return;
                    }
                }
            }

            _RelationshipsData = new Dictionary<Guid, DataNodesRelationshipData>();
            ICollection<DataLoader> dataLoaders = null;
            if (queryResultsDataLoaders == null)
                dataLoaders = DataLoaders.Values;
            else
            {
                DataLoaders.Clear();
                foreach (StorageDataLoader dataLoader in queryResultsDataLoaders)
                    DataLoaders[dataLoader.DataLoaderMetadata.ObjectsContextIdentity] = dataLoader;
                dataLoaders = queryResultsDataLoaders.OfType<DataLoader>().ToList();
            }

            foreach (var dataLoader in dataLoaders)
            {
                //DataLoader dataLoader = entry.Value;
                IDataTable dataTable = null;

#if !DeviceDotNet
                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(dataLoader))
                    dataTable = DataObjectsInstantiator.CreateDataTable(dataLoader.RemoteData);
                else
                {
                    dataTable = dataLoader.Data;
                    if (dataTable == null)
                        continue;
                    if (dataTable.DataSet != null)
                    {
                        dataTable.RemoveTableRelations();
                        dataTable.DataSet.RemoveTable(dataTable);
                    }
                }
#else
                dataTable = dataLoader.Data;
#endif

                if (_DataTable == null)
                {
                    _DataTable = dataTable;
                    _DataTable.OwnerDataSource = this;

                    #region Removed code

                    //_DataTable = new DataLoader.DataTable(false);
                    //(_DataTable as DataLoader.DataTable).OwnerDataSource = this;
                    //foreach (System.Data.DataColumn column in dataTable.Columns)
                    //    _DataTable.Columns.Add(column.ColumnName, column.DataType);
                    //SearchCondition constrainSearchCondition = null;
                    //foreach (var searchCondition in DataNode.HeaderDataNode.BranchSearchConditions)
                    //{
                    //    if (GetRowRemoveIndex(searchCondition) != -1)
                    //    {
                    //        constrainSearchCondition = null;
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        constrainSearchCondition = searchCondition;
                    //    }
                    //}
                    //if (constrainSearchCondition != null)
                    //{
                    //    foreach (System.Data.DataRow row in dataTable.Rows)
                    //    {
                    //        if (!constrainSearchCondition.IsRemovedRow(row, this))
                    //            _DataTable.Rows.Add(row.ItemArray);
                    //    }
                    //}
                    //else
                    //    _DataTable.Merge(dataTable);
                    #endregion
                }
                else
                {
                    #region RowRemove code
                    //_DataTable = new DataLoader.DataTable(false);
                    //(_DataTable as DataLoader.DataTable).OwnerDataSource = this;

                    //foreach (System.Data.DataColumn column in _DataTable.Columns)
                    //{
                    //    if (!dataTable.Columns.Contains(column.ColumnName))
                    //        dataTable.Columns.Add(column.ColumnName, column.DataType);
                    //}

                    //SearchCondition constrainSearchCondition = null;
                    //foreach (var searchCondition in DataNode.HeaderDataNode.BranchSearchConditions)
                    //{
                    //    if (GetRowRemoveIndex(searchCondition) != -1)
                    //    {
                    //        constrainSearchCondition = null;
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        constrainSearchCondition = searchCondition;
                    //    }
                    //}
                    //if (constrainSearchCondition != null)
                    //{
                    //    foreach (System.Data.DataRow row in dataTable.Rows)
                    //    {
                    //        if (!constrainSearchCondition.IsRemovedRow(row, this))
                    //            _DataTable.Rows.Add(row.ItemArray);
                    //    }
                    //}
                    //else
                    #endregion

                    _DataTable.Merge(dataTable);
                    //_DataTable.Merge(dataTable);

                }

                foreach (var relationshipDataEntry in dataLoader.RelationshipsData)
                {
                    SubDataNodeIdentity subDataNodeIdentity = relationshipDataEntry.Key;

                    if (RelationshipsData.ContainsKey(subDataNodeIdentity) && RelationshipsData[subDataNodeIdentity].AssotiationTableRelationshipData != null)
                    {
                        //ToDo Να γίνει ένα σενάριο με δύο βάσεις δεδομένων που θα παράγουν διαφορετικα schema Table
                        RelationshipsData[subDataNodeIdentity].AssotiationTableRelationshipData.Merge(relationshipDataEntry.Value);
                    }
                    else
                    {
                        AssotiationTableRelationshipData relationshipData = relationshipDataEntry.Value;
                        if (relationshipData.Data.DataSet != null)
                        {
                            relationshipData.Data.RemoveTableRelations();
                            relationshipData.Data.DataSet.Tables.Remove(relationshipData.Data);
                        }


                        DataNode subDataNode = DataNode.GetDataNode(subDataNodeIdentity);
                        relationshipData.Data.TableName = subDataNode.Alias + "_AssociationTable";
                        relationshipData.Data.OwnerDataSource = subDataNode.DataSource;

                        ///Collect data for subdatanodes first
                        if (DataNode.GetDataNode(subDataNodeIdentity).DataSource.ParentRelationshipData != null)
                        {
                            AssotiationTableRelationshipData subDataNodeParentRelationshipData = DataNode.GetDataNode(subDataNodeIdentity).DataSource.ParentRelationshipData.AssotiationTableRelationshipData;
                            dataSet.Tables.Remove(subDataNodeParentRelationshipData.Data);
                            relationshipData.Merge(subDataNodeParentRelationshipData);
                        }

                        dataSet.Tables.Add(relationshipData.Data);

                        if (RelationshipsData.ContainsKey(subDataNodeIdentity))
                            RelationshipsData[subDataNodeIdentity].AssotiationTableRelationshipData = relationshipData;
                        else
                        {
                            RelationshipsData[subDataNodeIdentity] = new DataNodesRelationshipData(subDataNode);
                            RelationshipsData[subDataNodeIdentity].AssotiationTableRelationshipData = relationshipData;
                        }
                    }
                }
                if (dataLoader.ParentRelationshipData != null)
                {
                    if (_ParentRelationshipData != null)
                    {
                        //ToDo Να γίνει ένα σενάριο με δύο βάσεις δεδομένων που θα παράγουν διαφορετικα schema Table
                        _ParentRelationshipData.AssotiationTableRelationshipData.Merge(dataLoader.ParentRelationshipData);
                    }
                    else
                    {
                        _ParentRelationshipData = new DataNodesRelationshipData(DataNode.ParentDataNode);
                        _ParentRelationshipData.AssotiationTableRelationshipData = dataLoader.ParentRelationshipData;
                        _ParentRelationshipData.AssotiationTableRelationshipData.Data.TableName = DataNode.Alias + "_AssociationTable";
                        _ParentRelationshipData.AssotiationTableRelationshipData.Data.OwnerDataSource = this;
                        if (_ParentRelationshipData.AssotiationTableRelationshipData.Data.DataSet != null)
                        {
                            _ParentRelationshipData.AssotiationTableRelationshipData.Data.RemoveTableRelations();
                            _ParentRelationshipData.AssotiationTableRelationshipData.Data.DataSet.Tables.Remove(_ParentRelationshipData.AssotiationTableRelationshipData.Data);
                        }
                        dataSet.Tables.Add(_ParentRelationshipData.AssotiationTableRelationshipData.Data);
                    }
                }
            }
            if (_DataTable != null)
            {
                _DataTable.TableName = DataNode.Alias;
                if (_DataTable.DataSet != null)
                    dataSet = _DataTable.DataSet;
                else
                    dataSet.AddTable(_DataTable);
            }
        }
        /// <summary>
        /// Create table schema and data source relation columns
        /// </summary>
        private void CreateGroupDataSchema()
        {
            if (DataNode.Type == DataNode.DataNodeType.Group && !GroupedDataLoaded)
            {
                #region Creates table and add colums

                _DataTable = DataObjectsInstantiator.CreateDataTable(DataNode.Alias);
                foreach (DataNode groupKeyDataNodes in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (groupKeyDataNodes.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType in groupKeyDataNodes.DataSource.ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                _DataTable.Columns.Add(groupKeyDataNodes.Alias + "_" + part.Name, part.Type);
                        }
                    }
                    else if (groupKeyDataNodes.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (groupKeyDataNodes.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            _DataTable.Columns.Add(groupKeyDataNodes.ParentDataNode.ParentDataNode.Alias + "_" + groupKeyDataNodes.ParentDataNode.Name + "_" + groupKeyDataNodes.AssignedMetaObject.Name, (groupKeyDataNodes.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                        else
                            _DataTable.Columns.Add(groupKeyDataNodes.ParentDataNode.Alias + "_" + groupKeyDataNodes.AssignedMetaObject.Name, (groupKeyDataNodes.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                    }
                }
                if (DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                {
                    foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType in DataNode.RealParentDataNode.DataSource.ObjectIdentityTypes)
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            _DataTable.Columns.Add(DataNode.RealParentDataNode.Alias + "_" + part.Name, part.Type);
                    }

                }
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode is AggregateExpressionDataNode)
                    {
                        if ((subDataNode as AggregateExpressionDataNode).ArithmeticExpression != null)
                            _DataTable.Columns.Add(subDataNode.Alias, (subDataNode as AggregateExpressionDataNode).ArithmeticExpression.ResultType);
                        else
                        {
                            if (subDataNode.Type != DataNode.DataNodeType.Count)
                                _DataTable.Columns.Add(subDataNode.Alias, ((subDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes[0].AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                            else
                                _DataTable.Columns.Add(subDataNode.Alias, typeof(ulong));
                        }
                    }
                }

                if (DataNode is GroupDataNode)
                    _DataTable.Columns.Add(DataLoader.GetGroupingDataColumnName(DataNode), typeof(object));

                _DataTable.Columns.Add("OSM_StorageIdentity", typeof(int));
                #endregion

                #region Creates data source relations columns with parent

                _DataSourceRelationsColumnsWithParent = new Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>();
                if (DataNode.RealParentDataNode != null)
                {
                    string relationPartIdentity = DataNode.FullName;
                    _DataSourceRelationsColumnsWithParent[relationPartIdentity] = new Dictionary<ObjectIdentityType, RelationColumns>();
                    if (DataNode.Type == DataNode.DataNodeType.Group && DataNode.RealParentDataNode != null)
                        _ObjectIdentityTypes = DataNode.RealParentDataNode.DataSource.ObjectIdentityTypes;
                    else
                        _ObjectIdentityTypes = (DataNode as GroupDataNode).GroupedDataNode.DataSource.ObjectIdentityTypes;
                    foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ObjectIdentityTypes)// (DataNode.GroupByDataNodeRoot.DataSource.DataLoaders[DataLoaderMetadata.StorageIdentity] as StorageDataLoader).ObjectIdentityTypes)
                    {
                        System.Collections.Generic.List<string> relationColumns = new List<string>();
                        if (!_DataSourceRelationsColumnsWithParent[relationPartIdentity].ContainsKey(objectIdentityType))
                        {
                            System.Collections.Generic.List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();

                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            {
                                relationColumns.Add(DataNode.RealParentDataNode.Alias + "_" + part.PartTypeName);
                                parts.Add(new MetaDataRepository.IdentityPart(DataNode.RealParentDataNode.Alias + "_" + part.Name, part.PartTypeName, part.Type));
                            }
                            _DataSourceRelationsColumnsWithParent[relationPartIdentity][new MetaDataRepository.ObjectIdentityType(parts)] = new RelationColumns(relationColumns, "OSM_StorageIdentity");
                        }
                    }
                }

                #endregion

                #region Creates group by key relation columns when key DataNode type is Object

                int count = 1;
                _GroupByKeyRelationColumns = new Dictionary<System.Guid, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>>();
                foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.ObjectIdentityTypes)// (dataNode.DataSource.DataLoaders[this.DataLoaderMetadata.StorageIdentity] as StorageDataLoader).DataLoaderMetadata.StorageCells)
                        {

                            List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                            if (!_GroupByKeyRelationColumns.ContainsKey(dataNode.Identity))
                                _GroupByKeyRelationColumns[dataNode.Identity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>();
                            if (!_GroupByKeyRelationColumns[dataNode.Identity].ContainsKey(objectIdentityType))
                            {
                                List<string> groupingColumnsNames = new List<string>();
                                List<string> groupedDataColumnsNames = new List<string>();
                                foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                {
                                    groupingColumnsNames.Add(dataNode.Alias + "_" + identityPart.PartTypeName);
                                    if ((dataNode.DataSource as StorageDataSource).DataLoadedInParentDataSource)
                                        groupedDataColumnsNames.Add(dataNode.Alias + "_" + identityPart.PartTypeName);
                                    else
                                        groupedDataColumnsNames.Add(identityPart.PartTypeName);
                                    parts.Add(new OOAdvantech.MetaDataRepository.IdentityPart(dataNode.Alias + "_" + identityPart.PartTypeName, identityPart.Name, identityPart.Type));
                                }
                                MetaDataRepository.ObjectIdentityType keyObjectIdentityType = new MetaDataRepository.ObjectIdentityType(parts);
                                ObjectKeyRelationColumns keyReferenceColumn = new ObjectKeyRelationColumns(keyObjectIdentityType, groupingColumnsNames, groupedDataColumnsNames, dataNode.DataSource);
                                keyReferenceColumn.RelationName = "Grouping_" + DataNode.Alias + "_" + keyReferenceColumn.DataSource.DataNode.Alias + "_" + count.ToString();
                                count++;
                                _GroupByKeyRelationColumns[dataNode.Identity][keyObjectIdentityType] = keyReferenceColumn;
                            }
                        }
                    }
                }

                #endregion

            }

        }

        //private void Merge(System.Data.DataTable targetDataTable, System.Data.DataTable dataTable)
        //{
        //    foreach (System.Data.DataColumn column in dataTable.Columns)
        //    {
        //        if (!targetDataTable.Columns.Contains(column.ColumnName))
        //            targetDataTable.Columns.Add(column.ColumnName, column.DataType);
        //    }

        //    targetDataTable.Constraints.Clear();
        //    int[] indices = new int[targetDataTable.Columns.Count];
        //    object[] values = new object[indices.Length];

        //    for (int i = 0; i != targetDataTable.Columns.Count; i++)
        //        indices[i] = dataTable.Columns.IndexOf(targetDataTable.Columns[i].ColumnName);
        //    foreach (System.Data.DataRow row in dataTable.Rows)
        //    {
        //        for (int i = 0; i != indices.Length; i++)
        //        {
        //            if (indices[i] != -1)
        //                values[i] = row[indices[i]];
        //            else
        //                values[i] = DBNull.Value;
        //        }
        //        targetDataTable.LoadDataRow(values, System.Data.LoadOption.OverwriteChanges);
        //    }
        //}




        #region Columns for DataNode SubDataNode relation

        /// <MetaDataID>{b739b5e6-5dc2-44d0-aa42-2343e9396274}</MetaDataID>
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> _DataSourceRelationsColumnsWithParent;

        /// <MetaDataID>{4feb0521-8e47-4bdd-9831-6a50479f6f61}</MetaDataID>
        /// <summary>
        /// This property defines the relation columns where needed for the relationship 
        /// with the parent data node table. 
        /// If the there is association between the classifier of node and parent node classifier, 
        /// with multiplicity manytomany, 
        /// there is relation table in parent node data source for this relationship.
        /// </summary>
        internal System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> DataSourceRelationsColumnsWithParent
        {
            get
            {
                if (_DataSourceRelationsColumnsWithParent != null)
                    return _DataSourceRelationsColumnsWithParent;

                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> relationsColumnsWithParent = new Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>();
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                {
                    DataLoader dataLoader = entry.Value;
                    foreach (KeyValuePair<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> relationPartEntry in dataLoader.DataSourceRelationsColumnsWithParent)
                    {
                        if (!relationsColumnsWithParent.ContainsKey(relationPartEntry.Key))
                            relationsColumnsWithParent[relationPartEntry.Key] = new Dictionary<ObjectIdentityType, RelationColumns>();
                        foreach (KeyValuePair<ObjectIdentityType, RelationColumns> relatedColumnsEntry in relationPartEntry.Value)
                        {
                            if (!relationsColumnsWithParent[relationPartEntry.Key].ContainsKey(relatedColumnsEntry.Key))
                                relationsColumnsWithParent[relationPartEntry.Key][relatedColumnsEntry.Key] = relatedColumnsEntry.Value;
                        }
                    }
                }
                return relationsColumnsWithParent;
            }
        }

        internal string DataSourceRelationsIndexerColumnName
        {
            get
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                {
                    DataLoader dataLoader = entry.Value;
                    return dataLoader.DataSourceRelationsIndexerColumnName;
                }
                return null;
            }
        }

        /// <exclude>Excluded</exclude>
        [NonSerialized]
        System.Collections.Generic.List<ObjectIdentityType> _ObjectIdentityTypes;
        /// <MetaDataID>{339d6bdd-39aa-40d4-971b-cd0ae05111cf}</MetaDataID>
        internal System.Collections.Generic.List<ObjectIdentityType> ObjectIdentityTypes
        {
            get
            {
                if (_ObjectIdentityTypes == null)
                {
                    _ObjectIdentityTypes = new List<ObjectIdentityType>();

                    foreach (DataLoader dataLoader in DataLoaders.Values)
                    {
                        foreach (ObjectIdentityType objectIdentityType in dataLoader.ObjectIdentityTypes)
                        {
                            if (!_ObjectIdentityTypes.Contains(objectIdentityType))
                                _ObjectIdentityTypes.Add(objectIdentityType);

                        }
                    }
                }
                return _ObjectIdentityTypes;
            }
        }



        /// <MetaDataID>{6eded13b-d3c9-4372-b0da-e2aa91c207d6}</MetaDataID>
        Dictionary<Guid, Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>> _DataSourceRelationsColumnsWithSubDataNodes;
        ///<summary>This property defines the relation columns where needed for the relationships 
        /// with the sub data nodes data tables. 
        /// You must give the sub data node as key for the relation columns. 
        /// If the there is association between the classifier of node and sub node classifier,
        /// with multiplicity manytomany, 
        /// there is relation table in data source for this relationship.
        ///</summary>
        /// <MetaDataID>{80f2e103-d8f4-42f7-9b64-a7535d5ca4bf}</MetaDataID>
        internal Dictionary<Guid, Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get
            {
                if (_DataSourceRelationsColumnsWithSubDataNodes != null)
                    return _DataSourceRelationsColumnsWithSubDataNodes;

                Dictionary<Guid, Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>> relationsColumnsWithSubDataNodes = new Dictionary<Guid, Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>>();
                foreach (DataLoader dataLoader in DataLoaders.Values)
                {
                    Dictionary<Guid, Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>> dataLoaderRelationsColumnsWithSubDataNodes = dataLoader.DataSourceRelationsColumnsWithSubDataNodes;

                    foreach (Guid subDataNodeIdentity in dataLoaderRelationsColumnsWithSubDataNodes.Keys)
                    {
                        if (!relationsColumnsWithSubDataNodes.ContainsKey(subDataNodeIdentity))
                            relationsColumnsWithSubDataNodes[subDataNodeIdentity] = dataLoaderRelationsColumnsWithSubDataNodes[subDataNodeIdentity];
                        else
                        {
                            foreach (KeyValuePair<string, Dictionary<ObjectIdentityType, RelationColumns>> relationPartEntry in dataLoaderRelationsColumnsWithSubDataNodes[subDataNodeIdentity])
                            {
                                foreach (KeyValuePair<ObjectIdentityType, RelationColumns> entry in relationPartEntry.Value)
                                    relationsColumnsWithSubDataNodes[subDataNodeIdentity][relationPartEntry.Key][entry.Key] = entry.Value;
                            }
                        }
                    }

                }
                return relationsColumnsWithSubDataNodes;
            }
        }
        #endregion

        #region Columns for recursive relation
        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> _RecursiveDetailsColumns;
        /// <summary>
        /// When the Data Node marked as Recursive the system must define a relation for recursion
        /// This property defines the details columns names for this relation 
        /// </summary>
        /// <MetaDataID>{a3e65e5f-9084-4dff-ab48-3467f5a7d30a}</MetaDataID>
        internal System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> RecursiveDetailsColumns
        {
            get
            {

                if (_RecursiveDetailsColumns == null)
                {
                    _RecursiveDetailsColumns = new Dictionary<string, Dictionary<ObjectIdentityType, List<string>>>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                    {
                        DataLoader dataLoader = entry.Value;
                        foreach (KeyValuePair<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> relationPartEntry in dataLoader.RecursiveParentReferenceColumns)
                        {
                            if (!_RecursiveDetailsColumns.ContainsKey(relationPartEntry.Key))
                                _RecursiveDetailsColumns[relationPartEntry.Key] = new Dictionary<ObjectIdentityType, List<string>>();
                            foreach (KeyValuePair<ObjectIdentityType, System.Collections.Generic.List<string>> relatedColumnsEntry in relationPartEntry.Value)
                            {
                                if (!_RecursiveDetailsColumns[relationPartEntry.Key].ContainsKey(relatedColumnsEntry.Key))
                                    _RecursiveDetailsColumns[relationPartEntry.Key][relatedColumnsEntry.Key] = relatedColumnsEntry.Value;
                            }
                        }
                    }
                }
                return _RecursiveDetailsColumns;
            }
        }
        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> _RecursiveMasterColumns;
        /// <summary>
        /// When the Data Node marked as Recursive the system must define a relation for recursion
        /// This property defines the details columns names for this relation 
        /// </summary>
        /// <MetaDataID>{20078342-ddce-4bdb-b6d2-096dc11b42d8}</MetaDataID>
        internal System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> RecursiveMasterColumns
        {
            get
            {
                if (_RecursiveMasterColumns == null)
                {
                    var recursiveMasterColumns = new Dictionary<string, Dictionary<ObjectIdentityType, List<string>>>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                    {
                        DataLoader dataLoader = entry.Value;
                        foreach (KeyValuePair<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> relationPartEntry in dataLoader.RecursiveParentColumns)
                        {
                            if (!recursiveMasterColumns.ContainsKey(relationPartEntry.Key))
                                recursiveMasterColumns[relationPartEntry.Key] = new Dictionary<ObjectIdentityType, List<string>>();
                            foreach (KeyValuePair<ObjectIdentityType, System.Collections.Generic.List<string>> relatedColumnsEntry in relationPartEntry.Value)
                            {
                                if (!recursiveMasterColumns[relationPartEntry.Key].ContainsKey(relatedColumnsEntry.Key))
                                    recursiveMasterColumns[relationPartEntry.Key][relatedColumnsEntry.Key] = relatedColumnsEntry.Value;
                            }
                        }
                    }
                    _RecursiveMasterColumns = recursiveMasterColumns;
                }

                return _RecursiveMasterColumns;
            }
        }
        #endregion

        #region Columns for Group By KeyRelation


        /// <exclude>Excluded</exclude>
        internal Dictionary<System.Guid, Dictionary<ObjectIdentityType, ObjectKeyRelationColumns>> _GroupByKeyRelationColumns;
        /// <MetaDataID>{10f519f3-7d5b-41c8-af19-09288249d34c}</MetaDataID>
        /// <summary>
        /// GroupByKeyRelationColumns used from query engine to create a relation between grouped data table and table with object.
        /// Objects participate as key in grouping data.  
        /// </summary>
        internal Dictionary<System.Guid, Dictionary<ObjectIdentityType, ObjectKeyRelationColumns>> GroupByKeyRelationColumns
        {
            get
            {
                int count = 1;
                if (_GroupByKeyRelationColumns == null)
                {
                    _GroupByKeyRelationColumns = new Dictionary<Guid, Dictionary<ObjectIdentityType, ObjectKeyRelationColumns>>();
                    foreach (DataLoader dataLoader in DataLoaders.Values)
                    {
                        foreach (KeyValuePair<System.Guid, Dictionary<ObjectIdentityType, ObjectKeyRelationColumns>> entry in dataLoader.GroupByKeyRelationColumns)
                        {
                            Dictionary<ObjectIdentityType, ObjectKeyRelationColumns> keysRelatedColumns = null;
                            if (!_GroupByKeyRelationColumns.TryGetValue(entry.Key, out keysRelatedColumns))
                            {
                                keysRelatedColumns = new Dictionary<ObjectIdentityType, ObjectKeyRelationColumns>();
                                _GroupByKeyRelationColumns[entry.Key] = keysRelatedColumns;
                            }
                            foreach (ObjectIdentityType objectIdentityType in entry.Value.Keys)
                            {
                                if (!keysRelatedColumns.ContainsKey(objectIdentityType))
                                {
                                    ObjectKeyRelationColumns keyRelatedColumns = entry.Value[objectIdentityType];
                                    keyRelatedColumns.RelationName = "Grouping_" + DataNode.Alias + "_" + keyRelatedColumns.DataSource.DataNode.Alias + "_" + count.ToString();
                                    count++;
                                    keysRelatedColumns[objectIdentityType] = keyRelatedColumns;
                                }
                            }
                        }
                    }
                }
                return _GroupByKeyRelationColumns;
            }
        }

        #endregion


        /// <MetaDataID>{4912ecbd-e7d9-43bf-b2c9-cb24b63f66e9}</MetaDataID>
        /// <summary>
        /// The property GroupedDataLoaded defines if the grouping data loaded localy from data loaders or produced from query engine.
        /// If it is true the grouping data loaded localy from data loaders. 
        /// </summary>
        public bool GroupedDataLoaded
        {
            get
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                {
                    DataLoader dataLoader = entry.Value;
                    if (!dataLoader.GroupedDataLoaded)
                        return false;
                }
                return true;
            }



        }

        /// <MetaDataID>{ced13fa5-e9c5-45bf-81e3-c5e78f7e11c6}</MetaDataID>
        /// <summary>
        ///If aggregateExpressionDataNode resolved from all data loaders locally then returns true
        ///else returns false
        /// </summary>
        public bool AggregateExpressionDataNodeResolved(AggregateExpressionDataNode aggregateExpressionDataNode)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
            {
                DataLoader dataLoader = entry.Value;
                if (!dataLoader.AggregateExpressionDataNodeResolved(aggregateExpressionDataNode.Identity))
                    return false;
            }
            return true;
        }

        /// <summary>
        ///  If it is true the data source table has the identity coloumns 
        /// of foreign key relation, otherwise table has the reference columns. 
        /// Always the identity columns has unique constrain 
        /// </summary>
        /// <MetaDataID>{80198429-458d-496d-8d1e-fba0cd7c9dbd}</MetaDataID>
        public bool HasRelationIdentityColumns
        {
            get
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in DataLoaders)
                {
                    DataLoader dataLoader = entry.Value;
                    return dataLoader.HasRelationIdentityColumns;
                }
                return false;
            }
        }
        /// <summary>
        ///  If it is true the data source table has the reference columns
        /// of foreign key relation, otherwise table has the identity coloumns. 
        /// Always the identity columns has unique constrain 
        /// </summary>
        /// <MetaDataID>{d13a5a40-33c0-4856-b7d4-c6f47c0b2275}</MetaDataID>
        public bool HasRelationReferenceColumns
        {
            get
            {
                OOAdvantech.Member<bool> hasRelationReferenceColumns = new Member<bool>();
                foreach (DataLoader dataLoader in DataLoaders.Values)
                {

                    if (hasRelationReferenceColumns.UnInitialized)
                    {
                        hasRelationReferenceColumns.Value = dataLoader.HasRelationReferenceColumns;
                    }
                    else
                    {
                        bool dataLoaderHasRelationReferenceColumns = dataLoader.HasRelationReferenceColumns;
                        if (dataLoaderHasRelationReferenceColumns != hasRelationReferenceColumns.Value)
                        {
                            hasRelationReferenceColumns.Value = false;
                            break;
                        }
                    }
                }
                if (hasRelationReferenceColumns.UnInitialized)
                    return false;
                return hasRelationReferenceColumns.Value;
            }
        }
        /// <MetaDataID>{9947265e-990f-4995-8605-a17f7f2a5526}</MetaDataID>
        /// <summary>
        /// This fanction check relation between data source and sub datanode data source. 
        /// If data source has the reference columns return true else return false 
        /// </summary>
        public bool HasRelationReferenceColumnsFor(DataNode subDataNode)
        {
            OOAdvantech.Member<bool> hasRelationReferenceColumns = new Member<bool>();
            foreach (DataLoader dataLoader in DataLoaders.Values)
            {

                if (hasRelationReferenceColumns.UnInitialized)
                    hasRelationReferenceColumns.Value = dataLoader.HasRelationReferenceColumnsFor(subDataNode);
                else
                {
                    bool dataLoaderHasRelationReferenceColumns = dataLoader.HasRelationReferenceColumnsFor(subDataNode);
                    if (dataLoaderHasRelationReferenceColumns != hasRelationReferenceColumns.Value)
                    {
                        hasRelationReferenceColumns.Value = false;
                        break;
                    }
                }
            }
            if (hasRelationReferenceColumns.UnInitialized)
                return false;
            return hasRelationReferenceColumns.Value;
        }

        /// <summary>
        ///ThereAreObjectsToActivate is true in case where there are objects in passive mode 
        ///and system must be activate.    
        /// </summary>
        /// <MetaDataID>{1c7acdfe-2837-4906-bf60-de6f203f5d3c}</MetaDataID>
        abstract internal bool ThereAreObjectsToActivate
        {
            get;
        }



        /// <MetaDataID>{c9dd1233-6cb6-4ea7-b55a-c2e61a244711}</MetaDataID>
        /// <summary>
        /// If property is true query engine use from parent data node table, child relation to retrieve data
        /// otherwise use parent relation.
        /// </summary>
        bool? RetrieveRowsThroughParentChildRelation;

        /// <MetaDataID>{6218050f-888c-48a6-acb8-8f09d4798eb7}</MetaDataID>
        /// <summary>
        /// If data source manage out storage data the property value is true. 
        /// </summary>
        public virtual bool HasOutObjectContextData
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{e6f68ef3-d375-4701-8886-83875b0da9cd}</MetaDataID>
        /// <summary>
        /// If data source manage in storage data the property value is true.
        /// There are cases where data source exist to define only relation 
        /// with data in other storage and the property value is false.
        /// </summary>
        public virtual bool HasInObjectContextData
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{2a0660dc-b0cf-4f36-b57e-f81ec4a1d4eb}</MetaDataID>
        public virtual bool HasDataInObjectContext(string storageIdentity)
        {
            return true;
        }

#if !DeviceDotNet
        /// <summary>
        /// This method creates table relations and adds them to the DataSet.
        /// For sub data nodes which have many to many with parent the system use relation table.
        /// </summary>
        /// <MetaDataID>{c942631f-ee03-4790-bcc6-f5705081f19d}</MetaDataID>
        public virtual void BuildTablesRelationsA()
        {

            if (DataTable != null)
            {
                if (DataNode.Type == DataNode.DataNodeType.Group)
                {
                    #region Builds relation in case where key data node is object
                    foreach (Dictionary<MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns> GroupedDataNodeKeyColumns in DataNode.DataSource.GroupByKeyRelationColumns.Values)
                    {
                        foreach (KeyValuePair<MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns> entry in GroupedDataNodeKeyColumns)
                        {
                            List<string> groupKeyColumns = entry.Value.GroupingColumnsNames;
                            List<string> groupedKeyColumns = entry.Value.GroupedDataColumnsNames;
                            System.Collections.Generic.Dictionary<Guid, DataSource> dataSources = null;
                            this.DataNode.GetDataSources(ref dataSources);
                            DataSource keyDataSource = dataSources[entry.Value.DataSource.Identity];
                            if (keyDataSource.DataTable == null)
                                continue;
                            IDataColumn[] columnsInGroupTable = GetTableColumns(DataTable, groupKeyColumns);
                            IDataColumn[] columnsInGroupedDataSourceTable = GetTableColumns(keyDataSource.DataTable, groupedKeyColumns);
                            IDataRelation relation = new DataRelation(entry.Value.RelationName, columnsInGroupTable, columnsInGroupedDataSourceTable, false);
                            DataTable.DataSet.Relations.Add(relation);
                        }
                    }
                    #endregion
                    return;
                }
                if (!HasInObjectContextData)
                    return;

                if (DataNode.Recursive && this.DataSourceRelationsColumnsWithParent.Count > 0)
                {
                    #region Add relation for recursion

                    Guid recursiveRelationDataIdentity = DataNode.Identity;
                    RelationshipsData[recursiveRelationDataIdentity] = new DataNodesRelationshipData(DataNode);

                    foreach (KeyValuePair<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> relationPartEntry in RecursiveDetailsColumns)
                    {
                        foreach (KeyValuePair<ObjectIdentityType, System.Collections.Generic.List<string>> entry in RecursiveDetailsColumns[relationPartEntry.Key])
                        {
                            IDataColumn[] masterColumns = GetTableColumns(DataTable, RecursiveMasterColumns[relationPartEntry.Key][entry.Key]);
                            IDataColumn[] detailsColumns = GetTableColumns(DataTable, entry.Value);
                            IDataRelation relation = null;
                            if (RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames.Count == 0)
                            {
                                relation = new DataRelation("Recursive_" + DataNode.Alias, masterColumns, detailsColumns, false);
                                RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames.Add("Recursive_" + DataNode.Alias);
                            }
                            else
                            {
                                relation = new DataRelation("Recursive_" + DataNode.Alias + "_" + RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames.Count, masterColumns, detailsColumns, false);
                                RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames.Add("Recursive_" + DataNode.Alias + "_" + RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames.Count);
                            }
                            DataTable.DataSet.Relations.Add(relation);
                        }
                    }
                    #endregion
                }
                List<DataNode> relatedDataNodes = new List<DataNode>();

                #region retrieve data nodes for parent sub datanode relation build
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Object)
                        relatedDataNodes.Add(subDataNode);
                    else if (subDataNode.Type == DataNode.DataNodeType.Group && DataNode.Type == DataNode.DataNodeType.Object)
                    {
                        relatedDataNodes.AddRange((subDataNode as GroupDataNode).GroupingDataNodes);
                        relatedDataNodes.Add(subDataNode);
                    }
                }
                #endregion

                foreach (DataNode subDataNode in relatedDataNodes)
                {
                    #region Skip this datanode there aren't to build relation
                    if (subDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        if (DropTableDataForUnachievedConstraintCriterion(subDataNode))
                            return;
                    }

                    if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject == null)
                        continue;

                    if (subDataNode.Type == DataNode.DataNodeType.Group)
                    {
                        DataNode dataSourceDataNode = (subDataNode as GroupDataNode).GroupedDataNodeRoot;
                        while (dataSourceDataNode.Type == DataNode.DataNodeType.Group)
                            dataSourceDataNode = (dataSourceDataNode as GroupDataNode).GroupedDataNodeRoot;
                        if (dataSourceDataNode.AssignedMetaObject == null)
                            continue;
                    }

                    if (!(DataNode.ObjectQuery is DistributedObjectQuery) && subDataNode.DataSource.DataTable == null)
                        continue;
                    if (subDataNode.DataSource.DataLoadedInParentDataSource)
                        continue;



                    if ((DataNode.ObjectQuery is DistributedObjectQuery) && (subDataNode.DataSource.DataTable == null))
                    {
                        if (subDataNode.Type == DataNode.DataNodeType.Group)
                            continue;
                        StorageDataLoader dataLoader = DataLoaders[((DataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity] as StorageDataLoader;
                        if (!dataLoader.DataLoaderMetadata.SubDataNodeOutStorageRelations.ContainsKey(subDataNode.AssignedMetaObject.Identity) ||
                            !dataLoader.DataLoaderMetadata.SubDataNodeOutStorageRelations[subDataNode.AssignedMetaObject.Identity])
                            continue;
                    }
                    //else
                    //    if (!(DataNode.ObjectQuery is DistributedObjectQuery) && subDataNode.DataSource.DataTable == null)
                    //        continue;
                    #endregion

                    Guid subDataNodeRelationDataIdentity = subDataNode.Identity;
                    //if (subDataNode.Type == DataNode.DataNodeType.Group)
                    //    subDataNodeRelationDataIdentity = subDataNode.Identity;
                    //else
                    //    subDataNodeRelationDataIdentity = subDataNode.Identity;
                    if (!RelationshipsData.ContainsKey(subDataNodeRelationDataIdentity))
                        RelationshipsData[subDataNodeRelationDataIdentity] = new DataNodesRelationshipData(subDataNode);
                    MetaDataRepository.MetaObject assignedMetaObject = subDataNode.AssignedMetaObject;

                    if ((assignedMetaObject is MetaDataRepository.AssociationEnd &&
                        !DataTable.DataSet.Relations.Contains(subDataNode.Alias) &&
                        !subDataNode.DataSource.DataLoadedInParentDataSource) ||
                        (subDataNode.MembersFetchingObjectActivation && subDataNode.Type == DataNode.DataNodeType.Object)
                        || subDataNode.Type == DataNode.DataNodeType.Group)
                    {

                        AssotiationTableRelationshipData relationshipData = null;
                        if ((subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) && subDataNode.ThroughRelationTable)
                            relationshipData = RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData;

                        IDataColumn[] columnsInDataSourceTable = null;
                        IDataColumn[] columnsInSubNodeDataSourceTable = null;
                        if (relationshipData == null)
                            relationshipData = subDataNode.DataSource.ParentRelationshipData.AssotiationTableRelationshipData;


                        if (relationshipData != null)
                        {
                            if (relationshipData.Data.DataSet == null)
                            {
                                relationshipData.Data.TableName = subDataNode.Alias + "_AssociationTable";
                                DataTable.DataSet.AddTable(relationshipData.Data);
                            }
                            List<string> columnsNames = new List<string>();
                            string columnPrefix = null;


                            foreach (var relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity].Keys)
                            {
                                foreach (var entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][relationPartIdentity])
                                {
                                    ObjectIdentityType objectIdentityType = entry.Key;
                                    columnsNames.Clear();
                                    //foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    //    columnsNames.Add(part.Name);
                                    columnsNames.AddRange(entry.Value.ObjectIdentityColumns);
                                    columnsInDataSourceTable = GetTableColumns(DataTable, columnsNames);
                                    IDataColumn[] associationTableColumnsDataSourceRelated = GetTableColumns(relationshipData.Data, relationshipData.GetDataSourceRelatedColumns(relationPartIdentity, objectIdentityType, DataNode.ObjectQuery.UseStorageIdintityInTablesRelations));
                                    if (RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count == 0)
                                        RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias, objectIdentityType));
                                    else
                                        RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_" + ((int)(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count + 1)).ToString(), objectIdentityType));
                                    IDataRelation relation = new DataRelation(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData[RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count - 1].RelationName, columnsInDataSourceTable, associationTableColumnsDataSourceRelated, false);
                                    DataTable.DataSet.Relations.Add(relation);
                                }
                            }
                            if (subDataNode.DataSource.HasInObjectContextData)
                            {
                                foreach (string relationPartIdentity in subDataNode.DataSource.DataSourceRelationsColumnsWithParent.Keys)
                                {
                                    foreach (var entry in subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity])
                                    {
                                        ObjectIdentityType objectIdentityType = entry.Key;

                                        IDataColumn[] associationTableColumnsSubNodeDataSourceRelated = GetTableColumns(relationshipData.Data, relationshipData.GetSubNodeDataSourceRelatedColumns(relationPartIdentity, objectIdentityType, DataNode.ObjectQuery.UseStorageIdintityInTablesRelations));
                                        columnsNames.Clear();
                                        //foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                        //    columnsNames.Add(part.Name);
                                        columnsNames.AddRange(entry.Value.ObjectIdentityColumns);
                                        columnsInSubNodeDataSourceTable = GetTableColumns(subDataNode.DataSource.DataTable, columnsNames);
                                        if (RelationshipsData[subDataNodeRelationDataIdentity].AssociationRelationsData.Count == 0)
                                            RelationshipsData[subDataNodeRelationDataIdentity].AssociationRelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_AssociationTable", objectIdentityType));
                                        else
                                            RelationshipsData[subDataNodeRelationDataIdentity].AssociationRelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_AssociationTable_" + ((int)(RelationshipsData[subDataNodeRelationDataIdentity].AssociationRelationsData.Count + 1)).ToString(), objectIdentityType));
                                        IDataRelation relation = new DataRelation(RelationshipsData[subDataNodeRelationDataIdentity].AssociationRelationsData[RelationshipsData[subDataNodeRelationDataIdentity].AssociationRelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, associationTableColumnsSubNodeDataSourceRelated, false);
                                        DataTable.DataSet.Relations.Add(relation);
                                    }
                                }
                            }
                        }
                        else if (subDataNode.DataSource.HasRelationIdentityColumns &&
                                (subDataNode.AssignedMetaObject as AssociationEnd).Association.MultiplicityType != AssociationType.ManyToMany)
                        {
                            foreach (var relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity].Keys)
                            {

                                foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, RelationColumns> entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][relationPartIdentity])
                                {
                                    if (!subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity].ContainsKey(entry.Key))
                                        continue;
                                    ObjectIdentityType objectIdentityType = entry.Key;
                                    columnsInDataSourceTable = GetTableColumns(DataTable, entry.Value.ObjectIdentityColumns);
                                    columnsInSubNodeDataSourceTable = GetTableColumns(subDataNode.DataSource.DataTable, subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity][entry.Key].ObjectIdentityColumns);
                                    if (RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count == 0)
                                        RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias, objectIdentityType));
                                    else
                                        RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_" + ((int)(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count + 1)).ToString(), objectIdentityType));

                                    IDataRelation relation = new DataRelation(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData[RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                                    DataTable.DataSet.Relations.Add(relation);
                                }
                            }
                        }
                        else if (subDataNode.DataSource.HasRelationReferenceColumns)
                        {
                            foreach (var relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity].Keys)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, RelationColumns> entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][relationPartIdentity])
                                {
                                    if (subDataNode.DataSource.DataTable != null)
                                    {
                                        if (!subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity].ContainsKey(entry.Key))
                                            continue;

                                        ObjectIdentityType objectIdentityType = entry.Key;

                                        columnsInDataSourceTable = GetTableColumns(DataTable, entry.Value.ObjectIdentityColumns);
                                        columnsInSubNodeDataSourceTable = GetTableColumns(subDataNode.DataSource.DataTable, subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity][entry.Key].ObjectIdentityColumns);
                                        if (RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count == 0)
                                            RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias, objectIdentityType));
                                        else
                                            RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_" + ((int)(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count + 1)).ToString(), objectIdentityType));

                                        IDataRelation relation = new DataRelation(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData[RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count - 1].RelationName, columnsInDataSourceTable, columnsInSubNodeDataSourceTable, false);
                                        DataTable.DataSet.Relations.Add(relation);
                                    }
                                }
                            }
                        }
                        else if (subDataNode.AssignedMetaObject is AssociationEnd && DataNode.Classifier.ClassHierarchyLinkAssociation == (subDataNode.AssignedMetaObject as AssociationEnd).Association)
                        {
                            foreach (var relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity].Keys)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, RelationColumns> entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][relationPartIdentity])
                                {
                                    if (!subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity].ContainsKey(entry.Key))
                                        continue;
                                    ObjectIdentityType objectIdentityType = entry.Key;

                                    columnsInDataSourceTable = GetTableColumns(DataTable, entry.Value.ObjectIdentityColumns);
                                    columnsInSubNodeDataSourceTable = GetTableColumns(subDataNode.DataSource.DataTable, subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity][entry.Key].ObjectIdentityColumns);
                                    if (RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count == 0)
                                        RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias, objectIdentityType));
                                    else
                                        RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_" + ((int)(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count + 1)).ToString(), objectIdentityType));

                                    IDataRelation relation = new DataRelation(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData[RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                                    DataTable.DataSet.Relations.Add(relation);
                                }
                            }
                        }
                        else
                        {
                            foreach (var relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity].Keys)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, RelationColumns> entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][relationPartIdentity])
                                {
                                    if (subDataNode.DataSource.DataTable == null || !subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity].ContainsKey(entry.Key))
                                        continue;

                                    ObjectIdentityType objectIdentityType = entry.Key;

                                    columnsInDataSourceTable = GetTableColumns(DataTable, entry.Value.ObjectIdentityColumns);
                                    columnsInSubNodeDataSourceTable = GetTableColumns(subDataNode.DataSource.DataTable, subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity][entry.Key].ObjectIdentityColumns);
                                    if (RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count == 0)
                                        RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias, objectIdentityType));
                                    else
                                        RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_" + ((int)(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count + 1)).ToString(), objectIdentityType));
                                    IDataRelation relation = new DataRelation(RelationshipsData[subDataNodeRelationDataIdentity].RelationsData[RelationshipsData[subDataNodeRelationDataIdentity].RelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                                    DataTable.DataSet.Relations.Add(relation);
                                }
                            }
                        }
                    }
                }
            }

        }
#endif
        internal bool TablesRelationsLoaded;

        /// <summary>
        /// This method creates table relations and adds them to the DataSet.
        /// For sub data nodes which have many to many with parent the system use relation table.
        /// </summary>
        /// <MetaDataID>{c942631f-ee03-4790-bcc6-f5705081f19d}</MetaDataID>
        public virtual void BuildTablesRelations()
        {
            if (TablesRelationsLoaded)
                return;

            TablesRelationsLoaded = true;
            if (DataTable != null)
            {
                if (DataNode.Type == DataNode.DataNodeType.Group)
                {
                    if (DataTable != null && DataTable.Columns.Count > 0)
                    {
                        #region Builds relation in case where key data node is object
                        foreach (Dictionary<MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns> GroupedDataNodeKeyColumns in DataNode.DataSource.GroupByKeyRelationColumns.Values)
                        {
                            foreach (KeyValuePair<MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns> entry in GroupedDataNodeKeyColumns)
                            {
                                List<string> groupKeyColumns = entry.Value.GroupingColumnsNames;
                                List<string> groupedKeyColumns = entry.Value.GroupedDataColumnsNames;
                                System.Collections.Generic.Dictionary<Guid, DataSource> dataSources = null;
                                this.DataNode.GetDataSources(ref dataSources);
                                DataSource keyDataSource = dataSources[entry.Value.DataSource.Identity];
                                if (keyDataSource.DataTable == null)
                                    continue;
                                IDataColumn[] columnsInGroupTable = GetTableColumns(DataTable, groupKeyColumns);
                                IDataColumn[] columnsInGroupedDataSourceTable = GetTableColumns(keyDataSource.DataTable, groupedKeyColumns);
                                DataTable.DataSet.Relations.Add(entry.Value.RelationName, columnsInGroupTable, columnsInGroupedDataSourceTable, false);
                                //IDataRelation relation = new DataRelation(entry.Value.RelationName, columnsInGroupTable, columnsInGroupedDataSourceTable, false);
                                //DataTable.DataSet.Relations.Add(relation);
                            }
                        }
                        #endregion
                    }
                    return;
                }
                if (!HasInObjectContextData)
                    return;

                if (DataNode.Recursive && this.DataSourceRelationsColumnsWithParent.Count > 0)
                {
                    #region Add relation for recursion

                    Guid recursiveRelationDataIdentity = DataNode.Identity;
                    RelationshipsData[recursiveRelationDataIdentity] = new DataNodesRelationshipData(DataNode);

                    foreach (KeyValuePair<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> relationPartEntry in RecursiveDetailsColumns)
                    {
                        string relationPartIdentity = relationPartEntry.Key;
                        foreach (KeyValuePair<ObjectIdentityType, System.Collections.Generic.List<string>> entry in RecursiveDetailsColumns[relationPartIdentity])
                        {
                            ObjectIdentityType objectIdentityType = entry.Key;

                            IDataColumn[] masterColumns = GetTableColumns(DataTable, RecursiveMasterColumns[relationPartIdentity][objectIdentityType]);
                            IDataColumn[] detailsColumns = GetTableColumns(DataTable, entry.Value);
                            IDataRelation relation = null;

                            PartialRelationData partialRelationData;
                            if (GetPartialRelationData(relationPartIdentity, objectIdentityType, RelationshipsData[recursiveRelationDataIdentity].RelationsData, out partialRelationData))
                            {
                                DataTable.DataSet.Relations.Add(partialRelationData.RelationName, masterColumns, detailsColumns, false);
                                //relation = new DataRelation(partialRelationData.RelationName, masterColumns, detailsColumns, false);
                                //DataTable.DataSet.Relations.Add(relation);
                            }
                            else
                            {
                                if (RelationshipsData[recursiveRelationDataIdentity].RelationsData.Count == 0)
                                    RelationshipsData[recursiveRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, "Recursive_" + DataNode.Alias, objectIdentityType));
                                else
                                    RelationshipsData[recursiveRelationDataIdentity].RelationsData.Add(new PartialRelationData(relationPartIdentity, "Recursive_" + DataNode.Alias + RelationshipsData[recursiveRelationDataIdentity].RelationsData.Count.ToString(), objectIdentityType));
                                DataTable.DataSet.Relations.Add(RelationshipsData[recursiveRelationDataIdentity].RelationsData[RelationshipsData[recursiveRelationDataIdentity].RelationsData.Count - 1].RelationName, masterColumns, detailsColumns, false);
                                //relation = new DataRelation(RelationshipsData[recursiveRelationDataIdentity].RelationsData[RelationshipsData[recursiveRelationDataIdentity].RelationsData.Count - 1].RelationName, masterColumns, detailsColumns, false);
                                //DataTable.DataSet.Relations.Add(relation);
                            }

                        }
                    }
                    #endregion
                }
                List<DataNode> relatedDataNodes = new List<DataNode>();

                #region Retrieve data nodes for parent sub datanode relation build
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Object)
                        relatedDataNodes.Add(subDataNode);
                    else if (subDataNode.Type == DataNode.DataNodeType.Group && DataNode.Type == DataNode.DataNodeType.Object)
                    {
                        relatedDataNodes.AddRange((subDataNode as GroupDataNode).GroupingDataNodes);
                        relatedDataNodes.Add(subDataNode);
                    }
                }
                #endregion

                foreach (DataNode subDataNode in relatedDataNodes)
                {
                    #region Skip this datanode there aren't to build relation
                    if (subDataNode.Type == DataNode.DataNodeType.Object)
                    {
                        if (DropTableDataForUnachievedConstraintCriterion(subDataNode))
                            return;
                    }
                    Guid subDataNodeRelationDataIdentity = subDataNode.Identity;
                    if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject == null && !RelationshipsData.ContainsKey(subDataNodeRelationDataIdentity))
                        continue;

                    if (subDataNode.Type == DataNode.DataNodeType.Group)
                    {
                        DataNode dataSourceDataNode = (subDataNode as GroupDataNode).GroupedDataNodeRoot;
                        while (dataSourceDataNode.Type == DataNode.DataNodeType.Group)
                            dataSourceDataNode = (dataSourceDataNode as GroupDataNode).GroupedDataNodeRoot;
                        if (dataSourceDataNode.AssignedMetaObject == null)
                            continue;
                    }

                    if (!(DataNode.ObjectQuery is DistributedObjectQuery) && subDataNode.DataSource.DataTable == null)
                        continue;
                    if (subDataNode.DataSource.DataLoadedInParentDataSource)
                        continue;



                    if ((DataNode.ObjectQuery is DistributedObjectQuery) && (subDataNode.DataSource.DataTable == null))
                    {
                        if (subDataNode.Type == DataNode.DataNodeType.Group)
                            continue;
                        StorageDataLoader dataLoader = DataLoaders[((DataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectsContext).Identity] as StorageDataLoader;
                        if (!dataLoader.DataLoaderMetadata.SubDataNodeOutStorageRelations.ContainsKey(subDataNode.AssignedMetaObject.Identity) ||
                            !dataLoader.DataLoaderMetadata.SubDataNodeOutStorageRelations[subDataNode.AssignedMetaObject.Identity])
                            continue;
                    }
                    //else
                    //    if (!(DataNode.ObjectQuery is DistributedObjectQuery) && subDataNode.DataSource.DataTable == null)
                    //        continue;
                    #endregion


                    if (!RelationshipsData.ContainsKey(subDataNodeRelationDataIdentity))
                    {
                        if (subDataNode.DataSource.ParentRelationshipData == null)
                            RelationshipsData[subDataNodeRelationDataIdentity] = new DataNodesRelationshipData(subDataNode);

                    }
                    MetaDataRepository.MetaObject assignedMetaObject = subDataNode.AssignedMetaObject;

                    if ((assignedMetaObject is MetaDataRepository.AssociationEnd &&
                        !DataTable.DataSet.Relations.Contains(subDataNode.Alias) &&
                        !subDataNode.DataSource.DataLoadedInParentDataSource) ||
                        (subDataNode.MembersFetchingObjectActivation && subDataNode.Type == DataNode.DataNodeType.Object)
                        || subDataNode.Type == DataNode.DataNodeType.Group)
                    {

                        IDataColumn[] columnsInDataSourceTable = null;
                        IDataColumn[] columnsInSubNodeDataSourceTable = null;

                        if (subDataNode.ThroughRelationTable)
                        {
                            AssotiationTableRelationshipData relationshipData = null;
                            DataNodesRelationshipData dataNodesRelationshipData = null;
                            if (RelationshipsData.ContainsKey(subDataNode.Identity))
                            {
                                relationshipData = RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData;
                                dataNodesRelationshipData = RelationshipsData[subDataNodeRelationDataIdentity];
                            }
                            else
                            {
                                relationshipData = subDataNode.DataSource.ParentRelationshipData.AssotiationTableRelationshipData;
                                dataNodesRelationshipData = subDataNode.DataSource.ParentRelationshipData;
                            }

                            #region Builds relation with association table


                            if (relationshipData.Data.DataSet == null)
                            {
                                relationshipData.Data.TableName = subDataNode.Alias + "_AssociationTable";
                                DataTable.DataSet.AddTable(relationshipData.Data);
                            }

                            List<string> columnsNames = new List<string>();
                            foreach (var relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity].Keys)
                            {
                                //Create relation between DataSource table and association table
                                foreach (var entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][relationPartIdentity])
                                {
                                    ObjectIdentityType objectIdentityType = entry.Key;
                                    columnsNames.Clear();

                                    columnsNames.AddRange(entry.Value.ObjectIdentityColumns);
                                    if (DataNode.ObjectQuery.UseStorageIdintityInTablesRelations)
                                        columnsNames.Add(entry.Value.StorageIdentityColumn);
                                    columnsInDataSourceTable = GetTableColumns(DataTable, columnsNames);

                                    IDataColumn[] associationTableColumnsDataSourceRelated = GetTableColumns(relationshipData.Data, relationshipData.GetDataSourceRelatedColumns(relationPartIdentity, objectIdentityType, DataNode.ObjectQuery.UseStorageIdintityInTablesRelations));
                                    PartialRelationData partialRelationData;

                                    if (GetPartialRelationData(relationPartIdentity, objectIdentityType, dataNodesRelationshipData.RelationsData, out partialRelationData))
                                    {
                                        DataTable.DataSet.Relations.Add(partialRelationData.RelationName, columnsInDataSourceTable, associationTableColumnsDataSourceRelated, false);
                                        //IDataRelation relation = new DataRelation(partialRelationData.RelationName, columnsInDataSourceTable, associationTableColumnsDataSourceRelated, false);
                                        //DataTable.DataSet.Relations.Add(relation);
                                    }
                                    else
                                    {
                                        if (dataNodesRelationshipData.RelationsData.Count == 0)
                                            dataNodesRelationshipData.RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias, objectIdentityType));
                                        else
                                            dataNodesRelationshipData.RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_R" + (dataNodesRelationshipData.RelationsData.Count).ToString(), objectIdentityType));
                                        DataTable.DataSet.Relations.Add(dataNodesRelationshipData.RelationsData[dataNodesRelationshipData.RelationsData.Count - 1].RelationName, columnsInDataSourceTable, associationTableColumnsDataSourceRelated, false);
                                        //IDataRelation relation = new DataRelation(dataNodesRelationshipData.RelationsData[dataNodesRelationshipData.RelationsData.Count - 1].RelationName, columnsInDataSourceTable, associationTableColumnsDataSourceRelated, false);
                                        //DataTable.DataSet.Relations.Add(relation);
                                    }
                                }
                            }
                            if (subDataNode.DataSource.HasInObjectContextData)
                            {
                                //Create relation between association table and subDataNode DataSource table  
                                foreach (string relationPartIdentity in subDataNode.DataSource.DataSourceRelationsColumnsWithParent.Keys)
                                {
                                    foreach (var entry in subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity])
                                    {

                                        MetaDataRepository.ObjectIdentityType objectIdentityType = entry.Key;
                                        columnsNames = relationshipData.GetSubNodeDataSourceRelatedColumns(relationPartIdentity, objectIdentityType, DataNode.ObjectQuery.UseStorageIdintityInTablesRelations);
                                        IDataColumn[] associationTableColumnsSubNodeDataSourceRelated = GetTableColumns(relationshipData.Data, columnsNames);

                                        columnsNames = entry.Value.ObjectIdentityColumns;
                                        if (DataNode.ObjectQuery.UseStorageIdintityInTablesRelations)
                                            columnsNames.Add(entry.Value.StorageIdentityColumn);

                                        columnsInSubNodeDataSourceTable = GetTableColumns(subDataNode.DataSource.DataTable, columnsNames);
                                        PartialRelationData partialRelationData;

                                        if (GetPartialRelationData(relationPartIdentity, objectIdentityType, dataNodesRelationshipData.AssociationRelationsData, out partialRelationData))
                                        {
                                            DataTable.DataSet.Relations.Add(partialRelationData.RelationName, columnsInSubNodeDataSourceTable, associationTableColumnsSubNodeDataSourceRelated, false);
                                            //IDataRelation relation = new DataRelation(partialRelationData.RelationName, columnsInSubNodeDataSourceTable, associationTableColumnsSubNodeDataSourceRelated, false);
                                            //DataTable.DataSet.Relations.Add(relation);
                                        }
                                        else
                                        {
                                            if (dataNodesRelationshipData.AssociationRelationsData.Count == 0)
                                                dataNodesRelationshipData.AssociationRelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_AssociationTable", objectIdentityType));
                                            else
                                                dataNodesRelationshipData.AssociationRelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_AssociationTable_" + ((int)(dataNodesRelationshipData.AssociationRelationsData.Count + 1)).ToString(), objectIdentityType));
                                            DataTable.DataSet.Relations.Add(dataNodesRelationshipData.AssociationRelationsData[dataNodesRelationshipData.AssociationRelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, associationTableColumnsSubNodeDataSourceRelated, false);
                                            //IDataRelation relation = new DataRelation(dataNodesRelationshipData.AssociationRelationsData[dataNodesRelationshipData.AssociationRelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, associationTableColumnsSubNodeDataSourceRelated, false);
                                            //DataTable.DataSet.Relations.Add(relation);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        #region Code removed
                        //else if ((subDataNode.DataSource.HasRelationIdentityColumns &&(subDataNode.AssignedMetaObject as AssociationEnd).Association.MultiplicityType != AssociationType.ManyToMany)||
                        //    (subDataNode.DataSource.HasRelationReferenceColumns) ||
                        //    (subDataNode.AssignedMetaObject is AssociationEnd && DataNode.Classifier.ClassHierarchyLinkAssociation == (subDataNode.AssignedMetaObject as AssociationEnd).Association))
                        //{
                        //    #region Builds relation where reference columns is in member DataTable

                        //    foreach (var relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity].Keys)
                        //    {
                        //        foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, RelationColumns> entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][relationPartIdentity])
                        //        {
                        //            MetaDataRepository.ObjectIdentityType objectIdentityType = entry.Key;
                        //            if (subDataNode.DataSource.DataTable != null)
                        //            {
                        //                if (!subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity].ContainsKey(entry.Key))
                        //                    continue;
                        //                List<string> columnNames = new List<string>(entry.Value.ObjectIdentityColumns);
                        //                if (DataNode.ObjectQuery.UseStorageIdintityInTablesRelations)
                        //                    columnNames.Add(entry.Value.StorageIdentityColumn);

                        //                columnsInDataSourceTable = GetTableColumns(DataTable, columnNames);

                        //                columnNames = new List<string>(subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity][entry.Key].ObjectIdentityColumns);
                        //                if (DataNode.ObjectQuery.UseStorageIdintityInTablesRelations)
                        //                    columnNames.Add(subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity][entry.Key].StorageIdentityColumn);

                        //                columnsInSubNodeDataSourceTable = GetTableColumns(subDataNode.DataSource.DataTable, columnNames);

                        //                PartialRelationData partialRelationData;
                        //                if (GetPartialRelationData(relationPartIdentity, objectIdentityType, dataNodesRelationshipData.RelationsData, out partialRelationData))
                        //                {
                        //                    System.Data.DataRelation relation = new System.Data.DataRelation(partialRelationData.RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                        //                    DataTable.DataSet.Relations.Add(relation);
                        //                }
                        //                else
                        //                {
                        //                    if (dataNodesRelationshipData.RelationsData.Count == 0)
                        //                        dataNodesRelationshipData.RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias, objectIdentityType));
                        //                    else
                        //                        dataNodesRelationshipData.RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_R" + dataNodesRelationshipData.RelationsData.Count.ToString(), objectIdentityType));
                        //                    System.Data.DataRelation relation = new System.Data.DataRelation(dataNodesRelationshipData.RelationsData[dataNodesRelationshipData.RelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                        //                    DataTable.DataSet.Relations.Add(relation);
                        //                }
                        //            }
                        //        }
                        //    }
                        //    #endregion
                        //}
                        #endregion
                        else
                        {
                            DataNodesRelationshipData dataNodesRelationshipData = RelationshipsData[subDataNodeRelationDataIdentity];

                            #region Builds relation where reference columns is in member DataTable

                            if (DataSourceRelationsColumnsWithSubDataNodes.Count == 0)
                            {

                            }
                            foreach (var relationPartIdentity in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity].Keys)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, RelationColumns> entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][relationPartIdentity])
                                {
                                    MetaDataRepository.ObjectIdentityType objectIdentityType = entry.Key;
                                    if (subDataNode.DataSource.DataTable != null)
                                    {
                                        if (!subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity].ContainsKey(entry.Key))
                                            continue;
                                        List<string> columnNames = new List<string>(entry.Value.ObjectIdentityColumns);
                                        if (DataNode.ObjectQuery.UseStorageIdintityInTablesRelations)
                                            columnNames.Add(entry.Value.StorageIdentityColumn);

                                        columnsInDataSourceTable = GetTableColumns(DataTable, columnNames);

                                        columnNames = new List<string>(subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity][entry.Key].ObjectIdentityColumns);
                                        if (DataNode.ObjectQuery.UseStorageIdintityInTablesRelations)
                                            columnNames.Add(subDataNode.DataSource.DataSourceRelationsColumnsWithParent[relationPartIdentity][entry.Key].StorageIdentityColumn);

                                        columnsInSubNodeDataSourceTable = GetTableColumns(subDataNode.DataSource.DataTable, columnNames);

                                        PartialRelationData partialRelationData;
                                        if (GetPartialRelationData(relationPartIdentity, objectIdentityType, dataNodesRelationshipData.RelationsData, out partialRelationData))
                                        {
                                            DataTable.DataSet.Relations.Add(partialRelationData.RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                                            //IDataRelation relation = new DataRelation(partialRelationData.RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                                            //DataTable.DataSet.Relations.Add(relation);
                                        }
                                        else
                                        {
                                            if (dataNodesRelationshipData.RelationsData.Count == 0)
                                                dataNodesRelationshipData.RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias, objectIdentityType));
                                            else
                                                dataNodesRelationshipData.RelationsData.Add(new PartialRelationData(relationPartIdentity, subDataNode.Alias + "_R" + dataNodesRelationshipData.RelationsData.Count.ToString(), objectIdentityType));
                                            DataTable.DataSet.Relations.Add(dataNodesRelationshipData.RelationsData[dataNodesRelationshipData.RelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                                            //IDataRelation relation = new DataRelation(dataNodesRelationshipData.RelationsData[dataNodesRelationshipData.RelationsData.Count - 1].RelationName, columnsInSubNodeDataSourceTable, columnsInDataSourceTable, false);
                                            //DataTable.DataSet.Relations.Add(relation);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }


        }

        /// <summary>
        /// Looks in relationsData collection for PartialRelationData of relationPartIdentity  and objectIdentityType.
        /// If there is then return true and out parameter has partial relation data
        /// </summary>
        /// <param name="relationPartIdentity">
        /// Defines relation part identity
        /// </param>
        /// <param name="objectIdentityType">
        /// Defines the type of objectIdentity for relations
        /// </param>
        /// <param name="relationsData">
        /// Defines the collection with ParialRelationData
        /// </param>
        /// <param name="partialRelationData">
        /// Defines a output parameter where the method loads the partial relation data when finds them
        /// </param>
        /// <returns>
        /// If the method finds partial relation data returns true else return false
        /// </returns>
        /// <MetaDataID>{60714ffa-41ed-455d-a1e7-5669cf02d39a}</MetaDataID>
        private bool GetPartialRelationData(string relationPartIdentity, ObjectIdentityType objectIdentityType, List<PartialRelationData> relationsData, out PartialRelationData partialRelationData)
        {

            foreach (var relationData in relationsData)
            {
                if (relationData.RelationPartIdentity == relationPartIdentity && relationData.ObjectIdentityType == objectIdentityType)
                {
                    partialRelationData = relationData;
                    return true;
                }
            }
            partialRelationData = default(PartialRelationData);
            return false;
        }


        ///TODO Θα πρέπει να γραφτουν test case σενάρια για όλες τις περιπτώσεις
        /// <MetaDataID>{521e54ec-328e-43bc-b04b-5771985ca331}</MetaDataID>
        private bool DropTableDataForUnachievedConstraintCriterion(DataNode subDataNode)
        {

            return false;

            if (!(subDataNode.ObjectQuery is DistributedObjectQuery) && subDataNode.Type == DataNode.DataNodeType.Object && (subDataNode.DataSource == null || subDataNode.DataSource.DataTable == null || subDataNode.DataSource.DataTable.Rows.Count == 0))
            {
                if (DataNode.SearchCondition != subDataNode.SearchCondition ||
                    subDataNode.BranchSearchCriterions.Count == 0 ||
                    DataNode.SearchCondition == null)
                    return false;
                bool thereIsnotRowsToPassFilter = false;
                if (subDataNode.BranchSearchCriterions.Count > 0)
                    thereIsnotRowsToPassFilter = true;

                #region Checks for Is NULL criterions
                foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in subDataNode.BranchSearchCriterions)
                {
                    if (criterion.IsNULL)
                    {
                        thereIsnotRowsToPassFilter = false;
                        break;
                    }
                }
                #endregion

                #region Checks if any criterion from sub data node branch criterions has or relationship with out of sub data node branch criterions
                System.Collections.Generic.List<Criterion> globalCriterions = DataNode.SearchCondition.Criterions;
                foreach (Criterion branchCriterion in subDataNode.BranchSearchCriterions)
                    globalCriterions.Remove(branchCriterion);
                foreach (MetaDataRepository.ObjectQueryLanguage.Criterion criterion in subDataNode.BranchSearchCriterions)
                {
                    foreach (Criterion globalCriterion in globalCriterions)
                    {
                        if (!DataNode.SearchCondition.HasAndRelation(criterion, globalCriterion))
                            thereIsnotRowsToPassFilter = false;
                    }
                }
                #endregion

                //If there is (Is NULL) criterion or any of criterions has or relationship with other criterions the flag thereIsnotRowsToPassFilter is false
                foreach (DataLoader dataLoader in DataLoaders.Values)
                {
                    if (thereIsnotRowsToPassFilter && dataLoader is StorageDataLoader && (dataLoader as StorageDataLoader).HasRows && !(dataLoader as StorageDataLoader).ParticipateInOnMemoryResolvedCriterions)
                        thereIsnotRowsToPassFilter = false;
                }
                if (thereIsnotRowsToPassFilter)
                {
                    DataTable.Clear();
                    return true;
                }


            }
            return false;
        }


        /// <MetaDataID>{1af0a10d-d0e0-4e6b-85bb-40cd7d275eb5}</MetaDataID>
        internal Dictionary<string, ICollection<IDataRow>> GetRelatedRowsPartial(IDataRow row, DataNode relatedDataNode)
        {
            Dictionary<string, ICollection<IDataRow>> partialRelationRelatedRows = new Dictionary<string, ICollection<IDataRow>>();
            //int rowsCount = masterRows.Count;
            List<IDataRow> relatedRows = null;
            // RelatedRowsCollection relatedRowsColection = null;
            if (relatedDataNode.RealParentDataNode == DataNode)
            {
                if (!TablesRelationsLoaded)
                    BuildTablesRelations();

                Guid subDataNodeRelateionDataIdentity;// relatedDataNode.ParentDataNode.ValueTypePath.ToString() + relatedDataNode.AssignedMetaObject.Identity.ToString();
                if (relatedDataNode.Type == DataNode.DataNodeType.Group)
                    subDataNodeRelateionDataIdentity = relatedDataNode.Identity;
                else
                    subDataNodeRelateionDataIdentity = relatedDataNode.Identity;

                if (relatedDataNode.DataSource == null)
                    return partialRelationRelatedRows;

                if (relatedDataNode.ThroughRelationTable && !relatedDataNode.DataSource.DataLoadedInParentDataSource)
                {

                    foreach (var partialRelationData in RelationshipsData[subDataNodeRelateionDataIdentity].RelationsData)
                    {
                        relatedRows = new List<IDataRow>();
                        foreach (IDataRow subNodeRow in row.GetChildRows(partialRelationData.RelationName))
                        {
                            foreach (var associationpartialRelationData in RelationshipsData[subDataNodeRelateionDataIdentity].AssociationRelationsData)
                            {
                                if (partialRelationData.RelationPartIdentity == associationpartialRelationData.RelationPartIdentity)
                                {
                                    IDataRow theSubNodeRow = subNodeRow.GetParentRow(associationpartialRelationData.RelationName);
                                    if (theSubNodeRow != null)
                                        relatedRows.Add(theSubNodeRow);
                                }
                            }
                        }
                        partialRelationRelatedRows[partialRelationData.RelationPartIdentity] = relatedRows;

                    }
                }
                else
                {
                    if (relatedDataNode.DataSource.DataLoadedInParentDataSource)
                    {
                        if (DataNode.IsParentDataNode(relatedDataNode))
                            partialRelationRelatedRows[DataNode.AssignedMetaObjectIdenty.ToString()] = new IDataRow[1] { row };//   masterRows;
                        else
                            partialRelationRelatedRows[relatedDataNode.AssignedMetaObjectIdenty.ToString()] = new IDataRow[1] { row };

                        return partialRelationRelatedRows;
                    }
                    if (!RelationshipsData.ContainsKey(subDataNodeRelateionDataIdentity) && (relatedDataNode.DataSource.DataTable == null || relatedDataNode.DataSource.DataTable.Rows.Count == 0))
                        return partialRelationRelatedRows;

                    //OOAdvantech.Member<bool> isChildRows = new Member<bool>();
                    //                    relatedRowsColection = new RelatedRowsCollection(rowsCount);
                    int i = 0;
                    //foreach (System.Data.DataRow row in masterRows)
                    {
                        if (!relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation.HasValue)
                        {
                            if (row.Table.ChildRelations.Contains(relatedDataNode.Alias))
                                relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation = true;
                            else if (row.Table.ParentRelations.Contains(relatedDataNode.Alias))
                                relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation = false;
                            else
                                relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation = true;
                        }
                        if (relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation.Value)
                        {
                            //if (RelationshipsData[subDataNodeRelateionDataIdentity].RelationNames.Count == 1)
                            //    relatedRowsColection.RowsColections[i++] = row.GetChildRows(relatedDataNode.Alias);
                            //else
                            {
                                if (relatedRows == null)
                                    relatedRows = new List<IDataRow>();
                                foreach (var partialRelationData in RelationshipsData[subDataNodeRelateionDataIdentity].RelationsData)
                                {
                                    //relatedRows.AddRange(row.GetChildRows(partialRelationData.RelationName));
                                    partialRelationRelatedRows[partialRelationData.RelationPartIdentity] = row.GetChildRows(partialRelationData.RelationName);
                                }

                            }
                        }
                        else
                        {
                            //if (RelationshipsData[subDataNodeRelateionDataIdentity].RelationNames.Count == 1)
                            //{
                            //    partialRelationRelatedRows[RelationshipsData[subDataNodeRelateionDataIdentity].RelationNames[0].RelationPartIdentity] = row.GetParentRows(RelationshipsData[subDataNodeRelateionDataIdentity].RelationNames[0].RelationName);

                            //}
                            //else
                            {
                                if (relatedRows == null)
                                    relatedRows = new List<IDataRow>();
                                foreach (var partialRelationData in RelationshipsData[subDataNodeRelateionDataIdentity].RelationsData)
                                    partialRelationRelatedRows[partialRelationData.RelationPartIdentity] = row.GetParentRows(partialRelationData.RelationName);

                            }
                        }
                    }
                }
            }
            else if (DataNode.RealParentDataNode == relatedDataNode)
            {
                throw new System.NotSupportedException("Functionality not suported for parent data node");

                //if (DataNode.ThrougthRelationTable)
                //{
                //    relatedRows = new List<System.Data.DataRow>();
                //    foreach (System.Data.DataRow row in masterRows)
                //    {
                //        foreach (System.Data.DataRow associationRow in row.GetChildRows(DataNode.Alias + "_AssociationTable"))
                //        {
                //            System.Data.DataRow parentDataNodeRow = associationRow.GetParentRow(DataNode.Alias);
                //            relatedRows.Add(parentDataNodeRow);
                //        }
                //    }
                //}
                //else
                //{
                //   // relatedRowsColection = new RelatedRowsCollection(rowsCount);
                //    int i = 0;
                //    OOAdvantech.Member<bool> isChildRows = new Member<bool>();
                //    foreach (System.Data.DataRow row in masterRows)
                //    {
                //        if (!RetrieveRowsThroughParentChildRelation.HasValue)
                //        {
                //            if (row.Table.ChildRelations.Contains(DataNode.Alias))
                //                RetrieveRowsThroughParentChildRelation = true;
                //            else if (row.Table.ParentRelations.Contains(DataNode.Alias))
                //                RetrieveRowsThroughParentChildRelation = false;
                //            else
                //                RetrieveRowsThroughParentChildRelation = true;
                //        }
                //        if (RetrieveRowsThroughParentChildRelation.Value)
                //        {
                //            relatedRowsColection.RowsColections[i++] = row.GetChildRows(DataNode.Alias);
                //        }
                //        else
                //        {
                //            relatedRowsColection.RowsColections[i++] = row.GetParentRows(DataNode.Alias);
                //        }
                //    }
                //}
            }

            else if (DataNode.Recursive && relatedDataNode == DataNode)
            {
                throw new System.NotSupportedException("Functionality not suported for recursive relation data node");
                //    string recursiveRelationDataIdentity = DataNode.ParentDataNode.ValueTypePath.ToString() + DataNode.AssignedMetaObject.Identity.ToString();
                //    relatedRowsColection = new RelatedRowsCollection(rowsCount);
                //    int i = 0;
                //    OOAdvantech.Member<bool> isChildRows = new Member<bool>();
                //    foreach (System.Data.DataRow row in masterRows)
                //    {
                //        if (RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames.Count == 1)
                //            relatedRowsColection.RowsColections[i++] = row.GetChildRows("Recursive_" + DataNode.Alias);
                //        else
                //        {
                //            if (relatedRows == null)
                //                relatedRows = new List<System.Data.DataRow>();
                //            foreach (string relationName in RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames)
                //                relatedRows.AddRange(row.GetChildRows(relationName));
                //        }
                //    }
            }
            else
                throw new System.Exception("System can't retrieve related rows");
            return partialRelationRelatedRows;
        }


        /// <MetaDataID>{582178aa-824c-4e10-8781-09e2e1fabc70}</MetaDataID>
        ///<summary>
        ///Retrieves row related rows for the relatedDataNode
        ///</summary>
        ///<param name="masterRows">
        ///Defines the master table rows  where method must retrieve related rows
        ///</param>
        ///<param name="relatedDataNode">
        ///This parameter defines the related data node and related data node defines the relation.
        ///</param>
        ///<returns>
        ///Returns a collaction with the related data rows
        ///</returns>
        internal ICollection<IDataRow> GetRelatedRows(System.Collections.Generic.ICollection<IDataRow> masterRows, DataNode relatedDataNode)
        {
            bool indexerAssociationEnd = false;// when is true the related rows must be sort before return
            int rowsCount = masterRows.Count;
            List<IDataRow> relatedRows = null;
            RelatedRowsCollection relatedRowsColection = null;

            relatedDataNode = DerivedDataNode.GetOrgDataNode(relatedDataNode);
            if (relatedDataNode.RealParentDataNode == DataNode)
            {
                //sorting Column
                string indexerColumn = null;
                if (relatedDataNode.AssignedMetaObject is AssociationEnd && (relatedDataNode.AssignedMetaObject as AssociationEnd).Indexer)
                {
                    indexerColumn = relatedDataNode.DataSource.DataSourceRelationsIndexerColumnName;
                    indexerAssociationEnd = true;
                }
                Guid subDataNodeRelateionDataIdentity;// relatedDataNode.ParentDataNode.ValueTypePath.ToString() + relatedDataNode.AssignedMetaObject.Identity.ToString();
                if (relatedDataNode.Type == DataNode.DataNodeType.Group)
                    subDataNodeRelateionDataIdentity = relatedDataNode.Identity; //relatedDataNode.ParentDataNode.ValueTypePath.ToString() + relatedDataNode.Identity.ToString();
                else
                    subDataNodeRelateionDataIdentity = relatedDataNode.Identity;//relatedDataNode.ParentDataNode.ValueTypePath.ToString() + relatedDataNode.AssignedMetaObject.Identity.ToString(); 

                if (relatedDataNode.DataSource == null)
                    return new List<IDataRow>();

                if (relatedDataNode.ThroughRelationTable && !relatedDataNode.DataSource.DataLoadedInParentDataSource)
                {
                    DataNodesRelationshipData dataNodesRelationshipData = null;
                    if ((relatedDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) && relatedDataNode.ThroughRelationTable)
                        if (RelationshipsData.ContainsKey(relatedDataNode.Identity))
                            dataNodesRelationshipData = RelationshipsData[relatedDataNode.Identity];
                        else
                            dataNodesRelationshipData = relatedDataNode.DataSource.ParentRelationshipData;

                    relatedRows = new List<IDataRow>();
                    foreach (IDataRow row in masterRows)
                    {
                        if (dataNodesRelationshipData != null)
                            foreach (var partialRelationData in dataNodesRelationshipData.RelationsData)
                            {
                                foreach (IDataRow subNodeRow in row.GetChildRows(partialRelationData.RelationName))
                                {
                                    foreach (var associationpartialRelationData in dataNodesRelationshipData.AssociationRelationsData)
                                    {
                                        if (partialRelationData.RelationPartIdentity == associationpartialRelationData.RelationPartIdentity)
                                        {
                                            IDataRow theSubNodeRow = subNodeRow.GetParentRow(associationpartialRelationData.RelationName);
                                            if (theSubNodeRow != null)
                                                relatedRows.Add(theSubNodeRow);
                                        }
                                    }
                                }
                            }
                    }
                }
                else
                {
                    if (relatedDataNode.DataSource.DataLoadedInParentDataSource)
                        return masterRows;
                    if (!RelationshipsData.ContainsKey(subDataNodeRelateionDataIdentity) && (relatedDataNode.DataSource.DataTable == null || relatedDataNode.DataSource.DataTable.Rows.Count == 0))
                        return new List<IDataRow>();

                    DataNodesRelationshipData dataNodesRelationshipData = null;
                    if ((relatedDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd))
                        if (RelationshipsData.ContainsKey(relatedDataNode.Identity))
                            dataNodesRelationshipData = RelationshipsData[relatedDataNode.Identity];
                        else
                            dataNodesRelationshipData = relatedDataNode.DataSource.ParentRelationshipData;

                    relatedRowsColection = new RelatedRowsCollection(rowsCount);
                    int i = 0;
                    foreach (IDataRow row in masterRows)
                    {
                        if (!relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation.HasValue)
                        {

                            if (row.Table.ChildRelations.Contains(relatedDataNode.Alias))
                                relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation = true;
                            else if (row.Table.ParentRelations.Contains(relatedDataNode.Alias))
                                relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation = false;
                            else
                                relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation = true;
                        }
                        if (relatedDataNode.DataSource.RetrieveRowsThroughParentChildRelation.Value)
                        {
                            if (RelationshipsData[subDataNodeRelateionDataIdentity].RelationsData.Count == 1)
                            {
                                var rows = row.GetChildRows(relatedDataNode.Alias);
                                if (rows != null && indexerColumn != null)
                                    if (rows != null && indexerColumn != null)
                                    {
                                        foreach (var relatedRow in rows)
                                            relatedRow.SetSortIndexValue((int)relatedRow[indexerColumn]);
                                    }

                                relatedRowsColection.RowsColections[i++] = rows;// row.GetChildRows(relatedDataNode.Alias);
                            }
                            else
                            {
                                if (relatedRows == null)
                                    relatedRows = new List<IDataRow>();
                                foreach (var partialRelationData in RelationshipsData[subDataNodeRelateionDataIdentity].RelationsData)
                                    relatedRows.AddRange(row.GetChildRows(partialRelationData.RelationName));

                                if (relatedRows != null && indexerColumn != null)
                                {
                                    foreach (var relatedRow in relatedRows)
                                        relatedRow.SetSortIndexValue((int)relatedRow[indexerColumn]);
                                }

                            }
                        }
                        else
                        {
                            if (RelationshipsData[subDataNodeRelateionDataIdentity].RelationsData.Count == 1)
                            {
                                var rows = row.GetParentRows(relatedDataNode.Alias);
                                if (rows != null && indexerColumn != null)
                                {
                                    foreach (var relatedRow in rows)
                                        relatedRow.SetSortIndexValue((int)relatedRow[indexerColumn]);
                                }
                                relatedRowsColection.RowsColections[i++] = rows;// row.GetParentRows(relatedDataNode.Alias);
                            }
                            else
                            {
                                if (relatedRows == null)
                                    relatedRows = new List<IDataRow>();



                                foreach (var partialRelationData in RelationshipsData[subDataNodeRelateionDataIdentity].RelationsData)
                                    relatedRows.AddRange(row.GetParentRows(partialRelationData.RelationName));

                                if (relatedRows != null && indexerColumn != null)
                                {
                                    foreach (var relatedRow in relatedRows)
                                        relatedRow.SetSortIndexValue((int)relatedRow[indexerColumn]);
                                }

                            }
                        }
                    }
                }
            }
            else if (DataNode.RealParentDataNode == relatedDataNode)
            {
                if (DataNode.ThroughRelationTable)
                {
                    relatedRows = new List<IDataRow>();
                    foreach (IDataRow row in masterRows)
                    {
                        foreach (IDataRow associationRow in row.GetChildRows(DataNode.Alias + "_AssociationTable"))
                        {
                            IDataRow parentDataNodeRow = associationRow.GetParentRow(DataNode.Alias);
                            relatedRows.Add(parentDataNodeRow);
                        }
                    }
                }
                else
                {
                    relatedRowsColection = new RelatedRowsCollection(rowsCount);
                    int i = 0;
                    OOAdvantech.Member<bool> isChildRows = new Member<bool>();
                    foreach (IDataRow row in masterRows)
                    {
                        if (!RetrieveRowsThroughParentChildRelation.HasValue)
                        {
                            if (row.Table.ChildRelations.Contains(DataNode.Alias))
                                RetrieveRowsThroughParentChildRelation = false; //ParentDataNode SubDataNode relation
                            else if (row.Table.ParentRelations.Contains(DataNode.Alias))
                                RetrieveRowsThroughParentChildRelation = true; //ParentDataNode SubDataNode relation
                            else
                                RetrieveRowsThroughParentChildRelation = false;
                        }
                        if (RetrieveRowsThroughParentChildRelation.Value)
                        {
                            //ParentDataNode SubDataNode relation
                            relatedRowsColection.RowsColections[i++] = row.GetParentRows(DataNode.Alias);
                        }
                        else
                        {
                            //ParentDataNode SubDataNode relation
                            relatedRowsColection.RowsColections[i++] = row.GetChildRows(DataNode.Alias);
                        }
                    }
                }
            }
            else if (DataNode.Type == DataNode.DataNodeType.Group && (DataNode as GroupDataNode).GroupKeyDataNodes.Contains(relatedDataNode))
            {
                OOAdvantech.Member<bool> isChildRows = new Member<bool>();
                int i = 0;
                relatedRowsColection = new RelatedRowsCollection(rowsCount);
                foreach (IDataRow row in masterRows)
                {
                    IDataRow[] keyRows = null;
                    foreach (ObjectKeyRelationColumns keyRelatedColumns in DataNode.DataSource.GroupByKeyRelationColumns[relatedDataNode.Identity].Values)
                    {
                        if (isChildRows.UnInitialized)
                        {
                            if (row.Table.ChildRelations.Contains(keyRelatedColumns.RelationName))
                                isChildRows.Value = true;
                            else if (row.Table.ParentRelations.Contains(keyRelatedColumns.RelationName))
                                isChildRows.Value = false;
                            else
                                isChildRows.Value = true;
                        }
                        if (isChildRows.Value)
                            keyRows = row.GetChildRows(keyRelatedColumns.RelationName);
                        else
                            keyRows = row.GetParentRows(keyRelatedColumns.RelationName);
                        if (keyRows.Length > 1)
                            keyRows = new IDataRow[] { keyRows[0] };
                        if (keyRows.Length > 0)
                            break;
                    }
                    relatedRowsColection.RowsColections[i++] = keyRows;
                }

            }
            else if (DataNode.Recursive && relatedDataNode == DataNode)
            {
                Guid recursiveRelationDataIdentity = DataNode.Identity; //DataNode.ParentDataNode.ValueTypePath.ToString() + DataNode.AssignedMetaObject.Identity.ToString();
                relatedRowsColection = new RelatedRowsCollection(rowsCount);
                int i = 0;
                OOAdvantech.Member<bool> isChildRows = new Member<bool>();
                foreach (IDataRow row in masterRows)
                {
                    if (RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames.Count == 1)
                        relatedRowsColection.RowsColections[i++] = row.GetChildRows("Recursive_" + DataNode.Alias);
                    else
                    {
                        if (relatedRows == null)
                            relatedRows = new List<IDataRow>();
                        foreach (string relationName in RelationshipsData[recursiveRelationDataIdentity].RecursiveRelationNames)
                            relatedRows.AddRange(row.GetChildRows(relationName));
                    }
                }
            }
            else if (DataNode is GroupDataNode && (DataNode as GroupDataNode).GroupedDataNode == relatedDataNode)
            {
                var name = MetaDataRepository.ObjectQueryLanguage.DataLoader.GetGroupingDataColumnName(DataNode);
                relatedRows = new List<IDataRow>();
                foreach (IDataRow row in masterRows)
                {
                    GroupingEntry groupingEntry = row[name] as GroupingEntry;
                    foreach (var groupedCompositeRow in groupingEntry.GroupedCompositeRows)
                        relatedRows.Add(groupedCompositeRow[(DataNode as GroupDataNode).DataGrouping.DataNodeRowIndices[relatedDataNode]].DataRow);
                }

                return relatedRows;
            }
            else
                throw new System.Exception("System can't retrieve related rows");
            if (relatedRows != null)
            {
                if (indexerAssociationEnd)
                    return relatedRows.OrderBy(x => x.GetSortIndexValue()).ToList();
                else
                    return relatedRows;

            }
            else
            {
                if (indexerAssociationEnd)
                    return relatedRowsColection.OrderBy(x => x.GetSortIndexValue()).ToList();
                else
                    return relatedRowsColection;

                
            }
        }



        ///<summary>
        ///Retrieves row related rows for the relatedDataNode
        ///</summary>
        ///<param name="row">
        ///Defines the row which method must retrieve related rows
        ///</param>
        ///<param name="relatedDataNode">
        ///This parameter defines the related data node and related data node defines the relation.
        ///</param>
        ///<returns>
        ///Returns a collaction with the related data rows
        ///</returns>
        /// <MetaDataID>{5b40399d-b50c-4ec1-bb99-a3d450f1c3f0}</MetaDataID>
        internal ICollection<IDataRow> GetRelatedRows(IDataRow row, DataNode relatedDataNode)
        {
            if (row == null)
                return new List<IDataRow>();

            return GetRelatedRows(new IDataRow[] { row }, relatedDataNode);

        }

        //internal Dictionary<string, ICollection<System.Data.DataRow>> GetRelatedRowsPartial(System.Data.DataRow row, DataNode relatedDataNode)
        //{
        //    if (row == null)
        //        return new Dictionary<string, ICollection<System.Data.DataRow>>();
        //    return GetRelatedRowsPartial(new System.Data.DataRow[] { row }, relatedDataNode);
        //}

        /// <summary>
        /// Defines objects link data
        /// </summary>
        internal struct ObjectsLinkRows
        {
            /// <summary>
            /// Defines the row which contain the linked object
            /// </summary>
            public IDataRow RelatedObjectRow;
            /// <summary>
            /// Defines the row which contain objects link data like objects identities and storage identities
            /// </summary>
            public IDataRow RelationDataRow;
        }
        /// <summary>
        /// Retrieve and return a collation with ObjectsLinkRows for of object of row. 
        /// The relation defined from related data node.
        /// </summary>
        /// <param name="row">
        /// This parameter defines the row with object.
        /// </param>
        /// <param name="relatedDataNode">
        /// Related data node parameter defines the relation. 
        /// </param>
        /// <returns>
        /// Returns a dictionary with partial relation keys and collection of objects link rows
        /// The objects link rows has a row with relation data object identities for linked objects and storage identities for linked objects and
        /// one row with related object.
        /// In case where related object is out the storage the second row "relate object row" is null 
        /// </returns>
        /// <MetaDataID>{ca78962a-1efd-4e46-8e8f-e0fbac67de3a}</MetaDataID>
        internal Dictionary<string, ICollection<ObjectsLinkRows>> GetRelationRowsPartial(IDataRow row, DataNode relatedDataNode)
        {

            Dictionary<string, ICollection<ObjectsLinkRows>> objectLinkRows = new Dictionary<string, ICollection<ObjectsLinkRows>>();

            if (relatedDataNode.RealParentDataNode == DataNode)
            {

                if (!TablesRelationsLoaded)
                    BuildTablesRelations();

                Guid subDataNodeRelationDataIdentity = relatedDataNode.Identity;// = null;// relatedDataNode.ParentDataNode.ValueTypePath.ToString() + relatedDataNode.AssignedMetaObject.Identity.ToString();
                if (relatedDataNode.ThroughRelationTable)
                {
                    foreach (var partialRelationData in RelationshipsData[subDataNodeRelationDataIdentity].RelationsData)
                    {
                        List<ObjectsLinkRows> partialRelationRelationRows = new List<ObjectsLinkRows>();
                        objectLinkRows[partialRelationData.RelationPartIdentity] = partialRelationRelationRows;
                        foreach (IDataRow subNodeRow in row.GetChildRows(partialRelationData.RelationName))
                        {
                            ObjectsLinkRows relationRow = new ObjectsLinkRows();
                            int roleAStorageIdentity = 0;
                            int roleBStorageIdentity = 0;

                            if (subNodeRow["RoleAStorageIdentity"] is int)
                                roleAStorageIdentity = (int)subNodeRow["RoleAStorageIdentity"];

                            if (subNodeRow["RoleBStorageIdentity"] is int)
                                roleBStorageIdentity = (int)subNodeRow["RoleBStorageIdentity"];
                            if (roleAStorageIdentity != roleBStorageIdentity)
                            {
                                relationRow.RelationDataRow = subNodeRow;
                                partialRelationRelationRows.Add(relationRow);

                                ///RelatedObjectRow is null

                            }
                            else
                            {
                                foreach (var associationPartialRelationData in RelationshipsData[subDataNodeRelationDataIdentity].AssociationRelationsData)
                                {
                                    if (associationPartialRelationData.RelationPartIdentity == partialRelationData.RelationPartIdentity)
                                    {
                                        IDataRow relatedObjectRow = subNodeRow.GetParentRow(associationPartialRelationData.RelationName);
                                        relationRow.RelatedObjectRow = relatedObjectRow;
                                        relationRow.RelationDataRow = subNodeRow;
                                        partialRelationRelationRows.Add(relationRow);
                                    }
                                }
                            }

                        }
                    }
                }
                else
                    throw new System.NotSupportedException("");
            }
            else
                throw new System.NotSupportedException("");
            return objectLinkRows;
        }


        /// <MetaDataID>{ac933435-c378-401a-9fb5-d2adace27915}</MetaDataID>
        private static IDataColumn[] GetTableColumns(IDataTable dataTable, List<string> columnsNames)
        {
            IDataColumn[] dataColumns = new IDataColumn[columnsNames.Count];
            int i = 0;
            foreach (string columnName in columnsNames)
                dataColumns[i++] = dataTable.Columns[columnName];
            return dataColumns;
        }






        /// <MetaDataID>{bed040ac-55cc-496b-b926-42628ac9bbea}</MetaDataID>
        internal protected virtual List<AssociationEnd> PrefetchingAssociationEnds
        {
            get
            {
                return new List<AssociationEnd>();
            }
        }


        #region Gets Main query valid column names for DataNode

        ///<summary>
        ///Returns valid column name for DataNode
        ///Column is unique for parent data node namespace 
        ///</summary>
        /// <MetaDataID>{a5baef66-aa51-4ffc-a210-491d8e4c7649}</MetaDataID>
        public static string GetColumnName(DataNode dataNode)
        {
            if (dataNode is AggregateExpressionDataNode)
                return dataNode.Alias;
            string columnName = null;
            DataNode dataLoaderDataNode = dataNode;
            while (dataLoaderDataNode.ValueTypePath.Count > 0 || dataLoaderDataNode.Type == DataNode.DataNodeType.OjectAttribute)
            {
                if (columnName != null)
                    columnName = "_" + columnName;
                if (dataLoaderDataNode.CastingParentType != null)
                    columnName = dataLoaderDataNode.CastingParentType.Name + "_" + dataLoaderDataNode.Name + columnName;
                else
                    columnName = dataLoaderDataNode.Name + columnName;

                dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;
            }
            if (!string.IsNullOrEmpty(columnName) && dataLoaderDataNode.DataSource.DataLoadedInParentDataSource)
                columnName = dataLoaderDataNode.Alias + "_" + columnName;
            return columnName;
        }
        ///<summary>
        ///Returns valid column name for DataNode
        ///Column name is unique for DataTree
        ///</summary>
        /// <MetaDataID>{aa8d5b86-3071-4893-8c1b-60925f14bb71}</MetaDataID>
        public static string GetDataTreeUniqueColumnName(DataNode dataNode)
        {
            string columnName = null;
            DataNode dataLoaderDataNode = dataNode;
            while (dataLoaderDataNode.ValueTypePath.Count > 0 || dataLoaderDataNode.Type == DataNode.DataNodeType.OjectAttribute || dataLoaderDataNode is AggregateExpressionDataNode)
            {
                if (columnName != null)
                    columnName = "_" + columnName;
                if (dataLoaderDataNode.CastingParentType != null)
                    columnName = dataLoaderDataNode.CastingParentType.Name + "_" + dataLoaderDataNode.Name + columnName;
                else
                    columnName = dataLoaderDataNode.Name + columnName;
                dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;
            }
            columnName = dataLoaderDataNode.Alias + "_" + columnName;
            return columnName;
        }


        /// <MetaDataID>{963b9c21-0459-426e-a849-d783d4f0bc5d}</MetaDataID>
        public static string GetDataTreeUniqueColumnName(DataNode dataNode, MetaDataRepository.IIdentityPart part)
        {
            string columnName = null;
            DataNode dataLoaderDataNode = dataNode;
            while (dataLoaderDataNode.ValueTypePath.Count > 0 || dataLoaderDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;
            columnName = dataLoaderDataNode.Alias + "_" + part.Name;
            return columnName;
        }

        /// <MetaDataID>{99740bfe-db8c-4f4e-9e25-f90751a238f6}</MetaDataID>
        public static string GetColumnName(DataNode dataNode, MetaDataRepository.IIdentityPart part)
        {
            string columnName = part.Name;
            DataNode dataLoaderDataNode = dataNode;
            while (dataLoaderDataNode.ValueTypePath.Count > 0 || dataLoaderDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                dataLoaderDataNode = dataLoaderDataNode.ParentDataNode;
            if (dataLoaderDataNode.DataSource.DataLoadedInParentDataSource)
                columnName = dataLoaderDataNode.Alias + "_" + columnName;
            return columnName;
        }

        #endregion


        /// <MetaDataID>{1130ba55-8292-4ca5-a188-d0832e155a2f}</MetaDataID>
        public int GetColumnIndex(DataNode dataNode)
        {
            if (DataNode.Type == DataNode.DataNodeType.Group)
            {
                if (DataTable == null)
                    return -1;

                if ((DataNode as GroupDataNode).GroupKeyDataNodes.Contains(dataNode))
                {
                    return DataTable.Columns.IndexOf(GetDataTreeUniqueColumnName(dataNode));
                    string columnName = dataNode.Name;
                    DataNode dataLoadinDataNode = dataNode;
                    while (dataLoadinDataNode.Type == DataNode.DataNodeType.OjectAttribute || dataLoadinDataNode.ValueTypePath.Count > 0)
                    {
                        dataLoadinDataNode = dataLoadinDataNode.ParentDataNode;
                        if (dataLoadinDataNode.Type == DataNode.DataNodeType.OjectAttribute || dataLoadinDataNode.ValueTypePath.Count > 0)
                            columnName = dataLoadinDataNode.Name + "_" + columnName;
                        else
                            columnName = dataLoadinDataNode.Alias + "_" + columnName;
                    }
                    if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        return DataTable.Columns.IndexOf(dataLoadinDataNode.Alias + "_" + dataNode.ParentDataNode.Name + "_" + dataNode.AssignedMetaObject.Name);
                    else
                        return DataTable.Columns.IndexOf(columnName);
                }
                else if (dataNode is AggregateExpressionDataNode)
                    return DataTable.Columns.IndexOf(dataNode.Alias);

                return -1;
            }
            else
            {
                if (DataTable == null)
                    return -1;
                return DataTable.Columns.IndexOf(GetColumnName(dataNode));
            }
        }



        /// <MetaDataID>{a81b3747-9349-4d75-8b4d-b65d23515714}</MetaDataID>
        /// <summary>
        /// The object identity types of data loader produce columns for its object identity type part.
        /// Sometimes the part name of one identity type is same with part name of other identity type.
        /// There is possibility two storagecell with different object identity type but same part from two object identity has the same name.      
        /// </summary>
        internal void MakeObjectIdentityPartsUnary()
        {
            #region distribute all object identity types
            //System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes = new List<ObjectIdentityType>();
            //System.Collections.Generic.List<string> unaryPartNames = new List<string>();
            //foreach (DataLoader dataLoader in DataLoaders.Values)
            //{
            //    System.Collections.Generic.List<ObjectIdentityType> dataLoaderObjectIdentityTypes = new List<ObjectIdentityType>();
            //    foreach (ObjectIdentityType objectIdentityType in dataLoader.ObjectIdentityTypes)
            //    {
            //        if (!objectIdentityTypes.Contains(objectIdentityType))
            //        {
            //            System.Collections.Generic.List<IIdentityPart> parts = new List<IIdentityPart>();
            //            foreach (IIdentityPart part in objectIdentityType.Parts)
            //            {
            //                string partName = part.Name;
            //                int count = 1;
            //                while (unaryPartNames.Contains(partName))
            //                {
            //                    partName = part.Name + "_" + count.ToString();
            //                    count++;
            //                }
            //                unaryPartNames.Add(partName);
            //                parts.Add(new IdentityPart(partName, part.PartTypeName, part.Type));
            //            }

            //            objectIdentityTypes.Add(new ObjectIdentityType(parts));
            //        }
            //       // dataLoaderObjectIdentityTypes.Add(objectIdentityTypes[objectIdentityTypes.IndexOf(objectIdentityType)]);
            //    }
            //  //  dataLoader.UpdateObjectIdentityTypes(dataLoaderObjectIdentityTypes);
            //}
            //foreach (DataLoader dataLoader in DataLoaders.Values)
            //{
            //    dataLoader.UpdateObjectIdentityTypes(objectIdentityTypes);
            //}
            #endregion


            System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes = new List<ObjectIdentityType>();
            System.Collections.Generic.List<string> unaryPartNames = new List<string>();
            foreach (DataLoader dataLoader in DataLoaders.Values)
            {
                System.Collections.Generic.List<ObjectIdentityType> dataLoaderObjectIdentityTypes = new List<ObjectIdentityType>();
                foreach (ObjectIdentityType objectIdentityType in dataLoader.ObjectIdentityTypes)
                {
                    if (!objectIdentityTypes.Contains(objectIdentityType))
                    {
                        System.Collections.Generic.List<IIdentityPart> parts = new List<IIdentityPart>();
                        foreach (IIdentityPart part in objectIdentityType.Parts)
                        {
                            string partName = part.Name;
                            int count = 1;
                            while (unaryPartNames.Contains(partName))
                            {
                                partName = part.Name + "_" + count.ToString();
                                count++;
                            }
                            unaryPartNames.Add(partName);
                            parts.Add(new IdentityPart(partName, part.PartTypeName, part.Type));
                        }

                        objectIdentityTypes.Add(new ObjectIdentityType(parts));
                    }
                    dataLoaderObjectIdentityTypes.Add(objectIdentityTypes[objectIdentityTypes.IndexOf(objectIdentityType)]);
                }
                dataLoader.UpdateObjectIdentityTypes(dataLoaderObjectIdentityTypes);
            }
            foreach (DataLoader dataLoader in DataLoaders.Values)
                dataLoader.UpdateObjectIdentityTypes(objectIdentityTypes);
            _ObjectIdentityTypes = objectIdentityTypes;



        }
        /// <MetaDataID>{0ab54ada-8664-487d-9f8d-6ec9c35dec08}</MetaDataID>
        /// <summary>
        /// Distribute the state change from object distribution to data loading.
        /// </summary>
        public void PrepareForDataLoading()
        {
            //MakeObjectIdentityPartsUnary();
            foreach (DataLoader dataLoader in DataLoaders.Values)
                dataLoader.PrepareForDataLoading();
        }


        /// <summary>
        /// Check data loading model for all data loaders. If there are two or more data loaders with different data loading model, 
        /// data source equates data loaders data loading model.
        /// Data loading model expressed from (DataSource,DataLoader) DataLoadedInParentDataSource property and (DataNode) ThrougthRelationTable property
        /// </summary>
        /// <MetaDataID>{31b998f0-f562-4c20-8550-afa0a0213a84}</MetaDataID>
        internal void UpdateDataLoadingModel()
        {
            try
            {
                if (DataNode.RealParentDataNode == null)
                    return;

                if (LoadDataInParentDataSourcePermited && DataNode.AssignedMetaObject is AssociationEnd)
                {
                    bool dataLoadingPolicyMismatch = false;
                    foreach (DataLoader dataLoader in DataLoaders.Values)
                    {
                        if (!dataLoader.DataLoadedInParentDataSource)
                        {
                            dataLoadingPolicyMismatch = true;
                            break;
                        }
                    }
                    if (dataLoadingPolicyMismatch)
                    {
                        foreach (DataLoader dataLoader in DataLoaders.Values)
                            dataLoader.DataLoadedInParentDataSource = false;
                        DataLoadedInParentDataSource = false;
                    }
                    else if (DataLoaders.Count > 0)
                        DataLoadedInParentDataSource = true;
                }
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && DataNode.Type == DataNode.DataNodeType.Object)
                    DataLoadedInParentDataSource = true;
                if (DataLoaders.Values.Count > 0 &&
                    !DataNode.ThroughRelationTable &&
                    DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                    !HasRelationReferenceColumns &&
                    !DataNode.ParentDataNode.DataSource.HasRelationReferenceColumnsFor(DataNode))
                {
                    DataNode.ThroughRelationTable = true;
                    foreach (DataLoader dataLoader in DataLoaders.Values)
                        dataLoader.ChangeDataNodeThrougthRelationTable(true);
                }
            }
            finally
            {
                MakeObjectIdentityPartsUnary();
            }
        }

        #region RowRemove code
        ///// <MetaDataID>{2f06c579-38ec-400c-b6f9-56385273c803}</MetaDataID>
        //public static string GetRowRemoveColumnName(DataNode DataNode, SearchCondition searchCondition)
        //{
        //    if (searchCondition == DataNode.SearchCondition)
        //        return DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove";
        //    else
        //        return DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove_" + DataNode.SearchConditions.IndexOf(searchCondition);
        //}
        #endregion

        /// <MetaDataID>{ae684eb7-c3cc-41bc-ab0b-d7f9f0619ddc}</MetaDataID>
        internal void CachingRelationshipData()
        {
            _DataSourceRelationsColumnsWithParent = DataSourceRelationsColumnsWithParent;
            _DataSourceRelationsColumnsWithSubDataNodes = DataSourceRelationsColumnsWithSubDataNodes;
        }


        /// <MetaDataID>{20dda285-aa5f-4298-bc82-7c146bccd27b}</MetaDataID>
        internal MultiPartKey GetGroupingKey(IDataRow dataRow)
        {
            ///TODO Πρεπει να γίνει cashing τα metadata για να πηγαίνει πιο γρήγορα 
            MultiPartKey groupingKey = null;
            if (DataNode.RealParentDataNode != null)
                groupingKey = new MultiPartKey((DataNode as GroupDataNode).GroupKeyDataNodes.Count + 1);
            else
                groupingKey = new MultiPartKey((DataNode as GroupDataNode).GroupKeyDataNodes.Count);

            int i = 0;
            foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
            {
                if (dataNode.Type == DataNode.DataNodeType.Object)
                {
                    GlobalObjectID? globalObjectID = StorageDataSource.GetGlobalObjectID(dataRow, GroupByKeyRelationColumns[dataNode.Identity].Keys);
                    groupingKey.KeyPartsValues[i++] = globalObjectID.Value;
                    //foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.StorageIdentity].ObjectIdentityTypes)
                    //{
                    //    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                    //    {
                    //        if (selectClause != null)
                    //            selectClause += ",";
                    //        selectClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(dataNode.Alias + "_" + part.Name);
                    //    }
                    //}
                }
                else if (dataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    groupingKey.KeyPartsValues[i++] = dataRow[DataNode.DataSource.GetColumnIndex(dataNode)];
                    //if (selectClause != null)
                    //    selectClause += ",";
                    //if (dataNode.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute && dataNode.ParentDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
                    //    selectClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(dataNode.ParentDataNode.ParentDataNode.Alias + "_" + dataNode.ParentDataNode.Name + "_" + dataNode.AssignedMetaObject.Name);
                    //else
                    //    selectClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataSource.GetDataTreeUniqueColumnName(dataNode));// GetSQLScriptForName(dataNode.ParentDataNode.Alias + "_" + dataNode.AssignedMetaObject.Name);
                }
            }
            if (!(DataNode as GroupDataNode).GroupKeyDataNodes.Contains(DataNode.RealParentDataNode) && DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
            {
                List<ObjectIdentityType> objectIdentityTypes = new List<ObjectIdentityType>();
                foreach (var entry in DataSourceRelationsColumnsWithParent.Values)
                {
                    foreach (ObjectIdentityType objectIdentityType in entry.Keys)
                        objectIdentityTypes.Add(objectIdentityType);
                }
                GlobalObjectID? globalObjectID = StorageDataSource.GetGlobalObjectID(dataRow, objectIdentityTypes);
                groupingKey.KeyPartsValues[i++] = globalObjectID.Value;
                //foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in GetDataLoader(DataNode.RealParentDataNode).ObjectIdentityTypes)
                //{
                //    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                //    {
                //        //if (selectClause != null)
                //        //    selectClause += ",";
                //        //selectClause += GetSQLScriptForName(DataNode.Alias) + "." + GetSQLScriptForName(DataNode.RealParentDataNode.Alias + "_" + part.Name); ;
                //    }
                //}
            }
            return groupingKey;
        }
        protected DataSource(Guid identity)
        {
            _Identity = identity;
        }
        internal protected abstract DataSource Clone(Dictionary<object, object> clonedObjects);

        internal void Copy(DataSource newDataSource, Dictionary<object, object> clonedObjects)
        {

            newDataSource._DataLoadedInParentDataSource = _DataLoadedInParentDataSource;
            newDataSource._ObjectIdentityColumnIndex = _ObjectIdentityColumnIndex;// (Nullable<int>)info.GetValue("ObjectIdentityColumnIndex", typeof(Nullable<int>));
            newDataSource._ObjectIndex = _ObjectIndex;// (Nullable<int>)info.GetValue("ObjectIndex", typeof(Nullable<int>));
            newDataSource.DataNode = DataNode.Clone(clonedObjects);// info.GetValue("DataNode", typeof(DataNode)) as DataNode;
            newDataSource._Identity = Identity;// (System.Guid)info.GetValue("Identity", typeof(System.Guid));
            newDataSource.RetrieveRowsThroughParentChildRelation = RetrieveRowsThroughParentChildRelation;// (Nullable<bool>)info.GetValue("RetrieveRowsThroughParentChildRelation", typeof(Nullable<bool>));

        }
    }


    /// <summary>
    /// Class RelatedRowsCollection define a collection which keeps the related rows with a method for perfomance advance 
    /// </summary>
    /// <MetaDataID>{28c36fc2-3bfa-4986-a98c-f3b660edc0f9}</MetaDataID>
    public class RelatedRowsCollection : IEnumerable<IDataRow>, ICollection<IDataRow>, ICollection
    {
        /// <MetaDataID>{1cce32c3-07fc-4d18-ba45-56b6b912a3fc}</MetaDataID>
        public RelatedRowsCollection(int countOfRowsColections)
        {

            RowsColections = new IDataRow[countOfRowsColections][];
        }
        /// <MetaDataID>{e301023e-ca15-4224-8200-6b3fa92a0ffd}</MetaDataID>
        internal IDataRow[][] RowsColections;

        class RowsEnumerator : System.Collections.Generic.IEnumerator<IDataRow>
        {
            IDataRow[][] RowsColections;
            public RowsEnumerator(IDataRow[][] rowsColections)
            {
                RowsColections = rowsColections;
            }
            int RowsIndex = -1;
            int RowIndex = -1;

            public IDataRow Current
            {
                get
                {
                    return RowsColections[RowsIndex][RowIndex];
                }
            }

            public void Dispose()
            {
            }
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;

                }
            }

            public bool MoveNext()
            {
                if (RowsColections.Length == 0)
                    return false;
                if (RowsIndex == -1)
                    RowsIndex++;
                if (RowIndex == -1)
                {
                    while (RowsColections[RowsIndex].Length == 0)
                    {
                        RowsIndex++;
                        if (RowsIndex >= RowsColections.Length)
                            return false;
                    }
                    RowIndex++;
                    return true;
                }
                else
                {

                    RowIndex++;
                    if (RowIndex >= RowsColections[RowsIndex].Length)
                    {
                        RowIndex = -1;
                        RowsIndex++;
                        if (RowsIndex >= RowsColections.Length)
                            return false;
                        while (RowsColections[RowsIndex].Length == 0)
                        {
                            RowsIndex++;
                            if (RowsIndex >= RowsColections.Length)
                                return false;
                        }
                        RowIndex++;
                        return true;
                    }
                    else
                        return true;
                }


            }

            public void Reset()
            {
                RowIndex = -1;
                RowsIndex = -1;
            }


        }


        #region IEnumerable<DataRow> Members

        /// <MetaDataID>{e0f6b313-794b-4e93-b217-9477e54814ae}</MetaDataID>
        public IEnumerator<IDataRow> GetEnumerator()
        {
            return new RowsEnumerator(RowsColections);
        }

        #endregion

        #region IEnumerable Members

        /// <MetaDataID>{f076ac23-dea0-458c-ba12-0030b488fc21}</MetaDataID>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new RowsEnumerator(RowsColections);
        }

        #endregion

        #region ICollection<DataRow> Members

        /// <MetaDataID>{44089163-d46c-41b3-bc1e-623ff1ef62e0}</MetaDataID>
        public void Add(IDataRow item)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{79a94af1-67ed-4a01-8902-65717f280721}</MetaDataID>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{00d6e893-9564-4639-a55f-e9bb46037527}</MetaDataID>
        public bool Contains(IDataRow item)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{de50f93a-5402-4011-b8ed-946010df5cbb}</MetaDataID>
        public void CopyTo(IDataRow[] array, int arrayIndex)
        {
            int i = 0;
            foreach (IDataRow row in this)
                array[i++] = row;
        }

        /// <MetaDataID>{034a4145-33c1-4aad-89b6-77690a7e37dc}</MetaDataID>
        int _Count = -1;
        /// <MetaDataID>{e4a00b07-679a-451c-9353-4a54cbb283d2}</MetaDataID>
        public int Count
        {
            get
            {
                if (_Count == -1)
                {
                    _Count = 0;
                    foreach (IDataRow[] dataRows in this.RowsColections)
                        _Count += dataRows.Length;
                }
                return _Count;
            }
        }

        /// <MetaDataID>{05ffd464-5f61-4a5b-a1e4-66a2cc5f621c}</MetaDataID>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{026734d2-4c7e-4863-b739-ddec5b61d45f}</MetaDataID>
        public bool Remove(IDataRow item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection Members

        /// <MetaDataID>{b0b0d229-7c52-4dfd-9959-c8b22f76bfb7}</MetaDataID>
        public void CopyTo(Array array, int index)
        {
            int i = 0;
            foreach (IDataRow row in this)
                array.SetValue(row, i++);

        }

        /// <MetaDataID>{8c059bcb-a6cb-4c1a-be27-922461e9731f}</MetaDataID>
        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        /// <MetaDataID>{5a65a470-13ca-4132-934c-0e95aaaac426}</MetaDataID>
        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}



