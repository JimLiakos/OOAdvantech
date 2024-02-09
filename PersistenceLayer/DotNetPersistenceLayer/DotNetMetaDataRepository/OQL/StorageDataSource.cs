using System;
using System.Linq;
using PartialRelationIdentity = System.String;
namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using StorageIdentity = System.String;
    using System.Collections.Generic;

    ///<summary>
    /// Defines a path with datanode as member 
    /// </summary>
    /// <MetaDataID>{a85a1bee-37ac-4a31-ac23-d9c1bf130844}</MetaDataID>
    class DataNodePath : System.Collections.Generic.Stack<DataNode>
    {
    }

    ///<summary>
    ///Class Storage Data Source manage data from Storage ObjectContext
    ///</summary>
    ///<MetaDataID>{e26b1927-7064-4784-aee8-85e06c4f73a3}</MetaDataID>
    [Serializable]
    public class StorageDataSource : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataSource, System.Runtime.Serialization.IDeserializationCallback
    {
        protected StorageDataSource(Guid identity)
            : base(identity)
        {

        }
        /// <summary>
        /// Clone StorageDataSource instance 
        /// </summary>
        /// <param name="clonedObjects">
        /// Defines cloned object dictionary
        /// </param>
        /// <returns>
        /// return new datasource instance
        /// </returns>
        protected internal override DataSource Clone(Dictionary<object, object> clonedObjects)
        {

            if (clonedObjects.ContainsKey(this))
                return clonedObjects[this] as DataSource;

            StorageDataSource newDataSource = new StorageDataSource(Identity);



            clonedObjects[this] = newDataSource;
            newDataSource._HasObjectsToActivate = new Member<bool>();
            newDataSource.DataLoaders = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoader>();
            newDataSource.DataLoadersMetadata = new Dictionary<string, DataLoaderMetadata>();

            object dataNode = null;
            if (clonedObjects.TryGetValue(_DataNode, out dataNode))
                newDataSource._DataNode = dataNode as DataNode;
            else
                newDataSource._DataNode = _DataNode.Clone(clonedObjects);

            Copy(newDataSource, clonedObjects);


            //newDataSource.DataLoadersMetadata = new Dictionary<StorageIdentity, DataLoaderMetadata>();
            //foreach (var dataLoadersMetadataEntry in DataLoadersMetadata)
            //    newDataSource.DataLoadersMetadata[dataLoadersMetadataEntry.Key] = dataLoadersMetadataEntry.Value.Clone(clonedObjects);


            newDataSource._HasObjectsToActivate = new Member<bool>();
            newDataSource.DataLoaders = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoader>();
            newDataSource.DataLoadersMetadata = new Dictionary<string, DataLoaderMetadata>();


            return newDataSource;
        }

        /// <MetaDataID>{48aa4876-e5db-49f9-998c-0923f4c845ae}</MetaDataID>
        internal static GlobalObjectID? GetGlobalObjectID(IDataRow row, IEnumerable<ObjectIdentityType> objectIdentityTypes)
        {
            return GetGlobalObjectID(row, objectIdentityTypes, "");
        }
        /// <MetaDataID>{dedd4cd4-b605-4bb3-9d28-4c2201dd8c26}</MetaDataID>
        internal static GlobalObjectID? GetGlobalObjectID(IDataRow row, ObjectIdentityType objectIdentityType)
        {
            return GetGlobalObjectID(row, objectIdentityType, "");
        }
        /// <MetaDataID>{f6efaf29-8bd8-49c8-884c-3e37634aba3c}</MetaDataID>
        internal static GlobalObjectID? GetGlobalObjectID(IDataRow row, IEnumerable<ObjectIdentityType> objectIdentityTypes, string columnPrefix)
        {
            if (row == null)
                return null;
            foreach (var objectIdentityType in objectIdentityTypes)
            {
                GlobalObjectID? globalObjectID = GetGlobalObjectID(row, objectIdentityType, columnPrefix);
                if (globalObjectID != null && globalObjectID.HasValue)
                    return globalObjectID;
            }
            return null;

        }

        /// <MetaDataID>{da67b7e2-7388-4338-8cea-5cec34ecc76f}</MetaDataID>
        internal static GlobalObjectID? GetGlobalObjectID(IDataRow row, ObjectIdentityType objectIdentityType, string columnPrefix)
        {
            object[] partValues = new object[objectIdentityType.PartsCount];
            int i = 0;
            foreach (var identityPart in objectIdentityType.Parts)
            {
                object value = row[columnPrefix + identityPart.Name];
                if (value == null)
                    return null;
                partValues[i++] = value;
            }
            GlobalObjectID globalObjectID = new GlobalObjectID(partValues, objectIdentityType, (int)row["OSM_StorageIdentity"]);
            return globalObjectID;
        }

#if !DeviceDotNet
        /// <MetaDataID>{a13cb5a4-0652-4639-b2f4-4cff6d8f5755}</MetaDataID>
        protected StorageDataSource(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

            _LoadObjectLinksIndex = (Nullable<int>)info.GetValue("LoadObjectLinksIndex", typeof(Nullable<int>));
            _ObjectActivation = (Nullable<bool>)info.GetValue("ObjectActivation", typeof(Nullable<bool>));

        }

        /// <MetaDataID>{c198455b-e3f9-4f77-8628-47d1b3a033b1}</MetaDataID>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("LoadObjectLinksIndex", _LoadObjectLinksIndex);
            info.AddValue("ObjectActivation", _ObjectActivation);
        }
