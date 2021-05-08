using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
#if !NETCompactFramework
using System.Reflection.Emit;
#endif

namespace OOAdvantech
{
    //public interface IAccount<T>
    //{
    //    object MakeTransaction(T newValue);

    //}
    /// <MetaDataID>{99ba3880-601b-4d94-971b-598df39812a6}</MetaDataID>
    //public class MemberAccount<T> : Transactions.TransactionalObject, IMember where T : struct
    //{
    //    T Value;

    //    public MemberAccount(T value)
    //    {
    //        id = 3;
    //        Value = value;
    //    }
    //    public static implicit operator T(MemberAccount<T> p)
    //    {
    //        return p.Value;
    //    }
    //    //public static explicit operator T(T? value)
    //    //{
    //    //    return value.Value;
    //    //}
    //    //public static implicit operator AccountMember<T>(T value)
    //    //{
    //    //    return new AccountMember<T>(value);
    //    //}









    //    public static  MemberAccount<T>  operator +(MemberAccount<T> left, T right)
    //    {
    //        return left.Add(right);
    //    }

    //    private MemberAccount<T> Add(T right)
    //    {

    //        if (typeof(T) == typeof(short))
    //            this.Value = (T)(object)((short)((object)this.Value) + (short)((object)right));
    //        else if (typeof(T) == typeof(int))
    //            this.Value = (T)(object)((int)((object)this.Value) + (int)((object)right));
    //        else if (typeof(T) == typeof(long))
    //            this.Value = (T)(object)((long)((object)this.Value) + (long)((object)right));
    //        else if (typeof(T) == typeof(float))
    //            this.Value = (T)(object)((float)((object)this.Value) + (float)((object)right));
    //        else if (typeof(T) == typeof(double))
    //            this.Value = (T)(object)((double)((object)this.Value) + (double)((object)right));
    //        else if (typeof(T) == typeof(decimal))
    //            this.Value = (T)(object)((decimal)((object)this.Value) + (decimal)((object)right));
    //        else if (typeof(T) == typeof(ushort))
    //            this.Value = (T)(object)((ushort)((object)this.Value) + (ushort)((object)right));
    //        else if (typeof(T) == typeof(uint))
    //            this.Value = (T)(object)((uint)((object)this.Value) + (uint)((object)right));
    //        else if (typeof(T) == typeof(ulong))
    //            this.Value = (T)(object)((ulong)((object)this.Value) + (ulong)((object)right));
    //        else
    //            throw new System.SuppressException("System doesn't support addition for type '" + typeof(T).FullName + "'");

    //        return this;
    //    }

    //    public static MemberAccount<T> operator -(MemberAccount<T> left, T right)
    //    {
    //        return left.Subtract(right);
    //    }
    //    int id;

    //    private MemberAccount<T> Subtract(T right)
    //    {
    //        if (typeof(T) == typeof(short))
    //            this.Value = (T)(object)((short)((object)this.Value) - (short)((object)right));
    //        else if (typeof(T) == typeof(int))
    //            this.Value = (T)(object)((int)((object)this.Value) - (int)((object)right));
    //        else if (typeof(T) == typeof(long))
    //            this.Value = (T)(object)((long)((object)this.Value) - (long)((object)right));
    //        else if (typeof(T) == typeof(float))
    //            this.Value = (T)(object)((float)((object)this.Value) - (float)((object)right));
    //        else if (typeof(T) == typeof(double))
    //            this.Value = (T)(object)((double)((object)this.Value) - (double)((object)right));
    //        else if (typeof(T) == typeof(decimal))
    //            this.Value = (T)(object)((decimal)((object)this.Value) - (decimal)((object)right));
    //        else if (typeof(T) == typeof(ushort))
    //            this.Value = (T)(object)((ushort)((object)this.Value) - (ushort)((object)right));
    //        else if (typeof(T) == typeof(uint))
    //            this.Value = (T)(object)((uint)((object)this.Value) - (uint)((object)right));
    //        else if (typeof(T) == typeof(ulong))
    //            this.Value = (T)(object)((ulong)((object)this.Value) - (ulong)((object)right)); 
    //        else
    //            throw new System.NotSupportedException("System doesn't support substraction for type '" + typeof(T).FullName + "'");
    //        return this;
    //    }


