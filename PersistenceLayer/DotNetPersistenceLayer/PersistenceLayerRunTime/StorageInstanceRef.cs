namespace OOAdvantech.PersistenceLayerRunTime
{
    using System.Collections.Generic;
    using System.Reflection;
    using OOAdvantech.Transactions;
    using OOAdvantech.MetaDataRepository;
    using System;
    using System.Linq;

    //Sos gg gggt

#if PORTABLE
    using System.PCL.Reflection;
    using Remoting;
#endif
#if DeviceDotNet
    using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
    using System;
    using OOAdvantech.DotNetMetaDataRepository;
    using System.Runtime.InteropServices.ComTypes;
    using System.Threading;
    using OOAdvantech.Remoting;
#endif
    internal delegate void StorageInstanceRefChangeState(object sender);

    public delegate void ObjectDeleted(object sender);
    /// <MetaDataID>{56A446A4-A5D0-4E19-80F9-BCF25FC0AE80}</MetaDataID>
    /// <summary>VT(8)RestoreLastSnapshot </summary>
    [Transactions.Transactional]
    public abstract class StorageInstanceRef : PersistenceLayer.StorageInstanceRef, Transactions.ITransactionalObject
    {
        /// <summary>
        /// Structure defines the value and metadata of object member.
        /// Metadata are necessary to set or get value from object   
        /// </summary>
        public struct ValueOfAttribute
        {
            /// <summary>
            /// Defines the Attribute
            /// </summary>
            public readonly DotNetMetaDataRepository.Attribute Attribute;
            /// <summary>
            /// Defines the field which contains the value of attribute. 
            /// </summary>
            public readonly System.Reflection.FieldInfo FieldInfo;
            /// <summary>
            /// Defines the accessor to set or get value.
            /// </summary>
            public readonly AccessorBuilder.FieldPropertyAccessor FastFieldAccessor;

            /// <summary>
            /// Defines the value of attribute
            /// </summary>
            public readonly object Value;
            /// <summary>
            /// Defines the identity of path to set or get value of attribute
            /// </summary>
            public readonly string PathIdentity;
            /// <summary>
            /// Defines the metadata to access the value of attribute in case where the attribute is member of value tyoe
            /// </summary>
            public readonly MetaDataRepository.ValueTypePath ValueTypePath;

            /// <summary>
            /// Defines the path description to set or get attribute value;
            /// </summary>
            public readonly string Path;

            public readonly bool IsMultilingual;
            public readonly System.Globalization.CultureInfo Culture;
            public ValueOfAttribute(DotNetMetaDataRepository.Attribute attribute, bool isMultilingual, System.Reflection.FieldInfo fieldInfo, AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, object value, MetaDataRepository.ValueTypePath valueTypePath, string path, System.Globalization.CultureInfo culture)
            {
                IsMultilingual = isMultilingual;
                FastFieldAccessor = fastFieldAccessor;
                Attribute = attribute;
                FieldInfo = fieldInfo;
                Value = value;
                if (isMultilingual && Value == null)
                {

                }

                PathIdentity = valueTypePath.ToString();
                ValueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath(valueTypePath);
                Path = path;
                Culture = culture;
            }
        }
        /// <MetaDataID>{4b054f2a-7df9-4056-abef-b52a1687d338}</MetaDataID>
        static string Lock = "";
        /// <exclude>Excluded</exclude>
        static int _NextMemoryID = 0;
        /// <MetaDataID>{62665fc1-dce2-4a9b-a984-fcc0fce62011}</MetaDataID>
        internal static int NextMemoryID
        {
            get
            {
                lock (Lock)
                {
                    _NextMemoryID++;
                    return _NextMemoryID;
                }
            }
        }

        public Guid InstantiateObjectQueryIdentity { get; set; }



        /// <MetaDataID>{e8a599a0-ee80-45cb-bb0b-318a3618cd79}</MetaDataID>
        public readonly int MemoryID = NextMemoryID;
        /// <MetaDataID>{2BD73C1E-00C9-43AB-880C-1C60BFC8A177}</MetaDataID>
        internal event StorageInstanceRefChangeState ChangeState;


        ///<summary>
        ///This method returns the storage instance ref agent of persistentObject.
        ///
        ///</summary>
        /// <MetaDataID>{58B0175C-298C-418A-901F-3730138118E3}</MetaDataID>
        public static StorageInstanceAgent GetStorageInstanceAgent(object persistentObject)
        {
            return (PersistenceLayer.ObjectStorage.PersistencyService as PersistencyService).GetStorageInsatnceAgent(persistentObject);
        }

        /// <MetaDataID>{1e0a8b82-c4f5-4587-a018-a27afd5a6964}</MetaDataID>
        protected StorageCell _StorageInstanceSet;


        ///<summary>
        ///Defines the collection with relation resolvers  
        ///</summary>
        /// <MetaDataID>{3CD0F639-D1C2-4F28-AE81-D01069901314}</MetaDataID>
        [Association("", Roles.RoleB, "47e85820-f68b-401d-b3e3-47a0488672d1")]
        [IgnoreErrorCheck]
        public System.Collections.Generic.List<RelResolver> RelResolvers = new List<RelResolver>();

        OOAdvantech.ObjectStateManagerLink _ObjectStateManagerLink;
        /// <MetaDataID>{D29F974D-CE3C-44F1-BB6C-017E49B2A8A5}</MetaDataID>
        private OOAdvantech.ObjectStateManagerLink ObjectStateManagerLink
        {
            get
            {
                if (_ObjectStateManagerLink == null && MemoryInstance != null)
                    _ObjectStateManagerLink = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(MemoryInstance);

                return _ObjectStateManagerLink;
            }
        }


        /// <MetaDataID>{0f284bf6-b0a0-4f02-8ecd-2be0e098bfdc}</MetaDataID>
        internal override bool IsRelationLoaded(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd)
        {
            if (Class.ClassHierarchyLinkAssociation != null && Class.ClassHierarchyLinkAssociation.Identity == associationEnd.Association.Identity)
                return true;
            foreach (var relResolver in RelResolvers)
            {
                if (relResolver.AssociationEnd.Identity == associationEnd.Identity)
                    return relResolver.IsCompleteLoaded;
            }
            return false;
        }

        #region Transactions.TransactionalObject members implementation.

        /// <MetaDataID>{A266D609-E8F3-4E07-95D5-FE4B61FEAD79}</MetaDataID>
        public virtual void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            StorageInstanceReferentialIntegrityCount = _ReferentialIntegrityCount;
            (_RuntimeReferentialIntegrityCount as ITransactionalObject).CommitChanges(transaction);
            _RuntimeReferentialIntegrityCount.Value = StorageInstanceReferentialIntegrityCount;
            StorageInstanceRefSnapshot.Clear();
            StorageInstanceValuesSnapshot = null;



        }
        /// <MetaDataID>{dd462746-e9eb-4c98-a775-783c603083b4}</MetaDataID>
        Dictionary<string, Dictionary<FieldInfo, object>> StorageInstanceValuesSnapshot = null;
        /// <MetaDataID>{40F1333C-B5D2-4835-852B-FEBF17790FD1}</MetaDataID>
        public virtual void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            //if(StorageInstanceValuesSnapshot ==null)
            StorageInstanceValuesSnapshot = new Dictionary<string, Dictionary<FieldInfo, object>>();

            foreach (KeyValuePair<string, Dictionary<FieldInfo, object>> entry in StorageInstanceValues)
                StorageInstanceValuesSnapshot.Add(entry.Key, new Dictionary<FieldInfo, object>(entry.Value));
            StorageInstanceRefSnapshot["LifeTimeController"] = LifeTimeController;
            StorageInstanceRefSnapshot["ObjectID"] = PersistentObjectID;
            //	StorageInstanceRefSnapshot["_ReferentialIntegrityCount"]=this._ReferentialIntegrityCount;
            StorageInstanceRefSnapshot["_ActiveStorageSession"] = this._ObjectStorage;
            (_RuntimeReferentialIntegrityCount as OOAdvantech.Transactions.ITransactionalObject).MarkChanges(transaction);
            if (PersistentObjectID == null)
                OOAdvantech.Transactions.LockedObjectEntry.ExclusiveLock(MemoryInstance, (transaction as Transactions.TransactionRunTime).TransactionContext);

        }

        /// <MetaDataID>{dd158683-2c4a-490c-a52b-6498d843fb4f}</MetaDataID>
        public virtual void MarkChanges(OOAdvantech.Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            foreach (System.Reflection.FieldInfo fieldInfo in fields)
            {
                ITransactionalObject transactionalObject = OOAdvantech.AccessorBuilder.GetFieldAccessor(fieldInfo).GetValue(this) as ITransactionalObject;
                if (transactionalObject != null)
                    transactionalObject.MarkChanges(transaction);
            }
        }


        /// <MetaDataID>{8250c872-5910-44e3-b9c6-3c44b9ade251}</MetaDataID>
        public virtual void MergeChanges(OOAdvantech.Transactions.Transaction mergeInTransaction, OOAdvantech.Transactions.Transaction mergedTransaction)
        {
            (_RuntimeReferentialIntegrityCount as OOAdvantech.Transactions.ITransactionalObject).MergeChanges(mergeInTransaction, mergedTransaction);
        }
        public void EnsureSnapshot(Transaction transaction)
        {
            (_RuntimeReferentialIntegrityCount as OOAdvantech.Transactions.ITransactionalObject).EnsureSnapshot(transaction);
        }

        /// <MetaDataID>{004C4FEF-09B1-486B-83D2-56D90BBB04D6}</MetaDataID>
        public virtual void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            //TODO ο κώδικας που διαχειρίζεται τα transaction για λογαριασμό του StorageInstanceRef 
            //είναι για κλάματα
            _ReferentialIntegrityCount = StorageInstanceReferentialIntegrityCount;
            if (StorageInstanceRefSnapshot.ContainsKey("ObjectID"))
                PersistentObjectID = (OOAdvantech.PersistenceLayer.ObjectID)StorageInstanceRefSnapshot["ObjectID"];
            if (StorageInstanceRefSnapshot.ContainsKey("LifeTimeController"))
                LifeTimeController = StorageInstanceRefSnapshot["LifeTimeController"] as ClassMemoryInstanceCollection;
            //	StorageInstanceRefSnapshot.Add("_ReferentialIntegrityCount",this._ReferentialIntegrityCount);
            if (StorageInstanceRefSnapshot.ContainsKey("_ActiveStorageSession"))
                _ObjectStorage = StorageInstanceRefSnapshot["_ActiveStorageSession"] as OOAdvantech.PersistenceLayer.ObjectStorage;

            if (LifeTimeController != null && _PersistentObjectID != null)
                if (LifeTimeController[_PersistentObjectID] != this)
                    LifeTimeController[_PersistentObjectID] = this;
            if (StorageInstanceValuesSnapshot != null)
            {
                StorageInstanceValues = StorageInstanceValuesSnapshot;
                foreach (RelResolver relResolver in RelResolvers)
                {
                    System.Reflection.FieldInfo associationEndFieldInfo = relResolver.FieldInfo;
                    if (associationEndFieldInfo.FieldType != typeof(PersistenceLayer.ObjectContainer) && !associationEndFieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                    {
                        System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> filedsValues = null;

                        if (StorageInstanceValues != null && StorageInstanceValues.TryGetValue(relResolver.ValueTypePath.ToString(), out filedsValues))
                        {
                            object relatedObject = null;
                            if (filedsValues.TryGetValue(associationEndFieldInfo, out relatedObject))
                            {
                                if (relResolver.Multilingual)
                                {
                                    if (relatedObject == null)
                                        relResolver.ResetMultilingualRelatedObject(new List<MultilingualObjectLink>());
                                    else
                                        relResolver.ResetMultilingualRelatedObject(relatedObject as List<MultilingualObjectLink>);
                                }
                                else
                                    relResolver.RelatedObject = relatedObject;
                            }
                        }
                    }
                }
            }

            (_RuntimeReferentialIntegrityCount as ITransactionalObject).UndoChanges(transaction);

        }



        public virtual bool ObjectHasGhanged(TransactionRunTime transaction)
        {

            if (_ReferentialIntegrityCount != StorageInstanceReferentialIntegrityCount)
                return true;
            if (StorageInstanceRefSnapshot.ContainsKey("ObjectID") && PersistentObjectID != (OOAdvantech.PersistenceLayer.ObjectID)StorageInstanceRefSnapshot["ObjectID"])
                return true;

            if (StorageInstanceRefSnapshot.ContainsKey("LifeTimeController") && LifeTimeController != StorageInstanceRefSnapshot["LifeTimeController"] as ClassMemoryInstanceCollection)
                return true;

            //	StorageInstanceRefSnapshot.Add("_ReferentialIntegrityCount",this._ReferentialIntegrityCount);
            if (StorageInstanceRefSnapshot.ContainsKey("_ActiveStorageSession") && _ObjectStorage != StorageInstanceRefSnapshot["_ActiveStorageSession"] as OOAdvantech.PersistenceLayer.ObjectStorage)
                return true;

            if (LifeTimeController != null && _PersistentObjectID != null && LifeTimeController[_PersistentObjectID] != this)
                return true;

            if (StorageInstanceValuesSnapshot != null)
            {
                if (StorageInstanceValues != StorageInstanceValuesSnapshot)
                    return true;
                foreach (RelResolver relResolver in RelResolvers)
                {
                    System.Reflection.FieldInfo associationEndFieldInfo = relResolver.FieldInfo;
                    if (associationEndFieldInfo.FieldType != typeof(PersistenceLayer.ObjectContainer) && !associationEndFieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                    {
                        System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> filedsValues = null;

                        if (StorageInstanceValues != null && StorageInstanceValues.TryGetValue(relResolver.ValueTypePath.ToString(), out filedsValues))
                        {
                            object relatedObject = null;
                            if (filedsValues.TryGetValue(associationEndFieldInfo, out relatedObject))
                            {
                                if (relResolver.Multilingual)
                                {
                                    if (relatedObject == null)
                                    {
                                        if (relResolver.HasMultilingualRelatedObjectChanges(new List<MultilingualObjectLink>()))
                                            return true;
                                    }
                                    else
                                        if (relResolver.HasMultilingualRelatedObjectChanges(relatedObject as List<MultilingualObjectLink>))
                                        return true;
                                }
                                else
                                    if (relResolver.RelatedObject != relatedObject)
                                    return true;
                            }
                        }
                    }
                }
            }
            return (_RuntimeReferentialIntegrityCount as ITransactionalObject).ObjectHasGhanged(transaction);
        }


        #endregion


        /// <MetaDataID>{ea3ff4a1-1518-48d4-b9b9-81ba1dc17329}</MetaDataID>
        public override bool IsPersistent
        {
            get
            {
                if (PersistentObjectID != null)
                {
                    //TODO να τεσταριστεί όταν σβηστεί το memory instance και δεν έχει κλείσει το transaction
                    object snapShotObjectID = null;
                    if (StorageInstanceRefSnapshot.TryGetValue("ObjectID", out snapShotObjectID))
                    {
                        // if StorsgeInstanceRef is under transaction check if there was ObjectID before transaction.
                        if (snapShotObjectID == null)
                            return false;
                        else
                            return true;
                    }
                    else
                        return true;


                }
                return false;
            }
        }

        #region PersistenceLayerRunTime.StorageInstanceRef abstract members implementation
        /// <MetaDataID>{CADA62B4-DD8C-41B9-A6B9-02105971B93C}</MetaDataID>
        public override void LazyFetching(string FieldName, System.Type declaringType)
        {
            //TODO θα πρεπει να υλοποιειθεί και για attribute

            // ********* The object Collections has the knowledge for Lazy Fetching ************
            System.Reflection.MemberInfo member = declaringType.GetMetaData().GetProperty(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (member == null)
                member = declaringType.GetMetaData().GetField(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (member == null)
                throw new System.Exception("There isn't member with name " + FieldName + " in type " + declaringType.FullName); // message
            MetaDataRepository.MetaObject metaObject = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(member);
            if (metaObject is DotNetMetaDataRepository.AssociationEndRealization &&
                ((metaObject as DotNetMetaDataRepository.AssociationEndRealization).Specification.Multiplicity.IsMany ||
                !(metaObject as DotNetMetaDataRepository.AssociationEndRealization).Persistent))
                return;
            if (metaObject is DotNetMetaDataRepository.AssociationEnd &&
                ((metaObject as DotNetMetaDataRepository.AssociationEnd).Multiplicity.IsMany ||
                !(metaObject as DotNetMetaDataRepository.AssociationEnd).Persistent))
                return;


            // ********* The object Collections has the knowledge for Lazy Fetching ************

            RelResolver relResolver = null;
            foreach (RelResolver currRelResolver in RelResolvers)
            {
                if (metaObject is DotNetMetaDataRepository.AssociationEnd && currRelResolver.AssociationEnd == metaObject)
                {
                    relResolver = currRelResolver;
                    break;
                }
                if (metaObject is DotNetMetaDataRepository.AssociationEndRealization && (metaObject as DotNetMetaDataRepository.AssociationEndRealization).Specification == currRelResolver.AssociationEnd)
                {
                    relResolver = currRelResolver;
                    break;
                }
            }
            LazyFetching(relResolver, declaringType);
        }



        /// <MetaDataID>{dee2fb3d-0fd6-4daa-bdd5-955c45cbd699}</MetaDataID>
        protected internal override void LazyFetching(object relResolver, System.Type declaringType)
        {
            if (this._PersistentObjectID != null)
                LazyFetching(relResolver as RelResolver, declaringType);
        }


        /// <MetaDataID>{6E20CCF9-CCB5-44EE-9C57-AABE17AB88C2}</MetaDataID>
        internal void LazyFetching(RelResolver relResolver, System.Type declaringType)
        {
            lock (this)
            {
                DotNetMetaDataRepository.Class declaringTypeClass = DotNetMetaDataRepository.MetaObjectMapper.FindMetaObjectFor(declaringType) as DotNetMetaDataRepository.Class;

                relResolver.CompleteLoad();
                if (relResolver.AssociationEnd.Multiplicity.IsMany)
                    System.Diagnostics.Debug.WriteLine(declaringType.FullName + "." + relResolver.AssociationEnd.Name + ": Role Constraints mismatch");

                if (relResolver.Multilingual)
                {
                    foreach (var culture in relResolver.MultilingualRelatedObject.Keys)
                    {
                        using (CultureContext cultureContext = new CultureContext(culture, false))
                        {
                            object linkedObject = relResolver.RelatedObject;
                            if (linkedObject != null)
                            {
                                object memoryInstance = MemoryInstance;
                                AccessorBuilder.FieldPropertyAccessor associationEndFastFieldAccessor = declaringTypeClass.GetFastFieldAccessor(relResolver.AssociationEnd);
                                Member<object>.SetValueImplicitly(associationEndFastFieldAccessor, ref memoryInstance, linkedObject);
                            }
                        }

                    }
                }
                else
                {
                    object linkedObject = relResolver.RelatedObject;
                    if (linkedObject != null)
                    {
                        object memoryInstance = MemoryInstance;
                        AccessorBuilder.FieldPropertyAccessor associationEndFastFieldAccessor = declaringTypeClass.GetFastFieldAccessor(relResolver.AssociationEnd);
                        Member<object>.SetValueImplicitly(associationEndFastFieldAccessor, ref memoryInstance, linkedObject);
                    }
                }
            }
        }




        /// <exclude>Excluded</exclude>
        protected OOAdvantech.PersistenceLayer.ObjectID _PersistentObjectID;
        /// <summary></summary>
        /// <MetaDataID>{B5E70A51-984B-4B34-88D6-7F3B910844B7}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.ObjectID PersistentObjectID
        {
            get
            {

                return _PersistentObjectID;
            }
            set
            {

                //TODO: θα πρεπεί να ληφθεί υπόψην ότι μπορεί να γίνει rollback
                if (value == _PersistentObjectID)
                    return;
                if (value == null)
                {
                    if (LifeTimeController != null)
                    {
                        LifeTimeController.ObjectDestroyed(this);
                        LifeTimeController = null;
                        //ChangeState(this);
                    }
                    _PersistentObjectID = value;


                    return;
                }

                PersistenceLayerRunTime.ObjectStorage mStorage = (PersistenceLayerRunTime.ObjectStorage)ObjectStorage;
                ClassMemoryInstanceCollection ClassObjectCollection = mStorage.OperativeObjectCollections[MemoryInstance.GetType()];
                LifeTimeController = ClassObjectCollection;
                _PersistentObjectID = value;
                ClassObjectCollection[value] = this;
            


                CreateMarshalByRefUri();

            }
        }
        string MarshalByRefUri;
        private void CreateMarshalByRefUri()
        {
            if ((MemoryInstance is MetaDataRepository.MetaObject))
                return;

#if !DeviceDotNet
            if (StorageInstanceSet != null && MarshalByRefUri == null)
            {
                string persistentUri = null;
                if (ObjectID != null && MemoryInstance is MarshalByRefObject)
                    persistentUri = (_ObjectStorage as ObjectStorage).GetPersistentObjectUri(this);


                if (MemoryInstance is System.MarshalByRefObject && MemoryInstance is IExtMarshalByRefObject && !string.IsNullOrEmpty(persistentUri))
                {
                    byte[] data = new byte[0x12];
                    rng.GetBytes(data);
                    string str = System.Convert.ToBase64String(data);
                    str = str.Replace('/', '_') + "_" + Remoting_Identity_GetNextSeqNum() + "#PID#" + persistentUri;
                    MarshalByRefUri = str;
                    System.Runtime.Remoting.RemotingServices.Marshal(MemoryInstance as System.MarshalByRefObject, str);
              
                }
              
            }
        
#endif
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.PersistenceLayer.ObjectID _TransientObjectID;
        /// <MetaDataID>{a0cafccf-bad4-419b-8b96-cf3a66a26832}</MetaDataID>
        public override OOAdvantech.PersistenceLayer.ObjectID ObjectID
        {
            get
            {
                if (_PersistentObjectID != null)
                    return _PersistentObjectID;
                else
                    return _TransientObjectID;

            }

            set
            {
                if (_TransientObjectID != null)
                    throw new System.Exception("TransientObjectID has already value");
                _TransientObjectID = value;
            }
        }


        ///// <MetaDataID>{ec43364c-8fd3-4ad7-8f11-3aeac6cf4547}</MetaDataID>
        //object _MemoryInstanceStrongReference;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C37569E3-2378-49C2-A4E9-E1CE2A0C8C25}</MetaDataID>
        internal object _MemoryInstance;
        /// <MetaDataID>{01112B86-8C8B-4364-986F-00E9BAC54EDC}</MetaDataID>
        public override object MemoryInstance
        {
            get
            {

                return _MemoryInstance;//.Target;

            }
        }


        //public override void WaitUntilObjectIsActive()
        //{
        //    if (this.ObjectID != null && this.ObjectID.ToString() == "37c98759-de79-4361-83a1-94d56fdb788d")
        //    {

        //    }
        //    bool isActive = IsObjectActive;
        //    while (!isActive)
        //    {
        //        Thread.Sleep(100);
        //        isActive = IsObjectActive;
        //    }
        //}

        #endregion

        /// <MetaDataID>{705037EC-205B-43D6-A5EF-F6FD4AC5D389}</MetaDataID>
        /// <summary>This event informs subscribers for the object deletion. 
        /// This event is useful for relation resolvers to clear reference to the memory instance. </summary>
        public event ObjectDeleted ObjectDeleted;

        /// <MetaDataID>{016ADF45-B134-40D1-8B61-C810D9772043}</MetaDataID>
        /// <summary>With this method persistency system 
        /// informs the storage instance ref object that the storage instance removed from storage. </summary>
        internal void OnObjectDeleted()
        {
            if (ObjectDeleted != null)
                ObjectDeleted(this.MemoryInstance);

            RemoveAssignment();

        }



        /// <MetaDataID>{F9FB9B27-1D03-4444-92A8-C158A1BCDC9A}</MetaDataID>
        /// <exclude>Excluded</exclude>
        private OOAdvantech.DotNetMetaDataRepository.Class _Class;
        /// <summary>This property defines the metadata for the memory instance.</summary>
        /// <MetaDataID>{073509B7-BF95-4F7A-A926-DE273514BC6C}</MetaDataID>
        [Association("", Roles.RoleA, "{EF6B9578-0954-4695-813C-158BA8EFD956}")]
        [IgnoreErrorCheck]
        public DotNetMetaDataRepository.Class Class
        {
            get
            {
                if (_Class == null)
                {
                    if (MemoryInstance == null)
                        throw new System.Exception("the MemoryInstance must be not null to get Class");
                    if (LifeTimeController == null)
                        _Class = ((PersistenceLayerRunTime.ObjectStorage)ObjectStorage).OperativeObjectCollections[MemoryInstance.GetType()].Class;
                    else
                        _Class = LifeTimeController.Class;
                }
                return _Class;

            }
        }


        /// <summary>This method checks the role fields values of relation object
        /// There is a main rule for relation object. 
        /// The relation object defines a relationship between two objects. 
        /// If you want to remove this relationship and create a new 
        /// then you must delete the relation object and create new. 
        /// You cant change the values of roles fields of relation object. </summary>
        /// <MetaDataID>{E7D9DE31-78EE-420E-9EBF-23E7FF6D610E}</MetaDataID>
        void ValidateRelatedObjects()
        {
            //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
            if (Class.LinkAssociation != null)
            {

                System.Reflection.FieldInfo fieldRoleA = (Class as DotNetMetaDataRepository.Class).LinkClassRoleAField;
                System.Reflection.FieldInfo fieldRoleB = (Class as DotNetMetaDataRepository.Class).LinkClassRoleBField;
                if (PersistentObjectID != null)
                {
                    System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> fieldValues = null;

                    StorageInstanceValues.TryGetValue("", out fieldValues);
                    if (fieldValues.Count != 0)
                    {
                        object StorageInstanceFieldValue = fieldValues[fieldRoleA];
                        //if (StorageInstanceFieldValue != null && StorageInstanceFieldValue != FieldRoleA.GetValue(MemoryInstance))
                        //    throw new System.Exception("The object " + MemoryInstance.ToString() + " is in inconsistent state.\nYou can't change the role objects from a relation object.\nYou must delete this and create a new.");

                        if (StorageInstanceFieldValue != null && StorageInstanceFieldValue != Member<object>.GetValue((Class as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, MemoryInstance))
                            throw new System.Exception("The object " + MemoryInstance.ToString() + " is in inconsistent state.\nYou can't change the role objects from a relation object.\nYou must delete this and create a new.");

                        //Member<object>.GetValue(relatedObject, roleAField);

                        StorageInstanceFieldValue = fieldValues[fieldRoleB];

                        //if (StorageInstanceFieldValue != null && StorageInstanceFieldValue != FieldRoleB.GetValue(MemoryInstance))
                        //    throw new System.Exception("The object " + MemoryInstance.ToString() + " is in inconsistent state.\nYou can't change the role objects from a relation object.\nYou must delete this and create a new.");
                        if (StorageInstanceFieldValue != null && StorageInstanceFieldValue != Member<object>.GetValue((Class as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, MemoryInstance))
                            throw new System.Exception("The object " + MemoryInstance.ToString() + " is in inconsistent state.\nYou can't change the role objects from a relation object.\nYou must delete this and create a new.");
                    }
                }
            }
        }

        /// <summary>
        /// This method checks if the memory instance has deferent 
        /// state from storage instance. 
        /// If it has then get method return true else return false.
        /// </summary>
        /// <MetaDataID>{A78AC9FB-F06A-48B7-87E4-4FF0AA5F8969}</MetaDataID>
        public bool HasChangeState()
        {

            if (ReferentialIntegrityCountHasChanged)
                return true;

            foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeValues(MemoryInstance, Class))
            {
                object storageInstanceFieldValue = null;

                if (!StorageInstanceValues.ContainsKey(valueOfAttribute.PathIdentity) || !StorageInstanceValues[valueOfAttribute.PathIdentity].TryGetValue(valueOfAttribute.FieldInfo, out storageInstanceFieldValue))
                    return true;
                System.Reflection.FieldInfo CurrFieldInfo = valueOfAttribute.FieldInfo;

#if !DeviceDotNet
                if (CurrFieldInfo.FieldType == typeof(System.Xml.XmlDocument))
                {
                    System.Xml.XmlDocument XmlDocumentValue = valueOfAttribute.Value as System.Xml.XmlDocument;
                    if (XmlDocumentValue != null)
                    {
                        if (storageInstanceFieldValue != null)
                            if (XmlDocumentValue.InnerXml != ((System.Xml.XmlDocument)storageInstanceFieldValue).InnerXml)
                                return true;
                    }
                    continue;
                }
#endif

                if (CurrFieldInfo.FieldType == typeof(System.Xml.Linq.XDocument))
                {
                    System.Xml.Linq.XDocument XmlDocumentValue = valueOfAttribute.Value as System.Xml.Linq.XDocument;
                    if (XmlDocumentValue != null)
                    {
                        if (storageInstanceFieldValue != null)
                            if (XmlDocumentValue.ToString() != storageInstanceFieldValue.ToString())
                                return true;
                    }
                    continue;
                }

                if (storageInstanceFieldValue == null)
                    if (valueOfAttribute.Value != null)
                        return true;
                    else
                        continue;

                if (valueOfAttribute.IsMultilingual)
                {
                    if ((storageInstanceFieldValue is System.Collections.IDictionary && !(valueOfAttribute.Value is System.Collections.IDictionary)) ||
                        (!(storageInstanceFieldValue is System.Collections.IDictionary) && (valueOfAttribute.Value is System.Collections.IDictionary)))
                        return true;

                    if ((storageInstanceFieldValue as System.Collections.IDictionary).Count != (valueOfAttribute.Value as System.Collections.IDictionary).Keys.Count)
                        return true;

                    foreach (System.Collections.DictionaryEntry dictionaryEntry in storageInstanceFieldValue as System.Collections.IDictionary)
                    {
                        //the value for the culture (dictionaryEntry.Key) is the same for two multilingual dictionaries 
                        if (!(valueOfAttribute.Value as System.Collections.IDictionary).Contains(dictionaryEntry.Key))
                            return true;


                        object value = (valueOfAttribute.Value as System.Collections.IDictionary)[dictionaryEntry.Key];
                        if (value == null && dictionaryEntry.Value == null)
                            continue;

                        if (value == null || dictionaryEntry.Value == null)
                            return true;

                        if (!value.Equals(dictionaryEntry.Value))
                            return true;
                    }

                }
                else
                    if (!storageInstanceFieldValue.Equals(valueOfAttribute.Value))
                    return true;

            }


            return false;

        }


        /// <exclude>Excluded</exclude>
        protected OOAdvantech.MemberAcount<AccountNumber> _RuntimeReferentialIntegrityCount = new MemberAcount<AccountNumber>();



        /// <summary>
        /// This property defines a temporary referential integrity count.
        /// Its value updated in transaction context when relation added or removed.
        /// When transaction commited takes 
        /// 
        /// </summary>
        /// <MetaDataID>{0d0dd132-eda2-464e-b09e-cba061990c71}</MetaDataID>
        [TransactionalMember(nameof(_RuntimeReferentialIntegrityCount))]
        public int RuntimeReferentialIntegrityCount
        {
            get
            {
                return (int)_RuntimeReferentialIntegrityCount.Value.Amount;
            }
        }
        /// <MetaDataID>{533460f0-bc73-4fd7-bb98-5610628176a3}</MetaDataID>
        public int AdvanceRuntimeReferentialIntegrity()
        {
            int count = 0;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, "RuntimeReferentialIntegrityCount"))
            {
                _RuntimeReferentialIntegrityCount.Value++;
                count = (int)_RuntimeReferentialIntegrityCount.Value.Amount;
                stateTransition.Consistent = true;
            }
            return count;

        }
        /// <MetaDataID>{e1d4648f-d183-4c39-b666-0e1becc6c898}</MetaDataID>
        public int ReduceRuntimeReferentialIntegrity()
        {
            int count = 0;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, "RuntimeReferentialIntegrityCount"))
            {
                _RuntimeReferentialIntegrityCount.Value--;
                count = (int)_RuntimeReferentialIntegrityCount.Value.Amount;
                stateTransition.Consistent = true;
            }
            return count;
        }



        /// <MetaDataID>{25ECEEA9-78FD-42D0-988D-030C5849F8A8}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected int _ReferentialIntegrityCount = 0;
        ///<summary>
        ///Objects use a reference counting mechanism to ensure 
        ///that the lifetime of the object includes the lifetime of references to it.
        ///The referential integrity count used 
        ///from persistent reference which implement association end 
        ///with referential integrity flag set.
        ///</summary>
        /// <MetaDataID>{BDFA357A-0239-4038-91C2-C1F3D8CE48CD}</MetaDataID>
        public int ReferentialIntegrityCount
        {
            get
            {
                return _ReferentialIntegrityCount;
            }
        }


        ///<summary>
        ///The ReferentialIntegrityLinkAdded method increments the referential integrity count 
        ///for a reference on an object. It should be called for every link to the object, 
        ///which the type is an <see cref="OOAdvantech.MetaDataRepository.Association"> association</see> where the 
        ///<see cref="OOAdvantech.MetaDataRepository.AssociationEnd">association end</see> has 
        ///<see cref="OOAdvantech.MetaDataRepository.PersistencyFlag.ReferentialIntegrity">referential flag integrity</see>
        ///set. 
        ///</summary>
        /// <MetaDataID>{B36200A3-382E-485D-BF15-A3C0567B32E8}</MetaDataID>
        public long ReferentialIntegrityLinkAdded()
        {




            //TODO σε περίπτωση που η class έχει ClassHierarchyLinkAssociation 
            //θα πρέπει να ενημερωθούν και τα role A και role B objects.
            //Να γραφτεί Test Case  
            //Να εξεταστεί που κάποια απο τα role objects είναι σε διαφορετικό process ή κόμβο

            if (ObjectStateManagerLink == null || !Transactions.ObjectStateTransition.IsLocked(ObjectStateManagerLink))
            {
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {
                    _ReferentialIntegrityCount++;
                    StateTransition.Consistent = true; ;
                }
            }
            else
            {
                _ReferentialIntegrityCount++;
            }

            if (Class.ClassHierarchyLinkAssociation != null && _ReferentialIntegrityCount == 1)
            {
                StorageInstanceAgent roleA = null, roleB = null;
                GetRolesObject(ref roleA, ref roleB);
                roleA.RealStorageInstanceRef.ReferentialIntegrityLinkAdded();
                roleB.RealStorageInstanceRef.ReferentialIntegrityLinkAdded();
            }

            if (!TransactionContext.CurrentTransactionContext.ContainCommand(Commands.UpdateStorageInstanceCommand.GetIdentity(this)) &&
                !TransactionContext.CurrentTransactionContext.ContainCommand(Commands.UpdateReferentialIntegrity.GetIdentity(this)))
            {
                (ObjectStorage as ObjectStorage).CreateUpdateReferentialIntegrity(this);
            }




            //			if(Class.ClassHierarchyLinkAssociation!=null)
            //			{
            //				object roleAObject=Class.LinkClassRoleAField.GetValue(MemoryInstance);
            //				StorageInstanceRef roleAStorageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(roleAObject) as StorageInstanceRef;
            //				roleAStorageInstanceRef.ReferentialIntegrityLinkAdded();
            //
            //				object roleBObject=Class.LinkClassRoleBField.GetValue(MemoryInstance);
            //				StorageInstanceRef roleBStorageInstanceRef=StorageInstanceRef.GetStorageInstanceRef(roleBObject) as StorageInstanceRef;
            //				roleBStorageInstanceRef.ReferentialIntegrityLinkAdded();
            //			}

            return _ReferentialIntegrityCount;
        }

        internal protected void GetRolesObject(ref StorageInstanceAgent roleA, ref StorageInstanceAgent roleB)
        {
            if (Class.ClassHierarchyLinkAssociation != null)
            {
                roleA = StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(Class.LinkClassRoleAFastFieldAccessor.GetValue, MemoryInstance));
                roleB = StorageInstanceRef.GetStorageInstanceAgent(Member<object>.GetValue(Class.LinkClassRoleBFastFieldAccessor.GetValue, MemoryInstance));
            }
        }


        /// <MetaDataID>{3BB1228B-5D69-4B19-9F56-E4907CA0DEC8}</MetaDataID>
        public long ReferentialIntegrityLinkRemoved()
        {

            //TODO σε περίπτωση που η class έχει ClassHierarchyLinkAssociation 
            //θα πρέπει να ενημερωθούν και τα role A και role B objects.
            //Να γραφτεί Test Case

            if (ObjectStateManagerLink == null || !Transactions.ObjectStateTransition.IsLocked(ObjectStateManagerLink))
            {

                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
                {

                    _ReferentialIntegrityCount--;
                    StateTransition.Consistent = true; ;
                }
            }
            else
                _ReferentialIntegrityCount--;



            if (Class.LinkAssociation != null && _ReferentialIntegrityCount == 0)
            {
                StorageInstanceAgent roleA = null, roleB = null;
                GetRolesObject(ref roleA, ref roleB);
                //roleA.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
                //roleB.RealStorageInstanceRef.ReferentialIntegrityLinkRemoved();
            }


            if (!TransactionContext.CurrentTransactionContext.ContainCommand(Commands.UpdateStorageInstanceCommand.GetIdentity(this)) &&
            !TransactionContext.CurrentTransactionContext.ContainCommand(Commands.UpdateReferentialIntegrity.GetIdentity(this)))
            {
                (ObjectStorage as ObjectStorage).CreateUpdateReferentialIntegrity(this);
            }

            return _ReferentialIntegrityCount;
        }


        /// <MetaDataID>{5D89D349-5CC0-481D-A65A-0EFD03466E6F}</MetaDataID>
        // protected System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> StorageInstanceFieldValues = new System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object>();

        /// <MetaDataID>{79354597-96ef-4c10-83bc-a18af85e4295}</MetaDataID>
        protected System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object>> StorageInstanceValues = new Dictionary<string, Dictionary<System.Reflection.FieldInfo, object>>();
        /// <MetaDataID>{1109F469-4AD7-4FC8-8711-2765A296C3E6}</MetaDataID>
        protected System.Collections.Generic.Dictionary<string, object> StorageInstanceRefSnapshot = new Dictionary<string, object>();

        /// <MetaDataID>{87C2E546-E13E-4FF9-B01E-D3A014C7192B}</MetaDataID>
        public bool ReferentialIntegrityCountHasChanged
        {
            get
            {
                return _ReferentialIntegrityCount != StorageInstanceReferentialIntegrityCount;
            }
        }
        /// <MetaDataID>{3f2d3149-1d92-4136-90d9-8eb9c14a2cef}</MetaDataID>
        bool _IsObjectActive;

        public bool  IsObjectActive=>_IsObjectActive;

        public void MarkAsReloadedObject()
        {
            _IsObjectActive = false;
        }
            

        /// <MetaDataID>{e49ebda2-836c-4a3b-9af5-5d9bd553fc59}</MetaDataID>
        public override void ObjectActived()
        {

            if (this.ObjectID.ToString() == "3607914b-85b4-4d0a-b751-18cb41628210")
            {

            }



            if (!_IsObjectActive)
            {

                CreateMarshalByRefUri();
                //                string persistentUri = null;
                //                if (ObjectID != null && MemoryInstance is MarshalByRefObject)
                //                    persistentUri = (_ObjectStorage as ObjectStorage).GetPersistentObjectUri(this);

                //#if !DeviceDotNet
                //                if (MemoryInstance is System.MarshalByRefObject && !string.IsNullOrEmpty(persistentUri))
                //                {
                //                    byte[] data = new byte[0x12];
                //                    rng.GetBytes(data);
                //                    string str = System.Convert.ToBase64String(data);
                //                    str = str.Replace('/', '_') + "_" + Remoting_Identity_GetNextSeqNum() + "#PID#" + persistentUri;
                //                    System.Runtime.Remoting.RemotingServices.Marshal(MemoryInstance as System.MarshalByRefObject, str);
                //                }
                //#endif


                OOAdvantech.MetaDataRepository.Operation operation = this.Class.ObjectActivation;
                if (operation != null)
                {
                    System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                    OOAdvantech.AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(MemoryInstance, new object[0]);
                }
                else if (MemoryInstance is OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer)
                {
                    (MemoryInstance as OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer).OnActivate();
                }
                else if (ObjectStateManagerLink != null)
                    (ObjectStateManagerLink as PersistenceLayer.IObjectStateEventsConsumer).OnActivate();


                _IsObjectActive = true;
            }
        }

        public override void ObjectDeleting()
        {
            OOAdvantech.MetaDataRepository.Operation operation = this.Class.DeleteObject;
            if (operation != null)
            {
                System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                OOAdvantech.AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(MemoryInstance, new object[0]);
            }
            else if (MemoryInstance is OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer)
            {
                (MemoryInstance as OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer).OnDeleting();
            }
            else if (ObjectStateManagerLink != null)
                (ObjectStateManagerLink as PersistenceLayer.IObjectStateEventsConsumer).OnDeleting();

        }


        /// <MetaDataID>{2ba1823d-841b-46fe-9a16-daf640e05ab4}</MetaDataID>
        public void ObjectStateCommit()
        {
            OOAdvantech.MetaDataRepository.Operation operation = this.Class.CommitObjectStateInStorage;
            if (operation != null)
            {
                System.Reflection.MethodInfo methodInfo = operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
                OOAdvantech.AccessorBuilder.GetMethodInvoker(methodInfo).Invoke(MemoryInstance, new object[0]);
            }
            else if (MemoryInstance is OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer)
            {
                (MemoryInstance as OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer).OnCommitObjectState();

            }
            else if (ObjectStateManagerLink != null)
                (ObjectStateManagerLink as PersistenceLayer.IObjectStateEventsConsumer).OnCommitObjectState();


        }

        #region Manipulate the attributes values and attributes metadata

        /// <MetaDataID>{8c353d2b-4ad8-4dc8-93fc-d10875ec1683}</MetaDataID>
        static System.Collections.Generic.Dictionary<MetaDataRepository.Classifier, System.Collections.Generic.List<ValueOfAttribute>> PersistentAttributeMetaData = new Dictionary<MetaDataRepository.Classifier, List<ValueOfAttribute>>();


        ///<summary>
        ///This method returns the values of persistent attributes.
        ///The values are valid for current transaction 
        ///</summary>
        /// <MetaDataID>{5d366054-d4d5-40d8-91ac-dadf1656f780}</MetaDataID>
        public List<ValueOfAttribute> GetPersistentAttributeValues()
        {


            Transactions.LockType memoryInstanceTransactionLockType = Transactions.ObjectStateTransition.GetTransactionLockType(MemoryInstance);
            List<System.Reflection.FieldInfo> partialObjectLockFields = null;
            if (memoryInstanceTransactionLockType == OOAdvantech.Transactions.LockType.PartialObjectLock ||
                memoryInstanceTransactionLockType == OOAdvantech.Transactions.LockType.SharedFieldsParticipation)
            {
                System.Collections.Generic.List<ValueOfAttribute> persistentAttributeValues = GetPersistentAttributeValues(MemoryInstance, Class);

                partialObjectLockFields = Transactions.ObjectStateTransition.GetPartialObjectLockFields(MemoryInstance);
                Dictionary<string, Dictionary<FieldInfo, object>> transactionValues = new Dictionary<string, Dictionary<FieldInfo, object>>();
                foreach (ValueOfAttribute valueOfAttribute in persistentAttributeValues)
                {
                    Dictionary<FieldInfo, object> fieldValues = null;
                    if (!transactionValues.TryGetValue(valueOfAttribute.PathIdentity, out fieldValues))
                    {
                        fieldValues = new Dictionary<FieldInfo, object>();
                        transactionValues[valueOfAttribute.PathIdentity] = fieldValues;
                    }
                    fieldValues[valueOfAttribute.FieldInfo] = valueOfAttribute.Value;
                }
                persistentAttributeValues.Clear();
                foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeMetaData())
                {
                    Dictionary<FieldInfo, object> fieldValues = null;
                    object value = null;
                    if (transactionValues.TryGetValue(valueOfAttribute.PathIdentity, out fieldValues))
                        if (fieldValues.TryGetValue(valueOfAttribute.FieldInfo, out value))
                        {
                            persistentAttributeValues.Add(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, valueOfAttribute.Culture));
                            continue;
                        }

                    if (StorageInstanceValues.TryGetValue(valueOfAttribute.PathIdentity, out fieldValues))
                        fieldValues.TryGetValue(valueOfAttribute.FieldInfo, out value);
                    persistentAttributeValues.Add(new ValueOfAttribute(valueOfAttribute.Attribute, valueOfAttribute.IsMultilingual, valueOfAttribute.FieldInfo, valueOfAttribute.FastFieldAccessor, value, valueOfAttribute.ValueTypePath, valueOfAttribute.Path, valueOfAttribute.Culture));
                }
                return persistentAttributeValues;


            }
            else
                return GetPersistentAttributeValues(MemoryInstance, Class);
        }

        /// <MetaDataID>{bac41287-c46a-44fc-a2c8-265969459345}</MetaDataID>
        public System.Collections.Generic.List<ValueOfAttribute> GetPersistentAttributeMetaData()
        {
            System.Collections.Generic.List<ValueOfAttribute> attributesMetaData = null;
            if (!PersistentAttributeMetaData.TryGetValue(Class, out attributesMetaData))
            {
                attributesMetaData = GetPersistentAttributeMetaData(Class);
                PersistentAttributeMetaData[Class] = attributesMetaData;
            }
            return attributesMetaData;
        }

        /// <MetaDataID>{8870448d-d240-43b8-ba93-97811465ae75}</MetaDataID>
        public static System.Collections.Generic.List<ValueOfAttribute> GetPersistentAttributeMetaData(DotNetMetaDataRepository.Class _class)
        {

            System.Collections.Generic.List<ValueOfAttribute> attributesMetaData = null;
            if (!PersistentAttributeMetaData.TryGetValue(_class, out attributesMetaData))
            {
                attributesMetaData = GetPersistentAttributeValues(null, _class);
                PersistentAttributeMetaData[_class] = attributesMetaData;
            }
            return attributesMetaData;
        }

        /// <MetaDataID>{1e9f1384-2420-4591-aab8-ddd4f2ee2166}</MetaDataID>
        public static System.Collections.Generic.List<ValueOfAttribute> GetPersistentAttributeMetaData(DotNetMetaDataRepository.Structure _struct, MetaDataRepository.ValueTypePath pathIdentity, string path)
        {

            System.Collections.Generic.List<ValueOfAttribute> attributesMetaData = null;
            if (!PersistentAttributeMetaData.TryGetValue(_struct, out attributesMetaData))
            {
                attributesMetaData = GetPersistentAttributeValues(null, _struct, pathIdentity, path, false, null);
                PersistentAttributeMetaData[_struct] = attributesMetaData;
            }
            return attributesMetaData;

        }

        /// <summary>
        /// This method provides a collection with ValueOfAttribute for the memoryInstance.
        /// In case where the memoryInstance participate partially in current transaction 
        /// method returns only ValueOfAttributes only for attribute wich are locked from transaction.  
        /// The memoryInstance must be instance of class which defined from parameter _class.
        /// </summary>
        /// <param name="memoryInstance">
        /// Defines the memoryInstance for which you want attribute values.
        /// If the value is null returns ValueOfAttribute object for all attribute of class with null in member value   
        /// </param>
        /// <param name="_class">
        /// Defines the class for ValueOfAttribute collection
        /// </param>
        /// <MetaDataID>{3a6c1ad7-731b-4f0a-aedf-8a76db8fdc7d}</MetaDataID>
        static System.Collections.Generic.List<ValueOfAttribute> GetPersistentAttributeValues(object memoryInstance, DotNetMetaDataRepository.Class _class)
        {

            List<System.Reflection.FieldInfo> partialObjectLockFields = null;
            if (memoryInstance != null)
            {
                Transactions.LockType memoryInstanceTransactionLockType = Transactions.ObjectStateTransition.GetTransactionLockType(memoryInstance);
                if (memoryInstanceTransactionLockType == OOAdvantech.Transactions.LockType.PartialObjectLock ||
                    memoryInstanceTransactionLockType == OOAdvantech.Transactions.LockType.SharedFieldsParticipation)
                    partialObjectLockFields = Transactions.ObjectStateTransition.GetPartialObjectLockFields(memoryInstance);
            }

            System.Collections.Generic.List<ValueOfAttribute> attributeValues = new System.Collections.Generic.List<ValueOfAttribute>();
            MetaDataRepository.ValueTypePath valueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
            foreach (DotNetMetaDataRepository.Attribute attribute in _class.GetAttributes(true))
            {
                //TODO να τσεκακριστεί ως προς τις datetime values 
                if (!_class.IsPersistent(attribute))// || (attribute.Type is MetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;
                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = _class.GetFastFieldAccessor(attribute);

                if (partialObjectLockFields != null && fastFieldAccessor.MemberInfo is FieldInfo && !partialObjectLockFields.Contains(fastFieldAccessor.MemberInfo as FieldInfo))
                    continue;

                bool isMultilingual = _class.IsMultilingual(attribute);
                if(!isMultilingual && attribute?.Identity?.ToString() == "{794bbf34-5df9-4ab0-9572-5773309ecc4c}.22")
                {

                }
                valueTypePath.Push(attribute.Identity);
                if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {
                    //System.Reflection.FieldInfo fieldInfo = _class.GetFieldMember(attribute);
                    object value = null;
                    if (memoryInstance != null)
                    {
                        //value = fieldInfo.GetValue(memoryInstance);
                        if (isMultilingual)
                            value = MultilingualMember<object>.GetValue(_class.GetFastFieldAccessor(attribute).GetValue, memoryInstance);
                        else
                            value = Member<object>.GetValue(_class.GetFastFieldAccessor(attribute).GetValue, memoryInstance);
                    }
                    //attributeValues.Add(fieldInfo, value);
                    if (isMultilingual)
                    {
                        if (value == null)
                            attributeValues.AddRange(GetPersistentAttributeValues(value, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, attribute.Name, true, null));
                        else
                        {
                            if ((value as System.Collections.IDictionary).Count > 0)
                                attributeValues.AddRange(GetPersistentAttributeMultilingualValues(valueTypePath, attribute, value as System.Collections.IDictionary));
                            else
                                attributeValues.AddRange(GetPersistentAttributeValues(null, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, attribute.Name, true, null));
                        }
                    }
                    else
                        attributeValues.AddRange(GetPersistentAttributeValues(value, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, attribute.Name, false, null));


                    //foreach (System.Collections.Generic.KeyValuePair<System.Reflection.FieldInfo, object> entry in GetPersistentAttributeValues(value, attribute.Type as DotNetMetaDataRepository.Structure))
                    //    attributeValues.Add(entry.Key, entry.Value);
                }
                else
                {
                    //System.Reflection.FieldInfo fieldInfo = _class.GetFieldMember(attribute);
                    fastFieldAccessor = _class.GetFastFieldAccessor(attribute);
                    object value = null;
                    if (memoryInstance != null)
                    {
                        //value = fieldInfo.GetValue(memoryInstance);
                        if (isMultilingual)
                            value = MultilingualMember<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                        else
                            value = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                    }
                    if (isMultilingual)
                    {
                        if (value == null)
                            attributeValues.Add(new ValueOfAttribute(attribute, isMultilingual, fastFieldAccessor.MemberInfo as System.Reflection.FieldInfo, fastFieldAccessor, value, new MetaDataRepository.ValueTypePath(), "", null));
                        else
                        {
                            attributeValues.Add(new ValueOfAttribute(attribute, isMultilingual, fastFieldAccessor.MemberInfo as System.Reflection.FieldInfo, fastFieldAccessor, value, new MetaDataRepository.ValueTypePath(), "", null));

                            //foreach (System.Collections.DictionaryEntry entry in (value as System.Collections.IDictionary))
                            //{
                            //    System.Globalization.CultureInfo culture = entry.Key as System.Globalization.CultureInfo;
                            //    using (CultureContext cultureContext = new CultureContext(culture, false))
                            //    {
                            //        attributeValues.Add(new ValueOfAttribute(attribute, isMultilingual, fastFieldAccessor.MemberInfo as System.Reflection.FieldInfo, fastFieldAccessor, entry.Value, new MetaDataRepository.ValueTypePath(), "", culture));
                            //    }
                            //}
                        }
                        //attributeValues.Add(new ValueOfAttribute(attribute, isMultilingual, fastFieldAccessor.MemberInfo as System.Reflection.FieldInfo, fastFieldAccessor, value, new MetaDataRepository.ValueTypePath(), ""));
                    }
                    else
                        attributeValues.Add(new ValueOfAttribute(attribute, isMultilingual, fastFieldAccessor.MemberInfo as System.Reflection.FieldInfo, fastFieldAccessor, value, new MetaDataRepository.ValueTypePath(), "", null));
                }
                valueTypePath.Pop();
            }
            return attributeValues;
        }

        private static List<ValueOfAttribute> GetPersistentAttributeMultilingualValues(ValueTypePath valueTypePath, DotNetMetaDataRepository.Attribute attribute, System.Collections.IDictionary multiligualValue)
        {
            List<ValueOfAttribute> attributeValues = new List<ValueOfAttribute>();
            List<ValueOfAttribute> multilingualAttributeValues = new List<ValueOfAttribute>();
            foreach (System.Collections.DictionaryEntry entry in (multiligualValue as System.Collections.IDictionary))
            {
                System.Globalization.CultureInfo culture = entry.Key as System.Globalization.CultureInfo;
                using (CultureContext cultureContext = new CultureContext(culture, false))
                {

                    multilingualAttributeValues.AddRange(GetPersistentAttributeValues(entry.Value, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, attribute.Name, true, culture));
                }
            }
            foreach (var multilingualAttributeValuesGroup in (from valueOfAttribute in multilingualAttributeValues
                                                              group valueOfAttribute by valueOfAttribute.FieldInfo into multilingualAttributeValue
                                                              select multilingualAttributeValue))
            {
                ValueOfAttribute? multilingualvalueOfAttribute = null; ;
                foreach (var valueOfAttribute in multilingualAttributeValuesGroup)
                {
                    if (multilingualvalueOfAttribute == null)
                    {
                        multilingualvalueOfAttribute = new ValueOfAttribute(valueOfAttribute.Attribute,
                                                                    valueOfAttribute.IsMultilingual,
                                                                    valueOfAttribute.FieldInfo,
                                                                    valueOfAttribute.FastFieldAccessor,
                                                                    new Dictionary<System.Globalization.CultureInfo, object>(),
                                                                    valueOfAttribute.ValueTypePath,
                                                                    valueOfAttribute.Path, null);
                    }
                    ((Dictionary<System.Globalization.CultureInfo, object>)multilingualvalueOfAttribute.Value.Value)[valueOfAttribute.Culture] = valueOfAttribute.Value;

                }
                attributeValues.Add(multilingualvalueOfAttribute.Value);
            }

            return attributeValues;
        }


        /// <summary>
        /// This method provides a collection with ValueOfAttribute for the persistent members of memoryInstance.
        /// The memoryInstance must be instance of Structure which defined from parameter structure.
        /// </summary>
        /// <param name="memoryInstance">
        /// Defines the memoryInstance for which you want attribute values.
        /// If the value is null returns ValueOfAttribute object for all persistent attribute of structure with null in member value   
        /// </param>
        /// <param name="structure">
        /// Defines the structure for ValueOfAttribute collection
        /// </param>
        /// <MetaDataID>{9e73ee8a-60d1-4a1c-bcaf-a29bf782c4ef}</MetaDataID>
        static System.Collections.Generic.List<ValueOfAttribute> GetPersistentAttributeValues(object memoryInstance, DotNetMetaDataRepository.Structure structure, MetaDataRepository.ValueTypePath pathIdentity, string path, bool multilingual, System.Globalization.CultureInfo culture)
        {
            //if (!string.IsNullOrEmpty(pathIdentity))
            //    pathIdentity += "_";

            System.Collections.Generic.List<ValueOfAttribute> attributeValues = new System.Collections.Generic.List<ValueOfAttribute>();
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {


                //TODO να τσεκακριστεί ως προς τις datetime values
                if (!structure.IsPersistent(attribute))// || (attribute.Type is MetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;
                pathIdentity.Push(attribute.Identity);

                bool isMultilingual = structure.IsMultilingual(attribute); //||multilingual;
                if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {
                    AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = structure.GetFastFieldAccessor(attribute);

                    object value = null;
                    if (memoryInstance != null)
                    {
                        //value = fieldInfo.GetValue(memoryInstance);
                        if (isMultilingual)
                            value = MultilingualMember<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                        else
                            value = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                    }
                    //attributeValues.Add(fieldInfo, value);
                    if (isMultilingual)
                    {
                        if (value == null)
                            attributeValues.AddRange(GetPersistentAttributeValues(value, attribute.Type as DotNetMetaDataRepository.Structure, pathIdentity, path + "_" + attribute.Name, true, culture));
                        else
                        {
                            List<ValueOfAttribute> multilingualAttributeValues = new List<ValueOfAttribute>();
                            foreach (System.Collections.DictionaryEntry entry in (value as System.Collections.IDictionary))
                            {
                                culture = entry.Key as System.Globalization.CultureInfo;
                                using (CultureContext cultureContext = new CultureContext(culture, false))
                                {
                                    multilingualAttributeValues.AddRange(GetPersistentAttributeValues(entry.Value, attribute.Type as DotNetMetaDataRepository.Structure, pathIdentity, path + "_" + attribute.Name, true, culture));
                                }
                            }
                        }
                    }
                    else
                        attributeValues.AddRange(GetPersistentAttributeValues(value, attribute.Type as DotNetMetaDataRepository.Structure, pathIdentity, path + "_" + attribute.Name, multilingual, culture));
                }
                else
                {
                    AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = structure.GetFastFieldAccessor(attribute);

                    object value = null;
                    if (memoryInstance != null)
                    {
                        if (isMultilingual)
                            value = MultilingualMember<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                        else
                            value = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                    }

                    if (isMultilingual)
                    {
                        if (value == null)
                            attributeValues.Add(new ValueOfAttribute(attribute, isMultilingual | multilingual, fastFieldAccessor.MemberInfo as System.Reflection.FieldInfo, fastFieldAccessor, value, pathIdentity, path, culture));
                        else
                            attributeValues.AddRange(GetPersistentAttributeMultilingualValues(pathIdentity, attribute, value as System.Collections.IDictionary));

                        //attributeValues.Add(new ValueOfAttribute(attribute, isMultilingual, fastFieldAccessor.MemberInfo as System.Reflection.FieldInfo, fastFieldAccessor, value, pathIdentity, path));
                    }
                    else
                        attributeValues.Add(new ValueOfAttribute(attribute, isMultilingual | multilingual, fastFieldAccessor.MemberInfo as System.Reflection.FieldInfo, fastFieldAccessor, value, pathIdentity, path, culture));

                }
                pathIdentity.Pop();
            }
            return attributeValues;
        }
        /// <MetaDataID>{d7050a78-246c-4a7f-8975-00777c6448d1}</MetaDataID>
        protected void SetAttributeValue(ValueOfAttribute valueOfAttribute)
        {
            try
            {

                object memoryInstance = MemoryInstance;
                System.Type type = MemoryInstance.GetType();
                if (valueOfAttribute.FieldInfo.DeclaringType == type || type.GetMetaData().IsSubclassOf(valueOfAttribute.FieldInfo.DeclaringType)) //Class.IsA(valueOfAttribute.Attribute.Owner))
                {
                    //valueOfAttribute.FieldInfo.SetValue(MemoryInstance, valueOfAttribute.Value);
                    if (valueOfAttribute.FieldInfo.FieldType.GetMetaData().IsValueType && valueOfAttribute.Value == null)
                        Member<object>.SetValueImplicitly(valueOfAttribute.FastFieldAccessor, ref memoryInstance, AccessorBuilder.GetDefaultValue(valueOfAttribute.FieldInfo.FieldType));
                    else
                        Member<object>.SetValueImplicitly(valueOfAttribute.FastFieldAccessor, ref memoryInstance, valueOfAttribute.Value);

                }
                else
                {
                    foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                    {
                        if (!Class.IsPersistent(attribute))// || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                            continue;
                        if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent && valueOfAttribute.ValueTypePath.ToArray()[valueOfAttribute.ValueTypePath.Count - 1] == attribute.Identity)
                        {
                            bool valueSetted = false;

                            //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                            //object owner = fieldInfo.GetValue(MemoryInstance);

                            bool multilingual = Class.IsMultilingual(attribute);
                            if (multilingual)
                            {
                                //var
                                using (OOAdvantech.CultureContext cultureContext = new CultureContext(valueOfAttribute.Culture, false))
                                {
                                    object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                    if (owner == null && fastFieldAccessor.IsNullable && fastFieldAccessor.MemberInfo.IsField())
                                    {
                                        object memberOwner = MemoryInstance;
                                        Member<object>.SetValueImplicitly(fastFieldAccessor, ref memberOwner, AccessorBuilder.GetDefaultValue((fastFieldAccessor.MemberInfo as FieldInfo).FieldType.GetMetaData().GetGenericArguments()[0]));
                                        owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                    }
                                    if (owner==null)
                                        owner=OOAdvantech.AccessorBuilder.GetDefaultValue(attribute.Type.GetExtensionMetaObject<System.Type>());
                                    SetAttributeValue(ref owner, valueOfAttribute, attribute.Type as DotNetMetaDataRepository.Structure, 1, out valueSetted);
                                    if (valueSetted)
                                    {
                                        Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                        //fieldInfo.SetValue(MemoryInstance, owner);
                                        return;
                                    }
                                }
                            }
                            else
                            {

                                object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                if (owner == null && fastFieldAccessor.IsNullable && fastFieldAccessor.MemberInfo.IsField())
                                {
                                    object memberOwner = MemoryInstance;
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memberOwner, AccessorBuilder.GetDefaultValue((fastFieldAccessor.MemberInfo as FieldInfo).FieldType.GetMetaData().GetGenericArguments()[0]));
                                    owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                }

                                SetAttributeValue(ref owner, valueOfAttribute, attribute.Type as DotNetMetaDataRepository.Structure, 1, out valueSetted);
                                if (valueSetted)
                                {
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                    //fieldInfo.SetValue(MemoryInstance, owner);
                                    return;
                                }
                            }
                        }
                    }
                    throw new System.Exception("System can't set value of '" + valueOfAttribute.Attribute.Name + "'");
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        protected object GetAttributeValue(ValueOfAttribute valueOfAttribute)
        {
            try
            {

                object memoryInstance = MemoryInstance;
                System.Type type = MemoryInstance.GetType();
                if (valueOfAttribute.FieldInfo.DeclaringType == type || type.GetMetaData().IsSubclassOf(valueOfAttribute.FieldInfo.DeclaringType)) //Class.IsA(valueOfAttribute.Attribute.Owner))
                {
                    if (valueOfAttribute.IsMultilingual)
                        return MultilingualMember<object>.GetValue(valueOfAttribute.FastFieldAccessor.GetValue, memoryInstance);
                    else
                        return Member<object>.GetValue(valueOfAttribute.FastFieldAccessor.GetValue, memoryInstance);
                }
                else
                {
                    foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                    {
                        if (!Class.IsPersistent(attribute))// || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                            continue;
                        if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent && valueOfAttribute.ValueTypePath.ToArray()[valueOfAttribute.ValueTypePath.Count - 1] == attribute.Identity)
                        {
                            if (valueOfAttribute.IsMultilingual)
                            {
                                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);

                                System.Collections.IDictionary multilingualOwner = MultilingualMember<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance) as System.Collections.IDictionary;

                                System.Collections.Hashtable multilingualValues = new System.Collections.Hashtable();
                                foreach (System.Collections.DictionaryEntry ownerEntry in multilingualOwner)
                                {
                                    object owner = ownerEntry.Value;
                                    System.Globalization.CultureInfo culture = ownerEntry.Key as System.Globalization.CultureInfo;

                                    object value = GetAttributeValue(owner, valueOfAttribute, attribute.Type as DotNetMetaDataRepository.Structure, 1);
                                    if (value is System.Collections.Hashtable && (value as System.Collections.Hashtable).Count > 0 && (value as System.Collections.Hashtable).Keys.OfType<System.Globalization.CultureInfo>().Count() > 0)
                                    {
                                        foreach (var valueCulture in (value as System.Collections.Hashtable).Keys.OfType<System.Globalization.CultureInfo>())
                                            multilingualValues[valueCulture] = (value as System.Collections.Hashtable)[valueCulture];
                                    }
                                    else
                                        multilingualValues[culture] = value;

                                }
                                return multilingualValues;

                            }
                            else
                            {
                                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                if (owner == null && fastFieldAccessor.IsNullable && fastFieldAccessor.MemberInfo.IsField())
                                {
                                    object memberOwner = MemoryInstance;
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memberOwner, AccessorBuilder.GetDefaultValue((fastFieldAccessor.MemberInfo as FieldInfo).FieldType.GetMetaData().GetGenericArguments()[0]));
                                    owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                }

                                return GetAttributeValue(owner, valueOfAttribute, attribute.Type as DotNetMetaDataRepository.Structure, 1);
                            }
                        }
                    }
                    throw new System.Exception("System can't set value of '" + valueOfAttribute.Attribute.Name + "'");
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{9c964e68-415f-4865-b08e-136463ff76c3}</MetaDataID>
        protected static void SetAttributeValue(ref object memoryInstance, ValueOfAttribute valueOfAttribute, DotNetMetaDataRepository.Structure structure, int pathIndex, out bool valueSetted)
        {

            if (structure.IsA(valueOfAttribute.Attribute.Owner))
            {
                //valueOfAttribute.FieldInfo.SetValue(memoryInstance, valueOfAttribute.Value);
                if (valueOfAttribute.FieldInfo.FieldType.GetMetaData().IsValueType && valueOfAttribute.Value == null)
                    Member<object>.SetValueImplicitly(valueOfAttribute.FastFieldAccessor, ref memoryInstance, AccessorBuilder.GetDefaultValue(valueOfAttribute.FieldInfo.FieldType));
                else
                    Member<object>.SetValueImplicitly(valueOfAttribute.FastFieldAccessor, ref memoryInstance, valueOfAttribute.Value);

                valueSetted = true;
                return;
            }
            else
            {
                foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
                {
                    if (!structure.IsPersistent(attribute))// || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                        continue;
                    if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent && valueOfAttribute.ValueTypePath.ToArray()[valueOfAttribute.ValueTypePath.Count - pathIndex - 1] == attribute.Identity)
                    {

                        //System.Reflection.FieldInfo fieldInfo = structure.GetFieldMember(attribute);
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = structure.GetFastFieldAccessor(attribute);
                        //object owner = fieldInfo.GetValue(memoryInstance);
                        object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                        SetAttributeValue(ref owner, valueOfAttribute, attribute.Type as DotNetMetaDataRepository.Structure, pathIndex + 1, out valueSetted);
                        if (valueSetted)
                        {
                            //fieldInfo.SetValue(memoryInstance, owner);
                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                            return;
                        }
                    }
                }

            }
            throw new System.Exception("System can't set value of '" + valueOfAttribute.Attribute.Name + "'");
        }



        protected static object GetAttributeValue(object memoryInstance, ValueOfAttribute valueOfAttribute, DotNetMetaDataRepository.Structure structure, int pathIndex)
        {

            if (structure.IsA(valueOfAttribute.Attribute.Owner))
            {
                if (structure.IsMultilingual(valueOfAttribute.Attribute))
                    return MultilingualMember<object>.GetValue(valueOfAttribute.FastFieldAccessor.GetValue, memoryInstance);
                else
                    return Member<object>.GetValue(valueOfAttribute.FastFieldAccessor.GetValue, memoryInstance);
            }
            else
            {
                foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
                {
                    if (!structure.IsPersistent(attribute))// || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                        continue;
                    if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent && valueOfAttribute.ValueTypePath.ToArray()[valueOfAttribute.ValueTypePath.Count - pathIndex - 1] == attribute.Identity)
                    {

                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = structure.GetFastFieldAccessor(attribute);
                        object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                        return GetAttributeValue(owner, valueOfAttribute, attribute.Type as DotNetMetaDataRepository.Structure, pathIndex + 1);

                    }
                }

            }
            throw new System.Exception("System can't set value of '" + valueOfAttribute.Attribute.Name + "'");
        }


        #endregion



        /// <MetaDataID>{31f5c6da-6bd5-416b-9dfb-50911d352d71}</MetaDataID>
        protected void SnapshotStorageInstanceValue(string pathIdentity, FieldInfo fieldInfo, object value)
        {
            System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> fieldValues = null;

            if (!StorageInstanceValues.TryGetValue(pathIdentity, out fieldValues))
            {
                fieldValues = new Dictionary<System.Reflection.FieldInfo, object>();
                StorageInstanceValues.Add(pathIdentity, fieldValues);
            }
            else
                fieldValues = StorageInstanceValues[pathIdentity];
#if DeviceDotNet
            if (fieldInfo.FieldType == typeof(System.Xml.Linq.XDocument) && value != null)
                fieldValues[fieldInfo] = System.Xml.Linq.XDocument.Parse(((System.Xml.Linq.XDocument)value).ToString());
            else
                fieldValues[fieldInfo] = value;
#else
            if (fieldInfo.FieldType == typeof(System.Xml.XmlDocument) && value != null)
                fieldValues[fieldInfo] = (System.Xml.XmlDocument)((System.Xml.XmlDocument)value).Clone();
            else if (fieldInfo.FieldType == typeof(System.Xml.Linq.XDocument) && value != null)
                fieldValues[fieldInfo] = System.Xml.Linq.XDocument.Parse(((System.Xml.Linq.XDocument)value).ToString());
            else
                fieldValues[fieldInfo] = value;
#endif
        }
        /// <MetaDataID>{BFE47095-FB46-4B25-9F4A-7C9D32794B6E}</MetaDataID>
        private int StorageInstanceReferentialIntegrityCount = 0;
        /// <MetaDataID>{2BA6F2BE-A24E-4BC0-8B2B-A2B5514BE450}</MetaDataID>
        /// <summary>
        /// This method takes a snapshot of object state. 
        /// This state is the state which is saved in storage instance 
        /// it is useful because the persistency system can knows if the state 
        /// of memory instance is deferent from the storage instance state. 
        /// If the two states differ then the persistency system execute 
        /// update command to update storage instance state.  
        /// </summary>
        internal void SnapshotStorageInstance()
        {
            if (MemoryInstance == null || ObjectStorage == null)
                throw new System.Exception("The StorageInstanceRef doesn't initialized");


            foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeValues(MemoryInstance, Class))
            {

                System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> fieldValues = null;
                if (StorageInstanceValues == null)
                {
                    StorageInstanceValues = new Dictionary<string, Dictionary<System.Reflection.FieldInfo, object>>();
                    fieldValues = new Dictionary<System.Reflection.FieldInfo, object>();
                    StorageInstanceValues[valueOfAttribute.PathIdentity] = fieldValues;
                }
                else
                {
                    if (!StorageInstanceValues.ContainsKey(valueOfAttribute.PathIdentity))
                    {
                        fieldValues = new Dictionary<System.Reflection.FieldInfo, object>();
                        StorageInstanceValues[valueOfAttribute.PathIdentity] = fieldValues;
                    }
                    else
                        fieldValues = StorageInstanceValues[valueOfAttribute.PathIdentity];
                }


#if DeviceDotNet
                if (valueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.Linq.XDocument) && valueOfAttribute.Value != null)
                    fieldValues[valueOfAttribute.FieldInfo] = System.Xml.Linq.XDocument.Parse(((System.Xml.Linq.XDocument)valueOfAttribute.Value).ToString());
                else
                    fieldValues[valueOfAttribute.FieldInfo] = valueOfAttribute.Value;
