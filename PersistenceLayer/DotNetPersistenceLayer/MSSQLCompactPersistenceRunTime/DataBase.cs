using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime.DataObjects
{
    /// <MetaDataID>{70ce603f-81b4-42c1-888f-b050f6ff06a8}</MetaDataID>
    class DataBase:OOAdvantech.RDBMSDataObjects.DataBase
    {
        public DataBase(RDBMSMetaDataRepository.Storage theObjectStorage, string name)
            : base( name )
        {
            _RDBMSSchema = new OOAdvantech.MSSQLCompactPersistenceRunTime.DataObjects.MSSQLCompactRDBMSSchema(this);
            Storage = theObjectStorage;
            
        }
        public DataBase(string name)
            : base(name)
        {
            _RDBMSSchema =new OOAdvantech.MSSQLCompactPersistenceRunTime.DataObjects.MSSQLCompactRDBMSSchema(this);

        }

        protected DataBase()
        {
        }
        OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator _RDBMSSchema;
        public override OOAdvantech.RDBMSDataObjects.IRDBMSSQLScriptGenarator RDBMSSQLScriptGenarator
        {
            get 
            {
                return _RDBMSSchema;
            }
        }
    }
    
}
