namespace OOAdvantech.MetaDataRepository
{
    using Transactions;
    /// <MetaDataID>{B25B9FAD-670A-45A6-B974-AA514C11D0ED}</MetaDataID>
    /// <summary>TemplateParameter references a ParameterableElement that is exposed as a formal template parameter in the containing
    /// template. </summary>
    public class TemplateParameter : MetaObject, IParameterableElement
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Position))
            {
                if (value == null)
                    _Position = default(int);
                else
                    _Position = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Signature))
            {
                if (value == null)
                    _Signature = default(OOAdvantech.MetaDataRepository.TemplateSignature);
                else
                    _Signature = (OOAdvantech.MetaDataRepository.TemplateSignature)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Position))
                return _Position;

            if (member.Name == nameof(_Signature))
                return _Signature;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{a92fc7eb-53cb-404f-b0aa-049ab9b65e0d}</MetaDataID>
        protected TemplateParameter()
        {
        }

        /// <MetaDataID>{a2d94145-0253-4036-bd11-5dd70e2dcb83}</MetaDataID>
        public TemplateParameter(string name)
        {
            _Name = name;
        }
        /// <MetaDataID>{e333d294-a3ec-4023-ba54-f5d584c100c0}</MetaDataID>
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
         
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2C739F6E-65E2-4927-ACC3-8EB65A00EA36}</MetaDataID>
        private int _Position=0;
        /// <summary>Order of the parameter in the operation’s parameter list </summary>
        /// <MetaDataID>{8988E1EC-A91D-4A42-AE3B-51823B5F37BE}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [PersistentMember("_Position")]
        public int Position
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _Position;
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
                    if (_Position != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this,TransactionOption.Supported))
                        {
                            _Position = value;
                            stateTransition.Consistent = true;
                        }

                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }

            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{536B01B0-CF18-4D2C-AC29-4DEAE9F1534C}</MetaDataID>
        private TemplateSignature _Signature;
        /// <summary>The template signature that owns this template parameter. </summary>
        /// <MetaDataID>{D6472DA2-3DEA-4C97-9528-0318AA0A5616}</MetaDataID>
        [Association("SignatureOwnedParameter",typeof(OOAdvantech.MetaDataRepository.TemplateSignature),Roles.RoleB,"{FF1C5235-2B5C-421B-884C-8162CA6FB480}")]
        [PersistentMember("_Signature")]
        [RoleBMultiplicityRange(1,1)]
        public TemplateSignature Signature
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Signature;
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
                    if (_Signature != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this,TransactionOption.Supported))
                        {
                            _Signature = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }

        /// <MetaDataID>{C51FC442-A12C-463E-8540-9F26A1FD9CD3}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }
    }
}
