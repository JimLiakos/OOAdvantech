namespace OOAdvantech.PersistenceLayerRunTime
{

    using RelationChangeCollection = Collections.Generic.Dictionary<string, Collections.Generic.Dictionary<object, ObjectsOfManyRelationsipp.RelationChange>>;


    /// <MetaDataID>{6ED1C5D2-91B9-42F0-8944-00157903C489}</MetaDataID>
    /// <summary>This defines a collection which keeps the related objects of persistent relationship.
    /// If there are a lot of related objects the ObjectsOfManyRelationsip can load a part of related objects. Also the ObjectsOfManyRelationsip keep all added and removed objects as transactions. </summary>
    [System.Serializable]
    public class ObjectsOfManyRelationsipp : /*System.MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject,*/ PersistenceLayer.ObjectCollection
    {
        public bool CanDeletePermanently(object theObject)
        {
            return false;
        }

        public void Insert(int index, object item)
        {
            ///TODO θα πρέπει να διασφαλιστή ότι τυχών error με το index π.χ. index έξω από το rangge της collection θα πρέπει να βγένει εδώ
            if (item == null||index==-1)
                return;

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {


                if (ObjectCollectionType == null)
                    ObjectCollectionType = theRelResolver.AssociationEnd.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                if (!ObjectCollectionType.IsInstanceOfType(item))
                    throw new System.ArgumentNullException("Type mismatch");


                RelationChange relationChangement = GetRelationChange(item, Transactions.Transaction.Current);
                if (relationChangement != null)
                {
                    if (relationChangement.TypeOfChange != RelationChange.ChangeType.Added)
                    {
                        relationChangement.ProducedFromRT = false;
                        relationChangement.TypeOfChange = RelationChange.ChangeType.Added;
                        relationChangement.Index = index;
                    }
                }
                else
                {
                    if (!theRelResolver.IsCompleteLoaded)
                        theRelResolver.CompleteLoad();

                    int oldIndex = theRelResolver.LoadedRelatedObjects.IndexOf(item);
                    if (oldIndex!=index)
                    {
                        relationChangement = new RelationChange(item, RelationChange.ChangeType.Added, index, false);
                        AddRelationChange(relationChangement, Transactions.Transaction.Current);
                    }
                }

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }


        }
        public void RemoveAt(int index)
        {
        }
        public int IndexOf(object item)
        {
            if (GetRelationChanges(Transactions.Transaction.Current).Length == 0)
            {
                if (!theRelResolver.IsCompleteLoaded)
                    theRelResolver.CompleteLoad();
                return theRelResolver.LoadedRelatedObjects.IndexOf(item);
            }

            if (!theRelResolver.IsCompleteLoaded)
                theRelResolver.CompleteLoad();
            System.Collections.ArrayList relatedObjects = new System.Collections.ArrayList(theRelResolver.LoadedRelatedObjects);
            //Take in it count the relation changes which are not saved yet.
            foreach (RelationChange relationChangement in GetFinalRelationChanges(Transactions.Transaction.Current))
            {
                //RelationChangement relationChangement=entry.Value as RelationChangement;
                if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                    relatedObjects.Remove(relationChangement.Object);
                if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && !theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                    relatedObjects.Add(relationChangement.Object);
            }
            return relatedObjects.IndexOf(item);

        }
        public int IndexOf(object item, int index)
        {
            if (GetRelationChanges(Transactions.Transaction.Current).Length == 0)
            {
                if (!theRelResolver.IsCompleteLoaded)
                    theRelResolver.CompleteLoad();
#if !NETCompactFramework
                return theRelResolver.LoadedRelatedObjects.IndexOf(item, index);
#else
                return theRelResolver.LoadedRelatedObjects.IndexOf(item, index, theRelResolver.LoadedRelatedObjects.Count-index);
#endif 
            }

            if (!theRelResolver.IsCompleteLoaded)
                theRelResolver.CompleteLoad();
            System.Collections.ArrayList relatedObjects = new System.Collections.ArrayList(theRelResolver.LoadedRelatedObjects);
            //Take in it count the relation changes which are not saved yet.
            foreach (RelationChange relationChangement in GetFinalRelationChanges(Transactions.Transaction.Current))
            {
                //RelationChangement relationChangement=entry.Value as RelationChangement;
                if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                    relatedObjects.Remove(relationChangement.Object);
                else if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && relationChangement.Index != -1)
                {
                    if (relationChangement.Index < index)
                        return -1;
                    return relationChangement.Index;
                }
                else if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && !theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                    relatedObjects.Add(relationChangement.Object);
            }
#if !NETCompactFramework
            return relatedObjects.IndexOf(item, index);
#else
            return relatedObjects.IndexOf(item, index,relatedObjects.Count-index);
#endif

        }
        public int IndexOf(object item, int index, int count)
        {
            if (GetRelationChanges(Transactions.Transaction.Current).Length == 0)
            {
                if (!theRelResolver.IsCompleteLoaded)
                    theRelResolver.CompleteLoad();
                return theRelResolver.LoadedRelatedObjects.IndexOf(item, index, count);
            }

            if (!theRelResolver.IsCompleteLoaded)
                theRelResolver.CompleteLoad();
            System.Collections.ArrayList relatedObjects = new System.Collections.ArrayList(theRelResolver.LoadedRelatedObjects);
            //Take in it count the relation changes which are not saved yet.
            foreach (RelationChange relationChangement in GetFinalRelationChanges(Transactions.Transaction.Current))
            {
                //RelationChangement relationChangement=entry.Value as RelationChangement;
                if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                    relatedObjects.Remove(relationChangement.Object);
                else if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && relationChangement.Index != -1)
                {
                    if (relationChangement.Index < index)
                        return -1;
                    if (relationChangement.Index > count)
                        return -1;
                    return relationChangement.Index;
                }
                else if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && !theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                    relatedObjects.Add(relationChangement.Object);
            }
            return relatedObjects.IndexOf(item, index, count);
        }

        public object this[int index]
        {
            get
            {
                if (GetRelationChanges(Transactions.Transaction.Current).Length == 0)
                {
                    if (!theRelResolver.IsCompleteLoaded)
                        theRelResolver.CompleteLoad();
                    return theRelResolver.LoadedRelatedObjects[index];
                }

                if (!theRelResolver.IsCompleteLoaded)
                    theRelResolver.CompleteLoad();
                System.Collections.ArrayList relatedObjects = new System.Collections.ArrayList(theRelResolver.LoadedRelatedObjects);
                //Take in it count the relation changes which are not saved yet.
                foreach (RelationChange relationChangement in GetFinalRelationChanges(Transactions.Transaction.Current))
                {
                    //RelationChangement relationChangement=entry.Value as RelationChangement;
                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && relatedObjects.Contains(relationChangement.Object))
                        relatedObjects.Remove(relationChangement.Object);
                    else if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && !relatedObjects.Contains(relationChangement.Object) && relationChangement.Index == -1)
                        relatedObjects.Add(relationChangement.Object);
                    else if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && relationChangement.Index == index)
                        return relationChangement.Object;
                    else if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && relatedObjects.Contains(relationChangement.Object) && relationChangement.Index != -1 && relationChangement.Index != relatedObjects.IndexOf(relationChangement.Object))
                    {
                        relatedObjects.Remove(relationChangement.Object);
                        relatedObjects.Insert(relationChangement.Index, relationChangement.Object);
                    }
                }
                return relatedObjects[index];
            }

        }
        /// <MetaDataID>{29A80D58-3D37-4F09-93E9-E395D1B2B6AB}</MetaDataID>
        OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();

        /// <MetaDataID>{16B8EACA-C5EF-4C79-8AFF-25D6F2E805F0}</MetaDataID>
        /// <summary>
        /// The Object of this type keeps information about the change in related objects collection
        /// </summary>
        internal class RelationChange
        {
            /// <summary>
            /// The zero-based index at which item should be inserted.
            /// Used only when change type is Added
            /// if the index is -1 and the change type is Added then the object added at the end of collection
            /// </summary>
            public int Index;

            /// <MetaDataID>{E4E250DA-BDC1-4849-834C-1F5E0B20C885}</MetaDataID>
            /// <summary>
            /// This field defines the type of change.
            /// </summary>
            public ChangeType TypeOfChange;

            /// <summary>
            /// ChangeType defines the type of change, Added or Deleted.
            /// </summary>
            public enum ChangeType
            {
                /// <summary>
                /// The object added to related objects.
                /// </summary>
                Added,
                /// <summary>
                /// The object deleted from related objects.
                /// </summary>
                Deleteded
            }




            /// <MetaDataID>{5C589AB6-1527-4754-A8DF-E37BCFB304D9}</MetaDataID>
            ///<summary>
            ///This field defines the object which added or deleted from related objects collection.
            ///</summary>
            public object Object;

            /// <MetaDataID>{F67C8AB9-B774-4348-BE44-750F4D134094}</MetaDataID>
                /// <summary>
                /// This field defines the source of change.
                /// If the change comes from client code then the value is false.
                /// If the change comes from Perstency system runtime the value is true.
                /// It is useful for system to decide if produce link or unlink command,
                /// with the call of corresponding method of relation resolver.
                /// </summary>
            public bool ProducedFromRT = false;
            /// <MetaDataID>{243FF108-8AFD-4DAA-8497-E9E9DD736E2F}</MetaDataID>
            /// <exclude>Excluded</exclude>
            public bool ChangeApplied = false;
            public bool TransactionFree = false;

            /// <summary>
            /// Create an instance of RelationChangement.
            /// </summary>
            /// <param name="_object">
            /// This parameter defines the object which added or deleted from related objects collection.
            /// </param>
            /// <param name="typeOfChange">
            /// This parameter defines the type of change.
            /// ChangeType.Added for addition to related objects collection,
            /// ChangeType.Deleteded for deletion from related objects collection.
            /// </param>
            /// <param name="producedFromRT">
            /// This parameter defines the source of change.
            /// If the change comes from client code then the value is false.
            /// If the change comes from Perstency system runtime the value is true.
            /// </param>
            /// <MetaDataID>{E4D40EAB-1B91-478F-B288-42F581A7E2BF}</MetaDataID>
            public RelationChange(object _object, ChangeType typeOfChange, bool producedFromRT)
                : this(_object, typeOfChange,-1,producedFromRT)
            {
            }
            /// <summary>
            /// Create an instance of RelationChangement.
            /// </summary>
            /// <param name="_object">
            /// This parameter defines the object which added or deleted from related objects collection.
            /// </param>
            /// <param name="typeOfChange">
            /// This parameter defines the type of change.
            /// ChangeType.Added for addition to related objects collection,
            /// ChangeType.Deleteded for deletion from related objects collection.
            /// </param>
            /// <param name="producedFromRT">
            /// This parameter defines the source of change.
            /// If the change comes from client code then the value is false.
            /// If the change comes from Perstency system runtime the value is true.
            /// </param>
            /// <param name="index">
            /// The zero-based index at which item should be inserted.
            /// Used only when change type is Added
            /// if the index is -1 and the change type is Added then the object added at the end of collection
            /// </param>
            /// <MetaDataID>{E4D40EAB-1B91-478F-B288-42F581A7E2BF}</MetaDataID>
            public RelationChange(object _object, ChangeType typeOfChange,int index, bool producedFromRT)
            {
                Object = _object;
                TypeOfChange = typeOfChange;
                ProducedFromRT = producedFromRT;
                TransactionFree = Transactions.Transaction.Current == null;
                Index = index;
            }

        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{4D9B4E7C-94E9-4285-A2ED-D628D100F462}</MetaDataID>
        private RelationChangeCollection _RelationChanges = new RelationChangeCollection();
        /// <MetaDataID>{342F2B52-4F6B-4AE6-BFE3-ADF90CBE5C8E}</MetaDataID>
        /// <summary>
        /// This prperty defines a collection with the unsaved changes of related objects
        /// </summary>
        private RelationChangeCollection RelationChanges
        {
            get
            {
                return _RelationChanges;
            }
        }

        /// <MetaDataID>{759DD1FF-FBBE-4B98-8CF0-D53B3374BCF4}</MetaDataID>
        /// <summary>With this method we add an object to the collection of the related objects.
        /// This addition translated to relation change. If transaction commited this addition saved in
        /// storage. In case where the transaction roll back the this addition canceled.
        /// Some times the addition derived from persistency system runtime. In this 
        /// case the ObjectsOfManyRelationsip object must not produce link command to save 
        /// the relation because the relation already exist in objects storage. </summary>
        /// <param name="anObject">This parameter defines the object, which we want to add to the related objects collection. </param>
        /// <param name="producedFromRT">This parameter indicate where the addition comes from persistency system runtime. </param>
        public void Add(object anObject, bool producedFromRT)
        {
            if (anObject == null)
                return;

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {


                if (ObjectCollectionType == null)
                    ObjectCollectionType = theRelResolver.AssociationEnd.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
                if (!ObjectCollectionType.IsInstanceOfType(anObject))
                    throw new System.ArgumentNullException("Type mismatch");

                RelationChange relationChangement = GetRelationChange(anObject, Transactions.Transaction.Current);
                if (relationChangement != null)
                {
                    if (relationChangement.TypeOfChange != RelationChange.ChangeType.Added)
                    {
                        relationChangement.ProducedFromRT = producedFromRT;
                        relationChangement.TypeOfChange = RelationChange.ChangeType.Added;
                    }
                }
                else
                {
                    if (!theRelResolver.LoadedRelatedObjects.Contains(anObject))
                    {
                        relationChangement = new RelationChange(anObject, RelationChange.ChangeType.Added, producedFromRT);
                        AddRelationChange(relationChangement, Transactions.Transaction.Current);
                        //RelationChanges.Add(anObject, relationChangement);
                    }
                }
                if (producedFromRT && !theRelResolver.LoadedRelatedObjects.Contains(anObject))
                {
                    if(relationChangement.Index!=-1)
                        theRelResolver.LoadedRelatedObjects.Insert(relationChangement.Index, anObject);
                    else
                        theRelResolver.LoadedRelatedObjects.Add(anObject);
                    relationChangement.ChangeApplied = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }


        }

        /// <MetaDataID>{7F806AD2-44DD-4ED5-8C90-312B30FCEC97}</MetaDataID>
        /// <summary>With this method we remove an object from the collection of the related objects.
        /// This deletion translated to relation change. If transaction commited this deletion saved in
        /// storage. In case where the transaction roll back the deletion canceled.
        /// Some times the deletion derived from persistency system runtime. In this 
        /// case the ObjectsOfManyRelationsip object must not produce unlink command to erase 
        /// the relation because the relation already removed from objects storage. </summary>
        /// <param name="anObject">This parameter defines the object, which we want to remove from the related objects collection. </param>
        /// <param name="producedFromRT">This parameter indicate where the deletion comes from persistency system runtime. </param>
        public void Remove(object anObject, bool producedFromRT)
        {

            if (anObject == null)
                return;

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                RelationChange relationChangement = GetRelationChange(anObject, Transactions.Transaction.Current);

                if (relationChangement != null)
                {
                    relationChangement.ProducedFromRT = producedFromRT;
                    if (relationChangement.TypeOfChange != RelationChange.ChangeType.Deleteded)
                        relationChangement.TypeOfChange = RelationChange.ChangeType.Deleteded;
                }
                else
                {
                    relationChangement = new RelationChange(anObject, RelationChange.ChangeType.Deleteded, producedFromRT);
                    AddRelationChange(relationChangement, Transactions.Transaction.Current);
                }
                if (producedFromRT && theRelResolver.LoadedRelatedObjects.Contains(anObject))
                {
                    theRelResolver.LoadedRelatedObjects.Remove(anObject);
                    relationChangement.ChangeApplied = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }

        /// <MetaDataID>{2A1A405F-A5AD-4A80-8D12-AF609E955BF3}</MetaDataID>
        /// This property defines the number of the related objects.
        /// Implement the property Count of PersistenceLayer.ObjectCollection interface;
        public virtual int Count
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    #region Loads all related objects and checks the relation change collection
                    if (!this.theRelResolver.IsCompleteLoaded)
                    {

                        this.theRelResolver.CompleteLoad();
                        //System.Collections.Hashtable relationChangements=new System.Collections.Hashtable(RelationChangements);

                        //In lazy fatching collections can already exist object, which added before the complete load of related objects.
                        //Also deleted objects does not exist in related objects collection but system didn't know that 
                        //before the complete load of related objects.
                        foreach (RelationChange relationChangement in GetAllRelationChanges())
                        {
                            //RelationChangement relationChangement=entry.Value as RelationChangement;
                            bool exist = theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object);
                            //With this code we keep the objects where added in this transaction
                            if (relationChangement.ChangeApplied)
                                continue;

                            if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && !exist)
                                RemoveRelationChange(relationChangement);
                            //    RelationChangements.Remove(entry.Key);


                            if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && exist)
                                RemoveRelationChange(relationChangement);
                            //RelationChangements.Remove(entry.Key);
                        }
                    }
                    #endregion

                    int count = theRelResolver.LoadedRelatedObjects.Count;

                    #region Take in it count the relation changes which are not saved yet for current transaction.
                    foreach (RelationChange relationChangement in GetFinalRelationChanges(Transactions.Transaction.Current))
                    {

                        //RelationChangement relationChangement=entry.Value as RelationChangement;
                        if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                            count--;

                        if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && !theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                            count++;
                    }
                    #endregion
                    return count;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <summary></summary>
        /// <MetaDataID>{4F7371C0-D7F6-4399-9954-C07F1D8FDC25}</MetaDataID>
        public virtual System.Collections.IEnumerator GetEnumerator()
        {

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {

                #region Loads all related objects and checks the relation change collection

                if (!this.theRelResolver.IsCompleteLoaded)
                {
                    this.theRelResolver.CompleteLoad();
                    //System.Collections.Hashtable relationChangements=new System.Collections.Hashtable(RelationChangements);

                    //In lazy fatching collections can already exist object, which added before the complete load of related objects.
                    //Also deleted objects does not exist in related objects collection but system didn't know that 
                    //before the complete load of related objects.

                    foreach (RelationChange relationChangement in GetAllRelationChanges())
                    {
                        //RelationChangement relationChangement=entry.Value as RelationChangement;
                        bool exist = theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object);
                        if (!relationChangement.ChangeApplied && relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && !exist)
                            RemoveRelationChange(relationChangement);

                        if (!relationChangement.ChangeApplied && relationChangement.TypeOfChange == RelationChange.ChangeType.Added && exist)
                            RemoveRelationChange(relationChangement);
                    }
                }
                #endregion

                if (GetRelationChanges(Transactions.Transaction.Current).Length == 0)
                    return theRelResolver.LoadedRelatedObjects.GetEnumerator();

                System.Collections.ArrayList relatedObjects = new System.Collections.ArrayList(theRelResolver.LoadedRelatedObjects);

                //Take in it count the relation changes which are not saved yet.
                foreach (RelationChange relationChangement in GetFinalRelationChanges(Transactions.Transaction.Current))
                {
                    //RelationChangement relationChangement=entry.Value as RelationChangement;
                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                        relatedObjects.Remove(relationChangement.Object);
                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && !theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                        relatedObjects.Add(relationChangement.Object);
                }
                return relatedObjects.GetEnumerator();
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }

        }

        /// <MetaDataID>{6D332B0E-8ED1-4380-AD1F-943F11B8F986}</MetaDataID>
        /// <summary>
        /// This method implement the operation Contains
        /// </summary>
        public virtual bool Contains(object theObject)
        {
            if (theObject == null)
                return false;


            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                RelationChange relationChangement = GetFinalRelationChange(theObject, Transactions.Transaction.Current);

                if (relationChangement != null)
                {
                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded)
                        return false;
                    else
                        return true;
                }
                return theRelResolver.Contains(theObject);

            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }


        }

        /// <MetaDataID>{18AB552B-E09E-49A0-89D2-DD64B263A577}</MetaDataID>
        /// <summary>This method retrieves the related objects which have 
        /// the criteria of criterion parameter.
        /// In case where we add or remove objects in a transaction context and transaction is open, 
        /// the persistency system ignores these movements. </summary>
        /// <param name="criterion">This parameter defines the criteria which must fulfill the related object.
        /// If the criterion parameter is null or zero length string then the method return all 
        /// related objects. </param>
        public PersistenceLayer.ObjectCollection GetObjects(string criterion)
        {

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                PersistenceLayerRunTime.OnMemoryObjectCollection objects = new OnMemoryObjectCollection();
                foreach (object _object in theRelResolver.Load(criterion))
                {
                    RelationChange relationChangement = GetFinalRelationChange(_object, Transactions.Transaction.Current);
                    if (relationChangement != null && relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded)
                        continue;
                    objects.Add(_object);
                }
                return objects;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }



        /// <MetaDataID>{232480E9-E695-4B6C-BBBC-9A2646CCC35C}</MetaDataID>
        /// <summary>This method implements the MarkChanges operation of TransactionalObject interface. 
        /// The transaction subsystem informs the collection object for the beginning of a transaction. 
        /// All object additions and deletions marked to the transaction of parameter if client code runs 
        /// under the same transaction. If the transaction abort the collection cancel t
        /// he changes which is register to the transaction of parameter. </summary>
        /// <param name="transaction">This parameter defines a transaction object. 
        /// For this transaction the collection will be mark the changes. </param>
        public virtual void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            foreach (RelationChange relationChange in GetRelationChanges(null))
                AddRelationChange(relationChange, transaction);

        }

        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            throw new System.NotImplementedException();
        }


        /// <MetaDataID>{9E61FA3F-C4AC-4EF2-91A6-E77377533DA5}</MetaDataID>
        public void MergeChanges(OOAdvantech.Transactions.Transaction mergeInTransaction, OOAdvantech.Transactions.Transaction mergedTransaction)
        {
            foreach (RelationChange relationChange in GetRelationChanges(mergedTransaction))
            {
                RelationChange masterTransactionRelationChange = GetRelationChange(relationChange.Object, mergeInTransaction); ;
                if (masterTransactionRelationChange == null)
                {
                    AddRelationChange(relationChange, mergeInTransaction);
                    continue;
                }
                if (masterTransactionRelationChange.ChangeApplied || relationChange.ChangeApplied)
                    throw new System.Exception("The functionality is not implemented.");
                if (masterTransactionRelationChange.TypeOfChange == relationChange.TypeOfChange)
                    continue;

                if (masterTransactionRelationChange.TypeOfChange != relationChange.TypeOfChange)
                    masterTransactionRelationChange.TypeOfChange = relationChange.TypeOfChange;



            }
        }




        /// <MetaDataID>{772DE49C-439A-46B8-B467-0568DF1C35E8}</MetaDataID>
        /// <summary>This method implements the UndoChanges operation of TransactionalObject interface. 
        /// The transaction subsystem informs the collection object for the transaction roll back. 
        /// The collection cancels all changes which are made in the transaction context of 
        /// transaction parameter. </summary>
        /// <param name="transaction">This parameter defines a transaction object. 
        /// The collection cancels all changes which belong to this transaction. </param>
        public virtual void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            if (theRelResolver.AssociationEnd.Name == "OwnedElements")
            {
                int stop = 0;
            }


            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                foreach (RelationChange relationChangement in GetRelationChanges(transaction))
                {
                    //RelationChangement relationChangement=entry.Value as RelationChangement;
                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && relationChangement.ChangeApplied)
                        theRelResolver.LoadedRelatedObjects.Remove(relationChangement.Object);

                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && relationChangement.ChangeApplied)
                        theRelResolver.LoadedRelatedObjects.Add(relationChangement.Object);
                }
                ClearRelationChangements(transaction);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <MetaDataID>{358CC8B3-2C8B-42F1-B2DE-09E762EEFC7F}</MetaDataID>
        /// <summary>This method implements the CommitChanges operation of TransactionalObject interface. 
        /// The transaction subsystem informs the collection object for the transaction commit. </summary>
        /// <param name="transaction">This parameter defines a transaction object. </param>
        public virtual void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {

            if (theRelResolver.AssociationEnd.Name == "JoinedTables")
            {
                int stop = 0;
            }

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                foreach (RelationChange relationChangement in GetRelationChanges(transaction))
                {
                    //RelationChangement relationChangement = entry.Value as RelationChangement;
                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added &&
                        !relationChangement.ChangeApplied &&
                        !theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                    {
                        if(relationChangement.Index==-1)
                            theRelResolver.LoadedRelatedObjects.Add(relationChangement.Object);
                        else
                            theRelResolver.LoadedRelatedObjects.Insert(relationChangement.Index,relationChangement.Object);
                    }

                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded &&
                        !relationChangement.ChangeApplied &&
                        theRelResolver.LoadedRelatedObjects.Contains(relationChangement.Object))
                    {
                        theRelResolver.LoadedRelatedObjects.Remove(relationChangement.Object);
                    }
                }
                foreach (RelationChange relationChange in GetRelationChanges(transaction))
                {
                    if (relationChange.TransactionFree)
                        RemoveRelationChange(relationChange);
                }
                ClearRelationChangements(transaction);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }


        /// <MetaDataID>{EAD6F409-68BD-41FE-A406-36A397182886}</MetaDataID>
        void RemoveRelationChange(RelationChange relationChangement)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, Collections.Generic.Dictionary<object, RelationChange>> entry in RelationChanges)
            {
                if (entry.Value.ContainsKey(relationChangement.Object))
                {
                    entry.Value.Remove(relationChangement.Object);
                    break;
                }
            }

        }
        /// <MetaDataID>{42A91A4E-D04B-4A28-9C56-416F8EE1A17B}</MetaDataID>
        void AddRelationChange(RelationChange relationChangement, Transactions.Transaction transaction)
        {
            string localTransactionUri = "null_transaction";
            if (transaction != null)
                localTransactionUri = transaction.LocalTransactionUri;
            if (!RelationChanges.ContainsKey(localTransactionUri))
                RelationChanges[localTransactionUri] = new Collections.Generic.Dictionary<object, RelationChange>();
            if (RelationChanges[localTransactionUri].ContainsKey(relationChangement.Object))
                throw new System.Exception("There is already relstion change");
            RelationChanges[localTransactionUri][relationChangement.Object] = relationChangement;
        }


        /// <MetaDataID>{90FF458E-8C4A-4494-8F83-48AE3C22AC06}</MetaDataID>
        RelationChange[] GetRelationChanges(Transactions.Transaction transaction)
        {
            string localTransactionUri = "null_transaction";
            if (transaction != null)
                localTransactionUri = transaction.LocalTransactionUri;
            if (!RelationChanges.ContainsKey(localTransactionUri))
                return new RelationChange[0];

            Collections.Generic.Dictionary<object, RelationChange> relationChanges = RelationChanges[localTransactionUri];
            RelationChange[] tempRelationChanges = null;
            if (relationChanges == null)
                return new RelationChange[0];
            tempRelationChanges = new RelationChange[relationChanges.Count];
            int i = 0;

            foreach (System.Collections.Generic.KeyValuePair<object, RelationChange> entry in relationChanges)
                tempRelationChanges[i++] = entry.Value;
            return tempRelationChanges;

        }
        /// <MetaDataID>{F318401D-80F7-4DEE-BB4C-11EF4BB970E6}</MetaDataID>
        /// <summary>
        ///This method returns the finals relation changes for transaction. 
        ///In case where the transaction is nested then the method takes in its account, 
        ///the relation changes for each object of transaction chain. 
        ///For instance if for there are two relation changes for one object, 
        ///the method takes the change of last transaction in transaction chain. 
        /// </summary>
        RelationChange[] GetFinalRelationChanges(Transactions.Transaction transaction)
        {
            Collections.Generic.List<RelationChange> allRelationChanges = new OOAdvantech.Collections.Generic.List<RelationChange>();
            string localTransactionUri = "null_transaction";
            if (transaction != null && transaction.OriginTransaction != null)
            {
                RelationChange[] masterTransactionRelelationChanges = GetFinalRelationChanges(transaction.OriginTransaction);
                localTransactionUri = transaction.LocalTransactionUri;
                Collections.Generic.Dictionary<object, RelationChange> transactionRelelationChanges =null;
                if( RelationChanges.ContainsKey(localTransactionUri))
                    transactionRelelationChanges = RelationChanges[localTransactionUri];
                foreach (RelationChange relationChange in masterTransactionRelelationChanges)
                {
                    if (!transactionRelelationChanges.ContainsKey(relationChange.Object))
                        allRelationChanges.Add(relationChange);
                }
                foreach (System.Collections.Generic.KeyValuePair<object, RelationChange> entry in transactionRelelationChanges)
                    allRelationChanges.Add(entry.Value);

                RelationChange[] tempRelationChanges = new RelationChange[allRelationChanges.Count];
                int i = 0;
                foreach (RelationChange relationChange in allRelationChanges)
                    tempRelationChanges[i++] = relationChange;
                return tempRelationChanges;
            }
            else
            {
                return GetRelationChanges(transaction);
            }

        }
        /// <MetaDataID>{2C477570-771B-4B14-8FA2-E130ACC1B4C3}</MetaDataID>
        /// <summary>
        ///This method returns the final relation change for object in transaction. 
        ///In case where the transaction is nested then the method takes in its account, 
        ///the relation changes for the object in transaction chain. 
        ///For instance if for there are two relation changes, 
        ///the method takes the change of last transaction in transaction chain. 
        /// </summary>
        RelationChange GetFinalRelationChange(object anObject, Transactions.Transaction transaction)
        {
            RelationChange relationChange = GetRelationChange(anObject, transaction);
            if (transaction != null && relationChange == null && transaction.OriginTransaction != null)
                return GetFinalRelationChange(anObject, transaction.OriginTransaction);
            return relationChange;
        }



        /// <MetaDataID>{51443C59-394B-4716-8D81-251AD2E4DB3E}</MetaDataID>
        RelationChange[] GetAllRelationChanges()
        {
            int relationChangesCount = 0;

            foreach (System.Collections.Generic.KeyValuePair<string, Collections.Generic.Dictionary<object, RelationChange>> entry in RelationChanges)
                relationChangesCount += entry.Value.Count;
            RelationChange[] tempRelationChanges = new RelationChange[relationChangesCount];

            int i = 0;

            foreach (System.Collections.Generic.KeyValuePair<string, Collections.Generic.Dictionary<object, RelationChange>> entry in RelationChanges)
            {
                Collections.Generic.Dictionary<object, RelationChange> relationChanges = entry.Value;
                foreach (System.Collections.Generic.KeyValuePair<object, RelationChange> relationChangeEntry in relationChanges)
                    tempRelationChanges[i++] = relationChangeEntry.Value;
            }
            return tempRelationChanges;
        }

        /// <MetaDataID>{CE1EE62D-FFCB-4623-81EA-E29CECCFFD5E}</MetaDataID>
        void ClearRelationChangements(Transactions.Transaction transaction)
        {
            string localTransactionUri = "null_transaction";
            if (transaction != null)
                localTransactionUri = transaction.LocalTransactionUri;
            if (RelationChanges.ContainsKey(localTransactionUri))
            {
                RelationChanges.Remove(localTransactionUri);
            }

        }
        /// <MetaDataID>{BE4383D6-C6E3-4E60-B9EA-06EA0F9E8AF8}</MetaDataID>
        RelationChange GetRelationChange(object anObject, Transactions.Transaction transaction)
        {
            string localTransactionUri = "null_transaction";
            if (transaction != null)
                localTransactionUri = transaction.LocalTransactionUri;
            if (RelationChanges.ContainsKey(localTransactionUri))
            {
                if (RelationChanges[localTransactionUri].ContainsKey(anObject))
                    return RelationChanges[localTransactionUri][anObject];
                else
                    return null;
            }
            else
                return null;
        }



        /// <MetaDataID>{A43BD5E3-AE8D-427E-9574-F77B1ECCF1F4}</MetaDataID>
        public virtual void AddObjects(PersistenceLayer.ObjectCollection objects)
        {
            foreach (object currObject in objects)
                Add(currObject);

        }
        /// <MetaDataID>{BEB9FAD9-E649-4BEF-BA91-58B2CABF82CF}</MetaDataID>
        public virtual void RemoveAll()
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                theRelResolver.CompleteLoad();
                foreach (object mObject in theRelResolver.LoadedRelatedObjects)
                    Remove(mObject);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }



        /// <summary>This method implements the operation RemoveObjects of interface ObjectCollection.
        /// Removes objects massively. </summary>
        /// <param name="objects">This parameter defines the collection of objects which will be removed. </param>
        /// <MetaDataID>{9D57AD5A-725D-4D03-826A-807E2C564E4F}</MetaDataID>
        public virtual void RemoveObjects(PersistenceLayer.ObjectCollection objects)
        {
            foreach (object currObject in objects)
                Remove(currObject);
        }

        /// <MetaDataID>{D629F64B-DA47-4E63-B46B-11A79302B144}</MetaDataID>
        public bool HasChanges
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (GetFinalRelationChanges(Transactions.Transaction.Current).Length > 0)
                        return true;
                    else
                        return false;

                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
        }
        /// <MetaDataID>{EBCB1FB1-3ADF-48C0-B150-30FB3C1097A3}</MetaDataID>
        public void MakeChangesCommands()
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                foreach (RelationChange relationChangement in GetRelationChanges(Transactions.Transaction.Current))
                {

                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Added && !relationChangement.ProducedFromRT && !relationChangement.ChangeApplied)
                        theRelResolver.LinkObject(relationChangement.Object,relationChangement.Index, out relationChangement.ChangeApplied);

                    if (relationChangement.TypeOfChange == RelationChange.ChangeType.Deleteded && !relationChangement.ProducedFromRT && !relationChangement.ChangeApplied)
                        theRelResolver.UnLinkObject(relationChangement.Object, out relationChangement.ChangeApplied);
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }

        /// <summary></summary>
        /// <MetaDataID>{BA38D7DB-DF50-4C38-BD68-AF86FF1C6293}</MetaDataID>
        public ObjectsOfManyRelationsipp(RelResolver relResolver)
        {
            if (relResolver == null)
                throw new System.ArgumentNullException("Parameter value must be not null", "relResolver");
            theRelResolver = relResolver;
            if (theRelResolver.AssociationEnd.Association.LinkClass != null)
                ObjectCollectionType = theRelResolver.AssociationEnd.Association.LinkClass.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
            else
                ObjectCollectionType = theRelResolver.AssociationEnd.SpecificationType;//.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
            if (ObjectCollectionType == null)
                throw new System.Exception("Meta data error");

        }
        /// <MetaDataID>{1A660DB2-097B-4CB6-9903-B8BB02606A09}</MetaDataID>

        private System.Type ObjectCollectionType;
        /// <summary></summary>
        /// <MetaDataID>{B4B1A18E-46DE-4315-B5D4-FB5F84FEFF12}</MetaDataID>
        public readonly PersistenceLayerRunTime.RelResolver theRelResolver;





        /// <MetaDataID>{2510A949-1BEF-4E4B-B472-BC174CA25890}</MetaDataID>
        /// <summary>With this method we remove an object from the collection of the related objects. </summary>
        /// <param name="anObject">This parameter defines the object, which we want to remove from the related objects collection. </param>
        public virtual void Remove(object anObject)
        {
            Remove(anObject, false);
        }

        /// <MetaDataID>{45B0642F-DE8E-4749-817F-447720EFD3D9}</MetaDataID>
        /// <summary>With this method we add an object to the collection of the related objects. </summary>
        /// <param name="anObject">This parameter defines the object, which we want to add to the related objects collection. </param>
        public virtual void Add(object anObject)
        {
            Add(anObject, false);
        }
    }
}
