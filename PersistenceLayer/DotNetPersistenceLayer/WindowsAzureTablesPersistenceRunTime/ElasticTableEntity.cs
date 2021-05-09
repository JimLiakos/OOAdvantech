using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Cosmos.Table;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{a80daf56-59e9-4bc5-9b57-024add4357b8}</MetaDataID>
    public class ElasticTableEntity : DynamicObject, ITableEntity,
            ICustomMemberProvider // For LinqPad's Dump
    {
        public ElasticTableEntity()
        {
            this.Properties = new Dictionary<string, EntityProperty>();
        }

        public String Value(String name)
        {
            switch (name)
            {
                case "PartitionKey":
                    return this.PartitionKey;
                case "RowKey":
                    return this.RowKey;
                default:
                    return this.Properties[name].StringValue;
            }
        }

        public IDictionary<string, EntityProperty> Properties { get; set; }

        public void SetNull(string key, System.Type type)
        {
            var property = this.GetNullEntityProperty(key, type);
            if (this.Properties.ContainsKey(key))
                this.Properties[key] = property;
            else
                this.Properties.Add(key, property);
        }

        private EntityProperty GetNullEntityProperty(string key, Type type)
        {
            if (type == typeof(string)) return new EntityProperty((string)null);
            if (type == typeof(byte[])) return new EntityProperty(default(byte[]));
            if (type == typeof(bool)) return new EntityProperty(default(bool?));
            if (type == typeof(DateTimeOffset)) return new EntityProperty(default(DateTimeOffset?));
            if (type == typeof(DateTime)) return new EntityProperty(default(DateTime?));
            if (type == typeof(double)) return new EntityProperty(default(double?));
            if (type == typeof(Guid)) return new EntityProperty(default(Guid?));
            if (type == typeof(int)) return new EntityProperty(default(int?));
            if (type == typeof(long)) return new EntityProperty(default(long?));
            if (type == typeof(decimal)) return new EntityProperty(default(double?));
            throw new Exception("not supported " + type + " for " + key);
        }

        public object this[string key]
        {
            get
            {
                if (!this.Properties.ContainsKey(key))
                    this.Properties.Add(key, this.GetEntityProperty(key, null));

                return this.Properties[key];
            }
            set
            {
                var property = this.GetEntityProperty(key, value);

                if (this.Properties.ContainsKey(key))
                    this.Properties[key] = property;
                else
                    this.Properties.Add(key, property);
            }
        }

        #region DynamicObject overrides

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        #endregion

        #region ITableEntity implementation

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        string _ETag;
        public string ETag
        {
            get
            {
                return _ETag;
            }
            set
            {
                _ETag = value;
            }
        }

        public string AzureTableName { get; internal set; }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            this.Properties = properties;
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            return this.Properties;
        }

        #endregion

        #region ICustomMemberProvider implementation for LinqPad's Dump

        public IEnumerable<string> GetNames()
        {
            return new[] { "PartitionKey", "RowKey", "Timestamp", "ETag" }
                .Union(this.Properties.Keys);
        }

        public IEnumerable<Type> GetTypes()
        {
            return new[] { typeof(string), typeof(string), typeof(DateTimeOffset), typeof(string) }
                .Union(this.Properties.Values.Select(x => this.GetType(x.PropertyType)));
        }

        public IEnumerable<object> GetValues()
        {
            return new object[] { this.PartitionKey, this.RowKey, this.Timestamp, this.ETag }
                .Union(this.Properties.Values.Select(x => this.GetValue(x)));
        }

        #endregion

        private EntityProperty GetEntityProperty(string key, object value)
        {
            if (value == null) return new EntityProperty((string)null);
            if (value.GetType() == typeof(byte[])) return new EntityProperty((byte[])value);
            if (value.GetType() == typeof(bool)) return new EntityProperty((bool)value);
            if (value.GetType() == typeof(DateTimeOffset)) return new EntityProperty((DateTimeOffset)value);
            if (value.GetType() == typeof(DateTime)) return new EntityProperty((DateTime)value);
            if (value.GetType() == typeof(double)) return new EntityProperty((double)value);
            if (value.GetType() == typeof(Guid)) return new EntityProperty((Guid)value);
            if (value.GetType() == typeof(int)) return new EntityProperty((int)value);
            if (value.GetType() == typeof(long)) return new EntityProperty((long)value);
            if (value.GetType() == typeof(string)) return new EntityProperty((string)value);
            if (value.GetType() == typeof(decimal)) return new EntityProperty(Convert.ToDouble((decimal)value));
            throw new Exception("not supported " + value.GetType() + " for " + key);
        }

        private Type GetType(EdmType edmType)
        {
            switch (edmType)
            {
                case EdmType.Binary: return typeof(byte[]);
                case EdmType.Boolean: return typeof(bool);
                case EdmType.DateTime: return typeof(DateTime);
                case EdmType.Double: return typeof(double);
                case EdmType.Guid: return typeof(Guid);
                case EdmType.Int32: return typeof(int);
                case EdmType.Int64: return typeof(long);
                case EdmType.String: return typeof(string);
                default: throw new Exception("not supported " + edmType);
            }
        }

        private object GetValue(EntityProperty property)
        {
            switch (property.PropertyType)
            {
                case EdmType.Binary: return property.BinaryValue;
                case EdmType.Boolean: return property.BooleanValue;
                case EdmType.DateTime: return property.DateTimeOffsetValue;
                case EdmType.Double: return property.DoubleValue;
                case EdmType.Guid: return property.GuidValue;
                case EdmType.Int32: return property.Int32Value;
                case EdmType.Int64: return property.Int64Value;
                case EdmType.String: return property.StringValue;
                default: throw new Exception("not supported " + property.PropertyType);
            }
        }



        internal void LoadRow(IDataTable temporaryDataTable, DataLoader dataLoader, int storageIdentity, Dictionary<DataLoader.DataColumn, RDBMSMetaDataRepository.Column> columnsMap)
        {

            IDataRow row = temporaryDataTable.NewRow();
            row[dataLoader.ObjectIdentityTypes[0].Parts[0].Name] = Guid.Parse(RowKey);
            row["OSM_StorageIdentity"] = storageIdentity;
            foreach (DataLoader.DataColumn dataColumn in dataLoader.ClassifierDataColumns)
            {
                RDBMSMetaDataRepository.Column column = null;
                columnsMap.TryGetValue(dataColumn, out column);

                if (column != null && column.DataBaseColumnName != dataColumn.Name)
                {

                }

                if (dataColumn.Name == "StorageCellID")
                {
                    row[dataColumn.Name] = int.Parse(PartitionKey);
                }
                else if (dataLoader.ObjectIdentityTypes[0].Parts[0].Name == column.DataBaseColumnName)
                    row[dataLoader.ObjectIdentityTypes[0].Parts[0].Name] = Guid.Parse(RowKey);
                else
                {
                    object value = null;
                    if (Properties.ContainsKey(column.DataBaseColumnName))
                        value = Properties[column.DataBaseColumnName].PropertyAsObject;
                    else
                    {

                    }

                    if (value is DateTime)
                    {

                        row[dataColumn.Name] = (DateTime)value;
                        if (((DateTime)row[dataColumn.Name]).ToUniversalTime() != ((DateTime)value).ToUniversalTime())
                            row[dataColumn.Name] = ((DateTime)value).ToLocalTime();
                        if (((DateTime)row[dataColumn.Name]).ToUniversalTime() != ((DateTime)value).ToUniversalTime())
                            row[dataColumn.Name] = ((DateTime)value).ToUniversalTime();

                    }
                    else
                    {
                        if (value != null)
                            row[dataColumn.Name] = value;
                        else
                            row[dataColumn.Name] = DBNull.Value;
                    }

                }
            }

            temporaryDataTable.Rows.Add(row);
        }



        internal void LoadRelationRow(IDataTable manyToManyRelationData, DataLoader dataLoader, int roleAStorageIdentity, int roleBStorageIdentity, List<DataLoader.DataColumn> associationTableColumns)
        {
            IDataRow row = manyToManyRelationData.NewRow();
            foreach (DataLoader.DataColumn column in associationTableColumns)
            {

                object value = Properties[column.Name].PropertyAsObject;
                if (value != null)
                    row[column.Alias] = value;
            }
            row["RoleAStorageIdentity"] = roleAStorageIdentity;
            row["RoleBStorageIdentity"] = roleBStorageIdentity;

            manyToManyRelationData.Rows.Add(row);
        }



    }
}
