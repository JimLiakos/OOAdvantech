namespace OOAdvantech.MetaDataRepository
{
    using Transactions;
    /// <MetaDataID>{694C055F-307C-4365-B908-D66C85FB1332}</MetaDataID>
    /// <summary>A template binding represents a relationship between a templateable element and a template. A template binding specifies
    /// the substitutions of actual parameters for the formal parameters of the template. </summary>
    public class TemplateBinding : Dependency
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_Signature))
            {
                if (value == null)
                    _Signature = default(OOAdvantech.MetaDataRepository.TemplateSignature);
                else
                    _Signature = (OOAdvantech.MetaDataRepository.TemplateSignature)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ParameterSubstitutions))
            {
                if (value == null)
                    _ParameterSubstitutions = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.TemplateParameterSubstitution>);
                else
                    _ParameterSubstitutions = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.TemplateParameterSubstitution>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_BoundElement))
            {
                if (value == null)
                    _BoundElement = default(OOAdvantech.MetaDataRepository.ITemplateable);
                else
                    _BoundElement = (OOAdvantech.MetaDataRepository.ITemplateable)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_Signature))
                return _Signature;

            if (member.Name == nameof(_ParameterSubstitutions))
                return _ParameterSubstitutions;

            if (member.Name == nameof(_BoundElement))
                return _BoundElement;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{6fd40f0c-abdb-44e5-9155-9be3ebabb694}</MetaDataID>
        TemplateBinding()
        {
        }
        /// <MetaDataID>{2676f2a0-9c15-4398-8835-941d07adb22f}</MetaDataID>
        public TemplateBinding(ITemplateable template, Collections.Generic.List<IParameterableElement> parameterSubstitutions)
        {
            if (template.TemplateBinding != null)
                _Signature = template.TemplateBinding.Signature;
            else
                _Signature = template.OwnedTemplateSignature;
            int parametersCount = _Signature.OwnedParameters.Count;
            if(parametersCount!=parameterSubstitutions.Count)
                throw new System.Exception("the number of Substitution parameter isn't equal with template ("+(template as MetaObject).FullName+") parameters");
            for (int i = 0; i < parametersCount; i++)
                _ParameterSubstitutions.Add(new TemplateParameterSubstitution(this, _Signature.OwnedParameters[i], parameterSubstitutions[i]));
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1B2D0317-B684-4BAD-ABC8-48B8FA620408}</MetaDataID>
        private TemplateSignature _Signature;
        /// <summary>The template signature for the template that is the target of the binding. </summary>
        /// <MetaDataID>{606477FB-B130-4D1C-ACDF-CD0360581B49}</MetaDataID>
        [Association("BindigTemplateSignature",typeof(OOAdvantech.MetaDataRepository.TemplateSignature),Roles.RoleA,"{192AFE31-DA73-4035-96CA-1D0290850918}")]
        [PersistentMember("_Signature")]
        [RoleAMultiplicityRange(1,1)]
        [RoleBMultiplicityRange(0)]
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
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
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

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{10CC2839-A99E-4592-96E8-4106080D39A9}</MetaDataID>
        private OOAdvantech.Collections.Generic.Set<TemplateParameterSubstitution> _ParameterSubstitutions=new OOAdvantech.Collections.Generic.Set<TemplateParameterSubstitution>();
        /// <summary>The parameter substitutions owned by this template binding. </summary>
        /// <MetaDataID>{8671F90B-31E2-4DCD-9F5D-4BDD1A3C2E41}</MetaDataID>
        [Association("BindParameter",typeof(OOAdvantech.MetaDataRepository.TemplateParameterSubstitution),Roles.RoleA,"{D3C758D9-AEA6-456A-98DD-6DDBF99FB75A}")]
        [PersistentMember("_ParameterSubstitutions")]
        [RoleAMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<TemplateParameterSubstitution> ParameterSubstitutions
        {


            get
            {

                return _ParameterSubstitutions.ToThreadSafeSet();
            }
        }
        /// <MetaDataID>{2693F809-BF0E-4F59-9F5E-19706B79E24C}</MetaDataID>
        public void AddParameterSubstitution(TemplateParameterSubstitution parameterSubstitution)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
            {
                if (!_ParameterSubstitutions.Contains(parameterSubstitution))
                    _ParameterSubstitutions.Add(parameterSubstitution);
                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{69954710-6FC2-4573-B1F5-52119D6FE837}</MetaDataID>
        public void RemoveParameterSubstitution(TemplateParameterSubstitution parameterSubstitution)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
            {
                if (_ParameterSubstitutions.Contains(parameterSubstitution))
                    _ParameterSubstitutions.Remove(parameterSubstitution);
                stateTransition.Consistent = true;
            }
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{28D665D3-11D7-499F-ACE9-7CE50BDEADA4}</MetaDataID>
        private ITemplateable _BoundElement;
        /// <summary>The element that is bound by this binding. </summary>
        /// <MetaDataID>{70454EDD-CC9C-4CCA-BFEF-65FDC1FD93CD}</MetaDataID>
        [Association("BindTamplate",typeof(OOAdvantech.MetaDataRepository.ITemplateable),Roles.RoleA,"{0C4399F4-36AF-45E4-AF09-41C6A7CA4EFB}")]
        [PersistentMember("_BoundElement")]
        [RoleAMultiplicityRange(1,1)]
        public ITemplateable BoundElement
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _BoundElement;

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
                    if (_BoundElement != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                        {
                            _BoundElement = value;
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


        /// <MetaDataID>{3390c5ce-dbbf-4638-8f90-8f110ce4ecb3}</MetaDataID>
        public IParameterableElement GetActualParameterFor(string templateParameterName)
        {
            foreach (TemplateParameterSubstitution parameterSubstitution in ParameterSubstitutions)
            {
                if (parameterSubstitution.Formal.Name == templateParameterName)
                    return parameterSubstitution.ActualParameters[0] ;
            }
            throw new System.Exception("There isn't ParameterSubstitutions for '" + templateParameterName + "'");

        }

        /// <MetaDataID>{66dd72c6-4f7a-4c96-a4f3-847efa4a3347}</MetaDataID>
        public TemplateParameter GetIntermediateParameterFor(TemplateParameter templateParameter)
        {
            foreach (TemplateParameterSubstitution parameterSubstitution in ParameterSubstitutions)
            {
                //TODO
                //public class Set<T> :System.Collections.Generic.ICollection<T> 
                //Σε αυτή την περίπτωση όταν μια operation με return Type T απο ICollection<T> μπορεί
                //να ζητησει ActualParameter απο την Set που έχει templateParameter T αλλά δεν είναι 
                //ίδια με την templateParameter T του ICollection interface

                if (parameterSubstitution.Formal.Name == templateParameter.Name)
                    return parameterSubstitution.ActualParameters[0] as TemplateParameter;
            }
            throw new System.Exception("There isn't ParameterSubstitutions for '" + templateParameter.Name + "'");
        }

        /// <MetaDataID>{339e1b14-1310-4c8a-a64d-229ea8310cf2}</MetaDataID>
        public IParameterableElement GetActualParameterFor(TemplateParameter templateParameter)
        {
            foreach (TemplateParameterSubstitution parameterSubstitution in ParameterSubstitutions)
            {
                //TODO
                //public class Set<T> :System.Collections.Generic.ICollection<T> 
                //Σε αυτή την περίπτωση όταν μια operation με return Type T απο ICollection<T> μπορεί
                //να ζητησει ActualParameter απο την Set που έχει templateParameter T αλλά δεν είναι 
                //ίδια με την templateParameter T του ICollection interface

                if (parameterSubstitution.Formal.Name == templateParameter.Name)
                    return parameterSubstitution.ActualParameters[0] ;
            }
            throw new System.Exception("There isn't ParameterSubstitutions for '" + templateParameter.Name + "'");
        }
    }
}