#else
                if (valueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.XmlDocument) && valueOfAttribute.Value != null)
                    fieldValues[valueOfAttribute.FieldInfo] = (System.Xml.XmlDocument)((System.Xml.XmlDocument)valueOfAttribute.Value).Clone();
                else if (valueOfAttribute.FieldInfo.FieldType == typeof(System.Xml.Linq.XDocument) && valueOfAttribute.Value != null)
                    fieldValues[valueOfAttribute.FieldInfo] = System.Xml.Linq.XDocument.Parse(((System.Xml.Linq.XDocument)valueOfAttribute.Value).ToString());
                else
                    fieldValues[valueOfAttribute.FieldInfo] = valueOfAttribute.Value;
#endif



            }

            StorageInstanceReferentialIntegrityCount = _ReferentialIntegrityCount;

            if (Class.LinkAssociation != null)
            {
                System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> fieldValues = null;
                if (!StorageInstanceValues.TryGetValue("", out fieldValues))
                {
                    fieldValues = new Dictionary<System.Reflection.FieldInfo, object>();
                    StorageInstanceValues[""] = fieldValues;
                }

                //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
                System.Reflection.FieldInfo FieldRoleA = (Class as DotNetMetaDataRepository.Class).LinkClassRoleAField;
                System.Reflection.FieldInfo FieldRoleB = (Class as DotNetMetaDataRepository.Class).LinkClassRoleBField;
                //StorageInstanceFieldValues[FieldRoleA] = FieldRoleA.GetValue(MemoryInstance);
                fieldValues[FieldRoleA] = Member<object>.GetValue((Class as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor.GetValue, MemoryInstance);
                //StorageInstanceFieldValues[FieldRoleB] = FieldRoleB.GetValue(MemoryInstance);
                fieldValues[FieldRoleB] = Member<object>.GetValue((Class as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor.GetValue, MemoryInstance);
            }
            foreach (RelResolver relResolver in RelResolvers)
            {


                DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;
                if (!relResolver.IsCompleteLoaded || associationEnd.CollectionClassifier != null)
                    continue;

                System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> fieldValues = null;
                if (!StorageInstanceValues.TryGetValue(relResolver.ValueTypePath.ToString(), out fieldValues))
                {
                    fieldValues = new Dictionary<System.Reflection.FieldInfo, object>();
                    StorageInstanceValues[relResolver.ValueTypePath.ToString()] = fieldValues;
                }

                if (relResolver.Multilingual)
                    fieldValues[relResolver.FieldInfo] = relResolver.RelatedObjectAsMultilingualObjectLinks;
                else
                    fieldValues[relResolver.FieldInfo] = relResolver.RelatedObject;
                //fieldValues[relResolver.FieldInfo] = relResolver.RelatedObject;
            }
        }






        /// <MetaDataID>{5BBDB579-3A14-4E7B-84FA-B5141AE7192C}</MetaDataID>
        /// <summary>
        /// This method informs the memory instance that the link 
        /// between the memory instance and related object has been removed from storage.
        /// </summary>
        public void ClearObjectsLink(AssociationEndAgent associationEnd, object relatedObject, bool producedFromRT)
        {
            //στην περίπτωση που ο owner είναι struct η σχέση δεν θα είναι ποτέ two way αρά σε αυτή την περίπτωση 
            //αυτή η operation δεν καλείται 
            RelResolver relResolver = null;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (!associationEnd.RealAssociationEnd.Multiplicity.IsMany)
                {

                    foreach (RelResolver tmpRelResolver in RelResolvers)
                    {
                        if (tmpRelResolver.AssociationEnd.Identity == associationEnd.Identity)
                        {
                            relResolver = tmpRelResolver;
                            break;
                        }
                    }
                    if (relResolver != null && relResolver.RelatedObject == relatedObject)
                    {
                        Dictionary<FieldInfo, object> fieldsValues = null;
                        if (!StorageInstanceValuesSnapshot.TryGetValue(relResolver.ValueTypePath.ToString(), out fieldsValues))
                        {
                            fieldsValues = new Dictionary<FieldInfo, object>();
                            if (relResolver.Multilingual)
                                fieldsValues.Add(relResolver.FieldInfo, relResolver.RelatedObjectAsMultilingualObjectLinks);
                            else
                                fieldsValues.Add(relResolver.FieldInfo, relResolver.RelatedObject);

                            StorageInstanceValuesSnapshot.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
                            relResolver.RelatedObject = relatedObject;
                            //fieldsValues = new Dictionary<FieldInfo, object>();
                            //fieldsValues.Add(relResolver.FieldInfo, relResolver.RelatedObject);
                            //StorageInstanceValues.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
                        }
                        else
                        {
                            //StorageInstanceValuesSnapshot[relResolver.ValueTypePath.ToString()][relResolver.FieldInfo] = relResolver.RelatedObject;
                            relResolver.RelatedObject = relatedObject;
                            //   StorageInstanceValues[relResolver.ValueTypePath.ToString()][relResolver.FieldInfo] = relResolver.RelatedObject;
                        }
                    }
                }
                RemoveObjectsLink(associationEnd, relatedObject);
                stateTransition.Consistent = true;
            }



            //********************** Link Class ***********************************
            //TODO: Σε αυτό το σημείο υπάρχει πρόβλημα γιατί έαν το memory instance είναι lock 
            //από άλλη transaction τότε αυτή η transaction θα οδηγηθεί σε ambort; 

            //if (Class.ClassHierarchyLinkAssociation != null && associationEnd.Identity == Class.ClassHierarchyLinkAssociation.RoleA.Identity)
            //{
            //    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(MemoryInstance))
            //    {
            //        Member<object>.SetValue(Class.LinkClassRoleAFastFieldAccessor, ref _MemoryInstance, null);
            //        StateTransition.Consistent = true;
            //    }
            //    return;
            //}
            //if (Class.ClassHierarchyLinkAssociation != null && associationEnd.Identity == Class.ClassHierarchyLinkAssociation.RoleB.Identity)
            //{
            //    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(MemoryInstance))
            //    {
            //        Member<object>.SetValue(Class.LinkClassRoleBFastFieldAccessor, ref _MemoryInstance, null);
            //        StateTransition.Consistent = true;
            //    }
            //    return;
            //}
            //********************** Link Class ***********************************




            //System.Reflection.FieldInfo field = relResolver.FieldInfo;
            //AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = relResolver.FastFieldAccessor;
            //if (field.FieldType == typeof(PersistenceLayer.ObjectContainer) || field.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
            //{
            //    #region Removes object from object collection which defines the realization of association end.
            //    PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)field.GetValue(MemoryInstance);
            //    if (theObjectContainer == null)
            //        throw new System.Exception("The object container " + field.Name + " must be initialized at construction time.");

            //    //PersistenceLayerRunTime.ObjectsOfManyRelationsip objectsOfManyRelationsip = StorageInstanceRef.GetObjectCollection(theObjectContainer) as PersistenceLayerRunTime.ObjectsOfManyRelationsip;
            //    PersistenceLayerRunTime.OnMemoryObjectCollection objectsOfManyRelationsip = StorageInstanceRef.GetObjectCollection(theObjectContainer) as PersistenceLayerRunTime.OnMemoryObjectCollection;

            //    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(MemoryInstance, Class.GetTransactionalMember(associationEnd.RealAssociationEnd)))
            //    {
            //        objectsOfManyRelationsip.Remove(relatedObject, true);
            //        StateTransition.Consistent = true;
            //    }
            //    #endregion
            //}
            //else
            //{
            //    #region Removes if needed the object from field which defines the realization of association end.
            //    //object oldMemoryInstanceValue = field.GetValue(MemoryInstance);
            //    object oldMemoryInstanceValue = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
            //    if (relResolver.RelatedObject == relatedObject || oldMemoryInstanceValue == relatedObject)
            //    {
            //        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(MemoryInstance))
            //        {
            //            if (relResolver.RelatedObject == relatedObject)
            //            {
            //                StorageInstanceValuesSnapshot[relResolver.ValueTypePath.ToString()][relResolver.FieldInfo] = relResolver.RelatedObject;
            //                relResolver.RelatedObject = null;
            //                StorageInstanceValues[relResolver.ValueTypePath.ToString()][relResolver.FieldInfo] = relResolver.RelatedObject;
            //            }
            //            if (oldMemoryInstanceValue == relatedObject)
            //            {
            //                Member<object>.SetValue(fastFieldAccessor, ref _MemoryInstance, null);
            //                //field.SetValue(MemoryInstance, null);
            //            }
            //            StateTransition.Consistent = true;
            //        }
            //    }
            //    #endregion
            //}


        }

        protected virtual void RemoveObjectsLink(AssociationEndAgent associationEnd, object relatedObject)
        {
            OOAdvantech.DotNetMetaDataRepository.AssociationEnd.RemoveObjectsLink(associationEnd.RealAssociationEnd, MemoryInstance, relatedObject);
        }

        /// <MetaDataID>{46a315a3-76ae-41a7-8802-41fd901790df}</MetaDataID>
        public object GetLinkedObject(MetaDataRepository.AssociationEnd associationEnd)
        {
            if (!associationEnd.Navigable)
                return null;
            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(associationEnd as DotNetMetaDataRepository.AssociationEnd);
            return Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);

        }

        /// <MetaDataID>{4C5928D0-A148-4A60-940F-61F72FEFA885}</MetaDataID>
        /// <summary>
        /// This method informs the memory instance that 
        /// a new link between the memory instance and related 
        /// object has been created in storage.
        /// </summary>
        public void SetObjectsLink(AssociationEndAgent associationEnd, object relatedObject, bool producedFromRT)
        {
            //στην περίπτωση που ο owner είναι struct η σχέση δεν θα είναι ποτέ two way αρά σε αυτή την περίπτωση 
            //αυτή η operation δεν καλείται 

            if (relatedObject == null)
                throw new System.Exception("You try to link memory instance with null object");



            if (!associationEnd.RealAssociationEnd.Multiplicity.IsMany)
            {

                RelResolver relResolver = null;
                foreach (RelResolver tmpRelResolver in RelResolvers)
                {
                    if (tmpRelResolver.AssociationEnd.Identity == associationEnd.Identity)
                    {
                        relResolver = tmpRelResolver;
                        break;
                    }
                }
                if (relResolver != null && relResolver.RelatedObject != relatedObject)
                {

                    Dictionary<FieldInfo, object> fieldsValues = null;
                    if (StorageInstanceValuesSnapshot != null)
                    {


                        if (!StorageInstanceValuesSnapshot.TryGetValue(relResolver.ValueTypePath.ToString(), out fieldsValues))
                        {
                            fieldsValues = new Dictionary<FieldInfo, object>();
                            if (relResolver.Multilingual)
                                fieldsValues.Add(relResolver.FieldInfo, relResolver.RelatedObjectAsMultilingualObjectLinks);
                            else
                                fieldsValues.Add(relResolver.FieldInfo, relResolver.RelatedObject);

                            StorageInstanceValuesSnapshot.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
                            relResolver.RelatedObject = relatedObject;
                        }
                        else
                        {
                            relResolver.RelatedObject = relatedObject;
                            if (relResolver.Multilingual)
                                fieldsValues[relResolver.FieldInfo] = relResolver.RelatedObjectAsMultilingualObjectLinks;
                            else
                                fieldsValues[relResolver.FieldInfo] = relResolver.RelatedObject;

                        }



                        if (!StorageInstanceValuesSnapshot.TryGetValue(relResolver.ValueTypePath.ToString(), out fieldsValues))
                        {
                            if (relResolver.Multilingual)
                            {
                                fieldsValues = new Dictionary<FieldInfo, object>
                                {

                                    { relResolver.FieldInfo, relResolver.RelatedObjectAsMultilingualObjectLinks}
                                };
                                StorageInstanceValuesSnapshot.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
                                relResolver.RelatedObject = relatedObject;

                            }
                            else
                            {
                                fieldsValues = new Dictionary<FieldInfo, object>();
                                fieldsValues.Add(relResolver.FieldInfo, relResolver.RelatedObject);
                                StorageInstanceValuesSnapshot.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
                                relResolver.RelatedObject = relatedObject;
                            }
                        }
                        else
                        {

                            relResolver.RelatedObject = relatedObject;
                            if (relResolver.Multilingual)
                                fieldsValues[relResolver.FieldInfo] = relResolver.RelatedObjectAsMultilingualObjectLinks;
                            else
                                fieldsValues[relResolver.FieldInfo] = relResolver.RelatedObject;

                        }
                    }
                    else
                        relResolver.RelatedObject = relatedObject;

                }
            }

            AddObjectsLink(associationEnd, relatedObject);



            //RelResolver relResolver = null;
            //foreach (RelResolver tmpRelResolver in RelResolvers)
            //{
            //    if (tmpRelResolver.AssociationEnd.Identity == associationEnd.Identity)
            //    {
            //        relResolver = tmpRelResolver;
            //        break;
            //    }
            //}

            ////System.Reflection.FieldInfo field = Class.GetFieldMember(associationEnd.RealAssociationEnd);
            ////if (field == null)
            ////    return;
            //AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(associationEnd.RealAssociationEnd);
            //if (fastFieldAccessor == null)
            //    return;




            ////TODO: Σε αυτό το σημείο υπάρχει πρόβλημα γιατί έαν το memory instance είναι lock 
            ////από άλλη transaction τότε αυτή η transaction θα οδηγηθεί σε ambort; 

            //if (associationEnd.RealAssociationEnd.Multiplicity.IsMany)//(field.FieldType == typeof(PersistenceLayer.ObjectContainer) || field.FieldType.IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
            //{
            //    #region Adds object to object collection which defines the realization of association end.
            //    PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)fastFieldAccessor.GetValue(MemoryInstance);
            //    if (theObjectContainer == null)
            //        throw new System.Exception("The object container " + fastFieldAccessor.MemberInfo.Name + " must be initialized at construction time.");
            //    System.Reflection.FieldInfo mFieldInfo = theObjectContainer.GetType().GetField("_theObjects", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //    //PersistenceLayerRunTime.ObjectsOfManyRelationsip ObjectCollection = (PersistenceLayerRunTime.ObjectsOfManyRelationsip)mFieldInfo.GetValue(theObjectContainer);
            //    PersistenceLayerRunTime.OnMemoryObjectCollection ObjectCollection = (PersistenceLayerRunTime.OnMemoryObjectCollection)mFieldInfo.GetValue(theObjectContainer);
            //    try
            //    {
            //        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(MemoryInstance, Class.GetTransactionalMember(associationEnd.RealAssociationEnd)))
            //        {
            //            ObjectCollection.Add(theObject, true);
            //            StateTransition.Consistent = true;
            //        }
            //    }
            //    catch (System.Exception error)
            //    {
            //        if (OOAdvantech.Transactions.ObjectStateTransition.IsLocked(MemoryInstance))
            //            throw new System.Exception("Transaction locks on member '" + associationEnd.RealAssociationEnd.FullName + "'.    Read help for LockOptions.Shared members", error);
            //        else
            //            throw;

            //    }
            //    #endregion
            //}
            //else
            //{
            //    #region Assign if it is valid the object to field which defines the realization of association end.

            //    //object oldMemoryInstanceValue = field.GetValue(MemoryInstance);
            //    object oldMemoryInstanceValue = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
            //    if (oldMemoryInstanceValue != null && oldMemoryInstanceValue != theObject)
            //        throw new System.Exception("Persistence system try to assigne to object at association end '" +
            //            associationEnd.RealAssociationEnd.GetOtherEnd().Specification.FullName + "." + associationEnd.RealAssociationEnd.Name + "' more than one objects.");
            //    try
            //    {
            //        using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(MemoryInstance, Class.GetTransactionalMember(associationEnd.RealAssociationEnd)))
            //        {
            //            Member<object>.SetValue(fastFieldAccessor, ref _MemoryInstance, theObject);
            //            //field.SetValue(MemoryInstance, theObject);

            //            if (relResolver != null)
            //            {
            //                Dictionary<FieldInfo, object> fieldsValues = null;
            //                if (!StorageInstanceValuesSnapshot.TryGetValue(relResolver.ValueTypePath.ToString(), out fieldsValues))
            //                {
            //                    fieldsValues = new Dictionary<FieldInfo, object>();
            //                    StorageInstanceValuesSnapshot.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
            //                    fieldsValues.Add(relResolver.FieldInfo, relResolver.RelatedObject);
            //                    fieldsValues = new Dictionary<FieldInfo, object>();
            //                    StorageInstanceValues.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
            //                    fieldsValues.Add(relResolver.FieldInfo, theObject);
            //                }
            //                else
            //                {
            //                    StorageInstanceValuesSnapshot[relResolver.ValueTypePath.ToString()][relResolver.FieldInfo] = relResolver.RelatedObject;
            //                    StorageInstanceValues[relResolver.ValueTypePath.ToString()][relResolver.FieldInfo] = theObject;
            //                }
            //                relResolver.RelatedObject = theObject;

            //                //StorageInstanceValuesSnapshot[relResolver.ValueTypePath.ToString()][fastFieldAccessor.MemberInfo as FieldInfo] = relResolver.RelatedObject;

            //                //StorageInstanceValues[relResolver.ValueTypePath.ToString()][fastFieldAccessor.MemberInfo as FieldInfo] = relResolver.RelatedObject;
            //            }
            //            StateTransition.Consistent = true;
            //        }
            //    }
            //    catch (System.Exception error)
            //    {
            //        //TODO να γραφτεί παράδειγμα για τα LockOptions.Shared members 
            //        if (OOAdvantech.Transactions.ObjectStateTransition.IsLocked(MemoryInstance))
            //            throw new System.Exception("Transaction locks on member '" + associationEnd.RealAssociationEnd.FullName + "'.    Read help for LockOptions.Shared members", error);
            //        else
            //            throw;

            //    }
            //    #endregion
            //}

            //if (associationEnd.RealAssociationEnd.Association.General != null)
            //{
            //    if (associationEnd.RealAssociationEnd.IsRoleA)
            //    {
            //        if (associationEnd.RealAssociationEnd.Association.General.RoleA.Navigable)
            //            OOAdvantech.DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.RealAssociationEnd.Association.General.RoleA, MemoryInstance, theObject);
            //        if (associationEnd.RealAssociationEnd.Association.General.RoleB.Navigable)
            //        {
            //            //TODO υπάρχει όταν είναι remote το Related object 
            //            OOAdvantech.DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.RealAssociationEnd.Association.General.RoleB, theObject, MemoryInstance);
            //        }
            //    }
            //    else
            //    {
            //        if (associationEnd.RealAssociationEnd.Association.General.RoleB.Navigable)
            //            OOAdvantech.DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.RealAssociationEnd.Association.General.RoleB, MemoryInstance, theObject);
            //        if (associationEnd.RealAssociationEnd.Association.General.RoleA.Navigable)
            //        {
            //            //TODO υπάρχει όταν είναι remote το Related object 
            //            OOAdvantech.DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.RealAssociationEnd.Association.General.RoleA, theObject, MemoryInstance);
            //        }
            //    }
            //}


        }

        protected virtual void AddObjectsLink(AssociationEndAgent associationEnd, object relatedObject)
        {
            OOAdvantech.DotNetMetaDataRepository.AssociationEnd.AddObjectsLink(associationEnd.RealAssociationEnd, MemoryInstance, relatedObject);
        }

        /// <MetaDataID>{1000ab97-06ce-487c-812c-07452725e3bb}</MetaDataID>
        public List<ObjectsLink> GetRelationChanges(MetaDataRepository.AssociationEnd relationType, OOAdvantech.MetaDataRepository.ValueTypePath valueTypePath)
        {
            List<ObjectsLink> objectsLinks = new List<ObjectsLink>();
            foreach (RelResolver relResolver in RelResolvers)
            {
                bool relResolverAssociationIsRelationType = false;
                relResolverAssociationIsRelationType = relationType == relResolver.AssociationEnd & valueTypePath == relResolver.ValueTypePath;
                if (!relResolverAssociationIsRelationType)
                {
                    MetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd;
                    while (associationEnd.Association.General != null)
                    {
                        if (associationEnd.IsRoleA)
                            associationEnd = associationEnd.Association.General.RoleA;
                        else
                            associationEnd = associationEnd.Association.General.RoleB;
                        if (associationEnd == relationType)
                        {
                            relResolverAssociationIsRelationType = true;
                            break;
                        }
                    }
                }
                if (relResolverAssociationIsRelationType)
                {
                    System.Reflection.FieldInfo associationEndFieldInfo = relResolver.FieldInfo;
                    if (associationEndFieldInfo.FieldType == typeof(PersistenceLayer.ObjectContainer) || associationEndFieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                    {
                        #region Produce commands for association end with multiplicity many
                        PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)GetMemoryInstanceMemberValue(relResolver);
                        if (theObjectContainer == null && !(relResolver.AssociationEnd.Namespace is MetaDataRepository.Structure))
                            throw new System.Exception("The collectio object " + Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.");
                        OOAdvantech.PersistenceLayer.ObjectCollection objectCollection = null;

                        if (theObjectContainer == null)
                            objectCollection = new OnMemoryObjectCollection();
                        else
                            objectCollection = StorageInstanceRef.GetObjectCollection(theObjectContainer);

                        if (objectCollection == null)
                            objectCollection = new OnMemoryObjectCollection();

                        if (relResolver.AssociationEnd.Namespace is MetaDataRepository.Structure && (relResolver.AssociationEnd.Namespace as MetaDataRepository.Structure).Persistent)
                        {
                            //TODO: Δεν είναι ότι το καλύτερο αυτή η μέθοδος γιατί αναγκάζει το σύστημα να φορτώσει τα related objects 
                            //εστω καί αν δεν έχει γίνει καμία μεταβολή  

                            System.Collections.Generic.List<object> relatedObjects = relResolver.GetLinkedObjects("");
                            System.Collections.Generic.List<object> onMemoryObjects = new System.Collections.Generic.List<object>();
                            foreach (object obj in objectCollection)
                            {
                                onMemoryObjects.Add(obj);
                                if (!relatedObjects.Contains(obj))
                                {

                                    StorageInstanceAgent linkedObjectStorageInstanceRef = StorageInstanceRef.GetStorageInstanceAgent(obj);
                                    if (linkedObjectStorageInstanceRef != null)
                                    {
                                        if (relResolver.AssociationEnd.IsRoleA)
                                            objectsLinks.Add(new ObjectsLink(relResolver.AssociationEnd.Association, linkedObjectStorageInstanceRef, StorageInstanceRef.GetStorageInstanceAgent(this.MemoryInstance), ObjectsLink.TypeOfChange.Added));
                                        else
                                            objectsLinks.Add(new ObjectsLink(relResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(this.MemoryInstance), linkedObjectStorageInstanceRef, ObjectsLink.TypeOfChange.Added));
                                    }

                                }
                            }
                            foreach (object obj in relatedObjects)
                            {
                                if (!onMemoryObjects.Contains(obj))
                                {

                                    if (relResolver.AssociationEnd.IsRoleA)
                                        objectsLinks.Add(new ObjectsLink(relResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(obj), StorageInstanceRef.GetStorageInstanceAgent(this.MemoryInstance), ObjectsLink.TypeOfChange.Removed));
                                    else
                                        objectsLinks.Add(new ObjectsLink(relResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(this.MemoryInstance), StorageInstanceRef.GetStorageInstanceAgent(obj), ObjectsLink.TypeOfChange.Removed));

                                }
                            }
                        }
                        else
                        {
                            PersistenceLayerRunTime.OnMemoryObjectCollection mObjectCollection = objectCollection as PersistenceLayerRunTime.OnMemoryObjectCollection;

                            if (mObjectCollection == null || mObjectCollection.RelResolver != relResolver)
                            {
                                throw new System.Exception("The object collection " + Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.\n The persistency system when create the memory " +
                                    "instance for corresponding storage instance connect the object collections of persistent associations ends with storage.\n" +
                                    " We cant assign a new object collection to association end field. ");
                            }
                            objectsLinks = mObjectCollection.GetRelationChanges();
                        }

                        #endregion
                    }
                    else
                    {
                        #region Produce commands for association end with multiplicity zero or one


                        object newValue = GetMemoryInstanceMemberValue(relResolver);// associationEndFieldInfo.GetValue(MemoryInstance);
                        //TODO Εδώ υπάρχει λίγο μπέρδεμα γιατί αν κάποιος σετάρει null και υπάρχει τιμή στην storage αλλά λόγο του lazy fetcing
                        //δεν έχει φορτωθεί τό το σύστημα το αγνώει το σετάρισμα του χρήστη.

                        if (newValue != relResolver.RelatedObject)
                        {

                            if (relResolver.RelatedObject != null)
                            {
                                if (relResolver.AssociationEnd.IsRoleA)
                                    objectsLinks.Add(new ObjectsLink(relResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(relResolver.RelatedObject), StorageInstanceRef.GetStorageInstanceAgent(this.MemoryInstance), ObjectsLink.TypeOfChange.Removed));
                                else
                                    objectsLinks.Add(new ObjectsLink(relResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(this.MemoryInstance), StorageInstanceRef.GetStorageInstanceAgent(relResolver.RelatedObject), ObjectsLink.TypeOfChange.Removed));

                            }
                            if (newValue != null)
                            {
                                StorageInstanceAgent linkedObjectStorageInstanceRef = StorageInstanceRef.GetStorageInstanceAgent(newValue);
                                if (linkedObjectStorageInstanceRef != null)
                                {
                                    if (relResolver.AssociationEnd.IsRoleA)
                                        objectsLinks.Add(new ObjectsLink(relResolver.AssociationEnd.Association, linkedObjectStorageInstanceRef, StorageInstanceRef.GetStorageInstanceAgent(this.MemoryInstance), ObjectsLink.TypeOfChange.Added));
                                    else
                                        objectsLinks.Add(new ObjectsLink(relResolver.AssociationEnd.Association, StorageInstanceRef.GetStorageInstanceAgent(this.MemoryInstance), linkedObjectStorageInstanceRef, ObjectsLink.TypeOfChange.Added));
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            if (relationType.Association == Class.ClassHierarchyLinkAssociation)
            {
                if (PersistentObjectID == null)
                {
                    object roleA = Member<object>.GetValue(Class.LinkClassRoleAFastFieldAccessor.GetValue, MemoryInstance);
                    object roleB = Member<object>.GetValue(Class.LinkClassRoleBFastFieldAccessor.GetValue, MemoryInstance);
                    objectsLinks.Add(new ObjectsLink(relationType.Association, StorageInstanceRef.GetStorageInstanceAgent(roleA), StorageInstanceRef.GetStorageInstanceAgent(roleB), StorageInstanceRef.GetStorageInstanceAgent(MemoryInstance), ObjectsLink.TypeOfChange.Added));
                }
            }

            return objectsLinks;

        }

        /// <MetaDataID>{62A7B045-4172-413F-B9F3-4380CA570280}</MetaDataID>
        /// <summary>
        /// This method produces the corresponding commands 
        /// for all changes of object relationships. 
        /// The executions of these commands remove or add relationships in storage.
        /// </summary>
        public void MakeRelationChangesCommands()
        {
            if (Class.LinkAssociation != null)
                ValidateRelatedObjects();

            using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this))
            {
                Transactions.LockType memoryInstanceTransactionLockType = Transactions.ObjectStateTransition.GetTransactionLockType(MemoryInstance);
                List<System.Reflection.FieldInfo> partialObjectLockFields = null;
                if (memoryInstanceTransactionLockType == OOAdvantech.Transactions.LockType.PartialObjectLock ||
                    memoryInstanceTransactionLockType == OOAdvantech.Transactions.LockType.SharedFieldsParticipation)
                    partialObjectLockFields = Transactions.ObjectStateTransition.GetPartialObjectLockFields(MemoryInstance);


                foreach (RelResolver relResolver in RelResolvers)
                {
                    System.Reflection.FieldInfo associationEndFieldInfo = relResolver.FieldInfo;
                    if (relResolver.ValueTypePath.Count > 0)
                    {
                        MetaDataRepository.MetaObjectID attributeIdentity = relResolver.ValueTypePath.ToArray()[relResolver.ValueTypePath.Count - 1];
                        MetaDataRepository.Attribute attribute = (Class as MetaDataRepository.Classifier).GetAttribute(attributeIdentity);
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = _Class.GetFastFieldAccessor(attribute as DotNetMetaDataRepository.Attribute);
                        if (partialObjectLockFields != null && fastFieldAccessor.MemberInfo is FieldInfo && !partialObjectLockFields.Contains(fastFieldAccessor.MemberInfo as FieldInfo))
                            continue; //On current transaction the object participate partially and the relation resolver field doesn't participate in transaction 
                    }
                    else
                    {
                        if (partialObjectLockFields != null && !partialObjectLockFields.Contains(associationEndFieldInfo))
                            continue;//On current transaction the object participate partially and the relation resolver field doesn't participate in transaction
                    }

                    if (relResolver.Multilingual && (relResolver.AssociationEnd.Association.General != null || relResolver.AssociationEnd.Association.Specializations.Count > 0))
                        throw new NotSupportedException("Multilingual doesn't supported in hierarchy  associations");

                    if (associationEndFieldInfo.FieldType == typeof(PersistenceLayer.ObjectContainer) || associationEndFieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                    {
                        // many association end 
                        if (relResolver.AssociationEnd.Namespace is MetaDataRepository.Structure &&
                            (relResolver.AssociationEnd.Namespace as MetaDataRepository.Structure).Persistent)
                        {
                            // value type relation
                            if (relResolver.ValueTypePath.Multilingual)
                            {
                                // multilingual value type relation
                                #region  Produce commands for multilingual value type  association end with multiplicity many 
                                foreach (System.Collections.DictionaryEntry entry in GetMultilingualMemoryInstanceMemberValue(relResolver))
                                {
                                    using (var cultureConext = new OOAdvantech.CultureContext((System.Globalization.CultureInfo)entry.Key, false))
                                    {
                                        PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)entry.Value;
                                        OOAdvantech.PersistenceLayer.ObjectCollection objectCollection = null;

                                        if (theObjectContainer != null)
                                            objectCollection = StorageInstanceRef.GetObjectCollection(theObjectContainer);
                                        if (objectCollection == null)
                                            objectCollection = new OnMemoryObjectCollection();
                                        //TODO: Δεν είναι ότι το καλύτερο αυτή η μέθοδος γιατί αναγκάζει το σύστημα να φορτώσει τα related objects 
                                        //εστω καί αν δεν έχει γίνει καμία μεταβολή  

                                        MakeRelationChangesCommands(relResolver, objectCollection);
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                // regular value type relation
                                #region  Produce commands for value type  association end with multiplicity many 

                                OOAdvantech.PersistenceLayer.ObjectCollection objectCollection = null;
                                PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)GetMemoryInstanceMemberValue(relResolver);
                                if (theObjectContainer != null)
                                    objectCollection = StorageInstanceRef.GetObjectCollection(theObjectContainer);
                                if (objectCollection == null)
                                    objectCollection = new OnMemoryObjectCollection();

                                MakeRelationChangesCommands(relResolver, objectCollection);

                                #endregion
                            }
                        }
                        else
                        {
                            // non value type relation
                            #region Produce commands for association end with multiplicity many
                            PersistenceLayer.ObjectContainer theObjectContainer = (PersistenceLayer.ObjectContainer)GetMemoryInstanceMemberValue(relResolver);
                            if (theObjectContainer == null)
                                throw new System.Exception("The collection object " + Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.");

                            OnMemoryObjectCollection onMemoryObjectCollection = StorageInstanceRef.GetObjectCollection(theObjectContainer) as PersistenceLayerRunTime.OnMemoryObjectCollection; ;
                            if (onMemoryObjectCollection == null || onMemoryObjectCollection.RelResolver != relResolver)
                            {
                                throw new System.Exception("The object collection " + Class.FullName + "." + relResolver.AssociationEnd.Name + " has loose the connection with storage.\n The persistency system when create the memory " +
                                    "instance for corresponding storage instance connect the object collections of persistent associations ends with storage.\n" +
                                    " We cant assign a new object collection to association end field. ");
                            }
                            onMemoryObjectCollection.MakeChangesCommands();

                            #endregion
                        }
                    }
                    else
                    {
                        if (relResolver.Multilingual || relResolver.ValueTypePath.Multilingual)
                            MakeMultilingualRelationChangesCommands(relResolver);
                        else
                            MakeRelationChangesCommands(relResolver);

                    }
                }
                StateTransition.Consistent = true;
            }
        }
        /// <summary>
        /// Compare relation resolver collection and memory instance collection and produce link and unlink commands
        /// </summary>
        /// <param name="relResolver">
        /// Relation resolver has the storage instance related objects
        /// </param>
        /// <param name="objectCollection">
        /// The objectCollection contains memory instance related objects
        /// </param>
        private static void MakeRelationChangesCommands(RelResolver relResolver, PersistenceLayer.ObjectCollection objectCollection)
        {
            System.Collections.Generic.List<object> relatedObjects = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                relatedObjects = relResolver.GetLinkedObjects("");
                stateTransition.Consistent = true;
            }
            System.Collections.Generic.List<object> onMemoryObjects = new List<object>();
            int index = 0;
            foreach (object obj in objectCollection)
            {
                onMemoryObjects.Add(obj);
                if (!relatedObjects.Contains(obj))
                {
                    bool changeApplied = false;
                    relResolver.LinkObject(obj, index, out changeApplied);
                }
                index++;
            }
            index = 0;
            foreach (object obj in relatedObjects)
            {
                if (!onMemoryObjects.Contains(obj))
                {
                    bool changeApplied = false;
                    relResolver.UnLinkObject(obj, index, out changeApplied);
                }
                index++;
            }
        }

        List<CollectionChange> GetMultilingualChanges(List<MultilingualObjectLink> storageInstanceMultilingualRelatedObject, List<MultilingualObjectLink> memoryInstanceMultilingualRelatedObject)
        {

            List<CollectionChange> changes = new List<CollectionChange>();
            foreach (var storageMultilingualRelatedObject in storageInstanceMultilingualRelatedObject)
            {
                var memoryMultilingualRelatedObject = memoryInstanceMultilingualRelatedObject.Where(x => x.Culture.Name == storageMultilingualRelatedObject.Culture.Name && x.LinkedObject == storageMultilingualRelatedObject.LinkedObject).FirstOrDefault();
                if (memoryMultilingualRelatedObject.LinkedObject == null)
                    changes.Add(new CollectionChange(storageMultilingualRelatedObject, CollectionChange.ChangeType.Deleteded));
            }
            foreach (var memoryMultilingualRelatedObject in memoryInstanceMultilingualRelatedObject)
            {
                if (memoryMultilingualRelatedObject.LinkedObject != null)
                {
                    var storageMultilingualRelatedObject = storageInstanceMultilingualRelatedObject.Where(x => x.Culture.Name == memoryMultilingualRelatedObject.Culture.Name && x.LinkedObject == memoryMultilingualRelatedObject.LinkedObject).FirstOrDefault();
                    if (storageMultilingualRelatedObject.LinkedObject == null)
                        changes.Add(new CollectionChange(memoryMultilingualRelatedObject, CollectionChange.ChangeType.Added));
                }
                else
                {

                }
            }
            return changes;
        }

        /// <summary>
        /// Produce commands for multilingual association end with multiplicity zero or one
        /// </summary>
        /// <param name="relResolver">
        /// Defines the relation resolver for multilingual  association end with multiplicity zero or one.
        /// </param>
        private void MakeMultilingualRelationChangesCommands(RelResolver relResolver)
        {
            #region Produce commands for association end with multiplicity zero or one
            List<MultilingualObjectLink> memoryRelatedObjectAsMultilingualObjectLinks = (GetMemoryInstanceMemberValue(relResolver) as System.Collections.IDictionary).OfType<System.Collections.DictionaryEntry>().Where(x => x.Value != null).Select(x => new MultilingualObjectLink() { Culture = x.Key as System.Globalization.CultureInfo, LinkedObject = x.Value }).ToList();

            List<MultilingualObjectLink> storageRelatedObjectAsMultilingualObjectLinks = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                storageRelatedObjectAsMultilingualObjectLinks = relResolver.RelatedObjectAsMultilingualObjectLinks;
                stateTransition.Consistent = true;
            }

            if (!relResolver.IsCompleteLoaded && memoryRelatedObjectAsMultilingualObjectLinks.Count == 0)
            {
                //memoryRelatedObjectAsMultilingualObjectLinks.Count == 0 In all language the value is null
                if (memoryRelatedObjectAsMultilingualObjectLinks != storageRelatedObjectAsMultilingualObjectLinks)
                {
                    LazyFetching(relResolver, relResolver.Owner.Class.GetExtensionMetaObject<System.Type>()); //Load in all languages
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        storageRelatedObjectAsMultilingualObjectLinks = relResolver.RelatedObjectAsMultilingualObjectLinks;
                        stateTransition.Consistent = true;
                    }
                }

                memoryRelatedObjectAsMultilingualObjectLinks = (GetMemoryInstanceMemberValue(relResolver) as System.Collections.IDictionary).OfType<System.Collections.DictionaryEntry>().Where(x => x.Value != null).Select(x => new MultilingualObjectLink() { Culture = x.Key as System.Globalization.CultureInfo, LinkedObject = x.Value }).ToList();
            }


            var changes = GetMultilingualChanges(storageRelatedObjectAsMultilingualObjectLinks, memoryRelatedObjectAsMultilingualObjectLinks);

            if (changes.Count > 0)
            {
                if (!StorageInstanceValuesSnapshot.TryGetValue(relResolver.ValueTypePath.ToString(), out Dictionary<FieldInfo, object> fieldsValues))
                {
                    fieldsValues = new Dictionary<FieldInfo, object>();
                    StorageInstanceValuesSnapshot.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
                }
                fieldsValues[relResolver.FieldInfo] = relResolver.RelatedObjectAsMultilingualObjectLinks;

                foreach (var change in changes)
                {
                    using (CultureContext cultureContext = new CultureContext(((MultilingualObjectLink)change.Object).Culture, false))
                    {
                        if (change.TypeOfChange == CollectionChange.ChangeType.Deleteded)
                            relResolver.UnLinkObject(((MultilingualObjectLink)change.Object).LinkedObject, -1, out bool changeApplied);
                        else
                            relResolver.LinkObject(((MultilingualObjectLink)change.Object).LinkedObject, 0, out bool changeApplied);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Produce commands for association end with multiplicity zero or one
        /// </summary>
        /// <param name="relResolver">
        /// Defines the relation resolver for  association end with multiplicity zero or one.
        /// </param>
        private void MakeRelationChangesCommands(RelResolver relResolver)
        {
            #region Produce commands for association end with multiplicity zero or one


            object newValue = null;
            System.Runtime.Remoting.Messaging.CallContext.SetData("MakeRelationChangesCommands", true);
            newValue = GetMemoryInstanceMemberValue(relResolver);
            System.Runtime.Remoting.Messaging.CallContext.FreeNamedDataSlot("MakeRelationChangesCommands");


            object storageInstanceRelatedObject = null;

            bool relResolverCompleteLoaded = relResolver.IsCompleteLoaded;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                if (relResolver.IsCompleteLoaded)
                    storageInstanceRelatedObject = relResolver.RelatedObject;
                stateTransition.Consistent = true;
            }

            if (!relResolverCompleteLoaded && newValue == null)
            {
                if (newValue != storageInstanceRelatedObject)
                {
                    LazyFetching(relResolver, relResolver.Owner.Class.GetExtensionMetaObject<System.Type>());
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        storageInstanceRelatedObject = relResolver.RelatedObject;
                        stateTransition.Consistent = true;
                    }
                }


                if (newValue == null)
                {
                    System.Runtime.Remoting.Messaging.CallContext.SetData("MakeRelationChangesCommands", true);
                    newValue = GetMemoryInstanceMemberValue(relResolver);
                    System.Runtime.Remoting.Messaging.CallContext.FreeNamedDataSlot("MakeRelationChangesCommands");
                }
            }

            //TODO Εδώ υπάρχει λίγο μπέρδεμα γιατί αν κάποιος σετάρει null και υπάρχει τιμή στην storage αλλά λόγο του lazy fetcing
            //δεν έχει φορτωθεί τό το σύστημα το αγνώει το σετάρισμα του χρήστη.


            if (newValue != storageInstanceRelatedObject)
            {
                if (!StorageInstanceValuesSnapshot.TryGetValue(relResolver.ValueTypePath.ToString(), out Dictionary<FieldInfo, object> fieldsValues))
                {
                    fieldsValues = new Dictionary<FieldInfo, object>();
                    StorageInstanceValuesSnapshot.Add(relResolver.ValueTypePath.ToString(), fieldsValues);
                }
                if (relResolver.Multilingual)
                    fieldsValues[relResolver.FieldInfo] = relResolver.RelatedObjectAsMultilingualObjectLinks;
                else
                    fieldsValues[relResolver.FieldInfo] = relResolver.RelatedObject;


                if (relResolver.RelatedObject != null)
                    relResolver.UnLinkObject(relResolver.RelatedObject, -1, out bool changeApplied);

                if (newValue != null)
                {
                    bool deriveAssociationValue = false;
                    if (relResolver.AssociationEnd.Association.Specializations.Count > 0)
                    {
                        foreach (RelResolver _relResolver in RelResolvers)
                        {
                            if (_relResolver.AssociationEnd.Association.IsGeneral(relResolver.AssociationEnd.Association))
                            {
                                if (GetMemoryInstanceMemberValue(relResolver) == newValue)
                                    deriveAssociationValue = true;
                            }
                        }
                    }
                    if (!deriveAssociationValue)
                        relResolver.LinkObject(newValue, 0, out bool changeApplied);
                }
            }
            #endregion
        }



        /// <MetaDataID>{2B276948-6DE9-42E8-B76A-F951668938F6}</MetaDataID>
        protected abstract RelResolver CreateRelationResolver(DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor);

        /// <MetaDataID>{1a2e3e59-42f9-4ae9-ba8a-4d63b359f19c}</MetaDataID>
        protected abstract RelResolver CreateRelationResolver(DotNetMetaDataRepository.AssociationEnd thePersistentAssociationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, StorageInstanceRef owner);


        /// <summary>
        /// This method retrieves the value of _theObjects field.
        /// Persistence system use this method to break the access level of field _theObjects 
        /// of ObjectContainer class
        /// </summary>
        /// <MetaDataID>{B4B1452F-813A-4563-90DE-50501FA1164D}</MetaDataID>
        public static new PersistenceLayer.ObjectCollection GetObjectCollection(PersistenceLayer.ObjectContainer objectContainer)
        {
            return PersistenceLayer.StorageInstanceRef.GetObjectCollection(objectContainer);
        }

        ///<summary>
        /// This method sets the value of _theObjects field.
        /// Persistence system use this method to break the access level of field _theObjects 
        /// of ObjectContainer class
        /// </summary>
        /// <MetaDataID>{A7CA511E-FCFA-43A4-A8CD-8F8BA21E9301}</MetaDataID>
        public static new void SetObjectCollection(PersistenceLayer.ObjectContainer objectContainer, PersistenceLayer.ObjectCollection objectCollection)
        {
            PersistenceLayer.StorageInstanceRef.SetObjectCollection(objectContainer, objectCollection);
        }
        /// <MetaDataID>{11dc7c68-3890-468c-bcce-b53c229bd650}</MetaDataID>
        System.Collections.Generic.List<RelResolver> GetRelResolvers()
        {
            System.Collections.Generic.List<RelResolver> relResolvers = new System.Collections.Generic.List<RelResolver>();

            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in Class.GetPersistentAssociateRoles())
            {
                if (!associationEnd.Navigable)
                    continue;
                OOAdvantech.AccessorBuilder.FieldPropertyAccessor associationEndfastFieldAccessor = Class.GetFastFieldAccessor(associationEnd);
                if(associationEndfastFieldAccessor==null)
                {

                }
                relResolvers.Add(CreateRelationResolver(associationEnd, associationEndfastFieldAccessor));
            }
            MetaDataRepository.ValueTypePath valueTypePath = new MetaDataRepository.ValueTypePath();


            foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetPersistentAttributes())
            {
                if ((attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;
                if (Class.IsMultilingual(attribute))
                    valueTypePath.Multilingual = true;

                valueTypePath.Push(attribute.Identity);
                if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                    relResolvers.AddRange(GetRelResolvers(attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath));
                valueTypePath.Pop();
            }
            return relResolvers;
        }

        /// <MetaDataID>{7146d714-abf7-4e3f-b91f-361e3d167c20}</MetaDataID>
        System.Collections.Generic.List<RelResolver> GetRelResolvers(DotNetMetaDataRepository.Structure structure, MetaDataRepository.ValueTypePath valueTypePath)
        {
            System.Collections.Generic.List<RelResolver> relResolvers = new System.Collections.Generic.List<RelResolver>();

            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
            {
                if (!Class.IsPersistent(associationEnd) || !associationEnd.Navigable)
                    continue;

                OOAdvantech.AccessorBuilder.FieldPropertyAccessor associationEndfastFieldAccessor = structure.GetFastFieldAccessor(associationEnd);
                relResolvers.Add(CreateRelationResolver(associationEnd, associationEndfastFieldAccessor, new StorageInstanceValuePathRef(this, valueTypePath)));
            }
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;
                valueTypePath.Push(attribute.Identity);
                if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                    relResolvers.AddRange(GetRelResolvers(attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath));
                valueTypePath.Pop();
            }
            return relResolvers;
        }
        /// <MetaDataID>{6c9e1f77-2215-4f25-9d10-7eecd3716bfb}</MetaDataID>
        internal protected object GetMemoryInstanceMemberValue(RelResolver relResolver)
        {
            System.Type type = MemoryInstance.GetType();
            if (relResolver.FieldInfo.DeclaringType == type || type.GetMetaData().IsSubclassOf(relResolver.FieldInfo.DeclaringType)) //if (Class.IsA(relResolver.AssociationEnd.Namespace as MetaDataRepository.Classifier))
            {
                //return relResolver.FieldInfo.GetValue(MemoryInstance);
                if (relResolver.Multilingual)
                    return MultilingualMember<object>.GetValue(relResolver.FastFieldAccessor.GetValue, MemoryInstance);
                else
                    return Member<object>.GetValue(relResolver.FastFieldAccessor.GetValue, MemoryInstance);
            }
            else
            {
                MetaDataRepository.ValueTypePath valueTypePath = new MetaDataRepository.ValueTypePath();
                foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                {

                    if (!Class.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                        continue;
                    if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                    {
                        bool valueGetted = false;
                        object value = null;
                        //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                        //object owner = fieldInfo.GetValue(MemoryInstance);
                        bool multilingual = Class.IsMultilingual(attribute);
                        if (multilingual)
                        {
                            System.Collections.IDictionary multilingualOwner = (System.Collections.IDictionary)MultilingualMember<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                            valueTypePath.Push(attribute.Identity);
                            GetMultilinguaRelResolverValue(multilingualOwner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, out value, out valueGetted);
                            valueTypePath.Pop();
                            if (valueGetted)
                                return value;
                        }
                        else
                        {

                            object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                            valueTypePath.Push(attribute.Identity);
                            GetRelResolverValue(owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, out value, out valueGetted);
                            valueTypePath.Pop();
                            if (valueGetted)
                                return value;
                        }
                    }
                }
                throw new System.Exception("System can't get value of '" + relResolver.AssociationEnd.Name + "'");
            }
        }



        internal protected System.Collections.IDictionary GetMultilingualMemoryInstanceMemberValue(RelResolver relResolver)
        {
            if (!relResolver.Multilingual)
                throw new Exception("Relation resolver isn't multilingual");
            System.Type type = MemoryInstance.GetType();
            if (relResolver.FieldInfo.DeclaringType == type || type.GetMetaData().IsSubclassOf(relResolver.FieldInfo.DeclaringType)) //if (Class.IsA(relResolver.AssociationEnd.Namespace as MetaDataRepository.Classifier))
            {
                return MultilingualMember<object>.GetValue(relResolver.FastFieldAccessor.GetValue, MemoryInstance) as System.Collections.IDictionary;
            }
            else
            {
                MetaDataRepository.ValueTypePath valueTypePath = new MetaDataRepository.ValueTypePath();
                foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                {
                    if (!Class.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                        continue;
                    if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                    {
                        bool valueGetted = false;
                        object value = null;
                        //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                        //object owner = fieldInfo.GetValue(MemoryInstance);
                        bool multilingual = Class.IsMultilingual(attribute);
                        if (multilingual)
                        {
                            System.Collections.IDictionary multilingualOwner = (System.Collections.IDictionary)MultilingualMember<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                            valueTypePath.Push(attribute.Identity);
                            GetMultilinguaRelResolverValue(multilingualOwner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, out value, out valueGetted);
                            valueTypePath.Pop();
                            if (valueGetted)
                                return value as System.Collections.IDictionary;
                        }
                    }
                }
                throw new System.Exception("System can't get value of '" + relResolver.AssociationEnd.Name + "'");
            }
        }

        void GetMultilinguaRelResolverValue(System.Collections.IDictionary multiligualValue, RelResolver relResolver, DotNetMetaDataRepository.Structure structure, MetaDataRepository.ValueTypePath valueTypePath, out object value, out bool valueGetted)
        {
            valueGetted = false;
            value = null;
            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
            {

                if (associationEnd == relResolver.AssociationEnd &&
                    relResolver.ValueTypePath == valueTypePath)
                {

                    if (multiligualValue != null)
                    {
                        System.Collections.Hashtable multilingualRoleValues = new System.Collections.Hashtable();
                        foreach (System.Collections.DictionaryEntry entry in (multiligualValue as System.Collections.IDictionary))
                        {
                            System.Globalization.CultureInfo culture = entry.Key as System.Globalization.CultureInfo;
                            using (CultureContext cultureContext = new CultureContext(culture, false))
                            {
                                value = Member<object>.GetValue(relResolver.FastFieldAccessor.GetValue, entry.Value);
                                multilingualRoleValues.Add(culture, value);
                            }
                        }
                        valueGetted = true;
                        value = multilingualRoleValues;
                    }
                    return;
                }
            }

            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;

                if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {

                    if (multiligualValue != null)
                    {
                        System.Collections.Hashtable multilingualRoleValues = new System.Collections.Hashtable();
                        foreach (System.Collections.DictionaryEntry entry in (multiligualValue as System.Collections.IDictionary))
                        {
                            System.Globalization.CultureInfo culture = entry.Key as System.Globalization.CultureInfo;
                            using (CultureContext cultureContext = new CultureContext(culture, false))
                            {
                                var memoryInstance = entry.Value;
                                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                                valueTypePath.Push(attribute.Identity);
                                GetRelResolverValue(owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, out value, out valueGetted);
                                valueTypePath.Pop();
                                multilingualRoleValues.Add(culture, value);
                            }
                        }
                        value = multilingualRoleValues;
                    }
                    return;
                }
            }

        }


        /// <MetaDataID>{2f1170bb-5b46-40c1-bff4-9d621f7e5285}</MetaDataID>
        void GetRelResolverValue(object memoryInstance, RelResolver relResolver, DotNetMetaDataRepository.Structure structure, MetaDataRepository.ValueTypePath valueTypePath, out object value, out bool valueGetted)
        {
            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
            {
                if (associationEnd == relResolver.AssociationEnd &&
                    relResolver.ValueTypePath == valueTypePath)
                {
                    valueGetted = true;
                    if (memoryInstance == null)
                        value = null;
                    else
                    {
                        if (relResolver.Multilingual)
                            value = MultilingualMember<object>.GetValue(relResolver.FastFieldAccessor.GetValue, memoryInstance);
                        else
                            value = Member<object>.GetValue(relResolver.FastFieldAccessor.GetValue, memoryInstance);
                    }
                    return;
                }
            }
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;
                if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {
                    AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                    object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                    valueTypePath.Push(attribute.Identity);
                    GetRelResolverValue(owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, valueTypePath, out value, out valueGetted);
                    valueTypePath.Pop();
                    return;
                }
            }
            value = null;
            valueGetted = false;
        }
        /// <summary>
        /// Set member multilingual value for resolver
        /// </summary>
        /// <param name="relResolver">
        /// Define the relation resolver which controls the related object.
        /// the related object loaded on instance member 
        /// </param>
        /// <param name="multilingualObjectLinks">
        /// In multilingual context may exist multiple related objects one for each culture
        /// </param>
        internal protected void SetMultilingualMemberValue(RelResolver relResolver, List<MultilingualObjectLink> multilingualObjectLinks)
        {

            object memoryInstance = MemoryInstance;
            System.Type type = MemoryInstance.GetType();
            lock (relResolver)
            {
                if (relResolver.FieldInfo.DeclaringType == type || type.GetMetaData().IsSubclassOf(relResolver.FieldInfo.DeclaringType)) //if (Class.IsA(relResolver.AssociationEnd.Namespace as MetaDataRepository.Classifier))
                {
                    if (relResolver.Multilingual)
                    {
                        if (!relResolver.FieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                        {
                            foreach (var multiligualObjectLink in multilingualObjectLinks)
                            {
                                using (CultureContext cultureContext = new CultureContext(multiligualObjectLink.Culture, false))
                                {
                                    Member<object>.SetValueImplicitly(relResolver.FastFieldAccessor, ref memoryInstance, multiligualObjectLink.LinkedObject);
                                }
                            }
                        }
                    }
                    else
                        throw new Exception("Relation resolver isn't multilingual");
                }
                else
                {
                    foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                    {
                        if (!Class.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                            continue;

                        if (relResolver.ValueTypePathContainsAttribute(attribute))
                        {
                            if (Class.IsMultilingual(attribute))
                            {
                                foreach (MultilingualObjectLink multilingualObjectLink in multilingualObjectLinks)
                                {
                                    using (OOAdvantech.CultureContext cultureContext = new CultureContext(multilingualObjectLink.Culture, false))
                                    {
                                        bool valueSetted = false;
                                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                        object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                                        SetMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLink.LinkedObject, 1, out valueSetted);
                                        if (valueSetted)
                                        {
                                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                        }
                                        else
                                            throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                                    }
                                }
                                return;
                            }
                            else
                            {
                                bool valueSetted = false;
                                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                SetMultilingualMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLinks, 1, out valueSetted);
                                if (valueSetted)
                                {
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                    return;
                                }

                                //bool valueSetted = false;
                                //AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                //object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                //SetMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLinks, 1, out valueSetted);
                                //if (valueSetted)
                                //{
                                //    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                //    return;
                                //}
                            }
                        }
                    }

                    throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                }

            }
            //relResolver._IsCompleteLoaded=true;
        }


        internal protected void LoadMultilingualCollection(RelResolver relResolver, List<MultilingualObjectLink> multilingualObjectLinks)
        {

            object memoryInstance = MemoryInstance;
            System.Type type = MemoryInstance.GetType();
            lock (relResolver)
            {
                if (relResolver.FieldInfo.DeclaringType == type || type.GetMetaData().IsSubclassOf(relResolver.FieldInfo.DeclaringType)) //if (Class.IsA(relResolver.AssociationEnd.Namespace as MetaDataRepository.Classifier))
                {
                    if (relResolver.Multilingual)
                    {
                        if (!relResolver.FieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                        {
                            foreach (var multiligualObjectLink in multilingualObjectLinks)
                            {
                                using (CultureContext cultureContext = new CultureContext(multiligualObjectLink.Culture, false))
                                {
                                    PersistenceLayer.ObjectContainer objectContainer = Member<object>.GetValue(relResolver.FastFieldAccessor.GetValue, memoryInstance) as PersistenceLayer.ObjectContainer;
                                    OnMemoryObjectCollection collection = PersistenceLayer.StorageInstanceRef.GetObjectCollection(objectContainer) as OnMemoryObjectCollection;
                                    collection.Add(multiligualObjectLink.LinkedObject, true);

                                }
                            }
                        }
                    }
                    else
                        throw new Exception("Relation resolver isn't multilingual");
                }
                else
                {
                    foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                    {
                        if (!Class.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                            continue;

                        if (relResolver.ValueTypePathContainsAttribute(attribute))
                        {
                            if (Class.IsMultilingual(attribute))
                            {
                                foreach (MultilingualObjectLink multilingualObjectLink in multilingualObjectLinks)
                                {
                                    using (OOAdvantech.CultureContext cultureContext = new CultureContext(multilingualObjectLink.Culture, false))
                                    {
                                        bool valueSetted = false;
                                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                        object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                                        LoadCollection(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLink.LinkedObject, 1, out valueSetted);
                                        if (valueSetted)
                                        {
                                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                        }
                                        else
                                            throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                                    }
                                }
                                return;
                            }
                            else
                            {
                                bool valueSetted = false;
                                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                LoadMultilingualCollection(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLinks, 1, out valueSetted);
                                if (valueSetted)
                                {
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                    return;
                                }

                                //bool valueSetted = false;
                                //AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                //object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                //SetMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLinks, 1, out valueSetted);
                                //if (valueSetted)
                                //{
                                //    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                //    return;
                                //}
                            }
                        }
                    }

                    throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                }

            }
            //relResolver._IsCompleteLoaded=true;
        }


        void LoadMultilingualCollection(ref object memoryInstance, RelResolver relResolver, DotNetMetaDataRepository.Structure structure, List<MultilingualObjectLink> multilingualObjectLinks, int pathIndex, out bool valueSetted)
        {
            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
            {
                if (associationEnd == relResolver.AssociationEnd)
                {
                    valueSetted = true;
                    if (relResolver.Multilingual)
                    {
                        if (!relResolver.FieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                        {
                            foreach (var multiligualObjectLink in multilingualObjectLinks)
                            {
                                using (CultureContext cultureContext = new CultureContext(multiligualObjectLink.Culture, false))
                                {
                                    Member<object>.SetValueImplicitly(relResolver.FastFieldAccessor, ref memoryInstance, multiligualObjectLink.LinkedObject);
                                }
                            }
                        }
                    }
                    else
                        throw new Exception("Relation resolver isn't multilingual");

                    return;
                }
            }
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;

                if (relResolver.ValueTypePathContainsAttribute(attribute, pathIndex))
                {
                    if (structure.IsMultilingual(attribute))
                    {
                        valueSetted = false;
                        foreach (MultilingualObjectLink multilingualObjectLink in multilingualObjectLinks)
                        {
                            using (OOAdvantech.CultureContext cultureContext = new CultureContext(multilingualObjectLink.Culture, false))
                            {
                                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                                LoadCollection(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLink.LinkedObject, pathIndex, out valueSetted);
                                if (valueSetted)
                                {
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                }
                                else
                                    throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                            }
                        }
                        return;
                    }
                    else
                    {

                        //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                        object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                        SetMultilingualMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLinks, pathIndex, out valueSetted);
                        if (valueSetted)
                        {
                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                            return;
                        }
                    }
                }
            }
            throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");

        }




        void LoadCollection(ref object memoryInstance, RelResolver relResolver, DotNetMetaDataRepository.Structure structure, object value, int pathIndex, out bool valueSetted)
        {

            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
            {
                if (associationEnd == relResolver.AssociationEnd)
                {
                    valueSetted = true;
                    if (memoryInstance == null)
                        throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                    else
                    {

                        PersistenceLayer.ObjectContainer objectContainer = Member<object>.GetValue(relResolver.FastFieldAccessor.GetValue, memoryInstance) as PersistenceLayer.ObjectContainer;
                        if (objectContainer == null)
                        {
                            objectContainer = AccessorBuilder.CreateInstance(relResolver.FieldInfo.FieldType) as PersistenceLayer.ObjectContainer;
                            Member<object>.SetValueImplicitly(relResolver.FastFieldAccessor, ref memoryInstance, objectContainer);
                        }
                        OnMemoryObjectCollection collection = PersistenceLayer.StorageInstanceRef.GetObjectCollection(objectContainer) as OnMemoryObjectCollection;
                        collection.Add(value, true);

                    }


                    return;
                }
            }
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;

                if (relResolver.ValueTypePathContainsAttribute(attribute, pathIndex))
                {
                    //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                    AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                    object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                    SetMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, value, pathIndex, out valueSetted);
                    if (valueSetted)
                    {
                        Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                        return;
                    }
                }
            }
            throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");

        }




        /// <summary>
        /// Set value of member which is part of  value type attribute 
        /// </summary>
        /// <param name="memoryInstance">
        /// Defines the value type memory instance 
        /// </param>
        /// <param name="relResolver">
        /// Define the relation resolver which controls the related object.
        /// the related object loaded on instance member 
        /// </param>
        /// <param name="structure">
        /// Defines the value type meta data
        /// </param>
        /// <param name="multilingualObjectLinks">
        /// In multilingual context may exist multiple related objects one for each culture
        /// </param>
        /// <param name="pathIndex">
        /// Defines the index of value type path (value type nested in value type 
        /// </param>
        /// <param name="valueSetted">
        /// Defines output parameter.
        /// When valueSetted is true the value type attribute must be updated with the updated memory instance
        /// </param>
        void SetMultilingualMemberValue(ref object memoryInstance, RelResolver relResolver, DotNetMetaDataRepository.Structure structure, List<MultilingualObjectLink> multilingualObjectLinks, int pathIndex, out bool valueSetted)
        {
            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
            {
                if (associationEnd == relResolver.AssociationEnd)
                {
                    valueSetted = true;
                    if (relResolver.Multilingual)
                    {
                        if (!relResolver.FieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                        {
                            foreach (var multiligualObjectLink in multilingualObjectLinks)
                            {
                                using (CultureContext cultureContext = new CultureContext(multiligualObjectLink.Culture, false))
                                {
                                    Member<object>.SetValueImplicitly(relResolver.FastFieldAccessor, ref memoryInstance, multiligualObjectLink.LinkedObject);
                                }
                            }
                        }
                    }
                    else
                        throw new Exception("Relation resolver isn't multilingual");

                    return;
                }
            }
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;

                if (relResolver.ValueTypePathContainsAttribute(attribute, pathIndex))
                {
                    if (structure.IsMultilingual(attribute))
                    {
                        valueSetted = false;
                        foreach (MultilingualObjectLink multilingualObjectLink in multilingualObjectLinks)
                        {
                            using (OOAdvantech.CultureContext cultureContext = new CultureContext(multilingualObjectLink.Culture, false))
                            {
                                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                                SetMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLink.LinkedObject, pathIndex, out valueSetted);
                                if (valueSetted)
                                {
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                }
                                else
                                    throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                            }
                        }
                        return;
                    }
                    else
                    {

                        //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                        object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                        SetMultilingualMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLinks, pathIndex, out valueSetted);
                        if (valueSetted)
                        {
                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                            return;
                        }
                    }
                }
            }
            throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");

        }



        /// <MetaDataID>{8cc29332-0bdb-423d-b6e1-13ad345ee949}</MetaDataID>
        internal protected void SetMemberValue(RelResolver relResolver, object value)
        {

            object memoryInstance = MemoryInstance;
            System.Type type = MemoryInstance.GetType();

            lock (relResolver)
            {
                if (relResolver.FieldInfo.DeclaringType == type || type.GetMetaData().IsSubclassOf(relResolver.FieldInfo.DeclaringType)) //if (Class.IsA(relResolver.AssociationEnd.Namespace as MetaDataRepository.Classifier))
                {
                    Member<object>.SetValueImplicitly(relResolver.FastFieldAccessor, ref memoryInstance, value);

                    //if (!relResolver.FieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                    //{
                    //    //if (relResolver.Multilingual)
                    //    //{
                    //    //    foreach (var multiligualObjectLink in value as System.Collections.Generic.List<PersistenceLayerRunTime.MultilingualObjectLink>)
                    //    //    {
                    //    //        using (CultureContext cultureContext = new CultureContext(multiligualObjectLink.Culture, false))
                    //    //        {
                    //    //            Member<object>.SetValueImplicitly(relResolver.FastFieldAccessor, ref memoryInstance, multiligualObjectLink.LinkedObject);
                    //    //        }
                    //    //    }
                    //    //}
                    //    //else
                    //    Member<object>.SetValueImplicitly(relResolver.FastFieldAccessor, ref memoryInstance, value);
                    //}
                }
                else
                {
                    foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                    {
                        if (!Class.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                            continue;

                        if (relResolver.ValueTypePathContainsAttribute(attribute))
                        {
                            //if (Class.IsMultilingual(attribute))
                            //{

                            //    List<MultilingualObjectLink> multilingualObjectLinks = value as List<MultilingualObjectLink>;
                            //    foreach (MultilingualObjectLink multilingualObjectLink in multilingualObjectLinks)
                            //    {
                            //        using (OOAdvantech.CultureContext cultureContext = new CultureContext(multilingualObjectLink.Culture, false))
                            //        {
                            //            bool valueSetted = false;
                            //            //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                            //            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                            //            object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                            //            SetMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, multilingualObjectLink.LinkedObject, 1, out valueSetted);
                            //            if (valueSetted)
                            //            {
                            //                Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                            //            }
                            //            else
                            //                throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                            //        }
                            //    }
                            //    return;
                            //}
                            //else
                            {
                                bool valueSetted = false;
                                //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                                AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                                object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                                SetMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, value, 1, out valueSetted);
                                if (valueSetted)
                                {
                                    Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                    return;
                                }
                            }
                        }

                    }
                    throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");

                }
                //relResolver._IsCompleteLoaded=true;
            }
        }


        /// <MetaDataID>{bf3019bf-2f95-4e57-aaf0-40a2db2c6b6f}</MetaDataID>
        void SetMemberValue(ref object memoryInstance, RelResolver relResolver, DotNetMetaDataRepository.Structure structure, object value, int pathIndex, out bool valueSetted)
        {

            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
            {
                if (associationEnd == relResolver.AssociationEnd)
                {
                    valueSetted = true;
                    if (memoryInstance == null)
                        throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                    else
                        Member<object>.SetValueImplicitly(relResolver.FastFieldAccessor, ref memoryInstance, value);


                    return;
                }
            }
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;

                if (relResolver.ValueTypePathContainsAttribute(attribute, pathIndex))
                {
                    //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                    AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                    object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                    SetMemberValue(ref owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, value, pathIndex, out valueSetted);
                    if (valueSetted)
                    {
                        Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                        return;
                    }
                }
            }
            throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");

        }





        /// <MetaDataID>{70042624-9354-49b1-b314-59ca73c09c80}</MetaDataID>
        internal void InitRelResolverMember(RelResolver relResolver)
        {
            System.Type type = MemoryInstance.GetType();
            if (relResolver.FieldInfo.DeclaringType == type || type.GetMetaData().IsSubclassOf(relResolver.FieldInfo.DeclaringType)) //if (Class.IsA(relResolver.AssociationEnd.Namespace as MetaDataRepository.Classifier))
            {
                Member<object>.InitValue(MemoryInstance, relResolver.FastFieldAccessor, relResolver.AssociationEnd, relResolver);
            }
            else
            {
                foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                {
                    if (!Class.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                        continue;
                    if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                    {
                        bool valueSetted = false;
                        //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                        object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                        InitRelResolverMember(owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, out valueSetted);
                        if (valueSetted)
                        {
                            object memoryInstance = MemoryInstance;
                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                            return;
                        }
                    }
                }
                throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
            }
        }

        /// <MetaDataID>{12cde471-e415-43d3-a258-8324d8d0a755}</MetaDataID>
        void InitRelResolverMember(object memoryInstance, RelResolver relResolver, DotNetMetaDataRepository.Structure structure, out bool valueSetted)
        {

            foreach (DotNetMetaDataRepository.AssociationEnd associationEnd in structure.GetAssociateRoles(true))
            {
                if (associationEnd == relResolver.AssociationEnd)
                {
                    valueSetted = true;
                    if (memoryInstance == null)
                        throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");
                    else
                        Member<object>.InitValue(memoryInstance, relResolver.FastFieldAccessor, relResolver.AssociationEnd);
                    return;
                }
            }
            foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
            {
                if (!structure.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                    continue;
                if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                {
                    //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                    AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);

                    object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                    InitRelResolverMember(owner, relResolver, attribute.Type as DotNetMetaDataRepository.Structure, out valueSetted);
                    if (valueSetted)
                    {
                        Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                        return;
                    }
                }
            }
            throw new System.Exception("System can't set value of '" + relResolver.AssociationEnd.Name + "'");

        }



        /// <MetaDataID>{0e6695e5-c3c6-4595-bf58-eb3d44dfb4cf}</MetaDataID>
        protected void InitAttributeMember(ValueOfAttribute valueOfAttribute)
        {
            try
            {
                if (Class.IsA(valueOfAttribute.Attribute.Owner))
                {
                    //valueOfAttribute.FieldInfo.SetValue(MemoryInstance, valueOfAttribute.Value);
                    Member<object>.InitValue(MemoryInstance, valueOfAttribute.FastFieldAccessor, valueOfAttribute.Attribute);
                }
                else
                {
                    foreach (DotNetMetaDataRepository.Attribute attribute in Class.GetAttributes(true))
                    {
                        if (!Class.IsPersistent(attribute))// || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                            continue;
                        if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                        {
                            bool valueSetted = false;
                            //System.Reflection.FieldInfo fieldInfo = Class.GetFieldMember(attribute);
                            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = Class.GetFastFieldAccessor(attribute);
                            //object owner = fieldInfo.GetValue(MemoryInstance);
                            object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, MemoryInstance);
                            InitAttributeMember(owner, valueOfAttribute, attribute.Type as DotNetMetaDataRepository.Structure, out valueSetted);
                            if (valueSetted)
                            {
                                var memoryInstance = MemoryInstance;
                                Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                                return;
                            }
                        }
                    }
                    throw new System.Exception("System can't set value of '" + valueOfAttribute.Attribute.Name + "'");
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
        }
        /// <MetaDataID>{74ae3fc2-cda8-4ac4-a7e1-0e3802d0b60a}</MetaDataID>
        protected void InitAttributeMember(object memoryInstance, ValueOfAttribute valueOfAttribute, DotNetMetaDataRepository.Structure structure, out bool valueSetted)
        {

            if (structure.IsA(valueOfAttribute.Attribute.Owner))
            {
                //valueOfAttribute.FieldInfo.SetValue(memoryInstance, valueOfAttribute.Value);
                Member<object>.InitValue(memoryInstance, valueOfAttribute.FastFieldAccessor, valueOfAttribute.Attribute);
                valueSetted = true;
                return;
            }
            else
            {
                foreach (DotNetMetaDataRepository.Attribute attribute in structure.GetAttributes(true))
                {
                    if (!structure.IsPersistent(attribute))// || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
                        continue;
                    if (attribute.Type is DotNetMetaDataRepository.Structure && (attribute.Type as DotNetMetaDataRepository.Structure).Persistent)
                    {
                        AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = structure.GetFastFieldAccessor(attribute);
                        object owner = Member<object>.GetValue(fastFieldAccessor.GetValue, memoryInstance);
                        InitAttributeMember(owner, valueOfAttribute, attribute.Type as DotNetMetaDataRepository.Structure, out valueSetted);
                        if (valueSetted)
                        {
                            Member<object>.SetValueImplicitly(fastFieldAccessor, ref memoryInstance, owner);
                            return;
                        }
                    }
                }

            }
            throw new System.Exception("System can't set value of '" + valueOfAttribute.Attribute.Name + "'");
        }





        /// <MetaDataID>{F79535D1-A7B0-4E24-997C-2F75B96D9820}</MetaDataID>
        /// <summary>
        /// For each persistent association end create relation resolver
        /// </summary>
        private void InitializeRelationshipResolvers()
        {

            Collections.Generic.Set<MetaDataRepository.AssociationEnd> AssociateRoles = Class.GetAssociateRoles(true);
            foreach (RelResolver mResolver in GetRelResolvers())
            {
                if (PersistentObjectID == null)//There isn't storage instance for this object yet.
                {
                    if (mResolver.AssociationEnd.Multiplicity.IsMany)
                    {
                        int count = mResolver.LoadedRelatedObjects.Count;
                    }
                    mResolver._IsCompleteLoaded = true;
                }
                RelResolvers.Add(mResolver);

                InitializeRelationshipResolver(mResolver);
            }
        }

        protected internal override void InitializeRelationshipResolverForValueType(MetaDataRepository.Attribute attribute, System.Globalization.CultureInfo culture)
        {
            foreach (RelResolver mResolver in GetRelResolvers())
            {
                if (mResolver.ValueTypePath.ToArray().ToList().Contains(attribute.Identity))
                {
                    if (mResolver.FieldInfo.FieldType == typeof(PersistenceLayer.ObjectContainer) || mResolver.FieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
                    {

                        var ss = mResolver.GetLinkedObjects("");

                        System.Collections.IDictionary multiligualObjectCollections = GetMemoryInstanceMemberValue(mResolver) as System.Collections.IDictionary;
                        PersistenceLayer.ObjectContainer objectCollection = null;
                        if (multiligualObjectCollections.Contains(culture))
                            objectCollection = multiligualObjectCollections[culture] as PersistenceLayer.ObjectContainer;

                        if (objectCollection == null)
                        {
                            objectCollection = AccessorBuilder.CreateInstance(mResolver.FieldInfo.FieldType) as PersistenceLayer.ObjectContainer;
                            SetMemberValue(mResolver, objectCollection);
                        }
                        OnMemoryObjectCollection theObjectContainer = GetObjectCollection(objectCollection) as PersistenceLayerRunTime.OnMemoryObjectCollection;
                        if (!(theObjectContainer as IMemberInitialization).Initialized)
                        {
                            if (objectCollection is OOAdvantech.Transactions.ITransactionalObject && OOAdvantech.Transactions.Transaction.Current != null)
                            {
                                bool mark = false;
                                foreach (Transactions.Transaction transaction in Transactions.ObjectStateTransition.GetTransaction(MemoryInstance))
                                {
                                    if (transaction == OOAdvantech.Transactions.Transaction.Current)
                                    {
                                        mark = true;
                                        break;
                                    }
                                }
                                if (mark)
                                    (objectCollection as OOAdvantech.Transactions.ITransactionalObject).MarkChanges(OOAdvantech.Transactions.Transaction.Current);
                            }

                            var associationEndRealization = this.Class.GetAssociationEndRealization(mResolver.AssociationEnd);
                            if (associationEndRealization != null)
                                (theObjectContainer as IMemberInitialization).SetMetadata(associationEndRealization);
                            else
                                (theObjectContainer as IMemberInitialization).SetMetadata(mResolver.AssociationEnd);
                        }
                    }

                }
            }
        }


        private void InitializeRelationshipResolver(RelResolver mResolver)
        {
            if (mResolver.FieldInfo.FieldType == typeof(PersistenceLayer.ObjectContainer) || mResolver.FieldInfo.FieldType.GetMetaData().IsSubclassOf(typeof(PersistenceLayer.ObjectContainer)))
            {

                PersistenceLayer.ObjectContainer objectCollection = GetMemoryInstanceMemberValue(mResolver) as PersistenceLayer.ObjectContainer;
                OnMemoryObjectCollection theObjectContainer = null;
                if (objectCollection == null)
                {
                    objectCollection = AccessorBuilder.CreateInstance(mResolver.FieldInfo.FieldType) as PersistenceLayer.ObjectContainer;
                    if (objectCollection is OOAdvantech.Transactions.ITransactionalObject && OOAdvantech.Transactions.Transaction.Current != null)
                    {
                        bool mark = false;
                        foreach (Transactions.Transaction transaction in Transactions.ObjectStateTransition.GetTransaction(MemoryInstance))
                        {
                            if (transaction == OOAdvantech.Transactions.Transaction.Current)
                            {
                                mark = true;
                                break;
                            }
                        }
                        if (mark)
                            (objectCollection as OOAdvantech.Transactions.ITransactionalObject).MarkChanges(OOAdvantech.Transactions.Transaction.Current);
                    }
                    theObjectContainer = GetObjectCollection(objectCollection) as PersistenceLayerRunTime.OnMemoryObjectCollection;
                    var associationEndRealization = this.Class.GetAssociationEndRealization(mResolver.AssociationEnd);
                    if (associationEndRealization != null)
                        (theObjectContainer as IMemberInitialization).SetMetadata(associationEndRealization);
                    else
                        (theObjectContainer as IMemberInitialization).SetMetadata(mResolver.AssociationEnd);
                }
                else
                    theObjectContainer = GetObjectCollection(objectCollection) as PersistenceLayerRunTime.OnMemoryObjectCollection;
                try
                {
                    //TODO: Να τσεκαριστή για την περιπτωση που η object cotntainer ειναι null ή που έχει objects στην onMemoryCollection
                    SetMemberValue(mResolver, objectCollection);
                }
                catch (System.Exception Exception)
                {
                    throw new System.Exception("The object container " + mResolver.FieldInfo.Name + " must be initialized at construction time.\n" + Exception.Message);
                }
                if (!((mResolver.FastFieldAccessor.MemberInfo.DeclaringType.GetMetaData().IsValueType && !mResolver.FastFieldAccessor.MemberInfo.DeclaringType.GetMetaData().IsPrimitive) && (mResolver.AssociationEnd.Namespace as MetaDataRepository.Structure).Persistent))
                {
                    //None valuetype collections
                    if (theObjectContainer.RelResolver != mResolver)
                        theObjectContainer.RelResolver = mResolver;
                }
            }
            else
            {
                if (Member<object>.IsMember(mResolver.FieldInfo.FieldType))
                    InitRelResolverMember(mResolver);
            }
        }

        /// <MetaDataID>{c7cbbad8-0871-4a6c-80a8-c8c7bc8ce0ac}</MetaDataID>
        internal protected StorageInstanceRef()
        {
        }
#if !DeviceDotNet
        /// <MetaDataID>{1006f3a8-6cfa-4ebe-9b5a-fc8b6bd89cf4}</MetaDataID>
        static System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        /// <exclude>Excluded</exclude>
        static int _Remoting_Identity_IDSeqNum = 0;
        /// <MetaDataID>{bb91f487-e2df-4d30-8ea1-7a8869b1a099}</MetaDataID>
        internal static int Remoting_Identity_GetNextSeqNum()
        {
            return System.Threading.Interlocked.Increment(ref _Remoting_Identity_IDSeqNum);
        }
#endif




        /// <MetaDataID>{2EF1044F-CD39-47FB-9C09-232C66860A57}</MetaDataID>
        public StorageInstanceRef(object memoryInstance, ObjectStorage activeStorage, MetaDataRepository.StorageCell storageCell, PersistenceLayer.ObjectID objectID, bool doubleStorageInstanceRefCheck = true)
        {
            if (memoryInstance == null)
                throw new System.ArgumentNullException("You can't create StorageInstanceRef with out memory instance");
            if (activeStorage == null)
                throw new System.ArgumentNullException("You can't create StorageInstanceRef with out Active storege session");

            _StorageInstanceSet = storageCell;

            if (doubleStorageInstanceRefCheck)
            {
                PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = GetStorageInstanceRef(memoryInstance) as StorageInstanceRef;

                if (storageInstanceRef != null)
                {
                    if (storageInstanceRef.ObjectStorage != activeStorage)
                        throw new System.Exception("The object is already stored in other than this storage");
                    else
                        throw new System.Exception("The object has already Storage instance reference");
                }
            }



            //Error prone να γίνει τεστ τι γινετε όταν δεν έχει η class του object δηλομένο ExtensionProperties field
            //καθώσ και όταν έχει δηλωθεί πάνω από δύο φορές στην ιεραρχία 


            _Class = MetaDataRepository.Classifier.GetClassifier(memoryInstance.GetType()) as OOAdvantech.DotNetMetaDataRepository.Class;

            //if (ObjectStateManagerLink.GetExtensionPropertiesField(memoryInstance.GetType()) != null)
            //    _MemoryInstanceStrongReference = memoryInstance;
            _MemoryInstance = memoryInstance;// new System.WeakReference(memoryInstance);

            DotNetMetaDataRepository.Assembly.InitObject(_MemoryInstance, null);


            _ObjectStorage = activeStorage;

            if (objectID != null)
                PersistentObjectID = objectID;


            InitializeRelationshipResolvers();

            Assign(memoryInstance);


            //if (objectID == null)
            //    ObjectActived();//new object in storage
            //else 

            foreach (ValueOfAttribute valueOfAttribute in GetPersistentAttributeMetaData())
            {
                ///TODO performance tuning
                if (Member<object>.IsMember(valueOfAttribute.FieldInfo.FieldType))
                    InitAttributeMember(valueOfAttribute);
            }


        }




        /// <summary></summary>
        /// <MetaDataID>{8fbe6aff-bf50-4de9-8c24-199a5f4bd8f9}</MetaDataID>
        ~StorageInstanceRef()
        {
            //TODO: ένα test case

            if (LifeTimeController != null)
                LifeTimeController.ObjectDestroyed(this);
            else
            {
                if (PersistentObjectID != null && !(this is OOAdvantech.PersistenceLayerRunTime.StorageInstanceValuePathRef))
                    throw new System.Exception("The object with ID " + PersistentObjectID.ToString() + " has lost the life time controller.");
            }
        }
        /// <exclude>Excluded</exclude>
        ClassMemoryInstanceCollection _LifeTimeController;
        /// <MetaDataID>{91BBB565-B7D0-4F4A-AB79-1C069B4CED14}</MetaDataID>
        /// <summary></summary>
        [Association("", Roles.RoleB, "7365ba7a-b52e-4e5e-8c34-fe678bd7a952")]
        [IgnoreErrorCheck]
        public ClassMemoryInstanceCollection LifeTimeController
        {
            get
            {
                return _LifeTimeController;
            }
            set
            {
                _LifeTimeController = value;
            }
        }


        // internal System.Reflection.FieldInfo GetFieldMember(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd)
        // {

        //     foreach (DotNetMetaDataRepository.AssociationEnd ownerClassAssociationEnd in Class.GetAssociateRoles(true))
        //     {
        //         if (ownerClassAssociationEnd == associationEnd)
        //             return _Class.GetFieldMember(associationEnd);
        //     }

        //     foreach (DotNetMetaDataRepository.Attribute attribute in _class.GetAttributes(true))
        //     {
        //         if (!_class.IsPersistent(attribute) || (attribute.Type is DotNetMetaDataRepository.Structure && !(attribute.Type as MetaDataRepository.Structure).Persistent))
        //             continue;
        //         if (attribute.Type is DotNetMetaDataRepository.Structure)
        //         {

        //             System.Reflection.FieldInfo fieldInfo = GetFieldMember(associationEnd, attribute.Type as DotNetMetaDataRepository.Structure);
        //             if (fieldInfo != null)
        //                 return fieldInfo;
        //         }
        //     }
        // }
        // System.Reflection.FieldInfo GetFieldMember(OOAdvantech.DotNetMetaDataRepository.AssociationEnd associationEnd, DotNetMetaDataRepository.Structure structure)
        // {
        //     foreach (DotNetMetaDataRepository.AssociationEnd ownerClassAssociationEnd in structure.GetAssociateRoles(true))
        //     {
        //         if (ownerClassAssociationEnd == associationEnd)
        //             return structure.GetFieldMember(associationEnd);
        //     }
        //}



        /// <MetaDataID>{c253a52b-77dc-4e2b-9fc4-842bdb5a875e}</MetaDataID>
        internal protected void InitializeRelatedObject(RelResolver relResolver, object relatedObject)
        {
            relResolver.InitializeRelatedObject(relatedObject);
            DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;
            if (!relResolver.IsCompleteLoaded || associationEnd.CollectionClassifier != null)
                return;

            System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> fieldValues = null;
            if (!StorageInstanceValues.TryGetValue(relResolver.ValueTypePath.ToString(), out fieldValues))
            {
                fieldValues = new Dictionary<System.Reflection.FieldInfo, object>();
                StorageInstanceValues[relResolver.ValueTypePath.ToString()] = fieldValues;
            }
            if (relResolver.Multilingual)
                fieldValues[relResolver.FieldInfo] = relResolver.RelatedObjectAsMultilingualObjectLinks;
            else
                fieldValues[relResolver.FieldInfo] = relResolver.RelatedObject;
            //  fieldValues[relResolver.FieldInfo] = relResolver.RelatedObject;

        }

        internal protected void InitializeRelatedObject(RelResolver relResolver, List<PersistenceLayerRunTime.MultilingualObjectLink> multiligualRelatedObjects)
        {
            relResolver.InitializeRelatedObject(multiligualRelatedObjects);
            DotNetMetaDataRepository.AssociationEnd associationEnd = relResolver.AssociationEnd as DotNetMetaDataRepository.AssociationEnd;
            if (!relResolver.IsCompleteLoaded || associationEnd.CollectionClassifier != null)
                return;

            System.Collections.Generic.Dictionary<System.Reflection.FieldInfo, object> fieldValues = null;
            if (!StorageInstanceValues.TryGetValue(relResolver.ValueTypePath.ToString(), out fieldValues))
            {
                fieldValues = new Dictionary<System.Reflection.FieldInfo, object>();
                StorageInstanceValues[relResolver.ValueTypePath.ToString()] = fieldValues;
            }
            fieldValues[relResolver.FieldInfo] = multiligualRelatedObjects;

        }





        /// <MetaDataID>{8488102b-0212-4b19-8b73-000dd7f590a7}</MetaDataID>
        public static List<StorageInstanceRef> GetNewObjectsUnderTransaction(MetaDataRepository.Classifier classifier)
        {
            List<StorageInstanceRef> storageInstanceRefs = new List<StorageInstanceRef>();
            if (TransactionContext.CurrentTransactionContext != null)
            {
                foreach (object memoryInstance in TransactionContext.CurrentTransactionContext.EnlistObjects)
                {

                    StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(memoryInstance) as StorageInstanceRef;
                    if (storageInstanceRef != null && storageInstanceRef.Class.IsA(classifier))
                    {
                        if (storageInstanceRef.PersistentObjectID == null)
                        {
                            if (!storageInstanceRefs.Contains(storageInstanceRef))
                                storageInstanceRefs.Add(storageInstanceRef);
                        }
                    }
                }
            }
            return storageInstanceRefs;
        }


    }
}


