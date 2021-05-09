using System;

namespace OOAdvantech.Transactions
{
    ///<summary>
    /// Manage the state of object in transaction duration.
    /// System takes a snapshot of object state on object enlistment and if transaction aborted system rollback to the object snapshot state
    ///</summary>
    /// <MetaDataID>{F94475AD-8F76-4A05-A571-B0261A607FB4}</MetaDataID>
    internal class ObjectStateSnapshot
    {

        /// <MetaDataID>{14afc5a8-108c-4bd1-a12f-d823157df6da}</MetaDataID>
        /// <exclude>Excluded</exclude>
        static AccessorBuilder.FastInvokeHandler _InitObjectFastInvoke;
        /// <MetaDataID>{47185f8e-71e2-4df0-93a6-b7fcb391a6b5}</MetaDataID>
        ///<summary>
        ///This property defines a fast invoke delegator to the DotNetMetaDataRepository.Assembly  InitObject static method.
        ///</summary>
        static AccessorBuilder.FastInvokeHandler InitObjectFastInvoke
        {
            get
            {
                if (_InitObjectFastInvoke == null)
                {
#if DeviceDotNet
                    _InitObjectFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").GetMetaData().GetMethod("InitObject", AccessorBuilder.AllMembers));
#else
                    _InitObjectFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.Assembly", "").GetMetaData().GetMethod("InitObject", AccessorBuilder.AllMembers));
#endif
                }
                return _InitObjectFastInvoke;
            }
        }
        /// <summary>
        /// This member defines an object snapshot. it is a dictonary with the field metadata of object class hierarchy 
        /// and the values of object field when system took the snapshot.  
        /// </summary>
        [MetaDataRepository.RoleBMultiplicityRange(0)]
        [MetaDataRepository.RoleAMultiplicityRange(1)]
        [MetaDataRepository.Association("", typeof(System.Reflection.FieldInfo), MetaDataRepository.Roles.RoleA, "496cd2b2-0878-4dba-a32c-f3666fe50dca")]
        [MetaDataRepository.IgnoreErrorCheck]
        System.Collections.Generic.Dictionary<AccessorBuilder.FieldMetadata, object> Snapshot;// = new System.Collections.Generic.Dictionary<AccessorBuilder.FieldMetadata, object>();


        ///<summary>
        ///This member defines the managed object.
        ///</summary>
        /// <MetaDataID>{F1427A50-EA7F-46AD-A098-10523DEE18F2}</MetaDataID>
        internal object MemoryInstance;
        ///<summary>
        ///ObjectStateSnapshot constructor.
        ///</summary>
        ///<param name="memoryInstance">
        ///This parameter has the object where system will takes a snapshoth.
        ///Must be not null.
        ///</param>
        ///<param name="transactionContext">
        /// This parameter defines the transaction context. 
        /// The snapshot keeps the sate of object when enlisted in this transaction context (partial or full).   
        ///</param>
        /// <MetaDataID>{1A1B3291-6DC1-49CB-9806-BF5D4F1DF46E}</MetaDataID>
        public ObjectStateSnapshot(object memoryInstance, TransactionContext transactionContext)
        {
            if (memoryInstance == null)
                throw new System.ArgumentNullException("System can't takes snapshot of nothing object.");
            MemoryInstance = memoryInstance;
            TransactionContext = transactionContext;
            InitObjectFastInvoke(null, new object[2] { MemoryInstance, null });

            Snapshot = new System.Collections.Generic.Dictionary<AccessorBuilder.FieldMetadata, object>(AccessorBuilder.LoadTypeMetadata(memoryInstance.GetType()).Fields.Length);
        }
        /// <summary>
        /// Defines a copy constructor. 
        /// Used when system transfer control of object from nested to origin transaction and there isn't ObjectStateSnapshot for origin  
        /// </summary>
        /// <param name="copyObjectStateSnapshot">
        /// This parameter difines the origin ObjectStateSnapshot which system want to copy 
        /// </param>
        /// <param name="transactionContext">
        /// This parameter difines the transaction context which will controls the object state 
        /// </param>
        /// <MetaDataID>{ec3df0e4-e763-412b-8bb2-139171d3c910}</MetaDataID>
        public ObjectStateSnapshot(ObjectStateSnapshot copyObjectStateSnapshot, TransactionContext transactionContext)
        {
            MemoryInstance = copyObjectStateSnapshot.MemoryInstance;
            ThereIsSnapshotForAllFields = copyObjectStateSnapshot.ThereIsSnapshotForAllFields;
            Snapshot = copyObjectStateSnapshot.Snapshot;
            TransactionContext = transactionContext;
            EnsureTransactionalMembersHasSnapshot(MemoryInstance.GetType());

            //Snapshot = new System.Collections.Generic.Dictionary<AccessorBuilder.FieldMetadata, object>(AccessorBuilder.LoadTypeMetadata(MemoryInstance.GetType()).Fields.Length);
        }

