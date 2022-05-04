namespace OOAdvantech.PersistenceLayer
{
    using System;
    using System.Collections.Generic;
    using OOAdvantech.MetaDataRepository;
    using Transactions;
    using Remoting;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
    using MarshalByRefObject = System.MarshalByRefObject;
#endif


    /// <MetaDataID>{9E8150CF-FD92-43AE-B41F-559104B26250}</MetaDataID>
    /// <summary>When the system makes the state durable allocate space 
    /// in persistence memory for instance record/s   for RDBMS, 
    /// tags for XML File or stream on binary file and save the state 
    /// of object on this space. This allocated space defined as 
    /// storage instance. The storage instance reference 
    /// "StorageInstanceRef" is object  in memory that operate as 
    /// logical connection between the object in memory 
    /// "MemoryInstance" and storage instance. It is know how 
    /// retrieve the state of object from storage instance and how to 
    /// save it.</summary>
    public abstract class StorageInstanceRef : MarshalByRefObject, Remoting.IExtMarshalByRefObject
    {

       
        /// <MetaDataID>{e49ebda2-836c-4a3b-9af5-5d9bd553fc59}</MetaDataID>
        public abstract void ObjectActived();
        //public abstract void WaitUntilObjectIsActive();


        public abstract void ObjectDeleting();



        /// <summary>
        /// Retrives the object identity and storage identity from the persistent object
        /// </summary>
        /// <param name="persistentObject">
        /// Defines the object from which we want the object identity and the identity of storage.
        /// </param>
        /// <param name="persistentObjectID">
        /// Defines the persistent object identity of object.
        /// This parameter has valid value when object state has been commited permanetly in storage.    
        /// </param>
        /// <param name="storageIdentity">
        /// Defines the identity of storage of object.  
        /// </param>
        /// <MetaDataID>{3ea0d95f-a582-4c48-bc87-5e084aff7771}</MetaDataID>
        public static void GetPersistentObjectID(object persistentObject, out OOAdvantech.PersistenceLayer.ObjectID persistentObjectID, out string storageIdentity)
        {
            PersistenceLayer.ObjectStorage.PersistencyService.GetStorageInstancePersistentObjectID(persistentObject, out persistentObjectID, out storageIdentity);
        }

        ///<summary>
        ///Retrives the object identity and storage identity from the persistent object.
        ///</summary>
        ///<param name="persistentObject">
        ///Defines the object from which we want the object identity and the identity of storage.
        ///</param>
        ///<param name="objectID">
        ///Defines the persistent object identity of object.
        ///This parameter has valid value when object state has been commited permanetly in storage.  
        ///The objectID can be changed when the object just created in object context and transction is open.
        ///Can be used alternatively the PersistentObjectID
        ///</param>
        ///<param name="storageIdentity">
        ///Defines the identity of storage of object.  
        ///</param>
        /// <MetaDataID>{744d8714-4ae7-4318-b839-ae784ac3e2e0}</MetaDataID>
        public static void GetObjectID(object persistentObject, out OOAdvantech.PersistenceLayer.ObjectID objectID, out string storageIdentity)
        {
            PersistenceLayer.ObjectStorage.PersistencyService.GetStorageInstanceObjectID(persistentObject, out objectID, out storageIdentity);
        }

        ///<summary>
        ///Defines the storage cell which host the storage instance   
        ///</summary>
        /// <MetaDataID>{2a96d279-8f61-49aa-931a-e2ef37bf5c07}</MetaDataID>
        public abstract MetaDataRepository.StorageCell StorageInstanceSet
        {
            get;
        }

        /// <MetaDataID>{7bf83826-4ed6-49f6-a434-a2ccaa0f44da}</MetaDataID>
        public class ObjectSate
        {
            ///// <MetaDataID>{c23c800e-6970-4407-ac59-56d7d1700ede}</MetaDataID>
            //System.Data.DataRow DbDataRecord;
            object[] Values;

            public object Tag;

            /// <MetaDataID>{af2bdc56-1b61-4ad1-a8dc-204c4d8c197d}</MetaDataID>
            public System.Collections.Generic.Dictionary<string, int> ColumnsIndices;
            public ObjectSate(object[] values, System.Collections.Generic.Dictionary<string, int> columnsIndices)
            {
                Values = values;//.Clone() as object[];
                ColumnsIndices = columnsIndices;
            }

            //public ObjectSate(System.Data.DataRow dbDataRecord)
            //{
            //    DbDataRecord = dbDataRecord;
            //}
            public object this[string index]
            {
                get
                {
                    //if (DbDataRecord != null)
                    //    return DbDataRecord[index];
                    //else
                        return Values[ColumnsIndices[index.ToLower()]];
                }
            }

            public object this[int index]
            {
                get
                {
                    //if (DbDataRecord != null)
                    //    return DbDataRecord[index];
                    //else
                        return Values[index];
                }
            }


            public bool TryGetValue(string index, out object value)
            {
                value =null;
                //if (DbDataRecord != null)
                //{
                //    if (DbDataRecord.Table.Columns.Contains(index))
                //    {
                //        value = DbDataRecord[index];
                //        return true;
                //    }
                //    else
                //        return false;
                //}
                //else
                {
                    int columnIndex = 0;
                    if (ColumnsIndices.TryGetValue(index.ToLower(), out columnIndex))
                    {
                        value = Values[columnIndex];
                        return true;
                    }
                    else
                        return false;

                }

            }
        }

        internal protected abstract void InitializeRelationshipResolverForValueType(MetaDataRepository.Attribute attribute,System.Globalization.CultureInfo culture);


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6C041AA0-C892-4F39-A0CC-9FF1BCF525B3}</MetaDataID>
        protected ObjectStorage _ObjectStorage;

        /// <summary>Defines the object storage wich host 
        /// the passive object (storage instance).</summary>
        /// <MetaDataID>{48A57C56-602A-485C-8765-26FF656D5A5D}</MetaDataID>
        [OOAdvantech.MetaDataRepository.Association("", Roles.RoleA, "{3C880369-A4F1-400C-8909-375BA9831BD9}")]
        [IgnoreErrorCheck]
        public ObjectStorage ObjectStorage
        {
            get
            {
                return _ObjectStorage;
            }
        }

       

        /// <summary>Return the storage instance reference for the operative object.</summary>
        /// <param name="persistentObject">
        /// Defines the operative object
        /// </param>
        /// <MetaDataID>{85A93AB0-FA00-4258-B058-0E929004292F}</MetaDataID>
        public static StorageInstanceRef GetStorageInstanceRef(object persistentObject)
        {
            try
            {
                bool remoteObject = Remoting.RemotingServices.IsOutOfProcess(persistentObject as MarshalByRefObject);
                if (remoteObject)
                    throw new System.Exception("System can't retrieve StorageInstanceRef from remote object");

                OOAdvantech.ObjectStateManagerLink extensionProperties =  OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(persistentObject);
                if (extensionProperties == null)
                {
                    return null;
                    //var storageInstanceRefLink = StorageInstanceRefLink.GetStorageInstanceRefLink(persistentObject);
                    //if (storageInstanceRefLink != null)
                    //    return storageInstanceRefLink.StorageInstanceRef;
                    //else
                    //    return null;
                }
                return extensionProperties.StorageInstanceRef;

            }
            catch (System.Exception Error)
            {
                throw Error;
            }
        }

        /// <MetaDataID>{31836357-869D-46D9-82C3-475AA96F4385}</MetaDataID>
        /// <summary>Return the storage instance reference from extension properties of operative.</summary>
        public static StorageInstanceRef GetStorageInstanceRef(OOAdvantech.ObjectStateManagerLink extensionProperties)
        {
            if (extensionProperties == null)
                return null;
            try
            {
                return extensionProperties.StorageInstanceRef;
            }
            catch (System.Exception Error)
            {
                int hh = 0;
                throw Error;
            }
        }

        /// <MetaDataID>{348F8CD9-325D-4752-85EA-AFD8E6402A51}</MetaDataID>
        /// <summary>
        /// This method retrieves the value of _theObjects field.
        /// Persistence system use this method to break the access level of field _theObjects 
        /// of ObjectContainer class
        /// </summary>
        protected static ObjectCollection GetObjectCollection(ObjectContainer objectContainer)
        {
            return objectContainer.theObjects;
        }



        ///<summary>
        /// This method sets the value of _theObjects field.
        /// Persistence system use this method to break the access level of field _theObjects 
        /// of ObjectContainer class
        /// </summary>
        /// <MetaDataID>{4CF01CBD-820E-46FD-947D-FA9EA739D747}</MetaDataID>
        protected static void SetObjectCollection(ObjectContainer objectContainer, ObjectCollection objectCollection)
        {
            objectContainer._theObjects = objectCollection;
        }



        /// <summary>When a field or association of class declared as lazy fetching the
        /// persistence system does not load the value of this field or linked objects 
        /// when object go to operative mode. But you can call this function to take 
        /// care of loading the field value or linked object when you need that. Lazy 
        /// fetching is useful in case when the data you retrieve is huge for instance 
        /// images, streams.</summary>
        /// <param name="FieldName">The field name that the persistency system set its value.</param>
        /// <param name="DeclaringType">the type that declare the lazy fetching field</param>
        /// <MetaDataID>{4E499CC4-DEFF-4A6D-A342-D6C76EBE9721}</MetaDataID>
        public abstract void LazyFetching(string FieldName, System.Type DeclaringType);

        /// <MetaDataID>{e1f63962-8ad2-4cdb-a867-527cfd4c24ab}</MetaDataID>
        internal protected abstract void LazyFetching(object relResolver, System.Type DeclaringType);





        /// <MetaDataID>{0DFB4115-630F-4675-B6E5-B9F363D6D62F}</MetaDataID>
        /// <summary>
        /// Define the operative object.
        /// This object has its state durable in storage instance.
        ///</summary>
        [OOAdvantech.MetaDataRepository.Association("", Roles.RoleA, "9cc4008c-d544-40d4-a899-0b36bff6d5b1")]
        [IgnoreErrorCheck]
        public abstract System.Object MemoryInstance
        {
            get;
        }

        /// <summary>
        /// Object Identity used from persistency layer to find the storage instance.
        /// This can't be changed
        /// </summary>
        /// <MetaDataID>{76C3944A-47ED-4516-86E8-24C8DBBDEEA9}</MetaDataID>
        public abstract OOAdvantech.PersistenceLayer.ObjectID PersistentObjectID
        {
            get;
            set;
        }

        ///<summary>
        ///ObjectID defines the identity which is unigue in ObjectContext like storage.
        ///The ObjectID can be changed when the object just created in object context and transction is open.
        ///Can be used alternatively the PersistentObjectID
        ///</summary>
        /// <MetaDataID>{2e9d674c-b9e7-495b-ab0b-f9871c4cbcca}</MetaDataID>
        public abstract OOAdvantech.PersistenceLayer.ObjectID ObjectID
        {
            get;
            set;
        }

        ///<summary> </summary>
        /// <MetaDataID>{1d5a6e2a-fdb3-43a9-9ec4-b3d759ebdd0e}</MetaDataID>
        public abstract bool IsPersistent
        {
            get;
        }


        ///<summary> 
        ///This method checks if the related object for relation of associationEnd, has been loaded. 
        ///</summary>
        ///<param name="associationEnd">
        ///The associationEnd parameter defines the relation
        ///</param>
        ///<returns>
        ///return true if the related object loaded else return false
        ///</returns>
        /// <MetaDataID>{f82db9fb-9ce1-44e7-b913-2594ea8c4eee}</MetaDataID>
        internal abstract bool IsRelationLoaded(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd);



        /// <MetaDataID>{72832a49-65e5-44c4-9a25-ca04aff18231}</MetaDataID>
        protected void Assign(object memoryInstance)
        {
            OOAdvantech.ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(memoryInstance);
            if (extensionProperties == null)
            {
                //StorageInstanceRefLink.NewStorageInstanceRefLink(memoryInstance).StorageInstanceRef = this;
               throw new System.Exception(string.Format("There isn’t {0} field in class hierarchy of '{1}' class.",typeof(OOAdvantech.ObjectStateManagerLink).FullName,  memoryInstance.GetType().FullName ));
            }
            else
                extensionProperties.StorageInstanceRef = this;
        }
        /// <MetaDataID>{18a27155-e8b8-425b-a967-f2b0e14b365e}</MetaDataID>
        protected void RemoveAssignment()
        {
            OOAdvantech.ObjectStateManagerLink extensionProperties = OOAdvantech.ObjectStateManagerLink.GetExtensionPropertiesFromObject(MemoryInstance);
            if (extensionProperties == null)
            {
                //var storageInstanceRefLink = StorageInstanceRefLink.GetStorageInstanceRefLink(MemoryInstance);
                //if(storageInstanceRefLink!=null)
                //    storageInstanceRefLink.StorageInstanceRef = null;
            }
            else
                extensionProperties.StorageInstanceRef = null;
        }

    }

    /// <MetaDataID>{11b98535-c064-46ce-ba36-7c8ae7556ae9}</MetaDataID>
    [Transactions.Transactional]
    public class StorageInstanceRefLink : Transactions.ITransactionalObject
    {
        static System.Collections.Generic.Dictionary<int, StorageInstanceRefLink[]> StorageInstanceRefLinks = new Dictionary<int, StorageInstanceRefLink[]>();
        public static StorageInstanceRefLink NewStorageInstanceRefLink(object index)
        {
            int hasCode = index.GetHashCode();
            StorageInstanceRefLink[] storageInstanceRefLinks = null;
            if (!StorageInstanceRefLinks.TryGetValue(hasCode, out storageInstanceRefLinks))
            {
                storageInstanceRefLinks = new StorageInstanceRefLink[2];
                storageInstanceRefLinks[0] = new StorageInstanceRefLink(index);
                StorageInstanceRefLinks[hasCode] = storageInstanceRefLinks;
                return storageInstanceRefLinks[0];
            }
            else
            {
                bool hasEmptySlot = false;
                for (int i = 0; i < storageInstanceRefLinks.Length; i++)
                {
                    StorageInstanceRefLink storageInstanceRefLink = storageInstanceRefLinks[i];
                    if (storageInstanceRefLink != null)
                    {
                        if (storageInstanceRefLink.MemoryInstance == null)
                        {
                            storageInstanceRefLinks[i] = null;
                            hasEmptySlot = true;
                        }
                        else if (storageInstanceRefLinks[i].MemoryInstance == index)
                            return storageInstanceRefLinks[i];
                    }
                    else
                        hasEmptySlot = true;
                }
                if (hasEmptySlot)
                {
                    for (int i = 0; i < storageInstanceRefLinks.Length; i++)
                    {
                        if (storageInstanceRefLinks[i] == null)
                        {
                            storageInstanceRefLinks[i] = new StorageInstanceRefLink(index);
                            return storageInstanceRefLinks[i];
                        }
                    }
                    return null;
                }
                else
                {
                    StorageInstanceRefLink[] extendStorageInstanceRefLinks = new StorageInstanceRefLink[storageInstanceRefLinks.Length + 2];
                    storageInstanceRefLinks.CopyTo(extendStorageInstanceRefLinks, 0);
                    extendStorageInstanceRefLinks[storageInstanceRefLinks.Length] = new StorageInstanceRefLink(index);
                    return extendStorageInstanceRefLinks[storageInstanceRefLinks.Length];
                }
            }
        }

        public static StorageInstanceRefLink GetStorageInstanceRefLink(object index)
        {
            int hasCode = index.GetHashCode();
            StorageInstanceRefLink[] storageInstanceRefLinks = null;
            if (!StorageInstanceRefLinks.TryGetValue(hasCode, out storageInstanceRefLinks))
            {
                return null;
            }
            else
            {
                bool hasEmptySlot = false;
                for (int i = 0; i < storageInstanceRefLinks.Length; i++)
                {
                    StorageInstanceRefLink storageInstanceRefLink = storageInstanceRefLinks[i];
                    if (storageInstanceRefLink != null)
                    {
                        if (storageInstanceRefLink.MemoryInstance == null)
                        {
                            storageInstanceRefLinks[i] = null;
                            hasEmptySlot = true;
                        }
                        else if (storageInstanceRefLinks[i].MemoryInstance == index)
                            return storageInstanceRefLinks[i];
                    }
                    
                }
                return null;
            }
        }


       static public void Collect()
        {
            foreach (var entry in StorageInstanceRefLinks)
            {
                for(int i=0 ;i<entry.Value.Length;i++)
                {
                    if (entry.Value[i] != null && entry.Value[i].MemoryInstance == null)
                        entry.Value[i] = null;
                }

            }


        }

        

        System.WeakReference MemoryInstanceReference;
        public StorageInstanceRefLink(object memoryInstance)
        {
            MemoryInstanceReference = new WeakReference(memoryInstance);
        }
        public object MemoryInstance
        {
            get
            {
                return MemoryInstanceReference.Target;
            }
        }

        #region ITransactionalObject Members

        public void MergeChanges(OOAdvantech.Transactions.Transaction masterTransaction, OOAdvantech.Transactions.Transaction nestedTransaction)
        {
            throw new NotImplementedException();
        }
        internal StorageInstanceRef StorageInstanceRef;
        StorageInstanceRef BackUpStorageInstanceRef;
        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            BackUpStorageInstanceRef = StorageInstanceRef;
        }

        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            throw new NotImplementedException();
        }

        public void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            StorageInstanceRef = BackUpStorageInstanceRef;
            BackUpStorageInstanceRef = null;
        }

        public bool ObjectHasGhanged(Transactions.TransactionRunTime transaction)
        {
            return StorageInstanceRef != BackUpStorageInstanceRef;
        }

        public void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            BackUpStorageInstanceRef = null;
        }

        public void EnsureSnapshot(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
