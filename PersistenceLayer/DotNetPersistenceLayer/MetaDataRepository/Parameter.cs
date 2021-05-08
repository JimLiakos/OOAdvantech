namespace OOAdvantech.MetaDataRepository
{
    using Transactions;
    /// <MetaDataID>{CC6D7440-2F80-49B2-A3C5-9190F97E691C}</MetaDataID>
    /// <summary>A parameter is an unbound variable that can be changed, passed, or returned. A parameter may include a name, type, and direction of communication. Parameters are used in the specification of operations, messages and events, templates. </summary>
    [BackwardCompatibilityID("{CC6D7440-2F80-49B2-A3C5-9190F97E691C}")]
    [Persistent()]
    public class Parameter : MetaObject
    {
        public override ObjectMemberGetSet SetMemberValue(object token, System.Reflection.MemberInfo member, object value)
        {
            if (member.Name == nameof(_ParameterizedType))
            {
                if (value == null)
                    _ParameterizedType = default(OOAdvantech.MetaDataRepository.TemplateParameter);
                else
                    _ParameterizedType = (OOAdvantech.MetaDataRepository.TemplateParameter)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Direction))
            {
                if (value == null)
                    _Direction = default(OOAdvantech.MetaDataRepository.Parameter.DirectionType);
                else
                    _Direction = (OOAdvantech.MetaDataRepository.Parameter.DirectionType)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }
            if (member.Name == nameof(_Type))
            {
                if (value == null)
                    _Type = default(OOAdvantech.MetaDataRepository.Classifier);
                else
                    _Type = (OOAdvantech.MetaDataRepository.Classifier)value;
                return ObjectMemberGetSet.MemberValueSetted;
            }

            return base.SetMemberValue(token, member, value);
        }

        public override object GetMemberValue(object token, System.Reflection.MemberInfo member)
        {

            if (member.Name == nameof(_ParameterizedType))
                return _ParameterizedType;

            if (member.Name == nameof(_Direction))
                return _Direction;

            if (member.Name == nameof(_Type))
                return _Type;


            return base.GetMemberValue(token, member);
        }

        /// <MetaDataID>{7086c58b-66e5-4e5b-acad-f6f4a0ad564e}</MetaDataID>
        public override MetaObjectID Identity
        {
            get
            {
                return base.Identity;
                if (Type == null)
                    return new MetaObjectID("void");
                else
                    return new MetaObjectID(Type.FullName);
            }
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{C3FF5155-D6A4-49C2-8A5C-11F67C8ED307}</MetaDataID>
        protected TemplateParameter _ParameterizedType;
        /// <MetaDataID>{0225BDEA-C5C0-4674-8124-6F2B21EF1F6B}</MetaDataID>
        [Association("Parameter ParameterizedType",typeof(OOAdvantech.MetaDataRepository.TemplateParameter),Roles.RoleA,"{6C2942E4-259C-4282-83D0-FAE94096F134}")]
        [RoleAMultiplicityRange(0,1)]
        [RoleBMultiplicityRange(0)]
        public virtual TemplateParameter ParameterizedType
        {
            get
            {

                ReaderWriterLock.AcquireReaderLock(10000);
                try
                {

                    return _ParameterizedType;
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
                    if (_ParameterizedType != value)
                    {

                        OOAdvantech.Synchronization.LockCookie lockCookie = ReaderWriterLock.UpgradeToWriterLock(10000);
                        try
                        {

                            _ParameterizedType = value;
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
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{A545AE24-0A44-4DDD-9C36-7373CEAE913F}</MetaDataID>
        private DirectionType _Direction = DirectionType.In;
		/// <MetaDataID>{9B87357A-5AE4-4B9B-9A5F-77A337F599BA}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember("_Direction")]
        public DirectionType Direction
		{
			get
			{

				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Direction;

				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			
			}
			set
			{

				OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
					if(value!=_Direction)
					{
                        using (Transactions.ObjectStateTransition stateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
						{
							_Direction=value;
							stateTransition.Consistent=true;
						}
					}
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}


        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{682CE2FE-87B1-4F16-BD16-0DA2D2C92420}</MetaDataID>
        //internal int _Position;
        ///// <summary>Order of the parameter in the operation’s parameter list </summary>
        ///// <MetaDataID>{64D409A5-CC1A-4F23-8732-12D6C228FE37}</MetaDataID>
        //[BackwardCompatibilityID("+3")]
        //[PersistentMember("_Position")]
        //public int Position
        //{
        //    get
        //    {
        //        ReaderWriterLock.AcquireReaderLock(10000);
        //        try
        //        {
        //            return _Position;
        //        }
        //        finally
        //        {
        //            ReaderWriterLock.ReleaseReaderLock();
        //        }
        //    }		
        //}
        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization. </summary>
        /// <MetaDataID>{A23F3D6F-DBCA-4E7D-A6DC-E34AE26F05C9}</MetaDataID>
		public override void Synchronize(MetaObject OriginMetaObject)
		{
			OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
			try
			{
				
				if(MetaDataRepository.SynchronizerSession.IsSynchronized(this))
					return;
                using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
				{

					base.Synchronize(OriginMetaObject);
					MetaDataRepository.SynchronizerSession.MetaObjectUnderSynchronization(this);

					Parameter OriginParameter=(Parameter)OriginMetaObject;

			
					if(_Type==null)
					{

                        if(OriginParameter.Type!=null)
                            _Type = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(OriginParameter.Type, this) as MetaDataRepository.Classifier;
                        if (_Type == null && OriginParameter.Type != null)
						{
							_Type=(Classifier)MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(OriginParameter.Type,this);
							_Type.ShallowSynchronize(OriginParameter.Type);
						}
					}
				//	Type.Synchronize(OriginParameter.Type);
					//_Position=OriginParameter.Position;
					StateTransition.Consistent=true;
				}
			}
			finally
			{
				ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}

	
	
		public enum DirectionType{In=0,Out,InOut}

        /// <MetaDataID>{2258B988-C009-4D5C-83C5-EF466878F7F9}</MetaDataID>
        public Parameter()
		{
			Direction=DirectionType.In;
			//_Position=0;
		}

        /// <MetaDataID>{866F0995-7CB0-4E57-8F9B-3BA0C35DEF95}</MetaDataID>
        public Parameter(string name, Classifier parameterType)//, int position)
		{
			_Type=parameterType;
			_Name=name;
			Direction=DirectionType.In;
			//_Position=position;
		}

        /// <MetaDataID>{5932dc11-cf7b-403f-a59a-539fa746b4c1}</MetaDataID>
        public Parameter(string name, TemplateParameter parameterType)//, int position)
        {
            _ParameterizedType= parameterType;
            _Name = name;
            Direction = DirectionType.In;
           // _Position = position;
        }


        /// <MetaDataID>{339B570B-E5FA-4EF0-B46D-6B9372AD8AC5}</MetaDataID>
        /// <exclude>Excluded</exclude>
        protected Classifier _Type;
        /// <summary>Designates a Classifier to which an argument value must conform. </summary>
        /// <MetaDataID>{27073B0A-43BD-434A-A56D-C053DA09AF2F}</MetaDataID>
        [Association("ParameterType", typeof(Classifier), Roles.RoleA, "{589C0490-F5C0-4784-982C-61740B0F5ABD}")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember("_Type")]
        [RoleAMultiplicityRange(0,1)]
        [RoleBMultiplicityRange(0)]
        public virtual OOAdvantech.MetaDataRepository.Classifier Type
		{
			get
			{
				ReaderWriterLock.AcquireReaderLock(10000);
				try
				{
					return _Type;
				}
				finally
				{
					ReaderWriterLock.ReleaseReaderLock();
				}
			}
			set
			{
				OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);
				try
				{
                    using (Transactions.ObjectStateTransition StateTransition = new OOAdvantech.Transactions.ObjectStateTransition(this, OOAdvantech.Transactions.TransactionOption.Supported))
					{
						_Type=value;
						StateTransition.Consistent=true;;
					}
				}
				finally
				{
					ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);
				}
			}
		}
        /// <MetaDataID>{5E0F18A4-BA83-4FAB-B681-8D59ED19AB86}</MetaDataID>
		public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
		{
			return new System.Collections.Generic.List<object>(); 
		}
	}
}
