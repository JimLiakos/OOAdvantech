namespace OOAdvantech.RDBMSPersistenceRunTime
{
	/// <MetaDataID>{AC07578E-9A78-4FF9-9730-2A38CD29A116}</MetaDataID>
	[MetaDataRepository.BackwardCompatibilityID("{AC07578E-9A78-4FF9-9730-2A38CD29A116}")]
	public class TypeDictionary
	{
		/// <MetaDataID>{4F476658-37A3-4F06-B541-A158BC81C0A3}</MetaDataID>
		public static string GetDBType(string theType)
		{
			return GetDBType(theType,true);
		}

		/// <MetaDataID>{AA1AF657-71C2-4C5F-89BC-44A54A9E4500}</MetaDataID>
		public static object GetNullValue(string theType)
		{
			if(theType.Equals("System.Boolean"))
				return false;
			if(theType.Equals("System.Byte"))
				return 0;
			if(theType.Equals("System.Char"))
				return ' ';
			if(theType.Equals("System.Decimal"))
				return 0;
			if(theType.Equals("System.Double"))
				return 0;
			if(theType.Equals("System.Single"))
				return 0;
			if(theType.Equals("System.Int32"))
				return 0;
			if(theType.Equals("System.Int64"))
				return 0;
			if(theType.Equals("System.SByte"))
				return 0;
			if(theType.Equals("System.Int16"))
				return 0;
			if(theType.Equals("System.UInt32"))
				return 0;
			if(theType.Equals("System.UInt64"))
				return 0;
			if(theType.Equals("System.UInt16"))
				return 0;
			if(theType.Equals("System.DateTime"))
				return System.DateTime.Now;
			if(theType.Equals("System.String"))
				return "";
			if(theType.Equals("System.Drawing.Image"))
				return System.DBNull.Value;

            if (theType.Equals("enum"))
                return System.DBNull.Value;

			
			if(theType.Equals("System.Guid"))
				return System.Guid.Empty;
				
			return null;
		}
		public static string GetDBNullValue(string theType)
		{
			if(theType.Equals("System.Boolean"))
				return "NULL";
			if(theType.Equals("System.Byte"))
				return "NULL";
			if(theType.Equals("System.Char"))
				return "NULL";
			if(theType.Equals("System.Decimal"))
				return "NULL";
			if(theType.Equals("System.Double"))
				return "NULL";
			if(theType.Equals("System.Single"))
				return "NULL";
			if(theType.Equals("System.Int32"))
				return "NULL";
			if(theType.Equals("System.Int64"))
				return "NULL";
			if(theType.Equals("System.SByte"))
				return "NULL";
			if(theType.Equals("System.Int16"))
				return "NULL";
			if(theType.Equals("System.UInt32"))
				return "NULL";
			if(theType.Equals("System.UInt64"))
				return "NULL";
			if(theType.Equals("System.UInt16"))
				return "NULL";
			if(theType.Equals("System.DateTime"))
				return "NULL";
			if(theType.Equals("System.String"))
				return "NULL";
			if(theType.Equals("System.Drawing.Image"))
				return "NULL";
			if(theType.Equals("System.Guid"))
                return "NULL";
				//return "CONVERT(uniqueidentifier, '"+System.Guid.Empty.ToString()+"')"; 
			return null;
		}

		/// <MetaDataID>{1ED248FD-0D6C-40C9-9C00-9B5B6E780685}</MetaDataID>
		public static bool IsTypeVarLength(string theType)
		{
			if(theType.Equals("System.Boolean"))
				return false;
			if(theType.Equals("System.Byte"))
				return false;
			if(theType.Equals("System.Char"))
				return false;
			if(theType.Equals("System.Decimal"))
				return false;
			if(theType.Equals("System.Double"))
				return false;
			if(theType.Equals("System.Single"))
				return false;
			if(theType.Equals("System.Int32"))
				return false;
			if(theType.Equals("System.Int64"))
				return false;
			if(theType.Equals("System.SByte"))
				return false;
			if(theType.Equals("System.Int16"))
				return false;
			if(theType.Equals("System.UInt32"))
				return false;
			if(theType.Equals("System.UInt64"))
				return false;
			if(theType.Equals("System.UInt16"))
				return false;
			if(theType.Equals("System.DateTime"))
				return false;
			if(theType.Equals("System.String"))
				return true;
			if(theType.Equals("System.Drawing.Image"))
				return false;

			return false;


		}
		/// <MetaDataID>{365EA3BB-7772-43DF-BDB0-D27F232CCF2E}</MetaDataID>
		public static int GeDefaultLength(string theType)
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
			if(theType.Equals("System.String"))
				return 50;
//			if(theType.Equals("System.Drawing.Image"))
//				return 16;
			return 0;
		}
		/// <MetaDataID>{4941F389-ED0B-41B9-A6A1-BA94B140808F}</MetaDataID>
		static System.Globalization.CultureInfo EnglishCulture=new System.Globalization.CultureInfo("en-US");
		/// <MetaDataID>{99B382C5-5C97-419E-8543-70478AC9E872}</MetaDataID>
		public static string ConvertToSQLString(object value)
		{
            ///TODO Να γραφτεί test case για enum type 
			if(value==null)
				return "NULL";

            if (value is System.Boolean)
            {
                if ((System.Boolean)value)
                    return "1";
                else
                    return "0";
            }
			if(value is System.Byte)
				return value.ToString();
			if(value is System.Single)
				return value.ToString();
			if(value is System.Int32)
				return value.ToString();
			if(value is System.Int64)
				return value.ToString();
			if(value is System.SByte)
				return value.ToString();
			if(value is System.Int16)
				return value.ToString();
			if(value is System.UInt32)
				return value.ToString();
			if(value is System.UInt64)
				return value.ToString();
			if(value is System.UInt16)
				return value.ToString();
			if(value is string)
				return "'"+value.ToString()+"'";
			if(value is System.DateTime)
			{
				string dateTimeString= ((System.DateTime)value).ToString("u");
				dateTimeString=dateTimeString.Replace("Z","");
				return "CONVERT(DATETIME, '"+dateTimeString+"',102)";
			}
			if(value is System.Double)
				return ((System.Double)value).ToString(EnglishCulture);
			if(value is System.Char)
				return "'"+value.ToString()+"'";

			if(value is System.Guid)
				return "'"+value.ToString()+"'";

			if(value is System.Decimal)
				return ((System.Decimal)value).ToString(EnglishCulture);
			return value.ToString();
		}

		/// <MetaDataID>{66850FBD-8D22-403D-80C1-FFEAFC6D1271}</MetaDataID>
		public static System.Type GetDotNetType(string theType)
		{
			if(theType.Equals("System.Guid"))
				return typeof(System.Guid);
			if(theType.Equals("System.Boolean"))
				return typeof(System.Boolean);
			if(theType.Equals("System.Byte"))
				return typeof(System.Byte);
			if(theType.Equals("System.Char"))
				return typeof(System.Char);
			if(theType.Equals("System.Decimal"))
				return typeof(System.Decimal);
			if(theType.Equals("System.Double"))
				return typeof(System.Double);
			if(theType.Equals("System.Single"))
				return typeof(System.Single);
			if(theType.Equals("System.Int32"))
				return typeof(System.Int32);
			if(theType.Equals("System.Int64"))
				return typeof(System.Int64);
			if(theType.Equals("System.SByte"))
				return typeof(System.SByte);
			if(theType.Equals("System.Int16"))
				return typeof(System.Int16);
			if(theType.Equals("System.UInt32"))
				return typeof(System.UInt32);
			if(theType.Equals("System.UInt64"))
				return typeof(System.UInt64);
			if(theType.Equals("System.UInt16"))
				return typeof(System.UInt16);
			if(theType.Equals("System.DateTime"))
				return typeof(System.DateTime);
			if(theType.Equals("System.String"))
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
		public static string GetDBType(string theType,bool FixLength)
		{
			
			if(theType.Equals("System.Guid"))
				return "uniqueidentifier";
			if(theType.Equals("System.Boolean"))
				return "bit";
			if(theType.Equals("System.Byte"))
				return "char";
			if(theType.Equals("System.Char"))
				return "char";
			if(theType.Equals("System.Decimal"))
				return "decimal";
			if(theType.Equals("System.Double"))
				return "float";
			if(theType.Equals("System.Single"))
				return "float";
			if(theType.Equals("System.Int32"))
				return "int";
			if(theType.Equals("System.Int64"))
				return "bigint";
			if(theType.Equals("System.SByte"))
				return "char";
			if(theType.Equals("System.Int16"))
				return "smallint";
			if(theType.Equals("System.UInt32"))
				return "int";
			if(theType.Equals("System.UInt64"))
				return "bigint";
			if(theType.Equals("System.UInt16"))
				return "smallint";
			if(theType.Equals("System.DateTime"))
				return "datetime";
			if(theType.Equals("System.String")&&FixLength)
				return "nvarchar";
			if(theType.Equals("System.String")&&(!FixLength))
				return "nvarchar";
			if(theType.Equals("System.Drawing.Image"))
				return "image";
            if(theType.Equals("enum"))
				return "int";

            
			return null;
		}

        public static object Convert(object value,System.Type type)
        {

            if (value == null)
                return null;
            else
                if (value.GetType() != type)
                {
                    if (type.GetMetaData().BaseType == typeof(System.Enum))
                    {
                        //TODO Κατι πρέπει να γίνει στην περίπτωση που το value τις data base είναι out of range 
                        return System.Enum.GetValues(type).GetValue((int)System.Convert.ChangeType(value,typeof(int)));
                    }
                    else
                        return System.Convert.ChangeType(value, type);
                }
                else
                    return value;

            
        }
    }
}
