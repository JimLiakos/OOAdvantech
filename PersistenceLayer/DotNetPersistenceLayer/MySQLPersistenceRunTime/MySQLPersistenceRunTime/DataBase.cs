using OOAdvantech.RDBMSDataObjects;
namespace OOAdvantech.MySQLPersistenceRunTime.DataObjects
{

    /// <MetaDataID>{c528bb87-368a-44e1-9f11-44e25adb5fd2}</MetaDataID>
    public class DataBase : OOAdvantech.RDBMSDataObjects.DataBase
    {
        public DataBase(RDBMSMetaDataRepository.Storage theObjectStorage, string name)
            : base(name)
        {
            _RDBMSSchema = new OOAdvantech.MySQLPersistenceRunTime.MySQLRDBMSSchema(this);
            Storage = theObjectStorage;
        }
        public DataBase(string name)
            : base(name)
        {
            _RDBMSSchema = new OOAdvantech.MySQLPersistenceRunTime.MySQLRDBMSSchema(this);
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


