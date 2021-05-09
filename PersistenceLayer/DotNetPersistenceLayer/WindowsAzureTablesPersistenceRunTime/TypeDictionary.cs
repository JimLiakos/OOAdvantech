using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Cosmos.Table;

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

        public string GenerateFilterConditionFor(string propertyName, string operation, object givenValue)
        {


            if (givenValue is byte[])
                return Microsoft.Azure.Cosmos.Table.TableQuery.GenerateFilterConditionForBinary(propertyName, operation, (byte[])givenValue);

            if (givenValue is bool)
                return Microsoft.Azure.Cosmos.Table.TableQuery.GenerateFilterConditionForBool(propertyName, operation, (bool)givenValue);

            if (givenValue is DateTime)
                return Microsoft.Azure.Cosmos.Table.TableQuery.GenerateFilterConditionForDate(propertyName, operation, (DateTime)givenValue);

            if (givenValue is double)
                return Microsoft.Azure.Cosmos.Table.TableQuery.GenerateFilterConditionForDouble(propertyName, operation, (double)givenValue);

            if (givenValue is Guid)
                return Microsoft.Azure.Cosmos.Table.TableQuery.GenerateFilterConditionForGuid(propertyName, operation, (Guid)givenValue);

            if (givenValue is int)
                return Microsoft.Azure.Cosmos.Table.TableQuery.GenerateFilterConditionForInt(propertyName, operation, (int)givenValue);

            if (givenValue is long)
                return Microsoft.Azure.Cosmos.Table.TableQuery.GenerateFilterConditionForLong(propertyName, operation, (long)givenValue);

            return  Microsoft.Azure.Cosmos.Table.TableQuery.GenerateFilterCondition(propertyName, operation,givenValue.ToString());


        }

        public static EdmType GetEdmType( Type type)
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
            if(type!=null&&type.BaseType == typeof(System.Enum)) return EdmType.Int32;
            throw new Exception("not supported type" + type.FullName );
        }
    }
}
