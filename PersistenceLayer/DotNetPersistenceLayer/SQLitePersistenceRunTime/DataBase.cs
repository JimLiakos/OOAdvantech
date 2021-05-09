using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.SQLitePersistenceRunTime
{
    /// <MetaDataID>{6f6efcf2-dc4b-4a8b-aa48-3602c9dfcd14}</MetaDataID>
    public class DataBase : OOAdvantech.RDBMSDataObjects.DataBase
    {
        /// <MetaDataID>{519124ef-0c8c-4dcc-b73b-342d3b2f19b4}</MetaDataID>
        public DataBase(RDBMSMetaDataRepository.Storage theObjectStorage, string name)
            : base(name)
        {
            _RDBMSSchema = new OOAdvantech.SQLitePersistenceRunTime.SQLiteRDBMSSchema(this);
            Storage = theObjectStorage;
        }
        /// <MetaDataID>{95e24453-5645-4bd6-947d-88cefb4e630b}</MetaDataID>
        public DataBase(string name)
            : base(name)
        {
            _RDBMSSchema = new OOAdvantech.SQLitePersistenceRunTime.SQLiteRDBMSSchema(this);
        }
        /// <MetaDataID>{f683a7da-bfe9-44ca-abd4-9dae92653da0}</MetaDataID>
        public DataBase(string name, OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection connection)
            : base(name, connection)
        {
            _RDBMSSchema = new OOAdvantech.SQLitePersistenceRunTime.SQLiteRDBMSSchema(this);
        }
        /// <MetaDataID>{9912c4cd-3cdd-4163-9bb7-f9996dddce76}</MetaDataID>
        protected DataBase()
        {
        }
        /// <MetaDataID>{1ef83ae2-895b-4bca-8b0b-763e65cf467b}</MetaDataID>
        OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator _RDBMSSchema;
        /// <MetaDataID>{20c5620c-63fe-47d9-8b0c-74af83ddb6b4}</MetaDataID>
        public override OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator RDBMSSQLScriptGenarator
        {
            get
            {
                return _RDBMSSchema;
            }
        }
        /// <MetaDataID>{c04af787-f05e-4f1d-aeea-ad850b3ea3e5}</MetaDataID>
        OOAdvantech.RDBMSMetaDataPersistenceRunTime.TypeDictionary _TypeDictionary = new SQLiteMetaDataPersistenceRunTime.TypeDictionary();
        /// <MetaDataID>{ff39353c-d55b-448b-aacb-4fb393209150}</MetaDataID>
        public override OOAdvantech.RDBMSMetaDataPersistenceRunTime.TypeDictionary TypeDictionary
        {
            get
            {
                return _TypeDictionary;
            }
        }
    }

}
