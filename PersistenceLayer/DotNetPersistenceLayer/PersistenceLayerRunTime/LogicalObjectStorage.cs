namespace OOAdvantech.PersistenceLayerRunTime
{
    /// <MetaDataID>{d1b9c74b-9377-495c-8c9f-292cb5edd848}</MetaDataID>
    public class LogicalObjectStorage : OOAdvantech.PersistenceLayerRunTime.ObjectStorage
    {
        /// <MetaDataID>{626f9ed2-38ae-4d7a-a4f7-1404aaece717}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.ObjectID GetTemporaryObjectID()
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{21f24ddb-f5d9-4810-93a4-0feafd378679}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoader CreateDataLoader(OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dataNode, OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataLoaderMetadata dataLoaderMetadata)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{0dd367f1-c258-4928-9e37-9c22bbd846e4}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(StorageInstanceAgent storageInstanceAgent)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{246b14ce-151f-48af-877d-2ce90120f4d1}</MetaDataID>
        public override void CreateMoveStorageInstanceCommand(StorageInstanceAgent storageInstance)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{6272f5dd-480c-4cc0-907e-f216c95c4e25}</MetaDataID>
        public override void CreateLinkCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            throw new System.NotImplementedException();
        }
        public override void CreateUpdateLinkIndexCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{251861bf-e040-430c-b6c7-116f3521c74e}</MetaDataID>
        public override void CreateUnLinkCommand(StorageInstanceAgent roleA, StorageInstanceAgent roleB, StorageInstanceAgent relationObject, AssociationEndAgent linkInitiatorAssociationEnd, int index)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{eff0fd66-7e63-4cd1-ae58-2b07dd43b950}</MetaDataID>
        public override void CreateUpdateReferentialIntegrity(StorageInstanceRef storageInstanceRef)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{6a26e105-f093-4096-975d-bdc551b72604}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(StorageInstanceAgent sourceStorageInstance, OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{db10397f-2bed-401d-9ba4-f9eca872b1bc}</MetaDataID>
        public override void CreateDeleteStorageInstanceCommand(StorageInstanceRef storageInstance, OOAdvantech.PersistenceLayer.DeleteOptions deleteOption)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{0adf151e-7ec9-421d-a49c-6e2b250f45fc}</MetaDataID>
        public override void CreateUpdateStorageInstanceCommand(StorageInstanceRef storageInstanceRef)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{656eafef-5dcc-4b76-ab36-3218f9a3ec53}</MetaDataID>
        public override void CreateNewStorageInstanceCommand(StorageInstanceRef storageInstance)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{d44de6a7-6483-407f-98b7-0c1881638115}</MetaDataID>
        public override void CreateUnlinkAllObjectCommand(StorageInstanceAgent deletedOutStorageInstanceRef, OOAdvantech.MetaDataRepository.AssociationEnd AssociationEnd, OOAdvantech.MetaDataRepository.StorageCell LinkedObjectsStorageCell)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{5dc31e50-dfe0-4873-be35-43ef4ec1cb6c}</MetaDataID>
        public override StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, OOAdvantech.PersistenceLayer.ObjectID objectID)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{5b50a804-f29a-49bf-9909-ea9fd9b66760}</MetaDataID>
        public override StorageInstanceRef CreateStorageInstanceRef(object memoryInstance, OOAdvantech.PersistenceLayer.ObjectID objectID, OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{aef7f9ec-95af-4e3d-92c2-f677fd7c6a6f}</MetaDataID>
        public override void AbortChanges(TransactionContext theTransaction)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{3a7c3c08-6a58-4902-a9e3-fb8a26d847b8}</MetaDataID>
        public override void CommitChanges(TransactionContext theTransaction)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{655c319a-5a9f-4b8b-9bcc-95761f894ac9}</MetaDataID>
        public override void BeginChanges(TransactionContext theTransaction)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{1ed6eb5d-8773-4fb4-98ba-0dd81758150d}</MetaDataID>
        public override void MakeChangesDurable(TransactionContext theTransaction)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{4225de9c-298a-44ec-a381-11c3382ded25}</MetaDataID>
        public override object GetObject(string persistentObjectUri)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{7011ff19-8f00-4bb3-841b-bd2d7db3ba62}</MetaDataID>
        public override string GetPersistentObjectUri(object obj)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{2b2c6808-334a-4883-a88d-27c5141399b1}</MetaDataID>
        public override void CreateBuildContainedObjectIndiciesCommand(IndexedCollection collection)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{468bf8f8-296a-4d70-81a1-0dd25a27b80e}</MetaDataID>
        protected internal override OOAdvantech.MetaDataRepository.StorageCellReference GetStorageCellReference(OOAdvantech.MetaDataRepository.StorageCell storageCell)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{746a0860-18c6-4167-ae74-dc14a41fb2eb}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier, System.DateTime timePeriodStartDate, System.DateTime timePeriodEndDate)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{aefedab2-5c91-4056-8676-b7aa83c3db47}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.RelatedStorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCell)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{8586f573-d8d6-4487-ab3e-4c39a2526339}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetStorageCells(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{72e49fb0-eec9-41a3-b317-f5d910546906}</MetaDataID>
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> GetRelationObjectsStorageCells(OOAdvantech.MetaDataRepository.Association association, OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell> relatedStorageCells, OOAdvantech.MetaDataRepository.Roles storageCellsRole)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{838f8db7-91cd-41a1-9584-0519e38f361c}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(int storageCellSerialNumber)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{8585a443-cc6e-4188-9eda-c4857bcb2a73}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell GetStorageCell(object ObjectID)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{8fdf7803-0afb-47e0-9277-1faf7a0aef1d}</MetaDataID>
        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement, OOAdvantech.Collections.Generic.Dictionary<string, object> parameters)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{539d4f77-7219-4e75-bf57-eb8255773516}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.Storage StorageMetaData
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <MetaDataID>{615e73e1-a6e2-41dd-965d-f7b941727592}</MetaDataID>
        public override OOAdvantech.Collections.StructureSet Execute(string OQLStatement)
        {
            throw new System.NotImplementedException();
        }
    }
}
