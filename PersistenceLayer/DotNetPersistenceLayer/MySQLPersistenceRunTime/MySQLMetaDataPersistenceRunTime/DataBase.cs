using OOAdvantech.RDBMSDataObjects;
namespace OOAdvantech.MSSQLPersistenceRunTime.DataObjects
{
    /// <MetaDataID>{95df41b6-e3cc-448a-804e-c01cb5decc9a}</MetaDataID>
    public class DataBase : OOAdvantech.RDBMSDataObjects.DataBase
    {
        public DataBase(RDBMSMetaDataRepository.Storage theObjectStorage, string name)
            : base(name)
        {
            _RDBMSSchema = new OOAdvantech.MSSQLPersistenceRunTime.MSSQLRDBMSSchema(this);
            Storage = theObjectStorage;
        }
        public DataBase(string name)
            : base(name)
        {
            _RDBMSSchema = new OOAdvantech.MSSQLPersistenceRunTime.MSSQLRDBMSSchema(this);
        }
        protected DataBase()
        {
        }
        OOAdvantech.RDBMSDataObjects.IRDBMSSchema _RDBMSSchema;
        public override OOAdvantech.RDBMSDataObjects.IRDBMSSchema RDBMSSchema
        {
            get
            {
                return _RDBMSSchema;
            }
        }
    }

 
}


