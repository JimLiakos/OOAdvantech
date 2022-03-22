


namespace OOAdvantech
{
    using System;
    using System.Linq;
    using OOAdvantech.Transactions;
#if PORTABLE 
    using System.PCL.Reflection;
#else
    using System.Reflection;
    using OOAdvantech.MetaDataRepository;
#endif
    public delegate void ObjectChangeStateHandle(object _object, string member);

    public delegate void ObjectStateChangedHandle();
    public delegate void LinkedObjectHandle(object linkedObject, AssociationEnd associationEnd);
    /// <MetaDataID>{61DF32D2-45F5-4A47-9BA9-D818BE57EC61}</MetaDataID>

    [Serializable]
    [Transactions.ContainByValue]
    public sealed class ObjectStateManagerLink : ITransactionalObject, PersistenceLayer.IObjectStateEventsConsumer
#if DISTRIBUTED_TRANSACTIONS
        , System.Runtime.Serialization.ISerializable
#endif
    {
        /// <MetaDataID>{809F9367-4727-4AE9-83F6-1191A3AD4405}</MetaDataID>
        public ObjectStateManagerLink()
        {

        }

        /// <MetaDataID>{f9479bf8-a09a-459a-994f-2ce290529b62}</MetaDataID>
        internal bool ObjectMembersInitialized;

#if DISTRIBUTED_TRANSACTIONS

        #region ISerializable Members

        /// <MetaDataID>{AE2605F9-753F-483A-8AF6-1BC2967330A0}</MetaDataID>
        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            OOAdvantech.Collections.Hashtable SerializedPrperties = new OOAdvantech.Collections.Hashtable();
            if (Properties != null)
            {
                foreach (var entry in Properties)
                {
                    if (entry.Value != null && (entry.Value.GetType().IsMarshalByRef || entry.Value.GetType().IsSerializable))
                        SerializedPrperties.Add(entry.Key, entry.Value);
                    else
                        SerializedPrperties.Add(entry.Key, null);
                }
            }
            info.AddValue("Properties", SerializedPrperties);
        }

        #endregion

        /// <MetaDataID>{121029D8-0E96-4DF4-A44E-14CA121DC310}</MetaDataID>
        ObjectStateManagerLink(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            CreatedFromSerialization = true;
            Properties = info.GetValue("Properties", typeof(OOAdvantech.Collections.Generic.Dictionary<string, object>)) as OOAdvantech.Collections.Generic.Dictionary<string, object>;
        }
#endif
        /// <MetaDataID>{54FFC3B2-9F83-478A-A0E0-62BD8D8009FB}</MetaDataID>
        bool CreatedFromSerialization = false;

        /// <MetaDataID>{63A17F25-A3E7-4C61-8DB2-F342117E6C03}</MetaDataID>
        static System.Collections.Generic.List<System.Reflection.FieldInfo> GetTypeFields(Type objectType)
        {
            System.Reflection.FieldInfo[] fields = objectType.GetMetaData().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            System.Collections.Generic.List<System.Reflection.FieldInfo> typeHierarchyfields = new System.Collections.Generic.List<System.Reflection.FieldInfo>();
            if (objectType.GetMetaData().BaseType != null)
                typeHierarchyfields.AddRange(GetTypeFields(objectType.GetMetaData().BaseType));
            typeHierarchyfields.AddRange(fields);
            return typeHierarchyfields;
        }
        /// <MetaDataID>{28754050-2550-4845-8848-DABDD19FD5EC}</MetaDataID>
        static System.Collections.Generic.Dictionary<System.Type, System.Reflection.FieldInfo> ExtensionPropertiesFieldInfos = new System.Collections.Generic.Dictionary<System.Type, System.Reflection.FieldInfo>();

        /// <MetaDataID>{3eddfc27-28b3-4503-a15c-81795760437c}</MetaDataID>
        PersistenceLayer.StorageInstanceRef _StorageInstanceRef;
        /// <MetaDataID>{6e4037cb-ee6f-4463-9b2f-13360ece51be}</MetaDataID>
        internal PersistenceLayer.StorageInstanceRef StorageInstanceRef
        {
            get
            {
                return _StorageInstanceRef;
            }
            set
            {

                //using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                //{
                if (value == null && _StorageInstanceRef != null)
                {

                }
                _StorageInstanceRef = value;
                //  stateTransition.Consistent = true;
                //}

            }
        }

