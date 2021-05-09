using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.DotNetMetaDataRepository;
using OOAdvantech.PersistenceLayerRunTime;

namespace OOAdvantech.WindowsAzureTablesPersistenceRunTime
{
    /// <MetaDataID>{cc046dcc-9976-4730-bc37-7c76e2ef10e0}</MetaDataID>
    public class RelResolver : PersistenceLayerRunTime.RelResolver
    {
        public RelResolver(PersistenceLayerRunTime.StorageInstanceRef owner, AssociationEnd associationEnd, AccessorBuilder.FieldPropertyAccessor fastFieldAccessor) : base(owner, associationEnd, fastFieldAccessor)
        {
        }

        public override List<object> GetLinkedStorageInstanceRefs(bool OperativeObjectOnly)
        {
            System.Collections.Generic.List<object> Objects = null;
            if (IsCompleteLoaded)
            {
                if (AssociationEnd.Multiplicity.IsMany)
                    Objects = InternalLoadedRelatedObjects;
                else
                {
                    Objects = new OOAdvantech.Collections.Generic.List<object>();
                    if (RelatedObject != null)
                        Objects.Add(RelatedObject);
                }
            }
            else
                Objects = GetLinkedObjects("");
            System.Collections.Generic.List<object> StorageInstanceRefs = new System.Collections.Generic.List<object>(Objects.Count);
            foreach (object _objcet in Objects)
                StorageInstanceRefs.Add(StorageInstanceRef.GetStorageInstanceRef(_objcet));
            return StorageInstanceRefs;
        }
    }
}
