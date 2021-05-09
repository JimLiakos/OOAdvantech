namespace OOAdvantech.RDBMSPersistenceRunTime
{
    using System;

    /// <MetaDataID>{6C5A1B95-909E-4990-896B-B36ABEC9A503}</MetaDataID>
    [MetaDataRepository.BackwardCompatibilityID("{6C5A1B95-909E-4990-896B-B36ABEC9A503}")]
    [Serializable()]
    public class ObjectID : PersistenceLayer.ObjectID
    {

        /// <MetaDataID>{feabae74-102f-4685-ba0f-a73caf548f2a}</MetaDataID>
         ObjectID()
        {
        }

        /// <MetaDataID>{1E5807A9-A52F-41EA-A130-8DC2AB24C13D}</MetaDataID>
        public ObjectID(Guid IntObjIDValue, int ObjCellIDValue)
            : base(IntObjIDValue, ObjCellIDValue)
        {

            
        }


        /// <MetaDataID>{4ac076b1-ddb3-4a01-ab37-edfd3c6c07f5}</MetaDataID>
        public ObjectID(OOAdvantech.MetaDataRepository.ObjectIdentityType objectIdentityType, object[] objectIDPartsValues, int ObjCellIDValue)
            :base(objectIdentityType, objectIDPartsValues, ObjCellIDValue)
        {
        }

    }
}