        /// <MetaDataID>{48061625-2AC1-463F-900B-2A76BD614D39}</MetaDataID>
        public static System.Reflection.FieldInfo GetExtensionPropertiesField(System.Type typeWithExtensionProperties)
        {


            System.Reflection.FieldInfo extensionPropertiesField = null;
            if (!ExtensionPropertiesFieldInfos.TryGetValue(typeWithExtensionProperties, out extensionPropertiesField))
            {

                if (typeWithExtensionProperties.GetMetaData().GetCustomAttributes(typeof(OOAdvantech.Transactions.TransactionalAttribute), true).Length == 0)
                    throw new System.ArgumentException("Error on class '" + typeWithExtensionProperties + "' You can use the extension properties only to the class with transactional attribute");
                System.Collections.Generic.List<System.Reflection.FieldInfo> typeHierarchyfields = GetTypeFields(typeWithExtensionProperties);

                foreach (System.Reflection.FieldInfo fieldInfo in typeHierarchyfields)
                {
                    if (fieldInfo.FieldType == typeof(OOAdvantech.ObjectStateManagerLink))
                    {
                        extensionPropertiesField = fieldInfo;
                        break;
                    }
                }
                //if(extensionPropertiesField==null)
                //    return null;
                ExtensionPropertiesFieldInfos.Add(typeWithExtensionProperties, extensionPropertiesField);
            }
            return extensionPropertiesField;

        }

        /// <MetaDataID>{012D3425-E6A2-46A9-A0D6-1C8988367A26}</MetaDataID>
        public static ObjectStateManagerLink GetExtensionPropertiesFromObject(object objectWithExtensionProperties)
        {
            //Error prone ίσως θα ηταν καλύτερο να κρατάω σε μια map το fieldinfo για κάθε type.
            if (objectWithExtensionProperties == null)
                throw new System.ArgumentNullException("The parameter 'objectWithExtensionProperties' value is null");

            AccessorBuilder.FieldPropertyAccessor fastFieldAccessor = AccessorBuilder.LoadTypeMetadata(objectWithExtensionProperties.GetType()).ExtensionPropertiesFastFieldFieldAccessor;

            if (fastFieldAccessor == null || fastFieldAccessor.MemberInfo == null)
            {
                return null;
            }
            else
            {
                ObjectStateManagerLink extensionProperties = fastFieldAccessor.GetValue(objectWithExtensionProperties) as ObjectStateManagerLink;
                if (extensionProperties == null)
                {
                    extensionProperties = new ObjectStateManagerLink();
                    fastFieldAccessor.SetValue(objectWithExtensionProperties, extensionProperties);
                }
                return extensionProperties;
            }
        }


        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<Transactions.Transaction, Collections.Generic.Dictionary<string, object>> _PropertiesValuesSnapshots;
        /// <MetaDataID>{e38731d1-e451-45af-8426-bf5f92ae2431}</MetaDataID>
        System.Collections.Generic.Dictionary<Transactions.Transaction, Collections.Generic.Dictionary<string, object>> PropertiesValuesSnapshots
        {
            get
            {
                if (_PropertiesValuesSnapshots == null)
                    _PropertiesValuesSnapshots = new System.Collections.Generic.Dictionary<OOAdvantech.Transactions.Transaction, Collections.Generic.Dictionary<string, object>>();
                return _PropertiesValuesSnapshots;
            }
        }

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<Transactions.Transaction, PersistenceLayer.StorageInstanceRef> _StorageInstanceRefSnapshots;
        /// <MetaDataID>{e38731d1-e451-45af-8426-bf5f92ae2431}</MetaDataID>
        System.Collections.Generic.Dictionary<Transactions.Transaction, PersistenceLayer.StorageInstanceRef> StorageInstanceRefSnapshots
        {
            get
            {
                if (_StorageInstanceRefSnapshots == null)
                    _StorageInstanceRefSnapshots = new System.Collections.Generic.Dictionary<OOAdvantech.Transactions.Transaction, PersistenceLayer.StorageInstanceRef>();
                return _StorageInstanceRefSnapshots;
            }
        }
        /// <MetaDataID>{7CD67107-3A6A-46F9-921C-7C6EA9148529}</MetaDataID>
        Collections.Generic.Dictionary<string, object> Properties = null;

