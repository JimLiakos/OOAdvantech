namespace OOAdvantech.OracleMetaDataPersistenceRunTime
{

    /// <MetaDataID>{79e5e686-ff50-429b-9015-b6535825a8be}</MetaDataID>
    public class TypeDictionary : RDBMSMetaDataPersistenceRunTime.TypeDictionary
    {
        public override OOAdvantech.RDBMSMetaDataPersistenceRunTime.DbType ToDbType(System.Type type)
        {
            if (type == typeof(System.Guid))
                type = typeof(byte[]);
            if (type == typeof(bool))
                type= typeof(string);
            if (type == typeof(char))
                type = typeof(string);
            if (type.BaseType == typeof(System.Enum))
                type = typeof(int);
            return base.ToDbType(type);
        }
        public override System.Type GetTypeForDBType(string dataType)
        {
            throw new System.NotImplementedException();
        }
        public override System.Type GetDataTransferDotNetType(System.Type type)
        {
            if (type == typeof(System.Guid))
                return typeof(byte[]);

            if (type == typeof(bool))
                return typeof(char);
            return type;
        }
        public override string GetDBType(string theType)
        {
            return GetDBType(theType, true);
        }



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
                return "NULL";
            return null;
        }

        public override string GetDBDefaultValue(string theType)
        {
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
                return "NULL";
            return null;
        }


        public override bool IsTypeVarLength(string theType)
        {
            if (theType.Equals("System.Boolean"))
                return true;
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
            if (theType.Equals("System.Guid"))
                return true;

            if (theType.Equals("System.Drawing.Image"))
                return false;

            return false;


        }

        public override int GeDefaultLength(string theType)
        {
            /*
            if(theType.Equals("System.Boolean"))
                return 1;
            if(theType.Equals("System.Byte"))
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

            if (theType.Equals("System.Guid"))
                return 16;

            if (theType.Equals("System.Boolean"))
                return 1;




            //			if(theType.Equals("System.Drawing.Image"))
            //				return 16;
            return 0;
        }

        static System.Globalization.CultureInfo EnglishCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

        public override string ConvertToSQLString(object value)
        {
            ///TODO Να γραφτεί test case για enum type 
            if (value == null)
                return "NULL";

            if (value is System.Boolean)
            {
                if ((System.Boolean)value)
                    return "'Y'";
                else
                    return "'N'";
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
                if ((System.DateTime)value >Oracle.DataAccess.Types.OracleDate.MaxValue.Value || (System.DateTime)value < Oracle.DataAccess.Types.OracleDate.MinValue.Value)
                    throw new System.Exception(string.Format("Date time value  ({0})  is out of Oracle Server datetime values range", value.ToString()));
   
                ///TODO Σαφώς δεν δουλεύει να ελεγχθούν και τα milisecond;
                string dateTimeString = ((System.DateTime)value).ToString("u");
                dateTimeString = dateTimeString.Replace("Z", "");
                return "CONVERT(DATETIME, '" + dateTimeString + "',102)";
            }
            if (value is System.Double)
                return ((System.Double)value).ToString(EnglishCulture);
            if (value is System.Char)
                return "'" + value.ToString() + "'";

            if (value is System.Guid)
            {
                string strValue = null;
                foreach(byte @byte in ((System.Guid)value).ToByteArray())
                {
                    strValue += @byte.ToString("X");
                }
                return "'" +strValue + "'";
            }

            if (value is System.Decimal)
                return ((System.Decimal)value).ToString(EnglishCulture);
            return value.ToString();
        }


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
            if (theType.Equals("System.Drawing.Image"))
                return typeof(System.Drawing.Image);
            return null;
        }



        public override string GetDBType(string theType, bool FixLength)
        {

            if (theType.Equals("System.Guid"))
                return "raw";
            if (theType.Equals("System.Boolean"))
                return "char";
            if (theType.Equals("System.Byte"))
                return "char";
            if (theType.Equals("System.Char"))
                return "char";
            if (theType.Equals("System.Decimal"))
                return "number";
            if (theType.Equals("System.Double"))
                return "float";
            if (theType.Equals("System.Single"))
                return "float";
            if (theType.Equals("System.Int32"))
                return "number";
            if (theType.Equals("System.Int64"))
                return "number";
            if (theType.Equals("System.SByte"))
                return "char";
            if (theType.Equals("System.Int16"))
                return "number";
            if (theType.Equals("System.UInt32"))
                return "number";
            if (theType.Equals("System.UInt64"))
                return "number";
            if (theType.Equals("System.UInt16"))
                return "number";
            if (theType.Equals("System.DateTime"))
                return "date";
            if (theType.Equals("System.String") && FixLength)
                return "nvarchar2";
            if (theType.Equals("System.String") && (!FixLength))
                return "nvarchar2";
            if (theType.Equals("System.Drawing.Image"))
                return "blob";
            if (theType.Equals("enum"))
                return "number";


            return null;
        }

        public override object Convert(object value, System.Type type)
        {
            if (value is System.DBNull)
                return OOAdvantech.AccessorBuilder.GetDefaultValue(type);

            if (type == typeof(System.Guid) && value is byte[])
                return new System.Guid(value as byte[]);
            if (type == typeof(bool) && value is char && ((char)value) == 'Y')
                return true;
            if (type == typeof(bool) && value is char && ((char)value) == 'N')
                return false;

            if (type == typeof(bool) && value is string && ((string)value) == "Y")
                return true;
            if (type == typeof(bool) && value is string && ((string)value) == "N")
                return false;

            if (type == typeof(byte[]) && value is System.Guid)
                return ((System.Guid)value).ToByteArray();
            if (type == typeof(char) && value is bool && ((bool)value))
                return 'Y';
            if (type == typeof(char) && value is bool && !((bool)value))
                return 'N';
            


            if (value == null)
                return null;
            else
                if (value.GetType() != type)
                {
                    if (type.BaseType == typeof(System.Enum))
                    {
                        if (typeof(System.DayOfWeek) == type)
                        {
                            int enumValue = (int)System.Convert.ChangeType(value, typeof(int)) ;
                            if (enumValue == 7)
                                enumValue = 0;
                            return System.Enum.GetValues(type).GetValue(enumValue);
                        }
                        else
                        {
                            //TODO Κατι πρέπει να γίνει στην περίπτωση που το value τις data base είναι out of range 
                            return System.Enum.GetValues(type).GetValue((int)System.Convert.ChangeType(value, typeof(int)));
                        }
                    }
                    else
                        return System.Convert.ChangeType(value, type);
                }
                else
                    return value;


        }
    }
}
