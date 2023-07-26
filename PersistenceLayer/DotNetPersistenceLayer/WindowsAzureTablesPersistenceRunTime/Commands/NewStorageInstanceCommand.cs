using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime.Commands
{

    /// <summary>Produced any time we call the NewObject method of storage session. 
    /// Its work is to produce one or more records at mapping tables. 
    /// The new records host the state of object.
    /// To do that uses a store procedure with name new_xxxx_instance. </summary>
    /// <MetaDataID>{1d88bf89-9d7c-4811-86bb-9fc0e43df417}</MetaDataID>
    public class NewStorageInstanceCommand : PersistenceLayerRunTime.Commands.NewStorageInstanceCommand
    {




        public override void Execute()
        {
            RDBMSMetaDataRepository.Class rdbmsMetadataClass = (OnFlyStorageInstance.ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(OnFlyStorageInstance.Class) as RDBMSMetaDataRepository.Class;
          
                ObjectID objectID = OnFlyStorageInstance.ObjectID as ObjectID;// new OOAdvantech.RDBMSPersistenceRunTime.ObjectID(Guid.NewGuid(), 0);
                Commands.UpdateDatabaseMassively.CurrentTransactionCommandUpdateMassively.NewStorageInstance(OnFlyStorageInstance, objectID);
                OnFlyStorageInstance.PersistentObjectID = objectID;
          
         
            
        }

        /// <MetaDataID>{e254e2d0-ca40-461e-bee0-974ffcef85b2}</MetaDataID>
        public NewStorageInstanceCommand(OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef)
            : base(storageInstanceRef)
        {

        }
    }
}
