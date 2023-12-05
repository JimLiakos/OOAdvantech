using System.Runtime.Remoting.Messaging;
using System.Transactions;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using System.Collections;
using System.Globalization;

namespace OOAdvantech
{
    /// <MetaDataID>{13d8c6b3-06de-42e3-af08-8c8995656ca3}</MetaDataID>
    public interface IMultilingual
    {
        /// <MetaDataID>{81ba0ca0-8063-496d-83b1-7972aaa08bb5}</MetaDataID>
        IDictionary Values { get; }

        /// <MetaDataID>{79b3e6bf-a576-45bf-88ab-9a6f96373ac8}</MetaDataID>
        string Def
        {
            get;
        }
    }
    /// <MetaDataID>{3406db12-480c-4bd9-8340-1da4dda51821}</MetaDataID>
    public interface IMultilingualMember : IMultilingual
    {

    }
    /// <MetaDataID>{58aece88-668c-42b3-82a7-ffee11d8b6e2}</MetaDataID>
    public class MultilingualMember<T> : Member<T>, IMultilingualMember
    {
        System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, T> MultilingualValue = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, T>();
        MultilingualRelationsType MultilingualRelationsType = MultilingualRelationsType.IgnoreDefaultLanguageValues;
        public MultilingualMember(MultilingualRelationsType multilingualRelationsType)
        {
            MultilingualRelationsType = multilingualRelationsType;
        }
        public MultilingualMember()
        {
        }
        public MultilingualMember(IDictionary values) : this(values, MultilingualRelationsType.IgnoreDefaultLanguageValues)
        {

        }
        public MultilingualMember(IMultilingual multilingual) : this(multilingual.Values, MultilingualRelationsType.IgnoreDefaultLanguageValues)
        {
            if (!string.IsNullOrWhiteSpace(multilingual.Def))
                _DefaultLanguage = CultureInfo.GetCultureInfo(multilingual.Def);
        }

        public MultilingualMember(IDictionary values, MultilingualRelationsType multilingualRelationsType)
        {
            MultilingualRelationsType = multilingualRelationsType;

            foreach (System.Collections.DictionaryEntry entry in values)
            {
                CultureInfo culture = entry.Key as System.Globalization.CultureInfo;
                if (culture == null && entry.Key is string)
                    culture = CultureInfo.GetCultureInfo(entry.Key as string);

                if (entry.Value != null)
                    MultilingualValue.Add(culture, (T)entry.Value);
                else
                    MultilingualValue.Add(culture, default(T));
            }
        }
        protected override bool ObjectHasGhanged(OOAdvantech.Transactions.Transaction transaction)
        {
            var snapshotValue = Snapshots[transaction] as System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, T>;
            if (snapshotValue == null)
                return false;

            if (MultilingualValue.Count != snapshotValue.Count)
                return true;

            foreach (var pair in MultilingualValue)
            {
                if (snapshotValue.ContainsKey(pair.Key))
                {

                    if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(MultilingualValue[pair.Key], snapshotValue[pair.Key]))
                        return true;
                }
                else
                    return true;
            }
            return false;
        }


        public override T Value
        {
            get
            {

                if (!_Initialized && RelResolver != null)
                {
                    _Initialized = true;

                    try
                    {
                        PersistenceLayer.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Owner);
                        using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(Transactions.TransactionOption.Suppress))
                        {
                            storageInstanceRef.LazyFetching(RelResolver, Owner.GetType());
                            if (_Snapshots != null && _Snapshots.Count > 0)
                            {
                                foreach (Transactions.Transaction transaction in new System.Collections.Generic.List<Transactions.Transaction>(_Snapshots.Keys))
                                    _Snapshots[transaction] = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, T>(MultilingualValue);
                            }
                            stateTransition.Consistent = true;
                        }
                    }
                    catch (System.Exception error)
                    {
                        _Initialized = false;
                        throw;
                    }
                    _Initialized = true;
                }

                var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
                bool useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;

                if (this.MetaObject is MetaDataRepository.AssociationEnd && MultilingualRelationsType == MultilingualRelationsType.IgnoreDefaultLanguageValues)
                    useDefaultCultureValue = false;

