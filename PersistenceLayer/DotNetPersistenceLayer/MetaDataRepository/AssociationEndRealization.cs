using System;
using System.Linq;
using OOAdvantech.Transactions;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{9BCE1F3A-0A8F-4BD2-A33D-0E6A9AB821B6}</MetaDataID>
    [BackwardCompatibilityID("{9BCE1F3A-0A8F-4BD2-A33D-0E6A9AB821B6}")]
    [Persistent()]
    public class AssociationEndRealization : Feature
    {

        /// <exclude>Excluded</exclude>
        bool _Multilingual;

        /// <MetaDataID>{6f832906-d633-4bcc-8ffe-70192d9ce63b}</MetaDataID>
        /// <summary>Declare that the value of attribute of the objects will be Multilingual. </summary>
        [PersistentMember(nameof(_Multilingual))]
        [BackwardCompatibilityID("+3")]
        public virtual bool Multilingual
        {
            get => _Multilingual;
            set
            {
                if (_Multilingual != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Multilingual = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{53dee44c-2785-4daa-b73e-1956214e73ec}</MetaDataID>
        public virtual object GetObjectStateValue(object _object)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c7402f95-2f17-4b01-bed3-fb3bc1b7124f}</MetaDataID>
        public virtual void SetObjectStateValue(object _object, object value)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{a41e8e2c-d73a-414d-ae5a-d6c34823d1c3}</MetaDataID>
        public virtual object GetValue(object _object)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{914ab520-2629-4374-b0da-5fa4c17127de}</MetaDataID>
        public virtual void SetValue(object _object, object value)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{cfbb9093-78db-484b-9d9d-fd24b388b7b3}</MetaDataID>
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_AllowTransient))
            {
                if (value == null)
                    _AllowTransient = default(bool);
                else
                    _AllowTransient = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_TryOnObjectActivationFetching))
            {
                if (value == null)
                    _TryOnObjectActivationFetching = default(bool);
                else
                    _TryOnObjectActivationFetching = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(TransactionalMember))
            {
                if (value == null)
                    TransactionalMember = default(bool);
                else
                    TransactionalMember = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Persistent))
            {
                if (value == null)
                    _Persistent = default(bool);
                else
                    _Persistent = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_HasBehavioralSettings))
            {
                if (value == null)
                    _HasBehavioralSettings = default(bool);
                else
                    _HasBehavioralSettings = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_CascadeDelete))
            {
                if (value == null)
                    _CascadeDelete = default(bool);
                else
                    _CascadeDelete = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ReferentialIntegrity))
            {
                if (value == null)
                    _ReferentialIntegrity = default(bool);
                else
                    _ReferentialIntegrity = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_LazyFetching))
            {
                if (value == null)
                    _LazyFetching = default(bool);
                else
                    _LazyFetching = (bool)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Specification))
            {
                if (value == null)
                    _Specification = default(OOAdvantech.MetaDataRepository.AssociationEnd);
                else
                    _Specification = (OOAdvantech.MetaDataRepository.AssociationEnd)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        /// <MetaDataID>{644810a6-373a-4266-99ee-2088d419ab55}</MetaDataID>
        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_AllowTransient))
                return _AllowTransient;

            if (member.Name == nameof(_TryOnObjectActivationFetching))
                return _TryOnObjectActivationFetching;

            if (member.Name == nameof(TransactionalMember))
                return TransactionalMember;

            if (member.Name == nameof(_Persistent))
                return _Persistent;

            if (member.Name == nameof(_HasBehavioralSettings))
                return _HasBehavioralSettings;

            if (member.Name == nameof(_CascadeDelete))
                return _CascadeDelete;

            if (member.Name == nameof(_ReferentialIntegrity))
                return _ReferentialIntegrity;

            if (member.Name == nameof(_LazyFetching))
                return _LazyFetching;

            if (member.Name == nameof(_Specification))
                return _Specification;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        protected bool _AllowTransient;
        /// <MetaDataID>{9e016674-dc76-43dd-9157-832fea887b9e}</MetaDataID>
        [PersistentMember("_AllowTransient"), BackwardCompatibilityID("+22")]
        public bool AllowTransient
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    if (!_HasBehavioralSettings)
                        throw new System.NotSupportedException("It hasn't BehavioralSettings");
                    return _AllowTransient;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    _AllowTransient = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <exclude>Excluded</exclude>
        protected bool _TryOnObjectActivationFetching;
        /// <MetaDataID>{de4eed86-95fc-40a4-ae58-cfbb72c29bb7}</MetaDataID>
        [BackwardCompatibilityID("+21")]
        [PersistentMember("_TryOnObjectActivationFetching")]
        public bool TryOnObjectActivationFetching
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _TryOnObjectActivationFetching;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {

                    _TryOnObjectActivationFetching = true;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }
        /// <MetaDataID>{56aadd57-71e7-4c67-9d13-30f748f9fadd}</MetaDataID>
        protected AssociationEndRealization()
        {

        }

        /// <MetaDataID>{2af9cc69-e611-4235-afcb-c0b74a38146e}</MetaDataID>
        public AssociationEndRealization(AssociationEnd specification)
        {
            _Specification = specification;
            specification.AddAssociationEndRealization(this);
        }
        /// <MetaDataID>{9f0ce11b-fbc9-4dac-8c1f-f725934bb325}</MetaDataID>
        public bool TransactionalMember;

        /// <MetaDataID>{517D7810-0D86-4681-BAD2-2802EF693B0B}</MetaDataID>
        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        public override void Synchronize(MetaObject OriginMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    base.Synchronize(OriginMetaObject);
                    AssociationEndRealization originAssociationEndRealization = (AssociationEndRealization)OriginMetaObject;
                    _Persistent = originAssociationEndRealization.Persistent;
                    _HasBehavioralSettings = originAssociationEndRealization.HasBehavioralSettings;
                    if (_HasBehavioralSettings)
                    {
                        _LazyFetching = originAssociationEndRealization.LazyFetching;
                        _CascadeDelete = originAssociationEndRealization.CascadeDelete;
                        _ReferentialIntegrity = originAssociationEndRealization.ReferentialIntegrity;
                    }

                    if (originAssociationEndRealization.Specification != null)
                    {
                        AssociationEnd specification = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEndRealization.Specification.Identity.ToString(), this) as MetaDataRepository.AssociationEnd;
                        if (specification == null)
                        {
                            specification = (AssociationEnd)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAssociationEndRealization.Specification, this);
                            specification.Name = originAssociationEndRealization.Specification.Name;
                        }
                        else if (_Specification == specification)
                        {
                            if (!_Specification.AssociationEndRealizations.Contains(this))
                                _Specification.AddAssociationEndRealization(this);
                        }
                        else
                        {
                            if (_Specification != null && _Specification.AssociationEndRealizations.Contains(this))
                                _Specification.RemoveAssociationEndRealization(this);

                            _Specification = specification;
                            _Specification.AddAssociationEndRealization(this);
                        }
                    }
                    else
                    {
                        if (_Specification != null && _Specification.AssociationEndRealizations.Contains(this))
                            _Specification.RemoveAssociationEndRealization(this);

                        _Specification = null;
                    }

                    //               if (_Specification==null)
                    //{
                    //	if(originAssociationEndRealization.Specification!=null)
                    //	{
                    //		//Error prone τι γινεται όταν τα meta data είναι transient
                    //		_Specification=MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEndRealization.Specification.Identity.ToString(),this) as MetaDataRepository.AssociationEnd;
                    //		if(_Specification==null)
                    //		{
                    //			_Specification=(AssociationEnd)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originAssociationEndRealization.Specification,this);
                    //			_Specification.Name=originAssociationEndRealization.Specification.Name;
                    //		}
                    //		_Specification.AddAssociationEndRealization(this);
                    //	}
                    //}
                    //               if (!_Specification.AssociationEndRealizations.Contains(this))
                    //                   _Specification.AddAssociationEndRealization(this);





                    stateTransition.Consistent = true;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }

        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1F976341-1310-4743-947F-D060B0564E59}</MetaDataID>
        protected bool _Persistent = false;
        /// <summary>Declare that the link between the objects will be persistent. </summary>
        /// <MetaDataID>{0987BDDF-3A36-4DB8-BA6C-D7935FD3245B}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [PersistentMember("_Persistent")]
        public bool Persistent
        {
            set
            {
            }
            get
            {
                return _Persistent;
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3076505E-16B4-4DA7-A27E-675C75EB39D5}</MetaDataID>
        protected bool _HasBehavioralSettings = false;
        /// <MetaDataID>{FAC9DEFB-9CC3-41B2-9581-4E426321A53E}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_HasBehavioralSettings")]
        public bool HasBehavioralSettings
        {
            get
            {
                return _HasBehavioralSettings;
            }
        }






        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7464DFCE-9C3F-4A35-9B13-ABDEDFFAA0F2}</MetaDataID>
        protected bool _CascadeDelete;
        /// <MetaDataID>{4171907D-E451-4A0B-8F53-49B7A2E6DA38}</MetaDataID>
        [BackwardCompatibilityID("+18")]
        [PersistentMember("_CascadeDelete")]
        public bool CascadeDelete
        {
            get
            {
                if (!_HasBehavioralSettings)
                    throw new System.NotSupportedException("It hasn't BehavioralSettings");
                return _CascadeDelete;
            }
            set
            {
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{B977A5D9-8CF7-4226-903F-01FB3379A002}</MetaDataID>
        protected bool _ReferentialIntegrity;
        /// <MetaDataID>{848CCC7C-B1C9-481E-B20C-C42B590111AD}</MetaDataID>
        [BackwardCompatibilityID("+19")]
        [PersistentMember("_ReferentialIntegrity")]
        public bool ReferentialIntegrity
        {
            get
            {
                if (!_HasBehavioralSettings)
                    throw new System.NotSupportedException("It hasn't BehavioralSettings");

                return _ReferentialIntegrity;
            }
            set
            {
            }
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{901172BB-9F34-4C1A-86AF-AA2E1945D8B2}</MetaDataID>
        protected bool _LazyFetching;
        /// <MetaDataID>{E4759F57-8E88-4B33-8374-6CEF9E25558C}</MetaDataID>
        [BackwardCompatibilityID("+20")]
        [PersistentMember("_LazyFetching")]
        public bool LazyFetching
        {
            get
            {
                if (!_HasBehavioralSettings)
                    throw new System.NotSupportedException("It hasn't BehavioralSettings");

                return _LazyFetching;
            }
            set
            {
            }
        }
        /// <MetaDataID>{41F8CCFB-BA04-438E-B762-095FFB24DAD4}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {

            return new System.Collections.Generic.List<object>();
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7B577B97-6263-4039-950A-7ED9191173EA}</MetaDataID>
        protected AssociationEnd _Specification;

        /// <MetaDataID>{EEDFB5F8-85D1-40D2-A012-948797DE9096}</MetaDataID>
        [Association("AssociationEndRealization", typeof(OOAdvantech.MetaDataRepository.AssociationEnd), Roles.RoleA, "{B48643D9-F660-492C-852D-E150BBE16B37}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Specification")]
        [RoleAMultiplicityRange(1, 1)]
        public AssociationEnd Specification
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Specification;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }

            }
            set
            {
                OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                try
                {
                    if (_Specification == value)
                        return;

                    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                    {
                        _Specification = value;
                        StateTransition.Consistent = true; ;
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
    }
}
