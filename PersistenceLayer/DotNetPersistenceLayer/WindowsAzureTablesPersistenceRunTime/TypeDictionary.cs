using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{373028d0-c0f2-4a08-835d-2bf692bb907b}</MetaDataID>
    public class TypeDictionary
    {
        public object Convert(object value, System.Type type)
        {
            return value;
        }

        internal Type GetDataTransferDotNetType(Type type)
        {
            return type;
        }

        internal string ConvertToString(object value)
        {
            if (value != null)
                return value.ToString();
            return "";
        }

        class QuerableType
        {
            public byte[] M_Binary { get; set; }

            public bool M_Bool { get; set; }
            public DateTime M_Date { get; set; }

            public double M_Double { get; set; }

            public Guid M_Guid { get; set; }

            public int M_Int { get; set; }

            public System.Int64 M_Long { get; set; }
            public String M_String { get; set; }
        }
        public string GenerateFilterConditionFor(string propertyName, string operation, object givenValue)
        {
            return GetFilterConditionFor(propertyName, operation, givenValue);
        }
        public static string GetFilterConditionFor(string propertyName, string operation, object givenValue)
        {
            string filter;

            if (givenValue is byte[])
            {
                filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_Binary == (byte[])givenValue);
                filter = string.Format(filter.Replace("M_Binary", propertyName).Replace(" eq ", " {0} "), operation);
                return filter;
            }
            if (givenValue is bool)
            {
                filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_Bool == (bool)givenValue);
                filter = string.Format(filter.Replace("M_Bool", propertyName).Replace(" eq ", " {0} "), operation);
                return filter;
            }
            if (givenValue is DateTime)
            {
                filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_Date == (DateTime)givenValue);
                filter = string.Format(filter.Replace("M_Date", propertyName).Replace(" eq ", " {0} "), operation);
                return filter;
            }
            if (givenValue is double)
            {
                filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_Double == (double)givenValue);
                filter = string.Format(filter.Replace("M_Double", propertyName).Replace(" eq ", " {0} "), operation);
                return filter;
            }
            if (givenValue is Guid)
            {
                filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_Guid == (Guid)givenValue);
                filter = string.Format(filter.Replace("M_Guid", propertyName).Replace(" eq ", " {0} "), operation);
                return filter;
            }
            if (givenValue is int)
            {
                filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_Int == (int)givenValue);
                filter = string.Format(filter.Replace("M_Int", propertyName).Replace(" eq ", " {0} "), operation);
                return filter;
            }
            if (givenValue is long)
            {
                filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_Long == (long)givenValue);
                filter = string.Format(filter.Replace("M_Long", propertyName).Replace(" eq ", " {0} "), operation);
                return filter;
            }
            if (givenValue is string)
            {
                givenValue= HttpUtility.UrlEncode((String)givenValue);
                filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_String == (String)givenValue);
                filter = string.Format(filter.Replace("M_String", propertyName).Replace(" eq ", " {0} "), operation);
                return filter;
            }
            filter = Azure.Data.Tables.TableClient.CreateQueryFilter<QuerableType>(x => x.M_String == givenValue.ToString());
            filter = string.Format(filter.Replace("M_String", propertyName).Replace(" eq ", " {0} "), operation);
            return filter;
        }

        public static EdmType GetEdmType(Type type)
        {

            if (type == typeof(byte[])) return EdmType.Binary;
            if (type == typeof(bool)) return EdmType.Boolean;
            if (type == typeof(DateTimeOffset)) return EdmType.DateTime;
            if (type == typeof(DateTime)) return EdmType.DateTime;
            if (type == typeof(double)) return EdmType.Double;
            if (type == typeof(Guid)) return EdmType.Guid;
            if (type == typeof(int)) return EdmType.Int32;
            if (type == typeof(long)) return EdmType.Int64;
            if (type == typeof(string)) return EdmType.String;
            if (type == typeof(decimal)) return EdmType.Double;
            if (type != null && type.BaseType == typeof(System.Enum)) return EdmType.Int32;
            throw new Exception("not supported type" + type.FullName);
        }
    }
}
