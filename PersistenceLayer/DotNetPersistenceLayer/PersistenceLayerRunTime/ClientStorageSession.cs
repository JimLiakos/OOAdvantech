namespace OOAdvantech.PersistenceLayerRunTime.ClientSide
{
    using System;
    /// <MetaDataID>{9A79D728-204A-453A-A013-75F20210DE3A}</MetaDataID>
    public class ObjectStorageAgent : PersistenceLayer.ObjectStorage
    {
        /// <MetaDataID>{c382ffdb-8af4-4179-b935-bdaed246f5c0}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCells, OOAdvantech.MetaDataRepository.Roles storageCellsRole, string ofTypeIdentity = null)
        {
            return ServerSideObjectStorage.GetRelationObjectsStorageCells(association, relatedStorageCells, storageCellsRole);
        }
        /// <MetaDataID>{71e2fa1f-7dfb-4caa-b28d-2c6c267df171}</MetaDataID>
        public override object GetObjectID(object persistentObject)
        {
           return ServerSideObjectStorage.GetObjectID(persistentObject);
        }
        public override string GetPersistentObjectUri(object obj)
        {
            return ServerSideObjectStorage.GetPersistentObjectUri(obj);
        }


        /// <MetaDataID>{3d609410-31bc-45bd-a426-506f10fba729}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier, DateTime timePeriodStartDate, DateTime timePeriodEndDate)
        {
            return ServerSideObjectStorage.GetStorageCells(classifier, timePeriodStartDate, timePeriodEndDate);
        }
        /// <MetaDataID>{d102e09a-e4a4-44ee-9467-2a22ee45637b}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, MetaDataRepository.ValueTypePath valueTypePath, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCell, string ofTypeIdentity = null)
        {
            return ServerSideObjectStorage.GetLinkedStorageCells(associationEnd,valueTypePath, relatedStorageCell, ofTypeIdentity);
        }
        /// <MetaDataID>{3263388d-5337-48da-935f-17055177580c}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            return ServerSideObjectStorage.GetStorageCells(classifier);
        }

        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(int storageCellSerialNumber)
        {
            return ServerSideObjectStorage.GetStorageCell(storageCellSerialNumber);
        }

        /// <MetaDataID>{3586c662-0d4f-4bf9-b041-d5f51038cf5d}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(object objectID)
        {
            return ServerSideObjectStorage.GetStorageCell(objectID);
        }
       
        /// <MetaDataID>{1CC51E27-C3D3-4764-B4CB-6B710467C182}</MetaDataID>
        public ObjectStorageAgent(PersistenceLayer.ObjectStorage serverSideStorageSession)
        {
            ServerSideObjectStorage = (PersistenceLayer.ObjectStorage)serverSideStorageSession;
        }

        /// <MetaDataID>{438B323A-7FA8-47B8-9DD9-BD460E9F26A0}</MetaDataID>
        public PersistenceLayer.ObjectStorage ServerSideObjectStorage;
        /// <summary>Executes the specified query in OQLStatement parameter. Return a StructureSet object that contain the result of object query statement.</summary>
        /// <param name="OQLStatement">A String value that contains the OQL statement</param>
        /// <MetaDataID>{DBDD957C-A19F-439F-9A65-47398D5AD91B}</MetaDataID>
        public override Collections.StructureSet Execute(string OQLStatement)
        {
            return ServerSideObjectStorage.Execute(OQLStatement);

        }
        /// <MetaDataID>{811C0358-2ED4-450A-BE42-E67F1BE3AE45}</MetaDataID>
        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
        {
            return ServerSideObjectStorage.Execute(OQLStatement, parameters);
        }

        /// <MetaDataID>{A3ADB7B8-9CAE-4209-8332-DF81FD9E78A1}</MetaDataID>
        public override PersistenceLayer.Storage StorageMetaData
        {
            get
            {
                return ServerSideObjectStorage.StorageMetaData;
            }
        }
        // mitsos
        /// <MetaDataID>{DEA9C37B-8D6C-4371-8D81-5ACE9F5B71EF}</MetaDataID>
        protected override void Delete(object thePersistentObject, OOAdvantech.PersistenceLayer.DeleteOptions deleteOption)
        {

            (ServerSideObjectStorage as IObjectStorage).UnprotectedDelete(thePersistentObject, deleteOption);
        }

        public override void MoveObject(object persistentObject)
        {
            ServerSideObjectStorage.MoveObject(persistentObject);
        }
        /// <MetaDataID>{1C9775F5-268C-4B47-8E48-7E1D6019DE75}</MetaDataID>
        public override object NewObject(System.Type type, Type[] paramsTypes, params object[] ctorParams)
        {
            return ServerSideObjectStorage.NewObject(type, paramsTypes,ctorParams);
        }
        /// <MetaDataID>{3B180D0B-F1EB-4060-9EB0-E6C48ED7E7F0}</MetaDataID>
        public override object NewTransientObject(System.Type type,Type[] paramsTypes, params object[] ctorParams)
        {
            return ServerSideObjectStorage.NewTransientObject(type, paramsTypes,ctorParams);
        }
        /// <MetaDataID>{b4d9fab5-099d-4cf3-873c-5be5850ed814}</MetaDataID>
        public override object NewTransientObject(Type type)
        {
            return ServerSideObjectStorage.NewTransientObject(type);
        }

        /// <MetaDataID>{878ae76d-c85f-44ad-870c-d93fdda2e855}</MetaDataID>
        public override object NewObject(Type type)
        {
            return ServerSideObjectStorage.NewObject(type);
        }

        /// <MetaDataID>{89AF587C-197A-412E-95BB-AA20CE06F387}</MetaDataID>
        public override void CommitTransientObjectState(object Object)
        {
            // Error prone to object αν δεν είναι στο ίδιο process με την storage
            // θα πρέπει να κοπανά exception.
            ServerSideObjectStorage.CommitTransientObjectState(Object);
        }

        public override bool HasReferencialintegrityConstrain(object thePersistentObject)
        {
           return ServerSideObjectStorage.HasReferencialintegrityConstrain(thePersistentObject);
        }

        public override object GetObject(string persistentObjectUri)
        {
            return ServerSideObjectStorage.GetObject(persistentObjectUri);
        }
    }
}
