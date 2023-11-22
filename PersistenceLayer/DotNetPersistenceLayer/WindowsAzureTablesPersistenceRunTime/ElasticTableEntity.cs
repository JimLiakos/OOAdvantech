using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Azure;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{a80daf56-59e9-4bc5-9b57-024add4357b8}</MetaDataID>
    public class ElasticTableEntity : DynamicObject, Azure.Data.Tables.ITableEntity,
            ICustomMemberProvider // For LinqPad's Dump
    {
        public ElasticTableEntity()
        {
            this.Properties = new Dictionary<string, EntityProperty>();
        }

        internal Azure.Data.Tables.TableEntity Values;
        public ElasticTableEntity(Azure.Data.Tables.TableEntity values)
        {
            Values = values;
            this.PartitionKey = values.PartitionKey;
            this.RowKey = values.RowKey;
            this._ETag = values.ETag.ToString();

            this.Properties = new Dictionary<string, EntityProperty>();

            foreach (var entry in values)
            {
                this.Properties[entry.Key] = new EntityProperty(entry.Key, entry.Value, values);
            }

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

        //private IDictionary<string, Microsoft.Azure.Cosmos.Table.EntityProperty> CosmoProperties;

        public IDictionary<string, OOAdvantech.WindowsAzureTablesPersistenceRunTime.EntityProperty> Properties { get; set; }

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

            //EntityProperty property = new EntityProperty(key, null, Values);
            Values[key]=null;

            if (type == typeof(string)) return new EntityProperty(key,EdmType.String,Values);
            if (type == typeof(byte[])) return new EntityProperty(key, EdmType.Binary, Values);
            if (type == typeof(bool)) return new EntityProperty(key, EdmType.Boolean, Values);
            if (type == typeof(DateTimeOffset)) return new EntityProperty(key, EdmType.DateTime, Values);
            if (type == typeof(DateTime)) return new EntityProperty(key, EdmType.DateTime, Values);
            if (type == typeof(double)) return new EntityProperty(key, EdmType.Double, Values);
            if (type == typeof(Guid)) return new EntityProperty(key, EdmType.Guid, Values);
            if (type == typeof(int)) return new EntityProperty(key, EdmType.Int32, Values);
            if (type == typeof(long)) return new EntityProperty(key, EdmType.Int64, Values);
            if (type == typeof(decimal)) return new EntityProperty(key, EdmType.Double, Values);
            if (type.BaseType == typeof(Enum)) return new EntityProperty(key, EdmType.Int32, Values);
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
                if (Values != null)
                    Values[key] = value;
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

        string _PartitionKey;
        public string PartitionKey
        {
            get => _PartitionKey;
            set
            {
                _PartitionKey = value;
                if (Values != null)
                    Values.PartitionKey = value;
            }
        }


        string _RowKey;
        public string RowKey
        {
            get => _RowKey;
            set
            {
                _RowKey = value;
                if (Values != null)
                    Values.RowKey = value;
            }
        }

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
        DateTimeOffset? Azure.Data.Tables.ITableEntity.Timestamp { get; set; }
        ETag Azure.Data.Tables.ITableEntity.ETag { get; set; }

        //public void ReadEntity(IDictionary<string, Microsoft.Azure.Cosmos.Table.EntityProperty> properties, OperationContext operationContext)
        //{
        //    CosmoProperties = properties;
        //    this.Properties = new Dictionary<string, EntityProperty>();
        //    foreach (var entry in CosmoProperties)
        //        this.Properties[entry.Key] = new EntityProperty(entry.Value);
        //}



        //public IDictionary<string, Microsoft.Azure.Cosmos.Table.EntityProperty> WriteEntity(OperationContext operationContext)
        //{

        //    return this.CosmoProperties;
        //}

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
            return new EntityProperty(key, value, Values);
            //if (Values != null)
            //{
            //    return new EntityProperty(key, value, Values);
            //}
            //if (value == null) return new EntityProperty((string)null);
            //if (value.GetType() == typeof(byte[])) return new EntityProperty((byte[])value);
            //if (value.GetType() == typeof(bool)) return new EntityProperty((bool)value);
            //if (value.GetType() == typeof(DateTimeOffset)) return new EntityProperty((DateTimeOffset)value);
            //if (value.GetType() == typeof(DateTime)) return new EntityProperty((DateTime)value);
            //if (value.GetType() == typeof(double)) return new EntityProperty((double)value);
            //if (value.GetType() == typeof(Guid)) return new EntityProperty((Guid)value);
            //if (value.GetType() == typeof(int)) return new EntityProperty((int)value);
            //if (value.GetType() == typeof(long)) return new EntityProperty((long)value);
            //if (value.GetType() == typeof(string)) return new EntityProperty((string)value);
            //if (value.GetType() == typeof(decimal)) return new EntityProperty(Convert.ToDouble((decimal)value));
            //throw new Exception("not supported " + value.GetType() + " for " + key);
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
                else
                {
                    if (column == null)
                        continue;

                    if (dataLoader.ObjectIdentityTypes[0].Parts[0].Name == column.DataBaseColumnName)
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


    /// <MetaDataID>{876b60d1-c177-4dac-bc2c-6ca574c604c8}</MetaDataID>
    public class EntityProperty
    {

        //Microsoft.Azure.Cosmos.Table.EntityProperty Property;
        //public EntityProperty(Microsoft.Azure.Cosmos.Table.EntityProperty property)
        //{
        //    Property = property;
        //}

        EdmType _PropertyType;
        public EdmType PropertyType
        {
            get
            {
                //if (Property != null)
                //    return Property.PropertyType;
                //else
                return _PropertyType;
            }
        }
        //
        // Summary:
        //     Gets or sets the byte array value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The byte array value of this Microsoft.Azure.Cosmos.Table.EntityProperty object.
        //
        // Remarks:
        //     An exception is thrown if this property is set to a value other than a byte array.
        public byte[] BinaryValue
        {
            get
            {
                return TableEntity.GetBinary(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.Binary)
                //        return (byte[])Value;
                //}
                //return Property?.BinaryValue;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.BinaryValue = value;
            }
        }
        //
        // Summary:
        //     Gets or sets the boolean value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The boolean value of this Microsoft.Azure.Cosmos.Table.EntityProperty object.
        //
        // Remarks:
        //     An exception is thrown if this property is set to a value other than a boolean
        //     value.
        public bool? BooleanValue
        {
            get
            {
                return TableEntity.GetBoolean(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.Boolean)
                //        return (bool)Value;
                //}
                //return Property?.BooleanValue;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.BooleanValue = value;
            }
        }
        //
        // Summary:
        //     Gets or sets the Microsoft.Azure.Cosmos.Table.EntityProperty.DateTime value of
        //     this Microsoft.Azure.Cosmos.Table.EntityProperty object. An exception will be
        //     thrown if you attempt to set this property to anything other than a Microsoft.Azure.Cosmos.Table.EntityProperty.DateTime
        //     object.
        //
        // Value:
        //     The Microsoft.Azure.Cosmos.Table.EntityProperty.DateTime value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        public DateTime? DateTime
        {
            get
            {
                return TableEntity.GetDateTime(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.DateTime)
                //        return (DateTime)Value;
                //}
                //return Property?.DateTime;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.DateTime = value;
            }
        }
        //
        // Summary:
        //     Gets or sets the System.DateTimeOffset value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The System.DateTimeOffset value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Remarks:
        //     An exception is thrown if this property is set to a value other than a System.DateTimeOffset
        //     value.
        public DateTimeOffset? DateTimeOffsetValue
        {
            get
            {
                return TableEntity.GetDateTime(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.DateTime)
                //        return (DateTime)Value;
                //}
                //return Property?.DateTimeOffsetValue;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.DateTimeOffsetValue = value;
            }
        }
        //
        // Summary:
        //     Gets or sets the double value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The double value of this Microsoft.Azure.Cosmos.Table.EntityProperty object.
        //
        // Remarks:
        //     An exception is thrown if this property is set to a value other than a double
        //     value.
        public double? DoubleValue
        {
            get
            {
                return TableEntity.GetDouble(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.Double)
                //        return (Double)Value;
                //}
                //return Property?.DoubleValue;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.DoubleValue = value;
            }
        }
        //
        // Summary:
        //     Gets or sets the System.Guid value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The System.Guid value of this Microsoft.Azure.Cosmos.Table.EntityProperty object.
        //
        // Remarks:
        //     An exception is thrown if this property is set to a value other than a System.Guid
        //     value.
        public Guid? GuidValue
        {
            get
            {
                return TableEntity.GetGuid(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.Guid)
                //        return (Guid)Value;
                //}
                //return Property?.GuidValue;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.GuidValue = value;
            }
        }
        //
        // Summary:
        //     Gets or sets the System.Int32 value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The System.Int32 value of this Microsoft.Azure.Cosmos.Table.EntityProperty object.
        //
        // Remarks:
        //     An exception is thrown if this property is set to a value other than a System.Int32
        //     value.
        public int? Int32Value
        {
            get
            {
                return TableEntity.GetInt32(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.Int32)
                //        return (int)Value;
                //}
                //return Property?.Int32Value;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.Int32Value = value;
            }
        }

        //
        // Summary:
        //     Gets the Microsoft.Azure.Cosmos.Table.EdmType of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The Microsoft.Azure.Cosmos.Table.EdmType of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.

        //
        // Summary:
        //     Gets the Microsoft.Azure.Cosmos.Table.EntityProperty as a generic object.
        public object PropertyAsObject
        {
            get
            {
                if (Value != null)
                    return Value;
                //if (Property == null)
                return TableEntity[Name];
                //return Property?.Int32Value;
            }
            set
            {
                TableEntity[Name] = value;
            }
        }
        //
        // Summary:
        //     Gets or sets the string value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The string value of this Microsoft.Azure.Cosmos.Table.EntityProperty object.
        //
        // Remarks:
        //     An exception is thrown if this property is set to a value other than a string
        //     value.
        public string StringValue
        {
            get
            {
                return TableEntity.GetString(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.String)
                //        return (string)Value;
                //}
                //return Property?.StringValue;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.StringValue = value;
            }
        }
        //
        // Summary:
        //     Gets or sets the System.Int64 value of this Microsoft.Azure.Cosmos.Table.EntityProperty
        //     object.
        //
        // Value:
        //     The System.Int64 value of this Microsoft.Azure.Cosmos.Table.EntityProperty object.
        //
        // Remarks:
        //     An exception is thrown if this property is set to a value other than an System.Int64
        //     value.
        public long? Int64Value
        {
            get
            {
                return TableEntity.GetInt64(Name);
                //if (Property == null)
                //{
                //    if (PropertyType == EdmType.Int64)
                //        return (System.Int64)Value;
                //}
                //return Property?.Int64Value;
            }
            set
            {
                //if (TableEntity != null)
                TableEntity[Name] = value;
                //else
                //    Property.Int64Value = value;
            }
        }

        object PropertyValue;

        object Value;
        string Name;
        Azure.Data.Tables.TableEntity TableEntity;

        public EntityProperty(string name, EdmType propertyType, Azure.Data.Tables.TableEntity tableEntity)
        {
            TableEntity = tableEntity;
            _PropertyType = propertyType;
            Name = name;
        }

        public EntityProperty(string name, object value, Azure.Data.Tables.TableEntity tableEntity)
        {
            TableEntity = tableEntity;
            Name = name;

            if (value is byte[])
                _PropertyType = EdmType.Binary;

            if (value is bool)
                _PropertyType = EdmType.Boolean;

            if (value is DateTime)
                _PropertyType = EdmType.DateTime;

            if (value is DateTimeOffset)
            {
                _PropertyType = EdmType.DateTime;
                value = ((DateTimeOffset)value).UtcDateTime;
                _PropertyType = EdmType.DateTime;
            }

            if (value is double)
                _PropertyType = EdmType.Double;

            if (value is Guid)
                _PropertyType = EdmType.Guid;

            if (value is System.Int32)
                _PropertyType = EdmType.Int32;

            if (value is System.Int64)
                _PropertyType = EdmType.Int64;

            if (value is string)
                _PropertyType = EdmType.String;

            Value = value;
        }

    }

    //
    // Summary:
    //     Enumeration containing the types of values that can be stored in a table entity
    //     property.
    /// <MetaDataID>{531516f8-cfec-4d91-ad05-cb13821c1bae}</MetaDataID>
    public enum EdmType
    {
        //
        // Summary:
        //     Represents fixed- or variable-length character data.
        String = 0,
        //
        // Summary:
        //     Represents fixed- or variable-length binary data.
        Binary = 1,
        //
        // Summary:
        //     Represents the mathematical concept of binary-valued logic.
        Boolean = 2,
        //
        // Summary:
        //     Represents date and time.
        DateTime = 3,
        //
        // Summary:
        //     Represents a floating point number with 15 digits precision that can represent
        //     values with approximate range of +/- 2.23e -308 through +/- 1.79e +308.
        Double = 4,
        //
        // Summary:
        //     Represents a 16-byte (128-bit) unique identifier value.
        Guid = 5,
        //
        // Summary:
        //     Represents a signed 32-bit integer value.
        Int32 = 6,
        //
        // Summary:
        //     Represents a signed 64-bit integer value.
        Int64 = 7
    }
}

