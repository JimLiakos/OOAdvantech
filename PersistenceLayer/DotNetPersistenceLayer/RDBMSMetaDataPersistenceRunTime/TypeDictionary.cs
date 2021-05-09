using System;
using System.Collections.Generic;
using System.Text;


namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime
{
    /// <MetaDataID>{b006811d-8176-4343-b900-71c2609d8492}</MetaDataID>
    public abstract class TypeDictionary
    {
        /// <MetaDataID>{a4def20b-3521-48ab-a40b-e0c1ea55b670}</MetaDataID>
        public abstract string GetDBDefaultValue(string theType);
        /// <MetaDataID>{2f652363-12a1-4f74-ae4e-078f74596b66}</MetaDataID>
        public abstract string GetDBType(string theType);
        /// <MetaDataID>{a9714082-c6d2-486d-948d-4dfd4681bb8c}</MetaDataID>
        public abstract object GetNullValue(string theType);
        /// <MetaDataID>{88e70666-d58d-4b17-bae3-6d51dbe8d34a}</MetaDataID>
        public abstract string GetDBNullScript(string theType);
        /// <MetaDataID>{ed780ad9-3ee8-45b4-9545-868ee0468beb}</MetaDataID>
        public abstract bool IsTypeVarLength(string theType);
        /// <MetaDataID>{6704f17f-2cd1-4400-995c-83e2f46dfcc3}</MetaDataID>
        public abstract int GeDefaultLength(string theType);
        /// <MetaDataID>{e07f422e-51f1-4183-b078-8cdda2f1cd55}</MetaDataID>
        public abstract string ConvertToSQLString(object value);
        /// <MetaDataID>{62d8b659-38d0-45e5-8ca7-3b70be46c51a}</MetaDataID>
        public abstract System.Type GetDotNetType(string theType);
        /// <MetaDataID>{643b9626-b272-497c-b4bc-1c47eed6e545}</MetaDataID>
        public abstract System.Type GetDataTransferDotNetType(System.Type type);
        /// <MetaDataID>{71044884-d9bd-44c0-bcc8-b98d32d378f9}</MetaDataID>
        public abstract string GetDBType(string theType, bool FixLength);
        /// <MetaDataID>{82664284-8e70-4c5b-84cd-520be8698284}</MetaDataID>
        public abstract object Convert(object value, System.Type type);
        /// <MetaDataID>{b2db0e72-1c41-4d10-acc7-4d2d6403b2f7}</MetaDataID>
        public abstract System.Type GetTypeForDBType(string dataType);


        /// <MetaDataID>{4b5fe83f-cd13-41ef-b6b5-c37e70e55103}</MetaDataID>
        public virtual DbType ToDbType(System.Type type)
        {
            return (DbType)(int)TypeConvertor.ToDbType(type);
        }
#if !DeviceDotNet
        #endif
    }



    /// <summary>
    /// Convert a base data type to another base data type
    /// </summary>
    /// <MetaDataID>{11cfa798-cbad-4ab8-9ba4-7e646902c911}</MetaDataID>
    public sealed class TypeConvertor
    {
        private struct DbTypeMapEntry
        {
            public Type Type;
            public DbType DbType;
            public System.Data.SqlDbType SqlDbType;
            public DbTypeMapEntry(Type type, DbType dbType, System.Data.SqlDbType sqlDbType)
            {
                this.Type = type;
                this.DbType = dbType;
                this.SqlDbType = sqlDbType;
            }
        };
        private static List<DbTypeMapEntry> _DbTypeList = new List<DbTypeMapEntry>();
#region Constructors
        static TypeConvertor()
        {
            DbTypeMapEntry dbTypeMapEntry = new DbTypeMapEntry(typeof(bool), DbType.Boolean, System.Data.SqlDbType.Bit);
            _DbTypeList.Add(dbTypeMapEntry);
            dbTypeMapEntry = new DbTypeMapEntry(typeof(byte), DbType.Double, System.Data.SqlDbType.TinyInt);
            _DbTypeList.Add(dbTypeMapEntry);
            dbTypeMapEntry = new DbTypeMapEntry(typeof(byte[]), DbType.Binary, System.Data.SqlDbType.Image);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, System.Data.SqlDbType.DateTime);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, System.Data.SqlDbType.Decimal);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(double), DbType.Double, System.Data.SqlDbType.Float);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(Guid), DbType.Guid, System.Data.SqlDbType.UniqueIdentifier);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(Int16), DbType.Int16, System.Data.SqlDbType.SmallInt);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(Int32), DbType.Int32, System.Data.SqlDbType.Int);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(Int64), DbType.Int64, System.Data.SqlDbType.BigInt);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(object), DbType.Object, System.Data.SqlDbType.Variant);
            _DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry = new DbTypeMapEntry(typeof(string), DbType.String, System.Data.SqlDbType.VarChar);
            _DbTypeList.Add(dbTypeMapEntry);
        }

        private TypeConvertor()
        {
        }

