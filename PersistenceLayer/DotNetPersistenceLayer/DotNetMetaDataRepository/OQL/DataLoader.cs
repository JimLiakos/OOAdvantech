using System;
using System.Linq;
using SubDataNodeIdentity = System.Guid;
using RelationPartIdentity = System.String;
using System.Collections.Generic;
using OOAdvantech.Transactions;
using OOAdvantech.Remoting;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using System;
#endif

namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{


    /// <MetaDataID>{43275459-51FE-4F03-A5E0-4165FC9CF9F4}</MetaDataID>
    public abstract class DataLoader : MarshalByRefObject, Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{c9bba5b8-a636-4e8d-b4c8-ae728aaf40c4}</MetaDataID>
        /// <summary>
        /// This method returns  true if exist column for data node in native data management system (DMS). 
        /// Otherwise return false;
        /// </summary>
        public abstract bool LocalDataColumnExistFor(DataNode dataNode);

        public class DataLoaderSorting : System.Collections.Generic.IComparer<DataLoader>
        {

            #region IComparer<DataLoader> Members

            public int Compare(DataLoader x, DataLoader y)
            {
                //Compare DataLoaders in ralation to the postion of its DataNode in DataTree hierarchy
                int ret = GetParentDataNodesCount(x.DataNode).CompareTo(GetParentDataNodesCount(y.DataNode));
                if (ret == 0)
                {
                    //If DataNodes have the same position checks DataNodes for implicit creation
                    if (x.DataNode.MembersFetchingObjectActivation != y.DataNode.MembersFetchingObjectActivation)
                    {
                        if (y.DataNode.MembersFetchingObjectActivation)
                            return 1;
                        else
                            return -1;
                    }
                }
                return ret;
            }
            /// <summary>
            /// Return the DataNode Position in DataTree hierarchy
            /// </summary>
            int GetParentDataNodesCount(DataNode dataNode)
            {
                if (dataNode.ParentDataNode != null)
                    return 1 + GetParentDataNodesCount(dataNode.ParentDataNode);
                else
                    return 0;
            }

            #endregion
        }
        /// <MetaDataID>{c6e7fe87-7843-4571-9f9a-47043f1fa3e1}</MetaDataID>
        ///<summary>
        ///Called from framework to inform the data loader for temporary data release
        ///</summary>
        virtual public void OnAllQueryDataLoaded()
        {

        }
        /// <MetaDataID>{a86529d9-c84e-4506-a478-c444c40c76dc}</MetaDataID>
        ///<summary>
        ///If Grouped data produced from the DMS data managment system the value of property is true
        ///else the value is false
        ///</summary>
        abstract public bool GroupedDataLoaded
        {
            get;
            protected internal set;
        }
        /// <summary>
        /// Defines the rosolved aggregation Expression DataNodes
        /// </summary>
        /// <MetaDataID>{78ee77a7-1203-490e-85f3-b94c4f663953}</MetaDataID>
        abstract public List<AggregateExpressionDataNode> ResolvedAggregateExpressions
        {
            get;
        }

        ///<summary>
        ///Defines property where when is true, data loader load data that used from query engine mechanism to resolve a global resolved criterion
        ///</summary>
        /// <MetaDataID>{03a84e53-f837-4774-b995-a2c295255f4b}</MetaDataID>
        abstract public bool ParticipateInGlobalResolvedCriterion
        {
            get;
        }
        /// <summary>
        /// Defines property where when is true, data loader load data that used from query engine mechanism to resolve a global resolved aggregation expresion
        /// </summary>
        /// <MetaDataID>{05ed99fb-c196-42bd-b2ed-264b2359f4df}</MetaDataID>
        abstract public bool ParticipateInGlobalResolvedAggregateExpression
        {
            get;
        }
        /// <summary>
        /// Defines property where when is true, data loader load data that used from query engine mechanism to resolve a global resolved group by.
        /// </summary>
        /// <MetaDataID>{f9c3f7cc-5574-498c-b938-1a687e30a581}</MetaDataID>
        abstract public bool ParticipateInGlobalResolvedGroup
        {
            get;
        }


        /// <MetaDataID>{e4cea98b-fe71-4e0e-b948-a07e660f671d}</MetaDataID>
        /// <summary>
        ///If aggregateExpressionDataNode resolved locally returns true
        ///else  false
        /// </summary>
        abstract public bool AggregateExpressionDataNodeResolved(Guid aggregateExpressionDataNodeIdentity);




        /// <summary>
        /// This property is true when data loaded in parent (dataloader ñ datasource) table.
        /// </summary>
        /// <MetaDataID>{11fa1bb7-4586-4e78-940d-7808192a3e58}</MetaDataID>
        abstract public bool DataLoadedInParentDataSource
        {
            get;
            set;
        }





        /// <MetaDataID>{EDD9954E-4699-4CC6-BDDF-4C8760AAAD96}</MetaDataID>
        [Serializable]
        public class RelationData
        {
            public string Name;
            public string ParentTableName;
            public string ChildTableName;
            public System.Collections.Generic.List<object> ParentColumns = new System.Collections.Generic.List<object>();
            public System.Collections.Generic.List<object> ChildColumns = new System.Collections.Generic.List<object>();
        }

        [Serializable]
        public struct StreamedTable
        {
            public byte[] StreamedData;
            public OOAdvantech.Collections.Generic.Dictionary<object, object> Objects;
            public Guid DataSourceIdentity;

        }


        /// <MetaDataID>{dc7eb8ee-18c9-4681-98ae-34e8a156e4ab}</MetaDataID>
        /// <summary>
        /// RemoteData defines a streamed table and it is usefull for distributed query
        /// </summary>
        public StreamedTable RemoteData
        {
            get
            {
                return Data.SerializeTable();
            }
        }
#if !DeviceDotNet
#endif

        #region DataSource relations meta data



        #region Info about relation with parent and subdatanodes
        /// <summary>
        /// If it is true the data source table has the identity coloumns, 
        /// otherwise table has the reference columns for relation with parent. 
        /// Always the identity columns has unique constrain 
        /// </summary>
        /// <MetaDataID>{6fb09e8f-869f-421b-84ad-af0389872580}</MetaDataID>
        abstract public bool HasRelationIdentityColumns
        {
            get;
        }

        /// <summary>
        /// If it is true the data source table has the reference columns,
        /// otherwise table has the identity coloumns for relation with parent. 
        /// Always the identity columns has unique constrain 
        /// </summary>
        /// <MetaDataID>{ad917417-3622-4e90-9fb5-16a5d92e276e}</MetaDataID>
        public bool HasRelationReferenceColumns
        {
            get
            {

                if (!(DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd))
                    return false;
                return !HasRelationIdentityColumns;
            }
        }

        /// <MetaDataID>{2ea99f10-5046-4128-b990-258c806ff985}</MetaDataID>
        /// <summary>
        /// If it is true the data source table has the reference columns,
        /// otherwise table has the identity coloumns for relation with subDataNode. 
        /// Always the identity columns has unique constrain       
        /// </summary>
        abstract public bool HasRelationIdentityColumnsFor(DataNode subDataNode);


        /// <MetaDataID>{2db66f94-8912-42ff-a7c8-92c318a17123}</MetaDataID>
        /// <summary>
        /// If it is true the data source table has the reference columns,
        /// otherwise table has the identity coloumns for relation with subDataNode. 
        /// Always the identity columns has unique constrain 
        /// </summary>
        public bool HasRelationReferenceColumnsFor(DataNode subDataNode)
        {
            return !HasRelationIdentityColumnsFor(subDataNode);
        }

        #endregion



        /// <summary>
        /// For recursive provide the parent identity columns.
        ///  </summary>
        /// <MetaDataID>{71b26594-eadb-42ea-b5c3-f8707c0b85bf}</MetaDataID>
        public abstract Dictionary<RelationPartIdentity, Dictionary<ObjectIdentityType, List<string>>> RecursiveParentColumns
        {
            get;
        }
        /// <summary>
        /// Recursive relation provide the child reference columns
        ///  </summary>
        /// <MetaDataID>{bdd9c146-ae46-4332-aeea-5c6fa1835a80}</MetaDataID>
        public abstract Dictionary<RelationPartIdentity, Dictionary<ObjectIdentityType, List<string>>> RecursiveParentReferenceColumns
        {
            get;
        }
        /// <MetaDataID>{52d18ac6-f4df-4fbe-92bd-5d9a65c5f45c}</MetaDataID>
        /// <summary>
        /// The Data perhaps contains more than one in ObjectIdentityTypes
        ///</summary>
        public abstract System.Collections.Generic.List<ObjectIdentityType> ObjectIdentityTypes
        {
            get;
        }

        ///<summary>
        ///This property defines the relation columns, 
        ///which used in table relation with parent data source. 
        ///</summary>
        /// <MetaDataID>{5D11BEB3-AD17-40DE-82D8-E56EF2F0DBB7}</MetaDataID>
        public abstract Dictionary<RelationPartIdentity, Dictionary<ObjectIdentityType, RelationColumns>> DataSourceRelationsColumnsWithParent
        {
            get;

        }
        ///<summary>
        ///This property defines the relation columns, 
        ///which used in table relation with subDataNodes data source. 
        ///</summary>
        /// <MetaDataID>{F4BB0893-BB0C-4035-84F4-EF9A96460D77}</MetaDataID>
        public abstract Dictionary<SubDataNodeIdentity, Dictionary<RelationPartIdentity, Dictionary<ObjectIdentityType, RelationColumns>>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get;
        }


        public string DataSourceRelationsIndexerColumnName
        {
            get
            {
                if (DataNode.AssignedMetaObject is AssociationEnd && (DataNode.AssignedMetaObject as AssociationEnd).Indexer)
                    return (DataNode.AssignedMetaObject as AssociationEnd).Name + "_Indexer";
                else
                    return null;

            }
        }
        //

        /// <MetaDataID>{e4ea3b06-9a85-4188-bc60-10ae32d6465d}</MetaDataID>
        public abstract Dictionary<SubDataNodeIdentity, Dictionary<MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>> GroupByKeyRelationColumns
        {
            get;
        }
        //Dictionary<SubDataNodeIdentity, string> _SubDataNodesReferenceStorageIdentityColumns ;
        //public Dictionary<SubDataNodeIdentity, string> SubDataNodesReferenceStorageIdentityColumns
        //{
        //    get
        //    {
        //        if (_SubDataNodesReferenceStorageIdentityColumns != null)
        //            return _SubDataNodesReferenceStorageIdentityColumns;

        //        _SubDataNodesReferenceStorageIdentityColumns = new Dictionary<Guid, string>();
        //        foreach (var subDataNodesRelationColumnsEntry in DataSourceRelationsColumnsWithSubDataNodes)
        //        {
        //            DataNode subDataNode = DataNode.HeaderDataNode.GetDataNode(subDataNodesRelationColumnsEntry.Key);
        //            if (subDataNode.ParentDataNode == DataNode)
        //            {
        //                if (HasRelationReferenceColumnsFor(subDataNode))
        //                    _SubDataNodesReferenceStorageIdentityColumns[subDataNode.Identity] = (subDataNode.AssignedMetaObject as AssociationEnd).ReferenceStorageIdentityColumnName;
        //            }
        //        }

        //        foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
        //        {
        //            foreach (var subDataNodesRelationColumnsEntry in hostedDataDataLoader.DataSourceRelationsColumnsWithSubDataNodes)
        //            {
        //                DataNode subDataNode = DataNode.HeaderDataNode.GetDataNode(subDataNodesRelationColumnsEntry.Key);
        //                if (hostedDataDataLoader.HasRelationReferenceColumnsFor(subDataNode))
        //                    _SubDataNodesReferenceStorageIdentityColumns[subDataNode.Identity] =hostedDataDataLoader.DataNode.Alias + "_" + (subDataNode.AssignedMetaObject as AssociationEnd).ReferenceStorageIdentityColumnName;
        //            }
        //        }
        //        foreach (DataLoader valueTypeDataLoader in ValueTypeDataLoaders)
        //        {
        //            foreach (var subDataNodesRelationColumnsEntry in valueTypeDataLoader.DataSourceRelationsColumnsWithSubDataNodes)
        //            {
        //                DataNode subDataNode = DataNode.HeaderDataNode.GetDataNode(subDataNodesRelationColumnsEntry.Key);
        //                if (valueTypeDataLoader.HasRelationReferenceColumnsFor(subDataNode))
        //                    _SubDataNodesReferenceStorageIdentityColumns[subDataNode.Identity] =valueTypeDataLoader.DataNode.ValueTypePathDiscription + (subDataNode.AssignedMetaObject as AssociationEnd).ReferenceStorageIdentityColumnName;
        //            }
        //        }
        //        foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
        //        {
        //            foreach (DataLoader hostedDataValueTypeDataLoader in hostedDataDataLoader.ValueTypeDataLoaders)
        //            {
        //                foreach (var subDataNodesRelationColumnsEntry in hostedDataValueTypeDataLoader.DataSourceRelationsColumnsWithSubDataNodes)
        //                {
        //                    DataNode subDataNode = DataNode.HeaderDataNode.GetDataNode(subDataNodesRelationColumnsEntry.Key);
        //                    if (hostedDataValueTypeDataLoader.HasRelationReferenceColumnsFor(subDataNode))
        //                        _SubDataNodesReferenceStorageIdentityColumns[subDataNode.Identity] =hostedDataDataLoader.DataNode.Alias + "_" + hostedDataValueTypeDataLoader.DataNode.ValueTypePathDiscription + (subDataNode.AssignedMetaObject as AssociationEnd).ReferenceStorageIdentityColumnName;
        //                }
        //            }
        //        }

        //        return _SubDataNodesReferenceStorageIdentityColumns;
        //    }
        //}

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{74b42342-cc16-41b4-ba54-dc6286b3eccb}</MetaDataID>
        OOAdvantech.Collections.Generic.Dictionary<SubDataNodeIdentity, AssotiationTableRelationshipData> _RelationshipsData = new OOAdvantech.Collections.Generic.Dictionary<SubDataNodeIdentity, AssotiationTableRelationshipData>();

        /// <MetaDataID>{95d56162-ab49-46ac-91b0-5f2af7c9410c}</MetaDataID>
        /// <summary>
        /// RelationshipsData dictionary contains relationship data for subDataNodes with ThrougthRelationTable property true 
        /// </summary>
        public OOAdvantech.Collections.Generic.Dictionary<SubDataNodeIdentity, AssotiationTableRelationshipData> RelationshipsData
        {
            get
            {
                if (DataLoadedInParentDataSource)
                    return GetDataLoader(DataNode.ParentDataNode).RelationshipsData;
                else
                    return _RelationshipsData;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1eec2ac9-c7cf-4e64-b91f-ed2e03e078dd}</MetaDataID>
        protected AssotiationTableRelationshipData _ParentRelationshipData;
        /// <MetaDataID>{902dc171-45c4-4d8e-a00b-981c20939259}</MetaDataID>
        public AssotiationTableRelationshipData ParentRelationshipData
        {
            get
            {
                if (DataLoadedInParentDataSource)
                    return null;
                else
                    return _ParentRelationshipData;
            }
        }



        #endregion


        /// <MetaDataID>{F7DA4FC2-55E2-4F63-B494-C9E660B8F18B}</MetaDataID>
        /// <summary>This classifier is the corresponding of the data node classifier and belongs to the storage metadata. </summary>
        public abstract Classifier Classifier
        {
            get;
        }



        /// <exclude>Excluded</exclude>
        internal protected IDataTable _Data = null;
        /// <MetaDataID>{4dbc8a72-b91e-42f8-bf9e-df4371056751}</MetaDataID>
        /// Defines the data which retrieved from DataLoader
        public virtual IDataTable Data
        {
            get
            {
                return _Data;
            }
        }



        /// <MetaDataID>{677E3847-427B-4EED-AB0C-944AE1C9728C}</MetaDataID>
        /// <summary>
        /// Retreives data from storage
        /// </summary>
        public abstract void RetrieveFromStorage();

        /// <exclude>Excluded</exclude>
        MetaDataRepository.ObjectQueryLanguage.SearchCondition _SearchCondition;
        /// <MetaDataID>{390613e1-8fca-4b32-8e1b-cf74ec240540}</MetaDataID>
        /// <summary>
        /// SearchCondition keeps info wich used from framework to filter query result
        /// </summary>
        public virtual List<SearchCondition> SearchConditions
        {
            get
            {
                return DataNode.SearchConditions;
            }
        }
        /// <MetaDataID>{1bac89c7-7b07-4412-b566-5d928d57486f}</MetaDataID>
        public virtual SearchCondition SearchCondition
        {
            get
            {
                return DataNode.SearchCondition;
            }
        }
        /// <MetaDataID>{503E5D93-EA73-464B-9818-2FFA9D8D64C2}</MetaDataID>
        /// <summary>
        /// Defines the columns names of data which retrieved from DMS (data managment system)
        /// </summary>
        protected abstract List<string> DataColumnNames
        {
            get;
        }
        /// <MetaDataID>{03278C51-02BD-4C5C-8248-DC3A7B27FDD4}</MetaDataID>
        public readonly DataNode DataNode;
        /// <MetaDataID>{77116b9f-32b0-455e-b2e8-52cc3d4fbbe6}</MetaDataID>
        protected DataLoader(DataNode dataNode)
        {

            DataNode = dataNode;
            foreach (var searchCondition in dataNode.BranchSearchConditions)
            {
                if (searchCondition != null)
                {
                    foreach (Criterion criterion in searchCondition.Criterions)
                    {
                        foreach (ComparisonTerm comparisonTerm in criterion.ComparisonTerms)
                        {
                            if (comparisonTerm.OQLStatement == null)
                                comparisonTerm.OQLStatement = dataNode.ObjectQuery;
                        }
                    }
                }
            }

            if ((dataNode is GroupDataNode) && (dataNode as GroupDataNode).GroupingSourceSearchCondition != null)
            {
                foreach (Criterion criterion in (dataNode as GroupDataNode).GroupingSourceSearchCondition.Criterions)
                {
                    foreach (ComparisonTerm comparisonTerm in criterion.ComparisonTerms)
                    {
                        if (comparisonTerm.OQLStatement == null)
                            comparisonTerm.OQLStatement = dataNode.ObjectQuery;
                    }
                }
            }
            foreach (AggregateExpressionDataNode aggregateExpressionDataNode in DataNode.SubDataNodes.OfType<AggregateExpressionDataNode>())
            {
                if (aggregateExpressionDataNode.SourceSearchCondition != null)
                    foreach (Criterion criterion in aggregateExpressionDataNode.SourceSearchCondition.Criterions)
                    {
                        foreach (ComparisonTerm comparisonTerm in criterion.ComparisonTerms)
                        {
                            if (comparisonTerm.OQLStatement == null)
                                comparisonTerm.OQLStatement = dataNode.ObjectQuery;
                        }
                    }

            }
        }







        ///<summary>
        ///ObjectActivation specifies when query engine converts the storage instance data to activate object
        ///True means object activation
        ///</summary>
        /// <MetaDataID>{230d16f7-c472-46f3-af37-9f44f068e6c8}</MetaDataID>
        public abstract bool ObjectActivation
        {
            get;
        }
        /// <MetaDataID>{36e8f4fb-a274-46cf-8c5c-29e1725d32df}</MetaDataID>
        /// <summary>
        /// Defines the value types sub data node data loaders which load thereís data on data loader table.
        /// </summary>
        protected System.Collections.Generic.List<DataLoader> ValueTypeDataLoaders
        {
            get
            {
                System.Collections.Generic.List<DataLoader> valueTypeDataLoaders = new System.Collections.Generic.List<DataLoader>();
                DataNode dataNode = DataNode;
                GetDataNodeValueTypeDataLoaders(valueTypeDataLoaders, DataNode);
                return valueTypeDataLoaders;
            }
        }

        /// <MetaDataID>{a138f2cd-3300-4a9d-991c-e2dc576fe08b}</MetaDataID>
        /// <summary>
        /// Defines all sub data node data loaders which load thereís data on data loader and there isnít value type data loaders.
        /// </summary>
        protected System.Collections.Generic.List<DataLoader> HostedDataDataLoaders
        {
            get
            {
                List<DataLoader> hostedDataDataLoaders = new List<DataLoader>();
                foreach (DataNode dataNode in DataNode.RealSubDataNodes)
                {
                    if (dataNode.Type == DataNode.DataNodeType.Object &&
                        (dataNode.DataSource as StorageDataSource).HasInObjectContextData &&
                        GetDataLoader(dataNode) is StorageDataLoader &&
                        !(dataNode.AssignedMetaObject is MetaDataRepository.Attribute) &&
                        (GetDataLoader(dataNode) as StorageDataLoader).DataLoadedInParentDataSource)
                    {
                        hostedDataDataLoaders.Add(GetDataLoader(dataNode));
                        hostedDataDataLoaders.AddRange(GetDataLoader(dataNode).HostedDataDataLoaders);
                    }
                    else if (dataNode.Type == DataNode.DataNodeType.Object && dataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                    {
                        hostedDataDataLoaders.AddRange(GetDataLoader(dataNode).HostedDataDataLoaders);
                    }
                }
                return hostedDataDataLoaders;
            }
        }



        /// <MetaDataID>{787df011-c682-42cf-9725-946b5d6d0021}</MetaDataID>
        /// <summary>
        /// Retrieve value types sub DataNode dataLoaders.
        /// </summary>
        /// <param name="dataNode">
        /// Defines the dataNode which perhaps contains value type subDataNode
        /// </param>
        /// <param name="valueTypeDataLoaders">
        /// Defines the collection where method adds the retrieved value type dataloaders
        /// </param>
        private void GetDataNodeValueTypeDataLoaders(System.Collections.Generic.List<DataLoader> valueTypeDataLoaders, DataNode dataNode)
        {
            foreach (DataNode subDataNode in dataNode.SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                {
                    valueTypeDataLoaders.Add(GetDataLoader(subDataNode));
                    GetDataNodeValueTypeDataLoaders(valueTypeDataLoaders, subDataNode);
                }
            }
        }

        /// <MetaDataID>{919c3c31-396d-4dc5-a975-3b7fe7d0b155}</MetaDataID>
        /// <summary>
        /// Returns the type of column with columnName
        /// </summary>
        protected abstract Type GetColumnType(string columnName);

        /// <MetaDataID>{5dea416e-eed5-4afa-b474-0da1834854c7}</MetaDataID>
        /// <summary>
        /// Convert value to new type.
        /// </summary>
        /// <param name="value">
        /// Defines value for convertion
        /// </param>
        /// <param name="type">
        /// Defines new type of value
        /// </param>
        protected abstract object Convert(object value, Type type);



        struct ObjectActivationData
        {
            public int RowIndex;
            public int ObjectColumnIndex;
            public int LoadObjectLinksIndex;
            public DataLoader DataLoader;


        }
        /// <MetaDataID>{9ea84bb0-7d95-4f99-8b89-308a7eec096c}</MetaDataID>
        List<ObjectActivationData> ObjectsActivationData = new List<ObjectActivationData>();


        /// <MetaDataID>{727e5b00-ca21-4a56-be0e-af1bba912117}</MetaDataID>
        bool ObjectsActivated;
        /// <MetaDataID>{873ec171-60dd-46b7-9285-e8ecc963b9bd}</MetaDataID>
        public void ActivateObjects()
        {
            if (!ObjectsActivated)
            {
                foreach (var objectActivationData in ObjectsActivationData)
                {
                    bool loadObjectLinks;

                    object _object = _Data.Rows[objectActivationData.RowIndex][objectActivationData.ObjectColumnIndex];
                    if (_object != DBNull.Value)
                    {
                        loadObjectLinks = (bool)_Data.Rows[objectActivationData.RowIndex][objectActivationData.LoadObjectLinksIndex];
                        if (objectActivationData.DataLoader.DataNode.ValueTypePath.Count == 0)
                        {
                            if (loadObjectLinks)
                            {
                                try
                                {
                                    PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(_object)?.ObjectActived();
                                }
                                catch (Exception error)
                                {
                                    try
                                    {
#if !DeviceDotNet
                                        //Error prone „ÂÏÈÛÂÈ ÏÂ message ÙÔ log file ÙÔÙÂ ·Ò‹„ÂÈ exception
                                        if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                                            System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
                                        System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                                        myLog.Source = "PersistencySystem";
                                        System.Diagnostics.Debug.WriteLine(
                                            error.Message + error.StackTrace);
                                        myLog.WriteEntry(error.Message + error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
#endif
                                    }
                                    catch (Exception)
                                    {

                                        
                                    }

                                }
                            }
                            //else
                            //    PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(_object)?.WaitUntilObjectIsActive();
                        }

                    }

                    //  object _object =objectActivationData.DataLoader.GetObject(objectActivationData.ObjectSate, out loadObjectLinks);
                    //  bool invalid1 = _Data.Rows[objectActivationData.RowIndex][objectActivationData.ObjectColumnIndex] != _object;// objectActivationData.DataLoader.GetObject(objectActivationData.ObjectSate, out loadObjectLinks);
                    ////   bool invalid2 = (_Data.Rows[objectActivationData.RowIndex][objectActivationData.LoadObjectLinksIndex] is bool) && ((bool)_Data.Rows[objectActivationData.RowIndex][objectActivationData.LoadObjectLinksIndex]) != loadObjectLinks;


                }
            }
            ObjectsActivated = true;
        }

        #region RowRemove code

        /////<summary>
        /////Property defines the columns names of row remove columns
        /////In case where data node has more than one search conditions then for each search condition 
        /////there is a boolean column where the value is true when row pass search condition and false when row doesn't pass 
        /////</summary>
        ///// <MetaDataID>{db0a0507-fdb2-4a47-95f5-33154d95f4db}</MetaDataID>
        //abstract public List<string> RowRemoveColumns
        //{
        //    get;
        //}

        #endregion

        /// <exclude>Excluded</exclude>
        Dictionary<string, string> _DataNodeColumnsNames = new Dictionary<string, string>();
        /// <MetaDataID>{18f672ef-f94e-47b1-a315-af64cae5d394}</MetaDataID>
        /// <summary>
        /// Defines a dictionary, which maps the RDBMS friendly name with the original OLTMS (Object Lifetime Management System) name.
        /// </summary>
        protected Dictionary<string, string> DataNodeColumnsNames
        {
            get
            {
                if (DataLoadedInParentDataSource)
                    return (GetDataLoader(DataNode.ParentDataNode) as DataLoader).DataNodeColumnsNames;
                else
                    return _DataNodeColumnsNames;
            }

        }


        /// <MetaDataID>{1d4d6ebe-c003-422c-a98f-b438a5b8e3fa}</MetaDataID>
        /// <summary>
        /// Load data from dataReader to data loader table
        /// If query wants objects, then method convert storage instance data to objects  
        /// </summary>
        /// <param name="dataReader">
        /// Defines the data reader where method use to retrieves data.
        /// </param>
        /// <param name="aliasesDictionary">
        /// Defines a map between the data reader column name and dataloader table column name. 
        /// </param>
        /// <param name="ensureDistinction">
        /// Sometimes data retrieve mechanism canít ensure row distinction and ensureDistinction parameter must be true.
        /// </param>
        public void LoadDataLoaderTable(IDataReader dataReader, Dictionary<string, string> aliasesDictionary, bool ensureDistinction)
        {

            if (_Data == null)
            {
                _Data = DataSource.DataObjectsInstantiator.CreateDataTable();

                _Data.DataSourceIdentity = DataNode.DataSource.Identity;
                _Data.OwnerDataSource = DataNode.DataSource;
            }
            System.Collections.Generic.Dictionary<string, int> columnsIndices = new System.Collections.Generic.Dictionary<string, int>();
            System.Collections.Generic.Dictionary<int, int> dataColumnsIndices = new Dictionary<int, int>();
            _Data.TableName = DataNode.Alias;

            //int[] sourceColumnsIndices = new int[DataColumnNames.Count];

            int maxObjectIdentityPartCount = 0;
            Dictionary<ObjectIdentityType, List<int>> objectIdentityTypesindicies = new Dictionary<ObjectIdentityType, List<int>>();

            #region RowRemove code

            //List<int> rowRemoveColumnsIndices = new List<int>();
            //List<int> dataRowRemoveColumnsIndices = new List<int>();

            #endregion


            if (ensureDistinction)
            {
                #region retrievies ObjectIdentityTypes parts indicies
                foreach (ObjectIdentityType objectIdentityType in ObjectIdentityTypes)
                {
                    System.Collections.Generic.List<int> partsIndicies = new List<int>();
                    objectIdentityTypesindicies[objectIdentityType] = partsIndicies;
                    if (objectIdentityType.Parts.Count > maxObjectIdentityPartCount)
                        maxObjectIdentityPartCount = objectIdentityType.Parts.Count;
                    foreach (IIdentityPart part in objectIdentityType.Parts)
                    {
                        partsIndicies.Add(-1);
                        for (int i = 0; i != dataReader.FieldCount; i++)
                        {
                            string columnName = null;
                            if (!aliasesDictionary.TryGetValue(dataReader.GetName(i), out columnName))
                                columnName = dataReader.GetName(i);
                            if (part.Name == columnName)
                            {
                                partsIndicies[partsIndicies.Count - 1] = i;
                                break;
                            }
                        }
                    }
                }
                #region RowRemove code
                //foreach (string rowRemoveColumnName in RowRemoveColumns)
                //{
                //    for (int i = 0; i != dataReader.FieldCount; i++)
                //    {
                //        string columnName = null;
                //        if (!aliasesDictionary.TryGetValue(dataReader.GetName(i), out columnName))
                //            columnName = dataReader.GetName(i);
                //        if (rowRemoveColumnName == columnName)
                //        {
                //            rowRemoveColumnsIndices.Add(i);

                //            break;
                //        }
                //    }
                //}
                #endregion
                #endregion
            }


            #region Removed Code for  reference  storageIdentity 
            #region Add reference storage identity columns
            List<int> referenceStorageIdentityColumnsIndices = new List<int>();


            //foreach (var referenceStorageIdentityColumn in (from subDataNodeRelationColumn in DataSourceRelationsColumnsWithSubDataNodes
            //                                                from relationPartColumns in subDataNodeRelationColumn.Value
            //                                                from objectIdentityTypeColumns in relationPartColumns.Value
            //                                                where !string.IsNullOrEmpty(objectIdentityTypeColumns.Value.StorageIdentityColumn) && objectIdentityTypeColumns.Value.StorageIdentityColumn != "StorageIdentity"
            //                                                select objectIdentityTypeColumns.Value.StorageIdentityColumn).Distinct())
            //{
            //    referenceStorageIdentityColumnsIndices.Add(_Data.Columns.Add(referenceStorageIdentityColumn, typeof(int)).Ordinal);
            //}


            //foreach (var referenceStorageIdentityColumn in (from relationPartColumns in DataSourceRelationsColumnsWithParent
            //                                                from objectIdentityTypeColumns in relationPartColumns.Value
            //                                                where !string.IsNullOrEmpty(objectIdentityTypeColumns.Value.StorageIdentityColumn) && objectIdentityTypeColumns.Value.StorageIdentityColumn != "StorageIdentity"
            //                                                select objectIdentityTypeColumns.Value.StorageIdentityColumn).Distinct())
            //{

            //    referenceStorageIdentityColumnsIndices.Add(_Data.Columns.Add(referenceStorageIdentityColumn, typeof(int)).Ordinal);
            //}

            #endregion
            #endregion


            #region Add value type object columns
            foreach (DataLoader dataLoader in ValueTypeDataLoaders)
            {

                if (dataLoader.ObjectActivation)
                {
                    _Data.Columns.Add(dataLoader.DataNode.ValueTypePathDiscription + "Object", dataLoader.DataNode.Classifier.GetExtensionMetaObject<System.Type>()).ReadOnly = false;
                    _Data.Columns.Add(dataLoader.DataNode.ValueTypePathDiscription + "LoadObjectLinks", typeof(bool)).ReadOnly = false;
                }
            }

            //Add hosted dataloader value type object columns
            foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
            {
                foreach (DataLoader hostedDataValueTypeDataLoader in hostedDataDataLoader.ValueTypeDataLoaders)
                {
                    if (hostedDataValueTypeDataLoader.ObjectActivation)
                    {
                        _Data.Columns.Add(hostedDataDataLoader.DataNode.Alias + "_" + hostedDataValueTypeDataLoader.DataNode.ValueTypePathDiscription + "Object", hostedDataValueTypeDataLoader.DataNode.Classifier.GetExtensionMetaObject<System.Type>()).ReadOnly = false;
                        _Data.Columns.Add(hostedDataDataLoader.DataNode.Alias + "_" + hostedDataValueTypeDataLoader.DataNode.ValueTypePathDiscription + "LoadObjectLinks", typeof(bool)).ReadOnly = false;
                    }
                }

            }
            #endregion

            #region Average data transformation

            Dictionary<string, string> averageColumns = new Dictionary<RelationPartIdentity, RelationPartIdentity>();

            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type == ObjectQueryLanguage.DataNode.DataNodeType.Average)
                {

                    AggregateExpressionDataNode countDataNode = (from dataNode in DataNode.SubDataNodes.OfType<AggregateExpressionDataNode>()
                                                                 where dataNode.Type == DataNode.DataNodeType.Count && (dataNode as AggregateExpressionDataNode).SourceSearchCondition == (subDataNode as AggregateExpressionDataNode).SourceSearchCondition
                                                                 select dataNode).FirstOrDefault();
                    if (countDataNode == null)
                        averageColumns[MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(subDataNode)] = null;
                    else
                        averageColumns[MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(subDataNode)] = MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(countDataNode);

                }
            }

            int[,] avrgDictionary = new int[averageColumns.Count, 2];
            int avrgDictionaryPos = 0;

            #endregion

            if (ObjectActivation)
            {
                using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(Transactions.TransactionOption.Suppress))
                {
                    #region Remove storage instance data columns

                    /// To find wich columns must be removed, system builds a collection all columns and remove 
                    /// from this collection the needed columns. the remaining columns will be removed.

                    System.Collections.Generic.List<string> uselessColumns = DataColumnNames;
                    uselessColumns.Remove("OSM_StorageIdentity");
                    if (DataNode.ParticipateInGroopByAsKey ||
                        DataNode.ParticipateInGroopByAsGrouped ||
                        DataNode.ParticipateInWereClause ||
                        DataNode.DataSource.HasRelationIdentityColumns)
                    {
                        ///Object identity is nessesary
                        foreach (ObjectIdentityType objectIdentityType in ObjectIdentityTypes)
                        {
                            foreach (IIdentityPart part in objectIdentityType.Parts)
                                uselessColumns.Remove(part.Name);
                        }
                    }
                    // if (DataNode.DataSource.HasRelationReferenceColumns)
                    {
                        foreach (System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns> dataSourceRelationsParts in DataSourceRelationsColumnsWithParent.Values)
                        {
                            foreach (RelationColumns relationColumns in dataSourceRelationsParts.Values)
                                foreach (string columnName in relationColumns.ObjectIdentityColumns)
                                    uselessColumns.Remove(columnName);
                        }
                    }
                    foreach (var recursiveParentRelationParts in RecursiveParentReferenceColumns.Values)
                    {
                        foreach (System.Collections.Generic.List<string> columns in recursiveParentRelationParts.Values)
                            foreach (string columnName in columns)
                                uselessColumns.Remove(columnName);
                    }
                    foreach (System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>> recursiveParentRelationParts in RecursiveParentColumns.Values)
                    {
                        foreach (System.Collections.Generic.List<string> columns in recursiveParentRelationParts.Values)
                            foreach (string columnName in columns)
                                uselessColumns.Remove(columnName);
                    }

                    foreach (System.Collections.Generic.KeyValuePair<SubDataNodeIdentity, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>>> entry in DataSourceRelationsColumnsWithSubDataNodes)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> relationPartEntry in DataSourceRelationsColumnsWithSubDataNodes[entry.Key])
                            foreach (var parentColumnsNames in DataSourceRelationsColumnsWithSubDataNodes[entry.Key][relationPartEntry.Key].Values)
                                foreach (string columnName in parentColumnsNames.ObjectIdentityColumns)
                                    uselessColumns.Remove(columnName);
                    }

                    if (!string.IsNullOrWhiteSpace(DataSourceRelationsIndexerColumnName))
                        uselessColumns.Remove(DataSourceRelationsIndexerColumnName);

                    foreach (DataNode subDataNode in DataNode.SubDataNodes)
                    {




                        if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                        subDataNode.Type == DataNode.DataNodeType.Object)
                        {
                            foreach (DataNode valueTypeSubDataNode in subDataNode.SubDataNodes)
                            {
                                if (valueTypeSubDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                                    uselessColumns.Remove(valueTypeSubDataNode.ParentDataNode.ValueTypePathDiscription + "_" + valueTypeSubDataNode.Name);
                            }
                        }

                        if (subDataNode is AggregateExpressionDataNode)
                            uselessColumns.Remove(GetLocalDataColumnName(subDataNode));
                        if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        {
                            if (LocalDataColumnExistFor(subDataNode))
                            {
                                uselessColumns.Remove(GetLocalDataColumnName(subDataNode));
                                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                    subDataNode.SubDataNodes.Count > 0 &&
                                    subDataNode.Classifier.FullName == typeof(DateTime).FullName)
                                {
                                    foreach (DataNode dateTimeMemberDataNode in subDataNode.SubDataNodes)
                                        uselessColumns.Remove(subDataNode.Name + "_" + dateTimeMemberDataNode.Name);
                                }
                            }
                        }
                    }

                    foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                    {
                        if (hostedDataDataLoader.DataNode.ParticipateInGroopByAsKey || hostedDataDataLoader.DataNode.ParticipateInWereClause)
                        {
                            foreach (ObjectIdentityType objectIdentityType in hostedDataDataLoader.ObjectIdentityTypes)
                            {
                                foreach (IIdentityPart part in objectIdentityType.Parts)
                                    uselessColumns.Remove(hostedDataDataLoader.DataNode.Alias + "_" + part.Name);
                            }
                        }

                        foreach (System.Collections.Generic.KeyValuePair<SubDataNodeIdentity, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>>> entry in hostedDataDataLoader.DataSourceRelationsColumnsWithSubDataNodes)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> relationPartEntry in hostedDataDataLoader.DataSourceRelationsColumnsWithSubDataNodes[entry.Key])
                                foreach (RelationColumns parentRelationColumns in hostedDataDataLoader.DataSourceRelationsColumnsWithSubDataNodes[entry.Key][relationPartEntry.Key].Values)
                                    foreach (string columnName in parentRelationColumns.ObjectIdentityColumns)
                                        uselessColumns.Remove(columnName);
                        }

                        foreach (DataNode subDataNode in hostedDataDataLoader.DataNode.SubDataNodes)
                        {
                            if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute &&
                                subDataNode.Type == DataNode.DataNodeType.Object)
                            {
                                foreach (DataNode valueTypeSubDataNode in subDataNode.SubDataNodes)
                                {
                                    if (valueTypeSubDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                                        uselessColumns.Remove(hostedDataDataLoader.DataNode.Alias + "_" + valueTypeSubDataNode.ParentDataNode.ValueTypePathDiscription + "_" + valueTypeSubDataNode.Name);
                                }
                            }
                            if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            {
                                uselessColumns.Remove(hostedDataDataLoader.GetLocalDataColumnName(subDataNode));
                                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                    subDataNode.SubDataNodes.Count > 0 &&
                                    subDataNode.Classifier.FullName == typeof(DateTime).FullName)
                                {
                                    foreach (DataNode dateTimeMemberDataNode in subDataNode.SubDataNodes)
                                        uselessColumns.Remove(hostedDataDataLoader.DataNode.Alias + "_" + subDataNode.Name + "_" + dateTimeMemberDataNode.Name);
                                }
                            }
                        }

                    }



                    foreach (DataLoader dataLoader in ValueTypeDataLoaders)
                    {
                        foreach (System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> dataSourceRelationsColumns in dataLoader.DataSourceRelationsColumnsWithSubDataNodes.Values)
                        {
                            foreach (System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns> objectIdentityTypesEntry in dataSourceRelationsColumns.Values)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, RelationColumns> entry in objectIdentityTypesEntry)
                                {
                                    System.Collections.Generic.List<string> parentColumnsNames = entry.Value.ObjectIdentityColumns;
                                    foreach (string columnName in parentColumnsNames)
                                        uselessColumns.Remove(columnName);
                                }
                            }
                        }
                        foreach (DataNode subDataNode in dataLoader.DataNode.SubDataNodes)
                        {
                            if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                                uselessColumns.Remove(subDataNode.Name);
                        }
                    }
                    foreach (string columnName in uselessColumns)
                        if (_Data.Columns.Contains(columnName))
                        {
                            System.Diagnostics.Debug.Assert(false, "Remove columns from DataLoader Table");
                            _Data.Columns.Remove(columnName);
                        }
                    #endregion

                    #region Add object activation columns. One column for object and one for LoadObjectLinks flag

                    int objectColumnIndex = -1;
                    int loadObjectLinksIndex = -1;
                    if (ObjectActivation)
                    {
                        System.Type type = DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                        objectColumnIndex = _Data.Columns.Add(DataNode.ValueTypePathDiscription + "Object", type).Ordinal;
                        loadObjectLinksIndex = _Data.Columns.Add(DataNode.ValueTypePathDiscription + "LoadObjectLinks", typeof(bool)).Ordinal;
                    }

                    //if (DataNode.ParticipateInGroopByAsKey)
                    //    _Data.Columns.Add("@" +  "Object_Identity", typeof(OOAdvantech.PersistenceLayer.ObjectID));

                    foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                    {
                        if (hostedDataDataLoader.ObjectActivation)
                        {
                            System.Type type = hostedDataDataLoader.DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                            _Data.Columns.Add(hostedDataDataLoader.DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object", type);
                            _Data.Columns.Add(hostedDataDataLoader.DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "LoadObjectLinks", typeof(bool));
                        }

                        if (hostedDataDataLoader.DataNode.ParticipateInGroopByAsKey)
                            _Data.Columns.Add("@" + hostedDataDataLoader.DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object_Identity", typeof(OOAdvantech.PersistenceLayer.ObjectID));
                    }

                    #endregion

                    #region Build column index map between Data table and dataReader table
                    for (int i = 0; i != dataReader.FieldCount; i++)
                    {
                        string columnName = null;
                        if (!aliasesDictionary.TryGetValue(dataReader.GetName(i), out columnName))
                            columnName = dataReader.GetName(i);
                        if (!uselessColumns.Contains(columnName))
                        {
                            string dataNodeColumnName = null;
                            if (!DataNodeColumnsNames.TryGetValue(columnName, out dataNodeColumnName))
                                dataNodeColumnName = columnName;

                            string avrgCountName = null;
                            if (averageColumns.TryGetValue(dataNodeColumnName, out avrgCountName))
                            {
                                Data.Columns.Add(dataNodeColumnName, typeof(AverageValue)).ReadOnly = false;
                                dataColumnsIndices[Data.Columns.IndexOf(dataNodeColumnName)] = i;
                                if (avrgCountName != null)
                                {
                                    avrgDictionary[avrgDictionaryPos, 0] = i;
                                    avrgDictionary[avrgDictionaryPos, 1] = dataReader.GetOrdinal(avrgCountName);
                                    avrgDictionaryPos++;
                                }
                                else
                                {
                                    avrgDictionary[avrgDictionaryPos, 0] = i;
                                    avrgDictionary[avrgDictionaryPos, 1] = -1;
                                    avrgDictionaryPos++;
                                }
                            }
                            else
                            {
                                Data.Columns.Add(dataNodeColumnName, GetColumnType(columnName)).ReadOnly = false;
                                dataColumnsIndices[Data.Columns.IndexOf(dataNodeColumnName)] = i;
                            }
                            //if (DataNodeColumnsNames.TryGetValue(columnName, out dataNodeColumnName))
                            //{
                            //    Data.Columns.Add(dataNodeColumnName, GetColumnType(columnName)).ReadOnly = false;
                            //    dataColumnsIndices[Data.Columns.IndexOf(dataNodeColumnName)] = i;
                            //}
                            //else
                            //{
                            //    Data.Columns.Add(columnName, GetColumnType(columnName)).ReadOnly = false;
                            //    dataColumnsIndices[Data.Columns.IndexOf(columnName)] = i;
                            //}

                        }
                        else
                        {

                        }
                        columnsIndices.Add(columnName.ToLower(), i);
                    }

                    #region RowRemove code

                    //foreach (string rowRemoveColumnName in RowRemoveColumns)
                    //    dataRowRemoveColumnsIndices.Add(Data.Columns.IndexOf(rowRemoveColumnName));

                    #endregion



                    #endregion

                    #region RowRemove code
                    //if (DataNode.SearchConditions.Count > 0)
                    //{
                    //    foreach (var searchCondition in DataNode.SearchConditions)
                    //    {
                    //        if (searchCondition != null)
                    //        {

                    //            string columnName = DataSource.GetRowRemoveColumnName(DataNode, searchCondition);
                    //            if (!_Data.Columns.Contains(columnName))
                    //                _Data.Columns.Add(columnName, typeof(bool));
                    //        }
                    //    }
                    //}

                    //foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                    //{
                    //    if (hostedDataDataLoader.DataNode.SearchConditions.Count > 0)
                    //    {
                    //        foreach (var searchCondition in hostedDataDataLoader.DataNode.SearchConditions)
                    //        {
                    //            if (searchCondition != null)
                    //            {
                    //                string columnName = DataSource.GetRowRemoveColumnName(hostedDataDataLoader.DataNode, searchCondition);
                    //                if (!_Data.Columns.Contains(columnName))
                    //                    _Data.Columns.Add(columnName, typeof(bool));
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion

                    UpdateDataTableColumnsNames();
                    _Data.BeginLoadData();

                    object[] retrievingValues = new object[dataReader.FieldCount];
                    object[] values = new object[_Data.Columns.Count];
                    object[] lastObjectIdentityPartsValues = new object[maxObjectIdentityPartCount];
                    object[] currentObjectIdentityPartsValues = new object[maxObjectIdentityPartCount];

                    ObjectIdentityType currObjectIDentiType = null;
                    Type[] valuesTypes = null;
                    int rowIndex = -1;
                    while (dataReader.Read())
                    {
                        if (valuesTypes == null)
                        {
                            valuesTypes = new Type[_Data.Columns.Count];
                            foreach (int index in dataColumnsIndices.Keys)
                            {
                                IDataColumn column = _Data.Columns[index];
                                if (column.DataType != dataReader.GetFieldType(dataColumnsIndices[index]))
                                    valuesTypes[index] = column.DataType;
                            }
                        }

                        dataReader.GetValues(retrievingValues);
                        if (ensureDistinction)
                        {
                            #region if objectIdentity values is the same as the values of last record continue with the next record

                            bool doubleRecord = true;
                            foreach (System.Collections.Generic.KeyValuePair<ObjectIdentityType, List<int>> objectIdentityIndicies in objectIdentityTypesindicies)
                            {
                                int i = 0;
                                bool allNull = true;
                                foreach (int index in objectIdentityIndicies.Value)
                                {
                                    object partValue = retrievingValues[index];
                                    if (!(partValue is DBNull) && partValue != null)
                                        allNull = false;
                                    if (objectIdentityIndicies.Key != currObjectIDentiType ||
                                        !lastObjectIdentityPartsValues[i].Equals(partValue))
                                        doubleRecord = false;
                                    currentObjectIdentityPartsValues[i] = partValue;
                                    i++;
                                }
                                if (allNull)
                                    doubleRecord = true;
                                else
                                {
                                    currObjectIDentiType = objectIdentityIndicies.Key;
                                    lastObjectIdentityPartsValues = currentObjectIdentityPartsValues;
                                    break;
                                }
                            }
                            if (doubleRecord)
                            {
                                #region RowRemove code
                                //int i = 0;
                                //foreach (int rowRemoveIndex in rowRemoveColumnsIndices)
                                //{
                                //    object value = retrievingValues[rowRemoveIndex];
                                //    if (value is bool && !((bool)value))
                                //        values[dataRowRemoveColumnsIndices[i++]] = value;
                                //}
                                #endregion
                                continue;
                            }
                            #endregion
                        }
                        rowIndex++;
                        bool loadObjectLinks = false;
                        foreach (KeyValuePair<int, int> entry in dataColumnsIndices)
                        {
                            if (valuesTypes[entry.Key] != null)
                                values[entry.Key] = retrievingValues[entry.Value] = Convert(retrievingValues[entry.Value], valuesTypes[entry.Key]);
                            else
                                values[entry.Key] = retrievingValues[entry.Value];
                        }
                        ObjectsActivationData.Add(new ObjectActivationData()
                        {
                            RowIndex = rowIndex,
                            ObjectColumnIndex = objectColumnIndex,
                            LoadObjectLinksIndex = loadObjectLinksIndex,
                            DataLoader = this
                        });
                        values[objectColumnIndex] = GetObject(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices), out loadObjectLinks);
                        values[loadObjectLinksIndex] = loadObjectLinks;

                        foreach (int referenceStorageIdentityIndex in referenceStorageIdentityColumnsIndices)
                        {
                            if (this is StorageDataLoader)
                                values[referenceStorageIdentityIndex] = (this as StorageDataLoader).DataLoaderMetadata.QueryStorageID;
                        }


                        //if (DataNode.ParticipateInGroopByAsKey && DataNode.ValueTypePath.Count == 0)
                        //{
                        //    object oid = GetObjectIdentity(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices));
                        //    values[ObjectIdentityColumnIndex] = oid;
                        //}

                        foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                        {
                            if (hostedDataDataLoader.ObjectActivation)
                            {
                                ObjectsActivationData.Add(new ObjectActivationData()
                                {
                                    RowIndex = rowIndex,
                                    ObjectColumnIndex = hostedDataDataLoader.ObjectColumnIndex,
                                    LoadObjectLinksIndex = hostedDataDataLoader.LoadObjectLinksIndex,
                                    DataLoader = hostedDataDataLoader
                                });

                                values[hostedDataDataLoader.ObjectColumnIndex] = hostedDataDataLoader.GetObject(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices), out loadObjectLinks);
                                values[hostedDataDataLoader.LoadObjectLinksIndex] = loadObjectLinks;
                            }
                            //if (hostedDataDataLoader.DataNode.ParticipateInGroopByAsKey && hostedDataDataLoader.DataNode.ValueTypePath.Count==0)
                            //{
                            //    object oid = hostedDataDataLoader.GetObjectIdentity(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices));
                            //    values[hostedDataDataLoader.ObjectIdentityColumnIndex] = oid;
                            //}
                            foreach (DataLoader dataLoader in hostedDataDataLoader.ValueTypeDataLoaders)
                            {
                                if (dataLoader.ObjectActivation)
                                {

                                    ObjectsActivationData.Add(new ObjectActivationData()
                                    {
                                        RowIndex = rowIndex,
                                        ObjectColumnIndex = dataLoader.ObjectColumnIndex,
                                        LoadObjectLinksIndex = dataLoader.LoadObjectLinksIndex,
                                        DataLoader = dataLoader
                                    });

                                    values[dataLoader.ObjectColumnIndex] = dataLoader.GetObject(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices), out loadObjectLinks);
                                    values[dataLoader.LoadObjectLinksIndex] = loadObjectLinks;
                                }
                            }
                        }
                        foreach (DataLoader dataLoader in ValueTypeDataLoaders)
                        {
                            if (dataLoader.ObjectActivation)
                            {
                                ObjectsActivationData.Add(new ObjectActivationData()
                                {
                                    //ObjectSate = new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices),
                                    RowIndex = rowIndex,
                                    ObjectColumnIndex = dataLoader.ObjectColumnIndex,
                                    LoadObjectLinksIndex = dataLoader.LoadObjectLinksIndex,
                                    DataLoader = dataLoader
                                });

                                values[dataLoader.ObjectColumnIndex] = dataLoader.GetObject(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices), out loadObjectLinks);
                                values[dataLoader.LoadObjectLinksIndex] = loadObjectLinks;
                            }
                        }
                        IDataRow dataRow = _Data.LoadDataRow(values, LoadOption.OverwriteChanges);
                    }
                    _Data.EndLoadData();

                    stateTransition.Consistent = true;
                }
            }
            else
            {



                foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                {
                    if (hostedDataDataLoader.ObjectActivation)
                    {
                        Type type = hostedDataDataLoader.DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type;// ModulePublisher.ClassRepository.GetType(DataNode.Classifier.FullName, "");
                        _Data.Columns.Add(hostedDataDataLoader.DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object", type);
                        _Data.Columns.Add(hostedDataDataLoader.DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "LoadObjectLinks", typeof(bool));
                    }
                    if (hostedDataDataLoader.DataNode.ParticipateInGroopByAsKey)
                        _Data.Columns.Add("@" + hostedDataDataLoader.DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object_Identity", typeof(OOAdvantech.PersistenceLayer.ObjectID));
                }
                for (int i = 0; i != dataReader.FieldCount; i++)
                {
                    string columnName = null;
                    if (!aliasesDictionary.TryGetValue(dataReader.GetName(i), out columnName))
                        columnName = dataReader.GetName(i);

                    string avrgCountName = null;
                    if (averageColumns.TryGetValue(columnName, out avrgCountName))
                    {
                        Data.Columns.Add(columnName, typeof(AverageValue)).ReadOnly = false;
                        dataColumnsIndices[Data.Columns.IndexOf(columnName)] = i;

                        if (avrgCountName != null)
                        {
                            avrgDictionary[avrgDictionaryPos, 0] = i;
                            avrgDictionary[avrgDictionaryPos, 1] = dataReader.GetOrdinal(avrgCountName);
                            avrgDictionaryPos++;
                        }
                        else
                        {
                            avrgDictionary[avrgDictionaryPos, 0] = i;
                            avrgDictionary[avrgDictionaryPos, 1] = -1;
                            avrgDictionaryPos++;
                        }
                    }
                    else
                    {
                        if (Data.Columns.IndexOf(columnName) == -1)
                            Data.Columns.Add(columnName, GetColumnType(columnName)).ReadOnly = false;

                        dataColumnsIndices[Data.Columns.IndexOf(columnName)] = i;

                    }
                    columnsIndices.Add(columnName.ToLower(), i);
                }
                #region RowRemove code

                //if (DataNode.SearchConditions.Count > 0)
                //{
                //    foreach (var searchCondition in DataNode.SearchConditions)
                //    {
                //        if (searchCondition != null)
                //        {
                //            string columnName = DataSource.GetRowRemoveColumnName(DataNode, searchCondition);
                //            if (!_Data.Columns.Contains(columnName))
                //                _Data.Columns.Add(columnName, typeof(bool));
                //        }
                //    }
                //}
                //foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                //{
                //    if (hostedDataDataLoader.DataNode.SearchConditions.Count > 0)
                //    {
                //        foreach (var searchCondition in hostedDataDataLoader.DataNode.SearchConditions)
                //        {
                //            if (searchCondition != null)
                //            {
                //                string columnName = DataSource.GetRowRemoveColumnName(hostedDataDataLoader.DataNode, searchCondition);
                //                if (!_Data.Columns.Contains(columnName))
                //                    _Data.Columns.Add(columnName, typeof(bool));
                //            }
                //        }
                //    }
                //}

                #endregion

                if (DataNode is GroupDataNode)
                    _Data.Columns.Add(GetGroupingDataColumnName(DataNode), typeof(object));


                UpdateDataTableColumnsNames();

                _Data.BeginLoadData();
                object[] values = new object[_Data.Columns.Count];
                object[] retrievingValues = new object[dataReader.FieldCount];

                object[] lastObjectIdentityPartsValues = new object[maxObjectIdentityPartCount];
                object[] currentObjectIdentityPartsValues = new object[maxObjectIdentityPartCount];

                Type[] valuesTypes = null;


                ObjectIdentityType currObjectIDentiType = null;
                int rowIndex = -1;
                while (dataReader.Read())
                {
                    if (valuesTypes == null)
                    {
                        valuesTypes = new Type[_Data.Columns.Count];
                        foreach (int index in dataColumnsIndices.Keys)
                        {
                            IDataColumn column = _Data.Columns[index];
                            if (column.DataType != dataReader.GetFieldType(dataColumnsIndices[index]))
                                valuesTypes[index] = column.DataType;
                        }
                    }

                    rowIndex++;
                    dataReader.GetValues(retrievingValues);
                    if (ensureDistinction)
                    {
                        #region if objectIdentity values is the same as the values of last record continue with the next record
                        bool doubleRecord = true;
                        foreach (KeyValuePair<ObjectIdentityType, List<int>> objectIdentityIndicies in objectIdentityTypesindicies)
                        {
                            int i = 0;
                            bool allNull = true;
                            foreach (int index in objectIdentityIndicies.Value)
                            {
                                object partValue = retrievingValues[index];
                                if (!(partValue is DBNull) && partValue != null)
                                    allNull = false;
                                if (objectIdentityIndicies.Key != currObjectIDentiType ||
                                    lastObjectIdentityPartsValues[i] != partValue)
                                    doubleRecord = false;
                                currentObjectIdentityPartsValues[i] = partValue;
                                i++;
                            }
                            if (allNull)
                                doubleRecord = true;
                            else
                            {
                                currObjectIDentiType = objectIdentityIndicies.Key;
                                currentObjectIdentityPartsValues.CopyTo(lastObjectIdentityPartsValues, 0);
                                break;
                            }
                        }
                        if (doubleRecord)
                            continue;
                        #endregion
                    }

                    foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
                    {
                        if (hostedDataDataLoader.ObjectActivation)
                        {

                            ObjectsActivationData.Add(new ObjectActivationData()
                            {
                                //ObjectSate = new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices),
                                RowIndex = rowIndex,
                                ObjectColumnIndex = hostedDataDataLoader.ObjectColumnIndex,
                                LoadObjectLinksIndex = hostedDataDataLoader.LoadObjectLinksIndex,
                                DataLoader = hostedDataDataLoader
                            });

                            bool loadObjectLinks = false;
                            values[hostedDataDataLoader.ObjectColumnIndex] = hostedDataDataLoader.GetObject(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices), out loadObjectLinks);
                            values[hostedDataDataLoader.LoadObjectLinksIndex] = loadObjectLinks;
                        }

                        //if (hostedDataDataLoader.DataNode.ParticipateInGroopByAsKey && hostedDataDataLoader.DataNode.ValueTypePath.Count == 0)
                        //{
                        //    object oid = hostedDataDataLoader.GetObjectIdentity(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices));
                        //    values[hostedDataDataLoader.ObjectIdentityColumnIndex] = oid;
                        //}
                        foreach (DataLoader dataLoader in hostedDataDataLoader.ValueTypeDataLoaders)
                        {
                            if (dataLoader.ObjectActivation)
                            {
                                ObjectsActivationData.Add(new ObjectActivationData()
                                {
                                    //ObjectSate = new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices),
                                    RowIndex = rowIndex,
                                    ObjectColumnIndex = dataLoader.ObjectColumnIndex,
                                    LoadObjectLinksIndex = dataLoader.LoadObjectLinksIndex,
                                    DataLoader = dataLoader
                                });
                                bool loadObjectLinks = false;



                                values[dataLoader.ObjectColumnIndex] = dataLoader.GetObject(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices), out loadObjectLinks);
                                values[dataLoader.LoadObjectLinksIndex] = loadObjectLinks;
                            }
                        }
                    }
                    foreach (DataLoader dataLoader in ValueTypeDataLoaders)
                    {
                        if (dataLoader.ObjectActivation)
                        {

                            ObjectsActivationData.Add(new ObjectActivationData()
                            {
                                //ObjectSate = new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices),
                                RowIndex = rowIndex,
                                ObjectColumnIndex = dataLoader.ObjectColumnIndex,
                                LoadObjectLinksIndex = dataLoader.LoadObjectLinksIndex,
                                DataLoader = dataLoader
                            });
                            bool loadObjectLinks = false;


                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                            {
                                values[dataLoader.ObjectColumnIndex] = dataLoader.GetObject(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices), out loadObjectLinks);
                                stateTransition.Consistent = true;
                            }


                            values[dataLoader.LoadObjectLinksIndex] = loadObjectLinks;
                        }
                    }

                    //if (DataNode.ParticipateInGroopByAsKey && DataNode.ValueTypePath.Count == 0)
                    //{
                    //    object oid = GetObjectIdentity(new PersistenceLayer.StorageInstanceRef.ObjectSate(retrievingValues, columnsIndices));
                    //    values[ObjectIdentityColumnIndex] = oid;
                    //}
                    foreach (KeyValuePair<int, int> entry in dataColumnsIndices)
                    {
                        AverageValue avrgValue = null;
                        for (int i = 0; i != averageColumns.Count; i++)
                            if (avrgDictionary[i, 0] == entry.Value)
                            {
                                avrgValue = new AverageValue();
                                avrgValue.Average = (decimal)Convert(retrievingValues[entry.Value], typeof(decimal));
                                if (avrgDictionary[i, 1] != -1)
                                {
                                    if (retrievingValues[avrgDictionary[i, 1]] is DBNull || retrievingValues[avrgDictionary[i, 1]] == null)
                                        avrgValue.AverageCount = -1;
                                    else
                                        avrgValue.AverageCount = (int)Convert(retrievingValues[avrgDictionary[i, 1]], typeof(int));
                                }
                                else
                                    avrgValue.AverageCount = -1;

                                values[entry.Key] = avrgValue;
                                break;
                            }
                        if (avrgValue == null)
                        {
                            if (valuesTypes[entry.Key] != null)
                                values[entry.Key] = retrievingValues[entry.Value] = Convert(retrievingValues[entry.Value], valuesTypes[entry.Key]);
                            else
                                values[entry.Key] = retrievingValues[entry.Value];
                        }

                    }

                    foreach (int referenceStorageIdentityIndex in referenceStorageIdentityColumnsIndices)
                    {
                        if (this is StorageDataLoader)
                            values[referenceStorageIdentityIndex] = (this as StorageDataLoader).DataLoaderMetadata.QueryStorageID;
                    }

                    IDataRow dataRow = _Data.LoadDataRow(values, LoadOption.OverwriteChanges);
                    if (DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        MultiPartKey groupingKey = DataNode.DataSource.GetGroupingKey(dataRow);
                        dataRow[DataLoader.GetGroupingDataColumnName(DataNode)] = new GroupingEntry(groupingKey, new List<CompositeRowData>());
                    }


                }
                _Data.TableName = DataNode.Alias;
                _Data.EndLoadData();
            }


        }




        /// <MetaDataID>{435e0f2d-dc09-41ae-b634-63d63decb126}</MetaDataID>
        public static string GetGroupingDataColumnName(DataNode dataNode)
        {
            if (dataNode is GroupDataNode)
                return dataNode.Alias + "_Group";
            else
                return null;
        }

        ///<summary>
        ///Translate columns name from native storage names to query engine names.
        ///Some times the name of column in native database it isn't the to query engine name, for example oracle doesn't support column name length more than 30 characters
        ///</summary>
        /// <MetaDataID>{f0c0797f-05ef-4a85-84f3-517e2731370d}</MetaDataID>
        private void UpdateDataTableColumnsNames()
        {
            foreach (DataNode dataNode in DataNode.SubDataNodes)
            {
                if ((dataNode.ParticipateInSelectClause || dataNode.ParticipateInWereClause) &&
                dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                dataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                {
                    if (LocalDataColumnExistFor(dataNode))
                    {
                        string columnName = GetLocalDataColumnName(dataNode);
                        IDataColumn column = Data.Columns[columnName];
                        if (column == null && DataNodeColumnsNames.ContainsKey(columnName))
                            column = Data.Columns[DataNodeColumnsNames[columnName]];
                        if (column != null)
                            column.ColumnName = MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(dataNode);
                    }
                }
                if (dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                dataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                {
                    foreach (DataNode subDataNode in dataNode.SubDataNodes)
                    {
                        if (subDataNode.ParticipateInSelectClause &&
                            subDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                            subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                        {
                            if (LocalDataColumnExistFor(dataNode))
                            {
                                string columnName = GetLocalDataColumnName(subDataNode);
                                IDataColumn column = Data.Columns[columnName];
                                column.ColumnName = MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(subDataNode);
                            }
                        }

                    }
                }
            }
            foreach (DataLoader valueTypeDataLoader in ValueTypeDataLoaders)
            {

                foreach (DataNode dataNode in valueTypeDataLoader.DataNode.SubDataNodes)
                {
                    if (dataNode.ParticipateInSelectClause &&
                    dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                         dataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                    {
                        if (LocalDataColumnExistFor(dataNode))
                        {
                            string columnName = GetLocalDataColumnName(dataNode);
                            IDataColumn column = Data.Columns[columnName];
                            column.ColumnName = MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(dataNode);
                        }
                    }
                    if (dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                         dataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                    {
                        foreach (DataNode subDataNode in dataNode.SubDataNodes)
                        {
                            if (subDataNode.ParticipateInSelectClause &&
                                subDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                            {
                                if (LocalDataColumnExistFor(dataNode))
                                {
                                    string columnName = GetLocalDataColumnName(subDataNode);
                                    IDataColumn column = Data.Columns[columnName];
                                    column.ColumnName = MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(subDataNode);
                                }
                            }
                        }
                    }
                }
            }
            foreach (DataLoader hostedDataDataLoader in HostedDataDataLoaders)
            {
                foreach (DataNode dataNode in hostedDataDataLoader.DataNode.SubDataNodes)
                {
                    if (dataNode.ParticipateInSelectClause &&
                    dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                    dataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                    {
                        if (LocalDataColumnExistFor(dataNode))
                        {
                            string columnName = hostedDataDataLoader.GetLocalDataColumnName(dataNode);
                            IDataColumn column = Data.Columns[columnName];
                            column.ColumnName = MetaDataRepository.ObjectQueryLanguage.DataSource.GetDataTreeUniqueColumnName(dataNode);
                        }
                    }

                    if (dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                    dataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                    {
                        foreach (DataNode subDataNode in dataNode.SubDataNodes)
                        {
                            if (subDataNode.ParticipateInSelectClause &&
                                subDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                            {
                                if (LocalDataColumnExistFor(dataNode))
                                {
                                    string columnName = GetLocalDataColumnName(subDataNode);
                                    IDataColumn column = Data.Columns[columnName];
                                    column.ColumnName = MetaDataRepository.ObjectQueryLanguage.DataSource.GetDataTreeUniqueColumnName(subDataNode);
                                }
                            }
                        }
                    }
                }
                foreach (DataLoader valueTypeDataLoader in hostedDataDataLoader.ValueTypeDataLoaders)
                {

                    foreach (DataNode dataNode in valueTypeDataLoader.DataNode.SubDataNodes)
                    {
                        if (dataNode.ParticipateInSelectClause &&
                        dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                        dataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                        {
                            if (hostedDataDataLoader.LocalDataColumnExistFor(dataNode))
                            {
                                string columnName = hostedDataDataLoader.GetLocalDataColumnName(dataNode);
                                IDataColumn column = Data.Columns[columnName];
                                column.ColumnName = MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(dataNode);
                            }
                        }
                        if (dataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                     dataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                        {
                            foreach (DataNode subDataNode in dataNode.SubDataNodes)
                            {
                                if (subDataNode.ParticipateInSelectClause &&
                                    subDataNode.Type == DataNode.DataNodeType.OjectAttribute &&
                                    subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                                {
                                    if (LocalDataColumnExistFor(dataNode))
                                    {
                                        string columnName = GetLocalDataColumnName(subDataNode);
                                        IDataColumn column = Data.Columns[columnName];
                                        column.ColumnName = MetaDataRepository.ObjectQueryLanguage.DataSource.GetColumnName(subDataNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <MetaDataID>{2dd82378-7ca9-4f66-9e47-db82743044f3}</MetaDataID>
        ///<summary>
        /// The value of this property is true in case where there arenít 
        /// criterions that refer to the object of data loader 
        /// or all that criterions resolved locally. 
        ///</summary>
        internal protected abstract bool CriterionsForDataLoaderResolvedLocally(DataLoader dataLoader);

        /// <MetaDataID>{dc1d84fd-c8bc-4943-b0ff-881de83b8a59}</MetaDataID>
        /// <summary>
        /// This method returns the column name which uses the data loader to retrieve data node data from data management system (DMS). 
        /// There are cases where column name for data retrieve is different from the column name which uses data loader to publish data. 
        /// </summary>
        abstract public string GetLocalDataColumnName(DataNode dataNode);


        /// <MetaDataID>{6e086536-4145-4d3c-8716-47bcf3cc18ac}</MetaDataID>
        /// <MetaDataID>{4e7e2ed8-76b2-4745-aad0-28d8d2f481e5}</MetaDataID>
        /// <summary>
        /// <MetaDataID>{658706f2-c85d-4b0e-b587-5d4cb6037116}</MetaDataID>
        /// Returns data loader for data node
        /// </summary>
        protected abstract DataLoader GetDataLoader(DataNode dataNode);

        /// <MetaDataID>{80634031-be79-4b96-bbf9-1415b3278633}</MetaDataID>
        int? _ObjectIdentityColumnIndex;

        /// <MetaDataID>{95ce229f-4fc0-421b-a4e6-64b13fd17e09}</MetaDataID>
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
                        _ObjectIdentityColumnIndex = Data.Columns.IndexOf("@" + dataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object_Identity");
                    }
                    else
                    {
                        _ObjectIdentityColumnIndex = Data.Columns.IndexOf("@" + DataNode.ValueTypePathDiscription + "Object_Identity");
                    }
                }
                return _ObjectIdentityColumnIndex.Value;
            }
        }



        /// <exclude>Excluded</exclude>
        int? _ObjectColumnIndex;
        /// <MetaDataID>{3a3c32e0-3e45-410b-bf3f-53ca4abfcecb}</MetaDataID>
        /// <summary>
        /// Return column index for the objects in data loader table.
        /// </summary>
        public int ObjectColumnIndex
        {
            get
            {
                if (!_ObjectColumnIndex.HasValue)
                {
                    DataNode nonValueTypeDataNode = DataNode;
                    while (nonValueTypeDataNode.ValueTypePath.Count > 0)
                        nonValueTypeDataNode = nonValueTypeDataNode.RealParentDataNode;
                    if (DataNode.ValueTypePath.Count != 0 && nonValueTypeDataNode.DataSource != null && nonValueTypeDataNode.DataSource.DataTable != null && nonValueTypeDataNode.DataSource.DataLoadedInParentDataSource)
                    {
                        _ObjectColumnIndex = Data.Columns.IndexOf(nonValueTypeDataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object");
                    }
                    else if (DataLoadedInParentDataSource && DataNode.ValueTypePath.Count == 0)
                    {
                        DataNode dataNode = DataNode;
                        while (dataNode.Type == DataNode.DataNodeType.Object && dataNode.ValueTypePath.Count > 0)
                            dataNode = dataNode.RealParentDataNode;

                        _ObjectColumnIndex = Data.Columns.IndexOf(dataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "Object");
                    }
                    else
                    {
                        _ObjectColumnIndex = Data.Columns.IndexOf(DataNode.ValueTypePathDiscription + "Object");
                    }
                }
                return _ObjectColumnIndex.Value;

            }
        }

        /// <exclude>Excluded</exclude>
        int? _LoadObjectLinksIndex;
        /// <MetaDataID>{2a35dd58-8430-4931-a25a-ef8cd901b16d}</MetaDataID>
        /// <summary>
        /// In object row there is flag which tell if the object is already in active mode or not. 
        /// If it isnít then the LoadObjectLinks flag is true and system must load all prefetching relations.   
        /// This property defines the index of column with LoadObjectLinks flag.
        /// </summary>
        public int LoadObjectLinksIndex
        {
            get
            {
                if (!_LoadObjectLinksIndex.HasValue)
                {
                    DataNode nonValueTypeDataNode = DataNode;
                    while (nonValueTypeDataNode.ValueTypePath.Count > 0)
                        nonValueTypeDataNode = nonValueTypeDataNode.RealParentDataNode;
                    if (DataNode.ValueTypePath.Count != 0 && nonValueTypeDataNode.DataSource != null && nonValueTypeDataNode.DataSource.DataTable != null && nonValueTypeDataNode.DataSource.DataLoadedInParentDataSource)
                    {
                        _LoadObjectLinksIndex = Data.Columns.IndexOf(nonValueTypeDataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "LoadObjectLinks");
                    }
                    else if (DataLoadedInParentDataSource && DataNode.ValueTypePath.Count == 0)
                    {
                        DataNode dataNode = DataNode;
                        while (dataNode.Type == DataNode.DataNodeType.Object && dataNode.ValueTypePath.Count > 0)
                            dataNode = dataNode.RealParentDataNode;

                        _LoadObjectLinksIndex = Data.Columns.IndexOf(dataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "LoadObjectLinks");
                    }
                    else
                    {
                        _LoadObjectLinksIndex = Data.Columns.IndexOf(DataNode.ValueTypePathDiscription + "LoadObjectLinks");
                    }

                }
                return _LoadObjectLinksIndex.Value;
            }
        }
        ///// <exclude>Excluded</exclude>
        //int? _RowRemoveIndex;
        ///// <MetaDataID>{5be1f361-86d3-42dd-a09c-80ad96d4581c}</MetaDataID>
        //public int RowRemoveIndex
        //{
        //    get
        //    {
        //        if (!_RowRemoveIndex.HasValue)
        //            _RowRemoveIndex = Data.Columns.IndexOf(DataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "RowRemove");
        //        return _RowRemoveIndex.Value;
        //    }
        //}


        /// <MetaDataID>{6580E842-7065-497F-94BA-D1203AAD76B7}</MetaDataID>
        /// <summary>
        /// This method takes the state of object and returns the object and a flag which indicates where the system must load prefetching relation or not. 
        /// </summary>
        /// <param name="loadObjectLinks">
        /// Defines an output parameter which indicates if system must load prefetching relation or not.
        /// </param>
        /// <param name="row">
        /// Defines the state of object which retrieved from storage instance.
        /// <MetaDataID>{19bd6f94-2d84-4a48-9dd3-7d1c0fe4a8cd}</MetaDataID>
        /// </param>
        /// <returns>
        /// Returns the object 
        /// </returns> 
        public abstract object GetObject(PersistenceLayer.StorageInstanceRef.ObjectSate row, out bool loadObjectLinks);

        /// <MetaDataID>{d3f9d1cf-4ab9-4f29-9cab-60276cecedf9}</MetaDataID>
        public abstract object GetObjectIdentity(PersistenceLayer.StorageInstanceRef.ObjectSate row);

        /// <exclude>Excluded</exclude>
        protected bool _DataLoadingPrepared;

        /// <MetaDataID>{b16d9eab-4810-4d49-8770-911e5bcc7716}</MetaDataID>
        public bool DataLoadingPrepared
        {
            get
            {
                return _DataLoadingPrepared;
            }
        }

        /// <exclude>Excluded</exclude>
        protected bool _DataLoaded;

        /// <MetaDataID>{539137e8-188a-4761-9872-99b43f1997f7}</MetaDataID>
        public bool DataLoaded
        {
            get
            {
                return _DataLoaded;
            }
        }

        /// <MetaDataID>{1c71c7cb-5f90-48bc-9c1f-2d43992cd0c1}</MetaDataID>
        /// <summary>
        /// Inform data loader for object query change state from distribution state to data loading state. 
        /// </summary>
        protected virtual void OnPrepareForDataLoading()
        {

        }
        /// <MetaDataID>{18fe8e8c-e598-4b02-aacc-7891e8404d01}</MetaDataID>
        public void PrepareForDataLoading()
        {
            if (!DataLoadingPrepared)
            {
                OnPrepareForDataLoading();
                _DataLoadingPrepared = true;
            }

        }



        /// <MetaDataID>{b6bf9bce-b7c1-475a-a94a-e8026487e366}</MetaDataID>
        public abstract void UpdateObjectIdentityTypes(System.Collections.Generic.List<ObjectIdentityType> dataLoaderObjectIdentityTypes);

        /// <MetaDataID>{6d210148-7f6b-4a52-a74a-13cfb56d50fb}</MetaDataID>
        internal void ChangeDataNodeThrougthRelationTable(bool value)
        {
            DataNode.ThroughRelationTable = value;
        }

        ///<summary>
        ///This property define when data loader retrieves data from native storage system
        ///Some times the query engine needs data, which are usefull only for native storage system because the major piece of query resolved from native storage system.
        ///For example group by ,sums average etc
        ///</summary>
        /// <MetaDataID>{eb466466-4396-4f20-a159-b2d0599ba06a}</MetaDataID>
        public abstract bool RetrievesData
        {
            get;
        }
        /// <MetaDataID>{b8d8631b-449d-428a-a324-27579ffe85be}</MetaDataID>
        abstract internal void AggregateExpressionDataNodeResolved(AggregateExpressionDataNode aggregateDataNode);



    }





    /// <summary></summary>
    /// <MetaDataID>{fa219b42-0020-4b95-a501-80fbf36cb5a4}</MetaDataID>
    //// <MetaDataID>{ae8091e6-2136-47d9-9da0-3f54865d0d24}</MetaDataID>
    [Serializable]
    public class AverageValue
    {
        public decimal Average;
        public int AverageCount;
    }
}
