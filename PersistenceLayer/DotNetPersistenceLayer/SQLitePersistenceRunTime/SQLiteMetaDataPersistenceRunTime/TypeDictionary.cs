using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace OOAdvantech.SQLiteMetaDataPersistenceRunTime
{
    /// <MetaDataID>{58ec88c2-bca7-4478-b56f-b3be88f65096}</MetaDataID>
    public class TypeDictionary:RDBMSMetaDataPersistenceRunTime.TypeDictionary
    {

        public override string GetDBDefaultValue(string theType)
        {
            if (theType.Equals("System.Boolean"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.Byte"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.Char"))
                return "CONVERT(" + GetDBType(theType) + ", '')";
            if (theType.Equals("System.Decimal"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.Double"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.Single"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.Int32"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.Int64"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.SByte"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.Int16"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.UInt32"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.UInt64"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.UInt16"))
                return "CONVERT(" + GetDBType(theType) + ", 0)";
            if (theType.Equals("System.DateTime"))
                return "NULL";
            if (theType.Equals("System.String"))
                return "CONVERT(" + GetDBType(theType) + ", '')";
            if (theType.Equals("System.Drawing.Image"))
                return "NULL";
            if (theType.Equals("System.Guid"))
                return "CONVERT(uniqueidentifier, '" + System.Guid.Empty.ToString() + "')";
            return null;
        }


        public override System.Type GetTypeForDBType(string dataType)
        {
            if (dataType.Equals("uniqueidentifier"))
                return typeof(System.Guid);
            if (dataType.Equals("bit"))
                return typeof(System.Boolean);
            if (dataType.Equals("char"))
                return typeof(System.Byte);
            if (dataType.Equals("decimal"))
                return typeof(System.Decimal);
            if (dataType.Equals("float"))
                return typeof(System.Double);
            //if (dataType.Equals("System.Single"))
            //    return typeof(System.Single);
            if (dataType.Equals("int"))
                return typeof(System.Int32);
            if (dataType.Equals("bigint"))
                return typeof(System.Int64);
            //if (dataType.Equals("System.SByte"))
            //    return typeof(System.SByte);
            if (dataType.Equals("smallint"))
                return typeof(System.Int16);
            //if (dataType.Equals("int"))
            //    return typeof(System.UInt32);
            //if (dataType.Equals("bigint"))
            //    return typeof(System.UInt64);
            //if (dataType.Equals("System.UInt16"))
            //    return typeof(System.UInt16);
            if (dataType.Equals("datetime"))
                return typeof(System.DateTime);
            if (dataType.Equals("nvarchar"))
                return typeof(System.String);
            if (dataType.Equals("varchar"))
                return typeof(System.String);
#if DeviceDotNet
            if (dataType.Equals("image"))
                return typeof(Xamarin.Forms.Image);
#else
            if (dataType.Equals("image"))
                return typeof(System.Drawing.Image);
#endif
            return null;

            //if (theType.Equals("System.Guid"))
            //    return "uniqueidentifier";
            //if (theType.Equals("System.Boolean"))
            //    return "bit";
            //if (theType.Equals("System.Byte"))
            //    return "char";
            //if (theType.Equals("System.Char"))
            //    return "char";
            //if (theType.Equals("System.Decimal"))
            //    return "decimal";
            //if (theType.Equals("System.Double"))
            //    return "float";
            //if (theType.Equals("System.Single"))
            //    return "float";
            //if (theType.Equals("System.Int32"))
            //    return "int";
            //if (theType.Equals("System.Int64"))
            //    return "bigint";
            //if (theType.Equals("System.SByte"))
            //    return "char";
            //if (theType.Equals("System.Int16"))
            //    return "smallint";
            //if (theType.Equals("System.UInt32"))
            //    return "int";
            //if (theType.Equals("System.UInt64"))
            //    return "bigint";
            //if (theType.Equals("System.UInt16"))
            //    return "smallint";
            //if (theType.Equals("System.DateTime"))
            //    return "datetime";
            //if (theType.Equals("System.String") && FixLength)
            //    return "nvarchar";
            //if (theType.Equals("System.String") && (!FixLength))
            //    return "nvarchar";
            //if (theType.Equals("System.Drawing.Image"))
            //    return "image";
            //if (theType.Equals("enum"))
            //    return "int";
        }
       

        /// <MetaDataID>{4F476658-37A3-4F06-B541-A158BC81C0A3}</MetaDataID>
        public override string GetDBType(string theType)
        {
            return GetDBType(theType, true);
        }
        public override Type GetDataTransferDotNetType(Type type)
        {
            return type;
        }


        /// <MetaDataID>{AA1AF657-71C2-4C5F-89BC-44A54A9E4500}</MetaDataID>
        public override object GetNullValue(string theType)
        {
            if (theType.Equals("System.Boolean"))
                return false;
            if (theType.Equals("System.Byte"))
                return 0;
            if (theType.Equals("System.Char"))
                return ' ';
            if (theType.Equals("System.Decimal"))
                return 0;
            if (theType.Equals("System.Double"))
                return 0;
            if (theType.Equals("System.Single"))
                return 0;
            if (theType.Equals("System.Int32"))
                return 0;
            if (theType.Equals("System.Int64"))
                return 0;
            if (theType.Equals("System.SByte"))
                return 0;
            if (theType.Equals("System.Int16"))
                return 0;
            if (theType.Equals("System.UInt32"))
                return 0;
            if (theType.Equals("System.UInt64"))
                return 0;
            if (theType.Equals("System.UInt16"))
                return 0;
            if (theType.Equals("System.DateTime"))
                return System.DateTime.Now;
            if (theType.Equals("System.String"))
                return "";
            if (theType.Equals("System.Drawing.Image"))
                return System.DBNull.Value;

            if (theType.Equals("enum"))
                return System.DBNull.Value;


            if (theType.Equals("System.Guid"))
                return System.Guid.Empty;

            return null;
        }
        public override string GetDBNullScript(string theType)
        {
            return "NULL";
            if (theType.Equals("System.Boolean"))
                return "NULL";
            if (theType.Equals("System.Byte"))
                return "NULL";
            if (theType.Equals("System.Char"))
                return "NULL";
            if (theType.Equals("System.Decimal"))
                return "NULL";
            if (theType.Equals("System.Double"))
                return "NULL";
            if (theType.Equals("System.Single"))
                return "NULL";
            if (theType.Equals("System.Int32"))
                return "NULL";
            if (theType.Equals("System.Int64"))
                return "NULL";
            if (theType.Equals("System.SByte"))
                return "NULL";
            if (theType.Equals("System.Int16"))
                return "NULL";
            if (theType.Equals("System.UInt32"))
                return "NULL";
            if (theType.Equals("System.UInt64"))
                return "NULL";
            if (theType.Equals("System.UInt16"))
                return "NULL";
            if (theType.Equals("System.DateTime"))
                return "NULL";
            if (theType.Equals("System.String"))
                return "NULL";
            if (theType.Equals("System.Drawing.Image"))
                return "NULL";
            if (theType.Equals("System.Guid"))
                return "NULL";// "CONVERT(uniqueidentifier, '" + System.Guid.Empty.ToString() + "')";
            return null;
        }

        /// <MetaDataID>{1ED248FD-0D6C-40C9-9C00-9B5B6E780685}</MetaDataID>
        public override bool IsTypeVarLength(string theType)
        {
            if (theType.Equals("System.Boolean"))
                return false;
            if (theType.Equals("System.Byte"))
                return false;
            if (theType.Equals("System.Char"))
                return false;
            if (theType.Equals("System.Decimal"))
                return false;
            if (theType.Equals("System.Double"))
                return false;
            if (theType.Equals("System.Single"))
                return false;
            if (theType.Equals("System.Int32"))
                return false;
            if (theType.Equals("System.Int64"))
                return false;
            if (theType.Equals("System.SByte"))
                return false;
            if (theType.Equals("System.Int16"))
                return false;
            if (theType.Equals("System.UInt32"))
                return false;
            if (theType.Equals("System.UInt64"))
                return false;
            if (theType.Equals("System.UInt16"))
                return false;
            if (theType.Equals("System.DateTime"))
                return false;
            if (theType.Equals("System.String"))
                return true;
            if (theType.Equals("System.Drawing.Image"))
                return false;
            if (theType.Equals("System.Guid"))
                return true;

            return false;


        }
        /// <MetaDataID>{365EA3BB-7772-43DF-BDB0-D27F232CCF2E}</MetaDataID>
        public override int GeDefaultLength(string theType)
        {

            if (theType.Equals("System.Guid"))
                return 36;
            /*if(theType.Equals("System.Byte"))
                return 1;
            if(theType.Equals("System.Char"))
                return 1;
            if(theType.Equals("System.Decimal"))
                return 8;
            if(theType.Equals("System.Double"))
                return 8;
            if(theType.Equals("System.Single"))
                return 8;
            if(theType.Equals("System.Int32"))
                return 4;
            if(theType.Equals("System.Int64"))
                return 8;
            if(theType.Equals("System.SByte"))
                return 1;
            if(theType.Equals("System.Int16"))
                return 2;
            if(theType.Equals("System.UInt32"))
                return 4;
            if(theType.Equals("System.UInt64"))
                return 8;
            if(theType.Equals("System.UInt16"))
                return 2;
            if(theType.Equals("System.DateTime"))
                return 8;
            if(theType.Equals("System.String"))
                return 50;
            if(theType.Equals("System.Drawing.Image"))
                return 16;*/
            if (theType.Equals("System.String"))
                return 50;
            //			if(theType.Equals("System.Drawing.Image"))
            //				return 16;
            return 0;
        }
        /// <MetaDataID>{4941F389-ED0B-41B9-A6A1-BA94B140808F}</MetaDataID>
        static System.Globalization.CultureInfo EnglishCulture = new System.Globalization.CultureInfo("en-US");
        /// <MetaDataID>{99B382C5-5C97-419E-8543-70478AC9E872}</MetaDataID>
        public override string ConvertToSQLString(object value)
        {
            ///TODO Να γραφτεί test case για enum type 
            if (value == null)
                return "NULL";

            if (value is System.Boolean)
            {
                if ((System.Boolean)value)
                    return "1";
                else
                    return "0";
            }
            if (value is System.Byte)
                return value.ToString();
            if (value is System.Single)
                return value.ToString();
            if (value is System.Int32)
                return value.ToString();
            if (value is System.Int64)
                return value.ToString();
            if (value is System.SByte)
                return value.ToString();
            if (value is System.Int16)
                return value.ToString();
            if (value is System.UInt32)
                return value.ToString();
            if (value is System.UInt64)
                return value.ToString();
            if (value is System.UInt16)
                return value.ToString();
            if (value is string)
                return "'" + value.ToString() + "'";
            if (value is System.DateTime)
            {
                string dateTimeString = ((System.DateTime)value).ToString("u");
                dateTimeString = dateTimeString.Replace("Z", "");
                return "CONVERT(DATETIME, '" + dateTimeString + "',102)";
            }
            if (value is System.Double)
                return ((System.Double)value).ToString(EnglishCulture);
            if (value is System.Char)
                return "'" + value.ToString() + "'";

            if (value is System.Guid)
                return "'" + value.ToString() + "'";

            if (value is System.Decimal)
                return ((System.Decimal)value).ToString(EnglishCulture);
            return value.ToString();
        }

        /// <MetaDataID>{66850FBD-8D22-403D-80C1-FFEAFC6D1271}</MetaDataID>
        public override System.Type GetDotNetType(string theType)
        {
            if (theType.Equals("System.Guid"))
                return typeof(System.Guid);
            if (theType.Equals("System.Boolean"))
                return typeof(System.Boolean);
            if (theType.Equals("System.Byte"))
                return typeof(System.Byte);
            if (theType.Equals("System.Char"))
                return typeof(System.Char);
            if (theType.Equals("System.Decimal"))
                return typeof(System.Decimal);
            if (theType.Equals("System.Double"))
                return typeof(System.Double);
            if (theType.Equals("System.Single"))
                return typeof(System.Single);
            if (theType.Equals("System.Int32"))
                return typeof(System.Int32);
            if (theType.Equals("System.Int64"))
                return typeof(System.Int64);
            if (theType.Equals("System.SByte"))
                return typeof(System.SByte);
            if (theType.Equals("System.Int16"))
                return typeof(System.Int16);
            if (theType.Equals("System.UInt32"))
                return typeof(System.UInt32);
            if (theType.Equals("System.UInt64"))
                return typeof(System.UInt64);
            if (theType.Equals("System.UInt16"))
                return typeof(System.UInt16);
            if (theType.Equals("System.DateTime"))
                return typeof(System.DateTime);
            if (theType.Equals("System.String"))
                return typeof(System.String);
#if DeviceDotNet
            if (theType.Equals("Xamarin.Forms.Image"))
                return typeof(Xamarin.Forms.Image);
#else
            if (theType.Equals("System.Drawing.Image"))
                return typeof(System.Drawing.Image);
#endif
            return null;
        }


        /// <MetaDataID>{A7BA2B39-69DB-4FDE-8F0D-9117339A4267}</MetaDataID>
        public override string GetDBType(string theType, bool FixLength)
        {

            if (theType.Equals("System.Guid"))
                return "varchar";
            if (theType.Equals("System.Boolean"))
                return "bit";
            if (theType.Equals("System.Byte"))
                return "char";
            if (theType.Equals("System.Char"))
                return "char";
            if (theType.Equals("System.Decimal"))
                return "decimal";
            if (theType.Equals("System.Double"))
                return "float";
            if (theType.Equals("System.Single"))
                return "float";
            if (theType.Equals("System.Int32"))
                return "int";
            if (theType.Equals("System.Int64"))
                return "bigint";
            if (theType.Equals("System.SByte"))
                return "char";
            if (theType.Equals("System.Int16"))
                return "smallint";
            if (theType.Equals("System.UInt32"))
                return "int";
            if (theType.Equals("System.UInt64"))
                return "bigint";
            if (theType.Equals("System.UInt16"))
                return "smallint";
            if (theType.Equals("System.DateTime"))
                return "datetime";
            if (theType.Equals("System.String") && FixLength)
                return "nvarchar";
            if (theType.Equals("System.String") && (!FixLength))
                return "nvarchar";
            if (theType.Equals("System.Drawing.Image"))
                return "image";
            if (theType.Equals("enum"))
                return "int";


            return null;
        }

        public override object Convert(object value, System.Type type)
        {

            if (value == null)
                return null;
            else
                if (value.GetType() != type)
                {
                    if (type == typeof(System.Guid))
                    {
                        return new System.Guid(value as string);
                    }
                    else if (type.GetMetaData().BaseType == typeof(System.Enum))
                    {
                        //TODO Κατι πρέπει να γίνει στην περίπτωση που το value τις data base είναι out of range 
                        return System.Enum.ToObject(type, (int)System.Convert.ChangeType(value, typeof(int), null));
                    }
                    else if (type == typeof(System.DateTime))
                    {
                        return new DateTime((long)value);
                    }
                    else return System.Convert.ChangeType(value, type, null);
                }
                else
                    return value;
        }
    
    }
}
