using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.PersistenceLayerRunTime;
using SubDataNodeIdentity = System.Guid;
using ComparisonTermsType = OOAdvantech.MetaDataRepository.ObjectQueryLanguage.Criterion.ComparisonTermsType;

namespace OOAdvantech.RDBMSPersistenceRunTime
{
    /// <MetaDataID>{1ae92ad4-0880-418b-be98-e61c41c227d2}</MetaDataID>
    /// <summary>
    /// Manage the objects which are type of data node classifier and enlisted in scoop transaction.
    //  Produce a data table with the states of objects under transaction
    /// </summary>
    public class ObjectUnderTransactionDataManager
    {
        /// <MetaDataID>{e6385379-859b-4806-9525-83d2ba478529}</MetaDataID>
        readonly DataLoader DataLoader;
        /// <summary>
        /// Initi
        /// </summary>
        /// <param name="dataLoader"></param>
        /// <MetaDataID>{6e5810d1-2252-4123-9ba9-32a4409d3819}</MetaDataID>
        public ObjectUnderTransactionDataManager(DataLoader dataLoader)
        {

            DataLoader = dataLoader;
        }
        /// <exclude>Excluded</exclude>
        string _ChangeTypeColumnName;
        /// <MetaDataID>{8b8601d1-da42-466c-8014-d303250eeded}</MetaDataID>
        public string ChangeTypeColumnName
        {
            get
            {
                return _ChangeTypeColumnName;
            }
        }
        /// <MetaDataID>{0bea9ab6-a45b-4438-aa1b-180925a0e50d}</MetaDataID>
        RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get
            {
                return (DataLoader.Storage as RDBMSPersistenceRunTime.Storage).StorageDataBase.TypeDictionary;
            }
        }
        /// <MetaDataID>{02aa68d8-bd2f-43fd-bf8e-82a459d2574d}</MetaDataID>
        void SetStorageInstanceColumnValue(PersistenceLayerRunTime.StorageInstanceRef storageInstance, string columnName, object value)
        {

            if (!(DataLoader as DataLoader).DataLoadingPrepared && !(DataLoader as DataLoader).OnDataLoadingPreparation)
                (DataLoader as DataLoader).DataNode.DataSource.PrepareForDataLoading();

            columnName = DataLoader.SqlScriptsBuilder.GeValidName(columnName);
            MetaDataRepository.ObjectQueryLanguage.IDataTable tableWithInstancesInTransaction = TableWithStatesOfObjectsUnderTransaction;
            if (tableWithInstancesInTransaction == null)
                DataLoader.UpdatedObjects.Add(storageInstance);
            InstancesDataRows[storageInstance][columnName] = value;
        }
        /// <summary></summary>
        /// <MetaDataID>{035c1c4f-716c-477f-8473-9709fd120bd3}</MetaDataID>
        public void PrepareTablesWithDataOfObjectsUnderTransaction()
        {
            if (DataLoader.UpdatedObjects.Count > 0 || DataLoader.NewObjects.Count > 0 || DataLoader.CandidateForDeleteObjects.Count > 0)
            {
                DataLoader.GetRelationChanges();
                _TableWithStatesOfObjectsUnderTransaction =MetaDataRepository.ObjectQueryLanguage.DataSource.DataObjectsInstantiator.CreateDataTable(true);
                foreach (DataLoader.DataColumn column in DataLoader.ClassifierDataColumns)
                    _TableWithStatesOfObjectsUnderTransaction.Columns.Add(DataLoader.SqlScriptsBuilder.GeValidName(column.Name), column.Type);

                //if(DataLoader.DataLoaderMetadata.MemoryCell!=null)
                //    foreach (var objectIdentyTypes in DataLoader.DataSourceRelationsColumnsWithParent.Values)
                //    {
                //        foreach (var objectIdentyType in objectIdentyTypes.Keys)
                //            foreach(var part in objectIdentyType.Parts)
                //                _TableWithStatesOfObjectsUnderTransaction.Columns.Add(new System.Data.DataColumn(DataLoader.SqlScriptsBuilder.GeValidName(part.Name), part.Type));
                //    }
                _ChangeTypeColumnName = "ChangeType_" + _TableWithStatesOfObjectsUnderTransaction.GetHashCode();
                _TableWithStatesOfObjectsUnderTransaction.Columns.Add(_ChangeTypeColumnName, typeof(int));
                _TableWithStatesOfObjectsUnderTransaction.TableName = DataLoader.Classifier.Name + "_" + _TableWithStatesOfObjectsUnderTransaction.GetHashCode();
                System.Collections.Generic.List<PersistenceLayerRunTime.StorageInstanceRef> candidateForDeleteObjects = DataLoader.CandidateForDeleteObjects;
                //RDBMSMetaDataRepository.AssociationEnd rdbmsAssociationEnd;
                //if (!DataLoader.DataNode.ThrougthRelationTable && DataLoader.DataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                //{

                //    rdbmsAssociationEnd = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataLoader.DataNode.AssignedMetaObject) as RDBMSMetaDataRepository.AssociationEnd;
                //    if (rdbmsAssociationEnd.GetReferenceColumns(rdbmsAssociationEnd.Specification) == null)
                //        rdbmsAssociationEnd = null;
                //}

                #region Load updated objects in table
                foreach (StorageInstanceRef storageInstance in DataLoader.UpdatedObjects)
                {
                    if (candidateForDeleteObjects.Contains(storageInstance))
                        continue;
                    if (DataLoader.DataLoaderMetadata.StorageCells.Contains(storageInstance.StorageInstanceSet))
                        LoadObjectStateInTable(storageInstance);
                }
                #endregion

                #region Load new objects in table
                foreach (PersistenceLayerRunTime.StorageInstanceRef storageInstance in DataLoader.NewObjects)
                {
                    if (candidateForDeleteObjects.Contains(storageInstance))
                        continue;
                    LoadObjectStateInTable(storageInstance as StorageInstanceRef);
                    DataLoader.ObjectUnderTransaction[storageInstance.ObjectID] = storageInstance;
                }
                #endregion

                #region Load deleted objects in table
                foreach (StorageInstanceRef storageInstance in DataLoader.CandidateForDeleteObjects)
                {
                    if (!DataLoader.DataLoaderMetadata.StorageCells.Contains(storageInstance.StorageInstanceSet))
                        continue;
                    MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = _TableWithStatesOfObjectsUnderTransaction.NewRow();
                    RDBMSMetaDataRepository.Class rdbmsMetadataClass = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstance.Class) as RDBMSMetaDataRepository.Class;
                    dataRow[_ChangeTypeColumnName] = 2;
                    OOAdvantech.RDBMSPersistenceRunTime.ObjectID objectID = storageInstance.ObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID;
                    foreach (RDBMSMetaDataRepository.IdentityColumn column in (storageInstance.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable.ObjectIDColumns)
                        dataRow[column.DataBaseColumnName] = TypeDictionary.Convert(objectID.GetMemberValue(column.ColumnType), dataRow.Table.Columns[column.DataBaseColumnName].DataType);
                    _TableWithStatesOfObjectsUnderTransaction.Rows.Add(dataRow);
                }
                #endregion

                #region Load relation data

                foreach (KeyValuePair<SubDataNodeIdentity, Dictionary<string, System.Collections.Generic.Dictionary<MetaDataRepository.ObjectIdentityType, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.RelationColumns>>> entry in DataLoader.DataSourceRelationsColumnsWithSubDataNodes)
                {

                    MetaDataRepository.ObjectQueryLanguage.DataNode dataNode = DataLoader.DataNode.GetDataNode(entry.Key);
                    if (!(dataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd))
                        continue;

                    MetaDataRepository.AssociationEnd associationEnd = dataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;

                    OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader dataLoader = null;
                    dataNode.DataSource.DataLoaders.TryGetValue(DataLoader.DataLoaderMetadata.ObjectsContextIdentity, out dataLoader);
                    DataLoader subNodeDataLoader = dataLoader as DataLoader;
                    if (subNodeDataLoader != null && !dataNode.ThroughRelationTable)
                    {
                        if (subNodeDataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction != null)
                        {
                            int rowsCount = subNodeDataLoader.ObjectsUnderTransactionData.TableWithStatesOfObjectsUnderTransaction.Rows.Count;
                        }
                        if (subNodeDataLoader.HasRelationReferenceColumns)
                        {

                            List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>(subNodeDataLoader.DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()].Keys);
                            foreach (PersistenceLayerRunTime.ObjectsLink relationChange in DataLoader.GetRelationChanges(dataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd, dataNode))
                            {
                                if (associationEnd.IsRoleA)
                                {
                                    MetaDataRepository.ObjectIdentityType referenceObjectIdentityType = referenceObjectIdentityTypes[referenceObjectIdentityTypes.IndexOf((relationChange.RoleA.RealStorageInstanceRef.ObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).ObjectIdentityType)];
                                    MetaDataRepository.ObjectIdentityType objectIdentityType = DataLoader.ObjectIdentityTypes[referenceObjectIdentityTypes.IndexOf((relationChange.RoleA.RealStorageInstanceRef.ObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).ObjectIdentityType)];
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    {
                                        foreach (MetaDataRepository.IIdentityPart referencePart in referenceObjectIdentityType.Parts)
                                        {
                                            if (part.PartTypeName == referencePart.PartTypeName)
                                            {
                                                if (relationChange.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                                    subNodeDataLoader.ObjectsUnderTransactionData.SetStorageInstanceColumnValue(relationChange.RoleA.RealStorageInstanceRef, referencePart.Name, (relationChange.RoleB.ObjectID as ObjectID).GetMemberValue(part.Name));
                                                else
                                                    subNodeDataLoader.ObjectsUnderTransactionData.SetStorageInstanceColumnValue(relationChange.RoleA.RealStorageInstanceRef, referencePart.Name, DBNull.Value);
                                            }
                                        }
                                    }

                                }
                                else
                                {

                                    MetaDataRepository.ObjectIdentityType referenceObjectIdentityType = referenceObjectIdentityTypes[referenceObjectIdentityTypes.IndexOf((relationChange.RoleB.RealStorageInstanceRef.PersistentObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).ObjectIdentityType)];
                                    MetaDataRepository.ObjectIdentityType objectIdentityType = DataLoader.ObjectIdentityTypes[referenceObjectIdentityTypes.IndexOf((relationChange.RoleB.RealStorageInstanceRef.PersistentObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).ObjectIdentityType)];
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    {
                                        foreach (MetaDataRepository.IIdentityPart referencePart in referenceObjectIdentityType.Parts)
                                        {
                                            if (part.PartTypeName == referencePart.PartTypeName)
                                            {
                                                if (relationChange.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                                    subNodeDataLoader.ObjectsUnderTransactionData.SetStorageInstanceColumnValue(relationChange.RoleB.RealStorageInstanceRef, referencePart.Name, (relationChange.RoleA.ObjectID as ObjectID).GetMemberValue(part.Name));
                                                else
                                                    subNodeDataLoader.ObjectsUnderTransactionData.SetStorageInstanceColumnValue(relationChange.RoleB.RealStorageInstanceRef, referencePart.Name, DBNull.Value);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            List<MetaDataRepository.ObjectIdentityType> referenceObjectIdentityTypes = new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>(entry.Value[associationEnd.Identity.ToString()].Keys);
                            foreach (PersistenceLayerRunTime.ObjectsLink relationChange in DataLoader.GetRelationChanges(dataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd, dataNode))
                            {
                                if (associationEnd.IsRoleA)
                                {
                                    MetaDataRepository.ObjectIdentityType referenceObjectIdentityType = referenceObjectIdentityTypes[referenceObjectIdentityTypes.IndexOf((relationChange.RoleA.RealStorageInstanceRef.ObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).ObjectIdentityType)];
                                    MetaDataRepository.ObjectIdentityType objectIdentityType = subNodeDataLoader.ObjectIdentityTypes[subNodeDataLoader.ObjectIdentityTypes.IndexOf((relationChange.RoleA.RealStorageInstanceRef.ObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).ObjectIdentityType)];
                                    //ObjectID referenceObjectID = relationChange.RoleA.RealStorageInstanceRef.ObjectID as ObjectID;
                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    {
                                        foreach (MetaDataRepository.IIdentityPart referencePart in referenceObjectIdentityType.Parts)
                                        {
                                            if (part.PartTypeName == referencePart.PartTypeName)
                                            {
                                                if (relationChange.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                                {
                                                    if (DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
                                                        SetStorageInstanceColumnValue(relationChange.RelationObject.RealStorageInstanceRef, referencePart.Name, (relationChange.RoleA.ObjectID as ObjectID).GetMemberValue(part.PartTypeName));
                                                    else
                                                        SetStorageInstanceColumnValue(relationChange.RoleB.RealStorageInstanceRef, referencePart.Name, (relationChange.RoleA.ObjectID as ObjectID).GetMemberValue(part.PartTypeName));

                                                }
                                                else
                                                {
                                                    if (DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
                                                        SetStorageInstanceColumnValue(relationChange.RelationObject.RealStorageInstanceRef, referencePart.Name, DBNull.Value);
                                                    else
                                                        SetStorageInstanceColumnValue(relationChange.RoleB.RealStorageInstanceRef, referencePart.Name, DBNull.Value);

                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {

                                    MetaDataRepository.ObjectIdentityType referenceObjectIdentityType = referenceObjectIdentityTypes[referenceObjectIdentityTypes.IndexOf((relationChange.RoleB.RealStorageInstanceRef.ObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).ObjectIdentityType)];
                                    MetaDataRepository.ObjectIdentityType objectIdentityType = DataLoader.ObjectIdentityTypes[referenceObjectIdentityTypes.IndexOf((relationChange.RoleB.RealStorageInstanceRef.ObjectID as OOAdvantech.RDBMSPersistenceRunTime.ObjectID).ObjectIdentityType)];

                                    foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                                    {
                                        foreach (MetaDataRepository.IIdentityPart referencePart in referenceObjectIdentityType.Parts)
                                        {
                                            if (part.PartTypeName == referencePart.PartTypeName)
                                            {
                                                if (relationChange.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                                                {
                                                    if (DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
                                                        SetStorageInstanceColumnValue(relationChange.RelationObject.RealStorageInstanceRef, referencePart.Name, (relationChange.RoleB.ObjectID as ObjectID).GetMemberValue(part.Name));
                                                    else
                                                        SetStorageInstanceColumnValue(relationChange.RoleA.RealStorageInstanceRef, referencePart.Name, (relationChange.RoleB.ObjectID as ObjectID).GetMemberValue(part.Name));
                                                }
                                                else
                                                {
                                                    if (DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation == associationEnd.Association)
                                                        SetStorageInstanceColumnValue(relationChange.RelationObject.RealStorageInstanceRef, referencePart.Name, DBNull.Value);
                                                    else
                                                        SetStorageInstanceColumnValue(relationChange.RoleA.RealStorageInstanceRef, referencePart.Name, DBNull.Value);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }

                }
                #endregion

                if (_TableWithStatesOfObjectsUnderTransaction.Rows.Count == 0)
                    _TableWithStatesOfObjectsUnderTransaction = null;
            }
            foreach (OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataLoader.DataNode.RealSubDataNodes)
            {
                if (subDataNode.ThroughRelationTable && subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    LoadTableWithRelationChangesInTransaction(subDataNode);
            }


        }
        /// <exclude>Excluded</exclude>
       MetaDataRepository.ObjectQueryLanguage.IDataTable _TableWithStatesOfObjectsUnderTransaction;
        /// <MetaDataID>{a3e7d5a3-a4a7-4a39-9e30-29e02c2ea92b}</MetaDataID>
        /// <summary>
        /// This property defines a data table with the states of objects under transaction.
        /// </summary>
        public MetaDataRepository.ObjectQueryLanguage.IDataTable TableWithStatesOfObjectsUnderTransaction
        {
            get
            {

                return _TableWithStatesOfObjectsUnderTransaction;

            }
        }

        bool? LoadRelationObjectReferencColumn;

        /// <MetaDataID>{6a420121-1789-4f63-87ba-579b08910c62}</MetaDataID>
        Dictionary<PersistenceLayerRunTime.StorageInstanceRef, MetaDataRepository.ObjectQueryLanguage.IDataRow> InstancesDataRows = new Dictionary<PersistenceLayerRunTime.StorageInstanceRef, MetaDataRepository.ObjectQueryLanguage.IDataRow>();
        /// <summary>
        /// Create a new row in TableWithStatesOfObjectsUnderTransaction and save the state of object in row. 
        /// </summary>
        /// <param name="storageInstance">
        /// This parameter defines the storage insance ref of object where th method will save its state
        /// </param>
        /// <MetaDataID>{d1be5411-4c27-42b3-9fbc-e095c865c93c}</MetaDataID>
        private void LoadObjectStateInTable(StorageInstanceRef storageInstanceRef)
        {
            
            MetaDataRepository.ObjectQueryLanguage.IDataRow dataRow = TableWithStatesOfObjectsUnderTransaction.NewRow();
            InstancesDataRows[storageInstanceRef] = dataRow;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(storageInstanceRef.Class) as RDBMSMetaDataRepository.Class;
            foreach (PersistenceLayerRunTime.StorageInstanceRef.ValueOfAttribute attributeValue in storageInstanceRef.GetPersistentAttributeValues())
            {
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attributeValue.Attribute) as RDBMSMetaDataRepository.Attribute;
                RDBMSMetaDataRepository.Column column = null;
                foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(rdbmsMetadataClass.ActiveStorageCell))
                {
                    if (attributeColumn.CreatorIdentity == attributeValue.PathIdentity)
                    {
                        column = attributeColumn;
                        break;
                    }
                }
                foreach (DataLoader.DataColumn dataColumn in DataLoader.ClassifierDataColumns)
                {
                    if (dataColumn.MappedObject != null && 
                        column.MappedAttribute != null && 
                        column.MappedAttribute.Identity == dataColumn.MappedObject.Identity&&
                        column.CreatorIdentity==dataColumn.CreatorIdentity)
                    {
                        if (dataRow.Table.Columns.Contains(DataLoader.SqlScriptsBuilder.GeValidName(dataColumn.Name)))
                        {
                            //if(attributeValue.Value!=null)
                            object value = TypeDictionary.Convert(attributeValue.Value, dataRow.Table.Columns[column.DataBaseColumnName].DataType);
                            if(value !=null)
                                dataRow[column.DataBaseColumnName] = value;
                        }
                        break;
                    }
                }
            }

            foreach (DataLoader.DataColumn dataColumn in DataLoader.ClassifierDataColumns)
            {
                foreach (RDBMSMetaDataRepository.Column column in (storageInstanceRef as StorageInstanceRef).RelationshipColumnsValues.Keys)
                {
                    if (dataColumn.MappedObject != null &&
                       column.MappedAssociationEnd != null &&
                       column.MappedAssociationEnd.Identity == dataColumn.MappedObject.Identity &&
                       column.CreatorIdentity == dataColumn.CreatorIdentity)
                    {
                        if (dataRow.Table.Columns.Contains(DataLoader.SqlScriptsBuilder.GeValidName(dataColumn.Name)))
                        {

                            object value = TypeDictionary.Convert(storageInstanceRef.RelationshipColumnsValues[column], dataRow.Table.Columns[column.DataBaseColumnName].DataType);
                            if (value != null)
                                dataRow[column.DataBaseColumnName] = value;
                        }
                        break;
                    }
                    
                }
            }

            //foreach (RDBMSMetaDataRepository.Column column in (storageInstance as StorageInstanceRef).RelationshipColumnsValues.Keys)
            //    StorageInstanceRefsRows[storageInstance][column.Namespace as RDBMSMetaDataRepository.Table][column.DataBaseColumnName] = (storageInstance as StorageInstanceRef).RelationshipColumnsValues[column];


           
            if (dataRow.Table.Columns.Contains("TypeID"))
                dataRow["TypeID"] = rdbmsMetadataClass.TypeID;
            if (dataRow.Table.Columns.Contains("StorageCellID"))
            {
                if (storageInstanceRef.PersistentObjectID == null)
                    dataRow["StorageCellID"] = -1;
                else
                    dataRow["StorageCellID"] = storageInstanceRef.StorageInstanceSet.SerialNumber;

            }
            dataRow[_ChangeTypeColumnName] = 1;
            foreach (MetaDataRepository.IIdentityPart column in DataLoader.ObjectIdentityTypes[DataLoader.ObjectIdentityTypes.IndexOf((storageInstanceRef.ObjectID as ObjectID).ObjectIdentityType)].Parts)
            {
                string columnName = DataLoader.SqlScriptsBuilder.GeValidName(column.Name);
                dataRow[columnName] = TypeDictionary.Convert((storageInstanceRef.ObjectID as ObjectID).GetMemberValue(column.PartTypeName), dataRow.Table.Columns[column.Name].DataType);
            }
            if (!DataLoader.DataNode.ThroughRelationTable &&
                DataLoader.DataNode.AssignedMetaObject is DotNetMetaDataRepository.AssociationEnd &&
                DataLoader.HasRelationReferenceColumns &&
                !((DataLoader.DataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).Association == DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation))
            {
                object linkedObject = storageInstanceRef.GetLinkedObject((DataLoader.DataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).GetOtherEnd());
                if (linkedObject != null)
                {
                    PersistenceLayer.StorageInstanceRef linkedObjectStorageInstanceRef = PersistenceLayerRunTime.StorageInstanceRef.GetStorageInstanceRef(linkedObject);
                    MetaDataRepository.ObjectIdentityType objectIdentitiType = null;
                    ObjectID referenceObjectID = null;

                    List<MetaDataRepository.ObjectIdentityType> objectIdentityTypes = new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithParent[DataLoader.DataNode.AssignedMetaObject.Identity.ToString()].Keys);
                    objectIdentitiType = objectIdentityTypes[objectIdentityTypes.IndexOf((linkedObjectStorageInstanceRef.ObjectID as ObjectID).ObjectIdentityType)];
                    referenceObjectID = linkedObjectStorageInstanceRef.ObjectID as ObjectID;
                    foreach (MetaDataRepository.IdentityPart part in objectIdentitiType.Parts)
                    {
                        string columnName = DataLoader.SqlScriptsBuilder.GeValidName(part.Name);
                        dataRow[columnName] = TypeDictionary.Convert(referenceObjectID.GetMemberValue(part.PartTypeName), dataRow.Table.Columns[part.Name].DataType);
                    }
                }
            }
            if (!LoadRelationObjectReferencColumn.HasValue)
            {
                LoadRelationObjectReferencColumn = false;
                if ((DataLoader.DataNode.AssignedMetaObject is DotNetMetaDataRepository.AssociationEnd) && (DataLoader.DataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).Association == DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation &&
                    !DataLoader.DataNode.ThroughRelationTable)
                    LoadRelationObjectReferencColumn = true;
                foreach (var subDataNode in DataLoader.DataNode.SubDataNodes)
                {
                    if ((subDataNode.AssignedMetaObject is DotNetMetaDataRepository.AssociationEnd) && (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.AssociationEnd).Association == DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation &&
                        !subDataNode.ThroughRelationTable)
                    {
                        LoadRelationObjectReferencColumn = true;
                        break;
                    }
                }

            }


            if (LoadRelationObjectReferencColumn.Value)
            {
                object roleAObject = Member<object>.GetValue(storageInstanceRef.Class.LinkClassRoleAFastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                object roleBObject = Member<object>.GetValue(storageInstanceRef.Class.LinkClassRoleBFastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                ObjectID roleAObjectID = null;
                ObjectID roleBObjectID = null;
                if (roleAObject != null)
                    roleAObjectID = StorageInstanceRef.GetStorageInstanceAgent(roleAObject).ObjectID as ObjectID;

                if (roleBObject != null)
                    roleBObjectID = StorageInstanceRef.GetStorageInstanceAgent(roleBObject).ObjectID as ObjectID;
                if (roleAObjectID != null || roleBObjectID != null)
                {

                    RDBMSMetaDataRepository.Association rdbmsAssociation = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation) as RDBMSMetaDataRepository.Association;
                    if (roleAObjectID != null)
                    {
                        
                        foreach (MetaDataRepository.ObjectIdentityType roleBObjectIdentityType in (rdbmsAssociation.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>() { roleAObjectID.ObjectIdentityType }))
                        {
                            foreach (MetaDataRepository.IIdentityPart part in roleBObjectIdentityType.Parts)
                            {
                                string columnName = DataLoader.SqlScriptsBuilder.GeValidName(part.Name);
                                if (dataRow.Table.Columns.Contains(columnName))
                                    dataRow[part.Name] = roleAObjectID.GetMemberValue(part.PartTypeName);
                            }
                        }
                    }
                    if (roleBObjectID != null)
                    {
                        foreach (MetaDataRepository.ObjectIdentityType roleAObjectIdentityType in (rdbmsAssociation.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>() { roleBObjectID.ObjectIdentityType }))
                        {
                            foreach (MetaDataRepository.IIdentityPart part in roleAObjectIdentityType.Parts)
                            {
                                
                                string columnName = DataLoader.SqlScriptsBuilder.GeValidName(part.Name);
                                if(dataRow.Table.Columns.Contains(columnName))
                                    dataRow[columnName] = roleBObjectID.GetMemberValue(part.PartTypeName);
                            }
                        }
                    }
                }


            }
            if (DataLoader.DataLoaderMetadata.MemoryCell != null)
                foreach (var objectIdentyTypes in DataLoader.DataSourceRelationsColumnsWithParent.Values)
                {
                    foreach (var objectIdentyType in objectIdentyTypes.Keys)
                    {

                        var objectData = DataLoader.DataLoaderMetadata.MemoryCell.Objects[storageInstanceRef.MemoryInstance];
                        if (DataLoader.HasRelationIdentityColumns)
                        {
                            int i = 0;
                            foreach (var part in objectData.ObjectID.ObjectIdentityType.Parts)
                            {
                                dataRow[objectIdentyType.Parts[i].Name] = objectData.ObjectID.GetPartValue(i);
                                i++;
                            }
                        }
                        else
                        {
                            int i = 0;
                            foreach (var part in objectData.ParentObjectID.ObjectIdentityType.Parts)
                            {
                                dataRow[objectIdentyType.Parts[i].Name] = objectData.ParentObjectID.GetPartValue(i);
                                i++;
                            }

                        }
                    }
                }
            _TableWithStatesOfObjectsUnderTransaction.Rows.Add(dataRow);
        }

        /// <MetaDataID>{4a4b51ae-4580-4ed0-889d-2f472f27ca9d}</MetaDataID>
        System.Collections.Generic.Dictionary<MetaDataRepository.AssociationEnd, MetaDataRepository.ObjectQueryLanguage.IDataTable> AssocitionsTablesWithChanges = new Dictionary<MetaDataRepository.AssociationEnd, MetaDataRepository.ObjectQueryLanguage.IDataTable>();

        ///<summary>Creates a Association table and load the relation changes data</summary>
        ///<returns>Return a table with relation change data </returns>
        /// <MetaDataID>{9acfdb6a-4a4a-4ffc-9e7b-34c4a1b0d8c1}</MetaDataID>
        public MetaDataRepository.ObjectQueryLanguage.IDataTable GetTableWithRelationChangesInTransaction(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode)
        {
            MetaDataRepository.AssociationEnd associationEnd = relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
           MetaDataRepository.ObjectQueryLanguage.IDataTable associationTable = null;
            AssocitionsTablesWithChanges.TryGetValue(associationEnd, out associationTable);
            return associationTable;
        }
        /// <MetaDataID>{8857973d-8cf9-4eba-9824-faf7343cbb7a}</MetaDataID>
        void LoadTableWithRelationChangesInTransaction(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode relatedDataNode)
        {
            MetaDataRepository.AssociationEnd associationEnd = relatedDataNode.AssignedMetaObject as MetaDataRepository.AssociationEnd;
           OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataTable associationTable = null;
            if (AssocitionsTablesWithChanges.TryGetValue(associationEnd, out associationTable))
                return;

            List<PersistenceLayerRunTime.ObjectsLink> relationChanges = null;

            //NonValueType defines the first ascending data node with non value type objects. 
            //This data node has the data loader with the objects in active mode. 
            //Some objects participate in current transaction (value type objects can’t participate in transaction). 
            //Data loader gives the relation changes for the parameter associationEnd.
            MetaDataRepository.ObjectQueryLanguage.DataNode nonValueTypedataNode = DataLoader.DataNode;
            while (nonValueTypedataNode != null && nonValueTypedataNode.Type == MetaDataRepository.ObjectQueryLanguage.DataNode.DataNodeType.Object && nonValueTypedataNode.ValueTypePath.Count > 0)
                nonValueTypedataNode = nonValueTypedataNode.ParentDataNode;


            DataLoader subDataNodeDataLoader = null;
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataLoader.DataNode.RealSubDataNodes)
            {
                if (subDataNode.AssignedMetaObject == associationEnd)
                {
                    relationChanges = (nonValueTypedataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader).GetRelationChanges(associationEnd, subDataNode);
                    if (subDataNode.DataSource.DataLoaders.ContainsKey(DataLoader.DataLoaderMetadata.ObjectsContextIdentity))
                    {
                        subDataNodeDataLoader = subDataNode.DataSource.DataLoaders[DataLoader.DataLoaderMetadata.ObjectsContextIdentity] as DataLoader;
                        if (nonValueTypedataNode == DataLoader.DataNode)
                            relationChanges.AddRange(subDataNodeDataLoader.GetRelationChanges(associationEnd.GetOtherEnd(), DataLoader.DataNode));
                    }
                    break;
                }
            }

            if (relationChanges.Count == 0)
                return;
            associationTable =MetaDataRepository.ObjectQueryLanguage.DataSource.DataObjectsInstantiator.CreateDataTable(false);
            associationTable.TableName = associationEnd.Association.RoleA.Specification.Name + "_" + associationEnd.Association.RoleB.Specification.Name + "_" + associationTable.GetHashCode();

            RDBMSMetaDataRepository.AssociationEnd rdbmsAssociationEnd = (DataLoader.Storage as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(associationEnd) as RDBMSMetaDataRepository.AssociationEnd;
            List<MetaDataRepository.ObjectIdentityType> roleAObjectIdentityTypes = null;
            List<MetaDataRepository.ObjectIdentityType> roleBObjectIdentityTypes = null;
            if (rdbmsAssociationEnd.IsRoleA)
            {
                roleBObjectIdentityTypes = rdbmsAssociationEnd.GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithSubDataNodes[subDataNodeDataLoader.DataNode.Identity][associationEnd.Identity.ToString()].Keys));
                roleAObjectIdentityTypes = (rdbmsAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(new List<MetaDataRepository.ObjectIdentityType>(subDataNodeDataLoader.DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()].Keys)));
            }
            else
            {
                roleAObjectIdentityTypes = rdbmsAssociationEnd.GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(DataLoader.DataSourceRelationsColumnsWithSubDataNodes[subDataNodeDataLoader.DataNode.Identity][associationEnd.Identity.ToString()].Keys));
                roleBObjectIdentityTypes = (rdbmsAssociationEnd.GetOtherEnd() as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(new List<MetaDataRepository.ObjectIdentityType>(subDataNodeDataLoader.DataSourceRelationsColumnsWithParent[associationEnd.Identity.ToString()].Keys));
            }

            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in roleAObjectIdentityTypes)
            {
                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                    associationTable.Columns.Add(DataLoader.SqlScriptsBuilder.GeValidName(part.Name), part.Type);

            }
            foreach (MetaDataRepository.ObjectIdentityType objectIdentityType in roleBObjectIdentityTypes)
            {
                foreach (MetaDataRepository.IIdentityPart part in objectIdentityType.Parts)
                    associationTable.Columns.Add(DataLoader.SqlScriptsBuilder.GeValidName(part.Name), part.Type);
            }

            associationTable.Columns.Add("ChangeType", typeof(int));
            associationTable.Columns.Add("RoleAStorageIdentity", typeof(int));
            associationTable.Columns.Add("RoleBStorageIdentity", typeof(int));


            foreach (PersistenceLayerRunTime.ObjectsLink relationChange in relationChanges)
            {
                PersistenceLayer.ObjectID roleAObjectID = null;
                PersistenceLayer.ObjectID roleBObjectID = null;
                int roleAStorageIdentity = 0;
                int roleBStorageIdentity = 0;

                if (associationEnd.Association.LinkClass != null && associationEnd.Association == relatedDataNode.Classifier.ClassHierarchyLinkAssociation)
                {
                    if (associationEnd.IsRoleA)
                    {
                        roleAObjectID = relationChange.RelationObject.ObjectID;
                        roleBObjectID = relationChange.RoleB.ObjectID;
                        roleAStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RelationObject.StorageIdentity);
                        roleBStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RoleB.StorageIdentity);
                    }
                    else
                    {
                        roleAObjectID = relationChange.RoleA.ObjectID;
                        roleBObjectID = relationChange.RelationObject.ObjectID;
                        roleAStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RoleA.StorageIdentity);
                        roleBStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RelationObject.StorageIdentity);

                    }
                }
                else if (associationEnd.Association.LinkClass != null && associationEnd.Association == DataLoader.DataNode.Classifier.ClassHierarchyLinkAssociation)
                {
                    if (associationEnd.IsRoleA)
                    {
                        roleBObjectID = relationChange.RelationObject.ObjectID;
                        roleAObjectID = relationChange.RoleA.ObjectID as ObjectID;
                        roleBStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RelationObject.StorageIdentity);
                        roleAStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RoleA.StorageIdentity);

                    }
                    else
                    {
                        roleBObjectID = relationChange.RoleB.ObjectID;
                        roleAObjectID = relationChange.RelationObject.ObjectID;
                        roleBStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RoleB.StorageIdentity);
                        roleAStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RelationObject.StorageIdentity);
                    }
                }
                else
                {
                    roleAObjectID = relationChange.RoleA.ObjectID;
                    roleBObjectID = relationChange.RoleB.ObjectID;
                    roleAStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RoleA.StorageIdentity);
                    roleBStorageIdentity = DataLoader.QueryStorageIdentities.IndexOf(relationChange.RoleB.StorageIdentity);
                }
               MetaDataRepository.ObjectQueryLanguage.IDataRow row = associationTable.NewRow();
                row["RoleAStorageIdentity"] = roleAStorageIdentity;
                row["RoleBStorageIdentity"] = roleBStorageIdentity;
                if (associationEnd.IsRoleA)
                {
                    List<MetaDataRepository.ObjectIdentityType> objectIdentityTypes = new List<MetaDataRepository.ObjectIdentityType>() { DataLoader.ObjectIdentityTypes[DataLoader.ObjectIdentityTypes.IndexOf(roleBObjectID.ObjectIdentityType)] };
                    foreach (MetaDataRepository.IIdentityPart column in (rdbmsAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(objectIdentityTypes)[0].Parts)
                        row[column.Name] = roleBObjectID.GetMemberValue(column.PartTypeName);
                }
                else
                {
                    List<MetaDataRepository.ObjectIdentityType> subDataNodeObjectIdentityTypes = new List<MetaDataRepository.ObjectIdentityType>() { subDataNodeDataLoader.ObjectIdentityTypes[subDataNodeDataLoader.ObjectIdentityTypes.IndexOf(roleBObjectID.ObjectIdentityType)] };
                    foreach (MetaDataRepository.IIdentityPart column in (rdbmsAssociationEnd.Association.RoleA as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(subDataNodeObjectIdentityTypes)[0].Parts)
                        row[column.Name] = roleBObjectID.GetMemberValue(column.PartTypeName);
                }
                if (associationEnd.IsRoleA)
                {
                    List<MetaDataRepository.ObjectIdentityType> subDataNodeObjectIdentityTypes = new List<MetaDataRepository.ObjectIdentityType>() { subDataNodeDataLoader.ObjectIdentityTypes[subDataNodeDataLoader.ObjectIdentityTypes.IndexOf(roleAObjectID.ObjectIdentityType)] };

                    foreach (MetaDataRepository.IIdentityPart column in (rdbmsAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(subDataNodeObjectIdentityTypes)[0].Parts)
                        row[column.Name] = roleAObjectID.GetMemberValue(column.PartTypeName);
                }
                else
                {
                    List<MetaDataRepository.ObjectIdentityType> objectIdentityTypes = new List<MetaDataRepository.ObjectIdentityType>() { DataLoader.ObjectIdentityTypes[DataLoader.ObjectIdentityTypes.IndexOf(roleAObjectID.ObjectIdentityType)] };
                    foreach (MetaDataRepository.IIdentityPart column in (rdbmsAssociationEnd.Association.RoleB as RDBMSMetaDataRepository.AssociationEnd).GetReferenceObjectIdentityTypes(objectIdentityTypes)[0].Parts)
                        row[column.Name] = roleAObjectID.GetMemberValue(column.PartTypeName);
                }

                associationTable.Rows.Add(row);

                if (relationChange.Change == OOAdvantech.PersistenceLayerRunTime.ObjectsLink.TypeOfChange.Added)
                    row["ChangeType"] = 0;
                else
                    row["ChangeType"] = 1;

            }
            AssocitionsTablesWithChanges[associationEnd] = associationTable;

        }

    }
}
