namespace OOAdvantech.MetaDataRepository
{
    using Transactions;


    /// <MetaDataID>{8C7BC8EA-FFC0-4391-88A6-241941D18354}</MetaDataID>
    /// <summary>An operation is a service that can be requested from an object to effect behavior. An
    /// operation has a signature, which describes the actual parameters that are possible
    /// (including possible return values).
    /// In the metamodel, an Operation is a BehavioralFeature that can be applied to the
    /// Instances of the Classifier that contains the Operation. </summary>
    [BackwardCompatibilityID("{8C7BC8EA-FFC0-4391-88A6-241941D18354}")]
    [Persistent()]
    public class Operation : BehavioralFeature
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_OverrideKind))
            {
                if (value == null)
                    _OverrideKind = default(OOAdvantech.MetaDataRepository.OverrideKind);
                else
                    _OverrideKind = (OOAdvantech.MetaDataRepository.OverrideKind)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ParameterizedReturnType))
            {
                if (value == null)
                    _ParameterizedReturnType = default(OOAdvantech.MetaDataRepository.TemplateParameter);
                else
                    _ParameterizedReturnType = (OOAdvantech.MetaDataRepository.TemplateParameter)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(Implementetions))
            {
                if (value == null)
                    Implementetions = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Method>);
                else
                    Implementetions = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Method>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_ReturnType))
            {
                if (value == null)
                    _ReturnType = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _ReturnType = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Parameters))
            {
                if (value == null)
                    _Parameters = default(OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Parameter>);
                else
                    _Parameters = (OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Parameter>)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(LastParameterPosition))
            {
                if (value == null)
                    LastParameterPosition = default(int);
                else
                    LastParameterPosition = (int)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_OverrideKind))
                return _OverrideKind;

            if (member.Name == nameof(_ParameterizedReturnType))
                return _ParameterizedReturnType;

            if (member.Name == nameof(Implementetions))
                return Implementetions;

            if (member.Name == nameof(_ReturnType))
                return _ReturnType;

            if (member.Name == nameof(_Parameters))
                return _Parameters;

            if (member.Name == nameof(LastParameterPosition))
                return LastParameterPosition;


            return base.GetMemberValue(token, member);
        }

        /// <exclude>Excluded</exclude>
        protected OverrideKind _OverrideKind;
        /// <MetaDataID>{e26ed4d3-33e0-4829-b84d-2c9546a26fe8}</MetaDataID>
        [PersistentMember("_OverrideKind")]
        [BackwardCompatibilityID("+22")]
        public virtual OverrideKind OverrideKind
        {
            get
            {
                return _OverrideKind;
            }
            set
            {
                _OverrideKind = value;
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{E133278C-A917-4F71-B6EC-45B3A4BF46FA}</MetaDataID>
        protected TemplateParameter _ParameterizedReturnType;
        /// <MetaDataID>{CC91E7D7-0E6F-45D7-A654-48A7CF3A07DD}</MetaDataID>
        [Association("ParameterizedReturnType", typeof(OOAdvantech.MetaDataRepository.TemplateParameter), Roles.RoleA, "{3B18135D-3C5F-425A-9E2E-A66C8C2C233B}")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0)]
        public virtual TemplateParameter ParameterizedReturnType
        {
            get
            {


                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _ParameterizedReturnType;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    if (_ParameterizedReturnType != value)
                    {

                        OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                        try
                        {

                            _ParameterizedReturnType = value;
                        }
                        finally
                        {
                            ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                        }
                    }
                    stateTransition.Consistent = true;
                }
            }
        }
        ///// <MetaDataID>{AEE28AC9-EC17-4551-8D89-A84E84413D63}</MetaDataID>
        //protected System.Collections.SortedList SortedParameters;
        /// <MetaDataID>{4042803F-02D3-4A56-9FF7-C49C3373A70F}</MetaDataID>
        [Association("OperationImplementation", typeof(OOAdvantech.MetaDataRepository.Method), Roles.RoleB, "{CAB0AA5F-C40F-436B-8965-652AF4D98C87}")]
        [RoleBMultiplicityRange(0)]
        public OOAdvantech.Collections.Generic.Set<Method> Implementetions = new OOAdvantech.Collections.Generic.Set<Method>();


        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        /// <MetaDataID>{6418E9C8-42F7-4514-8B15-CD44D1F3B08C}</MetaDataID>
        public override void Synchronize(MetaObject originMetaObject)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {

                if (MetaDataRepository.SynchronizerSession.IsSynchronized(this))
                    return;
                base.Synchronize(originMetaObject);

               
                MetaDataRepository.SynchronizerSession.MetaObjectUnderSynchronization(this);

                Operation originOperation = null;
                if (originMetaObject is MetaDataRepository.Method)
                {
                    originOperation = (originMetaObject as MetaDataRepository.Method).Specification;
                    _OverrideKind = (originMetaObject as MetaDataRepository.Method).OverrideKind;
                }
                else
                {
                    originOperation = originMetaObject as MetaDataRepository.Operation;
                    _OverrideKind = originOperation.OverrideKind;
                }

                ContainedItemsSynchronizer ParameterSynchronizer = MetaObjectsStack.CurrentMetaObjectCreator.BuildItemsSychronizer(originOperation.Parameters, _Parameters, this);
                ParameterSynchronizer.FindModifications();
                ParameterSynchronizer.ExecuteAddCommand();
                ParameterSynchronizer.ExecuteDeleteCommand();
                ParameterSynchronizer.Synchronize();

                if (originOperation.ReturnType != null)
                {
                    _ReturnType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originOperation.ReturnType, this) as MetaDataRepository.Classifier;

                    if (_ReturnType == null)
                    {
                        //TODO να τσεκαριστή με test case αν το return type θα έχει σωστό namespace και η διαδικασία συχρονισμού δέν θα προχωρίσει 
                        _ReturnType = (Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(originOperation.ReturnType, this);
                        if (_ReturnType.FullName != originOperation.ReturnType.FullName)
                        {
                            _ReturnType.Name = originOperation.ReturnType.Name;
                            _ReturnType.ShallowSynchronize(originOperation.ReturnType);
                        }
                    }
                }
                //    SortedParameters = null;

                //				if(_ReturnType!=null)
                //					_ReturnType.Synchronize(OriginOperation.ReturnType);
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
            //ReturnType=OriginOperation.ReturnType;
        }


        /// <MetaDataID>{6E8F29D0-4447-4EF5-A6A9-236E658A7BF5}</MetaDataID>
        public Parameter GetParameterAt(int Position)
        {
            ReaderWriterLock.AcquireReaderLock(10000);
            try
            {
                if (_Parameters.Count > Position)
                    return _Parameters[Position];
                else
                    return null;
                //if(SortedParameters==null)
                //{
                //    SortedParameters=new System.Collections.SortedList((int)_Parameters.Count);
                //    foreach(Parameter CurrParameter in _Parameters)
                //        SortedParameters.Add(CurrParameter.Position,CurrParameter); 
                //}
                //return (SortedParameters[Position]) as Parameter;
            }
            finally
            {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }

        /// <MetaDataID>{386B54B3-B454-4676-8E1A-4D1A812FD8D9}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Classifier _ReturnType;
        /// <summary>Designates the classifier whose instances is the return value of the operation. Must be a Class, Interface, or DataType. The actual type may be a descendant of the declared type or (for an Interface) a Class that realizes the declared type. </summary>
        /// <MetaDataID>{50920295-FAC2-4E11-8A03-BF04FB9ADC77}</MetaDataID>
        [Association("ReturnType", typeof(Classifier), Roles.RoleA, "{6174532F-191A-437A-8EDC-B73747C614B2}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_ReturnType")]
        [RoleAMultiplicityRange(0, 1)]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.MetaDataRepository.Classifier ReturnType
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {
                    return _ReturnType;
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

                    _ReturnType = value;
                }
                finally
                {
                    ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }
        }
        /// <MetaDataID>{DF86987D-DA54-47A8-A31F-620D6D2A3ABF}</MetaDataID>
        public Operation()
        {
            LastParameterPosition = -1;
        }
        /// <MetaDataID>{fc8018b7-912a-4a61-a7ef-a3855fd036bb}</MetaDataID>
        public static bool IsGeneric(Operation operation)
        {
            if (operation.ParameterizedReturnType != null)
                return true;
            foreach (MetaDataRepository.Parameter parameter in operation.Parameters)
            {
                if (parameter.ParameterizedType != null)
                    return true;
            }
            return false;
        }

        /// <MetaDataID>{56F36BA0-E228-477A-ABC7-8AB27559760D}</MetaDataID>
        public Operation(string name, Classifier returnType)
        {
            Name = name;
            _ReturnType = returnType;
            LastParameterPosition = -1;
        }


        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<Parameter> _Parameters = new OOAdvantech.Collections.Generic.Set<Parameter>();
        /// <summary>An ordered list of Parameters for the Operation. To call the Operation, the caller must supply a list of values compatible with the types of the
        /// Parameters. </summary>
        /// <MetaDataID>{D0EF77DE-EFE0-4BBB-9BF3-60187CBE58DD}</MetaDataID>
        [Association("OperationParameter", typeof(OOAdvantech.MetaDataRepository.Parameter), Roles.RoleA, true, "{10D160FA-46E4-4EA3-8A81-8C2EC6F128EF}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember("_Parameters")]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0, 1)]
        public virtual OOAdvantech.Collections.Generic.Set<Parameter> Parameters
        {
            get
            {
                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _Parameters.ToThreadSafeSet();//new OOAdvantech.Collections.Generic.Set<Parameter>(_Parameters.ToThreadSafeList());
                    //if(SortedParameters==null)
                    //{
                    //    SortedParameters=new System.Collections.SortedList((int)_Parameters.Count);
                    //    foreach(Parameter CurrParameter in _Parameters)
                    //        SortedParameters.Add(CurrParameter.Position,CurrParameter); 
                    //}
                    //MetaObjectCollection parameters=new MetaObjectCollection();
                    //foreach(System.Collections.DictionaryEntry CurrEntry in SortedParameters )
                    //    parameters.Add((Parameter)CurrEntry.Value);
                    //return parameters;
                }
                finally
                {
                    ReaderWriterLock.ReleaseReaderLock();
                }
            }
        }

        /// <MetaDataID>{15A32896-AC71-4FCA-B3CF-5AAF527917AC}</MetaDataID>
        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            return new System.Collections.Generic.List<object>();
        }


        /// <MetaDataID>{7D127CC7-3BBA-4A6F-B636-A715E1E72FCB}</MetaDataID>
        private int LastParameterPosition = -1;

        /// <summary>This methods deletes a parameter from an operation. </summary>
        /// <param name="theParameter">Parameter being deleted from the operation. </param>
        /// <MetaDataID>{7FFD45C3-3A76-4A4D-9873-A35CCB3A01EE}</MetaDataID>
        public virtual void DeleteParameter(Parameter theParameter)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                //foreach(Parameter currParameter in _Parameters)
                //{
                //    if(currParameter.Position>theParameter.Position)
                //        currParameter._Position--;
                //}
                //SortedParameters=null;

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                {
                    _Parameters.Remove(theParameter);
                    stateTransition.Consistent = true;
                }

            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }
        /// <summary>Set parameter data for the position of parameter position. </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterType"></param>
        /// <param name="initValue"></param>
        /// <param name="position">The position can be one of position of parameters which already exist 
        /// or the next position of last parameter. </param>
        /// <returns></returns>
        /// <MetaDataID>{B3F1D909-F118-4B27-8AED-BC1AB43AC8AB}</MetaDataID>
        public Parameter SetParameter(string parameterName, Classifier parameterType, string initValue, int position)
        {
            MetaDataRepository.Parameter parameter = GetParameterAt(position);
            if (parameter == null)
            {
                if (position != _Parameters.Count)
                    throw new System.Exception("There isn't parameter at position " + position.ToString());
                else
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                    {
                        parameter = AddParameter(parameterName, parameterType, initValue);
                        stateTransition.Consistent = true;
                    }

                }
            }
            else
            {
                parameter.Name = parameterName;
                parameter.Type = parameterType;
            }
            return parameter;
        }

        /// <summary>This method creates a new parameter and adds it at the end in operation. </summary>
        /// <param name="ParameterName">Name of the parameter being added to the operation </param>
        /// <param name="ParameterType">Type of parameter being added to the operation.
        /// If this name is not unique, you must use the qualified name (for example, 
        /// Namespace::Namespace(...)::supplier_name  CPlusplus Style or Namespace.Namespace(...).supplier_name CSharp Style) </param>
        /// <param name="InitValue">Initial value of the added parameter </param>
        /// <MetaDataID>{A763F913-E770-427D-BE5C-932D0BA8F5E5}</MetaDataID>
        public virtual Parameter AddParameter(string ParameterName, Classifier ParameterType, string InitValue)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    MetaDataRepository.Parameter NewParameter = new Parameter(ParameterName, ParameterType);
                    if (PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties) != null)
                        PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).CommitTransientObjectState(NewParameter);
                    _Parameters.Add(NewParameter);
                    //SortedParameters=null;
                    StateTransition.Consistent = true; ;
                    return NewParameter;
                }
            }
            finally
            {
                ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
            }
        }


        /// <MetaDataID>{c51f7290-fb97-49a5-a466-7f801bcfd571}</MetaDataID>
        public virtual void AddOperationImplementation(Method method)
        {
            if (!Implementetions.Contains(method))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    Implementetions.Add(method);
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <MetaDataID>{50f31f65-badc-442b-bc21-1d3a351cccb4}</MetaDataID>
        public virtual void RemoveOperationImplementation(Method method)
        {
            if (Implementetions.Contains(method))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    Implementetions.Add(method);
                    stateTransition.Consistent = true;
                }
            }
        }


        /// <summary>This method creates a new parameter and adds it  at specific position in operation. </summary>
        /// <param name="ParameterName">Name of the parameter being added to the operation </param>
        /// <param name="ParameterType">Type of parameter being added to the operation.
        /// If this name is not unique, you must use the qualified name (for example, 
        /// Namespace::Namespace(...)::supplier_name  CPlusplus Style or Namespace.Namespace(...).supplier_name CSharp Style) </param>
        /// <param name="InitValue">Initial value of the added parameter </param>
        /// <param name="AfterThatParameter">Specify the previously parameter of new parameter. If it is null then the new parameter will be first parameter. If you want to ad parameter at the end you can use the method "public Parameter AddParameter(string ParameterName, Classifier ParameterType, string InitValue)". </param>
        /// <MetaDataID>{73DFBF7A-A407-4A33-8CD5-0FE34EFC3B75}</MetaDataID>
        public virtual Parameter AddParameter(string ParameterName, Classifier ParameterType, string InitValue, Parameter afterThatParameter)
        {
            OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
            try
            {


                //if(afterThatParameter!=null)
                //{
                //    foreach(Parameter currParameter in _Parameters)
                //    {
                //        if(currParameter.Position>afterThatParameter.Position)
                //            currParameter._Position++;
                //    }
                //}
                //else
                //{
                //    foreach(Parameter currParameter in _Parameters)
                //        currParameter._Position++;
                //}
                //PersistenceLayer.PersistencyContext AppPersistencyContext=PersistenceLayer.PersistencyContext.CurrentPersistencyContext;
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
                {
                    MetaDataRepository.Parameter parameter = (MetaDataRepository.Parameter)PersistenceLayer.ObjectStorage.GetStorageOfObject(Properties).NewObject(typeof(MetaDataRepository.Parameter));
                    parameter.Name = ParameterName;
                    parameter.Type = ParameterType;
                    //if(afterThatParameter!=null)
                    //    NewParameter._Position=afterThatParameter.Position+1;
                    //else
                    //    NewParameter._Position=0;
                    parameter.Type = ParameterType;
                    _Parameters.Insert(_Parameters.IndexOf(afterThatParameter) + 1, parameter);
                    //SortedParameters=null;
                    return parameter;

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