        private void EnsureTransactionalMembersHasSnapshot(System.Type objectType)
        {
            if (MemoryInstance is ITransactionalObject)
                (MemoryInstance as ITransactionalObject).EnsureSnapshot(TransactionContext.Transaction);
            else
            {
                AccessorBuilder.TypeMetadata typeMetadata = AccessorBuilder.LoadTypeMetadata(objectType);
                foreach (AccessorBuilder.FieldMetadata fieldMetadata in typeMetadata.Fields)
                {
                    if (fieldMetadata.ContainByValue && Snapshot.ContainsKey(fieldMetadata))
                    {
                        object value = fieldMetadata.GetValue(MemoryInstance);
                        if (value is ITransactionalObject)
                            (value as ITransactionalObject).EnsureSnapshot(TransactionContext.Transaction);
                    }
                }
                if (objectType.GetMetaData().BaseType != null)
                    EnsureTransactionalMembersHasSnapshot(objectType.GetMetaData().BaseType);

            }


        }

        /// <summary>
        /// The ObjectStateSnapshot used from transction context which defines this member. 
        /// </summary>
        /// <MetaDataID>{2b67c111-92f3-4bde-ade4-ff81c976a65b}</MetaDataID>
        public readonly TransactionContext TransactionContext;



        /// <summary>
        /// When managed object enlisted partial the value of member ThereIsSnapshotForAllFields is false
        /// otherwise the member value is true.
        /// </summary>
        /// <MetaDataID>{4a61a972-1f45-450a-9b28-2c09fc2aa7fd}</MetaDataID>
        bool ThereIsSnapshotForAllFields = false;

        ///<summary>
        ///This method takes a full snapshot of managed object state 
        ///</summary>
        /// <MetaDataID>{c44d0980-f77f-4957-91d6-fbf87e5c8167}</MetaDataID>
        public void SnapshotMemoryInstance()
        {

            lock (this)
            {
                if (ThereIsSnapshotForAllFields)
                    return;
                ThereIsSnapshotForAllFields = true;
            }
            if (MemoryInstance is ITransactionalObject)
                (MemoryInstance as ITransactionalObject).MarkChanges(TransactionContext.Transaction);
            else
                SnapshotMemoryInstance(MemoryInstance.GetType(), Snapshot.Count == 0);
        }

