using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.AzureTableMetaDataPersistenceRunTime
{
    /// <MetaDataID>{bd09d837-a46d-46a4-8c83-3813e23907d4}</MetaDataID>
    public class DataLoader : OOAdvantech.PersistenceLayerRunTime.StorageDataLoader
    {
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCellsLink> GetStorageCellsLinks(DataNode relatedDataNode)
        {
            throw new NotImplementedException();
        }

        public override OOAdvantech.MetaDataRepository.ObjectIdentityType ObjectIdentityTypeForNewObject
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RetrievesData
        {
            get { return true; }
        }
        #region RowRemove code
        //public override List<string> RowRemoveColumns
        //{
        //    get { return new List<string>(); }
        //}
        #endregion
        protected override object GetObjectFromIdentity(OOAdvantech.PersistenceLayer.ObjectID objectIdentity)
        {
            throw new NotImplementedException();
        }

        public override bool CriterionCanBeResolvedFromNativeSystem(Criterion criterion)
        {
            return false;
        }

        public override bool DataLoadedInParentDataSource
        {
            get
            {
                return false;
            }
            set
            {

            }
        }
        protected override object Convert(object value, System.Type type)
        {
            if (value == null || value is System.DBNull)
                return value;
            else
                return System.Convert.ChangeType(value, type);

        }

        public override string GetLocalDataColumnName(DataNode dataNode)
        {
            return dataNode.Name;
        }
        protected override Type GetColumnType(string columnName)
        {
            return Data.Columns[columnName].DataType;
        }
        //protected override PersistenceLayer.ObjectID GetTemporaryObjectID()
        //{
        //    throw new NotImplementedException();
        //}
        //protected override void BuildDataTable(DataNode dataNode, System.Data.DataTable table, Dictionary<string, string> aliasColumns, Dictionary<string, int> columnsIndices, int[] sourceColumnsIndices)
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
        public override bool HasRelationIdentityColumnsFor(DataNode subDataNode)
        {
            if (subDataNode.ThroughRelationTable)
                return true;

            if (subDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
            {
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
        //public override List<string> ObjectIDColumns
        //{
        //    get { throw new NotImplementedException(); }
        //}

        public override Dictionary<Guid, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, ObjectKeyRelationColumns>> GroupByKeyRelationColumns
        {
            get { throw new NotImplementedException(); }
        }

        //protected override bool CanAggregateFanctionsResolvedLocally(DataNode aggregateFunctionDataNode)
        //{
        //    return false;
        //}

        public override Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>> RecursiveParentColumns
        {
            get { throw new NotImplementedException(); }
        }


        public override Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, List<string>>> RecursiveParentReferenceColumns
        {
            get { throw new NotImplementedException(); }
        }

        public override List<OOAdvantech.MetaDataRepository.ObjectIdentityType> ObjectIdentityTypes
        {
            get { return new List<OOAdvantech.MetaDataRepository.ObjectIdentityType>(); }
        }
        public override void UpdateObjectIdentityTypes(List<OOAdvantech.MetaDataRepository.ObjectIdentityType> dataLoaderObjectIdentityTypes)
        {

        }

        public override bool HasRelationIdentityColumns
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        protected override List<string> DataColumnNames
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
        public override object GetObject(PersistenceLayer.StorageInstanceRef.ObjectSate row, out bool loadObjectLinks)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override object GetObjectIdentity(OOAdvantech.PersistenceLayer.StorageInstanceRef.ObjectSate row)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>> DataSourceRelationsColumnsWithParent
        {
            get { throw new NotImplementedException(); }
        }
        public override Dictionary<Guid, Dictionary<string, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, RelationColumns>>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get { throw new NotImplementedException(); }
        }


        //OOAdvantech.MetaDataRepository.Storage _LoadFromStorage; 
        //public override OOAdvantech.MetaDataRepository.Storage LoadFromStorage
        //{
        //    get 
        //    {
        //        if (_LoadFromStorage == null)
        //        {
        //            foreach (StorageCell storageCell in DataLoaderMetadata.StorageCells)
        //            {
        //                _LoadFromStorage = storageCell.Namespace as OOAdvantech.MetaDataRepository.Storage;
        //                break;
        //            }
        //        }

        //        return _LoadFromStorage;
        //    }
        //}
        public override OOAdvantech.MetaDataRepository.Classifier Classifier
        {
            get
            {
                return DataNode.Classifier;
            }
        }
        public override void RetrieveFromStorage()
        {
            if (_Data == null)
                _Data = DataSource.DataObjectsInstantiator.CreateDataTable(false);

            Data.TableName = DataNode.Name;
            if (DataNode.Classifier == null && DataNode.AssignedMetaObjectIdenty != null)
            {
                DataNode.AssignedMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(DataNode.AssignedMetaObjectIdenty);
            }
            Data.Columns.Add("Object", DataNode.Classifier.GetExtensionMetaObject(typeof(Type)) as Type);
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    Data.Columns.Add(subDataNode.Name, (subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
            }

            Dictionary<object, PersistenceLayerRunTime.StorageInstanceRef> candidateForDeleteObjects = new Dictionary<object, PersistenceLayerRunTime.StorageInstanceRef>();

            foreach (var storageInstanceRef in CandidateForDeleteObjects)
                candidateForDeleteObjects[storageInstanceRef.MemoryInstance] = storageInstanceRef;

            foreach (StorageCell storageCell in DataLoaderMetadata.StorageCells)
            {

                foreach (System.Collections.Generic.KeyValuePair<object, WeakReference> entry in storageCell.Objects.StorageInstanceRefs)
                {
                    StorageInstanceRef storageInstanceRef = entry.Value.Target as StorageInstanceRef;
                    if (entry.Value == null || candidateForDeleteObjects.ContainsKey(storageInstanceRef.MemoryInstance))
                        continue;

                    IDataRow row = Data.NewRow();
                    row["Object"] = storageInstanceRef.MemoryInstance; ;

                    foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
                    {
                        if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        {
                            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = (storageCell.Type as DotNetMetaDataRepository.Class).GetFastFieldAccessor(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute);
                            row[subDataNode.Name] = Member<object>.GetValue(fastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                        }
                    }
                    Data.Rows.Add(row);
                }
            }
            foreach (var storageInstanceRef in NewObjects)
            {

                if (candidateForDeleteObjects.ContainsKey(storageInstanceRef.MemoryInstance))
                    continue;

                IDataRow row = Data.NewRow();
                row["Object"] = storageInstanceRef.MemoryInstance; ;

                foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
                {
                    if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    {

                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = storageInstanceRef.Class.GetFastFieldAccessor(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute);
                        row[subDataNode.Name] = Member<object>.GetValue(fastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                    }
                }
                Data.Rows.Add(row);

            }
            // throw new Exception("The method or operation is not implemented.");
        }

        public override bool LocalDataColumnExistFor(DataNode dataNode)
        {
            return true;
        }

        public DataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata dataLoaderMetadata)
            : base(dataNode, dataLoaderMetadata)
        {
            //foreach (StorageCell storageCell in DataLoaderMetadata.StorageCells)
            //{
            //    _LoadFromStorage = storageCell.Namespace as OOAdvantech.MetaDataRepository.Storage;
            //    break;
            //}
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


        public override Dictionary<Criterion, Stack<DataNode>[]> LocalResolvedCriterions
        {
            get
            {
                return new Dictionary<Criterion, Stack<DataNode>[]>();
            }
        }

        
    }
}
