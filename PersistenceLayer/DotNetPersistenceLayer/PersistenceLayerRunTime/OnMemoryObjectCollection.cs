namespace OOAdvantech.PersistenceLayerRunTime
{



    using CollectionChangeCollection = Collections.Generic.Dictionary<string, Collections.Generic.Dictionary<object, CollectionChange>>;
    using System.Linq;
    using System.Reflection;
    using System.Linq.Expressions;
    using System;
    using Transactions;
    using System.Collections;
    using System.Globalization;
    using OOAdvantech.Collections.Generic;

    /// <MetaDataID>{0D089B4A-C4B0-44DC-A7C7-57952E122DC0}</MetaDataID>
    [System.Serializable]
    public class OnMemoryObjectCollection : PersistenceLayer.ObjectCollection, ICollectionMember, IMemberInitialization, IndexedCollection, IMultilingualMember
    {
        /// <MetaDataID>{d079516e-c880-4a47-9284-9e5485d48a4c}</MetaDataID>
        public System.Linq.IQueryable QueryableCollection
        {
            get
            {


                //IQueryable queryable = null;// storage.GetObjectCollection(typeof(IClient));



                if (RelResolver == null)
                    return null;
                PersistenceLayer.ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(Owner);
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

                IQueryable queryable = storage.GetObjectCollection((RelResolver.AssociationEnd.Namespace as MetaDataRepository.Classifier).GetExtensionMetaObject<System.Type>());
                MemberInfo collectionMember = null;
                if ((RelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd).FieldMember != null)
                    collectionMember = (RelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd).FieldMember;
                else
                    collectionMember = (RelResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd).PropertyMember;

                System.Type collectionItemType = RelResolver.AssociationEnd.Specification.GetExtensionMetaObject<System.Type>();
                ParameterExpression collectionOwner = Expression.Parameter(queryable.ElementType, "collectionOwner");
                ParameterExpression collectionItem = Expression.Parameter(collectionItemType, "collectionItem");
                MemberExpression memberAccess = Expression.MakeMemberAccess(collectionOwner, collectionMember);
                // ***** Where(client => (client == this )) *****
                Expression left = collectionOwner;
                Expression right = Expression.Constant(Owner);
                Expression equal = Expression.Equal(left, right);
                MethodCallExpression whereCallExpression = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new System.Type[] { queryable.ElementType },
                    queryable.Expression,
                    Expression.Lambda(typeof(System.Func<,>).MakeGenericType(new System.Type[] { queryable.ElementType, typeof(bool) }), equal, new ParameterExpression[] { collectionOwner }));
                MethodInfo createQueryMethod = null;

                MethodCallExpression selectCallExpression = Expression.Call(
                    typeof(Queryable),
                    "SelectMany",
                    new System.Type[] { queryable.ElementType, collectionItemType, collectionItemType }, whereCallExpression,
                    Expression.Lambda(typeof(System.Func<,>).MakeGenericType(new System.Type[] { queryable.ElementType, typeof(System.Collections.Generic.IEnumerable<>).MakeGenericType(new System.Type[] { collectionItemType }) }), memberAccess, new ParameterExpression[] { collectionOwner }),
                    Expression.Lambda(typeof(System.Func<,,>).MakeGenericType(new System.Type[] { queryable.ElementType, collectionItemType, collectionItemType }), collectionItem, new ParameterExpression[] { collectionOwner, collectionItem }));
                foreach (MethodInfo methodInfo in queryable.Provider.GetType().GetMetaData().GetMethods())
                {
                    if (methodInfo.Name == "CreateQuery" && methodInfo.ReturnType.Name == typeof(System.Linq.IQueryable<>).Name)
                    {
                        createQueryMethod = methodInfo;
                        break;
                    }
                }
                return (IQueryable)createQueryMethod.MakeGenericMethod(collectionItemType).Invoke(queryable.Provider, new object[1] { selectCallExpression });

            }
        }


        public void CheckIndexes()
        {
            if (!HasChanges)
                return;

            System.Collections.IList relatedObjects = null;
            string localTransactionUri = "null_transaction";
            if (Transactions.Transaction.Current != null)
                localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;

            relatedObjects = new Collections.Generic.List<object>(ContainObjects.OfType<object>());
            System.Collections.Generic.List<CollectionChange> collectionChanges = GetFinalCollectionChanges(Transactions.Transaction.Current);

            if (collectionChanges == null && collectionChanges.Count == 0)
                return;
            foreach (CollectionChange collectionChangement in collectionChanges)
            {
                //Object removed
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && relatedObjects.Contains(collectionChangement.Object))
                    relatedObjects.Remove(collectionChangement.Object);
                //object in new position
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index != -1)
                    relatedObjects.Remove(collectionChangement.Object);
            }
            collectionChanges.Sort(new CollectionChangesSort());
            foreach (CollectionChange collectionChangement in collectionChanges.OrderBy(x => x.Index))
            {
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index != -1)
                {
                    if (collectionChangement.Index >= relatedObjects.Count)
                        relatedObjects.Add(collectionChangement.Object);
                    else
                        relatedObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                }
            }
            foreach (CollectionChange collectionChangement in collectionChanges)
            {
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index == -1)
                    relatedObjects.Add(collectionChangement.Object);
            }
            foreach (CollectionChange collectionChangement in collectionChanges)
            {
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && collectionChangement.Index != -1 && relatedObjects.IndexOf(collectionChangement.Object) != collectionChangement.Index)
                {

                }
            }
        }

        /// <MetaDataID>{b7d6364f-ebac-4189-8c6b-cd7e79cea1a3}</MetaDataID>
        public bool CanDeletePermanently(object theObject)
        {


            if (RelResolver != null)
            {
                if (!Contains(theObject))
                    return false;
                return RelResolver.CanDeletePermanently(theObject);
            }
            return false;
        }


        /// <MetaDataID>{3e19971b-cd95-4bec-a92d-4e5fcdb2c22f}</MetaDataID>
        public OnMemoryObjectCollection(bool multilingual = false)
        {
            Multilingual = multilingual;
        }
        /// <MetaDataID>{016217cf-77ba-4e70-911c-46000b00c698}</MetaDataID>
        private System.Type ObjectCollectionType;
        /// <MetaDataID>{54b19c72-18e2-4c26-9eb0-ddd481384eb8}</MetaDataID>
        public OnMemoryObjectCollection(RelResolver relResolver, bool multilingual = false)
        {

            Multilingual = multilingual;

            if (relResolver == null)
                throw new System.ArgumentNullException("Parameter value must be not null", "relResolver");
            _RelResolver = relResolver;
            if (_RelResolver.AssociationEnd.Association.LinkClass != null)
                ObjectCollectionType = _RelResolver.AssociationEnd.Association.LinkClass.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
            else
                ObjectCollectionType = _RelResolver.AssociationEnd.SpecificationType;//.Specification.GetExtensionMetaObject(typeof(System.Type)) as System.Type;
            if (ObjectCollectionType == null)
                throw new System.Exception("Meta data error");
            _Owner = _RelResolver.Owner.MemoryInstance;
            MetaObject = _RelResolver.AssociationEnd;

        }
        bool Multilingual;
        /// <MetaDataID>{de5a7b0f-60c1-4e9b-a86e-7315cb85f6ed}</MetaDataID>
        public OnMemoryObjectCollection(System.Collections.ICollection collection, bool multilingual = false)
        {
            Multilingual = multilingual;
            _TransientObjects = new OOAdvantech.Collections.Generic.List<object>();
            foreach (object obj in collection)
                TransientObjects.Add(obj);
        }

        ///// <MetaDataID>{0e20b5ac-1b2b-4761-9b43-2387d40d3fa8}</MetaDataID>
        //public bool HasChanges
        //{
        //    get
        //    {

        //        ReaderWriterLock.AcquireReaderLock(10000);
        //        try
        //        {
        //            if (GetFinalCollectionChanges(Transactions.Transaction.Current).Count > 0)
        //                return true;
        //            else
        //                return false;

        //        }
        //        finally
        //        {
        //            ReaderWriterLock.ReleaseReaderLock();
        //        }

        //    }
        //}

        /// <MetaDataID>{85264b95-f8bb-4e1d-9a63-f2a6bfcbbc67}</MetaDataID>
        internal System.Collections.Generic.List<ObjectsLink> GetRelationChanges()
        {
            System.Collections.Generic.List<ObjectsLink> objectsLinks = new System.Collections.Generic.List<ObjectsLink>();
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (TransientObjects.Count > 0)
                    foreach (object _object in TransientObjects)
                        Add(_object);


                System.Collections.Generic.ICollection<CollectionChange> collectionChanges = GetCollectionChanges(Transactions.Transaction.Current);
                if (collectionChanges != null)
                {
                    foreach (CollectionChange relationChangement in collectionChanges)
                    {
                        StorageInstanceAgent linkedObjectStorageInstanceRef = StorageInstanceRef.GetStorageInstanceAgent(relationChangement.Object);
                        if (linkedObjectStorageInstanceRef != null)
                        {

                            if (relationChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relationChangement.Implicitly && !relationChangement.ChangeApplied)
                            {
                                if (RelResolver.AssociationEnd.Association.LinkClass != null && linkedObjectStorageInstanceRef.RealStorageInstanceRef.Class.ClassHierarchyLinkAssociation == RelResolver.AssociationEnd.Association)
                                {
                                    ///TODO να γραφτεί Test case που το relation object θα είναι out of process 
                                    object roleA = Member<object>.GetValue(linkedObjectStorageInstanceRef.RealStorageInstanceRef.Class.LinkClassRoleAFastFieldAccessor.GetValue, linkedObjectStorageInstanceRef.RealStorageInstanceRef.MemoryInstance);
                                    object roleB = Member<object>.GetValue(linkedObjectStorageInstanceRef.RealStorageInstanceRef.Class.LinkClassRoleBFastFieldAccessor.GetValue, linkedObjectStorageInstanceRef.RealStorageInstanceRef.MemoryInstance);
                                    objectsLinks.Add(new ObjectsLink(RelResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(roleA), StorageInstanceRef.GetStorageInstanceAgent(roleB), linkedObjectStorageInstanceRef, ObjectsLink.TypeOfChange.Added));
                                }
                                else if (RelResolver.AssociationEnd.IsRoleA)
                                    objectsLinks.Add(new ObjectsLink(RelResolver.AssociationEnd.Association, linkedObjectStorageInstanceRef, StorageInstanceRef.GetStorageInstanceAgent(Owner), ObjectsLink.TypeOfChange.Added));
                                else
                                    objectsLinks.Add(new ObjectsLink(RelResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(Owner), linkedObjectStorageInstanceRef, ObjectsLink.TypeOfChange.Added));

                            }
                            if (relationChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && !relationChangement.Implicitly && !relationChangement.ChangeApplied)
                            {
                                if (RelResolver.AssociationEnd.Association.LinkClass != null && linkedObjectStorageInstanceRef.RealStorageInstanceRef.Class.ClassHierarchyLinkAssociation == RelResolver.AssociationEnd.Association)
                                {
                                    ///TODO να γραφτεί Test case που το relation object θα είναι out of process
                                    object roleA = Member<object>.GetValue(linkedObjectStorageInstanceRef.RealStorageInstanceRef.Class.LinkClassRoleAFastFieldAccessor.GetValue, linkedObjectStorageInstanceRef.RealStorageInstanceRef.MemoryInstance);
                                    object roleB = Member<object>.GetValue(linkedObjectStorageInstanceRef.RealStorageInstanceRef.Class.LinkClassRoleBFastFieldAccessor.GetValue, linkedObjectStorageInstanceRef.RealStorageInstanceRef.MemoryInstance);
                                    objectsLinks.Add(new ObjectsLink(RelResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(roleA), StorageInstanceRef.GetStorageInstanceAgent(roleB), linkedObjectStorageInstanceRef, ObjectsLink.TypeOfChange.Removed));
                                }
                                else if (RelResolver.AssociationEnd.IsRoleA)
                                    objectsLinks.Add(new ObjectsLink(RelResolver.AssociationEnd.Association, linkedObjectStorageInstanceRef, StorageInstanceRef.GetStorageInstanceAgent(Owner), ObjectsLink.TypeOfChange.Removed));
                                else
                                    objectsLinks.Add(new ObjectsLink(RelResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(Owner), linkedObjectStorageInstanceRef, ObjectsLink.TypeOfChange.Removed));
                            }
                        }
                    }
                }
                return objectsLinks;


            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }

        public void MakeMultilingualChangesCommands()
        {

        }

        void InternalMultilingualMakeChangesCommands()
        {
            foreach (var culture in MultilingualCollectionChanges.Keys)
            {
                using (var cultureConext = new OOAdvantech.CultureContext(culture, false))
                {
                    InternalMakeChangesCommands();
                }
            }
        }
        public void MakeChangesCommands()
        {
            if (Multilingual)
                InternalMultilingualMakeChangesCommands();
            else
                InternalMakeChangesCommands();
        }
        public bool ObjectHasGhanged(TransactionRunTime transaction)
        {
            if (Multilingual)
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    foreach (var culture in MultilingualCollectionChanges.Keys)
                    {
                        using (var cultureConext = new OOAdvantech.CultureContext(culture, false))
                        {
                            var collectionChanges = GetCollectionChanges(Transactions.Transaction.Current);
                            if (collectionChanges == null || collectionChanges.Count == 0)
                                return false;
                            else
                                return true;
                        }
                    }
                    return false;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
            else
            {
                try
                {
                    ReaderWriterLock.AcquireReaderLock(10000);
                    var collectionChanges = GetCollectionChanges(Transactions.Transaction.Current);
                    if (collectionChanges == null || collectionChanges.Count == 0)
                        return false;
                    else
                        return true;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }


        }
        /// <MetaDataID>{ff9e52c5-a1f2-465b-931d-be0866914a0a}</MetaDataID>
        void InternalMakeChangesCommands()
        {

            if (RelResolver != null)
            {
                if (RelResolver.Owner.PersistentObjectID != null && RelResolver.Owner.PersistentObjectID.ToString() == "2022")
                {
                }
            }
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (RelResolver.AssociationEnd.Indexer)
                {
                    System.Collections.Generic.List<CollectionChange> collectionChanges = null;
                    var collection = GetCollectionChanges(Transactions.Transaction.Current);
                    if (collection != null)
                        collectionChanges = new System.Collections.Generic.List<CollectionChange>(collection);
                    if (collectionChanges != null && collectionChanges.Count > 0)
                    {
                        bool indirectIndexCalculation = true;
                        if (RelResolver.IsCompleteLoaded)
                            indirectIndexCalculation = false;
                        else
                        {
                            foreach (var collectionChange in collectionChanges)
                            {
                                if (!collectionChange.Implicitly)
                                    indirectIndexCalculation = false;
                            }
                        }
                        if (indirectIndexCalculation)
                        {
                            IndirectIndexRebuild(collectionChanges);
                        }
                        else
                        {


                            System.Collections.IList objectsUnderCurrentTransaction = ObjectsUnderCurrentTransaction;
                            if (!TransactionContext.CurrentTransactionContext.ContainCommand(Commands.BuildContainedObjectIndicies.GetIdentity(this)))
                                (RelResolver.Owner.ObjectStorage as ObjectStorage).CreateBuildContainedObjectIndiciesCommand(this);

                            foreach (CollectionChange relationChangement in collectionChanges)
                            {
                                if (relationChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && !relationChangement.Implicitly && !relationChangement.ChangeApplied)
                                    relationChangement.Index = RelResolver.LoadedRelatedObjects.IndexOf(relationChangement.Object);
                            }
                            foreach (CollectionChange relationChangement in collectionChanges)
                            {
                                if (relationChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && !relationChangement.Implicitly && !relationChangement.ChangeApplied)
                                    RelResolver.UnLinkObject(relationChangement.Object, relationChangement.Index, out relationChangement.ChangeApplied);
                                if (relationChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relationChangement.Implicitly && !relationChangement.ChangeApplied)
                                    relationChangement.Index = objectsUnderCurrentTransaction.IndexOf(relationChangement.Object);
                            }
                            collectionChanges.Sort(new CollectionChangesSort());
                            foreach (CollectionChange relationChangement in collectionChanges)
                            {
                                if (relationChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relationChangement.Implicitly && !relationChangement.ChangeApplied)
                                    RelResolver.LinkObject(relationChangement.Object, relationChangement.Index, out relationChangement.ChangeApplied);
                            }
                        }
                    }
                }

                else
                {
                    System.Collections.Generic.ICollection<CollectionChange> collectionChanges = GetCollectionChanges(Transactions.Transaction.Current);
                    if (collectionChanges != null)
                    {
                        foreach (CollectionChange relationChangement in collectionChanges)
                        {

                            if (relationChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relationChangement.Implicitly && !relationChangement.ChangeApplied)
                            {
                                //if (RelResolver.IsCompleteLoaded && relationChangement.Index == -1)
                                //    relationChangement.Index = IndexOf(relationChangement.Object);
                                RelResolver.LinkObject(relationChangement.Object, relationChangement.Index, out relationChangement.ChangeApplied);
                            }

                            if (relationChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && !relationChangement.Implicitly && !relationChangement.ChangeApplied)
                                RelResolver.UnLinkObject(relationChangement.Object, relationChangement.Index, out relationChangement.ChangeApplied);
                        }
                    }
                }

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);

                if (RelResolver != null)
                {
                    if (RelResolver.Owner.PersistentObjectID != null && RelResolver.Owner.PersistentObjectID.ToString() == "2022")
                    {

                    }
                }
            }

        }

        /// <summary>
        /// This rebuild the index when the related objects doesn't complete loaded
        /// </summary>
        /// <param name="collectionChanges">
        /// Defines the changes of collection under current transaction
        /// </param>
        private void IndirectIndexRebuild(System.Collections.Generic.List<CollectionChange> collectionChanges)
        {
            foreach (var collectionChange in collectionChanges)
            {
                if (collectionChange.TypeOfChange == CollectionChange.ChangeType.Added && collectionChange.Implicitly && collectionChange.Index == -1)
                    collectionChange.Index = NextIndex++;
            }

            foreach (var collectionChange in collectionChanges)
            {
                if (collectionChange.TypeOfChange == CollectionChange.ChangeType.Deleteded && collectionChange.Implicitly && collectionChange.Index == -1)
                {
                    collectionChange.Index = RelResolver.IndexOf(collectionChange.Object);

                    if (!TransactionContext.CurrentTransactionContext.ContainCommand(Commands.BuildContainedObjectIndicies.GetIdentity(this)))
                        (RelResolver.Owner.ObjectStorage as ObjectStorage).CreateBuildContainedObjectIndiciesCommand(this);

                    foreach (var addCollectionChange in collectionChanges)
                    {
                        if (addCollectionChange.TypeOfChange == CollectionChange.ChangeType.Added && collectionChange.Implicitly)
                        {
                            addCollectionChange.Index--;
                            if (addCollectionChange.ChangeApplied)
                                RelResolver.RefreshLinkedObjectIndex(addCollectionChange.Object, collectionChange.Index);
                        }
                    }
                    NextIndex--;
                }
            }
            foreach (var collectionChange in collectionChanges)
            {
                if (collectionChange.TypeOfChange == CollectionChange.ChangeType.Added && collectionChange.Implicitly && !collectionChange.ChangeApplied)
                    RelResolver.LinkObject(collectionChange.Object, collectionChange.Index, out collectionChange.ChangeApplied);
            }
        }
        /// <MetaDataID>{b1fb001c-4e51-4235-a8fa-2325d053ba49}</MetaDataID>
        [System.NonSerialized]
        OOAdvantech.Transactions.Transaction MakePersistentTransaction;
        /// <MetaDataID>{79885866-0f38-407f-a2bf-9e530d5c370b}</MetaDataID>
        [System.NonSerialized]
        PersistenceLayerRunTime.RelResolver _RelResolver;
        /// <MetaDataID>{8949e955-03bf-431e-adf3-3457e7986d0d}</MetaDataID>
        public PersistenceLayerRunTime.RelResolver RelResolver
        {
            get
            {
                return _RelResolver;
            }
            internal set
            {


                //    #region Get indexChange taransactions
                //    System.Collections.Generic.List<OOAdvantech.Transactions.Transaction> waitForTransactionsComplete = null;

                //CheckIndexChangeTransactions:

                //    ReaderWriterLock.AcquireReaderLock(10000);
                //    try
                //    {
                //        if (TransactionWithIndexChange != null)
                //        {
                //            foreach (OOAdvantech.Transactions.Transaction transaction in TransactionWithIndexChange)
                //            {
                //                if (transaction != OOAdvantech.Transactions.Transaction.Current)
                //                {
                //                    if (waitForTransactionsComplete == null)
                //                        waitForTransactionsComplete = new System.Collections.Generic.List<OOAdvantech.Transactions.Transaction>();
                //                    waitForTransactionsComplete.Add(transaction);
                //                }
                //            }
                //        }
                //    }
                //    finally
                //    {
                //        ReaderWriterLock.ReleaseReaderLock();
                //    }
                //    #endregion

                //    OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                //    #region Lock on index change transactions
                //    if (waitForTransactionsComplete != null && waitForTransactionsComplete.Count > 0)
                //    {
                //        ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                //        foreach (OOAdvantech.Transactions.Transaction transaction in waitForTransactionsComplete)
                //        {
                //            if (!transaction.WaitToComplete(30000))
                //                throw new System.Exception("Time out expired. Collection is locked from other transaction");
                //        }
                //        goto CheckIndexChangeTransactions;
                //    }
                //    #endregion


                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    MakePersistentTransaction = OOAdvantech.Transactions.Transaction.Current;
                    _RelResolver = value;

                    if (Multilingual)
                    {
                        foreach (var cultrure in MultilingualCollectionChanges.Keys)
                        {
                            using (CultureContext cultureContext = new CultureContext(cultrure, false))
                            {
                                foreach (object obj in TransientObjects)
                                {
                                    CollectionChange collectionChangement = GetCollectionChange(obj, Transactions.Transaction.Current);
                                    if (collectionChangement != null && collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded)
                                        RemoveCollectionChange(collectionChangement);
                                    else
                                    {
                                        collectionChangement = new CollectionChange(obj, CollectionChange.ChangeType.Added, false);
                                        AddCollectionChange(collectionChangement, Transactions.Transaction.Current);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TransientObjects != null)
                        {
                            foreach (object obj in TransientObjects)
                            {
                                CollectionChange collectionChangement = GetCollectionChange(obj, Transactions.Transaction.Current);
                                if (collectionChangement != null && collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded)
                                    RemoveCollectionChange(collectionChangement);
                                else
                                {
                                    collectionChangement = new CollectionChange(obj, CollectionChange.ChangeType.Added, false);
                                    AddCollectionChange(collectionChangement, Transactions.Transaction.Current);
                                }
                            }
                        }
                    }

                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }




        /// <MetaDataID>{096dc1ad-7c7e-4637-b497-9848b002b280}</MetaDataID>
        [System.NonSerialized]
        OOAdvantech.Synchronization.ReaderWriterLock _ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();
        /// <MetaDataID>{daf31fc7-4fae-4f1a-8665-e9089471415a}</MetaDataID>
        OOAdvantech.Synchronization.ReaderWriterLock ReaderWriterLock
        {
            get
            {
                if (_ReaderWriterLock == null)
                    _ReaderWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();
                return _ReaderWriterLock;

            }
        }


        /// <MetaDataID>{2a73b292-7430-483c-a93a-cea1d9ed1821}</MetaDataID>
        public int IndexOf(object item)
        {
            if (item == null)
                return -1;
            //if (GetCollectionChanges(Transactions.Transaction.Current).Count == 0)
            //    return ContainObjects.IndexOf(item);
            System.Collections.Generic.List<object> collectionObjects = new System.Collections.Generic.List<object>(ContainObjects.OfType<object>());
            //Take in it count the collection changes which are not saved yet.

            System.Collections.Generic.List<CollectionChange> collectionChanges = GetFinalCollectionChanges(Transactions.Transaction.Current);
            if (collectionChanges == null || collectionChanges.Count == 0)
                return ContainObjects.IndexOf(item);

            collectionChanges.Sort(new CollectionChangesSort());

            foreach (CollectionChange collectionChangement in collectionChanges.Where(x => x.TypeOfChange == CollectionChange.ChangeType.Deleteded))
            {
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && ContainObjects.Contains(collectionChangement.Object))
                    collectionObjects.Remove(collectionChangement.Object);
            }

            foreach (CollectionChange collectionChangement in collectionChanges.Where(x => x.TypeOfChange == CollectionChange.ChangeType.Added && x.Index == -1))
            {
                if (!ContainObjects.Contains(collectionChangement.Object))
                {
                    if (collectionChangement.Index == -1)
                        collectionObjects.Add(collectionChangement.Object);
                    else
                        collectionObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                }
                else if (collectionChangement.Index != -1 && collectionObjects.IndexOf(collectionChangement.Object) != collectionChangement.Index)
                {

                    collectionObjects.Remove(collectionChangement.Object);
                    collectionObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                }
            }

            foreach (CollectionChange collectionChangement in collectionChanges.Where(x => x.TypeOfChange == CollectionChange.ChangeType.Added && x.Index != -1).OrderBy(x => x.Index))
            {
                if (!ContainObjects.Contains(collectionChangement.Object))
                {
                    if (collectionChangement.Index == -1)
                        collectionObjects.Add(collectionChangement.Object);
                    else
                        collectionObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                }
                else if (collectionChangement.Index != -1 && collectionObjects.IndexOf(collectionChangement.Object) != collectionChangement.Index)
                {

                    collectionObjects.Remove(collectionChangement.Object);
                    collectionObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                }
            }

            //foreach (CollectionChange collectionChangement in collectionChanges)
            //{
            //    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && ContainObjects.Contains(collectionChangement.Object))
            //        collectionObjects.Remove(collectionChangement.Object);
            //    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added)
            //    {
            //        if (!ContainObjects.Contains(collectionChangement.Object))
            //        {
            //            if (collectionChangement.Index == -1)
            //                collectionObjects.Add(collectionChangement.Object);
            //            else
            //                collectionObjects.Insert(collectionChangement.Index, collectionChangement.Object);
            //        }
            //        else if (collectionChangement.Index != -1 && collectionObjects.IndexOf(collectionChangement.Object) != collectionChangement.Index)
            //        {

            //            collectionObjects.Remove(collectionChangement.Object);
            //            collectionObjects.Insert(collectionChangement.Index, collectionChangement.Object);
            //        }
            //    }
            //}

            return collectionObjects.IndexOf(item);
        }
        /// <MetaDataID>{0cbe491e-819d-4420-83e1-c3d725122224}</MetaDataID>
        public int IndexOf(object item, int index)
        {
            var collectionChanges = GetCollectionChanges(Transactions.Transaction.Current);
            if (collectionChanges == null || collectionChanges.Count == 0)
                return ContainObjects.IndexOf(item);
            System.Collections.IList collectionObjects = ContainObjects.OfType<object>().ToList();
            //Take in it count the collection changes which are not saved yet.
            foreach (CollectionChange collectionChangement in GetFinalCollectionChanges(Transactions.Transaction.Current))
            {
                //CollectionChangement collectionChangement=entry.Value as CollectionChangement;
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && ContainObjects.Contains(collectionChangement.Object))
                    collectionObjects.Remove(collectionChangement.Object);
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !ContainObjects.Contains(collectionChangement.Object))
                    collectionObjects.Add(collectionChangement.Object);
            }
#if !DeviceDotNet
            return collectionObjects.IndexOf(item);
#else
            return collectionObjects.IndexOf(item);
#endif


        }
        /// <MetaDataID>{a04950df-2ed3-4bb8-80ad-f7b30a2a440f}</MetaDataID>
        public int IndexOf(object item, int index, int count)
        {
            var collectionChanges = GetCollectionChanges(Transactions.Transaction.Current);
            if (collectionChanges == null || collectionChanges.Count == 0)
                return ContainObjects.IndexOf(item);


            System.Collections.IList collectionObjects = ContainObjects.OfType<object>().ToList();
            //Take in it count the collection changes which are not saved yet.
            foreach (CollectionChange collectionChangement in GetFinalCollectionChanges(Transactions.Transaction.Current))
            {
                //CollectionChangement collectionChangement=entry.Value as CollectionChangement;
                if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && ContainObjects.Contains(collectionChangement.Object))
                    collectionObjects.Remove(collectionChangement.Object);
                else if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && collectionChangement.Index != -1)
                {
                    if (collectionChangement.Index < index)
                        return -1;
                    if (collectionChangement.Index > count)
                        return -1;
                    return collectionChangement.Index;
                }
                else if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !ContainObjects.Contains(collectionChangement.Object))
                    collectionObjects.Add(collectionChangement.Object);
            }


            return collectionObjects.IndexOf(item);

        }


        /// <MetaDataID>{AF5A521C-A8BD-4463-A2D7-3038131D8F80}</MetaDataID>
        public PersistenceLayer.ObjectCollection GetObjects(string criterion)
        {
            throw new System.NotSupportedException("This functionality doesn't supported from transient object");
        }
        /// <MetaDataID>{5CBD9A85-2717-42A3-977E-928C0B981E8A}</MetaDataID>
        /// 
        //private OOAdvantech.Collections.Generic.Dictionary<Transactions.Transaction,object> backup=new OOAdvantech.Collections.Generic.Dictionary<OOAdvantech.Transactions.Transaction,object>();

        /// <MetaDataID>{EFA438D1-5372-46ED-8971-058A172A711A}</MetaDataID>
        public virtual void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            if (Multilingual)
            {
                lock (MultilingualCollectionChanges)
                {

                    var currentCultureCollectionChanges = CollectionChanges;//Creates current culture CollectionChanges


                    foreach (var collectionChanges in MultilingualCollectionChanges.Values)
                        collectionChanges[transaction.LocalTransactionUri] = new Collections.Generic.Dictionary<object, CollectionChange>();
                }
            }
            else
            {
                CollectionChanges[transaction.LocalTransactionUri] = new Collections.Generic.Dictionary<object, CollectionChange>();
            }

        }
        /// <MetaDataID>{bbc872b3-74c9-45f5-a95a-65f44d934ed5}</MetaDataID>
        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            throw new System.NotImplementedException();
        }

        public void EnsureSnapshot(Transaction transaction)
        {
            if (Multilingual)
            {
                lock (MultilingualCollectionChanges)
                {
                    foreach (var cultrure in MultilingualCollectionChanges.Keys)
                    {
                        using (CultureContext cultureContext = new CultureContext(cultrure, false))
                        {
                            InternalEnsureSnapshot(transaction);
                        }
                    }
                }
            }
            else
            {
                InternalEnsureSnapshot(transaction);
            }

        }

        private void InternalEnsureSnapshot(Transaction transaction)
        {
            if (!CollectionChanges.ContainsKey(transaction.LocalTransactionUri))
                CollectionChanges[transaction.LocalTransactionUri] = new Collections.Generic.Dictionary<object, CollectionChange>();
        }

        /// <MetaDataID>{d8fc7f43-9e24-4f67-8c6d-4e7298954795}</MetaDataID>
        public void MergeChanges(OOAdvantech.Transactions.Transaction mergeInTransaction, OOAdvantech.Transactions.Transaction mergedTransaction)
        {
            if (Multilingual)
            {
                lock (MultilingualCollectionChanges)
                {
                    foreach (var cultrure in MultilingualCollectionChanges.Keys)
                    {
                        using (CultureContext cultureContext = new CultureContext(cultrure, false))
                        {
                            InternalMergeChanges(mergeInTransaction, mergedTransaction);
                        }
                    }
                }
            }
            else
            {
                InternalMergeChanges(mergeInTransaction, mergedTransaction);
            }

        }

        private void InternalMergeChanges(Transaction mergeInTransaction, Transaction mergedTransaction)
        {
            RelatedObjects = null;
            if (!CollectionChanges.ContainsKey(mergeInTransaction.LocalTransactionUri))
                CollectionChanges[mergeInTransaction.LocalTransactionUri] = new Collections.Generic.Dictionary<object, CollectionChange>();
            var collectionChanges = GetCollectionChanges(mergedTransaction);
            if (collectionChanges == null)
                return;
            foreach (CollectionChange relationChange in collectionChanges)
            {
                CollectionChange masterTransactionRelationChange = GetCollectionChange(relationChange.Object, mergeInTransaction); ;
                if (masterTransactionRelationChange == null)
                {
                    AddCollectionChange(relationChange, mergeInTransaction);
                    continue;
                }
                if (masterTransactionRelationChange.ChangeApplied || relationChange.ChangeApplied)
                    throw new System.Exception("The functionality is not implemented.");
                if (masterTransactionRelationChange.TypeOfChange == relationChange.TypeOfChange)
                {
                    if (relationChange.Index != -1)
                        masterTransactionRelationChange.Index = relationChange.Index;
                    else
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(mergedTransaction))
                        {
                            int index = IndexOf(relationChange.Object);
                            stateTransition.Consistent = true;
                        }

                    }
                    continue;
                }

                if (masterTransactionRelationChange.TypeOfChange != relationChange.TypeOfChange)
                {

                    masterTransactionRelationChange.TypeOfChange = relationChange.TypeOfChange;
                    if (relationChange.Index != -1)
                        masterTransactionRelationChange.Index = relationChange.Index;
                    else
                    {

                        using (SystemStateTransition stateTransition = new SystemStateTransition(mergedTransaction))
                        {
                            int index = IndexOf(relationChange.Object);
                            masterTransactionRelationChange.Index = index;
                            stateTransition.Consistent = true;
                        }

                    }
                }

            }
            RelatedObjects = null;
            ClearCollectionChangements(mergedTransaction);
        }




        ///// <MetaDataID>{876552D2-5281-4825-AB37-28E578AB8E44}</MetaDataID>
        //public virtual void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        //{
        //    if(backup.ContainsKey(transaction))
        //        ContainObjects=backup[transaction] as OOAdvantech.Collections.ArrayList;
        //}
        /// <MetaDataID>{42A863A7-2B32-466B-A7E4-1F20D3D78AA2}</MetaDataID>
        public virtual void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {

            if (Multilingual)
            {
                lock (MultilingualCollectionChanges)
                {
                    foreach (var cultrure in MultilingualCollectionChanges.Keys)
                    {
                        using (CultureContext cultureContext = new CultureContext(cultrure, false))
                        {
                            InternalCommitChanges(transaction);
                        }
                    }
                }
            }
            else
            {
                InternalCommitChanges(transaction);
            }


            //System.Diagnostics.Debug.WriteLine("################# Commit ####################");
        }

        private void InternalCommitChanges(Transaction transaction)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (RelResolver == null)
                {
                    var collectionChanges = GetCollectionChanges(transaction);
                    if (collectionChanges == null)
                        return;
                    foreach (CollectionChange collectionChangement in collectionChanges)
                    {
                        //CollectionChangement collectionChangement = entry.Value as CollectionChangement;
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added)
                        {
                            if (!ContainObjects.Contains(collectionChangement.Object))
                            {
                                if (collectionChangement.Index == -1)
                                {
                                    ContainObjects.Add(collectionChangement.Object);
                                    //System.Diagnostics.Debug.WriteLine("commit Add");
                                }
                                else
                                    ContainObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                            }
                        }

                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded)
                        {
                            if (ContainObjects.Contains(collectionChangement.Object))
                            {
                                ContainObjects.Remove(collectionChangement.Object);
                                //System.Diagnostics.Debug.WriteLine("commit remove");
                            }
                        }
                    }

                }
                else
                {


                    var collection = GetCollectionChanges(transaction);
                    if (collection != null)
                    {
                        System.Collections.Generic.List<CollectionChange> collectionChanges = new System.Collections.Generic.List<CollectionChange>(collection);
                        foreach (CollectionChange collectionChangement in collectionChanges)
                        {
                            if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded)
                            {
                                if (RelResolver.LoadedRelatedObjects.Contains(collectionChangement.Object))
                                    RelResolver.RemoveRelatedObject(collectionChangement.Object);
                            }

                            if (RelResolver.IsCompleteLoaded && collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added)
                            {
                                if (collectionChangement.Index != -1 && collectionChangement.Index != RelResolver.LoadedRelatedObjects.IndexOf(collectionChangement.Object))
                                    RelResolver.RemoveRelatedObject(collectionChangement.Object);
                            }
                        }

                        collectionChanges.Sort(new CollectionChangesSort());

                        foreach (CollectionChange collectionChangement in collectionChanges)
                        {
                            if (RelResolver.IsCompleteLoaded && collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added)
                            {
                                if (!RelResolver.LoadedRelatedObjects.Contains(collectionChangement.Object))
                                {

                                    if (collectionChangement.Index != -1 && collectionChangement.Index <= (RelResolver as ICollection).Count)
                                        RelResolver.InsertRelatedObject(collectionChangement.Index, collectionChangement.Object);
                                    else
                                        RelResolver.AddRelatedObject(collectionChangement.Object);
                                    RelResolver.InvalidateCount();
                                }
                                else if (collectionChangement.Index != -1 && collectionChangement.Index != RelResolver.LoadedRelatedObjects.IndexOf(collectionChangement.Object))
                                {
                                    RelResolver.RemoveRelatedObject(collectionChangement.Object);
                                    RelResolver.InsertRelatedObject(collectionChangement.Index, collectionChangement.Object);
                                }
                            }
                        }
                        RelResolver.InvalidateCount();
                    }
                    TransientObjects.Clear();

                }

                RelatedObjects = null;
                ClearCollectionChangements(transaction);


                #region Code for testing CFT
                if (RelResolver != null && RelResolver.AssociationEnd.FullName == "MenuPresentationModel.MenuCanvas.IRestaurantMenu.MenuCanvasItems")
                {
                    //var storedObjects = RelResolver.GetLinkedObjectsInstorage("");
                    var storedObjects = RelResolver.GetLinkedObjects("");
                    int i = 0;
                    foreach (var obj in storedObjects)
                    {
                        int k = this.IndexOf(obj);
                        if (k != i)
                        {
                        }
                        i++;
                    }
                }
                #endregion
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

            if (RelResolver != null && !OwnerIsPersistent.UnInitialized && !OwnerIsPersistent.Value)
                OwnerIsPersistent.Value = RelResolver.Owner.PersistentObjectID != null;
        }

        /// <MetaDataID>{9cb33003-b271-402d-84f6-c33f7a24dafa}</MetaDataID>
        void ClearCollectionChangements(Transactions.Transaction transaction)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                string localTransactionUri = "null_transaction";
                if (transaction != null)
                    localTransactionUri = transaction.LocalTransactionUri;
                if (CollectionChanges.ContainsKey(localTransactionUri))
                    CollectionChanges.Remove(localTransactionUri);
                if (TransactionWithIndexChange != null && TransactionWithIndexChange.Contains(transaction))
                    TransactionWithIndexChange.Remove(transaction);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }



        /// <MetaDataID>{624C5104-632B-4D66-AE2B-217535C1A495}</MetaDataID>
        public virtual int Count
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    //  System.Collections.ArrayList tt;

                    if (!HasChanges)
                        return ContainObjects.Count;


                    int count = ContainObjects.Count;

                    #region Take in it count the relation changes which are not saved yet for current transaction.
                    foreach (CollectionChange collectionChangement in GetFinalCollectionChanges(Transactions.Transaction.Current))
                    {

                        //CollectionChangement collectionChangement=entry.Value as CollectionChangement;
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && ContainObjects.Contains(collectionChangement.Object))
                            count--;

                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !ContainObjects.Contains(collectionChangement.Object))
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

        /// <MetaDataID>{26E578F7-7C7F-4404-BBDA-A24EFDC32662}</MetaDataID>
        public virtual void RemoveObjects(PersistenceLayer.ObjectCollection objects)
        {
            foreach (object _object in objects)
                Remove(_object);
        }

        /// <MetaDataID>{7911E9D0-5412-41FE-BD70-ABFB015106A2}</MetaDataID>
        public virtual void AddObjects(PersistenceLayer.ObjectCollection objects)
        {
            foreach (object currObject in objects)
                Add(currObject);
        }
        /// <MetaDataID>{AC2006AD-EC68-465A-80E2-8E308B268AAE}</MetaDataID>
        public virtual void RemoveAll()
        {

            #region Get indexChange taransactions
            System.Collections.Generic.List<OOAdvantech.Transactions.Transaction> waitForTransactionsComplete = null;

        CheckIndexChangeTransactions:

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (TransactionWithIndexChange != null)
                {
                    foreach (OOAdvantech.Transactions.Transaction transaction in TransactionWithIndexChange)
                    {
                        OOAdvantech.Transactions.Transaction hierarchyTransacion = OOAdvantech.Transactions.Transaction.Current;
                        while (hierarchyTransacion != null && transaction != hierarchyTransacion && hierarchyTransacion.OriginTransaction != null)
                            hierarchyTransacion = hierarchyTransacion.OriginTransaction;
                        if (transaction != OOAdvantech.Transactions.Transaction.Current && transaction != hierarchyTransacion)
                        {
                            if (waitForTransactionsComplete == null)
                                waitForTransactionsComplete = new System.Collections.Generic.List<OOAdvantech.Transactions.Transaction>();
                            waitForTransactionsComplete.Add(transaction);
                        }
                    }
                }
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            #endregion

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            #region Lock on index change transactions
            if (waitForTransactionsComplete != null && waitForTransactionsComplete.Count > 0)
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                foreach (OOAdvantech.Transactions.Transaction transaction in waitForTransactionsComplete)
                {
                    if (!transaction.WaitToComplete(30000))
                        throw new System.Exception("Time out expired. Collection is locked from other transaction");
                }
                goto CheckIndexChangeTransactions;
            }
            #endregion

            try
            {
                System.Collections.IList objectsCollection = ContainObjects.OfType<object>().ToList();

                //Take in it count the collection changes which are not saved yet.
                foreach (CollectionChange collectionChangement in GetFinalCollectionChanges(Transactions.Transaction.Current))
                {
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && ContainObjects.Contains(collectionChangement.Object))
                        objectsCollection.Remove(collectionChangement.Object);
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !ContainObjects.Contains(collectionChangement.Object))
                        objectsCollection.Add(collectionChangement.Object);
                }
                foreach (object obj in objectsCollection)
                    Remove(obj);

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }


        }


        /// <MetaDataID>{38b29c2b-5cd4-43d7-8b90-77db4ede296c}</MetaDataID>
        public void RemoveAt(int index)
        {


            #region Get indexChange taransactions
            System.Collections.Generic.List<OOAdvantech.Transactions.Transaction> waitForTransactionsComplete = null;

        CheckIndexChangeTransactions:

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (TransactionWithIndexChange != null)
                {
                    foreach (OOAdvantech.Transactions.Transaction transaction in TransactionWithIndexChange)
                    {
                        OOAdvantech.Transactions.Transaction hierarchyTransacion = OOAdvantech.Transactions.Transaction.Current;
                        while (hierarchyTransacion != null && transaction != hierarchyTransacion && hierarchyTransacion.OriginTransaction != null)
                            hierarchyTransacion = hierarchyTransacion.OriginTransaction;
                        if (transaction != OOAdvantech.Transactions.Transaction.Current && transaction != hierarchyTransacion)
                        {
                            if (waitForTransactionsComplete == null)
                                waitForTransactionsComplete = new System.Collections.Generic.List<OOAdvantech.Transactions.Transaction>();
                            waitForTransactionsComplete.Add(transaction);
                        }
                    }
                }
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            #endregion
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            #region Lock on index change transactions
            if (waitForTransactionsComplete != null && waitForTransactionsComplete.Count > 0)
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                foreach (OOAdvantech.Transactions.Transaction transaction in waitForTransactionsComplete)
                {
                    if (!transaction.WaitToComplete(30000))
                        throw new System.Exception("Time out expired. Collection is locked from other transaction");
                }
                goto CheckIndexChangeTransactions;
            }
            #endregion
            try
            {
                if (index != -1 && index > Count)
                    throw new System.ArgumentOutOfRangeException("index", "Insertion index was out of range. Must be non-negative and less than or equal to size.");

                //if (GetCollectionChanges(Transactions.Transaction.Current).Count == 0)
                //{
                //    ContainObjects.RemoveAt(index);
                //    return;
                //}

                Remove(this[index]);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <MetaDataID>{31a602c9-2f72-409a-8f1e-1f0dd7c2d370}</MetaDataID>
        public void Insert(int index, object item)
        {
            //ContainObjects.Insert(index, item);


            ///TODO θα πρέπει να διασφαλιστή ότι τυχών error με το index π.χ. index έξω από το rangge της collection θα πρέπει να βγένει εδώ
            if (item == null)
                return;

            #region Get indexChange taransactions
            System.Collections.Generic.List<OOAdvantech.Transactions.Transaction> waitForTransactionsComplete = null;

        CheckIndexChangeTransactions:

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (TransactionWithIndexChange != null)
                {
                    foreach (OOAdvantech.Transactions.Transaction transaction in TransactionWithIndexChange)
                    {
                        OOAdvantech.Transactions.Transaction hierarchyTransacion = OOAdvantech.Transactions.Transaction.Current;
                        while (hierarchyTransacion != null && transaction != hierarchyTransacion && hierarchyTransacion.OriginTransaction != null)
                            hierarchyTransacion = hierarchyTransacion.OriginTransaction;
                        if (transaction != OOAdvantech.Transactions.Transaction.Current && transaction != hierarchyTransacion)
                        {
                            if (waitForTransactionsComplete == null)
                                waitForTransactionsComplete = new System.Collections.Generic.List<OOAdvantech.Transactions.Transaction>();
                            waitForTransactionsComplete.Add(transaction);
                        }
                    }
                }
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            #endregion

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            RelatedObjects = null;
            #region Lock on index change transactions
            if (waitForTransactionsComplete != null && waitForTransactionsComplete.Count > 0)
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                foreach (OOAdvantech.Transactions.Transaction transaction in waitForTransactionsComplete)
                {
                    if (!transaction.WaitToComplete(30000))
                        throw new System.Exception("Time out expired. Collection is locked from other transaction");
                }
                goto CheckIndexChangeTransactions;
            }
            #endregion

            try
            {
                int tmpCount = Count;
                int itemIndex = -1;
                if (Contains(item, out itemIndex))
                {
                    if (itemIndex == index)
                        return;

                    if (index != -1 && index > tmpCount - 1)
                        throw new System.ArgumentOutOfRangeException("index", "Insertion index was out of range. Must be non-negative and less than or equal to size.");
                    Remove(item);
                }
                else
                {
                    if (index != -1 && index > tmpCount)
                        throw new System.ArgumentOutOfRangeException("index", "Insertion index was out of range. Must be non-negative and less than or equal to size.");

                }
                if ((Transactions.Transaction.Current == null || Owner == null) && RelResolver == null)
                {
                    ContainObjects.Insert(index, item);
                }
                else
                {
                    CollectionChange collectionChangement = GetCollectionChange(item, Transactions.Transaction.Current);
                    if (collectionChangement != null)
                    {
                        foreach (CollectionChange currentcollectionChangement in GetFinalCollectionChanges(Transactions.Transaction.Current))
                        {
                            if (currentcollectionChangement != collectionChangement && currentcollectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && currentcollectionChangement.Index == -1)
                                currentcollectionChangement.Index = IndexOf(currentcollectionChangement.Object);
                        }
                        if (collectionChangement.TypeOfChange != CollectionChange.ChangeType.Added)
                            collectionChangement.TypeOfChange = CollectionChange.ChangeType.Added;
                        collectionChangement.Index = index;
                    }
                    else
                    {
                        int oldIndex = ContainObjects.IndexOf(item);
                        if (oldIndex != index)
                        {
                            collectionChangement = new CollectionChange(item, CollectionChange.ChangeType.Added, index, false);
                            AddCollectionChange(collectionChangement, Transactions.Transaction.Current);
                        }
                    }
                    foreach (CollectionChange currentcollectionChangement in GetFinalCollectionChanges(Transactions.Transaction.Current))
                    {
                        if (currentcollectionChangement.TypeOfChange == CollectionChange.ChangeType.Added /*&& !ContainObjects.Contains(currentcollectionChangement.Object)*/)
                        {
                            if (currentcollectionChangement.Index != -1 && currentcollectionChangement.Object != item && currentcollectionChangement.Index >= index)
                                currentcollectionChangement.Index++;
                        }
                    }

                }
                UpdateTheEndObjectsForRelationChange(item, CollectionChange.ChangeType.Added);
                //if (Owner != null && IsTwoWayNavigableAssociationEnd)
                //{
                //    //στην περίπτωση που ο owner είναι struct η σχέση δεν θα είναι ποτέ two way
                //    MetaDataRepository.AssociationEnd otherAssociationEnd = null;
                //    if (MetaObject is MetaDataRepository.AssociationEnd)
                //        otherAssociationEnd = MetaObject as MetaDataRepository.AssociationEnd;
                //    else
                //        otherAssociationEnd = (MetaObject as MetaDataRepository.AssociationEndRealization).Specification;
                //    otherAssociationEnd = otherAssociationEnd.GetOtherEnd();
                //    if (otherAssociationEnd.Association.LinkClass == null)
                //    {
                //        AddObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, item, Owner });
                //        //TODO θα πρέπει να γραφτεί κώδικας και test case για την περίπτωση των relation objects
                //    }
                //}

                if (RelResolver != null && RelResolver.AssociationEnd.Association.LinkClass == null)
                    RelResolver.UpgrateReferencialIntegrity(item, true);

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }

        /// <MetaDataID>{eaf3ae61-2023-4aba-86db-642efb59020e}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler _AddObjectsLinkFastInvoke;
        /// <MetaDataID>{4556f049-7369-4e7d-939c-1850ab2e17b2}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler AddObjectsLinkFastInvoke
        {
            get
            {
                if (_AddObjectsLinkFastInvoke == null)
                {

#if DeviceDotNet
                    _AddObjectsLinkFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.AssociationEnd", "DotNetMetaDataRepository, Version=1.0.0.0,  Culture=neutral, PublicKeyToken=null").GetMetaData().GetMethod("AddObjectsLink", AccessorBuilder.AllMembers));
#else
                    _AddObjectsLinkFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.AssociationEnd", "DotNetMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=11a79ce55c18c4e7").GetMetaData().GetMethod("AddObjectsLink", AccessorBuilder.AllMembers));
#endif
                    //#endif
                }
                return _AddObjectsLinkFastInvoke;
            }
        }

        /// <MetaDataID>{b546b7f8-4f7a-46fc-99a4-889d69f91ee3}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler _RemoveObjectsLinkFastInvoke;
        /// <MetaDataID>{f285cd35-ecc4-44e5-ba86-92b3476e8a1a}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler RemoveObjectsLinkFastInvoke
        {
            get
            {
                if (_RemoveObjectsLinkFastInvoke == null)
                {

#if DeviceDotNet
                    _RemoveObjectsLinkFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.AssociationEnd", "DotNetMetaDataRepository, Version=1.0.0.0,  Culture=neutral, PublicKeyToken=null").GetMetaData().GetMethod("RemoveObjectsLink", AccessorBuilder.AllMembers));
#else
                    _RemoveObjectsLinkFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.AssociationEnd", "DotNetMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=11a79ce55c18c4e7").GetMetaData().GetMethod("RemoveObjectsLink", AccessorBuilder.AllMembers));
#endif

                }
                return _RemoveObjectsLinkFastInvoke;
            }
        }

        /// <MetaDataID>{a231bb63-e699-4056-9b2b-31710f3fa323}</MetaDataID>
        public virtual void Add(object theObject)
        {
            Add(theObject, false);
        }

        /// <MetaDataID>{179E418D-8D84-43FE-AC9A-A5FBBF4D3736}</MetaDataID>
        public void Add(object theObject, bool implicitly)
        {


            if (RelResolver != null && theObject is OOAdvantech.MetaDataRepository.MetaObject && (theObject as OOAdvantech.MetaDataRepository.MetaObject).Name == "List`1")
            {

            }
            if (RelResolver != null && (RelResolver.AssociationEnd.Association.Name == "HallLayoutShape" || RelResolver.AssociationEnd.Association.Name == "SalesOffice_AccountabilityTypes"))
            {
                int rer = 0;
            }
            if (theObject != null && RelResolver != null && RelResolver.AssociationEnd != null && RelResolver.AssociationEnd.Specification != null)
            {

                if (!RelResolver.AssociationEnd.SpecificationType.IsInstanceOfType(theObject))
                {
                    if (RelResolver.AssociationEnd.Association.LinkClass == null)
                        throw new ArrayTypeMismatchException();
                    else
                    {
                        Type LinkClassType = RelResolver.AssociationEnd.Association.LinkClass.GetExtensionMetaObject<System.Type>();
                        if (!LinkClassType.IsInstanceOfType(theObject))
                            throw new ArrayTypeMismatchException();

                    }
                }
            }

            if (theObject == null)
                return;

            if (implicitly && LoadedObjectsContains(theObject))
                return;
            if (!implicitly && Contains(theObject))
                return;

            #region Get indexChange taransactions
            System.Collections.Generic.List<OOAdvantech.Transactions.Transaction> waitForTransactionsComplete = null;

        CheckIndexChangeTransactions:

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (TransactionWithIndexChange != null)
                {
                    foreach (OOAdvantech.Transactions.Transaction transaction in TransactionWithIndexChange)
                    {
                        OOAdvantech.Transactions.Transaction hierarchyTransacion = OOAdvantech.Transactions.Transaction.Current;
                        while (hierarchyTransacion != null && transaction != hierarchyTransacion && hierarchyTransacion.OriginTransaction != null)
                            hierarchyTransacion = hierarchyTransacion.OriginTransaction;
                        if (transaction != OOAdvantech.Transactions.Transaction.Current && transaction != hierarchyTransacion)
                        {
                            if (waitForTransactionsComplete == null)
                                waitForTransactionsComplete = new System.Collections.Generic.List<OOAdvantech.Transactions.Transaction>();

                            waitForTransactionsComplete.Add(transaction);
                        }
                    }
                }
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            #endregion

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            RelatedObjects = null;
            #region Lock on index change transactions
            if (waitForTransactionsComplete != null && waitForTransactionsComplete.Count > 0)
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                foreach (OOAdvantech.Transactions.Transaction transaction in waitForTransactionsComplete)
                {
                    if (!transaction.WaitToComplete(30000))
                        throw new System.Exception("Time out expired. Collection is locked from other transaction");
                }
                goto CheckIndexChangeTransactions;
            }
            #endregion


            try
            {
                bool upgrateReferencialIntegrity = false;

                if (((Transactions.Transaction.Current == null || Owner == null) && RelResolver == null) ||
                    Transactions.Transaction.Current == null && implicitly)
                {
                    if (implicitly)
                    {
                        if (Transactions.Transaction.Current == null && RelResolver != null && !RelResolver.IsCompleteLoaded)
                            return;
                        if (!LoadedObjectsContains(theObject))
                            ContainObjects.Add(theObject);
                    }
                    else
                    {
                        if (!Contains(theObject))
                        {
                            ContainObjects.Add(theObject);
                            UpdateTheEndObjectsForRelationChange(theObject, CollectionChange.ChangeType.Added);
                        }
                    }
                }
                else
                {
                    CollectionChange collectionChangement = GetCollectionChange(theObject, Transactions.Transaction.Current);
                    if (collectionChangement != null)
                    {
                        if (collectionChangement.TypeOfChange != CollectionChange.ChangeType.Added)
                        {
                            collectionChangement.Index = Count;
                            collectionChangement.TypeOfChange = CollectionChange.ChangeType.Added;

                            if (!implicitly)
                                UpdateTheEndObjectsForRelationChange(theObject, CollectionChange.ChangeType.Added);

                            if (RelResolver != null && RelResolver.AssociationEnd.Association.LinkClass == null)
                                RelResolver.UpgrateReferencialIntegrity(theObject, true);

                        }
                    }
                    else
                    {
                        if (implicitly)
                        {
                            if (!LoadedObjectsContains(theObject))
                            {
                                collectionChangement = new CollectionChange(theObject, CollectionChange.ChangeType.Added, Count, implicitly);
                                AddCollectionChange(collectionChangement, Transactions.Transaction.Current);

                                if (RelResolver != null && RelResolver.AssociationEnd.Association.LinkClass == null)
                                    RelResolver.UpgrateReferencialIntegrity(theObject, true);
                            }

                        }
                        else
                        {
                            if (!Contains(theObject))
                            {

                                collectionChangement = new CollectionChange(theObject, CollectionChange.ChangeType.Added, Count, implicitly);
                                AddCollectionChange(collectionChangement, Transactions.Transaction.Current);


                                UpdateTheEndObjectsForRelationChange(theObject, CollectionChange.ChangeType.Added);

                                if (RelResolver != null && RelResolver.AssociationEnd.Association.LinkClass == null)
                                    RelResolver.UpgrateReferencialIntegrity(theObject, true);
                            }
                        }

                    }
                }

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);


            }


        }
        /// <MetaDataID>{c8daa7a4-646b-41f8-be0e-f792ca537c73}</MetaDataID>
        public virtual void Remove(object theObject)
        {
            Remove(theObject, false);
        }
        /// <MetaDataID>{3757DC50-EC70-4998-9E60-BAA8AD419A91}</MetaDataID>
        public void Remove(object item, bool implicitly)
        {
            if (item == null)
                return;
            if (implicitly && !LoadedObjectsContains(item))
                return;
            if (!implicitly && !Contains(item))
                return;


            if (RelResolver != null && (RelResolver.AssociationEnd.Association.Name == "HallLayoutShape" || RelResolver.AssociationEnd.Association.Name == "SalesOffice_AccountabilityTypes"))
            {
                int rer = 0;
            }
        CheckIndexChangeTransactions:
            #region Get indexChange transactions
            System.Collections.Generic.List<OOAdvantech.Transactions.Transaction> waitForTransactionsComplete = null;
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (TransactionWithIndexChange != null)
                {
                    foreach (OOAdvantech.Transactions.Transaction transaction in TransactionWithIndexChange)
                    {
                        OOAdvantech.Transactions.Transaction hierarchyTransacion = OOAdvantech.Transactions.Transaction.Current;
                        while (hierarchyTransacion != null && transaction != hierarchyTransacion && hierarchyTransacion.OriginTransaction != null)
                            hierarchyTransacion = hierarchyTransacion.OriginTransaction;
                        if (transaction != OOAdvantech.Transactions.Transaction.Current && transaction != hierarchyTransacion)
                        {

                            if (waitForTransactionsComplete == null)
                                waitForTransactionsComplete = new System.Collections.Generic.List<OOAdvantech.Transactions.Transaction>();
                            waitForTransactionsComplete.Add(transaction);
                        }
                    }
                }
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            #endregion
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            RelatedObjects = null;
            //If the wait on transactions completed before the defined time period 
            //then check for index change transactions again      
            #region Lock on index change transactions



            if (waitForTransactionsComplete != null && waitForTransactionsComplete.Count > 0)
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                foreach (OOAdvantech.Transactions.Transaction transaction in waitForTransactionsComplete)
                {

                    if (!transaction.WaitToComplete(30000))
                        throw new System.Exception("Time out expired. Collection is locked from other transaction");
                }
                goto CheckIndexChangeTransactions;
            }
            #endregion


            try
            {
                int index = -1;
                bool upgrateReferencialIntegrity = false;

                if ((Transactions.Transaction.Current == null || Owner == null) && RelResolver == null)
                {
                    if (ContainObjects == null)
                        return;
                    ContainObjects.Remove(item);

                }
                else
                {

                    CollectionChange collectionChangement = GetCollectionChange(item, Transactions.Transaction.Current);

                    if (collectionChangement != null)
                    {
                        if (collectionChangement.Index == -1)
                            index = IndexOf(item);
                        else
                            index = collectionChangement.Index;
                        if (collectionChangement.TypeOfChange != CollectionChange.ChangeType.Deleteded)
                        {
                            collectionChangement.TypeOfChange = CollectionChange.ChangeType.Deleteded;
                            upgrateReferencialIntegrity = true;
                        }
                    }
                    else
                    {
                        if (RelResolver != null && RelResolver.AssociationEnd.Indexer)
                            if (!RelResolver.IsCompleteLoaded && implicitly)
                                index = -1;
                            else
                                index = IndexOf(item);

                        collectionChangement = new CollectionChange(item, CollectionChange.ChangeType.Deleteded, implicitly);
                        upgrateReferencialIntegrity = true;
                        AddCollectionChange(collectionChangement, Transactions.Transaction.Current);
                        collectionChangement.Index = index;
                    }


                    foreach (CollectionChange currentcollectionChangement in GetFinalCollectionChanges(Transaction.Current))
                    {
                        if (currentcollectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && currentcollectionChangement.Object != item)
                        {
                            if (currentcollectionChangement.Index != -1 && currentcollectionChangement.Index > index)
                            {
                                var currentTransactionCollectionChange = GetCollectionChange(currentcollectionChangement.Object, Transaction.Current);
                                if (currentTransactionCollectionChange == null)
                                {
                                    currentTransactionCollectionChange = new CollectionChange(currentcollectionChangement.Object, CollectionChange.ChangeType.Added, currentcollectionChangement.Index - 1, false);
                                    AddCollectionChange(currentTransactionCollectionChange, Transactions.Transaction.Current);
                                }
                                else
                                    currentTransactionCollectionChange.Index--;
                            }
                        }
                    }
                }

                if (RelResolver != null && upgrateReferencialIntegrity && RelResolver.AssociationEnd.Association.LinkClass == null)
                    RelResolver.UpgrateReferencialIntegrity(item, false);


            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
            if (!implicitly)
                UpdateTheEndObjectsForRelationChange(item, CollectionChange.ChangeType.Deleteded);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="implicitly"></param>
        private void UpdateTheEndObjectsForRelationChange(object item, CollectionChange.ChangeType changeType)
        {
            MetaDataRepository.AssociationEnd associationEnd = null;
            if (MetaObject is MetaDataRepository.AssociationEnd)
                associationEnd = MetaObject as MetaDataRepository.AssociationEnd;
            else if (MetaObject is MetaDataRepository.AssociationEndRealization)
                associationEnd = (MetaObject as MetaDataRepository.AssociationEndRealization).Specification;



            #region Update  general association for objects link changes

            if (associationEnd != null && associationEnd.Association.General != null)
            {
                MetaDataRepository.AssociationEnd generalAssociationEnd = null;
                if (associationEnd.IsRoleA)
                    generalAssociationEnd = associationEnd.Association.General.RoleA;
                else
                    generalAssociationEnd = associationEnd.Association.General.RoleB;
                if (generalAssociationEnd.Navigable)
                {
                    if (changeType == CollectionChange.ChangeType.Added)
                        AddObjectsLinkFastInvoke(null, new object[3] { generalAssociationEnd, Owner, item });
                    if (changeType == CollectionChange.ChangeType.Deleteded)
                        RemoveObjectsLinkFastInvoke(null, new object[3] { generalAssociationEnd, Owner, item });
                }
            }

            #endregion

            #region Update  other association end for objects link changes
            if (Owner != null && IsTwoWayNavigableAssociationEnd)
            {
                //στην περίπτωση που ο owner είναι struct η σχέση δεν θα είναι ποτέ two way
                MetaDataRepository.AssociationEnd otherAssociationEnd = associationEnd.GetOtherEnd();

                if (changeType == CollectionChange.ChangeType.Added)
                {
                    if (otherAssociationEnd.Association.LinkClass == null)
                    {
                        AddObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, item, Owner });
                        //TODO θα πρέπει να γραφτεί κώδικας και test case για την περίπτωση των relation objects
                    }
                    else
                    {
                        MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(item.GetType()) as MetaDataRepository.Classifier;
                        if (classifier.ClassHierarchyLinkAssociation == otherAssociationEnd.Association)
                        {
                            DotNetMetaDataRepository.Class linkClass = classifier as DotNetMetaDataRepository.Class;
                            if (otherAssociationEnd.IsRoleA)
                            {
                                object rolaBObject = Member<object>.GetValue(linkClass.LinkClassRoleBFastFieldAccessor.GetValue, item);
                                AddObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, rolaBObject, item });
                            }
                            else
                            {
                                object rolaAObject = Member<object>.GetValue(linkClass.LinkClassRoleAFastFieldAccessor.GetValue, item);
                                AddObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, rolaAObject, item });
                            }
                        }
                    }
                }
                if (changeType == CollectionChange.ChangeType.Deleteded)
                {
                    if (otherAssociationEnd.Association.LinkClass == null)
                    {
                        RemoveObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, item, Owner });
                    }
                    else
                    {
                        MetaDataRepository.Classifier classifier = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(item.GetType()) as MetaDataRepository.Classifier;
                        if (classifier.ClassHierarchyLinkAssociation == otherAssociationEnd.Association)
                        {
                            DotNetMetaDataRepository.Class linkClass = classifier as DotNetMetaDataRepository.Class;
                            if (otherAssociationEnd.IsRoleA)
                            {
                                object rolaBObject = Member<object>.GetValue(linkClass.LinkClassRoleBFastFieldAccessor.GetValue, item);
                                RemoveObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, rolaBObject, item });
                            }
                            else
                            {
                                object rolaAObject = Member<object>.GetValue(linkClass.LinkClassRoleAFastFieldAccessor.GetValue, item);
                                RemoveObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, rolaAObject, item });
                            }
                        }
                    }
                    //TODO θα πρέπει να γραφτεί κώδικας και test case για την περίπτωση των relation objects
                }
            }
            #endregion
        }



        System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, Collections.Generic.Dictionary<string, int>> _MultilingualNextIndex = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, Collections.Generic.Dictionary<string, int>>();

        Collections.Generic.Dictionary<string, int> _NextIndex;
        int NextIndex
        {
            get
            {
                if (Multilingual)
                {
                    Collections.Generic.Dictionary<string, int> _nextIndex = null;
                    _MultilingualNextIndex.TryGetValue(OOAdvantech.CultureContext.CurrentNeutralCultureInfo, out _nextIndex);

                    if (_nextIndex == null)
                        return -1;

                    string localTransactionUri = "null_transaction";
                    if (Transactions.Transaction.Current != null)
                        localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;

                    int nextIndex = 0;
                    if (!_nextIndex.TryGetValue(localTransactionUri, out nextIndex))
                        return -1;
                    else
                        return nextIndex;


                }
                else
                {

                    if (_NextIndex == null)
                        return -1;

                    string localTransactionUri = "null_transaction";
                    if (Transactions.Transaction.Current != null)
                        localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;

                    int nextIndex = 0;
                    if (!_NextIndex.TryGetValue(localTransactionUri, out nextIndex))
                        return -1;
                    else
                        return nextIndex;
                }
            }
            set
            {
                if (Multilingual)
                {
                    Collections.Generic.Dictionary<string, int> _nextIndex = null;
                    if (!_MultilingualNextIndex.TryGetValue(OOAdvantech.CultureContext.CurrentNeutralCultureInfo, out _nextIndex))
                    {
                        _nextIndex = new OOAdvantech.Collections.Generic.Dictionary<string, int>();
                        _MultilingualNextIndex[OOAdvantech.CultureContext.CurrentNeutralCultureInfo] = _nextIndex;
                    }

                    string localTransactionUri = "null_transaction";
                    if (Transactions.Transaction.Current != null)
                        localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;
                    _nextIndex[localTransactionUri] = value;

                }
                else
                {
                    if (_NextIndex == null)
                        _NextIndex = new OOAdvantech.Collections.Generic.Dictionary<string, int>();

                    string localTransactionUri = "null_transaction";
                    if (Transactions.Transaction.Current != null)
                        localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;
                    _NextIndex[localTransactionUri] = value;
                }
            }
        }
        /// <MetaDataID>{79fda625-e202-4708-b6c9-4337a958960e}</MetaDataID>
        internal System.Collections.Generic.ICollection<CollectionChange> GetCollectionChanges(Transactions.Transaction transaction)
        {


            if (CollectionChanges.Count == 0)
                return null;
            string localTransactionUri = "null_transaction";
            if (transaction != null)
                localTransactionUri = transaction.LocalTransactionUri;

            Collections.Generic.Dictionary<object, CollectionChange> collectionChanges = null;

            if (!CollectionChanges.TryGetValue(localTransactionUri, out collectionChanges))
                return null;


            return collectionChanges.Values;

        }

        /// <MetaDataID>{ac2a6bc1-c3d0-4ceb-889d-2d82d6bed023}</MetaDataID>
        void AddCollectionChange(CollectionChange collectionChangement, Transactions.Transaction transaction)
        {

            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                if (collectionChangement.Index != -1 && transaction != null)
                {
                    if (TransactionWithIndexChange == null)
                        TransactionWithIndexChange = new System.Collections.Generic.List<OOAdvantech.Transactions.Transaction>();
                    if (!TransactionWithIndexChange.Contains(transaction))
                        TransactionWithIndexChange.Add(transaction);
                }
                string localTransactionUri = "null_transaction";
                if (transaction != null)
                    localTransactionUri = transaction.LocalTransactionUri;

                Collections.Generic.Dictionary<object, CollectionChange> collectionChanges = null;
                if (!CollectionChanges.TryGetValue(localTransactionUri, out collectionChanges))
                    throw new System.Exception("Use the ObjectStateTransition scoop before change the collection");

                /// TODO θα πρέπει να φύγει η παρακάτω γραμμή κώδικα για λόγους performance αφού πρώτα διασφαλιστεό η συνεκτικότητα του συστήματος.
                if (collectionChanges.ContainsKey(collectionChangement.Object))
                    throw new System.Exception("Collection change for the object already exist.");

                collectionChanges[collectionChangement.Object] = collectionChangement;
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <MetaDataID>{a7c33e50-5ad0-4b44-906c-4f655959995f}</MetaDataID>
        void RemoveCollectionChange(CollectionChange collectionChangement)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, Collections.Generic.Dictionary<object, CollectionChange>> entry in CollectionChanges)
            {
                if (entry.Value.ContainsKey(collectionChangement.Object))
                {
                    entry.Value.Remove(collectionChangement.Object);
                    break;
                }
            }

        }


        /// <MetaDataID>{bb368544-7d3a-445e-b13a-be0f087a8906}</MetaDataID>
        public virtual void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            if (Multilingual)
            {
                lock (MultilingualCollectionChanges)
                {
                    foreach (var cultrure in MultilingualCollectionChanges.Keys)
                    {
                        using (CultureContext cultureContext = new CultureContext(cultrure, false))
                        {
                            InternalUndoChanges(transaction);
                        }
                    }
                }
            }
            else
            {
                InternalUndoChanges(transaction);
            }


            //  System.Diagnostics.Debug.WriteLine("################# Undo ####################");
        }

        private void InternalUndoChanges(Transaction transaction)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                //foreach (CollectionChange collectionChangement in GetCollectionChanges(transaction))
                //{
                //    //CollectionChangement collectionChangement=entry.Value as CollectionChangement;
                //    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added)
                //    {
                //        ContainObjects.Remove(collectionChangement.Object);

                //        System.Diagnostics.Debug.WriteLine("Undo Remove");
                //    }

                //    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded)
                //    {
                //        ContainObjects.Add(collectionChangement.Object);
                //        System.Diagnostics.Debug.WriteLine("Undo Add");
                //    }
                //}
                System.Collections.Generic.ICollection<CollectionChange> collectionChanges = GetCollectionChanges(transaction);
                if (collectionChanges != null)
                {
                    foreach (CollectionChange collectionChangement in collectionChanges)
                    {
                        //CollectionChangement collectionChangement=entry.Value as CollectionChangement;
                        if (collectionChangement.ChangeApplied)
                        {
                            if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added)
                            {
                                if (RelResolver.LoadedRelatedObjects.Contains(collectionChangement.Object))
                                    RelResolver.RemoveRelatedObject(collectionChangement.Object);

                                System.Diagnostics.Debug.WriteLine("Undo Remove");
                            }

                            if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded)
                            {
                                if (!RelResolver.LoadedRelatedObjects.Contains(collectionChangement.Object))
                                    RelResolver.AddRelatedObject(collectionChangement.Object);
                            }
                        }
                    }
                }

                RelatedObjects = null;
                ClearCollectionChangements(transaction);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }

        /// <MetaDataID>{4ee93aac-3895-4d02-9660-23f891229c3e}</MetaDataID>
        bool Contains(object theObject, out int index)
        {
            index = -1;
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                //System.Collections.Generic.ICollection<CollectionChange> collectionChanges = GetCollectionChanges(Transactions.Transaction.Current);



                //Take in it count the collection changes which are not saved yet.
                System.Collections.Generic.List<CollectionChange> collectionChanges = GetFinalCollectionChanges(Transactions.Transaction.Current);
                if (collectionChanges == null || collectionChanges.Count == 0)
                    return ContainObjects.Contains(theObject);


                var relatedObjects = ContainObjects.OfType<object>().ToList();
                foreach (CollectionChange collectionChangement in collectionChanges)
                {
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && ContainObjects.Contains(collectionChangement.Object))
                        relatedObjects.Remove(collectionChangement.Object);
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !ContainObjects.Contains(collectionChangement.Object))
                        if (collectionChangement.Index == -1)
                            relatedObjects.Add(collectionChangement.Object);
                }
                collectionChanges.Sort(new CollectionChangesSort());
                foreach (CollectionChange collectionChangement in collectionChanges)
                {
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !ContainObjects.Contains(collectionChangement.Object))
                        if (collectionChangement.Index != -1)
                            relatedObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                }

                bool contained = relatedObjects.Contains(theObject);
                if (RelResolver != null && RelResolver.AssociationEnd.Indexer)
                    index = relatedObjects.IndexOf(theObject);
                return relatedObjects.Contains(theObject);
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }


        public virtual bool LoadedObjectsContains(object theObject)
        {

            if (theObject == null)
                return false;



            ReaderWriterLock.AcquireReaderLock(10000);

            System.Collections.Generic.List<object> loadedObjects = null;
            if (RelResolver != null)
            {
                if (OwnerIsPersistent.UnInitialized)
                    OwnerIsPersistent.Value = PersistenceLayer.ObjectStorage.IsPersistent(Owner);
                if (OwnerIsPersistent.Value)
                    loadedObjects = RelResolver.LoadedRelatedObjects;
                else
                    loadedObjects = TransientObjects;
            }
            else
                loadedObjects = TransientObjects;

            try
            {
                System.Collections.Generic.List<object> relatedObjects = new System.Collections.Generic.List<object>();
                System.Collections.Generic.List<CollectionChange> collectionChanges = GetFinalCollectionChanges(Transactions.Transaction.Current);
                if (collectionChanges == null || collectionChanges.Count == 0)
                    return loadedObjects.Contains(theObject);


                if (loadedObjects.Contains(theObject))
                    relatedObjects.Add(theObject);

                foreach (CollectionChange collectionChangement in collectionChanges)
                {
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && relatedObjects.Contains(collectionChangement.Object))
                        relatedObjects.Remove(collectionChangement.Object);
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object))
                        relatedObjects.Add(collectionChangement.Object);
                }
                return relatedObjects.Contains(theObject);
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            return false;
        }


        /// <MetaDataID>{A4987067-D9C4-4D34-A5D1-0CDEED874594}</MetaDataID>
        public virtual bool Contains(object theObject)
        {
            if (theObject == null)
                return false;

            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                //System.Collections.Generic.ICollection<CollectionChange> collectionChanges = GetCollectionChanges(Transactions.Transaction.Current);
                //Take in it count the collection changes which are not saved yet.
                System.Collections.Generic.List<object> relatedObjects = new System.Collections.Generic.List<object>();
                System.Collections.Generic.List<CollectionChange> collectionChanges = GetFinalCollectionChanges(Transactions.Transaction.Current);
                if (collectionChanges == null || collectionChanges.Count == 0)
                    return ContainObjects.Contains(theObject);


                if (ContainObjects.Contains(theObject))
                    relatedObjects.Add(theObject);

                foreach (CollectionChange collectionChangement in collectionChanges)
                {
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && relatedObjects.Contains(collectionChangement.Object))
                        relatedObjects.Remove(collectionChangement.Object);
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object))
                        relatedObjects.Add(collectionChangement.Object);
                }
                return relatedObjects.Contains(theObject);
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
            int index = 0;
            return Contains(theObject, out index);
        }


        class CollectionChangesSort : System.Collections.Generic.IComparer<CollectionChange>
        {
            public int Compare(CollectionChange x, CollectionChange y)
            {
                if (x == null || y == null)
                    return 0;
                else
                    return x.Index.CompareTo(y.Index);
            }
        }
        /// <MetaDataID>{a015fbde-7923-48ef-95be-b85e82e1bfe5}</MetaDataID>
        [System.NonSerialized]
        Member<bool> OwnerIsPersistent = new Member<bool>();




        System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, System.Collections.Generic.List<object>> _MultilingualContainObjects = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, System.Collections.Generic.List<object>>();




        /// <MetaDataID>{0BA5F4AF-D93F-45E2-A8AB-4565E891AAC7}</MetaDataID>
        System.Collections.Generic.List<object> _TransientObjects = new System.Collections.Generic.List<object>();

        System.Collections.Generic.List<object> TransientObjects
        {
            get
            {
                if (Multilingual)
                {
                    System.Collections.Generic.List<object> containTransientObjects = null;
                    if (!_MultilingualContainObjects.TryGetValue(OOAdvantech.CultureContext.CurrentNeutralCultureInfo, out containTransientObjects))
                    {
                        containTransientObjects = new System.Collections.Generic.List<object>();
                        _MultilingualContainObjects[OOAdvantech.CultureContext.CurrentNeutralCultureInfo] = containTransientObjects;
                    }
                    return containTransientObjects;
                }
                else
                    return _TransientObjects;
            }
        }




        /// <MetaDataID>{9f0d52b0-8c83-42c3-8955-562ac6332a29}</MetaDataID>
        System.Collections.IList ContainObjects
        {
            get
            {

                if (RelResolver != null)
                {
                    if (OwnerIsPersistent.UnInitialized)
                        OwnerIsPersistent.Value = PersistenceLayer.ObjectStorage.IsPersistent(Owner);
                    if (OwnerIsPersistent.Value)
                        return RelResolver;
                    else
                        return TransientObjects;
                }
                else
                    return TransientObjects;
            }
        }


        private System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, System.Collections.Generic.Dictionary<string, System.Collections.IList>> _MultilingualRelatedObjects = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, System.Collections.Generic.Dictionary<string, System.Collections.IList>>();


        System.Collections.Generic.Dictionary<string, System.Collections.IList> _RelatedObjects = null;
        /// <MetaDataID>{0721f38c-f7ca-4a44-9b83-743b8369deb6}</MetaDataID>
        System.Collections.Generic.Dictionary<string, System.Collections.IList> RelatedObjects
        {
            get
            {

                if (Multilingual)
                {
                    System.Collections.Generic.Dictionary<string, System.Collections.IList> relatedObjects = null;
                    if (!_MultilingualRelatedObjects.TryGetValue(OOAdvantech.CultureContext.CurrentNeutralCultureInfo, out relatedObjects))
                        return null;
                    return relatedObjects;
                }
                else
                    return _RelatedObjects;
            }
            set
            {
                if (Multilingual)
                    _MultilingualRelatedObjects[OOAdvantech.CultureContext.CurrentNeutralCultureInfo] = value;
                else
                    _RelatedObjects = value;
            }
        }


        public System.Collections.Generic.List<CultureInfo> Cultures
        {
            get
            {
                System.Collections.Generic.List<CultureInfo> cultures = new System.Collections.Generic.List<CultureInfo>();

                foreach (var culture in _MultilingualRelatedObjects.Keys)
                {
                    if (!cultures.Contains(culture))
                        cultures.Add(culture);
                }
                if (RelResolver != null)
                {
                    cultures = RelResolver.Cultures;
                    foreach (var culture in _MultilingualRelatedObjects.Keys)
                    {
                        if (!cultures.Contains(culture))
                            cultures.Add(culture);
                    }
                }
                return cultures;
            }
        }
        public System.Collections.ICollection ToThreadSafeSet(Type setType)
        {


            IList list = Activator.CreateInstance(typeof(Set<>).MakeGenericType(setType)) as IList;
            ReaderWriterLock.AcquireReaderLock(10000);

            //int frc = ReaderWriterLock.SystemReaderWriterLock.RefCount();
            //if (frc > 1)
            //{

            //}
            //int fc = ReaderWriterLock.SystemReaderWriterLock.ReadLocks.Count;
            //int frc = 0;
            //if (fc > 0)
            //    frc = ReaderWriterLock.SystemReaderWriterLock.ReadLocks[System.Threading.Thread.CurrentThread.ManagedThreadId].RefCount;

            //int fc2 = ReaderWriterLock.SystemReaderWriterLock.ReadLocks.Count;


            try
            {
                foreach (var obj in this)
                    list.Add(obj);
                return list;
            }
            finally
            {

                ////int c = ReaderWriterLock.SystemReaderWriterLock.ReadLocks.Count;
                //int rc = ReaderWriterLock.SystemReaderWriterLock.RefCount();
                ////                if (c > 0)
                ////                  rc = ReaderWriterLock.SystemReaderWriterLock.ReadLocks[System.Threading.Thread.CurrentThread.ManagedThreadId].RefCount;

                ReaderWriterLock.ReleaseReaderLock();
                //int rc2 = ReaderWriterLock.SystemReaderWriterLock.RefCount();
                ////  

                //if (rc2 >0)
                //{

                //}



            }
        }
        public OOAdvantech.Collections.Generic.List<object> ToThreadSafeList()
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            //int fc = ReaderWriterLock.SystemReaderWriterLock.ReadLocks.Count;
            //int frc = ReaderWriterLock.SystemReaderWriterLock.RefCount();
            //if (frc > 1)
            //{

            //}
            //if (fc > 0)
            //    frc = ReaderWriterLock.SystemReaderWriterLock.ReadLocks[System.Threading.Thread.CurrentThread.ManagedThreadId].RefCount;
            //else
            //    frc = ReaderWriterLock.SystemReaderWriterLock.LastAccessedReadLock.RefCount;

            //int fc2 = ReaderWriterLock.SystemReaderWriterLock.ReadLocks.Count;
            //if(fc==0)
            //{

            //}
            try
            {
                List<object> list = new List<object>();
                foreach (var obj in this)
                    list.Add(obj);
                return list;
            }
            finally
            {
                ////int c = ReaderWriterLock.SystemReaderWriterLock.ReadLocks.Count;
                //int rc = ReaderWriterLock.SystemReaderWriterLock.RefCount();
                ////                if (c > 0)
                ////                  rc = ReaderWriterLock.SystemReaderWriterLock.ReadLocks[System.Threading.Thread.CurrentThread.ManagedThreadId].RefCount;

                ReaderWriterLock.ReleaseReaderLock();
                //int rc2 = ReaderWriterLock.SystemReaderWriterLock.RefCount();
                //if (rc2 > 0)
                //{

                //}
                //  

            }
        }

        /// <MetaDataID>{1761192F-01A5-4C02-A065-FC02EFE68A67}</MetaDataID>
        public virtual System.Collections.IEnumerator GetEnumerator()
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (!HasChanges)
                    return ContainObjects.GetEnumerator();

                System.Collections.IList relatedObjects = null;
                string localTransactionUri = "null_transaction";
                if (Transactions.Transaction.Current != null)
                    localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;
                if (RelatedObjects != null)
                    if (RelatedObjects.TryGetValue(localTransactionUri, out relatedObjects))
                        return relatedObjects.GetEnumerator();

                relatedObjects = new Collections.Generic.List<object>(ContainObjects.OfType<object>());
                System.Collections.Generic.List<CollectionChange> collectionChanges = GetFinalCollectionChanges(Transactions.Transaction.Current);

                if (collectionChanges == null && collectionChanges.Count == 0)
                    return ContainObjects.GetEnumerator();
                foreach (CollectionChange collectionChangement in collectionChanges)
                {
                    //Object removed
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && relatedObjects.Contains(collectionChangement.Object))
                        relatedObjects.Remove(collectionChangement.Object);
                    //object in new position
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index != -1)
                        relatedObjects.Remove(collectionChangement.Object);
                }
                collectionChanges.Sort(new CollectionChangesSort());
                foreach (CollectionChange collectionChangement in collectionChanges.OrderBy(x => x.Index))
                {
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index != -1)
                    {
                        if (collectionChangement.Index >= relatedObjects.Count)
                            relatedObjects.Add(collectionChangement.Object);
                        else
                            relatedObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                    }
                }
                foreach (CollectionChange collectionChangement in collectionChanges)
                {
                    if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index == -1)
                        relatedObjects.Add(collectionChangement.Object);
                }


                if (RelatedObjects == null)
                    RelatedObjects = new System.Collections.Generic.Dictionary<string, System.Collections.IList>();
                RelatedObjects[localTransactionUri] = relatedObjects;

                return relatedObjects.GetEnumerator();
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
                //int rca = ReaderWriterLock.SystemReaderWriterLock.RefCount();
                //var res = ReaderWriterLock.ReleaseReaderLock();
                //int rc = ReaderWriterLock.SystemReaderWriterLock.RefCount();
                //if (rc > 1)
                //{

                //}
            }

        }




        /// <MetaDataID>{9fed1636-8608-4833-96af-15f34b8e510d}</MetaDataID>
        System.Collections.IList ObjectsUnderCurrentTransaction
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    System.Collections.IList relatedObjects = null;
                    string localTransactionUri = "null_transaction";
                    if (Transactions.Transaction.Current != null)
                        localTransactionUri = Transactions.Transaction.Current.LocalTransactionUri;
                    if (RelatedObjects != null)
                        if (RelatedObjects.TryGetValue(localTransactionUri, out relatedObjects))
                            return relatedObjects;
                    relatedObjects = ContainObjects.OfType<object>().ToList();
                    System.Collections.Generic.List<CollectionChange> collectionChanges = GetFinalCollectionChanges(Transactions.Transaction.Current);
                    if (collectionChanges == null && collectionChanges.Count == 0)
                        return ContainObjects;
                    foreach (CollectionChange collectionChangement in collectionChanges)
                    {
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && relatedObjects.Contains(collectionChangement.Object))
                            relatedObjects.Remove(collectionChangement.Object);
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object))
                            if (collectionChangement.Index == -1)
                            {
                                int tmp = collectionChangement.Object.GetHashCode();
                                relatedObjects.Add(collectionChangement.Object);
                            }
                    }
                    collectionChanges.Sort(new CollectionChangesSort());
                    foreach (CollectionChange collectionChangement in collectionChanges)
                    {
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index != -1)
                            relatedObjects.Remove(collectionChangement.Object);
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index != -1)
                        {
                            int tmp = collectionChangement.Object.GetHashCode();
                            relatedObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                        }
                    }
                    if (RelatedObjects == null)
                        RelatedObjects = new System.Collections.Generic.Dictionary<string, System.Collections.IList>();
                    RelatedObjects[localTransactionUri] = relatedObjects;
                    return relatedObjects;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }

        }


        /// <MetaDataID>{f34854d5-c3bd-45e9-a212-a9eef7915dbc}</MetaDataID>
        public bool HasChanges
        {
            get
            {
                if (CollectionChanges.Count == 0)
                    return false;
                Transactions.Transaction transaction = Transactions.Transaction.Current;
                if (transaction == null)
                    return false;
                while (transaction != null)
                {
                    if (CollectionChanges.ContainsKey(transaction.LocalTransactionUri) && CollectionChanges[transaction.LocalTransactionUri].Count > 0)
                        return true;
                    transaction = transaction.OriginTransaction;
                }
                return false;
            }
        }

        /// <MetaDataID>{60eb1cf6-c1f2-439c-a657-b6405d5a36ae}</MetaDataID>
        System.Collections.Generic.List<CollectionChange> GetFinalCollectionChanges(Transactions.Transaction transaction)
        {
            Collections.Generic.List<CollectionChange> allCollectionChanges = new OOAdvantech.Collections.Generic.List<CollectionChange>();
            string localTransactionUri = "null_transaction";
            if (transaction != null && transaction.OriginTransaction != null)
            {
                System.Collections.Generic.ICollection<CollectionChange> masterTransactionRelelationChanges = GetFinalCollectionChanges(transaction.OriginTransaction);
                localTransactionUri = transaction.LocalTransactionUri;
                Collections.Generic.Dictionary<object, CollectionChange> transactionRelelationChanges = null;
                CollectionChanges.TryGetValue(localTransactionUri, out transactionRelelationChanges);

                foreach (CollectionChange collectionChange in masterTransactionRelelationChanges)
                {
                    if (transactionRelelationChanges == null || !transactionRelelationChanges.ContainsKey(collectionChange.Object))
                        allCollectionChanges.Add(collectionChange);
                }

                if (transactionRelelationChanges != null)
                    allCollectionChanges.AddRange(transactionRelelationChanges.Values);

                //allCollectionChanges.Sort(new CollectionChangesSort());
                return allCollectionChanges;
                //CollectionChange[] tempCollectionChanges = new CollectionChange[allCollectionChanges.Count];
                //int i = 0;
                //foreach (CollectionChange collectionChange in allCollectionChanges)
                //    tempCollectionChanges[i++] = collectionChange;
                //return tempCollectionChanges;
            }
            else
            {
                System.Collections.Generic.ICollection<CollectionChange> collectionChanges = GetCollectionChanges(transaction);
                if (collectionChanges == null)
                    return new System.Collections.Generic.List<CollectionChange>();
                else
                    return new System.Collections.Generic.List<CollectionChange>(collectionChanges);
            }

        }
        /// <MetaDataID>{36899123-44ae-4372-ba6b-a46848b16d79}</MetaDataID>
        public object this[int index]
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    //if (GetCollectionChanges(Transactions.Transaction.Current).Count == 0)
                    //    return ContainObjects[index];

                    var relatedObjects = ContainObjects.OfType<object>().ToList();

                    //Take in it count the collection changes which are not saved yet.

                    System.Collections.Generic.List<CollectionChange> collectionChanges = GetFinalCollectionChanges(Transactions.Transaction.Current);
                    if (collectionChanges == null && collectionChanges.Count == 0)
                        return ContainObjects[index];


                    foreach (CollectionChange collectionChangement in collectionChanges)
                    {
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Deleteded && relatedObjects.Contains(collectionChangement.Object))
                            relatedObjects.Remove(collectionChangement.Object);
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object))
                            if (collectionChangement.Index == -1)
                                relatedObjects.Add(collectionChangement.Object);
                    }
                    collectionChanges.Sort(new CollectionChangesSort());
                    foreach (CollectionChange collectionChangement in collectionChanges)
                    {
                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index != -1)
                            relatedObjects.Remove(collectionChangement.Object);

                        if (collectionChangement.TypeOfChange == CollectionChange.ChangeType.Added && !relatedObjects.Contains(collectionChangement.Object) && collectionChangement.Index != -1)
                            relatedObjects.Insert(collectionChangement.Index, collectionChangement.Object);
                    }




                    return relatedObjects[index];
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }


            }
        }


        /// <exclude>Excluded</exclude>
        private System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, CollectionChangeCollection> MultilingualCollectionChanges = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, CollectionChangeCollection>();


        /// <exclude>Excluded</exclude>
        private CollectionChangeCollection _CollectionChanges = new CollectionChangeCollection();


        /// <exclude>Excluded</exclude
        private CollectionChangeCollection CollectionChanges
        {
            get
            {
                if (Multilingual)
                {
                    CollectionChangeCollection collectionChanges = null;
                    if (!MultilingualCollectionChanges.TryGetValue(OOAdvantech.CultureContext.CurrentNeutralCultureInfo, out collectionChanges))
                    {
                        collectionChanges = new CollectionChangeCollection();

                        var existingcollectionChanges = MultilingualCollectionChanges.Values.FirstOrDefault();
                        if (existingcollectionChanges != null)
                        {
                            foreach (var transactionUri in existingcollectionChanges.Keys)
                                collectionChanges[transactionUri] = new Collections.Generic.Dictionary<object, CollectionChange>();
                        }
                        MultilingualCollectionChanges[OOAdvantech.CultureContext.CurrentNeutralCultureInfo] = collectionChanges;
                    }
                    return collectionChanges;
                }
                else
                    return _CollectionChanges;
            }
        }



        [System.NonSerialized]
        System.Collections.Generic.List<Transactions.Transaction> TransactionWithIndexChange = null;

        /// <MetaDataID>{dab8d7c1-a3a9-4662-8625-dccca7c3ed75}</MetaDataID>
        CollectionChange GetCollectionChange(object anObject, Transactions.Transaction transaction)
        {

            Collections.Generic.Dictionary<object, CollectionChange> collectionChanges = null;
            string localTransactionUri = "null_transaction";
            if (transaction != null)
                localTransactionUri = transaction.LocalTransactionUri;
            if (CollectionChanges.TryGetValue(localTransactionUri, out collectionChanges))
            {
                CollectionChange collectionChange = null;
                if (collectionChanges.TryGetValue(anObject, out collectionChange))
                    return collectionChange;
                else
                    return null;
            }
            else
                return null;
        }

        #region ICollectionMember Members

        /// <MetaDataID>{1a25600a-5d11-4344-a524-56d19ad1fcd6}</MetaDataID>
        void ICollectionMember.AddImplicitly(object _object)
        {
            if (RelResolver != null && RelResolver.AssociationEnd.Indexer && NextIndex == -1 && !RelResolver.IsCompleteLoaded)
                NextIndex = (RelResolver as System.Collections.ICollection).Count;
            OOAdvantech.PersistenceLayerRunTime.Commands.Command command = null;
            if (RelResolver != null && RelResolver.AssociationEnd.Indexer &&
                TransactionContext.CurrentTransactionContext != null &&
                TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(Commands.UpdateStorageInstanceCommand.GetIdentity(RelResolver.Owner), out command))
                (command as Commands.UpdateStorageInstanceCommand).RefreshSubCommands();
            Add(_object, true);
        }

        /// <MetaDataID>{ecd2f1a5-2986-4751-8617-aaa88efda3e8}</MetaDataID>
        void ICollectionMember.RemoveImplicitly(object _object)
        {
            if (RelResolver != null)
            {
                OOAdvantech.PersistenceLayerRunTime.Commands.Command command = null;
                if (RelResolver != null && RelResolver.AssociationEnd.Indexer &&
                    TransactionContext.CurrentTransactionContext != null &&
                    TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(Commands.UpdateStorageInstanceCommand.GetIdentity(RelResolver.Owner), out command))
                    (command as Commands.UpdateStorageInstanceCommand).RefreshSubCommands();
            }

            Remove(_object, true);
        }

        /// <MetaDataID>{df95e6ba-a8e1-47db-8125-9cad5c33cf62}</MetaDataID>
        System.Collections.IEnumerator ICollectionMember.GetEnumeretor()
        {
            return GetEnumerator();
        }

        #endregion


        /// <MetaDataID>{9e845e32-f42f-470f-be5a-8341c1cc0a8d}</MetaDataID>
        object _Owner;
        /// <MetaDataID>{8ef1ddf7-6301-484e-85b9-5fb1e4e889ee}</MetaDataID>
        internal object Owner
        {
            get
            {
                if (_Owner == null && RelResolver != null)
                    _Owner = RelResolver.Owner.MemoryInstance;
                return _Owner;
            }
        }

        /// <MetaDataID>{e98a7273-07cf-4d9f-a268-065f94a14bb8}</MetaDataID>
        MetaDataRepository.MetaObject MetaObject;
        #region IMemberInitialization Members

        /// <MetaDataID>{507f314c-1ed5-4113-93b6-ee7ce401e7c0}</MetaDataID>
        void IMemberInitialization.SetOwner(object owner)
        {
            _Owner = owner;

        }
        /// <MetaDataID>{6e62cb98-022b-4f22-b3d6-4b95281b265e}</MetaDataID>
        bool IsTwoWayNavigableAssociationEnd;
        /// <MetaDataID>{fdc2d890-4365-45a4-beb0-df7e321ce92e}</MetaDataID>
        void IMemberInitialization.SetMetadata(OOAdvantech.MetaDataRepository.MetaObject metadata)
        {
            MetaObject = metadata;

            if (MetaObject is MetaDataRepository.AssociationEnd && (MetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Navigable)
                IsTwoWayNavigableAssociationEnd = true;
            else if (MetaObject is MetaDataRepository.AssociationEndRealization && (MetaObject as MetaDataRepository.AssociationEndRealization).Specification.GetOtherEnd().Navigable)
                IsTwoWayNavigableAssociationEnd = true;
            else
                IsTwoWayNavigableAssociationEnd = false;
        }


        /// <MetaDataID>{9b4f9993-5ef5-46a7-a62b-21f19697d32a}</MetaDataID>
        void IMemberInitialization.SetMetadata(OOAdvantech.MetaDataRepository.MetaObject metadata, object relResolver)
        {
            MetaObject = metadata;

            if (MetaObject is MetaDataRepository.AssociationEnd && (MetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Navigable)
                IsTwoWayNavigableAssociationEnd = true;
            else if (MetaObject is MetaDataRepository.AssociationEndRealization && (MetaObject as MetaDataRepository.AssociationEndRealization).Specification.GetOtherEnd().Navigable)
                IsTwoWayNavigableAssociationEnd = true;
            else
                IsTwoWayNavigableAssociationEnd = false;
        }

        /// <MetaDataID>{2ed7deca-e9a8-4579-a5ba-467c15d90c48}</MetaDataID>
        bool IMemberInitialization.Initialized
        {
            get
            {
                if (MetaObject != null && MetaObject.Namespace is OOAdvantech.MetaDataRepository.Structure)
                    return true;
                return Owner != null;
            }
        }

        public IDictionary Values
        {
            get
            {
                Hashtable hashtable = new Hashtable();
                foreach (var culture in Cultures)
                {
                    using (OOAdvantech.CultureContext cultureContext = new CultureContext(culture, false))
                    {
                        System.Collections.Generic.List<object> cultureObjects = new System.Collections.Generic.List<object>();
                        foreach (var obj in this)
                            cultureObjects.Add(obj);
                        hashtable.Add(culture, cultureObjects);
                    }
                }
                return hashtable;
            }
        }

        public CultureInfo DefaultLanguage
        {
            get
            {
                if (RelResolver != null)
                {
                    if (!string.IsNullOrWhiteSpace(RelResolver.Owner.ObjectStorage.StorageMetaData.Culture))
                        return CultureInfo.GetCultureInfo(RelResolver.Owner.ObjectStorage.StorageMetaData.Culture);
                }
                return CultureInfo.CurrentCulture;
            }
        }

        #endregion


        /// <MetaDataID>{4de053d2-3860-484e-92f2-901c03340f3b}</MetaDataID>
        public System.Collections.Generic.List<GroupIndexChange> GetIndexChanges(string transactionUri)
        {

            if (RelResolver != null)
            {
                if (RelResolver.Owner.PersistentObjectID != null && RelResolver.Owner.PersistentObjectID.ToString() == "2022")
                {
                }
            }

            System.Collections.Generic.List<GroupIndexChange> groupIndexChanges = new System.Collections.Generic.List<GroupIndexChange>();
            bool getIndexChangesIndirect = true;
            System.Collections.Generic.List<CollectionChange> collectionChanges = null;

            #region Determine the way where use to produce index changes


            var collection = GetCollectionChanges(Transactions.Transaction.Current);
            if (collection == null)
                collectionChanges = new System.Collections.Generic.List<CollectionChange>();
            else
                collectionChanges = new System.Collections.Generic.List<CollectionChange>(collection);

            collectionChanges = (from collectionChange in collectionChanges
                                 where collectionChange.TypeOfChange == CollectionChange.ChangeType.Deleteded && !collectionChange.RelatedObjectsIndexesRebuilded
                                 orderby collectionChange.Index ascending
                                 select collectionChange).ToList();


            if (RelResolver.IsCompleteLoaded)
                getIndexChangesIndirect = false;
            else
            {

                foreach (var collectionChange in collectionChanges)
                {
                    if (!collectionChange.Implicitly && collectionChange.TypeOfChange == CollectionChange.ChangeType.Deleteded)
                        getIndexChangesIndirect = false;
                }
            }

            #endregion

            GroupIndexChange groupIndexChange = null;
            int change = 0;
            if (getIndexChangesIndirect)
            {
                #region Get index changes directly from collection changes

                int lastIndex = (RelResolver as System.Collections.ICollection).Count - 1;
                change = -1;
                for (int i = 0; i != collectionChanges.Count; i++)
                {
                    int endIndex = 0;
                    if (i + 1 != collectionChanges.Count)
                        endIndex = collectionChanges[i + 1].Index - 1;
                    else
                        endIndex = lastIndex;
                    groupIndexChange = new GroupIndexChange();
                    groupIndexChange.StartIndex = collectionChanges[i].Index + 1;
                    groupIndexChange.EndIndex = endIndex;
                    groupIndexChange.Change = change--;
                    groupIndexChanges.Add(groupIndexChange);
                }
                groupIndexChanges.Sort(new Comparison<GroupIndexChange>(Ascend));
                return groupIndexChanges;

                #endregion
            }
            else
            {
                #region Get index changes directly from loaded collection

                if (!RelatedObjectsIndexesRebuilded)
                {
                    System.Collections.Generic.List<IndexChange> indexChanges = new System.Collections.Generic.List<IndexChange>();
                    int index = 0;

                    foreach (object _object in this)
                    {
                        int oldIndex = -1;
                        if (RelResolver.LoadedRelatedObjects.Contains(_object))
                            oldIndex = RelResolver.LoadedRelatedObjects.IndexOf(_object);

                        if (index != oldIndex)
                            indexChanges.Add(new IndexChange(StorageInstanceRef.GetStorageInstanceRef(_object) as StorageInstanceRef, oldIndex, index));
                        index++;
                    }
                    foreach (PersistenceLayerRunTime.IndexChange indexChange in indexChanges)
                    {
                        if (indexChange.OldIndex == -1)
                            continue;
                        if (groupIndexChange == null || change != indexChange.NewIndex - indexChange.OldIndex || groupIndexChange.EndIndex + 1 != indexChange.OldIndex)
                        {
                            if (groupIndexChange != null)
                                groupIndexChanges.Add(groupIndexChange);
                            groupIndexChange = new GroupIndexChange();
                            groupIndexChange.StartIndex = indexChange.OldIndex;
                            groupIndexChange.EndIndex = indexChange.OldIndex;
                            change = indexChange.NewIndex - indexChange.OldIndex;
                            groupIndexChange.Change = change;
                        }
                        if (groupIndexChange != null)
                            groupIndexChange.EndIndex = indexChange.OldIndex;

                    }
                    if (groupIndexChange != null)
                        groupIndexChanges.Add(groupIndexChange);

                    groupIndexChanges.Sort(new Comparison<GroupIndexChange>(Ascend));
                }



                return groupIndexChanges;

                #endregion
            }


        }

        bool RelatedObjectsIndexesRebuilded;

        public void IndexRebuilded(string localTransactionUri)
        {
            foreach (var collectionChange in GetCollectionChanges(Transactions.Transaction.Current))
                collectionChange.RelatedObjectsIndexesRebuilded = true;
            //RelatedObjectsIndexesRebuilded = true;
        }

        static int Descend(GroupIndexChange x, GroupIndexChange y)
        {
            return y.StartIndex.CompareTo(x.StartIndex);
        }
        static int Ascend(GroupIndexChange x, GroupIndexChange y)
        {
            return x.StartIndex.CompareTo(y.StartIndex);
        }


    }



    /// <MetaDataID>{16B8EACA-C5EF-4C79-8AFF-25D6F2E805F0}</MetaDataID>
    /// <summary>
    /// The Object of this type keeps information about the change in related objects collection
    /// </summary>
    internal class CollectionChange
    {

        /// <MetaDataID>{b23da86f-1e81-4a6f-8a14-454bbf579707}</MetaDataID>
        public bool ChangeApplied;

        public bool RelatedObjectsIndexesRebuilded;

        int _Index;

        /// <summary>
        /// The zero-based index at which item should be inserted.
        /// Used only when change type is Added
        /// if the index is -1 and the change type is Added then the object added at the end of collection
        /// </summary>
        /// <MetaDataID>{b4e36b67-c94c-4f0b-8a9d-f99c1a7af5ab}</MetaDataID>
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
            }
        }

        /// <MetaDataID>{E4E250DA-BDC1-4849-834C-1F5E0B20C885}</MetaDataID>
        /// <summary>
        /// This field defines the type of change.
        /// </summary>

        public ChangeType TypeOfChange;


        /// <MetaDataID>{88f229a3-4e5c-4cde-ac19-d52155763904}</MetaDataID>
        public bool Implicitly = false;

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
        /// <MetaDataID>{E4D40EAB-1B91-478F-B288-42F581A7E2BF}</MetaDataID>
        public CollectionChange(object _object, ChangeType typeOfChange)
            : this(_object, typeOfChange, -1, false)
        {
        }

        /// <summary>
        /// Create an instance of RelationChangement.
        /// </summary>
        /// <param name="_object">
        /// This parameter defines the object which added or deleted from related objects collection.
        /// </param>
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
        public CollectionChange(object _object, ChangeType typeOfChange, bool implicitly)
            : this(_object, typeOfChange, -1, implicitly)
        {

        }



        /// <summary>
        /// Create an instance of RelationChangement.
        /// </summary>
        /// <param name="_object">
        /// This parameter defines the object which added or deleted from related objects collection.
        /// </param>
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
        public CollectionChange(object _object, ChangeType typeOfChange, int index, bool implicitly)
        {
            Object = _object;
            TypeOfChange = typeOfChange;
            Index = index;
            Implicitly = implicitly;

            if (Object is MultilingualObjectLink && ((MultilingualObjectLink)Object).LinkedObject == null)
            {

            }
        }

    }

    /// <MetaDataID>{147fb4d2-d6d3-426a-a70a-c8c17f27046b}</MetaDataID>
    public interface IndexedCollection
    {
        /// <MetaDataID>{3e5f0193-0746-4955-b29e-b0b1ec3ba5f3}</MetaDataID>
        System.Collections.Generic.List<GroupIndexChange> GetIndexChanges(string transactionUri);
        void IndexRebuilded(string localTransactionUri);

        /// <MetaDataID>{98115ff8-6d3f-48d3-b3f6-eef4dc9bcb2e}</MetaDataID>
        RelResolver RelResolver
        {
            get;
        }

    }

    /// <MetaDataID>{6a022835-6ad2-4324-a6f4-2c954dd1c5c2}</MetaDataID>
    public class IndexChange
    {
        /// <MetaDataID>{c477793c-625f-4a32-b569-e66a044ae89a}</MetaDataID>
        public IndexChange(StorageInstanceRef collectionItem, int oldIndex, int newIndex)
        {
            CollectionItem = collectionItem;
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }
        /// <MetaDataID>{61ccbb02-64d6-4067-b7bb-084d64965890}</MetaDataID>
        public static int Compare(IndexChange x, IndexChange y)
        {
            return x.OldIndex.CompareTo(y.OldIndex);
        }
        /// <MetaDataID>{9bf563c5-fab7-49ce-91a9-9476e20a9cec}</MetaDataID>
        public readonly int OldIndex;
        /// <MetaDataID>{f6508b8c-5ef7-478a-99ca-1a2abb948bb8}</MetaDataID>
        public readonly int NewIndex;
        /// <MetaDataID>{14cca152-167f-4e85-8c88-d963be5e00a1}</MetaDataID>
        readonly StorageInstanceRef CollectionItem;

    }

    /// <MetaDataID>{40b71965-0ebf-4a8f-8657-6423dc6f7b5d}</MetaDataID>
    public class GroupIndexChange
    {
        public int StartIndex;
        public int EndIndex;
        public int Change;

    }


}
