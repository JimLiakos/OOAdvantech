namespace OOAdvantech.MetaDataRepository
{
    using Transactions;
    /// <MetaDataID>{A7015C8C-13ED-4C85-AA1D-0F28BDE2AC56}</MetaDataID>
    /// <summary>A TemplateSignature specifies the set of formal template parameters for the associated templated element. The formal
    /// template parameters specify the elements that may be substituted in a binding of the template. </summary>
    public class TemplateSignature : MetaObject
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Template))
            {
                if (value == null)
                    _Template = default(OOAdvantech.MetaDataRepository.ITemplateable);
                else
                    _Template = (OOAdvantech.MetaDataRepository.ITemplateable)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_OwnedParameters))
            {
                if (value == null)
                    _OwnedParameters = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.TemplateParameter>);
                else
                    _OwnedParameters = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.TemplateParameter>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Template))
                return _Template;

            if (member.Name == nameof(_OwnedParameters))
                return _OwnedParameters;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{7a1635d5-8fb3-4999-96ba-0874c0482e69}</MetaDataID>
        protected TemplateSignature()
        {
        }
        /// <MetaDataID>{fb62995b-43cd-4cd3-aff4-39571d1d896b}</MetaDataID>
        public TemplateSignature(ITemplateable template)
        {
            _Template = template;
        }

        /// <MetaDataID>{7EBC1422-548B-48B3-8126-86B5C2BFC1CA}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C2059AB7-12DB-4CB7-85C0-E52EBB454009}</MetaDataID>
        private ITemplateable _Template;
        /// <summary>The element that owns this template </summary>
        /// <MetaDataID>{A582E622-CD56-481C-BE2A-180587ED3CE5}</MetaDataID>
        [Association("TamplateDefinition",typeof(OOAdvantech.MetaDataRepository.ITemplateable),Roles.RoleB,"{660B2569-F0CD-4CA2-8241-CFD1DDBB02F0}")]
        [PersistentMember("_Template")]
        [RoleBMultiplicityRange(1,1)]
        public ITemplateable Template
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _Template;
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
                    if (_Template != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _Template = value;
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
        private Collections.Generic.Set<TemplateParameter> _OwnedParameters = new OOAdvantech.Collections.Generic.Set<TemplateParameter>();
        /// <summary>The formal template parameters that are owzned by this template
        /// signature. </summary>
        /// <MetaDataID>{9436D677-209A-4037-8413-8068A47E08F8}</MetaDataID>
        [Association("SignatureOwnedParameter",typeof(OOAdvantech.MetaDataRepository.TemplateParameter),Roles.RoleA,true,"{FF1C5235-2B5C-421B-884C-8162CA6FB480}")]
        [PersistentMember("_OwnedParameters")]
        [RoleAMultiplicityRange(0)]
        public Collections.Generic.Set<TemplateParameter> OwnedParameters
        {

            get
            {

                OOAdvantech.Collections.Generic.Set<TemplateParameter> ownedParameters = _OwnedParameters.ToThreadSafeSet();
                return ownedParameters;
            }
        }

      
        /// <MetaDataID>{0773D251-873F-4814-AA14-8C26EAFB5A66}</MetaDataID>
        public void AddOwnedParameter(TemplateParameter parameter)
        {
            try
            {
                if (!_OwnedParameters.Contains(parameter))
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this,TransactionOption.Supported))
                    {
                        parameter.Position = (int)_OwnedParameters.Count;
                        parameter.Signature = this;
                        _OwnedParameters.Add(parameter);
                        stateTransition.Consistent = true;
                    }
                }
            }
            catch (System.Exception error)
            {
                throw;
            }
        }
        /// <MetaDataID>{f928aa18-4547-4a12-9f3f-7660e6c1e8ec}</MetaDataID>
        public void AddOwnedParameter(int index, TemplateParameter parameter)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
            {
                //SortOwnedParameters();

                //if (SortedOwnedParameters.Contains(parameter))
                //{
                //    SortedOwnedParameters.Remove(parameter);
                //    SortedOwnedParameters.Insert(index, parameter);
                //}
                //else
                //    SortedOwnedParameters.Insert(index, parameter);

                if (!_OwnedParameters.Contains(parameter))
                    _OwnedParameters.Insert(index, parameter);
                //for (short i = 0; i != SortedOwnedParameters.Count; i++)
                //{
                //    parameter = SortedOwnedParameters[i];
                //    if (parameter!= null)
                //        parameter.Position = i;
                //}
                stateTransition.Consistent = true;
            }



        }
        /// <MetaDataID>{8046CED7-8B25-4F46-AF71-5AAD48A15CC7}</MetaDataID>
        public void RemoveOwnedParameter(TemplateParameter parameter)
        {
            if (_OwnedParameters.Contains(parameter))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    _OwnedParameters.Remove(parameter);
                    stateTransition.Consistent = true;
                }
            }
        }


        /// <MetaDataID>{F60AA5E0-C39F-49CF-94FE-1A4425EC51FA}</MetaDataID>
        /// <summary>The complete set of formal template parameters for this template
        /// signature. </summary>
        [Association("ReferedParameter",typeof(OOAdvantech.MetaDataRepository.TemplateParameter),Roles.RoleA,"{D6C79333-F027-41C3-8F10-874AA1B2B8D3}")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        public Collections.Generic.Set<TemplateParameter> Parameters
        {
            get
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }



        /// <MetaDataID>{5DDB555D-CFB7-49C3-BD31-D129D59B7679}</MetaDataID>
        public TemplateParameter GetParameterWithName(string parameterName)
        {
            foreach (TemplateParameter parameter in _OwnedParameters)
            {
                if (parameter.Name == parameterName)
                    return parameter;
            }
            return null;
        }
    }
}
