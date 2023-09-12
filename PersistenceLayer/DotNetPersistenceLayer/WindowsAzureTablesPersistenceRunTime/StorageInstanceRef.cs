using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.DotNetMetaDataRepository;
using PartTypeName = System.String;
using CreatorIdentity = System.String;

using OOAdvantech.RDBMSMetaDataRepository;
using System.Linq;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{e4ee33ef-19b4-41f7-92df-ff4ddc796ab0}</MetaDataID>
    public class StorageInstanceRef : PersistenceLayerRunTime.StorageInstanceRef
    {
        static Dictionary<RDBMSMetaDataRepository.Table, Dictionary<string, StorageInstanceRef>> TablesEntities = new Dictionary<RDBMSMetaDataRepository.Table, Dictionary<string, StorageInstanceRef>>();

        ElasticTableEntity _TableEntity;
        internal ElasticTableEntity TableEntity
        {
            get => _TableEntity;
            set
            {
                _TableEntity = value;
                lock (TablesEntities)
                {
                    Dictionary<string, StorageInstanceRef> tableStorageInstanceRefs = null;
                    if (!TablesEntities.TryGetValue((StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable, out tableStorageInstanceRefs))
                    {
                        tableStorageInstanceRefs = new Dictionary<PartTypeName, StorageInstanceRef>();
                        TablesEntities[(StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable] = tableStorageInstanceRefs;
                    }
                    tableStorageInstanceRefs[_TableEntity.PartitionKey + _TableEntity.RowKey] = this;
                }
            }
        }

        internal static StorageInstanceRef GetStorageInstanceRef(Table table, string partitionKey, string rowKey)
        {
            StorageInstanceRef storageInstanceRef = null;
            lock (TablesEntities)
            {


                Dictionary<string, StorageInstanceRef> tableStorageInstanceRefs = null;
                if (TablesEntities.TryGetValue(table, out tableStorageInstanceRefs))
                    tableStorageInstanceRefs.TryGetValue(partitionKey + rowKey, out storageInstanceRef);
            }
            return storageInstanceRef;
        }

        ~StorageInstanceRef()
        {
            if (TableEntity != null)
            {
                lock (TablesEntities)
                {
                    Dictionary<string, StorageInstanceRef> tableStorageInstanceRefs = null;
                    if (StorageInstanceSet != null && TablesEntities.TryGetValue((StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MainTable, out tableStorageInstanceRefs))
                        if (tableStorageInstanceRefs.ContainsKey(TableEntity.PartitionKey + TableEntity.RowKey))
                            tableStorageInstanceRefs.Remove(TableEntity.PartitionKey + TableEntity.RowKey);
                }
            }
        }



        /// <MetaDataID>{24d8250f-5cbd-40fe-878f-4d28208dd201}</MetaDataID>
        internal static object GetValueTypeValue(OOAdvantech.MetaDataRepository.Attribute member, PersistenceLayerRunTime.StorageInstanceRef.ObjectSate row, string columnPrefix, string columnsNameSuffix, RDBMSMetaDataRepository.StorageCell storageInstanceSet)
        {
            System.Type type = member.Type.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
            object obj = AccessorBuilder.GetDefaultValue(type);
            //obj= type.Assembly.CreateInstance(type.FullName);

            TypeDictionary TypeDictionary = (PersistenceLayerRunTime.ObjectStorage.GetStorageOfObject(storageInstanceSet) as ObjectStorage).TypeDictionary;

            MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
            valueTypePath.Push(member.Identity);

            foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeMetaData(member.Type as DotNetMetaDataRepository.Structure, valueTypePath, member.Name))
            {

                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (storageInstanceSet.Namespace as Storage).GetEquivalentMetaObject(valueOfAttribute.Attribute) as RDBMSMetaDataRepository.Attribute;
                if (rdbmsAttribute == null)
                    throw new System.Exception("There aren't mapping data for '" + valueOfAttribute.Attribute.FullName + "'.");
#if !DeviceDotNet

                if (valueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.XmlDocument))
                {
                    throw new System.Exception("MSSQL subsystem can't save xml document");
                }
                else
                if (valueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.Linq.XDocument))
                {
                    throw new System.Exception("MSSQL subsystem can't save xml document");
                }
                else
#else
                if (valueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.Linq.XDocument))
                {
                    throw new System.Exception("MSSQL subsystem can't save xml document");
                }
                else
#endif
                {
                    //TODO: Performance tunning πανω στο get value από την recordset 
                    //όταν πέρνω data χρησιμοποιό αλλη τεχνική από αυτήν που σώζω caseinsensitive τιμές
                    object Value = null;
                    if (string.IsNullOrEmpty(valueOfAttribute.Path))
                        Value = row[columnPrefix + rdbmsAttribute.Name + columnsNameSuffix];
                    else
                        Value = row[columnPrefix + valueOfAttribute.Path + "_" + rdbmsAttribute.Name + columnsNameSuffix];

                    Value = TypeDictionary.Convert(Value, valueOfAttribute.FieldInfo.FieldType);

                    bool valueSetted = false;
                    if (Value is System.DBNull)
                        SetAttributeValue(ref obj, new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, null, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, null), member.Type as DotNetMetaDataRepository.Structure, 0, out valueSetted);
                    else
                        SetAttributeValue(ref obj, new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, Value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, null), member.Type as DotNetMetaDataRepository.Structure, 0, out valueSetted);
                }
            }



            return obj;



        }

        /// <MetaDataID>{21d60cc8-0bc9-4584-8c73-1eb2b31cfeee}</MetaDataID>
        public void LoadObjectState(string columnsNamePreffix)
        {

            //Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, Dictionary<string, int>>  attributeIndices = null;
            ClassMembersMappedColumnsIndices metaObjectsColumnsIndices = DbDataRecord.Tag as ClassMembersMappedColumnsIndices;

            //attributeIndices = metaObjectsColumnsIndices.AttributeIndices;

            //Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, Dictionary<CreatorIdentity, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, Dictionary<PartTypeName, int>>>> associationEndIndices = null;
            //(DbDataRecord.Tag as Dictionary<string, object>).TryGetValue("AssociationEndIndices", out value);
            //associationEndIndices = value as Dictionary<OOAdvantech.MetaDataRepository.MetaObjectID, Dictionary<CreatorIdentity, Dictionary<OOAdvantech.MetaDataRepository.ObjectIdentityType, Dictionary<PartTypeName, int>>>>;

            object value = null;
            TypeDictionary TypeDictionary = (ObjectStorage as ObjectStorage).TypeDictionary;

            if ((StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).Type.HasReferentialIntegrityRelations())
            {
                object ReferentialIntegrityCountValue = DBNull.Value;
                DbDataRecord.TryGetValue(columnsNamePreffix + "ReferenceCount", out ReferentialIntegrityCountValue);
                if (ReferentialIntegrityCountValue == null)
                    _ReferentialIntegrityCount = 0;
                else if (ReferentialIntegrityCountValue.GetType() == typeof(System.DBNull))
                    _ReferentialIntegrityCount = 0;
                else
                {
                    _ReferentialIntegrityCount = System.Convert.ToInt32(ReferentialIntegrityCountValue);
                    _RuntimeReferentialIntegrityCount.Value = System.Convert.ToDecimal(ReferentialIntegrityCountValue);
                }
            }

            // (StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).GetRelationshipColumns();



            foreach (RDBMSMetaDataRepository.Table table in (StorageInstanceSet as RDBMSMetaDataRepository.StorageCell).MappedTables)
            {
                if (Class.FullName == "FlavourBusinessManager.EndUsers.FoodServiceClientSession")
                {
                    var sss = table.ContainedColumns.Where(x => x is IdentityColumn && x.MappedAssociationEnd != null);
                }

                foreach (RDBMSMetaDataRepository.Column column in table.ContainedColumns)
                {

                    if (column is IdentityColumn && column.MappedAssociationEnd != null)
                    {
                        var objIdType = (column as RDBMSMetaDataRepository.IdentityColumn).ObjectIdentityType;
                        RelationshipColumnsValues[column] = null;
                        //if (column.MappedAssociationEnd.Indexer)
                        //{
                        //    var valueTypePath = new MetaDataRepository.ValueTypePath(column.CreatorIdentity);
                        //    if(valueTypePath.Count>0)
                        //        valueTypePath.Pop();
                        //    RDBMSMetaDataRepository.Column indexerColumn = column.MappedAssociationEnd.GetIndexerColumnFor(table, valueTypePath.ToString());
                        //    if (indexerColumn != null)
                        //        RelationshipColumnsValues[indexerColumn] = null;
                        //}
                    }
                    if (column.IndexerAssociationEnd != null && column.IndexerAssociationEnd.Indexer)
                        RelationshipColumnsValues[column] = null;
                }
            }
            foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeMetaData())
            {
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(valueOfAttribute.Attribute) as RDBMSMetaDataRepository.Attribute;
                if (rdbmsAttribute == null)
                    throw new System.Exception("There aren't mapping data for '" + valueOfAttribute.Attribute.FullName + "'.");

                RDBMSMetaDataRepository.Column column = null;

                foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(this.StorageInstanceSet as RDBMSMetaDataRepository.StorageCell))
                {
                    if (attributeColumn.CreatorIdentity == valueOfAttribute.PathIdentity)
                    {
                        column = attributeColumn;
                        break;
                    }
                }
                //TODO: Performance tunning πανω στο get value από την recordset 
                //όταν πέρνω data χρησιμοποιό αλλη τεχνική από αυτήν που σώζω caseinsensitive τιμές
                //object Value = DbDataRecord[columnsNamePreffix + column.Name];

                if (column == null)
                    Class.IsPersistent(rdbmsAttribute);


                value = DbDataRecord[metaObjectsColumnsIndices.AttributeIndices[rdbmsAttribute.Identity][valueOfAttribute.PathIdentity]];
                if (this.Class.IsMultilingual(valueOfAttribute.Attribute) && value != null && value != DBNull.Value)
                {
                    if(valueOfAttribute.Attribute.FullName == "FlavourBusinessFacade.RoomService.IItemPreparation.FontUri")
                    {

                    }
                    var attributeType = valueOfAttribute.Attribute.Type.GetExtensionMetaObject<System.Type>();
                    var dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(string), attributeType);
                    var multiligualDictionary = OOAdvantech.Json.JsonConvert.DeserializeObject(value as string, dictionaryType) as System.Collections.IDictionary;

                    foreach (var entryKey in multiligualDictionary.Keys)
                    {
                        var culture = System.Globalization.CultureInfo.GetCultureInfo(entryKey as string);
                        using (OOAdvantech.CultureContext cultureContext = new CultureContext(culture, false))
                        {
                            value = multiligualDictionary[entryKey];
                            SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, null));
                            SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, value);
                        }
                    }

                }
                else
                {
                    if (valueOfAttribute.FieldInfo.FieldType.GetMetaData().IsGenericType &&
                        !valueOfAttribute.FieldInfo.FieldType.GetMetaData().IsGenericTypeDefinition &&
                        valueOfAttribute.FieldInfo.FieldType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                        value = TypeDictionary.Convert(value, valueOfAttribute.FieldInfo.FieldType.GetMetaData().GetGenericArguments()[0]);
                    else
                        value = TypeDictionary.Convert(value, valueOfAttribute.FieldInfo.FieldType);


                    if (value is System.DBNull)
                        value = null;
                    if (value is DateTime)
                        value = ((DateTime)value).ToUniversalTime().ToLocalTime();
                    SetAttributeValue(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, null));
                    SnapshotStorageInstanceValue(valueOfAttribute.PathIdentity, valueOfAttribute.FieldInfo, value);
                }


                RelationshipColumnsValues.Remove(column);
            }
            foreach (RDBMSMetaDataRepository.Column column in new System.Collections.Generic.List<RDBMSMetaDataRepository.Column>(RelationshipColumnsValues.Keys))
            {
                if (column is IdentityColumn && column.MappedAssociationEnd != null)
                {
                    value = DbDataRecord[metaObjectsColumnsIndices.AssociationEndIndices[column.CreatorIdentity][column.MappedAssociationEnd.Identity][(column as RDBMSMetaDataRepository.IdentityColumn).ObjectIdentityType][(column as MetaDataRepository.IIdentityPart).PartTypeName]];
                    RelationshipColumnsValues[column] = value;
                }
                if (column.IndexerAssociationEnd != null && column.IndexerAssociationEnd.Indexer)
                {
                    value = DbDataRecord[metaObjectsColumnsIndices.AssociationEndIndexerColumnIndices[column.CreatorIdentity][column.IndexerAssociationEnd.Identity]];
                    RelationshipColumnsValues[column] = value;
                }
            }


        }



        ///// <MetaDataID>{a1192dd7-d780-4dcb-81dc-e689e16f142e}</MetaDataID>
        //public System.Data.Common.DbCommand OleDbCommand;
        ///// <MetaDataID>{1EF210B7-836C-448C-B75B-FC1573C9C5B6}</MetaDataID>
        //public void SaveObjectState()
        //{



        //    /*
        //    if(!OleDbCommand.Parameters.Contains("@ObjCellID"))
        //        OleDbCommand.Parameters.Add("@ObjCellID",((int)PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(StorageInstanceSet.Properties).ObjectID));
        //    else
        //        OleDbCommand.Parameters["@ObjCellID"].Value=((int)PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(StorageInstanceSet.Properties).ObjectID);
        //        */


        //    //	theXmlElement.SetAttribute("ReferentialIntegrityCount",ReferentialIntegrityCount.ToString());

        //    if (Class.HasReferentialIntegrityRelations())
        //    {
        //        System.Data.Common.DbParameter CurrParameter = null;
        //        if (!OleDbCommand.Parameters.Contains("@ReferenceCount"))
        //        {
        //            CurrParameter = OleDbCommand.CreateParameter();
        //            CurrParameter.ParameterName = "@ReferenceCount";
        //            CurrParameter.DbType = System.Data.DbType.Int32;
        //            CurrParameter.Value = _ReferentialIntegrityCount;
        //            OleDbCommand.Parameters.Add(CurrParameter);

        //        }
        //        else
        //        {
        //            CurrParameter = OleDbCommand.Parameters["@ReferenceCount"];
        //            OleDbCommand.Parameters["@ReferenceCount"].Value = _ReferentialIntegrityCount;
        //        }
        //    }

        //    foreach (ValueOfAttribute ValueOfAttribute in GetPersistentAttributeValues())
        //    {
        //        //if(!Class.IsPersistent(attribute))
        //        //    continue;

        //        RDBMSMetaDataRepository.Attribute rdbmsAttribute = ValueOfAttribute.Attribute.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.Attribute)) as RDBMSMetaDataRepository.Attribute;
        //        RDBMSMetaDataRepository.Column column = null;
        //        foreach (RDBMSMetaDataRepository.Column attributeColumn in rdbmsAttribute.GetColumnsFor(StorageInstanceSet as RDBMSMetaDataRepository.StorageCell))
        //        {
        //            if (attributeColumn.CreatorIdentity == ValueOfAttribute.PathIdentity)
        //            {
        //                column = attributeColumn;
        //                break;
        //            }
        //        }


        //        //string parameterName=column.MappedAttribute.CaseInsensitiveName;
        //        string parameterName = column.Name;




        //        string FieldName = ValueOfAttribute.Attribute.Name;

        //        object Value = ValueOfAttribute.Value;
        //        System.Data.Common.DbParameter CurrParameter = null;
        //        RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary = (ObjectStorage as ObjectStorage).TypeDictionary;

        //        if (Value == null)
        //            Value = TypeDictionary.GetNullValue(ValueOfAttribute.FieldInfo.FieldType.FullName);

        //        if (!OleDbCommand.Parameters.Contains("@" + parameterName))
        //        {
        //            CurrParameter = OleDbCommand.CreateParameter();
        //            CurrParameter.ParameterName = "@" + parameterName;
        //            CurrParameter.Value = Value;
        //            OleDbCommand.Parameters.Add(CurrParameter);
        //        }
        //        else
        //        {
        //            CurrParameter = OleDbCommand.Parameters["@" + parameterName];
        //            OleDbCommand.Parameters["@" + parameterName].Value = Value;
        //        }
        //        //TODO να τσεκαριστεί τι γίνεται όταν η τιμή είναι DBNull
        //        /*
        //        if(Value is System.DBNull)
        //            CurrParameter.OleDbType=System.Data.OleDb.OleDbType.Empty;
        //         */


        //        //				CurrParameter.Value=Value;
        //        if (ValueOfAttribute.FieldInfo.FieldType.BaseType == typeof(System.Enum))
        //        {

        //            if (Value != null)
        //            {
        //                //StringValue=Value.ToString();
        //                //theXmlElement.SetAttribute(FieldName,StringValue);
        //            }
        //            continue;
        //        }
        //        if (ValueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.XmlDocument))
        //        {
        //        }
        //        //StringValue=CurrFieldInfo.GetValue(MemoryInstance).ToString();
        //        try
        //        {
        //            //System.Convert.ChangeType(StringValue,CurrFieldInfo.FieldType);
        //            //theXmlElement.SetAttribute(FieldName,StringValue);
        //        }
        //        catch (System.Exception aException)
        //        {

        //        }
        //    }


        //}


        /// <MetaDataID>{70eb72c9-95c9-47a8-a845-b40d9d117559}</MetaDataID>
        public PersistenceLayerRunTime.StorageInstanceRef.ObjectSate DbDataRecord;


        /// <MetaDataID>{755574c1-4fe1-4ac4-acbc-f608cf09cb39}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell StorageInstanceSet
        {
            get
            {
                return _StorageInstanceSet;
            }
        }


        /// <MetaDataID>{90815f6c-db63-4214-9a37-10d40390adf6}</MetaDataID>
        public StorageInstanceRef(object memoryInstance, OOAdvantech.MetaDataRepository.StorageCell storageCell, ObjectStorage activeStorageSession, PersistenceLayer.ObjectID objectID)
            : base(memoryInstance, activeStorageSession, storageCell, objectID)
        {
            _StorageInstanceSet = storageCell;

        }

        /// <MetaDataID>{252d6f46-9e42-4014-98d5-7f4783eef6c5}</MetaDataID>
        System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Column, object> RelationshipColumnsValuesSnashot;

        /// <MetaDataID>{145933a0-f469-4401-8582-f3e127014647}</MetaDataID>
        public System.Collections.Generic.Dictionary<RDBMSMetaDataRepository.Column, object> RelationshipColumnsValues = new Dictionary<RDBMSMetaDataRepository.Column, object>();

        /// <MetaDataID>{1a490537-8876-479b-9d4b-a96969590c5a}</MetaDataID>
        public override void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            base.UndoChanges(transaction);
            RelationshipColumnsValues = RelationshipColumnsValuesSnashot;
        }
        /// <MetaDataID>{50800b85-02cc-49e1-a21a-f227bcaa9be3}</MetaDataID>
        public override void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            base.MarkChanges(transaction);
            RelationshipColumnsValuesSnashot = new Dictionary<RDBMSMetaDataRepository.Column, object>(RelationshipColumnsValues);
        }


        /// <MetaDataID>{bc49f425-b520-415e-8483-46749a57a47f}</MetaDataID>
        protected override PersistenceLayerRunTime.RelResolver CreateRelationResolver(DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
        {
            return new RelResolver(this, thePersistentAssociationEnd, fastFieldAccessor);
        }
        /// <MetaDataID>{544c67a0-8ed8-4341-874d-ff83128b8236}</MetaDataID>
        protected override OOAdvantech.PersistenceLayerRunTime.RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef owner)
        {
            return new RelResolver(owner, thePersistentAssociationEnd, fastFieldAccessor);

        }

        public override void ObjectActived()
        {
            base.ObjectActived();
        }



    }


}
