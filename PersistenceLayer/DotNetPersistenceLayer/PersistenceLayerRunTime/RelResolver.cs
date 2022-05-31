namespace OOAdvantech.PersistenceLayerRunTime
{
    using System.Threading;
    using System.Reflection;
    using OOAdvantech.MetaDataRepository.ObjectQueryLanguage;
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;

    /// <MetaDataID>{FF859848-2F74-4A35-AAF7-F085B1BC26CE}</MetaDataID>
    /// <summary>Relation resolver is a class which manipulated the related objects of 
    /// owner object for a specific association. 
    /// It can load all or part of related objects, create a link with a candidate related object or 
    /// removes a link, with the production of corresponding commands. 
    /// It also reacts with different way in case where multiplicity is greater than one 
    /// and with different way when the multiplicity is less or equal to one. </summary>
    public abstract class RelResolver : System.Collections.IList
    {

        //�� Related objects ����� ����� ����� �� ��� ���� ����� ��� ����� ����� ��� ��� 
        //�� Related objects ��� ������� ���� Transaction �� �������������� ����� � class OnMemoryObjectCollection 
        //���� ��������� ��� � ����� ����� many ����� � class StorageInstanceRef  ���� ��������� ��� � ����� ����� zero


        protected System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, OOAdvantech.Collections.Generic.List<object>> MultilingualLoadedRelatedObjects;

        OOAdvantech.Collections.Generic.List<object> _InternalLoadedRelatedObjects;
        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.List<object> InternalLoadedRelatedObjects
        {
            get
            {
                if (Multilingual)
                {

                    OOAdvantech.Collections.Generic.List<object> loadedRelatedObjects = null;
                    if (MultilingualLoadedRelatedObjects.TryGetValue(OOAdvantech.CultureContext.CurrentNeutralCultureInfo, out loadedRelatedObjects))
                    {
                        return loadedRelatedObjects;
                    }
                    else
                        return null;
                }
                else
                {
                    return _InternalLoadedRelatedObjects;
                }

            }
            set
            {

                if (Multilingual)
                {

                    MultilingualLoadedRelatedObjects[OOAdvantech.CultureContext.CurrentNeutralCultureInfo] = value;
                }
                else
                {
                    _InternalLoadedRelatedObjects = value;
                }
            }
        }


        /// <summary>This collection keeps the relate objects which have already loaded. 
        /// There are cases where the collection contains part of related objects not all. </summary>
        /// <exception cref="System.NotSupportedException">
        /// If the <see cref="OOAdvantech.MetaDataRepository.AssociationEnd.Multiplicity">
        /// multiplicity of association end </see>
        ///  isn't many. </exception>
        /// <MetaDataID>{C83B9160-660D-4B8B-A7BB-7274DF66D3B8}</MetaDataID>
        internal OOAdvantech.Collections.Generic.List<object> LoadedRelatedObjects
        {
            get
            {
                /// If an object added when code runs under open transaction the transaction system 
                /// can undo the changes of collection when transaction fails. 
                /// If system ask from the collection to retrieve the related object the collection will 
                /// return all objects. 
                /// It will return also the temporary object of transaction. </summary>

                if (!AssociationEnd.Multiplicity.IsMany)
                    throw new System.NotSupportedException("This property doesn't supported when the multiplicity of association end is zero or one");

                if (InternalLoadedRelatedObjects == null)
                    InternalLoadedRelatedObjects = new OOAdvantech.Collections.Generic.List<object>();
                return InternalLoadedRelatedObjects;
            }

        }

        object _InternalRelatedObject;
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2C73C009-FD19-4E83-B8AD-230BD5608356}</MetaDataID>
        protected object InternalRelatedObject
        {
            get
            {
                if (Multilingual)
                {
                    lock (MultilingualRelatedObject)
                    {
                        if (MultilingualRelatedObject.TryGetValue(OOAdvantech.CultureContext.CurrentNeutralCultureInfo, out object relatedObject))
                            return relatedObject;
                        else
                            return null;
                    }
                }
                else
                {
                    return _InternalRelatedObject;
                }
            }
            set
            {
                if (Multilingual)
                {
                    if (value is System.Collections.Generic.List<MultilingualObjectLink>)
                        throw new System.ArgumentException("Invalid related object type ");

                    lock (MultilingualRelatedObject)
                    {
                        if (value == null)
                            MultilingualRelatedObject.Remove(OOAdvantech.CultureContext.CurrentNeutralCultureInfo);
                        else
                            MultilingualRelatedObject[OOAdvantech.CultureContext.CurrentNeutralCultureInfo] = value;
                    }
                }
                else
                {
                    _InternalRelatedObject = value;
                }
            }
        }

        /// <summary>This property defines the related object in case 
        /// where the association end multiplicity is zero or one. 
        /// In other cases raise no support exception. </summary>
        /// <exception cref="System.NotSupportedException">
        /// If the <see cref="OOAdvantech.MetaDataRepository.AssociationEnd.Multiplicity">
        /// multiplicity of association end 
        /// </see> 
        /// is many. 
        /// </exception>
        /// <MetaDataID>{3FC0996E-8FCE-4ED6-A5F7-C6FD69B6C276}</MetaDataID>
        public object RelatedObject
        {
            get
            {
                if (AssociationEnd.Multiplicity.IsMany)
                    throw new System.NotSupportedException("This property doesn't supported, because the multiplicity of association end '" + AssociationEnd.GetOtherEnd().Specification.FullName + "." + AssociationEnd.Name + "' is many");
                if (!IsCompleteLoaded)
                    CompleteLoad();

                return InternalRelatedObject;
            }
            set
            {

                InternalRelatedObject = value;
            }
        }

        /// <MetaDataID>{ec022811-cc8b-41c5-8a94-fc462f2d87fd}</MetaDataID>
        MetaDataRepository.ValueTypePath _ValueTypePath;
        /// <MetaDataID>{319d4079-9eac-4b04-9df6-8cb2630bd22c}</MetaDataID>
        public MetaDataRepository.ValueTypePath ValueTypePath
        {
            get
            {
                if (Owner is StorageInstanceValuePathRef)
                    return (Owner as StorageInstanceValuePathRef).ValueTypePath;
                else
                {
                    if (_ValueTypePath == null)
                        _ValueTypePath = new OOAdvantech.MetaDataRepository.ValueTypePath();
                    return _ValueTypePath;
                }
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2BCDFB4C-971A-4364-8A58-EDF89E54C324}</MetaDataID>
        private System.Reflection.FieldInfo _FieldInfo;
        /// <MetaDataID>{5614FE19-B90C-4598-9B83-EB83FE079757}</MetaDataID>
        /// <summary>This property define the field of class which implement the AssocionEnd. </summary>
        public System.Reflection.FieldInfo FieldInfo
        {
            get
            {
                return _FieldInfo;
            }
        }



        /// <exclude>Excluded</exclude>
        private AccessorBuilder.FieldPropertyAccessor _FastFieldAccessor;
        public readonly bool Multilingual;

        /// <MetaDataID>{b8695728-4ad8-4e60-9fd1-d3d094babdf7}</MetaDataID>
        public AccessorBuilder.FieldPropertyAccessor FastFieldAccessor
        {
            get
            {
                return _FastFieldAccessor;
            }
        }


        protected internal bool _IsCompleteLoaded;
        /// <summary>It is a flag which tell us when all related objects are loaded. 
        /// If the value of flag is false then a part or none from relate objects, 
        /// are loaded. </summary>
        /// <MetaDataID>{8B3FFEB4-741B-4ECC-8CFE-151C37F200E0}</MetaDataID>
        virtual public bool IsCompleteLoaded
        {
            get
            {
                return _IsCompleteLoaded;
            }
        }


        /// <MetaDataID>{0385A04E-40C2-40B5-8654-B1C87C913550}</MetaDataID>
        /// <summary>This method loads all related objects if there aren't loaded yet. </summary>
        internal protected void CompleteLoad()
        {
            lock (this)
            {
                if (IsCompleteLoaded)
                    return;
                Load("");
                _IsCompleteLoaded = true;

            }
        }


        /// <MetaDataID>{7DBCEEFA-55EE-4AF4-95AD-DDBEEBF440CA}</MetaDataID>
        /// <summary>This method consume the event ObjectDeleted StorageInstanceRef
        /// and clear the reference of MemoryInstance to the deleted object </summary>
        public void OnObjectDeleted(object sender)
        {
            //TODO: �� ������ ���� �� ��������� ��� �� ����������� ��� �������� ����� �������������
            if (AssociationEnd.Navigable)
                Owner.ClearObjectsLink(new AssociationEndAgent(AssociationEnd), sender, true);
        }





        /// <MetaDataID>{2393A9BA-5012-48ED-82D1-7F4C0F97D87A}</MetaDataID>
        /// <summary>The RelResolver constructor initiates the object during the object construction. </summary>
        /// <param name="owner">Defines the owner object. For this object and the association end the relation resolver
        /// load related objects. </param>
        /// <param name="associationEnd">This parameter defines the type of relationship with related object. </param>
        /// <exception cref="System.ArgumentException">If any of two parameters is null.
        /// </exception>
        public RelResolver(StorageInstanceRef owner, DotNetMetaDataRepository.AssociationEnd associationEnd, OOAdvantech.AccessorBuilder.FieldPropertyAccessor fastFieldAccessor)
        {
            if (owner == null)
                throw new System.ArgumentNullException("The owner parameter  must be not null");
            if (associationEnd == null)
                throw new System.ArgumentNullException("The associationEnd parameter  must be not null");
            if (fastFieldAccessor == null)
                throw new System.ArgumentNullException("The fastFieldAccessor parameter  must be not null");

            Owner = owner;
            AssociationEnd = associationEnd;
            _FieldInfo = fastFieldAccessor.MemberInfo as FieldInfo;
            _FastFieldAccessor = fastFieldAccessor;
            Multilingual = Owner.Class.IsMultilingual(associationEnd);
            if (ValueTypePath.Multilingual)
                Multilingual = true;
            if (Multilingual)
                MultilingualLoadedRelatedObjects = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, Collections.Generic.List<object>>();
        }
        /// <MetaDataID>{42299D6A-6AF3-4644-ACFB-361E56F09A8D}</MetaDataID>
        public DotNetMetaDataRepository.AssociationEnd AssociationEnd;
        /// <MetaDataID>{8524B2FF-D9B4-43FD-9EF1-A77A8059B038}</MetaDataID>
        public abstract System.Collections.Generic.List<object> GetLinkedStorageInstanceRefs(bool OperativeObjectOnly);

        public System.Collections.Generic.List<object> GetLinkedStorageInstanceRefsUnderTransaction(bool OperativeObjectOnly)
        {
            System.Collections.Generic.List<object> objects = new System.Collections.Generic.List<object>();

            if (AssociationEnd.Multiplicity.IsMany)
            {
                if (Multilingual)
                {
                    IMultilingual relatedObjects = FastFieldAccessor.GetValue(Owner.MemoryInstance) as IMultilingual;
                    foreach (System.Collections.DictionaryEntry relatedObjectEntry in relatedObjects.Values)
                    {
                        foreach (var relatedObject in relatedObjectEntry.Value as System.Collections.IEnumerable)
                        {
                            PersistenceLayerRunTime.MultilingualObjectLink multilingualObjectLink = new PersistenceLayerRunTime.MultilingualObjectLink();
                            multilingualObjectLink.Culture = relatedObjectEntry.Key as CultureInfo;
                            multilingualObjectLink.LinkedObject = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(relatedObject);
                            objects.Add(multilingualObjectLink);
                        }
                    }

                    return objects;
                }
                else
                {
                    System.Collections.IEnumerable relatedObjects = FastFieldAccessor.GetValue(Owner.MemoryInstance) as System.Collections.IEnumerable;
                    if (relatedObjects != null)
                    {
                        foreach (var _obj in relatedObjects)
                            objects.Add(_obj);
                    }
                }
            }
            else
            {
                var relatedObject = Owner.GetMemoryInstanceMemberValue(this);
                //FastFieldAccessor.GetValue(Owner.MemoryInstance);

                if (Multilingual)
                {
                    foreach (System.Collections.DictionaryEntry relatedObjectEntry in relatedObject as System.Collections.IDictionary)
                    {

                        if (relatedObjectEntry.Value != null)
                        {
                            PersistenceLayerRunTime.MultilingualObjectLink multilingualObjectLink = new PersistenceLayerRunTime.MultilingualObjectLink();
                            multilingualObjectLink.Culture = relatedObjectEntry.Key as CultureInfo;
                            multilingualObjectLink.LinkedObject = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(relatedObjectEntry.Value);
                            objects.Add(multilingualObjectLink);
                        }
                    }
                    return objects;

                }
                else if (relatedObject is IMember)
                {
                    if ((relatedObject as IMember).Value != null)
                        objects.Add((relatedObject as IMember).Value);
                }
                else if (relatedObject != null)
                    objects.Add(relatedObject);
            }


            System.Collections.Generic.List<object> StorageInstanceRefs = new System.Collections.Generic.List<object>(objects.Count);
            foreach (object _objcet in objects)
                StorageInstanceRefs.Add(PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(_objcet));
            return StorageInstanceRefs;
        }
        public System.Collections.Generic.List<object> GetLinkedObjectsInstorage(string criterion)
        {

            if (Transactions.Transaction.Current != null)
            {
                using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                {
                    if (Owner.PersistentObjectID == null)
                    {
                        System.Collections.Generic.List<object> ObjectCollection = new System.Collections.Generic.List<object>();
                        stateTransition.Consistent = true;
                        return ObjectCollection;
                    }
                    else
                    {
                        ObjectQuery objectQuery = new ObjectQuery(this);
                        System.Collections.Generic.List<object> ObjectCollection = objectQuery.RelatedObjects;
                        stateTransition.Consistent = true;
                        return ObjectCollection;
                    }
                }
            }
            else
            {
                if (Owner.PersistentObjectID == null)
                {
                    System.Collections.Generic.List<object> ObjectCollection = new System.Collections.Generic.List<object>();
                    return ObjectCollection;
                }
                else
                {

                    ObjectQuery objectQuery = new ObjectQuery(this);
                    System.Collections.Generic.List<object> ObjectCollection = objectQuery.RelatedObjects;
                    return ObjectCollection;
                }
            }
        }

        /// <MetaDataID>{50C04C86-F330-488B-BB91-16889B13B47D}</MetaDataID>
        public virtual System.Collections.Generic.List<object> GetLinkedObjects(string criterion)
        {


            if (Transactions.Transaction.Current != null)
            {
                using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                {
                    if (Owner.PersistentObjectID == null)
                    {
                        System.Collections.Generic.List<object> ObjectCollection = new System.Collections.Generic.List<object>();
                        stateTransition.Consistent = true;
                        return ObjectCollection;
                    }
                    else
                    {
                        ObjectQuery objectQuery = new ObjectQuery(this);
                        System.Collections.Generic.List<object> ObjectCollection = objectQuery.RelatedObjects;
                        stateTransition.Consistent = true;
                        return ObjectCollection;
                    }
                }
            }
            else
            {
                if (Owner.PersistentObjectID == null)
                {
                    System.Collections.Generic.List<object> ObjectCollection = new System.Collections.Generic.List<object>();
                    return ObjectCollection;
                }
                else
                {

                    ObjectQuery objectQuery = new ObjectQuery(this);
                    System.Collections.Generic.List<object> ObjectCollection = objectQuery.RelatedObjects;
                    return ObjectCollection;
                }
            }
        }


        /// <MetaDataID>{ABF9F91B-6644-4672-B26C-D134194301D1}</MetaDataID>
        internal protected System.Collections.Generic.List<object> Load(string criterion)
        {
            if(GetType().FullName == "OOAdvantech.WindowsAzureTablesPersistenceRunTime.RelResolver"&& AssociationEnd.Name != "Owner"&& AssociationEnd.Name != "Storages")
            {
                System.Diagnostics.Debug.WriteLine("RELATION RESOLVER " + AssociationEnd.FullName);
            }
            //TODO: �� ������� testcase ���� �� ��������� ����� objects ��� ������ �� 
            //�������� �� metadata �� assemply registration �� �� ������ � ReferentialIntegrity ��� ��� �����
            bool subscribeForObjectDeletion = false;
            //TODO: ��� �� type ��� association end ����� interface ���� ������� ��������  
            if (!(Owner.Class.HasReferentialIntegrity(AssociationEnd))	//it hasn't ReferentialIntegrity
                && !(AssociationEnd.Navigable && AssociationEnd.GetOtherEnd().Navigable))		//the association doesn't double navigable
            {
                subscribeForObjectDeletion = true;
            }
            System.Collections.Generic.List<object> objects = GetLinkedObjects(criterion);
            if (AssociationEnd.Multiplicity.IsMany)
            {
                if (LoadedRelatedObjects.Count == 0)
                {

                    if (Multilingual)
                    {
                        foreach (var langugeObjects in (from multiligualObjLink in objects.OfType<MultilingualObjectLink>()
                                                        group multiligualObjLink by multiligualObjLink.Culture into languageGroup
                                                        select new
                                                        {
                                                            key = languageGroup.Key,
                                                            loadedObjects = (from languageObject in languageGroup
                                                                             select languageObject.LinkedObject).Distinct().ToList()
                                                        }).ToList())
                        {

                            using (var cultureContext = new CultureContext(langugeObjects.key, false))
                            {
                                LoadedRelatedObjects.AddRange(langugeObjects.loadedObjects);
                            }

                        }

                    }
                    else
                        LoadedRelatedObjects.AddRange(objects);
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
                else
                {
                    foreach (object _object in objects)
                    {
                        if (!LoadedRelatedObjects.Contains(_object))
                        {
                            LoadedRelatedObjects.Add(_object);
                            if (subscribeForObjectDeletion)
                            {
                                //subscribe 
                                PersistenceLayerRunTime.StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(_object) as PersistenceLayerRunTime.StorageInstanceRef;
                                storageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(OnObjectDeleted);
                            }
                        }
                    }
                }
            }
            else
            {
                if (criterion == null || criterion.Trim().Length == 0)
                {
                    if (Multilingual)
                    {
                        foreach (var multiligualObjLink in objects.OfType<MultilingualObjectLink>())
                        {
                            using (var cultureContext = new CultureContext(multiligualObjLink.Culture, false))
                            {
                                InternalRelatedObject = multiligualObjLink.LinkedObject;
                            }
                        }
                    }
                    else
                    {
                        if (objects.Count > 0)
                            InternalRelatedObject = objects[0];
                        else
                            InternalRelatedObject = null;
                    }
                }
                else
                {
                    if (!AssociationEnd.Multiplicity.IsMany)
                        throw new System.NotSupportedException("Criterion doesn't supported when the multiplicity of association end is zero or one");

                }

            }
            if (criterion == null || criterion.Trim().Length == 0)
                _IsCompleteLoaded = true;
            return objects;
        }


        /// <summary>This method check if the object of parameter is one of related objects </summary>
        /// <param name="obj">The object of parameter obj 
        /// is the object which checks for the existence 
        /// among the related objects. </param>
        /// <MetaDataID>{E9BFEF90-AC5A-48B1-BDAC-297A1EF19F62}</MetaDataID>
        public virtual bool Contains(object obj)
        {
            if (obj == null)
                return false;
            if (AssociationEnd.Multiplicity.IsMany)
            {
                if (LoadedRelatedObjects.Contains(obj))
                    return true;
                if (!IsCompleteLoaded)
                {
                    StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(obj) as StorageInstanceRef;
                    if (storageInstanceRef == null || storageInstanceRef.PersistentObjectID == null)
                        return false;

                    if (Transactions.Transaction.Current != null)
                    {
                        using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                        {
                            ObjectQuery objectQuery = new ObjectQuery(this);
                            bool contains = false;
                            contains = objectQuery.Contains(obj);
                            stateTransition.Consistent = true;
                            return contains;
                        }
                    }
                    else
                    {
                        ObjectQuery objectQuery = new ObjectQuery(this);
                        return objectQuery.Contains(obj);
                    }
                }
                else
                    return false;
            }
            else
            {
                return RelatedObject == obj;
            }


            //if (obj == null)
            //    return false;
            //if (AssociationEnd.Multiplicity.IsMany)
            //{
            //    if (LoadedRelatedObjects.Contains(obj))
            //        return true;
            //    if (!IsCompleteLoaded)
            //    {
            //        StorageInstanceRef storageInstanceRef = StorageInstanceRef.GetStorageInstanceRef(obj) as StorageInstanceRef;
            //        if (storageInstanceRef == null || storageInstanceRef.ObjectID == null)
            //            return false;
            //        Load("");
            //        //Load(AssociationEnd.Name+" = "+storageInstanceRef.ObjectID.ToString());
            //        return LoadedRelatedObjects.Contains(obj);
            //    }
            //    else
            //        return false;
            //}
            //else
            //{
            //    return RelatedObject == obj;
            //}
        }



        /// <MetaDataID>{1a8d5a7c-f718-4f75-9b6f-f22e81c4ad65}</MetaDataID>
        protected StorageInstanceAgent OwnerStorageInstanceAgent = null;


        /// <summary>This method link the owner object with 
        /// the object of parameter if there isn�t link. 
        /// Also add the object to the related objects. </summary>
        /// <param name="relatedObject">This parameter defines the related object 
        /// which will be linked. </param>
        /// <param name="changeApplied">At the end of method call this parameter indicate 
        /// if the system produce command to save the link 
        /// or the link between the owner and related object already exist. </param>
        /// <param name="index">
        /// The index of related object, has value only in associations where are defined as indexer
        /// </param>
        /// <MetaDataID>{D23EBA63-9363-49B9-8AE1-E3BD18BA39DC}</MetaDataID>
        public void LinkObject(object relatedObject, int index, out bool changeApplied)
        {


            if (OwnerStorageInstanceAgent == null)
                OwnerStorageInstanceAgent = new StorageInstanceAgent(Owner);
            //TODO �� ������ �� ��������� exception ���� ������� ����������� �� ����� 
            //link �� ��� object ��� ���� ������� ������������.
            #region preconditiom check

            changeApplied = false;
            if (relatedObject == null)
                throw new System.Exception("You can't link an object with nothing");

            StorageInstanceAgent relatedStorageInstance = StorageInstanceRef.GetStorageInstanceAgent(relatedObject);
            if (relatedStorageInstance == null)
            {
                if (this.Owner.Class.AllowTransient(this.AssociationEnd))
                    return;

                throw new System.Exception("the object " + relatedObject.ToString() + " at " + Owner.Class.FullName + "." + AssociationEnd.Name + " is transient.");
            }

            if (AssociationEnd.Multiplicity.IsMany)
            {

                if (!AssociationEnd.Indexer && Contains(relatedObject))
                    return;
                else if (AssociationEnd.Indexer && index != -1 && LoadedRelatedObjects.IndexOf(relatedObject) == index)
                    return;

                if (AssociationEnd.Indexer && Contains(relatedObject) && index != -1)
                {
                    RefreshLinkedObjectIndex(relatedObject, index);
                    return;
                }

            }
            else
            {
                if (RelatedObject != null && RelatedObject != relatedObject)
                    throw new System.Exception("You must remove the old link first. Object : " + Owner.MemoryInstance.ToString() + " Assocition end : " + AssociationEnd.FullName);
            }
            #endregion

            if (AssociationEnd.Association.LinkClass == null)
            {
                #region Single relationship.
                StorageInstanceAgent roleA, roleB, collectionOwner, relationObject = null;

                if (AssociationEnd.IsRoleA)
                {
                    roleA = relatedStorageInstance;
                    roleB = OwnerStorageInstanceAgent;
                    collectionOwner = roleB;
                }
                else
                {
                    roleB = relatedStorageInstance;
                    roleA = OwnerStorageInstanceAgent;
                    collectionOwner = roleA;
                }
                PersistenceLayerRunTime.ObjectStorage roleAStorage, roleBStorage;
                roleAStorage = roleA.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
                roleBStorage = roleB.ObjectStorage as PersistenceLayerRunTime.ObjectStorage;
                {
                    if (roleAStorage == roleBStorage)
                    {
                        roleAStorage.CreateLinkCommand(roleA, roleB, relationObject, new AssociationEndAgent(AssociationEnd), index);
                        changeApplied = true;
                    }
                    else
                    {
                        if (Multilingual)
                            throw new System.NotSupportedException("the multilingual link doesn't supported");

                        if (AssociationEnd.Association.RoleB.Navigable)
                            roleAStorage.CreateLinkCommand(roleA, roleB, relationObject, new AssociationEndAgent(AssociationEnd), index);
                        if (AssociationEnd.Association.RoleA.Navigable)
                            roleBStorage.CreateLinkCommand(roleA, roleB, relationObject, new AssociationEndAgent(AssociationEnd), index);
                        changeApplied = true;
                    }
                }
                //else
                //{

                //}
                #endregion
            }
            else
            {
                //Error prone test case
                #region Relationship with relation object.
                if (!relatedStorageInstance.Class.IsA(AssociationEnd.Association.LinkClass))
                    throw new System.Exception("The link object at " + Owner.Class.FullName + "." + AssociationEnd.Name + " isn,t type of  " + AssociationEnd.Association.LinkClass.FullName + ".");

                // ������� �� ���������� field ��� relation Object
                System.Reflection.FieldInfo roleAField, roleBField = null;
                AccessorBuilder.FieldPropertyAccessor roleAFastFieldAccessor, roleBFastFieldAccessor;
                roleAField = (relatedStorageInstance.Class as DotNetMetaDataRepository.Class).LinkClassRoleAField;
                roleBField = (relatedStorageInstance.Class as DotNetMetaDataRepository.Class).LinkClassRoleBField;

                roleAFastFieldAccessor = (relatedStorageInstance.Class as DotNetMetaDataRepository.Class).LinkClassRoleAFastFieldAccessor;
                roleBFastFieldAccessor = (relatedStorageInstance.Class as DotNetMetaDataRepository.Class).LinkClassRoleBFastFieldAccessor;

                if (AssociationEnd.IsRoleA)
                {
                    object roleBFieldValue = Member<object>.GetValue(roleBFastFieldAccessor.GetValue, relatedObject);
                    if (roleBFieldValue != null && roleBFieldValue != Owner.MemoryInstance)
                        throw new System.Exception("Wrong value in " + roleBField.DeclaringType.FullName + "." + roleBField.Name);//Error Prone ���� ���� ����������� ������ ���������

                    //Sets relation object RoleB 
                    if (roleBFieldValue == null)
                        Member<object>.SetValueImplicitly(roleBFastFieldAccessor, ref relatedObject, Owner.MemoryInstance);
                    object roleAFieldValue = Member<object>.GetValue(roleAFastFieldAccessor.GetValue, relatedObject);
                    if (roleAFieldValue == null)
                        throw new System.Exception("Wrong value in " + roleAField.DeclaringType.FullName + "." + roleAField.Name);
                }
                else
                {

                    object roleAFieldValue = Member<object>.GetValue(roleAFastFieldAccessor.GetValue, relatedObject);
                    if (roleAFieldValue != null && roleAFieldValue != Owner.MemoryInstance)
                        throw new System.Exception("Wrong value in " + roleAField.DeclaringType.FullName + "." + roleAField.Name);//Error Prone ���� ���� ����������� ������ ���������
                    //Sets relation object RoleB 
                    if (roleAFieldValue == null)
                        Member<object>.SetValueImplicitly(roleAFastFieldAccessor, ref relatedObject, Owner.MemoryInstance);
                    object roleBFieldValue = Member<object>.GetValue(roleBFastFieldAccessor.GetValue, relatedObject);
                    if (roleBFieldValue == null)
                        throw new System.Exception("Wrong value in " + roleBField.DeclaringType.FullName + "." + roleBField.Name);

                }
                #endregion

            }
            if (AssociationEnd.Multiplicity.IsMany)
            {
                //_Count = -1;
                //if(index==-1)
                //    LoadedRelatedObjects.Add(relatedObject);
                //else
                //    LoadedRelatedObjects.Insert(index,relatedObject);


            }
            else
            {


                if (OOAdvantech.Transactions.Transaction.Current != null)
                {
                    if (TransactionsOriginalRelatedObject == null)
                        TransactionsOriginalRelatedObject = new System.Collections.Generic.Dictionary<string, object>();
                    TransactionsOriginalRelatedObject[OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri] = InternalRelatedObject;
                    OOAdvantech.Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(Current_TransactionCompleted);


                }
                InternalRelatedObject = relatedObject;
            }
            changeApplied = true;

            #region Consume ObjectDeleted event in case where needed to clear an object reference
            //TODO: ��� �� type ��� association end ����� interface ���� ������� ��������  
            if (!Owner.Class.HasReferentialIntegrity(AssociationEnd)	//it hasn't ReferentialIntegrity
                && !(AssociationEnd.Navigable && AssociationEnd.GetOtherEnd().Navigable))//the association doesn't double navigable
            {
                relatedStorageInstance.RealStorageInstanceRef.ObjectDeleted += new PersistenceLayerRunTime.ObjectDeleted(OnObjectDeleted);
            }
            #endregion


        }

        /// <summary>
        /// Creates command to save new index
        /// </summary>
        /// <param name="relatedObject">
        /// Defines the related object who has change index
        /// </param>
        /// <param name="index">
        /// Defines the new index
        /// </param>
        internal void RefreshLinkedObjectIndex(object relatedObject, int index)
        {
            StorageInstanceAgent relatedStorageInstance = StorageInstanceRef.GetStorageInstanceAgent(relatedObject);
            StorageInstanceAgent roleA = null;
            StorageInstanceAgent roleB = null;
            if (AssociationEnd.IsRoleA)
            {
                roleA = relatedStorageInstance;
                roleB = OwnerStorageInstanceAgent;
            }
            else
            {
                roleB = relatedStorageInstance;
                roleA = OwnerStorageInstanceAgent;
            }
            string cmdIdentity = Commands.UpdateLinkIndexCommand.GetIdentity(OwnerStorageInstanceAgent.ObjectStorage, roleA, roleB, AssociationEnd.Association as DotNetMetaDataRepository.Association);
            PersistenceLayerRunTime.Commands.Command command = null;
            if (PersistenceLayerRunTime.TransactionContext.CurrentTransactionContext.EnlistedCommands.TryGetValue(cmdIdentity, out command))
            {
                if (AssociationEnd.IsRoleA)
                    (command as Commands.UpdateLinkIndexCommand).RoleAIndex = index;
                else
                    (command as Commands.UpdateLinkIndexCommand).RoleBIndex = index;
            }
            else
                OwnerStorageInstanceAgent.ObjectStorage.CreateUpdateLinkIndexCommand(roleA, roleB, null, new AssociationEndAgent(AssociationEnd), index);

        }
        /// <MetaDataID>{cb6833ce-3e3d-4fcd-885e-6d3092256e96}</MetaDataID>
        void Current_TransactionCompleted(OOAdvantech.Transactions.Transaction transaction)
        {
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(Current_TransactionCompleted);
            if (TransactionsOriginalRelatedObject != null)
                TransactionsOriginalRelatedObject.Remove(transaction.LocalTransactionUri);

        }
        /// <MetaDataID>{7ccfa5bf-3073-4a51-b964-16d696104e0c}</MetaDataID>
        public object OriginalRelatedObject
        {
            get
            {
                if (TransactionsOriginalRelatedObject == null ||
                    OOAdvantech.Transactions.Transaction.Current == null ||
                    !TransactionsOriginalRelatedObject.ContainsKey(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri))
                    return InternalRelatedObject;
                else
                    return TransactionsOriginalRelatedObject[OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri];

            }
        }
        /// <MetaDataID>{916bcdfd-4a03-4cc5-895a-3760d01386cd}</MetaDataID>
        System.Collections.Generic.Dictionary<string, object> TransactionsOriginalRelatedObject;


        /// <summary>This method removes link between the owner object and 
        /// the object of parameter if there is link. 
        /// Also remove the object from loaded related objects. </summary>
        /// <param name="relatedObject">This parameter defines the related object 
        /// which will be removed. </param>
        /// <param name="changeApplied">At the end of method call this parameter indicate 
        /// the production of command from system to save the remove of link 
        /// or the absence of link with the related object. </param>
        /// <MetaDataID>{F7C03837-246E-4A83-9BE7-264EB7EAD1D5}</MetaDataID>
        public void UnLinkObject(object relatedObject, int index, out bool changeApplied)
        {
            #region preconditiom check
            changeApplied = false;
            if (relatedObject == null)
                throw new System.Exception("There isn't link with null object");

            StorageInstanceAgent relatedStorageInstance = StorageInstanceRef.GetStorageInstanceAgent(relatedObject);
            if (relatedStorageInstance == null)
            {

                if (InternalRelatedObject == relatedObject)
                {
                    InternalRelatedObject = null;
                    changeApplied = true;
                }
                //if (this.Owner.Class.AllowTransient(this.AssociationEnd))
                //    return;

                //throw new System.Exception("the object " + relatedObject.ToString() + " at " + Owner.Class.FullName + "." + AssociationEnd.Name + " is transient.");
                return;
            }


            //if (AssociationEnd.Multiplicity.IsMany)
            //{
            //    if (!Contains(relatedObject))
            //    {
            //        if (Owner.Class.IsCascadeDelete(AssociationEnd))
            //            ObjectStorage.DeleteObject(relatedObject, OOAdvantech.PersistenceLayer.DeleteOptions.TryToDelete);

            //        changeApplied = false;
            //        return;
            //    }
            //}
            //else
            if (!AssociationEnd.Multiplicity.IsMany)
            {
                if (RelatedObject != relatedObject)
                    throw new System.Exception("There isn't Link");
            }
            #endregion


            if (AssociationEnd.Association.LinkClass == null)
            {
                #region Single relationship.
                StorageInstanceAgent roleA, roleB, collectionOwner, relationObject = null;
                if (AssociationEnd.IsRoleA)
                {
                    roleA = relatedStorageInstance;
                    roleB = new StorageInstanceAgent(Owner);
                    collectionOwner = roleB;
                }
                else
                {
                    roleB = relatedStorageInstance;
                    roleA = new StorageInstanceAgent(Owner);
                    collectionOwner = roleA;
                }
                ObjectStorage roleAStorage, roleBStorage;
                roleAStorage = roleA.ObjectStorage;
                roleBStorage = roleB.ObjectStorage;


                //if (!TransactionContext.CurrentTransactionContext.ContainCommand(Commands.UnLinkObjectsCommand.GetIdentity(roleA, roleB, AssociationEnd.Association as DotNetMetaDataRepository.Association)))
                //{
                if (roleAStorage == roleBStorage)
                {
                    ((PersistenceLayerRunTime.ObjectStorage)Owner.ObjectStorage).CreateUnLinkCommand(roleA, roleB, null, new AssociationEndAgent(AssociationEnd), index);
                }
                else
                {
                    if (AssociationEnd.Association.RoleB.Navigable)
                        roleAStorage.CreateUnLinkCommand(roleA, roleB, relationObject, new AssociationEndAgent(AssociationEnd), index);
                    if (AssociationEnd.Association.RoleA.Navigable)
                        roleBStorage.CreateUnLinkCommand(roleA, roleB, relationObject, new AssociationEndAgent(AssociationEnd), index);
                    //						Commands.UnLinkObjectsCommand unLinkObjectsCommand =new Commands.UnLinkObjectsCommand(roleA,roleB,null,new AssociationEndAgent(AssociationEnd));
                    //						TransactionContext.CurrentTransactionContext.EnlistCommand(unLinkObjectsCommand);
                }
                //}

                #endregion
            }
            else
            {
                #region Relationship with relation object.

                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(relatedObject))
                {

                    relatedStorageInstance.ObjectStorage.CreateDeleteStorageInstanceCommand(relatedStorageInstance.RealStorageInstanceRef, PersistenceLayer.DeleteOptions.EnsureObjectDeletion);
                    stateTransition.Consistent = true;
                }
                #endregion
            }

            if (AssociationEnd.Multiplicity.IsMany)
            {
                //_Count = -1;
                //LoadedRelatedObjects.Remove(relatedObject);
                changeApplied = true;
            }
            else
            {
                if (InternalRelatedObject == relatedObject)
                {
                    InternalRelatedObject = null;
                    changeApplied = true;
                }
            }



            #region removes subscription from ObjectDeleted event
            //TODO: ��� �� type ��� association end ����� interface ���� ������� ��������  
            if (!Owner.Class.HasReferentialIntegrity(AssociationEnd)	//it hasn't ReferentialIntegrity
                && !(AssociationEnd.Navigable && AssociationEnd.GetOtherEnd().Navigable))		//the association doesn't double navigable
            {

                relatedStorageInstance.RealStorageInstanceRef.ObjectDeleted -= new PersistenceLayerRunTime.ObjectDeleted(OnObjectDeleted);
            }
            #endregion



        }

        public virtual int IndexOf(object memoryInstance)
        {
            if (IsCompleteLoaded)
                return InternalLoadedRelatedObjects.IndexOf(memoryInstance);
            else
                return -1;

        }
        /// <MetaDataID>{303FF305-8CA7-428F-BC68-4CE5B33CE282}</MetaDataID>
        public virtual long GetLinkedObjectsCount()
        {
            if (Transactions.Transaction.Current != null)
            {
                using (Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition(OOAdvantech.Transactions.TransactionOption.Suppress))
                {
                    if (Owner.PersistentObjectID == null)
                    {
                        long count = 0;
                        if (InternalLoadedRelatedObjects != null)
                            count = InternalLoadedRelatedObjects.Count;
                        stateTransition.Consistent = true;
                        return count;
                    }
                    else
                    {
                        ObjectQuery objectQuery = new ObjectQuery(this);
                        long count = 0;
                        count = objectQuery.Count;
                        stateTransition.Consistent = true;
                        return count;
                    }
                }
            }
            else
            {
                if (Owner.PersistentObjectID == null)
                {
                    if (InternalLoadedRelatedObjects != null)
                        return InternalLoadedRelatedObjects.Count;
                    else
                        return 0;
                }
                else
                {
                    ObjectQuery objectQuery = new ObjectQuery(this);
                    return objectQuery.Count;
                }
            }
        }


        internal System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, object> MultilingualRelatedObject = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, object>();


        internal System.Collections.Generic.List<MultilingualObjectLink> RelatedObjectAsMultilingualObjectLinks
        {
            get
            {
                return MultilingualRelatedObject.Select(x => new MultilingualObjectLink() { Culture = x.Key, LinkedObject = x.Value }).ToList();
            }
        }

        /// <summary></summary>
        /// <MetaDataID>{5634290F-B6C6-4BB4-B589-76AE7FDC26A2}</MetaDataID>
        public StorageInstanceRef Owner;

        /// <MetaDataID>{c5e928a8-0058-4917-aaff-c77daa952a34}</MetaDataID>
        internal void InitializeRelatedObject(object relatedObject)
        {
            InternalRelatedObject = relatedObject;
            _IsCompleteLoaded = true;
        }


        internal void InitializeRelatedObject(System.Collections.Generic.List<PersistenceLayerRunTime.MultilingualObjectLink> multiligualRelatedObjects)
        {
            lock (MultilingualRelatedObject)
            {
                foreach (var multiligualObjectLink in multiligualRelatedObjects)
                {
                    using (CultureContext cultureContext = new CultureContext(multiligualObjectLink.Culture, false))
                    {
                        InternalRelatedObject = multiligualObjectLink.LinkedObject;
                    }
                }
                _IsCompleteLoaded = true;
            }
        }

        internal void ResetMultilingualRelatedObject(System.Collections.Generic.List<PersistenceLayerRunTime.MultilingualObjectLink> multiligualRelatedObjects)
        {
            lock (MultilingualRelatedObject)
            {
                MultilingualRelatedObject.Clear();
                foreach (var multiligualObjectLink in multiligualRelatedObjects)
                {
                    using (CultureContext cultureContext = new CultureContext(multiligualObjectLink.Culture, false))
                    {
                        InternalRelatedObject = multiligualObjectLink.LinkedObject;
                    }
                }
                _IsCompleteLoaded = true;
            }
        }

        internal bool HasMultilingualRelatedObjectChanges(System.Collections.Generic.List<PersistenceLayerRunTime.MultilingualObjectLink> multiligualRelatedObjects)
        {
            lock (MultilingualRelatedObject)
            {
                if (MultilingualRelatedObject.Count != multiligualRelatedObjects.Count)
                    return true;
                foreach (var multiligualObjectLink in multiligualRelatedObjects)
                {
                    using (CultureContext cultureContext = new CultureContext(multiligualObjectLink.Culture, false))
                    {
                        if (InternalRelatedObject != multiligualObjectLink.LinkedObject)
                            return true;
                    }
                }
                return false;
            }
        }

        /// <MetaDataID>{53ab9545-00eb-4fab-b730-2f685c3b9243}</MetaDataID>
        public void CompleteLoad(System.Collections.Generic.List<object> relatedObjects)
        {

            InternalLoadedRelatedObjects = new OOAdvantech.Collections.Generic.List<object>();
            foreach (var relatedObject in relatedObjects)
            {
                if (AssociationEnd.SpecificationType != null && !AssociationEnd.SpecificationType.IsInstanceOfType(relatedObject))
                {

                }


                InternalLoadedRelatedObjects.Add(relatedObject);
            }

            //if (InternalLoadedRelatedObjects == null || InternalLoadedRelatedObjects.Count == 0)
            //{
            //    InternalLoadedRelatedObjects = new OOAdvantech.Collections.Generic.List<object>();
            //    InternalLoadedRelatedObjects.AddRange(relatedObjects);
            //}
            //else
            //{
            //    InternalLoadedRelatedObjects.Clear();
            //    foreach (object relatedObject in relatedObjects)
            //        if (!InternalLoadedRelatedObjects.Contains(relatedObject))
            //            InternalLoadedRelatedObjects.Add(relatedObject);

            //}
            _IsCompleteLoaded = true;

        }

        #region IList Members

        /// <MetaDataID>{70851362-5bf9-42ce-9ef9-be42db4aadfb}</MetaDataID>
        int System.Collections.IList.Add(object value)
        {
            _Count = -1;
            int index = LoadedRelatedObjects.Count;
            return index;
        }

        /// <MetaDataID>{c0adfe0b-c538-4b72-86b3-5524574ae1b5}</MetaDataID>
        void System.Collections.IList.Clear()
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{d7afb8bf-780e-4e30-a0d6-d8001a040168}</MetaDataID>
        bool System.Collections.IList.Contains(object value)
        {
            return Contains(value);

        }

        /// <MetaDataID>{5255c0a2-9fe9-4bd4-bff7-5fb1725d3527}</MetaDataID>
        int System.Collections.IList.IndexOf(object value)
        {
            if (!IsCompleteLoaded)
                CompleteLoad();
            return LoadedRelatedObjects.IndexOf(value);


        }

        /// <MetaDataID>{33ea71f4-1e51-483a-8da9-af73962b097c}</MetaDataID>
        void System.Collections.IList.Insert(int index, object value)
        {
            if (!IsCompleteLoaded)
                CompleteLoad();
            LoadedRelatedObjects.Insert(index, value);

        }

        /// <MetaDataID>{57dc86a3-b83e-42d0-8235-8b5048a5e668}</MetaDataID>
        bool System.Collections.IList.IsFixedSize
        {
            get { return false; }
        }

        /// <MetaDataID>{a924d4d2-c479-4f25-9f0d-29f829a42fe2}</MetaDataID>
        bool System.Collections.IList.IsReadOnly
        {
            get { return false; }
        }

        /// <MetaDataID>{0132dbad-2e8d-49b6-9270-e96545d7f6c2}</MetaDataID>
        void System.Collections.IList.Remove(object value)
        {
            _Count = -1;
            LoadedRelatedObjects.Remove(value);
        }

        /// <MetaDataID>{02841216-ada6-4a7c-8295-c2964e4bb05c}</MetaDataID>
        void System.Collections.IList.RemoveAt(int index)
        {
            if (!IsCompleteLoaded)
                CompleteLoad();
            LoadedRelatedObjects.RemoveAt(index);

        }

        /// <MetaDataID>{d71a461f-b53d-49b1-a8cd-f6ba77a89468}</MetaDataID>
        object System.Collections.IList.this[int index]
        {
            get
            {
                if (!IsCompleteLoaded)
                    CompleteLoad();
                return LoadedRelatedObjects[index];


            }
            set
            {
                if (!IsCompleteLoaded)
                    CompleteLoad();
                LoadedRelatedObjects[index] = value;

            }
        }

        #endregion

        #region ICollection Members

        /// <MetaDataID>{133aef03-df06-4fda-8604-733ac991ae80}</MetaDataID>
        void System.Collections.ICollection.CopyTo(System.Array array, int index)
        {
            if (!IsCompleteLoaded)
                CompleteLoad();

            for (int i = 0; i < array.Length; i++)
            {
                if (index + i < LoadedRelatedObjects.Count)
                    array.SetValue(LoadedRelatedObjects[index + i], i);
            }


        }
        internal protected void InvalidateCount()
        {
            _Count = -1;
        }
        /// <MetaDataID>{bfcdca0d-06d9-4884-b1f0-a07702e925db}</MetaDataID>
        int _Count = -1;
        /// <MetaDataID>{1462a667-a0aa-4800-87c3-8e53ebb121ad}</MetaDataID>
        int System.Collections.ICollection.Count
        {
            get
            {

                /// If an object added when code runs under open transaction the transaction system 
                /// can undo the changes of collection when transaction fails. 
                /// If system ask from the collection to retrieve the related object the collection will 
                /// return all objects. 
                /// It will return also the temporary object of transaction. </summary>


                if (!AssociationEnd.Multiplicity.IsMany)
                    throw new System.NotSupportedException("This property doesn't supported when the multiplicity of association end is zero or one");

                if (IsCompleteLoaded)
                    return LoadedRelatedObjects.Count;
                else
                {
                    if (_Count != -1)
                        return _Count;
                    _Count = (int)GetLinkedObjectsCount();
                    return _Count;
                }
            }
        }

        /// <MetaDataID>{9eb3c460-4c42-4130-bb6c-3667244997eb}</MetaDataID>
        bool System.Collections.ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <MetaDataID>{5e6adb9f-4e48-4097-b736-02691d50a0f9}</MetaDataID>
        object _syncRoot;
        /// <MetaDataID>{aca9afde-9a18-4cbf-9640-7bbc37fa0aed}</MetaDataID>
        object System.Collections.ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }
                return _syncRoot;
            }

        }

        public List<CultureInfo> Cultures
        {
            get
            {
                CompleteLoad();
                return MultilingualLoadedRelatedObjects.Keys.ToList();

            }
        }


        #endregion

        #region IEnumerable Members

        /// <MetaDataID>{c644a13d-2cf8-4496-8759-3e1d77b792d2}</MetaDataID>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if (!IsCompleteLoaded)
                CompleteLoad();
            return LoadedRelatedObjects.GetEnumerator();
        }

        #endregion

        /// <MetaDataID>{313b0549-fa53-4056-9e6d-6e391a83f308}</MetaDataID>
        internal bool CanDeletePermanently(object theObject)
        {
            var storageInstanceAgent = StorageInstanceRef.GetStorageInstanceAgent(theObject);
            if (storageInstanceAgent != null)
            {
                int referentialIntegrityCount = (storageInstanceAgent.RealStorageInstanceRef as StorageInstanceRef).RuntimeReferentialIntegrityCount;
                if (Owner.Class.HasReferentialIntegrity(AssociationEnd))
                    return referentialIntegrityCount <= 1;
                else
                    return referentialIntegrityCount == 0;
            }
            return true;

        }
        /// <MetaDataID>{d28abc39-617e-4323-9ab4-41bb7a13d6d1}</MetaDataID>
        public int UpgrateReferencialIntegrity(object reletedObject, bool objectLinkAdded)
        {

            if (Owner.Class.HasReferentialIntegrity(AssociationEnd))
            {
                int referentialIntegrity = 0;
                StorageInstanceAgent storageInstanceAgent = StorageInstanceRef.GetStorageInstanceAgent(reletedObject);
                StorageInstanceRef storageInstanceRef = null;
                if (storageInstanceAgent != null)
                    storageInstanceRef = storageInstanceAgent.RealStorageInstanceRef as StorageInstanceRef;

                if (storageInstanceRef == null)
                    return 0;
                else
                    if (objectLinkAdded)
                    referentialIntegrity = storageInstanceRef.AdvanceRuntimeReferentialIntegrity();
                else
                    referentialIntegrity = storageInstanceRef.ReduceRuntimeReferentialIntegrity();
                return referentialIntegrity;
            }
            else
                return 0;
        }

        /// <MetaDataID>{2adf4f90-638c-4662-94ef-171d68e74598}</MetaDataID>
        internal void RemoveRelatedObject(object relatedObject)
        {
            _Count = -1;
            InternalLoadedRelatedObjects.Remove(relatedObject);
        }

        /// <MetaDataID>{2706a844-ccfc-4a94-8bdc-70e5805f050c}</MetaDataID>
        internal void AddRelatedObject(object relatedObject)
        {
            _Count = -1;
            InternalLoadedRelatedObjects.Add(relatedObject);

        }
        /// <MetaDataID>{cd991f1f-a7cf-43ed-b455-99361fbabe7e}</MetaDataID>
        internal void InsertRelatedObject(int index, object relatedObject)
        {
            _Count = -1;
            InternalLoadedRelatedObjects.Insert(index, relatedObject);

        }

        public class ObjectQuery : ObjectsContextQuery
        {
            RelResolver RelResolver;
            public ObjectQuery(RelResolver relResolver)
                : base(new OOAdvantech.Collections.Generic.Dictionary<string, object>())
            {
                RelResolver = relResolver;
            }




            [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
            public bool Contains(object @object)
            {

                MetaDataRepository.ObjectQueryLanguage.DataNode relatedObjectDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(this);
                if (RelResolver.AssociationEnd.Association.LinkClass == null)
                {
                    relatedObjectDataNode.AssignedMetaObject = RelResolver.AssociationEnd.Specification;
                    relatedObjectDataNode.Name = RelResolver.AssociationEnd.Specification.Name;
                }
                else
                {
                    relatedObjectDataNode.AssignedMetaObject = RelResolver.AssociationEnd.Association.LinkClass;
                    relatedObjectDataNode.Name = RelResolver.AssociationEnd.Association.LinkClass.Name;
                }


                GroupDataNode groupDataNode = new GroupDataNode(this);
                groupDataNode.Type = DataNode.DataNodeType.Group;
                groupDataNode.Name = "AutoGenGroup";
                groupDataNode.Alias = "AutoGenGroup";
                relatedObjectDataNode.ParentDataNode = groupDataNode;


                DataTrees.Add(groupDataNode);
                MetaDataRepository.ObjectQueryLanguage.AggregateExpressionDataNode countDataNode = new AggregateExpressionDataNode(this);
                countDataNode.ParentDataNode = groupDataNode;
                countDataNode.Name = "count";
                countDataNode.Type = DataNode.DataNodeType.Count;




                groupDataNode.GroupedDataNode = relatedObjectDataNode;
                groupDataNode.GroupedDataNode.ParticipateInGroopByAsGrouped = true;


                string errors = null;
                BuildDataNodeTree(ref errors);
                ObjectsContext = RelResolver.Owner.ObjectStorage;
                AddSelectListItem(countDataNode);
                //AddSelectListItem(dataNode);
                OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell> ownerStorageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                ownerStorageCells.Add(RelResolver.Owner.StorageInstanceSet);
                OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> linkedStorageCells = null;

                if (RelResolver.AssociationEnd.Association.LinkClass == null)
                {
                    linkedStorageCells = ObjectStorage.GetLinkedStorageCells(RelResolver.AssociationEnd, new OOAdvantech.MetaDataRepository.ValueTypePath(RelResolver.ValueTypePath), new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>(ownerStorageCells));
                    foreach (MetaDataRepository.RelatedStorageCell relatedStorageCell in linkedStorageCells)
                        storageCells.Add(relatedStorageCell.StorageCell);
                }
                else
                {
                    if (RelResolver.AssociationEnd.IsRoleA)
                    {
                        linkedStorageCells = ObjectStorage.GetRelationObjectsStorageCells(RelResolver.AssociationEnd.Association, ownerStorageCells, OOAdvantech.MetaDataRepository.Roles.RoleB);
                        foreach (var relatedStorageCell in linkedStorageCells)
                            storageCells.Add(relatedStorageCell.StorageCell);
                    }
                    else
                    {
                        linkedStorageCells = ObjectStorage.GetRelationObjectsStorageCells(RelResolver.AssociationEnd.Association, ownerStorageCells, OOAdvantech.MetaDataRepository.Roles.RoleA);
                        foreach (var relatedStorageCell in linkedStorageCells)
                            storageCells.Add(relatedStorageCell.StorageCell);
                    }
                }

                //There aren't storageCells there aren't persistent 
                if (storageCells == null || storageCells.Count == 0)
                    return false;
                relatedObjectDataNode.DataSource = new StorageDataSource(relatedObjectDataNode, GetDataLoadersMetaData(storageCells, relatedObjectDataNode));
                groupDataNode.DataSource = new StorageDataSource(groupDataNode, GetDataLoadersMetaData(storageCells, relatedObjectDataNode));
                bool outStorageRelation = false;

                CreateDataSourcesForRelatedDataNode(relatedObjectDataNode);

                #region Construct data filter

                DataNode relationOwnerDataNode = null;
                foreach (DataNode subDataNode in relatedObjectDataNode.SubDataNodes)
                {
                    if (subDataNode.AssignedMetaObject == RelResolver.AssociationEnd.GetOtherEnd())
                    {
                        relationOwnerDataNode = subDataNode;
                        break;
                    }
                }
                if (relationOwnerDataNode == null)
                {
                    relationOwnerDataNode = new DataNode(this);
                    relationOwnerDataNode.ParentDataNode = relatedObjectDataNode;
                    relationOwnerDataNode.Name = RelResolver.Owner.MemoryInstance.GetType().Name;
                    relationOwnerDataNode.AssignedMetaObject = RelResolver.AssociationEnd.GetOtherEnd();
                    var ownerDataLoaderMetaData = GetDataLoadersMetaData(ownerStorageCells, relationOwnerDataNode);

                    relationOwnerDataNode.DataSource = new StorageDataSource(relationOwnerDataNode, ownerDataLoaderMetaData);


                    foreach (var relatedStorageCell in linkedStorageCells)
                    {

                        OOAdvantech.MetaDataRepository.RelatedStorageCell opositeRelatedStorageCell = new MetaDataRepository.RelatedStorageCell(relatedStorageCell.RootStorageCell, relatedStorageCell.StorageCell, relationOwnerDataNode.AssignedMetaObject.Identity.ToString(), relatedStorageCell.ThrougthRelationTable);
                        foreach (var dataLoaderMetadata in (relatedObjectDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                            dataLoaderMetadata.AddRelatedStorageCell(relationOwnerDataNode, opositeRelatedStorageCell);

                    }
                    ownerDataLoaderMetaData[RelResolver.Owner.ObjectStorage.StorageMetaData.StorageIdentity].AddRelatedStorageCell(relatedObjectDataNode, linkedStorageCells);

                }
                SearchTerm searchTerm = new SearchTerm();
                OOAdvantech.Collections.Generic.List<SearchTerm> searchTerms = new OOAdvantech.Collections.Generic.List<SearchTerm>();
                searchTerms.Add(searchTerm);

                SearchFactor searchFactor = new SearchFactor();
                DataNode searchExpressionDataNode = relationOwnerDataNode;
                ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                comparisonTerms[0] = new ObjectComparisonTerm(searchExpressionDataNode, this);
                object constantValue = RelResolver.Owner.MemoryInstance;
                string parameterName = "p" + searchExpressionDataNode.GetHashCode().ToString();
                Parameters.Add(parameterName, constantValue);
                comparisonTerms[1] = new ParameterComparisonTerm(parameterName, this);
                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, comparisonTerms, this, true, searchFactor);
                searchTerm.AddSearchFactor(searchFactor);


                searchFactor = new SearchFactor();
                searchExpressionDataNode = relatedObjectDataNode;
                comparisonTerms = new ComparisonTerm[2];
                comparisonTerms[0] = new ObjectComparisonTerm(searchExpressionDataNode, this);
                constantValue = @object;
                parameterName = "p" + searchExpressionDataNode.GetHashCode().ToString();
                Parameters.Add(parameterName, constantValue);
                comparisonTerms[1] = new ParameterComparisonTerm(parameterName, this);
                searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, comparisonTerms, this, true, searchFactor);
                searchTerm.AddSearchFactor(searchFactor);
                var searchCondition = new SearchCondition(searchTerms, this);
                #endregion


                groupDataNode.GroupingSourceSearchCondition = searchCondition;
                string errorOutput = null;
                DataTrees[0].Validate(ref errorOutput);

                var objectContextReference = new QueryResultObjectContextReference();
                objectContextReference.ObjectQueryContext = this;

                QueryResultType = new QueryResultType(groupDataNode.HeaderDataNode, objectContextReference);
                QueryResultType.ValueDataNode = countDataNode;

                //Distribute();
                LoadData();
                int count = 0;
                foreach (CompositeRowData rowData in QueryResultType.DataLoader)
                {
                    count = (int)(ulong)rowData[QueryResultType.ConventionTypeRowIndex][QueryResultType.ConventionTypeColumnIndex];
                    break;
                }

                //int count = (int)groupDataNode.DataSource.DataTable.Rows[0][0];
                return count != 0;



            }
            public int Count
            {
                get
                {
                    MetaDataRepository.ObjectQueryLanguage.DataNode relatedObjectDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(this);
                    if (RelResolver.AssociationEnd.Association.LinkClass == null)
                    {
                        relatedObjectDataNode.AssignedMetaObject = RelResolver.AssociationEnd.Specification;
                        relatedObjectDataNode.Name = RelResolver.AssociationEnd.Specification.Name;
                    }
                    else
                    {
                        relatedObjectDataNode.AssignedMetaObject = RelResolver.AssociationEnd.Association.LinkClass;
                        relatedObjectDataNode.Name = RelResolver.AssociationEnd.Association.LinkClass.Name;
                    }


                    GroupDataNode groupDataNode = new GroupDataNode(this);
                    groupDataNode.Type = DataNode.DataNodeType.Group;
                    groupDataNode.Name = "AutoGenGroup";
                    groupDataNode.Alias = "AutoGenGroup";
                    relatedObjectDataNode.ParentDataNode = groupDataNode;


                    DataTrees.Add(groupDataNode);
                    MetaDataRepository.ObjectQueryLanguage.AggregateExpressionDataNode countDataNode = new AggregateExpressionDataNode(this);
                    countDataNode.ParentDataNode = groupDataNode;
                    countDataNode.Name = "count";
                    countDataNode.Type = DataNode.DataNodeType.Count;




                    groupDataNode.GroupedDataNode = relatedObjectDataNode;
                    groupDataNode.GroupedDataNode.ParticipateInGroopByAsGrouped = true;


                    string errors = null;
                    BuildDataNodeTree(ref errors);
                    ObjectsContext = RelResolver.Owner.ObjectStorage;
                    AddSelectListItem(countDataNode);
                    //AddSelectListItem(dataNode);
                    OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell> ownerStorageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                    ownerStorageCells.Add(RelResolver.Owner.StorageInstanceSet);
                    OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell> storageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();

                    Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> linkedStorageCells = null;
                    if (RelResolver.AssociationEnd.Association.LinkClass == null)
                    {
                        linkedStorageCells = ObjectStorage.GetLinkedStorageCells(RelResolver.AssociationEnd, new OOAdvantech.MetaDataRepository.ValueTypePath(RelResolver.ValueTypePath), new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>(ownerStorageCells));
                        foreach (MetaDataRepository.RelatedStorageCell relatedStorageCell in linkedStorageCells)
                            storageCells.Add(relatedStorageCell.StorageCell);
                    }
                    else
                    {
                        if (RelResolver.AssociationEnd.IsRoleA)
                        {
                            linkedStorageCells = ObjectStorage.GetRelationObjectsStorageCells(RelResolver.AssociationEnd.Association, ownerStorageCells, OOAdvantech.MetaDataRepository.Roles.RoleB);
                            foreach (var relatedStorageCell in linkedStorageCells)
                                storageCells.Add(relatedStorageCell.StorageCell);
                        }
                        else
                        {
                            linkedStorageCells = ObjectStorage.GetRelationObjectsStorageCells(RelResolver.AssociationEnd.Association, ownerStorageCells, OOAdvantech.MetaDataRepository.Roles.RoleA);
                            foreach (var relatedStorageCell in linkedStorageCells)
                                storageCells.Add(relatedStorageCell.StorageCell);
                        }
                    }

                    //There aren't storageCells there aren't persistent 
                    if (storageCells == null || storageCells.Count == 0)
                        return 0;
                    relatedObjectDataNode.DataSource = new StorageDataSource(relatedObjectDataNode, GetDataLoadersMetaData(storageCells, relatedObjectDataNode));
                    groupDataNode.DataSource = new StorageDataSource(groupDataNode, GetDataLoadersMetaData(storageCells, groupDataNode));
                    bool outStorageRelation = false;

                    CreateDataSourcesForRelatedDataNode(relatedObjectDataNode);
                    DataNode relationOwnerDataNode = null;
                    foreach (DataNode subDataNode in relatedObjectDataNode.SubDataNodes)
                    {
                        if (subDataNode.AssignedMetaObject == RelResolver.AssociationEnd.GetOtherEnd())
                        {
                            relationOwnerDataNode = subDataNode;
                            break;
                        }
                    }
                    if (relationOwnerDataNode == null)
                    {
                        relationOwnerDataNode = new DataNode(this);
                        relationOwnerDataNode.ParentDataNode = relatedObjectDataNode;
                        relationOwnerDataNode.Name = RelResolver.Owner.MemoryInstance.GetType().Name;
                        relationOwnerDataNode.AssignedMetaObject = RelResolver.AssociationEnd.GetOtherEnd();

                        foreach (var storageCell in storageCells)
                        {
                            if (storageCell is MetaDataRepository.StorageCellReference)
                            {
                                OOAdvantech.PersistenceLayer.ObjectStorage remoteObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage((storageCell as MetaDataRepository.StorageCellReference).StorageName, (storageCell as MetaDataRepository.StorageCellReference).StorageLocation, (storageCell as MetaDataRepository.StorageCellReference).StorageType);
                                MetaDataRepository.StorageCellReference storageCellReference = (remoteObjectStorage as ObjectStorage).GetStorageCellReference(ownerStorageCells[0]);
                                (relatedObjectDataNode.DataSource as StorageDataSource).DataLoadersMetadata[(storageCell as MetaDataRepository.StorageCellReference).StorageIdentity].HasOutStorageRelationsWithSubDataNode(relationOwnerDataNode);
                                ownerStorageCells.Add(storageCellReference);
                                outStorageRelation = true;
                            }
                        }
                        var ownerDataLoaderMetaData = GetDataLoadersMetaData(ownerStorageCells, relationOwnerDataNode);
                        foreach (var relatedStorageCell in linkedStorageCells)
                        {

                            OOAdvantech.MetaDataRepository.RelatedStorageCell opositeRelatedStorageCell = new MetaDataRepository.RelatedStorageCell(relatedStorageCell.RootStorageCell, relatedStorageCell.StorageCell, relationOwnerDataNode.AssignedMetaObject.Identity.ToString(), relatedStorageCell.ThrougthRelationTable);
                            foreach (var dataLoaderMetadata in (relatedObjectDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                                dataLoaderMetadata.AddRelatedStorageCell(relationOwnerDataNode, opositeRelatedStorageCell);

                        }
                        ownerDataLoaderMetaData[RelResolver.Owner.ObjectStorage.StorageMetaData.StorageIdentity].AddRelatedStorageCell(relatedObjectDataNode, linkedStorageCells);

                        relationOwnerDataNode.DataSource = new StorageDataSource(relationOwnerDataNode, ownerDataLoaderMetaData);
                    }

                    if (outStorageRelation)
                    {
                        relationOwnerDataNode.ThroughRelationTable = true;
                        foreach (var dataLoaderMetaData in (relationOwnerDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                            dataLoaderMetaData.HasOutStorageRelationsWithParent = true;

                    }
                    SearchTerm searchTerm = new SearchTerm();
                    OOAdvantech.Collections.Generic.List<SearchTerm> searchTerms = new OOAdvantech.Collections.Generic.List<SearchTerm>();
                    searchTerms.Add(searchTerm);

                    SearchFactor searchFactor = new SearchFactor();
                    DataNode searchExpressionDataNode = relationOwnerDataNode;
                    ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                    comparisonTerms[0] = new ObjectComparisonTerm(searchExpressionDataNode, this);
                    object constantValue = RelResolver.Owner.MemoryInstance;
                    string parameterName = "p" + relationOwnerDataNode.GetHashCode().ToString();
                    Parameters.Add(parameterName, constantValue);
                    comparisonTerms[1] = new ParameterComparisonTerm(parameterName, this);
                    searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, comparisonTerms, this, true, searchFactor);
                    searchTerm.AddSearchFactor(searchFactor);
                    //dataNode.AddSearchCondition(new SearchCondition(searchTerms, this));
                    groupDataNode.GroupingSourceSearchCondition = new SearchCondition(searchTerms, this);
                    string errorOutput = null;
                    DataTrees[0].Validate(ref errorOutput);


                    var objectContextReference = new QueryResultObjectContextReference();
                    objectContextReference.ObjectQueryContext = this;

                    QueryResultType = new QueryResultType(groupDataNode.HeaderDataNode, objectContextReference);
                    QueryResultType.ValueDataNode = countDataNode;

                    //Distribute();
                    LoadData();
                    int count = 0;
                    foreach (CompositeRowData rowData in QueryResultType.DataLoader)
                    {
                        count = (int)(ulong)rowData[QueryResultType.ConventionTypeRowIndex][QueryResultType.ConventionTypeColumnIndex];
                        break;
                    }
                    return count;
                }
            }
            public System.Collections.Generic.List<object> RelatedObjects
            {
                get
                {
                    System.Collections.Generic.List<object> releatedObjects = new System.Collections.Generic.List<object>();
                    MetaDataRepository.ObjectQueryLanguage.DataNode relatedObjectDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(this);
                    if (RelResolver.AssociationEnd.Association.LinkClass == null)
                    {
                        if (RelResolver.AssociationEnd.Indexer)
                        {
                            relatedObjectDataNode.AssignedMetaObject = RelResolver.AssociationEnd;
                            relatedObjectDataNode.Name = RelResolver.AssociationEnd.Name;
                            //relatedObjectDataNode.MembersFetchingObjectActivation = true;
                        }
                        else
                        {
                            relatedObjectDataNode.AssignedMetaObject = RelResolver.AssociationEnd.Specification;
                            relatedObjectDataNode.Name = RelResolver.AssociationEnd.Specification.Name;
                            //relatedObjectDataNode.MembersFetchingObjectActivation = true;
                        }
                    }
                    else
                    {
                        relatedObjectDataNode.AssignedMetaObject = RelResolver.AssociationEnd.Association.LinkClass;
                        relatedObjectDataNode.Name = RelResolver.AssociationEnd.Association.LinkClass.Name;
                    }
                    DataNode relationOwnerDataNode = null;
                    if (relatedObjectDataNode.AssignedMetaObject is MetaDataRepository.AssociationEnd)
                    {
                        relationOwnerDataNode = new OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode(this);
                        relationOwnerDataNode.AssignedMetaObject = RelResolver.Owner.StorageInstanceSet.Type;
                        relationOwnerDataNode.Name = RelResolver.Owner.StorageInstanceSet.Type.Name;
                        relatedObjectDataNode.ParentDataNode = relationOwnerDataNode;
                        //relatedObjectDataNode.MembersFetchingObjectActivation = true;
                        DataTrees.Add(relationOwnerDataNode);
                    }
                    else
                        DataTrees.Add(relatedObjectDataNode);
                    string errors = null;
                    BuildDataNodeTree(ref errors);
                    ObjectsContext = RelResolver.Owner.ObjectStorage;
                    AddSelectListItem(relatedObjectDataNode);
                    OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell> ownerStorageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                    ownerStorageCells.Add(RelResolver.Owner.StorageInstanceSet);

                    OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell> relatedStorageCells = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.StorageCell>();
                    Collections.Generic.Set<MetaDataRepository.RelatedStorageCell> linkedStorageCells = null;
                    if (RelResolver.AssociationEnd.Association.LinkClass == null)
                    {
                        linkedStorageCells = ObjectStorage.GetLinkedStorageCells(RelResolver.AssociationEnd, new OOAdvantech.MetaDataRepository.ValueTypePath(RelResolver.ValueTypePath), new OOAdvantech.Collections.Generic.Set<MetaDataRepository.StorageCell>(ownerStorageCells));
                        foreach (MetaDataRepository.RelatedStorageCell relatedStorageCell in linkedStorageCells)
                            relatedStorageCells.Add(relatedStorageCell.StorageCell);
                    }
                    else
                    {
                        if (RelResolver.AssociationEnd.IsRoleA)
                        {
                            linkedStorageCells = ObjectStorage.GetRelationObjectsStorageCells(RelResolver.AssociationEnd.Association, ownerStorageCells, OOAdvantech.MetaDataRepository.Roles.RoleB);
                            foreach (var relatedStorageCell in linkedStorageCells)
                                relatedStorageCells.Add(relatedStorageCell.StorageCell);
                        }
                        else
                        {
                            linkedStorageCells = ObjectStorage.GetRelationObjectsStorageCells(RelResolver.AssociationEnd.Association, ownerStorageCells, OOAdvantech.MetaDataRepository.Roles.RoleA);
                            foreach (var relatedStorageCell in linkedStorageCells)
                                relatedStorageCells.Add(relatedStorageCell.StorageCell);
                        }
                    }

                    if (relatedStorageCells.Count == 0)
                        return releatedObjects;

                    relatedObjectDataNode.DataSource = new StorageDataSource(relatedObjectDataNode, GetDataLoadersMetaData(relatedStorageCells, relatedObjectDataNode));

                    if (RelResolver.AssociationEnd.Indexer)
                    {
                        relationOwnerDataNode.DataSource = new StorageDataSource(relationOwnerDataNode, GetDataLoadersMetaData(ownerStorageCells, relationOwnerDataNode));
                        CreateDataSourcesForRelatedDataNode(relatedObjectDataNode);
                        foreach (var relatedStorageCell in linkedStorageCells)
                        {
                            foreach (var dataLoaderMetadata in (relationOwnerDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                                dataLoaderMetadata.AddRelatedStorageCell(relatedObjectDataNode, relatedStorageCell);
                        }

                    }
                    else
                    {
                        CreateDataSourcesForRelatedDataNode(relatedObjectDataNode);

                        foreach (DataNode subDataNode in relatedObjectDataNode.SubDataNodes)
                        {
                            if (subDataNode.AssignedMetaObject == RelResolver.AssociationEnd.GetOtherEnd())
                            {
                                relationOwnerDataNode = subDataNode;
                                break;
                            }
                        }

                        if (relationOwnerDataNode == null)
                        {
                            relationOwnerDataNode = new DataNode(this);
                            relationOwnerDataNode.ParentDataNode = relatedObjectDataNode;
                            //relationOwnerDataNode.MembersFetchingObjectActivation = true;
                            relationOwnerDataNode.Name = RelResolver.Owner.MemoryInstance.GetType().Name;
                            relationOwnerDataNode.AssignedMetaObject = RelResolver.AssociationEnd.GetOtherEnd();

                            var ownerDataLoaderMetaData = GetDataLoadersMetaData(ownerStorageCells, relationOwnerDataNode);

                            foreach (var relatedStorageCell in linkedStorageCells)
                            {
                                MetaDataRepository.AssociationEnd opositeRelatedStorageCellAssocationEnd = null;
                                if (RelResolver.AssociationEnd.Identity.ToString() != relatedStorageCell.AssociationEndIdentity)
                                {
                                    if (RelResolver.AssociationEnd.IsRoleA)
                                        opositeRelatedStorageCellAssocationEnd = RelResolver.AssociationEnd.Association.Specializations.Where(x => x.RoleA.Identity.ToString() == relatedStorageCell.AssociationEndIdentity).FirstOrDefault().RoleB;
                                    else
                                        opositeRelatedStorageCellAssocationEnd = RelResolver.AssociationEnd.Association.Specializations.Where(x => x.RoleB.Identity.ToString() == relatedStorageCell.AssociationEndIdentity).FirstOrDefault().RoleA;
                                }
                                else
                                    opositeRelatedStorageCellAssocationEnd = RelResolver.AssociationEnd.GetOtherEnd();

                                OOAdvantech.MetaDataRepository.RelatedStorageCell opositeRelatedStorageCell = new MetaDataRepository.RelatedStorageCell(relatedStorageCell.RootStorageCell, relatedStorageCell.StorageCell, opositeRelatedStorageCellAssocationEnd.Identity.ToString(), relatedStorageCell.ThrougthRelationTable);
                                foreach (var dataLoaderMetadata in (relatedObjectDataNode.DataSource as StorageDataSource).DataLoadersMetadata.Values)
                                    dataLoaderMetadata.AddRelatedStorageCell(relationOwnerDataNode, opositeRelatedStorageCell);

                                //relatedStorageCell.AssociationEndIdentity


                            }

                            ownerDataLoaderMetaData[RelResolver.Owner.ObjectStorage.StorageMetaData.StorageIdentity].AddRelatedStorageCell(relatedObjectDataNode, linkedStorageCells);

                            // var gfd = ownerDataLoaderMetaData[RelResolver.Owner.ObjectStorage.StorageMetaData.StorageIdentity].RelatedStorageCells[relatedObjectDataNode.Identity];
                            relationOwnerDataNode.DataSource = new StorageDataSource(relationOwnerDataNode, ownerDataLoaderMetaData);
                        }
                    }
                    string sourceStorageIdentity = RelResolver.Owner.StorageInstanceSet.StorageIdentity;

                    foreach (var entry in (relatedObjectDataNode.DataSource as StorageDataSource).DataLoadersMetadata)
                    {
                        string storageIdentity = entry.Key;
                        #region Checks for out storage relations
                        if (storageIdentity != sourceStorageIdentity)
                        {

                            entry.Value.HasOutStorageRelationsWithSubDataNode(relationOwnerDataNode);
                            (relationOwnerDataNode.DataSource as StorageDataSource).DataLoadersMetadata[sourceStorageIdentity].HasOutStorageRelationsWithParent = true;
                            bool exist = false;
                            /// in case where there are related storarage cells in other storage than relation owner storage
                            /// creates  DataLoaderMetadata for those storages and adds StorageCellReferenceMetadata for the relation owner storageCell  
                            #region search for storageCellReference metadata
                            if (!(relationOwnerDataNode.DataSource as StorageDataSource).DataLoadersMetadata.ContainsKey(storageIdentity))
                                (relationOwnerDataNode.DataSource as StorageDataSource).DataLoadersMetadata[storageIdentity] = new DataLoaderMetadata(relationOwnerDataNode, entry.Value.QueryStorageID, entry.Value.ObjectsContextIdentity, entry.Value.StorageName, entry.Value.StorageLocation, entry.Value.StorageType);
                            else
                            {
                                foreach (MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData storageCellReferenceMetaData in (relationOwnerDataNode.DataSource as StorageDataSource).DataLoadersMetadata[storageIdentity].StorageCellReferencesMetaData)
                                {
                                    if (storageCellReferenceMetaData.SerialNumber == RelResolver.Owner.StorageInstanceSet.SerialNumber &&
                                        storageCellReferenceMetaData.StorageIdentity == RelResolver.Owner.StorageInstanceSet.StorageIdentity)
                                    {
                                        exist = true;
                                        break;
                                    }
                                }
                            }
                            #endregion

                            if (!exist)
                                (relationOwnerDataNode.DataSource as StorageDataSource).DataLoadersMetadata[storageIdentity].StorageCellReferencesMetaData.Add(new OOAdvantech.MetaDataRepository.StorageCellReference.StorageCellReferenceMetaData(RelResolver.Owner.StorageInstanceSet));

                        }
                        #endregion

                    }


                    SearchTerm searchTerm = new SearchTerm();

                    OOAdvantech.Collections.Generic.List<SearchTerm> searchTerms = new OOAdvantech.Collections.Generic.List<SearchTerm>();
                    searchTerms.Add(searchTerm);

                    SearchFactor searchFactor = new SearchFactor();
                    DataNode searchExpressionDataNode = relationOwnerDataNode;
                    ComparisonTerm[] comparisonTerms = new ComparisonTerm[2];
                    comparisonTerms[0] = new ObjectComparisonTerm(searchExpressionDataNode, this);
                    object constantValue = RelResolver.Owner.MemoryInstance;
                    string parameterName = "p" + relationOwnerDataNode.GetHashCode().ToString();
                    Parameters.Add(parameterName, constantValue);
                    comparisonTerms[1] = new ParameterComparisonTerm(parameterName, this);
                    searchFactor.Criterion = new Criterion(Criterion.ComparisonType.Equal, comparisonTerms, this, true, searchFactor);
                    searchTerm.AddSearchFactor(searchFactor);
                    relatedObjectDataNode.AddSearchCondition(new SearchCondition(searchTerms, this));

                    var objectContextReference = new QueryResultObjectContextReference();
                    objectContextReference.ObjectQueryContext = this;
                    QueryResultType = new QueryResultType(relatedObjectDataNode.HeaderDataNode, objectContextReference);
                    SinglePart relatedObjectMember = new SinglePart(relatedObjectDataNode, "RelatedObject", QueryResultType);
                    QueryResultType.AddMember(relatedObjectMember);
                    QueryResultType.DataFilter = relatedObjectDataNode.SearchCondition;

                    //Distribute();
                    LoadData();

                    if (this.RelResolver.AssociationEnd.Indexer)
                    {

                    }
                    foreach (CompositeRowData rowData in QueryResultType.DataLoader)
                        releatedObjects.Add((relatedObjectMember as QueryResultPart).GetValue(rowData));
                    //foreach (System.Data.DataRow dataRow in relatedObjectDataNode.DataSource.DataTable.Rows)
                    //    releatedObjects.Add(dataRow[relatedObjectDataNode.DataSource.ObjectIndex]);
                    return releatedObjects;
                }

            }
            public override OOAdvantech.Collections.Generic.List<DataNode> SelectListItems
            {
                get
                {
                    return _SelectListItems;
                }
            }
        }


        public bool ValueTypePathContainsAttribute(DotNetMetaDataRepository.Attribute attribute, int pathIndex = 0)
        {
            return attribute.Type is DotNetMetaDataRepository.Structure &&
                (attribute.Type as DotNetMetaDataRepository.Structure).Persistent &&
                Owner is StorageInstanceValuePathRef &&
                (Owner as StorageInstanceValuePathRef).ValueTypePath.ToArray()[(Owner as StorageInstanceValuePathRef).ValueTypePath.Count - pathIndex - 1] == attribute.Identity;


        }
    }


    /// <MetaDataID>{7e26c83e-c459-46cd-9308-35dfa4df48a2}</MetaDataID>
    public struct MultilingualObjectLink
    {
        public System.Globalization.CultureInfo Culture;
        public object LinkedObject;
    }



}
