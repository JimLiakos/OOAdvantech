using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.PersistenceLayerΝ
{
    /// <MetaDataID>{01C52C0C-9652-421B-9057-034B4E4D0508}</MetaDataID>
    public interface IPersistencyService
    {
        
        /// <MetaDataID>{0E58439A-6DBB-4CDF-8414-4F02089240C3}</MetaDataID>
        /// <summary>Create a storage access session. </summary>
        /// <param name="storageName">The name of Object Storage </param>
        /// <param name="storageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). 
        /// If it is null then the PersistencyContext will look at Persistence Layer repository. </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider.
        /// If it is null then the PersistencyContext will look at Persistence Layer repository. </param>
        void DeleteStorage(string storageName, string storageLocation, string storageType);
        /// <MetaDataID>{AFE8CB63-4A49-4694-A793-558BA09F0110}</MetaDataID>
        bool ClassOfObjectIsPersistent(object memoryInstance);
        /// <summary>This method creates a new storage with name, type and location that defined from parameters StorageName, StorageType and StorageLocation.
        /// The schema of storage will be the same with the OriginalStorage. </summary>
        /// <param name="storageMetadata">Define the Meta data of new storage. 
        /// The Persistency Context at first time will be cloning the OriginalStorage Meta data.
        /// After that will be create the schema of new Storage. For example tables for relational database, DTD data for XML files et cetera. </param>
        /// <param name="storageName">Define the name of new storage. </param>
        /// <param name="storageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). The format of string depends from the type of new storage </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider. </param>
        /// <MetaDataID>{F97962CD-EB7A-41BA-8890-202B81F95C05}</MetaDataID>
        ΙObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage storageMetadata, string storageName, string storageLocation, string storageType, bool InProcess);
        /// <summary>Create a storage access session. </summary>
        /// <param name="storageName">The name of Object Storage </param>
        /// <param name="storageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). 
        /// If it is null then the PersistencyContext will look at Persistence Layer repository. </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider.
        /// If it is null then the PersistencyContext will look at Persistence Layer repository. </param>
        /// <MetaDataID>{C44681A9-F32D-4E66-9E21-1B73C40B1984}</MetaDataID>
        ΙObjectStorage OpenStorage(string storageName, string storageLocation, string storageType);

        /// <summary>This method creates a new storage with name, type and location that defined from parameters StorageName, StorageType and StorageLocation.
        /// The schema of storage will be the same with the OriginalStorage. </summary>
        /// <param name="storageName">Define the name of new storage. </param>
        /// <param name="storageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). The format of string depends from the type of new storage </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider. </param>
        /// <MetaDataID>{F97962CD-EB7A-41BA-8890-202B81F95C05}</MetaDataID>
        ΙObjectStorage NewLogicalStorage(string storageName, string storageLocation, string storageType, string logicalStorageName, bool InProcess);


        /// <summary>This method creates a new storage with name and type  that defined from parameters StorageName, StorageType on rowStoragedata.
        /// The schema of storage will be the same with the OriginalStorage. </summary>
        /// <param name="storageMetadata">Define the Meta data of new storage. 
        /// The Persistency Context at first time will be cloning the OriginalStorage Meta data.
        /// After that will be create the schema of new Storage. For example tables for relational database, DTD data for XML files et cetera. </param>
        /// <param name="storageName">Define the name of new storage. </param>
        /// <param name="rawStorageData">This parameter defines an object which contains the raw data for stored object.
        /// It can be an xml document a file or memory stream etc. the storage loaded in process by default. </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider. </param>
        /// <MetaDataID>{927E6236-0E37-44D6-AACE-108F63943F60}</MetaDataID>
        ΙObjectStorage NewStorage(OOAdvantech.PersistenceLayer.Storage storageMetadata, string storageName, object rawStorageData, string storageType);
        /// <summary>Opens a storage access session. </summary>
        /// <param name="rawStorageData">This parameter defines an object which contains the raw data for stored object.
        /// It can be an xml document a file or memory stream etc. the storage loaded in process by default. </param>
        /// <param name="storageLocation">This string used to specify the location of new storage. Maybe is a SQL server name or URL (Uniform Resource Locator). 
        /// If it is null then the PersistencyContext will look at Persistence Layer repository. </param>
        /// <param name="storageType">This parameter used to specify the type of new storage. Actual is the full name of Storage Provider for example MSSQLRunTime. MSSQLStorageProvider.
        /// If it is null then the PersistencyContext will look at Persistence Layer repository. </param>
        /// <MetaDataID>{FC1F995A-6A2C-45FE-89FA-55F08728C378}</MetaDataID>
        ΙObjectStorage OpenStorage(string storageName, object rawStorageData, string storageType);

        /// <MetaDataID>{64945209-A20D-479F-8D3C-BA8B9CB0BB5A}</MetaDataID>
        ΙObjectStorage GetStorageOfObject(object memoryInstance);


        /// <MetaDataID>{fd19403c-064e-463e-be66-e03d437dbe49}</MetaDataID>
        bool IsPersistent(object memoryInstance);

        /// <MetaDataID>{6f598b8d-18e0-4b1a-9aac-d292f860c664}</MetaDataID>
        void GetStorageInstancePersistentObjectID(object _object, out IObjectID objectID, out string storageIdentity);

        /// <MetaDataID>{9551ef72-926c-45ce-a5c5-d1917552eaa6}</MetaDataID>
        void GetStorageInstanceObjectID(object _object, out IObjectID transientObjectID, out string storageIdentity);


        /// <MetaDataID>{ae2ff489-ddc1-4e76-b72a-b6040c961e08}</MetaDataID>
        //OOAdvantech.MetaDataRepository.StorageServer GetStorageServer(string storageServerLocation);


        /// <MetaDataID>{d3136ed2-77cd-4b35-9d66-8fa5266e067f}</MetaDataID>
        ΙObjectStorage NewLogicalStorage(ΙObjectStorage hostingStorage, string logicalStorageName);

    }
}