        ///// <MetaDataID>{8af8aee8-53a9-4235-88bf-e32a934e63a8}</MetaDataID>
        //Collections.Generic.Dictionary<string, object> BackUpProperties;
        ///// <MetaDataID>{0bdd8c78-d27f-4f9e-8c3c-1feadbe6dc28}</MetaDataID>
        //PersistenceLayer.StorageInstanceRef BackUpStorageInstanceRef;
        /// <MetaDataID>{1A15B83F-E691-4080-B407-53DF4174DBD8}</MetaDataID>
        void SetProperty(string propertyName, object propertyValue)
        {
            if (Properties == null)
                Properties = new OOAdvantech.Collections.Generic.Dictionary<string, object>();
            if (propertyValue is PersistenceLayer.StorageInstanceRef)
                StorageInstanceRef = propertyValue as PersistenceLayer.StorageInstanceRef;

            if (CreatedFromSerialization)
                throw new OOAdvantech.Transactions.TransactionException("You can't set extension propertie of object which run remotely");
            if (Properties.ContainsKey(propertyName))
                Properties[propertyName] = propertyValue;
            else
                Properties.Add(propertyName, propertyValue);

        }
        /// <MetaDataID>{0B779FAB-6542-4A2E-9B9E-9D4CC1DAA746}</MetaDataID>
        object GetProperty(string propertyName)
        {
            if (Properties == null)
                return null;
            if (Properties.ContainsKey(propertyName))
            {
                object value = Properties[propertyName];
                return value;
            }
            else
                return null;
        }
        /// <MetaDataID>{8AE1E59E-81EC-45D4-B21C-6133EEF05A2D}</MetaDataID>
        void RemoveProperty(string propertyName)
        {
            if (Properties == null)
                return;
            if (Properties.ContainsKey(propertyName))
                Properties.Remove(propertyName);

        }


        #region TransactionalObject Members

        /// <MetaDataID>{dc26729e-50ed-4bbe-a004-0c800b74068a}</MetaDataID>
        public void MergeChanges(OOAdvantech.Transactions.Transaction masterTransaction, OOAdvantech.Transactions.Transaction nestedTransaction)
        {
            lock (this)
            {
                if (!StorageInstanceRefSnapshots.ContainsKey(masterTransaction) && StorageInstanceRefSnapshots.ContainsKey(nestedTransaction))
                    StorageInstanceRefSnapshots[masterTransaction] = StorageInstanceRefSnapshots[nestedTransaction];

                if (!PropertiesValuesSnapshots.ContainsKey(masterTransaction) && PropertiesValuesSnapshots.ContainsKey(nestedTransaction))
                    PropertiesValuesSnapshots[masterTransaction] = PropertiesValuesSnapshots[nestedTransaction];

            }
        }
        /// <MetaDataID>{39ca848c-0e24-4b15-a38a-a1e77ea4d566}</MetaDataID>
        public void EnsureSnapshot(Transaction transaction)
        {
            lock (this)
            {
                if (!StorageInstanceRefSnapshots.ContainsKey(transaction))
                    StorageInstanceRefSnapshots[transaction] = StorageInstanceRef;

                if (!PropertiesValuesSnapshots.ContainsKey(transaction))
                    PropertiesValuesSnapshots[transaction] = Properties;
            }
        }
        /// <MetaDataID>{a54eef2c-991c-44c9-bfc1-47835a2d3892}</MetaDataID>
        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {

            lock (this)
            {
                StorageInstanceRefSnapshots[transaction] = StorageInstanceRef;
                PropertiesValuesSnapshots[transaction] = Properties;
            }
        }

