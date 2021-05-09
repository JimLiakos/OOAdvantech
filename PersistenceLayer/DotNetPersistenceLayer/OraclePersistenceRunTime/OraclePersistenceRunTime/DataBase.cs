using OOAdvantech.RDBMSDataObjects;
namespace OOAdvantech.OraclePersistenceRunTime.DataObjects
{

    /// <MetaDataID>{60986a0c-feca-4a96-9d4c-9115b67b3eca}</MetaDataID>
    public class DataBase : OOAdvantech.RDBMSDataObjects.DataBase
    {
        public DataBase(RDBMSMetaDataRepository.Storage theObjectStorage, string name)
            : base(name)
        {
            _RDBMSSchema = new OOAdvantech.OraclePersistenceRunTime.OracleRDBMSSchema(this);
            Storage = theObjectStorage;
        }
        public DataBase(string name)
            : base(name)
        {
            _RDBMSSchema = new OOAdvantech.OraclePersistenceRunTime.OracleRDBMSSchema(this);
        }
        protected DataBase()
        {
        }
        public DataBase(string name, OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection connection)
            : base(name, connection)
        {
            _RDBMSSchema = new OOAdvantech.OraclePersistenceRunTime.OracleRDBMSSchema(this);
        }
        OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator _RDBMSSchema;
        public override IRDBMSSQLScriptGenarator RDBMSSQLScriptGenarator
        {
            get { return _RDBMSSchema; }
        }
    }

 
}