    //    #region TransactionalObject Members

    //    public void MergeChanges(OOAdvantech.Transactions.Transaction masterTransaction, OOAdvantech.Transactions.Transaction nestedTransaction)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void MarkChanges(OOAdvantech.Transactions.Transaction transaction, FieldInfo[] fields)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion

    //    #region IMember Members

    //    object IMember.Value
    //    {
    //        get
    //        {
    //            return Value;
    //        }
    //        set
    //        {
    //            if (value != null && !(value is T))
    //                throw new System.Exception("The value isn't type of " + typeof(T).FullName);
    //            if (value != null)
    //                Value = (T)value;
    //            else
    //                Value = default(T);
    //        }
    //    }

    //    void IMember.SetValueImplicitly(object value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion
    //}
    public interface IMember
    {
        /// <MetaDataID>{f216dc85-58be-44f7-b1b9-ea63a4a44a95}</MetaDataID>
        object Value
        {
            get;
            set;
        }
        /// <MetaDataID>{1882dc3a-b154-49b4-929d-444384da12d9}</MetaDataID>
        void SetValueImplicitly(object value);
    }

    /// <MetaDataID>{7e7e2e40-6ce0-4ab7-b870-05bbda699f2f}</MetaDataID>
    internal interface ICollectionMember
    {
        /// <MetaDataID>{e3edfc9e-ef82-40d9-b11b-f639bef2771b}</MetaDataID>
        void AddImplicitly(object _object);
        /// <MetaDataID>{0f6f26c4-dc1a-408d-bd72-02d3e670c009}</MetaDataID>
        void RemoveImplicitly(object _object);
        /// <MetaDataID>{427acadc-1433-41b7-988b-305b924fb175}</MetaDataID>
        System.Collections.IEnumerator GetEnumeretor();
    }
    /// <MetaDataID>{c199c5d7-fd23-4371-b32c-d77e8f2cd2c1}</MetaDataID>
    interface IMemberInitialization
    {
        /// <MetaDataID>{4cdbc362-7761-4624-854f-95a91c4e87f1}</MetaDataID>
        void SetOwner(object owner);
        /// <MetaDataID>{61aa96a4-79a6-430f-bd28-050985d6a064}</MetaDataID>
        void SetMetadata(MetaDataRepository.IMetaObject metadata);
        /// <MetaDataID>{a949517a-dacd-48cc-9451-a04180e31933}</MetaDataID>
        void SetMetadata(MetaDataRepository.IMetaObject metadata, object relResolver);

        /// <MetaDataID>{e153b7b1-d12a-44ef-b063-989923fc8bbe}</MetaDataID>
        bool Initialized
        {
            get;
        }

    }
    /// <MetaDataID>{0d6b9e75-a598-40bb-b9c6-b94451295b19}</MetaDataID>
    [OOAdvantech.Transactions.ContainByValue]
    [Serializable]
    public class Member<T> : IMember, IMemberInitialization, Transactions.ITransactionalObject
    {

        /// <MetaDataID>{7f7c6bbb-ae0d-4415-bb53-395815445e6c}</MetaDataID>
        internal object Owner;

        /// <MetaDataID>{37eb8551-92d2-4e5c-9172-ab9e492ef950}</MetaDataID>
        public Member(T value)
        {

            _Initialized = true;
            _Value = value;

        }
        /// <MetaDataID>{f3b64eae-b752-4450-8e1d-91fa274f44f3}</MetaDataID>
        public Member()
        {
        }
        /// <MetaDataID>{8313416a-b52a-4c73-ab14-69aee153377a}</MetaDataID>
        public static implicit operator T(Member<T> p)
        {
            return p.Value;
        }
        




