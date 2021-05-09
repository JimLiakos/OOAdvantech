using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Cosmos.Table;
using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{13044d47-6e6a-4ad0-a19b-f0170c9920aa}</MetaDataID>


    #region remove temporarly
    /// <MetaDataID>{e252a4b4-f322-424a-815b-9ec502f55e5d}</MetaDataID>
    public class TableEntity : DynamicObject, ITableEntity,
            ICustomMemberProvider, MetaDataRepository.ObjectQueryLanguage.IDataRow // For LinqPad's Dump
    {
        /// <MetaDataID>{fad9a1c2-c27d-4702-96f9-e3e6f08eafab}</MetaDataID>
        public TableEntity()
        {
            this.Properties = new Dictionary<string, EntityProperty>();
        }

        /// <MetaDataID>{f1e63820-7d94-478a-8d02-2f35fd483e22}</MetaDataID>
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

        /// <MetaDataID>{4e9c0615-e300-4d7a-accb-8afe26a3a4dc}</MetaDataID>
        public IDictionary<string, EntityProperty> Properties { get; set; }

        /// <MetaDataID>{714b4c63-427e-44a9-9a0e-499761832831}</MetaDataID>
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
                if (value != DBNull.Value)
                {
                    var property = this.GetEntityProperty(key, value);

                    if (this.Properties.ContainsKey(key))
                        this.Properties[key] = property;
                    else
                        this.Properties.Add(key, property);
                }
            }
        }

        #region DynamicObject overrides

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }

        /// <MetaDataID>{e7d2f873-e573-477a-baeb-1b132b2abf05}</MetaDataID>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        #endregion

        #region ITableEntity implementation

        /// <MetaDataID>{dab261d8-ae60-44e4-9bde-55137cf5ac23}</MetaDataID>
        public string PartitionKey { get; set; }

        /// <MetaDataID>{5065acb6-8c0f-42de-a3fd-53e131376fd1}</MetaDataID>
        public string RowKey { get; set; }

        /// <MetaDataID>{5509ac6b-f718-480c-a07d-3ecc51ec2ea4}</MetaDataID>
        public DateTimeOffset Timestamp { get; set; }

        /// <MetaDataID>{fb93c5f7-9c3b-4db7-a313-8967d89161b9}</MetaDataID>
        public string ETag { get; set; }

        object[] IDataRow.ItemArray
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        IDataTable IDataRow.Table
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object IDataRow.this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        object IDataRow.this[int columnIndex]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{7f0f66dc-442e-4165-8975-e3ff4d0cea94}</MetaDataID>
        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            this.Properties = properties;
        }

        /// <MetaDataID>{397d79d5-b974-48a6-8712-858fedc4410d}</MetaDataID>
        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            return this.Properties;
        }

        #endregion

        #region ICustomMemberProvider implementation for LinqPad's Dump

        /// <MetaDataID>{8cca6ba4-e798-4ab4-9f06-a27d97d0a9d6}</MetaDataID>
        public IEnumerable<string> GetNames()
        {
            return new[] { "PartitionKey", "RowKey", "Timestamp", "ETag" }
                .Union(this.Properties.Keys);
        }

        /// <MetaDataID>{94296144-f7c7-4c27-9d31-79f010456fe6}</MetaDataID>
        public IEnumerable<Type> GetTypes()
        {
            return new[] { typeof(string), typeof(string), typeof(DateTimeOffset), typeof(string) }
                .Union(this.Properties.Values.Select(x => this.GetType(x.PropertyType)));
        }

        /// <MetaDataID>{8f2f34a5-6f00-4a8b-b116-66d6d44207bf}</MetaDataID>
        public IEnumerable<object> GetValues()
        {
            return new object[] { this.PartitionKey, this.RowKey, this.Timestamp, this.ETag }
                .Union(this.Properties.Values.Select(x => this.GetValue(x)));
        }

        #endregion

        /// <MetaDataID>{836d79b0-8330-4cf5-839c-b8e8092bc1a7}</MetaDataID>
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
            if (value.GetType() == typeof(decimal)) return new EntityProperty(Convert.ToDouble( (decimal)value));
            throw new Exception("not supported " + value.GetType() + " for " + key);
        }

        /// <MetaDataID>{b9c759e1-a7e3-4309-ae45-a2539b41079f}</MetaDataID>
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

        /// <MetaDataID>{fb596293-4fdf-4a9f-bea2-b40b70885b8a}</MetaDataID>
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

        IDataRow[] IDataRow.GetChildRows(string relationName)
        {
            throw new NotImplementedException();
        }

        IDataRow[] IDataRow.GetParentRows(string relationName)
        {
            throw new NotImplementedException();
        }

        IDataRow IDataRow.GetParentRow(string relationName)
        {
            throw new NotImplementedException();
        }

        void IDataRow.Delete()
        {
            throw new NotImplementedException();
        }
    }

    #endregion







    /// <MetaDataID>{822b70ee-843b-482c-a65f-3929febbc91d}</MetaDataID>
    public interface ICustomMemberProvider
    {
        // Each of these methods must return a sequence
        // with the same number of elements:
        /// <MetaDataID>{6d4b735f-cd56-4a1e-94cc-a99c6dc09ac6}</MetaDataID>
        IEnumerable<string> GetNames();
        /// <MetaDataID>{0676e485-ed71-443b-8579-31ffca65be3c}</MetaDataID>
        IEnumerable<Type> GetTypes();
        /// <MetaDataID>{40b9bb7d-786e-4c1b-abd2-387f71f58716}</MetaDataID>
        IEnumerable<object> GetValues();
    }
}