#endif


        /// <MetaDataID>{0abd15f6-943d-4712-84aa-40fc41d8b628}</MetaDataID>
        bool? _ObjectActivation;
        ///<summary>
        ///ObjectActivation specifies when query engine converts the storage instance data to activate object
        ///True means object activation
        ///</summary>
        /// <MetaDataID>{a335684b-c8e4-4068-9090-e188143b6800}</MetaDataID>
        public bool ObjectActivation
        {
            get
            {
                if (DataNode.Type != DataNode.DataNodeType.Object)
                    return false;
                if (DataNode.ValueTypePath.Count > 0 && DataNode.ParticipateInWereClause)
                    return true;
                else if (DataNode.Type == OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Group)
                    return false;
                else
                {
                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode recursiveSubDataNode = null;
                    foreach (OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.RealSubDataNodes)
                    {
                        if (subDataNode.Recursive)
                        {
                            recursiveSubDataNode = subDataNode;
                            break;
                        }
                    }
                    if (DataNode.ParticipateInSelectClause ||
                         // DataNode.ParticipateInAggregateFunction ||
                         (recursiveSubDataNode != null && recursiveSubDataNode.ParticipateInSelectClause))
                    {
                        return true;
                    }

                }
                return false;
            }
        }


        struct ValueTypeRow
        {
            public IDataRow DataRow;
            public DataNode RowDataNode;
            public ValueTypeRow(IDataRow dataRow, DataNode rowDataNode)
            {
                DataRow = dataRow;
                RowDataNode = rowDataNode;
            }
        }

        #region Load objects link in just activated objects

        /// <exclude>Excluded</exclude>
        int? _LoadObjectLinksIndex;
        ///<summary>
        ///Defines the index of LoadObjectLinks columns
        ///LoadObjectLinks column defines a flag in case where flag is true the previous state of object in datasource was passive.
        ///In this case query engine must load obects links for all predefined object class relations
        ///</summary>
        /// <MetaDataID>{2a35dd58-8430-4931-a25a-ef8cd901b16d}</MetaDataID>
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
                        _LoadObjectLinksIndex = DataTable.Columns.IndexOf(nonValueTypeDataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "LoadObjectLinks");
                    }
                    else
                        if (DataLoadedInParentDataSource && DataNode.ValueTypePath.Count == 0)
                    {
                        DataNode dataNode = DataNode;
                        while (dataNode.Type == DataNode.DataNodeType.Object && dataNode.ValueTypePath.Count > 0)
                            dataNode = dataNode.RealParentDataNode;

                        _LoadObjectLinksIndex = DataTable.Columns.IndexOf(dataNode.Alias + "_" + DataNode.ValueTypePathDiscription + "LoadObjectLinks");
                    }
                    else
                    {
                        _LoadObjectLinksIndex = DataTable.Columns.IndexOf(DataNode.ValueTypePathDiscription + "LoadObjectLinks");
                    }
                    //_LoadObjectLinksIndex = DataTable.Columns.IndexOf(DataNode.ValueTypePathDiscription + "LoadObjectLinks");
                }
                return _LoadObjectLinksIndex.Value;
            }
        }


        /// <summary>
        /// This method sets the association 
        /// fields of relate object to reproduce the objects link in memory. 
        /// System must be load the data sources before call this function. 
        /// </summary>
        /// <param name="subDataNode">
        /// This parameter defines the data node with the related objects.
        /// </param>
        /// <MetaDataID>{f8110e6e-d6ff-46f3-83d1-e332acd53d29}</MetaDataID>
        internal void LoadObjectRelationLinks(DataNode subDataNode)
        {

            if (DataNode.Classifier is MetaDataRepository.Structure)
                return;

            if (DataTable == null)
                return;

         
            if (subDataNode.ValueTypePath.Count > 0)
            {
                if ((subDataNode.DataSource as StorageDataSource).ObjectActivation && subDataNode.DataSource.ThereAreObjectsToActivate)
                {
                    #region Load objects links of ValueType structure. SubDataNode data are member of data node objects which contained by value
                    DataNodePath dataNodePath = new DataNodePath();
                    dataNodePath.Push(subDataNode);
                    LoadValueTypeObjectRelationLinks(dataNodePath);
                    #endregion

                }
            }
            else
            {
                if (ObjectActivation && (subDataNode.DataSource as StorageDataSource).ObjectActivation &&
                    ((subDataNode.ParentLoadsObjectsLinks && ThereAreObjectsToActivate) || (DataNode.IsThereBackOnConstructionRoute(subDataNode) && (subDataNode.DataSource as StorageDataSource).ThereAreObjectsToActivate)))
                {
                    string storageIdentity = ((DataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity;
                    if (!DataLoaders.ContainsKey(storageIdentity))
                        return;
                    StorageDataLoader dataLoader = DataLoaders[storageIdentity] as StorageDataLoader;
                    Guid subDataNodeRelationDataIdentity = subDataNode.Identity;
                    //To continue both DataNode and sub DataNode must retrieve objects which must be intitialize from the system
                    //this happens when the condition of if statement is true. If it is false method returns with no action.

                    #region Load objects links of data node object and subdatanode objects where data node and subdata node have relation through association


                    ///ObjectsLinks actual is table with two columns at list one column
                    ///for owner object wich system load field and collection with related object
                    ///and one column for related object. 
                    ///Only in case of inter storage objects links, the table contains columns for object identities
                    ///of owner and releted object

                    Dictionary<StorageIdentity, Dictionary<PartialRelationIdentity, IDataTable>> interStorageObjectsLinks = null;
                    Dictionary<PartialRelationIdentity, IDataTable> inStorageObjectsLinks = null;
                    Dictionary<PartialRelationIdentity, IDataTable> inStorageOppositeDirectionObjectsLinks = null;

                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd
                        && (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Navigable &&
                        !(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Multiplicity.IsMany)
                    {
                        //Opposite direction subDataNode parent DataNode zero or one multiplicity  objectsLinks 
                        inStorageOppositeDirectionObjectsLinks = new Dictionary<string, IDataTable>();
                    }
                    if (subDataNode.ThroughRelationTable && subDataNode.DataSource.HasOutObjectContextData)
                        interStorageObjectsLinks = new Dictionary<string, Dictionary<PartialRelationIdentity, IDataTable>>();

                    foreach (IDataRow row in DataTable.Rows)
                    {
                        bool loadObjectLinks = ((bool)row[LoadObjectLinksIndex]);
                        object nodeObject = null;
                        if (!loadObjectLinks)
                        {
                            nodeObject = row[ObjectIndex];
                            if (nodeObject != null && !(nodeObject is System.DBNull))
                            {
                                if (!(Classifier.GetClassifier(nodeObject.GetType()) as Class).IsLazyFetching((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd)))
                                    loadObjectLinks = !PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(nodeObject).IsRelationLoaded(subDataNode.AssignedMetaObject as AssociationEnd);
                            }
                        }

                        if (subDataNode.AssignedMetaObject is AssociationEnd && (subDataNode.AssignedMetaObject as AssociationEnd).Indexer)
                        {

                        }

                        if (loadObjectLinks)
                        {
                            if (nodeObject == null)
                                nodeObject = row[ObjectIndex];
                            if (inStorageObjectsLinks == null)
                                inStorageObjectsLinks = new Dictionary<PartialRelationIdentity, IDataTable>();
                            bool loadEmpty = true;
                      
                            if (interStorageObjectsLinks != null)
                            {
                                #region Retrieves in storage and out storage objects links
                                foreach (var partialRelationRows in DataNode.DataSource.GetRelationRowsPartial(row, subDataNode))
                                {
                                    IDataTable inStorageObjectsLinkTable = null;
                                    IDataTable inStorageOppositeDirectionObjectsLinkTable = null;
                                    foreach (ObjectsLinkRows relationRow in partialRelationRows.Value)
                                    {
                                        IDataRow relatedObjectRow = relationRow.RelatedObjectRow;
                                        loadEmpty = false;
                                        object subNodeObject = null;
                                        if (relatedObjectRow != null)
                                            subNodeObject = relatedObjectRow[subDataNode.DataSource.ObjectIndex];

                                        int sortIndex = 0;
                                        if (subDataNode.AssignedMetaObject is AssociationEnd && (subDataNode.AssignedMetaObject as AssociationEnd).Indexer && relatedObjectRow[subDataNode.DataSource.DataSourceRelationsIndexerColumnName] is int)
                                            sortIndex = (int)relatedObjectRow[subDataNode.DataSource.DataSourceRelationsIndexerColumnName];


                                        if (subNodeObject is DBNull)
                                            subNodeObject = null;
                                        string subNodeStorageIdentity = null;
                                        if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                            subNodeStorageIdentity = (DataNode.ObjectQuery as DistributedObjectQuery).QueryStorageIdentities[(int)relationRow.RelationDataRow["RoleAStorageIdentity"]];
                                        else
                                            subNodeStorageIdentity = (DataNode.ObjectQuery as DistributedObjectQuery).QueryStorageIdentities[(int)relationRow.RelationDataRow["RoleBStorageIdentity"]];

                                        if (storageIdentity != subNodeStorageIdentity)
                                        {
                                            /// current row define a relation with out storage object
                                            if (!interStorageObjectsLinks.ContainsKey(subNodeStorageIdentity))
                                                interStorageObjectsLinks[subNodeStorageIdentity] = new Dictionary<PartialRelationIdentity, IDataTable>();

                                            IDataTable objectsLinkTable = null;
                                            if (!interStorageObjectsLinks[subNodeStorageIdentity].TryGetValue(partialRelationRows.Key, out objectsLinkTable))
                                            {
                                                #region Creates objectsLink table for interstorage links
                                                objectsLinkTable = DataObjectsInstantiator.CreateDataTable();
                                                objectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                                objectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                                objectsLinkTable.Columns.Add("sortIndex", typeof(int));
                                                foreach (var partialRelationData in RelationshipsData[subDataNodeRelationDataIdentity].RelationsData)
                                                {
                                                    string relationName = partialRelationData.RelationName;
                                                    if (partialRelationRows.Key == partialRelationData.RelationPartIdentity)
                                                    {
                                                        foreach (IDataColumn column in RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.Data.Columns)
                                                            if (column.ColumnName != "RoleAStorageIdentity" && column.ColumnName != "RoleBStorageIdentity")
                                                                objectsLinkTable.Columns.Add(column.ColumnName, column.DataType);
                                                    }
                                                }
                                                #endregion
                                                interStorageObjectsLinks[subNodeStorageIdentity][partialRelationRows.Key] = objectsLinkTable;
                                            }

                                            IDataRow newRow = objectsLinkTable.NewRow();
                                            ///For inter storage 
                                            newRow["OwnerObject"] = subNodeObject;
                                            newRow["RelatedObject"] = nodeObject;
                                            newRow["sortIndex"] = sortIndex;
                                            
                                            #region Load object identities columns with values
                                            foreach (IDataColumn column in objectsLinkTable.Columns)
                                            {
                                                if (column.ColumnName != "OwnerObject" && column.ColumnName != "RelatedObject" && column.ColumnName != "sortIndex")
                                                    newRow[column.ColumnName] = relationRow.RelationDataRow[column.ColumnName];
                                            }
                                            #endregion
                                            objectsLinkTable.Rows.Add(newRow);
                                        }
                                        else
                                        {
                                            if (inStorageOppositeDirectionObjectsLinks != null) // in Storage OppositeDirection ObjectsLinks used only for in storage links
                                            {

                                                if (inStorageOppositeDirectionObjectsLinkTable == null && !inStorageOppositeDirectionObjectsLinks.TryGetValue(partialRelationRows.Key, out inStorageOppositeDirectionObjectsLinkTable))
                                                {
                                                    inStorageOppositeDirectionObjectsLinkTable = DataObjectsInstantiator.CreateDataTable();
                                                    inStorageOppositeDirectionObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                                    inStorageOppositeDirectionObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                                    inStorageOppositeDirectionObjectsLinkTable.Columns.Add("sortIndex", typeof(int));
                                                    inStorageOppositeDirectionObjectsLinks[partialRelationRows.Key] = inStorageOppositeDirectionObjectsLinkTable;
                                                }
                                                if ((bool)relatedObjectRow[(subDataNode.DataSource as StorageDataSource).LoadObjectLinksIndex])
                                                    inStorageOppositeDirectionObjectsLinkTable.Rows.Add(subNodeObject, nodeObject);
                                            }
                                            if (relatedObjectRow != null)
                                            {
                                                if (inStorageObjectsLinkTable == null && !inStorageObjectsLinks.TryGetValue(partialRelationRows.Key, out inStorageObjectsLinkTable))
                                                {
                                                    inStorageObjectsLinkTable = DataObjectsInstantiator.CreateDataTable();
                                                    inStorageObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                                    inStorageObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                                    inStorageObjectsLinkTable.Columns.Add("sortIndex", typeof(int));
                                                    inStorageObjectsLinks[partialRelationRows.Key] = inStorageObjectsLinkTable;
                                                }
                                                inStorageObjectsLinkTable.Rows.Add(nodeObject, subNodeObject, sortIndex);
                                            }
                                        }
                                    }
                                    if (loadEmpty && !subDataNode.BranchParticipateInWereClause)
                                    {
                                        if (inStorageObjectsLinkTable == null && !inStorageObjectsLinks.TryGetValue(subDataNode.AssignedMetaObjectIdenty.ToString(), out inStorageObjectsLinkTable))
                                        {
                                            inStorageObjectsLinkTable = DataObjectsInstantiator.CreateDataTable();
                                            inStorageObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                            inStorageObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                            inStorageObjectsLinkTable.Columns.Add("sortIndex", typeof(int));
                                        }
                                        inStorageObjectsLinkTable.Rows.Add(nodeObject, null,0);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region Retrieves in storage objects links
                                foreach (var partialRelationRows in DataNode.DataSource.GetRelatedRowsPartial(row, subDataNode))
                                {

                                    if (!inStorageObjectsLinks.TryGetValue(partialRelationRows.Key, out IDataTable inStorageObjectsLinkTable))
                                    {
                                        inStorageObjectsLinkTable = DataObjectsInstantiator.CreateDataTable();
                                        inStorageObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                        inStorageObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                        inStorageObjectsLinkTable.Columns.Add("sortIndex", typeof(int));
                                        inStorageObjectsLinks[partialRelationRows.Key] = inStorageObjectsLinkTable;
                                    }
                                    foreach (IDataRow relatedRow in partialRelationRows.Value)
                                    {

                                        int sortIndex = 0;
                                        if (subDataNode.AssignedMetaObject is AssociationEnd && (subDataNode.AssignedMetaObject as AssociationEnd).Indexer&& relatedRow[subDataNode.DataSource.DataSourceRelationsIndexerColumnName] is int)
                                            sortIndex =(int)relatedRow[subDataNode.DataSource.DataSourceRelationsIndexerColumnName];
                                        
                                        loadEmpty = false;
                                        object subNodeObject = null;
                                        subNodeObject = relatedRow[subDataNode.DataSource.ObjectIndex];
                                        if (subNodeObject is DBNull)
                                            subNodeObject = null;
                                        if (inStorageOppositeDirectionObjectsLinks != null)
                                        {
                                            if (!inStorageOppositeDirectionObjectsLinks.TryGetValue(partialRelationRows.Key, out IDataTable inStorageOppositeDirectionObjectsLinkTable))
                                            {
                                                inStorageOppositeDirectionObjectsLinkTable = DataObjectsInstantiator.CreateDataTable(false);
                                                inStorageOppositeDirectionObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                                inStorageOppositeDirectionObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                                inStorageOppositeDirectionObjectsLinkTable.Columns.Add("sortIndex", typeof(int));

                                                inStorageOppositeDirectionObjectsLinks[partialRelationRows.Key] = inStorageOppositeDirectionObjectsLinkTable;
                                            }
                                            if ((bool)relatedRow[(subDataNode.DataSource as StorageDataSource).LoadObjectLinksIndex])
                                                inStorageOppositeDirectionObjectsLinkTable.Rows.Add(subNodeObject, nodeObject,sortIndex);
                                            else
                                            {
                                                if(!PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(subNodeObject).IsRelationLoaded((subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd()))
                                                    inStorageOppositeDirectionObjectsLinkTable.Rows.Add(subNodeObject, nodeObject, sortIndex);
                                            }
                                        }
                                        inStorageObjectsLinkTable.Rows.Add(nodeObject, subNodeObject, sortIndex);
                                    }

                                    if (loadEmpty && !subDataNode.BranchParticipateInWereClause)
                                        inStorageObjectsLinkTable.Rows.Add(nodeObject, null,0);
                                }
                                #endregion
                            }

                        }
                    }


                    if (inStorageOppositeDirectionObjectsLinks != null)
                    {
                        StorageDataLoader subDataNodeDataLoader = subDataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader;
                        subDataNodeDataLoader.LoadObjectRelationLinks((subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Identity, inStorageOppositeDirectionObjectsLinks);
                    }

                    if (interStorageObjectsLinks != null)
                    {
                        foreach (KeyValuePair<StorageIdentity, Dictionary<PartialRelationIdentity, IDataTable>> entry in interStorageObjectsLinks)
                        {
                            DistributedObjectQuery distributedObjectQuery = (DataNode.ObjectQuery as DistributedObjectQuery).DistributedObjectQueries[entry.Key];
                            Dictionary<string, IDataTable> objectsLink = null;
                            if ((subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Navigable)
                            {
                                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(distributedObjectQuery))
                                {
                                    var serializeTables = new Dictionary<PartialRelationIdentity, DataLoader.StreamedTable>();
                                    foreach (var dataTableEntry in entry.Value)
                                        serializeTables[dataTableEntry.Key] = dataTableEntry.Value.SerializeTable();
                                    objectsLink = new Dictionary<PartialRelationIdentity, IDataTable>();
                                    foreach (var dataTableEntry in distributedObjectQuery.LoadParentSubDataNodeObjectRelationLinks(subDataNode.Identity, serializeTables, RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.DetailDataSourceReferenceObjectIdentityTypes))
                                        objectsLink[dataTableEntry.Key] = DataObjectsInstantiator.CreateDataTable(dataTableEntry.Value);
                                }
                                else
                                {
                                    objectsLink = distributedObjectQuery.LoadParentSubDataNodeObjectRelationLinks(subDataNode.Identity, entry.Value, RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.DetailDataSourceReferenceObjectIdentityTypes);
                                }
                            }
                            else
                            {
                                if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(distributedObjectQuery))
                                {
                                    var serializeTables = new Dictionary<PartialRelationIdentity, DataLoader.StreamedTable>();
                                    foreach (var dataTableEntry in entry.Value)
                                        serializeTables[dataTableEntry.Key] = dataTableEntry.Value.SerializeTable();
                                    objectsLink = new Dictionary<PartialRelationIdentity, IDataTable>();

                                    foreach (var dataTableEntry in distributedObjectQuery.GetParentSubDataNodeObjectRelationLinks(subDataNode.Identity, serializeTables, RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.DetailDataSourceReferenceObjectIdentityTypes))
                                        objectsLink[dataTableEntry.Key] = DataObjectsInstantiator.CreateDataTable(dataTableEntry.Value);
                                }
                                else
                                    objectsLink = distributedObjectQuery.GetParentSubDataNodeObjectRelationLinks(subDataNode.Identity, entry.Value, RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.DetailDataSourceReferenceObjectIdentityTypes);
                            }
                            dataLoader.LoadObjectRelationLinks(subDataNode.AssignedMetaObjectIdenty, objectsLink);
                        }
                    }
                    if (inStorageObjectsLinks != null)
                        dataLoader.LoadObjectRelationLinks(subDataNode.AssignedMetaObjectIdenty, inStorageObjectsLinks);

                    #endregion
                }

            }

            #region Old replaced code

            ///// Datanode 
            //if (!(DataNode.DataSource as StorageDataSource).ObjectActivation && subDataNode.ValueTypePath.Count==0)
            //    return;

            //if (!(DataNode.DataSource as StorageDataSource).ObjectActivation &&
            //    subDataNode.ValueTypePath.Count > 0 && 
            //    !(subDataNode.DataSource as StorageDataSource).ObjectActivation)
            //    return;


            // var tert = subDataNode.DataSource.ThereAreObjectsToActivate;

            // if ((ThereAreObjectsToActivate&&(subDataNode.DataSource as StorageDataSource).ObjectActivation && subDataNode.LoadObjectRelationLinksWithParent)||
            //     ((subDataNode.DataSource as StorageDataSource).ObjectActivation&&DataNode.IsThereBackOnConstructionRoute(subDataNode)&&subDataNode.DataSource.ThereAreObjectsToActivate))
            // {

            // }

            //if ((DataNode.Type == DataNode.DataNodeType.Object &&
            //   subDataNode.Type == DataNode.DataNodeType.Object &&
            //   DataNode.LoadObjectRelationLinksWithParent &&
            //   subDataNode.LoadObjectRelationLinksWithParent &&
            //   ThereAreObjectsToActivate) ||
            //   ((subDataNode.LoadObjectRelationLinksWithParent || DataNode.IsThereBackOnConstructionRoute(subDataNode)) &&
            //    subDataNode.DataSource.ThereAreObjectsToActivate) ||
            //    // For value type relation whithout owner object activation
            //   (DataNode.Type == DataNode.DataNodeType.Object &&
            //   subDataNode.Type == DataNode.DataNodeType.Object &&
            //   subDataNode.DataSource.ThereAreObjectsToActivate &&
            //   subDataNode.ValueTypePath.Count > 0 &&
            //   !(DataNode.DataSource as StorageDataSource).ObjectActivation &&
            //   (subDataNode.DataSource as StorageDataSource).ObjectActivation))
            //{
            //    string storageIdentity = ((DataNode.ObjectQuery as DistributedObjectQuery).ObjectStorage as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity;
            //    if (!DataLoaders.ContainsKey(storageIdentity))
            //        return;
            //    StorageDataLoader dataLoader = DataLoaders[storageIdentity] as StorageDataLoader;

            //    Guid subDataNodeRelationDataIdentity = subDataNode.Identity;
            //    //To continue both DataNode and sub DataNode must retrieve objects which must be intitialize from the system
            //    //this happens when the condition of if statement is true. If it is false method returns with no action.


            //    if (subDataNode.ValueTypePath.Count > 0)
            //    {
            //        #region Load objects links of ValueType structure. SubDataNode data are member of data node objects which contained by value
            //        DataNodePath dataNodePath = new DataNodePath();
            //        dataNodePath.Push(subDataNode);
            //        LoadValueTypeObjectRelationLinks(dataNodePath);
            //        #endregion

            //    }
            //    else
            //    {
            //        #region Load objects links of data node object and subdatanode objects where data node and subdata node have relation through association


            //        ///ObjectsLinks actual is table with two columns at list one column
            //        ///for owner object wich system load field and collection with related object
            //        ///and one column for related object. 
            //        ///Only in case of inter storage objects links, the table contains columns for object identities
            //        ///of owner and releted object

            //        Dictionary<StorageIdentity, Dictionary<PartialRelationIdentity, DataLoader.DataTable>> interStorageObjectsLinks = null;
            //        Dictionary<PartialRelationIdentity, DataLoader.DataTable> inStorageObjectsLinks = null;
            //        Dictionary<PartialRelationIdentity, DataLoader.DataTable> inStorageOppositeDirectionObjectsLinks = null;

            //        if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd
            //            && (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Navigable &&
            //            !(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Multiplicity.IsMany)
            //        {
            //            inStorageOppositeDirectionObjectsLinks = new Dictionary<string, DataLoader.DataTable>();
            //        }
            //        if (subDataNode.ThroughRelationTable && subDataNode.DataSource.HasOutObjectContextData)
            //            interStorageObjectsLinks = new Dictionary<string, Dictionary<PartialRelationIdentity, DataLoader.DataTable>>();

            //        foreach (System.Data.DataRow row in DataTable.Rows)
            //        {
            //            bool loadObjectLinks = ((bool)row[LoadObjectLinksIndex]);
            //            object nodeObject = null;
            //            if (!loadObjectLinks)
            //            {
            //                nodeObject = row[ObjectIndex];
            //                if (nodeObject != null)
            //                {
            //                    if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).LazyFetching)
            //                        loadObjectLinks = !OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(nodeObject).IsRelationLoaded(subDataNode.AssignedMetaObject as AssociationEnd);
            //                }
            //            }
            //            if (loadObjectLinks)
            //            {
            //                if (nodeObject == null)
            //                    nodeObject = row[ObjectIndex];
            //                if (inStorageObjectsLinks == null)
            //                    inStorageObjectsLinks = new Dictionary<PartialRelationIdentity, DataLoader.DataTable>();
            //                bool loadEmpty = true;
            //                if (interStorageObjectsLinks != null)
            //                {
            //                    #region Retrieves in storage and out storage objects links 
            //                    foreach (var partialRelationRows in DataNode.DataSource.GetRelationRowsPartial(row, subDataNode))
            //                    {
            //                        DataLoader.DataTable inStorageObjectsLinkTable = null;
            //                        DataLoader.DataTable inStorageOppositeDirectionObjectsLinkTable = null;
            //                        foreach (ObjectsLinkRows relationRow in partialRelationRows.Value)
            //                        {
            //                            System.Data.DataRow relatedObjectRow = relationRow.RelatedObjectRow;
            //                            loadEmpty = false;
            //                            object subNodeObject = null;
            //                            if (relatedObjectRow != null)
            //                                subNodeObject = relatedObjectRow[subDataNode.DataSource.ObjectIndex];
            //                            if (subNodeObject is DBNull)
            //                                subNodeObject = null;
            //                            string subNodeStorageIdentity = null;
            //                            if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
            //                                subNodeStorageIdentity = (DataNode.ObjectQuery as DistributedObjectQuery).QueryStorageIdentities[(int)relationRow.RelationDataRow["RoleAStorageIdentity"]];
            //                            else
            //                                subNodeStorageIdentity = (DataNode.ObjectQuery as DistributedObjectQuery).QueryStorageIdentities[(int)relationRow.RelationDataRow["RoleBStorageIdentity"]];

            //                            if (storageIdentity != subNodeStorageIdentity) 
            //                            {
            //                                /// current row define a relation with out storage object
            //                                if (!interStorageObjectsLinks.ContainsKey(subNodeStorageIdentity))
            //                                    interStorageObjectsLinks[subNodeStorageIdentity] = new Dictionary<PartialRelationIdentity, DataLoader.DataTable>();

            //                                DataLoader.DataTable objectsLinkTable = null;
            //                                if (!interStorageObjectsLinks[subNodeStorageIdentity].TryGetValue(partialRelationRows.Key, out objectsLinkTable))
            //                                {
            //                                    #region Creates objectsLink table for interstorage links
            //                                    objectsLinkTable = new DataLoader.DataTable(false);
            //                                    objectsLinkTable.Columns.Add("OwnerObject", typeof(object));
            //                                    objectsLinkTable.Columns.Add("RelatedObject", typeof(object));
            //                                    foreach (var partialRelationData in RelationshipsData[subDataNodeRelationDataIdentity].RelationsData)
            //                                    {
            //                                        string relationName = partialRelationData.RelationName;
            //                                        if (partialRelationRows.Key == partialRelationData.RelationPartIdentity)
            //                                        {
            //                                            foreach (System.Data.DataColumn column in RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.Data.Columns)
            //                                                if (column.ColumnName != "RoleAStorageIdentity" && column.ColumnName != "RoleBStorageIdentity")
            //                                                    objectsLinkTable.Columns.Add(column.ColumnName, column.DataType);
            //                                        }
            //                                    }
            //                                    #endregion
            //                                    interStorageObjectsLinks[subNodeStorageIdentity][partialRelationRows.Key] = objectsLinkTable;
            //                                }

            //                                System.Data.DataRow newRow = objectsLinkTable.NewRow();
            //                                ///For inter storage 
            //                                newRow["OwnerObject"] = subNodeObject;
            //                                newRow["RelatedObject"] = nodeObject;
            //                                #region Load object identities columns with values
            //                                foreach (System.Data.DataColumn column in objectsLinkTable.Columns)
            //                                {
            //                                    if (column.ColumnName != "OwnerObject" && column.ColumnName != "RelatedObject")
            //                                        newRow[column.ColumnName] = relationRow.RelationDataRow[column.ColumnName];
            //                                }
            //                                #endregion
            //                                objectsLinkTable.Rows.Add(newRow);
            //                            }
            //                            else
            //                            {
            //                                if (inStorageOppositeDirectionObjectsLinks != null) // in Storage OppositeDirection ObjectsLinks used only for in storage links
            //                                {

            //                                    if (inStorageOppositeDirectionObjectsLinkTable == null && !inStorageOppositeDirectionObjectsLinks.TryGetValue(partialRelationRows.Key, out inStorageOppositeDirectionObjectsLinkTable))
            //                                    {
            //                                        inStorageOppositeDirectionObjectsLinkTable = new DataLoader.DataTable(false);
            //                                        inStorageOppositeDirectionObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
            //                                        inStorageOppositeDirectionObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
            //                                        inStorageOppositeDirectionObjectsLinks[partialRelationRows.Key] = inStorageOppositeDirectionObjectsLinkTable;
            //                                    }
            //                                    if ((bool)relatedObjectRow[(subDataNode.DataSource as StorageDataSource).LoadObjectLinksIndex])
            //                                        inStorageOppositeDirectionObjectsLinkTable.Rows.Add(subNodeObject, nodeObject);
            //                                }
            //                                if (relatedObjectRow != null)
            //                                {
            //                                    if (inStorageObjectsLinkTable == null && !inStorageObjectsLinks.TryGetValue(partialRelationRows.Key, out inStorageObjectsLinkTable))
            //                                    {
            //                                        inStorageObjectsLinkTable = new DataLoader.DataTable(false);
            //                                        inStorageObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
            //                                        inStorageObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
            //                                        inStorageObjectsLinks[partialRelationRows.Key] = inStorageObjectsLinkTable;
            //                                    }
            //                                    inStorageObjectsLinkTable.Rows.Add(nodeObject, subNodeObject);
            //                                }
            //                            }
            //                        }
            //                        if (loadEmpty && !subDataNode.BranchParticipateInWereClause)
            //                        {
            //                            if (inStorageObjectsLinkTable == null && !inStorageObjectsLinks.TryGetValue(subDataNode.AssignedMetaObjectIdenty.ToString(), out inStorageObjectsLinkTable))
            //                            {
            //                                inStorageObjectsLinkTable = new DataLoader.DataTable(false);
            //                                inStorageObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
            //                                inStorageObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
            //                            }
            //                            inStorageObjectsLinkTable.Rows.Add(nodeObject, null);
            //                        }
            //                    }
            //                    #endregion
            //                }
            //                else
            //                {
            //                    #region Retrieves in storage objects links
            //                    foreach (var partialRelationRows in DataNode.DataSource.GetRelatedRowsPartial(row, subDataNode))
            //                    {
            //                        DataLoader.DataTable inStorageObjectsLinkTable = null;
            //                        if (!inStorageObjectsLinks.TryGetValue(partialRelationRows.Key, out inStorageObjectsLinkTable))
            //                        {
            //                            inStorageObjectsLinkTable = new DataLoader.DataTable(false);
            //                            inStorageObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
            //                            inStorageObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
            //                            inStorageObjectsLinks[partialRelationRows.Key] = inStorageObjectsLinkTable;
            //                        }
            //                        foreach (System.Data.DataRow relatedRow in partialRelationRows.Value)
            //                        {
            //                            loadEmpty = false;
            //                            object subNodeObject = null;
            //                            subNodeObject = relatedRow[subDataNode.DataSource.ObjectIndex];
            //                            if (subNodeObject is DBNull)
            //                                subNodeObject = null;
            //                            if (inStorageOppositeDirectionObjectsLinks != null)
            //                            {
            //                                DataLoader.DataTable inStorageOppositeDirectionObjectsLinkTable = null;
            //                                if (!inStorageOppositeDirectionObjectsLinks.TryGetValue(partialRelationRows.Key, out inStorageOppositeDirectionObjectsLinkTable))
            //                                {
            //                                    inStorageOppositeDirectionObjectsLinkTable = new DataLoader.DataTable(false);
            //                                    inStorageOppositeDirectionObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
            //                                    inStorageOppositeDirectionObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
            //                                    inStorageOppositeDirectionObjectsLinks[partialRelationRows.Key] = inStorageOppositeDirectionObjectsLinkTable;
            //                                }
            //                                if ((bool)relatedRow[(subDataNode.DataSource as StorageDataSource).LoadObjectLinksIndex])
            //                                    inStorageOppositeDirectionObjectsLinkTable.Rows.Add(subNodeObject, nodeObject);
            //                            }
            //                            inStorageObjectsLinkTable.Rows.Add(nodeObject, subNodeObject);
            //                        }

            //                        if (loadEmpty && !subDataNode.BranchParticipateInWereClause)
            //                            inStorageObjectsLinkTable.Rows.Add(nodeObject, null);
            //                    }
            //                    #endregion
            //                }

            //            }
            //        }


            //        if (inStorageOppositeDirectionObjectsLinks != null)
            //        {
            //            StorageDataLoader subDataNodeDataLoader = subDataNode.DataSource.DataLoaders[storageIdentity] as StorageDataLoader;
            //            subDataNodeDataLoader.LoadObjectRelationLinks((subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Identity, inStorageOppositeDirectionObjectsLinks);
            //        }

            //        if (interStorageObjectsLinks != null)
            //        {
            //            foreach (KeyValuePair<StorageIdentity, Dictionary<PartialRelationIdentity, DataLoader.DataTable>> entry in interStorageObjectsLinks)
            //            {
            //                DistributedObjectQuery distributedObjectQuery = (DataNode.ObjectQuery as DistributedObjectQuery).DistributedObjectQueries[entry.Key];
            //                Dictionary<string, DataLoader.DataTable> objectsLink = null;
            //                if ((subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Navigable)
            //                {
            //                    if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(distributedObjectQuery))
            //                    {
            //                        var serializeTables = new Dictionary<PartialRelationIdentity, DataLoader.StreamedTable>();
            //                        foreach (var dataTableEntry in entry.Value)
            //                            serializeTables[dataTableEntry.Key] = dataTableEntry.Value.SerializeTable();
            //                        objectsLink = new Dictionary<PartialRelationIdentity, DataLoader.DataTable>();
            //                        foreach (var dataTableEntry in distributedObjectQuery.LoadParentSubDataNodeObjectRelationLinks(subDataNode.Identity, serializeTables, RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.SubNodeDataSourceReferenceObjectIdentityTypes))
            //                            objectsLink[dataTableEntry.Key] = new DataLoader.DataTable(dataTableEntry.Value);
            //                    }
            //                    else
            //                    {
            //                        objectsLink = distributedObjectQuery.LoadParentSubDataNodeObjectRelationLinks(subDataNode.Identity, entry.Value, RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.SubNodeDataSourceReferenceObjectIdentityTypes);
            //                    }
            //                }
            //                else
            //                {
            //                    if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(distributedObjectQuery))
            //                    {
            //                        var serializeTables = new Dictionary<PartialRelationIdentity, DataLoader.StreamedTable>();
            //                        foreach (var dataTableEntry in entry.Value)
            //                            serializeTables[dataTableEntry.Key] = dataTableEntry.Value.SerializeTable();
            //                        objectsLink = new Dictionary<PartialRelationIdentity, DataLoader.DataTable>();

            //                        foreach (var dataTableEntry in distributedObjectQuery.GetParentSubDataNodeObjectRelationLinks(subDataNode.Identity, serializeTables, RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.SubNodeDataSourceReferenceObjectIdentityTypes))
            //                            objectsLink[dataTableEntry.Key] = new DataLoader.DataTable(dataTableEntry.Value);
            //                    }
            //                    else
            //                        objectsLink = distributedObjectQuery.GetParentSubDataNodeObjectRelationLinks(subDataNode.Identity, entry.Value, RelationshipsData[subDataNode.Identity].AssotiationTableRelationshipData.SubNodeDataSourceReferenceObjectIdentityTypes);
            //                }
            //                dataLoader.LoadObjectRelationLinks(subDataNode.AssignedMetaObjectIdenty, objectsLink);
            //            }
            //        }
            //        if (inStorageObjectsLinks != null)
            //            dataLoader.LoadObjectRelationLinks(subDataNode.AssignedMetaObjectIdenty, inStorageObjectsLinks);

            //        #endregion
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// walk recursive through data node to reach to the first data node where there is relation with parent through classes assotiation then
        /// load objects links on memory
        /// </summary>
        /// <param name="dataNodePath">
        /// Defines the path from non value type data node to data node which has relation with parent through AssociationEnd  
        /// </param>
        /// <MetaDataID>{75a6c48d-0e88-4c44-952d-72bbdd0ebd4d}</MetaDataID>
        void LoadValueTypeObjectRelationLinks(DataNodePath dataNodePath)
        {
            DataNode dataNode = dataNodePath.Peek();
            if (dataNode.ValueTypePath.Count > 0)
            {
                #region Walk recursive through data node to reach to the first data node with assotiation with object reference type
                //and build datanode path
                foreach (DataNode subDataNode in dataNode.SubDataNodes)
                {
                    if (subDataNode.Type != DataNode.DataNodeType.Object)
                        continue;
                    dataNodePath.Push(subDataNode);
                    try
                    {
                        LoadValueTypeObjectRelationLinks(dataNodePath);
                    }
                    finally
                    {
                        dataNodePath.Pop();
                    }
                }
                #endregion
            }
            else
            {
                DataNode relationDataNode = dataNodePath.Peek();
                if (relationDataNode.ParentLoadsObjectsLinks)
                {
                    string storageIdentity = ((DataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as PersistenceLayer.ObjectStorage).StorageMetaData.StorageIdentity;
                    Guid subDataNodeRelationDataIdentity = relationDataNode.Identity;
                    //The system reach to the data node with assotiation between value type and  object reference type. 
                    //foreach dataLoader loads the objects of root datanode and the related objects throught the path, in a table and
                    //pass the table and path to the dataloder to load object links in memory 

                    //The data node objects links grouped by storage of  data node objects

                    MetaDataRepository.ValueTypePath valueTypePath = new ValueTypePath();
                    #region Creates the value type path
                    DataNode[] dataNodes = dataNodePath.ToArray();
                    int i = 0;
                    while (dataNodes[dataNodes.Length - 1 - i] != relationDataNode)
                    {
                        DataNode subDataNode = dataNodes[dataNodes.Length - 1 - i];
                        valueTypePath.Push(subDataNode.AssignedMetaObject.Identity);
                        i++;
                    }
                    valueTypePath.Push(relationDataNode.AssignedMetaObject.Identity);
                    #endregion

                    #region Retrieve objects links from dataset

                    System.Collections.Generic.Dictionary<string, IDataTable> inStorageObjectsLinks = null;
                    System.Collections.Generic.Dictionary<StorageIdentity, System.Collections.Generic.Dictionary<string, IDataTable>> interStorageObjectsLinks = null;
                    foreach (IDataRow row in DataTable.Rows)
                    {
                        //LoadObjectLinksIndex!=-1 for value type without owner object activation
                        if (LoadObjectLinksIndex != -1 && !((bool)row[LoadObjectLinksIndex]))
                            continue;
                        object nodeObject = null;
                        if (ObjectActivation)
                            nodeObject = row[ObjectIndex];
                        if (inStorageObjectsLinks == null)
                            inStorageObjectsLinks = new System.Collections.Generic.Dictionary<string, IDataTable>();

                        DataNode rootDataNode = DataNode;

                        System.Collections.Generic.ICollection<IDataRow> masterRows = new System.Collections.Generic.List<IDataRow>();
                        masterRows.Clear();
                        masterRows.Add(row);
                        System.Collections.Generic.ICollection<IDataRow> detailsRows = new System.Collections.Generic.List<IDataRow>();
                        i = 0;
                        #region Walk through data tables and table relation and add object link pair to object link table
                        System.Collections.Generic.List<ValueTypeRow> valuePathDataRow = new System.Collections.Generic.List<ValueTypeRow>();
                        while (dataNodes[dataNodes.Length - 1 - i] != relationDataNode)
                        {
                            DataNode subDataNode = dataNodes[dataNodes.Length - 1 - i];

                            masterRows = rootDataNode.DataSource.GetRelatedRows(masterRows, subDataNode);
                            if (masterRows is System.Collections.IList)
                            {
                                valuePathDataRow.Insert(0, new ValueTypeRow((masterRows as System.Collections.IList)[0] as IDataRow, subDataNode));
                            }
                            else
                            {
                                foreach (IDataRow dataRow in masterRows)
                                {
                                    valuePathDataRow.Insert(0, new ValueTypeRow(dataRow, subDataNode));
                                    break;
                                }
                            }
                            rootDataNode = subDataNode;
                            i++;
                        }
                        System.Collections.IList objects = null;
                        object relatedObject = null;
                        if ((relationDataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).Multiplicity.IsMany)
                            objects = Activator.CreateInstance((relationDataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).FieldMember.FieldType) as System.Collections.IList;


                        if (relationDataNode.ThroughRelationTable && relationDataNode.DataSource.HasOutObjectContextData)
                        {
                            int sortIndex = 0;
                            if (interStorageObjectsLinks == null)
                                interStorageObjectsLinks = new Dictionary<string, Dictionary<string, IDataTable>>();

                            foreach (var partialRelationRows in rootDataNode.DataSource.GetRelationRowsPartial(row, relationDataNode))
                            {

                                foreach (ObjectsLinkRows relationRow in partialRelationRows.Value)
                                {
                                    IDataRow relatedObjectRow = relationRow.RelatedObjectRow;
                                    string subNodeStorageIdentity = null;
                                    if ((relationDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                        subNodeStorageIdentity = (DataNode.ObjectQuery as DistributedObjectQuery).QueryStorageIdentities[(int)relationRow.RelationDataRow["RoleAStorageIdentity"]];
                                    else
                                        subNodeStorageIdentity = (DataNode.ObjectQuery as DistributedObjectQuery).QueryStorageIdentities[(int)relationRow.RelationDataRow["RoleBStorageIdentity"]];


                                    if (subNodeStorageIdentity == storageIdentity)
                                    {
                                        if (!inStorageObjectsLinks.TryGetValue(partialRelationRows.Key, out IDataTable inStorageObjectsLinkTable))
                                        {
                                            inStorageObjectsLinkTable = DataObjectsInstantiator.CreateDataTable(false);
                                            inStorageObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                            inStorageObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                            inStorageObjectsLinkTable.Columns.Add("sortIndex", typeof(int));
                                            inStorageObjectsLinks[partialRelationRows.Key] = inStorageObjectsLinkTable;
                                        }
                                        object subNodeObject = relatedObjectRow[relationDataNode.DataSource.ObjectIndex];


                                        if (subNodeObject is DBNull)
                                            subNodeObject = null;
                                        if (ObjectActivation)
                                            inStorageObjectsLinkTable.Rows.Add(nodeObject, subNodeObject, sortIndex);
                                        if (objects != null)
                                            objects.Add(subNodeObject);
                                        else
                                            relatedObject = subNodeObject;
                                    }
                                    else
                                    {
                                        if (!interStorageObjectsLinks.ContainsKey(subNodeStorageIdentity))
                                            interStorageObjectsLinks[subNodeStorageIdentity] = new Dictionary<string, IDataTable>();


                                        if (!interStorageObjectsLinks[subNodeStorageIdentity].TryGetValue(partialRelationRows.Key, out IDataTable objectsLinkTable))
                                        {
                                            objectsLinkTable = DataObjectsInstantiator.CreateDataTable();
                                            objectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                            objectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                            objectsLinkTable.Columns.Add("sortIndex", typeof(int));
                                            foreach (var partialRelationData in RelationshipsData[subDataNodeRelationDataIdentity].RelationsData)
                                            {
                                                string relationName = partialRelationData.RelationName;
                                                foreach (IDataColumn column in RelationshipsData[relationDataNode.Identity].AssotiationTableRelationshipData.Data.Columns)
                                                    if (column.ColumnName != "RoleAStorageIdentity" && column.ColumnName != "RoleBStorageIdentity")
                                                        objectsLinkTable.Columns.Add(column.ColumnName, column.DataType);
                                            }
                                            interStorageObjectsLinks[subNodeStorageIdentity][partialRelationRows.Key] = objectsLinkTable;

                                        }
                                        if (ObjectActivation)
                                        {
                                            IDataRow newRow = objectsLinkTable.NewRow();
                                            newRow["OwnerObject"] = null;
                                            newRow["RelatedObject"] = nodeObject;
                                            newRow["sortIndex"] = sortIndex;
                                            
                                            objectsLinkTable.Rows.Add(newRow);
                                            foreach (IDataColumn column in objectsLinkTable.Columns)
                                            {
                                                if (column.ColumnName != "OwnerObject" && column.ColumnName != "RelatedObject")
                                                    newRow[column.ColumnName] = relationRow.RelationDataRow[column.ColumnName];
                                            }
                                        }
                                    }

                                }
                            }
                            i = 0;
                            object lastValueTypeObject = null;
                            ValueTypeRow lastValueTypeRow = valuePathDataRow[0];
                            foreach (ValueTypeRow valueTypeRow in valuePathDataRow)
                            {
                                if (i == 0)
                                {
                                    object valueTypeObject = valueTypeRow.DataRow[valueTypeRow.RowDataNode.DataSource.ObjectIndex];
                                    if (!(valueTypeObject is DBNull))
                                    {
                                        valueTypeObject = (relationDataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.SetValue(valueTypeObject, relatedObject);
                                        valueTypeRow.DataRow[rootDataNode.DataSource.ObjectIndex] = valueTypeObject;
                                        lastValueTypeRow = valueTypeRow;
                                        lastValueTypeObject = valueTypeObject;
                                    }
                                }
                                else
                                {
                                    //TODO να φτιαχτεί σεναριο γα struct μέσα σε struct
                                    object valueTypeObject = valueTypeRow.DataRow[valueTypeRow.RowDataNode.DataSource.ObjectIndex];
                                    if (!(valueTypeObject is DBNull))
                                    {
                                        (lastValueTypeRow.RowDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.SetValue(valueTypeObject, lastValueTypeObject);
                                        lastValueTypeRow = valueTypeRow;
                                        lastValueTypeObject = valueTypeObject;
                                    }
                                }
                                i++;
                            }

                        }
                        else
                        {
                            int sortIndex = 0;
                            foreach (var partialRelationRows in rootDataNode.DataSource.GetRelatedRowsPartial(row, relationDataNode))
                            {
                                IDataTable inStorageObjectsLinkTable = null;
                                if (!inStorageObjectsLinks.TryGetValue(partialRelationRows.Key, out inStorageObjectsLinkTable))
                                {
                                    inStorageObjectsLinkTable = DataObjectsInstantiator.CreateDataTable();
                                    inStorageObjectsLinkTable.Columns.Add("OwnerObject", typeof(object));
                                    inStorageObjectsLinkTable.Columns.Add("RelatedObject", typeof(object));
                                    inStorageObjectsLinkTable.Columns.Add("sortIndex", typeof(int));
                                    inStorageObjectsLinks[partialRelationRows.Key] = inStorageObjectsLinkTable;
                                }
                                foreach (IDataRow subNodeRow in partialRelationRows.Value)
                                {
                                    object subNodeObject = subNodeRow[relationDataNode.DataSource.ObjectIndex];

                                    if (subNodeObject is DBNull)
                                        subNodeObject = null;

                                    if (ObjectActivation)
                                        inStorageObjectsLinkTable.Rows.Add(nodeObject, subNodeObject, sortIndex);
                                    if (objects != null)
                                        objects.Add(subNodeObject);
                                    else
                                        relatedObject = subNodeObject;

                                }
                            }
                            i = 0;
                            object lastValueTypeObject = null;
                            ValueTypeRow lastValueTypeRow = valuePathDataRow[0];

                            foreach (ValueTypeRow valueTypeRow in valuePathDataRow)
                            {
                                if (i == 0)
                                {
                                    object valueTypeObject = valueTypeRow.DataRow[valueTypeRow.RowDataNode.DataSource.ObjectIndex];
                                    if (!(valueTypeObject is DBNull))
                                    {
                                        valueTypeObject = (relationDataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.SetValue(valueTypeObject, relatedObject);
                                        valueTypeRow.DataRow[rootDataNode.DataSource.ObjectIndex] = valueTypeObject;
                                        lastValueTypeRow = valueTypeRow;
                                        lastValueTypeObject = valueTypeObject;
                                    }
                                }
                                else
                                {
                                    //TODO να φτιαχτεί σεναριο γα struct μέσα σε struct
                                    object valueTypeObject = valueTypeRow.DataRow[valueTypeRow.RowDataNode.DataSource.ObjectIndex];
                                    if (!(valueTypeObject is DBNull))
                                    {
                                        (lastValueTypeRow.RowDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.SetValue(valueTypeObject, lastValueTypeObject);
                                        lastValueTypeRow = valueTypeRow;
                                        lastValueTypeObject = valueTypeObject;
                                    }
                                }
                                i++;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    StorageDataLoader dataLoader = DataLoaders[storageIdentity] as StorageDataLoader;
                    if (interStorageObjectsLinks != null)
                    {
                        foreach (var entry in interStorageObjectsLinks)
                        {
                            DistributedObjectQuery distributedObjectQuery = (DataNode.ObjectQuery as DistributedObjectQuery).DistributedObjectQueries[entry.Key];
                            Dictionary<string, IDataTable> objectsLink = null;

                            //relaationDataNode.DataSource
                            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(distributedObjectQuery))
                            {
                                var serializeTables = new Dictionary<string, DataLoader.StreamedTable>();
                                foreach (var dataTableEntry in entry.Value)
                                    serializeTables[dataTableEntry.Key] = dataTableEntry.Value.SerializeTable();
                                objectsLink = new Dictionary<string, IDataTable>();

                                foreach (var dataTableEntry in distributedObjectQuery.GetParentSubDataNodeObjectRelationLinks(relationDataNode.Identity, serializeTables, RelationshipsData[relationDataNode.Identity].AssotiationTableRelationshipData.DetailDataSourceReferenceObjectIdentityTypes))
                                    objectsLink[dataTableEntry.Key] = DataObjectsInstantiator.CreateDataTable(dataTableEntry.Value);
                            }
                            else
                            {
                                objectsLink = distributedObjectQuery.GetParentSubDataNodeObjectRelationLinks(relationDataNode.Identity, entry.Value, RelationshipsData[relationDataNode.Identity].AssotiationTableRelationshipData.DetailDataSourceReferenceObjectIdentityTypes);
                            }
#if !DeviceDotNet
                            dataLoader.LoadObjectRelationLinks(valueTypePath, objectsLink);
#else
                         dataLoader.LoadObjectRelationLinks(valueTypePath, objectsLink);
#endif
                        }
                    }
                    #region Pass objects links to the data loaders to load in memory
                    if (inStorageObjectsLinks != null)
                    {
#if !DeviceDotNet
                        dataLoader.LoadObjectRelationLinks(valueTypePath, inStorageObjectsLinks);
#else
                        dataLoader.LoadObjectRelationLinks(valueTypePath, inStorageObjectsLinks);
#endif
                    }
                    #endregion
                }
            }

        }
        #endregion





        /// <exclude>Excluded</exclude>
        [NonSerialized]
        OOAdvantech.Member<bool> _HasObjectsToActivate = new Member<bool>();
        ///<summary>
        ///HaveObjectsToActivate is truein case where there are objects in passive mode 
        ///and system must be activate.    
        ///</summary>
        /// <MetaDataID>{2682491f-cfd7-4156-bf88-eec521465ae2}</MetaDataID>
        internal override bool ThereAreObjectsToActivate
        {
            get
            {
                //you can't casing data because the ObjectActivation can change after prefetching calculation mechanism


                //if (_HasObjectsToActivate.UnInitialized)
                //{
                //_HasObjectsToActivate.Value = false;
                if (!ObjectActivation)
                    return false;// _HasObjectsToActivate.Value;

                if (DataLoadersMetadata == null)
                    return false;// _HasObjectsToActivate.Value;
                foreach (DataLoaderMetadata dataLoaderMetadata in DataLoadersMetadata.Values)
                {
                    foreach (StorageCell storageCell in dataLoaderMetadata.StorageCells)
                    {
                        if (!storageCell.AllObjectsInActiveMode)
                            return true;// _HasObjectsToActivate.Value = true;
                    }
                }
                //}
                return false;
            }
        }
        /// <MetaDataID>{ff4d03c1-7f10-4480-b937-ecbbff53e46a}</MetaDataID>
        [IgnoreErrorCheck]
        [NonSerialized]
        [RoleBMultiplicityRange(1, 1)]
        [Association("", Roles.RoleA, "87ecb47c-340f-4643-bdc9-0b25ea3205a6")]
        public System.Collections.Generic.Dictionary<string, DataLoaderMetadata> DataLoadersMetadata;


        ///<summary>
        ///
        ///</summary>
        /// <MetaDataID>{7abfad32-6166-4670-b40c-9248ed6aa218}</MetaDataID>
        protected internal override System.Collections.Generic.List<AssociationEnd> PrefetchingAssociationEnds
        {
            get
            {
                System.Collections.Generic.List<AssociationEnd> prefetchingAssociations = new System.Collections.Generic.List<AssociationEnd>();
                if (DataNode.Classifier is Structure)
                {

                    foreach (MetaDataRepository.AssociationEnd associationEnd in DataNode.Classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Association.HasPersistentObjectLink && !associationEnd.HasLazyFetchingRealization && associationEnd.Navigable)
                        {
                            if (!prefetchingAssociations.Contains(associationEnd))
                                prefetchingAssociations.Add(associationEnd);
                        }
                    }
                    return prefetchingAssociations;
                }
                //Get from each storageCell type the prefetching AssotionEnd
                foreach (DataLoaderMetadata dataLoaderMetadata in DataLoadersMetadata.Values)
                {
                    foreach (StorageCell storageCell in dataLoaderMetadata.StorageCells)
                    {
                        if (storageCell is MetaDataRepository.StorageCellReference)
                            continue;
                        MetaDataRepository.Classifier classifier = MetaDataRepository.Classifier.GetClassifier(storageCell.Type.GetExtensionMetaObject(typeof(Type)) as Type);
                        foreach (MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                        {
                            if (associationEnd.Association.HasPersistentObjectLink && !associationEnd.HasLazyFetchingRealization && associationEnd.Navigable)
                            {
                                if (!prefetchingAssociations.Contains(associationEnd))
                                    prefetchingAssociations.Add(associationEnd);
                            }
                        }
                    }
                }
                return prefetchingAssociations;

            }
        }
        /// <MetaDataID>{9c956bf4-72c8-45eb-85ef-449870de4465}</MetaDataID>
        public StorageDataSource(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, IDataTable table)
            : base(dataNode)
        {
            System.Diagnostics.Debug.Assert(false, "DataNode.DataSource is null");
            DataLoadersMetadata = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata>();
            _DataTable = table;
        }
        /// <MetaDataID>{737296d6-98f1-4dc8-be29-cf59d32c95be}</MetaDataID>
        public StorageDataSource(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, DataSource dataSource)
            : base(dataNode, dataSource)
        {

        }

        /// <MetaDataID>{d238cc23-166d-4fce-b496-1a6ade2a413a}</MetaDataID>
        public StorageDataSource(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode)
            : this(dataNode, new Dictionary<string, DataLoaderMetadata>())
        {

        }
        /// <MetaDataID>{adf5bfaa-74a1-44ba-a4e8-291a86b55116}</MetaDataID>
        public StorageDataSource(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, Dictionary<string, DataLoaderMetadata> dataLoadersMetadata)
            : base(dataNode)
        {
            DataLoadersMetadata = dataLoadersMetadata;
            foreach (DataLoaderMetadata dataLoaderMetadata in DataLoadersMetadata.Values)
            {
                foreach (StorageCell storageCell in dataLoaderMetadata.StorageCells)
                {
                    if (storageCell.Type != null && !(storageCell.Type is DotNetMetaDataRepository.Class))
                    {
                        Type type = storageCell.Type.GetExtensionMetaObject<Type>();
                        MetaDataRepository.Classifier _class = DotNetMetaDataRepository.Type.GetClassifierObject(type);
                        long count = _class.Features.Count;
                    }
                }
            }



            //Collections.Generic.Dictionary<string, Collections.Generic.Set<StorageCell>> storagCellsDictionary = new Collections.Generic.Dictionary<string, OOAdvantech.Collections.Generic.Set<StorageCell>>();
            //StorageCells = storageCells;

            //foreach (StorageCell storageCell in storageCells)
            //{
            //    if (!storagCellsDictionary.ContainsKey(storageCell.StorageIntentity))
            //        storagCellsDictionary[storageCell.StorageIntentity] = new OOAdvantech.Collections.Generic.Set<StorageCell>();

            //    storagCellsDictionary[storageCell.StorageIntentity].Add(storageCell);

            //}

            //foreach (System.Collections.Generic.KeyValuePair<string, Collections.Generic.Set<StorageCell>> entry in storagCellsDictionary)
            //{

            //    string storageName=null;
            //    string storageLocation=null;
            //    string storageType = null;



            //    entry.Value[0].GetStorageConnectionData(out storageName, out storageLocation, out storageType);

            //    ///TODO Ο κώδικας γράφτηκε γιατί έχει προβλημα όταν η storage είναι πάνω σε memory stream   
            //    PersistenceLayer.ObjectStorage objectStorage = null;
            //    if (entry.Key == (dataNode.ObjectQuery as OQLStatement).ObjectStorage.StorageMetaData.StorageIdentity)
            //        objectStorage = (dataNode.ObjectQuery as OQLStatement).ObjectStorage;
            //    else
            //        objectStorage = PersistenceLayer.ObjectStorage.OpenStorage(storageName, storageLocation, storageType);

            //    DataLoader dataLoader = (objectStorage.StorageMetaData as MetaDataRepository.Storage) .CreateDataLoader(dataNode,dataNode.ObjectQuery.SearchCondition, entry.Value) as DataLoader; //ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.MSSQLPersistenceRunTime.ObjectQueryLanguage.DataLoader", "", dataNode, entry.Value, new Type[2] { typeof(MetaDataRepository.ObjectQueryLanguage.DataNode), typeof(Collections.Generic.Set<StorageCell>) }) as DataLoader;
            //    DataLoaders[entry.Key] = dataLoader;

            //}

        }


        ///// <MetaDataID>{cd4a29f8-b090-406f-a76c-5a2418a72f75}</MetaDataID>
        //public StorageDataSource(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, OOAdvantech.Collections.Generic.Dictionary<string, DataLoaderMetadata> dataLoadersMetadata )
        //    : base(dataNode)
        //{

        //    //var dataLoadersMetadata = GetDataLoadersMetaData(storageCells);

        //    DataLoadersMetadata = dataLoadersMetadata;
        //}




        #region IDeserializationCallback Members

#if !PORTABLE
        /// <MetaDataID>{9757d211-27bf-4538-91cb-d028a14d5c7f}</MetaDataID>
        void System.Runtime.Serialization.IDeserializationCallback.OnDeserialization(object sender)
        {
            _HasObjectsToActivate = new Member<bool>();
            DataLoaders = new OOAdvantech.Collections.Generic.Dictionary<string, DataLoader>();
            DataLoadersMetadata = new Dictionary<string, DataLoaderMetadata>();

        }
#endif

        #endregion

        /// <MetaDataID>{f59b3c50-257f-460f-84a8-327f1adabb81}</MetaDataID>
        public virtual bool HasDataInObjectContext(string storageIdentity)
        {
            if (DataLoadersMetadata == null)
                return false;
            if (DataLoadersMetadata.ContainsKey(storageIdentity))
            {
                if (DataLoadersMetadata[storageIdentity].MemoryCell != null)
                    return true;
                foreach (MetaDataRepository.StorageCell storageCell in DataLoadersMetadata[storageIdentity].StorageCells)
                {
                    if (!(storageCell is MetaDataRepository.StorageCellReference))
                        return true;
                }
            }
            return false;
        }


        ///<summary>
        ///
        ///</summary>
        /// <MetaDataID>{fd518a34-a405-4d58-81a5-aa1985312446}</MetaDataID>
        public override bool HasInObjectContextData
        {
            get
            {
                if (!(DataNode.ObjectQuery is DistributedObjectQuery))
                    return true;
                if (DataLoadersMetadata == null)
                    return false;
                ObjectsContext objectsContext = (DataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectsContext;
                foreach (string storageIdentity in DataLoadersMetadata.Keys)
                {
                    if (objectsContext.Identity.ToLower() == storageIdentity.ToLower())
                    {
                        if (DataLoadersMetadata[storageIdentity].MemoryCell is InProcessMemoryCell)
                            return true;
                        foreach (MetaDataRepository.StorageCell storageCell in DataLoadersMetadata[storageIdentity].StorageCells)
                        {
                            if (!(storageCell is MetaDataRepository.StorageCellReference))
                                return true;
                        }
                    }

                    //if (DataNode.Classifier!=null&& DataLoaders.ContainsKey(storageIdentity)&&(DataLoaders[storageIdentity] as StorageDataLoader).HasTransientObjects)
                    //    return true;
                }
                return false;
            }
        }

        /// <MetaDataID>{0b0f155f-e860-40a0-8755-affdb699f168}</MetaDataID>
        public override bool HasOutObjectContextData
        {
            get
            {
                if (!(DataNode.ObjectQuery is DistributedObjectQuery))
                    return false;
                if (DataLoadersMetadata == null)
                    return false;
                ObjectsContext objectsContext = (DataNode.ObjectQuery as DistributedObjectQuery).ObjectsContext as ObjectsContext;
                foreach (string storageIdentity in DataLoadersMetadata.Keys)
                {
                    if (objectsContext.Identity.ToLower() == storageIdentity.ToLower())
                    {
                        foreach (MetaDataRepository.StorageCell storageCell in DataLoadersMetadata[storageIdentity].StorageCells)
                        {
                            if ((storageCell is MetaDataRepository.StorageCellReference))
                                return true;
                        }
                    }
                }
                return false;
            }
        }




        /// <summary>
        /// Check data source data loaders metadata and u
        /// </summary>
        /// <param name="dataLoadersMetadata">
        /// Defines a collection with all DataLoaderMetadata 
        /// Some of DataLoaderMatada is new for DataSource
        /// </param>
        /// <returns>
        /// Returns the new dataloaders metadata
        /// </returns>
        internal List<DataLoaderMetadata> AddNewDataLoadersMetaData(System.Collections.Generic.Dictionary<string, DataLoaderMetadata> dataLoadersMetadata)
        {

            List<DataLoaderMetadata> newDataLoadersMetaData = new List<DataLoaderMetadata>();
            foreach (var relatedDataLoaderMetadataEntry in dataLoadersMetadata)
            {
                if (!DataLoadersMetadata.ContainsKey(relatedDataLoaderMetadataEntry.Key))
                {
                    // add the new data loader metadata
                    DataLoadersMetadata[relatedDataLoaderMetadataEntry.Key] = relatedDataLoaderMetadataEntry.Value;
                    newDataLoadersMetaData.Add(relatedDataLoaderMetadataEntry.Value);
                }
                else
                {
                    DataLoaderMetadata dataLoaderMetadata = DataLoadersMetadata[relatedDataLoaderMetadataEntry.Key];
                    //Update the dataloaderMetaData with new storageCells 
                    foreach (var newStorageCell in relatedDataLoaderMetadataEntry.Value.StorageCells)
                    {
                        bool exist = false;
                        foreach (var storageCell in dataLoaderMetadata.StorageCells)
                        {
                            if (storageCell.GetType() == newStorageCell.GetType() &&
                                storageCell.SerialNumber == newStorageCell.SerialNumber &&
                                storageCell.StorageIdentity == newStorageCell.StorageIdentity)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            dataLoaderMetadata.AddStorageCell(newStorageCell);
                            newDataLoadersMetaData.Add(dataLoaderMetadata);
                        }
                    }
                }
            }
            return newDataLoadersMetaData;

        }
    }
}