        /// <MetaDataID>{77cc0f9e-a47e-4d01-94a5-b9380dda0523}</MetaDataID>
        public void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {

            lock (this)
            {
                StorageInstanceRef = StorageInstanceRefSnapshots[transaction];
                StorageInstanceRefSnapshots.Remove(transaction);
                Properties = PropertiesValuesSnapshots[transaction];
                PropertiesValuesSnapshots.Remove(transaction);
            }
        }
        /// <MetaDataID>{d9a7397c-d3ac-45bf-b606-d2ffdd8334cf}</MetaDataID>
        public bool ObjectHasGhanged(Transactions.TransactionRunTime transaction)
        {
            lock (this)
            {
                if (StorageInstanceRef != StorageInstanceRefSnapshots[transaction])
                    return true;
                var properties = PropertiesValuesSnapshots[transaction];
                if (Properties == null && properties == null)
                    return false;
                else if ((Properties == null || properties == null) && Properties != properties)
                    return true;
                else if (Properties.Count != properties.Count)
                    return true;
                else
                {
                    foreach (var pair in Properties)
                    {
                        if (!properties.ContainsKey(pair.Key) || properties[pair.Key] != Properties[pair.Key])
                            return true;
                    }
                }
                return false;
            }
        }

        /// <MetaDataID>{0a6739a5-1cb4-4ae2-b896-8edcb87cada6}</MetaDataID>
        public void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {

            lock (this)
            {
                PropertiesValuesSnapshots.Remove(transaction);
                foreach (Transactions.Transaction snapshotTransaction in PropertiesValuesSnapshots.Keys.ToList())
                    PropertiesValuesSnapshots[snapshotTransaction] = Properties;

                StorageInstanceRefSnapshots.Remove(transaction);
                foreach (Transactions.Transaction snapshotTransaction in StorageInstanceRefSnapshots.Keys.ToList())
                    StorageInstanceRefSnapshots[snapshotTransaction] = StorageInstanceRef;
            }

        }

        /// <MetaDataID>{d807394e-dd96-4e8c-885c-04fbed862571}</MetaDataID>
        public void MarkChanges(OOAdvantech.Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            throw new NotImplementedException();

        }

        public event ObjectStateChangedHandle ObjectStateCommited;
        /// <MetaDataID>{3825b993-e8a1-484a-b3a9-a8ef2eda49b4}</MetaDataID>
        public void OnCommitObjectState()
        {
            ObjectStateCommited?.Invoke();
        }

        public event ObjectStateChangedHandle ObjectActivated;
        /// <MetaDataID>{bc0b453c-0b38-4651-9a77-9e3f2e70debe}</MetaDataID>
        public void OnActivate()
        {
            ObjectActivated?.Invoke();
        }

        public event ObjectStateChangedHandle Deleting;
        /// <MetaDataID>{7895b290-a99c-47b2-8fc5-48be4a3db919}</MetaDataID>
        public void OnDeleting()
        {
            Deleting?.Invoke();
        }
        public event LinkedObjectHandle LinkedObjectAdded;
        /// <MetaDataID>{2c367f9a-d0cb-4bda-89d3-6f8d56222925}</MetaDataID>
        void PersistenceLayer.IObjectStateEventsConsumer.LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
            LinkedObjectAdded?.Invoke(linkedObject, associationEnd);
        }

        public event LinkedObjectHandle LinkedObjectRemoved;

        /// <MetaDataID>{eb6eae6f-b34a-4388-8ed7-e78d45ee5f07}</MetaDataID>
        void PersistenceLayer.IObjectStateEventsConsumer.LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
            LinkedObjectRemoved?.Invoke(linkedObject, associationEnd);
        }

        public event ObjectStateChangedHandle BeforeObjectStateCommited;
        /// <MetaDataID>{3b6de877-d302-42d5-91ea-2a5e03ccea11}</MetaDataID>
        public void BeforeCommitObjectState()
        {
            BeforeObjectStateCommited?.Invoke();
        }



        #endregion
    }
}
