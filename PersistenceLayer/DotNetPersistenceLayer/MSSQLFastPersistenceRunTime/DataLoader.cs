using System;
using System.Collections.Generic;
using System.Text;
using SubDataNodeIdentity = System.Guid;

namespace OOAdvantech.MSSQLFastPersistenceRunTime.ObjectQueryLanguage
{
    using MetaDataRepository.ObjectQueryLanguage;
    /// <MetaDataID>{f953de5c-53e9-4336-bc83-6e8a00c63515}</MetaDataID>
    public class DataLoader : OOAdvantech.PersistenceLayerRunTime.StorageDataLoader
    {
      
        public override List<string> RecursiveParentColumns
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override List<string> RecursiveParentReferenceColumns
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        public override bool HasRelationIdentityColumns
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        public override System.Collections.ArrayList GetDataColoumns()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override object GetObject(System.Data.DataRow row, out bool loadObjectLinks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override List<string> DataSourceRelationsColumnsWithParent
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }


        public override OOAdvantech.Collections.Generic.Dictionary<Guid, List<string>> DataSourceRelationsColumnsWithSubDataNodes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        OOAdvantech.MetaDataRepository.Storage _LoadFromStorage; 
        public override OOAdvantech.MetaDataRepository.Storage LoadFromStorage
        {
            get 
            {
                return _LoadFromStorage;
            }
        }
        public override OOAdvantech.MetaDataRepository.Classifier Classifier
        {
            get 
            {
                return DataNode.Classifier;
            }
        }
        public override void LoadDataLocally()
        {
            Data.TableName = DataNode.Name;
            if (DataNode.Classifier == null && DataNode.AssignedMetaObjectIdenty != null)
            {
                DataNode.AssignedMetaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObject(DataNode.AssignedMetaObjectIdenty);
            }
            Data.Columns.Add("Object",DataNode.Classifier.GetExtensionMetaObject(typeof(Type)) as Type);
            foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
            {
                if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                    Data.Columns.Add(subDataNode.Name,(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute).Type.GetExtensionMetaObject(typeof(Type)) as Type);
            }

            foreach (StorageCell storageCell in DataLoaderMetadata.StorageCells)
            {
                foreach (System.Collections.DictionaryEntry entry in storageCell.Objects.StorageInstanceRefs)
                {
                    StorageInstanceRef storageInstanceRef = (entry.Value as WeakReference).Target  as StorageInstanceRef;
                    if (entry.Value == null)
                        continue;

                    System.Data.DataRow row = Data.NewRow();
                    row["Object"] = storageInstanceRef.MemoryInstance; ;
                    
                    foreach (MetaDataRepository.ObjectQueryLanguage.DataNode subDataNode in DataNode.SubDataNodes)
                    {
                        if (subDataNode.Type == DataNode.DataNodeType.OjectAttribute)
                        {
                            //System.Reflection.FieldInfo field = (storageCell.Type as DotNetMetaDataRepository.Class).GetFieldMember(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute);
                            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = (storageCell.Type as DotNetMetaDataRepository.Class).GetFastFieldAccessor(subDataNode.AssignedMetaObject as DotNetMetaDataRepository.Attribute);
                            //row[subDataNode.Name] = field.GetValue(storageInstanceRef.MemoryInstance);
                            row[subDataNode.Name] = Member<object>.GetValue(fastFieldAccessor.GetValue, storageInstanceRef.MemoryInstance);
                        }
                    }

                    
                  //  if (Filter(row, DataNode.LocalFilter))
                        Data.Rows.Add(row);
                }
            }
           // throw new Exception("The method or operation is not implemented.");
        }

        //bool Filter(System.Data.DataRow row, SearchCondition filter)
        //{
        //    if (filter == null)
        //        return true;

