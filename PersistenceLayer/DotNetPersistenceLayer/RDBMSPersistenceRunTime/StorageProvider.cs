using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime
{
    /// <MetaDataID>{7593ab84-b8ee-4a2d-8c4e-f369c296735f}</MetaDataID>
    public abstract class StorageProvider : PersistenceLayerRunTime.StorageProvider
    {
        /// <MetaDataID>{1848138f-47cf-4d5f-8083-951056fe097d}</MetaDataID>
        public abstract RDBMSDataObjects.DataBase GetDataBase(string connectionString);
    }
}