        ///<exclude>Excluded</exclude>
        protected T _Value;
        /// <MetaDataID>{2edee2aa-8bea-445d-b79f-b1369e044492}</MetaDataID>
        public virtual T Value
        {
            get
            {
                if (!_Initialized && RelResolver!=null)
                {
                    PersistenceLayer.StorageInstanceRef storageInstanceRef = PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(Owner);

                    using (Transactions.SystemStateTransition stateTransition = new Transactions.SystemStateTransition(Transactions.TransactionOption.Suppress))
                    {
                        storageInstanceRef.LazyFetching(RelResolver, Owner.GetType());
                        if (_Snapshots != null && _Snapshots.Count>0)
                        {
                            foreach (Transactions.Transaction transaction in new List<Transactions.Transaction>(_Snapshots.Keys))
                                _Snapshots[transaction] = _Value;
                        }
                        stateTransition.Consistent = true;
                    }
        
                    _Initialized = true;
                }
                return _Value;
            }
            set
            {
                T oldValue = _Value;
                _Value = value;
                _Initialized = true;
                if (IsTwoWayNavigableAssociationEnd)
                {
                    //στην περίπτωση που ο owner είναι struct η σχέση δεν θα είναι ποτέ two way
                    MetaDataRepository.AssociationEnd associationEnd = null;
                    if (MetaObject is MetaDataRepository.AssociationEnd)
                        associationEnd = MetaObject as MetaDataRepository.AssociationEnd;
                    else
                        associationEnd = (MetaObject as MetaDataRepository.AssociationEndRealization).Specification;
                    associationEnd = associationEnd.GetOtherEnd();

                    if (oldValue != null && !oldValue.Equals(_Value))
                        RemoveObjectsLinkFastInvoke(null, new object[3] { associationEnd, oldValue, Owner });
                    if (_Value != null && !_Value.Equals(oldValue))
                        AddObjectsLinkFastInvoke(null, new object[3] { associationEnd, _Value, Owner });

                }


                if (RelResolver != null)
                {
                    if (_Value != null && !_Value.Equals(oldValue))
                        UpgrateReferencialIntegrityInvoke(RelResolver, new object[2] { _Value, true });
                    if (oldValue != null && !oldValue.Equals(_Value))
                        UpgrateReferencialIntegrityInvoke(RelResolver, new object[2] { oldValue, false });
                }
            }

        }


        /// <MetaDataID>{f10eca5f-3fa1-47e8-8a06-e5458d18ae1a}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler _UpgrateReferencialIntegrityInvoke;
        /// <MetaDataID>{55357f6b-8846-47cb-b43b-5fb118692aa8}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler UpgrateReferencialIntegrityInvoke
        {
            get
            {
#if Net4

                if (_UpgrateReferencialIntegrityInvoke == null)
                    _UpgrateReferencialIntegrityInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.RelResolver",  "PersistenceLayerRunTime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b").GetMethod("UpgrateReferencialIntegrity", AccessorBuilder.AllMembers));
                return _UpgrateReferencialIntegrityInvoke; ;
                
#else
                if (_UpgrateReferencialIntegrityInvoke == null)
                    _UpgrateReferencialIntegrityInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.RelResolver", "PersistenceLayerRunTime, Culture=neutral, PublicKeyToken=8cd83e51ed2de794").GetMethod("UpgrateReferencialIntegrity", AccessorBuilder.AllMembers));
                return _UpgrateReferencialIntegrityInvoke;
#endif
            }
        }


        /// <MetaDataID>{07e4f060-67bd-4b81-bfff-6394df9f60ee}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler _AddObjectsLinkFastInvoke;
        /// <MetaDataID>{e4844438-9157-4f57-a33f-714067e76eb0}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler AddObjectsLinkFastInvoke
        {
            get
            {
#if Net4
                if (_AddObjectsLinkFastInvoke == null)
                    _AddObjectsLinkFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.AssociationEnd", "DotNetMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=00a88b51a86dbd3c").GetMethod("AddObjectsLink", AccessorBuilder.AllMembers));
                return _AddObjectsLinkFastInvoke;
#else
                if (_AddObjectsLinkFastInvoke == null)
                    _AddObjectsLinkFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.AssociationEnd", "DotNetMetaDataRepository, Culture=neutral, PublicKeyToken=11a79ce55c18c4e7").GetMethod("AddObjectsLink", AccessorBuilder.AllMembers));
                return _AddObjectsLinkFastInvoke;

#endif
            }
        }

