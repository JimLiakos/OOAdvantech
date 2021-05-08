namespace OOAdvantech.MetaDataRepository.ObjectQueryLanguage
{
    using System.Collections.Generic;
    using System.Linq;

    using SubDataNodeIdentity = System.Guid;
    using RelationPartIdentity = System.String;
    using System;
    using Remoting;
#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
    using System;
#endif

    /// <MetaDataID>{b68e978e-7af6-48a3-954d-7607bd241d31}</MetaDataID>
    public class MemroryObjectsDataLoader : OOAdvantech.MetaDataRepository.ObjectQueryLanguage.StorageDataLoader
    {


        List<AggregateExpressionDataNode> _ResolvedAggregateExpressions = new List<AggregateExpressionDataNode>();
        public override List<AggregateExpressionDataNode> ResolvedAggregateExpressions
        {
            get { return _ResolvedAggregateExpressions; }
        }
        public override bool ParticipateInGlobalResolvedCriterion
        {
            get { return true; }
        }

        internal override void AggregateExpressionDataNodeResolved(AggregateExpressionDataNode aggregateDataNode)
        {
            _ResolvedAggregateExpressions.Add(aggregateDataNode);
        }



        bool _GroupedDataLoaded = false;
        public override bool GroupedDataLoaded
        {
            get
            {
                return _GroupedDataLoaded;
            }
            protected internal set
            {
                _GroupedDataLoaded = value;
            }
        }

        InProcessMemoryCell MemoryCell;
        protected override void OnPrepareForDataLoading()
        {

            base.OnPrepareForDataLoading();

            if (_Data == null && DataNode.Type == DataNode.DataNodeType.Object)
            {
                _Data = DataSource.DataObjectsInstantiator.CreateDataTable(false, DataNode.DataSource.Identity);
                if (!(DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
                {
                    Data.TableName = DataNode.Alias;
                    CreateTableColumns(Data);
                }

                if (DataNode.ThroughRelationTable)
                    CreateParentRelationshipData();

            }
        }

        /// <MetaDataID>{abb79650-298d-485a-a40b-130e03441414}</MetaDataID>
        public MemroryObjectsDataLoader(DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode, dataLoaderMetadata)
        {
            MemoryCell = dataLoaderMetadata.MemoryCell as InProcessMemoryCell;



        }
        /// <MetaDataID>{8313ac58-618c-48cb-ad91-e7e41e6596db}</MetaDataID>
        public override bool RetrievesData
        {
            get { return true; }
        }
        /// <MetaDataID>{3fd1e6eb-8cf9-4d03-82a2-10e5484a340d}</MetaDataID>
        protected override object Convert(object value, System.Type type)
        {
            if (value == null || value is System.DBNull)
                return value;
            else
                return System.Convert.ChangeType(value, type);

        }

        internal protected override bool CriterionsForDataLoaderResolvedLocally(DataLoader dataLoader)
        {
            return false;
        }
        /// <MetaDataID>{dcc15330-58a6-45bd-93e4-2ec81ce88c7b}</MetaDataID>
        public override string GetLocalDataColumnName(DataNode dataNode)
        {
            return dataNode.Name;
        }
        public override bool LocalDataColumnExistFor(DataNode dataNode)
        {
            return true;
        }
        /// <MetaDataID>{dbe859d3-ba58-4b87-8736-7dc445c877d9}</MetaDataID>
        public override bool ObjectActivation
        {
            get { return (DataNode.Type != DataNode.DataNodeType.Group && (DataNode.ParticipateInSelectClause || DataNode.ParticipateInAggregateFunction)); }
        }

        /// <MetaDataID>{3d6eab8e-6373-413c-8dce-f365dfdb3145}</MetaDataID>
        public override bool DataLoadedInParentDataSource
        {
            get
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && DataNode.Type == DataNode.DataNodeType.Object)
                    return true;
                else
                    return false;
            }
            set
            {

            }
        }
        /// <MetaDataID>{068c2a64-3d4d-4725-9dbd-e26c915755fd}</MetaDataID>
        public override bool HasRelationIdentityColumnsFor(DataNode subDataNode)
        {

            if (subDataNode.ThroughRelationTable)
                return true;
            if (subDataNode.AssignedMetaObject is AssociationEnd)
            {
                if (!(subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
                    (subDataNode.AssignedMetaObject as AssociationEnd).Association.MultiplicityType != AssociationType.ManyToMany)
                {
                    return true;
                }
                if ((subDataNode.AssignedMetaObject as AssociationEnd).Association.MultiplicityType == AssociationType.OneToOne &&
                    !(subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().IsRoleA)
                    return true;
                return false;
            }
            return true;
        }

        ///// <MetaDataID>{da7fb9d3-c301-42da-b56c-0bd7de65a493}</MetaDataID>
        //private bool HasSubNodeRelationIdentityColumns(DataNode subDataNode)
        //{
        //    return true;

        //    if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
        //        return true;

        //    if ((subDataNode.AssignedMetaObject is AssociationEnd) && (subDataNode.AssignedMetaObject as AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
        //        !(subDataNode.AssignedMetaObject as AssociationEnd).Multiplicity.IsMany)
        //    {
        //        return true;
        //    }
        //    else if ((subDataNode.AssignedMetaObject is AssociationEnd) &&
        //        (subDataNode.AssignedMetaObject as AssociationEnd).Association.MultiplicityType == AssociationType.ManyToMany &&
        //        subDataNode.Classifier.ClassHierarchyLinkAssociation != (subDataNode.AssignedMetaObject as AssociationEnd).Association)
        //    {
        //        return true;
        //    }
        //    else
        //        return false;

        //}

        ///// <MetaDataID>{0ee76590-8aa3-4b79-87ec-e3c8697b0cdc}</MetaDataID>
        //protected override DataLoader GetDataLoader(DataNode dataNode)
        //{
        //    if(dataNode.ObjectQuery is DistributedObjectQuery)
        //        return dataNode.DataSource.DataLoaders[(dataNode.ObjectQuery as DistributedObjectQuery).Identity];
        //    else
        //        return dataNode.DataSource.DataLoaders[dataNode.ObjectQuery.GetHashCode().ToString()];
        //}
        //protected override void BuildDataTable(DataNode dataNode, System.Data.DataTable table, System.Collections.Generic.Dictionary<string, string> aliasColumns, System.Collections.Generic.Dictionary<string, int> columnsIndices, int[] sourceColumnsIndices)
        //{

        //}
        //protected override void BuildDataTable(DataNode dataNode, System.Data.IDataReader dataReader, System.Collections.Generic.Dictionary<string, string> aliasColumns, System.Collections.Generic.Dictionary<string, int> columnsIndices, int[] sourceColumnsIndices)
        //{

        //}

        /// <MetaDataID>{006c0528-98c3-453a-baf9-a78ea75dfb92}</MetaDataID>
        public override bool AggregateExpressionDataNodeResolved(System.Guid aggregateExpressionDataNodeIdentity)
        {
            return false;
        }
        //protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        //{

        //}
        //protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, System.Collections.ArrayList relatedObjects)
        //{

        //}
        //protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, object ownerObject, object relatedObject)
        //{

        //}
        //protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, object ownerObject, System.Collections.ArrayList relatedObjects)
        //{

        //}

        /// <summary>If it is true then the data source table has the identity coloumns of foreign key relation,
        /// otherwise table has the reference columns. Always the identity columns has unique constrain </summary>
        /// <MetaDataID>{8ef88c24-9f0d-4243-83f1-620c87ec4012}</MetaDataID>

        //internal override void LoadObjectRelationLinks(DataNode subDataNode)
        //{

        //}
        //internal override void SetValueTypeValues(DataNode subDataNode)
        //{
        //    throw new System.Exception("The method or operation is not implemented.");
        //}

        public override System.Collections.Generic.Dictionary<RelationPartIdentity, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> RecursiveParentReferenceColumns
        {
            get
            {

                if (!DataNode.Recursive)
                    throw new System.Exception("The DataNode isn't recursive");



                System.Collections.Generic.List<string> childRelationColumns = new System.Collections.Generic.List<string>();
                ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> dataSourceRelationsColumnsWithParent = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>>();

                MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                MetaDataRepository.Attribute attribute = DataNode.AssignedMetaObject as MetaDataRepository.Attribute;

                if (associationEnd != null)
                {
                    if (associationEnd.IsRoleA)
                        childRelationColumns.Add(DataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleB_OID");
                    else
                        childRelationColumns.Add(DataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleA_OID");
                }
                if (!dataSourceRelationsColumnsWithParent.ContainsKey(DataNode.AssignedMetaObject.Identity.ToString()))
                    dataSourceRelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()] = new System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>();
                dataSourceRelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()][identityType] = childRelationColumns;
                return dataSourceRelationsColumnsWithParent;

                //return DataSourceRelationsColumnsWithParent;


            }
        }


        /// <MetaDataID>{8e03214e-bbc7-42d5-bf10-197c9cb2cc12}</MetaDataID>
        public override System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> RecursiveParentColumns
        {

            get
            {
                if (!DataNode.Recursive)
                    throw new System.Exception("The DataNode isn't recursive");

                List<string> oidColumns = new List<string>();
                oidColumns.Add("MC_ObjectID");
                ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });

                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>> relationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>>();
                relationColumns[DataNode.AssignedMetaObject.Identity.ToString()] = new System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>();
                relationColumns[DataNode.AssignedMetaObject.Identity.ToString()][identityType] = oidColumns;
                return relationColumns;
            }
        }

        /// <MetaDataID>{f28866f6-ce7e-4ee5-b324-b890924e380d}</MetaDataID>
        public override System.Collections.Generic.List<ObjectIdentityType> ObjectIdentityTypes
        {

            get
            {
                ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
                System.Collections.Generic.List<ObjectIdentityType> objectIdentityTypes = new System.Collections.Generic.List<ObjectIdentityType>();
                objectIdentityTypes.Add(identityType);
                return objectIdentityTypes;
            }
        }
        /// <MetaDataID>{a453a4ca-fd1f-40c8-b485-2f248615272e}</MetaDataID>
        public override void UpdateObjectIdentityTypes(System.Collections.Generic.List<ObjectIdentityType> dataLoaderObjectIdentityTypes)
        {

        }
        //public override System.Collections.Generic.List<string> ObjectIDColumns
        //{
        //    get 
        //    {
        //        return  new List<string>(){"ObjectID"};

        //    }
        //}


        /// <MetaDataID>{dda685e7-9d1d-4887-b0aa-249fccd1aafc}</MetaDataID>
        public override System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> DataSourceRelationsColumnsWithParent
        {
            get
            {
                System.Collections.Generic.List<string> childRelationColumns = new System.Collections.Generic.List<string>();
                RelationColumns relationColumns = new RelationColumns(childRelationColumns, "");
                //System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>> childRelationColumns = new System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>();
                ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>> dataSourceRelationsColumnsWithParent = new Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>();



                if (DataNode.Type == DataNode.DataNodeType.Group)
                {
                    if (DataNode.RealParentDataNode != null)
                    {
                        string relationPartIdentity = DataNode.FullName;
                        dataSourceRelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in (DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ObjectIdentityTypes)
                        {
                            childRelationColumns = new List<string>();
                            if (!dataSourceRelationsColumnsWithParent[relationPartIdentity].ContainsKey(objectIdentityType))
                            {
                                foreach (MetaDataRepository.IdentityPart column in objectIdentityType.Parts)
                                    childRelationColumns.Add(DataNode.RealParentDataNode.Alias + "_" + column.PartTypeName);
                                dataSourceRelationsColumnsWithParent[relationPartIdentity][objectIdentityType] = new RelationColumns(childRelationColumns, "OSM_StorageIdentity");
                            }
                        }
                        return dataSourceRelationsColumnsWithParent;
                    }
                }
                MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                MetaDataRepository.Attribute attribute = DataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                if (associationEnd != null && HasRelationIdentityColumns)
                {
                    childRelationColumns.Add("MC_ObjectID");
                    relationColumns = new RelationColumns(childRelationColumns, "OSM_StorageIdentity");

                }
                else
                {
                    if (DataNode.ThroughRelationTable)
                        throw new System.NotSupportedException();
                    if (associationEnd != null)
                    {
                        identityType = associationEnd.GetReferenceObjectIdentityTypes(new List<ObjectIdentityType> { identityType })[0];
                        foreach (IIdentityPart part in identityType.Parts)
                            childRelationColumns.Add(DataNode.ValueTypePathDiscription + part.Name);
                        relationColumns = new RelationColumns(childRelationColumns, DataNode.ValueTypePathDiscription + associationEnd.ReferenceStorageIdentityColumnName);
                    }
                }
                if (attribute != null)
                {
                    childRelationColumns.Add(attribute.Name + "RoleB_OID");
                }


                if (!dataSourceRelationsColumnsWithParent.ContainsKey(DataNode.AssignedMetaObject.Identity.ToString()))
                    dataSourceRelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()] = new System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>();
                dataSourceRelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()][identityType] = relationColumns;
                return dataSourceRelationsColumnsWithParent;

            }
        }

        System.Collections.Generic.Dictionary<Guid, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>> _GroupByKeyRelationColumns;

        /// <MetaDataID>{f9aa538e-7ce2-4f8a-bf88-aff6e2899020}</MetaDataID>
        public override System.Collections.Generic.Dictionary<System.Guid, System.Collections.Generic.Dictionary<ObjectIdentityType, ObjectKeyRelationColumns>> GroupByKeyRelationColumns
        {
            get
            {
                if (_GroupByKeyRelationColumns == null)
                {
                    _GroupByKeyRelationColumns = new System.Collections.Generic.Dictionary<Guid, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>>();
                    if (DataNode.Type == DataNode.DataNodeType.Group)
                    {
                        foreach (DataNode dataNode in (DataNode as GroupDataNode).GroupKeyDataNodes)
                        {

                            if (dataNode.Type == DataNode.DataNodeType.Object)
                            {
                                foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType identityType in (dataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity] as StorageDataLoader).ObjectIdentityTypes)
                                {

                                    if (!_GroupByKeyRelationColumns.ContainsKey(dataNode.Identity))
                                        _GroupByKeyRelationColumns[dataNode.Identity] = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>();
                                    if (!_GroupByKeyRelationColumns[dataNode.Identity].ContainsKey(identityType))
                                    {
                                        List<string> groupingColumnsNames = new List<string>();
                                        List<string> groupedDataColumnsNames = new List<string>();
                                        foreach (MetaDataRepository.IIdentityPart identityPart in identityType.Parts)
                                        {
                                            groupingColumnsNames.Add(dataNode.Alias + "_" + identityPart.PartTypeName);
                                            if ((dataNode.DataSource as StorageDataSource).DataLoadedInParentDataSource)
                                                groupedDataColumnsNames.Add(dataNode.Alias + "_" + identityPart.PartTypeName);
                                            else
                                                groupedDataColumnsNames.Add(identityPart.PartTypeName);
                                        }
                                        ObjectKeyRelationColumns keyReferenceColumn = new ObjectKeyRelationColumns(identityType, groupingColumnsNames, groupedDataColumnsNames, dataNode.DataSource);
                                        _GroupByKeyRelationColumns[dataNode.Identity][identityType] = keyReferenceColumn;
                                    }
                                }
                            }
                        }
                    }
                }
                return _GroupByKeyRelationColumns;
            }
        }




        /// <MetaDataID>{effd50cf-5736-4550-902c-ce7aa2f59969}</MetaDataID>
        public override System.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get
            {
                Dictionary<SubDataNodeIdentity, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns>>> parentRelationColumns = new Dictionary<Guid, Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>>();
                ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {

                    //System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                    //dataColumns.Add("MC_ObjectID");
                    //System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>> dataSourceRelationsColumns = new System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>();
                    //dataSourceRelationsColumns[identityType] = dataColumns;
                    //if (!parentRelationColumns.ContainsKey(subDataNode.Identity))
                    //    parentRelationColumns[subDataNode.Identity] = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<ObjectIdentityType, System.Collections.Generic.List<string>>>();
                    //parentRelationColumns[subDataNode.Identity][subDataNode.AssignedMetaObject.Identity.ToString()] = dataSourceRelationsColumns;

                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                    if (associationEnd != null)
                    {
                        associationEnd = associationEnd.GetOtherEnd();
                        System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                        RelationColumns relationColumns = new RelationColumns(dataColumns, "OSM_StorageIdentity");
                        if (HasRelationReferenceColumnsFor(subDataNode))
                        {
                            identityType = associationEnd.GetReferenceObjectIdentityTypes(new List<ObjectIdentityType> { identityType })[0];
                            foreach (IIdentityPart part in identityType.Parts)
                                dataColumns.Add(subDataNode.ValueTypePathDiscription + part.Name);

                            relationColumns = new RelationColumns(dataColumns, subDataNode.ValueTypePathDiscription + associationEnd.ReferenceStorageIdentityColumnName);
                        }
                        else
                        {
                            dataColumns.Add("MC_ObjectID");
                        }
                        System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns> dataSourceRelationsColumns = new Dictionary<ObjectIdentityType, RelationColumns>();

                        dataSourceRelationsColumns[identityType] = relationColumns;

                        if (!parentRelationColumns.ContainsKey(subDataNode.Identity))
                            parentRelationColumns[subDataNode.Identity] = new Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>();
                        parentRelationColumns[subDataNode.Identity][subDataNode.AssignedMetaObject.Identity.ToString()] = dataSourceRelationsColumns;
                    }
                    if (attribute != null && subDataNode.DataSource != null)
                    {
                        System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                        dataColumns.Add("MC_ObjectID");
                        System.Collections.Generic.Dictionary<ObjectIdentityType, RelationColumns> dataSourceRelationsColumns = new Dictionary<ObjectIdentityType, RelationColumns>();
                        dataSourceRelationsColumns[identityType] = new RelationColumns(dataColumns, "OSM_StorageIdentity");
                        if (!parentRelationColumns.ContainsKey(subDataNode.Identity))
                            parentRelationColumns[subDataNode.Identity] = new Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>();
                        parentRelationColumns[subDataNode.Identity][subDataNode.AssignedMetaObject.Identity.ToString()] = dataSourceRelationsColumns;
                    }
                    if (subDataNode.Type == DataNode.DataNodeType.Group)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ObjectIdentityTypes)
                        {

                            if (!parentRelationColumns.ContainsKey(subDataNode.Identity))
                            {
                                parentRelationColumns[subDataNode.Identity] = new Dictionary<string, Dictionary<ObjectIdentityType, RelationColumns>>();
                                parentRelationColumns[subDataNode.Identity][subDataNode.FullName] = new Dictionary<ObjectIdentityType, RelationColumns>();
                            }
                            if (!parentRelationColumns[subDataNode.Identity][subDataNode.FullName].ContainsKey(objectIdentityType))
                            {
                                System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();

                                foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                    relationColumns.Add(identityPart.PartTypeName);
                                parentRelationColumns[subDataNode.Identity][subDataNode.FullName][objectIdentityType] = new RelationColumns(relationColumns, "OSM_StorageIdentity");
                            }

                        }
                    }

                }

                return parentRelationColumns;


            }
        }

        //public override Storage LoadFromStorage
        //{
        //    get { throw new System.Exception("The method or operation is not implemented."); }
        //}

        //public override OOAdvantech.PersistenceLayer.ObjectStorage ObjectStorage
        //{
        //    get { throw new System.Exception("The method or operation is not implemented."); }
        //}

        /// <MetaDataID>{765e58bd-1c96-43cf-8ef1-17c04cf7d9aa}</MetaDataID>
        public override Classifier Classifier
        {
            get { return DataNode.Classifier; }
        }
        /// <MetaDataID>{96daf6e4-7bae-44ae-86eb-9e93eacbfc2a}</MetaDataID>
        public override IDataTable Data
        {
            get
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && DataNode.Type == DataNode.DataNodeType.Object)
                    return GetDataLoader(DataNode.ParentDataNode).Data;
                else
                    return base.Data;
            }
        }

        /// <MetaDataID>{e92f7763-590d-435b-80ab-8da1b33de6e4}</MetaDataID>
        public override void RetrieveFromStorage()
        {
            if (DataNode.ValueTypePath.Count > 0)
                return;

            if (DataNode.Type == DataNode.DataNodeType.Group)
            {
                return;
                if (_Data == null)
                    _Data = DataSource.DataObjectsInstantiator.CreateDataTable();

                #region Grouping data can't be resolved localy. Data loader build an empty table with necessary columns

                foreach (DataNode groupKeyDataNodes in (DataNode as GroupDataNode).GroupKeyDataNodes)
                {
                    if (groupKeyDataNodes.Type == DataNode.DataNodeType.Object)
                    {
                        foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType in groupKeyDataNodes.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                        {
                            foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                Data.Columns.Add(groupKeyDataNodes.Alias + "_" + part.Name, part.Type);
                        }
                    }
                    else if (groupKeyDataNodes.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        if (groupKeyDataNodes.ParentDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            Data.Columns.Add(groupKeyDataNodes.ParentDataNode.ParentDataNode.Alias + "_" + groupKeyDataNodes.ParentDataNode.Name + "_" + groupKeyDataNodes.AssignedMetaObject.Name, (groupKeyDataNodes.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                        else
                            Data.Columns.Add(groupKeyDataNodes.ParentDataNode.Alias + "_" + groupKeyDataNodes.AssignedMetaObject.Name, (groupKeyDataNodes.AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                    }
                }
                if (DataNode.RealParentDataNode != null && DataNode.RealParentDataNode.Type == DataNode.DataNodeType.Object)
                {
                    foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType in DataNode.RealParentDataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity].ObjectIdentityTypes)
                    {
                        foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                            Data.Columns.Add(DataNode.RealParentDataNode.Alias + "_" + part.Name, part.Type);
                    }
                }
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode is AggregateExpressionDataNode)
                    {
                        if ((subDataNode as AggregateExpressionDataNode).ArithmeticExpression != null)
                            Data.Columns.Add(subDataNode.Alias, (subDataNode as AggregateExpressionDataNode).ArithmeticExpression.ResultType);
                        else
                        {
                            if (subDataNode.Type != DataNode.DataNodeType.Count)
                                Data.Columns.Add(subDataNode.Alias, ((subDataNode as AggregateExpressionDataNode).AggregateExpressionDataNodes[0].AssignedMetaObject as MetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
                            else
                                Data.Columns.Add(subDataNode.Alias, typeof(ulong));
                        }
                    }
                }

                if (DataNode is GroupDataNode)
                    _Data.Columns.Add(GetGroupingDataColumnName(DataNode), typeof(object));

                #endregion

                return;

            }
            foreach (var dataObject in MemoryCell.Objects.Values)
            {
                var _object = dataObject._Object;
                var row = LoadObjectDataInTable(dataObject);
            }

        }

        /// <MetaDataID>{148fe77f-7cb3-4809-b26a-e81656421b11}</MetaDataID>
        private void CreateTableColumns(IDataTable dataTable)
        {
            string columnPrefix = DataNode.ValueTypePathDiscription;
            if (!string.IsNullOrEmpty(columnPrefix))
                columnPrefix += "_";
            ObjectIdentityType identityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });

            if (DataNode.Type != DataNode.DataNodeType.Object)
                return;
            if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
            {

            }
            else
            {
                IDataColumn[] keyColumns = new IDataColumn[1];
                keyColumns[0] = dataTable.Columns.Add("MC_ObjectID", typeof(System.Guid));//Add Object identity columns
                                                                                          // dataTable.PrimaryKey = keyColumns;

                #region Adds columns for the relation with the parent data node
                MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                if (associationEnd != null &&
                    (!HasRelationIdentityColumns || DataNode.Recursive))
                {
                    foreach (ObjectIdentityType objectIdentityType in associationEnd.GetReferenceObjectIdentityTypes(new List<ObjectIdentityType> { identityType }))
                    {
                        foreach (IIdentityPart part in objectIdentityType.Parts)
                            if (!dataTable.Columns.Contains(columnPrefix + part.Name))
                                dataTable.Columns.Add(columnPrefix + part.Name, part.Type);
                        //if (associationEnd.IsRoleA)
                        //    dataTable.Columns.Add(columnPrefix + associationEnd.Association.Name + "RoleB_OID", typeof(System.Guid));
                        //else
                        //    dataTable.Columns.Add(columnPrefix + associationEnd.Association.Name + "RoleA_OID", typeof(System.Guid));
                    }
                }

                #endregion
            }

            #region Adds columns for the relation with the subDataNodes
            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {

                MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                if (subDataNodeassociationEnd != null && HasRelationReferenceColumnsFor(subDataNode))
                {
                    foreach (ObjectIdentityType objectIdentityType in subDataNodeassociationEnd.GetOtherEnd().GetReferenceObjectIdentityTypes(new List<ObjectIdentityType> { identityType }))
                    {
                        foreach (IIdentityPart part in objectIdentityType.Parts)
                            if (!dataTable.Columns.Contains(columnPrefix + part.Name))
                                dataTable.Columns.Add(columnPrefix + part.Name, part.Type);
                        //if (associationEnd.IsRoleA)
                        //    dataTable.Columns.Add(columnPrefix + associationEnd.Association.Name + "RoleB_OID", typeof(System.Guid));
                        //else
                        //    dataTable.Columns.Add(columnPrefix + associationEnd.Association.Name + "RoleA_OID", typeof(System.Guid));
                    }
                    //if (subDataNodeassociationEnd.IsRoleA)
                    //{
                    //    if (!dataTable.Columns.Contains(columnPrefix + subDataNodeassociationEnd.Association.Name + "RoleA_OID"))
                    //        dataTable.Columns.Add(columnPrefix + subDataNodeassociationEnd.Association.Name + "RoleA_OID", typeof(System.Guid));
                    //}
                    //else
                    //{
                    //    if (!dataTable.Columns.Contains(columnPrefix + subDataNodeassociationEnd.Association.Name + "RoleB_OID"))
                    //        dataTable.Columns.Add(columnPrefix + subDataNodeassociationEnd.Association.Name + "RoleB_OID", typeof(System.Guid));
                    //}
                }
                else if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.Type == DataNode.DataNodeType.Object)
                {
                    (GetDataLoader(subDataNode) as MemroryObjectsDataLoader).CreateTableColumns(dataTable);
                }
            }
            #endregion

            #region Adds columns for the object Attributes
            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                    object Value = attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole");
                    bool IsAssociationClassRole = false;
                    if (Value != null)
                        IsAssociationClassRole = (bool)Value;
                    if (IsAssociationClassRole)
                    {
                        dataTable.Columns.Add(columnPrefix + subDataNode.Name, typeof(string));
                    }
                    else
                    {
                        System.Type type = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).Type.GetExtensionMetaObject<System.Type>();
                        dataTable.Columns.Add(columnPrefix + subDataNode.Name, type);
                        if (type == typeof(System.DateTime))
                        {
                            foreach (DataNode dateTimeDataNode in subDataNode.SubDataNodes)
                            {
                                System.Type columnType = dateTimeDataNode.Classifier.GetExtensionMetaObject<System.Type>();
                                dataTable.Columns.Add(columnPrefix + subDataNode.Name + "_" + dateTimeDataNode.Name, columnType);
                            }
                        }
                    }

                }
            }
            #endregion

            #region Adds columns for the object if it is necessary
            //  if (DataNode.ParticipateInSelectClause || DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
            {
                System.Type objectType = null;
                objectType = DataNode.Classifier.GetExtensionMetaObject<System.Type>();
                dataTable.Columns.Add(DataNode.ValueTypePathDiscription + "Object", objectType);
            }
            #endregion

            if (DataNode.ValueTypePath.Count == 0)
                dataTable.Columns.Add("OSM_StorageIdentity", typeof(int));



        }
        /// <summary>
        ///  If it is true the data source table has the identity coloumns 
        /// of foreign key relation, otherwise table has the reference columns. 
        /// Always the identity columns has unique constrain 
        /// </summary>
        /// <MetaDataID>{2d4904b3-cce2-4a0d-b455-806252b0c200}</MetaDataID>
        public override bool HasRelationIdentityColumns
        {
            get
            {


                if (DataNode.AssignedMetaObject is AssociationEnd)
                {
                    if (DataNode.ThroughRelationTable)
                        return true;
                    //if (DataNode.Recursive)
                    //    return false;

                    AssociationEnd associationEnd = DataNode.AssignedMetaObject as AssociationEnd;
                    if (!associationEnd.Multiplicity.IsMany &&  //zero ore one
                        associationEnd.GetOtherEnd().Multiplicity.IsMany)
                    {
                        return true;
                    }
                    else if (associationEnd.Association.MultiplicityType == AssociationType.ManyToMany &&
                        DataNode.Classifier.ClassHierarchyLinkAssociation != associationEnd.Association)
                    {
                        return true;
                    }
                    else if (associationEnd.Association.MultiplicityType == AssociationType.OneToOne &&
                       !associationEnd.IsRoleA)
                    {
                        return true;
                    }
                    else
                        return false;

                }
                else
                    return false;


            }
        }
        /// <MetaDataID>{5e5da815-6156-4e7c-9f5b-a32fdbf1d0c5}</MetaDataID>
        protected override System.Type GetColumnType(string columnName)
        {
            return Data.Columns[columnName].DataType;
        }



        ///// <MetaDataID>{c5eb3687-87d1-4072-974b-5fcf224acc12}</MetaDataID>
        //private void LoadObjectInTable(object parentNodeObject, object obj, System.Collections.Generic.Dictionary<int, object> values)
        //{
        //    System.Data.DataRow row = null;
        //    int OID = 0;
        //    if (obj != null && obj.GetType().IsValueType)
        //    {
        //        if (DataNodeObjects.ContainsKey(parentNodeObject))
        //            return;
        //        DataNodeObjects.Add(parentNodeObject, System.Guid.NewGuid());


        //        row = Data.NewRow();
        //        OID = parentNodeObject.GetHashCode();
        //        row["MC_ObjectID"] = OID;

        //    }
        //    else
        //    {
        //        if (DataNodeObjects.ContainsKey(obj))
        //            return;
        //        DataNodeObjects.Add(obj, System.Guid.NewGuid());


        //        row = Data.NewRow();
        //        OID = obj.GetHashCode();
        //        row["MC_ObjectID"] = OID;
        //    }

        //    //if ((DataNode.DataSource as RootObjectDataSource).HasLockRequest)
        //    //{
        //    //    System.Collections.Generic.List<Transactions.Transaction> transactions = Transactions.ObjectStateTransition.GetTransaction(obj);
        //    //    if (transactions.Count > 0 && transactions[0] != Transactions.Transaction.Current)
        //    //        row["Locked"] = transactions[0];
        //    //    else
        //    //        row["Locked"] = null;
        //    //}

        //    #region Loads object the object if it is necessary
        //    //if (DataNode.ParticipateInSelectClause || DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
        //    {
        //        System.Type objectType = DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type; ;
        //        int ObjID = OID;
        //        try
        //        {
        //            row[ObjectColumnIndex] = obj;
        //        }
        //        catch (System.Exception error)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion

        //    foreach (DataNode subDataNode in DataNode.SubDataNodes)
        //    {
        //        #region Loads columns for the object Attributes
        //        if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
        //        {
        //            object value = null;
        //            if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //            {
        //                value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
        //            }
        //            else
        //            {

        //                if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
        //                {
        //                    //value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember.GetValue(obj);
        //                    //value =Member<object>.GetValue(obj,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
        //                    value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
        //                }
        //            }
        //            row[subDataNode.Name] = value;
        //        }
        //        else if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is DotNetMetaDataRepository.Attribute)
        //        {
        //            object value = null;
        //            if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //            {
        //                value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
        //            }
        //            else
        //            {

        //                if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
        //                {
        //                    //value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember.GetValue(obj);
        //                    //value =Member<object>.GetValue(obj,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
        //                    value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
        //                }
        //            }
        //            (GetDataLoader(subDataNode) as RootObjectDataLoader).LoadObjectInRow(row, obj, value, values);
        //        }

        //        #endregion

        //        #region Loads columns for the relation with the subDataNodes
        //        if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.DataSource.HasRelationIdentityColumns)
        //        {
        //            MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //            if (subDataNodeassociationEnd != null)
        //            {
        //                if (Classifier.ClassHierarchyLinkAssociation == subDataNodeassociationEnd.Association)
        //                {
        //                    if (Classifier is DotNetMetaDataRepository.Interface)
        //                    {
        //                        if (subDataNodeassociationEnd.IsRoleA)
        //                        {
        //                            object roleAObject = (Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);
        //                            if (roleAObject != null)
        //                                row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleAObject.GetHashCode();
        //                        }
        //                        else
        //                        {
        //                            object roleBObject = (Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);
        //                            if (roleBObject != null)
        //                                row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleBObject.GetHashCode();
        //                        }

        //                    }
        //                    if (Classifier is DotNetMetaDataRepository.Class)
        //                    {
        //                        if (subDataNodeassociationEnd.IsRoleA)
        //                        {
        //                            //object roleAObject = (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
        //                            //object roleAObject = Member<object>.GetValue(obj, (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
        //                            object roleAObject = Member<object>.GetValue((Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
        //                            if (roleAObject != null)
        //                                row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleAObject.GetHashCode();
        //                        }
        //                        else
        //                        {
        //                            //object roleBObject = (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
        //                            //object roleBObject = Member<object>.GetValue(obj, (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
        //                            object roleBObject = Member<object>.GetValue((Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);
        //                            if (roleBObject != null)
        //                                row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleBObject.GetHashCode();
        //                        }

        //                    }

        //                }
        //                else
        //                {
        //                    System.Collections.Generic.Dictionary<int, object> objects = new System.Collections.Generic.Dictionary<int, object>();
        //                    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in subDataNode.DataSource.DataLoaders)
        //                    {
        //                        DataLoader dataLoader = entry.Value;
        //                        (dataLoader as RootObjectDataLoader).GetDataNodeObjects(obj, DataNode, objects);
        //                    }
        //                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
        //                       ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
        //                       (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Specializations.Count > 0) &&
        //                       (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association != DataNode.Classifier.ClassHierarchyLinkAssociation)
        //                    {
        //                        AssotiationTableRelationshipData relationshipData = RelationshipsData[subDataNode.Identity];

        //                        foreach (object item in objects.Values)
        //                        {
        //                            System.Data.DataRow associationRow = relationshipData.Data.NewRow();
        //                            if (subDataNodeassociationEnd.IsRoleA)
        //                            {
        //                                associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = item.GetHashCode();
        //                                associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = OID;
        //                            }
        //                            else
        //                            {
        //                                associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = item.GetHashCode(); ;
        //                                associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = OID;
        //                            }
        //                            relationshipData.Data.Rows.Add(associationRow);


        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (HasRelationReferenceColumnsFor(subDataNode))
        //                        {
        //                            if (objects.Count > 1)
        //                                throw new System.Exception("Invalid relation");
        //                            if (objects.Count > 0)
        //                            {

        //                                foreach (object item in objects.Values)
        //                                {
        //                                    if (subDataNodeassociationEnd.IsRoleA)
        //                                        row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = item.GetHashCode();
        //                                    else
        //                                        row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = item.GetHashCode();
        //                                    break;

        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //            //else if (subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
        //            //{
        //            //    System.Collections.ArrayList objects = new System.Collections.ArrayList();
        //            //    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in subDataNode.DataSource.DataLoaders)
        //            //    {
        //            //        DataLoader dataLoader = entry.Value;
        //            //        (dataLoader as RootObjectDataLoader).GetDataNodeObjects(obj, DataNode, objects);
        //            //    }

        //            //    if (objects.Count > 1)
        //            //        throw new System.Exception("Invalid relation");
        //            //    if (objects.Count > 0)
        //            //        row[subDataNode.AssignedMetaObject.Name + "RoleA_OID"] = objects[0].GetHashCode();
        //            //}
        //        }
        //        #endregion


        //    }


        //    #region Loads with columns for the relation with the parent data node
        //    MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //    if (associationEnd != null && HasRelationReferenceColumns)
        //    //!HasRelationIdentityColumns &&
        //    //!(associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
        //    //associationEnd.Association != DataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation))
        //    {
        //        if (associationEnd.IsRoleA)
        //            row[DataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleB_OID"] = parentNodeObject.GetHashCode();
        //        else
        //            row[DataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleA_OID"] = parentNodeObject.GetHashCode();

        //    }
        //    else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
        //    {
        //        row[(DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name + "RoleB_OID"] = parentNodeObject.GetHashCode();
        //    }


        //    #endregion



        //    //if (DataNode.LocalFilter == null)
        //    Data.Rows.Add(row);
        //    //else if (DataNode.LocalFilter.DoesRowPassCondition(row, DataNode))
        //    //    Data.Rows.Add(row);
        //    if (DataNode.Path != null && DataNode.Path.Recursive && RecursiveStep < DataNode.Path.RecursiveSteps)
        //    {
        //        RecursiveStep++;
        //        GetDataNodeObjects(obj, DataNode.ParentDataNode, values);
        //        RecursiveStep--;
        //    }
        //}

        static ObjectIdentityType IdentityType = new ObjectIdentityType(new List<IIdentityPart>() { new IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
        private IDataRow LoadObjectDataInTable(ObjectData objectData)
        {

            AssociationEnd associationEnd = DataNode.AssignedMetaObject as AssociationEnd;
            IDataRow row = null;
            System.Guid OID;
            System.Guid parentNodeObjectID = System.Guid.Empty;
            if (objectData.ParentObjectID != null)
                parentNodeObjectID = objectData.ParentObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
            bool newRow = false;
            object obj = objectData._Object;
            if (obj != null && obj.GetType().GetMetaData().IsValueType)
            {

                OID = objectData.ParentObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
                row = Data.Rows.Find(OID);
            }
            else
            {

                if (DataNodeObjects.ContainsKey(obj))
                {
                    if (DataNode.ThroughRelationTable)
                    {
                        OID = DataNodeObjects[obj];
                        AssotiationTableRelationshipData relationshipData = _ParentRelationshipData;

                        foreach (var parentObjectData in objectData.ParentDataNodeRelatedObjects.Values)
                        {
                            IDataRow associationRow = relationshipData.Data.NewRow();
                            associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.GetReferenceObjectIdentityType(IdentityType).Parts[0].Name] = parentObjectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
                            associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.GetOtherEnd().GetReferenceObjectIdentityType(IdentityType).Parts[0].Name] = OID;
                            relationshipData.Data.Rows.Add(associationRow);
                        }
                    }
                    return null;
                }
                else
                {
                    //DataNodeObjects.Add(obj, System.Guid.NewGuid());
                    row = Data.NewRow();
                    newRow = true;
                    OID = objectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
                    row["MC_ObjectID"] = OID;
                    if (DataNode.ThroughRelationTable)
                    {
                        AssotiationTableRelationshipData relationshipData = _ParentRelationshipData;

                        foreach (var parentObjectData in objectData.ParentDataNodeRelatedObjects.Values)
                        {
                            IDataRow associationRow = relationshipData.Data.NewRow();
                            associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.GetReferenceObjectIdentityType(IdentityType).Parts[0].Name] = parentObjectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
                            associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.GetOtherEnd().GetReferenceObjectIdentityType(IdentityType).Parts[0].Name] = OID;


                            //if (associationEnd.IsRoleA)
                            //{
                            //    associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleA_OID"] = OID;
                            //    associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleB_OID"] = parentObjectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID"); ;
                            //}
                            //else
                            //{
                            //    associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleB_OID"] = OID;
                            //    associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleA_OID"] = parentObjectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID"); ;
                            //}
                            relationshipData.Data.Rows.Add(associationRow);
                        }
                        //if (associationEnd.IsRoleA)
                        //{
                        //    associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleA_OID"] = OID;
                        //    associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleB_OID"] = parentNodeObjectID;
                        //}
                        //else
                        //{
                        //    associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleB_OID"] = OID;
                        //    associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.Association.Name + "RoleA_OID"] = parentNodeObjectID;
                        //}

                    }
                    if (DataNode.Recursive)
                    {
                        row[DataNode.ValueTypePathDiscription + associationEnd.GetReferenceObjectIdentityType(IdentityType).Parts[0].Name] = parentNodeObjectID;
                    }
                }
            }

            row["OSM_StorageIdentity"] = (DataNode.ObjectQuery as DistributedObjectQuery).QueryStorageIdentities.IndexOf(DataLoaderMetadata.ObjectsContextIdentity);



            #region Loads object the object if it is necessary
            //if (DataNode.ParticipateInSelectClause || DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
            {
                System.Type objectType = DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type; ;
                //int ObjID = OID;
                try
                {
                    row[ObjectColumnIndex] = obj;
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            #endregion

            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                #region Loads columns for the object Attributes
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    object value = null;
                    if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                        value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
                    else
                    {

                        if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
                            value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                    }
                    row[DataNode.ValueTypePathDiscription + subDataNode.Name] = value;
                    //if(subDataNode.Classifier.GetExtensionMetaObject<Type>()==typeof(DateTime))
                    foreach (DataNode dateTimeDataNode in subDataNode.SubDataNodes)
                    {
                        object memberValue = null;
                        if (value != null)
                        {
                            if ((dateTimeDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                                memberValue = (dateTimeDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(value);
                            else
                            {

                                if ((dateTimeDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
                                    memberValue = Member<object>.GetValue((dateTimeDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, value);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                            }
                            row[(this.DataNode.DataSource as StorageDataSource).GetColumnIndex(dateTimeDataNode)] = memberValue;

                        }
                        //  System.Type columnType = dateTimeDataNode.Classifier.GetExtensionMetaObject<System.Type>();
                        // dataTable.Columns.Add(columnPrefix + subDataNode.Name + "_" + dateTimeDataNode.Name, columnType);
                    }
                }
                else if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is DotNetMetaDataRepository.Attribute)
                {
                    object value = null;
                    if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                    {
                        value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
                    }
                    else
                    {

                        if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
                        {
                            //value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember.GetValue(obj);
                            //value =Member<object>.GetValue(obj,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                            value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                        }
                    }
                    (GetDataLoader(subDataNode) as MemroryObjectsDataLoader).LoadObjectDataInRow(row, obj, value);
                }

                #endregion

                if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.ValueTypePath.Count == 0 && HasRelationReferenceColumnsFor(subDataNode))
                {
                    var relatedObjects = objectData.RelatedObjects[subDataNode.AssignedMetaObject.Identity.ToString()];
                    if (relatedObjects.Count > 0)
                    {
                        foreach (ObjectData relateObjectData in relatedObjects.Values)
                        {
                            var objectID = relateObjectData.ObjectID;
                            int i = 0;
                            foreach (var columnName in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNode.AssignedMetaObject.Identity.ToString()][objectID.ObjectIdentityType].ObjectIdentityColumns)
                                row[columnName] = objectID.GetPartValue(i++);

                            break;
                        }

                    }
                }
            }


            #region Loads with columns for the relation with the parent data node
            if (associationEnd != null && HasRelationReferenceColumns)
            {

                row[DataNode.ValueTypePathDiscription + associationEnd.GetReferenceObjectIdentityType(IdentityType).Parts[0].Name] = parentNodeObjectID;

            }
            //else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
            //{
            //    row[(DataNode.AssignedMetaObject as MetaDataRepository.Attribute).Name + "RoleB_OID"] = parentNodeObjectID;
            //}


            #endregion



            //if (DataNode.LocalFilter == null)
            if (newRow)
                Data.Rows.Add(row);
            //else if (DataNode.LocalFilter.DoesRowPassCondition(row, DataNode))


            return row;
        }


        ///// <MetaDataID>{f4b6b800-d36f-4024-b1a9-306d4dbd0fa8}</MetaDataID>
        //private void LoadObjectInRow(System.Data.DataRow row, object parentNodeObject, object obj, System.Collections.Generic.Dictionary<int, object> values)
        //{


        //    int OID = (int)row["MC_ObjectID"];


        //    //if ((DataNode.DataSource as RootObjectDataSource).HasLockRequest)
        //    //{
        //    //    System.Collections.Generic.List<Transactions.Transaction> transactions = Transactions.ObjectStateTransition.GetTransaction(obj);
        //    //    if (transactions.Count > 0 && transactions[0] != Transactions.Transaction.Current)
        //    //        row["Locked"] = transactions[0];
        //    //    else
        //    //        row["Locked"] = null;
        //    //}

        //    #region Loads object the object if it is necessary
        //    //if (DataNode.ParticipateInSelectClause || DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
        //    {
        //        System.Type objectType = DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type; ;
        //        int ObjID = OID;
        //        try
        //        {
        //            row[ObjectColumnIndex] = obj;
        //        }
        //        catch (System.Exception error)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion

        //    foreach (DataNode subDataNode in DataNode.SubDataNodes)
        //    {
        //        #region Loads columns for the object Attributes
        //        if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
        //        {
        //            object value = null;
        //            if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //            {
        //                value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
        //            }
        //            else
        //            {

        //                if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
        //                {
        //                    //value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember.GetValue(obj);
        //                    //value =Member<object>.GetValue(obj,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
        //                    value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
        //                }
        //            }
        //            row[DataNode.ValueTypePathDiscription + subDataNode.Name] = value;
        //        }
        //        else if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is DotNetMetaDataRepository.Attribute)
        //        {
        //            object value = null;
        //            if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //            {
        //                value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
        //            }
        //            else
        //            {

        //                if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
        //                {
        //                    //value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember.GetValue(obj);
        //                    //value =Member<object>.GetValue(obj,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
        //                    value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
        //                }
        //            }
        //            (GetDataLoader(subDataNode) as RootObjectDataLoader).LoadObjectInRow(row, obj, value, values);
        //        }

        //        #endregion

        //        #region Loads columns for the relation with the subDataNodes
        //        if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.DataSource.HasRelationIdentityColumns)
        //        {
        //            MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //            if (subDataNodeassociationEnd != null)
        //            {
        //                if (Classifier.ClassHierarchyLinkAssociation == subDataNodeassociationEnd.Association)
        //                {
        //                    if (Classifier is DotNetMetaDataRepository.Interface)
        //                    {
        //                        if (subDataNodeassociationEnd.IsRoleA)
        //                        {
        //                            object roleAObject = (Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);
        //                            if (roleAObject != null)
        //                                row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleAObject.GetHashCode();
        //                        }
        //                        else
        //                        {
        //                            object roleBObject = (Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);
        //                            if (roleBObject != null)
        //                                row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleBObject.GetHashCode();
        //                        }

        //                    }
        //                    if (Classifier is DotNetMetaDataRepository.Class)
        //                    {
        //                        if (subDataNodeassociationEnd.IsRoleA)
        //                        {
        //                            //object roleAObject = (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
        //                            //object roleAObject = Member<object>.GetValue(obj, (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
        //                            object roleAObject = Member<object>.GetValue((Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
        //                            if (roleAObject != null)
        //                                row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleAObject.GetHashCode();
        //                        }
        //                        else
        //                        {
        //                            //object roleBObject = (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
        //                            //object roleBObject = Member<object>.GetValue(obj, (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
        //                            object roleBObject = Member<object>.GetValue((Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);
        //                            if (roleBObject != null)
        //                                row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleBObject.GetHashCode();
        //                        }

        //                    }

        //                }
        //                else
        //                {
        //                    System.Collections.Generic.Dictionary<int, object> objects = new System.Collections.Generic.Dictionary<int, object>();
        //                    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in subDataNode.DataSource.DataLoaders)
        //                    {
        //                        DataLoader dataLoader = entry.Value;
        //                        (dataLoader as RootObjectDataLoader).GetDataNodeObjects(obj, DataNode, objects);
        //                    }
        //                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
        //                       ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
        //                       (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Specializations.Count > 0) &&
        //                       (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association != DataNode.Classifier.ClassHierarchyLinkAssociation)
        //                    {
        //                        AssotiationTableRelationshipData relationshipData = RelationshipsData[subDataNode.Identity];

        //                        foreach (object item in objects.Values)
        //                        {
        //                            System.Data.DataRow associationRow = relationshipData.Data.NewRow();
        //                            if (subDataNodeassociationEnd.IsRoleA)
        //                            {
        //                                associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = item.GetHashCode();
        //                                associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = OID;
        //                            }
        //                            else
        //                            {
        //                                associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = item.GetHashCode(); ;
        //                                associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = OID;
        //                            }
        //                            relationshipData.Data.Rows.Add(associationRow);


        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (objects.Count > 1)
        //                            throw new System.Exception("Invalid relation");
        //                        if (objects.Count > 0)
        //                        {

        //                            foreach (object item in objects.Values)
        //                            {

        //                                if (subDataNodeassociationEnd.IsRoleA)
        //                                    row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = item.GetHashCode();
        //                                else
        //                                    row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = item.GetHashCode();
        //                                break;

        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //            //else if (subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
        //            //{
        //            //    System.Collections.ArrayList objects = new System.Collections.ArrayList();
        //            //    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in subDataNode.DataSource.DataLoaders)
        //            //    {
        //            //        DataLoader dataLoader = entry.Value;
        //            //        (dataLoader as RootObjectDataLoader).GetDataNodeObjects(obj, DataNode, objects);
        //            //    }

        //            //    if (objects.Count > 1)
        //            //        throw new System.Exception("Invalid relation");
        //            //    if (objects.Count > 0)
        //            //        row[subDataNode.AssignedMetaObject.Name + "RoleA_OID"] = objects[0].GetHashCode();
        //            //}
        //        }
        //        #endregion
        //    }

        //}

        public void CreateParentRelationshipData()
        {

            AssociationEnd associationEnd = DataNode.AssignedMetaObject as AssociationEnd;
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>> parentRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>> childRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ObjectIdentityType>>();
            parentRelationColumns[associationEnd.Identity.ToString()] = new System.Collections.Generic.List<ObjectIdentityType>();
            childRelationColumns[associationEnd.Identity.ToString()] = new System.Collections.Generic.List<ObjectIdentityType>();

            parentRelationColumns[associationEnd.Identity.ToString()].Add(associationEnd.GetReferenceObjectIdentityType(IdentityType));
            childRelationColumns[associationEnd.Identity.ToString()].Add(associationEnd.GetOtherEnd().GetReferenceObjectIdentityType(IdentityType));
            _ParentRelationshipData = new AssotiationTableRelationshipData(parentRelationColumns, childRelationColumns, DataSource.DataObjectsInstantiator.CreateDataTable(), associationEnd.GetOtherEnd().Role, DataNode.ParentDataNode.Identity, DataNode.Identity);

        }


        private void LoadObjectDataInRow(IDataRow row, object parentNodeObject, object obj)
        {
            System.Guid OID = (System.Guid)row["MC_ObjectID"];
            #region Loads object the object if it is necessary
            //if (DataNode.ParticipateInSelectClause || DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
            {
                System.Type objectType = DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type; ;
                System.Guid ObjID = OID;
                try
                {
                    row[ObjectColumnIndex] = obj;
                }
                catch (System.Exception error)
                {
                    throw;
                }
            }
            #endregion

            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                string columnPrefix = DataNode.ValueTypePathDiscription;
                if (!string.IsNullOrEmpty(columnPrefix))
                    columnPrefix += "_";

                #region Loads columns for the object Attributes
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    object value = null;
                    if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                    {
                        value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
                    }
                    else
                    {

                        if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
                        {
                            //value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember.GetValue(obj);
                            //value =Member<object>.GetValue(obj,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                            value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                        }
                    }
                    row[columnPrefix + subDataNode.Name] = value;
                }
                else if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is DotNetMetaDataRepository.Attribute)
                {
                    object value = null;
                    if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                    {
                        value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
                    }
                    else
                    {

                        if ((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember != null)
                        {
                            //value = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember.GetValue(obj);
                            //value =Member<object>.GetValue(obj,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                            value = Member<object>.GetValue((subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);//, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FieldMember);
                        }
                    }
                    (GetDataLoader(subDataNode) as MemroryObjectsDataLoader).LoadObjectDataInRow(row, obj, value);
                }

                #endregion

                #region Loads columns for the relation with the subDataNodes
                if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.DataSource.HasRelationIdentityColumns)
                {
                    MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    if (subDataNodeassociationEnd != null)
                    {
                        if (Classifier.ClassHierarchyLinkAssociation == subDataNodeassociationEnd.Association)
                        {
                            if (Classifier is DotNetMetaDataRepository.Interface)
                            {
                                if (subDataNodeassociationEnd.IsRoleA)
                                {
                                    object roleAObject = (Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);
                                    if (roleAObject != null)
                                        row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleAObject.GetHashCode();
                                }
                                else
                                {
                                    object roleBObject = (Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);
                                    if (roleBObject != null)
                                        row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleBObject.GetHashCode();
                                }

                            }
                            if (Classifier is DotNetMetaDataRepository.Class)
                            {
                                if (subDataNodeassociationEnd.IsRoleA)
                                {
                                    //object roleAObject = (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
                                    //object roleAObject = Member<object>.GetValue(obj, (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
                                    object roleAObject = Member<object>.GetValue((Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
                                    if (roleAObject != null)
                                        row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleAObject.GetHashCode();
                                }
                                else
                                {
                                    //object roleBObject = (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
                                    //object roleBObject = Member<object>.GetValue(obj, (Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
                                    object roleBObject = Member<object>.GetValue((Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);
                                    if (roleBObject != null)
                                        row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleBObject.GetHashCode();
                                }

                            }

                        }
                        else
                        {
                            System.Collections.Generic.Dictionary<int, object> objects = new System.Collections.Generic.Dictionary<int, object>();
                            //foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in subDataNode.DataSource.DataLoaders)
                            //{
                            //    DataLoader dataLoader = entry.Value;
                            //    (dataLoader as RootObjectDataLoader).GetDataNodeObjects(obj, DataNode, objects);
                            //}
                            if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&
                               ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                               (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Specializations.Count > 0) &&
                               (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association != DataNode.Classifier.ClassHierarchyLinkAssociation)
                            {
                                AssotiationTableRelationshipData relationshipData = RelationshipsData[subDataNode.Identity];

                                foreach (object item in objects.Values)
                                {
                                    IDataRow associationRow = relationshipData.Data.NewRow();
                                    if (subDataNodeassociationEnd.IsRoleA)
                                    {
                                        associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = item.GetHashCode();
                                        associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = OID;
                                    }
                                    else
                                    {
                                        associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = item.GetHashCode(); ;
                                        associationRow[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = OID;
                                    }
                                    relationshipData.Data.Rows.Add(associationRow);


                                }
                            }
                            else
                            {
                                if (objects.Count > 1)
                                    throw new System.Exception("Invalid relation");
                                if (objects.Count > 0)
                                {

                                    foreach (object item in objects.Values)
                                    {

                                        if (subDataNodeassociationEnd.IsRoleA)
                                            row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = item.GetHashCode();
                                        else
                                            row[DataNode.ValueTypePathDiscription + subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = item.GetHashCode();
                                        break;

                                    }
                                }
                            }

                        }
                    }
                    //else if (subDataNode.AssignedMetaObject is OOAdvantech.MetaDataRepository.Attribute)
                    //{
                    //    System.Collections.ArrayList objects = new System.Collections.ArrayList();
                    //    foreach (System.Collections.Generic.KeyValuePair<string, DataLoader> entry in subDataNode.DataSource.DataLoaders)
                    //    {
                    //        DataLoader dataLoader = entry.Value;
                    //        (dataLoader as RootObjectDataLoader).GetDataNodeObjects(obj, DataNode, objects);
                    //    }

                    //    if (objects.Count > 1)
                    //        throw new System.Exception("Invalid relation");
                    //    if (objects.Count > 0)
                    //        row[subDataNode.AssignedMetaObject.Name + "RoleA_OID"] = objects[0].GetHashCode();
                    //}
                }
                #endregion
            }

        }

        /// <MetaDataID>{cfc908c4-7c46-4e83-999d-b06b44e99afb}</MetaDataID>
        int RecursiveStep = 0;
        /// <MetaDataID>{532de020-06f0-490d-8c0c-8307b77c5a88}</MetaDataID>
        System.Collections.Generic.Dictionary<object, System.Guid> DataNodeObjects = new Dictionary<object, System.Guid>();


        ///// <MetaDataID>{83dcbb01-c124-4783-8121-ce811d1c7d67}</MetaDataID>
        //public void GetDataNodeObjects(object obj, DataNode dataNode, System.Collections.Generic.Dictionary<int, object> values)
        //{
        //    try
        //    {
        //        if (dataNode == DataNode && !values.ContainsKey(obj.GetHashCode()))
        //        {
        //            LoadObjectInTable(null, obj, values);
        //            values.Add(obj.GetHashCode(), obj);
        //            return;
        //        }
        //        foreach (DataNode subDataNode in dataNode.SubDataNodes)
        //        {
        //            if (this.DataNode.IsPathNode(subDataNode))
        //            {


        //                if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
        //                {
        //                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //                    object memberObject = null;

        //                    if (dataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
        //                    {
        //                        if (associationEnd.IsRoleA)
        //                        {
        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                                memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);

        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                            {
        //                                //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
        //                                //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
        //                                memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                                memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);

        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                            {
        //                                //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
        //                                //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
        //                                memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);

        //                            }
        //                        }
        //                        GetDataNodeObjects(memberObject, subDataNode, values);
        //                    }
        //                    else
        //                    {
        //                        if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember != null)
        //                            memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(obj);
        //                        else
        //                        {
        //                            //System.Reflection.FieldInfo fieldInfo = associationEnd.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
        //                            //memberObject = fieldInfo.GetValue(obj);

        //                            //memberObject = Member<object>.GetValue(obj, fieldInfo);
        //                            memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, obj);
        //                        }
        //                        if (memberObject == null)
        //                            return;
        //                        if (associationEnd.CollectionClassifier != null)
        //                        {

        //                            System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                            enumerator.Reset();
        //                            while (enumerator.MoveNext())
        //                            {
        //                                object collectionObj = enumerator.Current;
        //                                GetDataNodeObjects(collectionObj, subDataNode, values);
        //                            }

        //                        }
        //                        else
        //                        {
        //                            GetDataNodeObjects(memberObject, subDataNode, values);

        //                        }
        //                    }
        //                }

        //                if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.DataSource != null)
        //                {
        //                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
        //                    object memberObject = null;

        //                    if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //                        memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
        //                    else
        //                        memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue(obj);
        //                    if (memberObject == null)
        //                        return;
        //                    {
        //                        if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
        //                        {
        //                            if (memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]) != null
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.IsGenericType
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments().Length == 1
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments()[0] == subDataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type)
        //                            {
        //                                System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                                enumerator.Reset();
        //                                while (enumerator.MoveNext())
        //                                {
        //                                    object collectionObj = enumerator.Current;
        //                                    GetDataNodeObjects(collectionObj, subDataNode, values);

        //                                }
        //                            }
        //                            else
        //                            {
        //                                GetDataNodeObjects(memberObject, subDataNode, values);
        //                            }
        //                        }
        //                    }
        //                }





        //                return;

        //            }
        //            if (subDataNode == DataNode)
        //            {

        //                if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
        //                {
        //                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //                    object memberObject = null;

        //                    if (dataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
        //                    {
        //                        if (associationEnd.IsRoleA)
        //                        {
        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                                memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);

        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                            {
        //                                //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
        //                                //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
        //                                memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                                memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);

        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                            {
        //                                //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
        //                                //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
        //                                memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);
        //                            }
        //                        }
        //                        if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
        //                        {
        //                            LoadObjectInTable(obj, memberObject, values);
        //                            values.Add(memberObject.GetHashCode(), memberObject);
        //                        }




        //                    }
        //                    else
        //                    {



        //                        if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember != null)
        //                            memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(obj);
        //                        else
        //                        {
        //                            //System.Reflection.FieldInfo fieldInfo = associationEnd.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
        //                            //memberObject = fieldInfo.GetValue(obj);
        //                            //memberObject = Member<object>.GetValue(obj, fieldInfo);
        //                            memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, obj);
        //                        }
        //                        if (memberObject == null)
        //                            return;
        //                        if (associationEnd.CollectionClassifier != null)
        //                        {

        //                            System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                            enumerator.Reset();
        //                            while (enumerator.MoveNext())
        //                            {
        //                                object collectionObj = enumerator.Current;
        //                                if (collectionObj != null && !values.ContainsKey(collectionObj.GetHashCode()))
        //                                {
        //                                    LoadObjectInTable(obj, collectionObj, values);
        //                                    values.Add(collectionObj.GetHashCode(), collectionObj);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
        //                            {
        //                                LoadObjectInTable(obj, memberObject, values);
        //                                values.Add(memberObject.GetHashCode(), memberObject);
        //                            }
        //                        }
        //                    }


        //                    return;

        //                }
        //                if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.DataSource != null)
        //                {
        //                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
        //                    object memberObject = null;
        //                    object propertyMetadata = attribute.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo));
        //                    System.Type memberType = null;

        //                    if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //                        memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
        //                    else
        //                    {
        //                        //System.Reflection.FieldInfo fieldInfo = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
        //                        ////memberObject = fieldInfo.GetValue(obj);
        //                        //memberObject = Member<object>.GetValue(obj, fieldInfo);
        //                        memberObject = Member<object>.GetValue((attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);

        //                    }

        //                    if (memberObject == null)
        //                        return;
        //                    {
        //                        if (memberObject != null && !memberObject.GetType().IsValueType && !values.ContainsKey(memberObject.GetHashCode()))
        //                        {
        //                            if (memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]) != null
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.IsGenericType
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments().Length == 1
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments()[0] == DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type)
        //                            {
        //                                System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                                enumerator.Reset();
        //                                while (enumerator.MoveNext())
        //                                {
        //                                    object collectionObj = enumerator.Current;
        //                                    if (collectionObj != null && !values.ContainsKey(collectionObj.GetHashCode()))
        //                                    {
        //                                        LoadObjectInTable(obj, collectionObj, values);
        //                                        values.Add(collectionObj.GetHashCode(), collectionObj);
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                LoadObjectInTable(obj, memberObject, values);
        //                                values.Add(memberObject.GetHashCode(), memberObject);
        //                            }
        //                        }
        //                        if (memberObject != null && memberObject.GetType().IsValueType)
        //                        {
        //                            LoadObjectInTable(obj, memberObject, values);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception Error)
        //    {
        //        throw;

        //    }
        //}



        //public void LoadRecursiveDataNodeObjects(System.Guid parentNodeObjectID, object obj)
        //{
        //    try
        //    {
        //        if (!DataNodeObjects.ContainsKey(obj))
        //        {
        //            LoadObjectDataInTable(parentNodeObjectID, obj);
        //            return;
        //        }
        //        foreach (DataNode subDataNode in dataNode.SubDataNodes)
        //        {
        //            if (this.DataNode.IsPathNode(subDataNode))
        //            {


        //                if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
        //                {
        //                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //                    object memberObject = null;

        //                    if (dataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
        //                    {
        //                        if (associationEnd.IsRoleA)
        //                        {
        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                                memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);

        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                            {
        //                                //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
        //                                //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
        //                                memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                                memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);

        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                            {
        //                                //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
        //                                //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
        //                                memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);

        //                            }
        //                        }
        //                        GetDataNodeObjects(memberObject, subDataNode, values);
        //                    }
        //                    else
        //                    {
        //                        if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember != null)
        //                            memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(obj);
        //                        else
        //                        {
        //                            //System.Reflection.FieldInfo fieldInfo = associationEnd.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
        //                            //memberObject = fieldInfo.GetValue(obj);

        //                            //memberObject = Member<object>.GetValue(obj, fieldInfo);
        //                            memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, obj);
        //                        }
        //                        if (memberObject == null)
        //                            return;
        //                        if (associationEnd.CollectionClassifier != null)
        //                        {

        //                            System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                            enumerator.Reset();
        //                            while (enumerator.MoveNext())
        //                            {
        //                                object collectionObj = enumerator.Current;
        //                                GetDataNodeObjects(collectionObj, subDataNode, values);
        //                            }

        //                        }
        //                        else
        //                        {
        //                            GetDataNodeObjects(memberObject, subDataNode, values);

        //                        }
        //                    }
        //                }

        //                if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.DataSource != null)
        //                {
        //                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
        //                    object memberObject = null;

        //                    if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //                        memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
        //                    else
        //                        memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue(obj);
        //                    if (memberObject == null)
        //                        return;
        //                    {
        //                        if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
        //                        {
        //                            if (memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]) != null
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.IsGenericType
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments().Length == 1
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments()[0] == subDataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type)
        //                            {
        //                                System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                                enumerator.Reset();
        //                                while (enumerator.MoveNext())
        //                                {
        //                                    object collectionObj = enumerator.Current;
        //                                    GetDataNodeObjects(collectionObj, subDataNode, values);

        //                                }
        //                            }
        //                            else
        //                            {
        //                                GetDataNodeObjects(memberObject, subDataNode, values);
        //                            }
        //                        }
        //                    }
        //                }





        //                return;

        //            }
        //            if (subDataNode == DataNode)
        //            {

        //                if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
        //                {
        //                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
        //                    object memberObject = null;

        //                    if (dataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
        //                    {
        //                        if (associationEnd.IsRoleA)
        //                        {
        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                                memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(obj, null);

        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                            {
        //                                //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField.GetValue(obj);
        //                                //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAField);
        //                                memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, obj);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Interface)
        //                                memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(obj, null);

        //                            if (dataNode.Classifier is DotNetMetaDataRepository.Class)
        //                            {
        //                                //memberObject = (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField.GetValue(obj);
        //                                //memberObject = Member<object>.GetValue(obj, (dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBField);
        //                                memberObject = Member<object>.GetValue((dataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, obj);
        //                            }
        //                        }
        //                        if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
        //                        {
        //                            LoadObjectInTable(obj, memberObject, values);
        //                            values.Add(memberObject.GetHashCode(), memberObject);
        //                        }




        //                    }
        //                    else
        //                    {



        //                        if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember != null)
        //                            memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(obj);
        //                        else
        //                        {
        //                            //System.Reflection.FieldInfo fieldInfo = associationEnd.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
        //                            //memberObject = fieldInfo.GetValue(obj);
        //                            //memberObject = Member<object>.GetValue(obj, fieldInfo);
        //                            memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, obj);
        //                        }
        //                        if (memberObject == null)
        //                            return;
        //                        if (associationEnd.CollectionClassifier != null)
        //                        {

        //                            System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                            enumerator.Reset();
        //                            while (enumerator.MoveNext())
        //                            {
        //                                object collectionObj = enumerator.Current;
        //                                if (collectionObj != null && !values.ContainsKey(collectionObj.GetHashCode()))
        //                                {
        //                                    LoadObjectInTable(obj, collectionObj, values);
        //                                    values.Add(collectionObj.GetHashCode(), collectionObj);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (memberObject != null && !values.ContainsKey(memberObject.GetHashCode()))
        //                            {
        //                                LoadObjectInTable(obj, memberObject, values);
        //                                values.Add(memberObject.GetHashCode(), memberObject);
        //                            }
        //                        }
        //                    }


        //                    return;

        //                }
        //                if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.DataSource != null)
        //                {
        //                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
        //                    object memberObject = null;
        //                    object propertyMetadata = attribute.GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo));
        //                    System.Type memberType = null;

        //                    if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember != null)
        //                        memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(obj);
        //                    else
        //                    {
        //                        //System.Reflection.FieldInfo fieldInfo = attribute.GetExtensionMetaObject(typeof(System.Reflection.FieldInfo)) as System.Reflection.FieldInfo;
        //                        ////memberObject = fieldInfo.GetValue(obj);
        //                        //memberObject = Member<object>.GetValue(obj, fieldInfo);
        //                        memberObject = Member<object>.GetValue((attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, obj);

        //                    }

        //                    if (memberObject == null)
        //                        return;
        //                    {
        //                        if (memberObject != null && !memberObject.GetType().IsValueType && !values.ContainsKey(memberObject.GetHashCode()))
        //                        {
        //                            if (memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]) != null
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.IsGenericType
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments().Length == 1
        //                                && memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetGenericArguments()[0] == DataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type)
        //                            {
        //                                System.Collections.IEnumerator enumerator = memberObject.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
        //                                enumerator.Reset();
        //                                while (enumerator.MoveNext())
        //                                {
        //                                    object collectionObj = enumerator.Current;
        //                                    if (collectionObj != null && !values.ContainsKey(collectionObj.GetHashCode()))
        //                                    {
        //                                        LoadObjectInTable(obj, collectionObj, values);
        //                                        values.Add(collectionObj.GetHashCode(), collectionObj);
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                LoadObjectInTable(obj, memberObject, values);
        //                                values.Add(memberObject.GetHashCode(), memberObject);
        //                            }
        //                        }
        //                        if (memberObject != null && memberObject.GetType().IsValueType)
        //                        {
        //                            LoadObjectInTable(obj, memberObject, values);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception Error)
        //    {
        //        throw;

        //    }
        //}

        ///// <summary>
        ///// Return the member metadata form parameter type with name the parameter memberName.
        ///// If the there isn't then return null. If there are more than one in hierarchy then return the member which
        ///// is declared in type of parameter type.
        ///// Method is useful for member which is proprty or field.
        ///// </summary>
        ///// <param name="type">Defines the type where method look for member</param>
        ///// <param name="memberName">Defines the member name</param>
        ///// <returns>Member metadata object</returns>
        //static private System.Reflection.MemberInfo GetMember(System.Type type, string memberName)
        //{
        //    System.Reflection.MemberInfo[] members = type.GetMember(memberName);
        //    if (members.Length > 0)
        //        return members[0];
        //    else
        //    {
        //        OOAdvantech.MetaDataRepository.Classifier clasifier = OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as OOAdvantech.MetaDataRepository.Classifier;
        //        if (clasifier == null)
        //        {
        //            if (type != null)
        //            {
        //                OOAdvantech.DotNetMetaDataRepository.Assembly assembly = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type.Assembly) as OOAdvantech.DotNetMetaDataRepository.Assembly;
        //                if (assembly == null)
        //                    assembly = new OOAdvantech.DotNetMetaDataRepository.Assembly(type.Assembly);
        //                long count = assembly.Residents.Count;

        //            }
        //        }
        //        clasifier = OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(type) as OOAdvantech.MetaDataRepository.Classifier;


        //        if (clasifier != null)
        //        {
        //            foreach (OOAdvantech.MetaDataRepository.Attribute attribute in clasifier.GetAttributes(true))
        //            {
        //                if (attribute.Name == memberName)
        //                    return attribute.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
        //            }
        //            foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in clasifier.GetRoles(true))
        //            {
        //                if (associationEnd.Name == memberName)
        //                    return associationEnd.GetExtensionMetaObject(typeof(System.Reflection.MemberInfo)) as System.Reflection.MemberInfo;
        //            }

        //        }


        //        return null;
        //    }
        //}


        /// <MetaDataID>{b4b745bd-9425-409f-9232-030dea4a0a80}</MetaDataID>
        protected override List<string> DataColumnNames
        {
            get
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }

        /// <MetaDataID>{17bacc16-1a05-4b35-bb34-930ac53894d7}</MetaDataID>
        public override object GetObject(PersistenceLayer.StorageInstanceRef.ObjectSate row, out bool loadObjectLinks)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        public override object GetObjectIdentity(PersistenceLayer.StorageInstanceRef.ObjectSate row)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        internal Dictionary<System.Uri, List<ObjectsDataRetriever>> LoadObjects(List<ObjectData> list)
        {
            Dictionary<System.Uri, List<ObjectsDataRetriever>> objectsDataRetrievers = new Dictionary<System.Uri, List<ObjectsDataRetriever>>();
            if (_Data == null)
            {
                _Data = DataSource.DataObjectsInstantiator.CreateDataTable(false, DataNode.DataSource.Identity);
                if (!(DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
                {
                    Data.TableName = DataNode.Name;
                    CreateTableColumns(Data);
                }

                if (DataNode.ThroughRelationTable)
                    CreateParentRelationshipData();

            }
            foreach (var dataObject in list)
            {
                var _object = dataObject._Object;
                var row = LoadObjectDataInTable(dataObject);
            }


            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type != DataNode.DataNodeType.Object)
                    continue;
                Dictionary<object, List<ObjectData>> inProcessObjects = new Dictionary<object, List<ObjectData>>();

                foreach (var dataObject in list)
                {



                    var _object = dataObject._Object;
                    OOAdvantech.Collections.Generic.List<ObjectData> subDataNodeObjects = new OOAdvantech.Collections.Generic.List<ObjectData>();
                    inProcessObjects[_object] = subDataNodeObjects;
                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        object memberObject = null;

                        if (DataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
                        {
                            if (associationEnd.IsRoleA)
                            {
                                if (DataNode.Classifier is DotNetMetaDataRepository.Interface)
                                    memberObject = (DataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleAProperty.GetValue(_object, null);

                                if (DataNode.Classifier is DotNetMetaDataRepository.Class)
                                    memberObject = Member<object>.GetValue((DataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, _object);
                            }
                            else
                            {
                                if (DataNode.Classifier is DotNetMetaDataRepository.Interface)
                                    memberObject = (DataNode.Classifier as DotNetMetaDataRepository.Interface).LinkClassRoleBProperty.GetValue(_object, null);

                                if (DataNode.Classifier is DotNetMetaDataRepository.Class)
                                    memberObject = Member<object>.GetValue((DataNode.Classifier as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, _object);
                            }
                            subDataNodeObjects.Add(new ObjectData(_object, DataNodeObjects[_object]));

                        }
                        else
                        {
                            if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember != null)
                                memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(_object);
                            else
                                memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, _object);
                            if (memberObject != null)
                            {
                                if (associationEnd.CollectionClassifier != null)
                                {

                                    System.Collections.IEnumerator enumerator = memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                    enumerator.Reset();
                                    while (enumerator.MoveNext())
                                    {
                                        object collectionObj = enumerator.Current;
                                        subDataNodeObjects.Add(new ObjectData(collectionObj, DataNodeObjects[_object]));
                                    }

                                }
                                else
                                    subDataNodeObjects.Add(new ObjectData(memberObject, DataNodeObjects[_object]));
                            }
                        }
                    }

                    if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.DataSource != null)
                    {
                        MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                        object memberObject = null;

                        if ((attribute as DotNetMetaDataRepository.Attribute).PropertyMember != null)
                            memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastPropertyAccessor.GetValue(_object);
                        else
                            memberObject = (attribute as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue(_object);
                        if (memberObject != null)
                        {
                            if (memberObject != null && !this.DataNodeObjects.ContainsKey(memberObject))
                            {
                                if (memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]) != null
                                    && memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetMetaData().IsGenericType
                                    && memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetMetaData().GetGenericArguments().Length == 1
                                    && memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).ReturnType.GetMetaData().GetGenericArguments()[0] == subDataNode.Classifier.GetExtensionMetaObject(typeof(System.Type)) as System.Type)
                                {
                                    System.Collections.IEnumerator enumerator = memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                    enumerator.Reset();
                                    while (enumerator.MoveNext())
                                    {
                                        object collectionObj = enumerator.Current;
                                        subDataNodeObjects.Add(new ObjectData(collectionObj, DataNodeObjects[_object]));
                                    }
                                }
                                else
                                    subDataNodeObjects.Add(new ObjectData(memberObject, DataNodeObjects[_object]));
                            }
                        }
                    }

                    List<ObjectData> inStorageSubNodeObjects = new List<ObjectData>();
                    foreach (var subNodeObject in subDataNodeObjects)
                    {


                        if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(subNodeObject._Object as MarshalByRefObject))
                        {
                            //(subDataNode.DataSource.DataLoaders[subDataNode.ObjectQuery.GetHashCode().ToString()] as RootObjectDataLoader).LoadObjects(objectsDataRetriever.Objects,new Dictionary<int,object>());
                            //subDataNode.DataSource
                        }
                        else
                        {
                            if (subDataNode.Classifier.GetExtensionMetaObject<System.Type>().GetMetaData().IsInstanceOfType(subNodeObject._Object))
                                inStorageSubNodeObjects.Add(subNodeObject);
                            else
                            {

                            }
                        }
                    }

                    (GetDataLoader(subDataNode) as RootObjectDataLoader).LoadObjects(inStorageSubNodeObjects);

                }


            }

            //    Data.Rows.Add(row);
            if (DataNode.Recursive && RecursiveStep < DataNode.RecursiveSteps)
            {

                RecursiveStep++;
                OOAdvantech.Collections.Generic.List<ObjectData> recursiveObjects = new OOAdvantech.Collections.Generic.List<ObjectData>();

                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    object memberObject = null;

                    foreach (var dataObject in list)
                    {



                        var _object = dataObject._Object;
                        if ((associationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember != null)
                            memberObject = (associationEnd as DotNetMetaDataRepository.AssociationEnd).FastPropertyAccessor.GetValue(_object);
                        else
                            memberObject = Member<object>.GetValue((associationEnd as DotNetMetaDataRepository.AssociationEnd).FastFieldAccessor.GetValue, _object);
                        if (memberObject != null)
                        {
                            if (associationEnd.CollectionClassifier != null)
                            {

                                System.Collections.IEnumerator enumerator = memberObject.GetType().GetMetaData().GetMethod("GetEnumerator", new System.Type[0]).Invoke(memberObject, new object[0]) as System.Collections.IEnumerator;
                                enumerator.Reset();
                                while (enumerator.MoveNext())
                                {
                                    object collectionObj = enumerator.Current;
                                    recursiveObjects.Add(new ObjectData(collectionObj, DataNodeObjects[_object]));
                                }

                            }
                            else
                                recursiveObjects.Add(new ObjectData(memberObject, DataNodeObjects[_object]));
                        }
                    }
                }


                List<ObjectData> inStorageSubNodeObjects = new List<ObjectData>();
                foreach (var subNodeObject in recursiveObjects)
                {

                    if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(subNodeObject._Object as MarshalByRefObject))
                    {
                        //(subDataNode.DataSource.DataLoaders[subDataNode.ObjectQuery.GetHashCode().ToString()] as RootObjectDataLoader).LoadObjects(objectsDataRetriever.Objects,new Dictionary<int,object>());
                        //subDataNode.DataSource
                    }
                    else
                    {
                        inStorageSubNodeObjects.Add(subNodeObject);
                    }
                }
                LoadObjects(inStorageSubNodeObjects);
                RecursiveStep--;
            }

            return null;

        }

        protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, System.Collections.Generic.List<object> relatedObjects)
        {
            throw new System.NotImplementedException();
        }

        protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, object ownerObject, object relatedObject)
        {
            throw new System.NotImplementedException();
        }

        protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, ValueTypePath valueTypePath, object ownerObject, System.Collections.Generic.List<object> relatedObjects)
        {
            throw new System.NotImplementedException();
        }

        public override bool CriterionCanBeResolvedFromNativeSystem(Criterion criterion)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HasOutStorageRelationOnCurrentTrasaction(AssociationEnd associationEnd, DataNode relatedDataNode)
        {
            if (DataLoaderMetadata.RelatedMemoryCells.ContainsKey(relatedDataNode.Identity))
            {
                foreach (var memoryCell in DataLoaderMetadata.RelatedMemoryCells[relatedDataNode.Identity])
                    if (memoryCell.IsOutProcessMemoryCell)
                        return true;

            }
            return false;
        }

        protected override object GetObjectFromIdentity(OOAdvantech.PersistenceLayer.ObjectID objectIdentity)
        {
            throw new System.NotImplementedException();
        }
    }
}
