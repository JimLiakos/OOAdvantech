using System;
using System.Collections.Generic;

using System.Text;
using OOAdvantech.Transactions;

namespace OOAdvantech.RDBMSMetaDataRepository
{

    /// <MetaDataID>{7682C561-2A1A-41f7-A9F8-25A364D7BBDD}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{7682C561-2A1A-41f7-A9F8-25A364D7BBDD}")]
    [MetaDataRepository.Persistent()]
    public class DataBaseConnection
    {
        /// <exclude>Excluded</exclude>
        ObjectStateManagerLink Properties;
        /// <exclude>Excluded</exclude>
        string _ConnectionString;
        /// <MetaDataID>{141a02a5-8be4-41fd-9354-3ad84819ab64}</MetaDataID>
        [MetaDataRepository.PersistentMember("_ConnectionString")]
        [MetaDataRepository.BackwardCompatibilityID("+2")]
        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                if (_ConnectionString != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ConnectionString = value; 
                        stateTransition.Consistent = true;
                    }
        
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _RDBMSDataBaseType;
        /// <MetaDataID>{345b1db3-db04-49e3-9f36-fe2d325a6eb3}</MetaDataID>
        [MetaDataRepository.PersistentMember("_RDBMSDataBaseType"), MetaDataRepository.BackwardCompatibilityID("+1")]
        public string RDBMSDataBaseType 
        { 
            get
            {
                return _RDBMSDataBaseType;
            }
            set
            {
                if (_RDBMSDataBaseType != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RDBMSDataBaseType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}
