using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SubDataNodeIdentity = System.Guid;
namespace OOAdvantech.MetaDataLoadingSystem.ObjectQueryLanguage
{
    using System.Xml.Linq;
    using MetaDataRepository.ObjectQueryLanguage;
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.PersistenceLayerRunTime;


    /// <MetaDataID>{01572173-8598-429a-acd3-88ab58f86da4}</MetaDataID>
    public class DataLoader : PersistenceLayerRunTime.StorageDataLoader
    {
         
        public override OOAdvantech.MetaDataRepository.ObjectIdentityType ObjectIdentityTypeForNewObject
        {
            get { throw new NotImplementedException(); }
        }
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCellsLink> GetStorageCellsLinks(DataNode relatedDataNode)
        {
            throw new NotImplementedException();
        }

        //public override List<string> RowRemoveColumns
        //{
        //    get { return new List<string>(); }
        //}
        public override bool CriterionCanBeResolvedFromNativeSystem(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion criterion)
        {
            return false;
        }


        public override bool DataLoadedInParentDataSource
        {
            get
            {
                if (DataNode.Type == DataNode.DataNodeType.Object && DataNode.ValueTypePath.Count > 0)
                    return true;

                return false;
            }
            set
            {

            }
        }
        public override bool RetrievesData
        {
            get { return true; }
        }
        protected override object Convert(object value, System.Type type)
        {
            if (value == null || value is System.DBNull)
                return value;
            else
                return System.Convert.ChangeType(value, type, CultureInfoHelper.GetCultureInfo(1033));

        }
        protected override object GetObjectFromIdentity(OOAdvantech.PersistenceLayer.ObjectID objectIdentity)
        {
            object @object = null;
            LoadedObjects.TryGetValue(objectIdentity, out @object);
            return @object;
        }

