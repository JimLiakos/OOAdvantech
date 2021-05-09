using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime.Commands
{

    /// <summary>Produced any time we call the NewObject method of storage session. 
    /// Its work is to produce one or more records at mapping tables. 
    /// The new records host the state of object.
    /// To do that uses a store procedure with name new_xxxx_instance. </summary>
    /// <MetaDataID>{fa0c2d7e-8047-495d-a775-f6260ab7754a}</MetaDataID>
    public class NewStorageInstanceCommand : PersistenceLayerRunTime.Commands.NewStorageInstanceCommand
    {




        public override void Execute()
        {

            OOAdvantech.RDBMSPersistenceRunTime.ObjectID objectID = new OOAdvantech.RDBMSPersistenceRunTime.ObjectID();
            objectID.SetMemberValue("ObjectID", Guid.NewGuid());
            Commands.UpdateDatabaseMassivelly.CurrentTransactionCommandUpdateMassivelly.NewStorageInstance(OnFlyStorageInstance, objectID);
            OnFlyStorageInstance.ObjectID = objectID;
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (OnFlyStorageInstance.ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(OnFlyStorageInstance.Class) as RDBMSMetaDataRepository.Class;
            ((StorageInstanceRef)OnFlyStorageInstance).StorageInstanceSet = rdbmsMetadataClass.ActiveStorageCell;

            //ObjectStorage objectStorage = (ObjectStorage)OnFlyStorageInstance.ObjectStorage;
            //RDBMSMetaDataRepository.Class rdbmsMetadataClass = (objectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(OnFlyStorageInstance.Class) as RDBMSMetaDataRepository.Class;
            
            //throw new NotImplementedException();
        }

        public NewStorageInstanceCommand(StorageInstanceRef storageInstanceRef)
            : base(storageInstanceRef)
        {

        }
    }
}
