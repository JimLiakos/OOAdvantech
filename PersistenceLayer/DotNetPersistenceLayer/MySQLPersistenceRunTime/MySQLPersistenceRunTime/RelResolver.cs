namespace OOAdvantech.MySQLPersistenceRunTime
{

    /// <MetaDataID>{e3bcef4c-160e-4fe5-951b-c4db9150f4c8}</MetaDataID>
    public class RelResolver : PersistenceLayerRunTime.RelResolver
    {
        
        public RelResolver(PersistenceLayerRunTime.StorageInstanceRef theOwner, DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
            : base(theOwner, associationEnd, fastFieldAccessor)
        {
        }
        
        public override long GetLinkedObjectsCount()
        {
            //TODO να γραφτεί test case
            if (Owner.PersistentObjectID == null)
                if (_LoadedRelatedObjects == null)
                    return 0;
                else
                    return _LoadedRelatedObjects.Count;
            //if (AssociationEnd.Association.Specializations.Count > 0)
            //{
            //    if (IsCompleteLoaded)
            //        return LoadedRelatedObjects.Count;
            //    else
            //    {
            //        Load("");
            //        IsCompleteLoaded = true;
            //        return LoadedRelatedObjects.Count;
            //    }
            //}

            string query = null;
            string collactionAliasGuid = "the" + System.Guid.NewGuid().ToString().Replace("-", "");
            if (Owner is OOAdvantech.PersistenceLayerRunTime.StorageInstanceValuePathRef)
            {
                string AgregateFunctionExpresion = " the" + Owner.Class.Name + ".";
                foreach (OOAdvantech.MetaDataRepository.MetaObjectID identity in (Owner as OOAdvantech.PersistenceLayerRunTime.StorageInstanceValuePathRef).ValueTypePath)
                    AgregateFunctionExpresion += (Owner.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(identity.ToString(), typeof(MetaDataRepository.Attribute)).OQLName + ".";
                AgregateFunctionExpresion += AssociationEnd.OQLName + " " + collactionAliasGuid;


                query = "SELECT the" + Owner.Class.Name + ".Count(" + AgregateFunctionExpresion + ") [Count] \nFROM " + Owner.Class.OQLFullName + " the" + Owner.Class.Name;
                query += "\nWHERE  (the" + Owner.Class.Name + " = " + Owner.PersistentObjectID.ToString() + ")";

            }
            else
            {
                if (AssociationEnd.Association.LinkClass == null)
                    query = "SELECT  the" + Owner.Class.Name + ".Count(" + AssociationEnd.OQLName + ") [Count] \nFROM " + Owner.Class.OQLFullName + " the" + Owner.Class.Name + " , the" + Owner.Class.Name + "." + AssociationEnd.OQLName + " " + collactionAliasGuid;
                else
                    query = "SELECT  the" + Owner.Class.Name + ".Count(" + AssociationEnd.Association.OQLName + ") [Count] \nFROM " + Owner.Class.OQLFullName + " the" + Owner.Class.Name + " , the" + Owner.Class.Name + "." + AssociationEnd.Association.OQLName + " " + collactionAliasGuid;
                query += "\nWHERE  (the" + Owner.Class.Name + " = " + Owner.PersistentObjectID.ToString() + ")";

            }

            OOAdvantech.Collections.StructureSet structureSet = Owner.ObjectStorage.Execute(query);
            foreach (OOAdvantech.Collections.StructureSet instanceSet in structureSet)
            {
                return (int)instanceSet["Count"];
            }


            return 0;
        }

        
        public override System.Collections.ArrayList GetLinkedObjects(string Criterion)
        {
            string Query = null;
            string collactionAliasGuid = "the" + System.Guid.NewGuid().ToString().Replace("-", "");
            if (Owner is OOAdvantech.PersistenceLayerRunTime.StorageInstanceValuePathRef)
            {
                Query = "SELECT " + collactionAliasGuid + " \nFROM " + Owner.Class.OQLFullName + " the" + Owner.Class.Name + " , the" + Owner.Class.Name + ".";
                foreach (OOAdvantech.MetaDataRepository.MetaObjectID identity in (Owner as OOAdvantech.PersistenceLayerRunTime.StorageInstanceValuePathRef).ValueTypePath)
                    Query += (Owner.ObjectStorage.StorageMetaData as RDBMSPersistenceRunTime.Storage).GetEquivalentMetaObject(identity.ToString(), typeof(MetaDataRepository.Attribute)).OQLName + ".";
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
