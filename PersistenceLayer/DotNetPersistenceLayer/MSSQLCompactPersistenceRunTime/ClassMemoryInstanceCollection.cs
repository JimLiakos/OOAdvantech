using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime
{
    /// <MetaDataID>{fd1f9d08-ee07-4ab2-b363-c22f62bb5f18}</MetaDataID>
    class ClassMemoryInstanceCollection : PersistenceLayerRunTime.ClassMemoryInstanceCollection
    {
        /// <MetaDataID>{12C4AE8E-7E6E-414C-9868-C34B4924E1B1}</MetaDataID>
        public ClassMemoryInstanceCollection(System.Type theDotNetMetadata, PersistenceLayerRunTime.ObjectStorage theOwnerStorageSession)
            : base(theDotNetMetadata, theOwnerStorageSession)
        {

            ObjectStorage objectStorage = theOwnerStorageSession as ObjectStorage;

            RDBMSMetaDataRepository.Class rdbmsMappingClass = (objectStorage.StorageMetaData as RDBMSMetaDataRepository. Storage).GetEquivalentMetaObject(Class) as RDBMSMetaDataRepository.Class;
            if (rdbmsMappingClass == null)
                throw new System.Exception("There isn't mapping data for class " + Class.Name + ".");
            ((DotNetMetaDataRepository.Class)Class).AddExtensionMetaObject(rdbmsMappingClass);

            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in Class.GetAssociateRoles(true))
            {
                if (associationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd)) == null)
                {
                    RDBMSMetaDataRepository.AssociationEnd rdbmsAssociationEnd = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(associationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                    if (rdbmsAssociationEnd == null)
                    {
                        //  throw new System.Exception("Problem with mapping of " + Class.Name + " class.");
                    }
                    else
                        associationEnd.AddExtensionMetaObject(rdbmsAssociationEnd);
                }
            }

            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in Class.GetRoles(true))
            {
                if (associationEnd.GetExtensionMetaObject(typeof(RDBMSMetaDataRepository.AssociationEnd)) == null)
                {
                    RDBMSMetaDataRepository.AssociationEnd rdbmsAssociationEnd = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(associationEnd) as RDBMSMetaDataRepository.AssociationEnd;
                    if (rdbmsAssociationEnd == null)
                    {
                        //throw new System.Exception("Problem with mapping of " + Class.Name + " class.");
                    }
                    else
                        associationEnd.AddExtensionMetaObject(rdbmsAssociationEnd);

                }
            }

            AssignRDBMSAttributeAsExtensionMetaObjects(Class, objectStorage);
       
        }
        /// <MetaDataID>{163d63fe-0010-4884-9021-d1aeea8e5704}</MetaDataID>
        void AssignRDBMSAttributeAsExtensionMetaObjects(DotNetMetaDataRepository.Class _class, ObjectStorage objectStorage)
        {
            foreach (DotNetMetaDataRepository.Attribute attribute in _class.GetAttributes(true))
            {
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attribute) as RDBMSMetaDataRepository.Attribute;
                if (rdbmsAttribute == null && _class.IsPersistent(attribute))
                    throw new System.Exception("Problem with mapping of " + _class.Name + " class.");

                if (rdbmsAttribute != null)
                    attribute.AddExtensionMetaObject(rdbmsAttribute);

                if (attribute.Type is DotNetMetaDataRepository.Structure &&
                    _class.IsPersistent(attribute) &&
                    (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {
                    AssignRDBMSAttributeAsExtensionMetaObjects(attribute.Type as DotNetMetaDataRepository.Structure, objectStorage);
                }
            }

        }
        /// <MetaDataID>{56bda053-6846-46eb-b7cc-54a30bfcd74f}</MetaDataID>
        void AssignRDBMSAttributeAsExtensionMetaObjects(DotNetMetaDataRepository.Structure structure, ObjectStorage objectStorage)
        {
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                RDBMSMetaDataRepository.Attribute rdbmsAttribute = (objectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(attribute) as RDBMSMetaDataRepository.Attribute;
                if (rdbmsAttribute == null && structure.IsPersistent(attribute))
                    throw new System.Exception("Problem with mapping of " + structure.Name + " class.");

                if (rdbmsAttribute != null)
                    attribute.AddExtensionMetaObject(rdbmsAttribute);

                if (attribute.Type is DotNetMetaDataRepository.Structure &&
                    structure.IsPersistent(attribute) &&
                    (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {
                    AssignRDBMSAttributeAsExtensionMetaObjects(attribute.Type as DotNetMetaDataRepository.Structure, objectStorage);
                }
            }
        }
    }
}
