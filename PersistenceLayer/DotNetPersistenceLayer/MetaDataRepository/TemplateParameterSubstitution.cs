namespace OOAdvantech.MetaDataRepository
{
    using Transactions;
    /// <MetaDataID>{925BFD54-8505-4903-A617-864A5ED7416A}</MetaDataID>
    /// <summary>A template parameter substitution relates the actual parameter(s) to a formal template parameter as part of a template
    /// binding. </summary>
    public class TemplateParameterSubstitution : MetaObject
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_TemplateBinding))
            {
                if (value == null)
                    _TemplateBinding = default(OOAdvantech.MetaDataRepository.TemplateBinding);
                else
                    _TemplateBinding = (OOAdvantech.MetaDataRepository.TemplateBinding)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Formal))
            {
                if (value == null)
                    _Formal = default(OOAdvantech.MetaDataRepository.TemplateParameter);
                else
                    _Formal = (OOAdvantech.MetaDataRepository.TemplateParameter)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ActualParameters))
            {
                if (value == null)
                    _ActualParameters = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.IParameterableElement>);
                else
                    _ActualParameters = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.IParameterableElement>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_TemplateBinding))
                return _TemplateBinding;

            if (member.Name == nameof(_Formal))
                return _Formal;

            if (member.Name == nameof(_ActualParameters))
                return _ActualParameters;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{f46a3a45-bfc6-44e7-9704-937e3171e5fd}</MetaDataID>
        TemplateParameterSubstitution()
        {
        }
        /// <MetaDataID>{f94fff12-b820-47f6-b4a7-b1a144ef32b6}</MetaDataID>
        public TemplateParameterSubstitution(TemplateBinding templateBinding, TemplateParameter templateParameter, IParameterableElement substitutionParameter)
        {
            _ActualParameters.Add(substitutionParameter);
            _Formal = templateParameter;
            _TemplateBinding = templateBinding;

        }
        /// <MetaDataID>{CFA63930-75FC-49E1-9FB8-E6CBE53F30FA}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{FF66CD3D-4C13-4DE7-B04D-471C55603571}</MetaDataID>
        private TemplateBinding _TemplateBinding;
        /// <summary>The template binding that owns this substitution. </summary>
        /// <MetaDataID>{6D01E0FB-9B64-4CD8-94B6-55C07DCF04C1}</MetaDataID>
        [Association("BindParameter",typeof(OOAdvantech.MetaDataRepository.TemplateBinding),Roles.RoleB,"{D3C758D9-AEA6-456A-98DD-6DDBF99FB75A}")]
        [PersistentMember("_TemplateBinding")]
        [RoleBMultiplicityRange(1,1)]
        public TemplateBinding TemplateBinding
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _TemplateBinding;
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
                    if (_TemplateBinding != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _TemplateBinding = value;
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
        /// <MetaDataID>{CEE29C7B-2713-4924-97A5-498A3A1C7CF3}</MetaDataID>
        private TemplateParameter _Formal;
        /// <summary>The formal template parameter that is associated with this substitution. </summary>
        /// <MetaDataID>{237E99E5-23A4-4905-8E58-7F0A267871C5}</MetaDataID>
        [Association("SubstitutiedParameter",typeof(OOAdvantech.MetaDataRepository.TemplateParameter),Roles.RoleA,"{78BA6DA7-70D9-446E-AC4D-FB8F78F5E386}")]
        [RoleAMultiplicityRange(1,1)]
        [RoleBMultiplicityRange(0)]
        public TemplateParameter Formal
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    //if (ActualParameters[0] is TemplateParameter)
                    //    return ActualParameters[0] as TemplateParameter;
                    return _Formal;
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
                    if (_Formal != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _Formal = value;
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
        /// <MetaDataID>{EEFF2D34-EDF3-4B62-845B-4029E9160608}</MetaDataID>
        private Collections.Generic.Set<IParameterableElement> _ActualParameters=new OOAdvantech.Collections.Generic.Set<IParameterableElement>();

        /// <summary>The elements that are the actual parameters for this substitution. </summary>
        /// <MetaDataID>{BC36EF0B-64F6-47F4-9EA3-806749F191F1}</MetaDataID>
        [Association("SubstitutionParameter",typeof(OOAdvantech.MetaDataRepository.IParameterableElement),Roles.RoleA,"{039A1B2C-4862-4F2D-9D39-EF9F5F494D66}")]
        [PersistentMember("_ActualParameters")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<IParameterableElement> ActualParameters
        {
            get
            {

                return _ActualParameters.ToThreadSafeSet();
            }
        }
    }
}