                T value = default(T);
                if (MultilingualValue.TryGetValue(cultureInfo, out value))
                    return value;
                else
                {
                    if (useDefaultCultureValue && Owner != null)
                    {
                        var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Owner);
                        if (objectStorage != null && !string.IsNullOrWhiteSpace(objectStorage.StorageMetaData.Culture))
                        {
                            cultureInfo = OOAdvantech.CultureContext.GetNeutralCultureInfo(objectStorage.StorageMetaData.Culture);
                            if (cultureInfo != null && MultilingualValue.TryGetValue(cultureInfo, out value))
                                return value;
                        }

                    }
                    if (useDefaultCultureValue && DefaultLanguage != null)
                    {
                        if (cultureInfo != null && MultilingualValue.TryGetValue(DefaultLanguage, out value))
                            return value;
                    }
                    return default(T);
                }
            }
            set
            {
                if (value == null)
                {

                }
                var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
                T oldValue;
                MultilingualValue.TryGetValue(cultureInfo, out oldValue);
                MultilingualValue[cultureInfo] = value;

                if (this.MetaObject is Attribute || this.MetaObject is AttributeRealization)
                {
                    Attribute attribute = this.MetaObject as Attribute;
                    if (this.MetaObject is AttributeRealization)
                        attribute = (this.MetaObject as AttributeRealization).Specification;
                    if (attribute != null && attribute.Type is Structure && (attribute.Type as Structure).Persistent)
                    {
                        if (Owner != null)
                        {
                            Class @class = Classifier.GetClassifier(Owner.GetType()) as Class;
                            if (@class.IsPersistent(attribute))
                            {
                                if (!oldValue.Equals(value))
                                {
                                    var storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Owner);
                                    if (storageInstanceRef != null)
                                        storageInstanceRef.InitializeRelationshipResolverForValueType(attribute, cultureInfo);


                                }
                            }
                        }
                    }
                }
                else
                {

                    MetaDataRepository.AssociationEnd associationEnd = null;
                    if (MetaObject is MetaDataRepository.AssociationEnd)
                        associationEnd = MetaObject as MetaDataRepository.AssociationEnd;
                    else if (MetaObject is MetaDataRepository.AssociationEndRealization)
                        associationEnd = (MetaObject as MetaDataRepository.AssociationEndRealization).Specification;
                    if (associationEnd != null && associationEnd.Association.General != null)
                    {
                        MetaDataRepository.AssociationEnd generalAssociationEnd = null;
                        if (associationEnd.IsRoleA)
                            generalAssociationEnd = associationEnd.Association.General.RoleA;
                        else
                            generalAssociationEnd = associationEnd.Association.General.RoleB;

                        if (generalAssociationEnd.Navigable)
                        {
                            if (oldValue != null && !oldValue.Equals(value))
                                Member<object>.RemoveObjectsLinkFastInvoke(null, new object[3] { generalAssociationEnd, Owner, oldValue });
                            if (value != null && !value.Equals(oldValue))
                                Member<object>.AddObjectsLinkFastInvoke(null, new object[3] { generalAssociationEnd, Owner, value });
                        }
                    }

                    if (IsTwoWayNavigableAssociationEnd)
                    {
                        //στην περίπτωση που ο owner είναι struct η σχέση δεν θα είναι ποτέ two way

                        var otherAssociationEnd = associationEnd.GetOtherEnd();

                        if (oldValue != null && !oldValue.Equals(value))
                            RemoveObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, oldValue, Owner });
                        if (value != null && !value.Equals(oldValue))
                            AddObjectsLinkFastInvoke(null, new object[3] { otherAssociationEnd, value, Owner });
                    }


                    if (RelResolver != null && HasReferentialIntegrity)
                    {
                        if (value != null && !value.Equals(oldValue))
                            UpgrateReferencialIntegrityInvoke(RelResolver, new object[2] { value, true });
                        if (oldValue != null && !oldValue.Equals(value))
                            UpgrateReferencialIntegrityInvoke(RelResolver, new object[2] { oldValue, false });
                    }
                }

            }
        }

        public bool HasValue
        {
            get
            {
                var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
                if (MultilingualValue.ContainsKey(cultureInfo))
                    return true;

                bool useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;

                if (useDefaultCultureValue && Owner != null)
                {
                    var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Owner);
                    if (objectStorage != null && !string.IsNullOrWhiteSpace(objectStorage.StorageMetaData.Culture))
                    {
                        cultureInfo = OOAdvantech.CultureContext.GetNeutralCultureInfo(objectStorage.StorageMetaData.Culture);
                        if (cultureInfo != null && MultilingualValue.ContainsKey(cultureInfo))
                            return true;
                    }
                }
                return false;
            }
        }

        public void ClearValue()
        {
            var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
            if (MultilingualValue.ContainsKey(cultureInfo))
                MultilingualValue.Remove(cultureInfo);
        }
        public override bool UnInitialized
        {
            get
            {
                var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
                return !MultilingualValue.ContainsKey(cultureInfo);
            }
        }

        public IDictionary Values
        {
            get
            {
                System.Collections.Hashtable dictionary = new Hashtable();
                foreach (var entry in MultilingualValue)
                    dictionary.Add(entry.Key, entry.Value);
                return dictionary;
            }
        }

        /// <exclude>Excluded</exclude>
        CultureInfo _DefaultLanguage;
        public CultureInfo DefaultLanguage
        {
            get
            {
                if (_DefaultLanguage == null)
                {

                    if (Owner != null)
                    {
                        var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Owner);
                        if (objectStorage != null && !string.IsNullOrWhiteSpace(objectStorage.StorageMetaData.Culture))
                        {
                            _DefaultLanguage = OOAdvantech.CultureContext.GetNeutralCultureInfo(objectStorage.StorageMetaData.Culture);
                            return _DefaultLanguage;
                        }
                        else
                            return OOAdvantech.CultureContext.GetNeutralCultureInfo(CultureInfo.CurrentCulture.Name);
                    }
                    else
                        return OOAdvantech.CultureContext.GetNeutralCultureInfo(CultureInfo.CurrentCulture.Name);
                }
                else
                    return _DefaultLanguage;
            }
        }

        public string Def => DefaultLanguage?.Name;

        public static object GetValue(AccessorBuilder.GetHandler fieldGet, object memberOwner)
        {

            object memberValue = fieldGet(memberOwner);

            if (memberValue is IMultilingualMember)
                return (memberValue as IMultilingualMember).Values;
            if (memberValue is IMember)
                return (memberValue as IMember).Value;
            else
                return memberValue;
        }
        //public override bool Equals(object obj)
        //{
        //    if (obj is MultilingualMember<T>)
        //    {
        //        using (new CultureContext(CultureContext.CurrentCultureInfo,false))
        //        {
        //            return this== (obj as MultilingualMember<T>);
        //        }
        //    }
        //    else
        //        return false;

        //}




        public override void SetValueImplicitly(object value)
        {
            if (MetaObject is MetaDataRepository.AssociationEnd && (MetaObject as MetaDataRepository.AssociationEnd).Name == "Column")
            {

            }
            if (value != null && !(value is T))
                throw new System.Exception("The value isn't type of " + typeof(T).FullName);
            var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
            if (value != null)
            {
                MultilingualValue[cultureInfo] = (T)value;
                _Initialized = true;
                //Value = (T)value;
            }
            else
            {
                MultilingualValue[cultureInfo] = default(T);
                _Initialized = true;
                //Value = default(T);
            }
        }

        public override void SetMetadata(MetaObject metadata)
        {
            base.SetMetadata(metadata);
        }
        public override void SetMetadata(MetaObject metadata, object relResolver)
        {
            //if (relResolver != null)
            //    throw new System.NotSupportedException("Multilingual doesn't supported for the objects relations");
            base.SetMetadata(metadata, relResolver);
        }

        protected override void MarkChanges(Transactions.Transaction transaction)
        {
            lock (this)
            {
                Snapshots[transaction] = MultilingualValue;
                MultilingualValue = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, T>(MultilingualValue);
            }
        }

        protected override void MarkChanges(Transactions.Transaction transaction, System.Reflection.FieldInfo[] fields)
        {
            throw new System.NotImplementedException();
        }

        protected override void CommitChanges(Transactions.Transaction transaction)
        {
            lock (this)
            {
                Snapshots.Remove(transaction);
                foreach (Transactions.Transaction snapshotTransaction in Snapshots.Keys.ToList())
                    Snapshots[snapshotTransaction] = MultilingualValue;
                MultilingualValue = new System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, T>(MultilingualValue);
            }


        }

        protected override void MergeChanges(Transactions.Transaction masterTransaction, Transactions.Transaction nestedTransaction)
        {
            if (!Snapshots.ContainsKey(masterTransaction) && Snapshots.ContainsKey(nestedTransaction))
                Snapshots[masterTransaction] = Snapshots[nestedTransaction];
        }
        protected override void UndoChanges(Transactions.Transaction transaction)
        {
            lock (this)
            {
                object snapshotValue = Snapshots[transaction];
                MultilingualValue = snapshotValue as System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, T>;
                Snapshots.Remove(transaction);
            }
        }




    }

    /// <MetaDataID>{854c53ac-f4fe-49c5-9e5a-0c5f4ba5b9e8}</MetaDataID>
    public enum MultilingualRelationsType
    {
        IgnoreDefaultLanguageValues = 0,
        UseDefaultLanguageValues = 1


    }


}