        //protected override PersistenceLayer.ObjectID GetTemporaryObjectID()
        //{
        //    return (ObjectStorage as MetaDataStorageSession).GetTemporaryObjectID();
        //}
        public override string GetLocalDataColumnName(DataNode dataNode)
        {
            if (dataNode is AggregateExpressionDataNode)
                return dataNode.Alias;
            string valueTypePrefix = null;
            DataNode parentDataNode = dataNode.ParentDataNode;

            if (parentDataNode.Classifier.FullName == typeof(System.DateTime).FullName)
            {
                valueTypePrefix = parentDataNode.Name + "_";
                parentDataNode = parentDataNode.ParentDataNode;
            }
            if (!string.IsNullOrEmpty(parentDataNode.ValueTypePathDiscription))
                valueTypePrefix += parentDataNode.ValueTypePathDiscription + "_";

            return valueTypePrefix + dataNode.Name;
        }
        public override bool LocalDataColumnExistFor(DataNode dataNode)
        {
            return true;
        }
        public override bool HasRelationIdentityColumnsFor(DataNode subDataNode)
        {
            if (subDataNode.ThroughRelationTable)
                return true;
            if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
            {
                if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association == subDataNode.Classifier.ClassHierarchyLinkAssociation)
                    return true;

                if (!(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
                    (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType != MetaDataRepository.AssociationType.ManyToMany)
                {
                    return true;
                }
                if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.OneToOne &&
                    !(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().IsRoleA)
                    return true;
                return false;
            }
            return true;
        }
        //protected override void BuildDataTable(DataNode dataNode, System.Data.DataTable table, System.Collections.Generic.Dictionary<string, string> aliasColumns, System.Collections.Generic.Dictionary<string, int> columnsIndices, int[] sourceColumnsIndices)
        //{
        //    throw new NotImplementedException();
        //}


        public override bool ParticipateInGlobalResolvedCriterion
        {
            get
            {
                return true;
            }
        }
        System.Collections.Generic.Dictionary<Guid, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>> _GroupByKeyRelationColumns;
        public override System.Collections.Generic.Dictionary<Guid, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>> GroupByKeyRelationColumns
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
                                foreach (OOAdvantech.MetaDataRepository.ObjectIdentityType identityType in (dataNode.DataSource.DataLoaders[this.DataLoaderMetadata.ObjectsContextIdentity] as MetaDataRepository.ObjectQueryLanguage.StorageDataLoader).ObjectIdentityTypes)
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
        //protected override bool CanAggregateFanctionsResolvedLocally(DataNode aggregateFunctionDataNode)
        //{
        //    return false;
        //}
        public override System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>> RecursiveParentColumns
        {
            get
            {

                if (!DataNode.Recursive)
                    return new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>>();
                else
                {
                    List<string> recursiveMasterColumns = new List<string>();

                    MetaDataRepository.ObjectIdentityType identityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(ulong)) });
                    recursiveMasterColumns.Add("ObjectID");
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>> relationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>>();
                    //System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>> relationColumns = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>();
                    relationColumns[DataNode.AssignedMetaObject.Identity.ToString()] = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>();
                    relationColumns[DataNode.AssignedMetaObject.Identity.ToString()][identityType] = recursiveMasterColumns;
                    return relationColumns;

                }
            }
        }
        System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> _ObjectIdentityTypes;
        public override System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType> ObjectIdentityTypes
        {

            get
            {
                if (_ObjectIdentityTypes == null)
                {
                    MetaDataRepository.ObjectIdentityType identityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(ulong)) });
                    _ObjectIdentityTypes = new System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>();
                    _ObjectIdentityTypes.Add(identityType);
                }
                return new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>(_ObjectIdentityTypes);
            }
        }
        public override void UpdateObjectIdentityTypes(System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType> dataLoaderObjectIdentityTypes)
        {
            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in dataLoaderObjectIdentityTypes)
            {
                if (_ObjectIdentityTypes.Contains(objectIdentityType))
                {
                    _ObjectIdentityTypes.RemoveAt(_ObjectIdentityTypes.IndexOf(objectIdentityType));
                    _ObjectIdentityTypes.Add(objectIdentityType);
                }
                else
                {
                    _ObjectIdentityTypes.Add(objectIdentityType);
                }
            }
        }

        public override System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>> RecursiveParentReferenceColumns
        {
            get
            {
                if (!DataNode.Recursive)
                    return new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>>();
                else
                {
                    List<string> recursiveDetailsColumns = new List<string>();
                    MetaDataRepository.ObjectIdentityType identityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(ulong)) });
                    MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    if (associationEnd.IsRoleA)
                        recursiveDetailsColumns.Add("Recursive_" + associationEnd.Association.Name + "RoleB_OID");
                    else
                        recursiveDetailsColumns.Add("Recursive_" + associationEnd.Association.Name + "RoleA_OID");

                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>> relationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>>();
                    relationColumns[DataNode.AssignedMetaObject.Identity.ToString()] = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>();
                    relationColumns[DataNode.AssignedMetaObject.Identity.ToString()][identityType] = recursiveDetailsColumns;
                    return relationColumns;
                }
            }
        }

        //protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, object relatedObject)
        //{
        //    DotNetMetaDataRepository.Class _class = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(ownerObject.GetType()) as DotNetMetaDataRepository.Class;
        //    AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = _class.GetFastFieldAccessor(associationEnd);
        //    Member<object>.SetValue(fastFieldAccessor, ref ownerObject, relatedObject);
        //}
        //protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, object ownerObject, System.Collections.ArrayList relatedObjects)
        //{
        //    //throw new Exception("The method or operation is not implemented.");
        //}
        //protected override void LoadObjectsLink(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath, object ownerObject, object relatedObject)
        //{

        //}
        //protected override void LoadObjectsLinks(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath, object ownerObject, System.Collections.ArrayList relatedObjects)
        //{

        //}
        public override bool HasRelationIdentityColumns
        {
            get
            {
                if (DataNode.ThroughRelationTable)
                    return true;

                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    if (!associationEnd.Multiplicity.IsMany &&  //zero ore one
                        associationEnd.GetOtherEnd().Multiplicity.IsMany)
                    {
                        return true;
                    }
                    else if ((associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
                        associationEnd.Association.Specializations.Count > 0) &&
                        DataNode.Classifier.ClassHierarchyLinkAssociation != associationEnd.Association)
                    {
                        return true;
                    }
                    else if (associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.OneToOne &&
                       !associationEnd.IsRoleA)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;



                //if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                //    return true;

                //if ((DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) && (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
                //    !(DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany)
                //{
                //    return true;
                //}
                //else if ((DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) &&
                //    (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                //    DataNode.Classifier.ClassHierarchyLinkAssociation != (DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
                //{
                //    return true;
                //}
                //else
                //    return false;


            }
        }
        //private bool HasSubNodeRelationIdentityColumns(DataNode subDataNode)
        //  {
        //     if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
        //         return true;

        //     if ((subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) && (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Multiplicity.IsMany &&
        //         !(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Multiplicity.IsMany)
        //     {
        //         return true;
        //     }
        //     else if ((subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) &&
        //         ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||
        //         (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Specializations.Count > 0) &&
        //         subDataNode.Classifier.ClassHierarchyLinkAssociation != (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
        //     {

        //         return true;

        //     }
        //     else
        //         return false;

        // }

        MetaDataRepository.ObjectQueryLanguage.SearchCondition SearchCondition;
        public DataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode, dataLoaderMetadata)
        {
            SearchCondition = dataNode.SearchCondition;
            //foreach (StorageCell storageCell in DataLoaderMetadata.StorageCells)
            //{
            //    _LoadFromStorage = storageCell.Namespace as OOAdvantech.MetaDataRepository.Storage;
            //    break;
            //}

        }
        IDataTable _TemporaryDataTable;
        IDataTable TemporaryDataTable
        {
            get
            {
                if (_TemporaryDataTable == null)
                {
                    if (DataNode.Type == DataNode.DataNodeType.Object && DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                    {
                        DataNode dataNode = DataNode.ParentDataNode;
                        while (dataNode.Type == DataNode.DataNodeType.Object && dataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                            dataNode = dataNode.ParentDataNode;
                        _TemporaryDataTable = (dataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).TemporaryDataTable;
                    }
                    else
                        _TemporaryDataTable = DataSource.DataObjectsInstantiator.CreateDataTable(false);
                }
                return _TemporaryDataTable;
            }
        }
        protected override List<string> DataColumnNames
        {
            get
            {
                List<string> columns = new List<string>();
                foreach (IDataColumn column in TemporaryDataTable.Columns)
                    columns.Add(column.ColumnName);
                return columns;
            }
        }

        System.Collections.Generic.Dictionary<PersistenceLayer.ObjectID, object> LoadedObjects = new Dictionary<PersistenceLayer.ObjectID, object>();

        MetaDataLoadingSystem.StorageCell LastStorageCell = null;
        XElement LastStorageCellXmlElement = null;
        public override object GetObject(PersistenceLayer.StorageInstanceRef.ObjectSate row, out bool loadObjectLinks)
        {
            lock (ObjectStorage)
            {
                string columnPrefix = null;
                DataNode nonValueTypeDataNode = DataNode;
                while (nonValueTypeDataNode.ValueTypePath.Count > 0)
                    nonValueTypeDataNode = nonValueTypeDataNode.RealParentDataNode;

                if (nonValueTypeDataNode.DataSource.DataLoadedInParentDataSource)
                    columnPrefix = nonValueTypeDataNode.Alias + "_";

                if (DataNode.ValueTypePath.Count > 0 &&
                    !(DataNode.RealParentDataNode.DataSource as StorageDataSource).ObjectActivation &&
                    (row[TemporaryDataTable.GetHashCode().ToString() + "_InstanceOnMemory"] == null || row[TemporaryDataTable.GetHashCode().ToString() + "_InstanceOnMemory"] is DBNull))
                {

                    XElement objectElement = row[DataNode.ValueTypePathDiscription + TemporaryDataTable.GetHashCode().ToString() + "_StorageIstance"] as XElement;

                    XElement parentNode = objectElement.Parent;
                    while (parentNode.Parent.Name != "ObjectCollections")
                        parentNode = parentNode.Parent;
                    if (LastStorageCellXmlElement != parentNode)
                    {
                        foreach (MetaDataLoadingSystem.StorageCell dataLoaderStorageCell in DataLoaderMetadata.StorageCells)
                        {
                            if (parentNode == dataLoaderStorageCell.XmlElement)
                            {
                                LastStorageCellXmlElement = dataLoaderStorageCell.XmlElement;
                                LastStorageCell = dataLoaderStorageCell;
                                break;
                            }
                        }
                    }
                    object _object = MetaDataStorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, objectElement, columnPrefix, default(string), LastStorageCell);
                    if (_object == null)
                        loadObjectLinks = false;
                    else
                        loadObjectLinks = true;
                    return _object;
                }
                else
                {
                    PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = null;

                    storageInstanceRef = row[TemporaryDataTable.GetHashCode().ToString() + "_InstanceOnMemory"] as PersistenceLayerRunTime.StorageInstanceRef;
                    if (storageInstanceRef != null)
                    {
                        loadObjectLinks = false;

                        if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                        {
                            DotNetMetaDataRepository.Attribute attribute = DataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute;
                            object _object = storageInstanceRef.MemoryInstance;
                            DotNetMetaDataRepository.AttributeRealization attributeRealization = storageInstanceRef.Class.GetAttributeRealization(attribute) as DotNetMetaDataRepository.AttributeRealization;
                            if (attributeRealization != null)
                            {
                                if (attributeRealization.FastFieldAccessor != null)
                                    return attributeRealization.FastFieldAccessor.GetValue(_object);
                                else if (attributeRealization.FastPropertyAccessor != null)
                                    return attributeRealization.FastPropertyAccessor.GetValue(_object);
                            }

                            if (attribute.FastPropertyAccessor != null)
                                return attribute.FastPropertyAccessor.GetValue(_object);
                            else if (attribute.FastFieldAccessor != null)
                                return attribute.FastFieldAccessor.GetValue(_object);
                            else
                            {
                                if (attribute.PropertyMember != null)
                                    return attribute.PropertyMember.GetValue(_object, null);

                            }
                            return _object;

                            //MetaDataRepository.AttributeRealization attributeRealization = storageInstanceRef.Class.GetAttributeRealization(DataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute);
                            //if (attributeRealization != null)
                            //    return Member<object>.GetValue((attributeRealization as DotNetMetaDataRepository.AttributeRealization).FastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                            //return Member<object>.GetValue((DataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).FastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                        }

                        return storageInstanceRef.MemoryInstance;
                    }


                    XElement objectElement = row[TemporaryDataTable.GetHashCode().ToString() + "_StorageIstance"] as XElement;

                    if (LastStorageCellXmlElement != objectElement.Parent)
                    {
                        foreach (MetaDataLoadingSystem.StorageCell dataLoaderStorageCell in DataLoaderMetadata.StorageCells)
                        {
                            if (objectElement.Parent == dataLoaderStorageCell.XmlElement)
                            {
                                LastStorageCellXmlElement = dataLoaderStorageCell.XmlElement;
                                LastStorageCell = dataLoaderStorageCell;
                                break;
                            }
                        }
                    }
                    MetaDataLoadingSystem.StorageCell storageCell = LastStorageCell;
                    System.Type objectType = storageCell.Type.GetExtensionMetaObject<System.Type>();
                    ulong ObjID = 0;
                    OOAdvantech.PersistenceLayer.ObjectID objectID = null;
                    //PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef = null;
                    if (!(DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
                    {
                        ulong.TryParse(objectElement.GetAttribute("oid"), out ObjID);
                        objectID = new ObjectID(ObjID);
                        storageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[objectType][objectID];
                    }
                    else
                    {
                        ulong.TryParse(objectElement.GetAttribute("oid"), out ObjID);
                        objectID = new ObjectID(ObjID);
                        storageInstanceRef = (PersistenceLayerRunTime.StorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[objectType][objectID];
                        loadObjectLinks = true;
                        object _object = storageInstanceRef.MemoryInstance;
                        DotNetMetaDataRepository.Attribute attribute = DataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute;
                        DotNetMetaDataRepository.AttributeRealization attributeRealization = storageInstanceRef.Class.GetAttributeRealization(DataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute) as DotNetMetaDataRepository.AttributeRealization;
                        if (attributeRealization != null)
                        {
                            if (attributeRealization.FastFieldAccessor != null)
                                return attributeRealization.FastFieldAccessor.GetValue(_object);
                            else if (attributeRealization.FastPropertyAccessor != null)
                                return attributeRealization.FastPropertyAccessor.GetValue(_object);
                        }

                        if (attribute.FastPropertyAccessor != null)
                            return attribute.FastPropertyAccessor.GetValue(_object);
                        else if (attribute.FastFieldAccessor != null)
                            return attribute.FastFieldAccessor.GetValue(_object);
                        else
                        {
                            if (attribute.PropertyMember != null)
                                return attribute.PropertyMember.GetValue(_object, null);

                        }
                        return _object;

                    }

                    if (storageInstanceRef != null)
                    {
                        loadObjectLinks = false;
                        LoadedObjects[storageInstanceRef.PersistentObjectID] = storageInstanceRef.MemoryInstance;
                        return storageInstanceRef.MemoryInstance;
                    }
                    else
                    {
                        object NewObject = AccessorBuilder.CreateInstance(storageCell.Type.GetExtensionMetaObject<System.Type>());
                        if (NewObject == null)
                            throw new System.Exception("PersistencyService can't instadiate the " + storageCell.Type.FullName);

                        storageInstanceRef = (MetaDataStorageInstanceRef)((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).CreateStorageInstanceRef(NewObject, objectID);
                        ((MetaDataStorageInstanceRef)storageInstanceRef).TheStorageIstance = objectElement;
                        ((MetaDataStorageInstanceRef)storageInstanceRef).LoadObjectState();
                        //StorageInstanceRef.SnapshotStorageInstance();
                        //storageInstanceRef.ObjectActived();
                        loadObjectLinks = true;
                        LoadedObjects[storageInstanceRef.PersistentObjectID] = storageInstanceRef.MemoryInstance;
                        return storageInstanceRef.MemoryInstance;
                    }
                }
            }

        }



        public override object GetObjectIdentity(OOAdvantech.PersistenceLayer.StorageInstanceRef.ObjectSate row)
        {
            string columnPrefix = null;
            DataNode nonValueTypeDataNode = DataNode;
            while (nonValueTypeDataNode.ValueTypePath.Count > 0)
                nonValueTypeDataNode = nonValueTypeDataNode.RealParentDataNode;

            if (nonValueTypeDataNode.DataSource.DataLoadedInParentDataSource)
                columnPrefix = nonValueTypeDataNode.Alias + "_";

            if (DataNode.ValueTypePath.Count > 0 &&
                !(DataNode.RealParentDataNode.DataSource as StorageDataSource).ObjectActivation &&
                (row[TemporaryDataTable.GetHashCode().ToString() + "_InstanceOnMemory"] == null || row[TemporaryDataTable.GetHashCode().ToString() + "_InstanceOnMemory"] is DBNull))
            {

                XElement objectElement = row[DataNode.ValueTypePathDiscription + TemporaryDataTable.GetHashCode().ToString() + "_StorageIstance"] as XElement;

                XElement parentNode = objectElement.Parent;
                while (parentNode.Parent.Name != "ObjectCollections")
                    parentNode = parentNode.Parent;
                if (LastStorageCellXmlElement != parentNode)
                {
                    foreach (MetaDataLoadingSystem.StorageCell dataLoaderStorageCell in DataLoaderMetadata.StorageCells)
                    {
                        if (parentNode == dataLoaderStorageCell.XmlElement)
                        {
                            LastStorageCellXmlElement = dataLoaderStorageCell.XmlElement;
                            LastStorageCell = dataLoaderStorageCell;
                            break;
                        }
                    }
                }
                object _object = MetaDataStorageInstanceRef.GetValueTypeValue(DataNode.AssignedMetaObject as MetaDataRepository.Attribute, objectElement, columnPrefix, default(string), LastStorageCell);

                return _object;
            }
            else
            {
                PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = null;

                storageInstanceRef = row[TemporaryDataTable.GetHashCode().ToString() + "_InstanceOnMemory"] as PersistenceLayerRunTime.StorageInstanceRef;
                if (storageInstanceRef != null)
                    return storageInstanceRef.ObjectID;


                XElement objectElement = row[TemporaryDataTable.GetHashCode().ToString() + "_StorageIstance"] as XElement;

                if (LastStorageCellXmlElement != objectElement.Parent)
                {
                    foreach (MetaDataLoadingSystem.StorageCell dataLoaderStorageCell in DataLoaderMetadata.StorageCells)
                    {
                        if (objectElement.Parent == dataLoaderStorageCell.XmlElement)
                        {
                            LastStorageCellXmlElement = dataLoaderStorageCell.XmlElement;
                            LastStorageCell = dataLoaderStorageCell;
                            break;
                        }
                    }
                }
                MetaDataLoadingSystem.StorageCell storageCell = LastStorageCell;
                System.Type objectType = storageCell.Type.GetExtensionMetaObject<System.Type>();
                ulong ObjID = 0;
                OOAdvantech.PersistenceLayer.ObjectID objectID = null;
                PersistenceLayerRunTime.StorageInstanceRef StorageInstanceRef = null;
                if (!(DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
                {
                    ulong.TryParse(objectElement.GetAttribute("oid"), out ObjID);
                    objectID = new ObjectID(ObjID);
                    return objectID;
                }
                else
                {
                    ulong.TryParse(objectElement.GetAttribute("oid"), out ObjID);
                    objectID = new ObjectID(ObjID);
                    return objectID;
                }
            }
        }

        public OOAdvantech.PersistenceLayer.ObjectStorage ObjectStorage
        {
            get
            {

                MetaDataStorageProvider storageProvider = new MetaDataStorageProvider();
                return (Storage as MetaDataLoadingSystem.Storage).ObjectStorage;
            }
        }
        public override OOAdvantech.MetaDataRepository.Classifier Classifier
        {
            get { return DataNode.Classifier; }
        }

        protected override System.Type GetColumnType(string columnName)
        {
            return TemporaryDataTable.Columns[columnName].DataType;
        }


        public void CreateParentRelationshipData()
        {
            MetaDataRepository.ObjectIdentityType identityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
            MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>> parentRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>> childRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>>();
            parentRelationColumns[associationEnd.Identity.ToString()] = new System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>();
            childRelationColumns[associationEnd.Identity.ToString()] = new System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>();

            parentRelationColumns[associationEnd.Identity.ToString()].Add(associationEnd.GetReferenceObjectIdentityType(identityType));
            childRelationColumns[associationEnd.Identity.ToString()].Add(associationEnd.GetOtherEnd().GetReferenceObjectIdentityType(identityType));
            _ParentRelationshipData = new AssotiationTableRelationshipData(parentRelationColumns, childRelationColumns, DataSource.DataObjectsInstantiator.CreateDataTable(false), associationEnd.GetOtherEnd().Role, DataNode.ParentDataNode.Identity, DataNode.Identity);


        }
        //public override OOAdvantech.MetaDataRepository.ObjectIdentityType ObjectIdentityTypeForNewObject
        //{
        //    get { throw new NotImplementedException(); }
        //}
        //public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCellsLink> GetStorageCellsLinks(DataNode relatedDataNode)
        //{
        //    throw new NotImplementedException();
        //}

        //Used for foreign key columns on dataloader table
        Dictionary<string, Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink>> RelationWithSubDataNodeChanges = new Dictionary<string, Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink>>();

        Dictionary<Guid, Dictionary<string, MetaDataRepository.AssociationEnd>> SubNodesRoleNames = new Dictionary<Guid, Dictionary<string, MetaDataRepository.AssociationEnd>>();
        public override void RetrieveFromStorage()
        {
            if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                return;
            if (DataNode.Type == DataNode.DataNodeType.Group)
            {
                _Data = null;
                return;
                if (_Data == null)
                    _Data = DataSource.DataObjectsInstantiator.CreateDataTable(false);
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
            List<string> path = new List<string>();
            MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = DataNode;
            while (dataNode.AssignedMetaObject is MetaDataRepository.Attribute)
            {
                path.Insert(0, dataNode.AssignedMetaObject.Name);
                dataNode = dataNode.ParentDataNode;
            }

            #region Add table columns

            if (!(DataNode.AssignedMetaObject is MetaDataRepository.Attribute))
            {
                TemporaryDataTable.TableName = DataNode.Name;
                TemporaryDataTable.Columns.Add("ObjectID", typeof(ulong));//Add Object identity columns
                TemporaryDataTable.Columns.Add("OSM_StorageIdentity", typeof(int)).DefaultValue = DataLoaderMetadata.QueryStorageID;
            }

            AddTableDataColumns(TemporaryDataTable);

            #endregion

            MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
            MetaDataRepository.ObjectIdentityType xmlStorgeIdentityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(ulong)) });

            #region Load relation changes data
            MetaDataRepository.ObjectIdentityType identityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
            if (DataNode.ThroughRelationTable && DataLoaderMetadata.MemoryCell != null)
            {
                CreateParentRelationshipData();
                foreach (ObjectData objectData in DataLoaderMetadata.MemoryCell.Objects.Values)
                {
                    System.Guid OID = objectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
                    AssotiationTableRelationshipData relationshipData = _ParentRelationshipData;
                    foreach (var parentObjectData in objectData.ParentDataNodeRelatedObjects.Values)
                    {
                        IDataRow associationRow = relationshipData.Data.NewRow();
                        associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.GetReferenceObjectIdentityType(identityType).Parts[0].Name] = parentObjectData.ObjectID.GetTypedMemberValue<System.Guid>("MC_ObjectID");
                        associationRow[DataNode.ParentDataNode.ValueTypePathDiscription + associationEnd.GetOtherEnd().GetReferenceObjectIdentityType(identityType).Parts[0].Name] = OID;
                        relationshipData.Data.Rows.Add(associationRow);
                    }
                }

            }

            //Used for relation table 
            Dictionary<string, Dictionary<object, List<PersistenceLayerRunTime.ObjectsLink>>> relationshipsChangesData = new Dictionary<string, Dictionary<object, List<OOAdvantech.PersistenceLayerRunTime.ObjectsLink>>>();


            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                if (subDataNodeassociationEnd != null && subDataNode.ThroughRelationTable)
                {
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>> parentRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MetaDataRepository.ObjectIdentityType>> childRelationColumns = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>>();
                    parentRelationColumns[subDataNodeassociationEnd.Identity.ToString()] = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();
                    childRelationColumns[subDataNodeassociationEnd.Identity.ToString()] = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.ObjectIdentityType>();

                    #region Builds association table columns metadata
                    if (subDataNodeassociationEnd.IsRoleA)
                    {
                        foreach (var relationsColumns in GetDataLoader(subDataNode).DataSourceRelationsColumnsWithParent)
                        {
                            MetaDataRepository.AssociationEnd relationAssociationEnd = GetRelationAssociationEnd(subDataNodeassociationEnd, relationsColumns.Key);
                            foreach (var entry in relationsColumns.Value)
                            {
                                MetaDataRepository.ObjectIdentityType objectIdentityType = entry.Key;
                                List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                                foreach (var part in objectIdentityType.Parts)
                                    parts.Add(new MetaDataRepository.IdentityPart(relationAssociationEnd.Association.CaseInsensitiveName + "_" + part.PartTypeName + "A", part.PartTypeName, part.Type));
                                childRelationColumns[relationAssociationEnd.Identity.ToString()].Add(new MetaDataRepository.ObjectIdentityType(parts));
                            }
                        }
                        foreach (var relationsColumns in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity])
                        {
                            MetaDataRepository.AssociationEnd relationAssociationEnd = GetRelationAssociationEnd(subDataNodeassociationEnd, relationsColumns.Key);
                            foreach (var entry in relationsColumns.Value)
                            {
                                MetaDataRepository.ObjectIdentityType objectIdentityType = entry.Key;
                                List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                                foreach (var part in objectIdentityType.Parts)
                                    parts.Add(new MetaDataRepository.IdentityPart(relationAssociationEnd.Association.CaseInsensitiveName + "_" + part.PartTypeName + "B", part.PartTypeName, part.Type));
                                parentRelationColumns[relationAssociationEnd.Identity.ToString()].Add(new MetaDataRepository.ObjectIdentityType(parts));
                            }
                        }
                    }
                    else
                    {
                        foreach (var relationsColumns in GetDataLoader(subDataNode).DataSourceRelationsColumnsWithParent)
                        {
                            MetaDataRepository.AssociationEnd relationAssociationEnd = GetRelationAssociationEnd(subDataNodeassociationEnd, relationsColumns.Key);
                            foreach (var entry in relationsColumns.Value)
                            {
                                MetaDataRepository.ObjectIdentityType objectIdentityType = entry.Key;
                                List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                                foreach (var part in objectIdentityType.Parts)
                                    parts.Add(new MetaDataRepository.IdentityPart(relationAssociationEnd.Association.CaseInsensitiveName + "_" + part.PartTypeName + "B", part.PartTypeName, part.Type));
                                childRelationColumns[relationAssociationEnd.Identity.ToString()].Add(new MetaDataRepository.ObjectIdentityType(parts));
                            }
                        }
                        foreach (var relationsColumns in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity])
                        {
                            MetaDataRepository.AssociationEnd relationAssociationEnd = GetRelationAssociationEnd(subDataNodeassociationEnd, relationsColumns.Key);
                            foreach (var entry in relationsColumns.Value)
                            {
                                MetaDataRepository.ObjectIdentityType objectIdentityType = entry.Key;
                                List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                                foreach (var part in objectIdentityType.Parts)
                                    parts.Add(new MetaDataRepository.IdentityPart(relationAssociationEnd.Association.CaseInsensitiveName + "_" + part.PartTypeName + "A", part.PartTypeName, part.Type));
                                parentRelationColumns[relationAssociationEnd.Identity.ToString()].Add(new MetaDataRepository.ObjectIdentityType(parts));
                            }
                        }
                    }
                    #endregion

                    AssotiationTableRelationshipData relationshipData = new AssotiationTableRelationshipData(parentRelationColumns, childRelationColumns, DataSource.DataObjectsInstantiator.CreateDataTable(false), subDataNodeassociationEnd.GetOtherEnd().Role, dataNode.Identity, subDataNode.Identity);
                    RelationshipsData.Add(subDataNode.Identity, relationshipData);
                    relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()] = new Dictionary<object, List<OOAdvantech.PersistenceLayerRunTime.ObjectsLink>>();

                    #region load relation changes data
                    foreach (OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink in GetRelationChanges(subDataNodeassociationEnd, subDataNode))
                    {

                        if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association == DataNode.Classifier.ClassHierarchyLinkAssociation)
                        {
                            List<OOAdvantech.PersistenceLayerRunTime.ObjectsLink> objectsLinks = null;
                            var relationObject_OID = objectsLink.RelationObject.RealStorageInstanceRef.ObjectID;

                            if (!relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()].TryGetValue(relationObject_OID, out objectsLinks))
                            {
                                objectsLinks = new List<OOAdvantech.PersistenceLayerRunTime.ObjectsLink>();
                                relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()][relationObject_OID] = objectsLinks;
                            }
                            objectsLinks.Add(objectsLink);

                        }
                        else if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                        {
                            List<OOAdvantech.PersistenceLayerRunTime.ObjectsLink> objectsLinks = null;
                            var roleB_OID = objectsLink.RoleB.RealStorageInstanceRef.ObjectID;

                            if (!relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()].TryGetValue(roleB_OID, out objectsLinks))
                            {
                                objectsLinks = new List<OOAdvantech.PersistenceLayerRunTime.ObjectsLink>();
                                relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()][roleB_OID] = objectsLinks;
                            }
                            objectsLinks.Add(objectsLink);
                        }
                        else
                        {
                            List<OOAdvantech.PersistenceLayerRunTime.ObjectsLink> objectsLinks = null;
                            var roleA_OID = objectsLink.RoleA.ObjectID;

                            if (!relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()].TryGetValue(roleA_OID, out objectsLinks))
                            {
                                objectsLinks = new List<OOAdvantech.PersistenceLayerRunTime.ObjectsLink>();
                                relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()][roleA_OID] = objectsLinks;
                            }
                            objectsLinks.Add(objectsLink);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region load relation changes data
                    if ((subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) && HasRelationReferenceColumnsFor(subDataNode))// GetDataLoader(subDataNode).HasRelationIdentityColumns)
                    {
                        if (!RelationWithSubDataNodeChanges.ContainsKey(subDataNode.Identity.ToString()))
                            RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()] = new Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink>();
                        Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink> relationChanges = RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()];
                        foreach (OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink in GetRelationChanges(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, subDataNode))
                        {
                            if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                relationChanges[objectsLink.RoleB.ObjectID] = objectsLink;
                            else
                                relationChanges[objectsLink.RoleA.ObjectID] = objectsLink;
                        }
                    }
                    #endregion
                }
            }


            Dictionary<PersistenceLayer.ObjectID, PersistenceLayerRunTime.ObjectsLink> relationWithParentChanges = new System.Collections.Generic.Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink>();

            #region Load relation chages data with parent data node
            if (!dataNode.ThroughRelationTable && HasRelationReferenceColumns)
            {
                if (DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                {
                    foreach (OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink in (dataNode.ParentDataNode.DataSource.DataLoaders[ObjectStorage.StorageMetaData.StorageIdentity] as DataLoader).GetRelationChanges(associationEnd, DataNode))
                    {
                        if ((DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                            relationWithParentChanges[objectsLink.RoleA.ObjectID] = objectsLink;
                        else
                            relationWithParentChanges[objectsLink.RoleB.ObjectID] = objectsLink;
                    }
                }
            }
            #endregion


            #endregion

            #region Deleted and updated  objects under transaction
            Dictionary<PersistenceLayer.ObjectID, PersistenceLayerRunTime.StorageInstanceRef> candidateForDeleteObjects = new System.Collections.Generic.Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef>();
            foreach (PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef in CandidateForDeleteObjects)
                candidateForDeleteObjects[storageInstanceRef.ObjectID] = storageInstanceRef;
            Dictionary<PersistenceLayer.ObjectID, PersistenceLayerRunTime.StorageInstanceRef> updatedObjects = new System.Collections.Generic.Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef>();
            foreach (PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef in UpdatedObjects)
            {
                if (storageInstanceRef.PersistentObjectID == null)
                    continue;
                updatedObjects[(storageInstanceRef.PersistentObjectID)] = storageInstanceRef;
            }
            #endregion

            #region Loads data on data table

            {
                Dictionary<MetaDataRepository.Class, List<XElement>> typedObjectElements = new Dictionary<MetaDataRepository.Class, List<XElement>>();

                if (DataLoaderMetadata.MemoryCell == null)
                {
                    foreach (var storageCell in DataLoaderMetadata.StorageCells)
                    {
                        if (!(storageCell is MetaDataLoadingSystem.StorageCell) || (storageCell as MetaDataLoadingSystem.StorageCell).XmlElement == null)
                            continue;

                        MetaDataRepository.Class _class = MetaDataRepository.Classifier.GetClassifier(storageCell.Type.GetExtensionMetaObject(typeof(Type)) as Type) as MetaDataRepository.Class;
                        List<XElement> objectElements = null;
                        if (!typedObjectElements.TryGetValue(_class, out objectElements))
                            typedObjectElements.Add(_class, (storageCell as MetaDataLoadingSystem.StorageCell).XmlElement.Elements().ToList());
                        else
                            objectElements.AddRange((storageCell as MetaDataLoadingSystem.StorageCell).XmlElement.Elements().ToList());
                    }
                }
                else
                {
                    foreach (MetaDataStorageInstanceRef storageInstanceRef in UpdatedObjects)
                    {
                        List<XElement> objectElements = null;
                        if (!typedObjectElements.TryGetValue(storageInstanceRef.Class, out objectElements))
                        {
                            objectElements = new List<XElement>();
                            typedObjectElements.Add(storageInstanceRef.Class, objectElements);
                        }
                        objectElements.Add(storageInstanceRef.TheStorageIstance as XElement);
                    }
                }
                foreach (var _class in typedObjectElements.Keys)
                {

                    foreach (XElement xmlElement in typedObjectElements[_class])
                    {
                        XElement objectElement = xmlElement;
                        if (objectElement.Name != "Object")
                            continue;
                        ulong OID = 0;
                        ulong.TryParse(objectElement.GetAttribute("oid"), out OID);
                        ObjectID objectID = new ObjectID(OID);

                        PersistenceLayer.ObjectID mc_ObjectID = null;

                        if (DataLoaderMetadata.MemoryCell != null)
                        {
                            PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = null;
                            if (updatedObjects.TryGetValue(objectID, out storageInstanceRef))
                            {
                                ObjectData objectData = null;
                                if (DataLoaderMetadata.MemoryCell.Objects.TryGetValue(storageInstanceRef.MemoryInstance, out objectData))
                                    mc_ObjectID = objectData.ObjectID;
                            }
                        }

                        if (path.Count > 0)
                        {

                            bool exist = false;
                            foreach (string nodeName in path)
                            {
                                exist = false;
                                foreach (XElement childNode in objectElement.Elements())
                                {
                                    if (childNode.Name == nodeName)
                                    {
                                        exist = true;
                                        objectElement = childNode as XElement;
                                        break;
                                    }
                                }
                                if (!exist)
                                    break;
                            }
                            if (!exist && path.Count > 0)
                                continue;
                        }
                        IDataRow row = TemporaryDataTable.NewRow();
                        row["ObjectID"] = objectID.GetTypedMemberValue<ulong>("ObjectID");
                        if (mc_ObjectID != null)
                        {
                            int i = 0;
                            foreach (var identityPart in mc_ObjectID.ObjectIdentityType.Parts)
                                row[identityPart.Name] = mc_ObjectID.GetPartValue(i++);
                        }
                        if (candidateForDeleteObjects.Count > 0 && candidateForDeleteObjects.ContainsKey(objectID))
                            continue;

                        #region Loads object the object if it is necessary

                        if (ObjectActivation)
                            row[row.Table.GetHashCode().ToString() + "_StorageIstance"] = objectElement;
                        #endregion

                        #region Loads with columns for the relation with the parent data node
                        associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        //if (associationEnd != null &&
                        //    !HasRelationIdentityColumns &&
                        //    !(associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                        //    associationEnd.Association.Specializations.Count == 0 &&
                        //    associationEnd.Association != DataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation))


                        bool flag = (associationEnd != null &&
                            !HasRelationIdentityColumns &&
                            !(associationEnd.Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany &&
                            associationEnd.Association.Specializations.Count == 0 &&
                            associationEnd.Association != DataNode.ParentDataNode.Classifier.ClassHierarchyLinkAssociation));
                        if (HasRelationReferenceColumns)
                        {

                            bool thereIsRelationChange = false;
                            if (updatedObjects.Count > 0 && updatedObjects.ContainsKey(objectID))
                            {
                                PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = updatedObjects[objectID];
                                OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink = null;
                                if (relationWithParentChanges.TryGetValue(objectID, out objectsLink))
                                {
                                    thereIsRelationChange = true;
                                    PersistenceLayer.ObjectID parrentObjectID = null; ;
                                    if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                    {
                                        if (associationEnd.IsRoleA)
                                            parrentObjectID = objectsLink.RoleB.ObjectID;
                                        else
                                            parrentObjectID = objectsLink.RoleA.ObjectID;

                                        foreach (var objectIdentityType in DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()].Keys)
                                        {
                                            if (objectIdentityType == parrentObjectID.ObjectIdentityType)
                                            {
                                                int i = 0;
                                                foreach (var identityPart in objectIdentityType.Parts)
                                                    row[identityPart.Name] = parrentObjectID.GetPartValue(i++);

                                                //if (associationEnd.IsRoleA)
                                                //    row[associationEnd.Association.Name + "RoleB_OID"] = parrentObjectID.GetTypedMemberValue<ulong>("ObjectID");
                                                //else
                                                //    row[associationEnd.Association.Name + "RoleA_OID"] = parrentObjectID.GetTypedMemberValue<ulong>("ObjectID");
                                            }
                                        }
                                    }
                                }
                            }


                            if (!thereIsRelationChange)
                            {
                                foreach (XElement relationElement in objectElement.Elements())
                                {
                                    var tagName = (ObjectStorage as MetaDataStorageSession).GetMappedTagName(associationEnd.GetOtherEnd().Identity.ToString());
                                    if ((!string.IsNullOrWhiteSpace( associationEnd.GetOtherEnd().Name)&& relationElement.Name == associationEnd.GetOtherEnd().Name)
                                        || (string.IsNullOrEmpty(associationEnd.GetOtherEnd().Name) && relationElement.Name == associationEnd.Association.Name + associationEnd.GetOtherEnd().Role + "Name")
                                        || relationElement.Name == tagName)
                                    {

                                        foreach (var objectIdentityType in DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()].Keys)
                                        {
                                            if (objectIdentityType == xmlStorgeIdentityType)
                                            {
                                                foreach (XElement oidElemant in relationElement.Elements())
                                                {
                                                    ulong value = 0;
                                                    ulong.TryParse(oidElemant.Value, System.Globalization.NumberStyles.None, CultureInfoHelper.GetCultureInfo(1033), out value);
                                                    row[objectIdentityType.Parts[0].Name] = value;
                                                    if (!oidElemant.HasAttribute("StorageCellReference"))
                                                    {
                                                        row[DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()][objectIdentityType].StorageIdentityColumn] = DataLoaderMetadata.QueryStorageID;
                                                    }
                                                    else
                                                    {
                                                        MetaDataLoadingSystem.StorageCellReference storageCellReference = (ObjectStorage.StorageMetaData as MetaDataLoadingSystem.Storage).StorageCellsReference[oidElemant.GetAttribute("StorageCellReference")];
                                                        int storageIndex = QueryStorageIdentities.IndexOf(storageCellReference.StorageIdentity);
                                                        row[DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()][objectIdentityType].StorageIdentityColumn] = storageIndex;
                                                        //storageCellReference.StorageIdentity
                                                    }

                                                    //if (associationEnd.IsRoleA)
                                                    //    row[associationEnd.Association.Name + "RoleB_OID"] = value;
                                                    //else
                                                    //    row[associationEnd.Association.Name + "RoleA_OID"] = value;

                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        if (associationEnd != null && associationEnd.Association == Classifier.ClassHierarchyLinkAssociation)
                        {

                            MetaDataRepository.ObjectIdentityType parentRelationObjectIdentityType = null;
                            var objectIdentityTypes = DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()];
                            {
                                foreach (var objectIdentityType in objectIdentityTypes.Keys)
                                {
                                    if (objectIdentityType == xmlStorgeIdentityType)
                                    {
                                        parentRelationObjectIdentityType = objectIdentityType;
                                        break;
                                    }
                                }
                            }
                            if (associationEnd.GetOtherEnd().IsRoleA)
                                row[parentRelationObjectIdentityType.Parts[0].Name] = ulong.Parse(objectElement.GetAttribute("RoleA"));
                            else
                                row[parentRelationObjectIdentityType.Parts[0].Name] = ulong.Parse(objectElement.GetAttribute("RoleB"));

                        }
                        if (DataNode.Recursive)
                        {
                            foreach (XElement relationElement in objectElement.Elements())
                            {
                                if (relationElement.Name == associationEnd.GetOtherEnd().Name)
                                {
                                    foreach (XElement oidElemant in relationElement.Elements())
                                    {
                                        int value = -1;
                                        int.TryParse(oidElemant.Value, out value);


                                        if (associationEnd.IsRoleA)
                                            row["Recursive_" + associationEnd.Association.Name + "RoleB_OID"] = value;
                                        else
                                            row["Recursive_" + associationEnd.Association.Name + "RoleA_OID"] = value;

                                        break;
                                    }
                                    break;
                                }
                            }

                        }


                        #endregion


                        foreach (DataNode subDataNode in DataNode.SubDataNodes)
                        {
                            #region Loads relationObject columns for the relation with the sub data node


                            if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                            {
                                MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                                object Value = attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole");
                                bool IsAssociationClassRole = false;
                                if (Value != null)
                                    IsAssociationClassRole = (bool)Value;
                                if (IsAssociationClassRole)
                                {
                                    bool IsRoleA = (bool)attribute.GetPropertyValue(typeof(bool), "MetaData", "IsRoleA");
                                    if (IsRoleA)
                                    {
                                        string RoleAOID = objectElement.GetAttribute("RoleA");
                                        row[subDataNode.Name] = RoleAOID;
                                    }
                                    else
                                    {
                                        string RoleBOID = objectElement.GetAttribute("RoleB");
                                        row[subDataNode.Name] = RoleBOID;
                                    }
                                }
                            }
                            #endregion

                            #region Loads relation table the relation with the sub data node


                            if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                            {
                                MetaDataRepository.AssociationEnd subDataNodeAssociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                                if (!SubNodesRoleNames.ContainsKey(subDataNode.Identity))
                                {
                                    SubNodesRoleNames[subDataNode.Identity] = new Dictionary<string, MetaDataRepository.AssociationEnd>() { { subDataNodeAssociationEnd.Name, subDataNodeAssociationEnd } };
                                    foreach (var association in subDataNodeAssociationEnd.Association.Specializations)
                                    {
                                        if (subDataNodeAssociationEnd.IsRoleA)
                                            SubNodesRoleNames[subDataNode.Identity].Add((ObjectStorage as MetaDataStorageSession).GetMappedTagName(association.RoleA.Identity.ToString()), association.RoleA);
                                        else
                                            SubNodesRoleNames[subDataNode.Identity].Add((ObjectStorage as MetaDataStorageSession).GetMappedTagName(association.RoleB.Identity.ToString()), association.RoleB);
                                    }
                                }
                            }

                            //if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd &&((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.MultiplicityType == MetaDataRepository.AssociationType.ManyToMany ||(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Specializations.Count > 0) &&(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association != DataNode.Classifier.ClassHierarchyLinkAssociation)
                            if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && subDataNode.ThroughRelationTable)
                            {

                                MetaDataRepository.AssociationEnd subDataNodeAssociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;

                                AssotiationTableRelationshipData relationshipData = RelationshipsData[subDataNode.Identity];
                                var dataSourceRelationObjectIdentityType = relationshipData.MasterDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()][relationshipData.MasterDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()].IndexOf(objectID.ObjectIdentityType)];

                                List<PersistenceLayerRunTime.ObjectsLink> relationChanges = null;
                                if (relationshipsChangesData.ContainsKey(subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()) &&
                                    relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()].TryGetValue(objectID, out relationChanges))
                                {

                                    foreach (PersistenceLayerRunTime.ObjectsLink objectsLink in relationChanges)
                                    {
                                        if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                        {
                                            IDataRow associationRow = relationshipData.Data.NewRow();
                                            PersistenceLayer.ObjectID relatedObject = null;
                                            string relatedObjectStorageIdentity = null;
                                            if (subDataNodeAssociationEnd.IsRoleA)
                                            {
                                                relatedObject = objectsLink.RoleA.ObjectID;
                                                relatedObjectStorageIdentity = objectsLink.RoleA.StorageIdentity;
                                            }
                                            else
                                            {
                                                relatedObject = objectsLink.RoleB.ObjectID;
                                                relatedObjectStorageIdentity = objectsLink.RoleA.StorageIdentity;
                                            }
                                            var SubNodeDataDataSourceRelationObjectIdentityType = relationshipData.DetailDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()][relationshipData.DetailDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()].IndexOf((relatedObject as PersistenceLayer.ObjectID).ObjectIdentityType)];

                                            int i = 0;
                                            foreach (var part in dataSourceRelationObjectIdentityType.Parts)
                                            {
                                                associationRow[part.Name] = objectID.GetPartValue(i);
                                                i++;
                                            }
                                            i = 0;
                                            foreach (var part in SubNodeDataDataSourceRelationObjectIdentityType.Parts)
                                            {
                                                associationRow[part.Name] = relatedObject.GetPartValue(i);
                                                i++;
                                            }
                                            if (subDataNodeAssociationEnd.IsRoleA)
                                            {
                                                associationRow["RoleBStorageIdentity"] = QueryStorageIdentities.IndexOf(DataLoaderMetadata.ObjectsContextIdentity);
                                                associationRow["RoleAStorageIdentity"] = QueryStorageIdentities.IndexOf(relatedObjectStorageIdentity);
                                            }
                                            else
                                            {
                                                associationRow["RoleAStorageIdentity"] = QueryStorageIdentities.IndexOf(DataLoaderMetadata.ObjectsContextIdentity);
                                                associationRow["RoleBStorageIdentity"] = QueryStorageIdentities.IndexOf(relatedObjectStorageIdentity);
                                            }

                                            relationshipData.Data.Rows.Add(associationRow);

                                        }
                                    }

                                }


                                foreach (XElement relationElement in objectElement.Elements())
                                {
                                    if (SubNodesRoleNames[subDataNode.Identity].ContainsKey(relationElement.Name.LocalName))
                                    {
                                        var roleAssociationEnd = SubNodesRoleNames[subDataNode.Identity][relationElement.Name.LocalName];

                                        bool multilingual = _class.IsMultilingual(roleAssociationEnd);
                                        if (multilingual)
                                        {
                                            XElement multiligualElement = relationElement.Element(OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
                                            if (multiligualElement != null)
                                                GetRelationsDataFromRoleElement(objectID, subDataNodeAssociationEnd, relationshipData, dataSourceRelationObjectIdentityType, relationChanges, multiligualElement);
                                        }
                                        else
                                            GetRelationsDataFromRoleElement(objectID, subDataNodeAssociationEnd, relationshipData, dataSourceRelationObjectIdentityType, relationChanges, relationElement);

                                        break;
                                    }
                                }


                            }
                            #endregion

                            AssotiationTableRelationshipData relData = null;

                            RelationshipsData.TryGetValue(subDataNode.Identity,out relData);
                        }
                        System.Collections.Generic.Dictionary<string, object> objectValues = null;
                        if (updatedObjects.Count > 0 && updatedObjects.ContainsKey(objectID))
                        {
                            objectValues = new System.Collections.Generic.Dictionary<string, object>();
                            foreach (OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute valueOfAttribute in updatedObjects[objectID].GetPersistentAttributeValues())
                            {
                                if (valueOfAttribute.ValueTypePath.Count == 0)
                                    objectValues[valueOfAttribute.Attribute.Identity.ToString()] = valueOfAttribute.Value;
                                else
                                    objectValues[valueOfAttribute.PathIdentity] = valueOfAttribute.Value;
                            }
                        }
                        foreach (DataNode subDataNode in DataNode.SubDataNodes)
                        {
                            #region Loads columns for the object Attributes
                            if (objectElement.Attribute(subDataNode.Name) != null && subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                            {
                                object value = null;
                                if (updatedObjects.Count > 0 && updatedObjects.ContainsKey(objectID))
                                {
                                    row[subDataNode.Name] = objectValues[subDataNode.AssignedMetaObject.Identity.ToString()];
                                    if (value != null && row.Table.Columns[subDataNode.Name].DataType == typeof(System.DateTime))
                                    {
                                        foreach (DataNode dateTimeDataNode in subDataNode.SubDataNodes)
                                        {
                                            if (dateTimeDataNode.Name == "Year")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Year;
                                            if (dateTimeDataNode.Name == "Month")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Month;
                                            if (dateTimeDataNode.Name == "Date")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Date;

                                            if (dateTimeDataNode.Name == "Day")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Day;

                                            if (dateTimeDataNode.Name == "DayOfWeek")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).DayOfWeek;

                                            if (dateTimeDataNode.Name == "DayOfYear")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).DayOfYear;

                                            if (dateTimeDataNode.Name == "Hour")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Hour;

                                            if (dateTimeDataNode.Name == "Minute")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Minute;

                                            if (dateTimeDataNode.Name == "Second")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Second;
                                        }

                                    }
                                }
                                else
                                {
                                    if (TemporaryDataTable.Columns[subDataNode.Name].DataType == typeof(System.DateTime))
                                    {
                                        System.IFormatProvider format = CultureInfoHelper.GetCultureInfo(0x0409);
                                        value = System.Convert.ChangeType(objectElement.GetAttribute(subDataNode.Name), TemporaryDataTable.Columns[subDataNode.Name].DataType, format);
                                    }
                                    else if (TemporaryDataTable.Columns[subDataNode.Name].DataType.GetMetaData().BaseType == typeof(System.Enum))
                                    {
                                        if (string.IsNullOrEmpty(objectElement.GetAttribute(subDataNode.Name)))
                                            value = System.Enum.GetValues(TemporaryDataTable.Columns[subDataNode.Name].DataType).GetValue(0);
                                        else
                                            value = System.Enum.Parse(TemporaryDataTable.Columns[subDataNode.Name].DataType, objectElement.GetAttribute(subDataNode.Name));
                                    }
                                    else
                                        value = System.Convert.ChangeType(objectElement.GetAttribute(subDataNode.Name), TemporaryDataTable.Columns[subDataNode.Name].DataType, CultureInfoHelper.GetCultureInfo(1033));

                                    //TODO : Αυτή η τεχνική δεν είναι αξιόπιστη επειδή όταν φορτώσει data σωσμένα με 
                                    //διαφορετικό locale θα κοπανάει.
                                    row[subDataNode.Name] = value;
                                    if (value != null && row.Table.Columns[subDataNode.Name].DataType == typeof(System.DateTime))
                                    {
                                        foreach (DataNode dateTimeDataNode in subDataNode.SubDataNodes)
                                        {
                                            if (dateTimeDataNode.Name == "Year")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Year;
                                            if (dateTimeDataNode.Name == "Month")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Month;
                                            if (dateTimeDataNode.Name == "Date")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Date;

                                            if (dateTimeDataNode.Name == "Day")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Day;

                                            if (dateTimeDataNode.Name == "DayOfWeek")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).DayOfWeek;

                                            if (dateTimeDataNode.Name == "DayOfYear")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).DayOfYear;

                                            if (dateTimeDataNode.Name == "Hour")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Hour;

                                            if (dateTimeDataNode.Name == "Minute")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Minute;

                                            if (dateTimeDataNode.Name == "Second")
                                                row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Second;


                                        }
                                    }
                                }

                            }
                            if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                            {
                                if (!updatedObjects.ContainsKey(objectID))
                                    (GetDataLoader(subDataNode) as DataLoader).LoadValueTypeData(objectID, row, objectElement.Element(subDataNode.Name) as XElement);
                                else
                                {
                                    string instanceOnMemoryColumn = row.Table.GetHashCode().ToString() + "_InstanceOnMemory";
                                    if (ObjectActivation)
                                    {
                                        row[instanceOnMemoryColumn] = updatedObjects[objectID];
                                        (GetDataLoader(subDataNode) as DataLoader).LoadValueTypeDataFromObject(updatedObjects[objectID], row, objectValues, objectElement.Element(subDataNode.Name) as XElement);
                                    }
                                    else
                                    {
                                        if (row.Table.Columns.Contains(instanceOnMemoryColumn))
                                        {
                                            row[instanceOnMemoryColumn] = updatedObjects[objectID];
                                            (GetDataLoader(subDataNode) as DataLoader).LoadValueTypeDataFromObject(updatedObjects[objectID], row, objectValues, objectElement.Element(subDataNode.Name) as XElement);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Loads columns for the relation with the subDataNodes
                            if (subDataNode.Type == DataNode.DataNodeType.Object && HasRelationReferenceColumnsFor(subDataNode))
                            {
                                MetaDataRepository.AssociationEnd subDataNodeAssociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                                if (subDataNodeAssociationEnd != null)
                                {
                                    //associationEnd = associationEnd.GetOtherEnd();
                                    if (Classifier.ClassHierarchyLinkAssociation == subDataNodeAssociationEnd.Association)
                                    {
                                        foreach (var entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()])
                                        {
                                            if (entry.Key == xmlStorgeIdentityType)
                                            {
                                                if (subDataNodeAssociationEnd.IsRoleA)
                                                    row[entry.Key.Parts[0].Name] = ulong.Parse(objectElement.GetAttribute("RoleA"));
                                                else
                                                    row[entry.Key.Parts[0].Name] = ulong.Parse(objectElement.GetAttribute("RoleB"));
                                            }
                                        }
                                        //if (subDataNodeassociationEnd.IsRoleA)
                                        //    row[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = objectElement.GetAttribute("RoleA");
                                        //else
                                        //    row[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = objectElement.GetAttribute("RoleB");
                                    }
                                    else
                                    {

                                        foreach (XElement relationElement in objectElement.Elements())
                                        {

                                            if (SubNodesRoleNames[subDataNode.Identity].ContainsKey(relationElement.Name.LocalName))
                                            {
                                                var roleAssociationEnd = SubNodesRoleNames[subDataNode.Identity][relationElement.Name.LocalName];
                                                bool multilingual = _class.IsMultilingual(roleAssociationEnd);
                                                if (multilingual)
                                                {
                                                    XElement multiligualElement = relationElement.Element(OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
                                                    if (multiligualElement != null)
                                                    {
                                                        foreach (XElement oidElemant in multiligualElement.Elements("oid"))
                                                        {
                                                            ulong value = 0;
                                                            ulong.TryParse(oidElemant.Value, out value);
                                                            foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                                            {
                                                                if (objectIdentityType == xmlStorgeIdentityType)
                                                                {
                                                                    row[objectIdentityType.Parts[0].Name] = value;
                                                                    break;
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {

                                                    foreach (XElement oidElemant in relationElement.Elements("oid"))
                                                    {
                                                        ulong value = 0;
                                                        ulong.TryParse(oidElemant.Value, out value);

                                                        foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                                        {
                                                            if (objectIdentityType == xmlStorgeIdentityType)
                                                            {
                                                                row[objectIdentityType.Parts[0].Name] = value;
                                                                break;
                                                            }
                                                        }
                                                        //if (subDataNodeassociationEnd.IsRoleA)
                                                        //    row[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = value;
                                                        //else
                                                        //    row[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = value;

                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }

                                        OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink = null;
                                        if (RelationWithSubDataNodeChanges.ContainsKey(subDataNode.Identity.ToString()) &&
                                            RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()].TryGetValue(objectID, out objectsLink))
                                        {
                                            PersistenceLayer.ObjectID subDataNodeObjectID = null; ;
                                            if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                                if (subDataNodeAssociationEnd.IsRoleA)
                                                    subDataNodeObjectID = objectsLink.RoleA.ObjectID;
                                                else
                                                    subDataNodeObjectID = objectsLink.RoleB.ObjectID;

                                            if (subDataNodeObjectID != null)
                                            {
                                                foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                                {
                                                    if (objectIdentityType == subDataNodeObjectID.ObjectIdentityType)
                                                    {
                                                        int i = 0;
                                                        foreach (var identityPart in objectIdentityType.Parts)
                                                            row[identityPart.Name] = subDataNodeObjectID.GetPartValue(i);
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                                {

                                                    foreach (var identityPart in objectIdentityType.Parts)
                                                        row[identityPart.Name] = System.DBNull.Value;

                                                }
                                            }

                                            //if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                            //    row[(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Name + "RoleA_OID"] = subDataNodeObjectID.GetTypedMemberValue<ulong>("ObjectID");
                                            //else
                                            //    row[(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Name + "RoleB_OID"] = subDataNodeObjectID.GetTypedMemberValue<ulong>("ObjectID");

                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        TemporaryDataTable.Rows.Add(row);

                    }
                }
            }
            #endregion

            #region Load new objects on table

            System.Collections.Generic.List<OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef> objects = null;
            if (DataLoaderMetadata.MemoryCell == null)
                objects = NewObjects;
            else
                objects = UpdatedObjects;


            foreach (OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef in NewObjects)
            {
                if (candidateForDeleteObjects.ContainsKey(storageInstanceRef.ObjectID))
                    continue;

                System.Collections.Generic.Dictionary<string, object> objectValues = null;
                objectValues = new System.Collections.Generic.Dictionary<string, object>();
                foreach (OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute valueOfAttribute in storageInstanceRef.GetPersistentAttributeValues())
                {
                    if (valueOfAttribute.ValueTypePath.Count == 0)
                        objectValues[valueOfAttribute.Attribute.Identity.ToString()] = valueOfAttribute.Value;
                    else
                        objectValues[valueOfAttribute.PathIdentity] = valueOfAttribute.Value;
                }

                IDataRow row = TemporaryDataTable.NewRow();
                row["ObjectID"] = storageInstanceRef.ObjectID.GetTypedMemberValue<ulong>("ObjectID");
                string instanceOnMemoryColumn = row.Table.GetHashCode().ToString() + "_InstanceOnMemory";
                if (ObjectActivation)
                    row[instanceOnMemoryColumn] = storageInstanceRef;
                else
                {
                    if (row.Table.Columns.Contains(instanceOnMemoryColumn))
                        row[instanceOnMemoryColumn] = storageInstanceRef;
                }

                #region Loads relation columns for the relation with the parent data node
                associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                if (!dataNode.ThroughRelationTable && HasRelationReferenceColumns)
                {
                    OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink = null;
                    if (relationWithParentChanges.TryGetValue(storageInstanceRef.ObjectID, out objectsLink))
                    {
                        PersistenceLayer.ObjectID parrentObjectID = null; ;
                        if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                        {
                            if (associationEnd.IsRoleA)
                                parrentObjectID = objectsLink.RoleB.ObjectID;
                            else
                                parrentObjectID = objectsLink.RoleA.ObjectID;


                            foreach (var objectIdentityType in DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()].Keys)
                            {
                                if (objectIdentityType == parrentObjectID.ObjectIdentityType)
                                {
                                    int i = 0;
                                    foreach (var identityPart in objectIdentityType.Parts)
                                        row[identityPart.Name] = parrentObjectID.GetPartValue(i++);

                                    //if (associationEnd.IsRoleA)
                                    //    row[associationEnd.Association.Name + "RoleB_OID"] = parrentObjectID.GetTypedMemberValue<ulong>("ObjectID");
                                    //else
                                    //    row[associationEnd.Association.Name + "RoleA_OID"] = parrentObjectID.GetTypedMemberValue<ulong>("ObjectID");
                                }
                            }
                        }

                    }
                    if (associationEnd != null && associationEnd.Association == Classifier.ClassHierarchyLinkAssociation)
                    {
                        PersistenceLayerRunTime.StorageInstanceAgent roleStorageInstanceRef = null;
                        if (associationEnd.GetOtherEnd().IsRoleA)
                            roleStorageInstanceRef = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(storageInstanceRef.Class.LinkClassRoleAFastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance));
                        else
                            roleStorageInstanceRef = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(storageInstanceRef.Class.LinkClassRoleBFastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance));

                        foreach (var objectIdentityType in DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()].Keys)
                        {
                            if (objectIdentityType == roleStorageInstanceRef.ObjectID.ObjectIdentityType)
                            {
                                int i = 0;
                                foreach (var identityPart in objectIdentityType.Parts)
                                    row[identityPart.Name] = roleStorageInstanceRef.ObjectID.GetPartValue(i++);
                            }
                        }




                        //if (associationEnd.GetOtherEnd().IsRoleA)
                        //{
                        //    object roleAObject = Member<object>.GetValue(storageInstanceRef.Class.LinkClassRoleAFastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                        //    row[associationEnd.Association.Name + "RoleA_OID"] = (PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(roleAObject).TransientObjectID).GetTypedMemberValue<ulong>("ObjectID");
                        //}
                        //else
                        //{
                        //    object roleBObject = Member<object>.GetValue(storageInstanceRef.Class.LinkClassRoleBFastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                        //    row[associationEnd.Association.Name + "RoleB_OID"] = (PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(roleBObject).TransientObjectID).GetTypedMemberValue<ulong>("ObjectID");
                        //}

                    }
                }




                #endregion

                object value = null;
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    MetaDataRepository.AssociationEnd subDataNodeAssociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                    #region Load relation table
                    if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd && subDataNode.ThroughRelationTable)
                    {
                        AssotiationTableRelationshipData relationshipData = RelationshipsData[subDataNode.Identity];
                        List<PersistenceLayerRunTime.ObjectsLink> relationChanges = null;
                        if (relationshipsChangesData.ContainsKey(subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()) &&
                            relationshipsChangesData[subDataNode.ParentDataNode.ValueTypePath.ToString() + subDataNode.AssignedMetaObject.Identity.ToString()].TryGetValue(storageInstanceRef.ObjectID, out relationChanges))
                        {
                            foreach (PersistenceLayerRunTime.ObjectsLink objectsLink in relationChanges)
                            {
                                if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                {
                                    IDataRow associationRow = relationshipData.Data.NewRow();
                                    PersistenceLayer.ObjectID relatedObject = null;
                                    string relatedObjectStorageIdentity = null;
                                    if (subDataNodeAssociationEnd.IsRoleA)
                                    {
                                        relatedObject = objectsLink.RoleA.ObjectID;
                                        relatedObjectStorageIdentity = objectsLink.RoleA.StorageIdentity;
                                    }
                                    else
                                    {
                                        relatedObject = objectsLink.RoleB.ObjectID;
                                        relatedObjectStorageIdentity = objectsLink.RoleB.StorageIdentity;
                                    }
                                    PersistenceLayer.ObjectID objectID = storageInstanceRef.ObjectID;



                                    var dataSourceRelationObjectIdentityType = relationshipData.MasterDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()][relationshipData.MasterDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()].IndexOf(objectID.ObjectIdentityType)];
                                    var SubNodeDataDataSourceRelationObjectIdentityType = relationshipData.DetailDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()][relationshipData.DetailDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()].IndexOf((relatedObject as PersistenceLayer.ObjectID).ObjectIdentityType)];

                                    int i = 0;
                                    foreach (var part in dataSourceRelationObjectIdentityType.Parts)
                                    {
                                        associationRow[part.Name] = objectID.GetPartValue(i);
                                        i++;
                                    }
                                    i = 0;
                                    foreach (var part in SubNodeDataDataSourceRelationObjectIdentityType.Parts)
                                    {
                                        associationRow[part.Name] = relatedObject.GetPartValue(i);
                                        i++;
                                    }
                                    if (subDataNodeAssociationEnd.IsRoleA)
                                    {
                                        associationRow["RoleBStorageIdentity"] = QueryStorageIdentities.IndexOf(storageInstanceRef.ObjectStorage.StorageMetaData.StorageIdentity);
                                        associationRow["RoleAStorageIdentity"] = QueryStorageIdentities.IndexOf(relatedObjectStorageIdentity);
                                    }
                                    else
                                    {
                                        associationRow["RoleAStorageIdentity"] = QueryStorageIdentities.IndexOf(storageInstanceRef.ObjectStorage.StorageMetaData.StorageIdentity);
                                        associationRow["RoleBStorageIdentity"] = QueryStorageIdentities.IndexOf(relatedObjectStorageIdentity);
                                    }

                                    //if (subDataNodeassociationEnd.IsRoleA)
                                    //{
                                    //    var roleA_OID = objectsLink.RoleA.TransientObjectID;

                                    //    associationRow[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = roleA_OID.GetMemberValue("ObjectID");
                                    //    associationRow[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = (storageInstanceRef.TransientObjectID).GetTypedMemberValue<ulong>("ObjectID");
                                    //}
                                    //else
                                    //{
                                    //    var roleB_OID = objectsLink.RoleB.TransientObjectID;
                                    //    associationRow[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = roleB_OID.GetMemberValue("ObjectID");
                                    //    associationRow[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = (storageInstanceRef.TransientObjectID).GetTypedMemberValue<ulong>("ObjectID");
                                    //}
                                    relationshipData.Data.Rows.Add(associationRow);

                                }
                            }

                        }
                    }
                    #endregion

                    #region Load relation object reference columns data
                    if ((subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) && Classifier.ClassHierarchyLinkAssociation == (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association)
                    {
                        if (subDataNodeAssociationEnd.IsRoleA)
                        {
                            PersistenceLayerRunTime.StorageInstanceAgent roleAObject = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(storageInstanceRef.Class.LinkClassRoleAFastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance));
                            foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                            {
                                if (objectIdentityType == roleAObject.ObjectID.ObjectIdentityType)
                                {
                                    int i = 0;
                                    foreach (var identityPart in objectIdentityType.Parts)
                                    {
                                        row[identityPart.Name] = roleAObject.ObjectID.GetPartValue(i);
                                        i++;
                                    }
                                    //(PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(roleAObject).TransientObjectID).
                                    //row[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = (PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(roleAObject).TransientObjectID).GetTypedMemberValue<ulong>("ObjectID");
                                }
                            }
                        }
                        else
                        {
                            PersistenceLayerRunTime.StorageInstanceAgent roleBObject = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(storageInstanceRef.Class.LinkClassRoleBFastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance));

                            foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                            {
                                if (objectIdentityType == roleBObject.ObjectID.ObjectIdentityType)
                                {
                                    int i = 0;
                                    foreach (var identityPart in objectIdentityType.Parts)
                                    {
                                        row[identityPart.Name] = roleBObject.ObjectID.GetPartValue(i);
                                        i++;
                                    }
                                    //(PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(roleAObject).TransientObjectID).
                                    //row[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = (PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(roleAObject).TransientObjectID).GetTypedMemberValue<ulong>("ObjectID");
                                }
                            }

                            //row[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = (PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceAgent(roleBObject).TransientObjectID).GetTypedMemberValue<ulong>("ObjectID");
                        }
                    }
                    #endregion

                    #region Load reference columns with subDataNodes
                    if ((subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) &&
                        Classifier.ClassHierarchyLinkAssociation != (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association &&
                        RelationWithSubDataNodeChanges.ContainsKey(subDataNode.Identity.ToString()))
                    {
                        OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink = null;

                        if (RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()].TryGetValue(storageInstanceRef.ObjectID, out objectsLink))
                        {
                            PersistenceLayer.ObjectID subDataNodeObjectID = null;
                            if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                    subDataNodeObjectID = objectsLink.RoleA.ObjectID;
                                else
                                    subDataNodeObjectID = objectsLink.RoleB.ObjectID;

                            foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                            {
                                if (objectIdentityType == subDataNodeObjectID.ObjectIdentityType)
                                {
                                    int i = 0;
                                    foreach (var identityPart in objectIdentityType.Parts)
                                        row[identityPart.Name] = subDataNodeObjectID.GetPartValue(i++);
                                    break;
                                }
                            }
                            //if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                            //    row[(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Name + "RoleA_OID"] = subDataNodeObjectID.GetTypedMemberValue<ulong>("ObjectID");
                            //else
                            //    row[(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Name + "RoleB_OID"] = subDataNodeObjectID.GetTypedMemberValue<ulong>("ObjectID");
                        }
                    }
                    #endregion


                    if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                        (GetDataLoader(subDataNode) as DataLoader).LoadValueTypeDataFromObject(storageInstanceRef, row, objectValues, null);

                    if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        value = objectValues[subDataNode.AssignedMetaObject.Identity.ToString()];
                        row[subDataNode.Name] = value;
                        if (value != null && row.Table.Columns[subDataNode.Name].DataType == typeof(System.DateTime))
                        {
                            foreach (DataNode dateTimeDataNode in subDataNode.SubDataNodes)
                            {
                                if (dateTimeDataNode.Name == "Year")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Year;
                                if (dateTimeDataNode.Name == "Month")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Month;
                                if (dateTimeDataNode.Name == "Date")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Date;

                                if (dateTimeDataNode.Name == "Day")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Day;

                                if (dateTimeDataNode.Name == "DayOfWeek")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).DayOfWeek;

                                if (dateTimeDataNode.Name == "DayOfYear")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).DayOfYear;

                                if (dateTimeDataNode.Name == "Hour")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Hour;

                                if (dateTimeDataNode.Name == "Minute")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Minute;

                                if (dateTimeDataNode.Name == "Second")
                                    row[subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Second;

                            }
                        }
                    }
                }
                TemporaryDataTable.Rows.Add(row);
            }
            #endregion

            var temporaryDataTable = TemporaryDataTable;
            if (TemporaryDataTable.Columns.Contains("ObjectID"))
            {
                temporaryDataTable= DataSource.DataObjectsInstantiator.CreateDataTable(false);
                foreach(var col in TemporaryDataTable.Columns.OfType<IDataColumn>())
                    temporaryDataTable.Columns.Add(col.ColumnName, col.DataType);



                foreach (var row in TemporaryDataTable.Rows.OfType<IDataRow>().OrderBy(x => x["ObjectID"]))
                {
                    var newRow = temporaryDataTable.NewRow();
                    newRow.ItemArray = row.ItemArray;
                    temporaryDataTable.Rows.Add(newRow);
                }
            }

            //if (DataNode.ParticipateInSelectClause || DataNode.ParticipateInAggregateFanction || DataNode.ObjectQuery.SelectListItems.Contains(DataNode))
            LoadDataLoaderTable(temporaryDataTable.CreateDataReader(), new Dictionary<string, string>(), false);

            //ActivateObjects();



        }
        /// <summary>
        /// GetsAssotiationTableRelationshipData
        /// </summary>
        /// <param name="objectID">
        /// storage instance element objectID
        /// </param>
        /// <param name="subDataNodeAssociationEnd">
        /// Defines the relationship AssociationEnd of sub DataNode
        /// </param>
        /// <param name="relationshipData">
        /// Defines relationship association teble
        /// </param>
        /// <param name="dataSourceRelationObjectIdentityType">
        /// objectIdentities Types
        /// </param>
        /// <param name="relationChanges">
        /// relation changes on under current transaction
        /// </param>
        /// <param name="relationElement">
        /// xml element with relations data 
        /// </param>
        private void GetRelationsDataFromRoleElement(ObjectID objectID, AssociationEnd subDataNodeAssociationEnd, AssotiationTableRelationshipData relationshipData, ObjectIdentityType dataSourceRelationObjectIdentityType, List<ObjectsLink> relationChanges, XElement relationElement)
        {
            foreach (XElement oidElemant in relationElement.Elements("oid"))
            {
                PersistenceLayer.ObjectID relatedObjectID = null;
                string relatedObjectStorageIdentity = null;

                if (oidElemant.Attribute("StorageCellReference") != null)
                {
                    ulong relatedOID = 0;
                    if (ulong.TryParse(oidElemant.Value, out relatedOID))
                        relatedObjectID = new ObjectID(relatedOID);
                    else
                        relatedObjectID = PersistenceLayer.ObjectID.Parse(oidElemant.Value, CultureInfoHelper.GetCultureInfo(1033));

                    MetaDataLoadingSystem.StorageCellReference storageCellReference = (ObjectStorage.StorageMetaData as MetaDataLoadingSystem.Storage).StorageCellsReference[oidElemant.GetAttribute("StorageCellReference")];
                    relatedObjectStorageIdentity = storageCellReference.StorageIdentity;
                }
                else
                {
                    ulong relatedOID = 0;
                    ulong.TryParse(oidElemant.Value, out relatedOID);
                    relatedObjectID = new ObjectID(relatedOID);
                    relatedObjectStorageIdentity = DataLoaderMetadata.ObjectsContextIdentity;
                }

                bool relationRemoved = false;
                if (relationChanges != null)
                {
                    foreach (PersistenceLayerRunTime.ObjectsLink objectsLink in relationChanges)
                    {
                        if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Removed &&
                            subDataNodeAssociationEnd.Association.Identity == objectsLink.Association.Identity)
                        {
                            if (subDataNodeAssociationEnd.IsRoleA)
                            {
                                if (objectsLink.RoleA.PersistentObjectID == null)
                                    continue;
                                if (objectsLink.RoleA.PersistentObjectID.Equals(relatedObjectID))
                                {
                                    relationRemoved = true;
                                    continue;
                                }

                            }
                            else
                            {
                                if (objectsLink.RoleB.PersistentObjectID == null)
                                    continue;
                                if (objectsLink.RoleB.PersistentObjectID.Equals(relatedObjectID))
                                {
                                    relationRemoved = true;
                                    continue;
                                }
                            }
                        }
                    }
                }
                if (relationRemoved)
                    continue;

                IDataRow associationRow = relationshipData.Data.NewRow();
                var SubNodeDataDataSourceRelationObjectIdentityType = relationshipData.DetailDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()][relationshipData.DetailDataSourceReferenceObjectIdentityTypes[subDataNodeAssociationEnd.Identity.ToString()].IndexOf((relatedObjectID as PersistenceLayer.ObjectID).ObjectIdentityType)];
                int i = 0;
                foreach (var part in dataSourceRelationObjectIdentityType.Parts)
                {
                    associationRow[part.Name] = objectID.GetPartValue(i);
                    i++;
                }
                i = 0;
                foreach (var part in SubNodeDataDataSourceRelationObjectIdentityType.Parts)
                {
                    associationRow[part.Name] = relatedObjectID.GetPartValue(i);
                    i++;
                }
                if (subDataNodeAssociationEnd.IsRoleA)
                {
                    associationRow["RoleBStorageIdentity"] = QueryStorageIdentities.IndexOf(DataLoaderMetadata.ObjectsContextIdentity);
                    associationRow["RoleAStorageIdentity"] = QueryStorageIdentities.IndexOf(relatedObjectStorageIdentity);
                }
                else
                {
                    associationRow["RoleAStorageIdentity"] = QueryStorageIdentities.IndexOf(DataLoaderMetadata.ObjectsContextIdentity);
                    associationRow["RoleBStorageIdentity"] = QueryStorageIdentities.IndexOf(relatedObjectStorageIdentity);
                }
                //{
                //    associationRow[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = value;
                //    associationRow[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = objectID.GetTypedMemberValue<ulong>("ObjectID"); ;
                //}
                //else
                //{

                //    associationRow[subDataNodeassociationEnd.Association.Name + "RoleB_OID"] = value;
                //    associationRow[subDataNodeassociationEnd.Association.Name + "RoleA_OID"] = objectID.GetTypedMemberValue<ulong>("ObjectID"); ;
                //}
                relationshipData.Data.Rows.Add(associationRow);
            }
        }

        private OOAdvantech.MetaDataRepository.AssociationEnd GetRelationAssociationEnd(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, string associationEndIdentity)
        {
            if (associationEnd.Identity.ToString() == associationEndIdentity)
                return associationEnd;
            else
            {
                foreach (var association in associationEnd.Association.Specializations)
                {
                    if (associationEnd.IsRoleA && association.RoleA.Identity.ToString() == associationEndIdentity)
                        return association.RoleA;
                    else if (association.RoleB.Identity.ToString() == associationEndIdentity)
                        return association.RoleB;

                    if (associationEnd.IsRoleA)
                    {
                        OOAdvantech.MetaDataRepository.AssociationEnd relationAssocoationEnd = GetRelationAssociationEnd(associationEnd.Association.RoleA, associationEndIdentity);
                        if (relationAssocoationEnd != null)
                            return relationAssocoationEnd;
                    }
                    else
                    {
                        OOAdvantech.MetaDataRepository.AssociationEnd relationAssocoationEnd = GetRelationAssociationEnd(associationEnd.Association.RoleB, associationEndIdentity);
                        if (relationAssocoationEnd != null)
                            return relationAssocoationEnd;
                    }
                }
            }
            return null;

        }


        protected override void OnPrepareForDataLoading()
        {
            if (DataNode.ValueTypePath.Count == 0)
            {
                if (UpdatedObjects.Count > 0 || NewObjects.Count > 0 || CandidateForDeleteObjects.Count > 0)
                    GetRelationChanges();
            }
        }

        private void LoadValueTypeDataFromObject(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef, IDataRow row, System.Collections.Generic.Dictionary<string, object> objectValues, XElement objectElement)
        {
            if (storageInstanceRef != null)
            {

                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    #region Loads columns for the object Attributes

                    if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {
                        object value = objectValues[DataNode.ValueTypePath + ".(" + subDataNode.AssignedMetaObject.Identity.ToString() + ")"];
                        row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name] = value;
                        if (value != null && row.Table.Columns[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name].DataType == typeof(System.DateTime))
                        {
                            foreach (DataNode dateTimeDataNode in subDataNode.SubDataNodes)
                            {
                                if (dateTimeDataNode.Name == "Year")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Year;
                                if (dateTimeDataNode.Name == "Month")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Month;
                                if (dateTimeDataNode.Name == "Date")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Date;
                                if (dateTimeDataNode.Name == "Day")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Day;
                                if (dateTimeDataNode.Name == "DayOfWeek")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).DayOfWeek;
                                if (dateTimeDataNode.Name == "DayOfYear")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).DayOfYear;
                                if (dateTimeDataNode.Name == "Hour")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Hour;
                                if (dateTimeDataNode.Name == "Minute")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Minute;
                                if (dateTimeDataNode.Name == "Second")
                                    row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name + "_" + dateTimeDataNode.Name] = ((DateTime)value).Second;
                            }
                        }
                    }

                    #endregion

                    #region Load reference columns with subDataNodes


                    if ((subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd) &&
                        Classifier.ClassHierarchyLinkAssociation != (subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association &&
                        HasRelationReferenceColumnsFor(subDataNode))
                    {
                        MetaDataRepository.AssociationEnd subDataNodeAssociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;

                        if (subDataNodeAssociationEnd != null)
                        {
                            #region Load SubNodes role names
                            if (!SubNodesRoleNames.ContainsKey(subDataNode.Identity))
                            {
                                SubNodesRoleNames[subDataNode.Identity] = new Dictionary<string, MetaDataRepository.AssociationEnd> { { subDataNodeAssociationEnd.Name, subDataNodeAssociationEnd } };
                                foreach (var association in subDataNodeAssociationEnd.Association.Specializations)
                                {
                                    if (subDataNodeAssociationEnd.IsRoleA)
                                        SubNodesRoleNames[subDataNode.Identity].Add((ObjectStorage as MetaDataStorageSession).GetMappedTagName(association.RoleA.Identity.ToString()), association.RoleA);
                                    else
                                        SubNodesRoleNames[subDataNode.Identity].Add((ObjectStorage as MetaDataStorageSession).GetMappedTagName(association.RoleB.Identity.ToString()), association.RoleB);
                                }
                            }
                            #endregion
                        }


                        #region Load reference columns from storage instance
                        if (objectElement != null)
                        {
                            foreach (XElement relationElemant in objectElement.Elements())
                            {
                                if (SubNodesRoleNames[subDataNode.Identity].ContainsKey(relationElemant.Name.LocalName))
                                {

                                    //bool multilingual = _class.IsMultilingual(roleAssociationEnd);
                                    //if (multilingual)
                                    //{
                                    //    XElement multiligualElement = relationElement.Element(OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);

                                    //    _class
                                    //    var roleAssociationEnd = SubNodesRoleNames[subDataNode.Identity][relationElemant.Name.LocalName];
                                    foreach (XElement oidElemant in relationElemant.Elements())
                                    {
                                        PersistenceLayer.ObjectID relatedObjectID = null;
                                        string relatedObjectStorageIdentity = null;
                                        if (oidElemant.Attribute("StorageCellReference") != null)
                                        {
                                            relatedObjectID = PersistenceLayer.ObjectID.Parse(oidElemant.Value, CultureInfoHelper.GetCultureInfo(1033));
                                            MetaDataLoadingSystem.StorageCellReference storageCellReference = (ObjectStorage.StorageMetaData as MetaDataLoadingSystem.Storage).StorageCellsReference[oidElemant.GetAttribute("StorageCellReference")];
                                            relatedObjectStorageIdentity = storageCellReference.StorageIdentity;
                                        }
                                        else
                                        {
                                            ulong relatedOID = 0;
                                            ulong.TryParse(oidElemant.Value, out relatedOID);
                                            relatedObjectID = new ObjectID(relatedOID);
                                            relatedObjectStorageIdentity = ObjectStorage.StorageMetaData.StorageIdentity;
                                        }

                                        //ulong value = 0;
                                        //ulong.TryParse(oidElemant.InnerText, out value);
                                        foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                        {
                                            if (objectIdentityType == relatedObjectID.ObjectIdentityType)
                                            {
                                                int i = 0;
                                                foreach (var part in objectIdentityType.Parts)
                                                {
                                                    row[part.Name] = relatedObjectID.GetPartValue(i);
                                                    i++;
                                                }
                                                break;
                                            }
                                        }




                                        //ulong value = 0;
                                        //ulong.TryParse(oidElemant.InnerText, out value);
                                        //foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                        //{
                                        //    if (objectIdentityType == ObjectID.XMLObjectIdentityType)
                                        //    {
                                        //        row[objectIdentityType.Parts[0].Name] = value;
                                        //        break;
                                        //    }
                                        //}
                                        break;
                                    }
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Load relation changes for subDataNode

                        if (!RelationWithSubDataNodeChanges.ContainsKey(subDataNode.Identity.ToString()))
                        {

                            RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()] = new Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink>();
                            Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink> relationChanges = RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()];
                            foreach (OOAdvantech.PersistenceLayerRunTime.ObjectsLink relationChengeObjectsLink in GetRelationChanges(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, subDataNode))
                            {
                                if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                    relationChanges[relationChengeObjectsLink.RoleB.ObjectID] = relationChengeObjectsLink;
                                else
                                    relationChanges[relationChengeObjectsLink.RoleA.ObjectID] = relationChengeObjectsLink;
                            }

                        }
                        #endregion

                        if (RelationWithSubDataNodeChanges.ContainsKey(subDataNode.Identity.ToString()))
                        {
                            #region Load reference columns from relation changes
                            OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink = null;
                            if (RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()].TryGetValue(storageInstanceRef.ObjectID, out objectsLink))
                            {
                                PersistenceLayer.ObjectID subDataNodeObjectID = null;
                                if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                    if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                        subDataNodeObjectID = objectsLink.RoleA.ObjectID;
                                    else
                                        subDataNodeObjectID = objectsLink.RoleB.ObjectID;

                                foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                {
                                    if (subDataNodeObjectID != null)
                                    {
                                        if (objectIdentityType == subDataNodeObjectID.ObjectIdentityType)
                                        {
                                            int i = 0;
                                            foreach (var identityPart in objectIdentityType.Parts)
                                                row[identityPart.Name] = subDataNodeObjectID.GetPartValue(i++);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        int i = 0;
                                        foreach (var identityPart in objectIdentityType.Parts)
                                            row[identityPart.Name] = System.DBNull.Value;
                                    }
                                }
                                //if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                //    row[(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Name + "RoleA_OID"] = subDataNodeObjectID.GetTypedMemberValue<ulong>("ObjectID");
                                //else
                                //    row[(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).Association.Name + "RoleB_OID"] = subDataNodeObjectID.GetTypedMemberValue<ulong>("ObjectID");
                            }
                            #endregion
                        }
                    }
                    #endregion

                    if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                    {
                        if (objectElement != null)
                            (GetDataLoader(subDataNode) as DataLoader).LoadValueTypeDataFromObject(storageInstanceRef, row, objectValues, objectElement.Element(subDataNode.Name) as XElement);
                        else
                            (GetDataLoader(subDataNode) as DataLoader).LoadValueTypeDataFromObject(storageInstanceRef, row, objectValues, null);
                    }
                }
            }

        }


        private void LoadValueTypeData(ObjectID objectID, IDataRow row, XElement objectElement)
        {
            if (objectElement != null)
            {
                if (ObjectActivation)
                    row[DataNode.ValueTypePathDiscription + row.Table.GetHashCode().ToString() + "_StorageIstance"] = objectElement;
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {

                    if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {

                        #region Loads columns for the Attributes
                        object value = null;
                        if (row.Table.Columns[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name].DataType == typeof(System.DateTime))
                        {
                            System.IFormatProvider format = CultureInfoHelper.GetCultureInfo(0x0409);
                            value = System.Convert.ChangeType(objectElement.GetAttribute(subDataNode.Name), TemporaryDataTable.Columns[subDataNode.Name].DataType, format);
                        }
                        else if (row.Table.Columns[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name].DataType.GetMetaData().BaseType == typeof(System.Enum))
                        {
                            if (string.IsNullOrEmpty(objectElement.GetAttribute(subDataNode.Name)))
                                value = System.Enum.GetValues(row.Table.Columns[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name].DataType).GetValue(0);
                            else
                                value = System.Enum.Parse(row.Table.Columns[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name].DataType, objectElement.GetAttribute(subDataNode.Name));
                        }
                        else
                            value = System.Convert.ChangeType(objectElement.GetAttribute(subDataNode.Name), row.Table.Columns[DataNode.Name + "_" + subDataNode.Name].DataType, CultureInfoHelper.GetCultureInfo(1033));
                        row[DataNode.ValueTypePathDiscription + "_" + subDataNode.Name] = value;
                        #endregion

                    }
                    else if (subDataNode.Type == DataNode.DataNodeType.Object && HasRelationReferenceColumnsFor(subDataNode))
                    {
                        MetaDataRepository.AssociationEnd subDataNodeAssociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        if (subDataNodeAssociationEnd != null)
                        {
                            #region Load SubNodes role names
                            if (!SubNodesRoleNames.ContainsKey(subDataNode.Identity))
                            {
                                SubNodesRoleNames[subDataNode.Identity] = new Dictionary<string, MetaDataRepository.AssociationEnd> { { subDataNodeAssociationEnd.Name, subDataNodeAssociationEnd } };
                                foreach (var association in subDataNodeAssociationEnd.Association.Specializations)
                                {
                                    if (subDataNodeAssociationEnd.IsRoleA)
                                        SubNodesRoleNames[subDataNode.Identity].Add((ObjectStorage as MetaDataStorageSession).GetMappedTagName(association.RoleA.Identity.ToString()), association.RoleA);
                                    else
                                        SubNodesRoleNames[subDataNode.Identity].Add((ObjectStorage as MetaDataStorageSession).GetMappedTagName(association.RoleB.Identity.ToString()), association.RoleB);
                                }
                            }
                            #endregion
                        }
                        if (subDataNodeAssociationEnd != null)
                        {
                            //associationEnd = associationEnd.GetOtherEnd();
                            if (Classifier.ClassHierarchyLinkAssociation == subDataNodeAssociationEnd.Association)
                            {
                                foreach (var entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()])
                                {
                                    if (entry.Key == ObjectID.XMLObjectIdentityType)
                                    {
                                        if (subDataNodeAssociationEnd.IsRoleA)
                                            row[entry.Key.Parts[0].Name] = ulong.Parse(objectElement.GetAttribute("RoleA"));
                                        else
                                            row[entry.Key.Parts[0].Name] = ulong.Parse(objectElement.GetAttribute("RoleB"));
                                    }
                                }
                            }
                            else
                            {

                                foreach (XElement relationElemant in objectElement.Elements())
                                {
                                    if (SubNodesRoleNames[subDataNode.Identity].ContainsKey(relationElemant.Name.LocalName))
                                    {
                                        //var roleAssociationEnd = SubNodesRoleNames[subDataNode.Identity][relationElemant.Name.LocalName];
                                        //_class
                                        //bool multilingual = _class.IsMultilingual(roleAssociationEnd);
                                        foreach (XElement oidElemant in relationElemant.Elements())
                                        {

                                            PersistenceLayer.ObjectID relatedObjectID = null;
                                            string relatedObjectStorageIdentity = null;
                                            if (oidElemant.Attribute("StorageCellReference") != null)
                                            {
                                                relatedObjectID = PersistenceLayer.ObjectID.Parse(oidElemant.Value, CultureInfoHelper.GetCultureInfo(1033));
                                                MetaDataLoadingSystem.StorageCellReference storageCellReference = (ObjectStorage.StorageMetaData as MetaDataLoadingSystem.Storage).StorageCellsReference[oidElemant.GetAttribute("StorageCellReference")];
                                                relatedObjectStorageIdentity = storageCellReference.StorageIdentity;
                                            }
                                            else
                                            {
                                                ulong relatedOID = 0;
                                                ulong.TryParse(oidElemant.Value, out relatedOID);
                                                relatedObjectID = new ObjectID(relatedOID);
                                                relatedObjectStorageIdentity = ObjectStorage.StorageMetaData.StorageIdentity;
                                            }
                                            //ulong value = 0;
                                            //ulong.TryParse(oidElemant.InnerText, out value);
                                            foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                            {
                                                if (objectIdentityType == relatedObjectID.ObjectIdentityType)
                                                {
                                                    int i = 0;
                                                    foreach (var part in objectIdentityType.Parts)
                                                    {
                                                        row[part.Name] = relatedObjectID.GetPartValue(i);
                                                        i++;
                                                    }
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                }

                                if (!RelationWithSubDataNodeChanges.ContainsKey(subDataNode.Identity.ToString()))
                                {
                                    RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()] = new Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink>();
                                    Dictionary<PersistenceLayer.ObjectID, OOAdvantech.PersistenceLayerRunTime.ObjectsLink> relationChanges = RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()];
                                    foreach (OOAdvantech.PersistenceLayerRunTime.ObjectsLink relationChengeObjectsLink in GetRelationChanges(subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd, subDataNode))
                                    {
                                        if ((subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd).IsRoleA)
                                            relationChanges[relationChengeObjectsLink.RoleB.ObjectID] = relationChengeObjectsLink;
                                        else
                                            relationChanges[relationChengeObjectsLink.RoleA.ObjectID] = relationChengeObjectsLink;
                                    }
                                }

                                OOAdvantech.PersistenceLayerRunTime.ObjectsLink objectsLink = null;
                                if (RelationWithSubDataNodeChanges.ContainsKey(subDataNode.Identity.ToString()) &&
                                    RelationWithSubDataNodeChanges[subDataNode.Identity.ToString()].TryGetValue(objectID, out objectsLink))
                                {
                                    PersistenceLayer.ObjectID subDataNodeObjectID = null; ;
                                    if (objectsLink.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                        if (subDataNodeAssociationEnd.IsRoleA)
                                            subDataNodeObjectID = objectsLink.RoleA.ObjectID;
                                        else
                                            subDataNodeObjectID = objectsLink.RoleB.ObjectID;

                                    if (subDataNodeObjectID != null)
                                    {
                                        foreach (var objectIdentityType in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNodeAssociationEnd.Identity.ToString()].Keys)
                                        {
                                            if (objectIdentityType == subDataNodeObjectID.ObjectIdentityType)
                                            {
                                                int i = 0;
                                                foreach (var identityPart in objectIdentityType.Parts)
                                                    row[identityPart.Name] = subDataNodeObjectID.GetPartValue(i);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                    {
                        (GetDataLoader(subDataNode) as DataLoader).LoadValueTypeData(objectID, row, objectElement.Element(subDataNode.Name) as XElement);
                    }

                }
            }

        }

        private void AddTableDataColumns(IDataTable table)
        {
            string valueTypePrefix = null;
            if (!string.IsNullOrEmpty(DataNode.ValueTypePathDiscription))
                valueTypePrefix = DataNode.ValueTypePathDiscription + "_";

            #region Adds columns for the relation with the parent data node
            MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
            if (associationEnd != null)
            {
                if (!HasRelationIdentityColumns || DataLoaderMetadata.MemoryCell != null)
                {

                    foreach (var ObjectIdentityTypes in DataSourceRelationsColumnsWithParent.Values)
                    {
                        foreach (var objectIdentityType in ObjectIdentityTypes.Keys)
                        {
                            foreach (var identityPart in objectIdentityType.Parts)
                            {
                                table.Columns.Add(identityPart.Name, identityPart.Type);
                            }
                            table.Columns.Add(ObjectIdentityTypes[objectIdentityType].StorageIdentityColumn, typeof(int));
                        }
                    }
                }
                //if (associationEnd.IsRoleA)
                //    table.Columns.Add(valueTypePrefix + associationEnd.Association.Name + "RoleB_OID", typeof(ulong));
                //else
                //    table.Columns.Add(valueTypePrefix + associationEnd.Association.Name + "RoleA_OID", typeof(ulong));


            }
            if (DataNode.Recursive)
            {
                if (associationEnd.IsRoleA)
                    table.Columns.Add("Recursive_" + associationEnd.Association.Name + "RoleB_OID", typeof(ulong));
                else
                    table.Columns.Add("Recursive_" + associationEnd.Association.Name + "RoleA_OID", typeof(ulong));

            }
            #endregion

            #region Adds columns for the relation with the subDataNodes
            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {

                MetaDataRepository.AssociationEnd subDataNodeassociationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                if (HasRelationReferenceColumnsFor(subDataNode))//HasSubNodeRelationIdentityColumns(subDataNode))
                {

                    foreach (var entry in DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity])
                    {
                        foreach (var objectIdentityType in entry.Value.Keys)
                        {
                            foreach (var identityPart in objectIdentityType.Parts)
                            {
                                if (!table.Columns.Contains(identityPart.Name))
                                    table.Columns.Add(identityPart.Name, identityPart.Type);
                            }
                        }
                        //if (subDataNodeassociationEnd.IsRoleA)
                        //{
                        //    if (!table.Columns.Contains(valueTypePrefix + subDataNodeassociationEnd.Association.Name + "RoleA_OID"))
                        //        table.Columns.Add(valueTypePrefix + subDataNodeassociationEnd.Association.Name + "RoleA_OID", typeof(ulong));
                        //}
                        //else
                        //{
                        //    if (!table.Columns.Contains(valueTypePrefix + subDataNodeassociationEnd.Association.Name + "RoleB_OID"))
                        //        table.Columns.Add(valueTypePrefix + subDataNodeassociationEnd.Association.Name + "RoleB_OID", typeof(ulong));
                        //}
                    }
                }
            }
            #endregion

            #region Adds columns for the object Attributes
            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode is AggregateExpressionDataNode)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Count)
                        table.Columns.Add(subDataNode.Alias, typeof(int));
                    else
                        table.Columns.Add(subDataNode.Alias, (subDataNode as AggregateExpressionDataNode).ArithmeticExpression.ResultType);
                }

                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                {
                    MetaDataRepository.Attribute attribute = subDataNode.AssignedMetaObject as MetaDataRepository.Attribute;
                    object Value = attribute.GetPropertyValue(typeof(bool), "MetaData", "AssociationClassRole");
                    bool IsAssociationClassRole = false;
                    if (Value != null)
                        IsAssociationClassRole = (bool)Value;
                    if (IsAssociationClassRole)
                    {
                        table.Columns.Add(valueTypePrefix + subDataNode.Name, typeof(ulong));
                    }
                    else
                    {
                        System.Type type = (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).Type.GetExtensionMetaObject<System.Type>();
                        table.Columns.Add(valueTypePrefix + subDataNode.Name, type);
                        if (type == typeof(System.DateTime))
                        {
                            foreach (DataNode dateTimeDataNode in subDataNode.SubDataNodes)
                            {
                                System.Type columnType = dateTimeDataNode.Classifier.GetExtensionMetaObject<System.Type>();
                                table.Columns.Add(valueTypePrefix + subDataNode.Name + "_" + dateTimeDataNode.Name, columnType);
                            }
                        }
                    }
                }

            }
            #endregion

            foreach (DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.Object && subDataNode.AssignedMetaObject is MetaDataRepository.Attribute)
                    (subDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).AddTableDataColumns(TemporaryDataTable);
            }

            #region Adds columns for the object if it is necessary
            if (ObjectActivation)
            {
                //System.Type objectType = null;
                //objectType = DataNode.Classifier.GetExtensionMetaObject<System.Type>();
                table.Columns.Add(DataNode.ValueTypePathDiscription + table.GetHashCode().ToString() + "_StorageIstance", typeof(object));
            }
            if (ObjectActivation || NewObjects.Count > 0 || UpdatedObjects.Count > 0)
            {
                if (!table.Columns.Contains(table.GetHashCode().ToString() + "_InstanceOnMemory"))
                    table.Columns.Add(table.GetHashCode().ToString() + "_InstanceOnMemory", typeof(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef));
            }


            #endregion

        }

        System.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>>> _DataSourceRelationsColumnsWithSubDataNodes;
        ///<summary>
        ///This property defines the relation columns, 
        ///which used in table relation with subDataNodes data source. 
        ///</summary>
        public override System.Collections.Generic.Dictionary<SubDataNodeIdentity, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get
            {
                if (_DataSourceRelationsColumnsWithSubDataNodes != null)
                    return _DataSourceRelationsColumnsWithSubDataNodes;
                MetaDataRepository.ObjectIdentityType identityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(ulong)) });
                _DataSourceRelationsColumnsWithSubDataNodes = new Dictionary<Guid, Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>>();
                List<DataNode> relatedDataNodes = new List<DataNode>();
                foreach (DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.Object)
                        relatedDataNodes.Add(subDataNode);
                    else if (subDataNode.Type == DataNode.DataNodeType.Group && DataNode.Type == DataNode.DataNodeType.Object)
                    {
                        relatedDataNodes.Add(subDataNode);
                        relatedDataNodes.AddRange((subDataNode as GroupDataNode).GroupingDataNodes);
                    }
                }
                foreach (DataNode subDataNode in relatedDataNodes)
                {
                    MetaDataRepository.AssociationEnd associationEnd = subDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;

                    if (associationEnd != null)
                    {
                        associationEnd = associationEnd.GetOtherEnd();
                        System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();

                        if (HasRelationIdentityColumnsFor(subDataNode))
                        {
                            dataColumns.Add("ObjectID");
                            System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns> relationColumns = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                            relationColumns[new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(ulong)) })] = new RelationColumns(dataColumns, "OSM_StorageIdentity");
                            _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity] = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>();
                            _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNode.AssignedMetaObject.Identity.ToString()] = relationColumns;
                        }
                        else
                        {

                            //if (associationEnd.IsRoleA)
                            //    dataColumns.Add(associationEnd.Association.Name + "RoleB_OID");
                            //else
                            //    dataColumns.Add(associationEnd.Association.Name + "RoleA_OID");
                            //System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>> relationColumns = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>();
                            //relationColumns[new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart(dataColumns[0], "ObjectID", typeof(ulong)) })] = dataColumns;
                            //_DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity] = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, System.Collections.Generic.List<string>>>();
                            //_DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNode.AssignedMetaObject.Identity.ToString()] = relationColumns;

                            string prefix = null;
                            DataNode datanode = DataNode;
                            while (datanode != null && datanode.AssignedMetaObject is MetaDataRepository.Attribute)
                            {
                                prefix = (datanode.AssignedMetaObject as MetaDataRepository.Attribute).CaseInsensitiveName + "_" + prefix;
                                datanode = datanode.ParentDataNode;
                            }

                            System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns> relationColumns = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                            foreach (var identityPart in identityType.Parts)
                            {
                                if (associationEnd.IsRoleA)
                                    dataColumns.Add(prefix + associationEnd.Association.CaseInsensitiveName + "_" + identityPart.PartTypeName + "B");
                                else
                                    dataColumns.Add(prefix + associationEnd.Association.CaseInsensitiveName + "_" + identityPart.PartTypeName + "A");
                                if (associationEnd.Identity == DataNode.AssignedMetaObjectIdenty)
                                    relationColumns[new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart(dataColumns[0], "ObjectID", typeof(ulong)) })] = new RelationColumns(dataColumns, prefix + associationEnd.ReferenceStorageIdentityColumnName + "_SubDataNode");
                                else
                                    relationColumns[new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart(dataColumns[0], "ObjectID", typeof(ulong)) })] = new RelationColumns(dataColumns, prefix + associationEnd.ReferenceStorageIdentityColumnName);

                            }
                            _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity] = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>();
                            _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNode.AssignedMetaObject.Identity.ToString()] = relationColumns;

                        }
                    }
                    else if (subDataNode.AssignedMetaObject is MetaDataRepository.Attribute && (subDataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                    {
                        System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                        dataColumns.Add("ObjectID");
                        System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns> relationColumns = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                        relationColumns[identityType] = new RelationColumns(dataColumns, "OSM_StorageIdentity");
                        _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity] = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>();
                        _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNode.Identity.ToString()] = relationColumns;
                        //System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                        //int i = 0;
                        //foreach (RDBMSMetaDataRepository.Column column in (Classifier as RDBMSMetaDataRepository.MappedClassifier).ObjectIDColumns)
                        //    dataColumns.Add(column.Name);
                        //_ParentRelationColumns[dataNode.Identity] = dataColumns;

                    }
                    else if (subDataNode.Type == DataNode.DataNodeType.Group)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in ObjectIdentityTypes)
                        {
                            //System.Collections.Generic.List<string> dataColumns = new System.Collections.Generic.List<string>();
                            //List<MetaDataRepository.IIdentityPart> parts = new List<OOAdvantech.MetaDataRepository.IIdentityPart>();
                            //foreach (MetaDataRepository.IIdentityPart part in storageCell.MainTable.ObjectIDColumns)
                            //    parts.Add(new MetaDataRepository.IdentityPart(part));
                            //OOAdvantech.MetaDataRepository.ObjectIdentityType identityType = new OOAdvantech.MetaDataRepository.ObjectIdentityType(parts);

                            if (!_DataSourceRelationsColumnsWithSubDataNodes.ContainsKey(subDataNode.Identity))
                            {
                                _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity] = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>();
                                _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNode.FullName] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                            }
                            if (!_DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNode.FullName].ContainsKey(objectIdentityType))
                            {
                                System.Collections.Generic.List<string> relationColumns = new System.Collections.Generic.List<string>();
                                foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                    relationColumns.Add(identityPart.PartTypeName);

                                _DataSourceRelationsColumnsWithSubDataNodes[subDataNode.Identity][subDataNode.FullName][objectIdentityType] = new RelationColumns(relationColumns, "OSM_StorageIdentity");
                            }

                        }
                    }
                }
                return _DataSourceRelationsColumnsWithSubDataNodes;


            }
        }



  

        ///<summary>
        ///This property defines the relation columns, 
        ///which used in table relation with parent data source. 
        ///</summary>
        public override System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>> DataSourceRelationsColumnsWithParent
        {
            get
            {
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, RelationColumns>> _RelationsColumnsWithParent = new Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>();
                MetaDataRepository.MetaObject assignedMetaObject = DataNode.AssignedMetaObject;

                if (DataNode.Type == DataNode.DataNodeType.Group)
                {
                    if (DataNode.RealParentDataNode != null)
                    {
                        string relationPartIdentity = DataNode.FullName;
                        _RelationsColumnsWithParent[relationPartIdentity] = new Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                        foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in (DataNode.RealParentDataNode.DataSource.DataLoaders[DataLoaderMetadata.ObjectsContextIdentity] as MetaDataRepository.ObjectQueryLanguage.StorageDataLoader).ObjectIdentityTypes)
                        {
                            List<string> childRelationColumns = new List<string>();

                            if (!_RelationsColumnsWithParent[relationPartIdentity].ContainsKey(objectIdentityType))
                            {
                                foreach (MetaDataRepository.IdentityPart column in objectIdentityType.Parts)
                                    childRelationColumns.Add(DataNode.RealParentDataNode.Alias + "_" + column.PartTypeName);
                                _RelationsColumnsWithParent[relationPartIdentity][objectIdentityType] = new RelationColumns(childRelationColumns, "OSM_StorageIdentity");
                            }
                        }
                        return _RelationsColumnsWithParent;
                    }
                }

                if (DataNode.RealParentDataNode != null)
                {
                    _RelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()] = new System.Collections.Generic.Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>();
                    foreach (MetaDataRepository.StorageCell storageCell in DataLoaderMetadata.StorageCells)
                    {
                        MetaDataRepository.AssociationEnd associationEnd = DataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
                        List<string> childRelationColumns = new List<string>();
                        string storageIdentityColumn = null;
                        var objectIdentityType = storageCell.ObjectIdentityType;
                        if (HasRelationIdentityColumns)
                        {
                            if (DataLoaderMetadata.MemoryCell != null)
                            {

                                MetaDataRepository.ObjectIdentityType identityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
                                string columnName = "MC_ObjectID";
                                childRelationColumns.Add(columnName);
                                objectIdentityType = identityType;
                            }
                            else
                            {
                                foreach (MetaDataRepository.IIdentityPart identityPart in storageCell.ObjectIdentityType.Parts)
                                {
                                    if (storageCell is MetaDataLoadingSystem.StorageCell)
                                        childRelationColumns.Add(identityPart.Name);
                                    else if (storageCell is MetaDataLoadingSystem.StorageCellReference)
                                        childRelationColumns.Add(identityPart.PartTypeName);
                                }
                            }
                            storageIdentityColumn = "OSM_StorageIdentity";
                        }
                        else if (associationEnd != null)
                        {
                            if (DataLoaderMetadata.MemoryCell != null)
                            {
                                MetaDataRepository.ObjectIdentityType identityType = new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("MC_ObjectID", "MC_ObjectID", typeof(System.Guid)) });
                                if (associationEnd != null)
                                {
                                    identityType = associationEnd.GetReferenceObjectIdentityType(identityType);
                                    childRelationColumns.Add(identityType.Parts[0].Name);
                                }
                                objectIdentityType = identityType;
                            }
                            else
                            {
                                objectIdentityType = associationEnd.GetReferenceObjectIdentityType(objectIdentityType);
                                foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                                    childRelationColumns.Add(identityPart.Name);
                            }
                            storageIdentityColumn = associationEnd.ReferenceStorageIdentityColumnName;
                        }
                        else if (DataNode.AssignedMetaObject is MetaDataRepository.Attribute && (DataNode.AssignedMetaObject as MetaDataRepository.Attribute).IsPersistentValueType)
                        {
                            foreach (MetaDataRepository.IIdentityPart identityPart in objectIdentityType.Parts)
                            {
                                if (storageCell is MetaDataLoadingSystem.StorageCell)
                                    childRelationColumns.Add(identityPart.Name);
                                else
                                    childRelationColumns.Add(identityPart.PartTypeName);
                            }
                        }
                        _RelationsColumnsWithParent[DataNode.AssignedMetaObject.Identity.ToString()][objectIdentityType] = new RelationColumns(childRelationColumns, storageIdentityColumn);
                    }
                }
                return _RelationsColumnsWithParent;
            }

        }
        //System.Collections.Generic.List<string> _ObjectIDColumns;
        //public override System.Collections.Generic.List<string> ObjectIDColumns
        //{
        //    get 
        //    {
        //        if (_ObjectIDColumns == null)
        //        {
        //            _ObjectIDColumns = new System.Collections.Generic.List<string>();
        //            _ObjectIDColumns.Add("ObjectID");
        //        }
        //        return _ObjectIDColumns;

        //    }
        //}

        public override System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]> LocalResolvedCriterions
        {
            get
            {
                return new System.Collections.Generic.Dictionary<Criterion, System.Collections.Generic.Stack<DataNode>[]>();
            }
        }
        public override List<Criterion> GlobalResolveCriterions
        {
            get
            {
                List<Criterion> globalResolveCriterions = new List<Criterion>();
                foreach (var criterion in base.GlobalResolveCriterions)
                    globalResolveCriterions.Add(criterion);
                foreach (var criterion in base.LocalResolvedCriterions.Keys)
                    globalResolveCriterions.Add(criterion);
                foreach (var criterion in base.LocalOnMemoryResolvedCriterions.Keys)
                    globalResolveCriterions.Add(criterion);
                return globalResolveCriterions;
            }
        }

        //public override Dictionary<Criterion, Stack<DataNode>[]> OverridenOperatorCriterions
        //{
        //    get
        //    {
        //        return new Dictionary<Criterion, Stack<DataNode>[]>();
        //    }
        //}

        struct RoleName
        {
            public RoleName(MetaDataRepository.AssociationEnd associationEnd)
            {
                Name = associationEnd.Name;
                AssociationEnd = associationEnd;
            }
            public RoleName(MetaDataRepository.AssociationEnd associationEnd, string name)
            {
                Name = name;
                AssociationEnd = associationEnd;
            }
            public string Name;
            MetaDataRepository.AssociationEnd AssociationEnd;

        }
    }
}