#endregion

#region Methods

        /// <summary>
        /// Convert db type to .Net data type
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static Type ToNetType(DbType dbType)
        {
            DbTypeMapEntry entry = Find(dbType);
            return entry.Type;
        }
        /// <summary>
        /// Convert TSQL type to .Net data type
        /// </summary>
        /// <param name="sqlDbType"></param>
        /// <returns></returns>
        public static Type ToNetType(System.Data.SqlDbType sqlDbType)
        {
            DbTypeMapEntry entry = Find(sqlDbType);
            return entry.Type;
        }

        /// <summary>
        /// Convert .Net type to Db type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DbType ToDbType(Type type)
        {
            if (type.FullName == "System.Drawing.Image")
                type = typeof(byte[]);

            DbTypeMapEntry entry = Find(type);
            return entry.DbType;
        }

        /// <summary>
        /// Convert TSQL data type to DbType
        /// </summary>
        /// <param name="sqlDbType"></param>
        /// <returns></returns>
        public static DbType ToDbType(System.Data.SqlDbType sqlDbType)
        {
            DbTypeMapEntry entry = Find(sqlDbType);
            return entry.DbType;
        }

        /// <summary>
        /// Convert .Net type to TSQL data type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static System.Data.SqlDbType ToSqlDbType(Type type)
        {
            DbTypeMapEntry entry = Find(type);
            return entry.SqlDbType;
        }

        /// <summary>
        /// Convert DbType type to TSQL data type
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static System.Data.SqlDbType ToSqlDbType(DbType dbType)
        {
            DbTypeMapEntry entry = Find(dbType);
            return entry.SqlDbType;
        }

        private static DbTypeMapEntry Find(Type type)
        {
            object retObj = null;
            for (int i = 0; i < _DbTypeList.Count; i++)
            {
                DbTypeMapEntry entry = _DbTypeList[i];
                if (entry.Type == type)
                {
                    retObj = entry;
                    break;
                }
            }
            if (retObj == null)
            {
                throw new Exception("Referenced an unsupported Type");
            }
            return (DbTypeMapEntry)retObj;
        }

        private static DbTypeMapEntry Find(DbType dbType)
        {
            object retObj = null;
            for (int i = 0; i < _DbTypeList.Count; i++)
            {
                DbTypeMapEntry entry = (DbTypeMapEntry)_DbTypeList[i];
                if (entry.DbType == dbType)
                {
                    retObj = entry;
                    break;
                }
            }
            if (retObj == null)
            {
                throw new Exception("Referenced an unsupported DbType");
            }
            return (DbTypeMapEntry)retObj;
        }

        private static DbTypeMapEntry Find(System.Data.SqlDbType sqlDbType)
        {
            object retObj = null;
            for (int i = 0; i < _DbTypeList.Count; i++)
            {
                DbTypeMapEntry entry = (DbTypeMapEntry)_DbTypeList[i];
                if (entry.SqlDbType == sqlDbType)
                {
                    retObj = entry;
                    break;
                }
            }
            if (retObj == null)
            {
                throw new Exception("Referenced an unsupported SqlDbType");
            }

            return (DbTypeMapEntry)retObj;
        }
#endregion
    }

#if !DeviceDotNet
#endif



}