        /// <MetaDataID>{c1427a88-5881-4d26-9a53-4479c886fde2}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler _RemoveObjectsLinkFastInvoke;
        /// <MetaDataID>{5b8271d4-fbc6-498a-b9fe-058629149ab8}</MetaDataID>
        static AccessorBuilder.FastInvokeHandler RemoveObjectsLinkFastInvoke
        {
            get
            {
#if Net4
                if (_RemoveObjectsLinkFastInvoke == null)
                    _RemoveObjectsLinkFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.AssociationEnd", "DotNetMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=00a88b51a86dbd3c").GetMethod("RemoveObjectsLink", AccessorBuilder.AllMembers));
                return _RemoveObjectsLinkFastInvoke;
#else
                if (_RemoveObjectsLinkFastInvoke == null)
                    _RemoveObjectsLinkFastInvoke = AccessorBuilder.GetMethodInvoker(ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.AssociationEnd", "DotNetMetaDataRepository, Culture=neutral, PublicKeyToken=11a79ce55c18c4e7").GetMethod("RemoveObjectsLink", AccessorBuilder.AllMembers));
                return _RemoveObjectsLinkFastInvoke;

#endif
            }
        }


        ///<exclude>Excluded</exclude>
        bool _Initialized;
        /// <MetaDataID>{e174e31f-a447-4651-83bc-db907cf11139}</MetaDataID>
        public bool UnInitialized
        {
            get
            {
                return !_Initialized;
            }
        }




        /// <MetaDataID>{799331b9-da02-4e81-91f8-7950c151e8df}</MetaDataID>
        public static void SetValue(AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, ref object memberOwner, object value)
        {
            //if (fieldInfo.FieldType.GetInterface(typeof(OOAdvantech.IMember).FullName) == typeof(OOAdvantech.IMember))
            if (fastFieldAccessor.IsMember)
            {

                IMember memberObject = fastFieldAccessor.GetValue(memberOwner) as IMember;
                memberObject.Value = value;
                //fastFieldAccessor.SetValue(memberOwner, memberObject);
            }
            else
                memberOwner = fastFieldAccessor.SetValue(memberOwner, value);
        }
        /// <MetaDataID>{61a79039-ad33-41e1-a40d-a93543866ff5}</MetaDataID>
        public static void SetValueImplicitly(AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, ref object memberOwner, object value)
        {
            //if (fieldInfo.FieldType.GetInterface(typeof(OOAdvantech.IMember).FullName) == typeof(OOAdvantech.IMember))
            if (fastFieldAccessor.IsMember)
            {

                IMember memberObject = fastFieldAccessor.GetValue(memberOwner) as IMember;
                memberObject.SetValueImplicitly(value);
                //fastFieldAccessor.SetValue(memberOwner, memberObject);
            }
            else
            {
                memberOwner = fastFieldAccessor.SetValue(memberOwner, value);
            }
        }

        /// <MetaDataID>{e51863a6-8be5-4e78-9b31-f5006854b030}</MetaDataID>
        public static object GetValue(AccessorBuilder.GetHandler fieldGet, object memberOwner)
        {
            object memberValue = fieldGet(memberOwner);
            if (memberValue is OOAdvantech.IMember)
                return (memberValue as OOAdvantech.IMember).Value;
            else
                return memberValue;
        }







        /// <MetaDataID>{77abfbe5-623c-4d7e-a078-3418ec0984d6}</MetaDataID>
        object IMember.Value
        {
            get
            {
                return Value;
            }
            set
            {
                if (value != null && !(value is T))
                    throw new System.Exception("The value isn't type of " + typeof(T).FullName);
                if (value != null)
                    Value = (T)value;
                else
                    Value = default(T);
                this._Initialized = true;

            }
        }
        /// <MetaDataID>{caa80c5e-4566-4089-8347-f5c2d9ae577f}</MetaDataID>
        void IMember.SetValueImplicitly(object value)
        {
            if (value != null && !(value is T))
                throw new System.Exception("The value isn't type of " + typeof(T).FullName);
            if (value != null)
            {
                T oldValue = _Value;
                _Value = (T)value;
                if (RelResolver != null)
                {
                    if (_Value != null && !_Value.Equals(oldValue))
                        UpgrateReferencialIntegrityInvoke(RelResolver, new object[2] { _Value, true });
                    if (oldValue != null && !oldValue.Equals(_Value))
                        UpgrateReferencialIntegrityInvoke(RelResolver, new object[2] { oldValue, false });
                }
                _Initialized = true;
                //Value = (T)value;
            }
            else
            {
                T oldValue = _Value;
                _Value = (T)value;
                if (RelResolver != null)
                {
                    if (_Value != null && !_Value.Equals(oldValue))
                        UpgrateReferencialIntegrityInvoke(RelResolver, new object[2] { _Value, true });
                    if (oldValue != null && !oldValue.Equals(_Value))
                        UpgrateReferencialIntegrityInvoke(RelResolver, new object[2] { oldValue, false });
                }
                _Initialized = true;
                //Value = default(T);
            }


        }

        /// <MetaDataID>{b3c3ff2c-e411-49fd-b5d3-b57eb5b80c88}</MetaDataID>
        void IMemberInitialization.SetOwner(object owner)
        {
            Owner = owner;
            //OwnerStorageInstanceRef = storageInstanceRef;
        }

        /// <MetaDataID>{556c387e-3b0f-4bb5-9133-827085f8931d}</MetaDataID>
        bool IsTwoWayNavigableAssociationEnd;
        /// <MetaDataID>{69df841b-c74d-4216-8b08-84e5b63b2bec}</MetaDataID>
        [NonSerialized]
        MetaDataRepository.IMetaObject MetaObject;
        /// <MetaDataID>{092df979-faab-42a0-8eb3-cde82b7fcaf2}</MetaDataID>
        void IMemberInitialization.SetMetadata(MetaDataRepository.IMetaObject metadata)
        {
            MetaObject = metadata;
            if (MetaObject is MetaDataRepository.AssociationEnd && (MetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Navigable)
                IsTwoWayNavigableAssociationEnd = true;
            else if (MetaObject is MetaDataRepository.AssociationEndRealization && (MetaObject as MetaDataRepository.AssociationEndRealization).Specification.GetOtherEnd().Navigable)
                IsTwoWayNavigableAssociationEnd = true;
            else
                IsTwoWayNavigableAssociationEnd = false;
        }
        /// <MetaDataID>{275a2ef5-79b2-470d-bd68-bacf9cdad5e1}</MetaDataID>
        object RelResolver;

        /// <MetaDataID>{99f047e0-d167-4009-b2da-0892879beea9}</MetaDataID>
        void IMemberInitialization.SetMetadata(MetaDataRepository.IMetaObject metadata, object relResolver)
        {
            RelResolver = relResolver;
            MetaObject = metadata;
            if (MetaObject is MetaDataRepository.AssociationEnd && (MetaObject as MetaDataRepository.AssociationEnd).GetOtherEnd().Navigable)
                IsTwoWayNavigableAssociationEnd = true;
            else if (MetaObject is MetaDataRepository.AssociationEndRealization && (MetaObject as MetaDataRepository.AssociationEndRealization).Specification.GetOtherEnd().Navigable)
                IsTwoWayNavigableAssociationEnd = true;
            else
                IsTwoWayNavigableAssociationEnd = false;
        }

        /// <MetaDataID>{389d911a-a862-4522-b40d-8ef4e40e3a23}</MetaDataID>
        bool IMemberInitialization.Initialized
        {
            get
            {
                return Owner != null;
            }
        }


        /// <MetaDataID>{3b0427ed-b8da-4495-a822-a1f02ec481c9}</MetaDataID>
        internal static void InitValue(object MemoryInstance, AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, MetaDataRepository.IMetaObject metadata )
        {
            InitValue(MemoryInstance, fastFieldAccessor, metadata,null);
        }
        
        /// <MetaDataID>{cfe7909d-7b00-434b-a80d-242db84612d1}</MetaDataID>
        internal static void InitValue(object MemoryInstance, AccessorBuilder.FieldPropertyAccessor fastFieldAccessor, MetaDataRepository.IMetaObject metadata, object relResolver)
        {
            if (fastFieldAccessor.InitializationRequired)
            {
                
                IMemberInitialization memberObject = fastFieldAccessor.GetValue(MemoryInstance) as IMemberInitialization;
                if (memberObject == null)
                {
                    Type memberType = null;
                    if (fastFieldAccessor.MemberInfo is FieldInfo)
                        memberType = (fastFieldAccessor.MemberInfo as FieldInfo).FieldType;
                    else
                        memberType = (fastFieldAccessor.MemberInfo as PropertyInfo).PropertyType;


                    memberObject = AccessorBuilder.CreateInstance(memberType) as OOAdvantech.IMemberInitialization;

                    Transactions.Transaction transaction=Transactions.Transaction.Current;
                    if (memberObject is Transactions.ITransactionalObject && transaction != null)
                        (memberObject as Transactions.ITransactionalObject).MarkChanges(transaction);

                    fastFieldAccessor.SetValue(MemoryInstance, memberObject);
                }
                memberObject.SetOwner(MemoryInstance);
                //relResolver;
                memberObject.SetMetadata(metadata,relResolver);
            }
        }
        struct MemberType
        {
            public bool IsMember;
            public bool InitializationRequired;
        }

        /// <MetaDataID>{6017046c-3433-4f46-b5a1-046246d124e4}</MetaDataID>
        static System.Collections.Generic.Dictionary<Type, MemberType> MemberTypes = new Dictionary<Type, MemberType>();
        /// <MetaDataID>{66e26707-30dd-4b0f-b59d-7134697be696}</MetaDataID>
        internal static bool IsMember(Type type)
        {
            MemberType memberType;
            if (MemberTypes.TryGetValue(type, out memberType))
                return memberType.IsMember;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Member<>))
            {
                memberType.IsMember = true;
                memberType.InitializationRequired = true;
                MemberTypes[type] = memberType;
                return true;
            }
            else
            {
                memberType.IsMember = false;
                memberType.InitializationRequired = false;
                foreach (Type _interface in type.GetInterfaces())
                {
                    if (_interface == typeof(IMemberInitialization))
                    {
                        memberType.InitializationRequired = true;

                    }
                    if (_interface == typeof(IMember))
                    {
                        memberType.IsMember = true;
                    }
                    if (memberType.InitializationRequired || memberType.IsMember)
                        MemberTypes[type] = memberType;

                    if (memberType.InitializationRequired && memberType.IsMember)
                        break;
                }
                return memberType.IsMember;
            }
        }

