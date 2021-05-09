using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.RDBMSPersistenceRunTime
{
  //  using ObjectStorage = PersistenceLayerRunTime.ObjectStorage;
    /// <MetaDataID>{fd1f9d08-ee07-4ab2-b363-c22f62bb5f18}</MetaDataID>
    public class ClassMemoryInstanceCollection : PersistenceLayerRunTime.ClassMemoryInstanceCollection
    {
        /// <MetaDataID>{12C4AE8E-7E6E-414C-9868-C34B4924E1B1}</MetaDataID>
        public ClassMemoryInstanceCollection(System.Type theDotNetMetadata, PersistenceLayerRunTime.ObjectStorage theOwnerStorageSession)
            : base(theDotNetMetadata, theOwnerStorageSession)
        {
             
            ObjectStorage objectStorage = theOwnerStorageSession as ObjectStorage;
        }

        ///// <MetaDataID>{56bda053-6846-46eb-b7cc-54a30bfcd74f}</MetaDataID>
        //void AssignRDBMSAttributeAsExtensionMetaObjects(DotNetMetaDataRepository.Structure structure, ObjectStorage objectStorage)
        //{
        //    foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
        //    {
        //        RDBMSMetaDataRepository.Attribute rdbmsAttribute = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attribute) as RDBMSMetaDataRepository.Attribute;
        //        if (rdbmsAttribute == null && structure.IsPersistent(attribute))
        //            throw new System.Exception("Problem with mapping of " + structure.Name + " class.");

        //        if (rdbmsAttribute != null)
        //            attribute.AddExtensionMetaObject(rdbmsAttribute);

        //        if (attribute.Type is DotNetMetaDataRepository.Structure &&
        //            structure.IsPersistent(attribute) &&
        //            (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
        //        {
        //            AssignRDBMSAttributeAsExtensionMetaObjects(attribute.Type as DotNetMetaDataRepository.Structure, objectStorage);
        //        }
        //    }
        //}
    }
}