        //    foreach (SearchTerm searchTerm in filter.SearchTerms)
        //    {
        //        if (Filter(row, searchTerm))
        //            return true;
        //    }
        //    return false;
        //}
        //bool Filter(System.Data.DataRow row, SearchTerm searchTerm)
        //{
        //    foreach (SearchFactor searchFactor in searchTerm.SearchFactors)
        //    {
        //        if (searchFactor.SearchCondition != null)
        //        {
        //            if (!Filter(row, searchFactor.SearchCondition))
        //                return false;
        //        }
        //        else
        //        {
        //            if (!Filter(row, searchFactor.Criterion))
        //                return false;
        //        }
        //    }
        //    return true;
        //}
        //bool Filter(System.Data.DataRow row, Criterion criterion)
        //{
        //    if (criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm &&
        //        (criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm).DataNode.ParentDataNode == DataNode &&
        //        criterion.ComparisonTerms[1] is MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm)
        //    {
        //        object value = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm).Value;
        //        System.Type type = (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm).ValueType;
                
        //        value = System.Convert.ChangeType(value, type);
        //        object rowFieldValue = row[(criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm).DataNode.Name];
        //        rowFieldValue = System.Convert.ChangeType(rowFieldValue ,type);


        //        if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal && (value is IComparable && rowFieldValue is IComparable))
        //            return (rowFieldValue as IComparable).CompareTo(value)==0;
        //        if (criterion.ComparisonOperator == Criterion.ComparisonType.NotEqual && (value is IComparable && rowFieldValue is IComparable))
        //            return (rowFieldValue as IComparable).CompareTo(value) != 0;

        //        if (criterion.ComparisonOperator == Criterion.ComparisonType.GreaterThan && (value is IComparable || rowFieldValue is IComparable))
        //            return (rowFieldValue as IComparable).CompareTo(value) == 1;

        //        if (criterion.ComparisonOperator == Criterion.ComparisonType.LessThan && (value is IComparable || rowFieldValue is IComparable))
        //            return (rowFieldValue as IComparable).CompareTo(value) == -1;


        //    }

        //    if (criterion.ComparisonTerms[1] is MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm &&
        //        (criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm).DataNode.ParentDataNode == DataNode &&
        //        criterion.ComparisonTerms[0] is MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm)
        //    {
        //        object value = (criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm).Value;
        //        System.Type type = (criterion.ComparisonTerms[0] as MetaDataRepository.ObjectQueryLanguage.LiteralComparisonTerm).ValueType;
        //        value = System.Convert.ChangeType(value, type);
        //        object rowFieldValue = row[(criterion.ComparisonTerms[1] as MetaDataRepository.ObjectQueryLanguage.ObjectAttributeComparisonTerm).DataNode.Name];
        //        rowFieldValue = System.Convert.ChangeType(rowFieldValue, type);

        //        if (criterion.ComparisonOperator == Criterion.ComparisonType.Equal && (value is IComparable && rowFieldValue is IComparable))
        //            return (rowFieldValue as IComparable).CompareTo(value) == 0;
        //        if (criterion.ComparisonOperator == Criterion.ComparisonType.NotEqual && (value is IComparable && rowFieldValue is IComparable))
        //            return (rowFieldValue as IComparable).CompareTo(value) != 0;


        //        if (criterion.ComparisonOperator == Criterion.ComparisonType.GreaterThan && (value is IComparable || rowFieldValue is IComparable))
        //            return (rowFieldValue as IComparable).CompareTo(value) == -1;

        //        if (criterion.ComparisonOperator == Criterion.ComparisonType.LessThan && (value is IComparable || rowFieldValue is IComparable))
        //            return (rowFieldValue as IComparable).CompareTo(value) == 1;
        //    }


        //    return true;
        //}


        public DataLoader(MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, MetaDataRepository.ObjectQueryLanguage.StorageDataSource.DataLoaderMetadata storageCells)
            : base(dataNode, storageCells)
        {
            foreach (StorageCell storageCell in DataLoaderMetadata.StorageCells)
            {
                _LoadFromStorage = storageCell.Namespace as OOAdvantech.MetaDataRepository.Storage;
                break;
            }

        }
        public override Dictionary<Criterion, Stack<DataNode>[]> LocalResolvedCriterions
        {
            get
            {
                return new Dictionary<Criterion,Stack<DataNode>[]>();
            }
        }


    }
}