        ///<summary>
        ///This method takes a snapshot for field.
        ///</summary>
        ///<param name="fieldInfo">
        ///This parameter defines the field info which system want to take a snapshot
        ///</param>
        /// <MetaDataID>{ebf7e6e0-58e3-4fdd-a34e-695dd121821e}</MetaDataID>
        public void SnapshotMemoryInstance(AccessorBuilder.FieldMetadata fieldMetadata)
        {
            lock (this)
            {
                if (ThereIsSnapshotForAllFields)
                    return;
            }

            if (MemoryInstance is ITransactionalObject)
            {
                lock (this)
                {
                    if (Snapshot.ContainsKey(fieldMetadata))
                        return;
                    Snapshot.Add(fieldMetadata, null);
                }
                (MemoryInstance as ITransactionalObject).MarkChanges(TransactionContext.Transaction, new System.Reflection.FieldInfo[1] { fieldMetadata.FieldInfo });
            }
            else
            {
                object value = null;
                if (Snapshot.ContainsKey(fieldMetadata))
                    return;
                if (fieldMetadata.ContainByValue)
                {
                    //TODO θα πρέπει να ελεγχθεί πως δουλεύει σε περίπτωση που το FieldType είνα interface,struct,array πχ int[].
                    //Ελεγχθηκε
                    value = fieldMetadata.GetValue(MemoryInstance);
                    if (value != null)
                    {
#if !DeviceDotNet
                        if (value is ITransactionalObject)
                            (value as ITransactionalObject).MarkChanges(TransactionContext.Transaction);
                        else if (value is System.ICloneable)
                        {
                            object CloneValue = (value as System.ICloneable).Clone();
                            if (CloneValue.GetType() != value.GetType())
                                throw new OOAdvantech.Transactions.TransactionException("Bad cloning, the clone object '" + CloneValue.GetType().FullName
                                    + "' has deferent type from the origin object   '" + value.GetType().FullName + "'.");
                            value = CloneValue;
                        }
                        else
                            throw new OOAdvantech.Transactions.TransactionException("Only Cloneable you can declare with ContainByValue attribute");
#else
                        if (value is ITransactionalObject)
                            (value as ITransactionalObject).MarkChanges(TransactionContext.Transaction);
                        else
                            throw new OOAdvantech.Transactions.TransactionException("Only Cloneable you can declare with ContainByValue attribute");
#endif

                    }
                }
                else
                    value = fieldMetadata.GetValue(MemoryInstance);

                Snapshot[fieldMetadata] = value;


            }
        }
        /// <MetaDataID>{C53757B3-7D22-4210-9720-28AE161E6177}</MetaDataID>
        /// <summary>
        /// This method takes a snapshot recursively for all class hierarchy
        /// Method uses the type to take the fields infos to catch values for snapshot 
        /// and continue the recursive call with BaseType of type. 
        /// </summary>
        /// <param name="type">
        /// Method uses the parameter type to take the fields infos to catch values for snapshot 
        /// and continue the recursive call with BaseType of type. 
        /// </param>
        void SnapshotMemoryInstance(System.Type type, bool newSnapshot)
        {
            string fullName = type.FullName;

            AccessorBuilder.TypeMetadata typeMetadata = AccessorBuilder.LoadTypeMetadata(type);
            foreach (AccessorBuilder.FieldMetadata fieldMetadata in typeMetadata.Fields)
            {

                if (!newSnapshot && Snapshot.ContainsKey(fieldMetadata))
                    continue;

                object value = null;
                if (fieldMetadata.ContainByValue)
                {
                    //TODO θα πρέπει να ελεγχθεί πως δουλεύει σε περίπτωση που το FieldType είνα interface,struct,array πχ int[].
                    //Ελεγχθηκε
                    value = fieldMetadata.GetValue(MemoryInstance);
                    if (value != null)
                    {
#if !DeviceDotNet
                        if (value is ITransactionalObject)
                            (value as ITransactionalObject).MarkChanges(TransactionContext.Transaction);
                        else if (value is System.ICloneable)
                        {
                            object CloneValue = (value as System.ICloneable).Clone();
                            if (CloneValue.GetType() != value.GetType())
                                throw new OOAdvantech.Transactions.TransactionException("Bad cloning, the clone object '" + CloneValue.GetType().FullName
                                    + "' has deferent type from the origin object   '" + value.GetType().FullName + "'.");
                            value = CloneValue;
                        }
                        else
                            throw new OOAdvantech.Transactions.TransactionException("Only Cloneable you can declare with ContainByValue attribute");
#else
                        if (value is ITransactionalObject)
                            (value as ITransactionalObject).MarkChanges(TransactionContext.Transaction);
                        else
                            throw new OOAdvantech.Transactions.TransactionException("Only Cloneable you can declare with ContainByValue attribute");
#endif
                    }
                }
                else
                    value = fieldMetadata.GetValue(MemoryInstance);

                Snapshot.Add(fieldMetadata, value);
            }
            if (type.GetMetaData().BaseType != null)
                SnapshotMemoryInstance(type.GetMetaData().BaseType, newSnapshot);

        }

