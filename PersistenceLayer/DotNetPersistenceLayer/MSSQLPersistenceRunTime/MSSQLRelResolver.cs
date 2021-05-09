using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
using System.Collections.Generic;
namespace OOAdvantech.MSSQLPersistenceRunTime
{
    /// <MetaDataID>{6FA51C9F-9C42-4320-83C2-904B1725BE57}</MetaDataID>
    public class RelResolver : PersistenceLayerRunTime.RelResolver
    {
        /// <MetaDataID>{13F246DD-46D0-4E11-AE4D-FBB103276E52}</MetaDataID>
        public RelResolver(PersistenceLayerRunTime.StorageInstanceRef theOwner, DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
            : base(theOwner, associationEnd, fastFieldAccessor)
        {
        }
        /// <MetaDataID>{B87A52B0-2E5F-492F-A3BD-AE194AC96BC0}</MetaDataID>
        public override long GetLinkedObjectsCount()
        {
            return base.GetLinkedObjectsCount();
        }
 
        /// <MetaDataID>{4B45567C-F5A6-4989-A104-897C20C5578E}</MetaDataID>
        public override System.Collections.ArrayList GetLinkedObjects(string Criterion)
        {
            return base.GetLinkedObjects(Criterion);
        }


        /// <MetaDataID>{75B7B026-4F4B-4461-9A8B-1639B746638B}</MetaDataID>
        public override System.Collections.ArrayList GetLinkedStorageInstanceRefs(bool OperativeObjectOnly)
        {
            System.Collections.ArrayList Objects = null;
            if (IsCompleteLoaded)
            {
                if (AssociationEnd.Multiplicity.IsMany)
                    Objects = _LoadedRelatedObjects;
                else
                {
                    Objects = new System.Collections.ArrayList();
                    if (RelatedObject != null)
                        Objects.Add(RelatedObject);
                }
            }
            else
                Objects = GetLinkedObjects("");
            System.Collections.ArrayList StorageInstanceRefs = new System.Collections.ArrayList(Objects.Count);
            foreach (object _objcet in Objects)
                StorageInstanceRefs.Add(StorageInstanceRef.GetStorageInstanceRef(_objcet));
            return StorageInstanceRefs;

        }

        internal void LoadRelatedObjects(OOAdvantech.Collections.ArrayList objects, bool allRelatedObjects)
        {
            IsCompleteLoaded = allRelatedObjects;
            if (_LoadedRelatedObjects == null)
                _LoadedRelatedObjects = new System.Collections.ArrayList();
            if (_LoadedRelatedObjects.Count == 0)
            {
                _LoadedRelatedObjects.AddRange(objects);
                bool subscribeForObjectDeletion = false;
                //TODO: εάν το type του association end είναι interface τότε υπάρχει πρόβλημα  
                if (!(Owner.Class.HasReferentialIntegrity(AssociationEnd))	//it hasn't ReferentialIntegrity
                    && !(AssociationEnd.Navigable && AssociationEnd.GetOtherEnd().Navigable))		//the association doesn't double navigable
                {
                    subscribeForObjectDeletion = true;
                }
                if (subscribeForObjectDeletion)
                {
                    foreach (object _object in objects)
                    {
                        //subscribe 
                        PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(_object) as PersistenceLayerRunTime.StorageInstanceRef;
                        storageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(OnObjectDeleted);
                    }
                }
            }
        }
    }
}
