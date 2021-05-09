namespace OOAdvantech.PersistenceLayerRunTime
{
    using System.Collections.Generic;
    /// <MetaDataID>{ac59ab9b-0c80-4ff1-8489-af2325eefe0c}</MetaDataID>
    public class StorageInstanceValuePathRef : OOAdvantech.PersistenceLayerRunTime.StorageInstanceRef
    {
        /// <MetaDataID>{e4fd9056-52d4-4681-8d7b-ef88de8fc7f1}</MetaDataID>
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(1, 1)]
        [MetaDataRepository.Association("", typeof(StorageInstanceRef), MetaDataRepository.Roles.RoleA, "5f602444-a69b-4bb7-a45a-3c5b88236132")]
        [MetaDataRepository.IgnoreErrorCheck]
        public StorageInstanceRef OriginalStorageInstanceRef;

        /// <MetaDataID>{f964ca67-e2f9-41f4-8692-a9136d042b94}</MetaDataID>
        public MetaDataRepository.ValueTypePath ValueTypePath;
        /// <MetaDataID>{4b2d789f-ac12-4d82-86be-2aff289cc8ba}</MetaDataID>
        public StorageInstanceValuePathRef(StorageInstanceRef originalStorageInstanceRef, MetaDataRepository.ValueTypePath valueTypePath)
        {
            if (originalStorageInstanceRef.MemoryInstance == null)
                throw new System.ArgumentNullException("You can't create StorageInstanceRef with out memory instance");
            if (originalStorageInstanceRef.ObjectStorage == null)
                throw new System.ArgumentNullException("You can't create StorageInstanceRef with out Active storege session");

             
            _MemoryInstance =  originalStorageInstanceRef._MemoryInstance;
            _ObjectStorage = originalStorageInstanceRef.ObjectStorage;

            OriginalStorageInstanceRef = originalStorageInstanceRef;
            ValueTypePath = new MetaDataRepository.ValueTypePath(valueTypePath);
        }
        /// <MetaDataID>{8241e678-76db-45a9-9639-0fb3a48e910b}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.StorageCell StorageInstanceSet
        {
            get { return OriginalStorageInstanceRef.StorageInstanceSet; }
        }
        /// <MetaDataID>{52d94b56-e83c-4297-bb0b-85ed408dda4d}</MetaDataID>
        public override PersistenceLayer.ObjectID PersistentObjectID
        {
            get
            {
                return OriginalStorageInstanceRef.PersistentObjectID;
            }
            set
            {
                throw new System.Exception("The method set_ObjectID is not implemented from StorageInstanceValuePathRef.");
            }
        }


        /// <MetaDataID>{d2bdbaaf-d30b-41fe-82db-aeded8f6a581}</MetaDataID>
        protected override RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        /// <MetaDataID>{35691b30-f534-4c80-ad60-4492e8b9fcd3}</MetaDataID>
        protected override RelResolver CreateRelationResolver(OOAdvantech.DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, StorageInstanceRef owner)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }


        protected override void AddObjectsLink(AssociationEndAgent associationEnd, object relatedObject)
        {
            DotNetMetaDataRepository.AssociationEnd.AddValueTypeObjectsLink(associationEnd.RealAssociationEnd, MemoryInstance, relatedObject, ValueTypePath);
        }
        protected override void RemoveObjectsLink(AssociationEndAgent associationEnd, object relatedObject)
        {
            DotNetMetaDataRepository.AssociationEnd.RemoveValueTypeObjectsLink(associationEnd.RealAssociationEnd, MemoryInstance, relatedObject, ValueTypePath);
            
        }
    }
}