        ///<summary>
        ///This method moves the object to the origin state, 
        ///the state before the enlistment in transaction context 
        ///</summary>
        /// <MetaDataID>{EB0123A0-F281-40BA-9210-D6203D0B85AF}</MetaDataID>
        public void Rollback()
        {
            if (MemoryInstance is ITransactionalObject)
            {
                try
                {
                    (MemoryInstance as ITransactionalObject).UndoChanges(TransactionContext.Transaction);
                }
                catch (System.Exception error)
                {
                    //this try cacth scoop protect transaction roll back from bad custom roll back code
                }
            }
            else
            {
                foreach (System.Collections.Generic.KeyValuePair<OOAdvantech.AccessorBuilder.FieldMetadata, object> entry in Snapshot)
                {
                    try
                    {
                        OOAdvantech.AccessorBuilder.FieldMetadata fieldMetadata = (OOAdvantech.AccessorBuilder.FieldMetadata)entry.Key;
                        if (fieldMetadata.ContainByValue && entry.Value == null)
                        {
                            ITransactionalObject transactionalObject = fieldMetadata.GetValue(MemoryInstance) as ITransactionalObject;
                            if (transactionalObject != null)
                                transactionalObject.UndoChanges(TransactionContext.Transaction);
                        }
                        if (fieldMetadata.ContainByValue && entry.Value is ITransactionalObject)
                            (entry.Value as ITransactionalObject).UndoChanges(TransactionContext.Transaction);
                        else
                            ((OOAdvantech.AccessorBuilder.FieldMetadata)entry.Key).SetValue(MemoryInstance, entry.Value);
                    }
                    catch (System.Exception Error)
                    {
                        //this try cacth scoop protect transaction roll back from bad custom roll back code
                    }
                }
            }

        }
        ///<summary>
        ///Commits the object state, this state is available for alls
        ///</summary>
        /// <MetaDataID>{F0E93C47-381B-4A51-81FE-2261A767D4BB}</MetaDataID>
        public void Commit()
        {
            //MemoryInstance is OOAdvantech.MetaDataRepository.Attribute && (MemoryInstance as OOAdvantech.MetaDataRepository.Attribute).Namespace!=null&&(MemoryInstance as OOAdvantech.MetaDataRepository.Attribute).Name=="Name"&&(MemoryInstance as OOAdvantech.MetaDataRepository.Attribute).Namespace.Name=="IStorePlace"
            lock (MemoryInstance)
            {
                if (MemoryInstance is ITransactionalObject)
                {

                    try
                    {
                        (MemoryInstance as ITransactionalObject).CommitChanges(TransactionContext.Transaction);
                    }
                    catch (System.Exception error)
                    {
                        //this try cacth scoop protect transaction commit from bad custom roll back code
                    }

                }
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<AccessorBuilder.FieldMetadata, object> entry in Snapshot)
                    {
                        try
                        {
                            AccessorBuilder.FieldMetadata fieldMetadata = entry.Key;

                            if (fieldMetadata.ContainByValue && entry.Value == null)
                            {
                                ITransactionalObject transactionalObject = fieldMetadata.GetValue(MemoryInstance) as ITransactionalObject;
                                if (transactionalObject != null)
                                    transactionalObject.CommitChanges(TransactionContext.Transaction);
                            }
                            if (fieldMetadata.ContainByValue && entry.Value is ITransactionalObject)
                                (entry.Value as ITransactionalObject).CommitChanges(TransactionContext.Transaction);

                        }
                        catch (System.Exception error)
                        {
                            //this try cacth scoop protect transaction commit from bad custom roll back code
                        }
                    }

                    if (TransactionContext.Transaction.OriginTransaction != null)
                        return;
                    System.Collections.Generic.Dictionary<string, LockedObjectEntry> lockedObjectEntries = null;
                    lock (LockedObjectEntry.LockedObjects)
                    {
                        LockedObjectEntry.LockedObjects.TryGetValue(MemoryInstance, out lockedObjectEntries);
                    }
                    if (lockedObjectEntries.Count == 1 && lockedObjectEntries.ContainsKey(TransactionContext.Transaction.LocalTransactionUri))
                        lockedObjectEntries = null;

                    #region this code used in shared fields and set others snapshot rollback value with current fileld value

                    if (lockedObjectEntries != null)
                    {
                        LockedObjectEntry ownerlockedObjectEntry = lockedObjectEntries[TransactionContext.Transaction.LocalTransactionUri];
                        foreach (LockedObjectEntry lockedObjectEntry in lockedObjectEntries.Values)
                        {
                            if (lockedObjectEntry.TransactionContext != TransactionContext)
                            {
                                if (lockedObjectEntry.SharedLockMembers != null)
                                {
                                    foreach (TransactionalMemberAttribute transactionalMember in lockedObjectEntry.SharedLockMembers)
                                    {
                                        object filedValue = transactionalMember.FieldMetadata.GetValue(MemoryInstance);
                                        foreach (ObjectStateSnapshot objectStateSnapshot in lockedObjectEntry.ObjectStateSnapshots.Values)
                                        {
                                            if (objectStateSnapshot.Snapshot.ContainsKey(transactionalMember.FieldMetadata))
                                                objectStateSnapshot.Snapshot[transactionalMember.FieldMetadata] = filedValue;
                                        }
                                    }
                                }
                                if (ownerlockedObjectEntry.SharedLockMembers != null)
                                {
                                    foreach (TransactionalMemberAttribute transactionalMember in ownerlockedObjectEntry.SharedLockMembers)
                                    {
                                        object filedValue = transactionalMember.FieldMetadata.GetValue(MemoryInstance);
                                        foreach (ObjectStateSnapshot objectStateSnapshot in lockedObjectEntry.ObjectStateSnapshots.Values)
                                        {
                                            if (objectStateSnapshot.Snapshot.ContainsKey(transactionalMember.FieldMetadata))
                                                objectStateSnapshot.Snapshot[transactionalMember.FieldMetadata] = filedValue;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    #endregion



                }
            }

        }


        ///<summary>
        ///This method adds changes of nested transaction to the origin 
        ///when nested transaction committed.
        ///This method has value when the transaction of transaction context is nested transaction. T
        ///his method belongs to object state management mechanism where system records the changes of object state.
        ///</summary>
        /// <MetaDataID>{758a88ac-fc3f-42b5-9a76-71e951b36387}</MetaDataID>
        internal void Merge(Transaction masterTransaction, TransactionRunTime nestedTransaction)
        {
            if (MemoryInstance is ITransactionalObject)
                (MemoryInstance as ITransactionalObject).MergeChanges(masterTransaction, nestedTransaction);
            else
                Merge(MemoryInstance.GetType(), masterTransaction, nestedTransaction);



        }
        ///<summary>
        ///Recursive call of merge in type hierarchy
        ///</summary>
        /// <MetaDataID>{2fc38f3e-b987-45a7-b3fb-ad8fc67aa9f1}</MetaDataID>
        void Merge(System.Type objectType, Transaction masterTransaction, TransactionRunTime nestedTransaction)
        {
            AccessorBuilder.TypeMetadata typeMetadata = AccessorBuilder.LoadTypeMetadata(objectType);
            foreach (AccessorBuilder.FieldMetadata fieldMetadata in typeMetadata.Fields)
            {
                if (fieldMetadata.ContainByValue && Snapshot.ContainsKey(fieldMetadata))
                {
                    object value = fieldMetadata.GetValue(MemoryInstance);
                    if (value is ITransactionalObject)
                        (value as ITransactionalObject).MergeChanges(masterTransaction, nestedTransaction);
                }
            }
            if (objectType.GetMetaData().BaseType != null)
                Merge(objectType.GetMetaData().BaseType, masterTransaction, nestedTransaction);

        }

        internal bool ObjectHasGhanged()
        {

            {
                if (MemoryInstance is ITransactionalObject)
                {
                    try
                    {
                        return (MemoryInstance as ITransactionalObject).ObjectHasGhanged(TransactionContext.Transaction);
                    }
                    catch (System.Exception error)
                    {
                        return true;
                        //this try cacth scoop protect transaction roll back from bad custom roll back code
                    }
                }
                else
                {
                    bool hasGhanged = false;
                    foreach (System.Collections.Generic.KeyValuePair<OOAdvantech.AccessorBuilder.FieldMetadata, object> entry in Snapshot)
                    {
                        try
                        {
                            OOAdvantech.AccessorBuilder.FieldMetadata fieldMetadata = (OOAdvantech.AccessorBuilder.FieldMetadata)entry.Key;
                            if (fieldMetadata.ContainByValue && entry.Value == null)
                            {
                                ITransactionalObject transactionalObject = fieldMetadata.GetValue(MemoryInstance) as ITransactionalObject;
                                if (transactionalObject != null)
                                    hasGhanged |= transactionalObject.ObjectHasGhanged(TransactionContext.Transaction);
                            }
                            if (fieldMetadata.ContainByValue && entry.Value is ITransactionalObject)
                                hasGhanged |= (entry.Value as ITransactionalObject).ObjectHasGhanged(TransactionContext.Transaction);
                            else
                                hasGhanged |= ((OOAdvantech.AccessorBuilder.FieldMetadata)entry.Key).GetValue(MemoryInstance) != entry.Value;
                        }
                        catch (System.Exception Error)
                        {
                            //this try cacth scoop protect transaction roll back from bad custom roll back code
                        }
                    }

                    return hasGhanged;
                }

            }

        }
    }
}
