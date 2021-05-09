using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.MSSQLCompactPersistenceRunTime
{
    /// <MetaDataID>{28b49be2-f48c-4bfa-85ab-34b1cd6dbd9e}</MetaDataID>
    class RelResolver : PersistenceLayerRunTime.RelResolver
    {

        public RelResolver(PersistenceLayerRunTime.StorageInstanceRef theOwner, DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
            : base(theOwner, associationEnd, fastFieldAccessor)
        {
        }
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

        public override System.Collections.ArrayList GetLinkedObjects(string Criterion)
        {
            string Query = null;
            string collactionAliasGuid = "the" + System.Guid.NewGuid().ToString().Replace("-", "");
            if (Owner is OOAdvantech.PersistenceLayerRunTime.StorageInstanceValuePathRef)
            {
                Query = "SELECT " + collactionAliasGuid + " \nFROM " + Owner.Class.OQLFullName + " the" + Owner.Class.Name + " , the" + Owner.Class.Name + ".";
                foreach (OOAdvantech.MetaDataRepository.MetaObjectID identity in (Owner as OOAdvantech.PersistenceLayerRunTime.StorageInstanceValuePathRef).ValueTypePath)
                    Query += (Owner.ObjectStorage.StorageMetaData as RDBMSMetaDataRepository.Storage).GetEquivalentMetaObject(identity.ToString(), typeof(MetaDataRepository.Attribute)).OQLName + ".";
                Query += AssociationEnd.OQLName + " " + collactionAliasGuid;
                Query += "\nWHERE  (the" + Owner.Class.Name + " = " + Owner.PersistentObjectID.ToString() + ")";
                Criterion = Criterion.Trim();
                if (Criterion.Length > 3)
                    Query += " AND (" + Criterion + ")";
            }
            else
            {
                if (AssociationEnd.Association.LinkClass == null)
                    Query = "SELECT " + collactionAliasGuid + " \nFROM " + Owner.Class.OQLFullName + " the" + Owner.Class.Name + " , the" + Owner.Class.Name + "." + AssociationEnd.OQLName + " " + collactionAliasGuid;
                else
                    Query = "SELECT " + collactionAliasGuid + " \nFROM " + Owner.Class.OQLFullName + " the" + Owner.Class.Name + " , the" + Owner.Class.Name + "." + AssociationEnd.Association.OQLName + " " + collactionAliasGuid;
                Query += "\nWHERE  (the" + Owner.Class.Name + " = " + Owner.PersistentObjectID.ToString() + ")";
                Criterion = Criterion.Trim();
                if (Criterion.Length > 3)
                    Query += " AND (" + Criterion + ")";
            }




            if (Transactions.Transaction.Current != null)
            {

                using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                {
                    System.Collections.ArrayList ObjectCollection = new System.Collections.ArrayList();
                    if (Owner.PersistentObjectID == null)
                    {
                        stateTransition.Consistent = true;
                        return ObjectCollection;
                    }



                    Collections.StructureSet aStructureSet = Owner.ObjectStorage.Execute(Query);
                    foreach (Collections.StructureSet Rowset in aStructureSet)
                    {
                        object mObject = null;
                        if (AssociationEnd.Association.LinkClass == null)
                        {

                            if (AssociationEnd.Multiplicity.IsMany)
                            {
                                mObject = Rowset.Members[collactionAliasGuid].Value;
                                foreach (object obj in mObject as System.Collections.ArrayList)
                                    ObjectCollection.Add(obj);

                            }
                            else
                            {
                                mObject = Rowset.Members[collactionAliasGuid].Value;
                                if (mObject != null)
                                    ObjectCollection.Add(mObject);

                            }

                        }
                        else
                        {
                            mObject = Rowset.Members[collactionAliasGuid].Value;
                            if (mObject != null)
                                ObjectCollection.Add(mObject);
                        }
                    }
                    stateTransition.Consistent = true;

                    return ObjectCollection;
                }
            }
            else
            {
                System.Collections.ArrayList ObjectCollection = new System.Collections.ArrayList();

                if (Owner.PersistentObjectID == null)
                    return ObjectCollection;



                Collections.StructureSet aStructureSet = Owner.ObjectStorage.Execute(Query);
                foreach (Collections.StructureSet Rowset in aStructureSet)
                {
                    object mObject = null;
                    if (AssociationEnd.Multiplicity.IsMany)
                    {
                        mObject = Rowset.Members[collactionAliasGuid].Value;
                        foreach (object obj in mObject as System.Collections.ArrayList)
                            ObjectCollection.Add(obj);

                    }
                    else
                    {
                        mObject = Rowset.Members[collactionAliasGuid].Value;

                        if (mObject != null)
                            ObjectCollection.Add(mObject);
                    }
                }

                return ObjectCollection;

            }
        }


        public override long GetLinkedObjectsCount()
        {
            if (!IsCompleteLoaded)
                CompleteLoad();
            return _LoadedRelatedObjects.Count;
        }
    }
}