        /// <MetaDataID>{f6bdf601-112f-4a1f-bb78-5c07cdb0074a}</MetaDataID>
        internal static bool InitializationRequired(Type type)
        {
            MemberType memberType;
            if (MemberTypes.TryGetValue(type, out memberType))
                return memberType.InitializationRequired;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Member<>))
            {
                memberType.IsMember = true;
                memberType.InitializationRequired = true;
                MemberTypes[type] = memberType;
                return true;
            }
            else
            {
                memberType.IsMember = false;
                memberType.InitializationRequired = false;
                foreach (Type _interface in type.GetInterfaces())
                {
                    if (_interface == typeof(IMemberInitialization))
                    {
                        memberType.InitializationRequired = true;
                     
                    }
                    if (_interface == typeof(IMember))
                    {
                        memberType.IsMember = true;
                    }
                    if(memberType.InitializationRequired || memberType.IsMember)
                        MemberTypes[type] = memberType;

                    if (memberType.InitializationRequired && memberType.IsMember)
                        break;
                }
                return memberType.InitializationRequired;
            }
        }





        /// <MetaDataID>{99da7950-2c6f-4823-b10d-c0b45eef5cd5}</MetaDataID>
        void OOAdvantech.Transactions.ITransactionalObject.MergeChanges(OOAdvantech.Transactions.Transaction masterTransaction, OOAdvantech.Transactions.Transaction nestedTransaction)
        {
            MergeChanges(masterTransaction, nestedTransaction);

        }

        /// <MetaDataID>{461b3ce1-9f9f-46b1-99aa-1e6fd9a6acf5}</MetaDataID>
        void OOAdvantech.Transactions.ITransactionalObject.MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            MarkChanges(transaction);
        }

        /// <MetaDataID>{baf791ef-259f-4fbe-99ac-fab93a3505c1}</MetaDataID>
        void OOAdvantech.Transactions.ITransactionalObject.UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            UndoChanges(transaction);
        }

        /// <MetaDataID>{892dd830-3b70-4140-bb41-201a6b9be767}</MetaDataID>
        void OOAdvantech.Transactions.ITransactionalObject.CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            CommitChanges(transaction);
        }

        /// <MetaDataID>{e2385dd4-23af-4f6c-b713-8d3761a578de}</MetaDataID>
        void OOAdvantech.Transactions.ITransactionalObject.MarkChanges(OOAdvantech.Transactions.Transaction transaction, FieldInfo[] fields)
        {
            MarkChanges(transaction, fields);
        }



        /// <MetaDataID>{afbfa845-b918-4a02-a3bd-6a1d2e06c46e}</MetaDataID>
        protected virtual void MergeChanges(OOAdvantech.Transactions.Transaction masterTransaction, OOAdvantech.Transactions.Transaction nestedTransaction)
        {
            if (!Snapshots.ContainsKey(masterTransaction) && Snapshots.ContainsKey(nestedTransaction))
                Snapshots[masterTransaction] = Snapshots[nestedTransaction];

        }
        /// <MetaDataID>{ca660ec6-cd0a-4676-97d8-bd204909fe0b}</MetaDataID>
        System.Collections.Generic.Dictionary<Transactions.Transaction, object> _Snapshots;
        /// <MetaDataID>{e38731d1-e451-45af-8426-bf5f92ae2431}</MetaDataID>
        System.Collections.Generic.Dictionary<Transactions.Transaction, object> Snapshots
        {
            get
            {
                if (_Snapshots == null)
                    _Snapshots = new Dictionary<OOAdvantech.Transactions.Transaction, object>();
                return _Snapshots;
            }
        }


        /// <MetaDataID>{0518dcdb-001a-4060-91ba-cd30b0a6dd72}</MetaDataID>
        protected virtual void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            lock (this)
            {
                Snapshots[transaction] = _Value;
            }

        }

        /// <MetaDataID>{50fddbf8-816d-4242-99ca-58d4904b6bf6}</MetaDataID>
        protected virtual void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            lock (this)
            {
                object snapshotValue = Snapshots[transaction];
                if (typeof(T).IsValueType || snapshotValue != null)
                    _Value = (T)snapshotValue;
                else
                    _Value = default(T);

                Snapshots.Remove(transaction);
            }
        }

        /// <MetaDataID>{3ecc0201-0749-4c7d-a29b-89554a2c1d52}</MetaDataID>
        protected virtual void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            lock (this)
            {
                Snapshots.Remove(transaction);
                foreach (Transactions.Transaction snapshotTransaction in new System.Collections.ArrayList(Snapshots.Keys))
                    Snapshots[snapshotTransaction] = _Value;


            }


        }

        /// <MetaDataID>{b55e6230-00e3-4a00-8402-63484ac45549}</MetaDataID>
        protected virtual void MarkChanges(OOAdvantech.Transactions.Transaction transaction, FieldInfo[] fields)
        {
            throw new NotImplementedException();
        }


    }

    /// <MetaDataID>{3cd4d5f9-7f94-4e74-9d95-4c96792e68e9}</MetaDataID>
    public interface IAccount<T>
    {
        /// <MetaDataID>{c4b50238-f346-4718-94a4-0a404afbc3a9}</MetaDataID>
        object GetTransaction(T newValue);
        /// <MetaDataID>{e51f3e43-292e-4a08-8172-52424359a27c}</MetaDataID>
        T MakeTransaction(object transaction);
        //T MakeTransaction(object transaction);
    }


    /// <MetaDataID>{1a3ad299-e2ac-4b55-833b-b3bf12477c82}</MetaDataID>
    [Serializable]
    public class MemberAcount<T> : Member<T> where T : struct
    {

        /// <MetaDataID>{b12b523c-33f9-4779-a014-6c9783fd32b6}</MetaDataID>
        public MemberAcount()
        {

        }
        /// <MetaDataID>{e1f04a26-2a7b-466b-bc21-d6a959a02c76}</MetaDataID>
        public MemberAcount(T value)
            : base(value)
        {

        }

        object RollBackValue;


        public override T Value
        {
            get
            {

                if (Transactions.Transaction.Current != null)
                {
                    List<object> accountTransactions = null;
                    if (AccountTransactions.TryGetValue(Transactions.Transaction.Current, out accountTransactions))
                    {
                        T tempValue = _Value;
                        foreach (object accountTransaction in accountTransactions)
                            tempValue = ((IAccount<T>)tempValue).MakeTransaction(accountTransaction);

                        Transactions.Transaction originTransaction = Transactions.Transaction.Current.OriginTransaction;
                        while (originTransaction != null)
                        {
                            if (AccountTransactions.TryGetValue(originTransaction, out accountTransactions))
                            {
                                foreach (object accountTransaction in accountTransactions)
                                    tempValue = ((IAccount<T>)tempValue).MakeTransaction(accountTransaction);
                            }
                            originTransaction = originTransaction.OriginTransaction;
                        }
                        return tempValue;
                    }

                }

                return base.Value;
            }
            set
            {

                if (Transactions.Transaction.Current != null)
                {

                    List<object> accountTransactions = null;
                    if (!AccountTransactions.TryGetValue(Transactions.Transaction.Current, out accountTransactions))
                        throw new System.Exception("Use the ObjectStateTransition scoop before change the member value");
                    object accountTransaction = ((IAccount<T>)Value).GetTransaction(value);

                    if (accountTransaction == null)
                    {
                        //TODO Θα ήταν καλλυτερα το exclusive lock να γινεται πάνω στο field
                        Transactions.LockedObjectEntry.ExclusiveLock(Owner, (Transactions.Transaction.Current as Transactions.TransactionRunTime).TransactionContext);
                        RollBackValue = _Value;
                        _Value = value;
                        accountTransactions.Clear();
                    }
                    else
                        accountTransactions.Add(accountTransaction);
                }
                else
                    _Value = value;


            }
        }
        System.Collections.Generic.Dictionary<Transactions.Transaction, List<object>> _AccountTransactions;
        System.Collections.Generic.Dictionary<Transactions.Transaction, List<object>> AccountTransactions
        {
            get
            {
                if (_AccountTransactions == null)
                    _AccountTransactions = new Dictionary<OOAdvantech.Transactions.Transaction, List<object>>();
                return _AccountTransactions;
            }
        }

        /// <MetaDataID>{3d96e839-6b0d-40f8-b700-84e66b8e0cf9}</MetaDataID>
        protected override void CommitChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            RollBackValue = null;
            List<object> accountTransactions = AccountTransactions[transaction];
            foreach (object accountTransaction in accountTransactions)
                _Value = ((IAccount<T>)_Value).MakeTransaction(accountTransaction);


            AccountTransactions.Remove(transaction);


        }
        /// <MetaDataID>{5da0c989-7988-4680-bd31-8c3a9d894c52}</MetaDataID>
        protected override void MarkChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            if(!AccountTransactions.ContainsKey(transaction))
                AccountTransactions[transaction] = new List<object>();

        }
        /// <MetaDataID>{77aea38c-7871-4c5b-a85e-f0015d4a891e}</MetaDataID>
        protected override void MarkChanges(OOAdvantech.Transactions.Transaction transaction, FieldInfo[] fields)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{39510303-93dc-49e7-88c3-68ed158c1d78}</MetaDataID>
        protected override void MergeChanges(OOAdvantech.Transactions.Transaction masterTransaction, OOAdvantech.Transactions.Transaction nestedTransaction)
        {

            List<object> accountTransactions = AccountTransactions[nestedTransaction];
            List<object> masterAccountTransactions = null;
            if (!AccountTransactions.TryGetValue(masterTransaction, out masterAccountTransactions))
            {
                masterAccountTransactions = new List<object>();
                AccountTransactions.Add(masterTransaction, masterAccountTransactions);
            }
            foreach (object accountTransaction in accountTransactions)
                masterAccountTransactions.Add(accountTransaction);
            AccountTransactions.Remove(nestedTransaction);

        }
        /// <MetaDataID>{c4b670bb-0911-41a3-81af-b9dc82622d0e}</MetaDataID>
        protected override void UndoChanges(OOAdvantech.Transactions.Transaction transaction)
        {
            if (RollBackValue != null)
                _Value = (T)RollBackValue;
            RollBackValue = null;
            AccountTransactions.Remove(transaction);
            //List<object> accountTransactions = AccountTransactions[transaction];
            //foreach (object accountTransaction in accountTransactions)
            //    _Value=((IAccount<T>)_Value).CancelTransaction(accountTransaction);
        }



    }




}
